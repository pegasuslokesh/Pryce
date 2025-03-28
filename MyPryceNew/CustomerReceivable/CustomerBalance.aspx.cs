using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Text.RegularExpressions;
using System.Threading;

public partial class CustomerReceivable_CustomerBalance : System.Web.UI.Page
{
    Set_Location_Department objLocDept = null;
    Set_CustomerMaster ObjCoustmer = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Ac_Parameter_Location objAcParamLocation = null;
    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_Suppliers objSupplier = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_ParameterMaster objAcParameter = null;
    DataAccessClass da = null;
    CompanyMaster ObjCompany = null;
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
    Ac_EmailLog objAcEmailLog = null;
    FollowUp FollowupClass = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objAcParamLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objCustomerAgeingStatement = new CustomerAgeingEstatement(Session["DBConnection"].ToString());
        objAcEmailLog = new Ac_EmailLog(Session["DBConnection"].ToString());
        FollowupClass = new FollowUp(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/CustomerBalance.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            //AllPageCode(clsPagePermission);

            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();
            txtFromDate.Text = DateTime.Now.Date.ToString(Calender_VoucherDate.Format);
            txtToDate.Text = DateTime.Now.Date.ToString(Calender_VoucherDate.Format);
            FillLocation();

            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            HDFcurrentCustomerID.Value = "0";
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        getAgeingStatement();
        if (hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    public void AllPageCode()
    {
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
        GVAllCustomer.DataSource = null;
        GVAllCustomer.DataBind();
        HDFcurrentCustomerID.Value = "0";
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
    }
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        HDFcurrentCustomerID.Value = "0";
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

        //for Check Financial Year
        int fyear_id = 0;
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(), Session["CompId"].ToString());

        if (fyear_id == 0)
        {
            GVAllCustomer.DataSource = null;
            GVAllCustomer.DataBind();
            DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
            return;
        }

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

        //For Account Information
        string strAccountNumber = string.Empty;
        string strPaymentVoucherAcc = string.Empty;

        strPaymentVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        strAccountNumber = strPaymentVoucherAcc;
        //End

        DataTable dtAllCustomerData = new DataTable();
        if (strFlag == "True")
        {
            dtAllCustomerData = objVoucherDetail.GetAllCustomerBalanceData(StrCompId, StrBrandId, strLocationId, strAccountNumber, strAccountNumber, txtFromDate.Text, txtToDate.Text, "1", Session["FinanceYearId"].ToString());
        }
        else if (strFlag == "False")
        {
            dtAllCustomerData = objVoucherDetail.GetAllCustomerBalanceData(StrCompId, StrBrandId, strLocationId, strAccountNumber, strPaymentVoucherAcc, txtFromDate.Text, dtToDate.ToString(), "2", Session["FinanceYearId"].ToString());
        }

        if (dtAllCustomerData.Rows.Count > 0)
        {
            dtAllCustomerData = new DataView(dtAllCustomerData, "Name not is null", "", DataViewRowState.CurrentRows).ToTable();

            if (chkShowZero.Checked == false)
            {
                dtAllCustomerData = new DataView(dtAllCustomerData, "Closing_Balance<>0", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        dtAllCustomerData = dtAllCustomerData.DefaultView.ToTable( /*distinct*/ true);

        if (dtAllCustomerData.Rows.Count > 0)
        {
            GVAllCustomer.DataSource = dtAllCustomerData;
            GVAllCustomer.DataBind();
            Session["dt_customer_balance"] = dtAllCustomerData;
        }
        else
        {
            GVAllCustomer.DataSource = null;
            GVAllCustomer.DataBind();
            DisplayMessage("You have no record available");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
            Session["dt_customer_balance"] = null;
            return;
        }
        fillGrid();
    }
    protected void fillGrid()
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        foreach (GridViewRow gvr in GVAllCustomer.Rows)
        {
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");
            Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
            LinkButton lblgvBalance = (LinkButton)gvr.FindControl("lnkClosingBalance");
            LinkButton lblgvDays30 = (LinkButton)gvr.FindControl("lnkInvoiceStatement");
            LinkButton lnkDueBalance = (LinkButton)gvr.FindControl("lnkDueBalance");

            lblgvOpeningBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
            lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
            lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
            lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
            lblgvDays30.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDays30.Text);
            lnkDueBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lnkDueBalance.Text);
        }

        // for total Record
        //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        double OpeningTotal = 0;
        double DebitTotal = 0;
        double CreditTotal = 0;
        double balanceTotal = 0;
        foreach (GridViewRow gvr in GVAllCustomer.Rows)
        {
            Label lblgvOpeningBalance = (Label)gvr.FindControl("lblgvOpeningBalance");
            Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
            LinkButton lblgvClosingBalance = (LinkButton)gvr.FindControl("lnkClosingBalance");
            lblgvOpeningBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvOpeningBalance.Text);
            lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
            lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
            lblgvClosingBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvClosingBalance.Text);


            OpeningTotal += Convert.ToDouble(lblgvOpeningBalance.Text);
            DebitTotal += Convert.ToDouble(lblgvDebitAmount.Text);
            CreditTotal += Convert.ToDouble(lblgvCreditAmount.Text);
            balanceTotal += Convert.ToDouble(lblgvClosingBalance.Text);
        }

        if (GVAllCustomer.Rows.Count > 0)
        {
            Label lblOpeningTotal = (Label)GVAllCustomer.FooterRow.FindControl("lblOpeningTotal");
            Label lblDebitTotal = (Label)GVAllCustomer.FooterRow.FindControl("lblDebitTotal");
            Label lblCreditTotal = (Label)GVAllCustomer.FooterRow.FindControl("lblCreditTotal");
            Label lblbalanceTotal = (Label)GVAllCustomer.FooterRow.FindControl("lblbalanceTotal");

            lblOpeningTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, OpeningTotal.ToString());
            lblDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, DebitTotal.ToString());
            lblCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, CreditTotal.ToString());
            lblbalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, balanceTotal.ToString());
            //GVAllCustomer.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVAllCustomer.HeaderRow.Cells[4].Text);
            //GVAllCustomer.HeaderRow.Cells[5].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVAllCustomer.HeaderRow.Cells[5].Text);
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
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "')", true);
    }

    protected void lnkgvCustomer_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        DataTable dt = ObjCoustmer.GetCustomerHeaderDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(CustomerId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
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



        btnGetReport_Click(null, null);

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

        Session["dtAllCusParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AllCustomerDetail.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnRepotWithSales_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVAllCustomer.DataSource = null;
            GVAllCustomer.DataBind();
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

        btnGetReport_Click(null, null);

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

        Session["dtAllCusParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AllCustomerwithSalesPerson.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    protected void GVAllCustomer_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["dt_customer_balance"] == null)
        {
            return;
        }
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dt_customer_balance"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dt_customer_balance"] = dt;
        GVAllCustomer.DataSource = dt;
        GVAllCustomer.DataBind();
        fillGrid();
        //AllPageCode();
    }

    protected void lnkInvoiceStatement_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] args = myButton.CommandArgument.ToString().Split(',');
        HDFcurrentCustomerID.Value = args[0];
        HDFCurrencyID.Value = args[1];
        ViewState["CustomerAddressForStatement"] = null;
        ViewState["EmailFooterForStatement"] = null;
        ViewState["DtRvInvoiceStatement"] = null;
        getAgeingStatement();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Invoice_Statement_Popup()", true);
    }
    protected void getAgeingStatement()
    {
        if (HDFcurrentCustomerID.Value == "0")
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

        string strCustomerId = HDFcurrentCustomerID.Value;
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
        int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(HDFCurrencyID.Value).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

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
                ViewState["CustomerAddressForStatement"] = da.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Contact' or Set_AddressChild.Add_Type = 'Customer') AND ac_accountMaster.trans_id = " + HDFcurrentCustomerID.Value + ")");
            }
            if (ViewState["EmailFooterForStatement"] == null)
            {
                ViewState["EmailFooterForStatement"] = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer").Rows[0]["Param_Value"].ToString();
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
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkHeader = (CheckBox)GVAllCustomer.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in GVAllCustomer.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    protected void lnkClosingBalance_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];

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


        if (CustomerId != "0" && CustomerId != "")
        {
            if (IsObjectPermission("162", "308"))
            {
                Session["CusID"] = CustomerId;
                Session["CusterStatementFromDate"] = txtFromDate.Text;
                Session["CustomerStatementToDate"] = txtToDate.Text;
                Session["CustomerStatementLocations"] = strLocationId;
                string strCmd = string.Format("window.open('../CustomerReceivable/CustomerStatement.aspx?Id=" + CustomerId + "','window','width=1024, ');");
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
    protected string getFinancePersonEmail(string strFinancePerson)
    {
        string strEmail = string.Empty;
        try
        {
            string[] strArray = strFinancePerson.Split('(');
            strEmail = strArray[1].Substring(0, strArray[1].Length - 1).Trim();
        }
        catch
        {

        }
        return strEmail;
    }
    protected void btnsendMail_Click(object sender, EventArgs e)
    {
        DataTable dtExistingEmailLog = objAcEmailLog.getAcEmailLogByStatementDate(Session["LocId"].ToString(), "RV", txtToDate.Text);
        string strErrorLog = string.Empty;
        string strRecentInsertedRecord = string.Empty; //comma saperated value of tran_d field
        DataTable _dtConfirmEmail = getBlankCustomerEmailTable();
        foreach (GridViewRow dr in GVAllCustomer.Rows)
        {
            CheckBox chkChecked = (CheckBox)dr.FindControl("chkgvSelect");
            LinkButton lnkInvoiceBalance = (LinkButton)dr.FindControl("lnkInvoiceStatement");
            Label lblCustomerId = (Label)dr.FindControl("lblCustomerId");
            Label lblOtherAccountId = (Label)dr.FindControl("lblOtherAccountId");
            LinkButton lnkCustomerName = (LinkButton)dr.FindControl("lnkgvSupplier");
            if (chkChecked.Checked == true)
            {
                DataRow[] drLog = dtExistingEmailLog.Select("customer_id=" + lblCustomerId.Text + " and field3='" + lblOtherAccountId.Text + "' and statement_date='" + txtToDate.Text + "'");
                if (drLog.Length > 0)
                {
                    strErrorLog = strErrorLog + System.Environment.NewLine + "Email Already exit in Log for " + lnkCustomerName.Text + "(" + txtToDate.Text + ")";
                    chkChecked.BackColor = System.Drawing.Color.Red;
                    chkChecked.ToolTip = "Email Already exit in Log";
                    continue;
                }
                else
                {
                    Label lblEmails = (Label)dr.FindControl("lblFinancePersonEmail");
                    if (lblEmails.Text != string.Empty && lnkInvoiceBalance.Text != "0")
                    {
                        string strValidEmail = string.Empty;
                        string[] strEmails = lblEmails.Text.Trim().Split(',');
                        if (strEmails.Length > 1)
                        {
                            foreach (string str in strEmails)
                            {
                                if (IsValidEmailId(str))
                                {
                                    strValidEmail = strValidEmail + "," + str;
                                }

                            }

                        }
                        else
                        {
                            if (IsValidEmailId(strEmails[0]))
                            {
                                strValidEmail = strEmails[0];
                            }
                        }
                        if (strValidEmail != string.Empty)
                        {
                            DataRow drEmail = _dtConfirmEmail.Rows.Add();
                            drEmail["customer_id"] = lblCustomerId.Text;
                            drEmail["customer_name"] = lnkCustomerName.Text;
                            drEmail["other_account_id"] = lblOtherAccountId.Text;
                            drEmail["ageing_balance"] = lnkInvoiceBalance.Text;
                            drEmail["email"] = strValidEmail;
                        }

                    }
                    else
                    {
                        strErrorLog = strErrorLog + System.Environment.NewLine + "There is no email address information or may be balance is 0 for " + lnkCustomerName.Text;
                        chkChecked.ToolTip = "There is no email address information or may be balance is 0";
                        chkChecked.Checked = false;
                        chkChecked.BackColor = System.Drawing.Color.Red;
                    }

                }
            }
        }
        if (_dtConfirmEmail.Rows.Count > 0)
        {
            GvSelectedEmailList.DataSource = _dtConfirmEmail;
            GvSelectedEmailList.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Email_Open();", true);
        }
        else
        {
            //if (strErrorLog!=string.Empty)
            //{
            //    DisplayMessage(strErrorLog);
            //}
            //else
            //{
            DisplayMessage("There is no valid record");
            //}

        }

    }
    protected void btnsendMail_Click_1(object sender, EventArgs e)
    {
        DataTable dtExistingEmailLog = objAcEmailLog.getAcEmailLogByStatementDate(Session["LocId"].ToString(), "RV", txtToDate.Text);
        string strErrorLog = string.Empty;
        string strRecentInsertedRecord = string.Empty; //comma saperated value of tran_d field
        foreach (GridViewRow dr in GVAllCustomer.Rows)
        {
            CheckBox chkChecked = (CheckBox)dr.FindControl("chkgvSelect");
            LinkButton lnkInvoiceBalance = (LinkButton)dr.FindControl("lnkInvoiceStatement");
            Label lblCustomerId = (Label)dr.FindControl("lblCustomerId");
            Label lblOtherAccountId = (Label)dr.FindControl("lblOtherAccountId");
            LinkButton lnkCustomerName = (LinkButton)dr.FindControl("lnkgvSupplier");
            if (chkChecked.Checked == true)
            {
                DataRow[] drLog = dtExistingEmailLog.Select("customer_id=" + lblCustomerId.Text + " and field3='" + lblOtherAccountId.Text + "' and statement_date='" + txtToDate.Text + "'");
                if (drLog.Length > 0)
                {
                    strErrorLog = strErrorLog + System.Environment.NewLine + "Email Already exit in Log for " + lnkCustomerName.Text + "(" + txtToDate.Text + ")";
                    chkChecked.BackColor = System.Drawing.Color.Red;
                    continue;
                }
                else
                {
                    Label lblEmails = (Label)dr.FindControl("lblFinancePersonEmail");
                    if (lblEmails.Text != string.Empty && lnkInvoiceBalance.Text != "0")
                    {
                        string strValidEmail = string.Empty;
                        string[] strEmails = lblEmails.Text.Trim().Split(',');
                        if (strEmails.Length > 1)
                        {
                            foreach (string str in strEmails)
                            {
                                if (IsValidEmailId(str))
                                {
                                    strValidEmail = strValidEmail + "," + str;
                                }

                            }

                        }
                        else
                        {
                            if (IsValidEmailId(strEmails[0]))
                            {
                                strValidEmail = strEmails[0];
                            }
                        }
                        if (strValidEmail != string.Empty)
                        {
                            int b = objAcEmailLog.InsertAcEmailLog(strLocationId, DateTime.Now.ToString(), lblCustomerId.Text, "RV", txtToDate.Text, strValidEmail, lnkCustomerName.Text + " Outstanding SOA", "Please find attached invoice statement", "Pending", "01-Jan-1900", "No exception", Session["LocId"].ToString(), "", lblOtherAccountId.Text, "", "", false.ToString(), "01-Jan-1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            if (b != 0)
                            {
                                if (strRecentInsertedRecord == string.Empty)
                                {
                                    strRecentInsertedRecord = strRecentInsertedRecord + b.ToString();
                                }
                                else
                                {
                                    strRecentInsertedRecord = strRecentInsertedRecord + "," + b.ToString();
                                }
                            }
                        }
                        else
                        {
                            strErrorLog = strErrorLog + System.Environment.NewLine + "There is invalid email address for " + lnkCustomerName.Text;
                            chkChecked.BackColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {
                        strErrorLog = strErrorLog + System.Environment.NewLine + "There is no email address information or may be balance is 0 for " + lnkCustomerName.Text;
                        chkChecked.Checked = false;
                        chkChecked.BackColor = System.Drawing.Color.Red;
                    }
                }
            }


        }
        if (strRecentInsertedRecord != string.Empty)
        {
            DisplayMessage("Email send process for customer statement has been started in background, you will get notification after completion");
            ThreadStart ts = delegate () { new Ac_EmailSend(HttpContext.Current.Session["DBConnection"].ToString()).sendEmail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["CompName"].ToString(), Server.MapPath(Session["CompanyLogoUrl"].ToString()), Server.MapPath("~/Temp"), Session["EmpId"].ToString(), Session["UserId"].ToString(), "RV", strRecentInsertedRecord, HttpContext.Current.Session["DBConnection"].ToString()); };
            Thread t = new Thread(ts);
            t.Start();
            //objAcEmailLog.sendEmail(Session["LocId"].ToString(), "RV", strRecentInsertedRecord);
        }


    }
    private bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
    }
    protected void btnFolloup_Command(object sender, CommandEventArgs e)
    {
        Session["CustBal_CustID"] = "0";
        Session["CustBal_CustID"] = e.CommandArgument.ToString();
        FollowupUC.newBtnCall();
        //FollowupUC.fillFollowupListSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupList(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBinSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBin(hdnFollowupTableName.Value);
        FollowupUC.GetFollowupDocumentNumber();
        FollowupUC.SetGeneratedByName();
        FollowupUC.ResetFollowupType();
        DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());
        FollowupUC.fillHeader(dt, hdnFollowupTableName.Value);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Followup_Open();showNewTab();", true);

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string id = string.Empty;
            if (HttpContext.Current.Session["ContactID"] != null)
            {
                id = HttpContext.Current.Session["ContactID"].ToString();
            }
            else
            {
                id = "0";
            }
            DataTable dt_Contact = ObjContactMaster.GetContactAsPerFilterText(prefixText, id);
            string[] filterlist = new string[dt_Contact.Rows.Count];
            if (dt_Contact.Rows.Count > 0)
            {
                for (int i = 0; i < dt_Contact.Rows.Count; i++)
                {
                    filterlist[i] = dt_Contact.Rows[i]["FilterText"].ToString();
                }
                return filterlist;
            }
            else
            {
                DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
                string[] filterlistcon = new string[dtcon.Rows.Count];
                for (int i = 0; i < dtcon.Rows.Count; i++)
                {
                    filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
                }
                return filterlistcon;
            }
        }
        catch (Exception error)
        {

        }
        return null;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }

        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCallLogs(string prefixText, int count, string contextKey)
    {
        CallLogs call = new CallLogs(HttpContext.Current.Session["DBConnection"].ToString());

        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = call.GetCallLogsFor_CustomerID(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                txt[i] = dt1.Rows[i]["filteredText"].ToString();
            }
        }

        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVisitLogs(string prefixText, int count, string contextKey)
    {
        SM_WorkOrder visit = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = visit.GetWorkOrderNoPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                txt[i] = dt1.Rows[i]["Work_Order_No"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString();
            }
        }

        return txt;
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

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
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

    private DataTable getBlankCustomerEmailTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("customer_id", typeof(Int32));
        dt.Columns.Add("other_account_id", typeof(Int32));
        dt.Columns.Add("customer_name", typeof(string));
        dt.Columns.Add("email", typeof(string));
        dt.Columns.Add("ageing_balance", typeof(string));
        return dt;
    }



    protected void btnSendEmailFinal_Click(object sender, EventArgs e)
    {
        string strErrorLog = string.Empty;
        string strRecentInsertedRecord = string.Empty; //comma saperated value of tran_d field
        foreach (GridViewRow dr in GvSelectedEmailList.Rows)
        {
            Label lblBalance = (Label)dr.FindControl("lblAgeBalance");
            Label lblCustomerId = (Label)dr.FindControl("lblSelCustomerId");
            Label lblOtherAccountId = (Label)dr.FindControl("lblOtherAccountId");
            Label lblCustomerName = (Label)dr.FindControl("lblSelCustomer");
            Label lblEmail = (Label)dr.FindControl("lblEmail");
            int b = objAcEmailLog.InsertAcEmailLog(strLocationId, DateTime.Now.ToString(), lblCustomerId.Text, "RV", txtToDate.Text, lblEmail.Text, lblCustomerName.Text + " Outstanding SOA", "Please find attached invoice statement", "Pending", "01-Jan-1900", "No exception", Session["LocId"].ToString(), "", lblOtherAccountId.Text, "", "", false.ToString(), "01-Jan-1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                if (strRecentInsertedRecord == string.Empty)
                {
                    strRecentInsertedRecord = strRecentInsertedRecord + b.ToString();
                }
                else
                {
                    strRecentInsertedRecord = strRecentInsertedRecord + "," + b.ToString();
                }
            }
        }
        if (strRecentInsertedRecord != string.Empty)
        {
            DisplayMessage("Email send process for customer statement has been started in background, you will get notification after completion");
            ThreadStart ts = delegate () { new Ac_EmailSend(Session["DBConnection"].ToString()).sendEmail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["CompName"].ToString(), Server.MapPath(Session["CompanyLogoUrl"].ToString()), Server.MapPath("~/Temp"), Session["EmpId"].ToString(), Session["UserId"].ToString(), "RV", strRecentInsertedRecord, Session["DBConnection"].ToString()); };
            Thread t = new Thread(ts);
            t.Start();
            //objAcEmailLog.sendEmail(Session["LocId"].ToString(), "RV", strRecentInsertedRecord);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Email_Close();", true);
        }
    }
    //protected string GetPostBackControlId(this Page page)
    //{
    //    if (!page.IsPostBack)
    //        return string.Empty;

    //    Control control = null;
    //    // first we will check the "__EVENTTARGET" because if post back made by the controls
    //    // which used "_doPostBack" function also available in Request.Form collection.
    //    string controlName = page.Request.Params["__EVENTTARGET"];
    //    if (!String.IsNullOrEmpty(controlName))
    //    {
    //        control = page.FindControl(controlName);
    //    }
    //    else
    //    {
    //        // if __EVENTTARGET is null, the control is a button type and we need to
    //        // iterate over the form collection to find it

    //        // ReSharper disable TooWideLocalVariableScope
    //        string controlId;
    //        Control foundControl;
    //        // ReSharper restore TooWideLocalVariableScope

    //        foreach (string ctl in page.Request.Form)
    //        {
    //            // handle ImageButton they having an additional "quasi-property" 
    //            // in their Id which identifies mouse x and y coordinates
    //            if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
    //            {
    //                controlId = ctl.Substring(0, ctl.Length - 2);
    //                foundControl = page.FindControl(controlId);
    //            }
    //            else
    //            {
    //                foundControl = page.FindControl(ctl);
    //            }

    //            if (!(foundControl is IButtonControl)) continue;

    //            control = foundControl;
    //            break;
    //        }
    //    }

    //    return control == null ? String.Empty : control.ID;
    //}
}