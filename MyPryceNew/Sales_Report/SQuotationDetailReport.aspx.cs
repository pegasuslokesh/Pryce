using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_AdjustHeaderReport : BasePage
{
    SalesQuotationDetailByQuotationDate objReport = null;
    SalesQuotationDetailByQuotationNumber objReportByQuotationNumber = null;
    SalesQuotationDetailByInquiryNumber objReportByInqNo = null;
    SalesQuotationDetailByCustomer objReportByCustomer = null;
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

        objReport = new SalesQuotationDetailByQuotationDate(Session["DBConnection"].ToString());
        objReportByQuotationNumber = new SalesQuotationDetailByQuotationNumber(Session["DBConnection"].ToString());
        objReportByInqNo = new SalesQuotationDetailByInquiryNumber(Session["DBConnection"].ToString());
        objReportByCustomer = new SalesQuotationDetailByCustomer(Session["DBConnection"].ToString());
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
        Session["AccordianId"] = "30";
        Session["HeaderText"] = "Sales Report";
            }
    void GetReport()
    {
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
            if(Session["lang"].ToString()=="1")
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
        Title = "Sales Quotation Report";
        if (Session["ReportType"].ToString() == "0")
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
            objReport.setUserName(Session["UserId"].ToString());

            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "1")
        {
            objReportByQuotationNumber.setcompanyAddress(CompanyAddress);
            objReportByQuotationNumber.setcompanyname(CompanyName);
            objReportByQuotationNumber.setBrandName(BrandName);
            objReportByQuotationNumber.setLocationName(LocationName);
            objReportByQuotationNumber.settitle(Session["ReportHeader"].ToString());
            objReportByQuotationNumber.SetImage(Imageurl);
            objReportByQuotationNumber.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByQuotationNumber.SetDateCriteria("");
            }
            else
            {
                objReportByQuotationNumber.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByQuotationNumber.setUserName(Session["UserId"].ToString());

            objReportByQuotationNumber.DataSource = Dt;
            objReportByQuotationNumber.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
            rptViewer.Report = objReportByQuotationNumber;
            objReportByQuotationNumber.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "2")
        {
            objReportByInqNo.setcompanyAddress(CompanyAddress);
            objReportByInqNo.setcompanyname(CompanyName);
            objReportByInqNo.setBrandName(BrandName);
            objReportByInqNo.setLocationName(LocationName);
            objReportByInqNo.settitle(Session["ReportHeader"].ToString());
            objReportByInqNo.SetImage(Imageurl);
            objReportByInqNo.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByInqNo.SetDateCriteria("");
            }
            else
            {
                objReportByInqNo.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByInqNo.setUserName(Session["UserId"].ToString());

            objReportByInqNo.DataSource = Dt;
            objReportByInqNo.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
            rptViewer.Report = objReportByInqNo;
            objReportByInqNo.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "3")
        {
            objReportByCustomer.setcompanyAddress(CompanyAddress);
            objReportByCustomer.setcompanyname(CompanyName);
            objReportByCustomer.setBrandName(BrandName);
            objReportByCustomer.setLocationName(LocationName);
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
            objReportByCustomer.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
            rptViewer.Report = objReportByCustomer;
            objReportByCustomer.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
  
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;
       
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=SQ&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
