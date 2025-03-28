using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Globalization;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class GeneralLedger_DailyCashFlow : System.Web.UI.Page
{
    SystemParameter objsys = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    LocationMaster objLocation = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Groups ObjACGroup = null;
    Ac_CashFlow_Header ObjCashHeader = null;
    Ac_CashFlow_Detail ObjCashDetail = null;
    Ems_ContactMaster ObjContact = null;
    Set_ApplicationParameter objAppParam = null;
    Ac_SubChartOfAccount objSubCOA = null;
    HolidayMaster objHoliday = null;
    Common cmn = null;
    EmployeeMaster objEmployee = null;
    DataAccessClass ObjDa = null;
    UserMaster objUser = null;
    PageControlCommon objPageCmn = null;

    string StrUserId = string.Empty;
    static string MaxDate = "";
    static bool havePermission = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjACGroup = new Ac_Groups(Session["DBConnection"].ToString());
        ObjCashHeader = new Ac_CashFlow_Header(Session["DBConnection"].ToString());
        ObjCashDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        ObjContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        objHoliday = new HolidayMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/DailyCashFlow.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillLocationList();
            hdnLocId.Value = Session["LocId"].ToString();
            hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            FillDate();
            btnList_Click(sender, e);
            FillGridBin();
            FillGrid();
            //AllPageCode();
        }
    }
    public void FillDate()
    {
        string strCurrency = hdnCurrencyId.Value;
        string WeekOff = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        bool WeekOffValue = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowWeekOff"));
        bool HolidayValue = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowHoliday"));

        string strMaxId = string.Empty;
        DataTable dtMaxId = ObjCashHeader.GetCashFlowMaxIdPosted(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value);
        if (dtMaxId.Rows.Count > 0)
        {
            strMaxId = dtMaxId.Rows[0][0].ToString();

            if (strMaxId != "" && strMaxId != "0")
            {
                DataTable dtRecord = ObjCashHeader.GetCashFlowByCashFlowId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId);
                if (dtRecord.Rows.Count > 0)
                {
                    DateTime dtLastDate = DateTime.Parse(dtRecord.Rows[0]["CF_Date"].ToString());
                    txtDate.Text = dtLastDate.AddDays(1).ToString("dd-MMM-yyyy");

                    if (txtDate.Text != "")
                    {
                        if (WeekOffValue)
                        {
                            bool IsWeekOff = false;
                            foreach (string str in WeekOff.Split(','))
                            {
                                if (str == DateTime.Parse(txtDate.Text).DayOfWeek.ToString())
                                {
                                    IsWeekOff = true;
                                    if (IsWeekOff == true)
                                    {
                                        txtDate.Text = DateTime.Parse(txtDate.Text).AddDays(1).ToString("dd-MMM-yyyy");
                                    }
                                }
                            }
                        }
                        if (HolidayValue)
                        {
                            DataTable dtHoliday = objHoliday.GetHolidayMasterByCompanyOnly(Session["CompId"].ToString());
                            if (dtHoliday.Rows.Count > 0)
                            {
                                dtHoliday = new DataView(dtHoliday, "Field1='" + hdnLocId.Value + "' and From_Date='" + txtDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtHoliday.Rows.Count > 0)
                                {
                                    DateTime dtFromDate = DateTime.Parse(dtHoliday.Rows[0]["From_Date"].ToString());
                                    DateTime dtToDate = DateTime.Parse(dtHoliday.Rows[0]["To_Date"].ToString());

                                    if (dtToDate >= dtFromDate)
                                    {
                                        string strDiffrenceDays = (dtToDate - dtFromDate).TotalDays.ToString();
                                        string strFinalDays = (Convert.ToDouble(strDiffrenceDays) + 1).ToString();

                                        if (strFinalDays != "")
                                        {
                                            txtDate.Text = DateTime.Parse(txtDate.Text).AddDays(int.Parse(strFinalDays)).ToString("dd-MMM-yyyy");
                                        }
                                    }
                                }
                            }
                        }
                    }


                    txtOpeningBalance.Text = dtRecord.Rows[0]["CF_ClosingAmount"].ToString();
                    txtOpeningBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, txtOpeningBalance.Text);
                }
            }
            else
            {
                txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        else
        {
            txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        FillGrid();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnGetReport.Visible = btnShowReport.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        havePermission = clsPagePermission.bModifyDate;
    }
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        DateTime dtToDate = new DateTime();
        if (txtDate.Text == "")
        {
            DisplayMessage("Fill date First");
            txtDate.Focus();
            return;
        }
        else if (txtDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtDate.Text);

                Convert.ToDateTime(txtDate.Text);
                dtToDate = Convert.ToDateTime(txtDate.Text);
                dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtDate.Focus();
                return;
            }
        }

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        //for Account & Location Parameter        
        string strLocationId = string.Empty;
        DataTable dtLocation = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowLocation");
        if (dtLocation.Rows.Count > 0)
        {
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                if (strLocationId == "")
                {
                    strLocationId = dtLocation.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strLocationId = strLocationId + "," + dtLocation.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strLocationId = hdnLocId.Value;
        }



        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowAccount");
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


        //here we are checking that voucher is appproved or not before make cash flow


        string strreturnValue = ObjDa.get_SingleValue("select count( Ac_Voucher_Header.Voucher_No) from Ac_Voucher_Header inner join Ac_Voucher_Detail on Ac_Voucher_Header.Trans_Id = Ac_Voucher_Detail.Voucher_No where Ac_Voucher_Header.Voucher_Date='" + Convert.ToDateTime(txtDate.Text).ToString() + "' and Ac_Voucher_Header.Location_Id in (" + strLocationId + ") and Ac_Voucher_Detail.Account_No in (" + strAccountId + ") and Ac_Voucher_Header.Field3='Pending' and Ac_Voucher_Header.IsActive='true'");


        if (strreturnValue != "0")
        {
            DisplayMessage("Unable to create cash flow, Please approve pending voucher");
            return;
        }

        strreturnValue = ObjDa.get_SingleValue("select count( Ac_Voucher_Header.Voucher_No) from Ac_Voucher_Header inner join Ac_Voucher_Detail on Ac_Voucher_Header.Trans_Id = Ac_Voucher_Detail.Voucher_No where Ac_Voucher_Header.Voucher_Date='" + Convert.ToDateTime(txtDate.Text).ToString() + "' and Ac_Voucher_Header.Location_Id in (" + strLocationId + ") and Ac_Voucher_Detail.Account_No in (" + strAccountId + ") and Ac_Voucher_Header.ReconciledFromFinance = 'False' and Ac_Voucher_Header.IsActive='true'");


        if (strreturnValue != "0")
        {
            DisplayMessage("Unable to create cash flow, Please check transfer in finance there are some cash flow related vouchers for " + txtDate.Text);
            return;
        }



        DataTable dtCashflow = objVoucherDetail.GetStatementDetail();
        if (dtCashflow.Rows.Count > 0)
        {
            if (txtDate.Text != "")
            {
                dtCashflow = new DataView(dtCashflow, "Voucher_Date >= '" + txtDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            }

            if (strLocationId != "" && strAccountId != "")
            {
                dtCashflow = new DataView(dtCashflow, "Location_Id in (" + strLocationId + ") and Account_No in (" + strAccountId + ")", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strLocationId != "")
            {
                dtCashflow = new DataView(dtCashflow, "Location_Id in (" + strLocationId + ")", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strAccountId != "")
            {
                dtCashflow = new DataView(dtCashflow, "Account_No in (" + strAccountId + ")", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            }

            if (dtCashflow.Rows.Count > 0)
            {
                GVCashFlow.DataSource = dtCashflow;
                GVCashFlow.DataBind();
            }
            else
            {
                GVCashFlow.DataSource = null;
                GVCashFlow.DataBind();
            }

            string strCurrency = hdnCurrencyId.Value;
            double debitamount = 0;
            double creditamount = 0;
            double debitTotalamount = 0;
            double creditTotalamount = 0;
            string strStatus = "False";
            string strBalanceA = string.Empty;
            foreach (GridViewRow gvr in GVCashFlow.Rows)
            {
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditAmount.Text);
                debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");

                if (strStatus == "False")
                {
                    if (txtOpeningBalance.Text != "")
                    {
                        lblgvBalance.Text = txtOpeningBalance.Text;
                        strStatus = "True";
                    }
                    else
                    {
                        lblgvBalance.Text = "0";
                        strStatus = "True";
                    }
                }
                else
                {
                    lblgvBalance.Text = strBalanceA;
                }

                if (debitamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(debitamount.ToString())).ToString();
                    lblgvBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvBalance.Text);
                    strBalanceA = lblgvBalance.Text;
                }
                else if (creditamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(creditamount.ToString())).ToString();
                    lblgvBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvBalance.Text);
                    strBalanceA = lblgvBalance.Text;
                }
                else
                {
                    lblgvBalance.Text = "0.00";
                    strBalanceA = "0.00";
                }

                debitTotalamount += debitamount;
                creditTotalamount += creditamount;
                txtClosingBalance.Text = strBalanceA;
                txtClosingBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, txtClosingBalance.Text);
            }


            if (GVCashFlow.Rows.Count > 0)
            {
                Label lblgvDebitTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvDebitTotal");
                Label lblgvCreditTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvCreditTotal");
                Label lblgvBalanceTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvBalanceTotal");

                lblgvDebitTotal.Text = debitTotalamount.ToString();
                lblgvCreditTotal.Text = creditTotalamount.ToString();
                lblgvBalanceTotal.Text = strBalanceA;

                lblgvDebitTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitTotal.Text);
                lblgvCreditTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditTotal.Text);
                lblgvBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvBalanceTotal.Text);
            }
            else
            {
                txtClosingBalance.Text = txtOpeningBalance.Text;
                GVCashFlow.DataSource = null;
                GVCashFlow.DataBind();
                DisplayMessage("You have no record available");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
            }

            //For Bank Account
            string strBankAccountId = string.Empty;
            string strBankAccount = "False";
            DataTable dtBankAcc = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "BankAccount");
            if (dtBankAcc.Rows.Count > 0)
            {
                for (int i = 0; i < dtBankAcc.Rows.Count; i++)
                {
                    if (strBankAccountId == "")
                    {
                        strBankAccountId = dtBankAcc.Rows[i]["Param_Value"].ToString();
                    }
                    else
                    {
                        strBankAccountId = strBankAccountId + "," + dtBankAcc.Rows[i]["Param_Value"].ToString();
                    }
                }
            }
            else
            {
                strBankAccountId = "0";
            }


            //for Accounts Summarized
            if (dtAccount.Rows.Count > 0)
            {
                lblAccountsSummarized.Visible = true;
                GvSummarized.DataSource = dtAccount;
                GvSummarized.DataBind();

                foreach (GridViewRow gv in GvSummarized.Rows)
                {
                    HiddenField hdnAccountId = (HiddenField)gv.FindControl("hdnAccountId");
                    Label lblSystemBalance = (Label)gv.FindControl("lblgvSystemBalance");
                    TextBox txtPhysicalBalance = (TextBox)gv.FindControl("txtgvPhysicalBalance");

                    //Get Opening Balance   
                    double OpeningBalance = 0;
                    PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                    DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + txtDate.Text + "', '0','" + hdnAccountId.Value + "','0','1', '" + Session["FinanceYearId"].ToString() + "')) OpeningBalance");
                    if (dtOpeningBalance.Rows.Count > 0)
                    {
                        OpeningBalance = Convert.ToDouble(dtOpeningBalance.Rows[0][0].ToString());
                        lblSystemBalance.Text = OpeningBalance.ToString();
                    }
                }

                if (GvSummarized.Rows.Count > 0)
                {
                    //Label lblgvSystemBalanceTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvSystemBalanceTotal");
                    //lblgvSystemBalanceTotal.Text = TotalSystemBalance.ToString();
                    //lblgvSystemBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvSystemBalanceTotal.Text);
                }
                else
                {
                    DisplayMessage("You have no record available");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
                }
            }
            else
            {
                lblAccountsSummarized.Visible = false;
                GvSummarized.DataSource = null;
                GvSummarized.DataBind();
            }


            //Add Code On 10-01-2017 for Update Bank Balances
            double TotalSystemBalance = 0;
            foreach (GridViewRow gv in GvSummarized.Rows)
            {
                HiddenField hdnAccountId = (HiddenField)gv.FindControl("hdnAccountId");
                Label lblSystemBalance = (Label)gv.FindControl("lblgvSystemBalance");
                double SystemBalance = 0;

                try
                {
                    SystemBalance = Convert.ToDouble(lblSystemBalance.Text);
                }
                catch
                {
                    SystemBalance = 0;
                }

                TextBox txtPhysicalBalance = (TextBox)gv.FindControl("txtgvPhysicalBalance");

                if (strBankAccountId.Split(',').Contains(hdnAccountId.Value))
                {
                    strBankAccount = "True";
                }
                else
                {
                    strBankAccount = "False";
                }

                double DueBalance = 0;
                if (strBankAccount == "True")
                {
                    PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                    DataTable dtDueBalance = objDA.return_DataTable("select isnull( sum(debit_amount-credit_amount),0) as DueBalance from ac_voucher_detail  inner join Ac_Voucher_Header  on Ac_Voucher_Header.Trans_Id=ac_voucher_detail.Voucher_No  where Account_No='" + hdnAccountId.Value + "'  and ac_voucher_header.ReconciledFromFinance='True'   AND (Ac_Voucher_Header.Field3 = ''    OR Ac_Voucher_Header.Field3 IS NULL    OR Ac_Voucher_Header.Field3 = 'Approved')  and voucher_date <='" + txtDate.Text + "'   and ac_voucher_header.isactive='true'   and (Ac_Voucher_Detail.Field2='' or Ac_Voucher_Detail.Field2='false')  and ac_voucher_header.Location_Id in (" + strLocationId + ")");
                    if (dtDueBalance.Rows.Count > 0 && dtDueBalance != null)
                    {
                        DueBalance = Convert.ToDouble(dtDueBalance.Rows[0]["DueBalance"].ToString());
                        if (DueBalance != 0)
                        {
                            lblSystemBalance.Text = (SystemBalance - DueBalance).ToString();
                        }
                    }
                }

                lblSystemBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblSystemBalance.Text);

                double SysBal = Convert.ToDouble(lblSystemBalance.Text);
                if (SysBal == 0)
                {
                    txtPhysicalBalance.Text = "0.00";
                }
                TotalSystemBalance += Convert.ToDouble(lblSystemBalance.Text);
            }
            if (GvSummarized.Rows.Count > 0)
            {
                Label lblgvSystemBalanceTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvSystemBalanceTotal");

                lblgvSystemBalanceTotal.Text = TotalSystemBalance.ToString();
                lblgvSystemBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvSystemBalanceTotal.Text);
            }


            //for Customer Cheques
            DataTable dtCustomerCheque = ObjCashHeader.AllChequeInformationForCustomer();
            if (dtCustomerCheque.Rows.Count > 0)
            {
                dtCustomerCheque = new DataView(dtCustomerCheque, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id in (" + strLocationId + ") and DetailAccountNo in (" + strBankAccountId + ") and Voucher_Date <= '" + txtDate.Text + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                if (dtCustomerCheque.Rows.Count > 0)
                {
                    lblPostdatedCheque.Visible = true;
                    lblCustomersCheque.Visible = true;

                    GvCustomerChequeDetail.DataSource = dtCustomerCheque;
                    GvCustomerChequeDetail.DataBind();

                    double TotalChequeAmt = 0;
                    foreach (GridViewRow gvC in GvCustomerChequeDetail.Rows)
                    {
                        Label lblgvAmount = (Label)gvC.FindControl("lblgvChequeAmount");

                        if (lblgvAmount.Text != "")
                        {
                            lblgvAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvAmount.Text);
                            TotalChequeAmt += Convert.ToDouble(lblgvAmount.Text);
                        }
                    }

                    if (GvCustomerChequeDetail.Rows.Count > 0)
                    {
                        Label lblgvChequeAmtTotal = (Label)GvCustomerChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");

                        lblgvChequeAmtTotal.Text = TotalChequeAmt.ToString();
                        lblgvChequeAmtTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvChequeAmtTotal.Text);
                    }
                }
                else
                {
                    lblPostdatedCheque.Visible = false;
                    lblCustomersCheque.Visible = false;
                    GvCustomerChequeDetail.DataSource = null;
                    GvCustomerChequeDetail.DataBind();
                }
            }
            else
            {
                lblPostdatedCheque.Visible = false;
                lblCustomersCheque.Visible = false;
                GvCustomerChequeDetail.DataSource = null;
                GvCustomerChequeDetail.DataBind();
            }
            //For Supplier Cheque
            DataTable dtSupplierCheque = ObjCashHeader.AllChequeInformationForSupplier();
            if (dtSupplierCheque.Rows.Count > 0)
            {
                dtSupplierCheque = new DataView(dtSupplierCheque, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id in (" + strLocationId + ") and DetailAccountNo in (" + strBankAccountId + ") and Voucher_Date <= '" + txtDate.Text + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                if (dtSupplierCheque.Rows.Count > 0)
                {
                    lblPostdatedCheque.Visible = true;
                    lblSupplierCheque.Visible = true;
                    GvSupplierChequeDetail.DataSource = dtSupplierCheque;
                    GvSupplierChequeDetail.DataBind();

                    double TotalChequeAmt = 0;
                    foreach (GridViewRow gvS in GvSupplierChequeDetail.Rows)
                    {
                        Label lblgvAmount = (Label)gvS.FindControl("lblgvChequeAmount");

                        if (lblgvAmount.Text != "")
                        {
                            lblgvAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvAmount.Text);
                            TotalChequeAmt += Convert.ToDouble(lblgvAmount.Text);
                        }
                    }

                    if (GvSupplierChequeDetail.Rows.Count > 0)
                    {
                        Label lblgvChequeAmtTotal = (Label)GvSupplierChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");

                        lblgvChequeAmtTotal.Text = TotalChequeAmt.ToString();
                        lblgvChequeAmtTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvChequeAmtTotal.Text);
                    }
                }
                else
                {
                    lblSupplierCheque.Visible = false;
                    GvSupplierChequeDetail.DataSource = null;
                    GvSupplierChequeDetail.DataBind();
                }
            }
            else
            {
                lblSupplierCheque.Visible = false;
                GvSupplierChequeDetail.DataSource = null;
                GvSupplierChequeDetail.DataBind();
            }

            //for Summarized Total
            double gvSummrized = 0;
            double gvCustomer = 0;
            double gvSupplier = 0;
            if (GvSummarized.Rows.Count > 0)
            {
                Label lblgvSummarizedTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvSystemBalanceTotal");
                if (lblgvSummarizedTotal.Text != "")
                {
                    gvSummrized = Convert.ToDouble(lblgvSummarizedTotal.Text);
                }
            }

            if (GvCustomerChequeDetail.Rows.Count > 0)
            {
                Label lblgvCustomerTotal = (Label)GvCustomerChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");
                if (lblgvCustomerTotal.Text != "")
                {
                    gvCustomer = Convert.ToDouble(lblgvCustomerTotal.Text);
                }
            }

            if (GvSupplierChequeDetail.Rows.Count > 0)
            {
                Label lblgvSupplierTotal = (Label)GvSupplierChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");
                if (lblgvSupplierTotal.Text != "")
                {
                    gvSupplier = Convert.ToDouble(lblgvSupplierTotal.Text);
                }
            }

            txtSummarizedTotal.Text = Common.GetAmountDecimal((Convert.ToDouble(gvSummrized.ToString()) + Convert.ToDouble(gvCustomer.ToString()) - Convert.ToDouble(gvSupplier.ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtSummarizedTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, txtSummarizedTotal.Text);
        }
        else
        {
            GVCashFlow.DataSource = null;
            GVCashFlow.DataBind();

            GvSummarized.DataSource = null;
            GvSummarized.DataBind();

            GvCustomerChequeDetail.DataSource = null;
            GvCustomerChequeDetail.DataBind();
            GvSupplierChequeDetail.DataSource = null;
            GvSupplierChequeDetail.DataBind();

            DisplayMessage("You have no record available According to your cash flow Generation");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
            return;
        }
    }
    public string GetContactName(string strCustomerId)
    {
        string strCustomerName = string.Empty;
        if (strCustomerId != "" && strCustomerId != "0")
        {
            DataTable dtContact = ObjContact.GetContactAllData();
            if (dtContact.Rows.Count > 0)
            {
                dtContact = new DataView(dtContact, "Trans_Id='" + strCustomerId + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtContact.Rows.Count > 0)
                {
                    strCustomerName = dtContact.Rows[0]["Name"].ToString();
                }
            }
        }
        return strCustomerName;
    }
    public string strDate(string strDateG)
    {
        string strNewDate = string.Empty;
        if (strDateG != "")
        {
            strNewDate = DateTime.Parse(strDateG).ToString("dd-MMM-yyyy");
        }
        return strNewDate;
    }
    public void Reset()
    {
        btnPost.Visible = true;
        btnGetReport.Visible = true;
        btnShowReport.Visible = true;
        txtDate.Enabled = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;

        Calender_VoucherDate.Format = objsys.SetDateFormat();
        txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        txtOpeningBalance.Text = "";
        txtClosingBalance.Text = "";
        txtSummarizedTotal.Text = "";

        GVCashFlow.DataSource = null;
        GVCashFlow.DataBind();

        GvSummarized.DataSource = null;
        GvSummarized.DataBind();

        GvSummarizedView.DataSource = null;
        GvSummarizedView.DataBind();

        lblAccountsSummarized.Visible = false;
        lblPostdatedCheque.Visible = false;
        lblCustomersCheque.Visible = false;
        lblSupplierCheque.Visible = false;

        GvCustomerChequeDetail.DataSource = null;
        GvCustomerChequeDetail.DataBind();

        GvSupplierChequeDetail.DataSource = null;
        GvSupplierChequeDetail.DataBind();
        FillDate();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        //AllPageCode();
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        btnPost_Click(sender, e);

        if (Session["forPrint"].ToString() != "False")
        {
            string strCF_id = Session["forPrint"].ToString();

            string strCmd = string.Format("window.open('../Accounts_Report/DailyCashFlowPrint.aspx?Id=" + strCF_id + "','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
        else
        {
            DisplayMessage("You Missed Some Values");
            return;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    public string GetReconcilesStatus(string strDate)
    {
        string strStatus = "True";
        //for Account & Location Parameter        
        string strLocationId = string.Empty;
        DataTable dtLocation = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowLocation");
        if (dtLocation.Rows.Count > 0)
        {
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                if (strLocationId == "")
                {
                    strLocationId = dtLocation.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strLocationId = strLocationId + "," + dtLocation.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strLocationId = hdnLocId.Value;
        }

        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowAccount");
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

        DataTable dtCashflow = objVoucherHeader.GetAllVoucherHeaderData(Session["FinanceYearId"].ToString());
        if (dtCashflow.Rows.Count > 0)
        {
            if (strDate != "")
            {
                dtCashflow = new DataView(dtCashflow, "Voucher_Date <= '" + strDate + "' and Location_Id in (" + strLocationId + ") and Account_No in (" + strAccountId + ") and IsActive='True' and ReconciledFromFinance='False'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            }

            if (dtCashflow.Rows.Count > 0)
            {
                strStatus = "False";
            }
        }
        return strStatus;
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        Session["forPrint"] = "False";
        string strPost = string.Empty;
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        if (sender is Button)
        {
            Button btnId = (Button)sender;

            if (btnId.ID == "btnPost")
            {
                strPost = "True";
            }

            if (btnId.ID == "btnShowReport")
            {
                strPost = "False";
            }
        }

        if (txtDate.Text == "")
        {
            DisplayMessage("Fill date First");
            txtDate.Focus();
            return;
        }
        else
        {
            txtDate_TextChanged(sender, e);
        }

        if (txtDate.Text != "")
        {
            DataTable dtCashHeader = ObjCashHeader.GetCashFlowByCashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, objsys.getDateForInput(txtDate.Text).ToString());
            if (dtCashHeader.Rows.Count > 0)
            {
                dtCashHeader = new DataView(dtCashHeader, "ReconcileStatus='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCashHeader.Rows.Count > 0)
                {
                    DisplayMessage("For That Date Cash Flow Already Posted");
                    txtDate.Focus();
                    return;
                }
            }
        }

        if (txtOpeningBalance.Text == "")
        {
            txtOpeningBalance.Text = "0.00";
        }
        if (txtClosingBalance.Text == "")
        {
            txtOpeningBalance.Text = "0.00";
        }

        if (GvSummarized.Rows.Count <= 0)
        {
            DisplayMessage("You have no Data");
            txtDate.Focus();
            return;
        }


        if (strPost == "True")
        {
            if (GvSummarized.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in GvSummarized.Rows)
                {
                    HiddenField hdnAccId = (HiddenField)gvr.FindControl("hdnAccountId");
                    Label lblSystemBal = (Label)gvr.FindControl("lblgvSystemBalance");
                    TextBox txtPhysicalBal = (TextBox)gvr.FindControl("txtgvPhysicalBalance");

                    if (txtPhysicalBal.Text == "")
                    {
                        DisplayMessage("Your Physical Balance are Blank");
                        txtPhysicalBal.Focus();
                        return;
                    }

                    double SysBal = Convert.ToDouble(lblSystemBal.Text);

                    double PhyBal = 0;
                    if (txtPhysicalBal.Text != "")
                    {
                        PhyBal = Convert.ToDouble(txtPhysicalBal.Text);
                    }
                    else
                    {
                        PhyBal = 0;
                    }


                    if (SysBal == PhyBal)
                    {

                    }
                    else
                    {
                        DisplayMessage("Your System Balance and Physical Balance are not Equal");
                        txtPhysicalBal.Focus();
                        return;
                    }
                }
            }
        }


        DataTable dtAlready = ObjCashHeader.GetCashFlowByCashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, txtDate.Text);
        if (dtAlready.Rows.Count > 0)
        {
            string strHeaderId = dtAlready.Rows[0]["CF_Id"].ToString();
            if (strHeaderId != "" && strHeaderId != "0")
            {
                ObjCashHeader.DeleteCashFlowHeaderPermanent(strHeaderId);
                ObjCashDetail.DeleteCashFlowDetailPermanent(strHeaderId);
            }
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            b = ObjCashHeader.InsertCashFlowHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, objsys.getDateForInput(txtDate.Text).ToString(), txtOpeningBalance.Text, txtClosingBalance.Text, strPost, txtSummarizedTotal.Text, "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            if (b != 0)
            {
                string strMaxId = string.Empty;
                DataTable dtMaxId = ObjCashHeader.GetCashFlowMaxId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, ref trns);
                if (dtMaxId.Rows.Count > 0)
                {
                    strMaxId = dtMaxId.Rows[0][0].ToString();
                    Session["forPrint"] = strMaxId;
                }

                //for Voucher Detail
                foreach (GridViewRow gvD in GVCashFlow.Rows)
                {
                    HiddenField hdnVD_Id = (HiddenField)gvD.FindControl("hdnVoucherDetailId");

                    if (hdnVD_Id.Value != "")
                    {
                        ObjCashDetail.InsertCashFlowDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId, objsys.getDateForInput(txtDate.Text).ToString(), txtOpeningBalance.Text, txtClosingBalance.Text, "Voucher", hdnVD_Id.Value, "0", "0.00", "0.00", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //for Account Detail
                foreach (GridViewRow gvA in GvSummarized.Rows)
                {
                    HiddenField hdnAccountId = (HiddenField)gvA.FindControl("hdnAccountId");
                    Label lblSystemBalance = (Label)gvA.FindControl("lblgvSystemBalance");
                    TextBox txtPhysicalBalance = (TextBox)gvA.FindControl("txtgvPhysicalBalance");

                    if (txtPhysicalBalance.Text == "")
                    {
                        txtPhysicalBalance.Text = "0.00";
                    }
                    TextBox txtRemarks = (TextBox)gvA.FindControl("txtRemarks");

                    if (hdnAccountId.Value != "")
                    {
                        ObjCashDetail.InsertCashFlowDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId, objsys.getDateForInput(txtDate.Text).ToString(), txtOpeningBalance.Text, txtClosingBalance.Text, "Account", "0", hdnAccountId.Value, lblSystemBalance.Text, txtPhysicalBalance.Text, txtRemarks.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //For Customer Cheque
                foreach (GridViewRow gvC in GvCustomerChequeDetail.Rows)
                {
                    HiddenField hdnVoucherDetailId = (HiddenField)gvC.FindControl("hdnCVD_Id");
                    Label lblChequeAmt = (Label)gvC.FindControl("lblgvChequeAmount");
                    CheckBox chkCus = (CheckBox)gvC.FindControl("chkgvCSelect");

                    //if (chkCus.Checked == true)
                    //{
                    if (hdnVoucherDetailId.Value != "")
                    {
                        ObjCashDetail.InsertCashFlowDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId, objsys.getDateForInput(txtDate.Text).ToString(), txtOpeningBalance.Text, txtClosingBalance.Text, "CusCheque", hdnVoucherDetailId.Value, "0", lblChequeAmt.Text, "0.00", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //}
                }

                //For Supplier Cheque
                foreach (GridViewRow gvC in GvSupplierChequeDetail.Rows)
                {
                    HiddenField hdnVoucherDetailId = (HiddenField)gvC.FindControl("hdnSVD_Id");
                    Label lblChequeAmt = (Label)gvC.FindControl("lblgvChequeAmount");
                    CheckBox chkSup = (CheckBox)gvC.FindControl("chkgvSSelect");

                    //if (chkSup.Checked == true)
                    //{
                    if (hdnVoucherDetailId.Value != "")
                    {
                        ObjCashDetail.InsertCashFlowDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId, objsys.getDateForInput(txtDate.Text).ToString(), txtOpeningBalance.Text, txtClosingBalance.Text, "SupCheque", hdnVoucherDetailId.Value, "0", lblChequeAmt.Text, "0.00", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        //Update Reconcile Voucher Detail
                        //objVoucherDetail.UpdateVoucherDetailReconcile(hdnVoucherDetailId.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //}
                }
            }

            if (b != 0)
            {
                if (sender is Button)
                {
                    Button btnId = (Button)sender;

                    if (btnId.ID == "btnPost")
                    {
                        DisplayMessage("Cash Flow Posted Successfully");
                    }
                }
            }
            else
            {
                if (sender is Button)
                {
                    Button btnId = (Button)sender;

                    if (btnId.ID == "btnPost")
                    {
                        DisplayMessage("Cash Flow Not Posted");
                    }
                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            trns.Dispose();
            con.Dispose();
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

    protected void chkAllTrue_CheckedChanged(object sender, EventArgs e)
    {
        string strCurrency = hdnCurrencyId.Value;
        CheckBox chkAllTotal = (CheckBox)GvSummarized.FooterRow.FindControl("chkAllTrue");
        if (chkAllTotal.Checked == true)
        {
            double TotalPhysical = 0;
            foreach (GridViewRow gvr in GvSummarized.Rows)
            {
                Label lblSysBal = (Label)gvr.FindControl("lblgvSystemBalance");
                TextBox txtPhysicalBalance = (TextBox)gvr.FindControl("txtgvPhysicalBalance");

                if (lblSysBal.Text != "")
                {
                    txtPhysicalBalance.Text = lblSysBal.Text;
                }
                else
                {
                    txtPhysicalBalance.Text = "0.00";
                }
                if (txtPhysicalBalance.Text != "")
                {
                    TotalPhysical += Convert.ToDouble(txtPhysicalBalance.Text);
                }
            }

            if (GvSummarized.Rows.Count > 0)
            {
                Label lblgvPhysicalBalanceTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvPhysicalBalanceTotal");

                lblgvPhysicalBalanceTotal.Text = TotalPhysical.ToString();
                lblgvPhysicalBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvPhysicalBalanceTotal.Text);
            }
        }
        else if (chkAllTotal.Checked == false)
        {
            double TotalPhysical = 0;
            foreach (GridViewRow gvr in GvSummarized.Rows)
            {
                TextBox txtPhysicalBalance = (TextBox)gvr.FindControl("txtgvPhysicalBalance");

                txtPhysicalBalance.Text = "0";
                txtPhysicalBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, txtPhysicalBalance.Text);
                TotalPhysical += Convert.ToDouble(txtPhysicalBalance.Text);
            }
            if (GvSummarized.Rows.Count > 0)
            {
                Label lblgvPhysicalBalanceTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvPhysicalBalanceTotal");

                lblgvPhysicalBalanceTotal.Text = TotalPhysical.ToString();
                lblgvPhysicalBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvPhysicalBalanceTotal.Text);
            }
        }
    }
    protected void chkgvCSelect_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvC in GvCustomerChequeDetail.Rows)
        {
            HiddenField hdnVoucherDetailId = (HiddenField)gvC.FindControl("hdnCVD_Id");
            Label lblChequeAmt = (Label)gvC.FindControl("lblgvChequeAmount");
            CheckBox chkCus = (CheckBox)gvC.FindControl("chkgvCSelect");

            if (chkCus.Checked == true)
            {
                if (hdnVoucherDetailId.Value != "")
                {
                    //Update Reconcile Voucher Detail
                    objVoucherDetail.UpdateVoucherDetailReconcile(hdnVoucherDetailId.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
        btnGetReport_Click(sender, e);
    }
    protected void chkgvSSelect_CheckedChanged(object sender, EventArgs e)
    {
        //For Supplier Cheque
        foreach (GridViewRow gvC in GvSupplierChequeDetail.Rows)
        {
            HiddenField hdnVoucherDetailId = (HiddenField)gvC.FindControl("hdnSVD_Id");
            Label lblChequeAmt = (Label)gvC.FindControl("lblgvChequeAmount");
            CheckBox chkSup = (CheckBox)gvC.FindControl("chkgvSSelect");

            if (chkSup.Checked == true)
            {
                if (hdnVoucherDetailId.Value != "")
                {
                    //Update Reconcile Voucher Detail
                    objVoucherDetail.UpdateVoucherDetailReconcile(hdnVoucherDetailId.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
        btnGetReport_Click(sender, e);
    }
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        //Parameters 
        string WeekOff = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        bool WeekOffValue = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowWeekOff"));
        bool HolidayValue = Convert.ToBoolean(objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, "CashFlowHoliday"));


        if (txtDate.Text == "")
        {
            txtOpeningBalance.Text = "";
            txtClosingBalance.Text = "";

            GVCashFlow.DataSource = null;
            GVCashFlow.DataBind();

            GvSummarized.DataSource = null;
            GvSummarized.DataBind();

            lblAccountsSummarized.Visible = false;
        }
        else
        {
            string strMaxId = string.Empty;
            DataTable dtMaxId = ObjCashHeader.GetCashFlowMaxIdPosted(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value);
            if (dtMaxId.Rows.Count > 0)
            {
                strMaxId = dtMaxId.Rows[0][0].ToString();

                if (strMaxId != "" && strMaxId != "0")
                {
                    DataTable dtRecord = ObjCashHeader.GetCashFlowByCashFlowId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strMaxId);
                    if (dtRecord.Rows.Count > 0)
                    {
                        DateTime dtLastDate = DateTime.Parse(dtRecord.Rows[0]["CF_Date"].ToString());
                        string strcheckDate = dtLastDate.AddDays(1).ToString("dd-MMM-yyyy");

                        if (strcheckDate != "")
                        {
                            if (WeekOffValue)
                            {
                                bool IsWeekOff = false;
                                foreach (string str in WeekOff.Split(','))
                                {
                                    if (str == DateTime.Parse(strcheckDate).DayOfWeek.ToString())
                                    {
                                        IsWeekOff = true;
                                        if (IsWeekOff == true)
                                        {
                                            strcheckDate = DateTime.Parse(strcheckDate).AddDays(1).ToString("dd-MMM-yyyy");
                                        }
                                    }
                                }
                            }

                            if (HolidayValue)
                            {
                                DataTable dtHoliday = objHoliday.GetHolidayMasterByCompanyOnly(Session["CompId"].ToString());
                                if (dtHoliday.Rows.Count > 0)
                                {
                                    dtHoliday = new DataView(dtHoliday, "Field1='" + hdnLocId.Value + "' and From_Date='" + strcheckDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtHoliday.Rows.Count > 0)
                                    {
                                        DateTime dtFromDate = DateTime.Parse(dtHoliday.Rows[0]["From_Date"].ToString());
                                        DateTime dtToDate = DateTime.Parse(dtHoliday.Rows[0]["To_Date"].ToString());

                                        if (dtToDate >= dtFromDate)
                                        {
                                            string strDiffrenceDays = (dtToDate - dtFromDate).TotalDays.ToString();
                                            string strFinalDays = (Convert.ToDouble(strDiffrenceDays) + 1).ToString();

                                            if (strFinalDays != "")
                                            {
                                                strcheckDate = DateTime.Parse(strcheckDate).AddDays(int.Parse(strFinalDays)).ToString("dd-MMM-yyyy");
                                            }
                                        }
                                    }
                                }
                            }
                        }




                        if (DateTime.Parse(txtDate.Text) == DateTime.Parse(strcheckDate))
                        {

                        }
                        else
                        {
                            DisplayMessage("You cant Generate Cash flow for that date");
                            txtDate.Focus();
                            txtDate.Text = "";
                            return;
                        }
                    }
                }
            }
        }
    }
    protected void txtgvPhysicalBalance_TextChanged(object sender, EventArgs e)
    {
        double TotalPhysical = 0;
        foreach (GridViewRow gvr in GvSummarized.Rows)
        {
            TextBox txtPhysicalBalance = (TextBox)gvr.FindControl("txtgvPhysicalBalance");

            if (txtPhysicalBalance.Text != "")
            {
                TotalPhysical += Convert.ToDouble(txtPhysicalBalance.Text);
            }
        }

        string strCurrency = hdnCurrencyId.Value;
        if (GvSummarized.Rows.Count > 0)
        {
            Label lblgvPhysicalBalanceTotal = (Label)GvSummarized.FooterRow.FindControl("lblgvPhysicalBalanceTotal");

            lblgvPhysicalBalanceTotal.Text = TotalPhysical.ToString();
            lblgvPhysicalBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvPhysicalBalanceTotal.Text);
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
    protected string GetDateWithTime(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected string GetAccountName(string strAccountId)
    {
        string strAccountName = string.Empty;
        if (strAccountId != "")
        {
            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAccount.Rows.Count > 0)
            {
                strAccountName = dtAccount.Rows[0]["AccountName"].ToString();
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }
    protected string GetAccountNo(string strAccountId)
    {
        string strAccountName = string.Empty;
        if (strAccountId != "")
        {
            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAccount.Rows.Count > 0)
            {
                strAccountName = dtAccount.Rows[0]["Account_No"].ToString();
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }


    #region List Section
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        getMaxDate();
        string strCurrency = hdnCurrencyId.Value;
        string PostStatus = PostStatus = " ReconcileStatus='True'";
        DataTable dtCashDetail = ObjCashHeader.GetCashFlowAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value);
        dtCashDetail = new DataView(dtCashDetail, PostStatus, "CF_Date desc", DataViewRowState.CurrentRows).ToTable();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtCashDetail.Rows.Count + "";

        if (dtCashDetail != null && dtCashDetail.Rows.Count > 0)
        {
            Session["dtVoucher"] = dtCashDetail;
            Session["dtFilter_Daily_CF"] = dtCashDetail;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvCashDetail, dtCashDetail, "", "");
        }
        else
        {
            GvCashDetail.DataSource = null;
            GvCashDetail.DataBind();
        }

        foreach (GridViewRow gvr in GvCashDetail.Rows)
        {
            Label lblgvCFOpeningAmt = (Label)gvr.FindControl("lblgvCFOpeningAmount");
            Label lblgvCFClosingAmt = (Label)gvr.FindControl("lblgvCFClosingAmount");

            lblgvCFOpeningAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCFOpeningAmt.Text);
            lblgvCFClosingAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCFClosingAmt.Text);
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtCashDetail.Rows.Count.ToString() + "";
        //AllPageCode();     
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
                DataTable dtEmp = objUser.GetEmpDtlsFromUserID(strEmployeeCode, Session["CompId"].ToString()); ;
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
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = hdnCurrencyId.Value;

        string strCFId = e.CommandArgument.ToString();
        btnNew_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.View;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        if (strCFId != "" && strCFId != "0")
        {
            DataTable dtHeader = ObjCashHeader.GetCashFlowByCashFlowId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strCFId);
            if (dtHeader.Rows.Count > 0)
            {
                btnPost.Visible = false;
                btnGetReport.Visible = false;
                btnShowReport.Visible = false;

                txtDate.Text = Convert.ToDateTime(dtHeader.Rows[0]["CF_Date"].ToString()).ToString(objsys.SetDateFormat());
                txtDate.Enabled = false;
                txtOpeningBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, dtHeader.Rows[0]["CF_OpeningAmount"].ToString());
                txtClosingBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, dtHeader.Rows[0]["CF_ClosingAmount"].ToString());
                txtSummarizedTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, dtHeader.Rows[0]["Field1"].ToString());

                DataTable dtDetailView = ObjCashDetail.GetCashFlowViewDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strCFId);
                if (dtDetailView.Rows.Count > 0)
                {
                    GVCashFlow.DataSource = dtDetailView;
                    GVCashFlow.DataBind();
                }
                else
                {
                    //lblAccountsSummarized.Visible = false;
                    GVCashFlow.DataSource = null;
                    GVCashFlow.DataBind();
                }

                //Grid Calculation              
                double debitamount = 0;
                double creditamount = 0;
                double debitTotalamount = 0;
                double creditTotalamount = 0;
                string strStatus = "False";
                string strBalanceA = string.Empty;
                foreach (GridViewRow gvr in GVCashFlow.Rows)
                {
                    Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                    lblgvDebitAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitAmount.Text);
                    lblgvCreditAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditAmount.Text);
                    debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                    creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                    Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");

                    if (strStatus == "False")
                    {
                        if (txtOpeningBalance.Text != "")
                        {
                            lblgvBalance.Text = txtOpeningBalance.Text;
                            strStatus = "True";
                        }
                        else
                        {
                            lblgvBalance.Text = "0";
                            strStatus = "True";
                        }
                    }
                    else
                    {
                        lblgvBalance.Text = strBalanceA;
                    }

                    if (debitamount != 0)
                    {
                        lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(debitamount.ToString())).ToString();
                        strBalanceA = lblgvBalance.Text;
                    }
                    else if (creditamount != 0)
                    {
                        lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(creditamount.ToString())).ToString();
                        strBalanceA = lblgvBalance.Text;
                    }

                    debitTotalamount += debitamount;
                    creditTotalamount += creditamount;
                    txtClosingBalance.Text = strBalanceA;
                    txtClosingBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, txtClosingBalance.Text);
                }

                if (GVCashFlow.Rows.Count > 0)
                {
                    Label lblgvDebitTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvDebitTotal");
                    Label lblgvCreditTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvCreditTotal");
                    Label lblgvBalanceTotal = (Label)GVCashFlow.FooterRow.FindControl("lblgvBalanceTotal");

                    lblgvDebitTotal.Text = debitTotalamount.ToString();
                    lblgvCreditTotal.Text = creditTotalamount.ToString();
                    lblgvBalanceTotal.Text = strBalanceA;

                    lblgvDebitTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitTotal.Text);
                    lblgvCreditTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditTotal.Text);
                    lblgvBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvBalanceTotal.Text);
                }
                else
                {
                    DisplayMessage("You have no record available");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
                }
            }
            else
            {
                //lblAccountsSummarized.Visible = false;
                GVCashFlow.DataSource = null;
                GVCashFlow.DataBind();
            }


            DataTable dtAccount = ObjCashDetail.GetCashFlowAccountDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strCFId);
            if (dtAccount.Rows.Count > 0)
            {
                lblAccountsSummarized.Visible = true;
                GvSummarizedView.DataSource = dtAccount;
                GvSummarizedView.DataBind();

                double TotalSystemBalance = 0;
                double TotalPhysicalBal = 0;
                foreach (GridViewRow gv in GvSummarizedView.Rows)
                {
                    HiddenField hdnAccountId = (HiddenField)gv.FindControl("hdnAccountId");
                    Label lblSystemBalance = (Label)gv.FindControl("lblgvSystemBalance");
                    Label lblPhysicalBalance = (Label)gv.FindControl("lblgvPhysicalBalance");

                    if (lblSystemBalance.Text != "")
                    {
                        lblSystemBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblSystemBalance.Text);
                    }

                    if (lblPhysicalBalance.Text != "")
                    {
                        lblPhysicalBalance.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblPhysicalBalance.Text);
                    }

                    double SysBal = Convert.ToDouble(lblSystemBalance.Text);
                    if (SysBal == 0)
                    {
                        lblPhysicalBalance.Text = "0.00";
                    }
                    TotalSystemBalance += Convert.ToDouble(lblSystemBalance.Text);
                    TotalPhysicalBal += Convert.ToDouble(lblPhysicalBalance.Text);
                }

                if (GvSummarizedView.Rows.Count > 0)
                {
                    Label lblgvSystemBalanceTotal = (Label)GvSummarizedView.FooterRow.FindControl("lblgvSystemBalanceTotal");
                    Label lblgvPhysicalBalanceTotal = (Label)GvSummarizedView.FooterRow.FindControl("lblgvPhysicalBalanceTotal");

                    lblgvSystemBalanceTotal.Text = TotalSystemBalance.ToString();
                    lblgvSystemBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvSystemBalanceTotal.Text);
                    lblgvPhysicalBalanceTotal.Text = TotalPhysicalBal.ToString();
                    lblgvPhysicalBalanceTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvPhysicalBalanceTotal.Text);
                }
                else
                {
                    DisplayMessage("You have no record available");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
                }
            }
            else
            {
                lblAccountsSummarized.Visible = false;
                GvSummarizedView.DataSource = null;
                GvSummarizedView.DataBind();
            }

            //for Customer Cheques
            DataTable dtCustomerCheque = ObjCashDetail.GetCashFlowCusChequeDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strCFId);
            if (dtCustomerCheque.Rows.Count > 0)
            {
                if (dtCustomerCheque.Rows.Count > 0)
                {
                    lblPostdatedCheque.Visible = true;
                    lblCustomersCheque.Visible = true;

                    GvCustomerChequeDetail.DataSource = dtCustomerCheque;
                    GvCustomerChequeDetail.DataBind();

                    double TotalChequeAmt = 0;
                    foreach (GridViewRow gvC in GvCustomerChequeDetail.Rows)
                    {
                        CheckBox chkCustomerCheque = (CheckBox)gvC.FindControl("chkgvCSelect");
                        Label lblgvAmount = (Label)gvC.FindControl("lblgvChequeAmount");

                        chkCustomerCheque.Checked = false;
                        chkCustomerCheque.Enabled = false;
                        if (lblgvAmount.Text != "")
                        {
                            lblgvAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvAmount.Text);
                            TotalChequeAmt += Convert.ToDouble(lblgvAmount.Text);
                        }
                    }

                    if (GvCustomerChequeDetail.Rows.Count > 0)
                    {
                        Label lblgvChequeAmtTotal = (Label)GvCustomerChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");

                        lblgvChequeAmtTotal.Text = TotalChequeAmt.ToString();
                        lblgvChequeAmtTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvChequeAmtTotal.Text);
                    }
                }
            }

            //For Supplier Cheque
            DataTable dtSupplierCheque = ObjCashDetail.GetCashFlowSupChequeDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strCFId);
            if (dtSupplierCheque.Rows.Count > 0)
            {
                if (dtSupplierCheque.Rows.Count > 0)
                {
                    lblPostdatedCheque.Visible = true;
                    lblSupplierCheque.Visible = true;
                    GvSupplierChequeDetail.DataSource = dtSupplierCheque;
                    GvSupplierChequeDetail.DataBind();

                    double TotalChequeAmt = 0;
                    foreach (GridViewRow gvS in GvSupplierChequeDetail.Rows)
                    {
                        Label lblgvAmount = (Label)gvS.FindControl("lblgvChequeAmount");
                        CheckBox chkSupplierCheque = (CheckBox)gvS.FindControl("chkgvSSelect");

                        chkSupplierCheque.Checked = false;
                        chkSupplierCheque.Enabled = false;
                        if (lblgvAmount.Text != "")
                        {
                            lblgvAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvAmount.Text);
                            TotalChequeAmt += Convert.ToDouble(lblgvAmount.Text);
                        }
                    }

                    if (GvSupplierChequeDetail.Rows.Count > 0)
                    {
                        Label lblgvChequeAmtTotal = (Label)GvSupplierChequeDetail.FooterRow.FindControl("lblgvChequeAmountTotal");

                        lblgvChequeAmtTotal.Text = TotalChequeAmt.ToString();
                        lblgvChequeAmtTotal.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvChequeAmtTotal.Text);
                    }
                }
            }
        }
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Accounts_Report/DailyCashFlowPrint.aspx?Id=" + e.CommandArgument.ToString() + "&LocId=" + hdnLocId.Value + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        ///HERE WE WILL DELETE LAST CASH FLOW 

        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        if (Convert.ToDateTime(((Label)gvRow.FindControl("lblgvCFDate")).Text) == Ac_CashFlow_Header.getLastcashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, Session["DBConnection"].ToString()))
        {
            ObjDa.execute_Command("update Ac_CashFlow_Header set IsActive='False',ModifiedBy='" + Session["userId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where CF_Id=" + e.CommandArgument.ToString() + "");
            DisplayMessage("Record deleted Successfully");
            Reset();
            FillGrid();


        }
        else
        {
            DisplayMessage("you can delete only last created cash flow");
            return;
        }
    }
    protected void GvCashDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Daily_CF"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Daily_CF"] = dt;
        //bind gridview by function in common class

        objPageCmn.FillData((object)GvCashDetailBin, dt, "", "");

        //AllPageCode();
        GvCashDetailBin.HeaderRow.Focus();
    }
    protected void GvCashDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCashDetail.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Daily_CF"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCashDetail, dt, "", "");
        //AllPageCode();
        foreach (GridViewRow gvr in GvCashDetail.Rows)
        {
            Label lblgvCFOpeningAmt = (Label)gvr.FindControl("lblgvCFOpeningAmount");
            Label lblgvCFClosingAmt = (Label)gvr.FindControl("lblgvCFClosingAmount");

            lblgvCFOpeningAmt.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, lblgvCFOpeningAmt.Text);
            lblgvCFClosingAmt.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, lblgvCFClosingAmt.Text);
        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        hdnCashFlowId.Value = e.CommandArgument.ToString();
        btnNew_Click(null, null);

        DataTable dtVoucherEdit = ObjCashHeader.GetCashFlowByCashFlowId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            txtDate_TextChanged(null, null);
            btnGetReport_Click(null, null);
        }
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldName.SelectedValue.ToString() == "CF_Date")
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(Txt_Date_List.Text.Trim()) + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(Txt_Date_List.Text.Trim()) + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(Txt_Date_List.Text.Trim()) + "%'";
                }
            }
            else
            {
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
            }
            DataTable dtPurchaseRequest = (DataTable)Session["dtVoucher"];
            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Daily_CF"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //bind gridview by function in common class
            objPageCmn.FillData((object)GvCashDetail, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        //ddlFieldName.SelectedIndex = 0;
        //ddlOption.SelectedIndex = 2;
        // txtValue.Visible = true;
        txtValue.Focus();
        FillGrid();
    }
    #endregion





    #region Binsection


    private void FillGridBin()
    {
        string strCurrency = hdnCurrencyId.Value;
        string PostStatus = PostStatus = " ReconcileStatus='True'";
        DataTable dtCashDetail = ObjCashHeader.GetCashFlowAllFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value);
        dtCashDetail = new DataView(dtCashDetail, PostStatus, "CF_Date desc", DataViewRowState.CurrentRows).ToTable();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtCashDetail.Rows.Count + "";

        if (dtCashDetail != null && dtCashDetail.Rows.Count > 0)
        {
            Session["dtVoucherBin"] = dtCashDetail;
            Session["dtFilterBin"] = dtCashDetail;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvCashDetailBin, dtCashDetail, "", "");
        }
        else
        {
            GvCashDetailBin.DataSource = null;
            GvCashDetailBin.DataBind();
        }

        foreach (GridViewRow gvr in GvCashDetailBin.Rows)
        {
            Label lblgvCFOpeningAmt = (Label)gvr.FindControl("lblgvCFOpeningAmount");
            Label lblgvCFClosingAmt = (Label)gvr.FindControl("lblgvCFClosingAmount");

            lblgvCFOpeningAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCFOpeningAmt.Text);
            lblgvCFClosingAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCFClosingAmt.Text);
        }

        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dtCashDetail.Rows.Count.ToString() + "";
        //AllPageCode();

        lblSelectedRecord.Text = "";
    }



    protected void btnbindrptBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlOptionBin.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldNameBin.SelectedValue.ToString() == "CF_Date")
            {
                if (ddlOptionBin.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + Convert.ToDateTime(Txt_Date_Bin.Text.Trim()) + "'";
                }
                else if (ddlOptionBin.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(Txt_Date_Bin.Text.Trim()) + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(Txt_Date_Bin.Text.Trim()) + "%'";
                }
            }
            else
            {
                if (ddlOptionBin.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
                }
                else if (ddlOptionBin.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtbinValue.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtbinValue.Text.Trim() + "%'";
                }
            }
            DataTable dtPurchaseRequest = (DataTable)Session["dtVoucherBin"];
            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilterBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //bind gridview by function in common class
            objPageCmn.FillData((object)GvCashDetailBin, view.ToTable(), "", "");
            //AllPageCode();            
            lblSelectedRecord.Text = "";
        }
        txtbinValue.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        txtbinValue.Text = "";
        txtbinValue.Focus();
        FillGridBin();
    }


    protected void GvCashDetailBin_Sorting(object sender, GridViewSortEventArgs e)
    {


        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilterBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterBin"] = dt;
        //bind gridview by function in common class

        objPageCmn.FillData((object)GvCashDetailBin, dt, "", "");

        //AllPageCode();
        GvCashDetailBin.HeaderRow.Focus();

    }
    protected void GvCashDetailBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCashDetailBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterBin"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCashDetailBin, dt, "", "");
        //AllPageCode();
    }

    protected void imgBtnRestore_Click(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;


        if (ObjDa.return_DataTable("SELECT cf_date FROM Ac_CashFlow_Header WHERE Company_Id=" + Session["CompId"].ToString() + " AND BRAND_ID=" + Session["BrandId"].ToString() + " AND Location_Id=" + hdnLocId.Value + " AND IsActive='True' and ReconcileStatus='True' and CF_Date='" + Convert.ToDateTime(((Label)gvRow.FindControl("lblgvCFDate")).Text) + "'").Rows.Count > 0)
        {
            DisplayMessage("cash flow created  , you can not restore");
            return;
        }
        if (Convert.ToDateTime(((Label)gvRow.FindControl("lblgvCFDate")).Text) != Ac_CashFlow_Header.getLastcashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, Session["DBConnection"].ToString()).AddDays(1))
        {
            DisplayMessage("you can not restore , date sequence will be missmtach");
            return;

        }
        ///HERE WE WILL DELETE LAST CASH FLOW 


        ObjDa.execute_Command("update Ac_CashFlow_Header set IsActive='True',ModifiedBy='" + Session["userId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where CF_Id=" + e.CommandArgument.ToString() + "");
        DisplayMessage("Record restored Successfully");
        FillGridBin();
    }


    #endregion

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue.ToString() == "CF_Date")
        {
            txtValue.Text = "";
            Txt_Date_List.Text = "";
            txtValue.Visible = false;
            Txt_Date_List.Visible = true;
        }
        else
        {
            txtValue.Text = "";
            Txt_Date_List.Text = "";
            Txt_Date_List.Visible = false;
            txtValue.Visible = true;
        }
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedValue.ToString() == "CF_Date")
        {
            txtbinValue.Text = "";
            Txt_Date_Bin.Text = "";
            txtbinValue.Visible = false;
            Txt_Date_Bin.Visible = true;
        }
        else
        {
            txtbinValue.Text = "";
            Txt_Date_Bin.Text = "";
            Txt_Date_Bin.Visible = false;
            txtbinValue.Visible = true;
        }
    }



    public bool CheckPermission(string date)
    {
        if (MaxDate == "")
        {
            getMaxDate();
        }

        if (havePermission)
        {
            if (objsys.getDateForInput(GetDate(date)) == objsys.getDateForInput(GetDate(MaxDate)))
            {
                return true;
            }
        }

        return false;
    }

    public void getMaxDate()
    {
        string strMaxDate = "";
        try
        {
            DateTime dt = Ac_CashFlow_Header.getLastcashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, Session["DBConnection"].ToString());
            strMaxDate = GetDate(dt.ToString());
            MaxDate = strMaxDate;
        }
        catch (Exception ex)
        {

        }

        if (strMaxDate == string.Empty)
        {
            try
            {
                using (DataTable dtCashflowDate = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate"))
                {
                    if (dtCashflowDate.Rows.Count > 0)
                    {
                        strMaxDate = dtCashflowDate.Rows[0]["Field7"].ToString();
                        MaxDate = strMaxDate;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

    }

    protected void btnUpdateAmt_Click(object sender, EventArgs e)
    {
        getMaxDate();
        string amt = txtUPClosingAmount.Text;

        if (hdnCFID.Value == "")
        {
            DisplayMessage("Cant Update");
            return;
        }
        if (objsys.getDateForInput(hdnCFDate.Value) == objsys.getDateForInput(MaxDate))
        {
            int parsedValue;
            decimal parseddecimal;
            if (!int.TryParse(txtUPClosingAmount.Text, out parsedValue))
            {
                if (!decimal.TryParse(txtUPClosingAmount.Text, out parseddecimal))
                {
                    DisplayMessage("Amount entered was not in correct format");
                    txtUPClosingAmount.Text = amt;
                    txtUPClosingAmount.Focus();
                    return;
                }
            }


            ObjCashHeader.UpdateClosingAmt(txtUPClosingAmount.Text, Session["UserId"].ToString(), hdnCFID.Value);
            FillGrid();
            DisplayMessage("Update Successfully");
            Reset();
            hdnCFID.Value = "";
            hdnCFDate.Value = "";
            txtUPClosingAmount.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateAmtModal_Popup()", true);
        }
        else
        {
            txtUPClosingAmount.Text = amt;
            DisplayMessage("Cant Edit");
            return;
        }
    }

    protected void btnEditClosingAmt_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = hdnCurrencyId.Value;
        txtUPClosingAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, e.CommandArgument.ToString().Split('/')[0].ToString());
        hdnCFDate.Value = e.CommandName;
        hdnCFID.Value = e.CommandArgument.ToString().Split('/')[1].ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateAmtModal_Popup()", true);
    }

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        hdnLocId.Value = ddlLocationList.SelectedValue;
        hdnCurrencyId.Value = objLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        FillGrid();
        Reset();
    }
    public void FillLocationList()
    {
        string sql = "select location_id, location_name from Set_LocationMaster where isActive='true' and location_id in (select distinct Location_Id from dbo.Ac_Parameter_Location where Param_Name='CashFlowLocation' and Company_Id='" + Session["CompId"].ToString() + "' and IsActive='true')";
        //DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        DataTable dtLoc = ObjDa.return_DataTable(sql);
        //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtLoc == null || dtLoc.Rows.Count == 0)
        {
            return;
        }
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