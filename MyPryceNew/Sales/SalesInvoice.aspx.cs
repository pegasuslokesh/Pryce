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
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Configuration;
using DevExpress.DataProcessing;

public partial class Sales_SalesInvoice : BasePage
{
    #region defined Class Object
    Prj_VehicleMaster objVehicleMaster = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_Approval_Employee objEmpApproval = null;
    Common cmn = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    Inv_StockDetail objStockDetail = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    Set_AddressMaster objAddMaster = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductLedger = null;
    DataAccessClass objDa = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_ParameterMaster objInvParam = null;
    Contact_PriceList objCustomerPriceList = null;
    Ac_ChartOfAccount objCOA = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Set_BankMaster ObjBankMaster = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ShipExpHeader ObjShipExpHeader = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Set_CustomerMaster ObjCustmer = null;
    LocationMaster objLocation = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_StockDetail objStock = null;
    Set_AddressChild objAddressChild = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();
    Inv_ProductCategory_Tax objProTax = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    TaxMaster objTaxMaster = null;
    MerchantMaster objMerchantMaster = null;
    Country_Currency objCountryCurrency = null;
    EmployeeMaster ObjEmployeeMaster = null;
    //Ac_Ageing_Detail objAgeingDetail = new Ac_Ageing_Detail();
    Inv_SalesDeliveryVoucher_Header objdelVoucherHeader = null;
    Inv_SalesDeliveryVoucher_Detail objdelVoucherDetail = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    SM_JobCard_Header objJobCardheader = null;
    SM_JobCards_SpareLabourDetail objJobCardLabourdetail = null;
    ProductTaxMaster objProductTaxMaster = null;
    NotificationMaster Obj_Notifiacation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
            objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
            objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
            objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
            objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
            objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
            ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
            objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
            objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
            objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
            objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
            objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
            objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
            UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
            objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
            objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
            ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
            ObjUser = new UserMaster(Session["DBConnection"].ToString());
            objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
            ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
            ObjProductLedger = new Inv_ProductLedger(Session["DBConnection"].ToString());
            objDa = new DataAccessClass(Session["DBConnection"].ToString());
            ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
            objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
            objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
            objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
            ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
            ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
            ObjBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
            ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
            ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
            ObjShipExpHeader = new Inv_ShipExpHeader(Session["DBConnection"].ToString());
            ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
            ObjCustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
            objLocation = new LocationMaster(Session["DBConnection"].ToString());
            objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
            objStock = new Inv_StockDetail(Session["DBConnection"].ToString());
            objAddressChild = new Set_AddressChild(Session["DBConnection"].ToString());
            objProTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
            objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
            objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
            objMerchantMaster = new MerchantMaster(Session["DBConnection"].ToString());
            objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
            ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
            objdelVoucherHeader = new Inv_SalesDeliveryVoucher_Header(Session["DBConnection"].ToString());
            objdelVoucherDetail = new Inv_SalesDeliveryVoucher_Detail(Session["DBConnection"].ToString());
            objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
            objJobCardheader = new SM_JobCard_Header(Session["DBConnection"].ToString());
            objJobCardLabourdetail = new SM_JobCards_SpareLabourDetail(Session["DBConnection"].ToString());
            objProductTaxMaster = new ProductTaxMaster(Session["DBConnection"].ToString());
            Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
            objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
            objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
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

            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strDepartmentId = Session["DepartmentId"].ToString();
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
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
            else
            {
                StrLocationId = Session["LocId"].ToString();
            }
            if (Session["Expenses_Sales_Invoice_Local"] != null)
            {
                DataTable Dt_temp = Session["Expenses_Sales_Invoice_Local"] as DataTable;
                if (Dt_temp.Rows.Count > 0)
                {
                    Expenses_Read_Only();
                }
            }

            btnSInvSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());

            if (!IsPostBack)
            {
                Session["ExcelImportList"] = null;
                Session["ExcelImportInvoiceList"] = null;

                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/SalesInvoice.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                objPageCmn.fillLocationWithAllOption(ddlLocation);
                Session["Expenses_Tax_Sales_Invoice"] = null;
                Session["ContactId"] = null;
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

                string Decimal_Count = string.Empty;
                Decimal_Count = cmn.Get_Decimal_Count_By_Location(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
                //TO GET ACCURACY FOR TAX WE ADDED THIS HARD CODE - BCZ WHEN PRICE IS TOO SMALL WE ARE UNABLE TO GET ACCURACY WITH TWO DIGIT (CASE WITH DUBAI - 03/JUL/2018) ADDED BY NEELKANTH PUROHIT
                Decimal_Count_For_Tax = 4;
                bool Tax_Apply = IsApplyTax();
                Session["Is_Tax_Apply"] = Tax_Apply.ToString();
                Session["Temp_Product_Tax_SINV"] = null;
                FillPageSizeddl();
                ViewState["PostBackID"] = Guid.NewGuid().ToString();
                ViewState["dtSerial"] = null;
                ddlOption.SelectedIndex = 2;
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
                FillGrid();

                fillBank();
                FillPaymentMode();

                fillExpenses();
                txtSInvNo.Text = GetDocumentNumber();
                ViewState["DocNo"] = GetDocumentNumber();

                //updated by Lokesh on 29-05-2024 as Monika Mam Said for Invoice Date
                //if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11" || Session["LocId"].ToString() == "14" || Session["LocId"].ToString() == "15" )
                //{
                //    txtSInvDate.ReadOnly = false;
                //    Calender.Format = Session["DateFormat"].ToString();
                //}
                //else
                //{
                //    txtSInvDate.ReadOnly = true;
                //}


                Calender.Format = Session["DateFormat"].ToString();
                CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
                CalendarExtendertxtVisitDate.Format = Session["DateFormat"].ToString();
                CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
                CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
                txtChequedate_CalenderExtender.Format = Session["DateFormat"].ToString();
                txtSInvDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                DateTime CurrentTime = DateTime.Now;
                // Format the DateTime object to display only the hour and minute
                string formattedTime = CurrentTime.ToString("HH:mm");
                // Set the formatted time to the text box

                txtVisitTime.Text = formattedTime;



                PnlProductSearching.Visible = false;
                ViewState["ExchangeRate"] = "1";
                setSymbol();
                btnAddCustomer.Visible = IsAddCustomerPermission();
                LoadStores();
                txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
                TaxandDiscountParameter();

                //For Finance Code For Finance Use Only.

                //this code for when we redirect from the producte ledger page 
                //this code created on 22-07-2015
                //code start

                if (Request.QueryString["Id"] != null)
                {
                    LinkButton imgeditbutton = new LinkButton();
                    imgeditbutton.CausesValidation = false;
                    imgeditbutton.ID = "lnkViewDetail";
                    imgeditbutton.CommandArgument = Request.QueryString["Id"].ToString();
                    imgeditbutton.CommandName = Session["LocId"].ToString();
                    btnEdit_Command(imgeditbutton, new CommandEventArgs(Session["LocId"].ToString(), Request.QueryString["Id"].ToString()));

                    try
                    {
                        StrLocationId = Request.QueryString["LocId"].ToString();
                    }
                    catch
                    {
                        StrLocationId = Session["LocId"].ToString();
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_List()", true);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Bin()", true);
                    //btnUnPost.Visible = true;
                    btnSInvCancel.Visible = true;
                }

                Session["DtAdvancePayment"] = null;

                //for get sales perso n name according login employee
                if (Session["EmpId"].ToString() != "0" && Request.QueryString["Id"] == null)
                {
                    DataTable Dt_Temp_Emp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
                    txtSalesPerson.Text = Dt_Temp_Emp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Temp_Emp.Rows[0]["Emp_Code"].ToString();
                }
                //code end



                //ImageButton im = new ImageButton();
                //im.CommandArgument="2";
                //im.Command += new CommandEventHandler(btnEdit_Command);

                //FillGridBin();
                fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
                CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();

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
                getPageControlsVisibility();
            }
            Lbl_Expenses_Tax_Amount_ET.Text = Expenses_Tax_Modal.Expenses_Amount_Value();
            Lbl_Expenses_Tax_ET.Text = Expenses_Tax_Modal.Expenses_Tax_Value();

            //txtExpExchangeRate.Enabled = false;
            if (ddlCurrency.SelectedIndex != 0)
            {
                strCurrencyId = ddlCurrency.SelectedValue;
            }
            //AllPageCode();
            if (Session["Is_Tax_Apply"] != null && Session["Is_Tax_Apply"].ToString() == "False")
                Trans_Div.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (IsPostBack && hdfCurrentRow.Value != string.Empty)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
            }
            //btn_New_Click(null, null);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }

    private void FillPageSizeddl()
    {
        ddlPageSize.Items.Add(new ListItem("10", "10"));
        ddlPageSize.Items.Add(new ListItem("20", "20"));
        ddlPageSize.Items.Add(new ListItem("30", "30"));
        ddlPageSize.Items.Add(new ListItem("40", "40"));
        ddlPageSize.Items.Add(new ListItem("50", "50"));
        ddlPageSize.Items.Add(new ListItem("60", "60"));
        ddlPageSize.Items.Add(new ListItem("70", "70"));
        ddlPageSize.Items.Add(new ListItem("80", "80"));
        ddlPageSize.Items.Add(new ListItem("90", "90"));
        ddlPageSize.Items.Add(new ListItem("100", "100"));
        int StrPageSize = int.Parse(Session["GridSize"].ToString());
        if (String.IsNullOrEmpty(StrPageSize.ToString()))
        {
            StrPageSize = 1;
        }
        else
        {
            if (StrPageSize % 10 != 0)
            {
                StrPageSize = StrPageSize + (10 - (StrPageSize % 10));
            }
        }
        ddlPageSize.SelectedValue = StrPageSize.ToString();
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

    protected void ddlPageSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());


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
                    SearchType = "Equal";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    SearchType = "Contains";
                }
                else
                {
                    SearchType = "Like";
                }
            }

            SearchField = ddlFieldName.SelectedValue.ToString();
            SearchValue = txtValue.Text.Trim();

            if (SearchField == "Trans_Id")
                SearchField = "Inv_SalesInvoiceHeader.Trans_Id";

            string PageSize = currentpagesize.ToString();
            DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, true.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);
            DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, false.ToString(), "1".ToString(), PageSize, SearchField, SearchType, SearchValue);

            _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());

            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

            GvSalesInvoice.DataSource = dt;
            GvSalesInvoice.DataBind();

            object sumObject;
            sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


            lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            generatePager(_TotalRowCount, int.Parse(PageSize), 1);
        }
    }


    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            bindGrid(Convert.ToInt32(e.CommandArgument));
        }
    }

    public void generatePager(int totalRowCount, int pageSize, int currentPage)
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

        dlPager.DataSource = pageLinkContainer;
        dlPager.DataBind();
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
            string DPageSize = ddlPageSize.SelectedValue.ToString();
            if (DPageSize == "")
                DPageSize = "0";
            if (int.Parse(DPageSize) > int.Parse(PageSize))
                PageSize = DPageSize;

            DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, true.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);
            DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, false.ToString(), currentPage.ToString(), PageSize, SearchField, SearchType, SearchValue);

            _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());

            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

            GvSalesInvoice.DataSource = dt;
            GvSalesInvoice.DataBind();

            object sumObject;
            sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


            lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            generatePager(_TotalRowCount, int.Parse(PageSize), currentPage);

        }
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
    //public void getPosName()
    //{
    //    string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });

    //    txtPOSNo.Text = computer_name[0].ToString();
    //}
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
                gvProduct.Columns[16].Visible = false;
                gvProduct.Columns[17].Visible = false;

                GridExpenses.Columns[6].Visible = false;
                GridExpenses.Columns[7].Visible = false;

                lblTaxP.Visible = false;
                // lblTaxPcolon.Visible = false;
                txtTaxP.Visible = false;
                Label2.Visible = false;
                txtTaxV.Visible = false;
                gridView.Visible = false;
                Trans_Div.Visible = false;
            }
            else
            {
                gvProduct.Columns[16].Visible = true;
                gvProduct.Columns[17].Visible = true;
                if (Session["LocId"].ToString() == "7")
                {
                    GridExpenses.Columns[6].Visible = true;
                    GridExpenses.Columns[7].Visible = true;
                }
                else
                {
                    GridExpenses.Columns[6].Visible = false;
                    GridExpenses.Columns[7].Visible = false;
                }
                lblTaxP.Visible = true;
                //lblTaxPcolon.Visible = true;
                txtTaxP.Visible = true;
                Label2.Visible = true;
                txtTaxV.Visible = true;
                gridView.Visible = false;
                Trans_Div.Visible = true;
            }
        }

        Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
            {
                gvProduct.Columns[14].Visible = false;
                gvProduct.Columns[15].Visible = false;
                lblDiscountP.Visible = false;
                //lblDiscountPcolon.Visible = false;
                txtDiscountP.Visible = false;
                Label3.Visible = false;
                txtDiscountV.Visible = false;
                lblPriceafterdiscountheader.Visible = false;
                //lblcolonDiscountheader.Visible = false;
                txtPriceafterdiscountheader.Visible = false;

            }
            else
            {
                gvProduct.Columns[14].Visible = true;
                gvProduct.Columns[15].Visible = true;
                lblDiscountP.Visible = true;
                //lblDiscountPcolon.Visible = true;
                txtDiscountP.Visible = true;
                Label3.Visible = true;
                txtDiscountV.Visible = true;
                lblPriceafterdiscountheader.Visible = true;
                //lblcolonDiscountheader.Visible = true;
                txtPriceafterdiscountheader.Visible = true;
            }
        }



        //here we checking scanning solution parameter i stru eor not


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
        btnSInvSave.Visible = clsPagePermission.bAdd;
        Btn_Add_Expenses.Visible = clsPagePermission.bAdd;
        Btn_Exp_Reset.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        GvCustomerInquiry.Columns[0].Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        GridExpenses.Columns[0].Visible = clsPagePermission.bDelete;
        gvPayment.Columns[0].Visible = clsPagePermission.bDelete;
        txtSInvDate.Enabled = clsPagePermission.bModifyDate;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnSaveExcelInvoice.Visible = clsPagePermission.bAdd;
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "13", "92", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #region System defined Function


    public void setSymbol()
    {
        try
        {
            gvProduct.Columns[13].HeaderText = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Gross_Price, Session["DBConnection"].ToString());
            gvProduct.Columns[15].HeaderText = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Value, Session["DBConnection"].ToString());
            gvProduct.Columns[17].HeaderText = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Value, Session["DBConnection"].ToString());
        }
        catch
        { }
        lblAmount.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Gross_Amount, Session["DBConnection"].ToString());
        Label3.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Value, Session["DBConnection"].ToString());
        Label2.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Value, Session["DBConnection"].ToString());
        lblGrandTotal.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
        Label11.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Total_Expenses, Session["DBConnection"].ToString());
        Label12.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Net_Amount, Session["DBConnection"].ToString());
    }
    protected void btnUnPost_Click(object sender, EventArgs e)
    {
        FillGridBin();
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }
        string objSenderID;
        if (sender is Button)
        {
            Button b = (Button)sender;
            objSenderID = b.ID;
        }
        else
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }
        DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, e.CommandArgument.ToString());
        //transport Data Get
        DataTable dtTrnsPort = objDa.return_DataTable("SELECT * FROM Inv_InvoiceTransport where Ref_Id='" + e.CommandArgument.ToString() + "' And Ref_Type='SI'");
        if (dtTrnsPort.Rows.Count > 0)
        {
            if (dtTrnsPort.Rows[0]["Customer_Id"].ToString() != "0")
            {
                txtcustomername.Text = GetCustomerNameByCustomerId(dtTrnsPort.Rows[0]["Customer_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Customer_Id"].ToString();

            }
            if (dtTrnsPort.Rows[0]["Vehicle_Id"].ToString() != "0")
            {
                txtvehiclename.Text = GetvechileNameByVechileId(dtTrnsPort.Rows[0]["Vehicle_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Vehicle_Id"].ToString();

            }
            if (dtTrnsPort.Rows[0]["Vehicle_Id"].ToString() != "0")
            {
                txtdrivername.Text = GetDriverNamebyDriverId(dtTrnsPort.Rows[0]["Vehicle_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Driver_Id"].ToString();
            }
            txtChargableAmount.Text = dtTrnsPort.Rows[0]["Chargable_Amount"].ToString();
            txtdescription.Text = dtTrnsPort.Rows[0]["Description"].ToString();
            txtPermanentMobileNo.Text = dtTrnsPort.Rows[0]["Contact_No"].ToString();
            txtAreaName.Text = dtTrnsPort.Rows[0]["Field1"].ToString();
            txtPersonName.Text = dtTrnsPort.Rows[0]["Field2"].ToString();
            txtPersonMobileNo.Text = dtTrnsPort.Rows[0]["Field3"].ToString();
            txtTrakingId.Text = dtTrnsPort.Rows[0]["Field4"].ToString();
            txtVisitDate.Text = Convert.ToDateTime(dtTrnsPort.Rows[0]["Visit_Date"].ToString()).ToString("dd-MMM-yyyy");
            string strhour = dtTrnsPort.Rows[0]["Visit_Time"].ToString().Split(':')[0].ToString();
            string strminute = dtTrnsPort.Rows[0]["Visit_Time"].ToString().Split(':')[1].ToString();
            txtVisitTime.Text = strhour + ":" + strminute;
            if (txtdrivername.Text != "")
            {
                chkCustomer.Checked = false;
                chkEmployee.Checked = true;
                ChkTrans_Changed(null, null);
            }
        }
        try
        {
            if (objSenderID != "lnkViewDetail")
            {
                if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
                {
                    DisplayMessage("Sales Invoice has posted,can not be Update");
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
                        if (dtInvEdit.Rows[0]["Field4"].ToString() == "Approved")
                        {
                            //cmn.DisableControls(this.Page, false);
                            btnSInvCancel.Enabled = true;
                            btnPost.Enabled = true;
                            btnSInvSave.Enabled = true;
                        }

                    }

                }//End Approval Code
            }
            else
            {
                btnPost.Enabled = false;
                btnSInvSave.Enabled = false;
            }
        }
        catch
        {

        }
        editid.Value = e.CommandArgument.ToString();
        bool IsTax = IsApplyTax();
        if (IsTax == true)
        {
            Dt_Final_Save_Tax = null;
            Dt_Final_Save_Tax = new DataTable();
            Dt_Final_Save_Tax.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
            Dt_Final_Save_Tax = objTaxRefDetail.Get_Tax_Detail_For_Expenses("SINV", editid.Value, "Sales_Invoice", "Multiple", "1");
            Session["Expenses_Tax_Sales_Invoice"] = Dt_Final_Save_Tax;
        }
        DataTable dtSerial = new DataTable();
        dtSerial.Columns.Add("Product_Id");
        dtSerial.Columns.Add("SerialNo");
        dtSerial.Columns.Add("SOrderNo");
        dtSerial.Columns.Add("BarcodeNo");
        dtSerial.Columns.Add("BatchNo");
        dtSerial.Columns.Add("LotNo");
        dtSerial.Columns.Add("ExpiryDate");
        dtSerial.Columns.Add("ManufacturerDate");
        DataTable dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value);
        if (dtStock.Rows.Count > 0)
        {
            dtStock = new DataView(dtStock, "TransType='SI' and TransTypeId=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            for (int i = 0; i < dtStock.Rows.Count; i++)
            {
                DataRow dr = dtSerial.NewRow();
                dr["Product_Id"] = dtStock.Rows[i]["ProductId"].ToString();
                dr["SerialNo"] = dtStock.Rows[i]["SerialNo"].ToString();
                dr["SOrderNo"] = dtStock.Rows[i]["Field1"].ToString();
                dr["BarcodeNo"] = dtStock.Rows[i]["Barcode"].ToString(); ;
                dr["BatchNo"] = dtStock.Rows[i]["BatchNo"].ToString(); ;
                dr["LotNo"] = dtStock.Rows[i]["LotNo"].ToString(); ;
                dr["ExpiryDate"] = Convert.ToDateTime(dtStock.Rows[i]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                dr["ManufacturerDate"] = Convert.ToDateTime(dtStock.Rows[i]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                dtSerial.Rows.Add(dr);
            }
        }
        ViewState["dtFinal"] = dtSerial;
        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            //btn_New_Click(null, null);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        if (objSenderID == "btnEdit")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        if (dtInvEdit.Rows.Count > 0)
        {

            hdnOrderId.Value = dtInvEdit.Rows[0][30].ToString();
            DataTable dtRefDetailHeader = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SINV", editid.Value);
            try
            {
                dtRefDetailHeader = new DataView(dtRefDetailHeader, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            dtRefDetailHeader = dtRefDetailHeader.DefaultView.ToTable(true, "Tax_Id", "TaxName", "Tax_Per", "Tax_Value");
            ViewState["dtTaxHeader"] = dtRefDetailHeader;
            LoadStores();
            txtSInvNo.Text = dtInvEdit.Rows[0]["Invoice_No"].ToString();
            try
            {
                ViewState["TimeStamp"] = dtInvEdit.Rows[0]["Row_Lock_Id"].ToString();
            }
            catch
            {
            }

            if (!String.IsNullOrEmpty(dtInvEdit.Rows[0]["Condition4"].ToString()))
                ddlTransType.SelectedValue = dtInvEdit.Rows[0]["Condition4"].ToString();

            txtSInvNo.ReadOnly = true;
            //Address code

            try
            {
                DataTable dtShipmentDetail = objDa.return_DataTable("Select   * From Inv_SalesInvoiceHeader_Extra  Where Invoice_No = '" + editid.Value + "'");
                if (dtShipmentDetail.Rows.Count > 0)
                {
                    txtShipmentRef.Text = dtShipmentDetail.Rows[0]["Shipment_Id"].ToString();
                }
            }
            catch
            {

            }
            string strAddressId = dtInvEdit.Rows[0]["Field1"].ToString();
            DataTable dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, StrCompId);
            if (dtAddName.Rows.Count > 0)
            {
                txtInvoiceTo.Text = dtAddName.Rows[0]["Address_Name"].ToString();
            }
            else
            {
                txtInvoiceTo.Text = "";
            }
            strAddressId = dtInvEdit.Rows[0]["Field3"].ToString();
            dtAddName = null;
            dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, StrCompId);
            if (dtAddName.Rows.Count > 0)
            {
                txtShipingAddress.Text = dtAddName.Rows[0]["Address_Name"].ToString();
            }
            else
            {
                txtShipingAddress.Text = "";
            }
            string strShipCustomerId = dtInvEdit.Rows[0]["Field2"].ToString();
            if (strShipCustomerId != "" && strShipCustomerId != "0")
            {
                DataTable dtShipCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strShipCustomerId + "'");
                if (dtShipCustomerName != null && dtShipCustomerName.Rows.Count > 0)
                {
                    string strShipCustomerEmail = dtShipCustomerName.Rows[0]["Field1"].ToString();
                    string strShipCustomerNumber = dtShipCustomerName.Rows[0]["Field2"].ToString();
                    txtShipCustomerName.Text = objContact.GetContactNameByContactiD(strShipCustomerId) + "/" + strShipCustomerNumber + "/" + strShipCustomerEmail + "/" + strShipCustomerId;
                    //txtShipCustomerName.Text = dtInvEdit.Rows[0]["ShipCustomerName"].ToString() + "/" + strShipCustomerId;
                }
            }
            //end address code
            txtSInvDate.Text = Convert.ToDateTime(dtInvEdit.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            string strSOFromTransType = dtInvEdit.Rows[0]["SIFromTransType"].ToString();

            if (strSOFromTransType.Trim() == "J" || strSOFromTransType.Trim() == "W")
            {
                hdnTransType.Value = strSOFromTransType.Trim();
                hdnTransTypeValue.Value = dtInvEdit.Rows[0]["SIFromTransNo"].ToString();
                strSOFromTransType = "D";
            }
            else
            {
                hdnTransType.Value = "";
            }





            ViewState["ApprovalStatus"] = dtInvEdit.Rows[0]["Field4"].ToString();
            string strCustomerId = dtInvEdit.Rows[0]["Supplier_Id"].ToString();
            strSiCustomerId = int.Parse(strCustomerId);
            txtInvoiecRefno.Text = dtInvEdit.Rows[0]["Invoice_Ref_No"].ToString();
            try
            {
                if (dtInvEdit.Rows[0]["Invoice_Merchant_Id"].ToString() != "0" && dtInvEdit.Rows[0]["Invoice_Merchant_Id"].ToString() != "")
                {
                    ddlMerchant.SelectedValue = dtInvEdit.Rows[0]["Invoice_Merchant_Id"].ToString();
                }
                else
                {
                    ddlMerchant.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlMerchant.SelectedIndex = 0;
            }

            if (dtInvEdit.Rows[0]["Condition3"].ToString() != "")
            {

                Session["JobCardId"] = dtInvEdit.Rows[0]["Condition3"].ToString();
            }


            txtOrderId.Text = dtInvEdit.Rows[0]["Ref_Order_Number"].ToString();

            DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
            if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
            {
                string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                txtCustomer.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                //txtCustomer.Text = dtInvEdit.Rows[0]["CustomerName"].ToString() + "/" + strCustomerId;
            }

            Session["ContactID"] = strCustomerId;
            hdnContactId.Value = dtInvEdit.Rows[0]["contactId"].ToString();
            txtContactPerson.Text = dtInvEdit.Rows[0]["contactName"].ToString() + "/" + dtInvEdit.Rows[0]["contact_number"].ToString() + "/" + dtInvEdit.Rows[0]["contact_email"].ToString() + "/" + dtInvEdit.Rows[0]["contactId"].ToString();

            if (strCustomerId != "0" && strCustomerId != "")
            {
                GetCreditInfo();
                DataTable dtCust = ObjCustmer.GetCustomerAllDataByCustomerId(StrCompId, StrBrandId, strCustomerId);
                if (dtCust.Rows.Count > 0)
                {
                    Session["CustomerAccountId"] = dtCust.Rows[0]["Account_No"].ToString();
                }
            }
            DataTable dtRefDetail = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SINV", editid.Value);
            try
            {
                dtRefDetail = new DataView(dtRefDetail, "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            dtRefDetail = dtRefDetail.DefaultView.ToTable(true, "ProductId", "ProductCategoryId", "CategoryName", "TaxName", "Tax_Per", "Tax_value", "TaxSelected", "Tax_Id", "SO_Id");
            ViewState["DtTax"] = dtRefDetail;

            DataTable dtdetail = new DataTable();
            dtdetail = objSInvDetail.GetSInvDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, editid.Value, Session["FinanceYearId"].ToString());


            if (strSOFromTransType == "S")
            {
                RdoSo.Checked = true;
                RdoWithOutSo.Checked = false;
                DataTable dtsalesOrder = fillSOSearhgrid();
                //txtCustomer.Enabled = false;

                ViewState["dtPo"] = dtdetail;
                ViewState["Dtproduct"] = dtsalesOrder;
                if (ddlProductSerach.Items.FindByText("Sales Order No") == null)
                {
                    ddlProductSerach.Items.Insert(2, new ListItem("Sales Order No", "SalesOrderNo"));
                }

                DataTable dtadvancepayment = dtdetail.Copy();

                dtadvancepayment = dtadvancepayment.DefaultView.ToTable(true, "SoID", "SalesOrderNo");
                foreach (DataRow dr in dtadvancepayment.Rows)
                {
                    FillAdvancePayment_BYOrderId(dr["SoID"].ToString(), dr["SalesOrderNo"].ToString());
                }
                rbtNew.Visible = false;
                rbtEdit.Visible = false;
            }
            if (strSOFromTransType == "D")
            {
                rbtNew.Visible = true;
                rbtEdit.Visible = true;
                try
                {
                    ddlProductSerach.Items.RemoveAt(2);
                }
                catch
                {

                }

                RdoSo.Checked = false;
                RdoWithOutSo.Checked = true;
                ViewState["dtPo"] = null;

                ViewState["Dtproduct"] = dtdetail;
            }
            rbtNew.Checked = false;
            rbtEdit.Checked = true;
            Get_Tax_From_DB();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)gvProduct, dtdetail, "", "");
            GridCalculation();
            RdoSOandWithSO();
            RdoSo.Enabled = false;
            RdoWithOutSo.Enabled = false;
            try
            {
                ddlPaymentMode.SelectedValue = dtInvEdit.Rows[0]["PaymentModeId"].ToString();
            }
            catch
            {
                ddlPaymentMode.SelectedIndex = 0;
            }
            fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
            ddlCurrency.SelectedValue = dtInvEdit.Rows[0]["Currency_Id"].ToString();

            strCurrencyId = ddlCurrency.SelectedValue;
            ddlExpCurrency.SelectedValue = ddlCurrency.SelectedValue;

            txtExchangeRate.Text = dtInvEdit.Rows[0]["Field5"].ToString();
            txtPaymentExchangerate.Text = dtInvEdit.Rows[0]["Field5"].ToString();
            if (txtExchangeRate.Text == "")
            {
                txtExchangeRate.Text = "0";
            }
            string strEmployeeId = dtInvEdit.Rows[0]["SalesPerson_Id"].ToString();
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strEmployeeId);
            txtSalesPerson.Text = dtInvEdit.Rows[0]["EmployeeName"].ToString() + "/" + Emp_Code;
            txtPOSNo.Text = dtInvEdit.Rows[0]["PosNo"].ToString();
            txtAccountNo.Text = dtInvEdit.Rows[0]["Account_No"].ToString();
            txtInvoiceCosting.Text = dtInvEdit.Rows[0]["Invoice_Costing"].ToString();
            txtShift.Text = dtInvEdit.Rows[0]["Shift"].ToString();
            txtTender.Text = dtInvEdit.Rows[0]["Tender"].ToString();
            txtTotalQuantity.Text = SetDecimal(dtInvEdit.Rows[0]["TotalQuantity"].ToString());

            txtAmount.Text = SetDecimal(dtInvEdit.Rows[0]["TotalAmount"].ToString());

            txtTaxP.Text = SetDecimal(dtInvEdit.Rows[0]["NetTaxP"].ToString());
            txtTaxV.Text = Convert_Into_DF(dtInvEdit.Rows[0]["NetTaxV"].ToString());
            txtNetAmount.Text = SetDecimal(dtInvEdit.Rows[0]["NetAmount"].ToString());

            txtDiscountP.Text = SetDecimal(dtInvEdit.Rows[0]["NetDiscountP"].ToString());
            txtDiscountP.Text = SetDecimal(txtDiscountP.Text);
            txtDiscountV.Text = SetDecimal(dtInvEdit.Rows[0]["NetDiscountV"].ToString());

            txtPriceafterdiscountheader.Text = SetDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString());
            txtGrandTotal.Text = SetDecimal(dtInvEdit.Rows[0]["GrandTotal"].ToString());

            txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
            try
            {
                txtCondition1.Content = dtInvEdit.Rows[0]["Condition1"].ToString();
            }
            catch
            {

            }

            if (txtCondition1.Content.Trim() == "")
            {
                txtCondition1.Content = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();

            }

            fillExpGrid(ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value.ToString(), "SI"));

            try
            {

                txtTotalExpensesAmount.Text = ((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text;


                txtTotalExpensesAmount.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString());
            }
            catch
            {
                txtTotalExpensesAmount.Text = "0";
            }
            //txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
            txtNetAmountwithexpenses.Text = SetDecimal(dtInvEdit.Rows[0]["condition2"].ToString());
            fillPaymentGrid(ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "SI", editid.Value.ToString()));



            txtRemark.Text = dtInvEdit.Rows[0]["Remark"].ToString();
            try
            {
                chkPost.Checked = Convert.ToBoolean(dtInvEdit.Rows[0]["Post"].ToString());
            }
            catch
            {

            }

            if (ddlCurrency.SelectedValue == SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()))
            {
                txtPaymentExchangerate.Enabled = false;
            }
            else
            {
                txtPaymentExchangerate.Enabled = true;
            }
            //HeadearCalculateGrid();
        }

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
        TaxandDiscountParameter();
        if (objSenderID == "lnkViewDetail")
        {
            btnSInvSave.Enabled = false;
            BtnReset.Enabled = false;
            btnPost.Enabled = false;

        }
        else
        {
            btnSInvSave.Enabled = true;
            BtnReset.Enabled = true;
            btnPost.Enabled = true;
        }
        LoadStores();

        setSymbol();
        Application.Lock();
        if (gvProduct.Rows.Count > 0)
        {
            ddlTransType.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
        }

        txtInvoiceTo_TextChanged(null, null);
        txtShipingAddress_TextChanged(null, null);
        txtShipTo_TextChanged(null, null);

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
            string DPageSize = ddlPageSize.SelectedValue.ToString();
            if (DPageSize == "")
                DPageSize = "0";
            if (int.Parse(DPageSize) > int.Parse(PageSize))
                PageSize = DPageSize;

            DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, true.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);
            DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), PostType, false.ToString(), "1", PageSize, SearchField, SearchType, SearchValue);


            //DataTable dtAdd = (DataTable)Session["dtSInvoice"];
            //DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);

            GvSalesInvoice.DataSource = dt;
            GvSalesInvoice.DataBind();

            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";
            //objPageCmn.FillData((object)GvSalesInvoice, view.ToTable(), "", "");
            //Session["dtFilter_Sale_inv_mstr"] = view.ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            // //AllPageCode();

            object sumObject;
            sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


            lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            int _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());
            generatePager(_TotalRowCount, int.Parse(PageSize), 1);
        }


        //AllPageCode();

        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtCustValue.Text != "")
            txtCustValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvSalesInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Sale_inv_mstr"];
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
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");

        //AllPageCode();
    }

    protected void GvSalesInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesInvoice.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Sale_inv_mstr"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
        //AllPageCode();
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
    protected void btnSInvCancel_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        //cmn.DisableControls(this.Page, true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
    }
    protected void btnSInvSave_Click(object sender, EventArgs e)
    {
        if (rbtNew.Checked == true)
        {
            txtSInvNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtSInvNo.Text;
            hdnCanEdit.Value = "";
            editid.Value = "";
            //ddlOrderType.SelectedValue = "D";
        }
        if (txtSInvDate.Text == "")
        {
            DisplayMessage("Enter Sales Invoice Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvDate);
            //AllPageCode();
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            ViewState["OnSave"] = null;
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtSInvDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Sales Invoice Date in format " + Session["DateFormat"].ToString() + "");
                txtSInvDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvDate);
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                ViewState["OnSave"] = null;
                return;
            }
        }

        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtSInvDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        //first we check validation from customer setup and approval system 

        ViewState["OnSave"] = "Processing..";

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string AvgCost = string.Empty;
        if (sender is Button)
        {
            Button btnId = (Button)sender;

            if (btnId.ID == "btnPost")
            {
                chkPost.Checked = true;
            }
            if (btnId.ID == "btnSInvSave")
            {
                chkPost.Checked = false;
            }
        }

        //this code for check stock fro serialbe product

        //code start


        //modify by jitendra upadhyay for delivery voucher concept
        //here validation not require because stock deducted on delivery voucher page so
        //modify on 30/12/2015
        if (chkPost.Checked)
        {
            foreach (GridViewRow gvRow in gvProduct.Rows)
            {
                if (RdoWithOutSo.Checked || objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvRow.FindControl("lblSOId")).Text).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                {
                    if (objProductM.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ((HiddenField)gvRow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString().Trim().ToUpper() == "S")
                    {
                        if (((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text == "")
                        {
                            ((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text = "0";
                        }

                        if (Convert.ToDouble(((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text) > Convert.ToDouble(((Label)gvRow.FindControl("lblgvSystemQuantity")).Text))
                        {
                            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                            DisplayMessage("Invoice quantity should be less than or equal to system quantity for " + ((Label)gvRow.FindControl("lblgvProductName")).Text + " product");
                            ViewState["OnSave"] = null;
                            return;
                        }
                    }
                }
            }
        }

        hdnContactId.Value = hdnContactId.Value == "" ? "0" : hdnContactId.Value;

        string strPaymentMode = string.Empty;
        string InvoiceId = string.Empty;
        string InvoiceEditId = string.Empty;
        InvoiceEditId = editid.Value;


        strPaymentMode = ddlPaymentMode.SelectedValue;
        if (txtSInvNo.Text == "")
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Enter Sales Invoice No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);

            return;
        }
        else
        {
            if (editid.Value == "")
            {
                DataTable dtSQNo = objSInvHeader.GetSInvHeaderAllByInvoiceNo(StrCompId, StrBrandId, StrLocationId, txtSInvNo.Text);
                if (dtSQNo.Rows.Count > 0)
                {
                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                    DisplayMessage("Sales Invoice No. Already Exists");

                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);

                    return;
                }
            }
        }


        if (ddlCurrency.Text == "--Select--")
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Currency Required On Company Level");
            ddlCurrency.Focus();

            return;
        }

        if (txtSalesPerson.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            if (Emp_ID != "")
            {

            }
            else
            {
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                txtSalesPerson.Text = "";
                DisplayMessage("Sales Person Choose In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);

                return;
            }
        }
        else
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Enter Sales Person");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);

            return;
        }

        string strCustomerId = string.Empty;
        if (txtCustomer.Text != "")
        {
            if (GetCustomerId() != "")
            {

            }
            else
            {
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                txtCustomer.Text = "";
                DisplayMessage("Customer Choose In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomer);

                return;
            }
        }
        else
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
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

        if (strAddressId2 == "0")
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Please select invoice address");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceTo);
            return;
        }

        if (txtShipCustomerName.Text != "")
        {
            if (GetCustomerId(txtShipCustomerName) == "")
            {
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                txtShipCustomerName.Text = "";
                DisplayMessage("Ship To Choose In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipCustomerName);

                return;
            }
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


        if (strAddressId == "0")
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Please shipping invoice address");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
            return;
        }


        if ((RdoSo.Checked == true) || (RdoWithOutSo.Checked = true))
        {
            if (gvProduct.Rows.Count > 0)
            {

            }
            else
            {
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                DisplayMessage("You have no Product For Generate Sales Invoice");

                return;
            }
        }
        else
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Select Invoice Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlOrderType);

            return;
        }

        string strPost = string.Empty;
        if (chkPost.Checked == true)
        {
            strPost = "True";
        }
        else if (chkPost.Checked == false)
        {
            strPost = "False";
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
            txtTaxV.Text = Convert_Into_DF("0");
        }
        if (txtTaxP.Text == "NaN")
        {
            txtTaxP.Text = "0";
        }
        if (txtDiscountP.Text == "")
        {
            txtDiscountP.Text = "0";
        }
        if (txtDiscountV.Text == "")
        {
            txtDiscountV.Text = "0";
        }
        if (txtGrandTotal.Text == "")
        {
            txtGrandTotal.Text = "0";
        }
        if (txtInvoiceCosting.Text == "")
        {
            txtInvoiceCosting.Text = "0";
        }
        if (txtNetAmount.Text == "")
        {
            txtNetAmount.Text = "0";
        }
        if (txtTender.Text == "")
        {
            txtTender.Text = "0";
        }


        //here we are checking below cost Price if parameter is enable
        //code created by jitendra upadhyay on 05-10-2016
        //code start



        if (!Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Sales Below Cost Price").Rows[0]["ParameterValue"].ToString()))
        {
            Double AverageCost = 0;
            Double SalesPrice = 0;
            Double CurrencyRate = 0;

            try
            {
                CurrencyRate = Convert.ToDouble(txtExchangeRate.Text);
            }
            catch
            {
                CurrencyRate = 1;
            }
            foreach (GridViewRow gvrow in gvProduct.Rows)
            {
                try
                {
                    SalesPrice = Convert.ToDouble(((TextBox)gvrow.FindControl("lblgvUnitPrice")).Text);
                }
                catch
                {
                    SalesPrice = 0;
                }

                SalesPrice = SalesPrice / CurrencyRate;




                try
                {
                    AverageCost = Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value).Rows[0]["Field2"].ToString());
                }
                catch
                {
                    AverageCost = 0;
                }

                if (SalesPrice < AverageCost)
                {
                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                    DisplayMessage("Sales Price should be equal or greater than cost price for product Code = " + ((Label)gvrow.FindControl("lblgvProductCode")).Text);
                    return;
                }

            }
        }

        //for payment information
        if (gvadvancepayment.Rows.Count == 0 && gvPayment.Rows.Count == 0)
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("add payment information");
            TabContainer1.ActiveTabIndex = 0;

            return;
        }





        string TransType = string.Empty;

        int counter = 0;
        foreach (GridViewRow Gvr in gvProduct.Rows)
        {
            TextBox txtgvSalesQuantity = (TextBox)Gvr.FindControl("txtgvSalesQuantity");
            if (txtgvSalesQuantity.Text == "")
            {
                txtgvSalesQuantity.Text = "0";
            }
            if (Convert.ToDouble(txtgvSalesQuantity.Text) > 0)
            {
                counter = 1;
                break;
            }
        }

        if (counter == 0)
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Enter Sales Quantity");

            return;
        }



        if (RdoSo.Checked == true)
        {
            TransType = "S";

        }

        else if (RdoWithOutSo.Checked == true)
        {
            TransType = "D";
            if (hdnTransType.Value.Trim() != "")
            {
                TransType = hdnTransType.Value.Trim();
                hdnSalesOrderId.Value = hdnTransTypeValue.Value;
            }
        }


        //here we update status in job card if invoice created against  the job card

        //code created on 04-02-2017

        //code start

        string JobCardId = string.Empty;
        JobCardId = "0";
        if (Session["JobCardId"] != null)
        {

            if (Session["JobCardId"].ToString() != "")
            {
                JobCardId = Session["JobCardId"].ToString();
            }
        }
        //code end


        //here we check that payment amount and invoice amount should be equal or not
        //if (strPost == "True")
        //{
        double Invoiceamt = 0;
        double Paymentamt = 0;
        double advancepayment = 0;
        if (txtNetAmountwithexpenses.Text != "")
        {
            Invoiceamt = Convert.ToDouble(SetDecimal(txtNetAmountwithexpenses.Text));
        }
        if (gvPayment.Rows.Count > 0)
        {
            Paymentamt = Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text);
        }


        if (Invoiceamt != Paymentamt)
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            DisplayMessage("Payment Amount should be equal to invoice amount");
            return;
        }
        //}


        //here we are checking Payment Mode and Credit Amount as per current invoice - Neelkanth purohit 16-08-2018
        //--------------start-------------------
        double creditAmount = 0;

        DataTable dtPayMaster = ObjPaymentMaster.GetPaymentModeMaster(StrCompId, Session["BrandId"].ToString(), Session["LocId"].ToString());
        DataTable dtPayTrns = (DataTable)ViewState["PayementDt"];
        var JoinResult = (from p in dtPayMaster.AsEnumerable()
                          join t in dtPayTrns.AsEnumerable()
                          on (p.Field<int>("Pay_Mode_Id")).ToString() equals (t.Field<int>("PaymentModeId")).ToString()
                          select new
                          {
                              PaymentMode = p.Field<string>("field1"),
                              Amount = t.Field<decimal>("FCPayAmount"),
                          }).ToList();
        DataTable dtPayByMaster = cmn.ListToDataTable(JoinResult);
        if (dtPayByMaster.Rows.Count > 1)
        {
            //strPaymentMode = "Multi";
            strPaymentMode = "0"; //here i set 0 bcz in database field type is int
            var ab = dtPayByMaster.AsEnumerable()
                .Where(x => x.Field<string>("PaymentMode") == "Credit")
                .Sum(x => Convert.ToDouble(x["Amount"]));
            double.TryParse(ab.ToString(), out creditAmount);
        }
        else
        {
            if (dtPayByMaster.Rows[0]["PaymentMode"].ToString() == "Credit")
            {
                double.TryParse(dtPayByMaster.Rows[0]["Amount"].ToString(), out creditAmount);
            }
            strPaymentMode = "0"; //here i set 0 bcz in database field type is int
        }
        dtPayByMaster.Dispose();
        dtPayMaster.Dispose();
        dtPayTrns.Dispose();
        //------------

        //Here we are checking in case of credit invoice, Customer Account exist or his credit limit - Neelkanth Purohit-01-Sep-2018
        string strOtherAccountId = "0";
        if (creditAmount > 0)
        {
            if (ddlMerchant.SelectedValue != "0" && ddlMerchant.SelectedItem.Text.ToString().ToLower() != "direct")
            {
                using (DataTable _dtMerchant = objMerchantMaster.GetMerchantMasterById(ddlMerchant.SelectedValue))
                {
                    if (_dtMerchant.Rows.Count > 0)
                    {
                        int mContactId = 0;
                        int.TryParse(_dtMerchant.Rows[0]["field1"].ToString(), out mContactId);
                        if (mContactId == 0)
                        {
                            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                            DisplayMessage("Merchant is not linked with customer");
                            return;
                        }
                        else
                        {
                            strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(mContactId.ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
                        }
                    }
                }
            }
            else
            {
                strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(txtCustomer.Text.Split('/')[3].ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
            }

            if (strOtherAccountId == "0")
            {
                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                setDefaultValueForUcAcMaster();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
                //DisplayMessage("Account Detail no exist for this customer, Pleae first create Account");
                return;
            }

            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
            {
                string[] _result = objCustomerCreditParam.checkCreditLimit(editid.Value == "" ? 0 : int.Parse(editid.Value), creditAmount, txtCustomer.Text.Split('/')[3].ToString(), strOtherAccountId, txtSInvDate.Text, ddlCurrency.SelectedValue, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
                if (_result[0] == "false")  //array index 0 - return true/false and 1 - return message
                {
                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                    DisplayMessage(_result[1]);
                    return;
                }
            }
        }
        //----------------------------end----------------



        //if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
        //{
        //    if (creditAmount > 0)
        //    {

        //if (txtCustomer.Text == "")
        //{
        //    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //    DisplayMessage("Enter Customer Name");
        //    return;
        //}
        //else
        //{
        //    //DataTable dtCustomerEdit = ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[1].ToString());
        //    //bool IsCredit;
        //    //try
        //    //{
        //    //    IsCredit = Convert.ToBoolean(dtCustomerEdit.Rows[0]["Field41"].ToString());
        //    //}
        //    //catch
        //    //{
        //    //    IsCredit = false;
        //    //}

        //    //string Status = dtCustomerEdit.Rows[0]["Field51"].ToString();


        //    DataTable dtCustomerEdit = ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[1].ToString());
        //    bool IsCredit;
        //    string Status = string.Empty;
        //    try
        //    {
        //        IsCredit = double.Parse(dtCustomerEdit.Rows[0]["Credit_Limit"].ToString())>0?true:false;
        //        Status = dtCustomerEdit.Rows[0]["field4"].ToString();
        //    }
        //    catch
        //    {
        //        IsCredit = false;
        //    }

        //    if (!IsCredit)
        //    {
        //        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //        DisplayMessage("This is Cash Customer ,you can not generate credit invoice");
        //        return;
        //    }

        //    if (Status == "Pending")
        //    {
        //        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //        DisplayMessage("Customer credit request is pending. ,you can not generate invoice");
        //        return;
        //    }
        //    if (Status == "Rejected")
        //    {
        //        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //        DisplayMessage("Customer credit request is Rejected ,you can not generate invoice");
        //        return;
        //    }


        //    DataTable dtAccountsummary = new DataTable();

        //    DataTable dtCreditParameter = objCustomerCreditParam.GetCustomerRecord_By_OtherAccountId(strOtherAccountId);

        //    //here c are reference for customer record
        //    //dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

        //    if (dtCreditParameter.Rows.Count > 0)
        //    {
        //        double LocalInvoiceAmt = 0;
        //        double TotalUnpostInvoicesum = 0;
        //        double CreditLimit = 0;
        //        int CreditDays = 0;
        //        double AdvacnechequeAmount = 0;
        //        double closingBalance = 0;
        //        double dueamount = 0;
        //        double ActualCreditLimit = 0;
        //        double advanceamt = 0;

        //        CreditLimit = Convert.ToDouble(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
        //        CreditDays = Convert.ToInt32(dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim());

        //        //calculation for get unposted invoice sum
        //        //here we added validation for post all unposted invoice for current customer for get updated customer balance
        //        DataTable dtUnPostInvoice = new DataTable();
        //        string sqlUnPostedInvoice = string.Empty;
        //        if (editid.Value == "")
        //        {
        //            sqlUnPostedInvoice = "select SUM(pay.Pay_Charges) as LAmount,SUM(pay.FCPayAmount) as FAmount from inv_salesinvoiceheader SIH left join (select TransNo, FCPayAmount, Pay_Charges, PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans = 'SI')pay on pay.TransNo = SIH.Trans_Id inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id = pay.PaymentModeId where SIH.Post = '0' and Set_Payment_Mode_Master.Field1 = 'Credit' and sih.Location_Id = " + Session["LocId"].ToString() + " and SIH.Supplier_Id = '" + txtCustomer.Text.Split('/')[1].ToString() + "'";
        //        }
        //        else
        //        {
        //            sqlUnPostedInvoice = "select SUM(pay.Pay_Charges) as LAmount,SUM(pay.FCPayAmount) as FAmount from inv_salesinvoiceheader SIH left join (select TransNo, FCPayAmount, Pay_Charges, PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans = 'SI')pay on pay.TransNo = SIH.Trans_Id inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id = pay.PaymentModeId where SIH.Post = '0' and Set_Payment_Mode_Master.Field1 = 'Credit' and sih.Location_Id = " + Session["LocId"].ToString() + " and SIH.Supplier_Id = '" + txtCustomer.Text.Split('/')[1].ToString() + "' and SIH.trans_id='" + editid.Value + "'";
        //        }
        //        dtUnPostInvoice = objDa.return_DataTable(sqlUnPostedInvoice);

        //        Double UnpostedInvoiceAmt = 0;
        //        Double SumInvoice = 0;

        //        //here we are getting sum of unposted invoice 
        //        if (dtUnPostInvoice.Rows.Count>0)
        //        {
        //            Double.TryParse(dtUnPostInvoice.Rows[0]["FAmount"].ToString(), out UnpostedInvoiceAmt);
        //            SumInvoice += UnpostedInvoiceAmt;
        //        }


        //        string strsql = "select  dbo.Ac_GetBalance(" + Session["CompId"].ToString() + "," + Session["BrandId"].ToString() + "," + Session["LocId"].ToString() + ",'" + DateTime.Now.ToString() + "',0,7," + txtCustomer.Text.Split('/')[1].ToString() + ",2,'" + Session["FinanceYearId"].ToString() + "')";
        //        dtAccountsummary = objDa.return_DataTable(strsql);

        //        if (dtAccountsummary.Rows.Count > 0)
        //        {
        //            try
        //            {
        //                closingBalance = Convert.ToDouble(dtAccountsummary.Rows[0][0].ToString()) + SumInvoice;
        //            }
        //            catch
        //            {

        //            }

        //            if (closingBalance < 0)
        //            {
        //                advanceamt = -closingBalance;
        //                //closingBalance = advanceamt;
        //            }
        //            else
        //            {
        //                dueamount = closingBalance;
        //                //closingBalance = -dueamount;
        //            }
        //        }
        //        else
        //        {

        //            dueamount = SumInvoice;

        //        }

        //        //for get actual credi tlimit


        //        //calculation for get invoice amount incompany currency 

        //        //LocalInvoiceAmt = Convert.ToDouble(txtGrandTotal.Text) * Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString()));
        //        LocalInvoiceAmt = creditAmount * Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString()));


        //        //advance cheque amount

        //        DataTable dtAdvanceCheque = objVoucherDetail.GetVoucherDetailAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString());


        //        DateTime ToDate = Convert.ToDateTime(txtSInvDate.Text);
        //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
        //        ToDate = ToDate.AddDays(CreditDays);
        //        try
        //        {
        //            dtAdvanceCheque = new DataView(dtAdvanceCheque, "Other_Account_No=" + txtCustomer.Text.Split('/')[1].ToString() + " and Cheque_Clear_Date>='" + txtSInvDate.Text + "' and Cheque_Clear_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {

        //        }


        //        foreach (DataRow dr in dtAdvanceCheque.Rows)
        //        {
        //            AdvacnechequeAmount += Convert.ToDouble(dr["CompanyCurrCredit"].ToString());
        //        }

        //        ActualCreditLimit = CreditLimit - dueamount + advanceamt;

        //        //when only credi tlimit is mentioned then we add validation 


        //        if (!Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()) && !Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()) && !Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
        //        {

        //            if (LocalInvoiceAmt > ActualCreditLimit)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("Customer credit limit is over");
        //                return;
        //                //DisplayMessage("your credit balance is "+ActualCreditLimit +" "+Session["Currency"].ToString()+" so invoice amount should be less than or equal to credit balance");
        //                //return;
        //            }
        //        }

        //        else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
        //        {

        //            if ((LocalInvoiceAmt + TotalUnpostInvoicesum) > ActualCreditLimit)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("Customer credit limit is over");
        //                return;
        //            }

        //            if (AdvacnechequeAmount == 0)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("pdc cheque required of  " + CreditDays + " days");
        //                return;
        //            }

        //            if ((LocalInvoiceAmt + TotalUnpostInvoicesum) > AdvacnechequeAmount)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("pdc cheque date not allow more than " + CreditDays + " days");
        //                return;
        //            }
        //        }
        //        else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
        //        {


        //            if ((dueamount + TotalUnpostInvoicesum) > 0)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("Previous Invoice amount is Pending so you can not generate new invoice ");
        //                return;
        //            }
        //            else
        //            {
        //                if (LocalInvoiceAmt > ActualCreditLimit)
        //                {
        //                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                    DisplayMessage("Your Credit limit is " + CreditLimit + " " + Session["Currency"].ToString() + " so Invoice amount should be less than or equal to Credit limit");
        //                    return;
        //                }

        //            }
        //        }
        //        else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
        //        {

        //            if ((LocalInvoiceAmt + TotalUnpostInvoicesum) > ActualCreditLimit)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("your credit balance is " + (ActualCreditLimit) + " " + Session["Currency"].ToString() + " so invoice amount should be less than or equal to credit balance");
        //                return;
        //            }

        //            if ((LocalInvoiceAmt / 2) > closingBalance)
        //            {
        //                btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        //                DisplayMessage("half Invoice amount should be less than or equal to Closing balance");
        //                return;
        //            }
        //        }
        //    }
        //        //}
        //    }
        //}


        //code end 
        string VechileId = "0";
        string DriverId = "0";
        string strTransportEmpID = "0";
        try
        {
            VechileId = txtvehiclename.Text.Split('/')[1].ToString();
        }
        catch
        {
            VechileId = "0";
        }

        try
        {
            DriverId = txtdrivername.Text.Split('/')[2].ToString();
        }
        catch
        {
            DriverId = "0";

        }

        try
        {
            //if (txtEmployee.Text != "")
            //{
            //    string Emp_Code = txtEmployee.Text.Split('/')[1].ToString();

            //    string Emp_Id = objDa.get_SingleValue("Select Emp_ID from Set_EmployeeMaster where Emp_Code='"+ Emp_Code + "'");
            //    if (Emp_Id != "")
            //    {
            //        strTransportEmpID = Emp_Id;
            //    }
            //    else
            //    {
            //        strTransportEmpID = "0";
            //    }
            //}
        }
        catch
        {
            strTransportEmpID = "0";
        }


        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }
        string strTransportCustomertId = "0";

        if (txtcustomername.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtcustomername.Text.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                strTransportCustomertId = txtcustomername.Text.Split('/')[1].ToString();
                DataTable dtCompany = objContact.GetContactTrueById(strTransportCustomertId);
                if (dtCompany.Rows.Count > 0)
                {

                }
                else
                {
                    strTransportCustomertId = "0";
                }
            }
            else
            {
                strTransportCustomertId = "0";
            }
        }
        DateTime VisitDate;

        if (!string.IsNullOrEmpty(txtVisitDate.Text))
        {
            VisitDate = Convert.ToDateTime(txtVisitDate.Text);
        }
        else
        {
            VisitDate = DateTime.Now;
        }



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            string strSOId = string.Empty;
            string TransactionType = string.Empty;
            if (ddlTransType.SelectedIndex >= 0)
                TransactionType = ddlTransType.SelectedValue;
            else
                TransactionType = "-1";

            if (editid.Value != "")
            {
                InvoiceEditId = editid.Value;
                try
                {
                    objDa.execute_Command("Delete From Inv_InvoiceTransport  where Ref_Id='" + editid.Value + "' And Ref_Type='SI'");
                }
                catch
                {


                }
                //objDa.execute_Command("Update Inv_InvoiceTransport Set Customer_Id='" + strTransportCustomertId + "',Emp_Id='" + strTransportEmpID + "',Vehicle_Id='" + VechileId + "',Driver_Id='" + DriverId + "',Description='" + txtdescription.Text + "',Chargable_Amount='" + txtChargableAmount.Text + "',Contact_Address='" + strAddressId + "',Contact_No='" + txtPermanentMobileNo.Text + "',Visit_Date='" + VisitDate.ToString("yyyy-MM-dd") + "',Visit_Time='" + txtVisitTime.Text + "',Field1='" + txtAreaName.Text + "',Is_Active='1',Modified_By='" + Session["UserId"].ToString() + "',Modified_Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where Ref_Id='" + editid.Value + "'");
                try
                {
                    objDa.execute_Command("INSERT INTO [dbo].[Inv_InvoiceTransport]([Ref_Type],[Ref_Id],[Customer_Id],[Emp_Id],[Vehicle_Id],[Driver_Id],[Description],[Chargable_Amount],[Contact_Address],[Contact_No],[Visit_Date],[Visit_Time],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[Is_Active],[Created_By],[Created_Date],[Modified_By],[Modified_Date]) VALUES ('SI','" + InvoiceEditId.ToString() + "','" + strTransportCustomertId + "','" + strTransportEmpID + "','" + VechileId + "','" + DriverId + "','" + txtdescription.Text + "','" + txtChargableAmount.Text + "','" + strAddressId + "','" + txtPermanentMobileNo.Text + "','" + VisitDate.ToString("yyyy-MM-dd") + "','" + txtVisitTime.Text + "','" + txtAreaName.Text + "','" + txtPersonName.Text + "','" + txtPersonMobileNo.Text + "','" + txtTrakingId.Text + "','','1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','1','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                }
                catch (Exception exp)

                {

                }
                InvoiceId = editid.Value;
                DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval", ref trns);

                if (DtApprove.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                    {
                        if (ViewState["ApprovalStatus"].ToString() == "Rejected")
                        {
                            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                            b = objSInvHeader.UpdateSInvHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), strPaymentMode, ddlCurrency.SelectedValue, TransType, hdnSalesOrderId.Value, Emp_ID, txtPOSNo.Text, txtRemark.Text, txtAccountNo.Text, txtInvoiceCosting.Text, txtShift.Text, strPost, txtTender.Text, txtAmount.Text, txtTotalQuantity.Text, txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtNetAmount.Text, txtDiscountP.Text, txtDiscountV.Text, txtGrandTotal.Text, GetCustomerId(ref trns), txtInvoiecRefno.Text, ddlMerchant.SelectedValue, txtOrderId.Text, txtCondition1.Content, SetDecimal(txtNetAmountwithexpenses.Text), JobCardId, TransactionType, "", strAddressId2, GetCustomerId(txtShipCustomerName, ref trns), strAddressId, "Pending", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), hdnContactId.Value, ref trns);
                            DataTable dtEmp = objEmpApproval.GetApprovalTransation(StrCompId, ref trns);
                            dtEmp = new DataView(dtEmp, "Approval_Id='10' and Ref_Id='" + editid.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtEmp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtEmp.Rows.Count; i++)
                                {
                                    objEmpApproval.UpdateApprovalTransaciton("SalesInvoice", editid.Value.ToString(), "92", dtEmp.Rows[i]["Emp_Id"].ToString(), "Pending", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                }
                            }
                        }
                        else
                        {
                            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                            b = objSInvHeader.UpdateSInvHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), strPaymentMode, ddlCurrency.SelectedValue, TransType, hdnSalesOrderId.Value, Emp_ID, txtPOSNo.Text, txtRemark.Text, txtAccountNo.Text, txtInvoiceCosting.Text, txtShift.Text, strPost, txtTender.Text, txtAmount.Text, txtTotalQuantity.Text, txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtNetAmount.Text, txtDiscountP.Text, txtDiscountV.Text, txtGrandTotal.Text, GetCustomerId(ref trns), txtInvoiecRefno.Text, ddlMerchant.SelectedValue, txtOrderId.Text, txtCondition1.Content, SetDecimal(txtNetAmountwithexpenses.Text), JobCardId, TransactionType, "", strAddressId2, GetCustomerId(txtShipCustomerName, ref trns), strAddressId, ViewState["ApprovalStatus"].ToString(), txtExchangeRate.Text, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), hdnContactId.Value, ref trns);

                        }
                    }
                    else
                    {
                        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                        string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                        b = objSInvHeader.UpdateSInvHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), strPaymentMode, ddlCurrency.SelectedValue, TransType, hdnSalesOrderId.Value, Emp_ID, txtPOSNo.Text, txtRemark.Text, txtAccountNo.Text, txtInvoiceCosting.Text, txtShift.Text, strPost, txtTender.Text, txtAmount.Text, txtTotalQuantity.Text, txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtNetAmount.Text, txtDiscountP.Text, txtDiscountV.Text, txtGrandTotal.Text, GetCustomerId(ref trns), txtInvoiecRefno.Text, ddlMerchant.SelectedValue, txtOrderId.Text, txtCondition1.Content, SetDecimal(txtNetAmountwithexpenses.Text), JobCardId, TransactionType, "", strAddressId2, GetCustomerId(txtShipCustomerName, ref trns), strAddressId, ViewState["ApprovalStatus"].ToString(), txtExchangeRate.Text, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), hdnContactId.Value, ref trns);
                    }

                }

                if (b != 0)
                {
                    //code added on 19-feb-2020 to delete stock batch master existing data so new will reinsert
                    ObjStockBatchMaster.DeleteStockBatchMasterByRefTypeAndRefId(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value, ref trns);
                    //this code is created by jitendra upadhya on 17-04-2015
                    //this code for delete record from tax ref detail table by reftype and ref id
                    //code start
                    //for (int i = 0; i < gridView.Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        if (((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value == "")
                    //        {
                    //            continue;
                    //        }

                    //        if (i == 0)
                    //        {
                    //            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SINV", editid.Value, ref trns);
                    //        }
                    //        objTaxRefDetail.InsertRecord("SINV", editid.Value, "0", "0", "0", ((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value, ((Label)gridView.Rows[i].FindControl("lblTaxper")).Text, ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text, false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //    }
                    //    catch
                    //    {


                    //    }
                    //}
                    //code end

                    // Code for Entry of Product With their Tax Details in Inv_RefDetail Table
                    // Added By KSR on 06-09-2017
                    // Start Code



                    // End Code

                    DataTable dtExpense = new DataTable();
                    dtExpense = (DataTable)ViewState["ExpdtSales"];
                    if (dtExpense != null)
                    {
                        try
                        {
                            ObjShipExpHeader.ShipExpHeader_Delete(StrCompId, StrBrandId, StrLocationId, editid.Value, "SI", ref trns);
                        }
                        catch
                        {

                        }
                        string strFooterVal = "0";
                        try
                        {
                            //string Tax = (GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer") as Label).Text;
                            //string With_Tax = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                            // strFooterVal = (Convert.ToDouble(With_Tax) - Convert.ToDouble(Tax)).ToString();
                            strFooterVal = (GridExpenses.FooterRow.FindControl("txttotExp") as Label).Text;

                        }
                        catch
                        {
                            strFooterVal = "0";
                        }
                        ObjShipExpHeader.ShipExpHeader_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value.Trim().ToString(), ddlCurrency.SelectedValue.ToString(), "0", "0", txtGrandTotal.Text.Trim(), strFooterVal, "SI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), StrUserId.ToString(), DateTime.Now.ToString(), StrUserId.ToString(), DateTime.Now.ToString(), ref trns);
                        try
                        {
                            ObjShipExpDetail.ShipExpDetail_Delete(StrCompId, StrBrandId, StrLocationId, editid.Value.ToString(), "SI", ref trns);
                            foreach (DataRow dr in ((DataTable)ViewState["ExpdtSales"]).Rows)
                            {
                                ObjShipExpDetail.ShipExpDetail_Insert(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value.ToString(), dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "SI", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
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
                            ObjPaymentTrans.DeleteByRefandRefNo(StrCompId.ToString(), "SI", editid.Value.ToString(), ref trns);
                            foreach (DataRow dr in ((DataTable)ViewState["PayementDt"]).Rows)
                            {
                                //field3 used to store Customer credit note no
                                ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "SI", editid.Value.ToString(), "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", dr["field3"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        catch
                        {

                        }
                    }

                    objSInvDetail.DeleteSInvDetail(StrCompId, StrBrandId, StrLocationId, editid.Value, ref trns);

                    //code created for delete record in delivery voucher detail table

                    //code start
                    DataTable dtdelrecord = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);


                    dtdelrecord = new DataView(dtdelrecord, "Field2=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();


                    if (dtdelrecord.Rows.Count > 0)
                    {

                        //for delete record in vouche detail table

                        objdelVoucherDetail.DeleteRecord_By_VoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtdelrecord.Rows[0]["Trans_Id"].ToString(), ref trns);

                    }

                    //code end
                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SINV", editid.Value, ref trns);
                    foreach (GridViewRow gvr in gvProduct.Rows)
                    {
                        Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");
                        HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                        Label lblgvProductDescription = (Label)gvr.FindControl("lblgvProductDescription");
                        DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                        TextBox lblgvUnitPrice = (TextBox)gvr.FindControl("lblgvUnitPrice");

                        Label lblgvOrderqty = (Label)gvr.FindControl("lblgvOrderqty");
                        TextBox txtgvSalesQuantity = (TextBox)gvr.FindControl("txtgvSalesQuantity");

                        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                        Label lblSOId = (Label)gvr.FindControl("lblSOId");
                        TextBox lblgvFreeQuantity = (TextBox)gvr.FindControl("lblgvFreeQuantity");
                        TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
                        strSOId = lblSOId.Text;
                        if (lblgvUnitPrice.Text == "")
                        {
                            lblgvUnitPrice.Text = "0";
                        }

                        if (lblgvFreeQuantity.Text == "")
                        {
                            lblgvFreeQuantity.Text = "0";

                        }
                        if (txtgvSalesQuantity.Text == "")
                        {
                            txtgvSalesQuantity.Text = "0";
                        }
                        if (txtgvTaxP.Text == "")
                        {
                            txtgvTaxP.Text = "0";
                        }
                        if (txtgvTaxV.Text == "")
                        {
                            txtgvTaxV.Text = Convert_Into_DF("0");
                        }
                        if (txtgvDiscountP.Text == "")
                        {
                            txtgvDiscountP.Text = "0";
                        }
                        if (txtgvDiscountV.Text == "")
                        {
                            txtgvDiscountV.Text = "0";
                        }
                        if (lblSOId.Text == "")
                        {
                            lblSOId.Text = "0";
                        }


                        try
                        {
                            AvgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), hdngvProductId.Value).Rows[0]["Field2"].ToString();
                        }
                        catch
                        {
                            AvgCost = "0";
                        }
                        if (AvgCost == "")
                        {
                            AvgCost = "0";
                        }
                        int Details_ID = objSInvDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, lblgvSerialNo.Text, strPaymentMode, txtPOSNo.Text, TransType, lblSOId.Text, hdngvProductId.Value, lblgvProductDescription.Text, ddlUnitName.SelectedValue, lblgvUnitPrice.Text, "0", lblgvOrderqty.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, Convert_Into_DF(txtgvTaxV.Text), txtgvDiscountP.Text, txtgvDiscountV.Text, "False", strPost, AvgCost, lblgvFreeQuantity.Text, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        //int Details_ID = objSInvDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, lblgvSerialNo.Text, strPaymentMode, txtPOSNo.Text, TransType, lblSOId.Text, hdngvProductId.Value, lblgvProductDescription.Text, ddlUnitName.SelectedValue, lblgvUnitPrice.Text, "0", lblgvOrderqty.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, Convert_Into_DF("40.312"), txtgvDiscountP.Text, txtgvDiscountV.Text, "False", strPost, AvgCost, lblgvFreeQuantity.Text, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax("gvProduct", editid.Value, Details_ID.ToString(), gvr, ref trns);


                        //modify by jitendra upadhyay for deleivry voucher concept
                        //here stock will not affect if delivery falg is true in sales order 
                        //modify on 30/12/2015
                        if (RdoWithOutSo.Checked || objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvr.FindControl("lblSOId")).Text, ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                        {
                            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                            {
                                int LedgerId = 0;
                                if (chkPost.Checked)
                                {
                                    string UnitPrice = ((Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text) + Convert.ToDouble(txtgvTaxV.Text)) / Convert.ToDouble(txtExchangeRate.Text)).ToString();
                                    LedgerId = ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", editid.Value, "0", hdngvProductId.Value, ddlUnitName.SelectedValue, "O", "0", "0", "0", txtgvSalesQuantity.Text, "1/1/1800", UnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                }

                                string strIventoryMethod = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MainTainStock"].ToString();

                                if (strIventoryMethod == "SNO")
                                {
                                    if (RdoSo.Checked == true)
                                    {
                                        //ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value, hdngvProductId.Value, lblSOId.Text, ref trns);

                                        if (ViewState["dtFinal"] != null)
                                        {
                                            DataTable dt = (DataTable)ViewState["dtFinal"];
                                            dt = new DataView(dt, "Product_Id='" + hdngvProductId.Value + "' and SOrderNo='" + lblSOId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                                            foreach (DataRow dr in dt.Rows)
                                            {
                                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", editid.Value, hdngvProductId.Value, ddlUnitName.SelectedValue, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", lblSOId.Text, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ViewState["dtFinal"] != null)
                                        {
                                            DataTable dt = (DataTable)ViewState["dtFinal"];
                                            dt = new DataView(dt, "Product_Id='" + hdngvProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                                            ObjStockBatchMaster.DeleteStockBatchMaster(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value, hdngvProductId.Value, ref trns);

                                            foreach (DataRow dr in dt.Rows)
                                            {
                                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", editid.Value, hdngvProductId.Value, ddlUnitName.SelectedValue, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                    }
                                }
                                //else if (strIventoryMethod == "FM" || strIventoryMethod == "FE" || strIventoryMethod == "LM" || strIventoryMethod == "LE")
                                //{
                                //    ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value, hdngvProductId.Value, lblSOId.Text, ref trns);
                                //    UpdateRecordForStckableItem(hdngvProductId.Value, objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, ref trns).Rows[0]["MaintainStock"].ToString(), Convert.ToDouble(txtgvSalesQuantity.Text), editid.Value, ddlUnitName.SelectedValue, lblSOId.Text, LedgerId.ToString(), ref trns);
                                //}
                            }
                        }

                        //for insert record in delivery voucher header and detail table
                        //modify on 30/12/2015

                        //if delivery voucher is true fro currenct location 
                        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Delivery Voucher allow", ref trns).Rows[0]["ParameterValue"].ToString()))
                        {

                            int delvouchercounter = 0;
                            //if (chkPost.Checked)
                            //{
                            if (RdoSo.Checked && objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvr.FindControl("lblSOId")).Text, ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                            {
                                DataTable dt = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);


                                dt = new DataView(dt, "Field2=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();


                                int VoucherId = 0;
                                if (dt.Rows.Count == 0)
                                {
                                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                                    string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                                    string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                                    VoucherId = objdelVoucherHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), getDeliveryDocumentNumber(ref trns), DateTime.Now.ToString(), ((Label)gvr.FindControl("lblSOId")).Text, txtCustomer.Text.Split('/')[3].ToString(), Emp_ID, "True", "Created From Sales Invoice", txtSInvNo.Text, editid.Value, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                }
                                else
                                {
                                    VoucherId = Convert.ToInt32(dt.Rows[0]["Trans_Id"].ToString());
                                }

                                delvouchercounter++;
                                objdelVoucherDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VoucherId.ToString(), ((Label)gvr.FindControl("lblSOId")).Text, delvouchercounter.ToString(), hdngvProductId.Value, ddlUnitName.SelectedValue, lblgvOrderqty.Text, txtgvSalesQuantity.Text, "True", ref trns);

                            }
                            //}
                        }


                        //this code is created by jitendra upadhya on 17-04-2015
                        //this code for insert record in tax ref detail table when we apply multiple tax according product category
                        //code start
                        foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                        {
                            if (Convert.ToDouble(txtgvTotal.Text) != 0)
                                objTaxRefDetail.InsertRecord("SINV", editid.Value, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblSOId.Text, ((HiddenField)gvr.FindControl("hdngvProductId")).Value, ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Details_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        //code end
                    }

                    Tax_Insert_Into_Inv_TaxRefDetail(editid.Value, ref trns);

                    if (chkPost.Checked)
                    {
                        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                        DisplayMessage("Invoice has been Posted");
                        //    cmn.DisableControls(this.Page, true);
                        //}
                    }
                    else
                    {
                        // Insert Notification For Leave by  ghanshyam suthar
                        //  Set_Notification();
                        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                        DisplayMessage("Record Updated", "green");
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }

                    editid.Value = "";
                }
                else
                {
                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval", ref trns);
                DataTable dt1 = new DataTable();
                string EmpPermission = string.Empty;
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                    {
                        EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("SalesInvoice").Rows[0]["Approval_Level"].ToString();

                        dt1 = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "92", Session["EmpId"].ToString());

                        if (dt1.Rows.Count == 0)
                        {
                            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                            DisplayMessage("Approval setup issue , please contact to your admin");
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




                //Add New Code For Approval On 08-12-2014
                string IsApproved = "Pending";

                if (!Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval", ref trns).Rows[0]["ParameterValue"].ToString()))
                {
                    IsApproved = "Approved";
                }

                //here we check that invoice number is duplicate or bot which is generated according 

                //code start


                string strInvoiceNo = string.Empty;


                //code end

                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

                b = objSInvHeader.InsertSInvHeader(StrCompId, StrBrandId, StrLocationId, txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), strPaymentMode, ddlCurrency.SelectedValue, TransType, hdnSalesOrderId.Value, Emp_ID, txtPOSNo.Text, txtRemark.Text, txtAccountNo.Text, txtInvoiceCosting.Text, txtShift.Text, chkPost.Checked.ToString(), txtTender.Text, txtAmount.Text, txtTotalQuantity.Text, txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtNetAmount.Text, txtDiscountP.Text, txtDiscountV.Text, txtGrandTotal.Text, GetCustomerId(ref trns), txtInvoiecRefno.Text, ddlMerchant.SelectedValue, txtOrderId.Text, txtCondition1.Content, SetDecimal(txtNetAmountwithexpenses.Text), JobCardId, TransactionType, "", strAddressId2, GetCustomerId(txtShipCustomerName, ref trns), strAddressId, IsApproved.Trim(), txtExchangeRate.Text, "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), hdnContactId.Value, ref trns);
                if (b != 0)
                {
                    editid.Value = b.ToString();
                    //here we update status in job card if invoice created against  the job card

                    //code created on 04-02-2017
                    try
                    {
                        objDa.execute_Command("INSERT INTO [dbo].[Inv_InvoiceTransport]([Ref_Type],[Ref_Id],[Customer_Id],[Emp_Id],[Vehicle_Id],[Driver_Id],[Description],[Chargable_Amount],[Contact_Address],[Contact_No],[Visit_Date],[Visit_Time],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[Is_Active],[Created_By],[Created_Date],[Modified_By],[Modified_Date]) VALUES ('SI','" + b.ToString() + "','" + strTransportCustomertId + "','" + strTransportEmpID + "','" + VechileId + "','" + DriverId + "','" + txtdescription.Text + "','" + txtChargableAmount.Text + "','" + strAddressId + "','" + txtPermanentMobileNo.Text + "','" + VisitDate.ToString("yyyy-MM-dd") + "','" + txtVisitTime.Text + "','" + txtAreaName.Text + "','','','','','1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','1','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                    }
                    catch (Exception exp)

                    {

                    }
                    //code start

                    if (JobCardId.Trim() != "0")
                    {

                        string strsql = "update SM_JobCards_Header set Field2='" + b.ToString() + "',Field6='True' where Trans_Id=" + JobCardId + "";
                        objDa.execute_Command(strsql, ref trns);
                    }


                    //code end


                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    InvoiceEditId = b.ToString();

                    if (txtSInvNo.Text == ViewState["DocNo"].ToString())
                    {
                        string sql = string.Empty;
                        int skipNo = 0;
                        if (Session["LocId"].ToString() == "7") //this is for trading location
                        {
                            sql = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and Invoice_No like '" + ViewState["DocNo"].ToString() + "%'";
                            try
                            {
                                Int32.TryParse(ConfigurationManager.AppSettings["TradingInvoiceNoInterval"].ToString(), out skipNo);
                            }
                            catch
                            {

                            }
                        }
                        else if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11" || Session["LocId"].ToString() == "14" || Session["LocId"].ToString() == "15") //this is OPC Location
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
                            txtSInvNo.Text = ViewState["DocNo"].ToString() + (strInvoiceCount == 0 ? "1" : strInvoiceCount.ToString());
                            strInvoiceNo = txtSInvNo.Text;
                            string sql1 = "SELECT count(*) FROM Inv_SalesInvoiceHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and invoice_no='" + txtSInvNo.Text + "'";
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
                        objSInvHeader.Updatecode(b.ToString(), txtSInvNo.Text, ref trns);

                        ////DataTable Dttemp = new DataTable();
                        ////DataTable dtCount = objSInvHeader.GetSInvHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                        ////if (dtCount.Rows.Count == 0)
                        ////{
                        ////    objSInvHeader.Updatecode(b.ToString(), txtSInvNo.Text + "1", ref trns);
                        ////    txtSInvNo.Text = txtSInvNo.Text + "1";
                        ////    strInvoiceNo = txtSInvNo.Text;
                        ////}
                        ////else
                        ////{
                        ////    DataTable dtCount1 = new DataView(dtCount, "Invoice_No='" + txtSInvNo.Text + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
                        ////    int NoRow = dtCount.Rows.Count;
                        ////    if (dtCount1.Rows.Count > 0)
                        ////    {
                        ////        //trns.Rollback();
                        ////        //if (con.State == System.Data.ConnectionState.Open)
                        ////        //{
                        ////        //    con.Close();
                        ////        //}
                        ////        //trns.Dispose();
                        ////        //con.Dispose();
                        ////        //Button btnInvsave = new Button();
                        ////        //btnInvsave.ID = "btnSave";
                        ////        //btnSave_Click((object)btnInvsave, null);

                        ////        bool bCodeFlag = true;
                        ////        while (bCodeFlag)
                        ////        {
                        ////            NoRow += 1;
                        ////            DataTable dtTemp = new DataView(dtCount, "Invoice_No='" + txtSInvNo.Text + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                        ////            if (dtTemp.Rows.Count == 0)
                        ////            {
                        ////                bCodeFlag = false;
                        ////            }
                        ////        }
                        ////    }

                        ////    objSInvHeader.Updatecode(b.ToString(), txtSInvNo.Text + NoRow.ToString(), ref trns);
                        ////    txtSInvNo.Text = txtSInvNo.Text + NoRow.ToString();
                        ////    strInvoiceNo = txtSInvNo.Text;
                        ////}

                        ////Dttemp = dtCount.Copy();
                        ////try
                        ////{
                        ////    Dttemp = new DataView(Dttemp, "Invoice_No = '" + strInvoiceNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                        ////}
                        ////catch
                        ////{

                        ////}

                        ////if (Dttemp.Rows.Count > 0)
                        ////{
                        ////    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                        ////    DisplayMessage("Generated Invoice no. already exists");
                        ////    txtSInvNo.Focus();
                        ////    trns.Rollback();
                        ////    if (con.State == System.Data.ConnectionState.Open)
                        ////    {
                        ////        con.Close();
                        ////    }
                        ////    trns.Dispose();
                        ////    con.Dispose();
                        ////    return;
                        ////}
                    }

                    //this code is created by jitendra upadhya on 17-04-2015
                    //this code for delete record from tax ref detail table by reftype and ref id
                    //code start

                    //for (int i = 0; i < gridView.Rows.Count; i++)
                    //{
                    //    if (((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value == "")
                    //    {
                    //        continue;
                    //    }
                    //    objTaxRefDetail.InsertRecord("SINV", strMaxId, "0", "0", "0", ((HiddenField)gridView.Rows[i].FindControl("hdnTaxId")).Value, ((Label)gridView.Rows[i].FindControl("lblTaxper")).Text, ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text, false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //}
                    //code end


                    // Code for Entry of Product With their Tax Details in Inv_RefDetail Table
                    // Added By KSR on 06-09-2017
                    // Start Code



                    // End Code


                    //if (txtSInvNo.Text == ViewState["DocNo"].ToString())
                    //{
                    //  objSInvHeader.Updatecode(b.ToString(), strInvoiceNo);
                    //}

                    if (strMaxId != "" && strMaxId != "0")
                    {
                        InvoiceId = strMaxId;
                        if (Dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
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
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("10", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else if (EmpPermission == "2")
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("10", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else if (EmpPermission == "3")
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("10", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("10", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        Session["PriorityEmpId"] = PriorityEmpId;
                                        Session["cur_trans_id"] = cur_trans_id;
                                        Session["Ref_ID"] = strMaxId.ToString();
                                        Set_Notification();
                                    }
                                }
                            }
                        }

                        DataTable dtExpense = new DataTable();
                        dtExpense = (DataTable)ViewState["ExpdtSales"];
                        if (dtExpense != null)
                        {
                            try
                            {
                                // string Tax = (GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer") as Label).Text;
                                // string With_Tax = (GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer") as Label).Text;
                                //string strFooterVal = (Convert.ToDouble(With_Tax) - Convert.ToDouble(Tax)).ToString();

                                string strFooterVal = (GridExpenses.FooterRow.FindControl("txttotExp") as Label).Text;
                                ObjShipExpHeader.ShipExpHeader_Insert(StrCompId, StrBrandId, StrLocationId, strMaxId, ddlCurrency.SelectedValue, "0", "0", txtGrandTotal.Text.Trim(), strFooterVal, "SI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), StrUserId, DateTime.Now.ToString(), StrUserId, DateTime.Now.ToString(), ref trns);
                            }
                            catch
                            {

                            }

                            try
                            {
                                foreach (DataRow dr in ((DataTable)ViewState["ExpdtSales"]).Rows)
                                {
                                    ObjShipExpDetail.ShipExpDetail_Insert(StrCompId, StrBrandId, StrLocationId, strMaxId, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "SI", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
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
                                    //field3 used to store Customer credit note no
                                    ObjPaymentTrans.insert(StrCompId, dr["PaymentModeId"].ToString(), "SI", strMaxId, "0", dr["AccountNo"].ToString(), dr["CardNo"].ToString(), dr["CardName"].ToString(), dr["BankAccountNo"].ToString(), dr["BankId"].ToString(), dr["BankAccountName"].ToString(), dr["ChequeNo"].ToString(), dr["ChequeDate"].ToString(), dr["Pay_Charges"].ToString(), ddlCurrency.SelectedValue, dr["PayExchangeRate"].ToString(), dr["FCPayAmount"].ToString(), "", "", dr["field3"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                            catch
                            {

                            }
                        }

                        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SINV", strMaxId.ToString(), ref trns);

                        foreach (GridViewRow gvr in gvProduct.Rows)
                        {
                            Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");
                            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                            Label lblgvProductDescription = (Label)gvr.FindControl("lblgvProductDescription");
                            DropDownList ddlUnitName = (DropDownList)gvr.FindControl("ddlUnitName");
                            TextBox lblgvUnitPrice = (TextBox)gvr.FindControl("lblgvUnitPrice");

                            Label lblgvOrderqty = (Label)gvr.FindControl("lblgvOrderqty");
                            TextBox txtgvSalesQuantity = (TextBox)gvr.FindControl("txtgvSalesQuantity");

                            TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                            TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                            Label lblSOId = (Label)gvr.FindControl("lblSOId");
                            TextBox lblgvFreeQuantity = (TextBox)gvr.FindControl("lblgvFreeQuantity");
                            TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
                            strSOId = lblSOId.Text;

                            if (lblgvUnitPrice.Text == "")
                            {
                                lblgvUnitPrice.Text = "0";
                            }
                            if (lblgvFreeQuantity.Text == "")
                            {
                                lblgvFreeQuantity.Text = "0";
                            }
                            if (txtgvSalesQuantity.Text == "")
                            {
                                txtgvSalesQuantity.Text = "0";
                            }
                            if (txtgvTaxP.Text == "")
                            {
                                txtgvTaxP.Text = "0";
                            }
                            if (txtgvTaxV.Text == "")
                            {
                                txtgvTaxV.Text = Convert_Into_DF("0");
                            }
                            if (txtgvDiscountP.Text == "")
                            {
                                txtgvDiscountP.Text = "0";
                            }
                            if (txtgvDiscountV.Text == "")
                            {
                                txtgvDiscountV.Text = "0";
                            }
                            if (lblSOId.Text == "")
                            {
                                lblSOId.Text = "0";
                            }


                            try
                            {
                                AvgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), hdngvProductId.Value).Rows[0]["Field2"].ToString();
                            }
                            catch
                            {

                                AvgCost = "0";
                            }
                            if (AvgCost == "")
                            {
                                AvgCost = "0";
                            }



                            int Detail_ID = objSInvDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblgvSerialNo.Text, strPaymentMode, txtPOSNo.Text, TransType, lblSOId.Text, hdngvProductId.Value, lblgvProductDescription.Text, ddlUnitName.SelectedValue, lblgvUnitPrice.Text, "0", lblgvOrderqty.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, Convert_Into_DF(txtgvTaxV.Text), txtgvDiscountP.Text, txtgvDiscountV.Text, "False", strPost, AvgCost, lblgvFreeQuantity.Text, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            Insert_Tax("gvProduct", strMaxId, Detail_ID.ToString(), gvr, ref trns);




                            //this code is created by jitendra upadhya on 21-05-2015
                            //this code for updarte stock according the product rule 
                            //code start

                            //modify by jitendra upadhyay for deleivry voucher concept
                            //here stock will not affect if delivery falg is true in sales order 
                            //modify on 30/12/2015
                            if (RdoWithOutSo.Checked || objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvr.FindControl("lblSOId")).Text, ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                            {
                                if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                                {
                                    int LedgerId = 0;
                                    if (chkPost.Checked)
                                    {


                                        string UnitPrice = ((Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text) + Convert.ToDouble(txtgvTaxV.Text)) / Convert.ToDouble(txtExchangeRate.Text)).ToString();

                                        LedgerId = ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", strMaxId, "0", hdngvProductId.Value, ddlUnitName.SelectedValue, "O", "0", "0", "0", txtgvSalesQuantity.Text, "1/1/1800", UnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                    }

                                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MainTainStock"].ToString() == "SNO")
                                    {
                                        if (RdoSo.Checked == true)
                                        {
                                            //ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(StrCompId, StrBrandId, StrLocationId, "SI", editid.Value, hdngvProductId.Value, lblSOId.Text);

                                            if (ViewState["dtFinal"] != null)
                                            {
                                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                                dt = new DataView(dt, "Product_Id='" + hdngvProductId.Value + "' and SOrderNo='" + lblSOId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", strMaxId, hdngvProductId.Value, ddlUnitName.SelectedValue, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", lblSOId.Text, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ViewState["dtFinal"] != null)
                                            {
                                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                                dt = new DataView(dt, "Product_Id='" + hdngvProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", strMaxId, hdngvProductId.Value, ddlUnitName.SelectedValue, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }
                                        }
                                    }
                                    //else
                                    //{
                                    //    UpdateRecordForStckableItem(hdngvProductId.Value, objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvProductId.Value, ref trns).Rows[0]["MaintainStock"].ToString(), Convert.ToDouble(txtgvSalesQuantity.Text), strMaxId, ddlUnitName.SelectedValue, lblSOId.Text, LedgerId.ToString(), ref trns);
                                    //}
                                }
                            }

                            //for insert record in delivery voucher header and detail table
                            //modify on 30/12/2015
                            //if delivery voucher is true fro currenct location 
                            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Delivery Voucher allow", ref trns).Rows[0]["ParameterValue"].ToString()))
                            {
                                int delvouchercounter = 0;
                                //if (chkPost.Checked)
                                //{
                                if (RdoSo.Checked && objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvr.FindControl("lblSOId")).Text, ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                {
                                    DataTable dt = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);


                                    dt = new DataView(dt, "Field2=" + strMaxId + "", "", DataViewRowState.CurrentRows).ToTable();


                                    int VoucherId = 0;
                                    if (dt.Rows.Count == 0)
                                    {
                                        VoucherId = objdelVoucherHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), getDeliveryDocumentNumber(ref trns), DateTime.Now.ToString(), ((Label)gvr.FindControl("lblSOId")).Text, txtCustomer.Text.Split('/')[3].ToString(), Emp_ID, "True", "", txtSInvNo.Text, strMaxId, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                    }
                                    else
                                    {
                                        VoucherId = Convert.ToInt32(dt.Rows[0]["Trans_Id"].ToString());
                                    }

                                    delvouchercounter++;
                                    objdelVoucherDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VoucherId.ToString(), ((Label)gvr.FindControl("lblSOId")).Text, delvouchercounter.ToString(), hdngvProductId.Value, ddlUnitName.SelectedValue, lblgvOrderqty.Text, txtgvSalesQuantity.Text, "True", ref trns);

                                }
                                //}
                            }
                            //code end

                            //this code is created by jitendra upadhya on 17-04-2015
                            //this code for insert record in tax ref detail table when we apply multiple tax according product category
                            //code start
                            foreach (GridViewRow gvChildRow in ((GridView)gvr.FindControl("gvchildGrid")).Rows)
                            {
                                if (Convert.ToDouble(txtgvTotal.Text) != 0)
                                    objTaxRefDetail.InsertRecord("SINV", strMaxId, ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value, lblSOId.Text, ((HiddenField)gvr.FindControl("hdngvProductId")).Value, ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value, SetDecimal(((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text).ToString(), ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text, ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked.ToString(), "", Detail_ID.ToString(), "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            //code end
                        }
                        Tax_Insert_Into_Inv_TaxRefDetail(strMaxId, ref trns);
                    }

                    if (chkPost.Checked)
                    {
                        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                        DisplayMessage("Invoice has been Posted");
                    }
                    else
                    {
                        btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                        DisplayMessage("Record Saved", "green");
                    }
                    ViewState["OnSave"] = null;

                }
                else
                {
                    btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
                    DisplayMessage("Record  Not Saved");
                }
            }

            if (chkPost.Checked)
            {
                string strMaxId = string.Empty;
                if (editid.Value == "")
                {
                    DataTable dtMaxId = objSInvHeader.GetMaxSalesInvoiceId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                    if (dtMaxId.Rows.Count > 0)
                    {
                        strMaxId = dtMaxId.Rows[0][0].ToString();
                    }
                }
                else
                {
                    strMaxId = editid.Value.ToString();
                }

                string strCashAccount = string.Empty;
                string strCreditAccount = string.Empty;
                string strInventory = string.Empty;
                string strCostOfSales = string.Empty;
                string strReceiveVoucherAcc = string.Empty;

                DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId, ref trns);
                DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCash.Rows.Count > 0)
                {
                    strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Sales Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCredit.Rows.Count > 0)
                {
                    strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtInventory = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtInventory.Rows.Count > 0)
                {
                    strInventory = dtInventory.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtCostOfSales = new DataView(dtAcParameter, "Param_Name='Cost Of Sales'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCostOfSales.Rows.Count > 0)
                {
                    strCostOfSales = dtCostOfSales.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtReceiveVoucher.Rows.Count > 0)
                {
                    strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
                }

                //For Advance Credit Account
                string strAdvanceCreditAC = string.Empty;
                DataTable dtAdvanceCreditAC = new DataView(dtAcParameter, "Param_Name='SO Advance Credit Account'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAdvanceCreditAC.Rows.Count > 0)
                {
                    strAdvanceCreditAC = dtAdvanceCreditAC.Rows[0]["Param_Value"].ToString();
                }

                string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "303", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), strDepartmentId, strMaxId, "SINV", txtSInvNo.Text, txtSInvDate.Text, strVoucherNo, DateTime.Now.ToString(), "SI", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue, txtExchangeRate.Text, "Sales Invoice Detail On '" + txtSInvNo.Text + "'", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = string.Empty;
                DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                if (dtVMaxId.Rows.Count > 0)
                {
                    strVMaxId = dtVMaxId.Rows[0][0].ToString();
                    if (strVoucherNo != "")
                    {
                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
                        if (dtCount.Rows.Count == 0)
                        {
                            objVoucherHeader.Updatecode(strVMaxId, strVoucherNo + "1", ref trns);
                        }
                        else
                        {
                            objVoucherHeader.Updatecode(strVMaxId, strVoucherNo + dtCount.Rows.Count, ref trns);
                        }
                    }
                }

                //if (gvadvancepayment.Rows.Count > 0)
                //{
                //    string strAPayment = ((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text;
                //    if (strAPayment != "" && strAPayment != "0")
                //    {
                //        strAPayment = (Convert.ToDouble(txtNetAmountwithexpenses.Text) - Convert.ToDouble(strAPayment)).ToString();
                //        objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "", strCreditAccount, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", "0.00", strAPayment, "Sales Invoice Detail On Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strAPayment, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //    }
                //}
                //else
                //{
                //for Credit Entry
                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), SetDecimal(txtNetAmountwithexpenses.Text));
                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, "", strCreditAccount, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", "0.00", SetDecimal(txtNetAmountwithexpenses.Text), "Sales Invoice Detail On Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, SetDecimal(txtNetAmountwithexpenses.Text), "0.00", strCompanyCrrValueCr, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                // }


                string Expenses_Tax = "0";
                string[,] Net_Expenses_Tax = new string[1, 5];
                double Exp = 0;
                double CustomerExp = 0;
                //Expenses Entries In Voucher


                if (ViewState["ExpdtSales"] != null)
                {
                    foreach (DataRow dr in ((DataTable)ViewState["ExpdtSales"]).Rows)
                    {
                        string strExpensesName = GetExpName(dr["Expense_Id"].ToString());
                        string strForeignAmount = dr["FCExpAmount"].ToString();
                        string strExpensesId = dr["Expense_Id"].ToString();
                        string strExpAmount = dr["Exp_Charges"].ToString();
                        string strAccountNo = dr["Account_No"].ToString();
                        string strExpCurrencyId = dr["ExpCurrencyID"].ToString();
                        string strExchangeRate = dr["ExpExchangeRate"].ToString();
                        Exp = Exp + Convert.ToDouble(dr["Exp_Charges"].ToString());
                        Expenses_Tax = Get_Expenses_Tax(strExpAmount);
                        if (strAccountNo == strReceiveVoucherAcc)
                        {
                            CustomerExp += double.Parse(strExpAmount);
                            //For Debit Entry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), (Convert.ToDouble(strExpAmount) + Convert.ToDouble(Expenses_Tax)).ToString());
                            //string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strExpAmount);
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strReceiveVoucherAcc, txtCustomer.Text.Split('/')[3].ToString(), strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strExpAmount, "0.00", "'" + strExpensesName + "' On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, strExchangeRate, strForeignAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strVMaxId, "", strReceiveVoucherAcc, txtCustomer.Text.Split('/')[3].ToString(), strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strExpAmount, "0.00", "'" + strExpensesName + "' On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, strExchangeRate, strForeignAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                        }
                        else
                        {
                            //For Debit Entry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), (Convert.ToDouble(strExpAmount) + Convert.ToDouble(Expenses_Tax)).ToString());
                            //string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strExpAmount);
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strAccountNo, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strExpAmount, "0.00", "'" + strExpensesName + "' On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, strExchangeRate, strForeignAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strVMaxId, "", strAccountNo, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strExpAmount, "0.00", "'" + strExpensesName + "' On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, strExchangeRate, strForeignAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                        }
                    }
                }

                string strPayTotal = "0";
                int j = 0;
                //Payment Entries In Voucher
                double Cash = 0;
                DataTable dtPaymentTran = ObjPaymentTrans.GetPaymentTransTrue(StrCompId.ToString(), "SI", strMaxId, ref trns);
                if (dtPaymentTran.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPaymentTran.Rows.Count; i++)
                    {
                        string strPaymentType = dtPaymentTran.Rows[i]["PaymentType"].ToString();
                        string strPayAmount = dtPaymentTran.Rows[i]["Amount"].ToString();

                        if (dtPaymentTran.Rows[i]["AccountNo"].ToString() == strReceiveVoucherAcc)
                        {
                            //For Debit Entry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strPayAmount);
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, txtCustomer.Text.Split('/')[3].ToString(), strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strPayAmount, "0.00", "Payment Mode Detail On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strPayAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            //For Debit Entry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strPayAmount);
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strPayAmount, "0.00", "Payment Mode Detail On Sales Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strPayAmount, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        strPayTotal = (Convert.ToDouble(strPayTotal) + Convert.ToDouble(strPayAmount)).ToString();
                        Cash = Cash + Convert.ToDouble(strPayAmount);
                        //else if (strPaymentType == "Credit")
                        //{
                        //    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[i]["AccountNo"].ToString(), txtCustomer.Text.Split('/')[1].ToString(), strMaxId, "SINV", dtPaymentTran.Rows[i]["ChequeDate"].ToString(), "1/1/1800", dtPaymentTran.Rows[i]["ChequeNo"].ToString(), strPayAmount, "0.00", "Payment Mode Detail On Sales Invoice No '"+ txtSInvNo.Text +"'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //    strPayTotal = (Convert.ToDouble(strPayTotal) + Convert.ToDouble(strPayAmount)).ToString();
                        //}
                    }
                }

                string strTotalAmount = SetDecimal((Convert.ToDouble(SetDecimal(txtNetAmountwithexpenses.Text)) - Exp + Convert.ToDouble(Expenses_Tax)).ToString());
                string strDebitAmount = SetDecimal((Convert.ToDouble(strTotalAmount) - Convert.ToDouble(strPayTotal.ToString())).ToString());

                string strSalesFreign = (Convert.ToDouble(strDebitAmount) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                if (strDebitAmount != "0" && strDebitAmount != "")
                {
                    string strOrderAdvance = string.Empty;
                    if (strReceiveVoucherAcc != strAdvanceCreditAC)
                    {
                        if (gvadvancepayment.Rows.Count > 0)
                        {
                            strOrderAdvance = ((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text;
                            if (strOrderAdvance != "" && strOrderAdvance != "0")
                            {
                                strDebitAmount = (Convert.ToDouble(strDebitAmount) - Convert.ToDouble(strOrderAdvance)).ToString();
                            }
                        }
                    }

                    //if (strDebitAmount != "" && strDebitAmount != "0")
                    //{
                    //    if (CustomerExp != 0)
                    //    {
                    //        strDebitAmount = (Convert.ToDouble(strDebitAmount) - Convert.ToDouble(CustomerExp.ToString())).ToString();
                    //    }
                    //}

                    strSalesFreign = (Convert.ToDouble(strDebitAmount) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    //for Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strDebitAmount);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strReceiveVoucherAcc, txtCustomer.Text.Split('/')[3].ToString(), strMaxId, "SINV", "1/1/1800", "1/1/1800", "", strDebitAmount.ToString(), "0.00", "Sales Invoice Detail On Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strSalesFreign, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //Entry For Ageing Detail.
                string strPaidTotal = "0.00";
                string strTotalAllAmount = string.Empty;
                if (CustomerExp != 0)
                {
                    strTotalAllAmount = (Convert.ToDouble(strTotalAmount) + CustomerExp).ToString();
                    //if (Cash != 0)
                    //{
                    //    strTotalAllAmount = (Convert.ToDouble(strTotalAllAmount) - Convert.ToDouble(Cash.ToString())).ToString();
                    //}
                }
                else
                {
                    strTotalAllAmount = strTotalAmount;
                    //if (Cash != 0)
                    //{
                    //    strTotalAllAmount = (Convert.ToDouble(strTotalAllAmount) - Convert.ToDouble(Cash.ToString())).ToString();
                    //}
                }

                string strAdvanceOrderPayment = string.Empty;
                if (gvadvancepayment.Rows.Count > 0)
                {
                    strAdvanceOrderPayment = ((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text;
                    if (strTotalAllAmount != "" && strTotalAllAmount != "0")
                    {
                        strTotalAllAmount = (Convert.ToDouble(strTotalAllAmount) - Convert.ToDouble(strAdvanceOrderPayment)).ToString();
                    }
                    else
                    {

                    }
                }

                //for Deleting Sales Order Row From Ageing Table
                //if (strSOId != "0" && strSOId != "")
                //{
                //DataTable dtAgeingDetail = objAgeingDetail.GetAgeingDetailAll(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), ref trns);
                //dtAgeingDetail = new DataView(dtAgeingDetail, "Ref_Type='SO' and Ref_Id='" + strSOId + "'", "", DataViewRowState.CurrentRows).ToTable();
                //if (dtAgeingDetail.Rows.Count > 0)
                //{
                //    objAgeingDetail.DeleteAgeingDetailByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), dtAgeingDetail.Rows[0]["Trans_Id"].ToString(), ref trns);
                //}
                //}

                //strPaidTotal = Cash.ToString();
                //if (strTotalAllAmount != "0" && strTotalAllAmount != "")
                //{
                //    string strForeignTotAmount = (Convert.ToDouble(strTotalAllAmount) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                //    //for Debit Entry
                //    string strAgeDebit = (Convert.ToDouble(strTotalAllAmount) - Convert.ToDouble(Cash.ToString())).ToString();
                //    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strAgeDebit);
                //    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                //    //for Credit Entry
                //    string str2CompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), Cash.ToString());
                //    string Company2CurrCredit = str2CompanyCrrValueCr.Trim().Split('/')[0].ToString();

                //    //objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SINV", strMaxId, txtSInvNo.Text, txtSInvDate.Text, Session["AccountId"].ToString(), txtCustomer.Text.Split('/')[1].ToString(), strTotalAllAmount, Cash.ToString(), strTotalAllAmount, "1/1/1800", "1/1/1800", "", "Sales Invoice Detail On Sales Invoice No '" + txtSInvNo.Text + "'", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strForeignTotAmount, CompanyCurrDebit, Company2CurrCredit, Session["FinanceYearId"].ToString(), "RV", strVMaxId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //}

                //Entry for Cost of Sales.
                double CostofSales = 0;
                DataTable dtCOS = new DataTable();
                dtCOS = objSInvDetail.GetSInvDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, strMaxId, ref trns, Session["FinanceYearId"].ToString());
                if (dtCOS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCOS.Rows.Count; i++)
                    {
                        string strCost = (Convert.ToDouble(dtCOS.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dtCOS.Rows[i]["Field2"].ToString())).ToString();
                        CostofSales += Convert.ToDouble(strCost);
                    }
                }

                if (CostofSales != 0)
                {
                    string strCostFreign = (Convert.ToDouble(CostofSales.ToString()) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    //for debit entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", CostofSales.ToString(), "0.00", "Sales Invoice Cost Detail On Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strCostFreign, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //for credit entry
                    string str3CompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                    string Company3CurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", strMaxId, "SINV", "1/1/1800", "1/1/1800", "", "0.00", CostofSales.ToString(), "Sales Invoice Cost Detail On Invoice No '" + txtSInvNo.Text + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, txtExchangeRate.Text, strCostFreign, "0.00", Company3CurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }


            //Code for Approval

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue != null)
            {
                if (confirmValue.Length > 3)
                    confirmValue = confirmValue.Substring(confirmValue.LastIndexOf(',') + 1);
            }
            if (confirmValue == "Yes")
            {
                DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInvoiceApproval", ref trns);

                if (DtPara.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
                    {
                        string st = GetStatus(Convert.ToInt32(InvoiceEditId), ref trns);
                        if (st == "Approved")
                        {
                            InvoicePrint(InvoiceEditId);

                        }
                    }
                    else
                    {
                        InvoicePrint(InvoiceEditId);

                    }
                }
            }

            //here we commit transaction when all data insert and update proper 
            trns.Commit();

            try
            {
                DataTable dt = objSInquiryHeader.getInqIDFromInvoiceID(editid.Value);
                if (dt.Rows.Count > 0)
                    objSInquiryHeader.UpdateSalesStageFromInvoice(dt.Rows[0][0].ToString(), editid.Value);
            }
            catch
            {

            }


            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
            trns.Dispose();
            con.Dispose();
            FillGrid();
            //AllPageCode();
            Reset();

        }
        catch (Exception ex)
        {
            btnSInvSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
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

        DeleteTrigger(InvoiceEditId);
    }

    public void DeleteTrigger(string strInvoiceNo)
    {
        string strsql = "select Inv_SalesInvoiceDetail_Log.* from Inv_SalesInvoiceDetail_Log left join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail_Log.Invoice_No = Inv_SalesInvoiceDetail.Invoice_No and Inv_SalesInvoiceDetail_Log.Product_Id = Inv_SalesInvoiceDetail.Product_Id where   Inv_SalesInvoiceDetail_Log.Invoice_No = " + strInvoiceNo + " and  Inv_SalesInvoiceDetail.Product_Id is null";
        DataTable dt = objDa.return_DataTable(strsql);

        foreach (DataRow dr in dt.Rows)
        {
            objDa.execute_Command("INSERT INTO Inv_SalesInvoiceDetail_Log ([Company_Id], [Brand_Id], [Location_Id], [Operation_Type], [Invoice_No], [Serial_No], [SIFromTransType], [SIFromTransNO], [Product_Id], [Unit_Id], [UnitPrice], [AverageCost], OrderQty, [Quantity], ReturnQty, [TaxP], [TaxV], [DiscountP], [DiscountV], [Post], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])VALUES(" + Session["CompId"].ToString() + ", " + Session["BrandId"].ToString() + ",  " + Session["LocId"].ToString() + ", 'Delete'," + dr["Invoice_No"].ToString() + ", '1','" + dr["SIFromTransType"].ToString() + "', " + dr["SIFromTransNo"].ToString() + ", " + dr["Product_Id"].ToString() + "," + dr["Unit_Id"].ToString() + ", " + dr["UnitPrice"].ToString() + ", " + dr["AverageCost"].ToString() + ",  " + dr["OrderQty"].ToString() + "," + dr["Quantity"].ToString() + ", '0', '0', '0', '0', '0', 'True', '" + true.ToString() + "','" + Session["userId"].ToString() + "','" + DateTime.Now.ToString() + "', '" + Session["userId"].ToString() + "','" + DateTime.Now.ToString() + "')");
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

    public void UpdateRecordForStckableItem(string ProductId, string ItemType, double Quantity, string InvoiceId, string UnitId, string SoId, string Ledgerid, ref SqlTransaction trns)
    {

        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        double Currencyquantity = 0;

        DataTable dt = new DataTable();
        dt = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ProductId, ref trns);
        //for fifo expiry date
        if (ItemType == "FE")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ExpiryDate asc", DataViewRowState.CurrentRows).ToTable();

        }
        //for lifo expiry date
        else if (ItemType == "LE")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ExpiryDate desc", DataViewRowState.CurrentRows).ToTable();
        }
        //for fifo manufacturing date
        else if (ItemType == "FM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ManufacturerDate asc", DataViewRowState.CurrentRows).ToTable();
        }
        //for lifo manufacturing date
        else
            if (ItemType == "LM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ManufacturerDate desc", DataViewRowState.CurrentRows).ToTable();

        }



        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Currencyquantity = Convert.ToDouble(dt.Rows[i]["Quantity"].ToString());

            if (Quantity == 0)
            {
                break;
            }
            string sql = "select SUM(quantity) from Inv_StockBatchMaster where Field3='" + dt.Rows[i]["Trans_Id"].ToString() + "' and InOut='O'";

            if (da.return_DataTable(sql, ref trns).Rows.Count > 0)
            {

                try
                {



                    if (Currencyquantity == Convert.ToDouble(da.return_DataTable(sql, ref trns).Rows[0][0].ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        double Remqty = 0;

                        Remqty = Currencyquantity - Convert.ToDouble(da.return_DataTable(sql, ref trns).Rows[0][0].ToString());
                        if (Remqty > Quantity)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = 0;
                        }
                        else
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = Quantity - Remqty;

                        }

                    }
                }
                catch
                {
                    if (Currencyquantity > Quantity)
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                        Quantity = 0;
                    }
                    else
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                        Quantity = Quantity - Currencyquantity;

                    }

                }

            }
            else
            {
                if (Currencyquantity > Quantity)
                {

                    ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = 0;
                }
                else
                {

                    ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "SI", InvoiceId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = Quantity - Currencyquantity;

                }
            }
        }

    }
    public string GetDocumentNumberVoucher()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(StrCompId, "160", "184");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += StrCompId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += StrBrandId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += StrLocationId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["SessionDepId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }

            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(objVoucherHeader.GetVoucherAll(StrCompId.ToString(), StrBrandId, StrLocationId, Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(objVoucherHeader.GetVoucherAll(StrCompId.ToString(), StrBrandId, StrLocationId, Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(objVoucherHeader.GetVoucherAll(StrCompId.ToString(), StrBrandId, StrLocationId, Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        string strCustomerId = string.Empty;
        if (txtCustomer.Text != "")
        {
            try
            {
                strCustomerId = txtCustomer.Text.Trim().Split('/')[3].ToString();
                Int32.TryParse(strCustomerId, out strSiCustomerId);
                Session["ContactId"] = strCustomerId;
            }
            catch
            {
                DisplayMessage("Select customer in suggestion only");
                GetCreditInfo();
                txtCustomer.Text = "";
                txtCustomer.Focus();
                Session["ContactId"] = null;
            }

            DataTable dt = ObjContactMaster.GetContactTrueById(strCustomerId);
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Customer Name");
                GetCreditInfo();
                txtCustomer.Text = "";
                txtCustomer.Focus();

            }
            else
            {
                HttpContext.Current.Session["SI_UnAdjustedCreditNote"] = null;

                if (strCustomerId != "0" && strCustomerId != "")
                {
                    if (editid.Value == string.Empty && ObjCustmer.isCustomerBlock(strCustomerId, Session["CompId"].ToString(), Session["BrandId"].ToString()) == true)
                    {
                        DisplayMessage("This customer has been blocked so you can not generate any invoice");
                        txtCustomer.Text = string.Empty;
                        return;
                    }

                    if (txtShipCustomerName.Text == "")
                    {
                        txtShipCustomerName.Text = txtCustomer.Text;
                    }

                    DataTable dtAddress = new DataTable();

                    dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId);
                    //here we are filtering default address of customer

                    dtAddress = new DataView(dtAddress, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();


                    if (dtAddress != null && dtAddress.Rows.Count > 0)
                    {
                        if (txtInvoiceTo.Text == "")
                        {
                            txtInvoiceTo.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                        }
                        if (txtShipingAddress.Text == "")
                        {
                            txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                        }

                    }
                    else
                    {
                        txtShipingAddress.Text = "";
                    }

                    DataTable dtCust = ObjCustmer.GetCustomerAllDataByCustomerId(StrCompId, StrBrandId, strCustomerId);
                    if (dtCust.Rows.Count > 0)
                    {
                        Session["CustomerAccountId"] = dtCust.Rows[0]["Account_No"].ToString();
                    }
                }





                if (RdoSo.Checked == true)
                {
                    RdoSo_CheckedChanged(null, null);
                }
                string[] CustomerName = txtCustomer.Text.Split('/');
                if (gvProduct != null || gvProduct.Rows.Count > 0)
                {
                    if (CustomerName.Length > 1)
                    {
                        foreach (GridViewRow gvRow in gvProduct.Rows)
                        {

                            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");


                            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");

                            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
                            TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
                            TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
                            TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
                            TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
                            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
                            TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");



                            if (lblgvUnitPrice.Text == "")
                            {
                                lblgvUnitPrice.Text = "0";
                            }
                            if (txtgvSalesQuantity.Text == "")
                            {
                                txtgvSalesQuantity.Text = "0";
                            }


                            try
                            {

                                lblgvUnitPrice.Text = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", CustomerName[1].ToString(), hdngvProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                                lblgvUnitPrice.Text = (Convert.ToDouble(lblgvUnitPrice.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                                lblgvUnitPrice.Text = ObjSysParam.GetCurencyConversionForInv(strCurrencyId.ToString(), lblgvUnitPrice.Text);


                            }
                            catch
                            {

                                lblgvUnitPrice.Text = "0";

                            }




                            string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());

                            lblgvQuantityPrice.Text = strcalc[0].ToString();
                            txtgvTotal.Text = strcalc[5].ToString();
                            txtgvDiscountV.Text = strcalc[2].ToString();
                            txtgvTaxV.Text = Convert_Into_DF(strcalc[4].ToString());

                        }
                        HeadearCalculateGrid();
                    }
                    else
                    {
                        DisplayMessage("Customer id is not found with customer name");
                        GetCreditInfo();
                        txtCustomer.Text = "";
                        txtCustomer.Focus();
                        return;
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Select Customer Name");
            GetCreditInfo();
            txtCustomer.Focus();
            Session["ContactId"] = null;
        }
        GetCreditInfo();

    }


    public void GetCreditInfo()
    {
        //bool IsCredit = false;

        //here we will show credit terms and condition .
        int otherAcId = 0;
        try
        {
            otherAcId = objAcAccountMaster.GetCustomerAccountByCurrency(txtCustomer.Text.Split('/')[3].ToString(), ddlCurrency.SelectedValue);
            //IsCredit = Convert.ToBoolean(ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCustomer.Text.Split('/')[1].ToString()).Rows[0]["Field41"].ToString());
        }
        catch
        {

        }


        if (otherAcId > 0)
        {
            trCreditInfo.Visible = true;

            DataTable dtCreditParameter = objCustomerCreditParam.GetRecord_By_CustomerId(txtCustomer.Text.Split('/')[3].ToString());


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
                    lblCurrentBalance.Text = SystemParameter.GetAmountWithDecimal(objDa.get_SingleValue(sql), Session["LoginLocDecimalCount"].ToString());
                }
                catch (Exception ex)
                {
                    lblCurrentBalance.Text = "";
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
                lblCurrentBalance.Text = "";
            }
        }
        else
        {
            trCreditInfo.Visible = false;
        }
    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strEmployeeId = Emp_ID;
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
    }
    #region Bin Section
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
    #endregion
    #region AutocompleteRegion
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionALLContactList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ems = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";

        DataTable dtCon = ems.GetAllContactAsPerFilterText(prefixText, id);
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionContactList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ems = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";
        if (HttpContext.Current.Session["ContactID"] == null)
        {
            id = "0";
        }
        else
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }

        DataTable dtCon = ems.GetAllContactAsPerFilterText(prefixText, id);
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


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
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
        DataTable dt = AddressN.GetDistinctAddressName(prefixText);
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCreditNote(string prefixText, int count, string contextKey)
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
            PostType = " Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();

        string PageSize = Session["GridSize"].ToString();
        string DPageSize = ddlPageSize.SelectedValue.ToString();
        if (DPageSize == "")
            DPageSize = "0";
        if (int.Parse(DPageSize) > int.Parse(PageSize))
            PageSize = DPageSize;

        string BatchNo = "1";
        DataTable dtRecord = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, PostType, true.ToString(), BatchNo, PageSize, "", "", "");
        DataTable dt = objSInvHeader.GetSInvHeaderAllTrueByIndex(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, PostType, false.ToString(), BatchNo, PageSize, "", "", "");

        //DataTable dt = new DataView(objSInvHeader.GetSInvHeaderAllTrue(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString()), PostStatus, "Invoice_Date Desc", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
        Session["dtSInvoice"] = dt;
        Session["dtFilter_Sale_inv_mstr"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";
        GvSalesInvoice.Dispose();
        dt.Dispose();
        //AllPageCode();

        generatePager(int.Parse(dtRecord.Rows[0][0].ToString()), int.Parse(PageSize), 1);

        object sumObject;
        sumObject = dt.Compute("Sum(LocalGrandTotal)", "");


        lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), Session["LocCurrencyId"].ToString());

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
    public string GetSalesOrder(string TransType)
    {
        string InvoiceType = string.Empty;
        if (TransType.Trim() == "S")
        {
            InvoiceType = "Sales Order(Search From S)";
        }
        if (TransType.Trim() == "D")
        {
            InvoiceType = "Direct(Search From D)";
        }
        return InvoiceType;
    }
    public string GetSalesOrderNumber(string So_TransId)
    {
        string SalesOrderNumber = string.Empty;
        if (So_TransId.Trim() != "" && So_TransId.Trim() != "0")
        {
            DataTable Dtsalesorder = objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, So_TransId);

            if (Dtsalesorder.Rows.Count > 0)
            {
                SalesOrderNumber = Dtsalesorder.Rows[0]["SalesOrderNo"].ToString();
            }
        }
        return SalesOrderNumber;
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
    public void Reset()
    {
        rbtNew.Checked = false;
        rbtEdit.Checked = true;
        rbtNew.Visible = false;
        rbtEdit.Visible = false;
        ddlTransType.Enabled = true;
        Session["Expenses_Tax_Sales_Invoice"] = null;
        Session["JobCardId"] = null;
        txtSInvNo.Text = objSOrderHeader.GetAutoID(StrCompId, StrBrandId, StrLocationId);
        txtSInvDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtSInvNo.ReadOnly = false;
        txtCustomer.Text = "";
        ddlOrderType.SelectedValue = "--Select--";
        RdoSo.Checked = false;
        RdoWithOutSo.Checked = false;
        PnlProductSearching.Visible = false;
        FillCurrency();
        hdnTransType.Value = "";
        if (Lbl_Tab_New.Text == "View")
        {
            btnSInvSave.Enabled = false;
            btnPost.Enabled = false;
            BtnReset.Enabled = false;
        }
        else
        {
            btnSInvSave.Enabled = true;
            btnPost.Enabled = true;
            BtnReset.Enabled = true;
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

        txtAccountNo.Text = "";
        txtInvoiceCosting.Text = "";
        txtShift.Text = "";
        txtTender.Text = "";
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
        gvProduct.DataSource = null;
        gvProduct.DataBind();
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
        txtValueBin.Text = "";
        txtCustValueBin.Text = "";
        txtCustValue.Text = "";
        txtValueBinDate.Text = "";
        txtValueDate.Text = "";
        txtCustValue.Visible = false;
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtCustValueBin.Visible = false;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtSInvNo.Text = GetDocumentNumber();
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
        GridExpenses.DataSource = null;
        GridExpenses.DataBind();
        gvPayment.DataSource = null;
        gvPayment.DataBind();
        btnPaymentReset_Click(null, null);
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";
        txtFCExpAmount.Text = "0";
        txtExpExchangeRate.Text = "0";
        txtExpCharges.Text = "";
        rbtnFormView.Visible = false;
        rbtnAdvancesearchView.Visible = false;
        btnAddNewProduct.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        Session["DtSearchProduct"] = null;
        ViewState["ApprovalStatus"] = null;
        ViewState["dtProductSearch"] = null;
        ViewState["Dtproduct"] = null;
        ViewState["dtPo"] = null;
        gvSerachGrid.DataSource = null;
        gvSerachGrid.DataBind();
        ddlPaymentMode.SelectedIndex = 0;
        txtCustomer.Enabled = true;
        setSymbol();
        //AllPageCode();
        ViewState["DtTax"] = null;
        ViewState["dtTaxHeader"] = null;
        LoadStores();
        txtPriceafterdiscountheader.Text = "0";
        Application.UnLock();
        Session["DtAdvancePayment"] = null;
        Filladvancepaymentgrid((DataTable)Session["DtAdvancePayment"]);
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
        txtCondition1.Content = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();
        trCreditInfo.Visible = false;
        Session["Temp_Product_Tax_SINV"] = null;
        HttpContext.Current.Session["SI_UnAdjustedCreditNote"] = null;
        strSiCustomerId = 0;
        strSiCurrencyId = 0;
        txtContactPerson.Text = "";
        hdnContactId.Value = "";
        Session["contactId"] = null;
        ResetTransPort();
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
    #endregion
    #region Add Request Section

    private void FillSalesOrderNo()
    {
        //Check Query Like Sales Quotation No.
        DataTable DtSalesorderRequest = new DataTable();
        DataTable dtSalesOrderHeader = objSOrderHeader.GetSOHeaderAllTrue(StrCompId, StrBrandId, StrLocationId);
        try
        {
            dtSalesOrderHeader = new DataView(dtSalesOrderHeader, "CustomerId='" + txtCustomer.Text.Trim().Split('/')[3].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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

        if (DtSalesorderRequest != null && DtSalesorderRequest.Rows.Count > 0)
        {
            try
            {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProduct, DtSalesorderRequest, "", "");
            }
            catch
            { }
        }
        else
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
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
            ListItem li = new ListItem("Multi", "Multi");
            ddlPaymentMode.Items.Add(li);

            ddlPaymentMode.SelectedIndex = 0;
        }
        else
        {
            ddlPaymentMode.Items.Insert(0, "--Select--");
            ddlPaymentMode.SelectedIndex = 0;
        }

        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
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
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    { return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString()); }

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
    #region Calculations
    protected void txtTaxP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, strCurrencyId, true, Session["DBConnection"].ToString());
        txtTaxV.Text = Convert_Into_DF(str[4].ToString());
        txtGrandTotal.Text = str[5].ToString();
        txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }
        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        TaxDiscountFromHeader(false);
    }
    protected void txtTaxV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, strCurrencyId, true, Session["DBConnection"].ToString());
        txtTaxP.Text = str[3].ToString();
        txtGrandTotal.Text = str[5].ToString();
        txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }
        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        TaxDiscountFromHeader(false);
    }
    protected void txtDiscountP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, txtDiscountP.Text, "", false, strCurrencyId, false, Session["DBConnection"].ToString());
        txtTaxV.Text = Convert_Into_DF(str[4].ToString());
        txtDiscountV.Text = str[2].ToString();
        txtGrandTotal.Text = str[5].ToString();
        txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }
        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        try
        {
            txtPriceafterdiscountheader.Text = SetDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString());
        }
        catch
        {
            txtPriceafterdiscountheader.Text = "0";
        }
        TaxDiscountFromHeader(true);

        CalculationchangeIntaxGridview();
        TaxCalculationWithDiscount();
    }
    protected void txtDiscountV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, strCurrencyId, false, Session["DBConnection"].ToString());
        txtTaxV.Text = Convert_Into_DF(str[4].ToString());
        txtDiscountP.Text = str[6].ToString();
        txtGrandTotal.Text = str[5].ToString();
        txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }
        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        try
        {
            txtPriceafterdiscountheader.Text = SetDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString());
        }
        catch
        {
            txtPriceafterdiscountheader.Text = "0";
        }

        TaxDiscountFromHeader(true);
        CalculationchangeIntaxGridview();
        TaxCalculationWithDiscount();
    }
    public void HeadearCalculateGrid()
    {
        Double grosstotal = 0;
        Double Nettax = 0;
        Double Nettax_temp = 0;
        Double NetDiscount = 0;
        Double Nettotal = 0;
        Double TotalQty = 0;
        txtAmount.Text = "0";
        txtTaxV.Text = Convert_Into_DF("0");
        txtDiscountV.Text = "0";
        txtTotalQuantity.Text = "0";
        txtGrandTotal.Text = "0";
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
            TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
            TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");


            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }
            if (txtgvTaxV.Text == "")
            {
                txtgvTaxV.Text = Convert_Into_DF("0");
            }
            if (txtgvDiscountV.Text == "")
            {
                txtgvDiscountV.Text = "0";
            }
            if (txtgvSalesQuantity.Text == "")
            {
                txtgvSalesQuantity.Text = "0";
            }
            if (txtgvTotal.Text == "")
            {
                txtgvTotal.Text = "0";
            }
            if (lblgvQuantityPrice.Text == "")
            {
                lblgvQuantityPrice.Text = "0";
            }
            TotalQty += Convert.ToDouble(txtgvSalesQuantity.Text);
            grosstotal += Convert.ToDouble(lblgvQuantityPrice.Text);

            double Unit_Price = Convert.ToDouble(lblgvUnitPrice.Text);
            double Discount_Percent = Convert.ToDouble(txtgvDiscountP.Text);
            double Tax_Percent = Convert.ToDouble(txtgvTaxP.Text);
            double Sales_Qty = Convert.ToDouble(txtgvSalesQuantity.Text);

            double Net_Unit_Price = Unit_Price * Sales_Qty;
            double Net_Discount_Value = ((Net_Unit_Price * Discount_Percent) / 100);
            double Unit_Price_After_Discount = Net_Unit_Price - ((Net_Unit_Price * Discount_Percent) / 100);
            double Net_Tax_Value = (Unit_Price_After_Discount * Tax_Percent) / 100;
            Nettax = Nettax + Convert.ToDouble(Convert_Into_DF(Net_Tax_Value.ToString()));
            NetDiscount = NetDiscount + Net_Discount_Value;

            //double Tax_Value_Temp = Convert.ToDouble(SetDecimal(txtgvTaxV.Text));
            //double S_Qty_Temp = Convert.ToDouble(SetDecimal(txtgvSalesQuantity.Text));

            //double tax_Temp = Convert.ToDouble(SetDecimal((Tax_Value_Temp * S_Qty_Temp).ToString()));
            //Nettax = Convert.ToDouble(SetDecimal((Nettax + tax_Temp).ToString()));
            // Nettax += Convert.ToDouble(txtgvTaxV.Text) * Convert.ToDouble(txtgvSalesQuantity.Text);
            // NetDiscount += Convert.ToDouble(txtgvDiscountV.Text) * Convert.ToDouble(txtgvSalesQuantity.Text);

            Nettotal += Convert.ToDouble(txtgvTotal.Text);
        }
        string[] strTotal = Common.TaxDiscountCaluculation(grosstotal.ToString(), "", "", Nettax.ToString(), "", NetDiscount.ToString(), false, strCurrencyId, false, Session["DBConnection"].ToString());
        txtAmount.Text = SetDecimal(grosstotal.ToString());
        txtDiscountV.Text = strTotal[2].ToString();
        txtTaxV.Text = Convert_Into_DF(Nettax.ToString());
        txtTaxP.Text = strTotal[3].ToString();
        txtDiscountP.Text = strTotal[1].ToString();

        //txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(strTotal[5].ToString()), "0");
        txtGrandTotal.Text = SetDecimal(strTotal[5].ToString());
        try
        {
            txtPriceafterdiscountheader.Text = SetDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text)).ToString());
        }
        catch
        {
            txtPriceafterdiscountheader.Text = "0";
        }

        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }

        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        txtNetAmountwithexpenses.Text = Common.Get_Roundoff_Amount_By_Location(txtNetAmountwithexpenses.Text, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
        txtTotalQuantity.Text = SetDecimal(TotalQty.ToString());
        CalculationchangeIntaxGridview();
    }
    #endregion
    #region Grid Calculations
    public DataTable SavedGridRecordindatatble()
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            DropDownList ddlUnitName = (DropDownList)gvRow.FindControl("ddlUnitName");
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
            TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            Label lblgvSoldQuantity = (Label)gvRow.FindControl("lblgvSoldQuantity");
            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
            TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
            TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
            dr["Trans_Id"] = lblTransId.Text;
            dr["SalesOrderNo"] = ((Label)gvRow.FindControl("lblSONo")).Text;
            dr["SoID"] = ((Label)gvRow.FindControl("lblSOId")).Text;
            dr["Serial_No"] = lblgvSNo.Text;
            dr["Product_Id"] = hdngvProductId.Value;
            dr["ProductName"] = lblgvProductName.Text;
            dr["ProductDescription"] = lblgvProductDescription.Text;
            dr["UnitId"] = ddlUnitName.SelectedValue;
            dr["Unit_Name"] = lblgvUnit.Text;
            dr["UnitPrice"] = SetDecimal(lblgvUnitPrice.Text);
            dr["Quantity"] = SetDecimal(txtgvSalesQuantity.Text);
            dr["FreeQty"] = lblgvFreeQuantity.Text;
            dr["OrderQty"] = lblgvOrderqty.Text;
            dr["TaxP"] = SetDecimal(txtgvTaxP.Text);
            dr["TaxV"] = Convert_Into_DF(txtgvTaxV.Text);
            dr["DiscountP"] = SetDecimal(txtgvDiscountP.Text);
            dr["DiscountV"] = SetDecimal(txtgvDiscountV.Text);
            dr["SoldQty"] = SetDecimal(lblgvSoldQuantity.Text);
            dr["SysQty"] = SetDecimal(lblgvSystemQuantity.Text);
            dt.Rows.Add(dr);
        }
        return dt;
    }
    public void TaxDiscountFromHeader(bool IsDiscount)
    {
        foreach (GridViewRow Row in gvProduct.Rows)
        {
            TextBox lblgvQuantity = (TextBox)Row.FindControl("txtgvSalesQuantity");
            string[] str;
            if (IsDiscount)
            {
                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblgvUnitPrice")).Text, lblgvQuantity.Text, "", Convert_Into_DF(((TextBox)Row.FindControl("txtgvTaxV")).Text), txtDiscountP.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());
                //  ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("txtgvDiscountV")).Text = str[2].ToString();
                ((TextBox)Row.FindControl("txtgvDiscountP")).Text = str[6].ToString();
                ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
            }
            else
            {
                string UnitTaxValue = ObjSysParam.GetCurencyConversionForInv(strCurrencyId, (Convert.ToDouble(txtTaxV.Text) / Convert.ToDouble(lblgvQuantity.Text)).ToString());
                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("lblgvUnitPrice")).Text, lblgvQuantity.Text, "", UnitTaxValue.ToString(), "", ((TextBox)Row.FindControl("txtgvDiscountV")).Text, true, strCurrencyId, true, Session["DBConnection"].ToString());
                //    ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("txtgvTaxP")).Text = str[3].ToString();
                ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
            }
        }
    }
    protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        refreshGvProduct(gvRow);
        //GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
        //TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
        //Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        //TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
        //TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
        //TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
        //TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
        //TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        //HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
        //if (lblgvUnitPrice.Text == "")
        //{
        //    lblgvUnitPrice.Text = "0";
        //}
        //if (txtgvSalesQuantity.Text == "")
        //{
        //    txtgvSalesQuantity.Text = "0";
        //}
        //double PriceValue = double.Parse(lblgvUnitPrice.Text);
        //double QtyValue = double.Parse(txtgvSalesQuantity.Text);
        //double AmountValue = PriceValue * QtyValue;
        //double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
        //bool IsValidDiscount = IsApplyDiscount();
        //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
        //    txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();

        //if (!IsValidDiscount)
        //    AmntAfterDiscnt = AmountValue;

        //double TotalTax = TaxCalculation(AmntAfterDiscnt.ToString(), hdngvProductId.Value);

        //double DiscountPercentage = (double.Parse(txtgvDiscountV.Text) * 100 / AmountValue);

        //string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), txtgvDiscountP.Text, "", true, strCurrencyId, false);
        //lblgvQuantityPrice.Text = AmountValue.ToString();
        //txtgvDiscountP.Text = DiscountPercentage.ToString();
        ////txtgvDiscountV.Text = strtotal[2].ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        //txtgvTaxV.Text = TotalTax.ToString();
        //txtgvTotal.Text = (AmntAfterDiscnt + TotalTax).ToString();
        //GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
        //string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text)).ToString();
        //childGridCalculation(GridChild, PriceAfterDiscount);
        //HeadearCalculateGrid();
        ////AllPageCode();
        SetGridViewDecimal();
    }
    protected void txtgvSalesQuantity_TextChanged(object sender, EventArgs e)
    {
        //GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //refreshGvProduct(gvRow);
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
        Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
        TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
        TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
        Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
        TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
        TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");

        Label lblgvSerialNo = (Label)gvRow.FindControl("lblgvSerialNo");

        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }
        if (txtgvSalesQuantity.Text == "")
        {
            txtgvSalesQuantity.Text = "0";
        }
        if (lblgvSystemQuantity.Text == "")
        {
            lblgvSystemQuantity.Text = "0";
        }



        //modify by jitendra upadhyay for delivery voucher concept
        //here validation not require because stock deducted on delivery voucher page so
        //modify on 30/12/2015
        if (RdoWithOutSo.Checked)
        {
            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvRow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "S")
            {

                if (Convert.ToDouble(txtgvSalesQuantity.Text) > Convert.ToDouble(lblgvSystemQuantity.Text))
                {
                    //DisplayMessage("Invoice Quantity should be less than or equal to system quantity");
                    //txtgvSalesQuantity.Text = "0";
                }
            }
        }
        else
        {
            if (!Convert.ToBoolean(objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, ((Label)gvRow.FindControl("lblSOId")).Text).Rows[0]["IsdeliveryVoucher"].ToString()))
            {

                if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvRow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "S")
                {

                    if (Convert.ToDouble(txtgvSalesQuantity.Text) > Convert.ToDouble(lblgvSystemQuantity.Text))
                    {
                        //DisplayMessage("Invoice Quantity should be less than or equal to system quantity");
                        //txtgvSalesQuantity.Text = "0";
                    }
                }
            }
        }

        double PriceValue = double.Parse(lblgvUnitPrice.Text);
        double QtyValue = double.Parse(txtgvSalesQuantity.Text);
        double AmountValue = PriceValue * QtyValue;
        double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
        bool IsValidDiscount = IsApplyDiscount();
        if (IsValidDiscount)
            txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();

        if (!IsValidDiscount)
            AmntAfterDiscnt = AmountValue;

        double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblgvSerialNo.Text);
        TotalTax = TotalTax / QtyValue;
        if (Double.IsNaN(TotalTax) || Double.IsInfinity(TotalTax))
        {
            TotalTax = 0;
        }
        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), txtgvDiscountP.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());
        if (RdoWithOutSo.Checked)
        {
            lblgvOrderqty.Text = txtgvSalesQuantity.Text;
        }
        lblgvQuantityPrice.Text = AmountValue.ToString();
        txtgvDiscountP.Text = strtotal[1].ToString();
        txtgvDiscountV.Text = strtotal[2].ToString();
        txtgvTaxP.Text = Get_Tax_Percentage(hdngvProductId.Value, lblgvSerialNo.Text).ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxV.Text = Convert_Into_DF(TotalTax.ToString());
        //txtgvTotal.Text = (AmntAfterDiscnt + (TotalTax * QtyValue)).ToString();
        txtgvTotal.Text = (AmntAfterDiscnt + ((AmntAfterDiscnt * double.Parse(txtgvTaxP.Text)) / 100)).ToString();
        HeadearCalculateGrid();
        //AllPageCode();
    }
    protected void txtgvTaxP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        refreshGvProduct(gvRow);
        SetGridViewDecimal();
        //GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

        //Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
        //TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
        //TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
        //Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        //TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
        //TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
        //TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
        //TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
        //TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        //if (lblgvUnitPrice.Text == "")
        //{
        //    lblgvUnitPrice.Text = "0";
        //}
        //if (txtgvSalesQuantity.Text == "")
        //{
        //    txtgvSalesQuantity.Text = "0";
        //}
        //string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, "", "", txtgvDiscountV.Text, true, strCurrencyId, true);
        //txtgvTaxV.Text = strtotal[4].ToString();
        //txtgvTotal.Text = strtotal[5].ToString();
        //HeadearCalculateGrid();
        ////AllPageCode();
    }
    protected void txtgvTaxV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
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
        Label lblgvSerialNo = (Label)gvRow.FindControl("lblgvSerialNo");
        if (lblgvUnitPrice.Text == "")
        {
            lblgvUnitPrice.Text = "0";
        }
        if (txtgvSalesQuantity.Text == "")
        {
            txtgvSalesQuantity.Text = "0";
        }
        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", Convert_Into_DF(txtgvTaxV.Text), "", txtgvDiscountV.Text, true, strCurrencyId, true, Session["DBConnection"].ToString());
        txtgvTaxP.Text = Get_Tax_Percentage(hdngvProductId.Value, lblgvSerialNo.Text).ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        HeadearCalculateGrid();
    }
    protected void txtgvDiscountP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        refreshGvProduct(gvRow);
        SetGridViewDecimal();
        //GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
        //TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
        //TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
        //Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        //TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
        //TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
        //TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
        //TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
        //TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        //HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
        //if (lblgvUnitPrice.Text == "")
        //{
        //    lblgvUnitPrice.Text = "0";
        //}
        //if (txtgvSalesQuantity.Text == "")
        //{
        //    txtgvSalesQuantity.Text = "0";
        //}

        //double PriceValue = double.Parse(lblgvUnitPrice.Text);
        //double QtyValue = double.Parse(txtgvSalesQuantity.Text);
        //double AmountValue = PriceValue * QtyValue;
        //double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text))/100;
        //bool IsValidDiscount = IsApplyDiscount();

        //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
        //    txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();

        //if (!IsValidDiscount)
        //    AmntAfterDiscnt = AmountValue;

        //double TotalTax = TaxCalculation(AmntAfterDiscnt.ToString(), hdngvProductId.Value);
        //TotalTax = TotalTax / QtyValue;
        //double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);

        //string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), txtgvDiscountP.Text, "", true, strCurrencyId, false);
        //txtgvDiscountV.Text = strtotal[2].ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        //txtgvTaxV.Text = TotalTax.ToString();
        //txtgvTotal.Text = (AmntAfterDiscnt + (TotalTax * QtyValue)).ToString();
        //GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
        //string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text)).ToString();
        //childGridCalculation(GridChild, PriceAfterDiscount);
        //HeadearCalculateGrid();
        ////AllPageCode();
    }
    protected void txtgvDiscountV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
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
        double AmntAfterDiscnt = AmountValue - double.Parse(txtgvDiscountV.Text);
        bool IsValidDiscount = IsApplyDiscount();

        if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();

        if (!IsValidDiscount)
            AmntAfterDiscnt = AmountValue;

        double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblgvSerialNo.Text);

        double DiscountPercentage = (double.Parse(txtgvDiscountV.Text) * 100 / AmountValue);

        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), "", txtgvDiscountV.Text, true, strCurrencyId, false, Session["DBConnection"].ToString());
        txtgvDiscountP.Text = DiscountPercentage.ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxP.Text = Get_Tax_Percentage(hdngvProductId.Value, lblgvSerialNo.Text).ToString();
        txtgvTaxV.Text = Convert_Into_DF(TotalTax.ToString());
        //txtgvTotal.Text = (AmntAfterDiscnt + TotalTax).ToString();
        txtgvTotal.Text = (AmntAfterDiscnt + ((AmntAfterDiscnt * double.Parse(txtgvTaxP.Text)) / 100)).ToString();
        GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);
        HeadearCalculateGrid();
        //AllPageCode();

    }

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

        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), txtgvDiscountP.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());
        txtgvDiscountV.Text = strtotal[2].ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxP.Text = Get_Tax_Percentage(hdngvProductId.Value, lblgvSerialNo.Text).ToString();
        txtgvTaxV.Text = Convert_Into_DF(TotalTax.ToString());
        //txtgvTotal.Text = (AmntAfterDiscnt + (TotalTax * QtyValue)).ToString();
        txtgvTotal.Text = (AmntAfterDiscnt + ((AmntAfterDiscnt * double.Parse(txtgvTaxP.Text)) / 100)).ToString();
        txtgvTotal.Text = GetCurrency(Session["LocCurrencyId"].ToString(), txtgvTotal.Text).Split('/')[0].ToString();
        GridView GridChild = (GridView)gvRow.FindControl("gvchildGrid");
        string PriceAfterDiscount = (Convert.ToDouble(lblgvUnitPrice.Text) - Convert.ToDouble(txtgvDiscountV.Text)).ToString();
        childGridCalculation(GridChild, PriceAfterDiscount);


        HeadearCalculateGrid();
        //lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
        //lblgvFreeQuantity.Text = SetDecimal(lblgvFreeQuantity.Text);
        //lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
        //lblgvSoldQuantity.Text = SetDecimal(lblgvSoldQuantity.Text);
        //lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
        //txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
        //lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
        //txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
        //txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
        //txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
        //txtgvTaxV.Text = Convert_Into_DF(txtgvTaxV.Text);

        //AllPageCode();
    }

    public void SetGridViewDecimal()
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
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


            lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
            lblgvFreeQuantity.Text = SetDecimal(lblgvFreeQuantity.Text);
            lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
            lblgvSoldQuantity.Text = SetDecimal(lblgvSoldQuantity.Text);
            lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
            txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
            txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
            txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
            txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
            txtgvTaxV.Text = Convert_Into_DF(txtgvTaxV.Text);
        }
    }

    public void GridCalculation()
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            refreshGvProduct(gvRow);
            //Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            //Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            //Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            //HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            //Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            //DropDownList ddlUnitName = (DropDownList)gvRow.FindControl("ddlUnitName");
            //TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            //Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
            //TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
            //Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            //Label lblgvSoldQuantity = (Label)gvRow.FindControl("lblgvSoldQuantity");
            //Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            //TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
            //TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
            //TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
            //TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
            //TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
            //Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            //TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
            //Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");
            //if (lblgvUnitPrice.Text == "")
            //{
            //    lblgvUnitPrice.Text = "0";
            //}
            //if (txtgvSalesQuantity.Text == "")
            //{
            //    txtgvSalesQuantity.Text = "0";
            //}


            //double PriceValue = double.Parse(lblgvUnitPrice.Text);
            //double QtyValue = double.Parse(txtgvSalesQuantity.Text);
            //double AmountValue = PriceValue * QtyValue;
            //double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
            //bool IsValidDiscount = IsApplyDiscount();

            //if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
            //    txtgvDiscountV.Text = ((AmountValue- AmntAfterDiscnt)/QtyValue).ToString();

            //if (!IsValidDiscount)
            //    AmntAfterDiscnt = AmountValue;

            //double TotalTax = TaxCalculation(AmntAfterDiscnt.ToString(), hdngvProductId.Value);
            //TotalTax = TotalTax / QtyValue;
            //double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
            //string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), "", txtgvDiscountV.Text, true, strCurrencyId, false);
            //lblgvQuantityPrice.Text = strcalc[0].ToString();
            ////txtgvTotal.Text = (AmntAfterDiscnt + TotalTax).ToString();
            //txtgvTotal.Text = (AmntAfterDiscnt + (TotalTax * QtyValue)).ToString(); //strcalc[5].ToString();
            //lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
            //lblgvFreeQuantity.Text = SetDecimal(lblgvFreeQuantity.Text);
            //lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
            //lblgvSoldQuantity.Text = SetDecimal(lblgvSoldQuantity.Text);
            //lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
            //txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
            //lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
            //txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
            //txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
            //txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
            //txtgvTaxV.Text = SetDecimal(txtgvTaxV.Text);
        }
        HeadearCalculateGrid();
        SetGridViewDecimal();
        //AllPageCode();
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
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == false)
                IsValid = false;
            else
                IsValid = true;
        }
        return IsValid;
    }
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
    public void HeaderTexDiscount()
    {
        if (RdoSo.Checked)
        {
            foreach (GridViewRow Gvr in gvProduct.Rows)
            {
                GridView GvSalesOrderDetail = (GridView)Gvr.FindControl("GvSalesOrderDetail");
                foreach (GridViewRow row in GvSalesOrderDetail.Rows)
                {
                    if (row.Visible)
                    {
                        Label lblgvQuantityPrice = (Label)row.FindControl("lblgvQuantityPrice");
                        TextBox liTaxP = (TextBox)row.FindControl("txtgvTaxP");
                        TextBox liTaxPUnit = (TextBox)row.FindControl("txtgvTaxPUnit");
                        TextBox liTaxV = (TextBox)row.FindControl("txtgvTaxV");
                        TextBox liTaxVUnit = (TextBox)row.FindControl("txtgvTaxVUnit");
                        TextBox liTaxAfterAmount = (TextBox)row.FindControl("txtgvPriceAfterTax");
                        TextBox liDisP = (TextBox)row.FindControl("txtgvDiscountP");
                        TextBox liDisPUnit = (TextBox)row.FindControl("txtgvDiscountPUnit");
                        TextBox liDisV = (TextBox)row.FindControl("txtgvDiscountV");
                        TextBox liDisVUnit = (TextBox)row.FindControl("txtgvDiscountVUnit");
                        TextBox liDisAfterAmount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
                        TextBox ligvTotal = (TextBox)row.FindControl("txtgvTotal");
                        TextBox ligvQuantity = (TextBox)row.FindControl("txtgvSalesQuantity");
                        if (ViewState["Status"].ToString() == "Tax")
                        {
                            liTaxP.Text = SetDecimal(txtTaxP.Text.ToString());
                            liTaxPUnit.Text = liTaxP.Text;
                            liTaxV.Text = Convert_Into_DF((Convert.ToDouble(Convert.ToDouble(lblgvQuantityPrice.Text) * Convert.ToDouble(liTaxP.Text)) / 100).ToString()).ToString();
                            liTaxAfterAmount.Text = Convert_Into_DF((Convert.ToDouble(lblgvQuantityPrice.Text) + Convert.ToDouble(liTaxV.Text)).ToString()).ToString();
                            liTaxVUnit.Text = Convert_Into_DF((Convert.ToDouble(((TextBox)row.FindControl("txtgvTaxV")).Text) / Convert.ToDouble(((TextBox)row.FindControl("txtgvSalesQuantity")).Text)).ToString()).ToString();
                        }
                        if (ViewState["Status"].ToString() == "Dis")
                        {
                            liDisP.Text = SetDecimal(txtDiscountP.Text.ToString()).ToString();
                            liDisPUnit.Text = SetDecimal(txtDiscountP.Text.ToString()).ToString();
                        }
                        if (liDisP.Text != "")
                        {
                            if (liTaxAfterAmount.Text == "")
                            {
                                liTaxAfterAmount.Text = "0";
                            }
                            liDisV.Text = SetDecimal((Convert.ToDouble(Convert.ToDouble(liTaxAfterAmount.Text) * Convert.ToDouble(liDisP.Text)) / 100).ToString()).ToString();
                            if (ligvQuantity.Text == "")
                            {
                                ligvQuantity.Text = "0";
                            }
                            try
                            {
                                liDisVUnit.Text = ((Decimal.Parse(liDisV.Text)) / (Decimal.Parse(ligvQuantity.Text))).ToString();
                            }
                            catch
                            {
                                liDisVUnit.Text = "0";
                            }
                            liDisAfterAmount.Text = SetDecimal((Convert.ToDouble(liTaxAfterAmount.Text) - Convert.ToDouble(liDisV.Text)).ToString()).ToString();
                            ligvTotal.Text = SetDecimal(liDisAfterAmount.Text).ToString();

                        }
                        else
                        {
                            liDisAfterAmount.Text = SetDecimal(liTaxAfterAmount.Text.ToString()).ToString();
                            ligvTotal.Text = SetDecimal(liDisAfterAmount.Text).ToString();
                        }
                    }
                }
            }
        }
        if (RdoWithOutSo.Checked)
        {
            foreach (GridViewRow row in gvProduct.Rows)
            {
                Label lblgvQuantityPrice = (Label)row.FindControl("lblgvQuantityPrice");
                Label liTaxP = (Label)row.FindControl("lblgvTaxP");
                Label liTaxV = (Label)row.FindControl("lblgvTaxV");
                Label liTaxAfterAmount = (Label)row.FindControl("lblgvPriceAfterTax");
                Label liDisP = (Label)row.FindControl("lblgvDiscountP");
                Label liDisV = (Label)row.FindControl("lblgvDiscountV");
                Label liDisAfterAmount = (Label)row.FindControl("lblgvPriceAfterDiscount");
                Label ligvTotal = (Label)row.FindControl("lblgvTotal");
                if (ViewState["Status"].ToString() == "Tax")
                {
                    liTaxP.Text = SetDecimal(txtTaxP.Text.ToString());
                    ((Label)row.FindControl("lblgvTaxPUnit")).Text = txtTaxP.Text;
                    liTaxV.Text = SetDecimal((Convert.ToDouble(Convert.ToDouble(lblgvQuantityPrice.Text) * Convert.ToDouble(liTaxP.Text)) / 100).ToString()).ToString();
                    ((Label)row.FindControl("lblgvTaxVUnit")).Text = ((Decimal.Parse(((Label)row.FindControl("lblgvUnitPrice")).Text) * Decimal.Parse(txtTaxP.Text)) / (100)).ToString();
                    liTaxAfterAmount.Text = SetDecimal((Convert.ToDouble(lblgvQuantityPrice.Text) + Convert.ToDouble(liTaxV.Text)).ToString()).ToString();
                }
                if (ViewState["Status"].ToString() == "Dis")
                {
                    liDisP.Text = SetDecimal(txtDiscountP.Text.ToString()).ToString();
                    ((Label)row.FindControl("lblgvDiscountPUnit")).Text = SetDecimal(txtDiscountP.Text.ToString()).ToString();
                }
                if (liDisP.Text != "")
                {
                    liDisV.Text = SetDecimal((Convert.ToDouble(Convert.ToDouble(liTaxAfterAmount.Text) * Convert.ToDouble(liDisP.Text)) / 100).ToString()).ToString();

                    ((Label)row.FindControl("lblgvDiscountVUnit")).Text = ((Decimal.Parse(((Label)row.FindControl("lblgvDiscountV")).Text)) / (Decimal.Parse(((Label)row.FindControl("lblgvQuantity")).Text))).ToString();
                    liDisAfterAmount.Text = SetDecimal((Convert.ToDouble(liTaxAfterAmount.Text) - Convert.ToDouble(liDisV.Text)).ToString()).ToString();
                    ligvTotal.Text = SetDecimal(liDisAfterAmount.Text).ToString();
                }
                else
                {
                    liDisAfterAmount.Text = SetDecimal(liTaxAfterAmount.Text.ToString()).ToString();
                    ligvTotal.Text = SetDecimal(liDisAfterAmount.Text).ToString();
                }
            }

        }
    }
    private string GetCustomerId()
    {
        string retval = string.Empty;
        if (txtCustomer.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtCustomer.Text.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                retval = txtCustomer.Text.Split('/')[3].ToString();
                DataTable dtCompany = objContact.GetContactTrueById(retval);
                if (dtCompany.Rows.Count > 0)
                { }
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
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    private string GetCustomerId(ref SqlTransaction trns)
    {
        string retval = string.Empty;
        if (txtCustomer.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtCustomer.Text.Trim().Split('/')[0].ToString().Trim(), ref trns);
            if (dtSupp.Rows.Count > 0)
            {
                retval = txtCustomer.Text.Split('/')[3].ToString();
                DataTable dtCompany = objContact.GetContactTrueById(retval, ref trns);
                if (dtCompany.Rows.Count > 0)
                { }
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
    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderType.SelectedValue == "--Select--")
        {
            gvSerachGrid.DataSource = null;
            gvSerachGrid.DataBind();
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            FillPaymentMode();
            FillCurrency();
            txtCustomer.Text = "";
            txtSalesPerson.Text = "";

            txtAccountNo.Text = "";
            txtInvoiceCosting.Text = "";
            txtShift.Text = "";
            txtTender.Text = "";
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
            chkPost.Checked = false;
            btnAddNewProduct.Visible = false;
        }
        else if (ddlOrderType.SelectedValue == "D")
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            FillPaymentMode();
            FillCurrency();
            txtCustomer.Text = "";
            txtSalesPerson.Text = "";

            txtAccountNo.Text = "";
            txtInvoiceCosting.Text = "";
            txtShift.Text = "";
            txtTender.Text = "";
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
            chkPost.Checked = false;
            btnAddNewProduct.Visible = true;
        }
        else if (ddlOrderType.SelectedValue == "Q")
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            FillSalesOrderNo();
            FillPaymentMode();
            FillCurrency();
            txtCustomer.Text = "";
            txtSalesPerson.Text = "";

            txtAccountNo.Text = "";
            txtInvoiceCosting.Text = "";
            txtShift.Text = "";
            txtTender.Text = "";
            txtTotalQuantity.Text = "";
            txtAmount.Text = "0";
            txtTaxP.Text = "0";
            txtTaxV.Text = Convert_Into_DF("0");
            txtNetAmount.Text = "0";
            txtDiscountP.Text = "0";
            txtDiscountV.Text = "0";
            txtGrandTotal.Text = "0";
            txtRemark.Text = "";
            txtCondition1.Content = "";
            chkPost.Checked = false;
            btnAddNewProduct.Visible = false;
        }
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }
    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string strSerialNo = string.Empty;
        bool Isserial = false;
        if (((TextBox)sender).Text != "")
        {


            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchProductName.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    //heer we checking that scaned text is serial number or not
                    //code start
                    if (dt.Rows[0]["Type"].ToString() == "2")
                    {
                        strSerialNo = txtSearchProductName.Text.ToString();
                        Isserial = true;
                    }


                    //here checking if serial based product then valid or not
                    if (Isserial)
                    {
                        if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), ((TextBox)sender).Text))
                        {

                            DisplayMessage("Serial number is invalid");
                            txtSearchProductName.Text = "";
                            txtSearchProductName.Focus();
                            return;
                        }

                        if (ViewState["dtFinal"] != null)
                        {
                            if (new DataView((DataTable)ViewState["dtFinal"], "SerialNo='" + ((TextBox)sender).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                DisplayMessage("Serial number is already exists");
                                txtSearchProductName.Text = "";
                                txtSearchProductName.Focus();
                                return;

                            }
                        }
                    }

                    //code end

                    txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();
                    if (!RdoSo.Checked)
                    {
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
                            dtProduct = new DataView(dtProduct, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtProduct.Rows.Count > 0)
                            {
                                //DisplayMessage("Product Is already exists!");
                                //txtSearchProductName.Focus();
                                //txtSearchProductName.Text = "";
                                //return;
                            }
                        }
                    }



                    addProductbyProductRef(strSerialNo, Isserial);

                    txtSearchProductName.Focus();

                    //if (dt.Rows[0]["MaintainStock"].ToString().Trim() == "SNO")
                    //{
                    //    foreach (GridViewRow gvrow in gvProduct.Rows)
                    //    {
                    //        if (((HiddenField)gvrow.FindControl("hdngvProductId")).Value.Trim() == dt.Rows[0]["ProductId"].ToString().Trim())
                    //        {
                    //            lnkAddSO_Click(((object)((LinkButton)gvrow.FindControl("lnkAddSO"))), e);
                    //            txtSerialNo.Focus();
                    //            break;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    DisplayMessage("No Product Found");
                    txtSearchProductName.Text = "";
                    txtSearchProductName.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchProductName);
            txtSearchProductName.Focus();
        }

        ((TextBox)sender).Text = "";
        ((TextBox)sender).Focus();

    }

    protected void txtProductSerachValue_OnTextChanged(object sender, EventArgs e)
    {
        bool Isserial = false;

        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                DisableOrderList();

                int counter = 0;
                if (gvSerachGrid.Rows.Count == 0)
                {
                    DisplayMessage("Product Not Found");
                    txtProductSerachValue.Text = "";
                    txtProductSerachValue.Focus();
                    return;
                }

                if (txtProductSerachValue.Text != "")
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


                        //here checking if serial based product then valid or not
                        if (Isserial)
                        {
                            if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), ((TextBox)sender).Text))
                            {

                                DisplayMessage("Serial number is invalid");
                                ((TextBox)sender).Text = "";
                                ((TextBox)sender).Focus();
                                return;
                            }

                            if (ViewState["dtFinal"] != null)
                            {
                                if (new DataView((DataTable)ViewState["dtFinal"], "SerialNo='" + ((TextBox)sender).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    DisplayMessage("Serial number is already exists");
                                    ((TextBox)sender).Text = "";
                                    ((TextBox)sender).Focus();
                                    return;

                                }
                            }
                        }

                        //code end
                        ///here we will check that item already added or not in invoice list

                        if (gvProduct.Rows.Count > 0)
                        {




                            DataTable DtProduct = SavedGridRecordindatatble();

                            for (int i = 0; i < DtProduct.Rows.Count; i++)
                            {
                                if (DtProduct.Rows[i]["Product_Id"].ToString().Trim() == dt.Rows[0]["ProductId"].ToString().Trim())
                                {
                                    DtProduct.Rows[i]["Quantity"] = (float.Parse(DtProduct.Rows[i]["Quantity"].ToString()) + 1).ToString();
                                    DataTable dtso = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), DtProduct.Rows[i]["SoID"].ToString().Trim());
                                    if (dtso.Rows.Count > 0)
                                    {
                                        if (dtso.Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                        {
                                            if (Isserial)
                                            {
                                                addSerialfnc(dt.Rows[0]["ProductId"].ToString(), ((TextBox)sender).Text.Trim(), DtProduct.Rows[i]["SoID"].ToString());
                                            }
                                        }
                                    }
                                    counter = 1;
                                    break;
                                }
                            }


                            if (counter == 1)
                            {
                                ViewState["dtPo"] = DtProduct;
                                GetChildGridRecordInViewState();
                                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                                objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
                                GridCalculation();

                                //AllPageCode();
                                txtProductSerachValue.Focus();
                                txtProductSerachValue.Text = "";
                            }

                        }



                        if (counter == 0)
                        {
                            int itemcount = 0;


                            //here we checking that item exist or no tin order list iif we foudn multiple product then enable the check box and if found single then directly add in listt


                            foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                            {
                                if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                                {
                                    itemcount++;
                                }
                            }


                            //counter check start
                            if (itemcount > 0)
                            {
                                //loop start
                                foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                                {


                                    if (itemcount == 1)
                                    {


                                        if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                                        {

                                            DataTable dtso = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((Label)gvrow.FindControl("lblsoid")).Text.Trim());
                                            if (dtso.Rows.Count > 0)
                                            {
                                                if (dtso.Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                                {
                                                    if (Isserial)
                                                    {
                                                        addSerialfnc(dt.Rows[0]["ProductId"].ToString(), ((TextBox)sender).Text.Trim(), ((Label)gvrow.FindControl("lblsoid")).Text);
                                                    }
                                                }
                                            }
                                            chkTrandId_CheckedChanged(((object)((CheckBox)gvrow.FindControl("chkTrandId"))), e);

                                            counter = 1;
                                            txtProductSerachValue.Focus();
                                            txtProductSerachValue.Text = "";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                                        {

                                            ((CheckBox)gvrow.FindControl("chkTrandId")).Enabled = true;
                                            counter = 1;
                                        }
                                    }
                                }

                                //loop end



                            }

                            //counter check end




                        }


                    }


                    if (counter == 0)
                    {
                        DisplayMessage("Product Not Found");
                        txtProductSerachValue.Focus();
                        txtProductSerachValue.Text = "";

                    }
                }



            }
        }


    }




    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        string strSerialNo = string.Empty;
        bool Isserial = false;
        if (txtProductId.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductId.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    //heer we checking that scaned text is serial number or not
                    //code start
                    if (dt.Rows[0]["Type"].ToString() == "2")
                    {
                        strSerialNo = ((TextBox)sender).Text;
                        Isserial = true;
                    }


                    //here checking if serial based product then valid or not
                    if (Isserial)
                    {
                        if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), ((TextBox)sender).Text))
                        {

                            DisplayMessage("Serial number is invalid");
                            ((TextBox)sender).Text = "";
                            ((TextBox)sender).Focus();
                            return;
                        }

                        if (ViewState["dtFinal"] != null)
                        {
                            if (new DataView((DataTable)ViewState["dtFinal"], "SerialNo='" + ((TextBox)sender).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                DisplayMessage("Serial number is already exists");
                                ((TextBox)sender).Text = "";
                                ((TextBox)sender).Focus();
                                return;

                            }
                        }
                    }

                    //code end
                    if (!RdoSo.Checked)
                    {
                        txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                        txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();
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
                            dtProduct = new DataView(dtProduct, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtProduct.Rows.Count > 0)
                            {
                                //DisplayMessage("Product Is already exists!");
                                //txtProductId.Focus();
                                //txtProductId.Text = "";
                                //return;
                            }

                        }
                    }

                    addProductbyProductRef(strSerialNo, Isserial);
                    txtProductId.Focus();
                    //if (dt.Rows[0]["MaintainStock"].ToString().Trim() == "SNO")
                    //{
                    //    foreach (GridViewRow gvrow in gvProduct.Rows)
                    //    {
                    //        if (((HiddenField)gvrow.FindControl("hdngvProductId")).Value.Trim() == dt.Rows[0]["ProductId"].ToString().Trim())
                    //        {
                    //            lnkAddSO_Click(((object)((LinkButton)gvrow.FindControl("lnkAddSO"))), e);
                    //        }
                    //    }
                    //}

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
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string strtext = string.Empty;

        if (ddlProductSerach.SelectedIndex == 0)
        {
            if (txtProductId.Text == "")
            {
                DisplayMessage("Enter Product Id");
                txtProductId.Focus();
                return;
            }
            else
            {
                strtext = txtProductId.Text;
            }

        }
        if (ddlProductSerach.SelectedIndex == 1)
        {
            if (txtSearchProductName.Text == "")
            {
                DisplayMessage("Enter Product Name");
                txtSearchProductName.Focus();
                return;
            }
            else
            {
                strtext = txtSearchProductName.Text;
            }

        }

        addProductbyProductRef(strtext, false);


    }

    public void addProductbyProductRef(string reftext, bool isSerial)
    {

        bool isProductExist = false;

        DataTable DtProduct = new DataTable();
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string UnitCost = string.Empty;
        string SearchCriteria = string.Empty;
        if (txtExchangeRate.Text == "")
        {
            txtExchangeRate.Text = "0";
        }

        DataTable dt = new DataTable();
        if (ddlProductSerach.SelectedIndex == 0)
        {

            dt = objProductM.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

        }
        if (ddlProductSerach.SelectedIndex == 1)
        {
            dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchProductName.Text.ToString());
        }
        if (dt.Rows.Count != 0)
        {
            ProductId = dt.Rows[0]["ProductId"].ToString();

            if (isSerial)
            {

                if (dt.Rows[0]["MaintainStock"].ToString().Trim() == "SNO")
                {
                    addSerialfnc(ProductId, reftext, "");
                }
            }
            //here we will check that product exist or not in gridview

            foreach (GridViewRow gvrow in gvProduct.Rows)
            {

                if (((HiddenField)gvrow.FindControl("hdngvProductId")).Value == dt.Rows[0]["ProductId"].ToString())
                {
                    isProductExist = true;
                    break;
                }
            }
        }
        else
        {
            ProductId = "0";
        }


        if (isProductExist)
        {
            DtProduct = SavedGridRecordindatatble();

            //DtProduct = (DataTable)ViewState["Dtproduct"];

            for (int i = 0; i < DtProduct.Rows.Count; i++)
            {
                if (DtProduct.Rows[i]["Product_Id"].ToString().Trim() == ProductId)
                {
                    DtProduct.Rows[i]["Quantity"] = (float.Parse(DtProduct.Rows[i]["Quantity"].ToString()) + 1).ToString();

                    break;
                }
            }

        }
        else
        {
            UnitId = dt.Rows[0]["UnitId"].ToString(); try
            {
                UnitCost = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", txtCustomer.Text.Split('/')[3].ToString(), ProductId).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();
                UnitCost = (Convert.ToDouble(UnitCost) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                UnitCost = ObjSysParam.GetCurencyConversionForInv(strCurrencyId.ToString(), UnitCost);
            }
            catch
            {
                UnitCost = "0";
            }

            int SerialNO = 0;

            if (ViewState["Dtproduct"] == null)
            {
                DtProduct = CreateProductDataTable();
            }
            else
            {
                DtProduct = (DataTable)ViewState["Dtproduct"];
            }
            if (DtProduct.Rows.Count > 0)
            {
                DataTable dtTemp = new DataView(DtProduct, "", "Serial_No Desc", DataViewRowState.CurrentRows).ToTable();
                SerialNO = Convert.ToInt32(dtTemp.Rows[0]["Serial_No"].ToString());
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
                DtProduct = SavedGridRecordindatatble();
                dr = DtProduct.NewRow();
            }
            Add_Tax_In_Session(UnitCost, "0", ProductId.ToString(), SerialNO.ToString());
            dr["Trans_Id"] = SerialNO.ToString();
            dr["Serial_No"] = SerialNO.ToString();
            dr["SalesOrderNo"] = "0";
            dr["Product_Id"] = ProductId.ToString();
            dr["ProductName"] = GetProductName(ProductId);
            dr["ProductDescription"] = GetProductDescription(ProductId);
            dr["UnitId"] = UnitId;
            dr["Unit_Name"] = GetUnitName(UnitId);
            dr["UnitPrice"] = UnitCost;
            dr["FreeQty"] = "0";
            dr["OrderQty"] = "0";
            dr["Quantity"] = "1";
            dr["TaxP"] = Get_Tax_Percentage(ProductId.ToString(), SerialNO.ToString());
            dr["TaxV"] = Get_Tax_Amount(UnitCost, ProductId.ToString(), SerialNO.ToString());
            dr["DiscountP"] = "0";
            dr["DiscountV"] = "0";
            dr["SoldQty"] = "0";
            dr["SysQty"] = "0";



            try
            {
                dr["SysQty"] = SetDecimal(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ProductId).Rows[0]["Quantity"].ToString());
            }
            catch
            {
                dr["SysQty"] = "0";
            }

            DtProduct.Rows.Add(dr);

        }
        ViewState["Dtproduct"] = (DataTable)DtProduct;
        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        ViewState["Dtproduct"] = DtProduct;
        GetChildGridRecordInViewState();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
        GridCalculation();
        //AllPageCode();

        if (gvProduct.Rows.Count > 0)
        {
            ddlTransType.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
        }
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
    protected void GetGridTotal()
    {
        double fGrossTotal = 0.00f;
        double fTotalQuantity = 0.00f;
        double fTotalTax = 0.00f;
        double fTotalDiscount = 0.00f;
        foreach (GridViewRow gvr in gvProduct.Rows)
        {
            Label lblgvTotal = (Label)gvr.FindControl("lblgvQuantityPrice");
            Label lblgvQuantity = (Label)gvr.FindControl("lblgvQuantity");
            Label lblgvTaxV = (Label)gvr.FindControl("lblgvTaxV");
            Label lblgvDiscountV = (Label)gvr.FindControl("lblgvDiscountV");
            if (lblgvTotal.Text != "")
            {
                fGrossTotal = fGrossTotal + Convert.ToDouble(lblgvTotal.Text);
            }
            if (lblgvQuantity.Text != "")
            {
                fTotalQuantity = fTotalQuantity + Convert.ToDouble(lblgvQuantity.Text);
            }
            if (lblgvTaxV.Text != "")
            {
                fTotalTax = fTotalTax + Convert.ToDouble(lblgvTaxV.Text);
            }
            if (lblgvDiscountV.Text != "")
            {
                fTotalDiscount = fTotalDiscount + Convert.ToDouble(lblgvDiscountV.Text);
            }
        }
        txtTotalQuantity.Text = SetDecimal(fTotalQuantity.ToString());
        txtAmount.Text = SetDecimal(fGrossTotal.ToString());
        txtTaxV.Text = Convert_Into_DF(fTotalTax.ToString());
        txtDiscountV.Text = SetDecimal(fTotalDiscount.ToString());
        if (txtTaxV.Text != "")
        {
            try
            {
                txtTaxP.Text = SetDecimal((fTotalTax * 100 / fGrossTotal).ToString());
            }
            catch
            {
                txtTaxP.Text = SetDecimal("0");
            }
            try
            {
                txtNetAmount.Text = SetDecimal((fGrossTotal + fTotalTax).ToString());
            }
            catch
            {
                txtNetAmount.Text = SetDecimal("0");
            }
        }
        if (txtDiscountV.Text != "")
        {
            try
            {
                txtDiscountP.Text = SetDecimal((fTotalDiscount * 100 / (fGrossTotal + fTotalTax)).ToString());
            }
            catch
            {
                txtDiscountP.Text = SetDecimal("0");
            }
            txtGrandTotal.Text = SetDecimal((fGrossTotal + fTotalTax - fTotalDiscount).ToString());
            txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        }
    }
    protected void imgbtnsearch_Click(object sender, EventArgs e)
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
    protected void ddlProductSerach_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdoWithOutSo.Checked)
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
    protected void ImgbtnRefresh_Click(object sender, EventArgs e)
    {
        ViewState["dtProductSearch"] = null;
        txtProductSerachValue.Text = "";
        ddlProductSerach.SelectedIndex = 1;
        DataTable dtSalesOrder = new DataTable();
        if (ViewState["Dtproduct"] != null)
        {
            dtSalesOrder = (DataTable)ViewState["Dtproduct"];
        }
        else
        {
            dtSalesOrder = fillSOSearhgrid();
        }
        if (dtSalesOrder.Rows.Count != 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dtSalesOrder, "", "");
            ViewState["Dtproduct"] = dtSalesOrder;
        }
        //AllPageCode();
    }
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtSerialDetail = (DataTable)ViewState["dtFinal"];
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        DataTable dt = new DataTable();
        if (RdoSo.Checked)
        {
            dt = (DataTable)ViewState["dtPo"];

            if (dtSerialDetail != null)
            {
                dtSerialDetail = new DataView(dtSerialDetail, "Product_Id='" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "' and SOrderNo='" + ((Label)gvrow.FindControl("lblSOId")).Text + "' ", "", DataViewRowState.CurrentRows).ToTable();



                if (dtSerialDetail.Rows.Count > 0)
                {
                    string s = "SOrderNo Not In('" + ((Label)gvrow.FindControl("lblSOId")).Text + "') or Product_Id Not In('" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "')";
                    dtSerialDetail = new DataView(dtSerialDetail, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
                }
                ViewState["dtFinal"] = dtSerialDetail;
            }

        }
        else
        {
            dt = SavedGridRecordindatatble();
            if (ViewState["DtTax"] != null)
            {
                DataView view = new DataView();
                try
                {
                    view = new DataView((DataTable)ViewState["DtTax"], "productId<>'" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "'", "", DataViewRowState.CurrentRows);
                }
                catch
                {
                }
                ViewState["DtTax"] = view.ToTable();
            }


            if (dtSerialDetail != null)
            {
                dtSerialDetail = new DataView(dtSerialDetail, "Product_Id<>'" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "'", "", DataViewRowState.CurrentRows).ToTable();


                ViewState["dtFinal"] = dtSerialDetail;
            }
        }






        dt = new DataView(dt, "Trans_Id <>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");

        string ProductId = ((HiddenField)gvrow.FindControl("hdngvProductId")).Value;
        //DeleteRowFromTempProductTaxTable(ProductId);


        GridCalculation();

        //for advance payment

        //code start

        DataTable dttemp = new DataTable();
        dttemp = dt.Copy();
        try
        {
            dttemp = new DataView(dttemp, "SoID=" + ((Label)gvrow.FindControl("lblsoid")).Text.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

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
                    dtAdvancePayment = new DataView(dtAdvancePayment, "OrderNo<>'" + ((Label)gvrow.FindControl("lblSONo")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                Session["DtAdvancePayment"] = dtAdvancePayment;

                Filladvancepaymentgrid(dtAdvancePayment);
            }
        }
        //code end



        if (RdoSo.Checked)
        {

            if (ViewState["dtPo"] != null)
            {
                DataTable dtStorePO = dt;


                dt = new DataView((DataTable)ViewState["dtPo"], "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                DataTable dtPO = (DataTable)ViewState["Dtproduct"];
                dtPO.ImportRow(dt.Rows[0]);

                dt = dtPO;
                ViewState["dtPo"] = dtStorePO;
            }

        }
        else
        {
            ViewState["Dtproduct"] = dt;

        }

        ViewState["Dtproduct"] = dt;

        //AllPageCode();

        if (RdoSo.Checked)
        {
            //for refresh the order grid when we delete row in grid
            //created by jitendra upadhyay on 22-022-216

            ImgbtnRefresh_Click(null, null);
        }

        //for enable sales order and without sales order option when product grid have no record
        //for convert without sale sorder invoice with sales order 
        //create by jitendra upadhyay on 22-02-2016

        if (RdoWithOutSo.Checked && gvProduct.Rows.Count == 0)
        {
            RdoSo.Enabled = true;
            RdoWithOutSo.Enabled = true;
        }

        if (gvProduct.Rows.Count > 0)
        {
            //string ProductId = ((HiddenField)gvrow.FindControl("hdngvProductId")).Value;
            //DeleteRowFromTempProductTaxTable(ProductId);
            ddlTransType.Enabled = false;
        }
        else
        {
            ddlTransType.Enabled = true;
        }

    }
    protected void chkTrandId_CheckedChanged(object seder, EventArgs e)
    {




        GridViewRow row = (GridViewRow)((CheckBox)seder).Parent.Parent;


        //get sales person name from sale sorder page 
        //according the requirement of kuwait location

        //create by jitendra upadhyay on 22-01-2016

        //code statrt
        try
        {

            DataTable dtso = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((Label)row.FindControl("lblsoid")).Text.Trim());
            if (dtso.Rows.Count > 0)
            {
                DataTable dtSalesorderdetail = ObjSOrderDetail.GetSODetailBySOrderNo(StrCompId, StrBrandId, StrLocationId, dtso.Rows[0]["Trans_Id"].ToString());
                DataTable Dt_Temp = new DataView(dtSalesorderdetail, "Trans_Id='" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (Dt_Temp != null && Dt_Temp.Rows.Count > 0)
                {
                    if (ddlTransType.Visible == false)
                    {

                    }
                    else if (Dt_Temp.Rows[0]["Trans_Type"].ToString() != ddlTransType.SelectedValue.ToString())
                    {
                        DisplayMessage("Transction type of Sales Order and Sales Invoice does not match");
                        foreach (GridViewRow GVR in gvSerachGrid.Rows)
                        {
                            CheckBox Chk = (CheckBox)GVR.FindControl("chkTrandId");
                            //Chk.Checked = false;
                            Chk.Checked = true;
                        }
                        return;
                    }
                    ddlTransType.Enabled = false;
                    Add_Tax_In_Session_From_Order(Dt_Temp.Rows[0]["UnitPrice"].ToString(), Dt_Temp.Rows[0]["Product_Id"].ToString(), Dt_Temp.Rows[0]["SalesOrderNo"].ToString(), Dt_Temp.Rows[0]["Serial_No"].ToString());
                    //Add_Tax_In_Session_From_Order(Dt_Temp.Rows[0]["UnitPrice"].ToString(), Dt_Temp.Rows[0]["Product_Id"].ToString(), Dt_Temp.Rows[0]["SalesOrderNo"].ToString());
                }

                if (dtso.Rows[0]["Salespersonname"].ToString() != "")
                {
                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                    string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dtso.Rows[0]["SalespersonID"].ToString());

                    txtSalesPerson.Text = dtso.Rows[0]["Salespersonname"].ToString() + "/" + Emp_Code;

                }

                //here we update code for get currency which select ed in sales order

                ddlCurrency.SelectedValue = dtso.Rows[0]["Currency_Id"].ToString().Trim();






                ddlExpCurrency.SelectedValue = ddlCurrency.SelectedValue;
                ddlCurrency_OnSelectedIndexChanged(null, null);

                //txtExchangeRate.Text = SystemParameter.GetExchageRate(Session["LocCurrencyId"].ToString(), ddlCurrency.SelectedValue);

                if (dtso.Rows[0]["SOfromTransType"].ToString().Trim() == "D")
                {
                    txtDiscountP.Text = SetDecimal(dtso.Rows[0]["DiscountP"].ToString());
                    txtDiscountV.Text = SetDecimal(dtso.Rows[0]["DiscountV"].ToString());
                    txtTaxP.Text = SetDecimal(dtso.Rows[0]["TaxP"].ToString());
                    txtTaxV.Text = Convert_Into_DF(dtso.Rows[0]["TaxV"].ToString());

                }

                txtInvoiecRefno.Text = dtso.Rows[0]["Field3"].ToString();
                txtOrderId.Text = dtso.Rows[0]["Field3"].ToString();

                //get tax detail


                DataTable dtRefDetailHeader = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", ((Label)row.FindControl("lblsoid")).Text.Trim());
                try
                {
                    dtRefDetailHeader = new DataView(dtRefDetailHeader, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                dtRefDetailHeader = dtRefDetailHeader.DefaultView.ToTable(true, "Tax_Id", "TaxName", "Tax_Per", "Tax_Value");
                ViewState["dtTaxHeader"] = dtRefDetailHeader;
                LoadStores();
            }
        }
        catch
        {

        }
        //code end

        DataTable dt = new DataTable();

        dt = (DataTable)ViewState["Dtproduct"];

        dt = new DataView((DataTable)ViewState["Dtproduct"], "Trans_Id='" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        DataTable DtParameter = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
        if (DtParameter.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtParameter.Rows[0]["ParameterValue"]) == true)
            {
                try
                {
                    dt.Rows[0]["Quantity"] = "1";
                }
                catch
                {

                }
            }
        }
        if (ViewState["dtPo"] != null)
        {

            DataTable dtPO = (DataTable)ViewState["dtPo"];


            dtPO.ImportRow(dt.Rows[0]);
            dt = new DataView(dtPO, "", "Serial_No Asc", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015


        ViewState["dtPo"] = dt;

        objPageCmn.FillData((object)gvProduct, dt, "", "");
        GridCalculation();

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
        //for showing advance payment

        //code start

        FillAdvancePayment_BYOrderId(((Label)row.FindControl("lblsoid")).Text.Trim(), ((Label)row.FindControl("lblPONo")).Text.Trim());
        //code end
        //AllPageCode();

        if (txtProductSerachValue.Text.Trim() != "")
        {

            FillSerialForScanningsolution(((Label)row.FindControl("lblsoid")).Text.Trim());

            txtProductSerachValue.Text = "";
            txtProductSerachValue.Focus();
        }



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

    protected void gvSerachGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    //((CheckBox)e.Row.FindControl("chkTrandId")).Enabled = false;
                    ((CheckBox)e.Row.FindControl("chkTrandId")).Enabled = true;

                }
            }
        }
    }
    protected void RdoSo_CheckedChanged(object sender, EventArgs e)
    {
        if (txtCustomer.Text != "")
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            gvSerachGrid.DataSource = null;
            gvSerachGrid.DataBind();
            Session["DtAdvancePayment"] = null;
            Filladvancepaymentgrid((DataTable)Session["DtAdvancePayment"]);


            if (RdoSo.Checked)
            {
                ViewState["dtSerial"] = null;
                ViewState["dtFinal"] = null;
                ViewState["Tempdt"] = null;
                ViewState["dtPo"] = null;
                ViewState["DtTax"] = null;
                ViewState["dtTaxHeader"] = null;
                LoadStores();

                RdoSOandWithSO();
                DataTable dtsalesorder = fillSOSearhgrid();
                if (dtsalesorder.Rows.Count != 0)
                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvSerachGrid, dtsalesorder, "", "");


                    ViewState["Dtproduct"] = dtsalesorder;
                    //AllPageCode();

                    DataTable dtRefDetail = new DataTable();
                    for (int i = 0; i < dtsalesorder.Rows.Count; i++)
                    {


                        DataTable dtCopyRefDetail = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", dtsalesorder.Rows[i]["SoID"].ToString());
                        try
                        {
                            dtCopyRefDetail = new DataView(dtCopyRefDetail, "Field6='False' and ProductId='" + dtsalesorder.Rows[i]["Product_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }

                        if (dtCopyRefDetail.Rows.Count > 0)
                        {

                            dtCopyRefDetail = dtCopyRefDetail.DefaultView.ToTable(true, "ProductId", "ProductCategoryId", "CategoryName", "TaxName", "Tax_Per", "Tax_value", "TaxSelected", "Tax_Id", "SO_Id");


                            for (int j = 0; j < dtCopyRefDetail.Rows.Count; j++)
                            {
                                dtCopyRefDetail.Rows[j]["SO_Id"] = dtsalesorder.Rows[i]["SoID"].ToString();
                            }


                            dtRefDetail.Merge(dtCopyRefDetail);

                        }

                    }


                    ViewState["DtTax"] = dtRefDetail;

                }


                txtProductSerachValue.Focus();


            }
            if (RdoWithOutSo.Checked)
            {
                ViewState["dtSerial"] = null;
                ViewState["dtFinal"] = null;
                ViewState["Tempdt"] = null;
                ViewState["Dtproduct"] = null;
                ViewState["dtPo"] = null;

                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                Session["DtSearchProduct"] = null;
                ViewState["DtTax"] = null;
                ViewState["dtTaxHeader"] = null;
                LoadStores();
                RdoSOandWithSO();
                txtSearchProductName.Focus();
            }
        }
        else
        {
            DisplayMessage("Select Customer Name");
            ViewState["dtSerial"] = null;
            ViewState["dtFinal"] = null;
            ViewState["Tempdt"] = null;

            txtCustomer.Text = "";
            txtCustomer.Focus();
            RdoSo.Checked = false;
            RdoWithOutSo.Checked = false;
        }
        if (Session["Temp_Product_Tax_SINV"] != null)
            DeleteRowFromTempProductTaxTable("0");
    }
    public DataTable fillSOSearhgrid()
    {
        DataTable dtSalesOrder = null;
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesOrderApproval");

        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {

                dtSalesOrder = new DataView(objSOrderHeader.GetProductFromSalesOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtCustomer.Text.Trim().Split('/')[3].ToString(), Session["FinanceYearId"].ToString()), "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                dtSalesOrder = objSOrderHeader.GetProductFromSalesOrderForInvoice(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtCustomer.Text.Trim().Split('/')[3].ToString(), Session["FinanceYearId"].ToString());
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
            imgbtnsearch.Visible = true;
            ImgbtnRefresh.Visible = true;
            ImgbtnProductSave.Visible = false;
            txtSearchProductName.Visible = false;

            if (ddlProductSerach.Items.FindByText("Sales Order No") == null)
            {
                ddlProductSerach.Items.Insert(2, new ListItem("Sales Order No", "SalesOrderNo"));
            }
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
    #endregion
    #region Post
    protected void btnPost_Click(object sender, EventArgs e)
    {
        string InvoiceEditId = editid.Value;

        //Code for Approval
        DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "SalesInvoiceApproval");

        if (DtPara.Rows.Count > 0)
        {
            if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
            {
                if (InvoiceEditId != "")
                {
                    string st = GetStatus(Convert.ToInt32(InvoiceEditId.ToString()));
                    if (st == "Approved")
                    {
                        chkPost.Checked = true;
                        btnSInvSave_Click(null, null);
                    }
                    else
                    {
                        DisplayMessage("Cannot Post, Invoice not Approved");
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Cannot Post, Invoice not Approved");
                    return;
                }
            }
            else
            {
                chkPost.Checked = true;
                btnSInvSave_Click(null, null);
            }
        }

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
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Sales Invoice. on " + System.DateTime.Now.ToString();
        if (editid.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), editid.Value, "15");
    }

    #endregion
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        //AllPageCode();
        btnEdit_Command(sender, e);
        LoadStores();
    }
    #endregion
    #region InvoicePrint
    public void InvoicePrint(string InvoiceId)
    {
        if (Session["LocId"].ToString() == "8")
        {
            //utility/reportViewer.aspx?reportId=217&t=24175&l=&PageRef=SI&ProductId=
            string strCmd = string.Format("window.open('../Sales/SalesInvoicePrint.aspx?Id=" + InvoiceId.ToString() + "','window','width=1024, ');");
            //string strCmd = string.Format("window.open('../utility/reportViewer.aspx?reportId=217&t="+ InvoiceId.ToString() +"&l=&PageRef=SI&ProductId=','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
        else
        {
            string strCmd = string.Format("window.open('../Sales/SalesInvoicePrint.aspx?Id=" + InvoiceId.ToString() + "','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

        }
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
                    InvoicePrint(e.CommandArgument.ToString());

                }
                else
                {
                    DisplayMessage("Cannot Print, Invoice not Approved");
                    return;
                }
            }
            else
            {
                InvoicePrint(e.CommandArgument.ToString());
            }
        }

        ////here we check that is is click on allow deleievry voucher or not if yes than print the delivery voucher

        //if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Delivery Voucher allow").Rows[0]["ParameterValue"].ToString()))
        //{
        //  bool result=Convert.ToBoolean(ScriptManager.RegisterStartupScript(this, GetType(), "", "Confirm(Are u sure to print ?);", true););
        //    string confirmValue1 = Request.Form["confirm_value1"];

        //    if (confirmValue1 == "Yes")
        //    {
        //        DataTable dt = objdelVoucherHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        //        dt = new DataView(dt, "Field2=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        //        if (dt.Rows.Count > 0)
        //        {

        //            PrintDeliveryVoucher(dt.Rows[0]["Trans_Id"].ToString());
        //        }
        //    }
        //}


    }
    #endregion
    #region DeliveryPrint
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

    #endregion
    #region Expenses
    protected void GetData()
    {
        DataTable dtPay = new DataTable();
        dtPay = (DataTable)ViewState["PayementDt"];
        if (dtPay != null)
        {
            fillPaymentGrid(dtPay);
        }
        else
        {
            gvPayment.DataSource = null;
            gvPayment.DataBind();
        }

        DataTable dtExp = new DataTable();
        dtExp = (DataTable)ViewState["ExpdtSales"];
        if (dtExp != null)
        {
            fillExpGrid(dtExp);
        }
        else
        {
            GridExpenses.DataSource = null;
            GridExpenses.DataBind();
        }
    }
    public void fillExpGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

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
            if (Session["Expenses_Tax_Sales_Invoice"] != null)
            {
                DataTable Dt_Cal = Session["Expenses_Tax_Sales_Invoice"] as DataTable;
                if (Dt_Cal.Rows.Count > 0)
                {
                    Dt_Cal = new DataView(Dt_Cal, "Expenses_Id='" + Dt_Row["Expense_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (Dt_Cal.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Sub_Row in Dt_Cal.Rows)
                        {
                            Sum_Tax = Sum_Tax + Convert.ToDouble(Dt_Sub_Row["Tax_Percentage"].ToString());
                            Sum_Tax_Value = Sum_Tax_Value + Convert.ToDouble(Dt_Sub_Row["Tax_Value"].ToString());
                            Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["FCExpAmount"].ToString());
                        }
                    }
                    else
                    {
                        Sum_Tax = Sum_Tax + 0;
                        Sum_Tax_Value = Sum_Tax_Value + 0;
                        Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["FCExpAmount"].ToString());
                    }
                }
                else
                {
                    Sum_Tax = Sum_Tax + 0;
                    Sum_Tax_Value = Sum_Tax_Value + 0;
                    Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["FCExpAmount"].ToString());
                }
            }
            else
            {
                Sum_Tax = Sum_Tax + 0;
                Sum_Tax_Value = Sum_Tax_Value + 0;
                Sum_Line_Total = Sum_Tax_Value + Convert.ToDouble(Dt_Row["FCExpAmount"].ToString());
            }
            Dt_Row["F_Tax_Percent"] = Sum_Tax.ToString();
            Dt_Row["F_Tax_Value"] = Sum_Tax_Value.ToString();
            Dt_Row["Line_Total"] = Sum_Line_Total.ToString();
        }


        objPageCmn.FillData((object)GridExpenses, Dt_Expenses, "", "");

        ViewState["ExpdtSales"] = Dt_Expenses;
        if (Dt_Expenses.Rows.Count != 0)
        {
            double f = 0;
            double Toal_Tax_Value = 0;
            double Toal_Line_Toal = 0;
            foreach (GridViewRow Row in GridExpenses.Rows)
            {
                f += Convert.ToDouble(((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                Toal_Tax_Value += Convert.ToDouble(((Label)Row.FindControl("Lbl_Expenses_Tax_Value_GV")).Text.Trim());
                Toal_Line_Toal += Convert.ToDouble(((Label)Row.FindControl("Lbl_Line_Total_GV")).Text.Trim());
            }
            ((Label)GridExpenses.FooterRow.FindControl("txttotExp")).Text = SetDecimal(f.ToString());
            ((Label)GridExpenses.FooterRow.FindControl("Lbl_Total_Tax_Value_Footer")).Text = SetDecimal(Toal_Tax_Value.ToString());
            ((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text = SetDecimal(Toal_Line_Toal.ToString());
            txtTotalExpensesAmount.Text = SetDecimal(Toal_Line_Toal.ToString());


            if (txtGrandTotal.Text == "")
            {
                txtGrandTotal.Text = "0";
            }
            txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        }
        else
        {
            txtTotalExpensesAmount.Text = "0";
            txtNetAmountwithexpenses.Text = SetDecimal(txtGrandTotal.Text);
        }
        //AllPageCode();
    }
    public void fillExpenses()
    {
        DataTable dt = ObjShipExp.GetShipExpMaster(StrCompId.ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");

    }
    protected void IbtnAddExpenses_Click(object sender, ImageClickEventArgs e)
    {
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
            DisplayMessage("Enter Expense Account");
            txtExpensesAccount.Focus();
            return;
        }
        //

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





        DataTable dt = new DataTable();
        int i = 0;
        bool b = false;
        if (ViewState["ExpdtSales"] != null)
        {
            dt = (DataTable)ViewState["ExpdtSales"];
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
        }
        else
        {
            dt.Columns.Add("Expense_Id");
            dt.Columns.Add("Account_No");
            dt.Columns.Add("Exp_Charges");
            dt.Columns.Add("ExpCurrencyID");
            dt.Columns.Add("ExpExchangeRate");
            dt.Columns.Add("FCExpAmount");

            dt.Rows.Add();
        }

        dt.Rows[i]["Expense_Id"] = ddlExpense.SelectedValue.ToString();
        dt.Rows[i]["Account_No"] = GetAccountId(txtExpensesAccount.Text);
        dt.Rows[i]["Exp_Charges"] = txtExpCharges.Text.Trim();
        dt.Rows[i]["ExpCurrencyID"] = ddlExpCurrency.SelectedValue.ToString();
        dt.Rows[i]["ExpExchangeRate"] = SetDecimal(txtExchangeRate.Text.ToString());
        dt.Rows[i]["FCExpAmount"] = txtFCExpAmount.Text.ToString();

        txtExpCharges.Text = "0";
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";

        // txtExpExchangeRate.Text = "0";
        txtFCExpAmount.Text = "0";
        ViewState["ExpdtSales"] = dt;
        fillExpGrid(dt);

        DataTable dtPay = new DataTable();
        dtPay = (DataTable)ViewState["PayementDt"];
        if (dt != null)
        {
            fillPaymentGrid(dtPay);
        }
    }
    protected void IbtnDeleteExp_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["ExpdtSales"]), "Expense_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        fillExpGrid(dt);

        ViewState["ExpdtSales"] = dt;

        bool IsTax = IsApplyTax();
        if (IsTax == true)
        {
            if (Dt_Final_Save_Tax != null)
            {
                Dt_Final_Save_Tax = new DataView(Dt_Final_Save_Tax, "Expenses_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Session["Expenses_Tax_Sales_Invoice"] = Dt_Final_Save_Tax;
            }
        }

        GetData();
    }
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
            }
        }
        GetData();
    }
    public string GetExpName(string ExpId)
    {
        return (ObjShipExp.GetShipExpMasterById(StrCompId, ExpId)).Rows[0]["Exp_Name"].ToString();
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
        return CurrencyName;
    }

    protected void txtFCExpAmount_OnTextChanged(object sender, EventArgs e)
    {

        if (txtExpExchangeRate.Text == "")
        {
            txtExpExchangeRate.Text = "0";
        }

        if (txtFCExpAmount.Text == "")
        {
            txtFCExpAmount.Text = "0";
        }


        try
        {
            txtExpCharges.Text = (Convert.ToDouble(txtFCExpAmount.Text.Trim()) * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
            txtExpCharges.Text = SetDecimal(txtExpCharges.Text);
        }
        catch
        {
            txtExpCharges.Text = SetDecimal("0");
        }
    }
    public void FillCurrency(DropDownList ddlCurrency)
    {
        try
        {
            DataTable dt = ObjCurrencyMaster.GetCurrencyMaster();

            dt = new DataView(dt, "", "Currency_Name", DataViewRowState.CurrentRows).ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlExpCurrency, dt, "Currency_Name", "Currency_Id");

        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;

            ddlExpCurrency.Items.Insert(0, "--Select--");
            ddlExpCurrency.SelectedIndex = 0;

        }


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
                    return;
                }
                if (dtAccount != null && dtAccount.Rows.Count > 0)
                {
                    if (dtAccount.Rows[0]["Trans_Id"].ToString() == "7")
                    {
                        DisplayMessage("Please donot select " + txtExpensesAccount.Text.Split('/')[0].ToString() + "");
                        txtExpensesAccount.Focus();
                        txtExpensesAccount.Text = "";
                        return;
                    }
                }

            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                txtExpensesAccount.Text = "";
                DisplayMessage("No Account Found");
                txtExpensesAccount.Focus();
                return;
            }

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
    #endregion
    #region Payment
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
    protected void btnPaymentSave_Click(object sender, EventArgs e)
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

        //--------------------credit note validation -----------------------
        string strCCNId = "0";
        if (ddlTabPaymentMode.SelectedItem.Text.ToUpper() == "CREDIT NOTE")
        {
            if (txtCreditNote.Text.Trim() == string.Empty)
            {
                DisplayMessage("Select Customer Credit Note No");
                txtCreditNote.Focus();
                return;
            }
            else
            {
                DataTable _dt = getUnAdjustedCreditNote();
                if (_dt.Rows.Count > 0)
                {
                    _dt = new DataView(_dt, "voucher_no='" + txtCreditNote.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (_dt.Rows.Count > 0)
                    {
                        if (ddlCurrency.SelectedValue != _dt.Rows[0]["currency_id"].ToString())
                        {
                            txtCreditNote.Text = "";
                            txtCreditNote.Focus();
                            DisplayMessage("Currency Mismatch please select another credit note");
                            return;
                        }
                        else if (Convert.ToDouble(txtPayAmount.Text) > Convert.ToDouble(_dt.Rows[0]["Actual_balance_amount"].ToString()))
                        {
                            txtCreditNote.Text = "";
                            txtCreditNote.Focus();
                            DisplayMessage("Customer having only " + SetDecimal(_dt.Rows[0]["Actual_balance_amount"].ToString()) + " Amount in selected credit Note, Please select another one.");
                            return;
                        }
                        else
                        {
                            strCCNId = _dt.Rows[0]["Voucher_id"].ToString();
                        }
                    }


                }
                _dt.Dispose();

            }
        }
        //--------------------------------------------------------

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
            dt.Columns.Add("PaymentModeId", typeof(Int32));
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
            dt.Columns.Add("Pay_Charges", typeof(decimal));
            dt.Columns.Add("PayCurrencyID");
            dt.Columns.Add("PayExchangeRate");
            dt.Columns.Add("FCPayAmount", typeof(decimal));
            dt.Columns.Add("field3");

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

        dt.Rows[dt.Rows.Count - 1]["PaymentModeId"] = ddlTabPaymentMode.SelectedValue;
        dt.Rows[dt.Rows.Count - 1]["PaymentName"] = ddlTabPaymentMode.SelectedItem.ToString();
        dt.Rows[dt.Rows.Count - 1]["FCPayAmount"] = txtPayAmount.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["Pay_Charges"] = decimal.Parse(txtLocalAmount.Text.Trim());
        dt.Rows[dt.Rows.Count - 1]["AccountNo"] = GetAccountId(txtPayAccountNo.Text);
        dt.Rows[dt.Rows.Count - 1]["CardNo"] = txtPayCardNo.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["CardName"] = txtPayCardName.Text.Trim();
        dt.Rows[dt.Rows.Count - 1]["PayExchangeRate"] = txtPaymentExchangerate.Text;
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
        dt.Rows[dt.Rows.Count - 1]["field3"] = strCCNId;


        ViewState["PayementDt"] = dt;


        DataTable dtExp = new DataTable();
        dtExp = (DataTable)ViewState["Expdt"];
        if (dtExp != null)
        {
            fillExpGrid(dtExp);
        }
        btnPaymentReset_Click(null, null);
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
        fillPaymentGrid(dt);

        //here we change balance amount when we paid against the invoice amount

    }
    protected void btnPaymentReset_Click(object sender, EventArgs e)
    {
        txtPayAccountNo.Text = "";
        txtPayAmount.Text = "";
        txtLocalAmount.Text = "";
        txtPayCardName.Text = "";
        txtPayChequeNo.Text = "";
        txtPayCardNo.Text = "";
        txtPayChequeDate.Text = "";
        txtCreditNote.Text = "";
        fillBank();

        trcheque.Visible = false;
        trcard.Visible = false;
        lblPayBank.Visible = false;
        //lblpaybankcolon.Visible = false;
        ddlPayBank.Visible = false;
        txtCreditNote.Enabled = false;

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

                ((Label)gvrow.FindControl("lblAmount")).Text = SetDecimal(((Label)gvrow.FindControl("lblAmount")).Text);
                ((Label)gvrow.FindControl("lblgvExp_Charges")).Text = SetDecimal(((Label)gvrow.FindControl("lblgvExp_Charges")).Text);
                f += Convert.ToDouble(((Label)gvrow.FindControl("lblAmount")).Text);
                Label lblPaymentMode = (Label)gvrow.FindControl("lblgvPaymentMode");
                Label lblCreditNoteId = (Label)gvrow.FindControl("lblCreditNoteId");
                if (lblPaymentMode.Text.Trim().ToUpper() == "CREDIT NOTE" && lblCreditNoteId.Text != string.Empty)
                {
                    string sql = "select voucher_no from ac_voucher_header where trans_id=" + lblCreditNoteId.Text;
                    lblPaymentMode.Text = lblPaymentMode.Text + "(" + objDa.get_SingleValue(sql) + ")";
                }
            }
            catch
            {
                ((Label)gvrow.FindControl("lblAmount")).Text = "0";
                f += 0;
            }
        }

        try
        {
            ((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text = SetDecimal(f.ToString());
        }
        catch
        {

        }

        //here we showing balance amount
        GetBalanceAmount();
        //if (txtNetAmountwithexpenses.Text != "")
        //{
        //    if (Convert.ToDouble(txtNetAmountwithexpenses.Text) > 0)
        //    {
        //        try
        //        {
        //            txtPayAmount.Text = SetDecimal((Convert.ToDouble(txtNetAmountwithexpenses.Text) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
        //        }
        //        catch
        //        {

        //            txtPayAmount.Text = txtNetAmountwithexpenses.Text;
        //        }
        //    }
        //}


    }


    protected void txtPayAmount_OnTextChanged(object sender, EventArgs e)
    {

        Double ForeignAmt = 0;
        Double Exchangerate = 0;

        Double.TryParse(txtPayAmount.Text, out ForeignAmt);
        Double.TryParse(txtPaymentExchangerate.Text, out Exchangerate);

        txtLocalAmount.Text = SetDecimal((ForeignAmt * Exchangerate).ToString());

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
        txtCreditNote.Enabled = ddlTabPaymentMode.SelectedItem.Text.ToUpper() == "CREDIT NOTE" ? true : false;
        if (ddlTabPaymentMode.SelectedValue == "--Select--")
        {
            txtPayAccountNo.Text = "";
        }

        else if (ddlTabPaymentMode.SelectedValue != "--Select--")
        {
            strSiCurrencyId = int.Parse(ddlCurrency.SelectedValue);
            if (ViewState["PayementDt"] != null)
            {
                DataTable dt = (DataTable)ViewState["PayementDt"];
                dt = new DataView(dt, "PaymentModeId='" + ddlTabPaymentMode.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
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
                                txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                            }
                        }
                        else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                        {
                            strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                            DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                            }
                        }
                    }
                }
                else
                {
                    //for Account Fill
                    DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
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
                                txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                            }
                        }
                        else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                        {
                            strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                            DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                            if (dtAcc.Rows.Count > 0)
                            {
                                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                                txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                            }
                        }
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
                DataTable dtPay = ObjPaymentMaster.GetPaymentModeMasterById(StrCompId, ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
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
                            txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                        }
                    }
                    else if (dtPay.Rows[0]["Field1"].ToString() == "Credit")
                    {
                        strAccountId = Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, HttpContext.Current.Session["DBConnection"].ToString());
                        DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                        if (dtAcc.Rows.Count > 0)
                        {
                            string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                            txtPayAccountNo.Text = strAccountName + "/" + strAccountId;
                        }
                    }
                }
            }
        }

        //here we showing related field according the select payment mode

        //when payment mode is cash then we showing accounts no only 
        if (ddlTabPaymentMode.SelectedIndex != 0)
        {
            if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Cash")
            {
                pnlpaybank.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Credit")
            {
                pnlpaybank.Visible = true;
                lblPayBank.Visible = true;
                //lblpaybankcolon.Visible = true;
                ddlPayBank.Visible = true;
                trcheque.Visible = true;
            }
            else if (ObjPaymentMaster.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlTabPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString() == "Card")
            {
                pnlpaybank.Visible = true;
                trcard.Visible = true;
            }
        }

        GetBalanceAmount();
        txtPayAmount_OnTextChanged(null, null);
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
    public void GetBalanceAmount()
    {
        if (txtNetAmountwithexpenses.Text != "")
        {
            if (Convert.ToDouble(txtNetAmountwithexpenses.Text) > 0)
            {
                if (gvPayment.Rows.Count > 0)
                {


                    txtPayAmount.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(SetDecimal(txtNetAmountwithexpenses.Text)) - Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                    //if (gvadvancepayment.Rows.Count > 0)
                    //{
                    //    if ((Convert.ToDouble(txtNetAmountwithexpenses.Text) - (Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) + Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text))) > 0)
                    //    {
                    //        txtPayAmount.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(txtNetAmountwithexpenses.Text) - (Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) + Convert.ToDouble(((Label)gvPayment.FooterRow.FindControl("txttotAmount")).Text))).ToString());
                    //    }
                    //    else
                    //    {
                    //        txtPayAmount.Text = "0";

                    //    }
                    //}


                }
                else
                {
                    //if (gvadvancepayment.Rows.Count > 0)
                    //{
                    //    if ((Convert.ToDouble(txtNetAmountwithexpenses.Text) - Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text)) > 0)
                    //    {
                    //        txtPayAmount.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(txtNetAmountwithexpenses.Text) - Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text)).ToString());
                    //    }
                    //    else
                    //    {
                    //        txtPayAmount.Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text) - Convert.ToDouble(txtNetAmountwithexpenses.Text)).ToString());

                    //    }
                    //}
                    //else
                    //{
                    txtPayAmount.Text = SetDecimal(txtNetAmountwithexpenses.Text);
                    //}
                }
            }
        }
        txtPayAmount_OnTextChanged(null, null);
    }
    #endregion
    protected void btnGVPost_Click(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        btnPost_Click(null, null);
        btnUnPost_Click(null, null);

    }
    #region Address

    protected void txtInvoiceTo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceTo.Text != "")
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

            //txtShipCustomerName.Text.Trim().Split('/')[0].ToString());
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
    protected void txtShipTo_TextChanged(object sender, EventArgs e)
    {
        if (txtShipCustomerName.Text != "")
        {
            string[] ShipName = txtShipCustomerName.Text.Split('/');
            DataTable DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {
                DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[3].ToString().Trim());


                dtAddress = new DataView(dtAddress, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();


                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtShipCustomerName.Text.Split('/')[0].ToString());
                    if (dtAM.Rows.Count > 0)
                    {
                        string ShipToAddress = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                        ShipAddress.Text = ShipToAddress;
                    }
                    if (txtShipingAddress.Text == "")
                    {
                        //string name = string.Concat();

                        txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                        txtShipingAddress_TextChanged(null, null);
                    }
                }
                else
                {
                    DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtShipCustomerName.Text.Trim().Split('/')[0].ToString());
                    if (dtAM.Rows.Count > 0)
                    {
                        string ShipToAddress = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                        ShipAddress.Text = ShipToAddress;
                    }

                }
            }
            else
            {
                DisplayMessage("Select Ship to in Suggestion Only");
                txtShipCustomerName.Text = "";
                txtShipCustomerName.Focus();
                return;
            }

        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCon = objcontact.GetAllContactAsPerFilterText(prefixText);

        //DataTable dtContact = objcontact.GetContactTrueAllData();


        //DataTable dtMain = new DataTable();
        //dtMain = dtContact.Copy();


        //string filtertext = "Filtertext like '%" + prefixText + "%'";
        //DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

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

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
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
        string strNewSNo = string.Empty;
        DataTable dt = SavedGridRecordindatatble();

        Session["DtSearchProduct"] = dt;
        GetChildGridRecordInViewState();

        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=SIN&&CustomerId=" + txtCustomer.Text.Split('/')[3].ToString() + "&&CurId=" + ddlCurrency.SelectedValue + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
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
                objPageCmn.FillData((object)gvProduct, dt, "", "");
                ddlTransType.Enabled = false;
                ViewState["Dtproduct"] = dt;
                if (gvProduct.Rows.Count > 0)
                {
                    ddlTransType.Enabled = false;
                }
                else
                {
                    ddlTransType.Enabled = true;
                }
            }

            //objPageCmn.FillData((object)gvProduct, dt, "", "");
            //ViewState["Dtproduct"] = dt;

            GridCalculation();
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
        if (rbtnFormView.Checked == true)
        {
            btnAddNewProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;

        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
        }

    }
    #endregion
    #region Post
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();
    }
    #endregion
    #region Currency

    protected void ddlCurrency_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        txtPaymentExchangerate.Enabled = false;
        string OldExchangeRate = string.Empty;
        string NewExchangeRate = string.Empty;
        OldExchangeRate = "0";
        NewExchangeRate = "0";

        OldExchangeRate = txtExchangeRate.Text;
        if (ddlCurrency.SelectedIndex != 0)
        {
            strSiCurrencyId = int.Parse(ddlCurrency.SelectedValue);
            if (ddlCurrency.SelectedValue == SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()))
            {
                txtPaymentExchangerate.Enabled = false;
            }
            else
            {
                txtPaymentExchangerate.Enabled = true;
            }
            strCurrencyId = ddlCurrency.SelectedValue;
            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            //if (sender != null)
            //{
            txtExchangeRate.Text = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()), Session["DBConnection"].ToString());
            //}
            txtExpExchangeRate.Text = txtExchangeRate.Text;
            txtPaymentExchangerate.Text = txtExchangeRate.Text;
            NewExchangeRate = txtExchangeRate.Text;
            GridCalculation_CurrencyConversion(OldExchangeRate, NewExchangeRate);
        }

        ddlExpCurrency.SelectedValue = ddlCurrency.SelectedValue;
        setSymbol();
    }


    public void GridCalculation_CurrencyConversion(string OldExchangeRate, string NewExchangeRate)
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {

            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            DropDownList ddlUnitName = (DropDownList)gvRow.FindControl("ddlUnitName");
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
            TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            Label lblgvSoldQuantity = (Label)gvRow.FindControl("lblgvSoldQuantity");
            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
            TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
            TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
            Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");


            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }
            if (txtgvSalesQuantity.Text == "")
            {
                txtgvSalesQuantity.Text = "0";
            }


            try
            {


                lblgvUnitPrice.Text = SetDecimal(((Convert.ToDouble(lblgvUnitPrice.Text) / Convert.ToDouble(OldExchangeRate)) * Convert.ToDouble(NewExchangeRate)).ToString()).ToString();


            }
            catch
            {

            }



            string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, strCurrencyId, false, Session["DBConnection"].ToString());

            lblgvQuantityPrice.Text = strcalc[0].ToString();
            txtgvTotal.Text = strcalc[5].ToString();
            lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
            lblgvFreeQuantity.Text = SetDecimal(lblgvFreeQuantity.Text);
            lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
            lblgvSoldQuantity.Text = SetDecimal(lblgvSoldQuantity.Text);
            lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
            txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
            txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
            txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
            txtgvDiscountV.Text = strcalc[2].ToString();

            txtgvTaxV.Text = Convert_Into_DF(strcalc[4].ToString());

        }


        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }

        try
        {
            txtTotalExpensesAmount.Text = SetDecimal((Convert.ToDouble(((Label)GridExpenses.FooterRow.FindControl("Lbl_Line_Total_Footer")).Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString());
        }
        catch
        {
            txtTotalExpensesAmount.Text = "0";
        }


        HeadearCalculateGrid();
        //AllPageCode();
    }

    #endregion
    #region AddCustomer

    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../Sales/AddContact.aspx?Page=SINV','window','width=1024');");
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


            string ProductID = ((HiddenField)e.Row.FindControl("hdngvProductId")).Value;
            string So_Id = ((Label)e.Row.FindControl("lblSOId")).Text;
            DropDownList ddlUnit = ((DropDownList)e.Row.FindControl("ddlUnitName"));
            GridView gvchildGrid = (GridView)e.Row.FindControl("gvchildGrid");
            //modify by jitendra upadhyay for delivery voucher concept

            //modify on 30/12/2015

            //code start

            if (So_Id == "" || So_Id == "0")
            {


                if (objProductM.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
                {
                    //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = false;

                    ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = true;
                }
                else
                {
                    //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = true;

                    ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = false;
                }
            }
            else
            {
                if (Convert.ToBoolean(objSOrderHeader.GetSOHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, So_Id).Rows[0]["IsdeliveryVoucher"].ToString()))
                {
                    //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = true;

                    ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = false;
                }
                else
                {
                    if (objProductM.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
                    {
                        //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = false;

                        ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = true;
                    }
                    else
                    {
                        //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = true;

                        ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = false;
                    }
                }
            }

            //code end

            if (objProTax.GetRecord_ByProductId(ProductID).Rows.Count > 0)
            {
                ((ImageButton)e.Row.FindControl("imgBtnaddtax")).Visible = true;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvchildGrid, objProTax.GetRecord_ByProductId(ProductID), "", "");

                if (So_Id.Trim() == "")
                {
                    So_Id = "0";
                }
                if (ViewState["DtTax"] != null)
                {
                    try
                    {
                        DataTable dt = new DataView((DataTable)ViewState["DtTax"], "ProductId='" + ProductID + "' and SO_Id=" + So_Id + "", "", DataViewRowState.CurrentRows).ToTable();
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

            if (RdoSo.Checked)
            {
                try
                {
                    ddlUnit.SelectedValue = new DataView((DataTable)ViewState["dtPo"], "Product_Id=" + ProductID + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitId"].ToString();
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
                    ddlUnit.SelectedValue = new DataView((DataTable)ViewState["Dtproduct"], "Product_Id=" + ProductID + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitId"].ToString();
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
                dr[0] = ((HiddenField)gvrow.FindControl("hdngvProductId")).Value;
                dr[1] = ((HiddenField)gvChildRow.FindControl("hdntaxId")).Value;
                dr[2] = ((HiddenField)gvChildRow.FindControl("hdnCategoryId")).Value;
                dr[3] = ((Label)gvChildRow.FindControl("lblgvcategoryName")).Text;
                dr[4] = ((Label)gvChildRow.FindControl("lblgvtaxName")).Text;
                dr[5] = ((TextBox)gvChildRow.FindControl("txttaxPerchild")).Text;
                dr[6] = ((TextBox)gvChildRow.FindControl("txttaxValuechild")).Text;
                dr[7] = ((CheckBox)gvChildRow.FindControl("chkselecttax")).Checked;
                if (((Label)gvrow.FindControl("lblSOId")).Text == "")
                {
                    ((Label)gvrow.FindControl("lblSOId")).Text = "0";
                }
                dr[8] = ((Label)gvrow.FindControl("lblSOId")).Text;

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

        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {

            try
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = SetDecimal((priceafterdiscount * Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100).ToString());
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


        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxV")).Text = Convert_Into_DF(TotalTax.ToString());

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxP")).Text = SetDecimal(Percentage.ToString());


        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text != "")
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text)).ToString();
        }
        else
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = "0";
        }



        HeadearCalculateGrid();
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

        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {

            try
            {
                ((TextBox)gvrow.FindControl("txttaxValuechild")).Text = SetDecimal((priceafterdiscount * Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxPerchild")).Text) / 100).ToString());
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


        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxV")).Text = Convert_Into_DF(TotalTax.ToString());

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxP")).Text = SetDecimal(Percentage.ToString());


        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text != "")
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text)).ToString();
        }
        else
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = "0";
        }


        HeadearCalculateGrid();
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

        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text != "")
        {
            priceafterdiscount = Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("lblgvUnitPrice")).Text);
        }
        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text != "")
        {

            priceafterdiscount = priceafterdiscount - Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvDiscountV")).Text);


        }
        if (((CheckBox)gvrow.FindControl("chkselecttax")).Checked)
        {


            try
            {

                ((TextBox)gvrow.FindControl("txttaxPerchild")).Text = SetDecimal((Convert.ToDouble(((TextBox)gvrow.FindControl("txttaxValuechild")).Text) * 100 / priceafterdiscount).ToString());

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


        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxV")).Text = Convert_Into_DF(TotalTax.ToString());

        double Percentage = TotalTax * 100 / priceafterdiscount;

        ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTaxP")).Text = SetDecimal(Percentage.ToString());


        if (((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text != "")
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = ((priceafterdiscount + TotalTax) * Convert.ToDouble(((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvSalesQuantity")).Text)).ToString();
        }
        else
        {
            ((TextBox)gvProduct.Rows[Gv1Row.RowIndex].FindControl("txtgvTotal")).Text = "0";
        }

        HeadearCalculateGrid();

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

            if (txtPriceafterdiscountheader.Text != "" && txtPriceafterdiscountheader.Text != "0")
            {

                ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = SetDecimal((Convert.ToDouble(txtPriceafterdiscountheader.Text) * (Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text) / 100)).ToString());
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

            if (txtPriceafterdiscountheader.Text != "" && txtPriceafterdiscountheader.Text != "0")
            {

                ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = SetDecimal(((Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text) * 100) / Convert.ToDouble(txtPriceafterdiscountheader.Text)).ToString());
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
            //if (((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text != "")
            //{
            //    if (txtPriceafterdiscountheader.Text != "" && txtPriceafterdiscountheader.Text != "0")
            //    {
            //        ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = SetDecimal((Convert.ToDouble(txtPriceafterdiscountheader.Text) * (Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text) / 100)).ToString());
            //    }
            //    else
            //    {
            //        ((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text = "0";
            //    }
            //}
            //if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text != "")
            //{
            //    if (txtPriceafterdiscountheader.Text != "" && txtPriceafterdiscountheader.Text != "0")
            //    {
            //        ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = SetDecimal(((Convert.ToDouble(((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text) * 100) / Convert.ToDouble(txtPriceafterdiscountheader.Text)).ToString());
            //    }
            //    else
            //    {
            //        ((TextBox)gridView.FooterRow.FindControl("txtTaxperFooter")).Text = "0";
            //    }
            //}
            TextBox txtTaxName = (TextBox)gridView.FooterRow.FindControl("txtTaxFooter");
            TextBox txtTaxValue = (TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter");
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
        gridView.EditIndex = -1;
        LoadStores();
        TotalheaderTax();
    }

    public void CalculationchangeIntaxGridview()
    {
        if (txtPriceafterdiscountheader.Text != "")
        {
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                try
                {
                    if (((Label)gridView.Rows[i].FindControl("lblTaxper")).Text != "")
                    {
                        if (txtPriceafterdiscountheader.Text != "" && txtPriceafterdiscountheader.Text != "0")
                        {
                            ((Label)gridView.Rows[i].FindControl("lblTaxValue")).Text = SetDecimal((Convert.ToDouble(txtPriceafterdiscountheader.Text) * (Convert.ToDouble(((Label)gridView.Rows[i].FindControl("lblTaxper")).Text) / 100)).ToString());
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
        txtTaxP.Text = SetDecimal(netTaxPer.ToString());
        txtTaxV.Text = Convert_Into_DF(nettaxval.ToString());

        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, strCurrencyId, true, Session["DBConnection"].ToString());
        txtTaxV.Text = Convert_Into_DF(str[4].ToString());
        txtGrandTotal.Text = str[5].ToString();

        txtGrandTotal.Text = SystemParameter.GetScaleAmount(SetDecimal(txtGrandTotal.Text), "0");
        if (txtTotalExpensesAmount.Text == "")
        {
            txtTotalExpensesAmount.Text = "0";
        }
        txtNetAmountwithexpenses.Text = SetDecimal((Convert.ToDouble(txtTotalExpensesAmount.Text) + Convert.ToDouble(txtGrandTotal.Text)).ToString());
        TaxDiscountFromHeader(false);
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
    #region stockable with serial number
    protected void lnkAddSO_Click(object sender, EventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";
        Session["RdoType"] = "SerialNo";
        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["SOrderId"] = ((Label)Row.FindControl("lblSOId")).Text;
        HiddenField HdnProductId = (HiddenField)Row.FindControl("hdngvProductId");
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblgvProductCode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblgvProductName")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtSerialNo.Text = "";
        ViewState["PID"] = HdnProductId.Value;
        if (ViewState["dtFinal"] == null)
        {
        }
        else
        {
            DataTable dt = new DataTable();

            dt = (DataTable)ViewState["dtFinal"];
            //dt = new DataView(dt, "Product_Id='" + ViewState["PID"].ToString() + "' and SOrderNo='" + ViewState["SOrderId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt = new DataView(dt, "Product_Id='" + ViewState["PID"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            ViewState["dtSerial"] = dt;
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        }

        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
    }

    protected void btnNotFound_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["dtSerialIssue"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", "0", ViewState["PID"].ToString(), "1", "I", "1", "0", "0", DateTime.Now.ToString(), dt.Rows[i][1].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
            }
            ViewState["dtSerialIssue"] = null;
            btnNotFound.Visible = false;
        }
        catch
        {

        }
    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {

        DataTable dtSerialIssue = new DataTable();
        dtSerialIssue.Columns.Add(new DataColumn("ProductId"));
        dtSerialIssue.Columns.Add(new DataColumn("Serial"));
        dtSerialIssue.Columns.Add(new DataColumn("Status"));


        string TransId = string.Empty;
        if (editid.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = editid.Value;
        }
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        //DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductId(ViewState["PID"].ToString());

        if (txtSerialNo.Text.Trim() != "")
        {
            //  ViewState["dtSerial"] = null;

            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;

            if (ViewState["dtSerial"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Product_Id");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("SOrderNo");
                dt.Columns.Add("BarcodeNo");
                dt.Columns.Add("BatchNo");
                dt.Columns.Add("LotNo");
                dt.Columns.Add("ExpiryDate");
                dt.Columns.Add("ManufacturerDate");

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        string[] result = isSerialNumberValid(dt, txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {

                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = ViewState["SOrderId"].ToString();
                            //for batch number
                            dr[4] = result[4].ToString();

                            dr[5] = "0";
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();
                            counter++;

                        }
                        else if (result[0].ToString() == "ALREADY OUT")
                        {

                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "ALREADY OUT";

                            dtSerialIssue.Rows.Add(rowSerialIssue);

                            //Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString()
                            //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", "0", ViewState["PID"].ToString(), "1", "I", "1", "0", "0", DateTime.Now.ToString(), txt[i].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", "0", ViewState["PID"].ToString(), "1", "I", "1", "0", "0", DateTime.Now.ToString(), txt[i].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());

                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "NOT EXISTS";

                            dtSerialIssue.Rows.Add(rowSerialIssue);


                            serialNoExists += txt[i].ToString() + ",";
                        }
                    }
                }

            }
            else
            {
                dt = (DataTable)ViewState["dtSerial"];
                dtTemp = dt.Copy();

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        string[] result = isSerialNumberValid(dt, txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = ViewState["SOrderId"].ToString();
                            //for batch number
                            dr[4] = result[4].ToString();
                            dr[5] = "0";
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();
                            counter++;
                        }
                        else if (result[0].ToString() == "ALREADY OUT")
                        {
                            //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", "0", ViewState["PID"].ToString(), "1", "I", "1", "0", "0", DateTime.Now.ToString(), txt[i].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "ALREADY OUT";

                            dtSerialIssue.Rows.Add(rowSerialIssue);
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            // ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", "0", ViewState["PID"].ToString(), "1", "I", "1", "0", "0", DateTime.Now.ToString(), txt[i].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());

                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "NOT EXISTS";

                            dtSerialIssue.Rows.Add(rowSerialIssue);
                            serialNoExists += txt[i].ToString() + ",";


                        }
                    }
                }
            }
        }
        else
        {
            dt = (DataTable)ViewState["dtSerial"];
        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || serialNoExists != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Out from the stock=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {
                if (Message == "")
                {
                    Message += "Following serial number not exists in stock=" + serialNoExists;
                }
                else
                {
                    Message += Environment.NewLine + "Following serial number not exists in stock=" + serialNoExists;
                }
            }
            DisplayMessage(Message);
        }
        //here we check that sales quantity should be less than system quantity
        //this validation is add by jitendra upadhyay on 22-05-2015
        //code start
        if (((Label)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvSystemQuantity")).Text == "")
        {
            ((Label)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvSystemQuantity")).Text = "0";
        }
        if (Convert.ToDouble(((Label)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvSystemQuantity")).Text) < Convert.ToDouble(dt.Rows.Count.ToString()))
        {
            //DisplayMessage("Invoice Quantity Should be less than or equal to the system quantity");
            //return;
        }
        //code end
        ViewState["dtSerial"] = dt;
        if (ViewState["dtFinal"] == null)
        {
            if (ViewState["dtSerial"] != null)
            {
                ViewState["dtFinal"] = (DataTable)ViewState["dtSerial"];
            }
        }
        else
        {
            dt = new DataTable();
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];
            if (RdoWithOutSo.Checked == true)
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (RdoSo.Checked == true)
            {
                DataTable DtTemp = Dtfinal.Copy();
                try
                {
                    DtTemp = new DataView(DtTemp, "Product_Id='" + ViewState["PID"].ToString() + "' and SOrderNo='" + ViewState["SOrderId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtTemp.Rows.Count > 0)
                {
                    string s = "SOrderNo Not In('" + ViewState["SOrderId"].ToString() + "') or Product_Id Not In('" + ViewState["PID"].ToString() + "')";
                    Dtfinal = new DataView(Dtfinal, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
                }

            }
            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];
            }
            Dtfinal.Merge(dt);
            ViewState["dtFinal"] = Dtfinal;
        }
        if (ViewState["dtSerial"] != null)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, (DataTable)ViewState["dtSerial"], "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }
            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (ViewState["dtSerial"] != null)
                {
                    txtgvSalesQuantity.Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
                }
                else
                {
                    txtgvSalesQuantity.Text = "0";
                }

                if (RdoWithOutSo.Checked)
                {
                    lblgvOrderqty.Text = txtgvSalesQuantity.Text;
                }

                txtgvSalesQuantity_TextChanged(((TextBox)gvRow.FindControl("txtgvSalesQuantity")), null);
                break;
            }
        }
        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();

        gvSerialIssue.DataSource = null;
        gvSerialIssue.DataBind();
        ViewState["dtSerialIssue"] = dtSerialIssue;

        if (dtSerialIssue.Rows.Count > 0)
        {
            btnNotFound.Visible = true;
        }

        gvSerialIssue.DataSource = dtSerialIssue;
        gvSerialIssue.DataBind();
    }



    public static string[] isSerialNumberValid(DataTable dtserial, string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        string[] Result = new string[5];


        int counter = 0;

        if (dtserial != null)
        {
            if (new DataView(dtserial, "SerialNo='" + serialNumber.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                counter = 1;
            }
        }

        if (counter == 0)
        {
            dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);
            // Date  : 29042021

            dtserial = new DataView(dtserial, "Location_Id ='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

            if (dtserial.Rows.Count > 0)
            {

                if (dtserial.Rows[0]["InOut"].ToString() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                else if (dtserial.Rows[0]["InOut"].ToString() == "I")
                {
                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
                //else
                //{
                //    DataTable dtCopy = dtserial.Copy();

                //    dtserial = new DataView(dtserial, "InOut='O' and (TransType='SI' or TransType='DV')  and TransTypeId<>" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                //    if (dtserial.Rows.Count > 0)
                //    {

                //        Result[0] = "ALREADY OUT";

                //    }

                //    else
                //    {
                //        dtserial = dtCopy.Copy();
                //        dtserial = new DataView(dtserial, "InOut='I'", "", DataViewRowState.CurrentRows).ToTable();
                //        if (dtserial.Rows.Count > 0)
                //        {

                //            Result[0] = "VALID";
                //            Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                //            Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                //            Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                //            Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                //        }
                //    }
                //}

            }
            else
            {
                Result[0] = "NOT EXISTS";
            }
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    protected void btnResetSerial_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
        txtSerialNo.Text = "";
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtCount.Text = "0";
        txtselectedSerialNumber.Text = "0";
        if (ViewState["dtFinal"] != null)
        {
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];
            if (RdoWithOutSo.Checked == true)
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (RdoSo.Checked == true)
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "' and  SOrderNo<>'" + ViewState["SOrderId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                int SOId = Convert.ToInt32(ViewState["SOrderId"].ToString());
            }
            ViewState["dtFinal"] = Dtfinal;
        }

        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");

            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }

            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (ViewState["dtSerial"] != null)
                {
                    txtgvSalesQuantity.Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
                }
                else
                {
                    txtgvSalesQuantity.Text = "0";
                }

                if (RdoWithOutSo.Checked)
                {
                    lblgvOrderqty.Text = txtgvSalesQuantity.Text;
                }

                txtgvSalesQuantity_TextChanged(((TextBox)gvRow.FindControl("txtgvSalesQuantity")), null);

                break;
            }
        }
        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {

        ViewState["dtSerial"] = null;
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
    }
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
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
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
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
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
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();
    }
    protected void gvSerialNumber_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtSerial"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
    protected void btnLoadTempSerial_Click(object sender, EventArgs e)
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
        DataTable dtstock = new DataTable();
        if (txtOrderId.Text.Length > 0)
        {
            dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", hdnOrderId.Value, ddlMerchant.SelectedValue.ToString());
            if (dtstock.Rows.Count == 0)
            {
                DataTable dtShipment = ObjStockBatchMaster.GetTempSerial_Shipment(TransId.ToString());
                if (dtShipment.Rows.Count > 0)
                {
                    dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", dtShipment.Rows[0]["Shipment_Id"].ToString(), ddlMerchant.SelectedValue.ToString());
                    if (dtstock.Rows.Count == 0)
                    {
                        dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", txtSInvNo.Text, ddlMerchant.SelectedValue.ToString());
                    }

                }
            }
        }
        else
        {
            DataTable dtShipment = ObjStockBatchMaster.GetTempSerial_Shipment(TransId.ToString());
            if (dtShipment.Rows.Count > 0)
            {
                dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", dtShipment.Rows[0]["Shipment_Id"].ToString(), ddlMerchant.SelectedValue.ToString());
                if (dtstock.Rows.Count == 0)
                {
                    dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", txtSInvNo.Text, ddlMerchant.SelectedValue.ToString());
                }

            }
            else
            {
                dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SI", txtSInvNo.Text, ddlMerchant.SelectedValue.ToString());
            }

        }

        //DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(ViewState["PID"].ToString(), Session["LocId"].ToString());
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

        DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(ViewState["PID"].ToString(), Session["LocId"].ToString());
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





            ////try
            ////{
            ////    dtstock = new DataView(dtstock, "ProductId=" + ViewState["PID"].ToString() + "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            ////}
            ////catch
            ////{
            ////}
            ////dtstock = dtstock.DefaultView.ToTable(true, "SerialNo");
            //for (int i = 0; i < dtstock.Rows.Count; i++)
            //{
            //    DataRow dr = dtserial.NewRow();
            //    string[] result = isSerialNumberValid(dtserial, dtstock.Rows[i]["SerialNo"].ToString(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
            //    if (result[0] == "VALID")
            //    {
            //        dr[0] = dtstock.Rows[i]["SerialNo"].ToString();
            //        dtserial.Rows.Add(dr);
            //        if (txtSerialNo.Text == "")
            //        {
            //            txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();
            //        }
            //        else
            //        {
            //            txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();
            //        }
            //        counter++;
            //    }
            //}
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
    #region Advancepayment
    public void FillAdvancePayment_BYOrderId(string OrderId, string OrderNo)
    {
        int counter = 0;

        //here we check that invoice is created or not agaisnt this order id 

        //if invoice created and posted then advance payment not showing for selected  order is posted
        DataTable dtpurchaseinvoicedetail = objSInvDetail.GetSInvDetailAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            dtpurchaseinvoicedetail = new DataView(dtpurchaseinvoicedetail, "SIFromTransNo=" + OrderId + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtpurchaseinvoicedetail.Rows.Count > 0)
        {


            //here we check that created invoice is posted or not 

            DataTable dtsalesInvoiceheader = objSInvHeader.GetSInvHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                dtsalesInvoiceheader = new DataView(dtsalesInvoiceheader, "Trans_Id=" + dtpurchaseinvoicedetail.Rows[0]["Invoice_No"].ToString() + " and Post='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (dtsalesInvoiceheader.Rows.Count > 0)
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



            if (Session["DtAdvancePayment"] == null)
            {
                DataTable dtAdvanceRecord = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "SO", OrderId);

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
                dtAdvancepayment = (DataTable)Session["DtAdvancePayment"];

                DataTable dtAdvanceRecord = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "SO", OrderId);

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

                ((Label)gvrow.FindControl("lblAmount")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), ((Label)gvrow.FindControl("lblAmount")).Text);
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
            ((Label)gvadvancepayment.FooterRow.FindControl("txttotAmount")).Text = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), f.ToString());
        }
        catch
        {

        }



    }
    #endregion
    protected void ddlPaymentTypeMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillTabPaymentMode(ddlPaymentMode.SelectedItem.Text);
    }
    #region PendingOrder

    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQSeleclField.SelectedItem.Value == "SalesOrderDate")
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
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "SalesOrderDate")
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
            }
            else
            {
                DisplayMessage("Enter Date");
                txtQValueDate.Focus();
                txtQValue.Text = "";
                return;
            }
            txtQValueDate.Focus();
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
            DataTable dtAdd = (DataTable)Session["dtPendingOrder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSalesOrder, view.ToTable(), "", "");

            Session["dtFilterPendingOrder"] = view.ToTable();
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            txtQValue.Focus();
        }
    }

    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ddlQSeleclField.SelectedIndex = 1;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        txtQValue.Visible = true;
        txtQValueDate.Visible = false;
        FillGridPendingOrder();
    }

    protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSalesOrder.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilterPendingOrder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dt, "", "");
        //AllPageCode();
        gvSalesOrder.BottomPagerRow.Focus();
    }
    protected void gvPurchaseOrder_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilterPendingOrder"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterPendingOrder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dt, "", "");
        //AllPageCode();
        gvSalesOrder.HeaderRow.Focus();

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


    private void FillGridPendingOrder()
    {
        DataTable dtQuotation = objSOrderHeader.GetPendingSalesOrder_ForSalesInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
        try
        {
            dtQuotation = new DataView(dtQuotation, "RemainQty>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtQuotation.Rows.Count + "";
        Session["dtPendingOrder"] = dtQuotation;
        Session["dtFilterPendingOrder"] = dtQuotation;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dtQuotation, "", "");
    }

    protected void btnPendingOrder_Click(object sender, EventArgs e)
    {
        FillGridPendingOrder();


    }
    //for child grid




    public string GetOrderCurrencySymbol(string Amount)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), Amount), HttpContext.Current.Session["DBConnection"].ToString());
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
    }

    #endregion
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

        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + CustomerId + "&&Page=SINV','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
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


    public void FillJobCardGrid()
    {

        DataTable dt = new DataTable();

        dt = objJobCardheader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());




        dt = new DataView(dt, "Status='Close' and Field6='False'", "", DataViewRowState.CurrentRows).ToTable();


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");


        lblTotalRecordsQuote.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        Session["dtCInquiry"] = dt;
        Session["dtAllCInquiry"] = dt;
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


    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((Button)sender).Parent.Parent;
        Reset();

        DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + ((Label)gvrow.FindControl("lblgvCustomerId")).Text + "'");
        if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
        {
            string strCustomerId = dtCustomerName.Rows[0]["Trans_Id"].ToString();
            string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
            string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
            txtCustomer.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
            //txtCustomer.Text = ((Label)gvrow.FindControl("lblgvCustomerName")).Text + "/" + ((Label)gvrow.FindControl("lblgvCustomerId")).Text;
        }

        strSiCustomerId = Convert.ToInt32(((Label)gvrow.FindControl("lblgvCustomerId")).Text);
        txtCustomer_TextChanged(null, null);
        RdoWithOutSo.Checked = true;
        RdoSo_CheckedChanged(null, null);

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
            objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
            GridCalculation();
        }

        if (Lbl_Tab_New.Text == "View")
        {
            btnSInvSave.Enabled = false;
            btnPost.Enabled = false;
            BtnReset.Enabled = false;
        }
        else
        {
            btnSInvSave.Enabled = true;
            btnPost.Enabled = true;
            BtnReset.Enabled = true;
        }
        //AllPageCode();

    }

    protected void btnPendingjobCard_Click(object sender, EventArgs e)
    {
        FillJobCardGrid();
        //AllPageCode();
    }

    #endregion


    public void DisableOrderList()
    {
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning");
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                {
                    ((CheckBox)gvrow.FindControl("chkTrandId")).Enabled = true;
                }
            }
        }
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

    protected void ClosePopUp()
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Model_GST()", true);

    }

    protected void btnSaveGST_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvTaxRow in gvTaxCalculation.Rows)
        {
            HiddenField lblgvProductId = (HiddenField)gvTaxRow.FindControl("lblgvProductId");
            HiddenField lblgvTaxId = (HiddenField)gvTaxRow.FindControl("lblgvTaxId");
            TextBox txtTaxValueInPer = (TextBox)gvTaxRow.FindControl("txtTaxValueInPer");

            foreach (GridViewRow gvRow in gvProduct.Rows)
            {
                Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
                Label lblTransId = (Label)gvRow.FindControl("lblTransId");
                Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
                if (lblgvProductId.Value == hdngvProductId.Value)
                {
                    Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
                    DropDownList ddlUnitName = (DropDownList)gvRow.FindControl("ddlUnitName");
                    TextBox lblgvUnitPrice = (TextBox)gvRow.FindControl("lblgvUnitPrice");
                    Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
                    TextBox lblgvFreeQuantity = (TextBox)gvRow.FindControl("lblgvFreeQuantity");
                    Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
                    Label lblgvSoldQuantity = (Label)gvRow.FindControl("lblgvSoldQuantity");
                    Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
                    TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtgvSalesQuantity");
                    TextBox txtgvTaxP = (TextBox)gvRow.FindControl("txtgvTaxP");
                    TextBox txtgvTaxV = (TextBox)gvRow.FindControl("txtgvTaxV");
                    TextBox txtgvDiscountP = (TextBox)gvRow.FindControl("txtgvDiscountP");
                    TextBox txtgvDiscountV = (TextBox)gvRow.FindControl("txtgvDiscountV");
                    Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
                    TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
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
                    double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
                    bool IsValidDiscount = IsApplyDiscount();

                    if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
                        txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();

                    if (!IsValidDiscount)
                        AmntAfterDiscnt = AmountValue;

                    //double TotalTax = ModifyTaxCalculation(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblgvTaxId.Value, txtTaxValueInPer.Text);
                    double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), hdngvProductId.Value, lblgvSerialNo.Text);
                    string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvSalesQuantity.Text, "", TotalTax.ToString(), "", txtgvDiscountV.Text, true, strCurrencyId, false, Session["DBConnection"].ToString());
                    lblgvQuantityPrice.Text = strcalc[0].ToString();
                    //txtgvTotal.Text = (AmntAfterDiscnt + TotalTax).ToString();  //strcalc[5].ToString();
                    txtgvTotal.Text = (AmntAfterDiscnt + ((AmntAfterDiscnt * double.Parse(txtgvTaxP.Text)) / 100)).ToString();
                    lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
                    lblgvFreeQuantity.Text = SetDecimal(lblgvFreeQuantity.Text);
                    lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
                    lblgvSoldQuantity.Text = SetDecimal(lblgvSoldQuantity.Text);
                    lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
                    txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
                    lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
                    txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
                    //txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
                    txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
                    txtgvTaxV.Text = Convert_Into_DF(TotalTax.ToString());
                }
            }
        }
        HeadearCalculateGrid();
        //AllPageCode();
        ClosePopUp();
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


    public void TaxCalculationWithDiscount()
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            refreshGvProduct(gvRow);

        }

        SetGridViewDecimal();
    }

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }

    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_SINV"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SINV"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                string F_Serial_No = string.Empty;

                if (e.CommandName.ToString() == "gvProduct")
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
                    if (Grid_Name == "gvProduct")
                    {
                        TextBox Unit_Price = (TextBox)gvProduct.Rows[Row_Index].FindControl("lblgvUnitPrice");
                        TextBox Discount_Price = (TextBox)gvProduct.Rows[Row_Index].FindControl("txtgvDiscountV");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
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
    protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Temp_Product_Tax_SINV"] = null;
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

        if (Session["Temp_Product_Tax_SINV"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SINV"] as DataTable;
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
                        //if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value)
                        {
                            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                            DR_Tax["TaxAmount"] = (Convert.ToDouble(Net_Unit_Price) * Convert.ToDouble(Tax_Percentage.Text)) / 100;
                            DR_Tax["Amount"] = Net_Unit_Price;
                        }
                    }
                }
            }
            Session["Temp_Product_Tax_SINV"] = Dt_Cal;
            foreach (GridViewRow dl in gvProduct.Rows)
            {
                if (Dt_Cal.Rows.Count > 0)
                {
                    DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "' And Serial_No='" + Serial_No + "' ", "", DataViewRowState.CurrentRows).ToTable();
                    //DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                            Label hdnSerialNo = (Label)dl.FindControl("lblgvSerialNo");
                            if (Product_ID == hdnProductId.Value && Serial_No == hdnSerialNo.Text)
                            {
                                Tax_Percent.Text = SetDecimal(Tax_Val.ToString());
                            }
                        }
                    }
                }
            }
            GridCalculation();
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
                        //TaxAmount = ((Convert.ToDouble(Convert_Into_DF(dr["TaxAmount"].ToString())) * Convert.ToDouble(R_Order_Req_Qty)).ToString());
                        TaxAmount = (Net_Amount * double.Parse(TaxValue) / 100).ToString();
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

    //public void Add_Tax_In_Session_From_Order(string Amount, string ProductId, string PO_ID)
    //{
    //    try
    //    {
    //        string TaxQuery = string.Empty;
    //        bool IsTax = IsApplyTax();
    //        if (IsTax == true)
    //        {
    //            if (ddlTransType.SelectedIndex > 0)
    //            {
    //                if (PO_ID != "")
    //                {
    //                    DataTable DT_Db_Details = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SO", PO_ID);
    //                    if (DT_Db_Details.Rows.Count > 0)
    //                    {
    //                        TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesOrderDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + PO_ID + "' and TRD.Ref_Type='SO' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
    //                        DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
    //                        Session["Temp_Product_Tax_SINV"] = null;
    //                        DataTable Dt_Temp = new DataTable();
    //                        Dt_Temp = TemporaryProductWiseTaxes();
    //                        Dt_Temp = Dt_Inv_TaxRefDetail;
    //                        if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
    //                        {
    //                            Session["Temp_Product_Tax_SINV"] = Dt_Temp;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}




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
            if (txtFCExpAmount.Text.Trim() != "")
            {
                double Expenses_Tax_Amount = Convert.ToDouble(txtFCExpAmount.Text.Trim());
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

    protected void txtExpCharges_TextChanged(object sender, EventArgs e)
    {

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
    public void Tax_Insert_Into_Inv_TaxRefDetail(string PI_Header_ID, ref SqlTransaction trns)
    {
        bool IsTax = IsApplyTax();
        if (IsTax == true)
        {
            //objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SINV", PI_Header_ID.ToString(), ref trns);
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
                            objTaxRefDetail.InsertRecord_Expenses("SINV", PI_Header_ID, "0", "0", "0", Dt_Row["Tax_Type_Id"].ToString(), Dt_Row["Tax_Percentage"].ToString(), Dt_Row["Tax_Value"].ToString(), false.ToString(), Dt_Row["Expenses_Amount"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Dt_Row["Expenses_Id"].ToString(), ref trns);
                    }
                }
            }
        }
    }


    protected void Btn_Add_Expenses_Click(object sender, EventArgs e)
    {
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
            DisplayMessage("Enter Expense Account");
            txtExpensesAccount.Focus();
            return;
        }
        //

        if (txtFCExpAmount.Text == "")
        {
            DisplayMessage("Enter Expenses Charges");
            txtFCExpAmount.Focus();
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
            Dt_Grid_Final_Tax_Save = Session["Expenses_Tax_Sales_Invoice"] as DataTable;
            if (Dt_Grid_Final_Tax_Save == null)
            {
                Dt_Grid_Final_Tax_Save = new DataTable();
                Dt_Grid_Final_Tax_Save.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
            }

            DataTable Dt_Grid_Tax_Web_Control = Session[Hdn_Saved_Expenses_Tax_Session.Value] as DataTable;
            Dt_Grid_Tax_Web_Control = new DataView(Dt_Grid_Tax_Web_Control, "Expenses_Id='" + ddlExpense.SelectedValue.ToString() + "' and Expenses_Amount='" + txtFCExpAmount.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            foreach (DataRow Dt_Row in Dt_Grid_Tax_Web_Control.Rows)
            {
                Dt_Grid_Final_Tax_Save.Rows.Add(Dt_Row["Tax_Type_Id"].ToString(), Dt_Row["Tax_Type_Name"].ToString(), Dt_Row["Tax_Percentage"].ToString(), Dt_Row["Tax_Value"].ToString(), Dt_Row["Expenses_Id"].ToString(), Dt_Row["Expenses_Name"].ToString(), Dt_Row["Expenses_Amount"].ToString(), Dt_Row["Page_Name"].ToString(), Dt_Row["Tax_Entry_Type"].ToString(), Dt_Row["Tax_Account_Id"].ToString());
            }
            Session["Expenses_Tax_Sales_Invoice"] = Dt_Grid_Final_Tax_Save;
            Dt_Final_Save_Tax = Session["Expenses_Tax_Sales_Invoice"] as DataTable;
        }





        DataTable dt = new DataTable();
        int i = 0;
        bool b = false;
        if (ViewState["ExpdtSales"] != null)
        {
            dt = (DataTable)ViewState["ExpdtSales"];
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
        }
        else
        {
            dt.Columns.Add("Expense_Id");
            dt.Columns.Add("Account_No");
            dt.Columns.Add("Exp_Charges");
            dt.Columns.Add("ExpCurrencyID");
            dt.Columns.Add("ExpExchangeRate");
            dt.Columns.Add("FCExpAmount");

            dt.Rows.Add();
        }

        dt.Rows[i]["Expense_Id"] = ddlExpense.SelectedValue.ToString();
        dt.Rows[i]["Account_No"] = GetAccountId(txtExpensesAccount.Text);
        dt.Rows[i]["Exp_Charges"] = txtExpCharges.Text.Trim();
        dt.Rows[i]["ExpCurrencyID"] = ddlExpCurrency.SelectedValue.ToString();
        dt.Rows[i]["ExpExchangeRate"] = SetDecimal(txtExchangeRate.Text.ToString());
        dt.Rows[i]["FCExpAmount"] = txtFCExpAmount.Text.ToString();

        txtExpCharges.Text = "0";
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";

        txtExpExchangeRate.Text = "0";
        txtFCExpAmount.Text = "0";
        ViewState["ExpdtSales"] = dt;
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
        if (Session["Expenses_Tax_Sales_Invoice"] != null)
        {
            DataTable Dt_Cal = Session["Expenses_Tax_Sales_Invoice"] as DataTable;
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

    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {
        txtExpExchangeRate.Text = txtExchangeRate.Text;
        //ddlCurrency_OnSelectedIndexChanged(null, null);
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
                //Amount = Amount.Substring(0, index + (Decimal_Count_For_Tax + 1));
                Amount = Math.Round(Amount_D, Decimal_Count_For_Tax).ToString();
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

    protected void txtCreditNote_TextChanged(object sender, EventArgs e)
    {
        double amt = 0;
        if (txtCreditNote.Text.Trim() != string.Empty)
        {
            try
            {
                DataTable _dt = getUnAdjustedCreditNote();
                if (_dt.Rows.Count > 0)
                {
                    _dt = new DataView(_dt, "voucher_no='" + txtCreditNote.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (_dt.Rows.Count > 0)
                    {
                        if (ddlCurrency.SelectedValue != _dt.Rows[0]["currency_id"].ToString())
                        {
                            txtCreditNote.Text = "";
                            txtCreditNote.Focus();
                            DisplayMessage("Currency Mismatch please select another credit note");
                            return;
                        }
                        else
                        {
                            txtPayAmount.Text = SetDecimal(_dt.Rows[0]["Actual_balance_amount"].ToString());
                            txtPayAmount_OnTextChanged(null, null);
                            double.TryParse(txtPayAmount.Text, out amt);
                        }
                    }

                }
            }
            catch
            {

            }
        }
        if (amt == 0)
        {
            txtCreditNote.Text = "";
            txtCreditNote.Focus();
            DisplayMessage("This credit Note is not Valid ");
        }
    }

    //protected void btnModalAccountMaster_Click(object sender, EventArgs e)
    //{
    //    string CustomerId = txtCustomer.Text.Split('/')[1].ToString();
    //    UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", txtCustomer.Text.Split('/')[0].ToString());
    //    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
    //}

    protected void setDefaultValueForUcAcMaster()
    {
        string CustomerId = txtCustomer.Text.Split('/')[3].ToString();
        UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", txtCustomer.Text.Split('/')[0].ToString());
    }

    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings("CustomerMaster", GvSalesInvoice, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvSalesInvoice, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, EventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
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
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }


    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }

    protected void chkShortProductName_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in gvProduct.Rows)
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

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {


    }

    protected void Btn_Li_Import_Click(object sender, EventArgs e)
    {

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

    protected void lnkTotalExcelImportRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportList"] != null)
        {
            List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList> newInvoiceList = (List<Inv_SalesInvoiceHeader.clsImportEComInvoiceList>)Session["ExcelImportList"];
            gvImportInvoiceList.DataSource = newInvoiceList;
            gvImportInvoiceList.DataBind();
        }
    }

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

    #region Transport Section
    protected void ChkTrans_Changed(object sender, EventArgs e)
    {
        if (chkCustomer.Checked == true)
        {
            txtcustomername.Visible = true;
            txtPersonName.Visible = true;
            txtPersonMobileNo.Visible = true;
            Label23.Visible = true;
            lblPersonName.Visible = true;
            lblPersonMobileNo.Visible = true;
            pnlCustomer.Visible = true;
            pnlEmployee.Visible = false;
            txtPermanentMobileNo.Visible = true;
        }
        else
        {
            txtPermanentMobileNo.Visible = false;
            txtcustomername.Visible = false;
            txtPersonName.Visible = false;
            txtPersonMobileNo.Visible = false;
            Label23.Visible = false;
            lblPersonName.Visible = false;
            lblPersonMobileNo.Visible = false;
            pnlEmployee.Visible = true;
            pnlCustomer.Visible = false;
        }
    }
    protected void txtcustomername_TextChanged(object sender, EventArgs e)
    {

        string custid = string.Empty;
        if (txtcustomername.Text != "")
        {
            try
            {
                custid = txtcustomername.Text.Split('/')[1].ToString();
            }
            catch
            {
                custid = "0";
            }
            DataTable dtContactmaster = ObjContactMaster.GetContactTrueById(custid);
            if (dtContactmaster.Rows.Count > 0)
            {
                custid = dtContactmaster.Rows[0]["Trans_Id"].ToString();

                string[] strcustInfo = getCustomerInformation(custid);
                txtPermanentMobileNo.Text = strcustInfo[1].ToString();
                //hdncust.Value = custid;
            }
            else
            {
                txtPermanentMobileNo.Text = "";
            }
        }
        else
        {
            txtPermanentMobileNo.Text = "";

        }
    }
    public string[] getCustomerInformation(string Contactid)
    {
        //code start
        string[] strCusInfo = new string[4];
        string strContactNo = string.Empty;
        string Address = string.Empty;
        string Longitude = string.Empty;
        string Latitude = string.Empty;
        DataTable dtContact = objContact.GetContactTrueById(Contactid);
        if (dtContact.Rows.Count > 0)
        {
            strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString();
        }
        //for get address
        DataTable dt = objAddressChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", Contactid);

        if (dt.Rows.Count > 0)
        {

            Address = dt.Rows[0]["Address"].ToString();
            if (dt.Rows[0]["Address"].ToString() != "")
            {
                Address = dt.Rows[0]["Address"].ToString();
            }
            if (dt.Rows[0]["Street"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Street"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Street"].ToString();
                }
            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Block"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Block"].ToString();
                }
            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Avenue"].ToString();
                }
            }
            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["CityId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["CityId"].ToString();
                }
            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {


                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["StateId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["StateId"].ToString();
                }

            }
            if (dt.Rows[0]["CountryId"].ToString() != "")
            {
                CountryMaster objCountry = new CountryMaster(Session["DBConnection"].ToString());


                if (Address != "")
                {
                    Address = Address + "," + objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }
                else
                {
                    Address = objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["PinCode"].ToString();
                }
            }
            strCusInfo[0] = Address;
            strCusInfo[2] = dt.Rows[0]["Longitude"].ToString();
            strCusInfo[3] = dt.Rows[0]["Latitude"].ToString();
        }
        else
        {
            strCusInfo[0] = "";
            strCusInfo[2] = "0.0000";
            strCusInfo[3] = "0.0000";
        }
        return strCusInfo;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAreaName(string prefixText, int count, string contextKey)
    {
        Sys_AreaMaster objAreaMaster = new Sys_AreaMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAreaMaster.GetAreaMaster(), "Area_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Area_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDriverName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }
    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtvehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtvehiclename.Text = "";
                txtvehiclename.Focus();
                return;
            }
            else
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtdrivername.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString() + "/" + dt.Rows[0]["Emp_id"].ToString();
                }
            }
        }

    }
    protected void txtdrivername_TextChanged(object sender, EventArgs e)
    {



        string empname = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            try
            {
                empname = ((TextBox)sender).Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }

            DataTable dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();




            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Emp_Code='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtvehiclename.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Vehicle_Id"].ToString();
                }
                else
                {
                    txtvehiclename.Text = "";
                }

            }
            else
            {
                txtvehiclename.Text = "";
            }
            if (dtEmp.Rows.Count == 0)
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
    }
    public string GetCustomerNameByCustomerId(string CustomerId)
    {
        string CustomerName = "";
        string Name = objContact.GetContactNameByContactiD(CustomerId);
        //DataTable dt = ObjCustmer.GetCustomerAllDataByCustomerId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), CustomerId);
        if (Name != null)
        {
            CustomerName = Name;
        }
        return CustomerName;
    }
    public string GetvechileNameByVechileId(string VechileId)
    {
        string VechileName = "";
        DataTable dt = objVehicleMaster.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VechileId);
        if (dt.Rows.Count > 0)
        {
            VechileName = dt.Rows[0]["Name"].ToString();
        }
        return VechileName;
    }
    public string GetDriverNamebyDriverId(string DriverId)
    {
        string DriverName = "";
        DataTable dt = objVehicleMaster.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), DriverId);
        if (dt.Rows.Count > 0)
        {
            DriverName = dt.Rows[0]["Emp_Name"].ToString();
        }
        return DriverName;
    }
    public void ResetTransPort()
    {
        txtcustomername.Text = "";
        txtPermanentMobileNo.Text = "";
        txtAreaName.Text = "";
        //txtEmployee.Text = "";
        txtVisitDate.Text = "";
        txtVisitTime.Text = "";
        txtvehiclename.Text = "";
        txtdrivername.Text = "";
        txtdescription.Text = "";
        txtChargableAmount.Text = "";
        txtPersonMobileNo.Text = "";
        txtPersonName.Text = "";
        txtTrakingId.Text = "";
    }
    #endregion
    #region TransPort Model Code 
    protected void IbtnTransPort_Command(object sender, CommandEventArgs e)
    {

        //string CommandName = e.CommandName.ToString();
        //string CommandArgument = e.CommandArgument.ToString();
        ////GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
        //AddTransModalPort.SetTransportId(CommandName, CommandArgument);
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_TransPort_Open()", true);

    }
    #endregion


}
