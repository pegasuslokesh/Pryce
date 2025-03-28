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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using DevExpress.XtraReports.UI;

public partial class Attendance_Report_AttendanceSalaryReport : BasePage
{
    XtraReport RptShift = new XtraReport();
    Common ObjComman = null;
    Attendance objAttendance = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Set_ApplicationParameter objAppParam = null;
    Att_AttendanceRegister objAttReg = null;
    //Att_AttendanceSalaryReport RptShift = new Att_AttendanceSalaryReport();
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    // Filter Criteria According to Location and Department
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    BrandMaster objBrand = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_AttendanceSalaryReport.repx");
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ScriptManager sm = (ScriptManager)Master.FindControl("SM1");
        sm.RegisterPostBackControl(btnLogProcess);
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("117", (DataTable)Session["ModuleName"]);
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

        ObjComman = new Common(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objAttReg = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        //Att_AttendanceSalaryReport RptShift = new Att_AttendanceSalaryReport();
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        // Filter Criteria According to Location and Department
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "117", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            txtYear.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
            ddlMonth.SelectedValue = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Month.ToString();
            ddlLocation.Focus();
            pnlEmpAtt.Visible = true;
            FillddlDeaprtment();
            FillddlLocation();
            FillGrid();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            EmpGroupSal_CheckedChanged(null, null);
            pnlReport.Visible = false;
            GetReportPermission();
            //Session["CHECKED_ITEMS"] = null;

            // Created by ghanshyam suthar on 1/nov/2017
            if (Session["Report_EmpList"] != null)
            {
                Session["SelectedEmpId"] = Session["Report_EmpList"];
                ddlMonth.SelectedValue = Session["Report_FromMonth"].ToString();
                txtYear.Text = Session["Report_FromYear"].ToString();

                Session["DtShortCode"] = null;

                DataTable dtColorCode = new DataTable();
                dtColorCode.Columns.Add("Type");
                dtColorCode.Columns.Add("ShortId");
                dtColorCode.Columns.Add("ColorCode");

                DataRow dr1 = dtColorCode.NewRow();
                dr1["Type"] = "Absent";
                dr1["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr1["ShortId"] = "A";
                dtColorCode.Rows.Add(dr1);

                DataRow dr2 = dtColorCode.NewRow();
                dr2["Type"] = "Normal";
                dr2["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr2["ShortId"] = "N";
                dtColorCode.Rows.Add(dr2);


                DataRow dr3 = dtColorCode.NewRow();
                dr3["Type"] = "Holiday";
                dr3["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr3["ShortId"] = "H";
                dtColorCode.Rows.Add(dr3);

                DataRow dr4 = dtColorCode.NewRow();
                dr4["Type"] = "Leave";
                dr4["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr4["ShortId"] = "L";
                dtColorCode.Rows.Add(dr4);
                DataRow dr5 = dtColorCode.NewRow();

                dr5["Type"] = "WeekOff";
                dr5["ColorCode"] = "66FF99";
                dr5["ShortId"] = "W";
                dtColorCode.Rows.Add(dr5);

                Session["DtShortCode"] = dtColorCode;

                GetReport();
                pnlEmpAtt.Visible = false;
                pnlReport.Visible = true;
            }
            //----------- End By ghanshyam suthar--------------
        }

        GetReport();

    }

    protected void DocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }

    protected void DocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }


    private void GetReportPermission()
    {
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), "9", "73", HttpContext.Current.Session["CompId"].ToString());
        for (int i = 0; i < dtAllPageCode.Rows.Count; i++)
        {
            btnLogProc.Visible = true;
        }
    }
    protected void btnLogProc_Click(object sender, EventArgs e)
    {
        string url = "../Attendance/LogProcess.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
        ImageButton1.Focus();
        return;
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
            //ListItem li = new ListItem("--Select--", "0");
            ddlLocation.Items.Insert(0, "--Select--");


        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                //ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                //ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }
    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }

        dt = dt.DefaultView.ToTable(true, "DeptName", "Dep_Id");



        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)dpDepartment, dt, "DeptName", "Dep_Id");
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
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGrid();
                }
                else
                {

                    dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

                    if (ddlLocation.SelectedIndex == 0)
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    if (Session["SessionDepId"] != null)
                    {
                        dt = new DataView(dt, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    if (dpDepartment.SelectedIndex != 0)
                    {
                        try
                        {
                            dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        gvEmployee.DataSource = dt;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                        dpDepartment.Focus();
                    }
                    else
                    {
                        gvEmployee.DataSource = null;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                }





            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }

    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGrid();
                }
                else
                {

                    dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

                    if (ddlLocation.SelectedIndex == 0)
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    }

                    if (dpDepartment.SelectedIndex != 0)
                    {
                        try
                        {
                            dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        gvEmployee.DataSource = dt;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        btnLogProc.Focus();
                    }
                    else
                    {
                        gvEmployee.DataSource = null;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                }





            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }

    }
    public void FillGrid()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


        if (ddlLocation.SelectedIndex == 0)
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }

        if (dpDepartment.SelectedIndex != 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        }

        //if (Session["SessionDepId"] != null)
        //{
        //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        //}
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }
    }
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        ddlLocation.Focus();


    }
    #endregion
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
        ViewState["SalaryReportSelected"] = 0;
        if (rbtnGroupSal.Checked)
        {
            pnlEmp.Visible = false;
            pnlGroupSal.Visible = true;

            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            pnlSearchdpl.Visible = false;

            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
            //--------------------------------------
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
        }
        else if (rbtnEmpSal.Checked)
        {
            pnlEmp.Visible = true;
            pnlGroupSal.Visible = false;

            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;
            pnlSearchdpl.Visible = true;

            lblEmp.Text = "";
            lblSelectRecord.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (Session["SessionDepId"] != null)
            {

                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }


            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp4"] = dtEmp;

                gvEmployee.DataSource = dtEmp;
                gvEmployee.DataBind();

            }
            else
            {
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();

            }

        }


    }

    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        gvEmployeeSal.DataSource = (DataTable)Session["dtEmp"];
        gvEmployeeSal.DataBind();
    }

    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmployee.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dt;
        gvEmployee.DataBind();
        PopulateCheckedValues();
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
    //OLD CODE
    //    gvEmployee.PageIndex = e.NewPageIndex;
    //    DataTable dtEmp = (DataTable)Session["dtEmp"];
    //    gvEmployee.DataSource = dtEmp;
    //    gvEmployee.DataBind();
    //    string temp = string.Empty;


    //    for (int i = 0; i < gvEmployee.Rows.Count; i++)
    //    {
    //        Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
    //        string[] split = lblSelectRecord.Text.Split(',');

    //        for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
    //        {
    //            if (lblSelectRecord.Text.Split(',')[j] != "")
    //            {
    //                if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
    //                {
    //                    ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
    //                }
    //            }
    //        }
    //    }
    //}

    //protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
    //    if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectRecord.Text += empidlist;

    //    }

    //    else
    //    {

    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectRecord.Text += empidlist;
    //        string[] split = lblSelectRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != empidlist)
    //            {
    //                if (item != "")
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //        }
    //        lblSelectRecord.Text = temp;
    //    }
    //}



    protected void ImgbtnSelectAll_Clickary(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtAttendenceReport = (DataTable)Session["dtEmp"];
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {

            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAttendenceReport.Rows)
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

            DataTable dtAddressCategory1 = (DataTable)Session["dtEmp"];
            gvEmployee.DataSource = dtAddressCategory1;
            gvEmployee.DataBind();
            ViewState["Select"] = null;
        }


        //OLD CODE
        //DataTable dtProduct = (DataTable)Session["dtEmp"];

        //if (ViewState["Select"] == null)
        //{
        //    ViewState["Select"] = 1;
        //    foreach (DataRow dr in dtProduct.Rows)
        //    {
        //        if (!lblSelectRecord.Text.Split(',').Contains(dr["Emp_Id"]))
        //        {
        //            lblSelectRecord.Text += dr["Emp_Id"] + ",";
        //        }
        //    }
        //    for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //    {
        //        string[] split = lblSelectRecord.Text.Split(',');
        //        Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //        for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
        //        {
        //            if (lblSelectRecord.Text.Split(',')[j] != "")
        //            {
        //                if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
        //                {
        //                    ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    lblSelectRecord.Text = "";
        //    DataTable dtProduct1 = (DataTable)Session["dtEmp"];
        //    gvEmployee.DataSource = dtProduct1;
        //    gvEmployee.DataBind();
        //    ViewState["Select"] = null;
        //}

    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        SaveCheckedValues();
        lblSelectRecord.Text = "";
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
        {

            userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (userdetails.Count == 0)
            {
                //DisplayMessage("Select Employee First");
                //return;
            }
            else
            {


                for (int i = 0; i < userdetails.Count; i++)
                {
                    lblSelectRecord.Text += userdetails[i].ToString() + ",";
                }

            }

        }
        else
        {
            DisplayMessage("Select Employee First");
            gvEmployee.Focus();
            return;
        }
        //

        int b = 0;
        // Selected Emp Id 
        string empidlist = lblSelectRecord.Text;


        // string empidlist = hdnFldSelectedValues.Value.ToString();
        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Please select month");
            ddlMonth.Focus();
            return;

        }
        if (txtYear.Text == "")
        {
            DisplayMessage("Please enter year");
            txtYear.Focus();
            return;

        }
        ViewState["SalaryReportSelected"] = "1";

        if (rbtnEmpSal.Checked)
        {
            if (empidlist == "")
            {
                DisplayMessage("Select Atleast One Employee");
                return;
            }
            else
            {
                Session["SelectedEmpId"] = empidlist;
            }
        }
        else
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
                else
                {
                    Session["SelectedEmpId"] = empidlist;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }

        }

        pnlEmpAtt.Visible = false;
        pnlReport.Visible = true;

        Panel1.Visible = true;

        GetReport();
        //for (int i = 0; i < empidlist.Split(',').Length; i++)
        //{
        //    if (empidlist.Split(',')[i] == "")
        //    {
        //        continue;
        //    }

        //}       








    }



    public void GetReport()
    {


        if (Session["SelectedEmpId"] == null)
        {
            return;
        }


        DateTime dtFrom = new DateTime(Convert.ToInt32(txtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue), 1);

        int totalDays = DateTime.DaysInMonth(Convert.ToInt32(txtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue.ToString()));
        DateTime dtTo = dtFrom.AddDays(totalDays - 1);

        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Pay_Employee_Attendance_Select_RowTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Pay_Employee_Attendance_Select_RowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Pay_Employee_Attendance_Select_Row, 0, int.Parse(ddlMonth.SelectedValue), int.Parse(txtYear.Text), int.Parse(Session["CompId"].ToString()), "2");

        DataTable dtFilter = new DataTable();
        if (Session["SelectedEmpId"] != "")
        {
            dtFilter = new DataView(rptdata.sp_Pay_Employee_Attendance_Select_Row, "Emp_Id in (" + Session["SelectedEmpId"].ToString().Substring(0, Session["SelectedEmpId"].ToString().Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            rptdata.sp_Pay_Employee_Attendance_Select_Row.Rows.Clear();
        }
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string DepartmentName = "";
        // Get Company Name
        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
        // Get Brand Name
        BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());
        // Get Location Name
        if (Session["LocName"].ToString() == "")
        {
            LocationName = objAttendance.GetLocationName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["Lang"].ToString());
        }
        else
        {
            LocationName = Session["LocName"].ToString();
        }
        // Get Department Name
        //if (Session["DepName"].ToString() == "")
        //{
        //    DepartmentName = "All";
        //}
        //else
        //{
        //    DepartmentName = Session["DepName"].ToString();
        //}
        // Get Company Address
        DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }



        //---Company Logo
        XRPictureBox Company_Logo = (XRPictureBox)RptShift.FindControl("Company_Logo", true);
        try
        {
            Company_Logo.ImageUrl = Imageurl;
        }
        catch
        {
        }
        //------------------

        //Comany Name
        XRLabel Lbl_Company_Name = (XRLabel)RptShift.FindControl("Lbl_Company_Name", true);
        Lbl_Company_Name.Text = CompanyName;
        //------------------

        //Comapny Address
        XRLabel Lbl_Company_Address = (XRLabel)RptShift.FindControl("Lbl_Company_Address", true);
        Lbl_Company_Address.Text = CompanyAddress;
        //------------------


        //Brand Name
        XRLabel Lbl_Brand = (XRLabel)RptShift.FindControl("Lbl_Brand", true);
        Lbl_Brand.Text = Resources.Attendance.Brand + " : ";
        XRLabel Lbl_Brand_Name = (XRLabel)RptShift.FindControl("Lbl_Brand_Name", true);
        Lbl_Brand_Name.Text = BrandName;
        //------------------

        // Location Name
        XRLabel Lbl_Location = (XRLabel)RptShift.FindControl("Lbl_Location", true);
        Lbl_Location.Text = Resources.Attendance.Location + " : ";
        XRLabel Lbl_Location_Name = (XRLabel)RptShift.FindControl("Lbl_Location_Name", true);
        Lbl_Location_Name.Text = LocationName;
        //------------------

        // Department Name
        XRLabel Lbl_Department = (XRLabel)RptShift.FindControl("Lbl_Department", true);
        Lbl_Department.Text = Resources.Attendance.Department + " : ";
        XRLabel Lbl_Department_Name = (XRLabel)RptShift.FindControl("Lbl_Department_Name", true);
        Lbl_Department_Name.Text = DepartmentName;
        //------------------

        // Report Title
        XRLabel Report_Title = (XRLabel)RptShift.FindControl("Report_Title", true);
        Report_Title.Text = "Attendance Salary Report " + "For  Month : " + ddlMonth.SelectedItem.Text + " Year :" + txtYear.Text;
        //------------------



        // Detail Header Table
        XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
        xrTableCell1.Text = "Id";

        XRLabel xrTableCell5 = (XRLabel)RptShift.FindControl("xrTableCell5", true);
        xrTableCell5.Text = "Designation";

        XRLabel xrTableCell32 = (XRLabel)RptShift.FindControl("xrTableCell32", true);
        xrTableCell32.Text = "Brand";

        XRLabel xrTableCell11 = (XRLabel)RptShift.FindControl("xrTableCell11", true);
        xrTableCell11.Text = "Employee Type";

        XRLabel xrTableCell41 = (XRLabel)RptShift.FindControl("xrTableCell41", true);
        xrTableCell41.Text = "Total Working Shift";

        XRLabel xrTableCell6 = (XRLabel)RptShift.FindControl("xrTableCell6", true);
        xrTableCell6.Text = "Absent";

        XRLabel xrTableCell60 = (XRLabel)RptShift.FindControl("xrTableCell60", true);
        xrTableCell60.Text = "Holiday";

        XRLabel xrTableCell51 = (XRLabel)RptShift.FindControl("xrTableCell51", true);
        xrTableCell51.Text = "Total Days";

        XRLabel xrTableCell69 = (XRLabel)RptShift.FindControl("xrTableCell69", true);
        xrTableCell69.Text = "Total Assign Hour";

        XRLabel xrTableCell25 = (XRLabel)RptShift.FindControl("xrTableCell25", true);
        xrTableCell25.Text = "Late Count";

        XRLabel xrTableCell66 = (XRLabel)RptShift.FindControl("xrTableCell66", true);
        xrTableCell66.Text = "Early Leave Count";

        XRLabel xrTableCell33 = (XRLabel)RptShift.FindControl("xrTableCell33", true);
        xrTableCell33.Text = "Normal OT Count";

        XRLabel xrTableCell23 = (XRLabel)RptShift.FindControl("xrTableCell23", true);
        xrTableCell23.Text = "Holiday OT Count";

        XRLabel xrTableCell12 = (XRLabel)RptShift.FindControl("xrTableCell12", true);
        xrTableCell12.Text = "Week Off OT Count";

        XRLabel xrTableCell61 = (XRLabel)RptShift.FindControl("xrTableCell61", true);
        xrTableCell61.Text = "Basic Salary";

        XRLabel xrTableCell9 = (XRLabel)RptShift.FindControl("xrTableCell9", true);
        xrTableCell9.Text = "Work Hour Salary";

        XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
        xrTableCell13.Text = "Week Off OT Salary";

        XRLabel xrTableCell52 = (XRLabel)RptShift.FindControl("xrTableCell52", true);
        xrTableCell52.Text = "Gross Total";

        XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
        xrTableCell4.Text = "Name";

        XRLabel xrTableCell8 = (XRLabel)RptShift.FindControl("xrTableCell8", true);
        xrTableCell8.Text = "Department";

        XRLabel xrTableCell40 = (XRLabel)RptShift.FindControl("xrTableCell40", true);
        xrTableCell40.Text = "Location";

        XRLabel xrTableCell19 = (XRLabel)RptShift.FindControl("xrTableCell19", true);
        xrTableCell19.Text = "Present Shift";

        XRLabel xrTableCell55 = (XRLabel)RptShift.FindControl("xrTableCell55", true);
        xrTableCell55.Text = "Leave";

        XRLabel xrTableCell64 = (XRLabel)RptShift.FindControl("xrTableCell64", true);
        xrTableCell64.Text = "Week Off";

        XRLabel xrTableCell53 = (XRLabel)RptShift.FindControl("xrTableCell53", true);
        xrTableCell53.Text = "Paid Days";

        XRLabel xrTableCell71 = (XRLabel)RptShift.FindControl("xrTableCell71", true);
        xrTableCell71.Text = "Work Hour";

        XRLabel xrTableCell48 = (XRLabel)RptShift.FindControl("xrTableCell48", true);
        xrTableCell48.Text = "Late Hour";

        XRLabel xrTableCell24 = (XRLabel)RptShift.FindControl("xrTableCell24", true);
        xrTableCell24.Text = "Early Leave Hour";

        XRLabel xrTableCell35 = (XRLabel)RptShift.FindControl("xrTableCell35", true);
        xrTableCell35.Text = "Normal OT Hour";

        XRLabel xrTableCell30 = (XRLabel)RptShift.FindControl("xrTableCell30", true);
        xrTableCell30.Text = "Holiday OT Hour";

        XRLabel xrTableCell16 = (XRLabel)RptShift.FindControl("xrTableCell16", true);
        xrTableCell16.Text = "Week Off OT Hour";

        XRLabel xrTableCell27 = (XRLabel)RptShift.FindControl("xrTableCell27", true);
        xrTableCell27.Text = "OT Salary";

        XRLabel xrTableCell21 = (XRLabel)RptShift.FindControl("xrTableCell21", true);
        xrTableCell21.Text = "Holiday OT Salary";        
        //-------------------------------------------
        
        //Footer
        // Create by
        XRLabel Lbl_Created_By = (XRLabel)RptShift.FindControl("Lbl_Created_By", true);
        Lbl_Created_By.Text = Resources.Attendance.Created_By;
        XRLabel Lbl_Created_By_Name = (XRLabel)RptShift.FindControl("Lbl_Created_By_Name", true);
        Lbl_Created_By_Name.Text = Session["UserId"].ToString();
        //--------------------



        //RptShift.SetImage(Imageurl);
        //RptShift.SetBrandName(BrandName);
        //RptShift.SetLocationName(LocationName);
        //RptShift.SetDepartmentName(DepartmentName);
        //RptShift.setTitleName("Attendance Salary Report " + "For  Month : " + ddlMonth.SelectedItem.Text + " Year :" + txtYear.Text);
        //RptShift.setcompanyname(CompanyName);
        //RptShift.setaddress(CompanyAddress);
        //RptShift.setyearmonth(txtYear.Text, ddlMonth.SelectedValue);
        //RptShift.setUserName(Session["UserId"].ToString());
        //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Brand, Resources.Attendance.Location, Resources.Attendance.Employee_Type, Resources.Attendance.Total_Working_Shift, Resources.Attendance.Present_Shift, Resources.Attendance.Absent, Resources.Attendance.Leave, Resources.Attendance.Holiday, Resources.Attendance.Week_Off, Resources.Attendance.Total_Days, Resources.Attendance.Paid_Days, Resources.Attendance.Total_Assign_Hour, Resources.Attendance.Work_Hour, Resources.Attendance.Late_Count, Resources.Attendance.Late_Hour, Resources.Attendance.Early_Leave_Count, Resources.Attendance.Early_Leave_Hour, Resources.Attendance.Normal_OT_Count, Resources.Attendance.Normal_OT_Hour, Resources.Attendance.Holiday_OT_Count, Resources.Attendance.Holiday_OT_Hour, Resources.Attendance.Week_Off_OT_Count, Resources.Attendance.Week_Off_OT_Hour, Resources.Attendance.Basic_Salary, Resources.Attendance.WorkHourSalary, Resources.Attendance.OT_Salary, Resources.Attendance.Week_Off_OT_Salary, Resources.Attendance.Holiday_OT_Salary, Resources.Attendance.Gross_Total);
        RptShift.DataSource = dtFilter;
        RptShift.DataMember = "sp_Pay_Employee_Attendance_Select_Row";
        rptViewer.Report = RptShift;
        rptToolBar.ReportViewer = rptViewer;




    }

    protected void lnkback_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
        pnlEmpAtt.Visible = true;
        btnReset_Click(null, null);
        Session["SelectedEmpId"] = null;
    }



    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtYear.Text = "";
        ddlMonth.SelectedIndex = 0;

        lblSelectRecord.Text = "";
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }

        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        btnaryRefresh_Click1(null, null);
    }


    protected void btnarybind_Click1(object sender, ImageClickEventArgs e)
    {
        if (txtValue.Text == "")
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
            gvEmployee.DataSource = view.ToTable();
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            
        }
        txtValue.Focus();
    }

    protected void ddlField_SelectedIndexChanged(Object sender, EventArgs e)
    {
        ddlOption.Focus();
        return;
    }
    protected void ddlOption_SelectedIndexChanged(Object sender, EventArgs e)
    {
        txtValue.Focus();
        return;
    }

    protected void btnaryRefresh_Click1(object sender, ImageClickEventArgs e)
    {
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
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
    //OLD CODE
    //CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
    //for (int i = 0; i < gvEmployee.Rows.Count; i++)
    //{
    //    ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
    //    if (chkSelAll.Checked)
    //    {
    //        if (!lblSelectRecord.Text.Split(',').Contains(((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
    //        {
    //            lblSelectRecord.Text += ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
    //        }
    //    }
    //    else
    //    {
    //        string temp = string.Empty;
    //        string[] split = lblSelectRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
    //            {
    //                if (item != "")
    //                {
    //                    temp += item + ",";
    //                }
    //            }
    //        }
    //        lblSelectRecord.Text = temp;
    //    }
    //}


}



