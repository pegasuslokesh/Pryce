using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_TemporaryShift : BasePage
{
    Att_AttendanceLog objAttLog = null;
    EmployeeMaster objEmp = null;
    Att_ShiftManagement objShift = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    SystemParameter objSys = null;
    Att_ShiftDescription objShiftdesc = null;
    Common cmn = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_TimeTable objTimeTable = null;
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    PageControlCommon objPageCmn = null;
    private string _strConString = string.Empty;
    DataTable DtTime = new DataTable();
    //------------------------------

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        _strConString = Session["DBConnection"].ToString();
        objAttLog = new Att_AttendanceLog(_strConString);
        objEmp = new EmployeeMaster(_strConString);
        objShift = new Att_ShiftManagement(_strConString);
        objEmpGroup = new Set_EmployeeGroup_Master(_strConString);
        objGroupEmp = new Set_Group_Employee(_strConString);
        objSys = new SystemParameter(_strConString);
        objShiftdesc = new Att_ShiftDescription(_strConString);
        cmn = new Common(_strConString);
        objEmpSch = new Att_ScheduleMaster(_strConString);
        objTimeTable = new Att_TimeTable(_strConString);
        objempparam = new EmployeeParameter(_strConString);
        objRoleData = new RoleDataPermission(_strConString);
        objLocDept = new Set_Location_Department(_strConString);
        ObjLocationMaster = new LocationMaster(_strConString);
        objRole = new RoleMaster(_strConString);
        objPageCmn = new PageControlCommon(_strConString);
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/TemporaryShift.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["dtTempShiftEmpFiltered"] = null;
            objPageCmn.fillLocationWithAllOption(ddlLoc);
            objPageCmn.fillLocation(ddlLocation);
            //FillddlLocation();
            FillddlDeaprtment();
            FillDataListGrid(ddlLoc.SelectedValue);
            FillGrid();
            FillShift();
            rbtnEmp.Checked = true;
            rbtnGroup.Checked = false;
            EmpGroup_CheckedChanged(null, null);
            ddlFieldName.Focus();
        }

        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        txtFromDate_CalendarExtender.Format = objSys.SetDateFormat();
        txtTo_CalendarExtender.Format = objSys.SetDateFormat();
        txtToDate_CalendarExtender.Format = objSys.SetDateFormat();
        //AllPageCode();
    }
    #region Filter Criteria According to Location and Department



    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 26-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }
    private void FillddlDeaprtment()
    {
        
        PageControlCommon.GetLocationDepartment(ddlLocation, dpDepartment, Session["DBConnection"].ToString());

    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillGrid();
        PageControlCommon.GetLocationDepartment(ddlLocation, dpDepartment, Session["DBConnection"].ToString());

    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void FillGrid()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeFilteredRecord(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        Session["dtEmpLeave"] = dtEmp;
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvEmpList, dtEmp, "", "");
        lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

    }


    protected void GvEmpList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmpLeave"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";

            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtEmpLeave"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvEmpList, dt, "", "");
        //AllPageCode();
        GvEmpList.HeaderRow.Focus();
    }

    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        dpDepartment.SelectedValue = "--Select--";
        ddlLocation.SelectedValue = "--Select--";
        FillGrid();
        ddlLocation.Focus();
    }
    #endregion
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
        {
            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            Div_Group.Visible = true;
            Div_Employee.Visible = false;

            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 26-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }
            lbxGroup_SelectedIndexChanged(null, null);
            lbxGroup.Focus();
        }
        else if (rbtnEmp.Checked)
        {
            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }
        //AllPageCode();
    }
    // Modified By Nitin On 13/11/2014..................
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTimeTableName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetTimeTable(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "(" + dt.Rows[i][4].ToString() + "/" + dt.Rows[i][2].ToString() + "";
        }
        return str;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtTimeTable.Text == "")
        {
            DisplayMessage("Select Time Table");
            txtTimeTable.Focus();
            return;
        }
        DataTable dtTime1 = new DataTable();
        dtTime1.Columns.Add("EDutyTime");
        dtTime1.Columns.Add("TimeTableId");
        DataRow dr = dtTime1.NewRow();
        try
        {
            dr["EDutyTime"] = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 2];
            dr["TimeTableId"] = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 1];
        }
        catch (Exception Ex)
        {
        }
        dtTime1.Rows.Add(dr);
        if (Session["DtTime"] != null)
        {
            DataTable FilterDt = new DataTable();
            DtTime = (DataTable)Session["DtTime"];
            FilterDt = new DataView(DtTime, "TimeTableId='" + dr["TimeTableId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (FilterDt.Rows.Count > 0)
            {
                DisplayMessage("Time Table Already Added");
                txtTimeTable.Text = "";
                txtTimeTable.Focus();
                return;
            }

        }
        DtTime.Merge(dtTime1);
        txtTimeTable.Text = "";
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)chkTimeTableList, DtTime, "EDutyTime", "TimeTableId");
        Session["DtTime"] = DtTime;

    }
    protected void txtTimeTable_textChanged(object sender, EventArgs e)
    {
        string TimeTableid = string.Empty;
        DataTable dtTimeTable = new DataTable();
        dtTimeTable.Clear();
        if (txtTimeTable.Text != "")
        {
            TimeTableid = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 1];

            dtTimeTable = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
            try
            {
                dtTimeTable = new DataView(dtTimeTable, "TimeTable_Id='" + TimeTableid + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception Ex)
            {
                dtTimeTable.Clear();
            }
            if (dtTimeTable.Rows.Count > 0)
            {
                TimeTableid = dtTimeTable.Rows[0]["TimeTable_Id"].ToString();
                ViewState["TimeTable_Id"] = TimeTableid;
            }
            else
            {
                DisplayMessage("Time Table  Not Exists");
                txtTimeTable.Text = "";
                txtTimeTable.Focus();
                return;
            }
        }

    }

    protected void lbxGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroup.Items.Count; i++)
        {
            if (lbxGroup.Items[i].Selected == true)
            {
                GroupIds += lbxGroup.Items[i].Value + ",";

            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {
                    EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                }
            }


            if (EmpIds != "")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataTable();
            }
            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp1"] = dtEmp;
                //Common Function add By Lokesh on 26-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp1"] = null;
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();
            }
        }
        else
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();

        }
    }

    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
        //AllPageCode();
    }
    public void FillShift()
    {

        DataTable dtShift = objShift.GetShiftMaster(Session["CompId"].ToString());
        ddlShift.DataSource = dtShift;
        ddlShift.DataTextField = "Shift_Name";
        ddlShift.DataValueField = "Shift_Id";
        ddlShift.DataBind();
        ListItem lst = new ListItem("--Select--", "0");
        ddlShift.Items.Insert(0, lst);
    }


    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        txtFrom.Text = "";
        txtTo.Text = "";
        Session["dtTempShiftEmpFiltered"] = null;
        FillGrid();
        Div_Before_Next.Visible = true;
        Div_After_Next.Visible = false;
    }



    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        txtFrom.Text = "";
        txtTo.Text = "";
        Session["dtTempShiftEmpFiltered"] = null;
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;
        FillGrid();
        FillddlDeaprtment();
        ddlFieldName.Focus();
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;

        FillDataListGrid(ddlLoc.SelectedValue);
        //AllPageCode();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillDataListGrid(ddlLoc.SelectedValue);
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    protected void chkTimeTableList_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isoverlap = false;



        CheckBoxList list = (CheckBoxList)sender;
        string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
        int idx = control.Length - 1;
        string timetableid = string.Empty;
        try
        {
            timetableid = list.Items[Int32.Parse(control[idx])].Value;
        }
        catch (Exception ex)
        {
            return;
        }

        if (list.Items[Int32.Parse(control[idx])].Selected)
        {
            DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), timetableid);

            DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
            DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
            DateTime OnDutyTime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
            DateTime OffDutyTime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
            if (dtintime > dtouttime)
            {
                dtouttime = dtouttime.AddHours(24);
            }

            if (OnDutyTime > OffDutyTime)
            {
                OffDutyTime = OffDutyTime.AddHours(24);
            }

            for (int i = 0; i < chkTimeTableList.Items.Count; i++)
            {
                if (chkTimeTableList.Items[i].Selected && chkTimeTableList.Items[i].Value != timetableid)
                {
                    DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), chkTimeTableList.Items[i].Value);
                    DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                    DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                    DateTime OnDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                    DateTime OffDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);


                    if (dtintime1 > dtouttime1)
                    {
                        dtouttime1 = dtouttime1.AddHours(24);
                    }

                    if (OnDutyTime1 > OffDutyTime1)
                    {
                        OffDutyTime1 = OffDutyTime1.AddHours(24);
                    }

                    if (dtintime >= dtintime1 && dtintime <= dtouttime1)
                    {
                        isoverlap = true;
                        break;
                    }
                    if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
                    {
                        isoverlap = true;
                        break;
                    }

                    if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
                    {
                        isoverlap = true;
                        break;
                    }

                    if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
                    {
                        isoverlap = true;
                        break;
                    }
                }
            }
        }
        if (isoverlap)
        {
            list.Items[Int32.Parse(control[idx])].Selected = false;

            DisplayMessage("Time Overlaped");



        }


    }
    protected void chkUnselect_CheckedChanged(object sender, EventArgs e)
    {// GridViewRow gv=new GridViewRow();

        string empid = GvEmpListSelected.DataKeys[((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex][0].ToString();
        DataTable dtEmpFilter = (DataTable)Session["dtTempShiftEmpFiltered"];
        dtEmpFilter = new DataView(dtEmpFilter, "Emp_Code not ='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmpFilter, "", "");
        Session["dtTempShiftEmpFiltered"] = dtEmpFilter;
        ((CheckBox)GvEmpList.HeaderRow.FindControl("chkSelAll")).Checked = false;
        for (int i = 0; i < GvEmpList.Rows.Count; i++)
        {
            if (GvEmpList.DataKeys[i][0].ToString() == empid)
            {
                ((CheckBox)GvEmpList.Rows[i].FindControl("chkSelect")).Checked = false;
            }



        }
    }
    protected void btnShifttoEmpGroup_Click(object sender, EventArgs e)
    {
    }
    protected void chkUnselectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (((CheckBox)sender).Checked)
        {
            Session["dtTempShiftEmpFiltered"] = null;
            GvEmpListSelected.DataSource = null;
            GvEmpListSelected.DataBind();
        }
    }
    protected void btnNext1_Click(object sender, EventArgs e)
    {
        string EmpList = hdnFldSelectedValues.Value.Trim();

        if (rbtnGroup.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;

            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }

            }

            if (GroupIds == "")
            {
                DisplayMessage("Select Group First");
                return;

            }

            if (Session["dtEmp1"] != null)
            {
                DataTable dt = (DataTable)Session["dtEmp1"];
                Session["dtTempShiftEmpFiltered"] = dt;
                //Common Function add By Lokesh on 26-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");
            }
        }
        else
        {
            foreach (GridViewRow Row in GvEmpList.Rows)
            {
                CheckBox chkBox = (CheckBox)Row.FindControl("chkBxSelect");
                HiddenField hdnFld = (HiddenField)Row.FindControl("hdnFldId");
                if (chkBox.Checked == true)
                {
                    EmpList = hdnFld + ",";
                }
            }

            if (EmpList.ToString() == null || EmpList.ToString() == string.Empty)
            {

                DisplayMessage("Select Atleast One Employee");
                return;
            }
            else
            {
                FillGvEmpSelected();
            }
        }
        PnlTimeTableList.Visible = false;
        pnlAddDays.Visible = true;
        Div_Before_Next.Visible = false;
        Div_After_Next.Visible = true;
    }
    protected void GvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBxSelect = (CheckBox)e.Row.Cells[0].FindControl("chkBxSelect");
            CheckBox chkBxHeader = (CheckBox)this.GvEmpList.HeaderRow.FindControl("chkBxHeader");
            HiddenField hdnFldId = (HiddenField)e.Row.Cells[0].FindControl("hdnFldId");

            chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,document.getElementById('{0}'),'{1}');", chkBxHeader.ClientID, hdnFldId.Value.Trim());
        }
    }
    protected void btnRefresh3Report_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        DataTable dtEmp = (DataTable)Session["EmpDt"];
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp, "", "");
        Session["dtTempShiftEmpFiltered"] = dtEmp;

        ddlSelectOption0.SelectedIndex = 2;
        ddlSelectField0.SelectedIndex = 0;
        txtval0.Text = "";
    }
    public void FillGvEmpSelected()
    {
        DataTable dtEmpMain = (DataTable)Session["dtEmpLeave"];
        DataTable dtEmp = dtEmpMain.Clone();

        if (Session["dtTempShiftEmpFiltered"] != null)
        {
            dtEmp = (DataTable)Session["dtTempShiftEmpFiltered"];
        }
        //if (Session["dtTempShiftEmpFiltered"] != null)
        //{
        //    dtEmp = (DataTable)Session["dtTempShiftEmpFiltered"];
        //}
        //else
        //{

        // }

        for (int i = 0; i < GvEmpList.Rows.Count; i++)
        {
            if (((CheckBox)GvEmpList.Rows[i].FindControl("chkBxSelect")).Checked)
            {
                if (new DataView(dtEmp, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                {

                    dtEmp.Merge(new DataView(dtEmpMain, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
                }
            }
        }
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp, "", "");
        Session["dtTempShiftEmpFiltered"] = dtEmp;
        //AllPageCode();
    }
    protected void btnEmp0_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlSelectOption0.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlSelectOption0.SelectedIndex == 1)
            {
                condition = "convert(" + ddlSelectField0.SelectedValue + ",System.String)='" + txtval0.Text + "'";
            }
            else if (ddlSelectOption.SelectedIndex == 3)
            {
                condition = "convert(" + ddlSelectField0.SelectedValue + ",System.String) Like '" + txtval0.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlSelectField0.SelectedValue + ",System.String) like '%" + txtval0.Text + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtTempShiftEmpFiltered"];
            if (txtval0.Text != "")
            {
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);

                //Common Function add By Lokesh on 26-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, view.ToTable(), "", "");
                Session["dtFilter_Temp_Shift"] = view.ToTable();
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        FillGvEmpSelected();
    }
    protected void btnRefresh2Report_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlSelectOption.SelectedIndex = 2;
        ddlSelectField.SelectedIndex = 0;
        txtval.Text = "";
    }
    private void FillGvEmp()
    {
        DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(Session["LocId"].ToString(), "0", false, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),"0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());

        Session["EmpDt"] = dtEmp;
        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvEmpList, dtEmp, "", "");
        lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        GvEmpListSelected.DataSource = null;
        GvEmpListSelected.DataBind();
        //AllPageCode();
    }
    protected void btnEmp_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtval.Text.Trim() == "")
        {
            DisplayMessage("Please Fill Value");
            txtval.Focus();
            return;
        }
        if (ddlSelectOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlSelectOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlSelectField.SelectedValue + ",System.String)='" + txtval.Text + "'";
            }
            else if (ddlSelectOption.SelectedIndex == 3)
            {
                condition = "convert(" + ddlSelectField.SelectedValue + ",System.String) Like '" + txtval.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlSelectField.SelectedValue + ",System.String) like '%" + txtval.Text + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmpLeave"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);

            //Common Function add By Lokesh on 26-05-2015
            objPageCmn.FillData((object)GvEmpList, view.ToTable(), "", "");
            // Session["EmpDt"] = view.ToTable();
            lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        //AllPageCode();

    }
    protected void chkSelAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        for (int i = 0; i < GvEmpList.Rows.Count; i++)
        {
            ((CheckBox)GvEmpList.Rows[i].FindControl("chkSelect")).Checked = chk.Checked;
        }

        FillGvEmpSelected();
        btnrefresh2.Focus();
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        string PostedEmpList = string.Empty;
        string NonPostedLog = string.Empty;


        try
        {
            if (txtFrom.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFrom.Focus();
                return;
            }
            if (txtTo.Text == "")
            {
                DisplayMessage("To Date Required");
                txtTo.Focus();
                return;
            }

            if (objSys.getDateForInput(txtFrom.Text.ToString()) > objSys.getDateForInput(txtTo.Text.ToString()))
            {
                DisplayMessage("To Date should be Greater");




                return;
            }
        }
        catch (Exception)
        {

            DisplayMessage("Date Not In Proper Format");

            return;
        }

        if (rbtnEmp.Checked)
        {
            if (GvEmpListSelected.Rows.Count == 0)
            {

                DisplayMessage("Select Atleast One Employee");



                return;
            }

            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {
                int Month = objSys.getDateForInput(txtFrom.Text.ToString()).Month; ;
                int Year = Convert.ToDateTime(txtFrom.Text).Year;
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), Month.ToString(), Year.ToString());

                if (dtPostedList.Rows.Count > 0)
                {
                    PostedEmpList += GetEmployeeCode(dtPostedList.Rows[0]["Emp_Id"].ToString()) + ",";
                }
                else
                {
                    NonPostedLog += GvEmpListSelected.DataKeys[i][0].ToString() + ",";
                    objEmpSch.DeleteScheduleDescByEmpIdandDate(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString());

                }

            }

        }
        else
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;





            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }

            }



            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }




                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        int Month = objSys.getDateForInput(txtFrom.Text.ToString()).Month; ;
                        //    int Month = Convert.ToDateTime(txtFrom.Text).ToString();
                        int Year = Convert.ToDateTime(txtFrom.Text).Year;
                        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(str, Month.ToString(), Year.ToString());

                        if (dtPostedList.Rows.Count > 0)
                        {
                            PostedEmpList += GetEmployeeCode(str) + ",";
                        }
                        else
                        {
                            NonPostedLog += GetEmployeeCode(str) + ",";
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(str, objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString());

                        }

                    }
                }



            }
            else
            {
                DisplayMessage("Select Group First");
                return;
            }

        }
        if (PostedEmpList.ToString() != "" && NonPostedLog.ToString() != "")
        {
            DisplayMessage("Record Can Not Delete For Employee :- " + PostedEmpList.ToString().TrimEnd() + " Record Deleted For Employe :- " + NonPostedLog.ToString().TrimEnd() + "");
        }
        if (PostedEmpList.ToString() == "" && NonPostedLog.ToString().TrimEnd() != "")
        {
            DisplayMessage("Record Deleted");
        }
        if (PostedEmpList.ToString() != "" && NonPostedLog.ToString().TrimEnd() == "")
        {
            DisplayMessage("Record Can Not Delete For Employee :- " + PostedEmpList.ToString().TrimEnd() + "");
        }


    }
    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Code"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }

        }
        else
        {
            empname = "No Code";

        }

        return empname;



    }
    protected void btnIsTemprary_Click(object sender, EventArgs e)
    {
        chkOverWrite.Checked = false;
        chkTimeTableList.Items.Clear();
        Session["DtTime"] = null;
        if (GvEmpListSelected.Rows.Count == 0)
        {

            DisplayMessage("Select Atleast One Employee");

            return;
        }
        try
        {
            if (txtFrom.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFrom.Focus();
                return;
            }
            if (txtTo.Text == "")
            {
                DisplayMessage("To Date Required");
                txtTo.Focus();
                return;
            }

            if (objSys.getDateForInput(txtFrom.Text.ToString()) > objSys.getDateForInput(txtTo.Text.ToString()))
            {
                DisplayMessage("To Date should be Greater");




                return;
            }
        }
        catch (Exception)
        {

            DisplayMessage("Date Not In Proper Format");

            return;
        }
        Session["IsTemp"] = true;

        bindchecklist();

        pnlAddDays.Visible = true;
        PnlTimeTableList.Visible = true;

        Div_Last.Visible = true;
        Div_After_Next.Visible = false;

    }
    protected void btnCancelPanel_Click1(object sender, EventArgs e)
    {
        Session["IsTemp"] = null;
        pnlAddDays.Visible = false;
        PnlTimeTableList.Visible = false;
        Div_Last.Visible = false;
        Div_After_Next.Visible = true;
        btnCancel1_Click(null, null);
    }
    protected void btnsaveTempShift_Click(object sender, EventArgs e)
    {
        string confirmValue = string.Empty;
        string OverlapDate = string.Empty;
        int flag = 0;
        int flagc = 0;
        int b = 0;
        for (int i = 0; i < chkTimeTableList.Items.Count; i++)
        {

            if (chkTimeTableList.Items[i].Selected == true)
            {
                flag = 1;
                break;
            }

        }
        if (flag == 0)
        {
            DisplayMessage("Please Select Timetable");
            return;

        }
        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {

            if (chkDayUnderPeriod.Items[j].Selected == true)
            {
                flagc = 1;
                break;
            }

        }

        if (flagc == 0)
        {
            DisplayMessage("Please Select Day");
            return;

        }
        string[] weekdays = new string[8];
        weekdays[1] = "Sunday";
        weekdays[2] = "Monday";
        weekdays[3] = "Tuesday";
        weekdays[4] = "Wednesday";
        weekdays[5] = "Thursday";
        weekdays[6] = "Friday";
        weekdays[7] = "Saturday";



        if (rbtnEmp.Checked)
        {
            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {

                DataTable dtSch = objEmpSch.GetSheduleMaster();

                dtSch = new DataView(dtSch, "Shift_Type='Temp Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtSch.Rows.Count == 0)
                {
                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "Temp Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "Temp Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                }
                DataTable dtTime = new DataTable();
                for (int dayno = 0; dayno < chkDayUnderPeriod.Items.Count; dayno++)
                {
                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());

                    if (chkDayUnderPeriod.Items[dayno].Selected)
                    {
                        if (dtTime.Rows.Count > 0)
                        {
                            for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                            {

                                if (chkTimeTableList.Items[j].Selected)
                                {

                                    if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());

                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                    }
                                    else
                                    {
                                        DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", chkTimeTableList.Items[j].Value));
                                        DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", chkTimeTableList.Items[j].Value));
                                        int flag1 = 0;
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {

                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime) && chkOverWrite.Checked == false)
                                            {

                                                flag1 = 1;
                                                DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                OverlapDate += Date.ToString(objSys.SetDateFormat()) + ",";

                                            }
                                            else
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());
                                            }


                                        }


                                        if (flag1 == 0)
                                        {
                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                        }


                                    }


                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                            {
                                if (chkTimeTableList.Items[j].Selected)
                                {
                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;

            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }
            }
            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
                return;
            }

            foreach (string str in EmpIds.Split(','))
            {

                if (str != "")
                {

                    DataTable dtSch = objEmpSch.GetSheduleMaster();

                    dtSch = new DataView(dtSch, "Shift_Type='Temp Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + str + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtSch.Rows.Count == 0)
                    {
                        b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ddlShift.SelectedValue, "Temp Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                    }
                    else
                    {
                        b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ddlShift.SelectedValue, "Temp Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                    }
                    DataTable dtTime = new DataTable();
                    for (int dayno = 0; dayno < chkDayUnderPeriod.Items.Count; dayno++)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());

                        if (chkDayUnderPeriod.Items[dayno].Selected)
                        {
                            if (dtTime.Rows.Count > 0)
                            {
                                for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                                {

                                    if (chkTimeTableList.Items[j].Selected)
                                    {

                                        if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                        {
                                            objEmpSch.DeleteScheduleDescByEmpIdandDate(str, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());


                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), false.ToString(), true.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                        }
                                        else
                                        {
                                            DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", chkTimeTableList.Items[j].Value));
                                            DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", chkTimeTableList.Items[j].Value));
                                            int flag1 = 0;
                                            for (int s = 0; s < dtTime.Rows.Count; s++)
                                            {

                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime) && chkOverWrite.Checked == false)
                                                {
                                                    flag1 = 1;
                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                }
                                                else
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(str, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString());
                                                }

                                            }


                                            if (flag1 == 0)
                                            {
                                                objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, Convert.ToDateTime(chkDayUnderPeriod.Items[dayno].Value).ToString(), false.ToString(), true.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                            }


                                        }


                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                                {
                                    if (chkTimeTableList.Items[j].Selected)
                                    {

                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), str, "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }


                                }
                            }

                        }



                    }
                }

            }

        }

        if (OverlapDate == null || OverlapDate == string.Empty)
        {
            DisplayMessage("Record Saved", "green");
            btnCancelPanel_Click1(null, null);
        }
        else
        {
            DisplayMessage("Shift Overlapping : " + OverlapDate.TrimEnd(',').ToString() + "");
        }

        confirmValue = Request.Form["confirm_value"];
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string OverlapDate = string.Empty;
        int b = 0;
        bool IsTemp = false;



        try
        {
            if (txtFrom.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFrom.Focus();
                return;
            }
            if (txtTo.Text == "")
            {
                DisplayMessage("To Date Required");
                txtTo.Focus();
                return;
            }

            if (Convert.ToDateTime(txtFrom.Text.ToString()) > Convert.ToDateTime(txtTo.Text.ToString()))
            {
                DisplayMessage("To Date should be Greater");




                return;
            }
        }
        catch (Exception)
        {

            DisplayMessage("Date Not In Proper Format");

            return;
        }
        if (ddlShift.SelectedIndex == 0)
        {

            DisplayMessage("Please Select Shift");

            return;
        }



        string[] weekdays = new string[8];
        weekdays[1] = "Sunday";
        weekdays[2] = "Monday";
        weekdays[3] = "Tuesday";
        weekdays[4] = "Wednesday";
        weekdays[5] = "Thursday";
        weekdays[6] = "Friday";
        weekdays[7] = "Saturday";



        if (rbtnEmp.Checked)
        {

            if (GvEmpListSelected.Rows.Count == 0)
            {

                DisplayMessage("Select Atleast One Employee");



                return;
            }


            //start

            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {

                DataTable dtSch = objEmpSch.GetSheduleMaster();

                dtSch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtSch.Rows.Count == 0)
                {
                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "Normal Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "Normal Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                }

                if (b != 0)
                {
                    DateTime DtFromDate = Convert.ToDateTime(txtFrom.Text.ToString());
                    DateTime DtToDate = Convert.ToDateTime(txtTo.Text.ToString());
                    int days = DtToDate.Subtract(DtFromDate).Days + 1;
                    if (IsTemp == false)
                    {

                        int counter = 0;
                        // From Date to To Date
                        DataTable dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), ddlShift.SelectedValue);


                        DataTable dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(ddlShift.SelectedValue);

                        if (dtShiftD.Rows.Count == 0)
                        {
                            DisplayMessage("Shift Not Defined");
                            return;

                        }

                        dtShiftD = new DataView(dtShiftD, "", "", DataViewRowState.CurrentRows).ToTable();


                        DataTable dtTime = new DataTable();
                        DateTime dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());
                        DataTable dtTempShift = new DataTable();
                        int TotalDays = 1;
                        int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
                        int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

                        string cycletype = string.Empty;
                        string cycleday = string.Empty;
                        DateTime ApplyFromDate = new DateTime();
                        if (index == 7)
                        {

                            bool IsweekOff = false;
                            int daysShift = cycle * index;
                            string weekday = DtFromDate.DayOfWeek.ToString();


                            int state = 0;
                            int k = GetCycleDay(weekday);
                            int j = 1;
                            int a = k;
                            int f = 0;
                            while (DtFromDate <= DtToDate)
                            {
                                if (k % 7 == 0)
                                {
                                    if (f != 0)
                                    {
                                        j++;
                                        if (j > cycle)
                                        {
                                            j = 1;

                                        }
                                    }
                                }

                                if (k <= daysShift)
                                {

                                    if (k > 7)
                                    {
                                        f = 1;

                                    }
                                    a = GetCycleDay(DtFromDate.DayOfWeek.ToString());


                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    if (dtGetTemp1.Rows.Count > 0)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                        if (dtTime.Rows.Count > 0)
                                        {

                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                            {
                                                                flag1 = 1;
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                            {
                                                                flag1 = 1;
                                                                DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }

                                            }

                                        }
                                    }
                                    else
                                    {

                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                    }




                                    k++;
                                }
                                else
                                {
                                    k = 1;
                                    j = 1;
                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                    if (dtGetTemp1.Rows.Count > 0)
                                    {
                                        if (dtTime.Rows.Count > 0)
                                        {

                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {

                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                }
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                            {
                                                                flag1 = 1;
                                                                DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }

                                            }

                                        }
                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                    }

                                }



                                DtFromDate = DtFromDate.AddDays(1);
                            }

                        }
                        else if (index == 31)
                        {
                            int daysShift = cycle * index;

                            int k = DtFromDate.Day;
                            int a = 0;
                            int j = 1;
                            int mon = DtFromDate.Month;
                            while (DtFromDate <= DtToDate)
                            {
                                if (k <= daysShift)
                                {
                                    a = DtFromDate.Day;


                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    //
                                    if (dtGetTemp1.Rows.Count > 0)
                                    {


                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                            {

                                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                                if (dts.Rows.Count == 0)
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                }
                                            }
                                            else
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                            {
                                                                flag1 = 1;
                                                                DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {

                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {



                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());





                                                    }


                                                }
                                            }

                                        }



                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                    }


                                }

                                k++;
                                if (k > daysShift)
                                {

                                    k = 1;
                                    j = 1;
                                }


                                DtFromDate = DtFromDate.AddDays(1);
                                if (DtFromDate.Day == 1)
                                {

                                    j++;

                                }
                            }

                        }
                        else if (index == 1)
                        {
                            int k = 1;
                            int a = k;
                            int daysShift = cycle * index;
                            while (DtFromDate <= DtToDate)
                            {
                                if (k <= daysShift)
                                {
                                    a = k;


                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    //
                                    if (dtGetTemp1.Rows.Count > 0)
                                    {


                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                            {

                                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                                if (dts.Rows.Count == 0)
                                                {
                                                    objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                }
                                            }
                                            else
                                            {
                                                DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                        {
                                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                            {
                                                                flag1 = 1;
                                                                DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                            }
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }


                                                }
                                                else
                                                {

                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {



                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());





                                                    }


                                                }
                                            }

                                        }



                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                    }


                                }

                                k++;
                                if (k > daysShift)
                                {

                                    k = 1;

                                }


                                DtFromDate = DtFromDate.AddDays(1);

                            }




                        }


                    }
                    else
                    {//temporary


                    }
                }

            }
            //end
        }
        else
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;





            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }

            }



            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }

            }
            else
            {
                DisplayMessage("Select Group First");
                return;
            }








            foreach (string str in EmpIds.Split(','))
            {

                if (str != "")
                {
                    DataTable dtSch = objEmpSch.GetSheduleMaster();

                    dtSch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + str + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtSch.Rows.Count == 0)
                    {
                        b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ddlShift.SelectedValue, "Normal Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                    }
                    else
                    {
                        b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ddlShift.SelectedValue, "Normal Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                    }

                    if (b != 0)
                    {
                        DateTime DtFromDate = Convert.ToDateTime(txtFrom.Text.ToString());
                        DateTime DtToDate = Convert.ToDateTime(txtTo.Text.ToString());
                        int days = DtToDate.Subtract(DtFromDate).Days + 1;
                        if (IsTemp == false)
                        {

                            int counter = 0;
                            // From Date to To Date
                            DataTable dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), ddlShift.SelectedValue);


                            DataTable dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(ddlShift.SelectedValue);

                            if (dtShiftD.Rows.Count == 0)
                            {
                                DisplayMessage("Shift Not Defined");
                                return;

                            }

                            dtShiftD = new DataView(dtShiftD, "", "", DataViewRowState.CurrentRows).ToTable();


                            DataTable dtTime = new DataTable();
                            DateTime dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());
                            DataTable dtTempShift = new DataTable();
                            int TotalDays = 1;
                            int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
                            int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

                            string cycletype = string.Empty;
                            string cycleday = string.Empty;
                            DateTime ApplyFromDate = new DateTime();
                            if (index == 7)
                            {

                                bool IsweekOff = false;
                                int daysShift = cycle * index;
                                string weekday = DtFromDate.DayOfWeek.ToString();


                                int state = 0;
                                int k = GetCycleDay(weekday);
                                int j = 1;
                                int a = k;
                                int f = 0;
                                while (DtFromDate <= DtToDate)
                                {
                                    if (k % 7 == 0)
                                    {
                                        if (f != 0)
                                        {
                                            j++;
                                            if (j > cycle)
                                            {
                                                j = 1;

                                            }
                                        }
                                    }

                                    if (k <= daysShift)
                                    {

                                        if (k > 7)
                                        {
                                            f = 1;

                                        }
                                        a = GetCycleDay(DtFromDate.DayOfWeek.ToString());


                                        DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                        if (dtGetTemp1.Rows.Count > 0)
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                            if (dtTime.Rows.Count > 0)
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }

                                                }
                                            }
                                            else
                                            {
                                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }

                                                }

                                            }
                                        }
                                        else
                                        {

                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }




                                        k++;
                                    }
                                    else
                                    {
                                        k = 1;
                                        j = 1;
                                        DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                        if (dtGetTemp1.Rows.Count > 0)
                                        {
                                            if (dtTime.Rows.Count > 0)
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }

                                                }
                                            }
                                            else
                                            {
                                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";

                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                    }

                                                }

                                            }
                                        }
                                        else
                                        {
                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }

                                    }



                                    DtFromDate = DtFromDate.AddDays(1);
                                }

                            }
                            else if (index == 31)
                            {
                                int daysShift = cycle * index;

                                int k = DtFromDate.Day;
                                int a = 0;
                                int j = 1;
                                int mon = DtFromDate.Month;
                                while (DtFromDate <= DtToDate)
                                {
                                    if (k <= daysShift)
                                    {
                                        a = DtFromDate.Day;


                                        DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                        //
                                        if (dtGetTemp1.Rows.Count > 0)
                                        {


                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                                {

                                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                    if (dts.Rows.Count == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                                {
                                                                    if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                    {
                                                                        flag1 = 1;
                                                                        DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                        OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {

                                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                        {



                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());





                                                        }


                                                    }
                                                }

                                            }



                                        }
                                        else
                                        {
                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }


                                    }

                                    k++;
                                    if (k > daysShift)
                                    {

                                        k = 1;
                                        j = 1;
                                    }


                                    DtFromDate = DtFromDate.AddDays(1);
                                    if (DtFromDate.Day == 1)
                                    {

                                        j++;

                                    }
                                }

                            }
                            else if (index == 1)
                            {
                                int k = 1;
                                int a = k;
                                int daysShift = cycle * index;
                                while (DtFromDate <= DtToDate)
                                {
                                    if (k <= daysShift)
                                    {
                                        a = k;


                                        DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                        //
                                        if (dtGetTemp1.Rows.Count > 0)
                                        {


                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                                {

                                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                                    if (dts.Rows.Count == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                    int flag1 = 0;

                                                    if (dtTime.Rows.Count > 0)
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {
                                                            if (!Convert.ToBoolean(dtTime.Rows[s]["Is_Off"].ToString()))
                                                            {
                                                                if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                                {
                                                                    flag1 = 1;
                                                                    DateTime Date = Convert.ToDateTime(dtTime.Rows[s]["Att_Date"].ToString());
                                                                    OverlapDate += Date.ToString("MM/dd/yyyy") + ",";
                                                                }
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                        }


                                                    }
                                                    else
                                                    {

                                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                        {



                                                            objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());





                                                        }


                                                    }
                                                }

                                            }



                                        }
                                        else
                                        {
                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(str, DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                objEmpSch.InsertScheduleDescriptionForTempShift(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), str, ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }


                                    }

                                    k++;
                                    if (k > daysShift)
                                    {

                                        k = 1;

                                    }


                                    DtFromDate = DtFromDate.AddDays(1);

                                }




                            }


                        }
                        else
                        {//temporary


                        }
                    }
                }
            }
        }

        if (OverlapDate == null && OverlapDate == string.Empty)
        {
            DisplayMessage("Record Saved", "green");
        }
        else
        {
            DisplayMessage("Shift Overlapping : " + OverlapDate.TrimEnd(',').ToString() + "");
        }


    }
    protected void lnkBackToList_Click(object sender, EventArgs e)
    {

        GvShiftReport.DataSource = null;
        GvShiftReport.DataBind();
        Session["IsTemp"] = null;
        Lbl_Tab_New.Text = "New";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }
    private string GetDutyTime(string OnOff, string timetableid)
    {
        string retval = "";
        DataTable dtTimeTableId = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), timetableid);
        if (OnOff == "On")
        {
            retval = dtTimeTableId.Rows[0]["OnDuty_Time"].ToString();
        }
        else
        {
            retval = dtTimeTableId.Rows[0]["OffDuty_Time"].ToString();
        }
        return retval;

    }

    public string GetEmpId(string empcode)
    {

        string empId = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Code='" + empcode.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empId = dt.Rows[0]["Emp_Id"].ToString();



        }


        return empId;



    }

    public bool ISOverLapTimeTable(DateTime dtintime1, DateTime dtouttime1, DateTime dtintime, DateTime dtouttime)
    {
        dtintime1 = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Day, dtintime1.Hour, dtintime1.Minute, dtintime1.Second);
        dtouttime1 = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Day, dtouttime1.Hour, dtouttime1.Minute, dtouttime1.Second);

        bool isoverlap = false;
        if (dtintime >= dtintime1 && dtintime <= dtouttime1)
        {
            isoverlap = true;

        }
        else if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
        {
            isoverlap = true;

        }

        else if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
        {
            isoverlap = true;

        }

        else if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
        {
            isoverlap = true;

        }
        else if (dtintime1 == dtintime && dtouttime1 == dtouttime)
        {
            isoverlap = true;

        }
        return isoverlap;
    }

    public int GetCycleDay(string day)
    {
        string cycleday = string.Empty;
        string[] weekdays = new string[8];
        weekdays[1] = "Sunday";
        weekdays[2] = "Monday";
        weekdays[3] = "Tuesday";
        weekdays[4] = "Wednesday";
        weekdays[5] = "Thursday";
        weekdays[6] = "Friday";
        weekdays[7] = "Saturday";

        for (int i = 1; i <= 7; i++)
        {
            if (weekdays[i] == day)
            {
                cycleday = i.ToString();

            }
        }

        return int.Parse(cycleday);

    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        bool chkStatus = false;


        if (((Button)sender).ID == "btnSelectAll")
        {
            chkStatus = true;
        }


        for (int t = 0; t < chkTimeTableList.Items.Count; t++)
        {
            chkTimeTableList.Items[t].Selected = chkStatus;
        }
        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = chkStatus;
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        PnlTimeTableList.Visible = true;
        pnlAddDays.Visible = false;

    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnback_Click(object sender, EventArgs e)
    {

    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (txtValue.Text.Trim() == "")
        {
            DisplayMessage("Please Fill Value");
            txtValue.Focus();
            return;
        }
        DataTable dtproduct = Common.GetEmployeeListbyLocationIdandDepartmentValue("0", false, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());



        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            DataView view = new DataView(dtproduct, condition, "", DataViewRowState.CurrentRows);

            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";

            Session["dtEmp"] = view.ToTable();

            dtproduct = view.ToTable();
            if (dtproduct.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 26-05-2015
                objPageCmn.FillData((object)gvEmp, dtproduct, "", "");
            }
        }
        //AllPageCode();
        txtValue.Focus();
    }

    private void FillDataListGrid(string locationID)
    {

        DataTable dtproduct = Common.GetEmployeeListbyLocationIdandDepartmentValue(locationID, "0", true, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());

        Session["dtEmp"] = dtproduct;

        if (dtproduct.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 26-05-2015
            objPageCmn.FillData((object)gvEmp, dtproduct, "", "");
        }
        else
        {
            gvEmp.DataSource = null;
            gvEmp.DataBind();
        }

        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();

        //AllPageCode();

    }

    protected void gvEmp_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmp"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";

            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtEmp"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        //AllPageCode();
        gvEmp.HeaderRow.Focus();
    }




    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Select From Date And To Date');", true);
            return;
        }

        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("From date is invalid");
                return;
            }
        }

        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("To date is invalid");
                return;
            }
        }

        if (objSys.getDateForInput(txtFromDate.Text.ToString()) > objSys.getDateForInput(txtToDate.Text.ToString()))
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('To Date should be greater');", true);

            return;
        }
        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(editid.Value);
        dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFromDate.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtToDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvShiftReport, dtShiftAllDate, "", "");
    }
    public void bindchecklist()
    {//
        //DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
        //chkTimeTableList.DataSource = dt;
        //chkTimeTableList.DataTextField = "TimeTable_Name1";
        //chkTimeTableList.DataValueField = "TimeTable_Id";
        //chkTimeTableList.DataBind();


        //bind dayunderperiod checkboxlist

        DateTime dtfrom = objSys.getDateForInput(txtFrom.Text);
        DateTime dtto = objSys.getDateForInput(txtTo.Text);


        DataTable dtfordays = new DataTable();
        dtfordays.Columns.Add("days");
        dtfordays.Columns.Add("dayno");
        DateTime TempDate = dtfrom;
        int totaldays = dtto.Subtract(dtfrom).Days + 1;
        for (int i = 1; i <= totaldays; i++)
        {

            dtfordays.Rows.Add(dtfordays.NewRow());
            dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = TempDate.ToString("MMM") + ":" + TempDate.Day + ":" + TempDate.DayOfWeek.ToString();
            dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = TempDate.ToString();
            TempDate = TempDate.AddDays(1);
        }

        //Common Function add By Lokesh on 26-05-2015
        chkDayUnderPeriod.DataSource = dtfordays;
        chkDayUnderPeriod.DataTextField = "days";
        chkDayUnderPeriod.DataValueField = "dayno";
        chkDayUnderPeriod.DataBind();
        //objPageCmn.FillData((object)chkDayUnderPeriod, dtfordays, "days", "dayno");
        //for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        //{
        //    chkDayUnderPeriod.Items[j].Selected = true;
        //}
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Button1.Visible = clsPagePermission.bAdd;
        gvEmp.Columns[0].Visible = clsPagePermission.bEdit;
    }
    protected void GvShiftreport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            String s = GvShiftReport.DataKeys[e.Row.RowIndex]["OnDuty_Time"].ToString();
            if (s == "")
            {
                return;

            }
            ((Label)e.Row.FindControl("lblOnDuty")).Text = Convert.ToDateTime(s).ToString("HH:mm");
            String s1 = GvShiftReport.DataKeys[e.Row.RowIndex]["OffDuty_Time"].ToString();
            ((Label)e.Row.FindControl("lblOffDuty")).Text = Convert.ToDateTime(s1).ToString("HH:mm");
            String empid = GvShiftReport.DataKeys[e.Row.RowIndex]["Emp_Id"].ToString();

            string date = GvShiftReport.DataKeys[e.Row.RowIndex]["Att_Date"].ToString();

            Set_Employee_Holiday objempholi = new Set_Employee_Holiday(Session["DBConnection"].ToString());

            bool b = objempholi.GetEmployeeHolidayOnDateAndEmpId(date, empid);

            if (b)
            {
                ((CheckBox)e.Row.FindControl("chkHoliday")).Checked = true;
            }
            else
            {
                ((CheckBox)e.Row.FindControl("chkHoliday")).Checked = false;
            }
        }
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Common objcom = new Common(Session["DBConnection"].ToString());
        editid.Value = e.CommandArgument.ToString();
        lblempname.Text = Common.GetEmployeeName(editid.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
        objPageCmn.FillData((object)GvShiftReport, null, "", "");
        btnNew_Click(null, null);

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                return;
            }

            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                return;
            }
        }


        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(editid.Value);
        dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFromDate.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtToDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 26-05-2015
        objPageCmn.FillData((object)GvShiftReport, dtShiftAllDate, "", "");
        //Lbl_Tab_New.Text = "View";

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_View_Show()", true);
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {


        FillGvEmp();
        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        Session["dtTempShiftEmpFiltered"] = null;
        EmpGroup_CheckedChanged(null, null);
        btnCancel2.Visible = true;
        txtFrom.Text = "";
        txtTo.Text = "";
        //AllPageCode();
        btnNext.Visible = true;

        dpDepartment.SelectedValue = "--Select--";
        ddlLocation.Focus();

    }

    protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillDataListGrid(ddlLoc.SelectedValue);
    }
}
