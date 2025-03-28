using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_HolidayMaster : BasePage
{
    Att_AttendanceLog objAttLog = null;
    Common cmn = null;
    SystemParameter objSys = null;
    HolidayMaster objHoliday = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    EmployeeMaster objEmp = null;
    Set_Group_Employee objGroupEmp = null;
    Set_Holiday_Group objHolidayGroup = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Leave_Request objleaveReq = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_PartialLeave_Request objPartial = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    PegasusDataAccess.DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    string strPageLevel = string.Empty;
    Common ObjComman = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objHoliday = new HolidayMaster(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objHolidayGroup = new Set_Holiday_Group(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDa = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/HolidayMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            FillddlLocation();
            FillddlLocation(0);
            FillGrid();
            Session["CHECKED_ITEMS"] = null;
            ViewState["Checked_Item"] = null;
            pnlHolidayGroup.Visible = false;

        }
        CalendarExtender1.Format = objSys.SetDateFormat();
        CalendarExtender2.Format = objSys.SetDateFormat();
        //AllPageCode();
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
    }

    protected void txtHolidayName_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objHoliday.GetHolidayMasterByHolidayName(Session["CompId"].ToString().ToString(), txtHolidayName.Text.Trim(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dt.Rows.Count > 0)
            {
                txtHolidayName.Text = "";
                DisplayMessage("Holiday Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHolidayName);
                return;
            }
            DataTable dt1 = objHoliday.GetHolidayMasterInactive(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt1 = new DataView(dt1, "Holiday_Name='" + txtHolidayName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtHolidayName.Text = "";
                DisplayMessage("Holiday Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHolidayName);
                return;
            }
            txtHolidayNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objHoliday.GetHolidayMasterById(Session["CompId"].ToString().ToString(), editid.Value, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Holiday_Name"].ToString() != txtHolidayName.Text)
                {
                    DataTable dt = objHoliday.GetHolidayMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    dt = new DataView(dt, "Holiday_Name='" + txtHolidayName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtHolidayName.Text = "";
                        DisplayMessage("Holiday Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHolidayName);
                        return;
                    }
                    DataTable dt1 = objHoliday.GetHolidayMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    dt1 = new DataView(dt1, "Holiday_Name='" + txtHolidayName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtHolidayName.Text = "";
                        DisplayMessage("Holiday Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHolidayName);
                        return;
                    }
                }
            }
            txtHolidayNameL.Focus();
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
        //AllPageCode();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedValue.ToString() == "To_Date" || ddlFieldName.SelectedValue.ToString() == "From_Date")
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + TxtValueDate.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + TxtValueDate.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + TxtValueDate.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Holiday"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Holi_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            objPageCmn.FillData((object)gvHolidayMaster, view.ToTable(), "", "");
            //AllPageCode();
            TxtValueDate.Focus();
        }
        else
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Holiday"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Holi_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMaster, view.ToTable(), "", "");
            //AllPageCode();
            txtValue.Focus();
        }
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedValue.ToString() == "To_Date" || ddlbinFieldName.SelectedValue.ToString() == "From_Date")
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValueDate.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValueDate.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValueDate.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinHoliday"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            objPageCmn.FillData((object)gvHolidayMasterBin, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
            txtbinValueDate.Focus();
        }
        else
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinHoliday"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMasterBin, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
            txtbinValue.Focus();
        }
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvHolidayMasterBin.Rows)
        {
            index = (int)gvHolidayMasterBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvHolidayMasterBin.Rows)
            {
                int index = (int)gvHolidayMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void gvHolidayMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvHolidayMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvHolidayMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvHolidayMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objHoliday.GetHolidayMasterInactive(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvHolidayMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime dtTodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());

        bool HolidayOnWeekOff = false;
        HolidayOnWeekOff = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Holiday Assign On Week Off", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        int b = 0;
        if (txtHolidayName.Text == "")
        {
            DisplayMessage("Enter Holiday Name");
            txtHolidayName.Focus();
            return;
        }
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format dd-MMM-yyyy");
                txtFromDate.Focus();
                return;
            }
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtToDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtToDate.Focus();
                return;
            }
        }
        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            DisplayMessage("From Date cannot be greater than To Date");
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtFromDate.Focus();
            return;
        }
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for validation that user can not assign holiday on assigned weekoff in company parameter
        //code start
        if (HolidayOnWeekOff == false)
        {
            string[] WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Split(',');
            for (int j = 0; j < WeekOffDays.Length; j++)
            {
                DateTime dtFromDateweekOff = objSys.getDateForInput(txtFromDate.Text);
                DateTime dtToDateweekOff = objSys.getDateForInput(txtToDate.Text);
                while (dtFromDateweekOff <= dtToDateweekOff)
                {
                    if (WeekOffDays[j].ToString() == dtFromDateweekOff.DayOfWeek.ToString())
                    {
                        DisplayMessage("You have already weekoff on request date criteria");
                        return;
                    }
                    dtFromDateweekOff = dtFromDateweekOff.AddDays(1);
                }
            }
        }
        //code end
        if (editid.Value == "")
        {
            DataTable dt1 = objHoliday.GetHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt1 = new DataView(dt1, "Holiday_Name='" + txtHolidayName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Holiday Name Already Exists");
                txtHolidayName.Focus();
                return;
            }
            b = objHoliday.InsertHolidayMaster(Session["CompId"].ToString(), txtHolidayName.Text, txtHolidayNameL.Text, Session["BrandId"].ToString(), txtFromDate.Text, txtToDate.Text, "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                FillGrid();
                editid.Value = b.ToString();
                pnlHoliday.Visible = false;
                pnlHolidayGroup.Visible = true;
                rbtnEmp.Checked = true;
                pnlEmp.Visible = true;
                pnlGroup.Visible = false;
                EmpGroup_CheckedChanged(null, null);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string HolidayTypeName = string.Empty;
            DataTable dt1 = objHoliday.GetHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                HolidayTypeName = new DataView(dt1, "Holiday_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Holiday_Name"].ToString();
            }
            catch
            {
                HolidayTypeName = "";
            }
            dt1 = new DataView(dt1, "Holiday_Name='" + txtHolidayName.Text + "' and Holiday_Name<>'" + HolidayTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Holiday Name Already Exists");
                txtHolidayName.Focus();
                return;
            }
            b = objHoliday.UpdateHolidayMaster(editid.Value, Session["CompId"].ToString(), txtHolidayName.Text, txtHolidayNameL.Text, Session["BrandId"].ToString(), txtFromDate.Text, txtToDate.Text, "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                FillGrid();
                pnlHoliday.Visible = false;
                pnlHolidayGroup.Visible = true;
                rbtnEmp.Checked = true;
                pnlEmp.Visible = true;
                pnlGroup.Visible = false;
                EmpGroup_CheckedChanged(null, null);
                //Lbl_Tab_New.Text = Resources.Attendance.New;
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
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
    protected void ImgbtnSelectAll_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        if (ViewState["Checked_Item"] != null)
            userdetails = (ArrayList)ViewState["Checked_Item"];
        string PostedEmpLIst = string.Empty;
        DataTable dtGroup = (DataTable)Session["dtEmp"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtGroup.Rows)
            {
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dr["Emp_Id"].ToString(), Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    if (PostedEmpLIst == "")
                    {
                        PostedEmpLIst = GetEmployeeCode(dr["Emp_Id"].ToString());
                    }
                    else
                    {
                        PostedEmpLIst = PostedEmpLIst + "," + GetEmployeeCode(dr["Emp_Id"].ToString());
                    }
                }
                else
                {
                    if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    {
                        userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));
                    }
                }
            }
        }
        else
        {
            userdetails = new ArrayList();
            ViewState["Checked_Item"] = null;
            lblEmp.Text = "";
            DataTable dtGroup1 = (DataTable)Session["dtEmp"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvEmployee, dtGroup1, "", "");
            ViewState["Select"] = null;
            if (editid.Value != "")
            {
                DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["TimeZoneId"].ToString());
                if (dtEmpInGrp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmpInGrp.Rows.Count; i++)
                    {
                        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dtEmpInGrp.Rows[i]["Emp_Id"].ToString(), Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                        if (dtPostedList.Rows.Count > 0)
                        {
                            if (!userdetails.Contains(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString())))
                            {
                                userdetails.Add(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString()));
                            }
                        }
                    }
                }
            }
        }
        int index = 0;
        foreach (GridViewRow gvr in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvr.RowIndex].Value;
            CheckBox chk = (CheckBox)gvr.FindControl("chkgvSelect");
            Label lb = (Label)gvr.FindControl("lblEmpId");
            if (userdetails.Contains(index))
            {
                chk.Checked = true;
            }
            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text, Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
            if (dtPostedList.Rows.Count > 0)
            {
                chk.Enabled = false;
            }
        }
        ViewState["Checked_Item"] = userdetails;
        if (PostedEmpLIst != "")
        {
            DisplayMessage("Log Posted For Employee :- " + PostedEmpLIst.ToString().TrimEnd() + " ");
        }
    }
    protected void chkgvSelect_CheckedChanged1(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
        if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            if (!lblEmp.Text.Split(',').Contains(lb.Text))
            {
                lblEmp.Text += empidlist;
            }
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblEmp.Text += empidlist;
            string[] split = lblEmp.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblEmp.Text = temp;
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
            // Check in the Session
            if (ViewState["Checked_Item"] != null)
                userdetails = (ArrayList)ViewState["Checked_Item"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            ViewState["Checked_Item"] = userdetails;
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)ViewState["Checked_Item"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                int index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
                CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                if (userdetails.Contains(index))
                {
                    myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(index.ToString(), Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    myCheckBox.Enabled = false;
                }
            }
        }
    }
    protected void chkgvSelectAll_CheckedChanged1(object sender, EventArgs e)
    {
        ArrayList Userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
            CheckBox chk = (CheckBox)gvrow.FindControl("chkgvSelect");
            if (ViewState["Checked_Item"] != null)
                Userdetails = (ArrayList)ViewState["Checked_Item"];
            if (((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll")).Checked)
            {
                if (chk.Enabled == true)
                {
                    chk.Checked = true;
                    if (!Userdetails.Contains(index))
                    {
                        Userdetails.Add(index);
                    }
                }
            }
            else
            {
                if (chk.Enabled == true)
                {
                    chk.Checked = false;
                    if (Userdetails.Contains(index))
                    {
                        Userdetails.Remove(index);
                    }
                }
            }
        }
        if (Userdetails != null && Userdetails.Count > 0)
            ViewState["Checked_Item"] = Userdetails;
        string PostedEmpLIst = string.Empty;
        CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmployee.Rows.Count; i++)
        {
            if (chkSelAll.Checked)
            {
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(((Label)gvEmployee.Rows[i].FindControl("lblEmpId")).Text.ToString(), Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Enabled = false;
                    if (PostedEmpLIst == "")
                    {
                        PostedEmpLIst = GetEmployeeCode(((Label)gvEmployee.Rows[i].FindControl("lblEmpId")).Text.ToString());
                    }
                    else
                    {
                        PostedEmpLIst = PostedEmpLIst + "," + GetEmployeeCode(((Label)gvEmployee.Rows[i].FindControl("lblEmpId")).Text.ToString());
                    }
                }
            }
        }
        if (PostedEmpLIst != "")
        {
            DisplayMessage("Log Posted For Employee :- " + PostedEmpLIst.ToString().TrimEnd() + " ");
        }
    }
    protected void btnEmpRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        txtVal1.Text = "";
        ddlFieldName1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        EmpGroup_CheckedChanged(null, null);
    }
    protected void btnbindEmp_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOption1.SelectedIndex != 0)
        {
            if (ddlOption1.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String)='" + txtVal1.Text + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String) like '%" + txtVal1.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String) Like '" + txtVal1.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtEmp"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordEmp.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvEmployee, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVal1);
        }
        ArrayList userdetails = new ArrayList();
        if (ViewState["Checked_Item"] != null)
            userdetails = (ArrayList)ViewState["Checked_Item"];
        if (editid.Value != "")
        {
            DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["TimeZoneId"].ToString());
            if (dtEmpInGrp.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmpInGrp.Rows.Count; i++)
                {
                    DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dtEmpInGrp.Rows[i]["Emp_Id"].ToString(), Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                    if (dtPostedList.Rows.Count > 0)
                    {
                        if (!userdetails.Contains(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString())))
                        {
                            userdetails.Add(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString()));
                        }
                    }
                }
            }
        }
        foreach (GridViewRow gvr in gvEmployee.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkgvSelect");
            Label lb = (Label)gvr.FindControl("lblEmpId");
            if (userdetails.Contains(Convert.ToInt32(lb.Text)))
            {
                chk.Checked = true;
            }
            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text, Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
            if (dtPostedList.Rows.Count > 0)
            {
                chk.Enabled = false;
            }
        }
        ViewState["Checked_Item"] = userdetails;
    }
    protected void gvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp"], "", "");
        PopulateCheckedValues();
        //AllPageCode();
    }
    protected void gvEmployee_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblEmp.Text = "";
        HDFSortEmployee.Value = HDFSortEmployee.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmp"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortEmployee.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtEmp"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmployee, dt, "", "");
        PopulateCheckedValues();
        //AllPageCode();
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmp, (DataTable)Session["dtEmp1"], "", "");
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
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
                Div_Group.Visible = true; ;
            }
            else
            {
                Session["dtEmp1"] = null;
                gvEmp.DataSource = null;
                gvEmp.DataBind();
                Div_Group.Visible = false;
            }
        }
        else
        {
            Div_Group.Visible = false;
            gvEmp.DataSource = null;
            gvEmp.DataBind();
        }
    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
        {
            pnlEmp.Visible = false;
            pnlGroup.Visible = true;
            btnDelete.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }
            DataTable dtHolidayGroup = objHolidayGroup.GetHolidayGroupByHolidayId(Session["CompId"].ToString(), editid.Value);
            if (dtHolidayGroup.Rows.Count > 0)
            {
                for (int i = 0; i < lbxGroup.Items.Count; i++)
                {
                    DataTable dt = new DataView(dtHolidayGroup, "Group_Id='" + lbxGroup.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        lbxGroup.Items[i].Selected = true;
                    }
                }
            }
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else if (rbtnEmp.Checked)
        {
            pnlEmp.Visible = true;
            pnlGroup.Visible = false;
            btnDelete.Visible = false;
            ViewState["Checked_Item"] = null;
            lblEmp.Text = "";

            Get_Employee_List_By_Location_Department();

            //string GroupIds = string.Empty;
            //string EmpIds = string.Empty;

            //Update On 02-06-2015 For According to Company & Location
            //DataTable dtPageLevel = objAppParam.GetApplicationParameterByCompanyId("Page Level", Session["CompId"].ToString());
            //dtPageLevel = new DataView(dtPageLevel, "Param_Name='Page Level'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtPageLevel.Rows.Count > 0)
            //{
            //    strPageLevel = dtPageLevel.Rows[0]["Param_Value"].ToString();
            //}
            //else
            //{
            //    strPageLevel = "Location";
            //}
            //DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            //if (strPageLevel == "Company")
            //{
            //}
            //else if (strPageLevel == "Location")
            //{
            //    dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //try
            //{
            //    if (Session["SessionDepId"] != null)
            //    {
            //        dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //    }
            //}
            //catch (Exception Ex)
            //{
            //}
            ////  dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            ////for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            ////{
            ////    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
            ////    {
            ////        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
            ////    }
            ////}
            ////dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtEmp.Rows.Count > 0)
            //{
            //    Session["dtEmp"] = dtEmp;
            //    //Common Function add By Lokesh on 12-05-2015
            //    objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            //    lblTotalRecordEmp.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            //    DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value);
            //    ArrayList userdetails = new ArrayList();
            //    if (dtEmpInGrp.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dtEmpInGrp.Rows.Count; i++)
            //        {
            //            if (!userdetails.Contains(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString())))
            //            {
            //                userdetails.Add(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString()));
            //            }
            //        }
            //    }
            //    int index = 0;
            //    foreach (GridViewRow gvr in gvEmployee.Rows)
            //    {
            //        index = (int)gvEmployee.DataKeys[gvr.RowIndex].Value;
            //        CheckBox chk = (CheckBox)gvr.FindControl("chkgvSelect");
            //        Label lb = (Label)gvr.FindControl("lblEmpId");
            //        if (userdetails.Contains(index))
            //        {
            //            chk.Checked = true;
            //        }
            //        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text, Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
            //        if (dtPostedList.Rows.Count > 0)
            //        {
            //            chk.Enabled = false;
            //        }
            //    }
            //    ViewState["Checked_Item"] = userdetails;
            //}
        }
    }
    protected void btnDeleteHoliday_Click(object sender, EventArgs e)
    {
        string SaveGroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroup.Items.Count; i++)
        {
            if (lbxGroup.Items[i].Selected)
            {
                objHolidayGroup.DeleteHolidayGroupMaster(Session["CompId"].ToString(), editid.Value);
                SaveGroupIds += lbxGroup.Items[i].Value + ",";
            }
        }
        DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
        dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + SaveGroupIds.Substring(0, SaveGroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
        {
            DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            while (FromDate <= ToDate)
            {
                objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDate(Session["CompId"].ToString(), editid.Value, dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), FromDate.ToString());
                FromDate = FromDate.AddDays(1);
            }
        }
        for (int i = 0; i < lbxGroup.Items.Count; i++)
        {
            lbxGroup.Items[i].Selected = false;
        }
        DisplayMessage("Record Deleted");
    }
    protected void btnSaveHoliday_Click(object sender, EventArgs e)
    {
        DateTime dtTodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        string Employeecode = string.Empty;
        string Leaveexists = string.Empty;
        string PartialLeaveexists = string.Empty;
        string HalfdayLeaveexists = string.Empty;
        string SavedRecord = string.Empty;
        int b = 0;
        if (rbtnGroup.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            string EmpIds1 = string.Empty;
            string SaveGroupIds = string.Empty;
            DataTable dtGroup = objHolidayGroup.GetHolidayGroupByHolidayId(Session["CompId"].ToString(), editid.Value);
            for (int i = 0; i < dtGroup.Rows.Count; i++)
            {
                SaveGroupIds += dtGroup.Rows[i]["Group_Id"].ToString() + ",";
            }
            objHolidayGroup.DeleteHolidayGroupMaster(Session["CompId"].ToString(), editid.Value);
            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                    b = objHolidayGroup.InsertHolidayGroupMaster(Session["CompId"].ToString(), editid.Value, lbxGroup.Items[i].Value, "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
                }
            }
            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                DataTable dtEmp1 = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                dtEmp1 = new DataView(dtEmp1, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Session["SessionDepId"] != null)
                {
                    dtEmp1 = new DataView(dtEmp1, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                DataTable dtEmpInGroup1 = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                if (SaveGroupIds != "")
                {
                    dtEmpInGroup1 = new DataView(dtEmpInGroup1, "Group_Id in(" + SaveGroupIds.Substring(0, SaveGroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtEmpInGroup1.Rows.Count; i++)
                    {
                        if (!EmpIds1.Split(',').Contains(dtEmpInGroup1.Rows[i]["Emp_Id"].ToString()))
                        {
                            EmpIds1 += dtEmpInGroup1.Rows[i]["Emp_Id"].ToString() + ",";
                        }
                    }
                }
                foreach (string str in EmpIds1.Split(','))
                {
                    if (str != "")
                    {
                        DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                        while (FromDate <= ToDate)
                        {
                            objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDate(Session["CompId"].ToString(), editid.Value, str, FromDate.ToString());
                            FromDate = FromDate.AddDays(1);
                        }
                    }
                }
                if (dtEmp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmp.Rows.Count; i++)
                    {
                        //this code is created by jitendra upadhyay on 12-09-2014
                        //this code for set validation that user can not request if fullday leave,partial leave and half day leave is exists on request date
                        //code start
                        int Counter = 0;
                        Employeecode = GetEmployeeCode(dtEmp.Rows[i]["Emp_Id"].ToString());
                        DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                        while (FromDate <= ToDate)
                        {
                            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dtEmp.Rows[i]["Emp_Id"].ToString(), FromDate.Month.ToString(), FromDate.Year.ToString());
                            if (dtPostedList.Rows.Count == 0)
                            {
                                //check fullday leave
                                DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString());
                                dtLeaveR = new DataView(dtLeaveR, "From_Date <='" + FromDate.ToString() + "' and To_Date>='" + FromDate.ToString() + "' and Is_Canceled<>'True'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtLeaveR.Rows.Count > 0)
                                {
                                    Counter = 1;
                                    if (!Leaveexists.Split(',').Contains(Employeecode))
                                    {
                                        Leaveexists += Employeecode + ",";
                                    }
                                }
                                //check Half Day Leave
                                DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString());
                                dtHalfDay = new DataView(dtHalfDay, "HalfDay_Date>='" + FromDate + "' and HalfDay_Date<='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtHalfDay.Rows.Count > 0)
                                {
                                    if (dtHalfDay.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                                    {
                                        Counter = 1;
                                        if (!HalfdayLeaveexists.Split(',').Contains(Employeecode))
                                        {
                                            HalfdayLeaveexists += Employeecode + ",";
                                        }
                                    }
                                }
                                //check partial Leave
                                DataTable dtPartialLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                                dtPartialLeave = new DataView(dtPartialLeave, "Emp_Id='" + dtEmp.Rows[i]["Emp_Id"].ToString() + "' and Partial_Leave_Date>='" + FromDate + "' and Partial_Leave_Date<='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtPartialLeave.Rows.Count > 0)
                                {
                                    if (dtPartialLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                                    {
                                        Counter = 1;
                                        if (!PartialLeaveexists.Split(',').Contains(Employeecode))
                                        {
                                            PartialLeaveexists += Employeecode + ",";
                                        }
                                    }
                                }
                                // Modified By Nitin on 12/11/2014 to delete Holiday Master entry which exist in db if we entring from Group first time
                                if (dtEmp.Rows.Count > 0)
                                {
                                    for (int L = 0; L < dtEmp.Rows.Count; L++)
                                    {
                                        b = objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDate(Session["CompId"].ToString(), editid.Value, dtEmp.Rows[i]["Emp_Id"].ToString(), FromDate.ToString());
                                    }
                                }
                                //----------------------------------
                                if (Counter == 0)
                                {
                                    if (!SavedRecord.Split(',').Contains(Employeecode))
                                    {
                                        SavedRecord += Employeecode + ",";
                                    }
                                    b = objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), editid.Value, FromDate.ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
                                }
                            }
                            else
                            {
                                if (!SavedRecord.Split(',').Contains(Employeecode))
                                {
                                    SavedRecord += Employeecode + ",";
                                }
                                b = objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), editid.Value, FromDate.ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
                            }
                            FromDate = FromDate.AddDays(1);
                            //code end
                        }
                    }
                }
            }
        }
        else if (rbtnEmp.Checked)
        {
            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (ViewState["Checked_Item"] == null)
            {
                DisplayMessage("Select Employee First");
                return;
            }
            else
            {
                userdetails = (ArrayList)ViewState["Checked_Item"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
            }
            objEmpHoliday.DeleteEmployeeHolidayMaster(Session["CompId"].ToString(), editid.Value);
            string str = string.Empty;
            for (int i = 0; i < userdetails.Count; i++)
            {
                str = userdetails[i].ToString();
                if (str != "")
                {
                    int Counter = 0;
                    Employeecode = GetEmployeeCode(str);
                    DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                    DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                    while (FromDate <= ToDate)
                    {
                        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(str, FromDate.Month.ToString(), FromDate.Year.ToString());
                        if (dtPostedList.Rows.Count == 0)
                        {
                            //check fullday leave
                            DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), str);
                            dtLeaveR = new DataView(dtLeaveR, "From_Date <='" + FromDate.ToString() + "' and To_Date>='" + FromDate.ToString() + "' and Is_Canceled<>'True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtLeaveR.Rows.Count > 0)
                            {
                                Counter = 1;
                                if (!Leaveexists.Split(',').Contains(Employeecode))
                                {
                                    Leaveexists += Employeecode + ",";
                                }
                            }
                            //check Half Day Leave
                            DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), str);
                            dtHalfDay = new DataView(dtHalfDay, "HalfDay_Date>='" + FromDate + "' and HalfDay_Date<='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtHalfDay.Rows.Count > 0)
                            {
                                if (dtHalfDay.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                                {
                                    Counter = 1;
                                    if (!HalfdayLeaveexists.Split(',').Contains(Employeecode))
                                    {
                                        HalfdayLeaveexists += Employeecode + ",";
                                    }
                                }
                            }
                            //check partial Leave
                            DataTable dtPartialLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                            dtPartialLeave = new DataView(dtPartialLeave, "Emp_Id='" + str + "' and Partial_Leave_Date>='" + FromDate + "' and Partial_Leave_Date<='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtPartialLeave.Rows.Count > 0)
                            {
                                if (dtPartialLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                                {
                                    Counter = 1;
                                    if (!PartialLeaveexists.Split(',').Contains(Employeecode))
                                    {
                                        PartialLeaveexists += Employeecode + ",";
                                    }
                                }
                            }
                            if (Counter == 0)
                            {
                                if (!SavedRecord.Split(',').Contains(Employeecode))
                                {
                                    SavedRecord += Employeecode + ",";
                                }
                                b = objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), editid.Value, FromDate.ToString(), str, "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
                            }
                        }
                        else
                        {
                            if (!SavedRecord.Split(',').Contains(Employeecode))
                            {
                                SavedRecord += Employeecode + ",";
                            }
                            b = objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), editid.Value, FromDate.ToString(), str, "", "", "", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
                        }
                        FromDate = FromDate.AddDays(1);
                        //code end
                    }
                }
            }
        }
        if (b != 0)
        {
            string Message = string.Empty;
            if (Leaveexists != "")
            {
                Message = "Leave exists For Employee Code:-" + Leaveexists;
            }
            if (PartialLeaveexists != "")
            {
                Message = Message + "Partial Leave exists For Employee Code:-" + PartialLeaveexists;
            }
            if (HalfdayLeaveexists != "")
            {
                Message = Message + "Half Day Leave exists For Employee Code:-" + HalfdayLeaveexists;
            }
            if (SavedRecord != "")
            {
                Message = Message + "Record Saved For Employee Code:-" + SavedRecord;
            }
            if (Leaveexists != "" || PartialLeaveexists != "" || HalfdayLeaveexists != "")
            {
                Message = Message + "So Reject Exists Leave";
            }
            DisplayMessage(Message);
            Reset();
        }
    }
    protected void btnCancelHoliday_Click(object sender, EventArgs e)
    {
        pnlHoliday.Visible = true;
        pnlHolidayGroup.Visible = false;
        pnlEmp.Visible = false;
        pnlGroup.Visible = false;
        btnCancel_Click(null, null);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objHoliday.GetHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dt.Rows.Count > 0)
        {
            //this code is created by jitendra upadhyay on 11-09-2014
            //this code created for If done log process of that month then cant edit and delete holiday regarding posted log month.
            //code start
            string Month = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).Month.ToString();
            string Year = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).Year.ToString();
            DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["TimeZoneId"].ToString());
            try
            {
                dtEmpInGrp = new DataView(dtEmpInGrp, "", "", DataViewRowState.CurrentRows).ToTable(true, "Emp_Id");
            }
            catch
            {
            }
            foreach (DataRow dr in dtEmpInGrp.Rows)
            {
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dr["Emp_Id"].ToString(), Month.ToString(), Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                    break;
                }
            }
            //code end
            txtHolidayName.Text = dt.Rows[0]["Holiday_Name"].ToString();
            txtHolidayNameL.Text = dt.Rows[0]["Holiday_Name_L"].ToString();
            txtFromDate.Text = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).ToString(objSys.SetDateFormat());
            txtToDate.Text = Convert.ToDateTime(dt.Rows[0]["To_Date"].ToString()).ToString(objSys.SetDateFormat());
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DateTime dtTodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        //this code is created by jitendra upadhyay on 11-09-2014
        //this code created for If done log process of that month then cant edit and delete holiday regarding posted log month.
        //code start
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objHoliday.GetHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dt.Rows.Count > 0)
        {
            string Month = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).Month.ToString();
            string Year = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).Year.ToString();
            DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["TimeZoneId"].ToString());
            try
            {
                dtEmpInGrp = new DataView(dtEmpInGrp, "", "", DataViewRowState.CurrentRows).ToTable(true, "Emp_Id");
            }
            catch
            {
            }
            foreach (DataRow dr in dtEmpInGrp.Rows)
            {
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dr["Emp_Id"].ToString(), Month.ToString(), Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    DisplayMessage("Holiday is in Used ,You Can not Delete");
                    editid.Value = "";
                    return;
                }
            }
        }
        //code end
        int b = 0;
        b = objHoliday.DeleteHolidayMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvHolidayMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHolidayMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Holi_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvHolidayMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvHolidayMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Holi_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Holi_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvHolidayMaster, dt, "", "");
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListHolidayName(string prefixText, int count, string contextKey)
    {
        HolidayMaster objHolidayMaster = new HolidayMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objHolidayMaster.GetHolidayMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Holiday_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Holiday_Name"].ToString();
        }
        return txt;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LocId"] = ddlLocation.SelectedValue;
        FillGrid();
    }
    public void FillGrid()
    {
        //Add On 04-06-2015
        string strFLocId = string.Empty;
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoc.Rows.Count; i++)
                    {
                        strFLocId += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                    }
                }
            }
        }
        //End
        //Update On 02-06-2015 For According to Company & Location
        DataTable dtPageLevel = objAppParam.GetApplicationParameterByCompanyId("Page Level", Session["CompId"].ToString());
        dtPageLevel = new DataView(dtPageLevel, "Param_Name='Page Level'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPageLevel.Rows.Count > 0)
        {
            strPageLevel = dtPageLevel.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPageLevel = "Location";
        }
        DataTable dt = new DataTable();
        if (strPageLevel == "Company")
        {
            dt = objHoliday.GetHolidayMasterByCompanyOnly(Session["CompId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (strFLocId != "")
                {
                    dt = new DataView(dt, "Field1 in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMaster, dt, "", "");
        }
        else if (strPageLevel == "Location")
        {
            dt = objHoliday.GetHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMaster, dt, "", "");
        }
        //AllPageCode();
        Session["dtFilter_Holi_Mstr"] = dt;
        Session["Holiday"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        //Add On 04-06-2015
        string strFLocId = string.Empty;
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoc.Rows.Count; i++)
                    {
                        strFLocId += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                    }
                }
            }
        }
        //End
        DataTable dt = new DataTable();
        //Update On 02-06-2015 For According to Company & Location
        DataTable dtPageLevel = objAppParam.GetApplicationParameterByCompanyId("Page Level", Session["CompId"].ToString());
        dtPageLevel = new DataView(dtPageLevel, "Param_Name='Page Level'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPageLevel.Rows.Count > 0)
        {
            strPageLevel = dtPageLevel.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPageLevel = "Location";
        }
        if (strPageLevel == "Company")
        {
            dt = objHoliday.GetHolidayMasterInactiveByCompanyOnly(Session["CompId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (strFLocId != "")
                {
                    dt = new DataView(dt, "Field1 in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMasterBin, dt, "", "");
        }
        else if (strPageLevel == "Location")
        {
            dt = objHoliday.GetHolidayMasterInactive(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMasterBin, dt, "", "");
        }
        Session["dtbinFilter"] = dt;
        Session["dtbinHoliday"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckkseall = ((CheckBox)gvHolidayMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvHolidayMasterBin.Rows)
        {
            if (ckkseall.Checked)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtAttendenceReport = (DataTable)Session["dtBinFilter"];
        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAttendenceReport.Rows)
            {
                //Allowance_Id
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (!userdetails.Contains(Convert.ToInt32(dr["Holiday_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Holiday_Id"]));
            }
            foreach (GridViewRow gvrow in gvHolidayMasterBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvHolidayMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        DateTime dtTodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        ArrayList userdetails = new ArrayList();
        if (gvHolidayMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                int b = 0;
                if (userdetails.Count > 0)
                {
                    for (int j = 0; j < userdetails.Count; j++)
                    {
                        if (userdetails[j] != "")
                        {
                            b = objHoliday.DeleteHolidayMaster(Session["CompId"].ToString(), userdetails[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), dtTodayDate.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvHolidayMasterBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                gvHolidayMasterBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        pnlEmp.Visible = false;
        pnlGroup.Visible = false;
        pnlHolidayGroup.Visible = false;
        Session["CHECKED_ITEMS"] = null;
        txtHolidayName.Text = "";
        txtHolidayNameL.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        ViewState["Checked_Item"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        pnlHolidayGroup.Visible = false;
        pnlHoliday.Visible = true;
        rbtnEmp.Checked = false;
        rbtnGroup.Checked = false;
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedValue.ToString() == "To_Date" || ddlFieldName.SelectedValue.ToString() == "From_Date")
        {
            txtValue.Visible = false;
            TxtValueDate.Visible = true;
            txtValue.Text = "";
        }
        else
        {
            TxtValueDate.Visible = false;
            txtValue.Visible = true;
            TxtValueDate.Text = "";
        }
    }
    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedValue.ToString() == "To_Date" || ddlbinFieldName.SelectedValue.ToString() == "From_Date")
        {
            txtbinValue.Visible = false;
            txtbinValueDate.Visible = true;
            txtbinValue.Text = "";
        }
        else
        {
            txtbinValueDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValueDate.Text = "";
        }
    }
    private void FillddlLocation(int i)
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
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
            //Common Function add By Lokesh on 23-05-2015
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
    private void FillddlLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            listEmpLocation.DataSource = null;
            listEmpLocation.DataBind();
            listEmpLocation.DataSource = dtLoc;
            listEmpLocation.DataTextField = "Location_Name";
            listEmpLocation.DataValueField = "Location_Id";
            listEmpLocation.DataBind();
            listEmpLocation.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            try
            {
                listEmpLocation.Items.Clear();
                listEmpLocation.DataSource = null;
                listEmpLocation.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                listEmpLocation.Items.Insert(0, li);
                listEmpLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                listEmpLocation.Items.Insert(0, li);
                listEmpLocation.SelectedIndex = 0;
            }
        }
    }
    private void Get_Employee_List_By_Location_Department()
    {
        string strEmpId = string.Empty;
        string strLocationDept = string.Empty;
        string strLocId = string.Empty;
        foreach (ListItem li in listEmpLocation.Items)
        {
            if (li.Selected)
            {
                strLocId = li.Value;
                strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strLocationDept == "")
                {
                    strLocationDept = "0,";
                }
                strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
            }
        }
        if (strLocId == "")
        {
            strLocId = Session["LocId"].ToString();
            strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
        }
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        if (dtEmp.Rows.Count > 0)
        {
            dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            lblTotalRecordEmp.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            Session["dtEmp"] = dtEmp;
            objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            DataTable dtEmpInGrp = objEmpHoliday.GetEmployeeHolidayMasterById(Session["CompId"].ToString(), editid.Value, HttpContext.Current.Session["TimeZoneId"].ToString());
            ArrayList userdetails = new ArrayList();
            if (dtEmpInGrp.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmpInGrp.Rows.Count; i++)
                {
                    if (!userdetails.Contains(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString())))
                    {
                        userdetails.Add(Convert.ToInt32(dtEmpInGrp.Rows[i]["Emp_Id"].ToString()));
                    }
                }
            }
            int index = 0;
            foreach (GridViewRow gvr in gvEmployee.Rows)
            {
                index = (int)gvEmployee.DataKeys[gvr.RowIndex].Value;
                CheckBox chk = (CheckBox)gvr.FindControl("chkgvSelect");
                Label lb = (Label)gvr.FindControl("lblEmpId");
                if (userdetails.Contains(index))
                {
                    chk.Checked = true;
                }
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text, Convert.ToDateTime(txtFromDate.Text).Month.ToString(), Convert.ToDateTime(txtFromDate.Text).Year.ToString());
                if (dtPostedList.Rows.Count > 0)
                {
                    chk.Enabled = false;
                }
            }
            ViewState["Checked_Item"] = userdetails;
        }
        else
        {
            Session["dtEmp"] = dtEmp;
            lblTotalRecordEmp.Text = Resources.Attendance.Total_Records + " : 0";
        }
        //AllPageCode();
    }
    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";
        DataTable dtEmp = objDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id =" + strLocationId + " and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");
        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }
        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;
    }
    protected void btnbindLOCEmp_Click(object sender, EventArgs e)
    {
        Get_Employee_List_By_Location_Department();
    }

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/Holiday", "MasterSetUp", "Holiday", e.CommandArgument.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
}