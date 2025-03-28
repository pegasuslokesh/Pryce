using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class SuppliersPayable_SupplierBalance : System.Web.UI.Page
{
    CompanyMaster ObjCompany = null;
    Set_Location_Department objLocDept = null;
    Set_CustomerMaster ObjCoustmer = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole =null;

    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_Suppliers objSupplier = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_ParameterMaster objAcParameter = null;
    DataAccessClass da = null;
    Common cmn = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();

    Ac_Voucher_Header objVoucherHeader = null;
    Ems_ContactMaster objContact = null;
    Ac_Finance_Year_Info objFYI = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    Ac_ChartOfAccount objCOA = null;
    CustomerAgeingEstatement objCustomerAgeingStatement = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());

        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objCustomerAgeingStatement = new CustomerAgeingEstatement(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayable/SupplierBalance.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            //AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "342").ToString() == "False")
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
            FillLocation();
            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            HDFCurrentSupplierID.Value = "0";
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        getAgeingStatement();
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        ////New Code created by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("340");
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

        //DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "308");
        //foreach (DataRow DtRow in dtAllPageCode.Rows)
        //{
        //    if (DtRow["Op_Id"].ToString() == "1")
        //    {
        //        //btnGetReport.Visible = true;
        //    }
        //}
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
        txtFromDate.Text = "";
        txtToDate.Text = "";
        GVAllSupplier.DataSource = null;
        GVAllSupplier.DataBind();

        HDFCurrentSupplierID.Value = "0";
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        Session["dt_supplier_balance"] = null;
    }
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
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

        //for Check Financial Year
        //if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text)))
        //{
        //    GVAllSupplier.DataSource = null;
        //    GVAllSupplier.DataBind();
        //    DisplayMessage("Selected date should be belongs to login financial year");
        //     return;
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

        //for Check Financial Year
        //if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtToDate.Text)))
        //{
        //    GVAllSupplier.DataSource = null;
        //    GVAllSupplier.DataBind();
        //    DisplayMessage("Selected date should be belongs to login financial year ");
        //    return;
        //}

        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
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
            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                strCurrencyId = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }
        HDFCurrencyID.Value = strCurrencyId;
        DateTime dtToDate = new DateTime();
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            return;
        }

        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtFromDate.Focus();
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


        //for Check Financial Year neelkanth purohit - 18/09/2018
        int fyear_id = 0;
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(),Session["CompId"].ToString());
        if (fyear_id == 0)
        {
            GVAllSupplier.DataSource = null;
            GVAllSupplier.DataBind();
            DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
            return;
        }

        //For Account Information
        string strAccountNumber = string.Empty;
        string strCashAccount = string.Empty;
        string strCreditAccount = string.Empty;
        string strPaymentVoucherAcc = string.Empty;

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCash.Rows.Count > 0)
        {
            strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
        }

        strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        strAccountNumber = strPaymentVoucherAcc;
        //End


        DataTable dtAllSupplierData = new DataTable();
        if (strFlag == "True")
        {
            dtAllSupplierData = objVoucherDetail.GetAllSupplierBalanceData(StrCompId, StrBrandId, strLocationId, strAccountNumber, strPaymentVoucherAcc, txtFromDate.Text, dtToDate.ToString(), "1", fyear_id.ToString());
        }
        else if (strFlag == "False")
        {
            dtAllSupplierData = objVoucherDetail.GetAllSupplierBalanceData(StrCompId, StrBrandId, strLocationId, strAccountNumber, strPaymentVoucherAcc, txtFromDate.Text, dtToDate.ToString(), "2", fyear_id.ToString());
        }

        if (dtAllSupplierData.Rows.Count > 0)
        {
            dtAllSupplierData = new DataView(dtAllSupplierData, "Name not is null", "", DataViewRowState.CurrentRows).ToTable();

            if (chkShowZero.Checked == false)
            {
                dtAllSupplierData = new DataView(dtAllSupplierData, "Opening_Final <>'0' or Debit_Final<>'0' or Credit_Final<> '0' or Closing_Final<>'0'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtAllSupplierData.Rows.Count > 0)
        {
            GVAllSupplier.DataSource = dtAllSupplierData;
            GVAllSupplier.DataBind();
            Session["dt_supplier_balance"] = dtAllSupplierData;
        }
        else
        {
            GVAllSupplier.DataSource = null;
            GVAllSupplier.DataBind();
            DisplayMessage("You have no record available");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
            Session["dt_supplier_balance"] = null;
        }

        fillGrid();

    }
    protected void fillGrid()
    {
        double OpeningBalance = 0;
        double DebitAmount = 0;
        double CreditAmount = 0;
        double ClosingBalance = 0;
        double AgeingBalance = 0;

        double OpeningBalanceTotal = 0;
        double debitTotalamount = 0;
        double creditTotalamount = 0;
        double ClosingBalanceTotal = 0;
        double AgeingBalanceTotal = 0;

        string strCurrency = Session["LocCurrencyId"].ToString(); ;
        foreach (GridViewRow gvr in GVAllSupplier.Rows)
        {
            HiddenField hdngvSupplierId = (HiddenField)gvr.FindControl("hdnSupplierId");
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");
            Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
            //Label lblgvBalance = (Label)gvr.FindControl("lblgvClosingBalance");
            LinkButton lnkAcStatement = (LinkButton)gvr.FindControl("lnkAccountStatement");
            Label lblgvForeign = (Label)gvr.FindControl("lblgvForeignClosingBalance");
            LinkButton lnkInvoiceStatement = (LinkButton)gvr.FindControl("lnkInvoiceStatement");
            HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdn_currency_id");
            HiddenField hdnCurrencyCode = (HiddenField)gvr.FindControl("hdn_currency_code");
            OpeningBalance = Convert.ToDouble(lblgvOpeningBalance.Text);
            DebitAmount = Convert.ToDouble(lblgvDebitAmount.Text);
            CreditAmount = Convert.ToDouble(lblgvCreditAmount.Text);
            ClosingBalance = Convert.ToDouble(lnkAcStatement.Text);
            AgeingBalance = Convert.ToDouble(lnkInvoiceStatement.Text);

            lblgvOpeningBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
            lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
            lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
            lnkAcStatement.Text = objsys.GetCurencyConversionForInv(strCurrency, lnkAcStatement.Text);
            lnkInvoiceStatement.Text = objsys.GetCurencyConversionForInv(strCurrency, lnkInvoiceStatement.Text);

            OpeningBalanceTotal += OpeningBalance;
            debitTotalamount += DebitAmount;
            creditTotalamount += CreditAmount;
            ClosingBalanceTotal += ClosingBalance;
            AgeingBalanceTotal += AgeingBalance;

            lblgvForeign.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, lblgvForeign.Text);
            lblgvForeign.Text = "(" + hdnCurrencyCode.Value + ") " + lblgvForeign.Text;
        }

        if (GVAllSupplier.Rows.Count > 0)
        {
            Label lblgvOpeningBalanceTotal = (Label)GVAllSupplier.FooterRow.FindControl("lblgvOpeningBalanceTotal");
            Label lblgvDebitAmountTotal = (Label)GVAllSupplier.FooterRow.FindControl("lblgvDebitAmountTotal");
            Label lblgvCreditAmountTotal = (Label)GVAllSupplier.FooterRow.FindControl("lblgvCreditAmountTotal");
            Label lblgvClosingBalanceTotal = (Label)GVAllSupplier.FooterRow.FindControl("lblgvClosingBalanceTotal");
            Label lblgvAgeingBalanceTotal = (Label)GVAllSupplier.FooterRow.FindControl("lblgvAgeingBalanceTotal");

            lblgvOpeningBalanceTotal.Text = OpeningBalanceTotal.ToString();
            lblgvOpeningBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalanceTotal.Text);
            lblgvDebitAmountTotal.Text = debitTotalamount.ToString();
            lblgvDebitAmountTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmountTotal.Text);
            lblgvCreditAmountTotal.Text = creditTotalamount.ToString();
            lblgvCreditAmountTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmountTotal.Text);
            lblgvClosingBalanceTotal.Text = ClosingBalanceTotal.ToString();
            lblgvClosingBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvClosingBalanceTotal.Text);
            lblgvAgeingBalanceTotal.Text = AgeingBalanceTotal.ToString();
            lblgvAgeingBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvAgeingBalanceTotal.Text);
            //GVAllSupplier_RowCreated(object sender, GridViewRowEventArgs e);
            //string strText = GVAllSupplier.HeaderRow.Cells[0].Text;
            //GVAllSupplier.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(HDFCurrencyID.Value.ToString(), GVAllSupplier.HeaderRow.Cells[4].Text);
            //GVAllSupplier.HeaderRow.Cells[6].Text = SystemParameter.GetCurrencySmbol(HDFCurrencyID.Value.ToString(), GVAllSupplier.HeaderRow.Cells[6].Text);
        }
        else
        {
            DisplayMessage("You have no record available");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
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
    public void DisplayMessage(string str,string color="orange")
    {

        //displayMessage
        ScriptManager.RegisterStartupScript(this, GetType(), "", "displayMessage('" + str + "')", true);
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId,Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }

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

        if (!Common.GetStatus(Session["CompId"].ToString()))
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

    #region Print
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text),Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVAllSupplier.DataSource = null;
            GVAllSupplier.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

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

            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                strCurrencyId = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }

        //For Date
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            return;
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtFromDate.Focus();
            return;
        }

        DateTime dtToDate = new DateTime();
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

        ArrayList objArr = new ArrayList();
        objArr.Add(strLocationId);
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        if (chkShowZero.Checked == true)
        {
            objArr.Add("True");
        }
        else if (chkShowZero.Checked == false)
        {
            objArr.Add("False");
        }
        objArr.Add(strFlag);
        objArr.Add(strCurrencyId);

        Session["dtAllSupParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AllSupplierDetail.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    protected void GVAllSupplier_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["dt_supplier_balance"] == null)
        {
            return;
        }
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dt_supplier_balance"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dt_supplier_balance"] = dt;
        GVAllSupplier.DataSource = dt;
        GVAllSupplier.DataBind();
        fillGrid();
        //AllPageCode();
    }

    protected void lnkAccountStatement_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string SupplierId = arguments[0];


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


        if (SupplierId != "0" && SupplierId != "")
        {
            if (IsObjectPermission("162", "308"))
            {
                Session["SupID"] = SupplierId;
                Session["From"] = txtFromDate.Text;
                Session["To"] = txtToDate.Text;
                Session["SupLocId"] = strLocationId;
                string strCmd = string.Format("window.open('../SuppliersPayable/SupplierStatement.aspx?Id=" + SupplierId + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else
            {
                DisplayMessage("You have no permission for view detail");
                return;
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
    protected void lnkInvoiceStatement_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] args = myButton.CommandArgument.ToString().Split(',');
        HDFCurrentSupplierID.Value = args[0];
        HDFCurrencyID.Value = args[1];
        ViewState["SupplierAddressForStatement"] = null;
        //ViewState["EmailFooterForStatement"] = null;
        ViewState["DtPvInvoiceStatement"] = null;
        getAgeingStatement();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Invoice_Statement_Popup()", true);
    }
    protected void getAgeingStatement()
    {
        if (HDFCurrentSupplierID.Value == "0")
        {
            return;
        }

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

        string strSupplierId = HDFCurrentSupplierID.Value;
        DataTable dt = new DataTable();
        if (ViewState["DtPvInvoiceStatement"] == null)
        {
            AccountsDataset ObjAccountDataset = new AccountsDataset();
            ObjAccountDataset.EnforceConstraints = false;
            AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement, Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", strSupplierId, (Convert.ToDateTime(txtToDate.Text)).Date);
            ViewState["DtPvInvoiceStatement"] = ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement;
        }

        dt = (DataTable)ViewState["DtPvInvoiceStatement"];

        string strCompanyName = string.Empty;
        string strCompanyLogoUrl = Session["CompanyLogoUrl"].ToString();
        strCompanyName = Session["CompName"].ToString();

        int CurrencydecimalCount = 0;
        int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(HDFCurrencyID.Value).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

        //set ageing days detail in footer of the print
        Ac_Ageing_Detail.clsAgeingDaysDetail clsAging = new Ac_Ageing_Detail.clsAgeingDaysDetail();
        clsAging = ObjAgeingDetail.getAgingDayDetail(dt, CurrencydecimalCount == 0 ? 2 : CurrencydecimalCount);

        objCustomerAgeingStatement.DataSource = dt;
        ViewState["DtPvInvoiceStatement"] = dt;
        objCustomerAgeingStatement.DataMember = "sp_Ac_InvoiceAgeingStatement";

        //ReportToolbar1.ReportViewer = ReportViewer1;
        objCustomerAgeingStatement.setcompanyname(strCompanyName);
        objCustomerAgeingStatement.SetImage(strCompanyLogoUrl);
        objCustomerAgeingStatement.setStatementDate(txtToDate.Text);
        objCustomerAgeingStatement.setAgeingDaysDetail(clsAging);
        string strEmailFooter = string.Empty;
        string strCustomerAddress = string.Empty;
        try
        {
            if (ViewState["CustomerAddressForStatement"] == null)
            {
                ViewState["CustomerAddressForStatement"] = da.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Supplier' or Set_AddressChild.Add_Type = 'Supplier') AND ac_accountMaster.trans_id = " + HDFCurrentSupplierID.Value + ")");
            }
            if (ViewState["EmailFooterForStatement"] == null)
            {
                //ViewState["EmailFooterForStatement"] = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer").Rows[0]["Param_Value"].ToString();
            }
        }
        catch
        { }
        strCustomerAddress = ViewState["CustomerAddressForStatement"] == null ? "" : ViewState["CustomerAddressForStatement"].ToString();
        strEmailFooter = ViewState["EmailFooterForStatement"] == null ? "" : ViewState["EmailFooterForStatement"].ToString();

        strCustomerAddress = strCustomerAddress == "@NOTFOUND@" ? "" : strCustomerAddress;
        objCustomerAgeingStatement.setCustomerAddress(strCustomerAddress);
        objCustomerAgeingStatement.setFooterNote(strEmailFooter);
        objCustomerAgeingStatement.CreateDocument();
        ReportViewer1.OpenReport(objCustomerAgeingStatement);
    }

    protected void lnkgvSupplier_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string SupplierId = arguments[0];
        //DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);
        DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), SupplierId);
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(SupplierId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_ContactInfo_Open()", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return str;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return str;
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
}