using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HR_HR_EmployeePayment_Print : System.Web.UI.Page
{
    Employee_Payment objReport = null;
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

        objReport = new Employee_Payment(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());

        GetReport();
        AllPageCode();
    }
    public void AllPageCode()
    {
        Page.Title = ObjSysParam.GetSysTitle();
    }
    void GetReport()
    {
        DataTable Dt = new DataTable();
        EmployeeLoanDataset ObjSalesDataset = new EmployeeLoanDataset();
        EmployeeLoanDatasetTableAdapters.Get_Employee_Payment_Voucher_DetailTableAdapter adp = new EmployeeLoanDatasetTableAdapters.Get_Employee_Payment_Voucher_DetailTableAdapter();
        adp.Fill(ObjSalesDataset.Get_Employee_Payment_Voucher_Detail, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0, "Employee Salary", "EAP", Convert.ToString(Session["EP_Voucher_No"].ToString()));
        Dt = ObjSalesDataset.Get_Employee_Payment_Voucher_Detail;
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

        ParameterType = "Location";
        ParameterValue = Session["LocId"].ToString();

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
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        objReport.DataSource = Dt;
        objReport.DataMember = "Get_Employee_Payment_Voucher_DetailTableAdapter";
        rptViewer.Report = objReport;

        rptToolBar.ReportViewer = rptViewer;




    }
}
