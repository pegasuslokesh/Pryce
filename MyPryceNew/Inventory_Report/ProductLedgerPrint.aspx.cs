using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_ProductLedgerPrint : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    Inv_StockDetail objStockDetail = null;
    ProductLedgerPrint objProductLedgerPrint = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = new InventoryDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objProductLedgerPrint = new ProductLedgerPrint(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "145";
        Session["HeaderText"] = "Inventory Report";



    }

    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        ObjInvdataset.EnforceConstraints = false;
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string CompanyName_L = string.Empty;
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;



        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            CompanyName_L = DtCompany.Rows[0]["Company_Name_L"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
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
            Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();

            if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            {

                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
            if (DtAddress.Rows[0]["WebSite"].ToString() != "")
            {
                CompanyWebsite = DtAddress.Rows[0]["WebSite"].ToString();
            }



        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
        }
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


            signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field2"].ToString();

        }





        try
        {

            DataTable dt = new DataTable();
            InventoryDataSetTableAdapters.sp_Inv_ProductLedger_SelectRow_ReportTableAdapter ObjPrintAd = new InventoryDataSetTableAdapters.sp_Inv_ProductLedger_SelectRow_ReportTableAdapter();
            ObjPrintAd.Connection.ConnectionString = Session["DBConnection"].ToString();
            ObjPrintAd.Fill(ObjInvdataset.sp_Inv_ProductLedger_SelectRow_Report, Convert.ToInt32(strCompId.ToString()), Convert.ToInt32(strBrandId.ToString()));
            dt = ObjInvdataset.sp_Inv_ProductLedger_SelectRow_Report;

            string Trans_Id = string.Empty;
            DataTable dtProduct = (DataTable)Session["DtProductLedger"];

            foreach (DataRow dr in dtProduct.Rows)
            {
                if (Trans_Id == "")
                {
                    Trans_Id = dr["Trans_Id"].ToString();
                }
                else
                {
                    Trans_Id = Trans_Id + "," + dr["Trans_Id"].ToString();
                }

            }
            if (Trans_Id != "")
            {
                dt = new DataView(dt, "Trans_Id in (" + Trans_Id + ")", "Trans_Id", DataViewRowState.CurrentRows).ToTable();



                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    dt.Rows[j]["UnitPrice"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), dt.Rows[j]["Location_Id"].ToString(), Session["FinanceYearId"].ToString(), dt.Rows[j]["ProductId"].ToString()).Rows[0]["Field2"].ToString();

                }

                objProductLedgerPrint.DataSource = dt;
                objProductLedgerPrint.DataMember = "sp_Inv_ProductLedger_SelectRow_Report";
                rptViewer.Report = objProductLedgerPrint;
                rptToolBar.ReportViewer = rptViewer;
                objProductLedgerPrint.setcompanyname(CompanyName);
                objProductLedgerPrint.setSignature(signatureurl);
                objProductLedgerPrint.setCompanyArebicName(CompanyName_L);
                objProductLedgerPrint.setCompanyTelNo(Companytelno);
                objProductLedgerPrint.setCompanyFaxNo(CompanyFaxno);
                objProductLedgerPrint.setCompanyWebsite(CompanyWebsite);
                objProductLedgerPrint.setcompanyAddress(CompanyAddress);
                objProductLedgerPrint.SetImage(Imageurl);
                objProductLedgerPrint.ExportToPdf(Server.MapPath("~/Temp/Product Ledger -" + dt.Rows[0]["Trans_Id"].ToString() + ".pdf"));
                ViewState["Path"] = "Product Ledger -" + dt.Rows[0]["Trans_Id"].ToString() + ".pdf";
            }

        }
        catch
        {

        }
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {

        Response.Redirect("../EmailSystem/SendMail.aspx?Page=PS&&URL=" + ViewState["Path"].ToString() + "'");


    }
}
