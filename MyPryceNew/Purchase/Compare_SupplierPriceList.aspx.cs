using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Purchase_PurchaseRequestPrint : BasePage
{
    PurchaseRequestHeader InvPr = null;
    PurchaseRequestDetail InvPrDetails = null;
    SystemParameter ObjSysPeram = null;
    Inv_ProductMaster ObjProductMaster = null;

    Inv_UnitMaster objUnit = null;
    CompanyMaster ObjCompanyMaster = null;
    Set_AddressChild objAddChild =null;
    Set_AddressMaster ObjAddMaster = null;
    Set_Suppliers objSupplier = null;
    Contact_PriceList objSupplierPriceList = null;
    PurchaseInvoice ObjPurchaseInvoiceHeader = null;
    PurchaseInvoiceDetail ObjPurchaseInvoiceDetail = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        InvPrDetails = new PurchaseRequestDetail(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objSupplierPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        ObjPurchaseInvoiceHeader = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjPurchaseInvoiceDetail = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        if (Request.QueryString["SupplierId"] != null)
        {
            try
            {
                fillRequest(Request.QueryString["SupplierId"].ToString());
            }
            catch
            {

            }
        }
    }
    public void fillRequest(string SupplierId)
    {
        DataTable DtSupplier = new DataTable();
        DtSupplier = objSupplier.GetSupplierAllTrueData(strCompId, strBrandId);
        try
        {
            DtSupplier = new DataView(DtSupplier, "Supplier_Id in (" + SupplierId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)datalistSupplier, DtSupplier, "", "");

        DataTable dtComp = ObjCompanyMaster.GetCompanyMasterById(Session["CompId"].ToString());
        lblCompanyName.Text = dtComp.Rows[0]["Company_Name"].ToString();
        string RefName = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString()).Rows[0]["Address_Name"].ToString();
        DataTable dtAdd = ObjAddMaster.GetAddressDataByAddressName(RefName);
        lblAddress.Text = dtAdd.Rows[0]["Address"].ToString();
        lblPhone.Text = dtAdd.Rows[0]["PhoneNo1"].ToString();
        try
        {
            imgCompany.ImageUrl = "../CompanyResource" + "/" + dtComp.Rows[0]["Company_Id"].ToString().Trim() + "/" + dtComp.Rows[0]["Logo_Path"].ToString();
        }
        catch
        {
        }


    }
    protected void datalistSupplier_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        GridView gvSup = (GridView)e.Item.FindControl("gvSupplier");
        HiddenField hdnSupplierId = (HiddenField)e.Item.FindControl("hdnSupplierId");
        DataTable DtProduct = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        try
        {
            DtProduct = new DataView(DtProduct, "ProductId in (" + Session["SupplierProductList"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSup, DtProduct, "", "");
        foreach (GridViewRow gvrow in gvSup.Rows)
        {
            Label lblProductId = (Label)gvrow.FindControl("lblProductId");
            Label lblgvLastPurchasePrice = (Label)gvrow.FindControl("lblgvLastPurchasePrice");
            Label lblgvSalesPrice = (Label)gvrow.FindControl("lblgvSalesPrice");
            lblgvLastPurchasePrice.Text = "";
            DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(Session["CompId"].ToString(), hdnSupplierId.Value, "S");
            try
            {
                dtContactPriceList = new DataView(dtContactPriceList, "Product_Id =" + lblProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtContactPriceList.Rows.Count > 0)
            {
                lblgvSalesPrice.Text = ObjSysPeram.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtContactPriceList.Rows[0]["Sales_Price"].ToString());
            }
            else
            {
                lblgvSalesPrice.Text = "";
            }
            DataTable DtInvoiceHeader = ObjPurchaseInvoiceHeader.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                DtInvoiceHeader = new DataView(DtInvoiceHeader, "SupplierId ='" + hdnSupplierId.Value + "'", "CreatedDate desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (DtInvoiceHeader.Rows.Count > 0)
            {
                DataTable DtInvoiceDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                try
                {
                    DtInvoiceDetail = new DataView(DtInvoiceDetail, "InvoiceNo=" + DtInvoiceHeader.Rows[0]["TransID"].ToString() + " and  ProductId =" + lblProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtInvoiceDetail.Rows.Count > 0)
                {
                    lblgvLastPurchasePrice.Text = DtInvoiceDetail.Rows[0]["UnitCost"].ToString();
                }
                else
                {
                    lblgvLastPurchasePrice.Text = "";
                }
            }
            else
            {
                lblgvLastPurchasePrice.Text = "";
            }


        }

    }

    protected void imgPrint_Click(object sender, ImageClickEventArgs e)
    {
        fillRequest(Request.QueryString["SupplierId"].ToString());

    }


}
