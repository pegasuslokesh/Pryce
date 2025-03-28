using System;
using System.Collections.Generic;
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
using PegasusDataAccess;
using net.webservicex.www;
using System.Data.SqlClient;

public partial class VoucherEntries_VoucherDetailReport : System.Web.UI.Page
{
    Set_CustomerMaster ObjCoustmer = null;
    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_Suppliers objSupplier = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_ParameterMaster objAcParameter = null;
    DataAccessClass da = null;
    PageControlCommon objPageCmn = null;

    Common cmn = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();

    Ac_Voucher_Header objVoucherHeader = null;
    Ems_ContactMaster objContact = null;
    Ac_Finance_Year_Info objFYI = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    Ac_ChartOfAccount objCOA = null;


    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
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
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "339", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();

            FillLocation();
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("339", (DataTable)Session["ModuleName"]);
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

        //End Code
        Page.Title = objsys.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnGetReport.Visible = true;
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "339",Session["CompId"].ToString());
        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnGetReport.Visible = true;
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
        txtFromDate.Text = "";
        txtToDate.Text = "";

        AllRecord_Rb.Checked = true;
        EqualRecord_Rb.Checked = false;
        NotEqualRecord_Rb.Checked = false;

        GVSStatement.DataSource = null;
        GVSStatement.DataBind();
        FillLocation();

        lstLocationSelect.Items.Clear();
        lblTotalRecords.Text = "Total Records: 0";

    }
    protected void btnGetReport_Click(object sender, EventArgs e)
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

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        //For Account Information
        string strCashAccount = string.Empty;
        string strCreditAccount = string.Empty;
        string strPaymentVoucherAcc = string.Empty;
        DateTime dtToDate = new DateTime();

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

        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
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

        // string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, Session["SupplierCurrency"].ToString());
        DataTable dtStatement = new DataTable();

        if (AllRecord_Rb.Checked)
        {
            dtStatement = objVoucherDetail.GetStatementVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString());
        }
        else if (EqualRecord_Rb.Checked)
        {
            dtStatement = objVoucherDetail.GetStatementVoucherDetailEqualRecord(Session["CompId"].ToString(), Session["BrandId"].ToString());
        }
        else if (NotEqualRecord_Rb.Checked)
        {
            dtStatement = objVoucherDetail.GetStatementVoucherDetailNotEqualRecord(Session["CompId"].ToString(), Session["BrandId"].ToString());
        }


        if (dtStatement.Rows.Count != 0)
        {
            dtStatement = new DataView(dtStatement, "Location_Id in (" + strLocationId + ") ", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                dtStatement = new DataView(dtStatement, "Voucher_Date>='" + txtFromDate.Text + "' and  Voucher_Date<='" + txtToDate.Text + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtStatement.Rows.Count + "";

                GVSStatement.DataSource = dtStatement;
                GVSStatement.DataBind();
            }
            else
            {
                lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtStatement.Rows.Count + "";
                GVSStatement.DataSource = dtStatement;
                GVSStatement.DataBind();
            }
        }
        else
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtStatement.Rows.Count + "";
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
        }


        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        double debitamount = 0;
        double creditamount = 0;
        double debitTotalamount = 0;
        double creditTotalamount = 0;
        double difference = 0;
        double differenceTotalAmount = 0;
        foreach (GridViewRow gvr in GVSStatement.Rows)
        {
            Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
            Label lblgvDiffrence = (Label)gvr.FindControl("lblgvDiffrence");
            lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
            lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
            lblgvDiffrence.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDiffrence.Text);
            debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
            creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
            difference = Convert.ToDouble(lblgvDiffrence.Text);

            debitTotalamount += debitamount;
            creditTotalamount += creditamount;
            differenceTotalAmount += difference;
        }

        if (GVSStatement.Rows.Count > 0)
        {
            Label lblgvDebitTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvDebitAmountTotal");
            Label lblgvCreditTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvCreditAmountTotal");
            Label lblgvDiffrenceTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvDiffrenceTotal");


            lblgvDebitTotal.Text = debitTotalamount.ToString();
            lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitTotal.Text);
            lblgvCreditTotal.Text = creditTotalamount.ToString();
            lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditTotal.Text);
            lblgvDiffrenceTotal.Text = differenceTotalAmount.ToString();
            lblgvDiffrenceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDiffrenceTotal.Text);
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

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
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
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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
        DataTable dtloc = new DataTable();
        dtloc = ObjLocation.GetAllLocationMaster();
        if (dtloc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtloc, "Location_Name", "Location_Id");
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

        if (strLocationId == "")
        {
            strLocationId = Session["LocId"].ToString();
        }

        string strOptype = string.Empty;

        if (AllRecord_Rb.Checked)
        {
            strOptype = "1";
        }
        else if (EqualRecord_Rb.Checked)
        {
            strOptype = "2";
        }
        else if (NotEqualRecord_Rb.Checked)
        {
            strOptype = "3";
        }


        ArrayList objarr = new ArrayList();

        objarr.Add(txtFromDate.Text);
        objarr.Add(txtToDate.Text);
        objarr.Add(strLocationId);
        objarr.Add(strOptype);
        Session["ArrVoucherDetail"] = objarr;

        string strCmd = string.Format("window.open('../Accounts_Report/VoucherDetailTotal.aspx','window','width=1024, ');");
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
            }
        }

        //pnl1.Visible = true;
        //pnl2.Visible = true;


        // ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Voucher_Detail_Modal_Show()", true);
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
}