using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Services;
using System.Configuration;
using System.Data.OleDb;

public partial class Sales_SalesInvoice2 : BasePage
{
    #region defined Class Object
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Common cmn = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    Inv_StockDetail objStockDetail = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    DataAccessClass objDa = null;
    Inv_ParameterMaster objInvParam = null;
    Ac_ChartOfAccount objCOA = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Set_BankMaster ObjBankMaster = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Set_CustomerMaster ObjCustmer = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_ProductCategory_Tax objProTax = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    TaxMaster objTaxMaster = null;
    MerchantMaster objMerchantMaster = null;
    EmployeeMaster ObjEmployeeMaster = null;
    Inv_SalesDeliveryVoucher_Header objdelVoucherHeader = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    SM_JobCard_Header objJobCardheader = null;
    SM_JobCards_SpareLabourDetail objJobCardLabourdetail = null;
    NotificationMaster Obj_Notifiacation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;
    #region defined Class Object
  
    Set_Approval_Employee objEmpApproval = null;
 

    Set_AddressMaster objAddMaster = null;
  
    Inv_ProductLedger ObjProductLedger = null;
   
    Ems_ContactMaster ObjContactMaster = null;

    Contact_PriceList objCustomerPriceList = null;
  
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ShipExpHeader ObjShipExpHeader = null;

    LocationMaster objLocation = null;
   
    Inv_StockDetail objStock = null;
    Set_AddressChild objAddressChild = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();
 
    Country_Currency objCountryCurrency = null;
  
    //Ac_Ageing_Detail objAgeingDetail = new Ac_Ageing_Detail();
   
    Inv_SalesDeliveryVoucher_Detail objdelVoucherDetail = null;
 
    ProductTaxMaster objProductTaxMaster = null;

    PageControlsSetting objPageCtlSettting = null;
  

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strCurrencyId = string.Empty;
    string strDepartmentId = string.Empty;
    public static int strSiCustomerId = 0;
    public static int strSiCurrencyId = 0;
    public const int grdDefaultColCount = 7;
    private const string strPageName = "SalesInvoice";

    #endregion
    public static DataTable Dt_Final_Save_Tax;

    public static int Decimal_Count_For_Tax;

    #endregion
 
    protected void Page_Load(object sender, EventArgs e)
    {
       
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        ObjBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        ObjCustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objProTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        objMerchantMaster = new MerchantMaster(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objdelVoucherHeader = new Inv_SalesDeliveryVoucher_Header(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        objJobCardheader = new SM_JobCard_Header(Session["DBConnection"].ToString());
        objJobCardLabourdetail = new SM_JobCards_SpareLabourDetail(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (Session["Expenses_Tax_Sales_Invoice"] != null)
        {
            Dt_Final_Save_Tax = Session["Expenses_Tax_Sales_Invoice"] as DataTable;
        }
        else
        {
            Dt_Final_Save_Tax = null;
        }
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/SalesInvoice.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        if (clsPagePermission.bHavePermission == false)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        AllPageCode(clsPagePermission);


        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        strDepartmentId = Session["DepartmentId"].ToString();
        if (Session["Expenses_Sales_Invoice_Local"] != null)
        {
            DataTable Dt_temp = Session["Expenses_Sales_Invoice_Local"] as DataTable;
            if (Dt_temp.Rows.Count > 0)
            {
                Expenses_Read_Only();
            }
        }
        if (!IsPostBack)
        {
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            //FillGrid();
            Session["ContactID"] = null;
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "92", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["Expenses_Tax_Sales_Invoice"] = null;
            bool IsTax = IsApplyTax();
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
            hdnIsApproved.Value = objDa.get_SingleValue("select ParameterValue from Inv_ParameterMaster where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and ParameterName='SalesInvoiceApproval' and IsActive='True'");
            string Decimal_Count = string.Empty;
            Decimal_Count = cmn.Get_Decimal_Count_By_Location(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            //TO GET ACCURACY FOR TAX WE ADDED THIS HARD CODE - BCZ WHEN PRICE IS TOO SMALL WE ARE UNABLE TO GET ACCURACY WITH TWO DIGIT
            Decimal_Count_For_Tax = 4;
            bool Tax_Apply = IsApplyTax();
            Session["Is_Tax_Apply"] = Tax_Apply.ToString();
            Session["Temp_Product_Tax_SINV"] = null;
            ViewState["PostBackID"] = Guid.NewGuid().ToString();
            ViewState["dtSerial"] = null;
            FillCurrency();
            FillTransactionType();
            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
                if (strCurrencyId != "0" && strCurrencyId != "")
                {
                    ddlCurrency.SelectedValue = strCurrencyId;
                    ddlExpCurrency.SelectedValue = strCurrencyId;
                }
            }
            catch
            {
            }
            txtExchangeRate.Text = "1";
            FillMerchant();
            fillBank();
            FillPaymentMode();
            fillExpenses();
            hdnDocNo.Value = txtSInvNo.Text;
            Calender.Format = Session["DateFormat"].ToString();
            txtChequedate_CalenderExtender.Format = Session["DateFormat"].ToString();
            txtSInvDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            PnlProductSearching.Visible = true;
            ViewState["ExchangeRate"] = "1";
            btnAddCustomer.Visible = IsAddCustomerPermission();
            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            TaxandDiscountParameter();
            if (Request.QueryString["Id"] != null)
            {
                try
                {
                    StrLocationId = Request.QueryString["LocId"].ToString();
                }
                catch
                {
                    StrLocationId = Session["LocId"].ToString();
                }
            }
            Session["DtAdvancePayment"] = null;
            //for get sales perso n name according login employee
            if (Session["EmpId"].ToString() != "0" && Request.QueryString["Id"] == null)
            {
                DataTable Dt_Temp_Emp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
                txtSalesPerson.Text = Dt_Temp_Emp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Temp_Emp.Rows[0]["Emp_Code"].ToString();
            }
            //code end
            // fillTabPaymentMode();
            txtCondition1.Content = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();
            Session["JobCardId"] = null;
            Expenses_Tax_Modal.Expenses_Tax_And_Amount_Clear();
            Btn_Exp_Reset_Click(null, null);
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
        }
        Lbl_Expenses_Tax_Amount_ET.Text = Expenses_Tax_Modal.Expenses_Amount_Value();
        Lbl_Expenses_Tax_ET.Text = Expenses_Tax_Modal.Expenses_Tax_Value();
        //txtExpExchangeRate.Enabled = false;
        if (ddlCurrency.SelectedIndex != 0)
        {
            strCurrencyId = ddlCurrency.SelectedValue;
        }
        if (Session["Is_Tax_Apply"] != null && Session["Is_Tax_Apply"].ToString() == "False")
            Trans_Div.Visible = false;
    }
    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            bindGrid(Convert.ToInt32(e.CommandArgument));
        }
    }
       [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetGeoLocation(string ShippingAddress)
    {
        string[] result = new string[3];
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByAddressName(ShippingAddress.Split('/')[0].ToString());
        if (dtAM.Rows.Count > 0)
        {
            result[0] = dtAM.Rows[0]["Longitude"].ToString();
            result[1] = dtAM.Rows[0]["Latitude"].ToString();

            return result;
        }
        else
        {
            return result;
        }
    }
    public void bindGrid(int currentPage)
    {
        int _TotalRowCount = 0;

        string PostType = string.Empty;
        string myval = string.Empty;
        bool IsAll = true;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            myval = "0";
            IsAll = false;
        }

        if (IsAll == false)
            PostType = " Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();

        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldName.SelectedItem.Value == "Supplier_Id")
            {
                string retval = (txtCustValue.Text.Split('/'))[txtCustValue.Text.Split('/').Length - 1];

                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + retval + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + retval + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + retval + "%'";
                    SearchType = "Like";
                }
            }
            else
            {
                if (ddlFieldName.SelectedItem.Value == "Invoice_Date")
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

                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                    SearchType = "Like";
                }
            }

            SearchField = ddlFieldName.SelectedValue.ToString();
            SearchValue = txtValue.Text.Trim();

            if (SearchField == "Trans_Id")
                SearchField = "Inv_SalesInvoiceHeader.Trans_Id";

            string PageSize = Session["GridSize"].ToString().ToString();
          

    //        //DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, true.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);
           // DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, false.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);

            //_TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());

            //lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

            //GvSalesInvoice.DataSource = dt;
            //GvSalesInvoice.DataBind();

            //object sumObject;
            //sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


            //lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            //generatePager(_TotalRowCount, int.Parse(PageSize), currentPage);

        }
    }


    protected void IbtnCancel_Command(object sender, CommandEventArgs e)
    {
        DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
        if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
        {
            DisplayMessage("Sales Invoice has posted,can not be Cancel");
            //AllPageCode();
            return;
        }

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                if (st == "Approved")
                {
                    DisplayMessage("Sales Invoice has Approved, cannot be Cancel");
                   // AllPageCode();
                    return;

                }

            }

        }//End Approval Code

        editid.Value = e.CommandArgument.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
    }

    public void fillExpenses()
    {
        DataTable dt = ObjShipExp.GetShipExpMaster(StrCompId.ToString());
        objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");
    }
    protected void lnkAddNewContact_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        DataTable dt = ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        dt = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetLocationList()
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objDA.return_DataTable("Select   Location_Name +'/'+ Location_Code+'/'+  Cast(Location_Id as varchar(10)) as 'LocationName'  From Set_LocationMaster  Where IsActive ='1'");
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i][0].ToString();
        }
        return txt;
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
    private bool validatePostback()
    {
        // verify duplicate postback
        string postbackid = ViewState["PostBackID"] as string;
        bool isValid = Cache[postbackid] == null;
        if (isValid)
            Cache.Insert(postbackid, true, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));
        return isValid;
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void FillMerchant()
    {
        ddlMerchant.Items.Clear();
        ListItem Li = new ListItem();
        Li.Text = "--Select--";
        Li.Value = "0";
        ddlMerchant.DataSource = objMerchantMaster.GetMerchantMasterTrueAll();
        ddlMerchant.DataTextField = "Merchant_Name";
        ddlMerchant.DataValueField = "Trans_Id";
        ddlMerchant.DataBind();
        ddlMerchant.Items.Insert(0, Li);
    }
    void TaxandDiscountParameter()
    {
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
            {
                lblTaxP.Visible = false;
                txtTaxP.Visible = false;
                Label2.Visible = false;
                txtTaxV.Visible = false;
                Trans_Div.Visible = false;
            }
            else
            {
                lblTaxP.Visible = true;
                txtTaxP.Visible = true;
                Label2.Visible = true;
                txtTaxV.Visible = true;
                Trans_Div.Visible = true;
                hdnIsTaxEnabled.Value = "true";
            }
        }
        Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
            {
                lblDiscountP.Visible = false;
                txtDiscountP.Visible = false;
                Label3.Visible = false;
                txtDiscountV.Visible = false;
                lblPriceafterdiscountheader.Visible = false;
                txtPriceafterdiscountheader.Visible = false;
            }
            else
            {
                lblDiscountP.Visible = true;
                txtDiscountP.Visible = true;
                Label3.Visible = true;
                txtDiscountV.Visible = true;
                lblPriceafterdiscountheader.Visible = true;
                txtPriceafterdiscountheader.Visible = true;
                hdnIsDiscountEnable.Value = "true";
            }
        }
        Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
               AutoCompleteExtenderProductcode.Enabled = false;
               AutoCompleteExtenderProductName.Enabled = false;
                txtProductSerachValue.AutoPostBack = true;
            }
        }
        Dt = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Sales Invoice");
        if (Dt.Rows.Count > 0)
        {
            Session["AccountId"] = Dt.Rows[0]["Param_Value"].ToString();
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Exp_Reset.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanCancel.Value = "true";
        txtSInvDate.Enabled = clsPagePermission.bModifyDate;
        //btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    #region System defined Function
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
    public string getDeliveryDocumentNumber(ref SqlTransaction trns)
    {
        string strVoucherNo = string.Empty;
        DataTable dtCount = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        if (dtCount.Rows.Count == 0)
        {
            //objSInvHeader.Updatecode(b.ToString(), txtSInvNo.Text + "1");
            strVoucherNo = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "327", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + "1";
        }
        else
        {
            //objSInvHeader.Updatecode(b.ToString(), txtSInvNo.Text + dtCount.Rows.Count);
            string count = (dtCount.Rows.Count + 1).ToString();
            strVoucherNo = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "327", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + count;
        }
        return strVoucherNo;
    }
    #endregion
    #region AutocompleteRegion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.GetAddress_PreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["filterText"].ToString();
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCreditNote(string prefixText, int count, string contextKey)
    {
        try
        {
            DataTable _dt = getUnAdjustedCreditNote();
            string filtertext = "Voucher_No like '%" + prefixText + "%'";
            _dt = new DataView(_dt, filtertext, "", DataViewRowState.CurrentRows).ToTable();
            string[] filterlist = new string[_dt.Rows.Count];
            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    filterlist[i] = _dt.Rows[i]["voucher_no"].ToString();
                }
            }
            return filterlist;
        }
        catch
        {
            return null;
        }
    }
    #endregion
    #region User defined Function
    public static DataTable getUnAdjustedCreditNote()
    {
        DataTable _dt = new DataTable();
        if (HttpContext.Current.Session["SI_UnAdjustedCreditNote"] != null)
        {
            _dt = (DataTable)HttpContext.Current.Session["SI_UnAdjustedCreditNote"];
        }
        else
        {
            if (strSiCustomerId > 0 && strSiCurrencyId > 0)
            {
                Ac_AccountMaster objAccountMaster = new Ac_AccountMaster(HttpContext.Current.Session["DBConnection"].ToString());
                string strOtherAccountId = "0";
                strOtherAccountId = objAccountMaster.GetCustomerAccountByCurrency(strSiCustomerId.ToString(), strSiCurrencyId.ToString()).ToString();
                if (strOtherAccountId != "0")
                {
                    Ac_Ageing_Detail objAcAgeingDetail = new Ac_Ageing_Detail(HttpContext.Current.Session["DBConnection"].ToString());
                    string CustomerPrimaryAcNo = Ac_ParameterMaster.GetCustomerAccountNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
                    _dt = objAcAgeingDetail.GetUnAdjustedCustomerCreditVoucher(HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), CustomerPrimaryAcNo, strOtherAccountId, "CCN");
                    HttpContext.Current.Session["SI_UnAdjustedCreditNote"] = _dt;
                }
            }
        }
        return _dt;
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
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
    #endregion
    #region Add Request Section
    private void FillSalesOrderNo()
    {
        //Check Query Like Sales Quotation No.
        DataTable DtSalesorderRequest = new DataTable();
        DataTable dtSalesOrderHeader = objSOrderHeader.GetSOHeaderAllTrue(StrCompId, StrBrandId, StrLocationId);
        try
        {
            dtSalesOrderHeader = new DataView(dtSalesOrderHeader, "CustomerId='" + txtCustomer.Text.Trim().Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        { }
        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesOrderApproval");
        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                dtSalesOrderHeader = new DataView(dtSalesOrderHeader, "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        //code end Approval
        for (int i = 0; i < dtSalesOrderHeader.Rows.Count; i++)
        {
            DataTable dtSalesorderdetail = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, dtSalesOrderHeader.Rows[i]["Trans_Id"].ToString());
            for (int j = 0; j < dtSalesorderdetail.Rows.Count; j++)
            {
                double InvoiceQty = 0;
                double Orderqty = 0;
                DataTable dtSiDetail = objSInvDetail.GetSInvDetailAllTrue(StrCompId, StrBrandId, StrLocationId);
                try
                {
                    dtSiDetail = new DataView(dtSiDetail, "Product_Id=" + dtSalesorderdetail.Rows[j]["Product_Id"].ToString() + " and SIFromTransType='S' and SIFromTransNo=" + dtSalesOrderHeader.Rows[i]["Trans_Id"].ToString() + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                for (int k = 0; k < dtSiDetail.Rows.Count; k++)
                {
                    InvoiceQty = InvoiceQty + Convert.ToDouble(dtSiDetail.Rows[k]["Quantity"].ToString());
                }
                Orderqty = Convert.ToDouble(dtSalesorderdetail.Rows[j]["Quantity"].ToString());
                if (InvoiceQty != Orderqty)
                {
                    DataTable DtRequest = dtSalesOrderHeader.Copy();
                    try
                    {
                        DtRequest = new DataView(DtRequest, "Trans_Id=" + dtSalesOrderHeader.Rows[i]["Trans_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    { }
                    DtSalesorderRequest.Merge(DtRequest);
                    break;
                }
            }
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

            ddlTabPaymentMode.DataSource = dsPaymentMode;
            ddlTabPaymentMode.DataTextField = "Pay_Mod_Name";
            ddlTabPaymentMode.DataValueField = "Pay_Mode_Id";
            ddlTabPaymentMode.DataBind();
            //objPageCmn.FillData((object)ddlPaymentMode, dsPaymentMode, "Pay_Mod_Name", "Pay_Mode_Id");
            ListItem li = new ListItem("Multi", "Multi");
            ddlTabPaymentMode.Items.Add(li);

            ddlTabPaymentMode.SelectedIndex = 0;
        }
        else
        {
            ddlTabPaymentMode.Items.Insert(0, "--Select--");
            ddlTabPaymentMode.SelectedIndex = 0;
        }

        fillTabPaymentMode(ddlTabPaymentMode.SelectedItem.Text);
    }
    private void FillCurrency()
    {
        using (DataTable dsCurrency = objCurrency.GetCurrencyListForDDL())
        {
            if (dsCurrency.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                objPageCmn.FillData((object)ddlExpCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            }
            else
            {
                ddlCurrency.Items.Add("--Select--");
                ddlCurrency.SelectedValue = "--Select--";
                ddlExpCurrency.Items.Add("--Select--");
                ddlExpCurrency.SelectedValue = "--Select--";
            }
        }
    }
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    { return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), HttpContext.Current.Session["DBConnection"].ToString()); }
    protected string GetUnitCode(string strUnitId)
    {
        string strUnitCode = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUCode = UM.GetUnitMasterById(StrCompId, strUnitId);
            if (dtUCode.Rows.Count > 0)
            {
                strUnitCode = dtUCode.Rows[0]["Unit_Name"].ToString();
            }
        }
        else { strUnitCode = ""; }
        return strUnitCode;
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
                if (dtEmp.Rows.Count == 0)
                { retval = ""; }
            }
            else
            { retval = ""; }
        }
        else
        { retval = ""; }
        return retval;
    }
    protected void GvSalesOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Order;
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.S_No_;
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Product_Detail;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
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
            cell.HorizontalAlign = HorizontalAlign.Center;
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
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = Resources.Attendance.Total;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Stock";
            row.Controls.Add(cell);
            gvProduct.Controls[0].Controls.Add(row);
        }
    }
    #endregion
    #region Grid Calculations

    protected void refreshGvProduct(GridViewRow gvRow)
    {
        //GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
        TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
        TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
        Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
        TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
        TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
        TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
        Label lblgvSoldQuantity = (Label)gvRow.FindControl("lblgvSoldQuantity");
        Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
        Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");
        Label lblgvSerialNo = (Label)gvRow.FindControl("lblgvSerialNo");
        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }
        if (txtgvSalesQuantity.Text == "")
        {
            txtgvSalesQuantity.Text = "0";
        }
        double PriceValue = double.Parse(lblgvUnitPrice.Text);
        double QtyValue = double.Parse(txtgvSalesQuantity.Text);
        double AmountValue = PriceValue * QtyValue;
        Add_Tax_In_Session(PriceValue.ToString(), Convert_Into_DF((PriceValue * double.Parse(txtgvDiscountP.Text) / 100).ToString()), hdngvProductId.Value, lblgvSerialNo.Text);
        double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), AmountValue.ToString()).Split('/')[0].ToString(), out AmountValue);
        lblgvQuantityPrice.Text = AmountValue.ToString();
        double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
        bool IsValidDiscount = IsApplyDiscount();
        if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();
        if (!IsValidDiscount)
            AmntAfterDiscnt = AmountValue;
        double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblgvSerialNo.Text);
        TotalTax = TotalTax / QtyValue;
        if (Double.IsNaN(TotalTax) || Double.IsInfinity(TotalTax))
        {
            TotalTax = 0;
        }
        double.TryParse(Convert_Into_DF(TotalTax.ToString()).ToString(), out TotalTax);
        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), txtgvDiscountP.Text, "", true, strCurrencyId, false, HttpContext.Current.Session["DBConnection"].ToString());
        txtgvDiscountV.Text = strtotal[2].ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxP.Text = Get_Tax_Percentage(hdngvProductId.Value, lblgvSerialNo.Text).ToString();
        txtgvTaxV.Text = Convert_Into_DF(TotalTax.ToString());
        txtgvTotal.Text = (AmntAfterDiscnt + (TotalTax * QtyValue)).ToString();
        txtgvTotal.Text = GetCurrency(Session["LocCurrencyId"].ToString(), txtgvTotal.Text).Split('/')[0].ToString();
        GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
    }
    #endregion
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
    public bool IsApplyTax()
    {
        bool IsValid = false;
        string isTabable = objDa.get_SingleValue(" Select ParameterValue From Inv_ParameterMaster where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and ParameterName='IsTaxSales' and IsActive='True'");
        if (isTabable != "")
        {
            if (isTabable.ToLower() == "false")
                IsValid = false;
            else
                IsValid = true;
        }
        return IsValid;
    }
    public bool IsApplyDiscount()
    {
        bool IsValid = false;
        string isDiscountable = objDa.get_SingleValue(" Select ParameterValue From Inv_ParameterMaster where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and ParameterName='IsDiscountSales' and IsActive='True'");
        if (isDiscountable != "")
        {
            if (isDiscountable.ToLower() == "false")
                IsValid = false;
            else
                IsValid = true;
        }
        return IsValid;
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
        string strProductDescription = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(StrCompId, StrBrandId, strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductDescription = dtPName.Rows[0]["Description"].ToString();
            }
            else
            {
                strProductDescription = "";
            }
        }
        else
        {
            strProductDescription = "";
        }
        return strProductDescription;
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
    protected void gvProduct_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Edit;
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Delete;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.S_No_;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Product_Detail;
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
            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), StrBrandId, StrLocationId, "IsTaxSales");
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
            cell.Text = Resources.Attendance.Discount;
            Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), StrBrandId, StrLocationId, "IsDiscountSales");
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
            dt = null;
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
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        pnlProduct1.Visible = true;
        pnlProduct2.Visible = true;
        SerialPanel.Visible = false;
        Session["DtSearchProduct"] = null;
    }
    public void addSerialfnc(string strProductId, string strSerialNumber, string strOrderId)
    {
        DataTable dt = new DataTable();
        if (ViewState["dtFinal"] == null)
        {
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("SOrderNo");
            dt.Columns.Add("BarcodeNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("LotNo");
            dt.Columns.Add("ExpiryDate");
            dt.Columns.Add("ManufacturerDate");
        }
        else
        {
            dt = (DataTable)ViewState["dtFinal"];
        }
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dr[0] = strProductId;
        dr[1] = strSerialNumber;
        dr[2] = strOrderId;
        //for batch number
        dr[4] = "";
        dr[5] = "0";
        //for expiry date
        dr[6] = DateTime.Now.ToString();
        //for Manufacturer date
        dr[7] = DateTime.Now.ToString();
        ViewState["dtFinal"] = dt;
    }

    public void FillSerialForScanningsolution(string strOrderId)
    {
        bool Isserial = false;
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductSerachValue.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    //heer we checking that scaned text is serial number or not
                    //code start
                    if (dt.Rows[0]["Type"].ToString() == "2")
                    {
                        Isserial = true;
                    }
                    DataTable dtso = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strOrderId);
                    if (dtso.Rows.Count > 0)
                    {
                        if (dtso.Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                        {
                            if (Isserial)
                            {
                                addSerialfnc(dt.Rows[0]["ProductId"].ToString(), txtProductSerachValue.Text.Trim(), strOrderId);
                            }
                        }
                    }
                }
            }
        }
    }

    public DataTable fillSOSearhgrid()
    {
        DataTable dtSalesOrder = null;
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesOrderApproval");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                dtSalesOrder = new DataView(objSOrderHeader.GetProductFromSalesOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtCustomer.Text.Trim().Split('/')[1].ToString(), Session["FinanceYearId"].ToString()), "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtSalesOrder = objSOrderHeader.GetProductFromSalesOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtCustomer.Text.Trim().Split('/')[1].ToString(), Session["FinanceYearId"].ToString());
            }
        }
        return dtSalesOrder;
    }
    public void RdoSOandWithSO()
    {
        PnlProductSearching.Visible = true;
        txtAmount.Text = "0";
        txtTotalQuantity.Text = "0";
        txtTaxP.Text = "0";
        txtTaxV.Text = Convert_Into_DF("0");
        txtDiscountP.Text = "0";
        txtDiscountV.Text = "0";
        txtNetAmount.Text = "0";
        txtGrandTotal.Text = "0";
        if (!RdoWithOutSo.Checked)
        {
            txtSearchProductName.Visible = false;
            txtProductId.Visible = false;
            txtProductSerachValue.Visible = true;
            txtSearchProductName.Visible = false;
            if (ddlProductSerach.Items.FindByText("Sales Order No") == null)
            {
                ddlProductSerach.Items.Insert(2, new ListItem("Sales Order No", "SalesOrderNo"));
            }
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
            txtProductSerachValue.Visible = false;
            txtProductId.Visible = false;
            txtSearchProductName.Visible = true;
            ddlProductSerach.SelectedIndex = 1;
        }
    }
    #endregion

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


    public void fillTabPaymentMode(string PaymentType)
    {
        DataTable dt = ObjPaymentMaster.GetPaymentModeMaster(StrCompId.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //if (PaymentType != "Multi")
        //{
        //    dt = new DataView(dt, "Field1='" + PaymentType.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //}

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlTabPaymentMode, dt, "Pay_Mod_Name", "Pay_Mode_Id");

    }

    public void fillBank()
    {
        DataTable dt = objDa.return_DataTable("Select Bank_Id,Bank_Name	From Set_BankMaster Where IsActive='True' Order by CreatedDate desc");
        objPageCmn.FillData((object)ddlPayBank, dt, "Bank_Name", "Bank_Id");
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objcontact.GetAllContactAsPerFilterText(prefixText);
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

    #region Adavancesearch

    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            DataTable dt = (DataTable)Session["DtSearchProduct"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtTempDt = dt;
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
                    Add_Tax_In_Session(dt_Row["UnitPrice"].ToString(), dt_Row["DiscountV"].ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    dt_Row["DiscountP"] = "0";
                    dt_Row["DiscountV"] = "0.00";
                    dt_Row["TaxP"] = Get_Tax_Percentage(dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                    dt_Row["TaxV"] = Get_Tax_Amount(dt_Row["UnitPrice"].ToString(), dt_Row["Product_Id"].ToString(), dt_Row["Serial_No"].ToString());
                }
                dt = dtTempDt;
                ddlTransType.Enabled = false;
                ViewState["Dtproduct"] = dt;
            }
            Session["DtSearchProduct"] = null;
        }
        else
        {
            DisplayMessage("Product Not Found");
            return;
        }
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        //if (rbtnFormView.Checked == true)
        //{
        //    btnAddNewProduct.Visible = true;
        //    btnAddProductScreen.Visible = false;
        //    btnAddtoList.Visible = false;
        //}
        //if (rbtnAdvancesearchView.Checked == true)
        //{
        //    btnAddNewProduct.Visible = false;
        //    btnAddProductScreen.Visible = true;
        //    btnAddtoList.Visible = true;
        //}
    }
    #endregion
    #region AddCustomer
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
        ViewState["DtTax"] = dt;
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
    #endregion
    #region addheadertax

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
    #region stockable with serial number

    protected void Btnloadfile_Click(object sender, EventArgs e)
    {
        int counter = 0;
        txtSerialNo.Text = "";
        try
        {
            string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
            string[] csvRows = System.IO.File.ReadAllLines(Path);
            string[] fields = null;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(',');
                if (fields[0].ToString() != "")
                {
                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = fields[0].ToString();
                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + fields[0].ToString();
                    }
                    counter++;
                }
            }
            if (Directory.Exists(Path))
            {
                try
                {
                    Directory.Delete(Path);
                }
                catch
                {
                }
            }
            txtCount.Text = counter.ToString();
        }
        catch
        {
            txtSerialNo.Text = "";
            DisplayMessage("File Not Found ,Try Again");
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }
    }
    protected void IbtnDeleteserialNumber_Command(object sender, CommandEventArgs e)
    {
        if (ViewState["dtSerial"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtSerial"];
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "SerialNo<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            ViewState["dtSerial"] = dt;
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void btnsearchserial_Click(object sender, EventArgs e)
    {
        if (txtserachserialnumber.Text != "")
        {
            DataTable dt = new DataTable();
            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];
                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "SerialNo='" + txtserachserialnumber.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Serial Number Not Found");
                txtserachserialnumber.Text = "";
                txtserachserialnumber.Focus();
                return;
            }
            btnRefreshserial.Focus();
        }
        else
        {
            DisplayMessage("Enter Serial Number");
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void btnRefreshserial_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["dtSerial"] != null)
        {
            dt = (DataTable)ViewState["dtSerial"];
        }
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();
    }
    protected void gvSerialNumber_OnSorting(object sender, GridViewSortEventArgs e)
    {
        //HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtSerial"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " ";// + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtSerial"] = dt;
    }
    protected void btnexecute_Click(object sender, EventArgs e)
    {
        DataTable dtserial = new DataTable();
        dtserial.Columns.Add("SerialNo");
        string TransId = string.Empty;
        if (editid.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = editid.Value;
        }
        int counter = 0;
        txtSerialNo.Text = "";
        DataTable dtSockCopy = new DataTable();
        DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(ViewState["PID"].ToString());
        try
        {
            for (int i = 0; i < dtstock.Rows.Count; i++)
            {
                DataRow dr = dtserial.NewRow();
                dr[0] = dtstock.Rows[i]["SerialNo"].ToString();
                dtserial.Rows.Add(dr);
                if (txtSerialNo.Text == "")
                {
                    txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();
                }
                else
                {
                    txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();
                }
                counter++;
            }
        }
        catch
        {
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }
    }
    #endregion
    #region PendingOrder

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


    //for child grid
    public string GetOrderCurrencySymbol(string Amount)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), Amount), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }
        return Amountwithsymbol;
    }
    #endregion
    #region StockAnalysis
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

    #endregion
    protected void lblgvCustomerName_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + e.CommandArgument.ToString() + "&&Page=SINV','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public string getCurrencyConversion(string strCurrency, string strAmount)
    {
        string strConvertedAmount = string.Empty;
        try
        {
            strConvertedAmount = ObjSysParam.GetCurencyConversionForInv(strCurrency, strAmount);
        }
        catch
        {
        }
        return strConvertedAmount;
    }
    #region JobCardList

    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(int));
        dt.Columns.Add("SalesOrderNo");
        dt.Columns.Add("SoID");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
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
        dt.Columns.Add("SysQty");
        dt.Columns.Add("SoldQty");
        dt.Columns.Add("RemainQty");
        return dt;
    }

    #endregion    
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
    protected void ClosePopUp()
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Model_GST()", true);
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
    public void DeleteRowFromTempProductTaxTable(string ProductId)
    {
        DataTable dtTax = new DataTable();
        if (Session["Temp_Product_Tax_SINV"] != null)
            dtTax = (DataTable)Session["Temp_Product_Tax_SINV"];
        if (ProductId != "0" && ProductId != "" && dtTax.Rows.Count > 0)
        {
            dtTax = new DataView(dtTax, "product_id<>" + ProductId, "", DataViewRowState.CurrentRows).ToTable();
            Session["Temp_Product_Tax_SINV"] = dtTax;
        }
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }

    public void Get_Tax_From_DB()
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = IsApplyTax();
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (editid.Value != "")
                    {
                        DataTable DT_Db_Details = objSInvDetail.GetSInvDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, editid.Value, Session["FinanceYearId"].ToString());
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesInvoiceDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + editid.Value + "' and TRD.Ref_Type='SINV' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                            DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_SINV"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_SINV"] = Dt_Temp;
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
        bool IsTax = IsApplyTax();
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_SINV"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_SINV"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
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
        bool IsTax = IsApplyTax();
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_SINV"] != null)
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
        string TaxQuery = string.Empty;
        bool IsTax = IsApplyTax();
        if (Discount == "")
        {
            Discount = "0";
        }
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
                if (Session["Temp_Product_Tax_SINV"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_SINV"];
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
                        Newdr["TaxAmount"] = Convert_Into_DF(taxamount.ToString()).ToString();
                        //Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                        Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
                        Newdr["Serial_No"] = Serial_No;
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                        else
                        {
                            taxvalue = double.Parse(Convert_Into_DF(SRow[0]["Tax_Value"].ToString()));
                            taxamount = Convert.ToDouble(Convert_Into_DF(((TotalPriceBeforeDiscount * taxvalue) / 100).ToString()));
                            SRow[0]["TaxAmount"] = Convert_Into_DF(taxamount.ToString());
                            //SRow[0]["TaxAmount"] =  GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Serial_No"] = Serial_No;
                        }
                    }
                    Session["Temp_Product_Tax_SINV"] = dt;
                }
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
        if (Session["Temp_Product_Tax_SINV"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SINV"] as DataTable;
            Session["Temp_Product_Tax_SINV"] = Dt_Cal;
            Hdn_Product_Id_Tax.Value = "";
            Hdn_Serial_No_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row, ref SqlTransaction trns)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("SINV", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string R_Serial_No = string.Empty;
        string Grid = string.Empty;
        if (Grid_Name == "gvProduct")
        {
            Grid = "gvProduct";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdngvProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("txtgvSalesQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("lblgvUnitPrice");
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
            if (Session["Temp_Product_Tax_SINV"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_SINV"];
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
                        TaxValue = Convert_Into_DF(dr["Tax_Value"].ToString());
                        TaxAmount = ((Convert.ToDouble(Convert_Into_DF(dr["TaxAmount"].ToString())) * Convert.ToDouble(R_Order_Req_Qty)).ToString());
                        Amount = (Net_Amount.ToString());
                        //if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("SINV", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
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
    public void Add_Tax_In_Session_From_Order(string Amount, string ProductId, string PO_ID, string Serial_No)
    {
        string TaxQuery = string.Empty;
        bool IsTax = IsApplyTax();
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
        if (IsTax && double.Parse(Amount) > 0)
        {
            Get_Tax_Parameter();
            if (ddlTransType.SelectedIndex > 0)
            {
                //DataTable dtTax = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", PO_ID);
                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + PO_ID + "' and TRD.Ref_Type='SO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                DataTable dtTax = objDa.return_DataTable(TaxQuery);
                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_SINV"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_SINV"];
                if (dtTax.Rows.Count > 0)
                {
                    dtTax = new DataView(dtTax, "Product_Id=" + ProductId + " and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = dr["Product_Id"].ToString();
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["TaxName"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Per"].ToString();
                        Newdr["TaxAmount"] = Convert_Into_DF(dr["Tax_Value"].ToString()).ToString();
                        Newdr["Amount"] = Amount;
                        Newdr["Serial_No"] = Serial_No;
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                    }
                    Session["Temp_Product_Tax_SINV"] = dt;
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
            Hdn_Saved_Expenses_Tax_Session.Value = "Expenses_Tax_Sales_Invoice_Temp";
            Hdn_Local_Expenses_Tax_Session.Value = "Expenses_Sales_Invoice_Local";
            Hdn_Expenses_Id_Web_Control.Value = ddlExpense.SelectedValue.ToString();
            Hdn_Expenses_Name_Web_Control.Value = ddlExpense.SelectedItem.ToString();
            Hdn_Expenses_Amount_Web_Control.Value = txtFCExpAmount.Text.Trim();
            //Hdn_Session_Name_For_Expenses_Tax.Value = "PO_Expenses_Tax";
            //Hdn_Save_Session_Name_For_Expenses_Tax.Value = "Saved_PO_Expenses_Tax";
            Hdn_Page_Name_Web_Control.Value = "Sales_Invoice";
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
        }
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
            bool IsTax = IsApplyTax();
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
    public string Get_Expenses_Tax(string Debit_Amount)
    {
        string Expenses_Tax_Amount = "0";
        try
        {
            bool IsTax = IsApplyTax();
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
    protected void Btn_Exp_Reset_Click(object sender, EventArgs e)
    {
        Expenses_Tax_Modal.Expenses_Tax_And_Amount_Clear();
        Session["Expenses_Sales_Invoice_Local"] = null;
        ddlExpCurrency.SelectedValue = ddlCurrency.SelectedValue;
        // ddlExpCurrency_SelectedIndexChanged(null, null);
        ddlExpense.SelectedValue = "--Select--";
        txtExpensesAccount.Text = "";
        txtFCExpAmount.Text = "0.00";
        txtExpCharges.Text = "0.00";
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
        ddlExpense.Enabled = true;
        txtExpensesAccount.Enabled = true;
        txtFCExpAmount.Enabled = true;
        txtExpCharges.Enabled = true;
        Lbl_Expenses_Tax_Amount_ET.Text = "0.00";
        Lbl_Expenses_Tax_ET.Text = "0%";
    }
    public void Expenses_Read_Only()
    {
        ddlExpCurrency.Enabled = false;
        txtExpExchangeRate.Enabled = false;
        ddlExpense.Enabled = false;
        txtExpensesAccount.Enabled = false;
        txtFCExpAmount.Enabled = false;
        txtExpCharges.Enabled = false;
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
                Decimal_Count = cmn.Get_Decimal_Count_By_Location(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
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
    public string SetDecimal(string amount)
    {
        if (strCurrencyId == "")
        {
            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
                if (strCurrencyId != "0" && strCurrencyId != "")
                {
                    ddlCurrency.SelectedValue = strCurrencyId;
                    ddlExpCurrency.SelectedValue = strCurrencyId;
                }
            }
            catch
            {
            }
        }
        return ObjSysParam.GetCurencyConversionForInv(strCurrencyId, amount);
    }
    public static string[] Get_Amount_Calculation(string Unit_Price, string Sales_Qty, string Discount_Percentage, string Discount_Amount, string Tax_Percentage, string Tax_Amount)
    {
        string[] Str_Return = new string[6];
        if (Unit_Price.Trim() == "")
            Unit_Price = "0";
        if (Sales_Qty.Trim() == "")
            Sales_Qty = "0";
        if (Discount_Percentage.Trim() == "")
            Discount_Percentage = "0";
        if (Discount_Amount.Trim() == "")
            Discount_Amount = "0";
        if (Tax_Percentage.Trim() == "")
            Tax_Percentage = "0";
        if (Tax_Amount.Trim() == "")
            Tax_Amount = "0";
        decimal Get_Gross_Amount = 0;
        decimal Get_Discount_Percent = 0;
        decimal Get_Discount_Amount = 0;
        decimal Get_Tax_Percent = 0;
        decimal Get_Tax_Amount = 0;
        decimal Get_Net_Total_Amount = 0;
        if (Convert.ToDecimal(Sales_Qty) > 0)
            Get_Gross_Amount = Convert.ToDecimal(Unit_Price) * Convert.ToDecimal(Sales_Qty);
        else
            Get_Gross_Amount = Convert.ToDecimal(Unit_Price);
        if (Discount_Amount != "0")
        {
            Get_Discount_Percent = (Convert.ToDecimal(Discount_Amount) / Get_Gross_Amount) * 100;
            Get_Discount_Amount = Convert.ToDecimal(Discount_Amount);
        }
        if (Discount_Percentage != "0")
        {
            Get_Discount_Percent = Convert.ToDecimal(Discount_Percentage);
            Get_Discount_Amount = (Get_Gross_Amount * Convert.ToDecimal(Discount_Percentage)) / 100;
        }
        if (Tax_Amount != "0")
        {
            Get_Tax_Percent = (Convert.ToDecimal(Tax_Amount) / (Get_Gross_Amount - Get_Discount_Amount)) * 100;
            Get_Tax_Amount = Convert.ToDecimal(Tax_Amount);
        }
        if (Tax_Percentage != "0")
        {
            Get_Tax_Percent = Convert.ToDecimal(Tax_Percentage);
            Get_Tax_Amount = ((Get_Gross_Amount - Get_Discount_Amount) * Convert.ToDecimal(Tax_Percentage)) / 100;
        }
        Get_Net_Total_Amount = (Get_Gross_Amount - Get_Discount_Amount) + Get_Tax_Amount;
        Str_Return[0] = Get_Gross_Amount.ToString();
        Str_Return[1] = Get_Discount_Percent.ToString();
        Str_Return[2] = Get_Discount_Amount.ToString();
        Str_Return[3] = Get_Tax_Percent.ToString();
        Str_Return[4] = Get_Tax_Amount.ToString();
        Str_Return[5] = Get_Net_Total_Amount.ToString();
        return Str_Return;
    }
    protected void setDefaultValueForUcAcMaster(object sender, EventArgs e)
    {
        string CustomerId = txtCustomer.Text.Split('/')[1].ToString();
        UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", txtCustomer.Text.Split('/')[0].ToString());
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getUsbrousher(string CustomerName)
    {
        WebUserControl_AccountMaster UcAcMaster = new WebUserControl_AccountMaster();
        string CustomerId = CustomerName.Split('/')[1].ToString();
        try
        {
            UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", CustomerName.Split('/')[0].ToString());
            return "";
        }
      catch(Exception ex)
        {
            return "";
        }
      
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool ValidateEmployeeName(string strEmpCode, string strEmpName)
    {
        try
        {
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string sql = "select count(emp_id) from set_employeemaster where emp_name='" + strEmpName + "' and emp_code='" + strEmpCode + "'";
            return objDa.get_SingleValue(sql) == "0" ? false : true;
        }
        catch
        {
            return false;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCustomer_TextChanged(string strCustomerName, string strCustomerId)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        string sql = "select customer_id,is_block from set_customerMaster inner join ems_contactMaster on ems_contactMaster.trans_id=set_customerMaster.customer_id where Set_CustomerMaster.customer_id='" + strCustomerId + "' and ems_contactMaster.name='" + strCustomerName + "'";
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCustomer = objDa.return_DataTable(sql))
        {
            HttpContext.Current.Session["contactId"] = strCustomerId;
            if (dtCustomer.Rows.Count == 0 || bool.Parse(dtCustomer.Rows[0]["Is_Block"].ToString()))
            {
                HttpContext.Current.Session["contactId"] = null;
                return "false";
            }
        }
        HttpContext.Current.Session["SI_UnAdjustedCreditNote"] = null;
        //GetCreditInfo();
        //txtShipCustomerName.Text = txtCustomer.Text;
        string strAddress = "";
        Ems_ContactMaster objContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId);
        if (dtAddress.Rows.Count > 0)
        {
            dtAddress = new DataView(dtAddress, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAddress != null)
            {
                if (dtAddress.Rows.Count > 0)
                {
                    strAddress = dtAddress.Rows[0]["Address_Name"].ToString() + "/" + dtAddress.Rows[0]["Trans_Id"].ToString();
                }
            }
        }

        dtAddress = null;
        return strAddress;
    }
    //public void GetCreditInfo(string strCustomerName)
    //{
    //    //bool IsCredit = false;

    //    //here we will show credit terms and condition.
    //    int otherAcId = 0;
    //    try
    //    {
    //        otherAcId = objAcAccountMaster.GetCustomerAccountByCurrency(strCustomerName, ddlCurrency.SelectedValue);
    //        //IsCredit = Convert.ToBoolean(ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[1].ToString()).Rows[0]["Field41"].ToString());
    //    }
    //    catch
    //    {

    //    }


    //    if (otherAcId > 0)
    //    {
    //        trCreditInfo.Visible = true;

    //        DataTable dtCreditParameter = objCustomerCreditParam.GetRecord_By_CustomerId(strCustomerName);


    //        dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

    //        if (dtCreditParameter.Rows.Count > 0)
    //        {
    //            lblCreditLimitValue.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
    //            try
    //            {
    //                lblCurrencyCreditLimit.Text = objCurrency.GetCurrencyMasterById(dtCreditParameter.Rows[0]["Credit_Limit_Currency"].ToString().Trim()).Rows[0]["Currency_Name"].ToString();
    //            }
    //            catch
    //            {
    //            }

    //            //get current balance
    //            try
    //            {
    //                string _strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
    //                string _strOtherAcId = objAcAccountMaster.GetCustomerAccountByCurrency(dtCreditParameter.Rows[0]["Customer_id"].ToString(), ddlCurrency.SelectedValue).ToString();
    //                string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "', '0','" + _strAccountId + "','" + _strOtherAcId + "','3','" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
    //                lblCurrentBalance.Text = SystemParameter.GetAmountWithDecimal(objDa.get_SingleValue(sql), Session["LoginLocDecimalCount"].ToString());
    //            }
    //            catch (Exception ex)
    //            {
    //                lblCurrentBalance.Text = "";
    //            }

    //            lblCreditDaysValue.Text = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
    //            if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
    //            {
    //                lblCreditParameterValue.Text = "Advance Cheque Basis";
    //            }
    //            else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
    //            {
    //                lblCreditParameterValue.Text = "Invoice to Invoice Payment";
    //            }
    //            else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
    //            {
    //                lblCreditParameterValue.Text = "50% advance and 50% on delivery";
    //            }
    //            else
    //            {
    //                lblCreditParameterValue.Text = "None";
    //            }

    //        }
    //        else
    //        {
    //            lblCreditLimitValue.Text = "";
    //            lblCreditDaysValue.Text = "";
    //            lblCreditParameterValue.Text = "";
    //            lblCurrencyCreditLimit.Text = "";
    //            lblCurrentBalance.Text = "";
    //        }
    //    }

    //}



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
    //here we get salesprice according parameter
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetProductPrice(string ProductId, string CustomerID)
    {
        string UnitPrice = "0";
        Inv_ProductMaster objProductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            if (ProductId != null)
            {
                UnitPrice = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", CustomerID.Split('/')[1].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString())).ToString();
                return UnitPrice;
            }
            else
            {
                return UnitPrice;

            }
        }
        catch
        {
            return UnitPrice;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getSalesOrders(string strCustomerId)
    {
        try
        {
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_SalesOrderHeader objSOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
            List<object> sOrderList = new List<object> { };
            using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "SalesOrderApproval"))
            {
                DataTable dtSalesOrder = dtSalesOrder = objSOrderHeader.GetProductFromSalesOrderForInvoice(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), strCustomerId, HttpContext.Current.Session["FinanceYearId"].ToString()); ;
                if (Dt.Rows.Count > 0 && Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    dtSalesOrder = new DataView(dtSalesOrder, "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                }
                var tbl = from dr in dtSalesOrder.AsEnumerable()
                          select new
                          {
                              salesOrderId = dr["SoId"],
                              salesOrderDetailId = dr["Trans_id"],
                              salesOrderNo = dr["SalesOrderNo"],
                              salesOrderDate = DateTime.Parse(dr["SalesOrderDate"].ToString()).ToString("dd-MMM-yyyy"),
                              productCode = dr["productCode"],
                              productName = dr["ProductName"],
                              //productDescription = dr["ProductDescription"],
                              productDescription = "",
                              productId = dr["Product_Id"],
                              unitId = dr["UnitId"],
                              unitName = dr["Unit_Name"],
                              orderQty = dr["OrderQty"],
                              remainQty = dr["RemainQty"],
                              salesQty = dr["RemainQty"],
                              soldQty = dr["SoldQty"],
                              sysQty = dr["SysQty"],
                              freeQty = dr["FreeQty"],
                              unitPrice = dr["UnitPrice"],
                              taxPer = dr["TaxP"],
                              taxValue = dr["TaxV"],
                              discountPer = dr["DiscountP"],
                              discountValue = dr["DiscountV"],
                              inventoryType = dr["maintainStock"],
                              isDeliveryVoucher = dr["IsdeliveryVoucher"]
                              //name = p.Field<string>("name"),
                              //age = p.Field<int>("age")
                          };
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(tbl);
            }
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getSalesOrdersTax(string strOrderId, string strOrderDetailId)
    {
        try
        {
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string taxQuery = "Select TRD.Tax_Id as taxId,STM.Tax_Name as taxName,TRD.Tax_Per as taxPer  from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + strOrderId + "' and TRD.Ref_Type='SO' and TRD.Field2 ='" + strOrderDetailId + "' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
            using (DataTable dt = objDa.return_DataTable(taxQuery))
            {
                if (dt.Rows.Count > 0)
                {
                    return JsonConvert.SerializeObject(dt);
                }
                else
                {
                    return null;
                }
            }
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getCurrencyDecimalAndSymbol(string CurrencyID)
    {
        try
        {
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = new DataTable();
            string decimalCount = "", currencySymbol = "", smallestDenomination = "";
            using (dt = objDa.return_DataTable("select Sys_CurrencyMaster.Currency_Code,Sys_CurrencyMaster.field2 as smallestDenomiation,case when Sys_Country_Currency.field1 is null then '0' else Sys_Country_Currency.field1 end as decimal from Sys_CurrencyMaster left join Sys_Country_Currency on Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.Currency_ID where Sys_CurrencyMaster.Currency_Id ='" + CurrencyID + "'"))
            {
                if (dt == null)
                {
                    return null;
                }
                else
                {
                    string ExchangeRate = "";
                    ExchangeRate = SystemParameter.GetExchageRate(CurrencyID, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
                    decimalCount = dt.Rows[0]["decimal"].ToString();
                    currencySymbol = dt.Rows[0]["Currency_Code"].ToString();
                    smallestDenomination = dt.Rows[0]["smallestDenomiation"].ToString();
                    return decimalCount + "," + currencySymbol + "," + ExchangeRate + "," + smallestDenomination;
                }
            }
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] getAccountNo(string paymentMode)
    {
        string AccName = "";
        string[] result = new string[2];

        DataAccessClass daClass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtPay = daClass.return_DataTable("Select Account_No, Field1 From Set_Payment_Mode_Master Where Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' and Brand_Id = '" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "' and Pay_Mode_Id = '" + paymentMode + "' and IsActive = 'True'"))
        {
            if (dtPay.Rows.Count > 0)
            {
                string strAccountId = string.Empty;
                if (dtPay.Rows[0]["Field1"].ToString() == "Cash")
                {
                    strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                    AccName = daClass.get_SingleValue("SELECT AccountName FROM Ac_ChartOfAccount WHERE Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND IsActive = 'True' AND Trans_Id = '" + strAccountId + "' ");
                    if (AccName != "")
                    {
                        result[0] = AccName + "/" + strAccountId;
                        result[1] = "Cash";
                    }
                }
                else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                {
                    strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
                    AccName = daClass.get_SingleValue("SELECT AccountName FROM Ac_ChartOfAccount WHERE Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND IsActive = 'True' AND Trans_Id = '" + strAccountId + "' ");
                    if (AccName != "")
                    {
                        result[0] = AccName + "/" + strAccountId;
                        result[1] = "Credit";
                    }
                }
                return result;
            }
        }
        return null;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool txtPayAccountNo_TextChanged(string AccountNo)
    {
        DataTable dtAccount = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString()).GetCOAAll(HttpContext.Current.Session["CompId"].ToString());
        using (dtAccount = new DataView(dtAccount, "AccountName='" + AccountNo + "' ", "", DataViewRowState.CurrentRows).ToTable())
        {
            if (dtAccount.Rows.Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCreditNote_TextChanged(string creditNote, string currencyId, string payAmount)
    {
        try
        {
            DataTable _dt = getUnAdjustedCreditNote();
            if (_dt.Rows.Count > 0)
            {
                _dt = new DataView(_dt, "voucher_no='" + creditNote.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (_dt.Rows.Count > 0)
                {
                    if (currencyId != _dt.Rows[0]["currency_id"].ToString())
                    {
                        return "false,";
                    }
                    else if (Convert.ToDouble(payAmount) > Convert.ToDouble(_dt.Rows[0]["Actual_balance_amount"].ToString()))
                    {
                        return "Select another," + _dt.Rows[0]["Actual_balance_amount"].ToString();
                    }
                    else
                    {
                        return _dt.Rows[0]["Actual_balance_amount"].ToString() + ',' + _dt.Rows[0]["Voucher_id"].ToString();
                    }
                }
            }
        }
        catch
        {
            return "0";
        }
        return "0";
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlExpense_SelectedIndexChanged(string ExpenseValue)
    {
        DataTable dtExp = new Inv_ShipExpMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetShipExpMasterById(HttpContext.Current.Session["CompID"].ToString(), ExpenseValue);
        if (dtExp.Rows.Count > 0)
        {
            string strAccountId = dtExp.Rows[0]["Account_No"].ToString();
            DataTable dtAcc = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString()).GetCOAByTransId(HttpContext.Current.Session["CompID"].ToString(), strAccountId);
            if (dtAcc.Rows.Count > 0)
            {
                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                return strAccountName + "/" + strAccountId;
            }
        }
        return null;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtExpensesAccount_TextChanged(string ExpensesAccountName)
    {
        DataTable dtAccount = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString()).GetCOAAll(HttpContext.Current.Session["CompID"].ToString());
        try
        {
            dtAccount = new DataView(dtAccount, "AccountName='" + ExpensesAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAccount != null)
            {
                if (dtAccount.Rows.Count == 0)
                {
                    return "No Account Found";
                }
                if (dtAccount.Rows[0]["Trans_Id"].ToString() == "7")
                {
                    return "Please donot select " + ExpensesAccountName + "";
                }
                return "True";
            }
        }
        catch
        { }
        return "No Account Found";
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getProductDetail(string strSearchText, bool isSalesTax, int transType, string strCustomerId)
    {
        try
        {
            Inv_ProductMaster objProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string strSerialNo = string.Empty;
            object objProductTax = new object { };
            List<object> ls = new List<object> { };
            bool bSearchedbySerialNo = false;
            using (DataTable dt = objProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), strSearchText))
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    return "Product Not Found";
                }
                if (dt.Rows[0]["Type"].ToString() == "2")  //serial product
                {
                    if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), strSearchText))
                    {
                        return "Invalid Serial";
                    }
                    bSearchedbySerialNo = true;
                }
                //Get Product tax detail
                List<object> lstTax = new List<object> { };
                double totalTaxPer = 0;
                string strProductId = dt.Rows[0]["ProductId"].ToString();
                if (isSalesTax && transType >= 0)
                {
                    string strTaxLevel = Inv_ParameterMaster.getTaxSystemLevel(HttpContext.Current.Session["DBConnection"].ToString());
                    String Condition = string.Empty;
                    if (strTaxLevel == Resources.Attendance.Company)
                        Condition = "AND IPTM.Field1='" + strTaxLevel + "' AND IPTM.Company_ID = " + HttpContext.Current.Session["CompId"].ToString() + "";
                    else if (strTaxLevel == Resources.Attendance.Location)
                        Condition = "AND IPTM.Field1='" + strTaxLevel + "' AND IPTM.Location_ID = " + HttpContext.Current.Session["LocId"].ToString() + "";
                    string TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
                            where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + transType + ") + ',%') and IPTM.Product_Id = " + strProductId + "" + Condition + "";
                    using (DataTable dtTax = objDa.return_DataTable(TaxQuery))
                    {
                        foreach (DataRow dr in dtTax.Rows)
                        {
                            lstTax.Add(new { taxId = dr["Tax_Id"].ToString(), taxPer = dr["Tax_Value"].ToString(), taxName = dr["Tax_Name"].ToString() });
                            totalTaxPer += double.Parse(dr["Tax_Value"].ToString());
                        }
                    }
                }
                //If item type is stockable then get current stock
                double currentStock = 0;
                if (dt.Rows[0]["ItemType"].ToString() == "S")
                {
                    string sql = "select quantity from Inv_StockDetail where Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' and brand_id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and productid='" + strProductId + "' and Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' and IsActive='true'";
                    double.TryParse(objDa.get_SingleValue(sql), out currentStock);
                    currentStock = Math.Round(currentStock, 2);
                }
                //Get Sales Price
                double dUnitCost = 0;
                try
                {
                    double.TryParse(objProductMaster.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", strCustomerId, strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString(), out dUnitCost);

                }
                catch
                {
                } 
                object objItem = new
                {
                    productId = dt.Rows[0]["ProductId"].ToString(),
                    productCode = dt.Rows[0]["ProductCode"].ToString(),
                    productName = dt.Rows[0]["EProductName"].ToString(),
                    unitId = dt.Rows[0]["UnitId"].ToString(),
                    productDescription = dt.Rows[0]["Description"].ToString(),
                    inventoryType = dt.Rows[0]["MaintainStock"].ToString(),
                    unitName = dt.Rows[0]["Unit_Name"].ToString(),
                    itemType = dt.Rows[0]["ItemType"].ToString(),
                    sysQty = currentStock.ToString(),
                    unitPrice = Math.Round(dUnitCost, 2).ToString(),
                    taxPer = totalTaxPer.ToString(),
                    taxValue = "0",
                    searchBySerialNo = bSearchedbySerialNo,
                    LocationOut = "0",
                    taxDetail = lstTax
                };
                return JsonConvert.SerializeObject(objItem);
            }
        }
        catch (Exception ex)
        {
            return "Product Not Found";
        }
    }

    //protected void btnUpdateCustomerCode_Click(object sender, EventArgs e)
    //{
    //    string SysQty = string.Empty;
    //    try
    //    {

    //        //foreach (Table row in divProducts.row)
    //        //{
    //            string stCustomerProductId = ((TextBox)row.FindControl("txtCustomerCoopCode")).Text;
    //            string strProductId = ((Label)row.FindControl("lblgvProductCode")).Text;
    //            string strCustomerCode = objDa.return_DataTable("Select Code From Ems_ContactMaster Where   Trans_Id =  '" + txtCustomer.Text.Trim().Split('/')[1].ToString() + "'").Rows[0][0].ToString();

    //            int i = objDa.execute_Command("Update  Inv_Customer_Product_Code  Set Customer_Product_Code ='" + stCustomerProductId + "'  Where Customer_Code = '" + strCustomerCode + "' and Product_Code ='" + strProductId + "'");
    //            if (i == 0)
    //            {
    //                objDa.execute_Command("INSERT INTO  Inv_Customer_Product_Code VALUES('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + strCustomerCode + "', '" + strProductId + "', '" + stCustomerProductId + "', '" + Session["UserId"].ToString().ToString() + "', getdate())");
    //            }

    //        //}
    //    }
    //    catch
    //    {
    //        SysQty = "0";
    //    }


    //}

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string validateSerailNo(string strProductId, string strSerial)
    {
        try
        {
            List<object> lstSno = new List<object> { };
            string[] strSerialNos = JsonConvert.DeserializeObject<string[]>(strSerial);
            Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
            foreach (string strSno in strSerialNos)
            {
                using (DataTable dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(strProductId, strSno))
                {
                    if (dtserial.Rows.Count > 0)
                    {
                        if (dtserial.Rows[0]["InOut"].ToString() == "O")
                        {
                            lstSno.Add(new { sNo = strSno, status = "ALREADY OUT" });
                        }
                        else if (dtserial.Rows[0]["InOut"].ToString() == "I")
                        {
                            lstSno.Add(new { sNo = strSno, status = "VALID" });
                        }
                    }
                    else
                    {
                        lstSno.Add(new { sNo = strSno, status = "NOT EXISTS" });
                    }
                }
            }
            return JsonConvert.SerializeObject(lstSno);
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string fillGridNew(string ddlPosted, string ddlPageSize)
    {
        Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        string PostType = string.Empty;
        string myval = string.Empty;
        bool IsAll = true;
        if (ddlPosted == "Posted")
        {
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted == "UnPosted")
        {
            myval = "0";
            IsAll = false;
        }
        if (IsAll == false)
            PostType = " Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();
        string PageSize = PageControlCommon.GetPageSize().ToString();
        string DPageSize = ddlPageSize;
        if (DPageSize == "")
            DPageSize = "0";
        if (int.Parse(DPageSize) > int.Parse(PageSize))
            PageSize = DPageSize;
        string BatchNo = "1";
        //DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString(), PostType, true.ToString(), BatchNo, PageSize, "", "", "");
        //DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString(), PostType, false.ToString(), BatchNo, PageSize, "", "", "");

        DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString());
        DataTable dt = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString());
        HttpContext.Current.Session["dtSInvoice"] = dt;
        HttpContext.Current.Session["dtFilter_Sale_inv_mstr"] = dt;
        string lblTotalRecords = "";
        lblTotalRecords = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";
        string pager = generatePager_(int.Parse(dtRecord.Rows[0][0].ToString()), int.Parse(PageSize), 1);
        return JsonConvert.SerializeObject(dt) + "@" + pager;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string bindGridNew(string currentPage, string ddlPosted, string ddlOption, string ddlFieldName, string txtValueDate, string txtValue, string ddlPageSize, string txtCustValue)
    {
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        int _TotalRowCount = 0;
        string PostType = string.Empty;
        string myval = string.Empty;
        bool IsAll = true;
        if (ddlPosted == "Posted")
        {
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted == "UnPosted")
        {
            myval = "0";
            IsAll = false;
        }
        if (IsAll == false)
            PostType = " Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();
        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;
        if (ddlOption != "0")
        {
            string condition = string.Empty;
            if (ddlFieldName == "Supplier_Id")
            {
                string retval = txtCustValue;
                if (ddlOption == "1")
                {
                    condition = "convert(" + ddlFieldName + ",System.String)='" + retval + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption == "2")
                {
                    condition = "convert(" + ddlFieldName + ",System.String) like '%" + retval + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName + ",System.String) Like '" + retval + "%'";
                    SearchType = "Like";
                }
            }
            else
            {
                if (ddlOption == "1")
                {
                    condition = "convert(" + ddlFieldName + ",System.String)='" + txtValue + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption == "2")
                {
                    condition = "" + ddlFieldName + " like '%" + txtValue + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName + ",System.String) Like '" + txtValue + "%'";
                    SearchType = "Like";
                }
            }
            SearchField = ddlFieldName;
            SearchValue = txtValue;
            if (SearchField == "Trans_Id")
                SearchField = "Inv_SalesInvoiceHeader.Trans_Id";
            string PageSize = PageControlCommon.GetPageSize().ToString();
            string DPageSize = ddlPageSize;
            if (DPageSize == "")
                DPageSize = "0";
            if (int.Parse(DPageSize) > int.Parse(PageSize))
                PageSize = DPageSize;
            //DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString(), PostType, true.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);
            //DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString(), PostType, false.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);

            DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString());
            DataTable dt = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompID"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocID"].ToString());
            _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());
            string lblTotalRecords = "";
            string json = "";
            json = JsonConvert.SerializeObject(dt);
            lblTotalRecords = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";
            object sumObject;
            sumObject = dt.Compute("Sum(LocalGrandTotal)", "");
            string data = generatePager_(_TotalRowCount, int.Parse(PageSize), Convert.ToInt32(currentPage));
            return json + "@" + data;
        }
        return " @ ";
    }
    public static string generatePager_(int totalRowCount, int pageSize, int currentPage)
    {
        int totalLinkInPage = 5;
        int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);
        int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
        int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);
        if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
        {
            lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
            startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
        }
        List<ListItem> pageLinkContainer = new List<ListItem>();
        if (startPageLink != 1)
            pageLinkContainer.Add(new ListItem("First", "1", currentPage != 1));
        for (int i = startPageLink; i <= lastPageLink; i++)
        {
            pageLinkContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage != i));
        }
        if (lastPageLink != totalPageCount)
            pageLinkContainer.Add(new ListItem("Last", totalPageCount.ToString(), currentPage != totalPageCount));
        return JsonConvert.SerializeObject(pageLinkContainer);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getDocumentNumber()
    {
        string s = new Set_DocNumber(HttpContext.Current.Session["DBConnection"].ToString()).GetDocumentNo(true, HttpContext.Current.Session["CompID"].ToString(), true, "13", "92", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DepartmentId"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    public class clsSInvHeader
    {
        public int salesInvoiceId { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public int currencyDecimalCount { get; set; }
        public string denomination { get; set; }
        public string siFromTransType { get; set; }
        public string salesPersonId { get; set; }
        public string salesPersonCode { get; set; }
        public string salesPersonName { get; set; }
        public string posNo { get; set; }
        public string remark { get; set; }
        public string accountNo { get; set; }
        public string invoiceCosting { get; set; }
        public string grossAmount { get; set; }
        public string totalQuantity { get; set; }
        public string taxPer { get; set; }
        public string taxAmount { get; set; }
        public string discountPer { get; set; }
        public string discountAmount { get; set; }
        public string expensesAmount { get; set; }
        public string netAmount { get; set; }
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string invoiceRefNo { get; set; }
        public string invoiceMerchantId { get; set; }
        public string refOrderNumber { get; set; }
        public string condition1 { get; set; }
        public string netAmountWithExp { get; set; }
        public string jobCardId { get; set; }
        public string condition4 { get; set; }
        public string condition5 { get; set; }
        public string billingAddressId { get; set; } //field1
        public string billingAddressName { get; set; }
        public string shippingCustomerId { get; set; } //field2
        public string shippingCustomerName { get; set; }
        public string shippingAddressId { get; set; } //field3
        public string shippingAddressName { get; set; }
        public string exchangeRate { get; set; }
        public string transType { get; set; }
        public string docNo { get; set; }
        public string rowLocId { get; set; }
        public bool isApproved { get; set; }
        public string contactId { get; set; }
        public string contactName { get; set; }

        public List<clsSInvDetail> lstProductDetail { get; set; }
        public List<clsInvTaxDetail> lstTaxDetail { get; set; }
        public List<clsProductSno> lstProductSerialDetail { get; set; }
        public List<clsSInvExpenses> lstExpensesDetail { get; set; }
        public List<clsPayTrns> lstPaymentDetail { get; set; }
    }
    public class clsSInvDetail
    {
        public int siDetailId { get; set; }
        public string invoiceId { get; set; }
        public string sNo { get; set; }
        public string salesOrderId { get; set; }
        public string salesOrderNo { get; set; }
        public bool isDeliveryVoucherAllow { get; set; }
        public string productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string inventoryType { get; set; }
        public string unitId { get; set; }
        public string unitName { get; set; }
        public string unitPrice { get; set; }
        public string unitCost { get; set; }
        public string orderQty { get; set; }
        public string soldQty { get; set; }
        public string sysQty { get; set; }
        public string remainQty { get; set; }
        public string salesQty { get; set; }
        public string taxPer { get; set; }
        public string taxValue { get; set; }
        public string discountPer { get; set; }
        public string discountValue { get; set; }
        public string avgCost { get; set; } //field2
        public string freeQty { get; set; }
        public string lineTotal { get; set; }

        public string txtCustomerCoopCode { get; set; }

        public string Location_Out { get; set; }
    }
    public class clsInvTaxDetail
    {
        public string transId { get; set; }
        public string refType { get; set; }
        public string refId { get; set; }
        public string productId { get; set; }
        public string taxId { get; set; }
        public string taxName { get; set; }
        public string taxPer { get; set; }
        public string taxValue { get; set; }
        public string taxableAmount { get; set; } //field1
        public string refDetailId { get; set; }//field2
        public string transType { get; set; } //field3
        public string expensesId { get; set; }
        public string refRowSno { get; set; }
    }
    public class clsPayTrns
    {
        public int transId { get; set; }
        public string paymentModeId { get; set; }
        public string paymentModeName { get; set; }
        public string accountId { get; set; }
        public string accountName { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string localAmount { get; set; }
        public string foreignAmount { get; set; }
        public string exchangeRate { get; set; }
        public string cardNo { get; set; }
        public string cardName { get; set; }
        public string chequeNo { get; set; }
        public string chequeDate { get; set; }
        public string bankId { get; set; }
        public string bankName { get; set; }
        public string creditNoteVoucherId { get; set; } //in case of credit note(credit note id which one used in current invoice)
        public string paymentType { get; set; } //cash/credit
    }
    public class clsSInvExpenses
    {
        public string transId { get; set; }
        public string expenesId { get; set; }
        public string expensesName { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string foreignAmount { get; set; }
        public string localAmount { get; set; }
        public string exchangeRate { get; set; }
        public string accountId { get; set; }
        public string accountName { get; set; }
        public string refRowSno { get; set; }
    }
    public class clsProductSno
    {
        public string productId { get; set; }
        public string serialNo { get; set; }
        public string refRowSno { get; set; }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] updateInvoice(clsSInvHeader clsSInvHeader)
    {
        string[] result = new string[3];
        int b = 0;
        try
        {
            //***proceeds sales invoice header***
            Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string StrCompId = HttpContext.Current.Session["CompId"].ToString();
            string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
            string StrLocationId = HttpContext.Current.Session["LocId"].ToString();
            string strUserId = HttpContext.Current.Session["UserId"].ToString();

            clsSInvHeader.contactId = clsSInvHeader.contactId == "" ? "0" : clsSInvHeader.contactId;

            //**Vadidate Data**
            //Check financial year constrain
            if (!Common.IsFinancialyearAllow(Convert.ToDateTime(clsSInvHeader.invoiceDate), "I", HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {
                throw new Exception("Log In Financial year not allowing to perform this action");
            }
            //Check Sales Invoice not already exist or not
            if (clsSInvHeader.salesInvoiceId == 0)
            {
                string sql = "SELECT count(*) from Inv_SalesInvoiceHeader WHERE Company_Id = '" + StrCompId + "' AND Brand_Id = '" + StrBrandId + "' AND Location_Id = '" + StrLocationId + "' AND Invoice_No = '" + clsSInvHeader.invoiceNo + "'";
                if (objDa.get_SingleValue(sql) != "0")
                {
                    throw new Exception("Invoice No. Already Exists");
                }
            }
            if (clsSInvHeader.salesPersonCode == "" || clsSInvHeader.salesPersonCode == "0")
            {
                throw new Exception("Invalid sales person");
            }
            if (clsSInvHeader.customerId == "" || clsSInvHeader.customerId == "0")
            {
                throw new Exception("Invalid Customer");
            }
            if (clsSInvHeader.billingAddressId == "" || clsSInvHeader.billingAddressId == "0")
            {
                throw new Exception("Invalid billing address");
            }
            if (clsSInvHeader.billingAddressId == "" || clsSInvHeader.billingAddressId == "0")
            {
                throw new Exception("Invalid billing address");
            }
            if (clsSInvHeader.shippingCustomerId == "" || clsSInvHeader.shippingCustomerId == "0")
            {
                throw new Exception("Invalid Shipping customer");
            }
            if (clsSInvHeader.shippingAddressId == "" || clsSInvHeader.shippingAddressId == "0")
            {
                throw new Exception("Invalid Shippling address");
            }
            if (clsSInvHeader.shippingCustomerId == "" || clsSInvHeader.shippingCustomerId == "0")
            {
                throw new Exception("Invalid Shipping customer");
            }
            //Check invoice amount and payment amount should same
            //check credit amount in payment detail
            //here we are checking Payment Mode and Credit Amount as per current invoice
            //--------------start-------------------
            //Set_Payment_Mode_Master ObjPaymentMaster = new Set_Payment_Mode_Master();
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Common objCmn = new Common(HttpContext.Current.Session["DBConnection"].ToString());
            double creditAmount = 0;
            string strPaymentMode = "0";
            //DataTable dtPayMaster = ObjPaymentMaster.GetPaymentModeMaster(StrCompId);
            //var JoinResult = (from p in dtPayMaster.AsEnumerable()
            //                  join t in clsSInvHeader.lstPaymentDetail.AsEnumerable()
            //                  on (p.Field<int>("Pay_Mode_Id")).ToString() equals (t.paymentModeId).ToString()
            //                  select new
            //                  {
            //                      PaymentMode = p.Field<string>("field1"),
            //                      Amount = t.foreignAmount,
            //                  }).ToList();
            //DataTable dtPayByMaster = objCmn.ListToDataTable(JoinResult);
            //if (dtPayByMaster.Rows.Count > 1)
            //{
            //    var ab = dtPayByMaster.AsEnumerable()
            //        .Where(x => x.Field<string>("PaymentMode") == "Credit")
            //        .Sum(x => Convert.ToDouble(x["Amount"]));
            //    double.TryParse(ab.ToString(), out creditAmount);
            //}
            //else
            //{
            //    if (dtPayByMaster.Rows[0]["PaymentMode"].ToString() == "Credit")
            //    {
            //        double.TryParse(dtPayByMaster.Rows[0]["Amount"].ToString(), out creditAmount);
            //    }
            //}
            //dtPayByMaster = null;
            //dtPayMaster = null;
            //------------

            foreach (clsPayTrns clsPayment in clsSInvHeader.lstPaymentDetail)
            {
                if (clsPayment.paymentType == "Credit")
                {
                    creditAmount += Convert.ToDouble(clsPayment.foreignAmount);
                }
            }
            //Here we are checking in case of credit invoice, Customer Account exist or his credit limit
            Ac_AccountMaster objAcAccountMaster = new Ac_AccountMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string strOtherAccountId = "0";
            if (creditAmount > 0)
            {
                strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(clsSInvHeader.customerId, clsSInvHeader.currencyId).ToString();
                if (strOtherAccountId == "0")
                {
                    throw new Exception("Account Detail not exist for this customer, Pleae first create Account");
                }
                //check credit limit
                Set_CustomerMaster_CreditParameter objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(HttpContext.Current.Session["DBConnection"].ToString());
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
                {
                    string[] _result = objCustomerCreditParam.checkCreditLimit(clsSInvHeader.salesInvoiceId, creditAmount, clsSInvHeader.customerId, strOtherAccountId, clsSInvHeader.invoiceDate, clsSInvHeader.currencyId, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                    if (_result[0] == "false")  //array index 0 - return true/false and 1 - return message
                    {
                        throw new Exception(_result[1]);
                    }
                }
            }
            //----------------------------end----------------
            clsSInvHeader.accountNo = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
            string Emp_Code = clsSInvHeader.salesPersonCode;
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            Inv_SalesInvoiceDetail objSInvDetail = new Inv_SalesInvoiceDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_ShipExpHeader ObjShipExpHeader = new Inv_ShipExpHeader(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_PaymentTrans ObjPaymentTrans = new Inv_PaymentTrans(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_TaxRefDetail objTaxRefDetail = new Inv_TaxRefDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_ShipExpDetail ObjShipExpDetail = new Inv_ShipExpDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_SalesDeliveryVoucher_Header objdelVoucherHeader = new Inv_SalesDeliveryVoucher_Header(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_SalesDeliveryVoucher_Detail objdelVoucherDetail = new Inv_SalesDeliveryVoucher_Detail(HttpContext.Current.Session["DBConnection"].ToString());
            Set_DocNumber objDocNo = new Set_DocNumber(HttpContext.Current.Session["DBConnection"].ToString());
            bool isDeliveryVoucher = Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Is Delivery Voucher allow").Rows[0]["ParameterValue"].ToString());
            int deliveryVoucherId = 0;
            string strDeliveryDocNo = "";
            int deliveryVoucherCounter = 0;
            if (isDeliveryVoucher == true)
            {
                strDeliveryDocNo = objDocNo.GetDocumentNo(true, StrCompId, true, "13", "327", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DepartmentId"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            }
            using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
            {
                con.Open();
                SqlTransaction trns = con.BeginTransaction();
                string strSOId = string.Empty;
                string TransactionType = int.Parse(clsSInvHeader.transType) >= 0 ? clsSInvHeader.transType : "-1";
                try
                {
                    if (clsSInvHeader.salesInvoiceId > 0)
                    {
                        b = objSInvHeader.UpdateSInvHeader(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), clsSInvHeader.invoiceNo, clsSInvHeader.invoiceDate, strPaymentMode, clsSInvHeader.currencyId, clsSInvHeader.siFromTransType, "0", Emp_ID, "0", clsSInvHeader.remark, clsSInvHeader.accountNo, clsSInvHeader.invoiceCosting, "", "false", clsSInvHeader.grossAmount, clsSInvHeader.grossAmount, clsSInvHeader.totalQuantity, clsSInvHeader.grossAmount, clsSInvHeader.taxPer, clsSInvHeader.taxAmount, clsSInvHeader.grossAmount, clsSInvHeader.discountPer, clsSInvHeader.discountAmount, clsSInvHeader.netAmount, clsSInvHeader.customerId, clsSInvHeader.invoiceRefNo, clsSInvHeader.invoiceMerchantId, clsSInvHeader.refOrderNumber, clsSInvHeader.condition1, clsSInvHeader.netAmountWithExp, clsSInvHeader.jobCardId, clsSInvHeader.condition4, "", clsSInvHeader.billingAddressId, clsSInvHeader.shippingCustomerId, clsSInvHeader.shippingAddressId, "Approved", clsSInvHeader.exchangeRate, "True", DateTime.Now.ToString(), "True", strUserId, DateTime.Now.ToString(), TransactionType, clsSInvHeader.contactId, ref trns);
                        //delete record from child tables
                        objSInvDetail.DeleteSInvDetail(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), ref trns);
                        ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), "SI", ref trns);
                        ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "SI", clsSInvHeader.salesInvoiceId.ToString(), ref trns);
                        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SINV", clsSInvHeader.salesInvoiceId.ToString(), ref trns);
                        string sql = "DELETE FROM Inv_StockBatchMaster Where [TransType] = 'SI' and [TransTypeId] = '" + clsSInvHeader.salesInvoiceId + "'";
                        objDa.execute_Command(sql, ref trns);
                        ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), "SI", ref trns);
                    }
                    else
                    {
                        if (clsSInvHeader.invoiceRefNo == "")
                        {
                            clsSInvHeader.invoiceRefNo = "0";
                        }

                        b = objSInvHeader.InsertSInvHeader(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.invoiceNo, clsSInvHeader.invoiceDate, strPaymentMode, clsSInvHeader.currencyId, clsSInvHeader.siFromTransType, "0", Emp_ID, "0", clsSInvHeader.remark, clsSInvHeader.accountNo, clsSInvHeader.invoiceCosting, "", "false", "0.00", clsSInvHeader.grossAmount, clsSInvHeader.totalQuantity, clsSInvHeader.grossAmount, clsSInvHeader.taxPer, clsSInvHeader.taxAmount, clsSInvHeader.grossAmount, clsSInvHeader.discountPer, clsSInvHeader.discountAmount, clsSInvHeader.netAmount, clsSInvHeader.customerId, clsSInvHeader.invoiceRefNo, clsSInvHeader.invoiceMerchantId, clsSInvHeader.refOrderNumber, clsSInvHeader.condition1, clsSInvHeader.netAmountWithExp, clsSInvHeader.jobCardId, clsSInvHeader.condition4, "", clsSInvHeader.billingAddressId, clsSInvHeader.shippingCustomerId, clsSInvHeader.shippingAddressId, "Approved", clsSInvHeader.exchangeRate, "True", DateTime.Now.ToString(), "True", strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), TransactionType, clsSInvHeader.contactId, ref trns);
                        clsSInvHeader.salesInvoiceId = b;
                        //Update invoice no
                        if (clsSInvHeader.invoiceNo == clsSInvHeader.docNo)
                        {
                            string sql = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + StrCompId + "' AND Brand_Id = '" + StrBrandId + "' AND Location_Id = '" + StrLocationId + "'";
                            int strInvoiceCount = Int32.Parse(objDa.get_SingleValue(sql, ref trns));
                            bool bFlag = false;
                            while (bFlag == false)
                            {
                                clsSInvHeader.invoiceNo = clsSInvHeader.docNo + (strInvoiceCount == 0 ? "1" : strInvoiceCount.ToString());
                                string sql1 = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + StrCompId + "' AND Brand_Id = '" + StrBrandId + "' AND Location_Id = '" + StrLocationId + "' and invoice_no='" + clsSInvHeader.invoiceNo + "'";
                                string strInvCount = objDa.get_SingleValue(sql1, ref trns);
                                if (strInvCount == "0")
                                {
                                    bFlag = true;
                                }
                                else
                                {
                                    strInvoiceCount++;
                                }
                            }

                            objSInvHeader.Updatecode(b.ToString(), clsSInvHeader.invoiceNo, ref trns);
                        }
                    }
                    if (b == 0)
                    {
                        throw new Exception("There is some error to update invoice");
                    }
                    //update child table
                    //proceeds product detail(sales invoice detail table)
                    Inv_StockDetail objStockDetail = new Inv_StockDetail(HttpContext.Current.Session["DBConnection"].ToString());
                    foreach (clsSInvDetail clsSiDetail in clsSInvHeader.lstProductDetail)
                    {
                        try
                        {
                            clsSiDetail.avgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(StrCompId, StrBrandId, StrLocationId, HttpContext.Current.Session["FinanceYearId"].ToString(), clsSiDetail.productId).Rows[0]["Field2"].ToString();
                        }
                        catch
                        {
                            clsSiDetail.avgCost = "0";
                        }
                        //remove if add location out again
                        clsSiDetail.Location_Out = "";
                        if (clsSiDetail.Location_Out == "")
                        {
                            clsSiDetail.siDetailId = objSInvDetail.InsertSInvDetail(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), clsSiDetail.sNo, strPaymentMode, "0", clsSInvHeader.siFromTransType, clsSiDetail.salesOrderId, clsSiDetail.productId, clsSiDetail.productDescription, clsSiDetail.unitId, clsSiDetail.unitPrice, "0", clsSiDetail.orderQty, clsSiDetail.salesQty, clsSiDetail.taxPer, clsSiDetail.taxValue, clsSiDetail.discountPer, clsSiDetail.discountValue, "False", false.ToString(), clsSiDetail.avgCost, clsSiDetail.freeQty, "", "", "True", DateTime.Now.ToString(), "True", strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            clsSiDetail.siDetailId = objSInvDetail.InsertSInvDetail(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), clsSiDetail.sNo, strPaymentMode, "0", clsSInvHeader.siFromTransType, clsSiDetail.salesOrderId, clsSiDetail.productId, clsSiDetail.productDescription, clsSiDetail.unitId, clsSiDetail.unitPrice, "0", clsSiDetail.orderQty, clsSiDetail.salesQty, clsSiDetail.taxPer, clsSiDetail.taxValue, clsSiDetail.discountPer, clsSiDetail.discountValue, "False", false.ToString(), clsSiDetail.avgCost, clsSiDetail.freeQty, clsSiDetail.Location_Out.Trim().Split('/')[2].ToString(), clsSiDetail.Location_Out, "True", DateTime.Now.ToString(), "True", strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                        }




                        //proceed with product serial nos
                        foreach (clsProductSno clsProductSerial in clsSInvHeader.lstProductSerialDetail)
                        {
                            if (clsSiDetail.sNo == clsProductSerial.refRowSno)
                            {
                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId, StrBrandId, StrLocationId, "SI", clsSInvHeader.salesInvoiceId.ToString(), clsSiDetail.productId, clsSiDetail.unitId, "O", "0", "0", "1", DateTime.Now.ToString(), clsProductSerial.serialNo.Trim(), "1/1/1800", "", "", "", "", clsSiDetail.salesOrderId, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), strUserId.ToString(), DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                            }
                        }
                        //proceed with tax detail
                        double taxValue = 0;
                        double.TryParse(clsSiDetail.taxValue, out taxValue);
                        if (taxValue > 0)
                        {
                            double discountValue = 0;
                            double.TryParse(clsSiDetail.discountValue, out discountValue);
                            double actualUnitPrice = double.Parse(clsSiDetail.unitPrice) - discountValue;
                            double taxableAmt = actualUnitPrice * double.Parse(clsSiDetail.salesQty);
                            foreach (clsInvTaxDetail clsTax in clsSInvHeader.lstTaxDetail)
                            {
                                if (clsSiDetail.sNo == clsTax.refRowSno)
                                {
                                    string strTaxPerUnit = SystemParameter.GetAmountWithDecimal((actualUnitPrice * double.Parse(clsTax.taxPer) / 100).ToString(), "3"); //hardcoded pass 3 to get accuracy
                                    string strTaxOnTotalUnit = SystemParameter.GetAmountWithDecimal((double.Parse(clsSiDetail.salesQty) * double.Parse(strTaxPerUnit)).ToString(), "3"); //hardcoded pass 3 to get accuracy
                                    objTaxRefDetail.InsertRecord("SINV", clsSInvHeader.salesInvoiceId.ToString(), "0", "0", clsSiDetail.productId, clsTax.taxId, clsTax.taxPer, strTaxOnTotalUnit, false.ToString(), taxableAmt.ToString(), clsSiDetail.siDetailId.ToString(), clsSInvHeader.transType, "", "", "True", DateTime.Now.ToString(), "True", strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        //check for delivery voucher
                        if (isDeliveryVoucher && !clsSiDetail.isDeliveryVoucherAllow && clsSInvHeader.siFromTransType == "S")
                        {
                            if (deliveryVoucherId == 0)//search existing voucher (need to add condition to execute only in case of edit)
                            {
                                string sql = "select Trans_Id from Inv_SalesDeliveryVoucher_Header where Field2='" + clsSInvHeader.salesInvoiceId + "'";
                                int.TryParse(objDa.get_SingleValue(sql, ref trns), out deliveryVoucherId);
                                if (deliveryVoucherId > 0) //if record found we will delete existing record
                                {
                                    objdelVoucherDetail.DeleteRecord_By_VoucherNo(StrCompId, StrBrandId, StrLocationId, deliveryVoucherId.ToString());
                                }
                            }
                            if (deliveryVoucherId == 0) //if not found then insert
                            {
                                int recCount = 0;
                                string sql = "select count(*) from Inv_SalesDeliveryVoucher_Header where Company_Id = '" + StrCompId + "' AND Brand_Id = '" + StrBrandId + "' AND Location_Id = '" + StrLocationId + "'";
                                int.TryParse(objDa.get_SingleValue(sql, ref trns), out recCount);
                                strDeliveryDocNo = strDeliveryDocNo + (recCount + 1);
                                deliveryVoucherId = objdelVoucherHeader.InsertRecord(StrCompId, StrBrandId, StrLocationId, strDeliveryDocNo, DateTime.Now.ToString(), clsSiDetail.salesOrderId, clsSInvHeader.customerId, Emp_ID, "True", "Created From Sales Invoice", clsSInvHeader.invoiceNo, clsSInvHeader.salesInvoiceId.ToString(), "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                            }
                            objdelVoucherDetail.InsertRecord(StrCompId, StrBrandId, StrLocationId, deliveryVoucherId.ToString(), clsSiDetail.salesOrderId, (deliveryVoucherCounter++).ToString(), clsSiDetail.productId, clsSiDetail.unitId, clsSiDetail.orderQty, clsSiDetail.salesQty, "True", ref trns);
                        }
                    }
                    //proceed with expenses detail
                    if (double.Parse(clsSInvHeader.expensesAmount) > 0)
                    {
                        ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), clsSInvHeader.currencyId, "0", "0", (double.Parse(clsSInvHeader.netAmount) - double.Parse(clsSInvHeader.expensesAmount)).ToString(), clsSInvHeader.expensesAmount, "SI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                        foreach (clsSInvExpenses clsExpenses in clsSInvHeader.lstExpensesDetail)
                        {
                            ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, clsSInvHeader.salesInvoiceId.ToString(), clsExpenses.expenesId, clsExpenses.accountId, clsExpenses.localAmount, clsExpenses.currencyId, clsExpenses.exchangeRate, clsExpenses.foreignAmount, "SI", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //proceed with payment detail
                    foreach (clsPayTrns clsPayment in clsSInvHeader.lstPaymentDetail)
                    {
                        ObjPaymentTrans.insert(StrCompId, clsPayment.paymentModeId, "SI", clsSInvHeader.salesInvoiceId.ToString(), "0", clsPayment.accountId, clsPayment.cardNo, clsPayment.cardName, "", clsPayment.bankId, "", clsPayment.chequeNo, clsPayment.chequeDate, clsPayment.localAmount, clsPayment.currencyId, clsPayment.exchangeRate, clsPayment.foreignAmount, "", "", clsPayment.creditNoteVoucherId, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                    }

                    trns.Commit();

                    // added by divya
                    // this code checks if this invoice had been generated from inquiry/opportunity or not
                    // if yes, then code will update the sales stage of the inquiry/opportunity
                    try
                    {
                        if (b != 1)
                        {
                            string inquiryId = "";
                            inquiryId = objDa.get_SingleValue("SELECT Inv_SalesInquiryHeader.SInquiryID AS invoice_transID FROM Inv_SalesInquiryHeader INNER JOIN Inv_SalesQuotationHeader ON Inv_SalesQuotationHeader.SInquiry_No = Inv_SalesInquiryHeader.SInquiryID INNER JOIN inv_salesorderheader ON inv_salesorderheader.SOfromTransNo = Inv_SalesQuotationHeader.SQuotation_id INNER JOIN Inv_SalesInvoiceDetail ON Inv_SalesInvoiceDetail.SIFromTransNo = inv_salesorderheader.Trans_Id WHERE Inv_SalesInvoiceDetail.Invoice_No = '" + b.ToString() + "'");
                            if (inquiryId != "")
                            {
                                new Inv_SalesInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString()).UpdateSalesStageFromInvoice(inquiryId, b.ToString());
                            }
                        }
                    }
                    catch
                    {
                    }

                    b = clsSInvHeader.salesInvoiceId;
                }
                catch (Exception ex)
                {
                    trns.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            result[0] = "fail";
            result[1] = ex.Message;
            return result;
        }
        result[0] = "Success";
        result[1] = "Saved Successfully";
        result[2] = b.ToString();
        return result;
    }
    protected void ddlPaymentTypeMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillTabPaymentMode(ddlTabPaymentMode.SelectedItem.Text);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlPaymentMode_SelectedIndexChanged(string SelectedValue)
    {
        string txtPayAccountNo;
        Sales_SalesInvoice2 S = new Sales_SalesInvoice2();
        string StrCompId = HttpContext.Current.Session["CompID"].ToString();
        string BrandId = HttpContext.Current.Session["BrandId"].ToString();
        string LocId = HttpContext.Current.Session["LocId"].ToString();
        Set_Payment_Mode_Master ObjPaymentMaster = null;
        Ac_ChartOfAccount objCOA = null;
        //txtCreditNote = SelectedValue.ToUpper() == "CREDIT NOTE" ? true : false;
        if (SelectedValue == "--Select--")
        {
            return txtPayAccountNo = "";
        }
        else if (SelectedValue != "--Select--")
        {
            strSiCurrencyId = int.Parse(SelectedValue);
            if (S.ViewState["PayementDt"] != null)
            {
                DataTable dt = (DataTable)S.ViewState["PayementDt"];
                dt = new DataView(dt, "PaymentModeId='" + SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    // DataTable dtPay = S.ObjPaymentMaster.GetPaymentModeMasterById(S.StrCompId, SelectedValue, S.Session["BrandId"].ToString(), S.Session["LocId"].ToString());
                    DataTable dtPay = S.ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, SelectedValue, BrandId, LocId);

                    if (dtPay.Rows.Count > 0)
                    {
                        string strAccountId = string.Empty;
                        if (dtPay.Rows[0]["Field1"].ToString() == "Cash")
                        {
                            strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                            DataTable dtAcc = S.objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo = strAccountName + "/" + strAccountId;

                                return txtPayAccountNo;
                            }
                        }
                        else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                        {
                            strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                            DataTable dtAcc = S.objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo = strAccountName + "/" + strAccountId;
                                return txtPayAccountNo;
                            }
                        }
                    }

                    return txtPayAccountNo = "";
                }
                else
                {
                    //for Account Fill
                    DataTable dtPay = S.ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, SelectedValue, BrandId, LocId);
                    if (dtPay.Rows.Count > 0)
                    {
                        string strAccountId = string.Empty;
                        if (dtPay.Rows[0]["Field1"].ToString() == "Cash")
                        {
                            strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                            DataTable dtAcc = S.objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo = strAccountName + "/" + strAccountId;
                                return txtPayAccountNo;
                            }
                        }
                        else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                        {
                            strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                            DataTable dtAcc = S.objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo = strAccountName + "/" + strAccountId;
                                return txtPayAccountNo;
                            }
                        }
                    }

                    S.txtPayAmount.Text = dt.Rows[0]["Pay_Charges"].ToString();
                    S.txtPayCardNo.Text = dt.Rows[0]["CardNo"].ToString();
                    S.txtPayCardName.Text = dt.Rows[0]["CardName"].ToString();
                    if (dt.Rows[0]["BankId"].ToString() != "--Sel")
                    {
                        try
                        {
                            S.ddlPayBank.SelectedValue = dt.Rows[0]["BankId"].ToString();
                        }
                        catch
                        {

                        }
                    }
                    //txtPayBankAccountNo.Text = dt.Rows[0]["BankAccountNo"].ToString();
                    //txtPayBankAccountName.Text = dt.Rows[0]["BankAccountName"].ToString();
                    S.txtPayChequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                    S.txtPayChequeDate.Text = Convert.ToDateTime(dt.Rows[0]["ChequeDate"].ToString()).ToShortDateString();
                }
                return txtPayAccountNo = "";
            }
            else
            {
                HttpContext.Current.Session["DBConnection"].ToString();
                ObjPaymentMaster = new Set_Payment_Mode_Master(HttpContext.Current.Session["DBConnection"].ToString());
                DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, SelectedValue, BrandId, LocId);
                objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
                if (dtPay.Rows.Count > 0)
                {
                    string strAccountId = string.Empty;
                    if (dtPay.Rows[0]["Field1"].ToString() == "Cash")
                    {
                        strAccountId = dtPay.Rows[0]["Account_No"].ToString();
                        DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo = strAccountName + "/" + strAccountId;
                            return txtPayAccountNo;
                        }
                    }
                    else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                    {
                        strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                        DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo = strAccountName + "/" + strAccountId;
                            return txtPayAccountNo;
                        }
                    }
                }
                return txtPayAccountNo = "";
            }
        }
        return txtPayAccountNo = "";
        //here we showing related field according the select payment mode

        //when payment mode is cash then we showing accounts no only 
        //if (ddlTabPaymentMode.SelectedIndex != 0)
        //{
        //    if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Cash")
        //    {
        //        pnlpaybank.Visible = true;
        //    }
        //    else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Credit")
        //    {
        //        pnlpaybank.Visible = true;
        //        lblPayBank.Visible = true;
        //        //lblpaybankcolon.Visible = true;
        //        ddlPayBank.Visible = true;
        //        trcheque.Visible = true;
        //    }
        //    else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Card")
        //    {
        //        pnlpaybank.Visible = true;
        //        trcard.Visible = true;
        //    }
        //}

        //GetBalanceAmount();
        //txtPayAmount_OnTextChanged(null, null);
    }
  

  

    protected void btnUpdateCancelInvoice_Click(object sender, EventArgs e)
    {
        //if (txtCancelDescription.Text == "")
        //{
        //    DisplayMessage("Please fill comment before Cancel the Invoice");
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        //    return;
        //}


        //string strCancelQuery = "update Inv_SalesInvoiceHeader set IsCancel='True',Cancelby='" + StrUserId + "',CancelDate = '" + DateTime.Now.ToString() + "', CancelRemark='" + txtCancelDescription.Text + "' Where[Company_Id] = '" + StrCompId + "' and  [Brand_Id] = '" + StrBrandId + "' and[Location_Id] = '" + StrLocationId + "' and [Trans_Id] = '" + editid.Value + "'";
        //objDa.execute_Command(strCancelQuery);

        DisplayMessage("Record Cancelled Successfully !");

        //FillGridBin(); //Update grid view in bin tab
        FillGrid();
       // FillGridCancel();
        Reset();
        Response.Redirect("SalesInvoice.aspx");
        //AllPageCode();
    }





    [WebMethod]   
    [System.Web.Script.Services.ScriptMethod()]
    public static string getInvoice(string strInvoiceId, string strBtn, string locationId = "0")
    {
        try
        {
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            clsSInvHeader clsSiHeader = new clsSInvHeader();
            Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_SalesInvoiceDetail objSInvDetail = new Inv_SalesInvoiceDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string StrCompId = HttpContext.Current.Session["CompId"].ToString();
            string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
            string StrLocationId = "";
            if (locationId == "" || locationId == "0")
            {
                StrLocationId = HttpContext.Current.Session["LocId"].ToString();
            }
            else
            {
                StrLocationId = locationId;
            }
            string strUserId = HttpContext.Current.Session["UserId"].ToString();
            List<clsSInvDetail> lstClsSInvDetail = new List<clsSInvDetail> { };
            List<clsInvTaxDetail> lstClsInvTaxDetail = new List<clsInvTaxDetail> { };
            List<clsProductSno> lstClsProductSno = new List<clsProductSno> { };
            using (DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, strInvoiceId))
            {
                //fill sales invoice header class
                DataRow dr = dtInvEdit.Rows[0];
                if (string.IsNullOrEmpty(dr["currencyDecimalCount"].ToString()) || dr["currencyDecimalCount"].ToString() == "0")
                {
                    clsSiHeader.currencyDecimalCount = 2;
                }
                else
                {
                    clsSiHeader.currencyDecimalCount = int.Parse(dr["currencyDecimalCount"].ToString());
                }

                clsSiHeader.denomination = dr["denomination"].ToString();
                if (!String.IsNullOrEmpty(dr["Condition4"].ToString()))
                    clsSiHeader.transType = dr["Condition4"].ToString();
                clsSiHeader.salesInvoiceId = Convert.ToInt32(dr["trans_id"].ToString());
                clsSiHeader.invoiceNo = dr["Invoice_No"].ToString();
                clsSiHeader.billingAddressName = dr["billingAddressName"].ToString(); //update code
                clsSiHeader.billingAddressId = dr["Field1"].ToString();
                clsSiHeader.shippingAddressName = dr["shippingAddressName"].ToString(); ; //update code
                clsSiHeader.shippingAddressId = dr["Field3"].ToString();
                clsSiHeader.shippingCustomerId = dr["Field2"].ToString();
                clsSiHeader.shippingCustomerName = dr["ShipCustomerName"].ToString();
                clsSiHeader.invoiceDate = Convert.ToDateTime(dr["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                clsSiHeader.siFromTransType = dr["SIFromTransType"].ToString();
                clsSiHeader.invoiceRefNo = dr["Invoice_Ref_No"].ToString();
                clsSiHeader.refOrderNumber = dr["Ref_Order_Number"].ToString();
                clsSiHeader.customerId = dr["Supplier_Id"].ToString();
                clsSiHeader.customerName = dr["CustomerName"].ToString();
                clsSiHeader.exchangeRate = dr["Field5"].ToString();
                clsSiHeader.salesPersonId = dr["SalesPerson_Id"].ToString();
                clsSiHeader.salesPersonCode = dr["salesPersonCode"].ToString();
                clsSiHeader.salesPersonName = dr["salesPersonName"].ToString();
                clsSiHeader.accountNo = dr["Account_No"].ToString();
                clsSiHeader.contactId = dr["Contactid"].ToString();
                clsSiHeader.contactName = dr["contactName"].ToString();

                HttpContext.Current.Session["ContactId"] = dr["Supplier_Id"].ToString();

                clsSiHeader.invoiceCosting = SystemParameter.GetAmountWithDecimal(dr["Invoice_Costing"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.totalQuantity = SystemParameter.GetAmountWithDecimal(dr["TotalQuantity"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.grossAmount = SystemParameter.GetAmountWithDecimal(dr["TotalAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.taxPer = SystemParameter.GetAmountWithDecimal(dr["NetTaxP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.taxAmount = SystemParameter.GetAmountWithDecimal(dr["NetTaxV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.netAmount = SystemParameter.GetAmountWithDecimal(dr["NetAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.discountPer = SystemParameter.GetAmountWithDecimal(dr["NetDiscountP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.discountAmount = SystemParameter.GetAmountWithDecimal(dr["NetDiscountV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.grossAmount = SystemParameter.GetAmountWithDecimal(dr["GrandTotal"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.currencyId = dr["Currency_Id"].ToString();
                if (dr["Invoice_Merchant_Id"].ToString() != "0" && dr["Invoice_Merchant_Id"].ToString() != "")
                {
                    clsSiHeader.invoiceMerchantId = dr["Invoice_Merchant_Id"].ToString();
                }
                else
                {
                    clsSiHeader.invoiceMerchantId = "0";
                }
                if (dr["Condition3"].ToString() != "")
                {
                    clsSiHeader.jobCardId = dr["Condition3"].ToString();
                }
                try
                {
                    clsSiHeader.condition1 = dr["Condition1"].ToString();
                }
                catch { }
                clsSiHeader.remark = dr["Remark"].ToString();
                if (clsSiHeader.condition1.Trim() == "")
                {
                    clsSiHeader.condition1 = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();
                }
                //clsSiHeader.rowLocId = dr["Row_Lock_Id"].ToString();
                clsSiHeader.isApproved = dr["Field4"].ToString() == "Approved" ? true : false;
                //Get Detail Record
                using (DataTable dtDetail = objSInvDetail.GetSInvDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, strInvoiceId, HttpContext.Current.Session["FinanceYearId"].ToString()))
                {
                    DataTable dtSerial = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(StrCompId, StrBrandId, StrLocationId, "SI", strInvoiceId);
                    DataTable dtTax = new DataTable();
                    if (double.Parse(clsSiHeader.taxAmount) > 0)
                    {
                        string sql = "SELECT Inv_TaxRefDetail.Tax_Id,Inv_TaxRefDetail.Tax_Per, Sys_TaxMaster.Tax_Name as taxName,Inv_TaxRefDetail.Field2 FROM Inv_TaxRefDetail left join Sys_TaxMaster on Sys_TaxMaster.Trans_Id = Inv_TaxRefDetail.Tax_Id WHERE Inv_TaxRefDetail.Ref_Type = 'SINV' AND Inv_TaxRefDetail.Ref_Id = '" + clsSiHeader.salesInvoiceId + "'";
                        dtTax = objDa.return_DataTable(sql);
                    }
                    int counter = 0;
                    foreach (DataRow drDetail in dtDetail.Rows)
                    {
                        counter++;
                        clsSInvDetail clsDetail = new clsSInvDetail();
                        clsDetail.sNo = counter.ToString();
                        clsDetail.siDetailId = Convert.ToInt32(drDetail["Trans_id"].ToString());
                        clsDetail.salesOrderId = drDetail["SoID"].ToString();
                        clsDetail.salesOrderNo = drDetail["SalesOrderNo"].ToString();
                        clsDetail.isDeliveryVoucherAllow = false; //need to check
                        clsDetail.productCode = drDetail["productCode"].ToString();
                        clsDetail.productId = drDetail["Product_Id"].ToString();
                        clsDetail.productName = drDetail["ProductName"].ToString();
                        clsDetail.productDescription = drDetail["ProductDescription"].ToString();
                        clsDetail.unitId = drDetail["Unit_Id"].ToString();
                        clsDetail.unitName = drDetail["Unit_Name"].ToString();
                        clsDetail.unitPrice = SystemParameter.GetAmountWithDecimal(drDetail["UnitPrice"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.freeQty = SystemParameter.GetAmountWithDecimal(drDetail["FreeQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.orderQty = SystemParameter.GetAmountWithDecimal(drDetail["OrderQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.soldQty = SystemParameter.GetAmountWithDecimal(drDetail["SoldQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.sysQty = SystemParameter.GetAmountWithDecimal(drDetail["SysQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.remainQty = SystemParameter.GetAmountWithDecimal(drDetail["RemainQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.salesQty = SystemParameter.GetAmountWithDecimal(drDetail["Quantity"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.discountPer = SystemParameter.GetAmountWithDecimal(drDetail["DiscountP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.discountValue = SystemParameter.GetAmountWithDecimal(drDetail["DiscountV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.taxPer = SystemParameter.GetAmountWithDecimal(drDetail["TaxP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.taxValue = SystemParameter.GetAmountWithDecimal(drDetail["TaxV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.inventoryType = drDetail["MaintainStock"].ToString();
                        clsDetail.Location_Out = "";
                        lstClsSInvDetail.Add(clsDetail);
                        //Add Serial No detail.
                        if (dtSerial.Rows.Count > 0)
                        {
                            using (DataTable dtSerialTemp = new DataView(dtSerial, "ProductId='" + clsDetail.productId + "'", "", DataViewRowState.CurrentRows).ToTable())
                            {
                                foreach (DataRow drSerial in dtSerialTemp.Rows)
                                {
                                    clsProductSno clsSerial = new clsProductSno();
                                    clsSerial.serialNo = drSerial["SerialNo"].ToString();
                                    clsSerial.refRowSno = clsDetail.sNo;
                                    lstClsProductSno.Add(clsSerial);
                                }
                            }
                        }
                        //Add TaxDetail
                        if (dtTax.Rows.Count > 0)
                        {
                            using (DataTable dtTaxTemp = new DataView(dtTax, "Field2='" + clsDetail.siDetailId + "'", "", DataViewRowState.CurrentRows).ToTable())
                            {
                                foreach (DataRow drTax in dtTaxTemp.Rows)
                                {
                                    clsInvTaxDetail clsTax = new clsInvTaxDetail();
                                    clsTax.taxId = drTax["Tax_Id"].ToString();
                                    clsTax.taxName = drTax["taxName"].ToString();
                                    clsTax.taxPer = drTax["Tax_Per"].ToString();
                                    clsTax.refRowSno = clsDetail.sNo;
                                    lstClsInvTaxDetail.Add(clsTax);
                                }
                            }
                        }
                    }
                    clsSiHeader.lstProductDetail = lstClsSInvDetail;
                    clsSiHeader.lstTaxDetail = lstClsInvTaxDetail;
                    clsSiHeader.lstProductSerialDetail = lstClsProductSno;
                    dtSerial = null;
                    dtTax = null;
                }
                //add expenses detail
                List<clsSInvExpenses> lstClsExpenses = new List<clsSInvExpenses> { };
                decimal expensesAmount = 0;
                using (DataTable dtExpenses = new Inv_ShipExpDetail(HttpContext.Current.Session["DBConnection"].ToString()).Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strInvoiceId, "SI"))
                {
                    foreach (DataRow drExpenses in dtExpenses.Rows)
                    {
                        clsSInvExpenses clsExpenses = new clsSInvExpenses();
                        clsExpenses.expensesName = drExpenses["Exp_Name"].ToString();
                        clsExpenses.expenesId = drExpenses["Expense_Id"].ToString();
                        clsExpenses.currencyName = drExpenses["CurrencyName"].ToString();
                        clsExpenses.currencyId = drExpenses["ExpCurrencyId"].ToString();
                        clsExpenses.foreignAmount = SystemParameter.GetAmountWithDecimal(drExpenses["FCExpAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsExpenses.exchangeRate = SystemParameter.GetAmountWithDecimal(drExpenses["ExpExchangeRate"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsExpenses.localAmount = SystemParameter.GetAmountWithDecimal(drExpenses["Exp_Charges"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        expensesAmount += Convert.ToDecimal(clsExpenses.localAmount);
                        lstClsExpenses.Add(clsExpenses);
                    }
                }
                clsSiHeader.lstExpensesDetail = lstClsExpenses;
                // getting expenses amount total from expenses and setting it to header table
                clsSiHeader.expensesAmount = SystemParameter.GetAmountWithDecimal(expensesAmount.ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.netAmountWithExp = SystemParameter.GetAmountWithDecimal((expensesAmount + Convert.ToDecimal(clsSiHeader.grossAmount)).ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //addd payment detail
                List<clsPayTrns> lstClsPaymentDetail = new List<clsPayTrns> { };
                using (DataTable dtPayment = new Inv_PaymentTrans(HttpContext.Current.Session["DBConnection"].ToString()).GetPaymentTransTrue(StrCompId.ToString(), "SI", strInvoiceId))
                {
                    foreach (DataRow drPayment in dtPayment.Rows)
                    {
                        clsPayTrns clsPayment = new clsPayTrns();
                        clsPayment.paymentModeId = drPayment["PaymentModeId"].ToString();
                        clsPayment.paymentModeName = drPayment["PaymentName"].ToString();
                        clsPayment.accountId = drPayment["AccountNo"].ToString();
                        clsPayment.accountName = drPayment["AccountName"].ToString();
                        clsPayment.cardName = drPayment["CardName"].ToString();
                        clsPayment.cardNo = drPayment["CardNo"].ToString();
                        clsPayment.bankId = drPayment["BankId"].ToString();
                        clsPayment.chequeNo = drPayment["ChequeNo"].ToString();
                        clsPayment.chequeDate = drPayment["ChequeDate"].ToString();
                        clsPayment.localAmount = SystemParameter.GetAmountWithDecimal(drPayment["Pay_Charges"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsPayment.currencyId = drPayment["PayCurrencyID"].ToString();
                        clsPayment.exchangeRate = drPayment["PayExchangeRate"].ToString();
                        clsPayment.foreignAmount = SystemParameter.GetAmountWithDecimal(drPayment["FCPayAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsPayment.creditNoteVoucherId = drPayment["Field3"].ToString();
                        clsPayment.paymentType = drPayment["PaymentType"].ToString();
                        lstClsPaymentDetail.Add(clsPayment);
                    }
                }
                clsSiHeader.lstPaymentDetail = lstClsPaymentDetail;
            }
            return JsonConvert.SerializeObject(clsSiHeader);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string IbtnPrint_Command(string transId, string isApproved)
    {
        if (isApproved == "true")
        {
            string st = getStatus(transId);
            if (st == "Approved")
            {
                return "true";
            }
            else
            {
                return "Cannot Print, Invoice not Approved";
            }
        }
        else
        {
            return "true";
        }
    }
    public static string getStatus(string transId)
    {
        DataAccessClass da = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string result = "";
        result = da.get_SingleValue("select field4 from Inv_SalesInvoiceHeader where isactive='true' and Trans_Id='" + transId + "' and Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' and Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");
        return result;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string IbtnDeliveryPrint_Command(string transId)
    {
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = daclass.return_DataTable("select trans_id,SalesOrder_Id from Inv_SalesDeliveryVoucher_Header where field2='" + transId + "' and company_id='" + HttpContext.Current.Session["CompId"].ToString() + "' and brand_id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_id='" + HttpContext.Current.Session["LocId"].ToString() + "'");
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                return "true";
            }
        }
        else
        {
            if (dt.Rows[0]["SalesOrder_Id"].ToString() != "")
            {
                return "true";
            }
            else
            {
                return "Delivery Record Not Found !";
            }
        }
        return "";
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    protected void ShowPopup(object sender, EventArgs e)
    {
        string title = "Cancel Invoice";
        string body = "";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "');", true);
    }
    protected void SetCustomerTextBox(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.Text == "Supplier_Id")
        {
            txtValue.Visible = false;
            txtCustValue.Visible = true;
            txtValueDate.Visible = false;
        }
        else if (ddlFieldName.Text == "Invoice_Date")
        {
            txtValue.Visible = false;
            txtCustValue.Visible = false;
            txtValueDate.Visible = true;
        }
        else
        {
            txtValue.Visible = true;
            txtCustValue.Visible = false;
            txtValueDate.Visible = false;
        }
        txtValue.Text = "";
        txtCustValue.Text = "";
        txtValueDate.Text = "";
    }

    protected void SetCustomerTextBoxCancel(object sender, EventArgs e)
    {
        //I1.Attributes.Add("Class", "fa fa-minus");
        //Div1.Attributes.Add("Class", "box box-primary");
        //if (ddlFieldNameCancel.Text == "Supplier_Id")
        //{
        //    txtValueCancel.Visible = false;
        //    txtCustValueCancel.Visible = true;
        //    txtValueDateCancel.Visible = false;
        //}
        //else if (ddlFieldNameCancel.Text == "Invoice_Date")
        //{
        //    txtValueCancel.Visible = false;
        //    txtCustValueCancel.Visible = false;
        //    txtValueDateCancel.Visible = true;
        //}
        //else
        //{
        //    txtValueCancel.Visible = true;
        //    txtCustValueCancel.Visible = false;
        //    txtValueDateCancel.Visible = false;
        //}
        //txtValueCancel.Text = "";
        //txtCustValueCancel.Text = "";
        //txtValueDateCancel.Text = "";
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtCustValue.Text = "";
        txtCustValue.Visible = false;
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] FillGrid(string ddlLocation, string ddlPosted, string ddlFieldName, string ddlOption, string txtValue, string PageSize)
    {
        UserMaster ObjUser = new UserMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string[] result = new string[2];
        Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());

        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;

        if (ddlOption != "0")
        {
            string condition = string.Empty;
            if (ddlFieldName == "Supplier_Id")
            {
                string retval = (txtValue.Split('/'))[txtValue.Split('/').Length - 1];

                if (ddlOption == "1")
                {
                    condition = "convert(" + ddlFieldName + ",System.String)='" + retval + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption == "2")
                {
                    condition = "convert(" + ddlFieldName + ",System.String) like '%" + retval + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName + ",System.String) Like '" + retval + "%'";
                    SearchType = "Like";
                }
            }
            else
            {
                if (ddlFieldName == "Invoice_Date")
                {

                    if (ddlOption == "1")
                    {
                        condition = "convert(" + ddlFieldName + ",System.String)='" + txtValue.Trim() + "'";
                        SearchType = "Equal";
                    }
                    else if (ddlOption == "2")
                    {
                        //condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                        condition = "convert(" + ddlFieldName + ",System.String) like '%" + txtValue.Trim() + "%'";
                        SearchType = "Equal";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName + ",System.String) Like '" + txtValue.Trim() + "%'";
                        SearchType = "Like";
                    }
                }
                else
                {
                    if (txtValue != "")
                    {
                        if (ddlOption == "Equal")
                        {
                            condition = "convert(" + ddlFieldName + ",System.String)='" + txtValue.Trim() + "'";
                            SearchType = "Equal";
                        }
                        else if (ddlOption == "Contains")
                        {
                            condition = "" + ddlFieldName + " like '%" + txtValue.Trim() + "%'";
                            SearchType = "Contains";
                        }
                        else
                        {
                            condition = "convert(" + ddlFieldName + ",System.String) Like '" + txtValue.Trim() + "%'";
                            SearchType = "Like";
                        }
                    }
                }
            }

            string PostType = string.Empty;
            string myval = string.Empty;
            bool IsAll = true;
            if (ddlPosted == "Posted")
            {
                myval = "1";
                IsAll = false;
            }
            if (ddlPosted == "UnPosted")
            {
                myval = "0";
                IsAll = false;
            }

            if (IsAll == false)
                PostType = " Post = " + myval + "";
            else
                PostType = DBNull.Value.ToString();

            SearchField = ddlFieldName.ToString();
            SearchValue = txtValue.Trim();
            if (SearchField == "Trans_Id")
                SearchField = "Inv_SalesInvoiceHeader.Trans_Id";

           // PageSize = HttpContext.Current.Session["GridSize"].ToString();
            // string DPageSize = ddlPageSize.SelectedValue.ToString();
            string BatchNo = "1";
            if (txtValue != "")
            {
                //DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation);
                //DataTable dt = objSInvHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation);

                DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation, PostType, true.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);
                DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation, PostType, false.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);

                if (dt.Rows.Count > 0)
                {
                    string JSONresult = JsonConvert.SerializeObject(dt);
                    result[0] = JSONresult;
                    return result;
                }
                return result;
            }
            else
            {
                DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation, PostType, true.ToString(), BatchNo, PageSize, "", "", "");
                DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLocation, PostType, false.ToString(), BatchNo, PageSize, "", "", "");
                if (dt.Rows.Count > 0)
                {
                    string JSONresult = JsonConvert.SerializeObject(dt);
                    result[0] = JSONresult;
                    return result;
                }
                return result;
            }


          
            
           

        }
        return result;
    }


    private void FillGrid()
    {
        //string PostStatus = string.Empty;
        string PostType = string.Empty;
        string myval = string.Empty;
        bool IsAll = true;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            //PostStatus = " Post='True'";
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            //PostStatus = " Post='False'";
            myval = "0";
            IsAll = false;
        }

        if (IsAll == false)
            PostType = " Post = " + myval + " and IsCancel='False'";
        else
            //PostType = DBNull.Value.ToString();
            PostType = " IsCancel='False'";

        string PageSize = Session["GridSize"].ToString();
       // string DPageSize = ddlPageSize.SelectedValue.ToString();
        //if (DPageSize == "")
        //    DPageSize = "0";
        //if (int.Parse(DPageSize) > int.Parse(PageSize))
        //    PageSize = DPageSize;

        string BatchNo = "1";
        DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrue(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue);
        DataTable dt = objSInvHeader.GetSInvHeaderAllTrue(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue);

        Session["dtFilter_Sale_inv_mstr"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

        dt.Dispose();
        object sumObject;
        sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


       // lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), Session["LocCurrencyId"].ToString());

    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //lblUcSettingsTitle.Text = "Set Columns Visibility";
       // ucCtlSetting.getGrdColumnsSettings("CustomerMaster", GvSalesInvoice, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
 
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldName.SelectedItem.Value == "Supplier_Id")
            {
                string retval = (txtCustValue.Text.Split('/'))[txtCustValue.Text.Split('/').Length - 1];

                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + retval + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + retval + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + retval + "%'";
                    SearchType = "Like";
                }
            }
            else
            {
                if (ddlFieldName.SelectedItem.Value == "Invoice_Date")
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

                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                        SearchType = "Equal";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        //condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        SearchType = "Equal";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                        SearchType = "Like";
                    }
                }
                else
                {
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                        SearchType = "Equal";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                        SearchType = "Contains";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                        SearchType = "Like";
                    }
                }
            }

            string PostType = string.Empty;
            string myval = string.Empty;
            bool IsAll = true;
            if (ddlPosted.SelectedItem.Value == "Posted")
            {
                myval = "1";
                IsAll = false;
            }
            if (ddlPosted.SelectedItem.Value == "UnPosted")
            {
                myval = "0";
                IsAll = false;
            }

            if (IsAll == false)
                PostType = " Post = " + myval + "";
            else
                PostType = DBNull.Value.ToString();

            SearchField = ddlFieldName.SelectedValue.ToString();
            SearchValue = txtValue.Text.Trim();
            if (SearchField == "Trans_Id")
                SearchField = "Inv_SalesInvoiceHeader.Trans_Id";

            string PageSize = Session["GridSize"].ToString();
           // string DPageSize = ddlPageSize.SelectedValue.ToString();
        

            DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrue(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString());
            DataTable dt = objSInvHeader.GetSInvHeaderAllTrue(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString());


            //DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, true.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);
            //DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, false.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);

            ////DataTable dtAdd = (DataTable)Session["dtSInvoice"];
            //DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);

           // GvSalesInvoice.DataSource = dt;
            //GvSalesInvoice.DataBind();

            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";
            //objPageCmn.FillData((object)GvSalesInvoice, view.ToTable(), "", "");
            //Session["dtFilter_Sale_inv_mstr"] = view.ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            // //AllPageCode();

            object sumObject;
            sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


            lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            int _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());
            //generatePager(_TotalRowCount, int.Parse(PageSize), 1);
        }


        //AllPageCode();

        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtCustValue.Text != "")
            txtCustValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        //DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        //try
        //{
        //    dtCustomer = new DataView(dtCustomer, "Field6='True'  and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();

        //}
        //catch
        //{

        //}

        //DataTable dtMain = new DataTable();
        //dtMain = dtCustomer.Copy();


        //string filtertext = "Filtertext like '%" + prefixText + "%'";
        //DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        DataTable dtCon = objcustomer.GetCustomerAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
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

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] IbtnDelete_Command(string transId, string isApproved)
    {
        Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string[] result = new string[2];
        DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), transId);
        if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
        {
            result[0]= "Cannot Delete Posted Invoice";
            return result;
        }
        if (isApproved == "true")
        {
            string st = getStatus(transId);
            if (st == "Approved")
            {
                result[0]="Cannot Delete Approved Invoice";
                return result;
            }
        }
        int b = 0;
        b = objSInvHeader.DeleteSInvHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), transId, "false", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        string strSql = string.Empty;
        strSql = "delete from Inv_StockBatchMaster where TransType='SI' and TransTypeId=" + transId + "";
        objDa.execute_Command(strSql);
        if (b != 0)
        {
            result[0] = "Record Deleted Successfully !";
            return result;
        }
        else
        {
            result[0] = "Record  Not Deleted";
            return result;
        }
    }
    protected string GetDateTime(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString("dd-MMM-yyyy hh:mm tt");
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
 
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                if (st == "Approved")
                {
                  //  InvoicePrint(e.CommandArgument.ToString());

                }
                else
                {
                    DisplayMessage("Cannot Print, Invoice not Approved");
                    return;
                }
            }
            else
            {
                //InvoicePrint(e.CommandArgument.ToString());
            }
        }
    }
    protected void IbtnDeliveryPrint_Command(object sender, CommandEventArgs e)
    {
        //code created by jitednra on 12-02-2016 for deleievery printing

        DataTable dt = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //if created by invoice
        DataTable dtTemp = new DataView(dt, "Field2=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dtTemp.Rows.Count > 0)
        {
            PrintDeliveryVoucher(dtTemp.Rows[0]["Trans_Id"].ToString());
        }
        else
        {
            //if created by order 

            DataTable dtdetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), Session["FinanceYearId"].ToString());

            dtdetail = dtdetail.DefaultView.ToTable("SIFromTransNo");
            dtTemp = new DataView(dt, "SalesOrder_Id=" + dtdetail.Rows[0]["SIFromTransNo"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count > 0)
            {
                PrintDeliveryVoucher(dtTemp.Rows[0]["Trans_Id"].ToString());
            }
            else
            {
                DisplayMessage("Delivery Record Not Found !");
                return;
            }
        }
    }
    public void PrintDeliveryVoucher(string VoucherId)
    {
        string strCmd = string.Format("window.open('../Sales_Report/DeliveryVoucherReport.aspx?Id=" + VoucherId + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e) { 
}
   
    public void GetCreditInfo()
    {
        //bool IsCredit = false;

        //here we will show credit terms and condition .
        int otherAcId = 0;
        try
        {
            otherAcId = objAcAccountMaster.GetCustomerAccountByCurrency(txtCustomer.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue);
            //IsCredit = Convert.ToBoolean(ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[1].ToString()).Rows[0]["Field41"].ToString());
        }
        catch
        {

        }


        if (otherAcId > 0)
        {
            trCreditInfo.Visible = true;

            DataTable dtCreditParameter = objCustomerCreditParam.GetRecord_By_CustomerId(txtCustomer.Text.Split('/')[1].ToString());


            dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtCreditParameter.Rows.Count > 0)
            {
                lblCreditLimitValue.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
                try
                {
                    lblCurrencyCreditLimit.Text = objCurrency.GetCurrencyMasterById(dtCreditParameter.Rows[0]["Credit_Limit_Currency"].ToString().Trim()).Rows[0]["Currency_Name"].ToString();
                }
                catch
                {
                }

                //get current balance
                try
                {
                    string _strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    string _strOtherAcId = objAcAccountMaster.GetCustomerAccountByCurrency(dtCreditParameter.Rows[0]["Customer_id"].ToString(), ddlCurrency.SelectedValue).ToString();
                    string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "', '0','" + _strAccountId + "','" + _strOtherAcId + "','3','" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
                   // lblCurrentBalance.Text = SystemParameter.GetAmountWithDecimal(objDa.get_SingleValue(sql), Session["LoginLocDecimalCount"].ToString());
                }
                catch (Exception ex)
                {
                   // lblCurrentBalance.Text = "";
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
                //lblCurrentBalance.Text = "";
            }
        }
        else
        {
            trCreditInfo.Visible = false;
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
        if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
        {
            DisplayMessage("Sales Invoice has posted,can not be Cancel");
            //AllPageCode();
            return;
        }

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                string st = GetStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                if (st == "Approved")
                {
                    DisplayMessage("Sales Invoice has Approved, cannot be Cancel");
                    //AllPageCode();
                    return;

                }

            }

        }//End Approval Code

        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objSInvHeader.DeleteSInvHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, "false", StrUserId, DateTime.Now.ToString());

        string strSql = string.Empty;

        strSql = "delete from Inv_StockBatchMaster where TransType='SI' and TransTypeId=" + e.CommandArgument.ToString() + "";
        objDa.execute_Command(strSql);
        if (b != 0)
        {
            DisplayMessage("Record Deleted Successfully !");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
        //AllPageCode();
    }
    public void Reset()
    {
        ddlTransType.Enabled = true;
        Session["Expenses_Tax_Sales_Invoice"] = null;
        Session["JobCardId"] = null;
        txtSInvNo.Text = objSOrderHeader.GetAutoID(StrCompId, StrBrandId, StrLocationId);
        txtSInvDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtSInvNo.ReadOnly = false;
        txtCustomer.Text = "";
       // ddlOrderType.SelectedValue = "--Select--";
        RdoSo.Checked = false;
        RdoWithOutSo.Checked = false;
        PnlProductSearching.Visible = false;
        FillCurrency();
        hdnTransType.Value = "";
        if (Lbl_Tab_New.Text == "View")
        {
          //  btnSInvSave.Enabled = false;
          //  btnPost.Enabled = false;
          //  BtnReset.Enabled = false;
        }
        else
        {
            //btnSInvSave.Enabled = true;
           // btnPost.Enabled = true;
           // BtnReset.Enabled = true;
        }
        try
        {
            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
                if (strCurrencyId != "0" && strCurrencyId != "")
                {
                    ddlCurrency.SelectedValue = strCurrencyId;
                    ddlExpCurrency.SelectedValue = strCurrencyId;
                }
            }
            catch
            {

            }
        }
        catch
        {
        }
        txtTotalExpensesAmount.Text = "0";
        txtNetAmountwithexpenses.Text = SetDecimal("0");
        txtExchangeRate.Text = "1";

       // txtAccountNo.Text = "";
       // txtInvoiceCosting.Text = "";
       // txtShift.Text = "";
       // txtTender.Text = "";
        txtTotalQuantity.Text = "0";
        txtAmount.Text = "0";
        txtTaxP.Text = "0";
        txtTaxV.Text = Convert_Into_DF("0");
        txtNetAmount.Text = "0";
        txtDiscountP.Text = "0";
        txtDiscountV.Text = "0";
        txtGrandTotal.Text = "0";
        txtRemark.Text = "";
        txtCondition1.Content = "";
        txtShipCustomerName.Text = "";
        txtShipingAddress.Text = "";
        txtInvoiceTo.Text = "";
        chkPost.Checked = false;
        //gvProduct.DataSource = null;
       // gvProduct.DataBind();
        txtInvoiecRefno.Text = "";
        txtOrderId.Text = "";
        ddlMerchant.SelectedIndex = 0;
        //FillGrid();
        hdnSalesOrderId.Value = "0";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        //txtValueBin.Text = "";
        //txtCustValueBin.Text = "";
        txtCustValue.Text = "";
      //  txtValueBinDate.Text = "";
        txtValueDate.Text = "";
        txtCustValue.Visible = false;
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        //txtValueBin.Visible = true;
        //txtValueBinDate.Visible = false;
       // txtCustValueBin.Visible = false;
        //lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
       // txtSInvNo.Text = GetDocumentNumber();
        ViewState["dtSerial"] = null;
        ViewState["Tempdt"] = null;
        ViewState["dtDelete"] = null;
        ViewState["dtFinal"] = null;
        ViewState["ExpdtSales"] = null;
        ViewState["PayementDt"] = null;
        btnAddNewProduct.Visible = false;
        txtCustomer.ReadOnly = false;
        RdoWithOutSo.Enabled = true;
        RdoSo.Enabled = true;
        //GridExpenses.DataSource = null;
      //  GridExpenses.DataBind();
      //  gvPayment.DataSource = null;
       // gvPayment.DataBind();
       // btnPaymentReset_Click(null, null);
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";
        txtFCExpAmount.Text = "0";
        txtExpExchangeRate.Text = "0";
        txtExpCharges.Text = "";
        rbtnFormView.Visible = false;
        rbtnAdvancesearchView.Visible = false;
        btnAddNewProduct.Visible = false;
       /// btnAddProductScreen.Visible = false;
        //btnAddtoList.Visible = false;
        Session["DtSearchProduct"] = null;
        ViewState["ApprovalStatus"] = null;
        ViewState["dtProductSearch"] = null;
        ViewState["Dtproduct"] = null;
        ViewState["dtPo"] = null;
        //.DataSource = null;
       // gvSerachGrid.DataBind();
        //ddlPaymentMode.SelectedIndex = 0;
        txtCustomer.Enabled = true;
       // setSymbol();
        //AllPageCode();
        ViewState["DtTax"] = null;
        ViewState["dtTaxHeader"] = null;
        LoadStores();
        txtPriceafterdiscountheader.Text = "0";
        Application.UnLock();
        Session["DtAdvancePayment"] = null;
        ///Filladvancepaymentgrid((DataTable)Session["DtAdvancePayment"]);
        ///fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
        txtCondition1.Content = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();
        trCreditInfo.Visible = false;
        Session["Temp_Product_Tax_SINV"] = null;
        HttpContext.Current.Session["SI_UnAdjustedCreditNote"] = null;
        strSiCustomerId = 0;
        strSiCurrencyId = 0;
        txtContactPerson.Text = "";
        hdnContactId.Value = "";
        Session["contactId"] = null;
    }
    public void LoadStores()
    {
        //DataTable dt = new DataTable();
        //if (ViewState["dtTaxHeader"] != null)
        //{
        //    dt = new DataTable();
        //    dt = (DataTable)ViewState["dtTaxHeader"];
        //    if (dt.Rows.Count > 0)
        //    {
        //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //        objPageCmn.FillData((object)gridView, dt, "", "");
        //    }
        //    else
        //    {
        //        DataTable contacts = new DataTable();
        //        contacts.Columns.Add("Tax_Id", typeof(int));
        //        contacts.Columns.Add("TaxName", typeof(string));
        //        contacts.Columns.Add("Tax_Per", typeof(double));
        //        contacts.Columns.Add("Tax_Value", typeof(double));
        //        contacts.Rows.Add(contacts.NewRow());
        //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //        objPageCmn.FillData((object)gridView, contacts, "", "");
        //        int TotalColumns = gridView.Rows[0].Cells.Count;
        //        gridView.Rows[0].Cells.Clear();
        //        gridView.Rows[0].Cells.Add(new TableCell());
        //        gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
        //        gridView.Rows[0].Visible = false;
        //    }

        //}
        //else
        //{
        //    DataTable contacts = new DataTable();
        //    contacts.Columns.Add("Tax_Id", typeof(int));
        //    contacts.Columns.Add("TaxName", typeof(string));
        //    contacts.Columns.Add("Tax_Per", typeof(double));
        //    contacts.Columns.Add("Tax_Value", typeof(double));
        //    contacts.Rows.Add(contacts.NewRow());
        //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    objPageCmn.FillData((object)gridView, contacts, "", "");
        //    int TotalColumns = gridView.Rows[0].Cells.Count;
        //    gridView.Rows[0].Cells.Clear();
        //    gridView.Rows[0].Cells.Add(new TableCell());
        //    gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
        //    gridView.Rows[0].Visible = false;
        //}
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        //AllPageCode();
        btnEdit_Command(sender, e);
        LoadStores();
    }
    public string GetStatus(int TransID, ref SqlTransaction trns)
    {
        string ApprovalStatus = string.Empty;
        DataTable DtSInv = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, TransID.ToString(), ref trns);
        if (DtSInv.Rows.Count > 0)
        {
            ApprovalStatus = DtSInv.Rows[0]["Field4"].ToString();
        }
        else
        {
            ApprovalStatus = "";
        }
        return ApprovalStatus;
    }
    public string GetStatus(int TransID)
    {
        string ApprovalStatus = string.Empty;
        DataTable DtSInv = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, TransID.ToString());
        if (DtSInv.Rows.Count > 0)
        {
            ApprovalStatus = DtSInv.Rows[0]["Field4"].ToString();
        }
        else
        {
            ApprovalStatus = "";
        }
        return ApprovalStatus;
    }


    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string imgBtnRestore_Click(string transId)
    {
        int b = 0;
        b = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString()).DeleteSInvHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), transId, true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            return "Record Activate";
        }
        else
        {
            return "Something Went Wrong Please Try Again Later";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getProductSerialFromStock(string strProductId)
    {
        try
        {
            using (DataTable dtStock = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetStockBatchMasterAll_By_ProductIdForSalesInvoice(strProductId))
            {
                if (dtStock.Rows.Count > 0)
                {
                    List<clsProductSno> lstClsSerail = new List<clsProductSno> { };
                    foreach (DataRow dr in dtStock.Rows)
                    {
                        clsProductSno clsSerial = new clsProductSno();
                        clsSerial.serialNo = dr["SerialNo"].ToString();
                        lstClsSerail.Add(clsSerial);
                    }
                    return JsonConvert.SerializeObject(lstClsSerail);
                }
                else
                {
                    return "No Record Found";
                }
            }
        }
        catch (Exception ex)
        {
            return "No Record Found";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string fillTblPendingOrder()
    {
        Inv_SalesOrderHeader objSOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dtQuotation = objSOrderHeader.PendingOrderList(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()))
            {
                return JsonConvert.SerializeObject(dtQuotation);
            }
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getTblProductDataFromAvdSearch(string decimalCount)
    {
        try
        {
            DataTable dtProducts = HttpContext.Current.Session["DtSearchProduct"] as DataTable;
            List<clsSInvDetail> lstProductDetails = new List<clsSInvDetail> { };
            int counter = 0;
            foreach (DataRow drDetail in dtProducts.Rows)
            {
                counter++;
                clsSInvDetail clsDetail = new clsSInvDetail();
                clsDetail.sNo = counter.ToString();
                clsDetail.siDetailId = Convert.ToInt32(drDetail["Trans_id"].ToString());
                clsDetail.salesOrderId = "";
                clsDetail.salesOrderNo = drDetail["SalesOrderNo"].ToString();
                clsDetail.isDeliveryVoucherAllow = false; //need to check
                clsDetail.productCode = drDetail["Product_Code"].ToString();
                clsDetail.productId = drDetail["Product_Id"].ToString();
                clsDetail.productName = drDetail["ProductName"].ToString();
                clsDetail.productDescription = drDetail["ProductDescription"].ToString();
                clsDetail.unitId = drDetail["UnitId"].ToString();
                clsDetail.unitName = drDetail["Unit_Name"].ToString();
                clsDetail.unitPrice = SystemParameter.GetAmountWithDecimal(drDetail["UnitPrice"].ToString(), decimalCount);
                clsDetail.freeQty = SystemParameter.GetAmountWithDecimal(drDetail["FreeQty"].ToString(), decimalCount);
                clsDetail.orderQty = SystemParameter.GetAmountWithDecimal(drDetail["OrderQty"].ToString(), decimalCount);
                clsDetail.soldQty = SystemParameter.GetAmountWithDecimal(drDetail["SoldQty"].ToString(), decimalCount);
                clsDetail.sysQty = SystemParameter.GetAmountWithDecimal(drDetail["SysQty"].ToString(), decimalCount);
                clsDetail.remainQty = "0";
                clsDetail.salesQty = SystemParameter.GetAmountWithDecimal(drDetail["Quantity"].ToString(), decimalCount);
                clsDetail.discountPer = SystemParameter.GetAmountWithDecimal(drDetail["DiscountP"].ToString(), decimalCount);
                clsDetail.discountValue = SystemParameter.GetAmountWithDecimal(drDetail["DiscountV"].ToString(), decimalCount);
                clsDetail.taxPer = SystemParameter.GetAmountWithDecimal(drDetail["TaxP"].ToString(), decimalCount);
                clsDetail.taxValue = SystemParameter.GetAmountWithDecimal(drDetail["TaxV"].ToString(), decimalCount);
                clsDetail.inventoryType = drDetail["MaintainStock"].ToString();
                lstProductDetails.Add(clsDetail);
            }
            return JsonConvert.SerializeObject(lstProductDetails);
        }
        catch
        {
            return null;
        }
        finally
        {
            HttpContext.Current.Session["DtSearchProduct"] = null;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string[] fillAdvancePayment_BYOrderId(string OrderId, string OrderNo)
    {
        DataAccessClass daClass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string[] data = new string[2];
        string header = "", detail = "";
        int counter = 0;
        detail = daClass.get_SingleValue("select Invoice_No from Inv_SalesInvoiceDetail where SIFromTransNo='" + OrderId + "' and Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' and Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and IsActive='true'");

        if (detail != "")
        {
            header = daClass.get_SingleValue("select COUNT(Trans_Id) from Inv_SalesInvoiceHeader where Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' and Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Post='true' and Trans_Id='" + detail + "' and IsActive='true'");
            if (header != "0")
            {
                counter = 1;
            }
        }
        if (counter == 0)
        {
            DataTable dtAdvancepayment = new DataTable();
            dtAdvancepayment.Columns.Add("OrderNo");
            dtAdvancepayment.Columns.Add("PaymentName");
            dtAdvancepayment.Columns.Add("AccountName");
            dtAdvancepayment.Columns.Add("Pay_Charges");
            DataTable dtAdvanceRecord = daClass.return_DataTable("Select Inv_PaymentTrn.Pay_Charges, Set_Payment_Mode_Master.Pay_Mod_Name as PaymentName, Ac_ChartOfAccount.AccountName From Inv_PaymentTrn left join Set_Payment_Mode_Master on Inv_PaymentTrn.PaymentModeId = Set_Payment_Mode_Master.Pay_Mode_Id inner join Ac_ChartOfAccount on Ac_ChartOfAccount.Trans_Id = Inv_PaymentTrn.AccountNo where Inv_PaymentTrn.Company_Id ='" + HttpContext.Current.Session["CompId"].ToString() + "'  and Inv_PaymentTrn.TypeTrans = 'SO' and Inv_PaymentTrn.IsActive = 'True' and  Inv_PaymentTrn.TransNo = '" + OrderId + "'");

            if (HttpContext.Current.Session["DtAdvancePayment"] == null)
            {
                if (dtAdvanceRecord.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAdvanceRecord.Rows.Count; i++)
                    {
                        DataRow dr = dtAdvancepayment.NewRow();
                        dr[0] = OrderNo;
                        dr[1] = dtAdvanceRecord.Rows[i]["PaymentName"].ToString();
                        dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                        dr[3] = dtAdvanceRecord.Rows[i]["Pay_Charges"].ToString();
                        dtAdvancepayment.Rows.Add(dr);
                    }
                }
            }
            else
            {
                dtAdvancepayment = (DataTable)HttpContext.Current.Session["DtAdvancePayment"];
                if (dtAdvanceRecord.Rows.Count > 0)
                {
                    DataTable dtTemp = new DataTable();
                    try
                    {
                        dtTemp = new DataView(dtAdvancepayment, "OrderNo='" + OrderNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtTemp.Rows.Count == 0)
                    {
                        for (int i = 0; i < dtAdvanceRecord.Rows.Count; i++)
                        {
                            DataRow dr = dtAdvancepayment.NewRow();
                            dr[0] = OrderNo;
                            dr[1] = dtAdvanceRecord.Rows[i]["PaymentName"].ToString();
                            dr[2] = dtAdvanceRecord.Rows[i]["AccountName"].ToString();
                            dr[3] = dtAdvanceRecord.Rows[i]["Pay_Charges"].ToString();
                            dtAdvancepayment.Rows.Add(dr);
                        }
                    }
                }
            }
            HttpContext.Current.Session["DtAdvancePayment"] = dtAdvancepayment;

            data[0] = "true";
            data[1] = JsonConvert.SerializeObject(dtAdvancepayment);
            return data;
        }
        data[0] = "false";
        data[1] = "";
        return data;
    }

    protected void btnNewaddress_Click(object sender, EventArgs e)
    {
        if (Session["ContactID"].ToString() == "" || Session["ContactID"].ToString() == "0")
        {
            DisplayMessage("Please Select a Customer To Add New Address");
            return;
        }
        addaddress.Reset();

        Country_Currency objCountryCurrency = new Country_Currency(HttpContext.Current.Session["DBConnection"].ToString());

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
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
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
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        string[] strs = new string[1];
        try
        {
            ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = Email.GetEmailIdPreFixText(prefixText);
            if (dt.Rows.Count > 0)
            {
                string[] str = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Email_Id"].ToString();
                }
                return str;
            }
           
            return strs;
        }
        catch(Exception ex)
        {
            return strs;
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
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;
        }
    }

    #region Bin Section
    protected void btnUnPost_Click(object sender, EventArgs e)
    {
        FillGridBin();
        //AllPageCode();
    }
    protected void GvSalesInvoiceBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesInvoiceBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoiceBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvSalesInvoiceBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesInvoiceBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesInvoiceBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void GvSalesInvoiceBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["dtInactive"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoiceBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objSInvHeader.GetSInvHeaderAllFalse(StrCompId, StrBrandId, StrLocationId);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoiceBin, dt, "", "");
        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {

            if (ddlFieldNameBin.SelectedItem.Value == "Invoice_Date")
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

            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesInvoiceBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            //AllPageCode();
        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtCustValueBin.Text != "")
            txtCustValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objSOrderHeader.GetSOHeaderAllFalse(StrCompId, StrBrandId, StrLocationId);

        if (GvSalesInvoiceBin.Rows.Count != 0)
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
                //FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvSalesInvoiceBin.Rows)
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
        //AllPageCode();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesInvoiceBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesInvoiceBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesInvoiceBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvSalesInvoiceBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvSalesInvoiceBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvSalesInvoiceBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)GvSalesInvoiceBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvSalesInvoiceBin.Rows[index].FindControl("chkSelect")).Checked)
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
        //FillGrid();
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");


        FillGridBin();
        txtValueBin.Text = "";

        txtCustValueBin.Text = "";

        txtCustValueBin.Visible = false;

        txtValueBinDate.Text = "";

        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;

        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        //AllPageCode();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

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
            for (int i = 0; i < GvSalesInvoiceBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvSalesInvoiceBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesInvoiceBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesInvoiceBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
        //AllPageCode();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objSInvHeader.DeleteSInvHeader(StrCompId, StrBrandId, StrLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            //FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvSalesInvoiceBin.Rows)
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
            //AllPageCode();
        }
    }
    protected void SetCustomerTextBoxBin(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.Text == "Supplier_Id")
        {
            txtValueBin.Visible = false;
            txtCustValueBin.Visible = true;
            txtValueBinDate.Visible = false;
        }
        else if (ddlFieldNameBin.Text == "Invoice_Date")
        {
            txtValueBin.Visible = false;
            txtCustValueBin.Visible = false;
            txtValueBinDate.Visible = true;
        }
        else
        {
            txtValueBin.Visible = true;
            txtCustValueBin.Visible = false;
            txtValueBinDate.Visible = false;
        }
        txtValueBin.Text = "";
        txtCustValueBin.Text = "";
        txtValueBinDate.Text = "";
    }

    #endregion

    #region excel Upload
    protected void btnSaveExcelInvoice_Click(object sender, EventArgs e)
    {
        if (hdnInvalidExcelRecords.Value != "0" || Session["ExcelImportInvoiceList"] == null)
        {
            return;
        }
        List<Inv_SalesInvoiceHeader.clsSInvHeader> lstInvoiceList = (List<Inv_SalesInvoiceHeader.clsSInvHeader>)Session["ExcelImportInvoiceList"];
        if (lstInvoiceList.Count == 0)
        {
            return;
        }
        Set_AddressMaster objAddressMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        Ems_ContactMaster objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        Set_DocNumber ObjDocumentNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ContactNoMaster objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        CountryMaster objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        Set_AddressChild objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ES_EmailMasterDetail objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        Ems_Contact_Group objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        Ems_ContactCompanyBrand ObjCompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());

        Set_CustomerMaster.clsCustomerMaster clsCustomer = null;
        string strContactDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        string strInvoiceDocNo = ViewState["DocNo"].ToString();

        string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string billingAddressId = "";
        string shippingAddressId = "";

        string creditPayModeId = "0";
        using (DataTable _dt = ObjPaymentMaster.GetPaymentModeMasterByPaymentModeName(Session["CompId"].ToString(), "credit", Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            creditPayModeId = _dt.Rows[0]["Pay_Mode_Id"].ToString();
        }

        string cashPayModeId = "0";
        using (DataTable _dt = ObjPaymentMaster.GetPaymentModeMasterByPaymentModeName(Session["CompId"].ToString(), "cash", Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            cashPayModeId = _dt.Rows[0]["Pay_Mode_Id"].ToString();
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstInvoiceList)
            {
                shippingAddressId = billingAddressId = "0";
                int contactId = 0;
                clsCustomer = _cls.clsCustomerDetail;
                List<Set_AddressMaster.clsAddressMaster> lstAddress = _cls.clsCustomerDetail.lst_address;

                if (string.IsNullOrEmpty(_cls.customerId) || _cls.customerId == "0")
                {
                    //check in database same customer exist or not
                    string strCustomerId = ObjCustmer.getCustomerIdByNameAddressStateAndCity(Session["compId"].ToString(), clsCustomer.name, lstAddress[0].address, lstAddress[0].state_id, lstAddress[0].city_id, ref trns);
                    if (!string.IsNullOrEmpty(strCustomerId))
                    {
                        _cls.customerId = strCustomerId;
                        using (DataTable _dtAdd = objAddressChild.GetAddressListByAddTypeAndAddRefId("contact", _cls.customerId, ref trns))
                        {
                            foreach (DataRow _add in _dtAdd.Rows)
                            {
                                if (_add["StateId"].ToString() == lstAddress[0].state_id && _add["CityId"].ToString() == lstAddress[0].city_id && _add["address"].ToString() == lstAddress[0].address)
                                {
                                    lstAddress[0].id = _cls.billingAddressId = _add["Trans_id"].ToString();
                                }

                                if (lstAddress.Count > 1)
                                {
                                    if (_add["StateId"].ToString() == lstAddress[1].state_id && _add["CityId"].ToString() == lstAddress[1].city_id && _add["address"].ToString() == lstAddress[1].address)
                                    {
                                        lstAddress[1].id = _cls.shippingAddressId = _add["Trans_id"].ToString();
                                    }
                                }
                            }
                        }

                    }

                }

                if (string.IsNullOrEmpty(_cls.customerId) || _cls.customerId == "0")
                {
                    //save contact
                    contactId = objContact.InsertContactMaster(strContactDocNo, clsCustomer.name, clsCustomer.name_l, "", "0", "0", "0", "0", false.ToString(), false.ToString(), clsCustomer.type, false.ToString(), clsCustomer.email, clsCustomer.mobile == null ? "" : clsCustomer.mobile, "0", clsCustomer.country_id, Session["LocCurrencyId"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), clsCustomer.notes, "0", ref trns);
                    ObjContactMaster.UpdateContactMaster(contactId.ToString(), strContactDocNo + contactId.ToString(), ref trns);

                    //update contact no master table
                    if (!string.IsNullOrEmpty(clsCustomer.mobile))
                    {
                        string country_code = objCountryMaster.GetCountryMasterById(clsCustomer.country_id, ref trns).Rows[0]["Country_code"].ToString();
                        objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", contactId.ToString(), "Mobile", country_code, clsCustomer.mobile, string.Empty, "True", Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns);
                    }

                    //update email master table
                    if (!string.IsNullOrEmpty(clsCustomer.email))
                    {
                        int emailMasterId = 0;
                        using (DataTable _dt = objEmailHeader.Get_EmailMasterHeaderTrueByEmailId(clsCustomer.email))
                        {
                            if (_dt.Rows.Count > 0)
                            {
                                emailMasterId = int.Parse(_dt.Rows[0]["trans_id"].ToString());
                            }
                            else
                            {
                                emailMasterId = objEmailHeader.ES_EmailMasterHeader_Insert(clsCustomer.email, clsCustomer.country_id, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        objEmailDetail.ES_EmailMasterDetail_Insert(contactId.ToString(), "Contact", emailMasterId.ToString(), true.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //update address master and child table
                if (lstAddress != null && lstAddress.Count > 0)
                {
                    int addressCount = 0;
                    foreach (var clsAddress in lstAddress)
                    {
                        //save address master
                        if (clsAddress.id == "0")
                        {
                            int addressMasterId = objAddressMaster.InsertAddressMaster(clsAddress.category, clsAddress.name, clsAddress.address, "", "", "", clsAddress.country_id, clsAddress.state_id, clsAddress.city_id, clsAddress.pin_code, "", "", "", "", "", "", "", "", "0", "0", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            clsAddress.id = addressMasterId.ToString();
                            //save address child
                            objAddChild.InsertAddressChild(addressMasterId.ToString(), "Contact", contactId.ToString(), addressCount == 1 ? true.ToString() : false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True",
                               Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //save address child for customer
                            objAddChild.InsertAddressChild(addressMasterId.ToString(), "Customer", contactId.ToString(), addressCount == 1 ? true.ToString() : false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True",
                               Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        if (clsAddress.category == "1")
                        {
                            billingAddressId = shippingAddressId = clsAddress.id;
                        }
                        else
                        {
                            shippingAddressId = clsAddress.id;
                        }
                    }
                }

                if (string.IsNullOrEmpty(_cls.customerId) || _cls.customerId == "0")
                {
                    //update customer master table
                    ObjCustmer.InsertCustomerMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), contactId.ToString(), strReceiveVoucherAcc, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", false.ToString(), "", "", "", Session["LocCurrencyId"].ToString(), true.ToString(), "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), clsCustomer.gst_no == null ? "" : clsCustomer.gst_no, "", "0", "", ref trns);

                    //update contact group
                    objCG.InsertContactGroup(contactId.ToString(), "1", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), contactId.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);

                    //update company contact brand table
                    ObjCompanyBrand.InsertContactCompanyBrand(contactId.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);

                }
                else
                {
                    contactId = int.Parse(_cls.customerId);
                }



                //update sales invoice header
                _cls.exchangeRate = "1";
                _cls.expensesAmount = "0";
                _cls.siFromTransType = "D"; //direct
                _cls.accountNo = strReceiveVoucherAcc;
                _cls.billingAddressId = billingAddressId;
                _cls.shippingAddressId = shippingAddressId;
                _cls.customerId = contactId.ToString();
                _cls.shippingCustomerId = contactId.ToString();
                _cls.currencyId = Session["LocCurrencyId"].ToString();
                _cls.docNo = _cls.invoiceNo = strInvoiceDocNo;
                _cls.contactId = "0";
                _cls.transType = int.Parse(_cls.transType) >= 0 ? _cls.transType : "-1";
                _cls.condition4 = _cls.transType;
                _cls.remark = string.Empty;
                _cls.discountPer = _cls.discountPer == null ? "0" : _cls.discountPer;
                _cls.discountAmount = _cls.discountAmount == null ? "0" : _cls.discountAmount;
                _cls.condition1 = _cls.condition1 == null ? "" : _cls.condition1;
                _cls.invoiceCosting = "0";
                _cls.salesInvoiceId = objSInvHeader.InsertSInvHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _cls.invoiceNo, _cls.invoiceDate, "0", _cls.currencyId, _cls.siFromTransType, "0", _cls.salesPersonId, "0", _cls.remark, _cls.accountNo, _cls.invoiceCosting, "", "false", "0.00", _cls.grossAmount, _cls.totalQuantity, _cls.grossAmount, _cls.taxPer, _cls.taxAmount, _cls.grossAmount, _cls.discountPer, _cls.discountAmount, _cls.netAmount, _cls.customerId, _cls.invoiceRefNo, _cls.invoiceMerchantId, _cls.refOrderNumber, _cls.condition1, _cls.netAmountWithExp, "0", _cls.condition4, "", _cls.billingAddressId, _cls.shippingCustomerId, _cls.shippingAddressId, "Approved", _cls.exchangeRate, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), _cls.transType, _cls.contactId, ref trns);

                //update sales invoice no
                if (_cls.invoiceNo == strInvoiceDocNo)
                {
                    string sql = string.Empty;
                    int skipNo = 0;
                    if (Session["LocId"].ToString() == "7") //this is for trading location
                    {
                        sql = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and Invoice_No like '" + strInvoiceDocNo + "%'";
                        try
                        {
                            Int32.TryParse(ConfigurationManager.AppSettings["TradingInvoiceNoInterval"].ToString(), out skipNo);
                        }
                        catch
                        {

                        }
                    }
                    else if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is OPC Location
                    {
                        sql = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and Invoice_No Like '%" + ViewState["DocNo"].ToString() + "%'";
                    }
                    else
                    {
                        sql = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "'";
                    }
                    int strInvoiceCount = Int32.Parse(objDa.get_SingleValue(sql, ref trns));
                    strInvoiceCount += skipNo;

                    bool bFlag = false;
                    while (bFlag == false)
                    {
                        _cls.invoiceNo = _cls.docNo + (strInvoiceCount == 0 ? "1" : strInvoiceCount.ToString());
                        string sql1 = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and invoice_no='" + _cls.invoiceNo + "'";
                        string strInvCount = objDa.get_SingleValue(sql1, ref trns);
                        if (strInvCount == "0")
                        {
                            bFlag = true;
                        }
                        else
                        {
                            strInvoiceCount++;
                        }
                    }
                    objSInvHeader.Updatecode(_cls.salesInvoiceId.ToString(), _cls.invoiceNo, ref trns);
                }


                objSInvHeader.InsertSInvHeader_Extra(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _cls.salesInvoiceId.ToString(), _cls.shipmentid.ToString(), "", ref trns);



                //update sales invoice table
                foreach (Inv_SalesInvoiceHeader.clsSInvDetail clsSiDetail in _cls.lstProductDetail)
                {
                    try
                    {
                        clsSiDetail.avgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(StrCompId, StrBrandId, StrLocationId, HttpContext.Current.Session["FinanceYearId"].ToString(), clsSiDetail.productId).Rows[0]["Field2"].ToString();
                    }
                    catch
                    {
                        clsSiDetail.avgCost = "0";
                    }
                    using (DataTable _dt = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), clsSiDetail.productId, Session["FinanceYearId"].ToString()))
                    {
                        clsSiDetail.unitId = _dt.Rows[0]["UnitId"].ToString();
                        clsSiDetail.productDescription = _dt.Rows[0]["EProductName"].ToString();
                    }

                    clsSiDetail.siDetailId = objSInvDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _cls.salesInvoiceId.ToString(), clsSiDetail.sNo, "0", "0", _cls.siFromTransType, "0", clsSiDetail.productId, clsSiDetail.productDescription, clsSiDetail.unitId, clsSiDetail.unitPrice, "0", "0", clsSiDetail.salesQty, clsSiDetail.taxPer, clsSiDetail.taxValue, clsSiDetail.discountPer, clsSiDetail.discountValue == null ? "0" : clsSiDetail.discountValue, "False", false.ToString(), clsSiDetail.avgCost, "0", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //proceed with product serial nos
                    foreach (Inv_SalesInvoiceHeader.clsProductSno clsProductSerial in _cls.lstProductSerialDetail)
                    {
                        if (clsSiDetail.sNo == clsProductSerial.refRowSno)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", _cls.salesInvoiceId.ToString(), clsSiDetail.productId, clsSiDetail.unitId, "O", "0", "0", "1", DateTime.Now.ToString(), clsProductSerial.serialNo.Trim(), "1/1/1800", "", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //proceed with tax detail




                    double taxValue = 0;
                    double.TryParse(clsSiDetail.taxValue, out taxValue);
                    if (taxValue > 0)
                    {
                        //double discountValue = 0;
                        //double.TryParse(clsSiDetail.discountValue, out discountValue);
                        //double actualUnitPrice = double.Parse(clsSiDetail.unitPrice) - discountValue;
                        //double taxableAmt = actualUnitPrice * double.Parse(clsSiDetail.salesQty);
                        foreach (Inv_SalesInvoiceHeader.clsInvTaxDetail clsTax in _cls.lstTaxDetail)
                        {
                            if (clsSiDetail.sNo == clsTax.refRowSno)
                            {
                                //string strTaxPerUnit = SystemParameter.GetAmountWithDecimal((actualUnitPrice * double.Parse(clsTax.taxPer) / 100).ToString(), "3"); //hardcoded pass 3 to get accuracy
                                //string strTaxOnTotalUnit = SystemParameter.GetAmountWithDecimal((double.Parse(clsSiDetail.salesQty) * double.Parse(strTaxPerUnit)).ToString(), "3"); //hardcoded pass 3 to get accuracy
                                objTaxRefDetail.InsertRecord("SINV", _cls.salesInvoiceId.ToString(), "0", "0", clsSiDetail.productId, clsTax.taxId, clsTax.taxPer, clsTax.taxValue, false.ToString(), clsTax.taxableAmount, clsSiDetail.siDetailId.ToString(), _cls.transType, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                }

                //proceed with payment detail
                ObjPaymentTrans.insert(Session["CompId"].ToString(), _cls.paymentMode.ToString().ToLower() == "credit" ? creditPayModeId : cashPayModeId, "SI", _cls.salesInvoiceId.ToString(), "0", _cls.accountNo, "0", "", "", "0", "", "0", DateTime.Now.ToString(), _cls.netAmountWithExp, _cls.currencyId, _cls.exchangeRate, _cls.netAmountWithExp, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            trns.Commit();
            con.Close();
            DisplayMessage("Total " + lstInvoiceList.Count + " Records inserted successfully");
            gvImportInvoiceList.DataSource = null;
            gvImportInvoiceList.DataBind();
            btnSaveExcelInvoice.Enabled = false;
            Session["ExcelImportInvoiceList"] = null;

        }
        catch (Exception ex)
        {
            trns.Rollback();
            con.Close();
            DisplayMessage(ex.Message);
        }

        //save address master
    }
    protected void btnUploadExcel_Click(object sender, EventArgs e)
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
            hdnTotalExcelRecords.Value = "0";
            hdnInvalidExcelRecords.Value = "0";
            hdnValidExcelRecords.Value = "0";

            string strcon = string.Empty;
            if (fileType == 1)
            {
                Session["filetype"] = "excel";
                strcon = @"Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
            }
            else if (fileType == 0)
            {
                Session["filetype"] = "excel";
                strcon = @"Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
            }
            else
            {
                Session["filetype"] = "access";
                //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
                strcon = @"Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
            }

            using (OleDbConnection oledbConn = new OleDbConnection(strcon))
            {

                //oledbConn.Mode = ADODB.ConnectModeEnum.adModeReadWrite;
                oledbConn.Open();
                // DataTable  dtSheetName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                //OleDbCommand cmd = new OleDbCommand("SELECT * FROM "+ dtSheetName.Rows[0][0].ToString(), oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds, "poItem");
                List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> lstInvoice = new List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> { };
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Inv_SalesInvoiceHeader.clsImportEComInvoiceList _clsObj = new Inv_SalesInvoiceHeader.clsImportEComInvoiceList();
                        _clsObj.merchant = dr["merchant"].ToString().Trim();
                        _clsObj.customer_name = dr["customer_name"].ToString().Trim();
                        _clsObj.customer_id = dr["customer_id"].ToString().Trim();
                        _clsObj.billing_address = dr["billing_address"].ToString().Trim();
                        _clsObj.billing_country = dr["billing_country"].ToString().Trim();
                        _clsObj.billing_state = dr["billing_state"].ToString().Trim();
                        _clsObj.billing_city = dr["billing_city"].ToString().Trim();
                        _clsObj.billing_pin = dr["billing_pin"].ToString().Trim();
                        _clsObj.shipping_address = dr["shipping_address"].ToString().Trim();
                        _clsObj.shipping_country = dr["shipping_country"].ToString().Trim();
                        _clsObj.shipping_state = dr["shipping_state"].ToString().Trim();
                        _clsObj.shipping_city = dr["shipping_city"].ToString().Trim();
                        _clsObj.shipping_pin = dr["shipping_pin"].ToString().Trim();
                        _clsObj.mobile = dr["mobile"].ToString().Trim();
                        _clsObj.email = dr["email"].ToString().Trim();
                        _clsObj.gst_no = dr["gst_no"].ToString().Trim();
                        _clsObj.order_id = dr["order_id"].ToString().Trim();
                        _clsObj.invoice_no = dr["invoice_no"].ToString().Trim();
                        _clsObj.invoice_date = DateTime.Parse(dr["invoice_date"].ToString().Trim()).ToString("dd-MMM-yyyy");
                        _clsObj.invoice_amount = dr["invoice_amount"].ToString().Trim();
                        _clsObj.sales_person = dr["sales_person"].ToString().Trim();
                        _clsObj.payment_mode = dr["payment_mode"].ToString().Trim();
                        _clsObj.product_id = dr["product_id"].ToString().Trim();
                        _clsObj.qty = dr["qty"].ToString().Trim();
                        _clsObj.product_sno = dr["product_sno"].ToString().Trim();
                        _clsObj.unit_price = dr["unit_price"].ToString().Trim();
                        _clsObj.discount_rate = dr["discount_rate"].ToString().Trim();
                        _clsObj.cgst_rate = dr["cgst_rate"].ToString().Trim();
                        _clsObj.sgst_rate = dr["sgst_rate"].ToString().Trim();
                        _clsObj.igst_rate = dr["igst_rate"].ToString().Trim();
                        _clsObj.tcs_product_id = dr["tcs_product_id"].ToString().Trim();
                        _clsObj.tcs_rate = dr["tcs_rate"].ToString().Trim();
                        _clsObj.tcs_qty = dr["tcs_qty"].ToString().Trim();
                        _clsObj.tcs_cgst = dr["tcs_cgst"].ToString().Trim();
                        _clsObj.tcs_sgst = dr["tcs_sgst"].ToString().Trim();
                        _clsObj.tcs_igst = dr["tcs_igst"].ToString().Trim();
                        try
                        {
                            _clsObj.shipmentid = dr["shipmentid"].ToString().Trim();
                        }
                        catch
                        {
                            _clsObj.shipmentid = "NS";
                        }
                        lstInvoice.Add(_clsObj);
                    }
                }

                oledbConn.Close();

                if (lstInvoice.Count > 0)
                {
                    List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> newInvoiceList = validateExcelData(lstInvoice);
                    if (newInvoiceList.Count > 0)
                    {
                        gvImportInvoiceList.DataSource = newInvoiceList;
                        gvImportInvoiceList.DataBind();
                        hdnTotalExcelRecords.Value = newInvoiceList.Count().ToString();
                        hdnInvalidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == false).Count().ToString();
                        hdnValidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == true).Count().ToString();
                        Session["ExcelImportList"] = newInvoiceList;
                    }
                    else
                    {
                        Session["ExcelImportList"] = null;
                        Session["ExcelImportInvoiceList"] = null;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (hdnInvalidExcelRecords.Value != "0")
            {
                btnSaveExcelInvoice.Enabled = false;
            }
            else
            {
                btnSaveExcelInvoice.Enabled = true;
            }
            lnkTotalExcelImportRecords.Text = "Total Records:" + hdnTotalExcelRecords.Value;
            lnkValidRecords.Text = "Valid:" + hdnValidExcelRecords.Value;
            lnkInvalidRecords.Text = "Invalid:" + hdnInvalidExcelRecords.Value;
        }
    }
    protected List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> validateExcelData(List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> lstInvoice)
    {
        if (lstInvoice.Count == 0)
        {
            return null;
        }
        //CountryMaster().getCountryIdByName()
        CountryMaster objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        StateMaster objStateMaster = new StateMaster(Session["DBConnection"].ToString());
        CityMaster objCityMaster = new CityMaster(Session["DBConnection"].ToString());
        MerchantMaster objMerchantMaster = new MerchantMaster(Session["DBConnection"].ToString());


        int sno = 0;
        List<Inv_SalesInvoiceHeader.clsSInvHeader> lstNewInvoice = new List<Inv_SalesInvoiceHeader.clsSInvHeader> { };
        Inv_SalesInvoiceHeader.clsSInvHeader clsInvHeader = null;
        Set_CustomerMaster.clsCustomerMaster clsCustomerMaster = null;

        string igstId = objTaxMaster.GetTaxMaster_ByTaxName("igst").Rows[0]["trans_id"].ToString();
        string cgstId = objTaxMaster.GetTaxMaster_ByTaxName("cgst").Rows[0]["trans_id"].ToString();
        string sgstId = objTaxMaster.GetTaxMaster_ByTaxName("sgst").Rows[0]["trans_id"].ToString();

        double invGrossAmt = 0, InvGrossTaxableAmt = 0, InvGrossTaxAmt = 0, InvGrossDiscountableAmt = 0, InvGrossDisAmt = 0;
        string strInvoiceNo = "0";
        double invoiceAmt = 0;
        foreach (var _cls in lstInvoice)
        {
            sno++;
            try
            {
                if (_cls.invoice_no.Trim() != strInvoiceNo)
                {
                    if (clsInvHeader != null)
                    {
                        clsInvHeader.grossAmount = invGrossAmt.ToString();
                        if (InvGrossDisAmt > 0)
                        {
                            clsInvHeader.discountAmount = InvGrossDisAmt.ToString();
                            clsInvHeader.discountPer = SetDecimal(((InvGrossDisAmt * 100) / InvGrossDiscountableAmt).ToString());
                        }

                        if (InvGrossTaxAmt > 0)
                        {
                            clsInvHeader.taxAmount = InvGrossTaxAmt.ToString();
                            clsInvHeader.taxPer = SetDecimal(((InvGrossTaxAmt * 100) / InvGrossTaxableAmt).ToString());
                        }
                        clsInvHeader.totalQuantity = clsInvHeader.lstProductDetail.Sum(m => double.Parse(m.salesQty)).ToString();
                        clsInvHeader.netAmount = SetDecimal((invGrossAmt - InvGrossDisAmt + InvGrossTaxAmt).ToString());
                        clsInvHeader.netAmountWithExp = clsInvHeader.netAmount;
                        clsInvHeader.isApproved = true;
                        clsInvHeader.invoiceCosting = "0";
                        clsInvHeader.expensesAmount = "0";

                        lstNewInvoice.Add(clsInvHeader);
                        strInvoiceNo = "0";
                        invoiceAmt = 0;
                    }

                    clsInvHeader = new Inv_SalesInvoiceHeader.clsSInvHeader();
                    clsInvHeader.lstTaxDetail = new List<Inv_SalesInvoiceHeader.clsInvTaxDetail> { };
                    clsInvHeader.lstProductDetail = new List<Inv_SalesInvoiceHeader.clsSInvDetail> { };
                    clsInvHeader.lstProductSerialDetail = new List<Inv_SalesInvoiceHeader.clsProductSno> { };
                    clsInvHeader.lstPaymentDetail = new List<Inv_SalesInvoiceHeader.clsPayTrns> { };
                    clsInvHeader.shipmentid = _cls.shipmentid.ToString();
                    invGrossAmt = InvGrossTaxableAmt = InvGrossTaxAmt = InvGrossDiscountableAmt = InvGrossDisAmt = 0; //iniatilize variable for invoice level calculation

                    strInvoiceNo = _cls.invoice_no.Trim();
                    sno = 1;

                    //check customer id (code) already exist or not
                    if (!string.IsNullOrEmpty(_cls.customer_id) && _cls.customer_id != "0")
                    {
                        using (DataTable _dt = ObjCustmer.GetCustomerDataByCustomerCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), _cls.customer_id))
                        {
                            if (_dt.Rows.Count > 0)
                            {
                                clsInvHeader.customerId = _dt.Rows[0]["trans_id"].ToString();
                            }
                            else
                            {
                                _cls.validation_remark = "Customer Id does not exist";
                                continue;
                            }
                        }
                    }


                    //get customer master data
                    clsCustomerMaster = new Set_CustomerMaster.clsCustomerMaster();
                    clsCustomerMaster.name = _cls.customer_name.Trim();
                    clsCustomerMaster.name_l = _cls.customer_name.Trim();
                    clsCustomerMaster.type = "Company";

                    if (!string.IsNullOrEmpty(_cls.mobile.Trim()))
                    {
                        //if (!Regex.Match(_cls.mobile, @"^(\+[0-9]{9})$").Success)
                        //{
                        //    _cls.validation_remark = "Invalid mobile";
                        //    continue;
                        //}
                        //else
                        //{
                        clsCustomerMaster.mobile = _cls.mobile.Trim();
                        // }
                    }


                    if (!string.IsNullOrEmpty(_cls.email.Trim()))
                    {
                        if (!cmn.IsValidEmailId(_cls.email))
                        {
                            _cls.validation_remark = "Invalid email";
                            continue;
                        }
                    }
                    clsCustomerMaster.email = _cls.email.Trim();


                    clsCustomerMaster.notes = "Customer came from " + _cls.merchant.Trim();

                    if (!string.IsNullOrEmpty(_cls.gst_no))
                    {
                        if (cmn.IsValidateGstNo(_cls.gst_no) == true)
                        {
                            //check gst no already exist or not
                            if (string.IsNullOrEmpty(clsInvHeader.customerId) || clsInvHeader.customerId == "0")
                            {
                                string customerCode = ObjCustmer.GetCustomerByGSTno(_cls.gst_no);
                                if (!string.IsNullOrEmpty(customerCode))
                                {
                                    _cls.validation_remark = "Customer - " + customerCode + " already exist with same gst";
                                    continue;
                                }
                            }
                            clsCustomerMaster.gst_no = _cls.gst_no;
                        }
                        else
                        {
                            _cls.validation_remark = "Invalid GST No";
                            continue;
                        }

                    }


                    //get and validate address data
                    Set_AddressMaster.clsAddressMaster clsBillingAddress = new Set_AddressMaster.clsAddressMaster();
                    clsBillingAddress.category = "1"; //company address
                    if (string.IsNullOrEmpty(_cls.customer_name.Trim()))
                    {
                        _cls.validation_remark = "Invalid customer name";
                        continue;
                    }
                    if (_cls.customer_name.Contains('/'))
                    {
                        _cls.validation_remark = "Invalid customer name '/' is not allowed";
                        continue;
                    }

                    clsBillingAddress.name = _cls.customer_name.Trim() + "-" + _cls.billing_city + "-" + _cls.billing_state; //how to make it unique and meaningful

                    if (string.IsNullOrEmpty(_cls.billing_address.Trim()))
                    {
                        _cls.validation_remark = "Invalid billing address";
                        continue;
                    }
                    clsBillingAddress.address = _cls.billing_address.Trim();

                    if (string.IsNullOrEmpty(_cls.billing_country.Trim()))
                    {
                        _cls.validation_remark = "Invalid billing country";
                        continue;
                    }
                    clsBillingAddress.country_id = objCountryMaster.getCountryIdByName(_cls.billing_country.Trim());

                    if (string.IsNullOrEmpty(_cls.billing_state.Trim()))
                    {
                        _cls.validation_remark = "Invalid billing state";
                        continue;
                    }
                    clsBillingAddress.state_id = objStateMaster.GetStateIdFromCountryIdNStateName(clsBillingAddress.country_id, _cls.billing_state.Trim());
                    if (string.IsNullOrEmpty(clsBillingAddress.state_id))
                    {
                        _cls.validation_remark = "Invalid billing state";
                        continue;
                    }

                    if (string.IsNullOrEmpty(_cls.billing_city.Trim()))
                    {
                        _cls.validation_remark = "Invalid billing city";
                        continue;
                    }
                    clsBillingAddress.city_id = objCityMaster.GetCityIdFromStateIdNCityName(clsBillingAddress.state_id, _cls.billing_city.Trim());
                    if (string.IsNullOrEmpty(clsBillingAddress.city_id))
                    {
                        _cls.validation_remark = "Invalid billing city";
                        continue;
                    }

                    if (string.IsNullOrEmpty(_cls.billing_pin.Trim()))
                    {
                        _cls.validation_remark = "Invalid billing pin";
                        continue;
                    }
                    clsBillingAddress.pin_code = _cls.billing_pin.Trim();
                    //add address in customer class
                    if (clsCustomerMaster.lst_address == null)
                    {
                        clsCustomerMaster.lst_address = new List<Set_AddressMaster.clsAddressMaster> { };
                    }
                    clsInvHeader.billingAddressId = clsInvHeader.shippingAddressId = clsBillingAddress.id = "0";
                    //check address alread exist or not
                    if (!string.IsNullOrEmpty(clsInvHeader.customerId) && clsInvHeader.customerId != "0")
                    {
                        using (DataTable _dtAdd = objAddressChild.GetAddressListByAddTypeAndAddRefId("contact", clsInvHeader.customerId))
                        {
                            foreach (DataRow _add in _dtAdd.Rows)
                            {
                                if (_add["StateId"].ToString() == clsBillingAddress.state_id && _add["CityId"].ToString() == clsBillingAddress.city_id && _add["address"].ToString() == clsBillingAddress.address)
                                {
                                    clsInvHeader.shippingAddressId = clsBillingAddress.id = clsInvHeader.billingAddressId = _add["Trans_id"].ToString();
                                }
                            }
                        }
                    }


                    clsCustomerMaster.lst_address.Add(clsBillingAddress);

                    if (string.IsNullOrEmpty(_cls.shipping_address))
                    {
                        _cls.shipping_address = _cls.billing_address.Trim();
                        _cls.shipping_country = _cls.billing_country.Trim();
                        _cls.shipping_state = _cls.billing_state.Trim();
                        _cls.shipping_city = _cls.billing_city.Trim();
                        _cls.shipping_pin = _cls.billing_pin.Trim();
                    }
                    else
                    {
                        Set_AddressMaster.clsAddressMaster clsShippingAddress = new Set_AddressMaster.clsAddressMaster();
                        clsShippingAddress.category = "3"; //delivery address
                        clsShippingAddress.name = _cls.customer_name.Trim() + "-" + _cls.billing_city + "-" + _cls.billing_state; //how to make it unique and meaningful

                        if (string.IsNullOrEmpty(_cls.shipping_address.Trim()))
                        {
                            _cls.validation_remark = "Invalid shipping address";
                            continue;
                        }
                        clsShippingAddress.address = _cls.shipping_address.Trim();

                        if (string.IsNullOrEmpty(_cls.shipping_country.Trim()))
                        {
                            _cls.validation_remark = "Invalid shipping country";
                            continue;
                        }
                        clsShippingAddress.country_id = objCountryMaster.getCountryIdByName(_cls.shipping_country.Trim());
                        if (string.IsNullOrEmpty(clsShippingAddress.country_id))
                        {
                            _cls.validation_remark = "Invalid shipping country";
                            continue;
                        }


                        if (string.IsNullOrEmpty(_cls.shipping_state.Trim()))
                        {
                            _cls.validation_remark = "Invalid shipping state";
                            continue;
                        }

                        clsShippingAddress.state_id = objStateMaster.GetStateIdFromCountryIdNStateName(clsShippingAddress.country_id, _cls.shipping_state.Trim());
                        if (string.IsNullOrEmpty(clsShippingAddress.state_id))
                        {
                            _cls.validation_remark = "Invalid shipping state";
                            continue;
                        }


                        if (string.IsNullOrEmpty(_cls.shipping_city.Trim()))
                        {
                            _cls.validation_remark = "Invalid shipping city";
                            continue;
                        }
                        clsShippingAddress.city_id = objCityMaster.GetCityIdFromStateIdNCityName(clsShippingAddress.state_id, _cls.shipping_city);
                        if (string.IsNullOrEmpty(clsShippingAddress.city_id))
                        {
                            _cls.validation_remark = "Invalid shipping city";
                            continue;
                        }


                        if (string.IsNullOrEmpty(_cls.shipping_pin.Trim()))
                        {
                            _cls.validation_remark = "Invalid shipping pin";
                            continue;
                        }
                        clsShippingAddress.pin_code = _cls.shipping_pin.Trim();
                        //add address detail in customer class
                        clsInvHeader.shippingAddressId = clsShippingAddress.id = "0";
                        //check address alread exist or not
                        if (!string.IsNullOrEmpty(clsInvHeader.customerId) && clsInvHeader.customerId != "0")
                        {
                            using (DataTable _dtAdd = objAddressChild.GetAddressListByAddTypeAndAddRefId("contact", clsInvHeader.customerId))
                            {
                                foreach (DataRow _add in _dtAdd.Rows)
                                {
                                    if (_add["StateId"].ToString() == clsBillingAddress.state_id && _add["CityId"].ToString() == clsBillingAddress.city_id && _add["address"].ToString() == clsBillingAddress.address)
                                    {
                                        clsInvHeader.shippingAddressId = clsShippingAddress.id = _add["Trans_id"].ToString();
                                    }
                                }
                            }
                        }
                        clsCustomerMaster.lst_address.Add(clsShippingAddress);
                    }

                    //add customer class on sales invoice header class
                    clsCustomerMaster.country_id = clsBillingAddress.country_id;
                    clsInvHeader.clsCustomerDetail = clsCustomerMaster;

                    if (string.IsNullOrEmpty(clsInvHeader.customerId) || clsInvHeader.customerId == "0")
                    {
                        //check for duplicate customer 
                        string strCustomerCode = ObjCustmer.CheckDuplicateCustomerForEcomExcelImport(Session["compId"].ToString(), _cls.customer_name, clsCustomerMaster.lst_address[0].address, clsCustomerMaster.lst_address[0].state_id, clsCustomerMaster.lst_address[0].city_id);
                        if (!string.IsNullOrEmpty(strCustomerCode))
                        {
                            _cls.validation_remark = "Customer already exist please use customer_id=" + strCustomerCode;
                            _cls.customer_id = strCustomerCode;
                            continue;
                        }
                    }

                    //get invoice header
                    if (string.IsNullOrEmpty(_cls.merchant.Trim()))
                    {
                        _cls.validation_remark = "Invalid merchant please select direct or any merchant";
                        continue;
                    }
                    clsInvHeader.invoiceMerchantId = objMerchantMaster.GetMerchantMaster_ByMerchantName(_cls.merchant).Rows[0]["Trans_id"].ToString();

                    //order id
                    if (string.IsNullOrEmpty(_cls.order_id))
                    {
                        _cls.validation_remark = "invalid order id";
                        continue;
                    }
                    //check same invoice no with same merchant exist or not
                    if (objSInvHeader.GetInvoiceCountByMerchantIdAndRefOrderId(Session["LocId"].ToString(), clsInvHeader.invoiceMerchantId, _cls.order_id) > 0)
                    {
                        _cls.validation_remark = "Same order no already exist";
                        continue;
                    }
                    clsInvHeader.refOrderNumber = _cls.order_id;

                    if (string.IsNullOrEmpty(_cls.invoice_date.Trim()))
                    {
                        _cls.validation_remark = "Invalid invoice date";
                        continue;
                    }
                    else
                    {
                        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(_cls.invoice_date), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                        {
                            _cls.validation_remark = "Invoice date out of financial year";
                            continue;
                        }
                        DateTime.Parse(_cls.invoice_date.Trim());
                    }
                    clsInvHeader.invoiceDate = DateTime.Parse(_cls.invoice_date).ToString("dd-MMM-yyyy");



                    if (string.IsNullOrEmpty(_cls.invoice_no.Trim()))
                    {
                        _cls.validation_remark = "Invalid invoice no";
                        continue;
                    }
                    //check same invoice no with same merchant exist or not
                    if (objSInvHeader.GetInvoiceCountByMerchantIdAndRefInvoiceNo(Session["LocId"].ToString(), clsInvHeader.invoiceMerchantId, _cls.invoice_no) > 0)
                    {
                        _cls.validation_remark = "Same invoice no already exist";
                        continue;
                    }
                    clsInvHeader.invoiceRefNo = _cls.invoice_no;


                    double.TryParse(_cls.invoice_amount, out invoiceAmt);
                    if (invoiceAmt == 0)
                    {
                        _cls.validation_remark = "Invalid Invoice Amount";
                        continue;
                    }
                    clsInvHeader.netAmountWithExp = _cls.invoice_amount;

                    if (string.IsNullOrEmpty(_cls.sales_person))
                    {
                        _cls.validation_remark = "Invalid Sales Person";
                        continue;
                    }
                    clsInvHeader.salesPersonId = ObjEmployeeMaster.GetEmployeeIdByEmployeeName(_cls.sales_person, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    if (clsInvHeader.salesPersonId == "0")
                    {
                        _cls.validation_remark = "Invalid Sales Person";
                        continue;
                    }

                    if (!string.IsNullOrEmpty(_cls.payment_mode) && (_cls.payment_mode.ToLower() == "cash" || _cls.payment_mode.ToLower() == "credit"))
                    {
                        clsInvHeader.paymentMode = _cls.payment_mode;
                    }
                    else
                    {
                        _cls.validation_remark = "Invalid Payment Mode it should be cash/credit";
                        continue;
                    }

                }
                //get invoice detail
                Inv_SalesInvoiceHeader.clsSInvDetail clsInvDetail = new Inv_SalesInvoiceHeader.clsSInvDetail();
                clsInvDetail.sNo = sno.ToString();

                if (string.IsNullOrEmpty(_cls.product_id))
                {
                    _cls.validation_remark = "Invalid ProductId";
                    continue;
                }
                clsInvDetail.productId = objProductM.GetProductIdByCodeAndAlternetId(Session["compId"].ToString(), Session["brandId"].ToString(), _cls.product_id).ToString();
                if (clsInvDetail.productId == "0")
                {
                    _cls.validation_remark = "Invalid ProductId";
                    continue;
                }


                double ProductQty = 0;
                double.TryParse(_cls.qty, out ProductQty);
                if (ProductQty == 0)
                {
                    _cls.validation_remark = "Invalid Qty";
                    continue;
                }
                clsInvDetail.salesQty = _cls.qty;

                if (!string.IsNullOrEmpty(_cls.product_sno))
                {
                    string[] strProductSno = _cls.product_sno.Split(',');
                    if (strProductSno.Count() > int.Parse(_cls.qty))
                    {
                        _cls.validation_remark = "Serial detail should equal to or less then qty";
                        continue;
                    }
                    foreach (string str in strProductSno)
                    {
                        Inv_SalesInvoiceHeader.clsProductSno _clsProductSno = new Inv_SalesInvoiceHeader.clsProductSno();
                        _clsProductSno.productId = clsInvDetail.productId;
                        _clsProductSno.refRowSno = clsInvDetail.sNo;
                        _clsProductSno.serialNo = str;
                        clsInvHeader.lstProductSerialDetail.Add(_clsProductSno);
                    }
                }

                double UnitPrice = 0;
                double.TryParse(_cls.unit_price, out UnitPrice);
                if (UnitPrice == 0 || UnitPrice < 0)
                {
                    _cls.validation_remark = "Invalid Price";
                    continue;
                }
                clsInvDetail.unitPrice = _cls.unit_price;

                invGrossAmt += double.Parse(SetDecimal((UnitPrice * ProductQty).ToString())); //invoice gross amount to update sales header

                double DiscountRate = 0;
                double.TryParse(_cls.discount_rate, out DiscountRate);
                clsInvDetail.discountPer = DiscountRate.ToString();

                if (DiscountRate > 0)
                {
                    InvGrossDiscountableAmt += double.Parse(SetDecimal((ProductQty * UnitPrice).ToString()));
                    InvGrossDisAmt += double.Parse(SetDecimal(((ProductQty * UnitPrice) / DiscountRate).ToString()));
                }

                //Get tax detail

                Inv_SalesInvoiceHeader.clsInvTaxDetail clsInvTaxDetail = new Inv_SalesInvoiceHeader.clsInvTaxDetail();

                //product tax
                double CgstRate = 0;
                double.TryParse(_cls.cgst_rate, out CgstRate);
                double SgstRate = 0;
                double.TryParse(_cls.sgst_rate, out SgstRate);
                double IgstRate = 0;
                double.TryParse(_cls.igst_rate, out IgstRate);

                if ((CgstRate > 0 || SgstRate > 0) && IgstRate > 0)
                {
                    _cls.validation_remark = "Invalid Tax - Igst and Cgst both not applicable";
                    continue;
                }

                if ((CgstRate != SgstRate) && IgstRate == 0)
                {
                    _cls.validation_remark = "Invalid Tax - Cgst and Sgst should be same";
                    continue;
                }

                double TaxableAmount = 0;
                TaxableAmount = double.Parse(SetDecimal(((ProductQty * UnitPrice) - ((ProductQty * UnitPrice) * DiscountRate / 100)).ToString()));
                double TotalProductAmt = TaxableAmount;
                if (IgstRate > 0 || SgstRate > 0 || CgstRate > 0)
                {
                    InvGrossTaxableAmt += TaxableAmount; //for header level calculation
                    if (IgstRate > 0)
                    {
                        clsInvTaxDetail = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                        clsInvTaxDetail.refType = "SINV";
                        clsInvTaxDetail.refRowSno = sno.ToString();
                        clsInvTaxDetail.productId = clsInvDetail.productId;
                        clsInvTaxDetail.taxableAmount = TaxableAmount.ToString();
                        clsInvTaxDetail.expensesId = "0";
                        clsInvTaxDetail.taxId = igstId;
                        clsInvTaxDetail.taxPer = _cls.igst_rate;
                        clsInvTaxDetail.taxValue = SetDecimal((TaxableAmount * IgstRate / 100).ToString());
                        TotalProductAmt += double.Parse(clsInvTaxDetail.taxValue);
                        //lstClsInvTaxDetail.Add(clsInvTaxDetail);
                        clsInvHeader.lstTaxDetail.Add(clsInvTaxDetail);

                        clsInvHeader.transType = "2"; //intersate
                        InvGrossTaxAmt += double.Parse(clsInvTaxDetail.taxValue);
                    }
                    if (CgstRate > 0)
                    {
                        clsInvTaxDetail = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                        clsInvTaxDetail.refType = "SINV";
                        clsInvTaxDetail.refRowSno = sno.ToString();
                        clsInvTaxDetail.productId = clsInvDetail.productId;
                        clsInvTaxDetail.taxableAmount = TaxableAmount.ToString();
                        clsInvTaxDetail.expensesId = "0";
                        clsInvTaxDetail.taxId = cgstId;
                        clsInvTaxDetail.taxPer = _cls.cgst_rate;
                        clsInvTaxDetail.taxValue = SetDecimal((TaxableAmount * CgstRate / 100).ToString());
                        TotalProductAmt += double.Parse(clsInvTaxDetail.taxValue);
                        clsInvHeader.lstTaxDetail.Add(clsInvTaxDetail);
                        clsInvHeader.transType = "1"; //intrastate
                        InvGrossTaxAmt += double.Parse(clsInvTaxDetail.taxValue);
                    }
                    if (SgstRate > 0)
                    {
                        clsInvTaxDetail = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                        clsInvTaxDetail.refType = "SINV";
                        clsInvTaxDetail.refRowSno = sno.ToString();
                        clsInvTaxDetail.productId = clsInvDetail.productId;
                        clsInvTaxDetail.taxableAmount = TaxableAmount.ToString();
                        clsInvTaxDetail.expensesId = "0";
                        clsInvTaxDetail.taxId = sgstId;
                        clsInvTaxDetail.taxPer = _cls.sgst_rate;
                        clsInvTaxDetail.taxValue = SetDecimal((TaxableAmount * SgstRate / 100).ToString());
                        TotalProductAmt += double.Parse(clsInvTaxDetail.taxValue);
                        clsInvHeader.lstTaxDetail.Add(clsInvTaxDetail);
                        InvGrossTaxAmt += double.Parse(clsInvTaxDetail.taxValue);
                    }

                    clsInvDetail.taxPer = SetDecimal((SgstRate + CgstRate + IgstRate).ToString());
                    clsInvDetail.taxValue = SetDecimal(((UnitPrice - (UnitPrice * DiscountRate / 100)) * (SgstRate + CgstRate + IgstRate) / 100).ToString());

                    //add product detail in sales invoice header 
                    clsInvHeader.lstProductDetail.Add(clsInvDetail);

                    //Transportation tax
                    double TcsRate = 0;
                    double.TryParse(_cls.tcs_rate, out TcsRate);

                    double TcsQty = 0;
                    double.TryParse(_cls.tcs_qty, out TcsQty);

                    double TcsAmount = TcsQty * TcsRate;

                    double TcsCgstRate = 0;
                    double.TryParse(_cls.tcs_cgst, out TcsCgstRate);
                    double TcsSgstRate = 0;
                    double.TryParse(_cls.tcs_sgst, out TcsSgstRate);
                    double TcsIgstRate = 0;
                    double.TryParse(_cls.tcs_igst, out TcsIgstRate);

                    if ((TcsCgstRate > 0 || TcsSgstRate > 0) && TcsIgstRate > 0)
                    {
                        _cls.validation_remark = "Invalid Tax - Igst and Cgst both not applicable in Transportation Charge";
                        continue;
                    }

                    if ((TcsCgstRate != TcsSgstRate) && TcsIgstRate == 0)
                    {
                        _cls.validation_remark = "Invalid Tax - Cgst and Sgst should be same in transportation charges";
                        continue;
                    }

                    Double TotalTcsAmt = 0;
                    double TcsTaxableAmount = 0;
                    TcsTaxableAmount = TcsAmount;
                    invGrossAmt += TcsAmount;
                    TotalTcsAmt = TcsTaxableAmount;
                    TotalTcsAmt += double.Parse(SetDecimal(((TcsTaxableAmount * (TcsIgstRate + TcsSgstRate + TcsCgstRate)) / 100).ToString()));
                    if (((TotalProductAmt + TotalTcsAmt) - invoiceAmt) < 1)
                    {
                        if (TcsAmount > 0)
                        {
                            Inv_SalesInvoiceHeader.clsSInvDetail clsInvDetailTcs = new Inv_SalesInvoiceHeader.clsSInvDetail();
                            sno++;
                            clsInvDetailTcs.sNo = (sno).ToString();


                            clsInvDetailTcs.productId = objProductM.GetProductIdByCodeAndAlternetId(Session["compId"].ToString(), Session["brandId"].ToString(), _cls.tcs_product_id).ToString();
                            if (clsInvDetailTcs.productId == "0")
                            {
                                _cls.validation_remark = "Invalid Tcs ProductId";
                                continue;
                            }
                            clsInvDetailTcs.salesQty = TcsQty.ToString();
                            clsInvDetailTcs.unitPrice = TcsRate.ToString();
                            clsInvDetailTcs.discountPer = "0";


                            if (TcsIgstRate > 0 || TcsSgstRate > 0 || TcsCgstRate > 0)
                            {

                                InvGrossTaxableAmt += TcsTaxableAmount; //for header level calculation

                                if (TcsIgstRate > 0)
                                {
                                    Inv_SalesInvoiceHeader.clsInvTaxDetail clsTaxTcsIgst = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                                    clsTaxTcsIgst.refType = "SINV";
                                    clsTaxTcsIgst.refRowSno = (sno).ToString();
                                    clsTaxTcsIgst.productId = clsInvDetailTcs.productId;
                                    clsTaxTcsIgst.taxableAmount = TcsTaxableAmount.ToString();
                                    clsTaxTcsIgst.expensesId = "0";
                                    clsTaxTcsIgst.taxId = igstId;
                                    clsTaxTcsIgst.taxPer = _cls.tcs_igst;
                                    clsTaxTcsIgst.taxValue = SetDecimal((TcsTaxableAmount * TcsIgstRate / 100).ToString());
                                    clsInvHeader.lstTaxDetail.Add(clsTaxTcsIgst);
                                    InvGrossTaxAmt += double.Parse(clsTaxTcsIgst.taxValue);
                                }
                                if (TcsCgstRate > 0)
                                {
                                    Inv_SalesInvoiceHeader.clsInvTaxDetail clsTaxTcsCgst = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                                    clsTaxTcsCgst.refType = "SINV";
                                    clsTaxTcsCgst.refRowSno = (sno).ToString();
                                    clsTaxTcsCgst.productId = clsInvDetailTcs.productId;
                                    clsTaxTcsCgst.taxableAmount = TcsTaxableAmount.ToString();
                                    clsTaxTcsCgst.expensesId = "0";
                                    clsTaxTcsCgst.taxId = cgstId;
                                    clsTaxTcsCgst.taxPer = _cls.tcs_cgst;
                                    clsTaxTcsCgst.taxValue = SetDecimal((TcsTaxableAmount * TcsCgstRate / 100).ToString());
                                    clsInvHeader.lstTaxDetail.Add(clsTaxTcsCgst);
                                    InvGrossTaxAmt += double.Parse(clsTaxTcsCgst.taxValue);
                                }
                                if (TcsSgstRate > 0)
                                {
                                    Inv_SalesInvoiceHeader.clsInvTaxDetail clsTaxTcsSgst = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                                    clsTaxTcsSgst.refType = "SINV";
                                    clsTaxTcsSgst.refRowSno = (sno).ToString();
                                    clsTaxTcsSgst.productId = clsInvDetailTcs.productId;
                                    clsTaxTcsSgst.taxableAmount = TcsTaxableAmount.ToString();
                                    clsTaxTcsSgst.expensesId = "0";
                                    clsTaxTcsSgst.taxId = sgstId;
                                    clsTaxTcsSgst.taxPer = _cls.tcs_sgst;
                                    clsTaxTcsSgst.taxValue = SetDecimal((TcsTaxableAmount * TcsSgstRate / 100).ToString());
                                    clsInvHeader.lstTaxDetail.Add(clsTaxTcsSgst);
                                    InvGrossTaxAmt += double.Parse(clsTaxTcsSgst.taxValue);
                                }
                            }
                            clsInvDetailTcs.taxPer = SetDecimal((TcsSgstRate + TcsCgstRate + TcsIgstRate).ToString());
                            clsInvDetailTcs.taxValue = SetDecimal((TcsRate * (TcsSgstRate + TcsCgstRate + TcsIgstRate) / 100).ToString());
                            clsInvHeader.lstProductDetail.Add(clsInvDetailTcs);
                        }
                    }
                }
                if (_cls.validation_remark == null)
                {
                    _cls.is_valid = true;
                }
            }
            catch (Exception ex)
            {
                _cls.validation_remark = ex.Message;
                _cls.is_valid = false;
            }

        }
        if (clsInvHeader != null)
        {
            clsInvHeader.grossAmount = invGrossAmt.ToString();
            if (InvGrossDisAmt > 0)
            {
                clsInvHeader.discountAmount = InvGrossDisAmt.ToString();
                clsInvHeader.discountPer = SetDecimal(((InvGrossDisAmt * 100) / InvGrossDiscountableAmt).ToString());
            }

            if (InvGrossTaxAmt > 0)
            {
                clsInvHeader.taxAmount = InvGrossTaxAmt.ToString();
                clsInvHeader.taxPer = SetDecimal(((InvGrossTaxAmt * 100) / InvGrossTaxableAmt).ToString());
            }
            clsInvHeader.totalQuantity = clsInvHeader.lstProductDetail.Sum(m => double.Parse(m.salesQty)).ToString();
            clsInvHeader.netAmount = SetDecimal((invGrossAmt - InvGrossDisAmt + InvGrossTaxAmt).ToString());
            clsInvHeader.netAmountWithExp = clsInvHeader.netAmount;
            clsInvHeader.isApproved = true;
            clsInvHeader.invoiceCosting = "0";
            clsInvHeader.expensesAmount = "0";




            lstNewInvoice.Add(clsInvHeader);
        }

        if (lstNewInvoice.Count > 0)
        {
            Session["ExcelImportInvoiceList"] = lstNewInvoice;
        }
        else
        {
            Session["ExcelImportInvoiceList"] = null;
        }

        return lstInvoice;

    }

    protected void lnkTotalExcelImportRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportList"] != null)
        {
            List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> newInvoiceList = (List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList>)Session["ExcelImportList"];
            gvImportInvoiceList.DataSource = newInvoiceList;
            gvImportInvoiceList.DataBind();
        }
    }
    protected void lnkInvalidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportList"] != null)
        {
            List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> newInvoiceList = (List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList>)Session["ExcelImportList"];
            gvImportInvoiceList.DataSource = newInvoiceList.Where(m => m.is_valid == false).ToList();
            gvImportInvoiceList.DataBind();
        }
    }
    protected void lnkValidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportList"] != null)
        {
            List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> newInvoiceList = (List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList>)Session["ExcelImportList"];
            gvImportInvoiceList.DataSource = newInvoiceList.Where(m => m.is_valid == true).ToList();
            gvImportInvoiceList.DataBind();
        }
    }

    #endregion

    #region Job Card
    protected void GvCustomerInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCInquiry"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtCInquiry"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");

        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerInquiry.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiry"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");

        //AllPageCode();
    }
    protected void btnRefreshQuoteReport_Click(object sender, ImageClickEventArgs e)
    {

        ddlFieldNameQuote.SelectedIndex = 1;
        ddlOptionQuote.SelectedIndex = 2;

        txtValueQuote.Text = "";
        txtValueQuoteDate.Text = "";
        txtValueQuoteDate.Visible = false;
        txtValueQuote.Visible = true;

        //AllPageCode();
    }
    protected void ddlFieldNameQuote_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameQuote.SelectedItem.Value == "Job_date")
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
        ddlFieldName.Focus();
    }

    protected void btnbindrptQuote_Click(object sender, ImageClickEventArgs e)
    {

        if (ddlFieldNameQuote.SelectedItem.Value == "Job_date")
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
        if (ddlOptionQuote.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionQuote.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String)='" + txtValueQuote.Text.Trim() + "'";
            }
            else if (ddlOptionQuote.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String) like '%" + txtValueQuote.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameQuote.SelectedValue + ",System.String) Like '" + txtValueQuote.Text.Trim() + "%'";

            }
            DataTable dtAdd = (DataTable)Session["dtAllCInquiry"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiry, view.ToTable(), "", "");

            Session["dtCInquiry"] = view.ToTable();
            lblTotalRecordsQuote.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();
        }
        if (txtValueQuote.Text != "")
            txtValueQuote.Focus();
        else if (txtValueQuoteDate.Text != "")
            txtValueQuoteDate.Focus();
    }
    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((Button)sender).Parent.Parent;
        Reset();

        txtCustomer.Text = ((Label)gvrow.FindControl("lblgvCustomerName")).Text + "/" + ((Label)gvrow.FindControl("lblgvCustomerId")).Text;
        strSiCustomerId = Convert.ToInt32(((Label)gvrow.FindControl("lblgvCustomerId")).Text);
        txtCustomer_TextChanged(null, null);
        RdoWithOutSo.Checked = true;
        //RdoSo_CheckedChanged(null, null);

        Session["JobCardId"] = e.CommandArgument.ToString();

        //here we get labout detail for pull in sale sienvoice


        DataTable DtLaboutDetail = objJobCardLabourdetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());


        DtLaboutDetail = DtLaboutDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Product_Id", "ProductId", "Unit", "Quantity", "Unit_Price");


        if (DtLaboutDetail.Rows.Count > 0)
        {

            DataTable dtProduct = CreateProductDataTable();

            int counter = 0;

            foreach (DataRow drData in DtLaboutDetail.Rows)
            {
                counter++;
                DataRow dr = dtProduct.NewRow();

                dr["Trans_Id"] = counter.ToString();
                dr["Serial_No"] = counter.ToString();
                dr["SalesOrderNo"] = "0";
                dr["Product_Id"] = drData["ProductId"].ToString();
                dr["ProductName"] = GetProductName(drData["ProductId"].ToString());
                dr["ProductDescription"] = GetProductDescription(drData["ProductId"].ToString());
                dr["UnitId"] = drData["Unit"].ToString();
                dr["Unit_Name"] = GetUnitName(drData["Unit"].ToString());
                dr["UnitPrice"] = drData["Unit_Price"].ToString();
                dr["FreeQty"] = "0";
                dr["OrderQty"] = "0";
                dr["Quantity"] = drData["Quantity"].ToString();
                dr["TaxP"] = "0";
                dr["TaxV"] = "0";
                dr["DiscountP"] = "0";
                dr["DiscountV"] = "0";
                dr["SoldQty"] = "0";
                dr["SysQty"] = "0";



                try
                {
                    dr["SysQty"] = SetDecimal(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), drData["ProductId"].ToString()).Rows[0]["Quantity"].ToString());
                }
                catch
                {
                    dr["SysQty"] = "0";
                }

                dtProduct.Rows.Add(dr);
            }



            ViewState["Dtproduct"] = dtProduct;
            GetChildGridRecordInViewState();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
            //GridCalculation();
        }

        //if (Lbl_Tab_New.Text == "View")
        //{
        //    btnSInvSave.Enabled = false;
        //    btnPost.Enabled = false;
        //    BtnReset.Enabled = false;
        //}
        //else
        //{
        //    btnSInvSave.Enabled = true;
        //    btnPost.Enabled = true;
        //    BtnReset.Enabled = true;
        //}
        //AllPageCode();

    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {


    }
    #endregion


}