using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Data.SqlClient;
using MarketplaceWebServiceOrders;
using System.Xml;
using System.IO;

public partial class Sales_SalesOrder1 : BasePage
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
    Set_DocNumber objDocNo = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_ParameterMaster objInvParam = null;
    Set_CustomerMaster objCustomer = null;
    DataAccessClass objda = null;
    LocationMaster objLocation = null;
    Inv_ProductCategory_Tax objProTax = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    TaxMaster objTaxMaster = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Inv_ProductionRequestHeader objProductionRequest = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    SalesOrderPrint objReport = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    NotificationMaster Obj_Notifiacation = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    Ac_AccountMaster objAcAccountMaster = null;
    UserMaster ObjUser = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string StrUserId = string.Empty;
    string StrCurrencyId = string.Empty;

    public static int Decimal_Count_For_Tax;
    PageControlsSetting objPageCtlSettting = null;
    public const int grdDefaultColCount = 8;
    private const string strPageName = "SalesOrder";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

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
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objProTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objProductionRequest = new Inv_ProductionRequestHeader(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objReport = new SalesOrderPrint(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        try
        {
            StrCurrencyId = ddlCurrency.SelectedValue;
        }
        catch
        {

        }

        if (Request.QueryString["Id"] != null)
        {
            DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId("0", "0", "0", Request.QueryString["Id"].ToString());
            if (dtOrderEdit.Rows.Count > 0)
            {
                StrCompId = dtOrderEdit.Rows[0]["Company_Id"].ToString();
                StrBrandId = dtOrderEdit.Rows[0]["Brand_Id"].ToString();
                StrLocationId = dtOrderEdit.Rows[0]["Location_Id"].ToString();
            }
            else
            {
                StrCompId = Session["CompId"].ToString();
                StrBrandId = Session["BrandId"].ToString();
                if (hdnLocationId.Value != "")
                {
                    StrLocationId = hdnLocationId.Value;
                }
                else
                {
                    StrLocationId = Session["LocId"].ToString();
                }
            }
        }
        else
        {
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            if (hdnLocationId.Value != "")
            {
                StrLocationId = hdnLocationId.Value;
            }
            else
            {
                StrLocationId = Session["LocId"].ToString();
            }
        }

        StrUserId = Session["UserId"].ToString();
        btnSOrderSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSOrderSave, "").ToString());
        if (!IsPostBack)
        {
            Session["isSalesTaxEnabled"] = null;
            Session["IsSalesDiscountEnabled"] = null;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/SalesOrder1.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            using (DataTable dtLocation = new DataView(objLocation.GetLocationMaster(Session["CompId"].ToString()), "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable())
            {
                Session["dtLocation"] = dtLocation;
            }
            AllPageCode(clsPagePermission);
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillUser();
            Session["ContactID"] = "0";
            Session["Temp_Product_Tax_SO"] = null;
            Session["AddCtrl_State_Id"] = "";
            Session["AddCtrl_Country_Id"] = "";
            bool Tax_Apply = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            hdnIsTaxEnabled.Value = Tax_Apply.ToString();
            Session["Is_Tax_Apply"] = Tax_Apply.ToString();
            Decimal_Count_For_Tax = 3;

            //cmn.FillUser(StrCompId, StrUserId, ddlUser, objObjectEntry.GetModuleIdAndName("67").Rows[0]["Module_Id"].ToString(), "67");
            ddlOption.SelectedIndex = 2;
            FillPaymentMode();
            txtSONo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtSONo.Text;
            FillCurrency();
            try
            {
                ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                StrCurrencyId = ddlCurrency.SelectedValue;
            }
            catch
            {

            }
            ddlCurrency_OnSelectedIndexChanged(null, null);
            FillGrid(1);

            CalendarExtender.Format = Session["DateFormat"].ToString();
            Calender.Format = Session["DateFormat"].ToString();
            txtSODate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueQuoteDate.Format = Session["DateFormat"].ToString();
            Session["DtSearchProduct"] = null;
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), ddlCurrency.SelectedValue, Session["DBConnection"].ToString());
            btnAddCustomer.Visible = IsAddCustomerPermission();
            FillTransactionType();

            //this code for when we redirect from the producte ledger page 
            //this code created on 22-07-2015
            //code start

            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";

                DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId("0", "0", "0", Request.QueryString["Id"].ToString());
                if (dtOrderEdit.Rows.Count > 0)
                {
                    StrCompId = dtOrderEdit.Rows[0]["Company_Id"].ToString();
                    StrBrandId = dtOrderEdit.Rows[0]["Brand_Id"].ToString();
                    StrLocationId = dtOrderEdit.Rows[0]["Location_Id"].ToString();
                }
                else
                {
                    StrLocationId = Session["LocId"].ToString();
                }

                btnEdit_Command(imgeditbutton, new CommandEventArgs(StrLocationId, Request.QueryString["Id"].ToString()));

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_List_Hide()", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Bin_Hide()", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Quotation_Hide()", true);

                btnSOrderCancel.Visible = false;

                btnAddCustomer.Visible = false;
                rbtnFormView.Visible = false;
                rbtnAdvancesearchView.Visible = false;
                btnAddNewProduct.Visible = false;
            }
            else
            {
                btnSOrderCancel.Visible = true;
            }

            //code end


            //fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);

            //for showing deliver voucher according inventroy parameter
            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Delivery Voucher allow").Rows[0]["ParameterValue"].ToString()))
            {
                lbldeliveryVoucher.Visible = true;
                //lblcolondeliveryVoucher.Visible = true;
                ddlDeliveryvoucher.Visible = true;
            }

            ddlOrderType.SelectedIndex = 1;
            ddlOrderType_SelectedIndexChanged(null, null);


            DataTable Dt_Individual = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Allow TAX edit on individual transactions Sales");
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

            if (Request.QueryString["OrderNo"] != null)
            {
                ddlPosted.SelectedValue = "0";
                txtValue.Text = Request.QueryString["OrderNo"].ToString();
                btnbindrpt_Click(null, null);
            }
            TaxandDiscountParameter();

            //here we set tru visibility to get ordre button
            if (Session["LocId"].ToString() == "7")
            {
                pnlGetOrder.Visible = true;
            }
            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == false)
            {
                Trans_Div.Visible = false;
            }

            hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
            //AllPageCode();
            txtEstimateDeliveryDate.Text = DateTime.Now.ToString(ObjSysParam.SetDateFormat());
            getPageControlsVisibility();



            if (Request.QueryString["Id"] != null)
            {
                ViewState["View"] = "Yes";
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                btnEdit_Command(imgeditbutton, new CommandEventArgs(StrLocationId, Request.QueryString["Id"].ToString()));

                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
        }

        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
        Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }

        //try
        //{
        //    GetAmazonSalesOrder();
        //}
        //catch(Exception ex)
        //{

        //}                    
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
    private void FillCurrency()
    {
        using (DataTable dsCurrency = objCurrency.GetCurrencyMaster())
        {
            if (dsCurrency.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            }
            else
            {
                ddlCurrency.Items.Add("--Select--");
                ddlCurrency.SelectedValue = "--Select--";
            }
        }
    }
    protected void ddlCurrency_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        hdnDecimalCount.Value = "2";
        if (ddlCurrency.SelectedIndex != 0)
        {
            StrCurrencyId = ddlCurrency.SelectedValue;
            txtExchangerate.Text = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()), Session["DBConnection"].ToString());

            using (DataTable dt = objda.return_DataTable("select Sys_CurrencyMaster.Currency_Code,Sys_CurrencyMaster.field2 as smallestDenomiation,case when Sys_Country_Currency.field1 is null or Sys_Country_Currency.field1 ='' then '0' else Sys_Country_Currency.field1 end as decimal from Sys_CurrencyMaster left join Sys_Country_Currency on Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.Currency_ID where Sys_CurrencyMaster.Currency_Id ='" + ddlCurrency.SelectedValue + "'"))
            {
                hdnDecimalCount.Value = dt.Rows[0]["decimal"].ToString();
                hdnDenomination.Value = dt.Rows[0]["smallestDenomiation"].ToString();
                if (hdnDecimalCount.Value == "")
                {
                    hdnDecimalCount.Value = "2";
                }

                if (hdnDenomination.Value == "")
                {
                    hdnDenomination.Value = "1";
                }
            }


            if (ddlCurrency.SelectedValue == SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()))
            {
                txtExchangerate.Enabled = false;
                //hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
            }
            else
            {


                //hdnDecimalCount.Value = objCurrency.GetDecimalCountByCurrencyId(ddlCurrency.SelectedValue);

                txtExchangerate.Enabled = true;
            }
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetCreditInfo(string strCustomerId, string strCurrencyId)
    {
        try
        {
            string strCreditLimit = "0";
            string strCreditDays = "0";
            string strCreditContiditon = "";
            //Get Customer account id based on cutomer_id and currency_id
            Ac_AccountMaster objAcAccountMaster = new Ac_AccountMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(strCustomerId, strCurrencyId).ToString();
            if (strOtherAccountId == "0")
            {
                return null;
            }
            Set_CustomerMaster_CreditParameter objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(HttpContext.Current.Session["DBConnection"].ToString());
            using (DataTable dtCreditParameter = objCustomerCreditParam.GetCustomerRecord_By_OtherAccountId(strOtherAccountId))
            {
                if (dtCreditParameter.Rows.Count > 0)
                {
                    strCreditLimit = SystemParameter.GetAmountWithDecimal(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim(), HttpContext.Current.Session["LoginLocDecimalCount"].ToString());
                    strCreditDays = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
                    if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
                    {
                        strCreditContiditon = "Advance Cheque Basis";
                    }
                    else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
                    {
                        strCreditContiditon = "Invoice to Invoice Payment";
                    }
                    else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
                    {
                        strCreditContiditon = "50% advance and 50% on delivery";
                    }
                    else
                    {
                        strCreditContiditon = "None";
                    }
                }
            }
            return strCreditLimit + "," + strCreditDays + "," + strCreditContiditon;
        }
        catch
        {
            return null;
        }
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    void TaxandDiscountParameter()
    {
        hdnIsDiscountEnable.Value = false.ToString();
        hdnIsTaxEnabled.Value = false.ToString();

        //lblAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, Resources.Attendance.Gross_Total);
        //lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");
        //lblNetAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");


        if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
        {

            GvQuotationDetail.Columns[13].Visible = false;
            GvQuotationDetail.Columns[14].Visible = false;
            GvQuotationDetail.Columns[15].Visible = false;
            GvProductDetail.Columns[0].Visible = false;
            GvProductDetail.Columns[14].Visible = false;
            GvProductDetail.Columns[15].Visible = false;
            GvProductDetail.Columns[16].Visible = false;

            //lblTaxP.Visible = false;
            //lblTaxPcolon.Visible = false;
            txtTaxP.Visible = false;
            Label2.Visible = false;
            txtTaxV.Visible = false;

            //lblPTax.Visible = false;
            //lblPTaxcolon.Visible = false;

            Label4.Visible = false;


            txtPTaxPUnit.Visible = false;
            txtPTaxVUnit.Visible = false;
            Div_Tax.Visible = false;
            gridView.Visible = false;


        }
        else
        {

            GvQuotationDetail.Columns[13].Visible = true;
            GvQuotationDetail.Columns[14].Visible = true;
            hdnIsTaxEnabled.Value = true.ToString();
            GvProductDetail.Columns[14].Visible = true;
            GvProductDetail.Columns[15].Visible = true;
            GvProductDetail.Columns[0].Visible = true;
            //lblTaxP.Visible = true;
            //lblTaxPcolon.Visible = true;
            txtTaxP.Visible = true;
            Label2.Visible = true;
            txtTaxV.Visible = true;
            gridView.Visible = true;
        }


        if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
        {
            GvQuotationDetail.Columns[10].Visible = false;
            GvQuotationDetail.Columns[11].Visible = false;
            GvQuotationDetail.Columns[12].Visible = false;

            GvProductDetail.Columns[11].Visible = false;
            GvProductDetail.Columns[12].Visible = false;
            GvProductDetail.Columns[13].Visible = false;

            //lblDiscountP.Visible = false;
            txtDiscountP.Visible = false;
            Label3.Visible = false;
            txtDiscountV.Visible = false;
            //lblDiscountPvolon.Visible = false;
            //lblPDiscount.Visible = false;
            //lblPDiscountcolon.Visible = false;


            txtPDiscountPUnit.Visible = false;
            txtPDiscountVUnit.Visible = false;

            Label5.Visible = false;
            Label13.Visible = false;
            //Label12ViewDisPcolon.Visible = false;


            //lblafterDiscountPrice.Visible = false;
            //lblPriceafterdiscountcolon.Visible = false;
            txtPriceAfterDiscount.Visible = false;
            hdnIsDiscountEnable.Value = false.ToString();
        }
        else
        {

            hdnIsDiscountEnable.Value = true.ToString();
            //lblafterDiscountPrice.Visible = true;
            //lblPriceafterdiscountcolon.Visible = true;
            txtPriceAfterDiscount.Visible = true;
            GvQuotationDetail.Columns[10].Visible = true;
            GvQuotationDetail.Columns[11].Visible = true;
            GvProductDetail.Columns[11].Visible = true;
            GvProductDetail.Columns[12].Visible = true;
            //lblDiscountP.Visible = true;
            txtDiscountP.Visible = true;
            Label3.Visible = true;
            //Label3.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Value");
            txtDiscountV.Visible = true;
            //lblDiscountPvolon.Visible = true;
            //lblPDiscount.Visible = true;
            //lblPDiscountcolon.Visible = true;

            Label5.Visible = true;


            txtPDiscountPUnit.Visible = true;
            txtPDiscountVUnit.Visible = true;
            Label13.Visible = true;
            //Label12ViewDisPcolon.Visible = true;

        }



    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSOrderSave.Visible = clsPagePermission.bAdd;
        //GvSalesOrder.Columns[2].Visible = clsPagePermission.bEdit;
        //GvSalesOrder.Columns[3].Visible = clsPagePermission.bDelete;
        //GvSalesOrder.Columns[1].Visible = clsPagePermission.bView;
        //GvSalesOrder.Columns[4].Visible = clsPagePermission.bUpload;

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();



        imgBtnRestore.Visible = clsPagePermission.bRestore;
        ImgbtnSelectAll.Visible = clsPagePermission.bRestore;
        txtAgentName.Enabled = clsPagePermission.bPayCommission;
        txtSODate.ReadOnly = clsPagePermission.bModifyDate;
        Calender.Enabled = clsPagePermission.bModifyDate;
        //ddlUser.Visible = clsPagePermission.bViewAllUserRecord;
        hdnCanViewAllRec.Value = clsPagePermission.bViewAllUserRecord.ToString().ToLower();
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "13", "67", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #region System defined Function

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        bool isTaxApplicable = Inventory_Common.IsSalesTaxEnabled(e.CommandName.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //if (Convert.ToBoolean(Session["isSalesTaxEnabled"].ToString()) != isTaxApplicable)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Tax is not enabled on this location, do you want to continue ?');", true);
        //    return;
        //}


        Hdn_Edit_ID.Value = e.CommandArgument.ToString();
        hdnLocationId.Value = e.CommandName.ToString();

        DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
        if (dtOrderEdit.Rows[0]["Trans_Type"].ToString() != "")
            ddlTransType.SelectedValue = dtOrderEdit.Rows[0]["Trans_Type"].ToString();
        else
            ddlTransType.SelectedIndex = 0;


        string PaymentType = string.Empty;

        try
        {
            PaymentType = new DataView(objPaymentMode.GetPaymentModeMaster(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Pay_Mode_Id=" + dtOrderEdit.Rows[0]["PaymentModeId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString();
        }
        catch
        {
            PaymentType = "Cash";
        }

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        if (dtOrderEdit.Rows.Count > 0)
        {
            //SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            //con.Open();
            //SqlTransaction trns;
            //trns = con.BeginTransaction();

            //Update Approval Section By Lokesh On 22-09-2022
            if (objSenderID != "lnkViewDetail")
            {
                //Code for Approval
                DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesOrderApproval");
                if (DtPara.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
                    {
                        if (PaymentType.Trim() == "Credit" || (PaymentType.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString().Trim()) == true))
                        {

                            if (dtOrderEdit.Rows[0]["Field4"].ToString() == "Approved")
                            {

                                //string strsql = "update Inv_SalesOrderHeader set Field4='Pending' where Trans_Id='"+ e.CommandArgument.ToString() + "'";
                                //objda.execute_Command(strsql, ref trns);

                                //string strApprovesql = "update Set_Approval_Transaction set Status='Pending',Description='', Field1='' where Ref_Id='" + e.CommandArgument.ToString() + "' and Approval_Id='9'";
                                //objda.execute_Command(strApprovesql, ref trns);


                                //DisplayMessage("Order Approved, cannot be Edited");
                                //return;
                            }
                        }
                    }
                    else
                    {
                        if (PaymentType.Trim() == "Credit" || (PaymentType.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString().Trim()) == true))
                        {

                            if (dtOrderEdit.Rows[0]["Field4"].ToString() == "Approved")
                            {
                                //string strsql = "update Inv_SalesOrderHeader set Field4='Pending' where Trans_Id='" + e.CommandArgument.ToString() + "'";
                                //objda.execute_Command(strsql, ref trns);

                                //string strApprovesql = "update Set_Approval_Transaction set Status='Pending',Description='', Field1='' where Ref_Id='" + e.CommandArgument.ToString() + "' and Approval_Id='9'";
                                //objda.execute_Command(strApprovesql, ref trns);
                            }
                        }
                    }
                }
                //End Approval Code
            }

            //trns.Commit();
            //if (con.State == System.Data.ConnectionState.Open)
            //{
            //    con.Close();
            //}
            //trns.Dispose();
            //con.Dispose();



            //For Checking Finance Record
            if (editid.Value != "" && editid.Value != "0")
            {
                DataTable dtFinance = objVoucherHeader.GetVoucherHeaderAllTrue(StrCompId, StrBrandId, StrLocationId, HttpContext.Current.Session["FinanceYearId"].ToString());

                DataTable dtPosted = new DataView(dtFinance, "ReconciledFromFinance='True' and Ref_Type='SO' and Ref_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtPosted.Rows.Count > 0)
                {
                    DisplayMessage("Your Finance Record was Effected So you cant Edit That Record");
                    return;
                }
            }


            editid.Value = e.CommandArgument.ToString();
            string strCustomerId = dtOrderEdit.Rows[0]["CustomerId"].ToString();

            DataTable dtCustomerName = objda.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
            if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
            {
                string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                txtCustomer.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                //txtCustomer.Text = dtOrderEdit.Rows[0]["CustomerName"].ToString() + "/" + strCustomerId;   
            }



            Session["ContactID"] = strCustomerId;

            // added by divya on 18-8-2018
            if (dtOrderEdit.Rows[0]["Sales_Person_Name"].ToString() != "")
            {
                txtSalesPerson.Text = dtOrderEdit.Rows[0]["Sales_Person_Name"].ToString() + "/" + dtOrderEdit.Rows[0]["Sales_Person_Code"].ToString();
                hdnSalesPersonId.Value = dtOrderEdit.Rows[0]["SalesPerson_Id"].ToString();
            }
            else
            {
                hdnSalesPersonId.Value = "0";
            }
            if (dtOrderEdit.Rows[0]["Contact_Person_Name"].ToString() != "")
            {
                txtContactPerson.Text = dtOrderEdit.Rows[0]["Contact_Person_Name"].ToString() + "/" + dtOrderEdit.Rows[0]["Contact_Number"].ToString() + "/" + dtOrderEdit.Rows[0]["Contact_Email"].ToString() + "/" + dtOrderEdit.Rows[0]["contactperson_id"].ToString();
                hdnContactPersonId.Value = dtOrderEdit.Rows[0]["ContactPerson_Id"].ToString();
            }
            else
            {
                hdnContactPersonId.Value = "0";
            }
            //end

            //GetCreditInfo();
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

            txtSONo.Text = dtOrderEdit.Rows[0]["SalesOrderNo"].ToString();


            ViewState["TimeStamp"] = dtOrderEdit.Rows[0]["Row_Lock_Id"].ToString();
            txtSONo.ReadOnly = true;
            txtSODate.Text = Convert.ToDateTime(dtOrderEdit.Rows[0]["SalesOrderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            string strSOFromTransType = dtOrderEdit.Rows[0]["SOfromTransType"].ToString();
            txtCustOrderNo.Text = dtOrderEdit.Rows[0]["Field3"].ToString();
            ViewState["Status"] = dtOrderEdit.Rows[0]["Field4"].ToString();
            ddlCurrency.SelectedValue = dtOrderEdit.Rows[0]["Currency_Id"].ToString();
            StrCurrencyId = ddlCurrency.SelectedValue;
            //lblAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, Resources.Attendance.Gross_Total);
            //lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");
            //lblNetAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");
            ddlDeliveryvoucher.SelectedValue = dtOrderEdit.Rows[0]["IsdeliveryVoucher"].ToString();

            ///here we checking that  sale sorder alllowd for project management or not 
            ///

            chkSendInProjectManagement.Checked = Convert.ToBoolean(dtOrderEdit.Rows[0]["Post"].ToString());

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
                rbtNew.Visible = false;
                rbtEdit.Visible = false;
                txtAgentName.Enabled = false;
                ddlOrderType.SelectedValue = "Q";
                ddlOrderType_SelectedIndexChanged(sender, e);
                ddlQuotationNo.Visible = false;
                txtQuotationNo.Visible = true;
                hdnSalesQuotationId.Value = dtOrderEdit.Rows[0]["SOfromTransNo"].ToString();


                DataTable dtQuotationHeader = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value);
                if (dtQuotationHeader.Rows.Count > 0)
                {
                    txtQuotationNo.Text = dtQuotationHeader.Rows[0]["SQuotation_No"].ToString();

                    DataTable dtQuoteChild = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value, Session["FinanceYearId"].ToString());
                    if (dtQuoteChild.Rows.Count > 0)
                    {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)GvQuotationDetail, dtQuoteChild, "", "");
                        Get_Tax_From_DB();
                        GvQuotationDetail.Columns[12].Visible = false;
                        GvQuotationDetail.Columns[15].Visible = false;
                        FillUnit();
                        Decimal fGrossTotal = 0;
                        Decimal fquantityTotal = 0;
                        DataTable dtOrderDetailByProductId = new DataTable();
                        DataTable dtOrderDetailAll = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, editid.Value);
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
                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");

                            Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");



                            dtOrderDetailByProductId = new DataView(dtOrderDetailAll, "Product_Id='" + hdngvProductId.Value + "' and Serial_No='" + lblgvSerialNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                            if (dtOrderDetailByProductId.Rows.Count > 0)
                            {
                                chkgvQuantitySelect.Checked = true;
                                lblgvQuantity.Text = dtOrderDetailByProductId.Rows[0]["Quantity"].ToString();
                                txtgvFreeQuantity.Text = dtOrderDetailByProductId.Rows[0]["FreeQty"].ToString();

                                ddlgvUnit.SelectedValue = dtOrderDetailByProductId.Rows[0]["UnitId"].ToString();
                                txtgvRemainQuantity.Text = dtOrderDetailByProductId.Rows[0]["RemainQty"].ToString();
                                txtgvUnitPrice.Text = dtOrderDetailByProductId.Rows[0]["UnitPrice"].ToString();
                                if (dtOrderDetailByProductId.Rows[0]["Field2"].ToString() != "0")
                                {
                                    ddlLocation.SelectedValue = dtOrderDetailByProductId.Rows[0]["Field2"].ToString();

                                }
                                txtgvTaxP.Text = dtOrderDetailByProductId.Rows[0]["TaxP"].ToString();
                                txtgvTaxV.Text = dtOrderDetailByProductId.Rows[0]["TaxV"].ToString();
                                //txtgvPriceAfterTax.Text = (Decimal.Parse(txtgvQuantityPrice.Text) + Decimal.Parse(txtgvTaxV.Text)).ToString();
                                txtgvDiscountP.Text = dtOrderDetailByProductId.Rows[0]["DiscountP"].ToString();
                                txtgvDiscountV.Text = dtOrderDetailByProductId.Rows[0]["DiscountV"].ToString();
                                txtgvAgentCommission.Text = GetAmountDecimal(dtOrderDetailByProductId.Rows[0]["AgentCommission"].ToString());
                                string[] strcalc = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, "", txtgvTaxV.Text, "", txtgvDiscountV.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());
                                //chkIsProduction.Checked = Convert.ToBoolean(dtOrderDetailByProductId.Rows[0]["Field6"].ToString());
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
                        dtOrderDetailAll = null;
                        dtOrderDetailByProductId = null;
                        //Grid_Calculation("GvQuotationDetail", "Get Product Tax");
                        txtAmount.Text = fquantityTotal.ToString();
                        txtTotalAmount.Text = fGrossTotal.ToString();
                    }
                }
            }
            else
            {
                ddlOrderType.SelectedValue = "D";
                ddlOrderType_SelectedIndexChanged(sender, e);
                rbtNew.Visible = true;
                rbtEdit.Visible = true;
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
                    Get_Tax_From_DB();
                    DataTable dtOrderDetailByProductId = new DataTable();
                    DataTable dtsalesOrderDetail = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, editid.Value);
                    foreach (GridViewRow gvr in GvProductDetail.Rows)
                    {
                        DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                        HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");



                        dtOrderDetailByProductId = new DataView(dtsalesOrderDetail, "Product_Id='" + hdngvProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtOrderDetailByProductId.Rows.Count > 0)
                        {
                            if (dtOrderDetailByProductId.Rows[0]["Field2"].ToString() != "0")
                            {
                                ddlLocation.SelectedValue = dtOrderDetailByProductId.Rows[0]["Field2"].ToString();

                            }
                        }
                    }
                    dtOrderDetailByProductId = null;
                    dtsalesOrderDetail = null;
                    Grid_Calculation("GvProductDetail", "Add Product");
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

            DataTable dtShipCustomerName = objda.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
            if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
            {
                string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                txtShipCustomerName.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                //txtShipCustomerName.Text = dtOrderEdit.Rows[0]["ShipCustomerName"].ToString() + "/" + strCustomerId;
            }



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
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
            if (dtOrderEdit.Rows[0]["SOfromTransType"].ToString() == "D")
            {
                GetGridTotal();
            }
            txtShippingCharge.Text = dtOrderEdit.Rows[0]["ShippingCharge"].ToString();
            txtRemark.Text = dtOrderEdit.Rows[0]["Remark"].ToString();

            txtNetAmount.Text = (Decimal.Parse(txtTotalAmount.Text) + Decimal.Parse(txtShippingCharge.Text)).ToString();
            string strPost = dtOrderEdit.Rows[0]["Post"].ToString();
            string strSendInPO = dtOrderEdit.Rows[0]["IsInPO"].ToString();
            dtAddName = null;

        }

        ddlCurrency_OnSelectedIndexChanged(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);

        if (objSenderID == "lnkViewDetail")
        {
            btnSOrderSave.Enabled = false;
            BtnReset.Visible = false;
        }
        else
        {
            btnSOrderSave.Enabled = true;
            BtnReset.Visible = true;
        }
        TaxCalculationWithDiscount();
        setDecimal();
        txtDiscountP_TextChanged(null, null);

        Update_New.Update();
        Update_Li.Update();
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "AddressDetailFunctions();", true);
    }
    protected void GvSalesOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesOrder.Attributes["CurrentSortField"] != null &&
            GvSalesOrder.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesOrder.Attributes["CurrentSortField"])
            {
                if (GvSalesOrder.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesOrder.Attributes["CurrentSortField"] = sortField;
        GvSalesOrder.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvSalesOrderCurrentPageIndex.Value));
    }
    protected void GvSalesOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterSorder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");

        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "SalesOrderDate")
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
                return;
            }
        }

        FillGrid(1);

        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        string strOrderId = e.CommandArgument.ToString();
        if (strOrderId != "" && strOrderId != "0")
        {
            DataTable dtFinance = objda.return_DataTable("select Trans_Id from Ac_Voucher_Header where Field1='SO' and Field2=" + strOrderId + " and reconciledfromfinance='True'");
            if (dtFinance.Rows.Count > 0)
            {
                DisplayMessage("Your Finance Record was Effected So you cant delete");
                dtFinance = null;
                return;
            }
        }

        if (((Label)gvRow.FindControl("lblInvoiceStatus")).Text.Trim() == "Created")
        {

            DisplayMessage("Sales order is used in Sales invoice");
            return;
        }



        int b = 0;

        DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());

        string PaymentType = string.Empty;

        try
        {
            PaymentType = new DataView(objPaymentMode.GetPaymentModeMaster(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Pay_Mode_Id=" + dtOrderEdit.Rows[0]["PaymentModeId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString();
        }
        catch
        {
            PaymentType = "Cash";
        }


        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                if (PaymentType.Trim() == "Credit" || (PaymentType.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString().Trim()) == true))
                {
                    string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                    if (st == "Approved")
                    {
                        DisplayMessage("Order Approved, cannot be Deleted");
                        return;

                    }
                }
            }

        }
        //End Approval Code
        b = objSOrderHeader.DeleteSOHeader(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString(), "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            objEmpApproval.Delete_Approval_Transaction("9", StrCompId, StrBrandId, StrLocationId, "0", e.CommandArgument.ToString());
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillGridBin(); //Update grid view in bin tab
        FillGrid(Convert.ToInt32(hdnGvSalesOrderCurrentPageIndex.Value));
        Reset();
        //AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        FillGrid(1);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }
    protected void btnSOrderCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../sales/salesorder1.aspx");
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);
        //AllPageCode();
    }

    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
            ViewState["Emp_Img"] = "";
        }
        return strEmployeeName;
    }

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/sales"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Sales Order. on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
    }




    public string getOrderAcknowledgement(string strOrderId)
    {
        string strBody = string.Empty;

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string Companytelno = "";
        string CompanyFaxno = "";
        string CompanyWebsite = "";

        SalesDataSet objSalesdataset = new SalesDataSet();
        objSalesdataset.EnforceConstraints = false;
        SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(objSalesdataset.sp_Inv_SalesOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(strOrderId));
        DataTable Dt = objSalesdataset.sp_Inv_SalesOrderDetail_SelectRow_Report;


        string[] strParam = Common.ReportHeaderSetup("Location", Session["LocId"].ToString(), Session["DBConnection"].ToString());
        CompanyName = strParam[0].ToString();

        CompanyAddress = strParam[2].ToString();
        Companytelno = strParam[3].ToString();
        CompanyFaxno = strParam[4].ToString();
        CompanyWebsite = strParam[5].ToString();
        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();



        DevExpress.XtraReports.UI.PageFooterBand GF = (DevExpress.XtraReports.UI.PageFooterBand)objReport.FindControl("PageFooter", true);


        DevExpress.XtraReports.UI.XRLabel lbl1 = (DevExpress.XtraReports.UI.XRLabel)objReport.FindControl("xrLabel43", true);
        DevExpress.XtraReports.UI.XRLabel lbl2 = (DevExpress.XtraReports.UI.XRLabel)objReport.FindControl("xrLabel44", true);
        DevExpress.XtraReports.UI.XRLabel lbl3 = (DevExpress.XtraReports.UI.XRLabel)objReport.FindControl("xrLabel46", true);
        DevExpress.XtraReports.UI.XRPictureBox xrPic = (DevExpress.XtraReports.UI.XRPictureBox)objReport.FindControl("xrPictureBox2", true);
        GF.Visible = false;
        lbl1.Visible = false;
        lbl2.Visible = false;
        lbl3.Visible = false;
        xrPic.Visible = false;


        objReport.setSignature("");
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName);
        //objReport.setBrandName(BrandName);
        objReport.setLocationName("");
        objReport.settitle(Title);
        objReport.SetImage(Imageurl);
        objReport.setCompanyArebicName("");
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        objReport.setUserName("");
        objReport.DataSource = Dt;
        objReport.DataMember = "sp_Inv_SalesOrderDetail_SelectRow_Report";
        rptViewer.Report = objReport;






        objReport.ExportToHtml(Server.MapPath("~/Temp/SalesOrderAcknowledgement.html"));

        strBody = File.ReadAllText(Server.MapPath("~/Temp/SalesOrderAcknowledgement.html"));

        return strBody;
    }
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());

        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    public string GetStatus(int TransID)
    {
        //return objEmpApproval.GetApprovalTransactionStatus("SalesOrder", TransID.ToString(), StrCompId);
        string ApprovalStatus = string.Empty;
        DataTable dtSO = objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, TransID.ToString());
        if (dtSO.Rows.Count > 0)
        {
            ApprovalStatus = dtSO.Rows[0]["Field4"].ToString();
        }
        else
        {
            ApprovalStatus = "";
        }
        return ApprovalStatus;
    }
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        Update_Bin.Update();
        //AllPageCode();
    }
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        FillRequestGrid(1);
        Update_Quotation.Update();
        //FillGridBin();
        //AllPageCode();
    }
    protected void GvSalesOrderBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesOrderBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilterSorderBin"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrderBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvSalesOrderBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesOrderBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesOrderBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        updPnlBin.Update();
        //AllPageCode();
    }
    protected void GvSalesOrderBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["dtFilterSorderBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterSorderBin"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrderBin, dt, "", "");
        lblSelectedRecord.Text = "";
        updPnlBin.Update();
        //AllPageCode();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objSOrderHeader.GetSOHeaderAllFalse(StrCompId, StrBrandId, StrLocationId);
        if (ddlUser.SelectedValue != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrderBin, dt, "", "");

        //GvSalesOrderBin.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(),Resources.Attendance.Price);
        Session["dtSorderBin"] = dt;
        Session["dtFilterSorderBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "SalesOrderDate")
        {
            if (txtValueBinDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }

            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                return;
            }
        }
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }

            DataTable dtCust = (DataTable)Session["dtSorderBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilterSorderBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesOrderBin, view.ToTable(), "", "");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesOrderBin, view.ToTable(), "", "");



            lblSelectedRecord.Text = "";
            //if (view.ToTable().Rows.Count == 0)
            //{
            //    FillGridBin();
            //}
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objSOrderHeader.GetSOHeaderAllFalse(StrCompId, StrBrandId, StrLocationId);

        if (GvSalesOrderBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objSOrderHeader.DeleteSOHeader(StrCompId, StrBrandId, StrLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid(1);
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate", "green");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvSalesOrderBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        fleg = 1;
                    }
                    else
                    {
                        fleg = 0;
                    }
                }
                if (fleg == 0)
                {
                    DisplayMessage("Please Select Record");
                }
                else
                {
                    DisplayMessage("Record Not Activated");
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesOrderBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesOrderBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesOrderBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvSalesOrderBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvSalesOrderBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvSalesOrderBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)GvSalesOrderBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvSalesOrderBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
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
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        // FillGrid();
        FillGridBin();

        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;

        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        //AllPageCode();
    }

    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtPbrand = (DataTable)Session["dtFilterSorderBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvSalesOrderBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvSalesOrderBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesOrderBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtFilterSorderBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesOrderBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
        //AllPageCode();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["LocId"].ToString() != ddlLocation.SelectedValue)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "redirectToHome('Login location and Order location are not same, please change location to continue, do you want to continue ?');", true);
            return;
        }
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval");
            DataTable dt1 = new DataTable();
            string EmpPermission = string.Empty;

            if (DtApprove.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                {
                    EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("SalesOrder").Rows[0]["Approval_Level"].ToString();
                    dt1 = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "67", Session["EmpId"].ToString());
                    if (dt1.Rows.Count == 0)
                    {
                        DisplayMessage("Approval setup issue , please contact to your admin");
                        return;
                    }
                }
            }


            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objSOrderHeader.DeleteSOHeader(StrCompId, StrBrandId, StrLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    if (dt1.Rows.Count > 0)
                    {
                        for (int h = 0; h < dt1.Rows.Count; h++)
                        {
                            string PriorityEmpId = dt1.Rows[h]["Emp_Id"].ToString();
                            string IsPriority = dt1.Rows[h]["Priority"].ToString();
                            if (EmpPermission == "1")
                            {
                                objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "2")
                            {
                                objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "3")
                            {
                                objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else
                            {
                                objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                        }
                    }
                }
            }
        }

        if (b != 0)
        {
            FillGrid(1);
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate", "green");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvSalesOrderBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    fleg = 1;
                }
                else
                {
                    fleg = 0;
                }
            }
            if (fleg == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
        //AllPageCode();
    }
    #endregion
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = ObjContactMaster.GetCustomerAsPerFilterText(prefixText))
        {
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
                }
            }
            return filterlist;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = objcontact.GetAllContactAsPerFilterText(prefixText))
        {
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
                }
            }
            return filterlist;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
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
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    #region User defined Function
    private void FillGrid()
    {
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "'";
        if (ddlUser.SelectedValue != "--Select User--")
        {
            strWhereClause = " and CreatedBy='" + ddlUser.SelectedValue.ToString() + "'";
        }
        if (Request.QueryString["OrderNo"] != null)
        {
            ddlPosted.SelectedIndex = 2;
        }

        if (ddlPosted.SelectedIndex != 2)
        {
            strWhereClause = strWhereClause + " and InvoiceStatus='" + ddlPosted.SelectedValue.Trim() + "'";
        }

        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        int currentPageIndex = GvSalesOrder.PageIndex;
        using (DataTable dt = objSOrderHeader.getOrderList(GvSalesOrder.PageIndex.ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesOrder.Attributes["CurrentSortField"], GvSalesOrder.Attributes["CurrentSortDirection"], strWhereClause))
        {
            objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
            if (dt.Rows.Count > 0)
            {
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
                GvSalesOrder.PageIndex = currentPageIndex;
            }
        }
        PageControlCommon.PopulatePager(rptPager, totalRows, GvSalesOrder.PageIndex);
    }
    private void FillGrid(int currentPageIndex = 1)
    {
        string strWhereClause = string.Empty;
        strWhereClause = "SOH.isActive='true'  ";
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty && ddlFieldName.SelectedValue != "ProductId")
        {
            if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = strWhereClause + " and SOH." + ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = strWhereClause + " and SOH." + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = strWhereClause + " and SOH." + ddlFieldName.SelectedValue + " Like '" + txtValue.Text.Trim() + "'";
            }
        }


        if (ddlFieldName.SelectedValue == "ProductId")
        {
            if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = strWhereClause + " and  " + " SOH.Product_Id in (Select ProductId from Inv_ProductMaster where ProductCode='" + txtValue.Text + "')";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = strWhereClause + " and  " + " SOH.Product_Id in (Select ProductId from Inv_ProductMaster where ProductCode like '%" + txtValue.Text + "%')";
            }
            else
            {
                strSearchCondition = strWhereClause + " and  " + " SOH.Product_Id in (Select ProductId from Inv_ProductMaster where ProductCode like '" + txtValue.Text + "')";
            }
        }


        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strSearchCondition;
        }



        if (ddlUser.SelectedValue != "--Select User--")
        {
            strWhereClause = strWhereClause + " and SOH.CreatedBy='" + ddlUser.SelectedValue.ToString() + "'";
        }
        else
        {
            //bool isSingleUser = false;
            //if (Session["EmpId"].ToString() != "0")
            //{

            //    isSingleUser = bool.Parse(ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString()));
            //}

            if (hdnCanViewAllRec.Value == "false")
            {
                string strAllUser = string.Empty;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value == "--Select User--")
                    {
                        continue;
                    }
                    strAllUser += "," + item.Value.ToString();
                }
                if (!string.IsNullOrEmpty(strAllUser))
                {
                    strAllUser = strAllUser.Substring(1, strAllUser.Length - 1);
                    //strWhereClause = strWhereClause + " and  case when CreatedBy = 'superadmin' then 0 else CreatedBy end in(" + strAllUser + ")";
                    strWhereClause = strWhereClause + " and  case when SOH.CreatedBy = 'superadmin' then 0 else SOH.CreatedBy end in(SELECT CAST(Value AS int) FROM F_Split('" + strAllUser + "', ','))";
                }
            }
        }
        if (Request.QueryString["OrderNo"] != null)
        {
            ddlPosted.SelectedIndex = 2;
        }

        if (ddlPosted.SelectedIndex != 2)
        {
            strWhereClause = strWhereClause + " and SOH.InvoiceStatus='" + ddlPosted.SelectedValue.Trim() + "'";
        }
        if (ddlLocation.SelectedItem.Text != "All")
        {
            strWhereClause = strWhereClause + " and  SOH.Location_Id='" + ddlLocation.SelectedValue.ToString() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and  SOH.Location_Id in (" + ddlLocation.SelectedValue.ToString() + ")";
        }

        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;

        if (ddlFieldName.SelectedValue == "ProductId")
        {
            int totalRows = 0;
            using (DataTable dt = getOrderListNew((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesOrder.Attributes["CurrentSortField"], GvSalesOrder.Attributes["CurrentSortDirection"], strWhereClause))
            {
                if (dt.Rows.Count > 0)
                {
                    objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
                    totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

                }
                else
                {
                    GvSalesOrder.DataSource = null;
                    GvSalesOrder.DataBind();
                    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
                }

                PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
            }
        }
        else
        {
            int totalRows = 0;
            using (DataTable dt = objSOrderHeader.getOrderList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesOrder.Attributes["CurrentSortField"], GvSalesOrder.Attributes["CurrentSortDirection"], strWhereClause))
            {
                if (dt.Rows.Count > 0)
                {
                    objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
                    totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

                }
                else
                {
                    GvSalesOrder.DataSource = null;
                    GvSalesOrder.DataBind();
                    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
                }

                PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
            }

        }




    }

    public DataTable getOrderListNew(string strPageIndex, string strPageSize, string strSortExpr, string strSortDire, string strWhereClause)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());

        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@PageIndex", strPageIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@PageSize", strPageSize, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@SortExpr", strSortExpr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@SortDire", strSortDire, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@WhereClause", strWhereClause, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = da.Reuturn_Datatable_Search("sp_Inv_SalesOrderHeader_By_PageIndex_New", paramList))
        {
            return dtInfo;
        }
    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        rbtNew.Checked = false;
        rbtNew.Visible = false;
        rbtEdit.Checked = true;
        rbtEdit.Visible = false;

        hdnLocationId.Value = "";
        ddlTransType.Enabled = true;
        ddlUser.SelectedIndex = 0;
        // cmn.FillUser(StrCompId, StrUserId, ddlUser, objObjectEntry.GetModuleIdAndName("67").Rows[0]["Module_Id"].ToString(), "67");
        // txtSONo.Text = objSOrderHeader.GetAutoID(StrCompId, StrBrandId, StrLocationId);
        txtSODate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtSONo.ReadOnly = false;
        txtCustomer.Text = "";
        ddlOrderType.SelectedValue = "--Select--";
        ddlOrderType_SelectedIndexChanged(null, null);
        txtTransFrom.Text = "";
        FillPaymentMode();
        txtEstimateDeliveryDate.Text = "";
        txtShipingAddress.Text = "";
        txtCustomer.Text = "";
        txtInvoiceTo.Text = "";
        txtShipCustomerName.Text = "";
        ddlDeliveryvoucher.SelectedIndex = 0;
        txtAmount.Text = "";
        txtTaxP.Text = "";
        txtTaxV.Text = "";
        txtPriceAfterTax.Text = "";
        txtDiscountP.Text = "";
        txtDiscountV.Text = "";
        txtTotalAmount.Text = "";
        txtPriceAfterDiscount.Text = "";
        pnlDetail.Enabled = true;
        txtShippingCharge.Text = "";
        txtNetAmount.Text = "";
        txtRemark.Text = "";
        txtCustOrderNo.Text = "";
        //chkPost.Checked = false;
        chkSendInPO.Checked = false;
        txtInvoiceTo.Text = "";
        txtShipCustomerName.Text = "";
        GvQuotationDetail.DataSource = null;
        GvQuotationDetail.DataBind();

        GvProductDetail.DataSource = null;
        GvProductDetail.DataBind();

        //FillRequestGrid(1);
        FillGrid(1);

        hdnSalesQuotationId.Value = "0";
        txtAgentName.Enabled = true;
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        ddlOptionBin.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        txtValueQuoteDate.Text = "";
        txtValueBinDate.Text = "";
        txtValueDate.Text = "";

        txtValueDate.Visible = false;
        txtValue.Visible = true;

        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;

        txtValueQuote.Visible = true;
        txtValueQuoteDate.Visible = false;

        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtSONo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtSONo.Text;
        ddlFieldNameQuote.SelectedIndex = 1;
        ddlOptionQuote.SelectedIndex = 2;
        txtValueQuote.Text = "";
        lblTotalRecordsQuote.Text = "";

        ViewState["Status"] = null;
        rbtnFormView.Visible = false;
        rbtnAdvancesearchView.Visible = false;
        btnAddNewProduct.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        Session["DtSearchProduct"] = null;
        ViewState["DtTax"] = null;
        ViewState["dtTaxHeader"] = null;
        LoadStores();
        chkPartialShipment.Checked = true;
        ddlPaymentMode.SelectedIndex = 0;
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
        Session["PayementDt"] = null;
        ViewState["PayementDt"] = null;
        Session["ContactID"] = "0";
        btnPaymentReset_Click(null, null);
        gvPayment.DataSource = null;
        gvPayment.DataBind();
        chksendInproduction.Checked = false;

        chkSendInPO.Enabled = true;
        chksendInproduction.Enabled = true;
        hdnSalesPersonId.Value = "0";
        txtSalesPerson.Text = "";
        txtContactPerson.Text = "";
        hdnContactPersonId.Value = "0";
        try
        {
            ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        }
        catch
        {
        }
        StrCurrencyId = ddlCurrency.SelectedValue;
        txtAgentName.Text = "";
        GvProductDetail.Columns[GvProductDetail.Columns.Count - 2].Visible = false;
        GvQuotationDetail.Columns[GvQuotationDetail.Columns.Count - 1].Visible = false;
        ddlCurrency_OnSelectedIndexChanged(null, null);
        chkSendInProjectManagement.Checked = false;
        txtEstimateDeliveryDate.Text = DateTime.Now.ToString(ObjSysParam.SetDateFormat());
        btnSOrderSave.Enabled = true;
        BtnReset.Visible = true;
        pnlProduct1.Visible = true;
        rbtnFormView.Visible = true;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Visible = true;
        rbtnAdvancesearchView.Checked = false;
    }
    #endregion

    #region Add Request Section
    private void FillUnit()
    {
        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            DropDownList ddlUnit = (DropDownList)gvr.FindControl("ddlgvUnit");

            DataTable dsUnit = null;
            dsUnit = UM.GetUnitListforDDl(StrCompId);
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
    private void FillQuotationNo()
    {
        DataTable dsQuotationNo = null;
        dsQuotationNo = objSQuoteHeader.GetDataForSalesOrder(StrCompId, StrBrandId, StrLocationId);
        if (ddlUser.SelectedValue != "--Select User--")
        {
            dsQuotationNo = new DataView(dsQuotationNo, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {

                dsQuotationNo = new DataView(dsQuotationNo, "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();

            }
        }

        //code end Approval

        if (dsQuotationNo.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlQuotationNo, dsQuotationNo, "SQuotation_No", "SQuotation_Id");

        }
        else
        {
            ddlQuotationNo.DataSource = null;
            ddlQuotationNo.Items.Add("--Select--");
            ddlQuotationNo.SelectedValue = "--Select--";
            ddlQuotationNo.DataBind();
        }
    }
    protected void GvSalesQuotation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesQuotation.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterSOrderrequest"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesQuotation, dt, "", "");
        //try
        //{
        //    GvSalesQuotation.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Price");
        //}
        //catch
        //{
        //}

        //AllPageCode();
    }
    protected void GvSalesQuotation_Sorting(object sender, GridViewSortEventArgs e)
    {

        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesQuotation.Attributes["CurrentSortField"] != null &&
            GvSalesQuotation.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesQuotation.Attributes["CurrentSortField"])
            {
                if (GvSalesQuotation.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesQuotation.Attributes["CurrentSortField"] = sortField;
        GvSalesQuotation.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillRequestGrid(Int32.Parse(hdnGvSalesOrderCurrentPageIndex.Value));



        //DataTable dt = (DataTable)Session["dtFilterSOrderrequest"];
        //string sortdir = "DESC";
        //if (ViewState["PRSortDir"] != null)
        //{
        //    sortdir = ViewState["PRSortDir"].ToString();
        //    if (sortdir == "ASC")
        //    {
        //        e.SortDirection = SortDirection.Descending;
        //        ViewState["PRSortDir"] = "DESC";
        //    }
        //    else
        //    {
        //        e.SortDirection = SortDirection.Ascending;
        //        ViewState["PRSortDir"] = "ASC";
        //    }
        //}
        //else
        //{
        //    ViewState["PRSortDir"] = "DESC";
        //}

        //dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["PRSortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        //Session["dtFilterSOrderrequest"] = dt;
        ////this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //objPageCmn.FillData((object)GvSalesQuotation, dt, "", "");
        //AllPageCode();
    }
    private void FillRequestGrid()
    {
        DataTable dtPRequest = objSQuoteHeader.GetDataForSalesOrder(StrCompId, StrBrandId, StrLocationId);
        if (ddlUser.SelectedValue != "--Select User--")
        {
            dtPRequest = new DataView(dtPRequest, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {

                dtPRequest = new DataView(dtPRequest, "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();

            }
        }
        //code end Approval
        Session["dtSOrderrequest"] = dtPRequest;
        Session["dtFilterSOrderrequest"] = dtPRequest;
        if (dtPRequest != null && dtPRequest.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuotation, dtPRequest, "", "");
            //
        }
        else
        {
            GvSalesQuotation.DataSource = null;
            GvSalesQuotation.DataBind();
        }
        lblTotalRecordsQuote.Text = Resources.Attendance.Total_Records + " : " + dtPRequest.Rows.Count.ToString() + "";


    }
    private void FillRequestGrid(int currentPageIndex = 1, string strSearchFieldString = "")
    {
        string strWhereClause = string.Empty;
        strWhereClause = "IsActive='true'";
        if (ddlLocation.SelectedItem.ToString() != "All")
        {
            strWhereClause = strWhereClause + " and  Location_Id='" + ddlLocation.SelectedValue.ToString() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and  Location_Id in (" + ddlLocation.SelectedValue.ToString() + ")";
        }


        if (strSearchFieldString != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchFieldString;
        }

        if (ddlUser.SelectedValue != "--Select User--")
        {
            strWhereClause = strWhereClause + " and CreatedBy='" + ddlUser.SelectedValue.ToString() + "'";
        }
        using (DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationApproval"))
        {
            if (DtPara.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
                {
                    strWhereClause = strWhereClause + " and Field4='Approved'";
                }
            }
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objSQuoteHeader.getPendingSqByPageIndex((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesQuotation.Attributes["CurrentSortField"], GvSalesQuotation.Attributes["CurrentSortDirection"], strWhereClause))
        {
            objPageCmn.FillData((object)GvSalesQuotation, dt, "", "");
            if (dt.Rows.Count > 0)
            {
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecordsQuote.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            PageControlCommon.PopulatePager(rptGvSqPager, totalRows, currentPageIndex);
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(StrCompId, strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(StrCompId, retval);
                if (dtEmp.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        bool isTaxApplicable = Inventory_Common.IsSalesTaxEnabled(e.CommandName.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //if (Convert.ToBoolean(Session["isSalesTaxEnabled"].ToString()) != isTaxApplicable)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Tax is not enabled on this location, do you want to continue ?');", true);
        //    return;
        //}


        hdnLocationId.Value = e.CommandName.ToString();
        string[] strArgs = e.CommandArgument.ToString().Split(',');
        string strRequestId = strArgs[0].ToString();
        if (objCustomer.isCustomerBlock(strArgs[1].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString()) == true)
        {
            DisplayMessage("This customer has been blocked so you can not generate any order");
            txtCustomer.Text = string.Empty;
            return;
        }
        ShowOrderByQuotationNumber(strRequestId);
        Update_New.Update();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Quotation_Active()", true);
    }

    public void ShowOrderByQuotationNumber(string strRequestId)
    {
        decimal Amount = 0;
        DataTable dtDetail = new DataTable();
        DataTable dtPRequest = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, strRequestId);
        if (dtPRequest != null && dtPRequest.Rows.Count > 0)
        {
            if (dtPRequest.Rows[0]["Trans_Type"].ToString() != "")
                ddlTransType.SelectedValue = dtPRequest.Rows[0]["Trans_Type"].ToString();
            else
                ddlTransType.SelectedIndex = 0;
        }
        double GrossTotal = 0;
        if (dtPRequest.Rows.Count > 0)
        {
            trTransfer.Visible = true;
            ddlOrderType.SelectedValue = "Q";
            // ddlOrderType_SelectedIndexChanged(null, null);
            txtTransFrom.Text = "Sales Quotation";
            txtQuotationNo.Visible = true;
            ddlQuotationNo.Visible = false;
            txtQuotationNo.Text = dtPRequest.Rows[0]["SQuotation_No"].ToString();
            hdnSalesQuotationId.Value = dtPRequest.Rows[0]["SQuotation_Id"].ToString();
            ddlCurrency.SelectedValue = dtPRequest.Rows[0]["Currency_Id"].ToString();

            //added new field 
            try
            {
                DataTable dt_salesperson = HR_EmployeeDetail.GetEmpName_Code(dtPRequest.Rows[0]["Emp_Id"].ToString());
                txtSalesPerson.Text = dt_salesperson.Rows[0]["Emp_Name"].ToString() + "/" + dt_salesperson.Rows[0]["Emp_Code"].ToString();
                hdnSalesPersonId.Value = dtPRequest.Rows[0]["Emp_Id"].ToString();
            }
            catch
            {
                hdnSalesPersonId.Value = "0";
            }
            try
            {
                txtContactPerson.Text = dtPRequest.Rows[0]["ContactPersonName"].ToString() + "/" + dtPRequest.Rows[0]["Contact_Number"].ToString() + "/" + dtPRequest.Rows[0]["Contact_Email"].ToString() + "/" + dtPRequest.Rows[0]["Field1"].ToString();
                hdnContactPersonId.Value = dtPRequest.Rows[0]["Field1"].ToString();
            }
            catch
            {
                hdnContactPersonId.Value = "0";
            }

            StrCurrencyId = ddlCurrency.SelectedValue;
            //lblAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, Resources.Attendance.Gross_Total);
            //lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");
            //lblNetAmount.Text = SystemParameter.GetCurrencySmbol(StrCurrencyId, "Net Total");
            string strInquiryNo = dtPRequest.Rows[0]["SInquiry_No"].ToString();


            //for get agent name according condition 

            string strAgentId = dtPRequest.Rows[0]["Agent_Id"].ToString();
            if (strAgentId != "" && strAgentId != "0")
            {
                txtAgentName.Enabled = false;
                txtAgentName.Text = dtPRequest.Rows[0]["Agent_Name"].ToString() + "/" + strAgentId;
                GvQuotationDetail.Columns[GvQuotationDetail.Columns.Count - 1].Visible = true;
            }
            if (strInquiryNo != "0" && strInquiryNo != "")
            {
                DataTable dtSInquiryData = objSInquiryHeader.GetSIHeaderAllBySInquiryId(StrCompId, StrBrandId, StrLocationId, strInquiryNo);
                if (dtSInquiryData.Rows.Count > 0)
                {
                    string strCustomerId = dtSInquiryData.Rows[0]["Customer_Id"].ToString();
                    if (strCustomerId != "0" && strCustomerId != "")
                    {

                        DataTable dtCustomerName = objda.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
                        if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
                        {
                            string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                            string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                            txtCustomer.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                            //txtCustomer.Text = dtSInquiryData.Rows[0]["Name"].ToString() + "/" + strCustomerId;
                            Session["ContactID"] = strCustomerId;
                        }




                        DataTable dtAddress;
                        if (txtInvoiceTo.Text == "")
                        {
                            dtAddress = objContact.GetAddressByRefType_Id("Customer", strCustomerId);
                            if (dtAddress != null && dtAddress.Rows.Count > 0)
                            {
                                txtInvoiceTo.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                            }
                            else
                            {
                                txtInvoiceTo.Text = "";
                            }
                        }
                        if (txtShipCustomerName.Text == "")
                        {
                            txtShipCustomerName.Text = txtCustomer.Text;
                        }
                        if (txtShipCustomerName.Text != "")
                        {
                            DataTable DtCustomer = new DataTable();
                            string[] ShipName = txtShipCustomerName.Text.Split('/');
                            DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());

                            if (DtCustomer.Rows.Count > 0)
                            {
                                dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[3].ToString().Trim());
                                if (dtAddress != null && dtAddress.Rows.Count > 0)
                                {
                                    txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                                }
                                else
                                {
                                    txtShipingAddress.Text = "";
                                }
                            }
                        }
                    }
                }
            }
            //Add Detail Grid
            dtDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(StrCompId, StrBrandId, StrLocationId, strRequestId, Session["FinanceYearId"].ToString());

            if (dtDetail.Rows.Count > 0)
            {
                HDN_Sales_Quotation_ID.Value = strRequestId;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvQuotationDetail, dtDetail, "", "");
                Get_Tax_From_DB();
                //GvQuotationDetail.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Price");
                //GvQuotationDetail.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "");

                //GvQuotationDetail.HeaderRow.Cells[11].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Value");
                //GvQuotationDetail.HeaderRow.Cells[14].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "Value");
                //GvQuotationDetail.HeaderRow.Cells[16].Text = SystemParameter.GetCurrencySmbol(StrCurrencyId.ToString(), "");
                GvQuotationDetail.Columns[12].Visible = false;
                GvQuotationDetail.Columns[15].Visible = false;

                FillUnit();
                foreach (GridViewRow gvr in GvQuotationDetail.Rows)
                {//aaaaaa
                    HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                    CheckBox chkgvQuantitySelect = (CheckBox)gvr.FindControl("chkGvQuotationDetailSelect");
                    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");

                    DropDownList ddlgvUnit = (DropDownList)gvr.FindControl("ddlgvUnit");
                    TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
                    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");

                    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");

                    TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
                    Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");
                    //if (hdngvProductId.Value != "0" && hdngvProductId.Value != "")
                    {
                        DataTable dtgridetail = new DataView(dtDetail, "Product_Id=" + hdngvProductId.Value + " and Serial_No='" + lblgvSerialNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtgridetail.Rows.Count > 0)
                        {

                            txtgvUnitPrice.Text = dtgridetail.Rows[0]["UnitPrice"].ToString();

                            if (lblgvQuantity.Text == "")
                            {
                                lblgvQuantity.Text = "0";
                            }
                            if (txtgvUnitPrice.Text == "")
                            {
                                txtgvUnitPrice.Text = "0";
                            }


                            if (dtgridetail.Rows[0]["Field1"].ToString() != "")
                                ddlgvUnit.SelectedValue = dtgridetail.Rows[0]["Field1"].ToString();

                            txtgvTaxP.Text = Get_Tax_Percentage(dtgridetail.Rows[0]["Product_Id"].ToString(), lblgvSerialNo.Text).ToString();
                            txtgvDiscountP.Text = dtgridetail.Rows[0]["DiscountPercent"].ToString();

                            string[] strcalc = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());

                            txtgvQuantityPrice.Text = strcalc[0].ToString();

                            GrossTotal += Convert.ToDouble(txtgvQuantityPrice.Text);

                            txtgvDiscountV.Text = strcalc[2].ToString();
                            txtgvTaxV.Text = strcalc[4].ToString();
                            txtgvTotal.Text = strcalc[5].ToString();
                            chkgvQuantitySelect.Checked = true;
                        }
                    }
                }
                Grid_Calculation("GvQuotationDetail", "Get Product Tax");
            }
            txtAmount.Text = GrossTotal.ToString();

            txtDiscountP.Text = dtPRequest.Rows[0]["DiscountPercent"].ToString();

            txtTaxP.Text = dtPRequest.Rows[0]["TaxPercent"].ToString();

            txtDiscountV.Text = ((GrossTotal * Convert.ToDouble(dtPRequest.Rows[0]["DiscountPercent"].ToString())) / 100).ToString();
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
            txtTaxV.Text = ((Convert.ToDouble(txtPriceAfterDiscount.Text) * Convert.ToDouble(dtPRequest.Rows[0]["TaxPercent"].ToString())) / 100).ToString();

            txtTotalAmount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text) + Convert.ToDouble(txtTaxV.Text)).ToString();

            if (txtShippingCharge.Text == "")
            {
                txtNetAmount.Text = txtTotalAmount.Text;
            }
            else
            {
                txtNetAmount.Text = (Convert.ToDouble(txtTotalAmount.Text) + Convert.ToDouble(txtShippingCharge.Text)).ToString();
            }
        }
        setDecimal();
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
            if (gvProduct.ID == "GvQuotationDetailView")
            {

                cell.ColumnSpan = 1;

            }
            else
            {
                cell.ColumnSpan = 2;
            }
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
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

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
    #endregion
    protected void chkGvQuotationDetailSelect_CheckedChanged(object sender, EventArgs e)
    {

        GetGridTotal_InDirect();
        //AllPageCode();


    }
    #region Calculations

    public void GridRowValueChange()
    {

        foreach (GridViewRow GridRow in GvProductDetail.Rows)
        {
            ((Label)GridRow.FindControl("lblgvTaxP")).Text = txtTaxP.Text;
            ((Label)GridRow.FindControl("lblgvTaxPUnit")).Text = txtTaxP.Text;

            if (Decimal.Parse(txtTaxP.Text) != 0)
            {
                ((Label)GridRow.FindControl("lblgvTaxV")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) * Decimal.Parse(txtTaxP.Text)) / (100)).ToString();
                ((Label)GridRow.FindControl("lblgvTaxVUnit")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvUnitPrice")).Text) * Decimal.Parse(txtTaxP.Text)) / (100)).ToString();


            }
            else
            {
                ((Label)GridRow.FindControl("lblgvTaxV")).Text = "0";
                ((Label)GridRow.FindControl("lblgvTaxVUnit")).Text = "0";
            }

            ((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) + Decimal.Parse(((Label)GridRow.FindControl("lblgvTaxV")).Text))).ToString();

            ((Label)GridRow.FindControl("lblgvDiscountV")).Text = ((((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) + Decimal.Parse(((Label)GridRow.FindControl("lblgvTaxV")).Text))) * Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountP")).Text)) / (100)).ToString();

            ((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text = (Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text) - Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountV")).Text)).ToString();
            ((Label)GridRow.FindControl("lblgvTotal")).Text = Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text).ToString();


        }
        GetGridTotal();
    }
    public void GridRowValueChangeQuotation()
    {

        foreach (GridViewRow GridRow in GvQuotationDetail.Rows)
        {
            ((Label)GridRow.FindControl("lblgvTaxP")).Text = txtTaxP.Text;
            if (Convert.ToDouble(txtTaxP.Text) != 0)
            {
                ((Label)GridRow.FindControl("lblgvTaxV")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) * Decimal.Parse(txtTaxP.Text)) / (100)).ToString();
            }
            else
            {
                ((Label)GridRow.FindControl("lblgvTaxV")).Text = "0";
            }

            ((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) + Decimal.Parse(((Label)GridRow.FindControl("lblgvTaxV")).Text))).ToString();

            ((Label)GridRow.FindControl("lblgvDiscountV")).Text = ((((Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantityPrice")).Text) + Decimal.Parse(((Label)GridRow.FindControl("lblgvTaxV")).Text))) * Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountP")).Text)) / (100)).ToString();

            ((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text = (Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text) - Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountV")).Text)).ToString();
            ((Label)GridRow.FindControl("lblgvTotal")).Text = Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text).ToString();


        }
        GetGridTotal();
    }
    protected void txtTaxP_TextChanged(object sender, EventArgs e)
    {

        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }



        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, StrCurrencyId, true, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
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
        //AllPageCode();
    }
    protected void txtTaxV_TextChanged(object sender, EventArgs e)
    {

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

        //AllPageCode();
    }
    public void GridRowDiscountChange()
    {

        foreach (GridViewRow GridRow in GvProductDetail.Rows)
        {
            ((Label)GridRow.FindControl("lblgvDiscountP")).Text = txtDiscountP.Text;
            ((Label)GridRow.FindControl("lblgvDiscountPUnit")).Text = txtDiscountP.Text;
            ((Label)GridRow.FindControl("lblgvDiscountV")).Text = ((Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text) * Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountP")).Text)) / (100)).ToString();
            ((Label)GridRow.FindControl("lblgvDiscountVUnit")).Text = (Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountV")).Text) / (Decimal.Parse(((Label)GridRow.FindControl("lblgvQuantity")).Text))).ToString();

            ((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text = (Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterTax")).Text) - Decimal.Parse(((Label)GridRow.FindControl("lblgvDiscountV")).Text)).ToString();
            ((Label)GridRow.FindControl("lblgvTotal")).Text = Decimal.Parse(((Label)GridRow.FindControl("lblgvPriceAfterDiscount")).Text).ToString();


        }
        GetGridTotal();
    }
    protected void txtDiscountP_TextChanged(object sender, EventArgs e)
    {

        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }

        if (txtDiscountP.Text == "")
        {
            txtDiscountP.Text = "0";
        }

        txtDiscountP.Text = GetAmountDecimal(txtDiscountP.Text);

        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", txtDiscountP.Text, "", false, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();


        txtDiscountV.Text = str[2].ToString();
        try
        {
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }

        txtTotalAmount.Text = str[5].ToString();

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

            TaxDiscountFromHeader(true);
        }

        if (GvQuotationDetail.Rows.Count > 0)
        {
            TaxDiscountFromHeader_InDirect(true);
        }
        //AllPageCode();

    }
    protected void txtDiscountV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }


        if (txtDiscountV.Text == "")
        {
            txtDiscountV.Text = "0";
        }

        txtDiscountV.Text = GetAmountDecimal(txtDiscountV.Text);

        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtDiscountP.Text = str[1].ToString();

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

        try
        {
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }

        if (GvProductDetail.Rows.Count > 0)
        {
            TaxDiscountFromHeader(true);
        }
        if (GvQuotationDetail.Rows.Count > 0)
        {
            TaxDiscountFromHeader_InDirect(true);
        }
        //AllPageCode();
    }
    #endregion

    #region Grid Calculations
    #region Indirect

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
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetInvoiceAddressDetail(string InvAddrees)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByAddressName(InvAddrees.Split('/')[0].ToString());
        string Address = "0";
        if (dtAM.Rows.Count > 0)
        {
            Address = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());

            return Address;

        }
        else
        {
            return Address;
        }

    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetShipToAddress(string ShipToAddress)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByAddressName(ShipToAddress.Split('/')[0].ToString());
        string Address = "0";
        if (dtAM.Rows.Count > 0)
        {
            Address = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());

            return Address;

        }
        else
        {
            return Address;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetShippingAddress(string ShippingAddress)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByAddressName(ShippingAddress.Split('/')[0].ToString());
        string Address = "0";
        if (dtAM.Rows.Count > 0)
        {
            Address = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());

            return Address;

        }
        else
        {
            return Address;
        }
    }
    protected void GetGridTotal_InDirect()
    {

        Double fGrossTotal = 0;
        Double fGrossTax = 0;
        Double fGrossDis = 0;

        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {

            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
            TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            CheckBox chkGvQuotationDetailSelect = (CheckBox)gvr.FindControl("chkGvQuotationDetailSelect");


            if (chkGvQuotationDetailSelect.Checked)
            {
                if (txtgvQuantityPrice.Text == "")
                {
                    txtgvQuantityPrice.Text = "0";
                }


                if (txtgvTotal.Text != "")
                {
                    fGrossTotal = fGrossTotal + Convert.ToDouble(txtgvQuantityPrice.Text);
                }
                if (txtgvTaxV.Text == "")
                {
                    txtgvTaxV.Text = "0";
                }
                fGrossTax = fGrossTax + (Convert.ToDouble(txtgvTaxV.Text) * Convert.ToDouble(lblgvQuantity.Text));

                if (txtgvDiscountV.Text == "")
                {
                    txtgvDiscountV.Text = "0";
                }
                fGrossDis = fGrossDis + (Convert.ToDouble(txtgvDiscountV.Text) * Convert.ToDouble(lblgvQuantity.Text));
            }

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
            txtPriceAfterDiscount.Text = GetAmountDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString());
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }

        CalculationchangeIntaxGridview();
    }
    protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }


        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


        txtgvQuantityPrice.Text = strcal[0].ToString();
        txtgvDiscountV.Text = strcal[2].ToString();
        txtgvTaxV.Text = strcal[4].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();
    }
    protected void lblgvQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label txtgvRemainQuantity = (Label)gvr.FindControl("txtgvRemainQuantity");
        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }


        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtgvRemainQuantity.Text = lblgvQuantity.Text;

        txtgvQuantityPrice.Text = strcal[0].ToString();
        txtgvDiscountV.Text = strcal[2].ToString();
        txtgvTaxV.Text = strcal[4].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();
    }
    protected void txtgvTaxP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }



        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", "", txtgvDiscountV.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());
        txtgvTaxV.Text = strcal[4].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();
    }

    protected void txtgvTaxV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }


        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, "", txtgvTaxV.Text, "", txtgvDiscountV.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());
        txtgvTaxP.Text = strcal[3].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();
    }
    protected void txtgvDiscountP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


        txtgvDiscountV.Text = strcal[2].ToString();
        txtgvTaxV.Text = strcal[4].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();

    }
    protected void txtgvDiscountV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");


        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }
        string[] strcal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", "", txtgvDiscountV.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());


        txtgvDiscountP.Text = strcal[1].ToString();
        txtgvTaxV.Text = strcal[4].ToString();
        txtgvTotal.Text = strcal[5].ToString();
        GetGridTotal_InDirect();
        //AllPageCode();
    }
    #endregion

    #region Direct
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
        //txtTaxP.Text = strHeaderCalculation[3].ToString();

        txtTaxP.Text = Math.Round(Convert.ToDouble(strHeaderCalculation[3])).ToString();

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
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }
        setDecimal();
        CalculationchangeIntaxGridview();
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
    protected void lblgvdQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        Label lblgvSerialNo = (Label)gvrow.FindControl("lblgvSerialNo");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        lblgvQuantity.Text = ((TextBox)sender).Text;
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }
        if (lblgvQuantity.Text == "0")
        {
            lblgvUnitPrice.Text = "0";
            lblgvQuantityPrice.Text = "0";
            lblgvDiscountV.Text = "0";
        }


        HiddenField hdnProductId = (HiddenField)gvrow.FindControl("hdngvProductId");

        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");


        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");

        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");

        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (lblgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Percentage(hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", lblgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());

        lblgvRemainQuantity.Text = lblgvQuantity.Text;
        lblgvQuantityPrice.Text = strcal[0].ToString();
        lblgvDiscountV.Text = strcal[2].ToString();
        lblgvTaxV.Text = strcal[4].ToString();
        lblgvTotal.Text = strcal[5].ToString();
        Grid_Calculation("GvProductDetail", "Add Product");

        GetGridTotal();
        //AllPageCode();
    }
    protected void txtgvdUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)gvrow.FindControl("hdngvProductId");
        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");
        Label lblgvSerialNo = (Label)gvrow.FindControl("lblgvSerialNo");

        lblgvUnitPrice.Text = ((TextBox)sender).Text;
        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }
        if (lblgvUnitPrice.Text == "0")
        {
            lblgvDiscountV.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (lblgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Percentage(hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", lblgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());

        lblgvRemainQuantity.Text = lblgvQuantity.Text;
        lblgvQuantityPrice.Text = strcal[0].ToString();
        lblgvDiscountV.Text = strcal[2].ToString();

        GridView GridChild = (GridView)gvrow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(lblgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);

        lblgvTaxV.Text = Convert_Into_DF(strcal[4].ToString());
        lblgvTotal.Text = strcal[5].ToString();
        Grid_Calculation("GvProductDetail", "Add Product");
        GetGridTotal();
        //AllPageCode();
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


                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = GetAmountDecimal((Convert.ToDouble(Amount) * (Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100)).ToString());


            }


        }

    }
    protected void txtgvdDiscountP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)gvrow.FindControl("hdngvProductId");
        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");
        Label lblgvSerialNo = (Label)gvrow.FindControl("lblgvSerialNo");
        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (lblgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Percentage(hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", lblgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


        lblgvDiscountV.Text = strcal[2].ToString();

        GridView GridChild = (GridView)gvrow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(lblgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        lblgvTaxV.Text = strcal[4].ToString();
        lblgvTotal.Text = strcal[5].ToString();
        GetGridTotal();
        //AllPageCode();
    }
    protected void txtgvdDiscountV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)gvrow.FindControl("hdngvProductId");
        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");
        Label lblgvSerialNo = (Label)gvrow.FindControl("lblgvSerialNo");
        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (lblgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Percentage(hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", "", lblgvDiscountV.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());


        lblgvDiscountP.Text = strcal[1].ToString();

        GridView GridChild = (GridView)gvrow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(lblgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        lblgvTaxV.Text = strcal[4].ToString();
        lblgvTotal.Text = strcal[5].ToString();
        GetGridTotal();
        //AllPageCode();
    }
    protected void txtgvdTaxP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");

        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, lblgvTaxP.Text, "", "", lblgvDiscountV.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());
        lblgvTaxV.Text = strcal[4].ToString();
        lblgvTotal.Text = strcal[5].ToString();
        GetGridTotal();
        //AllPageCode();
    }

    protected void txtgvdTaxV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
        Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
        Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
        TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
        TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
        TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
        TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
        Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");

        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }

        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, "", lblgvTaxV.Text, "", lblgvDiscountV.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());
        lblgvTaxP.Text = strcal[3].ToString();
        lblgvTotal.Text = strcal[5].ToString();
        GetGridTotal();
        //AllPageCode();
    }
    #endregion






    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text != "")
        {
            Decimal flTemp = 0;
            if (Decimal.TryParse(txtAmount.Text, out flTemp))
            {
                if (txtTaxP.Text != "")
                {
                    txtTaxP_TextChanged(sender, e);
                }
                else
                {
                    txtPriceAfterTax.Text = "0";
                }
                if (txtDiscountP.Text != "")
                {
                    txtDiscountP_TextChanged(sender, e);
                }
                else
                {
                    txtTotalAmount.Text = (Decimal.Parse(txtAmount.Text) + Decimal.Parse(txtPriceAfterTax.Text)).ToString();
                }

                if (txtShippingCharge.Text != "")
                {
                    txtShippingCharge_TextChanged(sender, e);
                }

                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxP);
            }
        }
        setDecimal();
    }
    #endregion
    private string GetCustomerId(TextBox txt)
    {
        string retval = string.Empty;
        if (txt.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txt.Text.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                retval = txt.Text.Split('/')[3].ToString();

                DataTable dtCompany = objContact.GetContactTrueById(retval);
                if (dtCompany.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "0";
        }
        return retval;
    }
    private string GetCustomerId(TextBox txt, ref SqlTransaction trns)
    {
        string retval = string.Empty;
        if (txt.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txt.Text.Trim().Split('/')[0].ToString().Trim(), ref trns);
            if (dtSupp.Rows.Count > 0)
            {
                retval = txt.Text.Split('/')[3].ToString();

                DataTable dtCompany = objContact.GetContactTrueById(retval, ref trns);
                if (dtCompany.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "0";
        }
        return retval;
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
        btnAddNewProduct.Visible = false;
        GvProductDetail.DataSource = null;
        GvProductDetail.DataBind();
        txtNetAmount.Text = txtShippingCharge.Text.ToString();

        txtPriceAfterDiscount.Text = "";
        txtInvoiceTo.Text = "";
        txtShipCustomerName.Text = "";
        txtShipingAddress.Text = "";
        btnAddNewProduct.Visible = false;
        rbtnFormView.Visible = false;
        rbtnAdvancesearchView.Visible = false;
        rbtnFormView.Checked = false;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        btnClosePanel_Click(null, null);
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
            btnAddNewProduct.Visible = true;
            txtQuotationNo.Text = "";
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnAdvancesearchView.Checked = false;
            rbtnFormView_OnCheckedChanged(null, null);
        }
        else if (ddlOrderType.SelectedValue == "Q")
        {
            trTransfer.Visible = true;
            txtTransFrom.Text = "Sales Quotation";
            ddlQuotationNo.Visible = true;
            txtQuotationNo.Visible = false;
            FillQuotationNo();

        }
    }
    protected void ddlQuotationNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQuotationNo.SelectedValue != "--Select--")
        {
            ShowOrderByQuotationNumber(ddlQuotationNo.SelectedValue);
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);
        }
        else
        {
            GvQuotationDetail.DataSource = null;
            GvQuotationDetail.DataBind();
            txtTaxP.Text = "0";
            txtTaxV.Text = "0";
            txtDiscountP.Text = "0";
            txtDiscountV.Text = "0";
            txtAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtTotalAmount.Text = "0";
            txtPriceAfterDiscount.Text = "0";
            txtPriceAfterTax.Text = "0";
            txtShippingCharge.Text = "0";
            txtCustomer.Text = "";
            txtInvoiceTo.Text = "";
            txtShipCustomerName.Text = "";
            txtShipingAddress.Text = "";
        }
    }
    protected void txtShippingCharge_TextChanged(object sender, EventArgs e)
    {
        if (txtShippingCharge.Text != "")
        {
            Decimal flTemp = 0;
            if (Decimal.TryParse(txtShippingCharge.Text, out flTemp))
            {
                if (txtTotalAmount.Text != "")
                {
                    txtNetAmount.Text = (Decimal.Parse(txtTotalAmount.Text) + Decimal.Parse(txtShippingCharge.Text)).ToString();
                }
                else
                {
                    txtNetAmount.Text = txtShippingCharge.Text;
                }
            }
            else
            {
                txtShippingCharge.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShippingCharge);
            }
        }
        else
        {

        }
        //AllPageCode();
    }
    #region Add Product Concept
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

    protected string GetShortProductName(string strProductId)
    {
        string strProductName = string.Empty;
        strProductName = GetProductName(strProductId);
        if (strProductName.Length > 16)
        {
            strProductName = strProductName.Substring(0, 15) + "...";
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
    protected void GvProductDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DataTable Dt = new DataTable();
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableHeaderCell cell = new TableHeaderCell();

            cell.Text = Resources.Attendance.Tax;
            cell.ColumnSpan = 1;

            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }



            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Delete;
            row.Controls.Add(cell);

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


            if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;

            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
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

    private void FillProductUnit()
    {
        DataTable dsUnit = null;
        dsUnit = UM.GetUnitMaster(StrCompId);
        if (dsUnit.Rows.Count > 0)
        {
            try
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                objPageCmn.FillData((object)ddlUnit, dsUnit, "Unit_Name", "Unit_Id");

            }
            catch
            {
            }
        }
        else
        {
            ddlUnit.Items.Add("--Select--");
            ddlUnit.SelectedValue = "--Select--";
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            dt = null;
            return txt;
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
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }



    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
        ResetProduct();
        //AllPageCode();
        GetChildGridRecordInViewState();
        Session["DtSearchProduct"] = null;

    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataTable();
            dt = objProductM.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {

                string pid = dt.Rows[0]["ProductId"].ToString();

                //updated by Akshay
                DataTable Dt = FillProductDataTabel();
                DataTable dtProduct = new DataTable();
                try
                {
                    dtProduct = new DataView(Dt, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtProduct.Rows.Count > 0)
                {
                    //DisplayMessage("Product Is already exists!");
                    //txtProductName.Text = "";
                    //txtProductcode.Text = "";
                    //txtProductName.Focus();
                    //return;
                }




                //this code is created by jitendra upadhyay on 15-12-2014
                //this code to select the sales price according the Inventory Parameter
                string[] CustomerName = txtCustomer.Text.Split('/');


                try
                {




                    txtPUnitPrice.Text = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", txtCustomer.Text.Split('/')[3].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                    txtPUnitPrice.Text = GetAmountDecimal(txtPUnitPrice.Text);

                }
                catch
                {
                    txtPUnitPrice.Text = "0";
                }





                txtPUnitPrice_TextChanged(sender, e);

            }
            if (dt != null && dt.Rows.Count > 0)
            {
                string strUnitId = dt.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dt.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                FillProductUnit();
                txtPDescription.Text = "";
                hdnNewProductId.Value = "0";
            }
            txtPFreeQuantity.Text = "0";

        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        //AllPageCode();
    }
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(StrCompId, StrBrandId, StrLocationId, pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtProductcode.Text != "")
        {
            DataTable dt = new DataTable();
            dt = objProductM.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductcode.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                string pid = dt.Rows[0]["ProductId"].ToString();

                //updated by Akshay
                DataTable Dt = FillProductDataTabel();
                DataTable dtProduct = new DataTable();
                try
                {
                    dtProduct = new DataView(Dt, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtProduct.Rows.Count > 0)
                {
                    //DisplayMessage("Product Is already exists!");
                    //txtProductName.Text = "";
                    //txtProductcode.Text = "";
                    //txtProductcode.Focus();
                    //return;

                }





                //this code is created by jitendra upadhyay on 15-12-2014
                //this code to select the sales price according the Inventory Parameter
                string[] CustomerName = txtCustomer.Text.Split('/');

                try
                {

                    txtPUnitPrice.Text = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", txtCustomer.Text.Split('/')[3].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                    txtPUnitPrice.Text = GetAmountDecimal(txtPUnitPrice.Text);

                }
                catch
                {
                    txtPUnitPrice.Text = "0";
                }



                if (txtPUnitPrice.Text == "")
                {
                    txtPUnitPrice.Text = "0";
                }
                if (txtPQuantity.Text == "")
                {
                    txtPQuantity.Text = "0";
                }


                string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", txtPDiscountPUnit.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());
                txtPQuantityPrice.Text = strCalculation[0].ToString();
                txtPDiscountVUnit.Text = strCalculation[2].ToString();
                txtPTaxVUnit.Text = strCalculation[4].ToString();
                txtPTotalAmount.Text = strCalculation[5].ToString();


                // txtPUnitPrice_TextChanged(sender, e);


            }
            if (dt != null && dt.Rows.Count > 0)
            {
                string strUnitId = dt.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dt.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                FillProductUnit();
                txtPDescription.Text = "";
                hdnNewProductId.Value = "0";
            }
            txtPFreeQuantity.Text = "0";
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
        //AllPageCode();
    }
    protected void txtShipCustomerName_TextChanged(object sender, EventArgs e)
    {
        DataTable DtCustomer = new DataTable();
        DataTable dtAddress = new DataTable();
        if (txtShipCustomerName.Text != "")
        {
            string[] ShipName = txtShipCustomerName.Text.Split('/');
            DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {
                dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[1].ToString().Trim());
                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                }
                else
                {
                    txtShipingAddress.Text = "";
                }
            }
            else
            {
                DisplayMessage("Select Ship to in suggestion Only");
                txtShipCustomerName.Text = "";
                txtShipCustomerName.Focus();
                return;
            }

        }

    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        string objSenderID = "";

        TextBox b = (TextBox)sender;
        objSenderID = b.ID;



        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;

        if (txtCustomer.Text != "")
        {
            txtContactPerson.Text = "";
            Session["ContactID"] = "0";
            int parsedValue;
            try { txtCustomer.Text.Split('/')[1].ToString(); } catch { txtCustomer.Text = ""; Session["ContactID"] = "0"; DisplayMessage("Select from suggestion Only"); return; }
            if (!int.TryParse(txtCustomer.Text.Split('/')[1].ToString(), out parsedValue))
            {
                txtCustomer.Text = ""; DisplayMessage("Select from suggestion Only");
                Session["ContactID"] = "0";
                return;
            }

            if (editid.Value == "" && objCustomer.isCustomerBlock(txtCustomer.Text.Split('/')[1].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString()) == true)
            {
                DisplayMessage("This customer has been blocked so you can not generate any order");
                txtCustomer.Text = string.Empty;
                Session["ContactID"] = "0";
                return;
            }


            string[] CustomerName = txtCustomer.Text.Split('/');

            DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {
                Session["ContactID"] = CustomerName[1].ToString().Trim();


                DataTable dtAddress;
                if (txtInvoiceTo.Text == "")
                {
                    dtAddress = objContact.GetAddressByRefType_Id("Contact", CustomerName[1].ToString().Trim());
                    if (dtAddress != null && dtAddress.Rows.Count > 0)
                    {
                        txtInvoiceTo.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                    }
                    else
                    {
                        txtInvoiceTo.Text = "";
                    }
                }
                if (txtShipCustomerName.Text == "")
                {
                    txtShipCustomerName.Text = txtCustomer.Text;
                }
                if (txtShipCustomerName.Text != "")
                {
                    string[] ShipName = txtShipCustomerName.Text.Split('/');
                    DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());

                    if (DtCustomer.Rows.Count > 0)
                    {
                        dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[3].ToString().Trim());
                        if (dtAddress != null && dtAddress.Rows.Count > 0)
                        {
                            txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                        }
                        else
                        {
                            txtShipingAddress.Text = "";
                        }
                    }
                    else
                    {
                        DisplayMessage("Select Ship to in suggestion Only");
                        //GetCreditInfo();
                        txtShipCustomerName.Text = "";
                        txtShipCustomerName.Focus();
                        return;
                    }

                }

                if (objSenderID != "txtShipCustomerName")
                {
                    if (GvProductDetail != null && GvProductDetail.Rows.Count > 0)
                    {
                        if (CustomerName.Length > 1)
                        {
                            foreach (GridViewRow Gvrow in GvProductDetail.Rows)
                            {
                                HiddenField hdnProductId = (HiddenField)Gvrow.FindControl("hdngvProductId");
                                TextBox lblgvUnitPrice = (TextBox)Gvrow.FindControl("lblgvUnitPrice");
                                //this code is created by jitendra upadhyay on 15-12-2014
                                //this code for select the sales price according the inventory parameter

                                try
                                {
                                    lblgvUnitPrice.Text = GetAmountDecimal(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", CustomerName[1].ToString(), hdnProductId.Value).Rows[0]["Sales_Price"].ToString());
                                }
                                catch
                                {
                                    lblgvUnitPrice.Text = "0";

                                }
                            }
                        }
                        else
                        {
                            DisplayMessage("Customer id is not found with customer name");
                            Session["ContactID"] = "0";
                            // GetCreditInfo();
                            txtCustomer.Text = "";
                            txtCustomer.Focus();
                            return;
                        }

                        foreach (GridViewRow gvr in GvProductDetail.Rows)
                        {
                            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                            TextBox lblgvUnitPrice = (TextBox)gvr.FindControl("lblgvUnitPrice");
                            Label lblgvQuantityPrice = (Label)gvr.FindControl("lblgvQuantityPrice");
                            TextBox lblgvTaxV = (TextBox)gvr.FindControl("lblgvTaxV");
                            Label lblgvPriceAfterTax = (Label)gvr.FindControl("lblgvPriceAfterTax");
                            TextBox lblgvDiscountV = (TextBox)gvr.FindControl("lblgvDiscountV");
                            Label lblgvPriceAfterDiscount = (Label)gvr.FindControl("lblgvPriceAfterDiscount");
                            Label lblgvTotal = (Label)gvr.FindControl("lblgvTotal");

                            if (lblgvQuantity.Text != "")
                            {
                                if (lblgvUnitPrice.Text != "")
                                {
                                    lblgvQuantityPrice.Text = (Decimal.Parse(lblgvQuantity.Text) * Decimal.Parse(lblgvUnitPrice.Text)).ToString();
                                    if (lblgvTaxV.Text != "")
                                    {
                                        lblgvPriceAfterTax.Text = (Decimal.Parse(lblgvQuantityPrice.Text) + Decimal.Parse(lblgvTaxV.Text)).ToString();
                                        lblgvTotal.Text = (Decimal.Parse(lblgvQuantityPrice.Text) + Decimal.Parse(lblgvTaxV.Text)).ToString();
                                    }
                                    else
                                    {
                                        lblgvPriceAfterTax.Text = lblgvQuantityPrice.Text;
                                        lblgvTotal.Text = lblgvPriceAfterTax.Text;
                                    }
                                    if (lblgvDiscountV.Text != "")
                                    {
                                        lblgvPriceAfterDiscount.Text = (Decimal.Parse(lblgvPriceAfterTax.Text) - Decimal.Parse(lblgvDiscountV.Text)).ToString();
                                        lblgvTotal.Text = (Decimal.Parse(lblgvPriceAfterTax.Text) - Decimal.Parse(lblgvDiscountV.Text)).ToString();
                                    }
                                    else
                                    {
                                        lblgvPriceAfterDiscount.Text = lblgvPriceAfterTax.Text;
                                        lblgvTotal.Text = lblgvPriceAfterTax.Text;
                                    }
                                }
                            }
                        }
                        GetGridTotal();


                    }
                }
            }
            else
            {
                DisplayMessage("Enter Customer Name in suggestion Only");
                Session["ContactID"] = "0";
                txtCustomer.Text = "";
                // GetCreditInfo();
                txtCustomer.Focus();
                return;
            }
        }
        else
        {
            Session["ContactID"] = "0";
        }
        txtInvoiceTo_TextChanged(null, null);
        txtShipingAddress_TextChanged(null, null);

        //GetCreditInfo();
    }

    public void GetCreditInfo()
    {
        bool IsCredit = false;

        //here we will show credit terms and condition .
        try
        {
            IsCredit = Convert.ToBoolean(objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[3].ToString()).Rows[0]["Field41"].ToString());
        }
        catch
        {

        }


        if (IsCredit)
        {


            DataTable dtCreditParameter = objCustomerCreditParam.GetRecord_By_CustomerId(txtCustomer.Text.Split('/')[3].ToString());


            dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtCreditParameter.Rows.Count > 0)
            {
                lblCreditLimitValue.Text = GetAmountDecimal(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
                try
                {
                    lblCurrencyCreditLimit.Text = objCurrency.GetCurrencyMasterById(dtCreditParameter.Rows[0]["Credit_Limit_Currency"].ToString().Trim()).Rows[0]["Currency_Name"].ToString();
                }
                catch
                {
                }
                lblCreditDaysValue.Text = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
                if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
                {
                    lblCreditParameterValue.Text = "Advance Cheque Basis";
                }
                else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
                {
                    lblCreditParameterValue.Text = "Invoice to Invoice Payment";
                }
                else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
                {
                    lblCreditParameterValue.Text = "50% advance and 50% on delivery";
                }
                else
                {
                    lblCreditParameterValue.Text = "None";
                }

            }
            else
            {
                lblCreditLimitValue.Text = "";
                lblCreditDaysValue.Text = "";
                lblCreditParameterValue.Text = "";
                lblCurrencyCreditLimit.Text = "";
            }
        }
        else
        {

        }
    }




    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }


        if (txtProductName.Text != "")
        {
            string strA = "0";


            if (hdnNewProductId.Value != "0")
            {
                hdnNewProductId.Value = objProductM.GetProductIdbyProductCode(txtProductcode.Text.Trim(), Session["BrandId"].ToString());

            }

            if (ddlUnit.SelectedValue != "--Select--")
            {
                hdnUnitId.Value = ddlUnit.SelectedValue;
            }
            else
            {
                DisplayMessage("Select Unit Name");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
                return;
            }

            if (txtPQuantity.Text != "")
            {

            }
            else
            {
                DisplayMessage("Enter Required Quantity");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPQuantity);
                return;
            }


            if (txtPQuantity.Text != "")
            {
                double flTemp = 0;
                if (double.TryParse(txtPQuantity.Text, out flTemp))
                {

                }
                else
                {
                    txtPQuantity.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPQuantity);
                    return;
                }
            }
            else
            {
                txtPQuantity.Text = "0";
            }
            if (txtPFreeQuantity.Text == "")
            {
                txtPFreeQuantity.Text = "0";
            }


            if (hdnProductId.Value == "")
            {
                if (strA == "0")
                {
                    //Add_Tax_In_Session(txtPUnitPrice.Text, hdnNewProductId.Value);                    
                    FillProductChidGird("Save");
                    Grid_Calculation("GvProductDetail", "Add Product");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                    GetGridTotal();

                }
                else
                {
                    //txtProductName.Text = "";
                    //txtProductcode.Text = "";
                    //DisplayMessage("Product Name Already Exists");
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
                }
            }
            else
            {
                if (txtProductName.Text == hdnProductName.Value)
                {
                    //Add_Tax_In_Session(txtPUnitPrice.Text, hdnNewProductId.Value);                    
                    FillProductChidGird("Edit");
                    Grid_Calculation("GvProductDetail", "Add Product");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                    pnlProduct1.Visible = false;
                    GetGridTotal();

                }
                else
                {
                    if (strA == "0")
                    {
                        //Add_Tax_In_Session(txtPUnitPrice.Text, hdnNewProductId.Value);                        
                        FillProductChidGird("Edit");
                        Grid_Calculation("GvProductDetail", "Add Product");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                        pnlProduct1.Visible = false;
                        GetGridTotal();

                    }
                    else
                    {
                        //txtProductName.Text = "";
                        //txtProductcode.Text = "";
                        //DisplayMessage("Product Name Already Exists");
                        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
                    }
                }
            }

        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        txtProductcode.Focus();
        //AllPageCode();
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetProduct();
        //AllPageCode();
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
        GetGridTotal();

        //AllPageCode();
    }
    public void ResetProduct()
    {
        txtProductName.Text = "";
        FillProductUnit();
        txtPDescription.Text = "";

        txtPQuantity.Text = "1";
        txtPFreeQuantity.Text = "";
        txtPUnitPrice.Text = "0";
        txtPQuantityPrice.Text = "";

        txtPTaxPUnit.Text = "";
        txtPTaxVUnit.Text = "";



        txtPDiscountPUnit.Text = "";
        txtPDiscountVUnit.Text = "";


        txtPTotalAmount.Text = "";


        hdnProductId.Value = "";
        hdnProductName.Value = "";
        hdnNewProductId.Value = "0";
        txtProductcode.Text = "";
        txtProductcode.Focus();
        //AllPageCode();
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("FreeQty");
        dt.Columns.Add("RemainQty");
        dt.Columns.Add("UnitPrice");
        dt.Columns.Add("GrossPrice");

        dt.Columns.Add("TaxP");
        dt.Columns.Add("TaxV");
        dt.Columns.Add("DiscountP");
        dt.Columns.Add("DiscountV");

        dt.Columns.Add("TaxPUnit");
        dt.Columns.Add("TaxVUnit");
        dt.Columns.Add("DiscountPUnit");
        dt.Columns.Add("DiscountVUnit");
        dt.Columns.Add("NetTotal");
        dt.Columns.Add("Field6", typeof(bool));
        dt.Columns.Add("AgentCommission");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateProductDataTable();
        if (GvProductDetail.Rows.Count > 0)
        {
            for (int i = 0; i < GvProductDetail.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvProductDetail.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvProductDetail.Rows[i].FindControl("lblgvSerialNo");
                    HiddenField hdngvProductId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvProductId");
                    HiddenField hdngvUnitId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvUnitId");
                    TextBox lblgvQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvQuantity");
                    TextBox lblgvFreeQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvFreeQuantity");
                    Label lblgvRemainQuantity = (Label)GvProductDetail.Rows[i].FindControl("lblgvRemainQuantity");
                    TextBox lblgvUnitPrice = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvUnitPrice");
                    TextBox lblgvTaxP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxP");
                    TextBox lblgvTaxV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxV");
                    TextBox lblgvDiscountP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountP");
                    TextBox lblgvDiscountV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountV");
                    TextBox txtgvAgentCommission = (TextBox)GvProductDetail.Rows[i].FindControl("txtgvAgentCommission");

                    CheckBox chkProduction = (CheckBox)GvProductDetail.Rows[i].FindControl("chkIsProduction");
                    Label lblgvTotal = (Label)GvProductDetail.Rows[i].FindControl("lblgvTotal");
                    Label lblgvQuantityPrice = (Label)GvProductDetail.Rows[i].FindControl("lblgvQuantityPrice");

                    dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["Product_Id"] = hdngvProductId.Value;
                    dt.Rows[i]["UnitId"] = hdngvUnitId.Value;
                    dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
                    dt.Rows[i]["FreeQty"] = lblgvFreeQuantity.Text;
                    dt.Rows[i]["RemainQty"] = lblgvRemainQuantity.Text;
                    dt.Rows[i]["UnitPrice"] = lblgvUnitPrice.Text;
                    dt.Rows[i]["TaxP"] = lblgvTaxP.Text;
                    dt.Rows[i]["TaxV"] = lblgvTaxV.Text;
                    dt.Rows[i]["DiscountP"] = lblgvDiscountP.Text;
                    dt.Rows[i]["DiscountV"] = lblgvDiscountV.Text;
                    dt.Rows[i]["Field6"] = chkProduction.Checked;
                    dt.Rows[i]["NetTotal"] = lblgvTotal.Text;
                    dt.Rows[i]["GrossPrice"] = lblgvQuantityPrice.Text;
                    dt.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;

                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Serial_No"] = (Convert.ToDouble(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                    dt.Rows[i]["UnitId"] = hdnUnitId.Value;
                    dt.Rows[i]["Quantity"] = txtPQuantity.Text;
                    dt.Rows[i]["FreeQty"] = txtPFreeQuantity.Text;
                    dt.Rows[i]["RemainQty"] = txtPQuantity.Text;
                    dt.Rows[i]["UnitPrice"] = txtPUnitPrice.Text;
                    dt.Rows[i]["TaxP"] = txtPTaxPUnit.Text;
                    dt.Rows[i]["TaxV"] = txtPTaxVUnit.Text;
                    dt.Rows[i]["DiscountP"] = txtPDiscountPUnit.Text;
                    dt.Rows[i]["DiscountV"] = txtPDiscountVUnit.Text;
                    dt.Rows[i]["NetTotal"] = txtPTotalAmount.Text;
                    dt.Rows[i]["GrossPrice"] = txtPQuantityPrice.Text;
                    dt.Rows[i]["Field6"] = false;
                    dt.Rows[i]["AgentCommission"] = "0";
                }
            }
        }
        else
        {

            dt.Rows.Add(0);
            dt.Rows[0]["Serial_No"] = "1";
            dt.Rows[0]["Product_Id"] = hdnNewProductId.Value;
            dt.Rows[0]["UnitId"] = hdnUnitId.Value;
            dt.Rows[0]["Quantity"] = txtPQuantity.Text;
            dt.Rows[0]["FreeQty"] = txtPFreeQuantity.Text;
            dt.Rows[0]["RemainQty"] = txtPQuantity.Text;
            dt.Rows[0]["UnitPrice"] = txtPUnitPrice.Text;
            dt.Rows[0]["TaxP"] = txtPTaxPUnit.Text;
            dt.Rows[0]["TaxV"] = txtPTaxVUnit.Text;
            dt.Rows[0]["DiscountP"] = txtPDiscountPUnit.Text;
            dt.Rows[0]["DiscountV"] = txtPDiscountVUnit.Text;
            dt.Rows[0]["NetTotal"] = txtPTotalAmount.Text;
            dt.Rows[0]["GrossPrice"] = txtPQuantityPrice.Text;
            dt.Rows[0]["Field6"] = false;
            dt.Rows[0]["AgentCommission"] = "0";
        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProductDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProductDetail.Rows[i].FindControl("lblgvSerialNo");
            HiddenField hdngvProductId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvQuantity");
            TextBox lblgvFreeQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvFreeQuantity");
            Label lblgvRemainQuantity = (Label)GvProductDetail.Rows[i].FindControl("lblgvRemainQuantity");
            TextBox lblgvUnitPrice = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvUnitPrice");
            TextBox lblgvTaxP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxP");
            TextBox lblgvTaxV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxV");
            TextBox lblgvDiscountP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountP");
            TextBox lblgvDiscountV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountV");
            CheckBox chkProduction = (CheckBox)GvProductDetail.Rows[i].FindControl("chkIsProduction");
            Label lblgvTotal = (Label)GvProductDetail.Rows[i].FindControl("lblgvTotal");
            Label lblgvQuantityPrice = (Label)GvProductDetail.Rows[i].FindControl("lblgvQuantityPrice");
            TextBox txtgvAgentCommission = (TextBox)GvProductDetail.Rows[i].FindControl("txtgvAgentCommission");
            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Product_Id"] = hdngvProductId.Value;
            dt.Rows[i]["UnitId"] = hdngvUnitId.Value;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["FreeQty"] = lblgvFreeQuantity.Text;
            dt.Rows[i]["RemainQty"] = lblgvRemainQuantity.Text;
            dt.Rows[i]["UnitPrice"] = GetAmountDecimal(lblgvUnitPrice.Text);
            dt.Rows[i]["TaxP"] = GetAmountDecimal(lblgvTaxP.Text);
            dt.Rows[i]["TaxV"] = GetAmountDecimal(lblgvTaxV.Text);
            dt.Rows[i]["DiscountP"] = GetAmountDecimal(lblgvDiscountP.Text);
            dt.Rows[i]["DiscountV"] = GetAmountDecimal(lblgvDiscountV.Text);
            dt.Rows[i]["NetTotal"] = GetAmountDecimal(lblgvTotal.Text);
            dt.Rows[i]["Field6"] = chkProduction.Checked;
            dt.Rows[i]["GrossPrice"] = lblgvQuantityPrice.Text;
            dt.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No<>'" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    protected void imgBtnProductEdit_Command(object sender, CommandEventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }

        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductDataTabelEdit();
        pnlProduct1.Visible = true;
        txtProductcode.Focus();
        //AllPageCode();
    }
    public DataTable FillProductDataTabelEdit()
    {
        DataTable dt = CreateProductDataTable();

        for (int i = 0; i < GvProductDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProductDetail.Rows[i].FindControl("lblgvSerialNo");
            HiddenField hdngvProductId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvQuantity");
            TextBox lblgvFreeQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvFreeQuantity");
            Label lblgvRemainQuantity = (Label)GvProductDetail.Rows[i].FindControl("lblgvRemainQuantity");
            TextBox lblgvUnitPrice = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvUnitPrice");
            TextBox lblgvTaxP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxP");
            TextBox lblgvTaxV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxV");
            TextBox lblgvDiscountP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountP");
            TextBox lblgvDiscountV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountV");
            CheckBox chkProduction = (CheckBox)GvProductDetail.Rows[i].FindControl("chkIsProduction");
            Label lblgvTotal = (Label)GvProductDetail.Rows[i].FindControl("lblgvTotal");
            Label lblgvQuantityPrice = (Label)GvProductDetail.Rows[i].FindControl("lblgvQuantityPrice");
            TextBox txtgvAgentCommission = (TextBox)GvProductDetail.Rows[i].FindControl("txtgvAgentCommission");
            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Product_Id"] = hdngvProductId.Value;
            dt.Rows[i]["UnitId"] = hdngvUnitId.Value;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["FreeQty"] = lblgvFreeQuantity.Text;
            dt.Rows[i]["RemainQty"] = lblgvRemainQuantity.Text;
            dt.Rows[i]["UnitPrice"] = GetAmountDecimal(lblgvUnitPrice.Text);
            dt.Rows[i]["TaxP"] = GetAmountDecimal(lblgvTaxP.Text);
            dt.Rows[i]["TaxV"] = GetAmountDecimal(lblgvTaxV.Text);
            dt.Rows[i]["DiscountP"] = GetAmountDecimal(lblgvDiscountP.Text);
            dt.Rows[i]["DiscountV"] = GetAmountDecimal(lblgvDiscountV.Text);
            dt.Rows[i]["GrossPrice"] = GetAmountDecimal(lblgvQuantityPrice.Text);
            dt.Rows[i]["Field6"] = chkProduction.Checked;
            dt.Rows[i]["NetTotal"] = GetAmountDecimal(lblgvTotal.Text);
            dt.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No='" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {

            txtProductName.Text = GetProductName(dt.Rows[0]["Product_Id"].ToString());
            txtProductcode.Text = ProductCode(dt.Rows[0]["Product_Id"].ToString());
            FillProductUnit();

            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            txtPQuantity.Text = dt.Rows[0]["Quantity"].ToString();
            txtPFreeQuantity.Text = dt.Rows[0]["FreeQty"].ToString();
            txtPUnitPrice.Text = GetAmountDecimal(dt.Rows[0]["UnitPrice"].ToString());




            txtPTaxPUnit.Text = GetAmountDecimal(dt.Rows[0]["TaxP"].ToString());
            txtPTaxVUnit.Text = GetAmountDecimal(dt.Rows[0]["TaxV"].ToString());
            txtPDiscountPUnit.Text = GetAmountDecimal(dt.Rows[0]["DiscountP"].ToString());
            txtPDiscountVUnit.Text = GetAmountDecimal(dt.Rows[0]["DiscountV"].ToString());

            hdnProductName.Value = GetProductName(dt.Rows[0]["Product_Id"].ToString());
            txtPTotalAmount.Text = GetAmountDecimal(dt.Rows[0]["NetTotal"].ToString());
            txtPQuantityPrice.Text = GetAmountDecimal(dt.Rows[0]["GrossPrice"].ToString());

        }
        return dt;

    }
    protected void imgBtnProductDelete_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();


        if (ViewState["DtTax"] != null)
        {

            DataView view = new DataView();
            try
            {
                view = new DataView((DataTable)ViewState["DtTax"], "productId<>'" + hdnProductId.Value + "'", "", DataViewRowState.CurrentRows);
            }
            catch
            {
            }


            ViewState["DtTax"] = view.ToTable();
        }


        FillProductChidGird("Del");
        GetGridTotal();
        //AllPageCode();
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillProductDataTableUpdate();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductDetail, dt, "", "");
        TaxCalculationWithDiscount_ProductDetail();
        GvProductDetail.Columns[13].Visible = false;
        GvProductDetail.Columns[16].Visible = false;

        setDecimal();


        ResetProduct();
    }
    protected void GridRowQuotationValueChange()
    {

        double fGrossTotal = 0.00f;
        double fGrossTax = 0.00f;
        double fGrossDiscount = 0.00f;
        double fGrossPriceAfterTax = 0.0f;
        double fTotal = 0.0f;


        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            CheckBox chkgvQuantitySelect = (CheckBox)gvr.FindControl("chkGvQuotationDetailSelect");
            if (chkgvQuantitySelect.Checked)
            {
                TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
                TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
                TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
                TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
                TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");




                ((TextBox)gvr.FindControl("txtgvTaxP")).Text = GetAmountDecimal(txtTaxP.Text);


                ((TextBox)gvr.FindControl("txtgvTaxV")).Text = GetAmountDecimal(((Convert.ToDouble(txtgvQuantityPrice.Text) * Convert.ToDouble(txtgvTaxP.Text)) / (100)).ToString());
                txtgvPriceAfterTax.Text = GetAmountDecimal((Convert.ToDouble(txtgvQuantityPrice.Text) + Convert.ToDouble(txtgvTaxV.Text)).ToString());
                ((TextBox)gvr.FindControl("txtgvDiscountV")).Text = GetAmountDecimal(((Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text) * Convert.ToDouble(((TextBox)gvr.FindControl("txtgvDiscountP")).Text)) / (100)).ToString());
                ((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text = GetAmountDecimal((Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text) - Convert.ToDouble(((TextBox)gvr.FindControl("txtgvDiscountV")).Text)).ToString());
                ((TextBox)gvr.FindControl("txtgvTotal")).Text = GetAmountDecimal(Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text).ToString());


                fGrossTotal = fGrossTotal + Convert.ToDouble(txtgvQuantityPrice.Text);

                fGrossTax = fGrossTax + Convert.ToDouble(txtgvTaxV.Text);

                fGrossDiscount = fGrossDiscount + Convert.ToDouble(txtgvDiscountV.Text);

                fGrossPriceAfterTax = fGrossPriceAfterTax + Convert.ToDouble(txtgvPriceAfterTax.Text);

                fTotal = fTotal + Convert.ToDouble(txtgvTotal.Text);


            }



        }//end for
        txtAmount.Text = GetAmountDecimal(fGrossTotal.ToString());
        txtDiscountV.Text = GetAmountDecimal(fGrossDiscount.ToString());
        txtTaxV.Text = GetAmountDecimal(fGrossTax.ToString());
        txtPriceAfterTax.Text = GetAmountDecimal(fGrossPriceAfterTax.ToString());
        txtTotalAmount.Text = GetAmountDecimal(fTotal.ToString());
        try
        {
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }
        if (fGrossDiscount.ToString() != "0")
        {
            try
            {
                txtDiscountP.Text = GetAmountDecimal(((Convert.ToDouble(fGrossDiscount.ToString()) * 100) / (Convert.ToDouble(fGrossPriceAfterTax.ToString()))).ToString());
            }
            catch
            {
                txtDiscountP.Text = GetAmountDecimal("0");
            }
        }
        if (txtShippingCharge.Text != "")
        {
            txtNetAmount.Text = GetAmountDecimal(((Convert.ToDouble(txtTotalAmount.Text)) - (Convert.ToDouble(txtShippingCharge.Text))).ToString());
        }
        else
        {
            txtShippingCharge.Text = GetAmountDecimal("0");
        }



    }
    //Coded by Varsha
    protected void GridRowQuotationDiscountValueChange()
    {

        double fGrossTotal = 0.00f;
        double fGrossTax = 0.00f;
        double fGrossDiscount = 0.00f;
        double fGrossPriceAfterTax = 0.0f;
        double fTotal = 0.0f;

        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
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

            if (chkgvQuantitySelect.Checked)
            {
                ((TextBox)gvr.FindControl("txtgvDiscountP")).Text = GetAmountDecimal(txtDiscountP.Text);
                ((TextBox)gvr.FindControl("txtgvDiscountV")).Text = GetAmountDecimal(((Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text) * Convert.ToDouble(((TextBox)gvr.FindControl("txtgvDiscountP")).Text)) / (100)).ToString());
                ((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text = GetAmountDecimal((Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterTax")).Text) - Convert.ToDouble(((TextBox)gvr.FindControl("txtgvDiscountV")).Text)).ToString());
                ((TextBox)gvr.FindControl("txtgvTotal")).Text = GetAmountDecimal(Convert.ToDouble(((TextBox)gvr.FindControl("txtgvPriceAfterDiscount")).Text).ToString());

                if (txtgvQuantityPrice.Text != "")
                {
                    fGrossTotal = fGrossTotal + Convert.ToDouble(txtgvQuantityPrice.Text);
                }
                if (txtgvTaxV.Text != "")
                {
                    fGrossTax = fGrossTax + Convert.ToDouble(txtgvTaxV.Text);
                }
                if (txtgvDiscountV.Text != "")
                {
                    fGrossDiscount = fGrossDiscount + Convert.ToDouble(txtgvDiscountV.Text);
                }
                if (txtgvPriceAfterTax.Text != "")
                {
                    fGrossPriceAfterTax = fGrossPriceAfterTax + Convert.ToDouble(txtgvPriceAfterTax.Text);
                }
                if (txtgvTotal.Text != "")
                {
                    fTotal = fTotal + Convert.ToDouble(txtgvTotal.Text);
                }

            }



        }//end for
        txtAmount.Text = GetAmountDecimal(fGrossTotal.ToString());
        txtDiscountV.Text = GetAmountDecimal(fGrossDiscount.ToString());
        txtTaxV.Text = GetAmountDecimal(fGrossTax.ToString());
        txtPriceAfterTax.Text = GetAmountDecimal(fGrossPriceAfterTax.ToString());
        txtTotalAmount.Text = GetAmountDecimal(fTotal.ToString());
        if (fGrossDiscount.ToString() != "0")
        {
            try
            {
                txtDiscountP.Text = GetAmountDecimal(((Convert.ToDouble(fGrossDiscount.ToString()) * 100) / (Convert.ToDouble(fGrossPriceAfterTax.ToString()))).ToString());
            }
            catch
            {
                txtDiscountP.Text = GetAmountDecimal("0");
            }
        }
        if (txtShippingCharge.Text != "")
        {
            txtNetAmount.Text = ((Convert.ToDouble(txtTotalAmount.Text)) - (Convert.ToDouble(txtShippingCharge.Text))).ToString();
        }
        else
        {
            txtShippingCharge.Text = GetAmountDecimal("0");
        }
        try
        {
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }
    }

    public DataTable FillProductDataTableUpdate()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProductDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvProductDetail.Rows[i].FindControl("lblgvSerialNo");
            HiddenField hdngvProductId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)GvProductDetail.Rows[i].FindControl("hdngvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvQuantity");
            TextBox lblgvFreeQuantity = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvFreeQuantity");
            Label lblgvRemainQuantity = (Label)GvProductDetail.Rows[i].FindControl("lblgvRemainQuantity");
            TextBox lblgvUnitPrice = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvUnitPrice");
            TextBox lblgvTaxP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxP");
            TextBox lblgvTaxV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvTaxV");
            TextBox lblgvDiscountP = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountP");
            TextBox lblgvDiscountV = (TextBox)GvProductDetail.Rows[i].FindControl("lblgvDiscountV");
            Label lblgvTotal = (Label)GvProductDetail.Rows[i].FindControl("lblgvTotal");
            Label lblgvQuantityPrice = (Label)GvProductDetail.Rows[i].FindControl("lblgvQuantityPrice");
            TextBox txtgvAgentCommission = (TextBox)GvProductDetail.Rows[i].FindControl("txtgvAgentCommission");

            dt.Rows[i]["Serial_No"] = lblSNo.Text;
            dt.Rows[i]["Product_Id"] = hdngvProductId.Value;
            dt.Rows[i]["UnitId"] = hdngvUnitId.Value;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["FreeQty"] = lblgvFreeQuantity.Text;
            dt.Rows[i]["RemainQty"] = lblgvRemainQuantity.Text;
            dt.Rows[i]["UnitPrice"] = GetAmountDecimal(lblgvUnitPrice.Text);
            dt.Rows[i]["TaxP"] = GetAmountDecimal(lblgvTaxP.Text);
            dt.Rows[i]["TaxV"] = GetAmountDecimal(lblgvTaxV.Text);
            dt.Rows[i]["DiscountP"] = GetAmountDecimal(lblgvDiscountP.Text);
            dt.Rows[i]["DiscountV"] = GetAmountDecimal(lblgvDiscountV.Text);
            dt.Rows[i]["Nettotal"] = GetAmountDecimal(lblgvTotal.Text);
            dt.Rows[i]["GrossPrice"] = GetAmountDecimal(lblgvQuantityPrice.Text);
            dt.Rows[i]["AgentCommission"] = GetAmountDecimal(txtgvAgentCommission.Text);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnProductId.Value == dt.Rows[i]["Serial_No"].ToString())
            {
                dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                dt.Rows[i]["UnitId"] = hdnUnitId.Value;
                dt.Rows[i]["Quantity"] = txtPQuantity.Text;
                dt.Rows[i]["FreeQty"] = txtPFreeQuantity.Text;
                dt.Rows[i]["RemainQty"] = txtPQuantity.Text;
                dt.Rows[i]["UnitPrice"] = GetAmountDecimal(txtPUnitPrice.Text);
                dt.Rows[i]["TaxP"] = GetAmountDecimal(txtPTaxPUnit.Text);
                dt.Rows[i]["TaxV"] = GetAmountDecimal(txtPTaxVUnit.Text);
                dt.Rows[i]["DiscountP"] = GetAmountDecimal(txtPDiscountPUnit.Text);
                dt.Rows[i]["DiscountV"] = GetAmountDecimal(txtPDiscountVUnit.Text);
                dt.Rows[i]["Nettotal"] = GetAmountDecimal(txtPTotalAmount.Text);
                dt.Rows[i]["GrossPrice"] = GetAmountDecimal(txtPQuantityPrice.Text);
            }
        }
        return dt;
    }
    protected void txtPTaxP_TextChanged(object sender, EventArgs e)
    {


        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }

        if (txtPTaxPUnit.Text == "")
        {
            txtPTaxPUnit.Text = "0";
        }

        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", "", txtPDiscountVUnit.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());

        txtPTaxVUnit.Text = strCalculation[4].ToString();

        txtPTotalAmount.Text = strCalculation[5].ToString();
        Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());


        //AllPageCode();
    }
    protected void txtPTaxV_TextChanged(object sender, EventArgs e)
    {
        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }
        if (txtPTaxVUnit.Text == "")
        {
            txtPTaxVUnit.Text = "0";
        }


        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, "", txtPTaxVUnit.Text, "", txtPDiscountVUnit.Text, true, StrCurrencyId, true, Session["DBConnection"].ToString());

        txtPTaxPUnit.Text = strCalculation[3].ToString();

        txtPTotalAmount.Text = strCalculation[5].ToString();

        //AllPageCode();
    }
    protected void txtPDiscountP_TextChanged(object sender, EventArgs e)
    {

        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }



        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", txtPDiscountPUnit.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtPDiscountVUnit.Text = strCalculation[2].ToString();
        txtPTaxVUnit.Text = strCalculation[4].ToString();
        txtPTotalAmount.Text = strCalculation[5].ToString();

    }
    protected void txtPDiscountV_TextChanged(object sender, EventArgs e)
    {
        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }



        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", "", txtPDiscountVUnit.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtPDiscountPUnit.Text = strCalculation[1].ToString();
        txtPTaxVUnit.Text = strCalculation[4].ToString();
        txtPTotalAmount.Text = strCalculation[5].ToString();

        //AllPageCode();
    }
    protected void txtPQuantity_TextChanged(object sender, EventArgs e)
    {

        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }



        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", txtPDiscountPUnit.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtPQuantityPrice.Text = strCalculation[0].ToString();
        txtPDiscountVUnit.Text = strCalculation[2].ToString();
        txtPTaxVUnit.Text = strCalculation[4].ToString();
        txtPTotalAmount.Text = strCalculation[5].ToString();

        //AllPageCode();
    }

    protected void txtPUnitPrice_TextChanged(object sender, EventArgs e)
    {
        if (txtPUnitPrice.Text == "")
        {
            txtPUnitPrice.Text = "0";
        }
        if (txtPQuantity.Text == "")
        {
            txtPQuantity.Text = "0";
        }
        string[] strCalculation = Common.TaxDiscountCaluculation(txtPUnitPrice.Text, txtPQuantity.Text, txtPTaxPUnit.Text, "", txtPDiscountPUnit.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());
        txtPQuantityPrice.Text = strCalculation[0].ToString();
        txtPDiscountVUnit.Text = strCalculation[2].ToString();
        txtPTaxVUnit.Text = strCalculation[4].ToString();
        txtPTotalAmount.Text = strCalculation[5].ToString();
        //AllPageCode();
    }
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.getAddressNamePreText(prefixText);
        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "", "Address_Name Asc", DataViewRowState.CurrentRows).ToTable();

            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Address_Name"].ToString();
                }
            }

            return str;

        }
        else
        {
            return null;
        }
    }
    //
    protected void txtInvoiceTo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceTo.Text.Trim() != "")
        {
            DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtInvoiceTo.Text.Trim().Split('/')[0].ToString());
            if (dtAM.Rows.Count > 0)
            {
                string InvAddress = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                InvoiceAddress.Text = InvAddress;
            }
            else
            {
                txtInvoiceTo.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceTo);
                return;
            }
        }

        //AllPageCode();
    }
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtShipingAddress.Text);
            if (dtAM.Rows.Count > 0)
            {
                string name = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                ShippingAddress.Text = name;

            }
            else
            {
                txtShipingAddress.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
                return;
            }
        }

        //AllPageCode();
    }

    #region View

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        ViewState["View"] = "Yes";
        btnEdit_Command(sender, e);
        //AllPageCode();
    }

    protected void GvProductDetailView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (!IsPostBack)
        {
            GridView gvProduct = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableHeaderCell cell = new TableHeaderCell();


                cell = new TableHeaderCell();
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
                cell.ColumnSpan = 3;
                cell.Text = Resources.Attendance.Tax;

                Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

                DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
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
                cell.ColumnSpan = 3;
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
                cell.ColumnSpan = 1;
                cell.Text = Resources.Attendance.Total;
                row.Controls.Add(cell);
                cell = new TableHeaderCell();

                gvProduct.Controls[0].Controls.Add(row);
            }
        }
    }



    #endregion
    #region SetDecimal
    public string GetAmountDecimal(string Amount)
    {

        return SystemParameter.GetAmountWithDecimal(Amount, hdnDecimalCount.Value);

    }

    public void setDecimal()
    {
        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            ((TextBox)gvr.FindControl("lblgvQuantity")).Text = ((TextBox)gvr.FindControl("lblgvQuantity")).Text;
            ((TextBox)gvr.FindControl("txtgvUnitPrice")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvUnitPrice")).Text);

            ((TextBox)gvr.FindControl("txtgvQuantityPrice")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvQuantityPrice")).Text);
            ((TextBox)gvr.FindControl("txtgvTaxP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvTaxP")).Text);
            ((TextBox)gvr.FindControl("txtgvTaxV")).Text = Convert_Into_DF(((TextBox)gvr.FindControl("txtgvTaxV")).Text);
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

            ((TextBox)gvr.FindControl("lblgvTaxV")).Text = Convert_Into_DF(((TextBox)gvr.FindControl("lblgvTaxV")).Text);

            ((TextBox)gvr.FindControl("lblgvDiscountP")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvDiscountP")).Text);

            ((TextBox)gvr.FindControl("lblgvDiscountV")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("lblgvDiscountV")).Text);

            ((Label)gvr.FindControl("lblgvTotal")).Text = GetAmountDecimal(((Label)gvr.FindControl("lblgvTotal")).Text);
            ((TextBox)gvr.FindControl("txtgvAgentCommission")).Text = GetAmountDecimal(((TextBox)gvr.FindControl("txtgvAgentCommission")).Text);

        }

        txtNetAmount.Text = GetAmountDecimal(txtNetAmount.Text);
        txtPTotalAmount.Text = GetAmountDecimal(txtPTotalAmount.Text);
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


    #endregion
    #region printReport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {

                string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                if (st == "Approved")
                {

                    PrintReport(e.CommandArgument.ToString());

                }
                else
                {
                    DisplayMessage("Cannot be Printed,Order not Approved");
                }
            }
            else
            {
                PrintReport(e.CommandArgument.ToString());
            }
        }
        //End Approval Code

    }
    void PrintReport(string OrderID)
    {

        string strCmd = string.Format("window.open('../Sales/SalesOrder_Print.aspx?Id=" + OrderID.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    #endregion

    protected void ddlUser_Click(object sender, EventArgs e)
    {
        // Reset();
        FillGrid(1);
        //FillGridBin();
        //FillRequestGrid(1);
        FillQuotationNo();
        //AllPageCode();
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }

    #region Quotation Grid search
    protected void btnRefreshQuoteReport_Click(object sender, EventArgs e)
    {
        btnPRequest_Click(sender, e);
        ddlFieldNameQuote.SelectedIndex = 1;
        ddlOptionQuote.SelectedIndex = 2;

        txtValueQuote.Text = "";
        txtValueQuoteDate.Text = "";
        txtValueQuoteDate.Visible = false;
        txtValueQuote.Visible = true;

        I3.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //AllPageCode();
    }

    protected void btnbindrptQuote_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");

        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameQuote.SelectedItem.Value == "Quotation_Date")
        {
            if (txtValueQuoteDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueQuoteDate.Text);
                    txtValueQuote.Text = Convert.ToDateTime(txtValueQuoteDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueQuoteDate.Text = "";
                    txtValueQuote.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueQuoteDate);
                    return;
                }

            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueQuoteDate.Focus();
                return;
            }
        }
        if (ddlOptionQuote.SelectedIndex != 0 && txtValueQuote.Text.Trim() != string.Empty)
        {
            string condition = string.Empty;
            string strSearchCondition = string.Empty;
            if (ddlOptionQuote.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String)='" + txtValueQuote.Text.Trim() + "'";
                strSearchCondition = ddlFieldNameQuote.SelectedValue + "='" + txtValueQuote.Text.Trim() + "'";
            }
            else if (ddlOptionQuote.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String) like '%" + txtValueQuote.Text.Trim() + "%'";
                strSearchCondition = ddlFieldNameQuote.SelectedValue + " like '%" + txtValueQuote.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String) Like '" + txtValueQuote.Text.Trim() + "%'";
                strSearchCondition = ddlFieldNameQuote.SelectedValue + " like '" + txtValueQuote.Text.Trim() + "'";
            }

            FillRequestGrid(1, strSearchCondition);
            //DataTable dtAdd = (DataTable)Session["dtSOrderrequest"];
            //DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //objPageCmn.FillData((object)GvSalesQuotation, view.ToTable(), "", "");

            //Session["dtFilterSOrderrequest"] = view.ToTable();
            //lblTotalRecordsQuote.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();
        }
        if (txtValueQuote.Text != "")
            txtValueQuote.Focus();
        else if (txtValueQuoteDate.Text != "")
            txtValueQuoteDate.Focus();
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string CurrencyAmount = string.Empty;
        try
        {
            CurrencyAmount = SystemParameter.GetCurrencySmbol(CurrencyId, GetAmountDecimal(Amount), Session["DBConnection"].ToString());
        }
        catch
        {
        }


        return CurrencyAmount;
    }
    #endregion
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "SalesOrderDate")
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

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "SalesOrderDate")
        {
            txtValueBinDate.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
    }
    protected void ddlFieldNameQuote_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");

        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameQuote.SelectedItem.Value == "Quotation_Date")
        {
            txtValueQuoteDate.Visible = true;
            txtValueQuote.Visible = false;
            txtValueQuote.Text = "";
            txtValueQuoteDate.Text = "";

        }
        else
        {
            txtValueQuoteDate.Visible = false;
            txtValueQuote.Visible = true;
            txtValueQuote.Text = "";
            txtValueQuoteDate.Text = "";

        }
    }

    #endregion
    #region Adavancesearch

    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        DataTable dt = getProductDetailinDatatable();
        Session["DtSearchProduct"] = dt;
        GetChildGridRecordInViewState();
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=SO&&CustomerId=" + txtCustomer.Text.Split('/')[3].ToString() + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductDetail, (DataTable)Session["DtSearchProduct"], "", "");

            Grid_Calculation("GvProductDetail", "Add Product");
            GvProductDetail.Columns[13].Visible = false;
            GvProductDetail.Columns[16].Visible = false;
            GetGridTotal();
            Session["DtSearchProduct"] = null;
            ViewState["DtTax"] = null;
        }
        else
        {

            DisplayMessage("Product Not Found");
            return;
        }
    }

    public DataTable getProductDetailinDatatable()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in GvProductDetail.Rows)
        {

            DataRow dr = dt.NewRow();
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
            TextBox lblgvQuantity = (TextBox)gvRow.FindControl("lblgvQuantity");
            TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
            Label lblgvRemainQuantity = (Label)gvRow.FindControl("lblgvRemainQuantity");
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            TextBox lblgvTaxP = (TextBox)gvRow.FindControl("lblgvTaxP");
            TextBox lblgvTaxV = (TextBox)gvRow.FindControl("lblgvTaxV");
            TextBox lblgvDiscountP = (TextBox)gvRow.FindControl("lblgvDiscountP");
            TextBox lblgvDiscountV = (TextBox)gvRow.FindControl("lblgvDiscountV");
            Label lblgvTotal = (Label)gvRow.FindControl("lblgvTotal");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            CheckBox chkProduction = (CheckBox)gvRow.FindControl("chkIsProduction");
            TextBox txtgvAgentCommission = (TextBox)gvRow.FindControl("txtgvAgentCommission");

            dr["Serial_No"] = lblgvSNo.Text;
            strNewSNo = lblgvSNo.Text;
            dr["Product_Id"] = hdngvProductId.Value;
            dr["UnitId"] = hdngvUnitId.Value;
            dr["Quantity"] = lblgvQuantity.Text;
            dr["FreeQty"] = lblgvFreeQuantity.Text;
            dr["RemainQty"] = lblgvRemainQuantity.Text;
            dr["UnitPrice"] = lblgvUnitPrice.Text;
            dr["TaxP"] = lblgvTaxP.Text;
            dr["TaxV"] = lblgvTaxV.Text;
            dr["DiscountP"] = lblgvDiscountP.Text;
            dr["DiscountV"] = lblgvDiscountV.Text;
            dr["NetTotal"] = lblgvTotal.Text;
            dr["GrossPrice"] = lblgvQuantityPrice.Text;
            dr["Field6"] = chkProduction.Checked;
            dr["AgentCommission"] = txtgvAgentCommission.Text;
            dt.Rows.Add(dr);
        }

        return dt;
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFormView.Checked == true)
        {
            btnAddNewProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            btnAddNewProduct_Click(null, null);

        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
            btnClosePanel_Click(null, null);
        }

    }
    #endregion
    #region AddCustomer

    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        //string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?Page=SO','window','width=1024');");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        string strCmd = string.Format("window.open('../Sales/AddContact.aspx?Page=SO','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
        {
            allow = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19", Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            allow = true;
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
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = GvProductDetail.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvchildGrid = (GridView)e.Row.FindControl("gvchildGrid");
            DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            //here we bind the location dropdown

            objPageCmn.FillData((object)ddlLocation, (DataTable)Session["dtLocation"], "Location_Name", "Location_Id");

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

    protected void GvQuotationDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = GvQuotationDetail.DataKeys[e.Row.RowIndex].Value.ToString();

            DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            //here we bind the location dropdown

            objPageCmn.FillData((object)ddlLocation, new DataView(objLocation.GetLocationMaster(Session["CompId"].ToString()), "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable(), "Location_Name", "Location_Id");

        }
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

        foreach (GridViewRow gvrow in GvProductDetail.Rows)
        {


            foreach (GridViewRow gvChildRow in ((GridView)gvrow.FindControl("gvchildGrid")).Rows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = ((HiddenField)gvrow.FindControl("hdngvProductId")).Value;
                dr[1] = ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value;
                dr[2] = ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value;
                dr[3] = ((Label)gvChildRow.FindControl("lblgvcategoryName")).Text;
                dr[4] = ((Label)gvChildRow.FindControl("lblgvtaxName")).Text;
                dr[5] = ((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text;
                dr[6] = ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text;
                dr[7] = ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked;

                dt.Rows.Add(dr);


            }
        }

        ViewState["DtTax"] = dt;
    }


    protected void chkselecttax_OnCheckedChanged(object sender, EventArgs e)
    {
        double priceafterdiscount = 0;
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;

        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (((TextBox)gvrow.FindControl("txttaxPerchild")).Text == "")
            {
                DisplayMessage("Enter Tax Percentage");
                return;
            }
        }
        GridView Childgrid = (GridView)(gvrow.Parent.Parent);

        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);

        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {

            try
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = GetAmountDecimal((priceafterdiscount * Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100).ToString());
            }
            catch
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
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


        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxV")).Text = TotalTax.ToString();

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxP")).Text = GetAmountDecimal(Percentage.ToString());


        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text != "")
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text)).ToString();
        }
        else
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = "0";
        }
        GetGridTotal();
    }


    protected void txttaxPerchild_OnTextChanged(object sender, EventArgs e)
    {

        double priceafterdiscount = 0;
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (((TextBox)gvrow.FindControl("txttaxPerchild")).Text == "")
            {
                ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = "0";
            }
        }
        GridView Childgrid = (GridView)(gvrow.Parent.Parent);

        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);

        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {

            try
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = GetAmountDecimal((priceafterdiscount * Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100).ToString());
            }
            catch
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
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


        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxV")).Text = TotalTax.ToString();

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxP")).Text = GetAmountDecimal(Percentage.ToString());


        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text != "")
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text)).ToString();
        }
        else
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = "0";
        }
        GetGridTotal();

    }

    protected void txttaxValuechild_OnTextChanged(object sender, EventArgs e)
    {


        double priceafterdiscount = 0;
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {
            if (((TextBox)gvrow.FindControl("txttaxValuechild")).Text == "")
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = "0";
            }
        }
        GridView Childgrid = (GridView)(gvrow.Parent.Parent);

        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);

        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {

            try
            {

                ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = GetAmountDecimal((Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxValuechild")).Text) * 100 / priceafterdiscount).ToString());

            }
            catch
            {
                ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = "0";
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


        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxV")).Text = TotalTax.ToString();

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTaxP")).Text = GetAmountDecimal(Percentage.ToString());


        if (((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text != "")
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvQuantity")).Text)).ToString();
        }
        else
        {
            ((Label)GvProductDetail.Rows[Gv1Row.RowIndex].FindControl("lblgvTotal")).Text = "0";
        }

        GetGridTotal();
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
        if (((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text != "")
        {

            if (txtPriceAfterDiscount.Text != "" && txtPriceAfterDiscount.Text != "0")
            {

                ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = GetAmountDecimal((Convert.ToDouble(txtPriceAfterDiscount.Text) * (Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text) / 100)).ToString());
            }
            else
            {
                ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = "0";
            }
        }

    }
    protected void txtTaxValueFooter_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text != "")
        {

            if (txtPriceAfterDiscount.Text != "" && txtPriceAfterDiscount.Text != "0")
            {

                ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = GetAmountDecimal(((Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text) * 100) / Convert.ToDouble(txtPriceAfterDiscount.Text)).ToString());
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
                contacts.Columns.Add("Tax_Per", typeof(double));
                contacts.Columns.Add("Tax_Value", typeof(double));

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
            contacts.Columns.Add("Tax_Per", typeof(double));
            contacts.Columns.Add("Tax_Value", typeof(double));

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

        DisplayMessage("Updated Successfully", "green");
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

            if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text == "")
            {
                DisplayMessage("Enter Tax value");
                return;
            }

            if (((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text == "")
            {
                DisplayMessage("Enter Tax Percentage");
                return;
            }

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
                DisplayMessage("Choose tax in sugestion only");
                return;
            }


            if (ViewState["dtTaxHeader"] == null)
            {
                dt.Columns.Add("Tax_Id", typeof(int));
                dt.Columns.Add("TaxName", typeof(string));
                dt.Columns.Add("Tax_Per", typeof(double));
                dt.Columns.Add("Tax_Value", typeof(double));
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



        if (e.CommandName.Equals("Update"))
        {



        }




        gridView.EditIndex = -1;
        LoadStores();

        TotalheaderTax();

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
                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = GetAmountDecimal((Convert.ToDouble(txtPriceAfterDiscount.Text) * (Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text) / 100)).ToString());
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
    public void TotalheaderTax()
    {
        double netTaxPer = 0;
        double nettaxval = 0;


        for (int i = 0; i < gridView.Rows.Count; i++)
        {

            try
            {
                netTaxPer += Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text);
                nettaxval += Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text);
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

        //AllPageCode();

        //if (txtAmount.Text == "")
        //{
        //    txtAmount.Text = "0";
        //}
        //if (txtDiscountV.Text == "")
        //{
        //    txtDiscountV.Text = "0";
        //}
        //if (txtTaxV.Text == "")
        //{
        //    txtTaxV.Text = "0";
        //}

        //txtTotalAmount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text) + Convert.ToDouble(txtTaxV.Text)).ToString();


        //if (txtShippingCharge.Text == "" || txtShippingCharge.Text == "0")
        //{
        //    txtNetAmount.Text = txtTotalAmount.Text;
        //}
        //else
        //{
        //    txtNetAmount.Text = GetAmountDecimal((Decimal.Parse(txtTotalAmount.Text) + Decimal.Parse(txtShippingCharge.Text)).ToString());
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
                        //return;
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
    #region Payment

    protected void ddlPaymentTypeMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
    }

    private void FillPaymentMode()
    {
        DataTable dsPaymentMode = null;
        dsPaymentMode = objPaymentMode.GetPaymentModeMaster(StrCompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            dsPaymentMode = new DataView(dsPaymentMode, "Pay_Mod_Name in ('Cash','Credit')", "Pay_Mode_Id asc", DataViewRowState.CurrentRows).ToTable();
            // dsPaymentMode = new DataView(dsPaymentMode, "Pay_Mod_Name in ('Cash')", "Pay_Mode_Id asc", DataViewRowState.CurrentRows).ToTable();
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
    public void fillTabPaymentMode(string PaymentType)
    {
        if (PaymentType.Trim() == "Cash")
        {
            DataTable dt = objPaymentMode.GetPaymentModeMaster(StrCompId.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            dt = new DataView(dt, "Field1='" + PaymentType.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlTabPaymentMode, dt, "Pay_Mod_Name", "Pay_Mode_Id");
        }
        else
        {
            ddlTabPaymentMode.Items.Clear();
            objPageCmn.FillData((object)gvPayment, null, "", "");
            ViewState["PayementDt"] = null;
        }
    }
    protected void btnPaymentSave_Click(object sender, object e)
    {
        if (ddlTabPaymentMode.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Payment Mode");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlTabPaymentMode);
            return;
        }
        if (txtPayAmount.Text == "")
        {
            DisplayMessage("Enter Net Price");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPayAmount);
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
            if (new DataView(dt, "PaymentModeId='" + ddlTabPaymentMode.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
            {
                DisplayMessage("Payment Mode already exist");
                fillPaymentGrid((DataTable)ViewState["PayementDt"]);
                return;
            }
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

        dt.Rows[dt.Rows.Count - 1]["PaymentModeId"] = ddlTabPaymentMode.SelectedValue.ToString();
        dt.Rows[dt.Rows.Count - 1]["PaymentName"] = ddlTabPaymentMode.SelectedItem.ToString();
        dt.Rows[dt.Rows.Count - 1]["FCPayAmount"] = txtPayAmount.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["Pay_Charges"] = txtLocalAmount.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["AccountNo"] = GetAccountId(txtPayAccountNo.Text);
        dt.Rows[dt.Rows.Count - 1]["CardNo"] = txtPayCardNo.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["CardName"] = txtPayCardName.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["PayExchangeRate"] = txtExchangerate.Text;
        dt.Rows[dt.Rows.Count - 1]["PayCurrencyID"] = ddlCurrency.SelectedValue;
        if (ddlPayBank.SelectedValue != "--Select--")
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

        ViewState["PayementDt"] = dt;

        btnPaymentReset_Click(null, null);
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
        fillPaymentGrid(dt);
        //here we change balance amount when we paid against the invoice amount

    }
    protected void btnPaymentReset_Click(object sender, EventArgs e)
    {

        txtPayAccountNo.Text = "";
        txtPayAmount.Text = "";

        txtPayCardName.Text = "";
        txtPayChequeNo.Text = "";
        txtPayCardNo.Text = "";
        txtPayChequeDate.Text = "";
        fillBank();

        trcheque.Visible = false;
        trcard.Visible = false;
        lblPayBank.Visible = false;
        //lblpaybankcolon.Visible = false;
        ddlPayBank.Visible = false;
    }
    public void fillBank()
    {
        DataTable dt = ObjBankMaster.GetBankMaster();


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlPayBank, dt, "Bank_Name", "Bank_Id");

    }
    public void fillPaymentGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");
        ViewState["PayementDt"] = dt;
        //AllPageCode();
        double f = 0;

        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            try
            {

                ((Label)gvrow.FindControl("lblAmount")).Text = GetAmountDecimal(((Label)gvrow.FindControl("lblAmount")).Text);
                ((Label)gvrow.FindControl("lblgvExp_Charges")).Text = GetAmountDecimal(((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
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
            ((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text = GetAmountDecimal(f.ToString());
        }
        catch
        {

        }

        //here we showing balance amount
        if (txtNetAmount.Text != "")
        {
            if (Convert.ToDouble(txtNetAmount.Text) > 0)
            {
                try
                {
                    txtPayAmount.Text = GetAmountDecimal((Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                }
                catch
                {

                    txtPayAmount.Text = txtNetAmount.Text;
                }
            }
        }


    }

    protected void txtPayAccountNo_TextChanged(object sender, EventArgs e)
    {

        if (txtPayAccountNo.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
            try
            {
                dtAccount = new DataView(dtAccount, "AccountName='" + txtPayAccountNo.Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtAccount.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPayAccountNo);
                    txtPayAccountNo.Text = "";
                    DisplayMessage("No Account Found");
                    txtPayAccountNo.Focus();
                    return;
                }

            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPayAccountNo);
                txtPayAccountNo.Text = "";
                DisplayMessage("No Account Found");
                txtPayAccountNo.Focus();
                return;
            }

        }
    }
    protected void btnDeletePay_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)ViewState["PayementDt"], "TransId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        fillPaymentGrid(dt);
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPaymentReset_Click(null, null);
        if (ddlTabPaymentMode.SelectedValue == "--Select--")
        {
            txtPayAccountNo.Text = "";
        }
        else if (ddlTabPaymentMode.SelectedValue != "--Select--")
        {
            if (ViewState["PayementDt"] != null)
            {
                DataTable dt = (DataTable)ViewState["PayementDt"];
                dt = new DataView(dt, "PaymentModeId='" + ddlTabPaymentMode.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    DataTable dtPay = objPaymentMode.GetPaymentModeMasterById(StrCompId, ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
                    if (dtPay.Rows.Count > 0)
                    {
                        string strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                        DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                        }

                    }

                }
                else
                {
                    string strAccountId = dt.Rows[0]["AccountNo"].ToString();
                    DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                    if (dtAcc.Rows.Count > 0)
                    {
                        string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                        txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                    }
                    txtPayAmount.Text = dt.Rows[0]["Pay_Charges"].ToString();
                    txtPayCardNo.Text = dt.Rows[0]["CardNo"].ToString();
                    txtPayCardName.Text = dt.Rows[0]["CardName"].ToString();
                    if (dt.Rows[0]["BankId"].ToString() != "--Sel")
                    {
                        try
                        {
                            ddlPayBank.SelectedValue = dt.Rows[0]["BankId"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    //txtPayBankAccountNo.Text = dt.Rows[0]["BankAccountNo"].ToString();
                    //txtPayBankAccountName.Text = dt.Rows[0]["BankAccountName"].ToString();
                    txtPayChequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                    txtPayChequeDate.Text = Convert.ToDateTime(dt.Rows[0]["ChequeDate"].ToString()).ToShortDateString();
                }
            }

            else
            {
                DataTable dtPay = objPaymentMode.GetPaymentModeMasterById(StrCompId, ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (dtPay.Rows.Count > 0)
                {

                    string strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                    DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                    if (dtAcc.Rows.Count > 0)
                    {
                        string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                        txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                    }
                }
            }
        }

        //here we showing related field according the select payment mode

        //when payment mode is cash then we showing accounts no only 
        if (ddlTabPaymentMode.SelectedIndex != 0)
        {
            if (objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Cash")
            {
                pnlpaybank.Visible = true;

            }
            else if (objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Credit")
            {
                pnlpaybank.Visible = true;
                lblPayBank.Visible = true;
                //lblpaybankcolon.Visible = true;
                ddlPayBank.Visible = true;
                trcheque.Visible = true;
            }
            else if (objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Card")
            {
                pnlpaybank.Visible = true;
                trcard.Visible = true;
            }
        }

        //here we change balance amount when we paid against the invoice amount

        if (txtNetAmount.Text != "")
        {
            if (Convert.ToDouble(txtNetAmount.Text) > 0)
            {
                try
                {
                    txtPayAmount.Text = GetAmountDecimal((Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                }
                catch
                {

                    txtPayAmount.Text = txtNetAmount.Text;
                }
            }
        }
        txtPayAmount_OnTextChanged(null, null);

    }

    protected void txtPayAmount_OnTextChanged(object sender, EventArgs e)
    {

        Double ForeignAmt = 0;
        Double Exchangerate = 0;

        Double.TryParse(txtPayAmount.Text, out ForeignAmt);
        Double.TryParse(txtExchangerate.Text, out Exchangerate);

        txtLocalAmount.Text = SystemParameter.SetDecimal((ForeignAmt * Exchangerate).ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());

    }


    private string GetAccountId(string strAccountName)
    {
        string retval = string.Empty;
        if (strAccountName != "")
        {
            retval = (strAccountName.Split('/'))[strAccountName.Split('/').Length - 1];

            DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, retval);
            if (dtAccount.Rows.Count > 0)
            {

            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }

    #endregion
    #region PendingSalesOrder
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid(1);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        //AllPageCode();
    }
    #endregion
    //protected string GetCurrencySymbol(string Amount)
    //{ return SystemParameter.GetCurrencySmbol(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount)); }
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
    #region LOcationStock

    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string CustomerName = string.Empty;

        try
        {
            CustomerName = txtCustomer.Text.Split('/')[0].ToString();
        }
        catch
        {

        }

        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=SALES&&Contact=" + CustomerName + "')";

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);



        //DataTable dt = objStockDetail.GetStockDetail(Session["CompId"].ToString(), e.CommandArgument.ToString());
        //if (dt.Rows.Count == 0)
        //{
        //    DisplayMessage("Stock Not Found");
        //    return;
        //}
        //try
        //{
        //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    objPageCmn.FillData((object)gvStockInfo, dt, "", "");
        //}
        //catch
        //{
        //}
        //pnlStock1.Visible = true;
        //pnlStock2.Visible = true;
    }


    #endregion
    #region Commissionagent
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
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
        dtContact = null;
        return filterlist;
    }
    protected void txtAgentName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtAgentName.Text != "")
        {
            strCustomerId = GetCustomerId(txtAgentName);
            if (strCustomerId != "" && strCustomerId != "0")
            {
                GvProductDetail.Columns[GvProductDetail.Columns.Count - 2].Visible = true;
                GvProductDetail.DataSource = getProductDetailinDatatable();
                GvProductDetail.DataBind();
                //GvQuotationDetail_RowCreated(GvQuotationDetail, new GridViewRowEventArgs(GvQuotationDetail.HeaderRow));
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAgentName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAgentName);
                GvProductDetail.Columns[GvProductDetail.Columns.Count - 2].Visible = false;
                GvProductDetail.DataSource = getProductDetailinDatatable();
                GvProductDetail.DataBind();
                return;
            }
        }
        else
        {
            txtAgentName.Text = "";
            txtAgentName.Focus();
            GvProductDetail.Columns[GvProductDetail.Columns.Count - 2].Visible = false;
            GvProductDetail.DataSource = getProductDetailinDatatable();
            GvProductDetail.DataBind();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAgentName);
        }
    }

    #endregion



    public bool IsAddAgentCommission(string str)
    {
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        bool IsAllow = false;

        if (Session["EmpId"].ToString() == "0")
        {
            IsAllow = true;
        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), Session["AccordianId"].ToString(), "67", Session["CompId"].ToString());

            if (new DataView(dtAllPageCode, "Op_Id=16", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                IsAllow = true;

            }
        }

        return IsAllow;
    }


    protected void lnkcustomerHistory_OnClick(object sender, EventArgs e)
    {
        string CustomerId = string.Empty;


        if (txtCustomer.Text != "")
        {
            try
            {
                CustomerId = txtCustomer.Text.Split('/')[3].ToString();
            }
            catch
            {
                CustomerId = "0";
            }
        }
        else
        {
            CustomerId = "0";
        }

        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + CustomerId + "&&Page=SO','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }



    //protected void BtnAddTax_Gv_Detail_Command(object sender, CommandEventArgs e)
    //{
    //    string ProductId = e.CommandArgument.ToString();
    //    string TaxQuery = string.Empty;
    //    //if (ddlTransType.SelectedIndex > 0)
    //    //    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,IPTM.Product_Id,Tax_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //    //                    where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "";
    //    //else
    //    //    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,IPTM.Product_Id,Tax_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //    //                    where IPTM.Product_Id = " + ProductId + "";

    //    DataTable dt = new DataTable();
    //    if (ViewState["TempProductTax_SO"] == null)
    //        dt = TemporaryProductWiseTaxes();
    //    else
    //        dt = (DataTable)ViewState["TempProductTax_SO"];

    //    DataView view = new DataView(dt);
    //    view.RowFilter = "Product_Id = " + ProductId + "";

    //    DataTable dtTax = view.ToTable();
    //    if (dtTax.Rows.Count > 0)
    //    {
    //        gvTaxCalculation.DataSource = dtTax;
    //        gvTaxCalculation.DataBind();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
    //    }
    //    else
    //    {
    //        DisplayMessage("No Tax Details found");
    //        return;
    //    }
    //}


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
    }

    //public double Tax_Per_Calculation(string Amount, string ProductId)
    //{
    //    try
    //    {
    //        string TaxQuery = string.Empty;
    //        bool IsTax = Inventory_Common.IsSalesTaxEnabled();
    //        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
    //        double TotalTax = 0;
    //        if (IsTax && double.Parse(Amount) > 0)
    //        {
    //            Get_Tax_Parameter();
    //            String Condition = string.Empty;
    //            if (Hdn_Tax_By.Value == Resources.Attendance.Company)
    //                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
    //            else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
    //                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";



    //            if (ddlTransType.SelectedIndex > 0)
    //                TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //                        where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
    //            //Comment by ghanshyam suthar on 12-03-2018
    //            //else
    //            //    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //            //            where IPTM.Product_Id = " + ProductId + "" + Condition + "";

    //            DataTable dtTax = objda.return_DataTable(TaxQuery);
    //            double TotalPriceBeforeDiscount = double.Parse(Amount);

    //            DataTable dt = new DataTable();
    //            if (ViewState["TempProductTax_SO"] == null)
    //                dt = TemporaryProductWiseTaxes();
    //            else
    //                dt = (DataTable)ViewState["TempProductTax_SO"];

    //            if (dtTax.Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in dtTax.Rows)
    //                {
    //                    double taxvalue = double.Parse(dr["Tax_Percentage"].ToString());
    //                    double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;

    //                    DataRow Newdr = dt.NewRow();
    //                    Newdr["Product_Id"] = ProductId;
    //                    Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
    //                    Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
    //                    Newdr["Tax_Value"] = dr["Tax_Percentage"].ToString();
    //                    Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                    //Newdr["TaxAmount"] = taxamount.ToString();
    //                    Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();

    //                    DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
    //                    if (SRow.Length == 0)
    //                    {
    //                        dt.Rows.Add(Newdr);
    //                    }
    //                    else
    //                    {
    //                        taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
    //                        taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
    //                        //string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
    //                        SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                        SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
    //                    }
    //                    TotalTax = TotalTax + taxvalue;
    //                }
    //                ViewState["TempProductTax_SO"] = dt;
    //            }
    //        }
    //        double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
    //        return TotalTax;
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}



    //public double Tax_Value_Calculation(string Amount, string ProductId)
    //{
    //    string TaxQuery = string.Empty;
    //    bool IsTax = Inventory_Common.IsSalesTaxEnabled();
    //    Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
    //    double TotalTax = 0;
    //    if (IsTax && double.Parse(Amount) > 0)
    //    {
    //        Get_Tax_Parameter();
    //        String Condition = string.Empty;
    //        if (Hdn_Tax_By.Value == Resources.Attendance.Company)
    //            Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
    //        else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
    //            Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";



    //        if (ddlTransType.SelectedIndex > 0)
    //            TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //                        where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
    //        //Comment by ghanshyam suthar on 12-03-2018
    //        //else
    //        //    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //        //                where IPTM.Product_Id = " + ProductId + "" + Condition + "";

    //        DataTable dtTax = objda.return_DataTable(TaxQuery);
    //        double TotalPriceBeforeDiscount = double.Parse(Amount);

    //        DataTable dt = new DataTable();
    //        if (ViewState["TempProductTax_SO"] == null)
    //            dt = TemporaryProductWiseTaxes();
    //        else
    //            dt = (DataTable)ViewState["TempProductTax_SO"];

    //        if (dtTax.Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in dtTax.Rows)
    //            {
    //                double taxvalue = double.Parse(dr["Tax_Percentage"].ToString());
    //                double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;

    //                DataRow Newdr = dt.NewRow();
    //                Newdr["Product_Id"] = ProductId;
    //                Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
    //                Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
    //                Newdr["Tax_Value"] = dr["Tax_Percentage"].ToString();
    //                Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                //Newdr["TaxAmount"] = taxamount.ToString();
    //                Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();

    //                DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
    //                if (SRow.Length == 0)
    //                {
    //                    dt.Rows.Add(Newdr);
    //                }
    //                else
    //                {
    //                    taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
    //                    taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
    //                    //string strRoundCr = GetCurrency(Session["CurrencyId"].ToString(), diff.ToString());
    //                    SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                    SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
    //                }
    //                TotalTax = TotalTax + taxamount;
    //            }
    //            ViewState["TempProductTax_SO"] = dt;
    //        }
    //    }
    //    double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
    //    return TotalTax;
    //}
    //protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}


    public void TaxCalculationWithDiscount()
    {
        foreach (GridViewRow gvRow in GvQuotationDetail.Rows)
        {
            HiddenField lblgvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            TextBox txtUnitCost = (TextBox)gvRow.FindControl("txtgvUnitPrice");
            TextBox Qty_Good_Received = (TextBox)gvRow.FindControl("lblgvQuantity");
            TextBox TxtgvDiscount = (TextBox)gvRow.FindControl("txtgvDiscountP");
            TextBox TxtgvDiscountValue = (TextBox)gvRow.FindControl("txtgvDiscountV");
            TextBox TxtgvTax = (TextBox)gvRow.FindControl("txtgvTaxP");
            TextBox TxtgvTaxValue = (TextBox)gvRow.FindControl("txtgvTaxV");

            Label lblQtyAmmount = (Label)gvRow.FindControl("lblQtyAmmount");

            Label lblgvSerialNo = (Label)gvRow.FindControl("lblgvSerialNo");
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            if (Qty_Good_Received.Text == "")
            {
                Qty_Good_Received.Text = "0";
            }
            double PriceValue = double.Parse(txtUnitCost.Text);
            double QtyValue = double.Parse(Qty_Good_Received.Text);
            double AmountValue = PriceValue * QtyValue;
            TxtgvDiscount.Text = TxtgvDiscount.Text == "" ? "0" : TxtgvDiscount.Text;
            double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
            bool IsValidDiscount = IsApplyDiscount();

            //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            //    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();

            if (!IsValidDiscount)
                AmntAfterDiscnt = AmountValue;

            double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Value, lblgvSerialNo.Text);
            TotalTax = TotalTax / QtyValue;
            string[] strcalc = Common.TaxDiscountCaluculation(txtUnitCost.Text, Qty_Good_Received.Text, "", TotalTax.ToString(), "", TxtgvDiscountValue.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());

            if (Qty_Good_Received.Text != "0.00" && Qty_Good_Received.Text != "" && Qty_Good_Received.Text != "0")
                TxtgvTaxValue.Text = TotalTax.ToString();
            else
                TxtgvTaxValue.Text = "0.00";

            GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
            //string PriceAfterDiscount = (Convert.ToDouble(txtUnitCost.Text) - Convert.ToDouble(TxtgvDiscountValue.Text)).ToString();
            //childGridCalculation(GridChild, PriceAfterDiscount);
        }
    }
    //public double TaxCalculation(string Amount, string ProductId)
    //{
    //    string TaxQuery = string.Empty;
    //    bool IsTax = Inventory_Common.IsSalesTaxEnabled();
    //    double TotalTax = 0;
    //    if (IsTax)
    //    {
    //        Get_Tax_Parameter();
    //        String Condition = string.Empty;
    //        if (Hdn_Tax_By.Value == Resources.Attendance.Company)
    //            Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
    //        else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
    //            Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";


    //        if (ddlTransType.SelectedIndex > 0)
    //            TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //                        where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
    //        //Comment by ghanshyam suthar on 12-03-2018
    //        //else
    //        //    TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
    //        //                where IPTM.Product_Id = " + ProductId + "" + Condition + "";

    //        DataTable dtTax = objda.return_DataTable(TaxQuery);
    //        double TotalPriceBeforeDiscount = double.Parse(Amount);

    //        DataTable dt = new DataTable();
    //        if (ViewState["TempProductTax_SO"] == null)
    //            dt = TemporaryProductWiseTaxes();
    //        else
    //            dt = (DataTable)ViewState["TempProductTax_SO"];

    //        if (dtTax.Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in dtTax.Rows)
    //            {
    //                double taxvalue = double.Parse(dr["Tax_Percentage"].ToString());
    //                double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;

    //                DataRow Newdr = dt.NewRow();
    //                Newdr["Product_Id"] = ProductId;
    //                Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
    //                Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
    //                Newdr["Tax_Value"] = dr["Tax_Percentage"].ToString();
    //                Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();

    //                DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
    //                if (SRow.Length == 0)
    //                {
    //                    dt.Rows.Add(Newdr);
    //                }
    //                else
    //                {
    //                    taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
    //                    taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
    //                    SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
    //                    SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
    //                }
    //                TotalTax = TotalTax + taxamount;
    //            }
    //            ViewState["TempProductTax_SO"] = dt;
    //        }
    //    }
    //    double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
    //    return TotalTax;
    //}
    public bool IsApplyDiscount()
    {
        bool IsValid = false;
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                IsValid = false;
            else
                IsValid = true;
        }
        return IsValid;
    }

    public void TaxCalculationWithDiscount_ProductDetail()
    {
        foreach (GridViewRow gvRow in GvProductDetail.Rows)
        {
            HiddenField lblgvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            TextBox Qty_Good_Received = (TextBox)gvRow.FindControl("lblgvQuantity");
            TextBox TxtgvDiscount = (TextBox)gvRow.FindControl("lblgvDiscountP");
            TextBox TxtgvDiscountValue = (TextBox)gvRow.FindControl("lblgvDiscountV");
            TextBox TxtgvTax = (TextBox)gvRow.FindControl("lblgvTaxP");
            TextBox TxtgvTaxValue = (TextBox)gvRow.FindControl("lblgvTaxV");
            Label lblgvNetAmount = (Label)gvRow.FindControl("lblgvTotal");
            TextBox txtUnitCost = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            Label lblgvSerialNo = (Label)gvRow.FindControl("lblgvSerialNo");
            if (txtUnitCost.Text == "")
            {
                txtUnitCost.Text = "0";
            }
            if (Qty_Good_Received.Text == "")
            {
                Qty_Good_Received.Text = "0";
            }
            if (TxtgvDiscount.Text == "")
            {
                TxtgvDiscount.Text = "0";
            }
            double PriceValue = double.Parse(txtUnitCost.Text);
            double QtyValue = double.Parse(Qty_Good_Received.Text);
            double AmountValue = PriceValue * QtyValue;
            double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(TxtgvDiscount.Text)) / 100;
            bool IsValidDiscount = IsApplyDiscount();

            //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            //    TxtgvDiscountValue.Text = (AmountValue - AmntAfterDiscnt).ToString();

            if (!IsValidDiscount)
                AmntAfterDiscnt = AmountValue;

            double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), lblgvProductId.Value, lblgvSerialNo.Text);
            TotalTax = TotalTax / QtyValue;
            string[] strcalc = Common.TaxDiscountCaluculation(txtUnitCost.Text, Qty_Good_Received.Text, "", TotalTax.ToString(), "", TxtgvDiscountValue.Text, true, StrCurrencyId, false, Session["DBConnection"].ToString());
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

        }
    }
    //protected void BtnAddTax_Product_Command(object sender, CommandEventArgs e)
    //{

    //}

    public string Get_Total(string Qty, string Tax)
    {
        string Total = string.Empty;
        double Tax_Amt = 0;
        double Qty_Value = 0;
        if (Qty != "")
            Qty_Value = Convert.ToDouble(Qty);
        if (Tax != "")
            Tax_Amt = Convert.ToDouble(Tax);
        return Total;
    }

    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_SO"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SO"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {

                string F_Serial_No = string.Empty;

                if (e.CommandName.ToString() == "GvProductDetail")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "GvQuotationDetail")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + F_Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                //Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxCalculation.DataSource = Dt_Cal;
                    gvTaxCalculation.DataBind();
                    int Row_Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
                    string Grid_Name = e.CommandName.ToString();
                    if (Grid_Name == "GvProductDetail")
                    {
                        TextBox Unit_Price = (TextBox)GvProductDetail.Rows[Row_Index].FindControl("lblgvUnitPrice");
                        TextBox Discount_Price = (TextBox)GvProductDetail.Rows[Row_Index].FindControl("lblgvDiscountV");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    if (Grid_Name == "GvQuotationDetail")
                    {
                        TextBox Unit_Price = (TextBox)GvQuotationDetail.Rows[Row_Index].FindControl("txtgvUnitPrice");
                        TextBox Discount_Price = (TextBox)GvQuotationDetail.Rows[Row_Index].FindControl("txtgvDiscountV");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    Hdn_Serial_No_Tax.Value = F_Serial_No;
                    Hdn_Product_Id_Tax.Value = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                    Update_Modal_GST.Update();
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
            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (editid.Value != "")
                    {
                        DataTable DT_Db_Details = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, editid.Value);
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + editid.Value + "' and TRD.Ref_Type='SO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                            DataTable Dt_Inv_TaxRefDetail = objda.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_SO"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_SO"] = Dt_Temp;
                            }
                        }
                    }
                    else if (ddlOrderType.SelectedValue == "Q" && txtTransFrom.Text == "Sales Quotation" && txtQuotationNo.Text != "" && HDN_Sales_Quotation_ID.Value != "")
                    {
                        DataTable DT_Db_Details = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(StrCompId, StrBrandId, StrLocationId, HDN_Sales_Quotation_ID.Value, Session["FinanceYearId"].ToString());
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesQuotationDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + HDN_Sales_Quotation_ID.Value + "' and TRD.Ref_Type='SQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                            DataTable Dt_Inv_TaxRefDetail = objda.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_SO"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_SO"] = Dt_Temp;
                            }
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
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_SO"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_SO"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
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
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_SO"] != null)
            {
                double Tax_Value = Get_Tax_Percentage(ProductId, Serial_No);
                double Temp_Amount = Convert.ToDouble(Amount);
                TotalTax_Amount = Convert.ToDouble(Convert_Into_DF(((Tax_Value * Temp_Amount) / 100).ToString()));
            }
        }
        return TotalTax_Amount;
    }
    public void Add_Tax_In_Session(string Amount, string ProductId, string Serial_No)
    {
        string TaxQuery = string.Empty;
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
        if (IsTax && double.Parse(Amount) > 0)
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

                DataTable dtTax = objda.return_DataTable(TaxQuery);
                double TotalPriceBeforeDiscount = double.Parse(Amount);

                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_SO"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_SO"];

                if (dtTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                        double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;

                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = ProductId;
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Value"].ToString();
                        Newdr["TaxAmount"] = Convert_Into_DF(taxamount.ToString()).ToString();
                        Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                        Newdr["Serial_No"] = Serial_No;
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                        else
                        {
                            taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
                            taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                            //SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["TaxAmount"] = Convert_Into_DF(taxamount.ToString()).ToString();
                            SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Serial_No"] = Serial_No;
                        }
                    }
                    Session["Temp_Product_Tax_SO"] = dt;
                }
            }
        }
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
                Decimal_Count = cmn.Get_Decimal_Count_By_Location(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
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
    protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Temp_Product_Tax_SO"] = null;
        //foreach (DataListItem dl in dlProductDetail.Items)
        //{
        //    TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
        //    TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
        //    txtTaxP.Text = "0.00";
        //    txtTaxV.Text = "0.00";
        //}
    }
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        string Serial_No = Hdn_Serial_No_Tax.Value;
        string Product_ID = Hdn_Product_Id_Tax.Value;
        string Unit_Price = Hdn_unit_Price_Tax.Value;
        string Unit_Discount = Hdn_Discount_Tax.Value;
        string Net_Unit_Price = (Convert.ToDouble(Unit_Price) - Convert.ToDouble(Unit_Discount)).ToString();
        if (Session["Temp_Product_Tax_SO"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SO"] as DataTable;
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
                        //if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value)
                        if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value && DR_Tax["Serial_No"].ToString() == Serial_No)
                        {
                            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                            DR_Tax["TaxAmount"] = (Convert.ToDouble(Net_Unit_Price) * Convert.ToDouble(Tax_Percentage.Text)) / 100;
                            DR_Tax["Amount"] = Net_Unit_Price;
                        }
                    }
                }
            }
            Session["Temp_Product_Tax_SO"] = Dt_Cal;
            if (ddlOrderType.SelectedValue.ToString() == "D")
            {
                foreach (GridViewRow dl in GvProductDetail.Rows)
                {
                    if (Dt_Cal.Rows.Count > 0)
                    {
                        //DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                                TextBox Tax_Percent = (TextBox)dl.FindControl("lblgvTaxP");
                                HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                                Label lblgvSerialNo = (Label)dl.FindControl("lblgvSerialNo");
                                if (Product_ID == hdnProductId.Value && Serial_No == lblgvSerialNo.Text)
                                {
                                    Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                }
                            }
                        }
                    }
                }
                Grid_Calculation("GvProductDetail", "Add Product");
                GetGridTotal();
                //Calculate_Direct();
            }
            else if (ddlOrderType.SelectedValue.ToString() == "Q")
            {
                foreach (GridViewRow dl in GvQuotationDetail.Rows)
                {
                    if (Dt_Cal.Rows.Count > 0)
                    {
                        //DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                                TextBox Tax_Percent = (TextBox)dl.FindControl("txtgvTaxP");
                                HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                                Label lblgvSerialNo = (Label)dl.FindControl("lblgvSerialNo");
                                if (Product_ID == hdnProductId.Value && Serial_No == lblgvSerialNo.Text)
                                {
                                    Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                }
                            }
                        }
                    }
                }
                Grid_Calculation("GvQuotationDetail", "Get Product Tax");
                Calculate_Quotation();
            }
            Hdn_Product_Id_Tax.Value = "";
            Hdn_Serial_No_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row, ref SqlTransaction trns)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("SO", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string R_Serial_No = string.Empty;
        string Grid = string.Empty;
        if (Grid_Name == "GvProductDetail")
        {
            Grid = "GvProductDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("lblgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblgvDiscountV");
            Label lblSerialNO = (Label)Gv_Row.FindControl("lblgvSerialNo");

            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = lblSerialNO.Text;
        }
        if (Grid_Name == "GvQuotationDetail")
        {
            Grid = "GvQuotationDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("lblgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("txtgvDiscountV");
            Label lblSerialNO = (Label)Gv_Row.FindControl("lblgvSerialNo");

            R_Product_ID = Product_ID.Value;
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
            if (Session["Temp_Product_Tax_SO"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_SO"];
            string ProductId = string.Empty;
            string TaxId = string.Empty;
            string TaxValue = string.Empty;
            string TaxAmount = string.Empty;
            string Amount = string.Empty;
            if (ProductTax != null && ProductTax.Rows.Count > 0)
            {
                foreach (DataRow dr in ProductTax.Rows)
                {
                    //if (dr["Product_Id"].ToString() == R_Product_ID)
                    if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                    {
                        ProductId = dr["Product_Id"].ToString();
                        TaxId = dr["Tax_Id"].ToString();
                        TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                        TaxAmount = dr["TaxAmount"].ToString();
                        Amount = Net_Amount.ToString();
                        //if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("SO", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("SO", PQ_Header_ID.ToString(), Detail_ID.ToString());
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string R_Serial_No = string.Empty;

        string Grid = string.Empty;
        if (Grid_Name == "GvProductDetail")
        {
            Grid = "GvProductDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("lblgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblgvDiscountV");
            Label lblSerialNO = (Label)Gv_Row.FindControl("lblgvSerialNo");

            R_Product_ID = Product_ID.Value;
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
            if (Session["Temp_Product_Tax_SO"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_SO"];
            string ProductId = string.Empty;
            string TaxId = string.Empty;
            string TaxValue = string.Empty;
            string TaxAmount = string.Empty;
            string Amount = string.Empty;
            if (ProductTax != null && ProductTax.Rows.Count > 0)
            {
                foreach (DataRow dr in ProductTax.Rows)
                {
                    //if (dr["Product_Id"].ToString() == R_Product_ID)
                    if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                    {
                        ProductId = dr["Product_Id"].ToString();
                        TaxId = dr["Tax_Id"].ToString();
                        TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                        TaxAmount = dr["TaxAmount"].ToString();
                        Amount = Net_Amount.ToString();
                        //if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("SO", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
    }
    public double Get_Discount_Percentage(string Unit_Price, string Discount_Amount)
    {
        try
        {
            double Discount_Percent = 0;
            if (IsApplyDiscount() == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Amount = Convert.ToDouble(Discount_Amount);
                Discount_Percent = (D_Discount_Amount / D_Unit_Price) * 100;
            }
            return Discount_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Total_Tax_Percentage(string Unit_Price, string Tax_Amount)
    {
        try
        {
            double Tax_Percent = 0;
            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Tax_Amount = Convert.ToDouble(Tax_Amount);
                Tax_Percent = (D_Tax_Amount / D_Unit_Price) * 100;
            }
            return Tax_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Discount_Amount(string Unit_Price, string Discount_Percent)
    {
        try
        {
            double Discount_Amount = 0;
            if (IsApplyDiscount() == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
                Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            }
            return Discount_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Net_Amount(string Unit_Price, string Discount_Percent, string Product_Id, string Serial_No)
    {
        try
        {
            double Net_Amount = 0;
            double D_Unit_Price = Convert.ToDouble(Unit_Price);
            double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
            double Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            double Tax_Amount = Get_Tax_Amount((D_Unit_Price - Discount_Amount).ToString(), Product_Id, Serial_No);
            Net_Amount = (D_Unit_Price - Discount_Amount) + Tax_Amount;
            return Net_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public void Grid_Calculation(string Grid_Name, string Event_name)
    {
        if (Grid_Name == "GvProductDetail" && Event_name == "Add Product")
        {
            foreach (GridViewRow GVR_Row in GvProductDetail.Rows)
            {
                Label Product_Code = (Label)GVR_Row.FindControl("lblgvProductCode");
                HiddenField Product_Id = (HiddenField)GVR_Row.FindControl("hdngvProductId");
                HiddenField Unit_Id = (HiddenField)GVR_Row.FindControl("hdngvUnitId");
                Label Unit_Name = (Label)GVR_Row.FindControl("lblgvUnit");
                TextBox Quantity = (TextBox)GVR_Row.FindControl("lblgvQuantity");
                TextBox Free_Quantity = (TextBox)GVR_Row.FindControl("lblgvFreeQuantity");
                Label Remain_Quantity = (Label)GVR_Row.FindControl("lblgvRemainQuantity");
                TextBox Unit_Price = (TextBox)GVR_Row.FindControl("lblgvUnitPrice");
                Label Total_Quantity_Price = (Label)GVR_Row.FindControl("lblgvQuantityPrice");
                TextBox Discount_Percentage = (TextBox)GVR_Row.FindControl("lblgvDiscountP");
                TextBox Discount_Amount = (TextBox)GVR_Row.FindControl("lblgvDiscountV");
                Label Price_After_Discount = (Label)GVR_Row.FindControl("lblgvPriceAfterDiscount");
                TextBox Tax_Percentage = (TextBox)GVR_Row.FindControl("lblgvTaxP");
                TextBox Tax_Amount = (TextBox)GVR_Row.FindControl("lblgvTaxV");
                Label Amount_After_Tax = (Label)GVR_Row.FindControl("lblgvPriceAfterTax");
                Label Line_Total = (Label)GVR_Row.FindControl("lblgvTotal");
                TextBox Agent_Commission = (TextBox)GVR_Row.FindControl("txtgvAgentCommission");

                Label lblgvSerialNo = (Label)GVR_Row.FindControl("lblgvSerialNo");

                double F_Unit_Price = double.Parse(Unit_Price.Text);
                double F_Order_Quantity = double.Parse(Quantity.Text);
                double F_Discount_Percentage = double.Parse(Discount_Percentage.Text);
                double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                Add_Tax_In_Session((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
                double F_Tax_Percentage = Get_Tax_Percentage(Product_Id.Value, lblgvSerialNo.Text);
                double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
                double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;

                Discount_Percentage.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                Discount_Amount.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                Tax_Percentage.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                Tax_Amount.Text = Convert_Into_DF(F_Tax_Amount.ToString()).ToString();
                Line_Total.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                Total_Quantity_Price.Text = (F_Unit_Price * F_Order_Quantity).ToString();
            }
            TaxCalculationWithDiscount();
            setDecimal();
        }
        else if (Grid_Name == "GvQuotationDetail" && Event_name == "Get Product Tax")
        {
            foreach (GridViewRow GVR_Row in GvQuotationDetail.Rows)
            {
                Label Product_Code = (Label)GVR_Row.FindControl("lblgvProductCode");
                HiddenField Product_Id = (HiddenField)GVR_Row.FindControl("hdngvProductId");
                //HiddenField Unit_Id = (HiddenField)GVR_Row.FindControl("hdngvUnitId");
                //Label Unit_Name = (Label)GVR_Row.FindControl("lblgvUnit");
                TextBox Quantity = (TextBox)GVR_Row.FindControl("lblgvQuantity");
                TextBox Free_Quantity = (TextBox)GVR_Row.FindControl("txtgvFreeQuantity");
                Label Remain_Quantity = (Label)GVR_Row.FindControl("txtgvRemainQuantity");
                TextBox Unit_Price = (TextBox)GVR_Row.FindControl("txtgvUnitPrice");
                TextBox Total_Quantity_Price = (TextBox)GVR_Row.FindControl("txtgvQuantityPrice");
                TextBox Discount_Percentage = (TextBox)GVR_Row.FindControl("txtgvDiscountP");
                TextBox Discount_Amount = (TextBox)GVR_Row.FindControl("txtgvDiscountV");
                TextBox Price_After_Discount = (TextBox)GVR_Row.FindControl("txtgvPriceAfterDiscount");
                TextBox Tax_Percentage = (TextBox)GVR_Row.FindControl("txtgvTaxP");
                TextBox Tax_Amount = (TextBox)GVR_Row.FindControl("txtgvTaxV");
                TextBox Amount_After_Tax = (TextBox)GVR_Row.FindControl("txtgvPriceAfterTax");
                TextBox Line_Total = (TextBox)GVR_Row.FindControl("txtgvTotal");
                TextBox Agent_Commission = (TextBox)GVR_Row.FindControl("txtgvAgentCommission");

                Label lblgvSerialNo = (Label)GVR_Row.FindControl("lblgvSerialNo");

                //if (Unit_Price.Text == "")
                //{
                //    DisplayMessage("Cant Convert this Quotation Into Order due to Some Improper Data Please Check The Quotation");
                //    Reset();
                //    return;
                //}
                Unit_Price.Text = Unit_Price.Text == "" ? "0" : Unit_Price.Text;
                Discount_Percentage.Text = Discount_Percentage.Text == "" ? "0" : Discount_Percentage.Text;
                double F_Unit_Price = double.Parse(Unit_Price.Text);
                double F_Order_Quantity = double.Parse(Quantity.Text);
                double F_Discount_Percentage = double.Parse(Discount_Percentage.Text);
                double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                Add_Tax_In_Session((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
                double F_Tax_Percentage = Get_Tax_Percentage(Product_Id.Value, lblgvSerialNo.Text);
                double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
                double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;

                Discount_Percentage.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                Discount_Amount.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                Tax_Percentage.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                Tax_Amount.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                Line_Total.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                Total_Quantity_Price.Text = (F_Unit_Price * F_Order_Quantity).ToString();
            }
            TaxCalculationWithDiscount();
            setDecimal();
        }
    }


    #region Amazonwebservice






    //------------------------------------------------------------------------












    public int GetAmazonSalesOrder()
    {


        DataTable dt = new DataTable();
        DataTable dtOrderInfo = new DataTable();
        DataTable dtOrdershipping = new DataTable();
        DataTable dtOrderTotal = new DataTable();
        Set_AddressChild objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        Ems_ContactCompanyBrand ObjCompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());
        Ems_Contact_Group objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());

        DataSet ds = ConvertXMLToDataSet(MarketplaceWebServiceOrdersSample.getData("1", ""));


        dtOrderInfo = ds.Tables[2];




        // and OrderStatus<>'Pending'
        dtOrderInfo = new DataView(dtOrderInfo, "OrderStatus<>'Canceled' and OrderStatus<>'Pending'", "PurchaseDate Desc", DataViewRowState.CurrentRows).ToTable();
        //dtOrderInfo = new DataView(dtOrderInfo, "AmazonOrderId='407-9642935-7786731'", "", DataViewRowState.CurrentRows).ToTable();




        dtOrderTotal = ds.Tables[4];

        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtContactMaster = objContact.GetContactAllData();
        DataTable dtAddressdetailMaster = objAddMaster.GetAddressAllData(Session["CompId"].ToString());
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        string strProductIdList = string.Empty;
        int OrderCounter = 0;

        try
        {

            foreach (DataRow dr in dtOrderInfo.Rows)
            {


                if (new DataView(objSOrderHeader.GetSOHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns), "Field3='" + dr[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    continue;
                }


                dtOrdershipping = ds.Tables[3];

                dtOrderTotal = ds.Tables[4];

                DataTable dtcurrency = objCurrency.GetCurrencyMaster(ref trns);
                string OrderCurrencyCode = string.Empty;

                try
                {
                    OrderCurrencyCode = new DataView(dtOrderTotal, "Order_Id=" + dr["Order_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["CurrencyCode"].ToString();

                }
                catch
                {
                    OrderCurrencyCode = "INR";
                }

                dtcurrency = new DataView(dtcurrency, "Currency_Code='" + OrderCurrencyCode + "'", "", DataViewRowState.CurrentRows).ToTable();




                //first we insert address if not found in database 




                dtOrdershipping = new DataView(dtOrdershipping, "Order_Id=" + dr["Order_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                int addressId = 0;
                string strcontactNumber = string.Empty;

                try
                {



                    if (dtOrdershipping.Rows[0]["Phone"].ToString() != "")
                    {

                        strcontactNumber = "+" + dtcurrency.Rows[0]["Country_Code"].ToString() + "-" + dtOrdershipping.Rows[0]["Phone"].ToString();

                        //we check by contact number

                        DataTable dtAddressdetail = new DataView(dtAddressdetailMaster, "PhoneNo1='" + strcontactNumber + "' or PhoneNo2='" + strcontactNumber + "' or MobileNo1='" + strcontactNumber + "' or MobileNo2='" + strcontactNumber + "'", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtAddressdetail.Rows.Count == 0)
                        {

                            try
                            {
                                addressId = objAddMaster.InsertAddressMaster("1", dtOrdershipping.Rows[0]["Name"].ToString(), dtOrdershipping.Rows[0]["AddressLine1"].ToString() + " , " + dtOrdershipping.Rows[0]["AddressLine2"].ToString(), "", "", "", dtcurrency.Rows[0]["Country_Id"].ToString(), dtOrdershipping.Rows[0]["Stateorregion"].ToString(), dtOrdershipping.Rows[0]["City"].ToString(), dtOrdershipping.Rows[0]["PostalCode"].ToString(), strcontactNumber, "", "", "", dr["BuyerEmail"].ToString(), "", "", "", "0.000000", "0.000000", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            }
                            catch
                            {
                                addressId = objAddMaster.InsertAddressMaster("1", dtOrdershipping.Rows[0]["Name"].ToString(), dtOrdershipping.Rows[0]["AddressLine1"].ToString(), "", "", "", dtcurrency.Rows[0]["Country_Id"].ToString(), dtOrdershipping.Rows[0]["Stateorregion"].ToString(), dtOrdershipping.Rows[0]["City"].ToString(), dtOrdershipping.Rows[0]["PostalCode"].ToString(), strcontactNumber, "", "", "", dr["BuyerEmail"].ToString(), "", "", "", "0.000000", "0.000000", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            }

                        }
                        else
                        {
                            addressId = Convert.ToInt32(dtAddressdetail.Rows[0]["Trans_Id"].ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            addressId = objAddMaster.InsertAddressMaster("1", dtOrdershipping.Rows[0]["Name"].ToString(), dtOrdershipping.Rows[0]["AddressLine1"].ToString() + " , " + dtOrdershipping.Rows[0]["AddressLine2"].ToString(), "", "", "", dtcurrency.Rows[0]["Country_Id"].ToString(), dtOrdershipping.Rows[0]["Stateorregion"].ToString(), dtOrdershipping.Rows[0]["City"].ToString(), dtOrdershipping.Rows[0]["PostalCode"].ToString(), strcontactNumber, "", "", "", dr["BuyerEmail"].ToString(), "", "", "", "0.000000", "0.000000", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                        catch
                        {
                            addressId = objAddMaster.InsertAddressMaster("1", dtOrdershipping.Rows[0]["Name"].ToString(), dtOrdershipping.Rows[0]["AddressLine1"].ToString(), "", "", "", dtcurrency.Rows[0]["Country_Id"].ToString(), dtOrdershipping.Rows[0]["Stateorregion"].ToString(), dtOrdershipping.Rows[0]["City"].ToString(), dtOrdershipping.Rows[0]["PostalCode"].ToString(), strcontactNumber, "", "", "", dr["BuyerEmail"].ToString(), "", "", "", "0.000000", "0.000000", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                    }
                }
                catch
                {
                    addressId = 0;

                }

                //check customer exist or not otherwise create new contact 
                //check by contact number



                if (strcontactNumber == "")
                {
                    strcontactNumber = "0";
                }

                int contactId = 0;


                if (dr["BuyerEmail"].ToString().Trim() != "")
                {
                    DataTable dtcontact = new DataView(dtContactMaster, "Field1='" + dr["BuyerEmail"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



                    string Id = string.Empty;

                    if (dtcontact.Rows.Count == 0)
                    {
                        contactId = objContact.InsertContactMaster("", dr["BuyerName"].ToString(), dr["BuyerName"].ToString(), "", "0", "0", "0", "0", false.ToString(), false.ToString(), "Company", true.ToString(), dr["BuyerEmail"].ToString(), strcontactNumber, "", dtcurrency.Rows[0]["Country_Id"].ToString(), "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0", ref trns);


                        objContact.UpdateContactMaster(contactId.ToString(), objDocNo.GetDocumentNo(true, "0", false, "8", "19", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + contactId.ToString(), ref trns);

                        if (addressId != 0)
                        {

                            objAddChild.InsertAddressChild(addressId.ToString(), "Contact", contactId.ToString(), true.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        }
                        objCustomer.InsertCustomerMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), contactId.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", false.ToString(), "", "", "", "", "False", "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "0", "", ref trns);
                        ObjCompanyBrand.InsertContactCompanyBrand(contactId.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);


                        objCG.InsertContactGroup(contactId.ToString(), "1", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), objDocNo.GetDocumentNo(true, "0", false, "8", "18", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + contactId.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);

                    }
                    else
                    {
                        contactId = Convert.ToInt32(dtcontact.Rows[0]["Trans_Id"].ToString());
                    }
                }
                else
                {

                    contactId = objContact.InsertContactMaster("", dr["BuyerName"].ToString(), dr["BuyerName"].ToString(), "", "0", "0", "0", "0", false.ToString(), false.ToString(), "Company", true.ToString(), dr["BuyerEmail"].ToString(), strcontactNumber, "", dtcurrency.Rows[0]["Country_Id"].ToString(), "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0", ref trns);


                    objContact.UpdateContactMaster(contactId.ToString(), objDocNo.GetDocumentNo(true, "0", false, "8", "19", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + contactId.ToString(), ref trns);

                    if (addressId != 0)
                    {

                        objAddChild.InsertAddressChild(addressId.ToString(), "Contact", contactId.ToString(), true.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                    }
                    objCustomer.InsertCustomerMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), contactId.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", false.ToString(), "", "", "", "", "False", "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "0", "", ref trns);
                    ObjCompanyBrand.InsertContactCompanyBrand(contactId.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);


                    objCG.InsertContactGroup(contactId.ToString(), "1", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), objDocNo.GetDocumentNo(true, "0", false, "8", "18", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + contactId.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);

                }

                //insert record in customer master and contact group for create customer


                DateTime dtOrderdate = new DateTime(Convert.ToInt32(dr["PurchaseDate"].ToString().Split('/')[2].ToString().Split(' ')[0].ToString()), Convert.ToInt32(dr["PurchaseDate"].ToString().Split('/')[0].ToString()), Convert.ToInt32(dr["PurchaseDate"].ToString().Split('/')[1].ToString()));
                DateTime LatestDeliverydate = new DateTime(Convert.ToInt32(dr["LatestDeliverydate"].ToString().Split('/')[2].ToString().Split(' ')[0].ToString()), Convert.ToInt32(dr["LatestDeliverydate"].ToString().Split('/')[0].ToString()), Convert.ToInt32(dr["LatestDeliverydate"].ToString().Split('/')[1].ToString()));

                int b = objSOrderHeader.InsertSOHeader(StrCompId, StrBrandId, StrLocationId, txtSONo.Text, dtOrderdate.ToString(), "0", LatestDeliverydate.ToString(),/*(txtTransFrom.Text)  Replace To =>*/"D", "0", contactId.ToString(), addressId.ToString(), "0", "0", "0", "0", "0", "", "0", false.ToString(), false.ToString(), dtcurrency.Rows[0]["Currency_Id"].ToString(), "0", addressId.ToString(), contactId.ToString(), dr["AmazonOrderId"].ToString(), "Approved", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);

                OrderCounter++;


                DataTable dtCount = objSOrderHeader.GetSOHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                if (dtCount.Rows.Count == 0)
                {
                    objSOrderHeader.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + "1", ref trns);

                }
                else
                {
                    objSOrderHeader.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + dtCount.Rows.Count, ref trns);

                }
                //insert record in detail table 

                DataSet dsdetail = ConvertXMLToDataSet(MarketplaceWebServiceOrdersSample.getData("2", dr["AmazonOrderId"].ToString()));


                DataTable dtdetail = dsdetail.Tables[2];

                int counter = 0;
                double Netamount = 0;
                double shippingprice = 0;
                double ItemUnitPrice = 0;

                double ItemPriceForshipping = 0;


                foreach (DataRow drDetail in dtdetail.Rows)
                {

                    DataTable dtdetailItemprice = dsdetail.Tables[3];
                    DataTable dtdetailshippingprice = dsdetail.Tables[4];


                    dtdetailItemprice = new DataView(dtdetailItemprice, "OrderItem_Id=" + drDetail["OrderItem_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    dtdetailshippingprice = new DataView(dtdetailshippingprice, "OrderItem_Id=" + drDetail["OrderItem_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    ItemUnitPrice += (((Convert.ToDouble(dtdetailItemprice.Rows[0]["Amount"].ToString()) / 105.5) * 100) / Convert.ToDouble(drDetail["QuantityOrdered"].ToString()));


                    ItemPriceForshipping += (Convert.ToDouble(dtdetailItemprice.Rows[0]["Amount"].ToString()) / 105.5) * 100;

                    Netamount += Convert.ToDouble(dtdetailItemprice.Rows[0]["Amount"].ToString());
                    shippingprice += Convert.ToDouble(dtdetailshippingprice.Rows[0]["Amount"].ToString());


                }


                //here insert tax detailk

                double strvatvalue = Convert.ToDouble(GetAmountDecimal(((ItemUnitPrice * 5.50) / 100).ToString()));

                objTaxRefDetail.InsertRecord("SO", b.ToString(), "0", "0", "0", "4", "5.50", strvatvalue.ToString(), false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                double strshiipingpercentage = ((shippingprice * 100) / ItemPriceForshipping);

                strshiipingpercentage = Convert.ToDouble(GetAmountDecimal(strshiipingpercentage.ToString()));

                objTaxRefDetail.InsertRecord("SO", b.ToString(), "0", "0", "0", "3", strshiipingpercentage.ToString(), shippingprice.ToString(), false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                string strsql = "update Inv_SalesOrderHeader set Taxp=" + (5.5 + strshiipingpercentage).ToString() + ",Taxv=" + (strvatvalue + shippingprice).ToString() + ", NetAmount=" + (Netamount + shippingprice).ToString() + " where Trans_Id=" + b.ToString() + "";
                objda.execute_Command(strsql, ref trns);

                //for update tax info in  detail level

                foreach (DataRow drDetail in dtdetail.Rows)
                {
                    string strProductId = string.Empty;

                    string strUnitId = string.Empty;




                    DataTable dtProductInfo = new DataView(dtProductMaster, "AlternateId3='" + drDetail[1].ToString().Trim() + "' or ProductCode='" + drDetail[1].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();


                    DataTable dtdetailItemprice = dsdetail.Tables[3];
                    DataTable dtdetailshippingprice = dsdetail.Tables[4];

                    counter++;

                    dtdetailshippingprice = new DataView(dtdetailshippingprice, "OrderItem_Id=" + drDetail["OrderItem_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                    dtdetailItemprice = new DataView(dtdetailItemprice, "OrderItem_Id=" + drDetail["OrderItem_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                    string unitPrice = GetAmountDecimal((((Convert.ToDouble(dtdetailItemprice.Rows[0]["Amount"].ToString()) / 105.5) * 100) / Convert.ToDouble(drDetail["QuantityOrdered"].ToString())).ToString());

                    string strtaxV = GetAmountDecimal(((Convert.ToDouble(unitPrice) * (5.5 + strshiipingpercentage)) / 100).ToString());

                    ObjSOrderDetail.InsertSODetail(StrCompId, StrBrandId, StrLocationId, b.ToString(), counter.ToString(), dtProductInfo.Rows[0]["ProductId"].ToString(), drDetail["QuantityOrdered"].ToString(), "0", "0", dtProductInfo.Rows[0]["UnitId"].ToString(), unitPrice, (5.5 + strshiipingpercentage).ToString(), strtaxV, "0", "0", "0", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            //End Approval Code
        }
        catch (Exception ex)
        {

            if (ex.Message == null)
            {
                DisplayMessage("Error Found , Transaction has been rollback");
            }
            else
            {

                DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            }

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();

        }

        return OrderCounter;
    }





    //------------------------------------------------------------------------
    public DataSet ConvertXMLToDataSet(string xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData);
            // Load the XmlTextReader from the stream
            reader = new XmlTextReader(stream);
            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }
    protected void btnGetOrder_Click(object sender, EventArgs e)
    {

        //we are got two new sales order


        int NewOrder = GetAmazonSalesOrder();

        if (NewOrder > 0)
        {
            DisplayMessage("We have got " + NewOrder.ToString() + " new sales order");

        }
        else
        {
            DisplayMessage("Sales Order not found");

        }

        FillGrid(1);
        //AllPageCode();

    }
    #endregion

    public void Calculate_Direct()
    {
        foreach (GridViewRow gvrow in GvProductDetail.Rows)
        {
            HiddenField hdnProductId = (HiddenField)gvrow.FindControl("hdngvProductId");
            TextBox lblgvQuantity = (TextBox)gvrow.FindControl("lblgvQuantity");
            Label lblgvRemainQuantity = (Label)gvrow.FindControl("lblgvRemainQuantity");
            TextBox lblgvUnitPrice = (TextBox)gvrow.FindControl("lblgvUnitPrice");
            Label lblgvQuantityPrice = (Label)gvrow.FindControl("lblgvQuantityPrice");
            TextBox lblgvDiscountP = (TextBox)gvrow.FindControl("lblgvDiscountP");
            TextBox lblgvDiscountV = (TextBox)gvrow.FindControl("lblgvDiscountV");
            TextBox lblgvTaxP = (TextBox)gvrow.FindControl("lblgvTaxP");
            TextBox lblgvTaxV = (TextBox)gvrow.FindControl("lblgvTaxV");
            Label lblgvTotal = (Label)gvrow.FindControl("lblgvTotal");
            Label lblgvSerialNo = (Label)gvrow.FindControl("lblgvSerialNo");
            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }

            if (lblgvQuantity.Text == "")
            {
                lblgvQuantity.Text = "0";
            }

            double TotalTax = 0;
            if (lblgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
            {
                TotalTax = Get_Tax_Percentage(hdnProductId.Value, lblgvSerialNo.Text);
                double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
            }

            string[] strcal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", lblgvDiscountP.Text, "", true, StrCurrencyId, false, Session["DBConnection"].ToString());


            lblgvDiscountV.Text = strcal[2].ToString();

            GridView GridChild = (GridView)gvrow.FindControl("gvchildGrid");
            string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(lblgvDiscountV.Text)).ToString();
            childGridCalculation(GridChild, PriceAfterDiscount);
            lblgvTaxV.Text = strcal[4].ToString();
            lblgvTotal.Text = strcal[5].ToString();
            GetGridTotal();
            //AllPageCode();
        }
    }

    public void Calculate_Quotation()
    {
        Double fGrossTotal = 0;
        Double fGrossTax = 0;
        Double fGrossDis = 0;
        foreach (GridViewRow gvr in GvQuotationDetail.Rows)
        {
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            TextBox lblgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
            TextBox lblgvQuantityPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
            TextBox lblgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox lblgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            if (lblgvQuantityPrice.Text == "")
            {
                lblgvQuantityPrice.Text = "0";
            }
            if (lblgvTotal.Text != "" && lblgvTotal.Text != "0")
            {
                fGrossTotal = fGrossTotal + (Convert.ToDouble(lblgvQuantityPrice.Text) * Convert.ToDouble(lblgvQuantity.Text));
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
            txtPriceAfterDiscount.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString();
        }
        catch
        {
            txtPriceAfterDiscount.Text = "0";
        }
        setDecimal();
        CalculationchangeIntaxGridview();
    }



    protected void lBtnCustomerInfo_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);

        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(CustomerId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return str;
    }

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/SO", "Sales", "SalesOrder", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmpMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmpMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
            }
        }
        return txt;
    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strEmployeeId = Emp_ID;
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                hdnSalesPersonId.Value = Emp_ID;
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                hdnSalesPersonId.Value = "0";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = string.Empty;

        if (HttpContext.Current.Session["ContactID"] != null)
        {

            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        else
        {
            id = "0";
        }


        DataTable dt = ObjContactMaster.GetContactAsPerFilterText(prefixText, id);

        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }

            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Field2"].ToString() + "/" + dtcon.Rows[i]["Field1"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;
        }
    }


    private string GetContactId()
    {
        string retval = "";
        if (txtContactPerson.Text != "")
        {
            using (DataTable dtSupp = objContact.GetContactByContactName(txtContactPerson.Text.Trim().Split('/')[0].ToString()))
            {
                if (dtSupp.Rows.Count > 0)
                {
                    retval = (txtContactPerson.Text.Split('/'))[txtContactPerson.Text.Split('/').Length - 1];
                }
            }
        }
        return retval;
    }

    protected void txtContactId_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactPerson.Text != "")
        {
            strCustomerId = GetContactId();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                hdnContactPersonId.Value = strCustomerId;
                ddlCurrency.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtContactPerson.Text = "";
                hdnContactPersonId.Value = "0";
                txtContactPerson.Focus();
                return;
            }
        }
        else
        {
            hdnContactPersonId.Value = "0";
        }
    }

    protected void btnNewaddress_Click(object sender, EventArgs e)
    {
        if (Session["ContactID"].ToString() == "" || Session["ContactID"].ToString() == "0")
        {
            DisplayMessage("Please Select a Customer To Add New Address");
            return;
        }
        addaddress.Reset();

        Country_Currency objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());

        ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();

        if (ViewState["Country_Id"] != null)
        {
            addaddress.BtnNew_click(ViewState["Country_Id"].ToString());
            Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
        }

        addaddress.fillGridAdd(Session["ContactID"].ToString());
        addaddress.setCustomerID(Session["ContactID"].ToString());
        addaddress.fillHeader(txtCustomer.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_NewAddress_Open();displayList()", true);
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        return txt;
    }


    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);

        if (City_id != "")
        {
            return City_id;
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
        if (stateId != "")
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
            return stateId;
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }

    protected void setDefaultValueForUcAcMaster()
    {
        string CustomerId = txtCustomer.Text.Split('/')[3].ToString();
        UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", txtCustomer.Text.Split('/')[0].ToString());
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
    {
        // return  1 when 'Address Name Already Exists' and 0 when not present
        Set_AddressMaster AM = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
        if (data == "0")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
            {
                return null;
            }
            CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
            string[] txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["City_Name"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }


    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesOrderCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void GvSq_Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSqCurrentPageIndex.Value = pageIndex.ToString();
        FillRequestGrid(pageIndex);
    }


    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool validateAddress(string strAddress, string AddressId)
    {
        try
        {
            string _result = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).get_SingleValue("select COUNT(Trans_Id) from Set_AddressMaster where Address_Name='" + strAddress + "' and Trans_Id='" + AddressId + "'");
            return _result == "0" ? false : true;
        }
        catch
        {
            return false;
        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUser();
        FillGrid(1);
        if (ddlLocation.SelectedItem.ToString() != "All")
        {
            hdnLocationId.Value = ddlLocation.SelectedValue;
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }

    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            }
        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }
    public void FillUser()
    {
        try
        {
            string strEmpId = string.Empty;
            string strLocationDept = string.Empty;
            string strLocId = Session["LocId"].ToString();

            strLocId = ddlLocation.SelectedValue;
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept);


            DataTable dtEmp = new DataTable();

            string isSingle = ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString());
            bool IsSingleUser = false;
            try
            {
                IsSingleUser = Convert.ToBoolean(isSingle);
            }
            catch
            {
                IsSingleUser = false;
            }


            string sharedSalesPersons = string.Empty;
            if (Session["EmpId"].ToString() != "0" && IsSingleUser == true)
            {
                sharedSalesPersons = objda.get_SingleValue("select top 1 Param_Value from set_employee_parameter_new where Param_Name='SharedSalesPersons' and Company_Id='" + Session["CompId"].ToString() + "' and EmpId='" + Session["EmpId"].ToString() + "' and IsActive='true'");
            }

            // can see multiple employee data
            if (IsSingleUser == false)
            {
                //for normal user
                if (Session["EmpId"].ToString() != "0")
                {
                    dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), strEmpId);
                    //dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    //for super admin
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "emp_name asc", DataViewRowState.CurrentRows).ToTable();
                    }
                }
            }
            else
            {
                string strNewEmpList = Session["EmpId"].ToString();
                if (!string.IsNullOrEmpty(sharedSalesPersons) && sharedSalesPersons != "@NOTFOUND@")
                {
                    strNewEmpList = sharedSalesPersons;
                    strNewEmpList += "," + Session["EmpId"].ToString();
                }

                dtEmp = objEmployee.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "emp_id in(" + strNewEmpList + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (ddlLocation.SelectedIndex > 0)
                {
                    dtEmp = new DataView(dtEmp, "location_id='" + ddlLocation.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                //dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), Session["EmpId"].ToString());
            }

            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_name";
            ddlUser.DataValueField = "user_id";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("--Select User--", "--Select User--"));
        }
        catch
        {

        }
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";
        DataTable dtEmp = objda.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id in (" + strLocationId + ") and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");
        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }
        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;

    }


    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }

        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvSalesOrder, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvSalesOrder, lstCls);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void chkShortProductName_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in GvProductDetail.Rows)
        {
            Label lblDetailName = (Label)gvRow.FindControl("lblgvProductName");
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

    protected void btnSOrderSave_Click(object sender, EventArgs e)
    {
        if (rbtNew.Checked == true)
        {
            txtSONo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtSONo.Text;
            hdnCanEdit.Value = "";
            editid.Value = "";
            //ddlOrderType.SelectedValue = "D";
        }

        //if (ddlTransType.SelectedIndex == 0)
        //{
        //    DisplayMessage("Please Select Transaction Type");
        //    return;
        //}
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        //here we check that this page is updated by other user before save of current user 
        //this code is created by jitendra upadhyay on 02-06-2015
        //code start
        if (editid.Value != "")
        {
            DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtOrderEdit.Rows.Count != 0)
            {
                if (ViewState["TimeStamp"].ToString() != dtOrderEdit.Rows[0]["Row_Lock_Id"].ToString())
                {
                    DisplayMessage("Another User update Information reload and try again");
                    return;
                }
            }
        }
        //code end
        string SalesOrderId = string.Empty;

        if (txtSODate.Text == "")
        {
            DisplayMessage("Enter Sales Order Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSODate);
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtSODate.Text);
            }
            catch
            {
                DisplayMessage("Enter Sales Order Date in format " + Session["DateFormat"].ToString() + "");
                txtSODate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSODate);

                return;
            }
        }



        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        //if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtSODate.Text), "I"))
        //{
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}


        if (txtSONo.Text == "")
        {
            DisplayMessage("Enter Sales Order No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);

            return;
        }
        if (ddlOrderType.SelectedIndex == 0)
        {
            DisplayMessage("Select Order Type");
            ddlOrderType.Focus();

            return;
        }

        if (ddlOrderType.SelectedValue == "Q")//updated by jitendra on 02-12-2013 replace the D with Q
        // in the indirect case the value field is Q of thos dropdown
        {
            if (ddlQuotationNo.Visible == true)
            {
                if (ddlQuotationNo.SelectedValue == "--Select--")
                {
                    DisplayMessage("Select Quotation");
                    ddlQuotationNo.Focus();

                    return;
                }
            }
        }
        else
        {
            if (editid.Value == "")
            {
                DataTable dtSQNo = objSOrderHeader.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtSONo.Text);
                if (dtSQNo.Rows.Count > 0)
                {
                    DisplayMessage("Sales Quotation No. Already Exits");
                    txtSONo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);

                    return;
                }
            }
        }

        if (txtSalesPerson.Text.Trim() == "")
        {
            DisplayMessage("Please Enter Sales Person Name");
            txtSalesPerson.Focus();
            return;
        }
        if (txtCustomer.Text != "")
        {
            if (GetCustomerId(txtCustomer) == "")
            {
                txtCustomer.Text = "";
                DisplayMessage("Customer Choose In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomer);

                return;
            }
        }
        else
        {
            DisplayMessage("Enter Customer Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomer);
            return;
        }

        string strAddressId2 = string.Empty;
        if (txtInvoiceTo.Text != "")
        {
            DataTable dtAddId = objAddMaster.GetAddressDataByAddressName(txtInvoiceTo.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strAddressId2 = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
        }
        else
        {
            strAddressId2 = "0";

        }

        string strPaymentModeId = string.Empty;

        if (ddlPaymentMode.SelectedValue == "--Select--")
        {
            strPaymentModeId = "0";
            //DisplayMessage("Select PaymentMode");
            //ddlPaymentMode.Focus();
            //return;
        }
        else
        {
            strPaymentModeId = ddlPaymentMode.SelectedValue;
        }

        if (txtEstimateDeliveryDate.Text == "")
        {
            DisplayMessage("Enter Estimated Delivery Date");
            txtEstimateDeliveryDate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Estimated Delievery Date Date in format " + Session["DateFormat"].ToString() + "");
                txtEstimateDeliveryDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEstimateDeliveryDate);

                return;
            }
        }


        if (txtContactPerson.Text.Trim() == "")
        {
            DisplayMessage("Enter Contact Person Name");
            txtContactPerson.Focus();
            return;
        }

        if (txtShipCustomerName.Text != "")
        {
            if (GetCustomerId(txtShipCustomerName) == "")
            {

                txtShipCustomerName.Text = "";
                DisplayMessage("Ship To Choose In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipCustomerName);

                return;
            }
        }
        else
        {

        }

        string strAddressId = string.Empty;
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAddId = objAddMaster.GetAddressDataByAddressName(txtShipingAddress.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
        }
        else
        {
            strAddressId = "0";
        }

        if (txtShippingCharge.Text == "")
        {
            txtShippingCharge.Text = "0";
        }
        if (txtNetAmount.Text == "")
        {
            txtNetAmount.Text = "0";
        }
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }

        string CustOrderNo = string.Empty;
        if (txtCustOrderNo.Text != "")
        {
            CustOrderNo = txtCustOrderNo.Text;
        }
        else
        {
            CustOrderNo = "";
        }

        if (ddlOrderType.SelectedValue != "--Select--")
        {
            if (ddlOrderType.SelectedValue == "D")
            {
                if (GvProductDetail.Rows.Count > 0)
                {

                }
                else
                {
                    DisplayMessage("You have no Product For Generate Sales Order");

                    return;
                }
            }
            else if (ddlOrderType.SelectedValue == "Q")
            {
                if (GvQuotationDetail.Rows.Count > 0)
                {
                    bool b1 = false;
                    foreach (GridViewRow Row in GvQuotationDetail.Rows)
                    {
                        if (((CheckBox)Row.FindControl("chkGvQuotationDetailSelect")).Checked)
                        {
                            b1 = true;
                        }
                    }
                    if (!b1)
                    {
                        DisplayMessage("Select Atleast one product");

                        return;
                    }
                }
                else
                {
                    DisplayMessage("No Product available For Generate Sales Order");

                    return;
                }
            }
        }
        else
        {
            DisplayMessage("Select Order Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlOrderType);

            return;
        }

        if (chkSendInPO.Checked == true && chksendInproduction.Checked == true)
        {
            DisplayMessage("You can not send this sales order to both department ,so choose production or purchase !");
            chksendInproduction.Focus();

            return;
        }


        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        if (txtTaxP.Text == "")
        {
            txtTaxP.Text = "0";
        }
        if (txtTaxV.Text == "")
        {
            txtTaxV.Text = "0";
        }
        if (txtDiscountP.Text == "")
        {
            txtDiscountP.Text = "0";
        }
        if (txtDiscountV.Text == "")
        {
            txtDiscountV.Text = "0";
        }
        if (txtTotalAmount.Text == "")
        {
            txtTotalAmount.Text = "0";
        }

        //Add Code for Finance On 07-12-2015
        //For Accounts Entry
        string strReceiveVoucherAcc = string.Empty;

        if (ddlOrderType.SelectedValue == "D")
        {
            foreach (GridViewRow gvr in GvProductDetail.Rows)
            {
                if (((DropDownList)gvr.FindControl("ddlLocation")).SelectedIndex > 0)
                {
                    chksendInproduction.Checked = true;
                    break;
                }
                else
                {
                    chksendInproduction.Checked = false;
                }
            }
        }
        else
            if (ddlOrderType.SelectedValue == "Q")
        {
            foreach (GridViewRow gvrQ in GvQuotationDetail.Rows)
            {
                if (((DropDownList)gvrQ.FindControl("ddlLocation")).SelectedIndex > 0)
                {
                    chksendInproduction.Checked = true;
                    break;
                }
                else
                {
                    chksendInproduction.Checked = false;
                }
            }
        }

        string SendInPo = "0";
        if (chkSendInPO.Checked)
        {
            SendInPo = "1";
        }
        if (chksendInproduction.Checked)
        {
            SendInPo = "2";
        }

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        //For Suppliers Account
        strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

        //For Purchase Invoice Account
        string strSIAccount = string.Empty;
        DataTable dtSIAccount = new DataView(dtAcParameter, "Param_Name='Sales Invoice'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtSIAccount.Rows.Count > 0)
        {
            strSIAccount = dtSIAccount.Rows[0]["Param_Value"].ToString();
        }

        //For Advance Credit Account
        string strAdvanceCreditAC = string.Empty;
        DataTable dtAdvanceCreditAC = new DataView(dtAcParameter, "Param_Name='SO Advance Credit Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtAdvanceCreditAC.Rows.Count > 0)
        {
            strAdvanceCreditAC = dtAdvanceCreditAC.Rows[0]["Param_Value"].ToString();
        }

        //Delete Concept in Vouchers if Already Exists
        if (editid.Value != "" && editid.Value != "0")
        {
            string sql = "select trans_id,ReconciledFromFinance from ac_voucher_header where isactive='true' and field1='SO' and field2='" + editid.Value + "'";
            DataTable dtFinance = objda.return_DataTable(sql);
            if (dtFinance.Rows.Count > 0 && bool.Parse(dtFinance.Rows[0]["ReconciledFromFinance"].ToString()) == false)
            {
                string strTransId = dtFinance.Rows[0]["Trans_Id"].ToString();
                objVoucherHeader.DeleteVoucherHeaderPermanent(strTransId);
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, StrLocationId, strTransId);
            }
            else if (dtFinance.Rows.Count > 0 && bool.Parse(dtFinance.Rows[0]["ReconciledFromFinance"].ToString()) == true)
            {
                DisplayMessage("Related voucher has been posted, so you can't change anthing");
                return;
            }
            dtFinance.Dispose();

            //DataTable dtFinance = objVoucherHeader.GetVoucherHeaderAllTrueOnly(StrCompId, StrBrandId, StrLocationId, Session["FinanceYearId"].ToString());
            //DataTable dtNotPosted = new DataView(dtFinance, "ReconciledFromFinance='False' and Field1='SO' and Field2='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //DataTable dtPosted = new DataView(dtFinance, "ReconciledFromFinance='True' and Field1='SO' and Field2='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtNotPosted.Rows.Count > 0)
            //{
            //    string strTransId = dtNotPosted.Rows[0]["Trans_Id"].ToString();
            //    objVoucherHeader.DeleteVoucherHeaderPermanent(strTransId);
            //    objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, StrLocationId, strTransId);
            //    //objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, StrLocationId, strTransId);
            //}
            //else if (dtPosted.Rows.Count > 0)
            //{
            //    DisplayMessage("Your Finance Record was Effected So you cant change Anything");
            //    return;
            //}
        }
        //End Code for Finance

        string strPayAccount = string.Empty;
        double AgeingAmount = 0;

        //here we get agent id from contact master and selected agent name for generate commissson 
        //this code is created by jitendra upadhyay 
        //created on 07-05-2016
        string strAgentId = string.Empty;
        if (txtAgentName.Text != "")
        {
            strAgentId = txtAgentName.Text.Split('/')[1].ToString();
        }
        else
        {
            strAgentId = "0";
        }

        //here we are checking that sales order approval is set or not 

        DataTable dtEmpApproval = new DataTable();
        string ApprovalEmpPermission = string.Empty;
        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval").Rows[0]["ParameterValue"].ToString()))
        {
            if (ddlPaymentMode.SelectedItem.Text.Trim() == "Credit" || (ddlPaymentMode.SelectedItem.Text.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString().Trim()) == true))
            {
                ApprovalEmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("SalesOrder").Rows[0]["Approval_Level"].ToString();

                dtEmpApproval = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "67", Session["EmpId"].ToString());

                if (dtEmpApproval.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue , please contact to your admin");
                    return;
                }
            }
        }

        //Here we check account no exist or not in case or payment mode credit or in case of advance payment - Neelkanth Purohit - 04/09/2018
        DataTable _dtAdvancePayment = ViewState["PayementDt"] != null ? (DataTable)ViewState["PayementDt"] : new DataTable();
        string strOtherAccountId = "0";
        if (ddlPaymentMode.SelectedItem.Text == "Credit" || _dtAdvancePayment.Rows.Count > 0)
        {
            strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(txtCustomer.Text.Split('/')[3].ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
            if (strOtherAccountId == "0")
            {
                setDefaultValueForUcAcMaster();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
                return;
            }

            //Code added 09-11-2019 to check credit limit on sales order as per the requirement and suggestion of kuwait team
            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
            {
                string[] _result = objCustomerCreditParam.checkCreditLimit(0, double.Parse(txtNetAmount.Text), txtCustomer.Text.Split('/')[3].ToString(), strOtherAccountId, txtSODate.Text, ddlCurrency.SelectedValue, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
                if (_result[0] == "false")  //array index 0 - return true/false and 1 - return message
                {
                    btnSOrderSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSOrderSave, "").ToString());
                    DisplayMessage(_result[1]);
                    return;
                }
            }
        }
        //-----------------------end-------------------------------------------

        string strlocationId = "0";

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value != "")
            {

                SalesOrderId = editid.Value;
                DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval", ref trns);

                if (DtApprove.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                    {
                        if (ddlPaymentMode.SelectedItem.Text.Trim() == "Credit" || (ddlPaymentMode.SelectedItem.Text.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval", ref trns).Rows[0]["ParameterValue"].ToString().Trim()) == true))
                        {

                            if (ViewState["Status"].ToString() == "Rejected" || ViewState["Status"].ToString() == "Approved")
                            {
                                b = objSOrderHeader.UpdateSOHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSONo.Text, ObjSysParam.getDateForInput(txtSODate.Text).ToString(), strPaymentModeId, ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text).ToString(),/*(txtTransFrom.Text)  Replace To =>*/ddlOrderType.SelectedValue.Trim(), hdnSalesQuotationId.Value, GetCustomerId(txtCustomer), strAddressId, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, txtShippingCharge.Text, txtRemark.Text, txtNetAmount.Text, chkSendInProjectManagement.Checked.ToString(), ddlDeliveryvoucher.SelectedValue.ToString(), ddlCurrency.SelectedValue, strAgentId, strAddressId2, GetCustomerId(txtShipCustomerName), CustOrderNo.ToString(), "Pending", "", chkPartialShipment.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), SendInPo.ToString(), ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);
                                DataTable dtEmp = objEmpApproval.getApprovalTransByRef_IDandApprovalId(editid.Value.ToString(), "9", ref trns);
                                // dtEmp = new DataView(dtEmp, "Approval_Id='9' and Ref_Id='" + editid.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtEmp.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtEmp.Rows.Count; i++)
                                    {
                                        objEmpApproval.UpdateApprovalTransaciton("SalesOrder", editid.Value.ToString(), "67", dtEmp.Rows[i]["Emp_Id"].ToString(), "Pending", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                    }
                                }
                                else
                                {
                                    if (dtEmpApproval.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dtEmpApproval.Rows.Count; j++)
                                        {
                                            string PriorityEmpId = dtEmpApproval.Rows[j]["Emp_Id"].ToString();
                                            string IsPriority = dtEmpApproval.Rows[j]["Priority"].ToString();
                                            int cur_trans_id = 0;
                                            if (ApprovalEmpPermission == "1")
                                            {
                                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), editid.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                            }
                                            else if (ApprovalEmpPermission == "2")
                                            {
                                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), editid.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                            }
                                            else if (ApprovalEmpPermission == "3")
                                            {
                                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), editid.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                            }
                                            else
                                            {
                                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), editid.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                            }
                                            // Insert Notification For Leave by  ghanshyam suthar
                                            Session["PriorityEmpId"] = PriorityEmpId;
                                            Session["cur_trans_id"] = cur_trans_id;
                                            Session["Ref_ID"] = editid.Value;
                                            Set_Notification();


                                        }
                                    }
                                }
                            }
                            else
                            {
                                b = objSOrderHeader.UpdateSOHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSONo.Text, ObjSysParam.getDateForInput(txtSODate.Text).ToString(), strPaymentModeId, ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text).ToString(),/*(txtTransFrom.Text)  Replace To =>*/ddlOrderType.SelectedValue.Trim(), hdnSalesQuotationId.Value, GetCustomerId(txtCustomer), strAddressId, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, txtShippingCharge.Text, txtRemark.Text, txtNetAmount.Text, chkSendInProjectManagement.Checked.ToString(), ddlDeliveryvoucher.SelectedValue.ToString(), ddlCurrency.SelectedValue, strAgentId, strAddressId2, GetCustomerId(txtShipCustomerName), CustOrderNo.ToString(), ViewState["Status"].ToString(), "", chkPartialShipment.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), SendInPo.ToString(), ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);
                            }
                        }
                        else
                        {
                            b = objSOrderHeader.UpdateSOHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSONo.Text, ObjSysParam.getDateForInput(txtSODate.Text).ToString(), strPaymentModeId, ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text).ToString(),/*(txtTransFrom.Text)  Replace To =>*/ddlOrderType.SelectedValue.Trim(), hdnSalesQuotationId.Value, GetCustomerId(txtCustomer), strAddressId, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, txtShippingCharge.Text, txtRemark.Text, txtNetAmount.Text, chkSendInProjectManagement.Checked.ToString(), ddlDeliveryvoucher.SelectedValue.ToString(), ddlCurrency.SelectedValue, strAgentId, strAddressId2, GetCustomerId(txtShipCustomerName), CustOrderNo.ToString(), ViewState["Status"].ToString(), "", chkPartialShipment.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), SendInPo.ToString(), ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);
                        }
                    }
                    else
                    {
                        b = objSOrderHeader.UpdateSOHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSONo.Text, ObjSysParam.getDateForInput(txtSODate.Text).ToString(), strPaymentModeId, ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text).ToString(),/*(txtTransFrom.Text)  Replace To =>*/ddlOrderType.SelectedValue.Trim(), hdnSalesQuotationId.Value, GetCustomerId(txtCustomer), strAddressId, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, txtShippingCharge.Text, txtRemark.Text, txtNetAmount.Text, chkSendInProjectManagement.Checked.ToString(), ddlDeliveryvoucher.SelectedValue.ToString(), ddlCurrency.SelectedValue, strAgentId, strAddressId2, GetCustomerId(txtShipCustomerName), CustOrderNo.ToString(), "Approved", "", chkPartialShipment.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), SendInPo.ToString(), ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);
                    }
                }


                //For Finance Entry
                //payment mode
                DataTable dtPayment = new DataTable();
                dtPayment = (DataTable)ViewState["PayementDt"];
                if (dtPayment != null)
                {
                    ///here we arer checking that advance payment already exisst or not in finance
                    ///
                    string sql = "select trans_id from ac_voucher_header where isactive='true' and field1='SO' and field2='" + editid.Value + "' and ReconciledFromFinance='True'";

                    //DataTable dtFinance = objVoucherHeader.GetVoucherAll(StrCompId, StrBrandId, StrLocationId, HttpContext.Current.Session["FinanceYearId"].ToString());
                    DataTable dtFinance = objda.return_DataTable(sql);

                    //dtFinance = new DataView(dtFinance, "Ref_Type='SO' and Ref_Id =" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    string strMaxId = string.Empty;
                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                    if (dtPayment.Rows.Count > 0)
                    {
                        if (dtFinance.Rows.Count > 0)
                        {
                            objVoucherHeader.UpdateVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dtFinance.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), editid.Value, "SO", txtSONo.Text, txtSODate.Text, txtSONo.Text, txtSODate.Text, "RV", "1/1/1800", "1/1/1800", "", "0", ddlCurrency.SelectedValue, "0.00", "From SO On '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "SO", "", "", "Customer Order Number '" + txtCustOrderNo.Text + "'", "", "True", DateTime.Now.ToString(), dtFinance.Rows[0]["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objVoucherDetail.DeleteVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dtFinance.Rows[0]["Trans_Id"].ToString(), ref trns);

                            strMaxId = dtFinance.Rows[0]["Trans_Id"].ToString();
                        }
                        else
                        {
                            strMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), editid.Value, "SO", txtSONo.Text, txtSODate.Text, txtSONo.Text, txtSODate.Text, "RV", "1/1/1800", "1/1/1800", "", "0", ddlCurrency.SelectedValue, "0.00", "From SO On '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "SO", "", "", "Customer Order Number '" + txtCustOrderNo.Text + "'", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()).ToString();
                        }

                    }


                    try
                    {
                        ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "SO", editid.Value.ToString(), ref trns);

                        foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                        {
                            strPayAccount = dr["AccountNo"].ToString();
                            ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "SO", editid.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            string strPayType = string.Empty;
                            DataTable dtPayType = objPaymentMode.GetPaymentModeMasterById(StrCompId.ToString(), dr["PaymentModeId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                            if (dtPayType.Rows.Count > 0)
                            {
                                strPayType = dtPayType.Rows[0]["Field1"].ToString();
                            }

                            //Detail Entry
                            //CreditEntry
                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            if (strReceiveVoucherAcc == strAdvanceCreditAC)
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceCreditAC, strOtherAccountId, editid.Value, "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "PY", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                AgeingAmount = AgeingAmount + Convert.ToDouble(dr["Pay_Charges"].ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceCreditAC, "0", editid.Value, "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "PY", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }

                            //DebitEntry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            if (strReceiveVoucherAcc == dr["AccountNo"].ToString())
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, editid.Value, "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                //AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Amount"].ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", editid.Value, "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }
                        }


                    }
                    catch
                    {

                    }
                }

                if (b != 0)
                {
                    //this code is created by jitendra upadhya on 17-04-2015
                    //this code for delete record from tax ref detail table by reftype and ref id
                    //code start

                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SO", editid.Value, ref trns);

                    for (int i = 0; i < gridView.Rows.Count; i++)
                    {
                        if (((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value == "")
                        {
                            continue;
                        }
                        objTaxRefDetail.InsertRecord("SO", editid.Value, "0", "0", "0", ((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value, GetAmountDecimal(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text).ToString(), ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text, false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //code end

                    ObjSOrderDetail.DeleteSODetail(StrCompId, StrBrandId, StrLocationId, editid.Value, ref trns);
                    if (ddlOrderType.SelectedValue == "D")
                    {
                        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SO", editid.Value, ref trns);
                        foreach (GridViewRow gvr in GvProductDetail.Rows)
                        {
                            Label lblgvSerialNo = (Label)gvr.FindControl("lblSerialNo");
                            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                            HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdngvUnitId");
                            HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
                            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                            TextBox lblgvFreeQuantity = (TextBox)gvr.FindControl("lblgvFreeQuantity");
                            Label lblgvRemainQuantity = (Label)gvr.FindControl("lblgvRemainQuantity");
                            TextBox lblgvUnitPrice = (TextBox)gvr.FindControl("lblgvUnitPrice");

                            TextBox lblgvTaxP = (TextBox)gvr.FindControl("lblgvTaxP");
                            TextBox lblgvTaxV = (TextBox)gvr.FindControl("lblgvTaxV");
                            TextBox lblgvDiscountP = (TextBox)gvr.FindControl("lblgvDiscountP");
                            TextBox lblgvDiscountV = (TextBox)gvr.FindControl("lblgvDiscountV");
                            TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                            strlocationId = "0";
                            if (ddlLocation.SelectedIndex > 0)
                            {
                                strlocationId = ddlLocation.SelectedValue;
                            }

                            if (lblgvUnitPrice.Text == "")
                            {
                                lblgvUnitPrice.Text = "0";
                            }
                            if (lblgvTaxP.Text == "")
                            {
                                lblgvTaxP.Text = "0";
                            }
                            if (lblgvTaxV.Text == "")
                            {
                                lblgvTaxV.Text = "0";
                            }
                            if (lblgvDiscountP.Text == "")
                            {
                                lblgvDiscountP.Text = "0";
                            }

                            if (lblgvDiscountV.Text == "")
                            {
                                lblgvDiscountV.Text = "0";
                            }

                            if (txtgvAgentCommission.Text == "" || txtAgentName.Text == "")
                            {
                                txtgvAgentCommission.Text = "0";
                            }




                            int Detail_ID = ObjSOrderDetail.InsertSODetail(StrCompId, StrBrandId, StrLocationId, editid.Value, lblgvSerialNo.Text, hdngvProductId.Value, lblgvQuantity.Text, lblgvFreeQuantity.Text, lblgvRemainQuantity.Text, hdngvUnitId.Value, lblgvUnitPrice.Text, lblgvTaxP.Text, lblgvTaxV.Text, lblgvDiscountP.Text, lblgvDiscountV.Text, txtgvAgentCommission.Text, "", strlocationId, "", "", "", ((CheckBox)gvr.FindControl("chkIsProduction")).Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            Insert_Tax("GvProductDetail", editid.Value, Detail_ID.ToString(), gvr, ref trns);
                            //this code is created by jitendra upadhya on 17-04-2015
                            //this code for insert record in tax ref detail table when we apply multiple tax according product category
                            //code start
                            foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                            {
                                objTaxRefDetail.InsertRecord("SO", editid.Value, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, "0", ((HiddenField)gvr.FindControl("hdngvProductId")).Value, ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, GetAmountDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            //code end
                        }
                    }
                    else if (ddlOrderType.SelectedValue == "Q")
                    {
                        foreach (GridViewRow gvrQ in GvQuotationDetail.Rows)
                        {
                            if (((CheckBox)gvrQ.FindControl("chkGvQuotationDetailSelect")).Checked)
                            {
                                Label lblgvSerialNo = (Label)gvrQ.FindControl("lblgvSerialNo");
                                HiddenField hdngvProductId = (HiddenField)gvrQ.FindControl("hdngvProductId");
                                DropDownList ddlgvUnit = (DropDownList)gvrQ.FindControl("ddlgvUnit");
                                HiddenField hdngvCurrencyId = (HiddenField)gvrQ.FindControl("hdnCurrencyId");
                                TextBox lblgvQuantity = (TextBox)gvrQ.FindControl("lblgvQuantity");
                                TextBox txtgvFreeQuantity = (TextBox)gvrQ.FindControl("txtgvFreeQuantity");
                                Label txtgvRemainQuantity = (Label)gvrQ.FindControl("txtgvRemainQuantity");
                                TextBox txtgvUnitPrice = (TextBox)gvrQ.FindControl("txtgvUnitPrice");
                                TextBox txtgvTaxP = (TextBox)gvrQ.FindControl("txtgvTaxP");
                                TextBox txtgvTaxV = (TextBox)gvrQ.FindControl("txtgvTaxV");
                                TextBox txtgvDiscountP = (TextBox)gvrQ.FindControl("txtgvDiscountP");
                                TextBox txtgvDiscountV = (TextBox)gvrQ.FindControl("txtgvDiscountV");

                                TextBox txtgvTaxUnitP = (TextBox)gvrQ.FindControl("txtgvTaxUnitP");
                                TextBox txtgvTaxUnitV = (TextBox)gvrQ.FindControl("txtgvTaxUnitV");
                                TextBox txtgvDiscountUnitP = (TextBox)gvrQ.FindControl("txtgvDiscountUnitP");
                                TextBox txtgvDiscountUnitV = (TextBox)gvrQ.FindControl("txtgvDiscountUnitV");
                                TextBox txtgvAgentCommission = (TextBox)gvrQ.FindControl("txtgvAgentCommission");
                                DropDownList ddlLocation = (DropDownList)gvrQ.FindControl("ddlLocation");
                                strlocationId = "0";
                                if (ddlLocation.SelectedIndex > 0)
                                {
                                    strlocationId = ddlLocation.SelectedValue;
                                }
                                if (txtgvFreeQuantity.Text == "")
                                {
                                    txtgvFreeQuantity.Text = "0";
                                }
                                if (txtgvRemainQuantity.Text == "")
                                {
                                    txtgvRemainQuantity.Text = "0";
                                }
                                if (txtgvUnitPrice.Text == "")
                                {
                                    txtgvUnitPrice.Text = "0";
                                }
                                if (txtgvTaxP.Text == "")
                                {
                                    txtgvTaxP.Text = "0";
                                }
                                if (txtgvTaxV.Text == "")
                                {
                                    txtgvTaxV.Text = "0";
                                }
                                if (txtgvDiscountP.Text == "")
                                {
                                    txtgvDiscountP.Text = "0";
                                }
                                if (txtgvDiscountV.Text == "")
                                {
                                    txtgvDiscountV.Text = "0";
                                }

                                string strUnitId = string.Empty;
                                if (ddlgvUnit.SelectedValue == "--Select--")
                                {
                                    strUnitId = "0";
                                }
                                else
                                {
                                    strUnitId = ddlgvUnit.SelectedValue;
                                }

                                if (txtgvAgentCommission.Text == "" || txtAgentName.Text == "")
                                {
                                    txtgvAgentCommission.Text = "0";
                                }

                                int Detail_ID = ObjSOrderDetail.InsertSODetail(StrCompId, StrBrandId, StrLocationId, editid.Value, lblgvSerialNo.Text, hdngvProductId.Value, lblgvQuantity.Text, txtgvFreeQuantity.Text, txtgvRemainQuantity.Text, strUnitId, txtgvUnitPrice.Text, txtgvTaxP.Text, txtgvTaxV.Text, txtgvDiscountP.Text, txtgvDiscountV.Text, txtgvAgentCommission.Text, "", strlocationId, "", "", "", ((CheckBox)gvrQ.FindControl("chkIsProduction")).Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                Insert_Tax("GvQuotationDetail", editid.Value, Detail_ID.ToString(), gvrQ, ref trns);
                            }
                        }
                    }

                    // Insert Notification For Leave by  ghanshyam suthar
                    //Set_Notification();

                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Updated");
                }
            }
            else
            {
                DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval", ref trns);


                string IsApproved = "Approved";

                if (DtApprove.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                    {
                        if (ddlPaymentMode.SelectedItem.Text.Trim() == "Credit" || (ddlPaymentMode.SelectedItem.Text.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval", ref trns).Rows[0]["ParameterValue"].ToString().Trim()) == true))
                        {

                            IsApproved = "Pending";

                        }
                    }
                }

                b = objSOrderHeader.InsertSOHeader(StrCompId, StrBrandId, StrLocationId, txtSONo.Text, ObjSysParam.getDateForInput(txtSODate.Text).ToString(), strPaymentModeId, ObjSysParam.getDateForInput(txtEstimateDeliveryDate.Text).ToString(),/*(txtTransFrom.Text)  Replace To =>*/ddlOrderType.SelectedValue.Trim(), hdnSalesQuotationId.Value, GetCustomerId(txtCustomer), strAddressId, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, txtShippingCharge.Text, txtRemark.Text, txtNetAmount.Text, chkSendInProjectManagement.Checked.ToString(), ddlDeliveryvoucher.SelectedValue.ToString(), ddlCurrency.SelectedValue, strAgentId, strAddressId2, GetCustomerId(txtShipCustomerName), CustOrderNo.ToString(), IsApproved.Trim(), "", chkPartialShipment.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), SendInPo.ToString(), ddlTransType.SelectedValue.ToString(), hdnSalesPersonId.Value == "" ? "0" : hdnSalesPersonId.Value, hdnContactPersonId.Value, ref trns);
                //payment mode

                //if sales order created against the sales quotation than we need to update order close date iike sales inquiry page 
                //created by jitendra upadhyay on 21-09-2016
                //code start
                if (ddlOrderType.SelectedValue.Trim() == "Q")
                {
                    DataTable dtSQuotation = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value, ref trns);
                    if (dtSQuotation.Rows.Count > 0)
                    {
                        string strsql = "update Inv_SalesInquiryHeader set OrderCompletionDate='" + ObjSysParam.getDateForInput(txtSODate.Text).ToString("yyyy-MM-dd") + "' where SInquiryID=" + dtSQuotation.Rows[0]["SInquiry_No"].ToString() + "";
                        objda.execute_Command(strsql, ref trns);
                    }
                }

                //code end


                //for Sales Order Number
                if (b != 0)
                {
                    string strMaxId = b.ToString();
                    string strMaxCounter = string.Empty;
                    if (txtSONo.Text == ViewState["DocNo"].ToString())
                    {
                        if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is for OPC Location and Jaipur Location
                        {
                            string sql = "SELECT count(*) FROM Inv_SalesOrderHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and SalesOrderNo Like '%" + txtSONo.Text + "%'";
                            int strInvoiceCount = Int32.Parse(objda.get_SingleValue(sql, ref trns));

                            if (strInvoiceCount == 0)
                            {
                                strInvoiceCount = 1;
                                objSOrderHeader.Updatecode(b.ToString(), txtSONo.Text + strInvoiceCount, ref trns);
                                txtSONo.Text = txtSONo.Text + strInvoiceCount;
                            }
                            else
                            {
                                objSOrderHeader.Updatecode(b.ToString(), txtSONo.Text + strInvoiceCount, ref trns);
                                txtSONo.Text = txtSONo.Text + strInvoiceCount;
                            }

                        }
                        else
                        {
                            strMaxCounter = objSOrderHeader.GetSOHeaderAll_Count(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                            if (strMaxCounter == "0")
                            {
                                objSOrderHeader.Updatecode(b.ToString(), txtSONo.Text + "1", ref trns);
                                txtSONo.Text = txtSONo.Text + "1";
                            }
                            else
                            {
                                objSOrderHeader.Updatecode(b.ToString(), txtSONo.Text + strMaxCounter, ref trns);
                                txtSONo.Text = txtSONo.Text + strMaxCounter;
                            }
                        }



                    }
                }


                //For Finance Entry
                DataTable dtPayment = new DataTable();
                dtPayment = (DataTable)ViewState["PayementDt"];

                if (dtPayment != null)
                {
                    if (dtPayment.Rows.Count > 0)
                    {
                        string strVMaxId = string.Empty;
                        string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                        strVMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), b.ToString(), "SO", txtSONo.Text, txtSODate.Text, txtSONo.Text, txtSODate.Text, "RV", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, "0.00", "From SO On '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "SO", b.ToString(), "", "Customer Order Number '" + txtCustOrderNo.Text + "'", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()).ToString();

                        try
                        {
                            foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                            {
                                strPayAccount = dr["AccountNo"].ToString();
                                ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "SO", b.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                //Detail Entry
                                //CreditEntry
                                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                if (strReceiveVoucherAcc == strAdvanceCreditAC)
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "1", strAdvanceCreditAC, strOtherAccountId, b.ToString(), "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "PY", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    AgeingAmount = AgeingAmount + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "1", strReceiveVoucherAcc, strOtherAccountId, b.ToString(), "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "PY", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }

                                //DebitEntry
                                string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                string CompanyCurrDebit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                if (strReceiveVoucherAcc == dr["AccountNo"].ToString())
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, b.ToString(), "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "1", dr["AccountNo"].ToString(), "0", b.ToString(), "SO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On SO '" + txtSONo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                            }


                        }
                        catch
                        {

                        }
                    }
                }



                // updated by jitendra upadhyay 

                DataTable dtsalesquotation = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, hdnSalesQuotationId.Value, ref trns);
                if (dtsalesquotation.Rows.Count > 0)
                {
                    string SinquiryId = dtsalesquotation.Rows[0]["SInquiry_No"].ToString();
                    DataTable DtsalesInquiry = objSInquiryHeader.GetSIHeaderAllBySInquiryId(StrCompId, StrBrandId, StrLocationId, SinquiryId, ref trns);
                    if (DtsalesInquiry.Rows.Count > 0)
                    {
                        if (DtsalesInquiry.Rows[0]["Field1"].ToString() == "Quotation Come From Purchase")
                        {
                            DataTable dtPurchaseinquiry = objPIHeader.GetPIHeaderAllDataByTransFromAndNo(StrCompId, StrBrandId, StrLocationId, "SI", SinquiryId, ref trns);
                            if (dtPurchaseinquiry.Rows.Count > 0)
                            {
                                if (dtPurchaseinquiry.Rows.Count > 0)
                                {
                                    string PinquiryId = dtPurchaseinquiry.Rows[0]["Trans_Id"].ToString();
                                    objQuoteHeader.UpdateQuoteHeaderStatus(StrCompId, StrBrandId, StrLocationId, PinquiryId, "Sales Order Come From Customer", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                            dtPurchaseinquiry = null;
                        }
                    }
                    DtsalesInquiry = null;
                }
                dtsalesquotation = null;




                if (b != 0)
                {
                    string strMaxId = b.ToString();

                    //this code is created by jitendra upadhya on 17-04-2015
                    //this code for delete record from tax ref detail table by reftype and ref id
                    //code start



                    for (int i = 0; i < gridView.Rows.Count; i++)
                    {
                        if (((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value == "")
                        {
                            continue;
                        }
                        objTaxRefDetail.InsertRecord("SO", strMaxId, "0", "0", "0", ((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value, GetAmountDecimal(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text).ToString(), ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text, false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //code end

                    if (strMaxId != "" && strMaxId != "0")
                    {
                        SalesOrderId = strMaxId;
                        if (ddlOrderType.SelectedValue == "D")
                        {
                            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SO", strMaxId, ref trns);
                            foreach (GridViewRow gvr in GvProductDetail.Rows)
                            {
                                Label lblgvSerialNo = (Label)gvr.FindControl("lblSerialNo");
                                HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                                HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdngvUnitId");
                                HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
                                TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                                TextBox lblgvFreeQuantity = (TextBox)gvr.FindControl("lblgvFreeQuantity");
                                Label lblgvRemainQuantity = (Label)gvr.FindControl("lblgvRemainQuantity");
                                TextBox lblgvUnitPrice = (TextBox)gvr.FindControl("lblgvUnitPrice");
                                TextBox lblgvTaxP = (TextBox)gvr.FindControl("lblgvTaxP");
                                TextBox lblgvTaxV = (TextBox)gvr.FindControl("lblgvTaxV");
                                TextBox lblgvDiscountP = (TextBox)gvr.FindControl("lblgvDiscountP");
                                TextBox lblgvDiscountV = (TextBox)gvr.FindControl("lblgvDiscountV");
                                TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
                                DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                                strlocationId = "0";
                                if (ddlLocation.SelectedIndex > 0)
                                {
                                    strlocationId = ddlLocation.SelectedValue;
                                }
                                if (lblgvUnitPrice.Text == "")
                                {
                                    lblgvUnitPrice.Text = "0";
                                }
                                if (lblgvTaxP.Text == "")
                                {
                                    lblgvTaxP.Text = "0";
                                }
                                if (lblgvTaxV.Text == "")
                                {
                                    lblgvTaxV.Text = "0";
                                }
                                if (lblgvDiscountP.Text == "")
                                {
                                    lblgvDiscountP.Text = "0";
                                }
                                if (lblgvDiscountV.Text == "")
                                {
                                    lblgvDiscountV.Text = "0";
                                }
                                if (txtgvAgentCommission.Text == "" || txtAgentName.Text == "")
                                {
                                    txtgvAgentCommission.Text = "0";
                                }


                                int Detail_ID = ObjSOrderDetail.InsertSODetail(StrCompId, StrBrandId, StrLocationId, strMaxId, lblgvSerialNo.Text, hdngvProductId.Value, lblgvQuantity.Text, lblgvFreeQuantity.Text, lblgvRemainQuantity.Text, hdngvUnitId.Value, lblgvUnitPrice.Text, lblgvTaxP.Text, lblgvTaxV.Text, lblgvDiscountP.Text, lblgvDiscountV.Text, txtgvAgentCommission.Text, "", strlocationId, "", "", "", ((CheckBox)gvr.FindControl("chkIsProduction")).Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                Insert_Tax("GvProductDetail", strMaxId, Detail_ID.ToString(), gvr, ref trns);

                                //this code is created by jitendra upadhya on 17-04-2015
                                //this code for insert record in tax ref detail table when we apply multiple tax according product category
                                //code start
                                foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                                {
                                    objTaxRefDetail.InsertRecord("SO", strMaxId, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, "0", ((HiddenField)gvr.FindControl("hdngvProductId")).Value, ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, GetAmountDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                                //code end
                            }
                        }
                        else if (ddlOrderType.SelectedValue == "Q")
                        {
                            //update staus in sales quotation field when we create order against the order 
                            //code created by jitendra upadhyay on  16-03-2016
                            //code start


                            string strSql = "update Inv_SalesQuotationHeader set [status]='Close' where SQuotation_Id=" + hdnSalesQuotationId.Value + "";
                            objda.execute_Command(strSql, ref trns);
                            //code end 



                            foreach (GridViewRow gvrQ in GvQuotationDetail.Rows)
                            {
                                if (((CheckBox)gvrQ.FindControl("chkGvQuotationDetailSelect")).Checked)
                                {
                                    Label lblgvSerialNo = (Label)gvrQ.FindControl("lblgvSerialNo");
                                    HiddenField hdngvProductId = (HiddenField)gvrQ.FindControl("hdngvProductId");
                                    DropDownList ddlgvUnit = (DropDownList)gvrQ.FindControl("ddlgvUnit");
                                    HiddenField hdngvCurrencyId = (HiddenField)gvrQ.FindControl("hdnCurrencyId");
                                    TextBox lblgvQuantity = (TextBox)gvrQ.FindControl("lblgvQuantity");
                                    TextBox txtgvFreeQuantity = (TextBox)gvrQ.FindControl("txtgvFreeQuantity");
                                    Label txtgvRemainQuantity = (Label)gvrQ.FindControl("txtgvRemainQuantity");
                                    TextBox txtgvUnitPrice = (TextBox)gvrQ.FindControl("txtgvUnitPrice");
                                    TextBox txtgvTaxP = (TextBox)gvrQ.FindControl("txtgvTaxP");
                                    TextBox txtgvTaxV = (TextBox)gvrQ.FindControl("txtgvTaxV");
                                    TextBox txtgvDiscountP = (TextBox)gvrQ.FindControl("txtgvDiscountP");
                                    TextBox txtgvDiscountV = (TextBox)gvrQ.FindControl("txtgvDiscountV");

                                    TextBox txtgvTaxUnitP = (TextBox)gvrQ.FindControl("txtgvTaxUnitP");
                                    TextBox txtgvTaxUnitV = (TextBox)gvrQ.FindControl("txtgvTaxUnitV");
                                    TextBox txtgvDiscountUnitP = (TextBox)gvrQ.FindControl("txtgvDiscountUnitP");
                                    TextBox txtgvDiscountUnitV = (TextBox)gvrQ.FindControl("txtgvDiscountUnitV");
                                    TextBox txtgvAgentCommission = (TextBox)gvrQ.FindControl("txtgvAgentCommission");

                                    DropDownList ddlLocation = (DropDownList)gvrQ.FindControl("ddlLocation");
                                    strlocationId = "0";
                                    if (ddlLocation.SelectedIndex > 0)
                                    {
                                        strlocationId = ddlLocation.SelectedValue;
                                    }
                                    if (txtgvFreeQuantity.Text == "")
                                    {
                                        txtgvFreeQuantity.Text = "0";
                                    }
                                    if (txtgvRemainQuantity.Text == "")
                                    {
                                        txtgvRemainQuantity.Text = "0";
                                    }
                                    if (txtgvUnitPrice.Text == "")
                                    {
                                        txtgvUnitPrice.Text = "0";
                                    }
                                    if (txtgvTaxP.Text == "")
                                    {
                                        txtgvTaxP.Text = "0";
                                    }
                                    if (txtgvTaxV.Text == "")
                                    {
                                        txtgvTaxV.Text = "0";
                                    }
                                    if (txtgvDiscountP.Text == "")
                                    {
                                        txtgvDiscountP.Text = "0";
                                    }
                                    if (txtgvDiscountV.Text == "")
                                    {
                                        txtgvDiscountV.Text = "0";
                                    }

                                    string strUnitId = string.Empty;
                                    if (ddlgvUnit.SelectedValue == "--Select--")
                                    {
                                        strUnitId = "0";
                                    }
                                    else
                                    {
                                        strUnitId = ddlgvUnit.SelectedValue;
                                    }
                                    if (txtgvAgentCommission.Text == "" || txtAgentName.Text == "")
                                    {
                                        txtgvAgentCommission.Text = "0";
                                    }

                                    int Detail_ID = ObjSOrderDetail.InsertSODetail(StrCompId, StrBrandId, StrLocationId, strMaxId, lblgvSerialNo.Text, hdngvProductId.Value, lblgvQuantity.Text, txtgvFreeQuantity.Text, txtgvRemainQuantity.Text, strUnitId, txtgvUnitPrice.Text, txtgvTaxP.Text, txtgvTaxV.Text, txtgvDiscountP.Text, txtgvDiscountV.Text, txtgvAgentCommission.Text, "", strlocationId, "", "", "", ((CheckBox)gvrQ.FindControl("chkIsProduction")).Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    Insert_Tax("GvQuotationDetail", strMaxId, Detail_ID.ToString(), gvrQ, ref trns);

                                }
                            }
                        }

                        if (dtEmpApproval.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtEmpApproval.Rows.Count; j++)
                            {
                                string PriorityEmpId = dtEmpApproval.Rows[j]["Emp_Id"].ToString();
                                string IsPriority = dtEmpApproval.Rows[j]["Priority"].ToString();
                                int cur_trans_id = 0;
                                if (ApprovalEmpPermission == "1")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (ApprovalEmpPermission == "2")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (ApprovalEmpPermission == "3")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("9", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                // Insert Notification For Leave by  ghanshyam suthar
                                Session["PriorityEmpId"] = PriorityEmpId;
                                Session["cur_trans_id"] = cur_trans_id;
                                Session["Ref_ID"] = strMaxId.ToString();
                                Set_Notification();

                            }

                        }
                    }
                    DisplayMessage("Record Saved", "green");
                }
                else
                {
                    DisplayMessage("Record  Not Saved");
                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            editid.Value = "";
            FillGrid(1);

            string strpaymentModename = ddlPaymentMode.SelectedItem.Text;
            Reset();
            //AllPageCode();
            Session["ReportHtml"] = "";
            //Code for Approval
            Update_New.Update();
            DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval");

            if (DtPara.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
                {
                    if (ddlPaymentMode.SelectedItem.Text.Trim() == "Credit" || (strpaymentModename.Trim() == "Cash" && Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString().Trim()) == true))
                    {
                        string st = GetStatus(Convert.ToInt32(SalesOrderId));
                        if (st == "Approved")
                        {
                            //if (SalesOrderId != "")
                            //{
                            PrintReport(SalesOrderId);
                            //}
                        }
                    }
                    else
                    {
                        //if (SalesOrderId != "")
                        //{
                        PrintReport(SalesOrderId);
                        //}
                    }
                }
                else
                {
                    //if (SalesOrderId != "")
                    //{
                    PrintReport(SalesOrderId);
                    //}
                }
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
        string confirmValue = Request.Form["confirm_value"];

        if (SalesOrderId != "")
        {
            if (confirmValue.Contains("Yes"))
            {
                string strBody = string.Empty;
                strBody = getOrderAcknowledgement(SalesOrderId);
                strBody = strBody.Replace("<meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8'/>", "");
                strBody = "<h1>Purchase Order Acknowledgement</h1><hr/><h4> Thanks for purchase order now is being processed </h4>" + strBody;
                strBody = strBody.Replace("<title>Document</title>", "<title>Purchase Order Acknowledgement</title>");
                strBody = strBody.Replace("'Times New Roman'", "''Times New Roman''");
                strBody = strBody.Replace("'", "''");

                //Commented the code on 23-08-2024 regarding the email process for sending.
                //string strsql = "update Inv_SalesOrderHeader  set status='" + strBody + "' where trans_id='" + SalesOrderId + "'";
                //objda.execute_Command(strsql, Session["DBConnection"].ToString());
            }
        }
    }
}