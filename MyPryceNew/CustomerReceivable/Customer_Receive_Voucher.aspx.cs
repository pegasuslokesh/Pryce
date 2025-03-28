using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using net.webservicex.www;
using System.Data;
using System.Data.SqlClient;

public partial class CustomerReceivable_Customer_Receive_Voucher : System.Web.UI.Page
{
    Ems_ContactMaster ObjContactMaster = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    LocationMaster ObjLocation = null;
    SystemParameter objsys = null;
    Set_BankMaster objBank = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Finance_Year_Info objFYI = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ac_ParameterMaster objAccParameter = null;
    EmployeeMaster objEmployee = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Ac_Ageing_Detail_Old objAgeingOldDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;

    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strCurrency = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objAgeingOldDetail = new Ac_Ageing_Detail_Old(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/Customer_Receive_Voucher.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
                ddlLocalCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
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
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            //AllPageCode();
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "313", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Session["CrvLocId"] = strLocationId;
            hdnLocId.Value = Session["LocId"].ToString();
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
    }
    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/CR", "CustomerReceivable", "CustomerReceiveVoucher", e.CommandName.ToString(), e.CommandName.ToString());

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion
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
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
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
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPendingPaymentDetail.Visible = false;
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        btnAddCustomer.Visible = true;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public string SetDecimal(string amount)
    {
        return objsys.GetCurencyConversionForInv(ddlLocalCurrency.SelectedValue, amount);
    }
    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("First Fill Customer Name");
            txtCustomerName.Focus();
            return;
        }

        if (chkAdvancePay.Checked == true)
        {
            DisplayMessage("Sorry you selected advance payment option");
            return;
        }


        ddlForeginCurrency.SelectedIndex = 0;
        int voucher_id = 0;
        int.TryParse(hdnVoucherId.Value.ToString(), out voucher_id);
        strLocationId = hdnLocId.Value;
        if (getAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", GvPendingInvoice, txtCustomerName.Text.Split('/')[1].ToString(), voucher_id.ToString()) == false)
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
            string strCurrency = hdnCurrencyId.Value;

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

    protected void txtpayforeign_OnTextChanged(object sender, EventArgs e)
    {

        double sumForeignamt = 0;
        strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text = "0";
                }
                sumForeignamt += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text);
            }
        }
        txtNetAmountForeign.Text = sumForeignamt.ToString();
        txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtNetAmountForeign.Text);
    }
    protected void txtpayLocal_OnTextChanged(object sender, EventArgs e)
    {
        if (validateInvoiceGrid((GridViewRow)((TextBox)sender).Parent.Parent, "TextBox") == false)
        {
            DisplayMessage("Due Blance Amount is only " + ((Label)((GridViewRow)((TextBox)sender).Parent.Parent).FindControl("lblBalanceAmount")).Text);
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
        txtNetAmountLocal.Text = "0";
        txtNetAmountForeign.Text = "0";

        txtExchangeRate.Text = "1";
        ddlForeginCurrency.SelectedIndex = 0;
        ddlForeginCurrency.Enabled = false;
        if (currecny_id != "0")
        {
            if (SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()) != currecny_id)
            {
                txtNetAmountLocal.Enabled = false;
                txtNetAmountForeign.Enabled = true;
                txtExchangeRate.Enabled = true;
                ddlForeginCurrency.SelectedValue = currecny_id;
                txtNetAmountForeign.Text = SetDecimal(sumLocal.ToString());
                txtExchangeRate.Text = exechangeRate.ToString();
                hdnFExchangeRate.Value = exechangeRate.ToString();
                txtNetAmountLocal.Text = objsys.GetCurencyConversionForInv(SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString()), ((float.Parse(txtNetAmountForeign.Text)) * float.Parse(txtExchangeRate.Text)).ToString());
            }
            else
            {
                txtNetAmountLocal.Enabled = true;
                txtExchangeRate.Enabled = false;
                txtNetAmountForeign.Enabled = false;
                txtNetAmountLocal.Text = SetDecimal(sumLocal.ToString());
                txtNetAmountForeign.Text = "0";
                txtExchangeRate.Text = "1";
                ddlForeginCurrency.SelectedValue = currecny_id;
                hdnFExchangeRate.Value = "1";
            }
        }
        //GetNetAmount();
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        //if (txtCustomerName.Text != "")
        //{
        //    try
        //    {
        //        txtCustomerName.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Customer Name");
        //        txtCustomerName.Text = "";
        //        txtCustomerName.Focus();
        //        return;
        //    }

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Customer Name");
        //        txtCustomerName.Text = "";
        //        txtCustomerName.Focus();
        //    }
        //    else
        //    {
        //        string strCustomerId = txtCustomerName.Text.Trim().Split('/')[1].ToString();
        //        if (strCustomerId != "0" && strCustomerId != "")
        //        {
        //            DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
        //            if (dtCus.Rows.Count > 0)
        //            {
        //                Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
        //            }
        //            else
        //            {
        //                DisplayMessage("First Set Customer Details in Customer Setup");
        //                txtCustomerName.Text = "";
        //                txtCustomerName.Focus();
        //                return;
        //            }

        //            if (Session["CustomerAccountId"].ToString() == "0" && Session["CustomerAccountId"].ToString() == "")
        //            {
        //                DisplayMessage("First Set Customer Account in Customer Setup");
        //                txtCustomerName.Text = "";
        //                txtCustomerName.Focus();
        //                return;
        //            }
        //            else if (Session["CustomerAccountId"].ToString() == "0")
        //            {
        //                DisplayMessage("First Set Customer Account in Customer Setup");
        //                txtCustomerName.Text = "";
        //                txtCustomerName.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Customer Name");
        //    txtCustomerName.Focus();
        //}
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtCustomerName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtCustomerName.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["CustomerAccountId"] = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        return;
                    }

                }
            }
        }
        catch { }
        DisplayMessage("Customer is not valid");
        txtCustomerName.Text = "";
        txtCustomerName.Focus();
    }
    protected void txtNetAmountLocal_OnTextChanged(object sender, EventArgs e)
    {

        updateControlsValue();

    }
    protected void txtNetAmountForeign_OnTextChanged(object sender, EventArgs e)
    {
        updateControlsValue(true);
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

    }
    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForeginCurrency.SelectedIndex > 0)
        {

            hdnFExchangeRate.Value = SystemParameter.GetExchageRate(ddlForeginCurrency.SelectedValue, ddlLocalCurrency.SelectedValue, Session["DBConnection"].ToString());
            //  string  strFireignExchange = hdnFExchangeRate.Value;

            txtExchangeRate.Text = hdnFExchangeRate.Value;




            //line commented by jitendra on 06-03-2017

            if (ddlForeginCurrency.SelectedValue != ddlLocalCurrency.SelectedValue)
            {
                updateControlsValue(true);

                txtNetAmountLocal.Enabled = false;
                txtNetAmountForeign.Enabled = true;



            }
            else
            {
                //hdnFExchangeRate.Value = (1 / double.Parse(hdnFExchangeRate.Value)).ToString("0.000000");
                updateControlsValue();

                txtNetAmountLocal.Enabled = true;
                txtNetAmountForeign.Enabled = false;
            }
        }
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
    protected void txtgvReceiveAmountLocal_OnTextChanged(object sender, EventArgs e)
    {
        strCurrency = hdnCurrencyId.Value;
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        double SumReceiveAmount = 0;
        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            Label lblgvInvoiceAmt = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmt = (Label)gvrow.FindControl("lblpaidamount");
            Label lblDueCheck = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvForeignAmt = (TextBox)gvrow.FindControl("txtgvReceiveAmountForeign");
            TextBox txtReceiveCheck = (TextBox)gvrow.FindControl("txtgvReceiveAmountLocal");
            txtReceiveCheck.Text = SetDecimal(txtReceiveCheck.Text);
            double DueAmount = Convert.ToDouble(lblDueCheck.Text);
            double ReceiveAmount = Convert.ToDouble(txtReceiveCheck.Text);

            if (chkAdvancePay.Visible == true && chkAdvancePay.Checked == true)
            {

            }
            else
            {
                //if (ReceiveAmount > DueAmount)
                //{
                //    DisplayMessage("Enter Amount According to Due Amount");
                //    return;
                //}
            }

            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (txtReceiveCheck.Text == "")
                {
                    txtReceiveCheck.Text = "0";
                }
                SumReceiveAmount += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountLocal")).Text);

                if (txtgvForeignAmt.Text == "")
                {
                    txtgvForeignAmt.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtgvForeignAmt.Text);
            }

            lblgvInvoiceAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmt.Text);
            lblgvPaidAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmt.Text);
            lblDueCheck.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDueCheck.Text);
            txtReceiveCheck.Text = objsys.GetCurencyConversionForInv(strCurrency, txtReceiveCheck.Text);
            txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
            txtgvForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvForeignAmt.Text);
        }
        txtNetAmountLocal.Text = SetDecimal(SumReceiveAmount.ToString());
        txtNetAmountForeign.Text = SetDecimal(sumForeignAmt.ToString());
        txtNetAmountLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtNetAmountLocal.Text);
        txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtNetAmountForeign.Text);
    }
    protected void txtgvExcahangeRate_OnTextChanged(object sender, EventArgs e)
    {
        strCurrency = hdnCurrencyId.Value;
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;

        string strFireignExchange = objsys.GetCurencyConversionForInv(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, (float.Parse(((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text) * float.Parse(((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountLocal")).Text)).ToString());
        strFireignExchange = strFireignExchange + "/" + ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text;

        ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();
        txtNetAmountForeign.Text += ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text;

        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text);
            }
        }

        txtNetAmountForeign.Text = sumForeignAmt.ToString();
        txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtNetAmountForeign.Text);
    }

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False' and AccountName like '%" + prefixText + "%'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        //string filtertext = "AccountName like '%" + prefixText + "%'";
        //dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

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
    #endregion

    protected void btnVoucherSave_Click(object sender, EventArgs e)
    {
        string voucherId = string.Empty;
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        //Get Location by Cash Flow.
        string strLocationIdCash = string.Empty;
        strLocationId = hdnLocId.Value;
        DataTable dtCashLocation = objAccParameterLocation.GetParameterValue_By_NameAndValue("CashFlowLocation", strLocationId);
        if (dtCashLocation.Rows.Count > 0)
        {
            for (int l = 0; l < dtCashLocation.Rows.Count; l++)
            {
                if (strLocationIdCash == "")
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
        string strCashflowPostedV = string.Empty;
        string strAccountIdCash = string.Empty;
        DataTable dtAccountCash = objAccParameterLocation.GetParameterValue_By_ParameterNameforBrand(Session["CompId"].ToString(), Session["BrandId"].ToString(), "CashFlowAccount");
        if (dtAccountCash.Rows.Count > 0)
        {
            if (strLocationIdCash != "")
            {
                dtAccountCash = new DataView(dtAccountCash, "Location_Id in (" + strLocationIdCash + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
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

        try
        {
            if (strAccountIdCash.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
            {
                string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True' and  IsActive='True' ";
                DataTable dtCashflowDetail = objDA.return_DataTable(strsql);
                if (dtCashflowDetail.Rows.Count > 0)
                {
                    string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                    if (strCashFinalDate != "")
                    {
                        DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);
                        if (dtFinalDate >= DateTime.Parse(txtVoucherDate.Text))
                        {
                            if (strCashflowPostedV == "")
                            {
                                strCashflowPostedV = txtVoucherNo.Text;
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }

        if (strCashflowPostedV != "")
        {
            DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPostedV + "");
            return;
        }

        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strCashCheque = string.Empty;
        string strChequeIssueDate = string.Empty;
        string strChequeClearDate = string.Empty;

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

        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
            return;
        }

        if (GvPendingInvoice.Rows.Count == 0 && chkAdvancePay.Checked == false)
        {
            DisplayMessage("You Need Atleast One Row in Detail View Otherwise You can do it Advance Payment");
            return;
        }

        if (GvPendingInvoice.Rows.Count == 0)
        {
            if (chkAdvancePay.Checked == true)
            {
                if (txtNetAmountLocal.Text == "")
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
            if (chkAdvancePay.Checked == false)
            {
                if (strCheckBox != "True")
                {
                    DisplayMessage("You Need To Select Atleast One Row In Grid");
                    return;
                }
            }
        }

        if (txtNetAmountLocal.Text == "")
        {
            DisplayMessage("Get Net Amount Local");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNetAmountLocal);
            return;
        }

        if (chkAdvancePay.Checked == false)
        {
            if (txtNetAmountForeign.Text == "")
            {
                DisplayMessage("Get Net Amount Foreign");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNetAmountForeign);
                return;
            }
        }

        if (txtDebitAccountName.Text == "")
        {
            DisplayMessage("Fill Debit Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAccountName);
            return;
        }

        if (ddlForeginCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Foreign Currency");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlForeginCurrency);
            return;
        }

        string strReceiveVoucherAcc = string.Empty;
        DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
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


        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "BankAccount");
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


        //Here we check Account currency and voucher currency are same or not added by neelkanth purohit - 28/08/18
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtCustomerName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (ddlForeginCurrency.SelectedValue.ToString() != dt.Rows[0]["Currency_Id"].ToString())
                    {
                        DisplayMessage("Customer account currency and voucher currency should same");
                        return;
                    }
                }
            }
        }
        catch { }
        //--------------------End--------------------------

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strForeignAmount = "0";
        strForeignAmount = txtExchangeRate.Text == "1" && txtNetAmountForeign.Text == "0" ? txtNetAmountLocal.Text : txtNetAmountForeign.Text;
        try
        {
            int b = 0;
            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {
                b = objVoucherHeader.UpdateVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), hdnRef_Id.Value, hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlLocalCurrency.SelectedValue.ToString(), "0", txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "CRV", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                voucherId = hdnVoucherId.Value;
                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);
                objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);


                if (chkAdvancePay.Checked == false)
                {
                    string strDetailNarration = string.Empty;
                    foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                    {
                        Label lblgvInvoiceNo = (Label)gvr.FindControl("lblPONo");
                        HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");

                        if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                        {
                            //Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                            //for Credit
                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                            if (strDetailNarration == "")
                            {
                                strDetailNarration = "In this Voucher Settled Invoices are " + lblgvInvoiceNo.Text;
                            }
                            else
                            {
                                strDetailNarration = strDetailNarration + "/" + lblgvInvoiceNo.Text;
                            }

                            //For Ageing Detail Insert
                            // objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, lblgvInvoiceNo.Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", hdnVoucherId.Value, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    string strCompanyCrrDValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrDCredit = strCompanyCrrDValueCr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, strDetailNarration, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrDCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                else if (chkAdvancePay.Checked == true && txtNetAmountLocal.Text != "")
                {
                    //for Credit
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(strReceiveVoucherAcc))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrCredit, "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Ageing Detail Insert
                    // objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SINV", "0", "0", DateTime.Now.ToString(), strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", txtNetAmountLocal.Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", hdnVoucherId.Value, "AdvancePay", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                DisplayMessage("Voucher Updated successfully!");
                btnList_Click(sender, e);
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), "0", "0", "0", DateTime.Now.ToString(), txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlForeginCurrency.SelectedValue.ToString(), txtExchangeRate.Text, txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "CRV", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


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

                    voucherId = strMaxId;

                    //Add Detail/Ageing Detail Section.
                    objVoucherDetail.DeleteVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, ref trns);

                    if (chkAdvancePay.Checked == false)
                    {
                        string strDetailNarration = string.Empty;
                        foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                        {
                            HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                            Label lblgvInvoiceNo = (Label)gvr.FindControl("lblPONo");

                            if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                            {
                                //Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                                //for Credit
                                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                                //if (strAccountId.Split(',').Contains(Session["CustomerAccountId"].ToString()))
                                //{
                                //    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblgvSerialNo.Text, Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                //}
                                //else
                                //{
                                //    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblgvSerialNo.Text, Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                //}

                                if (strDetailNarration == "")
                                {
                                    strDetailNarration = "In this Voucher Settled Invoices are " + lblgvInvoiceNo.Text;
                                }
                                else
                                {
                                    strDetailNarration = strDetailNarration + "/" + lblgvInvoiceNo.Text;
                                }

                                //For Ageing Detail Insert
                                //objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, lblgvInvoiceNo.Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", strMaxId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        string strCompanyCrrDValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrDCredit = strCompanyCrrDValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, strDetailNarration, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrDCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        if (strAccountId.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    else if (chkAdvancePay.Checked == true && txtNetAmountLocal.Text != "")
                    {
                        //For Credit
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                        //For Ageing Detail Insert
                        if (strAccountId.Split(',').Contains(Session["CustomerAccountId"].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        //objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SINV", "0", "0", DateTime.Now.ToString(), strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", txtNetAmountLocal.Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", strMaxId, "AdvancePay", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                        if (strAccountId.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, strForeignAmount, txtNetAmountForeign.Text, CompanyCurrDebit, "0.00", "DebitCRV", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, strForeignAmount, CompanyCurrDebit, "0.00", "DebitCRV", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //End             
                }
                DisplayMessage("Voucher Saved successfully!");
            }


            if (objAgeingOldDetail.updateAgeingPendingInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Convert.ToDouble(voucherId), GvPendingInvoice, trns) == true)
            {

            }
            // new Ac_Ageing_Detail_Old(Session["DBConnection"].ToString()).updateAgeingPendingInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Convert.ToDouble(voucherId), GvPendingInvoice, trns);


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            FillGrid();
            Reset();
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
        rbCashPayment.Checked = true;
        rbChequePayment.Checked = false;
        rbCashPayment_CheckedChanged(null, null);

        txtChequeIssueDate.Text = "";
        txtChequeClearDate.Text = "";
        txtChequeNo.Text = "";
        txtReference.Text = "";
        txtCustomerName.Text = "";

        GvPendingInvoice.DataSource = null;
        GvPendingInvoice.DataBind();

        chkAdvancePay.Checked = false;
        //chkAdvancePay.Visible = false;

        txtNetAmountLocal.Text = "";
        txtNetAmountForeign.Text = "";
        hdnVoucherId.Value = "0";
        //txtAdvanceAmount.Text = "";
        btnVoucherSave.Visible = true;
        btnAddCustomer.Visible = true;
        txtDebitAccountName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtNarration.Text = "";

        hdnRef_Id.Value = "0";
        hdnRef_Type.Value = "0";
        hdnInvoiceNumber.Value = "0";
        hdnInvoiceDate.Value = "0";

        txtVoucherNo.Text = ViewState["DocNo"].ToString();
        //ViewState["DocNo"] = objDocNo.GetDocumentNo(true, "0", false, "160", "313", "0");
        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;

        txtNetAmountLocal.Enabled = true;
        txtNetAmountForeign.Enabled = true;
    }
    protected void btnVoucherCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
        strCurrency = hdnCurrencyId.Value;
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
            Session["dtCFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            // AllPageCode();

            foreach (GridViewRow gvr in GvVoucher.Rows)
            {
                Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
                lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
            }
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }
    protected void GvVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        strCurrency = hdnLocId.Value;
        GvVoucher.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        strCurrency = hdnLocId.Value;
        DataTable dt = (DataTable)Session["dtCFilter"];
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
        Session["dtCFilter"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
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

        strLocationId = hdnLocId.Value;
        strCurrency = hdnCurrencyId.Value;
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;

        hdnVoucherId.Value = e.CommandArgument.ToString();

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
        DataTable dtVDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited, You Can View Only");
                return;
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

            if (strCashflowPostedV != "" && ((LinkButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPostedV + "");
                return;
            }


            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            string strFinanceCode = dtVoucherEdit.Rows[0]["Finance_Code"].ToString();
            string strToLocationId = dtVoucherEdit.Rows[0]["Location_To"].ToString();
            string strDepartmentId = dtVoucherEdit.Rows[0]["Department_Id"].ToString();

            txtVoucherNo.Text = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.Text = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(objsys.SetDateFormat());

            strVoucherType = dtVoucherEdit.Rows[0]["Voucher_Type"].ToString();
            if (strVoucherType != "0")
            {
                ddlVoucherType.SelectedValue = strVoucherType;
            }
            else
            {
                ddlVoucherType.SelectedValue = "--Select--";
            }

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
            ddlLocalCurrency.SelectedValue = strCurrencyId;
            txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            hdnRef_Id.Value = dtVoucherEdit.Rows[0]["Ref_Id"].ToString();
            hdnRef_Type.Value = dtVoucherEdit.Rows[0]["Ref_Type"].ToString();
            hdnInvoiceNumber.Value = dtVoucherEdit.Rows[0]["Inv_Number"].ToString();
            hdnInvoiceDate.Value = dtVoucherEdit.Rows[0]["Inv_Date"].ToString();

            //For Debit Account
            DataTable dtDebit = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

            //Get customer name detail
            DataTable dtOtherAccount = new DataView(dtDebit, "other_account_no>0", "", DataViewRowState.CurrentRows).ToTable();
            if (dtOtherAccount.Rows.Count > 0)
            {
                DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(dtOtherAccount.Rows[0]["Other_Account_No"].ToString());
                txtCustomerName.Text = _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString();
                Session["CustomerAccountId"] = dtOtherAccount.Rows[0]["Account_No"].ToString();
                _dtTemp.Dispose();
                txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(strCurrencyId, dtOtherAccount.Rows[0]["foreign_amount"].ToString());
                txtNetAmountLocal.Text = objsys.GetCurencyConversionForInv(strCurrencyId, dtOtherAccount.Rows[0]["Credit_amount"].ToString());
            }
            dtOtherAccount.Dispose();
            //----------------end----------------


            dtDebit = new DataView(dtDebit, "Ref_Id='0' and Field1='DebitCRV'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                hdnFExchangeRate.Value = dtDebit.Rows[0]["Exchange_Rate"].ToString();
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtExchangeRate.Text);
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtDebitAccountName.Text = strAccountName + "/" + strAccountId;
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



            //Add Child Concept

            int voucher_id = 0;
            int.TryParse(hdnVoucherId.Value.ToString(), out voucher_id);
            string other_account_no = "";
            DataTable dtAgeDetail = objDA.return_DataTable("select top 1 * from ac_ageing_detail where voucherid=" + voucher_id);
            if (dtAgeDetail.Rows.Count > 0)
            {
                //objAcAccountMaster.GetAc_AccountMasterByTransId(dtAgeDetail.Rows[0]["Other_Account_No"].ToString());
                //DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(StrCompId, StrBrandId, dtAgeDetail.Rows[0]["Other_Account_No"].ToString());
                //if (dt.Rows.Count > 0)
                //{
                //    txtCustomerName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
                //    Session["CustomerAccountId"] = dt.Rows[0]["Account_No"].ToString();
                //}
                other_account_no = dtAgeDetail.Rows[0]["other_account_no"].ToString();
                if (getAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", GvPendingInvoice, other_account_no.ToString(), voucher_id.ToString()) == false)
                {
                    //DisplayMessage("No Record Found");
                }

            }

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
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin(); //Update grid view in bin tab
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
        strLocationId = hdnLocId.Value;
        strCurrency = hdnCurrencyId.Value;
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='False'";
        }

        DataTable dtBrand = objVoucherHeader.GetVoucherHeaderAllTrueWithVoucherDetail(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        if (dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, PostStatus, "", DataViewRowState.CurrentRows).ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        }

        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, "Field1='CRV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
            Session["dtVoucher"] = dtBrand;
            Session["dtCFilter"] = dtBrand;
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
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
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

    #endregion
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
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = hdnCurrencyId.Value;

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());

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
    protected void ddlgvCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        strCurrency = hdnCurrencyId.Value;
        GridViewRow gvrowtxt = (GridViewRow)((DropDownList)sender).Parent.Parent;

        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        txtNetAmountForeign.Text += ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text;

        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text);
            }
        }
        txtNetAmountForeign.Text = sumForeignAmt.ToString();
        txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtNetAmountForeign.Text);
    }
    protected void chkTrandId_CheckedChanged(object sender, EventArgs e)
    {
        if (validateInvoiceGrid((GridViewRow)((CheckBox)sender).Parent.Parent, "CheckBox") == false)
        {
            DisplayMessage("Sorry you can't select the invoice having different currency");
        }

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
    public string GetVoucherAmount(string strVoucherId)
    {
        string strVoucherAmount = string.Empty;
        strLocationId = hdnLocId.Value;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strVoucherId);
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

        DataTable dtDate = (DataTable)Session["dtCFilter"];
        if (dtDate.Rows.Count > 0)
        {
            dtDate = new DataView(dtDate, "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDate.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, dtDate, "", "");
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtDate.Rows.Count + "";
                Session["dtCFilter"] = dtDate;
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

        strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
        }
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
            txtNetAmountForeign.Enabled = false;
        }
        else
        {
            txtNetAmountForeign.Enabled = true;
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
            double.TryParse(txtNetAmountLocal.Text, out trns_amount);
            //trns_amount = debit;
            txtNetAmountForeign.Text = GetCurrency(ddlLocalCurrency.SelectedValue, (exchange_rate * trns_amount).ToString());
            txtNetAmountForeign.Text = txtNetAmountForeign.Text.Trim().Split('/')[0].ToString();
        }
        else
        {
            //line added by jitendra on 06-03-2017


            //code start

            double.TryParse((exchange_rate).ToString("0.000000"), out exchange_rate);

            //code end
            double.TryParse(txtNetAmountForeign.Text, out foreign);
            txtNetAmountLocal.Text = GetCurrency(ddlLocalCurrency.SelectedValue, (exchange_rate * foreign).ToString());
            txtNetAmountLocal.Text = txtNetAmountLocal.Text.Trim().Split('/')[0].ToString();
        }

        txtExchangeRate.Text = exchange_rate.ToString();
        hdnFExchangeRate.Value = exchange_rate.ToString();

    }
    #region Print

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Accounts_Report/CustomerReceiveVoucherReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
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
        strCurrency = Session["LocCurrencyId"].ToString();
        GvVoucherBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
        }

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
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        strCurrency = Session["LocCurrencyId"].ToString();
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
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
        }
        lblSelectedRecord.Text = "";
        // AllPageCode();
    }
    public void FillGridBin()
    {
        strCurrency = hdnCurrencyId.Value;
        strLocationId = hdnLocId.Value;
        DataTable dt = new DataTable();
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "Field1='CRV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
        }
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
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        strCurrency = Session["LocCurrencyId"].ToString();
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
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            foreach (GridViewRow gvr in GvVoucherBin.Rows)
            {
                Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
                lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
            }
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
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
                Label lblgvVoucherAmount = (Label)gvr.FindControl("lblgvVoucherAmount");
                lblgvVoucherAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmount.Text);
            }
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        strLocationId = hdnLocId.Value;
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j].Trim() != "")
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
    //protected void txtSettleCustomer_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtSettleCustomer.Text != "")
    //    {
    //        try
    //        {
    //            txtSettleCustomer.Text.Split('/')[1].ToString();
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Customer Name");
    //            txtSettleCustomer.Text = "";
    //            GVSettleMentCredit.DataSource = null;
    //            GVSettleMentCredit.DataBind();
    //            GVSettleMentDebit.DataSource = null;
    //            GVSettleMentDebit.DataBind();
    //            SettleCR.Visible = false;
    //            SettleDR.Visible = false;
    //            txtSettleCustomer.Focus();
    //            return;
    //        }

    //        DataTable dt = ObjContactMaster.GetContactByContactName(txtSettleCustomer.Text.Trim().Split('/')[0].ToString());
    //        if (dt.Rows.Count == 0)
    //        {
    //            DisplayMessage("Select Customer Name");
    //            txtSettleCustomer.Text = "";
    //            GVSettleMentCredit.DataSource = null;
    //            GVSettleMentCredit.DataBind();
    //            GVSettleMentDebit.DataSource = null;
    //            GVSettleMentDebit.DataBind();
    //            SettleCR.Visible = false;
    //            SettleDR.Visible = false;
    //            txtSettleCustomer.Focus();
    //        }
    //        else
    //        {
    //            string strCustomerId = txtSettleCustomer.Text.Trim().Split('/')[1].ToString();
    //            if (strCustomerId != "0" && strCustomerId != "")
    //            {
    //                DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
    //                if (dtCus.Rows.Count > 0)
    //                {
    //                    Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
    //                }
    //                else
    //                {
    //                    DisplayMessage("First Set Customer Details in Customer Setup");
    //                    txtSettleCustomer.Text = "";
    //                    txtSettleCustomer.Focus();
    //                    return;
    //                }

    //                if (Session["CustomerAccountId"].ToString() == "0" && Session["CustomerAccountId"].ToString() == "")
    //                {
    //                    DisplayMessage("First Set Customer Account in Customer Setup");
    //                    txtSettleCustomer.Text = "";
    //                    txtSettleCustomer.Focus();
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("Select Customer Name");
    //        GVSettleMentCredit.DataSource = null;
    //        GVSettleMentCredit.DataBind();
    //        GVSettleMentDebit.DataSource = null;
    //        GVSettleMentDebit.DataBind();
    //        SettleCR.Visible = false;
    //        SettleDR.Visible = false;
    //        txtSettleCustomer.Focus();
    //    }
    //}
    //protected void btnSettleCustomerAdd_OnClick(object sender, EventArgs e)
    //{
    //    if (txtSettleCustomer.Text != "")
    //    {
    //        //For Settlement Credit
    //        //string sqlC = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount, max(Paid_Receive_Amount)-sum(Due_Amount) as Due_Amount,Ref_Type,Ref_Id, Company_Id,Brand_Id,Field1, Location_Id, IsActive  from ac_ageing_detail   group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,Field1,AgeingType,IsActive having max(Paid_Receive_Amount)-sum(Due_Amount)>0 and other_account_no='" + txtSettleCustomer.Text.Split('/')[1].ToString() + "' and Field1='AdvancePay' and AgeingType='RV' and IsActive='True'";
    //        //DataTable dtCreditDetail = da.return_DataTable(sqlC);
    //        //if (dtCreditDetail.Rows.Count > 0)
    //        //{
    //        //    dtCreditDetail = new DataView(dtCreditDetail, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        //}

    //        DataTable dtCreditDetail = objAgeingDetail.GetAgeingDetailAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
    //        if (dtCreditDetail.Rows.Count > 0)
    //        {
    //            DataTable dtFilterd = new DataView(dtCreditDetail, "other_account_no='" + txtSettleCustomer.Text.Split('/')[1].ToString() + "' and Field1='AdvancePay' and AgeingType='RV' and Paid_Receive_Amount> Due_Amount and Field2=''", "", DataViewRowState.CurrentRows).ToTable();
    //            if (dtFilterd.Rows.Count == 0)
    //            {
    //                dtCreditDetail = new DataView(dtCreditDetail, "other_account_no='" + txtSettleCustomer.Text.Split('/')[1].ToString() + "' and Ref_Type='SO' and AgeingType='RV' and Paid_Receive_Amount> Due_Amount and Field2=''", "", DataViewRowState.CurrentRows).ToTable();
    //            }
    //            else
    //            {
    //                dtCreditDetail = dtFilterd;
    //            }
    //        }

    //        if (dtCreditDetail.Rows.Count > 0)
    //        {
    //            GVSettleMentCredit.DataSource = dtCreditDetail;
    //            GVSettleMentCredit.DataBind();
    //            SettleCR.Visible = true;
    //        }
    //        else
    //        {
    //            GVSettleMentCredit.DataSource = null;
    //            GVSettleMentCredit.DataBind();
    //            SettleCR.Visible = false;
    //        }

    //        //For Settlement Debit
    //        string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id, Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail   group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,IsActive  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtSettleCustomer.Text.Split('/')[1].ToString() + "' and AgeingType='RV' and IsActive='True'";
    //        //da.return_DataTable(sql);
    //        DataTable dtDetail = da.return_DataTable(sql);
    //        if (dtDetail.Rows.Count > 0)
    //        {
    //            dtDetail = new DataView(dtDetail, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        if (dtDetail.Rows.Count > 0)
    //        {
    //            GVSettleMentDebit.DataSource = dtDetail;
    //            GVSettleMentDebit.DataBind();
    //            SettleDR.Visible = true;
    //            ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
    //            ddlForeginCurrency_SelectedIndexChanged(sender, e);
    //            foreach (GridViewRow gvr in GVSettleMentDebit.Rows)
    //            {
    //                //CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
    //                DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");
    //                //chkSelect.Enabled = true;

    //                DataTable dsCurrency = null;
    //                dsCurrency = objCurrency.GetCurrencyMaster();
    //                if (dsCurrency.Rows.Count > 0)
    //                {
    //                    cmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
    //                    ddlgvCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
    //                }

    //                Label lblInvoiceAmount = (Label)gvr.FindControl("lblinvamount");
    //                Label lblReceivedAmt = (Label)gvr.FindControl("lblpaidamount");
    //                Label lblPaidAmt = (Label)gvr.FindControl("lbldueamount");

    //                lblInvoiceAmount.Text = SetDecimal(lblInvoiceAmount.Text);
    //                lblReceivedAmt.Text = SetDecimal(lblReceivedAmt.Text);
    //                lblPaidAmt.Text = SetDecimal(lblPaidAmt.Text);
    //            }
    //        }
    //        else
    //        {
    //            GVSettleMentDebit.DataSource = null;
    //            GVSettleMentDebit.DataBind();
    //            SettleDR.Visible = false;
    //            DisplayMessage("No Record Available for Customer");
    //            ddlForeginCurrency.SelectedIndex = 0;
    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("Fill Customer Name");
    //        txtSettleCustomer.Focus();
    //    }

    //    strCurrency = Session["LocCurrencyId"].ToString();
    //    foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //    {
    //        Label lblgvPaidAmt = (Label)gvr.FindControl("lblpaidamount");
    //        Label lblgvDueAmt = (Label)gvr.FindControl("lbldueamount");

    //        lblgvPaidAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmt.Text);
    //        lblgvDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmt.Text);
    //    }

    //    foreach (GridViewRow gvr in GVSettleMentDebit.Rows)
    //    {
    //        Label lblgvInvAmt = (Label)gvr.FindControl("lblinvamount");
    //        Label lblgvPaidAmt = (Label)gvr.FindControl("lblpaidamount");
    //        Label lblgvDueAmt = (Label)gvr.FindControl("lbldueamount");
    //        TextBox txtgvSettleAmt = (TextBox)gvr.FindControl("txtgvSettleAmount");
    //        TextBox txtgvExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
    //        TextBox txtgvReceiveAmount = (TextBox)gvr.FindControl("txtgvReceiveAmountForeign");

    //        lblgvInvAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvAmt.Text);
    //        lblgvPaidAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmt.Text);
    //        lblgvDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmt.Text);
    //        txtgvSettleAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvSettleAmt.Text);
    //        txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
    //        txtgvReceiveAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvReceiveAmount.Text);
    //    }
    //}
    //protected void txtgvSettleAmount_OnTextChanged(object sender, EventArgs e)
    //{
    //    string strCurrency = Session["LocCurrencyId"].ToString();

    //    if (GVSettleMentCredit.Rows.Count == 0)
    //    {
    //        DisplayMessage("You have no Advance Amount for Settle");
    //        txtSettleCustomer.Focus();
    //        return;
    //    }

    //    string strCheckValue = "False";
    //    foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //    {
    //        CheckBox chkCreditValue = (CheckBox)gvr.FindControl("chkSettleCeditId");
    //        if (chkCreditValue.Checked == true)
    //        {
    //            strCheckValue = "True";
    //        }
    //    }

    //    if (strCheckValue == "False")
    //    {
    //        DisplayMessage("Need to select Advance Amount");
    //        txtSettleCustomer.Focus();
    //        return;
    //    }


    //    GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
    //    string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvSettleAmount")).Text);

    //    ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
    //    ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
    //    ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

    //    double SumReceiveAmount = 0;
    //    double sumForeignAmt = 0;
    //    foreach (GridViewRow gvrow in GVSettleMentDebit.Rows)
    //    {
    //        Label lblDueCheck = (Label)gvrow.FindControl("lbldueamount");
    //        TextBox txtReceiveCheck = (TextBox)gvrow.FindControl("txtgvSettleAmount");
    //        txtReceiveCheck.Text = SetDecimal(txtReceiveCheck.Text);
    //        double DueAmount = Convert.ToDouble(lblDueCheck.Text);
    //        double ReceiveAmount = Convert.ToDouble(txtReceiveCheck.Text);

    //        //if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
    //        //{
    //        if (((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text == "")
    //        {
    //            ((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text = "0";
    //        }
    //        SumReceiveAmount += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text);

    //        if (((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text == "")
    //        {
    //            ((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text = "0";
    //        }
    //        sumForeignAmt += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text);
    //        //}
    //    }
    //    //txtNetAmountLocal.Text = SetDecimal(SumReceiveAmount.ToString());
    //    //txtNetAmountForeign.Text = SetDecimal(sumForeignAmt.ToString());

    //    if (SumReceiveAmount != 0)
    //    {
    //        foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //        {
    //            Label lblPaidAmt = (Label)gvr.FindControl("lblpaidamount");
    //            double PaidAmt = Convert.ToDouble(lblPaidAmt.Text);
    //            Label lblDueAmt = (Label)gvr.FindControl("lbldueamount");
    //            double DueAmount = Convert.ToDouble(lblDueAmt.Text);
    //            CheckBox chkChecked = (CheckBox)gvr.FindControl("chkSettleCeditId");
    //            if (chkChecked.Checked == true)
    //            {
    //                if (DueAmount != 0)
    //                {
    //                    if (SumReceiveAmount > DueAmount)
    //                    {
    //                        DisplayMessage("Your Settle Amount is Exceed From Advance Amount");
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (SumReceiveAmount > PaidAmt)
    //                    {
    //                        DisplayMessage("Your Settle Amount is Exceed From Advance Amount");
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    if (GVSettleMentDebit.Rows.Count > 0)
    //    {
    //        Label lblgvSettleTotal = (Label)GVSettleMentDebit.FooterRow.FindControl("lblgvSettleTotal");
    //        lblgvSettleTotal.Text = SumReceiveAmount.ToString();
    //        lblgvSettleTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvSettleTotal.Text);
    //    }
    //}
    //protected void btnUpdateAgeing_Click(object sender, EventArgs e)
    //{

    //    if (txtSettleCustomer.Text == "")
    //    {
    //        DisplayMessage("Fill Customer Name");
    //        txtSettleCustomer.Focus();
    //        return;
    //    }

    //    if (GVSettleMentCredit.Rows.Count == 0)
    //    {
    //        DisplayMessage("You have no Advance Amount for Settle");
    //        txtSettleCustomer.Focus();
    //        return;
    //    }

    //    string strCheckValue = "False";
    //    foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //    {
    //        CheckBox chkCreditValue = (CheckBox)gvr.FindControl("chkSettleCeditId");
    //        if (chkCreditValue.Checked == true)
    //        {
    //            strCheckValue = "True";
    //        }
    //    }

    //    if (strCheckValue == "False")
    //    {
    //        DisplayMessage("Need to select Advance Amount");
    //        txtSettleCustomer.Focus();
    //        return;
    //    }

    //    string strReceiveVoucherAcc = string.Empty;
    //    DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
    //    if (dtParam.Rows.Count > 0)
    //    {
    //        strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
    //    }
    //    else
    //    {
    //        strReceiveVoucherAcc = "0";
    //    }

    //    double dtSettleAmt = 0;
    //    double TotalSettleBalance = 0;
    //    foreach (GridViewRow gvD in GVSettleMentDebit.Rows)
    //    {
    //        TextBox txtSettleAmt = (TextBox)gvD.FindControl("txtgvSettleAmount");
    //        if (txtSettleAmt.Text != "")
    //        {
    //            dtSettleAmt = Convert.ToDouble(txtSettleAmt.Text);
    //        }

    //        if (dtSettleAmt != 0)
    //        {
    //            TotalSettleBalance += dtSettleAmt;
    //        }
    //    }

    //    if (TotalSettleBalance == 0)
    //    {
    //        DisplayMessage("You need to settle Amount then you can update it");
    //        return;
    //    }

    //    //For Check Amount
    //    double SumReceiveAmount = 0;
    //    foreach (GridViewRow gvrow in GVSettleMentDebit.Rows)
    //    {
    //        Label lblDueCheck = (Label)gvrow.FindControl("lbldueamount");
    //        TextBox txtReceiveCheck = (TextBox)gvrow.FindControl("txtgvSettleAmount");
    //        txtReceiveCheck.Text = SetDecimal(txtReceiveCheck.Text);
    //        double DueAmount = Convert.ToDouble(lblDueCheck.Text);
    //        double ReceiveAmount = Convert.ToDouble(txtReceiveCheck.Text);

    //        if (((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text == "")
    //        {
    //            ((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text = "0";
    //        }
    //        SumReceiveAmount += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSettleAmount")).Text);
    //    }

    //    //txtNetAmountLocal.Text = SetDecimal(SumReceiveAmount.ToString());
    //    //txtNetAmountForeign.Text = SetDecimal(sumForeignAmt.ToString());

    //    if (SumReceiveAmount != 0)
    //    {
    //        foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //        {
    //            Label lblPaidAmt = (Label)gvr.FindControl("lblpaidamount");
    //            double PaidAmt = Convert.ToDouble(lblPaidAmt.Text);
    //            Label lblDueAmt = (Label)gvr.FindControl("lbldueamount");
    //            double DueAmount = Convert.ToDouble(lblDueAmt.Text);
    //            CheckBox chkChecked = (CheckBox)gvr.FindControl("chkSettleCeditId");
    //            if (chkChecked.Checked == true)
    //            {
    //                if (DueAmount != 0)
    //                {
    //                    if (SumReceiveAmount > DueAmount)
    //                    {
    //                        DisplayMessage("Your Settle Amount is Exceed From Advance Amount");
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (SumReceiveAmount > PaidAmt)
    //                    {
    //                        DisplayMessage("Your Settle Amount is Exceed From Advance Amount");
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }


    //    //For Credit Value Save
    //    string strVoucherId = string.Empty;
    //    foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
    //    {
    //        CheckBox chkValue = (CheckBox)gvr.FindControl("chkSettleCeditId");
    //        HiddenField hdnTransId = (HiddenField)gvr.FindControl("hdnTransId");

    //        double FinalDueAmt = 0;
    //        if (chkValue.Checked == true)
    //        {
    //            HiddenField hdnVoucherId = (HiddenField)gvr.FindControl("hdnVoucherId");
    //            strVoucherId = hdnVoucherId.Value;

    //            DataTable dtCheckAgeing = objAgeingDetail.GetAgeingDetailAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
    //            if (dtCheckAgeing.Rows.Count > 0)
    //            {
    //                dtCheckAgeing = new DataView(dtCheckAgeing, "Trans_Id='" + hdnTransId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
    //                if (dtCheckAgeing.Rows.Count > 0)
    //                {
    //                    double SavedDueAmt = Convert.ToDouble(dtCheckAgeing.Rows[0]["Due_Amount"].ToString());
    //                    double SavedMainAmt = Convert.ToDouble(dtCheckAgeing.Rows[0]["Paid_Receive_Amount"].ToString());
    //                    if (SavedDueAmt == 0)
    //                    {
    //                        FinalDueAmt = SavedMainAmt - Convert.ToDouble(TotalSettleBalance);
    //                    }
    //                    else
    //                    {
    //                        FinalDueAmt = SavedDueAmt - Convert.ToDouble(TotalSettleBalance);
    //                    }
    //                }
    //            }

    //            if (FinalDueAmt != 0)
    //            {
    //                objAgeingDetail.UpdateAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnTransId.Value, FinalDueAmt.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //            }
    //            else
    //            {
    //                objAgeingDetail.UpdateAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnTransId.Value, FinalDueAmt.ToString(), "Settled", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //            }
    //        }
    //    }

    //    //For Debit Value Save
    //    foreach (GridViewRow gvD in GVSettleMentDebit.Rows)
    //    {
    //        TextBox txtSettleAmt = (TextBox)gvD.FindControl("txtgvSettleAmount");
    //        double SettleAmt = Convert.ToDouble(txtSettleAmt.Text);

    //        if (SettleAmt != 0)
    //        {
    //            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtSettleAmt.Text);
    //            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
    //            objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvD.FindControl("hdnRefType")).Value, ((HiddenField)gvD.FindControl("hdnRefId")).Value, ((Label)gvD.FindControl("lblPONo")).Text, ((Label)gvD.FindControl("lblInvoiceDate")).Text, strReceiveVoucherAcc, txtSettleCustomer.Text.Split('/')[1].ToString(), ((Label)gvD.FindControl("lblinvamount")).Text, ((TextBox)gvD.FindControl("txtgvSettleAmount")).Text, "0", "1/1/1900", "1/1/1900", "", "Settle Amount By Settlement", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), ((HiddenField)gvD.FindControl("hdnExchangeRate")).Value, ((TextBox)gvD.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", strVoucherId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //        }
    //    }
    //    DisplayMessage("Record Updated", "green");
    //    btnSettleCustomerAdd_OnClick(sender, e);
    //}
    //protected void btnAgeingReset_Click(object sender, EventArgs e)
    //{
    //    txtSettleCustomer.Text = "";
    //    GVSettleMentCredit.DataSource = null;
    //    GVSettleMentCredit.DataBind();
    //    GVSettleMentDebit.DataSource = null;
    //    GVSettleMentDebit.DataBind();
    //    SettleCR.Visible = false;
    //    SettleDR.Visible = false;
    //}
    //protected void chkSettleCeditId_CheckedChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrowchk = (GridViewRow)((CheckBox)sender).Parent.Parent;

    //    foreach (GridViewRow gv in GVSettleMentCredit.Rows)
    //    {
    //        CheckBox chkTrans = (CheckBox)gv.FindControl("chkSettleCeditId");
    //        HiddenField hdnTransId = (HiddenField)gv.FindControl("hdnCrTransId");

    //        chkTrans.Checked = false;
    //    }
    //    ((CheckBox)gvrowchk.FindControl("chkSettleCeditId")).Checked = true;
    //}
    #endregion

    #region
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
        FillGridBin();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
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
        string sql = "select distinct ref_id,Invoice_no from ac_ageing_detail where Location_id='" + strLocationId + "' and IsActive='true' and ref_type='" + ref_type + "' and other_account_no='" + other_account_id + "' and Invoice_No Like '%" + prefixText + "%'";
        DataTable _temp = objDa.return_DataTable(sql);
        string[] str = new string[_temp.Rows.Count];
        for (int i = 0; i < _temp.Rows.Count; i++)
        {
            str[i] = _temp.Rows[i]["Invoice_no"].ToString() + "/" + _temp.Rows[i]["ref_id"].ToString() + "";
        }
        return str;
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


    #endregion


    #region PendingPayment

    protected void btnPendingPayment_Click(object sender, EventArgs e)
    {
        pnlMenuAgeingDetail.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuSettle.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuPendingPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlPendingPaymentDetail.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlSettelment.Visible = false;
        PnlAgeingDetail.Visible = false;

    }
    #endregion

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        hdnLocId.Value = ddlLocationList.SelectedValue;
        Session["CrvLocId"] = hdnLocId.Value;
        hdnCurrencyId.Value = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;
        //ViewState["DocNo"] = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), hdnLocId.Value, false, "160", "313", "0");
        ViewState["DocNo"] = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), hdnLocId.Value, false, "160", "313", "0", Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
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

        if (!Common.GetStatus(Session["EmpId"].ToString()))
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
}
