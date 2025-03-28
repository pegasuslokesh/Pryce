using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PegasusDataAccess;

public partial class Attendance_Report_AttendanceReport : BasePage
{
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    // Filter Criteria According to Location and Department
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    Common ObjComman = null;
    UserMaster objUser = null;
    DepartmentMaster objDep = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;
    //------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "105";
        Session["HeaderText"] = "Attendance Reports";

        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        // Filter Criteria According to Location and Department
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {

            DateTime now = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            txtFromDate.Text = new DateTime(now.Year, now.Month, 1).ToString("dd-MMM-yyyy");
            txtToDate.Text = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");

            ddlLocation.Enabled = true;
            dpDepartment.Enabled = true;
            // Session["EmpList"] = null;
            Div_Main.Visible = true;
            FillddlLocation();
            FillGrid("");
            FillDepartment();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            EmpGroupSal_CheckedChanged(null, null);
            pnlReport.Visible = false;
            Session["CHECKED_ITEMS"] = null;
            if (Session["EmpList"] != null)
            {
                Div_Main.Visible = false;
                pnlReport.Visible = true;
            }

            try
            {
                Div_Terminated_Report.Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission(Common.GetObjectIdbyPageURL("../attendance_report/attendancereport.aspx", Session["DBConnection"].ToString()), "14", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {

            }

            GetReportPermission();
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            BindTreeView();

        }
        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        CalendarExtender1.Format = objSys.SetDateFormat();



    }


    protected void lnkWorkHourReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/WorkhourReport.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }

    public void GetReportPermission()
    {
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
        DataTable dtAllAttChild = new DataView(ObjComman.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id), "Module_Id=105", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
        for (int i = 0; i < dtAllAttChild.Rows.Count; i++)
        {
            if (dtAllAttChild.Rows[i]["Object_Id"].ToString().Trim() == "73")
            {
                //btnLogProc.Visible = true;
            }
        }

        DataTable dtAllReportChild = new DataView(ObjComman.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id, "True"), "Module_Id=105", "Order_Appear", DataViewRowState.CurrentRows).ToTable();

        for (int i = 0; i < dtAllReportChild.Rows.Count; i++)
        {
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "96")
            {
                linkInOut.Visible = true;
                linkInOutseperate.Visible = true;
                linkInOutDriect.Visible = true;
            }
           
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "97")
            {
                linkLatein.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "98")
            {
                linkEarlyOut.Visible = true;


            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "99")
            {
                linkAbsentReport.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "428")
            {
                linkTimeCardReport.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "429")
            {
                lnkAbsentDetail.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "430")
            {
                lnlClocl10.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "100")
            {
                linkOverTime.Visible = true;


            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "101")
            {
                linkWeekOffReport.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "102")
            {
                linkHolidayReport.Visible = true;


            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "105")
            {
                linkLeaveReport.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "106")
            {
                linkLeaveStatus.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "107")
            {
                lnkShiftReport.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "109")
            {
                linkLeaveRemaning.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "110")
            {
                linkPartialLeave.Visible = true;

            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "111")
            {
                linkPartialViolation.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "112")
            {
                linkDailySalary.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "113")
            {
                Lnk_Attendance_Register.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "114")
            {
                linkLogProcess.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "266")
            {
                lnkLogReport.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "115")
            {
                LinkButton12.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "116")
            {
                linkEmpInfo.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "117")
            {
                Lnl_Attendance_Salary_Report.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "126")
            {
                linkMailTransaction.Visible = true;
                LinkButton1.Visible = true;
            }
          

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "127")
            {
                linkSmsTransaction.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "129")
            {
                Lnk_Attendance_Summary_Report.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "130")
            {
                linkUserTransfer.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "134")
            {
                lnkshiftscheduleReport.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "153")
            {
                linkTourAttendance.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "154")
            {
                lnkLateIn.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "206")
            {
                linkBreakInOut.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "216")
            {
                linkHalfDay.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "230")
            {
                Lnl_Attendance_TimeSheet_Report.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == "231")
            {
                Lnl_Attendance_Salary_Summary_Report.Visible = true;

            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/AccessGroupSummary.aspx", Session["DBConnection"].ToString()))
            {
                LNK_Access_Group_Summary.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/ExceptionCountReport.aspx", Session["DBConnection"].ToString()))
            {
                lnkExceptionCount.Visible = true;
            }
            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../attendance_report/EmployeeTracking.aspx", Session["DBConnection"].ToString()))
            {
                lnkEmployeeTracking.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/EmployeeHolidayReport.aspx", Session["DBConnection"].ToString()))
            {
                linkEmployeeHolidayReport.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/WithoutShiftReport.aspx", Session["DBConnection"].ToString()))
            {
                lnkWithoutShiftReport.Visible = true;
            }


            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/AccessGroupSummaryDetail.aspx", Session["DBConnection"].ToString()))
            {
                LNK_Access_Group_Summary_Detail.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/WorkHourReport.aspx", Session["DBConnection"].ToString()))
            {
                lnkWorkHourReport.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/PendingHolidayReport.aspx", Session["DBConnection"].ToString()))
            {
                linkPendingHolidayReport.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/TimesDeductionReport.aspx", Session["DBConnection"].ToString()))
            {
                linkTimeDeductionReport.Visible = true;
            }

            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/PendingOffReport.aspx", Session["DBConnection"].ToString()))
            {
                linkPendingoffReport.Visible = true;
            }


            if (dtAllReportChild.Rows[i]["Object_Id"].ToString().Trim() == Common.GetObjectIdbyPageURL("../Attendance_Report/TSC_Overtimereport.aspx", Session["DBConnection"].ToString()))
            {
                linkTSCOvertimeReport.Visible = true;
            }

        }

    }

    protected void btnfilterdepartment_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        //string url = "../Attendance/LogProcess.aspx";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }
    #region Filter Criteria According to Location and Department

    private void FillddlLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

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
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ListItem li = new ListItem("All", "0");
            ddlLocation.Items.Insert(0, li);

            ddlLocation.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }


    protected void btnsave_OnClick(object sender, EventArgs e)
    {
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


        if (Session["deptFilter"] == null)
        {
            FillGrid(strDeptId);
        }
        else
        {
            FillGrid(Session["deptFilter"].ToString());
        }


        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
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


    #region BindDepartmentTreeview


    public DataTable GetUserDepartment()
    {
        dpDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();
        string strDeptvalue = string.Empty;
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            for (int j = 1; j < ddlLocation.Items.Count; j++)
            {
                strDeptvalue = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.Items[j].Value, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDeptvalue != "")
                {
                    strDepId += strDeptvalue;
                }
            }
        }


        dtDepartment = objDep.GetDepartmentMaster();



        if (strDepId == "")
        {
            strDepId = "0,";
        }

        //if (Session["EmpId"].ToString() != "0")
        //{
        dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId.Substring(0, strDepId.Length - 1) + ")", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id", "Parent_Id");
        //}



        return dtDepartment;
    }

    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {

        TreeViewDepartment.Nodes.Clear();

        DataTable dt = GetUserDepartment();

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


    #endregion


    public void FillDepartment()
    {
        dpDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();
        string strDeptvalue = string.Empty;
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            for (int j = 1; j < ddlLocation.Items.Count; j++)
            {
                strDeptvalue = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.Items[j].Value, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDeptvalue != "")
                {
                    strDepId += strDeptvalue;
                }
            }
        }


        dtDepartment = objDep.GetDepartmentMaster();



        if (strDepId == "")
        {
            strDepId = "0,";
        }

        //if (Session["EmpId"].ToString() != "0")
        //{
        dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id");
        //}

        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }


    public DataTable GetEmployeeFilteredRecord(DropDownList ddlLocationFilter, DropDownList ddlDepartmentFilter)
    {
        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
        if (ddlLocationFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + ddlLocationFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

            strLocationId = ddlLocationFilter.SelectedValue;
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            strLocationId = Session["LocId"].ToString();
        }

        if (Session["EmpId"].ToString() != "0")
        {

            strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (strDepId == "")
            {
                strDepId = "0";
            }

            dtEmp = new DataView(dtEmp, "Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlDepartmentFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + ddlDepartmentFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dtEmp;
    }


    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //btnfilterdepartment.Visible = ddlLocation.SelectedIndex <= 0 ? false : true;
        FillGrid("");
        FillDepartment();
        BindTreeView();
    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid("");
    }
    public void FillGrid(string strDeptId)
    {
        string strEmpId = string.Empty;
        string strLocationDept = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }

            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
        }
        else
        {


            //strLocId = Session["LocId"].ToString();

            //strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId);

            //if (strLocationDept == "")
            //{
            //    strLocationDept = "0,";
            //}

            // strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";

            foreach (ListItem li in ddlLocation.Items)
            {
                if (li.Value.Trim() == "0")
                {
                    continue;
                }

                strLocId = li.Value;

                strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

                if (strLocationDept == "")
                {
                    strLocationDept = "0,";
                }

                strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";

            }
        }

        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


        if (chkterminatedemployee.Checked)
        {
            dtEmp = objEmp.GetEmployeeTrueAllData(Session["CompId"].ToString());
            //dtEmp = objEmp.GetEmployeeMasterOnRole_withTerminated(Session["CompId"].ToString());
        }

        // Nitin Jasin , Get According to UserId to Get Records for Single User 
        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }



        if (IsSingleUser == false)
        {

            if (Session["EmpId"].ToString() != "0")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                if (ddlLocation.SelectedIndex > 0)
                {
                    dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            if (strDeptId != "" && strDeptId != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strDeptId.Substring(0, strDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }

            Session["dtEmp"] = dtEmp;
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            Session["CHECKED_ITEMS"] = null;

        }
        else
        {
            dt = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            Session["dtEmp"] = dt;
            gvEmployee.DataSource = dt;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            Session["CHECKED_ITEMS"] = null;
            ddlLocation.Enabled = false;
            dpDepartment.Enabled = false;
        }
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";

        DataTable dtEmp = ObjDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id =" + strLocationId + " and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");

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

    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid("");


    }
    #endregion
    protected void lbxGroupSal_SelectedIndexChanged(object sender, EventArgs e)
    {
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


            if (chkterminatedemployee.Checked)
            {
                dtEmp = objEmp.GetEmployeeMasterOnRole_withTerminated(Session["CompId"].ToString());
            }

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
                Session["dtEmp4"] = dtEmp;
                gvEmployeeSal.DataSource = dtEmp;
                gvEmployeeSal.DataBind();

            }
            else
            {
                Session["dtEmp4"] = null;
                gvEmployeeSal.DataSource = dtEmp;
                gvEmployeeSal.DataBind();
            }
        }
        else
        {
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();

        }
    }
    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {

        if (rbtnGroupSal.Checked)
        {
            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            pnlSearchdpl.Visible = false;

            Div_Employee.Visible = false;
            Div_Group.Visible = true;

            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                lbxGroupSal.DataSource = dtGroup;
                lbxGroupSal.DataTextField = "Group_Name";
                lbxGroupSal.DataValueField = "Group_Id";

                lbxGroupSal.DataBind();

            }
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
            lbxGroupSal_SelectedIndexChanged(null, null);
            lbxGroupSal.Focus();
        }
        else if (rbtnEmpSal.Checked)
        {
            Div_Employee.Visible = true;
            Div_Group.Visible = false;

            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            //lblGroupByDept.Visible = true;
            //dpDepartment.Visible = true;
            pnlSearchdpl.Visible = true;

            lblEmp.Text = "";
            lblSelectRecord.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            FillGrid("0");
            //DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //// Nitin Jasin , Get According to UserId to Get Records for Single User 
            //DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), "");
            //bool IsSingleUser = false;
            //try
            //{
            //    IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
            //}
            //catch
            //{
            //    IsSingleUser = false;
            //}
            //if (IsSingleUser == false)
            //{
            //    if (Session["SessionDepId"] != null)
            //    {

            //        dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            //    }

            //    if (dtEmp.Rows.Count > 0)
            //    {
            //        Session["dtEmp4"] = dtEmp;
            //        gvEmployee.DataSource = dtEmp;
            //        gvEmployee.DataBind();

            //    }
            //    else
            //    {
            //        gvEmployee.DataSource = null;
            //        gvEmployee.DataBind();

            //    }
            //    ddlLocation.Focus();
            //}
            //else
            //{
            //    dt = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    Session["dtEmp4"] = dt;
            //    gvEmployee.DataSource = dt;
            //    gvEmployee.DataBind();
            //}

        }
    }

    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        gvEmployeeSal.DataSource = (DataTable)Session["dtEmp4"];
        gvEmployeeSal.DataBind();
    }
    protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBxSelect = (CheckBox)e.Row.Cells[0].FindControl("chkBxSelect");
            CheckBox chkBxHeader = (CheckBox)this.gvEmployee.HeaderRow.FindControl("chkBxHeader");
            HiddenField hdnFldId = (HiddenField)e.Row.Cells[0].FindControl("hdnFldId");

            chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,document.getElementById('{0}'),'{1}');", chkBxHeader.ClientID, hdnFldId.Value.Trim());
        }
    }
    private void SaveCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
        {
            index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
            {
                int index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmployee);
        gvEmployee.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        PopulateCheckedValues(gvEmployee);
    }
    protected void ImgbtnSelectAll_Clickary(object sender, ImageClickEventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
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
            gvEmployee.DataSource = dtAddressCategory1;
            gvEmployee.DataBind();
            ViewState["Select"] = null;
        }
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

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string empidlist = string.Empty;

        if (rbtnEmpSal.Checked)
        {

            ArrayList userdetails = new ArrayList();

            SaveCheckedValues(gvEmployee);

            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select Atleast One Employee");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Atleast One Employee");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        empidlist = empidlist + userdetails[i].ToString() + ",";
                    }
                }

            }
        }
        if (rbtnGroupSal.Checked)
        {


            string GroupIds = string.Empty;
            string EmpIds = string.Empty;


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



                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        empidlist += str + ",";

                    }
                }

                if (empidlist == "")
                {
                    DisplayMessage("Employees are not exists in selected groups");
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }

        }







        int b = 0;
        // Selected Emp Id 
        //string empidlist = hdnFldSelectedValues.Value.Trim();


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

            if (objSys.getDateForInput(txtFromDate.Text.ToString()) > objSys.getDateForInput(txtToDate.Text.ToString()))
            {
                DisplayMessage("To Date should be Greater");
                txtToDate.Focus();
                return;
            }
        }
        catch (Exception)
        {
            DisplayMessage("Date Not In Proper Format");
            txtFromDate.Focus();
            return;
        }



        if (ddlLocation.SelectedValue == "0")
        {
            Session["LocationName"] = "";
            Session["LocationID"] = "0";

        }
        else
        {
            Session["LocationName"] = ddlLocation.SelectedItem.Text;
            Session["LocationID"] = ddlLocation.SelectedValue;
        }
        if (dpDepartment.SelectedIndex <= 0)
        {
            Session["DepName"] = "";
        }
        else
        {
            Session["DepName"] = dpDepartment.SelectedItem.Text;
        }


        Session["EmpList"] = empidlist;
        Session["FromDate"] = txtFromDate.Text;
        Session["ToDate"] = txtToDate.Text;

        Session["Report_EmpList"] = empidlist;
        Session["Report_FromDate"] = txtFromDate.Text;
        Session["Report_ToDate"] = txtToDate.Text;
        Session["Report_FromMonth"] = Convert.ToString(Convert.ToDateTime(txtFromDate.Text).Month);
        Session["Report_FromYear"] = Convert.ToString(Convert.ToDateTime(txtFromDate.Text).Year);


        Div_Main.Visible = false;
        pnlReport.Visible = true;
        lnkChangeFilter.Focus();
        //for (int i = 0; i < empidlist.Split(',').Length; i++)
        //{
        //    if (empidlist.Split(',')[i] == "")
        //    {
        //        continue;
        //    }

        //}  
    }

    protected void lnkChangeFilter_Click(object sender, EventArgs e)
    {
        //  Session["EmpList"] = null;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlLocation.SelectedValue = Session["LociD"].ToString();
        txtFromDate.Text = "";
        Div_Main.Visible = true;
        pnlReport.Visible = false;
        lblSelectRecord.Text = "";
        FillDepartment();
        FillGrid("");
        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);

    }

    public void DisplayMessage(string str)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["EmpList1"] = null;
        Session["FromDate1"] = null;
        Session["ToDate1"] = null;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        lblSelectRecord.Text = "";
        Session["EmpList"] = null;
        ddlLocation.SelectedValue = Session["Locid"].ToString();
        FillDepartment();
        FillGrid("0");
        //DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //if (Session["SessionDepId"] != null)
        //{
        //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        //}
        //if (dtEmp.Rows.Count > 0)
        //{
        //    Session["dtEmp"] = dtEmp;
        //    gvEmployee.DataSource = dtEmp;
        //    gvEmployee.DataBind();
        //    lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        //}

        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        btnaryRefresh_Click1(null, null);
    }

    protected void btnarybind_Click1(object sender, ImageClickEventArgs e)
    {
        if (txtValue.Text.Trim() == "")
        {
            txtValue.Focus();
            DisplayMessage("Please Fill Value");
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
            gvEmployee.DataSource = view.ToTable();
            gvEmployee.DataBind();
            Session["dtEmp1"] = view.ToTable();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            Session["CHECKED_ITEMS"] = null;
        }
        txtValue.Focus();
    }
    protected void btnaryRefresh_Click1(object sender, ImageClickEventArgs e)
    {
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

        FillGrid("");
        Session["dtLeave"] = null;
        ViewState["Select"] = null;
        // lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        ddlField.Focus();
        Session["CHECKED_ITEMS"] = null;

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = (CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }

        }


    }
    // Open Reports In New Window Code
    protected void lnkShiftReport_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('ShiftReport.aspx','_Self','height=650,width=900,scrollbars=Yes')", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/ShiftReport.aspx')", true);
    }

    protected void lnkWithoutShiftReport_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('ShiftReport.aspx','_Self','height=650,width=900,scrollbars=Yes')", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/WithoutShiftReport.aspx')", true);
    }



    protected void LinkButton12_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('InOutExceptionReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }

    protected void linkEmpInfo_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('EmployeeReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkAttRegister_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('AttendanceRegister.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLogProcess_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LogReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void lnkLogReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LogReport_1.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkHalfDay_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('HalfDayReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkBreakInOut_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('BreakInoutReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }


    protected void linkTourAttendance_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('TourAttendanceReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }

    protected void linkTimeCardReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('TimeCard.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }

    protected void lnkAbsentDetail_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('AbsentDate.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }


    protected void lnlClocl10_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('Clock10.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void lnkLateIn_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LateInReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkSmsTransaction_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('SMSTransactionReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkMailTransaction_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('MailTransactionReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLocation_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LeaveFilter.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }

    protected void linkUserTransfer_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('UserTransferReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkDailySalary_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('DailySalaryReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkPartialViolation_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('PartialViolaionReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkOverTime_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('OverTimeReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkPartialLeave_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('PartialLeaveReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkAbsentReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('AbsentReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLeaveRemaning_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LeaveRemainingReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkEarlyOut_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('EarlyOutReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLeaveStatus_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LeaveStatusReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkInOut_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('InOutReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkInOutseperate_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('InOutReport.aspx?_Id=1','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkInOutDriect_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('InOutReport_Direct.aspx?_Id=1','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLeaveReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LeaveReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLatein_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('LateReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkWeekOffReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('WeekoffReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    //protected void linkWeekOffReport_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('WeekoffReportNew.aspx','','height=650,width=950,scrollbars=Yes')", true);
    //}
    protected void linkHolidayReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('HolidayReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }


    protected void linkEmployeeHolidayReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('EmployeeHolidayReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    //-------------------------------

    protected void lnkshiftscheduleReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('Shift_Schedule_Report.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }


    protected void lnkDutyResumtionReport_Click(object sender, EventArgs e)
    {
        string EmpList = string.Empty;

        if (Session["EmpList"] == null)
        {
            Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
        }
        else
        {
            try
            {
                EmpList = Session["EmpList"].ToString().Split(',')[0].ToString();
            }
            catch
            {

            }
        }


        if (EmpList != "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('Vacation Leave Application.aspx?EmpId=" + EmpList + "&&Type=1','','height=650,width=950,scrollbars=Yes')", true);
        }


    }


    protected void lnkSalaryCertificaterequestForm_Click(object sender, EventArgs e)
    {

        string EmpList = string.Empty;

        if (Session["EmpList"] == null)
        {
            Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
        }
        else
        {
            try
            {
                EmpList = Session["EmpList"].ToString().Split(',')[0].ToString();
            }
            catch
            {

            }
        }


        if (EmpList != "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('Vacation Leave Application.aspx?EmpId=" + EmpList + "&&Type=2','','height=650,width=950,scrollbars=Yes')", true);
        }

    }

    protected void Lnk_Attendance_Register_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AttendanceRegister.aspx')", true);
    }

    protected void LNK_Access_Group_Summary_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInquiryHeaderReport.aspx','window','width=1024');", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummary.aspx', 'window', 'width=1024'); ", true);
    }

    protected void LNK_Access_Group_Summary_Detail_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }

    protected void linkPendingHolidayReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/PendingHolidayReport.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }


    protected void linkPendingoffReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/PendingOffReport.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }

    protected void linkTSCOvertimeReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/TSC_OvertimeReport.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }





    protected void linkTimeDeductionReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/TimesDeductionReport.aspx', 'window', 'width=1024'); ", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AccessGroupSummaryDetail.aspx')", true);
    }


    protected void Lnk_Attendance_Summary_Report_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/SummaryReport.aspx','_newtab');", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/SalarySummaryReport.aspx');", true);
    }

    protected void Lnl_Attendance_Salary_Report_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/AttendanceSalaryReport.aspx');", true);
    }

    protected void Lnl_Attendance_TimeSheet_Report_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/TimeSheetReport.aspx','_newtab');", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/TimeSheet_Report.aspx');", true);
    }

    protected void Lnl_Attendance_Salary_Summary_Report_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/SalaryReport.aspx','_newtab');", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/SalarySummaryReport.aspx');", true);
    }

    protected void chkterminatedemployee_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroupSal.Checked)
        {
            lbxGroupSal_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid("");
        }


    }

    protected void lnkExceptionCount_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../attendance_report/exceptioncountreport.aspx');", true);
    }

    protected void lnkEmployeeTracking_Click(object sender, EventArgs e)
    {
        Session["EmpDataExceptionCount"] = null;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../attendance_report/employeeTracking.aspx');", true);
    }
}
