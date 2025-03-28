using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_Product_Report : System.Web.UI.Page
{
    Productreport objReport = null;
    ContactReport_ByGroup objReport_ByGroup = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    Ems_GroupMaster ObjGroupMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new Productreport(Session["DBConnection"].ToString());
        objReport_ByGroup = new ContactReport_ByGroup(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "243", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            bindBrand();
            bindCategory();
        }
        else
        {
            GetReport();
        }

        AllPageCode();
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    public void bindBrand()
    {

        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString());
        try
        {
            ddlbrandsearch.DataSource = dt;
            ddlbrandsearch.DataTextField = "Brand_Name";
            ddlbrandsearch.DataValueField = "PBrandId";
            ddlbrandsearch.DataBind();
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.Items.Insert(1, "All Brand");

        }
        catch
        {
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.SelectedIndex = 0;
        }

    }
    public void bindCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        if (dsCategory.Rows.Count > 0)
        {
            ddlcategorysearch.DataSource = dsCategory;
            ddlcategorysearch.DataTextField = "Category_Name";
            ddlcategorysearch.DataValueField = "Category_Id";
            ddlcategorysearch.DataBind();
            ddlcategorysearch.Items.Insert(0, "--Select One--");
            ddlcategorysearch.Items.Insert(1, "All Category");

        }
        else
        {
            ddlcategorysearch.Items.Insert(0, "--Select One--");
            ddlcategorysearch.SelectedIndex = 0;
        }
    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("243", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code



        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


    }
    void GetReport()
    {
        string GroupBy = string.Empty;
        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Session["FinanceYearId"].ToString()),"","","");

        dtFilter = rptdata.sp_Inv_ProductMaster_SelectRow_Report;

        if (ddlbrandsearch.SelectedIndex != 0 && ddlbrandsearch.SelectedIndex != 1)
        {
            try
            {
                dtFilter = new DataView(dtFilter, "PBrandId='" + ddlbrandsearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        if (ddlcategorysearch.SelectedIndex != 0 && ddlcategorysearch.SelectedIndex != 1)
        {
            try
            {
                dtFilter = new DataView(dtFilter, "CategoryId='" + ddlcategorysearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        if (ddlbrandsearch.SelectedIndex == 0 && ddlcategorysearch.SelectedIndex == 0)
        {
            GroupBy = "NoGroup";
            dtFilter = dtFilter.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "AlternateId");

        }
        if (ddlbrandsearch.SelectedIndex != 0 && ddlcategorysearch.SelectedIndex == 0)
        {
            GroupBy = "Brand";
            dtFilter = dtFilter.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "Brand_Name", "PBrandId", "AlternateId");
            try
            {
                dtFilter = new DataView(dtFilter, "Brand_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

        }
        if (ddlbrandsearch.SelectedIndex == 0 && ddlcategorysearch.SelectedIndex != 0)
        {
            GroupBy = "Category";
            dtFilter = dtFilter.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "Category_Name", "CategoryId", "AlternateId");


            try
            {
                dtFilter = new DataView(dtFilter, "Category_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (ddlbrandsearch.SelectedIndex != 0 && ddlcategorysearch.SelectedIndex != 0)
        {
            GroupBy = "Both";

            try
            {
                dtFilter = new DataView(dtFilter, "Brand_Name<>' ' and Category_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }


        for (int i = 0; i < dtFilter.Rows.Count; i++)
        {

            string AlternateId = string.Empty;



            if (dtFilter.Rows[i]["AlternateId1"].ToString() != "")
            {
                AlternateId = dtFilter.Rows[i]["AlternateId1"].ToString();
            }

            if (dtFilter.Rows[i]["AlternateId2"].ToString() != "")
            {
                if (AlternateId == "")
                {
                    AlternateId = dtFilter.Rows[i]["AlternateId2"].ToString();
                }
                else
                {
                    AlternateId = AlternateId + " , " + dtFilter.Rows[i]["AlternateId2"].ToString();
                }
            }
            if (dtFilter.Rows[i]["AlternateId3"].ToString() != "")
            {
                if (AlternateId == "")
                {
                    AlternateId = dtFilter.Rows[i]["AlternateId3"].ToString();
                }
                else
                {
                    AlternateId = AlternateId + " , " + dtFilter.Rows[i]["AlternateId3"].ToString();
                }
            }

            dtFilter.Rows[i]["AlternateId"] = AlternateId;

        }


        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string FromDate = "";
        string Todate = "";
        string Title = "";
        string CompanyName_L = "";
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
            {

                Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();
            }
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


        if (Session["Lang"].ToString() == "1")
            Title = "PRODUCT REPORT";
        else
            Title = Resources.Attendance.Product_Report;


        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName, Session["CompId"].ToString());

        objReport.SetImage(Imageurl);
        objReport.setTitelName(Title);
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        objReport.setGroupByValue(GroupBy);

        objReport.setUserName(Session["UserId"].ToString());
        objReport.DataSource = dtFilter;
        objReport.DataMember = "sp_Inv_ProductMaster_SelectRow_Report";
        rptViewer.Report = objReport;

        rptToolBar.ReportViewer = rptViewer;
        //objReport.ExportToPdf(Server.MapPath("~/Temp/Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf"));
        //ViewState["Path"] = "Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf";
        //ViewState["Id"] = dtFilter.Rows[0]["Trans_Id"].ToString();

    }
}
