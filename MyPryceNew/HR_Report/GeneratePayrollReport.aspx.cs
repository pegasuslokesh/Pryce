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

public partial class HR_Report_GeneratePayrollReport : BasePage
{
    #region defind Class Object
    Set_Deduction objDeduction = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_Employee_Month objPayEmpMonth = null;
    EmployeeParameter objempparam = null;
    Pay_Employee_Penalty objPEpenalty = null;
    Pay_Employee_claim objPEClaim = null;
    Common ObjComman = null;
    Pay_Employee_claim ObjClaim = null;
    Pay_Employee_Deduction objpayrolldeduc = null;
    CountryMaster ObjSysCountryMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocMaster = null;
    Set_Allowance ObjAllow = null;
    Set_Deduction ObjDeduc = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Pay_Employee_Attendance objEmpAttendance = null;
    Set_Group_Employee objGroupEmp = null;
    DepartmentMaster objDep = null;
    ReligionMaster objRel = null;
    NationalityMaster objNat = null;
    DesignationMaster objDesg = null;
    QualificationMaster objQualif = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    UserMaster objUser = null;
    Set_ApplicationParameter objAppParam = null;
    CurrencyMaster objCurrency = null;
    CompanyMaster objComp = null;
    EmployeeParameter objEmpParam = null;
    Pay_Employee_Loan ObjLoan = null;
    Pay_Employee_Penalty ObjPenalty = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Pay_Employe_Allowance objpayrollall = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    PageControlCommon objPageCmn = null;
    RoleMaster objRole = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["AccordianId"] = "109";
        Session["HeaderText"] = "HR Report";

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDeduction = new Set_Deduction(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objPEpenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objPEClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objpayrolldeduc = new Pay_Employee_Deduction(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objEmpAttendance = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objRel = new ReligionMaster(Session["DBConnection"].ToString());
        objNat = new NationalityMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objQualif = new QualificationMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        ObjPenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }
            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtPayrollReport"] = null;
            TxtYear.Text = DateTime.Now.Year.ToString();
            int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
            ddlMonth.SelectedValue = (CurrentMonth).ToString();

            Session["empimgpath"] = null;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            bool IsCompOT = false;
            bool IsPartialComp = false;

            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_PayrollReport_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            catch
            {

            }

            FillddlDeaprtment();
            FillddlLocation();

            btnPayrollReport_Click(null, null);

            try
            {
                gvEmployee.PageSize = PageControlCommon.GetPageSize();
                gvEmpPayrollReport.PageSize = PageControlCommon.GetPageSize();
            }
            catch
            {

            }

            GetReportPermission();
            ddlLocation.Focus();
            Session["Querystring"] = null;
            try
            {
                Div_Terminated_Report.Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission(Common.GetObjectIdbyPageURL("../hr_Report/GeneratepayrollReport.aspx", Session["DBConnection"].ToString()), "14", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {

            }
        }
        Page.Title = objSys.GetSysTitle();
        if (Session["Querystring"] != null)
        {
            PanelReport.Visible = true;
            PnlEmployeePayrollReport.Visible = false;
        }
    }
    public void GetReportPermission()
    {
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
        DataTable dtAllChild = new DataView(ObjComman.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id), "Module_Id=109", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
        for (int i = 0; i < dtAllChild.Rows.Count; i++)
        {
            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "135")
            {
                lnkClaimReport.Visible = true;
            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "136")
            {
                lnkPenaltyReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "137")
            {
                lnkAllowanceReport.Visible = true;


            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "139")
            {
                lnkDeductionReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "141")
            {
                lnkLOanApprovedReport.Visible = true;


            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "142")
            {
                lnkLOanSummaryReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "143")
            {
                lnkLOanInstallmentReport.Visible = true;


            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "144")
            {
                lnkPayslipReport.Visible = true;
                Lnk_Pay_Slip.Visible = true;
            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "145")
            {
                lnkTotalSalaryReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "146")
            {
                lnkPFReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "147")
            {
                lnkESICReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "148")
            {
                lnksettlementReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "149")
            {
                lnknonadjustmentReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "150")
            {
                lnkDirectoryReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "151")
            {
                lnkDocExpiryReport.Visible = true;

            }

            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "152")
            {
                LnkPaySummaryBeforepost.Visible = true;
                lnkPayrollsummaryReport.Visible = true;
                Lnk_Projected_Salary_Transfer.Visible = true;
            }
            if (dtAllChild.Rows[i]["Object_Id"].ToString().Trim() == "262")
            {
                lnkSummarySalaryStatement.Visible = true;


            }

        }

    }

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
            ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
            ddlLocation.Items.Insert(0, li);


        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }

    private void FillddlDeaprtment()
    {
        dpDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }
        dtDepartment = objDep.GetDepartmentMaster();

        strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0,";
        }

        if (Session["EmpId"].ToString() != "0")
        {
            dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        FillddlDeaprtment();
        ddlLocation.Focus();

    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        dpDepartment.Focus();
    }
    public void FillGrid()
    {
        string strLocationDept = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }


        strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strLocationDept == "")
        {
            strLocationDept = "0";
        }

        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

        if (chkterminatedemployee.Checked)
        {
            dtEmp = objEmp.GetEmployee_InPayroll_WithTerminated(Session["CompId"].ToString());
        }

        // Nitin Jasin , Get According to UserId to Get Records for Single User 
        DataTable dt = objUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }


        if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
        {

            Session["CHECKED_ITEMS"] = null;
            ddlField1.SelectedIndex = 1;
            ddlOption1.SelectedIndex = 2;
            txtValue1.Text = "";
        }


        if (!IsSingleUser)
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            }



            if (strLocationDept != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strLocationDept.Substring(0, strLocationDept.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


            if (dpDepartment.SelectedIndex != 0)
            {
                dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            }
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //if (Session["SessionDepId"] != null)
        //{
        //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        //}
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpPayrollReport"] = dtEmp;
            gvEmpPayrollReport.DataSource = dtEmp;
            gvEmpPayrollReport.DataBind();
            lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            ViewState["Select"] = null;
        }
        else
        {
            gvEmpPayrollReport.DataSource = null;
            gvEmpPayrollReport.DataBind();
            Session["dtEmpPayrollReport"] = dtEmp;
            lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

    }
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        Session["dtPayrollReport"] = null;
        ddlLocation.Focus();

    }

    protected void lbxGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

        string strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        DataTable dt = objUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }


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
            DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

            if (chkterminatedemployee.Checked)
            {
                dtEmp = objEmp.GetEmployee_InPayroll_WithTerminated(Session["CompId"].ToString());
            }

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (strLocationDept != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strLocationDept.Substring(0, strLocationDept.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
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

            if (IsSingleUser)
            {
                dtEmp = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp1"] = dtEmp;
                gvEmployee.DataSource = dtEmp;
                gvEmployee.DataBind();

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
        lbxGroup.Focus();
    }
    protected void gvEmpPayrollReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmpPayrollReport.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpPayrollReport"];
        gvEmpPayrollReport.DataSource = dt;
        gvEmpPayrollReport.DataBind();
        PopulateCheckedValues();


    }
    protected void chkgvSelectAll_CheckedChangedPayrollReport(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmpPayrollReport.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvEmpPayrollReport.Rows)
        {
            index = (int)gvEmpPayrollReport.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpPayrollReport.Rows)
            {
                int index = (int)gvEmpPayrollReport.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmpPayrollReport.Rows)
        {
            index = (int)gvEmpPayrollReport.DataKeys[gvrow.RowIndex].Value;
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

    protected void ImgbtnSelectAll_ClickPayrollReport(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtPayrollReport = (DataTable)Session["dtEmpPayrollReport"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPayrollReport.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmpPayrollReport.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpPayrollReport"];
            gvEmpPayrollReport.DataSource = dtAddressCategory1;
            gvEmpPayrollReport.DataBind();
            ViewState["Select"] = null;
        }
    }
    protected void btnPayrollReportRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

        FillGrid();
        Session["dtPayrollReport"] = null;
        txtValue1.Focus();

    }
    protected void GridViewEmpPayrollReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpPayrollReport.Value = hdFSortgvEmpPayrollReport.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpPayrollReport"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdFSortgvEmpPayrollReport.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();

        gvEmpPayrollReport.DataSource = dt;
        gvEmpPayrollReport.DataBind();
        gvEmpPayrollReport.HeaderRow.Focus();
        ViewState["Select"] = null;

    }
    protected void btnPayrollReportbind_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["dtEmpPayrollReport"] != null)
        {


            if (ddlOption1.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption1.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
                }
                else if (ddlOption1.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";

                }
                DataTable dtEmp = (DataTable)Session["dtEmpPayrollReport"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                gvEmpPayrollReport.DataSource = view.ToTable();
                gvEmpPayrollReport.DataBind();
                ViewState["Select"] = null;
                Session["dtEmpPayrollReport"] = view.ToTable();
                lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";


            }
        }

        Session["dtPayrollReport"] = null;
        txtValue1.Focus();

    }


    protected void ChangeFilter_OnClick(object sender, EventArgs e)
    {


        btnPayrollReport_Click(null, null);

        PanelReport.Visible = false;

        PnlEmployeePayrollReport.Visible = true;
        Session["ClaimRecord"] = null;

        Session["Querystring"] = null;

        ViewState["Select"] = null;

        Session["Month"] = null;
        Session["Year"] = null;

    }

    protected void btnPayrollReport_Click(object sender, EventArgs e)
    {

        string strLocationDept = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }


        strLocationDept = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strLocationDept == "")
        {
            strLocationDept = "0";
        }


        // Nitin Jasin , Get According to UserId to Get Records for Single User 
        DataTable dt = objUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }

        pnlPayrollReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlEmployeePayrollReport.Visible = true;

        PanelReport.Visible = false;


        Session["CHECKED_ITEMS"] = null;
        gvEmpPayrollReport.Visible = true;
        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

        if (chkterminatedemployee.Checked)
        {
            dtEmp = objEmp.GetEmployee_InPayroll_WithTerminated(Session["CompId"].ToString());
        }
        if (!IsSingleUser)
        {

            if (ddlLocation.SelectedIndex == 0)
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            }

            if (strLocationDept != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strLocationDept.Substring(0, strLocationDept.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


            if (dpDepartment.SelectedIndex != 0)
            {
                dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            }

        }
        else
        {
            if (IsSingleUser)
            {
                dtEmp = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpPayrollReport"] = dtEmp;
            gvEmpPayrollReport.DataSource = dtEmp;
            gvEmpPayrollReport.DataBind();
            lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            ViewState["Select"] = null;
        }
        else
        {
            Session["dtEmpPayrollReport"] = dtEmp;
            gvEmpPayrollReport.DataSource = dtEmp;
            gvEmpPayrollReport.DataBind();
            lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            ViewState["Select"] = null;

        }
        Session["dtPayrollReport"] = null;


        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);

    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        gvEmployee.DataSource = (DataTable)Session["dtEmp1"];
        gvEmployee.DataBind();

    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = objUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }
        if (rbtnGroup.Checked)
        {
            Panel4.Visible = false;
            pnlGroup.Visible = true;

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
                lbxGroup.DataSource = dtGroup;
                lbxGroup.DataTextField = "Group_Name";
                lbxGroup.DataValueField = "Group_Id";

                lbxGroup.DataBind();

            }



            lbxGroup_SelectedIndexChanged(null, null);
            lbxGroup.Focus();
        }
        else if (rbtnEmp.Checked)
        {
            Panel4.Visible = true;
            pnlGroup.Visible = false;

            lblEmp.Text = "";
            FillGrid();


            //string GroupIds = string.Empty;
            //string EmpIds = string.Empty;
            //DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

            //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            //try
            //{
            //    if (HttpContext.Current.Session["SessionDepId"] != null)
            //    {
            //        dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            //    }
            //}
            //catch
            //{
            //}
            //if (IsSingleUser)
            //{
            //    dtEmp = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}

            //if (dtEmp.Rows.Count > 0)
            //{
            //    Session["dtEmp"] = dtEmp;
            //    gvEmployee.DataSource = dtEmp;
            //    gvEmployee.DataBind();


            //}
            ddlLocation.Focus();

        }


    }

    public void DisplayMessage(string str,string color="orange")
    {

        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }

    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void btnLeast_Click(object sender, EventArgs e)
    {

        pnlPayrollReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlEmployeePayrollReport.Visible = false;

        PanelReport.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Session["ClaimRecord"] = null;

        Session["Querystring"] = null;
        if (ddlMonth.SelectedValue == "0")
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;

        }
        if (TxtYear.Text == "")
        {
            DisplayMessage("Enter Year");
            TxtYear.Focus();
            return;
        }
        if (ddlLocation.SelectedIndex == 0)
        {
            Session["LocationName"] = "All";
        }
        else
        {            
            Session["LocationName"] = ddlLocation.SelectedItem.Text.ToString();
        }

        if (dpDepartment.SelectedIndex == 0)
        {
            Session["DepartmentName"] = "All";
            Session["DepName"] = "All";
        }
        else
        {
            Session["DepName"] = dpDepartment.SelectedItem.Text.ToString();
            Session["DepartmentName"] = dpDepartment.SelectedItem.Text.ToString();
        }


        Session["Month"] = ddlMonth.SelectedValue;
        Session["Year"] = TxtYear.Text;
        Session["MonthName"] = ddlMonth.SelectedItem.Text;
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



            if (GroupIds != "")
            {







                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

                        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                        try
                        {
                            if (HttpContext.Current.Session["SessionDepId"] != null)
                            {
                                dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                            }
                        }
                        catch
                        {
                        }
                        dtEmp = new DataView(dtEmp, "Emp_Id='" + dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtEmp.Rows.Count > 0)
                        {
                            EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                        }
                    }
                }
                if (EmpIds == "")
                {
                    DisplayMessage("Employee Not Exists");
                    return;
                }
                else
                {
                    PanelReport.Visible = true;
                    PnlEmployeePayrollReport.Visible = false;
                    Session["Querystring"] = EmpIds;

                }


            }
            else
            {
                DisplayMessage("Select Group First");
                lbxGroup.Focus();
            }
        }
        else
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
                else
                {
                    lblSelectRecd.Text = "";

                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        lblSelectRecd.Text += userdetails[i].ToString() + ",";
                    }
                    Session["Querystring"] = lblSelectRecd.Text;
                    Session["EmpList"] = lblSelectRecd.Text;
                    PanelReport.Visible = true;
                    PnlEmployeePayrollReport.Visible = false;


                }

            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }

        }



    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlMonth.SelectedIndex = Convert.ToInt32(DateTime.Now.Month);
        TxtYear.Text = DateTime.Now.Year.ToString();
        btnPayrollReport_Click(null, null);

    }



    public string GetEmployeeName(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Name"].ToString();

            if (empname == "")
            {
                empname = "No Name";
            }

        }
        else
        {
            empname = "No Name";

        }

        return empname;



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





    protected void lnkDeductionDetail_OnClick(object sender, EventArgs e)
    {


        Response.Redirect("../HR_Report/Employee_DeductionByDeduction.aspx");




    }


    protected void lnkAllowanceDetail_OnClick(object sender, EventArgs e)
    {



        Response.Redirect("../HR_Report/Employee_ByAllowance_Report.aspx");


    }
    protected void lnkPenalty_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/PenaltyReport.aspx')", true);




    }

    protected void lnkAllowance_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_Allowance_Report.aspx')", true);













    }


    protected void lnkDeduction_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_Deduction_Report.aspx')", true);






    }


    protected void lnkClaim_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/ClaimReport.aspx')", true);




    }




    protected void lnkLoanApproval_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/LoanReport.aspx')", true);



    }
    protected void lnkloanSummary_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/LoanDetailReport.aspx?LoanType=1')", true);







    }
    protected void lnkLoanInstallment_OnClick(object sender, EventArgs e)
    {




        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/LoanDetailReport.aspx?LoanType=2')", true);






    }

    protected void lnkPayslip_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_PaySlip_Report.aspx')", true);



    }



    protected void lnkSettlement_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Settlement_payment.aspx')", true);



    }

    protected void lnkNonAdjustment_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_DuePayement.aspx')", true);




    }
    protected void lnkDocExpiry_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmpDirectoryReport.aspx?DocString=1')", true);



    }

    protected void lnkDirectory_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmpDirectoryReport.aspx?DocString=2')", true);





    }
    protected void lnkPF_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_PF_Report.aspx')", true);

    }

    protected void linkLeaveRemaninig_OnClick(object sender, EventArgs e)
    {
        DateTime FromDate = new DateTime(Convert.ToInt16(TxtYear.Text), Convert.ToInt16(ddlMonth.SelectedValue), 1);
        Session["FromDate"] = FromDate.ToString();
        Session["ToDate"] = FromDate.AddDays(27).ToString();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/LeaveRemainingReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkStatus_OnClick(object sender, EventArgs e)
    {
        DateTime FromDate = new DateTime(Convert.ToInt16(TxtYear.Text), Convert.ToInt16(ddlMonth.SelectedValue), 1);
        Session["FromDate"] = FromDate.ToString();
        Session["ToDate"] = FromDate.AddDays(27).ToString();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/LeaveStatusReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }
    protected void linkLeaveReport_OnClick(object sender, EventArgs e)
    {
        DateTime FromDate = new DateTime(Convert.ToInt16(TxtYear.Text), Convert.ToInt16(ddlMonth.SelectedValue), 1);
        Session["FromDate"] = FromDate.ToString();
        Session["ToDate"] = FromDate.AddDays(27).ToString();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/LeaveReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
    }

    protected void lnkESIC_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_ESIC_Report.aspx')", true);
    }

    protected void lnkTotalSalary_OnClick(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_TotalSalary_Report.aspx')", true);



    }
    protected void LnkPaySummaryBeforepost_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/PayrollSummayReport.aspx?Value=1')", true);




    }

    protected void lnkPayrollSummary_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/PayrollSummayReport.aspx?Value=2')", true);




    }
    protected void lnkSummarySalaryStatement_OnClick(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../HR_Report/FinalSalaryStatement.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    protected void chkterminatedemployee_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }


    }

    protected void Lnk_Pay_Slip_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_Pay_Slip.aspx')", true);
    }

    protected void Lnk_Projected_Salary_Transfer_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Projected_Salary_Transfer_Report.aspx')", true);
    }
}