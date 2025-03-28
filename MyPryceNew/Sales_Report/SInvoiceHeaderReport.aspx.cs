using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_AdjustHeaderReport : BasePage
{
    SalesInvoiceHeaderByInvoiceDate objReport = null;
    SalesInvoiceVatReport objVatReport = null;
    SalesInvoiceHeaderByCustomer objReportByCustomer = null;
    SalesInvoiceHeaderBySalesPerson objReportBySalesperson = null;
    Inv_ParameterMaster objInvParam = null;
    SystemParameter ObjSysParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new SalesInvoiceHeaderByInvoiceDate(Session["DBConnection"].ToString());
        objVatReport = new SalesInvoiceVatReport(Session["DBConnection"].ToString());
        objReportByCustomer = new SalesInvoiceHeaderByCustomer(Session["DBConnection"].ToString());
        objReportBySalesperson = new SalesInvoiceHeaderBySalesPerson(Session["DBConnection"].ToString());
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
        Title = "Sales Order Report";

        if (Session["ReportType"].ToString() == "0")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReport.FindControl("GroupHeader1", true);
            DevExpress.XtraReports.UI.GroupFooterBand GF = (DevExpress.XtraReports.UI.GroupFooterBand)objReport.FindControl("GroupFooter1", true);
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
            if (!Convert.ToBoolean(Request.QueryString["IsGroup"].ToString()))
            {
                Gh.Visible = false;
                GF.Visible = false;
                Gh.GroupFields.RemoveAt(0);
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
            if(Session["LocId"].ToString() == "7")
            {
                for (int count = 0; count < Dt.Rows.Count; count++)
                {
                    Dt.Rows[count]["GrandTotal"] = Math.Round(Convert.ToDouble(Dt.Rows[count]["GrandTotal"]), 0).ToString();
                }
            }

            Dt.AcceptChanges();
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_SalesInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
          
            

        }
        if (Session["ReportType"].ToString() == "1")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReportByCustomer.FindControl("GroupHeader1", true);
            DevExpress.XtraReports.UI.GroupFooterBand GF = (DevExpress.XtraReports.UI.GroupFooterBand)objReportByCustomer.FindControl("GroupFooter1", true);


            if (Request.QueryString["orderby"].ToString() == "Asc")
            {

                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Customer_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
            
            });
            }
            else
            {
                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("Customer_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
            
            });
               

            }
            if (!Convert.ToBoolean(Request.QueryString["IsGroup"].ToString()))
            {
                Gh.Visible = false;
                GF.Visible = false;
                Gh.GroupFields.RemoveAt(0);
            }
          
            objReportByCustomer.setcompanyAddress(CompanyAddress);
            objReportByCustomer.setcompanyname(CompanyName);
          
            objReportByCustomer.settitle(Session["ReportHeader"].ToString());
            objReportByCustomer.SetImage(Imageurl);
            objReportByCustomer.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByCustomer.SetDateCriteria("");
            }
            else
            {
                objReportByCustomer.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByCustomer.setUserName(Session["UserId"].ToString());

            objReportByCustomer.DataSource = Dt;
            objReportByCustomer.DataMember = "sp_Inv_SalesInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReportByCustomer;
            objReportByCustomer.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "2")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReportBySalesperson.FindControl("GroupHeader1", true);
            DevExpress.XtraReports.UI.GroupFooterBand GF = (DevExpress.XtraReports.UI.GroupFooterBand)objReportBySalesperson.FindControl("GroupFooter1", true);


            if (Request.QueryString["orderby"].ToString() == "Asc")
            {

                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("SalesPersonName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
            
            });
            }
            else
            {
                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("SalesPersonName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
            
            });

               
            }
            if (!Convert.ToBoolean(Request.QueryString["IsGroup"].ToString()))
            {
                Gh.Visible = false;
                GF.Visible = false;
                Gh.GroupFields.RemoveAt(0);
            }
           
            objReportBySalesperson.setcompanyAddress(CompanyAddress);
            objReportBySalesperson.setcompanyname(CompanyName);
           
            objReportBySalesperson.settitle(Session["ReportHeader"].ToString());
            objReportBySalesperson.SetImage(Imageurl);
            objReportBySalesperson.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportBySalesperson.SetDateCriteria("");
            }
            else
            {
                objReportBySalesperson.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportBySalesperson.setUserName(Session["UserId"].ToString());

            objReportBySalesperson.DataSource = Dt;
            objReportBySalesperson.DataMember = "sp_Inv_SalesInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReportBySalesperson;
            objReportBySalesperson.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "3")
        {
            DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objVatReport.FindControl("GroupHeader1", true);
            DevExpress.XtraReports.UI.GroupFooterBand GF = (DevExpress.XtraReports.UI.GroupFooterBand)objVatReport.FindControl("GroupFooter1", true);


            if (Request.QueryString["orderby"].ToString() == "Asc")
            {
            
                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("InvoiceMerchant", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)
            
            });
            }
            else
            {
                Gh.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
               
            new DevExpress.XtraReports.UI.GroupField("InvoiceMerchant", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)
            
            });

            }

            if (!Convert.ToBoolean(Request.QueryString["IsGroup"].ToString()))
            {
                Gh.Visible = false;
                GF.Visible = false;
                Gh.GroupFields.RemoveAt(0);
            }
            try
            {
                Dt = new DataView(Dt, "VatValue>0", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
          
            objVatReport.setcompanyAddress(CompanyAddress);
            objVatReport.setcompanyname(CompanyName);

            objVatReport.settitle(Session["ReportHeader"].ToString());
            objVatReport.SetImage(Imageurl);
            //objVatReport.setArabic();
            if (Session["Parameter"] == null)
            {
                objVatReport.SetDateCriteria("");
            }
            else
            {
                objVatReport.SetDateCriteria(Session["Parameter"].ToString());
            }
            objVatReport.setUserName(Session["UserId"].ToString());

            objVatReport.DataSource = Dt;
            objVatReport.DataMember = "sp_Inv_SalesInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objVatReport;
            objVatReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=SO&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
