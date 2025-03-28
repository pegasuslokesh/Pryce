using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Inventory_Report_Purchase_Order_Report : BasePage
{
    PInvoiceDetail objReport = null;
    PInvoiceDetailByInvoiceNo objReportByInvoiceNo = null;
    PInvoiceDetailBySupplier objReportBySupplier = null;

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

        objReport = new PInvoiceDetail(Session["DBConnection"].ToString());
        objReportByInvoiceNo = new PInvoiceDetailByInvoiceNo(Session["DBConnection"].ToString());
        objReportBySupplier = new PInvoiceDetailBySupplier(Session["DBConnection"].ToString());

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
      
       
          
       
        Title="Purchase Invoice Report";

        if (Session["ReportType"].ToString() == "0")
        {
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.setBrandName(BrandName);
            objReport.setLocationName(LocationName);
            objReport.settitle(Session["ReportHeader"].ToString());
            objReport.SetImage(Imageurl);
            if (Session["Parameter"] != null)
            {
                objReport.SetDateCriteria(Session["Parameter"].ToString());
            }
            else
            {
                objReport.SetDateCriteria("");
            }
            objReport.setUserName(Session["UserId"].ToString());
            objReport.setArabic();


            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_PurchaseInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
      
        }
        if (Session["ReportType"].ToString() == "1")
        {
            objReportBySupplier.setcompanyAddress(CompanyAddress);
            objReportBySupplier.setcompanyname(CompanyName);
            objReportBySupplier.setBrandName(BrandName);
            objReportBySupplier.setLocationName(LocationName);
            objReportBySupplier.settitle(Session["ReportHeader"].ToString());
            objReportBySupplier.SetImage(Imageurl);
            objReportBySupplier.setArabic();
            if (Session["Parameter"] != null)
            {
                objReportBySupplier.SetDateCriteria(Session["Parameter"].ToString());
            }
            else
            {
                objReportBySupplier.SetDateCriteria("");
            }
            objReportBySupplier.setUserName(Session["UserId"].ToString());


            objReportBySupplier.DataSource = Dt;
            objReportBySupplier.DataMember = "sp_Inv_PurchaseInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReportBySupplier;
            objReportBySupplier.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
      
        }
        if (Session["ReportType"].ToString() == "2")
        {
            objReportByInvoiceNo.setcompanyAddress(CompanyAddress);
            objReportByInvoiceNo.setcompanyname(CompanyName);
            objReportByInvoiceNo.setBrandName(BrandName);
            objReportByInvoiceNo.setLocationName(LocationName);
            objReportByInvoiceNo.settitle(Session["ReportHeader"].ToString());
            objReportByInvoiceNo.SetImage(Imageurl);
            objReportByInvoiceNo.setArabic();
            if (Session["Parameter"] != null)
            {
                objReportByInvoiceNo.SetDateCriteria(Session["Parameter"].ToString());
            }
            else
            {
                objReportByInvoiceNo.SetDateCriteria("");
            }
            objReportByInvoiceNo.setUserName(Session["UserId"].ToString());


            objReportByInvoiceNo.DataSource = Dt;
            objReportByInvoiceNo.DataMember = "sp_Inv_PurchaseInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReportByInvoiceNo;
            objReportByInvoiceNo.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
           
        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=PI&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
