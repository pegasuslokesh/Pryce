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

public partial class Bank_BankStatement : System.Web.UI.Page
{
    CompanyMaster ObjCompany = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    Ac_Groups ObjACGroup = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_SubChartOfAccount ObjSubCOA = null;
    CurrencyMaster ObjCurrencyMaster = null;
    RoleDataPermission objRoleData = null;
    EmployeeMaster objEmployee = null;
    Ems_ContactMaster objContact = null;
    DataAccessClass da = null;
    Common cmn = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_Finance_Year_Info objFYI = null;
    DepartmentMaster objDepartment = null;
    Ac_Reconcile_Detail objReconcileDetail = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjACGroup = new Ac_Groups(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objReconcileDetail = new Ac_Reconcile_Detail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Bank/BankStatement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            //AllPageCode(clsPagePermission);

            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "315").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}

            //AllPageCode();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();

            if (strLocationId != "" && strLocationId != "0")
            {
                //txtToLocation.Text = Session["LocName"] + "/" + strLocationId;
            }
            FillLocation();

            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());
            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        SetColorCode();
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    //public void AllPageCode()
    //{
    //    IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
    //    Common ObjComman = new Common();

    //    //New Code created by jitendra on 09-12-2014
    //    string strModuleId = string.Empty;
    //    string strModuleName = string.Empty;

    //    DataTable dtModule = objObjectEntry.GetModuleIdAndName("315");
    //    if (dtModule.Rows.Count > 0)
    //    {
    //        strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
    //        strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
    //    }
    //    else
    //    {
    //        Session.Abandon();
    //        Response.Redirect("~/ERPLogin.aspx");
    //    }

    //    //End Code
    //    Page.Title = objsys.GetSysTitle();
    //    StrUserId = Session["UserId"].ToString();
    //    StrCompId = Session["CompId"].ToString();
    //    StrBrandId = Session["BrandId"].ToString();
    //    strLocationId = Session["LocId"].ToString();
    //    Session["AccordianId"] = strModuleId;
    //    Session["HeaderText"] = strModuleName;
    //    if (Session["EmpId"].ToString() == "0")
    //    {
    //        //btnGetReport.Visible = true;
    //    }

    //    DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "315");
    //    foreach (DataRow DtRow in dtAllPageCode.Rows)
    //    {
    //        if (DtRow["Op_Id"].ToString() == "1")
    //        {
    //            //btnGetReport.Visible = true;
    //        }
    //    }
    //}
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        txtOpenDebitBalance.Text = "0";
        txtCreditBalanceAmount.Text = "0";
        txtCreditBalanceAmountNoReconciled.Text = "0";
        txtTotalBalance.Text = "0";



        TabContainer1.ActiveTabIndex = 0;
        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtFromDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Need to Fill From Date");
            txtFromDate.Focus();
            return;
        }

        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();

            DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
            return;
        }

        //for Account
        txtAccountName_textChnaged(sender, e);

        //for Selected Location
        string strLocationId = string.Empty;
        string strCurrencyId = string.Empty;
        string strCurrencyType = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        //Check Location Currency
        string strCurrencyIdNew = string.Empty;
        string strFlag = "True";
        if (strLocationId != "")
        {
            DataTable dtLocationData = ObjLocation.GetAllLocationMaster();
            if (dtLocationData.Rows.Count > 0)
            {
                dtLocationData = new DataView(dtLocationData, "Location_Id in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int j = 0; j < dtLocationData.Rows.Count; j++)
                {
                    string strPresentCurrency = dtLocationData.Rows[j]["Field1"].ToString();

                    if (strCurrencyIdNew == "")
                    {
                        strCurrencyIdNew = strPresentCurrency;
                    }
                    else if (strCurrencyIdNew != "")
                    {
                        if (strCurrencyIdNew == strPresentCurrency)
                        {

                        }
                        else
                        {
                            strFlag = "False";
                            break;
                        }
                    }
                }
            }
        }

        if (strFlag == "True")
        {
            strCurrencyType = "1";
            string SelectedLocation = string.Empty;
            if (lstLocationSelect.Items.Count > 0)
            {
                SelectedLocation = lstLocationSelect.Items[0].Value.ToString();
            }
            else
            {
                SelectedLocation = Session["LocId"].ToString();
            }

            DataTable dtLocation = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), SelectedLocation);
            if (dtLocation.Rows.Count > 0)
            {
                strCurrencyId = dtLocation.Rows[0]["Field1"].ToString();
            }
        }
        else if (strFlag == "False")
        {
            strCurrencyType = "2";
            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                strCurrencyId = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }

        DateTime dtToDate = new DateTime();
        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name First");
            txtAccountName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            return;
        }

        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
                dtToDate = Convert.ToDateTime(txtToDate.Text);
                dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtToDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Need to Fill To Date");
            txtToDate.Focus();
            return;
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                DisplayMessage("from date should be less then to date ");
                txtFromDate.Focus();
                return;
            }
        }

        if (txtFromDate.Text != "")
        {
            if (txtToDate.Text != "")
            {

            }
            else
            {
                DisplayMessage("To date should be Enter");
                txtToDate.Focus();
                return;
            }
        }

        gvStatementNoReconciled.DataSource = null;
        gvStatementNoReconciled.DataBind();


        //For Bank Account
        string strAccountId = string.Empty;
        string strBankAccount = "True";
        

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        double OpeningBalance = 0;

        //for Opening Credit Balance Start
        if (txtFromDate.Text != "")
        {
            DateTime dtFromdate = Convert.ToDateTime(txtFromDate.Text);
            if (dtFromdate.Day == 1 && dtFromdate.Month == 1)
            {
                dtFromdate = new DateTime(dtFromdate.Year - 1, 12, 31, 23, 59, 1);
            }
            else if (dtFromdate.Day != 1 && dtFromdate.Month == 1)
            {
                dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day - 1, 23, 59, 1);
            }
            else if (dtFromdate.Day != 1 && dtFromdate.Month != 1)
            {
                dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day - 1, 23, 59, 1);
            }
            else if (dtFromdate.Day == 1 && dtFromdate.Month != 1)
            {
                int daysInMonth = DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month - 1);
                dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month - 1, daysInMonth, 23, 59, 1);
            }

            //Get Opening Balance               
            PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
            DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBankBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + txtAccountName.Text.Split('/')[1].ToString() + "','0','" + strCurrencyType + "', '" + Session["FinanceYearId"].ToString() + "')) OpeningBalance");
            if (dtOpeningBalance.Rows.Count > 0)
            {
                OpeningBalance = Convert.ToDouble(dtOpeningBalance.Rows[0][0].ToString());
            }



            //double DueBalance = 0;
            //if (strBankAccount == "True")
            //{

            //    DataTable dtDueBalance = objDA.return_DataTable("select sum(debit_amount-credit_amount) as DueBalance from ac_voucher_detail  inner join Ac_Voucher_Header  on Ac_Voucher_Header.Trans_Id=ac_voucher_detail.Voucher_No  where Account_No='" + txtAccountName.Text.Split('/')[1].ToString() + "'  and ac_voucher_header.ReconciledFromFinance='True'   AND (Ac_Voucher_Header.Field3 = ''    OR Ac_Voucher_Header.Field3 IS NULL    OR Ac_Voucher_Header.Field3 = 'Approved')  and voucher_date <'" + txtFromDate.Text + "'   and ac_voucher_header.isactive='true'   and (Ac_Voucher_Detail.Field2='' or Ac_Voucher_Detail.Field2='false')  and ac_voucher_header.Location_Id in (" + strLocationId + ")");
            //    if (dtDueBalance.Rows.Count > 0 && dtDueBalance != null)
            //    {
            //        double.TryParse(dtDueBalance.Rows[0]["DueBalance"].ToString(), out DueBalance);
            //        //DueBalance = Convert.ToDouble(dtDueBalance.Rows[0]["DueBalance"].ToString());
            //        if (DueBalance != 0)
            //        {
            //            OpeningBalance = (OpeningBalance - DueBalance);
            //        }
            //    }
            //}


            if (OpeningBalance < 0)
            {
                //Credit Opening
                string COB = OpeningBalance.ToString();
                txtOpenCreditBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, COB);
                Div_Credit.Visible = true;
                lblOpeningCreditBalance.Visible = true;                
                txtOpenCreditBalance.Visible = true;
                txtOpenDebitBalance.Text = "";
                Div_Debit.Visible = false;
                lblOpeningDebitBalance.Visible = false;
                txtOpenDebitBalance.Visible = false;
                Lbl_Mid_Debit.Visible = false;
            }
            else
            {
                //Debit Opening
                string DOB = OpeningBalance.ToString();
                txtOpenDebitBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, DOB);
                Div_Credit.Visible = false;
                lblOpeningCreditBalance.Visible = false;                
                txtOpenCreditBalance.Visible = false;
                txtOpenCreditBalance.Text = "";
                Div_Debit.Visible = true;
                lblOpeningDebitBalance.Visible = true;
                txtOpenDebitBalance.Visible = true;
                Lbl_Mid_Debit.Visible = true;
            }
        }
        //End




        //financne start date for current company


        DataTable dt = objFYI.GetInfoAllTrue(Session["CompId"].ToString());

        DateTime dtFromDate = Convert.ToDateTime(new DataView(dt, "", "Trans_id", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());

        DataTable dtStatementNotReconciled = objVoucherDetail.GetAllStatementBankData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtAccountName.Text.Split('/')[1].ToString(), "0", dtFromDate.ToString(), txtToDate.Text, strCurrencyType,"2");


        DataTable dtStatement = objVoucherDetail.GetAllStatementBankData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtAccountName.Text.Split('/')[1].ToString(), "0", txtFromDate.Text, txtToDate.Text, strCurrencyType,"1");
        if (dtStatement.Rows.Count > 0)
        {

            GetNotReconciled(dtStatementNotReconciled);


            dtStatement = new DataView(dtStatement, "BankReconcilation='True'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();


            if (dtStatement.Rows.Count > 0)
            {
                if (ddlSVoucherType.SelectedValue != "--Select--")
                {
                    dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                    if (dtStatement.Rows.Count > 0)
                    {
                        GVSStatement.DataSource = dtStatement;
                        GVSStatement.DataBind();

                    }
                }
                else
                {
                    GVSStatement.DataSource = dtStatement;
                    GVSStatement.DataBind();
                }
            }
            else
            {
                GVSStatement.DataSource = null;
                GVSStatement.DataBind();
            }


            //Regrading Balance & Total
            //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            //string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, Session["SupplierCurrency"].ToString());
            string strgvStatus = "False";
            string strgvBalanceA = string.Empty;

            double gvdebitamount = 0;
            double gvcreditamount = 0;
            double gvdebitTotalamount = 0;
            double gvcreditTotalamount = 0;
            SetColorCode();
            foreach (GridViewRow gvr in GVSStatement.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditAmount.Text);
                gvdebitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                gvcreditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");

                if (hdngvDetailId.Value != "" && hdngvDetailId.Value != "0")
                {
                    DataTable dtRDetail = objReconcileDetail.GetReconcileDetailAllDataOnly();
                    if (dtRDetail != null)
                    {
                        if (dtRDetail.Rows.Count > 0)
                        {
                            dtRDetail = new DataView(dtRDetail, "VD_Id='" + hdngvDetailId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtRDetail.Rows.Count > 0)
                            {
                                string status = dtRDetail.Rows[0]["Is_Reconciled"].ToString();
                                if (status == "True")
                                {
                                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtReconciled.Text);
                                }
                                else if (status == "False")
                                {
                                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtConflicted.Text);
                                }
                            }
                            else
                            {
                                gvr.BackColor = System.Drawing.Color.FromName("#" + txtNotReconciled.Text);
                            }
                        }
                    }
                }

                //gvr.BackColor = System.Drawing.Color.Cyan;

                if (strgvStatus == "False")
                {
                    if (txtOpenDebitBalance.Visible == true)
                    {
                        lblgvBalance.Text = txtOpenDebitBalance.Text;
                        strgvStatus = "True";
                    }
                    else if (txtOpenCreditBalance.Visible == true)
                    {
                        lblgvBalance.Text = txtOpenCreditBalance.Text;

                        if (txtFromDate.Text != "")
                        {
                            if (dtStatement.Rows.Count > 0)
                            {
                                if (gvdebitamount != 0)
                                {
                                    lblgvBalance.Text = (Convert.ToDouble(gvdebitamount.ToString()) + Convert.ToDouble(txtOpenCreditBalance.Text)).ToString();
                                    strgvBalanceA = lblgvBalance.Text;
                                }
                                else if (gvcreditamount != 0)
                                {
                                    lblgvBalance.Text = (Convert.ToDouble(txtOpenCreditBalance.Text) - Convert.ToDouble(gvcreditamount.ToString())).ToString();
                                    strgvBalanceA = lblgvBalance.Text;
                                }
                            }
                            else
                            {
                                if (hdnNOAId.Value == "1" || hdnNOAId.Value == "3")
                                {
                                    string OpenCredit = "-" + txtOpenCreditBalance.Text;
                                    if (gvdebitamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(gvdebitamount.ToString()) - Convert.ToDouble(txtOpenCreditBalance.Text)).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                    else if (gvcreditamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble("-" + gvcreditamount.ToString()) + Convert.ToDouble(OpenCredit)).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                }
                                else if (hdnNOAId.Value == "2" || hdnNOAId.Value == "4")
                                {
                                    if (gvdebitamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(txtOpenCreditBalance.Text) + Convert.ToDouble(gvdebitamount.ToString())).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                    else if (gvcreditamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(gvcreditamount.ToString()) - Convert.ToDouble(txtOpenCreditBalance.Text)).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblgvBalance.Text = "0";
                        strgvStatus = "True";
                    }
                }
                else
                {
                    lblgvBalance.Text = strgvBalanceA;
                }

                if (hdnNOAId.Value == "1" || hdnNOAId.Value == "3")
                {
                    if (strgvStatus == "False")
                    {
                        strgvStatus = "True";
                    }
                    else
                    {
                        if (gvdebitamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(gvdebitamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                        else if (gvcreditamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(gvcreditamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                    }
                }
                else if (hdnNOAId.Value == "2" || hdnNOAId.Value == "4")
                {
                    if (strgvStatus == "False")
                    {
                        strgvStatus = "True";
                    }
                    else
                    {
                        if (gvdebitamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(gvdebitamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                        else if (gvcreditamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(gvcreditamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                    }
                }

                gvdebitTotalamount += gvdebitamount;
                gvcreditTotalamount += gvcreditamount;
                txtClosingCreditBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, strgvBalanceA);
                txtCreditBalanceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, strgvBalanceA);
            }

            if (GVSStatement.Rows.Count > 0)
            {
                Label lblgvDebitTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvDebitTotal");
                Label lblgvCreditTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvCreditTotal");
                Label lblgvBalanceTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvBalanceTotal");

                lblgvDebitTotal.Text = gvdebitTotalamount.ToString();
                lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitTotal.Text);
                lblgvCreditTotal.Text = gvcreditTotalamount.ToString();
                lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditTotal.Text);
                lblgvBalanceTotal.Text = strgvBalanceA;
                lblgvBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalanceTotal.Text);
                GVSStatement.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVSStatement.HeaderRow.Cells[7].Text, Session["DBConnection"].ToString());
            }
            else
            {
                //DisplayMessage("You have no record available");
                GVSStatement.DataSource = null;
                GVSStatement.DataBind();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            //DisplayMessage("You have no record available");
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
            if (txtOpenCreditBalance.Text != "")
            {
                txtCreditBalanceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtOpenCreditBalance.Text);
            }
            else
            {
                txtCreditBalanceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtOpenDebitBalance.Text);
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }



        txtTotalBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, (Convert.ToDouble(txtCreditBalanceAmount.Text) + Convert.ToDouble(txtCreditBalanceAmountNoReconciled.Text)).ToString());

    }

    public void GetNotReconciled(DataTable dtStatement)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        if (dtStatement.Rows.Count > 0)
        {

            //dtStatement = new DataView(dtStatement, "BankReconcilation='False' or BankReconcilation=' '", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();


            if (dtStatement.Rows.Count > 0)
            {
                if (ddlSVoucherType.SelectedValue != "--Select--")
                {
                    dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                    if (dtStatement.Rows.Count > 0)
                    {
                        gvStatementNoReconciled.DataSource = dtStatement;
                        gvStatementNoReconciled.DataBind();

                    }
                }
                else
                {
                    gvStatementNoReconciled.DataSource = dtStatement;
                    gvStatementNoReconciled.DataBind();
                }
            }
            else
            {
                gvStatementNoReconciled.DataSource = null;
                gvStatementNoReconciled.DataBind();
            }


            //Regrading Balance & Total
            //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            //string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, Session["SupplierCurrency"].ToString());
            string strgvStatus = "False";
            string strgvBalanceA = string.Empty;

            double gvdebitamount = 0;
            double gvcreditamount = 0;
            double gvdebitTotalamount = 0;
            double gvcreditTotalamount = 0;
            SetColorCode();
            foreach (GridViewRow gvr in gvStatementNoReconciled.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objsys.GetCurencyConversionForFinance(strCurrency, lblgvCreditAmount.Text);
                gvdebitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                gvcreditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");

                if (hdngvDetailId.Value != "" && hdngvDetailId.Value != "0")
                {
                    DataTable dtRDetail = objReconcileDetail.GetReconcileDetailAllDataOnly();
                    if (dtRDetail != null)
                    {
                        if (dtRDetail.Rows.Count > 0)
                        {
                            dtRDetail = new DataView(dtRDetail, "VD_Id='" + hdngvDetailId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtRDetail.Rows.Count > 0)
                            {
                                string status = dtRDetail.Rows[0]["Is_Reconciled"].ToString();
                                if (status == "True")
                                {
                                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtReconciled.Text);
                                }
                                else if (status == "False")
                                {
                                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtConflicted.Text);
                                }
                            }
                            else
                            {
                                gvr.BackColor = System.Drawing.Color.FromName("#" + txtNotReconciled.Text);
                            }
                        }
                    }
                }

                //gvr.BackColor = System.Drawing.Color.Cyan;

                if (strgvStatus == "False")
                {
                    if (txtOpenDebitBalance.Visible == true)
                    {
                        lblgvBalance.Text = "0";
                        strgvStatus = "True";
                    }
                    else if (txtOpenCreditBalance.Visible == true)
                    {
                        lblgvBalance.Text = txtOpenCreditBalance.Text;

                        if (txtFromDate.Text != "")
                        {
                            if (dtStatement.Rows.Count > 0)
                            {
                                if (gvdebitamount != 0)
                                {
                                    lblgvBalance.Text = Convert.ToDouble(gvdebitamount.ToString()).ToString();
                                    strgvBalanceA = lblgvBalance.Text;
                                }
                                else if (gvcreditamount != 0)
                                {
                                    lblgvBalance.Text = (Convert.ToDouble(gvcreditamount.ToString())).ToString();
                                    strgvBalanceA = lblgvBalance.Text;
                                }
                            }
                            else
                            {
                                if (hdnNOAId.Value == "1" || hdnNOAId.Value == "3")
                                {
                                    string OpenCredit = "-" + txtOpenCreditBalance.Text;
                                    if (gvdebitamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(gvdebitamount.ToString())).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                    else if (gvcreditamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble("-" + gvcreditamount.ToString())).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                }
                                else if (hdnNOAId.Value == "2" || hdnNOAId.Value == "4")
                                {
                                    if (gvdebitamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(gvdebitamount.ToString())).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                    else if (gvcreditamount != 0)
                                    {
                                        lblgvBalance.Text = (Convert.ToDouble(gvcreditamount.ToString())).ToString();
                                        strgvBalanceA = lblgvBalance.Text;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblgvBalance.Text = "0";
                        strgvStatus = "True";
                    }
                }
                else
                {
                    lblgvBalance.Text = strgvBalanceA;
                }

                if (hdnNOAId.Value == "1" || hdnNOAId.Value == "3")
                {
                    if (strgvStatus == "False")
                    {
                        strgvStatus = "True";
                    }
                    else
                    {
                        if (gvdebitamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(gvdebitamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                        else if (gvcreditamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(gvcreditamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                    }
                }
                else if (hdnNOAId.Value == "2" || hdnNOAId.Value == "4")
                {
                    if (strgvStatus == "False")
                    {
                        strgvStatus = "True";
                    }
                    else
                    {
                        if (gvdebitamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(gvdebitamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                        else if (gvcreditamount != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(gvcreditamount.ToString())).ToString();
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                            strgvBalanceA = lblgvBalance.Text;
                        }
                    }
                }

                gvdebitTotalamount += gvdebitamount;
                gvcreditTotalamount += gvcreditamount;
                txtCreditBalanceAmountNoReconciled.Text = objsys.GetCurencyConversionForInv(strCurrency, strgvBalanceA);
                //txtCreditBalanceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, strgvBalanceA);
            }

            if (gvStatementNoReconciled.Rows.Count > 0)
            {
                Label lblgvDebitTotal = (Label)gvStatementNoReconciled.FooterRow.FindControl("lblgvDebitTotal");
                Label lblgvCreditTotal = (Label)gvStatementNoReconciled.FooterRow.FindControl("lblgvCreditTotal");
                Label lblgvBalanceTotal = (Label)gvStatementNoReconciled.FooterRow.FindControl("lblgvBalanceTotal");

                lblgvDebitTotal.Text = gvdebitTotalamount.ToString();
                lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitTotal.Text);
                lblgvCreditTotal.Text = gvcreditTotalamount.ToString();
                lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditTotal.Text);
                lblgvBalanceTotal.Text = strgvBalanceA;
                lblgvBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalanceTotal.Text);
                gvStatementNoReconciled.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(), gvStatementNoReconciled.HeaderRow.Cells[7].Text, Session["DBConnection"].ToString());
            }
            else
            {
                //DisplayMessage("You have no record available");
                gvStatementNoReconciled.DataSource = null;
                gvStatementNoReconciled.DataBind();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            //DisplayMessage("You have no record available");
            gvStatementNoReconciled.DataSource = null;
            gvStatementNoReconciled.DataBind();

            txtCreditBalanceAmountNoReconciled.Text = "0";

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }
    }
    protected void txtAccountName_textChnaged(object sender, EventArgs e)
    {
        DataTable dtCOA = new DataTable();
        if (txtAccountName.Text != "")
        {
            try
            {
                txtAccountName.Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + txtAccountName.Text.Split('/')[0].ToString() + "' and IsActive='True'";
                dtCOA = da.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }

        if (txtAccountName.Text != "")
        {
            if (dtCOA.Rows.Count > 0)
            {
                DataTable dtCOAOpening = objCOA.GetCOAByTransId(StrCompId, txtAccountName.Text.Split('/')[1].ToString());
                if (dtCOAOpening.Rows.Count > 0)
                {
                    //OpeningCredit = Convert.ToDouble(dtCOAOpening.Rows[0]["O_Cr_Bal"].ToString());
                    //OpeningDebit = Convert.ToDouble(dtCOAOpening.Rows[0]["O_Dr_Bal"].ToString());
                    string strGroupId = dtCOAOpening.Rows[0]["Acc_Group_Id"].ToString();
                    if (strGroupId != "")
                    {
                        DataTable dtGroupDetail = ObjACGroup.GetGroupsByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strGroupId);
                        if (dtGroupDetail.Rows.Count > 0)
                        {
                            hdnNOAId.Value = dtGroupDetail.Rows[0]["N_Group_ID"].ToString();
                        }
                    }
                }
                else
                {
                    txtOpenCreditBalance.Text = "0.00";
                }
            }
        }
        else
        {
            txtOpenCreditBalance.Text = "";
            txtOpenDebitBalance.Text = "";
            FillLocation();
            txtClosingCreditBalance.Text = "";
            txtCreditBalanceAmount.Text = "";
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void SetColorCode()
    {
        //ForColoCodes
        DataTable dtReconciled = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ReconciledColorCode");
        if (dtReconciled.Rows.Count > 0)
        {
            txtReconciled.Text = dtReconciled.Rows[0]["Param_Value"].ToString();
            txtReconciled.ForeColor = System.Drawing.Color.FromName("#" + txtReconciled.Text);
        }
        else
        {
            txtReconciled.Text = "33FFCC";
            txtReconciled.ForeColor = System.Drawing.Color.FromName("#33FFCC");
        }

        DataTable dtConflicted = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ConflictedColorCode");
        if (dtConflicted.Rows.Count > 0)
        {
            txtConflicted.Text = dtConflicted.Rows[0]["Param_Value"].ToString();
            txtConflicted.ForeColor = System.Drawing.Color.FromName("#" + txtConflicted.Text);
        }
        else
        {
            txtConflicted.Text = "66FF66";
            txtConflicted.ForeColor = System.Drawing.Color.FromName("#66FF66");
        }

        DataTable dtNotReconciled = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "NotReconciledColorCode");
        if (dtNotReconciled.Rows.Count > 0)
        {
            txtNotReconciled.Text = dtNotReconciled.Rows[0]["Param_Value"].ToString();
            txtNotReconciled.ForeColor = System.Drawing.Color.FromName("#" + txtNotReconciled.Text);
        }
        else
        {
            txtNotReconciled.Text = "FF33FF";
            txtNotReconciled.ForeColor = System.Drawing.Color.FromName("#FF33FF");
        }
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        DataTable dtCurr = ObjCurrencyMaster.GetCurrencyMasterById(strCurrencyId);
        if (dtCurr.Rows.Count > 0)
        {
            strCurrencyName = dtCurr.Rows[0]["Currency_Name"].ToString();
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtAccountName.Text = "";

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtOpenCreditBalance.Text = "";
        txtClosingCreditBalance.Text = "";
        txtCreditBalanceAmount.Text = "";

        GVSStatement.DataSource = null;
        GVSStatement.DataBind();

        Div_Debit.Visible = false;
        lblOpeningDebitBalance.Visible = false;
        txtOpenDebitBalance.Visible = false;
        Lbl_Mid_Debit.Visible = false;
        txtOpenDebitBalance.Text = "0.00";
        Div_Credit.Visible = false;
        lblOpeningCreditBalance.Visible = false;
        txtOpenCreditBalance.Visible = false;
        txtOpenCreditBalance.Text = "0.00";
        lblMid.Visible = false;
        hdnNOAId.Value = "0";
        SetColorCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocation(string prefixText, int count, string contextKey)
    {
        LocationMaster objLoc = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Location_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
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
                dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_Parameter_Location objAccParameterLocation = new Ac_Parameter_Location(HttpContext.Current.Session["DBConnection"].ToString());

        string strAccountId = string.Empty;
        string strBankAccount = "False";
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "BankAccount");
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
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Trans_id in (" + strAccountId + ")";
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

    

    #region Print
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        string strCurrencyType = string.Empty;
        string strCurrencyId = string.Empty;

        //for Selected Location
        string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        //Check Location Currency
        string strCurrencyIdNew = string.Empty;
        string strFlag = "True";
        if (strLocationId != "")
        {
            DataTable dtLocationData = ObjLocation.GetAllLocationMaster();
            if (dtLocationData.Rows.Count > 0)
            {
                dtLocationData = new DataView(dtLocationData, "Location_Id in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int j = 0; j < dtLocationData.Rows.Count; j++)
                {
                    string strPresentCurrency = dtLocationData.Rows[j]["Field1"].ToString();

                    if (strCurrencyIdNew == "")
                    {
                        strCurrencyIdNew = strPresentCurrency;
                    }
                    else if (strCurrencyIdNew != "")
                    {
                        if (strCurrencyIdNew == strPresentCurrency)
                        {

                        }
                        else
                        {
                            strFlag = "False";
                            break;
                        }
                    }
                }
            }
        }

        if (strFlag == "True")
        {
            strCurrencyType = "1";
            string SelectedLocation = string.Empty;
            if (lstLocationSelect.Items.Count > 0)
            {
                SelectedLocation = lstLocationSelect.Items[0].Value.ToString();
            }
            else
            {
                SelectedLocation = Session["LocId"].ToString();
            }

            DataTable dtLocation = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), SelectedLocation);
            if (dtLocation.Rows.Count > 0)
            {
                strCurrencyId = dtLocation.Rows[0]["Field1"].ToString();
            }
        }
        else if (strFlag == "False")
        {
            strCurrencyType = "2";
            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                strCurrencyId = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }

        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name First");
            txtAccountName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            return;
        }
        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtFromDate.Focus();
                return;
            }
        }
        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);

            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtToDate.Focus();
                return;
            }
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                DisplayMessage("from date should be less then to date ");
                txtFromDate.Focus();
                return;
            }
        }
        if (txtFromDate.Text != "")
        {
            if (txtToDate.Text != "")
            {

            }
            else
            {
                DisplayMessage("To date should be Enter");
                txtToDate.Focus();
                return;
            }
        }

        btnGetReport_Click(null, null);
        string strCD = string.Empty;
        ArrayList objArr = new ArrayList();
        objArr.Add(txtAccountName.Text.Split('/')[1].ToString());
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strLocationId);
        if (txtOpenDebitBalance.Visible == true && txtOpenCreditBalance.Visible == false)
        {
            objArr.Add(txtOpenDebitBalance.Text);
        }
        else if (txtOpenCreditBalance.Visible == true && txtOpenDebitBalance.Visible == false)
        {
            objArr.Add(txtOpenCreditBalance.Text);
        }
        else
        {
            objArr.Add("0.00");
        }

        objArr.Add(ddlSVoucherType.SelectedValue);
        objArr.Add(strCurrencyType);
        objArr.Add(strCurrencyId);
        if (txtOpenDebitBalance.Visible == true && txtOpenCreditBalance.Visible == false)
        {
            strCD = "Debit";
            objArr.Add(strCD);
        }
        else if (txtOpenCreditBalance.Visible == true && txtOpenDebitBalance.Visible == false)
        {
            strCD = "Credit";
            objArr.Add(strCD);
        }

        Session["dtAcParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AccountStatement.aspx?Type=Bank','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();

        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();

        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();

        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }

    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();


    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();

    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();

    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();

    }
    #endregion

    #region ViewSection
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
    protected string GetCustomerNameByContactId(string strContactId)
    {
        string strCustomerName = string.Empty;
        if (strContactId != "0" && strContactId != "")
        {
            DataTable dtAccName = objContact.GetContactTrueById(strContactId);
            if (dtAccName.Rows.Count > 0)
            {
                strCustomerName = dtAccName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strCustomerName = "";
        }
        return strCustomerName;
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
    protected void rbCashPayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rbCashPayment.Checked == true)
        {
            trCheque1.Visible = false;
            trCheque2.Visible = false;
            txtChequeIssueDate.Text = "";
            txtChequeClearDate.Text = "";
            txtChequeNo.Text = "";
        }
        else if (rbChequePayment.Checked == true)
        {
            trCheque1.Visible = true;
            trCheque2.Visible = true;
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            string strFinanceCode = dtVoucherEdit.Rows[0]["Finance_Code"].ToString();
            if (strFinanceCode != "0" && strFinanceCode != "")
            {
                DataTable dtFy = objFYI.GetInfoByTransId(StrCompId, strFinanceCode);
                if (dtFy.Rows.Count > 0)
                {
                    txtFinanceCode.Text = dtFy.Rows[0]["Finance_Code"].ToString() + "/" + dtFy.Rows[0]["Trans_Id"].ToString();
                }
            }

            string strToLocationId = dtVoucherEdit.Rows[0]["Location_To"].ToString();
            if (strToLocationId != "0" && strToLocationId != "")
            {
                txtToLocation.Text = GetLocationName(strToLocationId) + "/" + strToLocationId;
            }
            else
            {
                txtToLocation.Text = "";
            }

            string strDepartmentId = dtVoucherEdit.Rows[0]["Department_Id"].ToString();
            if (strDepartmentId != "0" && strDepartmentId != "")
            {
                txtDepartment.Text = GetDepartmentName(strDepartmentId) + "/" + strDepartmentId;
            }
            else
            {
                txtDepartment.Text = "";
            }

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

            strVoucherType = dtVoucherEdit.Rows[0]["Voucher_Type"].ToString();
            if (strVoucherType != "0")
            {
                ddlVoucherType.SelectedValue = strVoucherType;
                ddlVoucherType.Enabled = false;
            }
            else
            {
                ddlVoucherType.SelectedValue = "--Select--";
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
            //ddlCurrency.SelectedValue = strCurrencyId;

            //txtExchangeRate.Text = dtVoucherEdit.Rows[0]["Exchange_Rate"].ToString();
            //txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            //Add Child Concept
            GvDetail.DataSource = null;
            GvDetail.DataBind();

            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            if (dtDetail.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

                //For Total
                double sumDebit = 0;
                double sumCredit = 0;
                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
                    Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                    Label lblgvExchangerate = (Label)gvr.FindControl("lblgvExchangeRate");

                    if (lblgvDebitAmt.Text == "")
                    {
                        lblgvDebitAmt.Text = "0";
                    }
                    sumDebit += Convert.ToDouble(lblgvDebitAmt.Text);

                    if (lblgvCreditAmt.Text == "")
                    {
                        lblgvCreditAmt.Text = "0";
                    }
                    sumCredit += Convert.ToDouble(lblgvCreditAmt.Text);

                    lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
                    lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
                    lblgvFrgnAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
                    lblgvExchangerate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangerate.Text);
                }

                Label lblDebitTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvDebitTotal"));
                Label lblCreditTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvCreditTotal"));

                lblDebitTotal.Text = sumDebit.ToString();
                lblCreditTotal.Text = sumCredit.ToString();
                lblDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDebitTotal.Text);
                lblCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblCreditTotal.Text);
            }
        }

        //pnl1.Visible = true;
        //pnl2.Visible = true;


        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Popup_Popup()", true);
    }

    protected void btnUpdateReconcileDate_Command(object sender, CommandEventArgs e)
    {
        string[] cmdArgue = e.CommandArgument.ToString().Split(';');


        hdnVoucherDetailId.Value = cmdArgue[0].ToString();

        if (cmdArgue[1].ToString() == "")
        {
            button_delete_reconcile.Visible = false;
            txtReconcileDate.Text = GetDate(DateTime.Now.ToString()).ToString();
        }
        else
        {
            button_delete_reconcile.Visible = true;
            txtReconcileDate.Text = cmdArgue[1].ToString();
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_Popup()", true);
    }


    protected void Btn_Update_Reconcile_Click_1(object sender, EventArgs e)
    {
        DateTime? strReconcileDate;
        strReconcileDate = null;
        //DateTime.TryParse(txtReconcileDate.Text, out strReconcileDate);
        try
        {
            strReconcileDate = DateTime.Parse(txtReconcileDate.Text);
        }
        catch
        {

        }
        if (strReconcileDate == null)
        {
            DisplayMessage("Please enter valid date");
            return;
        }

        if (hdnVoucherDetailId.Value != "")
        {
            //Update Reconcile Voucher Detail
            objVoucherDetail.UpdateVoucherDetailReconcile(hdnVoucherDetailId.Value.ToString(), strReconcileDate == null ? "False" : "True", Session["UserId"].ToString(),strReconcileDate.ToString());
        }
        
        btnGetReport_Click(sender, e);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_Popup()", true);
    }

    protected void Btn_delete_Reconcile_Click(object sender, EventArgs e)
    {
        if (hdnVoucherDetailId.Value != "")
        {

            //Update Reconcile Voucher Detail
            string sql = "Update Ac_Voucher_Detail SET [Field2]='false',[ModifiedBy] = '" + Session["UserId"].ToString() + "',[bankReconcileDate] = null,[ModifiedDate] = '" + DateTime.Now.ToString("dd/MMM/yyyy") + "' Where [Trans_Id] = " + hdnVoucherDetailId.Value;
            da.execute_Command(sql);
            //objVoucherDetail.UpdateVoucherDetailReconcile(hdnVoucherDetailId.Value.ToString(), strReconcileDate == null ? "False" : "True", Session["UserId"].ToString(), strReconcileDate.ToString());
        }

        btnGetReport_Click(sender, e);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_Popup()", true);
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
    protected string GetDepartmentName(string strDepartmentId)
    {
        string strDepartmentName = string.Empty;
        if (strDepartmentId != "0" && strDepartmentId != "")
        {
            DataTable dtDepartmentName = objDepartment.GetDepartmentMasterById(strDepartmentId);
            if (dtDepartmentName.Rows.Count > 0)
            {
                strDepartmentName = dtDepartmentName.Rows[0]["Dep_Name"].ToString();
            }
        }
        else
        {
            strDepartmentName = "";
        }
        return strDepartmentName;
    }
    protected void btnCancelPopLeave_Click(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        //Reset();
    }
    #endregion

    protected void Btn_Update_Reconcile_Click(object sender, EventArgs e)
    {

    }
}