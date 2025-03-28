using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using PegasusDataAccess;

public partial class Inventory_AddItem : System.Web.UI.Page
{
    #region defined Class Object
    Inv_ProductMaster ObjProductMaster = null;
    Common ObjComman = null;
    LocationMaster ObjLocMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    SystemParameter ObjSysPeram = null;
    //For Stock:-Start
    Inv_StockDetail objStockDetail = null;
    Inv_Product_RelProduct objRelProduct = null;
    Inv_ParameterMaster objInvParam = null;
    Contact_PriceList objCustomerPriceList = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass ObjDa = null;
    //End
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    string CustomerId = string.Empty;
    #endregion
    string Parameter_Id = string.Empty;
    string ParameterValue = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        //For Stock:-Start
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objRelProduct = new Inv_Product_RelProduct(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
            //    ViewState["ExchangeRate"] = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Session["CurrencyId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
            //}
            //catch
            //{
            //    DataTable dt = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (dt.Rows.Count != 0)
            //    {
            //        ViewState["ExchangeRate"] = dt.Rows[0]["Currency_Value"].ToString();
            //    }
            //}
            ViewState["DtAddProduct"] = null;
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            FillddlBrandSearch(ddlbrandsearch);
            FillProductCategorySerch(ddlcategorysearch);
            FillDataListGrid();
            DataTable dtParameterList = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Price");
            Parameter_Id = dtParameterList.Rows[0]["Parameter_Id"].ToString();
            ParameterValue = dtParameterList.Rows[0]["ParameterValue"].ToString();
            ViewState["SubProductId"] = null;
            string Footer = "<div class=*pull-right hidden-xs*>      <b>Version</b> " + ObjSysPeram.GetSysParameterByParamName("Application_Version").Rows[0]["Param_Value"].ToString() + "    </div>    <strong>Copyright &copy; " + DateTime.Now.Year.ToString() + " Pryce All rights reserved";
            Ltr_Footer_Content.Text = Footer.Replace('*', '"');
        }
    }

    public string GetImageUrl(string ProductId)
    {
        DataTable dt = new DataTable();
        dt = ObjDa.return_DataTable("select Company_Id, Field1 from Inv_Product_Image where ProductId='" + ProductId + "'");
        if (dt.Rows.Count > 0)
        {
            string Path = "~/CompanyResource/" + dt.Rows[0]["Company_Id"].ToString() + "/Product/" + dt.Rows[0]["Field1"].ToString();
            return Path;
        }
        else
        {
            return "";
        }
    }
    private void FillDataListGrid()
    {
        DataTable dtproduct = new DataTable();
        InventoryDataSet rptdata = new InventoryDataSet();
        rptdata.EnforceConstraints = false;

        string fromdate = "", toDate = "";
        if (ddlFrequency.SelectedValue != "")
        {
            toDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DateTime dt = System.DateTime.Now;
            fromdate = dt.AddMonths(-Convert.ToInt32(ddlTimePeriod.SelectedValue)).ToString("dd-MMM-yyyy");
        }

        InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        if (ddlFrequency.SelectedValue == "")
        {
            adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(ddlLocation.SelectedValue), Convert.ToInt32(Session["FinanceYearId"].ToString()), "", "", "");
        }
        else
        {
            adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(ddlLocation.SelectedValue), Convert.ToInt32(Session["FinanceYearId"].ToString()), fromdate, toDate, ddlFrequency.SelectedValue);
        }

        dtproduct = rptdata.sp_Inv_ProductMaster_SelectRow_Report;
        if (ddlbrandsearch.SelectedIndex != 0)
        {
            try
            {
                dtproduct = new DataView(dtproduct, "PBrandId='" + ddlbrandsearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (ddlcategorysearch.SelectedIndex != 0)
        {
            try
            {
                dtproduct = new DataView(dtproduct, "CategoryId='" + ddlcategorysearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        //string strsearchProductId = string.Empty;
        //if (txtSearchPrduct.Text.Trim() != "")
        //{
        //    DataTable dtsearchProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchPrduct.Text.Trim());
        //    if (dtsearchProduct != null)
        //    {
        //        if (dtsearchProduct.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtsearchProduct.Rows)
        //            {
        //                strsearchProductId += dr["ProductId"].ToString() + ",";
        //            }
        //            dtproduct = new DataView(dtproduct, "ProductId in (" + strsearchProductId.Substring(0, strsearchProductId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        else
        //        {
        //            dtproduct = new DataView(dtproduct, "ProductId in ('0')", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //    }
        //    else
        //    {
        //        dtproduct = new DataView(dtproduct, "ProductId in ('0')", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}
        try
        {
            dtproduct = dtproduct.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "AlternateId", "StockQuantity", "Description", "ShortProductName", "Model_No", "categoryID");
        }
        catch
        {
        }
        if (dtproduct.Rows.Count > 0)
        {
            DataTable DtRowCount = new DataTable();
            if (ddlGridSize.SelectedValue == "0")
            {
                dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * PageControlCommon.GetPageSize()).Take(PageControlCommon.GetPageSize()).CopyToDataTable();
            }
            if (ddlGridSize.SelectedValue == "All")
            {
                dtlistProduct.DataSource = dtproduct;
            }
            if (ddlGridSize.SelectedValue != "0" && ddlGridSize.SelectedValue != "All")
            {
                dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * Convert.ToInt32(ddlGridSize.SelectedValue)).Take(Convert.ToInt32(ddlGridSize.SelectedValue)).CopyToDataTable();
            }
            dtlistProduct.DataBind();
            if (ViewState["DtAddProduct"] != null)
            {
                CheckedAddProductList();
            }
        }
        else
        {
            dtlistProduct.DataSource = null;
            dtlistProduct.DataBind();
        }
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
    }
    public void CheckedAddProductList()
    {
        foreach (DataListItem item in dtlistProduct.Items)
        {
            CheckBox chkAddItem = (CheckBox)item.FindControl("chkAddItem");
            HiddenField hdnProductId = (HiddenField)item.FindControl("hdnProductId");
            HiddenField hdnMaintainStock = (HiddenField)item.FindControl("hdnMaintainStock");
            TextBox txtquantity = (TextBox)item.FindControl("txtquantity");
            LinkButton btnRelatedProduct = (LinkButton)item.FindControl("btnRelatedProduct");
            chkAddItem.Checked = false;
            txtquantity.Text = "1";
            btnRelatedProduct.Visible = false;
            if (ViewState["DtAddProduct"] != null)
            {
                DataTable DtProduct = (DataTable)ViewState["DtAddProduct"];
                try
                {
                    DtProduct = new DataView(DtProduct, "ProductId=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtProduct.Rows.Count > 0)
                {
                    chkAddItem.Checked = true;
                    txtquantity.Text = DtProduct.Rows[0]["Quantity"].ToString();
                    DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value.ToString());
                    if (dtRelatedProduct.Rows.Count > 0 && btnBack.Visible == false)
                    {
                        btnRelatedProduct.Visible = true;
                    }
                }
            }
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        btnBack.Visible = false;
        FillDataListGrid();
        //txtSearchPrduct.Focus();
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        ddlbrandsearch.SelectedIndex = 0;
        ddlcategorysearch.SelectedIndex = 0;
        //txtSearchPrduct.Text = "";
        btnBack.Visible = false;
        ddlTimePeriod.SelectedValue = "1";
        ddlFrequency.SelectedValue = "";
        FillDataListGrid();
        txtValue.Focus();

    }
    #region SalesInquiry
    public DataTable CreateProductDataTableForSalesInquiry()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("SuggestedProductName");
        return dt;
    }
    public void AddRecordForSalesInquiry()
    {
        DataTable DtProduct = CreateProductDataTableForSalesInquiry();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["Quantity"] = gvQuantity.Text;
            try
            {
                dr["Currency_Id"] = Request.QueryString["CurId"].ToString();
            }
            catch
            {
                dr["Currency_Id"] = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            }
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            dr["EstimatedUnitPrice"] = "0";
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region SalesQuotation
    public DataTable CreateProductDataTableForSalesQuotation()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TaxPercent");
        dt.Columns.Add("TaxValue");
        dt.Columns.Add("PriceAfterTax");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("DiscountValue");
        dt.Columns.Add("PriceAfterDiscount");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Quantity");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("PurchaseProductDescription");
        dt.Columns.Add("PurchaseProductPrice");
        dt.Columns.Add("SalesPrice");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("AgentCommission");
        return dt;
    }
    public void AddRecordForSalesQuotation()
    {
        DataTable DtProduct = CreateProductDataTableForSalesQuotation();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["TaxPercent"] = "0";
            dr["TaxValue"] = "0";
            dr["PriceAfterTax"] = "0";
            dr["DiscountPercent"] = "0";
            dr["DiscountValue"] = "0";
            dr["PriceAfterDiscount"] = "0";
            dr["Quantity"] = gvQuantity.Text;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            try
            {
                dr["Currency_Id"] = Request.QueryString["CurId"].ToString();
            }
            catch
            {
                dr["Currency_Id"] = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            }
            dr["EstimatedUnitPrice"] = "0";
            dr["PurchaseProductDescription"] = "";
            dr["PurchaseProductPrice"] = "";
            dr["AgentCommission"] = "0";
            double unitPrice = 0;
            try
            {
                unitPrice = Convert.ToDouble(ObjProductMaster.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "C", Request.QueryString["CustomerId"].ToString(), gvProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString());
            }
            catch
            {
            }
            try
            {
                dr["SalesPrice"] = ObjSysPeram.GetCurencyConversionForInv(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), unitPrice.ToString());
            }
            catch
            {
                dr["SalesPrice"] = "0";
            }
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region SalesOrder
    public DataTable CreateProductDataTableForSalesOrder()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("FreeQty");
        dt.Columns.Add("RemainQty");
        dt.Columns.Add("UnitPrice");
        dt.Columns.Add("TaxP");
        dt.Columns.Add("TaxV");
        dt.Columns.Add("DiscountP");
        dt.Columns.Add("DiscountV");
        dt.Columns.Add("TaxPUnit");
        dt.Columns.Add("TaxVUnit");
        dt.Columns.Add("DiscountPUnit");
        dt.Columns.Add("DiscountVUnit");
        dt.Columns.Add("GrossPrice");
        dt.Columns.Add("NetTotal");
        dt.Columns.Add("Field6", typeof(bool));
        dt.Columns.Add("AgentCommission");
        return dt;
    }
    public void AddRecordForSalesOrder()
    {
        DataTable DtProduct = CreateProductDataTableForSalesOrder();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["Quantity"] = gvQuantity.Text;
            dr["FreeQty"] = "0";
            dr["RemainQty"] = gvQuantity.Text;
            dr["AgentCommission"] = "0";
            double unitPrice = 0;
            try
            {
                unitPrice = Convert.ToDouble(ObjProductMaster.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "C", Request.QueryString["CustomerId"].ToString(), gvProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString());
            }
            catch
            {
            }
            dr["UnitPrice"] = ObjSysPeram.GetCurencyConversionForInv(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), unitPrice.ToString());
            dr["GrossPrice"] = ObjSysPeram.GetCurencyConversionForInv(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(gvQuantity.Text) * Convert.ToDouble(unitPrice)).ToString());
            dr["NetTotal"] = ObjSysPeram.GetCurencyConversionForInv(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(gvQuantity.Text) * Convert.ToDouble(unitPrice)).ToString());
            dr["TaxP"] = "0";
            dr["TaxV"] = "0";
            dr["DiscountP"] = "0";
            dr["DiscountV"] = "0";
            dr["TaxPUnit"] = "0";
            dr["TaxVUnit"] = "0";
            dr["DiscountPUnit"] = "0";
            dr["DiscountVUnit"] = "0";
            dr["Field6"] = false;
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region Sales Invoice
    public DataTable CreateProductDataTableForSalesInvoice()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(int));
        dt.Columns.Add("SalesOrderNo");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("Product_Code");
        dt.Columns.Add("MaintainStock");
        dt.Columns.Add("ProductName");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Unit_Name");
        dt.Columns.Add("UnitPrice");
        dt.Columns.Add("FreeQty");
        dt.Columns.Add("OrderQty");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("TaxP");
        dt.Columns.Add("TaxV");
        dt.Columns.Add("DiscountP");
        dt.Columns.Add("DiscountV");
        dt.Columns.Add("SoldQty");
        dt.Columns.Add("SysQty");
        return dt;
    }
    public void AddRecordForSalesInvoice()
    {
        string ConvertexchangeRate = string.Empty;
        Inv_StockDetail objStock = new Inv_StockDetail(Session["DBConnection"].ToString());
        DataTable DtProduct = CreateProductDataTableForSalesInvoice();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        //try
        //{
        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        ConvertexchangeRate = SystemParameter.GetExchageRate(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Request.QueryString["CurId"].ToString(), Session["DBConnection"].ToString());
        //    ConvertexchangeRate = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Request.QueryString["CurId"].ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
        //}
        //catch
        //{
        //    DataTable dt = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + Request.QueryString["CurId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dt.Rows.Count != 0)
        //    {
        //        ConvertexchangeRate = dt.Rows[0]["Currency_Value"].ToString();
        //    }
        //}
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            HiddenField hdnMaintainStock = (HiddenField)gvRow.FindControl("hdnMaintainStock");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

            DataRow dr = DtProduct.NewRow();
            dr["Product_Code"] = gvProductCode.Text;
            dr["MaintainStock"] = hdnMaintainStock.Value;
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                dr["Trans_Id"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
                dr["Trans_Id"] = "1";
            }
            dr["SalesOrderNo"] = "0";
            dr["Product_Id"] = gvProductId.Value;
            dr["ProductName"] = lblgvproductname.Text;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["UnitId"] = hdngvunitId.Value;
            dr["Unit_Name"] = GetUnitName(hdngvunitId.Value);
            double unitPrice = 0;
            try
            {
                unitPrice = (Convert.ToDouble(ObjProductMaster.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "C", Request.QueryString["CustomerId"].ToString(), gvProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())) * Convert.ToDouble(ConvertexchangeRate);
            }
            catch
            {
            }
            dr["UnitPrice"] = ObjSysPeram.GetCurencyConversionForInv(Request.QueryString["CurId"].ToString(), unitPrice.ToString());
            dr["Quantity"] = gvQuantity.Text;
            dr["FreeQty"] = "0";
            dr["OrderQty"] = gvQuantity.Text;
            dr["TaxP"] = "0";
            dr["TaxV"] = "0";
            dr["DiscountP"] = "0";
            dr["DiscountV"] = "0";
            dr["SoldQty"] = "0";
            try
            {
                dr["SysQty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["SysQty"] = "0";
            }
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region Purchase Request
    public DataTable CreateProductDataTableForPurchaseRequest()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(int));
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("ReqQty");
        dt.Columns.Add("SuggestedProductName");
        return dt;
    }
    public void AddRecordForPurchaseRequest()
    {
        DataTable DtProduct = CreateProductDataTableForPurchaseRequest();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        int i = 0;
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                dr["Trans_Id"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
                dr["Trans_Id"] = "1";
            }
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["ReqQty"] = gvQuantity.Text;
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region Purchase Inquiry
    public DataTable CreateProductDataTableForPurchaseInquiry()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ReqQty");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        return dt;
    }
    public void AddRecordForPurchaseInquiry()
    {
        DataTable DtProduct = CreateProductDataTableForPurchaseInquiry();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["ReqQty"] = gvQuantity.Text;
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region TransferRequest
    public DataTable CreateProductDataTableForTransferRequest()
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id");
        DtProduct.Columns.Add("SerialNo");
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("ProductDescription");
        return DtProduct;
    }
    public void AddRecordForTransferRequest()
    {
        DataTable DtProduct = CreateProductDataTableForTransferRequest();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "SerialNo desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["SerialNo"] = (float.Parse(DtMaxSerial.Rows[0]["SerialNo"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["SerialNo"] = "1";
            }
            dr["Trans_Id"] = DtProduct.Rows.Count + 1;
            dr["ProductId"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["Quantity"] = gvQuantity.Text;
            //dr["ProductDescription"] = lblgvDesc.Text;
            dr["UnitCost"] = "0";
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region Purchase Order
    public DataTable CreateProductDataTableForPurchaseOrder()
    {
        DataTable dtProduct = new DataTable();
        dtProduct.Columns.Add("Serial_No", typeof(int));
        dtProduct.Columns.Add("Trans_Id");
        dtProduct.Columns.Add("PoNO");
        dtProduct.Columns.Add("Product_Id");
        dtProduct.Columns.Add("ProductDescription");
        dtProduct.Columns.Add("UnitId");
        dtProduct.Columns.Add("UnitCost");
        dtProduct.Columns.Add("OrderQty");
        dtProduct.Columns.Add("freeQty");
        return dtProduct;
    }
    public void AddRecordForPurchaseOrder()
    {
        string ConvertexchangeRate = string.Empty;
        DataTable DtProduct = CreateProductDataTableForPurchaseOrder();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        //try
        //{
        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        ConvertexchangeRate = SystemParameter.GetExchageRate(Request.QueryString["CurId"].ToString(), ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        //    ConvertexchangeRate = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Request.QueryString["CurId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
        //}
        //catch
        //{
        //    DataTable dt = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + Request.QueryString["CurId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dt.Rows.Count != 0)
        //    {
        //        ConvertexchangeRate = dt.Rows[0]["Currency_Value"].ToString();
        //    }
        //}
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                dr["Trans_Id"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
                dr["Trans_Id"] = "1";
            }
            dr["PONo"] = "";
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            string UnitCost = string.Empty;
            DataTable dtContactPriceList = objCustomerPriceList.GetContactPriceList(Session["CompId"].ToString(), Request.QueryString["SupId"].ToString(), "S");
            try
            {
                dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + gvProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtContactPriceList.Rows.Count > 0)
            {
                UnitCost = dtContactPriceList.Rows[0]["Sales_Price"].ToString();
                try
                {
                    UnitCost = (Convert.ToDouble(UnitCost) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(ConvertexchangeRate)).ToString();
                    UnitCost = ObjSysPeram.GetCurencyConversionForInv(Request.QueryString["CurId"].ToString(), UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }
            else
            {
                try
                {
                    UnitCost = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(ConvertexchangeRate)).ToString();
                    UnitCost = ObjSysPeram.GetCurencyConversionForInv(Request.QueryString["CurId"].ToString(), UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }
            dr["UnitCost"] = UnitCost;
            dr["OrderQty"] = gvQuantity.Text;
            dr["freeQty"] = "0";
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    #region Purchase Invoice
    public DataTable CreateProductDataTableForPurchaseInvoice()
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("TransID");
        DtProduct.Columns.Add("SerialNo", typeof(int));
        DtProduct.Columns.Add("POId");
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("OrderQty");
        DtProduct.Columns.Add("InvoiceQty");
        DtProduct.Columns.Add("FreeQty");
        DtProduct.Columns.Add("RecQty");
        DtProduct.Columns.Add("TaxV");
        DtProduct.Columns.Add("DiscountP");
        DtProduct.Columns.Add("TaxP");
        DtProduct.Columns.Add("DiscountV");
        DtProduct.Columns.Add("InvRemainQty");
        DtProduct.Columns.Add("RemainQty");
        DtProduct.Columns.Add("PONO");
        return DtProduct;
    }
    public void AddRecordForPurchaseInvoice()
    {
        string ConvertexchangeRate = string.Empty;
        DataTable DtProduct = CreateProductDataTableForPurchaseInvoice();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        //try
        //{
        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        ConvertexchangeRate = SystemParameter.GetExchageRate(Request.QueryString["CurId"].ToString(), ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        //    ConvertexchangeRate = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Request.QueryString["CurId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
        //}
        //catch
        //{
        //    DataTable dt = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + Request.QueryString["CurId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dt.Rows.Count != 0)
        //    {
        //        ConvertexchangeRate = dt.Rows[0]["Currency_Value"].ToString();
        //    }
        //}
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "SerialNo desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["SerialNo"] = (float.Parse(DtMaxSerial.Rows[0]["SerialNo"].ToString()) + 1).ToString();
                dr["TransId"] = (float.Parse(DtMaxSerial.Rows[0]["SerialNo"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["SerialNo"] = "1";
                dr["TransId"] = "1";
            }
            string UnitCost = string.Empty;
            DataTable dtContactPriceList = objCustomerPriceList.GetContactPriceList(Session["CompId"].ToString(), Request.QueryString["SupId"].ToString(), "S");
            try
            {
                dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + gvProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtContactPriceList.Rows.Count > 0)
            {
                UnitCost = dtContactPriceList.Rows[0]["Sales_Price"].ToString();
                try
                {
                    UnitCost = (Convert.ToDouble(UnitCost) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(ConvertexchangeRate)).ToString();
                    UnitCost = ObjSysPeram.GetCurencyConversionForInv(Request.QueryString["CurId"].ToString(), UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }
            else
            {
                UnitCost = "0";
            }
            dr["POID"] = "0";
            dr["ProductId"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            dr["OrderQty"] = gvQuantity.Text;
            dr["InvoiceQty"] = gvQuantity.Text;
            dr["FreeQty"] = "0";
            dr["RecQty"] = "0";
            dr["UnitCost"] = UnitCost;
            dr["TaxP"] = "0";
            dr["TaxV"] = "0";
            dr["DiscountP"] = "0";
            dr["DiscountV"] = "0";
            dr["InvRemainQty"] = gvQuantity.Text;
            dr["RemainQty"] = gvQuantity.Text;
            dr["PONO"] = "0";
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
    }
    #endregion
    protected string GetUnitName(string strUnitId)
    {
        Inv_UnitMaster UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }
    protected void ddlGridSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDataListGrid();
    }
    protected void btnAddtoLIst_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Page"].ToString() == "SI")
        {
            AddRecordForSalesInquiry();
        }
        if (Request.QueryString["Page"].ToString() == "SQ")
        {
            AddRecordForSalesQuotation();
        }
        if (Request.QueryString["Page"].ToString() == "SO")
        {
            AddRecordForSalesOrder();
        }
        if (Request.QueryString["Page"].ToString() == "SIN")
        {
            AddRecordForSalesInvoice();
        }
        if (Request.QueryString["Page"].ToString() == "PR")
        {
            AddRecordForPurchaseRequest();
        }
        if (Request.QueryString["Page"].ToString() == "PI")
        {
            AddRecordForPurchaseInquiry();
        }
        if (Request.QueryString["Page"].ToString() == "PO")
        {
            AddRecordForPurchaseOrder();
        }
        if (Request.QueryString["Page"].ToString() == "PIN")
        {
            AddRecordForPurchaseInvoice();
        }
        if (Request.QueryString["Page"].ToString() == "TR")
        {
            AddRecordForTransferRequest();
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
    }
    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
        btnBack.Visible = false;
        DataTable dtproduct = new DataTable();
        string condition = string.Empty;
        //InventoryDataSet rptdata = new InventoryDataSet();
        //rptdata.EnforceConstraints = false;
        //InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter();
        //adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        //adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Session["FinanceYearId"].ToString()), "", "", "");
        //dtproduct = rptdata.sp_Inv_ProductMaster_SelectRow_Report;
       
        if (ddlOption.SelectedIndex != 0)
        {
            condition = "1=1";
            if (ddlbrandsearch.SelectedIndex != 0)
            {
                try
                {
                    condition += " and PBrandId='" + ddlbrandsearch.SelectedValue + "'";
                    //dtproduct = new DataView(dtproduct, "PBrandId='" + ddlbrandsearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            if (ddlcategorysearch.SelectedIndex != 0)
            {
                try
                {
                    condition += " and CategoryId='" + ddlcategorysearch.SelectedValue + "'";
                    //dtproduct = new DataView(dtproduct, "CategoryId='" + ddlcategorysearch.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }

            
            if (ddlOption.SelectedIndex == 1)
            {
                if (ddlFieldName.SelectedValue != "AlternateId")
                {
                    condition += " and " + ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
                    //condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                }
                else
                {
                    condition += " and AlternateId1='" + txtValue.Text.Trim() + "' or AlternateId2='" + txtValue.Text.Trim() + "' or AlternateId3='" + txtValue.Text.Trim() + "')";
                    //condition = "convert(AlternateId1,System.String)='" + txtValue.Text.Trim() + "' or convert(AlternateId2,System.String)='" + txtValue.Text.Trim() + "' or convert(AlternateId3,System.String)='" + txtValue.Text.Trim() + "'";
                }
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                if (ddlFieldName.SelectedValue != "AlternateId")
                {
                    condition += " and " + ddlFieldName.SelectedValue  + " Like '" + txtValue.Text.Trim() + "%'";
                    //condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                }
                else
                {
                    condition += " and (AlternateId1 Like '" + txtValue.Text.Trim() + "' or AlternateId2 Like '" + txtValue.Text.Trim() + "' or AlternateId3 like '" + txtValue.Text.Trim() + "')";
                    //condition = "convert(AlternateId1,System.String) Like '" + txtValue.Text.Trim() + "' or convert(AlternateId2,System.String) Like '" + txtValue.Text.Trim() + "' or convert(AlternateId3,System.String)='" + txtValue.Text.Trim() + "'";
                }
            }
            else
            {
                if (ddlFieldName.SelectedValue != "AlternateId")
                {
                    condition += " and " + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                    //condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                }
                else
                {
                    condition += " and (AlternateId1 Like '" + txtValue.Text.Trim() + "' or AlternateId2 Like '" + txtValue.Text.Trim() + "' or AlternateId3='" + txtValue.Text.Trim() + "')";
                    //condition = "convert(AlternateId1,System.String) Like '" + txtValue.Text.Trim() + "' or convert(AlternateId2,System.String) Like '" + txtValue.Text.Trim() + "' or convert(AlternateId3,System.String)='" + txtValue.Text.Trim() + "'";
                }
            }
            //DataView view = new DataView(dtproduct, condition, "", DataViewRowState.CurrentRows);

            dtproduct = ObjProductMaster.getProductTableForAdvancFilter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), condition);
            if (dtproduct!=null && dtproduct.Rows.Count > 0)
            {
                try
                {
                    dtproduct = dtproduct.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "AlternateId", "StockQuantity", "Description", "ModelName", "ShortProductName", "model_no", "CategoryID");
                }
                catch
                {
                }

                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString() + "";
                if (ddlGridSize.SelectedValue == "0")
                {
                    dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * PageControlCommon.GetPageSize()).Take(PageControlCommon.GetPageSize()).CopyToDataTable();
                }
                if (ddlGridSize.SelectedValue == "All")
                {
                    dtlistProduct.DataSource = dtproduct;
                }
                if (ddlGridSize.SelectedValue != "0" && ddlGridSize.SelectedValue != "All")
                {
                    dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * Convert.ToInt32(ddlGridSize.SelectedValue)).Take(Convert.ToInt32(ddlGridSize.SelectedValue)).CopyToDataTable();
                }
                dtlistProduct.DataBind();
                CheckedAddProductList();
            }
            else
            {
                dtlistProduct.DataSource = null;
                dtlistProduct.DataBind();
            }
        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
        txtValue.Focus();
        FillDataListGrid();
        btnBack.Visible = false;
    }
    #region //User Defined Function Start
    public void FillddlBrandSearch(DropDownList ddl)
    {
        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(StrCompId.ToString());
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
    private void FillProductCategorySerch(DropDownList ddl)
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());
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
    #endregion
    protected void chkAddItem_CheckedChanged(object sender, EventArgs e)
    {
        DataListItem Row = (DataListItem)((CheckBox)sender).Parent;
        CheckBox chkAddItem = (CheckBox)Row.FindControl("chkAddItem");
        TextBox txtquantity = (TextBox)Row.FindControl("txtquantity");
        HiddenField hdnProductId = (HiddenField)Row.FindControl("hdnProductId");
        HiddenField hdnMaintainStock = (HiddenField)Row.FindControl("hdnMaintainStock");
        Label lblProductCode = (Label)Row.FindControl("lbldlProductId");
        HiddenField hdnunitId = (HiddenField)Row.FindControl("hdnunitId");
        Label lblDesc = (Label)Row.FindControl("lblDesc");
        LinkButton lbldlProductName = (LinkButton)Row.FindControl("lbldlProductName");
        LinkButton btnRelatedProduct = (LinkButton)Row.FindControl("btnRelatedProduct");
        if (((CheckBox)sender).Checked)
        {
            if (txtquantity.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Enter Quantity');", true);
                ((CheckBox)sender).Checked = false;
                txtquantity.Focus();
                return;
            }
            if (Session["DtSearchProduct"] != null)
            {
                if (Request.QueryString["Page"].ToString() == "PIN" || Request.QueryString["Page"].ToString() == "TR")
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "ProductId='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
                else
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "Product_Id='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
            }
            DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value.ToString());
            if (dtRelatedProduct.Rows.Count > 0 && btnBack.Visible == false)
            {
                btnRelatedProduct.Visible = true;
            }
        }
        else
        {
            btnRelatedProduct.Visible = false;
        }
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("ProductCode");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("MaintainStock");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SuggestedProductName");
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            HiddenField gvhdnMaintainStock = (HiddenField)gvRow.FindControl("hdnMaintainStock");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = gvProductId.Value;
            dr["MaintainStock"] = gvhdnMaintainStock.Value;
            dr["ProductCode"] = gvProductCode.Text;
            dr["Quantity"] = gvQuantity.Text;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            DtProduct.Rows.Add(dr);
        }
        if (chkAddItem.Checked == true)
        {
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = hdnProductId.Value;
            dr["MaintainStock"] = hdnMaintainStock.Value;
            dr["ProductCode"] = lblProductCode.Text;
            dr["Quantity"] = txtquantity.Text;
            dr["UnitId"] = hdnunitId.Value;
            dr["ProductDescription"] = lblDesc.Text;
            dr["SuggestedProductName"] = lbldlProductName.Text;
            DtProduct.Rows.Add(dr);
        }
        else
        {
            try
            {
                DtProduct = new DataView(DtProduct, "ProductId<>" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            txtquantity.Text = "1";
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        ViewState["DtAddProduct"] = DtProduct;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtProduct = (DataTable)ViewState["DtAddProduct"];
        try
        {
            dtProduct = new DataView(dtProduct, "ProductId<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        ViewState["DtAddProduct"] = dtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
        CheckedAddProductList();
    }
    public string GetSalesPrice(string ProductId, string SalesPrice)
    {
        string SalesPriceValue = "0";
        if (Request.QueryString["CustomerId"] != null)
        {
            try
            {
                SalesPriceValue = ObjSysPeram.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), ObjProductMaster.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "C", Request.QueryString["CustomerId"].ToString(), ProductId).Rows[0]["Sales_Price"].ToString());
            }
            catch
            {
                SalesPriceValue = "0";
            }
        }
        else
        {
            SalesPriceValue = SalesPrice;
        }
        return SalesPriceValue;
    }
    public string GetSalesPriceinLocal(string Amount)
    {
        string SalesPrice = string.Empty;
        try
        {
            SalesPrice = ObjSysPeram.GetCurencyConversionForInv(ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(Amount) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString());
        }
        catch
        {
            SalesPrice = "0";
        }
        return SalesPrice;
    }
    public string GetAmountDecimal(string Amount)
    {
        return ObjSysPeram.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);
    }
    #region RelatedProduct
    protected void btnRelatedProduct_Click(object sender, EventArgs e)
    {
        DataTable DtFilterCategory = new DataTable();
        btnBack.Visible = true;
        DataListItem Row = (DataListItem)((LinkButton)sender).Parent;
        HiddenField hdnProductId = (HiddenField)Row.FindControl("hdnProductId");
        DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value);
        string SubProductId = string.Empty;
        foreach (DataRow dr in dtRelatedProduct.Rows)
        {
            if (SubProductId == "")
            {
                SubProductId = dr["SubProduct_Id"].ToString();
            }
            else
            {
                SubProductId = SubProductId + "," + dr["SubProduct_Id"].ToString();
            }
        }
        ViewState["SubProductId"] = SubProductId;
        DataTable dtproduct = new DataTable();
        InventoryDataSet rptdata = new InventoryDataSet();
        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Session["FinanceYearId"].ToString()), "", "", "");
        dtproduct = rptdata.sp_Inv_ProductMaster_SelectRow_Report;
        DtFilterCategory = dtproduct.Copy();
        try
        {
            dtproduct = dtproduct.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "AlternateId", "StockQuantity", "Description", "ShortProductName");
        }
        catch
        {
        }
        try
        {
            dtproduct = new DataView(dtproduct, "ProductId in (" + SubProductId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtproduct.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)dtlistProduct, dtproduct, "", "");
            CheckedAddProductList();
            try
            {
                DtFilterCategory = new DataView(DtFilterCategory, "ProductId in (" + SubProductId + ") and Category_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtFilterCategory.Rows.Count > 0)
            {
                DtFilterCategory = new DataView(DtFilterCategory, "", "Category_Name asc", DataViewRowState.CurrentRows).ToTable();
                DtFilterCategory = DtFilterCategory.DefaultView.ToTable(true, "Category_Name", "CategoryId");
                pnlChkCategory.Visible = true;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)chkCategory, DtFilterCategory, "Category_Name", "CategoryId");
            }
            else
            {
                pnlChkCategory.Visible = false;
                chkCategory.DataSource = null;
                chkCategory.DataBind();
            }
        }
        else
        {
            dtlistProduct.DataSource = null;
            dtlistProduct.DataBind();
            pnlChkCategory.Visible = false;
            chkCategory.DataSource = null;
            chkCategory.DataBind();
            ViewState["SubProductId"] = null;
        }
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        btnBack.Visible = false;
        FillDataListGrid();
        pnlChkCategory.Visible = false;
        chkCategory.DataSource = null;
        chkCategory.DataBind();
        ViewState["SubProductId"] = null;
    }
    protected void chkCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string CategoryList = string.Empty;
        for (int i = 0; i < chkCategory.Items.Count; i++)
        {
            if (chkCategory.Items[i].Selected == true)
            {
                if (CategoryList == "")
                {
                    CategoryList = chkCategory.Items[i].Value;
                }
                else
                {
                    CategoryList = CategoryList + "," + chkCategory.Items[i].Value;
                }
            }
        }
        DataTable dtproduct = new DataTable();
        InventoryDataSet rptdata = new InventoryDataSet();
        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_ProductMaster_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_ProductMaster_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Session["FinanceYearId"].ToString()), "", "", "");
        dtproduct = rptdata.sp_Inv_ProductMaster_SelectRow_Report;
        if (CategoryList != "")
        {
            try
            {
                dtproduct = new DataView(dtproduct, "CategoryId in (" + CategoryList + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        try
        {
            dtproduct = dtproduct.DefaultView.ToTable(true, "ProductCode", "ProductId", "EProductName", "UnitId", "MaintainStock", "SalesPrice", "Unit_Name", "ItemType", "InventoryType", "CostPrice", "CurrencyId", "AlternateId1", "AlternateId2", "AlternateId3", "AlternateId", "StockQuantity", "Description");
        }
        catch
        {
        }
        try
        {
            dtproduct = new DataView(dtproduct, "ProductId in (" + ViewState["SubProductId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtproduct.Rows.Count > 0)
        {
            if (ddlGridSize.SelectedValue == "0")
            {
                dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * PageControlCommon.GetPageSize()).Take(PageControlCommon.GetPageSize()).CopyToDataTable();
            }
            if (ddlGridSize.SelectedValue == "All")
            {
                dtlistProduct.DataSource = dtproduct;
            }
            if (ddlGridSize.SelectedValue != "0" && ddlGridSize.SelectedValue != "All")
            {
                dtlistProduct.DataSource = dtproduct.Rows.Cast<System.Data.DataRow>().Skip((1 - 1) * Convert.ToInt32(ddlGridSize.SelectedValue)).Take(Convert.ToInt32(ddlGridSize.SelectedValue)).CopyToDataTable();
            }
            dtlistProduct.DataBind();
            CheckedAddProductList();
        }
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
    }
    #endregion
    #region LOcationStock
    protected void btnCloseStockPanel_Click(object sender, EventArgs e)
    {
        pnlStock1.Visible = false;
        pnlStock2.Visible = false;
    }
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=&&Contact=','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    protected void btnRPC_Click(object sender, CommandEventArgs e)
    {
        DataListItem Row = (DataListItem)((Button)sender).Parent;
        HiddenField hdnProductId = (HiddenField)Row.FindControl("hdnProductId");
        GridView gv = (GridView)Row.FindControl("gvRelCat");

        if (((Button)sender).ID != "btnHideRPC")
        {
            Inv_ProductMaster pm = new Inv_ProductMaster(Session["DBConnection"].ToString());
            DataTable dt = pm.GetProductDataByCategoryId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString(), e.CommandArgument.ToString());
            gv.DataSource = dt;
            gv.DataBind();
            dt = null;
        }
        else
        {
            gv.DataSource = null;
            gv.DataBind();
        }
    }

    protected void chkRelCat_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(((Control)sender).NamingContainer);
        CheckBox chkAddItem = (CheckBox)sender;
        TextBox txtquantity = (TextBox)Row.FindControl("gvTxtRelCatQuantity");
        HiddenField hdnProductId = (HiddenField)Row.FindControl("hdnRelCatProductId");
        HiddenField hdnMaintainStock = (HiddenField)Row.FindControl("hdnRelCatMaintainStock");
        Label lblProductCode = (Label)Row.FindControl("gvLblRelCatProductcode");
        HiddenField hdnunitId = (HiddenField)Row.FindControl("hdnRelCatUnitId");
        Label lblProductName = (Label)Row.FindControl("gvLblRelCatProductName");

        if (((CheckBox)sender).Checked)
        {
            if (txtquantity.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Enter Quantity');", true);
                ((CheckBox)sender).Checked = false;
                txtquantity.Focus();
                return;
            }
            if (Session["DtSearchProduct"] != null)
            {
                if (Request.QueryString["Page"].ToString() == "PIN" || Request.QueryString["Page"].ToString() == "TR")
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "ProductId='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
                else
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "Product_Id='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
            }
        }

        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("ProductCode");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("MaintainStock");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SuggestedProductName");
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            HiddenField gvhdnMaintainStock = (HiddenField)gvRow.FindControl("hdnMaintainStock");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = gvProductId.Value;
            dr["MaintainStock"] = gvhdnMaintainStock.Value;
            dr["ProductCode"] = gvProductCode.Text;
            dr["Quantity"] = gvQuantity.Text;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            DtProduct.Rows.Add(dr);
        }
        if (chkAddItem.Checked == true)
        {
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = hdnProductId.Value;
            dr["MaintainStock"] = hdnMaintainStock.Value;
            dr["ProductCode"] = lblProductCode.Text;
            dr["Quantity"] = txtquantity.Text;
            dr["UnitId"] = hdnunitId.Value;
            dr["ProductDescription"] = "";
            dr["SuggestedProductName"] = lblProductName.Text;
            DtProduct.Rows.Add(dr);
        }
        else
        {
            try
            {
                DtProduct = new DataView(DtProduct, "ProductId<>" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            txtquantity.Text = "1";
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        ViewState["DtAddProduct"] = DtProduct;
    }
    protected void chkRelMod_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)(((Control)sender).NamingContainer);
        CheckBox chkAddItem = (CheckBox)sender;
        TextBox txtquantity = (TextBox)Row.FindControl("gvTxtRelModQuantity");
        HiddenField hdnProductId = (HiddenField)Row.FindControl("hdnRelModProductId");
        HiddenField hdnMaintainStock = (HiddenField)Row.FindControl("hdnRelModMaintainStock");
        Label lblProductCode = (Label)Row.FindControl("gvLblRelModProductcode");
        HiddenField hdnunitId = (HiddenField)Row.FindControl("hdnRelModUnitId");
        Label lblProductName = (Label)Row.FindControl("gvLblRelModProductName");

        if (((CheckBox)sender).Checked)
        {
            if (txtquantity.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Enter Quantity');", true);
                ((CheckBox)sender).Checked = false;
                txtquantity.Focus();
                return;
            }
            if (Session["DtSearchProduct"] != null)
            {
                if (Request.QueryString["Page"].ToString() == "PIN" || Request.QueryString["Page"].ToString() == "TR")
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "ProductId='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
                else
                {
                    if (new DataView((DataTable)Session["DtSearchProduct"], "Product_Id='" + hdnProductId.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Product Already Exists');", true);
                        ((CheckBox)sender).Checked = false;
                        return;
                    }
                }
            }
        }

        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("ProductCode");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("MaintainStock");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SuggestedProductName");
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            HiddenField gvhdnMaintainStock = (HiddenField)gvRow.FindControl("hdnMaintainStock");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = gvProductId.Value;
            dr["MaintainStock"] = gvhdnMaintainStock.Value;
            dr["ProductCode"] = gvProductCode.Text;
            dr["Quantity"] = gvQuantity.Text;
            dr["UnitId"] = hdngvunitId.Value;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            DtProduct.Rows.Add(dr);
        }
        if (chkAddItem.Checked == true)
        {
            DataRow dr = DtProduct.NewRow();
            dr["ProductId"] = hdnProductId.Value;
            dr["MaintainStock"] = hdnMaintainStock.Value;
            dr["ProductCode"] = lblProductCode.Text;
            dr["Quantity"] = txtquantity.Text;
            dr["UnitId"] = hdnunitId.Value;
            dr["ProductDescription"] = "";
            dr["SuggestedProductName"] = lblProductName.Text;
            DtProduct.Rows.Add(dr);
        }
        else
        {
            try
            {
                DtProduct = new DataView(DtProduct, "ProductId<>" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            txtquantity.Text = "1";
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        ViewState["DtAddProduct"] = DtProduct;

    }
    protected void btnRPM_Click(object sender, CommandEventArgs e)
    {
        DataListItem Row = (DataListItem)((Button)sender).Parent;
        GridView gv = (GridView)Row.FindControl("gvRelMod");
        Inv_ProductMaster pm = new Inv_ProductMaster(Session["DBConnection"].ToString());

        if(((Button)sender).ID != "btnHideRPM")
        {
            DataTable dt = pm.GetRelatedProductDataByModelNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString(), e.CommandArgument.ToString());
            gv.DataSource = dt;
            gv.DataBind();
            dt = null;
        }
        else
        {            
            gv.DataSource = null;
            gv.DataBind();            
        }


    }

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void fillLocation()
    {
        DataTable dtLoc = ObjLocMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                //if (dtLoc.Rows.Count > 1)
                //{
                //    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                //}
            }
            //else
            //{
            //    ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            //}
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            //if (dtLoc.Rows.Count > 1 && LocIds != "")
            //{
            //    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            //}
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDataListGrid();
    }
    
    protected void lnkStockInfo_Command1(object sender, CommandEventArgs e)
    {
        modelSA.getProductDetail(e.CommandArgument.ToString(), "", "");
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
}