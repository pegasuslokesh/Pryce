using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;

public partial class Attendance_LogProcess : BasePage
{
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    Att_AttendanceLog objAttLog = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Att_Employee_Leave objEmpleave = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Att_TimeTable objTimeTable = null;
    SystemParameter ObjSysPeram = null;
    HR_Indemnity_Master objIndemnity = null;
    Attendance objAttendance = null;
    Set_ApplicationParameter objAppParam = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_ShiftDescription objShift = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Att_Leave_Request ObjLeaveReq = null;
    EmployeeParameter objEmpParam = null;
    Att_AttendanceRegister objAttReg = null;
    Pay_Employee_Month objPayEmpMonth = null;
    LeaveMaster_deduction objLeavededuction = null;
    Att_PartialLeave_Request objPartialReq = null;
    Pay_Employee_Attendance objPayEmpAtt = null;
    Att_ShiftManagement objShiftManagement = null;
    Att_Leave_Request objLeaveReq = null;
    Common cmn = null;
    NotificationMaster Obj_Notifiacation = null;
    Pay_Employee_Penalty ObjPenalty = null;
    // Filter Criteria According to Location and Department
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    DataAccessClass Objda = null;
    LogProcess objLogProcess = null;
    PageControlCommon objPageCmn = null;
    private string _strConString = string.Empty;
    string EarlyOut_MinuteDeductionType = string.Empty;
    string LateIn_MinuteDeductionType = string.Empty;
    int Fr_IncrementDuration = 0;
    int Exp_IncrementDuration = 0;
    //------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        _strConString = Session["DBConnection"].ToString();
        objEmpSalInc = new Set_Emp_SalaryIncrement(_strConString);
        objAttLog = new Att_AttendanceLog(_strConString);
        objEmpGroup = new Set_EmployeeGroup_Master(_strConString);
        objSys = new SystemParameter(_strConString);
        objEmpleave = new Att_Employee_Leave(_strConString);
        objGroupEmp = new Set_Group_Employee(_strConString);
        objEmp = new EmployeeMaster(_strConString);
        objTimeTable = new Att_TimeTable(_strConString);
        ObjSysPeram = new SystemParameter(_strConString);
        objIndemnity = new HR_Indemnity_Master(_strConString);
        objAttendance = new Attendance(_strConString);
        objAppParam = new Set_ApplicationParameter(_strConString);
        objEmpSch = new Att_ScheduleMaster(_strConString);
        objShift = new Att_ShiftDescription(_strConString);
        objEmpHoliday = new Set_Employee_Holiday(_strConString);
        ObjLeaveReq = new Att_Leave_Request(_strConString);
        objEmpParam = new EmployeeParameter(_strConString);
        objAttReg = new Att_AttendanceRegister(_strConString);
        objPayEmpMonth = new Pay_Employee_Month(_strConString);
        objLeavededuction = new LeaveMaster_deduction(_strConString);
        objPartialReq = new Att_PartialLeave_Request(_strConString);
        objPayEmpAtt = new Pay_Employee_Attendance(_strConString);
        objShiftManagement = new Att_ShiftManagement(_strConString);
        objLeaveReq = new Att_Leave_Request(_strConString);
        cmn = new Common(_strConString);
        Obj_Notifiacation = new NotificationMaster(_strConString);
        ObjPenalty = new Pay_Employee_Penalty(_strConString);
        // Filter Criteria According to Location and Department
        objempparam = new EmployeeParameter(_strConString);
        objRoleData = new RoleDataPermission(_strConString);
        objLocDept = new Set_Location_Department(_strConString);
        ObjLocationMaster = new LocationMaster(_strConString);
        objRole = new RoleMaster(_strConString);
        Objda = new DataAccessClass(_strConString);
        objLogProcess = new LogProcess(_strConString);
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();

        if (!IsPostBack)
        {

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "73", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            // Modified By Nitin jain On 20/11/2014 Get Salary Increment Parameter For Fresher 
            Fr_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            // Experienced 
            Exp_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            //.................................................................................
            ddlMonth.SelectedValue = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month.ToString();
            txtYear.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
            FillddlDeaprtment();
            FillddlLocation();
            //FillGrid();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            EmpGroupSal_CheckedChanged(null, null);
            ddlLocation.Focus();
            if (!string.IsNullOrEmpty(Request.QueryString["Request_ID"]))
            {
                Show_Request_Record();
            }
        }

        AllPageCode();
        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        CalendarExtender1.Format = objSys.SetDateFormat();
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "4")
        {
            btnPayroll.Visible = false;
        }


    }

    public void Show_Request_Record()
    {
        string Emp_Code = Request.QueryString["Request_ID"].ToString();
        ddlField.SelectedValue = "Emp_Code";
        txtValue.Text = Emp_Code;
        btnarybind_Click1(null, null);
    }

    # region Filter Criteria According to Location and Department
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
            //Common Function add By Lokesh on 22-05-2015
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
        //dt = new DataView(dt, "Location_Id in(" + FLocIds.Substring(0, FLocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
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
        FillGrid();
    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        ImageButton1.Focus();
    }
    public void FillGrid()
    {
        string strsql = string.Empty;
        string strMonth = ddlMonth.SelectedValue;
        string strYear = txtYear.Text;
        string strCompanyId = "0";
        string strBrandId = "0";
        string strLocationId = "0";
        string strDepId = "0";

        strCompanyId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();

        if (ddlLocation.SelectedIndex == 0)
        {
            strLocationId = Session["LocId"].ToString();
        }
        else
        {
            strLocationId = ddlLocation.SelectedValue;
        }


        strDepId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0";
        }
        //if (Session["SessionDepId"] != null)
        //{
        //    strDepId = Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1);
        //}

        if (dpDepartment.SelectedIndex != 0)
        {
            //if (strDepId == "0")
            //{
            strDepId = dpDepartment.SelectedValue;
            //}
            //else
            //{
            //    strDepId = strDepId + "," + dpDepartment.SelectedValue;
            //}
        }


        strsql = "SELECt Emp_Id,MONTH(DOJ) AS JoiningMonth,YEAR(DOJ) AS JoiningYear, Emp_Code, Emp_Name,Emp_Name_L, CONVERT(date, DOJ) AS DOJ, Email_Id, Phone_No FROM Set_EmployeeMaster WHERE (Company_Id = " + strCompanyId + ") AND (IsActive = 'True') AND Field2 = 'False' AND Emp_Type = 'On Role' and Set_EmployeeMaster.Brand_Id=" + strBrandId + " and Set_EmployeeMaster.Location_Id=" + strLocationId + " and (case when '" + strDepId + "'='0' then '" + strDepId + "' else Set_EmployeeMaster.Department_Id end) in (SELECT CAST(Value AS int) FROM F_Split('" + strDepId + "', ',')) and ( (DATEPART(year,DOJ) < DATEPART(year, CAST(RTRIM(" + txtYear.Text + " * 10000 + " + ddlMonth.SelectedValue + " * 100 + 1) AS DATETIME))) or ((DATEPART(year,DOJ)=DATEPART(year, CAST(RTRIM(" + txtYear.Text + " * 10000 + " + ddlMonth.SelectedValue + " * 100 + 1) AS DATETIME))) and (DATEPART(MONTH,DOJ)<=DATEPART(MONTH, CAST(RTRIM(" + txtYear.Text + " * 10000 + " + ddlMonth.SelectedValue + " * 100 + 1) AS DATETIME))))) and Set_EmployeeMaster.Emp_Id not in (select Pay_Employee_Attendance.Emp_Id from Pay_Employee_Attendance where Pay_Employee_Attendance.MONTH=" + ddlMonth.SelectedValue + " and Pay_Employee_Attendance.Year=" + txtYear.Text + ")   order by cast(emp_code as int)";
        DataTable dtEmp = Objda.return_DataTable(strsql);
        Session["dtEmp"] = dtEmp;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        dtEmp.Dispose();


    }

    protected void gvEmployee_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmp"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtEmp"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee, dt, "", "");

    }

    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        ddlLocation.Focus();
        return;
    }
    #endregion
    public void AllPageCode()
    {
        //New Code 
        //DataTable DtApp_Id = ObjSysPeram.GetSysParameterByParamName("Application_Id");
        //string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
        //if (Application_Id != "1")
        //{
        //    btnPayroll.Visible = false;
        //}
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("73", (DataTable)Session["ModuleName"]);
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
        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "web")
            {
                btnLogPost.Visible = true;
            }
            btnLogProcess.Visible = true;
            btnReset.Visible = true;
        }
        else
        {

            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "73", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {


                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "web")
                            {
                                btnLogPost.Visible = true;
                            }
                            btnLogProcess.Visible = true;
                            btnReset.Visible = true;
                        }

                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "web")
                            {
                                //btnLogPost.Visible = true;
                            }
                            btnLogProcess.Visible = true;
                        }


                        if (DtRow["Op_Id"].ToString() == "3")
                        {
                            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "web")
                            {
                                //btnLogPost.Visible = true;
                            }
                            btnLogProcess.Visible = true;

                        }
                    }
                }
            }
        }
    }
    protected void imgValidEmployee_Click(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (txtYear.Text.Trim() == "" || txtYear.Text.Trim() == "0")
        {
            DisplayMessage("Enter Year");
            txtYear.Focus();
            return;
        }
        if (rbtnGroupSal.Checked)
        {
            lbxGroupSal_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }
    }
    protected void lbxGroupSal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMonth.Focus();
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        {
            if (lbxGroupSal.Items[i].Selected == true)
            {
                GroupIds += lbxGroupSal.Items[i].Value + ",";

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
                    //here we check that employee is valid or not for selected month and year'

                    if (ValidEmployee(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), Session["CompId"].ToString()) != "")
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
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
                Session["dtEmp4"] = dtEmp;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployeeSal, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp4"] = null;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployeeSal, dtEmp, "", "");
            }
        }
        else
        {
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
        }
        ddlMonth.Focus();
        return;
    }
    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroupSal.Checked)
        {
            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            Div_Employee.Visible = false;
            Div_Group.Visible = true;

            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id  in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            //--------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
            }

            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
            lbxGroupSal_SelectedIndexChanged(null, null);
            lbxGroupSal.Focus();
            return;
        }
        else if (rbtnEmpSal.Checked)
        {
            Div_Employee.Visible = true;
            Div_Group.Visible = false;
            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;

            lblEmp.Text = "";
            Session["CHECKED_ITEMS"] = null;
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            FillGrid();
        }
    }
    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployeeSal, (DataTable)Session["dtEmp4"], "", "");
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmployee.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmp"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, dt, "", "");
        PopulateCheckedValues();
        AllPageCode();
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                int index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
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
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
        Label LbCode = (Label)gvEmployee.Rows[index].FindControl("lblEmpCode");
        Label JoiningMonth = (Label)gvEmployee.Rows[index].FindControl("lblJoiningMonth");
        Label JoiningYear = (Label)gvEmployee.Rows[index].FindControl("lblJoiningYear");
        //DataTable DtProcessedEmp = objAttReg.GetAttendanceRegDataByMonth_Year_EmpId(lb.Text, ddlMonth.SelectedValue, txtYear.Text);
        //if (DtProcessedEmp.Rows.Count > 0)
        //{
        if (Convert.ToInt32(JoiningYear.Text) < Convert.ToInt32(txtYear.Text))
        {
            //--------------------------------------------------------
            string PostedEmpList = string.Empty;
            //for (int j = 0; j < empidlist.Split(',').Length - 1; j++)
            //{
            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text.Trim(), ddlMonth.SelectedValue, txtYear.Text);
            if (dtPostedList.Rows.Count > 0)
            {
                PostedEmpList = LbCode.Text;
                ((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked = false;
            }
            // }
            if (PostedEmpList != string.Empty)
            {
                DisplayMessage("Log Already Posted For Employee :- " + PostedEmpList + " ");
                return;
            }
            //---------------------------
            if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
            {
                empidlist += lb.Text.Trim().ToString() + ",";
                lblSelectRecord.Text += empidlist;

            }


            else
            {

                empidlist += lb.Text.ToString().Trim();
                lblSelectRecord.Text += empidlist;
                string[] split = lblSelectRecord.Text.Split(',');
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
                lblSelectRecord.Text = temp;
            }
        }
        else if (Convert.ToInt32(JoiningYear.Text) == Convert.ToInt32(txtYear.Text))
        {
            if (Convert.ToInt32(JoiningMonth.Text) <= Convert.ToInt32(ddlMonth.SelectedValue))
            {
                //--------------------------------------------------------
                string PostedEmpList = string.Empty;
                //for (int j = 0; j < empidlist.Split(',').Length - 1; j++)
                //{
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Text.Trim(), ddlMonth.SelectedValue, txtYear.Text);
                if (dtPostedList.Rows.Count > 0)
                {
                    PostedEmpList = LbCode.Text;
                    ((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked = false;
                }
                // }
                if (PostedEmpList != string.Empty)
                {
                    DisplayMessage("Log Already Posted For Employee :- " + PostedEmpList + " ");
                    return;
                }
                //---------------------------
                if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
                {
                    empidlist += lb.Text.Trim().ToString() + ",";
                    lblSelectRecord.Text += empidlist;

                }


                else
                {

                    empidlist += lb.Text.ToString().Trim();
                    lblSelectRecord.Text += empidlist;
                    string[] split = lblSelectRecord.Text.Split(',');
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
                    lblSelectRecord.Text = temp;
                }
            }
            else
            {
                ((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked = false;
                empidlist += lb.Text.ToString().Trim();
                lblSelectRecord.Text += empidlist;
                string[] split = lblSelectRecord.Text.Split(',');
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

                lblSelectRecord.Text = temp;
                DisplayMessage("You May Not Proceed Before Joining Date of Employee :- " + LbCode.Text + "");
                return;
            }
        }
        else
        {
            ((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked = false;
            empidlist += lb.Text.ToString().Trim();
            lblSelectRecord.Text += empidlist;
            string[] split = lblSelectRecord.Text.Split(',');
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

            lblSelectRecord.Text = temp;
            DisplayMessage("You May Not Proceed Before Joining Date of Employee :- " + LbCode.Text + "");
            return;
        }
        //else
        //{
        //((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked = false;
        //empidlist += lb.Text.ToString().Trim();
        //lblSelectRecord.Text += empidlist;
        //string[] split = lblSelectRecord.Text.Split(',');
        //foreach (string item in split)
        //{
        //    if (item != empidlist)
        //    {
        //        if (item != "")
        //        {
        //            if (item != "")
        //            {
        //                temp += item + ",";
        //            }
        //        }
        //    }
        //}
        //lblSelectRecord.Text = temp;
        //DisplayMessage("You Can Not Log Post before Log Process For Employee :- " + LbCode.Text + "");
        //return;
        //}
    }
    public string ValidEmployee(string StrAllEmpId, string strCompanyId)
    {

        DataTable dtPostedList = new DataTable();
        if (txtYear.Text.Trim() == "")
        {
            txtYear.Text = "0";
        }
        string strEmpId = string.Empty;


        foreach (string str in StrAllEmpId.Split(','))
        {
            DataTable dt = objEmp.GetEmployeeMasterById(strCompanyId, str);
            if (dt.Rows.Count > 0)
            {
                string empidlist = string.Empty;
                string temp = string.Empty;
                string lb = dt.Rows[0]["Emp_Id"].ToString();
                string LbCode = dt.Rows[0]["Emp_Code"].ToString();
                DateTime Joiningdate = Convert.ToDateTime(dt.Rows[0]["Doj"].ToString());
                string JoiningMonth = Joiningdate.Month.ToString();
                string JoiningYear = Joiningdate.Year.ToString();
                //DataTable DtProcessedEmp = objAttReg.GetAttendanceRegDataByMonth_Year_EmpId(lb.Text, ddlMonth.SelectedValue, txtYear.Text);
                //if (DtProcessedEmp.Rows.Count > 0)
                //{
                if (Convert.ToInt32(JoiningYear) < Convert.ToInt32(txtYear.Text))
                {
                    //--------------------------------------------------------
                    string PostedEmpList = string.Empty;
                    //for (int j = 0; j < empidlist.Split(',').Length - 1; j++)
                    //{
                    dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Trim(), ddlMonth.SelectedValue, txtYear.Text);
                    if (dtPostedList.Rows.Count == 0)
                    {
                        strEmpId += str + ",";
                    }
                }
                else if (Convert.ToInt32(JoiningYear) == Convert.ToInt32(txtYear.Text))
                {
                    if (Convert.ToInt32(JoiningMonth) <= Convert.ToInt32(ddlMonth.SelectedValue))
                    {
                        //--------------------------------------------------------
                        string PostedEmpList = string.Empty;
                        //for (int j = 0; j < empidlist.Split(',').Length - 1; j++)
                        //{
                        dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lb.Trim(), ddlMonth.SelectedValue, txtYear.Text);
                        if (dtPostedList.Rows.Count == 0)
                        {
                            strEmpId += str + ",";
                        }
                    }
                }
            }
        }

        dtPostedList.Dispose();
        return strEmpId;
    }
    protected void ImgbtnSelectAll_Clickary(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmp"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmp"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    private string GetTime24(string timepart)
    {
        string str = "00:00";
        DateTime date = Convert.ToDateTime(timepart);
        str = date.ToString("HH:mm");
        return str;
    }

    public string DateFormat(string Date)
    {
        string SystemDate = string.Empty;

        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
        }
        catch
        {
            SystemDate = Date;
        }

        return SystemDate;
    }


    protected void btnLogPost1_Click(object sender, EventArgs e)
    {
        string AlreadyPostedEmpList = string.Empty;
        string NonPostedEmpList = string.Empty;
        string strFinalPayEmp = string.Empty;
        string empidlist = string.Empty;
        string NonLogProcessedEmp = string.Empty;
        string GroupIds = string.Empty;

        if (rbtnEmpSal.Checked == true)
        {

            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {

                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                strFinalPayEmp += userdetails[i].ToString() + ",";
            }

            empidlist = strFinalPayEmp;
            if (empidlist == "")
            {
                DisplayMessage("Select Atleast One Employee");
                gvEmployee.Focus();
                return;
            }


        }
        else
        {
            string EmpIds = string.Empty;
            empidlist = "";
            lblSelectRecord.Text = "";

            for (int i = 0; i < lbxGroupSal.Items.Count; i++)
            {
                if (lbxGroupSal.Items[i].Selected)
                {
                    GroupIds += lbxGroupSal.Items[i].Value + ",";
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
                        if (ValidEmployee(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), Session["CompId"].ToString()) != "")
                        {
                            //EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), ddlMonth.SelectedValue, txtYear.Text);
                            DataTable DtFilterEmpList = new DataView(dtEmp, "Emp_Id='" + dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtPostedList.Rows.Count > 0)
                            {
                                AlreadyPostedEmpList += DtFilterEmpList.Rows[0]["Emp_Code"].ToString() + ",";
                            }
                            else
                            {
                                NonPostedEmpList += DtFilterEmpList.Rows[0]["Emp_Code"].ToString() + ",";
                                lblSelectRecord.Text += DtFilterEmpList.Rows[0]["Emp_Id"].ToString() + ",";
                            }
                        }

                    }

                }



                foreach (string str in lblSelectRecord.Text.Split(','))
                {
                    if (str != "")
                    {
                        empidlist += str + ",";

                    }
                }

                if (dtEmpInGroup.Rows.Count > 0)
                {
                    if (empidlist.ToString() == "")
                    {
                        DisplayMessage("Log Already Posted For Selected Group Employee");
                        gvEmployee.Focus();
                        return;
                    }
                    else
                    {
                    }
                }
                else
                {
                    DisplayMessage("Employees Are Not Exists In Selected Groups");
                    gvEmployee.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
                return;
            }

        }



        //all pending leave validation


        DataTable dtPendingLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "1");
        //
        if (dtPendingLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingLeave.Rows.Count; i++)
            {
                Ids = dtPendingLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }
            Ids = objAttendance.GetEmployeeCode(Ids);
            DisplayMessage("Please Approve Full Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log post");
            //DisplayMessage("Please Approve Leave for Employee List : '"+Ids+"' then do Log Process");
            return;
        }

        DataTable dtPendingHalfLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "2");
        //
        if (dtPendingHalfLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingHalfLeave.Rows.Count; i++)
            {
                Ids = dtPendingHalfLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }
            Ids = objAttendance.GetEmployeeCode(Ids);
            DisplayMessage("Please Approve Half Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log post");
            return;
        }

        DataTable dtPendingPLLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "3");
        //
        if (dtPendingPLLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingPLLeave.Rows.Count; i++)
            {
                Ids = dtPendingPLLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }
            Ids = objAttendance.GetEmployeeCode(Ids);
            DisplayMessage("Please Approve Partial Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log post");
            return;
        }



        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        string[] strresult = new string[2];

        try
        {

            strresult = objLogProcess.autoLogPost(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), empidlist, ddlMonth.SelectedValue, txtYear.Text, Session["UserId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            NonLogProcessedEmp = strresult[1].ToString().Trim();

            if (GroupIds != string.Empty)
            {
                DisplayMessage("Log Posted Successfully For Employees:- " + NonPostedEmpList.ToString() + " Log Already Posted For Employees:- " + AlreadyPostedEmpList.ToString() + " ");
                ddlField.Focus();
            }
            if (NonLogProcessedEmp != string.Empty)
            {
                DisplayMessage("Log Has Not Processed So You Can Not Log Post For Employee :- " + NonLogProcessedEmp.TrimEnd() + "");
                ddlField.Focus();
                return;
            }
            else
            {
                Session["CHECKED_ITEMS"] = null;
                DataTable dtProduct1 = (DataTable)Session["dtEmp"];
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployee, dtProduct1, "", "");

                SystemLog.SaveSystemLog("Log Post", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Log Post done", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["DBConnection"].ToString());
                DisplayMessage("Log Posted Successfully");
                ddlField.Focus();
                btnaryRefresh_Click1(null, null);
            }


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
    }


    protected void btnPayroll_Click(object sender, EventArgs e)
    {
        string url = "../HR/GenerateEmpPayroll.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }


    public string GetEmployeeCode(string EmpIds)
    {
        string EmpCodeList = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = Objda.return_DataTable("select distinct Emp_Code from set_EmployeeMaster where (Emp_Id  IN (SELECT CAST(Value AS INT) FROM F_Split('" + EmpIds + "', ',')))");
        if (dtInfo.Rows.Count > 0)
        {
            for (int i = 0; i < dtInfo.Rows.Count; i++)
            {
                EmpCodeList += dtInfo.Rows[i]["Emp_Code"].ToString() + ",";
            }
        }
        return EmpCodeList;
    }

    public DataTable GetEmployeeinfo(string EmpIds)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = Objda.return_DataTable("select distinct Emp_Code,Emp_Name from set_EmployeeMaster where (Emp_Id  IN (SELECT CAST(Value AS INT) FROM F_Split('" + EmpIds + "', ',')))");
        return dtInfo;
    }


    protected void btnvalidationifo_Click(object sender, EventArgs e)
    {
        DataTable dtValidation = new DataTable();
        dtValidation.Columns.Add("EmployeeCode");
        dtValidation.Columns.Add("EmployeeName");
        dtValidation.Columns.Add("Type");



        string strCompanyId = Session["CompId"].ToString();
        string strBrandId = Session["BrandId"].ToString();
        string strLocationId = Session["LocId"].ToString();

        string strDepId = string.Empty;
        string empidlist = string.Empty;
        if (rbtnEmpSal.Checked == true)
        {
            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }

            for (int i = 0; i < userdetails.Count; i++)
            {
                empidlist += userdetails[i].ToString() + ",";
            }
        }
        else
        {
            string GroupIds = string.Empty;


            for (int i = 0; i < lbxGroupSal.Items.Count; i++)
            {
                if (lbxGroupSal.Items[i].Selected)
                {
                    GroupIds += lbxGroupSal.Items[i].Value + ",";
                }
            }


            if (GroupIds != "")
            {

                empidlist = "";
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(strCompanyId);

                dtEmp = new DataView(dtEmp, "Brand_Id='" + strBrandId + "'  and Location_Id='" + strLocationId + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(strCompanyId);

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!empidlist.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {

                        if (ValidEmployee(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), strCompanyId) != "")
                        {
                            empidlist += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select group first");
                return;
            }


        }

        if (Session["SessionDepId"] != null)
        {
            strDepId = Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1);
        }



        DataTable dttemp = new DataTable();
        int LogFlag = 0;
        string LogBeforeJoin = string.Empty;

        DateTime OutTimeE = new DateTime(1990, 1, 1);
        int b = 0;
        // Selected Emp Id 

        //

        // Modified By Nitin Jain On 06/11/2014
        DataTable dtPendingLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "1");
        //
        if (dtPendingLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingLeave.Rows.Count; i++)
            {
                Ids += dtPendingLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }


            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Full Day");
            }


            Ids = GetEmployeeCode(Ids);



            //DisplayMessage("Please Approve Full Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            ////DisplayMessage("Please Approve Leave for Employee List : '"+Ids+"' then do Log Process");
            //return;
        }

        DataTable dtPendingHalfLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "2");
        //
        if (dtPendingHalfLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingHalfLeave.Rows.Count; i++)
            {
                Ids += dtPendingHalfLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }


            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Half Day");
            }



            Ids = GetEmployeeCode(Ids);

            //DisplayMessage("Please Approve Half Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            //return;
        }

        DataTable dtPendingPLLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "3");
        //
        if (dtPendingPLLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingPLLeave.Rows.Count; i++)
            {
                Ids += dtPendingPLLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }

            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Partial Leave");
            }

            Ids = GetEmployeeCode(Ids);
            //DisplayMessage("Please Approve Partial Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            //return;
        }

        // Make Date From to To
        txtFromDate.Text = new DateTime(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue), 1).ToString("dd-MMM-yyyy");
        txtToDate.Text = new DateTime(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd-MMM-yyyy");
        bool SalIncrementEnable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Salary_Increment_Enable", strCompanyId, strBrandId, strLocationId));


        if (SalIncrementEnable == true)
        {
            // Salary Increment Employee List Here 
            double Month = Convert.ToDouble(ddlMonth.SelectedValue);
            double Year = Convert.ToDouble(txtYear.Text);
            if (Month != 0)
            {
                if (Month != 1)
                {
                    Month = Month - 1;
                }
                else
                {
                    Month = 12;
                    Year = Year - 1;
                }
            }

            DataTable dtSalaryInc = objEmpSalInc.GetEmpSalaryIncrementByMonthYear(strCompanyId, Month.ToString(), Year.ToString());
            dtSalaryInc = new DataView(dtSalaryInc, "Brand_Id='" + strBrandId + "'  and Location_Id='" + strLocationId + "' and Emp_Id in (" + empidlist.ToString().Substring(0, empidlist.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                if (strDepId != "")
                {
                    dtSalaryInc = new DataView(dtSalaryInc, "Department_Id in(" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch (Exception Ex)
            {

            }
            if (dtSalaryInc.Rows.Count > 0)
            {
                string IncEmp = string.Empty;
                for (int inc = 0; inc < dtSalaryInc.Rows.Count; inc++)
                {
                    dtValidation.Rows.Add(dtSalaryInc.Rows[inc]["Emp_Code"].ToString(), dtSalaryInc.Rows[inc]["Emp_Name"].ToString(), "Salary Increment");

                }
                //DisplayMessage("Salary Increment is Remaning For Employee List : " + IncEmp.ToString().TrimEnd() + " then do Log Process");
                //return;
            }
            //----------------------------------------------------------------------------------------------------------
        }
        // Response.Write("<script>alert('Hello');</script>");

        // dtValidation.Columns.Add("EmployeeCode");
        // dtValidation.Columns.Add("EmployeeName");
        /// dtValidation.Columns.Add("Type");

        if (dtValidation.Rows.Count == 0)
        {
            DisplayMessage("Validation not found");
        }
        else
        {
            dtValidation = dtValidation.DefaultView.ToTable(true, "EmployeeCode", "EmployeeName", "Type");
            ExportTableData(dtValidation);
        }
    }

    protected void btnLogProcess_Click(object sender, EventArgs e)
    {
        DataTable dtValidation = new DataTable();
        dtValidation.Columns.Add("EmployeeCode");
        dtValidation.Columns.Add("EmployeeName");
        dtValidation.Columns.Add("Type");

        string strEmpValidation = string.Empty;

        string strCompanyId = Session["CompId"].ToString();
        string strBrandId = Session["BrandId"].ToString();
        string strLocationId = Session["LocId"].ToString();

        string strDepId = string.Empty;
        string empidlist = string.Empty;
        if (rbtnEmpSal.Checked == true)
        {
            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }

            for (int i = 0; i < userdetails.Count; i++)
            {
                empidlist += userdetails[i].ToString() + ",";
            }
        }
        else
        {
            string GroupIds = string.Empty;


            for (int i = 0; i < lbxGroupSal.Items.Count; i++)
            {
                if (lbxGroupSal.Items[i].Selected)
                {
                    GroupIds += lbxGroupSal.Items[i].Value + ",";
                }
            }


            if (GroupIds != "")
            {

                empidlist = "";
                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(strCompanyId);

                dtEmp = new DataView(dtEmp, "Brand_Id='" + strBrandId + "'  and Location_Id='" + strLocationId + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(strCompanyId);

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!empidlist.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {

                        if (ValidEmployee(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), strCompanyId) != "")
                        {
                            empidlist += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select group first");
                return;
            }


        }

        if (Session["SessionDepId"] != null)
        {
            strDepId = Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1);
        }



        DataTable dttemp = new DataTable();
        int LogFlag = 0;
        string LogBeforeJoin = string.Empty;

        DateTime OutTimeE = new DateTime(1990, 1, 1);
        int b = 0;
        // Selected Emp Id 

        //

        // Modified By Nitin Jain On 06/11/2014
        DataTable dtPendingLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "1");
        //
        if (dtPendingLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingLeave.Rows.Count; i++)
            {
                Ids += dtPendingLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }

            strEmpValidation += Ids;
            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Full Day");
            }


            Ids = GetEmployeeCode(Ids);



            //DisplayMessage("Please Approve Full Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            ////DisplayMessage("Please Approve Leave for Employee List : '"+Ids+"' then do Log Process");
            //return;
        }

        DataTable dtPendingHalfLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "2");
        //
        if (dtPendingHalfLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingHalfLeave.Rows.Count; i++)
            {
                Ids += dtPendingHalfLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }

            strEmpValidation += Ids;
            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Half Day");
            }



            Ids = GetEmployeeCode(Ids);

            //DisplayMessage("Please Approve Half Day Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            //return;
        }

        DataTable dtPendingPLLeave = objAttLog.Get_EmployeePendingLeaveByEmpId(empidlist, ddlMonth.SelectedValue, txtYear.Text, "3");
        //
        if (dtPendingPLLeave.Rows.Count > 0)
        {
            string Ids = string.Empty;
            for (int i = 0; i < dtPendingPLLeave.Rows.Count; i++)
            {
                Ids += dtPendingPLLeave.Rows[i]["Emp_Id"].ToString() + ",";
            }

            strEmpValidation += Ids;

            dttemp = GetEmployeeinfo(Ids);

            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dtValidation.Rows.Add(dttemp.Rows[j][0].ToString(), dttemp.Rows[j][1].ToString(), "Partial Leave");
            }

            Ids = GetEmployeeCode(Ids);
            //DisplayMessage("Please Approve Partial Leave for Employee List :- " + Ids.ToString().TrimEnd() + "then do Log Process");
            //return;
        }

        // Make Date From to To
        txtFromDate.Text = new DateTime(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue), 1).ToString("dd-MMM-yyyy");
        txtToDate.Text = new DateTime(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(txtYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd-MMM-yyyy");
        bool SalIncrementEnable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Salary_Increment_Enable", strCompanyId, strBrandId, strLocationId));


        if (SalIncrementEnable == true)
        {
            // Salary Increment Employee List Here 
            double Month = Convert.ToDouble(ddlMonth.SelectedValue);
            double Year = Convert.ToDouble(txtYear.Text);
            if (Month != 0)
            {
                if (Month != 1)
                {
                    Month = Month - 1;
                }
                else
                {
                    Month = 12;
                    Year = Year - 1;
                }
            }

            DataTable dtSalaryInc = objEmpSalInc.GetEmpSalaryIncrementByMonthYear(strCompanyId, Month.ToString(), Year.ToString());
            dtSalaryInc = new DataView(dtSalaryInc, "Brand_Id='" + strBrandId + "'  and Location_Id='" + strLocationId + "' and Emp_Id in (" + empidlist.ToString().Substring(0, empidlist.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                if (strDepId != "")
                {
                    dtSalaryInc = new DataView(dtSalaryInc, "Department_Id in(" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch (Exception Ex)
            {

            }
            if (dtSalaryInc.Rows.Count > 0)
            {
                string IncEmp = string.Empty;
                for (int inc = 0; inc < dtSalaryInc.Rows.Count; inc++)
                {
                    dtValidation.Rows.Add(dtSalaryInc.Rows[inc]["Emp_Code"].ToString(), dtSalaryInc.Rows[inc]["Emp_Name"].ToString(), "Salary Increment");
                    IncEmp += dtSalaryInc.Rows[inc]["Emp_Code"].ToString() + ",";
                    strEmpValidation += dtSalaryInc.Rows[inc]["Emp_Code"].ToString() + ",";
                }
                //DisplayMessage("Salary Increment is Remaning For Employee List : " + IncEmp.ToString().TrimEnd() + " then do Log Process");
                //return;
            }
            //----------------------------------------------------------------------------------------------------------
        }


        string strFinalEmpList = string.Empty;

        foreach (string str in empidlist.Split(','))
        {
            if (str == "")
            {
                continue;
            }

            if (!strEmpValidation.Split(',').Contains(str))
            {
                strFinalEmpList += str + ",";
            }
        }

        // Response.Write("<script>alert('Hello');</script>");


        if (strFinalEmpList.Trim() == "")
        {
            DisplayMessage("Approval is pending, To get details click on validation info button");
            return;
        }
        else
        {

            if (dtValidation.Rows.Count == 0)
            {
                //DisplayMessage("Log Process started, you will get notification on completion");
                DisplayMessage("Log Process Completed");
            }
            else
            {
                //DisplayMessage("Log Process started, except some employees due to pending approval, To get details click on validation info button");
                DisplayMessage("Log Process completed, except some employees due to pending approval, To get details click on validation info button");
            }
        }


        //HttpContext ctx = HttpContext.Current;
        //Thread t = new Thread(new ThreadStart(() =>
        //{
        //    HttpContext.Current = ctx;
        //    objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strFinalEmpList, Session["UserId"].ToString(), strDepId, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Session["EmpId"].ToString());
        //}));
        //t.Start();

        //ThreadStart ts = delegate ()
        //{
        string strlogprocessLocationName = string.Empty;
        string strlogprocessLocationId = string.Empty;

        if (ddlLocation.SelectedIndex > 0)
        {
            strlogprocessLocationName = ddlLocation.SelectedItem.Text.Trim();
            strlogprocessLocationId = ddlLocation.SelectedValue.Trim();
        }
        else
        {
            strlogprocessLocationName = ObjLocationMaster.GetLocationMasterById(Session["Compid"].ToString(), Session["LocId"].ToString()).Rows[0]["LOcation_Name"].ToString();
            strlogprocessLocationId = Session["LocId"].ToString();
        }



        Session["LeaveIntegration"] = new DataTable();
        if (ConfigurationManager.AppSettings["LeaveIntegration"].ToString().Trim() == "1")
        {


            try
            {
                DataTable dtLeaveIntegration = Objda.GeTOracleRecord("SELECT employeecode,shiftnameleavetype,fromdate,todate FROM APPS.XXTSC_TMS_LEAVE_MF WHERE 1 = 1 and location='" + strlogprocessLocationName + "'", ReadHRMSConStringFromFile());
                if (dtLeaveIntegration.Rows.Count > 0)
                {
                    Session["LeaveIntegration"] = dtLeaveIntegration;
                }
            }
            catch
            {

            }
        }

        //objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(),strlogprocessLocationId, strFinalEmpList, Session["UserId"].ToString(), strDepId, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Session["EmpId"].ToString());
        objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), strlogprocessLocationId, strFinalEmpList, Session["UserId"].ToString(), strDepId, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Session["EmpId"].ToString(), (DataTable)Session["LeaveIntegration"], HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(), ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString());

        //};

        ////ThreadStart ts = delegate () { objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strFinalEmpList, Session["UserId"].ToString(), strDepId, Convert.ToDateTime("2018-08-09"), Convert.ToDateTime("2018-08-09"), Session["EmpId"].ToString()); };

        //// The thread.
        //Thread t = new Thread(ts);

        //// Run the thread.
        //t.Start();


        Session["TerminationDate"] = null;
        Session["TerminateEmpId"] = null;

        if (dtValidation.Rows.Count == 0)
        {
            btnaryRefresh_Click1(null, null);
        }

    }

    public string ReadHRMSConStringFromFile()
    {

        try
        {
            FileStream fs = new FileStream("C:\\PegasusSQL\\HRMSCONNECTIONSTRING.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string sqlStrSetting = sw.ReadLine();
            fs.Close();
            sw.Close();
            return sqlStrSetting;
        }
        catch
        {
            return "";
        }
    }


    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "ValidationInfo";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }


    public string GetApplicationParameterValueByParamName(string strParamName, DataTable dtCompanyParameter)
    {
        return new DataView(dtCompanyParameter, "Param_Name='" + strParamName + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
    }


    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["TerminationDate"] = null;
        Session["TerminateEmpId"] = null;
        btnaryRefresh_Click1(null, null);
        lblLocation.Visible = true;
        ddlLocation.Visible = true;
        lblGroupByDept.Visible = true;
        dpDepartment.Visible = true;

        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlMonth.SelectedValue = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month.ToString();
        txtYear.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
        Session["CHECKED_ITEMS"] = null;
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }

        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        btnaryRefresh_Click1(null, null);
    }

    protected void btnarybind_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (txtValue.Text.Trim() == "")
        {

            DisplayMessage("Please Fill Value");
            txtValue.Focus();
            return;
        }
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmp"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, view.ToTable(), "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        txtValue.Focus();
    }
    protected void btnaryRefresh_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        FillGrid();
        dpDepartment.SelectedIndex = 0;
        ddlField.Focus();
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {

        CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }


    public void updateEmployeeaLeavebalance(string strEmpId, string Date, DataTable dtLogDetail)
    {
        // thid function created fot add or deduct log process according employee log on leave date 

        string strleaveTypeId = string.Empty;
        string strTransId = string.Empty;
        DataTable dt = ObjLeaveReq.GetLeaveRequestChildData_By_Employeeidanddate(Date.ToString(), strEmpId);


        dtLogDetail = new DataView(dtLogDetail, "Event_Date='" + Date.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        // here we are checking that leave  balance added or not during last log process if added then 


        //code start

        if (dt.Rows.Count > 0)
        {
            strleaveTypeId = dt.Rows[0]["LeaveType_Id"].ToString();
            strTransId = dt.Rows[0]["Trans_Id"].ToString();

            if (new DataView(dt, "Field1<>' '", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                Objda.execute_Command("update Att_Leave_Request_Child set Field1=' ' where Trans_Id=" + strTransId + "");

                if (dt.Rows[0]["Is_Paid"].ToString() == "True")
                {
                    Objda.execute_Command("update Att_Employee_Leave_Trans set Used_Days=(Used_Days+1) ,Remaining_Days=(cast( Remaining_Days as decimal(18,6))-1),Field2=(cast( Field2 as decimal(18,6))-1) where emp_id=" + strEmpId + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year + "' and Leave_Type_Id=" + strleaveTypeId + "");
                }
                else
                {
                    Objda.execute_Command("update Att_Employee_Leave_Trans set Used_Days=(Used_Days+1) ,Remaining_Days=(cast( Remaining_Days as decimal(18,6))-1) where emp_id=" + strEmpId + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year + "' and Leave_Type_Id=" + strleaveTypeId + "");
                }
            }

            if (dtLogDetail.Rows.Count > 0)
            {

                string strLeaveRemarks = "Log Found on leave date " + Convert.ToDateTime(Date.ToString()).ToString(ObjSysPeram.SetDateFormat());

                Objda.execute_Command("update Att_Leave_Request_Child set Field1='" + strLeaveRemarks + "' where Trans_Id=" + strTransId + "");

                if (dt.Rows[0]["Is_Paid"].ToString() == "True")
                {
                    Objda.execute_Command("update Att_Employee_Leave_Trans set Used_Days=(Used_Days-1) ,Remaining_Days=(cast( Remaining_Days as decimal(18,6))+1),Field2=(cast( Field2 as decimal(18,6))+1) where emp_id=" + strEmpId + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year + "' and Leave_Type_Id=" + strleaveTypeId + "");
                }
                else
                {
                    Objda.execute_Command("update Att_Employee_Leave_Trans set Used_Days=(Used_Days-1) ,Remaining_Days=(cast( Remaining_Days as decimal(18,6))+1) where emp_id=" + strEmpId + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year + "' and Leave_Type_Id=" + strleaveTypeId + "");
                }
            }
        }

        dt.Dispose();
        dtLogDetail.Dispose();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (txtYear.Text.Trim() == "" || txtYear.Text.Trim() == "0")
        {
            DisplayMessage("Enter Year");
            txtYear.Focus();
            return;
        }
        if (rbtnGroupSal.Checked)
        {
            lbxGroupSal_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }
    }


}
