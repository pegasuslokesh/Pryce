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
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class AccountSetup_ClosingFYearLocation : System.Web.UI.Page
{
    PegasusDataAccess.DataAccessClass objDA = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_FinancialYear_Detail objFYIDetail = null;
    Ac_FinancialYear_Closing_Detail objFYIClosingDetail = null;
    Ac_ChartOfAccount ObjCOA = null;
    Ac_SubChartOfAccount objSubCOA = null;
    SystemParameter ObjSysParam = null;
    LocationMaster ObjLocation = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_CashFlow_Header objCashFlowHeader = null;
    Ac_ParameterMaster objAccParameter = null;
    Set_CustomerMaster objCustomerMaster = null;
    Set_Suppliers objSupplierMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    Ac_AccountMaster objAcAccountMaster = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    double liabilitiesTotal = 0;
    double AssestTotal = 0;

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

        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objFYIDetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        objFYIClosingDetail = new Ac_FinancialYear_Closing_Detail(Session["DBConnection"].ToString());
        ObjCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objCashFlowHeader = new Ac_CashFlow_Header(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objCustomerMaster = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());

            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "346", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            DataTable dtFinance = objFYI.GetInfoByTransId(StrCompId, Session["FinanceYearId"].ToString());
            if (dtFinance.Rows.Count > 0)
            {
                txtFinanceCode.Text = dtFinance.Rows[0]["Finance_Code"].ToString();
                txtFromDate.Text = DateTime.Parse(dtFinance.Rows[0]["From_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtToDate.Text = DateTime.Parse(dtFinance.Rows[0]["To_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
        }
    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("344", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        Page.Title = ObjSysParam.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


        if (Session["EmpId"].ToString() == "0")
        {
            btnExceute.Visible = true;
            btnCloseYear.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "344",HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            foreach (DataRow DtRow in dtAllPageCode.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "1")
                {
                    btnExceute.Visible = true;
                    btnCloseYear.Visible = true;
                }
            }
        }
        //End Code
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnExceute_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "")
        {
            DisplayMessage("You Require From Date");
            txtFromDate.Focus();
            return;
        }

        if (txtToDate.Text == "")
        {
            DisplayMessage("You Require To Date");
            txtFromDate.Focus();
            return;
        }

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtFromDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string stNatureIds = "1,2";
        string stNatureIdsPandL = "3,4";

        //for Selected Location
        strLocationId = Session["LocId"].ToString();

        //For Account Information
        string strReceiveVoucher = string.Empty;
        string strPaymentVoucherAcc = string.Empty;
        string strNatureOfAccountIncome = string.Empty;
        string strNatureOfAccountExpenses = string.Empty;
        string strNatureOfAccountLiabilities = string.Empty;
        string strNatureOfAccountAssets = string.Empty;

        strNatureOfAccountIncome = "4";
        strNatureOfAccountExpenses = "3";
        strNatureOfAccountLiabilities = "2";
        strNatureOfAccountAssets = "1";


        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucher = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }



        string strCapitalAccount = "Capital Account";
        //DataTable dtCapitalVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "CapitalAccount");
        //if (dtCapitalVoucher.Rows.Count > 0)
        //{
        //    strCapitalAccount = "Capital Account";
        //}
        //else
        //{
        //    strCapitalAccount = "0";
        //}
        //End


        if (txtToDate.Text != "")
        {
            if (stNatureIds != "" && stNatureIds != "0")
            {
                PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                //DataTable dtTrailBalance = objDA.return_DataTable("select *, Cast((case when cb>0 then cb else 0 end) as Varchar) as Debit, Cast((case when cb<0 then abs(cb) else 0 end) as Varchar) as Credit   from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIds + "',0,'" + txtToDate.Text + "','" + strCurrencyType + "','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0)ab");
                DataTable dtBalance = objDA.return_DataTable("select nature_of_account,parent_id,account_no,name,cb as cb_actual, other_account_no,cast(abs(cb) as nvarchar)as cb,cast(cb_type as nvarchar)as cb_type,foreign_cb,company_cb from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIds + "',0,'" + txtToDate.Text + "','" + txtToDate.Text + "','1','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0,'" + Session["FinanceYearId"].ToString() + "')ab");

                //fillIncomeGrid();
                //fillExpensesGrid;

                double TotalIncome = 0;
                double TotalExpenses = 0;
                double profit = 0;
                profit = Convert.ToDouble(objDA.get_SingleValue("select abs(income)-abs(expenses) from (select sum(case when nature_of_account=3 and account_no>0  then cb else 0 end)as expenses,sum(case when nature_of_account=4 and account_no>0 then cb else 0 end)as income from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIdsPandL + "',0,'" + txtToDate.Text + "','" + txtToDate.Text + "','1','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0,'" + Session["FinanceYearId"].ToString() + "')ab)tmp"));

                DataTable dtOpStock = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId.ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',3)");
                string strOpeningStock = ObjSysParam.GetCurencyConversionForInv(strCurrency, dtOpStock.Rows[0][0].ToString());

                if (strOpeningStock == "")
                {
                    strOpeningStock = "0";
                }

                DataTable dtClosingStock1 = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId.ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',1)");
                string strClosingStock1 = ObjSysParam.GetCurencyConversionForInv(strCurrency, dtClosingStock1.Rows[0][0].ToString());


                if (strClosingStock1 == "")
                {
                    strClosingStock1 = "0";
                }
                profit = profit - Convert.ToDouble(strOpeningStock) + Convert.ToDouble(strClosingStock1);

                profit = Convert.ToDouble(ObjSysParam.GetCurencyConversionForInv(strCurrency, profit.ToString()));

                foreach (DataColumn dc in dtBalance.Columns)
                {
                    dtBalance.Columns[dc.ColumnName.ToString()].ReadOnly = false;

                    if (dc.ColumnName.ToString() == "name")
                    {
                        dtBalance.Columns["name"].MaxLength = 1000;
                    }
                }

                if (dtBalance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBalance.Rows)
                    {
                        dt["cb"] = ObjSysParam.GetCurencyConversionForInv(strCurrency, dt["cb"].ToString());
                        //dt["Credit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Credit"].ToString());
                        dt["name"] = dt["name"].ToString().Replace(" ", "&nbsp;");

                        if (dt["nature_of_account"].ToString() == strNatureOfAccountLiabilities.ToString() && dt["account_no"].ToString() != "0")
                        {
                            //TotalIncome += Convert.ToDouble(dt["cb_actual"]);
                            TotalIncome += Convert.ToDouble(ObjSysParam.GetCurencyConversionForInv(strCurrency, dt["cb_actual"].ToString()));
                        }

                        if (dt["nature_of_account"].ToString() == strNatureOfAccountAssets.ToString() && dt["account_no"].ToString() != "0")
                        {
                            //TotalExpenses += Convert.ToDouble(dt["cb_actual"]);
                            TotalExpenses += Convert.ToDouble(ObjSysParam.GetCurencyConversionForInv(strCurrency, dt["cb_actual"].ToString()));
                        }


                        if (dt["cb"].ToString() == "0")
                        {
                            dt["cb"] = "";
                            dt["cb_type"] = "";
                        }


                        if (dt["parent_id"].ToString() == "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";
                            dt["cb"] = "<b>" + dt["cb"] + "</b>";
                            dt["cb_type"] = "<b>" + dt["cb_type"] + "</b>";
                        }
                        else if (dt["parent_id"].ToString() != "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";
                            dt["cb"] = "<b>" + dt["cb"] + "</b>";
                            dt["cb_type"] = "<b>" + dt["cb_type"] + "</b>";
                        }
                        else
                        {
                            dt["name"] = "<i>" + dt["name"] + "</i>";
                            dt["cb"] = "<i>" + dt["cb"] + "</i>";
                            dt["cb_type"] = "<i>" + dt["cb_type"] + "</i>";
                        }
                    }

                    DataTable dtStockDetail = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',1)");
                    string strClosingStock = ObjSysParam.GetCurencyConversionForInv(strCurrency, dtStockDetail.Rows[0][0].ToString());
                    TotalExpenses += Convert.ToDouble(strClosingStock);
                    TotalIncome = Math.Abs(TotalIncome);
                    TotalExpenses = Math.Abs(TotalExpenses);
                    double diff = 0;

                    if (dtBalance.Rows.Count > 0)
                    {
                        DataTable dtIncome = new DataView(dtBalance, "cb<>'0' and nature_of_account=" + strNatureOfAccountLiabilities, "", DataViewRowState.CurrentRows).ToTable();
                        DataTable dtExpenses = new DataView(dtBalance, "cb<>'0' and nature_of_account=" + strNatureOfAccountAssets, "", DataViewRowState.CurrentRows).ToTable();

                        DataRow row;
                        row = dtExpenses.NewRow();
                        row["Name"] = "<b>" + "Closing Stock" + "</b>";
                        row["cb"] = "<b>" + ObjSysParam.GetCurencyConversionForInv(strCurrency, strClosingStock) + "<b>";
                        row["cb_type"] = "DR";
                        row["parent_id"] = 0;
                        row["account_no"] = 0;
                        row["other_account_no"] = 0;
                        row["foreign_cb"] = 0;
                        row["company_cb"] = 0;

                        dtExpenses.Rows.Add(row);

                        if (profit != 0)
                        {
                            row = dtIncome.NewRow();
                            row["Name"] = "<b>" + "By Profit & Loss A/c" + "</b>";
                            row["cb"] = profit;
                            row["cb"] = "<b>" + ObjSysParam.GetCurencyConversionForInv(strCurrency, row["cb"].ToString()) + "<b>";
                            row["parent_id"] = 0;
                            row["account_no"] = 0;
                            row["other_account_no"] = 0;
                            row["foreign_cb"] = 0;
                            row["company_cb"] = 0;

                            dtIncome.Rows.Add(row);
                            TotalIncome += profit;
                        }

                        //diff = Math.Abs(TotalIncome) - Math.Abs(TotalExpenses);
                        diff = (TotalIncome) - Math.Abs(TotalExpenses);

                        if (diff > 0)
                        {
                            row = dtExpenses.NewRow();
                            row["Name"] = "<b>" + "Difference" + "</b>";
                            row["cb"] = diff;
                            row["cb"] = "<b>" + ObjSysParam.GetCurencyConversionForInv(strCurrency, row["cb"].ToString()) + "<b>";
                            row["parent_id"] = 0;
                            row["account_no"] = 0;
                            row["other_account_no"] = 0;
                            row["foreign_cb"] = 0;
                            row["company_cb"] = 0;

                            dtExpenses.Rows.Add(row);
                            TotalExpenses += diff;
                        }
                        else if (diff < 0)
                        {
                            row = dtIncome.NewRow();
                            row["Name"] = "<b>" + "Difference" + "</b>";
                            row["cb"] = Math.Abs(diff);
                            row["cb"] = "<b>" + ObjSysParam.GetCurencyConversionForInv(strCurrency, row["cb"].ToString()) + "<b>";
                            row["parent_id"] = 0;
                            row["account_no"] = 0;
                            row["other_account_no"] = 0;
                            row["foreign_cb"] = 0;
                            row["company_cb"] = 0;

                            dtIncome.Rows.Add(row);
                            TotalIncome += Math.Abs(diff);

                        }

                        if (dtExpenses.Rows.Count > dtIncome.Rows.Count)
                        {
                            //dtIncome.Rows.Count = dtExpenses.Rows.Count;
                            for (int counter = dtIncome.Rows.Count; counter < dtExpenses.Rows.Count; counter++)
                            {
                                row = dtIncome.NewRow();
                                row["Name"] = "&nbsp";
                                dtIncome.Rows.Add(row);
                            }
                        }
                        if (dtExpenses.Rows.Count < dtIncome.Rows.Count)
                        {
                            //dtIncome.Rows.Count = dtExpenses.Rows.Count;
                            for (int counter = dtExpenses.Rows.Count; counter < dtIncome.Rows.Count; counter++)
                            {
                                row = dtExpenses.NewRow();
                                row["Name"] = "&nbsp";
                                dtExpenses.Rows.Add(row);
                            }
                        }


                        DataTable dtLocation = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
                        if (dtLocation.Rows.Count > 0)
                        {
                            GVComplete.DataSource = dtLocation;
                            GVComplete.DataBind();
                        }

                        foreach (GridViewRow gvrC in GVComplete.Rows)
                        {
                            GridView gvIncome = (GridView)gvrC.FindControl("gvIncome");
                            GridView gvExpenses = (GridView)gvrC.FindControl("gvExpenses");
                            //fill income grid -------------
                            gvIncome.DataSource = dtIncome;
                            gvIncome.DataBind();

                            if (gvIncome.Rows.Count > 0)
                            {
                                Label lblgvCbTotal = (Label)gvIncome.FooterRow.FindControl("lblgvCbTotal");

                                lblgvCbTotal.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, TotalIncome.ToString());
                                liabilitiesTotal = Convert.ToDouble(ObjSysParam.GetCurencyConversionForInv(strCurrency, TotalIncome.ToString()));
                                gvIncome.HeaderRow.Cells[1].Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(), gvIncome.HeaderRow.Cells[1].Text + " ", Session["DBConnection"].ToString());
                            }
                            //----------------------------

                            //fill income grid -------------
                            gvExpenses.DataSource = dtExpenses;
                            gvExpenses.DataBind();

                            if (gvExpenses.Rows.Count > 0)
                            {
                                Label lblgvCbTotal_1 = (Label)gvExpenses.FooterRow.FindControl("lblgvCbTotal_1");

                                lblgvCbTotal_1.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, TotalExpenses.ToString());
                                AssestTotal = Convert.ToDouble(ObjSysParam.GetCurencyConversionForInv(strCurrency, TotalExpenses.ToString()));
                                gvExpenses.HeaderRow.Cells[1].Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(), gvExpenses.HeaderRow.Cells[1].Text + " ", Session["DBConnection"].ToString());
                            }
                            //----------------------------
                        }
                    }
                }
            }
        }
    }
    protected void btnCloseYear_Click(object sender, EventArgs e)
    {
        string strEmployeesalaryaccount = "0";
        string strEmployeeLoanaccount = "0";

        //For Check Balances
        btnExceute_Click(null, null);

        if (GVComplete.Rows.Count == 0)
        {
            DisplayMessage("Your have no Record to Close");
            return;
        }

        if (liabilitiesTotal != 0 && AssestTotal != 0)
        {
            if (liabilitiesTotal != AssestTotal)
            {
                DisplayMessage("Your Balances are not Same Please Reveiw your balances");
                return;
            }
        }

        //Commented Code
        //foreach (GridViewRow gv in GVComplete.Rows)
        //{
        //    GridView gvIncome = (GridView)gv.FindControl("gvIncome");

        //    foreach (GridViewRow gvI in gvIncome.Rows)
        //    {
        //        Label lblName = (Label)gvI.FindControl("lblgvGroupName");
        //        Label lblClosingBalance = (Label)gvI.FindControl("lblgvCb");
        //        string strCB = lblClosingBalance.Text.Trim().Replace("<b>", "").Trim();

        //        if (lblName.Text == "<b>Difference</b>")
        //        {
        //            double closingbalance = Convert.ToDouble(strCB);
        //            if (closingbalance != 0)
        //            {
        //                DisplayMessage("You have Diffrence Value So You Cant Close It");
        //                return;
        //            }
        //        }
        //    }

        //    GridView gvExpenses = (GridView)gv.FindControl("gvExpenses");

        //    foreach (GridViewRow gvE in gvExpenses.Rows)
        //    {
        //        Label lblName = (Label)gvE.FindControl("lblgvGroupName_1");
        //        Label lblClosingBalance = (Label)gvE.FindControl("lblgvCb_1");
        //        string strCB = lblClosingBalance.Text.Trim().Replace("<b>", "").Trim();

        //        if (lblName.Text == "<b>Difference</b>")
        //        {
        //            double closingbalance = Convert.ToDouble(strCB);
        //            if (closingbalance != 0)
        //            {
        //                DisplayMessage("You have Diffrence Value so you cant close It");
        //                return;
        //            }
        //        }
        //    }
        //}

        //Check Status
        string strStatus = objFYI.GetInfoByTransId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Status"].ToString();
        string strDetailStatus = string.Empty;

        if (strStatus == "ReOpen")
        {

        }
        else if (strStatus == "Open")
        {
            DataTable dtHeader = objFYI.GetInfoAllTrue(StrCompId);
            if (dtHeader.Rows.Count > 0)
            {
                dtHeader = new DataView(dtHeader, "Status='ReOpen'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtHeader.Rows.Count > 0)
                {
                    DisplayMessage("First Close Your ReOpen Year then you can close the Same");
                    return;
                }
            }
        }

        DataTable dtDetail = objFYIDetail.GetAllDataByHeader_Id(Session["FinanceYearId"].ToString());
        if (dtDetail.Rows.Count > 0)
        {
            if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtFromDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            {

            }
            else
            {
                DisplayMessage("First you need to Close Inventory Section");
                txtFromDate.Focus();
                return;
            }
        }

        //Check Validations For Accounts Closing.
        DataTable dtTransferInFinance = objVoucherHeader.GetRecordforReconcileFinance(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
        if (dtTransferInFinance.Rows.Count > 0)
        {
            DisplayMessage("You have UnPosted data in Transfer In Finance");
            txtFromDate.Focus();
            return;
        }

        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherAllTrueOnly(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
        if (dtVoucherHeader.Rows.Count > 0)
        {
            dtVoucherHeader = new DataView(dtVoucherHeader, "Field3='Pending'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherHeader.Rows.Count > 0)
            {
                DisplayMessage("You have " + dtVoucherHeader.Rows.Count + " Payment Vouchers Pending in Approval");
                txtFromDate.Focus();
                return;
            }
            else
            {

            }
        }

        //Commented Code
        string strCashFlowLocIds =  new Ac_Parameter_Location(Session["DBConnection"].ToString()).getCashFlowLocationGroup(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //DataTable dtCashFlowAll = objCashFlowHeader.GetCashFlowAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCashFlowLocIds);
        //if (dtCashFlowAll.Rows.Count > 0 && dtCashFlowAll != null)
        //{
        //    DataTable dtCashFlow = objCashFlowHeader.GetCashFlowByCashFlowDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCashFlowLocIds, txtToDate.Text);
        //    if (dtCashFlow.Rows.Count == 0)
        //    {
        //        DisplayMessage("You have UnPosted Cash Flow According to Financial Year");
        //        txtFromDate.Focus();
        //        return;
        //    }
        //}

        DateTime dtToDate = new DateTime();
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

        DataTable dtParam = new DataTable();

        //for Customer, Supplier and Capital Account
        string strReceiveVoucherAcc = string.Empty;
        dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
        }


        dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account");
        if (dtParam.Rows.Count > 0)
        {
            strEmployeesalaryaccount = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strEmployeesalaryaccount = "0";
        }

        dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Loan Account");
        if (dtParam.Rows.Count > 0)
        {
            strEmployeeLoanaccount = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strEmployeeLoanaccount = "0";
        }

        string strPaymentVoucherAcc = string.Empty;
        DataTable dtPaymentVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPaymentVoucherAcc = "0";
        }
       string strLOCCurrencyId = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string strCapitalAccountId = string.Empty;
        string strCapitalAccount = string.Empty;
        DataTable dtCapitalVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "CapitalAccount");
        if (dtCapitalVoucher.Rows.Count > 0)
        {
            strCapitalAccountId = dtCapitalVoucher.Rows[0]["Param_Value"].ToString();
            try
            {
                strCapitalAccount = ObjCOA.GetCOAByTransId(Session["CompId"].ToString(), strCapitalAccountId).Rows[0]["AccountName"].ToString();
            }
            catch
            {
                strCapitalAccount = "0";
            }
            
        }
        else
        {
            strCapitalAccount = "0";
        }

        dtDetail = new DataView(dtDetail, "Location_Id='" + Session["LocId"].ToString() + "' and Status='" + strStatus + "'", "", DataViewRowState.CurrentRows).ToTable();

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int q = 0;
            if (dtDetail.Rows.Count > 0)
            {
                string strDetailId = dtDetail.Rows[0]["Trans_Id"].ToString();
                objFYIClosingDetail.DeleteDetailRowsClosing(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, ref trns);
                string strSQL = "Update Ac_FinancialYear_Detail set Status='Close', ModifiedBy='" + Session["UserId"].ToString() + "', ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id='" + strDetailId + "'";
                q = objDA.execute_Command(strSQL, ref trns);

                if (q != 0)
                {
                    double CapitalAmount = 0;
                    double ProfitLossAmount = 0;
                    string strCAccType = string.Empty;
                    string strPFType = string.Empty;

                    //Insert In FDetail
                    foreach (GridViewRow gv in GVComplete.Rows)
                    {
                        GridView gvIncome = (GridView)gv.FindControl("gvIncome");

                        foreach (GridViewRow gvI in gvIncome.Rows)
                        {
                            double ClosingBalance = 0;
                            Label lblGName = (Label)gvI.FindControl("lblgvGroupName");
                            lblGName.Text = lblGName.Text.Trim().Replace("<b>", "").Trim();
                            lblGName.Text = lblGName.Text.Trim().Replace("</b>", "").Trim();
                            HiddenField hdnAccountId = (HiddenField)gvI.FindControl("hdngvAccountId");
                            Label lblClosingBalance = (Label)gvI.FindControl("lblgvCb");
                            lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("<i>", "").Trim();
                            lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("</i>", "").Trim();
                            Label lblBalanceType = (Label)gvI.FindControl("lblgvCbType");
                            lblBalanceType.Text = lblBalanceType.Text.Trim().Replace("<i>", "").Trim();
                            lblBalanceType.Text = lblBalanceType.Text.Trim().Replace("</i>", "").Trim();
                            HiddenField hdnForeignClosing = (HiddenField)gvI.FindControl("hdngvForeignCB");
                            HiddenField hdnCompanyClosing = (HiddenField)gvI.FindControl("hdngvCompanyCB");

                            if (lblGName.Text == "By Profit & Loss A/c")
                            {
                                lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("<b>", "").Trim();
                                lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("</b>", "").Trim();
                                ProfitLossAmount = Convert.ToDouble(lblClosingBalance.Text);
                                strPFType = "Income";
                            }

                            if (hdnAccountId.Value != "" && hdnAccountId.Value != "0")
                            {
                                ClosingBalance = Convert.ToDouble(lblClosingBalance.Text);
                                if (hdnAccountId.Value == strCapitalAccountId)
                                {
                                    CapitalAmount = ClosingBalance;
                                    strCAccType = lblBalanceType.Text;
                                }
                                else
                                {
                                    string strCurrencyId = (new DataView(ObjCOA.GetCOAAll(Session["CompId"].ToString(), ref trns), "Trans_Id='" + hdnAccountId.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Currency_Id"].ToString();
                                    //string strForeignRate = GetCurrency(strCurrencyId, lblClosingBalance.Text);
                                    //string strForignAmount = strForeignRate.Trim().Split('/')[0].ToString();
                                    //string strCompanyRate = GetCurrency(Session["CurrencyId"].ToString(), lblClosingBalance.Text);
                                    //string strCompanyAmount = strCompanyRate.Trim().Split('/')[0].ToString();

                                    if (hdnAccountId.Value == strPaymentVoucherAcc || hdnAccountId.Value== strEmployeesalaryaccount)
                                    {
                                    }
                                    else
                                    {
                                        if (ClosingBalance != 0)
                                        {
                                            if (lblBalanceType.Text == "DR")
                                            {
                                                objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, lblClosingBalance.Text, "Finance Closing Entry", lblBalanceType.Text, hdnForeignClosing.Value, hdnCompanyClosing.Value, strCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                                //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, "0", lblClosingBalance.Text, "0.00", strForignAmount, "0.00", strCurrencyId, strCompanyAmount, "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else if (lblBalanceType.Text == "CR")
                                            {
                                                objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, lblClosingBalance.Text, "Finance Closing Entry", lblBalanceType.Text, hdnForeignClosing.Value, hdnCompanyClosing.Value, strCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                                //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, "0", "0.00", lblClosingBalance.Text, "0.00", strForignAmount, strCurrencyId, "0.00", strCompanyAmount, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                }


                                if (hdnAccountId.Value == strPaymentVoucherAcc)
                                {
                                    DataTable dtAllSupplierData = objVoucherDetail.GetAllSupplierBalanceData(StrCompId, StrBrandId, strLocationId, strPaymentVoucherAcc, strPaymentVoucherAcc, txtFromDate.Text, dtToDate.ToString("dd-MMM-yyyy"), "1", Session["FinanceYearId"].ToString(), ref trns);
                                    if (dtAllSupplierData.Rows.Count > 0)
                                    {
                                        dtAllSupplierData = new DataView(dtAllSupplierData, "Name not is null and (Closing_Final<>'0')", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtAllSupplierData.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtAllSupplierData.Rows.Count; i++)
                                            {
                                                string strSupplierId = dtAllSupplierData.Rows[i]["OtherAccountId"].ToString();
                                                string strSupplierCurrency = string.Empty;
                                                DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strSupplierId);
                                                strSupplierCurrency = _dtTemp.Rows.Count > 0 ? _dtTemp.Rows[0]["Currency_Id"].ToString():"0";
                                                _dtTemp.Dispose();
                                                //DataTable dtSupData = objSupplierMaster.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId, ref trns);
                                                //if (dtSupData.Rows.Count > 0 && dtSupData != null)
                                                //{
                                                //    strSupplierCurrency = dtSupData.Rows[0]["Field3"].ToString();
                                                //}
                                                //else
                                                //{
                                                //    strSupplierCurrency = "80";
                                                //}


                                                //if (strSupplierCurrency == "")
                                                //{
                                                //    strSupplierCurrency = "0";
                                                //}

                                                double ClosingAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Closing_Balance"].ToString());
                                                double ForeignAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["ForeignClosing_Balance"].ToString());
                                                double CompanyAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Company_ClosingBalance"].ToString());
                                                if (ClosingAmount != 0)
                                                {
                                                    if (ClosingAmount > 0)
                                                    {
                                                        //Credit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "CR", ForeignAmount.ToString(), CompanyAmount.ToString(), strSupplierCurrency, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strSupplierId, ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, "0.00", ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), strSupplierCurrency, "0.00", CompanyAmount.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                    else
                                                    {
                                                        //Debit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "DR", ForeignAmount.ToString(), CompanyAmount.ToString(), strSupplierCurrency, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strSupplierId, ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), "0.00", strSupplierCurrency, CompanyAmount.ToString(), "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                                //code for insert employee opening balance


                                if (hdnAccountId.Value == strEmployeesalaryaccount)
                                {
                                    DataTable dtAllSupplierData = objVoucherDetail.GetAllEmployeeBalanceData(StrCompId, StrBrandId, strLocationId, strEmployeesalaryaccount, strEmployeesalaryaccount, txtFromDate.Text, dtToDate.ToString("dd-MMM-yyyy"), "1", Session["FinanceYearId"].ToString(), ref trns);
                                    if (dtAllSupplierData.Rows.Count > 0)
                                    {
                                        dtAllSupplierData = new DataView(dtAllSupplierData, "(Closing_Final<>'0')", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtAllSupplierData.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtAllSupplierData.Rows.Count; i++)
                                            {
                                                

                                                double ClosingAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Closing_Balance"].ToString());
                                                double ForeignAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["ForeignClosing_Balance"].ToString());
                                                double CompanyAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Company_ClosingBalance"].ToString());
                                                if (ClosingAmount != 0)
                                                {
                                                    if (ClosingAmount > 0)
                                                    {
                                                        //Credit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "CR", ForeignAmount.ToString(), CompanyAmount.ToString(), strLOCCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtAllSupplierData.Rows[i]["Emp_Id"].ToString(), ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, "0.00", ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), strSupplierCurrency, "0.00", CompanyAmount.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                    else
                                                    {
                                                        //Debit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "DR", ForeignAmount.ToString(), CompanyAmount.ToString(), strLOCCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtAllSupplierData.Rows[i]["Emp_Id"].ToString(), ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), "0.00", strSupplierCurrency, CompanyAmount.ToString(), "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }



                            }
                        }

                        GridView gvExpense = (GridView)gv.FindControl("gvExpenses");
                        foreach (GridViewRow gvE in gvExpense.Rows)
                        {
                            double ClosingBalance = 0;
                            Label lblGName = (Label)gvE.FindControl("lblgvGroupName_1");
                            lblGName.Text = lblGName.Text.Trim().Replace("<b>", "").Trim();
                            lblGName.Text = lblGName.Text.Trim().Replace("</b>", "").Trim();
                            HiddenField hdnAccountId = (HiddenField)gvE.FindControl("hdngvAccountId_1");
                            Label lblClosingBalance = (Label)gvE.FindControl("lblgvCb_1");
                            lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("<i>", "").Trim();
                            lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("</i>", "").Trim();
                            Label lblBalanceType = (Label)gvE.FindControl("lblgvCbType_1");
                            lblBalanceType.Text = lblBalanceType.Text.Trim().Replace("<i>", "").Trim();
                            lblBalanceType.Text = lblBalanceType.Text.Trim().Replace("</i>", "").Trim();
                            HiddenField hdnForeignClosing = (HiddenField)gvE.FindControl("hdngvForeignCB_1");
                            HiddenField hdnCompanyClosing = (HiddenField)gvE.FindControl("hdngvCompanyCB_1");

                            if (lblGName.Text == "By Profit & Loss A/c")
                            {
                                lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("<b>", "").Trim();
                                lblClosingBalance.Text = lblClosingBalance.Text.Trim().Replace("</b>", "").Trim();
                                ProfitLossAmount = Convert.ToDouble(lblClosingBalance.Text);
                                strPFType = "Expenses";
                            }

                            if (hdnAccountId.Value != "" && hdnAccountId.Value != "0")
                            {
                                ClosingBalance = Convert.ToDouble(lblClosingBalance.Text);

                                if (hdnAccountId.Value == strCapitalAccountId)
                                {
                                    CapitalAmount = ClosingBalance;
                                    strCAccType = lblBalanceType.Text;
                                }
                                else
                                {
                                    string strCurrencyId = (new DataView(ObjCOA.GetCOAAll(Session["CompId"].ToString()), "Trans_Id='" + hdnAccountId.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Currency_Id"].ToString();
                                    //string strForeignRate = GetCurrency(strCurrencyId, lblClosingBalance.Text);
                                    //string strForignAmount = strForeignRate.Trim().Split('/')[0].ToString();
                                    //string strCompanyRate = GetCurrency(Session["CurrencyId"].ToString(), lblClosingBalance.Text);
                                    //string strCompanyAmount = strCompanyRate.Trim().Split('/')[0].ToString();
                                    if (hdnAccountId.Value == strReceiveVoucherAcc || hdnAccountId.Value == strEmployeeLoanaccount)
                                    {
                                    }
                                    else
                                    {
                                        if (ClosingBalance != 0)
                                        {
                                            if (lblBalanceType.Text == "DR")
                                            {
                                                objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, lblClosingBalance.Text, "Finance Closing Entry", lblBalanceType.Text, hdnForeignClosing.Value, hdnCompanyClosing.Value, strCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                                //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, "0", lblClosingBalance.Text, "0.00", strForignAmount, "0.00", strCurrencyId, strCompanyAmount, "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else if (lblBalanceType.Text == "CR")
                                            {
                                                objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, lblClosingBalance.Text, "Finance Closing Entry", lblBalanceType.Text, hdnForeignClosing.Value, hdnCompanyClosing.Value, strCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                                //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, "0", "0.00", lblClosingBalance.Text, "0.00", strForignAmount, strCurrencyId, "0.00", strCompanyAmount, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                }

                                if (hdnAccountId.Value == strReceiveVoucherAcc)
                                {
                                    DataTable dtAllCustomerData = objVoucherDetail.GetAllCustomerBalanceData(StrCompId, StrBrandId, strLocationId, strReceiveVoucherAcc, strReceiveVoucherAcc, txtFromDate.Text, dtToDate.ToString("dd-MMM-yyyy"), "1", Session["FinanceYearId"].ToString(), ref trns);
                                    if (dtAllCustomerData.Rows.Count > 0)
                                    {
                                        dtAllCustomerData = new DataView(dtAllCustomerData, "Name not is null and (Closing_Final<>'0')", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtAllCustomerData.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtAllCustomerData.Rows.Count; i++)
                                            {
                                                string strCustomerId = dtAllCustomerData.Rows[i]["OtherAccountId"].ToString();
                                                string strCustomerCurrency = string.Empty;
                                                DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strCustomerId);
                                                strCustomerCurrency = _dtTemp.Rows.Count > 0 ? _dtTemp.Rows[0]["Currency_Id"].ToString() : "0";
                                                _dtTemp.Dispose();

                                                //string strCustomerCurrency = (new DataView(objCustomerMaster.GetCustomerAllDataByCompany(Session["CompId"].ToString(), ref trns), "Brand_Id='" + Session["BrandId"].ToString() + "' and Customer_Id='" + strCustomerId + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Field3"].ToString();
                                                //if (strCustomerCurrency == "")
                                                //{
                                                //    strCustomerCurrency = "0";
                                                //}

                                                double ClosingAmount = Convert.ToDouble(dtAllCustomerData.Rows[i]["Closing_Balance"].ToString());
                                                double ForeignAmount = Convert.ToDouble(dtAllCustomerData.Rows[i]["ForeignClosing_Balance"].ToString());
                                                double CompanyAmount = Convert.ToDouble(dtAllCustomerData.Rows[i]["cmp_cb"].ToString());
                                                if (ClosingAmount != 0)
                                                {
                                                    if (ClosingAmount > 0)
                                                    {
                                                        //Debit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "DR", ForeignAmount.ToString(), CompanyAmount.ToString(), strCustomerCurrency, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strCustomerId, ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strCustomerId, ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), "0.00", strCustomerCurrency, CompanyAmount.ToString(), "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                    else
                                                    {
                                                        //Credit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "CR", ForeignAmount.ToString(), CompanyAmount.ToString(), strCustomerCurrency, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strCustomerId, ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strCustomerId, "0.00", ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), strCustomerCurrency, "0.00", CompanyAmount.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                                //insert employee loan account  opening balance

                                if (hdnAccountId.Value == strEmployeeLoanaccount)
                                {
                                    DataTable dtAllSupplierData = objVoucherDetail.GetAllEmployeeBalanceData(StrCompId, StrBrandId, strLocationId, strEmployeeLoanaccount, strEmployeeLoanaccount, txtFromDate.Text, dtToDate.ToString("dd-MMM-yyyy"), "1", Session["FinanceYearId"].ToString(), ref trns);
                                    if (dtAllSupplierData.Rows.Count > 0)
                                    {
                                        dtAllSupplierData = new DataView(dtAllSupplierData, "(Closing_Final<>'0')", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtAllSupplierData.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtAllSupplierData.Rows.Count; i++)
                                            {


                                                double ClosingAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Closing_Balance"].ToString());
                                                double ForeignAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["ForeignClosing_Balance"].ToString());
                                                double CompanyAmount = Convert.ToDouble(dtAllSupplierData.Rows[i]["Company_ClosingBalance"].ToString());
                                                if (ClosingAmount != 0)
                                                {
                                                    if (ClosingAmount > 0)
                                                    { 
                                                        //Credit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "CR", ForeignAmount.ToString(), CompanyAmount.ToString(), strLOCCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtAllSupplierData.Rows[i]["Emp_Id"].ToString(), ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, "0.00", ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), strSupplierCurrency, "0.00", CompanyAmount.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                    else
                                                    {
                                                        //Debit Entry
                                                        ClosingAmount = Math.Abs(ClosingAmount);
                                                        objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", hdnAccountId.Value, ClosingAmount.ToString(), "Finance Closing Entry OtherAccount", "DR", ForeignAmount.ToString(), CompanyAmount.ToString(), strLOCCurrencyId, "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtAllSupplierData.Rows[i]["Emp_Id"].ToString(), ref trns);
                                                        //objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnAccountId.Value, strSupplierId, ClosingAmount.ToString(), "0.00", ForeignAmount.ToString(), "0.00", strSupplierCurrency, CompanyAmount.ToString(), "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }


                        double FinalAmt = 0;
                        if (CapitalAmount != 0)
                        {
                            if (strPFType == "Income")
                            {
                                FinalAmt = CapitalAmount + ProfitLossAmount;
                            }
                            else if (strPFType == "Expenses")
                            {
                                FinalAmt = CapitalAmount - ProfitLossAmount;
                            }

                            if (FinalAmt != 0)
                            {
                                if (0 > FinalAmt)
                                {
                                    objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", strCapitalAccountId, FinalAmt.ToString(), "Finance Closing Entry for CapitalAccount", "DR", FinalAmt.ToString(), FinalAmt.ToString(), "0", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                }
                                else if (0 < FinalAmt)
                                {
                                    objFYIClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDetailId, "F", strCapitalAccountId, FinalAmt.ToString(), "Finance Closing Entry for CapitalAccount", "CR", FinalAmt.ToString(), FinalAmt.ToString(), "0", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);
                                }
                            }
                        }
                    }
                }
                else
                {

                }

                DisplayMessage("Successfully Close Financial Year");
                GVComplete.DataSource = null;
                GVComplete.DataBind();
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
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
            return;
        }
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //try
        //{
        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
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
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
}