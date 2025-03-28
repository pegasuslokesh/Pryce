using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HR_Report_Hr_Employee_Pay_Slip : BasePage
{
    Projected_Salary_Transfer_Report_Print objReport = null;
    CompanyMaster Objcompany = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    UserMaster objUser = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new Projected_Salary_Transfer_Report_Print(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
         
            DataTable Dt = objVoucherDetail.Get_Employee_Pay_Slip_Before_Post(Session["CompId"].ToString(), Session["Querystring"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
            DataTable Dt_Filter_by_Type = Dt.DefaultView.ToTable(true, "AccountName");
            DDl_Filter.DataSource = Dt_Filter_by_Type;
            DDl_Filter.DataTextField = "AccountName";
            DDl_Filter.DataBind();
            DDl_Filter.Items.Insert(0, "--By All--");
            DDl_Filter.SelectedIndex = 0;
            Session["Post_Type_Projected_Salary_Report"] = "Before_Post";
        }
        GetReport();
        AllPageCode();
    }
    public void AllPageCode()
    {
        Page.Title = ObjSysParam.GetSysTitle();
    }
    void GetReport()
    {
        if (Session["Post_Type_Projected_Salary_Report"]==null || Session["LocationName"] == null || Session["DepartmentName"] == null || Session["CompId"] == null || Session["Querystring"] == null || Session["Month"] == null || Session["Year"] == null)
        {
            string TARGET_URL = "../HR_Report/GeneratePayrollReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            DataTable Dt = new DataTable();
            if(Ddl_Post.SelectedIndex==0)
            {
                if(Session["Post_Type_Projected_Salary_Report"].ToString() == "After_Post")
                {
                    DataTable Dt_Post = objVoucherDetail.Get_Employee_Pay_Slip_Before_Post(Session["CompId"].ToString(), Session["Querystring"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
                    DataTable Dt_Filter_by_Type = Dt_Post.DefaultView.ToTable(true, "AccountName");
                    DDl_Filter.DataSource = Dt_Filter_by_Type;
                    DDl_Filter.DataTextField = "AccountName";
                    DDl_Filter.DataBind();
                    DDl_Filter.Items.Insert(0, "--By All--");
                    DDl_Filter.SelectedIndex = 0;
                    Session["Post_Type_Projected_Salary_Report"] = "Before_Post";
                }
                Dt = objVoucherDetail.Get_Employee_Pay_Slip_Before_Post(Session["CompId"].ToString(), Session["Querystring"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    if (DDl_Filter.SelectedItem.Text.ToString() != "--By All--")
                        Dt = new DataView(Dt, "AccountName = '" + DDl_Filter.SelectedItem.ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            else
            {
                if (Session["Post_Type_Projected_Salary_Report"].ToString() == "Before_Post")
                {
                    DataTable Dt_Post = objVoucherDetail.Get_Employee_Pay_Slip_After_Post(Session["CompId"].ToString(), Session["Querystring"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
                    DataTable Dt_Filter_by_Type = Dt_Post.DefaultView.ToTable(true, "AccountName");
                    DDl_Filter.DataSource = Dt_Filter_by_Type;
                    DDl_Filter.DataTextField = "AccountName";
                    DDl_Filter.DataBind();
                    DDl_Filter.Items.Insert(0, "--By All--");
                    DDl_Filter.SelectedIndex = 0;
                    Session["Post_Type_Projected_Salary_Report"] = "After_Post";
                }
                Dt = objVoucherDetail.Get_Employee_Pay_Slip_After_Post(Session["CompId"].ToString(), Session["Querystring"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    if (DDl_Filter.SelectedItem.Text.ToString() != "--By All--")
                        Dt = new DataView(Dt, "AccountName = '" + DDl_Filter.SelectedItem.ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            
            string CompanyName = "";
            string CompanyAddress = "";
            string Imageurl = "";
            string CompanyName_L = "";
            string Companytelno = string.Empty;
            string CompanyFaxno = string.Empty;
            string CompanyWebsite = string.Empty;
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
            string ParameterType = string.Empty;
            string ParameterValue = string.Empty;

            ParameterType = "Company";
            ParameterValue = Session["Compid"].ToString();

            string[] strParam = Common.ReportHeaderSetup(ParameterType, ParameterValue, Session["DBConnection"].ToString());
            CompanyName = strParam[0].ToString();
            CompanyName_L = strParam[1].ToString();
            CompanyAddress = strParam[2].ToString();
            Companytelno = strParam[3].ToString();
            CompanyFaxno = strParam[4].ToString();
            CompanyWebsite = strParam[5].ToString();
            Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
            DataTable dtemployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());
            try
            {
                dtemployee = new DataView(dtemployee, "Emp_Id=" + Session["EmpId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            string signatureurl = string.Empty;
            if (dtemployee.Rows.Count > 0)
            {
                signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field3"].ToString();
            }
            Title = "Employee Payment";

            string createdby = string.Empty;
            if (Session["EmpId"].ToString().Trim() == "0")
            {
                createdby = "SuperAdmin";
            }
            else
            {
                try
                {
                    createdby = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString()).Rows[0]["EmpName"].ToString();
                }
                catch
                {
                }

            }

            objReport.setUserName(createdby);

            objReport.setSignature(signatureurl);
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.SetImage(Imageurl);
            objReport.SetParameter(Session["LocationName"].ToString(), Session["DepartmentName"].ToString());
            objReport.SetTitle("PROJECTED SALARY TRANSFER REPORT - " + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString());            
            
            if(Session["lang"].ToString()=="1")
            {
                //English
                objReport.Set_Parameter_By_Language("English",CompanyName, CompanyAddress, CompanyWebsite, Resources.Attendance.Location, Session["LocationName"].ToString(), Resources.Attendance.Department, Session["DepartmentName"].ToString(), Resources.Attendance.ProjectSalaryTransfeReport + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString(), Resources.Attendance.S_No, Resources.Attendance.EmpName, Resources.Attendance.BName, Resources.Attendance.Branch, Resources.Attendance.SwiftCode, Resources.Attendance.IBANCode, Resources.Attendance.IFSCCode, Resources.Attendance.Account_Number, Resources.Attendance.Amount, Resources.Attendance.Sub_Total, Resources.Attendance.TotalAmount, Resources.Attendance.GeneratedBy, Resources.Attendance.Signature, Resources.Attendance.Approved_By, Resources.Attendance.CreateBy);                
            }
            else
            {
                // Arabic
                objReport.Set_Parameter_By_Language("Arabic",CompanyName_L, CompanyAddress, CompanyWebsite, Resources.Attendance.Location, Session["LocationName"].ToString(), Resources.Attendance.Department, Session["DepartmentName"].ToString(), Resources.Attendance.ProjectSalaryTransfeReport + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString(), Resources.Attendance.S_No, Resources.Attendance.EmpName, Resources.Attendance.BName, Resources.Attendance.Branch, Resources.Attendance.SwiftCode, Resources.Attendance.IBANCode, Resources.Attendance.IFSCCode, Resources.Attendance.Account_Number, Resources.Attendance.Amount, Resources.Attendance.Sub_Total, Resources.Attendance.TotalAmount, Resources.Attendance.GeneratedBy, Resources.Attendance.Signature, Resources.Attendance.Approved_By, Resources.Attendance.CreateBy);
            }
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);
            objReport.DataSource = Dt;
            objReport.DataMember = "DT_Employee_Pay_Slip_Before_Post";
            rptViewer.Report = objReport;
            rptToolBar.ReportViewer = rptViewer;
        }
    }
    protected void DDl_Filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetReport();
    }

    protected void Ddl_Post_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetReport();
    }
}
