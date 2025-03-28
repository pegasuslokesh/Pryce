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

public partial class CustomerReceivable_CustomerCreditNote : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/CustomerCreditNote.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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

            }
            catch
            {

            }

            CalendarExtender_chequeCleardate.Format = objsys.SetDateFormat();
            CalendarExtender_txtchequeissuedate.Format = objsys.SetDateFormat();
            CalendarExtender_txtVoucherDate.Format = objsys.SetDateFormat();
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            //AllPageCode();
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "316", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            hdnLocId.Value = Session["LocId"].ToString();
            hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();
            btnList_Click(sender, e);
            FillGrid();
            rbCashPayment.Checked = true;
            RequiredFieldValidator7.ValidationGroup = "Cheque";
            RequiredFieldValidator8.ValidationGroup = "Cheque";
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnVoucherSave.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
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
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        btnAddCustomer.Visible = true;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        PnlList.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    public string SetDecimal(string amount)
    {
        return objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, amount);
    }
    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        strLocationId = hdnLocId.Value;
        string strCurrency = hdnCurrencyId.Value;
        string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id, Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail   group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,IsActive  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and AgeingType='RV' and IsActive='True'";
        //da.return_DataTable(sql);
        DataTable dtDetail = da.return_DataTable(sql);
        if (dtDetail.Rows.Count > 0)
        {
            dtDetail = new DataView(dtDetail, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + strLocationId + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dtDetail.Rows.Count > 0)
        {
            GvPendingInvoice.DataSource = dtDetail;
            GvPendingInvoice.DataBind();

            ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
            ddlForeginCurrency_SelectedIndexChanged(sender, e);
            foreach (GridViewRow gvr in GvPendingInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");
                chkSelect.Enabled = true;

                DataTable dsCurrency = null;
                dsCurrency = objCurrency.GetCurrencyMaster();
                if (dsCurrency.Rows.Count > 0)
                {
                    objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                    ddlgvCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
                }

                Label lblInvoiceAmount = (Label)gvr.FindControl("lblinvamount");
                Label lblReceivedAmt = (Label)gvr.FindControl("lblpaidamount");
                Label lblPaidAmt = (Label)gvr.FindControl("lbldueamount");
                TextBox txtgvReceiveAmtLocal = (TextBox)gvr.FindControl("txtgvReceiveAmountLocal");
                TextBox txtgvExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                TextBox txtgvForegnAmt = (TextBox)gvr.FindControl("txtgvReceiveAmountForeign");

                lblInvoiceAmount.Text = SetDecimal(lblInvoiceAmount.Text);
                lblReceivedAmt.Text = SetDecimal(lblReceivedAmt.Text);
                lblPaidAmt.Text = SetDecimal(lblPaidAmt.Text);
                txtgvReceiveAmtLocal.Text = SetDecimal(txtgvReceiveAmtLocal.Text);
                txtgvExchangeRate.Text = SetDecimal(txtgvExchangeRate.Text);
                txtgvForegnAmt.Text = SetDecimal(txtgvForegnAmt.Text);
            }
            //chkAdvancePay.Checked = false;
            //chkAdvancePay.Visible = false;
            //txtNetAmountLocal.ReadOnly = true;
        }
        else
        {
            GvPendingInvoice.DataSource = null;
            GvPendingInvoice.DataBind();
            DisplayMessage("No Record Available for Customer");
            ddlForeginCurrency.SelectedIndex = 0;
            //txtNetAmountLocal.ReadOnly = false;
            //chkAdvancePay.Checked = false;
            //chkAdvancePay.Visible = true;
        }
    }
    protected void txtpayforeign_OnTextChanged(object sender, EventArgs e)
    {
        double sumForeignamt = 0;

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
        txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerName.Text != "")
        {
            try
            {
                txtCustomerName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Customer Name");
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
                return;
            }

            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Customer Name");
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
            }
            else
            {
                string strCustomerId = txtCustomerName.Text.Trim().Split('/')[1].ToString();
                if (strCustomerId != "0" && strCustomerId != "")
                {
                    DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
                    if (dtCus.Rows.Count > 0)
                    {
                        Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
                    }
                    else
                    {
                        DisplayMessage("First Set Customer Details in Customer Setup");
                        txtCustomerName.Text = "";
                        txtCustomerName.Focus();
                        return;
                    }

                    if (Session["CustomerAccountId"].ToString() == "0" && Session["CustomerAccountId"].ToString() == "")
                    {
                        DisplayMessage("First Set Customer Account in Customer Setup");
                        txtCustomerName.Text = "";
                        txtCustomerName.Focus();
                        return;
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Select Customer Name");
            txtCustomerName.Focus();
        }
    }
    protected void txtNetAmountLocal_OnTextChanged(object sender, EventArgs e)
    {
        if (txtNetAmountLocal.Text != "")
        {
            double netamount = Convert.ToDouble(txtNetAmountLocal.Text);
            if (netamount != 0)
            {
                ddlForeginCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                ddlForeginCurrency_SelectedIndexChanged(sender, e);
                txtNetAmountLocal.Text = SetDecimal(txtNetAmountLocal.Text);
            }
        }
    }
    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {
        if (txtNetAmountLocal.Text != "")
        {
            if (txtExchangeRate.Text != "")
            {
                txtNetAmountForeign.Text = objsys.GetCurencyConversionForInv(ddlForeginCurrency.SelectedValue, (float.Parse(txtExchangeRate.Text) * float.Parse(txtNetAmountLocal.Text)).ToString());
                hdnFExchangeRate.Value = txtExchangeRate.Text;
                txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
            }
        }
    }
    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForeginCurrency.SelectedIndex == 0)
        {
            hdnFExchangeRate.Value = "0";
            txtExchangeRate.Text = "0";
        }
        else
        {
            string strFireignExchange = string.Empty;
            if (txtNetAmountLocal.Text != "")
            {
                strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue, txtNetAmountLocal.Text);
                txtNetAmountForeign.Text = strFireignExchange.Trim().Split('/')[0].ToString();
                hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
            }
            else
            {
                strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue, "0");
                hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
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
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        double SumReceiveAmount = 0;
        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            Label lblDueCheck = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtReceiveCheck = (TextBox)gvrow.FindControl("txtgvReceiveAmountLocal");
            TextBox txtForeignAmt = (TextBox)gvrow.FindControl("txtgvReceiveAmountForeign");
            txtReceiveCheck.Text = SetDecimal(txtReceiveCheck.Text);
            double DueAmount = Convert.ToDouble(lblDueCheck.Text);
            double ReceiveAmount = Convert.ToDouble(txtReceiveCheck.Text);

            if (chkCreditPay.Visible == true && chkCreditPay.Checked == true)
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
                SumReceiveAmount += Convert.ToDouble(txtReceiveCheck.Text);

                if (txtForeignAmt.Text == "")
                {
                    txtForeignAmt.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtForeignAmt.Text);
            }
        }
        txtNetAmountLocal.Text = SetDecimal(SumReceiveAmount.ToString());
        txtNetAmountForeign.Text = SetDecimal(sumForeignAmt.ToString());
    }
    protected void txtgvExcahangeRate_OnTextChanged(object sender, EventArgs e)
    {
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
        txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
    }

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();

        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
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
    #endregion

    protected void btnVoucherSave_Click(object sender, EventArgs e)
    {
        if(txtVoucherNo.Text=="")
        {
            DisplayMessage("Enter Voucher No.");
            txtVoucherNo.Focus();
            return;
        }
        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Enter Voucher Date");
            txtVoucherDate.Focus();
            return;
        }
        if (rbChequePayment.Checked == true)
        {
            if (txtChequeIssueDate.Text == "")
            {
                DisplayMessage("Enter Cheque Issue Date");
                txtChequeIssueDate.Focus();
                return;
            }
            if (txtChequeClearDate.Text == "")
            {
                DisplayMessage("Enter Cheque Clear Date");
                txtChequeClearDate.Focus();
                return;
            }
        }
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Enter Customer");
            txtCustomerName.Focus();
            return;
        }
        if (txtNetAmountLocal.Text == "")
        {
            DisplayMessage("Enter Net Amount Local");
            txtNetAmountLocal.Focus();
            return;
        }
        if (txtNetAmountForeign.Text == "")
        {
            DisplayMessage("Enter Net Amount Foreign");
            txtNetAmountForeign.Focus();
            return;
        }

        if (txtDebitAccountName.Text == "")
        {
            DisplayMessage("Enter Debit A/C Name");
            txtDebitAccountName.Focus();
            return;
        }

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        //for Cash flow Posted
        //For Cash flow Account
        string strCashflowPostedV = string.Empty;
        string strAccountIdCash = string.Empty;
        DataTable dtAccountCash = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount");
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

        if (strAccountIdCash.Split(',').Contains(txtDebitAccountName.Text.Split('/')[1].ToString()))
        {
            DataTable dtCashflowDetail = objCashFlowDetail.GetCashFlowDetailForAcountsEntry(StrCompId, StrBrandId, strLocationId);
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

        if (GvPendingInvoice.Rows.Count == 0 && chkCreditPay.Checked == false)
        {
            DisplayMessage("You Need Atleast One Row in Detail View Otherwise You can do it Credit Payment");
            return;
        }

        if (GvPendingInvoice.Rows.Count == 0)
        {
            if (chkCreditPay.Checked == true)
            {
                if (txtNetAmountLocal.Text == "")
                {
                    DisplayMessage("Fill Local Amount for Credit Pay");
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
            if (chkCreditPay.Checked == false)
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

        if (chkCreditPay.Checked == false)
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


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {
                b = objVoucherHeader.UpdateVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), hdnRef_Id.Value, hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlLocalCurrency.SelectedValue.ToString(), "0", txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "CCN", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);
                objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);

                if (chkCreditPay.Checked == false)
                {
                    foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                    {
                        HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");

                        if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                        {
                            Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");

                            //for Credit
                            string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text);
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, lblgvSerialNo.Text, Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //For Ageing Detail Insert
                            objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, ((Label)gvr.FindControl("lblPONo")).Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", hdnVoucherId.Value, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }

                    //Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, CompanyCurrDebit, "0.00", "DebitCCN", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else if (chkCreditPay.Checked == true && txtNetAmountLocal.Text != "")
                {
                    //for Credit
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //For Ageing Detail Insert
                    objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SINV", "0", "0", DateTime.Now.ToString(), strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", txtNetAmountLocal.Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", hdnVoucherId.Value, "CreditPay", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, CompanyCurrDebit, "0.00", "DebitCCN", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                DisplayMessage("Voucher Updated Successfully!");
                btnList_Click(sender, e);
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), "0", "0", "0", DateTime.Now.ToString(), txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, ddlForeginCurrency.SelectedValue.ToString(), txtExchangeRate.Text, txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "CCN", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                string strMaxId = string.Empty;
                DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                if (dtMaxId.Rows.Count > 0)
                {
                    strMaxId = dtMaxId.Rows[0][0].ToString();
                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {
                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
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

                    //Add Detail/Ageing Detail Section.
                    objVoucherDetail.DeleteVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, ref trns);

                    if (chkCreditPay.Checked == false)
                    {
                        foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                        {
                            HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");

                            if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                            {
                                Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                                HiddenField hdnExchangeRate = (HiddenField)gvr.FindControl("hdnExchangeRate");
                                TextBox txtgvExcahangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                                if (hdnExchangeRate.Value == "")
                                    hdnExchangeRate.Value = "0";

                                //for Credit
                                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text);
                                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblgvSerialNo.Text, Session["CustomerAccountId"].ToString(), txtCustomerName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                //For Ageing Detail Insert
                                objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, ((Label)gvr.FindControl("lblPONo")).Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtgvReceiveAmountLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtgvReceiveAmountForeign")).Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", strMaxId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        //Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, CompanyCurrDebit, "0.00", "DebitCCN", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else if (chkCreditPay.Checked == true && txtNetAmountLocal.Text != "")
                    {
                        //For Credit
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, "1", strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", txtNetAmountLocal.Text, txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //For Ageing Detail Insert
                        objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SINV", "0", "0", DateTime.Now.ToString(), strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), "0", txtNetAmountLocal.Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, "0.00", CompanyCurrCredit, Session["FinanceYearId"].ToString(), "RV", strMaxId, "CreditPay", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtNetAmountLocal.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, "1", txtDebitAccountName.Text.Split('/')[1].ToString(), "0", "0", "SINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNetAmountLocal.Text, "0", txtNarration.Text, "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, txtExchangeRate.Text, txtNetAmountForeign.Text, CompanyCurrDebit, "0.00", "DebitCCN", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //End             
                }
                DisplayMessage("Voucher Saved Successfully!");
            }

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

        chkCreditPay.Checked = false;
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

        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;

        txtVoucherNo.Text = ViewState["DocNo"].ToString();
        //ViewState["DocNo"] = objDocNo.GetDocumentNo(true, "0", false, "160", "316", "0");
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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
                Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
                lblVoucherAmt.Text = SetDecimal(lblVoucherAmt.Text);
            }

            Session["dtFilter_Cust_C_Note"] = view.ToTable();
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
        DataTable dt = (DataTable)Session["dtFilter_Cust_C_Note"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = SetDecimal(lblVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Cust_C_Note"];
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
        Session["dtFilter_Cust_C_Note"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = SetDecimal(lblVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;
        strLocationId = hdnLocId.Value;

        hdnVoucherId.Value = e.CommandArgument.ToString();
        btnNew_Click(null, null);

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited, You Can View Only");
                return;
            }

            Lbl_Tab_New.Text = Resources.Attendance.View;
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

            //For Debit Account
            DataTable dtDebit = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            dtDebit = new DataView(dtDebit, "Ref_Id='0'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                hdnFExchangeRate.Value = dtDebit.Rows[0]["Exchange_Rate"].ToString();
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtDebitAccountName.Text = strAccountName + "/" + strAccountId;
                }
            }

            //Add Child Concept
            DataTable dtDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtDetail.Rows.Count > 0)
            {
                btnAddCustomer.Visible = false;
                objPageCmn.FillData((object)GvPendingInvoice, dtDetail, "", "");

                string strSupplierId = dtDetail.Rows[0]["Other_Account_No"].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strSupplierId);
                    if (_dtTemp.Rows.Count > 0)
                    {
                        txtCustomerName.Text = _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString();
                        Session["CustomerAccountId"] = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    }
                    //DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(StrCompId, StrBrandId, strSupplierId);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    txtCustomerName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
                    //    Session["CustomerAccountId"] = dt.Rows[0]["Account_No"].ToString();
                    //}
                }

                ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();

                float strTotNetAmtLocal = 0;
                float strTotNetAmtForeign = 0;
                foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                {
                    Label lblInvAmt = (Label)gvr.FindControl("lblinvamount");
                    Label lblPaidAmt = (Label)gvr.FindControl("lblpaidamount");
                    Label lblDueAmt = (Label)gvr.FindControl("lbldueamount");

                    HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                    TextBox txtReceivedAmtLocal = (TextBox)gvr.FindControl("txtgvReceiveAmountLocal");
                    TextBox txtReceivedAmtForeign = (TextBox)gvr.FindControl("txtgvReceiveAmountForeign");
                    HiddenField hdnExchangeRate = (HiddenField)gvr.FindControl("hdnExchangeRate");
                    TextBox txtExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                    DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                    DataTable dt = new DataView(dtDetail, "Ref_Id='" + hdnRefId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnExchangeRate.Value = dt.Rows[0]["Exchange_Rate"].ToString();
                        txtExchangeRate.Text = dt.Rows[0]["Exchange_Rate"].ToString();
                        DataTable dsCurrency = null;
                        dsCurrency = objCurrency.GetCurrencyMaster();
                        if (dsCurrency.Rows.Count > 0)
                        {
                            objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                            ddlgvCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
                        }

                        txtReceivedAmtLocal.Text = dt.Rows[0]["Paid_Receive_Amount"].ToString();
                        txtReceivedAmtForeign.Text = dt.Rows[0]["Foreign_Amount"].ToString();
                        txtReceivedAmtLocal.Text = SetDecimal(txtReceivedAmtLocal.Text);
                        txtReceivedAmtForeign.Text = SetDecimal(txtReceivedAmtForeign.Text);
                        txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);

                        lblInvAmt.Text = SetDecimal(lblInvAmt.Text);
                        lblPaidAmt.Text = SetDecimal(lblPaidAmt.Text);
                        lblDueAmt.Text = SetDecimal(lblDueAmt.Text);

                        strTotNetAmtLocal += (float.Parse(txtReceivedAmtLocal.Text));
                        strTotNetAmtForeign += (float.Parse(txtReceivedAmtForeign.Text));
                    }
                }
                txtNetAmountLocal.Text = strTotNetAmtLocal.ToString();
                txtNetAmountForeign.Text = strTotNetAmtForeign.ToString();
                txtNetAmountLocal.Text = SetDecimal(txtNetAmountLocal.Text);
                txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
            }
        }

        btnAddCustomer.Visible = false;
        btnVoucherSave.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;
        strLocationId = hdnLocId.Value;
        hdnVoucherId.Value = e.CommandArgument.ToString();
        btnNew_Click(null, null);

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited, You Can View Only");
                return;
            }

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
            dtDebit = new DataView(dtDebit, "Ref_Id='0' and Field1='DebitCCN'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                hdnFExchangeRate.Value = dtDebit.Rows[0]["Exchange_Rate"].ToString();
                txtExchangeRate.Text = hdnFExchangeRate.Value;
                txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtDebitAccountName.Text = strAccountName + "/" + strAccountId;
                }
            }

            //Add Child Concept
            DataTable dtDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtDetail.Rows.Count > 0)
            {

                //if (dtDetail.Rows[0]["Field4"].ToString() == "AdvancePay")
                //{
                //    chkAdvancePay.Visible = true;
                //    chkAdvancePay.Checked = true;
                //    chkAdvancePay.Enabled = false;
                //}
                //else
                //{
                //    chkAdvancePay.Visible = false;
                //    chkAdvancePay.Checked = false;
                //    chkAdvancePay.Enabled = true;
                //}

                //btnAddCustomer.Visible = false;
                objPageCmn.FillData((object)GvPendingInvoice, dtDetail, "", "");

                string strSupplierId = dtDetail.Rows[0]["Other_Account_No"].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strSupplierId);
                    if (_dtTemp.Rows.Count > 0)
                    {
                        txtCustomerName.Text = _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString();
                        Session["CustomerAccountId"] = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    }
                    //DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(StrCompId, StrBrandId, strSupplierId);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    txtCustomerName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
                    //    Session["CustomerAccountId"] = dt.Rows[0]["Account_No"].ToString();
                    //}
                }

                ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();

                float strTotNetAmtLocal = 0;
                float strTotNetAmtForeign = 0;
                foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                {
                    //for Due Amount
                    Label lblInvAmt = (Label)gvr.FindControl("lblinvamount");
                    Label lblPaidAmt = (Label)gvr.FindControl("lblpaidamount");
                    Label lblDueAmt = (Label)gvr.FindControl("lbldueamount");

                    double invamt = Convert.ToDouble(lblInvAmt.Text);
                    double Paidamt = Convert.ToDouble(lblPaidAmt.Text);
                    if (invamt != 0)
                    {
                        if (Paidamt != 0)
                        {
                            lblDueAmt.Text = (float.Parse(invamt.ToString()) - float.Parse(Paidamt.ToString())).ToString();
                        }
                    }

                    HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                    TextBox txtReceivedAmtLocal = (TextBox)gvr.FindControl("txtgvReceiveAmountLocal");
                    TextBox txtReceivedAmtForeign = (TextBox)gvr.FindControl("txtgvReceiveAmountForeign");
                    HiddenField hdnExchangeRate = (HiddenField)gvr.FindControl("hdnExchangeRate");
                    TextBox txtExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                    DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

                    chkSelect.Checked = true;
                    //chkSelect.Enabled = false;
                    DataTable dt = new DataView(dtDetail, "Ref_Id='" + hdnRefId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnExchangeRate.Value = dt.Rows[0]["Exchange_Rate"].ToString();
                        txtExchangeRate.Text = dt.Rows[0]["Exchange_Rate"].ToString();
                        txtExchangeRate.Text = SetDecimal(txtExchangeRate.Text);
                        DataTable dsCurrency = null;
                        dsCurrency = objCurrency.GetCurrencyMaster();
                        if (dsCurrency.Rows.Count > 0)
                        {
                            objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                            ddlgvCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
                        }

                        txtReceivedAmtLocal.Text = dt.Rows[0]["Paid_Receive_Amount"].ToString();
                        txtReceivedAmtForeign.Text = dt.Rows[0]["Foreign_Amount"].ToString();
                        txtReceivedAmtLocal.Text = SetDecimal(txtReceivedAmtLocal.Text);
                        txtReceivedAmtForeign.Text = SetDecimal(txtReceivedAmtForeign.Text);

                        strTotNetAmtLocal += (float.Parse(txtReceivedAmtLocal.Text));
                        strTotNetAmtForeign += (float.Parse(txtReceivedAmtForeign.Text));
                    }
                    lblDueAmt.Text = SetDecimal(lblDueAmt.Text);
                }
                txtNetAmountLocal.Text = strTotNetAmtLocal.ToString();
                txtNetAmountForeign.Text = strTotNetAmtForeign.ToString();
                txtNetAmountLocal.Text = SetDecimal(txtNetAmountLocal.Text);
                txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
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
        string strCustomerParentAcId = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        //DataTable dtBrand = objVoucherHeader.GetVoucherHeaderAllTrueWithVoucherDetail(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        DataTable dtBrand = objVoucherHeader.GetActiveCustomerCreditNoteVoucher(strLocationId, Session["FinanceYearId"].ToString(), strCustomerParentAcId);
        if (dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, PostStatus, "", DataViewRowState.CurrentRows).ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        }

        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, "Voucher_Type='CCN'", "Voucher_Date,trans_id DESC", DataViewRowState.CurrentRows).ToTable();
            Session["dtVoucher"] = dtBrand;
            Session["dtFilter_Cust_C_Note"] = dtBrand;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtBrand, "", "");

            foreach (GridViewRow gv in GvVoucher.Rows)
            {
                Label lblVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
                lblVoucherAmt.Text = SetDecimal(lblVoucherAmt.Text);
            }
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
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
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Name"].ToString() + "/" + dtSupplier.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
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
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

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
    protected void ddlgvCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrowtxt = (GridViewRow)((DropDownList)sender).Parent.Parent;

        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        txtNetAmountForeign.Text += ((TextBox)gvrowtxt.FindControl("txtgvReceiveAmountForeign")).Text;
        txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);

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
        txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
    }
    protected void chkTrandId_CheckedChanged(object sender, EventArgs e)
    {
        double sumForeignamt = 0;
        double sumLocal = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtgvReceiveAmountLocal")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txtgvReceiveAmountLocal")).Text = "0";
                }
                sumLocal += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountLocal")).Text);

                if (((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text == "")
                {
                    ((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text = "0";
                }
                sumForeignamt += Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvReceiveAmountForeign")).Text);
            }
        }

        txtNetAmountLocal.Text = sumLocal.ToString();
        txtNetAmountForeign.Text = sumForeignamt.ToString();
        txtNetAmountLocal.Text = SetDecimal(txtNetAmountLocal.Text);
        txtNetAmountForeign.Text = SetDecimal(txtNetAmountForeign.Text);
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
            RequiredFieldValidator7.ValidationGroup = "Cheque";
            RequiredFieldValidator8.ValidationGroup = "Cheque";
        }
        else if (rbChequePayment.Checked == true)
        {
            rbCashPayment.Checked = false;
            trCheque1.Visible = true;
            trCheque2.Visible = true;
            RequiredFieldValidator7.ValidationGroup = "Save";
            RequiredFieldValidator8.ValidationGroup = "Save";
        }
    }
    public string GetVoucherAmount(string strVoucherId)
    {
        string strVoucherAmount = string.Empty;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVoucherId);
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

        DataTable dtDate = (DataTable)Session["dtFilter_Cust_C_Note"];
        if (dtDate.Rows.Count > 0)
        {
            dtDate = new DataView(dtDate, "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDate.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, dtDate, "", "");
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtDate.Rows.Count + "";
                Session["dtFilter_Cust_C_Note"] = dtDate;
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
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
            lblVoucherAmt.Text = SetDecimal(lblVoucherAmt.Text);
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
    #region Print

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Accounts_Report/CustomerReceiveVoucherReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion


    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        hdnLocId.Value = ddlLocationList.SelectedValue;
        hdnCurrencyId.Value = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;
        ViewState["DocNo"] = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), hdnLocId.Value, false, "160", "316", "0", Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (hdnVoucherId.Value == "0")
        {
            txtVoucherNo.Text = ViewState["DocNo"].ToString();
        }
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