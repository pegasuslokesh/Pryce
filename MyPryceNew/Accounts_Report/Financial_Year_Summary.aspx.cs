using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Accounts_Report_Financial_Year_Summary : System.Web.UI.Page
{
    FinancialYearReport objReport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Ac_Finance_Year_Info objFYI = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new FinancialYearReport(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            FillFinanceYear();
        }
        GetReport();

    }

    public void FillFinanceYear()
    {
        ddlFinanceYear.Items.Clear();


        DataTable dt = objFYI.GetInfoAllTrue(Session["CompId"].ToString());

        dt = new DataView(dt, "Status<>'New'", "Trans_id", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ddlFinanceYear.DataSource = dt;
            ddlFinanceYear.DataTextField = "finance_code";
            ddlFinanceYear.DataValueField = "trans_id";
            ddlFinanceYear.DataBind();


        }

        ddlFinanceYear.Items.Insert(0, "--Select--");

    }

    void GetReport()
    {
        DataTable Dt = new DataTable();





        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        AccountsDatasetTableAdapters.sp_Ac_FinancialYear_ReportTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_FinancialYear_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjAccountDataset.sp_Ac_FinancialYear_Report);
        Dt = ObjAccountDataset.sp_Ac_FinancialYear_Report;


        if (ddlFinanceYear.SelectedIndex > 0)
        {
            Dt = new DataView(Dt, "Trans_Id=" + ddlFinanceYear.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
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
        ParameterValue = Session["CompId"].ToString();
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
            objReport.setUserName(dtemployee.Rows[0]["Emp_Name"].ToString());
        }
        else
        {
            objReport.setUserName("Superadmin");
        }







        objReport.setReportTitle("FINANCIAL YEAR SUMMARY");
        objReport.setSignature(signatureurl);
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName.ToUpper());
        objReport.SetImage(Imageurl);
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        //objReport.setDateCriteria(Session["DtCommissionReport_Range"].ToString());
        objReport.DataSource = Dt;
        objReport.DataMember = "sp_Ac_FinancialYear_Report";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        GetReport();
    }
}