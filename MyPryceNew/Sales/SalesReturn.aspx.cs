using System;
using System.Collections;
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
using System.Configuration;

public partial class Sales_SalesReturn : BasePage
{
    #region defined Class Object
    Common cmn = null;
    DataAccessClass da = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    Set_AddressMaster objAddMaster = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductLedger = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    LocationMaster objLocation = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_SalesReturnHeader ObjSalesReturnHeader = null;
    Inv_SalesReturnDetail ObjSalesReturnDetail = null;
    Inv_StockDetail objStockDetail = null;
    Inv_SalesOrderHeader ObjSalesOrderHeader = null;
    Inv_SalesDeliveryVoucher_Header objDeliveryVoucherheader = null;
    Inv_SalesCommission_Detail objCommisisonDetail = null;
    Inv_ReturnCommission objReturnCommission = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    Ac_AccountMaster objAcAccountMaster = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Set_CustomerMaster objCustomer = null;
    string StrLocationId = string.Empty;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    public const int grdDefaultColCount = 4;
    private const string strPageName = "SalesReturn";
    #endregion
    public static int Decimal_Count_For_Tax;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductLedger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjSalesReturnHeader = new Inv_SalesReturnHeader(Session["DBConnection"].ToString());
        ObjSalesReturnDetail = new Inv_SalesReturnDetail(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSalesOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objDeliveryVoucherheader = new Inv_SalesDeliveryVoucher_Header(Session["DBConnection"].ToString());
        objCommisisonDetail = new Inv_SalesCommission_Detail(Session["DBConnection"].ToString());
        objReturnCommission = new Inv_ReturnCommission(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrLocationId = Session["LocId"].ToString();
        //btnSInvSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        if (!IsPostBack)
        {
            Session["isSalesTaxEnabled"] = null;
            Session["IsSalesDiscountEnabled"] = null;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/SalesReturn.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            string Decimal_Count = string.Empty;
            Decimal_Count = Session["LoginLocDecimalCount"].ToString();
            Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
            Session["SENID"] = "False";
            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            ViewState["dtSerial"] = null;
            ViewState["Tempdt"] = null;
            ViewState["dtFinal"] = null;
            ddlOption.SelectedIndex = 2;
            txtSreturnNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtSreturnNo.Text;
            txtsReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Calender.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            FillGrid();
            FillPaymentMode();
            //this code for when we redirect from the producte ledger page 
            //this code created on 22-07-2015
            //code start
            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                Session["SENID"] = "lnkViewDetail";
                btnEdit_Command(imgeditbutton, new CommandEventArgs("commandName", Request.QueryString["Id"].ToString()));
                // btnList.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_List()", true);
                btnSInvCancel.Visible = false;
                StrLocationId = Request.QueryString["LocId"].ToString();
                //((Panel)Master.FindControl("pnlaccordian")).Visible = false;
            }
            else
            {
                Session["SENID"] = "False";
                btnSInvCancel.Visible = true;
            }
            TaxandDiscountParameter();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            getPageControlsVisibility();
        }
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSInvSave.Visible = clsPagePermission.bAdd;
        btnPost.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnUploadExcel.Visible = clsPagePermission.bAdd;
        btnSaveExcelInvoice.Visible = clsPagePermission.bAdd;
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "120", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #region System defind Funcation

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string objSenderID = string.Empty;
        //Session["SENID"].ToString() == "lnkViewDetail"
        if (((LinkButton)sender).ID == "lnkViewDetail")
        {
            objSenderID = "lnkViewDetail";
        }
        else
        {
            GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
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

            //here we checking post status if record posted than can not update 
            if (objSenderID != "lnkViewDetail")
            {
                if (Convert.ToBoolean(((Label)gvRow.FindControl("lblgvPost")).Text))
                {
                    DisplayMessage("Record has Posted,Can not be update");
                    return;
                }
                else
                {
                    chkPost.Checked = false;
                }
            }
        }

        LinkButton lb = (LinkButton)sender;

        if (lb.ID == "lnkViewDetail")
        {
            objSenderID = "lnkViewDetail";
        }
        else
        {
            objSenderID = "btnEdit";
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

        editid.Value = e.CommandArgument.ToString();

        using (DataTable dtInvEdit = ObjSalesReturnHeader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, editid.Value))
        {
            if (objSenderID == "lnkViewDetail")
            {

                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else if (objSenderID == "btnpullBrand")
            {
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else if (objSenderID == "btnEdit")
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            if (dtInvEdit.Rows.Count > 0)
            {
                DataTable dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "SR", editid.Value);
                if (dtStock.Rows.Count > 0)
                {

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
                txtSreturnNo.Enabled = false;
                txtSreturnNo.Text = dtInvEdit.Rows[0]["Return_No"].ToString();
                txtsReturnDate.Text = GetDate(dtInvEdit.Rows[0]["Return_Date"].ToString());
                txtSInvNo.Text = dtInvEdit.Rows[0]["Invoice_No"].ToString();
                txtSInvDate.Text = GetDate(dtInvEdit.Rows[0]["Invoice_Date"].ToString());
                txtCustomerName.Text = Set_CustomerMaster.GetCustomerName(dtInvEdit.Rows[0]["Customer_Id"].ToString(), Session["DBConnection"].ToString());
                ArrayList Objarr = new ArrayList();
                Objarr.Add(dtInvEdit.Rows[0]["Currency_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["Customer_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["SalesPerson_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["SIFromTransType"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["Invoice_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["Invoice_Merchant_Id"].ToString());
                Session["InvInformation"] = Objarr;
                Session["CurrencyId"] = dtInvEdit.Rows[0]["Currency_Id"].ToString();
                Session["CustomerId"] = dtInvEdit.Rows[0]["Customer_Id"].ToString();

                try
                {
                    ddlPaymentMode.SelectedValue = dtInvEdit.Rows[0]["PaymentModeId"].ToString();
                }
                catch
                {
                    ddlPaymentMode.SelectedIndex = 0;
                }
                //txtPaymentMode.Text = GetPaymentModeName(dtInvEdit.Rows[0]["PaymentModeId"].ToString());
                txtCurrency.Text = CurrencyMaster.GetCurrencyNameByCurrencyId(dtInvEdit.Rows[0]["Currency_Id"].ToString(), Session["DBConnection"].ToString());

                txtSalesPerson.Text = Common.GetEmployeeName(dtInvEdit.Rows[0]["SalesPerson_Id"].ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString());
                txtPOSNo.Text = dtInvEdit.Rows[0]["PosNo"].ToString();
                try
                {
                    txtRemark.Text = dtInvEdit.Rows[0]["Remark"].ToString();
                }
                catch
                {

                }

                txtTaxP.Text = SetDecimal(dtInvEdit.Rows[0]["NetTaxP"].ToString());
                txtDiscountP.Text = SetDecimal(dtInvEdit.Rows[0]["NetDiscountP"].ToString());
                string strTransNo = string.Empty;
                string strTransType = dtInvEdit.Rows[0]["SIFromTransType"].ToString();
                DataTable Dt_Get_record = ObjSalesReturnDetail.GetAllRecord_ByReturnNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), editid.Value);
                Hdn_Exchange_Rate.Value = Dt_Get_record.Rows[0]["Exchange_Rate"].ToString();
                GvSalesInvoiceDetailDirect.DataSource = Dt_Get_record;
                GvSalesInvoiceDetailDirect.DataBind();
            }
        }
        EditGridCalculation();
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

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
        Update_New.Update();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        if (Convert.ToBoolean(((Label)gvRow.FindControl("lblgvPost")).Text))
        {
            DisplayMessage("Record has Posted,Can not be delete !");
            return;
        }
        ObjSalesReturnHeader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        string strSql = "delete from Inv_StockBatchMaster where TransType='SR' and TransTypeId=" + e.CommandArgument.ToString() + "";
        da.execute_Command(strSql);
        DisplayMessage("Record Deleted SuccessFully !");
        FillGrid();
    }
    public void GridCalculation()
    {
        foreach (GridViewRow gvRow in GvSalesInvoiceDetailDirect.Rows)
        {
            TextBox txtgvReturnQuantity = (TextBox)gvRow.FindControl("txtgvReturnQuantity");
            Label txtgvSalesQuantity = (Label)gvRow.FindControl("lblgvQuantity");
            Label lblgvUnitPrice = (Label)gvRow.FindControl("lblgvUnitPrice");
            Label txtgvTaxV = (Label)gvRow.FindControl("txtgvTaxV");
            Label txtgvDiscountV = (Label)gvRow.FindControl("txtgvDiscountV");
            Label txtgvTaxP = (Label)gvRow.FindControl("txtgvTaxP");
            Label txtgvDiscountP = (Label)gvRow.FindControl("txtgvDiscountP");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");

            Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");
            Label lblgvTotalReturnQuantity = (Label)gvRow.FindControl("lblgvReturnQuantity");
            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
            Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");
            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }
            if (txtgvReturnQuantity.Text == "")
            {
                txtgvReturnQuantity.Text = "0";
            }
            if (lblgvTotalReturnQuantity.Text == "")
            {
                lblgvTotalReturnQuantity.Text = "0";
            }
            txtgvReturnQuantity.Text = "0";
            string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvReturnQuantity.Text, "", Convert_Into_DF(txtgvTaxV.Text), "", txtgvDiscountV.Text, true, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), false, Session["DBConnection"].ToString());
            lblgvQuantityPrice.Text = strcalc[0].ToString();

            txtgvTotal.Text = Convert_Into_DF(strcalc[5].ToString());
            lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);
            lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);
            lblgvRemaningQuantity.Text = SetDecimal((Convert.ToDouble(txtgvSalesQuantity.Text) - Convert.ToDouble(lblgvTotalReturnQuantity.Text)).ToString());
            lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
            txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
            txtgvReturnQuantity.Text = SetDecimal(txtgvReturnQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
            lblgvTotalReturnQuantity.Text = SetDecimal(lblgvTotalReturnQuantity.Text);
            txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
            txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
            txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
            txtgvTaxV.Text = Convert_Into_DF(txtgvTaxV.Text);
        }
        HeadearCalculateGrid();
    }
    public void EditGridCalculation()
    {
        foreach (GridViewRow gvRow in GvSalesInvoiceDetailDirect.Rows)
        {
            TextBox txtgvReturnQuantity = (TextBox)gvRow.FindControl("txtgvReturnQuantity");
            Label txtgvSalesQuantity = (Label)gvRow.FindControl("lblgvQuantity");
            Label lblgvUnitPrice = (Label)gvRow.FindControl("lblgvUnitPrice");
            Label txtgvTaxV = (Label)gvRow.FindControl("txtgvTaxV");
            Label txtgvDiscountV = (Label)gvRow.FindControl("txtgvDiscountV");
            Label txtgvTaxP = (Label)gvRow.FindControl("txtgvTaxP");
            Label txtgvDiscountP = (Label)gvRow.FindControl("txtgvDiscountP");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
            Label lblgvTotalReturnQuantity = (Label)gvRow.FindControl("lblgvReturnQuantity");
            Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");
            Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");

            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
            Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");

            if (lblgvUnitPrice.Text == "")
            {
                lblgvUnitPrice.Text = "0";
            }
            if (txtgvReturnQuantity.Text == "")
            {
                txtgvReturnQuantity.Text = "0";
            }


            string[] strcalc = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvReturnQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), false, Session["DBConnection"].ToString());




            lblgvQuantityPrice.Text = strcalc[0].ToString();
            txtgvTotal.Text = Convert_Into_DF(strcalc[5].ToString());
            lblgvUnitPrice.Text = SetDecimal(lblgvUnitPrice.Text);

            lblgvOrderqty.Text = SetDecimal(lblgvOrderqty.Text);

            lblgvSystemQuantity.Text = SetDecimal(lblgvSystemQuantity.Text);
            txtgvSalesQuantity.Text = SetDecimal(txtgvSalesQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal((Convert.ToDouble(txtgvSalesQuantity.Text) - Convert.ToDouble(lblgvTotalReturnQuantity.Text)).ToString());
            txtgvReturnQuantity.Text = SetDecimal(txtgvReturnQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
            txtgvDiscountP.Text = SetDecimal(txtgvDiscountP.Text);
            txtgvDiscountV.Text = SetDecimal(txtgvDiscountV.Text);
            txtgvTaxP.Text = SetDecimal(txtgvTaxP.Text);
            txtgvTaxV.Text = Convert_Into_DF(txtgvTaxV.Text);
            lblgvTotalReturnQuantity.Text = SetDecimal(lblgvTotalReturnQuantity.Text);
            lblgvRemaningQuantity.Text = SetDecimal(lblgvRemaningQuantity.Text);
        }
        HeadearCalculateGrid();

    }
    public void HeadearCalculateGrid()
    {
        Double grosstotal = 0;
        Double Nettax = 0;
        Double NetDiscount = 0;
        Double Nettotal = 0;
        Double TotalQty = 0;
        txtAmount.Text = SetDecimal("0");
        txtTaxV.Text = Convert_Into_DF("0");
        txtDiscountV.Text = "0";
        txtTotalQuantity.Text = "0";
        txtNetAmount.Text = SetDecimal("0");
        foreach (GridViewRow gvRow in GvSalesInvoiceDetailDirect.Rows)
        {
            Label lblgvUnitPrice = (Label)gvRow.FindControl("lblgvUnitPrice");
            TextBox txtgvReturnQuantity = (TextBox)gvRow.FindControl("txtgvReturnQuantity");
            Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
            Label txtgvTaxV = (Label)gvRow.FindControl("txtgvTaxV");
            Label txtgvDiscountV = (Label)gvRow.FindControl("txtgvDiscountV");
            Label txtgvTaxP = (Label)gvRow.FindControl("txtgvTaxP");
            Label txtgvDiscountP = (Label)gvRow.FindControl("txtgvDiscountP");
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
            if (txtgvReturnQuantity.Text == "")
            {
                txtgvReturnQuantity.Text = "0";
            }
            if (txtgvTotal.Text == "")
            {
                txtgvTotal.Text = Convert_Into_DF("0");
            }
            if (lblgvQuantityPrice.Text == "")
            {
                lblgvQuantityPrice.Text = "0";
            }
            TotalQty += Convert.ToDouble(txtgvReturnQuantity.Text);
            grosstotal += Convert.ToDouble(lblgvQuantityPrice.Text);
            //Nettax += Convert.ToDouble(txtgvTaxV.Text) * Convert.ToDouble(txtgvReturnQuantity.Text);
            if (Convert.ToDouble(txtgvReturnQuantity.Text) != 0)
            {

                // Nettax += Convert.ToDouble(txtgvTaxV.Text);
                ////NetDiscount += Convert.ToDouble(txtgvDiscountV.Text) * Convert.ToDouble(txtgvReturnQuantity.Text);
                //NetDiscount += Convert.ToDouble(txtgvDiscountV.Text);

                double Unit_Price = Convert.ToDouble(lblgvUnitPrice.Text);
                double Discount_Percent = Convert.ToDouble(txtgvDiscountP.Text);
                double Tax_Percent = Convert.ToDouble(txtgvTaxP.Text);
                double Return_Qty = Convert.ToDouble(txtgvReturnQuantity.Text);
                double Net_Unit_Price = Unit_Price * Return_Qty;
                double Net_Discount_Value = ((Net_Unit_Price * Discount_Percent) / 100);
                double Unit_Price_After_Discount = Net_Unit_Price - ((Net_Unit_Price * Discount_Percent) / 100);
                double Net_Tax_Value = ((Convert.ToDouble(Convert_Into_DF(Unit_Price_After_Discount.ToString())) * Convert.ToDouble(Convert_Into_DF(Tax_Percent.ToString()))) / 100);
                Nettax = Convert.ToDouble(Convert_Into_DF(Nettax.ToString())) + Convert.ToDouble(Convert_Into_DF(Net_Tax_Value.ToString()));
                NetDiscount = NetDiscount + Net_Discount_Value;
            }
            Nettotal = Convert.ToDouble(Convert_Into_DF((Convert.ToDouble(Convert_Into_DF(Nettotal.ToString())) + Convert.ToDouble(Convert_Into_DF(txtgvTotal.Text))).ToString()));

        }
        txtAmount.Text = SetDecimal(grosstotal.ToString());

        string[] str = Common.TaxDiscountCaluculation(SetDecimal(txtAmount.Text), "0", txtTaxP.Text, "", txtDiscountP.Text, "", false, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), false, Session["DBConnection"].ToString());
        if (TotalQty > 0)
        {
            txtDiscountV.Text = SetDecimal(NetDiscount.ToString());
            txtTaxV.Text = Convert_Into_DF(Nettax.ToString());
            txtTaxP.Text = SetDecimal(((Convert.ToDouble(Convert_Into_DF(txtTaxV.Text)) * 100) / (Convert.ToDouble(SetDecimal(txtAmount.Text)) - Convert.ToDouble(txtDiscountV.Text))).ToString());
            txtDiscountP.Text = SetDecimal(((Convert.ToDouble(txtDiscountV.Text) * 100) / Convert.ToDouble(SetDecimal(txtAmount.Text))).ToString());
        }
        else
        {
            txtDiscountV.Text = "0.00";
            txtTaxV.Text = Convert_Into_DF("0.00");
            txtTaxP.Text = "0.00";
            txtDiscountP.Text = "0.00";
        }
        // txtTaxV.Text = SetDecimal(str[4].ToString());
        //txtNetAmount.Text = SetDecimal((Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscountV.Text) + Convert.ToDouble(txtTaxV.Text)).ToString());
        txtNetAmount.Text = SetDecimal(Nettotal.ToString());
        //txtNetAmount.Text = SystemParameter.GetScaleAmount(Nettotal.ToString(), "0");
        txtTotalQuantity.Text = SetDecimal(TotalQty.ToString());
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "Invoice_Date" || ddlFieldName.SelectedItem.Value == "Return_Date")
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
        FillGrid();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvSalesReturn_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesReturn.Attributes["CurrentSortField"] != null &&
            GvSalesReturn.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesReturn.Attributes["CurrentSortField"])
            {
                if (GvSalesReturn.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesReturn.Attributes["CurrentSortField"] = sortField;
        GvSalesReturn.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvSalesreturnCurrentPageIndex.Value));
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlFieldName.SelectedIndex = 2;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        FillGrid();

    }
    protected void btnSInvCancel_Click(object sender, EventArgs e)
    {
        Reset();
        //FillGrid();
        txtSreturnNo.Text = GetDocumentNumber();
        txtsReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
    }
    public string GetCurrencyforLocal(string strToCurrency, string strLocalAmount, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strToCurrency, strCurrency, ref trns);
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void btnSInvSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        Button btn = (Button)sender;

        if (txtSreturnNo.Text == "")
        {
            DisplayMessage("Enter Sales Return No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSreturnNo);

            return;
        }

        if (txtsReturnDate.Text == "")
        {
            DisplayMessage("Enter Sales Return Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtsReturnDate);

            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtsReturnDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Sales Return Date in format " + Session["DateFormat"].ToString() + "");
                txtsReturnDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtsReturnDate);

                return;
            }
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtsReturnDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        if (txtSInvNo.Text == "")
        {
            DisplayMessage("Enter Sales Invoice No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);

            return;
        }

        if (ddlPaymentMode.SelectedIndex == 0)
        {
            DisplayMessage("Select Payment Mode !");
            ddlPaymentMode.Focus();
            return;
        }


        bool IsReturnqty = false;

        foreach (GridViewRow gvr in GvSalesInvoiceDetailDirect.Rows)
        {
            TextBox txtgvReturnQty = (TextBox)gvr.FindControl("txtgvReturnQuantity");

            if (txtgvReturnQty.Text.Trim() == "")
            {
                txtgvReturnQty.Text = "0";
            }

            if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
            {
                IsReturnqty = true;
                break;
            }
        }

        if (!IsReturnqty)
        {
            DisplayMessage("Return Quantity Should be greater than 0 !");
            return;
        }


        //here we checking serial validation when serial validation parameter is true in inventory pages
        //cpde created on 16-09-2016
        //code created by jitendra upadhyay 


        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Sales Return").Rows[0]["ParameterValue"]))
        {
            if (btn.ID.Trim() == "btnPost")
            {
                foreach (GridViewRow gvr in GvSalesInvoiceDetailDirect.Rows)
                {
                    double Totalreturnqty = 0;

                    Totalreturnqty = Convert.ToDouble(((TextBox)gvr.FindControl("txtgvReturnQuantity")).Text);

                    if (Totalreturnqty > 0)
                    {
                        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
                        {
                            DataTable dt = (DataTable)ViewState["dtFinal"];

                            if (dt != null)
                            {

                                dt = new DataView(dt, "Product_Id='" + ((HiddenField)gvr.FindControl("hdngvProductId")).Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
                                if (dt.Rows.Count == 0)
                                {
                                    //DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblgvProductCode")).Text);
                                    //return;
                                }

                                if (dt.Rows.Count != Totalreturnqty)
                                {
                                    //DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblgvProductCode")).Text);
                                    //return;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        if (btn.ID.Trim() == "btnPost")
        {
            chkPost.Checked = true;
        }
        else
        {
            chkPost.Checked = false;
        }


        string strinvoiceId = string.Empty;
        string strOtherAccountId = "0";
        ArrayList Objarr = (ArrayList)Session["InvInformation"];
        //In case of credit return account no should exist - Neelkanth Purohit 03/09/2018
        if (chkPost.Checked == true)
        {
            DataTable _dtPayMode = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, Session["BrandId"].ToString(), Session["LocId"].ToString());
            bool isCreditMode = _dtPayMode.Rows.Count == 0 || _dtPayMode.Rows[0]["field1"].ToString() != "Credit" ? false : true;
            if (isCreditMode == true)
            {
                string strMerchantContactId = "0";
                if (!string.IsNullOrEmpty(Objarr[5].ToString()) && Objarr[5].ToString() != "0")
                {
                    using (DataTable _dt = new MerchantMaster(Session["DBConnection"].ToString()).GetMerchantMasterById(Objarr[5].ToString()))
                    {
                        if (_dt.Rows.Count > 0 && _dt.Rows[0]["Merchant_name"].ToString().ToLower() != "direct")
                        {
                            if (string.IsNullOrEmpty(_dt.Rows[0]["field1"].ToString()) || _dt.Rows[0]["field1"].ToString() == "0")
                            {
                                throw new System.ArgumentException("Contact is not linked with merchant - " + _dt.Rows[0]["Merchant_Name"].ToString());
                            }
                            else
                            {
                                strMerchantContactId = _dt.Rows[0]["field1"].ToString();
                            }

                        }
                    }
                }

                if (strMerchantContactId != "0")
                {
                    strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(strMerchantContactId, Objarr[0].ToString()).ToString();
                }
                else
                {
                    strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(Objarr[1].ToString(), Objarr[0].ToString()).ToString();
                }

                if (strOtherAccountId == "0")
                {
                    DisplayMessage("Account Detail not exist for this " + strMerchantContactId != "0" ? "Merchant" : "Customer" + " , Please first create Account");
                    return;
                }
            }
            _dtPayMode.Dispose();
        }
        //-------------end----------------------

        if (chkPost.Checked == true && ddlPaymentMode.SelectedItem.Text.ToUpper() == "CREDIT NOTE")
        {
            if (checkInvoiceForPending(strOtherAccountId, txtSInvNo.Text) == true)
            {
                DisplayMessage("You can not create Credit Note, because having some pending amount for invoice - " + txtSInvNo.Text + "");
                return;
            }
        }




        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            string strRetrunNo = string.Empty;
            string strReturnDate = string.Empty;



            strinvoiceId = Objarr[4].ToString();

            string TaxQuery = "Select * from Inv_TaxRefDetail where Ref_Type='SINV' and Ref_Id = '" + strinvoiceId + "' and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
            DataTable dtTaxDetails = da.return_DataTable(TaxQuery);

            if (editid.Value == "")
            {


                int b = ObjSalesReturnHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strinvoiceId, txtSreturnNo.Text, ObjSysParam.getDateForInput(txtsReturnDate.Text).ToString(), txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), ddlPaymentMode.SelectedValue, Objarr[0].ToString(), Objarr[3].ToString(), Objarr[4].ToString(), Objarr[2].ToString(), txtPOSNo.Text, txtRemark.Text, chkPost.Checked.ToString(), txtTotalQuantity.Text, SetDecimal(txtAmount.Text), txtTaxP.Text, Convert_Into_DF(txtTaxV.Text), txtDiscountP.Text, txtDiscountV.Text, SetDecimal(txtNetAmount.Text), Objarr[1].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                if (txtSreturnNo.Text == ViewState["DocNo"].ToString())
                {
                    if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is for OPC Location and Jaipur Location
                    {
                        string sql = "SELECT count(*) FROM Inv_SalesReturnHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and Return_No Like '%" + txtSreturnNo.Text + "%'";
                        int strInvoiceCount = Int32.Parse(da.get_SingleValue(sql, ref trns));

                        if (strInvoiceCount == 0)
                        {
                            strInvoiceCount = 1;
                            ObjSalesReturnHeader.Updatecode(b.ToString(), txtSreturnNo.Text + "1", ref trns);
                            txtSreturnNo.Text = txtSreturnNo.Text + "1";
                            strRetrunNo = txtSreturnNo.Text;
                        }
                        else
                        {
                            ObjSalesReturnHeader.Updatecode(b.ToString(), txtSreturnNo.Text + strInvoiceCount, ref trns);
                            txtSreturnNo.Text = txtSreturnNo.Text + "1";
                            strRetrunNo = txtSreturnNo.Text;
                        }
                    }
                    else
                    {
                        DataTable Dttemp = new DataTable();
                        DataTable dtCount = ObjSalesReturnHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                        if (dtCount.Rows.Count == 0)
                        {
                            ObjSalesReturnHeader.Updatecode(b.ToString(), txtSreturnNo.Text + "1", ref trns);
                            txtSreturnNo.Text = txtSreturnNo.Text + "1";
                            strRetrunNo = txtSreturnNo.Text;
                        }
                        else
                        {
                            DataTable dtCount1 = new DataView(dtCount, "Return_No='" + txtSreturnNo.Text + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
                            int NoRow = dtCount.Rows.Count;
                            if (dtCount1.Rows.Count > 0)
                            {

                                bool bCodeFlag = true;
                                while (bCodeFlag)
                                {
                                    NoRow += 1;
                                    DataTable dtTemp = new DataView(dtCount, "Return_No='" + txtSreturnNo.Text + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtTemp.Rows.Count == 0)
                                    {
                                        bCodeFlag = false;
                                    }
                                }
                            }

                            ObjSalesReturnHeader.Updatecode(b.ToString(), txtSreturnNo.Text + NoRow.ToString(), ref trns);
                            txtSreturnNo.Text = txtSreturnNo.Text + NoRow.ToString();
                            strRetrunNo = txtSreturnNo.Text;
                        }
                    }



                }
                editid.Value = b.ToString();

                //update retrun status in invoice table 

                objSInvHeader.UpdateReturnSInvHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Objarr[4].ToString(), strRetrunNo, ObjSysParam.getDateForInput(txtsReturnDate.Text).ToString(), txtRemark.Text, ref trns);
            }
            else
            {
                ObjSalesReturnHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtSreturnNo.Text, ObjSysParam.getDateForInput(txtsReturnDate.Text).ToString(), txtSInvNo.Text, ObjSysParam.getDateForInput(txtSInvDate.Text).ToString(), ddlPaymentMode.SelectedValue, Objarr[0].ToString(), Objarr[3].ToString(), Objarr[4].ToString(), Objarr[2].ToString(), txtPOSNo.Text, txtRemark.Text, chkPost.Checked.ToString(), txtTotalQuantity.Text, SetDecimal(txtAmount.Text), txtTaxP.Text, Convert_Into_DF(txtTaxV.Text), txtDiscountP.Text, txtDiscountV.Text, SetDecimal(txtNetAmount.Text), Objarr[1].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strRetrunNo = txtSreturnNo.Text; ;
            }

            strReturnDate = ObjSysParam.getDateForInput(txtsReturnDate.Text).ToString();


            //first delete record in return detail table on base of header id if exists 

            ObjSalesReturnDetail.DeleteRecord_By_ReturnNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, editid.Value, ref trns);


            objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SR", editid.Value, ref trns);

            Double TotalAmount = 0;
            string[] ProductDetail = null;
            double TaxAfterReturn = 0;
            List<string> newList = new List<string>();
            foreach (GridViewRow gvr in GvSalesInvoiceDetailDirect.Rows)
            {
                HiddenField hdnTransId = (HiddenField)gvr.FindControl("hdngvTransId");

                HiddenField hdnUnitId = (HiddenField)gvr.FindControl("hdngvUnitId");
                Label lblUnitPrice = (Label)gvr.FindControl("lblgvUnitPrice");
                HiddenField hdnProductId = (HiddenField)gvr.FindControl("hdngvProductId");
                TextBox txtgvReturnQty = (TextBox)gvr.FindControl("txtgvReturnQuantity");
                TextBox txtgvLineTotal = (TextBox)gvr.FindControl("txtgvTotal");
                Label lblgvQuantity = (Label)gvr.FindControl("lblgvQuantity");
                Label txtgvTaxV = (Label)gvr.FindControl("txtgvTaxV");
                if (txtgvLineTotal.Text == "")
                {
                    txtgvLineTotal.Text = Convert_Into_DF("0");
                }
                if (txtgvReturnQty.Text.Trim() == "")
                {
                    txtgvReturnQty.Text = "0";
                }

                double Totalreturnqty = Convert.ToDouble(txtgvReturnQty.Text) + Convert.ToDouble(((Label)gvr.FindControl("lblgvReturnQuantity")).Text);
                TotalAmount += Convert.ToDouble(txtgvReturnQty.Text) * Convert.ToDouble(lblUnitPrice.Text);

                //if (txtgvReturnQty.Text.Trim() != "0")
                //{

                if (hdnProductId.Value != "0" && hdnProductId.Value != "")
                {
                    string strItemType = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString();
                    string strstockmethod = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();

                    int Details_Id = ObjSalesReturnDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, strinvoiceId, ((Label)gvr.FindControl("lblgvsNo")).Text, Objarr[3].ToString(), ((HiddenField)gvr.FindControl("hdnSOId")).Value, hdnProductId.Value, hdnUnitId.Value, lblUnitPrice.Text, "0", ((Label)gvr.FindControl("lblgvOrderqty")).Text, ((Label)gvr.FindControl("lblgvQuantity")).Text, ((Label)gvr.FindControl("lblgvReturnQuantity")).Text, txtgvReturnQty.Text.Trim(), ((Label)gvr.FindControl("txtgvTaxP")).Text, Convert_Into_DF(((Label)gvr.FindControl("txtgvTaxV")).Text), ((Label)gvr.FindControl("txtgvDiscountP")).Text, ((Label)gvr.FindControl("txtgvDiscountV")).Text, ((Label)gvr.FindControl("lblsalesordreno")).Text, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    if (Convert.ToDouble(txtgvLineTotal.Text) != 0)
                        insertTaxEntry(dtTaxDetails, editid.Value, hdnProductId.Value, lblgvQuantity.Text, txtgvReturnQty.Text.Trim(), Session["LocCurrencyId"].ToString(), ref trns, Details_Id.ToString(), Convert_Into_DF(txtgvLineTotal.Text));

                    //code is modified by jitendra upadhyay on 09-08-2016
                    //code modifed for also insert row of non stockable item in ledger table for check complete cycle 

                    int LedgerId = 0;
                    if (chkPost.Checked == true)
                    {
                        objSInvDetail.UpdateReturnSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, hdnTransId.Value, strinvoiceId, hdnProductId.Value, Totalreturnqty.ToString(), chkPost.Checked.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
                        {
                            LedgerId = ObjProductLedger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", editid.Value, "0", hdnProductId.Value, hdnUnitId.Value, "I", "0", "0", txtgvReturnQty.Text, "0", "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtsReturnDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                    }


                    if (strItemType.Trim() == "S")
                    {

                        if (strstockmethod.Trim() == "SNO")
                        {
                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + hdnProductId.Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
                                if (dt.Rows.Count > 0)
                                {
                                    ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "SR", editid.Value, hdnProductId.Value, ((HiddenField)gvr.FindControl("hdnSOId")).Value, ref trns);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", editid.Value, hdnProductId.Value, hdnUnitId.Value, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", ((HiddenField)gvr.FindControl("hdnSOId")).Value, "", "", LedgerId.ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                            }
                        }
                        else if (strstockmethod == "FE" || strstockmethod == "LE" || strstockmethod == "FM" || strstockmethod == "LM")
                        {
                            ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "SR", editid.Value, hdnProductId.Value, ((HiddenField)gvr.FindControl("hdnSOId")).Value, ref trns);
                            UpdateRecordForStckableItem(hdnProductId.Value, objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString(), Convert.ToDouble(txtgvReturnQty.Text), editid.Value, hdnUnitId.Value, ((HiddenField)gvr.FindControl("hdnSOId")).Value, LedgerId.ToString(), ref trns);
                        }
                    }
                }

                //here we check in commision table by invoice wise and product wise that commison generated than enter reverse entry in return commison table
                //code created by jitendra upadhyay on 17-09-2016
                //code start

                if (chkPost.Checked)
                {

                    if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
                    {

                        DataTable dtCommission = objCommisisonDetail.GetCommisionDetail_By_InvoiceNo(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strinvoiceId, hdnProductId.Value, ref trns);

                        foreach (DataRow dr in dtCommission.Rows)
                        {
                            // double Exchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(Objarr[0].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString(), trns.Connection.ConnectionString ));

                            double Exchnagerate = Convert.ToDouble(GetExchageRate2(Objarr[0].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString(), ref trns));
                            double ProductReturmAmount = (Convert.ToDouble(Convert_Into_DF(txtgvLineTotal.Text)) - Convert.ToDouble(txtgvTaxV.Text)) * Exchnagerate;
                            double CommisionPercentage = Convert.ToDouble(dr["Commission_Percentage"].ToString());
                            double ReturnCommisionAmt = (ProductReturmAmount * CommisionPercentage) / 100;


                            objReturnCommission.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["Commission_Person"].ToString(), dr["Voucher_header_Id"].ToString(), dr["Detail_Voucher_Id"].ToString(), editid.Value, ReturnCommisionAmt.ToString(), false.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }



                    }
                }

                if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
                {
                    string StrDetail = hdnProductId.Value + "," + lblgvQuantity.Text + "," + txtgvReturnQty.Text;
                    newList.Add(StrDetail);
                }


                //code end

                //}
            }
            ProductDetail = newList.ToArray();
            //here we enter dat in payment transaction table for return amount

            //this code is created by jitendra upadhyay on 27-01-2016

            //cose start

            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = SetDecimal("0");
            }
            if (Convert.ToDouble(SetDecimal(txtNetAmount.Text)) > 0)
            {
                ObjPaymentTrans.DeleteByRefandRefNo(Session["CompId"].ToString().ToString(), "SR", editid.Value.ToString(), ref trns);

                ObjPaymentTrans.insert(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, "SR", editid.Value.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), SetDecimal(txtNetAmount.Text), Session["CurrencyId"].ToString(), "1", SetDecimal(txtNetAmount.Text), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            //cpde end


            //commente dby jitendra upadhyay on 30-08-2016

            //comment start

            if (chkPost.Checked == true)
            {
                //editid.Value = strinvoiceId;

                Session["CustomerId"] = Objarr[1].ToString();
                //Session["CurrencyId"] = Objarr[0].ToString();

                //Invoice Detail


                //For Payment Value
                string strPayValue = string.Empty;
                DataTable dtPay = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (dtPay.Rows.Count > 0)
                {
                    strPayValue = dtPay.Rows[0]["Field1"].ToString();
                }
                else
                {
                    strPayValue = "Cash";
                }
                string strNarration = string.Empty;
                strNarration = strRetrunNo + " Against invoice No " + txtSInvNo.Text;

                //For Voucher & Ageing(Due) Entries
                string strVoucherType = ddlPaymentMode.SelectedItem.Text.ToUpper() == "CREDIT NOTE" ? "CCN" : "SR";

                int _voucherId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), StrLocationId.ToString(), Session["DepartmentId"].ToString(), editid.Value, "SR", strRetrunNo, strReturnDate, strRetrunNo, strReturnDate, strVoucherType, "1/1/1800", "1/1/1800", "", "", Session["CurrencyId"].ToString(), "0.00", strNarration, false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = _voucherId.ToString();

                //-----If voucher type in CCN (Customer credit Note)------------------
                if (chkPost.Checked == true && ddlPaymentMode.SelectedItem.Text.ToUpper() == "CREDIT NOTE")
                {
                    string strCCNVoucherNo = string.Empty;
                    strCCNVoucherNo = get_CCN_No(ref trns);
                    objVoucherHeader.Updatecode(strVMaxId, strCCNVoucherNo, ref trns);
                }
                //DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                //if (dtVMaxId.Rows.Count > 0)
                //{
                //    strVMaxId = dtVMaxId.Rows[0][0].ToString();
                //}

                //----------------------------------------------------------------------

                string strCashAccount = string.Empty;
                string strSalesReturn = string.Empty;
                string strCostOfSales = string.Empty;
                string strReceiveVoucherAcc = string.Empty;
                string strInventory = string.Empty;

                DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString(), ref trns);
                DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCash.Rows.Count > 0)
                {
                    strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtSalesReturn = new DataView(dtAcParameter, "Param_Name='Sales Return'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtSalesReturn.Rows.Count > 0)
                {
                    strSalesReturn = dtSalesReturn.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtCostOfSales = new DataView(dtAcParameter, "Param_Name='Cost Of Sales'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCostOfSales.Rows.Count > 0)
                {
                    strCostOfSales = dtCostOfSales.Rows[0]["Param_Value"].ToString();
                }

                DataTable dtInventory = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtInventory.Rows.Count > 0)
                {
                    strInventory = dtInventory.Rows[0]["Param_Value"].ToString();
                }

                strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                //DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                //if (dtReceiveVoucher.Rows.Count > 0)
                //{
                //    strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
                //}

                double Exchange_Rate = 0;
                if (Hdn_Exchange_Rate.Value == "")
                    Hdn_Exchange_Rate.Value = "1";

                Exchange_Rate = Convert.ToDouble(Hdn_Exchange_Rate.Value);

                //strExchangeRate = Hdn_Exchange_Rate.Value;
                //string strForienAmount = string.Empty;
                //string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();

                double F_Net_Amount = 0;
                double L_Net_Amount = 0;
                double C_Net_Amount = 0;

                F_Net_Amount = Convert.ToDouble(SetDecimal((txtNetAmount.Text).ToString()));
                L_Net_Amount = Convert.ToDouble(Convert_Into_DF((F_Net_Amount * Exchange_Rate).ToString()));
                C_Net_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Net_Amount).ToString())))).Split('/')[0].ToString());

                int j = 0;

                string InvoiceQuery = "Select * from Inv_SalesInvoiceHeader where Invoice_No = '" + txtSInvNo.Text.Trim() + "'";
                DataTable dtInvoice = da.return_DataTable(InvoiceQuery, ref trns);
                string InvoiceNo = string.Empty;
                InvoiceNo = dtInvoice.Rows[0]["Trans_Id"].ToString();

                TaxAfterReturn = Convert.ToDouble(SaveTaxEntry(InvoiceNo, trns, ProductDetail, TaxAfterReturn, strVMaxId, Exchange_Rate.ToString(), TotalAmount.ToString(), true, j, strRetrunNo));

                double F_Net_Tax_Amount = 0;
                double L_Net_Tax_Amount = 0;
                double C_Net_Tax_Amount = 0;
                F_Net_Tax_Amount = Convert.ToDouble(Convert_Into_DF((TaxAfterReturn).ToString()));
                L_Net_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Net_Tax_Amount * Exchange_Rate).ToString()));
                C_Net_Tax_Amount = Convert.ToDouble((GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Net_Tax_Amount).ToString())))).Split('/')[0].ToString());


                if (strPayValue.Trim() == "Cash")
                {
                    //Credit Entry
                    string F_Net_Amount_Cr = F_Net_Amount.ToString();
                    string L_Net_Amount_Cr = L_Net_Amount.ToString();
                    string C_Net_Amount_Cr = C_Net_Amount.ToString();

                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strCashAccount, "0", editid.Value, "SR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Cr, strNarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), Exchange_Rate.ToString(), F_Net_Amount_Cr, "0.00", C_Net_Amount_Cr, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //Debit Entry

                    string F_Net_Amount_Dr = (F_Net_Amount - F_Net_Tax_Amount).ToString();
                    string L_Net_Amount_Dr = (L_Net_Amount - L_Net_Tax_Amount).ToString();
                    string C_Net_Amount_Dr = (C_Net_Amount - C_Net_Tax_Amount).ToString();

                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strSalesReturn, "0", editid.Value, "SR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Dr, "0.00", strNarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), Exchange_Rate.ToString(), F_Net_Amount_Dr, C_Net_Amount_Dr, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else if (strPayValue.Trim() == "Credit")
                {
                    //Credit Entry
                    string F_Net_Amount_Cr = F_Net_Amount.ToString();
                    string L_Net_Amount_Cr = L_Net_Amount.ToString();
                    string C_Net_Amount_Cr = C_Net_Amount.ToString();

                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, strOtherAccountId, editid.Value, "SR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Cr, strNarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), Exchange_Rate.ToString(), F_Net_Amount_Cr, "0.00", C_Net_Amount_Cr, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //Debit Entry
                    string F_Net_Amount_Dr = (F_Net_Amount - F_Net_Tax_Amount).ToString();
                    string L_Net_Amount_Dr = (L_Net_Amount - L_Net_Tax_Amount).ToString();
                    string C_Net_Amount_Dr = (C_Net_Amount - C_Net_Tax_Amount).ToString();

                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strSalesReturn, "0", editid.Value, "SR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Dr, "0.00", strNarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), Exchange_Rate.ToString(), F_Net_Amount_Dr, C_Net_Amount_Dr, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                SaveTaxEntry(InvoiceNo, trns, ProductDetail, F_Net_Tax_Amount, strVMaxId, Exchange_Rate.ToString(), TotalAmount.ToString(), false, j, strRetrunNo);


                //For Ageing Detail Entry


                string Asql = "select * from Ac_Ageing_Detail where other_account_no='" + strOtherAccountId + "' and AgeingType='RV' and Ref_Id='" + strinvoiceId + "' and Ref_Type='SINV'";
                DataTable dtAge = da.return_DataTable(Asql, ref trns);
                if (strPayValue.Trim() == "Credit")
                {
                    if (dtAge.Rows.Count > 0)
                    {
                        //for Credit Entry
                        string F_Net_Amount_Cr = F_Net_Amount.ToString();
                        string L_Net_Amount_Cr = L_Net_Amount.ToString();
                        string C_Net_Amount_Cr = C_Net_Amount.ToString();
                        // commented by neelkanth purohit - 13-07-18
                        //objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId, "SINV", strinvoiceId, dtAge.Rows[0]["Invoice_No"].ToString(), dtAge.Rows[0]["Invoice_Date"].ToString(), strReceiveVoucherAcc, Session["CustomerId"].ToString(), dtAge.Rows[0]["Invoice_Amount"].ToString(), L_Net_Amount_Cr, "0.00", "1/1/1800", "1/1/1800", "", "Sales Return Detail On Return No '" + strRetrunNo + "'", Session["EmpId"].ToString(), dtAge.Rows[0]["Currency_Id"].ToString(), dtAge.Rows[0]["Exchange_Rate"].ToString(), F_Net_Amount_Cr, "0.00", C_Net_Amount_Cr, Session["FinanceYearId"].ToString(), "RV", strVMaxId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //Entry for Cost of Sales.
                double CostofSales = 0;

                DataTable dtCOS = new DataTable();
                dtCOS = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ref trns, Session["FinanceYearId"].ToString());
                if (dtCOS.Rows.Count > 0)
                {
                    for (int D = 0; D < dtCOS.Rows.Count; D++)
                    {
                        string strReturnQty = dtCOS.Rows[D]["ReturnQty"].ToString();
                        if (strReturnQty == "")
                        {
                            strReturnQty = "0";
                        }
                        double temp = 0;
                        double.TryParse(dtCOS.Rows[D]["Field2"].ToString(), out temp);
                        string strCost = (Convert.ToDouble(strReturnQty) * temp).ToString();
                        CostofSales += Convert.ToDouble(strCost);
                    }
                }

                string strCExchnageRate = string.Empty;
                string strCCurrencyId = string.Empty;

                if (dtAge.Rows.Count > 0)
                {
                    strCExchnageRate = dtAge.Rows[0]["Exchange_Rate"].ToString();
                    strCCurrencyId = dtAge.Rows[0]["Currency_Id"].ToString();
                }
                else
                {
                    DataTable dtSINV = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strinvoiceId, ref trns);
                    if (dtSINV.Rows.Count > 0)
                    {
                        strCExchnageRate = dtSINV.Rows[0]["Field5"].ToString();
                        strCCurrencyId = dtSINV.Rows[0]["Currency_Id"].ToString();
                    }
                }
                bool IsCostingEntry = false;
                IsCostingEntry = Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsCostingEntry").Rows[0]["ParameterValue"].ToString());
                if (IsCostingEntry)
                {
                    if (CostofSales != 0)
                    {
                        string strCostFreign = (Convert.ToDouble(CostofSales.ToString()) * Convert.ToDouble(strCExchnageRate)).ToString();
                        //for Credit entry
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", editid.Value, "SINV", "1/1/1800", "1/1/1800", "", "0.00", CostofSales.ToString(), strNarration, "", Session["EmpId"].ToString(), strCCurrencyId, strCExchnageRate, strCostFreign, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //for Debit entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", editid.Value, "SINV", "1/1/1800", "1/1/1800", "", CostofSales.ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCCurrencyId, strCExchnageRate, strCostFreign, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        string strCostFreign = (Convert.ToDouble(CostofSales.ToString()) * Convert.ToDouble(strCExchnageRate)).ToString();
                        //for Credit entry
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", editid.Value, "SINV", "1/1/1800", "1/1/1800", "", "0.00", CostofSales.ToString(), strNarration, "", Session["EmpId"].ToString(), strCCurrencyId, strCExchnageRate, strCostFreign, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //for Debit entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString());
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", editid.Value, "SINV", "1/1/1800", "1/1/1800", "", CostofSales.ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCCurrencyId, strCExchnageRate, strCostFreign, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }

            //}
            if (chkPost.Checked)
            {
                DisplayMessage("Record Posted SuccessFully");
            }
            else
            {
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            trns.Dispose();
            con.Dispose();
            editid.Value = "";
            //FillGrid();
            FillInvoiceGrid();
            Reset();
            txtSreturnNo.Text = GetDocumentNumber();
            txtsReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

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
            chkPost.Checked = false;
            return;
        }
    }
    protected void insertTaxEntry(DataTable dtTaxDetails, string prReturnID, string product_id, string strInvoiceQty, string strReturnQty, string strCurrencyId, ref SqlTransaction trns, string Details_ID, string txtgvLineTotal) //to insert in tax detail table
    {
        double returnQty = 0;
        double.TryParse(strReturnQty, out returnQty);
        if (dtTaxDetails.Rows.Count > 0 && returnQty > 0)
        {
            //DataTable dtCountry = dt.DefaultView.ToTable("Countries", true, "Country");
            DataTable dttemp = dtTaxDetails.DefaultView.ToTable("Temp", true, "productId", "Tax_Value", "field1", "Tax_Id", "Tax_Per");

            DataRow[] drTax = dttemp.Select("productId = " + product_id);
            foreach (DataRow dr in drTax)
            {
                double actualInvoiceQty = 0;
                double.TryParse(strInvoiceQty, out actualInvoiceQty);
                string taxAmount = "0";
                string taxableAmount = "0";
                taxAmount = ((double.Parse(dr["Tax_Value"].ToString()) / actualInvoiceQty) * returnQty).ToString();
                taxableAmount = ((double.Parse(dr["field1"].ToString()) / actualInvoiceQty) * returnQty).ToString();
                taxAmount = GetCurrency(strCurrencyId, taxAmount).Split('/')[0].ToString();
                taxableAmount = GetCurrency(strCurrencyId, taxableAmount).Split('/')[0].ToString();
                if (Convert.ToDouble(txtgvLineTotal) != 0)
                    objTaxRefDetail.InsertRecord("SR", prReturnID, "0", "0", product_id, dr["Tax_Id"].ToString(), SetDecimal(dr["Tax_Per"].ToString()).ToString(), taxAmount.ToString(), false.ToString(), taxableAmount.ToString(), Details_ID.ToString(), "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }


    }

    public string GetExchageRate2(string strFromCurrency, string strToCurrency, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;


        if (ObjSysParam.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString() == strFromCurrency)
        {
            strExchangeRate = objCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString();
            //strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, trns.Connection.ConnectionString).Rows[0]["Currency_Value"].ToString();
        }
        else
        {
            strExchangeRate = ((1 / float.Parse(objCurrency.GetCurrencyMasterById(strFromCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(objCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();

        }
        return strExchangeRate;
    }
    protected string SaveTaxEntry(string InvoiceNo, SqlTransaction trns, string[] ProductDetail, double TaxAfterReturn, string strVMaxId, string strCusExchange, string TotalAmount, bool CalculatedOnly, int j, string strRetrunNo)
    {
        //Credit Entry for Tax
        //Added By KSR on 11-09-2017
        string TaxQuery = "Select * from Inv_TaxRefDetail where Ref_Type='SINV' and Ref_Id = " + InvoiceNo + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
        DataTable dtTaxDetails = da.return_DataTable(TaxQuery);
        if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
        {
            string TaxAccountDetails = "Select * from Sys_TaxMaster where IsActive = 'true'";
            DataTable dtTaxAccountDetails = da.return_DataTable(TaxAccountDetails);

            string TaxGrouping = "Select Tax_Id,Tax_Name,STM.Field3,SUM(CONVERT(decimal(18,2),Tax_value)) as TaxAmount from Inv_TaxRefDetail inner join Sys_TaxMaster STM on STM.Trans_Id = Tax_Id where Ref_Type='SINV' and Ref_Id = " + InvoiceNo + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 group by Tax_Id,Tax_Name,STM.Field3";
            DataTable TaxTableGrouping = da.return_DataTable(TaxGrouping, ref trns);

            string TaxAccountNo = string.Empty;
            string TaxIdInfo = string.Empty;
            string GroupTaxId = string.Empty;
            string GroupTaxAmount = string.Empty;
            string GroupTaxValue = string.Empty;
            string GroupTaxName = string.Empty;

            string strTaxPer = string.Empty;
            TaxAfterReturn = 0;
            bool IsSave = false;
            foreach (DataRow grouprow in TaxTableGrouping.Rows)
            {
                GroupTaxId = grouprow["Tax_Id"].ToString();
                GroupTaxAmount = grouprow["TaxAmount"].ToString();
                GroupTaxName = grouprow["Tax_Name"].ToString();
                TaxAccountNo = grouprow["Field3"].ToString();

                IsSave = false;
                if (!CalculatedOnly)
                    TaxAfterReturn = 0;
                foreach (DataRow row in dtTaxDetails.Rows)
                {
                    if (!TaxIdInfo.Contains(GroupTaxId))
                    {
                        DataView groupView = new DataView(dtTaxDetails);
                        groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
                        GroupTaxName = string.Empty;
                        GroupTaxValue = string.Empty;
                        foreach (DataRow newrow in groupView.ToTable().Rows)
                        {
                            if (String.IsNullOrEmpty(GroupTaxValue))
                                GroupTaxValue = newrow["Tax_Per"].ToString();
                            else if (strTaxPer != newrow["Tax_Per"].ToString())
                            {
                                GroupTaxValue = GroupTaxValue + "," + newrow["Tax_Per"].ToString();
                            }
                            strTaxPer = newrow["Tax_Per"].ToString();

                            if (ProductDetail.Length > 0)
                            {
                                foreach (string Pdetail in ProductDetail)
                                {
                                    string PD_Id = Pdetail.Split(',')[0];
                                    string PD_OrQty = Pdetail.Split(',')[1];
                                    string PD_ReQty = Pdetail.Split(',')[2];

                                    if (PD_Id == newrow["ProductId"].ToString() && GroupTaxId == newrow["Tax_Id"].ToString())
                                    {
                                        double tax_val = Convert.ToDouble(newrow["Tax_value"].ToString());
                                        double unit_tax_val = (tax_val / Convert.ToDouble(PD_OrQty));
                                        double new_tax_val = unit_tax_val * (Convert.ToDouble(PD_ReQty));
                                        //string FinalAmount = (Convert.ToDouble(GroupTaxAmount) - new_tax_val).ToString();
                                        string FinalAmount = new_tax_val.ToString();
                                        if (FinalAmount == "0")
                                            TaxAfterReturn += Convert.ToDouble(new_tax_val);
                                        else
                                            TaxAfterReturn += Convert.ToDouble(FinalAmount);

                                        GroupTaxAmount = FinalAmount;

                                        if (!CalculatedOnly)
                                            GroupTaxAmount = TaxAfterReturn.ToString();

                                        IsSave = true;
                                    }
                                }
                            }
                        }

                        if (CalculatedOnly == false && IsSave)
                        {
                            //objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), TaxAccountNo, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", "0.00", GroupTaxAmount, GroupTaxValue + "% " + GroupTaxName + "From PR On '" + txtReturnNo.Text + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strSupExchange, TotalAmount.ToString(), "0.00", GroupTaxAmount, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            //Debit Entry
                            // objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), GetLocationCode(Session["LocId"].ToString()), strVMaxId, (j++).ToString(), TaxAccountNo, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", GroupTaxAmount, "0.00", GroupTaxValue + "% " + GroupTaxName + "From PR On '" + txtReturnNo.Text + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strSupExchange, TotalAmount.ToString(), GroupTaxAmount, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), TaxAccountNo, "0", editid.Value, "SR", "1/1/1800", "1/1/1800", "", Convert_Into_DF(GroupTaxAmount).ToString(), "0.00", GroupTaxValue + "% " + "From SR On '" + strRetrunNo + "", "", Session["EmpId"].ToString(), Session["LocCurrencyId"].ToString(), strCusExchange, GroupTaxAmount, GroupTaxAmount, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        if (String.IsNullOrEmpty(TaxIdInfo))
                            TaxIdInfo = GroupTaxId;
                        else
                            TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
                        break;
                    }
                }
            }
        }
        return TaxAfterReturn.ToString();
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
    #endregion
    #region User defined Function
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesreturnCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
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
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            strWhereClause += " and Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            strWhereClause += "  and Post='False'";
        }

        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = ObjSalesReturnHeader.getReturnList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesReturn.Attributes["CurrentSortField"], GvSalesReturn.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvSalesReturn, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            else
            {
                GvSalesReturn.DataSource = null;
                GvSalesReturn.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }

            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }

    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();

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
        txtSInvNo.Text = "";
        txtSInvDate.Text = "";
        trTransfer.Visible = false;
        txtSalesOrderNo.Text = "";
        txtSalesOrderDate.Text = "";
        txtCustomerName.Text = "";
        ddlPaymentMode.SelectedIndex = 0;
        txtCurrency.Text = "";
        txtSalesPerson.Text = "";
        txtPOSNo.Text = "";
        txtAccountNo.Text = "";
        txtInvoiceCosting.Text = "";
        txtShift.Text = "";
        txtTender.Text = "";
        txtRemark.Text = "";
        GvSalesInvoiceDetailDirect.DataSource = null;
        GvSalesInvoiceDetailDirect.DataBind();
        FillGrid();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        ViewState["dtSerial"] = null;
        ViewState["Tempdt"] = null;
        ViewState["dtFinal"] = null;
        chkPost.Checked = false;
        txtsReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtSreturnNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtSreturnNo.Text;
        txtAmount.Text = SetDecimal("0");
        txtTotalQuantity.Text = "0";
        txtDiscountP.Text = "0";
        txtTaxP.Text = "0";
        txtDiscountV.Text = "0";
        txtTaxV.Text = Convert_Into_DF("0");
        txtNetAmount.Text = SetDecimal("0");
        btnSInvSave.Enabled = true;
        BtnReset.Visible = true;
        btnPost.Enabled = true;
        BtnReset.Enabled = true;
    }
    #endregion
    void TaxandDiscountParameter()
    {
        GvSalesInvoiceDetailDirect.Columns[16].Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        GvSalesInvoiceDetailDirect.Columns[17].Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        trTax.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        GvSalesInvoiceDetailDirect.Columns[14].Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        GvSalesInvoiceDetailDirect.Columns[15].Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        trDiscount.Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        using (DataTable Dt = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Sales Invoice"))
        {
            if (Dt.Rows.Count > 0)
            {
                Session["AccountId"] = Dt.Rows[0]["Param_Value"].ToString();
            }
        }
    }
    #region Add Request Section
    public string GetSalesOrder(string TransType)
    {
        return TransType == "S" ? "Sales Order(Search From S)" : "";
    }
    protected string GetPaymentModeName(string strPaymentModeId)
    {
        string strPaymentMode = string.Empty;
        if (strPaymentModeId != "0" && strPaymentModeId != "")
        {
            using (DataTable dtPMName = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), strPaymentModeId, Session["BrandId"].ToString(), Session["LocId"].ToString()))
            {
                if (dtPMName.Rows.Count > 0)
                {
                    strPaymentMode = dtPMName.Rows[0]["Pay_Mod_Name"].ToString();
                }
            }
        }
        return strPaymentMode;
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
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
    protected void GvSalesOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();


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
            gvProduct.Controls[0].Controls.Add(row);
        }
    }
    protected void GvSalesInvoiceDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Quantity;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();

            gvProduct.Controls[0].Controls.Add(row);
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_SalesInvoiceHeader objSalesInvoice = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            using (DataTable dtSalesInvoice = objSalesInvoice.FilterByInvoiceNo(HttpContext.Current.Session["LocId"].ToString(), prefixText))
            {
                string[] txt = new string[dtSalesInvoice.Rows.Count];
                if (dtSalesInvoice.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSalesInvoice.Rows.Count; i++)
                    {
                        txt[i] = dtSalesInvoice.Rows[i]["Invoice_No"].ToString();
                    }
                }
                return txt;
            }
        }
        catch
        {
            return null;
        }
    }
    #endregion
    protected void txtSInvNo_TextChanged(object sender, EventArgs e)
    {
        if (txtSInvNo.Text != "")
        {
            using (DataTable dtInvNo = objSInvHeader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, txtSInvNo.Text))
            {
                if (dtInvNo.Rows.Count > 0)
                {
                    if (!Convert.ToBoolean(dtInvNo.Rows[0]["Post"].ToString()))
                    {
                        DisplayMessage("First Of All Post This Invoice");
                        txtSInvNo.Text = "";
                        txtSInvNo.Focus();
                        return;
                    }
                    //updated by jitendra upadhyay on 2-Dec-2013 for set the validation that after post the record will not updated
                    using (DataTable dtSalesInvoiceDetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, dtInvNo.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString()))
                    {
                        if (dtSalesInvoiceDetail.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dtSalesInvoiceDetail.Rows[0]["Post"].ToString()))
                            {
                                DisplayMessage("Record has Posted,Can not be update");
                                txtSInvNo.Text = "";
                                txtSInvNo.Focus();
                                return;
                            }
                            else
                            {
                                chkPost.Checked = false;
                            }
                        }
                    }
                    editid.Value = dtInvNo.Rows[0]["Trans_Id"].ToString();
                    DataTable dtSerial = new DataTable();
                    dtSerial.Columns.Add("Product_Id");
                    dtSerial.Columns.Add("SerialNo");
                    dtSerial.Columns.Add("SOrderNo");
                    dtSerial.Columns.Add("BarcodeNo");
                    dtSerial.Columns.Add("BatchNo");
                    dtSerial.Columns.Add("LotNo");
                    dtSerial.Columns.Add("ExpiryDate");
                    dtSerial.Columns.Add("ManufacturerDate");
                    using (DataTable dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, "SR", editid.Value))
                    {
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
                    txtSInvDate.Text = GetDate(dtInvNo.Rows[0]["Invoice_Date"].ToString());
                    txtCustomerName.Text = Set_CustomerMaster.GetCustomerName(dtInvNo.Rows[0]["Supplier_Id"].ToString(), Session["DBConnection"].ToString());
                    Session["CustomerId"] = dtInvNo.Rows[0]["Supplier_Id"].ToString();
                    try
                    {
                        ddlPaymentMode.SelectedValue = dtInvNo.Rows[0]["PaymentModeId"].ToString();
                    }
                    catch
                    {
                    }
                    txtCurrency.Text = CurrencyMaster.GetCurrencyNameByCurrencyId(dtInvNo.Rows[0]["Currency_Id"].ToString(), Session["DBConnection"].ToString());
                    txtSalesPerson.Text = Common.GetEmployeeName(dtInvNo.Rows[0]["SalesPerson_Id"].ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString());
                    txtPOSNo.Text = dtInvNo.Rows[0]["PosNo"].ToString();
                    txtAccountNo.Text = dtInvNo.Rows[0]["Account_No"].ToString();
                    txtInvoiceCosting.Text = dtInvNo.Rows[0]["Invoice_Costing"].ToString();
                    txtShift.Text = dtInvNo.Rows[0]["Shift"].ToString();
                    txtTender.Text = dtInvNo.Rows[0]["Tender"].ToString();
                    //txtRemark.Text = dtInvNo.Rows[0]["Remark"].ToString();
                    txtTaxP.Text = SetDecimal(dtInvNo.Rows[0]["NetTaxP"].ToString());
                    txtDiscountP.Text = SetDecimal(dtInvNo.Rows[0]["NetDiscountP"].ToString());
                    string strTransNo = string.Empty;
                    string strTransType = dtInvNo.Rows[0]["SIFromTransType"].ToString();
                    using (DataTable Dt_Record = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), editid.Value, Session["FinanceYearId"].ToString()))
                    {
                        Hdn_Exchange_Rate.Value = Dt_Record.Rows[0]["Exchange_Rate"].ToString();
                        GvSalesInvoiceDetailDirect.DataSource = Dt_Record;
                        GvSalesInvoiceDetailDirect.DataBind();
                    }
                    if (dtInvNo.Rows[0]["ReturnNo"].ToString().Trim() != "")
                    {
                        txtSreturnNo.Text = dtInvNo.Rows[0]["ReturnNo"].ToString();
                        txtsReturnDate.Text = GetDate(dtInvNo.Rows[0]["Field7"].ToString());
                        EditGridCalculation();
                    }
                    else
                    {
                        GridCalculation();
                    }
                }
                else
                {
                    txtSInvNo.Text = "";
                    DisplayMessage("Enter Invoice No Is Invalid, Choose In Suggestion Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
                }
            }
        }
        else
        {
            DisplayMessage("Enter Sales Invoice No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
        }
    }
    protected void txtgvReturnQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;


        if (((TextBox)gvRow.FindControl("txtgvReturnQuantity")).Text == "")
        {
            ((TextBox)gvRow.FindControl("txtgvReturnQuantity")).Text = "0";
        }
        if (((Label)gvRow.FindControl("lblgvRemaningQuantity")).Text == "")
        {
            ((Label)gvRow.FindControl("lblgvRemaningQuantity")).Text = "0";
        }

        if (Convert.ToDouble(((TextBox)gvRow.FindControl("txtgvReturnQuantity")).Text) > Convert.ToDouble(((Label)gvRow.FindControl("lblgvRemaningQuantity")).Text))
        {
            DisplayMessage("Return Quantity should be equal or less than to Remain quantity");
            ((TextBox)gvRow.FindControl("txtgvReturnQuantity")).Text = "0";
            return;
        }

        TextBox txtgvReturnQuantity = (TextBox)gvRow.FindControl("txtgvReturnQuantity");
        Label txtgvSalesQuantity = (Label)gvRow.FindControl("lblgvQuantity");
        Label lblgvUnitPrice = (Label)gvRow.FindControl("lblgvUnitPrice");
        Label txtgvTaxV = (Label)gvRow.FindControl("txtgvTaxV");
        Label txtgvDiscountV = (Label)gvRow.FindControl("txtgvDiscountV");
        Label txtgvTaxP = (Label)gvRow.FindControl("txtgvTaxP");
        Label txtgvDiscountP = (Label)gvRow.FindControl("txtgvDiscountP");
        Label lblgvQuantityPrice = (Label)gvRow.FindControl("lblgvQuantityPrice");
        Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
        Label lblTransId = (Label)gvRow.FindControl("lblTransId");
        Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
        HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
        Label lblgvProductDescription = (Label)gvRow.FindControl("lblgvProductDescription");
        HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
        Label lblgvUnit = (Label)gvRow.FindControl("lblgvUnit");

        Label lblgvOrderqty = (Label)gvRow.FindControl("lblgvOrderqty");

        Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
        TextBox txtgvTotal = (TextBox)gvRow.FindControl("txtgvTotal");
        Label lblgvRemaningQuantity = (Label)gvRow.FindControl("lblgvRemaningQuantity");

        if (txtgvReturnQuantity.Text == "")
        {
            txtgvReturnQuantity.Text = "0";
        }

        string[] strtotal = Common.TaxDiscountCaluculation(lblgvUnitPrice.Text, txtgvReturnQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), false, Session["DBConnection"].ToString());

        lblgvQuantityPrice.Text = strtotal[0].ToString();
        txtgvDiscountP.Text = strtotal[1].ToString();
        double Discnt = (Convert.ToDouble(txtgvReturnQuantity.Text) * Convert.ToDouble(strtotal[2]));
        txtgvDiscountV.Text = Discnt.ToString();
        //txtgvTaxP.Text = strtotal[3].ToString();
        //txtgvTaxV.Text = strtotal[4].ToString();
        txtgvTaxV.Text = Convert_Into_DF(((Convert.ToDouble(lblgvQuantityPrice.Text) - Convert.ToDouble(Discnt)) * Convert.ToDouble(txtgvTaxP.Text) / 100).ToString());
        txtgvTotal.Text = Convert_Into_DF(strtotal[5].ToString());
        HeadearCalculateGrid();
    }
    public string GetTotalTaxByProductId(string ProductId)
    {
        string TotalTax = "0";
        string InvoiceNo = string.Empty;

        string InvoiceQuery = "Select Trans_Id from Inv_SalesInvoiceHeader where Invoice_No = '" + txtSInvNo.Text.Trim() + "'";
        using (DataTable dtInvoice = da.return_DataTable(InvoiceQuery))
        {
            Session["Sales_InvoiceNo"] = dtInvoice.Rows[0]["Trans_Id"].ToString();
            InvoiceNo = dtInvoice.Rows[0]["Trans_Id"].ToString();
        }

        string ProductTaxQuery = @"Select SUM(CONVERT(decimal(18,2),Tax_Per)) from Inv_TaxRefDetail where Ref_Id = " + InvoiceNo + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 and Ref_Type = 'SINV' and ProductId = " + ProductId + "";
        using (DataTable Pdt = da.return_DataTable(ProductTaxQuery))
        {
            if (Pdt.Rows.Count > 0)
                TotalTax = Pdt.Rows[0][0].ToString();
        }

        return TotalTax;
    }
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        Session["SENID"] = "lnkViewDetail";
        btnEdit_Command(sender, e);
    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Invoice_Date" || ddlFieldName.SelectedItem.Value == "Return_Date")
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
    #endregion
    #region Post
    protected void btnPostSave_Click(object sender, EventArgs e)
    {
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            chkPost.Checked = false;
            return;
        }
        chkPost.Checked = true;
        btnSInvSave_Click(sender, e);

    }
    #endregion
    #region StockableandNonStcokableconcept

    protected void GvSalesInvoiceDetailDirect_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ((HiddenField)e.Row.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
            {
                //((TextBox)e.Row.FindControl("txtgvReturnQuantity")).Enabled = false;

                ((LinkButton)e.Row.FindControl("lnkAddSerialDirect")).Visible = true;

            }
            else
            {
                //((TextBox)e.Row.FindControl("txtgvReturnQuantity")).Enabled = true;

                ((LinkButton)e.Row.FindControl("lnkAddSerialDirect")).Visible = false;


            }

        }
    }
    #endregion
    #region stockable with serial number
    protected void lnkAddSerialDirect_Command(object sender, EventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";

        Session["RdoType"] = "SerialNo";

        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;

        //    ViewState["RowIndex"] = Row.RowIndex;
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["SOrderId"] = ((HiddenField)Row.FindControl("hdnSOId")).Value;
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


            dt = new DataView(dt, "Product_Id='" + ViewState["PID"].ToString() + "' and SOrderNo='" + ViewState["SOrderId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            ViewState["dtSerial"] = dt;
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        }

        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        DataTable dtSerialIssue = new DataTable();
        dtSerialIssue.Columns.Add(new DataColumn("ProductId"));
        dtSerialIssue.Columns.Add(new DataColumn("Serial"));
        dtSerialIssue.Columns.Add(new DataColumn("Status"));


        ArrayList objarr = (ArrayList)Session["InvInformation"];
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
        string alreadyReturn = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();


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
                        string[] result = isSerialNumberValid_SalesReturn(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber, objarr[4].ToString(), ViewState["SOrderId"].ToString());
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
                        else if (result[0].ToString() == "NOT VALID")
                        {
                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "ALREADY IN";

                            dtSerialIssue.Rows.Add(rowSerialIssue);


                            alreadyReturn += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString().ToUpper() == "NOT EXISTS")
                        {
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

                        string[] result = isSerialNumberValid_SalesReturn(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber, objarr[4].ToString(), ViewState["SOrderId"].ToString());
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
                        else if (result[0].ToString() == "NOT VALID")
                        {

                            DataRow rowSerialIssue = dtSerialIssue.NewRow();
                            rowSerialIssue[0] = ViewState["PID"].ToString();
                            rowSerialIssue[1] = txt[i].ToString();
                            rowSerialIssue[2] = "ALREADY IN";

                            dtSerialIssue.Rows.Add(rowSerialIssue);


                            alreadyReturn += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString().ToUpper() == "NOT EXISTS")
                        {
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
            if (ViewState["dtSerial"] != null)
            {

                dt = (DataTable)ViewState["dtSerial"];
            }
        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || serialNoExists != "" || alreadyReturn != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Out from the stock=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {
                Message += "Following serial number not exists in stock=" + serialNoExists;
            }
            if (alreadyReturn != "")
            {
                Message += "Following serial number Not valid for return in stock=" + alreadyReturn;
            }
            DisplayMessage(Message);
        }
        //here we check that sales quantity should be less than system quantity
        //this validation is add by jitendra upadhyay on 22-05-2015
        //code start

        if (((Label)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvRemaningQuantity")).Text == "")
        {
            ((Label)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvRemaningQuantity")).Text = "0";
        }

        if (Convert.ToDouble(((Label)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("lblgvRemaningQuantity")).Text) < Convert.ToDouble(dt.Rows.Count.ToString()))
        {

            DisplayMessage("Return Quantity Should be less than or equal to the Remaining quantity");
            return;
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
            if (ViewState["SOrderId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
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
        foreach (GridViewRow gvRow in GvSalesInvoiceDetailDirect.Rows)
        {
            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                TextBox txtGvReturnQuantity = (TextBox)gvRow.FindControl("txtgvReturnQuantity");
                txtGvReturnQuantity.Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
                //((TextBox)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("txtgvReturnQuantity")).Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
                //txtgvReturnQuantity_TextChanged(((TextBox)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("txtgvReturnQuantity")), null);
                txtgvReturnQuantity_TextChanged(gvRow.FindControl("txtgvReturnQuantity"), null);
            }
        }
        Update_New.Update();
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

    protected void btnNotFound_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["dtSerialIssue"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", ViewState["SIno"].ToString(), ViewState["PID"].ToString(), "1", "O", "1", "0", "0", DateTime.Now.ToString(), dt.Rows[i][1].ToString(), DateTime.Now.ToString(), "0", "0", "0", "0", "", "", "", "", "", "true", DateTime.Now.ToString(), "true", "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());

            }
            ViewState["dtSerialIssue"] = null;
            btnNotFound.Visible = false;
        }
        catch
        {

        }
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
            if (ViewState["SOrderId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "' and  SOrderNo<>'" + ViewState["SOrderId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                int SOId = Convert.ToInt32(ViewState["SOrderId"].ToString());
            }
            ViewState["dtFinal"] = Dtfinal;
        }

        ((TextBox)GvSalesInvoiceDetailDirect.Rows[(int)ViewState["RowIndex"]].FindControl("txtgvReturnQuantity")).Text = "0";

        txtSerialNo.Focus();
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
        DataTable dtstock = ObjStockBatchMaster.GetTempSerial(ViewState["PID"].ToString(), "SR", hdnOrderId.Value, hdnMerchant.Value);

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
        txtSerialNo.Text = "";
        ArrayList objarr = (ArrayList)Session["InvInformation"];
        DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ViewState["PID"].ToString());
        if (ViewState["SOrderId"].ToString() != "0")
        {
            if (ObjSalesOrderHeader.GetSOHeaderAllByTransId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ViewState["SOrderId"].ToString()).Rows[0]["IsdeliveryVoucher"].ToString() == "True")
            {
                DataTable dtDeliveryVoucher = objDeliveryVoucherheader.GetAllRecord_By_SalesOrderId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ViewState["SOrderId"].ToString());
                string deliveryvoucherId = string.Empty;
                foreach (DataRow dr in dtDeliveryVoucher.Rows)
                {
                    if (deliveryvoucherId == "")
                    {
                        deliveryvoucherId = dr["Trans_Id"].ToString();
                    }
                    else
                    {
                        deliveryvoucherId = deliveryvoucherId + "," + dr["Trans_Id"].ToString();
                    }
                }
                if (deliveryvoucherId != "")
                {
                    try
                    {
                        dtserial = new DataView(dtserial, "TransType='DV' and TransTypeId in (" + deliveryvoucherId + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                }
                dtDeliveryVoucher = null;
            }
            else
            {
                try
                {
                    dtserial = new DataView(dtserial, "TransType='SI' and TransTypeId=" + objarr[4].ToString() + " and Field1='" + ViewState["SOrderId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
            }
        }
        else
        {
            try
            {
                dtserial = new DataView(dtserial, "TransType='SI' and TransTypeId=" + objarr[4].ToString().ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        int counter = 0;
        for (int i = 0; i < dtserial.Rows.Count; i++)
        {
            using (DataTable dt = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ViewState["PID"].ToString(), dtserial.Rows[i]["SerialNo"].ToString()))
            {
                if (dt.Rows[0]["InOut"].ToString() == "O")
                {
                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = dtserial.Rows[i]["SerialNo"].ToString();
                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + dtserial.Rows[i]["SerialNo"].ToString();
                    }
                    counter++;
                }
            }
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }
        dtserial = null;

    }
    public static string[] isSerialNumberValid_SalesReturn(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber, string InvoiceId, string SoId)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesOrderHeader ObjSalesOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesDeliveryVoucher_Header objDeliveryVoucherheader = new Inv_SalesDeliveryVoucher_Header(HttpContext.Current.Session["DBConnection"].ToString());

        string[] Result = new string[5];
        int counter = 0;

        foreach (GridViewRow gvrow in gvSerialNumber.Rows)
        {
            if (((Label)gvrow.FindControl("lblsrno")).Text.Trim() == serialNumber)
            {
                counter = 1;
                break;
            }
        }
        if (counter == 0)
        {
            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);



            if (dtserial.Rows.Count > 0)
            {
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "O")
                {
                    if (SoId.Trim() != "0")
                    {
                        if (ObjSalesOrderHeader.GetSOHeaderAllByTransId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), SoId).Rows[0]["IsdeliveryVoucher"].ToString() == "True")
                        {
                            DataTable dtDeliveryVoucher = objDeliveryVoucherheader.GetAllRecord_By_SalesOrderId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), SoId);
                            string deliveryvoucherId = string.Empty;
                            foreach (DataRow dr in dtDeliveryVoucher.Rows)
                            {
                                if (deliveryvoucherId == "")
                                {
                                    deliveryvoucherId = dr["Trans_Id"].ToString();
                                }
                                else
                                {
                                    deliveryvoucherId = deliveryvoucherId + "," + dr["Trans_Id"].ToString();
                                }
                            }
                            if (deliveryvoucherId != "")
                            {
                                try
                                {
                                    dtserial = new DataView(dtserial, "TransType='DV' and TransTypeId in (" + deliveryvoucherId + ")", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                catch
                                {

                                }
                            }
                            dtDeliveryVoucher = null;
                        }
                        else
                        {
                            try
                            {
                                //and Field1='" + SoId + "'
                                dtserial = new DataView(dtserial, "TransType='SI' and TransTypeId=" + InvoiceId + " ", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            dtserial = new DataView(dtserial, "TransType='SI' and TransTypeId=" + InvoiceId + "", "", DataViewRowState.CurrentRows).ToTable();
                            //dtserial = new DataView(dtserial, "TransType='SI' and TransTypeId=" + InvoiceId + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }
                }
            }

            if (dtserial.Rows.Count == 0)
            {
                //if we not found in database with thsi product id that we are allow this serial number
                Result[0] = "NOT EXISTS";
            }
            else
            {
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "I")
                {
                    Result[0] = "NOT VALID";
                }
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "O")
                {

                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
            }

            dtserial = null;
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
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
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='O'", "ExpiryDate asc", DataViewRowState.CurrentRows).ToTable();

        }
        //for lifo expiry date
        else if (ItemType == "LE")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='O'", "ExpiryDate desc", DataViewRowState.CurrentRows).ToTable();

        }
        //for fifo manufacturing date
        else if (ItemType == "FM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='O'", "ManufacturerDate asc", DataViewRowState.CurrentRows).ToTable();
        }
        //for lifo manufacturing date
        else
            if (ItemType == "LM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='O'", "ManufacturerDate desc", DataViewRowState.CurrentRows).ToTable();

        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Currencyquantity = Convert.ToDouble(dt.Rows[i]["Quantity"].ToString());
            if (Quantity == 0)
            {
                break;
            }
            string sql = "select SUM(quantity) from Inv_StockBatchMaster where Field3='" + dt.Rows[i]["Trans_Id"].ToString() + "' and   ProductId=" + ProductId + " and InOut='I'";
            DataTable dtstock = da.return_DataTable(sql, ref trns);
            if (dtstock.Rows.Count > 0)
            {
                try
                {
                    if (Currencyquantity == Convert.ToDouble(dtstock.Rows[0][0].ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        double Remqty = 0;
                        Remqty = Currencyquantity - Convert.ToDouble(dtstock.Rows[0][0].ToString());
                        if (Remqty > Quantity)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                            Quantity = 0;
                        }
                        else
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                            Quantity = Quantity - Remqty;
                        }
                    }
                }
                catch
                {
                    if (Currencyquantity > Quantity)
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        Quantity = 0;
                    }
                    else
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        Quantity = Quantity - Currencyquantity;
                    }
                }
            }
            else
            {
                if (Currencyquantity > Quantity)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = 0;
                }
                else
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), "SR", InvoiceId, ProductId, UnitId, "I", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = Quantity - Currencyquantity;

                }
            }
            dtstock = null;
        }
        dt = null;

    }

    #endregion
    #region Fillpaymentmode
    private void FillPaymentMode()
    {
        DataTable dsPaymentMode = null;
        dsPaymentMode = objPaymentMode.GetPaymentModeMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            dsPaymentMode = new DataView(dsPaymentMode, "", "Pay_Mode_Id asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        if (dsPaymentMode.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlPaymentMode, dsPaymentMode, "Pay_Mod_Name", "Pay_Mode_Id");

            ddlPaymentMode.SelectedIndex = 1;
        }
        else
        {
            ddlPaymentMode.Items.Insert(0, "--Select--");
            ddlPaymentMode.SelectedIndex = 0;
        }
        dsPaymentMode = null;
    }
    #endregion
    #region Currency
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString());
    }
    #endregion
    #region InvoiceList
    protected void InvoicePage_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesInvoiceCurrentPageIndex.Value = pageIndex.ToString();
        FillInvoiceGrid(pageIndex);
    }
    private void FillInvoiceGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlInvoiceOption.SelectedIndex != 0 && txtInvoiceValue.Text != string.Empty)
        {
            if (ddlInvoiceOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + "='" + txtInvoiceValue.Text.Trim() + "'";
            }
            else if (ddlInvoiceOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + " like '%" + txtInvoiceValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + " Like '" + txtInvoiceValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and isActive='true' and Post='True'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        int totalRows = 0;
        using (DataTable dt = objSInvHeader.getInvoiceListForSalesReturn((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesInvoice.Attributes["CurrentSortField"], GvSalesInvoice.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblInvoiceTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            else
            {
                GvSalesInvoice.DataSource = null;
                GvSalesInvoice.DataBind();
                lblInvoiceTotalRecord.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager_Invoice, totalRows, currentPageIndex);
        }
    }
    protected void SetCustomerTextBox(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlInvoiceFieldName.Text == "Supplier_Id")
        {
            txtInvoiceValue.Visible = false;
            txtCustValue.Visible = true;
            txtInvoiceValueDate.Visible = false;
        }
        else if (ddlInvoiceFieldName.Text == "Invoice_Date")
        {
            txtInvoiceValue.Visible = false;
            txtCustValue.Visible = false;
            txtInvoiceValueDate.Visible = true;
        }
        else
        {
            txtInvoiceValue.Visible = true;
            txtCustValue.Visible = false;
            txtInvoiceValueDate.Visible = false;
        }
        txtInvoiceValue.Text = "";
        txtCustValue.Text = "";
        txtInvoiceValueDate.Text = "";
    }
    protected void btnInvoicebind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlInvoiceFieldName.SelectedItem.Value == "Invoice_Date")
        {
            if (txtInvoiceValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtInvoiceValueDate.Text);
                    txtInvoiceValue.Text = Convert.ToDateTime(txtInvoiceValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtInvoiceValueDate.Text = "";
                    txtInvoiceValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtInvoiceValueDate.Focus();
                return;
            }
        }
        FillInvoiceGrid();

    }
    protected void btnInvoiceRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ddlInvoiceFieldName.SelectedIndex = 1;
        ddlInvoiceOption.SelectedIndex = 2;
        txtInvoiceValue.Text = "";
        txtCustValue.Text = "";
        txtCustValue.Visible = false;
        txtInvoiceValue.Visible = true;
        txtInvoiceValueDate.Visible = false;
        txtInvoiceValueDate.Text = "";
        FillInvoiceGrid();
    }
    protected void GvSalesInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvSalesInvoice.Attributes["CurrentSortField"] != null &&
            GvSalesInvoice.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesInvoice.Attributes["CurrentSortField"])
            {
                if (GvSalesInvoice.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesInvoice.Attributes["CurrentSortField"] = sortField;
        GvSalesInvoice.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillInvoiceGrid(Int32.Parse(hdnGvSalesInvoiceCurrentPageIndex.Value));
    }
    public string SetDecimal(string amount)
    {
        return SystemParameter.GetAmountWithDecimal(amount, Session["LoginLocDecimalCount"].ToString());

    }
    protected void btnInvoiceList_Click(object sender, EventArgs e)
    {
        FillInvoiceGrid();
    }
    protected void btnInvoice_Command(object sender, CommandEventArgs e)
    {//here we checking that previous return is posted or not again this invoice if exists 

        if (ObjSalesReturnHeader.GetAllRecord_By_InvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, e.CommandArgument.ToString()).Rows.Count > 0)
        {
            DisplayMessage("Previous return is not posted so you can not create return voucher");
            return;
        }
        editid.Value = "";
        ViewState["SIno"] = e.CommandArgument.ToString();
        GetinvoiceRecord(e.CommandArgument.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        Update_New.Update();
        btnSInvSave.Enabled = true;
        BtnReset.Visible = true;
        btnPost.Enabled = true;
    }
    public void GetinvoiceRecord(string strInvoiceId)
    {
        string objSenderID = string.Empty;


        using (DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, strInvoiceId))
        {

            Lbl_Tab_New.Text = Resources.Attendance.New;

            if (dtInvEdit.Rows.Count > 0)
            {
                txtSInvNo.Text = dtInvEdit.Rows[0]["Invoice_No"].ToString();
                txtSInvDate.Text = GetDate(dtInvEdit.Rows[0]["Invoice_Date"].ToString());
                txtCustomerName.Text = Set_CustomerMaster.GetCustomerName(dtInvEdit.Rows[0]["Supplier_Id"].ToString(), Session["DBConnection"].ToString());

                try
                {
                    ddlPaymentMode.SelectedValue = dtInvEdit.Rows[0]["PaymentModeId"].ToString();
                }
                catch
                {
                    ddlPaymentMode.SelectedIndex = 0;
                }
                //txtPaymentMode.Text = GetPaymentModeName(dtInvEdit.Rows[0]["PaymentModeId"].ToString());
                txtCurrency.Text = CurrencyMaster.GetCurrencyNameByCurrencyId(dtInvEdit.Rows[0]["Currency_Id"].ToString(), Session["DBConnection"].ToString());

                ArrayList Objarr = new ArrayList();

                Objarr.Add(dtInvEdit.Rows[0]["Currency_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["Supplier_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["SalesPerson_Id"].ToString());
                Objarr.Add(dtInvEdit.Rows[0]["SIFromTransType"].ToString());
                Objarr.Add(strInvoiceId);
                Objarr.Add(dtInvEdit.Rows[0]["Invoice_Merchant_Id"].ToString());
                Session["CurrencyId"] = dtInvEdit.Rows[0]["Currency_Id"].ToString();
                Session["CustomerId"] = dtInvEdit.Rows[0]["Supplier_Id"].ToString();
                Session["InvInformation"] = Objarr;
                txtSalesPerson.Text = Common.GetEmployeeName(dtInvEdit.Rows[0]["SalesPerson_Id"].ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString());
                txtPOSNo.Text = dtInvEdit.Rows[0]["PosNo"].ToString();
                txtAccountNo.Text = dtInvEdit.Rows[0]["Account_No"].ToString();
                txtInvoiceCosting.Text = dtInvEdit.Rows[0]["Invoice_Costing"].ToString();
                txtShift.Text = dtInvEdit.Rows[0]["Shift"].ToString();
                txtTender.Text = dtInvEdit.Rows[0]["Tender"].ToString();
                txtTaxP.Text = "0.00";
                txtDiscountP.Text = "0.00";
                //txtTaxP.Text = SetDecimal(dtInvEdit.Rows[0]["NetTaxP"].ToString());
                //txtDiscountP.Text = SetDecimal(dtInvEdit.Rows[0]["NetDiscountP"].ToString());
                string strTransNo = string.Empty;
                string strTransType = dtInvEdit.Rows[0]["SIFromTransType"].ToString();


                hdnMerchant.Value = dtInvEdit.Rows[0][29].ToString();
                hdnOrderId.Value = dtInvEdit.Rows[0][30].ToString();


                using (DataTable dtdetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocationId.ToString(), strInvoiceId, Session["FinanceYearId"].ToString()))
                {
                    Hdn_Exchange_Rate.Value = dtdetail.Rows[0]["Exchange_Rate"].ToString();
                    GvSalesInvoiceDetailDirect.DataSource = dtdetail;
                    GvSalesInvoiceDetailDirect.DataBind();
                }
            }
        }
        GridCalculation();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSInvNo);
    }
    #endregion
    #region PrintReturn
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Sales/SalesReturnPrint.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    #region Cancel
    public void FillGridCancel()
    {
        using (DataTable dt = ObjSalesReturnHeader.GetAllInActiveRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId))
        {
            objPageCmn.FillData((object)gvCancel, dt, "", "");
            Session["DtReturnBin"] = dt;
            Session["DtFilterReturnBin"] = dt;
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            lblSelectedRecord.Text = "";
        }
    }
    protected void gvCancel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCancel.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtFilterReturnBin"];
        objPageCmn.FillData((object)gvCancel, dt, "", "");

    }
    protected void gvCancel_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["DtFilterReturnBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        objPageCmn.FillData((object)gvCancel, dt, "", "");

    }
    protected void btnCancelMenu_Click(object sender, EventArgs e)
    {
        FillGridCancel();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {

            if (ddlFieldNameBin.SelectedItem.Value == "Invoice_Date" || ddlFieldNameBin.SelectedItem.Value == "Return_Date")
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

            DataTable dtCust = (DataTable)Session["DtReturnBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtFilterReturnBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvCancel, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvCancel.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvCancel.Rows.Count; i++)
        {
            ((CheckBox)gvCancel.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(gvCancel.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(gvCancel.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(gvCancel.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)gvCancel.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)gvCancel.Rows[index].FindControl("chkSelect")).Checked)
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
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridCancel();
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["DtFilterReturnBin"];

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
            for (int i = 0; i < gvCancel.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)gvCancel.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvCancel.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["DtFilterReturnBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvCancel, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        bool Result = true;

        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(ObjSalesReturnHeader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].ToString()).Rows[0]["Return_Date"].ToString()), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                    {
                        Result = false;
                        break;
                    }
                }
            }
        }
        if (!Result)
        {
            DisplayMessage("You can not restore closed financial year record");
            return;
        }
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjSalesReturnHeader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridCancel();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvCancel.Rows)
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
    protected void SetCustomerTextBoxBin(object sender, EventArgs e)
    {

        if (ddlFieldNameBin.Text == "Return_Date" || ddlFieldNameBin.Text == "Invoice_Date")
        {
            txtValueBin.Visible = false;

            txtValueBinDate.Visible = true;
        }
        else
        {
            txtValueBin.Visible = true;

            txtValueBinDate.Visible = false;
        }
        txtValueBin.Text = "";

        txtValueBinDate.Text = "";
    }
    #endregion
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
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
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
    protected string get_CCN_No(ref SqlTransaction trns)
    {
        string strCCNNo = string.Empty;
        int TotalRows = 0;
        strCCNNo = objDocNo.GetDocumentNo(true, "0", false, "160", "316", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        string sql = "select count(trans_id) from ac_voucher_header where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and Location_id='" + Session["LocId"].ToString() + "' and finance_code='" + Session["FinanceYearId"].ToString() + "' and Voucher_Type='CCN'";
        Int32.TryParse(da.get_SingleValue(sql, ref trns), out TotalRows);
        strCCNNo = strCCNNo + (TotalRows > 0 ? TotalRows.ToString() : "1");
        return strCCNNo;
    }
    protected bool checkInvoiceForPending(string strOtherAccountNo, string strInvoiceNo)
    {

        bool _result = false;

        using (DataTable dtPendingInvoice = objAgeingDetail.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "RV", strOtherAccountNo, "0", strInvoiceNo, false))
        {
            if (dtPendingInvoice.Rows.Count > 0)
            {
                _result = true;
            }
        }
        return _result;
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvSalesReturn, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvSalesReturn, lstCls);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void Btn_Li_Import_Click(object sender, EventArgs e)
    {

    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
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
                List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> lstInvoice = new List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> { };
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Inv_SalesReturnHeader.clsImportReturnInvoiceList _clsObj = new Inv_SalesReturnHeader.clsImportReturnInvoiceList();
                        _clsObj.merchant = dr["merchant"].ToString().Trim();
                        _clsObj.invoice_no = dr["invoice_no"].ToString().Trim();
                        _clsObj.ref_invoice_no = dr["ref_invoice_no"].ToString().Trim();
                        _clsObj.return_date = DateTime.Parse(dr["return_date"].ToString().Trim()).ToString("dd-MMM-yyyy");
                        _clsObj.payment_mode = dr["payment_mode"].ToString().Trim();
                        _clsObj.product_id = dr["product_id"].ToString().Trim();
                        _clsObj.return_qty = dr["return_qty"].ToString().Trim();
                        _clsObj.product_sno = dr["product_sno"].ToString().Trim();
                        _clsObj.return_reason = dr["return_reason"].ToString().Trim();
                        lstInvoice.Add(_clsObj);
                    }
                }
                oledbConn.Close();

                if (lstInvoice.Count > 0)
                {
                    List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> newInvoiceList = validateExcelData(lstInvoice);
                    if (newInvoiceList.Count > 0)
                    {
                        gvImportSRList.DataSource = newInvoiceList;
                        gvImportSRList.DataBind();
                        hdnTotalExcelRecords.Value = newInvoiceList.Count().ToString();
                        hdnInvalidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == false).Count().ToString();
                        hdnValidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == true).Count().ToString();
                        Session["ExcelSrImportList"] = newInvoiceList;
                    }
                    else
                    {
                        Session["ExcelSrImportList"] = null;
                        Session["ExcelImportInvoiceList"] = null;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Error in excel uploading - " + ex.Message);
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

    protected List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> validateExcelData(List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> lstInvoice)
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
        Set_AddressChild objAddressChild = new Set_AddressChild(Session["DBConnection"].ToString());

        int sno = 0;
        List<Inv_SalesReturnHeader.clsSrHeader> lstNewInvoice = new List<Inv_SalesReturnHeader.clsSrHeader> { };
        Inv_SalesReturnHeader.clsSrHeader clsSrInvHeader = null;

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


        Inv_SalesInvoiceHeader.clsSInvHeader clsSinvHeader = null;
        string strInvoiceNo = "0";
        string strRefInvoiceNo = "0";
        double invGrossAmt = 0, InvGrossTaxableAmt = 0, InvGrossTaxAmt = 0, InvGrossDiscountableAmt = 0, InvGrossDisAmt = 0;
        foreach (var _cls in lstInvoice)
        {
            double invoiceAmt = 0;
            sno++;
            try
            {
                if (_cls.invoice_no.Trim() != strInvoiceNo)
                {
                    if (clsSrInvHeader != null)
                    {
                        clsSrInvHeader.totalAmt = invGrossAmt.ToString();
                        if (InvGrossDisAmt > 0)
                        {
                            clsSrInvHeader.netDiscountV = InvGrossDisAmt.ToString();
                            clsSrInvHeader.netDiscountP = SetDecimal(((InvGrossDisAmt * 100) / InvGrossDiscountableAmt).ToString());
                        }
                        else
                        {
                            clsSrInvHeader.netDiscountP = clsSrInvHeader.netDiscountV = "0";
                        }

                        if (InvGrossTaxAmt > 0)
                        {
                            clsSrInvHeader.netTaxV = InvGrossTaxAmt.ToString();
                            clsSrInvHeader.netTaxP = SetDecimal(((InvGrossTaxAmt * 100) / InvGrossTaxableAmt).ToString());
                        }
                        else
                        {
                            clsSrInvHeader.netTaxV = clsSrInvHeader.netTaxP = "0";
                        }
                        clsSrInvHeader.totalQty = clsSrInvHeader.lstReturnProductDetail.Sum(m => double.Parse(m.returnQty)).ToString();
                        clsSrInvHeader.grandTotal = SetDecimal((invGrossAmt - InvGrossDisAmt + InvGrossTaxAmt).ToString());
                        lstNewInvoice.Add(clsSrInvHeader);
                    }

                    clsSrInvHeader = new Inv_SalesReturnHeader.clsSrHeader();
                    clsSrInvHeader.lstTaxDetail = new List<Inv_SalesInvoiceHeader.clsInvTaxDetail> { };
                    clsSrInvHeader.lstReturnProductDetail = new List<Inv_SalesReturnDetail.clsSrDetail> { };
                    clsSrInvHeader.lstSerailDetail = new List<Inv_SalesInvoiceHeader.clsProductSno> { };
                    clsSrInvHeader.lstPayTransDetail = new List<Inv_SalesInvoiceHeader.clsPayTrns> { };

                    invGrossAmt = InvGrossTaxableAmt = InvGrossTaxAmt = InvGrossDiscountableAmt = InvGrossDisAmt = 0; //iniatilize variable for invoice level calculation

                    //get invoice header
                    string strMerchantId = "0";
                    if (!string.IsNullOrEmpty(_cls.merchant))
                    {
                        using (DataTable _dt = objMerchantMaster.GetMerchantMaster_ByMerchantName(_cls.merchant))
                        {
                            if (_dt.Rows.Count == 0)
                            {
                                _cls.validation_remark = "Invalid merchant please select direct or any merchant";
                                continue;
                            }
                            else
                            {
                                strMerchantId = _dt.Rows[0]["Trans_id"].ToString();
                            }
                        }
                    }

                    strInvoiceNo = _cls.invoice_no;
                    strRefInvoiceNo = _cls.ref_invoice_no;
                    sno = 1;

                    if (!string.IsNullOrEmpty(_cls.invoice_no))
                    {
                        using (DataTable _dt = objSInvHeader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strInvoiceNo))
                        {
                            if (_dt.Rows.Count > 0 && _dt.Rows[0]["Post"].ToString() == "True")
                            {
                                clsSrInvHeader.invoiceId = _dt.Rows[0]["Trans_id"].ToString();
                            }
                            else
                            {
                                _cls.validation_remark = "There is no invoice found with this invoice_no " + strInvoiceNo + " or Invoice has been not posted yet";
                                continue;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(_cls.ref_invoice_no))
                    {
                        clsSrInvHeader.invoiceId = objSInvHeader.GetInvoiceIdByMerchantIdAndRefInvoiceNo(Session["LocId"].ToString(), strMerchantId, strRefInvoiceNo).ToString();
                        if (clsSrInvHeader.invoiceId == "0")
                        {
                            _cls.validation_remark = "There is no invoice found with the Merchant-" + _cls.merchant + " and ref_invoice_no " + strRefInvoiceNo;
                            continue;
                        }
                    }
                    else
                    {
                        _cls.validation_remark = "Please enter valid invoice no or ref_invoice_no";
                        continue;
                    }


                    //check unposted sales return for same invoice_id
                    using (DataTable _dtReturn = ObjSalesReturnHeader.GetAllRecord_By_InvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), clsSrInvHeader.invoiceId))
                    {
                        bool bIsUnpostedFound = false;
                        foreach (DataRow dr in _dtReturn.Rows)
                        {
                            if (dr["Post"].ToString().ToLower() == "false")
                            {
                                bIsUnpostedFound = true;
                            }
                        }

                        if (bIsUnpostedFound == true)
                        {
                            _cls.validation_remark = "Unposted sales return found with same invoice no please check and first post it";
                            continue;
                        }
                    }


                    if (string.IsNullOrEmpty(_cls.return_date))
                    {
                        _cls.validation_remark = "Invalid return date";
                        continue;
                    }
                    else
                    {
                        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(_cls.return_date), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                        {
                            _cls.validation_remark = "Return date out of financial year";
                            continue;
                        }
                        DateTime.Parse(_cls.return_date);
                    }
                    clsSrInvHeader.returnDate = DateTime.Parse(_cls.return_date).ToString("dd-MMM-yyyy");



                    if (!string.IsNullOrEmpty(_cls.payment_mode) && (_cls.payment_mode.ToLower() == "cash"))
                    {
                        clsSrInvHeader.payModeId = cashPayModeId;
                    }
                    else if (!string.IsNullOrEmpty(_cls.payment_mode) && (_cls.payment_mode.ToLower() == "credit"))
                    {
                        clsSrInvHeader.payModeId = creditPayModeId;
                    }
                    else
                    {
                        _cls.validation_remark = "Invalid Payment Mode it should be cash/credit";
                        continue;
                    }

                    clsSinvHeader = objSInvHeader.getInvoiceClassForSalesReturn(clsSrInvHeader.invoiceId, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), Session["FinanceYearId"].ToString());
                    clsSrInvHeader.companyId = Session["CompId"].ToString();
                    clsSrInvHeader.brandId = Session["BrandId"].ToString();
                    clsSrInvHeader.locationId = Session["LocId"].ToString();
                    clsSrInvHeader.customerId = clsSinvHeader.customerId;
                    clsSrInvHeader.currencyId = clsSinvHeader.currencyId;
                    clsSrInvHeader.invoiceNo = clsSinvHeader.invoiceNo;
                    clsSrInvHeader.salesPersonId = clsSinvHeader.salesPersonId;
                    clsSrInvHeader.siFromTransType = clsSinvHeader.siFromTransType;
                    clsSrInvHeader.siFromTransNo = "0";
                    clsSrInvHeader.invoiceDate = clsSinvHeader.invoiceDate;
                    clsSrInvHeader.isActive = true;
                    clsSrInvHeader.createdBy = clsSrInvHeader.modifiedBy = Session["userId"].ToString();
                    clsSrInvHeader.createdDate = clsSrInvHeader.modifiedDate = DateTime.Now.ToString();
                    clsSrInvHeader.transType = clsSinvHeader.transType;
                    clsSrInvHeader.remark = _cls.return_reason;
                }
                //get invoice detail
                Inv_SalesReturnDetail.clsSrDetail clsSrInvDetail = new Inv_SalesReturnDetail.clsSrDetail();
                clsSrInvDetail.sNo = sno.ToString();

                if (string.IsNullOrEmpty(_cls.product_id))
                {
                    _cls.validation_remark = "Invalid ProductId";
                    continue;
                }

                //get product id from code
                clsSrInvDetail.productId = objProductM.GetProductIdByCodeAndAlternetId(Session["compId"].ToString(), Session["brandId"].ToString(), _cls.product_id).ToString();
                if (clsSrInvDetail.productId == "0")
                {
                    _cls.validation_remark = "Invalid ProductId";
                    continue;
                }


                //check return quantity
                double ReturnQty = 0;
                double.TryParse(_cls.return_qty, out ReturnQty);
                if (ReturnQty == 0)
                {
                    _cls.validation_remark = "Invalid Return Qty";
                    continue;
                }
                clsSrInvDetail.returnQty = _cls.return_qty;

                //check product serial
                string[] strProductSno = { };
                if (!string.IsNullOrEmpty(_cls.product_sno))
                {
                    strProductSno = _cls.product_sno.Split(',');
                    if (strProductSno.Count() > int.Parse(_cls.return_qty))
                    {
                        _cls.validation_remark = "Serial detail should equal to or less then qty";
                        continue;
                    }
                }

                //check productid in sales detail
                try
                {
                    List<Inv_SalesInvoiceHeader.clsSInvDetail> lstSinvDetail = clsSinvHeader.lstProductDetail.Where(x => x.productId == clsSrInvDetail.productId).ToList();
                    foreach (var _clsSinvPDetail in lstSinvDetail)
                    {
                        double SalesQty = 0;
                        double oldReturnQty = 0;
                        double newReturnQty = 0;
                        double.TryParse(_clsSinvPDetail.salesQty, out SalesQty);
                        double.TryParse(_clsSinvPDetail.returnQty, out oldReturnQty);
                        double.TryParse(_cls.return_qty, out newReturnQty);
                        if (newReturnQty > (SalesQty - oldReturnQty))
                        {
                            _cls.validation_remark = "Return quantity exceed from bal qty";
                            continue;
                        }
                        else
                        {

                        }

                        //check serial 
                        if (strProductSno.Length > 0)
                        {
                            int validSerialCount = 0;
                            if (clsSinvHeader.lstProductSerialDetail != null)
                            {
                                List<Inv_SalesInvoiceHeader.clsProductSno> lstSiProductSno = clsSinvHeader.lstProductSerialDetail.Where(x => x.productId == clsSrInvDetail.productId && x.refRowSno == _clsSinvPDetail.sNo).ToList();
                                foreach (var _clsSiProductSno in lstSiProductSno)
                                {
                                    foreach (string str in strProductSno)
                                    {
                                        if (str == _clsSiProductSno.serialNo)
                                        {
                                            Inv_SalesInvoiceHeader.clsProductSno _clsSrProductSno = new Inv_SalesInvoiceHeader.clsProductSno();
                                            _clsSrProductSno.productId = clsSrInvDetail.productId;
                                            _clsSrProductSno.refRowSno = clsSrInvDetail.sNo;
                                            _clsSrProductSno.serialNo = str;
                                            clsSrInvHeader.lstSerailDetail.Add(_clsSrProductSno);
                                            validSerialCount++;
                                        }
                                    }
                                }

                                if (validSerialCount != strProductSno.Length)
                                {
                                    _cls.validation_remark = "There is serial mismatch";
                                    continue;
                                }

                            }
                            else
                            {
                                _cls.validation_remark = "There is no serial information found with invoice so please remove serail no detail";
                                continue;
                            }


                        }


                        double UnitPrice = 0;
                        double.TryParse(_clsSinvPDetail.unitPrice, out UnitPrice);
                        clsSrInvDetail.unitPrice = _clsSinvPDetail.unitPrice;
                        clsSrInvDetail.unitId = _clsSinvPDetail.unitId;

                        invGrossAmt += double.Parse(SetDecimal((UnitPrice * ReturnQty).ToString())); //invoice gross amount to update sales header

                        double DiscountRate = 0;
                        double.TryParse(_clsSinvPDetail.discountPer, out DiscountRate);
                        clsSrInvDetail.netDiscountP = _clsSinvPDetail.discountPer;

                        if (DiscountRate > 0)
                        {
                            InvGrossDiscountableAmt += double.Parse(SetDecimal((ReturnQty * UnitPrice).ToString()));
                            InvGrossDisAmt += double.Parse(SetDecimal(((ReturnQty * UnitPrice) / DiscountRate).ToString()));
                        }


                        double TaxableAmount = 0;
                        TaxableAmount = double.Parse(SetDecimal(((ReturnQty * UnitPrice) - ((ReturnQty * UnitPrice) * DiscountRate / 100)).ToString()));
                        double TotalProductAmt = TaxableAmount;

                        InvGrossTaxableAmt += TaxableAmount;
                        //Get tax detail
                        if (clsSinvHeader.lstTaxDetail != null)
                        {
                            List<Inv_SalesInvoiceHeader.clsInvTaxDetail> lstTaxDetail = clsSinvHeader.lstTaxDetail.Where(x => x.productId == _clsSinvPDetail.productId && x.refRowSno == _clsSinvPDetail.sNo).ToList();
                            if (lstTaxDetail.Count > 0)
                            {
                                foreach (var _clsSiTaxDetal in lstTaxDetail)
                                {
                                    Inv_SalesInvoiceHeader.clsInvTaxDetail clsSrTaxDetail = new Inv_SalesInvoiceHeader.clsInvTaxDetail();
                                    clsSrTaxDetail.refType = "SR";
                                    clsSrTaxDetail.refRowSno = sno.ToString();
                                    clsSrTaxDetail.productId = _clsSinvPDetail.productId;
                                    clsSrTaxDetail.taxableAmount = TaxableAmount.ToString();
                                    clsSrTaxDetail.expensesId = "0";
                                    clsSrTaxDetail.taxId = _clsSiTaxDetal.taxId;
                                    clsSrTaxDetail.taxPer = _clsSiTaxDetal.taxPer;
                                    clsSrTaxDetail.taxValue = SetDecimal((TaxableAmount * int.Parse(_clsSiTaxDetal.taxPer) / 100).ToString());
                                    clsSrInvHeader.lstTaxDetail.Add(clsSrTaxDetail);
                                    InvGrossTaxAmt += double.Parse(clsSrTaxDetail.taxValue);
                                }
                            }
                        }

                        //update sales return detail
                        clsSrInvDetail.brandId = Session["BrandId"].ToString();
                        clsSrInvDetail.companyId = Session["CompId"].ToString();
                        clsSrInvDetail.locationId = Session["LocId"].ToString();
                        clsSrInvDetail.orderQty = _clsSinvPDetail.orderQty;
                        clsSrInvDetail.salesqty = _clsSinvPDetail.salesQty;
                        clsSrInvDetail.returnQty = _cls.return_qty;
                        clsSrInvDetail.totalReturnQty = (newReturnQty + oldReturnQty).ToString();
                        clsSrInvDetail.netTaxP = _clsSinvPDetail.taxPer;
                        clsSrInvDetail.netTaxV = _clsSinvPDetail.taxValue;
                        clsSrInvDetail.netDiscountP = _clsSinvPDetail.discountPer;
                        clsSrInvDetail.netDiscountV = _clsSinvPDetail.discountValue;
                        clsSrInvDetail.siFromTransType = clsSinvHeader.siFromTransType;
                        clsSrInvDetail.siFromTransNo = _clsSinvPDetail.salesOrderId;

                    }
                }
                catch (Exception ex)
                {

                }




                //add product detail in sales invoice header 
                clsSrInvHeader.lstReturnProductDetail.Add(clsSrInvDetail);

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
        if (clsSrInvHeader != null)
        {
            clsSrInvHeader.totalAmt = invGrossAmt.ToString();
            if (InvGrossDisAmt > 0)
            {
                clsSrInvHeader.netDiscountV = InvGrossDisAmt.ToString();
                clsSrInvHeader.netDiscountP = SetDecimal(((InvGrossDisAmt * 100) / InvGrossDiscountableAmt).ToString());
            }
            else
            {
                clsSrInvHeader.netDiscountP = clsSrInvHeader.netDiscountV = "0";
            }

            if (InvGrossTaxAmt > 0)
            {
                clsSrInvHeader.netTaxV = InvGrossTaxAmt.ToString();
                clsSrInvHeader.netTaxP = SetDecimal(((InvGrossTaxAmt * 100) / InvGrossTaxableAmt).ToString());
            }
            else
            {
                clsSrInvHeader.netTaxV = clsSrInvHeader.netTaxP = "0";
            }
            clsSrInvHeader.totalQty = clsSrInvHeader.lstReturnProductDetail.Sum(m => double.Parse(m.returnQty)).ToString();
            clsSrInvHeader.grandTotal = SetDecimal((invGrossAmt - InvGrossDisAmt + InvGrossTaxAmt).ToString());
            lstNewInvoice.Add(clsSrInvHeader);
        }

        if (lstNewInvoice.Count > 0)
        {
            Session["ExcelImportSrInvoiceList"] = lstNewInvoice;
        }
        else
        {
            Session["ExcelImportSrInvoiceList"] = null;
        }

        return lstInvoice;

    }

    protected void lnkInvalidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelSrImportList"] != null)
        {
            List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> newInvoiceList = (List<Inv_SalesReturnHeader.clsImportReturnInvoiceList>)Session["ExcelSrImportList"];
            gvImportSRList.DataSource = newInvoiceList.Where(m => m.is_valid == false).ToList();
            gvImportSRList.DataBind();
        }
    }


    protected void lnkValidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelSrImportList"] != null)
        {
            List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> newInvoiceList = (List<Inv_SalesReturnHeader.clsImportReturnInvoiceList>)Session["ExcelSrImportList"];
            gvImportSRList.DataSource = newInvoiceList.Where(m => m.is_valid == true).ToList();
            gvImportSRList.DataBind();
        }
    }

    protected void lnkTotalExcelImportRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelSrImportList"] != null)
        {
            List<Inv_SalesReturnHeader.clsImportReturnInvoiceList> newInvoiceList = (List<Inv_SalesReturnHeader.clsImportReturnInvoiceList>)Session["ExcelSrImportList"];
            gvImportSRList.DataSource = newInvoiceList;
            gvImportSRList.DataBind();
        }
    }

    protected void btnSaveExcelInvoice_Click(object sender, EventArgs e)
    {
        if (hdnInvalidExcelRecords.Value != "0" || Session["ExcelImportSrInvoiceList"] == null)
        {
            return;
        }
        List<Inv_SalesReturnHeader.clsSrHeader> lstInvoiceList = (List<Inv_SalesReturnHeader.clsSrHeader>)Session["ExcelImportSrInvoiceList"];
        if (lstInvoiceList.Count == 0)
        {
            return;
        }

        string strInvoiceDocNo = GetDocumentNumber();

        string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstInvoiceList)
            {
                //update sales invoice header
                _cls.returnNo = strInvoiceDocNo;
                _cls.salesReturnId = ObjSalesReturnHeader.InsertRecord(_cls.companyId, _cls.brandId, _cls.locationId, _cls.invoiceId, _cls.returnNo, _cls.returnDate, _cls.invoiceNo, _cls.invoiceDate, _cls.payModeId, _cls.currencyId, _cls.siFromTransType, "0", _cls.salesPersonId, "0", _cls.remark, false.ToString(), _cls.totalQty, _cls.totalAmt, _cls.netTaxP, _cls.netTaxV, _cls.netDiscountP, _cls.netDiscountV, _cls.grandTotal, _cls.customerId, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns).ToString();
                //update sales return no as per document system
                if (_cls.returnNo == strInvoiceDocNo)
                {
                    string sql = string.Empty;
                    sql = "SELECT count(*) FROM Inv_SalesReturnHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "'";
                    int strInvoiceCount = Int32.Parse(da.get_SingleValue(sql, ref trns));
                    bool bFlag = false;
                    while (bFlag == false)
                    {
                        _cls.returnNo = strInvoiceDocNo + (strInvoiceCount == 0 ? "1" : strInvoiceCount.ToString());
                        string sql1 = "SELECT count(*) FROM Inv_SalesReturnHeader WHERE Company_Id = '" + Session["CompId"].ToString() + "' AND Brand_Id = '" + Session["BrandId"].ToString() + "' AND Location_Id = '" + Session["LocId"].ToString() + "' and return_no='" + _cls.returnNo + "'";
                        string strInvCount = da.get_SingleValue(sql1, ref trns);
                        if (strInvCount == "0")
                        {
                            bFlag = true;
                        }
                        else
                        {
                            strInvoiceCount++;
                        }
                    }
                    ObjSalesReturnHeader.Updatecode(_cls.salesReturnId.ToString(), _cls.returnNo, ref trns);
                }


                //update sales invoice table
                foreach (Inv_SalesReturnDetail.clsSrDetail clsSiDetail in _cls.lstReturnProductDetail)
                {
                    clsSiDetail.detailId = ObjSalesReturnDetail.InsertRecord(_cls.companyId, _cls.brandId, _cls.locationId, _cls.salesReturnId, _cls.invoiceId, clsSiDetail.sNo, clsSiDetail.siFromTransType, clsSiDetail.siFromTransNo, clsSiDetail.productId, clsSiDetail.unitId, clsSiDetail.unitPrice, "0", clsSiDetail.orderQty, clsSiDetail.salesqty, clsSiDetail.totalReturnQty, clsSiDetail.returnQty, clsSiDetail.netTaxP, clsSiDetail.netTaxV, clsSiDetail.netDiscountP, clsSiDetail.netDiscountV == null ? "0" : clsSiDetail.netDiscountV, "0", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns).ToString();
                    //proceed with product serial nos
                    foreach (Inv_SalesInvoiceHeader.clsProductSno clsProductSerial in _cls.lstSerailDetail)
                    {
                        if (clsSiDetail.sNo == clsProductSerial.refRowSno)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SR", _cls.salesReturnId, clsSiDetail.productId, clsSiDetail.unitId, "I", "0", "0", "1", DateTime.Now.ToString(), clsProductSerial.serialNo.Trim(), "1/1/1800", "", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //proceed with tax detail
                    double taxValue = 0;
                    double.TryParse(clsSiDetail.netTaxV, out taxValue);
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
                                objTaxRefDetail.InsertRecord("SR", _cls.salesReturnId.ToString(), "0", "0", clsSiDetail.productId, clsTax.taxId, clsTax.taxPer, clsTax.taxValue, false.ToString(), clsTax.taxableAmount, clsSiDetail.detailId.ToString(), _cls.transType, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            }
                        }
                    }
                }

                //proceed with payment detail
                ObjPaymentTrans.insert(Session["CompId"].ToString(), _cls.payModeId.ToString().ToLower(), "SR", _cls.salesReturnId, "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), _cls.grandTotal, _cls.currencyId, "1", _cls.grandTotal, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            trns.Commit();
            con.Close();
            DisplayMessage("Total " + lstInvoiceList.Count + " Records inserted successfully");
            gvImportSRList.DataSource = null;
            gvImportSRList.DataBind();
            btnSaveExcelInvoice.Enabled = false;
            Session["ExcelImportSrInvoiceList"] = null;

        }
        catch (Exception ex)
        {
            trns.Rollback();
            con.Close();
            DisplayMessage(ex.Message);
        }

        //save address master
    }
}