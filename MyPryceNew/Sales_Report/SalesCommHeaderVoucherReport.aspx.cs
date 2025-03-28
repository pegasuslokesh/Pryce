using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;


public partial class Sales_Report_SalesCommHeaderVoucherReport : System.Web.UI.Page
{

    SalesCommissionHeaderVoucherReport objHeaderVoucherReport = null;
    SalesCommissionHeaderInvoiceReport objHeaderInvoiceReport = null;
    SalesCommissionDetailCustomerReport objDetailCustomerReport = null;
    SalesCommissionDetailVoucherReport objDetailVoucherReport = null;
    SalesCommissionDetailInvoiceReport objDetailInvoiceReport = null;
    SalesCommissionSummaryReport objSummaryReport = null;
    EmployeeMaster objEmployee = null;

    
  
    string[] strParam;
    string CompanyName = string.Empty;
    string Imageurl = string.Empty;
    string strLoginUserName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        objHeaderVoucherReport = new SalesCommissionHeaderVoucherReport(Session["DBConnection"].ToString());
        objHeaderInvoiceReport = new SalesCommissionHeaderInvoiceReport(Session["DBConnection"].ToString());
        objDetailCustomerReport = new SalesCommissionDetailCustomerReport(Session["DBConnection"].ToString());
        objDetailVoucherReport = new SalesCommissionDetailVoucherReport(Session["DBConnection"].ToString());
        objDetailInvoiceReport = new SalesCommissionDetailInvoiceReport(Session["DBConnection"].ToString());
        objSummaryReport = new SalesCommissionSummaryReport(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

        try
        {
            strLoginUserName = objEmployee.GetEmployeeMasterById(Session["compId"].ToString(), Session["empId"].ToString()).Rows[0]["Emp_Name"].ToString();
        }
        catch
        {
            strLoginUserName = "Superadmin";
        }



        if (Request.QueryString["ReportType"].ToString().Trim() == "HV")
        {
            lblHeader.Text = "Header Report by Voucher Wise";
            GetVoucherHeaderReport();
        }
        else if (Request.QueryString["ReportType"].ToString().Trim()== "HI")
        {
            lblHeader.Text = "Header Report by Invoice Wise";
            GetInvoiceHeaderReport();
        }
        else if (Request.QueryString["ReportType"].ToString().Trim() == "DC")
        {
            lblHeader.Text = "Detail Report by Customer Wise";
            GetCustomerDetailReport();
        }
        else if (Request.QueryString["ReportType"].ToString().Trim() == "DV")
        {
            lblHeader.Text = "Detail Report by Voucher Wise";
            GetVoucherDetailReport();
        }
        else if (Request.QueryString["ReportType"].ToString().Trim() == "DI")
        {
            lblHeader.Text = "Detail Report by Invoice Wise";
            GetInvoiceDetailReport();
        }
        else if (Request.QueryString["ReportType"].ToString().Trim() == "SR")
        {
            lblHeader.Text = "Sales Commission Summary Report";
            GetSummaryReport();
        }
    }

    public void GetVoucherHeaderReport()
    {


        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();

        objHeaderVoucherReport.DataSource = (DataTable)objarr[0];
        
        objHeaderVoucherReport.DataMember = "sp_Inv_SalesCommission_Header_VoucherReport";
        ReportViewer1.Report = objHeaderVoucherReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objHeaderVoucherReport.setcompanyname(CompanyName);
        objHeaderVoucherReport.SetImage(Imageurl);
        objHeaderVoucherReport.setDateCriteria(objarr[1].ToString());
        objHeaderVoucherReport.setUserName(strLoginUserName);
    }

    public void GetInvoiceHeaderReport()
    {

        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();

        objHeaderInvoiceReport.DataSource = (DataTable)objarr[0];

        objHeaderInvoiceReport.DataMember = "sp_Inv_SalesCommission_Header_InvoiceReport";
        ReportViewer1.Report = objHeaderInvoiceReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objHeaderInvoiceReport.setcompanyname(CompanyName);
        objHeaderInvoiceReport.SetImage(Imageurl);
        objHeaderInvoiceReport.setDateCriteria(objarr[1].ToString());
        objHeaderInvoiceReport.setUserName(strLoginUserName);
    }

    public void GetCustomerDetailReport()
    {
        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
        objDetailCustomerReport.DataSource = (DataTable)objarr[0];


        objDetailCustomerReport.DataMember = "sp_Inv_SalesCommission_Detail_Report";
        ReportViewer1.Report = objDetailCustomerReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objDetailCustomerReport.setcompanyname(CompanyName);
        objDetailCustomerReport.SetImage(Imageurl);
        objDetailCustomerReport.setDateCriteria(objarr[1].ToString());
        objDetailCustomerReport.setUserName(strLoginUserName);


    }


    public void GetVoucherDetailReport()
    {
        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
        objDetailVoucherReport.DataSource = (DataTable)objarr[0];

        objDetailVoucherReport.DataMember = "sp_Inv_SalesCommission_Detail_Report";
        ReportViewer1.Report = objDetailVoucherReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objDetailVoucherReport.setcompanyname(CompanyName);
        objDetailVoucherReport.SetImage(Imageurl);
        objDetailVoucherReport.setDateCriteria(objarr[1].ToString());
        objDetailVoucherReport.setUserName(strLoginUserName);
    }

    public void GetInvoiceDetailReport()
    {
        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
        objDetailInvoiceReport.DataSource = (DataTable)objarr[0];
        
        
        objDetailInvoiceReport.DataMember = "sp_Inv_SalesCommission_Detail_Report";
        ReportViewer1.Report = objDetailInvoiceReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objDetailInvoiceReport.setcompanyname(CompanyName);
        objDetailInvoiceReport.SetImage(Imageurl);
        objDetailInvoiceReport.setDateCriteria(objarr[1].ToString());
        objDetailInvoiceReport.setUserName(strLoginUserName);

    }

    public void GetSummaryReport()
    {
        ArrayList objarr = (ArrayList)Session["dtCommissionreport"];

        strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        CompanyName = strParam[0].ToString();

        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
        objSummaryReport.DataSource = (DataTable)objarr[0];


        objSummaryReport.DataMember = "sp_Inv_SalesCommission_Summary_Report";
        ReportViewer1.Report = objSummaryReport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        objSummaryReport.setcompanyname(CompanyName);
        objSummaryReport.SetImage(Imageurl);
        objSummaryReport.setDateCriteria(objarr[1].ToString());
        objSummaryReport.setUserName(strLoginUserName);


    }


}