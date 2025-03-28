using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_DailyCashFlowPrint : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    AccountStatementPrint objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objsys = null;
    DailyCashFlowPrint objReport = null;

    string strCompId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new AccountStatementPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objReport = new DailyCashFlowPrint(Session["DBConnection"].ToString());

        GetReport();

    }


    void GetReport()
    {
        DataTable Dt = new DataTable();
        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;

        AccountsDatasetTableAdapters.sp_Ac_CashFlow_Header_SelectRowTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_CashFlow_Header_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        string strLocationId = Session["LocId"].ToString();
        try
        {
            strLocationId = Request.QueryString["LocId"].ToString();
        }
        catch
        {

        }

        adp.Fill(ObjAccountDataset.sp_Ac_CashFlow_Header_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["Id"].ToString()), null, false, 3);
        Dt = ObjAccountDataset.sp_Ac_CashFlow_Header_SelectRow;

        if (Dt.Rows.Count > 0)
        {

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
            string[] strParam = Common.ReportHeaderSetup("Location", strLocationId, Session["DBConnection"].ToString());
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

            EmployeeMaster ObjEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
            string createdby = string.Empty;
            if (Session["EmpId"].ToString().Trim() == "0")
            {
                createdby = "SuperAdmin";
            }
            else
            {
                try
                {
                    createdby = ObjEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString();
                }
                catch
                {
                }
            }

            DetailReportBand ObjVoucherDetail = (DetailReportBand)objReport.FindControl("DetailReport", true);
            DetailReportBand ObjAccountSummary = (DetailReportBand)objReport.FindControl("DetailReport1", true);
            DetailReportBand objCustomerCheque = (DetailReportBand)objReport.FindControl("DetailReport2", true);
            DetailReportBand objSupplierCheque = (DetailReportBand)objReport.FindControl("DetailReport3", true);





            //for Voucher detail


            AccountsDatasetTableAdapters.sp_Ac_CashFlow_VoucherDetail_ReportTableAdapter adp1 = new AccountsDatasetTableAdapters.sp_Ac_CashFlow_VoucherDetail_ReportTableAdapter();
            adp1.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp1.Fill(ObjAccountDataset.sp_Ac_CashFlow_VoucherDetail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            DataTable DtVoucherDetail = ObjAccountDataset.sp_Ac_CashFlow_VoucherDetail_Report;


            ObjVoucherDetail.DataSource = DtVoucherDetail;
            ObjVoucherDetail.DataMember = "sp_Ac_CashFlow_VoucherDetail_Report";

            //for accountsummary

            AccountsDatasetTableAdapters.sp_Ac_CashFlow_AccountSummarized_ReportTableAdapter adp2 = new AccountsDatasetTableAdapters.sp_Ac_CashFlow_AccountSummarized_ReportTableAdapter();
            adp2.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp2.Fill(ObjAccountDataset.sp_Ac_CashFlow_AccountSummarized_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            DataTable DtAccountSummary = ObjAccountDataset.sp_Ac_CashFlow_AccountSummarized_Report;



            ObjAccountSummary.DataSource = DtAccountSummary;
            ObjAccountSummary.DataMember = "sp_Ac_CashFlow_AccountSummarized_Report";

            //for customer cheque details


            AccountsDatasetTableAdapters.sp_Ac_CashFlow_ChequeDetails_ReportTableAdapter adp3 = new AccountsDatasetTableAdapters.sp_Ac_CashFlow_ChequeDetails_ReportTableAdapter();
            adp3.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp3.Fill(ObjAccountDataset.sp_Ac_CashFlow_ChequeDetails_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            DataTable Dtcheqedetails = ObjAccountDataset.sp_Ac_CashFlow_ChequeDetails_Report;

            if (new DataView(Dtcheqedetails, "Type='CusCheque'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            {
                objCustomerCheque.Visible = false;
            }
            else
            {
                objCustomerCheque.Visible = true;
            }


            objCustomerCheque.DataSource = new DataView(Dtcheqedetails, "Type='CusCheque'", "", DataViewRowState.CurrentRows).ToTable();
            objCustomerCheque.DataMember = "sp_Ac_CashFlow_ChequeDetails_Report";

            //for supplier cheque details
            if (new DataView(Dtcheqedetails, "Type='SupCheque'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            {
                objSupplierCheque.Visible = false;
            }
            else
            {
                objSupplierCheque.Visible = true;
            }



            objSupplierCheque.DataSource = new DataView(Dtcheqedetails, "Type='SupCheque'", "", DataViewRowState.CurrentRows).ToTable();
            objSupplierCheque.DataMember = "sp_Ac_CashFlow_ChequeDetails_Report";


            objReport.setSignature("");

            objReport.setcompanyname(CompanyName);
            objReport.SetImage(Imageurl);
            objReport.setUserName(createdby);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Ac_CashFlow_Header_SelectRow";
            rptViewer.Report = objReport;
            ReportToolbar1.ReportViewer = rptViewer;


        }
    }

}