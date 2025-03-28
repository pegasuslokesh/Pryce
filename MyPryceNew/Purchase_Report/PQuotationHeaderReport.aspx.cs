using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_AdjustHeaderReport : BasePage
{
    PQuotationHeader objReport = null;
    PQuotationHeaderByInquiryNo objReportByInquiryNo = null;
    PQuotationHeaderByQuotationNo objReportByQuotationNo = null;
    PQuotationHeaderByQuotationDate objReportByQuotationDate = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new PQuotationHeader(Session["DBConnection"].ToString());
        objReportByInquiryNo = new PQuotationHeaderByInquiryNo(Session["DBConnection"].ToString());
        objReportByQuotationNo = new PQuotationHeaderByQuotationNo(Session["DBConnection"].ToString());
        objReportByQuotationDate = new PQuotationHeaderByQuotationDate(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
        AllPageCode();
    }
    public void AllPageCode()
    {
        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "146";
        Session["HeaderText"] = "Purchase Report";
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
        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();
            }
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            }
            else
            {
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
           
            }
        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            if (DtAddress.Rows[0]["Address"].ToString() != "")
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            if (DtAddress.Rows[0]["Street"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Block"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
                }

            }
            if (DtAddress.Rows[0]["Avenue"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
                }
            }

            if (DtAddress.Rows[0]["CityId"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["StateId"].ToString() != "")
            {


                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["CountryId"].ToString() != "")
            {

                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + LocationName;
                }
                else
                {
                    CompanyAddress = LocationName;
                }

            }
            if (DtAddress.Rows[0]["PinCode"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
                }

            }


        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            }
            else
            {
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
            }
        }
      


        if (Session["FromDate"] != null)
        {
            FromDate = Session["FromDate"].ToString();
        }
        if (Session["ToDate"] != null)
        {
            Todate = Session["ToDate"].ToString();
        }
        
         
        if ( Session["ReportType"].ToString() == "0")
        {
           
            objReportByInquiryNo.setcompanyAddress(CompanyAddress);
            objReportByInquiryNo.setcompanyname(CompanyName);
            objReportByInquiryNo.setBrandName(BrandName);
            objReportByInquiryNo.setLocationName(LocationName);
            objReportByInquiryNo.settitle(Session["ReportHeader"].ToString());
            objReportByInquiryNo.SetImage(Imageurl);
            objReportByInquiryNo.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByInquiryNo.SetDateCriteria("");
            }
            else
            {
                objReportByInquiryNo.SetDateCriteria(Session["Parameter"].ToString());
            }
            //if (Session["TotalAmount"] == null)
            //{
            //}
            //else
            //{
            //    objReport.setTotalAmount(Session["TotalAmount"].ToString());
            //}
            objReportByInquiryNo.setUserName(Session["UserId"].ToString());

            //objReport.DataSource = Dt.DefaultView.ToTable(true,"RPQ_No","Trans_Id","CreatedDate","RPQ_Date","PI_No","TotalAmount","Status","Supplier_Id","Supplier_Name","Inquiry_No","Inquiry_Date","Amount"); 
            objReportByInquiryNo.DataSource = Dt;
            objReportByInquiryNo.DataMember = "sp_Inv_PurchaseQuoteHeader_SelectRow_Report";
            rptViewer.Report = objReportByInquiryNo;
            objReportByInquiryNo.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
  
        }
        if (Session["ReportType"].ToString() == "1")
        {
            objReportByQuotationNo.setcompanyAddress(CompanyAddress);
            objReportByQuotationNo.setcompanyname(CompanyName);
            objReportByQuotationNo.setBrandName(BrandName);
            objReportByQuotationNo.setLocationName(LocationName);
            objReportByQuotationNo.settitle(Session["ReportHeader"].ToString());
            objReportByQuotationNo.SetImage(Imageurl);
            objReportByQuotationNo.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByQuotationNo.SetDateCriteria("");
            }
            else
            {
                objReportByQuotationNo.SetDateCriteria(Session["Parameter"].ToString());
            }
            //if (Session["TotalAmount"] == null)
            //{
            //}
            //else
            //{
            //    objReport.setTotalAmount(Session["TotalAmount"].ToString());
            //}
            objReportByQuotationNo.setUserName(Session["UserId"].ToString());

            //objReport.DataSource = Dt.DefaultView.ToTable(true,"RPQ_No","Trans_Id","CreatedDate","RPQ_Date","PI_No","TotalAmount","Status","Supplier_Id","Supplier_Name","Inquiry_No","Inquiry_Date","Amount"); 
            objReportByQuotationNo.DataSource = Dt;
            objReportByQuotationNo.DataMember = "sp_Inv_PurchaseQuoteHeader_SelectRow_Report";
            rptViewer.Report = objReportByQuotationNo;
            objReportByQuotationNo.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
  
        }
             if ( Session["ReportType"].ToString() == "2")
       
        {

            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.setBrandName(BrandName);
            objReport.setLocationName(LocationName);
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
            //if (Session["TotalAmount"] == null)
            //{
            //}
            //else
            //{
            //    objReport.setTotalAmount(Session["TotalAmount"].ToString());
            //}
            objReport.setUserName(Session["UserId"].ToString());

            //objReport.DataSource = Dt.DefaultView.ToTable(true,"RPQ_No","Trans_Id","CreatedDate","RPQ_Date","PI_No","TotalAmount","Status","Supplier_Id","Supplier_Name","Inquiry_No","Inquiry_Date","Amount"); 
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_PurchaseQuoteHeader_SelectRow_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
  
        }
             if (Session["ReportType"].ToString() == "3")
             {
                 
                 objReportByQuotationDate.setcompanyAddress(CompanyAddress);
                 objReportByQuotationDate.setcompanyname(CompanyName);
                 objReportByQuotationDate.setBrandName(BrandName);
                 objReportByQuotationDate.setLocationName(LocationName);
                 objReportByQuotationDate.settitle(Session["ReportHeader"].ToString());
                 objReportByQuotationDate.SetImage(Imageurl);
                 objReportByQuotationDate.setArabic();
                 if (Session["Parameter"] == null)
                 {
                     objReportByQuotationDate.SetDateCriteria("");
                 }
                 else
                 {
                     objReportByQuotationDate.SetDateCriteria(Session["Parameter"].ToString());
                 }
                 //if (Session["TotalAmount"] == null)
                 //{
                 //}
                 //else
                 //{
                 //    objReport.setTotalAmount(Session["TotalAmount"].ToString());
                 //}
                 objReportByQuotationDate.setUserName(Session["UserId"].ToString());

                 //objReport.DataSource = Dt.DefaultView.ToTable(true,"RPQ_No","Trans_Id","CreatedDate","RPQ_Date","PI_No","TotalAmount","Status","Supplier_Id","Supplier_Name","Inquiry_No","Inquiry_Date","Amount"); 
                 objReportByQuotationDate.DataSource = Dt;
                 objReportByQuotationDate.DataMember = "sp_Inv_PurchaseQuoteHeader_SelectRow_Report";
                 rptViewer.Report = objReportByQuotationDate;
                 objReportByQuotationDate.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
             }
             ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;

       

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=PQ&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
