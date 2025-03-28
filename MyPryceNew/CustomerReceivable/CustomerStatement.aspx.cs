using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using net.webservicex.www;
using System.Data;
using System.Collections;
using Newtonsoft.Json;

public partial class CustomerReceivable_CustomerStatement : System.Web.UI.Page
{
    CompanyMaster ObjCompany = null;
    Set_CustomerMaster ObjCoustmer = null;
    Set_Location_Department objLocDept = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ems_ContactMaster objContact = null;
    Ac_Finance_Year_Info objFYI = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    DataAccessClass da = null;
    Ac_ParameterMaster objAcParameter = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Common cmn = null;
    Ac_Reconcile_Detail objReconcileDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PegasusDataAccess.DataAccessClass objDA = null;
    PageControlCommon objPageCmn = null;

    CustomerAgeingEstatement objCustomerAgeingStatement = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strSta = string.Empty;
    int a = 2;
    public decimal ab { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        ObjAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objReconcileDetail = new Ac_Reconcile_Detail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objCustomerAgeingStatement = new CustomerAgeingEstatement(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/CustomerStatement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "300").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}
            //AllPageCode();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();

            if (strLocationId != "" && strLocationId != "0")
            {
                //txtToLocation.Text = Session["LocName"] + "/" + strLocationId;
            }
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();

            FillLocation();

            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());


            //txtReconciled.Text = "33FFCC";
            // txtReconciled.ForeColor = System.Drawing.Color.FromName("#33FFCC");
            txtReconciled.BackColor = System.Drawing.Color.FromName("#33FFCC");
            txtConflicted.Text = "66FF66";
            txtConflicted.ForeColor = System.Drawing.Color.FromName("#66FF66");
            txtNotReconciled.Text = "FF33FF";
            txtNotReconciled.ForeColor = System.Drawing.Color.FromName("#FF33FF");

            if (Request.QueryString["Id"] != null)
            {
                DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(Request.QueryString["Id"].ToString());
                if (_dtTemp.Rows.Count > 0)
                {
                    txtCustomerName.Text = _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString();
                    txtFromDate.Text = Session["CusterStatementFromDate"].ToString();
                    txtToDate.Text = Session["CustomerStatementToDate"].ToString();
                    string strLocation = Session["CustomerStatementLocations"].ToString();
                    if (strLocation != "")
                    {
                        for (int j = 0; j < strLocation.Split(',').Length; j++)
                        {
                            if (strLocation.Split(',')[j] != "")
                            {
                                ListItem li = new ListItem();
                                li.Value = strLocation.Split(',')[j].ToString();
                                li.Text = ObjLocation.GetLocationMasterByLocationId(strLocation.Split(',')[j].ToString()).Rows[0]["Location_Name"].ToString();
                                lstLocationSelect.Items.Add(li);
                                lstLocation.Items.Remove(li);
                            }
                        }
                    }
                    btnGetReport_Click(sender, e);
                }
            }
            else
            {

            }

        }

        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        SetColorCode();
        //if (IsPostBack)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SetContextKey()", true);
        //}
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }

    }
    protected void SetColorCode()
    {
        lblReconciled.Text = "Reconciled";
        lblConflicted.Text = "Conflicted";
        lblNotReconciled.Text = "Not Reconciled";
        //ForColoCodes
        DataTable dtReconciled = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ReconciledColorCode");
        if (dtReconciled.Rows.Count > 0)
        {
            txtReconciled.Text = dtReconciled.Rows[0]["Param_Value"].ToString();
            txtReconciled.ForeColor = System.Drawing.Color.FromName("#" + txtReconciled.Text);
            //txtReconciled.BackColor= System.Drawing.Color.FromName("#" + dtReconciled.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            txtReconciled.Text = "33FFCC";
            txtReconciled.ForeColor = System.Drawing.Color.FromName("#33FFCC");
            //txtReconciled.BackColor = System.Drawing.Color.FromName("#" + dtReconciled.Rows[0]["Param_Value"].ToString());
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
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        //div_inAactive_account.Visible = clsPagePermission.bViewAllUserRecord;
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        ////New Code created by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("300");
        //if (dtModule.Rows.Count > 0)
        //{
        //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        //}
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}

        ////End Code
        //Page.Title = objsys.GetSysTitle();
        //StrUserId = Session["UserId"].ToString();
        //StrCompId = Session["CompId"].ToString();
        //StrBrandId = Session["BrandId"].ToString();
        //strLocationId = Session["LocId"].ToString();
        //Session["AccordianId"] = strModuleId;
        //Session["HeaderText"] = strModuleName;
        //if (Session["EmpId"].ToString() == "0")
        //{
        //    //btnGetReport.Visible = true;
        //}
        //else
        //{

        //    DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "300");
        //    if (dtAllPageCode.Rows.Count == 0)
        //    {
        //        Session.Abandon();
        //        Response.Redirect("~/ERPLogin.aspx");
        //    }
        //    foreach (DataRow DtRow in dtAllPageCode.Rows)
        //    {
        //        if (DtRow["Op_Id"].ToString() == "1")
        //        {
        //            //btnGetReport.Visible = true;
        //        }
        //    }
        //}
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            return;
        }
        string strCurrency = Session["LocCurrencyId"].ToString();

        //for Blank Values              
        GVCStatement.DataSource = null;
        GVCStatement.DataBind();

        if (hdnAgeing.Value != "0")
        {
            if (hdnAgeing.Value == "True")
            {
                chkAgeingAnalysis.Checked = true;
                tdAgeing.Visible = true;
            }
        }
        else
        {
            chkAgeingAnalysis.Checked = false;
            tdAgeing.Visible = false;
        }
        //End

        //Added by Neelkanth Purohit - 24/08/2018
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
                        ViewState["CustomerCurrencyName"] = dt.Rows[0]["Currency_Id"].ToString();
                        ViewState["CustomerCurrencyId"] = dt.Rows[0]["Currency_Id"].ToString();
                        ViewState["CustomerCurrencyCode"] = dt.Rows[0]["Currency_Code"].ToString();
                        return;
                    }
                }
            }
        }
        catch
        {
            DisplayMessage("Customer is not valid please select another on");
            txtCustomerName.Text = "";
            txtCustomerName.Focus();
        }

        //------------------end---------------------

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
        //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //        return;
        //    }

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Customer Name");
        //        txtCustomerName.Text = "";
        //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //    }
        //    else
        //    {
        //        string strCustomerId = dt.Rows[0]["Trans_Id"].ToString();
        //        if (strCustomerId != "0" && strCustomerId != "")
        //        {
        //            DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
        //            if (dtCus.Rows.Count > 0)
        //            {
        //                Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();

        //                string strCustomerAccount = Session["CustomerAccountId"].ToString();
        //                if (strCustomerAccount == "" || strCustomerAccount == "0")
        //                {
        //                    DisplayMessage("Need to Set Customer Account in Customer Setup");
        //                    txtCustomerName.Text = "";
        //                    return;
        //                }

        //                Session["CustomerCurrency"] = dtCus.Rows[0]["Field31"].ToString();
        //                if(Session["CustomerCurrency"].ToString()!="")
        //                {
        //                    string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, Session["CustomerCurrency"].ToString());
        //                }
        //                else
        //                {
        //                    DisplayMessage("Please Set Currency Id for the Customer");
        //                    return;
        //                }                        
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Customer Name");
        //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //    return;
        //}
    }


    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        //strLocationId = Session["LocId"].ToString();
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
        int fyear_id = 0;
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(), Session["CompId"].ToString());
        //fyear_id = 11;
        //if (fyear_id == 0)
        //{
        //    GVCStatement.DataSource = null;
        //    GVCStatement.DataBind();
        //    DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
        //    return;
        //}

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
        else
        {
            DisplayMessage("Need to Fill To Date");
            txtToDate.Focus();
            return;
        }

        txtCustomerName_TextChanged(sender, e);
        string strCurrencyId = string.Empty;
        string strCurrencyType = string.Empty;

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

        string strCurrencyVal = Session["LocCurrencyId"].ToString(); ;
        //For Account Parameter
        string strCashAccount = string.Empty;
        string strCreditAccount = string.Empty;
        string strReceiveVoucherAcc = string.Empty;

        strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        //End

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
                DisplayMessage("From date should be less then to date ");
                txtFromDate.Focus();
                return;
            }
        }


        DateTime dtFromdate = Convert.ToDateTime(txtFromDate.Text);

        if (txtCustomerName.Text != "")
        {
            //For Opening Balance Start
            if (txtFromDate.Text != "")
            {

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
                double OpeningBalance = 0;
                DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strReceiveVoucherAcc + "','" + txtCustomerName.Text.Split('/')[1].ToString() + "','" + strCurrencyType + "','" + fyear_id + "')) OpeningBalance");
                if (dtOpeningBalance.Rows.Count > 0)
                {
                    OpeningBalance = Convert.ToDouble(dtOpeningBalance.Rows[0][0].ToString());
                    if (OpeningBalance < 0)
                    {
                        strSta = "Credit";
                    }
                    else if (OpeningBalance > 0)
                    {
                        strSta = "Debit";
                    }
                    else
                    {
                        OpeningBalance = 0;
                    }
                }

                if (OpeningBalance != 0)
                {
                    string DOB = OpeningBalance.ToString();
                    txtOpeningBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, DOB);
                }
                else
                {
                    string DOB = OpeningBalance.ToString();
                    txtOpeningBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, DOB);
                }
            }
        }
        string strCustomerCurrencyCode = ViewState["CustomerCurrencyCode"] == null ? "" : ViewState["CustomerCurrencyCode"].ToString();
        lblFClosing.Text = "Closing(" + strCustomerCurrencyCode + ")";
        lblFOpening.Text = "Opening(" + strCustomerCurrencyCode + ")";
        if (txtCustomerName.Text != "")
        {
            //code to get foreign opening balance - Neelkanth Purohit (06/09/2018)
            double fOpeningBalance = 0;
            string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strReceiveVoucherAcc + "','" + txtCustomerName.Text.Split('/')[1].ToString() + "','3','" + fyear_id + "')) OpeningBalance";
            double.TryParse(objDA.get_SingleValue(sql).ToString(), out fOpeningBalance);
            txtFOpening.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, fOpeningBalance.ToString());
            txtFClosing.Text = txtFOpening.Text;

            DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, strCurrencyType);
            if (dtStatement.Rows.Count > 0)
            {
                if (dtStatement.Rows.Count > 0)
                {
                    if (ddlCVoucherType.SelectedValue != "--Select--")
                    {
                        dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlCVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                        if (dtStatement.Rows.Count > 0)
                        {

                            //GVCStatement.DataSource = dtStatement;
                            // GVCStatement.DataBind();
                            chkAgeingAnalysis.Visible = false;
                            if (hdnAgeing.Value != "0")
                            {
                                if (hdnAgeing.Value == "True")
                                {
                                    chkAgeingAnalysis.Checked = true;
                                }
                            }
                            else
                            {
                                chkAgeingAnalysis.Checked = false;
                            }
                        }
                    }
                    else
                    {

                        //GVCStatement.DataSource = dtStatement;
                        //GVCStatement.DataBind();
                        chkAgeingAnalysis.Visible = false;
                        if (hdnAgeing.Value != "0")
                        {
                            if (hdnAgeing.Value == "True")
                            {
                                chkAgeingAnalysis.Checked = true;
                            }
                        }
                        else
                        {
                            chkAgeingAnalysis.Checked = false;
                        }
                    }
                }
                else
                {
                    GVCStatement.DataSource = null;
                    GVCStatement.DataBind();
                }

                //Regrading Balance & Total                
                //string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, Session["SupplierCurrency"].ToString());           
                string strStatus = "False";
                string strBalanceA = string.Empty;
                string strBalanceF = string.Empty;
                double debitamount = 0;
                double creditamount = 0;
                double debitTotalamount = 0;
                double creditTotalamount = 0;
                int TotalReconciledVoucherCount = 0;
                int TotalConflictVoucherCount = 0;
                int TotalNotReconciledVoucherCount = 0;
                SetColorCode();
                DataTable dtRDetail_Temp = objReconcileDetail.GetReconcileDetailAllDataOnly();
                double fClosingBalance = fOpeningBalance;
                double fAmount = 0;
                foreach (GridViewRow gvr in GVCStatement.Rows)
                {
                    HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                    Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                    Label lblGvFAmount = (Label)gvr.FindControl("lblGvFAmount");
                    Label lblGvFBalance = (Label)gvr.FindControl("lblGvFBalance");

                    lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvDebitAmount.Text);
                    lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvCreditAmount.Text);
                    lblGvFAmount.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblGvFAmount.Text);
                    debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                    creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                    Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");
                    //Label lblgvFregnAmount = (Label)gvr.FindControl("lblgvForeignAmount");
                    //Label lblgvFregnBalance = (Label)gvr.FindControl("lblgvFVBalance");
                    lblGvFAmount.Text = debitamount == 0 ? "-" + lblGvFAmount.Text : lblGvFAmount.Text;
                    double.TryParse(lblGvFAmount.Text, out fAmount);
                    fClosingBalance += fAmount;
                    lblGvFBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, fClosingBalance.ToString());

                    if (hdngvDetailId.Value != "" && hdngvDetailId.Value != "0")
                    {
                        DataTable dtRDetail = dtRDetail_Temp;
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
                                        //gvr.BackColor = txtReconciled.BackColor;
                                        TotalReconciledVoucherCount++;
                                    }
                                    else if (status == "False")
                                    {
                                        gvr.BackColor = System.Drawing.Color.FromName("#" + txtConflicted.Text);
                                        //gvr.BackColor = txtConflicted.BackColor;
                                        TotalConflictVoucherCount++;
                                    }
                                }
                                else
                                {
                                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtNotReconciled.Text);
                                    //gvr.BackColor = txtNotReconciled.BackColor;
                                    TotalNotReconciledVoucherCount++;
                                }
                            }
                        }
                    }


                    if (strStatus == "False")
                    {
                        //string strCredit = "-";
                        if (strSta == "Credit")
                        {
                            lblgvBalance.Text = txtOpeningBalance.Text;
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalance.Text);
                            strStatus = "True";
                        }
                        else if (strSta == "Debit")
                        {
                            //lblgvBalance.Text = strCredit + "" + txtOpeningBalance.Text;
                            lblgvBalance.Text = txtOpeningBalance.Text;
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalance.Text);
                            strStatus = "True";
                        }
                        else
                        {
                            lblgvBalance.Text = "0";
                            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalance.Text);
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
                        lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalance.Text);
                    }
                    else if (creditamount != 0)
                    {
                        if (lblgvBalance.Text != "" && float.Parse(lblgvBalance.Text) != 0)
                        {
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(creditamount.ToString())).ToString();
                        }
                        else
                        {
                            lblgvBalance.Text = "0.00";
                            lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(creditamount.ToString())).ToString();
                        }


                        strBalanceA = lblgvBalance.Text;
                        lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalance.Text);
                    }

                    debitTotalamount += debitamount;
                    creditTotalamount += creditamount;
                    txtClosingBalance.Text = strBalanceA;
                    txtClosingBalance.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, txtClosingBalance.Text);
                }


                lblReconciled.Text = TotalReconciledVoucherCount > 0 ? lblReconciled.Text + "(" + TotalReconciledVoucherCount.ToString() + ")" : "Reconciled";
                lblConflicted.Text = TotalConflictVoucherCount > 0 ? lblConflicted.Text + "(" + TotalConflictVoucherCount.ToString() + ")" : "Conflicted";
                lblNotReconciled.Text = TotalNotReconciledVoucherCount > 0 ? lblNotReconciled.Text + "(" + TotalNotReconciledVoucherCount.ToString() + ")" : "Not Reconciled";

                if (GVCStatement.Rows.Count > 0)
                {
                    Label lblgvDebitTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvDebitTotal");
                    Label lblgvCreditTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvCreditTotal");
                    Label lblgvBalanceTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvBalanceTotal");
                    Label lblgvFTotal = (Label)GVCStatement.FooterRow.FindControl("lblGvFBalanceTotal");
                    lblgvDebitTotal.Text = debitTotalamount.ToString();
                    lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvDebitTotal.Text);
                    lblgvCreditTotal.Text = creditTotalamount.ToString();
                    lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvCreditTotal.Text);
                    lblgvBalanceTotal.Text = strBalanceA;
                    lblgvBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, lblgvBalanceTotal.Text);
                    lblgvFTotal.Text = objsys.GetCurencyConversionForInv(strCurrencyVal, fClosingBalance.ToString());
                    txtFClosing.Text = lblgvFTotal.Text;
                    GVCStatement.HeaderRow.Cells[9].Text = "Foreign(" + strCustomerCurrencyCode + ")";
                    GVCStatement.HeaderRow.Cells[10].Text = "Balance(" + strCustomerCurrencyCode + ")";
                }
                else
                {
                    chkAgeingAnalysis.Visible = false;
                    // DisplayMessage("You have no record available");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                }
            }
            else
            {
                GVCStatement.DataSource = null;
                GVCStatement.DataBind();
                chkAgeingAnalysis.Visible = false;
                txtClosingBalance.Text = txtOpeningBalance.Text;

                //DisplayMessage("You have no record available");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
            }
            if (dtStatement.Rows.Count > 0)
            {
                string data = JsonConvert.SerializeObject(dtStatement);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GetReport(" + data + ")", true);
            }
            else
            {
                DisplayMessage("You have no record available");
            }

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
        Reset();
    }
    public void Reset()
    {
        txtCustomerName.Text = "";
        DateTime dt = DateTime.Now;
        DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
        txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

        txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        txtOpeningBalance.Text = "";

        GVCStatement.DataSource = null;
        GVCStatement.DataBind();
        hdnAgeing.Value = "0";
        chkAgeingAnalysis.Checked = false;
        tdAgeing.Visible = false;
        txt0to30.Text = "";
        txt31to60.Text = "";
        txt61to90.Text = "";
        txt91to180.Text = "";
        txt181to365.Text = "";
        txtAbove365.Text = "";
        FillLocation();
        txtClosingBalance.Text = "";
        SetColorCode();
        txtFOpening.Text = "";
        txtFClosing.Text = "";
        ViewState["CustomerCurrencyName"] = null;
        ViewState["CustomerCurrencyId"] = null;
        ViewState["CustomerCurrencyCode"] = null;

    }
    protected void lblgvVoucherNo_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;


        string[] arguments = tblVoucherNo.Value.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string Trans_Id = arguments[2].Trim();
        string LocationId = arguments[3].Trim();

        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeader();
        if (dtVoucherHeader.Rows.Count > 0)
        {
            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + Trans_Id + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherHeader.Rows.Count > 0)
            {
                strVoucherType = dtVoucherHeader.Rows[0]["Voucher_Type"].ToString();
            }
        }

        if (RefId == "0" && RefId != "")
        {
            if (IsObjectPermission("160", "184"))
            {
                string strCmd = string.Format("window.open('../VoucherEntries/VoucherDetail.aspx?Id=" + Trans_Id + "&LocId=" + LocationId + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "reloadtblData()", true);
            }
            else
            {
                DisplayMessage("You have no permission for view detail");
                return;
            }
        }
        else if (RefId != "0" && RefId != "")
        {
            if (RefType == "SINV")
            {
                if (IsObjectPermission("144", "92"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "reloadtblData()", true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
        }
        else
        {
            //myButton.Attributes.Add("
            DisplayMessage("No Data");
            return;
        }
        //AllPageCode();

    }

    protected void chkAgeingAnalysis_CheckedChanged(object sender, EventArgs e)
    {
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

        string strCurrency = Session["LocCurrencyId"].ToString(); ;
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }


        //Check Location Currency
        string strCurrencyIdNew = string.Empty;
        string strCurrencyType = string.Empty;
        string strCurrencyId = string.Empty;
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

        if (chkAgeingAnalysis.Checked == true)
        {
            hdnAgeing.Value = "True";
            tdAgeing.Visible = true;
            lblAgeingAnalysis.Text = "";
            lblAgeingAnalysis.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Ageing_Analysis, Session["DBConnection"].ToString());

            if (txtCustomerName.Text != "")
            {
                //For 0-30 Days
                try
                {
                    DataTable dt30Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "4", strCurrencyType);

                    if (dt30Days.Rows.Count > 0)
                    {
                        txt0to30.Text = dt30Days.Rows[0][0].ToString();
                        if (txt0to30.Text == "")
                        {
                            txt0to30.Text = "0.00";
                        }
                        txt0to30.Text = objsys.GetCurencyConversionForInv(strCurrency, txt0to30.Text);
                    }
                    else
                    {
                        txt0to30.Text = "0.00";
                        txt0to30.Text = objsys.GetCurencyConversionForInv(strCurrency, txt0to30.Text);
                    }
                }
                catch
                {
                    txt0to30.Text = "0.00";
                    txt0to30.Text = objsys.GetCurencyConversionForInv(strCurrency, txt0to30.Text);
                }

                //For 31-60 Days
                try
                {
                    DataTable dt60Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "5", strCurrencyType);
                    if (dt60Days.Rows.Count > 0)
                    {
                        txt31to60.Text = dt60Days.Rows[0][0].ToString();
                        if (txt31to60.Text == "")
                        {
                            txt31to60.Text = "0.00";
                        }
                        txt31to60.Text = objsys.GetCurencyConversionForInv(strCurrency, txt31to60.Text);
                    }
                    else
                    {
                        txt31to60.Text = "0.00";
                        txt31to60.Text = objsys.GetCurencyConversionForInv(strCurrency, txt31to60.Text);
                    }
                }
                catch
                {
                    txt31to60.Text = "0.00";
                    txt31to60.Text = objsys.GetCurencyConversionForInv(strCurrency, txt31to60.Text);
                }

                //For 61-90 Days
                try
                {
                    DataTable dt90Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "6", strCurrencyType);
                    if (dt90Days.Rows.Count > 0)
                    {
                        txt61to90.Text = dt90Days.Rows[0][0].ToString();
                        if (txt61to90.Text == "")
                        {
                            txt61to90.Text = "0.00";
                        }
                        txt61to90.Text = objsys.GetCurencyConversionForInv(strCurrency, txt61to90.Text);
                    }
                    else
                    {
                        txt61to90.Text = "0.00";
                        txt61to90.Text = objsys.GetCurencyConversionForInv(strCurrency, txt61to90.Text);
                    }
                }
                catch
                {
                    txt61to90.Text = "0.00";
                    txt61to90.Text = objsys.GetCurencyConversionForInv(strCurrency, txt61to90.Text);
                }

                //For 91-180 Days
                try
                {
                    DataTable dt180Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "7", strCurrencyType);
                    if (dt180Days.Rows.Count > 0)
                    {
                        txt91to180.Text = dt180Days.Rows[0][0].ToString();
                        if (txt91to180.Text == "")
                        {
                            txt91to180.Text = "0.00";
                        }
                        txt91to180.Text = objsys.GetCurencyConversionForInv(strCurrency, txt91to180.Text);
                    }
                    else
                    {
                        txt91to180.Text = "0.00";
                        txt91to180.Text = objsys.GetCurencyConversionForInv(strCurrency, txt91to180.Text);
                    }
                }
                catch
                {
                    txt91to180.Text = "0.00";
                    txt91to180.Text = objsys.GetCurencyConversionForInv(strCurrency, txt91to180.Text);
                }

                //For 181-365 Days
                try
                {
                    DataTable dt365Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "8", strCurrencyType);
                    if (dt365Days.Rows.Count > 0)
                    {
                        txt181to365.Text = dt365Days.Rows[0][0].ToString();
                        if (txt181to365.Text == "")
                        {
                            txt181to365.Text = "0.00";
                        }
                        txt181to365.Text = objsys.GetCurencyConversionForInv(strCurrency, txt181to365.Text);
                    }
                    else
                    {
                        txt181to365.Text = "0.00";
                        txt181to365.Text = objsys.GetCurencyConversionForInv(strCurrency, txt181to365.Text);
                    }
                }
                catch
                {
                    txt181to365.Text = "0.00";
                    txt181to365.Text = objsys.GetCurencyConversionForInv(strCurrency, txt181to365.Text);
                }

                //For 181-365 Days
                try
                {
                    DataTable dt365DaysAbove = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtCustomerName.Text.Split('/')[1].ToString(), "RV", "9", strCurrencyType);
                    if (dt365DaysAbove.Rows.Count > 0)
                    {
                        txtAbove365.Text = dt365DaysAbove.Rows[0][0].ToString();
                        if (txtAbove365.Text == "")
                        {
                            txtAbove365.Text = "0.00";
                        }
                        txtAbove365.Text = objsys.GetCurencyConversionForInv(strCurrency, txtAbove365.Text);
                    }
                    else
                    {
                        txtAbove365.Text = "0.00";
                        txtAbove365.Text = objsys.GetCurencyConversionForInv(strCurrency, txtAbove365.Text);
                    }
                }
                catch
                {
                    txtAbove365.Text = "0.00";
                    txtAbove365.Text = objsys.GetCurencyConversionForInv(strCurrency, txtAbove365.Text);
                }
            }

            double debitamount = 0;
            double creditamount = 0;
            SetColorCode();
            foreach (GridViewRow gvr in GVCStatement.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
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
            }

        }
        else
        {
            hdnAgeing.Value = "0";
            tdAgeing.Visible = false;
        }
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
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText, contextKey == "True" ? true : false))
        {
            //dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListLocation(string prefixText, int count, string contextKey)
    //{
    //    LocationMaster objLoc = new LocationMaster();
    //    DataTable dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

    //    dt = new DataView(dt, "Location_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

    //    string[] str = new string[dt.Rows.Count];
    //    if (dt.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
    //        }
    //    }
    //    else
    //    {
    //        if (prefixText.Length > 2)
    //        {
    //            str = null;
    //        }
    //        else
    //        {
    //            dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
    //            if (dt.Rows.Count > 0)
    //            {
    //                str = new string[dt.Rows.Count];
    //                for (int i = 0; i < dt.Rows.Count; i++)
    //                {
    //                    str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
    //                }
    //            }
    //        }
    //    }
    //    return str;
    //}
    protected void lnkgvCustomer_Click(object sender, EventArgs e)
    {
        if (txtCustomerName.Text != "")
        {
            string CustomerId = objContact.GetContactIdByContactName(txtCustomerName.Text.Split('(')[0].ToString());

            DataTable dt = ObjCoustmer.GetCustomerHeaderDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);
            UcContactList.fillHeader(dt);
            UcContactList.fillFollowupList(CustomerId);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
        }
        else
        {
            DisplayMessage("Please Add Customer");
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);
        if (dt != null)
        {
            string[] str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Email_Id"].ToString();
            }
            return str;
        }
        return null;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        if (dt != null)
        {
            string[] str = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
            }
            return str;
        }
        return null;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);
        if (dt != null)
        {
            string[] str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
            }
            return str;
        }
        else
        {
            return null;
        }

    }
    #endregion

    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();

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
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }
    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId, Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
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

    #region Print
    protected void btnShowProductReport_Click(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            txtFromDate.Focus();
            return;
        }

        txtCustomerName_TextChanged(sender, e);
        string strCurrencyId = string.Empty;
        string strCurrencyType = string.Empty;
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

        //For Account Information
        string strReceiveVoucherAcc = string.Empty;
        DateTime dtToDate = new DateTime();

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

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

        //Check Record
        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, strCurrencyType);
        if (dtStatement.Rows.Count > 0)
        {
            if (ddlCVoucherType.SelectedValue != "--Select--")
            {
                dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlCVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                if (dtStatement.Rows.Count > 0)
                {

                }
                else
                {
                    DisplayMessage("You Have No Record According to Criteria");
                    txtFromDate.Focus();
                    return;
                }
            }
            else
            {

            }
        }
        else
        {
            DisplayMessage("You Have No Record According to Criteria");
            txtFromDate.Focus();
            return;
        }

        btnGetReport_Click(null, null);
        //for Ageing Data
        string strAgeingReport = string.Empty;
        if (hdnAgeing.Value != "0")
        {
            if (hdnAgeing.Value == "True")
            {
                strAgeingReport = "True";
            }
            else if (hdnAgeing.Value == "0")
            {
                strAgeingReport = "False";
            }
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(txtCustomerName.Text.Split('/')[1].ToString());
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strLocationId);
        if (txtOpeningBalance.Text != "")
        {
            objArr.Add(txtOpeningBalance.Text);
        }
        else
        {
            objArr.Add("0.00");
        }

        objArr.Add(strReceiveVoucherAcc);
        objArr.Add(strAgeingReport);
        objArr.Add(txt0to30.Text);
        objArr.Add(txt31to60.Text);
        objArr.Add(txt61to90.Text);
        objArr.Add(txt91to180.Text);
        objArr.Add(txt181to365.Text);
        objArr.Add(txtAbove365.Text);
        objArr.Add(ddlCVoucherType.SelectedValue);
        objArr.Add(strCurrencyType);
        objArr.Add(strCurrencyId);
        objArr.Add(strSta);

        Session["dtAcCParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/CustomerStatementWithProductDetail.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "reloadtblData()", true);
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        int fyear_id = 0;
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(), Session["CompId"].ToString());

        if (fyear_id == 0)
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;

        }

        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            txtFromDate.Focus();
            return;
        }

        txtCustomerName_TextChanged(sender, e);
        string strCurrencyId = string.Empty;
        string strCurrencyType = string.Empty;
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

        //For Account Information
        string strReceiveVoucherAcc = string.Empty;
        DateTime dtToDate = new DateTime();

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

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

        //Check Record
        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strReceiveVoucherAcc, txtCustomerName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, strCurrencyType);
        if (dtStatement.Rows.Count > 0)
        {
            if (ddlCVoucherType.SelectedValue != "--Select--")
            {
                dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlCVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                if (dtStatement.Rows.Count > 0)
                {

                }
                else
                {
                    DisplayMessage("You Have No Record According to Criteria");
                    txtFromDate.Focus();
                    return;
                }
            }
            else
            {

            }
        }
        else
        {
            DisplayMessage("You Have No Record According to Criteria");
            txtFromDate.Focus();
            return;
        }

        btnGetReport_Click(null, null);
        //for Ageing Data
        string strAgeingReport = string.Empty;
        if (hdnAgeing.Value != "0")
        {
            if (hdnAgeing.Value == "True")
            {
                strAgeingReport = "True";
            }
            else if (hdnAgeing.Value == "0")
            {
                strAgeingReport = "False";
            }
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(txtCustomerName.Text.Split('/')[1].ToString());
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strLocationId);
        if (txtOpeningBalance.Text != "")
        {
            objArr.Add(txtOpeningBalance.Text);
        }
        else
        {
            objArr.Add("0.00");
        }

        objArr.Add(strReceiveVoucherAcc);
        objArr.Add(strAgeingReport);
        objArr.Add(txt0to30.Text);
        objArr.Add(txt31to60.Text);
        objArr.Add(txt61to90.Text);
        objArr.Add(txt91to180.Text);
        objArr.Add(txt181to365.Text);
        objArr.Add(txtAbove365.Text);
        objArr.Add(ddlCVoucherType.SelectedValue);
        objArr.Add(strCurrencyType);
        objArr.Add(strCurrencyId);
        objArr.Add(strSta);

        Session["dtAcCParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/CustomerStatement.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "reloadtblData()", true);
    }
    protected void btnAllCustomer_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

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


        //For Account Parameter
        string strCashAccount = string.Empty;
        string strCreditAccount = string.Empty;
        string strReceiveVoucherAcc = string.Empty;

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCash.Rows.Count > 0)
        {
            strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Sales Invoice'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

        string strAllCutomerId = string.Empty;

        string Asql = "(SELECT STUFF((SELECT Distinct ',' + RTRIM(Other_Account_No) FROM Ac_Voucher_Detail where Account_No='" + strReceiveVoucherAcc + "' and Other_Account_No <>0  FOR XML PATH('')),1,1,'') )";
        DataTable dt = da.return_DataTable(Asql);
        if (dt.Rows.Count > 0)
        {
            strAllCutomerId = dt.Rows[0][0].ToString();
        }



        DataTable dtStatement = objVoucherDetail.GetStatementDetail();
        //dtStatement = new DataView(dtStatement, "Post='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (strLocationId != "" && strLocationId != "0")
        {
            dtStatement = new DataView(dtStatement, "Location_Id in (" + strLocationId + ") and  Other_Account_No in (" + strAllCutomerId + ") and Account_No in ('" + strCashAccount + "','" + strCreditAccount + "','" + strReceiveVoucherAcc + "')", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtStatement.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(strLocationId);


        Session["dtAllCParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AllCustomerDetail.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
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

        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(tblTransId.Value.ToString());

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

            string strToLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
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
            string strCurrency = Session["LocCurrencyId"].ToString(); ;
            GvDetail.DataSource = null;
            GvDetail.DataBind();
            DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, tblTransId.Value.ToString());
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
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");


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
                    lblgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
                }

                Label lblDebitTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvDebitTotal"));
                Label lblCreditTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvCreditTotal"));

                lblDebitTotal.Text = sumDebit.ToString();
                lblCreditTotal.Text = sumCredit.ToString();
                lblCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblCreditTotal.Text);
                lblDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDebitTotal.Text);
            }
        }

        //pnl1.Visible = true;
        //pnl2.Visible = true;
        //ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Voucher_Detail_Popup()", true);

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
            DataTable dtLocName = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), strLocationId);
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



    protected void btnAgeingData_Click(object sender, EventArgs e)
    {
        ViewState["CustomerAddressForStatement"] = null;
        ViewState["EmailFooterForStatement"] = null;
        ViewState["DtRvInvoiceStatement"] = null;
        getAgeingStatement();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Invoice_Statement_Popup()", true);
    }

    protected void getAgeingStatement()
    {
        string strCurrencyId = Session["LocCurrencyId"].ToString();


        string strLocationId = string.Empty;
        if (lstLocationSelect.Items.Count > 0)
        {
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
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }


        int otherAccountId = 0;
        int.TryParse(txtCustomerName.Text.Split('/')[1].ToString(), out otherAccountId);
        HDFcurrentCustomerID.Value = otherAccountId.ToString();
        string strCustomerId = otherAccountId.ToString();
        DataTable dt = new DataTable();
        if (ViewState["DtRvInvoiceStatement"] == null)
        {
            AccountsDataset ObjAccountDataset = new AccountsDataset();
            ObjAccountDataset.EnforceConstraints = false;
            AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement, Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", strCustomerId, (Convert.ToDateTime(txtToDate.Text)).Date);
            ViewState["DtRvInvoiceStatement"] = ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement;
        }

        dt = (DataTable)ViewState["DtRvInvoiceStatement"];

        int CurrencydecimalCount = 0;
        int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(strCurrencyId).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

        //set ageing days detail in footer of the print
        Ac_Ageing_Detail.clsAgeingDaysDetail clsAging = new Ac_Ageing_Detail.clsAgeingDaysDetail();
        clsAging = ObjAgeingDetail.getAgingDayDetail(dt, CurrencydecimalCount == 0 ? 2 : CurrencydecimalCount);

        string strCompanyName = string.Empty;
        string strCompanyLogoUrl = Session["CompanyLogoUrl"].ToString();
        strCompanyName = Session["CompName"].ToString();


        objCustomerAgeingStatement.DataSource = dt;
        ViewState["DtRvInvoiceStatement"] = dt;
        objCustomerAgeingStatement.DataMember = "sp_Ac_InvoiceAgeingStatement";

        //ReportToolbar1.ReportViewer = ReportViewer1;
        objCustomerAgeingStatement.setcompanyname(strCompanyName);
        objCustomerAgeingStatement.SetImage(strCompanyLogoUrl);
        objCustomerAgeingStatement.setStatementDate(txtToDate.Text);
        string strEmailFooter = string.Empty;
        string strCustomerAddress = string.Empty;
        try
        {
            if (ViewState["CustomerAddressForStatement"] == null)
            {
                ViewState["CustomerAddressForStatement"] = da.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Contact' or Set_AddressChild.Add_Type = 'Customer') AND ac_accountMaster.trans_id = " + strCustomerId + ")");
            }
            if (ViewState["EmailFooterForStatement"] == null)
            {
                ViewState["EmailFooterForStatement"] = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer").Rows[0]["Param_Value"].ToString();
            }
        }
        catch
        {

        }
        strCustomerAddress = ViewState["CustomerAddressForStatement"] == null ? "" : ViewState["CustomerAddressForStatement"].ToString();
        strEmailFooter = ViewState["EmailFooterForStatement"] == null ? "" : ViewState["EmailFooterForStatement"].ToString();

        strCustomerAddress = strCustomerAddress == "@NOTFOUND@" ? "" : strCustomerAddress;
        objCustomerAgeingStatement.setCustomerAddress(strCustomerAddress);
        objCustomerAgeingStatement.setFooterNote(strEmailFooter);
        objCustomerAgeingStatement.setAgeingDaysDetail(clsAging);
        objCustomerAgeingStatement.CreateDocument();
        ReportViewer1.OpenReport(objCustomerAgeingStatement);
    }
}