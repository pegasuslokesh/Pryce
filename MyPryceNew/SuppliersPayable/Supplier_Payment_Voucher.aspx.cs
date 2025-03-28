using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using net.webservicex.www;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class SuppliersPayable_Supplier_Payment_Voucher : System.Web.UI.Page
{
    Ems_ContactMaster ObjContactMaster = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    Ac_ChartOfAccount objCOA = null;
    LocationMaster ObjLocation = null;
    SystemParameter objsys = null;
    Set_BankMaster objBank = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Ac_Finance_Year_Info objFYI = null;
    Set_Suppliers objSupplier = null;
    Ac_ParameterMaster objAccParameter = null;
    EmployeeMaster objEmployee = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;
    Set_Approval_Employee objApproalEmp = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strLocationCurrency = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayable/Supplier_Payment_Voucher.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            FillLocationList();
            FillCurrency();
            try
            {
                ddlLocalCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
                txtExchangeRate.Text = "1";
                hdnFExchangeRate.Value = txtExchangeRate.Text;
            }
            catch
            {

            }

            CalendarExtender_chequeCleardate.Format = objsys.SetDateFormat();
            CalendarExtender_txtchequeissuedate.Format = objsys.SetDateFormat();
            CalendarExtender_txtVoucherDate.Format = objsys.SetDateFormat();
            //AllPageCode();
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "312", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            //For Comment
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            hdnLocId.Value = Session["LocId"].ToString();
            Session["SupLocId"] = hdnLocId.Value;
            hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();

            btnList_Click(sender, e);
            FillGrid();
            rbCashPayment.Checked = true;



        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = ddlLocationList.SelectedValue;
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnVoucherSave.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLocalCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            objPageCmn.FillData((object)ddlForeginCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlLocalCurrency.Items.Insert(0, "--Select--");
            ddlLocalCurrency.SelectedIndex = 0;
            ddlForeginCurrency.Items.Insert(0, "--Select--");
            ddlForeginCurrency.SelectedIndex = 0;
        }
    }
    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/SPV", "SuppliersPayable", "SupplierPaymentVoucher", e.CommandName.ToString(), e.CommandName.ToString());

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlPendingPaymentDetail.Visible = false;
        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlPendingPaymentDetail.Visible = false;
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        btnAddSupplier.Visible = true;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;
    }
    public void DisplayMessage(string str, string color = "orange")
    {

        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnAddSupplier_OnClick(object sender, EventArgs e)
    {
        if (txtSupplierName.Text == "")
        {
            DisplayMessage("First Fill Supplier Name");
            txtSupplierName.Focus();
            return;
        }

        if (chkAdvancePayment.Checked == true)
        {
            DisplayMessage("Sorry you selected advance payment option");
            return;
        }


        //here we are  checking that any request pending or not for selected supplier

        strLocationId = hdnLocId.Value;

        DataTable dtpendingSpv = objVoucherHeader.GetVoucherHeaderAllTrueWithVoucherDetail(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());


        dtpendingSpv = new DataView(dtpendingSpv, "Field1='SPV' and Field3='Pending' and Other_Account_No='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        if (dtpendingSpv.Rows.Count > 0)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Payment appproval request is pending for selected supplier');", true);

            //Response.Write("<script>alert('payment appproval request is pending for selected supplier');</script>");

        }

        txtPaidLocalAmount.Text = "0";
        txtPaidForeignamount.Text = "0";
        txtNetAmount.Text = "0";
        txtExchangeRate.Text = "1";
        ddlForeginCurrency.SelectedIndex = 0;
        int voucher_id = 0;
        int.TryParse(hdnVoucherId.Value.ToString(), out voucher_id);


        if (getAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", GvPendingInvoice, txtSupplierName.Text.Split('/')[1].ToString(), voucher_id.ToString()) == false)
        {
            DisplayMessage("No Record Found");
        }
    }


    protected Boolean getAgeingDetail(string strCompanyId, string strBrandId, string strLocationId, string strAgeingType, GridView GvPendingInvoice, string strOtherAccountNo, string strVoucherId)
    {
        Boolean _result = false;
        //if (strAgeingType != "PV" || strAgeingType != "RV")
        //{
        //    return _result;
        //}
        double _voucherId = 0;
        double.TryParse(strVoucherId, out _voucherId);


        DataTable dtDetail = objAgeingDetail.getPendingAgeingTable(strCompanyId, StrBrandId, strLocationId, strAgeingType, strOtherAccountNo, strVoucherId, "");


        if (dtDetail.Rows.Count > 0)
        {
            GvPendingInvoice.DataSource = dtDetail;
            GvPendingInvoice.DataBind();
            DataTable dtAgDetail = new DataTable();
            if (_voucherId > 0)
            {

                string sql = "select * from ac_ageing_detail where voucherId=" + _voucherId +
 " and Company_Id='" + strCompanyId.ToString() + "' and Brand_Id='" + strBrandId.ToString() + "' and Location_Id='" + strLocationId.ToString() + "'";
                dtAgDetail = da.return_DataTable(sql);
            }
            DataTable dtTemp = new DataTable();
            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

            foreach (GridViewRow gvS in GvPendingInvoice.Rows)
            {

                CheckBox chkSelect = (CheckBox)gvS.FindControl("chkTrandId");
                chkSelect.Enabled = true;
                ((TextBox)gvS.FindControl("txtpayLocal")).Enabled = false;
                Label lblInvAmt = (Label)gvS.FindControl("lblinvamount");
                Label lblBalanceAmt = (Label)gvS.FindControl("lblBalanceAmount");



                if (_voucherId > 0 && dtAgDetail.Rows.Count > 0)
                {
                    //DataRow[] drAgeing;
                    dtTemp = new DataView(dtAgDetail, "Invoice_No='" + ((Label)gvS.FindControl("lblPONo")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                    // drAgeing = dtAgDetail.Select("Invoice_No='" + ((Label)gvS.FindControl("lblPONo")).Text + "'");

                    if (dtTemp.Rows.Count > 0)
                    {
                        if (dtTemp.Rows[0]["Invoice_no"].ToString() == ((Label)gvS.FindControl("lblPONo")).Text)
                        {
                            chkSelect.Checked = true;
                            ((TextBox)gvS.FindControl("txtpayLocal")).Enabled = true;
                            if (double.Parse(dtTemp.Rows[0]["Exchange_Rate"].ToString()) == 1 || double.Parse(dtTemp.Rows[0]["Exchange_Rate"].ToString()) == 0)
                            {
                                ((TextBox)gvS.FindControl("txtpayLocal")).Text = SetDecimal(dtTemp.Rows[0]["Paid_Receive_Amount"].ToString());
                            }
                            else
                            {
                                ((TextBox)gvS.FindControl("txtpayLocal")).Text = SetDecimal(dtTemp.Rows[0]["Foreign_Amount"].ToString());
                            }
                            validateInvoiceGrid(gvS, "TextBox");
                        }
                    }
                }

                // Label lblDueAmt = (Label)gvS.FindControl("lbldueamount");
                // Label lblForeignAmt = (Label)gvS.FindControl("lblFregnamount");

                lblInvAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblInvAmt.Text);
                lblBalanceAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblBalanceAmt.Text);
                //lblDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDueAmt.Text);
                //lblForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblForeignAmt.Text);
            }
            _result = true;
        }
        else
        {
            //chkAdvancePayment.Checked = false;
            //txtExchangeRate.Text = "";

            GvPendingInvoice.DataSource = null;
            GvPendingInvoice.DataBind();
            //ddlForeginCurrency.SelectedIndex = 0;
            //DisplayMessage("No Record Available for Supplier");
            _result = false;
        }
        return _result;
    }
    protected void chkGvBank_CheckedChanged(object sender, EventArgs e)
    {
        // Get the GridView row that contains the clicked checkbox
        GridViewRow row = (GridViewRow)((CheckBox)sender).NamingContainer;

        // Loop through all the rows in the GridView
        foreach (GridViewRow gridViewRow in gvBankDetails.Rows)
        {
            // Find the checkbox in each row
            CheckBox chk = (CheckBox)gridViewRow.FindControl("chkGvBank");

            // Uncheck all checkboxes except the one that triggered the event
            if (gridViewRow.RowIndex != row.RowIndex)
            {
                chk.Checked = false;
            }
        }
    }



    public void FillSupplierBankDetail(string SupplierID)
    {
        try
        {
            DataTable dtSupplier = da.return_DataTable("Select Ref_Id from Ac_AccountMaster where Trans_Id='" + SupplierID + "'");
            if (dtSupplier.Rows.Count > 0)
            {
                DataTable dt = objSupplier.GetSupplierBankRecord(dtSupplier.Rows[0]["Ref_Id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    //ViewState["BankDetails"] = dt;
                    gvBankDetails.DataSource = dt;
                    gvBankDetails.DataBind();
                }
                else
                {
                    gvBankDetails.DataSource = null;
                    gvBankDetails.DataBind();
                }
            }
            else
            {
                gvBankDetails.DataSource = null;
                gvBankDetails.DataBind();
            }


        }
        catch (Exception ex)
        {
            gvBankDetails.DataSource = null;
            gvBankDetails.DataBind();
        }
    }
    protected void btn_PrintClick(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../SuppliersPayable/SupplierPaymentReport.aspx?VoucherId=" + e.CommandArgument.ToString() + "&&EmpId=" + e.CommandName.ToString() + " ','window','width=1024');", true);

    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            FillSupplierBankDetail(txtSupplierName.Text.Split('/')[1].ToString());
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSupplierName.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["spvSupplierId"] = dt.Rows[0]["Ref_id"].ToString();
                        Session["spvCurrencyId"] = dt.Rows[0]["Currency_id"].ToString();
                        Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        return;
                    }

                }
            }
        }
        catch
        { }
        DisplayMessage("Supplier is not valid");
        txtSupplierName.Text = "";
        txtSupplierName.Focus();

    }



    protected void updateControlsValue(bool isForeignToLocal = false)
    {
        if (ddlForeginCurrency.SelectedIndex == 0)
        {
            ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
            txtExchangeRate.Text = "1";
            hdnFExchangeRate.Value = txtExchangeRate.Text;
        }
        string strFireignExchange = string.Empty;

        //double debit = 0;
        //double credit = 0;
        double trns_amount = 0;
        double exchange_rate = 0;
        double foreign = 0;


        //double.TryParse(txtCreditAmount.Text, out credit);




        if (ddlForeginCurrency.SelectedValue == ddlLocalCurrency.SelectedValue)
        {
            txtExchangeRate.Text = "1";
            txtPaidForeignamount.Enabled = false;
        }
        else
        {
            txtPaidForeignamount.Enabled = true;
        }

        //hdnFExchangeRate.Value = txtExchangeRate.Text;
        double.TryParse(txtExchangeRate.Text, out exchange_rate);
        double.TryParse(exchange_rate.ToString("0.000000"), out exchange_rate);
        if (isForeignToLocal == false)
        {
            //line commented by  jitendra on 06-03-2017
            //double.TryParse((1 / exchange_rate).ToString("0.000000"), out exchange_rate);
            hdnFExchangeRate.Value = exchange_rate.ToString();
            //exchange_rate = (1 / exchange_rate);
            double.TryParse(txtPaidLocalAmount.Text, out trns_amount);
            //trns_amount = debit;
            txtPaidForeignamount.Text = GetCurrency(ddlLocalCurrency.SelectedValue, ((1 / exchange_rate) * trns_amount).ToString());
            txtPaidForeignamount.Text = txtPaidForeignamount.Text.Trim().Split('/')[0].ToString();
        }
        else
        {
            //line added by jitendra on 06-03-2017


            //code start

            double.TryParse((exchange_rate).ToString("0.000000"), out exchange_rate);

            //code end
            double.TryParse(txtPaidForeignamount.Text, out foreign);
            txtPaidLocalAmount.Text = GetCurrency(ddlLocalCurrency.SelectedValue, (exchange_rate * foreign).ToString());
            txtPaidLocalAmount.Text = txtPaidLocalAmount.Text.Trim().Split('/')[0].ToString();
        }

        txtExchangeRate.Text = exchange_rate.ToString();
        hdnFExchangeRate.Value = exchange_rate.ToString();

    }


    protected void txtPaidLocalAmount_TextChanged(object sender, EventArgs e)
    {



        updateControlsValue();
        //if (txtPaidLocalAmount.Text != "")
        //{
        //    double PaidAmt = Convert.ToDouble(txtPaidLocalAmount.Text);
        //    if (PaidAmt != 0)
        //    {
        //        ddlForeginCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //        ddlForeginCurrency_SelectedIndexChanged(sender, e);
        //        txtPaidLocalAmount.Text = SetDecimal(txtPaidLocalAmount.Text);
        //    }
        //}
        GetNetAmount();
    }

    protected void txtPaidForeignamount_TextChanged(object sender, EventArgs e)
    {
        updateControlsValue(true);
        GetNetAmount();
        //if (txtPaidForeignamount.Text != "")
        //{
        //    double PaidAmt=0;
        //    double.TryParse(txtPaidForeignamount.Text, out PaidAmt);
        //    double exchangeRate = 0;
        //    double.TryParse(txtExchangeRate.Text, out exchangeRate);
        //    if (PaidAmt != 0 && exchangeRate != 0)
        //    {
        //        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //        txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, (float.Parse(exchangeRate.ToString()) * float.Parse(PaidAmt.ToString())).ToString());
        //        GetNetAmount();
        //    }

        //}
    }

    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {

        if (ddlForeginCurrency.SelectedValue == ddlLocalCurrency.SelectedValue)
        {
            updateControlsValue();
        }
        else
        {
            updateControlsValue(true);
        }


        //if (txtExchangeRate.Text == "" || txtExchangeRate.Text == "0")
        //{
        //    txtPaidForeignamount.Text = "0";
        //    return;
        //}

        //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //if (ddlForeginCurrency.SelectedIndex != 0)
        //{
        //    //txtPaidLocalAmount.Text = getLocalAmount(ddlForeginCurrency.SelectedValue.ToString(), txtExchangeRate.Text, txtPaidForeignamount.Text);
        //    if (ddlForeginCurrency.SelectedValue != strCurrency)
        //    {
        //        if (txtPaidForeignamount.Text != "" && txtPaidForeignamount.Text != "0")
        //        {
        //            txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, (float.Parse(txtExchangeRate.Text) * float.Parse(txtPaidForeignamount.Text)).ToString());
        //            hdnFExchangeRate.Value = txtExchangeRate.Text;
        //            txtExchangeRate.Text = txtExchangeRate.Text;
        //        }

        //    }
        //    else
        //    {
        //        //txtPaidForeignamount.Text = txtPaidForeignamount.Text;
        //        txtExchangeRate.Text = "1";
        //        hdnFExchangeRate.Value = txtExchangeRate.Text;
        //        txtExchangeRate.Text = txtExchangeRate.Text;
        //    }

        //    //txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(ddlForeginCurrency.SelectedValue, (float.Parse(txtExchangeRate.Text) * float.Parse(txtPaidLocalAmount.Text)).ToString());
        //    //hdnFExchangeRate.Value = txtExchangeRate.Text;
        //    //txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
        //}
        //else
        //{
        //    DisplayMessage("Please select currency first");
        //    return;
        //}
        GetNetAmount();
        ////string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();



    }

    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForeginCurrency.SelectedIndex > 0)
        {


            hdnFExchangeRate.Value = SystemParameter.GetExchageRate(ddlForeginCurrency.SelectedValue, ddlLocalCurrency.SelectedValue, Session["DBConnection"].ToString());
            //  string  strFireignExchange = hdnFExchangeRate.Value;

            txtExchangeRate.Text = hdnFExchangeRate.Value;

            //string strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue,"1");
            //txtPaidForeignamount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
            // hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();


            //line commented by jitendra on 06-03-2017

            if (ddlForeginCurrency.SelectedValue != ddlLocalCurrency.SelectedValue)
            {
                updateControlsValue(true);

            }
            else
            {
                //hdnFExchangeRate.Value = (1 / double.Parse(hdnFExchangeRate.Value)).ToString("0.000000");
                updateControlsValue();
            }

        }

        GetNetAmount();
        //if (ddlForeginCurrency.SelectedIndex == 0)
        //{
        //    hdnFExchangeRate.Value = "0";
        //    txtExchangeRate.Text = "0";
        //}
        //else
        //{
        //    string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //    if (strCurrency == ddlForeginCurrency.SelectedValue)
        //    {
        //        txtPaidForeignamount.Enabled = false;
        //        txtExchangeRate.Enabled = false;

        //        txtPaidLocalAmount.Enabled = true;
        //    }
        //    else
        //    {
        //        txtPaidForeignamount.Enabled = true;
        //        txtExchangeRate.Enabled = true;
        //        txtPaidLocalAmount.Enabled = false;
        //        txtPaidLocalAmount.Text = "";
        //    }

        //    string strFireignExchange = string.Empty;
        //    if (txtPaidLocalAmount.Text != "")
        //    {
        //        strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue, txtPaidLocalAmount.Text);
        //        txtPaidForeignamount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
        //        txtPaidForeignamount.Text = SetDecimal(txtPaidForeignamount.Text);
        //        hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();
        //        txtExchangeRate.Text = hdnFExchangeRate.Value;
        //        txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
        //    }
        //    else
        //    {
        //        strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue, "0");
        //        hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();
        //        txtExchangeRate.Text = hdnFExchangeRate.Value;
        //        txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
        //    }
        //}
    }







    public string GetNetAmount()
    {
        string strTotal = "0";
        double LocalPaid = 0;
        double BankCharges = 0;
        if (txtPaidLocalAmount.Text != "")
        {
            LocalPaid = Convert.ToDouble(txtPaidLocalAmount.Text);
        }
        else
        {
            LocalPaid = 0;
        }

        if (txtBankCharges.Text != "")
        {
            BankCharges = Convert.ToDouble(txtBankCharges.Text);
        }
        else
        {
            BankCharges = 0;
        }

        if (LocalPaid != 0)
        {
            strTotal = LocalPaid.ToString();
        }
        if (BankCharges != 0)
        {
            strTotal = (float.Parse(LocalPaid.ToString()) + float.Parse(BankCharges.ToString())).ToString();
        }

        txtNetAmount.Text = strTotal;
        txtNetAmount.Text = SetDecimal(txtNetAmount.Text);
        return strTotal;
    }
    public string GetDateFormat(string Value)
    {
        string newdate = string.Empty;
        try
        {
            newdate = Convert.ToDateTime(Value).ToString(objsys.SetDateFormat());
        }
        catch
        {

        }
        return newdate;
    }
    protected void txtBankCharges_textChnaged(object sender, EventArgs e)
    {
        if (txtAcbankCharges.Text != "")
        {
            GetNetAmount();
        }
        txtBankCharges.Text = SetDecimal(txtBankCharges.Text);
    }
    protected void txtAcbankCharges_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = da.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }

        if (txtBankCharges.Text != "")
        {
            GetNetAmount();
        }
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = da.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
    }
    protected void txtBankName_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();
                DataTable dtCOA = objBank.GetBankMaster();
                dtCOA = new DataView(dtCOA, "Bank_Id='" + ((TextBox)sender).Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Bank in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Bank in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }
    protected void txtpayforeign_OnTextChanged(object sender, EventArgs e)
    {
        double sumForeignamt = 0;

        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (txtPayForeign.Text == "")
                {
                    txtPayForeign.Text = "0";
                }
                sumForeignamt += Convert.ToDouble(txtPayForeign.Text);
                txtPayForeign.Text = SetDecimal(txtPayForeign.Text);
            }
        }
        txtPaidForeignamount.Text = sumForeignamt.ToString();
        txtPaidForeignamount.Text = SetDecimal(txtPaidForeignamount.Text);
        GetNetAmount();
    }
    protected void txtpayLocal_OnTextChanged(object sender, EventArgs e)
    {
        if (validateInvoiceGrid((GridViewRow)((TextBox)sender).Parent.Parent, "TextBox") == false)
        {
            DisplayMessage("Due Blance Amount is only " + ((Label)((GridViewRow)((TextBox)sender).Parent.Parent).FindControl("lblBalanceAmount")).Text);
        }
        //GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
        //string strFireignExchange = "";
        ////string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text);

        //if (strFireignExchange == "")
        //{
        //    strFireignExchange = "0";
        //}
        //double dblBalanceAmt=0;
        //double dblPayAmt=0;
        //double.TryParse(((Label)gvrowtxt.FindControl("lbldueamount")).Text, out dblBalanceAmt);
        //double.TryParse(((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text, out dblPayAmt);

        //if (dblPayAmt > dblBalanceAmt)
        //{
        //    ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text = "0";
        //    return;
        //}
        ////((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ////((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ////((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        //double sumLocalamt = 0;
        //double sumForeignAmt = 0;
        //foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        //{
        //    CheckBox chkTransId = (CheckBox)gvrow.FindControl("chkTrandId");
        //    Label lblInvAmt = (Label)gvrow.FindControl("lblinvamount");
        //    //Label lblPaidAmt = (Label)gvrow.FindControl("lblpaidamount");
        //    //Label lblDueAmt = (Label)gvrow.FindControl("lbldueamount");
        //    //Label lblFregnAmt = (Label)gvrow.FindControl("lblFregnamount");
        //    TextBox txtPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
        //    TextBox txtExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
        //    TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

        //    //if (lblDueAmt.Text == "")
        //    //{
        //    //    lblDueAmt.Text = "0";
        //    //}
        //    if (txtPayLocal.Text == "")
        //    {
        //        txtPayLocal.Text = "0";
        //    }

        //    //double DueAmount = Convert.ToDouble(lblDueAmt.Text);
        //    //double PayAmount = Convert.ToDouble(txtPayLocal.Text);

        //    if (chkTransId.Checked)
        //    {
        //        if (txtPayLocal.Text == "")
        //        {
        //            txtPayLocal.Text = "0";
        //        }
        //        sumLocalamt += Convert.ToDouble(txtPayLocal.Text);

        //        //if (txtPayForeign.Text == "")
        //        //{
        //        //    txtPayForeign.Text = "0";
        //        //}
        //        //sumForeignAmt += Convert.ToDouble(txtPayForeign.Text);
        //    }

        //    lblInvAmt.Text = SetDecimal(lblInvAmt.Text);
        //    //lblPaidAmt.Text = SetDecimal(lblPaidAmt.Text);
        //    //lblDueAmt.Text = SetDecimal(lblDueAmt.Text);
        //    //lblFregnAmt.Text = SetDecimal(lblFregnAmt.Text);
        //    txtPayLocal.Text = SetDecimal(txtPayLocal.Text);
        //    txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
        //    txtPayForeign.Text = SetDecimal(txtPayForeign.Text);
        //}
        //txtPaidLocalAmount.Text = sumLocalamt.ToString();
        //txtPaidForeignamount.Text = sumForeignAmt.ToString();

        //GetNetAmount();
    }
    protected void txtgvExcahangeRate_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;

        string strFireignExchange = objsys.GetCurencyConversionForInv(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, (float.Parse(((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text) * float.Parse(((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text)).ToString());
        strFireignExchange = strFireignExchange + "/" + ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text;

        ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();
        txtPaidForeignamount.Text += ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text;

        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            CheckBox chkTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkTransId.Checked)
            {
                if (txtPayForeign.Text == "")
                {
                    txtPayForeign.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtPayForeign.Text);
            }
            txtPayForeign.Text = SetDecimal(txtPayForeign.Text);
        }
        txtPaidForeignamount.Text = sumForeignAmt.ToString();
        txtPaidForeignamount.Text = SetDecimal(txtPaidForeignamount.Text);
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = hdnCurrencyId.Value;

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        //try
        //{
        //    strExchangeRate = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(strCurrency).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(strToCurrency.ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
        //}
        //catch
        //{
        //    DataTable dt = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + strToCurrency + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dt.Rows.Count != 0)
        //    {
        //        strExchangeRate = dt.Rows[0]["Currency_Value"].ToString();
        //    }
        //    else
        //    {
        //        strExchangeRate = "0";
        //        //GetAmountDecimal("0");
        //    }
        //}

        try
        {
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    #region List View
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }

            DataTable dtVoucher = (DataTable)Session["dtVoucher"];
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, view.ToTable(), "", "");

            foreach (GridViewRow gvr in GvVoucher.Rows)
            {
                Label lblVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
                lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
            }

            Session["dtSFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }
    protected void GvVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucher.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtSFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtSFilter"];
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
        Session["dtSFilter"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
        //AllPageCode();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);

        Lbl_Tab_New.Text = Resources.Attendance.View;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;
        strLocationId = hdnLocId.Value;
        hdnVoucherId.Value = e.CommandArgument.ToString();

        if (((LinkButton)sender).ID == "btnEdit")
        {
            btnVoucherSave.Visible = true;
            GvPendingInvoice.Enabled = true;
        }
        else if (((LinkButton)sender).ID == "lnkViewDetail")
        {
            btnVoucherSave.Visible = false;
            GvPendingInvoice.Enabled = false;
        }
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
        DataTable dtVDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (((LinkButton)sender).ID == "btnEdit")
            {

                if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
                {
                    DisplayMessage("Voucher is Posted, So record cannot be Edited");
                    return;
                }
                txtPurposeOfTransfer.Text = dtVoucherEdit.Rows[0]["PurposeOfTransfer"].ToString();
                //this validation will check when approval parameter is true for supplier payment voucher

                bool PaymentApproval = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PaymentApproval"));

                if (PaymentApproval)
                {

                    DataTable dtApprovalcheck = new DataView(dtVoucherEdit, "Field3='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtApprovalcheck.Rows.Count > 0)
                    {
                        DisplayMessage("Voucher is Approved, So You Cant be Edited");
                        return;
                    }
                }

                //Get Location by Cash Flow.
                string strLocationIdCash = "0";
                DataTable dtCashLocation = objAccParameterLocation.GetParameterValue_By_NameAndValue("CashFlowLocation", strLocationId);
                if (dtCashLocation.Rows.Count > 0)
                {
                    for (int l = 0; l < dtCashLocation.Rows.Count; l++)
                    {
                        if (strLocationIdCash == "" || strLocationIdCash == "0")
                        {
                            strLocationIdCash = dtCashLocation.Rows[l]["Location_Id"].ToString();
                        }
                        else
                        {
                            strLocationIdCash = strLocationIdCash + "," + dtCashLocation.Rows[l]["Location_Id"].ToString();
                        }
                    }
                }

                //for Cash flow Posted
                //For Cash flow Account
                string strVoucherDate = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(objsys.SetDateFormat());
                string strVoucherNo = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
                string strCashflowPostedV = string.Empty;
                string strAccountIdCash = string.Empty;
                DataTable dtAccountCash = objAccParameterLocation.GetParameterValue_By_ParameterNameforBrand(Session["CompId"].ToString(), Session["BrandId"].ToString(), "CashFlowAccount");
                dtAccountCash = new DataView(dtAccountCash, "Location_Id in (" + strLocationIdCash + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccountCash.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAccountCash.Rows.Count; i++)
                    {
                        if (strAccountIdCash == "")
                        {
                            strAccountIdCash = dtAccountCash.Rows[i]["Param_Value"].ToString();
                        }
                        else
                        {
                            strAccountIdCash = strAccountIdCash + "," + dtAccountCash.Rows[i]["Param_Value"].ToString();
                        }
                    }
                }
                else
                {
                    strAccountIdCash = "0";
                }

                for (int i = 0; i < dtVDetail.Rows.Count; i++)
                {
                    string strAccountNo = dtVDetail.Rows[i]["Account_No"].ToString();
                    if (strAccountIdCash.Split(',').Contains(strAccountNo))
                    {
                        string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True' and  IsActive='True'";
                        DataTable dtCashflowDetail = objDA.return_DataTable(strsql);
                        if (dtCashflowDetail.Rows.Count > 0)
                        {
                            string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                            if (strCashFinalDate != "")
                            {
                                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                                if (dtFinalDate >= DateTime.Parse(strVoucherDate))
                                {
                                    if (strCashflowPostedV == "")
                                    {
                                        strCashflowPostedV = strVoucherNo;
                                    }
                                }
                            }
                        }
                    }
                }

                if (strCashflowPostedV != "")
                {
                    DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPostedV + "");
                    return;
                }

            }




            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            txtVoucherNo.Text = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.Text = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(objsys.SetDateFormat());

            strCashCheque = dtVoucherEdit.Rows[0]["Field2"].ToString();
            if (strCashCheque == "Cash")
            {
                rbCashPayment.Checked = true;
                rbChequePayment.Checked = false;
                rbCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "Cheque")
            {
                rbChequePayment.Checked = true;
                rbCashPayment.Checked = false;
                rbCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "")
            {
                rbCashPayment.Checked = true;
                rbCashPayment_CheckedChanged(null, null);
            }

            string strChequeIssueDate = dtVoucherEdit.Rows[0]["Cheque_Issue_Date"].ToString();
            if (strChequeIssueDate != "" && strChequeIssueDate != "1/1/1800")
            {
                txtChequeIssueDate.Text = Convert.ToDateTime(strChequeIssueDate).ToString(objsys.SetDateFormat());
            }

            string strChequeClearDate = dtVoucherEdit.Rows[0]["Cheque_Clear_Date"].ToString();
            if (strChequeClearDate != "" && strChequeClearDate != "1/1/1800")
            {
                txtChequeClearDate.Text = Convert.ToDateTime(strChequeClearDate).ToString(objsys.SetDateFormat());
            }
            txtChequeNo.Text = dtVoucherEdit.Rows[0]["Cheque_No"].ToString();

            txtReference.Text = dtVoucherEdit.Rows[0]["RefrenceNo"].ToString();
            string strCurrencyId = dtVoucherEdit.Rows[0]["Currency_Id"].ToString();
            ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
            txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            if (dtVoucherEdit.Rows[0]["ref_type"].ToString() == "PO")
            {
                int po_id = 0;
                int.TryParse(dtVoucherEdit.Rows[0]["ref_id"].ToString(), out po_id);
                if (po_id > 0)
                {
                    using (DataTable _dtPo = new PurchaseOrderHeader(Session["DBConnection"].ToString()).GetPoInfoById(po_id.ToString()))
                    {
                        string msg = string.Empty;
                        if (_dtPo.Rows.Count > 0)
                        {
                            msg += "PO Date-";
                            msg += DateTime.Parse(_dtPo.Rows[0]["poDate"].ToString()).ToString("dd-MMM-yyyy");
                            msg += "/Amount=";
                            msg += double.Parse(_dtPo.Rows[0]["po_amount"].ToString()).ToString("0.00");
                            msg += "/Advance(%)=";
                            msg += string.IsNullOrEmpty(_dtPo.Rows[0]["advancePer"].ToString()) ? "0" : _dtPo.Rows[0]["advanceper"].ToString();

                            txtPoInfo.Text = msg;
                            txtPoNumber.Text = _dtPo.Rows[0]["poNo"].ToString() + "/" + po_id.ToString();
                            Session["spvSupplierId"] = _dtPo.Rows[0]["supplierId"].ToString();
                            Session["spvCurrencyId"] = strCurrencyId;
                            divPoInfo.Visible = true;
                        }
                    }
                }
            }

            hdnRef_Id.Value = dtVoucherEdit.Rows[0]["Ref_Id"].ToString();
            hdnRef_Type.Value = dtVoucherEdit.Rows[0]["Ref_Type"].ToString();
            hdnInvoiceNumber.Value = dtVoucherEdit.Rows[0]["Inv_Number"].ToString();
            hdnInvoiceDate.Value = dtVoucherEdit.Rows[0]["Inv_Date"].ToString();

            //For Bank Charges
            DataTable dtBankCharges = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            dtBankCharges = new DataView(dtBankCharges, "Ref_Id='0' and Narration='Bank Charges'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtBankCharges.Rows.Count > 0)
            {
                txtBankCharges.Text = dtBankCharges.Rows[0]["Debit_Amount"].ToString();
                txtBankCharges.Text = SetDecimal(txtBankCharges.Text);
                string strAccountId = dtBankCharges.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtAcbankCharges.Text = strAccountName + "/" + strAccountId;
                }
            }

            //For Bank/Cash Account
            DataTable dtDebit = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            dtDebit = new DataView(dtDebit, "Ref_Id='0' and Field1='CreditSPV'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                hdnFExchangeRate.Value = dtDebit.Rows[0]["Exchange_Rate"].ToString();

                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtAccountNameBank.Text = strAccountName + "/" + strAccountId;
                }
            }

            //for Reconciel Cheque
            //For Bank Account
            string strAccountIdEdit = string.Empty;
            DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "BankAccount");
            if (dtAccount.Rows.Count > 0)
            {
                for (int i = 0; i < dtAccount.Rows.Count; i++)
                {
                    if (strAccountIdEdit == "")
                    {
                        strAccountIdEdit = dtAccount.Rows[i]["Param_Value"].ToString();
                    }
                    else
                    {
                        strAccountIdEdit = strAccountIdEdit + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                    }
                }
            }
            else
            {
                strAccountIdEdit = "0";
            }

            DataTable dtReconcile = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            if (dtReconcile.Rows.Count > 0)
            {
                for (int i = 0; i < dtReconcile.Rows.Count; i++)
                {
                    if (strAccountIdEdit.Split(',').Contains(dtReconcile.Rows[i]["Account_No"].ToString()))
                    {
                        string strStatus = dtReconcile.Rows[i]["Field2"].ToString();
                        if (strStatus == "False")
                        {
                            chkReconcile.Checked = false;
                            chkReconcile.Visible = true;
                        }
                        else if (strStatus == "True")
                        {
                            chkReconcile.Checked = true;
                            chkReconcile.Visible = true;
                        }
                        else
                        {
                            chkReconcile.Checked = false;
                            chkReconcile.Visible = true;
                        }
                    }
                    else
                    {
                        chkReconcile.Checked = false;
                        chkReconcile.Visible = true;
                    }
                }
            }
            else
            {
                chkReconcile.Checked = false;
                chkReconcile.Visible = true;
            }
            string other_account_no = "";
            other_account_no = dtVDetail.Select("other_account_no>0")[0]["other_account_no"].ToString();

            DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(other_account_no);
            FillSupplierBankDetail(_dtTemp.Rows[0]["Trans_Id"].ToString());
            txtSupplierName.Text = _dtTemp.Rows.Count > 0 ? _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString() : "";
            Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            _dtTemp.Dispose();


            //Add Child Concept
            int voucher_id = 0;
            int.TryParse(hdnVoucherId.Value.ToString(), out voucher_id);

            DataTable dtAgeDetail = objDA.return_DataTable("select top 1 * from ac_ageing_detail where voucherid=" + voucher_id);
            if (dtAgeDetail.Rows.Count > 0)
            {
                // other_account_no = dtAgeDetail.Rows[0]["other_account_no"].ToString();
                getAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", GvPendingInvoice, txtSupplierName.Text.Split('/')[1].ToString(), voucher_id.ToString());
                using (DataTable dt = new DataView(dtVDetail, "other_account_no>0", "", DataViewRowState.CurrentRows).ToTable())
                {
                    try
                    {
                        txtExchangeRate.Text = dt.Rows[0]["exchange_rate"].ToString();
                        if (ddlForeginCurrency.SelectedValue == ddlLocalCurrency.SelectedValue)
                        {
                            updateControlsValue(false);
                        }
                        else
                        {
                            updateControlsValue(true);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            //string other_account_no = objDA.get_SingleValue("select top * other_account_no from ac_ageing_detail where voucherid=" + voucher_id);
            else
            {
                chkAdvancePayment.Checked = true;
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
                DataTable dtTemp = new DataTable();
                dtTemp = new DataView(dtVDetail, "other_account_no>0", "", DataViewRowState.CurrentRows).ToTable();
                //other_account_no = dtTemp.Rows[0]["other_account_no"].ToString();

                txtPaidLocalAmount.Text = SetDecimal(dtTemp.Rows[0]["debit_amount"].ToString());
                txtPaidForeignamount.Text = SetDecimal(dtTemp.Rows[0]["Foreign_amount"].ToString());

            }


            //DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, other_account_no);
            //if (dt.Rows.Count > 0)
            //{
            //    txtSupplierName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Supplier_Id"].ToString();
            //    Session["SupplierAccountId"] = dt.Rows[0]["Account_No"].ToString();
            //}
            //string sql = "";
            //DataTable dtDetail = objDA.return_DataTable(sql);
            GetNetAmount();

        }
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);
    }
    protected string GetEmployeeName(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterById(StrCompId, strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //create this validation by jitendra upadhyay on 04-02-2014 
        // here we set validation that the after post the record user can't delete the record
        strLocationId = hdnLocId.Value;
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is posted, So it cannot be Deleted !");
                return;
            }
        }

        //when record approved then we can not delete 
        //validation added by jitendra upadhyay on 01-02-2017


        bool PaymentApproval = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PaymentApproval"));

        if (PaymentApproval)
        {

            DataTable dtApprovalcheck = new DataView(dtVoucherEdit, "Field3='Approved'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtApprovalcheck.Rows.Count > 0)
            {
                DisplayMessage("Voucher is Approved, So You Can not delete");
                return;
            }
        }



        int b = 0;
        hdnVoucherId.Value = e.CommandArgument.ToString();
        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DataTable dtAgeDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtAgeDetail.Rows.Count > 0)
            {
                objAgeingDetail.DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
            }

            DataTable dtApprovalData = da.return_DataTable("Select * from Set_Approval_Transaction where Approval_Id='15' and Ref_Id = '" + hdnVoucherId.Value + "' ");
            if (dtApprovalData.Rows.Count > 0)
            {
                int i = da.execute_Command("Update Set_Approval_Transaction Set IsActive ='0'  Where Approval_Id='15' and Ref_Id = '" + hdnVoucherId.Value + "'");
            }
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        ////FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objsys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    private void FillGrid()
    {
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='False'";
        }

        strLocationId = hdnLocId.Value;
        DataTable dtBrand = objVoucherHeader.GetVoucherHeaderAllTrueWithVoucherDetail(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        if (dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, PostStatus, "", DataViewRowState.CurrentRows).ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        }
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, "Field1='SPV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
            Session["dtVoucher"] = dtBrand;
            Session["dtSFilter"] = dtBrand;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtBrand, "", "");
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
        }

        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    public string GetFinanceCode(string strFinanceId)
    {
        string strFinanceCode = string.Empty;
        DataTable dtFI = objFYI.GetInfoByTransId(StrCompId, strFinanceId);
        if (dtFI.Rows.Count > 0)
        {
            strFinanceCode = dtFI.Rows[0]["Finance_Code"].ToString();
        }
        else
        {
            strFinanceCode = "0";
        }
        return strFinanceCode;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    #endregion

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetSupplierAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Name"].ToString() + "/" + dt.Rows[i]["Bank_Id"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPO(string prefixText, int count, string contextKey)
    {
        string sql = "select poNo,TransID from Inv_PurchaseOrderHeader where IsActive='true' and SupplierId='" + HttpContext.Current.Session["spvSupplierId"].ToString() + "' and CurrencyID='" + HttpContext.Current.Session["spvCurrencyId"].ToString() + "' and poNo like '%" + prefixText + "%'";
        DataTable dtCon = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).return_DataTable(sql);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["poNo"].ToString() + "/" + dtCon.Rows[i]["TransID"].ToString();
            }
        }
        return filterlist;
    }
    #endregion

    protected void btnVoucherSave_Click(object sender, EventArgs e)
    {
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        strLocationId = hdnLocId.Value;


        bool PaymentApproval = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PaymentApproval"));
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());


        if (txtAccountNameBank.Text == "")
        {
            DisplayMessage("Fill Bank/Cash Account");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNameBank);
            return;
        }

        if (objAccParameterLocation.ValidateVoucherForCashFlow(StrCompId, StrBrandId, strLocationId, txtAccountNameBank.Text.Split('/')[1].ToString(), txtVoucherDate.Text) == false)
        {
            DisplayMessage("Cashflow validation error");
            return;
        }
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strChequeIssueDate = string.Empty;
        string strChequeClearDate = string.Empty;
        string strCashCheque = string.Empty;

        if (rbCashPayment.Checked == true)
        {
            strCashCheque = "Cash";
        }
        else if (rbChequePayment.Checked == true)
        {
            strCashCheque = "Cheque";
        }
        else
        {
            DisplayMessage("Choose Any Payment Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(rbCashPayment);
            return;
        }

        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
            return;
        }

        if (GvPendingInvoice.Rows.Count == 0 && chkAdvancePayment.Checked == false)
        {
            DisplayMessage("You Need Atleast One Row in Detail View");
            return;
        }

        if (GvPendingInvoice.Rows.Count == 0)
        {
            if (chkAdvancePayment.Checked == true)
            {
                if (txtPaidLocalAmount.Text == "")
                {
                    DisplayMessage("Fill Local Amount for Advance Pay");
                    return;
                }
            }
        }

        string strCheckBox = string.Empty;
        foreach (GridViewRow gvr in GvPendingInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
            if (chkSelect.Checked == true)
            {
                strCheckBox = "True";
            }
        }

        if (GvPendingInvoice.Rows.Count != 0)
        {
            if (chkAdvancePayment.Checked == false)
            {
                if (strCheckBox != "True")
                {
                    DisplayMessage("You Need To Select Atleast One Row In Grid");
                    return;
                }
            }
        }

        if (txtPaidLocalAmount.Text == "")
        {
            DisplayMessage("Get Paid Amount Local");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaidLocalAmount);
            return;
        }

        if (txtPaidForeignamount.Text == "")
        {
            DisplayMessage("Get Paid Amount Foreign");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaidForeignamount);
            return;
        }

        if (txtBankCharges.Text != "")
        {
            if (txtAcbankCharges.Text != "")
            {

            }
            else
            {
                DisplayMessage("Please Also fill Bank Charge Account");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAcbankCharges);
                return;
            }
        }
        else if (txtAcbankCharges.Text != "")
        {
            if (txtBankCharges.Text != "")
            {

            }
            else
            {
                DisplayMessage("Please Also fill Bank Charges");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCharges);
                return;
            }
        }

        if (ddlForeginCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Foreign Currency");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlForeginCurrency);
            return;
        }


        string strPaymentVoucherAcc = string.Empty;
        DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }

        //For Bank Account
        string strAccountId = objAccParameterLocation.getBankAccounts(StrCompId, StrBrandId, strLocationId);


        string strReconcile = string.Empty;
        if (chkReconcile.Visible == true)
        {
            if (chkReconcile.Checked == true)
            {
                strReconcile = "True";
            }
            else if (chkReconcile.Checked == false)
            {
                strReconcile = "False";
            }
            else
            {
                strReconcile = "False";
            }
        }

        if (txtChequeIssueDate.Text == "")
        {
            strChequeIssueDate = "1/1/1800";
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtChequeIssueDate.Text);
                strChequeIssueDate = txtChequeIssueDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Issue Date in format " + objsys.SetDateFormat() + "");
                txtChequeIssueDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);
                return;
            }
        }

        if (txtChequeClearDate.Text == "")
        {
            strChequeClearDate = "1/1/1800";
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtChequeClearDate.Text);
                strChequeClearDate = txtChequeClearDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Clear Date in format " + objsys.SetDateFormat() + "");
                txtChequeClearDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);
                return;
            }
        }

        //For Check Approval
        string EmpPermission = string.Empty;
        EmpPermission = objsys.Get_Approval_Parameter_By_Name("Supplier Credit").Rows[0]["Approval_Level"].ToString();

        string strApprovalStatus = string.Empty;
        DataTable dtApproval = new DataTable();
        if (PaymentApproval)
        {
            strApprovalStatus = "Pending";
            // Get dt on bases of Permission 

            dtApproval = objApproalEmp.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "303", Session["EmpId"].ToString());

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


        //Here we check Account currency and voucher currency are same or not added by neelkanth purohit - 28/08/18
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (ddlForeginCurrency.SelectedValue.ToString() != dt.Rows[0]["Currency_Id"].ToString())
                    {
                        DisplayMessage("Supplier account currency and voucher currency should same");
                        return;
                    }
                }
            }
        }
        catch { }

        //Here we validate po number added on 03-Nov-2019
        int po_id = 0;
        try
        {
            if (!string.IsNullOrEmpty(txtPoNumber.Text) && chkAdvancePayment.Checked)
            {
                Int32.TryParse(txtPoNumber.Text.Split('/')[1].ToString(), out po_id);
                if (po_id > 0)
                {
                    if (new PurchaseOrderHeader(Session["DBConnection"].ToString()).GetPoInfoById(po_id.ToString()).Rows.Count == 0)
                    {
                        DisplayMessage("Invalid PO Please check again");
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Invalid PO Please check again");
                    return;
                }
            }
        }
        catch (Exception ex)
        {

        }

        string hdnSupplierBankId = "0";
        string hdnBankCurrencyId = "0";
        try
        {
            if (gvBankDetails.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvBankDetails.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkGvBank");
                    HiddenField hdnSupplierAccountId = (HiddenField)row.FindControl("hdnSupplierAccountId");
                    HiddenField hdnGvBankCurrencyId = (HiddenField)row.FindControl("hdnGvBankCurrencyId");

                    if (chk != null && hdnSupplierAccountId != null)
                    {
                        // Check if the checkbox is checked
                        if (chk.Checked)
                        {
                            // Here, you can use the HiddenField value as "true"
                            hdnBankCurrencyId = hdnGvBankCurrencyId.Value;
                            hdnSupplierBankId = hdnSupplierAccountId.Value;
                            // Perform your logic for "true"
                        }

                    }
                }
                if (ddlForeginCurrency.SelectedValue.ToString() != hdnBankCurrencyId)
                {
                    DisplayMessage("Bank Account Currency and ForeginCurrency Should be Same");
                    return;
                }
            }
        }
        catch (Exception ex)
        {

        }

        //--------------------End--------------------------
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            string strCreditAmount = string.Empty;


            //-------------------------By neelkanth purohit 18/feb/2017 for ageing------------------------------------------------------------------
            string strCurrency = hdnCurrencyId.Value;
            Boolean localTransaction;
            double exchangeRate = 0;
            double voucherId = 0;
            localTransaction = (strCurrency == ddlForeginCurrency.SelectedValue ? true : false);
            if (localTransaction == true)
            {
                exchangeRate = 1;
            }
            else
            {
                double.TryParse(hdnFExchangeRate.Value.ToString(), out exchangeRate);
            }

            double paidLocalAmount = 0;
            double.TryParse(txtPaidLocalAmount.Text, out paidLocalAmount);
            double paidForeignAmount = 0;
            double.TryParse(txtPaidForeignamount.Text, out paidForeignAmount);
            txtPaidForeignamount.Text = paidForeignAmount == 0 ? paidLocalAmount.ToString() : paidForeignAmount.ToString();


            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {

                b = objVoucherHeader.UpdateVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), po_id > 0 && chkAdvancePayment.Checked ? po_id.ToString() : hdnRef_Id.Value, po_id > 0 && chkAdvancePayment.Checked ? "PO" : hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlForeginCurrency.SelectedValue.ToString(), txtExchangeRate.Text, txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "SPV", strCashCheque, strApprovalStatus, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), hdnSupplierBankId, txtPurposeOfTransfer.Text, ref trns);

                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);
                objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);




                if (chkAdvancePayment.Checked == false)
                {
                    double.TryParse(hdnVoucherId.Value.ToString(), out voucherId);
                    string strDetailNarration = string.Empty;
                    string strCompanyDCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtPaidLocalAmount.Text);
                    string CompanyDCurrDebit = strCompanyDCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", Session["SupplierAccountId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", strDetailNarration, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyDCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else if (chkAdvancePayment.Checked == true && txtPaidLocalAmount.Text != "")
                {
                    //for Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtPaidLocalAmount.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyCurrDebit, "0.00", "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Ageing Detail Insert
                    //For Debit Entry

                    //------------------commented by neelkanth purohit 18-feb-2017 -----------------------------------------------------
                    //string strAgeCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtPaidLocalAmount.Text);
                    //string AgeCompanyCurrDebit = strAgeCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    //objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PINV", "0", "0", DateTime.Now.ToString(), strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), "0", txtPaidLocalAmount.Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", AgeCompanyCurrDebit, Session["FinanceYearId"].ToString(), "PV", hdnVoucherId.Value, "AdvancePay", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //for Bank Charges
                if (txtBankCharges.Text != "" && txtAcbankCharges.Text != "")
                {
                    //for Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtBankCharges.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtAcbankCharges.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    if (txtPaidLocalAmount.Text != "" && txtPaidLocalAmount.Text != "0")
                    {
                        strCreditAmount = (float.Parse(txtPaidLocalAmount.Text) + float.Parse(txtBankCharges.Text)).ToString();
                    }
                }
                else
                {
                    if (txtPaidLocalAmount.Text != "" && txtPaidLocalAmount.Text != "0")
                    {
                        strCreditAmount = txtPaidLocalAmount.Text;
                    }
                }

                //For Credit Entry

                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), strCreditAmount);
                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(txtAccountNameBank.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, "Payment Voucher", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "CreditSPV", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, "Payment Voucher", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "CreditSPV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Approval Entry                
                if (PaymentApproval)
                {
                    try
                    {
                        objApproalEmp.Delete_Approval_Transaction("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", hdnVoucherId.Value);
                    }
                    catch
                    {

                    }

                    if (dtApproval.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtApproval.Rows.Count; j++)
                        {
                            string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                            string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                            if (EmpPermission == "1")
                            {
                                objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), hdnVoucherId.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "2")
                            {
                                objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), hdnVoucherId.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "3")
                            {
                                objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Session["EmpId"].ToString(), hdnVoucherId.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Session["EmpId"].ToString(), hdnVoucherId.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                }

                DisplayMessage("Voucher Updated Successfully!");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), po_id > 0 && chkAdvancePayment.Checked ? po_id.ToString() : hdnRef_Id.Value, po_id > 0 && chkAdvancePayment.Checked ? "PO" : hdnRef_Type.Value, "0", DateTime.Now.ToString(), txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlForeginCurrency.SelectedValue.ToString(), hdnFExchangeRate.Value, txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "SPV", strCashCheque, strApprovalStatus, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), hdnSupplierBankId, txtPurposeOfTransfer.Text, ref trns);

                string strMaxId = string.Empty;
                DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                if (dtMaxId.Rows.Count > 0)
                {
                    strMaxId = dtMaxId.Rows[0][0].ToString();
                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {

                        string sql = string.Empty;
                        if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is OPC & Jaipur Location
                        {
                            sql = "SELECT count(*) FROM Ac_Voucher_Header where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Voucher_Type='" + ddlVoucherType.SelectedValue + "' and Voucher_No Like '%" + txtVoucherNo.Text + "%'";
                            int strVoucherCount = Int32.Parse(objDA.get_SingleValue(sql, ref trns));

                            if (strVoucherCount == 0)
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + "1", ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + "1";
                            }
                            else
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + strVoucherCount, ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + strVoucherCount;
                            }
                        }
                        else
                        {
                            DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString(), ref trns);
                            if (dtCount.Rows.Count > 0)
                            {
                                dtCount = new DataView(dtCount, "Voucher_Type='" + ddlVoucherType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            if (dtCount.Rows.Count == 0)
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + "1", ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + "1";
                            }
                            else
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + dtCount.Rows.Count, ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + dtCount.Rows.Count;
                            }
                        }
                    }

                    //Add Detail/Ageing Detail Section.
                    objVoucherDetail.DeleteVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, ref trns);


                    if (chkAdvancePayment.Checked == false)
                    {
                        double.TryParse(strMaxId, out voucherId);
                        string strDetailNarration = string.Empty;

                        string strCompanyDCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtPaidLocalAmount.Text);
                        string CompanyDCurrDebit = strCompanyDCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", Session["SupplierAccountId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", strDetailNarration, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyDCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else if (chkAdvancePayment.Checked == true && txtPaidLocalAmount.Text != "")
                    {
                        //for Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtPaidLocalAmount.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                        if (strAccountId.Split(',').Contains(strPaymentVoucherAcc))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtPaidLocalAmount.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                    }

                    //for Bank Charges
                    if (txtBankCharges.Text != "" && txtAcbankCharges.Text != "")
                    {
                        //for Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtBankCharges.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                        if (strAccountId.Split(',').Contains(txtAcbankCharges.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        if (txtPaidLocalAmount.Text != "" && txtPaidLocalAmount.Text != "0")
                        {
                            strCreditAmount = (float.Parse(txtPaidLocalAmount.Text) + float.Parse(txtBankCharges.Text)).ToString();
                        }
                    }
                    else
                    {
                        if (txtPaidLocalAmount.Text != "" && txtPaidLocalAmount.Text != "0")
                        {
                            strCreditAmount = txtPaidLocalAmount.Text;
                        }
                    }

                    //For Credit Entry
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), strCreditAmount);
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtAccountNameBank.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "CreditSPV", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "CreditSPV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //End       


                    //for Approval Entry
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
                                    objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strMaxId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "2")
                                {
                                    objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strMaxId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "3")
                                {
                                    objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Session["EmpId"].ToString(), strMaxId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else
                                {
                                    objApproalEmp.InsertApprovalTransaciton("15", Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Session["EmpId"].ToString(), strMaxId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtReference.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                    }
                }


                DisplayMessage("Voucher Saved Successfully!");


            }

            new Ac_Ageing_Detail_Old(Session["DBConnection"].ToString()).updateAgeingPendingInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, voucherId, GvPendingInvoice, trns);
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            FillGrid();
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));
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


    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        btnNew_Click(null, null);
    }
    public void Reset()
    {
        gvBankDetails.DataSource = null;
        gvBankDetails.DataBind();
        txtPurposeOfTransfer.Text = "";
        txtChequeIssueDate.Text = "";
        txtChequeClearDate.Text = "";
        txtChequeNo.Text = "";
        txtReference.Text = "";
        txtSupplierName.Text = "";

        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlForeginCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }

        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;

        GvPendingInvoice.DataSource = null;
        GvPendingInvoice.DataBind();

        txtNarration.Text = "";
        txtPaidLocalAmount.Text = "";
        txtPaidForeignamount.Text = "";
        txtBankCharges.Text = "";
        txtAcbankCharges.Text = "";
        txtNetAmount.Text = "";
        chkReconcile.Checked = false;
        chkReconcile.Visible = false;

        txtAccountNameBank.Text = "";
        chkAdvancePayment.Checked = false;
        //chkAdvancePayment.Visible = false;
        //chkAdvancePayment.Enabled = true;

        //Extd1.Visible = false;
        //Extd2.Visible = false;
        //Extd3.Visible = false;
        txtExchangeRate.Text = "";

        hdnFExchangeRate.Value = "0";
        hdnVoucherId.Value = "0";
        rbCashPayment.Checked = true;
        rbChequePayment.Checked = false;
        rbCashPayment_CheckedChanged(null, null);
        btnVoucherSave.Visible = true;
        btnAddSupplier.Visible = true;

        txtPoInfo.Text = "";
        txtPoNumber.Text = "";

        hdnRef_Id.Value = "0";
        hdnRef_Type.Value = "0";
        hdnInvoiceNumber.Value = "0";
        hdnInvoiceDate.Value = "0";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtVoucherNo.Text = ViewState["DocNo"].ToString();
        //ViewState["DocNo"] = objDocNo.GetDocumentNo(true, "0", false, "160", "312", "0");
    }
    protected void btnVoucherCancel_Click(object sender, EventArgs e)
    {
        Reset();

        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(StrCompId, strAccountNo);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString();
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }
    protected void ddlgvCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrowtxt = (GridViewRow)((DropDownList)sender).Parent.Parent;

        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();
        txtPaidForeignamount.Text += ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text;

        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            TextBox txtgvPaidForeign = (TextBox)gvrowtxt.FindControl("txtpayforeign");
            TextBox txtExchangeRate = (TextBox)gvrowtxt.FindControl("txtgvExcahangeRate");
            TextBox txtPayForeignAmt = (TextBox)gvrow.FindControl("txtpayforeign");
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (txtPayForeignAmt.Text == "")
                {
                    txtPayForeignAmt.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtPayForeignAmt.Text);
            }
            txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
            txtgvPaidForeign.Text = SetDecimal(txtgvPaidForeign.Text);
        }
        txtPaidForeignamount.Text = sumForeignAmt.ToString();
        txtPaidForeignamount.Text = SetDecimal(txtPaidForeignamount.Text);
    }










    protected void chkTrandId_CheckedChanged(object sender, EventArgs e)
    {
        if (validateInvoiceGrid((GridViewRow)((CheckBox)sender).Parent.Parent, "CheckBox") == false)
        {
            DisplayMessage("Sorry you can't select the invoice having different currency");
        }
    }
    protected Boolean validateInvoiceGrid(GridViewRow gvrowtxt, string objType)
    {
        bool _result = true;
        string currecny_id = "0";



        double dblBalanceAmt = 0;
        double dblPayAmt = 0;
        double.TryParse(((Label)gvrowtxt.FindControl("lblBalanceAmount")).Text, out dblBalanceAmt);
        double.TryParse(((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text, out dblPayAmt);


        if (((CheckBox)gvrowtxt.FindControl("chkTrandId")).Checked)
        {
            ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Enabled = true;
            if (dblPayAmt > dblBalanceAmt)
            {
                ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text = "0";
                _result = false;

            }
            //currecny_id = ((HiddenField)gvrowtxt.FindControl("hdnCurrencyId")).Value;
        }
        else
        {
            ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text = "0";
            ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Enabled = false;
        }



        //double sumForeignamt = 0;
        double sumLocal = 0;
        double exechangeRate = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            TextBox txtPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            //TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            CheckBox chkSelectInvoice = (CheckBox)gvrow.FindControl("chkTrandId");

            if (chkSelectInvoice.Checked)
            {
                if (currecny_id != "0")
                {
                    if (currecny_id != ((HiddenField)gvrow.FindControl("hdnCurrencyId")).Value)
                    {
                        ((CheckBox)gvrowtxt.FindControl("chkTrandId")).Checked = false;
                        ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Enabled = false;
                        //chkSelectInvoice.Checked = false;


                        _result = false;
                        break;
                    }
                }
                if (txtPayLocal.Text == "")
                {
                    txtPayLocal.Text = "0";
                }
                sumLocal += Convert.ToDouble(txtPayLocal.Text);

                currecny_id = ((HiddenField)gvrow.FindControl("hdnCurrencyId")).Value;
                exechangeRate = double.Parse(((Label)gvrow.FindControl("lblExchangeRate")).Text);

            }

        }



        if (sumLocal > 0)
        {
            updateHeaderControls(currecny_id, sumLocal, exechangeRate);
            //ddlForeginCurrency.Enabled = false;
        }

        return _result;
    }

    protected void updateHeaderControls(string currecny_id, double sumLocal, double exechangeRate)
    {
        //string strCurrency = strLocationCurrency;
        strLocationCurrency = hdnCurrencyId.Value;
        txtPaidLocalAmount.Text = "0";
        txtPaidForeignamount.Text = "0";
        txtNetAmount.Text = "0";
        txtExchangeRate.Text = "1";
        ddlForeginCurrency.SelectedIndex = 0;
        ddlForeginCurrency.Enabled = false;
        if (currecny_id != "0")
        {
            if (strLocationCurrency != currecny_id)
            {
                txtPaidLocalAmount.Enabled = false;
                txtPaidForeignamount.Enabled = true;
                txtExchangeRate.Enabled = true;
                ddlForeginCurrency.SelectedValue = currecny_id;
                txtPaidForeignamount.Text = SetDecimal(sumLocal.ToString());
                txtExchangeRate.Text = exechangeRate.ToString();
                hdnFExchangeRate.Value = exechangeRate.ToString();
                txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strLocationCurrency, ((float.Parse(txtPaidForeignamount.Text)) * float.Parse(txtExchangeRate.Text)).ToString());
            }
            else
            {
                txtPaidLocalAmount.Enabled = true;
                txtExchangeRate.Enabled = false;
                txtPaidForeignamount.Enabled = false;
                txtPaidLocalAmount.Text = SetDecimal(sumLocal.ToString());
                txtPaidForeignamount.Text = "0";
                txtExchangeRate.Text = "1";
                ddlForeginCurrency.SelectedValue = currecny_id;
                hdnFExchangeRate.Value = "1";
            }
        }
        GetNetAmount();
    }

    protected void rbCashPayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rbCashPayment.Checked == true)
        {
            rbChequePayment.Checked = false;
            trCheque1.Visible = false;
            trCheque2.Visible = false;
            txtChequeIssueDate.Text = "";
            txtChequeClearDate.Text = "";
            txtChequeNo.Text = "";
        }
        else if (rbChequePayment.Checked == true)
        {
            rbCashPayment.Checked = false;
            trCheque1.Visible = true;
            trCheque2.Visible = true;
        }
    }
    public string GetVoucherAmount(string strVoucherId)
    {
        string strVoucherAmount = string.Empty;
        strLocationId = hdnLocId.Value;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strVoucherId);
        if (dtVoucherD.Rows.Count > 0)
        {
            double debitamount = 0;
            for (int i = 0; i < dtVoucherD.Rows.Count; i++)
            {
                debitamount += Convert.ToDouble(dtVoucherD.Rows[i]["Debit_Amount"].ToString());
            }
            if (debitamount != 0)
            {
                strVoucherAmount = debitamount.ToString();
            }
        }
        else
        {
            strVoucherAmount = "0";
        }
        return strVoucherAmount;
    }
    public string GetSupplierName(string strVoucherId)
    {
        string strSupplierName = string.Empty;
        strLocationId = hdnLocId.Value;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strVoucherId);
        if (dtVoucherD.Rows.Count > 0)
        {
            string strSupId = string.Empty;
            for (int i = 0; i < dtVoucherD.Rows.Count; i++)
            {
                strSupId = dtVoucherD.Rows[i]["Other_Account_No"].ToString();
                if (strSupId != "" && strSupId != "0")
                {
                    break;
                }
            }

            if (strSupId != "" && strSupId != "0")
            {
                DataTable dtContact = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupId);
                if (dtContact.Rows.Count > 0)
                {
                    strSupplierName = dtContact.Rows[0]["Name"].ToString();
                }
            }
            else
            {
                strSupplierName = "";
            }
        }
        else
        {
            strSupplierName = "";
        }
        return strSupplierName;
    }
    protected void ImgDateSearch_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date.");
            txtFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + objsys.SetDateFormat() + "");
                txtFromDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                return;
            }
        }

        DateTime dtToDate = new DateTime();
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date.");
            txtToDate.Focus();
            return;
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtToDate.Text);

                Convert.ToDateTime(txtToDate.Text);
                dtToDate = Convert.ToDateTime(txtToDate.Text);
                dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + objsys.SetDateFormat() + "");
                txtToDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                return;
            }
        }

        if (DateTime.Parse(txtFromDate.Text) >= dtToDate)
        {
            DisplayMessage("From Date should small..");
            txtFromDate.Focus();
            return;
        }

        DataTable dtDate = (DataTable)Session["dtSFilter"];
        if (dtDate.Rows.Count > 0)
        {
            dtDate = new DataView(dtDate, "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDate.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, dtDate, "", "");
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtDate.Rows.Count + "";
                Session["dtSFilter"] = dtDate;
                //AllPageCode();
            }
            else
            {
                GvVoucher.DataSource = null;
                GvVoucher.DataBind();
                DisplayMessage("You have no record according to searching criteria");
            }
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
            DisplayMessage("You have no record according to searching criteria");
        }
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
    }
    protected string GetEmployeeNameByEmpCode(string strEmployeeCode)
    {
        string strEmpName = string.Empty;
        if (strEmployeeCode != "0" && strEmployeeCode != "")
        {
            if (strEmployeeCode == "superadmin")
            {
                strEmpName = "Admin";
            }
            else
            {
                DataTable dtEmp = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeCode);
                if (dtEmp.Rows.Count > 0)
                {
                    strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
                }
            }
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }
    public string SetDecimal(string amount)
    {
        return objsys.GetCurencyConversionForInv(ddlLocalCurrency.SelectedValue, amount);
    }
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Accounts_Report/SupplierPaymentVoucherReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlPendingPaymentDetail.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;
        FillGridBin();
    }
    protected void GvVoucherBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucherBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }

        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["dtInactive"];
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        lblSelectedRecord.Text = "";
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
        //AllPageCode();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        strLocationId = hdnLocId.Value;
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "Field1='SPV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        Session["dtVoucherBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

        if (dt.Rows.Count == 0)
        {

            imgBtnRestore.Visible = false;
        }
        else
        {

            imgBtnRestore.Visible = true;
        }
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmount = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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

            DataTable dtCust = (DataTable)Session["dtVoucherBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }

            foreach (GridViewRow gvr in GvVoucherBin.Rows)
            {
                Label lblVoucherAmount = (Label)gvr.FindControl("lblgvExchangerate");
                lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
            }

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        strLocationId = hdnLocId.Value;
        DataTable dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        if (GvVoucherBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvVoucherBin.Rows)
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
        CheckBox chkSelAll = ((CheckBox)GvVoucherBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvVoucherBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucherBin.Rows[index].FindControl("chkSelect")).Checked)
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
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtVoucher = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtVoucher.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtVocher = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, dtVocher, "", "");
            foreach (GridViewRow gvr in GvVoucherBin.Rows)
            {
                Label lblVoucherAmount = (Label)gvr.FindControl("lblgvExchangerate");
                lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
            }
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        strLocationId = hdnLocId.Value;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {

                    if (lblSelectedRecord.Text.Split(',')[j].Trim() != "" && lblSelectedRecord.Text.Split(',')[j].Trim() != "0")
                    {
                        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderAllFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString());
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + lblSelectedRecord.Text.Split(',')[j].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            string strVoucherDate = dtVoucherHeader.Rows[0]["Voucher_Date"].ToString();
                            if (strVoucherDate != "")
                            {
                                if (!Common.IsFinancialyearAllow(Convert.ToDateTime(strVoucherDate), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                                {
                                    DisplayMessage("Log In Financial year not allowing to perform this action");
                                    return;
                                }
                                else
                                {
                                    b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    DataTable dtAgeDetail = objAgeingDetail.GetAgeingDetailAllTrueFalse(StrCompId, StrBrandId, strLocationId);
                                    dtAgeDetail = new DataView(dtAgeDetail, "Field3='" + hdnVoucherId.Value + "' and IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtAgeDetail.Rows.Count > 0)
                                    {
                                        objAgeingDetail.DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }

                                    DataTable dtApprovalData = da.return_DataTable("Select * from Set_Approval_Transaction where Approval_Id='15' and Ref_Id = '" + lblSelectedRecord.Text.Split(',')[j].Trim() + "' ");
                                    if (dtApprovalData.Rows.Count > 0)
                                    {
                                        int i = da.execute_Command("Update Set_Approval_Transaction Set IsActive ='1'  Where Approval_Id='15' and Ref_Id = '" + lblSelectedRecord.Text.Split(',')[j].Trim() + "'");
                                    }
                                }
                            }
                        }
                    }


                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int flag = 0;
            foreach (GridViewRow Gvr in GvVoucherBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion

    #region SettlemetView
    protected void btnSettle_Click(object sender, EventArgs e)
    {
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlPendingPaymentDetail.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlSettelment.Visible = true;
        PnlAgeingDetail.Visible = false;
        FillGridBin();
    }

    #endregion

    #region AgeingView
    protected void btnAgeingDetail_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlPendingPaymentDetail.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = true;
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        string ref_type = "";
        //string other_account_id = "";
        string strLocationId = HttpContext.Current.Session["LocId"].ToString();
        try
        {
            strLocationId = HttpContext.Current.Session["SupLocId"].ToString();
        }
        catch (Exception ex)
        {

        }
        string other_account_id = HttpContext.Current.Session["CustomerAgeingId"].ToString() == null ? "0" : HttpContext.Current.Session["CustomerAgeingId"].ToString();
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        ref_type = HttpContext.Current.Session["ParentPageId"].ToString() == "1" ? "SINV" : "PINV";
        string sql = "select distinct ref_id,Invoice_no from ac_ageing_detail where Location_id='" + strLocationId + "' and IsActive='true' and ref_type='" + ref_type + "' and other_account_no='" + other_account_id + "' and Invoice_No Like '%" + prefixText + "%'";
        DataTable _temp = objDa.return_DataTable(sql);
        string[] str = new string[_temp.Rows.Count];
        for (int i = 0; i < _temp.Rows.Count; i++)
        {
            str[i] = _temp.Rows[i]["Invoice_no"].ToString() + "/" + _temp.Rows[i]["ref_id"].ToString() + "";

        }
        return str;
    }
    #endregion



    #region PendingPaymentCode

    protected void btnPendingPayment_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;
        pnlPendingPaymentDetail.Visible = true;
        //btnGetPendingList_OnClick(null, null);
    }


    //    protected void btnGetPendingList_OnClick(object sender, EventArgs e)
    //    {
    //        string strLocationId = string.Empty;
    //        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
    //        {
    //            if (strLocationId == "")
    //            {
    //                strLocationId = lstLocationSelect.Items[i].Value;
    //            }
    //            else
    //            {
    //                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
    //            }
    //        }

    //        if (strLocationId.Trim() == "")
    //        {
    //            strLocationId = Session["LocId"].ToString();
    //        }
    //        else
    //        {

    //        }

    //        string strsql = "select MAX(ac_ageing_detail.Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(ac_ageing_detail.Trans_id) asc) as Trans_Id,ac_ageing_detail.Invoice_No,ac_ageing_detail.Invoice_Date,MAX(ac_ageing_detail.Invoice_Amount) as Invoice_Amount,sum(ac_ageing_detail.Paid_Receive_Amount) as Paid_Receive_Amount,   max(ac_ageing_detail.Invoice_Amount)-sum(ac_ageing_detail.Paid_Receive_Amount) as Due_Amount,ac_ageing_detail.Ref_Type,ac_ageing_detail.Ref_Id,ac_ageing_detail.Company_Id,ac_ageing_detail.Brand_Id, ac_ageing_detail.Location_Id, ac_ageing_detail.IsActive,Ems_ContactMaster.Name , DATEADD(day,(select isnull( sum( Set_CustomerMaster_CreditParameter.Credit_Days),0) from Set_CustomerMaster_CreditParameter where Set_CustomerMaster_CreditParameter.Field2='S' and Set_CustomerMaster_CreditParameter.Customer_Id=ac_ageing_detail.Other_Account_No),ac_ageing_detail.Invoice_Date) as paymentDate,DATEDIFF(day,DATEADD(day,(select isnull( sum( Set_CustomerMaster_CreditParameter.Credit_Days),0) from Set_CustomerMaster_CreditParameter where Set_CustomerMaster_CreditParameter.Field2='S' and Set_CustomerMaster_CreditParameter.Customer_Id=ac_ageing_detail.Other_Account_No),ac_ageing_detail.Invoice_Date),getdate()) as Due_Days ,ac_ageing_detail.Other_Account_No, SUBSTRING(Set_LocationMaster.Location_Name,0,20) as Location_Name from ac_ageing_detail inner join Ems_ContactMaster on ac_ageing_detail.Other_Account_No=Ems_ContactMaster.Trans_Id inner join Set_LocationMaster on Ac_Ageing_Detail.Location_Id=Set_LocationMaster.Location_Id   group by ac_ageing_detail.Company_Id,ac_ageing_detail.Brand_Id, ac_ageing_detail.Location_Id, ac_ageing_detail.Ref_Type,ac_ageing_detail.Ref_Id,ac_ageing_detail.Invoice_No,ac_ageing_detail.Invoice_Date,ac_ageing_detail.Other_Account_No,ac_ageing_detail.AgeingType,ac_ageing_detail.IsActive,Ems_ContactMaster.Name,Set_LocationMaster.Location_Name  having max(ac_ageing_detail.Invoice_Amount)-sum(ac_ageing_detail.Paid_Receive_Amount)>0  and ac_ageing_detail.AgeingType='PV' and ac_ageing_detail.Ref_Type='PINV' and ac_ageing_detail.Location_Id in (" + strLocationId + ") and ac_ageing_detail.IsActive='True' order by ac_ageing_detail.Ref_Id ";
    //        DataTable dtPendingPayment = da.return_DataTable(strsql);


    //        if (dtPendingPayment == null)
    //        {
    //            return;
    //        }
    //        if (txtPaymentdate.Text != "")
    //        {
    //            dtPendingPayment = new DataView(dtPendingPayment, "paymentDate='" + Convert.ToDateTime(txtPaymentdate.Text) + "'", "paymentDate", DataViewRowState.CurrentRows).ToTable();
    //        }


    //        string strSupplierName = string.Empty;
    //        try
    //        {
    //            strSupplierName = txtSupplierPendingPayment.Text.Trim().Split('/')[0].ToString();
    //        }
    //        catch
    //        {

    //        }

    //        if (txtSupplierPendingPayment.Text != "")
    //        {
    //            dtPendingPayment = new DataView(dtPendingPayment, "Name='" + strSupplierName + "'", "paymentDate asc", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        if (dtPendingPayment.Rows.Count == 0)
    //        {
    //            cmn.FillData((object)gvPendingPayment, null, "", "");
    //        }
    //        else
    //        {
    //            cmn.FillData((object)gvPendingPayment, dtPendingPayment, "", "");
    //        }
    //    }


    //    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    //    {
    //        GridViewRow gvPaymentRow = (GridViewRow)((Button)sender).Parent.Parent;

    //        string strFireignExchange = GetCurrency(((HiddenField)gvPaymentRow.FindControl("hdnGvCurrencyId")).Value, ((Label)gvPaymentRow.FindControl("lbldueamount")).Text);

    //        if (strFireignExchange == "")
    //        {
    //            strFireignExchange = "0";
    //        }



    //        txtSupplierName.Text = e.CommandName.ToString() + "/" + e.CommandArgument.ToString();
    //        btnAddSupplier_OnClick(null, null);

    //        DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());
    //        if (dt.Rows.Count > 0)
    //        {
    //            txtSupplierName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Supplier_Id"].ToString();
    //            Session["SupplierAccountId"] = dt.Rows[0]["Account_No"].ToString();
    //        }


    //        double sumForeignamt = 0;
    //        double sumLocal = 0;
    //        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
    //        {
    //            TextBox txtPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
    //            TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");


    //            if (((HiddenField)gvPaymentRow.FindControl("hdnRefId")).Value == ((HiddenField)gvrow.FindControl("hdnRefId")).Value)
    //            {
    //                ((CheckBox)gvrow.FindControl("chkTrandId")).Checked = true;
    //            }

    //            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
    //            {
    //                ((TextBox)gvrow.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
    //                ((HiddenField)gvrow.FindControl("hdnExchangeRate")).Value = SetDecimal(strFireignExchange.Trim().Split('/')[1].ToString());
    //                ((TextBox)gvrow.FindControl("txtgvExcahangeRate")).Text = SetDecimal(strFireignExchange.Trim().Split('/')[1].ToString());


    //                txtPayLocal.Text = ((Label)gvrow.FindControl("lbldueamount")).Text;


    //                if (txtPayLocal.Text == "")
    //                {
    //                    txtPayLocal.Text = "0";
    //                }
    //                sumLocal += Convert.ToDouble(txtPayLocal.Text);

    //                if (txtPayForeign.Text == "")
    //                {
    //                    txtPayForeign.Text = "0";
    //                }
    //                sumForeignamt += Convert.ToDouble(txtPayForeign.Text);
    //            }
    //        }
    //        txtPaidLocalAmount.Text = sumLocal.ToString();
    //        txtPaidForeignamount.Text = sumForeignamt.ToString();
    //        txtPaidLocalAmount.Text = SetDecimal(txtPaidLocalAmount.Text);
    //        txtPaidForeignamount.Text = SetDecimal(txtPaidForeignamount.Text);
    //        GetNetAmount();


    //        btnNew_Click(null, null);




    //    }






    //    protected void lblgvVoucherNo_Click(object sender, EventArgs e)
    //    {
    //        string strVoucherType = string.Empty;
    //        LinkButton myButton = (LinkButton)sender;

    //        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
    //        string RefId = arguments[0];
    //        string RefType = arguments[1];
    //        string LocationId = arguments[2].Trim();


    //        if (RefId != "0" && RefId != "")
    //        {
    //            if (RefType.Trim() == "PINV")
    //            {
    //                if (IsObjectPermission("143", "48"))
    //                {
    //                    string strCmd = string.Format("window.open('../Purchase/PurchaseInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    //                }
    //                else
    //                {
    //                    DisplayMessage("You have no permission for view detail");
    //                    return;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //myButton.Attributes.Add("
    //            DisplayMessage("No Data");
    //            return;
    //        }
    //        AllPageCode();
    //    }

    //    public bool IsObjectPermission(string ModuelId, string ObjectId)
    //    {
    //        bool Result = false;
    //        if (Session["EmpId"].ToString() == "0")
    //        {
    //            Result = true;
    //        }
    //        else
    //        {
    //            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId).Rows.Count > 0)
    //            {
    //                Result = true;
    //            }
    //        }
    //        return Result;
    //    }

    //    protected string getLocalAmount(string currency_id,string exchage_rate,string foreing_amount)
    //    {
    //        string localAmount = "0";
    //        if ((currency_id == "" || currency_id == "0") || (exchage_rate == "" || exchage_rate == "0") || (foreing_amount == "" || foreing_amount == "0"))
    //        {
    //            return localAmount;
    //        }

    //        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

    //        if (currency_id != strCurrency)
    //        {
    //                localAmount = objsys.GetCurencyConversionForInv(ddlForeginCurrency.SelectedValue, (float.Parse(txtExchangeRate.Text) * float.Parse(txtPaidForeignamount.Text)).ToString());
    //        }
    //        return localAmount;
    //    }

    //    protected Boolean getAgeingDetail(string strCompanyId, string strBrandId, string strLocationId,string strAgeingType,GridView GvPendingInvoice, string strOtherAccountNo,string strVoucherId)
    //    {
    //        Boolean _result=false;
    //        //if (strAgeingType != "PV" || strAgeingType != "RV")
    //        //{
    //        //    return _result;
    //        //}
    //        double _voucherId=0;
    //        double.TryParse(strVoucherId,out _voucherId);


    //        DataTable dtDetail =objAgeingDetail.getPendingAgeingTable(strCompanyId, StrBrandId, strLocationId, strAgeingType, strOtherAccountNo, strVoucherId,"");


    //        if (dtDetail.Rows.Count > 0)
    //        {
    //            GvPendingInvoice.DataSource = dtDetail;
    //            GvPendingInvoice.DataBind();
    //            DataTable dtAgDetail=new DataTable();
    //            if (_voucherId > 0)
    //            {

    //               string sql = "select * from ac_ageing_detail where voucherId=" + _voucherId +
    //" and Company_Id='" + strCompanyId.ToString() + "' and Brand_Id='" + strBrandId.ToString() + "' and Location_Id='" + strLocationId.ToString() + "'";
    //                dtAgDetail = da.return_DataTable(sql);
    //            }
    //            DataTable dtTemp=new DataTable();
    //            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

    //            foreach (GridViewRow gvS in GvPendingInvoice.Rows)
    //            {

    //                CheckBox chkSelect = (CheckBox)gvS.FindControl("chkTrandId");
    //                chkSelect.Enabled = true;
    //                ((TextBox)gvS.FindControl("txtpayLocal")).Enabled = false;
    //                Label lblInvAmt = (Label)gvS.FindControl("lblinvamount");
    //                Label lblBalanceAmt = (Label)gvS.FindControl("lblBalanceAmount");



    //                if (_voucherId > 0 && dtAgDetail.Rows.Count>0)
    //                {
    //                    //DataRow[] drAgeing;
    //                    dtTemp = new DataView(dtAgDetail, "Invoice_No='" + ((Label)gvS.FindControl("lblPONo")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //                   // drAgeing = dtAgDetail.Select("Invoice_No='" + ((Label)gvS.FindControl("lblPONo")).Text + "'");

    //                    if (dtTemp.Rows.Count > 0)
    //                    {
    //                        if (dtTemp.Rows[0]["Invoice_no"].ToString() == ((Label)gvS.FindControl("lblPONo")).Text)
    //                        {
    //                            chkSelect.Checked = true;
    //                            ((TextBox)gvS.FindControl("txtpayLocal")).Enabled = true;
    //                            if (double.Parse(dtTemp.Rows[0]["Exchange_Rate"].ToString()) == 1 || double.Parse(dtTemp.Rows[0]["Exchange_Rate"].ToString()) == 0)
    //                            {
    //                                ((TextBox)gvS.FindControl("txtpayLocal")).Text = SetDecimal(dtTemp.Rows[0]["Paid_Receive_Amount"].ToString());
    //                            }
    //                            else
    //                            {
    //                                ((TextBox)gvS.FindControl("txtpayLocal")).Text = SetDecimal(dtTemp.Rows[0]["Foreign_Amount"].ToString());
    //                            }
    //                            validateInvoiceGrid(gvS, "TextBox");
    //                        }
    //                    }
    //                }

    //                // Label lblDueAmt = (Label)gvS.FindControl("lbldueamount");
    //                // Label lblForeignAmt = (Label)gvS.FindControl("lblFregnamount");

    //                lblInvAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblInvAmt.Text);
    //                lblBalanceAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblBalanceAmt.Text);
    //                //lblDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDueAmt.Text);
    //                //lblForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblForeignAmt.Text);
    //            }
    //            _result = true;
    //        }
    //        else
    //        {
    //            //chkAdvancePayment.Checked = false;
    //            //txtExchangeRate.Text = "";

    //            GvPendingInvoice.DataSource = null;
    //            GvPendingInvoice.DataBind();
    //            //ddlForeginCurrency.SelectedIndex = 0;
    //            //DisplayMessage("No Record Available for Supplier");
    //            _result = false;
    //        }
    //        return _result;
    //    }

    protected void chkAdvancePayment_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAdvancePayment.Checked == true && ddlForeginCurrency.Enabled == false)
        {
            chkAdvancePayment.Checked = false;
        }
        divPoInfo.Visible = chkAdvancePayment.Checked;
    }
    //    protected DataTable getAgeingTable(string strCompanyId, string strBrandId, string strLocationId, string strAgeingType, string strOtherAccountNo, string strVoucherId)
    //    {
    //        double _voucherId = 0;
    //        double.TryParse(strVoucherId, out _voucherId);
    //        string editSql = "";
    //        if (_voucherId > 0)
    //        {
    //            editSql = "and voucherId<>" + _voucherId + "";
    //        }
    //        string sql = "select Pending_Invoice.*,Sys_CurrencyMaster.Currency_Name," +
    //           " (case when Exchange_Rate<>1 then f_Invoice_Amount else Invoice_Amount end)as actual_Invoice_amt," +
    //" (case when Exchange_Rate<>1 then F_balance_Amount else L_Balance_Amount end)as actual_balance_amt" +
    //           " from (" +
    //" select AgeingType,Account_No,Other_Account_No,Max(Exchange_rate) as Exchange_Rate,MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount," +
    //"sum(due_amount-paid_Receive_Amount) as L_Balance_Amount, " +
    //" MAX(case when Invoice_Amount>0 then Foreign_Amount else 0 end) as f_Invoice_Amount," +
    //" sum(case when paid_Receive_Amount>0 then foreign_amount else 0 end) as F_Receive_Amount, " +
    //" sum(case when due_amount>0 then foreign_amount else 0 end) as F_due_Amount," +
    //" sum((case when due_amount>0 then foreign_amount else 0 end)- (case when paid_Receive_Amount>0 then foreign_amount else 0 end)) as F_balance_Amount," +
    //" sum(due_amount) as Due_Amount,Ref_Type,Ref_Id,Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail where other_account_no='" + strOtherAccountNo.ToString() + "' and AgeingType='" + strAgeingType + "' and IsActive='True' and " +
    //" Company_Id='" + strCompanyId.ToString() + "' and Brand_Id='" + strBrandId.ToString() + "' and Location_Id='" + strLocationId.ToString() + "'" +
    // editSql +
    //" group by Company_Id,Brand_Id, Location_Id,AgeingType, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Account_No,Other_Account_No,AgeingType,IsActive having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0)Pending_Invoice" +
    //" left join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Pending_Invoice.Currency_Id " +
    //" where (Case when Exchange_Rate=1 then L_balance_amount  else  F_balance_Amount end)>0";



    //        DataTable dtDetail = da.return_DataTable(sql);
    //        return dtDetail;
    //    }




    #endregion

    protected void txtPoNumber_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPoNumber.Text))
        {
            return;
        }
        try
        {
            int po_id = 0;
            Int32.TryParse(txtPoNumber.Text.Split('/')[1].ToString(), out po_id);
            if (po_id > 0)
            {
                string msg = string.Empty;
                using (DataTable dt = new PurchaseOrderHeader(Session["DBConnection"].ToString()).GetPoInfoById(po_id.ToString()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        msg += "PO Date-";
                        msg += DateTime.Parse(dt.Rows[0]["poDate"].ToString()).ToString("dd-MMM-yyyy");
                        msg += "/Amount=";
                        msg += double.Parse(dt.Rows[0]["po_amount"].ToString()).ToString("0.00");
                        msg += "/Advance(%)=";
                        msg += string.IsNullOrEmpty(dt.Rows[0]["advancePer"].ToString()) ? "0" : dt.Rows[0]["advancePer"].ToString();


                        double po_amount = 0;
                        double adv_per = 0;
                        double.TryParse(dt.Rows[0]["po_amount"].ToString(), out po_amount);
                        double.TryParse(dt.Rows[0]["advancePer"].ToString(), out adv_per);
                        if (po_amount > 0 && adv_per > 0)
                        {
                            txtPaidForeignamount.Text = (po_amount * adv_per / 100).ToString("0.000");
                        }
                    }
                }
                txtPoInfo.Text = msg;

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        hdnLocId.Value = ddlLocationList.SelectedValue;
        Session["SupLocId"] = hdnLocId.Value;
        hdnCurrencyId.Value = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        strLocationCurrency = hdnCurrencyId.Value;
        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;
        ViewState["DocNo"] = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), hdnLocId.Value, false, "160", "312", "0", Session["BrandId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (hdnVoucherId.Value == "0")
        {
            txtVoucherNo.Text = ViewState["DocNo"].ToString();
        }
        Reset();
        FillGrid();
    }

    public void FillLocationList()
    {
        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["CompId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlLocationList, dtLoc, "Location_Name", "Location_Id");
            ddlLocationList.Items.RemoveAt(0);
            ddlLocationList.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            ddlLocationList.Items.Insert(0, "--Select--");
            ddlLocationList.SelectedIndex = 0;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVoucherNo(string prefixText, int count, string contextKey)
    {
        string ref_type = "";
        //string other_account_id = "";
        string other_account_id = HttpContext.Current.Session["CustomerAgeingId"].ToString() == null ? "0" : HttpContext.Current.Session["CustomerAgeingId"].ToString();
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string strLocationId = string.Empty;
        strLocationId = HttpContext.Current.Session["LocId"].ToString();
        try
        {
            strLocationId = HttpContext.Current.Session["CrvLocId"].ToString();
        }
        catch
        {

        }

        ref_type = HttpContext.Current.Session["ParentPageId"].ToString() == "1" ? "SINV" : "PINV";
        string strVoucherType = HttpContext.Current.Session["ParentPageId"].ToString() == "1" ? "CRV" : "SPV";
        string sql = "select distinct vh.trans_id,vh.voucher_no from ac_ageing_detail ad inner join ac_voucher_header vh on vh.trans_id=ad.voucherId where ad.Location_id='" + strLocationId + "' and ad.IsActive='true' and ad.ref_type='" + ref_type + "' and ad.other_account_no='" + other_account_id + "' and vh.IsActive='true' and vh.voucher_type='" + strVoucherType + "' and vh.voucher_no Like '%" + prefixText + "%'";
        DataTable _temp = objDa.return_DataTable(sql);
        string[] str = new string[_temp.Rows.Count];
        for (int i = 0; i < _temp.Rows.Count; i++)
        {
            str[i] = _temp.Rows[i]["voucher_no"].ToString() + "/" + _temp.Rows[i]["trans_id"].ToString() + "";
        }
        return str;
    }
}