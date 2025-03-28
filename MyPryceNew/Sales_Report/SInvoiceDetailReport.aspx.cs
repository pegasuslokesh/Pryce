using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_AdjustHeaderReport : BasePage
{
    Salesinvoicedetail_By_InvoiceNo objReport = null;
    Salesinvoicedetail_By_InvoiceDate objReportbyInvoiceDate = null;
    Salesinvoicedetail_By_Customer objReportbyCustomer = null;
    Inv_ParameterMaster objInvParam = null;
    SystemParameter ObjSysParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new Salesinvoicedetail_By_InvoiceNo(Session["DBConnection"].ToString());
        objReportbyInvoiceDate = new Salesinvoicedetail_By_InvoiceDate(Session["DBConnection"].ToString());
        objReportbyCustomer = new Salesinvoicedetail_By_Customer(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
        AllPageCode();
    }
    public void AllPageCode()
    {

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "30";
        Session["HeaderText"] = "Sales Report";
    }
    void GetReport()
    {
        lblHeader.Text = Session["ReportHeader"].ToString();
        DataTable Dt = new DataTable();

        if (Session["DtFilter"] != null)
        {
            Dt = (DataTable)Session["DtFilter"];
        }

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string FromDate = "";
        string Todate = "";
        string Title = "";

        string ParameterType = string.Empty;
        string ParameterValue = string.Empty;
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Company Level")
        {
            ParameterType = "Company";
            ParameterValue = Session["CompId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Brand Level")
        {
            ParameterType = "Brand";
            ParameterValue = Session["BrandId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Location Level")
        {
            ParameterType = "Location";
            ParameterValue = Session["LocId"].ToString();
        }

        string[] strParam = Common.ReportHeaderSetup(ParameterType, ParameterValue, Session["DBConnection"].ToString());


        CompanyName = strParam[0].ToString();
        CompanyAddress = strParam[2].ToString();
        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
       

        if (Session["FromDate"] != null)
        {
            FromDate = Session["FromDate"].ToString();
        }
        if (Session["ToDate"] != null)
        {
            Todate = Session["ToDate"].ToString();
        }
        Title = "Sales Quotation Report";

        if (Session["ReportType"].ToString() == "0")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReportbyInvoiceDate.FindControl("GroupHeader1", true);

            if (Request.QueryString["orderby"].ToString() == "Asc")
            {

                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Invoice_Date", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
              
            
            });
            }
            else
            {


                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Invoice_Date", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
              
            
            });

            }

            objReportbyInvoiceDate.setcompanyAddress(CompanyAddress);
            objReportbyInvoiceDate.setcompanyname(CompanyName);
          
            objReportbyInvoiceDate.settitle(Session["ReportHeader"].ToString());
            objReportbyInvoiceDate.SetImage(Imageurl);
            objReportbyInvoiceDate.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportbyInvoiceDate.SetDateCriteria("");
            }
            else
            {
                objReportbyInvoiceDate.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportbyInvoiceDate.setUserName(Session["UserId"].ToString());

            objReportbyInvoiceDate.DataSource = Dt;
            objReportbyInvoiceDate.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            rptViewer.Report = objReportbyInvoiceDate;
            objReportbyInvoiceDate.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        if (Session["ReportType"].ToString() == "1")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReport.FindControl("GroupHeader1", true);

            if (Request.QueryString["orderby"].ToString() == "Asc")
            {

                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Invoice_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
              
            
            });
            }
            else
            {


                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Invoice_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
              
            
            });

            }
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
         
            objReport.settitle(Session["ReportHeader"].ToString());
            objReport.SetImage(Imageurl);
            objReport.setArabic();
            if (Session["Parameter"] == null)
            {
                objReport.SetDateCriteria("");
            }
            else
            {
                objReport.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReport.setUserName(Session["UserId"].ToString());

            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        
        if (Session["ReportType"].ToString() == "2")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReportbyCustomer.FindControl("GroupHeader1", true);

            if (Request.QueryString["orderby"].ToString() == "Asc")
            {

                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Supplier_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
              
            
            });
            }
            else
            {


                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Supplier_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
              
            
            });

            }
            objReportbyCustomer.setcompanyAddress(CompanyAddress);
            objReportbyCustomer.setcompanyname(CompanyName);
            
            objReportbyCustomer.settitle(Session["ReportHeader"].ToString());
            objReportbyCustomer.SetImage(Imageurl);
            objReportbyCustomer.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportbyCustomer.SetDateCriteria("");
            }
            else
            {
                objReportbyCustomer.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportbyCustomer.setUserName(Session["UserId"].ToString());

            objReportbyCustomer.DataSource = Dt;
            objReportbyCustomer.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            rptViewer.Report = objReportbyCustomer;
            objReportbyCustomer.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
      
            
        }
       

        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=SI&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
