using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using DevExpress.Web;

public partial class WebUserControl_AgeingSettlement : System.Web.UI.UserControl
{
    Ems_ContactMaster ObjContactMaster = null;
    Set_Suppliers objSupplier = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Ac_Ageing_Detail_Old objAgeingDetailOld = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
            objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
            objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
            objsys = new SystemParameter(Session["DBConnection"].ToString());
            ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
            da = new DataAccessClass(Session["DBConnection"].ToString());
            objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
            objAgeingDetailOld = new Ac_Ageing_Detail_Old(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                Session["ParentPageId"] = "1";
                lblSettleSupplier.Text = Resources.Attendance.Customer;

            }
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Supplier")
            {
                Session["ParentPageId"] = "2";
                lblSettleSupplier.Text = Resources.Attendance.Supplier;
            }
            if (!IsPostBack)
            {
                FillCurrency();
            }
        }
        catch
        {
        }

    }
  

    public string SetDecimal(string amount)
    {
        string strCurrencyId = Session["LocCurrencyId"].ToString();
        try
        {
            HiddenField hdnCurrency = (HiddenField)this.Parent.FindControl("hdnCurrencyId");
            strCurrencyId = hdnCurrency.Value;
        }
        catch (Exception ex)
        {

        }
        //return objsys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
        return objsys.GetCurencyConversionForInv(strCurrencyId, amount);
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

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    #region SettlemetView


    protected void txtSettleSupplier_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSettleSupplier.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSettleSupplier.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        return;
                    }

                }
            }
        }
        catch
        {

        }
        DisplayMessage("Select " + lblSettleSupplier.Text + " Name");
        GVSettleMentCredit.DataSource = null;
        GVSettleMentCredit.DataBind();
        GVSettleMentDebit.DataSource = null;
        GVSettleMentDebit.DataBind();
        SettleCR.Visible = false;
        SettleDR.Visible = false;
        txtSettleSupplier.Focus();
    }
    protected void btnSettleSupplierAdd_OnClick(object sender, EventArgs e)
    {
        GVSettleMentCredit.DataSource = null;
        GVSettleMentCredit.DataBind();

        GVSettleMentDebit.DataSource = null;
        GVSettleMentDebit.DataBind();

        string sql = string.Empty;
        string strCondition = string.Empty;
        //Get Location id from cash flow
        string strNewLocations = string.Empty;
        string strLocationId = Session["LocId"].ToString();
        string strCurrencyId = Session["LocCurrencyId"].ToString();

        try
        {
            HiddenField hdnCurrency = (HiddenField)this.Parent.FindControl("hdnCurrencyId");
            HiddenField hdnLocation = (HiddenField)this.Parent.FindControl("hdnLocId");
            strCurrencyId = hdnCurrency.Value;
            strLocationId = hdnLocation.Value;
        }
        catch(Exception ex)
        {

        }

        try
        {
            string sql1 = "SELECT STUFF((SELECT ',' + Param_Value FROM Ac_Parameter_Location where company_id='" + Session["CompId"].ToString() + "' and location_id='" + strLocationId + "' and Param_Name='CashFlowLocation' and isActive='true' FOR XML PATH('') ), 1, 1, '')";
            strNewLocations = da.get_SingleValue(sql1).ToString();
        }
        catch (Exception ex)
        {

        }
        if (string.IsNullOrEmpty(strNewLocations))
        {
            strNewLocations = strLocationId;
        }
        //supplier
        if (Session["ParentPageId"].ToString() == "2")
        {
            strCondition = "   Ac_voucher_Detail.Account_No='" + Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "' and Ac_voucher_Detail.other_account_no='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and  Ac_voucher_Detail.Debit_Amount>0  AND ( Ac_Voucher_Header.Field3 <>'Pending' and Ac_Voucher_Header.Field3 <>'Rejected') and Ac_voucher_Detail.Location_Id in (" + strNewLocations + ") ";
               sql = "select tmpVoucher.*,fAmount as Actual_voucher_amount, f_balance_amount as Actual_balance_amount from (select ac_voucher_header.trans_id as voucher_id,ac_voucher_header.Voucher_No,ac_voucher_header.Voucher_Date,Ac_voucher_Detail.Account_No,Ac_voucher_Detail.other_account_no,Ac_voucher_Detail.currency_id,Sys_CurrencyMaster.Currency_Name,Ac_voucher_Detail.exchange_rate," +
   " SUM(Ac_voucher_Detail.Debit_Amount)as LAmount," +
   " sum(Debit_Amount-(case when LAgeingAmount IS not null And LAgeingAmount>0 then LAgeingAmount else 0 end)  )as L_balance_amount, " +
   " SUM(case when Ac_voucher_Detail.Debit_Amount>0 then Ac_voucher_Detail.foreign_amount else 0 end)as FAmount, " +
   " sum((case when Ac_voucher_Detail.Debit_Amount>0 then Ac_voucher_Detail.foreign_amount else 0 end)-(case when FAgeingAmount IS not null And FAgeingAmount>0 then FAgeingAmount else 0 end)  )as F_balance_amount " +
   " from ac_voucher_header inner join Ac_voucher_Detail on Ac_Voucher_Header.Trans_Id=Ac_Voucher_Detail.Voucher_No " +
   " left join (select account_no,other_account_no,voucherId, SUM(paid_receive_amount)as LAgeingAmount,SUM(case when paid_receive_amount>0 then Foreign_Amount else 0 end)as FAgeingAmount from Ac_Ageing_Detail  where IsActive='True' group by account_no,other_account_no,voucherId)Ac_Ageing_Detail on Ac_Ageing_Detail.VoucherId=Ac_Voucher_Detail.Voucher_No and Ac_Ageing_Detail.Account_No=Ac_Voucher_Detail.Account_No and Ac_Ageing_Detail.Other_Account_No=Ac_Voucher_Detail.Other_Account_No " +
   " left join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Ac_voucher_Detail.currency_id" +
   " where  " + strCondition +
   " AND Ac_Voucher_Header.ReconciledFromFinance = 'True' AND Ac_Voucher_Header.isActive = 'True' " +
   " group by ac_voucher_header.trans_id,ac_voucher_header.Voucher_No, ac_voucher_header.Voucher_Date,Ac_voucher_Detail.Account_No,Ac_voucher_Detail.other_account_no,Ac_voucher_Detail.currency_id,Sys_CurrencyMaster.Currency_Name,Ac_voucher_Detail.exchange_rate)tmpVoucher " +
   " where f_balance_amount>0";
        }
        else
        {
           
           strCondition = "   Ac_voucher_Detail.Account_No='" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "' and Ac_voucher_Detail.other_account_no='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and  Ac_voucher_Detail.Credit_Amount>0   and Ac_voucher_Detail.Location_Id in (" + strNewLocations + ") ";

            sql = "select tmpVoucher.*,fAmount as Actual_voucher_amount, f_balance_amount as Actual_balance_amount from (select ac_voucher_header.trans_id as voucher_id,ac_voucher_header.Voucher_No,ac_voucher_header.Voucher_Date,Ac_voucher_Detail.Account_No,Ac_voucher_Detail.other_account_no,Ac_voucher_Detail.currency_id,Sys_CurrencyMaster.Currency_Name,Ac_voucher_Detail.exchange_rate," +
" SUM(Ac_voucher_Detail.Credit_Amount)as LAmount," +
" sum(Credit_Amount-(case when LAgeingAmount IS not null And LAgeingAmount>0 then LAgeingAmount else 0 end)  )as L_balance_amount, " +
" SUM(case when Ac_voucher_Detail.Credit_Amount>0 then Ac_voucher_Detail.foreign_amount else 0 end)as FAmount, " +
" sum((case when Ac_voucher_Detail.Credit_Amount>0 then Ac_voucher_Detail.foreign_amount else 0 end)-(case when FAgeingAmount IS not null And FAgeingAmount>0 then FAgeingAmount else 0 end)  )as F_balance_amount " +
" from ac_voucher_header inner join Ac_voucher_Detail on Ac_Voucher_Header.Trans_Id=Ac_Voucher_Detail.Voucher_No " +
" left join (select account_no,other_account_no,voucherId, SUM(paid_receive_amount)as LAgeingAmount,SUM(case when paid_receive_amount>0 then Foreign_Amount else 0 end)as FAgeingAmount from Ac_Ageing_Detail where IsActive='True' group by account_no,other_account_no,voucherId)Ac_Ageing_Detail on Ac_Ageing_Detail.VoucherId=Ac_Voucher_Detail.Voucher_No and Ac_Ageing_Detail.Account_No=Ac_Voucher_Detail.Account_No and Ac_Ageing_Detail.Other_Account_No=Ac_Voucher_Detail.Other_Account_No " +
" left join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Ac_voucher_Detail.currency_id" +
" where  " + strCondition +
" AND Ac_Voucher_Header.ReconciledFromFinance = 'True' AND Ac_Voucher_Header.isActive = 'True' " +
" group by ac_voucher_header.trans_id,ac_voucher_header.Voucher_No, ac_voucher_header.Voucher_Date,Ac_voucher_Detail.Account_No,Ac_voucher_Detail.other_account_no,Ac_voucher_Detail.currency_id,Sys_CurrencyMaster.Currency_Name,Ac_voucher_Detail.exchange_rate)tmpVoucher " +
" where (case when currency_id='" + Session["LocCurrencyId"].ToString() + "' then l_balance_amount else f_balance_amount end)>0";

        }


        if (txtSettleSupplier.Text != "")
        {
            
            DataTable dtStatement = da.return_DataTable(sql);
            
            if (dtStatement.Rows.Count > 0)
            {
                GVSettleMentCredit.DataSource = dtStatement;
                GVSettleMentCredit.DataBind();
                SettleCR.Visible = true;
            }
            else
            {
                GVSettleMentCredit.DataSource = null;
                GVSettleMentCredit.DataBind();
                SettleCR.Visible = false;
            }

            foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
            {
                Label lblVoucherAmount = (Label)gvr.FindControl("lblVoucherAmount");
                Label lblBlanceAmount = (Label)gvr.FindControl("lblBalanceAmount");


                //Label lblDueAmount = (Label)gvr.FindControl("lbldueamount");

                lblVoucherAmount.Text = SetDecimal(lblVoucherAmount.Text);
                lblBlanceAmount.Text = SetDecimal(lblBlanceAmount.Text);
                //lblDueAmount.Text = SetDecimal(lblDueAmount.Text);
            }

            //For Settlement Debit


            //sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id, Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail   group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,IsActive  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and AgeingType='PV' and IsActive='True'";
            ////da.return_DataTable(sql);
            //DataTable dtDetail = da.return_DataTable(sql);
            DataTable dtDetail = new DataTable();
            string strVoucherType = string.Empty;
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                strVoucherType = "RV";
            }
            else
            {
                strVoucherType = "PV";
            }

            dtDetail = objAgeingDetail.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strNewLocations, strVoucherType, txtSettleSupplier.Text.Split('/')[1].ToString(), "0", "");

            if (dtDetail.Rows.Count > 0)
            {
                GVSettleMentDebit.DataSource = dtDetail;
                GVSettleMentDebit.DataBind();
                SettleDR.Visible = true;
                //ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
                //ddlForeginCurrency_SelectedIndexChanged(sender, e);
                foreach (GridViewRow gvr in GVSettleMentDebit.Rows)
                {
                    ((TextBox)gvr.FindControl("txtSettleAmount")).Enabled = false;
                    Label lblInvAmt = (Label)gvr.FindControl("lblinvamount");
                    Label lblBalanceAmt = (Label)gvr.FindControl("lblBalanceAmount");
                    lblInvAmt.Text = SetDecimal(lblInvAmt.Text);
                    lblBalanceAmt.Text = SetDecimal(lblBalanceAmt.Text);

                }
            }
            else
            {
                GVSettleMentDebit.DataSource = null;
                GVSettleMentDebit.DataBind();
                SettleDR.Visible = false;
                DisplayMessage("No Record Available for Supplier");
                ((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedIndex = 0;
            }
        }
        else
        {
            DisplayMessage("Fill Supplier Name");
            txtSettleSupplier.Focus();
        }
    }
    protected void txtSettleAmount_OnTextChanged(object sender, EventArgs e)
    {
        if (GVSettleMentCredit.Rows.Count == 0)
        {
            DisplayMessage("You have no Advance Amount for Settle");
            txtSettleSupplier.Focus();
            return;
        }
        double settleAmount = 0;
        double SumReceiveAmount = 0;
        GridViewRow gvrowtxt = (GridViewRow)((TextBox)sender).Parent.Parent;
        //string strFireignExchange = GetCurrency(((DropDownList)gvrowtxt.FindControl("ddlgvCurrency")).SelectedValue, ((TextBox)gvrowtxt.FindControl("txtgvSettleAmount")).Text);
        TextBox _txtSettleAmount = (TextBox)gvrowtxt.FindControl("txtSettleAmount");
        Label lblgvSettleTotal = (Label)GVSettleMentDebit.FooterRow.FindControl("lblgvSettleTotal");


        double.TryParse(_txtSettleAmount.Text, out settleAmount);
        //double.TryParse(lblgvSettleTotal.Text, out SumReceiveAmount);

        if (settleAmount > double.Parse(((Label)gvrowtxt.FindControl("lblBalanceAmount")).Text))
        {
            DisplayMessage("Sorry you can't make this changes");
            _txtSettleAmount.Text = "0";
            return;
        }



        string strCheckValue = "False";
        string strVoucherCurrency = "";
        string strVoucherExchangeRate = "";
        double voucherBalance = 0;
        foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
        {
            CheckBox chkCreditValue = (CheckBox)gvr.FindControl("chkSettleCeditId");
            if (chkCreditValue.Checked == true)
            {
                strVoucherCurrency = ((HiddenField)gvr.FindControl("hdnCurrencyId")).Value;
                strVoucherExchangeRate = ((HiddenField)gvr.FindControl("hdnVoucherExchangeRate")).Value;
                double.TryParse(((Label)gvr.FindControl("lblBalanceAmount")).Text, out voucherBalance);
                strCheckValue = "True";
            }
        }

        if (strCheckValue == "False")
        {
            DisplayMessage("Need to select Advance Amount");
            txtSettleSupplier.Focus();
            return;
        }

        foreach (GridViewRow gvr in GVSettleMentDebit.Rows)
        {
            if (((TextBox)gvr.FindControl("txtSettleAmount")).Text == "")
            {
                ((TextBox)gvr.FindControl("txtSettleAmount")).Text = "0";
            }

            SumReceiveAmount = SumReceiveAmount + Convert.ToDouble(((TextBox)gvr.FindControl("txtSettleAmount")).Text);

        }

        if (SumReceiveAmount > voucherBalance)
        {
            DisplayMessage("Sorry you can't make this changes");
            _txtSettleAmount.Text = "0";
            return;
        }
        else
        {
            lblgvSettleTotal.Text = SetDecimal(SumReceiveAmount.ToString());

        }

    }
    protected void btnUpdateAgeing_Click(object sender, EventArgs e)
    {
        if (txtSettleSupplier.Text == "")
        {
            DisplayMessage("Fill " + lblSettleSupplier.Text + " Name");
            txtSettleSupplier.Focus();
            return;
        }

        if (GVSettleMentCredit.Rows.Count == 0)
        {
            DisplayMessage("You have no Advance Amount for Settle");
            txtSettleSupplier.Focus();
            return;
        }

        string strCheckValue = "False";
        double VoucherExchangeRate = 0;
        double voucherBalance = 0;
        string strVoucherCurrency = "";
        string strVoucherId = "";
        foreach (GridViewRow gvr in GVSettleMentCredit.Rows)
        {
            CheckBox chkCreditValue = (CheckBox)gvr.FindControl("chkSettleCeditId");
            if (chkCreditValue.Checked == true)
            {
                strVoucherCurrency = ((HiddenField)gvr.FindControl("hdnCurrencyId")).Value;
                double.TryParse(((HiddenField)gvr.FindControl("hdnVoucherExchangeRate")).Value, out VoucherExchangeRate);
                double.TryParse(((Label)gvr.FindControl("lblBalanceAmount")).Text, out voucherBalance);
                strVoucherId = ((HiddenField)gvr.FindControl("hdnVoucherId")).Value;
                strCheckValue = "True";
            }
        }

        if (strCheckValue == "False")
        {
            DisplayMessage("Need to select Advance Amount");
            txtSettleSupplier.Focus();
            return;
        }

        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());



        double dtSettleAmt = 0;
        double TotalSettleBalance = 0;
        foreach (GridViewRow gvD in GVSettleMentDebit.Rows)
        {
            TextBox _txtSettleAmount = (TextBox)gvD.FindControl("txtSettleAmount");

            //TextBox txtSettleAmt = (TextBox)gvD.FindControl("txtgvSettleAmount");
            double.TryParse(_txtSettleAmount.Text, out dtSettleAmt);

            TotalSettleBalance = TotalSettleBalance + dtSettleAmt;

            if (dtSettleAmt > double.Parse(((Label)gvD.FindControl("lblBalanceAmount")).Text))
            {
                DisplayMessage("Sorry settled amount exceed from balance amount");
                return;
            }
        }

        if (TotalSettleBalance == 0)
        {
            DisplayMessage("You need to settle Amount then you can update it");
            return;
        }

        //For Check Amount
        double SumReceiveAmount = 0;


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {


            if (objAgeingDetailOld.updateAgeingPendingInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), double.Parse(strVoucherId), GVSettleMentDebit, trns, true) == false)
            {
                DisplayMessage("Unable to save Record");
                trns.Rollback();

                return;
            }
            trns.Commit();

        }
        catch (Exception Ex)
        {
            DisplayMessage(Ex.Message);
            trns.Rollback();
        }
        finally
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
        }
    

        DisplayMessage("Record Updated", "green");
        btnSettleSupplierAdd_OnClick(sender, e);
    }
    protected void btnAgeingReset_Click(object sender, EventArgs e)
    {
        txtSettleSupplier.Text = "";
        GVSettleMentCredit.DataSource = null;
        GVSettleMentCredit.DataBind();
        GVSettleMentDebit.DataSource = null;
        GVSettleMentDebit.DataBind();
        SettleCR.Visible = false;
        SettleDR.Visible = false;
    }
    protected void chkSettleCeditId_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrowchk = (GridViewRow)((CheckBox)sender).Parent.Parent;

        foreach (GridViewRow gv in GVSettleMentCredit.Rows)
        {
            CheckBox chkTrans = (CheckBox)gv.FindControl("chkSettleCeditId");
            //HiddenField hdnTransId = (HiddenField)gv.FindControl("hdnCrTransId");

            chkTrans.Checked = false;
        }
        ((CheckBox)gvrowchk.FindControl("chkSettleCeditId")).Checked = true;
        string strVoucherCurrency = ((HiddenField)gvrowchk.FindControl("hdnCurrencyId")).Value;

        double settleAmount = 0;
        double totalSettleAmount = 0;

        foreach (GridViewRow gv in GVSettleMentDebit.Rows)
        {
            TextBox _txtSettleAmount = (TextBox)gv.FindControl("txtSettleAmount");

            _txtSettleAmount.Text = "0";
            if (strVoucherCurrency != ((HiddenField)gv.FindControl("hdnCurrencyId")).Value)
            {
                _txtSettleAmount.Text = "0";
                _txtSettleAmount.Enabled = false;
            }
            else
            {
                _txtSettleAmount.Enabled = true;
            }

            double.TryParse(_txtSettleAmount.Text, out settleAmount);
            totalSettleAmount = totalSettleAmount + settleAmount;
            //HiddenField hdnTransId = (HiddenField)gv.FindControl("hdnCrTransId");

            //chkTrans.Checked = false;
        }
        if (GVSettleMentDebit.Rows.Count > 0)
        {
            Label lblgvSettleTotal = (Label)GVSettleMentDebit.FooterRow.FindControl("lblgvSettleTotal");
            lblgvSettleTotal.Text = SetDecimal(totalSettleAmount.ToString());
        }


    }


    #endregion


    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    //{


    //    Set_Suppliers ObjSupplier = new Set_Suppliers();
    //    Set_CustomerMaster ObjCustomer = new Set_CustomerMaster();
    //    //for customer

    //    DataTable dtFilter = new DataTable();
    //    if (HttpContext.Current.Session["ParentPageId"].ToString() == "1")
    //    {
    //        dtFilter = ObjCustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


    //        string filtertext = "Name like '%" + prefixText + "%'";
    //        dtFilter = new DataView(dtFilter, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    ///supplier
    //    if (HttpContext.Current.Session["ParentPageId"].ToString() == "2")
    //    {

    //        dtFilter = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);



    //    }



    //    string[] filterlist = new string[dtFilter.Rows.Count];
    //    if (dtFilter.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dtFilter.Rows.Count; i++)
    //        {
    //            if (HttpContext.Current.Session["ParentPageId"].ToString() == "1")
    //            {
    //                filterlist[i] = dtFilter.Rows[i]["Name"].ToString() + "/" + dtFilter.Rows[i]["Customer_Id"].ToString();
    //            }
    //            if (HttpContext.Current.Session["ParentPageId"].ToString() == "2")
    //            {
    //                filterlist[i] = dtFilter.Rows[i]["Name"].ToString() + "/" + dtFilter.Rows[i]["Supplier_Id"].ToString();
    //            }
    //        }
    //    }
    //    return filterlist;
    //}

    public bool updateAgeingPendingInvoice(string StrCompId, string StrBrandId, string strLocationId, double voucherId, GridView gvAgeing, SqlTransaction trns, Boolean isSettlementGrid = false)
    {
        //------------------------code to update ageing by neelkanth purohit 18-feb-2017--------------------------
        Ac_Voucher_Detail objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        Ac_Voucher_Header objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        Ac_ParameterMaster objAccParameter = new Ac_ParameterMaster(StrCompId);
        Ac_Ageing_Detail objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        DataTable dtVoucherDetail;
        DataTable dtVoucherHeader;

        string strNarration;

        dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, voucherId.ToString(), ref trns);


        //dtVoucherHeader = new DataView(dtVoucherHeader, "ReconciledFromFinance='true'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtVoucherHeader.Rows.Count == 0)
        {
            return false;
        }


        dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, voucherId.ToString(), ref trns);
        dtVoucherDetail = new DataView(dtVoucherDetail, "Other_account_no >0", "", DataViewRowState.CurrentRows).ToTable();



        DataRow drVoucherHeader = dtVoucherHeader.Rows[0];

        strNarration = drVoucherHeader["Narration"].ToString();

        string transaction_currency = dtVoucherDetail.Rows[0]["Currency_Id"].ToString();
        string strExchangeRate = dtVoucherDetail.Rows[0]["Exchange_Rate"].ToString();

        //String strDetailNarration = string.Empty;
        double ageLocalAmount = 0;
        double ageForeignAmount = 0;
        double ageCompanyAmount = 0;
        double exchangeRate = 0;
        Boolean localTransaction;
        localTransaction = (SystemParameter.GetLocationCurrencyId(trns.Connection.ConnectionString,StrCompId,strLocationId) == transaction_currency ? true : false);
        if (localTransaction == true)
        {
            exchangeRate = 1;
        }
        else
        {
            double.TryParse(strExchangeRate, out exchangeRate);
        }

        foreach (GridViewRow gvr in gvAgeing.Rows)
        {
            if ((isSettlementGrid == false ? ((CheckBox)gvr.FindControl("chkTrandId")).Checked : true) == true)
            {

                Label lblgvInvoiceNo = (Label)gvr.FindControl("lblPONo");
                strNarration = "Paid Amount for That Invoices : " + lblgvInvoiceNo.Text;
                TextBox txtPayAmount = (isSettlementGrid == false ? (TextBox)gvr.FindControl("txtpayLocal") : (TextBox)gvr.FindControl("txtSettleAmount"));
                if (localTransaction == true)
                {
                    double.TryParse(txtPayAmount.Text, out ageLocalAmount);
                    ageForeignAmount = ageLocalAmount;
                    double.TryParse(SystemParameter.GetCurrency(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), trns.Connection.ConnectionString,StrCompId,strLocationId), out ageCompanyAmount);
                }
                else
                {
                    double.TryParse(txtPayAmount.Text, out ageForeignAmount);

                    ageLocalAmount = double.Parse(SystemParameter.SetDecimal((ageForeignAmount * exchangeRate).ToString(), trns.Connection.ConnectionString,StrCompId,strLocationId));
                    double.TryParse(SystemParameter.GetCurrency(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), trns.Connection.ConnectionString,StrCompId,strLocationId), out ageCompanyAmount);
                }
                if (ageLocalAmount > 0)
                {
                    objAgeingDetail.InsertAgeingDetail(StrCompId, StrBrandId, ((HiddenField)gvr.FindControl("hdnLocationId")).Value, ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, lblgvInvoiceNo.Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, ((HiddenField)gvr.FindControl("hdnAccountNo")).Value, ((HiddenField)gvr.FindControl("hdnOtherAccountNo")).Value, "0", ageLocalAmount.ToString(), "0", drVoucherHeader["Cheque_Issue_Date"].ToString(), drVoucherHeader["Cheque_Clear_Date"].ToString(), drVoucherHeader["Cheque_No"].ToString(), strNarration.ToString(), HttpContext.Current.Session["EmpId"].ToString(), transaction_currency.ToString(), exchangeRate.ToString(), ageForeignAmount.ToString(), "0.00", ageCompanyAmount.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ((HiddenField)gvr.FindControl("hdnAgeingType")).Value, voucherId.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), (drVoucherHeader["Field3"].ToString() == "Pending" ? "False" : "True"), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

            }
        }
        return true;

    }

    #region Direct Settle Amount
    protected void rbtnCheck_Changed(object sender, EventArgs e)
    {
        if (rbtnVoucher.Checked)
        {
            VoucherPannel.Visible = true;
            DirectPannel.Visible = false;
        }
        else
        {
           
            VoucherPannel.Visible = false;
            DirectPannel.Visible = true;
        }
    }
    protected void btnDirectSettleSupplierAdd_OnClick(object sender, EventArgs e)
    {
       

        string sql = string.Empty;
        string strCondition = string.Empty;
        //Get Location id from cash flow
        string strNewLocations = string.Empty;
        string strLocationId = Session["LocId"].ToString();
        string strCurrencyId = Session["LocCurrencyId"].ToString();

        
        if (txtDirectSupplier.Text != "")
        {
            //For Settlement Debit
            //sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id, Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail   group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,IsActive  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and AgeingType='PV' and IsActive='True'";
            ////da.return_DataTable(sql);
            //DataTable dtDetail = da.return_DataTable(sql);
            DataTable dtDetail = new DataTable();
            string strVoucherType = string.Empty;
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                strVoucherType = "RV";
            }
            else
            {
                strVoucherType = "PV";
            }


            try
            {
                string sql1 = "SELECT STUFF((SELECT ',' + Param_Value FROM Ac_Parameter_Location where company_id='" + Session["CompId"].ToString() + "' and location_id='" + strLocationId + "' and Param_Name='CashFlowLocation' and isActive='true' FOR XML PATH('') ), 1, 1, '')";
                strNewLocations = da.get_SingleValue(sql1).ToString();
            }
            catch (Exception ex)
            {

            }



            dtDetail = objAgeingDetail.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strNewLocations, strVoucherType, txtDirectSupplier.Text.Split('/')[1].ToString(), "0", "");

            if (dtDetail.Rows.Count > 0)
            {
                try
                {
                    gvDirectDebit.DataSource = dtDetail;
                    gvDirectDebit.DataBind();
                }
                catch(Exception ex)
                {

                }
                SettleDR.Visible = true;                
                foreach (GridViewRow gvr in gvDirectDebit.Rows)
                {
                    ((TextBox)gvr.FindControl("txtDirectSettleAmount")).Enabled = true;
                    Label lblInvAmt = (Label)gvr.FindControl("lblDirectinvamount");
                    Label lblBalanceAmt = (Label)gvr.FindControl("lblDirectBalanceAmount");
                    lblInvAmt.Text = SetDecimal(lblInvAmt.Text);
                    lblBalanceAmt.Text = SetDecimal(lblBalanceAmt.Text);
                }
            }
            else
            {
                gvDirectDebit.DataSource = null;
                gvDirectDebit.DataBind();
                SettleDR.Visible = false;
                DisplayMessage("No Record Available for Supplier");
                ((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedIndex = 0;
            }
        }
        else
        {
            DisplayMessage("Fill Supplier Name");
            txtDirectSupplier.Focus();
        }

    }
    protected void txtDirectSettleAmount_OnTextChanged(object sender, EventArgs e)
    {
        string Message = "";
        foreach(GridViewRow gvr in gvDirectDebit.Rows)
        {
            Label lblBalanceAmount = (Label)gvr.FindControl("lblDirectBalanceAmount");
            TextBox txtAmount = (TextBox)gvr.FindControl("txtDirectSettleAmount");
            if (txtAmount.Text != "")
            {
                float amount = float.Parse(txtAmount.Text);
                float balanceAmount = float.Parse(lblBalanceAmount.Text);
                if (amount > balanceAmount)
                {
                    txtAmount.Text = "";
                    Message = "Invalid Amount";
                }
            }
        }
        if (Message != "")
        {
            DisplayMessage("Invalid Amount");
        }
    }
    protected void btnDirectSave_Click(object sender, EventArgs e) 
    {
        string connection = Session["DBConnection"].ToString();
        using (SqlConnection connec = new SqlConnection(connection))
        {
            connec.Open();
            using (SqlTransaction trnss = connec.BeginTransaction())
            {
             
                try
                {
                    string strCompId = Session["CompId"].ToString();
                    string strLocationId = Session["LocId"].ToString();
                    string strBrandId = Session["BrandId"].ToString();
                    double exchangeRate = 0;

                    foreach (GridViewRow gvr in gvDirectDebit.Rows)
                    {
                       
                        string transaction_currency = ddlForeginCurrency.SelectedValue.ToString();
                        bool localTransaction = SystemParameter.GetLocationCurrencyId(connection, strCompId, strLocationId) == transaction_currency;

                        exchangeRate = Convert.ToDouble(txtExchangeRate.Text.ToString()); // You might want to change this logic if the exchange rate is not always 1 for non-local transactions

                        double ageLocalAmount = 0;
                        double ageCompanyAmount = 0;
                        double ageForeignAmount = 0;

                        Label lblgvInvoiceNo = (Label)gvr.FindControl("lblDirectPONo");
                        string strNarration = "Paid Amount for That Invoices: " + lblgvInvoiceNo.Text;
                        TextBox txtPayAmount = (TextBox)gvr.FindControl("txtDirectSettleAmount");

                        if (localTransaction)
                        {
                            double.TryParse(txtPayAmount.Text, out ageLocalAmount);
                            ageForeignAmount = ageLocalAmount;
                            double.TryParse(SystemParameter.GetCurrency(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), connection, strCompId, strLocationId), out ageCompanyAmount);
                        }
                        else
                        {
                            double.TryParse(txtPayAmount.Text, out ageForeignAmount);
                            ageLocalAmount = double.Parse(SystemParameter.SetDecimal((ageForeignAmount * exchangeRate).ToString(), connection, strCompId, strLocationId));
                            double.TryParse(SystemParameter.GetCurrency(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), connection, strCompId, strLocationId), out ageCompanyAmount);
                        }
                        if (ageLocalAmount > 0)
                        {
                            objAgeingDetail.InsertAgeingDetail(strCompId, strBrandId, ((HiddenField)gvr.FindControl("hdnDirectLocationId")).Value, ((HiddenField)gvr.FindControl("hdnDirectRefType")).Value, ((HiddenField)gvr.FindControl("hdnDirectRefId")).Value, lblgvInvoiceNo.Text, ((Label)gvr.FindControl("lblDirectInvoiceDate")).Text, ((HiddenField)gvr.FindControl("hdnDirectAccountNo")).Value, ((HiddenField)gvr.FindControl("hdnDirectOtherAccountNo")).Value, "0", ageLocalAmount.ToString(), "0", "1900-01-01", "1900-01-01", "0", strNarration.ToString(), HttpContext.Current.Session["EmpId"].ToString(), transaction_currency.ToString(), exchangeRate.ToString(), ageForeignAmount.ToString(), "0.00", ageCompanyAmount.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ((HiddenField)gvr.FindControl("hdnDirectAgeingType")).Value, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                    }

                    trnss.Commit();
                    DisplayMessage("Record Updated", "green");
                    ResetDirect();
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message);
                    trnss.Rollback();
                }
                finally
                {
                    if (connec.State == System.Data.ConnectionState.Open)
                    {
                        connec.Close();
                    }
                }
            }
        }



    }

    public void ResetDirect()
    {
        gvDirectDebit.DataSource = null;
        gvDirectDebit.DataBind();
        txtDirectSupplier.Text = "";
        ddlForeginCurrency.Items.Clear();
        ddlForeginCurrency.DataSource = null; ;
        ddlForeginCurrency.DataBind();
        txtExchangeRate.Text = "";

    }
    protected void btnDirectReset_Click(object sender, EventArgs e)
    {
        ResetDirect();
    }
    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForeginCurrency.SelectedIndex > 0)
        {
          string LocalCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

          string ExchangeRate= SystemParameter.GetExchageRate(ddlForeginCurrency.SelectedValue, LocalCurrency, Session["DBConnection"].ToString());
            //  string  strFireignExchange = hdnFExchangeRate.Value;
          txtExchangeRate.Text = ExchangeRate;
            
        }

     
    }

    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlForeginCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {            
            ddlForeginCurrency.Items.Insert(0, "--Select--");
            ddlForeginCurrency.SelectedIndex = 0;
        }
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            int otherAccountId = 0;
            int.TryParse(txtDirectSupplier.Text.Split('/')[1].ToString(), out otherAccountId);
          
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtDirectSupplier.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                    
                        ddlForeginCurrency.SelectedValue = dt.Rows[0]["Currency_id"].ToString();
                        ddlForeginCurrency_SelectedIndexChanged(null,null);
                    }

                }
            }
        }
        catch
        {
            DisplayMessage("Supplier is not valid");
            txtDirectSupplier.Text = "";
            txtDirectSupplier.Focus();

        }
        

    }
    #endregion



}