using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DayPilot.Web.Ui;
using PegasusDataAccess;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Text;
using System.Threading;
using System.Net;

public partial class Attendance_ShiftAssignToEmp : BasePage
{
    SendMailSms ObjSendMailSms = null;
    DepartmentMaster objDep = null;
    Att_AttendanceLog objAttLog = null;
    EmployeeMaster objEmp = null;
    Att_ShiftManagement objShift = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    Att_ShiftDescription objShiftdesc = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_TimeTable objTimeTable = null;
    Set_ApplicationParameter objAppParam = null;
    Common cmn = null;
    DataAccessClass ObjDa = null;
    // Filter Criteria According to Location and Department
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    Set_Approval_Employee objApproalEmp = null;
    Set_ApprovalMaster ObjApproval = null;
    RoleMaster objRole = null;
    Att_Leave_Request objleaveReq = null;
    Att_tmpEmpShiftSchedule objTempempshift = null;
    Att_Employee_Leave objEmpleave = null;
    Set_Employee_Holiday objEmpHoliday = null;

    Set_DocNumber objDocNo = null;
    PageControlCommon objPageCmn = null;
    string EmpCode = string.Empty;
    //------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objShiftdesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objTimeTable = new Att_TimeTable(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        // Filter Criteria According to Location and Department
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objTempempshift = new Att_tmpEmpShiftSchedule(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());

        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {


            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "59", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlLocation();
            FillddlDeaprtment();

            Session["EmpFiltered"] = null;
            FillDataListGrid();
            rbtnEmp.Checked = true;
            rbtnGroup.Checked = false;
            EmpGroup_CheckedChanged(null, null);
            FillGvEmp();
            txtFDate.Focus();
            CalendarExtender_txtNewFromDate.Format = objSys.SetDateFormat();
            CalendarExtender_txtNewToDate.Format = objSys.SetDateFormat();
            DateTime dttodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            txtFrom.Text = (new DateTime(dttodayDate.Year, dttodayDate.Month, 1)).ToString(objSys.SetDateFormat());
            txtTo.Text = (new DateTime(dttodayDate.Year, dttodayDate.Month, DateTime.DaysInMonth(dttodayDate.Year, dttodayDate.Month))).ToString(objSys.SetDateFormat());
            CalendarExtender3.Format = objSys.SetDateFormat();
            CalendarExtender4.Format = objSys.SetDateFormat();
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            BindTreeView();
            txtReferenceNo.Text = GetDocumentNumber();
            txtuploadReferenceNo.Text = txtReferenceNo.Text;
            ViewState["DocNo"] = GetDocumentNumber();

            Session["LocIdReport"] = Session["LocId"].ToString();
        }

        GetGridCalender();
        txtFromDate_CalendarExtender.Format = objSys.SetDateFormat();
        txtToDate_CalendarExtender.Format = objSys.SetDateFormat();
        AllPageCode();
        CalendarExtender1.Format = objSys.SetDateFormat();
        CalendarExtender2.Format = objSys.SetDateFormat();
        this.RegisterPostBackControl();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    protected string GetDocumentNumber()
    {
        DataTable dt = new DataTable();
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "108", "59", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        if (s != "")
        {
            dt = ObjDa.return_DataTable("select COUNT(distinct Field2)+1  from Att_tmpEmpShiftSchedule where company_id=" + Session["CompId"].ToString() + " and brand_id=" + Session["BrandId"].ToString() + " and location_id=" + Session["LocId"].ToString() + " and Field2<>''");

            s += dt.Rows[0][0].ToString();
        }

        return s;
    }
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {

        TreeViewDepartment.Nodes.Clear();

        DataTable dt = objDep.GetDepartmentMaster();


        string strDepId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0,";
        }

        if (Session["EmpId"].ToString() != "0")
        {
            dt = new DataView(dt, "Dep_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }



        string x = "Parent_Id=" + "0" + "";



        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, x, "Dep_Name asc", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count == 0)
        {
            dt = new DataView(dtTemp, "", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable();
        }

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.ShowCheckBox = true;
            tn.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn.Value = dt.Rows[i]["Dep_Id"].ToString();

            TreeViewDepartment.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn);

            i++;
        }
        TreeViewDepartment.DataBind();
        TreeViewDepartment.CollapseAll();
    }
    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = objDep.GetAllDepartmentMaster_By_ParentId(index);


        dt = new DataView(dt, "", "Dep_Name asc", DataViewRowState.OriginalRows).ToTable();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn1.Value = dt.Rows[i]["Dep_Id"].ToString();
            tn1.ShowCheckBox = true;
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn1);
            i++;
        }
        TreeViewDepartment.DataBind();
    }
    private void RegisterPostBackControl()
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Lnk_Shift_Assignment_Yearly);
    }
    # region Filter Criteria According to Location and Department

    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

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
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
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

        //Bind Department According to location on 18-04-2015
        //DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //string LocIds = GetRoleDataPermission(Session["RoleId"].ToString(), "L");

        //if (!GetStatus(Session["RoleId"].ToString()))
        //{
        //    if (LocIds != "")
        //    {
        //        dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}

        //string FLocIds = string.Empty;

        //for (int i = 0; i < dtLoc.Rows.Count; i++)
        //{
        //    if (Session["LocId"].ToString() == dtLoc.Rows[i]["Location_Id"].ToString())
        //    {
        //        FLocIds += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
        //        break;
        //    }
        //}
        //End New Code

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        string DepIds = string.Empty;
        //dt = new DataView(dt, "Location_Id in(" + FLocIds.Substring(0, FLocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        if (ddlLocation.SelectedIndex == 0)
        {
            dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.SelectedValue.Trim(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (DepIds == "")
            {
                DepIds = "0,";
            }


            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)dpDepartment, dt, "Dep_Name", "Dep_Id");
        }
        else
        {
            try
            {
                dpDepartment.Items.Clear();
                dpDepartment.DataSource = null;
                dpDepartment.DataBind();
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
            catch
            {
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
        }
    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillddlDeaprtment();
        FillGvEmp();
        dpDepartment.Focus();
    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGvEmp();
    }
    public void FillGrid()
    {
        string strLocationId = string.Empty;
        string strDepartmentId = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        if (ddlLocation.SelectedIndex == 0)
        {
            strLocationId = Session["LocId"].ToString();

        }
        else
        {
            strLocationId = ddlLocation.SelectedValue;

        }

        strDepartmentId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());


        if (strDepartmentId == "")
        {
            strDepartmentId = "0,";
        }

        if (dpDepartment.SelectedIndex != 0)
        {
            strDepartmentId = dpDepartment.SelectedValue + ",";

        }

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + strLocationId + " and Department_Id in(" + strDepartmentId.Substring(0, strDepartmentId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpLeave"] = dtEmp;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvEmpList, dtEmp, "", "");
            lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        else
        {
            Session["dtEmpLeave"] = null;
            GvEmpList.DataSource = null;
            GvEmpList.DataBind();
            lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + 0 + "";
        }
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        ddlLocation.Focus();
    }
    #endregion
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("59", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code


        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        Page.Title = objSys.GetSysTitle();
        if (Session["EmpId"].ToString() == "0")
        {
            btnNext.Visible = true;
            btndeleteshift.Visible = true;
        }
        else
        {

            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "59", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {

                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnNext.Visible = true;
                        }


                        if (DtRow["Op_Id"].ToString() == "3")
                        {
                            btndeleteshift.Visible = true;
                        }


                    }

                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }
    protected void ddlEmp_TextChanged(object sender, EventArgs e)
    {
        string empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];
        ViewState["EmpCode"] = empid;
        btngo.Focus();

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText); //Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString());

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    // Modified By Nitin On 13/11/2014..................
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListShiftName(string prefixText, int count, string contextKey)
    {
        Att_ShiftManagement objShift = new Att_ShiftManagement(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Common.GetShift(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = "(" + dt.Rows[i][3].ToString() + ") /" + dt.Rows[i][2].ToString() + "";
            }
        }
        else
        {
            dt = objShift.GetShiftMaster(HttpContext.Current.Session["CompId"].ToString());
            str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = "(" + dt.Rows[i]["Shift_Name"].ToString() + ") /" + dt.Rows[i]["Shift_Id"].ToString() + "";
            }
            return str;
        }
        return str;
    }
    protected void txtShiftName_textChanged(object sender, EventArgs e)
    {
        string Shiftid = string.Empty;
        DataTable dtShift = new DataTable();
        dtShift.Clear();


        if (((TextBox)sender).Text != "")
        {
            Shiftid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];

            dtShift = objShift.GetShiftMaster(Session["CompId"].ToString());
            try
            {
                dtShift = new DataView(dtShift, "Shift_Id='" + Shiftid + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception Ex)
            {
                dtShift.Clear();
            }
            if (dtShift.Rows.Count > 0)
            {
                Shiftid = dtShift.Rows[0]["Shift_Id"].ToString();
                ViewState["Shift_Id"] = Shiftid;
            }
            else
            {
                DisplayMessage("Shift Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }

    }
    //--------------------------------------------------
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        try
        {
            Date = Convert.ToDateTime(obj.ToString());
        }
        catch (Exception Ex)
        {
            Date = Convert.ToDateTime("01-01-0001");
        }
        return Date.ToString(objSys.SetDateFormat());

    }
    protected void chkselectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CheckAll_List()", true);
    }
    protected void imggetshiftDate_Click(object sender, EventArgs e)
    {
        GetGridCalender();
    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        Session["EmpFiltered"] = null;

        if (rbtnGroup.Checked)
        {


            objPageCmn.FillData((object)GvEmpList, null, "", "");

            dpDepartment.Visible = false;
            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }
            lbxGroup_SelectedIndexChanged(null, null);


        }
        else if (rbtnEmp.Checked)
        {
            objPageCmn.FillData((object)gvEmployee, null, "", "");

            FillGvEmp();

            dpDepartment.Visible = true;
            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;
        }

        ResetPanel();

    }
    protected void lbxGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

        objPageCmn.FillData((object)GvEmpListSelected, null, "", "");
        Session["EmpFiltered"] = null;
        Session["EmpDt"] = null;


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
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
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
                //Common Function add By Lokesh on 22-05-2015
                //objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");

                DataTable dt = (DataTable)Session["dtEmp1"];
                Session["EmpFiltered"] = dt;
                Session["EmpDt"] = dt;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");



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

        ResetPanel();
        GetGridCalender();

    }
    public void ResetPanel()
    {
        div_Save.Visible = false;
        GvEmpList.Enabled = true;
        chkshiftdateRange.Enabled = true;
        txtShiftName.Enabled = true;
        txtShiftName.Text = "";
        rbtnshiftName.Checked = true;
        rbtnweekOff.Checked = false;
        txtShiftName.Enabled = true;
        objPageCmn.FillData((object)gvAssgnList, null, "", "");
    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
        AllPageCode();
    }
    protected void GvEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["EmpDt"], "", "");
        AllPageCode();
    }

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //div_Save.Visible = false;
        GvEmpList.Enabled = true;
        chkshiftdateRange.Enabled = true;
        txtShiftName.Enabled = true;
        txtShiftName.Text = "";
        rbtnshiftName.Checked = true;
        rbtnweekOff.Checked = false;
        txtShiftName.Enabled = true;
        objPageCmn.FillData((object)gvAssgnList, null, "", "");
        btnGetShiftdate.Enabled = true;
        txtFrom.Enabled = true;
        txtTo.Enabled = true;
        chkselectAllshiftdate.Enabled = true;
        objPageCmn.FillData((object)GvEmpListSelected, null, "", "");
        Session["EmpFiltered"] = null;
    }
    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        txtFrom.Text = "";
        txtTo.Text = "";
        Session["EmpFiltered"] = null;
        FillGvEmp();
        txtFDate.Focus();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        AllPageCode();
        GvShiftReport.DataSource = null;
        GvShiftReport.DataBind();
        txtFDate.Focus();
    }
    protected void Btn_Report_Click(object sender, EventArgs e)
    {

        FillddlLocationReport();
        AllPageCode();
        GvShiftReport.DataSource = null;
        GvShiftReport.DataBind();
        txtFDate.Focus();
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillDataListGrid();
        // ddlFieldName.SelectedIndex = 1;
        //  ddlOption.SelectedIndex = 3;
        // txtValue.Text = "";
        AllPageCode();
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
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
        DataTable dtEmpFilter = (DataTable)Session["EmpFiltered"];
        dtEmpFilter = new DataView(dtEmpFilter, "Emp_Code not ='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmpFilter, "", "");
        Session["EmpFiltered"] = dtEmpFilter;

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
            Session["EmpFiltered"] = null;
            GvEmpListSelected.DataSource = null;
            GvEmpListSelected.DataBind();
        }
    }
    protected void txtTo_TextChanged(object sender, EventArgs e)
    {
        string EmpList = string.Empty;
        try
        {
            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {
                EmpList += (GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString())) + ",";
            }
            DataTable dtShiftAllDate = objEmpSch.GetSheduleDescriptionAll(EmpList);
            dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFrom.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtTo.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvAssgnList, dtShiftAllDate, "", "");
            // if else condition add by ghanshyam on 22-06-2017
            if (dtShiftAllDate.Rows.Count > 0)
            {
                GvShiftReport.Visible = true;
                Div_Grid.Visible = true;
            }
            else
            {
                GvShiftReport.Visible = false;
                Div_Grid.Visible = false;
            }
            //dtShiftAllDate = new DataView(dtShiftAllDate, "Emp_Id IN ('" + EmpList.TrimEnd()+ "')", "Emp_Id", DataViewRowState.CurrentRows).ToTable();
        }
        catch (Exception Ex)
        {

        }
        btnNext.Visible = false;
    }
    protected void btnAddList_Click(object sender, EventArgs e)
    {

        if (GvEmpList.Rows.Count <= 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        string EmpList = hdnFldSelectedValues.Value.Trim();

        if (rbtnGroup.Checked)
        {
            dpDepartment.Visible = false;
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
            //pnlEmpGroup.Visible = false;
            if (Session["dtEmp1"] != null)
            {
                DataTable dt = (DataTable)Session["dtEmp1"];
                Session["EmpFiltered"] = dt;
                Session["EmpDt"] = dt;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");
            }
            else
            {

            }
        }
        else
        {
            dpDepartment.Visible = true;
            DataTable dtEmp1 = new DataTable();
            if (EmpList.ToString() == null || EmpList.ToString() == string.Empty)
            {
            }
            else
            {
                DataTable dtEmp = (DataTable)Session["EmpDt"];

                if (Session["EmpFiltered"] == null)
                {
                    dtEmp1 = dtEmp.Clone();
                }
                else
                {
                    dtEmp1 = (DataTable)Session["EmpFiltered"];

                }


                for (int i = 0; i < GvEmpList.Rows.Count; i++)
                {
                    if (((CheckBox)GvEmpList.Rows[i].FindControl("chkBxSelect")).Checked)
                    {
                        if (new DataView(dtEmp1, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            dtEmp1.Merge(new DataView(dtEmp, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
                        }
                    }
                }
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dtEmp1, "", "");
                objPageCmn.FillData((object)GvEmpList, null, "", "");
                Session["EmpFiltered"] = dtEmp1;
            }

        }

        txtval.Text = "";

        GetGridCalender();

    }
    protected void btnNext1_Click(object sender, EventArgs e)
    {

        div_Save.Visible = false;


        string EmpList = hdnFldSelectedValues.Value.Trim();

        if (rbtnGroup.Checked)
        {
            dpDepartment.Visible = false;
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
            //pnlEmpGroup.Visible = false;
            if (Session["dtEmp1"] != null)
            {
                DataTable dt = (DataTable)Session["dtEmp1"];
                Session["EmpFiltered"] = dt;
                Session["EmpDt"] = dt;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");
            }
            else
            {

            }
        }
        else
        {
            dpDepartment.Visible = true;

            if (EmpList.ToString() == null || EmpList.ToString() == string.Empty)
            {
                //DisplayMessage("Select Atleast One Employee");
                //GvEmpList.HeaderRow.Cells[0].Focus();
                //return;
            }
            else
            {
                FillGvEmpSelected();



            }

        }


        if (chkshiftdateRange.Items.Count == 0)
        {
            DisplayMessage("Select date for assign shift");
            return;
        }

        GvEmpList.Enabled = false;
        chkshiftdateRange.Enabled = false;
        chkselectAllshiftdate.Enabled = false;
        txtShiftName.Enabled = false;
        div_Save.Visible = true;
        btnGetShiftdate.Enabled = false;
        txtFrom.Enabled = false;
        txtTo.Enabled = false;
        GetAssignedList();

    }
    public void GetAssignedList()
    {
        string EmpList = string.Empty;
        try
        {
            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {
                EmpList += (GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString())) + ",";
            }
            DataTable dtShiftAllDate = objEmpSch.GetSheduleDescriptionAll(EmpList);

            DataTable dtTemp = new DataTable();

            DataTable dtshiftTemp = dtShiftAllDate.Clone();


            dtTemp = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFrom.Text) + "' and Att_Date<='" + objSys.getDateForInput(txtTo.Text) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

            dtshiftTemp.Merge(dtTemp);

            //dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFrom.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtTo.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvAssgnList, dtshiftTemp, "", "");
        }
        catch (Exception Ex)
        {

        }


    }

    protected void btnRefresh3Report_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtEmp = (DataTable)Session["EmpDt"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp, "", "");
        Session["EmpFiltered"] = dtEmp;
        ddlSelectOption0.SelectedIndex = 2;
        ddlSelectField0.SelectedIndex = 0;
        txtval0.Text = "";
        AllPageCode();
    }
    public void FillGvEmpSelected()
    {
        DataTable dtEmp = (DataTable)Session["EmpDt"];
        DataTable dtEmp1 = dtEmp.Clone();

        for (int i = 0; i < GvEmpList.Rows.Count; i++)
        {
            if (((CheckBox)GvEmpList.Rows[i].FindControl("chkBxSelect")).Checked)
            {
                if (new DataView(dtEmp1, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                {
                    dtEmp1.Merge(new DataView(dtEmp, "Emp_Code='" + GvEmpList.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
                }
            }
        }
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp1, "", "");
        Session["EmpFiltered"] = dtEmp1;
    }
    protected void btnEmp0_Click(object sender, ImageClickEventArgs e)
    {
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
            DataTable dtEmp = (DataTable)Session["EmpFiltered"];
            if (txtval0.Text != "")
            {
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, view.ToTable(), "", "");
                Session["dtFilter"] = view.ToTable();
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        FillGvEmpSelected();
    }
    protected void btnRefresh2Report_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGvEmp();
        DataTable dt = (DataTable)Session["EmpFiltered"];
        if (Session["EmpFiltered"] != null)
        {
            if (dt.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");
            }
        }
        ddlSelectOption.SelectedIndex = 2;
        ddlSelectField.SelectedIndex = 0;
        txtval.Text = "";
        AllPageCode();
        ddlSelectField.Focus();
    }
    private void FillGvEmp()
    {
        string strLocationId = string.Empty;
        string strDepartmentId = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        if (ddlLocation.SelectedIndex == 0)
        {
            strLocationId = Session["LocId"].ToString();

        }
        else
        {
            strLocationId = ddlLocation.SelectedValue;

        }

        strDepartmentId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());


        if (strDepartmentId == "")
        {
            strDepartmentId = "0,";
        }

        if (dpDepartment.SelectedIndex > 0)
        {
            strDepartmentId = dpDepartment.SelectedValue + ",";

        }



        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + strLocationId + " and Department_Id in(" + strDepartmentId.Substring(0, strDepartmentId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        Session["EmpDt"] = dtEmp;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpList, dtEmp, "", "");
        lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        GvEmpListSelected.DataSource = null;
        GvEmpListSelected.DataBind();
    }
    protected void btnEmp_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlSelectOption.SelectedIndex != 0)
        {
            if (txtval.Text.Trim() == "")
            {
                txtval.Focus();
                DisplayMessage("Please Fill Value");
                return;
            }
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
            DataTable dtEmp = (DataTable)Session["EmpDt"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvEmpList, view.ToTable(), "", "");
            lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            // Session["EmpDt"] = view.ToTable();
        }
        btnrefresh2.Focus();
    }
    protected void chkSelAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        for (int i = 0; i < GvEmpList.Rows.Count; i++)
        {
            ((CheckBox)GvEmpList.Rows[i].FindControl("chkSelect")).Checked = chk.Checked;
        }
        FillGvEmpSelected();
    }
    protected void chkitemselect_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;

        for (int i = 3; i < gvrow.Cells.Count; i++)
        {
            CheckBox chkRow = (CheckBox)gvrow.Cells[i].Controls[0];
            chkRow.Checked = false;
        }
    }
    protected void btnDeleteShift_Click(object sender, EventArgs e)
    {
        if (gvAssgnList.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }


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

            DisplayMessage("Date Not in Proper Format");

            return;
        }
        DateTime DtFromDate = new DateTime();
        int Month = objSys.getDateForInput(txtFrom.Text.ToString()).Month;
        int Year = Convert.ToDateTime(txtFrom.Text).Year;

        int chkIndex = 0;

        foreach (GridViewRow row in GvEmpListSelected.Rows)
        {
            chkIndex = 3;
            foreach (TableCell cell in row.Cells)
            {
                BoundField field = new BoundField();
                try
                {
                    field = (BoundField)((DataControlFieldCell)cell).ContainingField;
                }
                catch
                {
                    continue;
                }


                if (field.DataField == "Code" || field.DataField == "Name")
                {
                    continue;
                }

                CheckBox chkRow = (CheckBox)row.Cells[chkIndex].Controls[0];


                chkIndex++;

                if (!chkRow.Checked)
                {
                    continue;
                }

                chkRow.Checked = false;
                //DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString()));
                DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString().Split('\r')[0]));



                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(GetEmpId(row.Cells[1].Text), Month.ToString(), Year.ToString());

                if (dtPostedList.Rows.Count > 0)
                {
                    if (!PostedEmpList.Split(',').Contains(row.Cells[1].Text))
                    {

                        PostedEmpList += row.Cells[1].Text + ",";
                    }
                }
                else
                {
                    if (!NonPostedLog.Split(',').Contains(row.Cells[1].Text))
                    {
                        NonPostedLog += row.Cells[1].Text + ",";
                    }
                    objEmpSch.DeleteScheduleDescByEmpIdandDate(GetEmpId(row.Cells[1].Text), DtFromDate.ToString(), DtFromDate.ToString());

                }
            }
        }

        if (PostedEmpList.ToString() != "" && NonPostedLog.ToString() != "")
        {
            DisplayMessage("Log posted For Employee code :- " + PostedEmpList.ToString().TrimEnd() + " Record Deleted For Employe :- " + NonPostedLog.ToString().TrimEnd() + "");
        }
        if (PostedEmpList.ToString() == "" && NonPostedLog.ToString().TrimEnd() != "")
        {
            DisplayMessage("Record Deleted");
        }
        if (PostedEmpList.ToString() != "" && NonPostedLog.ToString().TrimEnd() == "")
        {
            DisplayMessage("Log posted For Employee code :- " + PostedEmpList.ToString().TrimEnd() + "");
        }

        GetAssignedList();

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

    }
    protected void btnCancelPanel_Click1(object sender, EventArgs e)
    {
        Session["IsTemp"] = null;
        pnlAddDays.Visible = false;
        PnlTimeTableList.Visible = false;
        AllPageCode();
    }
    protected void btnsaveTempShift_Click(object sender, EventArgs e)
    {
        int flag = 0;
        int flagc = 0;
        int b = 0;
        for (int i = 0; i < chkTimeTableList.Items.Count; i++)
        {

            if (chkTimeTableList.Items[i].Selected == true)
            {
                flag = 1;

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

                dtSch = new DataView(dtSch, "Shift_Type='Temp Shift'  and Shift_Id='" + ViewState["Shift_Id"].ToString() + "' and Emp_Id='" + GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtSch.Rows.Count == 0)
                {
                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ViewState["Shift_Id"].ToString(), "Temp Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), ViewState["Shift_Id"].ToString(), "Temp Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


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
                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", chkTimeTableList.Items[j].Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", chkTimeTableList.Items[j].Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {

                                        if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                        {
                                            flag1 = 1;
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


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

                                    objEmpSch.InsertScheduleDescription(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString()), "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
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

                    dtSch = new DataView(dtSch, "Shift_Type='Temp Shift'  and Shift_Id='" + ViewState["Shift_Id"].ToString() + "' and Emp_Id='" + str + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtSch.Rows.Count == 0)
                    {
                        b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ViewState["Shift_Id"].ToString(), "Temp Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                    }
                    else
                    {
                        b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), str, ViewState["Shift_Id"].ToString(), "Temp Shift", Convert.ToDateTime(txtFrom.Text).ToString(), Convert.ToDateTime(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


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
                                        DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", chkTimeTableList.Items[j].Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                        DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", chkTimeTableList.Items[j].Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                        int flag1 = 0;
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {

                                            if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), str, "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


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

                                        objEmpSch.InsertScheduleDescription(b.ToString(), chkTimeTableList.Items[j].Value, chkDayUnderPeriod.Items[dayno].Value, false.ToString(), true.ToString(), false.ToString(), str, "0", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }


                                }
                            }

                        }



                    }
                }

            }

        }

        DisplayMessage("Record Updated", "green");


    }
    public bool IsWeekOff(string WeekDay)
    {
        bool b = false;
        Set_ApplicationParameter objApp = new Set_ApplicationParameter(Session["DBConnection"].ToString());

        if (objApp.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) != "No")
        {
            b = true;
        }
        else
        {

            b = false;
        }
        return b;
    }
    public void GetGridCalender()
    {

        div_Save.Visible = false;
        DataTable dt = new DataTable();

        dt = (DataTable)Session["EmpFiltered"];

        if (dt == null)
        {
            return;
        }
        dt = dt.DefaultView.ToTable(true, "Emp_Code", "Emp_Name");


        DateTime dtFromDate = Convert.ToDateTime(txtFrom.Text);
        DateTime dtTodate = Convert.ToDateTime(txtTo.Text);


        dt.AcceptChanges();


        while (dtFromDate <= dtTodate)
        {
            if (!dt.Columns.Contains(dtFromDate.Day.ToString()))
            {
                dt.Columns.Add(dtFromDate.Day.ToString() + Environment.NewLine + "" + dtFromDate.DayOfWeek.ToString().Substring(0, 3) + "", typeof(string));

            }

            dtFromDate = dtFromDate.AddDays(1);
        }



        dt.Columns["Emp_Code"].ColumnName = "Code";
        dt.Columns["Emp_Name"].ColumnName = "Name";

        objPageCmn.FillData((object)GvEmpListSelected, dt, "", "");


        foreach (GridViewRow row in GvEmpListSelected.Rows)
        {
            for (int i = 3; i < row.Cells.Count; i++)
            {
                CheckBox cb = new CheckBox();
                //cb.ID = "chk_" + i.ToString() ;
                cb.Text = row.Cells[i].Text;
                row.Cells[i].Controls.Add(cb);
            }
        }
        div_Save.Visible = true;

        GetAssignedList();
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        int chkIndex = 0;

        DateTime dtLogProcessFromdate = new DateTime();
        DateTime dtLogProcessTodate = new DateTime();
        DataTable dtApprovalMaster = new DataTable();
        DataTable dtSch = new DataTable();
        DataTable dtPostedList = new DataTable();
        DataTable dtShiftD = new DataTable();
        DataTable dtShift = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtTime = new DataTable();

        DataTable dtTempShift = new DataTable();
        DataTable dtGetTemp1 = new DataTable();
        string PostedEmpList = string.Empty;
        string OverlapDate = string.Empty;
        DateTime dtTodayDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Day, 23, 59, 1);
        int b = 0;
        bool IsTemp = false;
        int rem = 0;
        bool shiftApprovalFunction = false;
        string strEmpId = string.Empty;
        int MaxId = 0;
        string EmpPermission = string.Empty;
        bool isbackdateEntryAllow = Inventory_Common.CheckUserPermission("59", "17", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());

        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Shift Assignment").Rows[0]["Approval_Level"].ToString();
        shiftApprovalFunction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (shiftApprovalFunction && txtReferenceNo.Text == "")
        {
            DisplayMessage("Reference number not found, please configure it");
            return;
        }

        // btnNext.Visible = false;

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
                txtTo.Focus();
                return;
            }
        }
        catch (Exception)
        {
            txtFrom.Focus();
            DisplayMessage("Date Not In Proper Format");

            return;
        }

        if (txtShiftName.Text == "")
        {
            txtShiftName.Focus();
            DisplayMessage("Please Select Shift");
            return;
        }


        if (!isbackdateEntryAllow)
        {

            if (Convert.ToDateTime(txtFrom.Text).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date || Convert.ToDateTime(txtTo.Text).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
            {
                DisplayMessage("you can not upload back dated shift");
                return;
            }

        }


        if (rbtnshiftName.Checked)
        {

            dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(ViewState["Shift_Id"].ToString());

            if (dtShiftD.Rows.Count == 0)
            {
                DisplayMessage("Shift Not Defined");
                return;

            }
        }
        else
        {
            ViewState["Shift_Id"] = "0";
        }

        int Month = objSys.getDateForInput(txtFrom.Text.ToString()).Month; ;
        int Year = Convert.ToDateTime(txtFrom.Text).Year;

        //when shift approval functionality is true then need to check that approval configured or not

        if (shiftApprovalFunction)
        {

            for (int i = 0; i < GvEmpListSelected.Rows.Count; i++)
            {
                strEmpId = GetEmpId(GvEmpListSelected.DataKeys[i][0].ToString());

                dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "59", strEmpId);

                // dt1 = new DataView(dt1, "Approval='Leave'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtApprovalMaster.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue");
                    dtApprovalMaster.Dispose();
                    return;
                }


                dtPostedList = objAttLog.Get_Pay_Employee_Attendance(strEmpId, Month.ToString(), Year.ToString());

                if (dtPostedList.Rows.Count > 0)
                {
                    DisplayMessage("Log posted For Employee Code =" + GvEmpListSelected.DataKeys[i][0].ToString() + "");
                    dtPostedList.Dispose();
                    return;
                }

            }

        }

        dtSch = objEmpSch.GetSheduleMaster();
        dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), ViewState["Shift_Id"].ToString());

        //this query for get max id and source table is Att_tmpEmpShiftSchedule which we are using for insert approval request
        string strTRansNo = ObjDa.return_DataTable("select isnull( MAX( CAST( Att_tmpEmpShiftSchedule.TRans_no  as int)),0)+1 from Att_tmpEmpShiftSchedule where Location_id=" + Session["Locid"].ToString() + "").Rows[0][0].ToString();

        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;

        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString());
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        //if (rbtnEmp.Checked)
        //{
        if (GvEmpListSelected.Rows.Count == 0)
        {
            DisplayMessage("Select Atleast One Employee");
            return;
        }


        string strEmpList_LogProcess = string.Empty;

        int SaveCounter = 0;
        bool IsContinue = false;


        dtLogProcessFromdate = objSys.getDateForInput(txtFrom.Text.ToString());
        dtLogProcessTodate = objSys.getDateForInput(txtTo.Text.ToString());

        //start

        foreach (GridViewRow row in GvEmpListSelected.Rows)
        {
            IsContinue = false;
            strEmpId = GetEmpId(GvEmpListSelected.DataKeys[row.RowIndex][0].ToString());
            chkIndex = 3;

            if (!shiftApprovalFunction)
            {
                strEmpList_LogProcess += strEmpId + ",";
            }


            foreach (TableCell cell in row.Cells)
            {
                BoundField field = new BoundField();
                try
                {
                    field = (BoundField)((DataControlFieldCell)cell).ContainingField;
                }
                catch
                {
                    continue;
                }
                if (field.DataField == "Code" || field.DataField == "Name")
                {
                    continue;
                }
                CheckBox chkRow = (CheckBox)row.Cells[chkIndex].Controls[0];
                chkIndex++;

                if (chkRow.Checked)
                {
                    SaveCounter++;
                    IsContinue = false;
                    break;
                }
                else
                {
                    IsContinue = true;
                }
            }


            if (IsContinue)
            {
                continue;
            }


            //this code is created by jitendra upadhyay on 10-09-2014
            //code for user can not assign shift when log posted
            //code start



            if (shiftApprovalFunction)
            {
                dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "59", strEmpId);

                MaxId = InsertHeaderRecordForApproval(strEmpId, strTRansNo, objSys.getDateForInput(txtFrom.Text.ToString()).ToString(), objSys.getDateForInput(txtTo.Text.ToString()).ToString(), "Monthly", txtReferenceNo.Text);


                objleaveReq.InsertLeaveApproval(strEmpId, EmpPermission, dtApprovalMaster, MaxId.ToString(), null, Common.GetEmployeeName(strEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()), objSys.getDateForInput(txtFrom.Text.ToString()), objSys.getDateForInput(txtTo.Text.ToString()), false, true, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());

            }
            else
            {
                dtTemp = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + ViewState["Shift_Id"].ToString() + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count == 0)
                {
                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "Normal Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "Normal Shift", objSys.getDateForInput(txtFrom.Text).ToString(), objSys.getDateForInput(txtTo.Text).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                }
            }

            //if (b != 0 && !shiftApprovalFunction)
            //{
            DateTime DtFromDate = objSys.getDateForInput(txtFrom.Text.ToString());
            DateTime DtToDate = objSys.getDateForInput(txtTo.Text.ToString());


            int days = DtToDate.Subtract(DtFromDate).Days + 1;
            if (IsTemp == false)
            {

                // From Date to To Date

                int index = 0;
                int cycle = 0;
                int TotalDays = 1;

                try
                {
                    index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
                    cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());
                }
                catch
                {
                    index = 7;
                    cycle = 1;
                }

                string cycletype = string.Empty;
                string cycleday = string.Empty;


                if (index == 7)
                {
                    bool IsweekOff = false;
                    int daysShift = cycle * index;
                    string weekday = DtFromDate.DayOfWeek.ToString();
                    int state = 0;
                    int k = Att_ScheduleMaster.GetCycleDay(weekday);
                    int j = 1;
                    int a = k;
                    int f = 0;

                    chkIndex = 3;

                    foreach (TableCell cell in row.Cells)
                    {
                        BoundField field = new BoundField();
                        try
                        {
                            field = (BoundField)((DataControlFieldCell)cell).ContainingField;
                        }
                        catch
                        {
                            continue;
                        }
                        if (field.DataField == "Code" || field.DataField == "Name")
                        {
                            continue;
                        }
                        CheckBox chkRow = (CheckBox)row.Cells[chkIndex].Controls[0];
                        chkIndex++;

                        if (!chkRow.Checked)
                        {
                            continue;
                        }

                        chkRow.Checked = false;



                        DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString().Split('\r')[0]));


                        if (shiftApprovalFunction)
                        {
                            if (rbtnshiftName.Checked)
                            {
                                InsertDetailRecordForApproval(MaxId.ToString(), strEmpId, DtFromDate.ToString(), ViewState["Shift_Id"].ToString(), "Shift");
                            }
                            else
                            {
                                InsertDetailRecordForApproval(MaxId.ToString(), strEmpId, DtFromDate.ToString(), ViewState["Shift_Id"].ToString(), "OFF");
                            }
                            continue;
                        }

                        if (k % 7 == 0)
                        {
                            if (f != 0)
                            {
                                if (k % 7 == 0 && j > cycle)
                                {
                                    j++;
                                    rem = 1;
                                }

                                if (j > cycle)
                                {
                                    j = 1;
                                }
                            }

                        }
                        f++;
                        if (k <= daysShift || j == cycle)
                        {
                            a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());
                            if (rem == 1 && k % 7 == 0)
                            {
                                j++;
                            }

                            if (rbtnshiftName.Checked)
                            {
                                dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            if (k % 7 == 0 && j == cycle)
                            {
                                j = 1;
                            }
                            if (k % 7 == 0 && j < cycle)
                            {
                                j++;
                            }

                            objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                            if (dtGetTemp1.Rows.Count > 0)
                            {
                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dtTime.Rows.Count > 0)
                                {
                                    if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                    {
                                        //here we are deleting shift for selected date 

                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                        else
                                        {
                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            int flag1 = 0;
                                            for (int s = 0; s < dtTime.Rows.Count; s++)
                                            {

                                                if (((Button)sender).ID == "btnSaveShfitEmployee")
                                                {
                                                    if (dtTime.Rows[s]["Trans_Id"].ToString() == hdnShiftId.Value)
                                                    {
                                                        continue;
                                                    }
                                                }
                                                if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                {
                                                    flag1 = 1;
                                                    OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";

                                                }
                                            }
                                            if (flag1 == 0)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                    {
                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                    }

                                }
                            }
                            else
                            {
                                //Modified accoding to excludedays parameter 19 sept 2013 kunal
                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dts.Rows.Count == 0)
                                {
                                    if (ExcludeDayAs == "IsOff")
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                    }
                                    else
                                    {
                                        // Modified By Nitin Jain On 27/08/2014....
                                        foreach (string str in CompWeekOffDay.Split(','))
                                        {
                                            if (str == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                        //..............
                                    }

                                }
                                else
                                {


                                }
                            }




                            k++;
                        }
                        else
                        {
                            k = 1;
                            j = 1;
                            f = 0;
                            a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());

                            if (rbtnshiftName.Checked)
                            {
                                dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                            if (dtGetTemp1.Rows.Count > 0)
                            {
                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dtTime.Rows.Count > 0)
                                {
                                    if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }
                                        else
                                        {

                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            int flag1 = 0;
                                            for (int s = 0; s < dtTime.Rows.Count; s++)
                                            {
                                                if (((Button)sender).ID == "btnSaveShfitEmployee")
                                                {
                                                    if (dtTime.Rows[s]["Trans_Id"].ToString() == hdnShiftId.Value)
                                                    {
                                                        continue;
                                                    }
                                                }


                                                if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                {
                                                    flag1 = 1;
                                                }

                                            }
                                            if (flag1 == 0)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                    {
                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                        }

                                    }
                                    else
                                    {

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                    }
                                }
                            }
                            else
                            {
                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dts.Rows.Count == 0)
                                {
                                    if (ExcludeDayAs == "IsOff")
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else
                                    {
                                        // Modifed By Nitin On 27/8/2014/////
                                        foreach (string str in CompWeekOffDay.Split(','))
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            k++;

                        }



                        DtFromDate = DtFromDate.AddDays(1);
                    }
                    //}

                }
                else if (index == 31)
                {
                    int daysShift = cycle * index;

                    int k = DtFromDate.Day;
                    int a = 0;
                    int j = 1;
                    int mon = DtFromDate.Month;
                    //foreach (GridViewRow row in GvEmpListSelected.Rows)
                    //{

                    //    if (row.Cells[1].Text != GvEmpListSelected.DataKeys[i][0].ToString())
                    //    {
                    //        continue;
                    //    }

                    chkIndex = 3;

                    foreach (TableCell cell in row.Cells)
                    {

                        BoundField field = new BoundField();
                        try
                        {
                            field = (BoundField)((DataControlFieldCell)cell).ContainingField;
                        }
                        catch
                        {
                            continue;
                        }


                        if (field.DataField == "Code" || field.DataField == "Name")
                        {
                            continue;
                        }

                        CheckBox chkRow = (CheckBox)row.Cells[chkIndex].Controls[0];
                        chkIndex++;

                        if (!chkRow.Checked)
                        {
                            continue;
                        }

                        chkRow.Checked = false;

                        //DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString()));

                        DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString().Split('\r')[0]));


                        if (shiftApprovalFunction)
                        {
                            InsertDetailRecordForApproval(MaxId.ToString(), strEmpId, DtFromDate.ToString(), ViewState["Shift_Id"].ToString(), "Shift");
                            continue;
                        }

                        if (k <= daysShift)
                        {
                            a = DtFromDate.Day;

                            if (rbtnshiftName.Checked)
                            {
                                dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }

                            //
                            if (dtGetTemp1.Rows.Count > 0)
                            {


                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                    if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                    {

                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int flag1 = 0;

                                        if (dtTime.Rows.Count > 0)
                                        {
                                            DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {
                                                for (int s = 0; s < dtTime.Rows.Count; s++)
                                                {
                                                    if (((Button)sender).ID == "btnSaveShfitEmployee")
                                                    {
                                                        if (dtTime.Rows[s]["Trans_Id"].ToString() == hdnShiftId.Value)
                                                        {
                                                            continue;
                                                        }
                                                    }

                                                    if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                    {
                                                        flag1 = 1;
                                                    }

                                                }
                                                if (flag1 == 0)
                                                {

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {
                                                for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                                }

                                            }

                                        }
                                    }

                                }



                            }
                            else
                            {
                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dts.Rows.Count == 0)
                                {
                                    if (ExcludeDayAs == "IsOff")
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else
                                    {
                                        // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                        foreach (string str in CompWeekOffDay.Split(','))
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
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
                    //}

                }
                else if (index == 1)
                {
                    int k = 1;
                    int a = k;
                    int daysShift = cycle * index;

                    chkIndex = 3;

                    foreach (TableCell cell in row.Cells)
                    {
                        BoundField field = new BoundField();
                        try
                        {
                            field = (BoundField)((DataControlFieldCell)cell).ContainingField;
                        }
                        catch
                        {
                            continue;
                        }


                        if (field.DataField == "Code" || field.DataField == "Name")
                        {
                            continue;
                        }

                        CheckBox chkRow = (CheckBox)row.Cells[chkIndex].Controls[0];
                        chkIndex++;

                        if (!chkRow.Checked)
                        {
                            continue;
                        }


                        chkRow.Checked = false;


                        DtFromDate = new DateTime(Year, Month, Convert.ToInt32(field.DataField.ToString().Split('\r')[0]));

                        if (shiftApprovalFunction)
                        {
                            InsertDetailRecordForApproval(MaxId.ToString(), strEmpId, DtFromDate.ToString(), ViewState["Shift_Id"].ToString(), "Shift");
                            continue;
                        }


                        if (k <= daysShift)
                        {
                            a = k;

                            if (rbtnshiftName.Checked)
                            {
                                dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }


                            if (dtGetTemp1.Rows.Count > 0)
                            {


                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                    if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                    {

                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int flag1 = 0;

                                        if (dtTime.Rows.Count > 0)
                                        {
                                            DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {
                                                for (int s = 0; s < dtTime.Rows.Count; s++)
                                                {
                                                    if (((Button)sender).ID == "btnSaveShfitEmployee")
                                                    {
                                                        if (dtTime.Rows[s]["Trans_Id"].ToString() == hdnShiftId.Value)
                                                        {
                                                            continue;
                                                        }
                                                    }

                                                    if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                    {
                                                        flag1 = 1;
                                                    }

                                                }
                                                if (flag1 == 0)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString());

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                            else
                                            {

                                                for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                {

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                                }
                                            }

                                        }
                                    }

                                }



                            }
                            else
                            {
                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString());

                                if (dts.Rows.Count == 0)
                                {
                                    if (ExcludeDayAs == "IsOff")
                                    {

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else
                                    {
                                        // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                        foreach (string str in CompWeekOffDay.Split(','))
                                        {
                                            if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, ViewState["Shift_Id"].ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                            }
                                        }
                                    }
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

                    //}
                }


            }
            else
            {//temporary


            }
            //}

        }

        dtApprovalMaster.Dispose();
        dtSch.Dispose();
        dtPostedList.Dispose();
        dtShiftD.Dispose();
        dtShift.Dispose();
        dtTemp.Dispose();
        dtTime.Dispose();
        dtTempShift.Dispose();
        dtGetTemp1.Dispose();



        if (SaveCounter <= 0)
        {
            DisplayMessage("Record not saved");
        }
        else
        {
            string Message = string.Empty;

            if (OverlapDate == null || OverlapDate.Trim() == "")
            {
                if (shiftApprovalFunction)
                {
                    DisplayMessage("Shift request submitted successfully");
                    txtReferenceNo.Text = GetDocumentNumber();
                }
                else
                {
                    DisplayMessage("Record Saved", "green");
                }

                ViewState["Result"] = "True";
                //btnNew_Click(null, null);
                Div_Before_Next.Visible = true;
                Div_After_Next.Visible = false;
            }
            else
            {
                if (((Button)sender).ID == "btnsave")
                {
                    DisplayMessage("Shift Overlapping");
                }
                ViewState["Result"] = "False";
            }
        }

        //auto log process code when shift approval functionality is disabled

        if (!shiftApprovalFunction)
        {
            //ThreadStart ts = delegate () { ObjLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpList_LogProcess, Session["userId"].ToString(), "", dtLogProcessFromdate, dtLogProcessTodate, "0"); };

            //// The thread.
            //Thread t = new Thread(ts);

            //// Run the thread.
            //t.Start();



            // objpryceservice.AutoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpList_LogProcess, dtLogProcessFromdate.ToString(), dtLogProcessTodate.ToString(), Session["DBConnection"].ToString());
        }

        GetAssignedList();
        txtShiftName.Text = "";
        rbtnshiftName.Checked = true;
        rbtnweekOff.Checked = false;
        txtShiftName.Enabled = true;
    }
    protected void lnkBackToList_Click(object sender, EventArgs e)
    {
        GvShiftReport.DataSource = null;
        GvShiftReport.DataBind();
        Session["IsTemp"] = null;
        Lbl_Tab_New.Text = "New";
    }
    public string GetEmpId(string empcode)
    {
        string empId = string.Empty;
        DataTable dt = ObjDa.return_DataTable("select emp_id from Set_EmployeeMaster where Company_Id=" + Session["CompId"].ToString() + " and  Emp_Code='" + empcode + "'");
        if (dt.Rows.Count > 0)
        {
            empId = dt.Rows[0]["Emp_Id"].ToString();
        }
        dt.Dispose();
        return empId;
    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        for (int t = 0; t < chkTimeTableList.Items.Count; t++)
        {
            chkTimeTableList.Items[t].Selected = false;
        }
        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = false;
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
    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtproduct = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtproduct = new DataView(dtproduct, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        AllPageCode();
    }
    private void FillDataListGrid()
    {


        DataTable dtproduct = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtproduct = new DataView(dtproduct, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtproduct = new DataView(dtproduct, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }

        Session["dtEmp"] = dtproduct;


    }
    // Chart View Code Start Here by Nitin Jain on 08/11/2014
    protected void btnChart_Click(object sender, EventArgs e)
    {
        GvShiftReport.Visible = false;
        DayPilotScheduler1.Visible = true;
        //dtlistshift.Visible = true;

        dtlistshift.DataSource = getShiftDatatlist();
        dtlistshift.DataBind();


        LoadResources();
        DayPilotScheduler1.StartDate = DateTime.Today;
        DayPilotScheduler1.EndDate.AddDays(15);
        Label1.Text = DateTime.Today.ToShortDateString();
        DayPilotScheduler1.DataSource = getData();
        DayPilotScheduler1.DataBind();
    }
    public DataTable getShiftDatatlist()
    {
        DataTable dt = new DataTable();
        DataTable dtShiftAllDate = new DataTable();
        dt.Columns.Add("DayName");
        dt.Columns.Add("Day");
        dt.Columns.Add("shiftName");
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("attDate", typeof(DateTime));
        dt.Columns.Add("shiftName_1");

        DateTime dtFromdate = Convert.ToDateTime(txtFDate.Text);
        DateTime dttoDate = Convert.ToDateTime(txtTDate.Text);

        while (dtFromdate <= dttoDate)
        {
            DataRow dr = dt.NewRow();

            dr[0] = dtFromdate.DayOfWeek.ToString().Substring(0, 3);
            dr[1] = dtFromdate.Day.ToString();

            dtShiftAllDate = objEmpSch.Sp_GetEMployeeSchedule(GetEmpId(txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1].ToString()), dtFromdate.ToString(), dtFromdate.ToString());

            if (dtShiftAllDate.Rows.Count > 0)
            {
                if (dtShiftAllDate.Rows[0]["Shift_Name"].ToString().Trim() != "")
                {
                    if (dtShiftAllDate.Rows[0]["Shift_Name"].ToString().Length >= 20)
                        dr[2] = dtShiftAllDate.Rows[0]["Shift_Name"].ToString().Substring(0, 20);
                    else
                        dr[2] = dtShiftAllDate.Rows[0]["Shift_Name"].ToString();

                    dr[3] = dtShiftAllDate.Rows[0]["Trans_Id"].ToString();

                    dr[5] = dtShiftAllDate.Rows[0]["Shift_Name"].ToString();
                }
                else
                {
                    dr[2] = "Week Off";
                    dr[5] = "Week Off";
                    dr[3] = dtShiftAllDate.Rows[0]["Trans_Id"].ToString();
                }


            }
            else
            {
                dr[2] = "Add Shift";
                dr[5] = "Add Shift";
                dr[3] = "0";
            }

            dr[4] = dtFromdate.ToString();

            dtShiftAllDate.Dispose();


            dt.Rows.Add(dr);

            dtFromdate = dtFromdate.AddDays(1);
        }

        return dt;

    }
    private void LoadResources()
    {

        DayPilotScheduler1.Resources.Clear();
        DateTime EndDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).AddDays(15);
        DataTable locations = objEmpSch.Sp_GetEMployeeSchedule(GetEmpId(txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1].ToString()), txtFDate.Text, txtTDate.Text);
        DayPilotScheduler1.Resources.Clear();

        foreach (DataRow dr in locations.Rows)
        {
            Resource c = new Resource((string)dr["Att_Date"].ToString(), Convert.ToString(dr["Trans_Id"]));
            DayPilotScheduler1.Resources.Add(c);
        }
        //}
    }
    protected DataTable getData()
    {
        DateTime EndDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).AddDays(20);
        DataTable dtShiftAllDate = objEmpSch.Sp_GetEMployeeSchedule(GetEmpId(txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1].ToString()), txtFDate.Text, txtTDate.Text);
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("resource", typeof(string));

        DataRow dr;

        for (int i = 0; i < dtShiftAllDate.Rows.Count; i++)
        {
            dr = dt.NewRow();
            dr["id"] = dtShiftAllDate.Rows[i]["Trans_Id"].ToString();
            try
            {
                dr["start"] = Convert.ToDateTime(dtShiftAllDate.Rows[i]["On_Time"].ToString());
                dr["end"] = Convert.ToDateTime(dtShiftAllDate.Rows[i]["Off_Time"].ToString());
            }
            catch
            {
                dr["start"] = Convert.ToDateTime("12:00");
                dr["end"] = Convert.ToDateTime("12:00");
            }

            string EndDate1 = dr["end"].ToString();
            dr["name"] = dtShiftAllDate.Rows[i]["Shift_Name"].ToString();
            dr["resource"] = dtShiftAllDate.Rows[i]["Trans_Id"].ToString();
            if (Convert.ToDateTime(dr["end"].ToString()) < Convert.ToDateTime(dr["start"].ToString()))
            {
                dr["end"] = Convert.ToDateTime("23:59");
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["id"] = dtShiftAllDate.Rows[i]["Trans_Id"].ToString();
                dr["name"] = dtShiftAllDate.Rows[i]["Shift_Name"].ToString();
                dr["resource"] = dtShiftAllDate.Rows[i]["Trans_Id"].ToString();
                dr["start"] = Convert.ToDateTime("00:00");
                dr["end"] = Convert.ToDateTime(EndDate1);
                dt.Rows.Add(dr);
            }
            else
            {
                dt.Rows.Add(dr);
            }
            // dt.Rows.Add(dr);
        }

        return dt;

    }
    // Code over..........................................
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Select From Date And To Date');", true);
            return;
        }
        try
        {
            if (txtFromDate.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFromDate.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                DisplayMessage("To Date Required");
                txtToDate.Focus();
                return;
            }
        }
        catch
        {
            DisplayMessage("Date Not In Proper Format");
            return;
        }

        if (objSys.getDateForInput(txtFromDate.Text.ToString()) > objSys.getDateForInput(txtToDate.Text.ToString()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('To Date should be greater');", true);
            return;
        }
        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(GetEmpId(editid.Value));
        dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFromDate.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtToDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvShiftReport, dtShiftAllDate, "", "");
    }
    protected void btngo_Click(object sender, EventArgs e)
    {

        GvShiftReport.Visible = true;
        DayPilotScheduler1.Visible = false;
        dtlistshift.Visible = false;

        try
        {
            if (txtFDate.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFDate.Focus();
                return;
            }
            if (txtTDate.Text == "")
            {
                DisplayMessage("To Date Required");
                txtTDate.Focus();
                return;
            }
            if (txtEmp.Text == "")
            {
                DisplayMessage("Enter Employee Name");
                txtEmp.Focus();
                return;
            }
        }
        catch
        {
            DisplayMessage("Date Not In Proper Format");
            return;
        }

        if (objSys.getDateForInput(txtFDate.Text.ToString()) > objSys.getDateForInput(txtTDate.Text.ToString()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('To Date should be greater');", true);
            return;
        }
        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(GetEmpId(ViewState["EmpCode"].ToString()));

        dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtFDate.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtTDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvShiftReport, dtShiftAllDate, "", "");
        GvShiftReport.Visible = true;
    }
    public void bindchecklist()
    {//
        DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)chkTimeTableList, dt, "TimeTable_Name", "TimeTable_Id");

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

        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)chkDayUnderPeriod, dtfordays, "days", "dayno");

        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = true;
        }
    }
    protected void GvShiftreport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String s = GvShiftReport.DataKeys[e.Row.RowIndex]["OnDuty_Time"].ToString();

            String AttDate = GvShiftReport.DataKeys[e.Row.RowIndex]["Att_Date"].ToString();

            DataTable dtLeave = objEmpSch.GetSheduleDescriptionByDate(GetEmpId(ViewState["EmpCode"].ToString()), AttDate);
            string WeekOff_Color = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            string TempShift_Color = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            string Holiday_Color = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtLeave.Rows.Count > 0)
            {
                if (dtLeave.Rows[0]["IS_Off"].ToString() == "True")
                {
                    ((CheckBox)e.Row.FindControl("ChkIsOff")).Checked = true;
                    e.Row.Style.Add("background-color", "#" + WeekOff_Color);
                }
                else if (dtLeave.Rows[0]["Is_Temp"].ToString() == "True")
                {
                    ((CheckBox)e.Row.FindControl("Is_Temp")).Checked = true;
                    // e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
                    e.Row.Style.Add("background-color", "#" + TempShift_Color);
                }
                else
                {
                }
            }






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
                e.Row.Style.Add("background-color", "#" + Holiday_Color);
            }
            else
            {
                ((CheckBox)e.Row.FindControl("chkHoliday")).Checked = false;
            }
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Common objcom = new Common(Session["DBConnection"].ToString());
        editid.Value = e.CommandArgument.ToString();
        lblempname.Text = objcom.GetEmpName(GetEmpId(editid.Value), HttpContext.Current.Session["CompId"].ToString());
        Lbl_Tab_New.Text = "View";
    }


    protected void DayPilotScheduler1_EventClick(object sender, DayPilot.Web.Ui.Events.EventClickEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
        txtEditShiftName.Text = "";
        DataTable dtShiftAllDate = ObjDa.return_DataTable("select  Att_ScheduleDescription.Trans_Id, Att_ShiftManagement.Shift_Name, Att_ScheduleDescription.Shift_Id from     Att_ScheduleDescription LEFT OUTER JOIN  Att_ShiftManagement ON Att_ScheduleDescription.Shift_Id = Att_ShiftManagement.Shift_Id  where Att_ScheduleDescription.Trans_Id=" + e.Value.ToString() + "");
        if (dtShiftAllDate.Rows.Count > 0)
        {
            ViewState["Shift_Id"] = dtShiftAllDate.Rows[0]["Shift_Id"].ToString();
            hdnShiftId.Value = dtShiftAllDate.Rows[0]["Trans_Id"].ToString();

            if (dtShiftAllDate.Rows[0]["Shift_Id"].ToString() != "0")
            {
                txtEditShiftName.Text = dtShiftAllDate.Rows[0]["Shift_Name"].ToString() + "/" + dtShiftAllDate.Rows[0]["Shift_Id"].ToString();

            }
        }
    }
    protected void lnkshiftName_Command(object sender, CommandEventArgs e)
    {
        txtEditShiftName.Text = "";

        ViewState["Shift_Id"] = "0";
        hdnShiftId.Value = "0";

        txtFrom.Text = Convert.ToDateTime(e.CommandName.ToString()).ToString(objSys.SetDateFormat());
        txtTo.Text = Convert.ToDateTime(e.CommandName.ToString()).ToString(objSys.SetDateFormat());
        DataTable dtShiftAllDate = ObjDa.return_DataTable("select  Att_ScheduleDescription.Trans_Id, Att_ShiftManagement.Shift_Name, Att_ScheduleDescription.Shift_Id from     Att_ScheduleDescription LEFT OUTER JOIN  Att_ShiftManagement ON Att_ScheduleDescription.Shift_Id = Att_ShiftManagement.Shift_Id  where Att_ScheduleDescription.Trans_Id=" + e.CommandArgument.ToString() + "");

        if (dtShiftAllDate.Rows.Count > 0)
        {
            ViewState["Shift_Id"] = dtShiftAllDate.Rows[0]["Shift_Id"].ToString();
            hdnShiftId.Value = dtShiftAllDate.Rows[0]["Trans_Id"].ToString();

            if (dtShiftAllDate.Rows[0]["Shift_Id"].ToString() != "0")
            {
                txtEditShiftName.Text = dtShiftAllDate.Rows[0]["Shift_Name"].ToString() + "/" + dtShiftAllDate.Rows[0]["Shift_Id"].ToString();

            }
        }

        txtEditShiftName.Focus();

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEditShiftName);


        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);


    }
    public string GetMonthName(DateTime dtdate)
    {
        string monthName = string.Empty;
        System.Globalization.DateTimeFormatInfo mfi = new
System.Globalization.DateTimeFormatInfo();
        monthName = mfi.GetMonthName(dtdate.Month).ToString().Substring(0, 3);

        return monthName;

    }

    protected void btnSaveShfitEmployee_Click(object sender, EventArgs e)
    {
        string strdatecell = string.Empty;
        if (txtFDate_upload.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFDate_upload.Text);
            }
            catch
            {
                DisplayMessage("Invalid From date");
                txtFDate_upload.Focus();
                return;
            }
        }
        if (txtTDate_upload.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtTDate_upload.Text);
            }
            catch
            {
                DisplayMessage("Invalid To date");
                txtTDate_upload.Focus();
                return;
            }
        }

        if (Convert.ToDateTime(txtFDate_upload.Text) > Convert.ToDateTime(txtTDate_upload.Text))
        {
            DisplayMessage("From date should be less then or equal to to date");
            txtFDate_upload.Focus();
            return;
        }


        string strDeptId = string.Empty;

        Session["deptFilter"] = null;

        foreach (TreeNode ModuleNode in TreeViewDepartment.Nodes)
        {
            if (ModuleNode.Checked)
            {
                Session["deptFilter"] += ModuleNode.Value + ",";
                //objLocDept.InsertLocationDepartmentMaster(editid.Value, ModuleNode.Value, "0", "", "", "", "", "", "", "", "", "True", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "True", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                childNodeSave(ModuleNode);
            }
        }

        if (Session["deptFilter"] != null)
        {
            strDeptId = Session["deptFilter"].ToString();
            strDeptId = strDeptId.Substring(0, strDeptId.Length - 1);
        }



        DataTable dt = new DataTable();

        dt.Columns.Add("EmployeeCode");
        dt.Columns.Add("EmployeeName");
        dt.Columns.Add("Department");
        dt.Columns.Add("Designation");
        //dt.Columns.Add("Month", typeof(float));
        //dt.Columns.Add("Year", typeof(float));

        DateTime dtFromdate = Convert.ToDateTime(txtFDate_upload.Text);
        DateTime dttOdate = Convert.ToDateTime(txtTDate_upload.Text);
        string strleaveId = string.Empty;

        while (dtFromdate <= dttOdate)
        {
            strdatecell = dtFromdate.ToString(objSys.SetDateFormat());
            dt.Columns.Add(strdatecell, typeof(string));
            dtFromdate = dtFromdate.AddDays(1);
        }

        dt.Rows.Add();

        dt.Rows[0][0] = "";
        for (int i = 4; i < dt.Columns.Count; i++)
        {
            dt.Rows[0][i] = Convert.ToDateTime(dt.Columns[i].ColumnName.ToString()).DayOfWeek.ToString();
        }

        DataTable dtEmp = new DataTable();

        if (strDeptId == "")
        {
            dtEmp = ObjDa.return_DataTable("select Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_DepartmentMaster.Dep_Name,Set_DesignationMaster.Designation from Set_EmployeeMaster left join set_departmentmaster on Set_EmployeeMaster.Department_Id= Set_DepartmentMaster.Dep_Id left join Set_DesignationMaster on Set_EmployeeMaster.Designation_Id= Set_DesignationMaster.Designation_Id  where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Set_EmployeeMaster.isactive='True' and Set_EmployeeMaster.Field2='False' and Set_EmployeeMaster.Emp_Type='On Role'");
        }
        else
        {
            dtEmp = ObjDa.return_DataTable("select Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_DepartmentMaster.Dep_Name,Set_DesignationMaster.Designation from Set_EmployeeMaster left join set_departmentmaster on Set_EmployeeMaster.Department_Id= Set_DepartmentMaster.Dep_Id left join Set_DesignationMaster on Set_EmployeeMaster.Designation_Id= Set_DesignationMaster.Designation_Id where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Set_EmployeeMaster.isactive='True' and Set_EmployeeMaster.Field2='False' and Set_EmployeeMaster.Emp_Type='On Role' and Set_EmployeeMaster.Department_id in (" + strDeptId + ")");
        }

        foreach (DataRow dr in dtEmp.Rows)
        {
            dt.Rows.Add();

            dt.Rows[dt.Rows.Count - 1][0] = dr["Emp_Code"].ToString();
            dt.Rows[dt.Rows.Count - 1][1] = dr["Emp_Name"].ToString();
            dt.Rows[dt.Rows.Count - 1][2] = dr["Dep_Name"].ToString();
            dt.Rows[dt.Rows.Count - 1][3] = dr["Designation"].ToString();
            for (int j = 4; j < dt.Columns.Count; j++)
            {
                dt.Rows[dt.Rows.Count - 1][dt.Columns[j].ColumnName.ToString()] = IsLeaveOnDate(Convert.ToDateTime(dt.Columns[j].ColumnName.ToString()).ToString(), dr["Emp_Id"].ToString());

            }

        }



        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_View_Show()", true);

        if (Hdn_File_Download.Value == "lbkuploadMonthly")
            ExportTableData(dt, "Shift Assignment Monthly");
        else
            ExportTableData(dt, "Shift Assignment Weekly");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
        Hdn_File_Download.Value = "";
    }
    public void childNodeSave(TreeNode ModuleNode)
    {
        string strDepId = string.Empty;


        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        {
            if (ObjNode.Checked)
            {
                Session["deptFilter"] += ObjNode.Value + ",";

                childNodeSave(ObjNode);
            }
        }

    }
    public string IsLeaveOnDate(string Date, string EmpId)
    {
        string LeaveTRansId = "";
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Date", Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = ObjDa.Reuturn_Datatable_Search("sp_Att_Leave_Request_IsLeave", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            LeaveTRansId = Common.GetleaveNameById(dtInfo.Rows[0]["Leave_Type_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
        }
        else
        {
            LeaveTRansId = GetEmployeeHolidayOnDateAndEmpId(Date, EmpId);
        }

        dtInfo.Dispose();

        return LeaveTRansId;
    }
    public string GetEmployeeHolidayOnDateAndEmpId(string date, string empid)
    {
        string strHolidayName = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Holiday_Date", date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        dtInfo = ObjDa.Reuturn_Datatable_Search("sp_Set_Employee_Holiday_EmpId", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            strHolidayName = "PH";
        }
        return strHolidayName;
    }
    protected void Lnk_Shift_Assignment_Yearly_OnClick(object sender, EventArgs e)
    {
        string strdatecell = string.Empty;
        DataTable dt = new DataTable();
        dt.Columns.Add("EmployeeCode", typeof(Int32));
        dt.Columns.Add("EmployeeName", typeof(string));
        dt.Columns.Add("FromDate", typeof(string));
        dt.Columns.Add("ToDate", typeof(string));
        dt.Columns.Add("ShiftName/LeaveType", typeof(string));
        dt.Rows.Add("1002", "Mr. Henry", "01-Jan-2017", "28-Jan-2017", "Day Shift");
        dt.Rows.Add("1002", "Mr. Abdul", "01-Feb-2017", "28-Feb-2017", "Night Shift");
        dt.Rows.Add("1003", "Mr. David", "01-Mar-2017", "31-Mar-2017", "Annual Leave");
        ExportTableData(dt, "Shift Assignment Yearly");
    }
    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void chkBxHeader_CheckedChanged(object sender, EventArgs e)
    {

        foreach (GridViewRow gvrow in GvEmpList.Rows)
        {
            CheckBox chk = ((CheckBox)gvrow.FindControl("chkBxHeader"));
            if (chk.Checked == true)
            {
                foreach (GridViewRow gvemp in GvEmpList.Rows)
                {
                    ((CheckBox)gvemp.FindControl("chkBxHeader")).Checked = true;
                }
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkBxHeader")).Checked = false;
            }
        }
    }
    #region uploadlog
    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }


        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;

        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();

        conn.Close();


    }
    protected void lbkuploadMonthly_OnClick(object sender, EventArgs e)
    {
        LinkButton button = (LinkButton)sender;
        Hdn_File_Download.Value = button.ID;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
        txtFDate_upload.Text = "";
        txtTDate_upload.Text = "";
    }
    protected void lbkuploadWeekly_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
        txtFDate_upload.Text = "";
        txtTDate_upload.Text = "";
    }
    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        int fileType = 0;

        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);

                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }

                Import(Path, fileType);
            }
        }
    }
    protected void btnConnect_Click_Shift(object sender, EventArgs e)
    {
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }

        string strHoliDayValidity = objAppParam.GetApplicationParameterValueByParamName("Holiday_Validity", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;

        try
        {
            FinancialYearMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }

        DateTime DtshiftFromDate = new DateTime();
        DateTime DtshiftToDate = new DateTime();

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }

        DataTable dtHoliday1 = new DataTable();
        DataTable dtHoliday = new DataTable();
        dtHoliday.Columns.Add("Emp_Id");
        dtHoliday.Columns.Add("Holiday_Id");
        dtHoliday.Columns.Add("Holiday_Date");
        DataTable dtLocation = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        DataTable dtShiftmaster = ObjDa.return_DataTable("select Shift_Id,Shift_Name from Att_ShiftManagement where IsActive='True'");
        DataTable dtLeavemaster = ObjDa.return_DataTable("select Att_LeaveMaster.Leave_Id,Leave_Name from Att_LeaveMaster where isactive='True'");
        DataTable dtHolidaysetup = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtLeaveDetail = GetLeaveDatatable();
        DateTime Doj = new DateTime();
        DateTime dtFromdate = new DateTime();
        DateTime dtTodate = new DateTime();
        DataTable dtLeaveSummary = new DataTable();
        DataTable dtTempLeave = new DataTable();
        DateTime dtDoj = new DateTime();
        string strLeaveValidation = string.Empty;
        string strEmpId = string.Empty;
        DataTable dtApprovalMaster = new DataTable();
        bool isbackdateEntryAllow = Inventory_Common.CheckUserPermission("59", "17", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        int Month = 0;
        int Year = 0;
        int ProbationMonth = 0;
        int Applieddaycount = 0;
        int RemainingdayCount = 0;
        bool LeaveApprovalFunctionality = false;
        bool LeaveTransaction = false;
        Session["DtHoliday"] = null;
        LeaveApprovalFunctionality = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        LeaveTransaction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Transaction_on_uploading", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        bool IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (IsProbationPeriod == true)
        {
            ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        else
        {
            ProbationMonth = 0;
        }
        bool shiftApprovalFunction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (shiftApprovalFunction)
        {
            txtuploadReferenceNo.Text = GetDocumentNumber();
        }
        int AssignHolidays = 0;

        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());

                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }


                if (dt.Columns.Contains("EmployeeCode") && dt.Columns.Contains("EmployeeName") && dt.Columns.Contains("FromDate") && dt.Columns.Contains("ToDate") && dt.Columns.Contains("ShiftName/LeaveType"))
                {
                    dtTemp.Columns.Add("EmployeeCode");
                    dtTemp.Columns.Add("EmployeeName");
                    dtTemp.Columns.Add("FromDate");
                    dtTemp.Columns.Add("ToDate");
                    dtTemp.Columns.Add("ShiftName/LeaveType");
                    dtTemp.Columns.Add("IsValid");
                }


                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");

                    dt.AcceptChanges();
                }


                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 2155)
                        {

                        }

                        dt.Rows[i]["IsValid"] = "True";

                        if (dt.Rows[i][0].ToString() == "")
                        {
                            dt.Rows[i]["IsValid"] = "";
                            continue;
                        }

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "EmployeeCode")
                            {

                                strEmpId = GetEmpInformationIdbyName(dt.Rows[i][j].ToString()).Trim();

                                if (strEmpId.Trim() == "")
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Employee)";
                                    break;
                                }
                                else
                                {
                                    Doj = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), strEmpId).Rows[0]["DOJ"].ToString());
                                }


                                if (shiftApprovalFunction)
                                {
                                    dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "59", strEmpId);

                                    if (dtApprovalMaster.Rows.Count == 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Shift approval setup Issue)";
                                        break;
                                    }

                                }


                                dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);


                            }
                            else if (dt.Columns[j].ColumnName == "ShiftName/LeaveType")
                            {
                                if (dt.Rows[i][j].ToString() == "" || dt.Rows[i][j].ToString().ToUpper() == "OFF")
                                {
                                    continue;
                                }


                                string[] strShift = GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                if (strShift[0] == "")
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Shift -'" + dt.Rows[i][j].ToString() + "')";
                                    break;
                                }

                                if (strShift[0].ToUpper() == "SHIFT")
                                {

                                    if (GetShiftDescbyName(GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster)[1]) == "")
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Shift Not Defined -'" + dt.Rows[i][j].ToString() + "')";
                                        break;
                                    }
                                }
                                else if (strShift[0].ToUpper() == "LEAVE" && Common.LeaveTransactionFunctionality(strShift[1], Session["DBConnection"].ToString()))
                                {

                                    if (ObjDa.return_DataTable("select Emp_Id from att_leave_request where Emp_Id=" + strEmpId + " and Is_approved='True' and Leave_Type_Id='" + strShift[1].Trim() + "' and '" + dtFromdate.ToString() + "' between from_date and to_date").Rows.Count == 0)
                                    {

                                        dtTempLeave = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strShift[1].Trim() + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                                        if (dtTempLeave.Rows.Count == 0)
                                        {
                                            dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + "Not Assigned)";
                                            break;
                                        }
                                        else
                                        {
                                            RemainingdayCount = GetRemainingDayCount(strEmpId, dtTempLeave, dtLeaveDetail, strShift[1].Trim());

                                        }

                                        Applieddaycount = Att_Leave_Request.GetLeaveDayCount(strEmpId, dtFromdate, dtTodate, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

                                        if (Applieddaycount <= 0)
                                        {
                                            dt.Rows[i]["IsValid"] = "False(Applying Leave on week off or holiday)";
                                            break;
                                        }
                                        strLeaveValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShift[1].Trim(), dtFromdate, dtTodate, "", ProbationMonth, IsProbationPeriod, Doj, RemainingdayCount, Applieddaycount, dtLeaveDetail, LeaveApprovalFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString());


                                        if (strLeaveValidation == "")
                                        {
                                            dtLeaveDetail = FillLeaveDatatable(strEmpId, dtLeaveDetail, strShift[1].Trim(), dtFromdate.ToString(objSys.SetDateFormat()), dtTodate.ToString(objSys.SetDateFormat()));

                                        }
                                        else
                                        {
                                            dt.Rows[i]["IsValid"] = "False(" + strLeaveValidation + " for '" + dt.Rows[i][j].ToString() + "')";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + " Already applied)";
                                        break;
                                    }

                                }

                            }
                            else if (dt.Columns[j].ColumnName == "FromDate")
                            {
                                try
                                {
                                    dtFromdate = Convert.ToDateTime(dt.Rows[i][j].ToString());

                                    dt.Rows[i][j] = Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString(objSys.SetDateFormat());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid From Date)";
                                    break;
                                }


                                if (!isbackdateEntryAllow)
                                {
                                    if (Convert.ToDateTime(dt.Rows[i][j].ToString()).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }

                                if (isLogPosted(strEmpId, Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Month.ToString(), Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                            }
                            else if (dt.Columns[j].ColumnName == "ToDate")
                            {
                                try
                                {
                                    dtTodate = Convert.ToDateTime(dt.Rows[i][j].ToString());

                                    string strdate = Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString(objSys.SetDateFormat());
                                    dt.Rows[i][j] = strdate;
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid To Date)";
                                    break;
                                }


                                if (!isbackdateEntryAllow)
                                {
                                    if (Convert.ToDateTime(dt.Rows[i][j].ToString()).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }


                                if (isLogPosted(strEmpId, Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Month.ToString(), Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                            }
                            else if (dt.Columns[j].ColumnName == "Month")
                            {
                                try
                                {
                                    Month = Convert.ToInt32(dt.Rows[i][j].ToString());

                                    if (Month <= 0 || Month > 12)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                        break;
                                    }
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                    break;
                                }

                            }
                            else if (dt.Columns[j].ColumnName == "Year")
                            {
                                try
                                {
                                    Year = Convert.ToInt32(dt.Rows[i][j].ToString());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                    break;
                                }
                            }
                            else if (dt.Columns[j].ColumnName != "Month" && dt.Columns[j].ColumnName != "Year" && dt.Columns[j].ColumnName != "IsValid" && dt.Columns[j].ColumnName != "EmployeeName" && dt.Columns[j].ColumnName != "Department" && dt.Columns[j].ColumnName != "Designation")
                            {



                                if (dt.Rows[i][j].ToString() == "" || dt.Rows[i][j].ToString().ToUpper() == "OFF")
                                {
                                    continue;
                                }

                                try
                                {
                                    dtFromdate = Convert.ToDateTime(dt.Columns[j].ColumnName.ToString());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Date Column)";
                                    break;
                                }

                                if (isLogPosted(strEmpId, dtFromdate.Month.ToString(), dtFromdate.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                                DtshiftFromDate = Convert.ToDateTime(dt.Columns[4].ColumnName.ToString());
                                DtshiftToDate = DtshiftFromDate.AddDays(dt.Columns.Count - 6);

                                if (!isbackdateEntryAllow)
                                {
                                    if (dtFromdate.Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }

                                string[] strShift = GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                if (strShift[0] == "")
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Shift-'" + dt.Rows[i][j].ToString() + "')";
                                    break;
                                }

                                if (strShift[0].ToUpper() == "SHIFT")
                                {

                                    if (GetShiftDescbyName(GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster)[1]) == "")
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Shift Not Defined-'" + dt.Rows[i][j].ToString() + "')";
                                        break;
                                    }
                                }
                                else if (strShift[0].ToUpper() == "LEAVE" && Common.LeaveTransactionFunctionality(strShift[1], Session["DBConnection"].ToString()))
                                {


                                    //if (ObjDa.return_DataTable("select Emp_Id from att_leave_request where Emp_Id=" + strEmpId + " and Is_approved='True' and Leave_Type_Id='" + strShift[1].Trim() + "' and '" + dtFromdate.ToString() + "' between from_date and to_date").Rows.Count == 0)
                                    //{

                                    dtTempLeave = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strShift[1].Trim() + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                                    if (dtTempLeave.Rows.Count == 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + "Not Assigned)";
                                        break;
                                    }
                                    else
                                    {
                                        RemainingdayCount = GetRemainingDayCount(strEmpId, dtTempLeave, dtLeaveDetail, strShift[1].Trim());
                                    }


                                    Applieddaycount = Att_Leave_Request.GetLeaveDayCount(strEmpId, dtFromdate, dtFromdate, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                    if (Applieddaycount <= 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Applying Leave on week off or holiday)";
                                        break;
                                    }

                                    strLeaveValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShift[1].Trim(), dtFromdate, dtFromdate, "", ProbationMonth, IsProbationPeriod, Doj, RemainingdayCount, 1, dtLeaveDetail, LeaveApprovalFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString(), false);

                                    if (strLeaveValidation == "")
                                    {
                                        dtLeaveDetail = FillLeaveDatatable(strEmpId, dtLeaveDetail, strShift[1].Trim(), dtFromdate.ToString(objSys.SetDateFormat()), dtFromdate.ToString(objSys.SetDateFormat()));

                                    }
                                    else
                                    {
                                        dt.Rows[i]["IsValid"] = "False(" + strLeaveValidation + " for '" + dt.Rows[i][j].ToString() + "')";
                                        break;
                                    }
                                    //}

                                }
                                else if (strShift[0].ToUpper() == "HOLIDAY")
                                {
                                    if (ObjDa.return_DataTable("SELECT Emp_Id FROM Att_Leave_Request where Emp_Id=" + strEmpId + " and  ( '" + dtFromdate + "' between From_date and To_date) and (Is_Approved='True' or Is_Pending='True') and IsActive='True'").Rows.Count > 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Leave exists on '" + dtFromdate.ToString(objSys.SetDateFormat()) + "')";
                                        break;
                                    }

                                    bool isHolidayAssigned = false;
                                    DateTime dtFromdate_Holiday = new DateTime();
                                    DateTime dtTodate_Holiday = new DateTime();

                                    dtHolidaysetup = ObjDa.return_DataTable("select Set_HolidayMaster.Holiday_Id,Set_HolidayMaster.Holiday_Name,Set_HolidayMaster.From_Date,Set_HolidayMaster.To_Date,   (DATEDIFF(day,From_Date,To_Date)+1) as HolidayCount,(select COUNT(*) from Set_Employee_Holiday where Set_Employee_Holiday.Emp_Id=" + strEmpId + " and Set_Employee_Holiday.IsActive='True'  and Set_Employee_Holiday.Holiday_Id=Set_HolidayMaster.Holiday_Id and Set_Employee_Holiday.Holiday_Date not between '" + DtshiftFromDate.Date + "' And '" + DtshiftToDate.Date + "') as UsedCount from Set_HolidayMaster where  Set_HolidayMaster.isactive='True' and      (DATEDIFF(day,Set_HolidayMaster.From_Date,Set_HolidayMaster.To_Date)+1)<>(select COUNT(*) from Set_Employee_Holiday where Set_Employee_Holiday.Emp_Id=" + strEmpId + " and Set_Employee_Holiday.IsActive='True'  and Set_Employee_Holiday.Holiday_Id=Set_HolidayMaster.Holiday_Id and Set_Employee_Holiday.Holiday_Date not between '" + DtshiftFromDate.Date + "' And '" + DtshiftToDate.Date + "')  and Set_HolidayMaster.Company_Id=" + Session["CompId"].ToString() + " order by  Set_HolidayMaster.From_Date");

                                    for (int k = 0; k < dtHolidaysetup.Rows.Count; k++)
                                    {
                                        dtFromdate_Holiday = Convert.ToDateTime(dtHolidaysetup.Rows[k]["From_Date"].ToString()).AddDays(Convert.ToInt32(dtHolidaysetup.Rows[k]["UsedCount"].ToString())).Date;
                                        dtTodate_Holiday = Convert.ToDateTime(dtHolidaysetup.Rows[k]["To_Date"].ToString()).Date;

                                        while (dtFromdate_Holiday <= dtTodate_Holiday)
                                        {
                                            if (dtFromdate_Holiday.Date > dtFromdate.Date)
                                            {
                                                dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                continue;
                                            }
                                            //}


                                            if (dtFromdate_Holiday.Date < Doj.Date)
                                            {
                                                dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                continue;
                                            }

                                            if (strHoliDayValidity.Trim() == "0")
                                            {
                                                if (dtFromdate.Year > dtFromdate_Holiday.Year)
                                                {
                                                    dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                //adding validity days
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "1")
                                                {
                                                    if (dtFromdate_Holiday.AddDays(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }

                                                }
                                                //adding validity month
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "2")
                                                {
                                                    if (dtFromdate_Holiday.AddMonths(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }

                                                }
                                                //adding validity year
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "3")
                                                {
                                                    if (dtFromdate_Holiday.AddYears(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }
                                                }

                                            }



                                            AssignHolidays = Convert.ToInt32(dtHolidaysetup.Rows[k]["HolidayCount"].ToString());

                                            //dtHoliday1 = objEmpHoliday.GetRecordbyEmpIdandHolidayId_NotequalHolidayDate(Session["CompId"].ToString(), strEmpId, dtHolidaysetup.Rows[k]["Holiday_Id"].ToString(), dtFromdate.ToString());

                                            //if (dtHoliday1.Rows.Count > 0)
                                            //{
                                            //    if (Convert.ToInt32(dtHoliday1.Rows[0]["HolidayCount"].ToString()) == dtHoliday1.Rows.Count)
                                            //    {
                                            //        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                            //        continue;
                                            //    }
                                            //}

                                            dtHoliday1 = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Id=" + dtHolidaysetup.Rows[k]["Holiday_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


                                            if (dtHoliday1.Rows.Count > 0)
                                            {
                                                if (AssignHolidays == dtHoliday1.Rows.Count)
                                                {
                                                    dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                    break;
                                                }
                                            }

                                            DataRow dr = dtHoliday.NewRow();
                                            dr[0] = strEmpId;
                                            dr[1] = dtHolidaysetup.Rows[k]["Holiday_Id"].ToString();
                                            dr[2] = dtFromdate.ToString(objSys.SetDateFormat());
                                            dtHoliday.Rows.Add(dr);
                                            isHolidayAssigned = true;
                                            break;
                                        }

                                        if (isHolidayAssigned)
                                        {
                                            break;
                                        }

                                    }

                                    if (!isHolidayAssigned)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Holiday not available on '" + dtFromdate.ToString(objSys.SetDateFormat()) + "')";
                                        break;
                                    }

                                }
                            }

                        }



                        if (dtTemp.Columns.Count == 6)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr[0] = dt.Rows[i]["EmployeeCode"].ToString();
                            dr[1] = dt.Rows[i]["EmployeeName"].ToString();
                            try
                            {
                                dr[2] = Convert.ToDateTime(dt.Rows[i]["FromDate"].ToString()).ToString(objSys.SetDateFormat());
                            }
                            catch
                            {
                                dr[2] = "";
                            }

                            try
                            {
                                dr[3] = Convert.ToDateTime(dt.Rows[i]["ToDate"].ToString()).ToString(objSys.SetDateFormat());
                            }
                            catch
                            {
                                dr[3] = "";
                            }

                            dr[4] = dt.Rows[i]["ShiftName/LeaveType"].ToString();
                            dr[5] = dt.Rows[i]["IsValid"].ToString();
                            dtTemp.Rows.Add(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message.ToString());
                }

                if (dtTemp.Columns.Count == 6)
                {
                    dt = dtTemp.Copy();
                    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
                }
                else
                {
                    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
                }

                Div_showdata.Visible = true;
                gvSelected.DataSource = dt;
                gvSelected.DataBind();

                Session["UploadDt"] = dt;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        Session["dtLeaveDetail"] = dtLeaveDetail;
        Session["dtHolidayDetail"] = dtHoliday;

        dt.Dispose();
        dtTemp.Dispose();
        dtLeaveDetail.Dispose();
        dtLeaveSummary.Dispose();
        dtTempLeave.Dispose();
        rbtnupdall.Checked = true;
        rbtnupdInValid.Checked = false;
        rbtnupdValid.Checked = false;
        //ExportTableData(dt,"ShiftDetail");
    }
    protected void btnConnect_Click_TimeTable(object sender, EventArgs e)
    {
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }

        string strHoliDayValidity = objAppParam.GetApplicationParameterValueByParamName("Holiday_Validity", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;

        try
        {
            FinancialYearMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }

        DateTime DtshiftFromDate = new DateTime();
        DateTime DtshiftToDate = new DateTime();

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }

        DataTable dtHoliday1 = new DataTable();
        DataTable dtHoliday = new DataTable();
        dtHoliday.Columns.Add("Emp_Id");
        dtHoliday.Columns.Add("Holiday_Id");
        dtHoliday.Columns.Add("Holiday_Date");
        DataTable dtLocation = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        DataTable dtTimeTable = ObjDa.return_DataTable("Select TimeTable_Id ,TimeTable_Name  From Att_TimeTable  where IsActive ='1'");
        DataTable dtShiftmaster = ObjDa.return_DataTable("select Shift_Id,Shift_Name from Att_ShiftManagement where IsActive='True'");
        DataTable dtLeavemaster = ObjDa.return_DataTable("select Att_LeaveMaster.Leave_Id,Leave_Name from Att_LeaveMaster where isactive='True'");
        DataTable dtHolidaysetup = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtLeaveDetail = GetLeaveDatatable();
        DateTime Doj = new DateTime();
        DateTime dtFromdate = new DateTime();
        DateTime dtTodate = new DateTime();
        DataTable dtLeaveSummary = new DataTable();
        DataTable dtTempLeave = new DataTable();
        DateTime dtDoj = new DateTime();
        string strLeaveValidation = string.Empty;
        string strEmpId = string.Empty;
        DataTable dtApprovalMaster = new DataTable();
        bool isbackdateEntryAllow = Inventory_Common.CheckUserPermission("59", "17", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        int Month = 0;
        int Year = 0;
        int ProbationMonth = 0;
        int Applieddaycount = 0;
        int RemainingdayCount = 0;
        bool LeaveApprovalFunctionality = false;
        bool LeaveTransaction = false;
        Session["DtHoliday"] = null;
        LeaveApprovalFunctionality = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        LeaveTransaction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Transaction_on_uploading", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        bool IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (IsProbationPeriod == true)
        {
            ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        else
        {
            ProbationMonth = 0;
        }
        bool shiftApprovalFunction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (shiftApprovalFunction)
        {
            txtuploadReferenceNo.Text = GetDocumentNumber();
        }
        int AssignHolidays = 0;

        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());

                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }


                if (dt.Columns.Contains("EmployeeCode") && dt.Columns.Contains("EmployeeName") && dt.Columns.Contains("FromDate") && dt.Columns.Contains("ToDate") && dt.Columns.Contains("ShiftName/LeaveType"))
                {
                    dtTemp.Columns.Add("EmployeeCode");
                    dtTemp.Columns.Add("EmployeeName");
                    dtTemp.Columns.Add("FromDate");
                    dtTemp.Columns.Add("ToDate");
                    dtTemp.Columns.Add("ShiftName/LeaveType");
                    dtTemp.Columns.Add("IsValid");
                }


                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");

                    dt.AcceptChanges();
                }


                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 2155)
                        {

                        }

                        dt.Rows[i]["IsValid"] = "True";

                        if (dt.Rows[i][0].ToString() == "")
                        {
                            dt.Rows[i]["IsValid"] = "";
                            continue;
                        }

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "EmployeeCode")
                            {

                                strEmpId = GetEmpInformationIdbyName(dt.Rows[i][j].ToString()).Trim();

                                if (strEmpId.Trim() == "")
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Employee)";
                                    break;
                                }
                                else
                                {
                                    Doj = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), strEmpId).Rows[0]["DOJ"].ToString());
                                }


                                if (shiftApprovalFunction)
                                {
                                    dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "59", strEmpId);

                                    if (dtApprovalMaster.Rows.Count == 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Shift approval setup Issue)";
                                        break;
                                    }

                                }


                                dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);


                            }
                            else if (dt.Columns[j].ColumnName == "ShiftName/LeaveType")
                            {
                                if (dt.Rows[i][j].ToString() == "" || dt.Rows[i][j].ToString().ToUpper() == "OFF")
                                {
                                    continue;
                                }


                                // string[] strShift = GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                string[] strShift = GetTimeTableIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtTimeTable, dtLeavemaster);

                                if (strShift[0] == "")
                                {
                                    //                                    dt.Rows[i]["IsValid"] = "False(Invalid Shift -'" + dt.Rows[i][j].ToString() + "')";
                                    dt.Rows[i]["IsValid"] = "False(Invalid TimeTable -'" + dt.Rows[i][j].ToString() + "')";
                                    break;
                                }

                                if (strShift[0].ToUpper() == "SHIFT")
                                {

                                    //if (GetShiftDescbyName(GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster)[1]) == "")
                                    //{
                                    //    dt.Rows[i]["IsValid"] = "False(Shift Not Defined -'" + dt.Rows[i][j].ToString() + "')";
                                    //    break;
                                    //}
                                }
                                else if (strShift[0].ToUpper() == "LEAVE" && Common.LeaveTransactionFunctionality(strShift[1], Session["DBConnection"].ToString()))
                                {

                                    if (ObjDa.return_DataTable("select Emp_Id from att_leave_request where Emp_Id=" + strEmpId + " and Is_approved='True' and Leave_Type_Id='" + strShift[1].Trim() + "' and '" + dtFromdate.ToString() + "' between from_date and to_date").Rows.Count == 0)
                                    {

                                        dtTempLeave = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strShift[1].Trim() + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                                        if (dtTempLeave.Rows.Count == 0)
                                        {
                                            dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + "Not Assigned)";
                                            break;
                                        }
                                        else
                                        {
                                            RemainingdayCount = GetRemainingDayCount(strEmpId, dtTempLeave, dtLeaveDetail, strShift[1].Trim());

                                        }

                                        Applieddaycount = Att_Leave_Request.GetLeaveDayCount(strEmpId, dtFromdate, dtTodate, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

                                        if (Applieddaycount <= 0)
                                        {
                                            dt.Rows[i]["IsValid"] = "False(Applying Leave on week off or holiday)";
                                            break;
                                        }
                                        strLeaveValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShift[1].Trim(), dtFromdate, dtTodate, "", ProbationMonth, IsProbationPeriod, Doj, RemainingdayCount, Applieddaycount, dtLeaveDetail, LeaveApprovalFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString());


                                        if (strLeaveValidation == "")
                                        {
                                            dtLeaveDetail = FillLeaveDatatable(strEmpId, dtLeaveDetail, strShift[1].Trim(), dtFromdate.ToString(objSys.SetDateFormat()), dtTodate.ToString(objSys.SetDateFormat()));

                                        }
                                        else
                                        {
                                            dt.Rows[i]["IsValid"] = "False(" + strLeaveValidation + " for '" + dt.Rows[i][j].ToString() + "')";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + " Already applied)";
                                        break;
                                    }

                                }

                            }
                            else if (dt.Columns[j].ColumnName == "FromDate")
                            {
                                try
                                {
                                    dtFromdate = Convert.ToDateTime(dt.Rows[i][j].ToString());

                                    dt.Rows[i][j] = Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString(objSys.SetDateFormat());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid From Date)";
                                    break;
                                }


                                if (!isbackdateEntryAllow)
                                {
                                    if (Convert.ToDateTime(dt.Rows[i][j].ToString()).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }

                                if (isLogPosted(strEmpId, Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Month.ToString(), Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                            }
                            else if (dt.Columns[j].ColumnName == "ToDate")
                            {
                                try
                                {
                                    dtTodate = Convert.ToDateTime(dt.Rows[i][j].ToString());

                                    string strdate = Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString(objSys.SetDateFormat());
                                    dt.Rows[i][j] = strdate;
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid To Date)";
                                    break;
                                }


                                if (!isbackdateEntryAllow)
                                {
                                    if (Convert.ToDateTime(dt.Rows[i][j].ToString()).Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }


                                if (isLogPosted(strEmpId, Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Month.ToString(), Convert.ToDateTime(dt.Rows[i][j].ToString()).Date.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                            }
                            else if (dt.Columns[j].ColumnName == "Month")
                            {
                                try
                                {
                                    Month = Convert.ToInt32(dt.Rows[i][j].ToString());

                                    if (Month <= 0 || Month > 12)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                        break;
                                    }
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                    break;
                                }

                            }
                            else if (dt.Columns[j].ColumnName == "Year")
                            {
                                try
                                {
                                    Year = Convert.ToInt32(dt.Rows[i][j].ToString());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Month)";
                                    break;
                                }
                            }
                            else if (dt.Columns[j].ColumnName != "Month" && dt.Columns[j].ColumnName != "Year" && dt.Columns[j].ColumnName != "IsValid" && dt.Columns[j].ColumnName != "EmployeeName" && dt.Columns[j].ColumnName != "Department" && dt.Columns[j].ColumnName != "Designation")
                            {



                                if (dt.Rows[i][j].ToString() == "" || dt.Rows[i][j].ToString().ToUpper() == "OFF")
                                {
                                    continue;
                                }

                                try
                                {
                                    dtFromdate = Convert.ToDateTime(dt.Columns[j].ColumnName.ToString());
                                }
                                catch
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid Date Column)";
                                    break;
                                }

                                if (isLogPosted(strEmpId, dtFromdate.Month.ToString(), dtFromdate.Year.ToString()))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Log Posted)";
                                    break;
                                }



                                DtshiftFromDate = Convert.ToDateTime(dt.Columns[4].ColumnName.ToString());
                                DtshiftToDate = DtshiftFromDate.AddDays(dt.Columns.Count - 6);

                                if (!isbackdateEntryAllow)
                                {
                                    if (dtFromdate.Date < Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                    {

                                        dt.Rows[i]["IsValid"] = "Back dated entry Found";
                                        break;

                                    }
                                }

                                // string[] strShift = GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                string[] strShift = GetTimeTableIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtTimeTable, dtLeavemaster);


                                if (strShift[0] == "")
                                {
                                    //dt.Rows[i]["IsValid"] = "False(Invalid Shift-'" + dt.Rows[i][j].ToString() + "')";
                                    dt.Rows[i]["IsValid"] = "False(Invalid TimeTable-'" + dt.Rows[i][j].ToString() + "')";
                                    break;
                                }

                                if (strShift[0].ToUpper() == "SHIFT")
                                {

                                    //if (GetShiftDescbyName(GetShiftIdbyName(dt.Rows[i][j].ToString(), dtLocation, dtShiftmaster, dtLeavemaster)[1]) == "")
                                    //{
                                    //    dt.Rows[i]["IsValid"] = "False(Shift Not Defined-'" + dt.Rows[i][j].ToString() + "')";
                                    //    break;
                                    //}
                                }
                                else if (strShift[0].ToUpper() == "LEAVE" && Common.LeaveTransactionFunctionality(strShift[1], Session["DBConnection"].ToString()))
                                {


                                    //if (ObjDa.return_DataTable("select Emp_Id from att_leave_request where Emp_Id=" + strEmpId + " and Is_approved='True' and Leave_Type_Id='" + strShift[1].Trim() + "' and '" + dtFromdate.ToString() + "' between from_date and to_date").Rows.Count == 0)
                                    //{

                                    dtTempLeave = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strShift[1].Trim() + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                                    if (dtTempLeave.Rows.Count == 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False('" + dt.Rows[i][j].ToString() + "'" + "Not Assigned)";
                                        break;
                                    }
                                    else
                                    {
                                        RemainingdayCount = GetRemainingDayCount(strEmpId, dtTempLeave, dtLeaveDetail, strShift[1].Trim());
                                    }


                                    Applieddaycount = Att_Leave_Request.GetLeaveDayCount(strEmpId, dtFromdate, dtFromdate, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                    if (Applieddaycount <= 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Applying Leave on week off or holiday)";
                                        break;
                                    }

                                    strLeaveValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShift[1].Trim(), dtFromdate, dtFromdate, "", ProbationMonth, IsProbationPeriod, Doj, RemainingdayCount, 1, dtLeaveDetail, LeaveApprovalFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString(), false);

                                    if (strLeaveValidation == "")
                                    {
                                        dtLeaveDetail = FillLeaveDatatable(strEmpId, dtLeaveDetail, strShift[1].Trim(), dtFromdate.ToString(objSys.SetDateFormat()), dtFromdate.ToString(objSys.SetDateFormat()));

                                    }
                                    else
                                    {
                                        dt.Rows[i]["IsValid"] = "False(" + strLeaveValidation + " for '" + dt.Rows[i][j].ToString() + "')";
                                        break;
                                    }
                                    //}

                                }
                                else if (strShift[0].ToUpper() == "HOLIDAY")
                                {
                                    if (ObjDa.return_DataTable("SELECT Emp_Id FROM Att_Leave_Request where Emp_Id=" + strEmpId + " and  ( '" + dtFromdate + "' between From_date and To_date) and (Is_Approved='True' or Is_Pending='True') and IsActive='True'").Rows.Count > 0)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Leave exists on '" + dtFromdate.ToString(objSys.SetDateFormat()) + "')";
                                        break;
                                    }

                                    bool isHolidayAssigned = false;
                                    DateTime dtFromdate_Holiday = new DateTime();
                                    DateTime dtTodate_Holiday = new DateTime();

                                    dtHolidaysetup = ObjDa.return_DataTable("select Set_HolidayMaster.Holiday_Id,Set_HolidayMaster.Holiday_Name,Set_HolidayMaster.From_Date,Set_HolidayMaster.To_Date,   (DATEDIFF(day,From_Date,To_Date)+1) as HolidayCount,(select COUNT(*) from Set_Employee_Holiday where Set_Employee_Holiday.Emp_Id=" + strEmpId + " and Set_Employee_Holiday.IsActive='True'  and Set_Employee_Holiday.Holiday_Id=Set_HolidayMaster.Holiday_Id and Set_Employee_Holiday.Holiday_Date not between '" + DtshiftFromDate.Date + "' And '" + DtshiftToDate.Date + "') as UsedCount from Set_HolidayMaster where  Set_HolidayMaster.isactive='True' and      (DATEDIFF(day,Set_HolidayMaster.From_Date,Set_HolidayMaster.To_Date)+1)<>(select COUNT(*) from Set_Employee_Holiday where Set_Employee_Holiday.Emp_Id=" + strEmpId + " and Set_Employee_Holiday.IsActive='True'  and Set_Employee_Holiday.Holiday_Id=Set_HolidayMaster.Holiday_Id and Set_Employee_Holiday.Holiday_Date not between '" + DtshiftFromDate.Date + "' And '" + DtshiftToDate.Date + "')  and Set_HolidayMaster.Company_Id=" + Session["CompId"].ToString() + " order by  Set_HolidayMaster.From_Date");

                                    for (int k = 0; k < dtHolidaysetup.Rows.Count; k++)
                                    {
                                        dtFromdate_Holiday = Convert.ToDateTime(dtHolidaysetup.Rows[k]["From_Date"].ToString()).AddDays(Convert.ToInt32(dtHolidaysetup.Rows[k]["UsedCount"].ToString())).Date;
                                        dtTodate_Holiday = Convert.ToDateTime(dtHolidaysetup.Rows[k]["To_Date"].ToString()).Date;

                                        while (dtFromdate_Holiday <= dtTodate_Holiday)
                                        {
                                            if (dtFromdate_Holiday.Date > dtFromdate.Date)
                                            {
                                                dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                continue;
                                            }
                                            //}


                                            if (dtFromdate_Holiday.Date < Doj.Date)
                                            {
                                                dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                continue;
                                            }

                                            if (strHoliDayValidity.Trim() == "0")
                                            {
                                                if (dtFromdate.Year > dtFromdate_Holiday.Year)
                                                {
                                                    dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                //adding validity days
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "1")
                                                {
                                                    if (dtFromdate_Holiday.AddDays(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }

                                                }
                                                //adding validity month
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "2")
                                                {
                                                    if (dtFromdate_Holiday.AddMonths(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }

                                                }
                                                //adding validity year
                                                if (strHoliDayValidity.Trim().Split('-')[0] == "3")
                                                {
                                                    if (dtFromdate_Holiday.AddYears(Convert.ToInt32(strHoliDayValidity.Trim().Split('-')[1])).Date < dtFromdate.Date)
                                                    {
                                                        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                        continue;
                                                    }
                                                }

                                            }



                                            AssignHolidays = Convert.ToInt32(dtHolidaysetup.Rows[k]["HolidayCount"].ToString());

                                            //dtHoliday1 = objEmpHoliday.GetRecordbyEmpIdandHolidayId_NotequalHolidayDate(Session["CompId"].ToString(), strEmpId, dtHolidaysetup.Rows[k]["Holiday_Id"].ToString(), dtFromdate.ToString());

                                            //if (dtHoliday1.Rows.Count > 0)
                                            //{
                                            //    if (Convert.ToInt32(dtHoliday1.Rows[0]["HolidayCount"].ToString()) == dtHoliday1.Rows.Count)
                                            //    {
                                            //        dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                            //        continue;
                                            //    }
                                            //}

                                            dtHoliday1 = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Id=" + dtHolidaysetup.Rows[k]["Holiday_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


                                            if (dtHoliday1.Rows.Count > 0)
                                            {
                                                if (AssignHolidays == dtHoliday1.Rows.Count)
                                                {
                                                    dtFromdate_Holiday = dtFromdate_Holiday.AddDays(1);
                                                    break;
                                                }
                                            }

                                            DataRow dr = dtHoliday.NewRow();
                                            dr[0] = strEmpId;
                                            dr[1] = dtHolidaysetup.Rows[k]["Holiday_Id"].ToString();
                                            dr[2] = dtFromdate.ToString(objSys.SetDateFormat());
                                            dtHoliday.Rows.Add(dr);
                                            isHolidayAssigned = true;
                                            break;
                                        }

                                        if (isHolidayAssigned)
                                        {
                                            break;
                                        }

                                    }

                                    if (!isHolidayAssigned)
                                    {
                                        dt.Rows[i]["IsValid"] = "False(Holiday not available on '" + dtFromdate.ToString(objSys.SetDateFormat()) + "')";
                                        break;
                                    }

                                }
                            }

                        }



                        if (dtTemp.Columns.Count == 6)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr[0] = dt.Rows[i]["EmployeeCode"].ToString();
                            dr[1] = dt.Rows[i]["EmployeeName"].ToString();
                            try
                            {
                                dr[2] = Convert.ToDateTime(dt.Rows[i]["FromDate"].ToString()).ToString(objSys.SetDateFormat());
                            }
                            catch
                            {
                                dr[2] = "";
                            }

                            try
                            {
                                dr[3] = Convert.ToDateTime(dt.Rows[i]["ToDate"].ToString()).ToString(objSys.SetDateFormat());
                            }
                            catch
                            {
                                dr[3] = "";
                            }

                            dr[4] = dt.Rows[i]["ShiftName/LeaveType"].ToString();
                            dr[5] = dt.Rows[i]["IsValid"].ToString();
                            dtTemp.Rows.Add(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message.ToString());
                }

                if (dtTemp.Columns.Count == 6)
                {
                    dt = dtTemp.Copy();
                    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
                }
                else
                {
                    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
                }

                Div_showdata.Visible = true;
                gvSelected.DataSource = dt;
                gvSelected.DataBind();

                Session["UploadDt"] = dt;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        Session["dtLeaveDetail"] = dtLeaveDetail;
        Session["dtHolidayDetail"] = dtHoliday;

        dt.Dispose();
        dtTemp.Dispose();
        dtLeaveDetail.Dispose();
        dtLeaveSummary.Dispose();
        dtTempLeave.Dispose();
        rbtnupdall.Checked = true;
        rbtnupdInValid.Checked = false;
        rbtnupdValid.Checked = false;
        //ExportTableData(dt,"ShiftDetail");
    }

    protected void btnConnect_Click(object sender, EventArgs e)
    {
        if (rOperationType.SelectedValue == "3")
        {
            if (ddlTables == null)
            {
                DisplayMessage("Sheet not found");
                return;
            }
            else if (ddlTables.Items.Count == 0)
            {
                DisplayMessage("Sheet not found");
                return;
            }

            DataTable dt = new DataTable();
            DataTable dtTemp = new DataTable();
            DataTable dtTimeTable = ObjDa.return_DataTable("Select TimeTable_Id ,TimeTable_Name  From Att_TimeTable  where IsActive ='1'");

            if (fileLoad.HasFile)
            {
                string Path = string.Empty;
                string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
                if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
                {
                    DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                    return;
                }
                else
                {
                    fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                    Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                    dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());

                    if (dt.Rows.Count == 0)
                    {
                        DisplayMessage("Record not found");
                        return;
                    }




                    if (!dt.Columns.Contains("IsValid"))
                    {
                        dt.Columns.Add("IsValid");

                        dt.AcceptChanges();
                    }


                    try
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 2155)
                            {

                            }

                            dtTemp = new DataView(dtTimeTable, "TimeTable_Name =  '" + dt.Rows[i]["TimeTable_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTemp.Rows.Count > 0)
                            {
                                dt.Rows[i]["IsValid"] = "Update";
                            }
                            else
                            {
                                dt.Rows[i]["IsValid"] = "Insert";
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage(ex.Message.ToString());
                    }


                    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
                    Div_showdata.Visible = true;
                    gvSelected.DataSource = dt;
                    gvSelected.DataBind();

                    Session["UploadDt"] = dt;
                }
            }
            else
            {
                DisplayMessage("File Not Found");
                return;
            }

            dt.Dispose();
            dtTemp.Dispose();
            rbtnupdall.Checked = true;
            rbtnupdInValid.Checked = false;
            rbtnupdValid.Checked = false;
            //ExportTableData(dt,"ShiftDetail");
        }
        else if (rOperationType.SelectedValue == "2")
        {
            btnConnect_Click_TimeTable(null, null);
        }
        else if (rOperationType.SelectedValue == "1")
        {
            btnConnect_Click_Shift(null, null);
        }

    }
    public bool isLogPosted(string strEmpId, string strMonth, string strYear)
    {
        bool Result = false;

        DataTable dtpost = objAttLog.Get_Pay_Employee_Attendance(strEmpId, strMonth, strYear);

        if (dtpost.Rows.Count > 0)
        {
            Result = true;
        }

        return Result;
    }
    public int GetRemainingDayCount(string strEmpId, DataTable dtTempLeave, DataTable dtLeaveDetail, string strLeaveTypeId)
    {
        int RemainingdayCount = 0;

        if ((Convert.ToInt32(Math.Round(Convert.ToDouble(dtTempLeave.Rows[0]["Remaining_Days"].ToString()), 0).ToString()) - Att_Leave_Request.GetleaveBalance(dtLeaveDetail, strLeaveTypeId, strEmpId)) < 0)
        {
            RemainingdayCount = 0;
        }
        else
        {
            RemainingdayCount = (Convert.ToInt32(Att_Leave_Request.GetRoundValue(dtTempLeave.Rows[0]["Remaining_Days"].ToString())) - Att_Leave_Request.GetleaveBalance(dtLeaveDetail, strLeaveTypeId, strEmpId));
        }

        return RemainingdayCount;
    }
    public DataTable GetLeaveDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_id", typeof(float));
        dt.Columns.Add("Leave_Type_id");
        dt.Columns.Add("From_date", typeof(DateTime));
        dt.Columns.Add("To_Date", typeof(DateTime));
        dt.Columns.Add("Field1");
        dt.Columns.Add("Emp_Description");
        dt.Columns.Add("Field3");
        dt.Columns.Add("LeaveCount", typeof(float));
        dt.Columns.Add("Field7", typeof(DateTime));
        dt.Columns.Add("Emp_Id");
        dt.Columns.Add("Is_Approval", typeof(bool));
        return dt;
    }
    public DataTable FillLeaveDatatable(string strEmpId, DataTable dtLeaveDetail, string strShiftId, string strFromdate, string strToDate)
    {
        dtLeaveDetail.Rows.Add();
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][0] = dtLeaveDetail.Rows.Count + 1;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][1] = strShiftId;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][2] = strFromdate;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][3] = strToDate;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][4] = "0";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][5] = "Leave Requested from shift upload functionality";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][6] = "Yearly";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][7] = "1";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][8] = Convert.ToDateTime(strToDate).AddDays(1).ToString("dd-MMM-yyyy");
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][9] = strEmpId;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][10] = Common.LeaveApprovalFunctionality(strShiftId, Session["DBConnection"].ToString());

        return dtLeaveDetail;
    }
    protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["UploadDt"];


        if (rbtnupdValid.Checked)
        {
            dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (rbtnupdInValid.Checked)
        {
            dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }

        gvSelected.DataSource = dt;
        gvSelected.DataBind();
        if (dt.Columns.Count == 6)
        {
            lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
        }
        else
        {
            lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
        }

    }
    public DataTable ConvetExcelToDataTable(string path, string strtableName)
    {
        DataTable dt = new DataTable();

        try
        {

            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + strtableName.ToString() + "]";
            adp.SelectCommand.Connection = cnn;
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {

            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                try
                {
                    dt.Columns[i].ColumnName = Convert.ToDateTime(dt.Columns[i].ColumnName).ToString(objSys.SetDateFormat());
                }
                catch
                {
                    continue;
                }
            }

        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }
    protected void btnviewcolumns_Click(object sender, EventArgs e)
    {
        if (Session["cnn"] != null)
        {

            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + ddlTables.SelectedValue.ToString() + "]";
            adp.SelectCommand.Connection = cnn;

            DataTable userTable = new DataTable();
            try
            {
                adp.Fill(userTable);
            }
            catch (Exception)
            {
                Literal l4 = new Literal();
                l4.Text = @"<font size=4 color=red></font><script>alert(""Error in Mapping File"");</script></br></br>";
                this.Controls.Add(l4);
                return;
            }
            int counter = 0;
            DataTable dtSourceData = new DataTable();
            dtSourceData = userTable.Copy();

            lblMessage.Text = "";

            Session["SourceData"] = userTable;
            DataTable dtcolumn = new DataTable();
            dtcolumn.Columns.Add("COLUMN_NAME");
            dtcolumn.Columns.Add("COLUMN");
            for (int i = 0; i < userTable.Columns.Count; i++)
            {
                dtcolumn.Rows.Add(dtcolumn.NewRow());
                if (Session["filetype"].ToString() != "excel")
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Columns[i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
                else
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Rows[0][i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
            }

            Session["SourceTbl"] = dtcolumn;
            //get destination table field 
            DataTable dtDestinationDt = GetTable();

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvFieldMapping, dtDestinationDt, "", "");

            //get source field
            div_Grid_1.Visible = true;

            div_Grid_1.Visible = true;


        }

    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Column_Name");
        dt.Columns.Add("Nec", typeof(float));

        dt.Rows.Add("Emp_Code", "0");
        dt.Rows.Add("From_Date", "1");
        dt.Rows.Add("To_Date", "2");
        dt.Rows.Add("Shift_Name", "3");
        return dt;

    }
    public string GetShiftDescbyName(string strShiftId)
    {
        string strShiftName = string.Empty;

        DataTable dt = objShiftdesc.GetShiftDescriptionByShiftId(strShiftId);

        if (dt.Rows.Count > 0)
        {
            strShiftName = "0";
        }

        return strShiftName;
    }
    public string GetEmpInformationIdbyName(string strEmpCode)
    {
        string strEmpId = string.Empty;

        DataTable dt = ObjDa.return_DataTable("select Emp_Id from Set_EmployeeMaster where Company_Id='" + Session["CompId"].ToString() + "' and  IsActive='True' and Field2='False' and Emp_Code='" + strEmpCode + "'");

        if (dt.Rows.Count > 0)
        {
            strEmpId = dt.Rows[0]["Emp_Id"].ToString();
        }

        return strEmpId;
    }

    //GetTimeTableIdbyName
    public string[] GetTimeTableIdbyName(string strShiftName, DataTable dtLocation, DataTable dtTimeTablemaster, DataTable dtLeavemaster)
    {
        DataTable dttemp = new DataTable();
        string strSeperationKey = string.Empty;
        string strShiftkey = string.Empty;
        string strlocationkey = string.Empty;
        try
        {
            strSeperationKey = ConfigurationManager.AppSettings["ShiftSeperationKey"].ToString();
        }
        catch
        {
            strSeperationKey = "#";
        }

        strShiftkey = strShiftName.Split(Convert.ToChar(strSeperationKey))[0];
        try
        {
            strlocationkey = strShiftName.Split(Convert.ToChar(strSeperationKey))[1];
        }
        catch
        {

        }

        string[] strShiftId = new string[3];

        strShiftId[0] = "";
        strShiftId[1] = "";
        strShiftId[2] = "0";

        if (strShiftName.Trim().ToUpper() == "OFF")
        {
            strShiftId[0] = "OFF";
            strShiftId[1] = "0";
        }
        else if (strShiftName.Trim().ToUpper() == "PH")
        {
            strShiftId[0] = "Holiday";
            strShiftId[1] = "0";
        }
        else
        {

            dttemp = new DataView(dtTimeTablemaster, "TimeTable_Name='" + strShiftkey + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dttemp.Rows.Count > 0)
            {
                strShiftId[0] = "Shift";
                strShiftId[1] = dttemp.Rows[0]["TimeTable_Id"].ToString();
                if (strlocationkey.Trim() != "")
                {
                    dtLocation = new DataView(dtLocation, "Location_Name_L='" + strlocationkey.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtLocation.Rows.Count > 0)
                    {
                        strShiftId[2] = dtLocation.Rows[0]["Location_Id"].ToString();
                    }
                }
            }
            else
            {
                dttemp = new DataView(dtLeavemaster, "Leave_Name='" + strShiftName + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dttemp.Rows.Count > 0)
                {
                    strShiftId[0] = "Leave";
                    strShiftId[1] = dttemp.Rows[0]["Leave_Id"].ToString();
                }

            }
        }

        return strShiftId;
    }

    public string[] GetShiftIdbyName(string strShiftName, DataTable dtLocation, DataTable dtShiftmaster, DataTable dtLeavemaster)
    {
        DataTable dttemp = new DataTable();
        string strSeperationKey = string.Empty;
        string strShiftkey = string.Empty;
        string strlocationkey = string.Empty;
        try
        {
            strSeperationKey = ConfigurationManager.AppSettings["ShiftSeperationKey"].ToString();
        }
        catch
        {
            strSeperationKey = "#";
        }

        strShiftkey = strShiftName.Split(Convert.ToChar(strSeperationKey))[0];
        try
        {
            strlocationkey = strShiftName.Split(Convert.ToChar(strSeperationKey))[1];
        }
        catch
        {

        }

        string[] strShiftId = new string[3];

        strShiftId[0] = "";
        strShiftId[1] = "";
        strShiftId[2] = "0";

        if (strShiftName.Trim().ToUpper() == "OFF")
        {
            strShiftId[0] = "OFF";
            strShiftId[1] = "0";
        }
        else if (strShiftName.Trim().ToUpper() == "PH")
        {
            strShiftId[0] = "Holiday";
            strShiftId[1] = "0";
        }
        else
        {

            dttemp = new DataView(dtShiftmaster, "Shift_Name='" + strShiftkey + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dttemp.Rows.Count > 0)
            {
                strShiftId[0] = "Shift";
                strShiftId[1] = dttemp.Rows[0]["Shift_Id"].ToString();
                if (strlocationkey.Trim() != "")
                {
                    dtLocation = new DataView(dtLocation, "Location_Name_L='" + strlocationkey.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtLocation.Rows.Count > 0)
                    {
                        strShiftId[2] = dtLocation.Rows[0]["Location_Id"].ToString();
                    }
                }
            }
            else
            {
                dttemp = new DataView(dtLeavemaster, "Leave_Name='" + strShiftName + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dttemp.Rows.Count > 0)
                {
                    strShiftId[0] = "Leave";
                    strShiftId[1] = dttemp.Rows[0]["Leave_Id"].ToString();
                }

            }
        }

        return strShiftId;
    }
    public string GetEmpInformationIdbyName(string strEmpCode, ref SqlTransaction trns)
    {
        string strEmpId = string.Empty;

        DataTable dt = ObjDa.return_DataTable("select Emp_Id from Set_EmployeeMaster where Company_Id='" + Session["CompId"].ToString() + "' and  IsActive='True' and Field2='False' and Emp_Code='" + strEmpCode + "'", ref trns);

        if (dt.Rows.Count > 0)
        {
            strEmpId = dt.Rows[0]["Emp_Id"].ToString();
        }

        return strEmpId;
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        div_Grid_1.Visible = false;
        Div_showdata.Visible = false;
        ddlTables.Items.Clear();
    }
    protected void gvFieldMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string nec = gvFieldMapping.DataKeys[e.Row.RowIndex]["Nec"].ToString();
            if (nec.Trim() == "1")
            {
                ((Label)e.Row.FindControl("lblCompulsery")).Text = "*";
                ((Label)e.Row.FindControl("lblCompulsery")).ForeColor = System.Drawing.Color.Red;
            }
            DropDownList ddl = ((DropDownList)e.Row.FindControl("ddlExcelCol"));
            binddropdownlist(ddl);
        }
    }
    private void binddropdownlist(DropDownList ddl)
    {
        DataTable dt = (DataTable)Session["SourceTbl"];

        string filetype = Session["filetype"].ToString();
        int startingrow = 0;
        if (filetype == "excel")
            startingrow = 1;
        ListItem lst = new ListItem("--select one--", "0");

        if (ddl != null)
        {
            ddl.Items.Insert(0, lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst = new ListItem(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["COLUMN"].ToString());
                ddl.Items.Insert(i + 1, lst);
                //lst=new ListItem()
            }
        }
    }


    public bool SendShiftNotification(string strTo, string From, string Paswd, string strSubject, string strMsg, string Attachment, string strAttachpath, string strSMTP, bool bEnabled, int iPort)
    {
        //return true;
        try
        {
            //using gmail
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (Attachment != "")
            {
                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath(Attachment).ToString());
                attachment = new System.Net.Mail.Attachment(strAttachpath);
                message.Attachments.Add(attachment);
            }

            message.To.Add(strTo);
            message.Subject = strSubject;
            // message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid));
            message.From = new System.Net.Mail.MailAddress(From);
            message.IsBodyHtml = true;
            message.Body = strMsg;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strSMTP);

            NetworkCredential basiccr = new NetworkCredential(From.ToString(), Paswd.ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;

            smtp.EnableSsl = Convert.ToBoolean(bEnabled);
            smtp.Port = Convert.ToInt16(iPort);

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }


        }
        catch (Exception ex)
        {

        }
        return true;
    }

    protected void btnUpload_Click1_Shift(object sender, EventArgs e)
    {

        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }


        string strLogProceEmpId = string.Empty;
        DateTime dtLogProcessFromdate = new DateTime();
        DateTime dtLogProcessTodate = new DateTime();
        bool LeaveApprovalFunctionality = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        bool LeaveTransaction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Transaction_on_uploading", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        DataTable dtHoliday = (DataTable)Session["dtHolidayDetail"];
        DataTable dtHoliday1 = new DataTable();
        DataTable dtLocation = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        DataTable dtShiftmaster = ObjDa.return_DataTable("select Shift_Id,Shift_Name from Att_ShiftManagement where IsActive='True'");
        DataTable dtLeavemaster = ObjDa.return_DataTable("select Att_LeaveMaster.Leave_Id,Leave_Name from Att_LeaveMaster where isactive='True'");
        string EmpPermission_Shift = string.Empty;
        string EmpPermission_Leave = string.Empty;
        string strTRansNo = ObjDa.return_DataTable("select isnull( MAX( CAST( Att_tmpEmpShiftSchedule.TRans_no  as int)),0)+1 from Att_tmpEmpShiftSchedule where Location_id=" + Session["Locid"].ToString() + "").Rows[0][0].ToString();
        EmpPermission_Shift = objSys.Get_Approval_Parameter_By_Name("Shift Assignment").Rows[0]["Approval_Level"].ToString();
        EmpPermission_Leave = objSys.Get_Approval_Parameter_By_Name("Leave").Rows[0]["Approval_Level"].ToString();
        bool shiftApprovalFunction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (shiftApprovalFunction && txtReferenceNo.Text == "")
        {
            DisplayMessage("Reference number not found, please configure it");
            return;
        }


        DataTable dtSch = objEmpSch.GetSheduleMaster();
        DataTable dt = (DataTable)(Session["UploadDt"]);
        DataTable dtApprovalMaster = new DataTable();
        int MaxHeaderId = 0;
        int rowaffected = 0;
        DataTable dtLeaveDetail = (DataTable)Session["dtLeaveDetail"];
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            string strEmpCode = string.Empty;
            string strFromdate = string.Empty;
            string strtodate = string.Empty;
            string strshiftname = string.Empty;
            string strIsValid = string.Empty;
            string PostedEmpList = string.Empty;
            string strEmpId = string.Empty;
            string strShiftId = string.Empty;
            string[] strShiftType = new string[2];
            string MaxLeaveId = string.Empty;
            int b = 0;
            bool IsTemp = false;
            int rem = 0;
            string OverlapDate = string.Empty;
            string[] weekdays = new string[8];
            DateTime DtFromDate = objSys.getDateForInput(strFromdate);
            DateTime DtToDate = objSys.getDateForInput(strtodate);
            DateTime DtLeaveFromDate = new DateTime();
            DateTime DtLeaveToDate = new DateTime();
            DateTime DtLeaveRejoining = new DateTime();
            DataTable dtTempsch = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtShiftD = new DataTable();

            bool Isyearly = false;
            int Month = 0;
            int Year = 0;
            weekdays[1] = "Monday";
            weekdays[2] = "Tuesday";
            weekdays[3] = "Wednesday";
            weekdays[4] = "Thursday";
            weekdays[5] = "Friday";
            weekdays[6] = "Saturday";
            weekdays[7] = "Sunday";

            string ExcludeDayAs = string.Empty;
            string CompWeekOffDay = string.Empty;

            ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);

            //Update On 25-06-2015 For Week Off Parameter
            bool strWeekOffParam = true;
            DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtWeekOffParam.Rows.Count > 0)
            {
                strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
            }
            else
            {
                strWeekOffParam = true;
            }


            string WeekOff = string.Empty;
            if (strWeekOffParam == true)
            {
                CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            }

            //Send Email Code

            if (dt.Rows.Count > 0)
            {
                //GetLogin User Email

                //


                string strLog = "Company id : " + Session["CompId"].ToString() + " Brand Id : " + HttpContext.Current.Session["BrandId"].ToString() + " Location Id :   " + HttpContext.Current.Session["LocId"].ToString();
                strLog += " Shift Upload By  : " + Session["UserId"].ToString();
                // Get Approval Memeber and Check his email   and 
                strLog += txtuploadReferenceNo.Text;


                try
                {
                    string strFromEmail = objAppParam.GetApplicationParameterValueByParamName("Master_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strFromPassword = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strSMTP = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strPort = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strIsEnable = objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    //Select  Set_EmployeeMaster.Email_Id , Emp_Name   From  Set_Approval_Employee   INNER JOIN Set_EmployeeMaster  on Set_EmployeeMaster.Emp_Id = Set_Approval_Employee.Emp_Id  Where Approval_Id =17  and Set_Approval_Employee.Location_Id = 9  

                    DataTable dtUplaodLoc = ObjDa.return_DataTable("Select  * From Set_LocationMaster Where  Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'", ref trns);



                    string strTo = "";
                    string strBody = "";
                    strBody = "Dear Team";
                    strBody += "<br/> Approval Employee List ";

                    DataTable dtLocationApp = ObjDa.return_DataTable("Select  Set_EmployeeMaster.Email_Id , Emp_Name   From  Set_Approval_Employee   INNER JOIN Set_EmployeeMaster  on Set_EmployeeMaster.Emp_Id = Set_Approval_Employee.Emp_Id  Where Approval_Id =17  and Set_Approval_Employee.Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'", ref trns);
                   

                    for (int c = 0; c < dtLocationApp.Rows.Count; c++)
                    {
                        strBody += "<br/>" + dtLocationApp.Rows[c][1].ToString();
                        strTo += dtLocationApp.Rows[c][1].ToString() + ",";

                    }
                    if (dtLocationApp.Rows.Count > 0)
                    {
                        strFromEmail = "mkt@pegasustech.net";
                        strFromPassword = "Mkt@2020";
                        strSMTP = "smtpout.secureserver.net";
                        strPort = "80";
                        strIsEnable = "false";
                        //ObjSendMailSms.SendApprovalMail("pkhatrimca@gmail.com", strFromEmail, strFromPassword , "Shift Upload Reference No : "+ txtuploadReferenceNo.Text  +" '", "", HttpContext.Current.Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                        SendShiftNotification("pkhatrimca@gmail.com", strFromEmail, strFromPassword, "Shift Upload : Location -  '"+ dtUplaodLoc.Rows[0]["Location_Name"].ToString() + "'  Reference No - " + txtuploadReferenceNo.Text + "' Uploaded By - '"+ Session["UserId"].ToString() + "'", strBody, "", "", strSMTP, Convert.ToBoolean(strIsEnable), Convert.ToInt16(strPort));
                    }
                }
                catch
                {
                }


                //


                ///

            }


            //


            for (int rowcounter = 0; rowcounter < dt.Rows.Count; rowcounter++)
            {
                strShiftId = "0";
                strEmpCode = dt.Rows[rowcounter][0].ToString();
                strEmpId = GetEmpInformationIdbyName(strEmpCode, ref trns);
                strIsValid = dt.Rows[rowcounter]["IsValid"].ToString();

                if (strIsValid != "True")
                {
                    continue;
                }

                rowaffected++;

                strLogProceEmpId += strEmpId + ",";

                if (dt.Columns.Contains("EmployeeCode") && dt.Columns.Contains("EmployeeName") && dt.Columns.Contains("FromDate") && dt.Columns.Contains("ToDate") && dt.Columns.Contains("ShiftName/LeaveType"))
                {
                    strshiftname = dt.Rows[rowcounter]["ShiftName/LeaveType"].ToString();
                    strFromdate = dt.Rows[rowcounter]["FromDate"].ToString();
                    strtodate = dt.Rows[rowcounter]["ToDate"].ToString();
                    strShiftType = GetShiftIdbyName(strshiftname, dtLocation, dtShiftmaster, dtLeavemaster);
                    strShiftId = strShiftType[1];
                    Month = objSys.getDateForInput(strFromdate).Month;
                    Year = Convert.ToDateTime(strFromdate).Year;
                    DtFromDate = objSys.getDateForInput(strFromdate);
                    DtToDate = objSys.getDateForInput(strtodate);
                    Isyearly = true;
                    dtLogProcessFromdate = DtFromDate;
                    dtLogProcessTodate = DtToDate;
                }
                else if (dt.Columns.Contains("EmployeeCode"))
                {
                    DtFromDate = Convert.ToDateTime(dt.Columns[4].ColumnName.ToString());
                    int Days = dt.Columns.Count - 5;
                    DtToDate = DtFromDate.AddDays(Days);
                    strShiftId = "";
                    Isyearly = false;
                    dtLogProcessFromdate = DtFromDate;
                    dtLogProcessTodate = DtToDate;
                }





                //if (LeaveTransaction)
                //{

                //for approval functionality true

                if (Session["dtLeaveDetail"] != null)
                {


                    dtLeaveDetail = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id=" + strEmpId + " and Is_Approval='True'", "", DataViewRowState.CurrentRows).ToTable();

                    //here we are inserting in leave approval if record exist for current employee

                    if (dtLeaveDetail.Rows.Count > 0)
                    {
                        DtLeaveFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
                        DtLeaveToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
                        DtLeaveRejoining = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());
                        DeleteLeaveTransaction(strEmpId, DtLeaveFromDate, DtLeaveToDate, ref trns);
                        dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("52", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        MaxLeaveId = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, DtLeaveFromDate, DtLeaveToDate, DtLeaveRejoining, Convert.ToInt32((DtLeaveToDate - DtLeaveFromDate).TotalDays) + 1, dtLeaveDetail, "", true, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString())[1];
                        objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtLeaveFromDate, DtLeaveToDate, true, true, ref trns, HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());

                    }


                    dtLeaveDetail = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id=" + strEmpId + " and Is_Approval='False'", "", DataViewRowState.CurrentRows).ToTable();

                    //here we are inserting in leave approval if record exist for current employee

                    if (dtLeaveDetail.Rows.Count > 0)
                    {
                        DtLeaveFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
                        DtLeaveToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
                        DtLeaveRejoining = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());

                        //first we have to delete leave if already assigned 

                        DeleteLeaveTransaction(strEmpId, DtLeaveFromDate, DtLeaveToDate, ref trns);

                        dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("52", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        MaxLeaveId = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, DtLeaveFromDate, DtLeaveToDate, DtLeaveRejoining, Convert.ToInt32((DtLeaveToDate - DtLeaveFromDate).TotalDays) + 1, dtLeaveDetail, "", false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString())[1];

                        //objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtLeaveFromDate, DtLeaveToDate, true, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());
                        InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, "", "", "", DtLeaveFromDate, DtLeaveToDate, true, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                    }

                    if (Isyearly)
                    {
                        Session["dtLeaveDetail"] = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id<>" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }


                //}


                bool isShiftassigned = false;

                if (shiftApprovalFunction)
                {
                    //inserting record for shift approval


                    while (DtFromDate <= DtToDate)
                    {
                        if (!Isyearly)
                        {
                            if (!dt.Columns.Contains(DtFromDate.ToString(objSys.SetDateFormat())))
                            {
                                DtFromDate = DtFromDate.AddDays(1);
                                continue;
                            }
                            if (dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString().Trim() == "")
                            {
                                //objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                //DtFromDate = DtFromDate.AddDays(1);
                                //continue;
                            }


                            //here we are checking the shift name for specific date
                            strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                        }

                        if (!isShiftassigned)
                        {
                            MaxHeaderId = InsertHeaderRecordForApproval(strEmpId, strTRansNo, DtFromDate.ToString(), DtToDate.ToString(), Isyearly == true ? "Yearly" : "Monthly", txtuploadReferenceNo.Text, ref trns);

                            dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("59", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                            objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Shift, dtApprovalMaster, MaxHeaderId.ToString(), dtLeaveDetail, "Emp Name", "", "", DtFromDate, DtToDate, false, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                            //objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Shift, dtApprovalMaster, MaxHeaderId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtFromDate, DtToDate, false, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                            isShiftassigned = true;
                        }

                        if (strShiftType[0].Trim() == "")
                        {
                            strShiftType[0] = "";
                            strShiftType[1] = "0";
                        }
                        else if (strShiftType[0].Trim().ToUpper() == "HOLIDAY")
                        {
                            strShiftType[1] = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Date='" + DtFromDate.ToString(objSys.SetDateFormat()) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Holiday_Id"].ToString();

                        }

                        InsertDetailRecordForApproval(MaxHeaderId.ToString(), strEmpId, DtFromDate.ToString(), strShiftType[1].ToString(), strShiftType[0].ToString(), strShiftType[2].ToString(), ref trns);

                        DtFromDate = DtFromDate.AddDays(1);
                    }


                }
                else
                {
                    //if uploading shift type is yearly and recordtype is leave then we will break this loop

                    if (Isyearly && strShiftType[0].Trim().ToUpper() == "LEAVE")
                    {
                        continue;
                    }

                    if (Isyearly && strShiftType[0].Trim().ToUpper() == "SHIFT")
                    {
                        dtTempsch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + strShiftId + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtTempsch.Rows.Count == 0)
                        {

                            b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", objSys.getDateForInput(strFromdate).ToString(), objSys.getDateForInput(strtodate).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        }
                        else
                        {

                            b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", objSys.getDateForInput(strFromdate).ToString(), objSys.getDateForInput(strtodate).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            //DisplayMessage("Shift Already Assignd to this Employee");
                            //return;

                        }
                    }



                    //if (b != 0)
                    //{
                    int days = DtToDate.Subtract(DtFromDate).Days + 1;
                    if (IsTemp == false)
                    {

                        while (DtFromDate <= DtToDate)
                        {
                            if (!Isyearly)
                            {
                                if (!dt.Columns.Contains(DtFromDate.ToString(objSys.SetDateFormat())))
                                {
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }

                                //deleting exists holiday
                                objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDateOnly(Session["CompId"].ToString(), strEmpId, DtFromDate.ToString(), ref trns);

                                //we are deleting shift for secific date

                                if (dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString().Trim() == "")
                                {
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;
                                }

                                //here we are checking the shift name for specific date
                                strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);



                                if (strShiftType[0].ToUpper() == "LEAVE")
                                {
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;
                                }
                                if (strShiftType[0].ToUpper() == "HOLIDAY")
                                {
                                    //deleting scheduled shift
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    //inserting holiday
                                    strShiftType[1] = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Date='" + DtFromDate.ToString(objSys.SetDateFormat()) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Holiday_Id"].ToString();

                                    objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), strShiftType[1].ToString(), DtFromDate.ToString(), strEmpId, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }
                                else if (strShiftType[0].ToUpper() == "OFF")
                                {
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    try
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftType[2], "0", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }
                                else
                                {
                                    strShiftId = strShiftType[1].ToString();
                                }

                                dtTempsch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + strShiftId + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                if (dtTempsch.Rows.Count == 0)
                                {

                                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                                else
                                {
                                    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    //DisplayMessage("Shift Already Assignd to this Employee");
                                    //return;
                                }
                            }

                            objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                            int counter = 0;
                            // From Date to To Date

                            dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), strShiftId, ref trns);
                            dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(strShiftId, ref trns);

                            DataTable dtTime = new DataTable();

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
                                int k = Att_ScheduleMaster.GetCycleDay(weekday);
                                int j = 1;
                                int a = k;
                                int f = 0;

                                if (k % 7 == 0)
                                {
                                    if (f != 0)
                                    {
                                        if (k % 7 == 0 && j > cycle)
                                        {
                                            j++;
                                            rem = 1;
                                        }
                                        //else
                                        //{
                                        //    j++;
                                        //}

                                        if (j > cycle)
                                        {
                                            j = 1;
                                        }
                                    }

                                }
                                f++;
                                if (k <= daysShift || j == cycle)
                                {


                                    a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());
                                    if (rem == 1 && k % 7 == 0)
                                    {
                                        j++;
                                    }

                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (k % 7 == 0 && j == cycle)
                                    {
                                        j = 1;
                                    }
                                    if (k % 7 == 0 && j < cycle)
                                    {
                                        j++;
                                    }

                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);


                                    if (dtGetTemp1.Rows.Count > 0)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dtTime.Rows.Count > 0)
                                        {
                                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                            {

                                                //here we are deleting shift for selected date 

                                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                }
                                                else
                                                {

                                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    int flag1 = 0;
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {


                                                        if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                        {
                                                            flag1 = 1;
                                                            OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                                    }
                                                }


                                            }
                                        }
                                        else
                                        {
                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                            {
                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                }

                                            }
                                            else
                                            {

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                            }

                                        }
                                    }
                                    else
                                    {
                                        //Modified accoding to excludedays parameter 19 sept 2013 kunal
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (ExcludeDayAs == "IsOff")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                            }
                                            else
                                            {
                                                // Modified By Nitin Jain On 27/08/2014....
                                                foreach (string str in CompWeekOffDay.Split(','))
                                                {
                                                    if (str == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                                //..............
                                            }

                                        }
                                        else
                                        {


                                        }
                                    }

                                    k++;
                                }
                                else
                                {
                                    k = 1;
                                    j = 1;
                                    f = 0;
                                    a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());

                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                    if (dtGetTemp1.Rows.Count > 0)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dtTime.Rows.Count > 0)
                                        {
                                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                            {
                                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                }
                                                else
                                                {

                                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    int flag1 = 0;
                                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                    {
                                                        if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                        {
                                                            flag1 = 1;
                                                        }

                                                    }
                                                    if (flag1 == 0)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                            {
                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                }

                                            }
                                            else
                                            {

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (ExcludeDayAs == "IsOff")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                            }
                                            else
                                            {
                                                // Modifed By Nitin On 27/8/2014/////
                                                foreach (string str in CompWeekOffDay.Split(','))
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;

                                }

                            }
                            else if (index == 31)
                            {
                                int daysShift = cycle * index;

                                int k = DtFromDate.Day;
                                int a = 0;
                                int j = 1;
                                int mon = DtFromDate.Month;
                                if (k <= daysShift)
                                {
                                    a = DtFromDate.Day;

                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    //
                                    if (dtGetTemp1.Rows.Count > 0)
                                    {


                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                            {

                                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                                if (dts.Rows.Count == 0)
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {


                                                            if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                            {
                                                                flag1 = 1;
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {

                                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                        {
                                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                                        }

                                                    }

                                                }
                                            }

                                        }



                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (ExcludeDayAs == "IsOff")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                            }
                                            else
                                            {
                                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                                foreach (string str in CompWeekOffDay.Split(','))
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                k++;
                                if (k > daysShift)
                                {

                                    k = 1;
                                    j = 1;
                                }
                                if (DtFromDate.Day == 1)
                                {

                                    j++;

                                }
                            }
                            else if (index == 1)
                            {
                                int k = 1;
                                int a = k;
                                int daysShift = cycle * index;
                                if (k <= daysShift)
                                {
                                    a = k;


                                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    //
                                    if (dtGetTemp1.Rows.Count > 0)
                                    {


                                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                        {
                                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                            {

                                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                                if (dts.Rows.Count == 0)
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int flag1 = 0;

                                                if (dtTime.Rows.Count > 0)
                                                {
                                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                                        {

                                                            if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, HttpContext.Current.Session["TimeZoneId"].ToString()))
                                                            {
                                                                flag1 = 1;
                                                            }

                                                        }
                                                        if (flag1 == 0)
                                                        {
                                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                    else
                                                    {

                                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                        {

                                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                                        }
                                                    }

                                                }
                                            }

                                        }

                                    }
                                    else
                                    {
                                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                        if (dts.Rows.Count == 0)
                                        {
                                            if (ExcludeDayAs == "IsOff")
                                            {

                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                            }
                                            else
                                            {
                                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                                foreach (string str in CompWeekOffDay.Split(','))
                                                {
                                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                                    }
                                                }
                                            }
                                        }
                                    }


                                }

                                k++;
                                if (k > daysShift)
                                {

                                    k = 1;

                                }

                            }

                            DtFromDate = DtFromDate.AddDays(1);
                        }
                    }
                    else
                    {//temporary

                    }
                    //}
                }
            }







            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();




        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }


        //auto log process code
        if (shiftApprovalFunction)
        {
            txtuploadReferenceNo.Text = GetDocumentNumber();
        }
        else
        {
            //ThreadStart ts = delegate () { ObjLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strLogProceEmpId, Session["userId"].ToString(), "", dtLogProcessFromdate, dtLogProcessTodate, "0"); };

            //// The thread.
            //Thread t = new Thread(ts);

            //// Run the thread.
            //t.Start();
        }

        DisplayMessage(rowaffected + " rows updated");
        btnBackToMapData_Click(null, null);

    }

    public string InsertLeaveApproval(string strCompanyId, string strBrandId, string strLocationId, string strUserId, string strEmpId, string EmpPermission, DataTable dtApproval, string strMaxId, DataTable dtLeaveDetail, string strEmpName, string hdnTransid, string strEmpImage, DateTime dtFromDate, DateTime dtToDate, bool IsLeaveRequest, bool LeaveApprovalFunctionality, ref SqlTransaction trns, string strTimeZoneId, string strAbsoluteUri)
    {
        //Set_Approval_Employee objApproalEmp = new Set_Approval_Employee(_strConString);
        //Set_ApprovalMaster ObjApproval = new Set_ApprovalMaster(_strConString);
        //SendMailSms ObjSendMailSms = new SendMailSms(_strConString);
        //EmployeeMaster objEmp = new EmployeeMaster(_strConString);
        //SystemParameter objSys = new SystemParameter(_strConString);
        //CompanyMaster objComp = new CompanyMaster(_strConString);


        //Set_ApplicationParameter ObjParam = new Set_ApplicationParameter(_strConString);

        //string strRequestEmpCode = Common.GetEmployeeCode(strCompanyId, strEmpId, ref trns);
        //string strRequestEmpName = Common.GetEmployeeName(strCompanyId, strEmpId, ref trns);
        //string strRequestDepName = Common.GetDepartmentName(strCompanyId, strEmpId, ref trns);
        //string strRequestdoj = Common.GetEmployeeDateOfJoining(strCompanyId, strEmpId, ref trns);
        //string strCompanyName = objComp.GetCompanyMasterById(strCompanyId, ref trns).Rows[0]["Company_Name"].ToString();


        string strRequestEmpCode = "";
        string strRequestEmpName = "";
        string strRequestDepName = "";
        string strRequestdoj = "";
        string strCompanyName = "";


        string strStatus = "Pending";

        if (IsLeaveRequest && !LeaveApprovalFunctionality)
        {
            strStatus = "Approved";
        }


        if (dtApproval.Rows.Count > 0)
        {
            for (int j = 0; j < dtApproval.Rows.Count; j++)
            {
                int cur_trans_id = 0;
                string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, "0", "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else if (EmpPermission == "2")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else if (EmpPermission == "3")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, strLocationId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, strLocationId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                }

                // Insert Notification For Leave by  ghanshyam suthar
                //Session["PriorityEmpId"] = PriorityEmpId;
                //Session["cur_trans_id"] = cur_trans_id;

                if (IsLeaveRequest && LeaveApprovalFunctionality)
                {
                   // Set_Notification(strCompanyId, strBrandId, strLocationId, strUserId, strEmpId, PriorityEmpId, dtLeaveDetail, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat(ref trns)), dtToDate.ToString(objSys.SetDateFormat(ref trns)), ref trns, strAbsoluteUri, strTimeZoneId);

                    //--------------------------------------

                    //code for only insert mode  so added this condition 
                    if (hdnTransid == "" && Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", strCompanyId, strBrandId, strLocationId)))
                    {

                        string strPendingApproval = string.Empty;
                        //for Email Code
                        if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtApproval.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("1", ref trns).Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                        {
                            if (PriorityEmpId != "" && PriorityEmpId != "0")
                            {
                                DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(strCompanyId, PriorityEmpId, ref trns);
                                if (dtEmpDetail.Rows.Count > 0)
                                {
                                    if (dtEmpDetail.Rows[0]["Email_Id"].ToString().Trim() != "")
                                    {
                                        //if hierarchy system then we will find next level name

                                        string strhigherHodName = string.Empty;
                                        if (ObjApproval.GetApprovalMasterById("1", ref trns).Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtApproval.Rows.Count > 1)
                                        {
                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(strCompanyId, dtApproval.Rows[1]["Emp_Id"].ToString(), ref trns);
                                        }

                                        string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(strCompanyId, PriorityEmpId, ref trns) + "</h4> <hr /> Find below the pending leave application for your Approval <br /><br /> Employee Id : " + strRequestEmpCode + "<br /> Employee Name : " + strRequestEmpName + "<br /> Department : " + strRequestDepName + "<br /> Date of join :" + strRequestdoj + strPendingApproval + " " + Common.GetmailContentByLeaveTypeId(strMaxId, strEmpId, ref trns, strCompanyId) + "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + strCompanyName + "</h2> </div> </body> </html>";
                                        //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>Request for Full Day Leave Type of '" + GetLeaveTypeName(ddlLeaveType.SelectedValue) + "' By '" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "'<br />From Date '" + txtFromDate.Text + "' To Date '" + txtToDate.Text + "'<br /> and Given Reason is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                        try
                                        {
                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(ref trns, strCompanyId, strBrandId, strLocationId), Common.GetMasterEmailPassword(ref trns, strCompanyId, strBrandId, strLocationId), "Time Man: Full Day Leave Apply By '" + Common.GetEmployeeName(strCompanyId, strEmpId, ref trns) + "' From '" + dtFromDate.ToString(objSys.SetDateFormat(ref trns)) + "' To '" + dtToDate.ToString(objSys.SetDateFormat(ref trns)) + "'", MailMessage.ToString(), strCompanyId, "", "", strBrandId, strLocationId);

                                        }
                                        catch
                                        {
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Set_Notification(strCompanyId, strBrandId, strLocationId, "Superadmin", strEmpId, PriorityEmpId, null, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat(ref trns)), dtToDate.ToString(objSys.SetDateFormat(ref trns)), ref trns, strAbsoluteUri, strTimeZoneId);
                }

            }

        }

        return "";
    }


    protected void btnUpload_Click1_TimeTable(object sender, EventArgs e)
    {

        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }


        string strLogProceEmpId = string.Empty;
        DateTime dtLogProcessFromdate = new DateTime();
        DateTime dtLogProcessTodate = new DateTime();
        bool LeaveApprovalFunctionality = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        bool LeaveTransaction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Transaction_on_uploading", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        DataTable dtHoliday = (DataTable)Session["dtHolidayDetail"];
        DataTable dtHoliday1 = new DataTable();
        DataTable dtLocation = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        DataTable dtTimeTable = ObjDa.return_DataTable("Select TimeTable_Id ,TimeTable_Name  From Att_TimeTable  where IsActive ='1'");
        DataTable dtShiftmaster = ObjDa.return_DataTable("select Shift_Id,Shift_Name from Att_ShiftManagement where IsActive='True'");
        DataTable dtLeavemaster = ObjDa.return_DataTable("select Att_LeaveMaster.Leave_Id,Leave_Name from Att_LeaveMaster where isactive='True'");
        string EmpPermission_Shift = string.Empty;
        string EmpPermission_Leave = string.Empty;
        string strTRansNo = ObjDa.return_DataTable("select isnull( MAX( CAST( Att_tmpEmpShiftSchedule.TRans_no  as int)),0)+1 from Att_tmpEmpShiftSchedule where Location_id=" + Session["Locid"].ToString() + "").Rows[0][0].ToString();
        EmpPermission_Shift = objSys.Get_Approval_Parameter_By_Name("Shift Assignment").Rows[0]["Approval_Level"].ToString();
        EmpPermission_Leave = objSys.Get_Approval_Parameter_By_Name("Leave").Rows[0]["Approval_Level"].ToString();
        bool shiftApprovalFunction = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (shiftApprovalFunction && txtReferenceNo.Text == "")
        {
            DisplayMessage("Reference number not found, please configure it");
            return;
        }


        DataTable dtSch = objEmpSch.GetSheduleMaster();
        DataTable dt = (DataTable)(Session["UploadDt"]);
        DataTable dtApprovalMaster = new DataTable();
        int MaxHeaderId = 0;
        int rowaffected = 0;
        DataTable dtLeaveDetail = (DataTable)Session["dtLeaveDetail"];
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            string strEmpCode = string.Empty;
            string strFromdate = string.Empty;
            string strtodate = string.Empty;
            string strshiftname = string.Empty;
            string strIsValid = string.Empty;
            string PostedEmpList = string.Empty;
            string strEmpId = string.Empty;
            string strShiftId = string.Empty;
            string[] strShiftType = new string[2];
            string MaxLeaveId = string.Empty;
            int b = 0;
            bool IsTemp = false;
            int rem = 0;
            string OverlapDate = string.Empty;
            string[] weekdays = new string[8];
            DateTime DtFromDate = objSys.getDateForInput(strFromdate);
            DateTime DtToDate = objSys.getDateForInput(strtodate);
            DateTime DtLeaveFromDate = new DateTime();
            DateTime DtLeaveToDate = new DateTime();
            DateTime DtLeaveRejoining = new DateTime();
            DataTable dtTempsch = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtShiftD = new DataTable();

            bool Isyearly = false;
            int Month = 0;
            int Year = 0;
            weekdays[1] = "Monday";
            weekdays[2] = "Tuesday";
            weekdays[3] = "Wednesday";
            weekdays[4] = "Thursday";
            weekdays[5] = "Friday";
            weekdays[6] = "Saturday";
            weekdays[7] = "Sunday";

            string ExcludeDayAs = string.Empty;
            string CompWeekOffDay = string.Empty;

            ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);

            //Update On 25-06-2015 For Week Off Parameter
            bool strWeekOffParam = true;
            DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtWeekOffParam.Rows.Count > 0)
            {
                strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
            }
            else
            {
                strWeekOffParam = true;
            }


            string WeekOff = string.Empty;
            if (strWeekOffParam == true)
            {
                CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            }

            DataTable dtEmpDeleteValidation = new DataTable();
            dtEmpDeleteValidation.Columns.Add(new DataColumn("EmpId"));
            dtEmpDeleteValidation.Columns.Add(new DataColumn("AttDate"));


            if (dt.Rows.Count > 0)
            {
                //GetLogin User Email

                //


                string strLog = "Company id : " + Session["CompId"].ToString() + " Brand Id : " + HttpContext.Current.Session["BrandId"].ToString() + " Location Id :   " + HttpContext.Current.Session["LocId"].ToString();
                strLog += " Shift Upload By  : " + Session["UserId"].ToString();
                // Get Approval Memeber and Check his email   and 
                strLog += txtuploadReferenceNo.Text;


                try
                {
                    string strFromEmail = objAppParam.GetApplicationParameterValueByParamName("Master_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strFromPassword =Common.Decrypt( objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns));
                    string strSMTP = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strPort = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    string strIsEnable = objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
                    //Select  Set_EmployeeMaster.Email_Id , Emp_Name   From  Set_Approval_Employee   INNER JOIN Set_EmployeeMaster  on Set_EmployeeMaster.Emp_Id = Set_Approval_Employee.Emp_Id  Where Approval_Id =17  and Set_Approval_Employee.Location_Id = 9  

                    DataTable dtUplaodLoc = ObjDa.return_DataTable("Select  * From Set_LocationMaster Where  Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'", ref trns);



                    string strTo = "";
                    string strBody = "";
                    strBody = "Dear Team";
                    strBody += "Shift Uploaded Information ";
                    strBody += "<br/><br/><br/> Approval Employee List ";

                    DataTable dtLocationApp = ObjDa.return_DataTable("Select  Set_EmployeeMaster.Email_Id , Emp_Name   From  Set_Approval_Employee   INNER JOIN Set_EmployeeMaster  on Set_EmployeeMaster.Emp_Id = Set_Approval_Employee.Emp_Id  Where Approval_Id =17  and Set_Approval_Employee.Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'", ref trns);


                    for (int c = 0; c < dtLocationApp.Rows.Count; c++)
                    {
                        strBody += "<br/>" + dtLocationApp.Rows[c][1].ToString();
                        strTo += dtLocationApp.Rows[c][0].ToString() + ",";

                    }
                    if (dtLocationApp.Rows.Count > 0)
                    {
                        //strFromEmail = "mkt@pegasustech.net";
                        //strFromPassword = "Mkt@2020";
                        //strSMTP = "smtpout.secureserver.net";
                        //strPort = "80";
                        //strIsEnable = "false";
                        //ObjSendMailSms.SendApprovalMail("pkhatrimca@gmail.com", strFromEmail, strFromPassword , "Shift Upload Reference No : "+ txtuploadReferenceNo.Text  +" '", "", HttpContext.Current.Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        if(strTo.Length > 0)
                        {
                            SendShiftNotification(strTo+ ",pkhatrimca@gmail.com", strFromEmail, strFromPassword, "Shift Upload : Location -  '" + dtUplaodLoc.Rows[0]["Location_Name"].ToString() + "'  Reference No - " + txtuploadReferenceNo.Text + "' Uploaded By - '" + Session["UserId"].ToString() + "'", strBody, "", "", strSMTP, Convert.ToBoolean(strIsEnable), Convert.ToInt16(strPort));
                        }
                        else
                        {
                            SendShiftNotification("pkhatrimca@gmail.com", strFromEmail, strFromPassword, "Shift Upload : Location -  '" + dtUplaodLoc.Rows[0]["Location_Name"].ToString() + "'  Reference No - " + txtuploadReferenceNo.Text + "' Uploaded By - '" + Session["UserId"].ToString() + "'", strBody, "", "", strSMTP, Convert.ToBoolean(strIsEnable), Convert.ToInt16(strPort));
                        }

                        strFromEmail = "mkt@pegasustech.net";
                        strFromPassword = "Mkt@2020";
                        strSMTP = "smtpout.secureserver.net";
                        strPort = "80";
                        strIsEnable = "false";
                        ObjSendMailSms.SendApprovalMail("pkhatrimca@gmail.com", strFromEmail, strFromPassword, "Shift Upload Reference No : " + txtuploadReferenceNo.Text + " '", "", HttpContext.Current.Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    }
                }
                catch
                {
                }


                //


                ///

            }

            for (int rowcounter = 0; rowcounter < dt.Rows.Count; rowcounter++)
            {



                strShiftId = "0";
                strEmpCode = dt.Rows[rowcounter][0].ToString();
                strEmpId = GetEmpInformationIdbyName(strEmpCode, ref trns);
                strIsValid = dt.Rows[rowcounter]["IsValid"].ToString();

                if (strIsValid != "True")
                {
                    continue;
                }





                rowaffected++;

                strLogProceEmpId += strEmpId + ",";

                if (dt.Columns.Contains("EmployeeCode") && dt.Columns.Contains("EmployeeName") && dt.Columns.Contains("FromDate") && dt.Columns.Contains("ToDate") && dt.Columns.Contains("ShiftName/LeaveType"))
                {
                    strshiftname = dt.Rows[rowcounter]["ShiftName/LeaveType"].ToString();
                    strFromdate = dt.Rows[rowcounter]["FromDate"].ToString();
                    strtodate = dt.Rows[rowcounter]["ToDate"].ToString();
                    //strShiftType = GetShiftIdbyName(strshiftname, dtLocation, dtShiftmaster, dtLeavemaster);
                    strShiftType = GetTimeTableIdbyName(strshiftname, dtLocation, dtTimeTable, dtLeavemaster);
                    strShiftId = strShiftType[1];
                    Month = objSys.getDateForInput(strFromdate).Month;
                    Year = Convert.ToDateTime(strFromdate).Year;
                    DtFromDate = objSys.getDateForInput(strFromdate);
                    DtToDate = objSys.getDateForInput(strtodate);
                    Isyearly = true;
                    dtLogProcessFromdate = DtFromDate;
                    dtLogProcessTodate = DtToDate;
                }
                else if (dt.Columns.Contains("EmployeeCode"))
                {
                    DtFromDate = Convert.ToDateTime(dt.Columns[4].ColumnName.ToString());
                    int Days = dt.Columns.Count - 5;
                    DtToDate = DtFromDate.AddDays(Days);
                    strShiftId = "";
                    Isyearly = false;
                    dtLogProcessFromdate = DtFromDate;
                    dtLogProcessTodate = DtToDate;
                }





                //if (LeaveTransaction)
                //{

                //for approval functionality true

                if (Session["dtLeaveDetail"] != null)
                {


                    dtLeaveDetail = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id=" + strEmpId + " and Is_Approval='True'", "", DataViewRowState.CurrentRows).ToTable();

                    //here we are inserting in leave approval if record exist for current employee

                    if (dtLeaveDetail.Rows.Count > 0)
                    {
                        DtLeaveFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
                        DtLeaveToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
                        DtLeaveRejoining = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());
                        DeleteLeaveTransaction(strEmpId, DtLeaveFromDate, DtLeaveToDate, ref trns);
                        dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("52", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        MaxLeaveId = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, DtLeaveFromDate, DtLeaveToDate, DtLeaveRejoining, Convert.ToInt32((DtLeaveToDate - DtLeaveFromDate).TotalDays) + 1, dtLeaveDetail, "", true, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString())[1];
                        objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtLeaveFromDate, DtLeaveToDate, true, true, ref trns, HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());

                    }


                    dtLeaveDetail = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id=" + strEmpId + " and Is_Approval='False'", "", DataViewRowState.CurrentRows).ToTable();

                    //here we are inserting in leave approval if record exist for current employee

                    if (dtLeaveDetail.Rows.Count > 0)
                    {
                        DtLeaveFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
                        DtLeaveToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
                        DtLeaveRejoining = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());

                        //first we have to delete leave if already assigned 

                        DeleteLeaveTransaction(strEmpId, DtLeaveFromDate, DtLeaveToDate, ref trns);

                        dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("52", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        MaxLeaveId = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, DtLeaveFromDate, DtLeaveToDate, DtLeaveRejoining, Convert.ToInt32((DtLeaveToDate - DtLeaveFromDate).TotalDays) + 1, dtLeaveDetail, "", false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString())[1];
                        //objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtLeaveFromDate, DtLeaveToDate, true, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());
                        objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Leave, dtApprovalMaster, MaxLeaveId.ToString(), dtLeaveDetail, "", "", "", DtLeaveFromDate, DtLeaveToDate, true, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                    }

                    if (Isyearly)
                    {
                        Session["dtLeaveDetail"] = new DataView((DataTable)Session["dtLeaveDetail"], "Emp_Id<>" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();
                    }

                }


                //}




                bool isShiftassigned = false;

                if (shiftApprovalFunction)
                {
                    //inserting record for shift approval


                    while (DtFromDate <= DtToDate)
                    {
                        if (!Isyearly)
                        {
                            if (!dt.Columns.Contains(DtFromDate.ToString(objSys.SetDateFormat())))
                            {
                                DtFromDate = DtFromDate.AddDays(1);
                                continue;
                            }
                            if (dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString().Trim() == "")
                            {
                                //objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                //DtFromDate = DtFromDate.AddDays(1);
                                //continue;
                            }


                            //here we are checking the shift name for specific date
                            //strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                            //strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);
                            strShiftType = GetTimeTableIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtTimeTable, dtLeavemaster);
                        }

                        if (!isShiftassigned)
                        {
                            MaxHeaderId = InsertHeaderRecordForApproval(strEmpId, strTRansNo, DtFromDate.ToString(), DtToDate.ToString(), Isyearly == true ? "Yearly" : "Monthly", txtuploadReferenceNo.Text, ref trns);

                            dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid("59", strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                            //objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Shift, dtApprovalMaster, MaxHeaderId.ToString(), dtLeaveDetail, Common.GetEmployeeName(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), "", Common.GetEmployeeImage(strEmpId, ref trns, HttpContext.Current.Session["CompId"].ToString()), DtFromDate, DtToDate, false, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());
                            objleaveReq.InsertLeaveApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), strEmpId, EmpPermission_Shift, dtApprovalMaster, MaxHeaderId.ToString(), dtLeaveDetail, "Emp Name", "", "EmpImage", DtFromDate, DtToDate, false, false, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                            isShiftassigned = true;
                        }

                        if (strShiftType[0].Trim() == "")
                        {
                            strShiftType[0] = "";
                            strShiftType[1] = "0";
                        }
                        else if (strShiftType[0].Trim().ToUpper() == "HOLIDAY")
                        {
                            strShiftType[1] = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Date='" + DtFromDate.ToString(objSys.SetDateFormat()) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Holiday_Id"].ToString();

                        }

                        InsertDetailRecordForApproval(MaxHeaderId.ToString(), strEmpId, DtFromDate.ToString(), strShiftType[1].ToString(), strShiftType[0].ToString(), strShiftType[2].ToString(), ref trns);

                        DtFromDate = DtFromDate.AddDays(1);
                    }


                }
                else
                {
                    //if uploading shift type is yearly and recordtype is leave then we will break this loop



                    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, "0", "Normal Shift", DtFromDate.ToString().ToString(), DtToDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                    // Code For Approval Shift
                    MaxHeaderId = InsertHeaderRecordForApproval(strEmpId, strTRansNo, DtFromDate.ToString(), DtToDate.ToString(), Isyearly == true ? "Yearly" : "Monthly", txtuploadReferenceNo.Text, ref trns,true);


                    if (Isyearly && strShiftType[0].Trim().ToUpper() == "LEAVE")
                    {
                        continue;
                    }

                    if (Isyearly && strShiftType[0].Trim().ToUpper() == "SHIFT")
                    {
                        dtTempsch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + strShiftId + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                        //if (dtTempsch.Rows.Count == 0)
                        //{

                        //    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", objSys.getDateForInput(strFromdate).ToString(), objSys.getDateForInput(strtodate).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        //}
                        //else
                        //{

                        //    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", objSys.getDateForInput(strFromdate).ToString(), objSys.getDateForInput(strtodate).ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        //    //DisplayMessage("Shift Already Assignd to this Employee");
                        //    //return;

                        //}
                    }



                    //if (b != 0)
                    //{
                    int days = DtToDate.Subtract(DtFromDate).Days + 1;
                    if (IsTemp == false)
                    {




                        while (DtFromDate <= DtToDate)
                        {

                            if (!Isyearly)
                            {
                                if (!dt.Columns.Contains(DtFromDate.ToString(objSys.SetDateFormat())))
                                {
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }

                                //deleting exists holiday
                                objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDateOnly(Session["CompId"].ToString(), strEmpId, DtFromDate.ToString(), ref trns);

                                //we are deleting shift for secific date

                                if (dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString().Trim() == "")
                                {
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;
                                }

                                //here we are checking the shift name for specific date
                                //strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                //strShiftType = GetShiftIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtShiftmaster, dtLeavemaster);

                                strShiftType = GetTimeTableIdbyName(dt.Rows[rowcounter][DtFromDate.ToString(objSys.SetDateFormat())].ToString(), dtLocation, dtTimeTable, dtLeavemaster);



                                if (strShiftType[0].ToUpper() == "LEAVE")
                                {
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;
                                }
                                if (strShiftType[0].ToUpper() == "HOLIDAY")
                                {
                                    //deleting scheduled shift
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    //inserting holiday
                                    strShiftType[1] = new DataView(dtHoliday, "Emp_Id=" + strEmpId + " and Holiday_Date='" + DtFromDate.ToString(objSys.SetDateFormat()) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Holiday_Id"].ToString();

                                    objEmpHoliday.InsertEmployeeHolidayMaster(Session["CompId"].ToString(), strShiftType[1].ToString(), DtFromDate.ToString(), strEmpId, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }
                                else if (strShiftType[0].ToUpper() == "OFF")
                                {
                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftType[2], "0", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    DtFromDate = DtFromDate.AddDays(1);
                                    continue;

                                }
                                else
                                {
                                    strShiftId = strShiftType[1].ToString();
                                }

                                //dtTempsch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + strShiftId + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //if (dtTempsch.Rows.Count == 0)
                                //{

                                //    b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                //}
                                //else
                                //{
                                //    b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                //    //DisplayMessage("Shift Already Assignd to this Employee");
                                //    //return;
                                //}
                            }
                            // b = 52;

                            DataTable dtTempEV = new DataView(dtEmpDeleteValidation, "EmpId = '" + strEmpId + "'  and AttDate = '" + DtFromDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                            if (dtTempEV.Rows.Count == 0)
                            {
                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                DataRow rowEV = dtEmpDeleteValidation.NewRow();
                                rowEV[0] = strEmpId;
                                rowEV[1] = DtFromDate.ToString();
                                dtEmpDeleteValidation.Rows.Add(rowEV);

                            }


                            //objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            objEmpSch.InsertScheduleDescriptionByTimeTable(b.ToString(), "0", DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, strShiftType[2], "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            // For Detail Record
                            InsertDetailRecordForApproval(MaxHeaderId.ToString(), strEmpId, DtFromDate.ToString(), strShiftType[1].ToString(), strShiftType[0].ToString(), strShiftType[2].ToString(), ref trns);


                            DtFromDate = DtFromDate.AddDays(1);
                        }
                    }
                    else
                    {//temporary

                    }
                    //}
                }
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }


        //auto log process code
        if (shiftApprovalFunction)
        {
            txtuploadReferenceNo.Text = GetDocumentNumber();
        }
        else
        {
            //ThreadStart ts = delegate () { ObjLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strLogProceEmpId, Session["userId"].ToString(), "", dtLogProcessFromdate, dtLogProcessTodate, "0"); };

            //// The thread.
            //Thread t = new Thread(ts);

            //// Run the thread.
            //t.Start();
        }

        DisplayMessage(rowaffected + " rows updated");
        btnBackToMapData_Click(null, null);

    }

    protected void btnUpload_Click1(object sender, EventArgs e)
    {
        if (rOperationType.SelectedValue == "1")
        {
            btnUpload_Click1_Shift(null, null);
        }
        else if (rOperationType.SelectedValue == "2")
        {
            btnUpload_Click1_TimeTable(null, null);
        }
        else if (rOperationType.SelectedValue == "3")
        {



            if (gvSelected.Rows.Count == 0)
            {
                DisplayMessage("Record not found");
                return;
            }

            DataTable dtTimeTable = ObjDa.return_DataTable("Select TimeTable_Id ,TimeTable_Name  From Att_TimeTable  where IsActive ='1'");
            DataTable dt = (DataTable)(Session["UploadDt"]);

            int rowaffected = 0;

            SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                // Session["LocId"].ToString(), strLogProceEmpId, Session["userId"].ToString()
                string strCompId = Session["CompId"].ToString();
                string strBrandid = Session["BrandId"].ToString();
                string strLocationId = Session["LocId"].ToString();
                DateTime dDateTime = DateTime.Now;
                string strUserId = Session["userId"].ToString();

                for (int rowcounter = 0; rowcounter < dt.Rows.Count; rowcounter++)
                {
                    string strIsValid = dt.Rows[rowcounter]["IsValid"].ToString();

                    if (strIsValid == "Insert")
                    {
                        DateTime tOnTime = Convert.ToDateTime(dt.Rows[rowcounter]["On_DutyTime"].ToString());
                        DateTime tOffTime = Convert.ToDateTime(dt.Rows[rowcounter]["Off_DutyTime"].ToString());
                        tOnTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tOnTime.Hour, tOnTime.Minute, 0, 0);
                        tOffTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tOffTime.Hour, tOffTime.Minute, 0, 0);

                        int i = objTimeTable.InsertTimeTableMaster(strCompId, dt.Rows[rowcounter]["TimeTable_Name"].ToString(), dt.Rows[rowcounter]["TimeTable_Name"].ToString(), strBrandid, tOnTime, tOffTime, dt.Rows[rowcounter]["LateMin"].ToString(), dt.Rows[rowcounter]["EarlyMin"].ToString(), Convert.ToDateTime(dt.Rows[rowcounter]["Bin"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Bout"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Ein"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Eout"].ToString()).ToString("HH:mm"), dt.Rows[rowcounter]["Wmin"].ToString(), dt.Rows[rowcounter]["Bmin"].ToString(), "False", "0", "0", "", "", strLocationId, "True", dDateTime.ToString(), "True", strUserId, dDateTime.ToString(), strUserId, dDateTime.ToString());
                    }
                    else
                    {
                        DateTime tOnTime = Convert.ToDateTime(dt.Rows[rowcounter]["On_DutyTime"].ToString());
                        DateTime tOffTime = Convert.ToDateTime(dt.Rows[rowcounter]["Off_DutyTime"].ToString());
                        tOnTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tOnTime.Hour, tOnTime.Minute, 0, 0);
                        tOffTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tOffTime.Hour, tOffTime.Minute, 0, 0);

                        DataTable dtTemp = new DataView(dtTimeTable, "TimeTable_Name = '" + dt.Rows[rowcounter]["TimeTable_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        objTimeTable.UpdateTimeTableMaster(dtTemp.Rows[0][0].ToString(), strCompId, dt.Rows[rowcounter]["TimeTable_Name"].ToString(), dt.Rows[rowcounter]["TimeTable_Name"].ToString(), strBrandid, tOnTime.ToString(), tOffTime.ToString(), dt.Rows[rowcounter]["LateMin"].ToString(), dt.Rows[rowcounter]["EarlyMin"].ToString(), Convert.ToDateTime(dt.Rows[rowcounter]["Bin"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Bout"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Ein"].ToString()).ToString("HH:mm"), Convert.ToDateTime(dt.Rows[rowcounter]["Eout"].ToString()).ToString("HH:mm"), dt.Rows[rowcounter]["Wmin"].ToString(), dt.Rows[rowcounter]["Bmin"].ToString(), "False", "0", "0", "", "", strLocationId, "True", dDateTime.ToString(), "True", strUserId, dDateTime.ToString());
                    }

                    rowaffected++;

                }





                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();

            }
            catch (Exception ex)
            {
                DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {

                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }



            DisplayMessage(rowaffected + " rows updated");
            btnBackToMapData_Click(null, null);
        }

    }

    public void DeleteLeaveTransaction(string strEmpid, DateTime DtFromdate, DateTime DtTodate, ref SqlTransaction trns)
    {
        DataTable dt = ObjDa.return_DataTable("select LeaveType_Id,isnull( sum(Pendingleave),0) as PendingLeave,isnull( sum(Approvedleave),0) as AppprovedLeave,isnull(SUM(paidleave),0) as PaidLeave from ( select att_leave_request_child.LeaveType_Id, sum( case when Att_LeaveRequest_header.Is_Pending='True' then 1 else 0 end) as Pendingleave,sum(case when Att_LeaveRequest_header.Is_Approved='True' then 1 else 0 end) as Approvedleave ,sum(case when Att_Leave_Request_Child.is_paid='True' then 1 else 0 end) as Paidleave  from att_leave_request_child inner join att_leave_request on att_leave_request_child.Ref_Id = att_leave_request.Trans_Id  inner join Att_LeaveRequest_header on att_leave_request.field2=Att_LeaveRequest_header.trans_id where Att_LeaveRequest_header.Emp_Id = " + strEmpid + "  and Att_LeaveRequest_header.From_Date>='" + DtFromdate.Date.ToString() + "' and Att_LeaveRequest_header.To_Date<='" + DtTodate.Date.ToString() + "' and Att_LeaveRequest_header.isactive='True' and (Att_LeaveRequest_header.Is_Pending='True' or Att_LeaveRequest_header.Is_Approved='True') group by att_leave_request_child.LeaveType_Id,Att_LeaveRequest_header.Is_Pending,Att_LeaveRequest_header.Is_Approved,att_leave_request_child.Is_Paid)ab group by LeaveType_Id", ref trns);

        foreach (DataRow dr in dt.Rows)
        {
            double remainleave = Convert.ToDouble(dr["PendingLeave"].ToString()) + Convert.ToDouble(dr["AppprovedLeave"].ToString());

            ObjDa.execute_Command("update att_employee_leave_trans  set Remaining_Days= Remaining_Days+" + remainleave + ", used_days=used_days-" + dr["AppprovedLeave"].ToString() + ",Pending_Days=Pending_Days-" + dr["PendingLeave"].ToString() + ",Field2= cast(Field2 as int)+" + dr["Paidleave"].ToString() + "  where emp_id = " + strEmpid + " and Field3='Open' and Leave_Type_Id=" + dr["LeaveType_Id"].ToString() + "", ref trns);
        }

        ObjDa.execute_Command("delete from att_leave_request_child where ref_id in (select Trans_Id from att_leave_request where emp_id = " + strEmpid + " and Field2 in (select Trans_Id from Att_LeaveRequest_header where Emp_Id = " + strEmpid + "  and From_Date>='" + DtFromdate.Date.ToString() + "' and To_Date<='" + DtTodate.Date.ToString() + "'))", ref trns);
        ObjDa.execute_Command("delete from att_leave_request where emp_id = " + strEmpid + " and Field2 in (select Trans_Id from Att_LeaveRequest_header where Emp_Id = " + strEmpid + "  and From_Date>='" + DtFromdate.Date.ToString() + "' and To_Date<='" + DtTodate.Date.ToString() + "')", ref trns);
        ObjDa.execute_Command("delete from Set_Approval_Transaction where Approval_Id =1 and Ref_Id in (select Trans_Id from Att_LeaveRequest_header where Emp_Id = " + strEmpid + "  and From_Date>='" + DtFromdate.Date.ToString() + "' and To_Date<='" + DtTodate.Date.ToString() + "')", ref trns);
        ObjDa.execute_Command("delete from Att_LeaveRequest_header where Emp_Id = " + strEmpid + "  and From_Date>='" + DtFromdate.Date.ToString() + "' and To_Date<='" + DtTodate.Date.ToString() + "'", ref trns);

    }

    public void SetAllId()
    {
        objAttLog.CompanyId = Session["CompId"].ToString();
        objAttLog.Device_Id = "0";
        objAttLog.Verified_Type = "By Manual";
        objAttLog.IsActive = true.ToString();
        objAttLog.CreatedBy = Session["UserId"].ToString();
        objAttLog.CreatedDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        AllPageCode();
        ddlLocation.Focus();
        txtuploadReferenceNo.Text = GetDocumentNumber();
        div_Grid_1.Visible = false;
        Div_showdata.Visible = false;
        ddlTables.Items.Clear();
        lblMessage.Text = "";
        Session["cnn"] = null;

    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtDest"];

        dt = new DataView(dt, "" + ddlFiltercol.SelectedValue + "='" + txtfiltercol.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvSelected, dt, "", "");
    }
    protected void btnresetgv_Click(object sender, EventArgs e)
    {
        Div_showdata.Visible = false;
        div_Grid_1.Visible = true;
        txtfiltercol.Text = "";

    }
    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        Div_showdata.Visible = false;
        div_Grid_1.Visible = false;
        ddlTables.Items.Clear();
        fileLoad.Dispose();
        fileLoad.Attributes.Clear();
        //trmap.Visible = false;
        //trnew.Visible = false;

    }
    protected void btndownloadInvalid_Click(object sender, EventArgs e)
    {
        //this event for download inavid record excel sheet 

        if (Session["UploadDt"] == null)
        {
            DisplayMessage("Record Not found");
            return;
        }

        DataTable dt = (DataTable)(Session["UploadDt"]);

        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();

        ExportTableData(dt, "Employee_Invalid_Info");

    }
    protected void FileUploadComplete(object sender, EventArgs e)
    {
        int fileType = 0;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }

                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
    #endregion
    #region ShiftApprovalFunctionality
    public int InsertHeaderRecordForApproval(string strEmpId, string strTRansNo, string strFromdate, string strToDate, string strType, string strRefno)
    {
        int MaxId = 0;

        MaxId = objTempempshift.InsertHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strTRansNo, strFromdate, strToDate, "Pending", strType, strRefno, "", "", "", false.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


        return MaxId;
    }
    public int InsertHeaderRecordForApproval(string strEmpId, string strTRansNo, string strFromdate, string strToDate, string strType, string strRefno, ref SqlTransaction trns,bool bFlag =false)
    {
        int MaxId = 0;

        MaxId = objTempempshift.InsertHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpId, strTRansNo, strFromdate, strToDate, "Pending", strType, strRefno, "", "", "", bFlag.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


        return MaxId;
    }
    public int InsertDetailRecordForApproval(string strHeaderId, string strEmpId, string strScheduledate, string strRefId, string strRefType)
    {
        int MaxId = 0;

        MaxId = objTempempshift.InsertDetailRecord(strHeaderId, strEmpId, strScheduledate, strRefId, strRefType, "0", "", "", "", "", false.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

        return MaxId;
    }
    public int InsertDetailRecordForApproval(string strHeaderId, string strEmpId, string strScheduledate, string strRefId, string strRefType, string strLocationId, ref SqlTransaction trns)
    {
        int MaxId = 0;

        MaxId = objTempempshift.InsertDetailRecord(strHeaderId, strEmpId, strScheduledate, strRefId, strRefType, strLocationId, "", "", "", "", false.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

        return MaxId;
    }
    #endregion
    #region ShiftReport


    private void FillddlLocationReport()
    {

        ddlLocationReport.Items.Clear();
        DataTable dtLoc = new DataTable();


        bool IsStoreManager = false;
        bool IsStoreAdmin = false;
        bool IsSuperadmin = Session["EmpId"].ToString() == "0" ? true : false;

        if (!IsSuperadmin)
        {
            if (ObjDa.return_DataTable("select COUNT(*) from Set_Approval_Transaction where Emp_Id=" + Session["EmpId"].ToString() + "").Rows[0][0].ToString() != "0")
            {
                IsStoreManager = true;
            }
            if (ObjDa.return_DataTable("select COUNT(*) from Att_tmpEmpShiftSchedule where  Att_tmpEmpShiftSchedule.Is_Active='True'  and [Att_tmpEmpShiftSchedule].CreatedBY='" + Session["UserId"].ToString() + "' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'").Rows[0][0].ToString() != "0")
            {
                IsStoreAdmin = true;
            }
        }

        if (IsSuperadmin)
        {

            dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
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


        }
        else if (IsStoreManager)
        {

            dtLoc = ObjDa.return_DataTable("select distinct Att_tmpEmpShiftSchedule.location_id,Set_LocationMaster.Location_Name from Att_tmpEmpShiftSchedule left JOIN Set_LocationMaster ON [Att_tmpEmpShiftSchedule].location_id = Set_LocationMaster.Location_Id where  (([Att_tmpEmpShiftSchedule].TRans_id in (select ref_id from Set_Approval_Transaction where Emp_Id=" + Session["EmpId"].ToString() + " and Approval_Id=17 )) or ([Att_tmpEmpShiftSchedule].Location_Id=" + Session["LocId"].ToString() + ")) and Att_tmpEmpShiftSchedule.Is_Active='True' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "' order by Set_LocationMaster.Location_Name");

        }
        else if (IsStoreAdmin)
        {
            dtLoc = ObjDa.return_DataTable("select distinct Att_tmpEmpShiftSchedule.location_id,Set_LocationMaster.Location_Name from Att_tmpEmpShiftSchedule left JOIN Set_LocationMaster ON [Att_tmpEmpShiftSchedule].location_id = Set_LocationMaster.Location_Id where Att_tmpEmpShiftSchedule.CreatedBY='" + Session["UserId"].ToString() + "'  and Att_tmpEmpShiftSchedule.Is_Active='True' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "' order by Set_LocationMaster.Location_Name");
        }


        if (dtLoc.Rows.Count > 0)
        {

            ddlLocationReport.DataSource = dtLoc;
            ddlLocationReport.DataTextField = "Location_Name";
            ddlLocationReport.DataValueField = "Location_Id";
            ddlLocationReport.DataBind();

            if (ddlLocationReport.Items.FindByValue(Session["LocId"].ToString()) != null)
            {
                ddlLocationReport.SelectedValue = Session["LocId"].ToString();
            }

        }


        ddlLocationReport.Items.Insert(0, new ListItem("--Select--", "0"));


    }

    protected void ddlLocationReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LocIdReport"] = ddlLocationReport.SelectedValue;
        txtReferenceReport.Text = "";
    }

    protected void btnGet_Click(object sender, EventArgs e)
    {
        div_shiftinfo.InnerHtml = "";


        DataTable dt = GetRecord();

        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }

        StringBuilder strHrml = new StringBuilder();
        //stringbui strHrml = string.Empty;
        DataTable dtReferenceList = dt.DefaultView.ToTable(true, "ReferenceNo");
        DataSet ds = new DataSet();
        DataTable dtReferenceDetail = new DataTable();
        DataTable dtapprovaldetail = new DataTable();
        string strApprovalperson = string.Empty;
        string strApprovaldate = string.Empty;
        foreach (DataRow dr in dtReferenceList.Rows)
        {
            strApprovalperson = string.Empty;
            strApprovaldate = string.Empty;
            dtReferenceDetail = new DataView(dt, "ReferenceNo='" + dr["ReferenceNo"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            dtapprovaldetail = ObjDa.return_DataTable("select distinct Set_EmployeeMaster.Emp_Name+'/'+Set_EmployeeMaster.Emp_Code as Approval_person,cast(Set_Approval_Transaction.Status_Update_Date as DATE) as Status_Update_Date from Set_Approval_Transaction left join Set_EmployeeMaster on  Set_Approval_Transaction.Emp_Id = Set_EmployeeMaster.Emp_Id where  Set_Approval_Transaction.Approval_Id=17  and Set_Approval_Transaction.Ref_Id in (select Att_tmpEmpShiftSchedule.trans_id from Att_tmpEmpShiftSchedule where Att_tmpEmpShiftSchedule.Field2 = '" + dr["ReferenceNo"].ToString().Trim() + "')");

            if (dtapprovaldetail.Rows.Count > 0)
            {
                foreach (DataRow drapproval in dtapprovaldetail.Rows)
                {
                    strApprovalperson += drapproval["Approval_person"] + ",";
                }

                if (Convert.ToDateTime(dtapprovaldetail.Rows[0]["Status_Update_Date"].ToString()).ToString("dd-MMM-yyyy") != "01-Jan-1900")
                {
                    strApprovaldate = Convert.ToDateTime(dtapprovaldetail.Rows[0]["Status_Update_Date"].ToString()).ToString("dd-MMM-yyyy");
                }
            }

            ds = GetshiftDatatable(dtReferenceDetail);

            DataTable dtshiftheader = new DataView(ds.Tables[0], "", "Code", DataViewRowState.CurrentRows).ToTable();

            strHrml.Append("<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /><br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /><table  border='1' cellpadding='5' cellspacing='0' style='border: solid 1 Silver; font-size: x-small; width: 100%;'> ");
            strHrml.Append("<tr style='width: 100%;'>");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Location </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Ref. No. </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Status </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Uploaded By </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Uploaded Date </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Approval Person </td >");
            strHrml.Append("<td align = 'Center' bgcolor = '#F79646'  style = 'font-weight:bold; color:White; font-size: 14px;' > Action Date </td >");
            strHrml.Append("</tr>");
            strHrml.Append("<tr style='width: 100%;'>");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' >" + dtReferenceDetail.Rows[0]["Location_Name"].ToString() + "</td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' > " + dtReferenceDetail.Rows[0]["ReferenceNo"].ToString() + " </td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' > " + dtReferenceDetail.Rows[0]["Approval_Status"].ToString() + " </td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' > " + dtReferenceDetail.Rows[0]["uploaded_BY"].ToString() + " </td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' ><span style='font-size:1px;'>.</span>" + Convert.ToDateTime(dtReferenceDetail.Rows[0]["CreatedDate"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss") + " </td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' > " + strApprovalperson + " </td >");
            strHrml.Append("<td align = 'Center'  style = 'font-weight:bold; font-size: 14px;' ><span style='font-size:1px;'>.</span>" + strApprovaldate + " </td >");
            strHrml.Append("</tr>");
            strHrml.Append("</table>");
            strHrml.Append(GetHTMLContent(dtReferenceDetail.Rows[0]["ReferenceNo"].ToString(), dtshiftheader, false));
            strHrml.Append(GetHTMLContent(dtReferenceDetail.Rows[0]["ReferenceNo"].ToString(), ds.Tables[1], true));
        }

        div_shiftinfo.InnerHtml = strHrml.ToString();


        dt.Dispose();
        ds.Dispose();

    }

    public DataTable GetRecord()
    {
        DataTable dt = new DataTable();

        bool IsStoreManager = false;
        bool IsStoreAdmin = false;
        bool IsSuperadmin = Session["EmpId"].ToString() == "0" ? true : false;
        if (!IsSuperadmin)
        {
            if (ObjDa.return_DataTable("select COUNT(*) from Set_Approval_Transaction where Emp_Id=" + Session["EmpId"].ToString() + "").Rows[0][0].ToString() != "0")
            {
                IsStoreManager = true;
            }
            //if (ObjDa.return_DataTable("select COUNT(*) from Att_tmpEmpShiftSchedule where  Att_tmpEmpShiftSchedule.Is_Active='True'  and [Att_tmpEmpShiftSchedule].CreatedBY='" + Session["UserId"].ToString() + "' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'").Rows[0][0].ToString() != "0")

            if (ObjDa.return_DataTable("select COUNT(*) from Att_tmpEmpShiftSchedule where  Att_tmpEmpShiftSchedule.Is_Active='True'  and  Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'").Rows[0][0].ToString() != "0")
            {
                IsStoreAdmin = true;
            }
        }


        if (IsSuperadmin)
        {

            dt = objTempempshift.GetHeaderRecordByApproveStatus(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocationReport.SelectedValue, "0");


            dt = new DataView(dt, "approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (txtReferenceReport.Text.Trim() != "")
            {
                dt = new DataView(dt, "ReferenceNo='" + txtReferenceReport.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

        }
        else if (IsStoreManager)
        {
            dt = ObjDa.return_DataTable("SELECT  [Att_tmpEmpShiftSchedule].[trans_id],    [Att_tmpEmpShiftSchedule].[location_id],Set_LocationMaster.Location_Name,Set_EmployeeMaster.emp_name + '/' + Set_EmployeeMaster.Emp_Code AS uploaded_BY,[Att_tmpEmpShiftSchedule].[approval_status],[Att_tmpEmpShiftSchedule].[Field2] AS ReferenceNo,[Att_tmpEmpShiftSchedule].[CreatedDate] FROM [dbo].[Att_tmpEmpShiftSchedule] Left JOIN Set_LocationMaster ON [Att_tmpEmpShiftSchedule].location_id = Set_LocationMaster.Location_Id LEFT JOIN Set_EmployeeMaster ON [Att_tmpEmpShiftSchedule].CreatedBy = Set_EmployeeMaster.Emp_Code WHERE [Att_tmpEmpShiftSchedule].company_id = " + Session["CompId"].ToString() + " AND [Att_tmpEmpShiftSchedule].brand_id = " + Session["BrandId"].ToString() + "   and Att_tmpEmpShiftSchedule.Is_Active='True'  and (([Att_tmpEmpShiftSchedule].TRans_id in (select ref_id from Set_Approval_Transaction where Emp_Id=" + Session["EmpId"].ToString() + " and Approval_Id=17 )) or ([Att_tmpEmpShiftSchedule].Location_Id=" + Session["LocId"].ToString() + "))    and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'  ORDER BY Set_LocationMaster.Location_Name,[Att_tmpEmpShiftSchedule].[CreatedDate] DESC");
            //dt = ObjDa.return_DataTable("SELECT [Att_tmpEmpShiftSchedule].[company_id], [Att_tmpEmpShiftSchedule].[brand_id], [Att_tmpEmpShiftSchedule].[location_id], [Att_tmpEmpShiftSchedule].[trans_id], [Att_tmpEmpShiftSchedule].[emp_id], [Att_tmpEmpShiftSchedule].[trans_no], [Att_tmpEmpShiftSchedule].[schedule_from], [Att_tmpEmpShiftSchedule].[schedule_to], [Att_tmpEmpShiftSchedule].[approval_status], [Att_tmpEmpShiftSchedule].[Field1], [Att_tmpEmpShiftSchedule].[Field2] AS ReferenceNo, [Att_tmpEmpShiftSchedule].[Field3], [Att_tmpEmpShiftSchedule].[Field4], [Att_tmpEmpShiftSchedule].[Field5], [Att_tmpEmpShiftSchedule].[Field6], [Att_tmpEmpShiftSchedule].[Field7], [Att_tmpEmpShiftSchedule].[is_active], [Att_tmpEmpShiftSchedule].[CreatedBy], [Att_tmpEmpShiftSchedule].[CreatedDate], [Att_tmpEmpShiftSchedule].[ModifiedBy], [Att_tmpEmpShiftSchedule].[ModifiedDate], Set_LocationMaster.Location_Name, Set_EmployeeMaster.emp_name + '/' + Set_EmployeeMaster.Emp_Code AS uploaded_BY, Set_EmployeeMaster.emp_id AS uploader_Id FROM [dbo].[Att_tmpEmpShiftSchedule] Left JOIN Set_LocationMaster ON [Att_tmpEmpShiftSchedule].location_id = Set_LocationMaster.Location_Id LEFT JOIN Set_EmployeeMaster ON [Att_tmpEmpShiftSchedule].CreatedBy = Set_EmployeeMaster.Emp_Code WHERE [Att_tmpEmpShiftSchedule].company_id = " + Session["CompId"].ToString() + " AND [Att_tmpEmpShiftSchedule].brand_id = " + Session["BrandId"].ToString() + "   and Att_tmpEmpShiftSchedule.Is_Active='True' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "' and (([Att_tmpEmpShiftSchedule].TRans_id in (select ref_id from Set_Approval_Transaction where Emp_Id=" + Session["EmpId"].ToString() + " and Approval_Id=17 )) or ([Att_tmpEmpShiftSchedule].Location_Id=" + Session["LocId"].ToString() + "))  ORDER BY [Att_tmpEmpShiftSchedule].[CreatedDate] DESC");

            if (ddlLocationReport.SelectedIndex > 0)
            {
                dt = new DataView(dt, "Location_id=" + ddlLocationReport.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (txtReferenceReport.Text.Trim() != "")
            {
                dt = new DataView(dt, "ReferenceNo='" + txtReferenceReport.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else if (IsStoreAdmin)
        {
            dt = ObjDa.return_DataTable("SELECT  [Att_tmpEmpShiftSchedule].[location_id],Set_LocationMaster.Location_Name,Set_EmployeeMaster.emp_name + '/' + Set_EmployeeMaster.Emp_Code AS uploaded_BY,[Att_tmpEmpShiftSchedule].[approval_status],[Att_tmpEmpShiftSchedule].[Field2] AS ReferenceNo,[Att_tmpEmpShiftSchedule].[CreatedDate],[Att_tmpEmpShiftSchedule].[trans_id] FROM [dbo].[Att_tmpEmpShiftSchedule] Left JOIN Set_LocationMaster ON [Att_tmpEmpShiftSchedule].location_id = Set_LocationMaster.Location_Id LEFT JOIN Set_EmployeeMaster ON [Att_tmpEmpShiftSchedule].CreatedBy = Set_EmployeeMaster.Emp_Code WHERE [Att_tmpEmpShiftSchedule].company_id = " + Session["CompId"].ToString() + " AND [Att_tmpEmpShiftSchedule].brand_id = " + Session["BrandId"].ToString() + "   and Att_tmpEmpShiftSchedule.Is_Active='True'  and [Att_tmpEmpShiftSchedule].CreatedBY='" + Session["UserId"].ToString() + "' and Att_tmpEmpShiftSchedule.approval_status='" + ddlshiftstatus.SelectedValue.Trim() + "'  ORDER BY Set_LocationMaster.Location_Name, [Att_tmpEmpShiftSchedule].[CreatedDate] DESC");

            if (ddlLocationReport.SelectedIndex > 0)
            {
                dt = new DataView(dt, "Location_id=" + ddlLocationReport.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (txtReferenceReport.Text.Trim() != "")
            {
                dt = new DataView(dt, "ReferenceNo='" + txtReferenceReport.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        return dt;
    }

    public string GetHTMLContent(string strRefNO, DataTable dtshiftheader, bool isShiftSummary)
    {
        string strHrml = string.Empty;
        strHrml += "<table id='tbl_" + strRefNO + "'  border='1' cellpadding='5' cellspacing='0' style='border: solid 1 Silver; font-size: x-small; width: 100%;'> ";
        strHrml += "<tr>";
        //if (!isShiftSummary)
        //{
        //    strHrml += "<td align = 'Center'     style = 'font-weight:bold; font-size: 14px;' > <input type='checkbox' id='chkHeader_" + strRefNO + "' onchange='checkall(this);' /> </td >";
        //}
        for (int i = 0; i < dtshiftheader.Columns.Count; i++)
        {

            strHrml += "<td align = 'Center'     style = 'font-weight:bold; font-size: 14px;' > " + dtshiftheader.Columns[i].ColumnName + " </td >";
        }
        strHrml += "</tr>";


        for (int j = 0; j < dtshiftheader.Rows.Count; j++)
        {
            strHrml += "<tr>";

            //if (!isShiftSummary)
            //{
            //    strHrml += "<td align = 'Center'     style = 'font-weight:bold; font-size: 14px;' > <input type='checkbox' id='chkDetail_" + j.ToString() + "' /> </td >";
            //}


            for (int i = 0; i < dtshiftheader.Columns.Count; i++)
            {


                strHrml += "<td align = 'Center'  style = 'font-size: 13px;' > " + dtshiftheader.Rows[j][i].ToString() + " </td >";
            }
            strHrml += "</tr>";
        }

        strHrml += "</table>";

        return strHrml;
    }


    protected void btnRefRefresh_Click(object sender, EventArgs e)
    {
        txtReferenceReport.Text = "";
        div_shiftinfo.InnerHtml = "";
        txtReferenceReport.Focus();
        gvshiftsummary.DataSource = null;
        gvshiftsummary.DataBind();
        gvShiftView.DataSource = null;
        gvShiftView.DataBind();
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {


        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=ShiftSummary.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            pnlExport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }


    public DataSet GetshiftDatatable(DataTable dtApproval)
    {

        int  UType = 1;
        if(Convert.ToBoolean(dtApproval.Rows[0]["Field6"]))
        {
            UType = 2;
        }

        string strTransid = string.Empty;
        DataSet dtdataset = new DataSet();
        string strdatecell = string.Empty;
        DateTime dtFromdate = new DateTime();
        DataTable dt = new DataTable();
        DataTable dtDetail = new DataTable();
        int counter = 0;
        dt.Columns.Add("Code", typeof(float));
        dt.Columns.Add("Name");
        dt.Columns.Add("Month");
        dt.Columns.Add("Year");
        foreach (DataRow dr in dtApproval.Rows)
        {
            dt.Rows.Add();

            

            dtDetail = objTempempshift.GetDetailRecordBy_Header_Id(dr["Trans_Id"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(),UType);

            dt.Rows[dt.Rows.Count - 1]["Code"] = dtDetail.Rows[0]["Emp_Code"].ToString();
            dt.Rows[dt.Rows.Count - 1]["Name"] = dtDetail.Rows[0]["Emp_Name"].ToString();
            dt.Rows[dt.Rows.Count - 1]["Month"] = getMonth(Convert.ToDateTime(dtDetail.Rows[0]["schedule_date"].ToString()).Month).ToString();
            dt.Rows[dt.Rows.Count - 1]["Year"] = Convert.ToDateTime(dtDetail.Rows[0]["schedule_date"].ToString()).Year.ToString();
            foreach (DataRow drchild in dtDetail.Rows)
            {
                dtFromdate = Convert.ToDateTime(drchild["schedule_date"].ToString());
                strdatecell = "" + dtFromdate.Day.ToString() + "(" + dtFromdate.DayOfWeek.ToString().Substring(0, 3) + ")" + "";
                if (counter == 0)
                {
                    dt.Columns.Add(strdatecell, typeof(string));
                }

                dt.Rows[dt.Rows.Count - 1][strdatecell] = drchild["Ref_Name"].ToString();

            }

            strTransid += dr["TRans_Id"].ToString() + ",";
            counter++;
        }

        dtdataset.Merge(dt);
        DataTable dtshiftsummary = new DataTable();


        //DataTable dtShiftdetail = ObjDa.return_DataTable("select schedule_date,COUNT(ref_id) as count,MAX(Att_ShiftManagement.shift_name) as shift_name,MAX(Att_LeaveMaster.Leave_Name) as LeaveName,MAX(Set_HolidayMaster.Holiday_Name) as Holiday_Name,ref_type from Att_tmpEmpShiftSchedule inner join Att_tmpEmpShiftScheduleDetail on Att_tmpEmpShiftSchedule.trans_id=Att_tmpEmpShiftScheduleDetail.tmpEmpShiftSchedule_id left join Att_ShiftManagement on Att_tmpEmpShiftScheduleDetail.ref_id=Att_ShiftManagement.Shift_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Shift' left join Att_LeaveMaster on Att_tmpEmpShiftScheduleDetail.ref_id =Att_LeaveMaster.leave_id and Att_tmpEmpShiftScheduleDetail.ref_type='Leave' left join  set_holidaymaster on Att_tmpEmpShiftScheduleDetail.ref_id =Set_HolidayMaster.Holiday_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Holiday'  where Att_tmpEmpShiftSchedule.trans_id in (" + strTransid.Substring(0, strTransid.Length - 1) + ") and  Att_tmpEmpShiftScheduleDetail.ref_id<>0 group by Att_tmpEmpShiftScheduleDetail.schedule_date,Att_tmpEmpShiftScheduleDetail.ref_id,Att_tmpEmpShiftScheduleDetail.ref_type,Att_ShiftManagement.shift_name");
        DataTable dtShiftdetail = new DataTable();
        if (UType ==1)
        {
             dtShiftdetail = ObjDa.return_DataTable("select schedule_date,COUNT(ref_id) as count,MAX(Att_ShiftManagement.shift_name) as shift_name,MAX(Att_LeaveMaster.Leave_Name) as LeaveName,MAX(Set_HolidayMaster.Holiday_Name) as Holiday_Name,ref_type from Att_tmpEmpShiftSchedule inner join Att_tmpEmpShiftScheduleDetail on Att_tmpEmpShiftSchedule.trans_id=Att_tmpEmpShiftScheduleDetail.tmpEmpShiftSchedule_id left join Att_ShiftManagement on Att_tmpEmpShiftScheduleDetail.ref_id=Att_ShiftManagement.Shift_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Shift' left join Att_LeaveMaster on Att_tmpEmpShiftScheduleDetail.ref_id =Att_LeaveMaster.leave_id and Att_tmpEmpShiftScheduleDetail.ref_type='Leave' left join  set_holidaymaster on Att_tmpEmpShiftScheduleDetail.ref_id =Set_HolidayMaster.Holiday_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Holiday'  where Att_tmpEmpShiftSchedule.trans_id in (" + strTransid.Substring(0, strTransid.Length - 1) + ") and  Att_tmpEmpShiftScheduleDetail.ref_id<>0 group by Att_tmpEmpShiftScheduleDetail.schedule_date,Att_tmpEmpShiftScheduleDetail.ref_id,Att_tmpEmpShiftScheduleDetail.ref_type,Att_ShiftManagement.shift_name");
        }
        else
        {
            dtShiftdetail = ObjDa.return_DataTable("select schedule_date,COUNT(ref_id) as count,MAX(Att_TimeTable.TimeTable_Name) as shift_name,MAX(Att_LeaveMaster.Leave_Name) as LeaveName,MAX(Set_HolidayMaster.Holiday_Name) as Holiday_Name,ref_type from Att_tmpEmpShiftSchedule inner join Att_tmpEmpShiftScheduleDetail on Att_tmpEmpShiftSchedule.trans_id=Att_tmpEmpShiftScheduleDetail.tmpEmpShiftSchedule_id left join Att_TimeTable on Att_tmpEmpShiftScheduleDetail.ref_id=Att_TimeTable.TimeTable_Id   and Att_tmpEmpShiftScheduleDetail.ref_type='Shift' left join Att_LeaveMaster on Att_tmpEmpShiftScheduleDetail.ref_id =Att_LeaveMaster.leave_id and Att_tmpEmpShiftScheduleDetail.ref_type='Leave' left join  set_holidaymaster on Att_tmpEmpShiftScheduleDetail.ref_id =Set_HolidayMaster.Holiday_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Holiday'  where Att_tmpEmpShiftSchedule.trans_id in (" + strTransid.Substring(0, strTransid.Length - 1) + ") and  Att_tmpEmpShiftScheduleDetail.ref_id<>0 group by Att_tmpEmpShiftScheduleDetail.schedule_date,Att_tmpEmpShiftScheduleDetail.ref_id,Att_tmpEmpShiftScheduleDetail.ref_type,Att_TimeTable.TimeTable_Name");
        }
        

        if (dtShiftdetail.Rows.Count > 0)
        {

            DataTable dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Shift'", "", DataViewRowState.CurrentRows).ToTable(true, "shift_name");
            DataTable dtdistinctshiftdate = new DataView(dtShiftdetail, "", "", DataViewRowState.CurrentRows).ToTable(true, "schedule_date");
            dtshiftsummary = new DataTable();
            DataTable dtTempShiftsummary1 = new DataTable();
            dtshiftsummary.Columns.Add("Shift");
            dtshiftsummary.Columns.Add("Name");
            dtshiftsummary.Columns.Add("Month");
            dtshiftsummary.Columns.Add("Year");

            foreach (DataRow dr in dtdistinctshiftdate.Rows)
            {
                dtshiftsummary.Columns.Add(Convert.ToDateTime(dr["schedule_date"].ToString()).ToString(objSys.SetDateFormat()));
            }


            foreach (DataRow dr in dtdistinctshiftName.Rows)
            {
                dtshiftsummary.Rows.Add();

                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["Shift_Name"].ToString();
                for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                {
                    dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and Shift_Name='" + dr["Shift_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtTempShiftsummary1.Rows.Count > 0)
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                    }
                    else
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                    }
                }

            }


            dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Leave'", "", DataViewRowState.CurrentRows).ToTable(true, "LeaveName");

            foreach (DataRow dr in dtdistinctshiftName.Rows)
            {
                dtshiftsummary.Rows.Add();

                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["LeaveName"].ToString();
                for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                {
                    dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and LeaveName='" + dr["LeaveName"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtTempShiftsummary1.Rows.Count > 0)
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                    }
                    else
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                    }
                }

            }


            dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Holiday'", "", DataViewRowState.CurrentRows).ToTable(true, "Holiday_Name");

            foreach (DataRow dr in dtdistinctshiftName.Rows)
            {
                dtshiftsummary.Rows.Add();

                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["Holiday_Name"].ToString();
                for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                {
                    dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and Holiday_Name='" + dr["Holiday_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtTempShiftsummary1.Rows.Count > 0)
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                    }
                    else
                    {
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                    }
                }

            }
        }



        dtdataset.Merge(dtshiftsummary);
        return dtdataset;
    }

    public string getMonth(int MonthNumber)
    {
        string strMonthName = string.Empty;
        System.Globalization.DateTimeFormatInfo mfi = new
System.Globalization.DateTimeFormatInfo();
        strMonthName = mfi.GetMonthName(MonthNumber).ToString();
        return strMonthName;
    }
    #endregion
    protected void rbtnshiftName_CheckedChanged(object sender, EventArgs e)
    {

        if (rbtnshiftName.Checked)
        {
            txtShiftName.Text = "";
            txtShiftName.Enabled = true;
            txtShiftName.Focus();
        }
        else
        {
            txtShiftName.Text = "Week Off";
            txtShiftName.Enabled = false;

        }

    }

    protected void ddlshiftstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlLocationReport();
        txtReferenceReport.Text = "";
    }
}