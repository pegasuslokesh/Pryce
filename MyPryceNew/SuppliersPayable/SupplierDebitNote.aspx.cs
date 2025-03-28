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

public partial class SuppliersPayable_SupplierDebitNote : System.Web.UI.Page
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
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayble/SupplierDebitNote.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
           
            FillCurrency();
            try
            {
                ddlLocalCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            }
            catch
            {

            }

            CalendarExtender_chequeCleardate.Format = objsys.SetDateFormat();
            CalendarExtender_txtchequeissuedate.Format = objsys.SetDateFormat();
            CalendarExtender_txtVoucherDate.Format = objsys.SetDateFormat();
            //AllPageCode();
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "315", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            //For Comment
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            btnList_Click(sender, e);
            FillGrid();
            rbCashPayment.Checked = true;
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnVoucherSave.Visible = true;
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
        btnAddSupplier.Visible = true;
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
    protected void btnAddSupplier_OnClick(object sender, EventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id from ac_ageing_detail group by Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and AgeingType='PV'";
        da.return_DataTable(sql);
        if (da.return_DataTable(sql).Rows.Count > 0)
        {
            GvPendingInvoice.DataSource = da.return_DataTable(sql);
            GvPendingInvoice.DataBind();
            ddlForeginCurrency.SelectedValue = da.return_DataTable(sql).Rows[0]["currency_id"].ToString();
            ddlForeginCurrency_SelectedIndexChanged(sender, e);
            foreach (GridViewRow gvr in GvPendingInvoice.Rows)
            {
                Label lblgvInvoiceAmount = (Label)gvr.FindControl("lblinvamount");
                Label lblgvPaidAmount = (Label)gvr.FindControl("lblpaidamount");
                Label lblgvDueAmount = (Label)gvr.FindControl("lbldueamount");
                TextBox txtgvPayLocal = (TextBox)gvr.FindControl("txtpayLocal");
                TextBox txtgvExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                TextBox txtgvPayForeign = (TextBox)gvr.FindControl("txtpayforeign");


                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

                chkSelect.Enabled = true;

                DataTable dsCurrency = null;
                dsCurrency = objCurrency.GetCurrencyMaster();
                if (dsCurrency.Rows.Count > 0)
                {
                    objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                    ddlgvCurrency.SelectedValue = da.return_DataTable(sql).Rows[0]["currency_id"].ToString();
                }

                lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
                lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
                lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
                txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
                txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
                txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
            }
        }
        else
        {
            GvPendingInvoice.DataSource = null;
            GvPendingInvoice.DataBind();
            ddlForeginCurrency.SelectedIndex = 0;
            DisplayMessage("No Record Available for Supplier");
        }
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
                return;
            }

            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
            }
            else
            {
                string strSupplierId = txtSupplierName.Text.Trim().Split('/')[1].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
                    if (dtSup.Rows.Count > 0)
                    {
                        Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
                    }
                    else
                    {
                        DisplayMessage("First Set Supplier Details in Supplier Setup");
                        txtSupplierName.Text = "";
                        txtSupplierName.Focus();
                        return;
                    }

                    if (Session["SupplierAccountId"].ToString() == "0" && Session["SupplierAccountId"].ToString() == "")
                    {
                        DisplayMessage("First Set Supplier Account in Supplier Setup");
                        txtSupplierName.Text = "";
                        txtSupplierName.Focus();
                        return;
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Focus();
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
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        double sumForeignamt = 0;

        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            CheckBox chkgvTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            Label lblgvInvoiceAmount = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmount = (Label)gvrow.FindControl("lblpaidamount");
            Label lblgvDueAmount = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkgvTransId.Checked)
            {
                if (txtgvPayForeign.Text == "")
                {
                    txtgvPayForeign.Text = "0";
                }
                sumForeignamt += Convert.ToDouble(txtgvPayForeign.Text);
            }

            lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
            lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
            lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
            txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
            txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
            txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
        }
        txtPaidForeignamount.Text = sumForeignamt.ToString();
        txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
    }
    protected void txtpayLocal_OnTextChanged(object sender, EventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();

        double sumLocalamt = 0;
        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            CheckBox chkgvTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            Label lblgvInvoiceAmount = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmount = (Label)gvrow.FindControl("lblpaidamount");
            Label lblgvDueAmount = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkgvTransId.Checked)
            {
                if (txtgvPayLocal.Text == "")
                {
                    txtgvPayLocal.Text = "0";
                }
                sumLocalamt += Convert.ToDouble(txtgvPayLocal.Text);

                if (txtgvPayForeign.Text == "")
                {
                    txtgvPayForeign.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtgvPayForeign.Text);
            }

            lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
            lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
            lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
            txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
            txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
            txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
        }
        txtPaidLocalAmount.Text = sumLocalamt.ToString();
        txtPaidForeignamount.Text = sumForeignAmt.ToString();
        txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidLocalAmount.Text);
        txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
    }
    protected void txtgvExcahangeRate_OnTextChanged(object sender, EventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
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
            CheckBox chkgvTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            Label lblgvInvoiceAmount = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmount = (Label)gvrow.FindControl("lblpaidamount");
            Label lblgvDueAmount = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkgvTransId.Checked)
            {
                if (txtgvPayForeign.Text == "")
                {
                    txtgvPayForeign.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtgvPayForeign.Text);
            }

            lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
            lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
            lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
            txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
            txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
            txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
        }
        txtPaidForeignamount.Text = sumForeignAmt.ToString();
        txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
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
            Session["dtFilter_SupplierDebitN"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();

            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            foreach (GridViewRow gv in GvVoucher.Rows)
            {
                Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
                lblgvVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
            }
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
        DataTable dt = (DataTable)Session["dtFilter_SupplierDebitN"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        //AllPageCode();

    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_SupplierDebitN"];
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
        Session["dtFilter_SupplierDebitN"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;

        hdnVoucherId.Value = e.CommandArgument.ToString();
        btnNew_Click(null, null);

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited");
                return;
            }

            Lbl_Tab_New.Text = Resources.Attendance.View;
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
            ddlLocalCurrency.SelectedValue = strCurrencyId;
            txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            //For Bank Charges
            DataTable dtBankCharges = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            dtBankCharges = new DataView(dtBankCharges, "Ref_Id='0' and Narration='Bank Charges'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtBankCharges.Rows.Count > 0)
            {
                txtBankCharges.Text = dtBankCharges.Rows[0]["Debit_Amount"].ToString();
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
            dtDebit = new DataView(dtDebit, "Ref_Id='0' and Narration<>'Bank Charges'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtAccountNameBank.Text = strAccountName + "/" + strAccountId;
                }
            }


            //Add Child Concept
            DataTable dtDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtDetail.Rows.Count > 0)
            {
                btnAddSupplier.Visible = false;
                objPageCmn.FillData((object)GvPendingInvoice, dtDetail, "", "");
                string strSupplierId = dtDetail.Rows[0]["Other_Account_No"].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, strSupplierId);
                    if (dt.Rows.Count > 0)
                    {
                        txtSupplierName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Supplier_Id"].ToString();
                        Session["SupplierAccountId"] = dt.Rows[0]["Account_No"].ToString();
                    }
                }

                ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();

                float strTotLocal = 0;
                float strTotForeign = 0;

                foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                {
                    Label lblgvInvoiceAmount = (Label)gvr.FindControl("lblinvamount");
                    Label lblgvPaidAmount = (Label)gvr.FindControl("lblpaidamount");
                    Label lblgvDueAmount = (Label)gvr.FindControl("lbldueamount");
                    TextBox txtgvPayLocal = (TextBox)gvr.FindControl("txtpayLocal");
                    TextBox txtgvExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                    TextBox txtgvPayForeign = (TextBox)gvr.FindControl("txtpayforeign");

                    HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                    HiddenField hdnExchangeRate = (HiddenField)gvr.FindControl("hdnExchangeRate");
                    DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;

                    DataTable dt = new DataView(dtDetail, "Ref_Id='" + hdnRefId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnExchangeRate.Value = dt.Rows[0]["Exchange_Rate"].ToString();
                        txtgvExchangeRate.Text = dt.Rows[0]["Exchange_Rate"].ToString();
                        DataTable dsCurrency = null;
                        dsCurrency = objCurrency.GetCurrencyMaster();
                        if (dsCurrency.Rows.Count > 0)
                        {
                            objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                            ddlgvCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
                        }

                        txtgvPayLocal.Text = dt.Rows[0]["Paid_Receive_Amount"].ToString();
                        txtgvPayForeign.Text = dt.Rows[0]["Foreign_Amount"].ToString();

                        strTotLocal += (float.Parse(txtgvPayLocal.Text));
                        strTotForeign += (float.Parse(txtgvPayForeign.Text));
                    }

                    lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
                    lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
                    lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
                    txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
                    txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
                    txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
                }
                txtPaidLocalAmount.Text = strTotLocal.ToString();
                txtPaidForeignamount.Text = strTotForeign.ToString();
                txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidLocalAmount.Text);
                txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
            }
        }
        btnVoucherSave.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;

        hdnVoucherId.Value = e.CommandArgument.ToString();
        btnNew_Click(null, null);

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited");
                return;
            }

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
            ddlLocalCurrency.SelectedValue = strCurrencyId;
            txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

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
            dtDebit = new DataView(dtDebit, "Ref_Id='0' and Narration<>'Bank Charges'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                string strAccountId = dtDebit.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(StrCompId, strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtAccountNameBank.Text = strAccountName + "/" + strAccountId;
                }
            }


            //Add Child Concept
            DataTable dtDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtDetail.Rows.Count > 0)
            {
                btnAddSupplier.Visible = false;
                objPageCmn.FillData((object)GvPendingInvoice, dtDetail, "", "");
                string strSupplierId = dtDetail.Rows[0]["Other_Account_No"].ToString();
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, strSupplierId);
                    if (dt.Rows.Count > 0)
                    {
                        txtSupplierName.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Supplier_Id"].ToString();
                        Session["SupplierAccountId"] = dt.Rows[0]["Account_No"].ToString();
                    }
                }

                ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();

                float strTotLocal = 0;
                float strTotForeign = 0;

                foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                {
                    Label lblgvInvoiceAmount = (Label)gvr.FindControl("lblinvamount");
                    Label lblgvPaidAmount = (Label)gvr.FindControl("lblpaidamount");
                    Label lblgvDueAmount = (Label)gvr.FindControl("lbldueamount");
                    TextBox txtgvPayLocal = (TextBox)gvr.FindControl("txtpayLocal");
                    TextBox txtgvExchangeRate = (TextBox)gvr.FindControl("txtgvExcahangeRate");
                    TextBox txtgvPayForeign = (TextBox)gvr.FindControl("txtpayforeign");

                    HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
                    HiddenField hdnExchangeRate = (HiddenField)gvr.FindControl("hdnExchangeRate");
                    DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;

                    DataTable dt = new DataView(dtDetail, "Ref_Id='" + hdnRefId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnExchangeRate.Value = dt.Rows[0]["Exchange_Rate"].ToString();
                        txtgvExchangeRate.Text = dt.Rows[0]["Exchange_Rate"].ToString();
                        DataTable dsCurrency = null;
                        dsCurrency = objCurrency.GetCurrencyMaster();
                        if (dsCurrency.Rows.Count > 0)
                        {
                            objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                            ddlgvCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
                        }

                        txtgvPayLocal.Text = dt.Rows[0]["Paid_Receive_Amount"].ToString();
                        txtgvPayForeign.Text = dt.Rows[0]["Foreign_Amount"].ToString();

                        strTotLocal += (float.Parse(txtgvPayLocal.Text));
                        strTotForeign += (float.Parse(txtgvPayForeign.Text));
                    }

                    lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
                    lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
                    lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
                    txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
                    txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
                    txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);

                }
                txtPaidLocalAmount.Text = strTotLocal.ToString();
                txtPaidForeignamount.Text = strTotForeign.ToString();
                txtPaidLocalAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidLocalAmount.Text);
                txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
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

        DataTable dtBrand = new DataView(objVoucherHeader.GetVoucherHeaderAllTrue(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString()), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtVoucher"] = dtBrand;
        Session["dtFilter_SupplierDebitN"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, "Field1='SDN'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtBrand, "", "");
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
        }

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");
            lblgvVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
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

        if (strAccountIdCash.Split(',').Contains(txtAccountNameBank.Text.Split('/')[1].ToString()))
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

        //Add for Rollback On 26-02-2016
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

        if (GvPendingInvoice.Rows.Count == 0)
        {
            DisplayMessage("You Need Atleast One Row in Detail View");
            return;
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

        if (strCheckBox != "True")
        {
            DisplayMessage("You Need To Select Atleast One Row In Grid");
            return;
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
            { }
            else
            {
                DisplayMessage("Please Also fill Bank Charges");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCharges);
                return;
            }
        }

        if (txtAccountNameBank.Text == "")
        {
            DisplayMessage("Fill Bank/Cash Account");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNameBank);
            return;
        }

        string strPaymentVoucherAcc = string.Empty;
        DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
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
            string strCreditAmount = string.Empty;
            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {
                b = objVoucherHeader.UpdateVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), hdnRef_Id.Value, hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ddlForeginCurrency.SelectedValue.ToString(), "0", txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "SDN", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);
                objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);

                foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                    {
                        Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                        //for Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, lblgvSerialNo.Text, Session["SupplierAccountId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, ((TextBox)gvr.FindControl("txtpayLocal")).Text, "0", "Payment Voucher", "", Session["EmpId"].ToString(), ((DropDownList)gvr.FindControl("ddlgvCurrency")).SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtpayforeign")).Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //For Ageing Detail Insert
                        //For Debit Entry
                        string strAgeCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                        string AgeCompanyCurrDebit = strAgeCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, ((Label)gvr.FindControl("lblPONo")).Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtpayLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ((DropDownList)gvr.FindControl("ddlgvCurrency")).SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtpayforeign")).Text, AgeCompanyCurrDebit, "0.00", Session["FinanceYearId"].ToString(), "PV", hdnVoucherId.Value, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }


                //for Bank Charges
                if (txtBankCharges.Text != "" && txtAcbankCharges.Text != "")
                {
                    //for Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtBankCharges.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

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
                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnVoucherId.Value, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, "Payment Voucher", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                btnList_Click(sender, e);
                DisplayMessage("Voucher Updated Successfully!!");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), "0", "0", "0", DateTime.Now.ToString(), txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ddlForeginCurrency.SelectedValue.ToString(), "0", txtNarration.Text, false.ToString(), false.ToString(), true.ToString(), "SDN", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

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

                    foreach (GridViewRow gvr in GvPendingInvoice.Rows)
                    {
                        if (((CheckBox)gvr.FindControl("chkTrandId")).Checked)
                        {
                            Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                            //for Debit Entry
                            string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblgvSerialNo.Text, Session["SupplierAccountId"].ToString(), txtSupplierName.Text.Split('/')[1].ToString(), ((HiddenField)gvr.FindControl("hdnRefId")).Value, "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, ((TextBox)gvr.FindControl("txtpayLocal")).Text, "0", "Payment Voucher", "", Session["EmpId"].ToString(), ((DropDownList)gvr.FindControl("ddlgvCurrency")).SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtpayforeign")).Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //For Ageing Detail Insert
                            //for Debit Entry
                            string strAgeCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((TextBox)gvr.FindControl("txtpayLocal")).Text);
                            string AgeCompanyCurrDebit = strAgeCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objAgeingDetail.InsertAgeingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, ((Label)gvr.FindControl("lblPONo")).Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), ((Label)gvr.FindControl("lblinvamount")).Text, ((TextBox)gvr.FindControl("txtpayLocal")).Text, "0", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtNarration.Text, Session["EmpId"].ToString(), ((DropDownList)gvr.FindControl("ddlgvCurrency")).SelectedValue, ((HiddenField)gvr.FindControl("hdnExchangeRate")).Value, ((TextBox)gvr.FindControl("txtpayforeign")).Text, AgeCompanyCurrDebit, "0.00", Session["FinanceYearId"].ToString(), "PV", strMaxId, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }

                    //for Bank Charges
                    if (txtBankCharges.Text != "" && txtAcbankCharges.Text != "")
                    {
                        //for Debit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), txtBankCharges.Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, "1", txtAcbankCharges.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtBankCharges.Text, "0", "Bank Charges", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtBankCharges.Text, CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

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
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, "1", txtAccountNameBank.Text.Split('/')[1].ToString(), "0", "0", "PINV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", strCreditAmount, "Payment Voucher", "", Session["EmpId"].ToString(), ddlForeginCurrency.SelectedValue, hdnFExchangeRate.Value, txtPaidForeignamount.Text, "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //End             
                }
                DisplayMessage("Voucher Saved Successfully");
            }

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

        GvPendingInvoice.DataSource = null;
        GvPendingInvoice.DataBind();

        txtNarration.Text = "";
        txtPaidLocalAmount.Text = "";
        txtPaidForeignamount.Text = "";
        txtBankCharges.Text = "";
        txtAcbankCharges.Text = "";

        txtAccountNameBank.Text = "";
        chkAdvancePayment.Checked = false;

        hdnFExchangeRate.Value = "0";
        hdnVoucherId.Value = "0";
        rbCashPayment.Checked = true;
        rbChequePayment.Checked = false;
        rbCashPayment_CheckedChanged(null, null);
        btnVoucherSave.Visible = true;
        btnAddSupplier.Visible = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;

        hdnRef_Id.Value = "0";
        hdnRef_Type.Value = "0";
        hdnInvoiceNumber.Value = "0";
        hdnInvoiceDate.Value = "0";

        txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "315", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = txtVoucherNo.Text;
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
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        GridViewRow gvrowtxt = (GridViewRow)((DropDownList)sender).Parent.Parent;

        string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtpayLocal")).Text);

        ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
        ((HiddenField)gvrowtxt.FindControl("hdnExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
        ((TextBox)gvrowtxt.FindControl("txtgvExcahangeRate")).Text = strFireignExchange.Trim().Split('/')[1].ToString();
        txtPaidForeignamount.Text += ((TextBox)gvrowtxt.FindControl("txtpayforeign")).Text;

        double sumForeignAmt = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            CheckBox chkgvTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            Label lblgvInvoiceAmount = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmount = (Label)gvrow.FindControl("lblpaidamount");
            Label lblgvDueAmount = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkgvTransId.Checked)
            {
                if (txtgvPayForeign.Text == "")
                {
                    txtgvPayForeign.Text = "0";
                }
                sumForeignAmt += Convert.ToDouble(txtgvPayForeign.Text);
            }

            lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
            lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
            lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
            txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
            txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
            txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
        }
        txtPaidForeignamount.Text = sumForeignAmt.ToString();
        txtPaidForeignamount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtPaidForeignamount.Text);
    }
    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue, "1");
        hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();
    }

    protected void chkTrandId_CheckedChanged(object sender, EventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        double sumForeignamt = 0;
        double sumLocal = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            CheckBox chkgvTransId = (CheckBox)gvrow.FindControl("chkTrandId");
            Label lblgvInvoiceAmount = (Label)gvrow.FindControl("lblinvamount");
            Label lblgvPaidAmount = (Label)gvrow.FindControl("lblpaidamount");
            Label lblgvDueAmount = (Label)gvrow.FindControl("lbldueamount");
            TextBox txtgvPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtgvExchangeRate = (TextBox)gvrow.FindControl("txtgvExcahangeRate");
            TextBox txtgvPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");

            if (chkgvTransId.Checked)
            {
                if (txtgvPayLocal.Text == "")
                {
                    txtgvPayLocal.Text = "0";
                }
                sumLocal += Convert.ToDouble(txtgvPayLocal.Text);

                if (txtgvPayForeign.Text == "")
                {
                    txtgvPayForeign.Text = "0";
                }
                sumForeignamt += Convert.ToDouble(txtgvPayForeign.Text);

                lblgvInvoiceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvInvoiceAmount.Text);
                lblgvPaidAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvPaidAmount.Text);
                lblgvDueAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDueAmount.Text);
                txtgvPayLocal.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayLocal.Text);
                txtgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvExchangeRate.Text);
                txtgvPayForeign.Text = objsys.GetCurencyConversionForInv(strCurrency, txtgvPayForeign.Text);
            }
        }
        txtPaidLocalAmount.Text = sumLocal.ToString();
        txtPaidForeignamount.Text = sumForeignamt.ToString();
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
    public string GetSupplierName(string strVoucherId)
    {
        string strSupplierName = string.Empty;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVoucherId);
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

        DataTable dtDate = (DataTable)Session["dtFilter_SupplierDebitN"];
        if (dtDate.Rows.Count > 0)
        {
            dtDate = new DataView(dtDate, "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDate.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, dtDate, "", "");
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtDate.Rows.Count + "";
                Session["dtFilter_SupplierDebitN"] = dtDate;
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

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvVoucherAmount");

            lblgvVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
    }
}