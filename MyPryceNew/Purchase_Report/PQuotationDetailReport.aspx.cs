using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_AdjustHeaderReport : BasePage
{
    
    PQuotationDetailByRPQNo objReportByRPQNO = null;
    PQuotationDetailByRPQDate objReportByRPQDate = null;
    PQuotationDetailBySupplier objReportBySupplier = null;
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

        objReportByRPQNO = new PQuotationDetailByRPQNo(Session["DBConnection"].ToString());
        objReportByRPQDate = new PQuotationDetailByRPQDate(Session["DBConnection"].ToString());
        objReportBySupplier = new PQuotationDetailBySupplier(Session["DBConnection"].ToString());
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
        Title = "Purchase Quotation Report";
        if (Session["ReportType"].ToString() == "0")
        {
            objReportByRPQNO.setcompanyAddress(CompanyAddress);
            objReportByRPQNO.setcompanyname(CompanyName);
            objReportByRPQNO.setBrandName(BrandName);
            objReportByRPQNO.setLocationName(LocationName);
            objReportByRPQNO.settitle(Session["ReportHeader"].ToString());
            objReportByRPQNO.SetImage(Imageurl);
            objReportByRPQNO.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByRPQNO.SetDateCriteria("");
            }
            else
            {
                objReportByRPQNO.SetDateCriteria(Session["Parameter"].ToString());
            }
            if (Session["TotalAmount"] == null)
            {
            }
            else
            {
                objReportByRPQNO.setTotalAmount(Session["TotalAmount"].ToString());
            }
            objReportByRPQNO.setUserName(Session["UserId"].ToString());

            objReportByRPQNO.DataSource = Dt;
            objReportByRPQNO.DataMember = "sp_Inv_PurchaseQuoteDetail_SelectRow_Report";
            rptViewer.Report = objReportByRPQNO;
            objReportByRPQNO.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        if (Session["ReportType"].ToString() == "1")
        {
            objReportByRPQDate.setcompanyAddress(CompanyAddress);
            objReportByRPQDate.setcompanyname(CompanyName);
            objReportByRPQDate.setBrandName(BrandName);
            objReportByRPQDate.setLocationName(LocationName);
            objReportByRPQDate.settitle(Session["ReportHeader"].ToString());
            objReportByRPQDate.SetImage(Imageurl);
            objReportByRPQDate.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByRPQDate.SetDateCriteria("");
            }
            else
            {
                objReportByRPQDate.SetDateCriteria(Session["Parameter"].ToString());
            }
            if (Session["TotalAmount"] == null)
            {
            }
            else
            {
                objReportByRPQDate.setTotalAmount(Session["TotalAmount"].ToString());
            }
            objReportByRPQDate.setUserName(Session["UserId"].ToString());

            objReportByRPQDate.DataSource = Dt;
            objReportByRPQDate.DataMember = "sp_Inv_PurchaseQuoteDetail_SelectRow_Report";
            rptViewer.Report = objReportByRPQDate;
            objReportByRPQDate.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        if (Session["ReportType"].ToString() == "2")
        {
            objReportBySupplier.setcompanyAddress(CompanyAddress);
            objReportBySupplier.setcompanyname(CompanyName);
            objReportBySupplier.setBrandName(BrandName);
            objReportBySupplier.setLocationName(LocationName);
            objReportBySupplier.settitle(Session["ReportHeader"].ToString());
            objReportBySupplier.SetImage(Imageurl);
            objReportBySupplier.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportBySupplier.SetDateCriteria("");
            }
            else
            {
                objReportBySupplier.SetDateCriteria(Session["Parameter"].ToString());
            }
            if (Session["TotalAmount"] == null)
            {
            }
            else
            {
                objReportBySupplier.setTotalAmount(Session["TotalAmount"].ToString());
            }
            objReportBySupplier.setUserName(Session["UserId"].ToString());

            objReportBySupplier.DataSource = Dt;
            objReportBySupplier.DataMember = "sp_Inv_PurchaseQuoteDetail_SelectRow_Report";
            rptViewer.Report = objReportBySupplier;
            objReportBySupplier.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;
       

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=PQ&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
}
