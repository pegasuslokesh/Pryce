using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using PegasusDataAccess;
using System.Collections;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Globalization;
using DevExpress.Web;

public partial class HR_EditPayroll : BasePage
{
    #region defind Class Object

    Pay_Employee_Due_Payment objEmpDuePay = null;
    Pay_Employee_Loan objloan = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Pay_Employee_Month objPayEmpMonth = null;
    Pay_Employee_Deduction objpayrolldeduc = null;
    Pay_Employe_Allowance objpayrollAllowance = null;
    Pay_Employee_Penalty objPEpenalty = null;
    Pay_Employee_claim objPEClaim = null;
    Common ObjComman = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    Set_Allowance ObjAllow = null;
    Set_Deduction ObjDeduc = null;
    CountryMaster ObjSysCountryMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocMaster = null;
    Pay_Employee_Attendance objPayEmpAtt = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    HR_EmployeeDetail objEmpDetail = null;
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
    Pay_SalaryPlanDetail objsalaryplandetail = null;
    Att_Employee_Leave objEmpleave = null;
    LeaveMaster objLeaveType = null;
    Att_Employee_Notification objEmpNotice = null;
    Set_ApplicationParameter objAppParam = null;
    CurrencyMaster objCurrency = null;
    CompanyMaster objComp = null;
    EmployeeParameter objEmpParam = null;
    Set_Allowance objAllowance = null;
    Set_Deduction objDeduction = null;
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    DataAccessClass ObjDa = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_AdvancePayment objAdvancePayment = null;
    Pay_EmployeeArrear ObjEmpArrear = null;
    Pay_Employe_Allowance objpayrollall = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_DocNumber objDocNo = null;
    LogProcess ObjLogProcess = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "19";
        Session["HeaderText"] = "HR";

        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        objloan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objpayrolldeduc = new Pay_Employee_Deduction(Session["DBConnection"].ToString());
        objpayrollAllowance = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objPEpenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objPEClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        objPayEmpAtt = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objEmpDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
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
        objsalaryplandetail = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objLeaveType = new LeaveMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objDeduction = new Set_Deduction(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objAdvancePayment = new Pay_AdvancePayment(Session["DBConnection"].ToString());
        ObjEmpArrear = new Pay_EmployeeArrear(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/EditPayroll.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }

            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtPayroll"] = null;

            FillddlDeaprtment();
            FillddlLocation();

            Session["empimgpath"] = null;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Payroll_Enable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            Page.Title = objSys.GetSysTitle();
            ddlLocation.Focus();
            rbtnEmp.Checked = true;
            EmpGroup_CheckedChanged(null, null);
            Session["CHECKED_Edit_ITEMS"] = null;
        }


        if (Session["EmpIdc"] != null)
        {
            foreach (GridViewRow gvr in gvEmpEditPayroll.Rows)
            {
                Label lblEmpId = (Label)gvr.FindControl("lblEmpId");
                Label lblNetAmount = (Label)gvr.FindControl("lblGrossSalary");

                if (Session["EmpIdc"].ToString() == lblEmpId.Text)
                {
                    lblFinalAmount.Text = lblNetAmount.Text;
                }
            }
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnpostpayroll.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    private void FillddlLocation()
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


    public void FillddlDeaprtment()
    {
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationId(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();
        if (rbtnGroup.Checked == true)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }

    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked == true)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }


    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
        {
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
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }

            gvEmpEditPayroll.DataSource = null;
            gvEmpEditPayroll.DataBind();
            Session["dtEmpPayroll"] = null;
            lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " :0";

            lblSelectRecd.Text = "";
            ddlField1.SelectedIndex = 1;
            ddlOption1.SelectedIndex = 2;
            txtValue1.Text = "";

            lbxGroup_SelectedIndexChanged(null, null);
            lbxGroup.Focus();

            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else if (rbtnEmp.Checked)
        {
            Panel4.Visible = true;
            Div_Group.Visible = false;
            Div_Employee.Visible = true;

            FillGrid();
            ddlLocation.Focus();

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
            DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());

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
            dtEmp.Columns.Add("Gross_Salary");
            dtEmp.Columns.Add("Attendence_Salary");
            dtEmp.Columns.Add("BasicSalary");

            for (int i = 0; i < dtEmp.Rows.Count; i++)
            {

                DataTable dtemppara = new DataTable();
                dtemppara = objempparam.GetEmployeeParameterByEmpId(dtEmp.Rows[i]["Emp_Id"].ToString(), Session["CompId"].ToString());
                if (dtemppara.Rows.Count > 0)
                {
                    double BasicSalary = 0;
                    double EmployeePf = 0;
                    double EmployeeEsic = 0;

                    try
                    {
                        BasicSalary = double.Parse(dtemppara.Rows[0]["Basic_Salary"].ToString());
                    }
                    catch
                    {

                    }


                    dtEmp.Rows[i]["BasicSalary"] = dtemppara.Rows[0]["Basic_Salary"].ToString();
                    dtEmp.Rows[i]["BasicSalary"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["BasicSalary"].ToString(), Session["LocCurrencyId"].ToString());




                }


                if (dtEmp.Rows[i]["Payroll_Emp_Id"].ToString() != "" && dtEmp.Rows[i]["Payroll_Emp_Id"].ToString() != "0")
                {



                    //dtEmp.Rows[i]["Gross_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Payroll_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Employee_Penalty"].ToString()) + float.Parse(dtEmp.Rows[i]["Employee_Claim"].ToString()) - float.Parse(dtEmp.Rows[i]["Emlployee_Loan"].ToString()) + float.Parse(dtEmp.Rows[i]["Total_Allowance"].ToString()) - float.Parse(dtEmp.Rows[i]["Total_Deduction"].ToString()) + float.Parse(dtEmp.Rows[i]["Previous_Month_Balance"].ToString())  - double.Parse(dtEmp.Rows[i]["Employee_PF"].ToString())-double.Parse(dtEmp.Rows[i]["Employee_ESIC"].ToString())).ToString();
                    dtEmp.Rows[i]["Gross_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Leave_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Employee_Penalty"].ToString()) + float.Parse(dtEmp.Rows[i]["Employee_Claim"].ToString()) - float.Parse(dtEmp.Rows[i]["Emlployee_Loan"].ToString()) + float.Parse(dtEmp.Rows[i]["Total_Allowance"].ToString()) - float.Parse(dtEmp.Rows[i]["Total_Deduction"].ToString()) - double.Parse(dtEmp.Rows[i]["Employee_PF"].ToString()) - double.Parse(dtEmp.Rows[i]["Employee_ESIC"].ToString()) + double.Parse(dtEmp.Rows[i]["Previous_Month_Balance"].ToString())).ToString();

                    dtEmp.Rows[i]["Gross_Salary"] = objSys.GetCurencySymbol(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), Session["LocCurrencyId"].ToString(), Session["CompId"].ToString()) + Common.Get_Roundoff_Amount_By_Location(objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Gross_Salary"].ToString(), Session["LocCurrencyId"].ToString()), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                    dtEmp.Rows[i]["Attendence_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Leave_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString())).ToString();
                    dtEmp.Rows[i]["Attendence_Salary"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Attendence_Salary"].ToString(), Session["LocCurrencyId"].ToString());
                    dtEmp.Rows[i]["Total_Allowance"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Total_Allowance"].ToString(), Session["LocCurrencyId"].ToString());

                    dtEmp.Rows[i]["Employee_Claim"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_Claim"].ToString(), Session["LocCurrencyId"].ToString());
                    dtEmp.Rows[i]["Total_Deduction"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Total_Deduction"].ToString(), Session["LocCurrencyId"].ToString());
                    dtEmp.Rows[i]["Employee_Penalty"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_Penalty"].ToString(), Session["LocCurrencyId"].ToString());
                    dtEmp.Rows[i]["Emlployee_Loan"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Emlployee_Loan"].ToString(), Session["LocCurrencyId"].ToString());

                    dtEmp.Rows[i]["Employee_PF"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_PF"].ToString(), Session["LocCurrencyId"].ToString());


                    dtEmp.Rows[i]["Employee_ESIC"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_ESIC"].ToString(), Session["LocCurrencyId"].ToString());


                }
                else
                {
                    dtEmp.Rows[i]["Payroll_Emp_Id"] = "0";
                }
            }







            //here we showing only those employee in which payroll is generated
            //so we filter the record

            try
            {
                dtEmp = new DataView(dtEmp, "Payroll_Emp_Id<>0", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }


            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmpPayroll"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpEditPayroll, dtEmp, "", "");
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }
            else
            {
                gvEmpEditPayroll.DataSource = null;
                gvEmpEditPayroll.DataBind();
                Session["dtEmpPayroll"] = dtEmp;
                try
                {
                    object sumObject;
                    sumObject = dtEmp.Compute("Sum(Gross_Salary)", string.Empty);
                    lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + " (" + sumObject + ")";
                }
                catch
                {
                    lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
                }

                //
            }
            lblSelectRecd.Text = "";
            ddlField1.SelectedIndex = 1;
            ddlOption1.SelectedIndex = 2;
            txtValue1.Text = "";


        }



    }
    public void FillGrid()
    {

        DataTable dtEmp = new DataTable();
        dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());
        dtEmp.Columns.Add("Gross_Salary");
        dtEmp.Columns.Add("Attendence_Salary");
        dtEmp.Columns.Add("BasicSalary");

        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {

            DataTable dtemppara = new DataTable();
            dtemppara = objempparam.GetEmployeeParameterByEmpId(dtEmp.Rows[i]["Emp_Id"].ToString(), Session["CompId"].ToString());
            if (dtemppara.Rows.Count > 0)
            {
                double BasicSalary = 0;
                double EmployeePf = 0;
                double EmployeeEsic = 0;

                try
                {
                    BasicSalary = double.Parse(dtemppara.Rows[0]["Basic_Salary"].ToString());
                }
                catch
                {

                }
                dtEmp.Rows[i]["BasicSalary"] = dtemppara.Rows[0]["Basic_Salary"].ToString();
                dtEmp.Rows[i]["BasicSalary"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["BasicSalary"].ToString(), Session["LocCurrencyId"].ToString());
            }

            if (dtEmp.Rows[i]["Payroll_Emp_Id"].ToString() != "" && dtEmp.Rows[i]["Payroll_Emp_Id"].ToString() != "0")
            {
                //dtEmp.Rows[i]["Gross_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Payroll_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Employee_Penalty"].ToString()) + float.Parse(dtEmp.Rows[i]["Employee_Claim"].ToString()) - float.Parse(dtEmp.Rows[i]["Emlployee_Loan"].ToString()) + float.Parse(dtEmp.Rows[i]["Total_Allowance"].ToString()) - float.Parse(dtEmp.Rows[i]["Total_Deduction"].ToString()) + float.Parse(dtEmp.Rows[i]["Previous_Month_Balance"].ToString())  - double.Parse(dtEmp.Rows[i]["Employee_PF"].ToString())-double.Parse(dtEmp.Rows[i]["Employee_ESIC"].ToString())).ToString();
                dtEmp.Rows[i]["Gross_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Leave_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString()) + float.Parse(dtEmp.Rows[i]["Employee_Claim"].ToString()) - float.Parse(dtEmp.Rows[i]["Employee_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Emlployee_Loan"].ToString()) + float.Parse(dtEmp.Rows[i]["Total_Allowance"].ToString()) - float.Parse(dtEmp.Rows[i]["Total_Deduction"].ToString()) - double.Parse(dtEmp.Rows[i]["Employee_PF"].ToString()) - double.Parse(dtEmp.Rows[i]["Employee_ESIC"].ToString()) + double.Parse(dtEmp.Rows[i]["Previous_Month_Balance"].ToString())).ToString();

                dtEmp.Rows[i]["Gross_Salary"] = objSys.GetCurencySymbol(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), Session["LocCurrencyId"].ToString(), Session["CompId"].ToString()) + Common.Get_Roundoff_Amount_By_Location(objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Gross_Salary"].ToString(), Session["LocCurrencyId"].ToString()), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                dtEmp.Rows[i]["Attendence_Salary"] = (float.Parse(dtEmp.Rows[i]["Worked_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Leave_Days_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Week_Off_Salary"].ToString()) + float.Parse(dtEmp.Rows[i]["Holidays_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Absent_Salary"].ToString()) - float.Parse(dtEmp.Rows[i]["Late_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Early_Min_Penalty"].ToString()) - float.Parse(dtEmp.Rows[i]["Patial_Violation_Penalty"].ToString())).ToString();
                dtEmp.Rows[i]["Attendence_Salary"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Attendence_Salary"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Total_Allowance"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Total_Allowance"].ToString(), Session["LocCurrencyId"].ToString());

                dtEmp.Rows[i]["Employee_Claim"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_Claim"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Total_Deduction"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Total_Deduction"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Employee_Penalty"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_Penalty"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Emlployee_Loan"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Payroll_Emp_Id"].ToString(), dtEmp.Rows[i]["Emlployee_Loan"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Employee_PF"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_PF"].ToString(), Session["LocCurrencyId"].ToString());
                dtEmp.Rows[i]["Employee_ESIC"] = objSys.GetCurencyConversion(dtEmp.Rows[i]["Emp_Id"].ToString(), dtEmp.Rows[i]["Employee_ESIC"].ToString(), Session["LocCurrencyId"].ToString());
            }
            else
            {
                dtEmp.Rows[i]["Payroll_Emp_Id"] = "0";
            }
        }

        if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
        {
            lblSelectRecd.Text = "";
            ddlField1.SelectedIndex = 1;
            ddlOption1.SelectedIndex = 2;
            txtValue1.Text = "";
        }

        //here we showing only those employee in which payroll is generated
        //so we filter the record

        try
        {
            dtEmp = new DataView(dtEmp, "Payroll_Emp_Id<>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpPayroll"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpEditPayroll, dtEmp, "", "");

            try
            {
                decimal result = 0;
                for (int c = 0; c < dtEmp.Rows.Count; c++)
                {
                    result = result + Convert.ToDecimal(dtEmp.Rows[c]["Gross_Salary"].ToString());
                }




                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + " (" + result.ToString() + ")";
            }
            catch (Exception ex)
            {
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }

            // lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        else
        {
            gvEmpEditPayroll.DataSource = null;
            gvEmpEditPayroll.DataBind();
            Session["dtEmpPayroll"] = dtEmp;
            try
            {
                object sumObject;
                sumObject = dtEmp.Compute("Sum(Gross_Salary)", string.Empty);
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + " (" + sumObject + ")";
            }
            catch
            {
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }

            //lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_Edit_ITEMS"] = null;
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        FillddlDeaprtment();
        ddlLocation.SelectedIndex = 0;
        if (rbtnGroup.Checked == true)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }
        Session["dtPayroll"] = null;

        ddlLocation.Focus();

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
    protected void gvEmpEditPayroll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmpEditPayroll.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpPayroll"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpEditPayroll, dtEmp, "", "");
        PopulateCheckedValues();
        Session["dtPayroll"] = null;


        gvEmpEditPayroll.HeaderRow.Focus();

    }
    protected void gvEmpEditPayroll_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        hdFSortgvEmpPayroll.Value = hdFSortgvEmpPayroll.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpPayroll"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdFSortgvEmpPayroll.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpEditPayroll, dt, "", "");
        PopulateCheckedValues();

        gvEmpEditPayroll.HeaderRow.Focus();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["dtEmpPayroll"] != null)
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
                DataTable dtEmp = (DataTable)Session["dtEmpPayroll"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                DataTable dtTotalRecord = view.ToTable();
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpEditPayroll, view.ToTable(), "", "");
                Session["dtEmpPayroll"] = view.ToTable();
                try
                {
                    object sumObject;
                    sumObject = view.ToTable().Compute("Sum(Gross_Salary)", string.Empty);
                    lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtTotalRecord.Rows.Count.ToString() + " (" + sumObject + ")";
                }
                catch
                {
                    lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtTotalRecord.Rows.Count.ToString();

                }

            }
        }
        Session["dtPayroll"] = null;
        Session["CHECKED_Edit_ITEMS"] = null;

        txtValue1.Focus();
    }
    protected void imgBtnDetail_Command(object sender, CommandEventArgs e)
    {
        string strEmpId = e.CommandArgument.ToString();
        DataTable dt = ObjDa.return_DataTable("Select * From Pay_Employe_Month_Temp Where  Emp_Id = '" + strEmpId + "'");

        Session["FromDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), 1).ToString("dd-MMM-yyyy");

        Session["ToDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), DateTime.DaysInMonth(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()))).ToString("dd-MMM-yyyy");
        Session["EmpList"] = strEmpId + ",";
        Session["DepName"] = "";
        Session["LocationName"] = ddlLocation.SelectedItem.ToString();

        string strCmd = string.Format("window.open('../Attendance_Report/DailySalaryReport.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void btnEmpEdit_Command(object sender, CommandEventArgs e)
    {

        Session["EmpIdc"] = e.CommandArgument.ToString();

        DataTable dtempe = new DataTable();
        dtempe = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        if (dtempe.Rows.Count > 0 && dtempe.Rows.Count != null)
        {
            lblEmpCodeOT.Text = GetEmployeeCode(e.CommandArgument.ToString());
            lblEmpNameOT.Text = GetEmployeeName(e.CommandArgument.ToString());
            lblMonthName.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(dtempe.Rows[0]["Month"].ToString()));
            lblYearName.Text = dtempe.Rows[0]["Year"].ToString();


            Panel4.Visible = false;
            Div_Head.Style.Add("display", "none");

            Div_Head.Style.Add("display", "none");
            Div_Group.Visible = false;
            Div_Employee.Visible = false;

            Div_Emp_Details.Style.Add("display", "");
            if (RbtnListView.Checked == true)
            {
                Div_Payroll.Style.Add("display", "none");
                //Div_penaltyclaim.Style.Add("display", "none");
                //Div_gvallowance.Style.Add("display", "");
                //Div_gvdeduuc.Style.Add("display", "none");
                //Div_Head.Style.Add("display", "none");
                Emp_Details.Style.Add("display", "");
                Div_Payroll.Style.Add("display", "");
                Div_gvallowance.Style.Add("display", "");
                Div_gvdeduuc.Style.Add("display", "");
                Div_penaltyclaim.Style.Add("display", "");
                Div_loan.Style.Add("display", "");
                Div_attnd.Style.Add("display", "");

                btnAllowance_Click(null, null);
                btndeduction_Click(null, null);
                btnclaimpenalty_Click(null, null);
                btnloan_Click(null, null);
                btnattendance_Click(null, null);
            }
            else
            {
                Div_Payroll.Style.Add("display", "");
                Emp_Details.Style.Add("display", "");
                btnAllowance1_Click(null, null);
            }


            if (Session["EmpIdc"] != null)
            {
                foreach (GridViewRow gvr in gvEmpEditPayroll.Rows)
                {
                    Label lblEmpId = (Label)gvr.FindControl("lblEmpId");
                    Label lblNetAmount = (Label)gvr.FindControl("lblGrossSalary");

                    if (Session["EmpIdc"].ToString() == lblEmpId.Text)
                    {
                        lblFinalAmount.Text = lblNetAmount.Text;
                    }
                }
            }
            //btndeduction_Click(null, null);
            //btnclaimpenalty_Click(null, null);
            //btnloan_Click(null, null);
            //btnattendance_Click(null, null);
        }
        else
        {
            DisplayMessage("Payroll Not Generated");
            GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent;
            gvEmpEditPayroll.Rows[row.RowIndex].Cells[0].Focus();

        }
        Total_Final_Paid_Amount();
    }
    protected void imgBtnEmpDelete_Command(object sender, CommandEventArgs e)
    {

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_Edit_ITEMS"] = null;
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        if (rbtnGroup.Checked == true)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();
        }


        Session["dtPayroll"] = null;

        txtValue1.Focus();
    }
    protected void btnLogReport_Click(object sender, EventArgs e)
    {

        DataTable dt = ObjDa.return_DataTable("Select * From Pay_Employe_Month_Temp Where  Emp_Id = '" + Session["EmpIdc"].ToString() + "'");

        Session["FromDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), 1).ToString("dd-MMM-yyyy");

        Session["ToDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), DateTime.DaysInMonth(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()))).ToString("dd-MMM-yyyy");
        Session["EmpList"] = Session["EmpIdc"].ToString() + ",";
        Session["DepName"] = "";
        Session["LocationName"] = ddlLocation.SelectedItem.ToString();

        string strCmd = string.Format("window.open('../Attendance_Report/LogReport.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void btnSalaryReport_Click(object sender, EventArgs e)
    {

        DataTable dt = ObjDa.return_DataTable("Select * From Pay_Employe_Month_Temp Where  Emp_Id = '" + Session["EmpIdc"].ToString() + "'");

        Session["FromDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), 1).ToString("dd-MMM-yyyy");

        Session["ToDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), DateTime.DaysInMonth(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()))).ToString("dd-MMM-yyyy");
        Session["EmpList"] = Session["EmpIdc"].ToString() + ",";
        Session["DepName"] = "";
        Session["LocationName"] = ddlLocation.SelectedItem.ToString();

        string strCmd = string.Format("window.open('../Attendance_Report/DailySalaryReport.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void btnInOutReport_Click(object sender, EventArgs e)
    {

        DataTable dt = ObjDa.return_DataTable("Select * From Pay_Employe_Month_Temp Where  Emp_Id = '" + Session["EmpIdc"].ToString() + "'");

        Session["FromDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), 1).ToString("dd-MMM-yyyy");

        Session["ToDate"] = new DateTime(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()), DateTime.DaysInMonth(Convert.ToInt16(dt.Rows[0]["Year"].ToString()), Convert.ToInt16(dt.Rows[0]["Month"].ToString()))).ToString("dd-MMM-yyyy");
        Session["EmpList"] = Session["EmpIdc"].ToString() + ",";
        Session["DepName"] = "";
        Session["LocationName"] = ddlLocation.SelectedItem.ToString();

        string strCmd = string.Format("window.open('../Attendance_Report/InOutReport.aspx?_Id=1','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }

    protected void lnkbtnback_Click(object sender, EventArgs e)
    {

        Div_Head.Style.Add("display", "");




        Panel4.Visible = true;
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Div_loan.Style.Add("display", "none");
        Div_attnd.Style.Add("display", "none");
        Div_Emp_Details.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");

        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }
        try
        {

            gvEmpEditPayroll.Rows[0].Cells[0].Focus();
        }
        catch
        {
        }
        FillGrid();
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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

    #region Allowance
    #region ListView
    protected void btnAllowance_Click(object sender, EventArgs e)
    {

        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //Div_gvdeduuc.Style.Add("display", "none");
        //Div_attnd.Style.Add("display", "none");
        //Div_penaltyclaim.Style.Add("display", "none");
        //Div_loan.Style.Add("display", "none");
        DataTable dtBindGrid = new DataTable();
        dtBindGrid = objpayrollAllowance.GetPayAllowPayaRoll(Session["EmpIdc"].ToString());
        try
        {
            Session["monthAllow"] = dtBindGrid.Rows[0]["Month"].ToString();
            Session["YearAllow"] = dtBindGrid.Rows[0]["Year"].ToString();
        }
        catch
        {

        }
        if (dtBindGrid.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvallowance, dtBindGrid, "", "");

            foreach (GridViewRow gvr2 in gvallowance.Rows)
            {
                HiddenField hdnallowanceId = (HiddenField)gvr2.FindControl("hdnallowId");

                HiddenField hdnRefId = (HiddenField)gvr2.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvr2.FindControl("lblType");
                Label lblAllowValue = (Label)gvr2.FindControl("lblAllowValue");
                CheckBox chkallowance = (CheckBox)gvr2.FindControl("chkallowance");
                TextBox txtValue = (TextBox)gvr2.FindControl("txtValue");

                if (hdnallowanceId.Value != "0" && hdnallowanceId.Value != "")
                {

                    DataTable dtAllowance = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), hdnallowanceId.Value);

                    dtAllowance = new DataView(dtAllowance, "Allowance_Id=" + hdnallowanceId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtAllowance.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtAllowance.Rows[0]["Allowance"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month Allowance total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] AllowaceId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdnallowanceId.Value.ToString() == AllowaceId[1].ToString())
                                {

                                    chkallowance.Checked = true;
                                    break;


                                }
                            }
                            catch
                            {
                                chkallowance.Checked = false;
                            }
                        }

                    }

                    //code end






                }
            }
            Total_Allowances();
            //here end of foreach loop

        }
        else
        {
            Lbl_Total_Allowances.Text = "0.00";

            gvallowance.DataSource = null;
            gvallowance.DataBind();

            Div_gvallowance.Style.Add("display", "none");
        }


    }
    protected void btnsaveallow_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totalamount = 0;
        double txtamtal = 0;
        double lblamtal = 0;
        int type = 0;
        int trnsid;
        string id;
        string allowancename;
        string actvalue;
        string updatevale;
        double TotalAllowace = 0;
        foreach (GridViewRow gvr1 in gvallowance.Rows)
        {
            Label lblamt = (Label)gvr1.FindControl("lblAllowValue");
            HiddenField Empid = (HiddenField)gvr1.FindControl("hdnEmpId");
            HiddenField hdnTId = (HiddenField)gvr1.FindControl("hdntransId");
            TextBox txtvalue2 = (TextBox)gvr1.FindControl("txtValue");
            HiddenField allwId = (HiddenField)gvr1.FindControl("hdnallowId");
            CheckBox chkallow = (CheckBox)gvr1.FindControl("chkallowance");
            Label lblname = (Label)gvr1.FindControl("lblType");

            if (txtvalue2.Text == "")
            {
                txtvalue2.Text = "0";
            }

            if (float.Parse(txtvalue2.Text) > float.Parse(lblamt.Text))
            {
                DisplayMessage("Actual Allowance Value Cannot be Greater Than Allowance Value");
                txtvalue2.Text = lblamt.Text;
                txtvalue2.Focus();
                return;
            }



            lblamtal = Convert.ToDouble(lblamt.Text);
            txtamtal = Convert.ToDouble(txtvalue2.Text);
            try
            {
                TotalAllowace += double.Parse(txtvalue2.Text);
            }
            catch
            {

            }

            id = "Allowance Id=" + allwId.Value;
            allowancename = "Allowance Name=" + lblname.Text;
            actvalue = "Actual Value=" + txtvalue2.Text;

            c = objpayrollAllowance.UpdatePayEMpAllowActVale(hdnTId.Value, Empid.Value, txtvalue2.Text);


            if (txtamtal != lblamtal)
            {


                totalamount = lblamtal - txtamtal;
                type = 1;


                updatevale = "Updated Value=" + totalamount.ToString();
                trnsid = Convert.ToInt32(hdnTId.Value);


                if (((CheckBox)gvr1.FindControl("chkallowance")).Checked)
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }


                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id.ToString(), "1");
                    }

                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), type.ToString(), totalamount.ToString(), DateTime.Now.ToString(), id.ToString() + "," + allowancename + "," + actvalue + "," + updatevale, "True", "Allowance", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id, "1");
                    }
                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), type.ToString(), totalamount.ToString(), DateTime.Now.ToString(), id.ToString() + "," + allowancename + "," + actvalue + "," + updatevale, "False", "Allowance", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }


            }
            else if (txtamtal == lblamtal)
            {
                DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                try
                {
                    dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (dt.Rows.Count > 0)
                {
                    objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id.ToString(), "1");
                }
            }
        }


        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), TotalAllowace.ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }







        if (c != 0)
        {
            //btnAllowance_Click(null, null);
            //DisplayMessage("Record Updated", "green");

        }

    }
    protected void btncencelallow_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd.Style.Add("display", "none");
        Panel4.Visible = true;

    }
    protected void gvallowance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvallowance.PageIndex = e.NewPageIndex;
        DataTable dtBindGrid = new DataTable();
        dtBindGrid = objpayrollAllowance.GetPayAllowPayaRoll(Session["EmpIdc"].ToString());
        if (dtBindGrid.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvallowance, dtBindGrid, "", "");

            foreach (GridViewRow gvr2 in gvallowance.Rows)
            {
                HiddenField hdnallowanceId = (HiddenField)gvr2.FindControl("hdnallowId");

                HiddenField hdnRefId = (HiddenField)gvr2.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvr2.FindControl("lblType");
                Label lblAllowValue = (Label)gvr2.FindControl("lblAllowValue");
                CheckBox chkallowance = (CheckBox)gvr2.FindControl("chkallowance");
                TextBox txtValue = (TextBox)gvr2.FindControl("txtValue");

                if (hdnallowanceId.Value != "0" && hdnallowanceId.Value != "")
                {

                    DataTable dtAllowance = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), hdnallowanceId.Value);

                    dtAllowance = new DataView(dtAllowance, "Allowance_Id=" + hdnallowanceId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtAllowance.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtAllowance.Rows[0]["Allowance"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month Allowance total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] AllowaceId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdnallowanceId.Value.ToString() == AllowaceId[1].ToString())
                                {

                                    chkallowance.Checked = true;
                                    break;


                                }
                            }
                            catch
                            {
                                chkallowance.Checked = false;
                            }
                        }

                    }






                }
            }

        }
    }
    #endregion

    #region FormView
    protected void btnAllowance1_Click(object sender, EventArgs e)
    {

        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");



        DataTable dtBindGrid = new DataTable();
        dtBindGrid = objpayrollAllowance.GetPayAllowPayaRoll(Session["EmpIdc"].ToString());
        try
        {

            Session["monthAllow"] = dtBindGrid.Rows[0]["Month"].ToString();

            Session["YearAllow"] = dtBindGrid.Rows[0]["Year"].ToString();

        }
        catch
        {

        }
        try
        {
            dtBindGrid = new DataView(dtBindGrid, "Month='" + Session["monthAllow"].ToString() + "' and Year='" + Session["YearAllow"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (dtBindGrid.Rows.Count > 0)
        {


            Div_gvallowance1.Style.Add("display", "");
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvallowance1, dtBindGrid, "", "");

            foreach (GridViewRow gvr2 in gvallowance1.Rows)
            {
                HiddenField hdnallowanceId = (HiddenField)gvr2.FindControl("hdnallowId");

                HiddenField hdnRefId = (HiddenField)gvr2.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvr2.FindControl("lblType");
                Label lblAllowValue = (Label)gvr2.FindControl("lblAllowValue");
                CheckBox chkallowance = (CheckBox)gvr2.FindControl("chkallowance");
                TextBox txtValue = (TextBox)gvr2.FindControl("txtValue");

                if (hdnallowanceId.Value != "0" && hdnallowanceId.Value != "")
                {

                    DataTable dtAllowance = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), hdnallowanceId.Value);

                    dtAllowance = new DataView(dtAllowance, "Allowance_Id=" + hdnallowanceId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtAllowance.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtAllowance.Rows[0]["Allowance"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month Allowance total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] AllowaceId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdnallowanceId.Value.ToString() == AllowaceId[1].ToString())
                                {

                                    chkallowance.Checked = true;
                                    break;


                                }
                            }
                            catch
                            {
                                chkallowance.Checked = false;
                            }
                        }

                    }




                    //code end




                    //int Lastmonth = 0;
                    //int LastYear = 0;
                    //dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString());
                    //if (Session["monthAllow"].ToString() == "1")
                    //{
                    //    LastYear = Convert.ToInt32(Session["YearAllow"].ToString()) - 1;
                    //    Lastmonth = 12;   
                    //}
                    //else
                    //{
                    //    Lastmonth = Convert.ToInt32(Session["monthAllow"].ToString()) - 1;
                    //    LastYear = Convert.ToInt32(Session["YearAllow"].ToString());
                    //}

                    //try
                    //{
                    //    dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    //}
                    //catch
                    //{
                    //}
                    //if (dtDueamount.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < dtDueamount.Rows.Count; i++)
                    //    {
                    //        string[] AllowaceId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                    //        if (hdnallowanceId.Value.ToString() == AllowaceId[1].ToString())
                    //        {
                    //            lblAllowValue.Text = (float.Parse(lblAllowValue.Text) + float.Parse(dtDueamount.Rows[i]["Amount"].ToString())).ToString();
                    //            break;
                    //        }
                    //    }

                    //}

                    //code end


                }
            }


        }
        else
        {
            gvallowance1.DataSource = null;
            gvallowance1.DataBind();
            Div_gvallowance1.Style.Add("display", "none");
        }
    }
    protected void btnsaveallow1_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totalamount = 0;
        double txtamtal = 0;
        double lblamtal = 0;
        int type = 0;
        int trnsid;
        string id;
        string allowancename;
        string actvalue;
        string updatevale;
        double TotalAllowace = 0;
        foreach (GridViewRow gvr1 in gvallowance1.Rows)
        {
            Label lblamt = (Label)gvr1.FindControl("lblAllowValue");
            HiddenField Empid = (HiddenField)gvr1.FindControl("hdnEmpId");
            HiddenField hdnTId = (HiddenField)gvr1.FindControl("hdntransId");
            TextBox txtvalue2 = (TextBox)gvr1.FindControl("txtValue");
            HiddenField allwId = (HiddenField)gvr1.FindControl("hdnallowId");
            CheckBox chkallow = (CheckBox)gvr1.FindControl("chkallowance");
            Label lblname = (Label)gvr1.FindControl("lblType");

            if (txtvalue2.Text == "")
            {
                txtvalue2.Text = "0";
            }

            try
            {
                TotalAllowace += double.Parse(txtvalue2.Text);
            }
            catch
            {

            }


            id = "Allowance Id=" + allwId.Value;
            allowancename = "Allowance Name=" + lblname.Text;
            actvalue = "Actual Value=" + txtvalue2.Text;




            lblamtal = Convert.ToDouble(lblamt.Text);
            txtamtal = Convert.ToDouble(txtvalue2.Text);

            c = objpayrollAllowance.UpdatePayEMpAllowActVale(hdnTId.Value, Empid.Value, txtvalue2.Text);


            if (txtamtal != lblamtal)
            {

                if (txtamtal > lblamtal)
                {
                    DisplayMessage("Actual Allowance Value Cannot be Greater Than Allowance Value");
                    txtvalue2.Focus();
                    return;


                }
                else
                {
                    totalamount = lblamtal - txtamtal;
                    type = 1;

                }
                updatevale = "Updated Value=" + totalamount.ToString();
                trnsid = Convert.ToInt32(hdnTId.Value);


                if (((CheckBox)gvr1.FindControl("chkallowance")).Checked)
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }


                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id.ToString(), "1");
                    }

                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), type.ToString(), totalamount.ToString(), DateTime.Now.ToString(), id.ToString() + "," + allowancename + "," + actvalue + "," + updatevale, "True", "Allowance", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id, "1");
                    }
                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), type.ToString(), totalamount.ToString(), DateTime.Now.ToString(), id.ToString() + "," + allowancename + "," + actvalue + "," + updatevale, "False", "Allowance", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }


            }
            else if (txtamtal == lblamtal)
            {
                DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                try
                {
                    dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (dt.Rows.Count > 0)
                {
                    objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Allowance", id.ToString(), "1");
                }
            }
        }


        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), TotalAllowace.ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }







        if (c != 0)
        {
            btnAllowance_Click(null, null);
            DisplayMessage("Record Updated", "green");

        }

    }
    protected void btncencelallow1_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_Head.Style.Add("display", "");


        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }

    }
    protected void gvallowance1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvallowance1.PageIndex = e.NewPageIndex;
        DataTable dtBindGrid = new DataTable();
        dtBindGrid = objpayrollAllowance.GetPayAllowPayaRoll(Session["EmpIdc"].ToString());
        if (dtBindGrid.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvallowance1, dtBindGrid, "", "");

            foreach (GridViewRow gvr2 in gvallowance1.Rows)
            {
                HiddenField hdnallowanceId = (HiddenField)gvr2.FindControl("hdnallowId");

                HiddenField hdnRefId = (HiddenField)gvr2.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvr2.FindControl("lblType");
                Label lblAllowValue = (Label)gvr2.FindControl("lblAllowValue");
                CheckBox chkallowance = (CheckBox)gvr2.FindControl("chkallowance");
                TextBox txtValue = (TextBox)gvr2.FindControl("txtValue");

                if (hdnallowanceId.Value != "0" && hdnallowanceId.Value != "")
                {

                    DataTable dtAllowance = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), hdnallowanceId.Value);

                    dtAllowance = new DataView(dtAllowance, "Allowance_Id=" + hdnallowanceId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtAllowance.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtAllowance.Rows[0]["Allowance"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month Allowance total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] AllowaceId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdnallowanceId.Value.ToString() == AllowaceId[1].ToString())
                                {

                                    chkallowance.Checked = true;
                                    break;


                                }
                            }
                            catch
                            {
                                chkallowance.Checked = false;
                            }
                        }

                    }






                }
            }

        }
    }
    #endregion

    #endregion
    #region Deduction
    #region ListView
    protected void btndeduction_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //Div_gvallowance.Style.Add("display", "none");
        //Div_penaltyclaim.Style.Add("display", "none");
        //Div_loan.Style.Add("display", "none");
        //Div_attnd.Style.Add("display", "none");
        DataTable dtBindGrid1 = new DataTable();
        dtBindGrid1 = objpayrolldeduc.GetPayDeducPayaRoll(Session["EmpIdc"].ToString());


        if (dtBindGrid1.Rows.Count > 0)
        {
            Session["monthdeduc"] = dtBindGrid1.Rows[0]["Month"].ToString();
            Session["yeardeuc"] = dtBindGrid1.Rows[0]["Year"].ToString();

            Div_gvdeduuc.Style.Add("display", "");
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvdeduction, dtBindGrid1, "", "");

            Session["deducData"] = dtBindGrid1;
            foreach (GridViewRow gvrd in gvdeduction.Rows)
            {
                HiddenField hdndeductionId = (HiddenField)gvrd.FindControl("hdnDeducId");
                HiddenField hdnRefIdded = (HiddenField)gvrd.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvrd.FindControl("lblTypededuc");
                Label lblDeductValue = (Label)gvrd.FindControl("lblDeductValue");
                CheckBox chkdeduc = (CheckBox)gvrd.FindControl("chkdeduc");

                if (hdndeductionId.Value != "0" && hdndeductionId.Value != "")
                {

                    DataTable dtDeduction = objDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), hdndeductionId.Value);

                    dtDeduction = new DataView(dtDeduction, "Deduction_Id=" + hdndeductionId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDeduction.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtDeduction.Rows[0]["Deduction"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month deduction total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthdeduc"].ToString() + " and Year=" + Session["yeardeuc"].ToString() + " and Field1='Deduction'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] DeductionId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdndeductionId.Value.ToString() == DeductionId[1].ToString())
                                {

                                    chkdeduc.Checked = true;


                                }
                            }
                            catch
                            {
                                chkdeduc.Checked = false;
                            }
                        }

                    }


                }
            }
            Total_Deductions();

        }
        else
        {

            // DisplayMessage("NO Payroll Generated");

            // No Payroll generated
            Lbl_Total_Deductions.Text = "0.00";
            gvdeduction.DataSource = null;
            gvdeduction.DataBind();
            Div_gvdeduuc.Style.Add("display", "none");

        }

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totaldeduc = 0;
        double txtamtdeduc = 0;
        double lblamtdeduc = 0;
        int typed = 0;
        string id;
        string Deducname;
        string actvalue;
        string updatevale;

        int d = 0;
        double TotalDeduc = 0;
        foreach (GridViewRow gvrduc in gvdeduction.Rows)
        {

            Label lblamtded = (Label)gvrduc.FindControl("lblDeductValue");
            // HiddenField Empid = (HiddenField)gvrduc.FindControl("");

            HiddenField hdntransIdded = (HiddenField)gvrduc.FindControl("hdntransIddeduc");
            HiddenField hdnDeductionId = (HiddenField)gvrduc.FindControl("hdnDeducId");

            TextBox txtvalueDeduc = (TextBox)gvrduc.FindControl("txtDeducValue");
            Label lblname = (Label)gvrduc.FindControl("lblTypededuc");
            if (txtvalueDeduc.Text == "")
            {
                txtvalueDeduc.Text = "0";
            }

            if (float.Parse(txtvalueDeduc.Text) > float.Parse(lblamtded.Text))
            {
                DisplayMessage("Actual Deduction Value Cannot be Greater Than Deduction Value");
                txtvalueDeduc.Text = lblamtded.Text;
                txtvalueDeduc.Focus();
                return;

            }
            lblamtdeduc = Convert.ToDouble(lblamtded.Text);
            txtamtdeduc = Convert.ToDouble(txtvalueDeduc.Text);
            id = "Deduction Id=" + hdnDeductionId.Value;
            Deducname = "Deduction Name=" + lblname.Text;
            actvalue = "Actual Value=" + txtvalueDeduc.Text;

            try
            {
                TotalDeduc += double.Parse(txtvalueDeduc.Text);
            }
            catch
            {
            }

            d = objpayrolldeduc.UpdateRecordPayEmpDeduction(hdntransIdded.Value, Session["EmpIdc"].ToString(), txtvalueDeduc.Text);



            if (txtamtdeduc != lblamtdeduc)
            {


                totaldeduc = lblamtdeduc - txtamtdeduc;


                typed = 2;
                updatevale = " Updated Value= " + totaldeduc.ToString();

                if (((CheckBox)gvrduc.FindControl("chkdeduc")).Checked)
                {
                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                    }
                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthdeduc"].ToString(), Session["yeardeuc"].ToString(), typed.ToString(), totaldeduc.ToString(), DateTime.Now.ToString(), id.ToString() + "," + Deducname + "," + actvalue + "," + updatevale, "True", "Deduction", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                    }

                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthdeduc"].ToString(), Session["yeardeuc"].ToString(), typed.ToString(), totaldeduc.ToString(), DateTime.Now.ToString(), id.ToString() + "," + Deducname + "," + actvalue + "," + updatevale, "False", "Deduction", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

            }
            else if (txtamtdeduc == lblamtdeduc)
            {
                DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                try
                {
                    dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                }

            }


        }

        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), TotalDeduc.ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }
        if (d != 0)
        {
            //DisplayMessage("Record Updated", "green");

        }

    }

    protected void btncencelDeduc_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_attnd.Style.Add("display", "none");
        Div_Head.Style.Add("display", "");
    }
    protected void gvdeduction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdeduction.PageIndex = e.NewPageIndex;
        DataTable dtBindGrid1 = new DataTable();
        dtBindGrid1 = objpayrolldeduc.GetPayDeducPayaRoll(Session["EmpIdc"].ToString());

        if (dtBindGrid1.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvdeduction, dtBindGrid1, "", "");

            foreach (GridViewRow gvrd in gvdeduction.Rows)
            {
                HiddenField hdndeductionId = (HiddenField)gvrd.FindControl("hdnDeducId");
                HiddenField hdnRefIdded = (HiddenField)gvrd.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvrd.FindControl("lblTypededuc");
                Label lblDeductValue = (Label)gvrd.FindControl("lblDeductValue");
                CheckBox chkdeduc = (CheckBox)gvrd.FindControl("chkdeduc");

                if (hdndeductionId.Value != "0" && hdndeductionId.Value != "")
                {

                    DataTable dtDeduction = objDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), hdndeductionId.Value);

                    dtDeduction = new DataView(dtDeduction, "Deduction_Id=" + hdndeductionId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDeduction.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtDeduction.Rows[0]["Deduction"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month deduction total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthdeduc"].ToString() + " and Year=" + Session["yeardeuc"].ToString() + " and Field1='Deduction'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] DeductionId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdndeductionId.Value.ToString() == DeductionId[1].ToString())
                                {

                                    chkdeduc.Checked = true;


                                }
                            }
                            catch
                            {
                                chkdeduc.Checked = false;
                            }
                        }

                    }


                }
            }
        }
    }
    #endregion

    #region FormView
    protected void btndeduction1_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        Div_gvallowance1.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        DataTable dtBindGrid1 = new DataTable();
        dtBindGrid1 = objpayrolldeduc.GetPayDeducPayaRoll(Session["EmpIdc"].ToString());
        try
        {
            dtBindGrid1 = new DataView(dtBindGrid1, "Month='" + dtBindGrid1.Rows[0]["Month"].ToString() + "' and Year='" + dtBindGrid1.Rows[0]["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


        if (dtBindGrid1.Rows.Count > 0)
        {
            Session["monthdeduc"] = dtBindGrid1.Rows[0]["Month"].ToString();
            Session["yeardeuc"] = dtBindGrid1.Rows[0]["Year"].ToString();

            Div_gvdeduuc1.Style.Add("display", "");
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvdeduction1, dtBindGrid1, "", "");

            Session["deducData"] = dtBindGrid1;
            foreach (GridViewRow gvrd in gvdeduction1.Rows)
            {
                HiddenField hdndeductionId = (HiddenField)gvrd.FindControl("hdnDeducId");
                HiddenField hdnRefIdded = (HiddenField)gvrd.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvrd.FindControl("lblTypededuc");
                Label lblDeductValue = (Label)gvrd.FindControl("lblDeductValue");
                CheckBox chkdeduc = (CheckBox)gvrd.FindControl("chkdeduc");

                if (hdndeductionId.Value != "0" && hdndeductionId.Value != "")
                {

                    DataTable dtDeduction = objDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), hdndeductionId.Value);

                    dtDeduction = new DataView(dtDeduction, "Deduction_Id=" + hdndeductionId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDeduction.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtDeduction.Rows[0]["Deduction"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month deduction total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthdeduc"].ToString() + " and Year=" + Session["yeardeuc"].ToString() + " and Field1='Deduction'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] DeductionId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdndeductionId.Value.ToString() == DeductionId[1].ToString())
                                {

                                    chkdeduc.Checked = true;


                                }
                            }
                            catch
                            {
                                chkdeduc.Checked = false;
                            }
                        }

                    }


                }
            }


        }
        else
        {


            gvdeduction1.DataSource = null;
            gvdeduction1.DataBind();
            Div_gvdeduuc1.Style.Add("display", "none");

        }

    }
    protected void BtnSave1_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totaldeduc = 0;
        double txtamtdeduc = 0;
        double lblamtdeduc = 0;
        int typed = 0;
        string id;
        string Deducname;
        string actvalue;
        string updatevale;

        int d = 0;
        double TotalDeduc = 0;
        foreach (GridViewRow gvrduc in gvdeduction1.Rows)
        {

            Label lblamtded = (Label)gvrduc.FindControl("lblDeductValue");
            // HiddenField Empid = (HiddenField)gvrduc.FindControl("");

            HiddenField hdntransIdded = (HiddenField)gvrduc.FindControl("hdntransIddeduc");
            HiddenField hdnDeductionId = (HiddenField)gvrduc.FindControl("hdnDeducId");

            TextBox txtvalueDeduc = (TextBox)gvrduc.FindControl("txtDeducValue");
            Label lblname = (Label)gvrduc.FindControl("lblTypededuc");
            if (txtvalueDeduc.Text == "")
            {
                txtvalueDeduc.Text = "0";
            }

            id = "Deduction Id=" + hdnDeductionId.Value;
            Deducname = "Deduction Name=" + lblname.Text;
            actvalue = "Actual Value=" + txtvalueDeduc.Text;

            try
            {
                TotalDeduc += double.Parse(txtvalueDeduc.Text);
            }
            catch
            {
            }

            d = objpayrolldeduc.UpdateRecordPayEmpDeduction(hdntransIdded.Value, Session["EmpIdc"].ToString(), txtvalueDeduc.Text);

            lblamtdeduc = Convert.ToDouble(lblamtded.Text);
            txtamtdeduc = Convert.ToDouble(txtvalueDeduc.Text);

            if (txtamtdeduc != lblamtdeduc)
            {

                if (txtamtdeduc > lblamtdeduc)
                {
                    DisplayMessage("Actual Deduction Value Cannot be Greater Than Deduction Value");
                    txtvalueDeduc.Focus();
                    return;

                }
                else
                {
                    totaldeduc = lblamtdeduc - txtamtdeduc;

                }
                typed = 2;
                updatevale = " Updated Value= " + totaldeduc.ToString();

                if (((CheckBox)gvrduc.FindControl("chkdeduc")).Checked)
                {
                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                    }
                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthdeduc"].ToString(), Session["yeardeuc"].ToString(), typed.ToString(), totaldeduc.ToString(), DateTime.Now.ToString(), id.ToString() + "," + Deducname + "," + actvalue + "," + updatevale, "True", "Deduction", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {

                    DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    catch
                    {
                    }

                    if (dt.Rows.Count > 0)
                    {
                        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                    }

                    int p = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthdeduc"].ToString(), Session["yeardeuc"].ToString(), typed.ToString(), totaldeduc.ToString(), DateTime.Now.ToString(), id.ToString() + "," + Deducname + "," + actvalue + "," + updatevale, "False", "Deduction", id.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

            }
            else if (txtamtdeduc == lblamtdeduc)
            {
                DataTable dt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                try
                {
                    dt = new DataView(dt, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Deduction' and Field2='" + id.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), "Deduction", id, "1");
                }

            }


        }

        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), TotalDeduc.ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }
        if (d != 0)
        {
            DisplayMessage("Record Updated", "green");

        }

    }
    protected void btncencelDeduc1_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_attnd1.Style.Add("display", "none");
        Div_Head.Style.Add("display", "");
        Div_Head.Style.Add("display", "");

        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }


    }
    protected void gvdeduction1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdeduction1.PageIndex = e.NewPageIndex;
        DataTable dtBindGrid1 = new DataTable();
        dtBindGrid1 = objpayrolldeduc.GetPayDeducPayaRoll(Session["EmpIdc"].ToString());

        if (dtBindGrid1.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvdeduction1, dtBindGrid1, "", "");

            foreach (GridViewRow gvrd in gvdeduction1.Rows)
            {
                HiddenField hdndeductionId = (HiddenField)gvrd.FindControl("hdnDeducId");
                HiddenField hdnRefIdded = (HiddenField)gvrd.FindControl("hdnRefId");
                Label lblgvRefValue = (Label)gvrd.FindControl("lblTypededuc");
                Label lblDeductValue = (Label)gvrd.FindControl("lblDeductValue");
                CheckBox chkdeduc = (CheckBox)gvrd.FindControl("chkdeduc");

                if (hdndeductionId.Value != "0" && hdndeductionId.Value != "")
                {

                    DataTable dtDeduction = objDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), hdndeductionId.Value);

                    dtDeduction = new DataView(dtDeduction, "Deduction_Id=" + hdndeductionId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDeduction.Rows.Count > 0)
                    {
                        lblgvRefValue.Text = dtDeduction.Rows[0]["Deduction"].ToString();
                    }
                    else
                    {
                        lblgvRefValue.Text = "";
                    }
                    //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
                    //this code for showing the last month due amount and current month deduction total
                    //code start

                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Session["monthdeduc"].ToString() + " and Year=" + Session["yeardeuc"].ToString() + " and Field1='Deduction'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDueamount.Rows.Count; i++)
                        {
                            try
                            {
                                string[] DeductionId = dtDueamount.Rows[i]["Field2"].ToString().Split('=');
                                if (hdndeductionId.Value.ToString() == DeductionId[1].ToString())
                                {

                                    chkdeduc.Checked = true;


                                }
                            }
                            catch
                            {
                                chkdeduc.Checked = false;
                            }
                        }

                    }


                }
            }
        }
    }
    #endregion
    #endregion
    #region Claim_Penalty
    #region ListView
    protected void btnclaimpenalty_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //Div_gvallowance.Style.Add("display", "none");
        //Div_gvdeduuc.Style.Add("display", "none");
        //Div_loan.Style.Add("display", "none");
        //Div_attnd.Style.Add("display", "none");
        DataTable dtPenalty = new DataTable();
        DataTable dtClaim = new DataTable();
        DataTable dtclaimpenaltry = new DataTable();
        float TotalPenalty = 0;
        float TotalActualPenalty = 0;
        float TotalClaim = 0;
        float TotalActualClaim = 0;
        dtclaimpenaltry = objPayEmpMonth.GetPenaltyClaim(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        dtclaimpenaltry = new DataView(dtclaimpenaltry, "Month='" + dtclaimpenaltry.Rows[0]["Month"].ToString() + "' and Year='" + dtclaimpenaltry.Rows[0]["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtclaimpenaltry.Rows.Count > 0)
        {
            Session["monthclaim1"] = dtclaimpenaltry.Rows[0]["Month"].ToString();
            Session["yearclaim1"] = dtclaimpenaltry.Rows[0]["Year"].ToString();

        }
        dtPenalty = objPEpenalty.GetRecord_From_PayEmployeePenalty_By_EmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), "", dtclaimpenaltry.Rows[0]["Month"].ToString(), dtclaimpenaltry.Rows[0]["Year"].ToString(), "", "");
        if (dtPenalty.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvPenalty, dtPenalty, "", "");

            foreach (GridViewRow gvr in gvPenalty.Rows)
            {
                TextBox txtPenalty = (TextBox)(gvr.FindControl("txtPenaltyValue"));
                try
                {
                    TotalPenalty += float.Parse(txtPenalty.Text);
                }
                catch
                {

                }
                Label lblTotalPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));
                try
                {
                    TotalActualPenalty += float.Parse(lblTotalPenalty.Text);
                }
                catch
                {

                }



            }
        }
        else
        {
            gvPenalty.DataSource = null;
            gvPenalty.DataBind();

        }


        dtClaim = objPEClaim.GetRecord_From_PayEmployeeClaim_By_EmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), "", "Approved", dtclaimpenaltry.Rows[0]["Month"].ToString(), dtclaimpenaltry.Rows[0]["Year"].ToString(), "", "");
        if (dtClaim.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvClaim, dtClaim, "", "");

            foreach (GridViewRow gvr in GvClaim.Rows)
            {


                TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));
                if (txtClaim.Text != "")
                {
                    TotalClaim += float.Parse(txtClaim.Text);
                }
                Label lblTotalClaim = (Label)(gvr.FindControl("lblClaimValue1"));
                if (lblTotalClaim.Text != "")
                {
                    TotalActualClaim += float.Parse(lblTotalClaim.Text);
                }
            }
        }
        else
        {
            GvClaim.DataSource = null;
            GvClaim.DataBind();

        }
        //this code is created on 31-03-2014 by jitendra upadhyay with divya mam to showing the last month adjustment amount
        //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
        //this code for showing the last month due amount and current month Allowance total
        //code start
        double amtPenalty = 0;
        double amtClaim = 0;
        DataTable dtPenaltyAmt = new DataTable();
        dtPenaltyAmt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtPenaltyAmt = new DataView(dtPenaltyAmt, "Month=" + dtclaimpenaltry.Rows[0]["Month"].ToString() + " and Year=" + dtclaimpenaltry.Rows[0]["Year"].ToString() + " and Field1='Penalty'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtPenaltyAmt.Rows.Count > 0)
        {

            chkpenalty.Checked = true;

        }
        DataTable dtClaimamt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtClaimamt = new DataView(dtClaimamt, "Month=" + dtclaimpenaltry.Rows[0]["Month"].ToString() + " and Year=" + dtclaimpenaltry.Rows[0]["Year"].ToString() + " and Field1='Claim'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtClaimamt.Rows.Count > 0)
        {

            chkclaim.Checked = true;

        }





        int Lastmonth = 0;
        int LastYear = 0;


        if (dtclaimpenaltry.Rows[0]["Month"].ToString() == "1")
        {
            LastYear = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Year"].ToString()) - 1;
            Lastmonth = 12;
        }
        else
        {
            Lastmonth = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Month"].ToString()) - 1;
            LastYear = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Year"].ToString());
        }
        DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Penalty'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtDueamount.Rows.Count > 0)
        {
            amtPenalty = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
        }

        dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Claim'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtDueamount.Rows.Count > 0)
        {
            amtClaim = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
        }

        txtdueamt.Text = (amtClaim - amtPenalty).ToString();
        //code end


        txttotalpeanlty.Text = TotalPenalty.ToString();
        txttotalclaim.Text = TotalClaim.ToString();
        lbltotalpenalty.Text = TotalActualPenalty.ToString();
        lbltotalclaim.Text = TotalActualClaim.ToString();
    }
    protected void btnsaveclaimpenalty_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totalclaim = 0;
        double txtamtclaim = 0;
        double lblamtclaim = 0;
        double txtpenalty = 0;
        double lblpenalty = 0;
        double totalpenalty = 0;
        int typed = 0;
        int p = 0;

        string actvaluepenalty;
        string actvalueclaim;

        string updatevaleclaim;
        string updatevalepenalty;
        if (txttotalpeanlty.Text == "")
        {
            txttotalpeanlty.Text = "0";
        }
        if (txttotalclaim.Text == "")
        {
            txttotalclaim.Text = "0";
        }


        p = objPayEmpMonth.UpdateRecord_Pay_Employee_Penalty_claim(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), txttotalpeanlty.Text, txttotalclaim.Text);

        lblamtclaim = Convert.ToDouble(lbltotalclaim.Text);
        txtamtclaim = Convert.ToDouble(txttotalclaim.Text);
        txtpenalty = Convert.ToDouble(txttotalpeanlty.Text);
        lblpenalty = Convert.ToDouble(lbltotalpenalty.Text);


        actvalueclaim = "Actual Value Claim=" + lbltotalclaim.Text;

        actvaluepenalty = "Actual Value Penalty=" + lbltotalpenalty.Text;


        totalclaim = lblamtclaim - txtamtclaim;


        typed = 1;

        updatevaleclaim = " Updated Value= " + txtamtclaim.ToString();


        DataTable dtClaim = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtClaim = new DataView(dtClaim, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Claim' ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        //if (dtClaim.Rows.Count > 0)
        //{
        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), "Claim", "", "1");

        //}


        if (totalclaim > 0)
        {
            if (chkclaim.Checked)
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalclaim.ToString(), DateTime.Now.ToString(), actvalueclaim + "," + updatevaleclaim, "True", "Claim", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalclaim.ToString(), DateTime.Now.ToString(), actvalueclaim + "," + updatevaleclaim, "False", "Claim", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }


        foreach (GridViewRow gvr in GvClaim.Rows)
        {
            Label lblClaimId = (Label)(gvr.FindControl("lblClaimId1"));
            Label lblClaim = (Label)(gvr.FindControl("lblClaimValue1"));

            TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));
            //modified by jitendra upadhyay and divya mam on 05-04-2014

            //code start

            if (txtClaim.Text == "")
            {
                txtClaim.Text = "0";
            }
            //code end
            objPEClaim.UpdateRecord_In_Pay_Employee_ClaimAmount(Session["CompId"].ToString(), lblClaimId.Text, Session["EmpIdc"].ToString(), lblClaim.Text, txtClaim.Text, Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        totalpenalty = lblpenalty - txtpenalty;
        typed = 2;



        updatevalepenalty = " Updated Value= " + txtpenalty.ToString();

        DataTable dtPenalty = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtPenalty = new DataView(dtPenalty, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Penalty' ", "",

DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        //if (dtPenalty.Rows.Count > 0)
        //{
        objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), "Penalty", "", "1");

        //}
        if (totalpenalty > 0)
        {
            if (chkpenalty.Checked)
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalpenalty.ToString(), DateTime.Now.ToString(), actvaluepenalty + "," + updatevalepenalty, "True", "Penalty", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalpenalty.ToString(), DateTime.Now.ToString(), actvaluepenalty + "," + updatevalepenalty, "False", "Penalty", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }
        foreach (GridViewRow gvr in gvPenalty.Rows)
        {
            Label lblPenaltyId = (Label)(gvr.FindControl("lblPenaltyId1"));
            Label lblPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));

            TextBox txtPenalty = (TextBox)gvr.FindControl("txtPenaltyValue");
            //modified by jitendra upadhyay and divya mam on 05-04-2014

            //code start
            if (txtPenalty.Text == "")
            {
                txtPenalty.Text = "0";
            }
            //code end

            objPEpenalty.UpdateRecord_In_Pay_Employee_PenaltyAmount(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), lblPenaltyId.Text, lblPenalty.Text, txtPenalty.Text, Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }



        DataTable dtPay = new DataTable();
        try
        {



            dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["Yearclaim1"].ToString(), Session["CompId"].ToString());
        }
        catch (Exception ex)
        {

        }
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["Yearclaim1"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), txttotalpeanlty.Text, txttotalclaim.Text, dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }



        if (p != 0)
        {
            //DisplayMessage("Record Updated", "green");
            //btnclaimpenalty_Click(null, null);

        }

    }
    protected void btncencelpenaltyclaim_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_attnd.Style.Add("display", "none");

    }
    protected void txtPenaltyValue_OnTextChanged(object sender, EventArgs e)
    {
        float TotalPenalty = 0;
        foreach (GridViewRow gvr in gvPenalty.Rows)
        {
            Label lblPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));

            TextBox txtPenalty = (TextBox)(gvr.FindControl("txtPenaltyValue"));

            if (txtPenalty.Text == "")
            {
                DisplayMessage("Enter Penalty Value");
                txtPenalty.Text = "0";
                txtPenalty.Focus();
                return;

            }
            else if (float.Parse(txtPenalty.Text) > float.Parse(lblPenalty.Text))
            {
                DisplayMessage("Penalty Value Cannot be Greater Than Actual Value");
                txtPenalty.Text = "0";
                txtPenalty.Focus();
                return;

            }
            TotalPenalty += float.Parse(txtPenalty.Text);

        }

        txttotalpeanlty.Text = TotalPenalty.ToString();
    }
    protected void txtClaimValue_OnTextChanged(object sender, EventArgs e)
    {
        float TotalClaim = 0;
        foreach (GridViewRow gvr in GvClaim.Rows)
        {

            Label lblClaim = (Label)(gvr.FindControl("lblClaimValue1"));
            TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));

            if (txtClaim.Text == "")
            {
                DisplayMessage("Enter Claim Value");
                txtClaim.Text = "0";
                txtClaim.Focus();
                return;

            }
            else if (float.Parse(txtClaim.Text) > float.Parse(lblClaim.Text))
            {
                DisplayMessage("Claim Value Cannot be Greater Than Actual Value");
                txtClaim.Text = "0";
                txtClaim.Focus();
                return;

            }
            TotalClaim += float.Parse(txtClaim.Text);

        }
        txttotalclaim.Text = TotalClaim.ToString();
    }
    #endregion

    #region FormView
    protected void btnclaimpenalty1_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        Div_penaltyclaim1.Style.Add("display", "");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        DataTable dtPenalty = new DataTable();
        DataTable dtClaim = new DataTable();
        DataTable dtclaimpenaltry = new DataTable();
        float TotalPenalty = 0;
        float TotalActualPenalty = 0;
        float TotalClaim = 0;
        float TotalActualClaim = 0;
        dtclaimpenaltry = objPayEmpMonth.GetPenaltyClaim(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        dtclaimpenaltry = new DataView(dtclaimpenaltry, "Month='" + dtclaimpenaltry.Rows[0]["Month"].ToString() + "' and Year='" + dtclaimpenaltry.Rows[0]["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtclaimpenaltry.Rows.Count > 0)
        {
            Session["monthclaim1"] = dtclaimpenaltry.Rows[0]["Month"].ToString();
            Session["yearclaim1"] = dtclaimpenaltry.Rows[0]["Year"].ToString();

        }
        dtPenalty = objPEpenalty.GetRecord_From_PayEmployeePenalty_By_EmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), "", dtclaimpenaltry.Rows[0]["Month"].ToString(), dtclaimpenaltry.Rows[0]["Year"].ToString(), "", "");
        if (dtPenalty.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvPenalty1, dtPenalty, "", "");

            foreach (GridViewRow gvr in gvPenalty1.Rows)
            {
                TextBox txtPenalty = (TextBox)(gvr.FindControl("txtPenaltyValue"));
                try
                {
                    TotalPenalty += float.Parse(txtPenalty.Text);
                }
                catch
                {

                }
                Label lblTotalPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));
                try
                {
                    TotalActualPenalty += float.Parse(lblTotalPenalty.Text);
                }
                catch
                {

                }



            }
        }
        else
        {
            gvPenalty1.DataSource = null;
            gvPenalty1.DataBind();

        }


        dtClaim = objPEClaim.GetRecord_From_PayEmployeeClaim_By_EmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), "", "Approved", dtclaimpenaltry.Rows[0]["Month"].ToString(), dtclaimpenaltry.Rows[0]["Year"].ToString(), "", "");
        if (dtClaim.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvClaim1, dtClaim, "", "");

            foreach (GridViewRow gvr in GvClaim1.Rows)
            {


                TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));
                if (txtClaim.Text != "")
                {
                    TotalClaim += float.Parse(txtClaim.Text);
                }
                Label lblTotalClaim = (Label)(gvr.FindControl("lblClaimValue1"));
                if (lblTotalClaim.Text != "")
                {
                    TotalActualClaim += float.Parse(lblTotalClaim.Text);
                }
            }
        }
        else
        {
            GvClaim1.DataSource = null;
            GvClaim1.DataBind();

        }
        //this code is created on 31-03-2014 by jitendra upadhyay with divya mam to showing the last month adjustment amount
        //this code is created on 31-03-2014 by jitendra upadhyay and divya mam
        //this code for showing the last month due amount and current month Allowance total
        //code start
        double amtPenalty = 0;
        double amtClaim = 0;
        DataTable dtPenaltyAmt = new DataTable();
        dtPenaltyAmt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtPenaltyAmt = new DataView(dtPenaltyAmt, "Month=" + dtclaimpenaltry.Rows[0]["Month"].ToString() + " and Year=" + dtclaimpenaltry.Rows[0]["Year"].ToString() + " and Field1='Penalty'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtPenaltyAmt.Rows.Count > 0)
        {

            chkpenalty1.Checked = true;

        }
        DataTable dtClaimamt = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtClaimamt = new DataView(dtClaimamt, "Month=" + dtclaimpenaltry.Rows[0]["Month"].ToString() + " and Year=" + dtclaimpenaltry.Rows[0]["Year"].ToString() + " and Field1='Claim'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtClaimamt.Rows.Count > 0)
        {

            chkclaim1.Checked = true;

        }





        int Lastmonth = 0;
        int LastYear = 0;


        if (dtclaimpenaltry.Rows[0]["Month"].ToString() == "1")
        {
            LastYear = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Year"].ToString()) - 1;
            Lastmonth = 12;
        }
        else
        {
            Lastmonth = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Month"].ToString()) - 1;
            LastYear = Convert.ToInt32(dtclaimpenaltry.Rows[0]["Year"].ToString());
        }
        DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Penalty'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtDueamount.Rows.Count > 0)
        {
            amtPenalty = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
        }

        dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Claim'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtDueamount.Rows.Count > 0)
        {
            amtClaim = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
        }

        txtdueamt1.Text = (amtClaim - amtPenalty).ToString();
        //code end


        txttotalpeanlty1.Text = TotalPenalty.ToString();
        txttotalclaim1.Text = TotalClaim.ToString();
        lbltotalpenalty1.Text = TotalActualPenalty.ToString();
        lbltotalclaim1.Text = TotalActualClaim.ToString();
    }
    protected void btnsaveclaimpenalty1_Click(object sender, EventArgs e)
    {
        int c = 0;
        double totalclaim = 0;
        double txtamtclaim = 0;
        double lblamtclaim = 0;
        double txtpenalty = 0;
        double lblpenalty = 0;
        double totalpenalty = 0;
        int typed = 0;
        int p = 0;

        string actvaluepenalty;
        string actvalueclaim;

        string updatevaleclaim;
        string updatevalepenalty;
        if (txttotalpeanlty1.Text == "")
        {
            txttotalpeanlty1.Text = "0";
        }
        if (txttotalclaim1.Text == "")
        {
            txttotalclaim1.Text = "0";
        }


        p = objPayEmpMonth.UpdateRecord_Pay_Employee_Penalty_claim(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), txttotalpeanlty1.Text, txttotalclaim1.Text);

        lblamtclaim = Convert.ToDouble(lbltotalclaim1.Text);
        txtamtclaim = Convert.ToDouble(txttotalclaim1.Text);
        txtpenalty = Convert.ToDouble(txttotalpeanlty1.Text);
        lblpenalty = Convert.ToDouble(lbltotalpenalty1.Text);


        actvalueclaim = "Actual Value Claim=" + txttotalclaim1.Text;

        actvaluepenalty = "Actual Value Penalty=" + txttotalpeanlty1.Text;


        totalclaim = lblamtclaim - txtamtclaim;


        typed = 1;

        updatevaleclaim = " Updated Value= " + totalclaim.ToString();


        DataTable dtClaim = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtClaim = new DataView(dtClaim, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Claim' ", "",

DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        if (dtClaim.Rows.Count > 0)
        {
            objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), "Claim", "", "1");

        }


        if (totalclaim > 0)
        {
            if (chkclaim1.Checked)
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalclaim.ToString(), DateTime.Now.ToString(), actvalueclaim + "," + updatevaleclaim, "True", "Claim", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalclaim.ToString(), DateTime.Now.ToString(), actvalueclaim + "," + updatevaleclaim, "False", "Claim", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }


        foreach (GridViewRow gvr in GvClaim1.Rows)
        {
            Label lblClaimId = (Label)(gvr.FindControl("lblClaimId1"));
            Label lblClaim = (Label)(gvr.FindControl("lblClaimValue1"));

            TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));
            //modified by jitendra upadhyay and divya mam on 05-04-2014

            //code start

            if (txtClaim.Text == "")
            {
                txtClaim.Text = "0";
            }
            //code end
            objPEClaim.UpdateRecord_In_Pay_Employee_ClaimAmount(Session["CompId"].ToString(), lblClaimId.Text, Session["EmpIdc"].ToString(), lblClaim.Text, txtClaim.Text, Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        totalpenalty = lblpenalty - txtpenalty;
        typed = 2;



        updatevalepenalty = " Updated Value= " + totalpenalty.ToString();

        DataTable dtPenalty = objEmpDuePay.GetAllRecord_ByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        try
        {
            dtPenalty = new DataView(dtPenalty, "Month=" + Session["monthAllow"].ToString() + " and Year=" + Session["YearAllow"].ToString() + " and Field1='Penalty' ", "",

DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        if (dtPenalty.Rows.Count > 0)
        {
            objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), "Penalty", "", "1");

        }
        if (totalpenalty > 0)
        {
            if (chkpenalty1.Checked)
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalpenalty.ToString(), DateTime.Now.ToString(), actvaluepenalty + "," + updatevalepenalty, "True", "Penalty", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                c = objEmpDuePay.Insert_In_Employee_Due_Payment(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), typed.ToString(), totalpenalty.ToString(), DateTime.Now.ToString(), actvaluepenalty + "," + updatevalepenalty, "False", "Penalty", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }
        foreach (GridViewRow gvr in gvPenalty1.Rows)
        {
            Label lblPenaltyId = (Label)(gvr.FindControl("lblPenaltyId1"));
            Label lblPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));

            TextBox txtPenalty = (TextBox)gvr.FindControl("txtPenaltyValue");
            //modified by jitendra upadhyay and divya mam on 05-04-2014

            //code start
            if (txtPenalty.Text == "")
            {
                txtPenalty.Text = "0";
            }
            //code end

            objPEpenalty.UpdateRecord_In_Pay_Employee_PenaltyAmount(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), lblPenaltyId.Text, lblPenalty.Text, txtPenalty.Text, Session["monthclaim1"].ToString(), Session["yearclaim1"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }



        DataTable dtPay = new DataTable();
        try
        {



            dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["Yearclaim1"].ToString(), Session["CompId"].ToString());
        }
        catch (Exception ex)
        {

        }
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthclaim1"].ToString(), Session["Yearclaim1"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Emlployee_Loan"].ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), txttotalpeanlty1.Text, txttotalclaim1.Text, dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }



        if (p != 0)
        {
            DisplayMessage("Record Updated", "green");
            btnclaimpenalty_Click(null, null);

        }

    }
    protected void btncencelpenaltyclaim1_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_attnd1.Style.Add("display", "none");
        Div_Head.Style.Add("display", "");


        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }

    }
    protected void txtPenaltyValue1_OnTextChanged(object sender, EventArgs e)
    {
        float TotalPenalty = 0;
        foreach (GridViewRow gvr in gvPenalty1.Rows)
        {
            Label lblPenalty = (Label)(gvr.FindControl("lblPenaltyValue1"));

            TextBox txtPenalty = (TextBox)(gvr.FindControl("txtPenaltyValue"));

            if (txtPenalty.Text == "")
            {
                DisplayMessage("Enter Penalty Value");
                txtPenalty.Text = "0";
                txtPenalty.Focus();
                return;

            }
            else if (float.Parse(txtPenalty.Text) > float.Parse(lblPenalty.Text))
            {
                DisplayMessage("Penalty Value Cannot be Greater Than Actual Value");
                txtPenalty.Text = "0";
                txtPenalty.Focus();
                return;

            }
            TotalPenalty += float.Parse(txtPenalty.Text);

        }

        txttotalpeanlty1.Text = TotalPenalty.ToString();
    }
    protected void txtClaimValue1_OnTextChanged(object sender, EventArgs e)
    {
        float TotalClaim = 0;
        foreach (GridViewRow gvr in GvClaim1.Rows)
        {

            Label lblClaim = (Label)(gvr.FindControl("lblClaimValue1"));
            TextBox txtClaim = (TextBox)(gvr.FindControl("txtClaimValue"));

            if (txtClaim.Text == "")
            {
                DisplayMessage("Enter Claim Value");
                txtClaim.Text = "0";
                txtClaim.Focus();
                return;

            }
            else if (float.Parse(txtClaim.Text) > float.Parse(lblClaim.Text))
            {
                DisplayMessage("Claim value Cannot be Greater Than Actual Value");
                txtClaim.Text = "0";
                txtClaim.Focus();
                return;

            }
            TotalClaim += float.Parse(txtClaim.Text);

        }
        txttotalclaim1.Text = TotalClaim.ToString();
    }
    #endregion

    #endregion
    #region LOan
    #region Listview
    protected void btnloan_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //Div_penaltyclaim.Style.Add("display", "none");
        //Div_gvallowance.Style.Add("display", "none");
        //Div_gvdeduuc.Style.Add("display", "none");
        //Div_attnd.Style.Add("display", "none");

        DataTable dtempedit = new DataTable();
        dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        Session["monthAllow"] = dtempedit.Rows[0]["Month"].ToString();
        Session["YearAllow"] = dtempedit.Rows[0]["Year"].ToString();
        if (dtempedit.Rows.Count > 0)
        {
            DataTable Dtloan = new DataTable();
            Dtloan = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");

            Dtloan = new DataView(Dtloan, " Emp_Id=" + Session["EmpIdc"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (Dtloan.Rows.Count > 0)
            {
                DataTable dtloandetial = new DataTable();

                for (int i = 0; i < Dtloan.Rows.Count; i++)
                {
                    dtloandetial.Merge(objloan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString()));

                }

                if (dtloandetial.Rows.Count > 0)
                {
                    //dtloandetial = new DataView(dtloandetial, " Month=" + dtempedit.Rows[0]["Month"].ToString() + " and Year=" + dtempedit.Rows[0]["Year"].ToString() + " and total_amount>0 and Is_status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                    dtloandetial = new DataView(dtloandetial, " Month<=" + Session["monthAllow"].ToString() + " and Year=" + dtempedit.Rows[0]["Year"].ToString() + " and total_amount>0 and Is_status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                }


                if (dtloandetial.Rows.Count > 0)
                {
                    Div_loan.Style.Add("display", "");

                    dtloandetial.Columns.Add("Net_Pending_Amount");
                    foreach (DataRow DVR in dtloandetial.Rows)
                    {
                        // DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Previous_Balance"].ToString()) + Convert.ToDouble(DVR["Montly_Installment"].ToString())).ToString());

                        if (Convert.ToDouble(DVR["Employee_Paid"].ToString()) > 0)
                            DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Employee_Paid"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                        else
                            DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Previous_Balance"].ToString()) + Convert.ToDouble(DVR["Montly_Installment"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    }



                    //Common Function add By Lokesh on 23-05-2015
                    objPageCmn.FillData((object)gvloan, dtloandetial, "", "");

                    foreach (GridViewRow gvrl in gvloan.Rows)
                    {
                        HiddenField hdnId = (HiddenField)gvrl.FindControl("hdnloanId");
                        Label lblloanna = (Label)gvrl.FindControl("lblloanname");
                        Label lblAmt = (Label)gvrl.FindControl("lblloanamt");
                        TextBox txtloanamt = (TextBox)gvrl.FindControl("txtloanamt");
                        HiddenField HdnPaid = (HiddenField)gvrl.FindControl("hdnLoanAmount");
                        //if (HdnPaid.Value != "")
                        //{
                        //    lblAmt.Text = HdnPaid.Value;
                        //}
                        //if(txtloanamt.Text.Trim()!="")
                        //{
                        //    if (Convert.ToDouble(txtloanamt.Text.Trim()) == 0)
                        //    {
                        //        txtloanamt.Text =Common.GetAmountDecimal(lblAmt.Text);
                        //        HdnPaid.Value = Common.GetAmountDecimal(lblAmt.Text);
                        //        lblAmt.Text = Common.GetAmountDecimal(lblAmt.Text);
                        //    }
                        //}




                        if (hdnId.Value != "0" && hdnId.Value != "")
                        {

                            DataTable Dtloann = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                            Dtloann = new DataView(Dtloann, "Loan_Id=" + hdnId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();

                            if (Dtloann.Rows.Count > 0)
                            {
                                lblloanna.Text = Dtloann.Rows[0]["Loan_Name"].ToString();
                            }
                            else
                            {
                                lblloanna.Text = "";
                            }

                        }

                    }
                    Total_Loan_Amount();
                }


                else
                {
                    Lbl_Total_Loan_Amount.Text = "0.00";
                    gvloan.DataSource = null;
                    gvloan.DataBind();
                    Div_loan.Style.Add("display", "none");
                }
            }
            else
            {
                Lbl_Total_Loan_Amount.Text = "0.00";
                gvloan.DataSource = null;
                gvloan.DataBind();
                Div_loan.Style.Add("display", "none");
            }
        }
        else
        {
            // DisplayMessage("NO Payroll Generated");


        }
    }
    protected void btnsaveloan_Click(object sender, EventArgs e)
    {
        // Before Validation on Save
        // Validation  on Save Button too

        // Loop Acc to Grid View Rows
        // Check Acc to Loan Id and next Month Record 
        // Add in Month =1 if value is greater than 13 then we will set month = 1 and add in year one
        // Check Record Exists
        // Current Month Employee Paid Amount 
        // And Update in Next Month PB and Taotal Amount
        // Else
        // No Can edit installment
        double netamt = 0;

        double pvbal = 0;
        double instllamt = 0;
        double totalamt = 0;
        double txtamount = 0;
        double currentLoanAmt = 0;





        foreach (GridViewRow gvrloan in gvloan.Rows)
        {
            TextBox txtamt = (TextBox)gvrloan.FindControl("txtloanamt");
            HiddenField hdnlnId = (HiddenField)gvrloan.FindControl("hdnloanId");
            HiddenField hdntrnsid = (HiddenField)gvrloan.FindControl("hdntrnasLoanId");
            Label lblloanamt = (Label)gvrloan.FindControl("lblloanamt");
            Label lblActualLOan = (Label)gvrloan.FindControl("lblActualLOan");
            if (txtamt.Text == "")
            {
                txtamt.Text = "0";
            }
            DataTable dtempedit = new DataTable();
            dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
            int counter = 0;

            instllamt = Convert.ToDouble(lblloanamt.Text);

            txtamount = Convert.ToDouble(txtamt.Text);

            currentLoanAmt = Convert.ToDouble(lblActualLOan.Text);

            int trnsid = 0;

            trnsid = Convert.ToInt32(hdntrnsid.Value);
            if (currentLoanAmt != txtamount)
            {
                DataTable dtlndedetials = new DataTable();
                dtlndedetials = objloan.GetRecord_From_PayEmployeeLoanDetailAll();
                dtlndedetials = new DataView(dtlndedetials, "Loan_Id=" + hdnlnId.Value + " and Trans_Id > " + (trnsid) + " ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtlndedetials.Rows.Count > 0)
                {

                    if (currentLoanAmt > txtamount)
                    {
                        pvbal = currentLoanAmt - txtamount;


                        totalamt = instllamt + pvbal;
                        //objloan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), hdnlnId.Value.ToString(), (trnsid + 1).ToString(), pvbal.ToString(), totalamt.ToString(), "0");


                    }
                    else if (txtamount > currentLoanAmt)
                    {

                        //here we are adding validation that paid amount should not greater then loan installment
                        double TotalPendingamount = currentLoanAmt;

                        foreach (DataRow dr in dtlndedetials.Rows)
                        {
                            TotalPendingamount += Convert.ToDouble(dr["total_amount"].ToString());
                        }


                        if (txtamount > TotalPendingamount)
                        {
                            DisplayMessage("Paid amount should not be greater then loan due amount");
                            txtamt.Text = "0";
                            Session["LoanStatus"] = "1";
                            break;
                        }


                        double PreviousBalance = 0;
                        pvbal = txtamount - currentLoanAmt;
                        PreviousBalance = pvbal;
                        double currentloaninstallment = 0;
                        string strStatus = string.Empty;
                        foreach (DataRow dr in dtlndedetials.Rows)
                        {
                            if (pvbal <= 0)
                            {
                                break;
                            }

                            currentLoanAmt = Convert.ToDouble(dr["Montly_installment"].ToString());



                            if (PreviousBalance > currentLoanAmt)
                            {
                                currentloaninstallment = 0;
                                PreviousBalance = (currentLoanAmt);

                            }
                            else
                            {
                                currentloaninstallment = currentLoanAmt - PreviousBalance;
                            }


                            // objloan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), hdnlnId.Value.ToString(), dr["Trans_Id"].ToString(), (0 - PreviousBalance).ToString(), currentloaninstallment.ToString(), "0");



                            pvbal = pvbal - PreviousBalance;

                            if (currentloaninstallment <= 0 || currentloaninstallment <= 1)
                            {
                                // ObjDa.execute_Command("update pay_employee_loan_detail set is_status = 'Paid'  where Trans_id = " + dr["Trans_Id"].ToString() + "");

                            }


                            PreviousBalance = pvbal;
                        }

                    }

                }
                else
                {
                    counter = 1;

                }

            }

            string strstatus = string.Empty;

            if (counter == 1)
            {
                DisplayMessage("You Can Not Adjust The Amount");
                txtamt.Text = lblloanamt.Text;
                txtamt.Focus();
                return;


            }
            else
            {

                if (float.Parse(txtamt.Text) > 0)
                {
                    strstatus = "Paid";
                }
                else
                {
                    strstatus = "Pending";
                }




                // objloan.UpdateRecord_loandetials_WithPaidStatusandAmount(hdnlnId.Value.ToString(), trnsid.ToString(), txtamt.Text, strstatus);


            }



            // Modified by Divya Joshi on 4/4/2014
            if (txtamt.Text != "")
            {
                netamt += Convert.ToDouble(txtamt.Text);
            }
        }






        //this code is created by jitendra upadhyay an divya mam on 04-04-2014
        //for insert the current month paid loan installment in paid amount
        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), netamt.ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }
        //DisplayMessage("Record Updated", "green");


    }

    protected void btncenellaon_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_loan.Style.Add("display", "none");

        Div_attnd.Style.Add("display", "none");
        Panel4.Visible = true;


    }
    protected void gvloan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvloan.PageIndex = e.NewPageIndex;
        DataTable dtempedit = new DataTable();
        dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        Session["monthAllow"] = dtempedit.Rows[0]["Month"].ToString();
        Session["YearAllow"] = dtempedit.Rows[0]["Year"].ToString();
        if (dtempedit.Rows.Count > 0)
        {
            DataTable Dtloan = new DataTable();
            Dtloan = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");

            Dtloan = new DataView(Dtloan, " Emp_Id=" + Session["EmpIdc"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (Dtloan.Rows.Count > 0)
            {
                DataTable dtloandetial = new DataTable();

                for (int i = 0; i < Dtloan.Rows.Count; i++)
                {
                    dtloandetial.Merge(objloan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString()));

                }

                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial = new DataView(dtloandetial, " Month=" + dtempedit.Rows[0]["Month"].ToString() + " and Year=" + dtempedit.Rows[0]["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }


                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial.Columns.Add("Net_Pending_Amount");
                    foreach (DataRow DVR in dtloandetial.Rows)
                    {
                        DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Previous_Balance"].ToString()) + Convert.ToDouble(DVR["Montly_Installment"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    }
                    //Common Function add By Lokesh on 23-05-2015
                    objPageCmn.FillData((object)gvloan, dtloandetial, "", "");

                    foreach (GridViewRow gvrl in gvloan.Rows)
                    {
                        HiddenField hdnId = (HiddenField)gvrl.FindControl("hdnloanId");
                        Label lblloanna = (Label)gvrl.FindControl("lblloanname");
                        TextBox lblAmt = (TextBox)gvrl.FindControl("txtloanamt");

                        HiddenField HdnPaid = (HiddenField)gvrl.FindControl("hdnLoanAmount");

                        if (HdnPaid.Value != "")
                        {
                            lblAmt.Text = HdnPaid.Value;

                        }


                        if (hdnId.Value != "0" && hdnId.Value != "")
                        {

                            DataTable Dtloann = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                            Dtloann = new DataView(Dtloann, "Loan_Id=" + hdnId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();

                            if (Dtloann.Rows.Count > 0)
                            {
                                lblloanna.Text = Dtloann.Rows[0]["Loan_Name"].ToString();
                            }
                            else
                            {
                                lblloanna.Text = "";
                            }

                        }

                    }

                }


                else
                {
                    gvloan.DataSource = null;
                    gvloan.DataBind();

                }
            }
            else
            {
                gvloan.DataSource = null;
                gvloan.DataBind();

            }
        }
        else
        {
            // DisplayMessage("NO Payroll Generated");


        }
    }
    #endregion

    #region FormView
    protected void btnloan1_Click(object sender, EventArgs e)
    {
        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");

        Div_attnd1.Style.Add("display", "none");
        DataTable dtempedit = new DataTable();
        dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        Session["monthAllow"] = dtempedit.Rows[0]["Month"].ToString();
        Session["YearAllow"] = dtempedit.Rows[0]["Year"].ToString();
        if (dtempedit.Rows.Count > 0)
        {
            DataTable Dtloan = new DataTable();
            Dtloan = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");

            Dtloan = new DataView(Dtloan, " Emp_Id=" + Session["EmpIdc"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (Dtloan.Rows.Count > 0)
            {
                DataTable dtloandetial = new DataTable();

                for (int i = 0; i < Dtloan.Rows.Count; i++)
                {
                    dtloandetial.Merge(objloan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString()));

                }

                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial = new DataView(dtloandetial, " Month=" + dtempedit.Rows[0]["Month"].ToString() + " and Year=" + dtempedit.Rows[0]["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }


                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial.Columns.Add("Net_Pending_Amount");
                    foreach (DataRow DVR in dtloandetial.Rows)
                    {
                        DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Previous_Balance"].ToString()) + Convert.ToDouble(DVR["Montly_Installment"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    }
                    Div_loan1.Style.Add("display", "");

                    //Common Function add By Lokesh on 23-05-2015
                    objPageCmn.FillData((object)gvloan1, dtloandetial, "", "");

                    foreach (GridViewRow gvrl in gvloan1.Rows)
                    {
                        HiddenField hdnId = (HiddenField)gvrl.FindControl("hdnloanId");
                        Label lblloanna = (Label)gvrl.FindControl("lblloanname");
                        TextBox lblAmt = (TextBox)gvrl.FindControl("txtloanamt");

                        HiddenField HdnPaid = (HiddenField)gvrl.FindControl("hdnLoanAmount");

                        if (HdnPaid.Value != "")
                        {
                            lblAmt.Text = HdnPaid.Value;

                        }


                        if (hdnId.Value != "0" && hdnId.Value != "")
                        {

                            DataTable Dtloann = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                            Dtloann = new DataView(Dtloann, "Loan_Id=" + hdnId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();

                            if (Dtloann.Rows.Count > 0)
                            {
                                lblloanna.Text = Dtloann.Rows[0]["Loan_Name"].ToString();
                            }
                            else
                            {
                                lblloanna.Text = "";
                            }

                        }

                    }

                }


                else
                {
                    gvloan1.DataSource = null;
                    gvloan1.DataBind();
                }
            }
        }
        else
        {
            // DisplayMessage("NO Payroll Generated");


        }
    }
    protected void btnsaveloan1_Click(object sender, EventArgs e)
    {
        // Before Validation on Save
        // Validation  on Save Button too

        // Loop Acc to Grid View Rows
        // Check Acc to Loan Id and next Month Record 
        // Add in Month =1 if value is greater than 13 then we will set month = 1 and add in year one
        // Check Record Exists
        // Current Month Employee Paid Amount 
        // And Update in Next Month PB and Taotal Amount
        // Else
        // No Can edit installment
        double netamt = 0;

        double pvbal = 0;
        double instllamt = 0;
        double totalamt = 0;
        double txtamount = 0;
        double currentLoanAmt = 0;

        foreach (GridViewRow gvrloan in gvloan1.Rows)
        {
            TextBox txtamt = (TextBox)gvrloan.FindControl("txtloanamt");
            HiddenField hdnlnId = (HiddenField)gvrloan.FindControl("hdnloanId");
            HiddenField hdntrnsid = (HiddenField)gvrloan.FindControl("hdntrnasLoanId");
            Label lblloanamt = (Label)gvrloan.FindControl("lblloanamt");
            Label lblActualLOan = (Label)gvrloan.FindControl("lblActualLOan");
            if (txtamt.Text == "")
            {
                txtamt.Text = "0";
            }
            DataTable dtempedit = new DataTable();
            dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
            int counter = 0;

            instllamt = Convert.ToDouble(lblloanamt.Text);

            txtamount = Convert.ToDouble(txtamt.Text);

            currentLoanAmt = Convert.ToDouble(lblActualLOan.Text);

            int trnsid = 0;

            trnsid = Convert.ToInt32(hdntrnsid.Value);
            if (currentLoanAmt != txtamount)
            {
                DataTable dtlndedetials = new DataTable();
                dtlndedetials = objloan.GetRecord_From_PayEmployeeLoanDetailAll();
                dtlndedetials = new DataView(dtlndedetials, "Loan_Id=" + hdnlnId.Value + " and Trans_Id =" + (trnsid + 1) + " ", "", DataViewRowState.CurrentRows).ToTable();





                if (dtlndedetials.Rows.Count > 0)
                {

                    if (currentLoanAmt > txtamount)
                    {
                        pvbal = currentLoanAmt - txtamount;

                    }
                    if (txtamount > currentLoanAmt)
                    {
                        pvbal = currentLoanAmt - txtamount;

                    }

                    totalamt = instllamt + pvbal;
                    objloan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), hdnlnId.Value.ToString(), (trnsid + 1).ToString(), pvbal.ToString(), totalamt.ToString(), "0");

                }
                else
                {
                    counter = 1;

                }

            }

            if (counter == 1)
            {
                DisplayMessage("You Can Not Adjust The Amount");
                txtamt.Text = "0";
                txtamt.Focus();
                return;


            }
            else
            {
                objloan.UpdateRecord_loandetials_WithPaidStatusandAmount(hdnlnId.Value.ToString(), trnsid.ToString(), txtamt.Text, "Paid");


            }
            // Modified by Divya Joshi on 4/4/2014
            if (txtamt.Text != "")
            {
                netamt += Convert.ToDouble(txtamt.Text);
            }
        }
        //this code is created by jitendra upadhyay and divya mam on 04-04-2014
        //for insert the current month paid loan installment in paid amount
        DataTable dtPay = new DataTable();
        dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Session["EmpIdc"].ToString(), Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), netamt.ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString());
        }
        DisplayMessage("Record Updated", "green");
        return;

    }
    protected void btncenellaon1_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");

        Panel4.Visible = true;
        Div_Head.Style.Add("display", "");

        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }



    }
    protected void gvloan1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvloan1.PageIndex = e.NewPageIndex;
        DataTable dtempedit = new DataTable();
        dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        Session["monthAllow"] = dtempedit.Rows[0]["Month"].ToString();
        Session["YearAllow"] = dtempedit.Rows[0]["Year"].ToString();
        if (dtempedit.Rows.Count > 0)
        {
            DataTable Dtloan = new DataTable();
            Dtloan = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");

            Dtloan = new DataView(Dtloan, " Emp_Id=" + Session["EmpIdc"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (Dtloan.Rows.Count > 0)
            {
                DataTable dtloandetial = new DataTable();

                for (int i = 0; i < Dtloan.Rows.Count; i++)
                {
                    dtloandetial.Merge(objloan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString()));

                }

                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial = new DataView(dtloandetial, " Month=" + dtempedit.Rows[0]["Month"].ToString() + " and Year=" + dtempedit.Rows[0]["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }


                if (dtloandetial.Rows.Count > 0)
                {
                    dtloandetial.Columns.Add("Net_Pending_Amount");
                    foreach (DataRow DVR in dtloandetial.Rows)
                    {
                        DVR["Net_Pending_Amount"] = Common.GetAmountDecimal((Convert.ToDouble(DVR["Previous_Balance"].ToString()) + Convert.ToDouble(DVR["Montly_Installment"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    }
                    //Common Function add By Lokesh on 23-05-2015
                    objPageCmn.FillData((object)gvloan1, dtloandetial, "", "");

                    foreach (GridViewRow gvrl in gvloan1.Rows)
                    {
                        HiddenField hdnId = (HiddenField)gvrl.FindControl("hdnloanId");
                        Label lblloanna = (Label)gvrl.FindControl("lblloanname");
                        TextBox lblAmt = (TextBox)gvrl.FindControl("txtloanamt");

                        HiddenField HdnPaid = (HiddenField)gvrl.FindControl("hdnLoanAmount");

                        if (HdnPaid.Value != "")
                        {
                            lblAmt.Text = HdnPaid.Value;

                        }


                        if (hdnId.Value != "0" && hdnId.Value != "")
                        {

                            DataTable Dtloann = objloan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                            Dtloann = new DataView(Dtloann, "Loan_Id=" + hdnId.Value.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();

                            if (Dtloann.Rows.Count > 0)
                            {
                                lblloanna.Text = Dtloann.Rows[0]["Loan_Name"].ToString();
                            }
                            else
                            {
                                lblloanna.Text = "";
                            }

                        }

                    }

                }


                else
                {
                    gvloan1.DataSource = null;
                    gvloan1.DataBind();

                }
            }
            else
            {
                gvloan1.DataSource = null;
                gvloan1.DataBind();

            }
        }
        else
        {
            // DisplayMessage("NO Payroll Generated");


        }
    }
    #endregion



    #endregion
    #region Attendence
    #region ListView

    protected void btnattendance_Click(object sender, EventArgs e)
    {

        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //Div_gvallowance.Style.Add("display", "none");
        //Div_gvdeduuc.Style.Add("display", "none");
        //Div_loan.Style.Add("display", "none");
        //Div_penaltyclaim.Style.Add("display", "none");
        DataTable dtemppara = new DataTable();
        dtemppara = objempparam.GetEmployeeParameterByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        if (dtemppara.Rows.Count > 0)
        {
            txtBasicSalary.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtemppara.Rows[0]["Basic_Salary"].ToString());
        }
        else
        {
            txtBasicSalary.Text = "0";
        }

        DataTable dtattendance = new DataTable();
        dtattendance = objPayEmpMonth.GetallTemprecords_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        if (dtattendance.Rows.Count > 0)
        {
            //Div_gvallowance.Style.Add("display", "none");
            //Div_gvdeduuc.Style.Add("display", "none");
            //Div_loan.Style.Add("display", "none");
            //Div_penaltyclaim.Style.Add("display", "none");
            //Div_attnd.Style.Add("display", "");


            lbltrnsid.Text = dtattendance.Rows[0]["Trans_Id"].ToString();

            lblmonth.Text = dtattendance.Rows[0]["Month"].ToString();
            lblyear.Text = dtattendance.Rows[0]["Year"].ToString();

            txtworkedminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Worked_Min_Salary"].ToString());
            txtnormalOtminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Normal_OT_Min_Salary"].ToString());
            txtWeekoffotminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
            txthloidayotminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Holiday_OT_Min_Salary"].ToString());
            txtPayrolldayssal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Leave_Days_Salary"].ToString());
            txtweekoffsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Week_Off_Salary"].ToString());
            txtholidayssal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Holidays_Salary"].ToString());
            txtAbsaentsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Absent_Salary"].ToString());
            txtlateminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Late_Min_Penalty"].ToString());
            txtearlyminsal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Early_Min_Penalty"].ToString());
            txtpatialvoisal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtattendance.Rows[0]["Patial_Violation_Penalty"].ToString());



            try
            {

                double a = Convert.ToDouble(txtworkedminsal.Text);
                double b = Convert.ToDouble(txtnormalOtminsal.Text);
                double c = Convert.ToDouble(txtWeekoffotminsal.Text);
                double d = Convert.ToDouble(txthloidayotminsal.Text);
                double f = Convert.ToDouble(txtweekoffsal.Text);
                double g = Convert.ToDouble(txtholidayssal.Text);
                double h = Convert.ToDouble(txtPayrolldayssal.Text);
                double i = Convert.ToDouble(txtAbsaentsal.Text);
                double j = Convert.ToDouble(txtlateminsal.Text);
                double k = Convert.ToDouble(txtearlyminsal.Text);
                double l = Convert.ToDouble(txtpatialvoisal.Text);

                double sum = ((a + b + c + d + f + g + h) - (i + j + k + l));
                double Salary = (a + b + c + d + f + g + h);
                double Deduction = (i + j + k + l);
                Txt_Tot_Salary.Text = Salary.ToString();
                Txt_Tot_Deduction.Text = Deduction.ToString();
                txtGrossAmount.Text = sum.ToString();
                Total_Final_Paid_Amount();
            }



            catch
            {

            }
        }



    }
    protected void btnsaveattend_Click(object sender, EventArgs e)
    {

        objPayEmpMonth.UpdateAttendenceRecord_By_EmpId_Monthandyear(Session["EmpIdc"].ToString(), lblmonth.Text, lblyear.Text, txtworkedminsal.Text, txtnormalOtminsal.Text, txtWeekoffotminsal.Text, txthloidayotminsal.Text, txtPayrolldayssal.Text, txtweekoffsal.Text, txtholidayssal.Text, txtAbsaentsal.Text, txtlateminsal.Text, txtearlyminsal.Text, txtpatialvoisal.Text, Session["CompId"].ToString());
        //btnattendance_Click(null, null);
        FillGrid();
        if (Session["EmpIdc"] != null)
        {
            foreach (GridViewRow gvr in gvEmpEditPayroll.Rows)
            {
                Label lblEmpId = (Label)gvr.FindControl("lblEmpId");
                Label lblNetAmount = (Label)gvr.FindControl("lblGrossSalary");

                if (Session["EmpIdc"].ToString() == lblEmpId.Text)
                {
                    lblFinalAmount.Text = lblNetAmount.Text;
                }
            }
        }

        DisplayMessage("Record Updated", "green");
        return;

    }
    protected void btncencelattend_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd.Style.Add("display", "none");
        Panel4.Visible = true;
        FillGrid();

    }

    public void add()
    {

        try
        {

            double a = Convert.ToDouble(txtworkedminsal.Text);
            double b = Convert.ToDouble(txtnormalOtminsal.Text);
            double c = Convert.ToDouble(txtWeekoffotminsal.Text);
            double d = Convert.ToDouble(txthloidayotminsal.Text);
            double f = Convert.ToDouble(txtweekoffsal.Text);
            double g = Convert.ToDouble(txtholidayssal.Text);
            double h = Convert.ToDouble(txtPayrolldayssal.Text);
            double i = Convert.ToDouble(txtAbsaentsal.Text);
            double j = Convert.ToDouble(txtlateminsal.Text);
            double k = Convert.ToDouble(txtearlyminsal.Text);
            double l = Convert.ToDouble(txtpatialvoisal.Text);

            double sum = ((a + b + c + d + f + g + h) - (i + j + k + l));
            double Salary = (a + b + c + d + f + g + h);
            double Deduction = (i + j + k + l);
            Txt_Tot_Salary.Text = Salary.ToString();
            Txt_Tot_Deduction.Text = Deduction.ToString();
            txtGrossAmount.Text = sum.ToString();
            Total_Final_Paid_Amount();
        }
        catch
        {

        }
    }

    protected void txtworkedminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtWeekoffotminsal.Focus();
    }
    protected void txtWeekoffotminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtweekoffsal.Focus();
    }
    protected void txtweekoffsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txthloidayotminsal.Focus();
    }
    protected void txthloidayotminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtholidayssal.Focus();
    }
    protected void txtholidayssal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtnormalOtminsal.Focus();
    }
    protected void txtnormalOtminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtPayrolldayssal.Focus();
    }
    protected void txtPayrolldayssal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtlateminsal.Focus();
    }
    protected void txtlateminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtearlyminsal.Focus();
    }
    protected void txtearlyminsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtAbsaentsal.Focus();
    }
    protected void txtAbsaentsal_TextChanged(object sender, EventArgs e)
    {
        add();
        txtpatialvoisal.Focus();
    }


    protected void txtpatialvoisal_TextChanged(object sender, EventArgs e)
    {
        add();
        txttotalpeanlty.Focus();
    }

    #endregion

    #region Formview
    protected void btnattendance1_Click(object sender, EventArgs e)
    {

        pnlmenuloan.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenupenaltyclaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAllowance.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlattendance.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        DataTable dtemppara = new DataTable();
        dtemppara = objempparam.GetEmployeeParameterByEmpId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        if (dtemppara.Rows.Count > 0)
        {

            txtBasicSalary1.Text = dtemppara.Rows[0]["Basic_Salary"].ToString();
        }
        else
        {
            txtBasicSalary1.Text = "0";
        }

        DataTable dtattendance = new DataTable();
        dtattendance = objPayEmpMonth.GetallTemprecords_By_EmployeeId(Session["EmpIdc"].ToString(), Session["CompId"].ToString());
        if (dtattendance.Rows.Count > 0)
        {
            Div_gvallowance1.Style.Add("display", "none");
            Div_gvdeduuc1.Style.Add("display", "none");
            Div_loan1.Style.Add("display", "none");
            Div_penaltyclaim1.Style.Add("display", "none");
            Div_attnd1.Style.Add("display", "");

            lbltrnsid1.Text = dtattendance.Rows[0]["Trans_Id"].ToString();

            lblmonth1.Text = dtattendance.Rows[0]["Month"].ToString();
            lblyear1.Text = dtattendance.Rows[0]["Year"].ToString();

            txtworkedminsal1.Text = dtattendance.Rows[0]["Worked_Min_Salary"].ToString();
            txtnormalOtminsal1.Text = dtattendance.Rows[0]["Normal_OT_Min_Salary"].ToString();
            txtWeekoffotminsal1.Text = dtattendance.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
            txthloidayotminsal1.Text = dtattendance.Rows[0]["Holiday_OT_Min_Salary"].ToString();
            txtPayrolldayssal1.Text = dtattendance.Rows[0]["Leave_Days_Salary"].ToString();
            txtweekoffsal1.Text = dtattendance.Rows[0]["Week_Off_Salary"].ToString();
            txtholidayssal1.Text = dtattendance.Rows[0]["Holidays_Salary"].ToString();
            txtAbsaentsal1.Text = dtattendance.Rows[0]["Absent_Salary"].ToString();
            txtlateminsal1.Text = dtattendance.Rows[0]["Late_Min_Penalty"].ToString();
            txtearlyminsal1.Text = dtattendance.Rows[0]["Early_Min_Penalty"].ToString();
            txtpatialvoisal1.Text = dtattendance.Rows[0]["Patial_Violation_Penalty"].ToString();



            try
            {

                double a = Convert.ToDouble(txtworkedminsal1.Text);
                double b = Convert.ToDouble(txtnormalOtminsal1.Text);
                double c = Convert.ToDouble(txtWeekoffotminsal1.Text);
                double d = Convert.ToDouble(txthloidayotminsal1.Text);
                double f = Convert.ToDouble(txtweekoffsal1.Text);
                double g = Convert.ToDouble(txtholidayssal1.Text);
                double h = Convert.ToDouble(txtPayrolldayssal1.Text);
                double i = Convert.ToDouble(txtAbsaentsal1.Text);
                double j = Convert.ToDouble(txtlateminsal1.Text);
                double k = Convert.ToDouble(txtearlyminsal1.Text);
                double l = Convert.ToDouble(txtpatialvoisal1.Text);

                double sum = ((a + b + c + d + f + g + h) - (i + j + k + l));
                double Salary = (a + b + c + d + f + g + h);
                double Deduction = (i + j + k + l);
                Txt_Tot_Salary.Text = Salary.ToString();
                Txt_Tot_Deduction.Text = Deduction.ToString();
                txtGrossAmount1.Text = sum.ToString();
            }



            catch
            {

            }
        }
    }
    protected void btnsaveattend1_Click(object sender, EventArgs e)
    {
        objPayEmpMonth.UpdateAttendenceRecord_By_EmpId_Monthandyear(Session["EmpIdc"].ToString(), lblmonth1.Text, lblyear1.Text, txtworkedminsal1.Text, txtnormalOtminsal1.Text, txtWeekoffotminsal1.Text, txthloidayotminsal1.Text, txtPayrolldayssal1.Text, txtweekoffsal1.Text, txtholidayssal1.Text, txtAbsaentsal1.Text, txtlateminsal1.Text, txtearlyminsal1.Text, txtpatialvoisal1.Text, Session["CompId"].ToString());
        btnattendance_Click(null, null);
        FillGrid();
        if (Session["EmpIdc"] != null)
        {
            foreach (GridViewRow gvr in gvEmpEditPayroll.Rows)
            {
                Label lblEmpId = (Label)gvr.FindControl("lblEmpId");
                Label lblNetAmount = (Label)gvr.FindControl("lblGrossSalary");

                if (Session["EmpIdc"].ToString() == lblEmpId.Text)
                {
                    lblFinalAmount.Text = lblNetAmount.Text;
                }
            }
        }
        DisplayMessage("Record Updated", "green");
        return;
    }
    protected void btncencelattend1_Click(object sender, EventArgs e)
    {
        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");

        Div_Head.Style.Add("display", "");

        Panel4.Visible = true;
        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }


        FillGrid();

    }

    public void add1()
    {

        try
        {

            double a = Convert.ToDouble(txtworkedminsal1.Text);
            double b = Convert.ToDouble(txtnormalOtminsal1.Text);
            double c = Convert.ToDouble(txtWeekoffotminsal1.Text);
            double d = Convert.ToDouble(txthloidayotminsal1.Text);
            double f = Convert.ToDouble(txtweekoffsal1.Text);
            double g = Convert.ToDouble(txtholidayssal1.Text);
            double h = Convert.ToDouble(txtPayrolldayssal1.Text);
            double i = Convert.ToDouble(txtAbsaentsal1.Text);
            double j = Convert.ToDouble(txtlateminsal1.Text);
            double k = Convert.ToDouble(txtearlyminsal1.Text);
            double l = Convert.ToDouble(txtpatialvoisal1.Text);

            double sum = ((a + b + c + d + f + g + h) - (i + j + k + l));
            double Salary = (a + b + c + d + f + g + h);
            double Deduction = (i + j + k + l);
            Txt_Tot_Salary.Text = Salary.ToString();
            Txt_Tot_Deduction.Text = Deduction.ToString();
            txtGrossAmount1.Text = sum.ToString();
        }



        catch
        {

        }
    }

    protected void txtworkedminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtnormalOtminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtWeekoffotminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txthloidayotminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtPayrolldayssal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtweekoffsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtholidayssal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtAbsaentsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtlateminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtearlyminsal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    protected void txtpatialvoisal1_TextChanged(object sender, EventArgs e)
    {
        add1();
    }
    #endregion

    #endregion
    #region SavePayroll


    protected void btnSavePayroll_Click(object sender, EventArgs e)
    {
        Session["LoanStatus"] = null;
        if (gvloan.Rows.Count > 0)
        {
            btnsaveloan_Click(null, null);
        }


        if (Session["LoanStatus"] != null)
        {
            return;
        }

        if (gvallowance.Rows.Count > 0)
        {
            btnsaveallow_Click(null, null);
        }
        if (gvdeduction.Rows.Count > 0)
        {
            BtnSave_Click(null, null);
        }
        if (GvClaim.Rows.Count > 0 || gvPenalty.Rows.Count > 0)
        {
            btnsaveclaimpenalty_Click(null, null);
        }
        if (CheckAttendanceSalaryvalidationForSave(Session["EmpIdc"].ToString(), txtworkedminsal.Text, txtnormalOtminsal.Text, txtWeekoffotminsal.Text, txthloidayotminsal.Text, txtPayrolldayssal.Text, txtweekoffsal.Text, txtholidayssal.Text, txtAbsaentsal.Text, txtlateminsal.Text, txtearlyminsal.Text, txtpatialvoisal.Text, lblyear.Text, lblmonth.Text) == false)
        {
            DisplayMessage("Attendance Salary cannot exceed the basic Salary ", "green");
            return;
        }


        btnsaveattend_Click(null, null);

        Div_Head.Style.Add("display", "");




        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_loan.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }

    }

    #region export Excel
    protected void btnExportData_Clcik(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmpPayroll"];
        if (dt != null)
        {
            dt.Columns.Remove("company_id");
            dt.Columns.Remove("brand_id");
            dt.Columns.Remove("location_id");
            dt.Columns.Remove("emp_id");
            dt.Columns.Remove("civil_id");
            dt.Columns.Remove("emp_name_l");
            dt.Columns.Remove("emp_image");
            dt.Columns.Remove("department_id");
            dt.Columns.Remove("designation_id");
            dt.Columns.Remove("religion_id");
            dt.Columns.Remove("nationality_id");
            dt.Columns.Remove("qualification_id");
            dt.Columns.Remove("dob");
            dt.Columns.Remove("doj");
            dt.Columns.Remove("emp_type");
            dt.Columns.Remove("termination_date");
            dt.Columns.Remove("gender");
            dt.Columns.Remove("email_id");
            dt.Columns.Remove("phone_no");
            dt.Columns.Remove("field1");
            dt.Columns.Remove("field2");
            dt.Columns.Remove("field3");
            dt.Columns.Remove("field4");
            dt.Columns.Remove("field5");
            dt.Columns.Remove("field6");
            dt.Columns.Remove("field7");
            dt.Columns.Remove("isactive");
            dt.Columns.Remove("createdby");
            dt.Columns.Remove("createddate");
            dt.Columns.Remove("modifiedby");
            dt.Columns.Remove("modifieddate");
            dt.Columns.Remove("company_phone_no");
            dt.Columns.Remove("pan");
            dt.Columns.Remove("fathername");
            dt.Columns.Remove("ismarried");
            dt.Columns.Remove("dlno");
            dt.Columns.Remove("Payroll_Emp_Id");
            dt.Columns.Remove("employee_pf");
            dt.Columns.Remove("employee_esic");
            dt.Columns.Remove("employee_penalty");
            dt.Columns.Remove("employee_claim");
            dt.Columns.Remove("emlployee_loan");
            //dt.Columns.Remove("total_allowance");
            dt.Columns.Remove("total_deduction");
            dt.Columns.Remove("previous_month_balance");
            dt.Columns.Remove("Attendence_Salary");
            dt.AcceptChanges();
            ExportTableData(dt);
        }
    }
    public void ExportTableData(DataTable dtdata)
    {

        string strFname = "EmpPayroll";
        if (dpDepartment.SelectedValue == "--Select--")
        {
            strFname = strFname + "-" + dtdata.Rows[0]["MonthName"].ToString() + "-" + dtdata.Rows[0]["year"].ToString();
        }
        else
        {
            strFname = strFname + "-" + dpDepartment.SelectedItem.Text.ToString() + "-" + dtdata.Rows[0]["MonthName"].ToString() + "-" + dtdata.Rows[0]["year"].ToString();
        }

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, "EmpPayroll");
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

    protected void btnUploadSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GvImport"];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsValid"].ToString() == "True")
                {


                    string MonthNo = GetMonthNumber(dt.Rows[i]["MonthName"].ToString());
                    try
                    {
                        string Emp_Id = ObjDa.get_SingleValue("Select Emp_Id from Set_EmployeeMaster where Emp_Code='" + dt.Rows[i]["Emp_Code"].ToString() + "'");
                        if (Emp_Id != "")
                        {
                            objPayEmpMonth.UpdateAttendenceRecord_By_EmpId_Monthandyear(Emp_Id, MonthNo, dt.Rows[i]["Year"].ToString(), dt.Rows[i]["worked_min_salary"].ToString(), dt.Rows[i]["normal_ot_min_salary"].ToString(), dt.Rows[i]["week_off_ot_min_salary"].ToString(), dt.Rows[i]["holiday_ot_min_salary"].ToString(), dt.Rows[i]["leave_days_salary"].ToString(), dt.Rows[i]["week_off_salary"].ToString(), dt.Rows[i]["holidays_salary"].ToString(), dt.Rows[i]["absent_salary"].ToString(), dt.Rows[i]["late_min_penalty"].ToString(), dt.Rows[i]["early_min_penalty"].ToString(), dt.Rows[i]["patial_violation_penalty"].ToString(), Session["CompId"].ToString());

                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

            btnUploadReset_Click(null, null);
            FillGrid();
            DisplayMessage("Payroll Save Successfully");
        }
    }
    protected void btnUploadReset_Click(object sender, EventArgs e)
    {
        gvImport.DataSource = null;
        gvImport.DataBind();
        uploadOb.Visible = false;
        Session["GvImport"] = null;


    }
    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {

    }

    protected void btnUploadExcel_Click(object sender, EventArgs e)
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
    public void Import(String path, int fileType)
    {
        try
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

            using (OleDbConnection oledbConn = new OleDbConnection(strcon))
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [EmpPayroll$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                oleda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("IsValid", typeof(string));
                    dt.Columns.Add("TotalGrossSalary", typeof(string));
                    dt.Columns.Add("Message", typeof(string));
                    dt.AcceptChanges();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Emp_Code = dt.Rows[i]["Emp_Code"].ToString();

                        DataTable dtPayroll = ObjDa.return_DataTable("Select * from Pay_Employe_Month_Temp as PEMT inner join Set_EmployeeMaster as SE on SE.Emp_Id=PEMT.Emp_Id where SE.Emp_Code='" + Emp_Code + "'");
                        if (dtPayroll.Rows.Count > 0)
                        {


                            string Validation = CheckAttendanceSalaryvalidation(Emp_Code, dt.Rows[i]["worked_min_salary"].ToString(), dt.Rows[i]["normal_ot_min_salary"].ToString(), dt.Rows[i]["week_off_ot_min_salary"].ToString(), dt.Rows[i]["holiday_ot_min_salary"].ToString(), dt.Rows[i]["leave_days_salary"].ToString(), dt.Rows[i]["week_off_salary"].ToString(), dt.Rows[i]["holidays_salary"].ToString(), dt.Rows[i]["absent_salary"].ToString(), dt.Rows[i]["late_min_penalty"].ToString(), dt.Rows[i]["early_min_penalty"].ToString(), dt.Rows[i]["patial_violation_penalty"].ToString(), dt.Rows[i]["year"].ToString(), dt.Rows[i]["MonthName"].ToString());




                            if (Validation != "")
                            {
                                dt.Rows[i]["IsValid"] = Validation.Split('/')[0].ToString();
                                dt.Rows[i]["TotalGrossSalary"] = Validation.Split('/')[1].ToString();
                                dt.Rows[i]["Message"] = Validation.Split('/')[2].ToString();

                            }
                            else
                            {
                                dt.Rows[i]["IsValid"] = "False";
                                dt.Rows[i]["TotalGrossSalary"] = "";
                                dt.Rows[i]["Message"] = "Invalid Record Found";
                            }

                        }
                        else
                        {
                            dt.Rows[i]["IsValid"] = "False";
                            dt.Rows[i]["TotalGrossSalary"] = "";
                            dt.Rows[i]["Message"] = "Emp Code not Found";
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Session["GvImport"] = dt;
                        objPageCmn.FillData((object)gvImport, dt, "", "");

                    }

                }
                else
                {
                    uploadOb.Visible = false;
                }


            }




        }
        catch (Exception ex)
        {
            //hdnInvalidExcelRecords.Value = "0";
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (gvImport.Rows.Count == 0)
            {
                uploadOb.Visible = false;
            }
            else
            {
                uploadOb.Visible = true;
            }

        }
    }

    public string CheckAttendanceSalaryvalidation(string Emp_Code, string worked_min_salary, string normal_ot_min_salary, string week_off_ot_min_salary, string holiday_ot_min_salary, string leave_days_salary, string week_off_salary, string holidays_salary, string absent_salary, string late_min_penalty, string early_min_penalty, string patial_violation_penalty, string Year, string Month)
    {

        float AttendanceSalary = float.Parse(worked_min_salary) + float.Parse(normal_ot_min_salary) + float.Parse(week_off_ot_min_salary) + float.Parse(holiday_ot_min_salary) + float.Parse(leave_days_salary) + float.Parse(week_off_salary) + float.Parse(holidays_salary);
        float Deduction = float.Parse(late_min_penalty) + float.Parse(early_min_penalty) + float.Parse(patial_violation_penalty) + float.Parse(absent_salary);
        float TotalSalary = AttendanceSalary - Deduction;
        string Emp_Id = "";
        try
        {
            Emp_Id = ObjDa.get_SingleValue("Select Emp_Id from Set_EmployeeMaster where Emp_Code='" + Emp_Code + "'");
        }
        catch
        {
            return "False/" + TotalSalary + "/Employee Code Not Exist";
        }
        string MonthNo = "";
        try
        {
            MonthNo = GetMonthNumber(Month);
        }
        catch
        {
            return "False/" + TotalSalary + "/ Invalid Month Name";
        }
        string Allownce = ObjDa.get_SingleValue("Select SUM(Allowance_Value) as Allowance from Pay_Employe_Allowance_Temp where Emp_Id='" + Emp_Id + "' And Year='" + Year + "' and Month='" + MonthNo + "'");
        try
        {
            TotalSalary = TotalSalary + float.Parse(Allownce);
        }
        catch
        {
            Allownce = "0";
            TotalSalary = TotalSalary + 0;
        }
        try
        {
            float NetAttendanceSalary = 0;
            string BasicSalary = ObjDa.get_SingleValue("Select Basic_Salary from Set_Employee_Parameter  where IsActive='1' and  Emp_Id='" + Emp_Id + "'");
            if (BasicSalary == "")
            {
                BasicSalary = "0";
            }
            try
            {
                NetAttendanceSalary = float.Parse(BasicSalary) + float.Parse(Allownce);
            }
            catch
            {

            }
            if (TotalSalary > NetAttendanceSalary)
            {
                return "False/" + TotalSalary + "/you Can't Create Salary above " + NetAttendanceSalary + "";
            }
            else
            {
                return "True/" + TotalSalary + "/";
            }
        }
        catch
        {
            return "False/" + TotalSalary + "/Invalid Records found";
        }

    }
    public bool CheckAttendanceSalaryvalidationForSave(string Emp_Id, string worked_min_salary, string normal_ot_min_salary, string week_off_ot_min_salary, string holiday_ot_min_salary, string leave_days_salary, string week_off_salary, string holidays_salary, string absent_salary, string late_min_penalty, string early_min_penalty, string patial_violation_penalty, string Year, string MonthNo)
    {

        float AttendanceSalary = float.Parse(worked_min_salary) + float.Parse(normal_ot_min_salary) + float.Parse(week_off_ot_min_salary) + float.Parse(holiday_ot_min_salary) + float.Parse(leave_days_salary) + float.Parse(week_off_salary) + float.Parse(holidays_salary);
        float Deduction = float.Parse(late_min_penalty) + float.Parse(early_min_penalty) + float.Parse(patial_violation_penalty) + float.Parse(absent_salary);
        float TotalSalary = AttendanceSalary - Deduction;

        string Allownce = ObjDa.get_SingleValue("Select SUM(Allowance_Value) as Allowance from Pay_Employe_Allowance_Temp where Emp_Id='" + Emp_Id + "' And Year='" + Year + "' and Month='" + MonthNo + "'");
        if (Allownce == "")
        {
            Allownce = "0";
        }
        TotalSalary = TotalSalary + float.Parse(Allownce);
        try
        {
            string BasicSalary = ObjDa.get_SingleValue("Select Basic_Salary from Set_Employee_Parameter  where IsActive='1' and  Emp_Id='" + Emp_Id + "'");
            if (BasicSalary == "")
            {
                BasicSalary = "0";
            }
            float NetAttendanceSalary = float.Parse(BasicSalary) + float.Parse(Allownce);

            if (TotalSalary > NetAttendanceSalary)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch
        {
            return false;
        }

    }








    public string GetMonthNumber(string monthName)
    {
        // Parse the month name using the "MMMM" format specifier
        DateTime date = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture);

        // Extract the month number (1-based index)
        string monthNumber = date.Month.ToString();

        return monthNumber;
    }
    protected void rbtnupd_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GvImport"];
        if (dt != null)
        {
            if (rbtnupdValid.Checked)
            {
                DataTable _dtTemp = new DataView(dt, "IsValid='True'", "", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((object)gvImport, _dtTemp, "", "");
            }
            else if (rbtnupdInValid.Checked)
            {
                DataTable _dtTemp = new DataView(dt, "IsValid='False'", "", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((object)gvImport, _dtTemp, "", "");
            }
            else
            {
                objPageCmn.FillData((object)gvImport, dt, "", "");
            }
        }
    }
    #endregion
    protected void BtnCancelPayroll_Click(object sender, EventArgs e)
    {
        Div_Head.Style.Add("display", "");




        Div_Emp_Details.Style.Add("display", "none");
        Div_penaltyclaim.Style.Add("display", "none");
        Div_gvallowance.Style.Add("display", "none");
        Div_gvdeduuc.Style.Add("display", "none");
        Div_Payroll.Style.Add("display", "none");
        Emp_Details.Style.Add("display", "none");
        Div_attnd.Style.Add("display", "none");
        Panel4.Visible = true;
        Div_loan.Style.Add("display", "none");
        Div_penaltyclaim1.Style.Add("display", "none");
        Div_gvallowance1.Style.Add("display", "none");
        Div_gvdeduuc1.Style.Add("display", "none");
        Div_loan1.Style.Add("display", "none");
        Div_attnd1.Style.Add("display", "none");
        if (rbtnGroup.Checked == true)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }
        ddlLocation.Focus();

    }

    #endregion

    public void Total_Allowances()
    {
        double Net_Amount = 0;
        foreach (GridViewRow GVR in gvallowance.Rows)
        {
            TextBox Txt_Value = (TextBox)GVR.FindControl("txtValue");
            if (Txt_Value.Text.Trim() != "")
                Net_Amount += Convert.ToDouble(Txt_Value.Text);
        }
        Lbl_Total_Allowances.Text = Common.GetAmountDecimal(Net_Amount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
    }
    public void Total_Deductions()
    {
        double Net_Amount = 0;
        foreach (GridViewRow GVR in gvdeduction.Rows)
        {
            TextBox Txt_Value = (TextBox)GVR.FindControl("txtDeducValue");
            if (Txt_Value.Text.Trim() != "")
                Net_Amount += Convert.ToDouble(Txt_Value.Text);
        }
        Lbl_Total_Deductions.Text = Common.GetAmountDecimal(Net_Amount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

    }
    public void Total_Loan_Amount()
    {
        double Net_Amount = 0;
        foreach (GridViewRow GVR in gvloan.Rows)
        {
            TextBox Txt_Value = (TextBox)GVR.FindControl("txtloanamt");
            if (Txt_Value.Text.Trim() != "")
                Net_Amount += Convert.ToDouble(Txt_Value.Text);
        }
        Lbl_Total_Loan_Amount.Text = Common.GetAmountDecimal(Net_Amount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
    }
    public void Total_Final_Paid_Amount()
    {
        double Addition_Amount = 0;
        double Deduction_Amount = 0;
        double Net_Amount = 0;
        if (txtGrossAmount.Text.Trim() == "")
            txtGrossAmount.Text = "0.00";
        if (Lbl_Total_Allowances.Text.Trim() == "")
            Lbl_Total_Allowances.Text = "0.00";
        if (txttotalclaim.Text.Trim() == "")
            txttotalclaim.Text = "0.00";
        if (Lbl_Total_Deductions.Text.Trim() == "")
            Lbl_Total_Deductions.Text = "0.00";
        if (txttotalpeanlty.Text.Trim() == "")
            txttotalpeanlty.Text = "0.00";
        if (Lbl_Total_Loan_Amount.Text.Trim() == "")
            Lbl_Total_Loan_Amount.Text = "0.00";
        Addition_Amount = Convert.ToDouble(txtGrossAmount.Text) + Convert.ToDouble(Lbl_Total_Allowances.Text) + Convert.ToDouble(txttotalclaim.Text);
        Deduction_Amount = Convert.ToDouble(Lbl_Total_Deductions.Text) + Convert.ToDouble(txttotalpeanlty.Text) + Convert.ToDouble(Lbl_Total_Loan_Amount.Text);
        Net_Amount = Addition_Amount - Deduction_Amount + Convert.ToDouble(txtdueamt.Text);


        Lbl_Total_Final_Paid_Amount.Text = Common.Get_Roundoff_Amount_By_Location(Common.GetAmountDecimal(Net_Amount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
    }
    protected void txtloanamt_TextChanged(object sender, EventArgs e)
    {
        Total_Loan_Amount();
        Total_Final_Paid_Amount();
    }
    protected void txtDeducValue_TextChanged(object sender, EventArgs e)
    {
        Total_Deductions();
        Total_Final_Paid_Amount();
    }
    protected void txtValue_TextChanged(object sender, EventArgs e)
    {
        Total_Allowances();
        Total_Final_Paid_Amount();
    }

    #region selectall

    protected void chkgvSelectAll_CheckedChangedPayroll(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvEmpEditPayroll.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvEmpEditPayroll.Rows)
        {
            index = (int)gvEmpEditPayroll.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_Edit_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];

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
            Session["CHECKED_Edit_ITEMS"] = userdetails;

    }
    protected void ImgbtnSelectAll_ClickPayroll(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpPayroll"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_Edit_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_Edit_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmpEditPayroll.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_Edit_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_Edit_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpPayroll"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpEditPayroll, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }


    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpEditPayroll.Rows)
            {
                int index = (int)gvEmpEditPayroll.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmpEditPayroll.Rows)
        {
            index = (int)gvEmpEditPayroll.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_Edit_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_Edit_ITEMS"] = userdetails;
    }
    #endregion

    protected void ImgButtonDelete_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dtEmpMonth = new DataTable();
        SaveCheckedValues();
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_Edit_ITEMS"] != null)
        {
            userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];
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

        string strFinalPayEmp = string.Empty;
        for (int i = 0; i < userdetails.Count; i++)
        {
            strFinalPayEmp += userdetails[i].ToString() + ",";

            dtEmpMonth = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(userdetails[i].ToString(), Session["CompId"].ToString());
            if (dtEmpMonth.Rows.Count > 0)
            {
                b = objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), userdetails[i].ToString(), dtEmpMonth.Rows[0]["Month"].ToString(), dtEmpMonth.Rows[0]["Year"].ToString());
                objpayrolldeduc.DeletePayDeductionTemp_By_EmpId_MonthandYear(userdetails[i].ToString(), dtEmpMonth.Rows[0]["Month"].ToString(), dtEmpMonth.Rows[0]["Year"].ToString());
                objpayrollall.DeletePayAllowanceTemp_By_EmpId_MonthandYear(userdetails[i].ToString(), dtEmpMonth.Rows[0]["Month"].ToString(), dtEmpMonth.Rows[0]["Year"].ToString());

            }
        }

        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGrid();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }


    }

    #region PostPayroll
    protected void btnpostpayroll1_Click(object sender, EventArgs e)
    {
        string strCreditAccount = string.Empty;
        string strDebitAccount = string.Empty;
        string strLoanDebitAccount = string.Empty;
        SaveCheckedValues();
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_Edit_ITEMS"] != null)
        {
            userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];
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

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();

        }

        DataTable dtDebit = new DataView(dtAcParameter, "Param_Name='HR Section'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {
            strDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();

        }

        dtDebit = new DataView(dtAcParameter, "Param_Name='Employee Loan Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {

            strLoanDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();
        }

        if (strCreditAccount == "" || strDebitAccount == "" || strLoanDebitAccount == "")
        {
            DisplayMessage("Finance account not configured");
            return;
        }

        string strFinalPayEmp = string.Empty;
        for (int i = 0; i < userdetails.Count; i++)
        {
            strFinalPayEmp += userdetails[i].ToString() + ",";
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            ObjLogProcess.PostPayroll(strFinalPayEmp, ddlLocation.SelectedIndex > 0 ? ddlLocation.SelectedValue : Session["LocId"].ToString(), false, ref trns, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["TimeZoneId"].ToString(), Session["UserId"].ToString(), Session["FinanceYearId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString());

            DisplayMessage("Payroll Posted Successfully");
            Session["TerminateEmpId"] = null;
            Session["TerminationDate"] = null;
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();

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
            con.Dispose();
            return;
        }
    }
    protected void btnpostpayroll_Click(object sender, EventArgs e)
    {
        SaveCheckedValues();
        string strCreditAccount = string.Empty;
        string strDebitAccount = string.Empty;
        string strLoanCreditAccount = string.Empty;
        string strLoanDebitAccount = string.Empty;
        string strPayrollMonth = string.Empty;
        string strPayrollYear = string.Empty;
        DataTable dtEmppayroll = new DataTable();
        string FinanceAllowancesList = string.Empty;
        string FinanceDeductionList = string.Empty;

        string strLocId = ddlLocation.SelectedIndex > 0 ? ddlLocation.SelectedValue : Session["LocId"].ToString();

        string strMaxVoucheno = ObjDa.return_DataTable("select max(ISNULL( pay_employe_month.Voucher_No,0))+1 from pay_employe_month inner join set_employeemaster on pay_employe_month.Emp_Id= set_employeemaster.Emp_Id where set_employeemaster.Location_Id=" + strLocId + "").Rows[0][0].ToString();

        string strPendingGenPayroll = string.Empty;
        string StrPostedEmployee = string.Empty;
        string TransNo = string.Empty;
        string SalaryPlanid = string.Empty;
        DataTable dtAppParam = new DataTable();
        double Employee_PF = 0;
        double Employer_PF = 0;
        double Employee_ESIC = 0;
        double Employer_ESIC = 0;

        double PF = 0;
        double ESIC = 0;
        double PF1 = 0;
        double ESIC1 = 0;

        bool IsPF = false;
        bool IsESIC = false;
        double BasicSal = 0;
        double EmpGrossSalary = 0;
        double TotalEmployeeAllowance = 0;
        bool IsEmpINPayroll = false;
        double ActualGross = 0;
        string[] EmpDetails = new string[1];
        string[] LeaveDetails = new string[1];
        try
        {
            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Employee_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {
        }

        try
        {
            Employer_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        int TransId = 0;
        int EmpCount = -1;
        SaveCheckedValues();
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_Edit_ITEMS"] != null)
        {
            userdetails = (ArrayList)Session["CHECKED_Edit_ITEMS"];
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

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
            strLoanCreditAccount = strCreditAccount;
        }

        DataTable dtDebit = new DataView(dtAcParameter, "Param_Name='HR Section'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {
            strDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();

        }

        dtDebit = new DataView(dtAcParameter, "Param_Name='Employee Loan Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {

            strLoanDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();
        }



        if (strCreditAccount == "" || strDebitAccount == "" || strLoanDebitAccount == "")
        {
            DisplayMessage("Finance account not configured");
            return;
        }


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();









        string EmployeeAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account", ref trns).Rows[0]["Param_Value"].ToString());
        string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();
        bool Is_seperateVoucher_ForLeaveSalary = false;
        bool Is_seperateVoucher_ForAllowances = false;
        bool Is_seperateVoucher_ForDeductions = false;

        try
        {
            Is_seperateVoucher_ForLeaveSalary = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Leave", ref trns).Rows[0]["Param_Value"].ToString());
            Is_seperateVoucher_ForAllowances = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Allowance", ref trns).Rows[0]["Param_Value"].ToString());
            Is_seperateVoucher_ForDeductions = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Deduction", ref trns).Rows[0]["Param_Value"].ToString());
        }
        catch
        {

        }




        try
        {
            string strFinalPayEmp = string.Empty;
            for (int i = 0; i < userdetails.Count; i++)
            {
                strFinalPayEmp += userdetails[i].ToString() + ",";
            }

            string strCurrencyId = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();

            DataTable Dt_Decimal_Count = Get_Decimal_Count(strCurrencyId, "", "", "0", "", "");
            double TotalAmountPay = 0;
            double TotalLeaveSalary = 0;
            foreach (string str in strFinalPayEmp.Split(','))
            {
                if (str == "")
                {
                    continue;
                }

                dtEmppayroll = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(str, ref trns, Session["CompId"].ToString());

                if (dtEmppayroll.Rows.Count > 0)
                {
                    strPayrollMonth = dtEmppayroll.Rows[0]["Month"].ToString();
                    strPayrollYear = dtEmppayroll.Rows[0]["Year"].ToString();
                }

                var arr = strFinalPayEmp.Split(',');
                if (EmpCount == -1)
                {
                    EmpDetails = new string[arr.Length - 1];
                    LeaveDetails = new string[arr.Length - 1];
                }
                if ((str != ""))
                {
                    EmpCount++;
                    UpdateMobileAdjustedFlag(str, strPayrollMonth, strPayrollYear, trns);

                    //here we insert the row in pay_employee_attendence table with current month and current year
                    //this code is update on 05-04-2014
                    DataTable DtEmpAttendence = objPayEmpAtt.GetRecord_Emp_Attendance(str, strPayrollMonth, strPayrollYear, ref trns, Session["CompId"].ToString());
                    if (DtEmpAttendence.Rows.Count == 0)
                    {

                        //this code is created by jitendra upadhyay on 01-04-2015
                        //this code for check that record exists or not in employe detail table if exisr than we insert from employee detail table
                        DataTable dtobjempdetail = objEmpDetail.GetAllTrueRecord(ref trns);
                        try
                        {
                            dtobjempdetail = new DataView(dtobjempdetail, "Emp_Id=" + str + " and Month=" + strPayrollMonth + " and Year=" + strPayrollYear + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                        if (dtobjempdetail.Rows.Count > 0)
                        {
                            objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, strPayrollMonth, strPayrollYear, Math.Round(float.Parse(dtobjempdetail.Rows[0]["Total_Days"].ToString()), 0).ToString(), "0", Math.Round(float.Parse(dtobjempdetail.Rows[0]["Present_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["WeekOff_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Holiday_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Leave_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Absent_Days"].ToString()), 0).ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, strPayrollMonth, strPayrollYear, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }

                    PF = 0;
                    ESIC = 0;
                    PF1 = 0;
                    ESIC1 = 0;


                    IsPF = false;
                    IsESIC = false;
                    BasicSal = 0;


                    IsEmpINPayroll = false;
                    DataTable dtempmonthtemp = new DataTable();
                    dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, ref trns, Session["CompId"].ToString());
                    dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + " ", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtempmonthtemp.Rows.Count > 0)
                    {
                        EmployeeParameter objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
                        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString(), ref trns);

                        if (dt.Rows.Count > 0)
                        {

                            SalaryPlanid = dt.Rows[0]["Salary_Plan_Id"].ToString();
                            try
                            {
                                IsEmpINPayroll = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
                            }
                            catch
                            {

                            }



                            try
                            {
                                IsPF = Convert.ToBoolean(dt.Rows[0]["Field4"].ToString());
                            }
                            catch
                            {

                            }

                            try
                            {
                                IsESIC = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
                            }
                            catch
                            {

                            }


                            try
                            {
                                BasicSal = double.Parse(dt.Rows[0]["Basic_Salary"].ToString());
                            }
                            catch
                            {

                            }

                            try
                            {
                                EmpGrossSalary = Convert.ToDouble(dt.Rows[0]["Gross_Salary"].ToString());
                            }
                            catch
                            {


                            }


                            try
                            {
                                TotalEmployeeAllowance = GetActualAllowance(SalaryPlanid, EmpGrossSalary, BasicSal);
                            }
                            catch
                            {

                            }



                            if (IsEmpINPayroll)
                            {
                                if (BasicSal.ToString() != "")
                                {
                                    if (IsPF == true)
                                    {
                                        PF = (BasicSal * Employee_PF) / 100;
                                        PF1 = (BasicSal * Employer_PF) / 100;
                                    }
                                    if (IsESIC == true)
                                    {
                                        ESIC1 = (BasicSal * Employer_ESIC) / 100;
                                        ESIC = (BasicSal * Employee_ESIC) / 100;
                                    }
                                }
                            }
                        }

                        //this code also for insert the record in payroll table(pay_employee_month) from temporary table(pay_employee_month_Temp)
                        //code start

                        TransId = objPayEmpMonth.Insert_posted_Pay_Emp_Month(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), dtempmonthtemp.Rows[0]["Worked_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Normal_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holiday_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Leave_Days_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holidays_Salary"].ToString(), dtempmonthtemp.Rows[0]["Absent_Salary"].ToString(), dtempmonthtemp.Rows[0]["Late_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Early_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Patial_Violation_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Claim"].ToString(), dtempmonthtemp.Rows[0]["Emlployee_Loan"].ToString(), dtempmonthtemp.Rows[0]["Total_Allowance"].ToString(), dtempmonthtemp.Rows[0]["Total_Deduction"].ToString(), dtempmonthtemp.Rows[0]["Previous_Month_Balance"].ToString(), System.DateTime.Now.ToString(), dtempmonthtemp.Rows[0]["Employee_PF"].ToString(), dtempmonthtemp.Rows[0]["Employer_PF"].ToString(), dtempmonthtemp.Rows[0]["Employee_ESIC"].ToString(), dtempmonthtemp.Rows[0]["Employer_ESIC"].ToString(), dtempmonthtemp.Rows[0]["Field3"].ToString(), dtempmonthtemp.Rows[0]["Field4"].ToString(), dtempmonthtemp.Rows[0]["Field5"].ToString(), dtempmonthtemp.Rows[0]["Field6"].ToString(), EmpGrossSalary.ToString(), BasicSal.ToString(), TotalEmployeeAllowance.ToString(), "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        string strEmpCodeOnly = GetEmployeeCode(str);
                        StrPostedEmployee += GetEmployeeCode(str) + ",";
                        //Code end



                        DataTable Dtloan = new DataTable();
                        Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);

                        string Narration = string.Empty;
                        DataTable dtemppayrollpost = new DataTable();

                        string strAddition = string.Empty;
                        string strDeduction = string.Empty;
                        dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns, Session["CompId"].ToString());
                        string strEmployeeActualSalary = string.Empty;
                        if (dtemppayrollpost.Rows.Count > 0)
                        {
                            strEmployeeActualSalary = string.Empty;
                            string WorkedSalary = dtemppayrollpost.Rows[0]["Worked_Min_Salary"].ToString();
                            string NormalOT = dtemppayrollpost.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            string WeekOffOT = dtemppayrollpost.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            string HolidayOT = dtemppayrollpost.Rows[0]["Holiday_OT_Min_Salary"].ToString();
                            string LeaveDaysSalary = dtemppayrollpost.Rows[0]["Leave_Days_Salary"].ToString();
                            string WeekOffSalary = dtemppayrollpost.Rows[0]["Week_Off_Salary"].ToString();
                            string HolidaySalary = dtemppayrollpost.Rows[0]["Holidays_Salary"].ToString();
                            string AbsentSalary = dtemppayrollpost.Rows[0]["Absent_Salary"].ToString();
                            string LateMinPenalty = dtemppayrollpost.Rows[0]["Late_Min_Penalty"].ToString();
                            string EarlyMinPenalty = dtemppayrollpost.Rows[0]["Early_Min_Penalty"].ToString();
                            string PartialViolationPenalty = dtemppayrollpost.Rows[0]["Patial_Violation_Penalty"].ToString();
                            string EmpPenalty = dtemppayrollpost.Rows[0]["Employee_Penalty"].ToString();
                            string EmpClaim = dtemppayrollpost.Rows[0]["Employee_Claim"].ToString();
                            string EmpLoan = dtemppayrollpost.Rows[0]["Emlployee_Loan"].ToString();
                            string TotalAllowance = dtemppayrollpost.Rows[0]["Total_Allowance"].ToString();
                            string TotalDeduction = dtemppayrollpost.Rows[0]["Total_Deduction"].ToString();
                            string PreviousMonthAdjust = dtemppayrollpost.Rows[0]["Previous_Month_Balance"].ToString();
                            string EmployeePF = dtemppayrollpost.Rows[0]["Employee_PF"].ToString();
                            string EmployeeESIC = dtemppayrollpost.Rows[0]["Employee_ESIC"].ToString();

                            ActualGross = (float.Parse(WorkedSalary) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(TotalAllowance));


                            strAddition = (float.Parse(WorkedSalary) + float.Parse(NormalOT) + float.Parse(WeekOffOT) + float.Parse(HolidayOT) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(EmpClaim) + float.Parse(TotalAllowance) + float.Parse(PreviousMonthAdjust)).ToString();

                            if (Is_seperateVoucher_ForLeaveSalary)
                            {
                                strAddition = (float.Parse(strAddition) - float.Parse(LeaveDaysSalary)).ToString();
                                TotalLeaveSalary += float.Parse(LeaveDaysSalary);
                                if (float.Parse(LeaveDaysSalary) > 0)
                                {
                                    LeaveDetails[EmpCount] = str + "," + LeaveDaysSalary.ToString() + "," + "Leave Salary For the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "," + strPayrollMonth + "," + strPayrollYear;
                                }
                                else
                                {
                                    LeaveDetails[EmpCount] = "" + ",";
                                }
                            }


                            strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(TotalDeduction)).ToString();

                            //Code Comment by Ghanhshyam Suthar because loan Entry not save in this voucher on 09-02-2018
                            double sumloan = 0;

                            sumloan = Convert.ToDouble(EmpLoan);

                            //string strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(EmpLoan) + float.Parse(TotalDeduction)).ToString();
                            // Code End


                            //Narration = "Payroll For the month " + ddlMonth.SelectedItem.Text.ToString() + "-" + TxtYear.Text.ToString() + "";


                            if (strDeduction == "0")
                                Narration = "Payroll For the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "";
                            else
                                Narration = "Payroll For the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "";

                            //if (sumloan != 0)
                            //{
                            //    if (Convert.ToDouble(strAddition) < sumloan)
                            //    {
                            //        Narration = Narration + " Note: Unable to deduct loan installment amount " + sumloan + " because of insufficient salary.";
                            //    }
                            //}

                            //if (float.Parse(strAddition) > float.Parse(strDeduction))
                            //{
                            strEmployeeActualSalary = (float.Parse(strAddition) - float.Parse(strDeduction)).ToString();
                            //strEmployeeActualSalary = Common.Get_Roundoff_Amount_By_Location(strEmployeeActualSalary);
                            ////}
                            //else
                            //{
                            //    strEmployeeActualSalary = (float.Parse(strDeduction) - float.Parse(strAddition)).ToString();
                            //}


                        }


                        double GrossSalary = 0;

                        //GrossSalary = TotalAmountPay;
                        //string EmpDetail = str + "," + GrossSalary.ToString();

                        //here we are updating gross salary in pay_eemployee_month table
                        //28-06-2017 by jitendra
                        UpdateEmployeeGrossSalary(str, ActualGross.ToString(), strMaxVoucheno, strPayrollMonth, strPayrollYear, trns);

                        objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), str, DateTime.Now.ToString(), "0", GrossSalary.ToString(), Session["CurrencyId"].ToString(), "0", GrossSalary.ToString(), "0", GrossSalary.ToString(), "Pay_Employe_Month", TransId.ToString(), "0", "Employee Salary of " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + "-" + strPayrollYear, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), false.ToString(), ref trns);

                        DataTable dtArrear = ObjEmpArrear.GetRecordByEmployeeId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), str, ref trns);

                        if (dtArrear.Rows.Count > 0)
                        {
                            ObjEmpArrear.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), dtArrear.Rows[0]["Trans_Id"].ToString(), dtArrear.Rows[0]["Emp_Id"].ToString(), dtArrear.Rows[0]["From_Date"].ToString(), dtArrear.Rows[0]["To_Date"].ToString(), dtArrear.Rows[0]["Arrear_amount"].ToString(), dtArrear.Rows[0]["Currency_Id"].ToString(), true.ToString(), TransId.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        //Code End
                        //this code for insert the allowance record from temporary table to permanent table
                        //after that delete from temporary table using empid month and year

                        //code start

                        DataTable dtallowancetemp = new DataTable();
                        dtallowancetemp = objpayrollall.GetPayAllowPayaRoll(str, ref trns);

                        if (dtallowancetemp.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtallowancetemp.Rows.Count; k++)
                            {
                                if (Is_seperateVoucher_ForAllowances && (dtallowancetemp.Rows[k]["AccountNo"].ToString() != "" && dtallowancetemp.Rows[k]["AccountNo"].ToString() != "0"))
                                {
                                    if (!FinanceAllowancesList.Split(',').Contains(dtallowancetemp.Rows[k]["Allowance_Id"].ToString()))
                                    {
                                        FinanceAllowancesList += dtallowancetemp.Rows[k]["Allowance_Id"].ToString() + ",";
                                    }

                                    strEmployeeActualSalary = (float.Parse(strEmployeeActualSalary) - float.Parse(dtallowancetemp.Rows[k]["Act_Allowance_Value"].ToString())).ToString();
                                    //TotalAmountPay = TotalAmountPay - Convert.ToDouble(dtallowancetemp.Rows[k]["Act_Allowance_Value"].ToString());
                                }

                                objpayrollall.InsertPostPayrollAllowance(str, dtallowancetemp.Rows[k]["Month"].ToString(), dtallowancetemp.Rows[k]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtallowancetemp.Rows[k]["Allowance_Id"].ToString(), dtallowancetemp.Rows[k]["Allowance_Type"].ToString(), dtallowancetemp.Rows[k]["Allowance_Value"].ToString(), dtallowancetemp.Rows[k]["Act_Allowance_Value"].ToString(), System.DateTime.Now.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                objpayrollall.DeletePayAllowanceTemp_By_EmpId_MonthandYear(str, dtallowancetemp.Rows[k]["Month"].ToString(), dtallowancetemp.Rows[k]["Year"].ToString(), ref trns);
                            }
                        }
                        //code end

                        //this code for insert the deduction record from temporary table to permanent table
                        //after that delete from temporary table using empid month and year

                        //code start

                        DataTable dtdeductiontemp = new DataTable();
                        dtdeductiontemp = objpayrolldeduc.GetPayDeducPayaRoll(str, ref trns);

                        if (dtdeductiontemp.Rows.Count > 0)
                        {
                            for (int k1 = 0; k1 < dtdeductiontemp.Rows.Count; k1++)
                            {
                                if (Is_seperateVoucher_ForDeductions && (dtdeductiontemp.Rows[k1]["AccountNo"].ToString() != "" && dtdeductiontemp.Rows[k1]["AccountNo"].ToString() != "0"))
                                {
                                    if (!FinanceDeductionList.Split(',').Contains(dtdeductiontemp.Rows[k1]["Deduction_Id"].ToString()))
                                    {
                                        FinanceDeductionList += dtdeductiontemp.Rows[k1]["Deduction_Id"].ToString() + ",";
                                    }

                                    strEmployeeActualSalary = (float.Parse(strEmployeeActualSalary) + float.Parse(dtdeductiontemp.Rows[k1]["Act_Deduction_Value"].ToString())).ToString();

                                    //TotalAmountPay = TotalAmountPay + Convert.ToDouble(dtdeductiontemp.Rows[k1]["Act_Deduction_Value"].ToString());
                                }

                                objpayrolldeduc.InsertPostPayrollDeduction(str, dtdeductiontemp.Rows[k1]["Month"].ToString(), dtdeductiontemp.Rows[k1]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Id"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Type"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Value"].ToString(), dtdeductiontemp.Rows[k1]["Act_Deduction_Value"].ToString(), System.DateTime.Now.ToString(), dtdeductiontemp.Rows[k1]["applicable_amount"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                objpayrolldeduc.DeletePayDeductionTemp_By_EmpId_MonthandYear(str, dtdeductiontemp.Rows[k1]["Month"].ToString(), dtdeductiontemp.Rows[k1]["Year"].ToString(), ref trns);
                            }
                        }
                        //code end


                        strEmployeeActualSalary = Common.Get_Roundoff_Amount_By_Location(strEmployeeActualSalary, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());

                        TotalAmountPay += Convert.ToDouble(strEmployeeActualSalary);

                        string EmpDetail = str + "," + strEmployeeActualSalary.ToString() + "," + Narration + "," + strPayrollMonth + "," + strPayrollYear;
                        EmpDetails[EmpCount] = EmpDetail;



                        //this code for delete the record from pay_employee_month_temp table using employee id and month and year
                        //objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns);
                    }
                    else
                    {
                        // DisplayMessage("You can not payroll posted");
                    }
                }
            }

            //for Account Add

            //On 07-09-2015
            //Add Code for Voucher Entry for Paid Amount           

            //For Bank Account
            string strAccountId = string.Empty;
            string strVMaxIdLoan = string.Empty;
            strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);

            if (strAccountId == "")
            {
                strAccountId = "0";
            }

            //for Voucher Number
            string strVoucherNumber = getFinanceVoucherNo(ref trns);
            int VMaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", TransId.ToString(), "Pay_Employe_Month", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Payroll for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payroll posted for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            string strVMaxId = VMaxId.ToString();
            strVMaxIdLoan = VMaxId.ToString();
            //For Debit
            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay.ToString());
            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
            if (strAccountId.Split(',').Contains(strDebitAccount))
            {
                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalAmountPay.ToString(), "0.00", "Payroll for '" + StrPostedEmployee + "' for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalAmountPay.ToString(), "0.00", "Payroll for '" + StrPostedEmployee + "' for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Credit
            if (EmpDetails.Length > 0)
            {
                foreach (string EmpStr in EmpDetails)
                {
                    string Emp_Id = EmpStr.Split(',')[0].ToString();

                    string Emp_Sal = EmpStr.Split(',')[1].ToString();
                    string Voucher_Narration = EmpStr.Split(',')[2].ToString();
                    strPayrollMonth = EmpStr.Split(',')[3].ToString();
                    strPayrollYear = EmpStr.Split(',')[4].ToString();
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Emp_Sal.ToString());
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(strCreditAccount))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strCreditAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Sal, Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Sal, Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //updated on 21/11/2017
                    //updating finance voucher id in pay employee month table in case of rollback we can delete finance voucher
                    ObjDa.execute_Command("update pay_employe_month set Field10='" + strVMaxId + "'   where emp_id='" + Emp_Id + "' and month='" + strPayrollMonth + "' and year='" + strPayrollYear + "'", ref trns);

                }
            }


            //here we are making seperate voucher for leave , allowance and deduction if sepperate voucher option is true .

            if (Is_seperateVoucher_ForLeaveSalary && TotalLeaveSalary > 0)
            {
                strDebitAccount = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Leave Salary Account", ref trns).Rows[0]["Param_Value"].ToString());
                strDebitAccount = strDebitAccount.Split('/')[1].ToString();
                strVoucherNumber = getFinanceVoucherNo(ref trns);
                VMaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", TransId.ToString(), "Pay_Employe_Month", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Leave Salary for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Leave Salary for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", strVMaxIdLoan, "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strVMaxId = VMaxId.ToString();
                //For Debit
                strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), TotalLeaveSalary.ToString());
                CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(strDebitAccount))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalLeaveSalary.ToString(), "0.00", "Leave Salary  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalLeaveSalary.ToString(), "0.00", "Leave Salary  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //for credit


                if (LeaveDetails.Length > 0)
                {
                    foreach (string EmpStr in LeaveDetails)
                    {
                        string Emp_Id = EmpStr.Split(',')[0].ToString();
                        if (Emp_Id == "")
                        {
                            continue;
                        }
                        string Emp_Sal = EmpStr.Split(',')[1].ToString();
                        strPayrollMonth = EmpStr.Split(',')[3].ToString();
                        strPayrollYear = EmpStr.Split(',')[4].ToString();
                        string Voucher_Narration = EmpStr.Split(',')[2].ToString();
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Emp_Sal.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        if (strAccountId.Split(',').Contains(strCreditAccount))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strCreditAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Sal.ToString(), Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Sal, Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        //updated on 21/11/2017
                        //updating finance voucher id in pay employee month table in case of rollback we can delete finance voucher
                        ObjDa.execute_Command("update pay_employe_month set Leave_Voucher_No='" + strVMaxId + "'   where emp_id='" + Emp_Id + "' and month='" + strPayrollMonth + "' and year='" + strPayrollYear + "'", ref trns);

                    }
                }
            }


            //for allowance

            double Total_Finance_Allowance = 0;
            DataTable dtAllowanceList = new DataTable();
            DataTable dtpostedAllowances = new DataTable();
            bool IsVoucherCreated = false;
            string strRefId = string.Empty;
            string strRefName = string.Empty;
            if (Is_seperateVoucher_ForAllowances)
            {

                foreach (string str in FinanceAllowancesList.Split(','))
                {
                    if (str == "")
                    {
                        continue;
                    }

                    dtAllowanceList = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), str, ref trns);

                    if (dtAllowanceList.Rows.Count > 0)
                    {
                        strRefId = dtAllowanceList.Rows[0]["Field1"].ToString();
                        strRefName = dtAllowanceList.Rows[0]["Allowance"].ToString();
                    }
                    strVMaxId = "0";

                    IsVoucherCreated = false;
                    Total_Finance_Allowance = 0;
                    if (EmpDetails.Length > 0)
                    {
                        foreach (string EmpStr in EmpDetails)
                        {
                            string Emp_Id = EmpStr.Split(',')[0].ToString();
                            strPayrollMonth = EmpStr.Split(',')[3].ToString();
                            strPayrollYear = EmpStr.Split(',')[4].ToString();

                            dtpostedAllowances = objpayrollall.GetPostedAllowanceAll(Emp_Id, strPayrollMonth, strPayrollYear, ref trns);
                            dtpostedAllowances = new DataView(dtpostedAllowances, "Allowance_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtpostedAllowances.Rows.Count > 0)
                            {
                                Total_Finance_Allowance += Convert.ToDouble(dtpostedAllowances.Rows[0]["Act_Allowance_Value"].ToString());

                                if (!IsVoucherCreated && Total_Finance_Allowance > 0)
                                {
                                    strDebitAccount = strRefId;
                                    strVoucherNumber = getFinanceVoucherNo(ref trns);
                                    VMaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", TransId.ToString(), "Pay_Employe_Month", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Payroll Allowance(" + strRefName + ") for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payroll Allowance(" + strRefName + ") for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", strVMaxIdLoan, "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    strVMaxId = VMaxId.ToString();
                                    //For Debit
                                    strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), Total_Finance_Allowance.ToString());
                                    CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                    if (strAccountId.Split(',').Contains(strDebitAccount))
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", Total_Finance_Allowance.ToString(), "0.00", "Payroll Allowance(" + strRefName + ")  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", Total_Finance_Allowance.ToString(), "0.00", "Payroll Allowance(" + strRefName + ")  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }

                                    IsVoucherCreated = true;

                                }
                                //for credit

                                if (Convert.ToDouble(dtpostedAllowances.Rows[0]["Act_Allowance_Value"].ToString()) > 0)
                                {
                                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dtpostedAllowances.Rows[0]["Act_Allowance_Value"].ToString());
                                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                    if (strAccountId.Split(',').Contains(strCreditAccount))
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strCreditAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", dtpostedAllowances.Rows[0]["Act_Allowance_Value"].ToString(), "Payroll Allowance(" + strRefName + ") for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", dtpostedAllowances.Rows[0]["Act_Allowance_Value"].ToString(), "Payroll Allowance(" + strRefName + ") for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }


                                    ObjDa.execute_Command("update Pay_Employe_Allowance set Field10='" + strVMaxId + "' where Trans_id=" + dtpostedAllowances.Rows[0]["Trans_Id"].ToString() + "", ref trns);

                                }
                            }
                        }
                    }

                    if (strVMaxId != "0")
                    {
                        strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), Total_Finance_Allowance.ToString());
                        CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        ObjDa.execute_Command("	update ac_voucher_detail set Debit_Amount=" + Total_Finance_Allowance + ",CompanyCurrDebit=" + CompanyCurrDebit + " where Voucher_no=" + strVMaxId + " and Other_account_No='0'", ref trns);

                    }

                }
            }



            //For deduction

            if (Is_seperateVoucher_ForDeductions)
            {

                foreach (string str in FinanceDeductionList.Split(','))
                {
                    if (str == "")
                    {
                        continue;
                    }

                    dtAllowanceList = ObjDeduc.GetDeductionTruebyId(Session["CompId"].ToString(), str, ref trns);

                    if (dtAllowanceList.Rows.Count > 0)
                    {
                        strRefId = dtAllowanceList.Rows[0]["Field2"].ToString();
                        strRefName = dtAllowanceList.Rows[0]["Deduction"].ToString();
                    }
                    strVMaxId = "0";

                    IsVoucherCreated = false;
                    Total_Finance_Allowance = 0;
                    if (EmpDetails.Length > 0)
                    {
                        foreach (string EmpStr in EmpDetails)
                        {
                            string Emp_Id = EmpStr.Split(',')[0].ToString();
                            strPayrollMonth = EmpStr.Split(',')[3].ToString();
                            strPayrollYear = EmpStr.Split(',')[4].ToString();

                            dtpostedAllowances = objpayrolldeduc.GetRecordPostedDeductionAll(Emp_Id, strPayrollMonth, strPayrollYear, ref trns);
                            dtpostedAllowances = new DataView(dtpostedAllowances, "Deduction_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtpostedAllowances.Rows.Count > 0)
                            {
                                Total_Finance_Allowance += Convert.ToDouble(dtpostedAllowances.Rows[0]["Act_Deduction_Value"].ToString());

                                if (!IsVoucherCreated && Total_Finance_Allowance > 0)
                                {
                                    strDebitAccount = strRefId;
                                    strVoucherNumber = getFinanceVoucherNo(ref trns);
                                    VMaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", TransId.ToString(), "Pay_Employe_Month", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Payroll Deduction(" + strRefName + ") for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payroll Deduction(" + strRefName + ") for the month " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", strVMaxIdLoan, "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    strVMaxId = VMaxId.ToString();
                                    //For Credit
                                    strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), Total_Finance_Allowance.ToString());
                                    CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                    if (strAccountId.Split(',').Contains(strDebitAccount))
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Total_Finance_Allowance.ToString(), "Payroll Deduction(" + strRefName + ")  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strDebitAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Total_Finance_Allowance.ToString(), "Payroll Deduction(" + strRefName + ") for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }

                                    IsVoucherCreated = true;

                                }
                                //for Debit

                                if (Convert.ToDouble(dtpostedAllowances.Rows[0]["Act_Deduction_Value"].ToString()) > 0)
                                {
                                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dtpostedAllowances.Rows[0]["Act_Deduction_Value"].ToString());
                                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                    if (strAccountId.Split(',').Contains(strCreditAccount))
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strCreditAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", dtpostedAllowances.Rows[0]["Act_Deduction_Value"].ToString(), "0.00", "Payroll Deduction(" + strRefName + ")  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", dtpostedAllowances.Rows[0]["Act_Deduction_Value"].ToString(), "0.00", "Payroll Deduction(" + strRefName + ")  for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }


                                    ObjDa.execute_Command("update Pay_Employe_Deduction set Field10='" + strVMaxId + "' where Trans_id=" + dtpostedAllowances.Rows[0]["Trans_Id"].ToString() + "", ref trns);

                                }
                            }
                        }
                    }

                    if (strVMaxId != "0")
                    {
                        strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), Total_Finance_Allowance.ToString());
                        CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        ObjDa.execute_Command("	update ac_voucher_detail set Credit_Amount=" + Total_Finance_Allowance + ",CompanyCurrCredit=" + CompanyCurrDebit + " where Voucher_no=" + strVMaxId + " and Other_account_No='0'", ref trns);

                    }

                }
            }




            // if Radio button Employee is Select then
            Create_Loan_Voucher(strFinalPayEmp, Dt_Decimal_Count, strCurrencyId, strVMaxIdLoan.ToString(), TransId, strPayrollMonth, strPayrollYear, strLoanCreditAccount, strLoanDebitAccount, trns);



            if (StrPostedEmployee != "")
            {

                DisplayMessage("Payroll Posted Successfully For Employee Code = " + StrPostedEmployee);
                Session["TerminateEmpId"] = null;
                Session["TerminationDate"] = null;
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();

            }
            else
            {
                DisplayMessage("Payroll not generated");
                Session["TerminateEmpId"] = null;
                Session["TerminationDate"] = null;
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }

            FillGrid();
            txtValue1.Text = "";
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



    public string getFinanceVoucherNo(ref SqlTransaction trns)
    {
        //for Voucher Number
        string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (strVoucherNumber != "")
        {
            DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
            if (dtCount.Rows.Count > 0)
            {
                dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtCount.Rows.Count == 0)
            {
                strVoucherNumber = strVoucherNumber + "1";
            }
            else
            {
                double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                strVoucherNumber = strVoucherNumber + TotalCount;
            }
        }
        return strVoucherNumber;
    }
    public void Create_Loan_Voucher(string strFinalPayEmp, DataTable Dt_Decimal_Count, string strCurrencyId, string Salary_Header_ID, int Insert_posted_Pay_Emp_Month_Trans_ID, string strPayrollMonth, string strPayrollYear, string strCreditAccount, string strDebotAccount, SqlTransaction trns)
    {


        string Paid_Loan_Amount = string.Empty;

        double ActualGross_Loan = 0;
        string StrPostedEmployee_Loan = string.Empty;
        string[] EmpDetails_Loan = new string[1];
        int EmpCount_Loan = -1;
        double TotalAmountPay_Loan = 0;
        double All_employee_Loan_Sum = 0;
        foreach (string str in strFinalPayEmp.Split(','))
        {
            var arr = strFinalPayEmp.Split(',');
            if (EmpCount_Loan == -1)
                EmpDetails_Loan = new string[arr.Length - 1];
            if ((str != ""))
            {
                EmpCount_Loan++;
                DataTable dtempmonthtemp = new DataTable();
                dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, ref trns, Session["CompId"].ToString());
                dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + " ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtempmonthtemp.Rows.Count > 0)
                {
                    string strEmpCodeOnly = GetEmployeeCode(str);
                    StrPostedEmployee_Loan += GetEmployeeCode(str) + ",";
                    DataTable Dtloan = new DataTable();
                    Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);
                    string Narration = string.Empty;
                    DataTable dtemppayrollpost = new DataTable();
                    dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns, Session["CompId"].ToString());
                    string strEmployeeActualSalary = string.Empty;
                    if (dtemppayrollpost.Rows.Count > 0)
                    {
                        strEmployeeActualSalary = string.Empty;
                        string WorkedSalary = dtemppayrollpost.Rows[0]["Worked_Min_Salary"].ToString();
                        string NormalOT = dtemppayrollpost.Rows[0]["Normal_OT_Min_Salary"].ToString();
                        string WeekOffOT = dtemppayrollpost.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                        string HolidayOT = dtemppayrollpost.Rows[0]["Holiday_OT_Min_Salary"].ToString();
                        string LeaveDaysSalary = dtemppayrollpost.Rows[0]["Leave_Days_Salary"].ToString();
                        string WeekOffSalary = dtemppayrollpost.Rows[0]["Week_Off_Salary"].ToString();
                        string HolidaySalary = dtemppayrollpost.Rows[0]["Holidays_Salary"].ToString();
                        string AbsentSalary = dtemppayrollpost.Rows[0]["Absent_Salary"].ToString();
                        string LateMinPenalty = dtemppayrollpost.Rows[0]["Late_Min_Penalty"].ToString();
                        string EarlyMinPenalty = dtemppayrollpost.Rows[0]["Early_Min_Penalty"].ToString();
                        string PartialViolationPenalty = dtemppayrollpost.Rows[0]["Patial_Violation_Penalty"].ToString();
                        string EmpPenalty = dtemppayrollpost.Rows[0]["Employee_Penalty"].ToString();
                        string EmpClaim = dtemppayrollpost.Rows[0]["Employee_Claim"].ToString();
                        string Employee_Loan = dtemppayrollpost.Rows[0]["Emlployee_Loan"].ToString();
                        string TotalAllowance = dtemppayrollpost.Rows[0]["Total_Allowance"].ToString();
                        string TotalDeduction = dtemppayrollpost.Rows[0]["Total_Deduction"].ToString();
                        string PreviousMonthAdjust = dtemppayrollpost.Rows[0]["Previous_Month_Balance"].ToString();
                        string EmployeePF = dtemppayrollpost.Rows[0]["Employee_PF"].ToString();
                        string EmployeeESIC = dtemppayrollpost.Rows[0]["Employee_ESIC"].ToString();
                        ActualGross_Loan = (float.Parse(WorkedSalary) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(TotalAllowance));
                        string strAddition = (float.Parse(WorkedSalary) + float.Parse(NormalOT) + float.Parse(WeekOffOT) + float.Parse(HolidayOT) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(EmpClaim) + float.Parse(TotalAllowance) + float.Parse(PreviousMonthAdjust)).ToString();
                        string strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(TotalDeduction)).ToString();
                        double sumloan = 0;

                        Paid_Loan_Amount = Employee_Loan;
                        sumloan = Convert.ToDouble(Employee_Loan);
                        strEmployeeActualSalary = sumloan.ToString();
                        TotalAmountPay_Loan = sumloan;
                        All_employee_Loan_Sum = All_employee_Loan_Sum + sumloan;
                        //if (sumloan != 0)
                        //{
                        //if (Convert.ToDouble(strAddition) >= sumloan)
                        //{
                        Narration = "Loan installment deduction for the Month : " + GetMonthName(Convert.ToInt32(dtempmonthtemp.Rows[0]["Month"].ToString())) + " - " + dtempmonthtemp.Rows[0]["Year"].ToString() + " (Loan Amount = " + (Convert.ToDouble(sumloan)).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString()) + ")";
                        string EmpDetail = str + "," + strEmployeeActualSalary.ToString() + "," + Narration + "," + dtempmonthtemp.Rows[0]["Month"].ToString() + "," + dtempmonthtemp.Rows[0]["Year"].ToString();
                        EmpDetails_Loan[EmpCount_Loan] = EmpDetail;
                        //}
                        //}
                    }
                    double GrossSalary = 0;
                    objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns);
                }
                else
                {

                }
            }
        }

        string strAccountId_Loan = string.Empty;
        DataTable dtAccount_Loan = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
        if (dtAccount_Loan.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccount_Loan.Rows.Count; i++)
            {
                if (strAccountId_Loan == "")
                {
                    strAccountId_Loan = dtAccount_Loan.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountId_Loan = strAccountId_Loan + "," + dtAccount_Loan.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountId_Loan = "0";
        }


        string strVoucherNumber_Loan = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (strVoucherNumber_Loan != "")
        {
            DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
            if (dtCount.Rows.Count > 0)
            {
                dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtCount.Rows.Count == 0)
            {
                strVoucherNumber_Loan = strVoucherNumber_Loan + "1";
            }
            else
            {
                double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                strVoucherNumber_Loan = strVoucherNumber_Loan + TotalCount;
            }
        }

        if (EmpDetails_Loan.Length > 0)
        {
            EmpDetails_Loan = EmpDetails_Loan.Where(str => !string.IsNullOrEmpty(str)).ToArray();
            List<string> Temp_List_Emp = new List<string>();
            foreach (string str in EmpDetails_Loan)
            {
                if (!string.IsNullOrEmpty(str))
                    Temp_List_Emp.Add(str);
            }
            EmpDetails_Loan = Temp_List_Emp.ToArray();
        }

        if (EmpDetails_Loan.Length > 0)
        {
            btnsaveloan_Click(EmpDetails_Loan, Paid_Loan_Amount, "", strPayrollMonth, strPayrollYear, ref trns);


            if (All_employee_Loan_Sum != 0)
            {


                int VMaxId_Loan = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", Insert_posted_Pay_Emp_Month_Trans_ID.ToString(), "Loan_Deduction", "0", DateTime.Now.ToString(), strVoucherNumber_Loan, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Loan Deduction for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Loan installment for the month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", Salary_Header_ID.ToString(), "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId_Loan = VMaxId_Loan.ToString();
                string strLocationCurrencyId_Loan = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
                string strLocationCurrencyValue_Loan = GetCurrency(strLocationCurrencyId_Loan, TotalAmountPay_Loan.ToString());
                string TotalAmountLocation_Loan = TotalAmountPay_Loan.ToString();


                //For Credit


                foreach (string EmpStr in EmpDetails_Loan)
                {

                    string Emp_Id = EmpStr.Split(',')[0].ToString();
                    string Emp_Loan = EmpStr.Split(',')[1].ToString();
                    //For Debit
                    if (Emp_Loan.ToString() != "" && Emp_Loan.ToString() != "0")
                    {
                        string strCompanyCrrValueDr_Loan = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay_Loan.ToString());
                        string CompanyCurrDebit_Loan = strCompanyCrrValueDr_Loan.Trim().Split('/')[0].ToString();
                        if (strAccountId_Loan.Split(',').Contains(strDebotAccount))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", strDebotAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Loan, "Loan installment deposited for the Month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit_Loan, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", strDebotAccount, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Loan, "Loan installment deposited for the Month : " + GetMonthName(Convert.ToInt32(strPayrollMonth)) + " - " + strPayrollYear + " On Location'" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit_Loan, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }


                        string Voucher_Narration = EmpStr.Split(',')[2].ToString();
                        string EmployeeAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account", ref trns).Rows[0]["Param_Value"].ToString());
                        string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay_Loan.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        if (strAccountId_Loan.Split(',').Contains(strCreditAccount))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", strCreditAccount, "0", "0", "PP", "1/1/1800", "1/1/1800", "", Emp_Loan, "0.00", Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", Emp_Loan, "0.00", Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }

                foreach (string EmpStr in EmpDetails_Loan)
                {
                    string Emp_Id = EmpStr.Split(',')[0].ToString();
                    strPayrollMonth = EmpStr.Split(',')[3].ToString();
                    strPayrollYear = EmpStr.Split(',')[4].ToString();

                    UpdateEmployeeLoanVoucherId(Emp_Id, strVMaxId_Loan, strPayrollMonth, strPayrollYear, trns);
                }
            }


        }

    }
    protected void btnsaveloan_Click(string[] EmpDetails_Loan, string Paid_Loan_Amount, string Voucher_ID, string strMonth, string strYear, ref SqlTransaction trns)
    {
        double netamt = 0;
        double pvbal = 0;
        double instllamt = 0;
        double totalamt = 0;
        double txtamount = 0;
        double currentLoanAmt = 0;
        foreach (string EmpStr in EmpDetails_Loan)
        {
            string Emp_Id = EmpStr.Split(',')[0].ToString();
            string Emp_Loan = EmpStr.Split(',')[1].ToString();
            strMonth = EmpStr.Split(',')[3].ToString();
            strYear = EmpStr.Split(',')[4].ToString();


            double sumloan = 0;
            DataTable Dtloan = new DataTable();
            Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);
            DataTable Dtloan_Str = new DataView(Dtloan, " Emp_Id=" + Emp_Id + "", "", DataViewRowState.CurrentRows).ToTable();
            for (int i = 0; i < Dtloan_Str.Rows.Count; i++)
            {
                string strLoandetailId = string.Empty;
                DataTable dtloandetial = new DataTable();
                dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan_Str.Rows[i]["Loan_Id"].ToString(), ref trns);
                dtloandetial = new DataView(dtloandetial, "Month=" + strMonth + " and Year=" + strYear + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                //dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtloandetial.Rows.Count > 0)
                {
                    double txtamt = Convert.ToDouble(Paid_Loan_Amount);
                    DataTable dtempedit = new DataTable();
                    dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Emp_Id, ref trns, Session["CompId"].ToString());
                    int counter = 0;
                    instllamt = Convert.ToDouble(dtloandetial.Rows[0]["Montly_Installment"].ToString());
                    txtamount = txtamt;
                    //txtamount = Convert.ToDouble(dtloandetial.Rows[0]["Montly_Installment"].ToString());
                    currentLoanAmt = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"].ToString());
                    int trnsid = 0;
                    trnsid = Convert.ToInt32(dtloandetial.Rows[0]["Trans_Id"].ToString());
                    if (currentLoanAmt != txtamount)
                    {
                        DataTable dtlndedetials = new DataTable();
                        dtlndedetials = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailAll(ref trns);
                        dtlndedetials = new DataView(dtlndedetials, "Loan_Id=" + dtloandetial.Rows[0]["Loan_Id"].ToString() + " and Trans_Id > " + (trnsid) + " ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtlndedetials.Rows.Count > 0)
                        {
                            if (currentLoanAmt > txtamount)
                            {
                                pvbal = currentLoanAmt - txtamount;
                                totalamt = instllamt + pvbal;
                                objEmpLoan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), (trnsid + 1).ToString(), pvbal.ToString(), totalamt.ToString(), "0");
                            }
                            else if (txtamount > currentLoanAmt)
                            {
                                double TotalPendingamount = currentLoanAmt;
                                foreach (DataRow dr in dtlndedetials.Rows)
                                {
                                    TotalPendingamount += Convert.ToDouble(dr["total_amount"].ToString());
                                }
                                if (txtamount > TotalPendingamount)
                                {
                                    DisplayMessage("Paid amount should not be greater then loan due amount");
                                    return;
                                }
                                double PreviousBalance = 0;
                                pvbal = txtamount - currentLoanAmt;
                                PreviousBalance = pvbal;
                                double currentloaninstallment = 0;
                                string strStatus = string.Empty;
                                foreach (DataRow dr in dtlndedetials.Rows)
                                {
                                    if (pvbal <= 0)
                                    {
                                        break;
                                    }
                                    currentLoanAmt = Convert.ToDouble(dr["Montly_installment"].ToString());
                                    if (PreviousBalance > currentLoanAmt)
                                    {
                                        currentloaninstallment = 0;
                                        PreviousBalance = (currentLoanAmt);
                                    }
                                    else
                                    {
                                        currentloaninstallment = currentLoanAmt - PreviousBalance;
                                    }
                                    objEmpLoan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), dr["Trans_Id"].ToString(), (0 - PreviousBalance).ToString(), currentloaninstallment.ToString(), "0", ref trns);
                                    pvbal = pvbal - PreviousBalance;
                                    if (currentloaninstallment <= 0 || currentloaninstallment <= 1)
                                    {
                                        ObjDa.execute_Command("update pay_employee_loan_detail set is_status = 'Paid'  where Trans_id = " + dr["Trans_Id"].ToString() + "", ref trns);
                                    }
                                    PreviousBalance = pvbal;
                                }
                            }
                        }
                        else
                        {
                            counter = 1;
                        }
                    }

                    string strstatus = string.Empty;
                    if (counter == 1)
                    {
                        DisplayMessage("You Can Not Adjust The Amount");
                        return;
                    }
                    else
                    {
                        if (txtamt > 0)
                        {
                            strstatus = "Paid";
                        }
                        else
                        {
                            strstatus = "Pending";
                        }
                        objEmpLoan.UpdateRecord_loandetials_WithPaidStatusandAmount(dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), trnsid.ToString(), txtamt.ToString(), strstatus, ref trns);
                    }
                    if (txtamt != 0)
                    {
                        netamt += txtamt;
                    }
                }
                DataTable dtPay = new DataTable();
                dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Emp_Id, strMonth, strYear, ref trns, Session["CompId"].ToString());
                if (dtPay.Rows.Count > 0)
                {
                    objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Emp_Id, strMonth, strYear, dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), netamt.ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString(), ref trns);
                }
            }
            //UpdateEmployeeLoanVoucherId(Emp_Id, Voucher_ID, trns);
        }
    }
    public DataTable Get_Decimal_Count(string Currency_ID, string Currency_Code, string Currency_Name, string Country_Id, string Country_Name, string Country_Code)
    {
        DataTable Dt_Decimal_Count = new DataTable();
        Dt_Decimal_Count = objCurrency.GetDecimalCount(Currency_ID, Currency_Code, Currency_Name, Country_Id, Country_Name, Country_Code, "1");
        return Dt_Decimal_Count;
    }
    public void UpdateMobileAdjustedFlag(string strEmpId, string strMonth, string strYear, SqlTransaction trans)
    {
        Pay_MobileBillPayment ObjMobilePayment = new Pay_MobileBillPayment(Session["DBConnection"].ToString());

        DataTable dt = ObjMobilePayment.GetRecordByEmployeeId(Session["CompId"].ToString(), strEmpId, ref trans);

        dt = new DataView(dt, "Month='" + strMonth + "' and year='" + strYear + "' and Exceed_Amount>0", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ObjMobilePayment.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), strEmpId, dt.Rows[0]["Mobile_Number"].ToString(), dt.Rows[0]["Month"].ToString(), dt.Rows[0]["Year"].ToString(), dt.Rows[0]["Bill_Amount"].ToString(), dt.Rows[0]["Bill_Limit"].ToString(), dt.Rows[0]["Exceed_Amount"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), dt.Rows[0]["Operator"].ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), true.ToString(), ref trans);
        }
    }
    public string GetMonthName(int MonthNumber)
    {
        System.Globalization.DateTimeFormatInfo mfi = new
System.Globalization.DateTimeFormatInfo();
        return mfi.GetMonthName(MonthNumber).ToString();
    }
    public double GetActualAllowance(string strSalaryPlanId, double GrossAmt, double BasicSalary)
    {


        double RowAmount = 0;



        DataTable dtsalaryPlanAll = objsalaryplandetail.GetDeduction_By_headerId(strSalaryPlanId);


        DataTable dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        //  dttDetail = dttDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Id", "Calculation_Method", "Value", "Amount", "Deduction_Applicable");

        foreach (DataRow dr in dtsalaryPlan.Rows)
        {

            string strCalcmethod = string.Empty;
            if (dr["Calculation_Method"].ToString() == "Fixed")
            {



                RowAmount += Convert.ToDouble(dr["Value"].ToString());


            }
            else
            {


                RowAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;

            }
        }


        RowAmount += (GrossAmt - (BasicSalary + RowAmount));


        return RowAmount;
    }
    public void UpdateEmployeeGrossSalary(string strEmpId, string strGrossSalary, string strVoucherNo, string strPayrollMonth, string strPayrollYear, SqlTransaction trns)
    {
        ObjDa.execute_Command("update Pay_Employe_Month  set Field8='" + strGrossSalary + "',Voucher_No=" + strVoucherNo + " where Emp_Id='" + strEmpId + "' and MONTH='" + strPayrollMonth + "' and YEAR='" + strPayrollYear + "'", ref trns);
    }
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = ObjLocationMaster.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["CurrencyId"].ToString();
        //ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();


        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = ObjDa.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }
    public void UpdateEmployeeLoanVoucherId(string strEmpId, string strVoucherNo, string strPayrollMonth, string strPayrollYear, SqlTransaction trns)
    {
        ObjDa.execute_Command("update Pay_Employe_Month  set LOan_Voucher_No=" + strVoucherNo + " where Emp_Id='" + strEmpId + "' and MONTH='" + strPayrollMonth + "' and YEAR='" + strPayrollYear + "'", ref trns);

        ObjDa.execute_Command("update Pay_Employe_Month  set LOan_Voucher_No=" + strVoucherNo + " where Emp_Id='" + strEmpId + "' and MONTH='" + strPayrollMonth + "' and YEAR='" + strPayrollYear + "'", ref trns);
    }
    #endregion
}