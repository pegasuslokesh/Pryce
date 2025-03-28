using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Data.OleDb;

using Newtonsoft.Json;
using PurchaseDataSetTableAdapters;
using DevExpress.XtraCharts.Native;
using DevExpress.Utils.About;
using ClosedXML.Excel;
public partial class Purchase_PurchaseOrder : BasePage
{
    #region Class Object
    string StrCompId = string.Empty;
    string UserId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string strCurrencyId = string.Empty;
    Inv_TaxRefDetail objTaxRefDetail = null;
    DataAccessClass objDa = null;
    SystemParameter ObjSysParam = null;
    Inv_UnitMaster ObjUnitMaster = null;
    Set_Approval_Employee objEmpApproval = null;
    Ems_ContactMaster ObjContactMaster = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Set_AddressChild ObjAddChild = null;
    Set_AddressMaster ObjAdd = null;
    Set_BankMaster ObjBankMaster = null;
    Ac_ChartOfAccount objCOA = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Common cmn = null;
    Inv_ProductMaster ObjProductMaster = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    PurchaseOrderDetail ObjPurchaseOrderDetail = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseQuoteDetail ObjQuoteDetail = null;
    Inv_PurchaseInquiryHeader ObjPIHeader = null;
    Inv_StockDetail objStockDetail = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ShipExpHeader ObjShipExpHeader = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    EmployeeMaster objEmployee = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_SalesQuotationHeader ObjSalesQuotation = null;
    Inv_SalesOrderHeader ObjSalesOrder = null;
    Inv_SalesOrderDetail ObjSalesOrderDetail = null;
    PurchaseInvoiceDetail objPInvoiceDetail = null;
    Inv_ParameterMaster objInvParam = null;
    Contact_PriceList objSupplierPriceList = null;
    PoDetailByPoNumber objReport = null;
    LocationMaster objLocation = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Document_Master ObjDocument = null;
    Set_Suppliers ObjSupplier = null;
    Prj_ProjectMaster objProjctMaster = null;
    NotificationMaster Obj_Notifiacation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    CountryMaster ObjSysCountryMaster = null;
    Country_Currency objCountryCurrency = null;
    ProductTaxMaster objProductTaxMaster = null;
    Inv_SizeMaster ObjSizeMaster = null;
    Inv_ColorMaster ObjColorMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    public const int grdDefaultColCount = 11;
    private const string strPageName = "PurchaseOrder";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Expenses_Tax_Purchase_Order"] != null)
        {
            Session["Dt_Final_Save_Tax"] = Session["Expenses_Tax_Purchase_Order"];
        }
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        try
        {
            strCurrencyId = Session["LocCurrencyId"].ToString();
        }
        catch
        {

        }

        StrBrandId = Session["BrandId"].ToString();
        StrCompId = Session["CompId"].ToString();
        UserId = Session["UserId"].ToString();
        if (hdnLocationID.Value == "")
        {
            StrLocationId = Session["LocId"].ToString();
        }
        else
        {
            StrLocationId = hdnLocationID.Value;
        }

        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjAdd = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objProductTaxMaster = new ProductTaxMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrderDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjQuoteDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        ObjPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
        ObjShipExpHeader = new Inv_ShipExpHeader(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjSalesQuotation = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSalesOrder = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSalesOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objPInvoiceDetail = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objSupplierPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objReport = new PoDetailByPoNumber(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        ObjSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        ObjSizeMaster = new Inv_SizeMaster(Session["DBConnection"].ToString());
        ObjColorMaster = new Inv_ColorMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Session["Is_Tax_Apply"] = null;
            Session["Expenses_Tax_Purchase_Order"] = null;
            Session["Temp_Product_Tax_PO"] = null;
            Session["IsPurchaseTaxEnabled"] = null;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseOrder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            fillLocation();
            ddlLocation.SelectedValue = Session["LocID"].ToString();
            FillUser();

            bool Tax_Apply = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            bool IsTax = Tax_Apply;
            if (IsTax == true)
            {
                if (Session["LocId"].ToString() == "7")
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

            Session["Is_Tax_Apply"] = Tax_Apply.ToString();
            ViewState["dtProduct"] = null;
            ViewState["DefaultCurrency"] = null;
            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
                ViewState["CurrencyId"] = strCurrencyId;
            }
            catch
            {
            }
            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), strCurrencyId.ToString(), Session["DBConnection"].ToString());
            //ViewState["ExchangeRate"] = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), ObjCurrencyMaster.GetCurrencyMasterById(Session["CurrencyId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), ObjCurrencyMaster.GetCurrencyMasterById(strCurrencyId.ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
            txtCurrencyRate.Text = ViewState["ExchangeRate"].ToString();
            txtPoNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtPoNo.Text;
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtbinValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            //btnReset_Click(null, null);
            fillGrid();
            FillCurrency();
            Session["DtSearchProduct"] = null;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);

            string hasPermission = objDa.get_SingleValue("Select ParameterValue From Inv_ParameterMaster where Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' AND Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'  and ParameterName='Purchase Order(IsDuty)' and IsActive='True'");
            hasPermission = hasPermission == "@NOTFOUND@" ? "false" : hasPermission;
            if (hasPermission == "true")
            {
                lblAmount.Visible = true;
                //lblAmountColon.Visible = true;
                lblNetdutyPer.Visible = true;
                //lblNetdutyPercolon.Visible = true;
                txtGrossAmount.Visible = true;
                Div_GrossAmount.Visible = true;
                txtNetDutyPer.Visible = true;
                Div_NetDutyPer.Visible = true;
                txtNetDutyVal.Visible = true;
                txtGrandTotal.Visible = true;
                Div_GrandTotal.Visible = true;
                lblNetDutyVal.Visible = true;
                lblGrandTotal.Visible = true;
                //lblGrandTotalcolon.Visible = true;
                lblAmount.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Gross_Amount, Session["DBConnection"].ToString());
                lblGrandTotal.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
            }
            Session["PayementDt"] = null;
            btnAddSupplier.Visible = IsAddCustomerPermission();
            FillShipUnitddl();
            fillPaymentMode();
            txtChequedate_CalenderExtender.Format = Session["DateFormat"].ToString();
            fillExpenses();
            try
            {
                FillTransactionType();
            }
            catch
            {

            }
            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                btnEdit_Command(imgeditbutton, new CommandEventArgs(StrLocationId, Request.QueryString["Id"].ToString()));
                try
                {
                    StrLocationId = Request.QueryString["LocId"].ToString();
                }
                catch
                {
                    StrLocationId = Session["LocId"].ToString();
                }
                btnSave.Visible = false;
            }
            else
            {
                //btnList.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "List_Hide_Tab_Content()", true);
                btnSave.Visible = true;
                //btnUnPost.Visible = true;
                //btnSInvCancel.Visible = true;
            }

            string Dt_Individual = objInvParam.getParameterValueByParameterName("Allow TAX edit on individual transactions Purchase", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (Dt_Individual != "")
            {
                if (Convert.ToBoolean(Dt_Individual) == true)
                    Btn_Update_Tax.Visible = true;
                else
                    Btn_Update_Tax.Visible = false;
            }
            else
            {
                Btn_Update_Tax.Visible = false;
            }
            Dt_Individual = null;
            Expenses_Tax_Modal.Expenses_Tax_And_Amount_Clear();
            TaxandDiscountParameter();
            if (Session["Is_Tax_Apply"] != null && Session["Is_Tax_Apply"].ToString() == "False")
                Trans_Div.Visible = false;
            txtPOdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtShippingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtDeliveryDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            getPageControlsVisibility();
        }
        Lbl_Expenses_Tax_Amount_ET.Text = Expenses_Tax_Modal.Expenses_Amount_Value();
        Lbl_Expenses_Tax_ET.Text = Expenses_Tax_Modal.Expenses_Tax_Value();
        FillRefferenceprojectList();
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
        //AllPageCode();
        Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }


    }
    //call this method from web user conrol to refresh page contorls
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
    public void fillExpenses()
    {
        DataTable dt = ObjShipExp.GetShipExpMaster(StrCompId.ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");
        dt = null;
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void FillShipUnitddl()
    {
        objPageCmn.FillData((object)ddlShipUnit, ObjUnitMaster.GetUnitMaster(Session["CompId"].ToString()), "Unit_Name", "Unit_Id");
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnProductSave.Visible = clsPagePermission.bAdd;

        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();


        gvProduct.Columns[0].Visible = clsPagePermission.bEdit;
        gvProduct.Columns[1].Visible = clsPagePermission.bDelete;
        GvQuotationProductEdit.Columns[0].Visible = clsPagePermission.bDelete;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }

    #endregion
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "45", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #region System Defined Function :
    public void SaveApprovalData(string strVoucherId, string strDescription)
    {
        bool PaymentApproval = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PaymentApproval"));
        string EmpPermission = string.Empty;
        EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("PurchaseOrder").Rows[0]["Approval_Level"].ToString();
        string strApprovalStatus = string.Empty;
        DataTable dtApproval = new DataTable();
        if (PaymentApproval)
        {
            strApprovalStatus = "Pending";
            // Get dt on bases of Permission 
            dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "45", Session["EmpId"].ToString());
        }
        else
        {
            strApprovalStatus = "Approved";
        }
        if (PaymentApproval)
        {
            if (dtApproval.Rows.Count > 0)
            {
                for (int j = 0; j < dtApproval.Rows.Count; j++)
                {
                    string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                    string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                    if (EmpPermission == "1")
                    {
                        objEmpApproval.InsertApprovalTransaciton("15", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strVoucherId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", strDescription, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    }
                    else if (EmpPermission == "2")
                    {
                        objEmpApproval.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strVoucherId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", strDescription, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    }
                    else if (EmpPermission == "3")
                    {
                        objEmpApproval.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strVoucherId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", strDescription, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    }
                    else
                    {
                        objEmpApproval.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strVoucherId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", strDescription, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    }
                }
            }
        }
        dtApproval = null;
    }
    //protected string GetEmployeeCode(string strEmployeeId)
    //{
    //    string strEmployeeName = string.Empty;
    //    if (strEmployeeId != "0" && strEmployeeId != "")
    //    {
    //        DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
    //        if (dtEName.Rows.Count > 0)
    //        {
    //            strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
    //            ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
    //        }
    //        else
    //        {
    //            ViewState["Emp_Img"] = "";
    //        }
    //        dtEName = null;
    //    }
    //    else
    //    {
    //        strEmployeeName = "";
    //    }
    //    return strEmployeeName;
    //}
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/purchase"));
        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Purchase Order. on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
        Dt_Request_Type = null;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (rbtNew.Checked == true)
        {
            txtPoNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtPoNo.Text;
            HdnEdit.Value = "";
        }
        double advancePer = 0;
        double.TryParse(txtAdvancePer.Text, out advancePer);
        if (advancePer > 100)
        {
            DisplayMessage("Advance Percentage can not be greater then 100");
            return;
        }
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        //here we check that this page is updated by other user before save of current user 
        //this code is created by jitendra upadhyay on 15-05-2015
        //code start
        if (HdnEdit.Value != "")
        {
            string lockId = ObjPurchaseOrder.getRowLockId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, HdnEdit.Value);
            if (lockId != "")
            {
                if (ViewState["TimeStamp"].ToString() != lockId)
                {
                    DisplayMessage("Another User update Information reload and try again");
                    return;
                }
            }
        }
        //code end

        string locationCode = objLocation.getLocationCode(StrLocationId);

        string ReFerVoNo = string.Empty;
        string SupplierId = string.Empty;
        string ShippingLine = string.Empty;
        string shipUnit = string.Empty;
        string strprojectId = string.Empty;
        strprojectId = "0";
        if (ddlprojectname.SelectedIndex > 0)
        {
            strprojectId = ddlprojectname.Value.ToString();
        }
        if (txtPOdate.Text == "")
        {
            DisplayMessage("Enter Purchase Order Date");
            txtPOdate.Focus();
            txtPOdate.Text = "";
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtPOdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Purchase order Date in format " + Session["DateFormat"].ToString() + "");
                txtPOdate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPOdate);
                return;
            }
        }
        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtPOdate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }
        if (txtPoNo.Text == "")
        {
            DisplayMessage("Enter Purchase Order No");
            txtPoNo.Focus();
            return;
        }
        if (ddlCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Currency ");
            ddlCurrency.Focus();
            return;
        }
        if (txtCurrencyRate.Text.Trim() == "")
        {
            DisplayMessage("Enter Currency Rate");
            txtCurrencyRate.Focus();
            return;
        }
        if (txtDeliveryDate.Text.Trim() != "")
        {
            try
            {
                Convert.ToDateTime(txtDeliveryDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Delivery Date");
                txtDeliveryDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Delivery Date");
            txtDeliveryDate.Focus();
            return;
        }
        if (txtSupplierName.Text != "")
        {
            SupplierId = txtSupplierName.Text.Split('/')[1].ToString();
        }
        else
        {
            DisplayMessage("Select Supplier");
            txtSupplierName.Focus();
            return;
        }
        string strAddressId = string.Empty;
        string strshipto = string.Empty;
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAddId = ObjAdd.GetAddressDataByAddressName(txtShipingAddress.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
            dtAddId = null;
        }
        else
        {
            strAddressId = "0";
        }
        if (txtShipSupplierName.Text != "")
        {
            strshipto = txtShipSupplierName.Text.Split('/')[1].ToString();
        }
        else
        {
            strshipto = "0";
        }
        if (ddlShipUnit.SelectedIndex == 0)
        {
            shipUnit = "0";
        }
        else
        {
            shipUnit = ddlShipUnit.SelectedValue;
        }
        if (ddlFreightStatus.SelectedIndex == 0)
        {
            if (ddlExpense.SelectedIndex != 0)
            {
                if (txtShippingAcc.Text == "")
                {
                    DisplayMessage("Select Shipping accounts");
                    txtShippingAcc.Focus();
                    return;
                }
                if (txtExpensesAccount.Text == "")
                {
                    DisplayMessage("Select Expenses accounts");
                    txtExpensesAccount.Focus();
                    return;
                }
                if (Get_Expenses_Tax_Amount() == "")
                {
                    DisplayMessage("Enter Paid Amount");
                    txtpaidamount.Focus();
                    return;
                }
            }
        }

        string hasPurchaseOrderApproval = objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DataTable dt1 = new DataTable();
        string EmpPermission = string.Empty;
        try
        {
            EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("PurchaseOrder").Rows[0]["Approval_Level"].ToString();
        }
        catch
        {

        }

        if (hasPurchaseOrderApproval != "")
        {
            if (Convert.ToBoolean(hasPurchaseOrderApproval) == true)
            {
                dt1 = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "45", Session["EmpId"].ToString());
                if (dt1.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue , please contact to your admin");
                    return;
                }
            }
        }
        //Add New Code For Check Approval On 29-08-2016
        bool PaymentApproval = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "PaymentApproval"));
        string strApprovalStatus = string.Empty;
        DataTable dtApproval = new DataTable();
        DataTable dtPaymentCheck = new DataTable();
        dtPaymentCheck = (DataTable)Session["PayementDt"];
        double _expensesAmt = 0;
        double.TryParse(Get_Expenses_Tax_Amount(), out _expensesAmt);
        if (ddlFreightStatus.SelectedValue == "Y" && _expensesAmt > 0)
        {
            if (PaymentApproval)
            {
                strApprovalStatus = "Pending";
                // Get dt on bases of Permission 
                dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "45", Session["EmpId"].ToString());
                // dt1 = new DataView(dt1, "Approval='Leave'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtApproval.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue , please contact to your admin");
                    return;
                }
            }
            else
            {
                strApprovalStatus = "Approved";
            }
        }
        else if (dtPaymentCheck != null && dtPaymentCheck.Rows.Count > 0)
        {
            if (PaymentApproval)
            {
                strApprovalStatus = "Pending";
                // Get dt on bases of Permission 
                dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "45", Session["EmpId"].ToString());
                // dt1 = new DataView(dt1, "Approval='Leave'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtApproval.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue , please contact to your admin");
                    return;
                }
            }
            else
            {
                strApprovalStatus = "Approved";
            }
        }
        dtApproval = null;
        dtPaymentCheck = null;
        //End Code
        if (ddlOrderType.SelectedValue == "D")
        {
            if (gvProduct.Rows.Count == 0)
            {
                DisplayMessage("Add at least one product");
                btnAddProduct.Focus();
                return;
            }
        }
        else
        {
            if (HdnEdit.Value == "")
            {
                bool TrofFl = false;
                if (Session["RPQ_No"] != null)
                {
                    foreach (GridViewRow GridRow in gvQuatationProduct.Rows)
                    {
                        CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("chk");
                        if (CheckBoxId.Checked)
                        {
                            TrofFl = true;
                        }
                    }
                    if (!TrofFl)
                    {
                        DisplayMessage("Select at least one product");
                        gvQuatationProduct.Focus();
                        return;
                    }
                }
                else
                {
                    foreach (GridViewRow GridRow in GvSalesOrderDetail.Rows)
                    {
                        CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("ChkSelect");
                        if (CheckBoxId.Checked)
                        {
                            TrofFl = true;
                        }
                    }
                    if (!TrofFl)
                    {
                        DisplayMessage("Select at least one Product");
                        GvSalesOrderDetail.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (Session["OrderType"].ToString() == "PQ")
                {
                    if (GvQuotationProductEdit.Rows.Count == 0)
                    {
                        DisplayMessage("Add at least one Product");
                        btnAddProduct.Focus();
                        return;
                    }
                }
                else
                {
                    if (gvProduct.Rows.Count == 0)
                    {
                        DisplayMessage("Add at least one Product");
                        btnAddProduct.Focus();
                        return;
                    }
                }
            }
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
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Shipping Date");
            txtShippingDate.Focus();
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
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Receiving Date");
            txtReceivingDate.Focus();
            return;
        }
        if (txtNetDutyPer.Text == "")
        {
            txtNetDutyPer.Text = "0";
        }
        if (txtNetDutyVal.Text == "")
        {
            txtNetDutyVal.Text = "0";
        }
        if (PnlReference.Visible == true)
        {
            ReFerVoNo = ddlReferenceVoucherType.SelectedValue;
        }
        int b = 0;
        DataTable DtQuotation = new DataTable();
        string RpqId = string.Empty;
        string SOId = "0";
        try
        {
            DtQuotation = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["RPQ_No"].ToString().Trim());
            RpqId = DtQuotation.Rows[0]["Trans_Id"].ToString();
        }
        catch
        {
            RpqId = "0";
        }
        try
        {
            SOId = ObjSalesOrder.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.Trim()).Rows[0]["Trans_Id"].ToString();
        }
        catch
        {
            SOId = "0";
        }
        //For Accounts Entry
        string strPaymentVoucherAcc = string.Empty;
        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        //For Suppliers Account
        strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        //For Purchase Invoice Account
        string strPIAccount = string.Empty;
        DataTable dtPIAccount = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPIAccount.Rows.Count > 0)
        {
            strPIAccount = dtPIAccount.Rows[0]["Param_Value"].ToString();
        }
        //For Expesnes Debit Account
        string strExpensesDebitAC = string.Empty;
        //DataTable dtExpensesDebitAC = new DataView(dtAcParameter, "Param_Name='PO Expenses Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dtExpensesDebitAC.Rows.Count > 0)
        //{
        if (txtShippingAcc.Text != "")
        {
            strExpensesDebitAC = txtShippingAcc.Text.Split('/')[1].ToString();
        }
        //}
        //For Advance Debit Account
        string strAdvanceDebitAC = string.Empty;
        DataTable dtAdvanceDebitAC = new DataView(dtAcParameter, "Param_Name='PO Advance Debit Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtAdvanceDebitAC.Rows.Count > 0)
        {
            strAdvanceDebitAC = dtAdvanceDebitAC.Rows[0]["Param_Value"].ToString();
        }
        string strExpCrAc = string.Empty;
        if (txtExpensesAccount.Text != "")
        {
            strExpCrAc = txtExpensesAccount.Text.Split('/')[1].ToString();
        }
        //Here we check account no exist or not in case or payment mode credit or in case of advance payment - Neelkanth Purohit - 04/09/2018
        DataTable _dtAdvancePayment = Session["PayementDt"] != null ? (DataTable)Session["PayementDt"] : new DataTable();
        string strOtherAccountId = "0";
        if (strPaymentVoucherAcc == strExpCrAc || _dtAdvancePayment.Rows.Count > 0 || strPaymentVoucherAcc == strExpensesDebitAC)
        {
            strOtherAccountId = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
            if (strOtherAccountId == "0")
            {
                DisplayMessage("Account not exist in " + ddlCurrency.SelectedItem.Text + " , First create it");
                return;
            }
        }
        //-----------------------end-------------------------------------------
        //Delete Concept in Vouchers if Already Exists
        string strCheckApproval = string.Empty;
        if (HdnEdit.Value != "" && HdnEdit.Value != "0")
        {
            string sql = "select trans_id,ReconciledFromFinance,Field3 from ac_voucher_header where isactive='true' and Ref_Type='PO' and Ref_Id='" + HdnEdit.Value + "'";
            DataTable dtFinance = objDa.return_DataTable(sql);
            if (dtFinance.Rows.Count > 0 && bool.Parse(dtFinance.Rows[0]["ReconciledFromFinance"].ToString()) == false)
            {
                string strTransId = dtFinance.Rows[0]["Trans_Id"].ToString();
                strCheckApproval = dtFinance.Rows[0]["Field3"].ToString();
                if (strCheckApproval == "Pending")
                {
                    objVoucherHeader.DeleteVoucherHeaderPermanent(strTransId);
                    objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, StrLocationId, strTransId);
                    objEmpApproval.Delete_Approval_Transaction("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "0", strTransId);
                }
                else if (strCheckApproval == "Approved")
                {
                    DisplayMessage("Payment voucher has been approved, so you can not edit it");
                    txtReceivingDate.Focus();
                    return;
                }
            }
            else if (dtFinance.Rows.Count > 0 && bool.Parse(dtFinance.Rows[0]["ReconciledFromFinance"].ToString()) == true)
            {
                DisplayMessage("Relavent voucher has been posted, so you can not edit it");
                txtReceivingDate.Focus();
                return;
            }
        }
        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "BankAccount");
        if (dtAccount.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccount.Rows.Count; i++)
            {
                if (strAccountId == "")
                {
                    strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountId = "0";
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        dtAccount = null;
        double AgeingAmountDebit = 0;
        double AgeingAmountCredit = 0;
        string Id = string.Empty;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (HdnEdit.Value == "")
            {
                string IsApproved = "Pending";
                if (!Convert.ToBoolean(objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                {
                    IsApproved = "Approved";
                }
                b = ObjPurchaseOrder.InsertPurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtPoNo.Text.Trim(), ddlPaymentMode.SelectedValue.ToString(), ObjSysParam.getDateForInput(txtPOdate.Text).ToString(), ReFerVoNo.ToString(), RpqId.ToString(), SOId.ToString(), ObjSysParam.getDateForInput(txtDeliveryDate.Text).ToString(), ddlOrderType.SelectedValue.ToString(), SupplierId.ToString(), ddlCurrency.SelectedValue.ToString(), txtCurrencyRate.Text, txRemark.Text, ShippingLine.ToString(), ddlShipBy.SelectedValue.ToString(), ddlShipmentType.SelectedValue.ToString(), ddlFreightStatus.SelectedValue.ToString(), shipUnit, txtTotalWeight.Text.ToString(), txtUnitRate.Text.ToString(), ObjSysParam.getDateForInput(txtShippingDate.Text).ToString(), ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString(), ddlPartialShipment.SelectedValue.ToString(), txtDesc.Content, strshipto, strAddressId, strprojectId, "", txtAirwaybillno.Text, txtvolumetricweight.Text, txtVendorQNo.Text.ToString(), txtNetDutyPer.Text, txtNetDutyVal.Text, IsApproved.ToString().Trim(), string.IsNullOrEmpty(txtAdvancePer.Text) ? "0" : txtAdvancePer.Text, true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                HdnEdit.Value = b.ToString();
                if (b != 0)
                {
                    if (txtPoNo.Text == ViewState["DocNo"].ToString())
                    {
                        string count = ObjPurchaseOrder.getTotalCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, ref trns);
                        if (count == "")
                        {
                            ObjPurchaseOrder.Updatecode(b.ToString(), txtPoNo.Text + "1", ref trns);
                            txtPoNo.Text = txtPoNo.Text + "1";
                        }
                        else
                        {
                            ObjPurchaseOrder.Updatecode(b.ToString(), txtPoNo.Text + count, ref trns);
                            txtPoNo.Text = txtPoNo.Text + count;
                        }
                    }
                    string strMaxId = string.Empty;
                    if (ddlFreightStatus.SelectedValue == "Y" && ddlExpense.SelectedIndex != 0 && Convert.ToDouble(Get_Expenses_Tax_Amount()) > 0)
                    {
                        string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                        strMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), HdnEdit.Value, "PO", txtPoNo.Text, txtPOdate.Text, txtPoNo.Text, txtPOdate.Text, "PV", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, "0.00", "From PO '" + txtPoNo.Text + "' On '" + locationCode + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "Cash", strApprovalStatus, "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString()).ToString();
                        SaveApprovalData(strMaxId, "Purchase Order Approval");
                    }
                    string strexpexchagerate = "0";
                    if (ddlFreightStatus.SelectedIndex == 0 && ddlExpense.SelectedIndex != 0 && Convert.ToDouble(Get_Expenses_Tax_Amount()) > 0)
                    {
                        ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, b.ToString(), ddlCurrency.SelectedValue, txtCurrencyRate.Text, "0", txtGrandTotal.Text.Trim(), Get_Expenses_Tax_Amount(), "PO", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
                        strexpexchagerate = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objLocation.GetLocationMasterById(Session["CompId"].ToString(), StrLocationId, ref trns).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
                        ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, b.ToString(), ddlExpense.SelectedValue, txtExpensesAccount.Text.Split('/')[1].ToString(), txtpaidamount.Text.Trim(), ddlCurrency.SelectedValue, strexpexchagerate, (Convert.ToDouble(Get_Expenses_Tax_Amount()) * Convert.ToDouble(strexpexchagerate)).ToString(), "PO", txtShippingAcc.Text.Split('/')[1].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        Tax_Insert_Into_Inv_TaxRefDetail(b.ToString());
                        if (ddlFreightStatus.SelectedValue == "Y")
                        {
                            //Detail Entry
                            //DebitEntry
                            if (strPaymentVoucherAcc == strExpensesDebitAC)
                            {
                                string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                                string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                string TaxstrCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                                string TaxCompanyCurrDebit = TaxstrCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                if (strAccountId.Split(',').Contains(strExpensesDebitAC))
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(Get_Expenses_Tax_Amount());
                            }
                            else
                            {
                                string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                                string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                string TaxstrCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                                string TaxCompanyCurrDebit = TaxstrCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                if (strAccountId.Split(',').Contains(strExpensesDebitAC))
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                    Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                            }
                            //CreditEntry
                            if (strPaymentVoucherAcc == txtExpensesAccount.Text.Split('/')[1].ToString())
                            {
                                if (Lbl_Expenses_Tax_Amount_ET.Text.Trim() == "")
                                    Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                                if (txtpaidamount.Text.Trim() == "")
                                    txtpaidamount.Text = "0.00";
                                double Net_Credit_Amount = Convert.ToDouble(Lbl_Expenses_Tax_Amount_ET.Text.Trim()) + Convert.ToDouble(txtpaidamount.Text.Trim());
                                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Net_Credit_Amount.ToString());
                                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                if (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()))
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(Get_Expenses_Tax_Amount());
                            }
                            else
                            {
                                if (Lbl_Expenses_Tax_Amount_ET.Text.Trim() == "")
                                    Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                                if (txtpaidamount.Text.Trim() == "")
                                    txtpaidamount.Text = "0.00";
                                double Net_Credit_Amount = Convert.ToDouble(Lbl_Expenses_Tax_Amount_ET.Text.Trim()) + Convert.ToDouble(txtpaidamount.Text.Trim());
                                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Net_Credit_Amount.ToString());
                                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                if (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()))
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                }
                            }
                        }
                    }
                    //insert advance payment record
                    //code start
                    DataTable dtPayment = new DataTable();
                    dtPayment = (DataTable)Session["PayementDt"];
                    if (dtPayment != null)
                    {
                        try
                        {
                            foreach (DataRow dr in ((DataTable)Session["PayementDt"]).Rows)
                            {
                                ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PO", b.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                try
                                {
                                    if (ddlFreightStatus.SelectedValue == "Y" && Get_Expenses_Tax_Amount() != "")
                                    {
                                        //DebitEntry
                                        if (strPaymentVoucherAcc == strAdvanceDebitAC)
                                        {
                                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                        }
                                        else
                                        {
                                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                        //CreditEntry
                                        if (strPaymentVoucherAcc == dr["AccountNo"].ToString())
                                        {
                                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                        }
                                        else
                                        {
                                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                    else if (ddlFreightStatus.SelectedValue == "N")
                                    {
                                        string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                        strMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), HdnEdit.Value, "PO", txtPoNo.Text, txtPOdate.Text, txtPoNo.Text, txtPOdate.Text, "PV", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, "0.00", "From PO '" + txtPoNo.Text + "' On '" + locationCode + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "Cash", strApprovalStatus, "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString()).ToString();
                                        //DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId();
                                        //if (dtMaxId.Rows.Count > 0)
                                        //{
                                        //strMaxId = dtMaxId.Rows[0][0].ToString();
                                        SaveApprovalData(strMaxId, "Purchase Order Approval");
                                        //}
                                        //DebitEntry
                                        if (strPaymentVoucherAcc == strAdvanceDebitAC)
                                        {
                                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                        }
                                        else
                                        {
                                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                        //CreditEntry
                                        if (strPaymentVoucherAcc == dr["AccountNo"].ToString())
                                        {
                                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                        }
                                        else
                                        {
                                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                            if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {
                                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    //code end
                    Id = b.ToString();
                    if (gvProduct.Rows.Count != 0)
                    {
                        foreach (GridViewRow gvRow in gvProduct.Rows)
                        {
                            Label lblSNo = (Label)gvRow.FindControl("lblSerialNO");
                            Label lblgvProductId = (Label)gvRow.FindControl("lblGvProductId");
                            Label lblProductId = (Label)gvRow.FindControl("lblProductId");
                            Label lblgvUnitId = (Label)gvRow.FindControl("lblUnit");
                            Label lblgvUnitID = (Label)gvRow.FindControl("lblgvUnitID");
                            Label lblgvReqQty = (Label)gvRow.FindControl("lblReqQty");
                            Label lblgvFreeQty = (Label)gvRow.FindControl("lblFreeQty");
                            TextBox lblgvUnitCost = (TextBox)gvRow.FindControl("lblUnitRate");
                            TextBox Discount_Per = (TextBox)gvRow.FindControl("lblDiscount");
                            TextBox Discount_Value = (TextBox)gvRow.FindControl("lblDiscountValue");
                            TextBox Tax_Per = (TextBox)gvRow.FindControl("lblTax");
                            TextBox Tax_Value = (TextBox)gvRow.FindControl("lblTaxValue");
                            HiddenField hdnSOId = (HiddenField)gvRow.FindControl("hdnSOId");

                            hdnSOId.Value = hdnSOId.Value == "" ? "0" : hdnSOId.Value;

                            if (Tax_Per.Text == "")
                                Tax_Per.Text = "0";
                            if (Tax_Value.Text == "")
                                Tax_Value.Text = "0";
                            if (Discount_Per.Text == "")
                                Discount_Per.Text = "0";
                            if (Discount_Value.Text == "")
                                Discount_Value.Text = "0";
                            double Net_Unit_Cost = Convert.ToDouble(lblgvUnitCost.Text.Trim().ToString());
                            double Net_Tax_Value = Convert.ToDouble(Tax_Value.Text);
                            string Pryce_After_Tax = (Net_Unit_Cost + Net_Tax_Value).ToString();
                            string NetPrice = (Convert.ToDouble(Pryce_After_Tax) * Convert.ToDouble(lblgvReqQty.Text.Trim().ToString())).ToString();
                            if (NetPrice == "")
                                NetPrice = "0";
                            if (Pryce_After_Tax == "")
                                Pryce_After_Tax = "0";
                            int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString(), lblSNo.Text.Trim().ToString(), lblgvProductId.Text.Trim().ToString(), lblProductId.Text.Trim().ToString(), lblgvUnitID.Text.Trim().ToString(), lblgvUnitCost.Text.Trim().ToString(), lblgvReqQty.Text.Trim().ToString(), "0", lblgvFreeQty.Text.Trim().ToString(), "", "0", hdnSOId.Value, Tax_Per.Text, Tax_Value.Text, Pryce_After_Tax, Discount_Per.Text, Discount_Value.Text, NetPrice, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            Insert_Tax("gvProduct", Id.ToString(), Detail_ID.ToString(), gvRow, ref trns);
                        }
                    }
                    try
                    {
                        RpqId = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["RPQ_No"].ToString().Trim(), ref trns).Rows[0]["Trans_Id"].ToString();
                    }
                    catch
                    {
                        RpqId = "0";
                    }
                    try
                    {
                        SOId = ObjSalesOrder.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.Trim(), ref trns).Rows[0]["Trans_Id"].ToString();
                    }
                    catch
                    {
                        SOId = "0";
                    }
                    int SerialNo = 1;
                    if (gvQuatationProduct.Rows.Count != 0)
                    {
                        foreach (GridViewRow GridRow in gvQuatationProduct.Rows)
                        {
                            CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("chk");
                            Label lblgvProductId = (Label)GridRow.FindControl("lblgvProductId");
                            Label lblgvProductDes = (Label)GridRow.FindControl("lblgvProductName");
                            Label lblgvUnitId = (Label)GridRow.FindControl("lblgvUnitId");
                            TextBox lblgvRequiredQty = (TextBox)GridRow.FindControl("txtgvRequiredQty");
                            TextBox lblgvUnitCost = (TextBox)GridRow.FindControl("txtUnitCost");
                            Label lblTaxPercentage = (Label)GridRow.FindControl("lblTax");
                            Label lblTaxvalue = (Label)GridRow.FindControl("lblTaxValue");
                            Label lblPriceAftertax = (Label)GridRow.FindControl("lblTaxafterPrice");
                            TextBox lblDiscountPercentage = (TextBox)GridRow.FindControl("Txt_Discount_Per_Quatation");
                            TextBox lblDiscountValue = (TextBox)GridRow.FindControl("Txt_DiscountValue_Quatation");
                            Label lblNetAmount = (Label)GridRow.FindControl("lblgvAmmount");
                            TextBox txtgvFreeQty = (TextBox)GridRow.FindControl("txtFreeQty");
                            TextBox txtgvRemainQty = new TextBox();
                            txtgvRemainQty.Text = "0";
                            if (lblTaxPercentage.Text == "")
                                lblTaxPercentage.Text = "0";
                            if (lblTaxvalue.Text == "")
                                lblTaxvalue.Text = "0";
                            if (lblPriceAftertax.Text == "")
                                lblPriceAftertax.Text = "0";
                            if (lblDiscountPercentage.Text == "")
                                lblDiscountPercentage.Text = "0";
                            if (lblDiscountValue.Text == "")
                                lblDiscountValue.Text = "0";
                            if (lblNetAmount.Text == "")
                                lblNetAmount.Text = "0";
                            if (CheckBoxId.Checked)
                            {
                                int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString(), SerialNo.ToString(), lblgvProductId.Text, lblgvProductDes.Text, lblgvUnitId.Text, lblgvUnitCost.Text, lblgvRequiredQty.Text, txtgvRemainQty.Text, txtgvFreeQty.Text, ddlReferenceVoucherType.SelectedValue.ToString(), RpqId.Trim(), SOId.ToString(), lblTaxPercentage.Text, lblTaxvalue.Text, lblPriceAftertax.Text, lblDiscountPercentage.Text, lblDiscountValue.Text, lblNetAmount.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                Insert_Tax("gvQuatationProduct", Id.ToString(), Detail_ID.ToString(), GridRow, ref trns);
                                SerialNo++;
                            }
                        }
                    }
                    if (GvSalesOrderDetail.Rows.Count != 0)
                    {
                        SerialNo = 1;
                        foreach (GridViewRow GridRow in GvSalesOrderDetail.Rows)
                        {
                            CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("ChkSelect");
                            //Label lblSNo = (Label)GridRow.FindControl("lblSerialNO");
                            HiddenField lblgvProductId = (HiddenField)GridRow.FindControl("hdngvProductId");
                            Label lblProductId = (Label)GridRow.FindControl("lblgvProductName");
                            DropDownList lblgvUnitID = (DropDownList)GridRow.FindControl("ddlgvUnit");
                            HiddenField lblgvUnitId = (HiddenField)GridRow.FindControl("hdnUnitId");
                            //Label lblgvUnitID = (Label)GridRow.FindControl("lblgvUnitID");
                            TextBox lblgvReqQty = (TextBox)GridRow.FindControl("txtgvQuantity");
                            TextBox lblgvFreeQty = (TextBox)GridRow.FindControl("txtgvFreeQuantity");
                            TextBox lblgvUnitCost = (TextBox)GridRow.FindControl("txtgvUnitPrice");
                            Label Discount_Per = (Label)GridRow.FindControl("lblTaxAfterPrice");
                            TextBox Discount_Value = (TextBox)GridRow.FindControl("Txt_DiscountValue_Sales");
                            TextBox Tax_Per = (TextBox)GridRow.FindControl("Txt_Tax_Per_Sales");
                            TextBox Tax_Value = (TextBox)GridRow.FindControl("Txt_Tax_Value_Sales");
                            if (Tax_Per.Text == "")
                                Tax_Per.Text = "0";
                            if (Tax_Value.Text == "")
                                Tax_Value.Text = "0";
                            if (Discount_Per.Text == "")
                                Discount_Per.Text = "0";
                            if (Discount_Value.Text == "")
                                Discount_Value.Text = "0";
                            double Net_Unit_Cost = Convert.ToDouble(lblgvUnitCost.Text.Trim().ToString());
                            double Net_Tax_Value = Convert.ToDouble(Tax_Value.Text);
                            string Pryce_After_Tax = (Net_Unit_Cost + Net_Tax_Value).ToString();
                            string NetPrice = (Convert.ToDouble(Pryce_After_Tax) * Convert.ToDouble(lblgvReqQty.Text.Trim().ToString())).ToString();
                            if (NetPrice == "")
                                NetPrice = "0";
                            if (Pryce_After_Tax == "")
                                Pryce_After_Tax = "0";
                            if (CheckBoxId.Checked)
                            {
                                int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString(), SerialNo.ToString(), lblgvProductId.Value.Trim().ToString(), lblProductId.Text.Trim().ToString(), lblgvUnitID.SelectedValue.ToString(), lblgvUnitCost.Text.Trim().ToString(), lblgvReqQty.Text.Trim().ToString(), "0", lblgvFreeQty.Text.Trim().ToString(), "", "0", "0", Tax_Per.Text, Tax_Value.Text, Pryce_After_Tax, Discount_Per.Text, Discount_Value.Text, NetPrice, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                Insert_Tax("GvSalesOrderDetail", Id.ToString(), Detail_ID.ToString(), GridRow, ref trns);
                                SerialNo++;
                            }
                        }
                    }
                    if (hasPurchaseOrderApproval != "")
                    {
                        if (Convert.ToBoolean(hasPurchaseOrderApproval) == true)
                        {
                            if (dt1.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {
                                    string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                                    string IsPriority = dt1.Rows[j]["Priority"].ToString();
                                    int cur_trans_id = 0;
                                    if (EmpPermission == "1")
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                    }
                                    else if (EmpPermission == "2")
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                    }
                                    else if (EmpPermission == "3")
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                    }
                                    else
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                    }
                                    // Insert Notification For Leave by  ghanshyam suthar
                                    Session["PriorityEmpId"] = PriorityEmpId;
                                    Session["cur_trans_id"] = cur_trans_id;
                                    Session["Ref_ID"] = b.ToString();
                                    Set_Notification();
                                }
                            }
                        }
                    }
                    if (RpqId != "0")
                    {
                        string PINo = string.Empty;
                        PINo = DtQuotation.Rows[0]["PI_No"].ToString();
                        ObjPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, PINo, "Purchase Order has Send", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    DisplayMessage("Record Saved", "green");
                }
                string isApproved = objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (isApproved != "")
                {
                    if (Convert.ToBoolean(isApproved) == true)
                    {
                        string st = GetStatus(Convert.ToInt32(Id), ref trns);
                        if (st == "Approved")
                        {
                            //PrintReport(Id);
                        }
                        else
                        {
                            DisplayMessage("Order not Approved");
                        }
                    }
                    else
                    {
                        //PrintReport(Id);
                    }
                }
            }
            else
            {
                string OrderId = string.Empty;
                OrderId = HdnEdit.Value;
                string isApprove = objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (isApprove != "")
                {
                    if (Convert.ToBoolean(isApprove) == true)
                    {
                        if (ViewState["Status"].ToString() == "Rejected")
                        {
                            b = ObjPurchaseOrder.UpdatePurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value, txtPoNo.Text.Trim(), ddlPaymentMode.SelectedValue.ToString(), ObjSysParam.getDateForInput(txtPOdate.Text).ToString(), ReFerVoNo.ToString(), RpqId.ToString(), SOId.ToString(), ObjSysParam.getDateForInput(txtDeliveryDate.Text).ToString(), ddlOrderType.SelectedValue.ToString(), SupplierId.ToString(), ddlCurrency.SelectedValue.ToString(), txtCurrencyRate.Text, txRemark.Text, ShippingLine.ToString(), ddlShipBy.SelectedValue.ToString(), ddlShipmentType.SelectedValue.ToString(), ddlFreightStatus.SelectedValue.ToString(), shipUnit, txtTotalWeight.Text.ToString(), txtUnitRate.Text.ToString(), ObjSysParam.getDateForInput(txtShippingDate.Text).ToString(), ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString(), ddlPartialShipment.SelectedValue.ToString(), txtDesc.Content, strshipto, strAddressId, strprojectId, "", txtAirwaybillno.Text, txtvolumetricweight.Text, txtVendorQNo.Text.ToString(), txtNetDutyPer.Text, txtNetDutyVal.Text, "Pending", string.IsNullOrEmpty(txtAdvancePer.Text) ? "0" : txtAdvancePer.Text, true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                            DataTable dtEmp = objEmpApproval.GetApprovalTransation(StrCompId, ref trns);
                            dtEmp = new DataView(dtEmp, "Approval_Id='7' and Ref_Id='" + HdnEdit.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtEmp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtEmp.Rows.Count; i++)
                                {
                                    objEmpApproval.UpdateApprovalTransaciton("PurchaseOrder", HdnEdit.Value.ToString(), "45", dtEmp.Rows[i]["Emp_Id"].ToString(), "Pending", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                }
                            }
                        }
                        else
                        {
                            b = ObjPurchaseOrder.UpdatePurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value, txtPoNo.Text.Trim(), ddlPaymentMode.SelectedValue.ToString(), ObjSysParam.getDateForInput(txtPOdate.Text).ToString(), ReFerVoNo.ToString(), RpqId.ToString(), SOId.ToString(), ObjSysParam.getDateForInput(txtDeliveryDate.Text).ToString(), ddlOrderType.SelectedValue.ToString(), SupplierId.ToString(), ddlCurrency.SelectedValue.ToString(), txtCurrencyRate.Text, txRemark.Text, ShippingLine.ToString(), ddlShipBy.SelectedValue.ToString(), ddlShipmentType.SelectedValue.ToString(), ddlFreightStatus.SelectedValue.ToString(), shipUnit, txtTotalWeight.Text.ToString(), txtUnitRate.Text.ToString(), ObjSysParam.getDateForInput(txtShippingDate.Text).ToString(), ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString(), ddlPartialShipment.SelectedValue.ToString(), txtDesc.Content, strshipto, strAddressId, strprojectId, "", txtAirwaybillno.Text, txtvolumetricweight.Text, txtVendorQNo.Text.ToString(), txtNetDutyPer.Text, txtNetDutyVal.Text, ViewState["Status"].ToString(), string.IsNullOrEmpty(txtAdvancePer.Text) ? "0" : txtAdvancePer.Text, true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                        }
                    }
                    else
                    {
                        b = ObjPurchaseOrder.UpdatePurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value, txtPoNo.Text.Trim(), ddlPaymentMode.SelectedValue.ToString(), ObjSysParam.getDateForInput(txtPOdate.Text).ToString(), ReFerVoNo.ToString(), RpqId.ToString(), SOId.ToString(), ObjSysParam.getDateForInput(txtDeliveryDate.Text).ToString(), ddlOrderType.SelectedValue.ToString(), SupplierId.ToString(), ddlCurrency.SelectedValue.ToString(), txtCurrencyRate.Text, txRemark.Text, ShippingLine.ToString(), ddlShipBy.SelectedValue.ToString(), ddlShipmentType.SelectedValue.ToString(), ddlFreightStatus.SelectedValue.ToString(), shipUnit, txtTotalWeight.Text.ToString(), txtUnitRate.Text.ToString(), ObjSysParam.getDateForInput(txtShippingDate.Text).ToString(), ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString(), ddlPartialShipment.SelectedValue.ToString(), txtDesc.Content, strshipto, strAddressId, strprojectId, "", txtAirwaybillno.Text, txtvolumetricweight.Text, txtVendorQNo.Text.ToString(), txtNetDutyPer.Text, txtNetDutyVal.Text, ViewState["Status"].ToString(), string.IsNullOrEmpty(txtAdvancePer.Text) ? "0" : txtAdvancePer.Text, true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                    }
                }
                //For Finance Entry
                string strMaxId = string.Empty;
                double _expensesAmt1 = 0;
                double.TryParse(Get_Expenses_Tax_Amount(), out _expensesAmt1);
                if (ddlFreightStatus.SelectedValue == "Y" && _expensesAmt1 > 0)
                {
                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    strMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), HdnEdit.Value, "PO", txtPoNo.Text, txtPOdate.Text, txtPoNo.Text, txtPOdate.Text, "PV", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, "0.00", "From PO '" + txtPoNo.Text + "' On '" + locationCode + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "Cash", strApprovalStatus, "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString()).ToString();
                    //DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId();
                    //if (dtMaxId.Rows.Count > 0)
                    //{
                    //  strMaxId = dtMaxId.Rows[0][0].ToString();
                    SaveApprovalData(strMaxId, "Purchase Order Approval");
                    //}
                }
                //delete ship expenses header and detail and also reinserted
                ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, "PO", ref trns);
                ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.ToString(), "PO", ref trns);
                string strexpexchagerate = "0";
                if (ddlFreightStatus.SelectedIndex == 0 && ddlExpense.SelectedIndex != 0 && Get_Expenses_Tax_Amount() != "")
                {
                    ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ddlCurrency.SelectedValue, txtCurrencyRate.Text, "0", txtGrandTotal.Text.Trim(), Get_Expenses_Tax_Amount(), "PO", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(), ref trns);
                    strexpexchagerate = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objLocation.GetLocationMasterById(Session["CompId"].ToString(), StrLocationId, ref trns).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
                    ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value, ddlExpense.SelectedValue, txtExpensesAccount.Text.Split('/')[1].ToString(), txtpaidamount.Text.Trim(), ddlCurrency.SelectedValue, strexpexchagerate, (Convert.ToDouble(Get_Expenses_Tax_Amount()) * Convert.ToDouble(strexpexchagerate)).ToString(), "PO", txtShippingAcc.Text.Split('/')[1].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    if (ddlFreightStatus.SelectedValue == "Y" && Get_Expenses_Tax_Amount() != "")
                    {
                        //Detail Entry
                        //DebitEntry
                        if (strPaymentVoucherAcc == strExpensesDebitAC)
                        {
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            string TaxstrCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                            string TaxCompanyCurrDebit = TaxstrCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            if (strAccountId.Split(',').Contains(strExpensesDebitAC))
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, txtSupplierName.Text.Split('/')[1].ToString(), HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(Get_Expenses_Tax_Amount());
                        }
                        else
                        {
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            string TaxstrCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtpaidamount.Text.Trim());
                            string TaxCompanyCurrDebit = TaxstrCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            if (strAccountId.Split(',').Contains(strExpensesDebitAC))
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                Tax_Insert_Into_Ac_Voucher_Detail_Debit(strMaxId, "1", strExpensesDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", txtpaidamount.Text.Trim(), "0.00", "'" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, txtpaidamount.Text.Trim(), TaxCompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                        }
                        //CreditEntry
                        if (strPaymentVoucherAcc == txtExpensesAccount.Text.Split('/')[1].ToString())
                        {
                            if (Lbl_Expenses_Tax_Amount_ET.Text.Trim() == "")
                                Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                            if (txtpaidamount.Text.Trim() == "")
                                txtpaidamount.Text = "0.00";
                            double Net_Credit_Amount = Convert.ToDouble(Lbl_Expenses_Tax_Amount_ET.Text.Trim()) + Convert.ToDouble(txtpaidamount.Text.Trim());
                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Net_Credit_Amount.ToString());
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            if (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()))
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(Get_Expenses_Tax_Amount());
                        }
                        else
                        {
                            if (Lbl_Expenses_Tax_Amount_ET.Text.Trim() == "")
                                Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                            if (txtpaidamount.Text.Trim() == "")
                                txtpaidamount.Text = "0.00";
                            double Net_Credit_Amount = Convert.ToDouble(Lbl_Expenses_Tax_Amount_ET.Text.Trim()) + Convert.ToDouble(txtpaidamount.Text.Trim());
                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Net_Credit_Amount.ToString());
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            if (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()))
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                            else
                            {
                                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", "1/1/1800", "", "0.00", Net_Credit_Amount.ToString(), "Expenses On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strexpexchagerate, Net_Credit_Amount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                            }
                        }
                    }
                }
                //delete record from payment transaction table and inserted 
                //code start
                DataTable dtPayment = new DataTable();
                dtPayment = (DataTable)Session["PayementDt"];
                if (dtPayment != null)
                {
                    try
                    {
                        ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "PO", HdnEdit.Value.ToString(), ref trns);
                        foreach (DataRow dr in ((DataTable)Session["PayementDt"]).Rows)
                        {
                            ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "PO", HdnEdit.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            try
                            {
                                if (ddlFreightStatus.SelectedValue == "Y" && Get_Expenses_Tax_Amount() != "")
                                {
                                    //DebitEntry
                                    if (strPaymentVoucherAcc == strAdvanceDebitAC)
                                    {
                                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                    }
                                    else
                                    {
                                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                    }
                                    //CreditEntry
                                    if (strPaymentVoucherAcc == dr["AccountNo"].ToString())
                                    {
                                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                    }
                                    else
                                    {
                                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                    }
                                }
                                else if (ddlFreightStatus.SelectedValue == "N")
                                {
                                    string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                    strMaxId = objVoucherHeader.InsertVoucherHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), HdnEdit.Value, "PO", txtPoNo.Text, txtPOdate.Text, txtPoNo.Text, txtPOdate.Text, "PV", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, "0.00", "From PO '" + txtPoNo.Text + "' On '" + locationCode + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "Cash", strApprovalStatus, "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString()).ToString();
                                    SaveApprovalData(strMaxId, "Purchase Order Approval");
                                    if (strPaymentVoucherAcc == strAdvanceDebitAC)
                                    {
                                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        AgeingAmountDebit = AgeingAmountDebit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                    }
                                    else
                                    {
                                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(strAdvanceDebitAC))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", strAdvanceDebitAC, "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), dr["Pay_Charges"].ToString(), "0.00", "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "PY", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                    }
                                    //CreditEntry
                                    if (strPaymentVoucherAcc == dr["AccountNo"].ToString())
                                    {
                                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), strOtherAccountId, HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        AgeingAmountCredit = AgeingAmountCredit + Convert.ToDouble(dr["Pay_Charges"].ToString());
                                    }
                                    else
                                    {
                                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), dr["Pay_Charges"].ToString());
                                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                        if (strAccountId.Split(',').Contains(dr["AccountNo"].ToString()))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrCredit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strMaxId, "1", dr["AccountNo"].ToString(), "0", HdnEdit.Value, "PO", "1/1/1800", dr["ChequeDate"].ToString(), dr["ChequeNo"].ToString(), "0.00", dr["Pay_Charges"].ToString(), "Payment On PO '" + txtPoNo.Text + "' On '" + locationCode + "'", "", Session["EmpId"].ToString(), dr["PayCurrencyID"].ToString(), dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), CompanyCurrCredit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                //code end
                if (b != 0)
                {
                    try
                    {
                        RpqId = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Session["RPQ_No"].ToString().Trim(), ref trns).Rows[0]["Trans_Id"].ToString();
                    }
                    catch
                    {
                        RpqId = "0";
                    }
                    try
                    {
                        SOId = ObjSalesOrder.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.Trim(), ref trns).Rows[0]["Trans_Id"].ToString();
                    }
                    catch
                    {
                        SOId = "0";
                    }
                }
                //delete record in purchase order detail table using header order id 
                ObjPurchaseOrderDetail.DeletePurchaseOrderDetailByPONo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.Trim(), ref trns);
                objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PO", HdnEdit.Value.ToString(), ref trns);
                int SerialNo = 1;
                foreach (GridViewRow GridRow in gvQuatationProduct.Rows)
                {
                    CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("chk");
                    //Label lblSNo = (Label)GridRow.FindControl("lblSerialNO");
                    Label lblgvProductId = (Label)GridRow.FindControl("lblgvProductId");
                    Label lblProductId = (Label)GridRow.FindControl("lblgvProductName");
                    Label lblgvUnitId = (Label)GridRow.FindControl("lblgvUnitId");
                    Label lblgvUnitID = (Label)GridRow.FindControl("lblgvUnitId");
                    TextBox lblgvReqQty = (TextBox)GridRow.FindControl("txtgvRequiredQty");
                    TextBox lblgvFreeQty = (TextBox)GridRow.FindControl("txtFreeQty");
                    TextBox lblgvUnitCost = (TextBox)GridRow.FindControl("txtUnitCost");
                    TextBox Discount_Per = (TextBox)GridRow.FindControl("Txt_Discount_Per_Quatation");
                    TextBox Discount_Value = (TextBox)GridRow.FindControl("Txt_DiscountValue_Quatation");
                    TextBox Tax_Per = (TextBox)GridRow.FindControl("lblTax");
                    TextBox Tax_Value = (TextBox)GridRow.FindControl("lblTaxValue");
                    if (Tax_Per.Text == "")
                        Tax_Per.Text = "0";
                    if (Tax_Value.Text == "")
                        Tax_Value.Text = "0";
                    if (Discount_Per.Text == "")
                        Discount_Per.Text = "0";
                    if (Discount_Value.Text == "")
                        Discount_Value.Text = "0";
                    double Net_Unit_Cost = Convert.ToDouble(lblgvUnitCost.Text.Trim().ToString());
                    double Net_Tax_Value = Convert.ToDouble(Tax_Value.Text);
                    string Pryce_After_Tax = (Net_Unit_Cost + Net_Tax_Value).ToString();
                    string NetPrice = (Convert.ToDouble(Pryce_After_Tax) * Convert.ToDouble(lblgvReqQty.Text.Trim().ToString())).ToString();
                    if (NetPrice == "")
                        NetPrice = "0";
                    if (Pryce_After_Tax == "")
                        Pryce_After_Tax = "0";
                    if (CheckBoxId.Checked)
                    {
                        int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value, SerialNo.ToString(), lblgvProductId.Text.Trim().ToString(), lblProductId.Text.Trim().ToString(), lblgvUnitID.Text.Trim().ToString(), lblgvUnitCost.Text.Trim().ToString(), lblgvReqQty.Text.Trim().ToString(), "0", lblgvFreeQty.Text.Trim().ToString(), "", "0", "0", Tax_Per.Text, Tax_Value.Text, Pryce_After_Tax, Discount_Per.Text, Discount_Value.Text, NetPrice, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax("gvQuatationProduct", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow, ref trns);
                        SerialNo++;
                    }
                }
                SerialNo = 1;
                foreach (GridViewRow GridRow in GvSalesOrderDetail.Rows)
                {
                    CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("ChkSelect");
                    //Label lblSNo = (Label)GridRow.FindControl("lblSerialNO");
                    Label lblgvProductId = (Label)GridRow.FindControl("hdngvProductId");
                    Label lblProductId = (Label)GridRow.FindControl("lblgvProductName");
                    DropDownList lblgvUnitID = (DropDownList)GridRow.FindControl("ddlgvUnit");
                    HiddenField lblgvUnitId = (HiddenField)GridRow.FindControl("hdnUnitId");
                    //Label lblgvUnitID = (Label)GridRow.FindControl("lblgvUnitID");
                    Label lblgvReqQty = (Label)GridRow.FindControl("txtgvQuantity");
                    Label lblgvFreeQty = (Label)GridRow.FindControl("txtgvFreeQuantity");
                    TextBox lblgvUnitCost = (TextBox)GridRow.FindControl("txtgvUnitPrice");
                    TextBox Discount_Per = (TextBox)GridRow.FindControl("lblTaxAfterPrice");
                    TextBox Discount_Value = (TextBox)GridRow.FindControl("Txt_DiscountValue_Sales");
                    TextBox Tax_Per = (TextBox)GridRow.FindControl("Txt_Tax_Per_Sales");
                    TextBox Tax_Value = (TextBox)GridRow.FindControl("Txt_Tax_Value_Sales");
                    if (Tax_Per.Text == "")
                        Tax_Per.Text = "0";
                    if (Tax_Value.Text == "")
                        Tax_Value.Text = "0";
                    if (Discount_Per.Text == "")
                        Discount_Per.Text = "0";
                    if (Discount_Value.Text == "")
                        Discount_Value.Text = "0";
                    double Net_Unit_Cost = Convert.ToDouble(lblgvUnitCost.Text.Trim().ToString());
                    double Net_Tax_Value = Convert.ToDouble(Tax_Value.Text);
                    string Pryce_After_Tax = (Net_Unit_Cost + Net_Tax_Value).ToString();
                    string NetPrice = (Convert.ToDouble(Pryce_After_Tax) * Convert.ToDouble(lblgvReqQty.Text.Trim().ToString())).ToString();
                    if (NetPrice == "")
                        NetPrice = "0";
                    if (Pryce_After_Tax == "")
                        Pryce_After_Tax = "0";
                    if (CheckBoxId.Checked)
                    {
                        int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), SerialNo.ToString(), lblgvProductId.Text.Trim().ToString(), lblProductId.Text.Trim().ToString(), lblgvUnitID.SelectedValue.ToString(), lblgvUnitCost.Text.Trim().ToString(), lblgvReqQty.Text.Trim().ToString(), "0", lblgvFreeQty.Text.Trim().ToString(), "", "0", "0", Tax_Per.Text, Tax_Value.Text, Pryce_After_Tax, Discount_Per.Text, Discount_Value.Text, NetPrice, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax("GvSalesOrderDetail", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow, ref trns);
                        SerialNo++;
                    }
                }
                string RefType = string.Empty;
                string RefId = string.Empty;
                RefType = ddlReferenceVoucherType.SelectedValue;
                RefId = RpqId.ToString();
                if (gvProduct.Rows.Count != 0)
                {
                    if (gvProduct.Rows.Count != 0)
                    {
                        foreach (GridViewRow gvRow in gvProduct.Rows)
                        {
                            Label lblSNo = (Label)gvRow.FindControl("lblSerialNO");
                            Label lblgvProductId = (Label)gvRow.FindControl("lblGvProductId");
                            Label lblProductId = (Label)gvRow.FindControl("lblProductId");
                            Label lblgvUnitId = (Label)gvRow.FindControl("lblUnit");
                            Label lblgvUnitID = (Label)gvRow.FindControl("lblgvUnitID");
                            Label lblgvReqQty = (Label)gvRow.FindControl("lblReqQty");
                            Label lblgvFreeQty = (Label)gvRow.FindControl("lblFreeQty");
                            TextBox lblgvUnitCost = (TextBox)gvRow.FindControl("lblUnitRate");
                            TextBox Discount_Per = (TextBox)gvRow.FindControl("lblDiscount");
                            TextBox Discount_Value = (TextBox)gvRow.FindControl("lblDiscountValue");
                            TextBox Tax_Per = (TextBox)gvRow.FindControl("lblTax");
                            TextBox Tax_Value = (TextBox)gvRow.FindControl("lblTaxValue");
                            HiddenField hdnSOId = (HiddenField)gvRow.FindControl("hdnSOId");

                            if (Tax_Per.Text == "")
                                Tax_Per.Text = "0";
                            if (Tax_Value.Text == "")
                                Tax_Value.Text = "0";
                            if (Discount_Per.Text == "")
                                Discount_Per.Text = "0";
                            if (Discount_Value.Text == "")
                                Discount_Value.Text = "0";
                            double Net_Unit_Cost = Convert.ToDouble(lblgvUnitCost.Text.Trim().ToString());
                            double Net_Tax_Value = Convert.ToDouble(Tax_Value.Text);
                            string Pryce_After_Tax = (Net_Unit_Cost + Net_Tax_Value).ToString();
                            string NetPrice = (Convert.ToDouble(Pryce_After_Tax) * Convert.ToDouble(lblgvReqQty.Text.Trim().ToString())).ToString();
                            if (NetPrice == "")
                                NetPrice = "0";
                            if (Pryce_After_Tax == "")
                                Pryce_After_Tax = "0";
                            int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), lblSNo.Text.Trim().ToString(), lblgvProductId.Text.Trim().ToString(), lblProductId.Text.Trim().ToString(), lblgvUnitID.Text.Trim().ToString(), lblgvUnitCost.Text.Trim().ToString(), lblgvReqQty.Text.Trim().ToString(), "0", lblgvFreeQty.Text.Trim().ToString(), "", "0", hdnSOId.Value, Tax_Per.Text, Tax_Value.Text, Pryce_After_Tax, Discount_Per.Text, Discount_Value.Text, NetPrice, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            Insert_Tax("gvProduct", HdnEdit.Value.ToString(), Detail_ID.ToString(), gvRow, ref trns);
                        }
                    }
                }
                if (GvQuotationProductEdit.Rows.Count != 0)
                {
                    foreach (GridViewRow GridRow in GvQuotationProductEdit.Rows)
                    {
                        int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), ((Label)GridRow.FindControl("lblSerialNO")).Text, ((Label)GridRow.FindControl("lblGvProductId")).Text, ((Label)GridRow.FindControl("lblProductId")).Text, ((Label)GridRow.FindControl("lblGvUnitId")).Text, ((TextBox)GridRow.FindControl("txtUnitRate")).Text, ((TextBox)GridRow.FindControl("txtReqQty")).Text, "0", ((Label)GridRow.FindControl("lblFreeQty")).Text, RefType.ToString(), RefId.ToString(), SOId.ToString(), ((Label)GridRow.FindControl("lbltaxpercentage")).Text, ((Label)GridRow.FindControl("lbltaxvalue")).Text, ((Label)GridRow.FindControl("lblNetprice")).Text, ((Label)GridRow.FindControl("lblDiscountPercentage")).Text, ((Label)GridRow.FindControl("lblDiscountvalue")).Text, ((Label)GridRow.FindControl("lblNetprice")).Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax("GvQuotationProductEdit", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow, ref trns);
                    }
                }
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            ViewState["dtProduct"] = null;
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            fillGrid(1);
            Reset();

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
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();
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
    protected void btnBin_Click(object sender, EventArgs e)
    {
        fillGridBin();
    }
    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {

        gvAddQuotationGrid.DataSource = null;
        gvAddQuotationGrid.DataBind();
        gvAddSelesOrder.DataSource = null;
        gvAddSelesOrder.DataBind();
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        GvQuotationProductEdit.DataSource = null;
        GvQuotationProductEdit.DataBind();
        GvSalesOrderDetail.DataSource = null;
        GvSalesOrderDetail.DataBind();
        gvQuatationProduct.DataSource = null;
        gvQuatationProduct.DataBind();
        gvPayment.DataSource = null;
        gvPayment.DataBind();
        btnAddtoList.Visible = false;
        btnAddProductScreen.Visible = false;
        rbtnAdvancesearchView.Checked = false;

        if (ddlOrderType.SelectedIndex == 0)
        {
            ddlTransType.Enabled = true;
            PnlReference.Visible = false;
            btnCompareQuatation.Visible = false;
            btnAddProduct.Visible = true;
            txtReferenceNo.Text = "";
            txtVendorQNo.Text = "";
            txtSupplierName.Text = "";
            //code for advance search
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            rbtnSalesOrder.Visible = true;
            Session["DtSearchProduct"] = null;
            ViewState["dtProduct"] = null;
            trVQ.Visible = false;
            //end code
        }
        else
        {
            ddlTransType.Enabled = false;
            pnlProduct1.Visible = false;
            btnAddProduct.Visible = false;
            PnlReference.Visible = true;
            //code for advance search
            rbtnFormView.Visible = false;
            rbtnAdvancesearchView.Visible = false;
            rbtnSalesOrder.Visible = false;
            div_salesOrder.Visible = false;
            Session["DtSearchProduct"] = null;
            ViewState["dtProduct"] = null;
            //end code
            if (ddlReferenceVoucherType.SelectedIndex == 0)
            {
                Session["ReferenceType"] = "PQ";
                btnCompareQuatation.Visible = true;
            }
            else
            {
                Session["ReferenceType"] = null;
            }
            trVQ.Visible = true;
        }
    }
    protected void txtShippingLine_TextChanged(object sender, EventArgs e)
    {
        lblshipingLineMobileNo.Text = "";
        lblShipingEmailId.Text = "";
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
                else
                {
                    string RefName = ObjAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", ContactId.ToString()).Rows[0]["Address_Name"].ToString();
                    DataTable dt = ObjAdd.GetAddressDataByAddressName(RefName.ToString());
                    if (dt.Rows.Count != 0)
                    {
                        string Temp = dt.Rows[0]["MobileNo1"].ToString();
                        Temp = Temp != "" ? dt.Rows[0]["MobileNo1"].ToString() : "No Mobile No";
                        lblshipingLineMobileNo.Text = Resources.Attendance.Mobile_No_1 + " : " + Temp.Trim();
                        Temp = dt.Rows[0]["EmailId1"].ToString();
                        Temp = Temp != "" ? dt.Rows[0]["EmailId1"].ToString() : "No Email Id";
                        lblShipingEmailId.Text = Resources.Attendance.Email_Id + " : " + Temp.Trim();
                        Temp = null;
                    }
                    dt = null;
                }
            }
            catch
            {
            }
        }
    }
    protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPurchaseOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterPurchaseOrder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPurchaseOrder, dt, "", "");
        //AllPageCode();
        gvPurchaseOrder.BottomPagerRow.Focus();
        dt = null;
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
        fillGrid(Int32.Parse(hdngvPurchaseOrderCurrentPageIndex.Value));
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlFieldName.SelectedItem.Value == "PODate" || ddlFieldName.SelectedItem.Value == "DeliveryDate")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString("dd-MMM-yyyy");
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
        fillGrid(1);
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
        txtValueDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        fillGrid(1);
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtPOdate.Focus();
        ddlCurrency_SelectedIndexChanged(null, null);

        if (Lbl_Tab_New.Text == "View")
        {
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        else
        {
            btnReset.Visible = true;
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        resetVariables();
        bool isTaxApplicable = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (Convert.ToBoolean(Session["Is_Tax_Apply"].ToString()) != isTaxApplicable)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Tax is not enabled on this location do you want to continue ?');", true);
            return;
        }
        hdnLocationID.Value = e.CommandName.ToString();

        Session["Temp_Product_Tax_PO"] = null;
        ddlTransType.Enabled = false;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        if (objSenderID != "lnkViewDetail")
        {
            DataTable DtPInvoicedetail = objPInvoiceDetail.GetPurchaseInvoiceDetailAllTrue(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());

            if (DtPInvoicedetail.Rows.Count > 0)
            {
                DisplayMessage("Purchase Invoice has Created,so you can edit only shipping information !");
                pnlOrderInfo.Enabled = false;
                TabProductSupplier.Enabled = false;
                TabAdvancePayment.Enabled = false;
            }
            else
            {
                pnlOrderInfo.Enabled = true;
                TabProductSupplier.Enabled = true;
                TabAdvancePayment.Enabled = true;
            }
            DtPInvoicedetail = null;
        }
        DataTable dt = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (objSenderID != "lnkViewDetail")
            {
                string isApproved = objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (isApproved != "")
                {
                    if (Convert.ToBoolean(isApproved) == true)
                    {
                        if (dt.Rows[0]["Field4"].ToString() == "Approved")
                        {
                            DisplayMessage("Order Approved , cannot be Edited");
                            return;
                        }
                    }
                }
            }
            if (dt.Rows[0]["Trans_Type"].ToString() != "")
                ddlTransType.SelectedValue = dt.Rows[0]["Trans_Type"].ToString();
            else
                ddlTransType.SelectedIndex = 0;
            Hdn_Edit_ID.Value = e.CommandArgument.ToString();
            HdnEdit.Value = e.CommandArgument.ToString();
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                DataTable Dt_Final_Save_Tax = Session["Dt_Final_Save_Tax"] as DataTable;
                Dt_Final_Save_Tax = null;
                Dt_Final_Save_Tax = new DataTable();
                Dt_Final_Save_Tax.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
                Dt_Final_Save_Tax = objTaxRefDetail.Get_Tax_Detail_For_Expenses("PQ", HdnEdit.Value, "Purchase_Quotation", "Single", "1");
                Dt_Final_Save_Tax = null;
            }
            Get_Tax_From_DB();
            txtPOdate.Text = Convert.ToDateTime(dt.Rows[0]["PODate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtPoNo.Text = dt.Rows[0]["PONo"].ToString();
            ddlOrderType.SelectedValue = dt.Rows[0]["OrderType"].ToString();
            ddlOrderType.Enabled = false;
            try
            {
                txtSupplierName.Text = ObjContactMaster.GetContactTrueById(dt.Rows[0]["SupplierId"].ToString()).Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["SupplierId"].ToString();
                ViewState["SuplierId"] = dt.Rows[0]["SupplierId"].ToString();
                fillSupplierCurrency(dt.Rows[0]["SupplierId"].ToString());
                ddlCurrency.SelectedValue = dt.Rows[0]["CurrencyId"].ToString();
            }
            catch
            {
            }
            GetSupplierCreditInfo();
            txtDeliveryDate.Text = Convert.ToDateTime(dt.Rows[0]["DeliveryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ddlPaymentMode.SelectedValue = dt.Rows[0]["PaymentModeId"].ToString();
            ViewState["DefaultCurrency"] = dt.Rows[0]["CurrencyId"].ToString();
            txtCurrencyRate.Text = GetAmountDecimal(dt.Rows[0]["CurrencyRate"].ToString());
            txRemark.Text = dt.Rows[0]["Remark"].ToString();
            ViewState["TimeStamp"] = dt.Rows[0]["Row_Lock_Id"].ToString();
            txtVendorQNo.Text = dt.Rows[0]["Field1"].ToString();
            txtNetDutyPer.Text = dt.Rows[0]["Field2"].ToString();
            txtNetDutyVal.Text = dt.Rows[0]["Field3"].ToString();
            ViewState["Status"] = dt.Rows[0]["Field4"].ToString();
            try
            {
                txtShippingLine.Text = ObjContactMaster.GetContactTrueById(dt.Rows[0]["ShippingLine"].ToString()).Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["ShippingLine"].ToString();
                txtShippingLine_TextChanged(null, null);
            }
            catch
            {
            }
            //for ship to name and ship to address value 
            if (dt.Rows[0]["Condition_2"].ToString().Trim() != "0")
            {
                txtShipSupplierName.Text = dt.Rows[0]["ShipToName"].ToString().Trim() + "/" + dt.Rows[0]["Condition_2"].ToString().Trim();
            }
            //if (dt.Rows[0]["Condition_3"].ToString().Trim() != "0")
            //{
            //    txtShipingAddress.Text = dt.Rows[0]["ShipToAddressName"].ToString().Trim() + "/" + dt.Rows[0]["Condition_3"].ToString().Trim();
            //}

            if (dt.Rows[0]["Condition_3"].ToString().Trim() != "" && dt.Rows[0]["ShipToAddressName"].ToString().Trim() != "")
            {
                txtShipingAddress.Text = dt.Rows[0]["ShipToAddressName"].ToString().Trim();
            }

            try
            {
                ddlprojectname.Value = dt.Rows[0]["Condition_4"].ToString().Trim();
            }
            catch
            {
                ddlprojectname.SelectedIndex = 0;
            }
            ddlShipBy.SelectedValue = dt.Rows[0]["ShipBy"].ToString();
            ddlShipmentType.SelectedValue = dt.Rows[0]["ShipmentType"].ToString();
            ddlFreightStatus.SelectedValue = dt.Rows[0]["Freight_Status"].ToString();
            try
            {
                ddlShipUnit.SelectedValue = dt.Rows[0]["ShipUnit"].ToString();
            }
            catch
            {
                ddlShipUnit.SelectedIndex = 0;
            }
            txtAirwaybillno.Text = dt.Rows[0]["Airway_Bill_No"].ToString();
            txtvolumetricweight.Text = dt.Rows[0]["Volumetric_weight"].ToString();
            //get ship paid amount information
            DataTable dtshipexpenses = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HdnEdit.Value.ToString(), "PO");
            if (dtshipexpenses.Rows.Count > 0)
            {
                ddlExpense.SelectedValue = dtshipexpenses.Rows[0]["Expense_Id"].ToString();
                txtExpensesAccount.Text = dtshipexpenses.Rows[0]["AccountName"].ToString() + "/" + dtshipexpenses.Rows[0]["Account_No"].ToString();
                txtShippingAcc.Text = dtshipexpenses.Rows[0]["ShippingAccountName"].ToString() + "/" + dtshipexpenses.Rows[0]["Field2"].ToString();
                txtpaidamount.Text = dtshipexpenses.Rows[0]["Exp_Charges"].ToString();
            }
            //fill payment grid
            //code start

            //code end
            txtTotalWeight.Text = dt.Rows[0]["TotalWeight"].ToString();
            txtUnitRate.Text = GetAmountDecimal(dt.Rows[0]["UnitRate"].ToString());
            txtReceivingDate.Text = Convert.ToDateTime(dt.Rows[0]["DateReceiving"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtShippingDate.Text = Convert.ToDateTime(dt.Rows[0]["DateShipping"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ddlPartialShipment.SelectedValue = dt.Rows[0]["PartialShipment"].ToString();
            txtDesc.Content = dt.Rows[0]["Condition_1"].ToString();
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
            txtPoNo.Enabled = false;
            Session["OrderType"] = dt.Rows[0]["ReferenceVoucherType"].ToString();
            //   fillgridDetail();
            try
            {
                ddlReferenceVoucherType.SelectedValue = dt.Rows[0]["ReferenceVoucherType"].ToString();
            }
            catch
            {
            }
            if (ddlOrderType.SelectedValue == "R")
            {
                ddlOrderType_SelectedIndexChanged(null, null);
                btnAddProduct.Visible = false;
                ddlReferenceVoucherType.Enabled = false;
                txtReferenceNo.Enabled = false;
                ddlOrderType.Enabled = false;
            }
            //for arcawing
            //start
            txtPOdate.Focus();
            ddlCurrency_SelectedIndexChanged(null, null);
            //GridCalculation();
            fillPaymentGrid(ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "PO", HdnEdit.Value.ToString()));
            try
            {
                if (dt.Rows[0]["ReferenceVoucherType"].ToString() != "SO")
                {
                    txtReferenceNo.Text = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dt.Rows[0]["ReferenceId"].ToString()).Rows[0]["RPQ_No"].ToString();
                    rbtNew.Visible = true;
                    rbtEdit.Visible = true;

                }
                else
                {

                    rbtNew.Visible = false;
                    rbtEdit.Visible = false;
                    txtReferenceNo.Text = ObjSalesOrder.GetSOHeaderAllByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dt.Rows[0]["SalesOrderId"].ToString()).Rows[0]["SalesOrderNo"].ToString();
                }
            }
            catch
            {
                if (dt.Rows[0]["ReferenceVoucherType"].ToString() != "SO")
                {
                    rbtNew.Visible = true;
                    rbtEdit.Visible = true;
                }
            }

            rbtNew.Checked = false;
            rbtEdit.Checked = true;


            try
            {
                ViewState["SOId"] = dt.Rows[0]["SalesOrderId"].ToString();
                ViewState["RefId"] = dt.Rows[0]["ReferenceId"].ToString();
                Session["RPQ_No"] = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dt.Rows[0]["ReferenceId"].ToString()).Rows[0]["RPQ_No"].ToString(); ;
            }
            catch
            {
            }

            txtAdvancePer.Text = string.IsNullOrEmpty(dt.Rows[0]["field5"].ToString()) ? "0" : dt.Rows[0]["field5"].ToString();

            btnAddProduct.Visible = true;
            dtshipexpenses = null;
        }
        dt = null;
        if (objSenderID == "lnkViewDetail")
        {
            btnSave.Enabled = false;
            btnReset.Visible = false;
        }
        else
        {
            btnSave.Enabled = true;
            btnReset.Visible = true;
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please Select one location to continue");
            ddlLocation.Focus();
            return;
        }
        string purInvCount = "0";
        purInvCount = objDa.get_SingleValue("select count(transId) as Count from Inv_PurchaseInvoiceDetail where POId='" + e.CommandArgument.ToString() + "'");
        purInvCount = purInvCount == "@NOTFOUND@" ? "0" : purInvCount;
        if (purInvCount != "0")
        {
            DisplayMessage("Purchase Invoice has Created,So this Record cannot be Deleted");
            return;
        }
        StrLocationId = ddlLocation.SelectedValue;
        string hasPermission = "";
        hasPermission = objDa.get_SingleValue("Select ParameterValue From Inv_ParameterMaster where Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id ='" + Session["BrandId"].ToString() + "' AND Location_Id = '" + StrLocationId + "'  and ParameterName='PurchaseOrderApproval' and IsActive='True'");
        hasPermission = hasPermission == "@NOTFOUND" ? "" : hasPermission;
        if (hasPermission.ToLower() == "true")
        {
            string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
            if (st == "Approved")
            {
                DisplayMessage("Order has not Approved, So Cannot be Deleted");
                return;
            }
        }
        //Delete Concept in Vouchers if Already Exists
        string strCheckApproval = string.Empty;
        if (e.CommandArgument.ToString() != "" && e.CommandArgument.ToString() != "0")
        {
            //DataTable dtFinance = objVoucherHeader.GetVoucherHeaderAllTrueOnly(StrCompId, StrBrandId, StrLocationId, Session["FinanceYearId"].ToString());
            //DataTable dtNotPosted = new DataView(dtFinance, "ReconciledFromFinance='False' and Ref_Type='PO' and Ref_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //DataTable dtPosted = new DataView(dtFinance, "ReconciledFromFinance='True' and Ref_Type='PO' and Ref_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            DataTable dtFinance = objDa.return_DataTable("select trans_id,field3,ReconciledFromFinance from Ac_Voucher_Header where company_Id='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id='" + StrLocationId + "' and Finance_Code='" + Session["FinanceYearId"].ToString() + "' and Ref_Id='" + e.CommandArgument.ToString() + "' and Ref_Type='po'");

            if (dtFinance.Rows.Count > 0)
            {
                DataTable dtNotPosted = new DataView(dtFinance, "ReconciledFromFinance='False'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtPosted = new DataView(dtFinance, "ReconciledFromFinance='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtNotPosted.Rows.Count > 0)
                {
                    string strTransId = dtNotPosted.Rows[0]["Trans_Id"].ToString();
                    strCheckApproval = dtNotPosted.Rows[0]["Field3"].ToString();
                    if (strCheckApproval == "Pending")
                    {
                    }
                    else if (strCheckApproval == "Approved")
                    {
                        DisplayMessage("Your Record was Approved From Approval Regarding Finance Section So you cant Delete");
                        txtReceivingDate.Focus();
                        return;
                    }

                }
                else if (dtPosted.Rows.Count > 0)
                {
                    DisplayMessage("Your Finance Record was Effected So you cant Delete");
                    return;
                }

                dtNotPosted = null;
                dtPosted = null;
            }
            dtFinance = null;
        }
        int b = 0;
        b = ObjPurchaseOrder.DeletePurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString(), false.ToString(), UserId.ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            objEmpApproval.Delete_Approval_Transaction("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "0", e.CommandArgument.ToString());
            try
            {
                fillGrid(Convert.ToInt32(hdngvPurchaseOrderCurrentPageIndex.Value));
            }
            catch
            {
                fillGrid();
            }
            //fillGridBin();
            //updated By jitendra upadhyay on 08-Jan-2014 to update the status in purchase Inquiry table
            string QuotationId = string.Empty;
            DataTable DtPOHeader = ObjPurchaseOrder.GetPurchaseOrderHeader(StrCompId, StrBrandId, StrLocationId);
            try
            {
                DtPOHeader = new DataView(DtPOHeader, "TransID=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtPOHeader.Rows[0]["ReferenceVoucherType"].ToString() == "PQ")
            {
                QuotationId = DtPOHeader.Rows[0]["ReferenceID"].ToString();
                DataTable DtPoHeaderAll = ObjPurchaseOrder.GetPurchaseOrderTrueAll(StrCompId, StrBrandId, StrLocationId);
                try
                {
                    DtPoHeaderAll = new DataView(DtPoHeaderAll, "ReferenceVoucherType='PQ' and ReferenceID='" + QuotationId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtPoHeaderAll.Rows.Count > 0)
                {
                }
                else
                {
                    string PinquiryNo = string.Empty;
                    DataTable DtQuotation = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, QuotationId);
                    if (DtQuotation.Rows.Count > 0)
                    {
                        PinquiryNo = DtQuotation.Rows[0]["PI_No"].ToString();
                    }
                    ObjPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, PinquiryNo, "Quotation Come From Supplier", Session["UserId"].ToString(), DateTime.Now.ToString());
                    DtQuotation = null;
                }
                DtPoHeaderAll = null;
            }
            DtPOHeader = null;
            DisplayMessage("Record Deleted");
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ViewState["dtProduct"] = null;
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["dtProduct"] = null;
        Session["DtSearchProduct"] = null;
        Reset();
        gvProduct.Columns[0].Visible = true;
        gvProduct.Columns[1].Visible = true;
        GvQuotationProductEdit.Columns[0].Visible = true;
        GvQuotationProductEdit.Columns[1].Visible = true;
        btnRefresh_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void txtPoNo_TextChanged(object sender, EventArgs e)
    {
        if (txtPoNo.Text != "")
        {
            DataTable dt = new DataView(ObjPurchaseOrder.GetPurchaseOrderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString()), "PONo='" + txtPoNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    DisplayMessage("Purchase Order No Already Exist");
                    txtPoNo.Text = "";
                    txtPoNo.Focus();
                }
                else
                {
                    DisplayMessage("Purchase Order No Already Exist :- Go To Bin");
                    txtPoNo.Text = "";
                    txtPoNo.Focus();
                }
            }
            else
            {
                ddlOrderType.Focus();
            }
            dt = null;
        }
    }
    protected void btnProductClose_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
    }
    protected void btnPDEdit_Command(object sender, CommandEventArgs e)
    {
        hidProduct.Value = e.CommandArgument.ToString();
        DataTable dt = new DataView(((DataTable)ViewState["dtProduct"]), "Trans_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Product_Id"].ToString() != "0")
            {
                txtProductName.Text = ProductName(dt.Rows[0]["Product_Id"].ToString());
                txtProductcode.Text = ProductCode(dt.Rows[0]["Product_Id"].ToString());
            }
            else
            {
                txtProductName.Text = dt.Rows[0]["ProductDescription"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
            }
            FillUnit(dt.Rows[0]["Product_Id"].ToString());
            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
            txtfreeQty.Text = dt.Rows[0]["FreeQty"].ToString();
            txtOrderQty.Text = dt.Rows[0]["OrderQty"].ToString();
            txtUnitCost.Text = GetAmountDecimal(dt.Rows[0]["UnitCost"].ToString());
        }
        pnlAddProductDetail.Visible = true;
        PnlQuatationGrid.Visible = false;
        pnlProduct1.Visible = true;
        txtProductcode.Focus();
        dt = null;
    }
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["dtProduct"]), "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (HdnEdit.Value != "" && ddlOrderType.SelectedIndex > 0)
        {
            ObjPurchaseOrderDetail.DeletePurchaseOrderDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "", Session["UserId"].ToString(), DateTime.Now.ToString());
            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PO", HdnEdit.Value.ToString(), e.CommandArgument.ToString());
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        //AllPageCode();
        ViewState["dtProduct"] = dt;
        GridCalculation();
        if (dt.Rows.Count == 0)
            ddlTransType.Enabled = true;
        dt = null;
    }
    protected void IbtnPQDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["dtProduct"]), "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (HdnEdit.Value != "")
        {
            ObjPurchaseOrderDetail.DeletePurchaseOrderDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvQuotationProductEdit, dt, "", "");
        GridCalculation();
        //AllPageCode();
        ViewState["dtProduct"] = dt;
        dt = null;
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetDetail();
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        //if (txtSupplierName.Text == "")
        //{
        //    DisplayMessage("Enter Supplier Name");
        //    txtSupplierName.Focus();
        //    return;
        //}
        if (ddlOrderType.SelectedValue == "D")
        {
            pnlAddProductDetail.Visible = true;
            PnlQuatationGrid.Visible = false;
        }
        else
        {
            if (HdnEdit.Value != "")
            {
                string Id = "0";
                if (ViewState["RefId"].ToString() == null)
                    Id = "0";
                else
                    Id = ViewState["RefId"].ToString();
                DataTable dtPQDetail = new DataTable();
                if (Id != "0")
                {
                    dtPQDetail = new DataView(ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Supplier_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvAddQuotationGrid, dtPQDetail, "", "");
                    Add_Quotation_Tax_On_Edit(Id.ToString(), txtSupplierName.Text.Split('/')[1].ToString());
                    gvAddSelesOrder.DataSource = null;
                    gvAddSelesOrder.DataBind();
                }
                else
                {
                    dtPQDetail = ObjSalesOrderDetail.GetSelesOrderDetailNotInPoDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), ViewState["SOId"].ToString());
                    gvAddQuotationGrid.DataSource = null;
                    gvAddQuotationGrid.DataBind();
                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvAddSelesOrder, dtPQDetail, "", "");
                    // gvAddSelesOrder.Columns[10].Visible = false;
                    FillddlUnitInSalesOrderGrid(gvAddSelesOrder);
                }
                if (dtPQDetail.Rows.Count != 0)
                {
                    pnlAddProductDetail.Visible = false;
                    PnlQuatationGrid.Visible = true;
                }
                else
                {
                    DisplayMessage("Product Not Found");
                    return;
                }
                dtPQDetail = null;
            }
        }
        pnlProduct1.Visible = true;
        txtProductcode.Focus();
        Session["DtSearchProduct"] = null;
        ResetDetail();
    }
    protected void GvQuotationProductEdit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[13].Visible = false;
        }
    }
    protected void GvQuotationProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[12].Visible = false;
        }
    }
    protected void gvQuotationProducteditView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[10].Visible = false;
        }
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());
                if (dt == null)
                {
                    DisplayMessage("Product Not Found");
                    txtProductName.Text = "";
                    txtProductName.Focus();
                    return;
                }
                if (dt.Rows.Count != 0)
                {
                    txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                    //this code for get price according selected supplier
                    //code start
                    if (txtSupplierName.Text != "")
                    {
                        //DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S");
                        //try
                        //{
                        //    dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        //catch
                        //{
                        //}
                        //if (dtContactPriceList.Rows.Count > 0)
                        //{
                        //    txtUnitCost.Text = GetAmountDecimal(dtContactPriceList.Rows[0]["Sales_Price"].ToString());
                        string cost = objSupplierPriceList.GetContactPriceByProductID(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S", dt.Rows[0]["ProductId"].ToString());

                        if (cost != "")
                        {
                            txtUnitCost.Text = GetAmountDecimal(cost);

                            try
                            {
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                                txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(strCurrencyId.ToString(), txtUnitCost.Text);
                            }
                            catch
                            {
                                txtUnitCost.Text = "0";
                            }
                        }
                        else
                        {
                            try
                            {
                                txtUnitCost.Text = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                                txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, txtUnitCost.Text);
                            }
                            catch
                            {
                                txtUnitCost.Text = "0";
                            }
                        }
                    }
                    else
                    {
                        //if supplier not selecte dthen we will get last price according selected product 
                        try
                        {
                            txtUnitCost.Text = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                            txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                            txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, txtUnitCost.Text);
                        }
                        catch
                        {
                            txtUnitCost.Text = "0";
                        }
                    }
                    //code end
                    FillUnit(dt.Rows[0]["ProductId"].ToString());
                    txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                    txtOrderQty.Text = "1";
                    txtfreeQty.Text = "0";
                }
                else
                {
                    FillUnit("0");
                    txtPDescription.Text = "";
                    txtOrderQty.Text = "1";
                    txtfreeQty.Text = "0";
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductName.Focus();
                }
                ddlUnit.Focus();
                dt = null;
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductcode.Text.ToString());
                if (dt == null)
                {
                    DisplayMessage("Product Not Found");
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;
                }
                if (dt.Rows.Count != 0)
                {
                    txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                    //this code for get price according the selected supplier
                    //code start
                    if (txtSupplierName.Text != "")
                    {
                        //DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S");
                        //try
                        //{
                        //    dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        //catch
                        //{
                        //}
                        //if (dtContactPriceList.Rows.Count > 0)
                        //{
                        //    txtUnitCost.Text = GetAmountDecimal(dtContactPriceList.Rows[0]["Sales_Price"].ToString());
                        string cost = objSupplierPriceList.GetContactPriceByProductID(StrCompId, txtSupplierName.Text.Split('/')[1].ToString(), "S", dt.Rows[0]["ProductId"].ToString());

                        if (cost != "")
                        {
                            txtUnitCost.Text = GetAmountDecimal(cost);

                            try
                            {
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                                txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, txtUnitCost.Text);
                            }
                            catch
                            {
                                txtUnitCost.Text = "0";
                            }
                        }
                        else
                        {
                            try
                            {
                                txtUnitCost.Text = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                                txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                                txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, txtUnitCost.Text);
                            }
                            catch
                            {
                                txtUnitCost.Text = "0";
                            }
                        }
                    }
                    else
                    {
                        //if supplier not selecte dthen we will get last price according selected product 
                        try
                        {
                            txtUnitCost.Text = (Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[0]["ProductId"].ToString()).Rows[0]["Field1"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString(); ;
                            txtUnitCost.Text = (Convert.ToDouble(txtUnitCost.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                            txtUnitCost.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, txtUnitCost.Text);
                        }
                        catch
                        {
                            txtUnitCost.Text = "0";
                        }
                    }
                    //code end
                    FillUnit(dt.Rows[0]["ProductId"].ToString());
                    txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                    txtOrderQty.Text = "1";
                    txtfreeQty.Text = "0";
                }
                else
                {
                    FillUnit("0");
                    txtPDescription.Text = "";
                    txtOrderQty.Text = "1";
                    txtfreeQty.Text = "0";
                    txtProductcode.Text = "";
                    txtProductName.Text = "";
                    txtProductName.Focus();
                }
                dt = null;
                ddlUnit.Focus();
            }
            catch
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
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

    //This Event Add by Rahul Sharama on date:26-06-2024

    protected void btnGvProductRefresh_Click(object sender, EventArgs e)
    {
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        ViewState["dtProduct"] = null;
        DisplayMessage("Detail Delete Successfully");
    }

    protected void btnDownloadProduct_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("productCode");
        dt.Columns.Add("orderQty");
        dt.Columns.Add("freeQty");
        dt.Columns.Add("unit");
        dt.Columns.Add("Unit_Price");
        dt.Columns.Add("discountPer");
        dt.Columns.Add("Model_Name");
        dt.Columns.Add("ColorName");
        dt.Columns.Add("SizeName");
        foreach (GridViewRow gvr in gvProduct.Rows)
        {
            Label lblproductcode = (Label)gvr.FindControl("lblgvProductId");
            Label lblUnit = (Label)gvr.FindControl("lblUnit");
            Label lblOrderQty = (Label)gvr.FindControl("lblReqQty");
            Label lblFreeQty = (Label)gvr.FindControl("lblFreeQty");
            TextBox lblUnitRate = (TextBox)gvr.FindControl("lblUnitRate");
            TextBox lblDiscount = (TextBox)gvr.FindControl("lblDiscount");
            // Create a new row in DataTable
            DataRow dr = dt.NewRow();

            // Populate the DataRow with values from GridViewRow controls
            dr["productCode"] = lblproductcode.Text;
            dr["orderQty"] = lblOrderQty.Text;
            dr["freeQty"] = lblFreeQty.Text;
            dr["unit"] = lblUnit.Text;
            dr["Unit_Price"] = lblUnitRate.Text; // Assuming TextBox is used for rate
            dr["discountPer"] = lblDiscount.Text; // Assuming TextBox is used for discount
            DataTable dtProduct = objDa.return_DataTable("Select  SSM.Size_Code,SCM.Color_Code,IMM.Model_No from Inv_ProductMaster as IPM inner join Inv_ModelMaster as IMM on IMM.Trans_Id=IPM.ModelNo inner join Set_SizeMaster as SSM on SSM.Trans_Id=IPM.SizeId inner join Set_ColorMaster as SCM on SCM.Trans_Id=IPM.ColourId where ProductCode='" + lblproductcode.Text.ToString() + "'");
            if (dtProduct.Rows.Count > 0)
            {
                dr["Model_Name"] = dtProduct.Rows[0]["Model_No"].ToString(); // You need to add logic to get Model_No, ColorCode, SizeCode
                dr["ColorName"] = dtProduct.Rows[0]["Color_Code"].ToString();
                dr["SizeName"] = dtProduct.Rows[0]["Size_Code"].ToString();
            }
            else
            {
                dr["Model_Name"] = ""; // You need to add logic to get Model_No, ColorCode, SizeCode
                dr["ColorName"] = "";
                dr["SizeName"] = "";
            }
            // Add the DataRow to DataTable
            dt.Rows.Add(dr);

        }
        //gvProduct.DataSource = null;
        //gvProduct.DataBind();
        ExportTableData(dt);

    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "Sheet1";
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
    protected void txtFreeQty_TextChanged(object sender, EventArgs e)
    {
        TextBox TxtBox = (TextBox)((GridViewRow)((TextBox)sender).Parent.Parent).FindControl("txtFreeQty");
        if (TxtBox.Text == "")
        {
            TxtBox.Text = "0";
            TxtBox.Focus();
        }
    }
    protected void lblRemainQty_TextChanged(object sender, EventArgs e)
    {
        TextBox TxtBox = (TextBox)((GridViewRow)((TextBox)sender).Parent.Parent).FindControl("txtRemainQty");
        if (TxtBox.Text == "")
        {
            TxtBox.Text = "0";
            TxtBox.Focus();
        }
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        string OldCurrencyRate = string.Empty;
        OldCurrencyRate = txtCurrencyRate.Text;
        if (OldCurrencyRate == "")
        {
            OldCurrencyRate = "1";
        }
        if (ddlCurrency.SelectedIndex != 0)
        {
            GetSupplierCreditInfo();
            //try
            //{
            try
            {
                gvProduct.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Unit Cost", Session["DBConnection"].ToString());
            }
            catch
            {

            }
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay

            //if (OldCurrencyRate != "" && OldCurrencyRate != "0")
            //{

            //}
            //else
            //{
            txtCurrencyRate.Text = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            //}

            //txtCurrencyRate.Text = (Convert.ToDouble(obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), strfrom), (Currency)System.Enum.Parse(Currency.GetType(), strto)))).ToString();
            ViewState["DefaultCurrency"] = ddlCurrency.SelectedValue.ToString();
            lblAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Gross_Amount, Session["DBConnection"].ToString());
            lblGrandTotal.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue, Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
        }
        else
        {
            try
            {
                gvProduct.HeaderRow.Cells[8].Text = "Unit Cost";
            }
            catch
            {
            }

        }
        if (HdnEdit.Value == "")
        {
            if (txtSupplierName.Text.Trim() != "")
            {
                try
                {
                    txtSupplierName.Text.Split('/')[1].ToString();
                }
                catch
                {
                    DisplayMessage("Select Supplier Name");
                    txtSupplierName.Text = "";
                    txtSupplierName.Focus();
                    return;
                }
                OnSupplierChanged(true, OldCurrencyRate);
            }
        }
        else
        {
            fillgridDetail();
        }
        setDecimalFormate();
        GridCalculation();
        //AllPageCode();
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text.Trim() != "")
        {
            try
            {
                txtSupplierName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                return;
            }
            fillSupplierCurrency(txtSupplierName.Text.Split('/')[1].ToString());
            //GetSupplierCreditInfo();
            OnSupplierChanged(false, "0");
        }
        setDecimalFormate();
        GridCalculation();
    }
    public void GetSupplierCreditInfo()
    {
        string strOtherAccountID = "0";
        btnShowCreditDetails.Visible = false;
        try
        {
            strOtherAccountID = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
            if (strOtherAccountID == "0")
            {
                return;
            }
            DataTable dtCreditParameter = objCustomerCreditParam.GetSupplierRecord_By_OtherAccountId(strOtherAccountID);
            //dtCreditParameter = new DataView(dtCreditParameter, "RecordType='S'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCreditParameter.Rows.Count > 0)
            {
                btnShowCreditDetails.Visible = true;
                lblCreditLimitValue.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
                try
                {
                    lblCurrencyCreditLimit.Text = ObjCurrencyMaster.GetCurrencyMasterById(dtCreditParameter.Rows[0]["Credit_Limit_Currency"].ToString().Trim()).Rows[0]["Currency_Name"].ToString();
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
            dtCreditParameter = null;
        }
        catch
        {
            btnShowCreditDetails.Visible = false;
        }
    }
    public void OnSupplierChanged(bool IsCurrencyChnaged, string OldCurrencyRate)
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            Label lblProductId = (Label)gvRow.FindControl("lblGvProductId");
            TextBox lblUnitRate = (TextBox)gvRow.FindControl("lblUnitRate");
            DataTable dtContactPriceList = objSupplierPriceList.GetContactPriceList(Session["CompId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), "S");
            try
            {
                dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + lblProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (IsCurrencyChnaged == false)
            {
                if (dtContactPriceList.Rows.Count > 0)
                {
                    lblUnitRate.Text = dtContactPriceList.Rows[0]["Sales_Price"].ToString();
                    try
                    {
                        lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                        lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                        lblUnitRate.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, lblUnitRate.Text);
                    }
                    catch
                    {
                        lblUnitRate.Text = "0";
                    }
                }
                else
                {
                    lblUnitRate.Text = "0";
                }
                dtContactPriceList = null;
            }
            if (IsCurrencyChnaged == true)
            {
                try
                {
                    lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) * Convert.ToDouble(OldCurrencyRate)).ToString();
                    lblUnitRate.Text = (Convert.ToDouble(lblUnitRate.Text) / Convert.ToDouble(txtCurrencyRate.Text)).ToString();
                    lblUnitRate.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, lblUnitRate.Text);
                }
                catch
                {
                    lblUnitRate.Text = "0";
                }
            }
            //here we also update the dynamic table
            //this code is created by jitendra upadhyay on 22-03-2014
            if (ViewState["dtProduct"] != null)
            {
                DataTable Dtproduct = (DataTable)(DataTable)ViewState["dtProduct"];
                for (int i = 0; i < Dtproduct.Rows.Count; i++)
                {
                    if (Dtproduct.Rows[i]["Product_Id"].ToString() == lblProductId.Text)
                    {
                        Dtproduct.Rows[i]["UnitCost"] = lblUnitRate.Text;
                        break;
                    }
                }
                ViewState["dtProduct"] = Dtproduct;
                Dtproduct = null;
            }
        }
        if (ddlOrderType.SelectedIndex != 0)
        {
            DataTable dtPQDetail = new DataTable();
            if (Session["ReferenceType"].ToString() != null)
            {
                try
                {
                    if (Session["ReferenceType"].ToString() == "PQ")
                    {
                        string Id = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, Session["RPQ_No"].ToString().Trim()).Rows[0]["Trans_Id"].ToString();
                        if (ddlReferenceVoucherType.SelectedValue != "SO")
                        {
                            Session["RPQ_No"] = txtReferenceNo.Text;
                        }
                        DataTable dtQuotationDetail = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, Id.ToString());
                        bool st = false;
                        for (int i = 0; i < dtQuotationDetail.Rows.Count; i++)
                        {
                            if (dtQuotationDetail.Rows[i]["Field2"].ToString() == "" || dtQuotationDetail.Rows[i]["Field2"].ToString() == "False")
                            {
                            }
                            else
                            {
                                st = true;
                            }
                        }
                        dtQuotationDetail = null;
                        if (st == false)
                        {
                            string RPQId = Session["RPQ_No"].ToString();
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(RPQId)) + "');", true);
                            txtSupplierName.Text = "";
                            return;
                            //txtSupplierName.Enabled = false;
                        }
                        else
                        {
                            dtPQDetail = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString());
                            dtPQDetail = new DataView(dtPQDetail, "Supplier_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (HdnEdit.Value == "")
                            {
                                ddlCurrency.SelectedValue = dtPQDetail.Rows[0]["Field4"].ToString();
                                txtVendorQNo.Text = dtPQDetail.Rows[0]["Field1"].ToString();
                                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                                objPageCmn.FillData((object)gvQuatationProduct, dtPQDetail, "", "");
                                if (dtPQDetail.Rows.Count > 0)
                                    ddlTransType.SelectedValue = dtPQDetail.Rows[0]["Trans_Type"].ToString();
                                else
                                    ddlTransType.SelectedIndex = 0;
                                Quotation_Tax_According_Reference_No(Id, txtReferenceNo.Text);
                                Get_Tax_From_DB();
                                if (gvQuatationProduct != null && gvQuatationProduct.Rows.Count > 0)
                                {
                                    gvQuatationProduct.HeaderRow.Cells[5].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Cost", Session["DBConnection"].ToString());
                                    gvQuatationProduct.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), Resources.Attendance.Gross_Price, Session["DBConnection"].ToString());
                                    gvQuatationProduct.HeaderRow.Cells[13].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Price", Session["DBConnection"].ToString());
                                    gvQuatationProduct.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
                                    gvQuatationProduct.HeaderRow.Cells[11].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
                                }
                                for (int i = 0; i < gvQuatationProduct.Rows.Count; i++)
                                {
                                    Label lblgvproduct = (Label)gvQuatationProduct.Rows[i].FindControl("lblgvProductId");
                                    Label lbltrans = (Label)gvQuatationProduct.Rows[i].FindControl("lblgvProductName");
                                    if (dtPQDetail.Rows[i]["Field2"].ToString() == "True")
                                    {
                                        CheckBox chkgvproduct = (CheckBox)gvQuatationProduct.Rows[i].FindControl("chk");
                                        chkgvproduct.Checked = true;
                                    }
                                }
                                try
                                {
                                    foreach (GridViewRow gvRow in gvQuatationProduct.Rows)
                                    {
                                        TextBox lblDiscount = (TextBox)gvRow.FindControl("Txt_Discount_Per_Quatation");
                                        TextBox lblDiscountValue = (TextBox)gvRow.FindControl("Txt_DiscountValue_Quatation");
                                        lblDiscount.Text = GetAmountDecimal(lblDiscount.Text);
                                        lblDiscountValue.Text = GetAmountDecimal(lblDiscountValue.Text);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                                objPageCmn.FillData((object)gvAddQuotationGrid, dtPQDetail, "", "");
                                if (gvAddQuotationGrid != null)
                                {
                                    gvAddQuotationGrid.HeaderRow.Cells[5].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Cost", Session["DBConnection"].ToString());
                                    gvAddQuotationGrid.HeaderRow.Cells[13].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Price", Session["DBConnection"].ToString());
                                    gvAddQuotationGrid.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
                                    gvAddQuotationGrid.HeaderRow.Cells[11].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
                                }
                            }
                            foreach (GridViewRow GridRow in gvQuatationProduct.Rows)
                            {
                                TextBox lblgvRequiredQty = (TextBox)GridRow.FindControl("txtgvRequiredQty");
                                TextBox lblgvUnitCost = (TextBox)GridRow.FindControl("txtUnitCost");
                                Label lblgvPrice = (Label)GridRow.FindControl("lblQtyPrice");
                                Label lblgvTaxValue = (Label)GridRow.FindControl("lblTaxValue");
                                Label lblgvTaxafterPrice = (Label)GridRow.FindControl("lblTaxafterPrice");
                                TextBox lblDiscount = (TextBox)GridRow.FindControl("Txt_Discount_Per_Quatation");
                                TextBox lblDiscountValue = (TextBox)GridRow.FindControl("Txt_DiscountValue_Quatation");
                                if (lblgvRequiredQty.Text == "")
                                {
                                    lblgvRequiredQty.Text = "0";
                                }
                                if (lblgvUnitCost.Text == "")
                                {
                                    lblgvUnitCost.Text = "0";
                                }
                                if (lblgvTaxValue.Text == "")
                                {
                                    lblgvTaxValue.Text = "0";
                                }
                                lblgvPrice.Text = (Convert.ToDouble(lblgvUnitCost.Text) * Convert.ToDouble(lblgvRequiredQty.Text)).ToString();
                                lblgvTaxafterPrice.Text = (Convert.ToDouble(lblgvPrice.Text) + Convert.ToDouble(lblgvTaxValue.Text)).ToString();
                                lblDiscount.Text = GetAmountDecimal(lblDiscount.Text);
                                lblDiscountValue.Text = GetAmountDecimal(lblDiscountValue.Text);
                            }
                        }
                    }
                    else
                    {
                        string Id = ObjSalesOrder.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.ToString().Trim()).Rows[0]["Trans_Id"].ToString();
                        dtPQDetail = ObjSalesOrderDetail.GetSelesOrderDetailNotInPoDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString());
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)GvSalesOrderDetail, dtPQDetail, "", "");
                        objPageCmn.FillData((object)gvAddSelesOrder, dtPQDetail, "", "");
                        if (dtPQDetail.Rows.Count > 0)
                            ddlTransType.SelectedValue = dtPQDetail.Rows[0]["Trans_Type"].ToString();
                        else
                            ddlTransType.SelectedIndex = 0;
                        Sales_Order_Tax_According_Reference_No(Id, txtReferenceNo.Text);
                        foreach (GridViewRow gvr in GvSalesOrderDetail.Rows)
                        {
                            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                            TextBox txtgvQuantity = (TextBox)gvr.FindControl("txtgvQuantity");
                            TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
                            TextBox Txt_Discount_Sales = (TextBox)gvr.FindControl("Txt_Discount_Sales");
                            TextBox Txt_Tax_Per_Sales = (TextBox)gvr.FindControl("Txt_Tax_Per_Sales");
                            TextBox Txt_Tax_Value_Sales = (TextBox)gvr.FindControl("Txt_Tax_Value_Sales");
                            Label lblLineTotal = (Label)gvr.FindControl("lblLineTotal");
                            Label Serial_No = (Label)gvr.FindControl("lblgvSerialNo");
                            Txt_Tax_Per_Sales.Text = Get_Tax_Percentage(hdngvProductId.Value, Serial_No.Text).ToString();
                            string[] strcalc = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, txtgvQuantity.Text, Txt_Tax_Per_Sales.Text, "", Txt_Discount_Sales.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());
                            Txt_Tax_Value_Sales.Text = strcalc[4].ToString();
                            lblLineTotal.Text = strcalc[5].ToString();
                        }
                        if (GvSalesOrderDetail != null && GvSalesOrderDetail.Rows.Count > 0)
                        {
                            GvSalesOrderDetail.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Unit Price", Session["DBConnection"].ToString());
                        }
                        if (gvAddSelesOrder != null && gvAddSelesOrder.Rows.Count > 0)
                        {
                            gvAddSelesOrder.HeaderRow.Cells[5].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Unit Price", Session["DBConnection"].ToString());
                        }
                        FillddlUnitInSalesOrderGrid(GvSalesOrderDetail);
                        FillddlUnitInSalesOrderGrid(gvAddQuotationGrid);
                    }
                }
                catch (Exception ex)
                {
                }
                if (gvQuatationProduct.Rows.Count != 0)
                {
                    btnAddProduct.Visible = false;
                }
                else
                {
                    if (HdnEdit.Value != "")
                    {
                        btnAddProduct.Visible = true;
                    }
                }
                if (dtPQDetail.Rows.Count == 0)
                {
                    if (HdnEdit.Value != "")
                    {
                        if (ViewState["SuplierId"].ToString() != txtSupplierName.Text.Trim().Split('/')[1].ToString())
                        {
                            if (gvProduct.Rows.Count == 0)
                            {
                                DisplayMessage("Product Not Found");
                                txtSupplierName.Text = "";
                                GvQuotationProductEdit.Visible = false;
                            }
                        }
                        else
                        {
                            GvQuotationProductEdit.Visible = true;
                        }
                    }
                    else
                    {
                        DisplayMessage("Product Not Found");
                        txtSupplierName.Text = "";
                    }
                }
            }
            dtPQDetail = null;
        }
    }
    protected void btnCompareQuatation_Click(object sender, EventArgs e)
    {
        if (Session["RPQ_No"] != null)
        {
            ViewState["RequestId"] = null;
            Session["Products"] = null;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(Session["RPQ_No"].ToString().Trim())) + "');", true);
        }
    }
    protected void txtReferenceNo_TextChanged(object seder, EventArgs e)
    {
        txtSupplierName.Text = "";
        if (txtReferenceNo.Text.ToString() != "")
        {
            if (ddlReferenceVoucherType.SelectedIndex == 0)
            {
                DataTable dtQuotation = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.Trim());
                ddlCurrency.SelectedValue = dtQuotation.Rows[0]["Field1"].ToString();
                if (dtQuotation.Rows.Count == 0)
                {
                    DisplayMessage("Select Reference No");
                    txtReferenceNo.Text = "";
                    txtReferenceNo.Focus();
                    return;
                }
                else
                {
                    Session["RPQ_No"] = txtReferenceNo.Text;
                    string Transid = dtQuotation.Rows[0]["Trans_Id"].ToString();
                    DataTable dtQuotationDetail = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, Transid.ToString());
                    bool st = false;
                    for (int i = 0; i < dtQuotationDetail.Rows.Count; i++)
                    {
                        if (dtQuotationDetail.Rows[i]["Field2"].ToString() == "" || dtQuotationDetail.Rows[i]["Field2"].ToString() == "False")
                        {
                        }
                        else
                        {
                            st = true;
                        }
                    }
                    if (st == false)
                    {
                        string RPQId = Session["RPQ_No"].ToString();
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(RPQId)) + "');", true);
                    }
                }
                dtQuotation = null;
            }
            else
            {
                DataTable dtSelesOrder = ObjSalesOrder.GetSOHeaderAllBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, txtReferenceNo.Text.Trim());
                if (dtSelesOrder.Rows.Count == 0)
                {
                    DisplayMessage("Select Reference No");
                    txtReferenceNo.Text = "";
                    txtReferenceNo.Focus();
                    return;
                }
                else
                {
                    if (dtSelesOrder.Rows[0]["SOfromTransType"].ToString() == "Q")
                    {
                        try
                        {
                            DataTable dtPQ = objQuoteHeader.GetQuoteHeaderAllDataBySOId(StrCompId, StrBrandId, StrLocationId, dtSelesOrder.Rows[0]["SOfromTransNo"].ToString());
                            if (dtPQ.Rows[0]["Trans_Id"].ToString() != "")
                            {
                                Session["ReferenceType"] = "PQ";
                                ddlCurrency.SelectedValue = dtPQ.Rows[0]["Field1"].ToString();
                                Session["RPQ_No"] = dtPQ.Rows[0]["RPQ_No"].ToString();
                            }
                            dtPQ = null;
                        }
                        catch
                        {
                        }
                    }
                }
                dtSelesOrder = null;
            }
        }
        else
        {
            Session["RPQ_No"] = null;
        }
    }
    protected void ddlReferenceVoucherType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReferenceVoucherType.SelectedIndex == 0)
        {
            ddlTransType.Enabled = false;
            Session["ReferenceType"] = "PQ";
            txtReferenceNo.Text = "";
            btnCompareQuatation.Visible = true;
        }
        else
        {
            Session["ReferenceType"] = "SO";
            txtReferenceNo.Text = "";
            btnCompareQuatation.Visible = false;
        }
    }
    protected void btnAddOrder_Click(object sender, EventArgs e)
    {
        string S = "0";
        int counter = 0;
        if (GvQuotationProductEdit.Rows.Count != 0)
        {
            ObjPurchaseOrderDetail.DeletePurchaseOrderDetailByPONo(StrCompId, StrBrandId, StrLocationId, HdnEdit.Value.Trim());
            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PO", HdnEdit.Value.ToString());
            foreach (GridViewRow GridRow in GvQuotationProductEdit.Rows)
            {
                counter++;
                int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), counter.ToString(), ((Label)GridRow.FindControl("lblGvProductId")).Text, ((Label)GridRow.FindControl("lblProductId")).Text, ((Label)GridRow.FindControl("lblGvUnitId")).Text, ((TextBox)GridRow.FindControl("txtUnitRate")).Text, ((TextBox)GridRow.FindControl("txtReqQty")).Text, "0", ((Label)GridRow.FindControl("lblFreeQty")).Text, ddlReferenceVoucherType.SelectedValue.ToString(), ViewState["RefId"].ToString().Trim(), S.ToString(), ((Label)GridRow.FindControl("lbltaxpercentage")).Text, ((Label)GridRow.FindControl("lbltaxvalue")).Text, ((Label)GridRow.FindControl("lblNetprice")).Text, ((Label)GridRow.FindControl("lblDiscountPercentage")).Text, ((Label)GridRow.FindControl("lblDiscountvalue")).Text, ((Label)GridRow.FindControl("lblNetprice")).Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                Insert_Tax("GvQuotationProductEdit", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow);
            }
        }
        foreach (GridViewRow GridRow in gvAddQuotationGrid.Rows)
        {
            CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("chk");
            Label lblgvProductId = (Label)GridRow.FindControl("lblgvProductId");
            Label lblgvProductDes = (Label)GridRow.FindControl("lblgvProductName");
            Label lblgvUnitId = (Label)GridRow.FindControl("lblgvUnitId");
            Label lblgvRequiredQty = (Label)GridRow.FindControl("lblgvRequiredQty");
            Label lblgvUnitCost = (Label)GridRow.FindControl("lblUnitCost");
            TextBox txtgvFreeQty = (TextBox)GridRow.FindControl("txtFreeQty");
            Label lbltaxPercantage = (Label)GridRow.FindControl("lbltaxpercentage");
            Label lbltaxValue = (Label)GridRow.FindControl("lbltaxvalue");
            Label lblPriceaftertax = (Label)GridRow.FindControl("lblPriceaftertax");
            Label lblDiscountPercentage = (Label)GridRow.FindControl("lblDiscountPercentage");
            Label lblDiscountValue = (Label)GridRow.FindControl("lblDiscountvalue");
            Label lblNetPrice = (Label)GridRow.FindControl("lblNetprice");
            TextBox txtgvRemainQty = new TextBox();
            txtgvRemainQty.Text = "0";
            if (lbltaxPercantage.Text == "")
                lbltaxPercantage.Text = "0";
            if (lbltaxValue.Text == "")
                lbltaxValue.Text = "0";
            if (lblPriceaftertax.Text == "")
                lblPriceaftertax.Text = "0";
            if (lblDiscountPercentage.Text == "")
                lblDiscountPercentage.Text = "0";
            if (lblDiscountValue.Text == "")
                lblDiscountValue.Text = "0";
            if (lblNetPrice.Text == "")
                lblNetPrice.Text = "0";
            if (CheckBoxId.Checked)
            {
                counter++;
                int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), counter.ToString(), lblgvProductId.Text, lblgvProductDes.Text, lblgvUnitId.Text, lblgvUnitCost.Text, lblgvRequiredQty.Text, txtgvRemainQty.Text, txtgvFreeQty.Text, ddlReferenceVoucherType.SelectedValue.ToString(), ViewState["RefId"].ToString().Trim(), S.ToString(), lbltaxPercantage.Text, lbltaxValue.Text, lblPriceaftertax.Text, lblDiscountPercentage.Text, lblDiscountValue.Text, lblNetPrice.Text, "0", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                Insert_Tax("gvAddQuotationGrid", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow);
            }
        }
        foreach (GridViewRow GridRow in gvAddSelesOrder.Rows)
        {
            CheckBox CheckBoxId = (CheckBox)GridRow.FindControl("ChkSelect");
            HiddenField lblgvProductId = (HiddenField)GridRow.FindControl("hdngvProductId");
            Label lblgvProductDes = (Label)GridRow.FindControl("lblgvProductName");
            DropDownList ddlgvUnit = (DropDownList)GridRow.FindControl("ddlgvUnit");
            TextBox txtgvRequiredQty = (TextBox)GridRow.FindControl("txtgvQuantity");
            TextBox txtgvUnitCost = (TextBox)GridRow.FindControl("txtgvUnitPrice");
            TextBox txtgvFreeQty = (TextBox)GridRow.FindControl("txtgvFreeQuantity");
            TextBox txtgvRemainQty = new TextBox();
            Label Serial_No = (Label)GridRow.FindControl("lblgvSerialNo");
            txtgvRemainQty.Text = "0";
            if (CheckBoxId.Checked)
            {
                DataTable dt = (DataTable)ViewState["dtProduct"];
                string st = ViewState["SOId"].ToString();
                if (dt != null)
                {
                    int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), (((DataTable)ViewState["dtProduct"]).Rows.Count + 1).ToString(), lblgvProductId.Value.Trim(), lblgvProductDes.Text, ddlgvUnit.SelectedValue.ToString(), txtgvUnitCost.Text, txtgvRequiredQty.Text, txtgvRemainQty.Text, txtgvFreeQty.Text, ddlReferenceVoucherType.SelectedValue.ToString(), "0", ViewState["SOId"].ToString(), "0", "0", "0", "0", "0", "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    Insert_Tax("gvAddSelesOrder", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow);
                }
                else
                {
                    int i = 1;
                    int Detail_ID = ObjPurchaseOrderDetail.InsertPurchaseOrderDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value.ToString(), i.ToString(), lblgvProductId.Value.Trim(), lblgvProductDes.Text, ddlgvUnit.SelectedValue.ToString(), txtgvUnitCost.Text, txtgvRequiredQty.Text, txtgvRemainQty.Text, txtgvFreeQty.Text, ddlReferenceVoucherType.SelectedValue.ToString(), "0", ViewState["SOId"].ToString(), "0", "0", "0", "0", "0", "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    Insert_Tax("gvAddSelesOrder", HdnEdit.Value.ToString(), Detail_ID.ToString(), GridRow);
                }
                Add_Tax_In_Session(txtgvUnitCost.Text, lblgvProductId.Value, Serial_No.Text);
                dt = null;
            }
        }
        fillgridDetail();
        GridCalculation();
        pnlProduct1.Visible = false;
    }
    #region Bin
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if ((ddlbinFieldName.SelectedItem.Value == "PODate") || (ddlbinFieldName.SelectedItem.Value == "DeliveryDate"))
        {
            if (txtbinValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtbinValueDate.Text);
                    txtbinValue.Text = (txtbinValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtbinValueDate.Text = "";
                    txtbinValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtbinValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtbinValueDate.Focus();
                txtbinValue.Text = "";
                return;
            }
        }
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtPOBin = (DataTable)Session["DtBinPurchaseOrder"];
            DataView view = new DataView(dtPOBin, condition, "", DataViewRowState.CurrentRows);
            Session["DtFilterBinPurchaseOrder"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvBinPurchaseOrder, view.ToTable(), "", "");
            dtPOBin = null;
            if (view.ToTable().Rows.Count != 0)
            {
                //AllPageCode();
            }
            // btnbinbind.Focus();
            view = null;
        }
        if (txtbinValue.Text != "")
            txtbinValue.Focus();
        else if (txtbinValueDate.Text != "")
            txtbinValueDate.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        fillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtbinValueDate.Text = "";
        txtbinValue.Visible = true;
        txtbinValueDate.Visible = false;
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void GvBinPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBinPurchaseOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtFilterBinPurchaseOrder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBinPurchaseOrder, dt, "", "");
        //AllPageCode();
        string temp = string.Empty;
        bool isselcted;
        for (int i = 0; i < GvBinPurchaseOrder.Rows.Count; i++)
        {
            Label lblconid = (Label)GvBinPurchaseOrder.Rows[i].FindControl("lblId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvBinPurchaseOrder.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        dt = null;
        GvBinPurchaseOrder.BottomPagerRow.Focus();
    }
    protected void GvBinPurchaseOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtFilterBinPurchaseOrder"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtFilterBinPurchaseOrder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBinPurchaseOrder, dt, "", "");
        //AllPageCode();
        GvBinPurchaseOrder.HeaderRow.Focus();
        dt = null;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvBinPurchaseOrder.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvBinPurchaseOrder.Rows.Count; i++)
        {
            ((CheckBox)GvBinPurchaseOrder.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvBinPurchaseOrder.Rows[i].FindControl("lblId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvBinPurchaseOrder.Rows[i].FindControl("lblId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvBinPurchaseOrder.Rows[i].FindControl("lblId"))).Text.Trim().ToString())
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
        ((CheckBox)GvBinPurchaseOrder.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvBinPurchaseOrder.Rows[index].FindControl("lblId");
        if (((CheckBox)GvBinPurchaseOrder.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)GvBinPurchaseOrder.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPr = (DataTable)Session["DtFilterBinPurchaseOrder"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPr.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["TransId"]))
                {
                    lblSelectedRecord.Text += dr["TransId"] + ",";
                }
            }
            for (int i = 0; i < GvBinPurchaseOrder.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvBinPurchaseOrder.Rows[i].FindControl("lblId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvBinPurchaseOrder.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtPr1 = (DataTable)Session["DtFilterBinPurchaseOrder"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvBinPurchaseOrder, dtPr1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
            dtPr1 = null;
        }
        dtPr = null;
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please Select only one location");
            ddlLocation.Focus();
            return;
        }
        if (Session["LocID"].ToString() != ddlLocation.SelectedValue)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Purchase Order you are trying to restore is on another location, please change the location to continue, do you want to continue ?')", true);
            return;
        }

        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            string isApproved = objInvParam.getParameterValueByParameterName("PurchaseOrderApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            DataTable dt1 = new DataTable();
            string EmpPermission = string.Empty;
            if (isApproved != "")
            {
                if (Convert.ToBoolean(isApproved) == true)
                {
                    EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("PurchaseOrder").Rows[0]["Approval_Level"].ToString();
                    dt1 = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "45", Session["EmpId"].ToString());
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
                    b = ObjPurchaseOrder.DeletePurchaseOderHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    //updated by jitendra upadhyay on 08-jan-2014 for update the status in purchase inquiry table
                    string QuotationId = string.Empty;
                    DataTable DtPOHeader = ObjPurchaseOrder.GetPurchaseOrderHeader(StrCompId, StrBrandId, StrLocationId);
                    try
                    {
                        DtPOHeader = new DataView(DtPOHeader, "TransID=" + lblSelectedRecord.Text.Split(',')[j].Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    //Add Approval New Concept On 08-12-2014
                    if (dt1.Rows.Count > 0)
                    {
                        for (int h = 0; h < dt1.Rows.Count; h++)
                        {
                            string PriorityEmpId = dt1.Rows[h]["Emp_Id"].ToString();
                            string IsPriority = dt1.Rows[h]["Priority"].ToString();
                            if (EmpPermission == "1")
                            {
                                objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "2")
                            {
                                objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "3")
                            {
                                objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else
                            {
                                objEmpApproval.InsertApprovalTransaciton("7", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                        }
                        DtPOHeader = null;
                    }
                    if (DtPOHeader.Rows[0]["ReferenceVoucherType"].ToString() == "PQ")
                    {
                        QuotationId = DtPOHeader.Rows[0]["ReferenceID"].ToString();
                        DataTable DtPoHeaderAll = ObjPurchaseOrder.GetPurchaseOrderTrueAll(StrCompId, StrBrandId, StrLocationId);
                        try
                        {
                            DtPoHeaderAll = new DataView(DtPoHeaderAll, "ReferenceVoucherType='PQ' and ReferenceID='" + QuotationId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        string PinquiryNo = string.Empty;
                        DataTable DtQuotation = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, QuotationId);
                        if (DtQuotation.Rows.Count > 0)
                        {
                            PinquiryNo = DtQuotation.Rows[0]["PI_No"].ToString();
                        }
                        if (DtPoHeaderAll.Rows.Count > 0)
                        {
                            ObjPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, PinquiryNo, "Purchase Order has send", Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else
                        {
                            ObjPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, PinquiryNo, "Quotation Come From Supplier", Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        DtPoHeaderAll = null;
                        DtQuotation = null;
                    }
                }
            }
            dt1 = null;
        }
        if (b != 0)
        {
            fillGrid();
            fillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvBinPurchaseOrder.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
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
        txtbinValue.Focus();
    }
    #endregion
    #region Quotation
    protected void BtnQuotation_Click(object sender, EventArgs e)
    {
        FillGridQuotation();
    }
    protected void GvPurchaseQuote_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtQFilter"];
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
        Session["dtQFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseQuote, dt, "", "");
        dt = null;
    }
    protected void GvPurchaseQuote_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseQuote.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtQFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseQuote, dt, "", "");
        dt = null;
    }
    protected void ImgBtnQBind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "RPQ_Date")
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
            DataTable dtAdd = (DataTable)Session["dtQuotation"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseQuote, view.ToTable(), "", "");
            Session["dtQFilter"] = Session["dtQuotation"];
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            txtQValue.Focus();
            dtAdd = null;
            view = null;
        }
    }
    protected void gvQuatationProduct_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //AllPageCode();
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Product_Description;
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Discount;
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

            string isDiscount = objInvParam.getParameterValueByParameterName("IsDiscount", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (isDiscount != "")
            {
                if (Convert.ToBoolean(isDiscount) == false)
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
            string isTax = objInvParam.getParameterValueByParameterName("IsTax", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (isTax != "")
            {
                if (Convert.ToBoolean(isTax) == false)
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
            cell.Text = "";
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Total;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            gvProduct.Controls[0].Controls.Add(row);

        }
        gvProduct = null;
    }
    protected void btnPurchaseQuote_Command(object sender, CommandEventArgs e)
    {
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please Select only one location");
            ddlLocation.Focus();
            return;
        }
        if (Session["LocID"].ToString() != ddlLocation.SelectedValue)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Purchase Quotation Location does not matches with the login location, please change the location to continue, do you want to continue ?')", true);
            return;
        }
        Hdn_Quot_Id.Value = e.CommandArgument.ToString();
        DataTable dt = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            //start code vs
            DataTable dtQuotationDetail = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
            bool st = false;
            for (int i = 0; i < dtQuotationDetail.Rows.Count; i++)
            {
                if (dtQuotationDetail.Rows[i]["Field2"].ToString() == "" || dtQuotationDetail.Rows[i]["Field2"].ToString() == "False")
                {
                }
                else
                {
                    st = true;
                }
            }
            //dtQuotationDetail = null;
            if (st == false)
            {
                string RPQId = dt.Rows[0]["RPQ_No"].ToString();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(RPQId)) + "');", true);
            }
            else
            {
                //end code vs
                try
                {
                    txtSupplierName.Text = dtQuotationDetail.Rows[0]["SupplierName"].ToString() + "/" + dtQuotationDetail.Rows[0]["Supplier_Id"].ToString();
                    //txtSupplierName.Text = ObjContactMaster.GetContactTrueById(dt.Rows[0]["FinalSupplierId"].ToString()).Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["FinalSupplierId"].ToString();
                }
                catch
                {
                }
                try
                {
                    string POId = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString()).Rows[0]["PI_No"].ToString();
                    try
                    {
                        DataTable dtPI = ObjPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, POId.ToString());
                        if (dtPI.Rows[0]["TransFrom"].ToString() == "SI")
                        {
                            DataTable dtSo = new DataTable();
                            try
                            {
                                dtSo = ObjSalesOrder.GetSOHeaderAllByFromTransType(StrCompId, StrBrandId, StrLocationId, "Q", ObjSalesQuotation.GetQuotationHeaderAllBySInquiry_No(StrCompId, StrBrandId, StrLocationId, dtPI.Rows[0]["TransNo"].ToString()).Rows[0]["SQuotation_ID"].ToString());
                            }
                            catch
                            {
                            }
                            ddlOrderType.SelectedValue = "R";
                            ddlOrderType_SelectedIndexChanged(null, null);
                            if (dtSo.Rows.Count != 0)
                            {
                                ddlReferenceVoucherType.SelectedValue = "SO";
                                txtReferenceNo.Text = dtSo.Rows[0]["SalesOrderNo"].ToString();
                            }
                            else
                            {
                                ddlReferenceVoucherType.SelectedValue = "PQ";
                                txtReferenceNo.Text = dt.Rows[0]["RPQ_No"].ToString();
                                ddlCurrency.SelectedValue = dt.Rows[0]["Field1"].ToString();
                            }
                            Session["RPQ_No"] = dt.Rows[0]["RPQ_No"].ToString();
                            ddlOrderType.Enabled = false;
                            ddlReferenceVoucherType.Enabled = false;
                            txtReferenceNo.ReadOnly = true;
                            dtSo = null;
                        }
                        else
                        {
                            ddlOrderType.SelectedValue = "R";
                            ddlOrderType_SelectedIndexChanged(null, null);
                            ddlReferenceVoucherType.SelectedValue = "PQ";
                            txtReferenceNo.Text = dt.Rows[0]["RPQ_No"].ToString();
                            Session["RPQ_No"] = dt.Rows[0]["RPQ_No"].ToString();
                            ddlCurrency.SelectedValue = dt.Rows[0]["Field1"].ToString();
                            ddlOrderType.Enabled = false;
                            ddlReferenceVoucherType.Enabled = false;
                            txtReferenceNo.ReadOnly = true;
                        }
                        dtPI = null;
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
                txtPOdate.Focus();
                ddlCurrency_SelectedIndexChanged(null, null);
                btnAddProduct.Visible = false;
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Quotation_Active()", true);
            }
        }
        btnSave.Enabled = true;
        btnReset.Visible = true;
        dt = null;
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        BtnQuotation_Click(sender, e);
        ddlQSeleclField.SelectedIndex = 0;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        txtQValue.Visible = true;
        txtQValueDate.Visible = false;
    }
    #endregion
    #region View
    protected void BtnCancelView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    void ViewReset()
    {
        HdnEdit.Value = "";
        Reset();
        lblPOdateView.Text = "";
        lblPONUmberView.Text = "";
        lblOrderTypeView.Text = "";
        lblSupplierNameView.Text = "";
        lblDeliveryDateView.Text = "";
        lblPaymentModeView.Text = "";
        lblCurrencyView.Text = "";
        lblCurrencyRateView.Text = "";
        txtRemarksView.Text = "";
        lblShippingLineView.Text = "";
        lblShipByView.Text = "";
        lblShipmentTypeView.Text = "";
        lblFrieghtStatusView.Text = "";
        lblShipUnitView.Text = "";
        lblTotalWeightview.Text = "";
        lblUnitRateView.Text = "";
        lblReceivingDateView.Text = "";
        lblShippingDateView.Text = "";
        lblPartialShipmentView.Text = "";
        txtCondition1View.Text = "";
        txtCondition2View.Text = "";
        txtCondition3View.Text = "";
        txtCondition4View.Text = "";
        txtCondition5View.Text = "";
        GridViewDirectView.DataSource = null;
        GridViewDirectView.DataBind();
        gvQuotationProducteditView.DataSource = null;
        gvQuotationProducteditView.DataBind();
        lblReferenceVoucherTypeView.Text = "";
        lblReferenceNoView.Text = "";
        lblMobileNoView.Text = "";
        lblEmailIdView.Text = "";
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    #endregion
    #endregion
    #region User Defined Function
    public string GetPurchaseinquiryNo(string PId)
    {
        string PInqNo = "";
        if (PId != "")
        {
            DataTable Dt = ObjPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, PId);
            if (Dt.Rows.Count > 0)
            {
                PInqNo = Dt.Rows[0]["PI_No"].ToString();
            }
            Dt = null;
        }
        return PInqNo;
    }
    public void Reset()
    {
        rbtEdit.Visible = false;
        rbtNew.Visible = false;
        Session["Expenses_Tax_Purchase_Order"] = null;
        Hdn_Quot_Id.Value = "";
        Session["Temp_Product_Tax_PO"] = null;
        txtPOdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtShippingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtDeliveryDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtCurrencyRate.Text = "0";
        FillCurrency();
        ddlPayCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        txtPayExchangeRate.Text = "1";
        ddlUnit.Items.Clear();
        txtPoNo.Text = "";
        txtSupplierName.Text = "";
        ddlPaymentMode.SelectedIndex = 0;
        txRemark.Text = "";
        txtShippingLine.Text = "";
        lblshipingLineMobileNo.Text = "";
        lblShipingEmailId.Text = "";
        ddlShipBy.SelectedIndex = 0;
        txtTotalWeight.Text = "";
        ddlShipmentType.SelectedIndex = 0;
        ddlFreightStatus.SelectedIndex = 1;
        txtUnitRate.Text = "";
        txtVendorQNo.Text = "";
        ddlPartialShipment.SelectedIndex = 0;
        txtDesc.Content = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtPoNo.Enabled = true;

        ddlOrderType.SelectedValue = "D";
        ddlOrderType_SelectedIndexChanged(null, null);
        txtReferenceNo.Text = "";
        ddlReferenceVoucherType.SelectedIndex = 0;
        btnAddProduct.Visible = true;
        HdnEdit.Value = "";
        ddlReferenceVoucherType.Enabled = true;
        txtReferenceNo.Enabled = true;
        ddlOrderType.Enabled = true;
        txtPoNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtPoNo.Text;
        ddlOrderType.Enabled = true;
        ddlReferenceVoucherType.Enabled = true;
        txtReferenceNo.ReadOnly = false;
        Session["RPQ_No"] = null;

        txtbinValueDate.Text = "";
        txtbinValue.Text = "";
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        fillPaymentMode();
        Session["PayementDt"] = null;
        btnPaymentReset_Click(null, null);
        gvPayment.DataSource = null;
        gvPayment.DataBind();
        txtQValueDate.Visible = false;
        txtValueDate.Visible = false;
        txtbinValueDate.Visible = false;
        txtQValue.Visible = true;
        txtValue.Visible = true;
        txtbinValue.Visible = true;
        ViewState["Status"] = null;
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        ViewState["dtProduct"] = null;
        txtGrossAmount.Text = "0";
        txtNetDutyPer.Text = "0";
        txtNetDutyVal.Text = "0";
        txtGrandTotal.Text = "0";
        ddlShipUnit.SelectedIndex = 0;
        txtCurrencyRate.Text = ViewState["ExchangeRate"].ToString();
        fillExpenses();
        txtpaidamount.Text = "";
        ddlExpense.SelectedIndex = 0;
        txtAirwaybillno.Text = "";
        txtExpensesAccount.Text = "";
        txtShippingAcc.Text = "";
        txtvolumetricweight.Text = "";
        pnlOrderInfo.Enabled = true;
        TabProductSupplier.Enabled = true;
        TabAdvancePayment.Enabled = true;
        //for arcawing
        txtShipSupplierName.Text = "";
        txtShipingAddress.Text = "";
        btnShowCreditDetails.Visible = false;
        ddlprojectname.SelectedIndex = 0;

        txtAdvancePer.Text = "0";
        btnSave.Enabled = true;
        btnReset.Visible = true;
    }
    //added by divya, in this function i m not filling dropdown again to avoid the error of "select currency"
    public void resetVariables()
    {
        Session["Expenses_Tax_Purchase_Order"] = null;
        Hdn_Quot_Id.Value = "";
        Session["Temp_Product_Tax_PO"] = null;
        txtPOdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtShippingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtDeliveryDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtCurrencyRate.Text = "0";
        ddlPayCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        txtPayExchangeRate.Text = "1";
        txtPoNo.Text = "";
        txtSupplierName.Text = "";
        ddlPaymentMode.SelectedIndex = 0;
        txRemark.Text = "";
        txtShippingLine.Text = "";
        lblshipingLineMobileNo.Text = "";
        lblShipingEmailId.Text = "";
        ddlShipBy.SelectedIndex = 0;
        txtTotalWeight.Text = "";
        ddlShipmentType.SelectedIndex = 0;
        ddlFreightStatus.SelectedIndex = 1;
        txtUnitRate.Text = "";
        txtVendorQNo.Text = "";
        ddlPartialShipment.SelectedIndex = 0;
        txtDesc.Content = "";
        ddlOrderType.SelectedValue = "D";
        ddlOrderType_SelectedIndexChanged(null, null);
        txtReferenceNo.Text = "";
        ddlReferenceVoucherType.SelectedIndex = 0;
        btnAddProduct.Visible = true;
        HdnEdit.Value = "";
        ddlReferenceVoucherType.Enabled = true;
        ddlOrderType.Enabled = true;
        ViewState["DocNo"] = GetDocumentNumber();
        txtReferenceNo.ReadOnly = false;
        Session["RPQ_No"] = null;
        Session["PayementDt"] = null;
        btnPaymentReset_Click(null, null);
        ViewState["Status"] = null;
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        ViewState["dtProduct"] = null;
        txtGrossAmount.Text = "0";
        txtNetDutyPer.Text = "0";
        txtNetDutyVal.Text = "0";
        txtGrandTotal.Text = "0";
        ddlShipUnit.SelectedIndex = 0;
        txtCurrencyRate.Text = ViewState["ExchangeRate"].ToString();
        txtpaidamount.Text = "";
        ddlExpense.SelectedIndex = 0;
        txtAirwaybillno.Text = "";
        txtExpensesAccount.Text = "";
        txtShippingAcc.Text = "";
        txtvolumetricweight.Text = "";
        pnlOrderInfo.Enabled = true;
        TabProductSupplier.Enabled = true;
        TabAdvancePayment.Enabled = true;
        txtShipSupplierName.Text = "";
        txtShipingAddress.Text = "";
        btnShowCreditDetails.Visible = false;
        ddlprojectname.SelectedIndex = 0;
        btnSave.Enabled = true;
        btnReset.Visible = true;
    }

    //public void fillGrid()
    //{
    //    DataTable dt = ObjPurchaseOrder.GetPurchaseOrderTrueAll(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString());
    //    //filter added for get pending order 
    //    if (ddlPosted.SelectedIndex != 2)
    //    {
    //        try
    //        {
    //            dt = new DataView(dt, "InvoiceStatus='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    //    objPageCmn.FillData((object)gvPurchaseOrder, dt, "", "");
    //    Session["dtPurchaseOrder"] = dt;
    //    Session["dtFilterPurchaseOrder"] = dt;
    //    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
    //    //AllPageCode();
    //    dt = null;
    //}
    private void fillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlFieldName.SelectedItem.Value == "PODate" || ddlFieldName.SelectedItem.Value == "DeliveryDate")
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 1)
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
        strWhereClause = " isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        if (ddlLocation.SelectedIndex > 0)
        {
            strWhereClause = strWhereClause + " and location_id='" + ddlLocation.SelectedValue.Trim() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and location_id in(" + ddlLocation.SelectedValue.Trim() + ")";
        }

        if (ddlPosted.SelectedIndex != 2)
        {
            strWhereClause = strWhereClause + " and  InvoiceStatus='" + ddlPosted.SelectedValue.ToString() + "'";
        }

        if (ddlUser.SelectedItem.ToString() != "--Select User--")
        {
            strWhereClause = strWhereClause + " and createduser='" + ddlUser.SelectedItem.ToString() + "'";
        }

        int totalRows = 0;
        using (DataTable dt = ObjPurchaseOrder.GetPurchaseOrderDataByPageIndexing((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), gvPurchaseOrder.Attributes["CurrentSortField"], gvPurchaseOrder.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvPurchaseOrder, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                gvPurchaseOrder.DataSource = null;
                gvPurchaseOrder.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }

            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }

    }
    public void fillGridBin()
    {
        DataTable dt = ObjPurchaseOrder.GetPurchaseOrderFalseAll(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBinPurchaseOrder, dt, "", "");
        Session["DtBinPurchaseOrder"] = dt;
        Session["DtFilterBinPurchaseOrder"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
        dt = null;
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
    public string GetOrderType(string OrderType)
    {
        string Type = string.Empty;
        if (OrderType == "D")
        {
            Type = "Direct(search as D)";
        }
        else
        {
            Type = "Reference of Quotation(Search as R)";
        }
        return Type;
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
        dtres = null;
        return ArebicMessage;
    }
    public void FillUnitdetail(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    public void FillCurrency()
    {
        try
        {
            DataTable dt = ObjCurrencyMaster.GetCurrencyMaster();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlPayCurrency, dt, "Currency_Name", "Currency_Id");
            dt = null;
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
            ddlPayCurrency.Items.Insert(0, "--Select--");
            ddlPayCurrency.SelectedIndex = 0;
        }
        if (ddlCurrency.Items.Count > 0)
        {
            try
            {
                ddlCurrency.SelectedValue = ViewState["CurrencyId"].ToString();
                ddlCurrency_SelectedIndexChanged(null, null);
            }
            catch
            {
            }
        }
    }
    public void fillgridDetail()
    {
        string ReqId = ObjPurchaseOrder.getAutoId();
        if (HdnEdit.Value == "")
        {
            ReqId = ObjPurchaseOrder.getAutoId();
        }
        else
        {
            ReqId = HdnEdit.Value.ToString();
        }
        DataTable dt = ObjPurchaseOrderDetail.GetPurchaseOrderDetailbyPOId(StrCompId, StrBrandId, StrLocationId, ReqId.ToString());
        if (Session["OrderType"].ToString() == "PQ")
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvQuotationProductEdit, dt, "", "");
            if (GvQuotationProductEdit != null && GvQuotationProductEdit.Rows.Count > 0)
            {
                if (ViewState["DefaultCurrency"].ToString() != null)
                {
                    GvQuotationProductEdit.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), "Unit Cost", Session["DBConnection"].ToString());
                    GvQuotationProductEdit.HeaderRow.Cells[14].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), "Net Price", Session["DBConnection"].ToString());
                    GvQuotationProductEdit.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), Resources.Attendance.Gross_Price, Session["DBConnection"].ToString());
                    GvQuotationProductEdit.HeaderRow.Cells[12].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), "Tax Value", Session["DBConnection"].ToString());
                    GvQuotationProductEdit.HeaderRow.Cells[10].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), "Discount Value", Session["DBConnection"].ToString());
                }
            }
            //this code is created by jitenra upadhyay on 02-01-2015
            //this code for set the decimal format according company currency
            foreach (GridViewRow gvRow in GvQuotationProductEdit.Rows)
            {
                Label lblAmount = (Label)gvRow.FindControl("lblAmount");
                Label lblDiscountPercentage = (Label)gvRow.FindControl("lblDiscountPercentage");
                Label lblDiscountvalue = (Label)gvRow.FindControl("lblDiscountvalue");
                Label lbltaxpercentage = (Label)gvRow.FindControl("lbltaxpercentage");
                Label lbltaxvalue = (Label)gvRow.FindControl("lbltaxvalue");
                Label lblPriceaftertax = (Label)gvRow.FindControl("lblPriceaftertax");
                Label lblNetprice = (Label)gvRow.FindControl("lblNetprice");
                lblAmount.Text = GetAmountDecimal(lblAmount.Text);
                lblDiscountPercentage.Text = GetAmountDecimal(lblDiscountPercentage.Text);
                lblDiscountvalue.Text = GetAmountDecimal(lblDiscountvalue.Text);
                lbltaxpercentage.Text = GetAmountDecimal(lbltaxpercentage.Text);
                lbltaxvalue.Text = GetAmountDecimal(lbltaxvalue.Text);
                lblPriceaftertax.Text = GetAmountDecimal(lblPriceaftertax.Text);
                lblNetprice.Text = GetAmountDecimal(lblNetprice.Text);
            }
        }
        else
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            int Dis_Per = 0, Dis_Value = 0, Tax_Per = 0, Tax_Value = 0;
            foreach (DataColumn DTC in dt.Columns)
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
                dt.Columns.Add("DiscountP");
            }
            if (Dis_Value == 0)
            {
                dt.Columns.Add("DiscountV");
            }
            if (Tax_Per == 0)
            {
                dt.Columns.Add("TaxP");
            }
            if (Tax_Value == 0)
            {
                dt.Columns.Add("TaxV");
            }
            foreach (DataRow dt_Row in dt.Rows)
            {
                dt_Row["DiscountP"] = dt_Row["DisPercentage"].ToString();
                dt_Row["DiscountV"] = dt_Row["DiscountValue"].ToString();
                dt_Row["TaxP"] = dt_Row["TaxPercentage"].ToString();
                dt_Row["TaxV"] = dt_Row["TaxValue"].ToString();
                dt_Row["TaxP"] = Get_Tax_Percentage(dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                dt_Row["TaxV"] = Get_Tax_Amount((Convert.ToDouble(dt_Row["UnitCost"].ToString()) - Convert.ToDouble(dt_Row["DiscountValue"].ToString())).ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
            }
            objPageCmn.FillData((object)gvProduct, dt, "", "");
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                TextBox lblTax = (TextBox)gvr.FindControl("lblTax");
                ImageButton BtnAddTax = (ImageButton)gvr.FindControl("BtnAddTax");
                //lblTax.Visible = true;
                //BtnAddTax.Visible = false;
            }
            if (gvProduct != null && gvProduct.Rows.Count > 0)
            {
                if (ViewState["DefaultCurrency"].ToString() != null)
                {
                    gvProduct.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ViewState["DefaultCurrency"].ToString(), "Unit Cost", Session["DBConnection"].ToString());
                }
            }
        }
        ViewState["dtProduct"] = dt;
        //AllPageCode();
        dt = null;
    }
    public string SuggestedProductName(string ProductId, string SuggestedProduct)
    {
        string ProductNam = string.Empty;
        try
        {
            if (ProductId != "0")
            {
                ProductNam = ProductName(ProductId);
            }
            else
            {
                ProductNam = SuggestedProduct;
            }
        }
        catch
        {
        }
        return ProductNam;
    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;
        ProductName = ObjProductMaster.GetProductNamebyProductId(ProductId.ToString());
        return ProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductCode = string.Empty;
        ProductCode = ObjProductMaster.GetProductCodebyProductId(ProductId.ToString());
        ProductCode = ProductCode == "0" ? "" : ProductCode;
        return ProductCode;
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        UnitName = ObjUnitMaster.GetUnitNameByUnitId(UnitId.ToString(), StrCompId.ToString());
        return UnitName;
    }
    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    public void ResetDetail()
    {
        txtProductName.Text = "";
        txtPDescription.Text = "";
        ddlUnit.Items.Clear();
        txtfreeQty.Text = "";
        hidProduct.Value = "";
        txtOrderQty.Text = "";
        txtUnitCost.Text = "";
        txtProductcode.Text = "";
        txtProductcode.Focus();
    }
    //It  Will Bind The unit In SalseOrder Grid.
    public void FillddlUnitInSalesOrderGrid(GridView Gv)
    {
        DataTable dtUnit = ObjUnitMaster.GetUnitMaster(StrCompId);
        foreach (GridViewRow Row in Gv.Rows)
        {
            DropDownList ddlUnit = (DropDownList)Row.FindControl("ddlgvUnit");
            HiddenField HdnUnitId = (HiddenField)Row.FindControl("hdnUnitId");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlUnit, dtUnit, "Unit_Name", "Unit_Id");
            try
            {
                ddlUnit.SelectedValue = HdnUnitId.Value.ToString();
            }
            catch
            {
            }
        }
        dtUnit = null;
    }
    #region Quotation
    private void FillGridQuotation()
    {
        DataTable dtQuotation = objQuoteHeader.GetQuoteHeaderAllTrue(StrCompId, StrBrandId, StrLocationId);
        for (int i = 0; i < dtQuotation.Rows.Count; i++)
        {
            DataTable dtQuotationDetail = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId, StrBrandId, StrLocationId, dtQuotation.Rows[i]["Trans_Id"].ToString());
            if (dtQuotationDetail.Rows.Count == 0)
            {
                dtQuotation.Rows[i]["IsActive"] = false.ToString();
            }
        }
        dtQuotation = new DataView(dtQuotation, "IsActive<>'False'", "", DataViewRowState.CurrentRows).ToTable();
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtQuotation.Rows.Count + "";
        Session["dtQuotation"] = dtQuotation;
        Session["dtQFilter"] = dtQuotation;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseQuote, dtQuotation, "", "");
        dtQuotation = null;
    }
    #endregion
    #endregion
    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_PurchaseQuotation(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseQuoteHeader objquot = new Inv_PurchaseQuoteHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_PurchaseQuoteDetail ObjQuDetail = new Inv_PurchaseQuoteDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesOrderHeader ObjSalesOrder = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesOrderDetail ObjSelseOrderDetail = new Inv_SalesOrderDetail(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        if (HttpContext.Current.Session["ReferenceType"].ToString() == "PQ")
        {
            dt = objquot.GetQuoteHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtTemp = new DataView(dt, "RPQ_No like'%" + prefixText + "%'", "RPQ_NO asc", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count != 0)
            {
                dt.Rows.Clear();
                dt.Merge(dtTemp);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtQuotationDetail = ObjQuDetail.GetPurchaseQuationDatilsNotInPOdetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), dt.Rows[i]["Trans_Id"].ToString());
                if (dtQuotationDetail.Rows.Count == 0)
                {
                    dt.Rows[i]["IsActive"] = false.ToString();
                }
            }
        }
        if (HttpContext.Current.Session["ReferenceType"].ToString() == "SO")
        {
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string isApproved = objInvParam.getParameterValueByParameterName("SalesOrderApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (isApproved != "")
            {
                if (Convert.ToBoolean(isApproved) == true)
                {
                    dt = new DataView(ObjSalesOrder.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "IsInPO='1' and Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(ObjSalesOrder.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "IsInPO='1'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            else
            {
                dt = new DataView(ObjSalesOrder.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "IsInPO='1'", "", DataViewRowState.CurrentRows).ToTable();
            }
            //  dt = new DataView(ObjSalesOrder.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Post='" + true.ToString() + "'and IsInPO='1'", "", DataViewRowState.CurrentRows).ToTable();
            dtTemp = new DataView(dt, "SalesOrderNo like'%" + prefixText + "%'", "SalesOrderNo asc", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count != 0)
            {
                dt.Rows.Clear();
                dt.Merge(dtTemp);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtSelseOrderDetail = ObjSelseOrderDetail.GetSelesOrderDetailNotInPoDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), dt.Rows[i]["Trans_Id"].ToString());
                if (dtSelseOrderDetail.Rows.Count == 0)
                {
                    dt.Rows[i]["IsActive"] = false.ToString();
                }
            }
        }
        dt = new DataView(dt, "IsActive<>'" + false.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[dt.Rows.Count];
        if (HttpContext.Current.Session["ReferenceType"].ToString() == "PQ")
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["RPQ_No"].ToString();
            }
        }
        if (HttpContext.Current.Session["ReferenceType"].ToString() == "SO")
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
            }
        }
        dt = null;
        dtTemp = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtContact = ObjContactMaster.getSupplierNamePreText(prefixText);
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        DataTable dtSupplier = new DataTable();
        if (HttpContext.Current.Session["RPQ_No"] != null && HttpContext.Current.Session["RPQ_No"].ToString() == "PQ")
        {
            Inv_PurchaseQuoteHeader objPQHeader = new Inv_PurchaseQuoteHeader(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_PurchaseQuoteDetail objPQDetail = new Inv_PurchaseQuoteDetail(HttpContext.Current.Session["DBConnection"].ToString());
            dtSupplier = objPQDetail.GetQuoteDetailByRPQ_No(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objPQHeader.GetQuoteHeaderAllDataByRPQ_No(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["RPQ_No"].ToString()).Rows[0]["Trans_Id"].ToString());
            string filtertext = "Filtertext like '%" + prefixText + "%'";
            dtSupplier = dtSupplier.DefaultView.ToTable(true, "Name", "Trans_Id", "Filtertext");
        }
        else
        {
            Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
            dtSupplier = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
        }
        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
            }
        }
        dtSupplier = null;
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
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
        dt = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
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
    #endregion
    #region InvoicePrint
    //modify by jitendra upadhyay on 19-04-2014
    //create new region for print Purchase Order
    void PrintReport(string OrderID)
    {
        string strCmd = string.Format("window.open('../Purchase/PurchaseOrder_Print.aspx?Id=" + OrderID.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    #endregion
    public string GetAmountDecimal(string Amount)
    {
        if (ddlCurrency.SelectedIndex == 0)
        {
            return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);
        }
        else
        {
            return ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, Amount);
        }
    }
    public string GetStatus(int TransID, ref SqlTransaction trns)
    {
        string status = "";
        status = objDa.get_SingleValue("select field4 from Inv_PurchaseOrderHeader where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "' and transId='" + TransID.ToString() + "'", ref trns);
        status = status == "@NOTFOUND@" ? "" : status;
        if (status != "")
        {
            return status;
        }
        else
        {
            return "";
        }
    }
    public string GetStatus(int TransID)
    {
        string status = "";
        status = objDa.get_SingleValue("select field4 from Inv_PurchaseOrderHeader where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "' and transId='" + TransID.ToString() + "'");
        status = status == "@NOTFOUND@" ? "" : status;
        if (status != "")
        {
            return status;
        }
        else
        {
            return "";
        }
    }
    public void setDecimalFormate()
    {
        foreach (GridViewRow GridRow in gvQuatationProduct.Rows)
        {
            TextBox txtUnitCost = (TextBox)GridRow.FindControl("txtUnitCost");
            TextBox txtgvRequiredQty = (TextBox)GridRow.FindControl("txtgvRequiredQty");
            Label lblTax = (Label)GridRow.FindControl("lblTax");
            Label lblgvTaxValue = (Label)GridRow.FindControl("lblTaxValue");
            Label lblgvTaxafterPrice = (Label)GridRow.FindControl("lblTaxafterPrice");
            if (txtUnitCost != null)
                txtUnitCost.Text = GetAmountDecimal(txtUnitCost.Text);
            if (txtgvRequiredQty != null)
                txtgvRequiredQty.Text = GetAmountDecimal(txtgvRequiredQty.Text);
            if (lblTax != null)
                lblTax.Text = GetAmountDecimal(lblTax.Text);
            if (lblgvTaxValue != null)
                lblgvTaxValue.Text = GetAmountDecimal(lblgvTaxValue.Text);
            if (lblgvTaxafterPrice != null)
                lblgvTaxafterPrice.Text = GetAmountDecimal(lblgvTaxafterPrice.Text);
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            TextBox lblUnitRate = (TextBox)gvRow.FindControl("lblUnitRate");
            lblUnitRate.Text = GetAmountDecimal(lblUnitRate.Text);
        }
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
            dtSName = null;
        }
        else
        {
            strSupplierName = "";
        }
        return strSupplierName;
    }
    protected string GetSuppliers(string QuoteID)
    {
        DataTable dtPQDetail;
        string SupplierName = "";
        string quoteIDTemp = "";
        dtPQDetail = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, QuoteID);
        //if (dtPQDetail != null && dtPQDetail.Rows.Count > 0)
        for (int i = 0; i < dtPQDetail.Rows.Count; i++)
        {
            if (quoteIDTemp != dtPQDetail.Rows[i]["Supplier_Id"].ToString())
            {
                if (SupplierName == "")
                {
                    SupplierName = GetSupplierName(dtPQDetail.Rows[i]["Supplier_Id"].ToString());
                }
                else
                {
                    SupplierName = SupplierName + "," + GetSupplierName(dtPQDetail.Rows[i]["Supplier_Id"].ToString());
                }
                quoteIDTemp = dtPQDetail.Rows[i]["Supplier_Id"].ToString();
            }
        }
        dtPQDetail = null;
        return SupplierName;
    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldName.SelectedItem.Value == "PODate") || (ddlFieldName.SelectedItem.Value == "DeliveryDate"))
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
    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if ((ddlbinFieldName.SelectedItem.Value == "PODate") || (ddlbinFieldName.SelectedItem.Value == "DeliveryDate"))
        {
            txtbinValueDate.Visible = true;
            txtbinValue.Visible = false;
            txtbinValue.Text = "";
            txtbinValueDate.Text = "";
        }
        else
        {
            txtbinValueDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValue.Text = "";
            txtbinValueDate.Text = "";
        }
    }
    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "RPQ_Date")
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
    #endregion
    #region Advance Search
    public DataTable CreateProductDataTable()
    {
        DataTable dtProduct = new DataTable();
        dtProduct.Columns.Add("Serial_No");
        dtProduct.Columns.Add("Trans_Id");
        dtProduct.Columns.Add("PoNO");
        dtProduct.Columns.Add("Product_Id");
        dtProduct.Columns.Add("ProductDescription");
        dtProduct.Columns.Add("UnitId");
        dtProduct.Columns.Add("UnitCost");
        dtProduct.Columns.Add("OrderQty");
        dtProduct.Columns.Add("freeQty");
        dtProduct.Columns.Add("SOId");
        dtProduct.Columns.Add("SONO");
        return dtProduct;
    }
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        if (txtSupplierName.Text.Trim() == "")
        {
            DisplayMessage("Enter Supplier Name");
            txtSupplierName.Focus();
            return;
        }
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblSNo = (Label)gvRow.FindControl("lblSerialNO");
            Label lblgvProductId = (Label)gvRow.FindControl("lblGvProductId");
            Label lblgvUnitId = (Label)gvRow.FindControl("lblUnit");
            Label lblgvUnitID = (Label)gvRow.FindControl("lblgvUnitID");
            Label lblgvReqQty = (Label)gvRow.FindControl("lblReqQty");
            Label lblgvFreeQty = (Label)gvRow.FindControl("lblFreeQty");
            TextBox lblgvUnitCost = (TextBox)gvRow.FindControl("lblUnitRate");
            dr["Serial_No"] = lblSNo.Text;
            dr["Trans_Id"] = lblSNo.Text;
            dr["PONo"] = "";
            dr["Product_Id"] = lblgvProductId.Text;
            dr["UnitId"] = lblgvUnitID.Text;
            dr["ProductDescription"] = "";
            dr["OrderQty"] = lblgvReqQty.Text;
            dr["freeQty"] = lblgvFreeQty.Text;
            dr["UnitCost"] = lblgvUnitCost.Text;
            dt.Rows.Add(dr);
        }
        ViewState["dtProduct"] = dt;
        Session["DtSearchProduct"] = ViewState["dtProduct"];
        dt = null;
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=PO&&SupId=" + txtSupplierName.Text.Split('/')[1].ToString() + "&&CurId=" + ddlCurrency.SelectedValue + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        div_salesOrder.Visible = false;
        div_ExcelUpload.Visible = false;
        btnAddProduct.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        pnlProduct1.Visible = false;
        if (rbtnFormView.Checked == true)
        {
            pnlProduct1.Visible = true;
            btnAddProduct.Visible = true;
            btnAddProduct_Click(null, null);
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
        }

        if (rbtnSalesOrder.Checked == true)
        {
            div_salesOrder.Visible = true;
            btnGetDataFromSalesOrder_Click();
        }
        if (rbtnUsingExcel.Checked == true)
        {
            div_ExcelUpload.Visible = true;
            //btnGetDataFromSalesOrder_Click();
        }

        if (gvProduct.Rows.Count == 0)
            ddlTransType.Enabled = true;
    }
    #endregion
    #region GridCalculation
    protected void txtNetDutyPer_OnTextChanged(object sender, EventArgs e)
    {
        if (txtGrossAmount.Text == "")
        {
            txtGrossAmount.Text = "0";
        }
        if (txtNetDutyPer.Text != "" && txtNetDutyPer.Text != "0")
        {
            txtNetDutyVal.Text = GetAmountDecimal(((Convert.ToDouble(txtGrossAmount.Text) * Convert.ToDouble(txtNetDutyPer.Text)) / 100).ToString());
            txtGrandTotal.Text = GetAmountDecimal((Convert.ToDouble(txtGrossAmount.Text) + Convert.ToDouble(txtNetDutyVal.Text)).ToString());
        }
        else
        {
            txtNetDutyVal.Text = "0";
            txtGrandTotal.Text = GetAmountDecimal(txtGrossAmount.Text);
        }
        //AllPageCode();
    }
    protected void txtNetDutyVal_OnTextChanged(object sender, EventArgs e)
    {
        if (txtGrossAmount.Text == "")
        {
            txtGrossAmount.Text = "0";
        }
        if (txtNetDutyVal.Text != "" && txtNetDutyVal.Text != "0")
        {
            txtNetDutyPer.Text = GetAmountDecimal(((100 * Convert.ToDouble(txtNetDutyVal.Text)) / Convert.ToDouble(txtGrossAmount.Text)).ToString());
            txtGrandTotal.Text = GetAmountDecimal((Convert.ToDouble(txtGrossAmount.Text) + Convert.ToDouble(txtNetDutyVal.Text)).ToString());
        }
        else
        {
            txtNetDutyPer.Text = "0";
            txtGrandTotal.Text = GetAmountDecimal(txtGrossAmount.Text);
        }
        //AllPageCode();
    }
    protected void chk_OnCheckedChanged(object sender, EventArgs e)
    {
        GridCalculation();
        //AllPageCode();
    }
    protected void txtgvQuantity_OnTextChanged(object sender, EventArgs e)
    {
        GridCalculation();
        //AllPageCode();
    }
    protected void txtgvUnitPriceSo_OnTextChanged(object sender, EventArgs e)
    {
        GridCalculation();
        //AllPageCode();
    }
    protected void ChkSelectSo_OnCheckedChanged(object sender, EventArgs e)
    {
        GridCalculation();
        //AllPageCode();
    }
    #endregion
    #region AddCustomer
    protected void btnAddSupplier_OnClick(object sender, EventArgs e)
    {
        // string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?Page=PO','window','width=1024');");

        string strCmd = string.Format("window.open('../Sales/AddContact.aspx?Page=PO', 'window', 'width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
        {
            allow = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            allow = true;
        }
        dtAllPageCode = null;
        return allow;
    }
    #endregion
    #region Payment
    protected void ddlPayCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayCurrency.SelectedIndex != 0)
        {
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            txtPayExchangeRate.Text = SystemParameter.GetExchageRate(ddlPayCurrency.SelectedValue, Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            if (txtPayExchangeRate.Text != "" && txtFCPayCharges.Text != "")
            {
                txtLCPayCharges.Text = GetAmountDecimal((Convert.ToDouble(txtFCPayCharges.Text.Trim()) * Convert.ToDouble(txtPayExchangeRate.Text.Trim())).ToString());
            }
        }
    }
    public void fillPaymentMode()
    {
        try
        {
            DataTable dt = ObjPaymentMaster.GetPaymentModeMaster(StrCompId.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt = new DataView(dt, "Field1='Cash'", "", DataViewRowState.CurrentRows).ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlAdvancePayment, dt, "Pay_Mod_Name", "Pay_Mode_Id");
            dt = null;
        }
        catch
        {
            ddlAdvancePayment.Items.Insert(0, "--Select--");
            ddlAdvancePayment.SelectedIndex = 0;
        }
    }
    protected void btnPaymentSave_Click(object sender, object e)
    {
        if (ddlAdvancePayment.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Payment Mode");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAdvancePayment);
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
        if (Session["PayementDt"] != null)
        {
            dt = (DataTable)Session["PayementDt"];
            //if (new DataView(dt, "PaymentModeId='" + ddlAdvancePayment.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
            //{
            //    DisplayMessage("Payment Mode already exist");
            //    fillPaymentGrid((DataTable)Session["PayementDt"]);
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
        dt.Rows[dt.Rows.Count - 1]["PaymentModeId"] = ddlAdvancePayment.SelectedValue.ToString();
        dt.Rows[dt.Rows.Count - 1]["PaymentName"] = ddlAdvancePayment.SelectedItem.ToString();
        //dt.Rows[dt.Rows.Count - 1]["Amount"] = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), txtPayAmount.Text.Trim());
        dt.Rows[dt.Rows.Count - 1]["AccountNo"] = GetAccountId(txtPayAccountNo.Text);
        dt.Rows[dt.Rows.Count - 1]["CardNo"] = txtPayCardNo.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["CardName"] = txtPayCardName.Text.Trim();
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
        dt.Rows[dt.Rows.Count - 1]["Pay_Charges"] = txtLCPayCharges.Text;
        dt.Rows[dt.Rows.Count - 1]["PayCurrencyID"] = ddlPayCurrency.SelectedValue;
        dt.Rows[dt.Rows.Count - 1]["PayExchangeRate"] = txtPayExchangeRate.Text;
        dt.Rows[dt.Rows.Count - 1]["FCPayAmount"] = txtFCPayCharges.Text;
        Session["PayementDt"] = dt;
        fillPaymentGrid(dt);
        btnPaymentReset_Click(null, null);
        fillPaymentMode();
        //here we change balance amount when we paid against the invoice amount
        if (txtGrandTotal.Text != "")
        {
            if (Convert.ToDouble(txtGrandTotal.Text) > 0)
            {
                if (gvPayment.Rows.Count > 0)
                {
                    txtFCPayCharges.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(txtGrandTotal.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text)).ToString());
                }
                else
                {
                    txtFCPayCharges.Text = txtGrandTotal.Text;
                }
            }
        }
        dt = null;
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
        fillBank();
        trcheque.Visible = false;
        trcard.Visible = false;
        lblPayBank.Visible = false;
        //lblpaybankcolon.Visible = false;
        ddlPayBank.Visible = false;
        ddlPayCurrency.SelectedValue = ddlCurrency.SelectedValue;
        txtPayExchangeRate.Text = "1";
    }
    public void fillBank()
    {
        DataTable dt = ObjBankMaster.GetBankMaster();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)ddlPayBank, dt, "Bank_Name", "Bank_Id");
        dt = null;
    }
    public void fillPaymentGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");
        Session["PayementDt"] = dt;
        //AllPageCode();
        double f = 0;
        double fc = 0;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            try
            {
                ((Label)gvrow.FindControl("lblgvExp_Charges")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), ((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
                ((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), ((Label)gvrow.FindControl("lblgvFCExchangeAmount")).Text);
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
        try
        {
            ((Label)gvPayment.FooterRow.FindControl("txttotExpShow")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), f.ToString());
            ((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), fc.ToString());
        }
        catch
        {
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
            finally
            {
                dtAccount = null;
            }
        }
    }
    protected void btnDeletePay_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)Session["PayementDt"], "TransId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        fillPaymentGrid(dt);
        //here we change balance amount when we paid against the invoice amount
        if (txtGrandTotal.Text != "")
        {
            if (Convert.ToDouble(txtGrandTotal.Text) > 0)
            {
                if (gvPayment.Rows.Count > 0)
                {
                    txtFCPayCharges.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(txtGrandTotal.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text)).ToString());
                }
                else
                {
                    txtFCPayCharges.Text = txtGrandTotal.Text;
                }
            }
        }
        dt = null;
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        TabContainer1.ActiveTabIndex = 2;
        btnPaymentReset_Click(null, null);
        if (ddlAdvancePayment.SelectedValue == "--Select--")
        {
            txtPayAccountNo.Text = "";
        }
        else if (ddlAdvancePayment.SelectedValue != "--Select--")
        {
            ddlCurrency.SelectedValue = ddlCurrency.SelectedValue;
            DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, ddlAdvancePayment.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtPay.Rows.Count > 0)
            {
                string strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                }
                dtAcc = null;
            }
            dtPay = null;
        }
        //here we showing related field according the select payment mode
        //when payment mode is cash then we showing accounts no only 
        if (ddlAdvancePayment.SelectedIndex != 0)
        {
            if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlAdvancePayment.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Cash")
            {
                trBank.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlAdvancePayment.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Credit")
            {
                trBank.Visible = true;
                lblPayBank.Visible = true;
                //lblpaybankcolon.Visible = true;
                ddlPayBank.Visible = true;
                trcheque.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlAdvancePayment.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Card")
            {
                trBank.Visible = true;
                trcard.Visible = true;
            }
        }
        ddlPayCurrency.SelectedValue = ddlCurrency.SelectedValue;
        txtPayExchangeRate.Text = txtCurrencyRate.Text;
        //here we change balance amount when we paid against the invoice amount
        if (txtGrandTotal.Text != "")
        {
            if (Convert.ToDouble(txtGrandTotal.Text) > 0)
            {
                if (gvPayment.Rows.Count > 0)
                {
                    txtFCPayCharges.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(txtGrandTotal.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotFCExpShow")).Text)).ToString());
                }
                else
                {
                    txtFCPayCharges.Text = txtGrandTotal.Text;
                }
                txtFCPayCharges_TextChanged(null, null);
            }
        }
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
            dtAccount = null;
        }
        else
        {
            retval = "";
        }
        return retval;
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
            dtLocation = null;
        }
        return strLocationCode;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = COA.getAccountDetailsByPreText(prefixText, HttpContext.Current.Session["CompId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["filterText"].ToString();
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
        dt = null;
        return str;
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
        txtLCPayCharges.Text = GetAmountDecimal((Convert.ToDouble(txtFCPayCharges.Text.Trim()) * Convert.ToDouble(txtPayExchangeRate.Text.Trim())).ToString());
    }
    public string CurrencyName(string CurrencyId)
    {
        string CurrencyName = string.Empty;
        DataTable dt = ObjCurrencyMaster.GetCurrencyMasterById(CurrencyId.ToString());
        if (dt.Rows.Count != 0)
        {
            CurrencyName = dt.Rows[0]["Currency_Name"].ToString();
        }
        else
        {
            CurrencyName = "0";
        }
        dt = null;
        return CurrencyName;
    }
    #endregion
    #region shipexpenses
    protected void ddlExpense_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpense.SelectedValue == "--Select--")
        {
            txtExpensesAccount.Text = "";
        }
        else if (ddlExpense.SelectedValue != "--Select--")
        {
            DataTable dtExp = ObjShipExp.GetShipExpMasterById(StrCompId, ddlExpense.SelectedValue);
            if (dtExp.Rows.Count > 0)
            {
                string strAccountId = dtExp.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtExpensesAccount.Text = strAccountName + "/" + strAccountId;
                }
                dtAcc = null;
            }
            dtExp = null;
        }
        Session["Expenses_Tax_Purchase_Order"] = null;
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
        // GetData();
    }


    #endregion
    #region Encrypt
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    #endregion
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    { return SystemParameter.GetCurrencySmbol(CurrencyId, ObjSysParam.GetCurencyConversionForInv(strCurrencyId, Amount), Session["DBConnection"].ToString()); }
    #region PendingPurchaseOrder
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid(1);
        //AllPageCode();
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
        return GetAmountDecimal(SysQty);
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
        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=PURCHASE&&Contact=" + CustomerName + "')";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
    }
    #endregion
    protected void lnkSupplierHistory_OnClick(object sender, EventArgs e)
    {
        string strSupplierId = string.Empty;
        if (txtSupplierName.Text != "")
        {
            try
            {
                strSupplierId = txtSupplierName.Text.Split('/')[1].ToString();
            }
            catch
            {
                strSupplierId = "0";
            }
        }
        else
        {
            strSupplierId = "0";
        }
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + strSupplierId + "&&Page=PO','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    #region ShippingInfo
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objcontact.getSupplierNamePreText(prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        dtCon = null;
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.getAddressNamePreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        dt = null;
        return str;
    }
    protected void txtShipSupplierName_TextChanged(object sender, EventArgs e)
    {
        DataTable DtCustomer = new DataTable();
        DataTable dtAddress = new DataTable();
        if (txtShipSupplierName.Text != "")
        {
            string[] ShipName = txtShipSupplierName.Text.Split('/');
            DtCustomer = ObjContactMaster.GetContactByContactName(ShipName[0].ToString().Trim());
            if (DtCustomer.Rows.Count > 0)
            {
                dtAddress = ObjContactMaster.GetAddressByRefType_Id("Contact", ShipName[1].ToString().Trim());
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
                txtShipSupplierName.Text = "";
                txtShipSupplierName.Focus();
                return;
            }
        }
        dtAddress = null;
        DtCustomer = null;
    }
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAM = ObjAdd.GetAddressDataByAddressName(txtShipingAddress.Text);
            if (dtAM.Rows.Count > 0)
            {
            }
            else
            {
                txtShipingAddress.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
                return;
            }
            dtAM = null;
        }
        //AllPageCode();
    }
    #endregion
    //create new grid textbox event
    //created by jitendra upadhyay on 01-09-2016
    //for grid calculation 
    //in editable mode
    protected void txtReqQty_OnTextChanged(object sender, EventArgs e)
    {
        GridCalculation();
        //AllPageCode();
    }
    #region bindProjectList
    public void FillRefferenceprojectList()
    {
        using (DataTable dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer())
        {
            objPageCmn.FillData((object)ddlprojectname, dtProjectMAster, "Project_Name", "");
            dtProjectMAster.Dispose();
        }
    }
    #endregion

    public void TaxandDiscountParameter()
    {
        string isTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        if (Convert.ToBoolean(isTax) == false)
        {
            gvProduct.Columns[11].Visible = false;
            gvProduct.Columns[12].Visible = false;
            gvQuatationProduct.Columns[10].Visible = false;
            gvQuatationProduct.Columns[11].Visible = false;
            gvQuatationProduct.Columns[12].Visible = false;
            GvQuotationProductEdit.Columns[11].Visible = false;
            GvQuotationProductEdit.Columns[12].Visible = false;
            GvQuotationProductEdit.Columns[13].Visible = false;
            gvQuotationProducteditView.Columns[8].Visible = false;
            gvQuotationProducteditView.Columns[9].Visible = false;
            gvQuotationProducteditView.Columns[10].Visible = false;
            gvAddQuotationGrid.Columns[10].Visible = false;
            gvAddQuotationGrid.Columns[11].Visible = false;
            gvAddQuotationGrid.Columns[12].Visible = false;
        }
        else
        {
            gvProduct.Columns[11].Visible = true;
            gvProduct.Columns[12].Visible = true;
            gvQuatationProduct.Columns[10].Visible = true;
            gvQuatationProduct.Columns[11].Visible = true;
            gvQuatationProduct.Columns[12].Visible = true;
            GvQuotationProductEdit.Columns[11].Visible = true;
            GvQuotationProductEdit.Columns[12].Visible = true;
            GvQuotationProductEdit.Columns[13].Visible = true;
            gvQuotationProducteditView.Columns[8].Visible = true;
            gvQuotationProducteditView.Columns[9].Visible = true;
            gvQuotationProducteditView.Columns[10].Visible = true;
            gvAddQuotationGrid.Columns[10].Visible = true;
            gvAddQuotationGrid.Columns[11].Visible = true;
            gvAddQuotationGrid.Columns[12].Visible = false;
        }

        string isDiscount = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        if (Convert.ToBoolean(isDiscount) == false)
        {
            gvProduct.Columns[9].Visible = false;
            gvProduct.Columns[10].Visible = false;
            GvQuotationProductEdit.Columns[9].Visible = false;
            GvQuotationProductEdit.Columns[10].Visible = false;
            gvQuotationProducteditView.Columns[11].Visible = false;
            gvQuotationProducteditView.Columns[12].Visible = false;
            gvAddQuotationGrid.Columns[8].Visible = false;
            gvAddQuotationGrid.Columns[9].Visible = false;
            gvQuatationProduct.Columns[8].Visible = false;
            gvQuatationProduct.Columns[9].Visible = false;
        }
        else
        {
            gvProduct.Columns[9].Visible = true;
            gvProduct.Columns[10].Visible = true;
            GvQuotationProductEdit.Columns[9].Visible = true;
            GvQuotationProductEdit.Columns[10].Visible = true;
            gvQuotationProducteditView.Columns[11].Visible = true;
            gvQuotationProducteditView.Columns[12].Visible = true;
            gvAddQuotationGrid.Columns[8].Visible = true;
            gvAddQuotationGrid.Columns[9].Visible = true;
            gvQuatationProduct.Columns[8].Visible = true;
            gvQuatationProduct.Columns[9].Visible = true;
        }

        setDecimalFormate();
    }

    //--------------------------------------------------
    //---------new Code By Ghanshyam Suthar on 02-01-2018
    protected void lblDiscount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label ProductId = (Label)Row.FindControl("lblGvProductId");
            Label Order_Qty = (Label)Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("lblUnitRate");
            TextBox Discount_Percent = (TextBox)Row.FindControl("lblDiscount");
            TextBox Discount_Value = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox Tax_Per = (TextBox)Row.FindControl("lblTax");
            TextBox Tax_Value = (TextBox)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            TextBox lblTax = (TextBox)Row.FindControl("lblTax");
            ImageButton BtnAddTax = (ImageButton)Row.FindControl("BtnAddTax");
            Label Serial_No = (Label)Row.FindControl("lblSerialNO");
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = double.Parse(((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void lblDiscountValue_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label ProductId = (Label)Row.FindControl("lblGvProductId");
            Label Order_Qty = (Label)Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("lblUnitRate");
            TextBox Discount_Percent = (TextBox)Row.FindControl("lblDiscount");
            TextBox Discount_Value = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox Tax_Per = (TextBox)Row.FindControl("lblTax");
            TextBox Tax_Value = (TextBox)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            TextBox lblTax = (TextBox)Row.FindControl("lblTax");
            ImageButton BtnAddTax = (ImageButton)Row.FindControl("BtnAddTax");
            Label Serial_No = (Label)Row.FindControl("lblSerialNO");
            //lblTax.Visible = false;
            //BtnAddTax.Visible = true;
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = Get_Discount_Percentage(F_Unit_Price.ToString(), ((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void lblUnitRate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label ProductId = (Label)Row.FindControl("lblGvProductId");
            Label Order_Qty = (Label)Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("lblUnitRate");
            TextBox Discount_Percent = (TextBox)Row.FindControl("lblDiscount");
            TextBox Discount_Value = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox Tax_Per = (TextBox)Row.FindControl("lblTax");
            TextBox Tax_Value = (TextBox)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            Label Serial_No = (Label)Row.FindControl("lblSerialNO");
            TextBox lblTax = (TextBox)Row.FindControl("lblTax");
            ImageButton BtnAddTax = (ImageButton)Row.FindControl("BtnAddTax");
            //lblTax.Visible = false;
            //BtnAddTax.Visible = true;
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    Add_Tax_In_Session(Unit_Price.Text, ProductId.Text, Serial_No.Text);
                    double F_Unit_Price = double.Parse(((TextBox)sender).Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);

                    double newprice = (F_Order_Quantity * F_Unit_Price);
                    if (newprice != 0)
                    {

                    }


                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        ddlTransType.Enabled = false;
        if (Session["DtSearchProduct"] != null)
        {
            ViewState["dtProduct"] = Session["DtSearchProduct"];
            if (ViewState["dtProduct"] != null)
            {
                DataTable dtTempDt = ViewState["dtProduct"] as DataTable;
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
                    Add_Tax_In_Session(dt_Row["UnitCost"].ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    dt_Row["DiscountP"] = "0";
                    dt_Row["DiscountV"] = "0.00";
                    dt_Row["TaxP"] = Get_Tax_Percentage(dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    DataColumnCollection columns = dtTempDt.Columns;
                    if (columns.Contains("DiscountValue"))
                    {
                        dt_Row["TaxV"] = Get_Tax_Amount((Convert.ToDouble(dt_Row["UnitCost"].ToString()) - Convert.ToDouble(dt_Row["DiscountValue"].ToString())).ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    }
                    else
                    {
                        dt_Row["TaxV"] = Get_Tax_Amount((Convert.ToDouble(dt_Row["UnitCost"].ToString()) - Convert.ToDouble(dt_Row["DiscountV"].ToString())).ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    }
                }
                ViewState["dtProduct"] = dtTempDt;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProduct, (DataTable)ViewState["dtProduct"], "", "");
                GridCalculation();
                dtTempDt = null;
            }
            Session["DtSearchProduct"] = null;
        }
        else
        {
            if (ViewState["dtProduct"] != null)
            {
                DataTable dtTempDt = ViewState["dtProduct"] as DataTable;
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
                    Add_Tax_In_Session(dt_Row["UnitCost"].ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    dt_Row["DiscountP"] = "0";
                    dt_Row["DiscountV"] = "0.00";
                    dt_Row["TaxP"] = Get_Tax_Percentage(dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    dt_Row["TaxV"] = Get_Tax_Amount((Convert.ToDouble(dt_Row["UnitCost"].ToString()) - Convert.ToDouble(dt_Row["DiscountValue"].ToString())).ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                }
                ViewState["dtProduct"] = dtTempDt;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProduct, (DataTable)ViewState["dtProduct"], "", "");
                GridCalculation();
                dtTempDt = null;
            }
            DisplayMessage("Product Not Found");
            return;
        }
        //gvProduct.HeaderRow.Cells[5]
        //AllPageCode();
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        ddlTransType.Enabled = false;
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Enter Supplier Name");
            txtSupplierName.Focus();
            return;
        }
        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Focus();
            return;
        }
        if (ddlUnit == null)
        {
            DisplayMessage("Select Unit Name");
            ddlUnit.Focus();
            return;
        }
        if (txtUnitCost.Text == "")
        {
            DisplayMessage("Ener Unit Cost");
            txtUnitCost.Focus();
            return;
        }
        if (txtOrderQty.Text == "")
        {
            DisplayMessage("Enter Order Quantity");
            txtOrderQty.Focus();
            return;
        }
        if (txtfreeQty.Text == "")
        {
            txtfreeQty.Text = "0";
        }
        if (HdnEdit.Value == "")
        {
            ReqId = ObjPurchaseOrder.getAutoId();
        }
        else
        {
            ReqId = HdnEdit.Value.ToString();
        }
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(StrCompId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtProductName.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                ProductId = "0";
            }
            dt = null;
        }
        UnitId = ddlUnit.SelectedValue.ToString();
        DataTable dtTempDt = new DataTable();
        if (ViewState["dtProduct"] == null)
        {
            dtTempDt.Columns.Add("Trans_Id");
            dtTempDt.Columns.Add("PoNo");
            dtTempDt.Columns.Add("Product_Id");
            dtTempDt.Columns.Add("ProductDescription");
            dtTempDt.Columns.Add("UnitId");
            dtTempDt.Columns.Add("UnitCost");
            dtTempDt.Columns.Add("OrderQty");
            dtTempDt.Columns.Add("freeQty");
            dtTempDt.Columns.Add("Serial_No");
            dtTempDt.Columns.Add("DiscountP");
            dtTempDt.Columns.Add("DiscountV");
            dtTempDt.Columns.Add("TaxP");
            dtTempDt.Columns.Add("TaxV");
            dtTempDt.Columns.Add("SOId");
            dtTempDt.Columns.Add("SONO");
            dtTempDt.Rows.Add();
            dtTempDt.Rows[0]["Trans_Id"] = dtTempDt.Rows.Count;
            dtTempDt.Rows[0]["Serial_No"] = dtTempDt.Rows.Count;
            dtTempDt.Rows[0]["PoNo"] = ReqId.ToString();
            dtTempDt.Rows[0]["Product_Id"] = ProductId.ToString();
            dtTempDt.Rows[0]["ProductDescription"] = txtPDescription.Text.ToString();
            dtTempDt.Rows[0]["UnitId"] = UnitId.ToString();
            dtTempDt.Rows[0]["UnitCost"] = txtUnitCost.Text.ToString();
            dtTempDt.Rows[0]["OrderQty"] = txtOrderQty.Text.ToString();
            dtTempDt.Rows[0]["freeQty"] = txtfreeQty.Text.ToString();
            dtTempDt.Rows[0]["DiscountP"] = "0";
            dtTempDt.Rows[0]["DiscountV"] = "0.00";
            Add_Tax_In_Session(txtUnitCost.Text, ProductId, dtTempDt.Rows.Count.ToString());
            dtTempDt.Rows[0]["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), dtTempDt.Rows.Count.ToString());
            dtTempDt.Rows[0]["TaxV"] = Get_Tax_Amount(txtUnitCost.Text.ToString(), ProductId.ToString(), dtTempDt.Rows.Count.ToString());
            ViewState["dtProduct"] = dtTempDt;
        }
        else
        {
            int i = 0;
            dtTempDt = (DataTable)ViewState["dtProduct"];
            if (hidProduct.Value != "")
            {
                for (int j = 0; j < dtTempDt.Rows.Count; j++)
                {
                    if (dtTempDt.Rows[j]["Trans_Id"].ToString() == hidProduct.Value.ToString())
                    {
                        i = j;
                    }
                }
            }
            else
            {
                dtTempDt.Rows.Add();
                i = Convert.ToInt32(dtTempDt.Rows.Count - 1);
            }
            if (hidProduct.Value != "")
            {
                dtTempDt.Rows[i]["Trans_Id"] = hidProduct.Value.Trim();
            }
            else
            {
                try
                {
                    dtTempDt.Rows[i]["Trans_Id"] = (Convert.ToInt32(dtTempDt.Rows[i - 1]["Trans_Id"].ToString()) + 1).ToString();
                }
                catch
                {
                    dtTempDt.Rows[i]["Trans_Id"] = "1";
                }
            }
            try
            {
                if (hidProduct.Value == "")
                {
                    dtTempDt.Rows[i]["Serial_No"] = (Convert.ToInt32(dtTempDt.Rows[i - 1]["Trans_Id"].ToString()) + 1).ToString();
                }
            }
            catch
            {
                dtTempDt.Rows[i]["Serial_No"] = "1";
            }
            dtTempDt.Rows[i]["PoNo"] = ReqId.ToString();
            dtTempDt.Rows[i]["Product_Id"] = ProductId.ToString();
            dtTempDt.Rows[i]["ProductDescription"] = txtPDescription.Text.ToString();
            dtTempDt.Rows[i]["UnitId"] = UnitId.ToString();
            dtTempDt.Rows[i]["UnitCost"] = txtUnitCost.Text.ToString();
            dtTempDt.Rows[i]["OrderQty"] = txtOrderQty.Text.ToString();
            dtTempDt.Rows[i]["freeQty"] = txtfreeQty.Text.ToString();
            dtTempDt.Rows[i]["DiscountP"] = "0";
            dtTempDt.Rows[i]["DiscountV"] = "0.00";
            Add_Tax_In_Session(txtUnitCost.Text, ProductId, dtTempDt.Rows[i]["Serial_No"].ToString());
            dtTempDt.Rows[i]["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), dtTempDt.Rows[i]["Serial_No"].ToString());
            dtTempDt.Rows[i]["TaxV"] = Get_Tax_Amount(txtUnitCost.Text.ToString(), ProductId.ToString(), dtTempDt.Rows[i]["Serial_No"].ToString());
            ViewState["dtProduct"] = dtTempDt;
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dtTempDt, "", "");
        Session["dummyProduct"] = dtTempDt;
        dtTempDt = null;
        ViewState["DefaultCurrency"] = ddlCurrency.SelectedValue.ToString();
        if (ddlCurrency.SelectedIndex != 0)
        {
            gvProduct.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Unit Cost", Session["DBConnection"].ToString());
        }
        else
        {
            gvProduct.HeaderRow.Cells[8].Text = "Unit Cost";
        }
        //AllPageCode();
        ResetDetail();
        GridCalculation();
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
    public string GetLocalCurrencyConversion(string Price)
    {
        try
        {
            return ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), Price);
        }
        catch
        {
            return "0";
        }
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
    protected void Txt_Discount_Quatation_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label ProductId = (Label)Row.FindControl("lblgvProductId");
            TextBox Order_Qty = (TextBox)Row.FindControl("txtgvRequiredQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("txtUnitCost");
            TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Per_Quatation");
            TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Quatation");
            Label Tax_Per = (Label)Row.FindControl("lblTax");
            Label Tax_Value = (Label)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblgvAmmount");
            Label Serial_No = (Label)Row.FindControl("lbltrans");
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = double.Parse(((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void Txt_DiscountValue_Quatation_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label ProductId = (Label)Row.FindControl("lblgvProductId");
            TextBox Order_Qty = (TextBox)Row.FindControl("txtgvRequiredQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("txtUnitCost");
            TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Per_Quatation");
            TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Quatation");
            Label Tax_Per = (Label)Row.FindControl("lblTax");
            Label Tax_Value = (Label)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblgvAmmount");
            Label Serial_No = (Label)Row.FindControl("lbltrans");
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = Get_Discount_Percentage(F_Unit_Price.ToString(), ((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    public void GridCalculation()
    {
        double d = 0;
        foreach (GridViewRow gvrow in GvSalesOrderDetail.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("ChkSelect");
            TextBox Order_Qty = (TextBox)gvrow.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)gvrow.FindControl("txtgvUnitPrice");
            TextBox Discount_Percent = (TextBox)gvrow.FindControl("Txt_Discount_Sales");
            TextBox Discount_Value = (TextBox)gvrow.FindControl("Txt_DiscountValue_Sales");
            TextBox Tax_Per = (TextBox)gvrow.FindControl("Txt_Tax_Per_Sales");
            TextBox Tax_Value = (TextBox)gvrow.FindControl("Txt_Tax_Value_Sales");
            Label Net_Amount = (Label)gvrow.FindControl("lblLineTotal");
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (chk.Checked == true)
            {
                double Net_Discount = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Discount_Value.Text);
                double Net_Tax = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Tax_Value.Text);
                double Net_Unit_Price = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Unit_Price.Text);
                double Net_Line_Total = (Net_Unit_Price - Net_Discount) + Net_Tax;
                Net_Amount.Text = GetAmountDecimal(Net_Line_Total.ToString());
                d += Net_Line_Total;
            }
        }
        foreach (GridViewRow Row in gvProduct.Rows)
        {
            Label Order_Qty = (Label)Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Row.FindControl("lblUnitRate");
            TextBox Discount_Percent = (TextBox)Row.FindControl("lblDiscount");
            TextBox Discount_Value = (TextBox)Row.FindControl("lblDiscountValue");
            TextBox Tax_Per = (TextBox)Row.FindControl("lblTax");
            TextBox Tax_Value = (TextBox)Row.FindControl("lblTaxValue");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            double Net_Discount = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Discount_Value.Text);
            double Net_Tax = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Tax_Value.Text);
            double Net_Unit_Price = Convert.ToDouble(Order_Qty.Text) * Convert.ToDouble(Unit_Price.Text);
            double Net_Line_Total = (Net_Unit_Price - Net_Discount) + Net_Tax;
            Net_Amount.Text = GetAmountDecimal(Net_Line_Total.ToString());
            d += Net_Line_Total;
        }
        foreach (GridViewRow gvrow in gvQuatationProduct.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("chk");
            Label lblgvAmmount = (Label)gvrow.FindControl("lblgvAmmount");
            TextBox txtorderqty = (TextBox)gvrow.FindControl("txtgvRequiredQty");
            TextBox txtorderunitPrice = (TextBox)gvrow.FindControl("txtUnitCost");
            TextBox lblDiscountValue = (TextBox)gvrow.FindControl("Txt_DiscountValue_Quatation");
            Label lblTaxValue = (Label)gvrow.FindControl("lblTaxValue");
            Label lblQtyPrice = (Label)gvrow.FindControl("lblQtyPrice");
            if (txtorderqty.Text == "")
            {
                txtorderqty.Text = "0";
            }
            if (txtorderunitPrice.Text == "")
            {
                txtorderunitPrice.Text = "0";
            }
            lblQtyPrice.Text = GetAmountDecimal((Convert.ToDouble(txtorderunitPrice.Text) * Convert.ToDouble(txtorderqty.Text)).ToString());
            lblgvAmmount.Text = GetAmountDecimal(((Convert.ToDouble(txtorderunitPrice.Text) - Convert.ToDouble(lblDiscountValue.Text) + Convert.ToDouble(lblTaxValue.Text)) * Convert.ToDouble(txtorderqty.Text)).ToString());
            if (lblgvAmmount.Text == "")
            {
                lblgvAmmount.Text = "0";
            }
            if (chk.Checked == true)
            {
                d += Convert.ToDouble(lblgvAmmount.Text);
            }
        }
        foreach (GridViewRow gvRow in GvQuotationProductEdit.Rows)
        {
            Label lblNetprice = (Label)gvRow.FindControl("lblNetprice");
            TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQty");
            TextBox txtUnitRate = (TextBox)gvRow.FindControl("txtUnitRate");
            Label lblDiscountvalue = (Label)gvRow.FindControl("lblDiscountvalue");
            Label lbltaxvalue = (Label)gvRow.FindControl("lbltaxvalue");
            Label lblAmount = (Label)gvRow.FindControl("lblAmount");
            if (txtReqQty.Text == "")
            {
                txtReqQty.Text = "0";
            }
            if (txtUnitRate.Text == "")
            {
                txtUnitRate.Text = "0";
            }
            txtReqQty.Text = GetAmountDecimal(txtReqQty.Text);
            txtUnitRate.Text = GetAmountDecimal(txtUnitRate.Text);
            lblAmount.Text = GetAmountDecimal((Convert.ToDouble(txtUnitRate.Text) * Convert.ToDouble(txtReqQty.Text)).ToString());
            lblNetprice.Text = GetAmountDecimal(((Convert.ToDouble(txtUnitRate.Text) - Convert.ToDouble(lblDiscountvalue.Text) + Convert.ToDouble(lbltaxvalue.Text)) * Convert.ToDouble(txtReqQty.Text)).ToString());
            if (lblNetprice.Text == "")
            {
                lblNetprice.Text = "0";
            }
            d += Convert.ToDouble(lblNetprice.Text);
        }
        txtGrossAmount.Text = GetAmountDecimal(d.ToString());
        txtGrandTotal.Text = GetAmountDecimal(d.ToString());
        if (txtNetDutyPer.Text != "" && txtNetDutyPer.Text != "0")
        {
            txtNetDutyVal.Text = GetAmountDecimal(((d * Convert.ToDouble(txtNetDutyPer.Text)) / 100).ToString());
            txtGrandTotal.Text = GetAmountDecimal((Convert.ToDouble(txtGrossAmount.Text) + Convert.ToDouble(txtNetDutyVal.Text)).ToString());
        }
        //AllPageCode();
    }
    protected void Txt_Discount_Sales_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            HiddenField ProductId = (HiddenField)Row.FindControl("hdngvProductId");
            TextBox Order_Qty = (TextBox)Row.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Sales");
            TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Sales");
            TextBox Tax_Per = (TextBox)Row.FindControl("Txt_Tax_Per_Sales");
            TextBox Tax_Value = (TextBox)Row.FindControl("Txt_Tax_Value_Sales");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            Label Serial_No = (Label)Row.FindControl("lblgvSerialNo");
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = double.Parse(((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Value, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Value, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void Txt_DiscountValue_Sales_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
            HiddenField ProductId = (HiddenField)Row.FindControl("hdngvProductId");
            TextBox Order_Qty = (TextBox)Row.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Sales");
            TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Sales");
            TextBox Tax_Per = (TextBox)Row.FindControl("Txt_Tax_Per_Sales");
            TextBox Tax_Value = (TextBox)Row.FindControl("Txt_Tax_Value_Sales");
            Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
            Label Serial_No = (Label)Row.FindControl("lblgvSerialNo");
            if (Order_Qty.Text == "")
            {
                Order_Qty.Text = "0";
            }
            if (Unit_Price.Text == "")
            {
                Unit_Price.Text = "0";
            }
            if (Discount_Percent.Text == "")
            {
                Discount_Percent.Text = "0";
            }
            if (Discount_Value.Text == "")
            {
                Discount_Value.Text = "0";
            }
            if (Tax_Per.Text == "")
            {
                Tax_Per.Text = "0";
            }
            if (Tax_Value.Text == "")
            {
                Tax_Value.Text = "0";
            }
            if (Order_Qty.Text != "0")
            {
                if (Unit_Price.Text != "0")
                {
                    double F_Unit_Price = double.Parse(Unit_Price.Text);
                    double F_Order_Quantity = double.Parse(Order_Qty.Text);
                    double F_Discount_Percentage = Get_Discount_Percentage(F_Unit_Price.ToString(), ((TextBox)sender).Text);
                    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                    double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Value, Serial_No.Text);
                    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Value, Serial_No.Text);
                    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                    Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                    Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                    Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                    Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    ((TextBox)sender).Text = GetAmountDecimal(((TextBox)sender).Text);
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
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
                        DataTable DT_Db_Details = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value);
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            if (DT_Db_Details.Rows[0]["OrderType"].ToString() == "D")
                            {
                                // Purchase Order
                                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + HdnEdit.Value + "' and TRD.Ref_Type='PO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                                DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                                Session["Temp_Product_Tax_PO"] = null;
                                DataTable Dt_Temp = new DataTable();
                                Dt_Temp = TemporaryProductWiseTaxes();
                                Dt_Temp = Dt_Inv_TaxRefDetail;
                                if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                                {
                                    Session["Temp_Product_Tax_PO"] = Dt_Temp;
                                }
                            }
                            else if (DT_Db_Details.Rows[0]["OrderType"].ToString() == "R")
                            {
                                // Purchaes Quotation
                                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + HdnEdit.Value + "' and TRD.Ref_Type='PO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                                DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                                Session["Temp_Product_Tax_PO"] = null;
                                DataTable Dt_Temp = new DataTable();
                                Dt_Temp = TemporaryProductWiseTaxes();
                                Dt_Temp = Dt_Inv_TaxRefDetail;
                                if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                                {
                                    Session["Temp_Product_Tax_PO"] = Dt_Temp;
                                }
                                DataTable DT_Db_Details_Quotation = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value);
                                if (DT_Db_Details_Quotation.Rows.Count > 0)
                                {
                                    TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Field3 as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseQuoteDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + Hdn_Quot_Id.Value + "' and TRD.Ref_Type='PQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Field3";
                                    DataTable Dt_Inv_TaxRefDetail_1_1 = objDa.return_DataTable(TaxQuery);
                                    Session["Temp_Product_Tax_PO"] = null;
                                    DataTable Dt_Temp_1 = new DataTable();
                                    Dt_Temp_1 = TemporaryProductWiseTaxes();
                                    Dt_Temp_1 = Dt_Inv_TaxRefDetail_1_1;
                                    if (Dt_Inv_TaxRefDetail_1_1.Rows.Count > 0)
                                    {
                                        Session["Temp_Product_Tax_PO"] = Dt_Temp_1;
                                    }
                                }
                            }
                            else
                            {
                                //Purchae Sales
                                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + HdnEdit.Value + "' and TRD.Ref_Type='SO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                                DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                                Session["Temp_Product_Tax_PO"] = null;
                                DataTable Dt_Temp = new DataTable();
                                Dt_Temp = TemporaryProductWiseTaxes();
                                Dt_Temp = Dt_Inv_TaxRefDetail;
                                if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                                {
                                    Session["Temp_Product_Tax_PO"] = Dt_Temp;
                                }
                            }
                        }
                        DT_Db_Details = null;
                    }
                    else if (Hdn_Quot_Id.Value != "")
                    {
                        DataTable DT_Db_Details = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Hdn_Quot_Id.Value);
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Trans_Id as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseQuoteDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + Hdn_Quot_Id.Value + "' and TRD.Ref_Type='PQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Trans_Id";
                            DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_PO"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_PO"] = Dt_Temp;
                            }
                        }
                        DT_Db_Details = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_PO"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PO"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                string F_Serial_No = string.Empty;
                if (e.CommandName.ToString() == "gvProduct")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblSerialNO");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "gvQuatationProduct")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lbltrans");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "GvSalesOrderDetail")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "GvQuotationProductEdit")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblSerialNO");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "gvAddQuotationGrid")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("Lbl_Serial_No_Gv");
                    F_Serial_No = Serial_No.Text;
                }
                if (e.CommandName.ToString() == "gvAddSelesOrder")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }
                // Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + F_Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxCalculation.DataSource = Dt_Cal;
                    gvTaxCalculation.DataBind();
                    int Row_Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
                    string Grid_Name = e.CommandName.ToString();
                    if (Grid_Name == "gvAddQuotationGrid")
                    {
                        Label Serial_No = (Label)gvAddQuotationGrid.Rows[Row_Index].FindControl("Lbl_Serial_No_Gv");
                        Label Unit_Price = (Label)gvAddQuotationGrid.Rows[Row_Index].FindControl("lblUnitCost");
                        Label Discount_Price = (Label)gvAddQuotationGrid.Rows[Row_Index].FindControl("lblDiscountvalue");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    else if (Grid_Name == "gvProduct")
                    {
                        Label Serial_No = (Label)gvProduct.Rows[Row_Index].FindControl("lblSerialNO");
                        TextBox Unit_Price = (TextBox)gvProduct.Rows[Row_Index].FindControl("lblUnitRate");
                        TextBox Discount_Price = (TextBox)gvProduct.Rows[Row_Index].FindControl("lblDiscountValue");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    else if (Grid_Name == "GvQuotationProductEdit")
                    {
                        Label Serial_No = (Label)GvQuotationProductEdit.Rows[Row_Index].FindControl("lblSerialNO");
                        TextBox Unit_Price = (TextBox)GvQuotationProductEdit.Rows[Row_Index].FindControl("txtUnitRate");
                        Label Discount_Price = (Label)GvQuotationProductEdit.Rows[Row_Index].FindControl("lblDiscountvalue");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    else if (Grid_Name == "GvSalesOrderDetail")
                    {
                        Label Serial_No = (Label)GvSalesOrderDetail.Rows[Row_Index].FindControl("lblgvSerialNo");
                        TextBox Unit_Price = (TextBox)GvSalesOrderDetail.Rows[Row_Index].FindControl("txtgvUnitPrice");
                        TextBox Discount_Price = (TextBox)GvSalesOrderDetail.Rows[Row_Index].FindControl("Txt_DiscountValue_Sales");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    else if (Grid_Name == "gvQuatationProduct")
                    {
                        Label Serial_No = (Label)gvQuatationProduct.Rows[Row_Index].FindControl("lbltrans");
                        TextBox Unit_Price = (TextBox)gvQuatationProduct.Rows[Row_Index].FindControl("txtUnitCost");
                        TextBox Discount_Price = (TextBox)gvQuatationProduct.Rows[Row_Index].FindControl("Txt_DiscountValue_Quatation");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    else if (Grid_Name == "gvAddSelesOrder")
                    {
                        Label Serial_No = (Label)gvAddSelesOrder.Rows[Row_Index].FindControl("lblgvSerialNo");
                        TextBox Unit_Price = (TextBox)gvAddSelesOrder.Rows[Row_Index].FindControl("txtgvUnitPrice");
                        //TextBox Discount_Price = (TextBox)gvQuatationProduct.Rows[Row_Index].FindControl("Txt_DiscountValue_Quatation");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = "0";
                    }
                    Hdn_Serial_No_Tax.Value = F_Serial_No;
                    Hdn_Product_Id_Tax.Value = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                }
                else
                {
                    DisplayMessage("No Tax Details found");
                    return;
                }
            }
            Dt_Cal = null;
        }
        else
        {
            DisplayMessage("No Tax Details found");
            return;
        }
    }
    public void Add_Tax_In_Session(string Amount, string ProductId, string Serial_No)
    {
        string TaxQuery = string.Empty;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
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
                DataTable dtTax = objDa.return_DataTable(TaxQuery);
                double TotalPriceBeforeDiscount = double.Parse(Amount);
                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_PO"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_PO"];
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
                        Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
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
                            SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Serial_No"] = Serial_No;
                        }
                    }
                    Session["Temp_Product_Tax_PO"] = dt;
                }
                dtTax = null;
                dt = null;
            }
        }
    }
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        string Serial_No = Hdn_Serial_No_Tax.Value;
        string Product_ID = Hdn_Product_Id_Tax.Value;
        string Unit_Price = Hdn_unit_Price_Tax.Value;
        string Unit_Discount = Hdn_Discount_Tax.Value;
        string Net_Unit_Price = (Convert.ToDouble(Unit_Price) - Convert.ToDouble(Unit_Discount)).ToString();
        if (Session["Temp_Product_Tax_PO"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PO"] as DataTable;
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
            Session["Temp_Product_Tax_PO"] = Dt_Cal;
            try
            {
                bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (IsTax == true)
                {
                    if (ddlTransType.SelectedIndex > 0)
                    {
                        if (HdnEdit.Value != "")
                        {
                            DataTable DT_Db_Details = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), HdnEdit.Value);
                            if (DT_Db_Details.Rows.Count > 0)
                            {
                                if (DT_Db_Details.Rows[0]["OrderType"].ToString() == "D")
                                {
                                    foreach (GridViewRow dl in gvProduct.Rows)
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
                                                    TextBox Tax_Percent = (TextBox)dl.FindControl("lblTax");
                                                    Label hdnProductId = (Label)dl.FindControl("lblGvProductId");
                                                    Label Serial_No_GV = (Label)dl.FindControl("lblSerialNO");
                                                    //if (Product_ID == hdnProductId.Text)
                                                    if (Product_ID == hdnProductId.Text && Serial_No == Serial_No_GV.Text)
                                                    {
                                                        Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                                    }
                                                    Dt_Cal_Temp2 = null;
                                                }
                                            }
                                            Dt_Cal_Temp = null;
                                        }
                                    }
                                    Product_Grid_Calculation();
                                }
                                else if (DT_Db_Details.Rows[0]["OrderType"].ToString() == "R")
                                {
                                    Order_Edit_With_Quotation_Grid_Calculation();
                                }
                                else
                                {
                                }
                            }
                            if (ddlOrderType.SelectedValue == "R" && ddlReferenceVoucherType.SelectedValue == "PQ" && txtReferenceNo.Text != "")
                            {
                                foreach (GridViewRow dl in gvAddQuotationGrid.Rows)
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
                                                Label Tax_Percent = (Label)dl.FindControl("lbltaxpercentage");
                                                Label hdnProductId = (Label)dl.FindControl("lblgvProductId");
                                                Label Serial_No_GV = (Label)dl.FindControl("Lbl_Serial_No_Gv");
                                                //if (Product_ID == hdnProductId.Text)
                                                if (Product_ID == hdnProductId.Text && Serial_No == Serial_No_GV.Text)
                                                {
                                                    Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                                }
                                                Dt_Cal_Temp2 = null;
                                            }
                                        }
                                        Dt_Cal_Temp = null;
                                    }
                                }
                                Quotation_Grid_Calculation();
                            }
                            DT_Db_Details = null;
                        }
                        else if (Hdn_Quot_Id.Value != "")
                        {
                            foreach (GridViewRow dl in gvQuatationProduct.Rows)
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
                                            Label Tax_Percent = (Label)dl.FindControl("lblTax");
                                            Label hdnProductId = (Label)dl.FindControl("lblgvProductId");
                                            Label Serial_No_GV = (Label)dl.FindControl("lbltrans");
                                            if (Product_ID == hdnProductId.Text && Serial_No == Serial_No_GV.Text)
                                            {
                                                Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                            }
                                            Dt_Cal_Temp2 = null;
                                        }
                                    }
                                    Dt_Cal_Temp = null;
                                }
                            }
                            Quotation_Grid_Calculation();
                        }
                        else if (ddlOrderType.SelectedValue == "R" && ddlReferenceVoucherType.SelectedValue == "SO" && txtReferenceNo.Text != "")
                        {
                            foreach (GridViewRow dl in GvSalesOrderDetail.Rows)
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
                                            TextBox Tax_Percent = (TextBox)dl.FindControl("Txt_Tax_Per_Sales");
                                            HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                                            Label Serial_No_GV = (Label)dl.FindControl("lblgvSerialNo");
                                            if (Product_ID == hdnProductId.Value && Serial_No == Serial_No_GV.Text)
                                            {
                                                Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                            }
                                            Dt_Cal_Temp2 = null;
                                        }
                                    }
                                    Dt_Cal_Temp = null;
                                }
                            }
                            Quotation_Grid_Calculation();
                        }
                        else if (HdnEdit.Value == "")
                        {
                            foreach (GridViewRow dl in gvProduct.Rows)
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
                                            TextBox Tax_Percent = (TextBox)dl.FindControl("lblTax");
                                            Label hdnProductId = (Label)dl.FindControl("lblGvProductId");
                                            Label Serial_No_GV = (Label)dl.FindControl("lblSerialNO");
                                            if (Product_ID == hdnProductId.Text && Serial_No == Serial_No_GV.Text)
                                            {
                                                Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                            }
                                            Dt_Cal_Temp2 = null;
                                        }
                                    }
                                    Dt_Cal_Temp = null;
                                }
                            }
                            Product_Grid_Calculation();
                        }
                    }
                }
                Dt_Cal = null;
            }
            catch (Exception ex)
            {
            }
            Hdn_Serial_No_Tax.Value = "";
            Hdn_Product_Id_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Order_Edit_With_Quotation_Grid_Calculation()
    {
        try
        {
            foreach (GridViewRow Row in GvQuotationProductEdit.Rows)
            {
                Label ProductId = (Label)Row.FindControl("lblGvProductId");
                TextBox Order_Qty = (TextBox)Row.FindControl("txtReqQty");
                TextBox Unit_Price = (TextBox)Row.FindControl("txtUnitRate");
                Label Discount_Percent = (Label)Row.FindControl("lblDiscountPercentage");
                Label Discount_Value = (Label)Row.FindControl("lblDiscountvalue");
                Label Tax_Per = (Label)Row.FindControl("lbltaxpercentage");
                Label Tax_Value = (Label)Row.FindControl("lbltaxvalue");
                Label Net_Amount = (Label)Row.FindControl("lblNetprice");
                Label Serial_No = (Label)Row.FindControl("lblSerialNO");
                if (Order_Qty.Text == "")
                {
                    Order_Qty.Text = "0";
                }
                if (Unit_Price.Text == "")
                {
                    Unit_Price.Text = "0";
                }
                if (Discount_Percent.Text == "")
                {
                    Discount_Percent.Text = "0";
                }
                if (Discount_Value.Text == "")
                {
                    Discount_Value.Text = "0";
                }
                if (Tax_Per.Text == "")
                {
                    Tax_Per.Text = "0";
                }
                if (Tax_Value.Text == "")
                {
                    Tax_Value.Text = "0";
                }
                if (Order_Qty.Text != "0")
                {
                    if (Unit_Price.Text != "0")
                    {
                        double F_Unit_Price = double.Parse(Unit_Price.Text);
                        double F_Order_Quantity = double.Parse(Order_Qty.Text);
                        double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                        double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                        double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                        double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                        double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                        double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                        Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                        Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                        Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                        Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                        Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    }
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    public void Quotation_Grid_Calculation()
    {
        try
        {
            foreach (GridViewRow Row in gvQuatationProduct.Rows)
            {
                Label ProductId = (Label)Row.FindControl("lblgvProductId");
                TextBox Order_Qty = (TextBox)Row.FindControl("txtgvRequiredQty");
                TextBox Unit_Price = (TextBox)Row.FindControl("txtUnitCost");
                TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Per_Quatation");
                TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Quatation");
                Label Tax_Per = (Label)Row.FindControl("lblTax");
                Label Tax_Value = (Label)Row.FindControl("lblTaxValue");
                Label Net_Amount = (Label)Row.FindControl("lblgvAmmount");
                Label Serial_No = (Label)Row.FindControl("lbltrans");
                if (Order_Qty.Text == "")
                {
                    Order_Qty.Text = "0";
                }
                if (Unit_Price.Text == "")
                {
                    Unit_Price.Text = "0";
                }
                if (Discount_Percent.Text == "")
                {
                    Discount_Percent.Text = "0";
                }
                if (Discount_Value.Text == "")
                {
                    Discount_Value.Text = "0";
                }
                if (Tax_Per.Text == "")
                {
                    Tax_Per.Text = "0";
                }
                if (Tax_Value.Text == "")
                {
                    Tax_Value.Text = "0";
                }
                if (Order_Qty.Text != "0")
                {
                    if (Unit_Price.Text != "0")
                    {
                        double F_Unit_Price = double.Parse(Unit_Price.Text);
                        double F_Order_Quantity = double.Parse(Order_Qty.Text);
                        double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                        double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                        double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                        double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                        double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                        double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                        Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                        Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                        Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                        Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                        Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    }
                }
            }
            // Quotation not in order
            foreach (GridViewRow Row in gvAddQuotationGrid.Rows)
            {
                Label ProductId = (Label)Row.FindControl("lblgvProductId");
                Label Order_Qty = (Label)Row.FindControl("lblgvRequiredQty");
                Label Unit_Price = (Label)Row.FindControl("lblUnitCost");
                Label Discount_Percent = (Label)Row.FindControl("lblDiscountPercentage");
                Label Discount_Value = (Label)Row.FindControl("lblDiscountvalue");
                Label Tax_Per = (Label)Row.FindControl("lbltaxpercentage");
                Label Tax_Value = (Label)Row.FindControl("lbltaxvalue");
                Label Net_Amount = (Label)Row.FindControl("lblNetprice");
                Label Serial_No = (Label)Row.FindControl("Lbl_Serial_No_Gv");
                if (Order_Qty.Text == "")
                {
                    Order_Qty.Text = "0";
                }
                if (Unit_Price.Text == "")
                {
                    Unit_Price.Text = "0";
                }
                if (Discount_Percent.Text == "")
                {
                    Discount_Percent.Text = "0";
                }
                if (Discount_Value.Text == "")
                {
                    Discount_Value.Text = "0";
                }
                if (Tax_Per.Text == "")
                {
                    Tax_Per.Text = "0";
                }
                if (Tax_Value.Text == "")
                {
                    Tax_Value.Text = "0";
                }
                if (Order_Qty.Text != "0")
                {
                    if (Unit_Price.Text != "0")
                    {
                        double F_Unit_Price = double.Parse(Unit_Price.Text);
                        double F_Order_Quantity = double.Parse(Order_Qty.Text);
                        double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                        double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                        double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                        double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                        double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                        double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                        Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                        Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                        Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                        Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                        Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    }
                }
            }
            // Quotation not in order
            foreach (GridViewRow Row in GvSalesOrderDetail.Rows)
            {
                HiddenField ProductId = (HiddenField)Row.FindControl("hdngvProductId");
                TextBox Order_Qty = (TextBox)Row.FindControl("txtgvQuantity");
                TextBox Unit_Price = (TextBox)Row.FindControl("txtgvUnitPrice");
                TextBox Discount_Percent = (TextBox)Row.FindControl("Txt_Discount_Sales");
                TextBox Discount_Value = (TextBox)Row.FindControl("Txt_DiscountValue_Sales");
                TextBox Tax_Per = (TextBox)Row.FindControl("Txt_Tax_Per_Sales");
                TextBox Tax_Value = (TextBox)Row.FindControl("Txt_Tax_Value_Sales");
                Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
                Label Serial_No = (Label)Row.FindControl("lblgvSerialNo");
                if (Order_Qty.Text == "")
                {
                    Order_Qty.Text = "0";
                }
                if (Unit_Price.Text == "")
                {
                    Unit_Price.Text = "0";
                }
                if (Discount_Percent.Text == "")
                {
                    Discount_Percent.Text = "0";
                }
                if (Discount_Value.Text == "")
                {
                    Discount_Value.Text = "0";
                }
                if (Tax_Per.Text == "")
                {
                    Tax_Per.Text = "0";
                }
                if (Tax_Value.Text == "")
                {
                    Tax_Value.Text = "0";
                }
                if (Order_Qty.Text != "0")
                {
                    if (Unit_Price.Text != "0")
                    {
                        double F_Unit_Price = double.Parse(Unit_Price.Text);
                        double F_Order_Quantity = double.Parse(Order_Qty.Text);
                        double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                        double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                        double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Value, Serial_No.Text);
                        double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Value, Serial_No.Text);
                        double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                        double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                        Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                        Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                        Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                        Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                        Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    }
                }
            }
            GridCalculation();
            //AllPageCode();
        }
        catch
        {
        }
    }
    public void Product_Grid_Calculation()
    {
        try
        {
            foreach (GridViewRow Row in gvProduct.Rows)
            {
                Label ProductId = (Label)Row.FindControl("lblGvProductId");
                Label Order_Qty = (Label)Row.FindControl("lblReqQty");
                TextBox Unit_Price = (TextBox)Row.FindControl("lblUnitRate");
                TextBox Discount_Percent = (TextBox)Row.FindControl("lblDiscount");
                TextBox Discount_Value = (TextBox)Row.FindControl("lblDiscountValue");
                TextBox Tax_Per = (TextBox)Row.FindControl("lblTax");
                TextBox Tax_Value = (TextBox)Row.FindControl("lblTaxValue");
                Label Net_Amount = (Label)Row.FindControl("lblLineTotal");
                Label Serial_No = (Label)Row.FindControl("lblSerialNO");
                TextBox lblTax = (TextBox)Row.FindControl("lblTax");
                ImageButton BtnAddTax = (ImageButton)Row.FindControl("BtnAddTax");
                //lblTax.Visible = false;
                //BtnAddTax.Visible = true;
                if (Order_Qty.Text == "")
                {
                    Order_Qty.Text = "0";
                }
                if (Unit_Price.Text == "")
                {
                    Unit_Price.Text = "0";
                }
                if (Discount_Percent.Text == "")
                {
                    Discount_Percent.Text = "0";
                }
                if (Discount_Value.Text == "")
                {
                    Discount_Value.Text = "0";
                }
                if (Tax_Per.Text == "")
                {
                    Tax_Per.Text = "0";
                }
                if (Tax_Value.Text == "")
                {
                    Tax_Value.Text = "0";
                }
                if (Order_Qty.Text != "0")
                {
                    if (Unit_Price.Text != "0")
                    {
                        Add_Tax_In_Session(Unit_Price.Text, ProductId.Text, Serial_No.Text);
                        double F_Unit_Price = double.Parse(Unit_Price.Text);
                        double F_Order_Quantity = double.Parse(Order_Qty.Text);
                        double F_Discount_Percentage = double.Parse(Discount_Percent.Text);
                        double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
                        double F_Tax_Percentage = Get_Tax_Percentage(ProductId.Text, Serial_No.Text);
                        double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), ProductId.Text, Serial_No.Text);
                        double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                        double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                        Discount_Percent.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
                        Discount_Value.Text = GetAmountDecimal(F_Discount_Amount.ToString());
                        Tax_Per.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
                        Tax_Value.Text = GetAmountDecimal(F_Tax_Amount.ToString());
                        Net_Amount.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
                    }
                }
                GridCalculation();
                //AllPageCode();
            }
        }
        catch
        {
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row, ref SqlTransaction trns)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PO", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string R_Serial_No = string.Empty;
        string Grid = string.Empty;
        if (Grid_Name == "gvProduct")
        {
            Grid = "gvProduct";
            Label Product_ID = (Label)Gv_Row.FindControl("lblGvProductId");
            Label Order_Req_Qty = (Label)Gv_Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblUnitRate");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblDiscountValue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblSerialNO");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvQuatationProduct")
        {
            Grid = "gvQuatationProduct";
            Label Product_ID = (Label)Gv_Row.FindControl("lblgvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvRequiredQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtUnitCost");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("Txt_DiscountValue_Quatation");
            Label Serial_No = (Label)Gv_Row.FindControl("lbltrans");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "GvSalesOrderDetail")
        {
            Grid = "GvSalesOrderDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("Txt_DiscountValue_Sales");
            Label Serial_No = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "GvQuotationProductEdit")
        {
            Grid = "GvQuotationProductEdit";
            Label Product_ID = (Label)Gv_Row.FindControl("lblGvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtReqQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtUnitRate");
            Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblSerialNO");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvAddQuotationGrid")
        {
            Grid = "gvAddQuotationGrid";
            Label Product_ID = (Label)Gv_Row.FindControl("lblgvProductId");
            Label Order_Req_Qty = (Label)Gv_Row.FindControl("lblgvRequiredQty");
            Label Unit_Price = (Label)Gv_Row.FindControl("lblUnitCost");
            Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("Lbl_Serial_No_Gv");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvAddSelesOrder")
        {
            Grid = "gvAddSelesOrder";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvQuantity");
            //TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            //Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            //R_Unit_Price = Unit_Price.Text;
            //R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        if (Grid != "")
        {
            double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
            double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
            double Net_Amount = A_Unit_Cost - A_Unit_Discount;
            //Get_Tax_Insert(R_Product_ID);
            DataTable ProductTax = new DataTable();
            if (Session["Temp_Product_Tax_PO"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_PO"];
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
                        objTaxRefDetail.InsertRecord("PO", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }
            ProductTax = null;
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PO", PQ_Header_ID.ToString(), Detail_ID.ToString());
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
            Label Order_Req_Qty = (Label)Gv_Row.FindControl("lblReqQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblUnitRate");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("lblDiscountValue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblSerialNO");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvQuatationProduct")
        {
            Grid = "gvQuatationProduct";
            Label Product_ID = (Label)Gv_Row.FindControl("lblgvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvRequiredQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtUnitCost");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("Txt_DiscountValue_Quatation");
            Label Serial_No = (Label)Gv_Row.FindControl("lbltrans");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "GvSalesOrderDetail")
        {
            Grid = "GvSalesOrderDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("Txt_DiscountValue_Sales");
            Label Serial_No = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "GvQuotationProductEdit")
        {
            Grid = "GvQuotationProductEdit";
            Label Product_ID = (Label)Gv_Row.FindControl("lblGvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtReqQty");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtUnitRate");
            Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblSerialNO");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvAddQuotationGrid")
        {
            Grid = "gvAddQuotationGrid";
            Label Product_ID = (Label)Gv_Row.FindControl("lblgvProductId");
            Label Order_Req_Qty = (Label)Gv_Row.FindControl("lblgvRequiredQty");
            Label Unit_Price = (Label)Gv_Row.FindControl("lblUnitCost");
            Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("Lbl_Serial_No_Gv");
            R_Product_ID = Product_ID.Text;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = Serial_No.Text;
        }
        else if (Grid_Name == "gvAddSelesOrder")
        {
            Grid = "gvAddSelesOrder";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            Label Discount_Value = (Label)Gv_Row.FindControl("lblDiscountvalue");
            Label Serial_No = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            //R_Discount_Value = Discount_Value.Text;
            R_Discount_Value = "0";
            R_Serial_No = Serial_No.Text;
        }
        if (Grid != "")
        {
            double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
            double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
            double Net_Amount = A_Unit_Cost - A_Unit_Discount;
            //Get_Tax_Insert(R_Product_ID);
            DataTable ProductTax = new DataTable();
            if (Session["Temp_Product_Tax_PO"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_PO"];
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
                        // if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("PO", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
            ProductTax = null;
        }
    }
    public double Get_Discount_Percentage(string Unit_Price, string Discount_Amount)
    {
        try
        {
            double Discount_Percent = 0;
            if (Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
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
    public double Get_Discount_Amount(string Unit_Price, string Discount_Percent)
    {
        try
        {
            double Discount_Amount = 0;
            if (Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
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
    public double Get_Tax_Percentage(string ProductId, string Serial_No)
    {
        double TotalTax_Percentage = 0;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PO"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_PO"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
                Dt_Session_Tax = null;
            }
        }
        return TotalTax_Percentage;
    }
    public double Get_Tax_Amount(string Amount_After_Discount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PO"] != null)
            {
                double Tax_Value = Get_Tax_Percentage(ProductId, Serial_No);
                double Temp_Amount = Convert.ToDouble(Amount_After_Discount);
                TotalTax_Amount = Convert.ToDouble(GetAmountDecimal(((Tax_Value * Temp_Amount) / 100).ToString()));
            }
        }
        return TotalTax_Amount;
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
            if (txtpaidamount.Text == "")
            {
                DisplayMessage("Please Enter Paid Amount");
                txtpaidamount.Focus();
                return;
            }
            if (Convert.ToDouble(txtpaidamount.Text) == 0)
            {
                DisplayMessage("Please Enter Paid Amount");
                txtpaidamount.Focus();
                return;
            }
            Hdn_Expenses_Id_Web_Control.Value = ddlExpense.SelectedValue.ToString();
            Hdn_Expenses_Name_Web_Control.Value = ddlExpense.SelectedItem.ToString();
            Hdn_Expenses_Amount_Web_Control.Value = txtpaidamount.Text.Trim();
            Hdn_Saved_Expenses_Tax_Session.Value = "Expenses_Tax_Purchase_Order";
            Hdn_Page_Name_Web_Control.Value = "Purchase_Order";
            Hdn_Tax_Entry_Type.Value = "Single";
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
    public string Get_Expenses_Tax_Amount()
    {
        string Exp_Amount = "0";
        try
        {
            double Expenses_Amount = 0;
            if (txtpaidamount.Text.Trim() == "")
                txtpaidamount.Text = "0";
            if (Lbl_Expenses_Tax_Amount_ET.Text.Trim() == "")
                Lbl_Expenses_Tax_Amount_ET.Text = "0";
            Expenses_Amount = Convert.ToDouble(txtpaidamount.Text) + Convert.ToDouble(Lbl_Expenses_Tax_Amount_ET.Text);
            Exp_Amount = Common.GetAmountDecimal_By_Location(Expenses_Amount.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()).ToString();
        }
        catch
        {
            return "0";
        }
        return Exp_Amount;
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
            if (txtpaidamount.Text.Trim() != "")
            {
                double Expenses_Tax_Amount = Convert.ToDouble(txtpaidamount.Text.Trim());
                if (Expenses_Tax_Amount > 0)
                {
                    Lbl_Expenses_Tax_Amount_ET.Text = ((Expenses_Tax_Amount * Net_Tax) / 100).ToString();
                    Lbl_Expenses_Tax_ET.Text = Net_Tax.ToString() + "%";
                }
                else
                {
                    Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                    Lbl_Expenses_Tax_ET.Text = "0%";
                }
            }
            else
            {
                Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
                Lbl_Expenses_Tax_ET.Text = "0%";
            }
        }
    }
    protected void txtpaidamount_TextChanged(object sender, EventArgs e)
    {
        Session["Expenses_Tax_Purchase_Order"] = null;
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
    }
    public void Tax_Insert_Into_Ac_Voucher_Detail_Debit(string strVoucher_No, string strSerial_No, string strAccount_No, string strOther_Account_No, string strRef_Id, string strRef_Type, string strCheque_Issue_Date, string strCheque_Clear_Date, string strCheque_No, string strDebit_Amount, string strCredit_Amount, string strNarration, string strCostCenter_ID, string strEmp_Id, string strCurrency_Id, string strExchange_Rate, string strForeign_Amount, string strCompanyCurrDebit, string strCompanyCurrCredit, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            DataTable Dt_Final_Save_Tax = Session["Dt_Final_Save_Tax"] as DataTable;
            if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
            {
                string strNarration_Temp = string.Empty;
                string strDebit_Amount_Temp = string.Empty;
                string strForeign_Amount_Temp = string.Empty;
                string strCompanyCurrDebit_Temp = string.Empty;
                foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                {
                    strNarration_Temp = strNarration;
                    strDebit_Amount_Temp = strDebit_Amount;
                    strForeign_Amount_Temp = strForeign_Amount;
                    strCompanyCurrDebit_Temp = strCompanyCurrDebit;
                    if (Dt_Row["Expenses_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                    {
                        double Tax_Percentage = Convert.ToDouble(Common.GetAmountDecimal_By_Location(Dt_Row["Tax_Percentage"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()));
                        if (strDebit_Amount_Temp != "")
                            strDebit_Amount_Temp = ((Convert.ToDouble(strDebit_Amount_Temp) * Tax_Percentage) / 100).ToString();
                        if (strForeign_Amount_Temp != "")
                            strForeign_Amount_Temp = ((Convert.ToDouble(strForeign_Amount_Temp) * Tax_Percentage) / 100).ToString();
                        if (strCompanyCurrDebit_Temp != "")
                            strCompanyCurrDebit_Temp = ((Convert.ToDouble(strCompanyCurrDebit_Temp) * Tax_Percentage) / 100).ToString();
                        strNarration_Temp = Common.GetAmountDecimal_By_Location(Dt_Row["Tax_Percentage"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + "% " + Dt_Row["Tax_Type_Name"].ToString() + " On " + ddlExpense.SelectedItem.ToString() + " for " + strNarration;
                        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVoucher_No, strSerial_No, Dt_Row["Tax_Account_Id"].ToString(), strOther_Account_No, strRef_Id, strRef_Type, strCheque_Issue_Date, strCheque_Clear_Date, strCheque_No, strDebit_Amount_Temp, strCredit_Amount, strNarration_Temp, strCostCenter_ID, strEmp_Id, strCurrency_Id, strExchange_Rate, strForeign_Amount_Temp, strCompanyCurrDebit_Temp, strCompanyCurrCredit, strField1, strField2, strField3, strField4, strField5, strField6, strField7, strIsActive, strCreatedBy, strCreatedDate, strModifiedBy, strModifiedDate);
                    }
                }
            }
            Dt_Final_Save_Tax = null;
        }
    }
    public void Tax_Insert_Into_Inv_TaxRefDetail(string Po_Header_ID)
    {
        bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PO", Po_Header_ID.ToString());
            DataTable Dt_Final_Save_Tax = Session["Dt_Final_Save_Tax"] as DataTable;
            if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
            {
                foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                {
                    if (Dt_Row["Expenses_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                    {
                        objTaxRefDetail.InsertRecord_Expenses("PO", Po_Header_ID, "0", "0", "0", Dt_Row["Tax_Type_Id"].ToString(), Dt_Row["Tax_Percentage"].ToString(), Lbl_Expenses_Tax_Amount_ET.Text.Trim(), false.ToString(), txtpaidamount.Text.Trim(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlExpense.SelectedValue.ToString());
                    }
                }
            }
            Dt_Final_Save_Tax = null;
        }
    }
    public void Sales_Order_Tax_According_Reference_No(string Sales_Odr_Id, string ReferenceNo)
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    DataTable DT_Db_Details = ObjSalesOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, Sales_Odr_Id);
                    if (DT_Db_Details.Rows.Count > 0)
                    {
                        TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2  where TRD.Ref_Id='" + Sales_Odr_Id + "' and TRD.Ref_Type='SO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                        DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                        Session["Temp_Product_Tax_PO"] = null;
                        DataTable Dt_Temp = new DataTable();
                        Dt_Temp = TemporaryProductWiseTaxes();
                        Dt_Temp = Dt_Inv_TaxRefDetail;
                        if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                        {
                            Session["Temp_Product_Tax_PO"] = Dt_Temp;
                        }
                        Dt_Temp = null;
                        Dt_Inv_TaxRefDetail = null;
                    }
                    DT_Db_Details = null;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Add_Quotation_Tax_On_Edit(string Quot_ID, string Supplier_Id)
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
                        DataTable DT_Db_Details = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Quot_ID);
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Field3 as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseQuoteDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + Quot_ID + "' and TRD.Ref_Type='PQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Field3";
                            DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                            //Session["Temp_Product_Tax_PO"] = null;
                            DataTable Dt_Temp = new DataTable();
                            //Dt_Temp = TemporaryProductWiseTaxes();
                            //Dt_Temp = Dt_Inv_TaxRefDetail;
                            //DataTable Dt_Temp = new DataTable();
                            if (Session["Temp_Product_Tax_PO"] == null)
                                Dt_Temp = TemporaryProductWiseTaxes();
                            else
                                Dt_Temp = (DataTable)Session["Temp_Product_Tax_PO"];
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                foreach (DataRow dr in Dt_Inv_TaxRefDetail.Rows)
                                {
                                    double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                                    //double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                                    DataRow Newdr = Dt_Temp.NewRow();
                                    Newdr["Product_Id"] = dr["Product_Id"].ToString();
                                    Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                                    Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                                    Newdr["Tax_Value"] = dr["Tax_Value"].ToString();
                                    Newdr["TaxAmount"] = "0";
                                    Newdr["Amount"] = "0";
                                    Newdr["Serial_No"] = dr["Serial_No"].ToString();
                                    //DataRow[] SRow = Dt_Temp.Select("Product_id = " + dr["Product_Id"].ToString() + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                                    DataRow[] SRow = Dt_Temp.Select("Product_id = " + dr["Product_Id"].ToString() + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + dr["Serial_No"].ToString() + "'");
                                    if (SRow.Length == 0)
                                    {
                                        Dt_Temp.Rows.Add(Newdr);
                                    }
                                }
                                Session["Temp_Product_Tax_PO"] = Dt_Temp;
                            }
                            Dt_Temp = null;
                            Dt_Inv_TaxRefDetail = null;
                        }
                        DT_Db_Details = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Quotation_Tax_According_Reference_No(string Quot_Id, string ReferenceNo)
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    DataTable DT_Db_Details = ObjQuoteDetail.GetPurchaseQuationDatilsNotInPOdetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Quot_Id);
                    if (DT_Db_Details.Rows.Count > 0)
                    {
                        TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Trans_Id as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseQuoteDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + Quot_Id + "' and TRD.Ref_Type='PQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Trans_Id";
                        DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                        Session["Temp_Product_Tax_PO"] = null;
                        DataTable Dt_Temp = new DataTable();
                        Dt_Temp = TemporaryProductWiseTaxes();
                        Dt_Temp = Dt_Inv_TaxRefDetail;
                        if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                        {
                            Session["Temp_Product_Tax_PO"] = Dt_Temp;
                        }
                        Dt_Temp = null;
                        Dt_Inv_TaxRefDetail = null;
                    }
                    DT_Db_Details = null;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/PO", "Purchase", "Purchase Order", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion
    protected void fillSupplierCurrency(string strSupplierId)
    {
        ddlCurrency.Items.Clear();
        //Code added by neelkanth purohit - 12/09/2018 (as per discussion with neeraj sir currency should display as per the selected supplier accounts)
        string sql = "select sys_currencymaster.Currency_ID,sys_currencymaster.currency_name from sys_currencymaster inner join ac_accountmaster on ac_accountmaster.currency_id=sys_currencymaster.currency_id where ac_accountmaster.ref_type='Supplier' and Ref_Id='" + strSupplierId + "' and ac_accountmaster.Is_Active='true'";
        DataTable _dtCurrency = new DataTable();
        try
        {
            _dtCurrency = objDa.return_DataTable(sql);
            if (_dtCurrency.Rows.Count == 0)
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
                ddlPayCurrency.Items.Insert(0, "--Select--");
                ddlPayCurrency.SelectedIndex = 0;
            }
            else
            {
                objPageCmn.FillData((object)ddlCurrency, _dtCurrency, "Currency_Name", "Currency_Id");
                objPageCmn.FillData((object)ddlPayCurrency, _dtCurrency, "Currency_Name", "Currency_Id");

                ddlCurrency.SelectedValue = _dtCurrency.Rows[0]["Currency_ID"].ToString();

                ddlCurrency_SelectedIndexChanged(null, null);
            }
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
            ddlPayCurrency.Items.Insert(0, "--Select--");
            ddlPayCurrency.SelectedIndex = 0;
        }
        _dtCurrency.Dispose();
        //-----------------------end-----------------------------------
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void ddlTransType_SelectedIndexChanged()
    {
        HttpContext.Current.Session["Temp_Product_Tax_PO"] = null;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] IbtnQC_Command(string referenceId, string referenceVoucherType)
    {
        string[] result = new string[2];
        string id = "0";
        id = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).get_SingleValue("SELECT RPQ_No FROM Inv_PurchaseQuoteHeader WHERE Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' AND Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "' AND IsActive = 'True' AND Trans_Id = '" + referenceId + "'");
        id = id == "@NOTFOUND@" ? "" : id;
        if (id == "")
        {
            result[0] = "false";
            result[1] = id;
        }
        else
        {
            result[0] = "true";
            result[1] = HttpUtility.UrlEncode(EncryptString(id.ToString())); ;
        }
        return result;
    }
    public static string EncryptString(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] IbtnPrint_Command(string purchaseOrderId)
    {
        string[] result = new string[2];
        DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        result[0] = "false";
        result[1] = "Dont Have Permission";

        string hasPermission = "";
        hasPermission = objda.get_SingleValue("Select ParameterValue From Inv_ParameterMaster where Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' AND Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'  and ParameterName='PurchaseOrderApproval' and IsActive='True'");
        hasPermission = hasPermission == "@NOTFOUND" ? "" : hasPermission;

        if (hasPermission != "")
        {
            if (hasPermission == "true")
            {
                string status = "";
                status = objda.get_SingleValue("SELECT Field4 FROM Inv_PurchaseOrderHeader WHERE TransID = '" + purchaseOrderId + "' ");
                if (status == "Approved")
                {
                    result[0] = "true";
                    result[1] = purchaseOrderId;
                }
                else
                {
                    result[0] = "false";
                    result[1] = "Order has not Approved, So Cannot be Printed";
                }
            }
            else
            {
                result[0] = "true";
                result[1] = purchaseOrderId;
            }
        }
        return result;
    }


    protected void btnFillUnit_Click(object sender, EventArgs e)
    {
        FillUnit(hdnProductID.Value);
        txtPDescription.Text = hdnProductDesc.Value;
    }
    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static string[] txtShippingLine_TextChanged(string ContactId)
    //{
    //    string[] result = new string[2];
    //    result[0] = "false";
    //    result[1] = "false";

    //    DataTable dt = new DataAccessClass().return_DataTable("SELECT distinct top 1 Set_AddressMaster.MobileNo1, Set_AddressMaster.EmailId1 FROM dbo.Set_AddressMaster INNER JOIN dbo.Set_AddressChild ON dbo.Set_AddressMaster.Trans_Id = dbo.Set_AddressChild.Ref_Id WHERE Add_Type = 'contact' AND Add_Ref_Id = '" + ContactId + "'");
    //    if (dt.Rows.Count != 0)
    //    {
    //        string Temp = dt.Rows[0]["MobileNo1"].ToString();
    //        Temp = Temp != "" ? dt.Rows[0]["MobileNo1"].ToString() : "No Mobile No";
    //        result[0] = Resources.Attendance.Mobile_No_1 + " : " + Temp.Trim();
    //        Temp = dt.Rows[0]["EmailId1"].ToString();
    //        Temp = Temp != "" ? dt.Rows[0]["EmailId1"].ToString() : "No Email Id";
    //        result[1] = Resources.Attendance.Email_Id + " : " + Temp.Trim();
    //    }
    //    dt = null;
    //    return result;
    //}
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtShippingAcc_TextChanged(string trans_id, string accountName)
    {
        string count = "0";
        count = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).get_SingleValue("SELECT COUNT(trans_id) FROM Ac_ChartOfAccount WHERE Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' and trans_id='" + trans_id + "' and AccountName='" + accountName + "'");
        count = count == "@NOTFOUND@" ? "0" : count;
        if (count != "0")
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    protected void lnkPage_Click(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdngvPurchaseOrderCurrentPageIndex.Value = pageIndex.ToString();
        fillGrid(pageIndex);
    }

    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, gvPurchaseOrder, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(gvPurchaseOrder, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, EventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
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
                dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), Session["EmpId"].ToString());
                //dtEmp = new DataView(dtEmp, "Emp_Id='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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

        DataTable dtEmp = objDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id in (" + strLocationId + ") and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");

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
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUser();
        fillGrid(1);
        if (ddlLocation.SelectedItem.ToString() != "All")
        {
            hdnLocationID.Value = ddlLocation.SelectedValue;
        }
        else
        {
            hdnLocationID.Value = "";
        }
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
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
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
    protected void ddlUser_Click(object sender, EventArgs e)
    {
        fillGrid(1);
    }

    protected void btnGetDataFromSalesOrder_Click()
    {
        DataTable dt = ObjSalesOrderDetail.getSODetailDataForPurchaseOrder(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
        gvPendingSalesOrder.DataSource = dt;
        gvPendingSalesOrder.DataBind();
    }

    protected void chkAddSOToPO_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lblOrderNo = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblOrderNo");
        HiddenField trans_id = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvOrderId");
        Label lblProductCode = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblProductCode");
        HiddenField gvhdnProductId = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvhdnProductId");
        Label lblUnit = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblUnit");
        HiddenField gvhdnUnitId = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvhdnUnitId");
        HiddenField gvHdnUnitCost = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvHdnUnitCost");
        Label lblProductName = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblProductName");
        Label lblQuantity = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblQuantity");


        ddlTransType.Enabled = false;
        string ReqId = string.Empty;
        string ProductId = gvhdnProductId.Value;
        string UnitId = gvhdnUnitId.Value;
        string txtProductName = lblProductName.Text;
        string txtUnitCost = gvHdnUnitCost.Value;
        string txtOrderQty = lblQuantity.Text;
        string txtfreeQty = "0";


        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Enter Supplier Name");
            txtSupplierName.Focus();
            cb.Checked = false;
            return;
        }

        if (HdnEdit.Value == "")
        {
            ReqId = ObjPurchaseOrder.getAutoId();
        }
        else
        {
            ReqId = HdnEdit.Value.ToString();
        }


        DataTable dtTempDt = new DataTable();
        if (ViewState["dtProduct"] == null)
        {
            dtTempDt.Columns.Add("Trans_Id");
            dtTempDt.Columns.Add("PoNo");
            dtTempDt.Columns.Add("Product_Id");
            dtTempDt.Columns.Add("ProductDescription");
            dtTempDt.Columns.Add("UnitId");
            dtTempDt.Columns.Add("UnitCost");
            dtTempDt.Columns.Add("OrderQty");
            dtTempDt.Columns.Add("freeQty");
            dtTempDt.Columns.Add("Serial_No");
            dtTempDt.Columns.Add("DiscountP");
            dtTempDt.Columns.Add("DiscountV");
            dtTempDt.Columns.Add("TaxP");
            dtTempDt.Columns.Add("TaxV");
            dtTempDt.Columns.Add("SOId");
            dtTempDt.Columns.Add("SONO");
            dtTempDt.Rows.Add();
            dtTempDt.Rows[0]["Trans_Id"] = dtTempDt.Rows.Count;
            dtTempDt.Rows[0]["Serial_No"] = dtTempDt.Rows.Count;
            dtTempDt.Rows[0]["PoNo"] = ReqId.ToString();
            dtTempDt.Rows[0]["Product_Id"] = ProductId.ToString();
            dtTempDt.Rows[0]["ProductDescription"] = "";
            dtTempDt.Rows[0]["UnitId"] = UnitId;
            dtTempDt.Rows[0]["UnitCost"] = txtUnitCost;
            dtTempDt.Rows[0]["OrderQty"] = txtOrderQty;
            dtTempDt.Rows[0]["freeQty"] = txtfreeQty;
            dtTempDt.Rows[0]["DiscountP"] = "0";
            dtTempDt.Rows[0]["DiscountV"] = "0.00";
            dtTempDt.Rows[0]["SOId"] = trans_id.Value;
            dtTempDt.Rows[0]["SONO"] = lblOrderNo.Text;

            Add_Tax_In_Session(txtUnitCost, ProductId, dtTempDt.Rows.Count.ToString());
            dtTempDt.Rows[0]["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), dtTempDt.Rows.Count.ToString());
            dtTempDt.Rows[0]["TaxV"] = Get_Tax_Amount(txtUnitCost, ProductId.ToString(), dtTempDt.Rows.Count.ToString());
            ViewState["dtProduct"] = dtTempDt;
        }
        else
        {
            int i = 0;
            dtTempDt = (DataTable)ViewState["dtProduct"];
            if (hidProduct.Value != "")
            {
                for (int j = 0; j < dtTempDt.Rows.Count; j++)
                {
                    if (dtTempDt.Rows[j]["Trans_Id"].ToString() == hidProduct.Value.ToString())
                    {
                        i = j;
                    }
                }
            }
            else
            {
                dtTempDt.Rows.Add();
                i = Convert.ToInt32(dtTempDt.Rows.Count - 1);
            }
            if (hidProduct.Value != "")
            {
                dtTempDt.Rows[i]["Trans_Id"] = hidProduct.Value.Trim();
            }
            else
            {
                try
                {
                    dtTempDt.Rows[i]["Trans_Id"] = (Convert.ToInt32(dtTempDt.Rows[i - 1]["Trans_Id"].ToString()) + 1).ToString();
                }
                catch
                {
                    dtTempDt.Rows[i]["Trans_Id"] = "1";
                }
            }
            try
            {
                if (hidProduct.Value == "")
                {
                    dtTempDt.Rows[i]["Serial_No"] = (Convert.ToInt32(dtTempDt.Rows[i - 1]["Trans_Id"].ToString()) + 1).ToString();
                }
            }
            catch
            {
                dtTempDt.Rows[i]["Serial_No"] = "1";
            }
            dtTempDt.Rows[i]["PoNo"] = ReqId.ToString();
            dtTempDt.Rows[i]["Product_Id"] = ProductId.ToString();
            dtTempDt.Rows[i]["ProductDescription"] = txtPDescription.Text.ToString();
            dtTempDt.Rows[i]["UnitId"] = UnitId.ToString();
            dtTempDt.Rows[i]["UnitCost"] = txtUnitCost;
            dtTempDt.Rows[i]["OrderQty"] = txtOrderQty;
            dtTempDt.Rows[i]["freeQty"] = txtfreeQty;
            dtTempDt.Rows[i]["DiscountP"] = "0";
            dtTempDt.Rows[i]["DiscountV"] = "0.00";
            dtTempDt.Rows[i]["SOId"] = trans_id.Value;
            dtTempDt.Rows[i]["SONO"] = lblOrderNo.Text;
            Add_Tax_In_Session(txtUnitCost, ProductId, dtTempDt.Rows[i]["Serial_No"].ToString());
            dtTempDt.Rows[i]["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), dtTempDt.Rows[i]["Serial_No"].ToString());
            dtTempDt.Rows[i]["TaxV"] = Get_Tax_Amount(txtUnitCost, ProductId.ToString(), dtTempDt.Rows[i]["Serial_No"].ToString());
            ViewState["dtProduct"] = dtTempDt;
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dtTempDt, "", "");
        dtTempDt = null;
        ViewState["DefaultCurrency"] = ddlCurrency.SelectedValue.ToString();
        if (ddlCurrency.SelectedIndex != 0)
        {
            gvProduct.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Unit Cost", Session["DBConnection"].ToString());
        }
        else
        {
            gvProduct.HeaderRow.Cells[8].Text = "Unit Cost";
        }

        GridCalculation();
    }

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

                            CheckProductCodeAndInsertIfNotExist("", dr["unit"].ToString(), dr["Model_Name"].ToString().Trim(), dr["ColorName"].ToString().Trim(), dr["SizeName"].ToString().Trim(), dr["SalesPrice1"].ToString(), dr["SalesPrice2"].ToString(), dr["SalesPrice3"].ToString());
                            //if (dr["productCode"].ToString() != "")
                            //{
                            //    txtProductcode.Text = dr["productCode"].ToString();
                            //}

                            txtProductCode_TextChanged(null, null);
                            ddlUnit.SelectedItem.Text = dr["unit"].ToString();
                            txtOrderQty.Text = dr["orderQty"].ToString();
                            txtfreeQty.Text = dr["freeQty"].ToString();
                            txtUnitCost.Text = dr["Unit_Price"].ToString();
                            btnProductSave_Click(null, null);


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

                txtProductcode.Text = ProductCode;

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

                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "7", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "8", b.ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "6", b.ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                    txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                }
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

    protected void lnkSpv_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;
        if (myButton.Text == "0")
        {
            return;
        }
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string Trans_Id = arguments[0].Trim();
        string LocationId = arguments[1].Trim();

        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(Trans_Id);
        if (dtVoucherHeader.Rows.Count > 0)
        {
            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + Trans_Id + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherHeader.Rows.Count > 0)
            {
                strVoucherType = dtVoucherHeader.Rows[0]["Voucher_Type"].ToString();
            }
        }

        string strCmd = string.Format("window.open('../VoucherEntries/VoucherDetail.aspx?Id=" + Trans_Id + "&LocId=" + LocationId + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
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

    protected string GetShortProductName(string strProductId)
    {
        string strProductName = string.Empty;
        strProductName = ProductName(strProductId);
        if (strProductName.Length > 16)
        {
            strProductName = strProductName.Substring(0, 15) + "...";
        }
        return strProductName;
    }
}