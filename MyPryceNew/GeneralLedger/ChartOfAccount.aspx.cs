using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;

public partial class GeneralLedger_ChartOfAccount : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_SubChartOfAccount objSubCOA = null;
    Ac_Nature_Accounts objNOA = null;
    Ac_ParameterMaster objAcParameter = null;
    SystemParameter ObjSysParam = null;
    CurrencyMaster objCurrency = null;
    Set_DocNumber objDocNo = null;
    Ac_Groups objAccGroup = null;
    UserMaster ObjUser = null;
    LocationMaster ObjLocation = null;
    CompanyMaster ObjCompany = null;
    Ems_ContactMaster objContact = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = "admin";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        //AllPageCode();
        cmn = new Common(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        objNOA = new Ac_Nature_Accounts(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objAccGroup = new Ac_Groups(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/ChartOfAccount.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            FillCurrency();
            ddlCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGridBin();
            FillGrid();
            txtAccountNo.Text = objDocNo.GetDocumentNo(true, "0", false, "150", "175", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtAccountNo.Text;
            BindTreeView_AcGroup();
            FillLocation();
            FillLocationView();
            //Get opening balance update allow or not
            ViewState["idObUpdateAllow"] = true;
            DataTable _dtFyear = objFYI.GetInfoAllTrue(Session["CompId"].ToString());
            if (_dtFyear.Rows.Count > 0)
            {
                _dtFyear = new DataView(_dtFyear, "Status='Close'", "", DataViewRowState.CurrentRows).ToTable();
                if (_dtFyear.Rows.Count > 0)
                {
                    ViewState["idObUpdateAllow"] = false;
                }
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCOASave.Visible = true;
        imgBtnRestore.Visible = true;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    #region System defined Function
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
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        DataTable dtSCOAEdit = objCOA.GetCOAByTransId(StrCompId, e.CommandArgument.ToString());
        if (dtSCOAEdit.Rows.Count > 0)
        {
            hdnCOAId.Value = e.CommandArgument.ToString();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtAccountNo.Text = dtSCOAEdit.Rows[0]["Account_No"].ToString();
            txtAccountName.Text = dtSCOAEdit.Rows[0]["AccountName"].ToString();
            txtAccountNameL.Text = dtSCOAEdit.Rows[0]["AccountNameL"].ToString();
            //txtOpeningBalance.Text = dtSCOAEdit.Rows[0]["Opening_Balance"].ToString();
            txtGstNo.Text = dtSCOAEdit.Rows[0]["field2"].ToString();
            //Get Contact Info
            int contactId = 0;
            int.TryParse(dtSCOAEdit.Rows[0]["field2"].ToString(), out contactId);
            if (contactId != 0)
            {
                txtContactList.Text = objContact.GetContactNameByContactiD(contactId.ToString()) + "/" + contactId.ToString();
            }

            string strAccGroupId = dtSCOAEdit.Rows[0]["Acc_Group_Id"].ToString();
            txtAccountGroup.Text = GetAccountGroupName(strAccGroupId) + "/" + strAccGroupId;

            txtOpeningCreditBalance.Text = dtSCOAEdit.Rows[0]["O_Cr_Bal"].ToString();
            txtOpeningCreditBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtOpeningCreditBalance.Text);
            txtOpeningDebitBalance.Text = dtSCOAEdit.Rows[0]["O_Dr_Bal"].ToString();
            txtOpeningDebitBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtOpeningDebitBalance.Text);
            string strCurrencyId = dtSCOAEdit.Rows[0]["Currency_Id"].ToString();

            if (strCurrencyId != "" && strCurrencyId != "0")
            {
                ddlCurrency.SelectedValue = strCurrencyId;
            }
            else
            {
                FillCurrency();
            }

            txtAccountType.Text = dtSCOAEdit.Rows[0]["Type_Account"].ToString();
            txtDebitAmount.Text = dtSCOAEdit.Rows[0]["DB_Amount"].ToString();
            txtDebitAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtDebitAmount.Text);
            txtCreditAmount.Text = dtSCOAEdit.Rows[0]["CR_Amount"].ToString();
            txtCreditAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtCreditAmount.Text);
            txtLastDebit.Text = dtSCOAEdit.Rows[0]["Last_Debit"].ToString();
            txtLastDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtLastDebit.Text);
            txtLastCredit.Text = dtSCOAEdit.Rows[0]["Last_Credit"].ToString();
            txtLastCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtLastCredit.Text);

            trLock.Visible = true;

            bool strLockAcc = Convert.ToBoolean(dtSCOAEdit.Rows[0]["Field1"].ToString());
            if (strLockAcc)
            {
                chkLockAccount.Checked = true;
            }
            else
            {
                chkLockAccount.Checked = false;
            }

            //for Customer & Supplier Account
            string strReceiveVoucherAcc = string.Empty;
            DataTable dtParam = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
            if (dtParam.Rows.Count > 0)
            {
                strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strReceiveVoucherAcc = "0";
            }

            string strPaymentVoucherAcc = string.Empty;
            DataTable dtPaymentVoucher = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
            if (dtPaymentVoucher.Rows.Count > 0)
            {
                strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strPaymentVoucherAcc = "0";
            }

            //For Location Opening Balance 
            foreach (GridViewRow gvr in GvLocation.Rows)
            {
                HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
                TextBox txtgvDebit = (TextBox)gvr.FindControl("txtgvDebit");
                TextBox txtgvCredit = (TextBox)gvr.FindControl("txtgvCredit");
                TextBox txtgvForeignDebit = (TextBox)gvr.FindControl("txtgvForeignDebit");
                TextBox txtgvForeignCredit = (TextBox)gvr.FindControl("txtgvForeignCredit");

                if (strReceiveVoucherAcc == hdnCOAId.Value || strPaymentVoucherAcc == hdnCOAId.Value)
                {
                    DataTable dtSubCOA = objSubCOA.GetSubCOAAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, Session["FinanceYearId"].ToString());
                    if (dtSubCOA.Rows.Count > 0)
                    {
                        dtSubCOA = new DataView(dtSubCOA, "AccTransId='" + hdnCOAId.Value + "' and Other_Account_No<>'0'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtSubCOA.Rows.Count > 0)
                        {
                            double DebitAmount = 0;
                            double CreditAmount = 0;
                            double DebitAmountFr = 0;
                            double CreditAmountFr = 0;
                            double DebitLocalamountTotal = 0;
                            double CreditLocalamountTotal = 0;
                            double DebitForeignamountTotal = 0;
                            double CreditForeignamountTotal = 0;

                            for (int i = 0; i < dtSubCOA.Rows.Count; i++)
                            {
                                if (dtSubCOA.Rows[i]["LDr_Amount"].ToString() != "")
                                {
                                    DebitAmount = Convert.ToDouble(dtSubCOA.Rows[i]["LDr_Amount"].ToString());
                                }
                                if (dtSubCOA.Rows[i]["LCr_Amount"].ToString() != "")
                                {
                                    CreditAmount = Convert.ToDouble(dtSubCOA.Rows[i]["LCr_Amount"].ToString());
                                }
                                if (dtSubCOA.Rows[i]["FDr_Amount"].ToString() != "")
                                {
                                    DebitAmountFr = Convert.ToDouble(dtSubCOA.Rows[i]["FDr_Amount"].ToString());
                                }
                                if (dtSubCOA.Rows[i]["FCr_Amount"].ToString() != "")
                                {
                                    CreditAmountFr = Convert.ToDouble(dtSubCOA.Rows[i]["FCr_Amount"].ToString());
                                }

                                DebitLocalamountTotal += DebitAmount;
                                CreditLocalamountTotal += CreditAmount;
                                DebitForeignamountTotal += DebitAmountFr;
                                CreditForeignamountTotal += CreditAmountFr;
                            }

                            txtgvDebit.Text = DebitLocalamountTotal.ToString();
                            txtgvDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvDebit.Text);
                            txtgvCredit.Text = CreditLocalamountTotal.ToString();
                            txtgvCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvCredit.Text);
                            txtgvForeignDebit.Text = DebitForeignamountTotal.ToString();
                            txtgvForeignDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignDebit.Text);
                            txtgvForeignCredit.Text = CreditForeignamountTotal.ToString();
                            txtgvForeignCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignCredit.Text);
                        }
                    }
                }
                else
                {
                    DataTable dtSubCOA = objSubCOA.GetSubCOAAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, Session["FinanceYearId"].ToString());
                    if (dtSubCOA.Rows.Count > 0)
                    {
                        dtSubCOA = new DataView(dtSubCOA, "AccTransId='" + hdnCOAId.Value + "' and Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtSubCOA.Rows.Count > 0)
                        {
                            txtgvDebit.Text = dtSubCOA.Rows[0]["LDr_Amount"].ToString();
                            txtgvDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvDebit.Text);
                            txtgvCredit.Text = dtSubCOA.Rows[0]["LCr_Amount"].ToString();
                            txtgvCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvCredit.Text);
                            txtgvForeignDebit.Text = dtSubCOA.Rows[0]["FDr_Amount"].ToString();
                            txtgvForeignDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignDebit.Text);
                            txtgvForeignCredit.Text = dtSubCOA.Rows[0]["FCr_Amount"].ToString();
                            txtgvForeignCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignCredit.Text);
                        }
                    }
                }
            }
            //End
        }
        btnNew_Click(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void GvCOA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        GvCOA.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Chart_Acc"];
        //Common Function add By Lokesh on 23-05-2015
       objPageCmn.FillData((object)GvCOA, dt, "", "");
        foreach (GridViewRow gvr in GvCOA.Rows)
        {
            Label lblgvOpeningCredit = (Label)gvr.FindControl("lblgvOpeningCrBalance");
            Label lblgvOpeningDebit = (Label)gvr.FindControl("lblgvOpeningDrBalance");

            lblgvOpeningCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningCredit.Text);
            lblgvOpeningDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningDebit.Text);
        }
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
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

            DataTable dtAdd = (DataTable)Session["dtCurrency"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
           objPageCmn.FillData((object)GvCOA, view.ToTable(), "", "");
            Session["dtFilter_Chart_Acc"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            foreach (GridViewRow gvr in GvCOA.Rows)
            {
                Label lblgvOpeningCredit = (Label)gvr.FindControl("lblgvOpeningCrBalance");
                Label lblgvOpeningDebit = (Label)gvr.FindControl("lblgvOpeningDrBalance");

                lblgvOpeningCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningCredit.Text);
                lblgvOpeningDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningDebit.Text);
            }
        }
    }
    protected void GvCOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        DataTable dt = (DataTable)Session["dtFilter_Chart_Acc"];
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
        Session["dtFilter_Chart_Acc"] = dt;
        //Common Function add By Lokesh on 23-05-2015
       objPageCmn.FillData((object)GvCOA, dt, "", "");
        foreach (GridViewRow gvr in GvCOA.Rows)
        {
            Label lblgvOpeningCredit = (Label)gvr.FindControl("lblgvOpeningCrBalance");
            Label lblgvOpeningDebit = (Label)gvr.FindControl("lblgvOpeningDrBalance");

            lblgvOpeningCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningCredit.Text);
            lblgvOpeningDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningDebit.Text);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dtSInquiryEdit = objCOA.GetCOAByTransId(StrCompId, e.CommandArgument.ToString());

        if (dtSInquiryEdit.Rows.Count > 0)
        {

        }

        hdnCOAId.Value = e.CommandArgument.ToString();
        b = objCOA.DeleteCOA(StrCompId, hdnCOAId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }

        FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
    }
    protected void btnCOACancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
           objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
        }
    }
    protected void btnCOASave_Click(object sender, EventArgs e)
    {
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strCustomerId = string.Empty;
        string strReceivedEmployeeId = string.Empty;
        string strHandledEmployeeId = string.Empty;
        string strBuyingPriority = string.Empty;
        string strSendMail = string.Empty;
        string strPost = string.Empty;
        string strLockAccount = string.Empty;

        if (txtAccountNo.Text == "")
        {
            DisplayMessage("Enter Account No.");
            txtAccountNo.Focus();
            return;
        }
        else
        {
            if (hdnCOAId.Value == "0")
            {
                DataTable dtAccountNo = objCOA.GetCOAByAccountNo(StrCompId, txtAccountNo.Text);
                if (dtAccountNo.Rows.Count > 0)
                {
                    DisplayMessage("Account No. Already Exits");
                    txtAccountNo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
                    return;
                }
            }
        }

        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name");
            txtAccountName.Focus();
            return;
        }
        else
        {
            if (txtAccountGroup.Text == "")
            {
                DisplayMessage("Please Enter Account Group");
                txtAccountGroup.Focus();
                return;
            }
        }

        //FinancialYear Status
        string strFYStatus = string.Empty;
        DataTable dtFY = objFYI.GetInfoAllTrue(Session["CompId"].ToString());
        if (dtFY.Rows.Count > 0)
        {
            dtFY = new DataView(dtFY, "Status='Close'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtFY.Rows.Count > 0)
            {
                strFYStatus = "False";
                //DisplayMessage("You Cant Change your Opening Balances");
            }
            else
            {
                DataTable dtReCheck = objFYI.GetInfoAllTrue(Session["CompId"].ToString());
                if (dtReCheck.Rows.Count > 0)
                {
                    string strFId = dtReCheck.Rows[0]["Trans_Id"].ToString();
                    string strReStatus = dtReCheck.Rows[0]["Status"].ToString();
                    if (strReStatus == "Open" || strReStatus == "ReOpen")
                    {
                        if (strFId == Session["FinanceYearId"].ToString())
                        {
                            strFYStatus = "True";
                        }
                        else
                        {
                            strFYStatus = "False";
                            DisplayMessage("You Cant Change your Opening Balances");
                        }
                    }
                }
            }
        }
        else
        {
            strFYStatus = "False";
            DisplayMessage("You Cant Change your Opening Balances");
        }

        string strCurrencyId = string.Empty;
        if (ddlCurrency.SelectedValue != "--Select--")
        {
            strCurrencyId = ddlCurrency.SelectedValue;
        }
        else if (ddlCurrency.SelectedValue == "--Select--")
        {
            //strCurrencyId = "0";

            DisplayMessage("Select Account Currency");
            ddlCurrency.Focus();
            return;
        }

        string strAccountGroupId = string.Empty;
        if (txtAccountGroup.Text != "")
        {
            strAccountGroupId = GetAccGroupId(txtAccountGroup.Text);
        }
        else
        {
            strAccountGroupId = "0";
        }


        if (txtOpeningCreditBalance.Text != "")
        {

        }
        else
        {
            txtOpeningCreditBalance.Text = "0";
        }
        if (txtOpeningDebitBalance.Text != "")
        {

        }
        else
        {
            txtOpeningDebitBalance.Text = "0";
        }
        if (txtDebitAmount.Text != "")
        {

        }
        else
        {
            txtDebitAmount.Text = "0";
        }
        if (txtCreditAmount.Text != "")
        {

        }
        else
        {
            txtCreditAmount.Text = "0";
        }
        if (txtLastDebit.Text != "")
        {

        }
        else
        {
            txtLastDebit.Text = "0";
        }
        if (txtLastCredit.Text != "")
        {

        }
        else
        {
            txtLastCredit.Text = "0";
        }

        //for Customer & Supplier Account
        string strReceiveVoucherAcc = string.Empty;
        DataTable dtParamReceive = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
        if (dtParamReceive.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParamReceive.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
        }

        string strPaymentVoucherAcc = string.Empty;
        DataTable dtPaymentVoucher = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPaymentVoucherAcc = "0";
        }

        //Check Opening Balance Location Validations
        if (strFYStatus == "True")
        {
            foreach (GridViewRow gv in GvLocation.Rows)
            {
                double OpeningDebitAmount = 0;
                double OpeningCreditAmount = 0;
                double OpeningFDebitAmount = 0;
                double OpeningFCreditAmount = 0;
                TextBox txtgvDebit = (TextBox)gv.FindControl("txtgvDebit");
                TextBox txtgvCredit = (TextBox)gv.FindControl("txtgvCredit");
                TextBox txtgvFDebit = (TextBox)gv.FindControl("txtgvForeignDebit");
                TextBox txtgvFCredit = (TextBox)gv.FindControl("txtgvForeignCredit");

                if (strReceiveVoucherAcc == hdnCOAId.Value || strPaymentVoucherAcc == hdnCOAId.Value)
                {

                }
                else
                {
                    if (txtgvDebit.Text != "")
                    {
                        OpeningDebitAmount = Convert.ToDouble(txtgvDebit.Text);
                    }
                    if (txtgvCredit.Text != "")
                    {
                        OpeningCreditAmount = Convert.ToDouble(txtgvCredit.Text);
                    }
                    if (txtgvFDebit.Text != "")
                    {
                        OpeningFDebitAmount = Convert.ToDouble(txtgvFDebit.Text);
                    }
                    if (txtgvFCredit.Text != "")
                    {
                        OpeningFCreditAmount = Convert.ToDouble(txtgvFCredit.Text);
                    }

                    if (OpeningDebitAmount != 0 && OpeningCreditAmount != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('You cant enter opening both values');", true);
                        return;
                    }

                    if (OpeningFDebitAmount != 0 && OpeningFCreditAmount != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('You cant enter opening both values');", true);
                        return;
                    }

                    if (OpeningDebitAmount != 0)
                    {
                        if (OpeningFCreditAmount != 0)
                        {
                            DisplayMessage("Need to Enter Both Debit Values");
                            return;
                        }
                    }
                    if (OpeningCreditAmount != 0)
                    {
                        if (OpeningFDebitAmount != 0)
                        {
                            DisplayMessage("Need to Enter Both Credit Values");
                            return;
                        }
                    }

                    if (OpeningFDebitAmount != 0)
                    {
                        if (OpeningDebitAmount == 0)
                        {
                            DisplayMessage("Need to Enter Local Debit Values");
                            return;
                        }
                    }
                    if (OpeningFCreditAmount != 0)
                    {
                        if (OpeningCreditAmount == 0)
                        {
                            DisplayMessage("Need to Enter Local Credit Values");
                            return;
                        }
                    }
                }
            }
        }

        if (chkLockAccount.Checked == true)
        {
            strLockAccount = "True";
            if (hdnCOAId.Value != "0" && hdnCOAId.Value != "")
            {
                DataTable dtParam = objAcParameter.GetParameterMasterAllTrue(StrCompId);
                dtParam = new DataView(dtParam, "Param_Value='" + hdnCOAId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtParam.Rows.Count > 0)
                {
                    chkLockAccount.Checked = false;
                    DisplayMessage("This Account Used in Parameters So you cant Lock this Account");
                    return;
                }
            }
        }
        else
        {
            strLockAccount = "False";
        }




        //Check FYI Status
        //string Status = string.Empty;
        //DataTable dtClose = objFYI.GetInfoAllTrue(StrCompId);
        //if (dtClose.Rows.Count > 0)
        //{
        //    dtClose = new DataView(dtClose, "Status='Close' or Status='ReOpen'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (dtClose.Rows.Count > 0)
        //    {
        //        Status = "Close";
        //    }
        //    else
        //    {
        //        Status = "Open";
        //    }
        //}
        //else
        //{
        //    Status = "Open";
        //}

        string strContactId = string.Empty;
        if (!string.IsNullOrEmpty(txtContactList.Text))
        {
            strContactId = txtContactList.Text.Split('/')[1].ToString();
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnCOAId.Value != "0")
            {
                b = objCOA.UpdateCOA(StrCompId, hdnCOAId.Value, txtAccountNo.Text, txtAccountName.Text, txtAccountNameL.Text, "0.00", strAccountGroupId, txtOpeningCreditBalance.Text, txtOpeningDebitBalance.Text, strCurrencyId, txtDebitAmount.Text, txtCreditAmount.Text,txtAccountType.Text, txtLastDebit.Text, txtLastCredit.Text, strLockAccount, txtGstNo.Text, strContactId, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");

                    //if (Status == "Open")
                    //{
                    if (strReceiveVoucherAcc == hdnCOAId.Value || strPaymentVoucherAcc == hdnCOAId.Value)
                    {

                    }
                    else
                    {
                        if (strFYStatus == "True")
                        {
                            DataTable dtSubCOAData = objSubCOA.GetSubCOAAllTrueByAccountId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnCOAId.Value, Session["FinanceYearId"].ToString(), ref trns);
                            if (dtSubCOAData.Rows.Count > 0)
                            {
                                dtSubCOAData = new DataView(dtSubCOAData, "Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (GridViewRow gvr in GvLocation.Rows)
                                {
                                    HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                                    if (hdngvLocationId.Value != "0" && hdngvLocationId.Value != "")
                                    {
                                        DataTable dtSubByLocation = new DataView(dtSubCOAData, "Location_Id='" + hdngvLocationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtSubByLocation.Rows.Count > 0)
                                        {
                                            objSubCOA.DeleteSubCOADetailByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, dtSubByLocation.Rows[0]["Trans_Id"].ToString(), ref trns);
                                        }
                                    }
                                }
                            }


                            //ForSupplierOpeningBalanceAccordingToLocation.
                            foreach (GridViewRow gvr in GvLocation.Rows)
                            {
                                string strCompanyDebit = string.Empty;
                                string strCompanyCredit = string.Empty;
                                HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                                HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
                                TextBox txtgvDebit = (TextBox)gvr.FindControl("txtgvDebit");
                                TextBox txtgvCredit = (TextBox)gvr.FindControl("txtgvCredit");
                                TextBox txtgvFDebit = (TextBox)gvr.FindControl("txtgvForeignDebit");
                                TextBox txtgvFCredit = (TextBox)gvr.FindControl("txtgvForeignCredit");

                                if (txtgvDebit.Text != "")
                                {
                                    string strCompanyCrrValueDr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvDebit.Text);
                                    strCompanyDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                }
                                else
                                {
                                    txtgvDebit.Text = "0.00";
                                    strCompanyDebit = "0.00";
                                }
                                if (txtgvCredit.Text != "")
                                {
                                    string strCompanyCrrValueCr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvCredit.Text);
                                    strCompanyCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                }
                                else
                                {
                                    txtgvCredit.Text = "0.00";
                                    strCompanyCredit = "0.00";
                                }

                                if (txtgvFDebit.Text == "")
                                {
                                    txtgvFDebit.Text = "0.00";
                                }
                                if (txtgvFCredit.Text == "")
                                {
                                    txtgvFCredit.Text = "0.00";
                                }
                                objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, hdnCOAId.Value, "0", txtgvDebit.Text, txtgvCredit.Text, txtgvFDebit.Text, txtgvFCredit.Text, ddlCurrency.SelectedValue, strCompanyDebit, strCompanyCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                    //}

                    btnList_Click(null, null);
                    DisplayMessage("Record Updated Successfully", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                b = objCOA.InsertCOA(StrCompId, txtAccountNo.Text, txtAccountName.Text, txtAccountNameL.Text, "0.00", strAccountGroupId, txtOpeningCreditBalance.Text, txtOpeningDebitBalance.Text, strCurrencyId, txtDebitAmount.Text, txtCreditAmount.Text, txtAccountType.Text, txtLastDebit.Text, txtLastCredit.Text, strLockAccount, txtGstNo.Text, strContactId, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    //Add Code For Account No
                    if (txtAccountNo.Text == ViewState["DocNo"].ToString())
                    {
                        DataTable dtCount = objCOA.GetCOAAll(Session["CompId"].ToString(), ref trns);
                        try
                        {
                            dtCount = new DataView(dtCount, "Account_No like '" + txtAccountNo.Text + "%'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                        if (dtCount.Rows.Count == 0)
                        {
                            if (txtAccountNo.Text == "")
                            {
                                txtAccountNo.Text = "1";
                            }

                            objCOA.Updatecode(b.ToString(), txtAccountNo.Text + "1", ref trns);
                            txtAccountNo.Text = txtAccountNo.Text + "1";
                        }
                        else
                        {
                            if (txtAccountNo.Text == "")
                            {
                                txtAccountNo.Text = dtCount.Rows.Count.ToString();
                            }
                            objCOA.Updatecode(b.ToString(), txtAccountNo.Text + dtCount.Rows.Count, ref trns);
                            txtAccountNo.Text = txtAccountNo.Text + dtCount.Rows.Count;
                        }
                    }



                    if (strReceiveVoucherAcc == b.ToString() || strPaymentVoucherAcc == b.ToString())
                    {

                    }
                    else
                    {
                        if (strFYStatus == "True")
                        {
                            DataTable dtSubCOAData = objSubCOA.GetSubCOAAllTrueByAccountId(Session["CompId"].ToString(), Session["BrandId"].ToString(), b.ToString(), Session["FinanceYearId"].ToString(), ref trns);
                            if (dtSubCOAData.Rows.Count > 0)
                            {
                                foreach (GridViewRow gvr in GvLocation.Rows)
                                {
                                    HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                                    if (hdngvLocationId.Value != "0" && hdngvLocationId.Value != "")
                                    {
                                        DataTable dtSubByLocation = new DataView(dtSubCOAData, "Location_Id='" + hdngvLocationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtSubByLocation.Rows.Count > 0)
                                        {
                                            objSubCOA.DeleteSubCOADetailByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, dtSubByLocation.Rows[0]["Trans_Id"].ToString(), ref trns);
                                        }
                                    }
                                }
                            }


                            //ForSupplierOpeningBalanceAccordingToLocation.
                            foreach (GridViewRow gvr in GvLocation.Rows)
                            {
                                string strCompanyDebit = string.Empty;
                                string strCompanyCredit = string.Empty;
                                HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                                HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
                                TextBox txtgvDebit = (TextBox)gvr.FindControl("txtgvDebit");
                                TextBox txtgvCredit = (TextBox)gvr.FindControl("txtgvCredit");
                                TextBox txtgvFDebit = (TextBox)gvr.FindControl("txtgvForeignDebit");
                                TextBox txtgvFCredit = (TextBox)gvr.FindControl("txtgvForeignCredit");

                                if (txtgvDebit.Text != "")
                                {
                                    string strCompanyCrrValueDr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvDebit.Text);
                                    strCompanyDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                                }
                                else
                                {
                                    txtgvDebit.Text = "0.00";
                                    strCompanyDebit = "0.00";
                                }
                                if (txtgvCredit.Text != "")
                                {
                                    string strCompanyCrrValueCr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvCredit.Text);
                                    strCompanyCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                }
                                else
                                {
                                    txtgvCredit.Text = "0.00";
                                    strCompanyCredit = "0.00";
                                }

                                if (txtgvFDebit.Text == "")
                                {
                                    txtgvFDebit.Text = "0.00";
                                }
                                if (txtgvFCredit.Text == "")
                                {
                                    txtgvFCredit.Text = "0.00";
                                }
                                objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, b.ToString(), "0", txtgvDebit.Text, txtgvCredit.Text, txtgvFDebit.Text, txtgvFCredit.Text, ddlCurrency.SelectedValue, strCompanyDebit, strCompanyCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }

                    }
                    //End Code
                    DisplayMessage("Record Saved Successfully !!", "green");
                }
                else
                {
                    DisplayMessage("Record  Not Saved");
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
    public string GetCurrencyForOpening(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strToCurrency, strCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    protected void txtAccountGroup_TextChanged(object sender, EventArgs e)
    {
        //string strAddValue = "1";
        string strAccountNo = string.Empty;
        string strAccGroupId = string.Empty;
        if (txtAccountGroup.Text != "")
        {
            strAccGroupId = GetAccGroupId(txtAccountGroup.Text);
            if (strAccGroupId != "" && strAccGroupId != "0")
            {
                //string strNatureAccId = string.Empty;
                //DataTable dtNGroupId = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, strAccGroupId);
                //if (dtNGroupId.Rows.Count > 0)
                //{
                //    strNatureAccId = dtNGroupId.Rows[0]["N_Group_ID"].ToString();
                //    DataTable dtNGrpDetail = objAccGroup.GetAccountsGroupByNOA(StrCompId, StrBrandId, int.Parse(strNatureAccId));
                //    if (dtNGrpDetail.Rows.Count > 0)
                //    {
                //        string strGroupId = string.Empty;
                //        for (int i = 0; i < dtNGrpDetail.Rows.Count; i++)
                //        {
                //            strGroupId += dtNGrpDetail.Rows[i]["Ac_Group_Id"].ToString() + ",";
                //        }

                //        if (strGroupId != "")
                //        {
                //            DataTable dtCOADetail = objCOA.GetCOAAllTrue(StrCompId);
                //            if (dtCOADetail.Rows.Count > 0)
                //            {
                //                dtCOADetail = new DataView(dtCOADetail, "Acc_Group_Id  in (" + strGroupId.Substring(0, strGroupId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                //                if (dtCOADetail.Rows.Count > 0)
                //                {
                //                    string strRowCount = dtCOADetail.Rows.Count.ToString();

                //                    DataTable dtNOA = objNOA.GetNatureAccountsAllTrue();
                //                    if (dtNOA.Rows.Count > 0)
                //                    {
                //                        for (int n = 0; n < dtNOA.Rows.Count; n++)
                //                        {
                //                            if (strNatureAccId == dtNOA.Rows[n]["N_Group_Id"].ToString())
                //                            {
                //                                strAccountNo = (strNatureAccId + "00000");
                //                                txtAccountNo.Text = ((float.Parse(strAccountNo)) + (float.Parse(strRowCount)) + (float.Parse(strAddValue))).ToString();
                //                            }
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    strAccountNo = (strNatureAccId + "00000");
                //                    txtAccountNo.Text = strAccountNo;
                //                }
                //            }
                //            else
                //            {
                //                strAccountNo = (strNatureAccId + "00000");
                //                txtAccountNo.Text = strAccountNo;
                //            }
                //        }
                //    }
                //}
                //else
                //{

                //}
            }
            else
            {
                DisplayMessage("Select Group In Suggestions Only");
                txtAccountGroup.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountGroup);
            }
        }
    }
    private string GetAccGroupId(string strAccountGroupName)
    {
        string retval = string.Empty;
        if (strAccountGroupName != "")
        {
            DataTable dtGroup = objAccGroup.GetGroupsByGroupName(StrCompId, StrBrandId, strAccountGroupName.Split('/')[0].ToString());
            if (dtGroup.Rows.Count > 0)
            {
                retval = (strAccountGroupName.Split('/'))[strAccountGroupName.Split('/').Length - 1];

                DataTable dtGroupById = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, retval);
                if (dtGroupById.Rows.Count > 0)
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
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        hdnCOAId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objCOA.DeleteCOA(StrCompId, hdnCOAId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }

    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccGroupName(string prefixText, int count, string contextKey)
    {
        Ac_Groups ObjAccountGroup = new Ac_Groups(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjAccountGroup.GetGroupsAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

        DataTable dt = new DataView(dt1, "Ac_GroupName like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Ac_GroupName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["Ac_GroupName"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
    #endregion

    #region Bin Section
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
    protected void GvCOABin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        GvCOABin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
       objPageCmn.FillData((object)GvCOABin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvCOABin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvCOABin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvCOABin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        foreach (GridViewRow gvr in GvCOABin.Rows)
        {
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");

            lblgvOpeningBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
        }
    }
    protected void GvCOABin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objCOA.GetCOAAllFalse(StrCompId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
       objPageCmn.FillData((object)GvCOABin, dt, "", "");
        lblSelectedRecord.Text = "";

        foreach (GridViewRow gvr in GvCOABin.Rows)
        {
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");

            lblgvOpeningBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
        }
    }
    public void FillGridBin()
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        DataTable dt = new DataTable();
        dt = objCOA.GetCOAAllFalse(StrCompId);
        //Common Function add By Lokesh on 23-05-2015
       objPageCmn.FillData((object)GvCOABin, dt, "", "");
        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            //ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }

        foreach (GridViewRow gvr in GvCOABin.Rows)
        {
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");

            lblgvOpeningBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
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


            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
           objPageCmn.FillData((object)GvCOABin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            foreach (GridViewRow gvr in GvCOABin.Rows)
            {
                Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");

                lblgvOpeningBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
            }
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objCOA.GetCOAAllFalse(StrCompId);

        if (GvCOABin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objCOA.DeleteCOA(StrCompId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                int flag = 0;
                foreach (GridViewRow Gvr in GvCOABin.Rows)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvCOABin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvCOABin.Rows.Count; i++)
        {
            ((CheckBox)GvCOABin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvCOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvCOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvCOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvCOABin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvCOABin.Rows[index].FindControl("chkSelect")).Checked)
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
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
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
            for (int i = 0; i < GvCOABin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvCOABin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvCOABin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
           objPageCmn.FillData((object)GvCOABin, dtUnit1, "", "");
            foreach (GridViewRow gvr in GvCOABin.Rows)
            {
                Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");

                lblgvOpeningBalance.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
            }
            ViewState["Select"] = null;
        }
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
                    b = objCOA.DeleteCOA(StrCompId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            foreach (GridViewRow Gvr in GvCOABin.Rows)
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

    #endregion

    #region User defined Function
    protected string GetLocationName(string strLocationId)
    {
        string strLocationName = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocName = ObjLocation.GetLocationMasterById(StrCompId, strLocationId);
            if (dtLocName.Rows.Count > 0)
            {
                strLocationName = dtLocName.Rows[0]["Location_Name"].ToString();
            }
        }
        else
        {
            strLocationName = "";
        }
        return strLocationName;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCurrName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCurrName.Rows.Count > 0)
            {
                strCurrencyName = dtCurrName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    public void FillLocationView()
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
            //Common Function add By Lokesh on 13-05-2015
           objPageCmn.FillData((object)GVLocationView, dtLoc, "", "");
        }
    }
    public void FillLocation()
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
            //Common Function add By Lokesh on 13-05-2015
           objPageCmn.FillData((object)GvLocation, dtLoc, "", "");
        }
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
    private void FillGrid()
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        DataTable dtBrand = objCOA.GetCOAAllTrueForCOA(StrCompId);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["dtFilter_Chart_Acc"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
           objPageCmn.FillData((object)GvCOA, dtBrand, "", "");
        }
        else
        {
            GvCOA.DataSource = null;
            GvCOA.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";

        foreach (GridViewRow gvr in GvCOA.Rows)
        {
            Label lblgvOpeningCredit = (Label)gvr.FindControl("lblgvOpeningCrBalance");
            Label lblgvOpeningDebit = (Label)gvr.FindControl("lblgvOpeningDrBalance");

            lblgvOpeningCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningCredit.Text);
            lblgvOpeningDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvOpeningDebit.Text);
        }
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
        FillGrid();
        txtAccountName.Text = "";
        txtAccountNameL.Text = "";
        //txtOpeningBalance.Text = "";
        txtAccountGroup.Text = "";

        FillCurrency();
        txtAccountType.Text = "";
        chkLockAccount.Checked = false;
        trLock.Visible = false;

        PnlNewContant.Enabled = true;
        hdnCOAId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtAccountNo.Text = objDocNo.GetDocumentNo(true, "0", false, "150", "175", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = txtAccountNo.Text;
        txtGstNo.Text = "";
        txtContactList.Text = "";
    }
    #endregion

    #region View
    protected void BtnCancelView_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;
        ViewReset();
    }
    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;
        ViewReset();
    }
    void ViewReset()
    {
        btnList_Click(null, null);
        hdnCOAIdView.Value = "";
        txtVAccountNo.Text = "";
        txtVAccountName.Text = "";
        txtVAccountNameL.Text = "";
        FillLocationView();
        txtVAccountGroup.Text = "";
        FillCurrency();
        chkVLockAccount.Checked = false;
        hdnCOAId.Value = "";
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        DataTable dtCOAEdit = objCOA.GetCOAByTransId(StrCompId, e.CommandArgument.ToString());

        hdnCOAIdView.Value = e.CommandArgument.ToString();

        hdnCOAId.Value = e.CommandArgument.ToString();
        txtVAccountNo.Text = dtCOAEdit.Rows[0]["Account_No"].ToString();
        txtVAccountName.Text = dtCOAEdit.Rows[0]["AccountName"].ToString();
        txtVAccountNameL.Text = dtCOAEdit.Rows[0]["AccountNameL"].ToString();

        string strAccoutGroupId = dtCOAEdit.Rows[0]["Acc_Group_Id"].ToString();
        txtVAccountGroup.Text = GetAccountGroupName(strAccoutGroupId);

        string strCurrencyId = dtCOAEdit.Rows[0]["Currency_Id"].ToString();
        txtVCurrency.Text = GetCurrency(strCurrencyId);

        bool strLockAcc = Convert.ToBoolean(dtCOAEdit.Rows[0]["Field1"].ToString());
        if (strLockAcc)
        {
            chkVLockAccount.Checked = true;
        }
        else
        {
            chkVLockAccount.Checked = false;
        }

        //for Customer & Supplier Account
        string strReceiveVoucherAcc = string.Empty;
        DataTable dtParam = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
        }

        string strPaymentVoucherAcc = string.Empty;
        DataTable dtPaymentVoucher = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPaymentVoucherAcc = "0";
        }

        //For Location Opening Balance 
        foreach (GridViewRow gvr in GVLocationView.Rows)
        {
            HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
            HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
            Label txtgvDebit = (Label)gvr.FindControl("txtgvDebit");
            Label txtgvCredit = (Label)gvr.FindControl("txtgvCredit");
            Label txtgvForeignDebit = (Label)gvr.FindControl("txtgvForeignDebit");
            Label txtgvForeignCredit = (Label)gvr.FindControl("txtgvForeignCredit");

            if (strReceiveVoucherAcc == hdnCOAId.Value || strPaymentVoucherAcc == hdnCOAId.Value)
            {
                DataTable dtSubCOA = objSubCOA.GetSubCOAAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, Session["FinanceYearId"].ToString());
                if (dtSubCOA.Rows.Count > 0)
                {
                    dtSubCOA = new DataView(dtSubCOA, "AccTransId='" + hdnCOAId.Value + "' and Other_Account_No<>'0'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtSubCOA.Rows.Count > 0)
                    {
                        double DebitAmount = 0;
                        double CreditAmount = 0;
                        double DebitAmountFr = 0;
                        double CreditAmountFr = 0;
                        double DebitLocalamountTotal = 0;
                        double CreditLocalamountTotal = 0;
                        double DebitForeignamountTotal = 0;
                        double CreditForeignamountTotal = 0;

                        for (int i = 0; i < dtSubCOA.Rows.Count; i++)
                        {
                            if (dtSubCOA.Rows[i]["LDr_Amount"].ToString() != "")
                            {
                                DebitAmount = Convert.ToDouble(dtSubCOA.Rows[i]["LDr_Amount"].ToString());
                            }
                            if (dtSubCOA.Rows[i]["LCr_Amount"].ToString() != "")
                            {
                                CreditAmount = Convert.ToDouble(dtSubCOA.Rows[i]["LCr_Amount"].ToString());
                            }
                            if (dtSubCOA.Rows[i]["FDr_Amount"].ToString() != "")
                            {
                                DebitAmountFr = Convert.ToDouble(dtSubCOA.Rows[i]["FDr_Amount"].ToString());
                            }
                            if (dtSubCOA.Rows[i]["FCr_Amount"].ToString() != "")
                            {
                                CreditAmountFr = Convert.ToDouble(dtSubCOA.Rows[i]["FCr_Amount"].ToString());
                            }

                            DebitLocalamountTotal += DebitAmount;
                            CreditLocalamountTotal += CreditAmount;
                            DebitForeignamountTotal += DebitAmountFr;
                            CreditForeignamountTotal += CreditAmountFr;
                        }

                        txtgvDebit.Text = DebitLocalamountTotal.ToString();
                        txtgvDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvDebit.Text);
                        txtgvCredit.Text = CreditLocalamountTotal.ToString();
                        txtgvCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvCredit.Text);
                        txtgvForeignDebit.Text = DebitForeignamountTotal.ToString();
                        txtgvForeignDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignDebit.Text);
                        txtgvForeignCredit.Text = CreditForeignamountTotal.ToString();
                        txtgvForeignCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignCredit.Text);
                    }
                }
            }
            else
            {
                DataTable dtSubCOA = objSubCOA.GetSubCOAAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, Session["FinanceYearId"].ToString());
                if (dtSubCOA.Rows.Count > 0)
                {
                    dtSubCOA = new DataView(dtSubCOA, "AccTransId='" + hdnCOAId.Value + "' and Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtSubCOA.Rows.Count > 0)
                    {
                        txtgvDebit.Text = dtSubCOA.Rows[0]["LDr_Amount"].ToString();
                        txtgvDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvDebit.Text);
                        txtgvCredit.Text = dtSubCOA.Rows[0]["LCr_Amount"].ToString();
                        txtgvCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvCredit.Text);
                        txtgvForeignDebit.Text = dtSubCOA.Rows[0]["FDr_Amount"].ToString();
                        txtgvForeignDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignDebit.Text);
                        txtgvForeignCredit.Text = dtSubCOA.Rows[0]["FCr_Amount"].ToString();
                        txtgvForeignCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignCredit.Text);
                    }
                }
            }
        }

        //AllPageCode();
        PanelView1.Visible = true;
        PanelView2.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Chart_Of_Account_Popup()", true);
    }
    protected string GetAccountGroupName(string strAccountGroupId)
    {
        string strAccountGroupName = string.Empty;
        if (strAccountGroupId != "0" && strAccountGroupId != "")
        {
            DataTable dtAccGroup = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, strAccountGroupId);
            if (dtAccGroup.Rows.Count > 0)
            {
                strAccountGroupName = dtAccGroup.Rows[0]["Ac_GroupName"].ToString();
            }
        }
        else
        {
            strAccountGroupName = "";
        }
        return strAccountGroupName;
    }
    protected string GetCurrency(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCurrency = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCurrency.Rows.Count > 0)
            {
                strCurrencyName = dtCurrency.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    #endregion
    protected void lnkProductBulider_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/ProductBuilder.aspx')", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";
        DataTable dt = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }
            dt = null;
        }
        return filterlist;

    }

    #region TreeView
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewCOA.Visible == true)//To show grid view
        {
            TreeViewCOA.Visible = false;
            GvCOA.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else //To show tree view
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvCOA.Visible = false;
            TreeViewCOA.Visible = true;

            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }
    protected void btnTreeView_Click(object sender, ImageClickEventArgs e)
    {
        if (TreeViewCOA.Visible == true)
        {
            TreeViewCOA.Visible = false;
            GvCOA.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvCOA.Visible = false;
            TreeViewCOA.Visible = true;
        }
        btnTreeView.Focus();
    }
    protected void TreeViewCOA_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewCOA.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewCOA.Nodes.Clear();
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllTrue();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Name"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn.SelectAction = TreeNodeSelectAction.None;
            TreeViewCOA.Nodes.Add(tn);

            FillChild((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn);
            i++;
        }

        TreeViewCOA.DataBind();
        TreeViewCOA.CollapseAll();
    }
    private void FillChild(int s, TreeNode tn1)
    {
        DataTable dt = new DataTable();
        string x = "N_Group_ID=" + s + " and Parant_Ac_Group_Id=" + "'0'" + "";
        dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, x, "", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn2 = new TreeNode();
            tn2.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn2.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn2.SelectAction = TreeNodeSelectAction.None;
            tn1.ChildNodes.Add(tn2);

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn2);
            i++;
        }
        TreeViewCOA.DataBind();
    }
    private void FillChild1(int s, TreeNode tn2)
    {
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        dt = GetAccountGroupByParentId(s);

        int i = 0;
        int j = 0;

        if (dt.Rows.Count == 0)
        {
            dt2 = GetAllCOA(s, tn2);
            while (j < dt2.Rows.Count)
            {
                TreeNode tn4 = new TreeNode();
                tn4.Text = "(" + dt2.Rows[j]["Account_No"].ToString() + "/" + dt2.Rows[j]["AccountName"].ToString() + "/" + GetCurrency(dt2.Rows[j]["Currency_Id"].ToString()) + ")";
                tn4.Value = dt2.Rows[j]["Trans_Id"].ToString();
                //tn4.SelectAction = TreeNodeSelectAction.None;
                tn2.ChildNodes.Add(tn4);

                j++;
            }
        }
        else
        {
            while (i < dt.Rows.Count)
            {
                TreeNode tn3 = new TreeNode();
                tn3.Text = dt.Rows[i]["Ac_GroupName"].ToString();
                tn3.Value = dt.Rows[i]["Trans_Id"].ToString();
                tn3.SelectAction = TreeNodeSelectAction.None;
                tn2.ChildNodes.Add(tn3);

                dt2 = GetAllCOA(Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString()), tn3);
                while (j < dt2.Rows.Count)
                {
                    TreeNode tn4 = new TreeNode();
                    tn4.Text = "(" + dt2.Rows[j]["Account_No"].ToString() + "/" + dt2.Rows[j]["AccountName"].ToString() + "/" + GetCurrency(dt2.Rows[j]["Currency_Id"].ToString()) + ")";
                    tn4.Value = dt2.Rows[j]["Trans_Id"].ToString();
                    //tn4.SelectAction = TreeNodeSelectAction.None;
                    tn3.ChildNodes.Add(tn4);
                    FillChild1((Convert.ToInt32(dt2.Rows[j]["Trans_Id"].ToString())), tn4);
                    j++;
                }
                i++;
            }
        }
        TreeViewCOA.DataBind();
    }
    public DataTable GetAccountGroupByParentId(int ParentId) //Function to get entries of same ProductId
    {
        //dt = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, strLocationId, "0");
        string query = "Parant_Ac_Group_Id='" + ParentId + "'";
        DataTable dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        return dt;
    }
    public DataTable GetAllCOA(int s, TreeNode tn)
    {
        string query = "Acc_Group_Id='" + s + "'";
        DataTable dt = objCOA.GetCOAAllTrueForCOA(StrCompId);
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        return dt;
    }

    #endregion

    #region AccountGroup_TreeView

    protected void navTreeAccontGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)navTreeAccontGroup.SelectedValue.ToString());
        DataTable dtAccGroupEdit = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, navTreeAccontGroup.SelectedValue.ToString());
        {
            txtAccountGroup.Text = dtAccGroupEdit.Rows[0]["Ac_GroupName"].ToString() + "/" + dtAccGroupEdit.Rows[0]["Ac_Group_Id"].ToString();
            navTreeAccontGroup.Visible = false;
            Div_Tree.Visible = false;
            lblslectgroup.Visible = false;
            btnAddAccountGroup.Text = "Open Group";
        }
        txtAccountGroup_TextChanged(sender, e);
    }

    protected void btnAddAccountGroup_OnClick(object sender, EventArgs e)
    {
        BindTreeView_AcGroup();
        if (!navTreeAccontGroup.Visible)
        {
            navTreeAccontGroup.Visible = true;
            Div_Tree.Visible = true;
            lblslectgroup.Visible = true;
            btnAddAccountGroup.Text = "Close Group";
        }
        else
        {
            navTreeAccontGroup.Visible = false;
            Div_Tree.Visible = false;
            lblslectgroup.Visible = false;
            btnAddAccountGroup.Text = "Open Group";
        }
    }
    private void BindTreeView_AcGroup()//fucntion to fill up TreeView according to parent child nodes
    {
        navTreeAccontGroup.Nodes.Clear();
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllTrue();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Name"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn.SelectAction = TreeNodeSelectAction.None;
            navTreeAccontGroup.Nodes.Add(tn);

            FillChildACGroup((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn);
            i++;
        }
        navTreeAccontGroup.DataBind();

        navTreeAccontGroup.CollapseAll();
    }
    private void FillChildACGroup(int s, TreeNode tn1)
    {

        DataTable dt = new DataTable();
        string x = "N_Group_ID=" + s + " and Parant_Ac_Group_Id=" + "'0'" + "";
        dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, x, "", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn2 = new TreeNode();
            tn2.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn2.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn2.SelectAction = TreeNodeSelectAction.Select;
            tn1.ChildNodes.Add(tn2);

            FillChild1_AcGroup((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn2);
            i++;
        }
        navTreeAccontGroup.DataBind();
    }
    private void FillChild1_AcGroup(int s, TreeNode tn2)
    {
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        dt = GetAccountGroupByParentId(s);

        int i = 0;
        int j = 0;
        //if (dt.Rows.Count == 0)
        //{
        //    FillChild1(s, tn2);
        //}
        //else
        //{
        while (i < dt.Rows.Count)
        {
            TreeNode tn3 = new TreeNode();
            tn3.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn3.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn3.SelectAction = TreeNodeSelectAction.Select;
            tn2.ChildNodes.Add(tn3);

            //dt2 = GetAllCOA(Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString()), tn3);
            //while (j < dt2.Rows.Count)
            //{
            //    TreeNode tn4 = new TreeNode();
            //    tn4.Text = dt2.Rows[j]["AccountName"].ToString();
            //    tn4.Value = dt2.Rows[j]["Trans_Id"].ToString();
            //    //tn4.SelectAction = TreeNodeSelectAction.None;
            //    tn3.ChildNodes.Add(tn4);

            //    j++;
            //}

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn3);
            i++;
        }
        navTreeAccontGroup.DataBind();
        //}
    }
    #endregion

    #region PrintReport
    protected void IbtnPrint_Command(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Accounts_Report/TrialBalanceReport.aspx','window','width=1024');", true);
    }
    #endregion

    public class clsCofExcelImport
    {
        public bool is_valid { get; set; }
        public string validation_remark { get; set; }
        public string loc_id { get; set; }
        public string loc_name { get; set; }
        public string ac_id { get; set; }
        public string ac_no { get; set; }
        public string ac_name { get; set; }
        public string ac_name_l { get; set; }
        public string ac_group_id { get; set; }
        public string ac_group_name { get; set; }
        public double ob { get; set; }
        public string balance_type { get; set; }
        public string currency_id { get; set; }
    }


    protected void btnUploadExcel_Click(object sender, EventArgs e)
    {
        try
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
        catch(Exception ex)
        {
            DisplayMessage(ex.Message);
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
                string strSheetName = "";
                using (DataTable _dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" }))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        strSheetName = _dt.Rows[0]["TABLE_NAME"].ToString();
                    }
                }

                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + strSheetName + "]", oledbConn);

                //OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds, "poItem");

                List<clsCofExcelImport> lstInvoice = new List<clsCofExcelImport> { };
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        clsCofExcelImport _clsObj = new clsCofExcelImport();
                        _clsObj.loc_name= dr["location"].ToString().Trim();
                        _clsObj.ac_no = dr["account_no"].ToString().Trim();
                        _clsObj.ac_name = dr["account_name"].ToString().Trim();
                        _clsObj.ac_name_l = dr["account_name_l"].ToString().Trim();
                        _clsObj.ac_group_name = dr["account_group"].ToString().Trim();
                        Double ob = 0;
                        Double.TryParse(dr["opening_balance"].ToString().Trim(), out ob);
                        _clsObj.ob = ob;
                        _clsObj.balance_type = dr["balance_type"].ToString().Trim();
                        lstInvoice.Add(_clsObj);
                    }
                }
                oledbConn.Close();

                if (lstInvoice.Count > 0)
                {
                    List<clsCofExcelImport> newInvoiceList = validateExcelData(lstInvoice);
                    if (newInvoiceList.Count > 0)
                    {
                        gvImport.DataSource = newInvoiceList;
                        gvImport.DataBind();
                        hdnTotalExcelRecords.Value = newInvoiceList.Count().ToString();
                        hdnInvalidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == false).Count().ToString();
                        hdnValidExcelRecords.Value = newInvoiceList.Where(m => m.is_valid == true).Count().ToString();
                        Session["ExcelImportCofList"] = newInvoiceList;
                    }
                    else
                    {
                        Session["ExcelImportCofList"] = null;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            hdnInvalidExcelRecords.Value = "0";
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (hdnInvalidExcelRecords.Value != "0")
            {
                btnSaveExcelData.Enabled = false;
            }
            else
            {
                btnSaveExcelData.Enabled = true;
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

    protected List<clsCofExcelImport> validateExcelData(List<clsCofExcelImport> lstCof)
    {
        if (lstCof.Count == 0)
        {
            return null;
        }

        DataTable _dtCof = objCOA.GetCOAAll(Session["CompId"].ToString());
        DataTable _dtGroup = objAccGroup.GetGroupsAll(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataTable _dtLocation = ObjLocation.GetLocationMaster(Session["CompId"].ToString());

        //find duplicate record by account_name
        var lst = lstCof.GroupBy(x => x.ac_name).Where(g=>g.Count()>1)
            .Select(y => y.Key)
            .ToList();
        foreach(var ab in lst)
        {
            foreach (var _cls in lstCof.Where(r => r.ac_name == ab))
            {
                _cls.is_valid = false;
                _cls.validation_remark = "duplicate record";
            }
        }

        //find duplicate record by account_no
        lst = lstCof.GroupBy(x => x.ac_no).Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();
        foreach (var ab in lst)
        {
            foreach (var _cls in lstCof.Where(r => r.ac_no == ab))
            {
                _cls.is_valid = false;
                _cls.validation_remark = "duplicate record";
            }
        }

        foreach (var _cls in lstCof)
        {
            try
            {
                if (!string.IsNullOrEmpty(_cls.validation_remark))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(_cls.loc_name))
                {
                    _cls.loc_id = Session["LocId"].ToString();
                    _cls.currency_id= Session["LocCurrencyId"].ToString();
                }
                else
                {
                    using (DataTable _dtTempLoc = new DataView(_dtLocation, "Location_name='" + _cls.loc_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_dtTempLoc.Rows.Count>0)
                        {
                            _cls.loc_id = _dtTempLoc.Rows[0]["Location_id"].ToString();
                            _cls.currency_id = _dtTempLoc.Rows[0]["field1"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Invalidation Location";
                            continue;
                        }
                    }
                }
                if (string.IsNullOrEmpty(_cls.ac_no))
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Account No";
                    continue;
                }

                if (string.IsNullOrEmpty(_cls.ac_name))
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Account Name";
                    continue;
                }

                if (!string.IsNullOrEmpty(_cls.ac_group_name))
                {
                    using (DataTable _dtTempGroup = new DataView(_dtGroup, "ac_groupName='" + _cls.ac_group_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_dtTempGroup.Rows.Count > 0)
                        {
                            _cls.ac_group_id = _dtTempGroup.Rows[0]["Trans_id"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Account group is not valid";
                            continue;
                        }
                    }
                }
                else
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Group";
                    continue;
                }


                using (DataTable _dtTempAc = new DataView(_dtCof, "AccountName='" + _cls.ac_name + "' or Account_No='" + _cls.ac_no + "'", "", DataViewRowState.CurrentRows).ToTable())
                {
                    if (_dtTempAc.Rows.Count == 1)
                    {
                        _cls.ac_id = _dtTempAc.Rows[0]["Trans_id"].ToString();
                    }
                    else if (_dtTempAc.Rows.Count == 2)
                    {
                        _cls.is_valid = false;
                        _cls.validation_remark = "Duplicate Records";
                        _cls.ac_id = "0";
                    }
                    else
                    {
                        _cls.ac_id = "0";
                    }
                }

                if (_cls.ob<0)
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Opening balance";
                }

                if (_cls.ob > 0)
                {
                    if (string.IsNullOrEmpty(_cls.balance_type))
                    {
                        _cls.is_valid = false;
                        _cls.validation_remark = "Invalid Balance Type";
                    }
                    else
                    {
                        if (_cls.balance_type.ToUpper() == "DR" || _cls.balance_type.ToUpper() == "CR" || _cls.balance_type.ToUpper() == "DEBIT" || _cls.balance_type.ToUpper() == "CREDIT")
                        {

                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Invalid Balance Type";
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

        Session["ExcelImportCofList"] = lstCof;
        return lstCof;

    }


    protected void lnkInvalidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportCofList"] != null)
        {
            List<clsCofExcelImport> newInvoiceList = (List<clsCofExcelImport>)Session["ExcelImportCofList"];
            gvImport.DataSource = newInvoiceList.Where(m => m.is_valid == false).ToList();
            gvImport.DataBind();
        }
       
    }


    protected void lnkValidRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportCofList"] != null)
        {
            List<clsCofExcelImport> newInvoiceList = (List<clsCofExcelImport>)Session["ExcelImportCofList"];
            gvImport.DataSource = newInvoiceList;
            gvImport.DataBind();
        }
    }

    protected void lnkTotalExcelImportRecords_Click(object sender, EventArgs e)
    {
        if (Session["ExcelImportCofList"] != null)
        {
            List<clsCofExcelImport> newInvoiceList = (List<clsCofExcelImport>)Session["ExcelImportCofList"];
            gvImport.DataSource = newInvoiceList;
            gvImport.DataBind();
        }
    }

    protected void btnSaveExcelData_Click(object sender, EventArgs e)
    {
        if (hdnInvalidExcelRecords.Value != "0" || Session["ExcelImportCofList"] == null)
        {
            return;
        }
        List<clsCofExcelImport> lstCofList = (List<clsCofExcelImport>)Session["ExcelImportCofList"];
        if (lstCofList.Count == 0)
        {
            return;
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstCofList)
            {
                if (string.IsNullOrEmpty(_cls.ac_id) || _cls.ac_id == "0")
                {
                    _cls.ac_id = objCOA.InsertCOA(Session["CompId"].ToString(), _cls.ac_no, _cls.ac_name, _cls.ac_name_l, "0", _cls.ac_group_id, "0", "0", Session["LocCurrencyId"].ToString(), "0", "0", "0", "0", "0", false.ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns).ToString();
                }
                else
                {
                    objCOA.UpdateCOA(Session["CompId"].ToString(),_cls.ac_id, _cls.ac_no, _cls.ac_name, _cls.ac_name_l, "0", _cls.ac_group_id, "0", "0", Session["LocCurrencyId"].ToString(), "0", "0", "0", "0", "0", false.ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns).ToString();
                }
                if (bool.Parse(ViewState["idObUpdateAllow"].ToString()) == true)
                {
                    if (_cls.ob>0)
                    {
                        objSubCOA.deleteByAccIdAndFyId(Session["compId"].ToString(), _cls.loc_id, _cls.ac_id, Session["FinanceYearId"].ToString(), ref trns);
                        objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(),_cls.loc_id, _cls.ac_id, "0", _cls.balance_type.ToUpper()=="DR" || _cls.balance_type.ToUpper() == "DEBIT"? _cls.ob.ToString():"0", _cls.balance_type.ToUpper() == "CR" || _cls.balance_type.ToUpper() == "CREDIT" ? _cls.ob.ToString() : "0", _cls.balance_type.ToUpper() == "DR" || _cls.balance_type.ToUpper() == "DEBIT" ? _cls.ob.ToString() : "0", _cls.balance_type.ToUpper() == "CR" || _cls.balance_type.ToUpper() == "CREDIT" ? _cls.ob.ToString() : "0",_cls.currency_id, "0", "0", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }
            trns.Commit();
            con.Close();
            DisplayMessage("Total " + lstCofList.Count + " Records inserted successfully");
            gvImport.DataSource = null;
            gvImport.DataBind();
            btnSaveExcelData.Enabled = false;
            Session["ExcelImportCofList"] = null;
        }
        catch (Exception ex)
        {
            trns.Rollback();
            con.Close();
            DisplayMessage(ex.Message);
        }

    }

    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xls");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void lnkDownloadData_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable _dt = new DataTable())
            {
                _dt.Columns.Add("location");
                _dt.Columns.Add("account_no");
                _dt.Columns.Add("account_name");
                _dt.Columns.Add("account_name_l");
                _dt.Columns.Add("account_group");
                _dt.Columns.Add("opening_balance");
                _dt.Columns.Add("balance_type");
                DataTable _dtAc = objCOA.GetRecordsForExcelDownload(Session["compId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
                if (_dtAc.Rows.Count>0)
                {
                    foreach(DataRow _dr in _dtAc.Rows)
                    {
                        DataRow _drNewRow = _dt.Rows.Add();
                        _drNewRow["location"] = "";
                        _drNewRow["account_no"] = _dr["account_no"].ToString();
                        _drNewRow["account_name"] = _dr["AccountName"].ToString();
                        _drNewRow["account_name_l"] = _dr["AccountNameL"].ToString();
                        _drNewRow["account_group"] = _dr["Ac_GroupName"].ToString();
                        _drNewRow["opening_balance"] = _dr["opening_balance"].ToString();
                        _drNewRow["balance_type"] = _dr["balance_type"].ToString();
                    }
                }
               
               ExportTableData(_dt, "ChartOfAccount");
            }
        }
        catch(Exception ex)
        {

        }
       
    }
}
