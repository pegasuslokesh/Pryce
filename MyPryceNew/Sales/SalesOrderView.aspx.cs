using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
public partial class Sales_SalesOrderView : System.Web.UI.Page
{
    #region defined Class Object
    Common cmn = null;
    IT_ObjectEntry objObjectEntry = null;
    Set_Approval_Employee objEmpApproval = null;
    PurchaseOrderHeader ObjPurchaseOrderheader = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Inv_SalesQuotationHeader objSQuoteHeader = null;
    Inv_SalesQuotationDetail ObjSQuoteDetail = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_ProductMaster objProductM = null;
    Set_BankMaster ObjBankMaster = null;
    Inv_StockDetail objStockDetail = null;
    Ac_ChartOfAccount objCOA = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    Set_AddressMaster objAddMaster = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_ParameterMaster objInvParam = null;
    Contact_PriceList objCustomerPriceList = null;
    Set_CustomerMaster objCustomer = null;
    DataAccessClass objda = null;
    LocationMaster objLocation = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();
    Inv_ProductCategory_Tax objProTax = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    TaxMaster objTaxMaster = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Inv_ProductionRequestHeader objProductionRequest = null;

    Ac_ParameterMaster objAcParameter = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string StrUserId = string.Empty;
    string StrCurrencyId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjPurchaseOrderheader = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSQuoteDetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objProTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objProductionRequest = new Inv_ProductionRequestHeader(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        TaxandDiscountParameter();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        if (!IsPostBack)
        {
            FillUnit();
            FillCurrency();
            FillPaymentMode();
            try
            {
                ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                StrCurrencyId = ddlCurrency.SelectedValue;
            }
            catch
            {
            }
            GetOrderDetail(Request.QueryString["OrderId"].ToString());
        }
       
        try
        {
            StrCurrencyId = ddlCurrency.SelectedValue;
        }
        catch
        {
        }
    }
    private void FillPaymentMode()
    {
        DataTable dsPaymentMode = null;
        dsPaymentMode = objPaymentMode.GetPaymentModeMaster(StrCompId, Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            dsPaymentMode = new DataView(dsPaymentMode, "Pay_Mod_Name in ('Cash','Credit')", "Pay_Mode_Id asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        if (dsPaymentMode.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            ddlPaymentMode.DataSource = dsPaymentMode;
            ddlPaymentMode.DataTextField = "Pay_Mod_Name";
            ddlPaymentMode.DataValueField = "Pay_Mode_Id";
            ddlPaymentMode.DataBind();
            //objPageCmn.FillData((object)ddlPaymentMode, dsPaymentMode, "Pay_Mod_Name", "Pay_Mode_Id");

            ddlPaymentMode.SelectedIndex = 0;
        }
        else
        {
            ddlPaymentMode.Items.Insert(0, "--Select--");
            ddlPaymentMode.SelectedIndex = 0;
        }

        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
    }
    void TaxandDiscountParameter()
    {

        lblAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, Resources.Attendance.Gross_Total, Session["DBConnection"].ToString());
        lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total", Session["DBConnection"].ToString());
        lblNetAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total", Session["DBConnection"].ToString());

        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
            {

                GvQuotationDetail.Columns[13].Visible = false;
                GvQuotationDetail.Columns[14].Visible = false;
                GvQuotationDetail.Columns[15].Visible = false;
                GvProductDetail.Columns[0].Visible = false;
                GvProductDetail.Columns[14].Visible = false;
                GvProductDetail.Columns[15].Visible = false;
                GvProductDetail.Columns[16].Visible = false;

                lblTaxP.Visible = false;
                lblTaxPcolon.Visible = false;
                txtTaxP.Visible = false;
                Label2.Visible = false;
                txtTaxV.Visible = false;
                gridView.Visible = false;


            }
            else
            {

                GvQuotationDetail.Columns[13].Visible = true;
                GvQuotationDetail.Columns[14].Visible = true;

                GvProductDetail.Columns[14].Visible = true;
                GvProductDetail.Columns[15].Visible = true;
                GvProductDetail.Columns[0].Visible = true;
               
                lblTaxP.Visible = true;
                lblTaxPcolon.Visible = true;
                txtTaxP.Visible = true;
                Label2.Visible = true;
                txtTaxV.Visible = true;
                Label2.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Value", Session["DBConnection"].ToString());

                
                gridView.Visible = true;
            }
        }
        Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
            {
                GvQuotationDetail.Columns[10].Visible = false;
                GvQuotationDetail.Columns[11].Visible = false;
                GvQuotationDetail.Columns[12].Visible = false;

                GvProductDetail.Columns[11].Visible = false;
                GvProductDetail.Columns[12].Visible = false;
                GvProductDetail.Columns[13].Visible = false;
              
                lblDiscountP.Visible = false;
                txtDiscountP.Visible = false;
                Label3.Visible = false;
                txtDiscountV.Visible = false;
                lblDiscountPvolon.Visible = false;
               

                lblafterDiscountPrice.Visible = false;
                lblPriceafterdiscountcolon.Visible = false;
                txtPriceAfterDiscount.Visible = false;
            }
            else
            {
                lblafterDiscountPrice.Visible = true;
                lblPriceafterdiscountcolon.Visible = true;
                txtPriceAfterDiscount.Visible = true;
                GvQuotationDetail.Columns[10].Visible = true;
                GvQuotationDetail.Columns[11].Visible = true;
                GvProductDetail.Columns[11].Visible = true;
                GvProductDetail.Columns[12].Visible = true;
               
                lblDiscountP.Visible = true;
                txtDiscountP.Visible = true;
                Label3.Visible = true;
                Label3.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Value", Session["DBConnection"].ToString());
                txtDiscountV.Visible = true;
                lblDiscountPvolon.Visible = true;
               

            }
        }


    }
    private void FillUnit()
    {
        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            DropDownList ddlUnit = (DropDownList)gvr.FindControl("ddlgvUnit");

            DataTable dsUnit = null;
            dsUnit = UM.GetUnitMaster(StrCompId);
            if (dsUnit.Rows.Count > 0)
            {

                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                objPageCmn.FillData((object)ddlUnit, dsUnit, "Unit_Name", "Unit_Id");

            }
            else
            {
                ddlUnit.Items.Add("--Select--");
                ddlUnit.SelectedValue = "--Select--";
            }
        }
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");

        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
    }
    public string GetProductStock(string strProductId)
    {

        string SysQty = string.Empty;
        try
        {
            SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
        }
        catch
        {
            SysQty = "0";
        }


        if (SysQty == "")
        {
            SysQty = "0.000";
        }

        return GetAmountDecimal(SysQty);
    }
    public void GetOrderDetail(string strOrderId)
    {
        DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, strOrderId);

        string PaymentType = string.Empty;

        try
        {
            PaymentType = new DataView(objPaymentMode.GetPaymentModeMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()), "Pay_Mode_Id=" + dtOrderEdit.Rows[0]["PaymentModeId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString();
        }
        catch
        {
            PaymentType = "Cash";
        }

       

        if (dtOrderEdit.Rows.Count > 0)
        {
            editid.Value = strOrderId;

            
            string strCustomerId = dtOrderEdit.Rows[0]["CustomerId"].ToString();
            txtCustomer.Text = dtOrderEdit.Rows[0]["CustomerName"].ToString() + "/" + strCustomerId;
            //For Checking Finance Record

            txtSONo.Text = dtOrderEdit.Rows[0]["SalesOrderNo"].ToString();
            ViewState["TimeStamp"] = dtOrderEdit.Rows[0]["Row_Lock_Id"].ToString();
            txtSONo.ReadOnly = true;
            txtSODate.Text = Convert.ToDateTime(dtOrderEdit.Rows[0]["SalesOrderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            string strSOFromTransType = dtOrderEdit.Rows[0]["SOfromTransType"].ToString();
            txtCustOrderNo.Text = dtOrderEdit.Rows[0]["Field3"].ToString();
            ViewState["Status"] = dtOrderEdit.Rows[0]["Field4"].ToString();
            ddlCurrency.SelectedValue = dtOrderEdit.Rows[0]["Currency_Id"].ToString();
            StrCurrencyId = ddlCurrency.SelectedValue;
            lblAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, Resources.Attendance.Gross_Total, Session["DBConnection"].ToString());
            lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total", Session["DBConnection"].ToString());
            lblNetAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total", Session["DBConnection"].ToString());
            ddlDeliveryvoucher.SelectedValue = dtOrderEdit.Rows[0]["IsdeliveryVoucher"].ToString();
            //for get agent name according condition 

            string strAgentId = dtOrderEdit.Rows[0]["Agent_Id"].ToString();
            if (strAgentId != "" && strAgentId != "0")
            {
                txtAgentName.Text = dtOrderEdit.Rows[0]["Agent_Name"].ToString() + "/" + strAgentId;
                try
                {
                    GvProductDetail.Columns[GvProductDetail.Columns.Count - 2].Visible = true;
                    GvQuotationDetail.Columns[GvQuotationDetail.Columns.Count - 1].Visible = true;
                }
                catch
                {
                }
            }
            fillPaymentGrid(ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "SO", editid.Value.ToString()));
            DataTable dtRefDetailHeader = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", editid.Value);
            try
            {
                dtRefDetailHeader = new DataView(dtRefDetailHeader, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            chkPartialShipment.Checked = Convert.ToBoolean(dtOrderEdit.Rows[0]["Field6"].ToString());


            dtRefDetailHeader = dtRefDetailHeader.DefaultView.ToTable(true, "Tax_Id", "TaxName", "Tax_Per", "Tax_Value");
            ViewState["dtTaxHeader"] = dtRefDetailHeader;
            LoadStores();

            if (strSOFromTransType == "Q")
            {
                txtAgentName.Enabled = false;
                ddlOrderType.SelectedValue = "Q";
                ddlOrderType_SelectedIndexChanged(null,null);
                ddlQuotationNo.Visible = false;
                txtQuotationNo.Visible = true;
                hdnSalesQuotationId.Value = dtOrderEdit.Rows[0]["SOfromTransNo"].ToString();


                DataTable dtQuotationHeader = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value);
                if (dtQuotationHeader.Rows.Count > 0)
                {
                    txtQuotationNo.Text = dtQuotationHeader.Rows[0]["SQuotation_No"].ToString();

                    DataTable dtQuoteChild = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value,Session["FinanceYearId"].ToString());
                    if (dtQuoteChild.Rows.Count > 0)
                    {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)GvQuotationDetail, dtQuoteChild, "", "");

                        GvQuotationDetail.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "", Session["DBConnection"].ToString());
                        GvQuotationDetail.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Price", Session["DBConnection"].ToString());
                        GvQuotationDetail.HeaderRow.Cells[11].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Value", Session["DBConnection"].ToString());
                        GvQuotationDetail.HeaderRow.Cells[14].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Value", Session["DBConnection"].ToString());
                        GvQuotationDetail.HeaderRow.Cells[16].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "", Session["DBConnection"].ToString());

                        GvQuotationDetail.Columns[12].Visible = false;
                        GvQuotationDetail.Columns[15].Visible = false;
                        FillUnit();
                        Decimal fGrossTotal = 0;
                        Decimal fquantityTotal = 0;
                        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
                        {
                            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                            DropDownList ddlgvUnit = (DropDownList)gvr.FindControl("ddlgvUnit");
                            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                            TextBox txtgvFreeQuantity = (TextBox)gvr.FindControl("txtgvFreeQuantity");
                            Label txtgvRemainQuantity = (Label)gvr.FindControl("txtgvRemainQuantity");
                            TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
                            TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
                            TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                            TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
                            TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                            TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
                            TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
                            CheckBox chkgvQuantitySelect = (CheckBox)gvr.FindControl("chkGvQuotationDetailSelect");
                            CheckBox chkIsProduction = (CheckBox)gvr.FindControl("chkIsProduction");
                            TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
                            DataTable dtOrderDetailByProductId = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, editid.Value);

                            dtOrderDetailByProductId = new DataView(dtOrderDetailByProductId, "Product_Id='" + hdngvProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                            if (dtOrderDetailByProductId.Rows.Count > 0)
                            {
                                chkgvQuantitySelect.Checked = true;
                                lblgvQuantity.Text = dtOrderDetailByProductId.Rows[0]["Quantity"].ToString();
                                txtgvFreeQuantity.Text = dtOrderDetailByProductId.Rows[0]["FreeQty"].ToString();

                                ddlgvUnit.SelectedValue = dtOrderDetailByProductId.Rows[0]["UnitId"].ToString();
                                txtgvRemainQuantity.Text = dtOrderDetailByProductId.Rows[0]["RemainQty"].ToString();
                                txtgvUnitPrice.Text = dtOrderDetailByProductId.Rows[0]["UnitPrice"].ToString();

                                txtgvTaxP.Text = dtOrderDetailByProductId.Rows[0]["TaxP"].ToString();
                                txtgvTaxV.Text = dtOrderDetailByProductId.Rows[0]["TaxV"].ToString();
                                //txtgvPriceAfterTax.Text = (Decimal.Parse(txtgvQuantityPrice.Text) + Decimal.Parse(txtgvTaxV.Text)).ToString();
                                txtgvDiscountP.Text = dtOrderDetailByProductId.Rows[0]["DiscountP"].ToString();
                                txtgvDiscountV.Text = dtOrderDetailByProductId.Rows[0]["DiscountV"].ToString();
                                txtgvAgentCommission.Text = GetAmountDecimal(dtOrderDetailByProductId.Rows[0]["AgentCommission"].ToString());
                                string[] strcalc = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, "", txtgvTaxV.Text, "", txtgvDiscountV.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());
                                chkIsProduction.Checked = Convert.ToBoolean(dtOrderDetailByProductId.Rows[0]["Field6"].ToString());
                                txtgvQuantityPrice.Text = strcalc[0].ToString();
                                txtgvTotal.Text = strcalc[5].ToString();
                            }
                            else
                            {
                                chkgvQuantitySelect.Checked = false;
                                if (hdngvProductId.Value != "0" && hdngvProductId.Value != "")
                                {
                                    for (int i = 0; i < dtQuoteChild.Rows.Count; i++)
                                    {
                                        string strProductId = dtQuoteChild.Rows[i]["Product_Id"].ToString();
                                        if (strProductId == hdngvProductId.Value)
                                        {

                                            txtgvUnitPrice.Text = dtQuoteChild.Rows[i]["UnitPrice"].ToString();
                                            ddlgvUnit.SelectedValue = dtQuoteChild.Rows[0]["Field1"].ToString();

                                            txtgvTaxP.Text = dtQuoteChild.Rows[i]["TaxPercent"].ToString();
                                            txtgvTaxV.Text = dtQuoteChild.Rows[i]["TaxValue"].ToString();

                                            txtgvDiscountP.Text = dtQuoteChild.Rows[i]["DiscountPercent"].ToString();
                                            txtgvDiscountV.Text = dtQuoteChild.Rows[i]["DiscountValue"].ToString();
                                            txtgvAgentCommission.Text = GetAmountDecimal(dtQuoteChild.Rows[i]["AgentCommission"].ToString());
                                            string[] strcalc = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, "", txtgvTaxV.Text, "", txtgvDiscountV.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());
                                            txtgvQuantityPrice.Text = strcalc[0].ToString();
                                            txtgvTotal.Text = strcalc[5].ToString();
                                        }
                                    }
                                }

                            }
                            if (chkgvQuantitySelect.Checked)
                            {
                                if (txtgvTotal.Text != "")
                                {
                                    fGrossTotal = fGrossTotal + Decimal.Parse(txtgvTotal.Text);
                                }
                                if (txtgvQuantityPrice.Text != "")
                                {
                                    fquantityTotal = fquantityTotal + Decimal.Parse(txtgvQuantityPrice.Text);
                                }
                            }
                        }
                        txtAmount.Text = fquantityTotal.ToString();
                        txtTotalAmount.Text = fGrossTotal.ToString();
                    }
                }
            }
            else
            {
                ddlOrderType.SelectedValue = "D";
                ddlOrderType_SelectedIndexChanged(null,null);

                DataTable dtOrderDetail = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, editid.Value);
                if (dtOrderDetail.Rows.Count > 0)
                {
                    DataTable dtRefDetail = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", editid.Value);
                    try
                    {
                        dtRefDetail = new DataView(dtRefDetail, "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    dtRefDetail = dtRefDetail.DefaultView.ToTable(true, "ProductId", "ProductCategoryId", "CategoryName", "TaxName", "Tax_Per", "Tax_value", "TaxSelected", "Tax_Id");
                    ViewState["DtTax"] = dtRefDetail;

                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)GvProductDetail, dtOrderDetail, "", "");

                    GvProductDetail.Columns[13].Visible = false;
                    GvProductDetail.Columns[16].Visible = false;
                }
            }
            if (dtOrderEdit.Rows[0]["SOfromTransType"].ToString() == "Q")
            {
                txtTransFrom.Text = "Sales Quotation";

            }
            else
            {
                txtTransFrom.Text = "Direct";
            }
            strCustomerId = dtOrderEdit.Rows[0]["Field2"].ToString();
            txtShipCustomerName.Text = dtOrderEdit.Rows[0]["ShipCustomerName"].ToString() + "/" + strCustomerId;
            try
            {
                ddlPaymentMode.SelectedValue = dtOrderEdit.Rows[0]["PaymentModeId"].ToString();
            }
            catch
            {
                ddlPaymentMode.SelectedIndex = 0;
            }

            fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
            txtEstimateDeliveryDate.Text = Convert.ToDateTime(dtOrderEdit.Rows[0]["EstimateDeliveryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            string strAddressId = dtOrderEdit.Rows[0]["ShipToAddressID"].ToString();

            DataTable dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, StrCompId);
            if (dtAddName.Rows.Count > 0)
            {
                txtShipingAddress.Text = dtAddName.Rows[0]["Address_Name"].ToString();
            }
            else
            {
                txtShipingAddress.Text = "";
            }
            strAddressId = dtOrderEdit.Rows[0]["Field1"].ToString();
            dtAddName = null;
            dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, StrCompId);
            if (dtAddName.Rows.Count > 0)
            {
                txtInvoiceTo.Text = dtAddName.Rows[0]["Address_Name"].ToString();
            }
            else
            {
                txtInvoiceTo.Text = "";
            }

            txtTaxP.Text = dtOrderEdit.Rows[0]["TaxP"].ToString();
            txtTaxV.Text = dtOrderEdit.Rows[0]["TaxV"].ToString();
            try
            {
                txtPriceAfterTax.Text = (Decimal.Parse(txtAmount.Text) + Decimal.Parse(txtTaxV.Text)).ToString();
            }
            catch
            {
                txtPriceAfterTax.Text = txtAmount.Text;
            }
            txtDiscountP.Text = dtOrderEdit.Rows[0]["DiscountP"].ToString();
            txtDiscountV.Text = dtOrderEdit.Rows[0]["DiscountV"].ToString();

            try
            {
                txtTotalAmount.Text = (Decimal.Parse(txtPriceAfterTax.Text) - Decimal.Parse(txtDiscountV.Text)).ToString();
            }
            catch
            {
                txtTotalAmount.Text = txtPriceAfterTax.Text;
            }

            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            if (txtDiscountV.Text == "")
            {
                txtDiscountV.Text = "0";
            }
            txtPriceAfterDiscount.Text = (float.Parse(txtAmount.Text) - float.Parse(txtDiscountV.Text)).ToString();
            if (dtOrderEdit.Rows[0]["SOfromTransType"].ToString() == "D")
            {
                GetGridTotal();
            }
            txtShippingCharge.Text = dtOrderEdit.Rows[0]["ShippingCharge"].ToString();
            txtRemark.Text = dtOrderEdit.Rows[0]["Remark"].ToString();

            txtNetAmount.Text = (Decimal.Parse(txtTotalAmount.Text) + Decimal.Parse(txtShippingCharge.Text)).ToString();
            string strPost = dtOrderEdit.Rows[0]["Post"].ToString();
            string strSendInPO = dtOrderEdit.Rows[0]["IsInPO"].ToString();
            DataTable dt = new DataTable();

            if (strSendInPO == "1")
            {
                try
                {
                    dt = new DataView(ObjPurchaseOrderheader.GetPurchaseOrderHeader(), "SalesOrderID=" +editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                chkSendInPO.Checked = true;

                if (dt.Rows.Count > 0)
                {

                    pnlDetail.Enabled = false;
                    chkSendInPO.Enabled = false;
                }

            }
            if (strSendInPO == "2")
            {
                chksendInproduction.Checked = true;
                try
                {
                    dt = new DataView(objProductionRequest.GetAllRecord(), "Field1='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    pnlDetail.Enabled = false;
                    chksendInproduction.Enabled = false;
                }
            }
        }
       
       
       
        setDecimal();
    

    }

    public void LoadStores()
    {
        DataTable dt = new DataTable();
        if (ViewState["dtTaxHeader"] != null)
        {
            dt = new DataTable();

            dt = (DataTable)ViewState["dtTaxHeader"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gridView, dt, "", "");


            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Tax_Id", typeof(int));
                contacts.Columns.Add("TaxName", typeof(string));
                contacts.Columns.Add("Tax_Per", typeof(float));
                contacts.Columns.Add("Tax_Value", typeof(float));

                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gridView, contacts, "", "");



                int TotalColumns = gridView.Rows[0].Cells.Count;

                gridView.Rows[0].Cells.Clear();

                gridView.Rows[0].Cells.Add(new TableCell());

                gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gridView.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Tax_Id", typeof(int));
            contacts.Columns.Add("TaxName", typeof(string));
            contacts.Columns.Add("Tax_Per", typeof(float));
            contacts.Columns.Add("Tax_Value", typeof(float));

            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gridView, contacts, "", "");


            int TotalColumns = gridView.Rows[0].Cells.Count;

            gridView.Rows[0].Cells.Clear();

            gridView.Rows[0].Cells.Add(new TableCell());

            gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gridView.Rows[0].Visible = false;
        }


    }
    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        trTransfer.Visible = false;
        txtTransFrom.Text = "";
        GvQuotationDetail.DataSource = null;
        GvQuotationDetail.DataBind();
        txtAmount.Text = "";
        txtTaxP.Text = "";
        txtTaxV.Text = "";
        txtPriceAfterTax.Text = "";
        txtDiscountP.Text = "";
        txtDiscountV.Text = "";
        txtTotalAmount.Text = "";
       
        GvProductDetail.DataSource = null;
        GvProductDetail.DataBind();
        txtNetAmount.Text = txtShippingCharge.Text.ToString();

        txtPriceAfterDiscount.Text = "";
        txtInvoiceTo.Text = "";
        txtShipCustomerName.Text = "";
        txtShipingAddress.Text = "";
     
        if (ddlOrderType.SelectedValue == "D")
        {
            trTransfer.Visible = false;
            txtTransFrom.Text = "";
            GvQuotationDetail.DataSource = null;
            GvQuotationDetail.DataBind();
            txtAmount.Text = "";
            txtTaxP.Text = "";
            txtTaxV.Text = "";
            txtPriceAfterTax.Text = "";
            txtDiscountP.Text = "";
            txtDiscountV.Text = "";
            txtTotalAmount.Text = "";
            txtPriceAfterDiscount.Text = "";
            
            txtQuotationNo.Text = "";
        }
        else if (ddlOrderType.SelectedValue == "Q")
        {
            trTransfer.Visible = true;
            txtTransFrom.Text = "Sales Quotation";
            ddlQuotationNo.Visible = true;
            txtQuotationNo.Visible = false;
          
        }
    }
    protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridView.EditIndex = e.NewEditIndex;
        LoadStores();
    }




    protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        TextBox txtTaxName = (TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxName");
        TextBox txtTaxValue = (TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxValue");






        DataTable dt = new DataTable();
        if (ViewState["dtTaxHeader"] != null)
        {

            dt = (DataTable)ViewState["dtTaxHeader"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Tax_Id"].ToString() == ((HiddenField)gridView.Rows[e.RowIndex].FindControl("hdnTaxId")).Value)
                {
                    dt.Rows[i]["Tax_Name"] = txtTaxName.Text;
                    dt.Rows[i]["Tax_Value"] = txtTaxValue.Text;
                    break;
                }
            }

        }
        ViewState["dtTaxHeader"] = dt;

       
        gridView.EditIndex = -1;
        LoadStores();


    }
    protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {


        LoadStores();
        gridView.EditIndex = -1;
    }
    protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        DataTable dt = new DataTable();
        string TaxId = "";
        if (e.CommandName.Equals("AddNew"))
        {

          

            TextBox txtTaxName = (TextBox)gridView.FooterRow.FindControl("txtTaxFooter");
            TextBox txtTaxValue = (TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter");



            DataTable dtTax = objTaxMaster.GetTaxMasterTrueAll();
            dtTax = new DataView(dtTax, "Tax_Name='" + txtTaxName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTax.Rows.Count > 0)
            {

                TaxId = dtTax.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
              
                return;
            }


            if (ViewState["dtTaxHeader"] == null)
            {
                dt.Columns.Add("Tax_Id", typeof(int));
                dt.Columns.Add("TaxName", typeof(string));
                dt.Columns.Add("Tax_Per", typeof(float));
                dt.Columns.Add("Tax_Value", typeof(float));
                DataRow dr = dt.NewRow();
                dr[0] = TaxId;
                dr[1] = txtTaxName.Text;
                dr[2] = ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text;
                dr[3] = txtTaxValue.Text;
                dt.Rows.Add(dr);
            }
            else
            {

                DataView view = new DataView((DataTable)ViewState["dtTaxHeader"], "Tax_Id=" + TaxId + "", "", DataViewRowState.CurrentRows);

                if (view.ToTable().Rows.Count > 0)
                {
                   
                    return;
                }

                dt = (DataTable)ViewState["dtTaxHeader"];
                DataRow dr = dt.NewRow();
                dr[0] = TaxId;
                dr[1] = txtTaxName.Text;
                dr[2] = ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text;
                dr[3] = txtTaxValue.Text;
                dt.Rows.Add(dr);
            }

            ViewState["dtTaxHeader"] = dt;
        }

        if (e.CommandName.Equals("Delete"))
        {

            if (ViewState["dtTaxHeader"] != null)
            {

                dt = (DataTable)ViewState["dtTaxHeader"];

                dt = new DataView(dt, "Tax_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                ViewState["dtTaxHeader"] = dt;

            }

        }



        if (e.CommandName.Equals("Update"))
        {



        }




        gridView.EditIndex = -1;
        LoadStores();

        TotalheaderTax();

    }
    public void TotalheaderTax()
    {
        float netTaxPer = 0;
        float nettaxval = 0;


        for (int i = 0; i < gridView.Rows.Count; i++)
        {

            try
            {
                netTaxPer += float.Parse(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text);
                nettaxval += float.Parse(((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text);
            }
            catch
            {
            }
        }
        txtTaxP.Text = GetAmountDecimal(netTaxPer.ToString());
        txtTaxV.Text = GetAmountDecimal(nettaxval.ToString());

        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, StrCurrencyId, true, Session["DBConnection"].ToString());
        txtTaxP.Text = str[3].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        if (txtShippingCharge.Text == "")
        {
            txtNetAmount.Text = txtTotalAmount.Text;
        }
        else
        {
            txtNetAmount.Text = GetAmountDecimal((Convert.ToDouble(txtTotalAmount.Text) + Convert.ToDouble(txtShippingCharge.Text)).ToString());
        }
        if (GvProductDetail.Rows.Count > 0)
        {

            TaxDiscountFromHeader(false);
        }

        if (GvQuotationDetail.Rows.Count > 0)
        {

            TaxDiscountFromHeader_InDirect(false);
        }
      
    }
    public void TaxDiscountFromHeader_InDirect(bool IsDiscount)
    {


        double netPrice = 0;
        double netTax = 0;
        double netDiscount = 0;


        foreach (GridViewRow Row in GvQuotationDetail.Rows)
        {
            TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");
            CheckBox chkGvQuotationDetailSelect = (CheckBox)Row.FindControl("chkGvQuotationDetailSelect");

            if (chkGvQuotationDetailSelect.Checked)
            {
                string[] str;
                if (IsDiscount)
                {


                    str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, ((TextBox)Row.FindControl("txtgvTaxP")).Text, "", txtDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


                    ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                    ((TextBox)Row.FindControl("txtgvDiscountV")).Text = str[2].ToString();
                    ((TextBox)Row.FindControl("txtgvDiscountP")).Text = str[1].ToString();

                    ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
                }
                else
                {

                    str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, txtTaxP.Text, "", "", ((TextBox)Row.FindControl("txtgvDiscountV")).Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());


                    ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                    ((TextBox)Row.FindControl("txtgvTaxP")).Text = str[3].ToString();
                    ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();

                }
            }

        }
        CalculationchangeIntaxGridview();
        setDecimal();
    }
    public void TaxDiscountFromHeader(bool IsDiscount)
    {
        double netPrice = 0;
        double netTax = 0;
        double netDiscount = 0;


        foreach (GridViewRow Row in GvProductDetail.Rows)
        {
            TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");

            string[] str;
            if (IsDiscount)
            {


                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblgvUnitPrice")).Text, lblgvQuantity.Text, ((TextBox)Row.FindControl("lblgvTaxP")).Text, "", txtDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


                ((TextBox)Row.FindControl("lblgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("lblgvDiscountV")).Text = str[2].ToString();
                ((TextBox)Row.FindControl("lblgvDiscountP")).Text = str[1].ToString();

                ((Label)Row.FindControl("lblgvTotal")).Text = str[5].ToString();
            }
            else
            {

                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblgvUnitPrice")).Text, lblgvQuantity.Text, txtTaxP.Text, "", "", ((TextBox)Row.FindControl("lblgvDiscountV")).Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());


                ((TextBox)Row.FindControl("lblgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("lblgvTaxP")).Text = str[3].ToString();
                ((Label)Row.FindControl("lblgvTotal")).Text = str[5].ToString();

            }
        }


        CalculationchangeIntaxGridview();
    }

    protected void GvQuotationDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.S_No_;
                cell.ColumnSpan = 1;

            
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = Resources.Attendance.Product_Detail;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Gross_Price;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();



            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Discount;
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                {
                    cell.ColumnSpan = 0;
                }
                else
                {
                    row.Controls.Add(cell);
                }

            }
            else
            {
                row.Controls.Add(cell);
            }



            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;
            Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                {
                    cell.ColumnSpan = 0;
                }
                else
                {
                    row.Controls.Add(cell);
                }
            }
            else
            {
                row.Controls.Add(cell);
            }



            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Line_total;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "Is Production";
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "Stock";
            row.Controls.Add(cell);


            if (txtAgentName.Text != "")
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Agent";
                row.Controls.Add(cell);
            }

            gvProduct.Controls[0].Controls.Add(row);
        }
    }
    protected void GvProductDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DataTable Dt = new DataTable();
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableHeaderCell cell = new TableHeaderCell();

           

            //cell = new TableHeaderCell();
            //cell.ColumnSpan = 1;
            //cell.Text = Resources.Attendance.Delete;
            //row.Controls.Add(cell);

            cell = new TableHeaderCell();
            //}
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.S_No_;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = Resources.Attendance.Product_Detail;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Gross_Price;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();




            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Discount;
            Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                {
                    cell.ColumnSpan = 0;
                }
                else
                {
                    row.Controls.Add(cell);
                }

            }
            else
            {
                row.Controls.Add(cell);
            }


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;
            Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                {
                    cell.ColumnSpan = 0;
                }
                else
                {
                    row.Controls.Add(cell);
                }

            }
            else
            {
                row.Controls.Add(cell);
            }


            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Total;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "Is Production";
            row.Controls.Add(cell);

            if (txtAgentName.Text != "")
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Agent";
                row.Controls.Add(cell);
            }

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "Stock";
            row.Controls.Add(cell);

            gvProduct.Controls[0].Controls.Add(row);
        }


    }
    protected void GvProductDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = GvProductDetail.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvchildGrid = (GridView)e.Row.FindControl("gvchildGrid");
            if (objProTax.GetRecord_ByProductId(ProductID).Rows.Count > 0)
            {
                ((ImageButton)e.Row.FindControl("imgBtnaddtax")).Visible = true;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvchildGrid, objProTax.GetRecord_ByProductId(ProductID), "", "");


                if (ViewState["DtTax"] != null)
                {
                    DataTable dt = new DataView((DataTable)ViewState["DtTax"], "ProductId='" + ProductID + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)gvchildGrid, dt, "", "");


                    }
                }
            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgBtnaddtax")).Visible = false;
            }
        }
    }
    public void fillTabPaymentMode(string PaymentType)
    {
        DataTable dt = objPaymentMode.GetPaymentModeMaster(StrCompId.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        dt = new DataView(dt, "Field1='" + PaymentType.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlTabPaymentMode, dt, "Pay_Mod_Name", "Pay_Mode_Id");

    }
    protected void GetGridTotal()
    {

        Double fGrossTotal = 0;
        Double fGrossTax = 0;
        Double fGrossDis = 0;

        foreach (GridViewRow gvr in GvProductDetail.Rows)
        {
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            Label lblgvTotal = (Label)gvr.FindControl("lblgvTotal");
            Label lblgvQuantityPrice = (Label)gvr.FindControl("lblgvQuantityPrice");
            TextBox lblgvTaxV = (TextBox)gvr.FindControl("lblgvTaxV");
            TextBox lblgvDiscountV = (TextBox)gvr.FindControl("lblgvDiscountV");

            if (lblgvQuantityPrice.Text == "")
            {
                lblgvQuantityPrice.Text = "0";
            }


            if (lblgvTotal.Text != "" && lblgvTotal.Text != "0")
            {
                fGrossTotal = fGrossTotal + Convert.ToDouble(lblgvQuantityPrice.Text);
            }


            if (lblgvTaxV.Text == "")
            {
                lblgvTaxV.Text = "0";
            }
            fGrossTax = fGrossTax + (Convert.ToDouble(lblgvTaxV.Text) * Convert.ToDouble(lblgvQuantity.Text));

            if (lblgvDiscountV.Text == "")
            {
                lblgvDiscountV.Text = "0";
            }
            fGrossDis = fGrossDis + (Convert.ToDouble(lblgvDiscountV.Text) * Convert.ToDouble(lblgvQuantity.Text));

        }


        string[] strHeaderCalculation = Common.TaxDiscountCaluculation(fGrossTotal.ToString(), "0", "", fGrossTax.ToString(), "", fGrossDis.ToString(), false, StrCurrencyId, false, Session["DBConnection"].ToString());

        txtAmount.Text = GetAmountDecimal(fGrossTotal.ToString());
        txtTaxV.Text = GetAmountDecimal(fGrossTax.ToString());
        txtDiscountV.Text = GetAmountDecimal(fGrossDis.ToString());
        txtTaxP.Text = strHeaderCalculation[3].ToString();
        txtDiscountP.Text = strHeaderCalculation[1].ToString();
        txtTotalAmount.Text = strHeaderCalculation[5].ToString();


        if (txtShippingCharge.Text == "")
        {
            txtNetAmount.Text = strHeaderCalculation[5].ToString();
        }
        else
        {
            txtNetAmount.Text = GetAmountDecimal((Decimal.Parse(txtTotalAmount.Text) + Decimal.Parse(txtShippingCharge.Text)).ToString());
        }

        try
        {
            txtPriceAfterDiscount.Text = (float.Parse(txtAmount.Text) - float.Parse(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }
        setDecimal();
        CalculationchangeIntaxGridview();
    }
    public void CalculationchangeIntaxGridview()
    {
        if (txtPriceAfterDiscount.Text != "")
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                try
                {
                    if (((Label)gridView.Rows[i].FindControl("lblTaxper")).Text != "")
                    {

                        if (txtPriceAfterDiscount.Text != "" && txtPriceAfterDiscount.Text != "0")
                        {

                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = GetAmountDecimal((float.Parse(txtPriceAfterDiscount.Text) * (float.Parse(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text) / 100)).ToString());
                        }
                        else
                        {
                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = "0";
                        }
                    }
                }
                catch
                {
                }
            }
        }

    }
    public void setDecimal()
    {

        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            ((TextBox)gvr.FindControl("lblgvQuantity")).Text = ((TextBox)gvr.FindControl("lblgvQuantity")).Text;
            ((TextBox)gvr.FindControl("txtgvUnitPrice")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvUnitPrice")).Text);

            ((TextBox)gvr.FindControl("txtgvQuantityPrice")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvQuantityPrice")).Text);
            ((TextBox)gvr.FindControl("txtgvTaxP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvTaxP")).Text);
            ((TextBox)gvr.FindControl("txtgvTaxV")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvTaxV")).Text);
            ((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text);
            ((TextBox)gvr.FindControl("txtgvDiscountP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvDiscountP")).Text);
            ((TextBox)gvr.FindControl("txtgvDiscountV")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvDiscountV")).Text);
            ((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text);
            ((TextBox)gvr.FindControl("txtgvTotal")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvTotal")).Text);
            ((TextBox)gvr.FindControl("txtgvAgentCommission")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvAgentCommission")).Text);
        }
        foreach (GridViewRow gvr in GvProductDetail.Rows)
        {

            ((TextBox)gvr.FindControl("lblgvQuantity")).Text = ((TextBox)gvr.FindControl("lblgvQuantity")).Text;

            ((TextBox)gvr.FindControl("lblgvUnitPrice")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvUnitPrice")).Text);

            ((Label)gvr.FindControl("lblgvQuantityPrice")).Text = GetAmountDecimal(((Label)gvr.FindControl("lblgvQuantityPrice")).Text);

            ((TextBox)gvr.FindControl("lblgvTaxP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvTaxP")).Text);

            ((TextBox)gvr.FindControl("lblgvTaxV")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvTaxV")).Text);

            ((TextBox)gvr.FindControl("lblgvDiscountP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvDiscountP")).Text);


            ((TextBox)gvr.FindControl("lblgvDiscountV")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvDiscountV")).Text);


            ((Label)gvr.FindControl("lblgvTotal")).Text = GetAmountDecimal(((Label)gvr.FindControl("lblgvTotal")).Text);
            ((TextBox)gvr.FindControl("txtgvAgentCommission")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvAgentCommission")).Text);

        }


        txtNetAmount.Text = GetAmountDecimal(txtNetAmount.Text);
       
        txtTaxP.Text = GetAmountDecimal(txtTaxP.Text);
        txtTaxV.Text = GetAmountDecimal(txtTaxV.Text);
        txtTotalAmount.Text = GetAmountDecimal(txtTotalAmount.Text);
        txtDiscountP.Text = GetAmountDecimal(txtDiscountP.Text);
        txtDiscountV.Text = GetAmountDecimal(txtDiscountV.Text);
        txtPriceAfterTax.Text = GetAmountDecimal(txtPriceAfterTax.Text);
        txtAmount.Text = GetAmountDecimal(txtAmount.Text);
        txtShippingCharge.Text = GetAmountDecimal(txtShippingCharge.Text);
        txtPriceAfterDiscount.Text = GetAmountDecimal(txtPriceAfterDiscount.Text);

    }

    public void fillPaymentGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");
      
        float f = 0;

        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            try
            {

                ((Label)gvrow.FindControl("lblAmount")).Text = GetAmountDecimal(((Label)gvrow.FindControl("lblAmount")).Text);
                f += float.Parse(((Label)gvrow.FindControl("lblAmount")).Text);
            }
            catch
            {
                ((Label)gvrow.FindControl("lblAmount")).Text = "0";
                f += 0;
            }
        }
        try
        {
            ((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text = GetAmountDecimal(f.ToString());
        }
        catch
        {

        }

        //here we showing balance amount
        if (txtNetAmount.Text != "")
        {
            if (float.Parse(txtNetAmount.Text) > 0)
            {
                try
                {
                    txtPayAmount.Text = GetAmountDecimal((float.Parse(txtNetAmount.Text) - float.Parse(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                }
                catch
                {

                    txtPayAmount.Text = txtNetAmount.Text;
                }
            }
        }


    }


    public string GetAmountDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(StrCurrencyId, Amount);

    }

    protected string GetProductName(string strProductId)
    {
        string strProductName = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(StrCompId, StrBrandId, strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductName = dtPName.Rows[0]["EProductName"].ToString();
            }
        }
        else
        {
            strProductName = "";
        }
        return strProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(StrCompId.ToString(), StrBrandId, ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "0";


        }

        return ProductName;

    }
    protected string GetProductDescription(string strProductId)
    {
        string strProductDesc = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(StrCompId, StrBrandId, strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductDesc = dtPName.Rows[0]["Description"].ToString();
            }
        }
        else
        {
            strProductDesc = "";
        }
        return strProductDesc;
    }

    protected string GetUnitName(string strUnitId)
    {
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(StrCompId, strUnitId);
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
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }

}