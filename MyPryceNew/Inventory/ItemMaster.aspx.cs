using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Data.OleDb;
using System.Web.Services;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.Text;

public partial class Inventory_ItemMaster : BasePage
{
    #region defined Class Object
    Inv_Model_Suppliers objModelSupplier = null;
    Inv_Model_Category_Product objInvModelCategoryProduct = null;
    ProductOptionCategoryDetail objProOpCatedetail = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster ObjUnitMaster = null;
    CountryMaster ObjSysCountryMaster = null;
    BrandMaster ObjBrandMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_Product_Brand ObjProductBrand = null;
    Inv_Product_Category ObjProductCate = null;
    Inv_Product_Location ObjProductLocation = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    Inv_ProductImage ObjProductImage = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    SystemParameter ObjSysPeram = null;
    Set_ApplicationParameter objAppParam = null;
    Common cmn = null;
    Inv_ModelMaster ObjModelMaster = null;

    Inv_ColorMaster ObjColorMaster = null;
    Inv_SizeMaster ObjSizeMaster = null;

    Set_DocNumber objDoc = null;
    IT_ObjectEntry objObjectEntry = null;
    DataAccessClass objDa = null;
    Set_Suppliers ObjSupplierMaster = null;
    Inv_ProductSuppliers ObjProductSupplier = null;
    Inv_StockDetail objStockDetail = null;
    Inv_Product_RelProduct objRelProduct = null;
    ProductTaxMaster objProductTaxMaster = null;
    BillOfMaterial ObjInvBOM = null;
    Inv_Model_Category objModelCategory = null;
    Inv_RackMaster ObjRackMaster = null;
    Inv_RackDetail ObjRackDetail = null;
    Country_Currency objCountryCurrency = null;
    LocationMaster objLocation = null;
    PageControlsSetting objPageCtlSettting = null;
    Inv_ParameterMaster objInvParam = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected string FuLogoUploadFolderPath = "~/CompanyResource/Contact/";
    private int PageSize = 10;
    public const int grdDefaultColCount = 5;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objModelSupplier = new Inv_Model_Suppliers(Session["DBConnection"].ToString());
        objInvModelCategoryProduct = new Inv_Model_Category_Product(Session["DBConnection"].ToString());
        objProOpCatedetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductBrand = new Inv_Product_Brand(Session["DBConnection"].ToString());
        ObjProductCate = new Inv_Product_Category(Session["DBConnection"].ToString());
        ObjProductLocation = new Inv_Product_Location(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        ObjProductImage = new Inv_ProductImage(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjColorMaster = new Inv_ColorMaster(Session["DBConnection"].ToString());
        ObjSizeMaster = new Inv_SizeMaster(Session["DBConnection"].ToString());
        objDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjProductSupplier = new Inv_ProductSuppliers(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objRelProduct = new Inv_Product_RelProduct(Session["DBConnection"].ToString());
        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        objModelCategory = new Inv_Model_Category(Session["DBConnection"].ToString());
        ObjRackMaster = new Inv_RackMaster(Session["DBConnection"].ToString());
        ObjRackDetail = new Inv_RackDetail(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objProductTaxMaster = new ProductTaxMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            //Session["pageIndex"] = 0;
            try
            {
                hdnReportLocation.Value = Session["LocId"].ToString();
            }
            catch
            {
                hdnReportLocation.Value = "";
            }

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ItemMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);

            Session["DtReOrder"] = null;
            string strCurrencyId = string.Empty;
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }

            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
            }
            catch
            {

            }

            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), strCurrencyId.ToString(), Session["DBConnection"].ToString());

            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 10;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 10;
            Session["hdnProductId"] = null;
            ViewState["SearchPage"] = null;
            ViewState["dtSearch"] = null;
            Session["CHECKED_ITEMS"] = null;
            Session["dtProductSupplierCode"] = null;
            #region  remove
            FillddlBrandSearch(ddlbrandsearch);
            FillddlBrandSearch(ddlBinBrandSearch);
            FillProductBrand();
            FillBrand();
            FillProductCategory();
            FillProductCategorySerch(ddlcategorysearch);
            FillProductCategorySerch(ddlBinCategorySearch);
            FillddlBrandSearch(ddlReOrderProductBrand);
            FillProductCategorySerch(ddlReOrderProductcategory);
            #endregion


            //FillddlLocationList();
            //ddlSearchLocation.SelectedValue = Session["LocId"].ToString();

            FillLocation();
            FillDataListGrid(1);
            //FillbinDataListGrid(1);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Session["dtRelatedProduct"] = null;
            fillproductId();
            //FillVeryGrid();
            string CurrencyCode = "";
            try
            {
                CurrencyCode = objDa.get_SingleValue("Select Currency_Code from Set_LocationMaster inner join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Set_LocationMaster.Field1 where Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");
            }
            catch (Exception ex)
            {

            }

            //lblSalesPrice1.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Sales_Price_1, Session["DBConnection"].ToString());
            //lblSalesPrice2.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Sales_Price_2, Session["DBConnection"].ToString());
            //lblSalesPrice3.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Sales_Price_3, Session["DBConnection"].ToString());
            lblSalesPrice1.Text = CurrencyCode + " " + Resources.Attendance.Sales_Price_1;
            lblSalesPrice2.Text = CurrencyCode + " " + Resources.Attendance.Sales_Price_2;
            lblSalesPrice3.Text = CurrencyCode + " " + Resources.Attendance.Sales_Price_3;

            GetLocationCountry();
            //Update page control visibility
            getPageControlVisibility();
            RootFolder();
            try
            {
                string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch
            {

            }

            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            //{
            //    File_Manager_Product.Visible = false;
            //}

            //FillStockColorSizeGrid();


            //Set Default Value On 19-11-2024
            try
            {
                ddlItemType.SelectedValue = "S";
                txtProductUnit.Text = "Pcs/1";
                foreach (ListItem lival in lstProductBrand.Items)
                {
                    lival.Selected = true;
                    break;
                }
                foreach (ListItem lival in ChkProductCategory.Items)
                {
                    lival.Selected = true;
                    break;
                }
            }
            catch (Exception ex)
            {

            }
        }
        //AllPageCode();
        //ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
    }

    //private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    //{
    //    getPageControlVisibility();
    //}

    public void GetLocationCountry()
    {
        try
        {
            txtProductCountry.Text = ObjSysCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Name"].ToString();
        }
        catch
        {
            txtProductCountry.Text = "";
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public string GetSalesPriceinLocal(string Amount)
    {
        string Exchangerate = string.Empty;
        try
        {
            Exchangerate = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        }
        catch
        {
            Exchangerate = "1";
        }
        string SalesPrice = string.Empty;
        try
        {
            SalesPrice = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(Amount) * Convert.ToDouble(Exchangerate)).ToString());
        }
        catch
        {
            SalesPrice = "0";
        }
        return SalesPrice;
    }
    public string GetSalesPriceUsingID(string ProductID)
    {
        double dUnitCost = 0;
        try
        {
            double.TryParse(ObjProductMaster.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", "0", ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString(), out dUnitCost);
        }
        catch
        {

        }
        return dUnitCost.ToString();
    }
    public string setDecimal(string strAmount)
    {
        return ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), strAmount);
    }
    public void fillproductId()
    {
        DataTable dt = objDoc.GetDocumentNumberAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "11", "24", "2");

        dt.Columns.Add("DocNo");
        dt.Columns["Field1"].ColumnName = "ModelId";
        dt.Columns["Field2"].ColumnName = "CategoryId";
        dt.Columns["Field3"].ColumnName = "ManufacturingBrandId";
        dt.Columns["Field4"].ColumnName = "SupplierId";
        dt.Columns["Colour"].ColumnName = "ColourId";
        dt.Columns["Size"].ColumnName = "SizeId";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string DocNo = string.Empty;
            for (int j = 6; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.ToString() != "IsUse")
                {
                    if (dt.Columns[j].ColumnName.ToString() != "IsActive")
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            try
                            {
                                if (Convert.ToBoolean(dt.Rows[i][j].ToString()))
                                {
                                    if (DocNo.ToString() == "")
                                    {
                                        DocNo = dt.Columns[j].ToString();
                                    }
                                    else
                                    {
                                        DocNo = DocNo + "+" + dt.Columns[j].ToString();
                                    }
                                }
                            }
                            catch
                            {
                                //break;
                            }
                        }
                    }
                }
            }
            dt.Rows[i]["DocNo"] = "Prefix+" + DocNo + "+Suffix";
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)rdoProductid, dt, "DocNo", "Trans_Id");
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnExport.Visible = clsPagePermission.bDownload;
        ddlstatus.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        txtProductId.Enabled = clsPagePermission.bEditProductId;
        chkVerify.Visible = clsPagePermission.bVerify;
        chkVerify.Checked = clsPagePermission.bVerify;
        Li_Verify.Visible = clsPagePermission.bVerify;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        if (gvProduct.Visible == true)
        {
            btnGvProductSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        }
        lblcostcolon.Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
        lblCostPrice.Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
        txtCostPrice.Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
    }

    #endregion
    #region//System Function Or Event:-Start
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 10;
        FillDataListGrid(1);
    }
    public string GetFilterString()
    {
        string condition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && (ddlitemtypeserach.SelectedIndex > 0 || txtValue.Text.Trim() != ""))
        {
            if (ddlFieldName.SelectedIndex == 3)
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = ddlFieldName.SelectedValue + "='" + ddlitemtypeserach.SelectedValue + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = ddlFieldName.SelectedValue + " like '%" + ddlitemtypeserach.SelectedValue + "%'";
                }
                else
                {
                    condition = ddlFieldName.SelectedValue + " like '" + ddlitemtypeserach.SelectedValue + "%'";
                }
            }
            else
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = ddlFieldName.SelectedValue + "='" + txtValue.Text + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text + "%'";
                }
                else
                {
                    condition = ddlFieldName.SelectedValue + " like '" + txtValue.Text + "%'";
                }
            }
        }
        return condition;
    }
    public string GetFilterString_ForBin()
    {
        string condition = string.Empty;
        if (ddlbinOption.SelectedIndex != 0 && txtbinVal.Text.Trim() != "")
        {
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = ddlbinFieldName.SelectedValue + "='" + txtbinVal.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = ddlbinFieldName.SelectedValue + " like '%" + txtbinVal.Text + "%'";
            }
            else
            {
                condition = ddlbinFieldName.SelectedValue + " like '" + txtbinVal.Text + "%'";
            }
        }
        return condition;
    }
    protected void ddlFieldName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedIndex == 3)
        {
            ddlitemtypeserach.Visible = true;
            txtValue.Visible = false;
            ddlitemtypeserach.SelectedIndex = 0;
            ddlOption.SelectedIndex = 1;
            ddlOption.Enabled = false;
        }
        else
        {
            ddlitemtypeserach.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
            ddlOption.Enabled = true;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 10;
        ddlbrandsearch.SelectedIndex = 0;
        ddlcategorysearch.SelectedIndex = 0;
        txtSearchPrduct.Text = "";
        Session["pageIndex"] = 1;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlitemtypeserach.Visible = false;
        txtValue.Visible = true;
        ddlitemtypeserach.SelectedIndex = 0;
        btnbind_Click(null, null);
        txtValue.Focus();
        //AllPageCode();
        ddlOption.Enabled = true;
    }

    protected void imbBtnGrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dtlistProduct.Visible = false;
        gvProduct.Visible = true;
        imgBtnDatalist.Visible = true;
        imbBtnGrid.Visible = false;
        FillDataListGrid(1);
        txtValue.Focus();
        //rptPager.Visible = false;
        //AllPageCode();
    }
    protected void imgBtnDatalist_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 10;
        dtlistProduct.Visible = true;
        gvProduct.Visible = false;
        imgBtnDatalist.Visible = false;
        imbBtnGrid.Visible = true;
        FillDataListGrid(1);
        txtValue.Focus();
        btnGvProductSetting.Visible = false;
        //AllPageCode();
        rptPager.Visible = true;

    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {

        btnRefresh_Click(null, null);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 10;
        DataTable dt = (DataTable)Session["dtProduct"];
        FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        ViewState["SubSize"] = 10;
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        ViewState["SubSize"] = 10;
        DataTable dt = (DataTable)Session["dtProduct"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
        ViewState["CurrIndex"] = index;
        int tot = dt.Rows.Count;
        if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
        else if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) == 0)
        {
            FillDataList(dt, index - 1, Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
        else
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtProduct"];
        ViewState["SubSize"] = 10;
        ViewState["CurrIndex"] = Convert.ToInt32(ViewState["CurrIndex"].ToString()) - 1;
        if (Convert.ToInt32(ViewState["CurrIndex"].ToString()) < 0)
        {
            ViewState["CurrIndex"] = 0;
        }
        if (Convert.ToInt16(ViewState["CurrIndex"]) == 0)
        {
        }
        else
        {
        }
        FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        ViewState["SubSize"] = 10;
        DataTable dt = (DataTable)Session["dtProduct"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
        int k1 = Convert.ToInt32(ViewState["CurrIndex"].ToString());
        ViewState["CurrIndex"] = Convert.ToInt32(ViewState["CurrIndex"].ToString()) + 1;
        int k = Convert.ToInt32(ViewState["CurrIndex"].ToString());
        if (Convert.ToInt32(ViewState["CurrIndex"].ToString()) >= index)
        {
            ViewState["CurrIndex"] = index;
        }
        int tot = dt.Rows.Count;
        if (k == index)
        {
            if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
            {
                FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
            }
        }
        else if (k < index)
        {
            if (k + 1 == index)
            {
                if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
                {
                    FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                }
                else
                {
                    FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                }
            }
            else
            {
                FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
            }
        }
        else
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
    }
    protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProduct.PageIndex = e.NewPageIndex;

        objPageCmn.FillData((object)gvProduct, (DataTable)Session["dtProductFilter"], "", "");
        rptPager.Visible = false;
        //AllPageCode();
    }
    protected void gvProduct_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtProductFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtProductFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        //AllPageCode();
    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        ProductEdit();
        Lbl_Tab_New.Text = Resources.Attendance.View;
        //here we modify code for show supplier according the login user permission
        //code created by jitendra on 02-01-2018
        GridProductSupplierCode.Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24", "20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        txtProductId.Enabled = true;
        lnkAddExp.Visible = false;
        pnldescdetail.Visible = false;
        string objSenderID;
        if (sender is LinkButton)
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }
        else
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }
        if (objSenderID != "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
        }
        ProductEdit();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //DataTable dtproduct = ObjProductMaster.GetProductMasterTrueAllwithoutcompanyidandBrandId(HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtproduct = new DataTable();
        if (ddlbrandsearch.SelectedIndex == 0 && ddlcategorysearch.SelectedIndex == 0)
        {
            dtproduct = objDa.return_DataTable("select inv_modelmaster.Model_no as ModelNo,inv_productmaster.ProductCode as ProductId, inv_productmaster.eproductname as ProductName,inv_productmaster.LProductName as ProductName_L,'' as Description,inv_unitmaster.Unit_Name as Unit,case when inv_productmaster.ItemType='S' then 'Stockable' else 'NonStockable' end as ProductType,case when inv_productmaster.MaintainStock='SNO' then 'Y' else 'N' end as IsSerialMaintain,(select brand_name from inv_productbrandmaster  where pbrandid=(select top 1 PBrandId from Inv_Product_Brand where productid=inv_productmaster.productid)) as ProductBrand,(select Category_Name from Inv_Product_CategoryMaster  where Category_Id=(select top 1 CategoryId from Inv_Product_Category where productid=inv_productmaster.productid)) as ProductCategory,case when inv_productmaster.field1='' then 'N' else 'Y' end as IsDiscontinue,case when inv_productmaster.IsActive='True' then 'Y' else 'N' end as IsActive,Inv_ProductMaster.SalesPrice1,Inv_ProductMaster.SalesPrice2,Inv_ProductMaster.salesprice3,Inv_ProductMaster.ReorderQty,(select Rack_Name from inv_rackmaster where Rack_ID in (select top 1 rack_id from Inv_RackDetail where Product_Id=inv_productmaster.productid and Location_Id=" + Session["LocId"].ToString() + ")) as RackName,Inv_ProductMaster.AlternateId1,Inv_ProductMaster.AlternateId2,inv_productmaster.AlternateId3,Inv_ProductMaster.HSCode,isnull(Inv_ProductMaster.URL,0) as WarrantyDay,Inv_ProductMaster.Field4 as SalesComission,Inv_ProductMaster.Field5 as TechnicalComission,isnull(Inv_ProductMaster.DeveloperCommission,0) as DeveloperComission,(select project_name from prj_project_master where project_id=Inv_ProductMaster.ProjectId) as ProjectName,(select Name from Ems_ContactMaster where Trans_Id in (select top 1 supplier_id from Inv_Product_Suppliers where Product_Id=Inv_ProductMaster.productid)) as ProductSupplier from inv_productmaster  left join inv_modelmaster on inv_productmaster.modelno =inv_modelmaster.Trans_id  left join inv_unitmaster on inv_productmaster.unitid = inv_unitmaster.unit_id where inv_productmaster.isactive='True'");
        }
        else
        {
            string strsql = "select inv_modelmaster.Model_no as ModelNo,inv_productmaster.ProductCode as ProductId, inv_productmaster.eproductname as ProductName,inv_productmaster.LProductName as ProductName_L,'' as Description,inv_unitmaster.Unit_Name as Unit,case when inv_productmaster.ItemType='S' then 'Stockable' else 'NonStockable' end as ProductType,case when inv_productmaster.MaintainStock='SNO' then 'Y' else 'N' end as IsSerialMaintain,inv_productbrandmaster.Brand_Name as ProductBrand,Inv_Product_CategoryMaster.category_name as ProductCategory,case when inv_productmaster.field1='' then 'N' else 'Y' end as IsDiscontinue,case when inv_productmaster.IsActive='True' then 'Y' else 'N' end as IsActive,Inv_ProductMaster.SalesPrice1,Inv_ProductMaster.SalesPrice2,Inv_ProductMaster.salesprice3,Inv_ProductMaster.ReorderQty,(select Rack_Name from inv_rackmaster where Rack_ID in (select top 1 rack_id from Inv_RackDetail where Product_Id=inv_productmaster.productid and Location_Id=2)) as RackName,Inv_ProductMaster.AlternateId1,Inv_ProductMaster.AlternateId2,inv_productmaster.AlternateId3,Inv_ProductMaster.HScode,isnull(Inv_ProductMaster.URL,0) as WarrantyDay,Inv_ProductMaster.Field4 as SalesComission,Inv_ProductMaster.Field5 as TechnicalComission,isnull(Inv_ProductMaster.DeveloperCommission,0) as DeveloperCommission,(select project_name from prj_project_master where project_id=Inv_ProductMaster.ProjectId) as ProjectName,(select Name from Ems_ContactMaster where Trans_Id in (select top 1 supplier_id from Inv_Product_Suppliers where Product_Id=Inv_ProductMaster.productid)) as ProductSupplier from inv_productmaster  left join inv_modelmaster on inv_productmaster.modelno =inv_modelmaster.Trans_id left join inv_unitmaster on inv_productmaster.unitid = inv_unitmaster.unit_id left join Inv_Product_Category on inv_productmaster.productid = Inv_Product_Category.ProductId left join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id =Inv_Product_Category.categoryid left join Inv_Product_Brand on inv_productmaster.productid = Inv_Product_Brand.ProductId left join inv_productbrandmaster on inv_productbrandmaster.pBrandId =Inv_Product_Brand.PBrandId  where inv_productmaster.isactive='True'";

            if (ddlbrandsearch.SelectedIndex > 0)
            {
                strsql = strsql + " and Inv_Product_Brand.PBrandId=" + ddlbrandsearch.SelectedValue.Trim() + "";
            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                strsql = strsql + " and Inv_Product_Category.categoryid=" + ddlcategorysearch.SelectedValue.Trim() + "";
            }

            dtproduct = objDa.return_DataTable(strsql);
        }


        if (chkDiscontinue.Checked)
        {
            dtproduct = new DataView(dtproduct, "IsDiscontinue= 'Y'", "", DataViewRowState.CurrentRows).ToTable();
        }



        if (dtproduct.Rows.Count > 0)
        {
            ExportTableData(dtproduct, "ProductList");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
    }
    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    public DataTable ConvetExcelToDataTable(string path)
    {
        DataTable dt = new DataTable();
        string strcon = string.Empty;
        if (Path.GetExtension(path) == ".xls")
        {
            strcon = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + "; Extended Properties =\"Excel 8.0;HDR=YES;\"";
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        try
        {
            OleDbConnection oledbConn = new OleDbConnection(strcon);
            oledbConn.Open();
            DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
            OleDbCommand com = new OleDbCommand(strquery, oledbConn);
            DataSet ds = new DataSet();
            OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
            oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
            oledbConn.Close();
            dt = ds.Tables[0];
        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillbinDataListGrid(1);
        ddlBinBrandSearch.Focus();
    }
    protected void txtProductCountry_TextChanged(object sender, EventArgs e)
    {
        if (txtProductCountry.Text != "")
        {
            string CountryId = string.Empty;
            try
            {
                CountryId = new DataView(ObjSysCountryMaster.GetCountryMaster(), "Country_Name='" + txtProductCountry.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Country_Id"].ToString();
                txtProductCountry.Focus();
            }
            catch
            {
                txtProductCountry.Text = "";
                txtProductCountry.Focus();
                DisplayMessage("Select Product Country");
            }
        }
    }
    protected void btnpushBrandAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstProductBrand.Items)
        {
            lstSelectedProductBrand.Items.Add(li);
        }
        foreach (ListItem li in lstSelectedProductBrand.Items)
        {
            lstProductBrand.Items.Remove(li);
        }
        btnpushBrandAll.Focus();
    }
    protected void btnpullBrandAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstSelectedProductBrand.Items)
        {
            lstProductBrand.Items.Add(li);
        }
        foreach (ListItem li in lstProductBrand.Items)
        {
            lstSelectedProductBrand.Items.Remove(li);
        }
        btnpullBrandAll.Focus();
    }
    protected void btnpushBrand_Click(object sender, EventArgs e)
    {
        if (lstProductBrand.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstProductBrand.Items)
            {
                if (li.Selected)
                {
                    lstSelectedProductBrand.Items.Add(li);
                }
            }
            foreach (ListItem li in lstSelectedProductBrand.Items)
            {
                lstProductBrand.Items.Remove(li);
            }
            lstSelectedProductBrand.SelectedIndex = -1;
        }
        btnpushBrand.Focus();
    }
    protected void btnpullBrand_Click(object sender, EventArgs e)
    {
        if (lstSelectedProductBrand.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstSelectedProductBrand.Items)
            {
                if (li.Selected)
                {
                    lstProductBrand.Items.Add(li);
                }
            }
            foreach (ListItem li in lstProductBrand.Items)
            {
                lstSelectedProductBrand.Items.Remove(li);
            }
            lstProductBrand.SelectedIndex = -1;
        }
        btnpullBrand.Focus();
    }
    protected void ChkProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Ems_Contact_ProductCategory objContactcategory = new Ems_Contact_ProductCategory();
        //DataTable dtSupplier = new DataTable();
        //string strSelectedCategory = string.Empty;
        //foreach (ListItem li in ChkProductCategory.Items)
        //{
        //    if (li.Selected == true)
        //    {
        //        if (strSelectedCategory == "")
        //        {
        //            strSelectedCategory = li.Value.Trim();
        //        }
        //        else
        //        {
        //            strSelectedCategory = strSelectedCategory + "," + li.Value.Trim();
        //        }
        //    }
        //}
        //string strSupplierList = string.Empty;
        //if (strSelectedCategory != "")
        //{
        //    DataTable dtContactCategory = objContactcategory.GetDateBy_ContactType(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "S");
        //    if (dtContactCategory.Rows.Count > 0)
        //    {
        //        dtContactCategory = new DataView(dtContactCategory, "CategoryId in (" + strSelectedCategory + ")", "", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Contact_Id");
        //        foreach (DataRow dr in dtContactCategory.Rows)
        //        {
        //            strSupplierList += dr["Contact_Id"].ToString() + ",";
        //        }
        //        if (strSupplierList != "")
        //        {
        //            dtSupplier = new DataView(ObjSupplierMaster.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString()), "Supplier_Id in (" + strSupplierList.Substring(0, strSupplierList.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //    }
        //}
        //DataTable dt = (DataTable)Session["dtProductSupplierCode"];
        //foreach (DataRow dr in dtSupplier.Rows)
        //{
        //    if (dt == null)
        //    {
        //        dt = new DataTable();
        //        dt.Columns.Add("Supplier_Id");
        //        dt.Columns.Add("Name");
        //        dt.Columns.Add("ProductSupplierCode");
        //    }
        //    else
        //    {
        //        DataTable dttemp = new DataView(dt, "Supplier_Id=" + dr["Supplier_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //        if (dttemp.Rows.Count > 0)
        //        {
        //            continue;
        //        }
        //    }
        //    dt.Rows.Add(dr["Supplier_Id"].ToString(), dr["Name"].ToString(), "0");
        //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    Session["dtProductSupplierCode"] = dt;
        //}
        //objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
        //txtProductSupplierCode.Text = "";
        //txtSuppliers.Text = "";
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
    }
    protected void btnPushAllCate_Click(object sender, EventArgs e)
    {
        //foreach (ListItem li in lstProductCategory.Items)
        //{
        //    lstSelectProductCategory.Items.Add(li);
        //}
        //foreach (ListItem li in lstSelectProductCategory.Items)
        //{
        //    lstProductCategory.Items.Remove(li);
        //}
        //btnPushAllCate.Focus();
    }
    protected void btnPullAllCate_Click(object sender, EventArgs e)
    {
        //foreach (ListItem li in lstSelectProductCategory.Items)
        //{
        //    lstProductCategory.Items.Add(li);
        //}
        //foreach (ListItem li in lstProductCategory.Items)
        //{
        //    lstSelectProductCategory.Items.Remove(li);
        //}
        //btnPullAllCate.Focus();
    }
    protected void btnPushCate_Click(object sender, EventArgs e)
    {
        //if (lstProductCategory.SelectedIndex >= 0)
        //{
        //    foreach (ListItem li in lstProductCategory.Items)
        //    {
        //        if (li.Selected)
        //        {
        //            lstSelectProductCategory.Items.Add(li);
        //        }
        //    }
        //    foreach (ListItem li in lstSelectProductCategory.Items)
        //    {
        //        lstProductCategory.Items.Remove(li);
        //    }
        //    lstSelectProductCategory.SelectedIndex = -1;
        //}
        //btnPushCate.Focus();
    }
    protected void btnPullCate_Click(object sender, EventArgs e)
    {
        //if (lstSelectProductCategory.SelectedIndex >= 0)
        //{
        //    foreach (ListItem li in lstSelectProductCategory.Items)
        //    {
        //        if (li.Selected)
        //        {
        //            lstProductCategory.Items.Add(li);
        //        }
        //    }
        //    foreach (ListItem li in lstProductCategory.Items)
        //    {
        //        lstSelectProductCategory.Items.Remove(li);
        //    }
        //    lstProductCategory.SelectedIndex = -1;
        //}
        //btnPullCate.Focus();
    }
    protected void txtSuppliers_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtProductSupplierCode"];
        if (dt != null)
        {
            if (txtSuppliers.Text != "")
            {
                try
                {
                    string strSupplierId = "0";
                    try
                    {
                        strSupplierId = txtSuppliers.Text.Split('/')[1].ToString();
                    }
                    catch
                    {
                    }
                    string query = "Supplier_Id = '" + strSupplierId + "'";
                    dt = new DataView(dt, query, "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DisplayMessage("Supplier Name Already Exists");
                        txtSuppliers.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                    }
                    else
                    {
                        DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
                        dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductSupplierCode);
                        }
                        else
                        {
                            DisplayMessage("Invalid Supplier Name");
                            txtSuppliers.Text = "";
                            txtSuppliers.Focus();
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                        }
                    }
                }
                catch
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }
        else
        {
            if (txtSuppliers.Text != "")
            {
                string strSupplierId = "0";
                try
                {
                    strSupplierId = txtSuppliers.Text.Split('/')[1].ToString();
                }
                catch
                {
                }
                DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
                try
                {
                    dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                    dt1 = new DataTable();
                }
                if (dt1.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductSupplierCode);
                }
                else
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSuppliers.Focus();
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }
    }
    protected void GridProductSupplierCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridProductSupplierCode.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtProductSupplierCode"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
    }
    protected void IbtnAddProductSupplierCode_Click(object sender, EventArgs e)
    {
        if (txtSuppliers.Text != "")
        {
            DataTable dt = (DataTable)Session["dtProductSupplierCode"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Supplier_Id");
                dt.Columns.Add("Name");
                dt.Columns.Add("ProductSupplierCode");
            }
            string strSupplierId = "";
            string strSupplierName = "";
            if (txtSuppliers.Text != "")
            {
                strSupplierId = txtSuppliers.Text.Split('/')[1].ToString();
                strSupplierName = txtSuppliers.Text.Split('/')[0].ToString();
            }
            dt.Rows.Add(strSupplierId, strSupplierName, txtProductSupplierCode.Text);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
            Session["dtProductSupplierCode"] = dt;
            txtProductSupplierCode.Text = "";
            txtSuppliers.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
        }
        else
        {
            // DisplayMessage("Please Select Supplier First");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
        }
    }
    protected void IbtnDeleteSupplier_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtProductSupplierCode"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("Supplier_Id ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
                Session["dtProductSupplierCode"] = dt;
            }
        }
    }
    #region RelatedProduct
    protected void txtProductCode_OnTextChanged(object sender, EventArgs e)
    {
        if (txtProductCode.Text != "")
        {
            DataTable dt = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            try
            {
                dt = new DataView(dt, "ProductCode= '" + txtProductCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count > 0)
            {
                hdnRelatedProductId.Value = dt.Rows[0]["ProductId"].ToString();
                txtERelatedProduct.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductCode.Text = dt.Rows[0]["ProductCode"].ToString();
                if (Session["dtRelatedProduct"] != null)
                {
                    DataTable DtProduct = (DataTable)Session["dtRelatedProduct"];
                    try
                    {
                        DtProduct = new DataView(DtProduct, "RelatedProductId='" + hdnRelatedProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (DtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product is already exists");
                        txtProductCode.Text = "";
                        txtERelatedProduct.Text = "";
                        txtProductCode.Focus();
                    }
                    else
                    {
                        if (dt.Rows[0]["ProductCode"].ToString() == txtProductId.Text)
                        {
                            DisplayMessage("Select Product in Suggestion only");
                            txtProductCode.Text = "";
                            txtERelatedProduct.Text = "";
                            txtProductCode.Focus();
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select Product Id in Suggestion only");
                txtProductCode.Text = "";
                txtERelatedProduct.Text = "";
                txtProductCode.Focus();
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductCode);
        }
    }
    protected void txtERelatedProduct_OnTextChanged(object sender, EventArgs e)
    {
        if (txtERelatedProduct.Text != "")
        {
            DataTable dt = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            try
            {
                dt = new DataView(dt, "EProductName='" + txtERelatedProduct.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count > 0)
            {
                hdnRelatedProductId.Value = dt.Rows[0]["ProductId"].ToString();
                txtProductCode.Text = dt.Rows[0]["ProductCode"].ToString();
                if (Session["dtRelatedProduct"] != null)
                {
                    DataTable DtProduct = (DataTable)Session["dtRelatedProduct"];
                    try
                    {
                        DtProduct = new DataView(DtProduct, "RelatedProductId='" + hdnRelatedProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (DtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product is already exists");
                        txtERelatedProduct.Text = "";
                        txtProductCode.Text = "";
                        txtERelatedProduct.Focus();
                    }
                    else
                    {
                        if (dt.Rows[0]["ProductCode"].ToString() == txtProductId.Text)
                        {
                            DisplayMessage("Select Product in Suggestion only");
                            txtERelatedProduct.Text = "";
                            txtProductCode.Text = "";
                            txtERelatedProduct.Focus();
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select Product in Suggestion only");
                txtERelatedProduct.Text = "";
                txtProductCode.Text = "";
                txtERelatedProduct.Focus();
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtERelatedProduct);
        }
    }
    protected void GvRelatedProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvRelatedProduct.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtRelatedProduct"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvRelatedProduct, dt, "", "");
    }
    protected void ImgButtonRelatedProductAdd_Click(object sender, EventArgs e)
    {
        if (txtERelatedProduct.Text.Trim() == "")
        {
            DisplayMessage("Enter Product Name");
            txtERelatedProduct.Focus();
            return;
        }
        if (Session["dtRelatedProduct"] != null)
        {
            DataTable DtProduct = (DataTable)Session["dtRelatedProduct"];
            try
            {
                DtProduct = new DataView(DtProduct, "RelatedProductId='" + hdnRelatedProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtProduct.Rows.Count > 0)
            {
                DisplayMessage("Product is already exists");
                txtERelatedProduct.Text = "";
                txtProductCode.Text = "";
                txtERelatedProduct.Focus();
                return;
            }
        }
        DataTable dt = new DataTable();
        if (Session["dtRelatedProduct"] == null)
        {
            dt = new DataTable();
            dt.Columns.Add("RelatedProductId");
            dt.Columns.Add("RelatedProductCode");
            dt.Columns.Add("RelatedProductName");
        }
        else
        {
            dt = (DataTable)Session["dtRelatedProduct"];
        }
        string strRelatedProductId = "";
        string strRelatedProductName = "";
        string strRelatedProductCode = "";
        if (txtERelatedProduct.Text != "")
        {
            strRelatedProductId = hdnRelatedProductId.Value;
            strRelatedProductCode = txtProductCode.Text;
            strRelatedProductName = txtERelatedProduct.Text;
        }
        dt.Rows.Add(strRelatedProductId, strRelatedProductCode, strRelatedProductName);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvRelatedProduct, dt, "", "");
        Session["dtRelatedProduct"] = dt;
        txtERelatedProduct.Text = "";
        txtProductCode.Text = "";
        hdnRelatedProductId.Value = "0";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtERelatedProduct);
    }
    protected void IbtnDeleteRelatedProduct_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtRelatedProduct"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("RelatedProductId ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvRelatedProduct, dt, "", "");
                Session["dtRelatedProduct"] = dt;
            }
        }
    }
    #endregion
    protected void txtProductUnit_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            if (GetUnitId(((TextBox)sender).Text) == "")
            {
                ((TextBox)sender).Text = "";
                DisplayMessage("Select Product Unit");
                ((TextBox)sender).Focus();
            }
            else
            {
                txtDesc.Focus();
            }
        }
    }
    protected void ddlMaintainStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMaintainStock.SelectedIndex == 1)
        {
            TdTypeOfBatchNo1.Visible = true;
            //TdTypeOfBatchNo2.Visible = true;
            //TdTypeOfBatchNo3.Visible = true;
        }
        else if (ddlMaintainStock.SelectedIndex == 3)
        {
            TdTypeOfBatchNo1.Visible = true;
            //TdTypeOfBatchNo2.Visible = true;
            //TdTypeOfBatchNo3.Visible = true;
        }
        else
        {
            TdTypeOfBatchNo1.Visible = false;
            //TdTypeOfBatchNo2.Visible = false;
            //TdTypeOfBatchNo3.Visible = false;
        }
        ddlTypeOfBatchNo.Focus();
    }
    protected void ddlItypeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemType.SelectedIndex == 1 || ddlItemType.SelectedIndex == 3)
        {
            ddlMaintainStock.Enabled = true;
        }
        else
        {
            ddlMaintainStock.SelectedIndex = 0;
            ddlMaintainStock.Enabled = false;
        }
        txtReorderQty.Text = "";
    }
    protected void ddlstatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlstatus.SelectedIndex == 0 || ddlstatus.SelectedIndex == 1)
        {
            trdiscontinuereason.Visible = false;
        }
        else
        {
            trdiscontinuereason.Visible = true;
            txtDiscontinueReason.Text = "";
        }
    }
    public string GetRagistrationCode()
    {
        string RegistrationCode = "";
        try
        {
            RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
            if (RegistrationCode != "")
            {
                return RegistrationCode;
            }
            else
            {
                return RegistrationCode;
            }
        }
        catch
        {
            return RegistrationCode;
        }

    }


    public class GetItemData
    {
        public string Param { get; set; }
        public ItemMaster ProductDetail { get; set; }
    }

    public class ItemMaster
    {
        public string CompanyId { get; set; }
        public string BrandId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string PartNo { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string ProductName { get; set; }
        public string ProductNameAr { get; set; }
        public string CountryId { get; set; }
        public string UnitId { get; set; }
        public string ItemType { get; set; }
        public string HScode { get; set; }
        public string HasBatchNo { get; set; }
        public string TypeOfBatchNo { get; set; }
        public string HasSerialNo { get; set; }
        public string ReorderQty { get; set; }
        public string CostPrice { get; set; }
        public string Description { get; set; }
        public string SalesPrice1 { get; set; }
        public string SalesPrice2 { get; set; }
        public string SalesPrice3 { get; set; }
        public string ProductColor { get; set; }
        public string WSalesPrice { get; set; }
        public string ReservedQty { get; set; }
        public string DamageQty { get; set; }
        public string ExpiredQty { get; set; }
        public string MaximumQty { get; set; }
        public string MinimumQty { get; set; }
        public string Profit { get; set; }
        public string Discount { get; set; }
        public string MaintainStock { get; set; }
        public string URL { get; set; }
        public string Weight { get; set; }
        public string WeightUnitID { get; set; }
        public string DimLenth { get; set; }
        public string DimHeight { get; set; }
        public string DimDepth { get; set; }
        public string AlternateId1 { get; set; }
        public string AlternateId2 { get; set; }
        public string AlternateId3 { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string DeveloperCommission { get; set; }
        public string ProjectId { get; set; }
        public string CurrencyId { get; set; }
        public string SnoPrefix { get; set; }
        public string SnoSuffix { get; set; }
        public string SnoStartFrom { get; set; }
        public string SizeId { get; set; }
        public string ColourId { get; set; }
        public List<ProductBrand> ProductBrand { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }
    }

    public class ProductBrand
    {
        public string ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
    }
    public class ProductCategory
    {
        public string ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
    }


    public string DataUploadInPOS()
    {
        string strResponse = string.Empty;

        ServicePointManager.Expect100Continue = true;
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;

        //var baseAddress = "https://localhost:44300/ItemMasterAPI";
        var baseAddress = ConfigurationManager.AppSettings["POSApiURLforPryce"].ToString();
        var http = (System.Net.HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        http.Accept = "application/json; charset=utf-8";
        http.ContentType = "application/x-www-form-urlencoded";
        http.Method = "POST";

        var requestData = new GetItemData
        {
            Param = "InsertInPOS",
            ProductDetail = new ItemMaster
            {
                CompanyId = "2",
                BrandId = "2",
                ProductId = "4",
                ProductCode = "KX-NCS3508",
                PartNo = "False",
                ModelNo = "178",
                ModelName = "0",
                ProductName = "Product A",
                ProductNameAr = "Product A Arabic",
                CountryId = "789",
                UnitId = "1",
                ItemType = "S",
                HScode = "HS1234",
                HasBatchNo = "true",
                TypeOfBatchNo = "Internally",
                HasSerialNo = "false",
                ReorderQty = "10",
                CostPrice = "100.00",
                Description = "Description of Product A",
                SalesPrice1 = "150.00",
                SalesPrice2 = "160.00",
                SalesPrice3 = "170.00",
                ProductColor = "0",
                WSalesPrice = "140.00",
                ReservedQty = "5",
                DamageQty = "2",
                ExpiredQty = "1",
                MaximumQty = "50",
                MinimumQty = "5",
                Profit = "20.00",
                Discount = "5.00",
                MaintainStock = "NM",
                URL = "http://example.com/productA",
                Weight = "0",
                WeightUnitID = "0",
                DimLenth = "0",
                DimHeight = "0",
                DimDepth = "0",
                AlternateId1 = "ALT001",
                AlternateId2 = "ALT002",
                AlternateId3 = "ALT003",
                Field1 = "Field 1",
                Field2 = "Field 2",
                Field3 = "Field 3",
                Field4 = "Field 4",
                Field5 = "Field 5",
                Field6 = "false",
                Field7 = "2024-06-29T12:00:00",
                IsActive = "true",
                CreatedBy = "Admin",
                CreatedDate = "2024-06-29T12:00:00",
                ModifiedBy = "Admin",
                ModifiedDate = "2024-06-30T10:30:00",
                DeveloperCommission = "0",
                ProjectId = "0",
                CurrencyId = "10",
                SnoPrefix = "0",
                SnoSuffix = "0",
                SnoStartFrom = "0",
                SizeId = "1",
                ColourId = "1",
                ProductBrand = new List<ProductBrand>
                {
                    new ProductBrand { ProductBrandId = "8", ProductBrandName = ""}
                },
                ProductCategory = new List<ProductCategory>
                {
                    new ProductCategory { ProductCategoryId = "8", ProductCategoryName = ""}
                }
            }
        };

        //  string jsonContent = JsonConvert.SerializeObject(requestData);

        var jsonContent = JsonConvert.SerializeObject(new[] { requestData });

        ASCIIEncoding encoding = new ASCIIEncoding();
        Byte[] bytes = encoding.GetBytes(jsonContent);

        Stream newStream = http.GetRequestStream();
        newStream.Write(bytes, 0, bytes.Length);
        newStream.Close();

        var response = (HttpWebResponse)http.GetResponse();
        var stream = response.GetResponseStream();
        var sr = new StreamReader(stream);
        var content = sr.ReadToEnd();

        return content;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strModelNo = string.Empty;


        if (!string.IsNullOrEmpty(txtSnoStartFrom.Text))
        {
            int sNoStratFrom = 0;
            int.TryParse(txtSnoStartFrom.Text, out sNoStratFrom);
            if (sNoStratFrom == 0)
            {
                DisplayMessage("Please check Serial Start From Value");
                return;
            }
        }

        if (rdoProductid.Items.Count == 1)
        {
            DataTable dt = objDoc.GetDocumentNumberAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "11", "24", "2");
            if (dt.Rows.Count > 0)
            {
                rdoProductid.SelectedValue = dt.Rows[0]["Trans_Id"].ToString();
            }
        }


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string strDiscontinueReason = string.Empty;
        string strDiscontinue = string.Empty;
        string HSCode = string.Empty;
        string HSSerialNo = string.Empty;
        string IsActive = string.Empty;
        if ((txtProductId.Text == "") && (rdoProductid.SelectedValue.ToString() == ""))
        {
            DisplayMessage("Enter Product Id OR Select In Suggestions");
            txtProductId.Focus();
            return;
        }
        else
        {
            if (txtProductId.Text != "")
            {
                DataTable dt = ObjProductMaster.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtProductCode.Text, HttpContext.Current.Session["FinanceYearId"].ToString());
                if (dt.Rows.Count != 0)
                {
                    if (!Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Product Id Is Already Exits :- Go To Bin Tab");
                        txtProductId.Focus();
                        return;
                    }
                    else
                    {
                        if (hdnProductId.Value != dt.Rows[0]["ProductId"].ToString())
                        {
                            DisplayMessage("Product Id Is Already Exits");
                            txtProductId.Focus();
                            return;
                        }
                    }
                }
                //if (txtProductId.Text.Trim() == txtAlterId1.Text.Trim() || txtProductId.Text.Trim() == txtAlterId2.Text.Trim() || txtProductId.Text.Trim() == txtAlterId3.Text.Trim())
                //{
                //    DisplayMessage("Product Id Is Already Exits as Alternate Id");
                //    txtProductId.Focus();
                //    return;
                //}
            }
        }

        if (chkserialNo.Checked)
        {
            ddlMaintainStock.SelectedValue = "SNO";
        }
        else
        {
            ddlMaintainStock.SelectedValue = "0";
        }
        if (txtModelName.Text == "")
        {
            txtModelName.Text = "0";
        }
        if (txtEProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtEProductName.Focus();
            return;
        }
        if (ddlItemType.SelectedIndex == 0)
        {
            DisplayMessage("Select Item Type");
            ddlItemType.Focus();
            return;
        }
        else
        {

        }
        if (txtReorderQty.Text == "")
        {
            txtReorderQty.Text = "1";
        }
        //if (ddlMaintainStock.SelectedIndex == 0 && ddlMaintainStock.Enabled == true)
        //{
        //    DisplayMessage("Select Inventory Type");
        //    ddlMaintainStock.Focus();
        //    return;
        //}
        if (txtProductUnit.Text == "")
        {
            DisplayMessage("Enter Product Unit");
            txtProductUnit.Focus();
            return;
        }
        bool IsBrand = false;
        foreach (ListItem li in ChkBrand.Items)
        {
            if (li.Selected)
            {
                IsBrand = true;
                break;
            }
        }
        if (!IsBrand)
        {
            DisplayMessage("Select Atleast one brand");
            return;
        }
        if (txtSalesPrice1.Text == "")
        {
            if (txtSalesPrice2.Text == "")
            {
                if (txtSalesPrice3.Text == "")
                {
                    //DisplayMessage("Enter Sales Price");
                    //TabContainer1.ActiveTabIndex = 1;
                    //txtSalesPrice1.Focus();
                    //return;
                }
            }
        }
        string ModelId = string.Empty;
        if (txtModelNo.Text == "")
        {
            hdnModelId.Value = "0";
        }
        else
        {
            try
            {
                if (txtModelNo.Text.Contains('/'))
                {
                    string[] s = txtModelNo.Text.Split('/');
                    hdnModelId.Value = Convert.ToInt32(txtModelNo.Text.Split('/')[s.Length - 1].ToString()).ToString();
                    strModelNo = txtModelNo.Text.Split('/')[0].ToString();
                }
            }
            catch
            {
                hdnModelId.Value = "0";
            }
        }

        //SizeId & Colour Id
        string SizeId = string.Empty;
        string ColourId = string.Empty;
        if (txtSize.Text == "")
        {
            hdnSizeId.Value = "0";
        }
        else
        {
            try
            {
                if (txtSize.Text.Contains('/'))
                {
                    string[] s = txtSize.Text.Split('/');
                    hdnSizeId.Value = Convert.ToInt32(txtSize.Text.Split('/')[s.Length - 1].ToString()).ToString();
                }
            }
            catch
            {
                hdnSizeId.Value = "0";
            }
        }
        if (txtColour.Text == "")
        {
            hdnColourId.Value = "0";
        }
        else
        {
            try
            {
                if (txtColour.Text.Contains('/'))
                {
                    string[] s = txtColour.Text.Split('/');
                    hdnColourId.Value = Convert.ToInt32(txtColour.Text.Split('/')[s.Length - 1].ToString()).ToString();
                }
            }
            catch
            {
                hdnColourId.Value = "0";
            }
        }



        if (ChkHasBatchNo.Checked)
        {
            HSCode = true.ToString();
        }
        else
        {
            HSCode = false.ToString();
        }
        if (ChkHasSerialNo.Checked)
        {
            HSSerialNo = true.ToString();
        }
        else
        {
            HSSerialNo = false.ToString();
        }
        //here we are  set 0 warranty where warrant box  is blank
        if (txtProductyWarranty.Text == "")
        {
            txtProductyWarranty.Text = "0";
        }
        if (ddlstatus.SelectedItem.Text == "Active")
        {
            IsActive = true.ToString();
        }
        else
        {
            if (ddlstatus.SelectedItem.Text == "InActive")
            {
                IsActive = false.ToString();
            }
            else
            {
                IsActive = true.ToString();
                if (txtDiscontinueReason.Text == "")
                {
                    DisplayMessage("Enter Discontinue reason");
                    txtDiscontinueReason.Focus();
                    return;
                }
                else
                {
                    strDiscontinueReason = txtDiscontinueReason.Text;
                }
            }
        }
        if (txtLenth.Text == "")
        {
            txtLenth.Text = "0";
        }
        if (txtHeight.Text == "")
        {
            txtHeight.Text = "0";
        }
        if (txtDepth.Text == "")
        {
            txtDepth.Text = "0";
        }
        if (txtprofit.Text == "")
        {
            txtprofit.Text = "0";
        }
        if (txtProductCountry.Text == "")
        {
            DisplayMessage("Enter Made In !");
            txtProductCountry.Focus();
            return;
        }
        //here we add validation for add supplier 
        //validation added by jitendra upadhyay on 04-07-2016
        bool IsSelect = false;
        foreach (ListItem li in lstProductBrand.Items)
        {
            if (li.Selected)
            {
                IsSelect = true;
                break;
            }
        }
        if (!IsSelect)
        {
            DisplayMessage("Select Manufacturing Brand ");
            TabContainer1.ActiveTabIndex = 2;
            return;
        }
        IsSelect = false;
        foreach (ListItem li in ChkProductCategory.Items)
        {
            if (li.Selected)
            {
                IsSelect = true;
                break;
            }
        }
        if (!IsSelect)
        {
            DisplayMessage("Select Product Category ");
            TabContainer1.ActiveTabIndex = 2;
            return;
        }
        //if (GridProductSupplierCode.Rows.Count == 0)
        //{
        //    DisplayMessage("Enter Supplier Information");
        //    TabContainer1.ActiveTabIndex = 3;
        //    return;
        //}
        try
        {
            if (txtProductUnit.Text != "")
            {
                txtProductUnit.Text = txtProductUnit.Text.Split('/')[1].ToString();
            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Please correct the Unit");
            txtProductUnit.Focus();
            return;
        }

        if (txtWeightUnit.Text != "")
        {
            txtWeightUnit.Text = txtWeightUnit.Text.Split('/')[1].ToString();
        }
        //if (txtAlterId1.Text != "")
        //{
        //    if (!checkAlternateid(txtAlterId1))
        //    {
        //        txtAlterId1.Text = "";
        //        txtAlterId1.Focus();
        //        DisplayMessage("Alternate Id 1 is Already Exists");
        //        return;
        //    }
        //}
        //if (txtAlterId2.Text != "")
        //{
        //    if (!checkAlternateid(txtAlterId2))
        //    {
        //        txtAlterId2.Text = "";
        //        txtAlterId2.Focus();
        //        DisplayMessage("Alternate Id 2 is Already Exists");
        //        return;
        //    }
        //}
        //if (txtAlterId3.Text != "")
        //{
        //    if (!checkAlternateid(txtAlterId3))
        //    {
        //        txtAlterId3.Text = "";
        //        txtAlterId3.Focus();
        //        DisplayMessage("Alternate Id 3 is Already Exists");
        //        return;
        //    }
        //}
        int b = 0;
        if (txtProductCountry.Text != "")
        {
            string CountryId = string.Empty;
            try
            {
                CountryId = new DataView(ObjSysCountryMaster.GetCountryMaster(), "Country_Name='" + txtProductCountry.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Country_Id"].ToString();
                txtProductCountry.Text = CountryId.ToString();
            }
            catch
            {
                txtProductCountry.Text = "0";
            }
        }
        else
        {
            txtProductCountry.Text = "0";
        }
        if (ViewState["ExchangeRate"] == null)
        {
            ViewState["ExchangeRate"] = "1";
        }
        string Exchangerate = string.Empty;
        try
        {
            Exchangerate = SystemParameter.GetExchageRate(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());
        }
        catch
        {
            Exchangerate = "1";
        }
        if (txtSalesPrice1.Text == "")
        {
            txtSalesPrice1.Text = "0";
        }
        else
        {
            //txtSalesPrice1.Text = (Convert.ToDouble(txtSalesPrice1.Text) * Convert.ToDouble(Exchangerate)).ToString();
        }
        if (txtSalesPrice2.Text == "")
        {
            txtSalesPrice2.Text = "0";
        }
        else
        {
            //txtSalesPrice2.Text = (Convert.ToDouble(txtSalesPrice2.Text) * Convert.ToDouble(Exchangerate)).ToString();
        }
        if (txtSalesPrice3.Text == "")
        {
            txtSalesPrice3.Text = "0";
        }
        else
        {
            //txtSalesPrice3.Text = (Convert.ToDouble(txtSalesPrice3.Text) * Convert.ToDouble(Exchangerate)).ToString();
        }
        if (txtSalesCommission.Text == "")
        {
            txtSalesCommission.Text = "0";
        }
        if (txtTechnicalCommission.Text == "")
        {
            txtTechnicalCommission.Text = "0";
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting("ItemMaster", this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }




        string currencyId = objCountryCurrency.getCurrencyByLocation(Session["locId"].ToString());

        bool isTaxEnabled = Inv_ParameterMaster.isSalesTax(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        bool bInsert = false;
        string ProductCode = string.Empty;
        try
        {
            //Need to check the new product code concept. If Already Exist. 13-07-2024 By Lokesh
            if (txtProductId.Text == "")
            {

                if (hdnModelId.Value != "0" && hdnColourId.Value != "0" && hdnSizeId.Value != "0")
                {
                    ProductCode = objDoc.GetDocumentNoProduct(rdoProductid.SelectedValue.ToString(), Session["CompId"].ToString().ToString(), "11", "24", strModelNo, hdnColourId.Value, hdnSizeId.Value, "0", "0", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                    if (ProductCode != "")
                    {
                        DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode='" + ProductCode.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count != 0)
                        {
                            if (hdnProductId.Value != "")
                            {
                                dt = new DataView(dt, "ProductId<>" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            if (dt.Rows.Count > 0)
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                                {
                                    DisplayMessage("Product Code Already Exist");
                                    trns.Rollback();
                                    if (con.State == System.Data.ConnectionState.Open)
                                    {
                                        con.Close();
                                    }
                                    trns.Dispose();
                                    con.Dispose();
                                    return;
                                }
                            }
                        }
                    }
                }
            }


            if (hdnProductId.Value == "")
            {
                b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), txtProductId.Text.Trim().ToString().ToUpper(), chkIsResouces.Checked.ToString(), hdnModelId.Value, "0", txtEProductName.Text.Trim().ToString(), txtLProductName.Text.Trim().ToString(), txtProductCountry.Text.Trim().ToString(), txtProductUnit.Text.Trim().ToString(), ddlItemType.SelectedValue.ToString(), txtHasCode.Text.Trim().ToString(), HSCode.ToString(), ddlTypeOfBatchNo.SelectedValue.ToString(), HSSerialNo.ToString(), txtReorderQty.Text.Trim().ToString(), txtCostPrice.Text.Trim().ToString(), txtDesc.Content.Trim().ToString(), txtSalesPrice1.Text.Trim(), txtSalesPrice2.Text.Trim(), txtSalesPrice3.Text.Trim(), txtProductColor.Text.Trim(), txtWholesalePrice.Text.Trim(), "ReseverQty", txtDamageQty.Text.Trim(), txtExpQty.Text.Trim().ToString(), txtMaxQty.Text.Trim().ToString(), txtMiniQty.Text.Trim().ToString(), txtprofit.Text.Trim().ToString(), txtDiscount.Text.Trim().ToString(), ddlMaintainStock.SelectedValue.ToString(), txtProductyWarranty.Text, txtActualWeight.Text, txtWeightUnit.Text, txtLenth.Text.Trim().ToString(), txtHeight.Text.Trim().ToString(), txtDepth.Text.Trim().ToString(), txtAlterId1.Text.Trim().ToString(), txtAlterId2.Text.Trim().ToString(), txtAlterId3.Text.Trim().ToString(), strDiscontinueReason.Trim(), txtManufactururCode.Text, chkVerify.Checked.ToString(), txtSalesCommission.Text, txtTechnicalCommission.Text, true.ToString(), DateTime.Now.ToString(), IsActive.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), txtDeveloperCommission.Text, hdnProjectId.Value, currencyId, txtPrefixName.Text, txtSuffixName.Text, txtSnoStartFrom.Text, hdnSizeId.Value, hdnColourId.Value, false.ToString(), ref trns);
                if (txtRackName.Text.Trim() != "")
                {
                    ObjRackDetail.InsertRackDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtRackName.Text.Split('/')[1].ToString(), b.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                }

                ObjProductMaster.InsertProductLabelInfo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), txtLabelProductName.Text.ToString(), txtLabelProductDescription.Text.ToString(), ref trns);

                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "6", b.ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());




                bInsert = true;
            }
            else
            {
                b = ObjProductMaster.UpdateProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value.ToString(), txtProductId.Text.Trim().ToString().ToUpper(), chkIsResouces.Checked.ToString(), hdnModelId.Value, "0", txtEProductName.Text.Trim().ToString(), txtLProductName.Text.Trim().ToString(), txtProductCountry.Text.Trim().ToString(), txtProductUnit.Text.Trim().ToString(), ddlItemType.SelectedValue.ToString(), txtHasCode.Text.Trim().ToString(), HSCode.ToString(), ddlTypeOfBatchNo.SelectedValue.ToString(), HSSerialNo.ToString(), txtReorderQty.Text.Trim().ToString(), txtCostPrice.Text.Trim().ToString(), txtDesc.Content.Trim().ToString(), txtSalesPrice1.Text.Trim(), txtSalesPrice2.Text.Trim(), txtSalesPrice3.Text.Trim(), txtProductColor.Text.Trim(), txtWholesalePrice.Text.Trim(), "ReseverQty", txtDamageQty.Text.Trim(), txtExpQty.Text.Trim().ToString(), txtMaxQty.Text.Trim().ToString(), txtMiniQty.Text.Trim().ToString(), txtprofit.Text.Trim().ToString(), txtDiscount.Text.Trim().ToString(), ddlMaintainStock.SelectedValue.ToString(), txtProductyWarranty.Text, txtActualWeight.Text, txtWeightUnit.Text, txtLenth.Text.Trim().ToString(), txtHeight.Text.Trim().ToString(), txtDepth.Text.Trim().ToString(), txtAlterId1.Text.Trim().ToString(), txtAlterId2.Text.Trim().ToString(), txtAlterId3.Text.Trim().ToString(), strDiscontinueReason.Trim(), txtManufactururCode.Text, chkVerify.Checked.ToString(), txtSalesCommission.Text, txtTechnicalCommission.Text, true.ToString(), DateTime.Now.ToString(), IsActive.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), txtDeveloperCommission.Text, hdnProjectId.Value, currencyId, txtPrefixName.Text, txtSuffixName.Text, txtSnoStartFrom.Text, hdnSizeId.Value, hdnColourId.Value, ref trns);

                //Update APIStatus for API on 16-07-2024 By Lokesh
                string strsql = "update Inv_ProductMaster set APIStatus='False' where ProductId='" + hdnProductId.Value + "'";
                objDa.execute_Command(strsql, ref trns);

                ObjProductMaster.InsertProductLabelInfo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value.ToString(), txtLabelProductName.Text.ToString(), txtLabelProductDescription.Text.ToString(), ref trns);
                ObjProductCate.DeleteProductCategory(Session["CompId"].ToString().ToString(), hdnProductId.Value, ref trns);
                ObjCompanyBrand.DeleteProductCompanyBrand(Session["CompId"].ToString().ToString(), hdnProductId.Value, ref trns);
                ObjProductLocation.DeleteProductLocation(Session["CompId"].ToString().ToString(), hdnProductId.Value, ref trns);
                objDa.execute_Command("delete from inv_product_image where productid=" + hdnProductId.Value + "", ref trns);
                ObjProductBrand.DeleteProductBrand(Session["CompId"].ToString().ToString(), hdnProductId.Value, ref trns);
                ObjProductSupplier.DeleteProductSuppliers(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value, ref trns);
                objRelProduct.DeleteRelatedProduct(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, ref trns);
                if (txtRackName.Text.Trim() != "")
                {
                    ObjRackDetail.DeleteRackDetail_By_LocationId_and_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnProductId.Value, ref trns);
                    ObjRackDetail.InsertRackDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtRackName.Text.Split('/')[1].ToString(), hdnProductId.Value, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            if (b != 0)
            {
                string MBrandId = string.Empty;
                string CateId = string.Empty;
                string ProductId = b.ToString();

                if (hdnProductId.Value == "")
                {
                    ProductId = b.ToString();
                }
                else
                {
                    ProductId = hdnProductId.Value.ToString();
                }

                if (ProductId != "")
                {
                    DataTable dtStock = objDa.return_DataTable("Select* from Inv_StockDetail where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + ProductId.ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
                    if (dtStock.Rows.Count > 0)
                    {
                        objDa.execute_Command("Update Inv_StockDetail Set SalesPrice1='" + txtSalesPrice1.Text + "',SalesPrice2='" + txtSalesPrice2.Text + "',SalesPrice3='" + txtSalesPrice3.Text + "' where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + ProductId.ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
                    }
                    else
                    {
                        objDa.execute_Command("Insert Into Inv_StockDetail ([Company_Id],[Brand_Id],[Location_Id],[ProductId],[OpeningBalance],[RackID],[Quantity],[Minimum_Qty],[Maximum_Qty],[ReserveQty],[DamageQty],[BlockedQty],[OrderQty],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate], Finance_Year_Id, SalesPrice1, SalesPrice2, SalesPrice3) Values ('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + ProductId.ToString() + "','0','0','0','0','0','0','0','0','0', '0','0','','','','1','" + DateTime.Now.ToString("yyyy-MM-dd") + "','1','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["FinanceYearId"].ToString() + "','" + txtSalesPrice1.Text + "', '" + txtSalesPrice2.Text + "', '" + txtSalesPrice3.Text + "')");
                    }
                }

                foreach (ListItem li in ChkBrand.Items)
                {
                    if (li.Selected)
                    {
                        ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString().ToString(), ProductId.Trim().ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //Comment By Akshay after Discuss opening stock
                if (Session["Image"] != null)
                {
                    try
                    {
                        byte[] byteVal = new byte[0];
                        ObjProductImage.InsertProductImage(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.Trim().ToString(), byteVal, Session["Image"].ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    catch
                    {

                    }
                }
                foreach (ListItem li in lstProductBrand.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            foreach (ListItem librand in ChkBrand.Items)
                            {
                                if (librand.Selected)
                                {
                                    if (MBrandId == "")
                                    {
                                        MBrandId = li.Value.ToString();
                                    }

                                    ObjProductBrand.InsertProductBrand(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), ProductId.Trim().ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                    // ObjProductBrand.InsertProductBrand(Session["CompId"].ToString().ToString(), librand.Value, ProductId.Trim().ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                //foreach (ListItem li in ChkProductCategory.Items)
                //{
                //    if (li.Selected)
                //    {
                //        try
                //        {
                //            foreach (ListItem librand in ChkBrand.Items)
                //            {
                //                if (librand.Selected)
                //                {
                //                    if (CateId == "")
                //                    {
                //                        CateId = li.Value.ToString();
                //                    }
                //                    ObjProductCate.InsertProductCategory(Session["CompId"].ToString().ToString(), librand.Value, ProductId.Trim().ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }
                //}


                foreach (ListItem li in ChkProductCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            ObjProductCate.InsertProductCategory(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.Trim().ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                //Comment By Akshay After  Descuss  with lokesh and Neeraj sir
                DataTable dtSupplier = (DataTable)Session["dtProductSupplierCode"];
                if (dtSupplier != null)
                {
                    for (int i = 0; i < dtSupplier.Rows.Count; i++)
                    {
                        ObjProductSupplier.InsertProductSuppliers(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.Trim().ToString(), dtSupplier.Rows[i]["Supplier_Id"].ToString(), dtSupplier.Rows[i]["ProductSupplierCode"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                DataTable dtRelProduct = (DataTable)Session["dtRelatedProduct"];
                if (dtRelProduct != null)
                {
                    objRelProduct.DeleteRelatedProduct(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, ref trns);
                    for (int i = 0; i < dtRelProduct.Rows.Count; i++)
                    {
                        objRelProduct.InsertRelatedProduct(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.Trim().ToString(), dtRelProduct.Rows[i]["RelatedProductId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        objRelProduct.InsertRelatedProduct(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), dtRelProduct.Rows[i]["RelatedProductId"].ToString(), ProductId.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //DataTable dt = objda.return_DataTable("Select * From Set_LocationMaster Where Field1 = (Select Field1 From Set_LocationMaster Where Location_Id ='" + Tax_Location + "')");
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    int k = objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), dt.Rows[count]["Location_Id"].ToString(), userdetails[i].ToString(), ddlTaxType.SelectedValue, TaxValue, "", Tax_According, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //}


                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", ProductId.Trim().ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", ProductId.Trim().ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", ProductId.Trim().ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", ProductId.Trim().ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", ProductId.Trim().ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", ProductId.Trim().ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "6", ProductId.Trim().ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                if (hdnProductId.Value == "")
                {

                    if (rdoProductid.SelectedValue.ToString() == "")
                    {

                    }
                    else
                    {
                        ProductCode = objDoc.GetDocumentNoProduct(rdoProductid.SelectedValue.ToString(), Session["CompId"].ToString().ToString(), "11", "24", strModelNo, hdnColourId.Value, hdnSizeId.Value, MBrandId.ToString(), CateId.ToString(), "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                    }
                    if (txtProductId.Text == "")
                    {
                        if (ProductCode == "")
                        {
                            ProductCode = txtProductId.Text;
                        }
                    }
                    else
                    {
                        ProductCode = txtProductId.Text;
                    }
                    if (rdoProductid.SelectedValue.ToString() == "")
                    {

                    }
                    else
                    {
                        if (txtProductId.Text == "" && hdnModelId.Value != "0" && hdnColourId.Value != "0" && hdnSizeId.Value != "0")
                        {
                            ObjProductMaster.UpdateProductIdforModelColourSize(Session["CompId"].ToString(), ProductId, Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }
                        else
                        {
                            ObjProductMaster.UpdateProductId(Session["CompId"].ToString(), ProductId, Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }
                    }
                    DisplayMessage("Record Saved", "green");
                    txtProductId.Focus();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    // if tax system is there then it should redirect on tax entry page

                    if (isTaxEnabled)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active();fnOpenTaxConfigPage();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                }
                else
                {
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
            }
            //here we commit transaction when all data insert and update proper 
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            btnbind_Click(null, null);

            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("POSIntegrationWithPryce", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (ParmValue == "True")
            {
                DataUploadInPOS();
            }

            //Set Default Value On 19-11-2024
            try
            {
                ddlItemType.SelectedValue = "S";
                txtProductUnit.Text = "Pcs/1";
                foreach (ListItem lival in lstProductBrand.Items)
                {
                    lival.Selected = true;
                    break;
                }
                foreach (ListItem lival in ChkProductCategory.Items)
                {
                    lival.Selected = true;
                    break;
                }
            }
            catch (Exception ex)
            {

            }

        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }


    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtProductId.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        FillDataListGrid(1);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void txtProductId_TextChanged(object sender, EventArgs e)
    {
        //hdnProductId.Value = "";
        if (txtProductId.Text != "")
        {
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode='" + ((TextBox)sender).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (hdnProductId.Value != "")
                {
                    dt = new DataView(dt, "ProductId<>" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Product Already Exists");
                        txtProductId.Text = "";
                        return;
                        //Updated by varsha on 10-aug-2014
                        //hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                        //ProductEdit();
                        //end updation
                    }
                    else
                    {
                        DisplayMessage("Product Id Is Already Exits :- Go To Bin Tab");
                        txtProductId.Text = "";
                        txtProductId.Focus();
                        return;
                    }
                }
            }
            else
            {
                try
                {
                    if (txtProductId.Text.Split('-').Length > 1)
                    {
                        DataTable dtModel = new DataView(ObjModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), "True"), "Model_No='" + txtProductId.Text.Split('-')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtModel.Rows.Count > 0)
                        {
                            if (txtProductId.Text.Contains('#'))
                            {
                                try
                                {
                                    txtSalesPrice1.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(CalculateLabelCost(dtModel.Rows[0]["Trans_Id"].ToString(), txtProductId.Text.Split('-')[1].ToString())) * float.Parse(dtModel.Rows[0]["Sales_Price_1"].ToString())).ToString());
                                }
                                catch
                                {

                                }

                                try
                                {
                                    txtSalesPrice2.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(CalculateLabelCost(dtModel.Rows[0]["Trans_Id"].ToString(), txtProductId.Text.Split('-')[1].ToString())) * float.Parse(dtModel.Rows[0]["Sales_Price_2"].ToString())).ToString());
                                }
                                catch
                                {
                                }
                                try
                                {
                                    txtSalesPrice3.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(CalculateLabelCost(dtModel.Rows[0]["Trans_Id"].ToString(), txtProductId.Text.Split('-')[1].ToString())) * float.Parse(dtModel.Rows[0]["Sales_Price_3"].ToString())).ToString());
                                }
                                catch
                                {
                                }
                                try
                                {
                                    txtCostPrice.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), CalculateLabelCost(dtModel.Rows[0]["Trans_Id"].ToString(), txtProductId.Text.Split('-')[1].ToString()));
                                }
                                catch
                                {
                                    txtCostPrice.Text = "0";
                                }
                            }
                            txtModelNo.Text = dtModel.Rows[0]["Model_No"].ToString();
                            txtModelName.Text = dtModel.Rows[0]["Model_Name"].ToString();
                            txtEProductName.Text = dtModel.Rows[0]["Field3"].ToString();
                            hdnModelId.Value = dtModel.Rows[0]["Trans_Id"].ToString();
                            string PartNumber = txtProductId.Text.Split('-')[1].ToString();
                            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), hdnModelId.Value.ToString());
                            for (int i = 0; i < PartNumber.Length; i++)
                            {
                                string srno = (i + 1).ToString();
                                string Charvalue = PartNumber[i].ToString();
                                if (txtProductId.Text.Contains('#'))
                                {
                                    if (i == 1)
                                    {
                                        try
                                        {
                                            string sql = "select width,height,Field1,Field3 from Inv_Model_LabelSize where Model_Id=" + hdnModelId.Value + " and  Field2=" + PartNumber.Split('#')[1].ToString() + "";
                                            dt = objDa.return_DataTable(sql);
                                            if (dt.Rows[0]["Field1"].ToString().Trim() != "")
                                                txtEProductName.Text = txtEProductName.Text + "," + dt.Rows[0]["Field3"].ToString() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm - " + dt.Rows[0]["Field1"].ToString().Trim();
                                            else
                                                txtEProductName.Text = txtEProductName.Text + "," + dt.Rows[0]["Field3"].ToString() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm";
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                if (Charvalue != "0")
                                {
                                    if (txtProductId.Text.Contains('#'))
                                    {
                                        if (Convert.ToInt32(srno) == 2)
                                        {
                                            continue;
                                        }
                                    }
                                    if (Charvalue == "#")
                                    {
                                        break;
                                    }
                                    if (new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString().Trim() != "")
                                    {
                                        if (txtEProductName.Text == "")
                                        {
                                            txtEProductName.Text = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();
                                        }
                                        else
                                        {
                                            txtEProductName.Text = txtEProductName.Text + "," + new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();
                                        }
                                    }
                                }
                            }
                            //get arcwing from model master page
                            GetModelInformation(hdnModelId.Value);
                            //code end 
                        }
                        else
                        {
                            if (hdnProductId.Value.Trim() == "")
                            {
                                string s;
                                s = txtProductId.Text;
                                Reset();
                                txtProductId.Text = s;
                            }
                            txtModelNo.Text = "0";
                            txtModelName.Text = "0";
                        }
                    }
                    else
                    {
                        if (hdnProductId.Value.Trim() == "")
                        {
                            string s;
                            s = txtProductId.Text;
                            Reset();
                            txtProductId.Text = s;
                        }
                        txtModelNo.Text = "0";
                        txtModelName.Text = "0";
                    }
                }
                catch
                {
                    if (hdnProductId.Value.Trim() == "")
                    {
                        string s;
                        s = txtProductId.Text;
                        Reset();
                        txtProductId.Text = s;
                    }
                    txtModelNo.Text = "0";
                    txtModelName.Text = "0";
                }
                txtModelNo.Focus();
            }
            if (txtProductId.Text.Trim() == txtAlterId1.Text.Trim() || txtProductId.Text.Trim() == txtAlterId2.Text.Trim() || txtProductId.Text.Trim() == txtAlterId3.Text.Trim())
            {
                DisplayMessage("Product Id Is Already Exits");
                txtProductId.Text = "";
                txtProductId.Focus();
                return;
            }
        }
        else
        {
            txtModelNo.Text = "0";
            txtModelName.Text = "0";
        }
    }
    public void GetModelInformation(string strModelId)
    {
        DataTable dtModel = new DataTable();
        dtModel = ObjModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), strModelId, "True");
        //this code is created by jitenra upadhyay on 30-10-2015
        if (dtModel.Rows.Count > 0)
        {
            string DirectoryName = string.Empty;
            //code created by jitendra upadhyay 
            //for pulll model info in product master like category , related product ,images and description  .
            //code created on 07-04-2016
            //code start
            if (txtDesc.Content == null || txtDesc.Content == "")
            {
                txtDesc.Content = dtModel.Rows[0]["Description"].ToString();
            }
            try
            {
                if (Session["Image"] == null)
                {
                    string strFilePath = Server.MapPath(Session["CompId"].ToString() + "/Model/" + dtModel.Rows[0]["Field2"].ToString());
                    strFilePath = strFilePath.Replace("Inventory", "CompanyResource");
                    if (File.Exists(strFilePath))
                    {
                        imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + dtModel.Rows[0]["Field2"].ToString();
                        Session["Image"] = dtModel.Rows[0]["Field2"].ToString();
                        if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/"));
                        }
                        File.Copy(strFilePath, strFilePath.Replace("Model", "Product"));
                    }
                }
            }
            catch
            {
            }
            ///select category
            DataTable dtModelCategory = objModelCategory.GetDataModelId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), dtModel.Rows[0]["Trans_Id"].ToString());
            if (dtModelCategory.Rows.Count > 0)
            {
                foreach (ListItem li in ChkProductCategory.Items)
                {
                    if (new DataView(dtModelCategory, "CategoryId=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        li.Selected = true;
                    }
                }
            }
            else
            {
                //DisplayMessage("Please Create Product Category");
                return;
            }
            //for related product 
            if (GvRelatedProduct.Rows.Count == 0)
            {
                DataTable dtrelatedProduct = objInvModelCategoryProduct.SelectModelCategoryProductRow(dtModel.Rows[0]["Trans_Id"].ToString());
                if (dtrelatedProduct.Rows.Count > 0)
                {
                    dtrelatedProduct = dtrelatedProduct.DefaultView.ToTable(true, "ProductId", "ProductCode", "ProductName");
                    dtrelatedProduct.Columns["ProductId"].ColumnName = "RelatedProductId";
                    dtrelatedProduct.Columns["ProductCode"].ColumnName = "RelatedProductCode";
                    dtrelatedProduct.Columns["ProductName"].ColumnName = "RelatedProductName";
                    dtrelatedProduct = dtrelatedProduct.DefaultView.ToTable(true, "RelatedProductId", "RelatedProductCode", "RelatedProductName");
                    GvRelatedProduct.DataSource = dtrelatedProduct;
                    GvRelatedProduct.DataBind();
                    Session["dtRelatedProduct"] = dtrelatedProduct;
                }
            }
            //for get supplier
            DataTable dtModelSupplier = (DataTable)Session["dtProductSupplierCode"];
            DataTable dtSupplier = objModelSupplier.GetModelSuppliersByModelId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), dtModel.Rows[0]["Trans_Id"].ToString());
            dtSupplier.Columns["Supplier_Id"].ColumnName = "Supplier_Id";
            dtSupplier.Columns["Suppliername"].ColumnName = "Name";
            dtSupplier.Columns["ModelSupplierCode"].ColumnName = "ProductSupplierCode";
            foreach (DataRow dr in dtSupplier.Rows)
            {
                if (dtModelSupplier == null)
                {
                    dtModelSupplier = new DataTable();
                    dtModelSupplier.Columns.Add("Supplier_Id");
                    dtModelSupplier.Columns.Add("Name");
                    dtModelSupplier.Columns.Add("ProductSupplierCode");
                }
                else
                {
                    DataTable dttemp = new DataView(dtModelSupplier, "Supplier_Id=" + dr["Supplier_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dttemp.Rows.Count > 0)
                    {
                        continue;
                    }
                }
                dtModelSupplier.Rows.Add(dr["Supplier_Id"].ToString(), dr["Name"].ToString(), "0");
                Session["dtProductSupplierCode"] = dtModelSupplier;
            }
            GridProductSupplierCode.DataSource = dtModelSupplier;
            GridProductSupplierCode.DataBind();
        }
    }
    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        lblImgMessageShow.Text = "";
        TextBox txt = (TextBox)sender;
        int i = 0;
        try
        {
            string[] s = txt.Text.Split('/');
            i = Convert.ToInt32(txt.Text.Split('/')[s.Length - 1].ToString());
        }
        catch
        {
            DisplayMessage("Please select  Model No in suggestion list");
            txt.Text = "";
            txtModelNo.Text = "";
            txtModelName.Text = "";
        }
        DataTable dt = ObjModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), i.ToString(), "True");
        if (dt.Rows.Count != 0)
        {
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString() + "/" + i;
            txtModelName.Text = dt.Rows[0]["Model_Name"].ToString() + "/" + i;
            GetModelInformation(i.ToString());
        }
        else
        {
            DisplayMessage("Please select  Model No in suggestion list");
            txt.Text = "";
            txtModelNo.Text = "";
            txtModelName.Text = "";
        }
    }
    #region // Bin Section Start
    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
    }
    //    int b = 0;
    //    for (int i = 0; i < dtlistbinProduct.Items.Count; i++)
    //    {
    //        CheckBox chb = (CheckBox)(dtlistbinProduct.Items[i].FindControl("chkActive"));
    //        HiddenField hdpid = (HiddenField)(dtlistbinProduct.Items[i].FindControl("hdnChkActive"));
    //        if (chb.Checked)
    //        {
    //            b = ObjProductMaster.RestoreProductMaster(Session["CompId"].ToString().ToString(), hdpid.Value, Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), true.ToString());
    //        }
    //    }
    //    if (b != 0)
    //    {
    //        DisplayMessage("Record Activated");
    //        btnbinbind_Click(null, null);
    //        btnbind_Click(null, null);
    //        try
    //        {
    //            ((DataListItem)((CheckBox)sender).Parent.Parent).FindControl("chkActive").Focus();
    //        }
    //        catch
    //        {
    //            txtbinVal.Focus();
    //        }
    //    }
    //}
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 10;
        FillbinDataListGrid(1);
        btnbinbind.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 10;
        ddlbinFieldName.SelectedIndex = 1;
        ddlbinOption.SelectedIndex = 3;
        txtbinVal.Text = "";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        txtValue.Focus();
        ddlBinBrandSearch.SelectedIndex = 0;
        ddlBinCategorySearch.SelectedIndex = 0;
        FillbinDataListGrid(1);
        txtbinVal.Focus();
    }

    protected void imgBtnbinGrid_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //dtlistbinProduct.Visible = false;
        gvBinProduct.Visible = true;
        //AllPageCode();
        //imgBtnbinDatalist.Visible = true;
        //imgBtnbinGrid.Visible = false;
        txtbinVal.Focus();
        //imgBtnbinDatalist.Focus();
        imgBtnRestore.Visible = true;
        ImgbtnSelectAll.Visible = false;
        FillbinDataListGrid(1);
    }
    protected void imgbtnbinDatalist_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 10;
        //dtlistbinProduct.Visible = true;

        //AllPageCode();
        //imgBtnbinDatalist.Visible = false;
        //imgBtnbinGrid.Visible = true;
        txtbinVal.Focus();
        imgBtnRestore.Visible = false;
        ImgbtnSelectAll.Visible = false;
        //imgBtnbinGrid.Focus();
        FillbinDataListGrid(1);
    }

    protected void lnkbinFirst_Click(object sender, EventArgs e)
    {
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 10;
        DataTable dt = (DataTable)Session["dtProductBin"];
        FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        ViewState["SubSizebin"] = 10;
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
    }
    protected void lnkbinLast_Click(object sender, EventArgs e)
    {
        ViewState["SubSizebin"] = 10;
        DataTable dt = (DataTable)Session["dtProductBin"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
        ViewState["CurrIndexbin"] = index;
        int tot = dt.Rows.Count;
        if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
        {
            FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
        else if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) == 0)
        {
            FillBinDataList(dt, index - 1, Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
        else
        {
            FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
    }
    protected void lnkbinPrev_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtProductBIn"];
        ViewState["SubSizebin"] = 10;
        ViewState["CurrIndexbin"] = Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) - 1;
        if (Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) < 0)
        {
            ViewState["CurrIndexbin"] = 0;
        }
        FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
    }
    protected void lnkbinNext_Click(object sender, EventArgs e)
    {
        ViewState["SubSizebin"] = 10;
        DataTable dt = (DataTable)Session["dtProductBin"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
        int k1 = Convert.ToInt32(ViewState["CurrIndexbin"].ToString());
        ViewState["CurrIndexbin"] = Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) + 1;
        int k = Convert.ToInt32(ViewState["CurrIndexbin"].ToString());
        if (Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) >= index)
        {
            ViewState["CurrIndexbin"] = index;
        }
        int tot = dt.Rows.Count;
        if (k == index)
        {
            if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
            {
                FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
            }
            else
            {
            }
        }
        else if (k < index)
        {
            if (k + 1 == index)
            {
                if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
                {
                    FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                }
                else
                {
                    FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                }
            }
            else
            {
                FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
            }
        }
        else
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
    }
    protected void gvBinProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBinProduct.PageIndex = e.NewPageIndex;
        FillbinDataListGrid(1);
        string temp = string.Empty;
        bool isselcted;
        for (int i = 0; i < gvBinProduct.Rows.Count; i++)
        {
            Label lblconid = (Label)gvBinProduct.Rows[i].FindControl("lblProductId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvBinProduct.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        gvBinProduct.BottomPagerRow.Focus();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinProduct.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvBinProduct.Rows.Count; i++)
        {
            ((CheckBox)gvBinProduct.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvBinProduct.Rows[i].FindControl("lblProductId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvBinProduct.Rows[i].FindControl("lblProductId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvBinProduct.Rows[i].FindControl("lblProductId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
        chkSelAll.Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvBinProduct.Rows[index].FindControl("lblProductId");
        if (((CheckBox)gvBinProduct.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
        ((CheckBox)gvBinProduct.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = (DataTable)Session["dtProductBin"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtProduct.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["ProductId"]))
                {
                    lblSelectedRecord.Text += dr["ProductId"] + ",";
                }
            }
            for (int i = 0; i < gvBinProduct.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvBinProduct.Rows[i].FindControl("lblProductId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvBinProduct.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtProductBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvBinProduct, dtProduct1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;

        foreach (GridViewRow gvrow in gvBinProduct.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked)
            {
                if (ddlrecordType.SelectedIndex == 0)
                {
                    b = ObjProductMaster.RestoreProductMaster(Session["CompId"].ToString().ToString(), ((Label)gvrow.FindControl("lblProductId")).Text.Trim(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), true.ToString());
                    b = 1;
                }
                else
                {
                    //for convert deactive to active
                    objDa.execute_Command("update inv_productmaster set Field1='' ,ModfiedDate=GETDATE(),ModifiedBy='' where ProductId ='" + ((Label)gvrow.FindControl("lblProductId")).Text.Trim() + "'");
                    b = 1;
                }
            }
        }

        if (b != 0)
        {
            btnbind_Click(null, null);
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
            btnbinRefresh_Click(null, null);
        }
        else
        {
            DisplayMessage("Please Select Record");

        }
        imgBtnRestore.Focus();
    }
    #endregion End
    #endregion  //End
    #region Auto Complete Method
    //Country :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Contry(string prefixText, int count, string contextKey)
    {
        CountryMaster ObjContryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtContryAll = ((DataTable)ObjContryMaster.GetCountryMaster()).DefaultView.ToTable(true, "Country_Name");
        string filtertext = "Country_Name like '%" + prefixText + "%'";
        DataTable dtContry = new DataView(dtContryAll, filtertext, "Country_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] filterlist = new string[dtContry.Rows.Count];
        if (dtContry.Rows.Count > 0)
        {
            for (int i = 0; i < dtContry.Rows.Count; i++)
            {
                filterlist[i] = dtContry.Rows[i]["Country_Name"].ToString();
            }
        }
        return filterlist;
    }
    //Country :- End
    //Supplier :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        Ems_Contact_ProductCategory objContactcategory = new Ems_Contact_ProductCategory(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = new DataTable();
        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        //string strSelectedCategory = HttpContext.Current.Session["SelectCategory"].ToString();
        //string strSupplierList = string.Empty;
        //if (strSelectedCategory != "")
        //{
        //  DataTable dtContactCategory=  objContactcategory.GetDateBy_ContactType(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "S");
        //  if (dtContactCategory.Rows.Count > 0)
        //  {
        //      dtContactCategory = new DataView(dtContactCategory, "CategoryId in (" + strSelectedCategory.Substring(0, strSelectedCategory.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Contact_Id");
        //      foreach (DataRow dr in dtContactCategory.Rows)
        //      {
        //          strSupplierList += dr["Contact_Id"].ToString() + ",";
        //      }
        //      if (strSupplierList != "")
        //      {
        //          dtSupplier = new DataView(dtSupplier, "Supplier_Id in (" + strSupplierList.Substring(0, strSupplierList.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //      }
        //  }
        //}
        string filtertext = string.Empty;
        //if (strSupplierList != "" && dtSupplier.Rows.Count > 0)
        //{
        //    dtCon = dtSupplier;
        //}
        //else
        //{
        filtertext = "Name like '%" + prefixText + "%'";
        dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();
        //}
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    //Supplier :- End
    //Unit :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList3(string prefixText, int count, string contextKey)
    {
        Inv_UnitMaster UMM = new Inv_UnitMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        try
        {
            dt = UMM.GetUnitMaster(HttpContext.Current.Session["CompId"].ToString());
            string filtertext = "Unit_Name like '%" + prefixText + "%'";
            dt = new DataView(dt, filtertext, "Unit_Name Asc", DataViewRowState.CurrentRows).ToTable();
            string[] filterlist = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    filterlist[i] = "" + dt.Rows[i]["Unit_Name"].ToString() + "/" + dt.Rows[i]["Unit_Id"].ToString() + "";
                }
            }
            return filterlist;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //Unit :- End
    //Product Name :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        dt = new DataView(dt, "EProductName like '%" + prefixText.ToString() + "%'", "EProductName asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LOcId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (HttpContext.Current.Session["hdnProductId"] != null)
        {
            dt = new DataView(dt, "ProductId<>" + HttpContext.Current.Session["hdnProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        dt = new DataView(dt, "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (HttpContext.Current.Session["hdnProductId"] != null)
        {
            dt = new DataView(dt, "ProductId<>" + HttpContext.Current.Session["hdnProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        dt = new DataView(dt, "EProductName like '%" + prefixText.ToString() + "%'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }
        return txt;
    }
    //Product Name :- End
    //Product Id :-Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductId(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    //Product Id :- End
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListtxtModelName(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjMM = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjMM.GetModelMasterByCompanyId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString()), "Model_Name like '%" + prefixText.ToString() + "%'", "Model_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListtxtModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjMM = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjMM.GetModelMasterByCompanyId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString()), "Model_No like '%" + prefixText.ToString() + "%'", "Model_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }
    //
    #endregion
    #region //User Defined Function Start
    public void FillddlBrandSearch(DropDownList ddl)
    {
        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString().ToString());
        try
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddl, dt, "Brand_Name", "PBrandId");
        }
        catch
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }
    private void FillProductBrand()
    {
        DataTable dsBrand = null;
        dsBrand = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString().ToString());
        if (dsBrand.Rows.Count > 0)
        {
            lstProductBrand.Items.Clear();
            lstSelectedProductBrand.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)lstProductBrand, dsBrand, "Brand_Name", "PBrandId");
        }
    }
    public void FillBrand()
    {
        string strCompanyId = string.Empty;
        strCompanyId = Session["CompId"].ToString().ToString();
        DataTable dt = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "", "Brand_Name Asc", DataViewRowState.CurrentRows).ToTable();
        try
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ChkBrand, dt, "Brand_Name", "Brand_Id");
        }
        catch
        {
        }
        foreach (ListItem li in ChkBrand.Items)
        {
            if (Session["BrandId"].ToString() == li.Value.Trim())
            {
                li.Selected = true;
                li.Enabled = false;
            }
        }
    }
    //Comment by akshay after discuss neeraj and neeraj sir
    private void FillProductCategorySerch(DropDownList ddl)
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddl, dsCategory, "Category_Name", "Category_Id");
        }
        else
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }
    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {
            lstSelectedProductBrand.Items.Clear();
            ChkProductCategory.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ChkProductCategory, dsCategory, "Category_Name", "Category_Id");
        }
        else
        {
            ChkProductCategory.Items.Add("No Category Available Here");
        }
    }
    public string GetUnitId(string Value)
    {
        string UnitId = string.Empty;
        string[] Temp = Value.Split('/');
        if (Temp.Length == 2)
        {
            DataTable dt = ObjUnitMaster.GetUnitMasterById(Session["CompId"].ToString().ToString(), Temp[1].ToString());
            if (dt.Rows.Count != 0)
            {
                UnitId = dt.Rows[0]["Unit_Id"].ToString();
            }
        }
        return UnitId;
    }
    public void FillDataList(DataTable dt, int currentIndex, int SubSize)
    {
        int startRow = currentIndex * SubSize;
        int rowCounter = 0;
        DataTable dtBind = dt.Clone();
        while (rowCounter < SubSize)
        {
            if (startRow < dt.Rows.Count)
            {
                DataRow row = dtBind.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dt.Rows[startRow][dc.ColumnName];
                }
                dtBind.Rows.Add(row);
                startRow++;
            }
            rowCounter++;
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)dtlistProduct, dtBind, "", "");
        //AllPageCode();
    }
    public string GetProductStock(string strProductId)
    {
        string SysQty = string.Empty;
        try
        {
            SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
        }
        catch
        {
            SysQty = "0";
        }
        if (SysQty == "")
        {
            SysQty = "0.000";
        }
        return setDecimal(SysQty);
    }
    private void FillDataListGrid(int pageIndex)
    {
        string strcategoryId = "0";
        string strBrandId = "0";
        //ViewState["SearchPage"] = null;
        //ViewState["dtSearch"] = null;
        int RecordCount = 0;
        DataTable dtProduct = new DataTable();
        if (ddlcategorysearch.SelectedIndex > 0)
        {
            strcategoryId = ddlcategorysearch.SelectedValue;
        }
        if (ddlbrandsearch.SelectedIndex > 0)
        {
            strBrandId = ddlbrandsearch.SelectedValue;
        }
        string strsearchProductId = "0";
        if (txtSearchPrduct.Text.Trim() != "")
        {
            DataTable dtsearchProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchPrduct.Text.Trim());
            if (dtsearchProduct != null)
            {
                if (dtsearchProduct.Rows.Count > 0)
                {
                    strsearchProductId = dtsearchProduct.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    strsearchProductId = "0";
                    //    dtproduct = new DataView(dtproduct, "ProductId in ('0')", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }


        string strStockQty = string.Empty;
        if (chkAvailableStock.Checked == true)
        {
            strStockQty = "True";
        }
        else if (chkAvailableStock.Checked == false)
        {
            strStockQty = "False";
        }

        try
        {
            dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, chkDiscontinue.Checked.ToString(), txtSearchPrduct.Text.Trim(), GetFilterString(), "0", strStockQty, true.ToString(), "2", true.ToString());
            RecordCount = Convert.ToInt32(dtProduct.Rows[0][0].ToString());
            ViewState["RecordCount"] = RecordCount;
            pageIndex = Convert.ToInt32(Session["pageIndex"]);
            if (pageIndex == 0)
                pageIndex = 1;
        }
        catch (Exception ex)
        {
            pageIndex = 1;
        }

        PopulatePager(RecordCount, pageIndex);
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + RecordCount.ToString();
        //strsql= ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, chkDiscontinue.Checked.ToString(), "", GetFilterString(), "0", "", false.ToString(), "2");


        try
        {
            dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, chkDiscontinue.Checked.ToString(), txtSearchPrduct.Text.Trim(), GetFilterString(), "0", strStockQty, false.ToString(), "2", true.ToString());
        }
        catch (Exception ex)
        {

        }

        //chkAvailableStock.Checked = true;
        //if (chkAvailableStock.Checked == true)
        //{
        //    dtProduct = new DataView(dtProduct, "StockQty <> '0.000'", "", DataViewRowState.CurrentRows).ToTable();
        //}


        try
        {
            if (imbBtnGrid.Visible == true)
            {
                objPageCmn.FillData((object)dtlistProduct, dtProduct, "", "");
            }
            else
            {
                objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
            }
        }
        catch
        {

        }


        try
        {
            dtProduct.Dispose();
        }
        catch
        {

        }
        //AllPageCode();
        txtValue.Focus();
    }
    public DataTable SelectTopDataRow(DataTable dt, int count)
    {
        DataTable dtn = dt.Clone();
        for (int i = 0; i < count; i++)
        {
            dtn.ImportRow(dt.Rows[i]);
        }
        return dtn;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void Reset()
    {
        lblImgMessageShow.Text = "";
        chkserialNo.Checked = false;
        pnldescdetail.Visible = true;
        lnkAddExp.Visible = true;
        txtProductId.Text = "";
        txtModelNo.Text = "";
        txtPartNo.Text = "";
        txtAlterId1.Text = "";
        txtAlterId2.Text = "";
        txtAlterId3.Text = "";
        txtCostPrice.Text = "";
        txtDamageQty.Text = "";
        txtDepth.Text = "";
        txtDesc.Content = "";
        txtDiscount.Text = "";
        txtEProductName.Text = "";
        txtExpQty.Text = "";
        chkIsResouces.Checked = false;
        txtHasCode.Text = "";
        txtHeight.Text = "";
        txtLenth.Text = "";
        txtRackName.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtLProductName.Text = "";
        txtMaxQty.Text = "";
        txtMiniQty.Text = "";
        txtProductId.Enabled = true;
        txtProductColor.Text = "";
        txtSalesCommission.Text = "0";
        txtTechnicalCommission.Text = "0";
        txtProductSupplierCode.Text = "";
        txtProductUnit.Text = "";
        txtprofit.Text = "";
        txtReorderQty.Text = "";
        txtSalesPrice1.Text = "";
        txtSalesPrice2.Text = "";
        txtSalesPrice3.Text = "";
        txtSuppliers.Text = "";
        txtWholesalePrice.Text = "";
        ddlItemType.SelectedIndex = 0;
        ddlItypeType_SelectedIndexChanged(null, null);
        ddlMaintainStock.SelectedIndex = 0;
        ddlstatus.SelectedIndex = 0;
        ddlTypeOfBatchNo.SelectedIndex = 0;
        ddlMaintainStock_SelectedIndexChanged(null, null);
        lstSelectedProductBrand.Items.Clear();
        lstSelectProductCategory.Items.Clear();
        FillBrand();
        FillProductCategory();
        FillProductBrand();
        txtDiscontinueReason.Text = "";
        GridProductSupplierCode.DataSource = null;
        GridProductSupplierCode.DataBind();
        Session["dtProductSupplierCode"] = null;
        Session["dtRelatedProduct"] = null;
        imgProduct.ImageUrl = null;
        hdnProductId.Value = "";
        txtProductId.Enabled = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ChkHasBatchNo.Checked = false;
        ChkHasSerialNo.Checked = false;
        FillbinDataListGrid(1);
        TabContainer1.ActiveTabIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["Image"] = null;
        txtModelName.Text = "";
        GvRelatedProduct.DataSource = null;
        GvRelatedProduct.DataBind();
        txtERelatedProduct.Text = "";
        Session["hdnProductId"] = null;
        gvStockLocation.DataSource = null;
        gvStockLocation.DataBind();
        rdoProductid.SelectedIndex = -1;
        btnSave.Enabled = true;
        hdnModelId.Value = "0";
        trdiscontinuereason.Visible = false;
        txtDiscontinueReason.Text = "";
        txtManufactururCode.Text = "";
        txtCostPrice.Visible = true;
        GridProductSupplierCode.Visible = true;
        txtWeightUnit.Text = "";
        txtActualWeight.Text = "";
        txtProductUnit.Enabled = true;
        txtProductyWarranty.Text = "0";
        GetLocationCountry();
        txtDeveloperCommission.Text = "";
        txtProjectName.Text = "";
        hdnProjectId.Value = "";

        txtPrefixName.Text = txtSuffixName.Text = txtSnoStartFrom.Text = string.Empty;
        ddlItemType.Enabled = true;
    }
    public void ProductEdit()
    {
        txtProductId.Enabled = true;
        TabContainer1.ActiveTabIndex = 0;
        txtSalesCommission.Text = "0";
        txtTechnicalCommission.Text = "0";
        txtProductId.Text = hdnProductId.Value.ToString();
        DataTable DtProductImage = ObjProductImage.GetProductImageById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
        DataTable dtProduct = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtProductLabel = ObjProductMaster.GetProductLabelMasterInfo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
        if (dtProduct.Rows.Count != 0)
        {
            if (DtProductImage.Rows.Count > 0)
            {
                //string strFileapth = Server.MapPath(DtProductImage.Rows[0]["Field1"].ToString().Trim());

                //string strFilePath = Server.MapPath(Session["CompId"].ToString() + "/Product/" + DtProductImage.Rows[0]["Field1"].ToString().Trim());
                //strFilePath = strFilePath.Replace("Inventory", "CompanyResource");

                //strFileapth = "../CompanyResource/" + Session["CompId"].ToString() + "/Product/" + DtProductImage.Rows[0]["Field1"].ToString().Trim();
                //strFileapth = strFileapth.Replace("Inventory", "Temp");
                try
                {
                    string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));

                    imgProduct.ImageUrl = "~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "/Product/" + DtProductImage.Rows[0]["Field1"].ToString();
                    //imgProduct.ImageUrl = "~/Handler.ashx?ImID=" + hdnProductId.Value.ToString() + "";
                    Session["Image"] = DtProductImage.Rows[0]["Field1"].ToString();
                }
                catch (Exception ex)
                {
                    imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + DtProductImage.Rows[0]["Field1"].ToString();
                    //imgProduct.ImageUrl = "~/Handler.ashx?ImID=" + hdnProductId.Value.ToString() + "";
                    Session["Image"] = DtProductImage.Rows[0]["Field1"].ToString();
                }
            }
            else
            {
                Session["Image"] = null;
            }
            try
            {
                chkIsResouces.Checked = Convert.ToBoolean(dtProduct.Rows[0]["PartNo"].ToString());
            }
            catch
            {
                chkIsResouces.Checked = false;
            }
            txtProductId.Text = dtProduct.Rows[0]["ProductCode"].ToString();
            //if (Session["EmpId"].ToString() == "0")
            //{
            //    txtProductId.Enabled = true;
            //}
            //else
            //{
            //    DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), Session["AccordianId"].ToString(), "24");
            //    if (new DataView(dtAllPageCode, "Op_Id=13", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            //    {
            //        txtProductId.Enabled = false;
            //    }
            //    else
            //    {
            //        txtProductId.Enabled = true;
            //    }
            //}
            DataTable dtRackInfo = ObjRackDetail.SelectRowRackDetailProduct(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["LocId"].ToString());
            if (dtRackInfo.Rows.Count > 0)
            {
                txtRackName.Text = dtRackInfo.Rows[0]["Rack_Name"].ToString() + "/" + dtRackInfo.Rows[0]["Rack_Id"].ToString();
            }
            else
            {
                txtRackName.Text = "";
            }

            try
            {
                if (dtProductLabel.Rows.Count > 0)
                {
                    txtLabelProductName.Text = dtProductLabel.Rows[0]["L_ProductName"].ToString();
                    txtLabelProductDescription.Text = dtProductLabel.Rows[0]["L_ProductDescription"].ToString();

                }
                else
                {
                    txtLabelProductName.Text = "";
                    txtLabelProductDescription.Text = "";


                }
            }
            catch
            {
                txtLabelProductName.Text = "";
                txtLabelProductDescription.Text = "";
            }
            txtProductyWarranty.Text = dtProduct.Rows[0]["URL"].ToString();
            txtEProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
            txtLProductName.Text = dtProduct.Rows[0]["LProductName"].ToString();
            txtAlterId1.Text = dtProduct.Rows[0]["AlternateId1"].ToString();
            txtAlterId2.Text = dtProduct.Rows[0]["AlternateId2"].ToString();
            txtAlterId3.Text = dtProduct.Rows[0]["AlternateId3"].ToString();
            txtCostPrice.Text = dtProduct.Rows[0]["CostPrice"].ToString();
            txtDamageQty.Text = dtProduct.Rows[0]["DamageQty"].ToString();
            txtDepth.Text = Convert.ToDouble(dtProduct.Rows[0]["DimDepth"].ToString()).ToString("0.0");
            txtDesc.Content = dtProduct.Rows[0]["Description"].ToString();
            txtDiscount.Text = dtProduct.Rows[0]["Discount"].ToString();
            txtExpQty.Text = dtProduct.Rows[0]["ExpiredQty"].ToString();
            txtHasCode.Text = dtProduct.Rows[0]["HScode"].ToString();
            txtHeight.Text = Convert.ToDouble(dtProduct.Rows[0]["DimHieght"].ToString()).ToString("0.0");
            txtLenth.Text = Convert.ToDouble(dtProduct.Rows[0]["DimLenth"].ToString()).ToString("0.0");
            txtMaxQty.Text = dtProduct.Rows[0]["MaximumQty"].ToString();
            txtMiniQty.Text = dtProduct.Rows[0]["MinimumQty"].ToString();
            txtDeveloperCommission.Text = dtProduct.Rows[0]["developerCommission"].ToString();
            hdnProjectId.Value = dtProduct.Rows[0]["projectId"].ToString();
            txtProjectName.Text = dtProduct.Rows[0]["Project_Name"].ToString();
            //this code is created by jitendra upadhyay to get updated average stock in cost price textbox
            //created on 29-09-2016
            try
            {
                txtCostPrice.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), hdnProductId.Value).Rows[0]["Field2"].ToString());
            }
            catch
            {
                txtCostPrice.Text = dtProduct.Rows[0]["CostPrice"].ToString();
            }
            //try
            //{
            //    chkVerify.Checked = Convert.ToBoolean(dtProduct.Rows[0]["Field3"].ToString());
            //}
            //catch
            //{
            //    chkVerify.Checked = false;
            //}
            txtPartNo.Text = dtProduct.Rows[0]["PartNo"].ToString();
            txtProductColor.Text = dtProduct.Rows[0]["ProductColor"].ToString();
            string ModelId = string.Empty;
            if (dtProduct.Rows[0]["ModelNo"].ToString() == "")
            {
                ModelId = "0";
            }
            else
            {
                ModelId = dtProduct.Rows[0]["ModelNo"].ToString();
            }
            DataTable dtModel = ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, "True");
            if (dtModel.Rows.Count != 0)
            {
                txtModelName.Text = dtModel.Rows[0]["Model_Name"].ToString();
                txtModelNo.Text = dtModel.Rows[0]["Model_No"].ToString();
                hdnModelId.Value = dtModel.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtModelName.Text = "0";
                txtModelNo.Text = "0";
            }


            try
            {
                txtProductCountry.Text = new DataView(ObjSysCountryMaster.GetCountryMaster(), "Country_Id='" + dtProduct.Rows[0]["CountryID"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Country_Name"].ToString();
            }
            catch
            {

            }
            //here we checking that any stock transaction is exist on not for this prodcut 
            //if exist then we can not edit and diable unit textbox
            //code created by jitendra upadhyay on 29-09-2016
            DataTable dtStock = objStockDetail.GetStockDetailByProductId(hdnProductId.Value);
            if (dtStock != null)
            {
                if (dtStock.Rows.Count > 0)
                {
                    txtProductUnit.Enabled = false;
                }
                else
                {
                    txtProductUnit.Enabled = true;
                }
            }
            else
            {
                txtProductUnit.Enabled = true;
            }

            try
            {
                txtProductUnit.Text = ObjUnitMaster.GetUnitMasterById(Session["CompId"].ToString().ToString(), dtProduct.Rows[0]["UnitId"].ToString()).Rows[0]["Unit_Name"].ToString() + "/" + dtProduct.Rows[0]["UnitId"].ToString();
            }
            catch
            {

            }

            //Colour and Size work.
            try
            {
                txtColour.Text = ObjColorMaster.GetColorMasterById(Session["CompId"].ToString().ToString(), dtProduct.Rows[0]["ColourId"].ToString()).Rows[0]["Color_Name"].ToString() + "/" + dtProduct.Rows[0]["ColourId"].ToString();
            }
            catch
            {

            }

            try
            {
                txtSize.Text = ObjSizeMaster.GetSizeMasterById(Session["CompId"].ToString().ToString(), dtProduct.Rows[0]["SizeId"].ToString()).Rows[0]["Size_Name"].ToString() + "/" + dtProduct.Rows[0]["SizeId"].ToString();
            }
            catch
            {

            }




            try
            {
                txtWeightUnit.Text = ObjUnitMaster.GetUnitMasterById(Session["CompId"].ToString().ToString(), dtProduct.Rows[0]["WeighUnitID"].ToString()).Rows[0]["Unit_Name"].ToString() + "/" + dtProduct.Rows[0]["WeighUnitID"].ToString();
            }
            catch
            {

            }

            txtActualWeight.Text = dtProduct.Rows[0]["ActualWeight"].ToString();
            txtprofit.Text = dtProduct.Rows[0]["Profit"].ToString();
            string Exchangerate = string.Empty;
            try
            {
                Exchangerate = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
            }
            catch
            {
                Exchangerate = "1";
            }
            //try
            //{
            //    txtSalesPrice1.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(dtProduct.Rows[0]["SalesPrice1"].ToString()) * Convert.ToDouble(Exchangerate)).ToString());
            //}
            //catch
            //{
            //    txtSalesPrice1.Text = "0";
            //}
            //try
            //{
            //    txtSalesPrice2.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(dtProduct.Rows[0]["SalesPrice2"].ToString()) * Convert.ToDouble(Exchangerate)).ToString());
            //}
            //catch
            //{
            //    txtSalesPrice2.Text = "0";
            //}
            //try
            //{
            //    txtSalesPrice3.Text = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(dtProduct.Rows[0]["SalesPrice3"].ToString()) * Convert.ToDouble(Exchangerate)).ToString());
            //}
            //catch
            //{
            //    txtSalesPrice3.Text = "0";
            //}
            try
            {
                DataTable dtSalesPrice = objDa.return_DataTable("Select * from Inv_StockDetail WHERE ProductId = " + hdnProductId.Value + " And Company_Id='" + Session["CompId"].ToString() + "' And Brand_Id='" + Session["BrandId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' ");
                if (dtSalesPrice.Rows.Count > 0)
                {
                    txtSalesPrice1.Text = dtSalesPrice.Rows[0]["SalesPrice1"].ToString();
                    txtSalesPrice2.Text = dtSalesPrice.Rows[0]["SalesPrice2"].ToString();
                    txtSalesPrice3.Text = dtSalesPrice.Rows[0]["SalesPrice3"].ToString();
                }
                else
                {
                    txtSalesPrice1.Text = "0.00";
                    txtSalesPrice2.Text = "0.00";
                    txtSalesPrice3.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                txtSalesPrice1.Text = "0.00";
                txtSalesPrice2.Text = "0.00";
                txtSalesPrice3.Text = "0.00";
            }

            txtWholesalePrice.Text = dtProduct.Rows[0]["WSalePrice"].ToString();
            txtSalesCommission.Text = dtProduct.Rows[0]["Field4"].ToString();
            txtTechnicalCommission.Text = dtProduct.Rows[0]["Field5"].ToString();
            ddlItemType.SelectedValue = dtProduct.Rows[0]["ItemType"].ToString();
            //check that if transaction exit in database for selected prodcutId then use can't change item type - 17-Mar-2020
            try
            {
                ddlItemType.Enabled = true;
                if (int.Parse(objDa.get_SingleValue("select COUNT(*) from inv_productledger where IsActive='true' and company_id='" + Session["CompId"].ToString() + "' and ProductId='" + hdnProductId.Value + "'")) > 0)
                {
                    ddlItemType.Enabled = false;
                }
            }
            catch (Exception ex)
            {

            }



            ddlItypeType_SelectedIndexChanged(null, null);
            try
            {
                ddlMaintainStock.SelectedValue = dtProduct.Rows[0]["MaintainStock"].ToString();
                if (ddlMaintainStock.SelectedValue == "SNO")
                {
                    chkserialNo.Checked = true;
                }
                else
                {
                    chkserialNo.Checked = false;
                }
            }
            catch
            {
                chkserialNo.Checked = false;
                ddlMaintainStock.SelectedIndex = 0;
            }
            ddlMaintainStock_SelectedIndexChanged(null, null);
            txtReorderQty.Text = dtProduct.Rows[0]["ReorderQty"].ToString();
            ddlstatus.SelectedValue = "True";
            if (dtProduct.Rows[0]["Field1"].ToString() != "")
            {
                ddlstatus.SelectedIndex = 2;
                ddlstatus_OnSelectedIndexChanged(null, null);
                txtDiscontinueReason.Text = dtProduct.Rows[0]["Field1"].ToString();
            }
            txtManufactururCode.Text = dtProduct.Rows[0]["Field2"].ToString();
            ddlTypeOfBatchNo.SelectedValue = dtProduct.Rows[0]["TypeOfBatchNo"].ToString();
            ChkHasBatchNo.Checked = Convert.ToBoolean(dtProduct.Rows[0]["HasBatchNo"].ToString());
            ChkHasSerialNo.Checked = Convert.ToBoolean(dtProduct.Rows[0]["HasSerialNo"].ToString());
            DataTable dtProductBrand = ObjProductBrand.GetDataProductId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
            for (int i = 0; i < dtProductBrand.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dtProductBrand.Rows[i]["PBrandId"].ToString();
                try
                {
                    li.Text = ObjProductBrandMaster.GetProductBrandByPBrandId(Session["CompId"].ToString().ToString(), dtProductBrand.Rows[i]["PBrandId"].ToString()).Rows[0]["Brand_Name"].ToString();
                }
                catch
                {
                }
                //lstSelectedProductBrand.Items.Add(li);
                //lstProductBrand.Items.Remove(li);
                foreach (ListItem lival in lstProductBrand.Items)
                {
                    if (lival.Value == li.Value)
                    {
                        lival.Selected = true;
                        break;
                    }
                }
            }
            DataTable dtProductCate = ObjProductCate.GetDataProductId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
            for (int i = 0; i < dtProductCate.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dtProductCate.Rows[i]["CategoryId"].ToString();
                li.Text = ObjProductCateMaster.GetProductCategoryByCategoryId(Session["CompId"].ToString().ToString(), dtProductCate.Rows[i]["CategoryId"].ToString()).Rows[0]["Category_Name"].ToString();
                //lstSelectProductCategory.Items.Add(li);
                //lstProductCategory.Items.Remove(li);
                foreach (ListItem lival in ChkProductCategory.Items)
                {
                    if (lival.Value == li.Value)
                    {
                        lival.Selected = true;
                        break;
                    }
                }
            }
            DataTable dtCompBrand = ObjCompanyBrand.GetDataProductId(Session["CompId"].ToString().ToString(), hdnProductId.Value);
            for (int i = 0; i < dtCompBrand.Rows.Count; i++)
            {
                foreach (ListItem chklst in ChkBrand.Items)
                {
                    if (dtCompBrand.Rows[i]["BrandId"].ToString() == chklst.Value.ToString())
                    {
                        chklst.Selected = true;
                    }
                }
            }
            DataTable dtSupplier = ObjProductSupplier.GetProductSuppliersByProductId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
            DataTable dt = new DataTable();
            dt.Columns.Add("Supplier_Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("ProductSupplierCode");
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                dt.Rows.Add(i);
                dt.Rows[i]["Supplier_Id"] = dtSupplier.Rows[i]["Supplier_Id"].ToString();
                dt.Rows[i]["Name"] = dtSupplier.Rows[i]["Name"].ToString();
                dt.Rows[i]["ProductSupplierCode"] = dtSupplier.Rows[i]["ProductSupplierCode"].ToString();
            }
            Session["dtProductSupplierCode"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GridProductSupplierCode, dtSupplier, "", "");
            DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
            DataTable dtGvRelProduct = new DataTable();
            dtGvRelProduct.Columns.Add("RelatedProductId");
            dtGvRelProduct.Columns.Add("RelatedProductCode");
            dtGvRelProduct.Columns.Add("RelatedProductName");
            for (int i = 0; i < dtRelatedProduct.Rows.Count; i++)
            {
                dtGvRelProduct.Rows.Add(i);
                dtGvRelProduct.Rows[i]["RelatedProductId"] = dtRelatedProduct.Rows[i]["SubProduct_Id"].ToString();
                dtGvRelProduct.Rows[i]["RelatedProductCode"] = dtRelatedProduct.Rows[i]["ProductCode"].ToString();
                dtGvRelProduct.Rows[i]["RelatedProductName"] = dtRelatedProduct.Rows[i]["EProductName"].ToString();
            }
            Session["dtRelatedProduct"] = dtGvRelProduct;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvRelatedProduct, dtGvRelProduct, "", "");
            Session["hdnProductId"] = hdnProductId.Value;
            FillStockLocation();
            if (hdnModelId.Value != "")
            {
                GetModelInformation(hdnModelId.Value);
            }

            txtPrefixName.Text = dtProduct.Rows[0]["SnoPrefix"].ToString();
            txtSuffixName.Text = dtProduct.Rows[0]["SnoSuffix"].ToString();
            txtSnoStartFrom.Text = dtProduct.Rows[0]["SnoStartFrom"].ToString();
        }
    }
    public string getModelNo(string ModelId)
    {
        string modelNo = string.Empty;
        if (ModelId == "")
        {
            ModelId = "0";
        }
        DataTable dtModel = ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, "True");
        if (dtModel.Rows.Count != 0)
        {
            modelNo = dtModel.Rows[0]["Model_No"].ToString();
        }
        return modelNo;
    }
    public void FillStockLocation()
    {
        string strCompanyId = string.Empty;
        strCompanyId = Session["CompId"].ToString().ToString();
        DataTable dt = objStockDetail.GetStockDetail(Session["CompId"].ToString(), hdnProductId.Value.Trim(), Session["FinanceYearId"].ToString());
        try
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvStockLocation, dt, "", "");
        }
        catch
        {
        }
    }
    #region For Bin
    public void FillBinDataList(DataTable dt, int currentIndex, int SubSize)
    {
        int startRow = currentIndex * SubSize;
        int rowCounter = 0;
        DataTable dtBind = dt.Clone();
        while (rowCounter < SubSize)
        {
            if (startRow < dt.Rows.Count)
            {
                DataRow row = dtBind.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dt.Rows[startRow][dc.ColumnName];
                }
                dtBind.Rows.Add(row);
                startRow++;
            }
            rowCounter++;
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //objPageCmn.FillData((object)dtlistbinProduct, dtBind, "", "");
        //AllPageCode();
        if (gvBinProduct.Visible != true)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
    }

    protected void ddlrecordType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillbinDataListGrid(0);
    }

    private void FillbinDataListGrid(int pageIndex)
    {

        string strWhere = string.Empty;
        DataTable dt = objDa.return_DataTable("SELECT inv_productmaster.ProductId, inv_productmaster.ProductCode, inv_productmaster.EProductName, Inv_ModelMaster.Model_no as ModelNo, CASE WHEN ItemType = 's' THEN 'Stockable' ELSE 'Non Stockable' END AS ItemTypeValue, Inv_UnitMaster.Unit_Name AS UnitName, CASE WHEN EMP_CREATED.User_Id = '0' THEN 'Superadmin' ELSE SUBSTRING(EMP_CREATED.Emp_Name, 0, 17) END AS CreatedEmpName, CASE WHEN EMP_MODIFIED.User_Id = '0' THEN 'Superadmin' ELSE SUBSTRING(EMP_MODIFIED.Emp_Name, 0, 17) END AS ModifiedEmpName, Inv_Product_Category.categoryid, Inv_Product_Brand.Brand_Id, inv_productmaster.field1, inv_productmaster.isactive FROM inv_productmaster LEFT JOIN Inv_ModelMaster ON inv_productmaster.ModelNo = Inv_ModelMaster.Trans_Id LEFT JOIN inv_unitmaster ON inv_productmaster.UnitId = Inv_UnitMaster.Unit_Id LEFT JOIN (SELECT Set_UserMaster.Emp_Id, set_employeemaster.Emp_Name, Set_UserMaster.User_Id FROM Set_UserMaster INNER JOIN set_employeemaster ON Set_UserMaster.Emp_Id = set_employeemaster.Emp_Id) EMP_CREATED ON inv_productmaster.CreatedBy = EMP_CREATED.User_Id LEFT JOIN (SELECT Set_UserMaster.Emp_Id, set_employeemaster.Emp_Name, Set_UserMaster.User_Id FROM Set_UserMaster INNER JOIN set_employeemaster ON Set_UserMaster.Emp_Id = set_employeemaster.Emp_Id) EMP_MODIFIED ON inv_productmaster.ModifiedBy = EMP_MODIFIED.User_Id LEFT JOIN Inv_Product_Category ON inv_productmaster.productid = inv_product_category.productid LEFT JOIN Inv_Product_Brand ON inv_productmaster.productid = Inv_Product_Brand.productid WHERE Inv_ProductMaster.Brand_Id = " + Session["BrandId"].ToString() + " AND (Inv_ProductMaster.IsActive = 'False' OR inv_productmaster.Field1 <> '')");

        if (dt.Rows.Count == 0)
        {
            objPageCmn.FillData((object)gvBinProduct, null, "", "");
            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : 0";
            return;
        }

        if (ddlrecordType.SelectedValue == "InActive")
        {

            strWhere += "IsActive='False'";
        }
        else if (ddlrecordType.SelectedValue == "Discontinue")
        {
            strWhere += "Field1<>''";
        }
        DataTable dtProduct = new DataTable();
        if (ddlBinCategorySearch.SelectedIndex > 0)
        {
            strWhere = strWhere + " and categoryid='" + ddlBinCategorySearch.SelectedValue + "'";
            //strcategoryId = ddlBinCategorySearch.SelectedValue;
        }
        if (ddlBinBrandSearch.SelectedIndex > 0)
        {
            strWhere = strWhere + " and Brand_Id='" + ddlBinBrandSearch.SelectedValue + "'";
            //strBrandId = ddlBinBrandSearch.SelectedValue;
        }

        if (txtbinVal.Text != "")
        {
            strWhere = strWhere + " and " + ddlbinFieldName.SelectedValue + "='" + txtbinVal.Text + "'";

        }

        dtProduct = new DataView(dt, "" + strWhere + "", "", DataViewRowState.CurrentRows).ToTable(true, "ProductId", "ProductCode", "EProductName", "ModelNo", "ItemTypeValue", "UnitName", "CreatedEmpName", "ModifiedEmpName");

        lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtProduct.Rows.Count.ToString();
        //string strsearchProductId = "0";
        //dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "0", pageIndex.ToString(), strBrandId, strcategoryId, false.ToString(), "", GetFilterString_ForBin(), "0", "", true.ToString(), "2", false.ToString());
        //RecordCount = Convert.ToInt32(dtProduct.Rows[0][0].ToString());
        //ViewState["BinRecordCount"] = RecordCount;
        ////PopulatePagerBin(RecordCount, pageIndex);

        ////strsql= ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, chkDiscontinue.Checked.ToString(), "", GetFilterString(), "0", "", false.ToString(), "2");
        //dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "0", pageIndex.ToString(), strBrandId, strcategoryId, false.ToString(), "", GetFilterString_ForBin(), "0", "", false.ToString(), "2", false.ToString());
        ////if (imgBtnbinGrid.Visible == true)
        ////{
        ////    objPageCmn.FillData((object)dtlistbinProduct, dtProduct, "", "");
        ////}
        ////else
        ////{
        objPageCmn.FillData((object)gvBinProduct, dtProduct, "", "");
        //}
        //Session["dtProductBin"] = dtProduct;
        dtProduct.Dispose();
        txtbinVal.Focus();
        //ViewState["SearchPage"] = null;
        //ViewState["dtSearch"] = null;
        //int counter = 0;
        //DataTable dtproduct = ObjProductMaster.GetProductMasterFalseAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        //DataTable dtproductBrandSearch = new DataTable();
        //DataTable dtproductCateSearch = new DataTable();
        //if (ddlBinBrandSearch.SelectedIndex != 0)
        //{
        //    dtproductBrandSearch = dtproduct.Clone();
        //    DataTable dtProductBrand = ObjProductBrand.GetDataBrandId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ddlBinBrandSearch.SelectedValue);
        //    for (int i = 0; i < dtProductBrand.Rows.Count; i++)
        //    {
        //        dtproductBrandSearch.Merge((new DataView(dtproduct, "ProductId='" + dtProductBrand.Rows[i]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows)).ToTable());
        //    }
        //    dtproduct.Rows.Clear();
        //    dtproduct.Merge(dtproductBrandSearch);
        //    counter = 1;
        //}
        //if (ddlBinCategorySearch.SelectedIndex != 0)
        //{
        //    dtproductCateSearch = dtproduct.Clone();
        //    DataTable dtProductCate = ObjProductCate.GetProductByCategoryId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ddlBinCategorySearch.SelectedValue);
        //    for (int i = 0; i < dtProductCate.Rows.Count; i++)
        //    {
        //        dtproductCateSearch.Merge((new DataView(dtproduct, "ProductId='" + dtProductCate.Rows[i]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows)).ToTable());
        //    }
        //    dtproduct.Rows.Clear();
        //    dtproduct.Merge(dtproductCateSearch);
        //    counter = 1;
        //}
        //Session["dtProductBin"] = dtproduct;
        //lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
        //if (dtproduct.Rows.Count > 0)
        //{
        //    rptPagerBin.Visible = true;
        //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    objPageCmn.FillData((object)gvBinProduct, dtproduct, "", "");
        //    PageSize = 10;
        //    int rownumberfrom = (pageIndex - 1) * PageSize + 1;
        //    int rownumberto = (((pageIndex - 1) * PageSize + 1) + PageSize) - 1;
        //    if (counter == 0)
        //    {
        //        DataTable dtDatalist = new DataView(dtproduct, "RowNumber >=" + rownumberfrom + " and RowNumber <=" + rownumberto + "", "", DataViewRowState.CurrentRows).ToTable();
        //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //        objPageCmn.FillData((object)dtlistbinProduct, dtDatalist, "", "");
        //        PopulatePagerBin(dtproduct.Rows.Count, pageIndex);
        //    }
        //    else
        //    {
        //        if (dtproduct.Rows.Count <= 10)
        //        {
        //            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //            objPageCmn.FillData((object)dtlistbinProduct, dtproduct, "", "");
        //            rptPagerBin.Visible = false;
        //        }
        //        else
        //        {
        //            rptPagerBin.Visible = true;
        //            int rowcount = 0;
        //            for (int j = 0; j < dtproduct.Rows.Count; j++)
        //            {
        //                if (j % 10 == 0)
        //                {
        //                    rowcount++;
        //                }
        //                dtproduct.Rows[j]["RowNumber"] = rowcount;
        //            }
        //            ViewState["dtSearch"] = dtproduct;
        //            PopulatePagerBin(dtproduct.Rows.Count, pageIndex);
        //            dtproduct = new DataView(dtproduct, "RowNumber=" + pageIndex + "", "", DataViewRowState.CurrentRows).ToTable();
        //            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //            objPageCmn.FillData((object)dtlistbinProduct, dtproduct, "", "");
        //        }
        //    }
        //}
        //else
        //{
        //    rptPagerBin.Visible = false;
        //    gvBinProduct.DataSource = null;
        //    gvBinProduct.DataBind();
        //    dtlistbinProduct.DataSource = null;
        //    dtlistbinProduct.DataBind();
        //}
        //if (gvBinProduct.Visible == true)
        //{
        //    if (dtproduct.Rows.Count != 0)
        //    {
        //        //AllPageCode();
        //    }
        //    else
        //    {
        //        //imgBtnRestore.Visible = false;
        //        //ImgbtnSelectAll.Visible = false;
        //    }
        //}
        //else
        //{
        //    //imgBtnRestore.Visible = false;
        //    //ImgbtnSelectAll.Visible = false;
        //}
    }
    #endregion
    #endregion  //End Function
    protected void txtAlterNateId_TextChanged(object sender, EventArgs e)
    {
        //bool IsAllow = checkAlternateid(((TextBox)sender));
        //if (!IsAllow)
        //{
        //    ((TextBox)sender).Text = "";
        //    ((TextBox)sender).Focus();
        //    DisplayMessage("Alternate Id Already Exists");
        //    return;
        //}
    }
    public bool checkAlternateid(TextBox alternateidvalue)
    {
        bool IsAllow = true;
        bool b = false;
        if (alternateidvalue.Text != "")
        {
            if (alternateidvalue.ID == txtAlterId1.ID)
            {
                if (txtAlterId1.Text.Trim() == txtProductId.Text.Trim() || txtAlterId1.Text.Trim() == txtAlterId3.Text.Trim() || txtAlterId1.Text.Trim() == txtAlterId2.Text.Trim())
                {
                    b = true;
                }
            }
            if (alternateidvalue.ID == txtAlterId2.ID)
            {
                if (txtAlterId2.Text.Trim() == txtProductId.Text.Trim() || txtAlterId2.Text.Trim() == txtAlterId3.Text.Trim() || txtAlterId2.Text.Trim() == txtAlterId1.Text.Trim())
                {
                    b = true;
                }
            }
            if (alternateidvalue.ID == txtAlterId3.ID)
            {
                if (txtAlterId3.Text.Trim() == txtProductId.Text.Trim() || txtAlterId3.Text.Trim() == txtAlterId2.Text.Trim() || txtAlterId3.Text.Trim() == txtAlterId1.Text.Trim())
                {
                    b = true;
                }
            }
            if (b)
            {
                alternateidvalue.Text = "";
                alternateidvalue.Focus();
                //DisplayMessage("Alternate Id Already Exist ");
                IsAllow = false;
            }
            //else
            //{
            //    DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString()), "ProductCode='" + alternateidvalue.Text.Trim() + "' or alternateId1='" + alternateidvalue.Text.Trim() + "' or alternateId2='" + alternateidvalue.Text.Trim() + "' or alternateId3='" + alternateidvalue.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (dt.Rows.Count != 0)
            //    {
            //        if (hdnProductId.Value != "")
            //        {
            //            if (dt.Rows[0]["ProductId"].ToString() != hdnProductId.Value)
            //            {
            //                alternateidvalue.Text = "";
            //                alternateidvalue.Focus();
            //                //DisplayMessage("Alternate Id Already Exists");
            //                IsAllow = false;
            //            }
            //        }
            //        else
            //        {
            //            alternateidvalue.Text = "";
            //            alternateidvalue.Focus();
            //            //DisplayMessage("Alternate Id Already Exists");
            //            IsAllow = false;
            //        }
            //    }
            //}
        }
        alternateidvalue.Focus();
        return IsAllow;
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        List<ListItem> pages = new List<ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;
        //Calculate the Start and End Index of pages to be displayed.
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(10));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }
        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }
        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }
        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("First", "1"));
        }
        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        }
        for (int i = startIndex; i <= endIndex; i++)
        {
            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        }
        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        }
        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new ListItem("Last", pageCount.ToString()));
        }
        rptPager.DataSource = pages;
        rptPager.DataBind();
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        string RowNumber = string.Empty;
        int RecordCount = 0;
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        string strcategoryId = "0";
        string strBrandId = "0";
        //ViewState["SearchPage"] = null;
        //ViewState["dtSearch"] = null;
        DataTable dtProduct = new DataTable();
        if (ddlcategorysearch.SelectedIndex > 0)
        {
            strcategoryId = ddlcategorysearch.SelectedValue;
        }
        if (ddlbrandsearch.SelectedIndex > 0)
        {
            strBrandId = ddlbrandsearch.SelectedValue;
        }
        string strsearchProductId = "0";
        if (txtSearchPrduct.Text.Trim() != "")
        {
            DataTable dtsearchProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchPrduct.Text.Trim());
            if (dtsearchProduct != null)
            {
                if (dtsearchProduct.Rows.Count > 0)
                {
                    strsearchProductId = dtsearchProduct.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    strsearchProductId = "0";
                }
            }
        }
        PopulatePager((int)ViewState["RecordCount"], pageIndex);

        string strStockQty = string.Empty;
        if (chkAvailableStock.Checked == true)
        {
            strStockQty = "True";
        }
        else if (chkAvailableStock.Checked == false)
        {
            strStockQty = "False";
        }

        dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, chkDiscontinue.Checked.ToString(), txtSearchPrduct.Text.Trim(), GetFilterString(), "0", strStockQty, false.ToString(), "2", true.ToString());
        if (imbBtnGrid.Visible == true)
        {
            objPageCmn.FillData((object)dtlistProduct, dtProduct, "", "");
        }
        else
        {
            objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
        }
        Session["pageIndex"] = pageIndex;
        //if (ViewState["dtSearch"] == null)
        //{
        //    FillDataListGrid(pageIndex);
        //}
        //else
        //{
        //    DataTable dt = (DataTable)ViewState["dtSearch"];
        //    dt = new DataView(dt, "RowNumber = '" + pageIndex + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dt.Rows.Count > 0)
        //    {
        //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //        objPageCmn.FillData((object)dtlistProduct, dt, "", "");
        //        PopulatePager(((DataTable)ViewState["dtSearch"]).Rows.Count, pageIndex);
        //    }
        //}
        //AllPageCode();
    }
    protected void PageBin_Changed(object sender, EventArgs e)
    {
        string strcategoryId = "0";
        string strBrandId = "0";
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        DataTable dtProduct = new DataTable();
        if (ddlBinCategorySearch.SelectedIndex > 0)
        {
            strcategoryId = ddlBinCategorySearch.SelectedValue;
        }
        if (ddlBinBrandSearch.SelectedIndex > 0)
        {
            strBrandId = ddlBinBrandSearch.SelectedValue;
        }
        string strsearchProductId = "0";
        dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", pageIndex.ToString(), strBrandId, strcategoryId, false.ToString(), "", GetFilterString_ForBin(), "0", "", false.ToString(), "2", false.ToString());
        //PopulatePagerBin((int)ViewState["BinRecordCount"], pageIndex);
        //if (imgBtnbinGrid.Visible == true)
        //{
        //    objPageCmn.FillData((object)dtlistbinProduct, dtProduct, "", "");
        //}
        //else
        //{
        objPageCmn.FillData((object)gvBinProduct, dtProduct, "", "");
        //}
        dtProduct.Dispose();
    }
    private void PopulatePagerBin(int recordCount, int currentPage)
    {
        List<ListItem> pages = new List<ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;
        //Calculate the Start and End Index of pages to be displayed.
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(10));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }
        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }
        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }
        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("First", "1"));
        }
        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        }
        for (int i = startIndex; i <= endIndex; i++)
        {
            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        }
        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        }
        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new ListItem("Last", pageCount.ToString()));
        }
        //rptPagerBin.DataSource = pages;
        //rptPagerBin.DataBind();
    }
    #region upload
    #region imagetobyte
    private static byte[] ReadImage(string p_postedImageFileName, string[] p_fileType)
    {
        bool isValidFileType = false;
        try
        {
            FileInfo file = new FileInfo(p_postedImageFileName);
            foreach (string strExtensionType in p_fileType)
            {
                if (strExtensionType == file.Extension)
                {
                    isValidFileType = true;
                    break;
                }
            }
            if (isValidFileType)
            {
                FileStream fs = new FileStream(p_postedImageFileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] image = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                return image;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    protected void btnloadimg_Click(object sender, EventArgs e)
    {
        if (fugProduct.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/"));
            }
            imgProduct.ImageUrl = "../Bootstrap_Files/dist/img/NoImage.jpg";
            int fileSizeKB = fugProduct.PostedFile.ContentLength / 1000;
            // Check if the file size is greater than Parameter Value
            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (fileSizeKB > int.Parse(ParmValue))
            {
                fugProduct.FileContent.Dispose();
                lblImgMessageShow.Text = "File size should be " + ParmValue + "KB or less.";
                return;

            }
            fugProduct.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + fugProduct.FileName));
            imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + fugProduct.FileName;
            Session["Image"] = fugProduct.FileName;
        }
        fugProduct.FileContent.Dispose();
    }
    #endregion
    #region filemanager
    #endregion
    public string CalculateLabelCost(string ModelId, string PartNumber)
    {
        string Usedquantity = "0";
        try
        {
            DataTable dt = new DataTable();
            DataTable dtmodellabel = new DataTable();
            dt = objProOpCatedetail.GetAllRecord(Session["CompId"].ToString().ToString());
            string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
            dtmodellabel = objDa.return_DataTable(sql);
            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString());
            float labelQty = 0;
            float ColorPrice = 0;
            float MaterialPrice = 0;
            float PaperSize = 0;
            float Packingqty = 0;
            string labelQtyCateId = dt.Rows[0]["QtyCategoryId"].ToString();
            string labelMeterialCateId = dt.Rows[0]["MaterialCategoryId"].ToString();
            string labelColorCateId = dt.Rows[0]["ColorCategoryId"].ToString();
            string LabelSizeCatId = dt.Rows[0]["OptionCategoryId"].ToString();
            string PackingCatId = dt.Rows[0]["Field1"].ToString();
            string strExchangerate = string.Empty;
            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();
                if (Charvalue == "#")
                {
                    dtmodellabel = new DataView(dtmodellabel, "Field2='" + PartNumber.Split('#')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    PaperSize = float.Parse(dtmodellabel.Rows[0]["MMSize"].ToString()) / float.Parse(dt.Rows[0]["DefaultValue"].ToString());
                    break;
                }
                DataTable dtTemp = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelQtyCateId.Trim())
                    {
                        labelQty = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());
                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelMeterialCateId.Trim())
                    {
                        if (dtTemp.Rows[0]["SubProductID"].ToString() != "0")
                        {
                            if (objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim()).Rows.Count > 0)
                            {
                                try
                                {
                                    MaterialPrice = float.Parse(new DataView(objStockDetail.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitPrice"].ToString());
                                    strExchangerate = SystemParameter.GetExchageRate(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString(), Session["DBConnection"].ToString());
                                    if (strExchangerate == "")
                                    {
                                        strExchangerate = "1";
                                    }
                                    MaterialPrice = MaterialPrice * float.Parse(strExchangerate);
                                }
                                catch
                                {
                                    MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                                }
                            }
                            else
                            {
                                MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                            }
                        }
                        else
                        {
                            MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                        }
                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelColorCateId.Trim())
                    {
                        ColorPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtTemp.Rows[0]["Quantity"].ToString());
                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == PackingCatId.Trim())
                    {
                        Packingqty = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());
                    }
                }
            }
            strExchangerate = "1";
            try
            {
                if (ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString() != "" && ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString() != "0")
                {
                    strExchangerate = SystemParameter.GetExchageRate(ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
                }
                else
                {
                    strExchangerate = "1";
                }
            }
            catch
            {
                strExchangerate = "1";
            }
            float M2PaperUsage = PaperSize * labelQty * Packingqty;
            float Price = (M2PaperUsage * MaterialPrice) + (M2PaperUsage * ColorPrice);
            Usedquantity = (float.Parse(strExchangerate) * Price).ToString();
        }
        catch
        {
            Usedquantity = "0";
        }
        return Usedquantity;
    }
    #region VerifyProduct
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        Reset();
        btnEdit_Command(sender, e);
        //AllPageCode();
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        FillVeryGrid();
        ddlBinBrandSearch.Focus();
    }
    public void FillVeryGrid()
    {
        DataTable dtProduct = new DataView(ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "Field3='False'", "", DataViewRowState.CurrentRows).ToTable();
        gvVerifyProduct.DataSource = dtProduct;
        gvVerifyProduct.DataBind();
        Session["DtVerifyProduct"] = dtProduct;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProduct.Rows.Count.ToString() + "";
    }
    protected void btnVerifyProduct_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (gvVerifyProduct.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = ObjProductMaster.updateVerifyStatus(userdetails[i].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    if (Msg != 0)
                    {
                        FillVeryGrid();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Verify successfully");
                        Session["CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                return;
            }
        }
    }
    protected void btnSelectRecord_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["DtVerifyProduct"];
        if (btnSelectRecord.Text == "Select All")
        {
            btnSelectRecord.Text = "UnSelect All";
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (!userdetails.Contains(Convert.ToInt32(dr["ProductId"])))
                    userdetails.Add(Convert.ToInt32(dr["ProductId"]));
            }
            foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["DtVerifyProduct"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvVerifyProduct, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
            btnSelectRecord.Text = "Select All";
        }
    }
    protected void imgSearchVeryProduct_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlVerifyProductOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlVerifyProductOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String)='" + txtVerifyProduct.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String) like '%" + txtVerifyProduct.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String) Like '" + txtVerifyProduct.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtVerifyProduct"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtVerifyProduct"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVerifyProduct, view.ToTable(), "", "");
            //AllPageCode();
            btnbind.Focus();
        }
        txtVerifyProduct.Focus();
    }
    protected void imgSearchVeryRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        txtVerifyProduct.Text = "";
        ddlVerifyProductFieldName.SelectedIndex = 0;
        ddlVerifyProductOption.SelectedIndex = 3;
        txtVerifyProduct.Focus();
        FillVeryGrid();
        //AllPageCode();
    }
    protected void chkgvVerifySelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvVerifyProduct.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
        {
            index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
            {
                int index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
        {
            index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void gvVerifyProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvVerifyProduct.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtVerifyProduct"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvVerifyProduct, dt, "", "");
        PopulateCheckedValues();
        //AllPageCode();
    }
    protected void gvVerifyProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtVerifyProduct"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtVerifyProduct"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvVerifyProduct, dt, "", "");
        Session["CHECKED_ITEMS"] = null;
        //AllPageCode();
        gvVerifyProduct.HeaderRow.Focus();
    }
    #endregion
    #region LOcationStock
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        modelSA.getProductDetail(e.CommandArgument.ToString(), "", "");
    }
    #endregion
    #region Filetobytearray
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }
    #endregion
    #region RackLIst
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRackName(string prefixText, int count, string contextKey)
    {
        Inv_RackMaster ObjRackMaster = new Inv_RackMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjRackMaster.GetDistinctRackName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Rack_Name"].ToString() + "/" + dt.Rows[i]["Rack_Id"].ToString();
        }
        return txt;
    }
    protected void txtRackName_TextChanged(object sender, EventArgs e)
    {
        string strRackName = string.Empty;
        try
        {
            strRackName = txtRackName.Text.Split('/')[0].ToString();
        }
        catch
        {
            DisplayMessage("Select rack in suggestion only");
            txtRackName.Text = "";
            txtRackName.Focus();
            return;
        }
        DataTable dt = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRackName.Trim());
        if (dt.Rows.Count == 0)
        {
            txtRackName.Text = "";
            DisplayMessage("Select rack in suggestion only");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRackName);
            return;
        }
    }
    #endregion
    #region ModelHistory
    protected void lnkModelInfo_Command(object sender, CommandEventArgs e)
    {
        if (txtModelNo.Text == "")
        {
            DisplayMessage("Model Not Found");
            return;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/ModelProductList.aspx?ModelNo=" + txtModelNo.Text + "')", true);
    }
    protected void lnkModel1Info_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "" || e.CommandName.ToString() == "0")
        {
            DisplayMessage("Model Not Found");
            return;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/ModelProductList.aspx?ModelNo=" + e.CommandName.ToString() + "')", true);
    }
    #endregion
    [WebMethod]
    public static string MyMethod(string name)
    {
        HttpContext.Current.Session["SelectCategory"] = name;
        return name;
    }
    #region ReOrderReport
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        string[] txt = new string[0];
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dt != null)
        {
            txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
        }
        return txt;
    }
    protected void txtReOrderProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtReOrderProductName.Text.Trim() != "")
        {
            DataTable dtProduct = new DataTable();
            dtProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtReOrderProductName.Text.Trim());
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtReOrderProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtReOrderProductCode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                DisplayMessage("Product Not Found !");
                txtReOrderProductName.Text = "";
                txtReOrderProductCode.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReOrderProductName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReOrderProductName);
        }
    }
    protected void txtReOrderProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtReOrderProductCode.Text.Trim() != "")
        {
            DataTable dtProduct = new DataTable();
            dtProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtReOrderProductCode.Text.Trim());
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                ////updated by jitendra upadhyay
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtReOrderProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtReOrderProductCode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                DisplayMessage("Product Not Found !");
                txtReOrderProductName.Text = "";
                txtReOrderProductCode.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReOrderProductCode);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReOrderProductCode);
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_ReOrderSupplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    protected void btnreOrderreport_Click(object sender, EventArgs e)
    {
        string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }
        if (strLocationId != "")
        {
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }
        if (txtOrderForMonth.Text == "")
        {
            txtOrderForMonth.Text = "1";
        }
        //string strsql = "with tb as (select distinct Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode,Inv_UnitMaster.Unit_Name,Inv_ProductMaster.EProductName,ISNULL( Inv_StockDetail.Quantity,0) as StockQuantity,isnull((select SUM(Quantity) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0) as Totalsalesqty,isnull((select Datediff(day,min(sh.Invoice_Date),max(sh.Invoice_Date)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0) as TotalDurationindays,case when isnull((select Datediff(day,min(sh.Invoice_Date),max(sh.Invoice_Date)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)=0 then(isnull((select SUM(Quantity) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)/1)*30 else (isnull((select SUM(Quantity) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)/isnull((select Datediff(day,min(sh.Invoice_Date),max(sh.Invoice_Date)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0))*30  end as MonthlySales,     isnull((select DATEDIFF(DAY, MAX(Inv_PurchaseOrderHeader.PODate),MAX(PH.invoicedate)) from Inv_PurchaseInvoiceDetail as PD inner join Inv_PurchaseInvoiceHeader as PH on PD.InvoiceNo=ph.TransID inner join Inv_PurchaseOrderHeader on PD.POId=Inv_PurchaseOrderHeader.TransID where PD.Location_ID='" + Session["LocId"].ToString() + "' and pd.ProductId=Inv_ProductMaster.ProductId),30)  as LeadTimeDays,  case when isnull((select Datediff(day,min(sh.Invoice_Date),max(sh.Invoice_Date)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)=0 then(isnull((select SUM(Quantity) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)/1)*isnull((select DATEDIFF(DAY, MAX(Inv_PurchaseOrderHeader.PODate),MAX(PH.invoicedate)) from Inv_PurchaseInvoiceDetail as PD inner join Inv_PurchaseInvoiceHeader as PH on PD.InvoiceNo=ph.TransID inner join Inv_PurchaseOrderHeader on PD.POId=Inv_PurchaseOrderHeader.TransID where PD.Location_ID='" + Session["LocId"].ToString() + "' and pd.ProductId=Inv_ProductMaster.ProductId),30)else (isnull((select SUM(Quantity) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0)/isnull((select Datediff(day,min(sh.Invoice_Date),max(sh.Invoice_Date)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on sd.Invoice_No=sh.Trans_Id  and sd.Location_Id='" + Session["LocId"].ToString() + "' and sd.Product_Id=Inv_ProductMaster.ProductId and sh.post='True'),0))*isnull((select DATEDIFF(DAY, MAX(Inv_PurchaseOrderHeader.PODate),MAX(PH.invoicedate)) from Inv_PurchaseInvoiceDetail as PD inner join Inv_PurchaseInvoiceHeader as PH on PD.InvoiceNo=ph.TransID inner join Inv_PurchaseOrderHeader on PD.POId=Inv_PurchaseOrderHeader.TransID where PD.Location_ID='" + Session["LocId"].ToString() + "' and pd.ProductId=Inv_ProductMaster.ProductId),30) end as LeadTimeQty,  ISNULL(Inv_ProductMaster.ReorderQty,1)  as SuggestedOrderMonth     from Inv_ProductMaster Left join Inv_SalesInvoiceDetail on Inv_ProductMaster.ProductId=Inv_SalesInvoiceDetail.Product_Id inner join Inv_UnitMaster on Inv_ProductMaster.UnitId=Inv_UnitMaster.Unit_Id    and Inv_SalesInvoiceDetail.Location_Id='" + Session["LocId"].ToString() + "' left join inv_salesinvoiceheader on Inv_SalesInvoiceDetail.Invoice_No=inv_salesinvoiceheader.Trans_Id and inv_salesinvoiceheader.Location_Id='" + Session["LocId"].ToString() + "' Left join Inv_StockDetail on Inv_ProductMaster.ProductId=Inv_StockDetail.ProductId and Inv_StockDetail.Location_Id='" + Session["LocId"].ToString() + "'       where Inv_ProductMaster.Company_Id='" + Session["CompId"].ToString() + "' and Inv_ProductMaster.Brand_Id='" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.IsActive='True' )   select ProductId,ProductCode,Unit_Name,EProductName,StockQuantity as Currentstock,MonthlySales,LeadTimeDays,LeadTimeQty ,SuggestedOrderMonth,((MonthlySales*SuggestedOrderMonth)+LeadTimeQty) as SuggestedOrderQty,((MonthlySales*SuggestedOrderMonth)+LeadTimeQty) as OrderQty from tb";
        string strsql = "WITH tb AS (SELECT DISTINCT (SELECT inv_productbrandmaster.Brand_Name FROM inv_productbrandmaster WHERE inv_productbrandmaster.pbrandid IN (SELECT TOP 1 Inv_Product_Brand.PBrandId FROM Inv_Product_Brand WHERE Inv_Product_Brand.ProductId = Inv_ProductMaster.ProductId)) AS BrandName, (SELECT Inv_Product_CategoryMaster.Category_Name FROM Inv_Product_CategoryMaster WHERE Inv_Product_CategoryMaster.Category_Id IN (SELECT TOP 1 Inv_Product_Category.CategoryId FROM Inv_Product_Category WHERE Inv_Product_Category.ProductId = Inv_ProductMaster.ProductId)) AS CategoryName, Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_UnitMaster.Unit_Name, Inv_ProductMaster.EProductName, ISNULL(Inv_StockDetail.Quantity, 0) AS StockQuantity, ISNULL((SELECT SUM(Quantity) FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON sd.Invoice_No = sh.Trans_Id AND sd.Location_Id IN (" + strLocationId + ") AND sd.Product_Id = Inv_ProductMaster.ProductId AND sh.post = 'True'), 0) AS Totalsalesqty, (ISNULL((SELECT SUM(Quantity) FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON sd.Invoice_No = sh.Trans_Id AND sd.Location_Id IN (" + strLocationId + ") AND sd.Product_Id = Inv_ProductMaster.ProductId AND sh.post = 'True' and sh.Invoice_Date>='" + Convert.ToDateTime(Session["FinanceFromdate"].ToString()) + "' and sh.Invoice_Date<='" + DateTime.Now.ToString() + "'), 0) / ISNULL((select DATEDIFF (day,Ac_Finance_Year_Info.From_Date,GETDATE()) from Ac_Finance_Year_Info where Ac_Finance_Year_Info.Trans_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "), 0)) * 30 as MonthlySales, ISNULL((SELECT DATEDIFF(DAY, MAX(Inv_PurchaseOrderHeader.PODate), MAX(PH.invoicedate)) FROM Inv_PurchaseInvoiceDetail AS PD INNER JOIN Inv_PurchaseInvoiceHeader AS PH ON PD.InvoiceNo = ph.TransID INNER JOIN Inv_PurchaseOrderHeader ON PD.POId = Inv_PurchaseOrderHeader.TransID WHERE PD.Location_Id IN (" + strLocationId + ") AND pd.ProductId = Inv_ProductMaster.ProductId), 30) AS LeadTimeDays," + txtOrderForMonth.Text + " AS SuggestedOrderMonth FROM Inv_ProductMaster LEFT JOIN Inv_SalesInvoiceDetail ON Inv_ProductMaster.ProductId = Inv_SalesInvoiceDetail.Product_Id INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id AND Inv_SalesInvoiceDetail.Location_Id IN (" + strLocationId + ") LEFT JOIN inv_salesinvoiceheader ON Inv_SalesInvoiceDetail.Invoice_No = inv_salesinvoiceheader.Trans_Id AND inv_salesinvoiceheader.Location_Id IN (" + strLocationId + ") LEFT JOIN Inv_StockDetail ON Inv_ProductMaster.ProductId = Inv_StockDetail.ProductId AND Inv_StockDetail.Location_Id IN (" + strLocationId + ") AND Inv_StockDetail.Finance_Year_Id = " + HttpContext.Current.Session["FinanceYearId"].ToString() + " WHERE Inv_ProductMaster.Company_Id = " + Session["CompId"].ToString() + " AND Inv_ProductMaster.Brand_Id =" + Session["BrandId"].ToString() + " AND Inv_ProductMaster.IsActive = 'True') SELECT BrandName, CategoryName, ProductId, ProductCode, Unit_Name, EProductName, SUM(StockQuantity) AS Currentstock, MAX(MonthlySales) AS MonthlySales, MAX(LeadTimeDays) AS LeadTimeDays, (MAX(MonthlySales)/30)*MAX(LeadTimeDays) as LeadTimeQty, MAX(SuggestedOrderMonth) AS SuggestedOrderMonth,MAX((MonthlySales * SuggestedOrderMonth) +((MonthlySales)/30)*(LeadTimeDays)) AS SuggestedOrderQty, cast( Round( (MAX((MonthlySales * SuggestedOrderMonth) + ((MonthlySales)/30)*(LeadTimeDays))),0) as Int) AS OrderQty FROM tb GROUP BY ProductId, ProductCode, Unit_Name, EProductName, BrandName, CategoryName";
        DataTable dtproduct = objDa.return_DataTable(strsql);
        DataTable dtproductBrandSearch = new DataTable();
        DataTable dtproductCateSearch = new DataTable();
        DataTable dtProductSupplier = new DataTable();
        if (ddlReOrderProductBrand.SelectedIndex != 0)
        {
            try
            {
                dtproduct = new DataView(dtproduct, "BrandName='" + ddlReOrderProductBrand.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (ddlReOrderProductcategory.SelectedIndex != 0)
        {
            try
            {
                dtproduct = new DataView(dtproduct, "CategoryName='" + ddlReOrderProductcategory.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (txtreOrderSuppliername.Text != "")
        {
            DataTable dtSupplier = ObjProductSupplier.GetProductSuppliersBySupplierId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), txtreOrderSuppliername.Text.Split('/')[1].ToString());
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                dtProductSupplier.Merge((new DataView(dtproduct, "ProductId='" + dtSupplier.Rows[i]["Product_Id"].ToString() + "'", "", DataViewRowState.CurrentRows)).ToTable());
            }
            dtproduct.Rows.Clear();
            dtproduct.Merge(dtProductSupplier);
        }
        if (txtReOrderProductCode.Text != "")
        {
            dtproduct = new DataView(dtproduct, "ProductCode='" + txtReOrderProductCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        try
        {
            dtproduct.Columns["Currentstock"].ReadOnly = false;
            dtproduct.Columns["MonthlySales"].ReadOnly = false;
            dtproduct.Columns["SuggestedOrderQty"].ReadOnly = false;
            dtproduct.Columns["OrderQty"].ReadOnly = false;
            dtproduct.Columns["LeadTimeQty"].ReadOnly = false;
        }
        catch
        {
        }
        for (int i = 0; i < dtproduct.Rows.Count; i++)
        {
            dtproduct.Rows[i]["Currentstock"] = setDecimal(dtproduct.Rows[i]["Currentstock"].ToString());
            dtproduct.Rows[i]["MonthlySales"] = setDecimal(dtproduct.Rows[i]["MonthlySales"].ToString());
            dtproduct.Rows[i]["LeadTimeQty"] = setDecimal(dtproduct.Rows[i]["LeadTimeQty"].ToString());
            dtproduct.Rows[i]["SuggestedOrderQty"] = setDecimal(dtproduct.Rows[i]["SuggestedOrderQty"].ToString());
        }
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
        Session["DtReOrder"] = dtproduct;
        Session["DtFilterReOrder"] = dtproduct;
        objPageCmn.FillData((object)gvReOrder, dtproduct, null, null);
    }
    protected void ImgBtnQBind_Click(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (ddlQOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String)='" + txtQValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String) like '%" + txtQValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String) Like '" + txtQValue.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["DtReOrder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvReOrder, view.ToTable(), "", "");
            Session["DtFilterReOrder"] = view.ToTable();
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString();
        }
    }
    protected void gvReOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtFilterReOrder"];
        string sortdir = "DESC";
        if (ViewState["QSortDir"] != null)
        {
            sortdir = ViewState["QSortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["QSortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["QSortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["QSortDir"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["QSortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvReOrder, dt, "", "");
        Session["DtFilterReOrder"] = dt;
    }
    protected void IbtnDeleteOrderItem_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)Session["DtReOrder"]), "ProductId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dtFilter = new DataView(((DataTable)Session["DtFilterReOrder"]), "ProductId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvReOrder, dtFilter, "", "");
        Session["DtReOrder"] = dt;
        Session["DtFilterReOrder"] = dtFilter;
        DisplayMessage("Record Deleted");
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
        //AllPageCode();
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        objPageCmn.FillData((object)gvReOrder, (DataTable)Session["DtReOrder"], "", "");
        Session["DtFilterReOrder"] = (DataTable)Session["DtReOrder"];
        int TotalRecord = 0;
        try
        {
            TotalRecord = ((DataTable)Session["DtReOrder"]).Rows.Count;
        }
        catch
        {
        }
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + TotalRecord.ToString();
        ddlQSeleclField.SelectedIndex = 0;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValue.Visible = true;
    }
    protected void btnreOrderreset_Click(object sender, EventArgs e)
    {
        txtReOrderProductCode.Text = "";
        txtReOrderProductName.Text = "";
        ddlReOrderProductBrand.SelectedIndex = 0;
        ddlReOrderProductcategory.SelectedIndex = 0;
        txtreOrderSuppliername.Text = "";
        Session["DtReOrder"] = null;
        objPageCmn.FillData((object)gvReOrder, null, "", "");
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
    }
    protected void txtreOrderSuppliername_OnTextChanged(object sender, EventArgs e)
    {
        if (txtreOrderSuppliername.Text != "")
        {
            string strSupplierId = "";
            try
            {
                strSupplierId = txtreOrderSuppliername.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Invalid Supplier Name");
                txtreOrderSuppliername.Focus();
                txtreOrderSuppliername.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtreOrderSuppliername);
            }
            DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
            try
            {
                dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt1.Rows.Count == 0)
            {
                DisplayMessage("Invalid Supplier Name");
                txtreOrderSuppliername.Focus();
                txtreOrderSuppliername.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtreOrderSuppliername);
            }
        }
    }
    protected void btnReOrderExport_Click(object sender, EventArgs e)
    {
        //if (gvReOrder.Rows.Count > 0)
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition",
        //    "attachment;filename=ReOrderList.xls");
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    //GVTrailBalance.AllowPaging = false;
        //    //GVTrailBalance.DataBind();
        //    //Change the Header Row back to white color
        //    //GVComplete.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //    //Apply style to Individual Cells
        //    //GVComplete.HeaderRow.Cells[0].Style.Add("background-color", "green");
        //    //GVComplete.HeaderRow.Cells[1].Style.Add("background-color", "green");
        //    //GVComplete.HeaderRow.Cells[2].Style.Add("background-color", "green");
        //    //GVTrailBalance.HeaderRow.Cells[3].Style.Add("background-color", "green");
        //    for (int i = 0; i < gvReOrder.Rows.Count; i++)
        //    {
        //        GridViewRow row = gvReOrder.Rows[i];
        //        //Change Color back to white
        //        row.BackColor = System.Drawing.Color.White;
        //        //Apply text style to each Row
        //        row.Attributes.Add("class", "textmode");
        //        //Apply style to Individual Cells of Alternating Row
        //        if (i % 2 != 0)
        //        {
        //            // row.Cells[0].Style.Add("background-color", "#C2D69B");
        //            // row.Cells[1].Style.Add("background-color", "#C2D69B");
        //            // row.Cells[2].Style.Add("background-color", "#C2D69B");
        //            //row.Cells[3].Style.Add("background-color", "#C2D69B");
        //        }
        //    }
        //    gvReOrder.RenderControl(hw);
        //    //style to format numbers to string
        //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //    Response.Write(style);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();
        //}
        //else
        //{
        //    DisplayMessage("Record Not Available");
        //}
        DataTable dt = new DataTable();
        if (Session["DtFilterReOrder"] != null)
        {
            dt = (DataTable)Session["DtFilterReOrder"];
        }
        ExportTableData(dt, "ProductList");
    }
    protected void txtOrderforMonth_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        if (((TextBox)gvRow.FindControl("txtOrderforMonth")).Text == "")
        {
            ((TextBox)gvRow.FindControl("txtOrderforMonth")).Text = "0";
        }
        if (((Label)gvRow.FindControl("lblSalesMonth")).Text == "")
        {
            ((Label)gvRow.FindControl("lblSalesMonth")).Text = "0";
        }
        if (((Label)gvRow.FindControl("lblLeadqty")).Text == "")
        {
            ((Label)gvRow.FindControl("lblLeadqty")).Text = "0";
        }
        ((Label)gvRow.FindControl("lblSuggestedOrderQty")).Text = setDecimal(((Convert.ToDouble(((Label)gvRow.FindControl("lblSalesMonth")).Text) * Convert.ToDouble(((TextBox)gvRow.FindControl("txtOrderforMonth")).Text)) + Convert.ToDouble(((Label)gvRow.FindControl("lblLeadqty")).Text)).ToString());
        if (((Label)gvRow.FindControl("lblSuggestedOrderQty")).Text == "")
        {
            ((Label)gvRow.FindControl("lblSuggestedOrderQty")).Text = "0";
        }
        ((TextBox)gvRow.FindControl("txtOrderqty")).Text = Math.Round(Convert.ToDouble(((Label)gvRow.FindControl("lblSuggestedOrderQty")).Text), 0).ToString();
    }
    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }

    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion
    #endregion
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        //if (fugProduct.HasFile)
        //{
        //    string uploadedFileName = Path.GetFileName(fugProduct.FileName);
        //    fugProduct.SaveAs(Server.MapPath("~/temp/") + uploadedFileName);
        //    imgProduct.ImageUrl = "~/temp/" + uploadedFileName;
        //}
        if (fugProduct.HasFile)
        {
            // Get the file size in kilobytes
            int fileSizeKB = fugProduct.PostedFile.ContentLength / 1000;

            // Check if the file size is greater than 50KB
            if (fileSizeKB > 50)
            {
                lblImgMessageShow.Text = "File size should be 50KB or less.";
                return;

            }
            else
            {
                // File size is within the allowed limit, proceed with saving the file
                string uploadedFileName = Path.GetFileName(fugProduct.FileName);
                fugProduct.SaveAs(Server.MapPath("~/temp/") + uploadedFileName);
                imgProduct.ImageUrl = "~/temp/" + uploadedFileName;
            }
        }
    }
    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
            }
        }
    }
    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/Product", "Inventory", "Product", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion
    protected void ASPxFileManager1_SelectedFileOpened(object source, DevExpress.Web.FileManagerFileOpenedEventArgs e)
    {
        Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
        try
        {
            File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + e.File.Name), buffer);
        }
        catch
        {

        }
        imgProduct.ImageUrl = e.File.FullName.ToString();
        Session["Image"] = e.File.Name;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProject(string prefixText, int count, string contextKey)
    {
        Prj_ProjectMaster objProjectMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = objProjectMaster.GetProjectNamePreText(prefixText))
        {
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Project_Name"].ToString();
                }
            }
            return filterlist;
        }
    }
    [WebMethod]
    [System.Web.Script.Services.ScriptMethodAttribute()]
    public static string validateProjectName(string projectName)
    {
        Prj_ProjectMaster objProjectMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objProjectMaster.GetProjectIdByName(projectName);
        if (count != "")
        {
            return count;
        }
        else
        {
            return "0";
        }
    }

    protected void btnGvProductSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings("ItemMaster", gvProduct, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
        //showUcControlsSettings
    }
    protected void getPageControlVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting("ItemMaster");
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(gvProduct, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, EventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting("ItemMaster");
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting("ItemMaster", lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                fullname = fullname.Replace("Product_", "Product//Product_");
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/" + RegistrationCode + "/" + HttpContext.Current.Session["CompId"].ToString() + "/Product/";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {
                    if (RegistrationCode != "" && RegistrationCode != null)
                    {
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);

                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/" + HttpContext.Current.Session["CompId"].ToString() + "/Product/";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            HttpContext.Current.Session["Image"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        Common cmn = new Common(HttpContext.Current.Session["DBConnection"].ToString());
        bool Result = false;

        if (HttpContext.Current.Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }


    #region uploadEmployee
    protected void FileUploadComplete(object sender, EventArgs e)
    {
        int fileType = 0;
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
    protected void btnGetSheet_Click(object sender, EventArgs e)
    {

        int fileType = 0;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                Import(Path, fileType);
            }
        }
    }

    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }
        Session["cnn"] = strcon;
        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();
        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;
        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();
        conn.Close();
    }

    public DataTable ConvetExcelToDataTable(string path, string strtableName)
    {
        DataTable dt = new DataTable();

        try
        {
            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + strtableName.ToString() + "]";
            adp.SelectCommand.Connection = cnn;
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }

        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }



    public bool CheckSheetValidation(DataTable dt)
    {
        bool Result = true;
        if (dt.Columns.Contains("ModelNo") && dt.Columns.Contains("ProductId") && dt.Columns.Contains("ProductName") && dt.Columns.Contains("ProductName_L") && dt.Columns.Contains("Unit") && dt.Columns.Contains("ProductType") && dt.Columns.Contains("IsSerialMaintain") && dt.Columns.Contains("ProductBrand") && dt.Columns.Contains("ProductCategory") && dt.Columns.Contains("IsDiscontinue") && dt.Columns.Contains("IsActive") && dt.Columns.Contains("RackName") && dt.Columns.Contains("AlternateId1") && dt.Columns.Contains("AlternateId2") && dt.Columns.Contains("AlternateId3") && dt.Columns.Contains("HSCode") && dt.Columns.Contains("WarrantyDay") && dt.Columns.Contains("SalesComission") && dt.Columns.Contains("TechnicalComission") && dt.Columns.Contains("DeveloperComission") && dt.Columns.Contains("ProjectName") && dt.Columns.Contains("ProductSupplier"))
        {
        }
        else
        {
            Result = false;
        }
        return Result;
    }


    public DataTable AddColumn(DataTable dt)
    {
        dt.Columns.Add("ModelNo_Id");
        dt.Columns.Add("ColorCode_Id");
        dt.Columns.Add("SizeCode_Id");
        dt.Columns.Add("ProductId_Id");
        dt.Columns.Add("Unit_Id");
        dt.Columns.Add("ProductBrand_Id");
        dt.Columns.Add("ProductCategory_Id");
        dt.Columns.Add("RackName_Id");
        dt.Columns.Add("ProjectName_Id");
        dt.Columns.Add("ProductSupplier_Id");

        return dt;
    }

    public string GetcolumnValue(string strtablename, string strKeyfieldname, string strKeyfieldvalue, string strKeyFieldResult)
    {
        string strResult = "0";
        strKeyfieldvalue = strKeyfieldvalue.Replace("'", "");
        DataTable dt = objDa.return_DataTable("select " + strKeyFieldResult + " from " + strtablename + " where " + strKeyfieldname + "='" + strKeyfieldvalue + "'");
        if (dt.Rows.Count > 0)
        {
            strResult = dt.Rows[0][0].ToString();
        }
        return strResult;
    }



    protected void btnConnect_Click(object sender, EventArgs e)
    {

        string strResult = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        string strDateVal = string.Empty;
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        string strItemId = string.Empty;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }
                if (!CheckSheetValidation(dt))
                {
                    DisplayMessage("Upload valid excel sheet");
                    return;
                }
                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");
                }
                dt = AddColumn(dt);

                //dt = RemoveDuplicates(dt);

                dt = dt.DefaultView.ToTable( /*ProductId*/ true);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (i == 0)
                    {
                        dt.Rows[i]["IsValid"] = "";
                        continue;
                    }
                    dt.Rows[i]["IsValid"] = "True";
                    //if (dt.Rows[i][1].ToString() == "")
                    //{
                    //    dt.Rows[i]["IsValid"] = "";
                    //    continue;
                    //}
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (dt.Columns[j].ColumnName.Trim() == "ModelNo")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("inv_modelmaster", "Model_No", dt.Rows[i][j].ToString(), "Trans_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(ModelNo - not exists)";
                                break;
                            }
                        }

                        //Added for Color and Size By Lokesh on 23-07-2024
                        if (dt.Columns[j].ColumnName.Trim() == "ColorCode")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("Set_ColorMaster", "Color_Code", dt.Rows[i][j].ToString(), "Trans_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(ColorCode - not exists)";
                                break;
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "SizeCode")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("Set_SizeMaster", "Size_Code", dt.Rows[i][j].ToString(), "Trans_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(SizeCode - not exists)";
                                break;
                            }
                        }


                        if (dt.Columns[j].ColumnName.Trim() == "ProductId")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                //dt.Rows[i]["IsValid"] = "False(ProductId - Enter Value)";
                                //break;
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("Inv_ProductMaster", "ProductCode", dt.Rows[i][j].ToString(), "ProductId");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            strItemId = strResult;

                            //if(dt.Rows[i][j].ToString()== "CBAU01S07ZAR")
                            //{

                            //}

                            try
                            {
                                if (Convert.ToInt32(strItemId) > 0)
                                {
                                    dt.Rows[i]["IsValid"] = "False(ProductId/Code - Already Exists)";
                                    break;
                                }
                            }
                            catch
                            {

                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "ProductName")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductName - Enter Value)";
                                break;
                            }

                            strResult = GetcolumnValue("Inv_ProductMaster", "EProductName", dt.Rows[i][j].ToString(), "ProductId");
                            //  dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            strItemId = strResult;


                            try
                            {
                                if (Convert.ToInt32(strItemId) > 0)
                                {
                                    dt.Rows[i]["IsValid"] = "False(Product Name - Already Exists)";
                                    break;
                                }
                            }
                            catch
                            {

                            }

                        }


                        if (dt.Columns[j].ColumnName.Trim() == "Unit")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Unit - Enter Value)";
                                break;
                            }

                            strResult = GetcolumnValue("inv_unitmaster", "Unit_Name", dt.Rows[i][j].ToString(), "Unit_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;

                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(Unit - not exists)";
                                break;
                            }
                        }


                        if (dt.Columns[j].ColumnName.Trim() == "ProductBrand")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductBrand - Enter Value)";
                                break;
                            }

                            strResult = GetcolumnValue("Inv_ProductBrandMaster", "Brand_Name", dt.Rows[i][j].ToString(), "PBrandId");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductBrand - not exists)";
                                break;
                            }
                        }


                        if (dt.Columns[j].ColumnName.Trim() == "ProductCategory")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductCategory - Enter Value)";
                                break;
                            }

                            strResult = GetcolumnValue("Inv_Product_CategoryMaster", "Category_Name", dt.Rows[i][j].ToString(), "Category_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductCategory - not exists)";
                                break;
                            }
                        }




                        if (dt.Columns[j].ColumnName.Trim() == "RackName")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "" || dt.Rows[i][j].ToString().Trim() == "0")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";

                            }
                            else
                            {

                                strResult = GetcolumnValue("Inv_RackMaster", "Rack_Name", dt.Rows[i][j].ToString(), "Rack_ID");
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                                if (strResult == "0")
                                {
                                    dt.Rows[i]["IsValid"] = "False(RackName - not exists)";
                                    break;
                                }
                            }
                        }


                        if (dt.Columns[j].ColumnName.Trim() == "ProjectName")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "" || dt.Rows[i][j].ToString().Trim() == "0")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";

                            }
                            else
                            {

                                strResult = GetcolumnValue("Prj_Project_Master", "Project_Name", dt.Rows[i][j].ToString(), "Project_Id");
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                                if (strResult == "0")
                                {
                                    dt.Rows[i]["IsValid"] = "False(ProjectName - not exists)";
                                    break;
                                }
                            }
                        }




                        if (dt.Columns[j].ColumnName.Trim() == "ProductSupplier")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "" || dt.Rows[i][j].ToString().Trim() == "0")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";

                            }
                            else
                            {

                                strResult = GetcolumnValue("ems_contactmaster", "Name", dt.Rows[i][j].ToString(), "Trans_Id");
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                                if (strResult == "0")
                                {
                                    dt.Rows[i]["IsValid"] = "False(ProductSupplier - not exists)";
                                    break;
                                }
                            }
                        }







                    }
                }
                uploadEmpdetail.Visible = true;
                dtTemp = dt.DefaultView.ToTable(true, "ModelNo", "ColorCode", "SizeCode", "ProductId", "ProductName", "ProductName_L", "Description", "Unit", "ProductType", "IsSerialMaintain", "ProductBrand", "ProductCategory", "IsDiscontinue", "IsActive", "SalesPrice1", "SalesPrice2", "SalesPrice3", "ReorderQty", "RackName", "AlternateId1", "AlternateId2", "AlternateId3", "HSCode", "WarrantyDay", "SalesComission", "TechnicalComission", "DeveloperComission", "ProjectName", "ProductSupplier", "IsValid");
                gvSelected.DataSource = dtTemp;
                gvSelected.DataBind();
                lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count - 1).ToString();
                Session["UploadEmpDtAll"] = dt;
                Session["UploadEmpDt"] = dtTemp;
                rbtnupdall.Checked = true;
                rbtnupdInValid.Checked = false;
                rbtnupdValid.Checked = false;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        dt.Dispose();
    }

    public DataTable RemoveDuplicates(DataTable dt)
    {
        List<string> columnNames = new List<string>();
        foreach (DataColumn col in dt.Columns)
        {
            columnNames.Add(col.ColumnName);
        }
        return dt.DefaultView.ToTable(true, columnNames.Select(c => c.ToString()).ToArray());
    }

    protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["UploadEmpDt"];
        if (rbtnupdValid.Checked)
        {
            dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (rbtnupdInValid.Checked)
        {
            dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        gvSelected.DataSource = dt;
        gvSelected.DataBind();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "EmployeeInformation";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void btnUploaditemInfo_Click(object sender, EventArgs e)
    {
        string strEmpType = string.Empty;
        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        int newhirecount = 0;
        string strItemId = string.Empty;
        int counter = 0;
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        DataTable dtProduct = new DataTable();
        SqlTransaction trns;
        int b = 0;
        trns = con.BeginTransaction();
        try
        {
            dt = (DataTable)Session["UploadEmpDtAll"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsValid"].ToString().Trim() != "True")
                {
                    continue;
                }

                if (dt.Rows[i]["ProductId_Id"].ToString().Trim() == "0")
                {
                    string strDocumentId = string.Empty;
                    DataTable dtDoc = objDoc.GetDocumentNumberAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "11", "24", "2");
                    if (dtDoc.Rows.Count > 0)
                    {
                        strDocumentId = dtDoc.Rows[0]["Trans_Id"].ToString();
                    }

                    string ProductCode = string.Empty;

                    ProductCode = objDoc.GetDocumentNoProduct(strDocumentId, Session["CompId"].ToString().ToString(), "11", "24", dt.Rows[i]["ModelNo_Id"].ToString().Trim(), dt.Rows[i]["ColorCode_Id"].ToString().Trim(), dt.Rows[i]["SizeCode_Id"].ToString().Trim(), "0", "0", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();

                    DataTable dtPCode = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode='" + ProductCode.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPCode.Rows.Count != 0)
                    {
                        if (dtPCode.Rows.Count > 0)
                        {
                            strItemId = dtPCode.Rows[i]["ProductId"].ToString().Trim();
                            dtProduct = objDa.return_DataTable("select * from inv_productmaster where productid=" + strItemId + "", ref trns);
                            if (dtProduct.Rows.Count > 0)
                            {
                                ObjProductMaster.UpdateProductMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), strItemId, dtProduct.Rows[0]["ProductCode"].ToString(), "False", dt.Rows[i]["ModelNo_Id"].ToString().Trim(), "0", dt.Rows[i]["ProductName"].ToString().Trim(), dt.Rows[i]["ProductName_L"].ToString().Trim(), dtProduct.Rows[0]["CountryID"].ToString(), dt.Rows[i]["Unit_Id"].ToString().Trim(), dt.Rows[i]["ProductType"].ToString().Trim().ToLower() == "stockable" ? "S" : "NS", dt.Rows[i]["HSCode"].ToString().Trim(), false.ToString(), "Internally", false.ToString(), string.IsNullOrEmpty(dt.Rows[i]["ReorderQty"].ToString().Trim()) ? "0" : dt.Rows[i]["ReorderQty"].ToString().Trim(), "0", dt.Rows[i]["Description"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice1"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice1"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice2"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice2"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice3"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice3"].ToString().Trim(), dtProduct.Rows[0]["ProductColor"].ToString().Trim(), dtProduct.Rows[0]["WSalePrice"].ToString().Trim(), "ReseverQty", dtProduct.Rows[0]["DamageQty"].ToString().Trim(), dtProduct.Rows[0]["ExpiredQty"].ToString().Trim(), dtProduct.Rows[0]["MaximumQty"].ToString().Trim(), dtProduct.Rows[0]["MinimumQty"].ToString().Trim(), dtProduct.Rows[0]["Profit"].ToString().Trim(), dtProduct.Rows[0]["Discount"].ToString().Trim(), dt.Rows[i]["IsSerialMaintain"].ToString().Trim().ToLower() == "y" ? "SNO" : "0", "URL", dtProduct.Rows[0]["ActualWeight"].ToString().Trim(), dtProduct.Rows[0]["WeighUnitID"].ToString().Trim(), dtProduct.Rows[0]["DimLenth"].ToString().Trim(), dtProduct.Rows[0]["DimHieght"].ToString().Trim(), dtProduct.Rows[0]["DimDepth"].ToString().Trim(), dt.Rows[i]["AlternateId1"].ToString().Trim(), dt.Rows[i]["AlternateId2"].ToString().Trim(), dt.Rows[i]["AlternateId3"].ToString().Trim(), dt.Rows[i]["IsDiscontinue"].ToString().Trim().ToLower() == "y" ? "Discontinue Product" : "", "", true.ToString(), string.IsNullOrEmpty(dt.Rows[i]["SalesComission"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesComission"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["TechnicalComission"].ToString().Trim()) ? "0" : dt.Rows[i]["TechnicalComission"].ToString().Trim(), true.ToString(), DateTime.Now.ToString(), dt.Rows[i]["IsActive"].ToString().Trim().ToLower() == "y" ? true.ToString() : false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), string.IsNullOrEmpty(dt.Rows[i]["DeveloperComission"].ToString().Trim()) ? "0" : dt.Rows[i]["DeveloperComission"].ToString().Trim(), dt.Rows[i]["ProjectName_Id"].ToString().Trim(), Session["LocCurrencyId"].ToString(), dtProduct.Rows[0]["SnoPrefix"].ToString(), dtProduct.Rows[0]["SnoSuffix"].ToString(), dtProduct.Rows[0]["SnoStartFrom"].ToString(), dt.Rows[i]["SizeCode_Id"].ToString().Trim(), dt.Rows[i]["ColorCode_Id"].ToString().Trim(), ref trns);

                                counter++;
                            }
                        }
                    }
                    else
                    {
                        b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), dt.Rows[i]["ProductId"].ToString().Trim(), "False", dt.Rows[i]["ModelNo_Id"].ToString().Trim(), "0", dt.Rows[i]["ProductName"].ToString().Trim(), dt.Rows[i]["ProductName_L"].ToString().Trim(), "0", dt.Rows[i]["Unit_Id"].ToString().Trim(), dt.Rows[i]["ProductType"].ToString().Trim().ToLower() == "stockable" ? "S" : "NS", dt.Rows[i]["HSCode"].ToString().Trim(), false.ToString(), "Internally", false.ToString(), string.IsNullOrEmpty(dt.Rows[i]["ReorderQty"].ToString().Trim()) ? "0" : dt.Rows[i]["ReorderQty"].ToString().Trim(), "0", dt.Rows[i]["Description"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice1"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice1"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice2"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice2"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice3"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice3"].ToString().Trim(), "", "", "ReseverQty", "", "", "", "", "0.000", "", dt.Rows[i]["IsSerialMaintain"].ToString().Trim().ToLower() == "y" ? "SNO" : "0", "URL", "", "0", "0.000", "0.000", "0.000", dt.Rows[i]["AlternateId1"].ToString().Trim(), dt.Rows[i]["AlternateId2"].ToString().Trim(), dt.Rows[i]["AlternateId3"].ToString().Trim(), dt.Rows[i]["IsDiscontinue"].ToString().Trim().ToLower() == "y" ? "Discontinue Product" : "", "", true.ToString(), string.IsNullOrEmpty(dt.Rows[i]["SalesComission"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesComission"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["TechnicalComission"].ToString().Trim()) ? "0" : dt.Rows[i]["TechnicalComission"].ToString().Trim(), true.ToString(), DateTime.Now.ToString(), dt.Rows[i]["IsActive"].ToString().Trim().ToLower() == "y" ? true.ToString() : false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), string.IsNullOrEmpty(dt.Rows[i]["DeveloperComission"].ToString().Trim()) ? "0" : dt.Rows[i]["DeveloperComission"].ToString().Trim(), dt.Rows[i]["ProjectName_Id"].ToString().Trim(), Session["LocCurrencyId"].ToString(), "", "", "", dt.Rows[i]["ColorCode_Id"].ToString().Trim(), dt.Rows[i]["SizeCode_Id"].ToString().Trim(), false.ToString(), ref trns);

                        //Update Product Code By Lokesh on 23-07-2024
                        if (dt.Rows[i]["ProductId_Id"].ToString().Trim() == "0" && dt.Rows[i]["ModelNo_Id"].ToString().Trim() != "0" && dt.Rows[i]["ColorCode_Id"].ToString().Trim() != "0" && dt.Rows[i]["SizeCode_Id"].ToString().Trim() != "0")
                        {
                            ObjProductMaster.UpdateProductIdforModelColourSize(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }
                        else
                        {
                            ObjProductMaster.UpdateProductId(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }
                        strItemId = b.ToString();
                        newhirecount++;
                    }
                }
                else
                {
                    strItemId = dt.Rows[i]["ProductId_Id"].ToString().Trim();

                    dtProduct = objDa.return_DataTable("select * from inv_productmaster where productid=" + strItemId + "", ref trns);
                    if (dtProduct.Rows.Count > 0)
                    {
                        ObjProductMaster.UpdateProductMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), strItemId, dt.Rows[i]["ProductId"].ToString().Trim(), "False", dt.Rows[i]["ModelNo_Id"].ToString().Trim(), "0", dt.Rows[i]["ProductName"].ToString().Trim(), dt.Rows[i]["ProductName_L"].ToString().Trim(), dtProduct.Rows[0]["CountryID"].ToString(), dt.Rows[i]["Unit_Id"].ToString().Trim(), dt.Rows[i]["ProductType"].ToString().Trim().ToLower() == "stockable" ? "S" : "NS", dt.Rows[i]["HSCode"].ToString().Trim(), false.ToString(), "Internally", false.ToString(), string.IsNullOrEmpty(dt.Rows[i]["ReorderQty"].ToString().Trim()) ? "0" : dt.Rows[i]["ReorderQty"].ToString().Trim(), "0", dt.Rows[i]["Description"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice1"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice1"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice2"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice2"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["SalesPrice3"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesPrice3"].ToString().Trim(), dtProduct.Rows[0]["ProductColor"].ToString().Trim(), dtProduct.Rows[0]["WSalePrice"].ToString().Trim(), "ReseverQty", dtProduct.Rows[0]["DamageQty"].ToString().Trim(), dtProduct.Rows[0]["ExpiredQty"].ToString().Trim(), dtProduct.Rows[0]["MaximumQty"].ToString().Trim(), dtProduct.Rows[0]["MinimumQty"].ToString().Trim(), dtProduct.Rows[0]["Profit"].ToString().Trim(), dtProduct.Rows[0]["Discount"].ToString().Trim(), dt.Rows[i]["IsSerialMaintain"].ToString().Trim().ToLower() == "y" ? "SNO" : "0", "URL", dtProduct.Rows[0]["ActualWeight"].ToString().Trim(), dtProduct.Rows[0]["WeighUnitID"].ToString().Trim(), dtProduct.Rows[0]["DimLenth"].ToString().Trim(), dtProduct.Rows[0]["DimHieght"].ToString().Trim(), dtProduct.Rows[0]["DimDepth"].ToString().Trim(), dt.Rows[i]["AlternateId1"].ToString().Trim(), dt.Rows[i]["AlternateId2"].ToString().Trim(), dt.Rows[i]["AlternateId3"].ToString().Trim(), dt.Rows[i]["IsDiscontinue"].ToString().Trim().ToLower() == "y" ? "Discontinue Product" : "", "", true.ToString(), string.IsNullOrEmpty(dt.Rows[i]["SalesComission"].ToString().Trim()) ? "0" : dt.Rows[i]["SalesComission"].ToString().Trim(), string.IsNullOrEmpty(dt.Rows[i]["TechnicalComission"].ToString().Trim()) ? "0" : dt.Rows[i]["TechnicalComission"].ToString().Trim(), true.ToString(), DateTime.Now.ToString(), dt.Rows[i]["IsActive"].ToString().Trim().ToLower() == "y" ? true.ToString() : false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), string.IsNullOrEmpty(dt.Rows[i]["DeveloperComission"].ToString().Trim()) ? "0" : dt.Rows[i]["DeveloperComission"].ToString().Trim(), dt.Rows[i]["ProjectName_Id"].ToString().Trim(), Session["LocCurrencyId"].ToString(), dtProduct.Rows[0]["SnoPrefix"].ToString(), dtProduct.Rows[0]["SnoSuffix"].ToString(), dtProduct.Rows[0]["SnoStartFrom"].ToString(), "0", "0", ref trns);

                        counter++;
                    }
                }

                ObjRackDetail.DeleteRackDetail_By_LocationId_and_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strItemId, ref trns);
                ObjProductCate.DeleteProductCategory(Session["CompId"].ToString().ToString(), strItemId, ref trns);
                ObjCompanyBrand.DeleteProductCompanyBrand(Session["CompId"].ToString().ToString(), strItemId, ref trns);
                ObjProductBrand.DeleteProductBrand(Session["CompId"].ToString().ToString(), strItemId, ref trns);
                ObjProductSupplier.DeleteProductSuppliers(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), strItemId, ref trns);

                if (dt.Rows[i]["RackName_Id"].ToString().Trim() != "0")
                    ObjRackDetail.InsertRackDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["RackName_Id"].ToString().Trim(), strItemId, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                if (dt.Rows[i]["ProductCategory_Id"].ToString().Trim() != "0")
                    ObjProductCate.InsertProductCategory(Session["CompId"].ToString(), Session["BrandId"].ToString(), strItemId, dt.Rows[i]["ProductCategory_Id"].ToString().Trim(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString(), strItemId, Session["BrandId"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (dt.Rows[i]["ProductBrand_Id"].ToString().Trim() != "0")
                    ObjProductBrand.InsertProductBrand(Session["CompId"].ToString(), Session["BrandId"].ToString(), strItemId, dt.Rows[i]["ProductBrand_Id"].ToString().Trim(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (dt.Rows[i]["ProductSupplier_Id"].ToString().Trim() != "0")
                    ObjProductSupplier.InsertProductSuppliers(Session["CompId"].ToString(), Session["BrandId"].ToString(), strItemId, dt.Rows[i]["ProductSupplier_Id"].ToString().Trim(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            if (newhirecount > 0)
            {
                DisplayMessage(newhirecount.ToString() + " new Product inserted and " + counter.ToString() + " Product information updated");
            }
            else
            {
                DisplayMessage(counter.ToString() + " Product information updated");
            }
            btnResetitemInfo_Click(null, null);

        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }

    protected void btnResetitemInfo_Click(object sender, EventArgs e)
    {
        gvSelected.DataSource = null;
        gvSelected.DataBind();
        uploadEmpdetail.Visible = false;
        FillDataListGrid(1);

    }

    protected void btndownloadInvalid_Click(object sender, EventArgs e)
    {
        //this event for download inavid record excel sheet 
        if (Session["UploadEmpDt"] == null)
        {
            DisplayMessage("Record Not found");
            return;
        }
        DataTable dt = (DataTable)(Session["UploadEmpDt"]);
        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        ExportTableData(dt);
    }
    #endregion

    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            try
            {
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
        }

    }
    #endregion


    #region New Added Code for New Application on 19-06-2024
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListtxtColour(string prefixText, int count, string contextKey)
    {
        Inv_ColorMaster ObjColour = new Inv_ColorMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjColour.GetColorMaster(HttpContext.Current.Session["CompId"].ToString()), "Color_Name like '%" + prefixText.ToString() + "%'", "Color_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Color_Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListtxtSize(string prefixText, int count, string contextKey)
    {
        Inv_SizeMaster ObjSize = new Inv_SizeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjSize.GetSizeMaster(HttpContext.Current.Session["CompId"].ToString()), "Size_Name like '%" + prefixText.ToString() + "%'", "Size_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Size_Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }
    protected void txtSize_TextChanged(object sender, EventArgs e)
    {
        lblImgMessageShow.Text = "";
        TextBox txt = (TextBox)sender;
        int i = 0;
        try
        {
            string[] s = txt.Text.Split('/');
            i = Convert.ToInt32(txt.Text.Split('/')[s.Length - 1].ToString());
        }
        catch
        {
            DisplayMessage("Please select Size in suggestion list");
            txt.Text = "";
            txtSize.Text = "";
        }
    }

    protected void txtColour_TextChanged(object sender, EventArgs e)
    {
        lblImgMessageShow.Text = "";
        TextBox txt = (TextBox)sender;
        int i = 0;
        try
        {
            string[] s = txt.Text.Split('/');
            i = Convert.ToInt32(txt.Text.Split('/')[s.Length - 1].ToString());
        }
        catch
        {
            DisplayMessage("Please select Size in suggestion list");
            txt.Text = "";
            txtColour.Text = "";
        }
    }
    #endregion





    #region New Stock Screen were created 
    //protected void btnStockBind_Click(object sender, EventArgs e)
    //{
    //    FillStockColorSizeGrid();
    //}
    //protected void btnStockReferesh_Click(object sender, EventArgs e)
    //{
    //    ddlSearchLocation.SelectedIndex = 0;
    //    txtSearchProduct.Text = "";
    //    txtSearchModelNo.Text = "";
    //    txtSearchColor.Text = "";
    //    txtSearchSize.Text = "";
    //    btnStockBind_Click(null, null);
    //}
    //protected void GvStcokProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    GvStockProduct.PageIndex = e.NewPageIndex;
    //    FillStockColorSizeGrid();
    //}
    //protected void GvStockProduct_OnSorting(object sender, GridViewSortEventArgs e)
    //{
    //    HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["dtProductSearchFilter"];
    //    DataView dv = new DataView(dt);
    //    string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
    //    dv.Sort = Query;
    //    dt = dv.ToTable();
    //    Session["dtProductSearchFilter"] = dt;
    //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    //    objPageCmn.FillData((object)GvStockProduct, dt, "", "");
    //    //AllPageCode();
    //}
    //private void FillStockColorSizeGrid()
    //{
    //    string strLocationId = string.Empty;
    //    DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
    //    dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
    //    if (LocIds != "")
    //    {
    //        strLocationId = LocIds;
    //    }
    //    else
    //    {
    //        strLocationId = Session["LocId"].ToString();
    //    }

    //    string strColorId = string.Empty;
    //    if (txtSearchColor.Text != "")
    //    {
    //        string[] s = txtSearchColor.Text.Split('/');
    //        strColorId = Convert.ToInt32(txtSearchColor.Text.Split('/')[s.Length - 1].ToString()).ToString();
    //    }
    //    else
    //    {
    //        strColorId = "0";
    //    }

    //    string strSizeId = string.Empty;
    //    if (txtSearchSize.Text != "")
    //    {
    //        string[] s = txtSearchSize.Text.Split('/');
    //        strSizeId = Convert.ToInt32(txtSearchSize.Text.Split('/')[s.Length - 1].ToString()).ToString();
    //    }
    //    else
    //    {
    //        strSizeId = "0";
    //    }



    //    DataTable dtProduct = new DataTable();

    //    string strsearchProductId = "0";
    //    if (txtSearchProduct.Text.Trim() != "")
    //    {
    //        DataTable dtsearchProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchProduct.Text.Trim());
    //        if (dtsearchProduct != null)
    //        {
    //            if (dtsearchProduct.Rows.Count > 0)
    //            {
    //                strsearchProductId = dtsearchProduct.Rows[0]["ProductId"].ToString();
    //            }
    //            else
    //            {
    //                strsearchProductId = "0";
    //            }
    //        }
    //    }


    //    string strStockQty = string.Empty;
    //    if (chkAvailableStock.Checked == true)
    //    {
    //        strStockQty = "True";
    //    }
    //    else if (chkAvailableStock.Checked == false)
    //    {
    //        strStockQty = "False";
    //    }

    //    try
    //    {
    //        dtProduct = ObjProductMaster.GetProductMaster_By_PageNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", strsearchProductId, Session["FinanceYearId"].ToString(), "10", "0", "0", "0", "True", strLocationId, GetFilterString(), "0", strStockQty, true.ToString(), "13", true.ToString());
    //        if (dtProduct.Rows.Count > 0)
    //        {
    //            if (ddlSearchLocation.SelectedValue == "--Select--")
    //            {
    //                dtProduct = new DataView(dtProduct, "Location_Id in (" + LocIds + ")", "", DataViewRowState.CurrentRows).ToTable();
    //            }
    //            else
    //            {
    //                dtProduct = new DataView(dtProduct, "Location_Id='" + ddlSearchLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            }

    //            if (strsearchProductId != "" && strsearchProductId != "0")
    //            {
    //                dtProduct = new DataView(dtProduct, "ProductId='" + strsearchProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            }

    //            if (strColorId != "" && strColorId != "0")
    //            {
    //                dtProduct = new DataView(dtProduct, "ColourId='" + strColorId + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            }

    //            if (strSizeId != "" && strSizeId != "0")
    //            {
    //                dtProduct = new DataView(dtProduct, "SizeId='" + strSizeId + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            }

    //            lblTotalRecordsStock.Text = Resources.Attendance.Total_Records + " : " + dtProduct.Rows.Count.ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    try
    //    {
    //        Session["dtProductSearchFilter"] = dtProduct;
    //        objPageCmn.FillData((object)GvStockProduct, dtProduct, "", "");
    //    }
    //    catch
    //    {

    //    }


    //    try
    //    {
    //        dtProduct.Dispose();
    //    }
    //    catch
    //    {

    //    }
    //}
    //private void FillddlLocationList()
    //{
    //    ddlSearchLocation.Items.Clear();
    //    DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
    //    dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
    //    if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
    //    {
    //        if (LocIds != "")
    //        {
    //            dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //    }
    //    objPageCmn.FillData((object)ddlSearchLocation, dtLoc, "Location_Name", "Location_Id");
    //}
    #endregion


    #region Get Data from ECommerece
    protected void btnGetEcomData_Click(object sender, EventArgs e)
    {
        List<Product> products = getMasterCompanyInfo();

        if (products != null)
        {
            InsertProductsIntoDatabase(products);
            FillDataListGrid(1);
            DisplayMessage("Your All Items are Synched");
        }
    }
    public void InsertProductsIntoDatabase(List<Product> products)
    {
        // Replace with your connection string
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                foreach (var product in products)
                {

                    DataTable dtProductDataMain = new DataTable();
                    string PryceProductDataMain = @" select * from Inv_ProductMaster where ECom_ProductID='" + product.Product_Id + "'";
                    SqlCommand cmdProductDataMain = new SqlCommand(PryceProductDataMain, connection);
                    SqlDataReader readerProductDataMain = cmdProductDataMain.ExecuteReader();
                    dtProductDataMain.Load(readerProductDataMain);
                    if (dtProductDataMain.Rows.Count > 0 && dtProductDataMain != null)
                    {

                    }
                    else
                    {
                        string strProductId = product.Product_Id;
                        float strProductPrice = (float.Parse(product.Price) / 100);
                        string strProductName = product.Title;
                        string strProductDescription = product.Description;
                        string strProductImage = product.Image;


                        //Check the Model If not in Pryce System
                        string strModelId = "0";
                        if (strProductName != "")
                        {
                            DataTable dtModelMaster = new DataTable();
                            string QueryModelDetail = @" select * from Inv_ModelMaster where Model_Name='" + strProductName + "'";
                            SqlCommand cmdStcok = new SqlCommand(QueryModelDetail, connection);
                            SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                            dtModelMaster.Load(readerStcok);
                            if (dtModelMaster.Rows.Count > 0 && dtModelMaster != null)
                            {
                                strModelId = dtModelMaster.Rows[0]["Trans_Id"].ToString();
                            }
                            else
                            {
                                try
                                {
                                    //New Model Insert
                                    string ModelInsert = "INSERT INTO Inv_ModelMaster (Company_Id,Brand_Id,Model_No,Model_Name,Model_Name_L,Description,Local_Price,IsLabel,Sales_Price_1,Sales_Price_2,Sales_Price_3,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) " +
                                                        "VALUES (@Company_Id,@Brand_Id,@Model_No,@Model_Name,@Model_Name_L,@Description,@Local_Price,@IsLabel,@Sales_Price_1,@Sales_Price_2,@Sales_Price_3,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate); " +
                                                        "SELECT SCOPE_IDENTITY();";


                                    using (SqlCommand command = new SqlCommand(ModelInsert, connection))
                                    {
                                        // Add parameters to prevent SQL injection
                                        command.Parameters.AddWithValue("@Company_ID", '1');
                                        command.Parameters.AddWithValue("@Brand_Id", '1');
                                        command.Parameters.AddWithValue("@Model_No", strProductName);
                                        command.Parameters.AddWithValue("@Model_Name", strProductName);
                                        command.Parameters.AddWithValue("@Model_Name_L", strProductName);
                                        command.Parameters.AddWithValue("@Description", strProductName);
                                        command.Parameters.AddWithValue("@Local_Price", '0');
                                        command.Parameters.AddWithValue("@IsLabel", '0');
                                        command.Parameters.AddWithValue("@Sales_Price_1", '0');
                                        command.Parameters.AddWithValue("@Sales_Price_2", '0');
                                        command.Parameters.AddWithValue("@Sales_Price_3", '0');
                                        command.Parameters.AddWithValue("@Field1", "0");
                                        command.Parameters.AddWithValue("@Field2", "");
                                        command.Parameters.AddWithValue("@Field3", strProductName);
                                        command.Parameters.AddWithValue("@Field4", "80");
                                        command.Parameters.AddWithValue("@Field5", "");
                                        command.Parameters.AddWithValue("@Field6", "");
                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                        command.Parameters.AddWithValue("@IsActive", '1');
                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                        command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                        // Execute the query
                                        object result = command.ExecuteScalar();

                                        if (result != null)
                                        {
                                            // Convert the result to an integer (assuming the ID is an integer)
                                            strModelId = result.ToString();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }

                        List<Variation> Variations = product.Variations;
                        List<Category> ProductCategory = product.Categories;

                        if (Variations != null)
                        {
                            foreach (var vari in Variations)
                            {

                                List<Option> option = vari.Options;

                                // Need to Insert Values in this tables Like
                                // ItemMaster, Model Master, Colo, Size, Product Image, Product Category
                                foreach (var opt in option)
                                {
                                    try
                                    {

                                        string strVariationId = vari.Id;
                                        string strOptionId = opt.Id;

                                        string strSizeId = "0";
                                        string strColorId = "0";

                                        string optLableValue = opt.Label;
                                        if (vari.Label == "SIZE" || vari.Label == "Size")
                                        {
                                            //Check the Size If not in Pryce System
                                            if (optLableValue != "")
                                            {
                                                DataTable dtSizeDetail = new DataTable();
                                                string QuerySizeDetail = @" select * from Set_SizeMaster where Size_Name='" + optLableValue + "'";
                                                SqlCommand cmdStcok = new SqlCommand(QuerySizeDetail, connection);
                                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                                dtSizeDetail.Load(readerStcok);
                                                if (dtSizeDetail.Rows.Count > 0 && dtSizeDetail != null)
                                                {
                                                    strSizeId = dtSizeDetail.Rows[0]["Trans_Id"].ToString();
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        //New Size Insert
                                                        string SizeInsert = "INSERT INTO Set_SizeMaster (Company_ID, Size_Code, Size_name, Field5, IsActive, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) " +
                                                                            "OUTPUT INSERTED.Trans_Id  " +
                                                                         "VALUES (@Company_ID, @Size_Code, @Size_name, @Field5, @IsActive, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate); ";


                                                        using (SqlCommand command = new SqlCommand(SizeInsert, connection))
                                                        {
                                                            // Add parameters to prevent SQL injection
                                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                                            command.Parameters.AddWithValue("@Size_Code", optLableValue);
                                                            command.Parameters.AddWithValue("@Size_name", optLableValue);
                                                            command.Parameters.AddWithValue("@Field5", DateTime.Now);
                                                            command.Parameters.AddWithValue("@IsActive", '1');
                                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                                            // Execute the query
                                                            object result = command.ExecuteScalar();

                                                            if (result != null)
                                                            {
                                                                // Convert the result to an integer (assuming the ID is an integer)
                                                                strSizeId = result.ToString();
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }

                                                }
                                            }
                                        }
                                        else if (vari.Label == "COLOR" || vari.Label == "Color")
                                        {
                                            //Check the Size If not in Pryce System
                                            if (optLableValue != "")
                                            {
                                                DataTable dtColorDetail = new DataTable();
                                                string QueryColorDetail = @" select * from Set_ColorMaster where Color_Name='" + optLableValue + "'";
                                                SqlCommand cmdStcok = new SqlCommand(QueryColorDetail, connection);
                                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                                dtColorDetail.Load(readerStcok);
                                                if (dtColorDetail.Rows.Count > 0 && dtColorDetail != null)
                                                {
                                                    strColorId = dtColorDetail.Rows[0]["Trans_Id"].ToString();
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        //New Color Insert
                                                        string ColorInsert = "INSERT INTO Set_ColorMaster (Company_ID, Color_Code, Color_name, Field5, IsActive, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) " +
                                                                            "OUTPUT INSERTED.Trans_Id  " +
                                                                         "VALUES (@Company_ID, @Color_Code, @Color_name, @Field5, @IsActive, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate); ";


                                                        using (SqlCommand command = new SqlCommand(ColorInsert, connection))
                                                        {
                                                            // Add parameters to prevent SQL injection
                                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                                            command.Parameters.AddWithValue("@Color_Code", optLableValue);
                                                            command.Parameters.AddWithValue("@Color_name", optLableValue);
                                                            command.Parameters.AddWithValue("@Field5", DateTime.Now);
                                                            command.Parameters.AddWithValue("@IsActive", '1');
                                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                                            // Execute the query
                                                            object result = command.ExecuteScalar();

                                                            if (result != null)
                                                            {
                                                                // Convert the result to an integer (assuming the ID is an integer)
                                                                strColorId = result.ToString();
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }



                                        string strPryceProductId = string.Empty;
                                        if (strProductName != "")
                                        {
                                            try
                                            {
                                                SqlTransaction trns;
                                                trns = connection.BeginTransaction();

                                                //Create the Product Code for Pryce
                                                string ProductCode = objDoc.GetDocumentNoProduct("126", "1", "11", "24", strModelId, strColorId, strSizeId, "1", "1", "0", ref trns, "1", "1", "0", "3", HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();

                                                trns.Dispose();

                                                //INSERT query for Product, If Product are not exist.
                                                string query = "INSERT INTO Inv_ProductMaster (Company_Id,Brand_Id,ProductCode,PartNo,ModelNo,ModelName,EProductName,LProductName,CountryID,UnitId,ItemType,HScode,HasBatchNo,TypeOfBatchNo,HasSerialNo,ReorderQty,CostPrice,Description,SalesPrice1,SalesPrice2,SalesPrice3,ProductColor,WSalePrice,ReservedQty,DamageQty,ExpiredQty,MaximumQty,MinimumQty,Profit,Discount,MaintainStock,URL,ActualWeight,VMWeight,WeighUnitID,DimLenth,DimHieght,DimDepth,AlternateId1,AlternateId2,AlternateId3,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModfiedDate,DeveloperCommission,ProjectId,currency_Id,SnoPrefix,SnoSuffix,SnoStartFrom,SizeId,ColourId,APIStatus,ECom_ProductID, ECom_VaritaionId, ECom_OptionId) " +
                                                               "VALUES (@Company_Id,@Brand_Id,@ProductCode,@PartNo,@ModelNo,@ModelName,@EProductName,@LProductName,@CountryID,@UnitId,@ItemType,@HScode,@HasBatchNo,@TypeOfBatchNo,@HasSerialNo,@ReorderQty,@CostPrice,@Description,@SalesPrice1,@SalesPrice2,@SalesPrice3,@ProductColor,@WSalePrice,@ReservedQty,@DamageQty,@ExpiredQty,@MaximumQty,@MinimumQty,@Profit,@Discount,@MaintainStock,@URL,@ActualWeight,@VMWeight,@WeighUnitID,@DimLenth,@DimHieght,@DimDepth,@AlternateId1,@AlternateId2,@AlternateId3,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModfiedDate,@DeveloperCommission,@ProjectId,@currency_Id,@SnoPrefix,@SnoSuffix,@SnoStartFrom,@SizeId,@ColourId,@APIStatus,@ECom_ProductID, @ECom_VaritaionId, @ECom_OptionId);" +
                                                               "SELECT SCOPE_IDENTITY();";

                                                using (SqlCommand command = new SqlCommand(query, connection))
                                                {
                                                    // Add parameters to prevent SQL injection
                                                    command.Parameters.AddWithValue("@Company_Id", '1');
                                                    command.Parameters.AddWithValue("@Brand_Id", '1');
                                                    command.Parameters.AddWithValue("@ProductCode", ProductCode);
                                                    command.Parameters.AddWithValue("@PartNo", '0');
                                                    command.Parameters.AddWithValue("@ModelNo", strModelId);
                                                    command.Parameters.AddWithValue("@ModelName", '0');
                                                    command.Parameters.AddWithValue("@EProductName", strProductName);
                                                    command.Parameters.AddWithValue("@LProductName", strProductName);
                                                    command.Parameters.AddWithValue("@CountryID", "124");
                                                    command.Parameters.AddWithValue("@UnitId", '1');
                                                    command.Parameters.AddWithValue("@ItemType", "S");
                                                    command.Parameters.AddWithValue("@HScode", "");
                                                    command.Parameters.AddWithValue("@HasBatchNo", '0');
                                                    command.Parameters.AddWithValue("@TypeOfBatchNo", "Internally");
                                                    command.Parameters.AddWithValue("@HasSerialNo", '0');
                                                    command.Parameters.AddWithValue("@ReorderQty", '1');
                                                    command.Parameters.AddWithValue("@CostPrice", '0');
                                                    command.Parameters.AddWithValue("@Description", strProductDescription);
                                                    command.Parameters.AddWithValue("@SalesPrice1", strProductPrice);
                                                    command.Parameters.AddWithValue("@SalesPrice2", "0");
                                                    command.Parameters.AddWithValue("@SalesPrice3", "0");
                                                    command.Parameters.AddWithValue("@ProductColor", "");
                                                    command.Parameters.AddWithValue("@WSalePrice", "");
                                                    command.Parameters.AddWithValue("@ReservedQty", "ReseverQty");
                                                    command.Parameters.AddWithValue("@DamageQty", "");
                                                    command.Parameters.AddWithValue("@ExpiredQty", "");
                                                    command.Parameters.AddWithValue("@MaximumQty", "");
                                                    command.Parameters.AddWithValue("@MinimumQty", "");
                                                    command.Parameters.AddWithValue("@Profit", "0");
                                                    command.Parameters.AddWithValue("@Discount", "");
                                                    command.Parameters.AddWithValue("@MaintainStock", "0");
                                                    command.Parameters.AddWithValue("@URL", "");
                                                    command.Parameters.AddWithValue("@ActualWeight", "");
                                                    command.Parameters.AddWithValue("@VMWeight", "");
                                                    command.Parameters.AddWithValue("@WeighUnitID", "");
                                                    command.Parameters.AddWithValue("@DimLenth", "0");
                                                    command.Parameters.AddWithValue("@DimHieght", "0");
                                                    command.Parameters.AddWithValue("@DimDepth", "0");
                                                    command.Parameters.AddWithValue("@AlternateId1", "");
                                                    command.Parameters.AddWithValue("@AlternateId2", "");
                                                    command.Parameters.AddWithValue("@AlternateId3", "");
                                                    command.Parameters.AddWithValue("@Field1", "");
                                                    command.Parameters.AddWithValue("@Field2", "");
                                                    command.Parameters.AddWithValue("@Field3", "True");
                                                    command.Parameters.AddWithValue("@Field4", "0");
                                                    command.Parameters.AddWithValue("@Field5", "0");
                                                    command.Parameters.AddWithValue("@Field6", '1');
                                                    command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                    command.Parameters.AddWithValue("@IsActive", '1');
                                                    command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                    command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                    command.Parameters.AddWithValue("@ModfiedDate", DateTime.Now);
                                                    command.Parameters.AddWithValue("@DeveloperCommission", "0");
                                                    command.Parameters.AddWithValue("@ProjectId", "0");
                                                    command.Parameters.AddWithValue("@currency_Id", "80");
                                                    command.Parameters.AddWithValue("@SnoPrefix", "");
                                                    command.Parameters.AddWithValue("@SnoSuffix", "");
                                                    command.Parameters.AddWithValue("@SnoStartFrom", "");
                                                    command.Parameters.AddWithValue("@SizeId", strSizeId);
                                                    command.Parameters.AddWithValue("@ColourId", strColorId);
                                                    command.Parameters.AddWithValue("@APIStatus", "False");
                                                    command.Parameters.AddWithValue("@ECom_ProductID", strProductId);
                                                    command.Parameters.AddWithValue("@ECom_VaritaionId", strVariationId);
                                                    command.Parameters.AddWithValue("@ECom_OptionId", strOptionId);

                                                    // Execute the query
                                                    object result = command.ExecuteScalar();

                                                    if (result != null)
                                                    {
                                                        // Convert the result to an integer (assuming the ID is an integer)
                                                        strPryceProductId = result.ToString();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                        }


                                        //Insert the data in Stock Table
                                        if (strPryceProductId != "0" && strPryceProductId != "")
                                        {
                                            DataTable dtStockDetail = new DataTable();
                                            string QueryStockDetail = @" select * from Inv_StockDetail where ProductId='" + strPryceProductId + "' and Company_Id='1' and Brand_Id='1' and Location_Id='1' and Finance_Year_Id='1' ";
                                            SqlCommand cmdStcok = new SqlCommand(QueryStockDetail, connection);
                                            SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                            dtStockDetail.Load(readerStcok);
                                            if (dtStockDetail.Rows.Count > 0 && dtStockDetail != null)
                                            {

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    //New Stock Entry Data
                                                    string StockDetailInsert = "INSERT INTO Inv_StockDetail (Company_Id,Brand_Id,Location_Id,ProductId,OpeningBalance,RackID,Quantity,Minimum_Qty,Maximum_Qty,ReserveQty,DamageQty,BlockedQty,OrderQty,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Finance_Year_Id,SalesPrice1,SalesPrice2,SalesPrice3) " +
                                                                        "VALUES (@Company_Id,@Brand_Id,@Location_Id,@ProductId,@OpeningBalance,@RackID,@Quantity,@Minimum_Qty,@Maximum_Qty,@ReserveQty,@DamageQty,@BlockedQty,@OrderQty,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Finance_Year_Id,@SalesPrice1,@SalesPrice2,@SalesPrice3); ";

                                                    using (SqlCommand command = new SqlCommand(StockDetailInsert, connection))
                                                    {
                                                        // Add parameters to prevent SQL injection
                                                        command.Parameters.AddWithValue("@Company_ID", '1');
                                                        command.Parameters.AddWithValue("@Brand_Id", '1');
                                                        command.Parameters.AddWithValue("@Location_Id", '1');
                                                        command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                                        command.Parameters.AddWithValue("@OpeningBalance", '0');
                                                        command.Parameters.AddWithValue("@RackID", '0');
                                                        command.Parameters.AddWithValue("@Quantity", '0');
                                                        command.Parameters.AddWithValue("@Minimum_Qty", '0');
                                                        command.Parameters.AddWithValue("@Maximum_Qty", '0');
                                                        command.Parameters.AddWithValue("@ReserveQty", '0');
                                                        command.Parameters.AddWithValue("@DamageQty", '0');
                                                        command.Parameters.AddWithValue("@BlockedQty", "0");
                                                        command.Parameters.AddWithValue("@OrderQty", '0');
                                                        command.Parameters.AddWithValue("@Field1", '0');
                                                        command.Parameters.AddWithValue("@Field2", '0');
                                                        command.Parameters.AddWithValue("@Field3", "");
                                                        command.Parameters.AddWithValue("@Field4", "");
                                                        command.Parameters.AddWithValue("@Field5", "");
                                                        command.Parameters.AddWithValue("@Field6", '1');
                                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                        command.Parameters.AddWithValue("@IsActive", "True");
                                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                        command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                                                        command.Parameters.AddWithValue("@Finance_Year_Id", '1');
                                                        command.Parameters.AddWithValue("@SalesPrice1", strProductPrice);
                                                        command.Parameters.AddWithValue("@SalesPrice2", "0");
                                                        command.Parameters.AddWithValue("@SalesPrice3", "0");

                                                        command.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }
                                        //Insert the data in Product Image Table
                                        if (strPryceProductId != "0" && strPryceProductId != "")
                                        {
                                            DataTable dtImageDetail = new DataTable();
                                            string QueryImageDetail = @" select * from Inv_Product_Image where ProductId='" + strPryceProductId + "' and Company_Id='1' and Brand_Id='1'";
                                            SqlCommand cmdStcok = new SqlCommand(QueryImageDetail, connection);
                                            SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                            dtImageDetail.Load(readerStcok);
                                            if (dtImageDetail.Rows.Count > 0 && dtImageDetail != null)
                                            {

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    byte[] byteVal = new byte[0];
                                                    //New Product Image Insert
                                                    string ImageInsert = "INSERT INTO Inv_Product_Image (Company_Id,Brand_Id,ProductId,PImage,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) " +
                                                                        "VALUES (@Company_Id,@Brand_Id,@ProductId,@PImage,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate); ";

                                                    using (SqlCommand command = new SqlCommand(ImageInsert, connection))
                                                    {
                                                        // Add parameters to prevent SQL injection
                                                        command.Parameters.AddWithValue("@Company_ID", '1');
                                                        command.Parameters.AddWithValue("@Brand_Id", '1');
                                                        command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                                        command.Parameters.AddWithValue("@PImage", byteVal);
                                                        command.Parameters.AddWithValue("@Field1", "");
                                                        command.Parameters.AddWithValue("@Field2", strProductImage);
                                                        command.Parameters.AddWithValue("@Field3", "");
                                                        command.Parameters.AddWithValue("@Field4", "");
                                                        command.Parameters.AddWithValue("@Field5", "");
                                                        command.Parameters.AddWithValue("@Field6", "True");
                                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                        command.Parameters.AddWithValue("@IsActive", "True");
                                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                        command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                                        command.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }

                                        //Insert the data in Product Brand
                                        if (strPryceProductId != "0" && strPryceProductId != "")
                                        {
                                            DataTable dtPBrand = new DataTable();
                                            string QueryPBrandDetail = @" select * from Inv_Product_Brand where ProductId='" + strPryceProductId + "' and PBrandId='69'";
                                            SqlCommand cmdStcok = new SqlCommand(QueryPBrandDetail, connection);
                                            SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                            dtPBrand.Load(readerStcok);
                                            if (dtPBrand.Rows.Count > 0 && dtPBrand != null)
                                            {

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    //New Stock Entry Data
                                                    string PBrandInsert = "INSERT INTO Inv_Product_Brand (Company_Id,Brand_Id,ProductId,PBrandId,Field1,Field2,Field3,Field4,Field5,Field6,Field7,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive) " +
                                                                        "VALUES (@Company_Id,@Brand_Id,@ProductId,@PBrandId,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@IsActive); ";

                                                    using (SqlCommand command = new SqlCommand(PBrandInsert, connection))
                                                    {
                                                        // Add parameters to prevent SQL injection
                                                        command.Parameters.AddWithValue("@Company_ID", '1');
                                                        command.Parameters.AddWithValue("@Brand_Id", '1');
                                                        command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                                        command.Parameters.AddWithValue("@PBrandId", "69");
                                                        command.Parameters.AddWithValue("@Field1", "");
                                                        command.Parameters.AddWithValue("@Field2", "");
                                                        command.Parameters.AddWithValue("@Field3", "");
                                                        command.Parameters.AddWithValue("@Field4", "");
                                                        command.Parameters.AddWithValue("@Field5", "");
                                                        command.Parameters.AddWithValue("@Field6", "True");
                                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                        command.Parameters.AddWithValue("@IsActive", "True");
                                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                        command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                                        command.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                        //Insert the data in Product Company Brand Table
                                        if (strPryceProductId != "0" && strPryceProductId != "")
                                        {
                                            DataTable dtCompBrand = new DataTable();
                                            string QueryCompBrandDetail = @" select * from Inv_Product_CompanyBrand where ProductId='" + strPryceProductId + "'";
                                            SqlCommand cmdStcok = new SqlCommand(QueryCompBrandDetail, connection);
                                            SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                            dtCompBrand.Load(readerStcok);
                                            if (dtCompBrand.Rows.Count > 0 && dtCompBrand != null)
                                            {

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    //Insert Product Company Brand
                                                    string PBrandInsert = "INSERT INTO Inv_Product_CompanyBrand (Company_Id,ProductId,BrandId,Field1,Field2,Field3,Field4,Field5,Field6,Field7,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive) " +
                                                                        "VALUES (@Company_Id,@ProductId,@BrandId,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@IsActive); ";

                                                    using (SqlCommand command = new SqlCommand(PBrandInsert, connection))
                                                    {
                                                        // Add parameters to prevent SQL injection
                                                        command.Parameters.AddWithValue("@Company_ID", '1');
                                                        command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                                        command.Parameters.AddWithValue("@BrandId", "1");
                                                        command.Parameters.AddWithValue("@Field1", "");
                                                        command.Parameters.AddWithValue("@Field2", "");
                                                        command.Parameters.AddWithValue("@Field3", "");
                                                        command.Parameters.AddWithValue("@Field4", "");
                                                        command.Parameters.AddWithValue("@Field5", "");
                                                        command.Parameters.AddWithValue("@Field6", "True");
                                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                        command.Parameters.AddWithValue("@IsActive", "True");
                                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                        command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                                        command.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle errors (log them or display them)
                                        //Console.WriteLine($"Error inserting product {product.Title}: {ex.Message}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            string strPryceProductId = string.Empty;
                            if (strProductName != "")
                            {
                                try
                                {
                                    SqlTransaction trns;
                                    trns = connection.BeginTransaction();

                                    //Create the Product Code for Pryce
                                    string ProductCode = objDoc.GetDocumentNoProduct("126", "1", "11", "24", strModelId, "0", "0", "1", "1", "0", ref trns, "1", "1", "0", "3", HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();

                                    trns.Dispose();
                                    //INSERT query for Product, If Product are not exist.
                                    string query = "INSERT INTO Inv_ProductMaster (Company_Id,Brand_Id,ProductCode,PartNo,ModelNo,ModelName,EProductName,LProductName,CountryID,UnitId,ItemType,HScode,HasBatchNo,TypeOfBatchNo,HasSerialNo,ReorderQty,CostPrice,Description,SalesPrice1,SalesPrice2,SalesPrice3,ProductColor,WSalePrice,ReservedQty,DamageQty,ExpiredQty,MaximumQty,MinimumQty,Profit,Discount,MaintainStock,URL,ActualWeight,VMWeight,WeighUnitID,DimLenth,DimHieght,DimDepth,AlternateId1,AlternateId2,AlternateId3,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModfiedDate,DeveloperCommission,ProjectId,currency_Id,SnoPrefix,SnoSuffix,SnoStartFrom,SizeId,ColourId,APIStatus,ECom_ProductID, ECom_VaritaionId, ECom_OptionId) " +
                                                   "VALUES (@Company_Id,@Brand_Id,@ProductCode,@PartNo,@ModelNo,@ModelName,@EProductName,@LProductName,@CountryID,@UnitId,@ItemType,@HScode,@HasBatchNo,@TypeOfBatchNo,@HasSerialNo,@ReorderQty,@CostPrice,@Description,@SalesPrice1,@SalesPrice2,@SalesPrice3,@ProductColor,@WSalePrice,@ReservedQty,@DamageQty,@ExpiredQty,@MaximumQty,@MinimumQty,@Profit,@Discount,@MaintainStock,@URL,@ActualWeight,@VMWeight,@WeighUnitID,@DimLenth,@DimHieght,@DimDepth,@AlternateId1,@AlternateId2,@AlternateId3,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModfiedDate,@DeveloperCommission,@ProjectId,@currency_Id,@SnoPrefix,@SnoSuffix,@SnoStartFrom,@SizeId,@ColourId,@APIStatus,@ECom_ProductID, @ECom_VaritaionId, @ECom_OptionId);" +
                                                   "SELECT SCOPE_IDENTITY();";

                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        // Add parameters to prevent SQL injection
                                        command.Parameters.AddWithValue("@Company_Id", '1');
                                        command.Parameters.AddWithValue("@Brand_Id", '1');
                                        command.Parameters.AddWithValue("@ProductCode", ProductCode);
                                        command.Parameters.AddWithValue("@PartNo", '0');
                                        command.Parameters.AddWithValue("@ModelNo", strModelId);
                                        command.Parameters.AddWithValue("@ModelName", '0');
                                        command.Parameters.AddWithValue("@EProductName", strProductName);
                                        command.Parameters.AddWithValue("@LProductName", strProductName);
                                        command.Parameters.AddWithValue("@CountryID", "124");
                                        command.Parameters.AddWithValue("@UnitId", '1');
                                        command.Parameters.AddWithValue("@ItemType", "S");
                                        command.Parameters.AddWithValue("@HScode", "");
                                        command.Parameters.AddWithValue("@HasBatchNo", '0');
                                        command.Parameters.AddWithValue("@TypeOfBatchNo", "Internally");
                                        command.Parameters.AddWithValue("@HasSerialNo", '0');
                                        command.Parameters.AddWithValue("@ReorderQty", '1');
                                        command.Parameters.AddWithValue("@CostPrice", '0');
                                        command.Parameters.AddWithValue("@Description", strProductDescription);
                                        command.Parameters.AddWithValue("@SalesPrice1", strProductPrice);
                                        command.Parameters.AddWithValue("@SalesPrice2", "0");
                                        command.Parameters.AddWithValue("@SalesPrice3", "0");
                                        command.Parameters.AddWithValue("@ProductColor", "");
                                        command.Parameters.AddWithValue("@WSalePrice", "");
                                        command.Parameters.AddWithValue("@ReservedQty", "ReseverQty");
                                        command.Parameters.AddWithValue("@DamageQty", "");
                                        command.Parameters.AddWithValue("@ExpiredQty", "");
                                        command.Parameters.AddWithValue("@MaximumQty", "");
                                        command.Parameters.AddWithValue("@MinimumQty", "");
                                        command.Parameters.AddWithValue("@Profit", "0");
                                        command.Parameters.AddWithValue("@Discount", "");
                                        command.Parameters.AddWithValue("@MaintainStock", "0");
                                        command.Parameters.AddWithValue("@URL", "");
                                        command.Parameters.AddWithValue("@ActualWeight", "");
                                        command.Parameters.AddWithValue("@VMWeight", "");
                                        command.Parameters.AddWithValue("@WeighUnitID", "");
                                        command.Parameters.AddWithValue("@DimLenth", "0");
                                        command.Parameters.AddWithValue("@DimHieght", "0");
                                        command.Parameters.AddWithValue("@DimDepth", "0");
                                        command.Parameters.AddWithValue("@AlternateId1", "");
                                        command.Parameters.AddWithValue("@AlternateId2", "");
                                        command.Parameters.AddWithValue("@AlternateId3", "");
                                        command.Parameters.AddWithValue("@Field1", "");
                                        command.Parameters.AddWithValue("@Field2", "");
                                        command.Parameters.AddWithValue("@Field3", "True");
                                        command.Parameters.AddWithValue("@Field4", "0");
                                        command.Parameters.AddWithValue("@Field5", "0");
                                        command.Parameters.AddWithValue("@Field6", '1');
                                        command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                        command.Parameters.AddWithValue("@IsActive", '1');
                                        command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                        command.Parameters.AddWithValue("@ModfiedDate", DateTime.Now);
                                        command.Parameters.AddWithValue("@DeveloperCommission", "0");
                                        command.Parameters.AddWithValue("@ProjectId", "0");
                                        command.Parameters.AddWithValue("@currency_Id", "80");
                                        command.Parameters.AddWithValue("@SnoPrefix", "");
                                        command.Parameters.AddWithValue("@SnoSuffix", "");
                                        command.Parameters.AddWithValue("@SnoStartFrom", "");
                                        command.Parameters.AddWithValue("@SizeId", "0");
                                        command.Parameters.AddWithValue("@ColourId", "0");
                                        command.Parameters.AddWithValue("@APIStatus", "False");
                                        command.Parameters.AddWithValue("@ECom_ProductID", strProductId);
                                        command.Parameters.AddWithValue("@ECom_VaritaionId", "0");
                                        command.Parameters.AddWithValue("@ECom_OptionId", "0");

                                        // Execute the query
                                        object result = command.ExecuteScalar();

                                        if (result != null)
                                        {
                                            // Convert the result to an integer (assuming the ID is an integer)
                                            strPryceProductId = result.ToString();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }

                            //Insert the data in Stock Table
                            if (strPryceProductId != "0" && strPryceProductId != "")
                            {
                                DataTable dtStockDetail = new DataTable();
                                string QueryStockDetail = @" select * from Inv_StockDetail where ProductId='" + strPryceProductId + "' and Company_Id='1' and Brand_Id='1' and Location_Id='1' and Finance_Year_Id='1' ";
                                SqlCommand cmdStcok = new SqlCommand(QueryStockDetail, connection);
                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                dtStockDetail.Load(readerStcok);
                                if (dtStockDetail.Rows.Count > 0 && dtStockDetail != null)
                                {

                                }
                                else
                                {
                                    try
                                    {
                                        //New Stock Entry Data
                                        string StockDetailInsert = "INSERT INTO Inv_StockDetail (Company_Id,Brand_Id,Location_Id,ProductId,OpeningBalance,RackID,Quantity,Minimum_Qty,Maximum_Qty,ReserveQty,DamageQty,BlockedQty,OrderQty,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Finance_Year_Id,SalesPrice1,SalesPrice2,SalesPrice3) " +
                                                            "VALUES (@Company_Id,@Brand_Id,@Location_Id,@ProductId,@OpeningBalance,@RackID,@Quantity,@Minimum_Qty,@Maximum_Qty,@ReserveQty,@DamageQty,@BlockedQty,@OrderQty,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Finance_Year_Id,@SalesPrice1,@SalesPrice2,@SalesPrice3); ";

                                        using (SqlCommand command = new SqlCommand(StockDetailInsert, connection))
                                        {
                                            // Add parameters to prevent SQL injection
                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                            command.Parameters.AddWithValue("@Brand_Id", '1');
                                            command.Parameters.AddWithValue("@Location_Id", '1');
                                            command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                            command.Parameters.AddWithValue("@OpeningBalance", '0');
                                            command.Parameters.AddWithValue("@RackID", '0');
                                            command.Parameters.AddWithValue("@Quantity", '0');
                                            command.Parameters.AddWithValue("@Minimum_Qty", '0');
                                            command.Parameters.AddWithValue("@Maximum_Qty", '0');
                                            command.Parameters.AddWithValue("@ReserveQty", '0');
                                            command.Parameters.AddWithValue("@DamageQty", '0');
                                            command.Parameters.AddWithValue("@BlockedQty", "0");
                                            command.Parameters.AddWithValue("@OrderQty", '0');
                                            command.Parameters.AddWithValue("@Field1", '0');
                                            command.Parameters.AddWithValue("@Field2", '0');
                                            command.Parameters.AddWithValue("@Field3", "");
                                            command.Parameters.AddWithValue("@Field4", "");
                                            command.Parameters.AddWithValue("@Field5", "");
                                            command.Parameters.AddWithValue("@Field6", '1');
                                            command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", "True");
                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@Finance_Year_Id", '1');
                                            command.Parameters.AddWithValue("@SalesPrice1", strProductPrice);
                                            command.Parameters.AddWithValue("@SalesPrice2", "0");
                                            command.Parameters.AddWithValue("@SalesPrice3", "0");

                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                            //Insert the data in Product Image Table
                            if (strPryceProductId != "0" && strPryceProductId != "")
                            {
                                DataTable dtImageDetail = new DataTable();
                                string QueryImageDetail = @" select * from Inv_Product_Image where ProductId='" + strPryceProductId + "' and Company_Id='1' and Brand_Id='1'";
                                SqlCommand cmdStcok = new SqlCommand(QueryImageDetail, connection);
                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                dtImageDetail.Load(readerStcok);
                                if (dtImageDetail.Rows.Count > 0 && dtImageDetail != null)
                                {

                                }
                                else
                                {
                                    try
                                    {
                                        byte[] byteVal = new byte[0];
                                        //New Product Image Insert
                                        string ImageInsert = "INSERT INTO Inv_Product_Image (Company_Id,Brand_Id,ProductId,PImage,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) " +
                                                            "VALUES (@Company_Id,@Brand_Id,@ProductId,@PImage,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate); ";

                                        using (SqlCommand command = new SqlCommand(ImageInsert, connection))
                                        {
                                            // Add parameters to prevent SQL injection
                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                            command.Parameters.AddWithValue("@Brand_Id", '1');
                                            command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                            command.Parameters.AddWithValue("@PImage", byteVal);
                                            command.Parameters.AddWithValue("@Field1", "");
                                            command.Parameters.AddWithValue("@Field2", strProductImage);
                                            command.Parameters.AddWithValue("@Field3", "");
                                            command.Parameters.AddWithValue("@Field4", "");
                                            command.Parameters.AddWithValue("@Field5", "");
                                            command.Parameters.AddWithValue("@Field6", "True");
                                            command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", "True");
                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                            //Insert the data in Product Brand
                            if (strPryceProductId != "0" && strPryceProductId != "")
                            {
                                DataTable dtPBrand = new DataTable();
                                string QueryPBrandDetail = @" select * from Inv_Product_Brand where ProductId='" + strPryceProductId + "' and PBrandId='69'";
                                SqlCommand cmdStcok = new SqlCommand(QueryPBrandDetail, connection);
                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                dtPBrand.Load(readerStcok);
                                if (dtPBrand.Rows.Count > 0 && dtPBrand != null)
                                {

                                }
                                else
                                {
                                    try
                                    {
                                        //New Stock Entry Data
                                        string PBrandInsert = "INSERT INTO Inv_Product_Brand (Company_Id,Brand_Id,ProductId,PBrandId,Field1,Field2,Field3,Field4,Field5,Field6,Field7,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive) " +
                                                            "VALUES (@Company_Id,@Brand_Id,@ProductId,@PBrandId,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@IsActive); ";

                                        using (SqlCommand command = new SqlCommand(PBrandInsert, connection))
                                        {
                                            // Add parameters to prevent SQL injection
                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                            command.Parameters.AddWithValue("@Brand_Id", '1');
                                            command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                            command.Parameters.AddWithValue("@PBrandId", "69");
                                            command.Parameters.AddWithValue("@Field1", "");
                                            command.Parameters.AddWithValue("@Field2", "");
                                            command.Parameters.AddWithValue("@Field3", "");
                                            command.Parameters.AddWithValue("@Field4", "");
                                            command.Parameters.AddWithValue("@Field5", "");
                                            command.Parameters.AddWithValue("@Field6", "True");
                                            command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", "True");
                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                            //Insert the data in Product Company Brand Table
                            if (strPryceProductId != "0" && strPryceProductId != "")
                            {
                                DataTable dtCompBrand = new DataTable();
                                string QueryCompBrandDetail = @" select * from Inv_Product_CompanyBrand where ProductId='" + strPryceProductId + "'";
                                SqlCommand cmdStcok = new SqlCommand(QueryCompBrandDetail, connection);
                                SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                dtCompBrand.Load(readerStcok);
                                if (dtCompBrand.Rows.Count > 0 && dtCompBrand != null)
                                {

                                }
                                else
                                {
                                    try
                                    {
                                        //Insert Product Company Brand
                                        string PBrandInsert = "INSERT INTO Inv_Product_CompanyBrand (Company_Id,ProductId,BrandId,Field1,Field2,Field3,Field4,Field5,Field6,Field7,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive) " +
                                                            "VALUES (@Company_Id,@ProductId,@BrandId,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@IsActive); ";

                                        using (SqlCommand command = new SqlCommand(PBrandInsert, connection))
                                        {
                                            // Add parameters to prevent SQL injection
                                            command.Parameters.AddWithValue("@Company_ID", '1');
                                            command.Parameters.AddWithValue("@ProductId", strPryceProductId);
                                            command.Parameters.AddWithValue("@BrandId", "1");
                                            command.Parameters.AddWithValue("@Field1", "");
                                            command.Parameters.AddWithValue("@Field2", "");
                                            command.Parameters.AddWithValue("@Field3", "");
                                            command.Parameters.AddWithValue("@Field4", "");
                                            command.Parameters.AddWithValue("@Field5", "");
                                            command.Parameters.AddWithValue("@Field6", "True");
                                            command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", "True");
                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                        if (ProductCategory != null)
                        {
                            foreach (var cat in ProductCategory)
                            {
                                string strCategoryName = cat.Name;

                                try
                                {
                                    //Check the Category If not in Pryce System
                                    string strCategoryId = "";
                                    if (product.Product_Id != "")
                                    {
                                        DataTable dtCategoryMaster = new DataTable();
                                        string QueryCategoryMaster = @" select * from Inv_Product_CategoryMaster where Category_Name='" + strCategoryName + "'";
                                        SqlCommand cmdStcok = new SqlCommand(QueryCategoryMaster, connection);
                                        SqlDataReader readerStcok = cmdStcok.ExecuteReader();
                                        dtCategoryMaster.Load(readerStcok);
                                        if (dtCategoryMaster.Rows.Count > 0 && dtCategoryMaster != null)
                                        {
                                            strCategoryId = dtCategoryMaster.Rows[0]["Category_Id"].ToString();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                //New Category Insert
                                                string CategoryMasterInsert = "INSERT INTO Inv_Product_CategoryMaster (Company_Id,Brand_Id,Category_Name,Category_Name_L,Account_No,Description,Field1,Field2,Field3,Field4,Field5,Field6,Field7,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) " +
                                                                   "VALUES (@Company_Id,@Brand_Id,@Category_Name,@Category_Name_L,@Account_No,@Description,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@IsActive,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate) " +
                                                                    "SELECT SCOPE_IDENTITY();";

                                                using (SqlCommand command = new SqlCommand(CategoryMasterInsert, connection))
                                                {
                                                    // Add parameters to prevent SQL injection
                                                    command.Parameters.AddWithValue("@Company_ID", '1');
                                                    command.Parameters.AddWithValue("@Brand_Id", '1');
                                                    command.Parameters.AddWithValue("@Category_Name", strCategoryName);
                                                    command.Parameters.AddWithValue("@Category_Name_L", strCategoryName);
                                                    command.Parameters.AddWithValue("@Account_No", '0');
                                                    command.Parameters.AddWithValue("@Description", strCategoryName);
                                                    command.Parameters.AddWithValue("@Field1", "");
                                                    command.Parameters.AddWithValue("@Field2", "");
                                                    command.Parameters.AddWithValue("@Field3", "");
                                                    command.Parameters.AddWithValue("@Field4", "");
                                                    command.Parameters.AddWithValue("@Field5", "");
                                                    command.Parameters.AddWithValue("@Field6", "True");
                                                    command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                    command.Parameters.AddWithValue("@IsActive", '1');
                                                    command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                    command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);


                                                    // Execute the query
                                                    object result = command.ExecuteScalar();

                                                    if (result != null)
                                                    {
                                                        // Convert the result to an integer (assuming the ID is an integer)
                                                        strCategoryId = result.ToString();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }


                                        if (strCategoryId != "0" && strCategoryId != "")
                                        {
                                            try
                                            {
                                                DataTable dtProductData = new DataTable();
                                                string PryceProductData = @" select * from Inv_ProductMaster where ECom_ProductID='" + product.Product_Id + "'";
                                                SqlCommand cmdProductData = new SqlCommand(PryceProductData, connection);
                                                SqlDataReader readerProductData = cmdProductData.ExecuteReader();
                                                dtProductData.Load(readerProductData);
                                                if (dtProductData.Rows.Count > 0 && dtProductData != null)
                                                {
                                                    for (int i = 0; i < dtProductData.Rows.Count; i++)
                                                    {
                                                        string strProductIdNew = dtProductData.Rows[i]["ProductId"].ToString();

                                                        //Insert Product Category
                                                        string ProductCategoryInsert = "INSERT INTO Inv_Product_Category (Company_Id,Brand_Id,ProductId,CategoryId,Field1,Field2,Field3,Field4,Field5,Field6,Field7,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive) " +
                                                                             "VALUES (@Company_Id,@Brand_Id,@ProductId,@CategoryId,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@IsActive); ";


                                                        using (SqlCommand command = new SqlCommand(ProductCategoryInsert, connection))
                                                        {
                                                            // Add parameters to prevent SQL injection
                                                            command.Parameters.AddWithValue("@Company_Id", '1');
                                                            command.Parameters.AddWithValue("@Brand_Id", '1');
                                                            command.Parameters.AddWithValue("@ProductId", strProductIdNew);
                                                            command.Parameters.AddWithValue("@CategoryId", strCategoryId);
                                                            command.Parameters.AddWithValue("@Field1", "");
                                                            command.Parameters.AddWithValue("@Field2", "");
                                                            command.Parameters.AddWithValue("@Field3", "");
                                                            command.Parameters.AddWithValue("@Field4", "");
                                                            command.Parameters.AddWithValue("@Field5", "");
                                                            command.Parameters.AddWithValue("@Field6", "True");
                                                            command.Parameters.AddWithValue("@Field7", DateTime.Now);
                                                            command.Parameters.AddWithValue("@CreatedBy", "Admin");
                                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                                            command.Parameters.AddWithValue("@ModifiedBy", "Admin");
                                                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                                                            command.Parameters.AddWithValue("@IsActive", "True");

                                                            // Execute the query
                                                            command.ExecuteNonQuery();
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                    }

                }

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }
        catch (Exception ex)
        { 
        
        }
        
    }
    public List<Product> getMasterCompanyInfo()
    {
        List<Product> ProductData = new List<Product>();
        try
        {
            ServicePointManager.Expect100Continue = true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;

            var baseAddress = "https://www.dra3atomi.com/api/products";
            var http = (System.Net.HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json; charset=utf-8";
            http.ContentType = "application/x-www-form-urlencoded";
            http.Method = "POST";
            string parsedContent = "token=605db27058f1a3-69891225-72068939&u_id=85eb56a4-3d61-4776-bedc-52bf25a8147d&comment_limit=10&lang_id=1&page=2&per_page=500";
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = (HttpWebResponse)http.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            Root objPhpApiResult = JsonConvert.DeserializeObject<Root>(content);
            if (objPhpApiResult.Status == 200)
            {
                ProductData = objPhpApiResult.Data.Products;
            }
            else
            {
                ProductData = null;
            }
            return ProductData;

        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public class Root
    {
        public int Status { get; set; }
        public Data Data { get; set; }
    }
    public class Data
    {
        public List<object> QueryStringArray { get; set; }
        public int TotalProducts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public List<Product> Products { get; set; }
    }
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Product
    {
        public string Product_Id { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string DiscountRate { get; set; }
        public string DiscountType { get; set; }
        public string ShopName { get; set; }
        public int Rating { get; set; }
        public List<Variation> Variations { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<Category> Categories { get; set; }
        public List<object> Reviews { get; set; }
        public string TotalComments { get; set; }
        public List<object> Comments { get; set; }
        public string ShippingTime { get; set; }
        public bool IsInWishlist { get; set; }
        public bool HasVariation { get; set; }
        public string Image { get; set; }
    }
    public class Variation
    {
        public string Id { get; set; }
        public bool HasChildVariation { get; set; }
        public string Label { get; set; }
        public string ParentId { get; set; }
        public bool UseDifferentPrice { get; set; }
        public string VariationType { get; set; }
        public string OptionDisplayType { get; set; }
        public List<Option> Options { get; set; }
    }
    public class Option
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool HasChildOption { get; set; }
        public string Stock { get; set; }
        public string DiscountRate { get; set; }
        public string DiscountType { get; set; }
        public string IsDefault { get; set; }
        public string Price { get; set; }
        public string ParentId { get; set; }
        public bool UseDefaultPrice { get; set; }
    }
    public class ProductImage
    {
        public string Id { get; set; }
        public string ImageDefault { get; set; }
        public string ImageSmall { get; set; }
        public string IsMain { get; set; }
    }

    #endregion   
}
