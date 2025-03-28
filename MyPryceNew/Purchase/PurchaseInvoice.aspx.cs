using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.OleDb;

using ClosedXML.Excel;
using System.IO;
public partial class Purchase_PurchaseInvoice : BasePage
{
    #region Defined Class Object
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnit = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    PurchaseOrderDetail ObjPurchaseOrderDetail = null;
    SystemParameter ObjSysParam = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ems_ContactMaster ObjContactMaster = null;
    PurchaseInvoice ObjPurchaseInvoice = null;
    PurchaseInvoiceDetail ObjPurchaseInvoiceDetail = null;
    Inv_StockDetail objStockDetail = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ShipExpHeader ObjShipExpHeader = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseInquiryHeader ObjPIHeader = null;
    Common cmn = null;
    Inv_ParameterMaster objInvParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Set_BankMaster ObjBankMaster = null;
    Contact_PriceList objSupplierPriceList = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Inv_ProductLedger ObjProductLadger = null;
    Set_Suppliers objSupplier = null;
    Ac_ChartOfAccount objCOA = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    LocationMaster objLocation = null;
    Contact_PriceList objContactPriceList = null;
    Inv_ProductCategory_Tax objProTax = null;
    TaxMaster objTaxMaster = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    Country_Currency objCountryCurrency = null;
    ProductTaxMaster objProductTaxMaster = null;
    Inv_SizeMaster ObjSizeMaster = null;
    Inv_ColorMaster ObjColorMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;

    public const int grdDefaultColCount = 10;
    private const string strPageName = "PurchaseInvoice";
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string UserId = string.Empty;
    string strDepartmentId = string.Empty;
    #endregion
    public static DataTable Dt_Final_Save_Tax;
    public static int Decimal_Count_For_Tax;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Expenses_Tax_Purchase_Invoice"] != null)
        {
            Dt_Final_Save_Tax = Session["Expenses_Tax_Purchase_Invoice"] as DataTable;
        }
        else
        {
            Dt_Final_Save_Tax = null;
        }

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        ObjPurchaseOrderDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjPurchaseInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjPurchaseInvoiceDetail = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
        ObjShipExpHeader = new Inv_ShipExpHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        ObjBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        objSupplierPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objContactPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objProTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjSizeMaster = new Inv_SizeMaster(Session["DBConnection"].ToString());
        ObjColorMaster = new Inv_ColorMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        UserId = Session["UserId"].ToString();
        strDepartmentId = Session["DepartmentId"].ToString();
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());

        if (Common.DtPhysical != null)
        {
            DataTable dt = new DataTable();
            dt = Common.DtPhysical;
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "CompanyId='" + StrCompId + "' and BrandId='" + Session["BrandId"].ToString() + "' and LocationId='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TransType"].ToString() == "1")
                    {
                        DisplayMessage("Don't transaction ,Stock Work is going On... ");
                        Response.Redirect("../MasterSetup/Home.aspx");
                    }
                }
            }
        }
        //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        //btnSave.Attributes.Add("onclick", "Confirm()");
        if (!IsPostBack)
        {
            Session["IsPurchaseTaxEnabled"] = null;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseInvoice.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocList);
            if (Session["Expenses_Purchase_Invoice_Local"] != null)
            {
                DataTable Dt_temp = Session["Expenses_Purchase_Invoice_Local"] as DataTable;
                if (Dt_temp.Rows.Count > 0)
                {
                    Expenses_Read_Only();
                }
            }
            Session["Expenses_Tax_Purchase_Invoice"] = null;
            Session["Temp_Product_Tax_PI"] = null;

            if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
            {
                if (Session["LocId"].ToString() == "7" || Session["LocId"].ToString() == "8")
                {
                    Div_Add_Tax.Visible = true;
                }
                else
                {
                    Div_Add_Tax.Visible = false;
                }
            }
            else
            {
                Div_Add_Tax.Visible = false;
            }

            string Decimal_Count = string.Empty;
            Decimal_Count = Session["LoginLocDecimalCount"].ToString();
            Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
            bool Tax_Apply = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Session["Is_Tax_Apply"] = Tax_Apply.ToString();
            setPurchaseAccount();
            FillTransactionType();
            //btnSave.Attributes.Add("onclick", "Confirm()");
            StrBrandId = Session["BrandId"].ToString();
            StrLocationId = Session["LocId"].ToString();
            SetRunningBillConfiguration();
            txtInvoiceNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtInvoiceNo.Text;
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender4.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            txtInvoicedate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSupInvoiceDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillCurrency(ddlCurrency);
            FillCurrency(ddlExpCurrency);
            FillCurrency(ddlPayCurrency);
            FillGrid();
            fillExpenses();
            //btnList_Click(null, null);
            btnReset_Click(null, null);
            ViewState["Dtproduct"] = null;
            ViewState["Dis"] = "";
            ViewState["Tax"] = "";
            Session["VPost"] = "False";
            txtChequedate_CalenderExtender.Format = Session["DateFormat"].ToString();
            Session["DtSearchProduct"] = null;
            try
            {
                TxtCurrencyValue.Text = Session["LocCurrencyId"].ToString();
                ViewState["CurrencyId"] = Session["LocCurrencyId"].ToString();
                ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
                hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
            }
            catch
            {
            }
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            ViewState["ExchangeRateinComp"] = SystemParameter.GetExchageRate(Session["LocCurrencyId"].ToString(), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            txtExchangeRate.Text = "1";
            btnAddSupplier.Visible = IsAddCustomerPermission();
            LoadStores();
            Session["DtAdvancePayment"] = null;
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            FillShipUnitddl();
            CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();
            FillddlLocation();
            fillBank();
            //For Redirect 
            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                string strLocId = StrLocationId;
                try
                {
                    strLocId = Request.QueryString["LocId"].ToString();
                }
                catch
                {

                }
                btnEdit_Command(imgeditbutton, new CommandEventArgs(strLocId, Request.QueryString["Id"].ToString()));
                //btnList.Visible = false;
                //btnPendingOrder.Visible = false;
                //btnPReturnCancel.Visible = false;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_LI()", true);
                //((Panel)Master.FindControl("pnlaccordian")).Visible = false;
            }
            Expenses_Tax_Modal.Expenses_Tax_And_Amount_Clear();
            Btn_Exp_Reset_Click(null, null);
            DataTable Dt_Individual = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Allow TAX edit on individual transactions Purchase");
            if (Dt_Individual.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt_Individual.Rows[0]["ParameterValue"]) == true)
                    Btn_Update_Tax.Visible = true;
                else
                    Btn_Update_Tax.Visible = false;
            }
            else
            {
                Btn_Update_Tax.Visible = false;
            }
            getPageControlsVisibility();

        }
        Lbl_Expenses_Tax_Amount_ET.Text = Expenses_Tax_Modal.Expenses_Amount_Value();
        Lbl_Expenses_Tax_ET.Text = Expenses_Tax_Modal.Expenses_Tax_Value();

        TaxandDiscountandAccountParameter();
        if (gvProduct.Rows.Count > 0)
            fillPaymentGrid((DataTable)ViewState["PayementDt"]);
        GetSymbol();
        if (Session["Is_Tax_Apply"] != null && Session["Is_Tax_Apply"].ToString() == "False")
            Trans_Div.Visible = false;
        DataTable dtExp = new DataTable();
        dtExp = (DataTable)ViewState["Expdt"];
        if (dtExp != null)
        {
            fillExpGrid(dtExp);
        }
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
        Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            //  ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        ImgbtnProductSave.Visible = clsPagePermission.bAdd;
        Btn_Add_Expenses.Visible = clsPagePermission.bAdd;
        Btn_Exp_Reset.Visible = clsPagePermission.bAdd;
        try
        {
            hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
            hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
            hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
            hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
            hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        }
        catch
        {
        }

        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;

    }
    #endregion
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    public void FillTransactionType()
    {
        ddlTransType.DataSource = Enum
        .GetValues(typeof(Common.TransactionType))
        .Cast<Common.TransactionType>()
        .Select(s => new KeyValuePair<int, string>((int)s, s.ToString()))
        .ToList();
        ddlTransType.DataValueField = "Key";
        ddlTransType.DataTextField = "Value";
        ddlTransType.DataBind();
        ddlTransType.Items.Insert(0, new ListItem("--Select--", "-1"));
    }
    public void SetRunningBillConfiguration()
    {
        ddlBillType.SelectedIndex = 1;
        if (objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Is Purchase Running Bill").Rows.Count > 0)
        {
            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Purchase Running Bill").Rows[0]["ParameterValue"].ToString()))
            {
                lblBillType.Visible = true;
                ddlBillType.Visible = true;
                ddlBillType.SelectedIndex = 0;
            }
        }
    }
    //this function is created by jitendra upadhyay on 05-02-2014 
    // for set the tax and discount visibility according the user parameter
    public void TaxandDiscountandAccountParameter()
    {
        lblNetTaxPar.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //lblNettaxpercolon.Visible = false;
        txtNetTaxPar.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        lblNetTaxVal.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtNetTaxVal.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[16].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[17].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[9].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[10].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtTotalTaxPrice")).Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
        }
        gridView.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Trans_Div.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //lblNetDisPercolon.Visible = false;
        txtNetDisPer.Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        lblNetDisVal.Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtNetDisVal.Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[14].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[15].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtTotalDiscountPrice")).Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
        }
    }
    protected void IbtnDeleteExp_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["Expdt"]), "Expense_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        ViewState["Expdt"] = dt;
        fillExpGrid(dt);
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Dt_Final_Save_Tax != null)
            {
                Dt_Final_Save_Tax = new DataView(Dt_Final_Save_Tax, "Expenses_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Session["Expenses_Tax_Purchase_Invoice"] = Dt_Final_Save_Tax;
            }
        }
        // GetData();
    }
    protected void btnPaymentSave_Click(object sender, EventArgs e)
    {
        if (txtPurchaseAccount.Text.Trim() == "")
        {
            DisplayMessage("Enter Purchase Account");
            txtPurchaseAccount.Focus();
            return;
        }
        if (ddlPaymentMode.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Payment Mode");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlPaymentMode);
            return;
        }
        if (txtFCPayCharges.Text == "")
        {
            DisplayMessage("Enter Amount");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFCPayCharges);
            return;
        }
        if (txtPayAccountNo.Text == "")
        {
            DisplayMessage("Select Accounts");
            txtPayAccountNo.Focus();
            return;
        }
        DataTable dt = new DataTable();
        if (ViewState["PayementDt"] != null)
        {
            dt = (DataTable)ViewState["PayementDt"];
            //if (new DataView(dt, "PaymentModeId='" + ddlPaymentMode.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
            //{
            //    DisplayMessage("Payment Mode already exist");
            //    fillPaymentGrid((DataTable)ViewState["PayementDt"]);
            //    return;
            //}
        }
        else
        {
            dt.Columns.Add("TransId");
            dt.Columns.Add("PaymentModeId");
            dt.Columns.Add("PaymentName");
            dt.Columns.Add("AccountNo");
            dt.Columns.Add("CardNo");
            dt.Columns.Add("CardName");
            dt.Columns.Add("BankId");
            dt.Columns.Add("BankAccountNo");
            dt.Columns.Add("BankAccountName");
            dt.Columns.Add("ChequeNo");
            dt.Columns.Add("ChequeDate");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("Pay_Charges");
            dt.Columns.Add("PayCurrencyID");
            dt.Columns.Add("PayExchangeRate");
            dt.Columns.Add("FCPayAmount");
            dt.Columns.Add("Field1");
            dt.Columns.Add("Field2");
        }
        dt.Rows.Add();
        if (dt.Rows.Count == 1)
        {
            dt.Rows[dt.Rows.Count - 1]["TransId"] = 1;
        }
        else
        {
            dt.Rows[dt.Rows.Count - 1]["TransId"] = (Convert.ToInt32(dt.Rows[dt.Rows.Count - 2]["TransId"].ToString()) + 1).ToString();
        }
        dt.Rows[dt.Rows.Count - 1]["PaymentModeId"] = ddlPaymentMode.SelectedValue.ToString();
        dt.Rows[dt.Rows.Count - 1]["PaymentName"] = ddlPaymentMode.SelectedItem.ToString();
        dt.Rows[dt.Rows.Count - 1]["AccountNo"] = GetAccountId(txtPayAccountNo.Text);
        dt.Rows[dt.Rows.Count - 1]["CardNo"] = txtPayCardNo.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["CardName"] = txtPayCardName.Text.Trim();
        if (ddlPayBank.SelectedValue != "--Select--" && ddlPayBank.SelectedValue != "")
        {
            dt.Rows[dt.Rows.Count - 1]["BankId"] = ddlPayBank.SelectedValue;
        }
        else
        {
            dt.Rows[dt.Rows.Count - 1]["BankId"] = "0";
        }
        dt.Rows[dt.Rows.Count - 1]["BankAccountNo"] = "";
        dt.Rows[dt.Rows.Count - 1]["BankAccountName"] = "";
        dt.Rows[dt.Rows.Count - 1]["ChequeNo"] = txtPayChequeNo.Text.Trim();
        if (txtPayChequeDate.Text.Trim() != "")
        {
            dt.Rows[dt.Rows.Count - 1]["ChequeDate"] = txtPayChequeDate.Text.Trim();
        }
        else
        {
            dt.Rows[dt.Rows.Count - 1]["ChequeDate"] = "1/1/1800";
        }
        dt.Rows[dt.Rows.Count - 1]["AccountName"] = txtPayAccountNo.Text.Split('/')[0].ToString();
        dt.Rows[dt.Rows.Count - 1]["Pay_Charges"] = txtLCPayCharges.Text;
        dt.Rows[dt.Rows.Count - 1]["PayCurrencyID"] = ddlPayCurrency.SelectedValue;
        dt.Rows[dt.Rows.Count - 1]["PayExchangeRate"] = txtPayExchangeRate.Text;
        dt.Rows[dt.Rows.Count - 1]["FCPayAmount"] = txtFCPayCharges.Text;
        dt.Rows[dt.Rows.Count - 1]["Field1"] = txtPurchaseAccount.Text.Split('/')[1].ToString();
        dt.Rows[dt.Rows.Count - 1]["Field2"] = Txt_Narration.Text.Trim();
        ViewState["PayementDt"] = dt;
        fillPaymentGrid(dt);
        DataTable dtExp = new DataTable();
        dtExp = (DataTable)ViewState["Expdt"];
        if (dtExp != null)
        {
            fillExpGrid(dtExp);
        }
        btnPaymentReset_Click(null, null);
        fillPaymentMode();
        //here we change balance amount when we paid against the invoice amount
    }
    public void fillExpGrid(DataTable dt)
    {

        DataTable Dt_Expenses = new DataTable();
        Dt_Expenses = dt;
        DataColumnCollection columns = Dt_Expenses.Columns;
        if (!columns.Contains("F_Tax_Value"))
        {
            Dt_Expenses.Columns.Add("F_Tax_Value");
        }
        if (!columns.Contains("F_Tax_Percent"))
        {
            Dt_Expenses.Columns.Add("F_Tax_Percent");
        }
        if (!columns.Contains("Line_Total"))
        {
            Dt_Expenses.Columns.Add("Line_Total");
        }
        foreach (DataRow Dt_Row in Dt_Expenses.Rows)
        {
            double Sum_Tax = 0;
            double Sum_Tax_Value = 0;
            double Sum_Line_Total = 0;
            if (Session["Expenses_Tax_Purchase_Invoice"] != null)
            {
                DataTable Dt_Cal = Session["Expenses_Tax_Purchase_Invoice"] as DataTable;
                if (Dt_Cal.Rows.Count > 0)
                {
                    Dt_Cal = new DataView(Dt_Cal, "Expenses_Id='" + Dt_Row["Expense_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (Dt_Cal.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Sub_Row in Dt_Cal.Rows)
                        {
                            Sum_Tax = Sum_Tax + Convert.ToDouble(Dt_Sub_Row["Tax_Percentage"].ToString());
                            Sum_Tax_Value = Sum_Tax_Value + Convert.ToDouble(Dt_Sub_Row["Tax_Value"].ToString());
                            Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["Exp_Charges"].ToString());
                        }
                    }
                    else
                    {
                        Sum_Tax = Sum_Tax + 0;
                        Sum_Tax_Value = Sum_Tax_Value + 0;
                        Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["Exp_Charges"].ToString());
                    }
                }
                else
                {
                    Sum_Tax = Sum_Tax + 0;
                    Sum_Tax_Value = Sum_Tax_Value + 0;
                    Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["Exp_Charges"].ToString());
                }
            }
            else
            {
                Sum_Tax = Sum_Tax + 0;
                Sum_Tax_Value = Sum_Tax_Value + 0;
                Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["Exp_Charges"].ToString());
            }
            Dt_Row["F_Tax_Percent"] = Sum_Tax.ToString();
            Dt_Row["F_Tax_Value"] = Sum_Tax_Value.ToString();
            Dt_Row["Line_Total"] = Sum_Line_Total.ToString();
        }
        objPageCmn.FillData((object)GridExpenses, dt, "", "");
        ViewState["Expdt"] = dt;
        if (dt.Rows.Count != 0)
        {
            double f = 0;
            double Toal_Tax_Value = 0;
            double Toal_Line_Toal = 0;
            foreach (GridViewRow Row in GridExpenses.Rows)
            {
                f += Convert.ToDouble(((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                Toal_Tax_Value += Convert.ToDouble(((Label)Row.FindControl("Lbl_Expenses_Tax_Value_GV")).Text.Trim());
                Toal_Line_Toal += Convert.ToDouble(((Label)Row.FindControl("Lbl_Line_Total_GV")).Text.Trim());
                ((Label)Row.FindControl("lblgvExp_Charges")).Text = SetDecimal(((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                ((Label)Row.FindControl("lblgvExp_Charges")).Text = GetCurrencySymbol(((Label)Row.FindControl("lblgvExp_Charges")).Text, ViewState["CurrencyId"].ToString());
                ((Label)Row.FindControl("lblgvFCExchangeAmount")).Text = GetCurrencySymbol(((Label)Row.FindControl("lblgvFCExchangeAmount")).Text, ((Label)Row.FindControl("hidExpCur")).Text);
            }
            ((Label)GridExpenses.FooterRow.FindControl("txttotExp")).Text = SetDecimal(f.ToString());
            ((Label)GridExpenses.FooterRow.FindControl("txttotExpShow")).Text = GetCurrencySymbol(SetDecimal(f.ToString()), ViewState["CurrencyId"].ToString());
            ((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text = SetDecimal(Toal_Tax_Value.ToString());
            ((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text = SetDecimal(Toal_Line_Toal.ToString());
        }
        else
        {
            try
            {
                ((Label)GridExpenses.FooterRow.FindControl("txttotExp")).Text = "0";
                ((Label)GridExpenses.FooterRow.FindControl("txttotExpShow")).Text = "0";
                ((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text = "0";
                ((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text = "0";
            }
            catch
            {
            }
        }
        CostingRate();

    }
    public void setPurchaseAccount()
    {
        using (DataTable Dt = objAcParameter.GetParameterValue_By_ParameterName(StrCompId, "Purchase Invoice"))
        {
            if (Dt.Rows.Count > 0)
            {
                txtPurchaseAccount.Text = Ac_ParameterMaster.GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Purchase Invoice").Rows[0]["Param_Value"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                Session["AccountId"] = Dt.Rows[0]["Param_Value"].ToString();
            }
        }
    }

    #region Advancepayment
    public void FillAdvancePayment_BYOrderId(string OrderId, string OrderNo)
    {
        int counter = 0;

        if (objDa.return_DataTable("select Inv_PurchaseInvoiceHeader.TransID from inv_purchaseinvoicedetail left join Inv_PurchaseInvoiceHeader on inv_purchaseinvoicedetail.InvoiceNo = Inv_PurchaseInvoiceHeader.TransID where inv_purchaseinvoicedetail.POId=" + OrderId + " and Inv_PurchaseInvoiceHeader.Post='True'").Rows.Count > 0)
        {
            counter = 1;
        }
        if (counter == 0)
        {
            string sql = "select top 1 coa.AccountName,vd.Credit_Amount,vh.Trans_Id from ac_voucher_header vh inner join Ac_Voucher_Detail vd on vh.Trans_Id = vd.Voucher_No left join Ac_ChartOfAccount coa on coa.Trans_Id = vd.Account_No where vh.Company_Id = '" + Session["CompId"].ToString() + "' and vh.Ref_Type = 'PO' and vh.Ref_Id = '" + OrderId + "' and Credit_Amount> 0";
            DataTable dtAdvancepayment = new DataTable();
            dtAdvancepayment.Columns.Add("OrderNo");
            dtAdvancepayment.Columns.Add("PaymentName");
            dtAdvancepayment.Columns.Add("AccountName");
            dtAdvancepayment.Columns.Add("Pay_Charges");
            if (Session["DtAdvancePayment"] == null)
            {
                //DataTable dtAdvanceRecord = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "PO", OrderId);
                DataTable dtAdvanceRecord = objDa.return_DataTable(sql);
                if (dtAdvanceRecord.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAdvanceRecord.Rows.Count; i++)
                    {
                        DataRow dr = dtAdvancepayment.NewRow();
                        dr[0] = OrderNo;
                        //dr[1] = dtAdvanceRecord.Rows[i]["PaymentName"].ToString();
                        //dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                        //dr[3] = dtAdvanceRecord.Rows[i]["Pay_Charges"].ToString();
                        dr[1] = "Cash";
                        dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                        dr[3] = dtAdvanceRecord.Rows[i]["Credit_Amount"].ToString();
                        dtAdvancepayment.Rows.Add(dr);
                    }
                }
            }
            else
            {
                dtAdvancepayment = (DataTable)Session["DtAdvancePayment"];
                //DataTable dtAdvanceRecord = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "PO", OrderId);
                DataTable dtAdvanceRecord = objDa.return_DataTable(sql);
                if (dtAdvanceRecord.Rows.Count > 0)
                {
                    //DataTable dtTemp = new DataTable();
                    //try
                    //{
                    //    dtTemp = new DataView(dtAdvancepayment, "OrderNo='" + OrderNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //}
                    //catch
                    //{
                    //}
                    //if (dtTemp.Rows.Count == 0)
                    //{
                    for (int i = 0; i < dtAdvanceRecord.Rows.Count; i++)
                    {
                        DataRow dr = dtAdvancepayment.NewRow();
                        dr[0] = OrderNo;
                        //dr[1] = dtAdvanceRecord.Rows[i]["PaymentName"].ToString();
                        //dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                        //dr[3] = dtAdvanceRecord.Rows[i]["Pay_Charges"].ToString();
                        dr[1] = "Cash";
                        dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                        dr[3] = dtAdvanceRecord.Rows[i]["Credit_Amount"].ToString();
                        dtAdvancepayment.Rows.Add(dr);
                    }
                    // }
                }
            }
            Session["DtAdvancePayment"] = dtAdvancepayment;
            Filladvancepaymentgrid(dtAdvancepayment);
        }
    }
    public void Filladvancepaymentgrid(DataTable dt)
    {
        gvadvancepayment.DataSource = dt;
        gvadvancepayment.DataBind();
        double f = 0;
        foreach (GridViewRow gvrow in gvadvancepayment.Rows)
        {
            try
            {
                ((Label)gvrow.FindControl("lblAmount")).Text = SetDecimal(((Label)gvrow.FindControl("lblAmount")).Text);
                f += Convert.ToDouble(((Label)gvrow.FindControl("lblAmount")).Text);
            }
            catch
            {
                ((Label)gvrow.FindControl("lblAmount")).Text = "0";
                f += 0;
            }
        }

        try
        {
            ((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text = SetDecimal(f.ToString());
        }
        catch
        {

        }
        dt = null;
    }
    #endregion
    #region System Defined Function:-Events
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (ddlBillType.SelectedValue.Trim() == "Running")
        {
            DisplayMessage("You can not post running bill");
            return;
        }
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }
        //if (ddlTransType.SelectedIndex == 0)
        //{
        //    DisplayMessage("Please Select Transaction Type");
        //    return;
        //}
        Session["VPost"] = "True";
        btnSave_Click(sender, e);
    }
    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    if (rbtNew.Checked == true)
    //    {
    //        txtInvoiceNo.Text = GetDocumentNumber();
    //        ViewState["DocNo"] = txtInvoiceNo.Text;
    //        HdnEdit.Value = "";
    //    }
    //    SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
    //    if (sender is Button)
    //    {
    //        Button btnId = (Button)sender;
    //        if (btnId.ID == "btnPost")
    //        {
    //            Session["VPost"] = "True";
    //        }
    //        if (btnId.ID == "btnSave")
    //        {
    //            Session["VPost"] = "False";
    //        }
    //    }
    //    //For Finance Code
    //    //here we check that this page is updated by other user before save of current user 
    //    //this code is created by jitendra upadhyay on 15-05-2015
    //    //code start
    //    if (HdnEdit.Value != "")
    //    {
    //        DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HdnEdit.Value);
    //        if (dt.Rows.Count != 0)
    //        {
    //            if (ViewState["TimeStamp"].ToString() != dt.Rows[0]["Row_Lock_Id"].ToString())
    //            {
    //                DisplayMessage("Another User update Information reload and try again");
    //                return;
    //            }
    //        }
    //    }
    //    //code end
    //    string strReceivedQty = string.Empty;
    //    if (txtInvoicedate.Text == "")
    //    {
    //        DisplayMessage("Enter Invoice Date");
    //        txtInvoicedate.Focus();
    //        return;
    //    }
    //    else
    //    {
    //        try
    //        {
    //            ObjSysParam.getDateForInput(txtInvoicedate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Invoice Date in format " + Session["DateFormat"].ToString() + "");
    //            txtInvoicedate.Text = "";
    //            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoicedate);
    //            return;
    //        }
    //    }
    //    //code added by jitendra upadhyay on 09-12-2016
    //    //for insert record according the log in financial year
    //    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtInvoicedate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
    //    {
    //        DisplayMessage("Log In Financial year not allowing to perform this action");
    //        return;
    //    }
    //    if (txtSupInvoiceDate.Text == "")
    //    {
    //        DisplayMessage("Select Supplier Invoice Date");
    //        txtSupInvoiceDate.Focus();
    //        return;
    //    }
    //    else
    //    {
    //        try
    //        {
    //            ObjSysParam.getDateForInput(txtSupInvoiceDate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Supplier Invoice Date in format " + Session["DateFormat"].ToString() + "");
    //            txtSupInvoiceDate.Text = "";
    //            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupInvoiceDate);
    //            return;
    //        }
    //    }
    //    if (txtInvoiceNo.Text == "")
    //    {
    //        txtInvoiceNo.Text = "";
    //        txtInvoiceNo.Focus();
    //        DisplayMessage("Enter Invoice No");
    //        return;
    //    }
    //    //if (txtSupplierInvoiceNo.Text == "")
    //    //{
    //    //    txtSupplierInvoiceNo.Text = "";
    //    //    txtSupplierInvoiceNo.Focus();
    //    //    DisplayMessage("Enter Supplier Invoice No");
    //    //    return;
    //    //}
    //    if (txtSupplierName.Text == "")
    //    {
    //        txtSupplierName.Focus();
    //        DisplayMessage("Enter Supplier Name");
    //        return;
    //    }
    //    if (txtCostingRate.Text == "")
    //    {
    //        txtCostingRate.Text = "0";
    //    }
    //    if (txtOtherCharges.Text == "")
    //    {
    //        txtOtherCharges.Text = "0";
    //    }
    //    if (!RdoPo.Checked && !RdoWithOutPo.Checked)
    //    {
    //        DisplayMessage("Select One With Purchase Order Or WithOut Purchase Order");
    //        RdoPo.Focus();
    //        return;
    //    }
    //    if (gvProduct.Rows.Count == 0)
    //    {
    //        DisplayMessage("Select Product");
    //        return;
    //    }
    //    if (ddlCurrency.SelectedIndex == 0)
    //    {
    //        ddlCurrency.Focus();
    //        DisplayMessage("Currency Required On Company Level");
    //        return;
    //    }
    //    string strTotalQty = string.Empty;
    //    string strAmount = string.Empty;
    //    string PoId = string.Empty;
    //    bool Post = false;
    //    if (Session["VPost"].ToString() == "True")
    //    {
    //        DataTable dt = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value);
    //        if (dt.Rows.Count > 0)
    //        {
    //            if (new DataView(dt, "RecQty<>'0'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //            {
    //                Post = true;
    //            }
    //            else
    //            {
    //                Session["VPost"] = "False";
    //                Post = false;
    //                DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            Session["VPost"] = "False";
    //            Post = false;
    //            DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
    //            return;
    //        }
    //    }
    //    else if (Session["VPost"].ToString() == "False")
    //    {
    //        Post = false;
    //    }
    //    string POst = string.Empty;
    //    if (RdoPo.Checked)
    //    {
    //        POst = "PO";
    //    }
    //    if (RdoWithOutPo.Checked)
    //    {
    //        POst = "WOutPO";
    //    }
    //    if (txtExchangeRate.Text.Trim() == "")
    //    {
    //        txtExchangeRate.Text = "0";
    //    }
    //    if (txtCostingRate.Text == "")
    //    {
    //        txtCostingRate.Text = "0";
    //    }
    //    if (txtBillAmount.Text == "0")
    //    {
    //        txtBillAmount.Text = "0";
    //    }
    //    //this validation for check that payment amount is equal to invoice amount or not
    //    if (Session["VPost"].ToString() == "True")
    //    {
    //        if (txtPurchaseAccount.Text == "")
    //        {
    //            DisplayMessage("Enter Purchase Account");
    //            txtPurchaseAccount.Focus();
    //            return;
    //        }
    //        if (gvPayment.Rows.Count == 0)
    //        {
    //            Session["VPost"] = "False";
    //            Post = false;
    //            DisplayMessage("Enter Payment Mode Details");
    //            return;
    //        }
    //    }
    //    string strAccountId = "0";
    //    //For Bank Account
    //    if (txtPurchaseAccount.Text != "" && txtPurchaseAccount.Text != null)
    //    {
    //        strAccountId = txtPurchaseAccount.Text.Split('/')[1].ToString();
    //    }
    //    else
    //    {
    //        DisplayMessage("Purchase Account Not Found");
    //        return;
    //    }

    //    //string strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
    //    //added payment validation according the discussion with nafees sir on 27-06-2016
    //    //created by jitendra upadhyay 
    //    double Invoiceamt = 0;
    //    double Paymentamt = 0;
    //    double advancepayment = 0;
    //    if (txtGrandTotal.Text != "")
    //    {
    //        Invoiceamt = Convert.ToDouble(txtGrandTotal.Text);
    //    }
    //    if (gvPayment.Rows.Count > 0)
    //    {
    //        Paymentamt = Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text);
    //    }
    //    if (Invoiceamt != Paymentamt)
    //    {
    //        DisplayMessage("Payment Amount should be equal to invoice amount");
    //        return;
    //    }
    //    //for add confirmation when invoice quantity and recenve quantityis not same 
    //    if (txtCostingRate.Text.Trim() == "" || txtCostingRate.Text.Trim() == "Infinity")
    //    {
    //        txtCostingRate.Text = "0";
    //    }

    //    //Check controls Value from page setting
    //    string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
    //    if (result[0] == "false")
    //    {
    //        DisplayMessage(result[1]);
    //        return;
    //    }

    //    string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
    //    strTotalQty = "0";
    //    strAmount = "0";
    //    int b = 0;
    //    string InvoiceId = "0";

    //    con.Open();
    //    SqlTransaction trns;
    //    trns = con.BeginTransaction();
    //    try
    //    {
    //        if (HdnEdit.Value == "")
    //        {
    //            //start rollback transaction
    //            // GetData();
    //            b = ObjPurchaseInvoice.InsertPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
    //            InvoiceId = b.ToString();

    //            if (txtInvoiceNo.Text == ViewState["DocNo"].ToString())
    //            {
    //                if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is OPC Location
    //                {

    //                    string sql = "SELECT count(*) FROM Inv_PurchaseInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and InvoiceNo Like '%" + ViewState["DocNo"].ToString() + "%'";
    //                    int strInvoiceCount = Int32.Parse(objDa.get_SingleValue(sql, ref trns));
    //                    int skipNo = 0;
    //                    strInvoiceCount += skipNo;

    //                    bool bFlag = false;
    //                    while (bFlag == false)
    //                    {
    //                        txtInvoiceNo.Text = ViewState["DocNo"].ToString() + (strInvoiceCount == 0 ? "1" : strInvoiceCount.ToString());
    //                        string sql1 = "SELECT count(*) FROM Inv_PurchaseInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and invoiceno='" + txtInvoiceNo.Text + "'";
    //                        string strInvCount = objDa.get_SingleValue(sql1, ref trns);
    //                        if (strInvCount == "0")
    //                        {
    //                            bFlag = true;
    //                        }
    //                        else
    //                        {
    //                            strInvoiceCount++;
    //                        }
    //                    }
    //                    ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text, ref trns);
    //                    txtInvoiceNo.Text = txtInvoiceNo.Text + strInvoiceCount.ToString();
    //                }
    //                else
    //                {

    //                    int invoicecount = ObjPurchaseInvoice.GetInvoiceCountByLocationId(Session["LocId"].ToString(), ref trns);
    //                    if (invoicecount == 0)
    //                    {
    //                        ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + "1", ref trns);
    //                        txtInvoiceNo.Text = txtInvoiceNo.Text + "1";
    //                    }
    //                    else
    //                    {
    //                        if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count > 0)
    //                        {

    //                            bool bCodeFlag = true;
    //                            while (bCodeFlag)
    //                            {
    //                                invoicecount += 1;

    //                                if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count == 0)
    //                                {
    //                                    bCodeFlag = false;
    //                                }

    //                            }

    //                        }
    //                        ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + invoicecount.ToString(), ref trns);
    //                        txtInvoiceNo.Text = txtInvoiceNo.Text + invoicecount.ToString();
    //                    }
    //                }



    //            }

    //            DataTable dtExpense = new DataTable();
    //            dtExpense = (DataTable)ViewState["Expdt"];
    //            if (dtExpense != null)
    //            {
    //                try
    //                {
    //                    string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
    //                    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, ddlCurrency.SelectedValue, txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
    //                }
    //                catch
    //                {

    //                }
    //                try
    //                {
    //                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
    //                    {
    //                        //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                        ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            DataTable dtPayment = new DataTable();
    //            dtPayment = (DataTable)ViewState["PayementDt"];
    //            if (dtPayment != null)
    //            {
    //                try
    //                {
    //                    foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
    //                    {
    //                        ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", InvoiceId.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", InvoiceId.ToString(), ref trns);
    //            int SerialNo = 1;
    //            foreach (GridViewRow gvr in gvProduct.Rows)
    //            {
    //                Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
    //                Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
    //                DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
    //                TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
    //                Label lblPOId = (Label)gvr.FindControl("lblPOId");
    //                Label OrderQty = (Label)gvr.FindControl("OrderQty");
    //                TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
    //                TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
    //                Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
    //                TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
    //                TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
    //                Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
    //                TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
    //                TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
    //                Label lblAmount = (Label)gvr.FindControl("lblAmount");
    //                Label lblRecQty = (Label)gvr.FindControl("lblRecQty");
    //                if (lblUnitRate.Text == "")
    //                {
    //                    lblUnitRate.Text = "0";
    //                }
    //                if (lblTax.Text == "")
    //                {
    //                    lblTax.Text = "0";
    //                }
    //                if (lblTaxValue.Text == "")
    //                {
    //                    lblTaxValue.Text = Convert_Into_DF("0");
    //                }
    //                if (lblDiscount.Text == "")
    //                {
    //                    lblDiscount.Text = "0";
    //                }
    //                if (lblDiscountValue.Text == "")
    //                {
    //                    lblDiscountValue.Text = "0";
    //                }
    //                //if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), lblgvProductId.Text, Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "NS")
    //                //{
    //                //    lblRecQty.Text = QtyGoodReceived.Text.ToString();
    //                //}
    //                if (QtyGoodReceived.Text != "" || QtyGoodReceived.Text != "0")
    //                {
    //                    int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), b.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                    Insert_Tax("gvProduct", b.ToString(), Details_ID.ToString(), gvr, ref trns);
    //                    SerialNo++;
    //                    //this code is created by jitendra upadhya on 14-07-2015
    //                    //this code for insert record in tax ref detail table when we apply multiple tax according product category
    //                    //code start
    //                    foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
    //                    {
    //                        if (Convert.ToDouble(lblAmount.Text) != 0)
    //                            objTaxRefDetail.InsertRecord("PINV", InvoiceId, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //                //code end
    //            }
    //            Tax_Insert_Into_Inv_TaxRefDetail(InvoiceId, ref trns);
    //            //committ
    //        }
    //        else
    //        {
    //            string strPOId = string.Empty;
    //            string ShippingLine = string.Empty;
    //            string shipUnit = string.Empty;
    //            if (ddlShipUnit.SelectedIndex == 0)
    //            {
    //                shipUnit = "0";
    //            }
    //            else
    //            {
    //                shipUnit = ddlShipUnit.SelectedValue;
    //            }
    //            if (txtTotalWeight.Text == "")
    //            {
    //                txtTotalWeight.Text = "0";
    //            }
    //            if (txtUnitRate.Text == "")
    //            {
    //                txtUnitRate.Text = "0";
    //            }
    //            if (txtShippingLine.Text != "")
    //            {
    //                ShippingLine = txtShippingLine.Text.Split('/')[1].ToString();
    //            }
    //            if (txtShippingDate.Text.Trim() != "")
    //            {
    //                try
    //                {
    //                    Convert.ToDateTime(txtShippingDate.Text);
    //                }
    //                catch
    //                {
    //                    DisplayMessage("Enter Valid Shipping Date");
    //                    txtShippingDate.Focus();
    //                    trns.Rollback();
    //                    if (con.State == System.Data.ConnectionState.Open)
    //                    {
    //                        con.Close();
    //                    }
    //                    trns.Dispose();
    //                    con.Dispose();
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Enter Shipping Date");
    //                txtShippingDate.Focus();
    //                trns.Rollback();
    //                if (con.State == System.Data.ConnectionState.Open)
    //                {
    //                    con.Close();
    //                }
    //                trns.Dispose();
    //                con.Dispose();
    //                return;
    //            }
    //            if (txtReceivingDate.Text.Trim() != "")
    //            {
    //                try
    //                {
    //                    Convert.ToDateTime(txtReceivingDate.Text);
    //                }
    //                catch
    //                {
    //                    DisplayMessage("Enter Valid Receiving Date");
    //                    txtReceivingDate.Focus();
    //                    trns.Rollback();
    //                    if (con.State == System.Data.ConnectionState.Open)
    //                    {
    //                        con.Close();
    //                    }
    //                    trns.Dispose();
    //                    con.Dispose();
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Enter Receiving Date");
    //                txtReceivingDate.Focus();
    //                trns.Rollback();
    //                if (con.State == System.Data.ConnectionState.Open)
    //                {
    //                    con.Close();
    //                }
    //                trns.Dispose();
    //                con.Dispose();
    //                return;
    //            }
    //            // GetData();
    //            b = ObjPurchaseInvoice.UpdatePurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
    //            //update shipping information 
    //            //delete and reinsert 
    //            string sql = "delete from dbo.Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + HdnEdit.Value.Trim() + "";
    //            objDa.execute_Command(sql, ref trns);
    //            sql = "INSERT INTO [Inv_InvoiceShippingHeader]([Ref_Type],[Ref_Id] ,[Shipping_Line],[Ship_By] ,[Airway_Bill_No],[Ship_Unit],[Actual_Weight],[Volumetric_weight],[Shipping_Date],[Receiving_date],[Shipment_Type],[Freight_Status],[UnitRate],[Divide_By])VALUES('PINV'," + HdnEdit.Value.Trim() + ",'" + ShippingLine.Trim() + "','" + ddlShipBy.SelectedValue.Trim() + "','" + txtAirwaybillno.Text.Trim() + "','" + shipUnit.Trim() + "','" + txtTotalWeight.Text.Trim() + "','" + txtvolumetricweight.Text.Trim() + "','" + ObjSysParam.getDateForInput(txtShippingDate.Text).ToString().Trim() + "','" + ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString().Trim() + "','" + ddlShipmentType.SelectedValue.Trim() + "','" + ddlFreightStatus.SelectedValue.Trim() + "','" + txtUnitRate.Text + "','" + txtdivideby.Text.Trim() + "')";
    //            objDa.execute_Command(sql, ref trns);
    //            DataTable dtExpense = new DataTable();
    //            dtExpense = (DataTable)ViewState["Expdt"];
    //            if (dtExpense != null)
    //            {
    //                try
    //                {
    //                    ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, "PI", ref trns);
    //                    string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
    //                    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim().ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                }
    //                catch
    //                {
    //                }
    //                try
    //                {
    //                    ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.ToString(), "PI", ref trns);
    //                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
    //                    {
    //                        //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                        ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                }
    //            }
    //            DataTable dtPayment = new DataTable();
    //            dtPayment = (DataTable)ViewState["PayementDt"];
    //            if (dtPayment != null)
    //            {
    //                try
    //                {
    //                    ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
    //                    foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
    //                    {
    //                        ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", HdnEdit.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            //here we update receive qty in one table 
    //            //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
    //            //by jitendra upadhyay 
    //            //on 22-02-2016
    //            //code start
    //            DataTable dtInvDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
    //            //code end
    //            ObjPurchaseInvoiceDetail.DeletePurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
    //            int SerialNo = 1;
    //            CostingRate();
    //            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", HdnEdit.Value.Trim(), ref trns);
    //            foreach (GridViewRow gvr in gvProduct.Rows)
    //            {
    //                Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
    //                Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
    //                DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
    //                TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
    //                Label lblPOId = (Label)gvr.FindControl("lblPOId");
    //                strPOId = lblPOId.Text;
    //                Label OrderQty = (Label)gvr.FindControl("OrderQty");
    //                TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
    //                Label QtyReceived = (Label)gvr.FindControl("QtyReceived");
    //                TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
    //                Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
    //                TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
    //                TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
    //                Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
    //                TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
    //                TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
    //                Label lblAmount = (Label)gvr.FindControl("lblAmount");
    //                Label lblRecQty = (Label)gvr.FindControl("lblRecQty");
    //                string UnitCost = (Convert.ToDouble(((Convert.ToDouble(lblUnitRate.Text) + Convert.ToDouble(Convert_Into_DF(lblTaxValue.Text))) - Convert.ToDouble(lblDiscountValue.Text)).ToString()) * Convert.ToDouble(txtCostingRate.Text)).ToString();
    //                if (lblUnitRate.Text == "")
    //                {
    //                    lblUnitRate.Text = "0";
    //                }
    //                if (QtyGoodReceived.Text == "")
    //                {
    //                    QtyGoodReceived.Text = "0";
    //                }
    //                if (lblTax.Text == "")
    //                {
    //                    lblTax.Text = "0";
    //                }
    //                if (lblTaxValue.Text == "")
    //                {
    //                    lblTaxValue.Text = Convert_Into_DF("0");
    //                }
    //                if (lblDiscount.Text == "")
    //                {
    //                    lblDiscount.Text = "0";
    //                }
    //                if (lblDiscountValue.Text == "")
    //                {
    //                    lblDiscountValue.Text = "0";
    //                }
    //                //if (lblRecQty.Text == "")
    //                //{
    //                //    lblRecQty.Text = "0";
    //                //}
    //                //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
    //                //by jitendra upadhyay 
    //                //on 20-02-2016
    //                //code start
    //                //string strsql = "select RecQty from Inv_PurchaseInvoiceDetail where InvoiceNo=" + HdnEdit.Value.ToString() + " and ProductId=" + lblgvProductId.Text + "";
    //                //DataTable dtRecqty= objDa.return_DataTable(strsql,ref trns);
    //                //if (dtRecqty.Rows.Count > 0)
    //                //{
    //                DataTable dtTemp = new DataView(dtInvDetail, "ProductId=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
    //                if (dtTemp.Rows.Count > 0)
    //                {
    //                    if (dtTemp.Rows.Count == 1)
    //                    {
    //                        lblRecQty.Text = dtTemp.Rows[0]["RecQty"].ToString();
    //                    }
    //                }
    //                int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                Insert_Tax("gvProduct", HdnEdit.Value.ToString(), Details_ID.ToString(), gvr, ref trns);
    //                //cpde start
    //                if (Session["VPost"].ToString() == "True")
    //                {
    //                    ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", "0", lblRecQty.Text.Trim(), "0", "1/1/1800", UnitCost.ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
    //                }
    //                //}
    //                //code end
    //                SerialNo++;
    //                //this code is created by jitendra upadhya on 14-07-2015
    //                //this code for insert record in tax ref detail table when we apply multiple tax according product category
    //                //code start
    //                foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
    //                {
    //                    if (Convert.ToDouble(lblAmount.Text) != 0)
    //                        objTaxRefDetail.InsertRecord("PINV", HdnEdit.Value, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                }
    //                //code end
    //            }
    //            Tax_Insert_Into_Inv_TaxRefDetail(HdnEdit.Value, ref trns);
    //            //Add Post Value Posted for Account Section On 31-03-2014 By Lokesh
    //            if (Post == true)
    //            {
    //                string strRefrenceNumber = string.Empty;
    //                if (HdnEdit.Value != "")
    //                {
    //                    foreach (GridViewRow gvrow in gvProduct.Rows)
    //                    {
    //                        string SalesPrice = string.Empty;
    //                        Label lblUnitRateLocal = (Label)gvrow.FindControl("lblUnitRateLocal");
    //                        Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
    //                        if (lblUnitRateLocal.Text == "")
    //                        {
    //                            lblUnitRateLocal.Text = "0";
    //                        }
    //                        SalesPrice = SetDecimal((Convert.ToDouble(lblUnitRateLocal.Text) * Convert.ToDouble(ViewState["ExchangeRateinComp"].ToString())).ToString());
    //                        DataTable dtcustomerPriceList = objContactPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S", ref trns);
    //                        try
    //                        {
    //                            dtcustomerPriceList = new DataView(dtcustomerPriceList, "Product_Id=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
    //                        }
    //                        catch
    //                        {
    //                        }
    //                        if (dtcustomerPriceList.Rows.Count == 0)
    //                        {
    //                            objContactPriceList.InsertContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                        }
    //                        else
    //                        {
    //                            objContactPriceList.UpdateContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                        }
    //                    }
    //                    //Commented Code Start
    //                    strRefrenceNumber = txtSupplierInvoiceNo.Text;
    //                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
    //                    objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, txtInvoicedate.Text, "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                    string strMaxId = string.Empty;
    //                    DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
    //                    if (dtMaxId.Rows.Count > 0)
    //                    {
    //                        strMaxId = dtMaxId.Rows[0][0].ToString();
    //                    }
    //                    int j = 0;
    //                    string strPayTotal = "0";
    //                    string strCashAccount = string.Empty;
    //                    string strCreditAccount = string.Empty;
    //                    //                        string strPaymentVoucherAcc = string.Empty;
    //                    string strRoundoffAccount = string.Empty;
    //                    DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId, ref trns);
    //                    DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
    //                    if (dtCash.Rows.Count > 0)
    //                    {
    //                        strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
    //                    }
    //                    DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
    //                    if (dtCredit.Rows.Count > 0)
    //                    {
    //                        strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
    //                    }
    //                    //DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
    //                    //if (dtPaymentVoucher.Rows.Count > 0)
    //                    //{
    //                    //    strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
    //                    //}
    //                    DataTable dtRoundoff = new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable();
    //                    if (dtRoundoff.Rows.Count > 0)
    //                    {
    //                        strRoundoffAccount = dtRoundoff.Rows[0]["Param_Value"].ToString();
    //                    }
    //                    bool IsNonRegistered = false;
    //                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, txtSupplierName.Text.Trim().Split('/')[1].ToString());
    //                    if (dtSup.Rows.Count > 0)
    //                    {
    //                        if (!String.IsNullOrEmpty(dtSup.Rows[0]["Field2"].ToString()))
    //                            IsNonRegistered = Convert.ToBoolean(dtSup.Rows[0]["Field2"].ToString());
    //                    }
    //                    //For Advance Debit Account
    //                    string strAdvanceDebitAC = string.Empty;
    //                    DataTable dtAdvanceDebitAC = new DataView(dtAcParameter, "Param_Name='PO Advance Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
    //                    if (dtAdvanceDebitAC.Rows.Count > 0)
    //                    {
    //                        strAdvanceDebitAC = dtAdvanceDebitAC.Rows[0]["Param_Value"].ToString();
    //                    }
    //                    double fOrderExp = 0;
    //                    foreach (GridViewRow gvr in gvOrderExpenses.Rows)
    //                    {
    //                        Label lblExpCharge = (Label)gvr.FindControl("lblgvExp_Charges");
    //                        if (lblExpCharge.Text != "" && lblExpCharge.Text != "0")
    //                        {
    //                            fOrderExp = fOrderExp + Convert.ToDouble(lblExpCharge.Text);
    //                        }
    //                    }
    //                    // Code By Ghanshyam Suthar on 11-04-2018
    //                    double Exchange_Rate = 0;
    //                    if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
    //                    {
    //                        if (txtExchangeRate.Text.Trim() == "")
    //                            Exchange_Rate = 1;
    //                        else
    //                            Exchange_Rate = Convert.ToDouble(txtExchangeRate.Text.Trim());
    //                    }
    //                    else
    //                    {
    //                        Exchange_Rate = 1;
    //                    }
    //                    //---------------------------------------------------------------------------------------------------------------------
    //                    // For Location Currency
    //                    double L_Advance_Expenses_Amount = 0;
    //                    double L_Invoice_Without_Tax_Amount = 0;
    //                    double L_Invoice_Tax_Amount = 0;
    //                    double L_Invoice_With_Tax_Amount = 0;
    //                    double L_Total_Expenses_Without_Tax_Amount = 0;
    //                    double L_Total_Expenses_Tax_Amount = 0;
    //                    double L_Total_Expenses_With_Tax_Amount = 0;
    //                    double L_Separate_Expenses_Without_Tax_Amount = 0;
    //                    double L_Separate_Expenses_Tax_Amount = 0;
    //                    double L_Separate_Expenses_With_Tax_Amount = 0;
    //                    //---------------------------------------------------------------------------------------------------------------------
    //                    // For Foregin Currency
    //                    double F_Advance_Expenses_Amount = 0;
    //                    double F_Invoice_Without_Tax_Amount = 0;
    //                    double F_Invoice_Tax_Amount = 0;
    //                    double F_Invoice_With_Tax_Amount = 0;
    //                    double F_Total_Expenses_Without_Tax_Amount = 0;
    //                    double F_Total_Expenses_Tax_Amount = 0;
    //                    double F_Total_Expenses_With_Tax_Amount = 0;
    //                    double F_Separate_Expenses_Without_Tax_Amount = 0;
    //                    double F_Separate_Expenses_Tax_Amount = 0;
    //                    double F_Separate_Expenses_With_Tax_Amount = 0;
    //                    //---------------------------------------------------------------------------------------------------------------------
    //                    // For Company Currency
    //                    double C_Advance_Expenses_Amount = 0;
    //                    double C_Invoice_Without_Tax_Amount = 0;
    //                    double C_Invoice_Tax_Amount = 0;
    //                    double C_Invoice_With_Tax_Amount = 0;
    //                    double C_Total_Expenses_Without_Tax_Amount = 0;
    //                    double C_Total_Expenses_Tax_Amount = 0;
    //                    double C_Total_Expenses_With_Tax_Amount = 0;
    //                    double C_Separate_Expenses_Without_Tax_Amount = 0;
    //                    double C_Separate_Expenses_Tax_Amount = 0;
    //                    double C_Separate_Expenses_With_Tax_Amount = 0;
    //                    double Advance_Amount = 0;
    //                    double NetTaxVal = 0;
    //                    double GrandTotal = 0;
    //                    double Total_Expenses_Tax = 0;
    //                    double Total_Expenses = 0;
    //                    if (fOrderExp.ToString() != "")
    //                        Advance_Amount = fOrderExp;
    //                    if (txtNetTaxVal.Text.Trim() != "")
    //                        NetTaxVal = Convert.ToDouble(Convert_Into_DF(txtNetTaxVal.Text.Trim()));
    //                    if (txtGrandTotal.Text.Trim() != "")
    //                        GrandTotal = Convert.ToDouble(txtGrandTotal.Text.Trim());
    //                    if (GridExpenses.Rows.Count > 0)
    //                    {
    //                        if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text != "")
    //                            Total_Expenses_Tax = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text);
    //                        if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text != "")
    //                            Total_Expenses = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text);
    //                    }
    //                    //-------------------------------------------------------------------------------------------------------------------------------
    //                    // Location Currency
    //                    L_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal(Advance_Amount.ToString()));
    //                    L_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((NetTaxVal * Exchange_Rate).ToString()));
    //                    L_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((GrandTotal * Exchange_Rate).ToString()));
    //                    L_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount - L_Invoice_Tax_Amount).ToString()));
    //                    L_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses_Tax.ToString()));
    //                    L_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses.ToString()));
    //                    L_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount - L_Total_Expenses_Tax_Amount).ToString()));
    //                    L_Separate_Expenses_Without_Tax_Amount = 0;
    //                    L_Separate_Expenses_Tax_Amount = 0;
    //                    L_Separate_Expenses_With_Tax_Amount = 0;
    //                    //-------------------------------------------------------------------------------------------------------------------------------
    //                    // Foregin Currency
    //                    F_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal((L_Advance_Expenses_Amount / Exchange_Rate).ToString()));
    //                    //F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_Tax_Amount / Exchange_Rate).ToString()));
    //                    F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal(NetTaxVal.ToString()));
    //                    //F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount / Exchange_Rate).ToString()));
    //                    F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal(GrandTotal.ToString()));
    //                    F_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((F_Invoice_With_Tax_Amount - F_Invoice_Tax_Amount).ToString()));
    //                    F_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Tax_Amount / Exchange_Rate).ToString()));
    //                    F_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount / Exchange_Rate).ToString()));
    //                    F_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Without_Tax_Amount / Exchange_Rate).ToString()));
    //                    F_Separate_Expenses_Without_Tax_Amount = 0;
    //                    F_Separate_Expenses_Tax_Amount = 0;
    //                    F_Separate_Expenses_With_Tax_Amount = 0;
    //                    //-------------------------------------------------------------------------------------------------------------------------------
    //                    // Company Currency
    //                    C_Advance_Expenses_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Advance_Expenses_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Invoice_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Invoice_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Invoice_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Total_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Total_Expenses_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Total_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                    C_Separate_Expenses_Without_Tax_Amount = 0;
    //                    C_Separate_Expenses_Tax_Amount = 0;
    //                    C_Separate_Expenses_With_Tax_Amount = 0;
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    //First Line Entry 
    //                    // Invoice_Without_Tax_Amount + Without_Expenses_Tax - Advance Amount
    //                    if (strAccountId.Split(',').Contains(Session["AccountId"].ToString()))
    //                    {
    //                        string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                        string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                        string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    else
    //                    {
    //                        string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                        string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                        string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    // Debit Entry for Expenses when Supplier is Non-Registered
    //                    //Start Code
    //                    if (IsNonRegistered)
    //                    {
    //                        string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
    //                        if (ExpensesAccountId == "0")
    //                        {
    //                            DisplayMessage("Please First set Expenses Account in Inventory parameter");
    //                            trns.Rollback();
    //                            return;
    //                        }
    //                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Invoice_Tax_Amount.ToString(), "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "'(Non registered purchase) " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Invoice_Tax_Amount.ToString(), C_Invoice_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    //End Code
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    // Entry for Taxes of Product
    //                    // Start Code
    //                    string TaxQuery = "Select * from Inv_TaxRefDetail where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' and Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
    //                    DataTable dtTaxDetails = objDa.return_DataTable(TaxQuery, ref trns);
    //                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
    //                    {
    //                        string Product_Id = string.Empty;
    //                        string Tax_Id = string.Empty;
    //                        string Tax_Name = string.Empty;
    //                        string Tax_Value = string.Empty;
    //                        string Tax_Amount = string.Empty;
    //                        string TaxAccountNo = string.Empty;
    //                        string TaxAccountDetails = "Select * from Sys_TaxMaster where IsActive = 'true'";
    //                        DataTable dtTaxAccountDetails = objDa.return_DataTable(TaxAccountDetails);
    //                        if (dtTaxAccountDetails == null || dtTaxAccountDetails.Rows.Count == 0)
    //                        {
    //                            DisplayMessage("Please Configure Account for Tax in Tax Master");
    //                            trns.Rollback();
    //                            return;
    //                        }
    //                        string TaxGrouping = "Select Tax_Id,Tax_Name,STM.Field3,SUM(CONVERT(decimal(18,2),Tax_value)) as TaxAmount from Inv_TaxRefDetail inner join Sys_TaxMaster STM on STM.Trans_Id = Tax_Id where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' AND Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 group by Tax_Id,Tax_Name,STM.Field3";
    //                        DataTable TaxTableGrouping = objDa.return_DataTable(TaxGrouping, ref trns);
    //                        string TaxIdInfo = string.Empty;
    //                        string GroupTaxId = string.Empty;
    //                        double GroupTaxAmount = 0;
    //                        string S_Tax_Percentage = string.Empty;
    //                        string GroupTaxName = string.Empty;
    //                        string strTaxPer = string.Empty;
    //                        foreach (DataRow grouprow in TaxTableGrouping.Rows)
    //                        {
    //                            GroupTaxId = grouprow["Tax_Id"].ToString();
    //                            GroupTaxAmount = Convert.ToDouble(grouprow["TaxAmount"].ToString());
    //                            GroupTaxName = grouprow["Tax_Name"].ToString();
    //                            TaxAccountNo = grouprow["Field3"].ToString();
    //                            if (String.IsNullOrEmpty(TaxAccountNo))
    //                            {
    //                                DisplayMessage("Please Configure Account for Tax in Tax Master");
    //                                trns.Rollback();
    //                                return;
    //                            }
    //                            L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((GroupTaxAmount * Exchange_Rate).ToString()));
    //                            F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Exchange_Rate).ToString()));
    //                            C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                            S_Tax_Percentage = string.Empty;
    //                            foreach (DataRow row in dtTaxDetails.Rows)
    //                            {
    //                                if (!TaxIdInfo.Contains(GroupTaxId))
    //                                {
    //                                    DataView groupView = new DataView(dtTaxDetails);
    //                                    groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
    //                                    GroupTaxName = string.Empty;
    //                                    double N_Tax_Per = 0;
    //                                    foreach (DataRow newrow in groupView.ToTable().Rows)
    //                                    {
    //                                        N_Tax_Per = N_Tax_Per + Convert.ToDouble(newrow["Tax_Per"].ToString());
    //                                        S_Tax_Percentage = Convert.ToString(N_Tax_Per / Convert.ToDouble(groupView.ToTable().Rows.Count));
    //                                    }
    //                                    if (IsNonRegistered)
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Tax_Amount.ToString(), S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "' Non registered purchase", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    else
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    if (String.IsNullOrEmpty(TaxIdInfo))
    //                                        TaxIdInfo = GroupTaxId;
    //                                    else
    //                                        TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    //End Code
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    //Start Code
    //                    // Insert Expenses Entry
    //                    double Expenses_Tax = 0;
    //                    string[,] Net_Expenses_Tax = new string[1, 5];
    //                    double Exp = 0;
    //                    double SupplierExp = 0;
    //                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
    //                    {
    //                        string strExpensesName = dr["Exp_Name"].ToString();
    //                        string strForeignAmount = dr["FCExpAmount"].ToString();
    //                        string strExpensesId = dr["Expense_Id"].ToString();
    //                        double strExpAmount = Convert.ToDouble(dr["Exp_Charges"].ToString());
    //                        string strAccountNo = dr["Account_No"].ToString();
    //                        string strExpCurrencyId = dr["ExpCurrencyID"].ToString();
    //                        string strExchangeRate = dr["ExpExchangeRate"].ToString();
    //                        Exp = Convert.ToDouble(SetDecimal((Exp + Convert.ToDouble(dr["Exp_Charges"].ToString())).ToString()));
    //                        if (strExpensesName == "")
    //                        {
    //                            strExpensesName = GetExpName(strExpensesId);
    //                        }
    //                        Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax_Post(strExpAmount.ToString()));
    //                        double Expenses_Exchange_Rate = 0;
    //                        if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
    //                        {
    //                            if (strExchangeRate.Trim() == "")
    //                                Expenses_Exchange_Rate = 1;
    //                            else
    //                                Expenses_Exchange_Rate = Convert.ToDouble(SetDecimal(strExchangeRate.Trim()));
    //                        }
    //                        else
    //                        {
    //                            Expenses_Exchange_Rate = 1;
    //                        }
    //                        L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((Expenses_Tax).ToString()));
    //                        F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Expenses_Exchange_Rate).ToString()));
    //                        C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                        L_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((strExpAmount).ToString()));
    //                        F_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Without_Tax_Amount / Expenses_Exchange_Rate).ToString()));
    //                        C_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
    //                        if (strAccountNo == strPaymentVoucherAcc)
    //                        {
    //                            //Check account exist or not in selected currency - Neelkanth Purohit -24-08-2018
    //                            string strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dr["ExpCurrencyId"].ToString()).ToString();
    //                            if (strSupplierAcc == "0")
    //                            {
    //                                throw new System.ArgumentException("Account No not exist for this expenses currency , first create it");
    //                            }
    //                            string L_Debit_Amount = string.Empty;
    //                            string F_Debit_Amount = string.Empty;
    //                            string C_Debit_Amount = string.Empty;
    //                            if (IsNonRegistered)
    //                            {
    //                                L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
    //                            }
    //                            else
    //                            {
    //                                L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
    //                            }
    //                            //For Debit Entry
    //                            SupplierExp += L_Separate_Expenses_Without_Tax_Amount;
    //                            string Exp_Narration = string.Empty;
    //                            if (txtAirwaybillno.Text != "")
    //                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
    //                            else
    //                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
    //                            if (IsNonRegistered)
    //                            {
    //                                string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
    //                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                if (L_Separate_Expenses_Tax_Amount != 0)
    //                                {
    //                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                }
    //                                if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                {
    //                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                {
    //                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string L_Debit_Amount = string.Empty;
    //                            string F_Debit_Amount = string.Empty;
    //                            string C_Debit_Amount = string.Empty;
    //                            if (IsNonRegistered)
    //                            {
    //                                L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
    //                            }
    //                            else
    //                            {
    //                                L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
    //                                C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
    //                            }
    //                            //For Credit Entry                                
    //                            string Exp_Narration = string.Empty;
    //                            if (txtAirwaybillno.Text != "")
    //                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
    //                            else
    //                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
    //                            if (strAccountId.Split(',').Contains(strAccountNo))
    //                            {
    //                                if (IsNonRegistered)
    //                                {
    //                                    string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
    //                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    if (L_Separate_Expenses_Tax_Amount != 0)
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                    {
    //                                        Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                    {
    //                                        Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (IsNonRegistered)
    //                                {
    //                                    string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
    //                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    if (L_Separate_Expenses_Tax_Amount != 0)
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                    {
    //                                        Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
    //                                    {
    //                                        Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    //End Code
    //                    // Insert Expenses Entry
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    //------------------------------------------------------------------------------------------------------------------------
    //                    //Start Code
    //                    //Payment Entries In Voucher
    //                    double Cash = 0;
    //                    double AGeingFrnAmt = 0;
    //                    string strPaymentType = string.Empty;
    //                    DataTable dtPaymentTran = ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
    //                    if (dtPaymentTran.Rows.Count > 0)
    //                    {
    //                        for (int i = 0; i < dtPaymentTran.Rows.Count; i++)
    //                        {
    //                            string strSupplierAcc = "0";
    //                            if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
    //                            {
    //                                //Check account exist or not in selected currency - Neelkanth Purohit 24-08-2018
    //                                strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dtPaymentTran.Rows[i]["PayCurrencyId"].ToString()).ToString();
    //                                if (strSupplierAcc == "0")
    //                                {
    //                                    throw new System.ArgumentException("Account No not exist for this invoice currency , first create it");
    //                                }
    //                            }
    //                            strPaymentType = dtPaymentTran.Rows[i]["PaymentType"].ToString();
    //                            string strPayAmount = dtPaymentTran.Rows[i]["Pay_Charges"].ToString();
    //                            if (strPaymentType == "Cash" || strPaymentType == "Card")
    //                            {
    //                                if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
    //                                {
    //                                    string L_Debit_Amount = string.Empty;
    //                                    string F_Debit_Amount = string.Empty;
    //                                    string C_Debit_Amount = string.Empty;
    //                                    if (IsNonRegistered)
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
    //                                    }
    //                                    else
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                                    }
    //                                    if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    else
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    string L_Debit_Amount = string.Empty;
    //                                    string F_Debit_Amount = string.Empty;
    //                                    string C_Debit_Amount = string.Empty;
    //                                    if (IsNonRegistered)
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
    //                                    }
    //                                    else
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                                    }
    //                                    if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "False", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    else
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                }
    //                                strPayTotal = (Convert.ToDouble(strPayTotal) + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString())).ToString();
    //                                Cash = Cash + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString());
    //                                AGeingFrnAmt = AGeingFrnAmt + Convert.ToDouble(dtPaymentTran.Rows[i]["FCPayAmount"].ToString());
    //                            }
    //                            else if (strPaymentType == "Credit")
    //                            {
    //                                if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
    //                                {
    //                                    string L_Debit_Amount = string.Empty;
    //                                    string F_Debit_Amount = string.Empty;
    //                                    string C_Debit_Amount = string.Empty;
    //                                    if (IsNonRegistered)
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
    //                                    }
    //                                    else
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                                    }
    //                                    if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    else
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    string L_Debit_Amount = string.Empty;
    //                                    string F_Debit_Amount = string.Empty;
    //                                    string C_Debit_Amount = string.Empty;
    //                                    if (IsNonRegistered)
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
    //                                    }
    //                                    else
    //                                    {
    //                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
    //                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
    //                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
    //                                    }
    //                                    if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                    else
    //                                    {
    //                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    DataTable newcheck = objVoucherDetail.GetSumRecordByVoucherNo(strMaxId, ref trns);
    //                    if (newcheck.Rows.Count > 0)
    //                    {
    //                        string strRoundCurrencyId = Session["LocCurrencyId"].ToString();
    //                        double DebitTotal = Convert.ToDouble(newcheck.Rows[0]["DebitTotal"].ToString());
    //                        double CreditTotal = Convert.ToDouble(newcheck.Rows[0]["CreditTotal"].ToString());
    //                        if (DebitTotal == CreditTotal)
    //                        {
    //                        }
    //                        else
    //                        {
    //                            if (DebitTotal > CreditTotal)
    //                            {
    //                                double diff = DebitTotal - CreditTotal;
    //                                string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
    //                                string CompanyCurrRoundCredit = strRoundCr.Trim().Split('/')[0].ToString();
    //                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", "0.00", diff.ToString(), "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundCredit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                            }
    //                            else if (CreditTotal > DebitTotal)
    //                            {
    //                                double diff = CreditTotal - DebitTotal;
    //                                string strRoundDr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
    //                                string CompanyCurrRoundDebit = strRoundDr.Trim().Split('/')[0].ToString();
    //                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", diff.ToString(), "0.00", "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundDebit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
    //                            }
    //                        }
    //                    }
    //                }
    //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    //            }
    //            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    //        }
    //        if (Session["VPost"].ToString() == "True")
    //        {
    //            DisplayMessage("Invoice has been Posted");
    //        }
    //        else
    //        {
    //            DisplayMessage("Record Saved", "green");
    //        }
    //        //here we commit transaction when all data insert and update proper 
    //        trns.Commit();
    //        if (con.State == System.Data.ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //        trns.Dispose();
    //        con.Dispose();
    //        Reset();
    //        Lbl_Tab_New.Text = Resources.Attendance.New;
    //        FillGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //        DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
    //        trns.Rollback();
    //        if (con.State == System.Data.ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //        trns.Dispose();
    //        con.Dispose();
    //        return;
    //    }
    //}



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (HdnEdit.Value != "" && HdnEdit.Value != "0")
        {
            //If Invoice Already Posted then only User Can Add Expenses on 28-01-2025
            DataTable dtPostedPI = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value);
            if (dtPostedPI.Rows.Count > 0)
            {
                string strMaxIdPosted = string.Empty;
                bool PIPost = Convert.ToBoolean(dtPostedPI.Rows[0]["Post"].ToString());
                if (PIPost)
                {
                    //Get the Updated Average Cost First
                    //First Get Data from Stock Detail to Update
                    string strPaymentVoucherAccPosted = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    string strAccountIdPosted = txtPurchaseAccount.Text.Split('/')[1].ToString();
                    string strRoundoffAccountPosted = string.Empty;

                    foreach (GridViewRow gvr in gvProduct.Rows)
                    {
                        Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                        Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                        DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                        TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
                        Label lblPOId = (Label)gvr.FindControl("lblPOId");
                        Label OrderQty = (Label)gvr.FindControl("OrderQty");
                        TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
                        Label QtyReceived = (Label)gvr.FindControl("QtyReceived");
                        TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
                        Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
                        TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                        TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
                        Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
                        TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
                        TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
                        Label lblAmount = (Label)gvr.FindControl("lblAmount");
                        Label lblRecQty = (Label)gvr.FindControl("lblRecQty");

                        Label lblExpiryDate = (Label)gvr.FindControl("lblgvExpiryDate");
                        Label lblBatchNo = (Label)gvr.FindControl("lblgvBatchNo");

                        string UnitCost = (Convert.ToDouble(((Convert.ToDouble(lblUnitRate.Text) + Convert.ToDouble(Convert_Into_DF(lblTaxValue.Text))) - Convert.ToDouble(lblDiscountValue.Text)).ToString()) * Convert.ToDouble(txtCostingRate.Text)).ToString();
                        if (lblExpiryDate.Text == "")
                        {
                            lblExpiryDate.Text = "1/1/1800";
                        }
                        if (lblUnitRate.Text == "")
                        {
                            lblUnitRate.Text = "0";
                        }
                        if (QtyGoodReceived.Text == "")
                        {
                            QtyGoodReceived.Text = "0";
                        }
                        if (lblTax.Text == "")
                        {
                            lblTax.Text = "0";
                        }
                        if (lblTaxValue.Text == "")
                        {
                            lblTaxValue.Text = Convert_Into_DF("0");
                        }
                        if (lblDiscount.Text == "")
                        {
                            lblDiscount.Text = "0";
                        }
                        if (lblDiscountValue.Text == "")
                        {
                            lblDiscountValue.Text = "0";
                        }

                        try
                        {
                            int i = objDa.execute_Command("Delete from Inv_ProductLedger where TransType='PG' and TransTypeId='" + HdnEdit.Value + "' and ProductId='" + lblgvProductId.Text + "' and Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "'");
                        }
                        catch (Exception ex)
                        {

                        }

                        //Insert Data in Product Ledger and Update the Stock Detail
                        string Qty = objDa.get_SingleValue("Select  Coversion_Qty from Inv_UnitMaster where unit_Id = '" + ddlUnitName.SelectedValue + "'");
                        float Unit_Cost = float.Parse(UnitCost) / float.Parse(Qty);

                        ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", lblBatchNo.Text, lblRecQty.Text.Trim(), "0", "1/1/1800", Unit_Cost.ToString(), lblExpiryDate.Text, "0", "1/1/1800", "0", "0", "AlreadyPosted", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

                    }


                    //New Expense Entry
                    string strTotalExpenseAmount = "";
                    DataTable dtExpense = new DataTable();
                    dtExpense = (DataTable)ViewState["Expdt"];
                    if (dtExpense != null)
                    {
                        //try
                        //{
                        //    string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                        //    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, ddlCurrency.SelectedValue, txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
                        //}
                        //catch
                        //{
                        //}
                        try
                        {

                            foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                            {
                                DataTable dtInsertedExpenses = objDa.return_DataTable("Select * from Inv_ShipExpDetail where IsActive='True' and InvoiceNo='" + HdnEdit.Value + "' and Location_Id='" + StrLocationId + "' and Expense_Id='" + dr["Expense_Id"].ToString() + "'");
                                if (dtInsertedExpenses.Rows.Count > 0)
                                {

                                }
                                else
                                {
                                    ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "NotPosted", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    if (strTotalExpenseAmount == "")
                                    {
                                        strTotalExpenseAmount = dr["Exp_Charges"].ToString();
                                    }
                                    else
                                    {
                                        strTotalExpenseAmount = (float.Parse(strTotalExpenseAmount) + float.Parse(dr["Exp_Charges"].ToString())).ToString();
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }


                    //Start the Finance Entry
                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    DataTable dtAcParameterLocation = objAccParameterLocation.GetParameterMasterAllTrue(StrCompId, StrBrandId, StrLocationId);

                    bool strTransferInFinance = false;
                    DataTable dtTransferInFinance = new DataView(dtAcParameterLocation, "Param_Name='Allow Transfer In Finance'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTransferInFinance.Rows.Count > 0)
                    {
                        strTransferInFinance = Convert.ToBoolean(dtTransferInFinance.Rows[0]["Param_Value"].ToString());
                    }
                    else
                    {
                        strTransferInFinance = true;
                    }


                    if (strTransferInFinance == true)
                    {
                        objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, DateTime.Now.ToString(), "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    }
                    else if (strTransferInFinance == false)
                    {
                        objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, DateTime.Now.ToString(), "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), true.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    }




                    DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId();
                    if (dtMaxId.Rows.Count > 0)
                    {
                        strMaxIdPosted = dtMaxId.Rows[0][0].ToString();
                    }

                    int j = 0;
                    string strPayTotal = "0";
                    string strCashAccount = string.Empty;
                    string strCreditAccount = string.Empty;
                    //string strPaymentVoucherAcc = string.Empty;



                    DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
                    DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCash.Rows.Count > 0)
                    {
                        strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                    }
                    DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCredit.Rows.Count > 0)
                    {
                        strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
                    }

                    DataTable dtRoundoff = new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtRoundoff.Rows.Count > 0)
                    {
                        strRoundoffAccountPosted = dtRoundoff.Rows[0]["Param_Value"].ToString();
                    }
                    //For Advance Debit Account
                    string strAdvanceDebitAC = string.Empty;
                    DataTable dtAdvanceDebitAC = new DataView(dtAcParameter, "Param_Name='PO Advance Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtAdvanceDebitAC.Rows.Count > 0)
                    {
                        strAdvanceDebitAC = dtAdvanceDebitAC.Rows[0]["Param_Value"].ToString();
                    }


                    bool IsNonRegistered = false;
                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, txtSupplierName.Text.Trim().Split('/')[1].ToString());
                    if (dtSup.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(dtSup.Rows[0]["Field2"].ToString()))
                            IsNonRegistered = Convert.ToBoolean(dtSup.Rows[0]["Field2"].ToString());
                    }

                    double fOrderExp = 0;

                    // Code By Ghanshyam Suthar on 11-04-2018
                    double Exchange_Rate = 0;
                    if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                    {
                        if (txtExchangeRate.Text.Trim() == "")
                            Exchange_Rate = 1;
                        else
                            Exchange_Rate = Convert.ToDouble(txtExchangeRate.Text.Trim());
                    }
                    else
                    {
                        Exchange_Rate = 1;
                    }


                    // For Location Currency
                    double L_Advance_Expenses_Amount = 0;
                    double L_Invoice_Without_Tax_Amount = 0;
                    double L_Invoice_Tax_Amount = 0;
                    double L_Invoice_With_Tax_Amount = 0;
                    double L_Total_Expenses_Without_Tax_Amount = 0;
                    double L_Total_Expenses_Tax_Amount = 0;
                    double L_Total_Expenses_With_Tax_Amount = 0;
                    double L_Separate_Expenses_Without_Tax_Amount = 0;
                    double L_Separate_Expenses_Tax_Amount = 0;
                    double L_Separate_Expenses_With_Tax_Amount = 0;
                    //---------------------------------------------------------------------------------------------------------------------
                    // For Foregin Currency
                    double F_Advance_Expenses_Amount = 0;
                    double F_Invoice_Without_Tax_Amount = 0;
                    double F_Invoice_Tax_Amount = 0;
                    double F_Invoice_With_Tax_Amount = 0;
                    double F_Total_Expenses_Without_Tax_Amount = 0;
                    double F_Total_Expenses_Tax_Amount = 0;
                    double F_Total_Expenses_With_Tax_Amount = 0;
                    double F_Separate_Expenses_Without_Tax_Amount = 0;
                    double F_Separate_Expenses_Tax_Amount = 0;
                    double F_Separate_Expenses_With_Tax_Amount = 0;
                    //---------------------------------------------------------------------------------------------------------------------
                    // For Company Currency
                    double C_Advance_Expenses_Amount = 0;
                    double C_Invoice_Without_Tax_Amount = 0;
                    double C_Invoice_Tax_Amount = 0;
                    double C_Invoice_With_Tax_Amount = 0;
                    double C_Total_Expenses_Without_Tax_Amount = 0;
                    double C_Total_Expenses_Tax_Amount = 0;
                    double C_Total_Expenses_With_Tax_Amount = 0;
                    double C_Separate_Expenses_Without_Tax_Amount = 0;
                    double C_Separate_Expenses_Tax_Amount = 0;
                    double C_Separate_Expenses_With_Tax_Amount = 0;
                    double Advance_Amount = 0;
                    double NetTaxVal = 0;
                    double GrandTotal = 0;
                    double Total_Expenses_Tax = 0;
                    double Total_Expenses = 0;


                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strAccountIdPosted, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", strTotalExpenseAmount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strTotalExpenseAmount, strTotalExpenseAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());

                    DataTable dtPostedExpenses = objDa.return_DataTable("Select * from Inv_ShipExpDetail as SD inner join Inv_ShipExpMaster SM on SM.Expense_Id=SD.Expense_Id where SD.IsActive='True' and SD.InvoiceNo='" + HdnEdit.Value + "' and SD.Location_Id='" + StrLocationId + "' and SD.Field5='NotPosted'");
                    if (dtPostedExpenses.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtPostedExpenses.Rows.Count; i++)
                        {
                            string strExpensesId = dtPostedExpenses.Rows[i]["Expense_Id"].ToString();

                            int G = objDa.execute_Command("Update  Inv_ShipExpDetail  Set Field5 =''  Where IsActive='True' and InvoiceNo='" + HdnEdit.Value + "' and Location_Id='" + StrLocationId + "' and Field5='NotPosted' and Expense_Id='" + strExpensesId + "'");

                            //Start Code
                            // Insert Expenses Entry
                            double Expenses_Tax = 0;
                            string[,] Net_Expenses_Tax = new string[1, 5];
                            double Exp = 0;
                            double SupplierExp = 0;

                            string strExpensesName = dtPostedExpenses.Rows[i]["Exp_Name"].ToString();
                            string strForeignAmount = dtPostedExpenses.Rows[i]["FCExpAmount"].ToString();

                            double strExpAmount = Convert.ToDouble(dtPostedExpenses.Rows[i]["Exp_Charges"].ToString());
                            string strAccountNo = dtPostedExpenses.Rows[i]["Account_No"].ToString();
                            string strExpCurrencyId = dtPostedExpenses.Rows[i]["ExpCurrencyID"].ToString();
                            string strExchangeRate = dtPostedExpenses.Rows[i]["ExpExchangeRate"].ToString();
                            Exp = Convert.ToDouble(SetDecimal((Exp + Convert.ToDouble(dtPostedExpenses.Rows[i]["Exp_Charges"].ToString())).ToString()));
                            if (strExpensesName == "")
                            {
                                strExpensesName = GetExpName(strExpensesId);
                            }

                            Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax_Post(strExpAmount.ToString()));
                            double Expenses_Exchange_Rate = 0;
                            if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                            {
                                if (strExchangeRate.Trim() == "")
                                    Expenses_Exchange_Rate = 1;
                                else
                                    Expenses_Exchange_Rate = Convert.ToDouble(SetDecimal(strExchangeRate.Trim()));
                            }
                            else
                            {
                                Expenses_Exchange_Rate = 1;
                            }
                            L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((Expenses_Tax).ToString()));
                            F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                            C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            L_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((strExpAmount).ToString()));
                            F_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Without_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                            C_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            if (strAccountNo == strPaymentVoucherAccPosted)
                            {
                                //Check account exist or not in selected currency - Neelkanth Purohit -24-08-2018
                                string strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dtPostedExpenses.Rows[i]["ExpCurrencyId"].ToString()).ToString();
                                if (strSupplierAcc == "0")
                                {
                                    throw new System.ArgumentException("Account No not exist for this expenses currency , first create it");
                                }
                                string L_Debit_Amount = string.Empty;
                                string F_Debit_Amount = string.Empty;
                                string C_Debit_Amount = string.Empty;
                                if (IsNonRegistered)
                                {
                                    L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                    F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                    C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                }
                                else
                                {
                                    L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                    F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                    C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                }
                                //For Debit Entry
                                SupplierExp += L_Separate_Expenses_Without_Tax_Amount;
                                string Exp_Narration = string.Empty;
                                if (txtAirwaybillno.Text != "")
                                    Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                else
                                    Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                if (IsNonRegistered)
                                {
                                    string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strPaymentVoucherAccPosted, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    if (L_Separate_Expenses_Tax_Amount != 0)
                                    {
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    }
                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                    {
                                        //Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAccPosted, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId);
                                    }
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strPaymentVoucherAccPosted, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                    {
                                        //Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAccPosted, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId);
                                    }
                                }
                            }
                            else
                            {
                                string L_Debit_Amount = string.Empty;
                                string F_Debit_Amount = string.Empty;
                                string C_Debit_Amount = string.Empty;
                                if (IsNonRegistered)
                                {
                                    L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                    F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                    C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                }
                                else
                                {
                                    L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                    F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                    C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                }
                                //For Credit Entry                                
                                string Exp_Narration = string.Empty;
                                if (txtAirwaybillno.Text != "")
                                    Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                else
                                    Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                if (strAccountIdPosted.Split(',').Contains(strAccountNo))
                                {
                                    if (IsNonRegistered)
                                    {
                                        string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        if (L_Separate_Expenses_Tax_Amount != 0)
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            //Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId );
                                        }
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            //Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId);
                                        }
                                    }
                                }
                                else
                                {
                                    if (IsNonRegistered)
                                    {
                                        string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        if (L_Separate_Expenses_Tax_Amount != 0)
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            //Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId);
                                        }
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            //Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId);
                                        }
                                    }
                                }
                            }



                            //End Code
                            // Insert Expenses Entry

                            //---------------------------------------------------------------------------------------------------------------------

                            DataTable newcheckPosted = objVoucherDetail.GetSumRecordByVoucherNo(strMaxIdPosted);
                            if (newcheckPosted.Rows.Count > 0)
                            {
                                string strRoundCurrencyId = Session["LocCurrencyId"].ToString();
                                double DebitTotal = Convert.ToDouble(newcheckPosted.Rows[0]["DebitTotal"].ToString());
                                double CreditTotal = Convert.ToDouble(newcheckPosted.Rows[0]["CreditTotal"].ToString());
                                if (DebitTotal == CreditTotal)
                                {

                                }
                                else
                                {
                                    if (DebitTotal > CreditTotal)
                                    {
                                        double diff = DebitTotal - CreditTotal;
                                        string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                        string CompanyCurrRoundCredit = strRoundCr.Trim().Split('/')[0].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, "1", strRoundoffAccountPosted, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", "0.00", diff.ToString(), "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundCredit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    }
                                    else if (CreditTotal > DebitTotal)
                                    {
                                        double diff = CreditTotal - DebitTotal;
                                        string strRoundDr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                        string CompanyCurrRoundDebit = strRoundDr.Trim().Split('/')[0].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxIdPosted, (j++).ToString(), strRoundoffAccountPosted, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", diff.ToString(), "0.00", "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundDebit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    }
                                }
                                //End Finance Entry
                            }
                        }
                    }


                    DisplayMessage("Record Posted Successfully");
                    Reset();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    FillGrid();
                }
                else
                {
                    //If Record is Not Posted
                    SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
                    if (sender is Button)
                    {
                        Button btnId = (Button)sender;
                        if (btnId.ID == "btnPost")
                        {
                            Session["VPost"] = "True";
                        }
                        if (btnId.ID == "btnSave")
                        {
                            Session["VPost"] = "False";
                        }
                    }

                    //For Finance Code
                    //here we check that this page is updated by other user before save of current user 
                    //this code is created by jitendra upadhyay on 15-05-2015
                    //code start
                    if (HdnEdit.Value != "")
                    {
                        DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HdnEdit.Value);
                        if (dt.Rows.Count != 0)
                        {
                            if (ViewState["TimeStamp"].ToString() != dt.Rows[0]["Row_Lock_Id"].ToString())
                            {
                                DisplayMessage("Another User update Information reload and try again");
                                return;
                            }
                        }
                    }
                    //code end
                    string strReceivedQty = string.Empty;
                    if (txtInvoicedate.Text == "")
                    {
                        DisplayMessage("Enter Invoice Date");
                        txtInvoicedate.Focus();
                        return;
                    }
                    else
                    {
                        try
                        {
                            ObjSysParam.getDateForInput(txtInvoicedate.Text);
                        }
                        catch
                        {
                            DisplayMessage("Enter Invoice Date in format " + Session["DateFormat"].ToString() + "");
                            txtInvoicedate.Text = "";
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoicedate);
                            return;
                        }
                    }
                    //code added by jitendra upadhyay on 09-12-2016
                    //for insert record according the log in financial year
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtInvoicedate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                    {
                        DisplayMessage("Log In Financial year not allowing to perform this action");
                        return;
                    }
                    if (txtSupInvoiceDate.Text == "")
                    {
                        DisplayMessage("Select Supplier Invoice Date");
                        txtSupInvoiceDate.Focus();
                        return;
                    }
                    else
                    {
                        try
                        {
                            ObjSysParam.getDateForInput(txtSupInvoiceDate.Text);
                        }
                        catch
                        {
                            DisplayMessage("Enter Supplier Invoice Date in format " + Session["DateFormat"].ToString() + "");
                            txtSupInvoiceDate.Text = "";
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupInvoiceDate);
                            return;
                        }
                    }
                    if (txtInvoiceNo.Text == "")
                    {
                        txtInvoiceNo.Text = "";
                        txtInvoiceNo.Focus();
                        DisplayMessage("Enter Invoice No");
                        return;
                    }
                    //if (txtSupplierInvoiceNo.Text == "")
                    //{
                    //    txtSupplierInvoiceNo.Text = "";
                    //    txtSupplierInvoiceNo.Focus();
                    //    DisplayMessage("Enter Supplier Invoice No");
                    //    return;
                    //}
                    if (txtSupplierName.Text == "")
                    {
                        txtSupplierName.Focus();
                        DisplayMessage("Enter Supplier Name");
                        return;
                    }
                    if (txtCostingRate.Text == "")
                    {
                        txtCostingRate.Text = "0";
                    }
                    if (txtOtherCharges.Text == "")
                    {
                        txtOtherCharges.Text = "0";
                    }
                    if (!RdoPo.Checked && !RdoWithOutPo.Checked)
                    {
                        DisplayMessage("Select One With Purchase Order Or WithOut Purchase Order");
                        RdoPo.Focus();
                        return;
                    }
                    if (gvProduct.Rows.Count == 0)
                    {
                        DisplayMessage("Select Product");
                        return;
                    }
                    if (ddlCurrency.SelectedIndex == 0)
                    {
                        ddlCurrency.Focus();
                        DisplayMessage("Currency Required On Company Level");
                        return;
                    }
                    string strTotalQty = string.Empty;
                    string strAmount = string.Empty;
                    string PoId = string.Empty;
                    bool Post = false;
                    if (Session["VPost"].ToString() == "True")
                    {
                        DataTable dt = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value);
                        if (dt.Rows.Count > 0)
                        {
                            if (new DataView(dt, "RecQty<>'0'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                Post = true;
                            }
                            else
                            {
                                Session["VPost"] = "False";
                                Post = false;
                                DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
                                return;
                            }
                        }
                        else
                        {
                            Session["VPost"] = "False";
                            Post = false;
                            DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
                            return;
                        }
                    }
                    else if (Session["VPost"].ToString() == "False")
                    {
                        Post = false;
                    }
                    string POst = string.Empty;
                    if (RdoPo.Checked)
                    {
                        POst = "PO";
                    }
                    if (RdoWithOutPo.Checked)
                    {
                        POst = "WOutPO";
                    }
                    if (txtExchangeRate.Text.Trim() == "")
                    {
                        txtExchangeRate.Text = "0";
                    }
                    if (txtCostingRate.Text == "")
                    {
                        txtCostingRate.Text = "0";
                    }
                    if (txtBillAmount.Text == "0")
                    {
                        txtBillAmount.Text = "0";
                    }
                    //this validation for check that payment amount is equal to invoice amount or not
                    if (Session["VPost"].ToString() == "True")
                    {
                        if (txtPurchaseAccount.Text == "")
                        {
                            DisplayMessage("Enter Purchase Account");
                            txtPurchaseAccount.Focus();
                            return;
                        }
                        if (gvPayment.Rows.Count == 0)
                        {
                            Session["VPost"] = "False";
                            Post = false;
                            DisplayMessage("Enter Payment Mode Details");
                            return;
                        }
                    }
                    //For Bank Account
                    string strAccountId = txtPurchaseAccount.Text.Split('/')[1].ToString();
                    //string strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    //added payment validation according the discussion with nafees sir on 27-06-2016
                    //created by jitendra upadhyay 
                    double Invoiceamt = 0;
                    double Paymentamt = 0;
                    double advancepayment = 0;
                    if (txtGrandTotal.Text != "")
                    {
                        Invoiceamt = Convert.ToDouble(txtGrandTotal.Text);
                    }
                    if (gvPayment.Rows.Count > 0)
                    {
                        Paymentamt = Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text);
                    }
                    if (Invoiceamt != Paymentamt)
                    {
                        DisplayMessage("Payment Amount should be equal to invoice amount");
                        return;
                    }
                    //for add confirmation when invoice quantity and recenve quantityis not same 
                    if (txtCostingRate.Text.Trim() == "" || txtCostingRate.Text.Trim() == "Infinity")
                    {
                        txtCostingRate.Text = "0";
                    }

                    //Check controls Value from page setting
                    string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
                    if (result[0] == "false")
                    {
                        DisplayMessage(result[1]);
                        return;
                    }

                    string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    strTotalQty = "0";
                    strAmount = "0";
                    int b = 0;
                    string InvoiceId = "0";

                    con.Open();
                    SqlTransaction trns;
                    trns = con.BeginTransaction();
                    try
                    {
                        if (HdnEdit.Value == "")
                        {
                            //start rollback transaction
                            // GetData();
                            b = ObjPurchaseInvoice.InsertPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                            InvoiceId = b.ToString();

                            if (txtInvoiceNo.Text == ViewState["DocNo"].ToString())
                            {

                                int invoicecount = ObjPurchaseInvoice.GetInvoiceCountByLocationId(Session["LocId"].ToString(), ref trns);

                                if (invoicecount == 0)
                                {
                                    ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + "1", ref trns);
                                    txtInvoiceNo.Text = txtInvoiceNo.Text + "1";
                                }
                                else
                                {
                                    if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count > 0)
                                    {

                                        bool bCodeFlag = true;
                                        while (bCodeFlag)
                                        {
                                            invoicecount += 1;

                                            if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count == 0)
                                            {
                                                bCodeFlag = false;
                                            }

                                        }

                                    }
                                    ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + invoicecount.ToString(), ref trns);
                                    txtInvoiceNo.Text = txtInvoiceNo.Text + invoicecount.ToString();
                                }
                            }

                            DataTable dtExpense = new DataTable();
                            dtExpense = (DataTable)ViewState["Expdt"];
                            if (dtExpense != null)
                            {
                                try
                                {
                                    string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                                    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, ddlCurrency.SelectedValue, txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
                                }
                                catch
                                {
                                }
                                try
                                {
                                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                                    {
                                        //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            DataTable dtPayment = new DataTable();
                            dtPayment = (DataTable)ViewState["PayementDt"];
                            if (dtPayment != null)
                            {
                                try
                                {
                                    foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                                    {
                                        ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", InvoiceId.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", InvoiceId.ToString(), ref trns);
                            int SerialNo = 1;
                            foreach (GridViewRow gvr in gvProduct.Rows)
                            {
                                Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                                Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                                DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                                TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
                                Label lblPOId = (Label)gvr.FindControl("lblPOId");
                                Label OrderQty = (Label)gvr.FindControl("OrderQty");
                                TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
                                TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
                                Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
                                TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                                TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
                                Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
                                TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
                                TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
                                Label lblAmount = (Label)gvr.FindControl("lblAmount");
                                Label lblRecQty = (Label)gvr.FindControl("lblRecQty");
                                if (lblUnitRate.Text == "")
                                {
                                    lblUnitRate.Text = "0";
                                }
                                if (lblTax.Text == "")
                                {
                                    lblTax.Text = "0";
                                }
                                if (lblTaxValue.Text == "")
                                {
                                    lblTaxValue.Text = Convert_Into_DF("0");
                                }
                                if (lblDiscount.Text == "")
                                {
                                    lblDiscount.Text = "0";
                                }
                                if (lblDiscountValue.Text == "")
                                {
                                    lblDiscountValue.Text = "0";
                                }
                                //if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), lblgvProductId.Text, Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "NS")
                                //{
                                //    lblRecQty.Text = QtyGoodReceived.Text.ToString();
                                //}
                                if (QtyGoodReceived.Text != "" || QtyGoodReceived.Text != "0")
                                {
                                    int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), b.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    Insert_Tax("gvProduct", b.ToString(), Details_ID.ToString(), gvr, ref trns);
                                    SerialNo++;
                                    //this code is created by jitendra upadhya on 14-07-2015
                                    //this code for insert record in tax ref detail table when we apply multiple tax according product category
                                    //code start
                                    foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                                    {
                                        if (Convert.ToDouble(lblAmount.Text) != 0)
                                            objTaxRefDetail.InsertRecord("PINV", InvoiceId, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                //code end
                            }
                            Tax_Insert_Into_Inv_TaxRefDetail(InvoiceId, ref trns);
                            //committ
                        }
                        else
                        {
                            string strPOId = string.Empty;
                            string ShippingLine = string.Empty;
                            string shipUnit = string.Empty;
                            if (ddlShipUnit.SelectedIndex == 0)
                            {
                                shipUnit = "0";
                            }
                            else
                            {
                                shipUnit = ddlShipUnit.SelectedValue;
                            }
                            if (txtTotalWeight.Text == "")
                            {
                                txtTotalWeight.Text = "0";
                            }
                            if (txtUnitRate.Text == "")
                            {
                                txtUnitRate.Text = "0";
                            }
                            if (txtShippingLine.Text != "")
                            {
                                ShippingLine = txtShippingLine.Text.Split('/')[1].ToString();
                            }
                            if (txtShippingDate.Text.Trim() != "")
                            {
                                try
                                {
                                    Convert.ToDateTime(txtShippingDate.Text);
                                }
                                catch
                                {
                                    DisplayMessage("Enter Valid Shipping Date");
                                    txtShippingDate.Focus();
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
                            else
                            {
                                DisplayMessage("Enter Shipping Date");
                                txtShippingDate.Focus();
                                trns.Rollback();
                                if (con.State == System.Data.ConnectionState.Open)
                                {
                                    con.Close();
                                }
                                trns.Dispose();
                                con.Dispose();
                                return;
                            }
                            if (txtReceivingDate.Text.Trim() != "")
                            {
                                try
                                {
                                    Convert.ToDateTime(txtReceivingDate.Text);
                                }
                                catch
                                {
                                    DisplayMessage("Enter Valid Receiving Date");
                                    txtReceivingDate.Focus();
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
                            else
                            {
                                DisplayMessage("Enter Receiving Date");
                                txtReceivingDate.Focus();
                                trns.Rollback();
                                if (con.State == System.Data.ConnectionState.Open)
                                {
                                    con.Close();
                                }
                                trns.Dispose();
                                con.Dispose();
                                return;
                            }
                            // GetData();
                            b = ObjPurchaseInvoice.UpdatePurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                            //update shipping information 
                            //delete and reinsert 
                            string sql = "delete from dbo.Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + HdnEdit.Value.Trim() + "";
                            objDa.execute_Command(sql, ref trns);
                            sql = "INSERT INTO [Inv_InvoiceShippingHeader]([Ref_Type],[Ref_Id] ,[Shipping_Line],[Ship_By] ,[Airway_Bill_No],[Ship_Unit],[Actual_Weight],[Volumetric_weight],[Shipping_Date],[Receiving_date],[Shipment_Type],[Freight_Status],[UnitRate],[Divide_By])VALUES('PINV'," + HdnEdit.Value.Trim() + ",'" + ShippingLine.Trim() + "','" + ddlShipBy.SelectedValue.Trim() + "','" + txtAirwaybillno.Text.Trim() + "','" + shipUnit.Trim() + "','" + txtTotalWeight.Text.Trim() + "','" + txtvolumetricweight.Text.Trim() + "','" + ObjSysParam.getDateForInput(txtShippingDate.Text).ToString().Trim() + "','" + ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString().Trim() + "','" + ddlShipmentType.SelectedValue.Trim() + "','" + ddlFreightStatus.SelectedValue.Trim() + "','" + txtUnitRate.Text + "','" + txtdivideby.Text.Trim() + "')";
                            objDa.execute_Command(sql, ref trns);
                            DataTable dtExpense = new DataTable();
                            dtExpense = (DataTable)ViewState["Expdt"];
                            if (dtExpense != null)
                            {
                                try
                                {
                                    ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, "PI", ref trns);
                                    string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                                    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim().ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                }
                                catch
                                {

                                }
                                try
                                {
                                    ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.ToString(), "PI", ref trns);
                                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                                    {
                                        //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            DataTable dtPayment = new DataTable();
                            dtPayment = (DataTable)ViewState["PayementDt"];
                            if (dtPayment != null)
                            {
                                try
                                {
                                    ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
                                    foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                                    {
                                        ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", HdnEdit.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            //here we update receive qty in one table 
                            //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
                            //by jitendra upadhyay 
                            //on 22-02-2016
                            //code start
                            DataTable dtInvDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
                            //code end
                            ObjPurchaseInvoiceDetail.DeletePurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
                            int SerialNo = 1;
                            CostingRate();
                            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", HdnEdit.Value.Trim(), ref trns);
                            foreach (GridViewRow gvr in gvProduct.Rows)
                            {
                                Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                                Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                                DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                                TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
                                Label lblPOId = (Label)gvr.FindControl("lblPOId");
                                strPOId = lblPOId.Text;
                                Label OrderQty = (Label)gvr.FindControl("OrderQty");
                                TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
                                Label QtyReceived = (Label)gvr.FindControl("QtyReceived");
                                TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
                                Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
                                TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                                TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
                                Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
                                TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
                                TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
                                Label lblAmount = (Label)gvr.FindControl("lblAmount");
                                Label lblRecQty = (Label)gvr.FindControl("lblRecQty");

                                Label lblExpiryDate = (Label)gvr.FindControl("lblgvExpiryDate");
                                Label lblBatchNo = (Label)gvr.FindControl("lblgvBatchNo");

                                if (lblExpiryDate.Text == "")
                                {
                                    lblExpiryDate.Text = "1/1/1800";
                                }

                                string UnitCost = (Convert.ToDouble(((Convert.ToDouble(lblUnitRate.Text) + Convert.ToDouble(Convert_Into_DF(lblTaxValue.Text))) - Convert.ToDouble(lblDiscountValue.Text)).ToString()) * Convert.ToDouble(txtCostingRate.Text)).ToString();
                                if (lblUnitRate.Text == "")
                                {
                                    lblUnitRate.Text = "0";
                                }
                                if (QtyGoodReceived.Text == "")
                                {
                                    QtyGoodReceived.Text = "0";
                                }
                                if (lblTax.Text == "")
                                {
                                    lblTax.Text = "0";
                                }
                                if (lblTaxValue.Text == "")
                                {
                                    lblTaxValue.Text = Convert_Into_DF("0");
                                }
                                if (lblDiscount.Text == "")
                                {
                                    lblDiscount.Text = "0";
                                }
                                if (lblDiscountValue.Text == "")
                                {
                                    lblDiscountValue.Text = "0";
                                }


                                //if (lblRecQty.Text == "")
                                //{
                                //    lblRecQty.Text = "0";
                                //}
                                //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
                                //by jitendra upadhyay 
                                //on 20-02-2016
                                //code start
                                //string strsql = "select RecQty from Inv_PurchaseInvoiceDetail where InvoiceNo=" + HdnEdit.Value.ToString() + " and ProductId=" + lblgvProductId.Text + "";
                                //DataTable dtRecqty= objDa.return_DataTable(strsql,ref trns);
                                //if (dtRecqty.Rows.Count > 0)
                                //{
                                DataTable dtTemp = new DataView(dtInvDetail, "ProductId=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtTemp.Rows.Count > 0)
                                {
                                    if (dtTemp.Rows.Count == 1)
                                    {
                                        lblRecQty.Text = dtTemp.Rows[0]["RecQty"].ToString();
                                    }
                                }

                                int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                Insert_Tax("gvProduct", HdnEdit.Value.ToString(), Details_ID.ToString(), gvr, ref trns);
                                //cpde start
                                if (Session["VPost"].ToString() == "True")
                                {
                                    string strMaintainStock = "";
                                    string strProductCode = "";
                                    //Check the Batch Number and Expiry Date Value.
                                    DataTable dtItemDetail = objDa.return_DataTable("Select * from Inv_ProductMaster where ProductId='" + lblgvProductId.Text + "'");
                                    if (dtItemDetail.Rows.Count > 0)
                                    {
                                        strMaintainStock = dtItemDetail.Rows[0]["MaintainStock"].ToString();
                                        strProductCode = dtItemDetail.Rows[0]["ProductCode"].ToString();
                                        if (strMaintainStock == "BatchNo")
                                        {
                                            if (lblBatchNo.Text == "")
                                            {
                                                DisplayMessage("Please Enter the Expiry Date and Batch Number for Maintain Stock");
                                                txtReceivingDate.Focus();
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
                                        else if (strMaintainStock == "Expiry")
                                        {
                                            if (lblExpiryDate.Text == "")
                                            {
                                                DisplayMessage("Please Enter the Expiry Date for Maintain Stock");
                                                txtReceivingDate.Focus();
                                                trns.Rollback();
                                                if (con.State == System.Data.ConnectionState.Open)
                                                {
                                                    con.Close();
                                                }
                                                trns.Dispose();
                                                con.Dispose();
                                                return;
                                            }
                                            if (lblBatchNo.Text == "")
                                            {
                                                DisplayMessage("Please Enter the Batch No for Maintain Stock");
                                                txtReceivingDate.Focus();
                                                trns.Rollback();
                                                if (con.State == System.Data.ConnectionState.Open)
                                                {
                                                    con.Close();
                                                }
                                                trns.Dispose();
                                                con.Dispose();
                                                return;
                                            }
                                            if (lblExpiryDate.Text == "1/1/1800")
                                            {
                                                DisplayMessage("Please Enter the Expiry Date for Maintain Stock");
                                                txtReceivingDate.Focus();
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

                                    string Qty = objDa.get_SingleValue("Select  Coversion_Qty from Inv_UnitMaster where unit_Id = '" + ddlUnitName.SelectedValue + "'");
                                    float Unit_Cost = float.Parse(UnitCost) / float.Parse(Qty);

                                    ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", lblBatchNo.Text, lblRecQty.Text.Trim(), "0", "1/1/1800", Unit_Cost.ToString(), lblExpiryDate.Text, "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);


                                    //Entry for Stock Batch Master Table
                                    if (strMaintainStock == "Expiry")
                                    {
                                        if (lblRecQty.Text != "" && lblRecQty.Text != "0")
                                        {

                                            //Code for Create Barcode
                                            string strBarcode = "";
                                            if (lblExpiryDate.Text != "" && lblBatchNo.Text != "")
                                            {
                                                string formattedDate = DateTime.Parse(lblExpiryDate.Text).ToString("ddMMyyyy");
                                                strBarcode = formattedDate + "/" + strProductCode;
                                            }


                                            ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "", lblBatchNo.Text, lblRecQty.Text, lblExpiryDate.Text, "", "1/1/1800", strBarcode, "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);


                                            //decimal dQty = decimal.Parse(lblRecQty.Text);
                                            //int intQty = (int)dQty;
                                            //for (int i = 0; i < intQty; i++)
                                            //{
                                            //    //Code for Create Barcode
                                            //    string strBarcode = "";
                                            //    if (lblExpiryDate.Text != "" && lblBatchNo.Text != "")
                                            //    {
                                            //        string formattedDate = DateTime.Parse(lblExpiryDate.Text).ToString("ddMMyyyy");
                                            //        strBarcode = strProductCode + "/" + formattedDate + "/" + lblBatchNo.Text;
                                            //    }
                                            //    else if (lblBatchNo.Text != "" && lblExpiryDate.Text == "")
                                            //    {
                                            //        strBarcode = strProductCode + "/" + lblBatchNo.Text;
                                            //    }
                                            //    else if (lblBatchNo.Text == "" && lblExpiryDate.Text != "")
                                            //    {
                                            //        string formattedDate = DateTime.Parse(lblExpiryDate.Text).ToString("ddMMyyyy");
                                            //        strBarcode = strProductCode + "/" + formattedDate;
                                            //    }

                                            //    ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "", lblBatchNo.Text, "1", lblExpiryDate.Text, "", "1/1/1800", strBarcode, "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            //}
                                        }
                                    }

                                    //ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", "0", lblRecQty.Text.Trim(), "0", "1/1/1800", UnitCost.ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                }
                                //}
                                //code end
                                SerialNo++;
                                //this code is created by jitendra upadhya on 14-07-2015
                                //this code for insert record in tax ref detail table when we apply multiple tax according product category
                                //code start
                                foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                                {
                                    if (Convert.ToDouble(lblAmount.Text) != 0)
                                        objTaxRefDetail.InsertRecord("PINV", HdnEdit.Value, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                                //code end
                            }
                            Tax_Insert_Into_Inv_TaxRefDetail(HdnEdit.Value, ref trns);
                            //Add Post Value Posted for Account Section On 31-03-2014 By Lokesh
                            if (Post == true)
                            {
                                string strRefrenceNumber = string.Empty;
                                if (HdnEdit.Value != "")
                                {
                                    foreach (GridViewRow gvrow in gvProduct.Rows)
                                    {
                                        string SalesPrice = string.Empty;
                                        Label lblUnitRateLocal = (Label)gvrow.FindControl("lblUnitRateLocal");
                                        Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                                        if (lblUnitRateLocal.Text == "")
                                        {
                                            lblUnitRateLocal.Text = "0";
                                        }

                                        SalesPrice = SetDecimal((Convert.ToDouble(lblUnitRateLocal.Text) * Convert.ToDouble(ViewState["ExchangeRateinComp"].ToString())).ToString());
                                        DataTable dtcustomerPriceList = objContactPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S", ref trns);
                                        try
                                        {
                                            dtcustomerPriceList = new DataView(dtcustomerPriceList, "Product_Id=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                                        }
                                        catch
                                        {

                                        }

                                        if (dtcustomerPriceList.Rows.Count == 0)
                                        {
                                            objContactPriceList.InsertContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            objContactPriceList.UpdateContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                    }
                                    //Commented Code Start
                                    strRefrenceNumber = txtSupplierInvoiceNo.Text;


                                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                    DataTable dtAcParameterLocation = objAccParameterLocation.GetParameterMasterAllTrue(StrCompId, StrBrandId, StrLocationId, ref trns);

                                    bool strTransferInFinance = false;
                                    DataTable dtTransferInFinance = new DataView(dtAcParameterLocation, "Param_Name='Allow Transfer In Finance'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtTransferInFinance.Rows.Count > 0)
                                    {
                                        strTransferInFinance = Convert.ToBoolean(dtTransferInFinance.Rows[0]["Param_Value"].ToString());
                                    }
                                    else
                                    {
                                        strTransferInFinance = true;
                                    }


                                    if (strTransferInFinance == true)
                                    {
                                        objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, txtInvoicedate.Text, "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else if (strTransferInFinance == false)
                                    {
                                        objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, txtInvoicedate.Text, "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), true.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }



                                    string strMaxId = string.Empty;
                                    DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                                    if (dtMaxId.Rows.Count > 0)
                                    {
                                        strMaxId = dtMaxId.Rows[0][0].ToString();
                                    }
                                    int j = 0;
                                    string strPayTotal = "0";
                                    string strCashAccount = string.Empty;
                                    string strCreditAccount = string.Empty;
                                    //string strPaymentVoucherAcc = string.Empty;
                                    string strRoundoffAccount = string.Empty;


                                    DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId, ref trns);
                                    DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtCash.Rows.Count > 0)
                                    {
                                        strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                                    }
                                    DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtCredit.Rows.Count > 0)
                                    {
                                        strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
                                    }
                                    //DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                                    //if (dtPaymentVoucher.Rows.Count > 0)
                                    //{
                                    //    strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
                                    //}
                                    DataTable dtRoundoff = new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtRoundoff.Rows.Count > 0)
                                    {
                                        strRoundoffAccount = dtRoundoff.Rows[0]["Param_Value"].ToString();
                                    }
                                    bool IsNonRegistered = false;
                                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, txtSupplierName.Text.Trim().Split('/')[1].ToString());
                                    if (dtSup.Rows.Count > 0)
                                    {
                                        if (!String.IsNullOrEmpty(dtSup.Rows[0]["Field2"].ToString()))
                                            IsNonRegistered = Convert.ToBoolean(dtSup.Rows[0]["Field2"].ToString());
                                    }
                                    //For Advance Debit Account
                                    string strAdvanceDebitAC = string.Empty;
                                    DataTable dtAdvanceDebitAC = new DataView(dtAcParameter, "Param_Name='PO Advance Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtAdvanceDebitAC.Rows.Count > 0)
                                    {
                                        strAdvanceDebitAC = dtAdvanceDebitAC.Rows[0]["Param_Value"].ToString();
                                    }


                                    double fOrderExp = 0;
                                    //Commented On 28-02-2023
                                    //foreach (GridViewRow gvr in gvOrderExpenses.Rows)
                                    //{
                                    //    Label lblExpCharge = (Label)gvr.FindControl("lblgvExp_Charges");
                                    //    if (lblExpCharge.Text != "" && lblExpCharge.Text != "0")
                                    //    {
                                    //        fOrderExp = fOrderExp + Convert.ToDouble(lblExpCharge.Text);
                                    //    }
                                    //}



                                    // Code By Ghanshyam Suthar on 11-04-2018
                                    double Exchange_Rate = 0;
                                    if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                                    {
                                        if (txtExchangeRate.Text.Trim() == "")
                                            Exchange_Rate = 1;
                                        else
                                            Exchange_Rate = Convert.ToDouble(txtExchangeRate.Text.Trim());
                                    }
                                    else
                                    {
                                        Exchange_Rate = 1;
                                    }

                                    //---------------------------------------------------------------------------------------------------------------------
                                    // For Location Currency
                                    double L_Advance_Expenses_Amount = 0;
                                    double L_Invoice_Without_Tax_Amount = 0;
                                    double L_Invoice_Tax_Amount = 0;
                                    double L_Invoice_With_Tax_Amount = 0;
                                    double L_Total_Expenses_Without_Tax_Amount = 0;
                                    double L_Total_Expenses_Tax_Amount = 0;
                                    double L_Total_Expenses_With_Tax_Amount = 0;
                                    double L_Separate_Expenses_Without_Tax_Amount = 0;
                                    double L_Separate_Expenses_Tax_Amount = 0;
                                    double L_Separate_Expenses_With_Tax_Amount = 0;
                                    //---------------------------------------------------------------------------------------------------------------------
                                    // For Foregin Currency
                                    double F_Advance_Expenses_Amount = 0;
                                    double F_Invoice_Without_Tax_Amount = 0;
                                    double F_Invoice_Tax_Amount = 0;
                                    double F_Invoice_With_Tax_Amount = 0;
                                    double F_Total_Expenses_Without_Tax_Amount = 0;
                                    double F_Total_Expenses_Tax_Amount = 0;
                                    double F_Total_Expenses_With_Tax_Amount = 0;
                                    double F_Separate_Expenses_Without_Tax_Amount = 0;
                                    double F_Separate_Expenses_Tax_Amount = 0;
                                    double F_Separate_Expenses_With_Tax_Amount = 0;
                                    //---------------------------------------------------------------------------------------------------------------------
                                    // For Company Currency
                                    double C_Advance_Expenses_Amount = 0;
                                    double C_Invoice_Without_Tax_Amount = 0;
                                    double C_Invoice_Tax_Amount = 0;
                                    double C_Invoice_With_Tax_Amount = 0;
                                    double C_Total_Expenses_Without_Tax_Amount = 0;
                                    double C_Total_Expenses_Tax_Amount = 0;
                                    double C_Total_Expenses_With_Tax_Amount = 0;
                                    double C_Separate_Expenses_Without_Tax_Amount = 0;
                                    double C_Separate_Expenses_Tax_Amount = 0;
                                    double C_Separate_Expenses_With_Tax_Amount = 0;
                                    double Advance_Amount = 0;
                                    double NetTaxVal = 0;
                                    double GrandTotal = 0;
                                    double Total_Expenses_Tax = 0;
                                    double Total_Expenses = 0;
                                    if (fOrderExp.ToString() != "")
                                        Advance_Amount = fOrderExp;
                                    if (txtNetTaxVal.Text.Trim() != "")
                                        NetTaxVal = Convert.ToDouble(Convert_Into_DF(txtNetTaxVal.Text.Trim()));
                                    if (txtGrandTotal.Text.Trim() != "")
                                        GrandTotal = Convert.ToDouble(txtGrandTotal.Text.Trim());
                                    if (GridExpenses.Rows.Count > 0)
                                    {
                                        if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text != "")
                                            Total_Expenses_Tax = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text);
                                        if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text != "")
                                            Total_Expenses = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text);
                                    }
                                    //-------------------------------------------------------------------------------------------------------------------------------
                                    // Location Currency
                                    L_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal(Advance_Amount.ToString()));
                                    L_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((NetTaxVal * Exchange_Rate).ToString()));
                                    L_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((GrandTotal * Exchange_Rate).ToString()));
                                    L_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount - L_Invoice_Tax_Amount).ToString()));
                                    L_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses_Tax.ToString()));
                                    L_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses.ToString()));
                                    L_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount - L_Total_Expenses_Tax_Amount).ToString()));
                                    L_Separate_Expenses_Without_Tax_Amount = 0;
                                    L_Separate_Expenses_Tax_Amount = 0;
                                    L_Separate_Expenses_With_Tax_Amount = 0;
                                    //-------------------------------------------------------------------------------------------------------------------------------
                                    // Foregin Currency
                                    F_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal((L_Advance_Expenses_Amount / Exchange_Rate).ToString()));
                                    //F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_Tax_Amount / Exchange_Rate).ToString()));
                                    F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal(NetTaxVal.ToString()));
                                    //F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount / Exchange_Rate).ToString()));
                                    F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal(GrandTotal.ToString()));
                                    F_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((F_Invoice_With_Tax_Amount - F_Invoice_Tax_Amount).ToString()));
                                    F_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Tax_Amount / Exchange_Rate).ToString()));
                                    F_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount / Exchange_Rate).ToString()));
                                    F_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Without_Tax_Amount / Exchange_Rate).ToString()));
                                    F_Separate_Expenses_Without_Tax_Amount = 0;
                                    F_Separate_Expenses_Tax_Amount = 0;
                                    F_Separate_Expenses_With_Tax_Amount = 0;
                                    //-------------------------------------------------------------------------------------------------------------------------------
                                    // Company Currency
                                    C_Advance_Expenses_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Advance_Expenses_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Invoice_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Invoice_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Invoice_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Total_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Total_Expenses_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Total_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    C_Separate_Expenses_Without_Tax_Amount = 0;
                                    C_Separate_Expenses_Tax_Amount = 0;
                                    C_Separate_Expenses_With_Tax_Amount = 0;
                                    //------------------------------------------------------------------------------------------------------------------------
                                    //First Line Entry 
                                    // Invoice_Without_Tax_Amount + Without_Expenses_Tax - Advance Amount
                                    if (strAccountId.Split(',').Contains(Session["AccountId"].ToString()))
                                    {
                                        string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                        string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                        string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();

                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                        string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                        string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    //------------------------------------------------------------------------------------------------------------------------
                                    // Debit Entry for Expenses when Supplier is Non-Registered
                                    //Start Code
                                    if (IsNonRegistered)
                                    {
                                        string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                        if (ExpensesAccountId == "0")
                                        {
                                            DisplayMessage("Please First set Expenses Account in Inventory parameter");
                                            trns.Rollback();
                                            return;
                                        }
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Invoice_Tax_Amount.ToString(), "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "'(Non registered purchase) " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Invoice_Tax_Amount.ToString(), C_Invoice_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    //End Code
                                    //------------------------------------------------------------------------------------------------------------------------
                                    // Entry for Taxes of Product
                                    // Start Code
                                    string TaxQuery = "Select * from Inv_TaxRefDetail where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' and Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
                                    DataTable dtTaxDetails = objDa.return_DataTable(TaxQuery, ref trns);
                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                    {
                                        string Product_Id = string.Empty;
                                        string Tax_Id = string.Empty;
                                        string Tax_Name = string.Empty;
                                        string Tax_Value = string.Empty;
                                        string Tax_Amount = string.Empty;
                                        string TaxAccountNo = string.Empty;
                                        string TaxAccountDetails = "Select * from Sys_TaxMaster where IsActive = 'true'";
                                        DataTable dtTaxAccountDetails = objDa.return_DataTable(TaxAccountDetails);
                                        if (dtTaxAccountDetails == null || dtTaxAccountDetails.Rows.Count == 0)
                                        {
                                            DisplayMessage("Please Configure Account for Tax in Tax Master");
                                            trns.Rollback();
                                            return;
                                        }
                                        string TaxGrouping = "Select Tax_Id,Tax_Name,STM.Field3,SUM(CONVERT(decimal(18,2),Tax_value)) as TaxAmount from Inv_TaxRefDetail inner join Sys_TaxMaster STM on STM.Trans_Id = Tax_Id where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' AND Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 group by Tax_Id,Tax_Name,STM.Field3";
                                        DataTable TaxTableGrouping = objDa.return_DataTable(TaxGrouping, ref trns);
                                        string TaxIdInfo = string.Empty;
                                        string GroupTaxId = string.Empty;
                                        double GroupTaxAmount = 0;
                                        string S_Tax_Percentage = string.Empty;
                                        string GroupTaxName = string.Empty;
                                        string strTaxPer = string.Empty;
                                        foreach (DataRow grouprow in TaxTableGrouping.Rows)
                                        {
                                            GroupTaxId = grouprow["Tax_Id"].ToString();
                                            GroupTaxAmount = Convert.ToDouble(grouprow["TaxAmount"].ToString());
                                            GroupTaxName = grouprow["Tax_Name"].ToString();
                                            TaxAccountNo = grouprow["Field3"].ToString();
                                            if (String.IsNullOrEmpty(TaxAccountNo))
                                            {
                                                DisplayMessage("Please Configure Account for Tax in Tax Master");
                                                trns.Rollback();
                                                return;
                                            }
                                            L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((GroupTaxAmount * Exchange_Rate).ToString()));
                                            F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Exchange_Rate).ToString()));
                                            C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                            S_Tax_Percentage = string.Empty;
                                            foreach (DataRow row in dtTaxDetails.Rows)
                                            {
                                                if (!TaxIdInfo.Contains(GroupTaxId))
                                                {
                                                    DataView groupView = new DataView(dtTaxDetails);
                                                    groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
                                                    GroupTaxName = string.Empty;
                                                    double N_Tax_Per = 0;
                                                    foreach (DataRow newrow in groupView.ToTable().Rows)
                                                    {
                                                        N_Tax_Per = N_Tax_Per + Convert.ToDouble(newrow["Tax_Per"].ToString());
                                                        S_Tax_Percentage = Convert.ToString(N_Tax_Per / Convert.ToDouble(groupView.ToTable().Rows.Count));
                                                    }
                                                    if (IsNonRegistered)
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Tax_Amount.ToString(), S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "' Non registered purchase", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    else
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (String.IsNullOrEmpty(TaxIdInfo))
                                                        TaxIdInfo = GroupTaxId;
                                                    else
                                                        TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //End Code
                                    //------------------------------------------------------------------------------------------------------------------------
                                    //------------------------------------------------------------------------------------------------------------------------
                                    //Start Code
                                    // Insert Expenses Entry
                                    double Expenses_Tax = 0;
                                    string[,] Net_Expenses_Tax = new string[1, 5];
                                    double Exp = 0;
                                    double SupplierExp = 0;
                                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                                    {
                                        string strExpensesName = dr["Exp_Name"].ToString();
                                        string strForeignAmount = dr["FCExpAmount"].ToString();
                                        string strExpensesId = dr["Expense_Id"].ToString();
                                        double strExpAmount = Convert.ToDouble(dr["Exp_Charges"].ToString());
                                        string strAccountNo = dr["Account_No"].ToString();
                                        string strExpCurrencyId = dr["ExpCurrencyID"].ToString();
                                        string strExchangeRate = dr["ExpExchangeRate"].ToString();
                                        Exp = Convert.ToDouble(SetDecimal((Exp + Convert.ToDouble(dr["Exp_Charges"].ToString())).ToString()));
                                        if (strExpensesName == "")
                                        {
                                            strExpensesName = GetExpName(strExpensesId);
                                        }
                                        Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax_Post(strExpAmount.ToString()));
                                        double Expenses_Exchange_Rate = 0;
                                        if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                                        {
                                            if (strExchangeRate.Trim() == "")
                                                Expenses_Exchange_Rate = 1;
                                            else
                                                Expenses_Exchange_Rate = Convert.ToDouble(SetDecimal(strExchangeRate.Trim()));
                                        }
                                        else
                                        {
                                            Expenses_Exchange_Rate = 1;
                                        }
                                        L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((Expenses_Tax).ToString()));
                                        F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                                        C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                        L_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((strExpAmount).ToString()));
                                        F_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Without_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                                        C_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                        if (strAccountNo == strPaymentVoucherAcc)
                                        {
                                            //Check account exist or not in selected currency - Neelkanth Purohit -24-08-2018
                                            string strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dr["ExpCurrencyId"].ToString()).ToString();
                                            if (strSupplierAcc == "0")
                                            {
                                                throw new System.ArgumentException("Account No not exist for this expenses currency , first create it");
                                            }
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                                F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                                C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                                F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                                C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                            }
                                            //For Debit Entry
                                            SupplierExp += L_Separate_Expenses_Without_Tax_Amount;
                                            string Exp_Narration = string.Empty;
                                            if (txtAirwaybillno.Text != "")
                                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                            else
                                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                            if (IsNonRegistered)
                                            {
                                                string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                if (L_Separate_Expenses_Tax_Amount != 0)
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                                if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                {
                                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                }
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                {
                                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                                F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                                C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                                F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                                C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                            }
                                            //For Credit Entry                                
                                            string Exp_Narration = string.Empty;
                                            if (txtAirwaybillno.Text != "")
                                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                            else
                                                Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                            if (strAccountId.Split(',').Contains(strAccountNo))
                                            {
                                                if (IsNonRegistered)
                                                {
                                                    string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (L_Separate_Expenses_Tax_Amount != 0)
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                    {
                                                        Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                    }
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                    {
                                                        Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (IsNonRegistered)
                                                {
                                                    string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (L_Separate_Expenses_Tax_Amount != 0)
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                    {
                                                        Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                    }
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                                    {
                                                        Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //End Code
                                    // Insert Expenses Entry
                                    //------------------------------------------------------------------------------------------------------------------------
                                    //------------------------------------------------------------------------------------------------------------------------
                                    //Start Code
                                    //Payment Entries In Voucher
                                    double Cash = 0;
                                    double AGeingFrnAmt = 0;
                                    string strPaymentType = string.Empty;
                                    DataTable dtPaymentTran = ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
                                    if (dtPaymentTran.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtPaymentTran.Rows.Count; i++)
                                        {
                                            string strSupplierAcc = "0";
                                            if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                            {
                                                //Check account exist or not in selected currency - Neelkanth Purohit 24-08-2018
                                                strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dtPaymentTran.Rows[i]["PayCurrencyId"].ToString()).ToString();
                                                if (strSupplierAcc == "0")
                                                {
                                                    throw new System.ArgumentException("Account No not exist for this invoice currency , first create it");
                                                }
                                            }
                                            strPaymentType = dtPaymentTran.Rows[i]["PaymentType"].ToString();
                                            string strPayAmount = dtPaymentTran.Rows[i]["Pay_Charges"].ToString();
                                            if (strPaymentType == "Cash" || strPaymentType == "Card")
                                            {
                                                if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                                {
                                                    string L_Debit_Amount = string.Empty;
                                                    string F_Debit_Amount = string.Empty;
                                                    string C_Debit_Amount = string.Empty;
                                                    if (IsNonRegistered)
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                                    }
                                                    else
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                                    }
                                                    if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                }
                                                else
                                                {
                                                    string L_Debit_Amount = string.Empty;
                                                    string F_Debit_Amount = string.Empty;
                                                    string C_Debit_Amount = string.Empty;
                                                    if (IsNonRegistered)
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                                    }
                                                    else
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                                    }
                                                    if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "False", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                }
                                                strPayTotal = (Convert.ToDouble(strPayTotal) + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString())).ToString();
                                                Cash = Cash + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString());
                                                AGeingFrnAmt = AGeingFrnAmt + Convert.ToDouble(dtPaymentTran.Rows[i]["FCPayAmount"].ToString());
                                            }
                                            else if (strPaymentType == "Credit")
                                            {
                                                if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                                {
                                                    string L_Debit_Amount = string.Empty;
                                                    string F_Debit_Amount = string.Empty;
                                                    string C_Debit_Amount = string.Empty;
                                                    if (IsNonRegistered)
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                                    }
                                                    else
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                                    }
                                                    if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                }
                                                else
                                                {
                                                    string L_Debit_Amount = string.Empty;
                                                    string F_Debit_Amount = string.Empty;
                                                    string C_Debit_Amount = string.Empty;
                                                    if (IsNonRegistered)
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                                    }
                                                    else
                                                    {
                                                        L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                        F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                        C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                                    }
                                                    if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                    else
                                                    {
                                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    DataTable newcheck = objVoucherDetail.GetSumRecordByVoucherNo(strMaxId, ref trns);
                                    if (newcheck.Rows.Count > 0)
                                    {
                                        string strRoundCurrencyId = Session["LocCurrencyId"].ToString();
                                        double DebitTotal = Convert.ToDouble(newcheck.Rows[0]["DebitTotal"].ToString());
                                        double CreditTotal = Convert.ToDouble(newcheck.Rows[0]["CreditTotal"].ToString());
                                        if (DebitTotal == CreditTotal)
                                        {

                                        }
                                        else
                                        {
                                            if (DebitTotal > CreditTotal)
                                            {
                                                double diff = DebitTotal - CreditTotal;
                                                string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                                string CompanyCurrRoundCredit = strRoundCr.Trim().Split('/')[0].ToString();
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", "0.00", diff.ToString(), "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundCredit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else if (CreditTotal > DebitTotal)
                                            {
                                                double diff = CreditTotal - DebitTotal;
                                                string strRoundDr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                                string CompanyCurrRoundDebit = strRoundDr.Trim().Split('/')[0].ToString();
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", diff.ToString(), "0.00", "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundDebit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                            }
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                        }
                        if (Session["VPost"].ToString() == "True")
                        {
                            DisplayMessage("Invoice has been Posted");
                        }
                        else
                        {
                            DisplayMessage("Record Saved", "green");
                        }
                        //here we commit transaction when all data insert and update proper 
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();


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



                    //Added Transfer In Finance Code On 01-03-2023
                    if (Session["VPost"].ToString() == "True")
                    {
                        SqlConnection connew = new SqlConnection(Session["DBConnection"].ToString());
                        connew.Open();
                        SqlTransaction trnsnew;
                        trnsnew = connew.BeginTransaction();

                        bool strTransferInFinancePost = false;
                        DataTable dtAcParameterLocationPost = objAccParameterLocation.GetParameterMasterAllTrue(StrCompId, StrBrandId, StrLocationId, ref trnsnew);
                        DataTable dtTransferInFinancePost = new DataView(dtAcParameterLocationPost, "Param_Name='Allow Transfer In Finance'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTransferInFinancePost.Rows.Count > 0)
                        {
                            strTransferInFinancePost = Convert.ToBoolean(dtTransferInFinancePost.Rows[0]["Param_Value"].ToString());
                        }
                        else
                        {
                            strTransferInFinancePost = true;
                        }

                        string strMaxIdPost = string.Empty;
                        DataTable dtMaxIdPost = objVoucherHeader.GetVoucherHeaderMaxId(ref trnsnew);
                        if (dtMaxIdPost.Rows.Count > 0)
                        {
                            strMaxIdPost = dtMaxIdPost.Rows[0][0].ToString();
                        }
                        if (strTransferInFinancePost == false)
                        {
                            if (strMaxIdPost != "" && strMaxIdPost != "0")
                            {
                                DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(strMaxIdPost, "1");
                                if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
                                {
                                    if (Dt_Sal_Loan.Rows.Count > 1)
                                    {
                                        foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                                        {
                                            if (Convert.ToBoolean(Dr_Loan["IsActive"].ToString()) == false)
                                            {
                                                if (Dr_Loan["Field5"].ToString() == "")
                                                {
                                                    DisplayMessage("Salary Voucher is Deleted, So it cannot be Post !");
                                                    return;
                                                }
                                                else
                                                {
                                                    DisplayMessage("Loan Voucher is Deleted, So it cannot be Post !");
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }



                            string strVoucherDetailNumber = string.Empty;
                            string strVoucherDetailNumberFYC = string.Empty;
                            string strCashflowPosted = string.Empty;

                            //for Detail Record Not Exists
                            DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                            if (dtVoucherHeader.Rows.Count > 0)
                            {
                                if (Common.IsFinancialyearAllow(Convert.ToDateTime(dtVoucherHeader.Rows[0]["Voucher_Date"].ToString()), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                                {
                                    DataTable dtVoucherDetail = objVoucherDetail.GetSumRecordByVoucherNo(strMaxIdPost);
                                    if (dtVoucherDetail.Rows.Count > 0)
                                    {

                                        double sumDebit = 0;
                                        double sumCredit = 0;
                                        try
                                        {
                                            sumDebit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["DebitTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                        }
                                        catch
                                        {

                                        }


                                        try
                                        {
                                            sumCredit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["CreditTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                        }
                                        catch
                                        {

                                        }





                                        if (sumDebit == sumCredit)
                                        {

                                        }
                                        else
                                        {
                                            if (strVoucherDetailNumber == "")
                                            {
                                                strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                            }
                                            else
                                            {
                                                strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (strVoucherDetailNumber == "")
                                        {
                                            strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                        }
                                        else
                                        {
                                            strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (strVoucherDetailNumberFYC == "")
                                    {
                                        strVoucherDetailNumberFYC = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                    else
                                    {
                                        strVoucherDetailNumberFYC = strVoucherDetailNumberFYC + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                }
                            }



                            if (strVoucherDetailNumberFYC != "")
                            {
                                DisplayMessage("Log In Financial year not allowing to perform this action Voucher Number is :- " + strVoucherDetailNumberFYC + "");
                                return;
                            }

                            //for Cash flow Posted
                            //For Cash flow Account

                            string strAccountIdNew = string.Empty;
                            DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount");
                            if (dtAccount.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAccount.Rows.Count; i++)
                                {
                                    if (strAccountIdNew == "")
                                    {
                                        strAccountIdNew = dtAccount.Rows[i]["Param_Value"].ToString();
                                    }
                                    else
                                    {
                                        strAccountIdNew = strAccountIdNew + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                strAccountIdNew = "0";
                            }



                            DataTable dtVoucherHeaderPost = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                            if (dtVoucherHeaderPost.Rows.Count > 0)
                            {
                                DataTable dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                                if (dtVoucherDetail.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtVoucherDetail.Rows.Count; i++)
                                    {

                                        if (strAccountIdNew.Split(',').Contains(dtVoucherDetail.Rows[i]["Account_No"].ToString()))
                                        {
                                            DataTable dtCashflowDetail = objCashFlowDetail.GetCashFlowDetailForAcountsEntry(StrCompId, StrBrandId, StrLocationId);
                                            if (dtCashflowDetail.Rows.Count > 0)
                                            {
                                                string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                                                if (strCashFinalDate != "")
                                                {
                                                    DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                                                    if (dtFinalDate >= DateTime.Parse(dtVoucherHeaderPost.Rows[0]["Voucher_Date"].ToString()))
                                                    {
                                                        if (strCashflowPosted == "")
                                                        {
                                                            strCashflowPosted = dtVoucherHeaderPost.Rows[0]["Voucher_No"].ToString();
                                                        }
                                                        else
                                                        {
                                                            strCashflowPosted = strCashflowPosted + "," + dtVoucherHeaderPost.Rows[0]["Voucher_No"].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (strCashflowPosted != "")
                            {
                                DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPosted + "");
                                return;
                            }

                            if (strVoucherDetailNumber != "")
                            {
                                DisplayMessage("Your Detail Record is Not Proper Please check that Records :- " + strVoucherDetailNumber + "");
                                return;
                            }

                            try
                            {

                                b = objVoucherHeader.UpdateVoucherReconciledFinance(StrCompId, StrBrandId, StrLocationId, strMaxIdPost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trnsnew);
                                if (b != 0)
                                {
                                    objAgeingDetail.insert_Ageing(StrCompId, StrBrandId, StrLocationId, Session["EmpId"].ToString(), Session["UserId"].ToString(), strMaxIdPost, trnsnew);
                                }


                                if (b != 0)
                                {

                                }

                                trnsnew.Commit();
                                if (connew.State == System.Data.ConnectionState.Open)
                                {
                                    connew.Close();
                                }
                                trnsnew.Dispose();
                                connew.Dispose();



                                //AllPageCode();
                                DisplayMessage("Record Posted Successfully");
                            }
                            catch (Exception ex)
                            {
                                DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
                                trnsnew.Rollback();
                                if (connew.State == System.Data.ConnectionState.Open)
                                {

                                    connew.Close();
                                }
                                trnsnew.Dispose();
                                connew.Dispose();
                                return;
                            }
                        }
                    }
                    //Ended Transfer In Finance Code On 01-03-2023


                    Reset();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    FillGrid();

                }
            }
        }
        else
        {
            //If Record is Not Posted
            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            if (sender is Button)
            {
                Button btnId = (Button)sender;
                if (btnId.ID == "btnPost")
                {
                    Session["VPost"] = "True";
                }
                if (btnId.ID == "btnSave")
                {
                    Session["VPost"] = "False";
                }
            }

            //For Finance Code
            //here we check that this page is updated by other user before save of current user 
            //this code is created by jitendra upadhyay on 15-05-2015
            //code start
            if (HdnEdit.Value != "")
            {
                DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HdnEdit.Value);
                if (dt.Rows.Count != 0)
                {
                    if (ViewState["TimeStamp"].ToString() != dt.Rows[0]["Row_Lock_Id"].ToString())
                    {
                        DisplayMessage("Another User update Information reload and try again");
                        return;
                    }
                }
            }
            //code end
            string strReceivedQty = string.Empty;
            if (txtInvoicedate.Text == "")
            {
                DisplayMessage("Enter Invoice Date");
                txtInvoicedate.Focus();
                return;
            }
            else
            {
                try
                {
                    ObjSysParam.getDateForInput(txtInvoicedate.Text);
                }
                catch
                {
                    DisplayMessage("Enter Invoice Date in format " + Session["DateFormat"].ToString() + "");
                    txtInvoicedate.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoicedate);
                    return;
                }
            }
            //code added by jitendra upadhyay on 09-12-2016
            //for insert record according the log in financial year
            if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtInvoicedate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {
                DisplayMessage("Log In Financial year not allowing to perform this action");
                return;
            }
            if (txtSupInvoiceDate.Text == "")
            {
                DisplayMessage("Select Supplier Invoice Date");
                txtSupInvoiceDate.Focus();
                return;
            }
            else
            {
                try
                {
                    ObjSysParam.getDateForInput(txtSupInvoiceDate.Text);
                }
                catch
                {
                    DisplayMessage("Enter Supplier Invoice Date in format " + Session["DateFormat"].ToString() + "");
                    txtSupInvoiceDate.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupInvoiceDate);
                    return;
                }
            }
            if (txtInvoiceNo.Text == "")
            {
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Focus();
                DisplayMessage("Enter Invoice No");
                return;
            }
            //if (txtSupplierInvoiceNo.Text == "")
            //{
            //    txtSupplierInvoiceNo.Text = "";
            //    txtSupplierInvoiceNo.Focus();
            //    DisplayMessage("Enter Supplier Invoice No");
            //    return;
            //}
            if (txtSupplierName.Text == "")
            {
                txtSupplierName.Focus();
                DisplayMessage("Enter Supplier Name");
                return;
            }
            if (txtCostingRate.Text == "")
            {
                txtCostingRate.Text = "0";
            }
            if (txtOtherCharges.Text == "")
            {
                txtOtherCharges.Text = "0";
            }
            if (!RdoPo.Checked && !RdoWithOutPo.Checked)
            {
                DisplayMessage("Select One With Purchase Order Or WithOut Purchase Order");
                RdoPo.Focus();
                return;
            }
            if (gvProduct.Rows.Count == 0)
            {
                DisplayMessage("Select Product");
                return;
            }
            if (ddlCurrency.SelectedIndex == 0)
            {
                ddlCurrency.Focus();
                DisplayMessage("Currency Required On Company Level");
                return;
            }
            string strTotalQty = string.Empty;
            string strAmount = string.Empty;
            string PoId = string.Empty;
            bool Post = false;
            if (Session["VPost"].ToString() == "True")
            {
                DataTable dt = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value);
                if (dt.Rows.Count > 0)
                {
                    if (new DataView(dt, "RecQty<>'0'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        Post = true;
                    }
                    else
                    {
                        Session["VPost"] = "False";
                        Post = false;
                        DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
                        return;
                    }
                }
                else
                {
                    Session["VPost"] = "False";
                    Post = false;
                    DisplayMessage("You Cannot Post This Because You Not Received Any Quantity");
                    return;
                }
            }
            else if (Session["VPost"].ToString() == "False")
            {
                Post = false;
            }
            string POst = string.Empty;
            if (RdoPo.Checked)
            {
                POst = "PO";
            }
            if (RdoWithOutPo.Checked)
            {
                POst = "WOutPO";
            }
            if (txtExchangeRate.Text.Trim() == "")
            {
                txtExchangeRate.Text = "0";
            }
            if (txtCostingRate.Text == "")
            {
                txtCostingRate.Text = "0";
            }
            if (txtBillAmount.Text == "0")
            {
                txtBillAmount.Text = "0";
            }
            //this validation for check that payment amount is equal to invoice amount or not
            if (Session["VPost"].ToString() == "True")
            {
                if (txtPurchaseAccount.Text == "")
                {
                    DisplayMessage("Enter Purchase Account");
                    txtPurchaseAccount.Focus();
                    return;
                }
                if (gvPayment.Rows.Count == 0)
                {
                    Session["VPost"] = "False";
                    Post = false;
                    DisplayMessage("Enter Payment Mode Details");
                    return;
                }
            }
            //For Bank Account
            string strAccountId = txtPurchaseAccount.Text.Split('/')[1].ToString();
            //string strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            //added payment validation according the discussion with nafees sir on 27-06-2016
            //created by jitendra upadhyay 
            double Invoiceamt = 0;
            double Paymentamt = 0;
            double advancepayment = 0;
            if (txtGrandTotal.Text != "")
            {
                Invoiceamt = Convert.ToDouble(txtGrandTotal.Text);
            }
            if (gvPayment.Rows.Count > 0)
            {
                Paymentamt = Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text);
            }
            if (Invoiceamt != Paymentamt)
            {
                DisplayMessage("Payment Amount should be equal to invoice amount");
                return;
            }
            //for add confirmation when invoice quantity and recenve quantityis not same 
            if (txtCostingRate.Text.Trim() == "" || txtCostingRate.Text.Trim() == "Infinity")
            {
                txtCostingRate.Text = "0";
            }

            //Check controls Value from page setting
            string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
            if (result[0] == "false")
            {
                DisplayMessage(result[1]);
                return;
            }

            string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            strTotalQty = "0";
            strAmount = "0";
            int b = 0;
            string InvoiceId = "0";

            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                if (HdnEdit.Value == "")
                {
                    //start rollback transaction
                    // GetData();
                    b = ObjPurchaseInvoice.InsertPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                    InvoiceId = b.ToString();

                    if (txtInvoiceNo.Text == ViewState["DocNo"].ToString())
                    {

                        int invoicecount = ObjPurchaseInvoice.GetInvoiceCountByLocationId(Session["LocId"].ToString(), ref trns);

                        if (invoicecount == 0)
                        {
                            ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + "1", ref trns);
                            txtInvoiceNo.Text = txtInvoiceNo.Text + "1";
                        }
                        else
                        {
                            if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count > 0)
                            {

                                bool bCodeFlag = true;
                                while (bCodeFlag)
                                {
                                    invoicecount += 1;

                                    if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + txtInvoiceNo.Text + invoicecount.ToString() + "'", ref trns).Rows.Count == 0)
                                    {
                                        bCodeFlag = false;
                                    }

                                }

                            }
                            ObjPurchaseInvoice.Updatecode(b.ToString(), txtInvoiceNo.Text + invoicecount.ToString(), ref trns);
                            txtInvoiceNo.Text = txtInvoiceNo.Text + invoicecount.ToString();
                        }
                    }

                    DataTable dtExpense = new DataTable();
                    dtExpense = (DataTable)ViewState["Expdt"];
                    if (dtExpense != null)
                    {
                        try
                        {
                            string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                            ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, ddlCurrency.SelectedValue, txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
                        }
                        catch
                        {
                        }
                        try
                        {
                            foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                            {
                                //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, InvoiceId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        catch
                        {
                        }
                    }

                    DataTable dtPayment = new DataTable();
                    dtPayment = (DataTable)ViewState["PayementDt"];
                    if (dtPayment != null)
                    {
                        try
                        {
                            foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                            {
                                ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", InvoiceId.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        catch
                        {
                        }
                    }
                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", InvoiceId.ToString(), ref trns);
                    int SerialNo = 1;
                    foreach (GridViewRow gvr in gvProduct.Rows)
                    {
                        Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                        Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                        DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                        TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
                        Label lblPOId = (Label)gvr.FindControl("lblPOId");
                        Label OrderQty = (Label)gvr.FindControl("OrderQty");
                        TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
                        TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
                        Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
                        TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                        TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
                        Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
                        TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
                        TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
                        Label lblAmount = (Label)gvr.FindControl("lblAmount");
                        Label lblRecQty = (Label)gvr.FindControl("lblRecQty");
                        if (lblUnitRate.Text == "")
                        {
                            lblUnitRate.Text = "0";
                        }
                        if (lblTax.Text == "")
                        {
                            lblTax.Text = "0";
                        }
                        if (lblTaxValue.Text == "")
                        {
                            lblTaxValue.Text = Convert_Into_DF("0");
                        }
                        if (lblDiscount.Text == "")
                        {
                            lblDiscount.Text = "0";
                        }
                        if (lblDiscountValue.Text == "")
                        {
                            lblDiscountValue.Text = "0";
                        }
                        //if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), lblgvProductId.Text, Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "NS")
                        //{
                        //    lblRecQty.Text = QtyGoodReceived.Text.ToString();
                        //}
                        if (QtyGoodReceived.Text != "" || QtyGoodReceived.Text != "0")
                        {
                            int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), b.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            Insert_Tax("gvProduct", b.ToString(), Details_ID.ToString(), gvr, ref trns);
                            SerialNo++;
                            //this code is created by jitendra upadhya on 14-07-2015
                            //this code for insert record in tax ref detail table when we apply multiple tax according product category
                            //code start
                            foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                            {
                                if (Convert.ToDouble(lblAmount.Text) != 0)
                                    objTaxRefDetail.InsertRecord("PINV", InvoiceId, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        //code end
                    }
                    Tax_Insert_Into_Inv_TaxRefDetail(InvoiceId, ref trns);
                    //committ
                }
                else
                {
                    string strPOId = string.Empty;
                    string ShippingLine = string.Empty;
                    string shipUnit = string.Empty;
                    if (ddlShipUnit.SelectedIndex == 0)
                    {
                        shipUnit = "0";
                    }
                    else
                    {
                        shipUnit = ddlShipUnit.SelectedValue;
                    }
                    if (txtTotalWeight.Text == "")
                    {
                        txtTotalWeight.Text = "0";
                    }
                    if (txtUnitRate.Text == "")
                    {
                        txtUnitRate.Text = "0";
                    }
                    if (txtShippingLine.Text != "")
                    {
                        ShippingLine = txtShippingLine.Text.Split('/')[1].ToString();
                    }
                    if (txtShippingDate.Text.Trim() != "")
                    {
                        try
                        {
                            Convert.ToDateTime(txtShippingDate.Text);
                        }
                        catch
                        {
                            DisplayMessage("Enter Valid Shipping Date");
                            txtShippingDate.Focus();
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
                    else
                    {
                        DisplayMessage("Enter Shipping Date");
                        txtShippingDate.Focus();
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    if (txtReceivingDate.Text.Trim() != "")
                    {
                        try
                        {
                            Convert.ToDateTime(txtReceivingDate.Text);
                        }
                        catch
                        {
                            DisplayMessage("Enter Valid Receiving Date");
                            txtReceivingDate.Focus();
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
                    else
                    {
                        DisplayMessage("Enter Receiving Date");
                        txtReceivingDate.Focus();
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    // GetData();
                    b = ObjPurchaseInvoice.UpdatePurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim(), txtInvoiceNo.Text.Trim(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), "0", ObjSysParam.getDateForInput(txtSupInvoiceDate.Text).ToString(), txtSupplierInvoiceNo.Text.Trim(), ddlInvoiceType.SelectedValue.ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtOtherCharges.Text.Trim(), strAmount.ToString(), strTotalQty.ToString(), txtNetTaxPar.Text.Trim(), Convert_Into_DF(txtNetTaxVal.Text.Trim()), txtNetAmount.Text.Trim(), txtNetDisPer.Text.Trim(), txtNetDisVal.Text.Trim(), txtGrandTotal.Text.Trim(), Post.ToString(), txRemark.Text.Trim(), POst, txtBillAmount.Text.Trim(), ddlLocation.SelectedValue, ddlBillType.SelectedValue, "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                    //update shipping information 
                    //delete and reinsert 
                    string sql = "delete from dbo.Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + HdnEdit.Value.Trim() + "";
                    objDa.execute_Command(sql, ref trns);
                    sql = "INSERT INTO [Inv_InvoiceShippingHeader]([Ref_Type],[Ref_Id] ,[Shipping_Line],[Ship_By] ,[Airway_Bill_No],[Ship_Unit],[Actual_Weight],[Volumetric_weight],[Shipping_Date],[Receiving_date],[Shipment_Type],[Freight_Status],[UnitRate],[Divide_By])VALUES('PINV'," + HdnEdit.Value.Trim() + ",'" + ShippingLine.Trim() + "','" + ddlShipBy.SelectedValue.Trim() + "','" + txtAirwaybillno.Text.Trim() + "','" + shipUnit.Trim() + "','" + txtTotalWeight.Text.Trim() + "','" + txtvolumetricweight.Text.Trim() + "','" + ObjSysParam.getDateForInput(txtShippingDate.Text).ToString().Trim() + "','" + ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString().Trim() + "','" + ddlShipmentType.SelectedValue.Trim() + "','" + ddlFreightStatus.SelectedValue.Trim() + "','" + txtUnitRate.Text + "','" + txtdivideby.Text.Trim() + "')";
                    objDa.execute_Command(sql, ref trns);
                    DataTable dtExpense = new DataTable();
                    dtExpense = (DataTable)ViewState["Expdt"];
                    if (dtExpense != null)
                    {
                        try
                        {
                            ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, "PI", ref trns);
                            string strFooterVal = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                            ObjShipExpHeader.ShipExpHeader_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.Trim().ToString(), ddlCurrency.SelectedValue.ToString(), txtExchangeRate.Text.Trim(), txtCostingRate.Text.Trim(), txtGrandTotal.Text.Trim(), strFooterVal, "PI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        catch
                        {

                        }
                        try
                        {
                            ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.ToString(), "PI", ref trns);
                            foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                            {
                                //ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PI", dr["Debit_Account_No"].ToString(), "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), string.IsNullOrEmpty(dr["FCExpAmount"].ToString()) ? "0" : dr["FCExpAmount"].ToString(), "PI", "", dr["field3"].ToString(), dr["field4"].ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    DataTable dtPayment = new DataTable();
                    dtPayment = (DataTable)ViewState["PayementDt"];
                    if (dtPayment != null)
                    {
                        try
                        {
                            ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
                            foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                            {
                                ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PI", HdnEdit.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), txtPurchaseAccount.Text.Trim().Split('/')[1].ToString(), Txt_Narration.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        catch
                        {
                        }
                    }
                    //here we update receive qty in one table 
                    //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
                    //by jitendra upadhyay 
                    //on 22-02-2016
                    //code start
                    DataTable dtInvDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
                    //code end
                    ObjPurchaseInvoiceDetail.DeletePurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ref trns);
                    int SerialNo = 1;
                    CostingRate();
                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PINV", HdnEdit.Value.Trim(), ref trns);
                    foreach (GridViewRow gvr in gvProduct.Rows)
                    {
                        Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                        Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                        DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                        TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
                        Label lblPOId = (Label)gvr.FindControl("lblPOId");
                        strPOId = lblPOId.Text;
                        Label OrderQty = (Label)gvr.FindControl("OrderQty");
                        TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
                        Label QtyReceived = (Label)gvr.FindControl("QtyReceived");
                        TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
                        Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
                        TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                        TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
                        Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
                        TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
                        TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
                        Label lblAmount = (Label)gvr.FindControl("lblAmount");
                        Label lblRecQty = (Label)gvr.FindControl("lblRecQty");
                        Label lblExpiryDate = (Label)gvr.FindControl("lblgvExpiryDate");
                        Label lblBatchNo = (Label)gvr.FindControl("lblgvBatchNo");

                        string UnitCost = (Convert.ToDouble(((Convert.ToDouble(lblUnitRate.Text) + Convert.ToDouble(Convert_Into_DF(lblTaxValue.Text))) - Convert.ToDouble(lblDiscountValue.Text)).ToString()) * Convert.ToDouble(txtCostingRate.Text)).ToString();
                        if (lblExpiryDate.Text == "")
                        {
                            lblExpiryDate.Text = "1/1/1800";
                        }
                        if (lblUnitRate.Text == "")
                        {
                            lblUnitRate.Text = "0";
                        }
                        if (QtyGoodReceived.Text == "")
                        {
                            QtyGoodReceived.Text = "0";
                        }
                        if (lblTax.Text == "")
                        {
                            lblTax.Text = "0";
                        }
                        if (lblTaxValue.Text == "")
                        {
                            lblTaxValue.Text = Convert_Into_DF("0");
                        }
                        if (lblDiscount.Text == "")
                        {
                            lblDiscount.Text = "0";
                        }
                        if (lblDiscountValue.Text == "")
                        {
                            lblDiscountValue.Text = "0";
                        }


                        //if (lblRecQty.Text == "")
                        //{
                        //    lblRecQty.Text = "0";
                        //}
                        //this code is created for solve issue when we goods receive in new tab but detail grid not refresh so it was save 0 so 
                        //by jitendra upadhyay 
                        //on 20-02-2016
                        //code start
                        //string strsql = "select RecQty from Inv_PurchaseInvoiceDetail where InvoiceNo=" + HdnEdit.Value.ToString() + " and ProductId=" + lblgvProductId.Text + "";
                        //DataTable dtRecqty= objDa.return_DataTable(strsql,ref trns);
                        //if (dtRecqty.Rows.Count > 0)
                        //{
                        DataTable dtTemp = new DataView(dtInvDetail, "ProductId=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTemp.Rows.Count > 0)
                        {
                            if (dtTemp.Rows.Count == 1)
                            {
                                lblRecQty.Text = dtTemp.Rows[0]["RecQty"].ToString();
                            }
                        }

                        int Details_ID = ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), SerialNo.ToString(), lblgvProductId.Text.ToString(), lblPOId.Text.Trim(), ddlUnitName.SelectedValue, lblUnitRate.Text.ToString(), OrderQty.Text.ToString(), lblFreeQty.Text.ToString(), QtyGoodReceived.Text.ToString(), lblRecQty.Text.Trim(), lblDiscount.Text.ToString(), lblDiscountValue.Text.ToString(), lblTax.Text.ToString(), Convert_Into_DF(lblTaxValue.Text.ToString()), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax("gvProduct", HdnEdit.Value.ToString(), Details_ID.ToString(), gvr, ref trns);
                        //cpde start
                        if (Session["VPost"].ToString() == "True")
                        {
                            //Check the Batch Number and Expiry Date Value.
                            string strMaintainStock = "";
                            string strProductCode = "";
                            DataTable dtItemDetail = objDa.return_DataTable("Select * from Inv_ProductMaster where ProductId='" + lblgvProductId.Text + "'");
                            if (dtItemDetail.Rows.Count > 0)
                            {
                                strMaintainStock = dtItemDetail.Rows[0]["MaintainStock"].ToString();
                                strProductCode = dtItemDetail.Rows[0]["ProductCode"].ToString();
                                if (strMaintainStock == "BatchNo")
                                {
                                    if (lblBatchNo.Text == "")
                                    {
                                        DisplayMessage("Please Enter the Batch Number for Maintain Stock");
                                        txtReceivingDate.Focus();
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
                                else if (strMaintainStock == "Expiry")
                                {
                                    if (lblExpiryDate.Text == "")
                                    {
                                        DisplayMessage("Please Enter the Expiry Date for Maintain Stock");
                                        txtReceivingDate.Focus();
                                        trns.Rollback();
                                        if (con.State == System.Data.ConnectionState.Open)
                                        {
                                            con.Close();
                                        }
                                        trns.Dispose();
                                        con.Dispose();
                                        return;
                                    }
                                    if (lblBatchNo.Text == "")
                                    {
                                        DisplayMessage("Please Enter the Batch No for Maintain Stock");
                                        txtReceivingDate.Focus();
                                        trns.Rollback();
                                        if (con.State == System.Data.ConnectionState.Open)
                                        {
                                            con.Close();
                                        }
                                        trns.Dispose();
                                        con.Dispose();
                                        return;
                                    }
                                    if (lblExpiryDate.Text == "1/1/1800")
                                    {
                                        DisplayMessage("Please Enter the Expiry Date for Maintain Stock");
                                        txtReceivingDate.Focus();
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

                            string Qty = objDa.get_SingleValue("Select  Coversion_Qty from Inv_UnitMaster where unit_Id = '" + ddlUnitName.SelectedValue + "'");
                            float Unit_Cost = float.Parse(UnitCost) / float.Parse(Qty);

                            ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", lblBatchNo.Text, lblRecQty.Text.Trim(), "0", "1/1/1800", Unit_Cost.ToString(), lblExpiryDate.Text, "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                            //Entry for Stock Batch Master Table
                            if (strMaintainStock == "Expiry")
                            {

                                if (lblRecQty.Text != "" && lblRecQty.Text != "0")
                                {
                                    //Code for Create Barcode
                                    string strBarcode = "";
                                    if (lblExpiryDate.Text != "" && lblBatchNo.Text != "")
                                    {
                                        string formattedDate = DateTime.Parse(lblExpiryDate.Text).ToString("ddMMyyyy");
                                        strBarcode = formattedDate + "/" + strProductCode;
                                    }


                                    ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "", lblBatchNo.Text, lblRecQty.Text, lblExpiryDate.Text, "", "1/1/1800", "", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);

                                }
                            }

                            //ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", HdnEdit.Value, "0", lblgvProductId.Text, ddlUnitName.SelectedValue, "I", "0", "0", lblRecQty.Text.Trim(), "0", "1/1/1800", UnitCost.ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), ObjSysParam.getDateForInput(txtInvoicedate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        //}
                        //code end
                        SerialNo++;
                        //this code is created by jitendra upadhya on 14-07-2015
                        //this code for insert record in tax ref detail table when we apply multiple tax according product category
                        //code start
                        foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                        {
                            if (Convert.ToDouble(lblAmount.Text) != 0)
                                objTaxRefDetail.InsertRecord("PINV", HdnEdit.Value, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblPOId.Text, lblgvProductId.Text.ToString(), ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        //code end
                    }
                    Tax_Insert_Into_Inv_TaxRefDetail(HdnEdit.Value, ref trns);
                    //Add Post Value Posted for Account Section On 31-03-2014 By Lokesh
                    if (Post == true)
                    {
                        string strRefrenceNumber = string.Empty;
                        if (HdnEdit.Value != "")
                        {
                            foreach (GridViewRow gvrow in gvProduct.Rows)
                            {
                                string SalesPrice = string.Empty;
                                Label lblUnitRateLocal = (Label)gvrow.FindControl("lblUnitRateLocal");
                                Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                                if (lblUnitRateLocal.Text == "")
                                {
                                    lblUnitRateLocal.Text = "0";
                                }

                                SalesPrice = SetDecimal((Convert.ToDouble(lblUnitRateLocal.Text) * Convert.ToDouble(ViewState["ExchangeRateinComp"].ToString())).ToString());
                                DataTable dtcustomerPriceList = objContactPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S", ref trns);
                                try
                                {
                                    dtcustomerPriceList = new DataView(dtcustomerPriceList, "Product_Id=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                catch
                                {

                                }

                                if (dtcustomerPriceList.Rows.Count == 0)
                                {
                                    objContactPriceList.InsertContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                                else
                                {
                                    objContactPriceList.UpdateContact_PriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), lblgvProductId.Text, "S", SalesPrice, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                            //Commented Code Start
                            strRefrenceNumber = txtSupplierInvoiceNo.Text;


                            string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                            DataTable dtAcParameterLocation = objAccParameterLocation.GetParameterMasterAllTrue(StrCompId, StrBrandId, StrLocationId, ref trns);

                            bool strTransferInFinance = false;
                            DataTable dtTransferInFinance = new DataView(dtAcParameterLocation, "Param_Name='Allow Transfer In Finance'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTransferInFinance.Rows.Count > 0)
                            {
                                strTransferInFinance = Convert.ToBoolean(dtTransferInFinance.Rows[0]["Param_Value"].ToString());
                            }
                            else
                            {
                                strTransferInFinance = true;
                            }


                            if (strTransferInFinance == true)
                            {
                                objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, txtInvoicedate.Text, "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            else if (strTransferInFinance == false)
                            {
                                objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, HdnEdit.Value, "PINV", txtInvoiceNo.Text, txtInvoicedate.Text, txtInvoiceNo.Text, txtInvoicedate.Text, "PI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "From PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", false.ToString(), false.ToString(), true.ToString(), "AV", "", "", txtSupplierInvoiceNo.Text, "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }



                            string strMaxId = string.Empty;
                            DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                            if (dtMaxId.Rows.Count > 0)
                            {
                                strMaxId = dtMaxId.Rows[0][0].ToString();
                            }
                            int j = 0;
                            string strPayTotal = "0";
                            string strCashAccount = string.Empty;
                            string strCreditAccount = string.Empty;
                            //string strPaymentVoucherAcc = string.Empty;
                            string strRoundoffAccount = string.Empty;


                            DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId, ref trns);
                            DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtCash.Rows.Count > 0)
                            {
                                strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                            }
                            DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtCredit.Rows.Count > 0)
                            {
                                strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
                            }
                            //DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                            //if (dtPaymentVoucher.Rows.Count > 0)
                            //{
                            //    strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
                            //}
                            DataTable dtRoundoff = new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtRoundoff.Rows.Count > 0)
                            {
                                strRoundoffAccount = dtRoundoff.Rows[0]["Param_Value"].ToString();
                            }
                            bool IsNonRegistered = false;
                            DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, txtSupplierName.Text.Trim().Split('/')[1].ToString());
                            if (dtSup.Rows.Count > 0)
                            {
                                if (!String.IsNullOrEmpty(dtSup.Rows[0]["Field2"].ToString()))
                                    IsNonRegistered = Convert.ToBoolean(dtSup.Rows[0]["Field2"].ToString());
                            }
                            //For Advance Debit Account
                            string strAdvanceDebitAC = string.Empty;
                            DataTable dtAdvanceDebitAC = new DataView(dtAcParameter, "Param_Name='PO Advance Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtAdvanceDebitAC.Rows.Count > 0)
                            {
                                strAdvanceDebitAC = dtAdvanceDebitAC.Rows[0]["Param_Value"].ToString();
                            }


                            double fOrderExp = 0;
                            //Commented On 28-02-2023
                            //foreach (GridViewRow gvr in gvOrderExpenses.Rows)
                            //{
                            //    Label lblExpCharge = (Label)gvr.FindControl("lblgvExp_Charges");
                            //    if (lblExpCharge.Text != "" && lblExpCharge.Text != "0")
                            //    {
                            //        fOrderExp = fOrderExp + Convert.ToDouble(lblExpCharge.Text);
                            //    }
                            //}



                            // Code By Ghanshyam Suthar on 11-04-2018
                            double Exchange_Rate = 0;
                            if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                            {
                                if (txtExchangeRate.Text.Trim() == "")
                                    Exchange_Rate = 1;
                                else
                                    Exchange_Rate = Convert.ToDouble(txtExchangeRate.Text.Trim());
                            }
                            else
                            {
                                Exchange_Rate = 1;
                            }

                            //---------------------------------------------------------------------------------------------------------------------
                            // For Location Currency
                            double L_Advance_Expenses_Amount = 0;
                            double L_Invoice_Without_Tax_Amount = 0;
                            double L_Invoice_Tax_Amount = 0;
                            double L_Invoice_With_Tax_Amount = 0;
                            double L_Total_Expenses_Without_Tax_Amount = 0;
                            double L_Total_Expenses_Tax_Amount = 0;
                            double L_Total_Expenses_With_Tax_Amount = 0;
                            double L_Separate_Expenses_Without_Tax_Amount = 0;
                            double L_Separate_Expenses_Tax_Amount = 0;
                            double L_Separate_Expenses_With_Tax_Amount = 0;
                            //---------------------------------------------------------------------------------------------------------------------
                            // For Foregin Currency
                            double F_Advance_Expenses_Amount = 0;
                            double F_Invoice_Without_Tax_Amount = 0;
                            double F_Invoice_Tax_Amount = 0;
                            double F_Invoice_With_Tax_Amount = 0;
                            double F_Total_Expenses_Without_Tax_Amount = 0;
                            double F_Total_Expenses_Tax_Amount = 0;
                            double F_Total_Expenses_With_Tax_Amount = 0;
                            double F_Separate_Expenses_Without_Tax_Amount = 0;
                            double F_Separate_Expenses_Tax_Amount = 0;
                            double F_Separate_Expenses_With_Tax_Amount = 0;
                            //---------------------------------------------------------------------------------------------------------------------
                            // For Company Currency
                            double C_Advance_Expenses_Amount = 0;
                            double C_Invoice_Without_Tax_Amount = 0;
                            double C_Invoice_Tax_Amount = 0;
                            double C_Invoice_With_Tax_Amount = 0;
                            double C_Total_Expenses_Without_Tax_Amount = 0;
                            double C_Total_Expenses_Tax_Amount = 0;
                            double C_Total_Expenses_With_Tax_Amount = 0;
                            double C_Separate_Expenses_Without_Tax_Amount = 0;
                            double C_Separate_Expenses_Tax_Amount = 0;
                            double C_Separate_Expenses_With_Tax_Amount = 0;
                            double Advance_Amount = 0;
                            double NetTaxVal = 0;
                            double GrandTotal = 0;
                            double Total_Expenses_Tax = 0;
                            double Total_Expenses = 0;
                            if (fOrderExp.ToString() != "")
                                Advance_Amount = fOrderExp;
                            if (txtNetTaxVal.Text.Trim() != "")
                                NetTaxVal = Convert.ToDouble(Convert_Into_DF(txtNetTaxVal.Text.Trim()));
                            if (txtGrandTotal.Text.Trim() != "")
                                GrandTotal = Convert.ToDouble(txtGrandTotal.Text.Trim());
                            if (GridExpenses.Rows.Count > 0)
                            {
                                if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text != "")
                                    Total_Expenses_Tax = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text);
                                if (((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text != "")
                                    Total_Expenses = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text);
                            }
                            //-------------------------------------------------------------------------------------------------------------------------------
                            // Location Currency
                            L_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal(Advance_Amount.ToString()));
                            L_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((NetTaxVal * Exchange_Rate).ToString()));
                            L_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((GrandTotal * Exchange_Rate).ToString()));
                            L_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount - L_Invoice_Tax_Amount).ToString()));
                            L_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses_Tax.ToString()));
                            L_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal(Total_Expenses.ToString()));
                            L_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount - L_Total_Expenses_Tax_Amount).ToString()));
                            L_Separate_Expenses_Without_Tax_Amount = 0;
                            L_Separate_Expenses_Tax_Amount = 0;
                            L_Separate_Expenses_With_Tax_Amount = 0;
                            //-------------------------------------------------------------------------------------------------------------------------------
                            // Foregin Currency
                            F_Advance_Expenses_Amount = Convert.ToDouble(SetDecimal((L_Advance_Expenses_Amount / Exchange_Rate).ToString()));
                            //F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_Tax_Amount / Exchange_Rate).ToString()));
                            F_Invoice_Tax_Amount = Convert.ToDouble(SetDecimal(NetTaxVal.ToString()));
                            //F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Invoice_With_Tax_Amount / Exchange_Rate).ToString()));
                            F_Invoice_With_Tax_Amount = Convert.ToDouble(SetDecimal(GrandTotal.ToString()));
                            F_Invoice_Without_Tax_Amount = Convert.ToDouble(SetDecimal((F_Invoice_With_Tax_Amount - F_Invoice_Tax_Amount).ToString()));
                            F_Total_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Tax_Amount / Exchange_Rate).ToString()));
                            F_Total_Expenses_With_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_With_Tax_Amount / Exchange_Rate).ToString()));
                            F_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Total_Expenses_Without_Tax_Amount / Exchange_Rate).ToString()));
                            F_Separate_Expenses_Without_Tax_Amount = 0;
                            F_Separate_Expenses_Tax_Amount = 0;
                            F_Separate_Expenses_With_Tax_Amount = 0;
                            //-------------------------------------------------------------------------------------------------------------------------------
                            // Company Currency
                            C_Advance_Expenses_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Advance_Expenses_Amount).ToString())))).Split('/')[0].ToString());
                            C_Invoice_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Invoice_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Invoice_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Invoice_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Total_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Total_Expenses_With_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_With_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Total_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Total_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                            C_Separate_Expenses_Without_Tax_Amount = 0;
                            C_Separate_Expenses_Tax_Amount = 0;
                            C_Separate_Expenses_With_Tax_Amount = 0;
                            //------------------------------------------------------------------------------------------------------------------------
                            //First Line Entry 
                            // Invoice_Without_Tax_Amount + Without_Expenses_Tax - Advance Amount
                            if (strAccountId.Split(',').Contains(Session["AccountId"].ToString()))
                            {
                                string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();

                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount + L_Total_Expenses_Without_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount + F_Total_Expenses_Without_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount + C_Total_Expenses_Without_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), Session["AccountId"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount, "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            //------------------------------------------------------------------------------------------------------------------------
                            // Debit Entry for Expenses when Supplier is Non-Registered
                            //Start Code
                            if (IsNonRegistered)
                            {
                                string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                if (ExpensesAccountId == "0")
                                {
                                    DisplayMessage("Please First set Expenses Account in Inventory parameter");
                                    trns.Rollback();
                                    return;
                                }
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Invoice_Tax_Amount.ToString(), "0.00", "PI Detail '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "'(Non registered purchase) " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Invoice_Tax_Amount.ToString(), C_Invoice_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            //End Code
                            //------------------------------------------------------------------------------------------------------------------------
                            // Entry for Taxes of Product
                            // Start Code
                            string TaxQuery = "Select * from Inv_TaxRefDetail where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' and Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
                            DataTable dtTaxDetails = objDa.return_DataTable(TaxQuery, ref trns);
                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                            {
                                string Product_Id = string.Empty;
                                string Tax_Id = string.Empty;
                                string Tax_Name = string.Empty;
                                string Tax_Value = string.Empty;
                                string Tax_Amount = string.Empty;
                                string TaxAccountNo = string.Empty;
                                string TaxAccountDetails = "Select * from Sys_TaxMaster where IsActive = 'true'";
                                DataTable dtTaxAccountDetails = objDa.return_DataTable(TaxAccountDetails);
                                if (dtTaxAccountDetails == null || dtTaxAccountDetails.Rows.Count == 0)
                                {
                                    DisplayMessage("Please Configure Account for Tax in Tax Master");
                                    trns.Rollback();
                                    return;
                                }
                                string TaxGrouping = "Select Tax_Id,Tax_Name,STM.Field3,SUM(CONVERT(decimal(18,2),Tax_value)) as TaxAmount from Inv_TaxRefDetail inner join Sys_TaxMaster STM on STM.Trans_Id = Tax_Id where (Expenses_Id is null or Expenses_Id='') and Ref_Type='PINV' AND Ref_Id = " + HdnEdit.Value + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 group by Tax_Id,Tax_Name,STM.Field3";
                                DataTable TaxTableGrouping = objDa.return_DataTable(TaxGrouping, ref trns);
                                string TaxIdInfo = string.Empty;
                                string GroupTaxId = string.Empty;
                                double GroupTaxAmount = 0;
                                string S_Tax_Percentage = string.Empty;
                                string GroupTaxName = string.Empty;
                                string strTaxPer = string.Empty;
                                foreach (DataRow grouprow in TaxTableGrouping.Rows)
                                {
                                    GroupTaxId = grouprow["Tax_Id"].ToString();
                                    GroupTaxAmount = Convert.ToDouble(grouprow["TaxAmount"].ToString());
                                    GroupTaxName = grouprow["Tax_Name"].ToString();
                                    TaxAccountNo = grouprow["Field3"].ToString();
                                    if (String.IsNullOrEmpty(TaxAccountNo))
                                    {
                                        DisplayMessage("Please Configure Account for Tax in Tax Master");
                                        trns.Rollback();
                                        return;
                                    }
                                    L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((GroupTaxAmount * Exchange_Rate).ToString()));
                                    F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Exchange_Rate).ToString()));
                                    C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                    S_Tax_Percentage = string.Empty;
                                    foreach (DataRow row in dtTaxDetails.Rows)
                                    {
                                        if (!TaxIdInfo.Contains(GroupTaxId))
                                        {
                                            DataView groupView = new DataView(dtTaxDetails);
                                            groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
                                            GroupTaxName = string.Empty;
                                            double N_Tax_Per = 0;
                                            foreach (DataRow newrow in groupView.ToTable().Rows)
                                            {
                                                N_Tax_Per = N_Tax_Per + Convert.ToDouble(newrow["Tax_Per"].ToString());
                                                S_Tax_Percentage = Convert.ToString(N_Tax_Per / Convert.ToDouble(groupView.ToTable().Rows.Count));
                                            }
                                            if (IsNonRegistered)
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Tax_Amount.ToString(), S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "' Non registered purchase", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            else
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), TaxAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            if (String.IsNullOrEmpty(TaxIdInfo))
                                                TaxIdInfo = GroupTaxId;
                                            else
                                                TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
                                            break;
                                        }
                                    }
                                }
                            }
                            //End Code
                            //------------------------------------------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            //Start Code
                            // Insert Expenses Entry
                            double Expenses_Tax = 0;
                            string[,] Net_Expenses_Tax = new string[1, 5];
                            double Exp = 0;
                            double SupplierExp = 0;
                            foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                            {
                                string strExpensesName = dr["Exp_Name"].ToString();
                                string strForeignAmount = dr["FCExpAmount"].ToString();
                                string strExpensesId = dr["Expense_Id"].ToString();
                                double strExpAmount = Convert.ToDouble(dr["Exp_Charges"].ToString());
                                string strAccountNo = dr["Account_No"].ToString();
                                string strExpCurrencyId = dr["ExpCurrencyID"].ToString();
                                string strExchangeRate = dr["ExpExchangeRate"].ToString();
                                Exp = Convert.ToDouble(SetDecimal((Exp + Convert.ToDouble(dr["Exp_Charges"].ToString())).ToString()));
                                if (strExpensesName == "")
                                {
                                    strExpensesName = GetExpName(strExpensesId);
                                }
                                Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax_Post(strExpAmount.ToString()));
                                double Expenses_Exchange_Rate = 0;
                                if (Session["LocCurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
                                {
                                    if (strExchangeRate.Trim() == "")
                                        Expenses_Exchange_Rate = 1;
                                    else
                                        Expenses_Exchange_Rate = Convert.ToDouble(SetDecimal(strExchangeRate.Trim()));
                                }
                                else
                                {
                                    Expenses_Exchange_Rate = 1;
                                }
                                L_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((Expenses_Tax).ToString()));
                                F_Separate_Expenses_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                                C_Separate_Expenses_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                L_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((strExpAmount).ToString()));
                                F_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(SetDecimal((L_Separate_Expenses_Without_Tax_Amount / Expenses_Exchange_Rate).ToString()));
                                C_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Separate_Expenses_Without_Tax_Amount).ToString())))).Split('/')[0].ToString());
                                if (strAccountNo == strPaymentVoucherAcc)
                                {
                                    //Check account exist or not in selected currency - Neelkanth Purohit -24-08-2018
                                    string strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dr["ExpCurrencyId"].ToString()).ToString();
                                    if (strSupplierAcc == "0")
                                    {
                                        throw new System.ArgumentException("Account No not exist for this expenses currency , first create it");
                                    }
                                    string L_Debit_Amount = string.Empty;
                                    string F_Debit_Amount = string.Empty;
                                    string C_Debit_Amount = string.Empty;
                                    if (IsNonRegistered)
                                    {
                                        L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                        F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                        C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                    }
                                    else
                                    {
                                        L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                        F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                        C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                    }
                                    //For Debit Entry
                                    SupplierExp += L_Separate_Expenses_Without_Tax_Amount;
                                    string Exp_Narration = string.Empty;
                                    if (txtAirwaybillno.Text != "")
                                        Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                    else
                                        Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                    if (IsNonRegistered)
                                    {
                                        string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                        if (L_Separate_Expenses_Tax_Amount != 0)
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                        }
                                    }
                                    else
                                    {
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                        if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                        {
                                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                        }
                                    }
                                }
                                else
                                {
                                    string L_Debit_Amount = string.Empty;
                                    string F_Debit_Amount = string.Empty;
                                    string C_Debit_Amount = string.Empty;
                                    if (IsNonRegistered)
                                    {
                                        L_Debit_Amount = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                        F_Debit_Amount = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                        C_Debit_Amount = (C_Separate_Expenses_Without_Tax_Amount).ToString();
                                    }
                                    else
                                    {
                                        L_Debit_Amount = (L_Separate_Expenses_Tax_Amount + L_Separate_Expenses_Without_Tax_Amount).ToString();
                                        F_Debit_Amount = (F_Separate_Expenses_Tax_Amount + F_Separate_Expenses_Without_Tax_Amount).ToString();
                                        C_Debit_Amount = (C_Separate_Expenses_Tax_Amount + C_Separate_Expenses_Without_Tax_Amount).ToString();
                                    }
                                    //For Credit Entry                                
                                    string Exp_Narration = string.Empty;
                                    if (txtAirwaybillno.Text != "")
                                        Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "', AWB No. '" + txtAirwaybillno.Text + "'";
                                    else
                                        Exp_Narration = strExpensesName + " Expenses Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' From '" + txtSupplierName.Text + "'";
                                    if (strAccountId.Split(',').Contains(strAccountNo))
                                    {
                                        if (IsNonRegistered)
                                        {
                                            string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            if (L_Separate_Expenses_Tax_Amount != 0)
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                            {
                                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                            }
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                            {
                                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsNonRegistered)
                                        {
                                            string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            if (L_Separate_Expenses_Tax_Amount != 0)
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), ExpensesAccountId, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", L_Separate_Expenses_Tax_Amount.ToString(), "0.00", Exp_Narration + "'(Non registered purchase) ", "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Separate_Expenses_Tax_Amount.ToString(), C_Separate_Expenses_Tax_Amount.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                            {
                                                Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", (L_Separate_Expenses_Without_Tax_Amount).ToString(), Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", (C_Separate_Expenses_Without_Tax_Amount).ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                            }
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            if (L_Separate_Expenses_Without_Tax_Amount != 0)
                                            {
                                                Net_Expenses_Tax = Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, (j++).ToString(), strAccountNo, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", (L_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", Exp_Narration, "", Session["EmpId"].ToString(), strExpCurrencyId, Expenses_Exchange_Rate.ToString(), (F_Separate_Expenses_Without_Tax_Amount).ToString(), (C_Separate_Expenses_Without_Tax_Amount).ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                            }
                                        }
                                    }
                                }
                            }
                            //End Code
                            // Insert Expenses Entry
                            //------------------------------------------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            //Start Code
                            //Payment Entries In Voucher
                            double Cash = 0;
                            double AGeingFrnAmt = 0;
                            string strPaymentType = string.Empty;
                            DataTable dtPaymentTran = ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "PI", HdnEdit.Value.ToString(), ref trns);
                            if (dtPaymentTran.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtPaymentTran.Rows.Count; i++)
                                {
                                    string strSupplierAcc = "0";
                                    if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                    {
                                        //Check account exist or not in selected currency - Neelkanth Purohit 24-08-2018
                                        strSupplierAcc = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), dtPaymentTran.Rows[i]["PayCurrencyId"].ToString()).ToString();
                                        if (strSupplierAcc == "0")
                                        {
                                            throw new System.ArgumentException("Account No not exist for this invoice currency , first create it");
                                        }
                                    }
                                    strPaymentType = dtPaymentTran.Rows[i]["PaymentType"].ToString();
                                    string strPayAmount = dtPaymentTran.Rows[i]["Pay_Charges"].ToString();
                                    if (strPaymentType == "Cash" || strPaymentType == "Card")
                                    {
                                        if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                        {
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                            }
                                            if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, C_Debit_Amount, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                        else
                                        {
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                            }
                                            if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "False", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                        strPayTotal = (Convert.ToDouble(strPayTotal) + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString())).ToString();
                                        Cash = Cash + Convert.ToDouble(dtPaymentTran.Rows[i]["Pay_Charges"].ToString());
                                        AGeingFrnAmt = AGeingFrnAmt + Convert.ToDouble(dtPaymentTran.Rows[i]["FCPayAmount"].ToString());
                                    }
                                    else if (strPaymentType == "Credit")
                                    {
                                        if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strPaymentVoucherAcc)
                                        {
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                            }
                                            if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strPaymentVoucherAcc, strSupplierAcc, HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), dtPaymentTran.Rows[i]["PayCurrencyID"].ToString(), Exchange_Rate.ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                        else
                                        {
                                            string L_Debit_Amount = string.Empty;
                                            string F_Debit_Amount = string.Empty;
                                            string C_Debit_Amount = string.Empty;
                                            if (IsNonRegistered)
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount - L_Invoice_Tax_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount - F_Invoice_Tax_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount - C_Invoice_Tax_Amount).ToString();
                                            }
                                            else
                                            {
                                                L_Debit_Amount = ((L_Invoice_With_Tax_Amount) - L_Advance_Expenses_Amount).ToString();
                                                F_Debit_Amount = ((F_Invoice_With_Tax_Amount) - F_Advance_Expenses_Amount).ToString();
                                                C_Debit_Amount = ((C_Invoice_With_Tax_Amount) - C_Advance_Expenses_Amount).ToString();
                                            }
                                            if (strAccountId.Split(',').Contains(dtPaymentTran.Rows[i]["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "False", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", HdnEdit.Value, "PINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), "0.00", L_Debit_Amount, "Payment On PI '" + txtInvoiceNo.Text.Trim() + "' Ref. Supplier Invoice No : '" + txtSupplierInvoiceNo.Text.Trim() + "' " + Txt_Narration.Text.Trim() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), "0.00", "0.00", C_Debit_Amount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                    }
                                }
                            }
                            DataTable newcheck = objVoucherDetail.GetSumRecordByVoucherNo(strMaxId, ref trns);
                            if (newcheck.Rows.Count > 0)
                            {
                                string strRoundCurrencyId = Session["LocCurrencyId"].ToString();
                                double DebitTotal = Convert.ToDouble(newcheck.Rows[0]["DebitTotal"].ToString());
                                double CreditTotal = Convert.ToDouble(newcheck.Rows[0]["CreditTotal"].ToString());
                                if (DebitTotal == CreditTotal)
                                {

                                }
                                else
                                {
                                    if (DebitTotal > CreditTotal)
                                    {
                                        double diff = DebitTotal - CreditTotal;
                                        string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                        string CompanyCurrRoundCredit = strRoundCr.Trim().Split('/')[0].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", "0.00", diff.ToString(), "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundCredit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else if (CreditTotal > DebitTotal)
                                    {
                                        double diff = CreditTotal - DebitTotal;
                                        string strRoundDr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
                                        string CompanyCurrRoundDebit = strRoundDr.Trim().Split('/')[0].ToString();
                                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "PINV", "1/1/1800", "1/1/1800", "0", diff.ToString(), "0.00", "Diffrence RoundOff On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundDebit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                if (Session["VPost"].ToString() == "True")
                {
                    DisplayMessage("Invoice has been Posted");
                }
                else
                {
                    DisplayMessage("Record Saved", "green");
                }
                //here we commit transaction when all data insert and update proper 
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();


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



            //Added Transfer In Finance Code On 01-03-2023
            if (Session["VPost"].ToString() == "True")
            {
                SqlConnection connew = new SqlConnection(Session["DBConnection"].ToString());
                connew.Open();
                SqlTransaction trnsnew;
                trnsnew = connew.BeginTransaction();

                bool strTransferInFinancePost = false;
                DataTable dtAcParameterLocationPost = objAccParameterLocation.GetParameterMasterAllTrue(StrCompId, StrBrandId, StrLocationId, ref trnsnew);
                DataTable dtTransferInFinancePost = new DataView(dtAcParameterLocationPost, "Param_Name='Allow Transfer In Finance'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTransferInFinancePost.Rows.Count > 0)
                {
                    strTransferInFinancePost = Convert.ToBoolean(dtTransferInFinancePost.Rows[0]["Param_Value"].ToString());
                }
                else
                {
                    strTransferInFinancePost = true;
                }

                string strMaxIdPost = string.Empty;
                DataTable dtMaxIdPost = objVoucherHeader.GetVoucherHeaderMaxId(ref trnsnew);
                if (dtMaxIdPost.Rows.Count > 0)
                {
                    strMaxIdPost = dtMaxIdPost.Rows[0][0].ToString();
                }
                if (strTransferInFinancePost == false)
                {
                    if (strMaxIdPost != "" && strMaxIdPost != "0")
                    {
                        DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(strMaxIdPost, "1");
                        if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
                        {
                            if (Dt_Sal_Loan.Rows.Count > 1)
                            {
                                foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                                {
                                    if (Convert.ToBoolean(Dr_Loan["IsActive"].ToString()) == false)
                                    {
                                        if (Dr_Loan["Field5"].ToString() == "")
                                        {
                                            DisplayMessage("Salary Voucher is Deleted, So it cannot be Post !");
                                            return;
                                        }
                                        else
                                        {
                                            DisplayMessage("Loan Voucher is Deleted, So it cannot be Post !");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }



                    string strVoucherDetailNumber = string.Empty;
                    string strVoucherDetailNumberFYC = string.Empty;
                    string strCashflowPosted = string.Empty;

                    //for Detail Record Not Exists
                    DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                    if (dtVoucherHeader.Rows.Count > 0)
                    {
                        if (Common.IsFinancialyearAllow(Convert.ToDateTime(dtVoucherHeader.Rows[0]["Voucher_Date"].ToString()), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                        {
                            DataTable dtVoucherDetail = objVoucherDetail.GetSumRecordByVoucherNo(strMaxIdPost);
                            if (dtVoucherDetail.Rows.Count > 0)
                            {

                                double sumDebit = 0;
                                double sumCredit = 0;
                                try
                                {
                                    sumDebit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["DebitTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                }
                                catch
                                {

                                }


                                try
                                {
                                    sumCredit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["CreditTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                }
                                catch
                                {

                                }





                                if (sumDebit == sumCredit)
                                {

                                }
                                else
                                {
                                    if (strVoucherDetailNumber == "")
                                    {
                                        strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                    else
                                    {
                                        strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (strVoucherDetailNumber == "")
                                {
                                    strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                }
                                else
                                {
                                    strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (strVoucherDetailNumberFYC == "")
                            {
                                strVoucherDetailNumberFYC = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                            }
                            else
                            {
                                strVoucherDetailNumberFYC = strVoucherDetailNumberFYC + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                            }
                        }
                    }



                    if (strVoucherDetailNumberFYC != "")
                    {
                        DisplayMessage("Log In Financial year not allowing to perform this action Voucher Number is :- " + strVoucherDetailNumberFYC + "");
                        return;
                    }

                    //for Cash flow Posted
                    //For Cash flow Account

                    string strAccountIdNew = string.Empty;
                    DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount");
                    if (dtAccount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAccount.Rows.Count; i++)
                        {
                            if (strAccountIdNew == "")
                            {
                                strAccountIdNew = dtAccount.Rows[i]["Param_Value"].ToString();
                            }
                            else
                            {
                                strAccountIdNew = strAccountIdNew + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                            }
                        }
                    }
                    else
                    {
                        strAccountIdNew = "0";
                    }



                    DataTable dtVoucherHeaderPost = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                    if (dtVoucherHeaderPost.Rows.Count > 0)
                    {
                        DataTable dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, StrLocationId, strMaxIdPost);
                        if (dtVoucherDetail.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtVoucherDetail.Rows.Count; i++)
                            {

                                if (strAccountIdNew.Split(',').Contains(dtVoucherDetail.Rows[i]["Account_No"].ToString()))
                                {
                                    DataTable dtCashflowDetail = objCashFlowDetail.GetCashFlowDetailForAcountsEntry(StrCompId, StrBrandId, StrLocationId);
                                    if (dtCashflowDetail.Rows.Count > 0)
                                    {
                                        string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                                        if (strCashFinalDate != "")
                                        {
                                            DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                                            if (dtFinalDate >= DateTime.Parse(dtVoucherHeaderPost.Rows[0]["Voucher_Date"].ToString()))
                                            {
                                                if (strCashflowPosted == "")
                                                {
                                                    strCashflowPosted = dtVoucherHeaderPost.Rows[0]["Voucher_No"].ToString();
                                                }
                                                else
                                                {
                                                    strCashflowPosted = strCashflowPosted + "," + dtVoucherHeaderPost.Rows[0]["Voucher_No"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (strCashflowPosted != "")
                    {
                        DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPosted + "");
                        return;
                    }

                    if (strVoucherDetailNumber != "")
                    {
                        DisplayMessage("Your Detail Record is Not Proper Please check that Records :- " + strVoucherDetailNumber + "");
                        return;
                    }

                    try
                    {

                        b = objVoucherHeader.UpdateVoucherReconciledFinance(StrCompId, StrBrandId, StrLocationId, strMaxIdPost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trnsnew);
                        if (b != 0)
                        {
                            objAgeingDetail.insert_Ageing(StrCompId, StrBrandId, StrLocationId, Session["EmpId"].ToString(), Session["UserId"].ToString(), strMaxIdPost, trnsnew);
                        }


                        if (b != 0)
                        {

                        }

                        trnsnew.Commit();
                        if (connew.State == System.Data.ConnectionState.Open)
                        {
                            connew.Close();
                        }
                        trnsnew.Dispose();
                        connew.Dispose();



                        //AllPageCode();
                        DisplayMessage("Record Posted Successfully");
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
                        trnsnew.Rollback();
                        if (connew.State == System.Data.ConnectionState.Open)
                        {

                            connew.Close();
                        }
                        trnsnew.Dispose();
                        connew.Dispose();
                        return;
                    }
                }
            }
            //Ended Transfer In Finance Code On 01-03-2023


            Reset();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            FillGrid();
        }
    }



    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = SetDecimal((Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (HdnEdit.Value == "")
        {
            string Id = ObjPurchaseInvoice.getAutoId();
            ObjPurchaseInvoiceDetail.DeletePurchaseInvoiceDetailByInvoiceNo(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString());
            ObjShipExpDetail.ShipExpDetail_Delete(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString(), "PI");
        }
        Reset();
        btnRefresh_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            try
            {
                txtSupplierName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                gvProduct.DataSource = null;
                gvProduct.DataBind();
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                ViewState["Dtproduct"] = null;
                ViewState["dtPo"] = null;
                return;
            }
            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                gvProduct.DataSource = null;
                gvProduct.DataBind();
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                ViewState["Dtproduct"] = null;
                ViewState["dtPo"] = null;
            }
            else
            {
                string strSupplierId = dt.Rows[0]["Trans_Id"].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, strSupplierId);
                    if (dtSup.Rows.Count > 0)
                    {
                        Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
                        bool IsNonRegistered = false;
                        if (!String.IsNullOrEmpty(dtSup.Rows[0]["Field2"].ToString()))
                            IsNonRegistered = Convert.ToBoolean(dtSup.Rows[0]["Field2"].ToString());
                    }
                }
                txtGrandTotal.Text = "0";
                if (gvProduct != null || gvProduct.Rows.Count > 0)
                {
                    if (gvProduct != null || gvProduct.Rows.Count > 0)
                    {
                        double d = 0;
                        double q = 0;
                        foreach (GridViewRow gvRow in gvProduct.Rows)
                        {
                            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
                            TextBox lblUnitRate = (TextBox)gvRow.FindControl("lblUnitRate");
                            TextBox TxtgvQty = (TextBox)gvRow.FindControl("QtyGoodReceived");
                            TextBox TxtgvDiscount = (TextBox)gvRow.FindControl("lblDiscount");
                            TextBox TxtgvDiscountValue = (TextBox)gvRow.FindControl("lblDiscountValue");
                            TextBox TxtgvTax = (TextBox)gvRow.FindControl("lblTax");
                            TextBox TxtgvTaxValue = (TextBox)gvRow.FindControl("lblTaxValue");
                            Label lblgvNetAmount = (Label)gvRow.FindControl("lblAmount");
                            Label lblQtyAmmount = (Label)gvRow.FindControl("lblQtyAmmount");
                            DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S");
                            try
                            {
                                dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + lblgvProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                            if (dtContactPriceList.Rows.Count > 0)
                            {
                                lblUnitRate.Text = dtContactPriceList.Rows[0]["Sales_Price"].ToString();
                                try
                                {
                                    lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                                    lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                                    lblUnitRate.Text = SetDecimal(lblUnitRate.Text);
                                }
                                catch
                                {
                                    lblUnitRate.Text = "0";
                                }
                                string[] taxVal = Common.TaxDiscountCaluculation(lblUnitRate.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                                lblQtyAmmount.Text = taxVal[0].ToString();
                                TxtgvTaxValue.Text = Convert_Into_DF(taxVal[2].ToString());
                                TxtgvDiscountValue.Text = taxVal[4].ToString();
                                lblgvNetAmount.Text = taxVal[5].ToString();
                            }
                            q += Convert.ToDouble(((TextBox)gvRow.FindControl("QtyGoodReceived")).Text.Trim());
                            d += Convert.ToDouble(((Label)gvRow.FindControl("lblQtyAmmount")).Text.Trim());
                        }
                        try
                        {
                            DirectGridFooterCalculation();
                        }
                        catch
                        {
                        }
                    }
                }
                if (HdnEdit.Value == "")
                {
                    PnlProductSearching.Visible = false;
                    btnAddProductScreen.Visible = false;
                    btnAddtoList.Visible = false;
                    RdoPo.Checked = false;
                    RdoWithOutPo.Checked = false;
                    ViewState["Dtproduct"] = null;
                    ViewState["dtPo"] = null;
                    gvSerachGrid.DataSource = null;
                    gvSerachGrid.DataBind();
                    RdoPo_CheckedChanged(null, null);
                }
            }
            setDecimal();
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Focus();
        }
    }


    protected void gvProduct_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            gvProduct.Columns[6].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
            gvProduct.Columns[7].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Cost, Session["DBConnection"].ToString());
            gvProduct.Columns[12].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
            gvProduct.Columns[13].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Cost, Session["DBConnection"].ToString());
            gvProduct.Columns[15].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
            gvProduct.Columns[17].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
            gvProduct.Columns[18].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Net_Price, Session["DBConnection"].ToString());
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Product_Description;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.ColumnSpan = 5;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);

            if (Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = Resources.Attendance.Discount;
                row.Controls.Add(cell);
            }
            else
            {
            }


            if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = Resources.Attendance.Tax;
                row.Controls.Add(cell);
            }

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Total;
            row.Controls.Add(cell);
            gvProduct.Controls[0].Controls.Add(row);
        }
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string PostStatus = string.Empty;
        if ((ddlFieldName.SelectedItem.Value == "InvoiceDate") || (ddlFieldName.SelectedItem.Value == "SupInvoiceDate"))
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtValue.Text = "";
                return;
            }
        }
        FillGrid();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Focus();
        FillGrid();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }
        txtSupplierName.Enabled = true;
        ddlTransType.Enabled = false;
        ddlCurrency.Enabled = false;
        txtExchangeRate.Enabled = false;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (objSenderID != "lnkViewDetail")
            {
                if (Convert.ToBoolean(dt.Rows[0]["Post"].ToString()))
                {
                    //{
                    //    DisplayMessage("This Purchase Invoice is posted , You cannot update");
                    //    return;
                    //}
                }
                if (dt.Rows[0]["Field5"].ToString().Trim() != "" && dt.Rows[0]["Field5"].ToString().Trim() != "0")
                {
                    DisplayMessage("Running bill adjusted , you cannot edit");
                    return;
                }
            }
            HdnEdit.Value = e.CommandArgument.ToString();
            DataTable dtRefDetailHeader = objTaxRefDetail.GetRecord_By_RefType_and_RefId("PINV", HdnEdit.Value);
            try
            {
                dtRefDetailHeader = new DataView(dtRefDetailHeader, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                Dt_Final_Save_Tax = null;
                Dt_Final_Save_Tax = new DataTable();
                Dt_Final_Save_Tax.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
                Dt_Final_Save_Tax = objTaxRefDetail.Get_Tax_Detail_For_Expenses("PINV", HdnEdit.Value, "Purchase_Invoice", "Multiple", "1");
                Session["Expenses_Tax_Purchase_Invoice"] = Dt_Final_Save_Tax;
            }
            LoadStores();
            txtInvoicedate.Text = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy");
            txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
            txtSupInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["SupInvoiceDate"].ToString()).ToString("dd-MMM-yyyy");
            txtSupplierInvoiceNo.Text = dt.Rows[0]["SupInvoiceNo"].ToString();
            try
            {
                ddlBillType.SelectedValue = dt.Rows[0]["Field4"].ToString();
            }
            catch
            {
            }
            ViewState["TimeStamp"] = dt.Rows[0]["Row_Lock_Id"].ToString();
            txtInvoiceNo.Enabled = false;
            ddlInvoiceType.SelectedValue = dt.Rows[0]["InvoiceType"].ToString();
            try
            {
                ddlLocation.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            catch
            {
                ddlLocation.SelectedValue = Session["LocId"].ToString();
            }
            try
            {
                ddlCurrency.SelectedValue = dt.Rows[0]["CurrencyId"].ToString();
                TxtCurrencyValue.Text = dt.Rows[0]["CurrencyId"].ToString();

            }
            catch
            {
            }
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            txtExchangeRate.Text = dt.Rows[0]["ExchangeRate"].ToString();
            if (Convert.ToBoolean(dt.Rows[0]["Post"].ToString()))
            {
                RdoPo.Enabled = false;
                RdoWithOutPo.Enabled = false;
                txtSupplierName.ReadOnly = true;
            }
            else
            {
                RdoPo.Enabled = true;
                RdoWithOutPo.Enabled = true;
                txtSupplierName.ReadOnly = false;
                btnPost.Visible = true;
            }
            txtSupplierName.Text = objSupplier.GetSupplierAllDataBySupplierId(StrCompId.ToString(), StrBrandId, dt.Rows[0]["SupplierId"].ToString()).Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["SupplierId"].ToString();
            DataTable dtRefDetail = objTaxRefDetail.GetRecord_By_RefType_and_RefId("PINV", HdnEdit.Value);
            try
            {
                dtRefDetail = new DataView(dtRefDetail, "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataTable DtDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.ToString());

            if (DtDetail.Rows.Count > 0)
            {
                if (dt.Rows[0]["Trans_Type"].ToString() != "")
                    ddlTransType.SelectedValue = dt.Rows[0]["Trans_Type"].ToString();
                else
                    ddlTransType.SelectedValue = "0";
            }
            else
            {
                ddlTransType.SelectedValue = "0";
            }
            Get_Tax_From_DB();
            txtBillAmount.Text = dt.Rows[0]["Field2"].ToString();
            if (dt.Rows[0]["Field1"].ToString() == "PO")
            {
                RdoPo.Checked = true;
                RdoWithOutPo.Checked = false;
                DataTable dtPurchaseOrder = fillPOSearhgrid();
                ViewState["dtPo"] = DtDetail;
                ViewState["Dtproduct"] = dtPurchaseOrder;
                gvSerachGrid.DataSource = dtPurchaseOrder;
                gvSerachGrid.DataBind();
                txtSupplierName.Enabled = false;
                //for showing advance payment
                //code start
                DataTable dtadvancepayment = DtDetail.Copy();
                dtadvancepayment = dtadvancepayment.DefaultView.ToTable(true, "POID", "PONo");
                fillOrderExpGrid(ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dtadvancepayment.Rows[0]["POID"].ToString(), "PO"));
                foreach (DataRow dr in dtadvancepayment.Rows)
                {
                    FillAdvancePayment_BYOrderId(dr["POID"].ToString(), dr["PONo"].ToString());
                }
                //code end
            }
            if (dt.Rows[0]["Field1"].ToString() == "WOutPO")
            {
                RdoPo.Checked = false;
                RdoWithOutPo.Checked = true;
                ViewState["dtPo"] = null;
                ViewState["Dtproduct"] = DtDetail;
                rbtNew.Visible = true;
                rbtEdit.Visible = true;
            }
            objPageCmn.FillData((object)gvProduct, DtDetail, "", "");
            TaxCalculationWithDiscount();
            DirectGridFooterCalculation();
            try
            {
                txtCostingRate.Text = dt.Rows[0]["CostingRate"].ToString();
            }
            catch
            {
                txtCostingRate.Text = "0";
            }
            fillExpGrid(ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), "PI"));
            RdoPOandWithPO();
            RdoPo.Enabled = false;
            RdoWithOutPo.Enabled = false;
            txtNetTaxPar.Text = SetDecimal(dt.Rows[0]["NetTax"].ToString());
            txtNetTaxVal.Text = Convert_Into_DF(dt.Rows[0]["NetTaxValue"].ToString());
            txtNetAmount.Text = SetDecimal(dt.Rows[0]["NetAmount"].ToString());
            txtNetDisPer.Text = SetDecimal(dt.Rows[0]["NetDiscount"].ToString());
            txtNetDisVal.Text = SetDecimal(dt.Rows[0]["NetDiscountValue"].ToString());
            txtGrandTotal.Text = SetDecimal(dt.Rows[0]["GrandTotal"].ToString());
            try
            {
                ddlPaymentMode.SelectedValue = dt.Rows[0]["PaymentModeID"].ToString();
            }
            catch
            {
            }
            CalculationchangeIntaxGridview();
            txRemark.Text = dt.Rows[0]["Remark"].ToString();
            txtOtherCharges.Text = SetDecimal(dt.Rows[0]["OtherCharges"].ToString());
            fillPaymentGrid(ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "PI", HdnEdit.Value.ToString()));
            CostingRate();
            gvProduct.Columns[16].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[17].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            GridExpenses.Columns[9].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            GridExpenses.Columns[10].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[14].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[15].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        GetSymbol();
        if (objSenderID == "lnkViewDetail")
        {
            btnSave.Enabled = false;
            btnPost.Enabled = false;
            btnReset.Visible = false;
        }
        else
        {
            btnSave.Enabled = true;
            btnPost.Enabled = true;
            btnReset.Visible = true;
        }
        setDecimal();
        //get shipping information
        using (DataTable dtshipInformation = objDa.return_DataTable("select * from Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + e.CommandArgument.ToString() + ""))
        {
            if (dtshipInformation.Rows.Count > 0)
            {
                try
                {
                    txtShippingLine.Text = ObjContactMaster.GetContactTrueById(dtshipInformation.Rows[0]["Shipping_Line"].ToString()).Rows[0]["Name"].ToString().Trim() + "/" + dtshipInformation.Rows[0]["Shipping_Line"].ToString().Trim();
                }
                catch
                {
                }
                ddlShipBy.SelectedValue = dtshipInformation.Rows[0]["Ship_By"].ToString().Trim();
                ddlShipmentType.SelectedValue = dtshipInformation.Rows[0]["Shipment_Type"].ToString().Trim();
                ddlFreightStatus.SelectedValue = dtshipInformation.Rows[0]["Freight_Status"].ToString().Trim();
                try
                {
                    ddlShipUnit.SelectedValue = dtshipInformation.Rows[0]["Ship_Unit"].ToString().Trim();
                }
                catch
                {
                    ddlShipUnit.SelectedIndex = 0;
                }
                txtAirwaybillno.Text = dtshipInformation.Rows[0]["Airway_Bill_No"].ToString().Trim();
                txtvolumetricweight.Text = dtshipInformation.Rows[0]["Volumetric_weight"].ToString().Trim();
                txtTotalWeight.Text = dtshipInformation.Rows[0]["Actual_Weight"].ToString().Trim();
                txtUnitRate.Text = dtshipInformation.Rows[0]["UnitRate"].ToString().Trim();
                txtReceivingDate.Text = Convert.ToDateTime(dtshipInformation.Rows[0]["Receiving_date"].ToString().Trim()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtShippingDate.Text = Convert.ToDateTime(dtshipInformation.Rows[0]["Shipping_Date"].ToString().Trim()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtdivideby.Text = dtshipInformation.Rows[0]["Divide_By"].ToString().Trim();
            }
        }

        DataTable dtPackingList = objDa.return_DataTable("select Trans_Id,Length,Height,Width,Cartons,Total from Inv_InvoiceShippingDetail  where ref_type='PINV' and ref_id=" + e.CommandArgument.ToString() + "");
        objPageCmn.FillData((object)gvShippingInformation, dtPackingList, "", "");
        shippingCalculation();
        Tabshippinginformation.Visible = true;
        tabContainer.ActiveTabIndex = 0;
        foreach (GridViewRow Row in gvProduct.Rows)
        {
            Label lblSerialNO = (Label)Row.FindControl("lblSerialNO");
            TextBox TxtgvQty = (TextBox)Row.FindControl("QtyGoodReceived");
            TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
            TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
            TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
            Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
            TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
            Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
            Label lblgvProductId = (Label)Row.FindControl("lblgvProductId");
            if (TxtgvQty.Text == "")
            {
                TxtgvQty.Text = "0";
            }
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            if (TxtgvDiscountValue.Text == "")
            {
                TxtgvDiscountValue.Text = "0";
            }
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            Add_Tax_In_Session(txtUnitCost.Text, TxtgvDiscountValue.Text, lblgvProductId.Text, lblSerialNO.Text);
            double PriceValue = double.Parse(txtUnitCost.Text);
            double QtyValue = double.Parse(TxtgvQty.Text);
            double AmountValue = PriceValue * QtyValue;
            double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;

            if (AmountValue != AmntAfterDiscnt & Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
            if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                AmntAfterDiscnt = AmountValue;
            double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
            TotalTax = TotalTax / QtyValue;
            double DiscountPercentage = (double.Parse(TxtgvDiscountValue.Text) * 100 / AmountValue);
            string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
            lblQtyAmmount.Text = strvalue[0].ToString();
            TxtgvDiscountValue.Text = strvalue[2].ToString();
            TxtgvTaxValue.Text = Convert_Into_DF(TotalTax.ToString());
            TxtgvTax.Text = Get_Tax_Percentage(lblgvProductId.Text, lblSerialNO.Text).ToString();
            // TxtgvTaxValue.Text = strvalue[4].ToString();
            lblgvNetAmount.Text = strvalue[5].ToString();
            txtUnitCost.Text = SetDecimal(txtUnitCost.Text);
            DirectGridFooterCalculation();
            if (TxtgvDiscountValue.Text == "")
            {
                TxtgvDiscountValue.Text = "0";
            }
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            GridView GridChild = (GridView)Row.FindControl("gvchildGrid");
            string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
            childGridCalculation(GridChild, PriceAfterDiscount);
        }

        TaxCalculationWithDiscount();
        dtPackingList = null;
        dt = null;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        using (DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["Post"].ToString()))
                {
                    DisplayMessage("This Purchase Invoice is posted , you cannot delete");
                    return;
                }
                if (dt.Rows[0]["Field5"].ToString().Trim() != "" && dt.Rows[0]["Field5"].ToString().Trim() != "0")
                {
                    DisplayMessage("Running bill adjusted , you cannot delete");
                    return;
                }
                ObjPurchaseInvoice.DeletePurchaseInvoiceHeader(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
                ObjPurchaseInvoiceDetail.DeletePurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
                ObjStockBatchMaster.DeleteStockBatchMasterByRefTypeAndRefId(StrCompId, StrBrandId, StrLocationId, "PG", e.CommandArgument.ToString());
                DisplayMessage("Record deleted successfully");
            }
        }
        FillGrid();
        txtInvoiceNo.Text = GetDocumentNumber();
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvPurchaseInvoiceCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void GvPurchaseInvoice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvPurchaseInvoice.Attributes["CurrentSortField"] != null &&
            GvPurchaseInvoice.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPurchaseInvoice.Attributes["CurrentSortField"])
            {
                if (GvPurchaseInvoice.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPurchaseInvoice.Attributes["CurrentSortField"] = sortField;
        GvPurchaseInvoice.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvPurchaseInvoiceCurrentPageIndex.Value));
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnDecimalCount.Value = "0";
        string OldExchangeRate = txtExchangeRate.Text;
        if (OldExchangeRate == "")
        {
            OldExchangeRate = "0";
        }

        if (ddlCurrency.SelectedIndex != 0)
        {
            txtExchangeRate.Text = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            TxtCurrencyValue.Text = ddlCurrency.SelectedValue;
            if (ddlCurrency.SelectedValue == Session["LocId"].ToString())
            {
                hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
            }
            else
            {
                using (DataTable dt = objDa.return_DataTable("select Sys_CurrencyMaster.Currency_Code,Sys_CurrencyMaster.field2 as smallestDenomiation,case when Sys_Country_Currency.field1 is null or Sys_Country_Currency.field1 ='' then '0' else Sys_Country_Currency.field1 end as decimal from Sys_CurrencyMaster left join Sys_Country_Currency on Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.Currency_ID where Sys_CurrencyMaster.Currency_Id ='" + ddlCurrency.SelectedValue + "'"))
                {
                    hdnDecimalCount.Value = dt.Rows[0]["decimal"].ToString();
                    if (hdnDecimalCount.Value == "")
                    {
                        hdnDecimalCount.Value = "2";
                    }
                }
            }
        }
        GetSymbol();
        CostingRate();
        if (RdoWithOutPo.Checked == true)
        {
            OnCurrencyChnaged(OldExchangeRate);
            DataTable dtGridRecord = GetGridRecirdInDatattable();
            ViewState["Dtproduct"] = dtGridRecord;
        }
        setDecimal();
    }
    public void OnCurrencyChnaged(string OldExchnageRate)
    {
        foreach (GridViewRow Row in gvProduct.Rows)
        {
            TextBox lblUnitRate = (TextBox)Row.FindControl("lblUnitRate");
            TextBox TxtgvQty = (TextBox)Row.FindControl("QtyGoodReceived");
            TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
            TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
            TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
            Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
            Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
            if (TxtgvQty.Text != "0")
            {
                if (lblUnitRate.Text != "0")
                {
                    try
                    {
                        lblUnitRate.Text = ((Convert.ToDouble(lblUnitRate.Text) * Convert.ToDouble(OldExchnageRate)) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    }
                    catch
                    {
                    }
                    string[] strvalue = Common.TaxDiscountCaluculation(lblUnitRate.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                    lblQtyAmmount.Text = strvalue[0].ToString();
                    TxtgvDiscountValue.Text = strvalue[2].ToString();
                    TxtgvTaxValue.Text = Convert_Into_DF(strvalue[4].ToString());
                    lblgvNetAmount.Text = strvalue[5].ToString();
                    DirectGridFooterCalculation();
                }
            }
        }
    }
    public void GetSymbol()
    {
        try
        {
            lbllocalGrandtotal.Text = SystemParameter.GetCurrencySmbol(ViewState["CurrencyId"].ToString(), Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
            lblNetAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
            //lblPayAmmount.Text = SystemParameter.GetCurrencySmbol(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Resources.Attendance.Balance_Amount);
            try
            {
                gvProduct.HeaderRow.Cells[6].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
                gvProduct.HeaderRow.Cells[12].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
                gvProduct.HeaderRow.Cells[15].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
                gvProduct.HeaderRow.Cells[17].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
                gvProduct.HeaderRow.Cells[18].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Net_Price, Session["DBConnection"].ToString());
            }
            catch
            {
            }
        }
        catch
        {
        }
    }
    protected void ddlExpCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpCurrency.SelectedIndex != 0)
        {
            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            txtExpExchangeRate.Text = SystemParameter.GetExchageRate(ddlExpCurrency.SelectedValue, Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());

            if (txtExpExchangeRate.Text != "")
            {
                txtExpCharges.Text = SetDecimal((Convert.ToDouble(txtFCExpAmount.Text.Trim()) * Convert.ToDouble(txtExpExchangeRate.Text.Trim())).ToString());
            }
        }
        // GetData();
    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(StrCompId, StrBrandId, StrLocationId, pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void imgbtnsearch_Click(object sender, ImageClickEventArgs e)
    {
        if (ViewState["Dtproduct"] != null)
        {
            if (txtProductSerachValue.Text != "")
            {
                string condition = string.Empty;
                condition = "convert(" + ddlProductSerach.SelectedValue + ",System.String) like '%" + txtProductSerachValue.Text.Trim() + "%'";
                DataTable dtProductSearch = (DataTable)ViewState["Dtproduct"];
                DataView view = new DataView(dtProductSearch, condition, "", DataViewRowState.CurrentRows);
                ViewState["dtProductSearch"] = view.ToTable();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerachGrid, view.ToTable(), "", "");
                if (view.ToTable().Rows.Count == 0)
                {
                    DisplayMessage("No Record found");
                }
            }
        }
        else
        {
            DisplayMessage("No Record found");
        }
    }
    protected void ImgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["dtProductSearch"] = null;
        txtProductSerachValue.Text = "";
        ddlProductSerach.SelectedIndex = 1;
        DataTable dtPurchaseOrder = new DataTable();
        if (ViewState["Dtproduct"] != null)
        {
            dtPurchaseOrder = (DataTable)ViewState["Dtproduct"];
        }
        else
        {
            dtPurchaseOrder = fillPOSearhgrid();
        }
        if (dtPurchaseOrder.Rows.Count != 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dtPurchaseOrder, "", "");
            ViewState["Dtproduct"] = dtPurchaseOrder;

        }
    }
    protected void RdoPo_CheckedChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            ViewState["Dtproduct"] = null;
            ViewState["dtPo"] = null;
            ViewState["DtTax"] = null;
            ViewState["dtTaxHeader"] = null;
            LoadStores();
            Session["DtAdvancePayment"] = null;
            Filladvancepaymentgrid((DataTable)Session["DtAdvancePayment"]);
            fillOrderExpGrid(new DataTable());
            if (RdoPo.Checked)
            {
                pnlProductUpload.Visible = false;
                RdoPOandWithPO();
                DataTable dtPurchaseOrder = fillPOSearhgrid();
                if (dtPurchaseOrder.Rows.Count != 0)
                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvSerachGrid, dtPurchaseOrder, "", "");
                    ViewState["Dtproduct"] = dtPurchaseOrder;
                    GetSymbol();
                }
                else
                {
                    gvSerachGrid.DataSource = null;
                    gvSerachGrid.DataBind();
                    gvProduct.DataSource = null;
                    gvProduct.DataBind();
                    ViewState["Dtproduct"] = null;
                }
            }
            if (RdoWithOutPo.Checked)
            {
                pnlProductUpload.Visible = false;
                ViewState["Dtproduct"] = null;
                ViewState["dtPo"] = null;
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                RdoPOandWithPO();
                DirectGridFooterCalculation();
                GetSymbol();
            }
            if (RdoUpload.Checked)
            {
                pnlProductUpload.Visible = true;
            }
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Text = "";
            txtSupplierName.Focus();
            RdoPo.Checked = false;
            RdoWithOutPo.Checked = false;
            ViewState["Dtproduct"] = null;
            ViewState["dtPo"] = null;
        }
        // GetData();
    }

    //File Upload Section Add By Rahul Sharma On Date 26-06-2024
    protected void FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
        }
    }

    protected void btnGetExcelRecord_Click(object sender, EventArgs e)
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
    public void Import(String path, int fileType)
    {
        try
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

            using (OleDbConnection oledbConn = new OleDbConnection(strcon))
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds, "poItem");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {

                            CheckProductCodeAndInsertIfNotExist(dr["productCode"].ToString(), dr["unit"].ToString(), dr["Model_No"].ToString(), dr["ColorCode"].ToString(), dr["SizeCode"].ToString(), "", "", "");
                            //txtProductcode.Text = dr["productCode"].ToString();
                            //txtProductCode_TextChanged(null, null);
                            //ddlUnit.SelectedItem.Text = dr["unit"].ToString();
                            //txtOrderQty.Text = dr["orderQty"].ToString();
                            //txtfreeQty.Text = dr["freeQty"].ToString();
                            //txtUnitCost.Text = dr["rate"].ToString();
                            AddProductInDetail(txtProductSerachValue.Text, dr["InvoiceQty"].ToString(), dr["UnitCost"].ToString(), dr["DiscountPer"].ToString());
                            //btnProductSave_Click(null, null);
                            pnlProductUpload.Visible = false;
                            PnlProductSearching.Visible = true;
                            txtProductId.Visible = false;
                            txtProductSerachValue.Visible = false;
                            txtSearchProductName.Visible = true;

                        }
                        catch (Exception ex)
                        {


                        }
                    }
                }
                oledbConn.Close();
            }

        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void AddProductInDetail(string ProductCode, string RequestQty, string UnitCost, string DiscountPer)
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("TransID");
        DtProduct.Columns.Add("SerialNo");
        DtProduct.Columns.Add("PONo");
        DtProduct.Columns.Add("POID");
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
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string SearchCriteria = string.Empty;

        SearchCriteria = ProductCode;




        if (HdnEdit.Value == "")
        {
            ReqId = ObjPurchaseInvoice.getAutoId();
        }
        else
        {
            ReqId = HdnEdit.Value.ToString();
        }
        if (SearchCriteria != "")
        {
            DataTable dt = new DataTable();

            dt = objDa.return_DataTable("select ProductCode,EProductName,ProductId,UnitId from inv_productmaster where Brand_Id=" + Session["BrandId"].ToString() + " and ProductCode = '" + ProductCode.ToString().Trim() + "'");

            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                ProductId = "0";
            }
            UnitId = dt.Rows[0]["UnitId"].ToString();
            //this code for get price according selected supplier
            //code start
            if (txtSupplierName.Text != "")
            {
                try
                {
                    UnitCost = (Convert.ToDouble(UnitCost) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    UnitCost = SetDecimal(UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }
            else
            {
                //if supplier not selecte dthen we will get last price according selected product 
                try
                {
                    UnitCost = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    UnitCost = SetDecimal(UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }

        }
        int SerialNO = 0;
        //DataTable dtProduct = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, ReqId.Trim());
        DataTable dtProduct = new DataTable();
        if (ViewState["Dtproduct"] == null)
        {
        }
        else
        {
            dtProduct = (DataTable)ViewState["Dtproduct"];
        }
        if (dtProduct.Rows.Count > 0)
        {
            dtProduct = new DataView(dtProduct, "", "SerialNo Desc", DataViewRowState.CurrentRows).ToTable();
            SerialNO = Convert.ToInt32(dtProduct.Rows[0]["SerialNo"].ToString());
            SerialNO += 1;
        }
        else
        {
            SerialNO = 1;
        }
        DataRow dr;
        if (ViewState["Dtproduct"] == null)
        {
            dr = DtProduct.NewRow();
        }
        else
        {
            DtProduct = GetGridRecirdInDatattable();
            dr = DtProduct.NewRow();
        }
        Add_Tax_In_Session(UnitCost, "0", ProductId.ToString(), SerialNO.ToString());
        dr["TransID"] = DtProduct.Rows.Count + 1;
        dr["SerialNo"] = SerialNO.ToString();
        dr["PONo"] = "0";
        dr["POID"] = "0";
        dr["ProductId"] = ProductId.ToString();
        dr["UnitId"] = UnitId.ToString();
        dr["UnitCost"] = UnitCost;
        dr["OrderQty"] = RequestQty;
        if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "NS")
        {
            dr["RecQty"] = RequestQty;
        }
        else
        {
            dr["RecQty"] = RequestQty;
        }
        dr["FreeQty"] = "0";
        dr["InvoiceQty"] = RequestQty;
        dr["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), SerialNO.ToString()).ToString();
        dr["DiscountV"] = "0";
        dr["DiscountP"] = DiscountPer;
        dr["TaxV"] = Get_Tax_Amount(UnitCost, ProductId.ToString(), SerialNO.ToString());
        dr["InvRemainQty"] = "1";
        dr["RemainQty"] = "1";
        DtProduct.Rows.Add(dr);
        ViewState["Dtproduct"] = (DataTable)DtProduct;
        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        GetChildGridRecordInViewState();
        ViewState["Dtproduct"] = DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        gvProduct.Columns[16].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[17].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[9].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[10].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        gvProduct.Columns[14].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[15].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DirectGridFooterCalculation();
        if (gvProduct.Rows.Count > 0)
        {
            ddlTransType.Enabled = false;
            ddlCurrency.Enabled = false;
            txtExchangeRate.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
            ddlCurrency.Enabled = true;
            txtExchangeRate.Enabled = true;
        }
    }


    //This Function Add By Rahul Sharma on Date 21-06-2024 for Check Product Is Exist or not if not exist then Insert in Database
    public void CheckProductCodeAndInsertIfNotExist(string ProductCode, string Unit, string ModelCode, string ColorCode, string SizeCode, string strSalesPrice1, string strSalesPrice2, string strSalesPrice3)
    {
        //If Product Code not in Excel Sheet
        if (ProductCode == "")
        {
            string strModelId = "";
            string strModelNo = "";
            string strSizeId = "";
            string strColorId = "";

            try
            {
                strSizeId = objDa.get_SingleValue("Select Trans_Id from Set_SizeMaster where Size_Name='" + SizeCode + "'");
                if (strSizeId == "@NOTFOUND@")
                {
                    bool isActive = true; // Assuming IsActive should be true by default

                    // Convert Session variables to string explicitly if not already done
                    string companyId = Session["CompId"].ToString();
                    string userId = Session["UserId"].ToString();
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    int i = 0;
                    // Call the InsertSizeMaster method with corrected parameters
                    i = ObjSizeMaster.InsertSizeMaster(companyId,
                                                     SizeCode,
                                                     SizeCode,
                                                     "",
                                                     "", "", "", "", // Assuming empty strings for Field1 to Field4
                                                     "1900-01-01", // Field5
                                                     isActive.ToString(),
                                                     userId,
                                                     currentDate,
                                                     userId,
                                                     currentDate);
                    if (i != 0)
                    {
                        strSizeId = i.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                strSizeId = "0";
            }
            try
            {
                strColorId = objDa.get_SingleValue("Select Trans_Id from Set_ColorMaster where Color_Name='" + ColorCode + "'");
                if (strColorId == "@NOTFOUND@")
                {
                    bool isActive = true; // Assuming IsActive should be true by default

                    // Convert Session variables to string explicitly if not already done
                    string companyId = Session["CompId"].ToString();
                    string userId = Session["UserId"].ToString();
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    int i = 0;
                    // Call the InsertColorMaster method with corrected parameters
                    i = ObjColorMaster.InsertColorMaster(companyId,
                                                     ColorCode,
                                                     ColorCode,
                                                     "",
                                                     "", "", "", "", // Assuming empty strings for Field1 to Field4
                                                     "1900-01-01", // Field5
                                                     isActive.ToString(),
                                                     userId,
                                                     currentDate,
                                                     userId,
                                                     currentDate);

                    if (i != 0)
                    {
                        strColorId = i.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                strColorId = "0";
            }

            try
            {
                strModelId = objDa.get_SingleValue("Select Trans_Id from Inv_ModelMaster where  Model_No='" + ModelCode + "'");
                if (strModelId == "@NOTFOUND@")
                {
                    SqlConnection conM = new SqlConnection(Session["DBConnection"].ToString());
                    conM.Open();
                    SqlTransaction trnss;
                    trnss = conM.BeginTransaction();
                    int k = 0;
                    try
                    {
                        k = ObjInvModelMaster.InsertModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelCode, ModelCode, ModelCode, "", "0", false.ToString(), "0", "0", "0", "0", "", "", ddlCurrency.SelectedValue.ToString(), "", "", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "", "", "", "", ref trnss);
                        if (k != 0)
                        {
                            strModelId = k.ToString();
                            strModelNo = ModelCode;
                        }
                        trnss.Commit();
                        if (conM.State == System.Data.ConnectionState.Open)
                        {
                            conM.Close();
                        }
                        trnss.Dispose();
                        conM.Dispose();
                    }
                    catch (Exception ex)
                    {
                        trnss.Rollback();
                        if (conM.State == System.Data.ConnectionState.Open)
                        {
                            conM.Close();
                        }
                        trnss.Dispose();
                        conM.Dispose();
                        strModelId = "0";
                    }
                }
                else
                {
                    DataTable dt = objDa.return_DataTable("Select * from Inv_ModelMaster where  Model_No='" + ModelCode + "'");
                    if (dt.Rows.Count > 0)
                    {
                        strModelNo = dt.Rows[0]["Model_No"].ToString();
                    }
                    else
                    {
                        strModelNo = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                strModelId = "0";
            }

            //Here Create New Product Code
            SqlConnection conD = new SqlConnection(Session["DBConnection"].ToString());
            conD.Open();
            SqlTransaction trnD;
            trnD = conD.BeginTransaction();
            try
            {

                string strDocumentId = string.Empty;
                DataTable dtDoc = objDocNo.GetDocumentNumberAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "11", "24", "2");
                if (dtDoc.Rows.Count > 0)
                {
                    strDocumentId = dtDoc.Rows[0]["Trans_Id"].ToString();
                }

                //Need to Update Document Number Id
                ProductCode = objDocNo.GetDocumentNoProduct(strDocumentId, Session["CompId"].ToString().ToString(), "11", "24", strModelNo, strColorId, strSizeId, "", "", "0", ref trnD, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                trnD.Commit();
                if (conD.State == System.Data.ConnectionState.Open)
                {
                    conD.Close();
                }
                trnD.Dispose();
                conD.Dispose();
            }
            catch (Exception ex)
            {
                trnD.Rollback();
                if (conD.State == System.Data.ConnectionState.Open)
                {
                    conD.Close();
                }
                trnD.Dispose();
                conD.Dispose();
            }
        }

        //Already we have Product Code
        if (ProductCode != "")
        {
            DataTable dt = objDa.return_DataTable("Select * from Inv_ProductMaster where ProductCode='" + ProductCode + "'");
            if (dt.Rows.Count > 0)
            {
                bool IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
                if (IsActive == false)
                {
                    objDa.execute_Command("Update Inv_ProductMaster Set IsActive='1', SalesPrice1='" + strSalesPrice1 + "',SalesPrice2='" + strSalesPrice2 + "',SalesPrice3='" + strSalesPrice3 + "' where ProductId='" + dt.Rows[0]["ProductId"].ToString() + "'");
                }
                else
                {
                    objDa.execute_Command("Update Inv_ProductMaster Set SalesPrice1='" + strSalesPrice1 + "',SalesPrice2='" + strSalesPrice2 + "',SalesPrice3='" + strSalesPrice3 + "' where ProductId='" + dt.Rows[0]["ProductId"].ToString() + "'");
                }

                txtProductSerachValue.Text = ProductCode;

                //Update Sales Price
                objDa.execute_Command("Update Inv_StockDetail Set SalesPrice1='" + strSalesPrice1 + "',SalesPrice2='" + strSalesPrice2 + "',SalesPrice3='" + strSalesPrice3 + "' where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + dt.Rows[0]["ProductId"].ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
            }
            else
            {
                string ModelId = "";
                string ModelNo = "";
                string CountryId = "";
                string SizeId = "";
                string ColorId = "";
                string UnitId = "";
                DataTable dtUnit = objDa.return_DataTable("Select * from Inv_UnitMaster where Unit_Name='" + Unit + "'");
                if (dtUnit.Rows.Count > 0)
                {
                    UnitId = dtUnit.Rows[0]["Unit_Id"].ToString();
                }
                else
                {
                    UnitId = "1";
                }

                try
                {
                    SizeId = objDa.get_SingleValue("Select Trans_Id from Set_SizeMaster where Size_Name='" + SizeCode + "'");
                    if (SizeId == "@NOTFOUND@")
                    {
                        bool isActive = true; // Assuming IsActive should be true by default

                        // Convert Session variables to string explicitly if not already done
                        string companyId = Session["CompId"].ToString();
                        string userId = Session["UserId"].ToString();
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                        int i = 0;
                        // Call the InsertSizeMaster method with corrected parameters
                        i = ObjSizeMaster.InsertSizeMaster(companyId,
                                                         SizeCode,
                                                         SizeCode,
                                                         "",
                                                         "", "", "", "", // Assuming empty strings for Field1 to Field4
                                                         "1900-01-01", // Field5
                                                         isActive.ToString(),
                                                         userId,
                                                         currentDate,
                                                         userId,
                                                         currentDate);
                        if (i != 0)
                        {
                            SizeId = i.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SizeId = "0";
                }
                try
                {
                    ColorId = objDa.get_SingleValue("Select Trans_Id from Set_ColorMaster where Color_Name='" + ColorCode + "'");
                    if (ColorId == "@NOTFOUND@")
                    {
                        bool isActive = true; // Assuming IsActive should be true by default

                        // Convert Session variables to string explicitly if not already done
                        string companyId = Session["CompId"].ToString();
                        string userId = Session["UserId"].ToString();
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                        int i = 0;
                        // Call the InsertColorMaster method with corrected parameters
                        i = ObjColorMaster.InsertColorMaster(companyId,
                                                         ColorCode,
                                                         ColorCode,
                                                         "",
                                                         "", "", "", "", // Assuming empty strings for Field1 to Field4
                                                         "1900-01-01", // Field5
                                                         isActive.ToString(),
                                                         userId,
                                                         currentDate,
                                                         userId,
                                                         currentDate);

                        if (i != 0)
                        {
                            ColorId = i.ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ColorId = "0";
                }
                try
                {
                    CountryId = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();
                }
                catch (Exception ex)
                {
                    CountryId = "0";
                }
                try
                {
                    ModelId = objDa.get_SingleValue("Select Trans_Id from Inv_ModelMaster where  Model_No='" + ModelCode + "'");
                    if (ModelId == "@NOTFOUND@")
                    {
                        SqlConnection conM = new SqlConnection(Session["DBConnection"].ToString());
                        conM.Open();
                        SqlTransaction trnss;
                        trnss = conM.BeginTransaction();
                        int k = 0;
                        try
                        {
                            k = ObjInvModelMaster.InsertModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelCode, ModelCode, ModelCode, "", "0", false.ToString(), "0", "0", "0", "0", "", "", ddlCurrency.SelectedValue.ToString(), "", "", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "", "", "", "", ref trnss);
                            if (k != 0)
                            {
                                ModelId = k.ToString();
                                ModelNo = ModelCode;
                            }
                            trnss.Commit();
                            if (conM.State == System.Data.ConnectionState.Open)
                            {
                                conM.Close();
                            }
                            trnss.Dispose();
                            conM.Dispose();
                        }
                        catch (Exception ex)
                        {
                            trnss.Rollback();
                            if (conM.State == System.Data.ConnectionState.Open)
                            {
                                conM.Close();
                            }
                            trnss.Dispose();
                            conM.Dispose();
                            ModelId = "0";
                        }
                    }
                    else
                    {
                        DataTable dtModel = objDa.return_DataTable("Select * from Inv_ModelMaster where  Model_No='" + ModelCode + "'");
                        if (dtModel.Rows.Count > 0)
                        {
                            ModelNo = dtModel.Rows[0]["Model_No"].ToString();
                        }
                        else
                        {
                            ModelNo = "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelId = "0";
                }

                SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
                con.Open();
                SqlTransaction trns;
                trns = con.BeginTransaction();
                int b = 0;
                try
                {
                    string strProductName = ModelCode + "-" + ColorCode + "-" + SizeCode;
                    b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductCode.Trim().ToString().ToUpper(), false.ToString(), ModelId, "0", strProductName.Trim().ToString(), strProductName.Trim().ToString(), CountryId.ToString(), UnitId.ToString(), "S", "0", false.ToString(), "Internally", false.ToString(), "1", "0", "0", strSalesPrice1, strSalesPrice2, strSalesPrice3, "0", "0", "ReseverQty", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "" + ProductCode.ToString() + "", "", "0", true.ToString(), "0", "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "0", "0", Session["CurrencyId"].ToString(), "0", "0", "0", SizeId, ColorId, false.ToString(), ref trns);

                    // b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductCode.Trim().ToString().ToUpper(), false.ToString(), ModelId, "0", ProductCode.Trim().ToString(), ProductCode.Trim().ToString(), CountryId, "1", "S","0",false.ToString(), "Internally", "0", "1","0", "0", "0","0","0","0","0", "ReseverQty","0","0","0","0","0.000","0","0","0", "0", "0", "0", "0","0","0", "0","0", "0", "0", true.ToString(),"0","0", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(),"0.000", "0", Session["CurrencyId"].ToString(),"0","0","0",SizeId,ColorId, ref trns);
                    if (b != 0)
                    {

                        if (ModelId != "0" && ColorId != "0" && SizeId != "0")
                        {
                            ObjProductMaster.UpdateProductIdforModelColourSize(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }
                        else
                        {
                            ObjProductMaster.UpdateProductId(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ProductCode, ref trns);
                        }

                        //Insert Sales Price in Item Master 
                        objDa.execute_Command("Insert Into Inv_StockDetail ([Company_Id],[Brand_Id],[Location_Id],[ProductId],[OpeningBalance],[RackID],[Quantity],[Minimum_Qty],[Maximum_Qty],[ReserveQty],[DamageQty],[BlockedQty],[OrderQty],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate], Finance_Year_Id, SalesPrice1, SalesPrice2, SalesPrice3) Values ('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + b.ToString() + "','0','0','0','0','0','0','0','0','0', '0','0','','','','1','" + DateTime.Now.ToString("yyyy-MM-dd") + "','1','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["FinanceYearId"].ToString() + "','" + strSalesPrice1 + "', '" + strSalesPrice2 + "', '" + strSalesPrice3 + "')");

                        ObjProductMaster.InsertProductLabelInfo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), "", "", ref trns);

                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "6", b.ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString().ToString(), b.ToString(), Session["BrandId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    }
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }

                    trns.Dispose();
                    con.Dispose();

                }

                DataTable dtProduct = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), "0");
                if (dtProduct.Rows.Count > 0)
                {
                    txtProductSerachValue.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                }
            }
        }
    }




    //public void CheckProductCodeAndInsertIfNotExist(string ProductCode,string Unit ,string ModelCode, string ColorCode, string SizeCode)
    //{
    //    if (ProductCode != "")
    //    {
    //        DataTable dt = objDa.return_DataTable("Select * from Inv_ProductMaster where ProductCode='" + ProductCode + "'");
    //        if (dt.Rows.Count > 0)
    //        {
    //            bool IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
    //            if (IsActive == false)
    //            {
    //                objDa.execute_Command("Update Inv_ProductMaster Set IsActive='1' where ProductId='" + dt.Rows[0]["ProductId"].ToString() + "'");
    //            }
    //        }
    //        else
    //        {
    //            string ModelId = "";
    //            string CountryId = "";
    //            string SizeId = "";
    //            string ColorId = "";
    //            string UnitId = "";
    //            DataTable dtUnit = objDa.return_DataTable("Select * from Inv_UnitMaster where Unit_Name='" + Unit + "'");
    //            if (dtUnit.Rows.Count > 0)
    //            {
    //                UnitId = dtUnit.Rows[0]["Unit_Id"].ToString();
    //            }
    //            else
    //            {
    //                UnitId = "1";
    //            }
    //            try
    //            {
    //                SizeId = objDa.get_SingleValue("Select Trans_Id from Set_SizeMaster where Size_Code='" + SizeCode + "'");
    //                if (SizeId == "@NOTFOUND@")
    //                {
    //                    bool isActive = true; // Assuming IsActive should be true by default

    //                    // Convert Session variables to string explicitly if not already done
    //                    string companyId = Session["CompId"].ToString();
    //                    string userId = Session["UserId"].ToString();
    //                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
    //                    int i = 0;
    //                    // Call the InsertSizeMaster method with corrected parameters
    //                    i = ObjSizeMaster.InsertSizeMaster(companyId,
    //                                                     SizeCode,
    //                                                     SizeCode,
    //                                                     "",
    //                                                     "", "", "", "", // Assuming empty strings for Field1 to Field4
    //                                                     "1900-01-01", // Field5
    //                                                     isActive.ToString(),
    //                                                     userId,
    //                                                     currentDate,
    //                                                     userId,
    //                                                     currentDate);
    //                    if (i != 0)
    //                    {
    //                        SizeId = i.ToString();
    //                    }
    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //                SizeId = "0";
    //            }
    //            try
    //            {
    //                ColorId = objDa.get_SingleValue("Select Trans_Id from Set_ColorMaster where Color_Code='" + ColorCode + "'");
    //                if (ColorId == "@NOTFOUND@")
    //                {
    //                    bool isActive = true; // Assuming IsActive should be true by default

    //                    // Convert Session variables to string explicitly if not already done
    //                    string companyId = Session["CompId"].ToString();
    //                    string userId = Session["UserId"].ToString();
    //                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
    //                    int i = 0;
    //                    // Call the InsertColorMaster method with corrected parameters
    //                    i = ObjColorMaster.InsertColorMaster(companyId,
    //                                                     ColorCode,
    //                                                     ColorCode,
    //                                                     "",
    //                                                     "", "", "", "", // Assuming empty strings for Field1 to Field4
    //                                                     "1900-01-01", // Field5
    //                                                     isActive.ToString(),
    //                                                     userId,
    //                                                     currentDate,
    //                                                     userId,
    //                                                     currentDate);

    //                    if (i != 0)
    //                    {
    //                        ColorId = i.ToString();
    //                    }
    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //                ColorId = "0";
    //            }
    //            try
    //            {
    //                CountryId = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();

    //            }
    //            catch (Exception ex)
    //            {
    //                CountryId = "0";
    //            }
    //            try
    //            {
    //                ModelId = objDa.get_SingleValue("Select Trans_Id from Inv_ModelMaster where  Model_No='" + ModelCode + "'");
    //                if (ModelId == "@NOTFOUND@")
    //                {
    //                    SqlConnection conM = new SqlConnection(Session["DBConnection"].ToString());
    //                    conM.Open();
    //                    SqlTransaction trnss;
    //                    trnss = conM.BeginTransaction();
    //                    int k = 0;
    //                    try
    //                    {
    //                        k = ObjInvModelMaster.InsertModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelCode, ModelCode, ModelCode, "", "0", false.ToString(), "0", "0", "0", "0", "", "", ddlCurrency.SelectedValue.ToString(), "", "", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "", "", "", "", ref trnss);
    //                        if (k != 0)
    //                        {
    //                            ModelId = k.ToString();
    //                        }
    //                        trnss.Commit();
    //                        if (conM.State == System.Data.ConnectionState.Open)
    //                        {
    //                            conM.Close();
    //                        }
    //                        trnss.Dispose();
    //                        conM.Dispose();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        trnss.Rollback();
    //                        if (conM.State == System.Data.ConnectionState.Open)
    //                        {
    //                            conM.Close();
    //                        }
    //                        trnss.Dispose();
    //                        conM.Dispose();
    //                        ModelId = "0";
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                ModelId = "0";
    //            }
    //            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
    //            con.Open();
    //            SqlTransaction trns;
    //            trns = con.BeginTransaction();
    //            int b = 0;
    //            try
    //            {
    //                b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductCode.Trim().ToString().ToUpper(), false.ToString(), ModelId, "0", ProductCode.Trim().ToString(), ProductCode.Trim().ToString(), CountryId.ToString(), UnitId.ToString(), "S", "0", false.ToString(), "Internally", false.ToString(), "1", "0", "0", "0", "0", "0", "0", "0", "ReseverQty", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "" + ProductCode.ToString() + "", "", "0", true.ToString(), "0", "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "0", "0", Session["CurrencyId"].ToString(), "0", "0", "0", SizeId, ColorId, false.ToString(), ref trns);

    //                // b = ObjProductMaster.InsertProductMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductCode.Trim().ToString().ToUpper(), false.ToString(), ModelId, "0", ProductCode.Trim().ToString(), ProductCode.Trim().ToString(), CountryId, "1", "S","0",false.ToString(), "Internally", "0", "1","0", "0", "0","0","0","0","0", "ReseverQty","0","0","0","0","0.000","0","0","0", "0", "0", "0", "0","0","0", "0","0", "0", "0", true.ToString(),"0","0", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(),"0.000", "0", Session["CurrencyId"].ToString(),"0","0","0",SizeId,ColorId, ref trns);
    //                if (b != 0)
    //                {
    //                    ObjProductMaster.InsertProductLabelInfo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), "", "", ref trns);

    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "6", b.ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString().ToString(), b.ToString(), Session["BrandId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

    //                }
    //                trns.Commit();
    //                if (con.State == System.Data.ConnectionState.Open)
    //                {
    //                    con.Close();
    //                }
    //                trns.Dispose();
    //                con.Dispose();
    //            }
    //            catch (Exception ex)
    //            {
    //                trns.Rollback();
    //                if (con.State == System.Data.ConnectionState.Open)
    //                {
    //                    con.Close();
    //                }

    //                trns.Dispose();
    //                con.Dispose();

    //            }

    //        }
    //    }

    //}
    protected void btnGvProductRefresh_Click(object sender, EventArgs e)
    {
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        DisplayMessage("Detail Delete Successfully");
    }
    protected void btnDownloadProduct_Click(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("productCode");
        //dt.Columns.Add("InvoiceQty");    
        //dt.Columns.Add("unit");
        //dt.Columns.Add("UnitCost");
        //dt.Columns.Add("DiscountPer");
        //dt.Columns.Add("Model_No");
        //dt.Columns.Add("ColorCode");
        //dt.Columns.Add("SizeCode");
        //foreach (GridViewRow gvr in gvProduct.Rows)
        //{
        //    Label lblproductId = (Label)gvr.FindControl("lblproductcode");
        //    TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");


        //    // Create a new row in DataTable
        //    DataRow dr = dt.NewRow();

        //    // Populate the DataRow with values from GridViewRow controls
        //    dr["productCode"] = lblproductcode.Text;

        //    // Add the DataRow to DataTable
        //    dt.Rows.Add(dr);

        //}

        //gvProduct.DataSource = null;
        //gvProduct.DataBind();


        DataTable dt = (DataTable)ViewState["Dtproduct"];
        if (dt.Rows.Count > 0)
        {

        }
        else
        {
            dt = new DataTable();
            dt.Columns.Add("productCode");
            dt.Columns.Add("InvoiceQty");
            dt.Columns.Add("unit");
            dt.Columns.Add("UnitCost");
            dt.Columns.Add("DiscountPer");
            dt.Columns.Add("Model_No");
            dt.Columns.Add("ColorCode");
            dt.Columns.Add("SizeCode");
        }
        ExportTableData(dt);

    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "UploadPoItem";
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
    //End Code
    public DataTable fillPOSearhgrid()
    {
        DataTable dtPurchaseOrder = null;
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseOrderApproval");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                dtPurchaseOrder = new DataView(ObjPurchaseOrder.GetProductFromPurchaseOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtSupplierName.Text.Trim().Split('/')[1].ToString()), "Field41='Approved'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtPurchaseOrder = ObjPurchaseOrder.GetProductFromPurchaseOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtSupplierName.Text.Trim().Split('/')[1].ToString());
            }
        }
        dtPurchaseOrder.Columns["PONO"].ColumnName = "POID";
        dtPurchaseOrder.Columns["TaxPercentage"].ColumnName = "TaxP";
        dtPurchaseOrder.Columns["TaxValue"].ColumnName = "TaxV";
        dtPurchaseOrder.Columns["DisPercentage"].ColumnName = "DiscountP";
        dtPurchaseOrder.Columns["DiscountValue"].ColumnName = "DiscountV";
        dtPurchaseOrder.Columns["Serial_No"].ColumnName = "SerialNo";
        dtPurchaseOrder.Columns["Product_Id"].ColumnName = "ProductId";
        dtPurchaseOrder.Columns["PONO1"].ColumnName = "PONO";

        return dtPurchaseOrder;
    }
    public void RdoPOandWithPO()
    {
        PnlProductSearching.Visible = true;
        txtNetDisPer.Text = "0";
        txtNetDisVal.Text = "0";
        txtNetTaxPar.Text = "0";
        txtNetTaxVal.Text = Convert_Into_DF("0");
        txtNetAmount.Text = "0";
        txtGrandTotal.Text = "0";
        if (!RdoWithOutPo.Checked)
        {
            if (ddlProductSerach.Items.FindByText("Purchase Order No") == null)
            {
                ddlProductSerach.Items.Insert(2, new ListItem("Purchase Order No", "PONO"));
            }
            txtSearchProductName.Visible = false;
            txtProductId.Visible = false;
            txtProductSerachValue.Visible = true;
            imgbtnsearch.Visible = true;
            ImgbtnRefresh.Visible = true;
            ImgbtnProductSave.Visible = false;
            txtSearchProductName.Visible = false;
            btnAddtoList.Visible = false;
            btnAddProductScreen.Visible = false;
        }
        else
        {
            try
            {
                ddlProductSerach.Items.RemoveAt(2);
            }
            catch
            {
            }
            btnAddtoList.Visible = true;
            btnAddProductScreen.Visible = true;
            imgbtnsearch.Visible = false;
            ImgbtnRefresh.Visible = false;
            ImgbtnProductSave.Visible = true;
            txtProductSerachValue.Visible = false;
            txtProductId.Visible = false;
            txtSearchProductName.Visible = true;
            ddlProductSerach.SelectedIndex = 1;
        }
    }
    protected void chkTrandId_CheckedChanged(object seder, EventArgs e)
    {
        DataTable dt = new DataTable();
        GridViewRow row = (GridViewRow)((CheckBox)seder).Parent.Parent;
        dt = (DataTable)ViewState["Dtproduct"];

        dt = new DataView((DataTable)ViewState["Dtproduct"], "Trans_Id='" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["CurrencyId"].ToString() == "")
            {
                string Query = "Select CurrencyID From Inv_PurchaseOrderHeader where TransId = '" + dt.Rows[0]["POID"].ToString() + "' and IsActive = 'True'";
                string Currency_ID_Temp = objDa.return_DataTable(Query).Rows[0][0].ToString();
                dt.Rows[0]["CurrencyId"] = Currency_ID_Temp;
            }
            if (dt.Rows[0]["CurrencyId"].ToString() != ddlCurrency.SelectedValue.ToString())
            {
                DisplayMessage("Purchase Order and Purchase Invoice currency for this supplier dose not match");
                foreach (GridViewRow GVR in gvSerachGrid.Rows)
                {
                    CheckBox Chk = (CheckBox)GVR.FindControl("chkTrandId");
                    Chk.Checked = false;
                }
                return;
            }
            ddlCurrency.Enabled = false;
            txtExchangeRate.Enabled = false;

            if (dt.Rows[0]["Trans_Type"].ToString() != ddlTransType.SelectedValue.ToString())
            {
                DisplayMessage("Transction type of Purchase Order and Purchase Invoice dose not match");
                foreach (GridViewRow GVR in gvSerachGrid.Rows)
                {
                    CheckBox Chk = (CheckBox)GVR.FindControl("chkTrandId");
                    Chk.Checked = false;
                }
                return;
            }

            ddlTransType.Enabled = false;
            Add_Tax_In_Session_From_Order(dt.Rows[0]["UnitCost"].ToString(), dt.Rows[0]["ProductId"].ToString(), dt.Rows[0]["POID"].ToString(), dt.Rows[0]["SerialNo"].ToString());
        }
        if (ViewState["dtPo"] != null)
        {
            // DataTable dtPO = (DataTable)ViewState["dtPo"];
            DataTable dtPO = GetGridRecirdInDatattable();
            try
            {
                dt.Columns["Trans_Id"].ColumnName = "TransId";
            }
            catch
            {
            }
            dtPO.ImportRow(dt.Rows[0]);
            dt = new DataView(dtPO, "", "SerialNo Asc", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        ViewState["dtPo"] = dt;
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        //TaxCalculationWithDiscount();
        if (gvProduct.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                Label Product_Id = (Label)gvr.FindControl("lblgvProductId");
                TextBox Unit_Rate = (TextBox)gvr.FindControl("lblUnitRate");
                Label Order_Qty = (Label)gvr.FindControl("OrderQty");
                TextBox Qty_Good_Received = (TextBox)gvr.FindControl("QtyGoodReceived");
                TextBox Discount_Per = (TextBox)gvr.FindControl("lblDiscount");
                TextBox Discount_Value = (TextBox)gvr.FindControl("lblDiscountValue");
                TextBox Tax_Per = (TextBox)gvr.FindControl("lblTax");
                TextBox Tax_Value = (TextBox)gvr.FindControl("lblTaxValue");
                Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
                if (Unit_Rate.Text == "")
                    Unit_Rate.Text = "0";
                if (Order_Qty.Text == "")
                    Order_Qty.Text = "0";
                if (Qty_Good_Received.Text == "")
                    Qty_Good_Received.Text = "0";
                if (Discount_Per.Text == "")
                    Discount_Per.Text = "0";
                if (Discount_Value.Text == "")
                    Discount_Value.Text = "0";
                Tax_Per.Text = Get_Tax_Percentage(Product_Id.Text, lblSerialNO.Text).ToString();
                if (Tax_Per.Text == "")
                    Tax_Per.Text = "0";
                if (Tax_Value.Text == "")
                    Tax_Value.Text = Convert_Into_DF("0");
                TaxCalculationWithDiscount();
                if (Qty_Good_Received.Text == "0.00")
                {
                    Discount_Value.Text = "0.00";
                    Tax_Value.Text = Convert_Into_DF("0.00");
                }
                else
                {
                    TaxCalculationWithDiscount();
                }
                //Tax_Value.Text = SetDecimal(TaxCalculation(Unit_Rate.Text, Product_Id.Text).ToString());
            }
        }
        dt = new DataView((DataTable)ViewState["Dtproduct"], "Trans_Id<>'" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        ViewState["Dtproduct"] = dt;
        if (ViewState["dtProductSearch"] == null)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        }
        else
        {
            dt = new DataView((DataTable)ViewState["dtProductSearch"], "Trans_Id<>'" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["dtProductSearch"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        }
        DirectGridFooterCalculation();
        //for showing advance payment
        //code start
        FillAdvancePayment_BYOrderId(((Label)row.FindControl("lblOrderId")).Text.Trim(), ((Label)row.FindControl("lblPONo")).Text.Trim());
        //code end
        //for showing ship expenses on invoice 
        DataTable dtpurchaseinvoicedetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            dtpurchaseinvoicedetail = new DataView(dtpurchaseinvoicedetail, "POId=" + ((Label)row.FindControl("lblOrderId")).Text.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtpurchaseinvoicedetail.Rows.Count == 0)
        {
            fillOrderExpGrid(ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), ((Label)row.FindControl("lblOrderId")).Text.Trim(), "PO"));
        }
    }
    public void fillOrderExpGrid(DataTable dt)
    {
        gvOrderExpenses.DataSource = dt;
        gvOrderExpenses.DataBind();
        dt = null;
    }
    //protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    //{
    //    GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent;
    //    DataTable dt = new DataTable();
    //    if (RdoPo.Checked)
    //    {
    //        dt = (DataTable)ViewState["dtPo"];
    //    }
    //    else
    //    {
    //        dt = (DataTable)ViewState["Dtproduct"];
    //    }
    //    DataRow[] drProduct;
    //    drProduct = dt.Select("TransId=" + e.CommandArgument.ToString());
    //    if (drProduct[0]["productId"].ToString() != "")
    //    {
    //        //string ProductId = ((HiddenField)row.FindControl("hdngvProductId")).Value;
    //        // DeleteRowFromTempProductTaxTable(drProduct[0]["productId"].ToString());
    //    }
    //    dt = new DataView(dt, "TransId <>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
    //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    //    objPageCmn.FillData((object)gvProduct, dt, "", "");
    //    //for advance payment
    //    //code start
    //    DataTable dttemp = new DataTable();
    //    dttemp = dt.Copy();
    //    dttemp = new DataView(dttemp, "POID=" + ((Label)row.FindControl("lblPOId")).Text.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
    //    if (dttemp.Rows.Count == 0)
    //    {
    //        DataTable dtAdvancePayment = new DataTable();
    //        if (Session["DtAdvancePayment"] != null)
    //        {
    //            //we can not delete 
    //            //here we delete advance payment row
    //            dtAdvancePayment = (DataTable)Session["DtAdvancePayment"];
    //            try
    //            {
    //                dtAdvancePayment = new DataView(dtAdvancePayment, "OrderNo<>'" + ((Label)row.FindControl("lblPONo")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            }
    //            catch
    //            {
    //            }
    //            Session["DtAdvancePayment"] = dtAdvancePayment;
    //            Filladvancepaymentgrid(dtAdvancePayment);
    //        }
    //    }
    //    //code end
    //    if (RdoPo.Checked)
    //    {
    //        if (ViewState["dtPo"] != null)
    //        {
    //            DataTable dtStorePO = dt;
    //            dt = new DataView((DataTable)ViewState["dtPo"], "TransId=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
    //            try
    //            {
    //                dt.Columns["TransId"].ColumnName = "Trans_Id";
    //            }
    //            catch
    //            {
    //            }
    //            DataTable Dt_Temp = (DataTable)ViewState["Dtproduct"];
    //            DataTable dtPO = Dt_Temp.Clone();
    //            dtPO.Columns["InvoiceQty"].DataType = typeof(double);
    //            dtPO.Columns["RecQty"].DataType = typeof(double);
    //            dtPO.ImportRow(dt.Rows[0]);
    //            if (dtPO.Rows.Count != 0)
    //            {
    //                try
    //                {
    //                    dtPO.Rows[dtPO.Rows.Count - 1]["TransId"] = dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Id"].ToString();
    //                    dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Type"] = ddlTransType.SelectedValue.ToString();
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            dttemp = dtPO.Copy();
    //            try
    //            {
    //                dttemp = new DataView(dttemp, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
    //            }
    //            catch
    //            {
    //            }
    //            if ((new DataView(dttemp, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count > 1)
    //            {
    //                try
    //                {
    //                    dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Id"] = Convert.ToDouble(dttemp.Rows[0]["Trans_Id"].ToString()) + 1;
    //                    dtPO.Rows[dtPO.Rows.Count - 1]["TransId"] = Convert.ToDouble(dttemp.Rows[0]["Trans_Id"].ToString()) + 1;
    //                }
    //                catch
    //                {
    //                }
    //            }
    //            //DataTable Dt_Temp_Order = new DataTable();
    //            //if(ViewState["dtProductSearch"]!=null)
    //            //{
    //            //    Dt_Temp_Order = ViewState["dtProductSearch"] as DataTable;
    //            //}
    //            //else
    //            //{
    //            //    Dt_Temp_Order = dtPO.Clone();
    //            //}
    //            //foreach(DataRow Dt_Row in dtPO.Rows)
    //            //{
    //            //    Dt_Temp_Order.ImportRow(dtPO.Rows[0]);
    //            //    //Dt_Temp_Order.Rows.Add(Dt_Row.ItemArray);
    //            //}
    //            //ViewState["dtProductSearch"] = Dt_Temp_Order;
    //            //objPageCmn.FillData((object)gvSerachGrid, Dt_Temp_Order, "", "");
    //            //ViewState["dtPo"] = dtStorePO;
    //            dt = dtPO;
    //            ViewState["dtProductSearch"] = dtPO;
    //            objPageCmn.FillData((object)gvSerachGrid, dtPO, "", "");
    //            ViewState["dtPo"] = dtStorePO;
    //        }
    //    }
    //    else
    //    {
    //        ViewState["Dtproduct"] = dt;
    //    }
    //    if (gvProduct.Rows.Count == 0)
    //    {
    //        fillOrderExpGrid(new DataTable());
    //    }
    //    ViewState["Dtproduct"] = dt;
    //    DirectGridFooterCalculation();
    //    CostingRate();
    //    AllPageCode();
    //    if (gvProduct.Rows.Count > 0)
    //    {
    //        ddlTransType.Enabled = false;
    //    }
    //    else
    //    {
    //        ddlTransType.Enabled = true;
    //    }
    //}
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent;
        DataTable dt = new DataTable();
        if (RdoPo.Checked)
        {
            dt = (DataTable)ViewState["dtPo"];
        }
        else
        {
            dt = (DataTable)ViewState["Dtproduct"];
        }
        DataRow[] drProduct;
        drProduct = dt.Select("TransId=" + e.CommandArgument.ToString());
        if (drProduct[0]["productId"].ToString() != "")
        {
            //string ProductId = ((HiddenField)row.FindControl("hdngvProductId")).Value;
            DeleteRowFromTempProductTaxTable(drProduct[0]["productId"].ToString(), e.CommandName.ToString());
        }
        dt = new DataView(dt, "TransId <>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        //for advance payment
        //code start
        DataTable dttemp = new DataTable();
        dttemp = dt.Copy();
        dttemp = new DataView(dttemp, "POID=" + ((Label)row.FindControl("lblPOId")).Text.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dttemp.Rows.Count == 0)
        {
            DataTable dtAdvancePayment = new DataTable();
            if (Session["DtAdvancePayment"] != null)
            {
                //we can not delete 
                //here we delete advance payment row
                dtAdvancePayment = (DataTable)Session["DtAdvancePayment"];
                try
                {
                    dtAdvancePayment = new DataView(dtAdvancePayment, "OrderNo<>'" + ((Label)row.FindControl("lblPONo")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["DtAdvancePayment"] = dtAdvancePayment;
                Filladvancepaymentgrid(dtAdvancePayment);
            }
        }
        //code end
        if (RdoPo.Checked)
        {
            if (ViewState["dtPo"] != null)
            {
                DataTable dtStorePO = dt;
                dt = new DataView((DataTable)ViewState["dtPo"], "TransId=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                try
                {
                    dt.Columns["TransId"].ColumnName = "Trans_Id";
                }
                catch
                {
                }
                DataTable dtPO = (DataTable)ViewState["Dtproduct"];
                dtPO.ImportRow(dt.Rows[0]);
                if (dtPO.Rows.Count != 0)
                {
                    try
                    {
                        dtPO.Rows[dtPO.Rows.Count - 1]["TransId"] = dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Id"].ToString();
                        dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Type"] = ddlTransType.SelectedValue.ToString();
                    }
                    catch
                    {
                    }
                }
                dttemp = dtPO.Copy();
                try
                {
                    dttemp = new DataView(dttemp, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if ((new DataView(dttemp, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count > 1)
                {
                    try
                    {
                        dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Id"] = Convert.ToDouble(dttemp.Rows[0]["Trans_Id"].ToString()) + 1;
                        dtPO.Rows[dtPO.Rows.Count - 1]["TransId"] = Convert.ToDouble(dttemp.Rows[0]["Trans_Id"].ToString()) + 1;
                    }
                    catch
                    {
                    }
                }
                dt = dtPO;
                foreach (DataRow Dt_Row in dtPO.Rows)
                {
                    if (Dt_Row["CurrencyId"].ToString() == "" || Dt_Row["PODate"].ToString() == "")
                    {
                        string Query = "Select CurrencyID,PODate From Inv_PurchaseOrderHeader where TransId = '" + Dt_Row["POID"].ToString() + "' and IsActive = 'True'";
                        DataTable Temp_DT = objDa.return_DataTable(Query);
                        Dt_Row["CurrencyId"] = Temp_DT.Rows[0]["CurrencyID"].ToString();
                        Dt_Row["PODate"] = Temp_DT.Rows[0]["PODate"].ToString();
                    }
                }
                ViewState["dtProductSearch"] = dtPO;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerachGrid, dtPO, "", "");
                ViewState["dtPo"] = dtStorePO;
            }
        }
        else
        {
            ViewState["Dtproduct"] = dt;
        }
        if (gvProduct.Rows.Count == 0)
        {
            fillOrderExpGrid(new DataTable());
        }
        ViewState["Dtproduct"] = dt;
        DirectGridFooterCalculation();
        CostingRate();
        if (gvProduct.Rows.Count > 0)
        {
            ddlTransType.Enabled = false;
            ddlCurrency.Enabled = false;
            txtExchangeRate.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
            ddlCurrency.Enabled = true;
            txtExchangeRate.Enabled = true;
        }
        dt = null;
    }
    protected void txtNetTaxPar_TextChanged(object sender, EventArgs e)
    {
        if (txtNetTaxPar.Text == "")
        {
            txtNetTaxPar.Text = "0";
        }
        double GossAmount = 0;
        try
        {
            GossAmount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text);
        }
        catch
        {
        }
        string[] str = Common.TaxDiscountCaluculation(GossAmount.ToString(), "0", txtNetTaxPar.Text, "", "", txtNetDisVal.Text, false, ddlCurrency.SelectedValue, true, Session["DBConnection"].ToString());
        txtNetTaxVal.Text = Convert_Into_DF(str[4].ToString());
        txtGrandTotal.Text = str[5].ToString();
        try
        {
            txtlocalGrandtotal.Text = ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text;
        }
        catch
        {
            txtlocalGrandtotal.Text = "0";
        }
        TaxDiscountFromHeader(false);
        txtBillAmount.Text = txtGrandTotal.Text;
        txtBillAmount_TextChanged(null, null);
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
    }
    protected void txtNetTaxVal_TextChanged(object sender, EventArgs e)
    {
        if (txtNetTaxVal.Text == "")
        {
            txtNetTaxVal.Text = Convert_Into_DF("0");
        }
        double GossAmount = 0;
        try
        {
            GossAmount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text);
        }
        catch
        {
        }
        string[] str = Common.TaxDiscountCaluculation(GossAmount.ToString(), "0", "", Convert_Into_DF(txtNetTaxVal.Text), "", txtNetDisVal.Text, false, ddlCurrency.SelectedValue, true, Session["DBConnection"].ToString());
        txtNetTaxPar.Text = str[3].ToString();
        txtGrandTotal.Text = str[5].ToString();
        try
        {
            txtlocalGrandtotal.Text = ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text;
        }
        catch
        {
            txtlocalGrandtotal.Text = "0";
        }
        TaxDiscountFromHeader(false);
        txtBillAmount.Text = txtGrandTotal.Text;
        txtBillAmount_TextChanged(null, null);
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
    }
    protected void txtNetDisPer_TextChanged(object sender, EventArgs e)
    {
        if (txtNetDisPer.Text == "")
        {
            txtNetDisPer.Text = "0";
        }
        double GossAmount = 0;
        try
        {
            GossAmount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text);
        }
        catch
        {
        }
        string[] str = Common.TaxDiscountCaluculation(GossAmount.ToString(), "0", txtNetTaxPar.Text, "", txtNetDisPer.Text, "", false, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
        txtNetTaxVal.Text = Convert_Into_DF(str[4].ToString());
        txtNetDisVal.Text = str[2].ToString();
        txtGrandTotal.Text = str[5].ToString();
        CalculationchangeIntaxGridview();
        TaxDiscountFromHeader(true);
        txtBillAmount.Text = txtGrandTotal.Text;
        txtBillAmount_TextChanged(null, null);

        TaxCalculationWithDiscount();
    }
    protected void txtNetDisVal_TextChanged(object sender, EventArgs e)
    {
        if (txtNetDisVal.Text == "")
        {
            txtNetDisVal.Text = "0";
        }
        double GossAmount = 0;
        try
        {
            GossAmount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text);
        }
        catch
        {
        }
        string[] str = Common.TaxDiscountCaluculation(GossAmount.ToString(), "0", txtNetTaxPar.Text, "", "", txtNetDisVal.Text, false, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
        txtNetTaxVal.Text = Convert_Into_DF(str[4].ToString());
        txtNetDisPer.Text = str[1].ToString();
        txtGrandTotal.Text = str[5].ToString();
        txtlocalGrandtotal.Text = GetLocalPrice(txtGrandTotal.Text.Trim());
        CalculationchangeIntaxGridview();
        TaxDiscountFromHeader(true);
        txtBillAmount.Text = txtGrandTotal.Text;
        txtBillAmount_TextChanged(null, null);
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
        TaxCalculationWithDiscount();
    }
    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text != "")
        {
            using (DataTable dt = new DataView(ObjPurchaseInvoice.GetPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString()), "InvoiceNo='" + txtInvoiceNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable())
            {
                if (dt.Rows.Count != 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Purchase Invoice No Already Exist");
                        txtInvoiceNo.Text = "";
                        txtInvoiceNo.Focus();
                    }
                    else
                    {
                        DisplayMessage("Purchase Invoice No Already Exist :- Go To Bin");
                        txtInvoiceNo.Text = "";
                        txtInvoiceNo.Focus();
                    }
                }
                else
                {
                    ddlInvoiceType.Focus();
                }
            }
        }
    }
    public void TaxDiscountFromHeader(bool IsDiscount)
    {
        double netPrice = 0;
        double netTax = 0;
        double netDiscount = 0;
        foreach (GridViewRow Row in gvProduct.Rows)
        {
            if (((TextBox)Row.FindControl("QtyGoodReceived")).Text != "0")
            {
                string[] str;
                if (IsDiscount)
                {
                    str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblUnitRate")).Text, ((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim(), ((TextBox)Row.FindControl("lblTax")).Text, "", txtNetDisPer.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                    ((TextBox)Row.FindControl("lblTax")).Text = str[3].ToString();
                    // ((TextBox)Row.FindControl("lblTaxValue")).Text = str[4].ToString();
                    ((TextBox)Row.FindControl("lblDiscountValue")).Text = str[2].ToString();
                    ((TextBox)Row.FindControl("lblDiscount")).Text = str[1].ToString();
                    ((Label)Row.FindControl("lblAmount")).Text = str[5].ToString();
                    ((Label)Row.FindControl("lblUnitRateLocal")).Text = GetLocalPrice((Convert.ToDouble(((TextBox)Row.FindControl("lblUnitRate")).Text.Trim()) - Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim()) + Convert.ToDouble(Convert_Into_DF(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim()))).ToString()).ToString();
                    ((Label)Row.FindControl("lblLocalQtyAmmount")).Text = GetLocalCurrencyConversion((Convert.ToDouble(((Label)Row.FindControl("lblUnitRateLocal")).Text) * Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text)).ToString());
                }
                else
                {
                    str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblUnitRate")).Text, ((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim(), txtNetTaxPar.Text, "", "", ((TextBox)Row.FindControl("lblDiscountValue")).Text, true, ddlCurrency.SelectedValue, true, Session["DBConnection"].ToString());
                    ((TextBox)Row.FindControl("lblTaxValue")).Text = Convert_Into_DF(str[4].ToString());
                    ((TextBox)Row.FindControl("lblTax")).Text = str[3].ToString();
                    ((Label)Row.FindControl("lblAmount")).Text = str[5].ToString();
                    ((Label)Row.FindControl("lblUnitRateLocal")).Text = GetLocalPrice((Convert.ToDouble(((TextBox)Row.FindControl("lblUnitRate")).Text.Trim()) - Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim()) + Convert.ToDouble(Convert_Into_DF(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim()))).ToString()).ToString();
                    ((Label)Row.FindControl("lblLocalQtyAmmount")).Text = GetLocalCurrencyConversion((Convert.ToDouble(((Label)Row.FindControl("lblUnitRateLocal")).Text) * Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text)).ToString());
                }
                netDiscount += Convert.ToDouble((Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text)) * Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim()));
                netTax += Convert.ToDouble((Convert.ToDouble(Convert_Into_DF(((TextBox)Row.FindControl("lblTaxValue")).Text))) * Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim()));
                netPrice += Convert.ToDouble(((Label)Row.FindControl("lblAmount")).Text.Trim());
            }
        }
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text = SetDecimal(netPrice.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalTaxPrice")).Text = SetDecimal(netTax.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalDiscountPrice")).Text = SetDecimal(netDiscount.ToString());
        }
        catch
        {
        }
    }
    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        //DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
        DataTable dtSupplier = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["EProductName"].ToString();
                }
            }
            return str;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }
    #endregion
    #endregion
    #region User Defined Function
    public string CurrencyName(string CurrencyId)
    {
        string CurrencyName = string.Empty;
        using (DataTable dt = ObjCurrencyMaster.GetCurrencyMasterById(CurrencyId.ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                CurrencyName = dt.Rows[0]["Currency_Name"].ToString();
            }
            else
            {
                CurrencyName = "0";
            }
        }
        return CurrencyName;
    }
    public string GetExpName(string ExpId)
    {
        return (ObjShipExp.GetShipExpMasterById(StrCompId, ExpId)).Rows[0]["Exp_Name"].ToString();
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        using (DataTable dt = objUnit.GetUnitMasterById(StrCompId.ToString(), UnitId.ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                UnitName = dt.Rows[0]["Unit_Name"].ToString();
            }
        }
        return UnitName;
    }
    private void FillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '" + txtValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id in (" + ddlLocList.SelectedValue + ") and isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            strWhereClause += " and Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            strWhereClause += " and Post='False'";
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = ObjPurchaseInvoice.getInvoiceList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseInvoice.Attributes["CurrentSortField"], GvPurchaseInvoice.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseInvoice, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseInvoice.DataSource = null;
                GvPurchaseInvoice.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
    }
    public void FillCurrency(DropDownList ddlCurrency)
    {
        try
        {
            using (DataTable dt = ObjCurrencyMaster.GetCurrencyMaster())
            {
                objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            }
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
        }
    }
    public void CostingRate()
    {
        double TotExp = 0;
        try
        {
            TotExp = Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text);
        }
        catch
        {
            TotExp = 0;
        }
        foreach (GridViewRow gvrow in gvOrderExpenses.Rows)
        {
            try
            {
                TotExp += Convert.ToDouble(((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
            }
            catch
            {
            }
        }
        // txtGrandTotal.Text = (Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtNetDisVal.Text.Trim())).ToString();
        try
        {
            if (txtBillAmount.Text == "" || txtBillAmount.Text == "0")
            {
                txtBillAmount.Text = txtGrandTotal.Text;
            }
            double Cost = 0;
            Cost = (Convert.ToDouble(txtBillAmount.Text.Trim()) * Convert.ToDouble(txtExchangeRate.Text.Trim()) + TotExp);
            if ((Cost > 0) && (txtBillAmount.Text.ToString() != "0"))
            {
                txtCostingRate.Text = (Cost / Convert.ToDouble(txtBillAmount.Text.Trim())).ToString();
            }
            else
            {
                txtCostingRate.Text = "0";
            }
        }
        catch
        {
            txtCostingRate.Text = "0";
        }
        double d = 0;
        try
        {
            foreach (GridViewRow Row in gvProduct.Rows)
            {
                ((Label)Row.FindControl("lblUnitRateLocal")).Text = GetLocalPrice((Convert.ToDouble(((TextBox)Row.FindControl("lblUnitRate")).Text.Trim()) - Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim()) + Convert.ToDouble(Convert_Into_DF(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim()))).ToString()).ToString();
                ((Label)Row.FindControl("lblLocalQtyAmmount")).Text = GetLocalCurrencyConversion((Convert.ToDouble(((Label)Row.FindControl("lblUnitRateLocal")).Text) * Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text)).ToString());
                try
                {
                    d += Convert.ToDouble(((Label)Row.FindControl("lblLocalQtyAmmount")).Text);
                }
                catch
                {
                    d += 0;
                }
                if (((TextBox)Row.FindControl("lblTaxValue")).Text.Trim() == "")
                {
                    ((TextBox)Row.FindControl("lblTaxValues")).Text = Convert_Into_DF("0");
                }
                if (((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim() == "")
                {
                    ((TextBox)Row.FindControl("lblDiscountValue")).Text = "0";
                }
            }
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = GetLocalCurrencyConversion(d.ToString());
        }
        catch
        {
        }
        try
        {
            txtlocalGrandtotal.Text = GetLocalPrice(txtGrandTotal.Text);
        }
        catch
        {
            txtlocalGrandtotal.Text = "0";
        }
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
        //txtlocalGrandtotal.Text = SystemParameter.GetScaleAmount(GetLocalPrice(txtGrandTotal.Text.Trim()), "0");
    }
    protected void txtBillAmount_TextChanged(object sender, EventArgs e)
    {
        CostingRate();
        setDecimal();
    }
    public string GetLocalPrice(string Price)
    {
        try
        {
            return SetDecimal((Convert.ToDouble(txtCostingRate.Text) * Convert.ToDouble(Price)).ToString());
        }
        catch
        {
            return "0";
        }
    }
    public string GetLocalCurrencyConversion(string Price)
    {
        try
        {

            return SetDecimal(Price);
        }
        catch
        {
            return "0";
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }
    public void Reset()
    {
        HdnEdit.Value = "";
        rbtNew.Checked = false;
        rbtNew.Visible = false;
        rbtEdit.Checked = true;
        rbtEdit.Visible = false;
        ddlTransType.Enabled = true;
        ddlCurrency.Enabled = true;
        txtExchangeRate.Enabled = true;
        Session["Expenses_Tax_Purchase_Invoice"] = null;
        Txt_Narration.Text = "";
        RdoPo.Enabled = true;
        RdoWithOutPo.Enabled = true;
        PnlProductSearching.Visible = false;
        txtInvoicedate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtSupInvoiceDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ddlCurrency.SelectedIndex = 0;
        ddlExpCurrency.SelectedIndex = 0;
        //FillCurrency(ddlExpCurrency);
        txtInvoiceNo.Text = "";
        ddlInvoiceType.SelectedIndex = 0;
        txtSupplierInvoiceNo.Text = "";
        txtSupplierName.Text = "";
        txtNetAmount.Text = "0";
        txtNetDisPer.Text = "0";
        txtNetDisVal.Text = "0";
        txtNetTaxPar.Text = "0";
        txtNetTaxVal.Text = Convert_Into_DF("0");
        txtGrandTotal.Text = "0";
        txtOtherCharges.Text = "0";
        txtExchangeRate.Text = "";
        txRemark.Text = "";
        btnPost.Visible = false;
        fillPaymentMode();
        HdnEdit.Value = "";
        txtCostingRate.Text = "0";
        txtSupplierName.Text = "";
        ddlProductSerach.SelectedIndex = 1;
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        RdoPo.Checked = false;
        RdoWithOutPo.Checked = false;
        ViewState["Expdt"] = null;
        GridExpenses.DataSource = null;
        GridExpenses.DataBind();
        txtInvoiceNo.Enabled = true;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtInvoiceNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtInvoiceNo.Text;
        RdoPo.Enabled = true;
        RdoWithOutPo.Enabled = true;
        txtSupplierName.ReadOnly = false;
        ddlBillType.SelectedIndex = 0;
        try
        {
            ddlProductSerach.Items.RemoveAt(2);
        }
        catch
        {
        }
        ViewState["Dtproduct"] = null;
        ViewState["dtPo"] = null;
        Session["VPost"] = "False";
        ddlPaymentMode.SelectedIndex = 0;
        try
        {
            ddlPayBank.SelectedIndex = 0;
        }
        catch
        {

        }
        ViewState["PayementDt"] = null;
        btnPaymentReset_Click(null, null);
        gvPayment.DataSource = null;
        gvPayment.DataBind();
        if (Session["LocCurrencyId"].ToString() != "0")
        {
            ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
            TxtCurrencyValue.Text = Session["LocCurrencyId"].ToString();
            txtExchangeRate.Text = "1";
            ddlExpCurrency.SelectedValue = TxtCurrencyValue.Text;
            txtExpExchangeRate.Text = "1";
            hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
        }
        txtBillAmount.Text = "0";
        txtCostingRate.Text = "0";
        txtlocalGrandtotal.Text = "0";
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        Session["DtSearchProduct"] = null;
        txtSupplierName.Enabled = true;
        ViewState["DtTax"] = null;
        ViewState["dtTaxHeader"] = null;
        LoadStores();
        Session["DtAdvancePayment"] = null;
        Filladvancepaymentgrid((DataTable)Session["DtAdvancePayment"]);
        fillOrderExpGrid(new DataTable());
        txtdivideby.Text = "0";
        gvShippingInformation.DataSource = null;
        gvShippingInformation.DataBind();
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtShippingLine.Text = "";
        ddlShipBy.SelectedIndex = 0;
        ddlShipmentType.SelectedIndex = 0;
        ddlFreightStatus.SelectedIndex = 0;
        ddlShipUnit.SelectedIndex = 0;
        txtAirwaybillno.Text = "";
        txtvolumetricweight.Text = "";
        txtTotalWeight.Text = "";
        txtUnitRate.Text = "";
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txttotalvolumetricweight.Text = "0";
        txttotalVolume.Text = "0";
        Tabshippinginformation.Visible = false;
        tabContainer.ActiveTabIndex = 0;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        txtNetTaxPar.Enabled = false;
        btnSave.Enabled = true;
        btnPost.Enabled = true;
        btnReset.Visible = true;
    }
    #endregion
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "48", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #region DirectInvoiceGrid
    public void DirectGridFooterCalculation()
    {
        try
        {
            double d = 0;
            double q = 0;
            double K = 0;
            double tax = 0;
            double Net_tax_Percent = 0;
            double tax_Per = 0;
            double netprice_s = 0;
            double netprice = 0;
            double Tax_Count = 0;
            double discount = 0;
            double Qty = 0;
            double LocalUnitprice = 0;
            foreach (GridViewRow Row in gvProduct.Rows)
            {
                Qty = Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim());
                q += Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim());
                K += Convert.ToDouble(((TextBox)Row.FindControl("QtyGoodReceived")).Text.Trim());
                if (((Convert.ToDouble(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim())) * Qty).ToString() == "NaN")
                    tax_Per = 0;
                else
                {
                    double x = (Convert.ToDouble(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim()) * Qty);
                    tax_Per = Convert.ToDouble(Convert_Into_DF(x.ToString()));
                }

                tax += tax_Per;
                discount += Convert.ToDouble((Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim())) * Qty);
                d += Convert.ToDouble(((Label)Row.FindControl("lblQtyAmmount")).Text.Trim());
                ((Label)Row.FindControl("lblAmount")).Text = SetDecimal((Convert.ToDouble(((Label)Row.FindControl("lblQtyAmmount")).Text.Trim()) - (Convert.ToDouble(((TextBox)Row.FindControl("lblDiscountValue")).Text.Trim()) * Qty) + (Convert.ToDouble(Convert_Into_DF(((TextBox)Row.FindControl("lblTaxValue")).Text.Trim())) * Qty)).ToString()).ToString();
                if (Convert.ToDouble((Convert.ToDouble(((Label)Row.FindControl("lblAmount")).Text.Trim()))).ToString() == "NaN")
                    netprice_s = 0;
                else
                    netprice_s = Convert.ToDouble((Convert.ToDouble(((Label)Row.FindControl("lblAmount")).Text.Trim())));
                netprice += netprice_s;
                LocalUnitprice += Convert.ToDouble((Convert.ToDouble(((Label)Row.FindControl("lblLocalQtyAmmount")).Text.Trim())));
                double Tax_Percent = (Convert.ToDouble(((TextBox)Row.FindControl("lblTax")).Text.Trim()));
                Net_tax_Percent = Net_tax_Percent + Tax_Percent;
                Tax_Count++;
            }
            ((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text = SetDecimal(d.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalQuantity")).Text = SetDecimal(q.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalTaxPrice")).Text = SetDecimal(tax.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalDiscountPrice")).Text = SetDecimal(discount.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text = SetDecimal(netprice.ToString());
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = GetLocalCurrencyConversion(LocalUnitprice.ToString());

            string[] TaxDiscountVal = Common.TaxDiscountCaluculation(d.ToString(), "0", "", Convert_Into_DF(tax.ToString()), "", discount.ToString(), false, ddlCurrency.SelectedValue.ToString(), false, Session["DBConnection"].ToString());
            txtNetDisPer.Text = TaxDiscountVal[1].ToString();
            txtNetDisVal.Text = SetDecimal(discount.ToString());
            txtNetTaxVal.Text = Convert_Into_DF(tax.ToString());
            txtNetTaxPar.Text = (Net_tax_Percent / Tax_Count).ToString();

            txtGrandTotal.Text = TaxDiscountVal[5].ToString();
            CalculationchangeIntaxGridview();
            txtBillAmount.Text = txtGrandTotal.Text;
            txtBillAmount_TextChanged(null, null);
        }
        catch
        {
        }
        setDecimal();
    }
    #region Product Form
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtSearchProductName.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!RdoPo.Checked)
                    {
                        txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                        txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();
                        string ReqId = ObjPurchaseInvoice.getAutoId();
                        DataTable dtProduct = new DataTable();
                        if (ViewState["Dtproduct"] == null)
                        {
                        }
                        else
                        {
                            dtProduct = (DataTable)ViewState["Dtproduct"];
                        }
                        btnProductSave_Click(null, null);
                    }
                }
                else
                {
                    DisplayMessage("No Product Found");
                    txtSearchProductName.Text = "";
                    txtSearchProductName.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchProductName);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchProductName);
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductId.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductId.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!RdoPo.Checked)
                    {
                        txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                        txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();
                        string ReqId = ObjPurchaseInvoice.getAutoId();
                        DataTable dtProduct = new DataTable();
                        if (ViewState["Dtproduct"] == null)
                        {
                        }
                        else
                        {
                            dtProduct = (DataTable)ViewState["Dtproduct"];
                        }

                        btnProductSave_Click(null, null);
                        txtProductId.Focus();
                    }
                }
                else
                {
                    DisplayMessage("No Product Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
    }
    protected void btnProductSave_Click(object sender, ImageClickEventArgs e)
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("TransID");
        DtProduct.Columns.Add("SerialNo");
        DtProduct.Columns.Add("PONo");
        DtProduct.Columns.Add("POID");
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
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string UnitCost = string.Empty;
        string SearchCriteria = string.Empty;
        if (ddlProductSerach.SelectedIndex == 0)
        {
            if (txtProductId.Text == "")
            {
                DisplayMessage("Enter Product Id");
                txtProductId.Focus();
                return;
            }
            SearchCriteria = txtProductId.Text;
        }
        if (ddlProductSerach.SelectedIndex == 1)
        {
            if (txtSearchProductName.Text == "")
            {
                DisplayMessage("Enter Product Name");
                txtSearchProductName.Focus();
                return;
            }
            SearchCriteria = txtSearchProductName.Text;
        }
        if (HdnEdit.Value == "")
        {
            ReqId = ObjPurchaseInvoice.getAutoId();
        }
        else
        {
            ReqId = HdnEdit.Value.ToString();
        }
        if (SearchCriteria != "")
        {
            DataTable dt = new DataTable();
            if (ddlProductSerach.SelectedIndex == 0)
            {
                dt = objDa.return_DataTable("select ProductCode,EProductName,ProductId,UnitId from inv_productmaster where Brand_Id=" + Session["BrandId"].ToString() + " and ProductCode = '" + txtProductId.Text.ToString().Trim() + "'");

            }
            if (ddlProductSerach.SelectedIndex == 1)
            {
                dt = objDa.return_DataTable("select ProductCode,EProductName,ProductId,UnitId from inv_productmaster where Brand_Id=" + Session["BrandId"].ToString() + " and EProductName = '" + txtSearchProductName.Text.ToString().Trim() + "'");
            }
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                ProductId = "0";
            }
            UnitId = dt.Rows[0]["UnitId"].ToString();
            //this code for get price according selected supplier
            //code start
            if (txtSupplierName.Text != "")
            {
                DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S");
                try
                {
                    dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtContactPriceList.Rows.Count > 0)
                {
                    UnitCost = SetDecimal(dtContactPriceList.Rows[0]["Sales_Price"].ToString());
                    try
                    {
                        UnitCost = (Convert.ToDouble(UnitCost) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                        UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                        UnitCost = SetDecimal(UnitCost);
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
                        UnitCost = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                        UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                        UnitCost = SetDecimal(UnitCost);
                    }
                    catch
                    {
                        UnitCost = "0";
                    }
                }
            }
            else
            {
                //if supplier not selecte dthen we will get last price according selected product 
                try
                {
                    UnitCost = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                    UnitCost = (Convert.ToDouble(UnitCost) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    UnitCost = SetDecimal(UnitCost);
                }
                catch
                {
                    UnitCost = "0";
                }
            }

        }
        int SerialNO = 0;
        //DataTable dtProduct = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, ReqId.Trim());
        DataTable dtProduct = new DataTable();
        if (ViewState["Dtproduct"] == null)
        {
        }
        else
        {
            dtProduct = (DataTable)ViewState["Dtproduct"];
        }
        if (dtProduct.Rows.Count > 0)
        {
            dtProduct = new DataView(dtProduct, "", "SerialNo Desc", DataViewRowState.CurrentRows).ToTable();
            SerialNO = Convert.ToInt32(dtProduct.Rows[0]["SerialNo"].ToString());
            SerialNO += 1;
        }
        else
        {
            SerialNO = 1;
        }
        DataRow dr;
        if (ViewState["Dtproduct"] == null)
        {
            dr = DtProduct.NewRow();
        }
        else
        {
            DtProduct = GetGridRecirdInDatattable();
            dr = DtProduct.NewRow();
        }
        Add_Tax_In_Session(UnitCost, "0", ProductId.ToString(), SerialNO.ToString());
        dr["TransID"] = DtProduct.Rows.Count + 1;
        dr["SerialNo"] = SerialNO.ToString();
        dr["PONo"] = "0";
        dr["POID"] = "0";
        dr["ProductId"] = ProductId.ToString();
        dr["UnitId"] = UnitId.ToString();
        dr["UnitCost"] = UnitCost;
        dr["OrderQty"] = "1";
        if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "NS")
        {
            dr["RecQty"] = "1";
        }
        else
        {
            dr["RecQty"] = "0";
        }
        dr["FreeQty"] = "0";
        dr["InvoiceQty"] = "1";
        dr["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), SerialNO.ToString()).ToString();
        dr["DiscountV"] = "0";
        dr["DiscountP"] = "0";
        dr["TaxV"] = Get_Tax_Amount(UnitCost, ProductId.ToString(), SerialNO.ToString());
        dr["InvRemainQty"] = "1";
        dr["RemainQty"] = "1";
        DtProduct.Rows.Add(dr);
        ViewState["Dtproduct"] = (DataTable)DtProduct;
        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        GetChildGridRecordInViewState();
        ViewState["Dtproduct"] = DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        gvProduct.Columns[16].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[17].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[9].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GridExpenses.Columns[10].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        gvProduct.Columns[14].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        gvProduct.Columns[15].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DirectGridFooterCalculation();
        if (gvProduct.Rows.Count > 0)
        {
            ddlTransType.Enabled = false;
            ddlCurrency.Enabled = false;
            txtExchangeRate.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
            ddlCurrency.Enabled = true;
            txtExchangeRate.Enabled = true;
        }
    }
    protected void ddlProductSerach_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdoWithOutPo.Checked)
        {
            txtSearchProductName.Text = "";
            txtProductId.Text = "";
            if (ddlProductSerach.SelectedIndex == 0)
            {
                txtProductId.Visible = true;
                txtSearchProductName.Visible = false;
            }
            else
            {
                txtProductId.Visible = false;
                txtSearchProductName.Visible = true;
            }
        }
    }
    protected void lblGvUnitRate_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblSerialNO = (Label)Row.FindControl("lblSerialNO");
        TextBox TxtgvQty = (TextBox)Row.FindControl("QtyGoodReceived");
        TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)sender;
        Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
        Label lblgvProductId = (Label)Row.FindControl("lblgvProductId");
        if (TxtgvQty.Text == "")
        {
            TxtgvQty.Text = "0";
        }
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        if (TxtgvDiscountValue.Text == "")
        {
            TxtgvDiscountValue.Text = "0";
        }
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        Add_Tax_In_Session(txtUnitCost.Text, TxtgvDiscountValue.Text, lblgvProductId.Text, lblSerialNO.Text);
        double PriceValue = double.Parse(txtUnitCost.Text);
        double QtyValue = double.Parse(TxtgvQty.Text);
        double AmountValue = PriceValue * QtyValue;
        double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;

        if (AmountValue != AmntAfterDiscnt & Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
        if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            AmntAfterDiscnt = AmountValue;
        double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
        TotalTax = TotalTax / QtyValue;
        double DiscountPercentage = (double.Parse(TxtgvDiscountValue.Text) * 100 / AmountValue);
        string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
        lblQtyAmmount.Text = strvalue[0].ToString();
        TxtgvDiscountValue.Text = strvalue[2].ToString();
        TxtgvTaxValue.Text = Convert_Into_DF(TotalTax.ToString());
        TxtgvTax.Text = Get_Tax_Percentage(lblgvProductId.Text, lblSerialNO.Text).ToString();
        // TxtgvTaxValue.Text = strvalue[4].ToString();
        lblgvNetAmount.Text = strvalue[5].ToString();
        ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
        DirectGridFooterCalculation();
        if (TxtgvDiscountValue.Text == "")
        {
            TxtgvDiscountValue.Text = "0";
        }
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        GridView GridChild = (GridView)Row.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        TaxCalculationWithDiscount();
    }
    protected void lblOrderQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblSerialNO = (Label)Row.FindControl("lblSerialNO");
        TextBox TxtgvQty = (TextBox)sender;
        Label OrderQty = (Label)Row.FindControl("OrderQty");
        TextBox QtyReceived = (TextBox)Row.FindControl("QtyGoodReceived");
        TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
        Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
        Label lblgvProductId = (Label)Row.FindControl("lblgvProductId");
        if (TxtgvQty.Text == "")
        {
            TxtgvQty.Text = "0";
        }
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        Add_Tax_In_Session(txtUnitCost.Text, TxtgvDiscountValue.Text, lblgvProductId.Text, lblSerialNO.Text);
        double PriceValue = double.Parse(txtUnitCost.Text);
        double QtyValue = double.Parse(TxtgvQty.Text);
        double AmountValue = PriceValue * QtyValue;
        double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
        bool IsValidDiscount = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
        if (!IsValidDiscount)
            AmntAfterDiscnt = AmountValue;
        double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
        TotalTax = TotalTax / QtyValue;
        string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
        lblQtyAmmount.Text = strvalue[0].ToString();
        TxtgvDiscountValue.Text = strvalue[2].ToString();
        if (QtyReceived.Text != "0.00" && QtyReceived.Text != "" && QtyReceived.Text != "0")
            TxtgvTaxValue.Text = Convert_Into_DF(TotalTax.ToString());
        else
            TxtgvTaxValue.Text = Convert_Into_DF("0.00");
        // TxtgvTaxValue.Text = strvalue[4].ToString();
        lblgvNetAmount.Text = strvalue[5].ToString();
        ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
        if (!RdoPo.Checked)
        {
            QtyReceived.Text = ((TextBox)sender).Text;
            OrderQty.Text = ((TextBox)sender).Text;
        }
        if (TxtgvDiscountValue.Text == "")
        {
            TxtgvDiscountValue.Text = "0";
        }
        GridView GridChild = (GridView)Row.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        DirectGridFooterCalculation();
        TaxCalculationWithDiscount();
    }
    protected void lblDiscount_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblSerialNO = (Label)Row.FindControl("lblSerialNO");
        Label TxtgvQty = (Label)Row.FindControl("OrderQty");
        TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
        Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
        Label lblgvProductId = (Label)Row.FindControl("lblgvProductId");
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        if (TxtgvQty.Text != "0")
        {
            if (txtUnitCost.Text != "0")
            {
                double PriceValue = double.Parse(txtUnitCost.Text);
                double QtyValue = double.Parse(TxtgvQty.Text);
                double AmountValue = PriceValue * QtyValue;
                double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
                Add_Tax_In_Session(txtUnitCost.Text, ((AmountValue * double.Parse(TxtgvDiscount.Text)) / 100).ToString(), lblgvProductId.Text, lblSerialNO.Text);
                bool IsValidDiscount = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
                    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
                if (!IsValidDiscount)
                    AmntAfterDiscnt = AmountValue;
                double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
                TotalTax = TotalTax / QtyValue;
                string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", TxtgvDiscount.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                lblQtyAmmount.Text = strvalue[0].ToString();
                TxtgvDiscountValue.Text = strvalue[2].ToString();
                TxtgvTaxValue.Text = Convert_Into_DF(TotalTax.ToString());
                // TxtgvTaxValue.Text = strvalue[4].ToString();
                lblgvNetAmount.Text = strvalue[5].ToString();
                ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
                DirectGridFooterCalculation();
            }
        }
        if (TxtgvDiscountValue.Text == "")
        {
            TxtgvDiscountValue.Text = "0";
        }
        GridView GridChild = (GridView)Row.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        TaxCalculationWithDiscount();
    }
    protected void lblDiscountValue_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblSerialNO = (Label)Row.FindControl("lblSerialNO");
        Label TxtgvQty = (Label)Row.FindControl("OrderQty");
        TextBox TxtgvDiscount = (TextBox)Row.FindControl("lblDiscount");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
        Label lblQtyAmmount = (Label)Row.FindControl("lblQtyAmmount");
        Label lblgvProductId = (Label)Row.FindControl("lblgvProductId");
        if (txtUnitCost.Text == "")
        {
            txtUnitCost.Text = "0";
        }
        if (TxtgvQty.Text != "0")
        {
            if (txtUnitCost.Text != "0")
            {
                double PriceValue = double.Parse(txtUnitCost.Text);
                double QtyValue = double.Parse(TxtgvQty.Text);
                double AmountValue = PriceValue * QtyValue;
                double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
                Add_Tax_In_Session(txtUnitCost.Text, ((AmountValue * double.Parse(TxtgvDiscount.Text)) / 100).ToString(), lblgvProductId.Text, lblSerialNO.Text);
                bool IsValidDiscount = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
                    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
                if (!IsValidDiscount)
                    AmntAfterDiscnt = AmountValue;
                double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
                TotalTax = TotalTax / QtyValue;
                string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", "", TxtgvDiscountValue.Text, true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                lblQtyAmmount.Text = strvalue[0].ToString();
                TxtgvDiscount.Text = strvalue[1].ToString();
                TxtgvTaxValue.Text = Convert_Into_DF(TotalTax.ToString());
                // TxtgvTaxValue.Text = strvalue[4].ToString();
                lblgvNetAmount.Text = strvalue[5].ToString();
                ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
                DirectGridFooterCalculation();
            }
        }
        if (TxtgvDiscountValue.Text == "")
        {
            TxtgvDiscountValue.Text = "0";
        }
        GridView GridChild = (GridView)Row.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        TaxCalculationWithDiscount();
    }
    protected void lblTax_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label TxtgvQty = (Label)Row.FindControl("OrderQty");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
        if (TxtgvQty.Text != "0")
        {
            if (txtUnitCost.Text != "0")
            {
                string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, TxtgvTax.Text, "", "", TxtgvDiscountValue.Text, true, ddlCurrency.SelectedValue, true, Session["DBConnection"].ToString());
                TxtgvTaxValue.Text = Convert_Into_DF(strvalue[4].ToString());
                lblgvNetAmount.Text = strvalue[5].ToString();
                ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
                DirectGridFooterCalculation();
            }
        }
    }
    protected void lblTaxValue_TextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label TxtgvQty = (Label)Row.FindControl("OrderQty");
        TextBox TxtgvDiscountValue = (TextBox)Row.FindControl("lblDiscountValue");
        TextBox TxtgvTax = (TextBox)Row.FindControl("lblTax");
        TextBox TxtgvTaxValue = (TextBox)Row.FindControl("lblTaxValue");
        Label lblgvNetAmount = (Label)Row.FindControl("lblAmount");
        TextBox txtUnitCost = (TextBox)Row.FindControl("lblUnitRate");
        if (TxtgvQty.Text != "0")
        {
            if (txtUnitCost.Text != "0")
            {
                string[] strvalue = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, "", Convert_Into_DF(TxtgvTaxValue.Text), "", TxtgvDiscountValue.Text, true, ddlCurrency.SelectedValue, true, Session["DBConnection"].ToString());
                TxtgvTax.Text = strvalue[3].ToString();
                lblgvNetAmount.Text = strvalue[5].ToString();
                ((TextBox)sender).Text = SetDecimal(((TextBox)sender).Text);
                DirectGridFooterCalculation();
            }
        }
    }
    #endregion
    #endregion
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        txtSupplierName.Enabled = true;
        btnEdit_Command(sender, e);
    }
    #endregion
    #region Expenses
    public void fillExpenses()
    {
        using (DataTable dt = ObjShipExp.GetShipExpMaster(StrCompId.ToString()))
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");
        }
    }
    protected void ddlExpense_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpense.SelectedValue == "--Select--")
        {
            txtExpensesAccount.Text = "";
        }
        else if (ddlExpense.SelectedValue != "--Select--")
        {
            using (DataTable dtExp = ObjShipExp.GetShipExpMasterById(StrCompId, ddlExpense.SelectedValue))
            {
                if (dtExp.Rows.Count > 0)
                {
                    string strAccountId = dtExp.Rows[0]["Account_No"].ToString();
                    DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                    if (dtAcc.Rows.Count > 0)
                    {
                        string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                        txtExpensesAccount.Text = strAccountName + "/" + strAccountId;
                    }
                }
            }
        }
        // GetData();
    }
    protected void txtExpensesAccount_TextChanged(object sender, EventArgs e)
    {
        if (txtExpensesAccount.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
            try
            {
                dtAccount = new DataView(dtAccount, "AccountName='" + txtExpensesAccount.Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                    txtExpensesAccount.Text = "";
                    DisplayMessage("No Account Found");
                    txtExpensesAccount.Focus();
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                txtExpensesAccount.Text = "";
                DisplayMessage("No Account Found");
                txtExpensesAccount.Focus();
            }
            dtAccount = null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        try
        {
            Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
            dt = new DataView(dt, "AccountName Like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                }
            }
            dt = null;
            return str;
        }
        catch
        {
            return null;
        }
    }
    #endregion
    #region Payment
    public void fillPaymentMode()
    {
        try
        {
            using (DataTable dt = ObjPaymentMaster.GetPaymentModeMaster(StrCompId.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlPaymentMode, dt, "Pay_Mod_Name", "Pay_Mode_Id");
            }
        }
        catch
        {
            ddlPaymentMode.Items.Insert(0, "--Select--");
            ddlPaymentMode.SelectedIndex = 0;
        }
    }
    protected void btnPaymentReset_Click(object sender, EventArgs e)
    {
        txtPayAccountNo.Text = "";
        txtFCPayCharges.Text = "";
        txtLCPayCharges.Text = "";
        ddlPayCurrency.SelectedIndex = 0;
        txtPayExchangeRate.Text = "";
        txtPayCardName.Text = "";
        txtPayChequeNo.Text = "";
        txtPayCardNo.Text = "";
        txtPayChequeDate.Text = "";
        //fillBank();
        try
        {
            ddlPayBank.SelectedIndex = 0;
        }
        catch
        {

        }
        trcheque.Visible = false;
        trcard.Visible = false;
        lblPayBank.Visible = false;
        //lblpaybankcolon.Visible = false;
        ddlPayBank.Visible = false;
        ddlPayCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        txtPayExchangeRate.Text = "1";
    }
    public void fillBank()
    {
        using (DataTable dt = ObjBankMaster.GetBankMasterForDDL())
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlPayBank, dt, "Bank_Name", "Bank_Id");
        }
    }
    public void fillPaymentGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");
        ViewState["PayementDt"] = dt;
        if (dt != null && dt.Rows.Count > 0)
        {
            Txt_Narration.Text = dt.Rows[0]["Field2"].ToString();
            if (dt.Rows[0]["Field1"].ToString() != "")
            {
                Txt_Narration.Text = dt.Rows[0]["Field2"].ToString();
                DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
                try
                {
                    dtAccount = new DataView(dtAccount, "Trans_ID='" + dt.Rows[0]["Field1"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                    txtPurchaseAccount.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + dtAccount.Rows[0]["Trans_ID"].ToString();
                    Session["AccountId"] = dtAccount.Rows[0]["Trans_ID"].ToString();
                }
                catch
                {
                    txtPurchaseAccount.Text = "";
                }
            }
        }
        double f = 0;
        double fc = 0;
        string ForeignCurrency = string.Empty;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            try
            {
                ForeignCurrency = ((Label)gvrow.FindControl("hidExpCur")).Text;
                ((Label)gvrow.FindControl("lblgvExp_Charges")).Text = SetDecimal(((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
                ((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text = SetDecimal(((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text);

                f += Convert.ToDouble(((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
                fc += Convert.ToDouble(((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text);
            }
            catch
            {
                ((Label)gvrow.FindControl("lblgvExp_Charges")).Text = "0";
                ((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text = "0";
                f += 0;
                fc += 0;
            }
        }
        if (ForeignCurrency == "")
        {
            ForeignCurrency = Session["LocCurrencyId"].ToString();
        }
        try
        {
            ((Label)gvPayment.FooterRow.FindControl("txttotExpShow")).Text = SetDecimal(f.ToString());
            ((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text = SetDecimal(fc.ToString());
        }
        catch
        {
        }
        dt = null;
    }
    //protected void txtPayAccountNo_TextChanged(object sender, EventArgs e)
    //{
    //    if (((TextBox)sender).Text != "")
    //    {
    //        DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
    //        try
    //        {
    //            dtAccount = new DataView(dtAccount, "AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
    //            if (dtAccount.Rows.Count == 0)
    //            {
    //                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
    //                ((TextBox)sender).Text = "";
    //                DisplayMessage("No Account Found");
    //                ((TextBox)sender).Focus();
    //                return;
    //            }
    //        }
    //        catch
    //        {
    //            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
    //            ((TextBox)sender).Text = "";
    //            DisplayMessage("No Account Found");
    //            ((TextBox)sender).Focus();
    //            return;
    //        }
    //        dtAccount = null;
    //    }
    //    if (((TextBox)sender).ID == "txtPurchaseAccount")
    //    {
    //        Session["AccountId"] = ((TextBox)sender).Text.Split('/')[1].ToString();
    //    }
    //}
    protected void btnDeletePay_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)ViewState["PayementDt"], "TransId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        fillPaymentGrid(dt);
        dt = null;
        //here we change balance amount when we paid against the invoice amount
    }
    public void GetBalanceAmount()
    {
        string strLGT = string.Empty;
        if (txtlocalGrandtotal.Text != "")
        {
            if (Convert.ToDouble(txtlocalGrandtotal.Text) > 0)
            {
                //Local Grand Total
                strLGT = txtlocalGrandtotal.Text;
                double fOrderExp = 0;
                foreach (GridViewRow gvr in gvOrderExpenses.Rows)
                {
                    Label lblExpCharge = (Label)gvr.FindControl("lblgvExp_Charges");
                    if (lblExpCharge.Text != "" && lblExpCharge.Text != "0")
                    {
                        fOrderExp = fOrderExp + Convert.ToDouble(lblExpCharge.Text);
                    }
                }
                double Exp = 0;
                double SupplierExp = 0;
                string strPaymentVoucherAcc = string.Empty;
                DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
                DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtPaymentVoucher.Rows.Count > 0)
                {
                    strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
                }
                DataTable dtExpPay = (DataTable)ViewState["Expdt"];
                if (dtExpPay != null)
                {
                    foreach (DataRow dr in ((DataTable)ViewState["Expdt"]).Rows)
                    {
                        string strExpAmount = dr["Exp_Charges"].ToString();
                        string strAccountNo = dr["Account_No"].ToString();
                        Exp = Exp + Convert.ToDouble(dr["Exp_Charges"].ToString());
                        if (strAccountNo == strPaymentVoucherAcc)
                        {
                            SupplierExp += double.Parse(strExpAmount);
                        }
                    }
                }
                strLGT = (Convert.ToDouble(strLGT) - fOrderExp).ToString();
                strLGT = (Convert.ToDouble(strLGT) - Exp).ToString();
                strLGT = (Convert.ToDouble(strLGT) + SupplierExp).ToString();
                if (gvPayment.Rows.Count > 0)
                {
                    txtFCPayCharges.Text = SetDecimal((Convert.ToDouble(strLGT) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotExpShow")).Text)).ToString());
                    if (gvadvancepayment.Rows.Count > 0)
                    {
                        if ((Convert.ToDouble(strLGT) - (Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) + Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text))) > 0)
                        {
                            txtFCPayCharges.Text = SetDecimal((Convert.ToDouble(strLGT) - (Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) + Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text))).ToString());
                        }
                        else
                        {
                            txtFCPayCharges.Text = "0";
                        }
                    }
                }
                else
                {
                    if (gvadvancepayment.Rows.Count > 0)
                    {
                        if ((Convert.ToDouble(strLGT) - Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text)) > 0)
                        {
                            txtFCPayCharges.Text = SetDecimal((Convert.ToDouble(strLGT) - Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                        }
                        else
                        {
                            txtFCPayCharges.Text = SetDecimal((Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) - Convert.ToDouble(strLGT)).ToString());
                        }
                    }
                    else
                    {
                        txtFCPayCharges.Text = strLGT;
                        txtFCPayCharges.Text = SetDecimal(strLGT);
                    }
                }
            }
        }
        txtFCPayCharges_TextChanged(null, null);
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPaymentReset_Click(null, null);
        ddlPayCurrency.Enabled = false;
        if (ddlPaymentMode.SelectedValue == "--Select--")
        {
            txtPayAccountNo.Text = "";
        }
        else if (ddlPaymentMode.SelectedValue != "--Select--")
        {
            DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, ddlPaymentMode.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtPay.Rows.Count > 0)
            {
                string strAccountId = string.Empty;
                if (dtPay.Rows[0]["Field1"].ToString() == "Cash")
                {
                    strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                    using (DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId))
                    {
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                        }
                    }
                }
                else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                {
                    DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
                    using (DataTable dtPayVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (dtPayVoucher.Rows.Count > 0)
                        {
                            strAccountId = dtPayVoucher.Rows[0]["Param_Value"].ToString();
                        }
                    }
                    using (DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId))
                    {
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                        }
                    }
                }
            }
        }
        ddlPayCurrency.SelectedValue = ddlCurrency.SelectedValue;
        txtPayExchangeRate.Text = txtExchangeRate.Text;

        if (ddlPaymentMode.SelectedIndex != 0)
        {
            if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Cash")
            {
                trBank.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Credit")
            {
                trBank.Visible = true;
                lblPayBank.Visible = true;
                //lblpaybankcolon.Visible = true;
                ddlPayBank.Visible = true;
                trcheque.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Card")
            {
                trBank.Visible = true;
                trcard.Visible = true;
            }
        }
        if (txtGrandTotal.Text != "")
        {
            if (Convert.ToDouble(txtGrandTotal.Text) > 0)
            {
                if (gvPayment.Rows.Count > 0)
                {
                    txtFCPayCharges.Text = SetDecimal((Convert.ToDouble(txtGrandTotal.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text)).ToString());
                }
                else
                {
                    txtFCPayCharges.Text = txtGrandTotal.Text;
                }
                txtFCPayCharges_TextChanged(null, null);
            }
        }
        //if (gvPayment.Rows.Count == 0)
        //{
        //    //txtLCPayCharges.Text = txtlocalGrandtotal.Text;
        //}
    }
    private string GetAccountId(string strAccountName)
    {
        string retval = string.Empty;
        if (strAccountName != "")
        {
            retval = (strAccountName.Split('/'))[strAccountName.Split('/').Length - 1];
            using (DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, retval))
            {
                if (dtAccount.Rows.Count > 0)
                {
                }
                else
                {
                    retval = "";
                }
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    protected void txtFCPayCharges_TextChanged(object sender, EventArgs e)
    {
        if (txtPayExchangeRate.Text == "")
        {
            txtPayExchangeRate.Text = "0";
        }
        if (txtFCPayCharges.Text == "")
        {
            txtFCPayCharges.Text = "0";
        }
        txtLCPayCharges.Text = GetLocalCurrencyConversion((Convert.ToDouble(txtFCPayCharges.Text.Trim()) * Convert.ToDouble(txtPayExchangeRate.Text.Trim())).ToString());
    }
    protected void ddlPayCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayCurrency.SelectedIndex != 0)
        {
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            txtPayExchangeRate.Text = SystemParameter.GetExchageRate(ddlPayCurrency.SelectedValue, Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            if (txtPayExchangeRate.Text != "" && txtFCPayCharges.Text != "")
            {
                txtLCPayCharges.Text = txtlocalGrandtotal.Text;
            }
        }
    }
    #endregion
    #region common
    public string GetAmountDecimal(string Amount)
    {

        return SetDecimal(Amount);

    }

    public void setDecimal()
    {

        double LocalUnitprice = 0;
        foreach (GridViewRow ChRow in gvProduct.Rows)
        {
            ((TextBox)ChRow.FindControl("lblUnitRate")).Text = SetDecimal(((TextBox)ChRow.FindControl("lblUnitRate")).Text.Trim());
            ((Label)ChRow.FindControl("lblQtyAmmount")).Text = SetDecimal(((Label)ChRow.FindControl("lblQtyAmmount")).Text);
            ((Label)ChRow.FindControl("lblInvRemainQty")).Text = SetDecimal(((Label)ChRow.FindControl("lblInvRemainQty")).Text);
            string good_receive = (((Label)ChRow.FindControl("lblgoodsreceive")).Text).ToString();
            if (good_receive == "")
                good_receive = "0";
            ((Label)ChRow.FindControl("lblgoodsreceive")).Text = (Convert.ToDouble(good_receive)).ToString();
            ((Label)ChRow.FindControl("lblRemainQty")).Text = SetDecimal(((Label)ChRow.FindControl("lblRemainQty")).Text);
            ((TextBox)ChRow.FindControl("lblTaxValue")).Text = Convert_Into_DF(((TextBox)ChRow.FindControl("lblTaxValue")).Text, 3);
            ((Label)ChRow.FindControl("lblTaxAfterPrice")).Text = SetDecimal(((Label)ChRow.FindControl("lblTaxAfterPrice")).Text);
            ((TextBox)ChRow.FindControl("lblDiscountValue")).Text = SetDecimal(((TextBox)ChRow.FindControl("lblDiscountValue")).Text);
            ((Label)ChRow.FindControl("lblAmount")).Text = SetDecimal(((Label)ChRow.FindControl("lblAmount")).Text);
            ((TextBox)ChRow.FindControl("lblTax")).Text = SetDecimal(((TextBox)ChRow.FindControl("lblTax")).Text);
            ((TextBox)ChRow.FindControl("lblDiscount")).Text = SetDecimal(((TextBox)ChRow.FindControl("lblDiscount")).Text);
            ((Label)ChRow.FindControl("OrderQty")).Text = SetDecimal(Convert.ToDouble(((Label)ChRow.FindControl("OrderQty")).Text).ToString()).ToString();
            ((TextBox)ChRow.FindControl("lblFreeQty")).Text = SetDecimal((Convert.ToDouble(((TextBox)ChRow.FindControl("lblFreeQty")).Text)).ToString()).ToString();
            ((TextBox)ChRow.FindControl("QtyGoodReceived")).Text = (Convert.ToDouble(((TextBox)ChRow.FindControl("QtyGoodReceived")).Text).ToString()).ToString();
            LocalUnitprice += Convert.ToDouble((Convert.ToDouble(((Label)ChRow.FindControl("lblLocalQtyAmmount")).Text.Trim())));
        }
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text = SetDecimal(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text);
            ((Label)gvProduct.FooterRow.FindControl("txtTotalTaxPrice")).Text = SetDecimal(((Label)gvProduct.FooterRow.FindControl("txtTotalTaxPrice")).Text);
            ((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text = SetDecimal(((Label)gvProduct.FooterRow.FindControl("txtTotalNetPrice")).Text);
            ((Label)gvProduct.FooterRow.FindControl("txtTotalDiscountPrice")).Text = SetDecimal(((Label)gvProduct.FooterRow.FindControl("txtTotalDiscountPrice")).Text);
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = GetLocalCurrencyConversion(LocalUnitprice.ToString());
        }
        catch
        {

        }
        try
        {
            gvProduct.Columns[6].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
            gvProduct.Columns[7].HeaderText = SystemParameter.GetCurrencySmbol(ViewState["CurrenyId"].ToString(), Resources.Attendance.Cost, Session["DBConnection"].ToString());
            gvProduct.Columns[12].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Price, Session["DBConnection"].ToString());
            gvProduct.Columns[13].HeaderText = SystemParameter.GetCurrencySmbol(ViewState["CurrenyId"].ToString(), Resources.Attendance.Cost, Session["DBConnection"].ToString());
            gvProduct.Columns[15].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
            gvProduct.Columns[17].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Value, Session["DBConnection"].ToString());
            gvProduct.Columns[18].HeaderText = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Net_Price, Session["DBConnection"].ToString());
        }
        catch { }
        txtGrandTotal.Text = SetDecimal(txtGrandTotal.Text);
        txtNetAmount.Text = SetDecimal(txtNetAmount.Text);
        txtNetDisVal.Text = SetDecimal(txtNetDisVal.Text);
        txtNetTaxVal.Text = Convert_Into_DF(txtNetTaxVal.Text);
        txtOtherCharges.Text = SetDecimal(txtOtherCharges.Text);
        txtNetTaxPar.Text = SetDecimal(txtNetTaxPar.Text);
        txtNetDisPer.Text = SetDecimal(txtNetDisPer.Text);
        CostingRate();
        txtlocalGrandtotal.Text = GetLocalPrice(txtGrandTotal.Text);
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
        //txtBillAmount.Text = txtGrandTotal.Text;
        //txtBillAmount_TextChanged(null, null);
    }
    protected string GetSupplierName(string strSupplierId)
    {
        string strSupplierName = string.Empty;
        if (strSupplierId != "0" && strSupplierId != "")
        {
            DataTable dtSName = ObjContactMaster.GetContactTrueById(strSupplierId);
            if (dtSName.Rows.Count > 0)
            {
                strSupplierName = dtSName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strSupplierName = "";
        }
        return strSupplierName;
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }
        return Amountwithsymbol;
    }
    #endregion
    protected void txtFCExpAmount_TextChanged(object sender, EventArgs e)
    {
        if (txtFCExpAmount.Text != "")
        {
            if (txtExpExchangeRate.Text == "")
            {
                txtExpExchangeRate.Text = "0";
            }
            txtExpCharges.Text = SetDecimal((Convert.ToDouble(txtFCExpAmount.Text) * Convert.ToDouble(txtExpExchangeRate.Text)).ToString());
        }
    }
    #region PrintReport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        PrintReport(e.CommandArgument.ToString());
    }
    void PrintReport(string InvoiceID)
    {
        string strCmd = string.Format("window.open('../Purchase/PurchaseInvoicePrint.aspx?Id=" + InvoiceID.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldName.SelectedItem.Value == "InvoiceDate") || (ddlFieldName.SelectedItem.Value == "SupInvoiceDate"))
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
    }
    #endregion
    #region Advance Search
    public DataTable CreateProductDataTable()
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("TransID");
        DtProduct.Columns.Add("SerialNo");
        DtProduct.Columns.Add("PONo");
        DtProduct.Columns.Add("POID");
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
        return DtProduct;
    }
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        DataTable dt = GetGridRecirdInDatattable();
        ViewState["Dtproduct"] = dt;
        Session["DtSearchProduct"] = ViewState["Dtproduct"];
        GetChildGridRecordInViewState();
        //Session["DtSearchProduct"] = dt;
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=PIN&&SupId=" + txtSupplierName.Text.Split('/')[1].ToString() + "&&CurId=" + ddlCurrency.SelectedValue + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public DataTable GetGridRecirdInDatattable()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvr in gvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblPOId = (Label)gvr.FindControl("lblPOId");
            Label lblSerialNO = (Label)gvr.FindControl("lblSerialNO");
            Label lblPONo = (Label)gvr.FindControl("lblPONo");
            Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
            DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
            TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
            Label OrderQty = (Label)gvr.FindControl("OrderQty");
            TextBox lblFreeQty = (TextBox)gvr.FindControl("lblFreeQty");
            TextBox QtyReceived = (TextBox)gvr.FindControl("QtyReceived");
            TextBox QtyGoodReceived = (TextBox)gvr.FindControl("QtyGoodReceived");
            Label lblQtyAmmount = (Label)gvr.FindControl("lblQtyAmmount");
            TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
            TextBox lblTaxValue = (TextBox)gvr.FindControl("lblTaxValue");
            Label lblTaxAfterPrice = (Label)gvr.FindControl("lblTaxAfterPrice");
            TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
            TextBox lblDiscountValue = (TextBox)gvr.FindControl("lblDiscountValue");
            Label lblAmount = (Label)gvr.FindControl("lblAmount");
            Label lblInvRemainQty = (Label)gvr.FindControl("lblInvRemainQty");
            Label lblRemainQty = (Label)gvr.FindControl("lblRemainQty");
            Label lblgoodsreceive = (Label)gvr.FindControl("lblgoodsreceive");
            Label lblgvExpiryDate = (Label)gvr.FindControl("lblgvExpiryDate");
            Label lblgvBatchNo = (Label)gvr.FindControl("lblgvBatchNo");

            dr["SerialNo"] = lblSerialNO.Text;
            dr["TransID"] = lblSerialNO.Text;
            strNewSNo = lblSerialNO.Text;
            dr["POID"] = lblPOId.Text;
            dr["PONO"] = lblPONo.Text;
            dr["ProductId"] = lblgvProductId.Text;
            dr["UnitId"] = ddlUnitName.SelectedValue;
            dr["UnitCost"] = lblUnitRate.Text;
            dr["OrderQty"] = OrderQty.Text;
            dr["InvRemainQty"] = lblInvRemainQty.Text;
            dr["RemainQty"] = lblRemainQty.Text;
            dr["FreeQty"] = lblFreeQty.Text;
            dr["InvoiceQty"] = QtyGoodReceived.Text;
            dr["TaxP"] = lblTax.Text;
            dr["TaxV"] = Convert_Into_DF(lblTaxValue.Text);
            dr["DiscountP"] = lblDiscount.Text;
            dr["DiscountV"] = lblDiscountValue.Text;
            dr["RecQty"] = lblgoodsreceive.Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            ViewState["Dtproduct"] = Session["DtSearchProduct"];
            if (ViewState["Dtproduct"] != null)
            {
                DataTable dtTempDt = ViewState["Dtproduct"] as DataTable;
                int Dis_Per = 0, Dis_Value = 0, Tax_Per = 0, Tax_Value = 0;
                foreach (DataColumn DTC in dtTempDt.Columns)
                {
                    if (DTC.ColumnName.ToString() == "DiscountP")
                    {
                        Dis_Per = 1;
                    }
                    else if (DTC.ColumnName.ToString() == "DiscountV")
                    {
                        Dis_Value = 1;
                    }
                    else if (DTC.ColumnName.ToString() == "TaxP")
                    {
                        Tax_Per = 1;
                    }
                    else if (DTC.ColumnName.ToString() == "TaxV")
                    {
                        Tax_Value = 1;
                    }
                }
                if (Dis_Per == 0)
                {
                    dtTempDt.Columns.Add("DiscountP");
                }
                if (Dis_Value == 0)
                {
                    dtTempDt.Columns.Add("DiscountV");
                }
                if (Tax_Per == 0)
                {
                    dtTempDt.Columns.Add("TaxP");
                }
                if (Tax_Value == 0)
                {
                    dtTempDt.Columns.Add("TaxV");
                }
                foreach (DataRow dt_Row in dtTempDt.Rows)
                {
                    Add_Tax_In_Session(dt_Row["UnitCost"].ToString(), dt_Row["DiscountV"].ToString(), dt_Row["ProductId"].ToString(), dt_Row["SerialNo"].ToString());
                    dt_Row["DiscountP"] = "0";
                    dt_Row["DiscountV"] = "0.00";
                    dt_Row["TaxP"] = Get_Tax_Percentage(dt_Row["ProductId"].ToString(), dt_Row["SerialNo"].ToString());
                    dt_Row["TaxV"] = Get_Tax_Amount(dt_Row["UnitCost"].ToString(), dt_Row["ProductId"].ToString(), dt_Row["SerialNo"].ToString());
                }
                ViewState["Dtproduct"] = dtTempDt;
                objPageCmn.FillData((object)gvProduct, (DataTable)ViewState["Dtproduct"], "", "");
            }


            gvProduct.Columns[14].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[15].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[16].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvProduct.Columns[17].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            GridExpenses.Columns[9].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            GridExpenses.Columns[10].Visible = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            DirectGridFooterCalculation();
            Session["DtSearchProduct"] = null;
        }
        else
        {
            DisplayMessage("Product Not Found");
            return;
        }
    }
    #endregion
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    public string getAccountname(string strAccountId)
    {
        string strAccountName = string.Empty;
        using (DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId))
        {
            if (dtAcc.Rows.Count > 0)
            {
                strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
            }
        }
        return strAccountName;
    }
    #region AddCustomer

    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
        {
            allow = true;
        }
        using (DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19", HttpContext.Current.Session["CompId"].ToString()))
        {
            if (dtAllPageCode.Rows.Count != 0)
            {
                allow = true;
            }
        }
        return allow;
    }
    #endregion
    #region ChildTaxGrid
    protected void imgaddTax_Command(object sender, CommandEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        if (((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl == "~/Images/plus.png")
        {
            ((GridView)gvrow.FindControl("gvchildGrid")).Visible = true;
            ((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl = "~/Images/minus.png";
        }
        else
        {
            ((GridView)gvrow.FindControl("gvchildGrid")).Visible = false;
            ((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl = "~/Images/plus.png";
        }
    }
    protected void GvProductDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ClientScriptManager cs = Page.ClientScript;
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((Label)e.Row.FindControl("lblgvProductId")).Text;
            string Po_Id = ((Label)e.Row.FindControl("lblPOId")).Text;
            cs.RegisterArrayDeclaration("grd_Recqty", String.Concat("'", ((Label)e.Row.FindControl("lblgoodsreceive")).ClientID, "'"));
            cs.RegisterArrayDeclaration("grd_Invoiceqty", String.Concat("'", ((TextBox)e.Row.FindControl("QtyGoodReceived")).ClientID, "'"));
            DropDownList ddlUnit = ((DropDownList)e.Row.FindControl("ddlUnitName"));
            GridView gvchildGrid = (GridView)e.Row.FindControl("gvchildGrid");
            if (objProTax.GetRecord_ByProductId(ProductID).Rows.Count > 0)
            {
                ((ImageButton)e.Row.FindControl("imgBtnaddtax")).Visible = true;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvchildGrid, objProTax.GetRecord_ByProductId(ProductID), "", "");
                if (Po_Id.Trim() == "")
                {
                    Po_Id = "0";
                }
                if (ViewState["DtTax"] != null)
                {
                    try
                    {
                        DataTable dt = new DataView((DataTable)ViewState["DtTax"], "ProductId='" + ProductID + "' and SO_Id=" + Po_Id + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count > 0)
                        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                            objPageCmn.FillData((object)gvchildGrid, dt, "", "");
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgBtnaddtax")).Visible = false;
            }
            //for add unit dropdown in invoice page 
            //created by jitendra upadhyay on 25-2-2016
            //code start
            FillUnit(ProductID, ddlUnit);
            if (RdoPo.Checked)
            {
                try
                {
                    ddlUnit.SelectedValue = new DataView((DataTable)ViewState["dtPo"], "ProductId=" + ProductID + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitId"].ToString();
                }
                catch
                {
                    ddlUnit.SelectedIndex = 0;
                }
                ddlUnit.Enabled = false;
            }
            else
            {
                try
                {
                    ddlUnit.SelectedValue = new DataView((DataTable)ViewState["Dtproduct"], "ProductId=" + ProductID + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitId"].ToString();
                }
                catch
                {
                    ddlUnit.SelectedIndex = 0;
                }
                ddlUnit.Enabled = true;
            }
            //code end
        }
    }
    public void FillUnit(string ProductId, DropDownList ddlUnit)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    public void GetChildGridRecordInViewState()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ProductId");
        dt.Columns.Add("Tax_Id");
        dt.Columns.Add("ProductCategoryId");
        dt.Columns.Add("CategoryName");
        dt.Columns.Add("TaxName");
        dt.Columns.Add("Tax_Per");
        dt.Columns.Add("Tax_value");
        dt.Columns.Add("TaxSelected", typeof(bool));
        dt.Columns.Add("SO_Id");
        foreach (GridViewRow gvrow in gvProduct.Rows)
        {
            foreach (GridViewRow gvChildRow in ((GridView)gvrow.FindControl("gvchildGrid")).Rows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = ((Label)gvrow.FindControl("lblgvProductId")).Text;
                dr[1] = ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value;
                dr[2] = ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value;
                dr[3] = ((Label)gvChildRow.FindControl("lblgvcategoryName")).Text;
                dr[4] = ((Label)gvChildRow.FindControl("lblgvtaxName")).Text;
                dr[5] = ((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text;
                dr[6] = ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text;
                dr[7] = ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked;
                if (((Label)gvrow.FindControl("lblPOId")).Text == "")
                {
                    ((Label)gvrow.FindControl("lblPOId")).Text = "0";
                }
                dr[8] = ((Label)gvrow.FindControl("lblPOId")).Text;
                dt.Rows.Add(dr);
            }
        }
        ViewState["DtTax"] = dt;
        dt = null;
    }
    protected void chkselecttax_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (((TextBox)gvrow.FindControl("txttaxPerchild")).Text == "")
            {
                DisplayMessage("Enter Tax Percentage");
                return;
            }
        }
        onTaxselectionandchange(true, gvrow);
    }
    protected void txttaxPerchild_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        onTaxselectionandchange(true, gvrow);
    }
    protected void txttaxValuechild_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        onTaxselectionandchange(false, gvrow);
    }
    public void childGridCalculation(GridView gridchild, string Amount)
    {
        if (Amount == "")
        {
            Amount = "0";
        }
        foreach (GridViewRow gvrow in gridchild.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = SetDecimal((Convert.ToDouble(Amount) * (Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100)).ToString());
            }
        }
    }
    public void onTaxselectionandchange(bool istaxper, GridViewRow gvrow)
    {
        double priceafterdiscount = 0;
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (istaxper)
            {
                if (((TextBox)gvrow.FindControl("txttaxPerchild")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = "0";
                }
            }
            else
            {
                if (((TextBox)gvrow.FindControl("txttaxValuechild")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
                }
            }
        }
        GridView Childgrid = (GridView)(gvrow.Parent.Parent);
        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblUnitRate")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblUnitRate")).Text);
        }
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblDiscountValue")).Text != "")
        {
            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblDiscountValue")).Text);
        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (istaxper)
            {
                try
                {
                    ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = SetDecimal((priceafterdiscount * Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100).ToString());
                }
                catch
                {
                    ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
                }
            }
            else
            {
                try
                {
                    ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = SetDecimal((Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxValuechild")).Text) * 100 / priceafterdiscount).ToString());
                }
                catch
                {
                    ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = "0";
                }
            }
            ((TextBox)gvrow.FindControl("txttaxValuechild")).Enabled = true;
            ((TextBox)gvrow.FindControl("txttaxPerchild")).Enabled = true;
        }
        else
        {
            ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
            ((TextBox)gvrow.FindControl("txttaxValuechild")).Enabled = false;
            ((TextBox)gvrow.FindControl("txttaxPerchild")).Enabled = false;
        }
        double TotalTax = 0;
        foreach (GridViewRow gvchildrow in Childgrid.Rows)
        {
            if (((CheckBox)gvchildrow.FindControl("chkselecttax")).Checked)
            {
                if (((TextBox)gvchildrow.FindControl("txttaxValuechild")).Text != "")
                {
                    TotalTax += Convert.ToDouble(((TextBox)gvchildrow.FindControl("txttaxValuechild")).Text);
                }
                else
                {
                    ((TextBox)gvchildrow.FindControl("txttaxValuechild")).Text = "0";
                    TotalTax += 0;
                }
            }
        }
        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblTaxValue")).Text = Convert_Into_DF(TotalTax.ToString());
        double Percentage = TotalTax * 100 / priceafterdiscount;
        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblTax")).Text = SetDecimal(Percentage.ToString());
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("QtyGoodReceived")).Text != "")
        {
            ((Label)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblAmount")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("QtyGoodReceived")).Text)).ToString();
        }
        else
        {
            ((Label)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblAmount")).Text = "0";
        }
        DirectGridFooterCalculation();
    }
    #endregion
    #region addheadertax
    protected void txtTaxFooter_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
        dt = new DataView(dt, "Tax_Name='" + ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Choose Tax In Suggestion Only");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")));
            ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text = "";
            return;
        }
        else
        {
            if (ViewState["dtTaxHeader"] != null)
            {
                DataTable dtCheckExistTax = new DataTable();
                dtCheckExistTax = (DataTable)ViewState["dtTaxHeader"];
                try
                {
                    dtCheckExistTax = new DataView(dtCheckExistTax, "Tax_Id=" + dt.Rows[0]["Trans_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtCheckExistTax.Rows.Count > 0)
                {
                    DisplayMessage("Tax is Already Exists");
                    ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text = "";
                    ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Focus();
                    return;
                }
            }
            ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Focus();
        }
    }
    protected void txtTaxperFooter_OnTextChanged(object sender, EventArgs e)
    {
        if (txtNetDisVal.Text == "")
        {
            txtNetDisVal.Text = "0";
        }
        double Priceafterdiscount = 0;
        try
        {
            Priceafterdiscount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text) - Convert.ToDouble(txtNetDisVal.Text);
        }
        catch
        {
        }
        if (((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text != "")
        {
            if (Priceafterdiscount != 0)
            {
                ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = SetDecimal((Priceafterdiscount * (Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text) / 100)).ToString());
            }
            else
            {
                ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = "0";
            }
        }
    }
    protected void txtTaxValueFooter_OnTextChanged(object sender, EventArgs e)
    {
        if (txtNetDisVal.Text == "")
        {
            txtNetDisVal.Text = "0";
        }
        double Priceafterdiscount = 0;
        try
        {
            Priceafterdiscount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text) - Convert.ToDouble(txtNetDisVal.Text);
        }
        catch
        {
        }
        if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text != "")
        {
            if (Priceafterdiscount != 0)
            {
                ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = SetDecimal(((Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text) * 100) / Priceafterdiscount).ToString());
            }
            else
            {
                ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = "0";
            }
        }
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
    protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridView.EditIndex = e.NewEditIndex;
        LoadStores();
    }
    protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtTaxName = (TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxName");
        TextBox txtTaxValue = (TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxValue");
        if (txtTaxName.Text == "")
        {
            DisplayMessage("Enter Tax name");
            return;
        }
        if (txtTaxValue.Text == "")
        {
            DisplayMessage("Enter Tax value");
            return;
        }
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
        DisplayMessage("Updated Successfully");
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
            if (((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text == "")
            {
                DisplayMessage("Enter Tax name");
                return;
            }
            DataTable dtTax = objTaxMaster.GetTaxMasterTrueAll();
            dtTax = new DataView(dtTax, "Tax_Name='" + ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTax.Rows.Count > 0)
            {
                TaxId = dtTax.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                DisplayMessage("Choose tax in sugestion only");
                return;
            }
            if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text == "" && ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text == "")
            {
                DisplayMessage("Enter Tax Percentage or Value");
                return;
            }
            TextBox txtTaxName = (TextBox)gridView.FooterRow.FindControl("txtTaxFooter");
            TextBox txtTaxValue = (TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter");
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
                    DisplayMessage("Tax Already Exists");
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
        gridView.EditIndex = -1;
        LoadStores();
        TotalheaderTax();
    }
    public void CalculationchangeIntaxGridview()
    {
        if (txtNetDisVal.Text == "")
        {
            txtNetDisVal.Text = "0";
        }
        double Priceafterdiscount = 0;
        try
        {
            Priceafterdiscount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text) - Convert.ToDouble(txtNetDisVal.Text);
        }
        catch
        {
        }
        if (Priceafterdiscount != 0)
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                try
                {
                    if (((Label)gridView.Rows[i].FindControl("lblTaxper")).Text != "")
                    {
                        if (Priceafterdiscount != 0)
                        {
                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = Convert_Into_DF((Priceafterdiscount * (Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text) / 100)).ToString());
                        }
                        else
                        {
                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = Convert_Into_DF("0");
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
    public void TotalheaderTax()
    {
        double Priceafterdiscount = 0;
        try
        {
            Priceafterdiscount = Convert.ToDouble(((Label)gvProduct.FooterRow.FindControl("txtTotalGrossAmount")).Text);
        }
        catch
        {
        }
        double netTaxPer = 0;
        double nettaxval = 0;
        for (int i = 0; i < gridView.Rows.Count; i++)
        {
            try
            {
                netTaxPer += Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text);
                nettaxval += Convert.ToDouble(Convert_Into_DF(((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text));
            }
            catch
            {
            }
        }
        txtNetTaxPar.Text = netTaxPer.ToString();
        txtNetTaxVal.Text = Convert_Into_DF(nettaxval.ToString());
        if (txtNetDisVal.Text == "")
        {
            txtNetDisVal.Text = "0";
        }
        if (txtNetTaxVal.Text == "")
        {
            txtNetTaxVal.Text = Convert_Into_DF("0");
        }
        txtGrandTotal.Text = SetDecimal((Priceafterdiscount - Convert.ToDouble(txtNetDisVal.Text) + Convert.ToDouble(Convert_Into_DF(txtNetTaxVal.Text))).ToString());
        txtlocalGrandtotal.Text = GetLocalPrice(txtGrandTotal.Text);
        txtBillAmount.Text = txtGrandTotal.Text;
        txtBillAmount_TextChanged(null, null);
        TaxDiscountFromHeader(false);
        try
        {
            ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text = txtlocalGrandtotal.Text;
        }
        catch
        {
        }
        //try
        //{
        //    txtlocalGrandtotal.Text = ((Label)gvProduct.FooterRow.FindControl("txtLocalTotalGrossAmount")).Text;
        //}
        //catch
        //{
        //    txtlocalGrandtotal.Text = "0";
        //}
    }
    protected void txtTaxName_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdhtaxId = (HiddenField)gvrow.FindControl("hdnTaxId");
        TextBox txtTaxname = (TextBox)gvrow.FindControl("txtTaxName");
        if (txtTaxname.Text.Trim() != "")
        {
            DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
            dt = new DataView(dt, "Tax_Name='" + txtTaxname.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Choose Tax In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxname);
                txtTaxname.Text = "";
                hdhtaxId.Value = "0";
            }
            else
            {
                hdhtaxId.Value = dt.Rows[0]["Trans_Id"].ToString();
                if (ViewState["dtTaxHeader"] != null)
                {
                    DataTable dtCheckExistTax = new DataTable();
                    dtCheckExistTax = (DataTable)ViewState["dtTaxHeader"];
                    try
                    {
                        dtCheckExistTax = new DataView(dtCheckExistTax, "Tax_Id=" + hdhtaxId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtCheckExistTax.Rows.Count > 0)
                    {
                        DisplayMessage("Tax is Already Exists");
                        txtTaxname.Text = "";
                        txtTaxname.Focus();
                        hdhtaxId.Value = "0";
                    }
                }
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTax(string prefixText, int count, string contextKey)
    {
        TaxMaster objtaxMaster = new TaxMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objtaxMaster.GetTaxMaster_ByTaxName(prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Tax_Name"].ToString();
        }
        return txt;
    }
    #endregion
    #region ShippingInformation
    public void FillShipUnitddl()
    {
        objPageCmn.FillData((object)ddlShipUnit, objUnit.GetUnitMaster(Session["CompId"].ToString()), "Unit_Name", "Unit_Id");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListShippingLine(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtContact = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
            string[] filterlist = new string[dtContact.Rows.Count];
            if (dtContact.Rows.Count > 0)
            {
                for (int i = 0; i < dtContact.Rows.Count; i++)
                {
                    filterlist[i] = dtContact.Rows[i]["Filtertext"].ToString();
                }
            }
            return filterlist;
        }
        catch
        {
            return null;
        }
    }
    protected void txtShippingLine_TextChanged(object sender, EventArgs e)
    {
        if (txtShippingLine.Text != "")
        {
            try
            {
                string ContactId = string.Empty;
                try
                {
                    ContactId = txtShippingLine.Text.Split('/')[1].ToString();
                }
                catch
                {
                    ContactId = "0";
                }
                if ((ContactId == "") || (ContactId == "0"))
                {
                    DisplayMessage("Please Select from Suggestions Only");
                    txtShippingLine.Text = "";
                    txtShippingLine.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
    }
    protected void getTotalVolume(object sender, EventArgs e)
    {
        decimal length = 0;
        decimal height = 0;
        decimal width = 0;
        decimal carton = 0;
        try
        {
            length = decimal.Parse(((TextBox)gvShippingInformation.FooterRow.FindControl("txtLength")).Text);
        }
        catch
        {
        }
        try
        {
            height = decimal.Parse(((TextBox)gvShippingInformation.FooterRow.FindControl("txtheight")).Text);
        }
        catch
        {
        }
        try
        {
            width = decimal.Parse(((TextBox)gvShippingInformation.FooterRow.FindControl("txtwidth")).Text);
        }
        catch
        {
        }
        try
        {
            carton = decimal.Parse(((TextBox)gvShippingInformation.FooterRow.FindControl("txtcartons")).Text);
        }
        catch
        {
        }
        ((TextBox)gvShippingInformation.FooterRow.FindControl("txttotal")).Text = ((length * height * width) * carton).ToString();
    }
    public void shippingCalculation()
    {
        double totalsum = 0;
        foreach (GridViewRow gvRow in gvShippingInformation.Rows)
        {
            try
            {
                totalsum += Convert.ToDouble(((Label)gvRow.FindControl("lblTotal")).Text);
            }
            catch
            {
            }
        }
        txttotalVolume.Text = totalsum.ToString();
        if (txtdivideby.Text == "")
        {
            txtdivideby.Text = "0";
        }
        if (Convert.ToDouble(txtdivideby.Text) > 0)
        {
            txttotalvolumetricweight.Text = (Convert.ToDouble(txttotalVolume.Text) / Convert.ToDouble(txtdivideby.Text)).ToString();
        }
    }
    protected void txtdivideby_OnTextChanged(object sender, EventArgs e)
    {
        shippingCalculation();
    }
    #endregion
    #region PendingOrder
    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "PODate" || ddlQSeleclField.SelectedItem.Value == "DeliveryDate")
        {
            txtQValueDate.Visible = true;
            txtQValue.Visible = false;
            txtQValue.Text = "";
            txtQValueDate.Text = "";
        }
        else
        {
            txtQValueDate.Visible = false;
            txtQValue.Visible = true;
            txtQValueDate.Text = "";
            txtQValue.Text = "";
        }
    }
    protected void ImgBtnQBind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "PODate" || ddlQSeleclField.SelectedItem.Value == "DeliveryDate")
        {
            if (txtQValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtQValueDate.Text);
                    txtQValue.Text = Convert.ToDateTime(txtQValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtQValueDate.Text = "";
                    txtQValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQValueDate);
                    return;
                }
                txtQValueDate.Focus();
            }
            else
            {
                DisplayMessage("Enter Date");
                txtQValueDate.Focus();
                txtQValue.Text = "";
                return;
            }
        }
        FillGridPendingOrder();
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlQSeleclField.SelectedIndex = 0;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        txtQValue.Visible = true;
        txtQValueDate.Visible = false;
        FillGridPendingOrder();
    }
    protected void gvPurchaseOrder_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (gvPurchaseOrder.Attributes["CurrentSortField"] != null &&
            gvPurchaseOrder.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvPurchaseOrder.Attributes["CurrentSortField"])
            {
                if (gvPurchaseOrder.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvPurchaseOrder.Attributes["CurrentSortField"] = sortField;
        gvPurchaseOrder.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGridPendingOrder(Int32.Parse(hdnGvPurchaseOrderCurrentPageIndex.Value));
    }
    public string GetDateFromat(string Date)
    {
        try
        {
            return Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
        }
        catch
        {
            return "";
        }
    }
    private void FillGridPendingOrder(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlQOption.SelectedIndex != 0 && txtQValue.Text != string.Empty)
        {
            if (ddlQOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + "='" + txtQValue.Text.Trim() + "'";
            }
            else if (ddlQOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + " like '%" + txtQValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + " Like '" + txtQValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        DataTable dt = ObjPurchaseInvoice.getPendingOrderList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), gvPurchaseOrder.Attributes["CurrentSortField"], gvPurchaseOrder.Attributes["CurrentSortDirection"], strWhereClause);
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)gvPurchaseOrder, dt, "", "");
            totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
        }
        else
        {
            gvPurchaseOrder.DataSource = null;
            gvPurchaseOrder.DataBind();
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        }
        PageControlCommon.PopulatePager(rptPagerPO, totalRows, currentPageIndex);
    }
    protected void btnPendingOrder_Click(object sender, EventArgs e)
    {
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        pnlPendingOrder.Visible = true;
        txtInvoiceNo.Focus();
        FillGridPendingOrder();
    }
    protected void PagePO_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvPurchaseOrderCurrentPageIndex.Value = pageIndex.ToString();
        FillGridPendingOrder(pageIndex);
    }
    //for child grid
    #endregion
    #region GetLocation
    private void FillddlLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        //Common Function add By jitendra on 06-02-2016
        ddlLocation.DataSource = dtLoc;
        ddlLocation.DataTextField = "Location_Name";
        ddlLocation.DataValueField = "Location_Id";
        ddlLocation.DataBind();
        dtLoc = null;
    }
    #endregion
    #region ProductHistory
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
        return SetDecimal(SysQty);
    }
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string CustomerName = string.Empty;
        try
        {
            CustomerName = txtSupplierName.Text.Split('/')[0].ToString();
        }
        catch
        {
        }
        modelSA.getProductDetail(e.CommandArgument.ToString(), "PURCHASE", CustomerName);
        string strCmd = "$('#Product_StockAnalysis').modal('show');";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
    }
    #endregion
    #region SupplierHistory


    #endregion
    public string GetSupplierCurrency(string str)
    {
        string strSupplierCurrency = string.Empty;
        strSupplierCurrency = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString()).Rows[0]["Field3"].ToString();
        return strSupplierCurrency;
    }
    public string GetInvoiceCurrency(string str)
    {
        string strInvoiceCurrency = string.Empty;
        strInvoiceCurrency = ddlCurrency.SelectedValue;
        return strInvoiceCurrency;
    }
    public DataTable TemporaryProductWiseTaxes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Product_Id", typeof(float));
        dt.Columns.Add("Tax_Id", typeof(float));
        dt.Columns.Add("Tax_Name", typeof(string));
        dt.Columns.Add("Tax_Value", typeof(float));
        dt.Columns.Add("TaxAmount", typeof(float));
        dt.Columns.Add("Amount", typeof(float));
        dt.Columns.Add("Serial_No", typeof(float));
        return dt;
    }
    protected void txtTaxValueInPer_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = sender as TextBox;
        double TaxValue = 0;
        if (String.IsNullOrEmpty(txt.Text))
            TaxValue = 0;
        else
            TaxValue = double.Parse(txt.Text);
        if (TaxValue > 100)
        {
            DisplayMessage("Please Enter Valid Percentage");
            return;
        }
    }
    protected void btnSaveGST_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvTaxRow in gvTaxCalculation.Rows)
        {
            HiddenField hdngvProductId = (HiddenField)gvTaxRow.FindControl("lblgvProductId");
            HiddenField lblgvTaxId = (HiddenField)gvTaxRow.FindControl("lblgvTaxId");
            TextBox txtTaxValueInPer = (TextBox)gvTaxRow.FindControl("txtTaxValueInPer");
            foreach (GridViewRow gvRow in gvProduct.Rows)
            {
                Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
                Label lblSerialNO = (Label)gvRow.FindControl("lblSerialNO");
                Label lblTransId = (Label)gvRow.FindControl("lblTransId");
                Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
                if (lblgvProductId.Text == hdngvProductId.Value)
                {
                    Label TxtgvQty = (Label)gvRow.FindControl("OrderQty");
                    TextBox TxtgvDiscount = (TextBox)gvRow.FindControl("lblDiscount");
                    TextBox TxtgvDiscountValue = (TextBox)gvRow.FindControl("lblDiscountValue");
                    TextBox TxtgvTax = (TextBox)gvRow.FindControl("lblTax");
                    TextBox TxtgvTaxValue = (TextBox)gvRow.FindControl("lblTaxValue");
                    Label lblgvNetAmount = (Label)gvRow.FindControl("lblAmount");
                    TextBox txtUnitCost = (TextBox)gvRow.FindControl("lblUnitRate");
                    Label lblQtyAmmount = (Label)gvRow.FindControl("lblQtyAmmount");
                    if (txtUnitCost.Text == "")
                    {
                        txtUnitCost.Text = "0";
                    }
                    if (TxtgvQty.Text == "")
                    {
                        TxtgvQty.Text = "0";
                    }
                    double PriceValue = double.Parse(txtUnitCost.Text);
                    double QtyValue = double.Parse(TxtgvQty.Text);
                    double AmountValue = PriceValue * QtyValue;
                    double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
                    bool IsValidDiscount = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
                    //    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
                    if (!IsValidDiscount)
                        AmntAfterDiscnt = AmountValue;
                    double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblSerialNO.Text);
                    TotalTax = TotalTax / QtyValue;
                    string[] strcalc = Common.TaxDiscountCaluculation(txtUnitCost.Text, TxtgvQty.Text, "", TotalTax.ToString(), "", TxtgvDiscountValue.Text, true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                    //lblgvQuantityPrice.Text = AmountValue.ToString();
                    lblgvNetAmount.Text = (AmntAfterDiscnt + TotalTax).ToString();  //strcalc[5].ToString();
                    //lblQtyAmmount.Text = lblQtyAmmount.Text;
                    TxtgvTaxValue.Text = TotalTax.ToString();
                    GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
                    string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
                    childGridCalculation(GridChild, PriceAfterDiscount);
                }
            }
        }
        DirectGridFooterCalculation();
        ClosePopUp();
    }
    protected void ClosePopUp()
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Model_GST()", true);
    }
    protected void Get_Tax_Parameter()
    {
        DataTable Dt_Parameter = ObjSysParam.GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
            {
                Hdn_Tax_By.Value = "Company";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
            {
                Hdn_Tax_By.Value = "Location";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
            {
                Hdn_Tax_By.Value = "System";
            }
            else
            {
                Hdn_Tax_By.Value = "Select";
            }
        }
        Dt_Parameter = null;
    }
    public void TaxCalculationWithDiscount()
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblSerialNO = (Label)gvRow.FindControl("lblSerialNO");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label TxtgvQty = (Label)gvRow.FindControl("OrderQty");
            TextBox Qty_Good_Received = (TextBox)gvRow.FindControl("QtyGoodReceived");
            TextBox TxtgvDiscount = (TextBox)gvRow.FindControl("lblDiscount");
            TextBox TxtgvDiscountValue = (TextBox)gvRow.FindControl("lblDiscountValue");
            TextBox TxtgvTax = (TextBox)gvRow.FindControl("lblTax");
            TextBox TxtgvTaxValue = (TextBox)gvRow.FindControl("lblTaxValue");
            Label lblgvNetAmount = (Label)gvRow.FindControl("lblAmount");
            TextBox txtUnitCost = (TextBox)gvRow.FindControl("lblUnitRate");
            Label lblQtyAmmount = (Label)gvRow.FindControl("lblQtyAmmount");
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            if (TxtgvQty.Text == "")
            {
                TxtgvQty.Text = "0";
            }
            double PriceValue = double.Parse(txtUnitCost.Text);
            double QtyValue = double.Parse(Qty_Good_Received.Text);
            double AmountValue = PriceValue * QtyValue;
            double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
            bool IsValidDiscount = lblNetDisPer.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            //    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();
            if (!IsValidDiscount)
                AmntAfterDiscnt = AmountValue;
            double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Text, lblSerialNO.Text);
            TotalTax = TotalTax / QtyValue;
            string[] strcalc = Common.TaxDiscountCaluculation(txtUnitCost.Text, Qty_Good_Received.Text, "", TotalTax.ToString(), "", TxtgvDiscountValue.Text, true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
            //lblgvQuantityPrice.Text = AmountValue.ToString();
            lblgvNetAmount.Text = (AmntAfterDiscnt + TotalTax).ToString();  //strcalc[5].ToString();
            if (lblgvNetAmount.Text == "NaN" || lblgvNetAmount.Text == "")
                lblgvNetAmount.Text = "0.00";
            //lblQtyAmmount.Text = lblQtyAmmount.Text;
            if (Qty_Good_Received.Text != "0.00" && Qty_Good_Received.Text != "" && Qty_Good_Received.Text != "0")
                TxtgvTaxValue.Text = TotalTax.ToString();
            else
                TxtgvTaxValue.Text = "0.00";
            GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
            string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
            childGridCalculation(GridChild, PriceAfterDiscount);
        }
        DirectGridFooterCalculation();
    }
    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_PI"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PI"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + e.CommandName.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxCalculation.DataSource = Dt_Cal;
                    gvTaxCalculation.DataBind();
                    Hdn_Product_Id_Tax.Value = e.CommandArgument.ToString();
                    Hdn_Serial_No_Tax.Value = e.CommandName.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                }
                else
                {
                    DisplayMessage("No Tax Details found");
                    return;
                }
            }
        }
        else
        {
            DisplayMessage("No Tax Details found");
            return;
        }
    }
    public void Get_Tax_From_DB()
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (HdnEdit.Value != "")
                    {
                        DataTable DT_Db_Details = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value);
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.SerialNo as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseInvoiceDetail IPID on IPID.TransID=TRD.Field2 where TRD.Ref_Id='" + HdnEdit.Value + "' and TRD.Ref_Type='PINV' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.SerialNo";
                            DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_PI"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_PI"] = Dt_Temp;
                            }
                            Dt_Inv_TaxRefDetail = null;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public double Get_Tax_Percentage(string ProductId, string Serial_No)
    {
        double TotalTax_Percentage = 0;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PI"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_PI"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
            }
        }
        return TotalTax_Percentage;
    }
    public double Get_Tax_Amount(string Amount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PI"] != null)
            {
                double Tax_Value = Get_Tax_Percentage(ProductId, Serial_No);
                double Temp_Amount = Convert.ToDouble(Convert_Into_DF(Amount));
                TotalTax_Amount = Convert.ToDouble(Convert_Into_DF(((Tax_Value * Temp_Amount) / 100).ToString()));
            }
        }
        return TotalTax_Amount;
    }
    public void Add_Tax_In_Session(string Amount, string Discount, string ProductId, string Serial_No)
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (Discount == "")
                Discount = "0";
            Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
            Discount = Convert_Into_DF(Discount.ToString());
            //if (IsTax && double.Parse(Amount) > 0)
            if (IsTax)
            {
                Get_Tax_Parameter();
                String Condition = string.Empty;
                if (Hdn_Tax_By.Value == Resources.Attendance.Company)
                    Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
                else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
                    Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";
                if (ddlTransType.SelectedIndex > 0)
                {
                    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
                            where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
                    DataTable dtTax = objDa.return_DataTable(TaxQuery);
                    double TotalPriceBeforeDiscount = double.Parse(Amount) - double.Parse(Discount);
                    DataTable dt = new DataTable();
                    if (Session["Temp_Product_Tax_PI"] == null)
                        dt = TemporaryProductWiseTaxes();
                    else
                        dt = (DataTable)Session["Temp_Product_Tax_PI"];
                    if (dtTax.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTax.Rows)
                        {
                            double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                            double taxamount = Convert.ToDouble(Convert_Into_DF(((TotalPriceBeforeDiscount * taxvalue) / 100).ToString()));
                            DataRow Newdr = dt.NewRow();
                            Newdr["Product_Id"] = ProductId;
                            Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                            Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                            Newdr["Tax_Value"] = Convert_Into_DF(dr["Tax_Value"].ToString());
                            Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
                            Newdr["Serial_No"] = Serial_No;
                            DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                            if (SRow.Length == 0)
                            {
                                dt.Rows.Add(Newdr);
                            }
                            else
                            {
                                taxvalue = double.Parse(Convert_Into_DF(SRow[0]["Tax_Value"].ToString()));
                                taxamount = Convert.ToDouble(Convert_Into_DF(((TotalPriceBeforeDiscount * taxvalue) / 100).ToString()));
                                SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                                SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
                                SRow[0]["Serial_No"] = Serial_No;
                            }
                        }
                        Session["Temp_Product_Tax_PI"] = dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Add_Tax_In_Session_From_Order(string Amount, string ProductId, string PO_ID, string Serial_No)
    {
        string TaxQuery = string.Empty;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
        //if (IsTax && double.Parse(Amount) > 0)
        if (IsTax)
        {
            Get_Tax_Parameter();
            if (ddlTransType.SelectedIndex > 0)
            {
                DataTable dtTax = objTaxRefDetail.GetRecord_By_RefType_and_RefId("PO", PO_ID);
                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_PI"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_PI"];
                if (dtTax.Rows.Count > 0)
                {
                    DataColumnCollection columns = dtTax.Columns;
                    if (columns.Contains("TaxName"))
                    {
                        dtTax.Columns["TaxName"].ColumnName = "Tax_Name";
                    }
                    dtTax = new DataView(dtTax, "ProductId=" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = dr["ProductId"].ToString();
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Per"].ToString();
                        Newdr["TaxAmount"] = dr["Tax_Value"].ToString();
                        Newdr["Amount"] = Amount;
                        Newdr["Serial_No"] = Serial_No;
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                    }
                    Session["Temp_Product_Tax_PI"] = dt;
                }
            }
        }
    }
    protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Temp_Product_Tax_PI"] = null;
    }
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        string Serial_No = Hdn_Serial_No_Tax.Value;
        string Product_ID = Hdn_Product_Id_Tax.Value;
        if (Session["Temp_Product_Tax_PI"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PI"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                foreach (DataRow DR_Tax in Dt_Cal.Rows)
                {
                    foreach (GridViewRow GVR in gvTaxCalculation.Rows)
                    {
                        TextBox Tax_Percentage = (TextBox)GVR.FindControl("txtTaxValueInPer");
                        HiddenField TaxId = (HiddenField)GVR.FindControl("lblgvTaxId");
                        HiddenField ProductId = (HiddenField)GVR.FindControl("lblgvProductId");
                        if (Tax_Percentage.Text == "")
                            Tax_Percentage.Text = "0.00";
                        if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value && DR_Tax["Serial_No"].ToString() == Serial_No)
                            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                    }
                }
            }
            Session["Temp_Product_Tax_PI"] = Dt_Cal;
            foreach (GridViewRow dl in gvProduct.Rows)
            {
                if (Dt_Cal.Rows.Count > 0)
                {
                    DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "' And Serial_No='" + Serial_No + "' ", "", DataViewRowState.CurrentRows).ToTable();
                    if (Dt_Cal_Temp.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Cal_Temp.Rows)
                        {
                            DataTable Dt_Cal_Temp2 = Dt_Cal_Temp.DefaultView.ToTable(true, "Tax_Id", "Tax_Value");
                            double Tax_Val = 0;
                            foreach (DataRow DRR in Dt_Cal_Temp2.Rows)
                            {
                                Tax_Val = Tax_Val + Convert.ToDouble(DRR["Tax_Value"].ToString());
                            }
                            TextBox Tax_Percent = (TextBox)dl.FindControl("lblTax");
                            Label hdnProductId = (Label)dl.FindControl("lblgvProductId");
                            Label lblSerialNO = (Label)dl.FindControl("lblSerialNO");
                            if (Product_ID == hdnProductId.Text && Serial_No == lblSerialNO.Text)
                            {
                                Tax_Percent.Text = SetDecimal(Tax_Val.ToString());
                            }
                        }
                    }
                }
            }
            TaxCalculationWithDiscount();
            DirectGridFooterCalculation();
            Hdn_Serial_No_Tax.Value = "";
            Hdn_Product_Id_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row, ref SqlTransaction trns)
    {
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PINV", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
            string R_Product_ID = string.Empty;
            string R_Order_Req_Qty = string.Empty;
            string R_Unit_Price = string.Empty;
            string R_Discount_Value = string.Empty;
            string R_Serial_No = string.Empty;
            string Grid = string.Empty;
            if (Grid_Name == "gvProduct")
            {
                Grid = "gvProduct";
                Label Product_ID = (Label)Gv_Row.FindControl("lblgvProductId");
                TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("QtyGoodReceived");
                TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblUnitRate");
                TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblDiscountValue");
                Label lblSerialNO = (Label)Gv_Row.FindControl("lblSerialNO");
                R_Product_ID = Product_ID.Text;
                R_Order_Req_Qty = Order_Req_Qty.Text;
                R_Unit_Price = Unit_Price.Text;
                R_Discount_Value = Discount_Value.Text;
                R_Serial_No = lblSerialNO.Text;
            }
            if (Grid != "")
            {
                double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
                double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
                double Net_Amount = A_Unit_Cost - A_Unit_Discount;
                //Get_Tax_Insert(R_Product_ID);
                DataTable ProductTax = new DataTable();
                if (Session["Temp_Product_Tax_PI"] == null)
                    ProductTax = TemporaryProductWiseTaxes();
                else
                    ProductTax = (DataTable)Session["Temp_Product_Tax_PI"];
                string ProductId = string.Empty;
                string TaxId = string.Empty;
                string TaxValue = string.Empty;
                string TaxAmount = string.Empty;
                string Amount = string.Empty;
                if (ProductTax != null && ProductTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in ProductTax.Rows)
                    {
                        if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                        {
                            ProductId = dr["Product_Id"].ToString();
                            TaxId = dr["Tax_Id"].ToString();
                            TaxValue = Convert_Into_DF(dr["Tax_Value"].ToString());
                            TaxAmount = Convert_Into_DF((Convert.ToDouble((Convert.ToDouble(Convert_Into_DF(Net_Amount.ToString())) * Convert.ToDouble(TaxValue))) / 100).ToString());
                            //if (Session["VPost"] == null || Session["VPost"].ToString() != "True")
                            //    TaxAmount = (Convert.ToDouble(dr["TaxAmount"].ToString()) * Convert.ToDouble(R_Order_Req_Qty)).ToString();
                            //else TaxAmount = dr["TaxAmount"].ToString();
                            Amount = Net_Amount.ToString();
                            //if (Convert.ToDouble(Amount) != 0)
                            objTaxRefDetail.InsertRecord("PINV", PQ_Header_ID, "0", "0", ProductId, TaxId, Convert_Into_DF(TaxValue), Convert_Into_DF(TaxAmount), false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
            }
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row)
    {
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PINV", PQ_Header_ID.ToString(), Detail_ID.ToString());
            string R_Product_ID = string.Empty;
            string R_Order_Req_Qty = string.Empty;
            string R_Unit_Price = string.Empty;
            string R_Discount_Value = string.Empty;
            string R_Serial_No = string.Empty;
            string Grid = string.Empty;
            if (Grid_Name == "gvProduct")
            {
                Grid = "gvProduct";
                Label Product_ID = (Label)Gv_Row.FindControl("lblProductId");
                TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("QtyGoodReceived");
                TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblUnitRate");
                TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblDiscountValue");
                Label lblSerialNO = (Label)Gv_Row.FindControl("lblSerialNO");
                R_Product_ID = Product_ID.Text;
                R_Order_Req_Qty = Order_Req_Qty.Text;
                R_Unit_Price = Unit_Price.Text;
                R_Discount_Value = Discount_Value.Text;
                R_Serial_No = lblSerialNO.Text;
            }
            if (Grid != "")
            {
                double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
                double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
                double Net_Amount = A_Unit_Cost - A_Unit_Discount;
                //Get_Tax_Insert(R_Product_ID);
                DataTable ProductTax = new DataTable();
                if (Session["Temp_Product_Tax_PI"] == null)
                    ProductTax = TemporaryProductWiseTaxes();
                else
                    ProductTax = (DataTable)Session["Temp_Product_Tax_PI"];
                string ProductId = string.Empty;
                string TaxId = string.Empty;
                string TaxValue = string.Empty;
                string TaxAmount = string.Empty;
                string Amount = string.Empty;
                if (ProductTax != null && ProductTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in ProductTax.Rows)
                    {
                        if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                        {
                            ProductId = dr["Product_Id"].ToString();
                            TaxId = dr["Tax_Id"].ToString();
                            TaxValue = Convert_Into_DF(dr["Tax_Value"].ToString());
                            TaxAmount = Convert_Into_DF((Convert.ToDouble((Convert.ToDouble(Convert_Into_DF(Net_Amount.ToString())) * Convert.ToDouble(TaxValue))) / 100).ToString());
                            ////TaxAmount = dr["TaxAmount"].ToString();
                            //if (Session["VPost"] == null || Session["VPost"].ToString() != "True")
                            //    TaxAmount = (Convert.ToDouble(dr["TaxAmount"].ToString()) * Convert.ToDouble(R_Order_Req_Qty)).ToString();
                            //else TaxAmount = dr["TaxAmount"].ToString();
                            Amount = Net_Amount.ToString();
                            //if (Convert.ToDouble(Amount) != 0)
                            objTaxRefDetail.InsertRecord("PINV", PQ_Header_ID, "0", "0", ProductId, TaxId, Convert_Into_DF(TaxValue), Convert_Into_DF(TaxAmount), false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
            }
        }
    }
    protected void Btn_Add_Expenses_Tax_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlExpense.SelectedValue.ToString() == "--Select--")
            {
                DisplayMessage("Please select Expenses");
                ddlExpense.Focus();
                return;
            }
            if (txtExpCharges.Text == "")
            {
                DisplayMessage("Please Enter Expenses Amount");
                txtExpCharges.Focus();
                return;
            }
            if (Convert.ToDouble(txtExpCharges.Text) == 0)
            {
                DisplayMessage("Please Enter Expenses Amount");
                txtExpCharges.Focus();
                return;
            }
            if (GridExpenses != null && GridExpenses.Rows.Count > 0)
            {
                int Same_Expenses_Condition = 0;
                foreach (GridViewRow GV_row in GridExpenses.Rows)
                {
                    HiddenField Hdn_Expense_Id_GV = (HiddenField)GV_row.FindControl("Hdn_Expense_Id_GV");
                    if (Hdn_Expense_Id_GV.Value == ddlExpense.SelectedValue.ToString())
                    {
                        Same_Expenses_Condition++;
                    }
                }
                if (Same_Expenses_Condition != 0)
                {
                    DisplayMessage(ddlExpense.SelectedItem.ToString() + " already Exists.");
                    ddlExpCurrency.Focus();
                    return;
                }
            }
            if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
            {
                int Same_Expenses_Condition = 0;
                foreach (DataRow Dt_row in Dt_Final_Save_Tax.Rows)
                {
                    if (Dt_row["Expenses_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                    {
                        Same_Expenses_Condition++;
                    }
                }
                if (Same_Expenses_Condition != 0)
                {
                    DisplayMessage(ddlExpense.SelectedItem.ToString() + " already Exists.");
                    ddlExpCurrency.Focus();
                    return;
                }
            }
            Hdn_Saved_Expenses_Tax_Session.Value = "Expenses_Tax_Purchase_Invoice_Temp";
            Hdn_Local_Expenses_Tax_Session.Value = "Expenses_Purchase_Invoice_Local";
            Hdn_Expenses_Id_Web_Control.Value = ddlExpense.SelectedValue.ToString();
            Hdn_Expenses_Name_Web_Control.Value = ddlExpense.SelectedItem.ToString();
            Hdn_Expenses_Amount_Web_Control.Value = txtExpCharges.Text.Trim();
            //Hdn_Session_Name_For_Expenses_Tax.Value = "PO_Expenses_Tax";
            //Hdn_Save_Session_Name_For_Expenses_Tax.Value = "Saved_PO_Expenses_Tax";
            Hdn_Page_Name_Web_Control.Value = "Purchase_Invoice";
            Hdn_Tax_Entry_Type.Value = "Multiple";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Expenses_Tax_Web_Control()", true);
        }
        catch { }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Complete_List_Of_Tax(string prefixText, int count, string contextKey)
    {
        TaxMaster Obj_TaxMaster = new TaxMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable Dt_Tax = Obj_TaxMaster.GetTaxMasterFor_Suggestion();
        Dt_Tax = new DataView(Dt_Tax, "TaxName Like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] filterlist = new string[Dt_Tax.Rows.Count];
        if (Dt_Tax.Rows.Count > 0)
        {
            for (int i = 0; i < Dt_Tax.Rows.Count; i++)
            {
                filterlist[i] = Dt_Tax.Rows[i]["TaxName"].ToString();
            }
        }
        return filterlist;
    }
    public void Change_Tax_Amount(DataTable Dt_Saved_Tax_Expenses)
    {
        if (Dt_Saved_Tax_Expenses != null)
        {
            double Net_Tax = 0;
            foreach (DataRow DT_Row in Dt_Saved_Tax_Expenses.Rows)
            {
                if (DT_Row["Expenses_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                    Net_Tax = Net_Tax + (Convert.ToDouble(DT_Row["Tax_Percentage"].ToString()));
            }
            if (txtExpCharges.Text.Trim() != "")
            {
                double Expenses_Tax_Amount = Convert.ToDouble(txtExpCharges.Text.Trim());
                if (Expenses_Tax_Amount > 0)
                {
                    //Lbl_Expenses_Tax_Amount_ET.Text = ((Expenses_Tax_Amount * Net_Tax) / 100).ToString();
                    //Lbl_Expenses_Tax_ET.Text = Net_Tax.ToString() + "%";
                }
                else
                {
                    //Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                    //Lbl_Expenses_Tax_ET.Text = "0%";
                }
            }
            else
            {
                //Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                //Lbl_Expenses_Tax_ET.Text = "0%";
            }
        }
    }
    public string Get_Expenses_Tax(string Debit_Amount)
    {
        string Expenses_Tax_Amount = "0";
        try
        {
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
                {
                    string strDebit_Amount_Temp = string.Empty;
                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        strDebit_Amount_Temp = SetDecimal(((Convert.ToDouble(Dt_Row["Expenses_Amount"].ToString()) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                        Expenses_Tax_Amount = (Convert.ToDouble(Expenses_Tax_Amount) + Convert.ToDouble(strDebit_Amount_Temp)).ToString();
                    }
                }
            }
            return Expenses_Tax_Amount;
        }
        catch { return "0"; }
    }
    public string Get_Expenses_Tax_Post(string Debit_Amount)
    {
        string Expenses_Tax_Amount = "0";
        try
        {
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
                {
                    string strDebit_Amount_Temp = string.Empty;
                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        if (Convert.ToDouble(Dt_Row["Expenses_Amount"].ToString()) == Convert.ToDouble(Debit_Amount))
                        {
                            strDebit_Amount_Temp = SetDecimal(((Convert.ToDouble(Dt_Row["Expenses_Amount"].ToString()) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            Expenses_Tax_Amount = (Convert.ToDouble(Expenses_Tax_Amount) + Convert.ToDouble(strDebit_Amount_Temp)).ToString();
                        }
                    }
                }
            }
            return Expenses_Tax_Amount;
        }
        catch { return "0"; }
    }
    public string[,] Tax_Insert_Into_Ac_Voucher_Detail_Debit(string strVoucher_No, string strSerial_No, string strAccount_No, string strOther_Account_No, string strRef_Id, string strRef_Type, string strCheque_Issue_Date, string strCheque_Clear_Date, string strCheque_No, string strDebit_Amount, string strCredit_Amount, string strNarration, string strCostCenter_ID, string strEmp_Id, string strCurrency_Id, string strExchange_Rate, string strForeign_Amount, string strCompanyCurrDebit, string strCompanyCurrCredit, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strExpensesId, ref SqlTransaction trns)
    {
        string[,] Expenses_Tax_Amount = new string[1, 5];
        try
        {
            double Debit_Amount = 0;
            double CompanyCurrDebit = 0;
            double Foreign_Amount = 0;
            double Credit_Amount = 0;
            double CompanyCurrCredit = 0;
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
                {
                    string strNarration_Temp = string.Empty;
                    string strDebit_Amount_Temp = string.Empty;
                    string strCompanyCurrDebit_Temp = string.Empty;
                    string strForeign_Amount_Temp = string.Empty;
                    string strCredit_Amount_Temp = string.Empty;
                    string strCompanyCurrCredit_Temp = string.Empty;
                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        if (strExpensesId == Dt_Row["Expenses_Id"].ToString())
                        {
                            strNarration_Temp = strNarration;
                            strDebit_Amount_Temp = SetDecimal(((Convert.ToDouble(strDebit_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strForeign_Amount_Temp = SetDecimal(((Convert.ToDouble(strForeign_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strCompanyCurrDebit_Temp = SetDecimal(((Convert.ToDouble(strCompanyCurrDebit) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strCredit_Amount_Temp = SetDecimal(((Convert.ToDouble(strCredit_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strCompanyCurrCredit_Temp = SetDecimal(((Convert.ToDouble(strCompanyCurrCredit) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            Debit_Amount = Debit_Amount + Convert.ToDouble(strDebit_Amount_Temp);
                            CompanyCurrDebit = CompanyCurrDebit + Convert.ToDouble(strCompanyCurrDebit_Temp);
                            Foreign_Amount = Foreign_Amount + Convert.ToDouble(strForeign_Amount_Temp);
                            Credit_Amount = Credit_Amount + Convert.ToDouble(strCredit_Amount_Temp);
                            CompanyCurrCredit = CompanyCurrCredit + Convert.ToDouble(strCompanyCurrCredit_Temp);
                            Expenses_Tax_Amount[0, 0] = Debit_Amount.ToString();
                            Expenses_Tax_Amount[0, 1] = CompanyCurrDebit.ToString();
                            Expenses_Tax_Amount[0, 2] = Foreign_Amount.ToString();
                            Expenses_Tax_Amount[0, 3] = Credit_Amount.ToString();
                            Expenses_Tax_Amount[0, 4] = CompanyCurrCredit.ToString();
                            strNarration_Temp = SetDecimal(Dt_Row["Tax_Percentage"].ToString()) + "% " + Dt_Row["Tax_Type_Name"].ToString() + " On " + strNarration;
                            objVoucherDetail.InsertVoucherDetail
                                (StrCompId.ToString(),
                                StrBrandId.ToString(),
                                StrLocationId.ToString(),
                                strVoucher_No,
                                strSerial_No,
                                Dt_Row["Tax_Account_Id"].ToString(),
                                strOther_Account_No,
                                strRef_Id,
                                strRef_Type,
                                strCheque_Issue_Date,
                                strCheque_Clear_Date,
                                strCheque_No,
                                strDebit_Amount_Temp,
                                strCredit_Amount_Temp,
                                strNarration_Temp,
                                strCostCenter_ID,
                                strEmp_Id,
                                strCurrency_Id,
                                strExchange_Rate,
                                strForeign_Amount_Temp,
                                strCompanyCurrDebit_Temp,
                                strCompanyCurrCredit_Temp,
                                strField1,
                                strField2,
                                strField3,
                                strField4,
                                strField5,
                                strField6,
                                strField7,
                                strIsActive,
                                strCreatedBy,
                                strCreatedDate,
                                strModifiedBy,
                                strModifiedDate,
                                ref trns);
                        }
                    }
                }
            }
            return Expenses_Tax_Amount;
        }
        catch { return Expenses_Tax_Amount; }
    }
    public void Tax_Insert_Into_Inv_TaxRefDetail(string PI_Header_ID, ref SqlTransaction trns)
    {
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            //objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PI", PI_Header_ID.ToString(), ref trns);
            if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
            {
                foreach (GridViewRow GV_Row in GridExpenses.Rows)
                {
                    Label Exp_Charges = (Label)GV_Row.FindControl("lblgvExp_Charges");
                    HiddenField Hdn_Expense_Id_GV = (HiddenField)GV_Row.FindControl("Hdn_Expense_Id_GV");
                    if (Exp_Charges.Text == "")
                        Exp_Charges.Text = "0.00";
                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        if (Hdn_Expense_Id_GV.Value == Dt_Row["Expenses_Id"].ToString())
                            objTaxRefDetail.InsertRecord_Expenses("PINV", PI_Header_ID, "0", "0", "0", Dt_Row["Tax_Type_Id"].ToString(), Dt_Row["Tax_Percentage"].ToString(), Dt_Row["Tax_Value"].ToString(), false.ToString(), Dt_Row["Expenses_Amount"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Dt_Row["Expenses_Id"].ToString(), ref trns);
                    }
                }
            }
        }
    }
    protected void Btn_Add_Expenses_Click(object sender, EventArgs e)
    {
        //here we checking that supplier name should not be blank
        if (txtSupplierName.Text.Trim() == "")
        {
            DisplayMessage("Enter Supplier Name");
            txtSupplierName.Focus();
            return;
        }
        string ExpId = string.Empty;
        if (ddlExpense.SelectedIndex == 0)
        {
            DisplayMessage("Select Expenses");
            ddlExpense.Focus();
            return;
        }
        else
        {
            ExpId = ddlExpense.SelectedValue.ToString();
        }
        //Add By Lokesh On 21-01-2016
        if (txtExpensesAccount.Text == "")
        {
            DisplayMessage("Enter Expenses Account");
            txtExpensesAccount.Focus();
            return;
        }
        if (txtExpCharges.Text == "")
        {
            DisplayMessage("Enter Expenses Charges");
            txtExpCharges.Focus();
            return;
        }
        if (ddlExpCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Currency Required On Company Level");
            ddlExpCurrency.Focus();
            return;
        }
        if (Session[Hdn_Saved_Expenses_Tax_Session.Value] != null)
        {
            DataTable Dt_Grid_Final_Tax_Save = new DataTable();
            Dt_Grid_Final_Tax_Save = Session["Expenses_Tax_Purchase_Invoice"] as DataTable;
            if (Dt_Grid_Final_Tax_Save == null)
            {
                Dt_Grid_Final_Tax_Save = new DataTable();
                Dt_Grid_Final_Tax_Save.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
            }
            DataTable Dt_Grid_Tax_Web_Control = Session[Hdn_Saved_Expenses_Tax_Session.Value] as DataTable;
            Dt_Grid_Tax_Web_Control = new DataView(Dt_Grid_Tax_Web_Control, "Expenses_Id='" + ddlExpense.SelectedValue.ToString() + "' and Expenses_Amount='" + txtExpCharges.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            foreach (DataRow Dt_Row in Dt_Grid_Tax_Web_Control.Rows)
            {
                Dt_Grid_Final_Tax_Save.Rows.Add(Dt_Row["Tax_Type_Id"].ToString(), Dt_Row["Tax_Type_Name"].ToString(), Dt_Row["Tax_Percentage"].ToString(), Dt_Row["Tax_Value"].ToString(), Dt_Row["Expenses_Id"].ToString(), Dt_Row["Expenses_Name"].ToString(), Dt_Row["Expenses_Amount"].ToString(), Dt_Row["Page_Name"].ToString(), Dt_Row["Tax_Entry_Type"].ToString(), Dt_Row["Tax_Account_Id"].ToString());
            }
            Session["Expenses_Tax_Purchase_Invoice"] = Dt_Grid_Final_Tax_Save;
            Dt_Final_Save_Tax = Session["Expenses_Tax_Purchase_Invoice"] as DataTable;
        }
        //
        //if expenses account is supplier account thencurrency should be supplier current other wise supplier statement will show incorrect 
        //validation added  by jitendra upadhyay  on 01-02-2017
        //code start
        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
        dtAcParameter = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (txtExpensesAccount.Text.Split('/')[1].ToString().Trim() == Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()))
        {
            if (ddlCurrency.SelectedValue != ddlExpCurrency.SelectedValue.Trim())
            {
                DisplayMessage("Expenses currency should be equal to Invoicecurrency");
                ddlExpCurrency.Focus();
                return;
            }
        }
        //code end
        DataTable dt = new DataTable();
        int i = 0;
        bool b = false;
        if (ViewState["Expdt"] != null)
        {
            dt = (DataTable)ViewState["Expdt"];
            foreach (DataRow Dr in dt.Rows)
            {
                if (Dr["Expense_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                {
                    b = true;
                    i = dt.Rows.IndexOf(Dr);
                }
            }
            if (!b)
            {
                dt.Rows.Add();
                i = dt.Rows.Count - 1;
            }
            else
            {
                DisplayMessage(ddlExpense.SelectedItem.ToString() + " already Exists.");
                ddlExpCurrency.Focus();
                return;
            }
        }
        else
        {
            dt.Columns.Add("Expense_Id");
            dt.Columns.Add("Account_No");
            //dt.Columns.Add("Debit_Account_No");
            dt.Columns.Add("Exp_Charges");
            dt.Columns.Add("ExpCurrencyID");
            dt.Columns.Add("ExpExchangeRate");
            dt.Columns.Add("FCExpAmount");
            dt.Columns.Add("field3"); //exp invoice no
            dt.Columns.Add("field4"); // exp invoice date
            dt.Rows.Add();
        }
        dt.Rows[i]["Expense_Id"] = ddlExpense.SelectedValue.ToString();
        dt.Rows[i]["Account_No"] = GetAccountId(txtExpensesAccount.Text);
        if (ddlExpCurrency.SelectedValue != Session["LocCurrencyId"].ToString())
        {
            dt.Rows[i]["FCExpAmount"] = SetDecimal(txtFCExpAmount.Text);
        }
        //dt.Rows[i]["Debit_Account_No"] = GetAccountId(Txt_Debit_Expenses_Account.Text);
        dt.Rows[i]["Exp_Charges"] = SetDecimal(txtExpCharges.Text.Trim());
        dt.Rows[i]["ExpCurrencyID"] = ddlExpCurrency.SelectedValue.ToString();
        dt.Rows[i]["ExpExchangeRate"] = txtExpExchangeRate.Text.ToString();
        dt.Rows[i]["field3"] = txtExpInvoiceNo.Text.Trim();
        dt.Rows[i]["field4"] = txtExpInvoiceDate.Text.ToString();
        txtExpCharges.Text = "0";
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";
        txtExpExchangeRate.Text = "0";
        txtFCExpAmount.Text = "0";
        txtExpInvoiceNo.Text = "0";
        txtExpInvoiceDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        FillCurrency(ddlExpCurrency);
        ddlExpCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        ddlExpCurrency_SelectedIndexChanged(null, null);
        ViewState["Expdt"] = dt;
        fillExpGrid(dt);
        DataTable dtPay = new DataTable();
        dtPay = (DataTable)ViewState["PayementDt"];
        if (dt != null)
        {
            fillPaymentGrid(dtPay);
        }
        Btn_Exp_Reset_Click(null, null);
    }
    protected void Imgbtn_Expesnses_Tax_GV_Command(object sender, CommandEventArgs e)
    {
        if (Session["Expenses_Tax_Purchase_Invoice"] != null)
        {
            DataTable Dt_Cal = Session["Expenses_Tax_Purchase_Invoice"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                Dt_Cal = new DataView(Dt_Cal, "Expenses_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxExpenses_tax.DataSource = Dt_Cal;
                    gvTaxExpenses_tax.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Expenses_Tax()", true);
                }
                else
                {
                    DisplayMessage("No Tax Details found");
                    return;
                }
            }
        }
        else
        {
            DisplayMessage("No Tax Details found");
            return;
        }
    }
    protected void Btn_Exp_Reset_Click(object sender, EventArgs e)
    {
        Expenses_Tax_Modal.Expenses_Tax_And_Amount_Clear();
        Session["Expenses_Purchase_Invoice_Local"] = null;
        ddlExpCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        ddlExpCurrency_SelectedIndexChanged(null, null);
        ddlExpense.SelectedValue = "--Select--";
        txtExpensesAccount.Text = "";
        txtExpCharges.Text = "0.00";
        txtFCExpAmount.Text = "0.00";
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
        ddlExpCurrency.Enabled = true;
        txtExpExchangeRate.Enabled = true;
        ddlExpense.Enabled = true;
        txtExpensesAccount.Enabled = true;
        txtExpCharges.Enabled = true;
        txtFCExpAmount.Enabled = true;
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
        txtExpInvoiceNo.Text = "0";
        txtExpInvoiceDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
    }
    public void Expenses_Read_Only()
    {
        ddlExpCurrency.Enabled = false;
        txtExpExchangeRate.Enabled = false;
        ddlExpense.Enabled = false;
        txtExpensesAccount.Enabled = false;
        txtExpCharges.Enabled = false;
        txtFCExpAmount.Enabled = false;
    }
    public string SetDecimal(string amount)
    {
        return SystemParameter.GetAmountWithDecimal(amount, hdnDecimalCount.Value);

    }
    public void DeleteRowFromTempProductTaxTable(string ProductId, string Serial_No)
    {

    }
    public string Convert_Into_DF(string Amount)
    {
        try
        {
            if (Amount == "")
                Amount = "0";
            if (Decimal_Count_For_Tax.ToString() == "")
                Decimal_Count_For_Tax = 0;
            if (Decimal_Count_For_Tax == 0)
            {
                string Decimal_Count = string.Empty;
                Decimal_Count = Session["LoginLocDecimalCount"].ToString();
                Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
            }
            decimal Amount_D = Convert.ToDecimal((Convert.ToDouble(Amount)).ToString("F7"));
            Amount = Amount_D.ToString();
            int index = Amount.ToString().LastIndexOf(".");
            if (index > 0)
                Amount = Amount.Substring(0, index + (Decimal_Count_For_Tax + 1));
            return Amount;
        }
        catch
        {
            return "0";
        }
    }
    public string Convert_Into_DF(string Amount, int decimalCount)
    {
        try
        {
            if (Amount == "")
                Amount = "0";
            decimal Amount_D = Convert.ToDecimal((Convert.ToDouble(Amount)).ToString("F7"));
            Amount = Amount_D.ToString();
            int index = Amount.ToString().LastIndexOf(".");
            if (index > 0)
                Amount = Amount.Substring(0, index + (decimalCount + 1));
            return Amount;
        }
        catch
        {
            return "0";
        }
    }
    protected void lnkSupplierAccountMaster_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string strSupplierId = arguments[0];
        UcAcMaster.setUcAcMasterValues(strSupplierId, "Supplier", myButton.Text);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] txtPayAccountNo_TextChanged(string accountName, string id, string txtBoxId)
    {
        string[] result = new string[2];
        try
        {
            if (!new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString()).validateAccount(accountName, id, HttpContext.Current.Session["CompId"].ToString()))
            {
                result[0] = "false";
                result[1] = "No Account Found";
            }
            else
            {
                result[0] = "true";
                result[1] = "true";
                if (txtBoxId.Contains("txtPurchaseAccount"))
                {
                    HttpContext.Current.Session["AccountId"] = id;
                }
            }
        }
        catch
        {
            result[0] = "false";
            result[1] = "No Account Found";
            return result;
        }
        return result;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlPayCurrency_SelectedIndexChanged(string ddlPayCurrency)
    {
        string txtPayExchangeRate = SystemParameter.GetExchageRate(ddlPayCurrency, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        return txtPayExchangeRate;
    }

    protected void IbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/PI", "Purchase", "Purchase Invoice", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvPurchaseInvoice, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvPurchaseInvoice, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, EventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void ddlLocList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGrid();
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

    protected void chkShortProductName_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            Label lblDetailName = (Label)gvRow.FindControl("lblProductId");
            Label lblShortName = (Label)gvRow.FindControl("lblShortProductName1");
            if (((CheckBox)sender).Checked)
            {
                lblDetailName.Visible = true;
                lblShortName.Visible = false;
                ((CheckBox)sender).ToolTip = "Display short name";
            }
            else
            {
                lblDetailName.Visible = false;
                lblShortName.Visible = true;
                ((CheckBox)sender).ToolTip = "Display detail name";
            }
        }
    }
}