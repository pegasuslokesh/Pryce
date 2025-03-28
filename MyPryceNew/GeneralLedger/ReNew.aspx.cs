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

public partial class GeneralLedger_ReNew : System.Web.UI.Page
{
    CompanyMaster ObjCompany = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    Set_DocNumber objDocNo = null;

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
    Ac_ParameterMaster objAccParameter = null;
    Ac_Finance_Year_Info objFYI = null;
    DepartmentMaster objDepartment = null;
    Set_CustomerMaster ObjCustomer = null;
    Set_Suppliers ObjSupplier = null;
    Ac_Reconcile_Header ObjReconcileHeader = null;
    Ac_Reconcile_Detail ObjReconcileDetail = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Document_Master ObjDocument = null;
    PegasusDataAccess.DataAccessClass objDA = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;

    public const int grdDefaultColCount = 6;
    private const string strPageName = "ReNew";


    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
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
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjReconcileHeader = new Ac_Reconcile_Header(Session["DBConnection"].ToString());
        ObjReconcileDetail = new Ac_Reconcile_Detail(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn =new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/ReNew.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();

            if (strLocationId != "" && strLocationId != "0")
            {
                //txtToLocation.Text = Session["LocName"] + "/" + strLocationId;
            }

            txtReconcilationNo.Text = objDocNo.GetDocumentNo(true, "0", false, "150", "287", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtReconcilationNo.Text;
            FillLocation();

            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());
            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());

            DateTime dtReport = DateTime.Now;
            DateTime FistdaydateReport = dt.AddDays(-(dt.Day - 1));
            txtFromDateReport.Text = Fistdaydate.ToString(objsys.SetDateFormat());
            txtToDateReport.Text = DateTime.Now.ToString(objsys.SetDateFormat());

            FillGrid();
            btnList_Click(null, null);
            //AllPageCode();
            CalendarExtender2.Format = objsys.SetDateFormat();
            CalendarExtender3.Format = objsys.SetDateFormat();
            getPageControlsVisibility();
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnControlsSetting.Visible = false;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlView.Visible = false;
        PnlReport.Visible = false;
        FillGrid();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        Lbl_Tab_New.Text = "New";
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlView.Visible = false;
        PnlReport.Visible = false;
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlList.Visible = false;
        PnlNewEdit.Visible = false;
        PnlView.Visible = false;
        PnlReport.Visible = true;
    }
    protected void txtAccountName_TextChanged(object sender, EventArgs e)
    {
        RequiredFieldValidator1.Enabled = false;
        RequiredFieldValidator2.Enabled = false;
        if (txtAccountName.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
            dtAccount = new DataView(dtAccount, "IsActive='True' and Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
            string retval = string.Empty;
            if (txtAccountName.Text != "")
            {
                string strAccountName = txtAccountName.Text.Trim().Split('/')[0].ToString();
                dtAccount = new DataView(dtAccount, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count > 0)
                {
                    retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];
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

            if (retval != "0" && retval != "")
            {
                if (dtAccount != null && dtAccount.Rows.Count > 0)
                {
                    ////for Customer & Supplier Account
                    //string strReceiveVoucherAcc = string.Empty;
                    //DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
                    //if (dtParam.Rows.Count > 0)
                    //{
                    //    strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
                    //}
                    //else
                    //{
                    //    strReceiveVoucherAcc = "0";
                    //}

                    //string strPaymentVoucherAcc = string.Empty;
                    //DataTable dtPaymentVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
                    //if (dtPaymentVoucher.Rows.Count > 0)
                    //{
                    //    strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
                    //}
                    //else
                    //{
                    //    strPaymentVoucherAcc = "0";
                    //}

                    //for Customer & Supplier Account
                    string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

                    // for Employee and Vehicle Account
                    string strEmployeeAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                    string strVehicleAcc = Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

                    trSupplier.Visible = false;
                    trCustomer.Visible = false;
                    trEmployee.Visible = false;
                   
                    txtCustomerName.Text = "";
                    txtSupplierName.Text = "";
                    txtEmployeeName.Text = "";
                    
                    if (txtAccountName.Text.Split('/')[1].ToString() == strReceiveVoucherAcc)
                    {
                        trCustomer.Visible = true;
                        txtCustomerName.Focus();
                    }
                    else if (txtAccountName.Text.Split('/')[1].ToString() == strPaymentVoucherAcc)
                    {
                        trSupplier.Visible = true;
                        txtSupplierName.Focus();
                    }
                    else if (txtAccountName.Text.Split('/')[1].ToString() == strEmployeeAcc)
                    {
                        trEmployee.Visible = true;
                        txtEmployeeName.Focus();
                    }
                 
                }
                else
                {
                    txtAccountName.Text = "";
                    DisplayMessage("No Account Found");
                    txtAccountName.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployee(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole_withTerminated(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }

        return txt;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    protected void txtEmployeeName_TextChanged(object sender, EventArgs e)
    {
        if (txtEmployeeName.Text != "")
        {
            try
            {
                txtEmployeeName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Employee Name");
                txtEmployeeName.Text = "";
                txtEmployeeName.Focus();
                return;
            }
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtEmployeeName.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            DataTable dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Emp_ID);
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Employee Name");
                txtEmployeeName.Text = "";
                txtEmployeeName.Focus();
                return;
            }

        }
        else
        {
            DisplayMessage("Select Employee Name");
            txtEmployeeName.Focus();
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
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
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

        DateTime dtToDate = new DateTime();
        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name First");
            txtAccountName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            return;
        }

        if (trSupplier.Visible == true)
        {
            if (txtSupplierName.Text == "")
            {
                DisplayMessage("Fill Supplier Name");
                txtSupplierName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
                return;
            }
        }
        else if (trCustomer.Visible == true)
        {
            if (txtCustomerName.Text == "")
            {
                DisplayMessage("Fill Customer Name");
                txtCustomerName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                return;
            }
        }
        else if (trEmployee.Visible==true)
        {
            if (txtEmployeeName.Text == "")
            {
                DisplayMessage("Fill Employee Name");
                txtEmployeeName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                return;
            }
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
        else
        {
            DisplayMessage("Need to Fill From Date");
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

        string strCurrency = Session["LocCurrencyId"].ToString();
        DataTable dtReconcileData = ObjReconcileDetail.GetReconcileDetailAllDataFromVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strCurrencyType);

        if (dtReconcileData.Rows.Count > 0)
        {
            dtReconcileData = new DataView(dtReconcileData, "Account_No='" + txtAccountName.Text.Split('/')[1].ToString() + "' and Voucher_Date>='" + txtFromDate.Text + "' and Voucher_Date<='" + txtToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtReconcileData.Rows.Count > 0)
            {
                if (trSupplier.Visible == true)
                {
                    dtReconcileData = new DataView(dtReconcileData, "Other_Account_No='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (trCustomer.Visible == true)
                {
                    dtReconcileData = new DataView(dtReconcileData, "Other_Account_No='" + txtCustomerName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (trEmployee.Visible==true)
                {
                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                    string Emp_Code = txtEmployeeName.Text.Split('/')[1].ToString();
                    string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                    if (!string.IsNullOrEmpty(Emp_ID))
                    {
                        dtReconcileData = new DataView(dtReconcileData, "Other_Account_No='" + Emp_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }

                if (ddlReconciled.SelectedValue == "NR")
                {
                    dtReconcileData = new DataView(dtReconcileData, "ReconciledBy='' and ReconciledDate=''", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlReconciled.SelectedValue == "R")
                {
                    dtReconcileData = new DataView(dtReconcileData, "ReconciledBy<>'' and ReconciledDate<>''", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            if (ddlSVoucherType.SelectedValue != "--Select--")
            {
                dtReconcileData = new DataView(dtReconcileData, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtReconcileData.Rows.Count + "";

        if (dtReconcileData != null && dtReconcileData.Rows.Count > 0)
        {
            Session["dtRVoucher"] = dtReconcileData;
            Session["dtRFilter"] = dtReconcileData;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtReconcileData, "", "");

            if (ddlReconciled.SelectedValue == "NR")
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = false;
                GvVoucher.Columns[3].Visible = false;
                GvVoucher.Columns[11].Visible = true;
            }
            else if (ddlReconciled.SelectedValue == "R")
            {
                GvVoucher.Columns[0].Visible = false;
                GvVoucher.Columns[1].Visible = false;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = false;
            }
            else
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = true;

                foreach (GridViewRow gvd in GvVoucher.Rows)
                {
                    CheckBox chkYes = (CheckBox)gvd.FindControl("chkReconciledYes");
                    CheckBox chkNo = (CheckBox)gvd.FindControl("chkReconciledNot");
                    Label lblgvempname = (Label)gvd.FindControl("lblgvReconciledBy");
                    Label lblgvRDate = (Label)gvd.FindControl("lblgvReconciledDate");
                    TextBox txtgvRemarks = (TextBox)gvd.FindControl("txtgvRemarks");

                    if (lblgvempname.Text != "" && lblgvRDate.Text != "")
                    {
                        chkYes.Visible = false;
                        chkNo.Visible = false;
                        txtgvRemarks.Visible = false;
                    }
                    else
                    {
                        chkYes.Visible = true;
                        chkNo.Visible = true;
                        txtgvRemarks.Visible = true;
                    }
                }
            }
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
            DisplayMessage("You have no record");
        }

        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
            Label lblgvForeignAmt = (Label)gvr.FindControl("lblgvForeignAmount");

            lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
            lblgvForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvForeignAmt.Text);
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtReconcileData.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSupplierName.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        return;
                    }

                }
            }
        }
        catch
        { }
        DisplayMessage("Supplier is not valid");
        txtSupplierName.Text = "";
        txtSupplierName.Focus();
        //if (txtSupplierName.Text != "")
        //{
        //    try
        //    {
        //        txtSupplierName.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Supplier Name");
        //        txtSupplierName.Text = "";
        //        txtSupplierName.Focus();
        //        return;
        //    }

        //    DataTable dt = objContact.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Supplier Name");
        //        txtSupplierName.Text = "";
        //        txtSupplierName.Focus();
        //    }
        //    else
        //    {
        //        string strSupplierId = txtSupplierName.Text.Trim().Split('/')[1].ToString();
        //        if (strSupplierId != "0" && strSupplierId != "")
        //        {
        //            DataTable dtSup = ObjSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
        //            if (dtSup.Rows.Count > 0)
        //            {
        //                Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
        //            }
        //            else
        //            {
        //                DisplayMessage("First Set Supplier Details in Supplier Setup");
        //                txtSupplierName.Text = "";
        //                txtSupplierName.Focus();
        //                return;
        //            }

        //            if (Session["SupplierAccountId"].ToString() == "0" && Session["SupplierAccountId"].ToString() == "")
        //            {
        //                DisplayMessage("First Set Supplier Account in Supplier Setup");
        //                txtSupplierName.Text = "";
        //                txtSupplierName.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Supplier Name");
        //    txtSupplierName.Focus();
        //}
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
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
                        return;
                    }

                }
            }
        }
        catch
        {

        }
        DisplayMessage("Customer is not valid");
        txtCustomerName.Text = "";
        txtCustomerName.Focus();
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
        //        txtCustomerName.Focus();
        //        return;
        //    }

        //    DataTable dt = objContact.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Customer Name");
        //        txtCustomerName.Text = "";
        //        txtCustomerName.Focus();
        //    }
        //    else
        //    {
        //        string strCustomerId = txtCustomerName.Text.Trim().Split('/')[1].ToString();
        //        if (strCustomerId != "0" && strCustomerId != "")
        //        {
        //            DataTable dtCus = ObjCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
        //            if (dtCus.Rows.Count > 0)
        //            {
        //                Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
        //            }
        //            else
        //            {
        //                DisplayMessage("First Set Customer Details in Customer Setup");
        //                txtCustomerName.Text = "";
        //                txtCustomerName.Focus();
        //                return;
        //            }

        //            if (Session["CustomerAccountId"].ToString() == "0" && Session["CustomerAccountId"].ToString() == "")
        //            {
        //                DisplayMessage("First Set Customer Account in Customer Setup");
        //                txtCustomerName.Text = "";
        //                txtCustomerName.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Customer Name");
        //    txtCustomerName.Focus();
        //}
    }
    protected void btnReconciled_Click(object sender, EventArgs e)
    {
        //if (!Common.IsFinancialyearAllow(Convert.ToDateTime(DateTime.Now.ToString()), "F"))
        //{
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}

        if (Session["EmpId"].ToString() == "0")
        {
            DisplayMessage("You need to login from User with Employee for Update");
            return;
        }

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

        if (strFlag == "False")
        {
            DisplayMessage("You cant update that record your location currency are different");
            txtAccountName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            return;
        }


        string strOtherAccountNo = "0";
        DateTime dtToDate = new DateTime();
        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name First");
            txtAccountName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            return;
        }

        if (trSupplier.Visible == true)
        {
            if (txtSupplierName.Text == "")
            {
                DisplayMessage("Fill Supplier Name");
                txtSupplierName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
                return;
            }
            else
            {
                strOtherAccountNo = txtSupplierName.Text.Split('/')[1].ToString();
            }
        }
        else if (trCustomer.Visible == true)
        {
            if (txtCustomerName.Text == "")
            {
                DisplayMessage("Fill Customer Name");
                txtCustomerName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                return;
            }
            else
            {
                strOtherAccountNo = txtCustomerName.Text.Split('/')[1].ToString();
            }
        }
        else if (trEmployee.Visible == true)
        {
            if (txtEmployeeName.Text == "")
            {
                DisplayMessage("Fill Employee Name");
                txtEmployeeName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                return;
            }
            else
            {
                //string strEmployeeAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString());
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtEmployeeName.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strOtherAccountNo = Emp_ID;
            }
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
        else
        {
            DisplayMessage("Need to Fill From Date");
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

        int flag = 0;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            CheckBox chkReconciledYes = (CheckBox)gvr.FindControl("chkReconciledYes");
            CheckBox chkReconciledNo = (CheckBox)gvr.FindControl("chkReconciledNot");

            if (chkReconciledYes.Checked == true || chkReconciledNo.Checked == true)
            {
                flag = 1;
                break;
            }
            else
            {
                flag = 0;
            }
        }

        if (flag == 0)
        {
            DisplayMessage("Please Select Record");
            GvVoucher.Focus();
            return;
        }

        int R = 0;
        R = ObjReconcileHeader.InsertReconcileHeader(Session["FinanceYearId"].ToString(), StrCompId, StrBrandId, Session["LocId"].ToString(), Session["EmpId"].ToString(), txtReconcilationNo.Text, DateTime.Now.ToString("dd-MMM-yyyy"), txtAccountName.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "0", "0", "0", txtRemarks.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (R != 0)
        {
            string strMaxId = string.Empty;
            DataTable dtMaxId = ObjReconcileHeader.GetReconcileMaxId();
            if (dtMaxId.Rows.Count > 0)
            {
                strMaxId = dtMaxId.Rows[0][0].ToString();
                if (txtReconcilationNo.Text == ViewState["DocNo"].ToString())
                {
                    DataTable dtCount = ObjReconcileHeader.GetReconcileHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    if (dtCount.Rows.Count == 0)
                    {
                        ObjReconcileHeader.Updatecode(strMaxId, txtReconcilationNo.Text + "1");
                        txtReconcilationNo.Text = txtReconcilationNo.Text + "1";
                    }
                    else
                    {
                        ObjReconcileHeader.Updatecode(strMaxId, txtReconcilationNo.Text + dtCount.Rows.Count);
                        txtReconcilationNo.Text = txtReconcilationNo.Text + dtCount.Rows.Count;
                    }
                }

                int d = 0;
                int DR = 0;
                double debitamt = 0;
                double creditamt = 0;
                double TotalRRecord = 0;
                double TotalRAmount = 0;
                double TotalNRecord = 0;
                double TotalNAmount = 0;
                foreach (GridViewRow gvr in GvVoucher.Rows)
                {
                    CheckBox chkgvReconciledYes = (CheckBox)gvr.FindControl("chkReconciledYes");
                    CheckBox chkgvReconciledNo = (CheckBox)gvr.FindControl("chkReconciledNot");
                    Label lblTransId = (Label)gvr.FindControl("lblgvTransId");
                    Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                    TextBox txtgvRemarks = (TextBox)gvr.FindControl("txtgvRemarks");

                    if (lblgvDebitAmount.Text == "")
                    {
                        debitamt = 0;
                    }
                    else
                    {
                        debitamt = Convert.ToDouble(lblgvDebitAmount.Text);
                    }

                    if (lblgvCreditAmount.Text == "")
                    {
                        creditamt = 0;
                    }
                    else
                    {
                        creditamt = Convert.ToDouble(lblgvCreditAmount.Text);
                    }

                    if (chkgvReconciledYes.Checked == true)
                    {
                        TotalRRecord += Convert.ToDouble("1");
                        if (debitamt != 0)
                        {
                            TotalRAmount += Convert.ToDouble(debitamt.ToString());
                        }
                        else if (creditamt != 0)
                        {
                            TotalRAmount += Convert.ToDouble(creditamt.ToString());
                        }

                        d = objVoucherDetail.UpdateVoucherDetailReconcileStatus(lblTransId.Text, Session["EmpId"].ToString(), DateTime.Now.ToString("dd-MMM-yyyy"));
                        DR = ObjReconcileDetail.InsertReconcileDetail(StrCompId, StrBrandId, Session["LocId"].ToString(), strMaxId, Session["EmpId"].ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), lblTransId.Text, debitamt.ToString(), creditamt.ToString(), "True", txtgvRemarks.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else if (chkgvReconciledNo.Checked == true)
                    {
                        TotalNRecord += Convert.ToDouble("1");
                        if (debitamt != 0)
                        {
                            TotalNAmount += Convert.ToDouble(debitamt.ToString());
                        }
                        else if (creditamt != 0)
                        {
                            TotalNAmount += Convert.ToDouble(creditamt.ToString());
                        }
                        DR = ObjReconcileDetail.InsertReconcileDetail(StrCompId, StrBrandId, Session["LocId"].ToString(), strMaxId, Session["EmpId"].ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), lblTransId.Text, debitamt.ToString(), creditamt.ToString(), "False", txtgvRemarks.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }

                ObjReconcileHeader.UpdateReconcileHeader(strMaxId, TotalRRecord.ToString(), TotalRAmount.ToString(), TotalNRecord.ToString(), TotalNAmount.ToString());
            }
        }

        if (R != 0)
        {
            btnCancel_Click(null, null);
            DisplayMessage("Record Reconciled");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtReconcilationNo.Text = ViewState["DocNo"].ToString();
        txtAccountName.Text = "";
        txtCustomerName.Text = "";
        txtSupplierName.Text = "";
        txtRemarks.Text = "";
        trCustomer.Visible = false;
        trSupplier.Visible = false;
        GvVoucher.DataSource = null;
        GvVoucher.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + 0 + "";
        ddlReconciled.SelectedValue = "NR";
        ddlSVoucherType.SelectedValue = "--Select--";

        DateTime dt = DateTime.Now;
        DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
        txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

        txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
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
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
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
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetSupplierAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //txt[i] = dt.Rows[i]["Emp_Name"].ToString();
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //txt[i] = dt1.Rows[i]["Emp_Name"].ToString();
                    txt[i] = dt1.Rows[i]["Emp_Name"].ToString() + "/" + dt1.Rows[i]["Emp_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListReconcileNo(string prefixText, int count, string contextKey)
    {
        Ac_Reconcile_Header ObjHeader = new Ac_Reconcile_Header(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt1 = ObjHeader.GetReconcileHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        DataTable dt = new DataView(dt1, "ReconcilationNo like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //txt[i] = dt.Rows[i]["ReconcilationNo"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                txt[i] = dt.Rows[i]["ReconcilationNo"].ToString();
            }
        }
        return txt;
    }


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

    protected void chkReconciledYes_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        //Label lb = (Label)GvVoucher.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledYes")).Checked)
        {
            ((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledNot")).Checked = false;
        }
        else if (((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledNot")).Checked)
        {
            ((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledYes")).Checked = false;
        }
    }
    protected void chkReconciledNot_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        //Label lb = (Label)GvVoucher.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledNot")).Checked)
        {
            ((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledYes")).Checked = false;
        }
        else if (((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledYes")).Checked)
        {
            ((CheckBox)GvVoucher.Rows[index].FindControl("chkReconciledNot")).Checked = false;
        }
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
    protected string GetEmployeeNameByEmpId(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEmployeeName = objEmployee.GetEmployeeMasterById(StrCompId, strEmployeeId);
            if (dtEmployeeName.Rows.Count > 0)
            {
                strEmployeeName = dtEmployeeName.Rows[0]["Emp_Name"].ToString();
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
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
    protected void GvVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        GvVoucher.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtRFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");

        if (ddlReconciled.SelectedValue == "NR")
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = false;
                GvVoucher.Columns[3].Visible = false;
                GvVoucher.Columns[11].Visible = true;
            }
        }
        else if (ddlReconciled.SelectedValue == "R")
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = false;
                GvVoucher.Columns[1].Visible = false;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = false;
            }
        }
        else
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = true;
            }

            foreach (GridViewRow gvd in GvVoucher.Rows)
            {
                CheckBox chkYes = (CheckBox)gvd.FindControl("chkReconciledYes");
                CheckBox chkNo = (CheckBox)gvd.FindControl("chkReconciledNot");
                Label lblgvempname = (Label)gvd.FindControl("lblgvReconciledBy");
                Label lblgvRDate = (Label)gvd.FindControl("lblgvReconciledDate");
                TextBox txtgvRemarks = (TextBox)gvd.FindControl("txtgvRemarks");

                if (lblgvempname.Text != "" && lblgvRDate.Text != "")
                {
                    chkYes.Visible = false;
                    chkNo.Visible = false;
                    txtgvRemarks.Visible = false;
                }
                else
                {
                    chkYes.Visible = true;
                    chkNo.Visible = true;
                    txtgvRemarks.Visible = true;
                }
            }
        }
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
            Label lblgvForeignAmt = (Label)gvr.FindControl("lblgvForeignAmount");

            lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
            lblgvForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvForeignAmt.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        DataTable dt = (DataTable)Session["dtRFilter"];
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
        Session["dtRFilter"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");

        if (ddlReconciled.SelectedValue == "NR")
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = false;
                GvVoucher.Columns[3].Visible = false;
                GvVoucher.Columns[11].Visible = true;
            }
        }
        else if (ddlReconciled.SelectedValue == "R")
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = false;
                GvVoucher.Columns[1].Visible = false;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = false;
            }
        }
        else
        {
            if (GvVoucher.Rows.Count > 0)
            {
                GvVoucher.Columns[0].Visible = true;
                GvVoucher.Columns[1].Visible = true;
                GvVoucher.Columns[2].Visible = true;
                GvVoucher.Columns[3].Visible = true;
                GvVoucher.Columns[11].Visible = true;
            }

            foreach (GridViewRow gvd in GvVoucher.Rows)
            {
                CheckBox chkYes = (CheckBox)gvd.FindControl("chkReconciledYes");
                CheckBox chkNo = (CheckBox)gvd.FindControl("chkReconciledNot");
                Label lblgvempname = (Label)gvd.FindControl("lblgvReconciledBy");
                Label lblgvRDate = (Label)gvd.FindControl("lblgvReconciledDate");
                TextBox txtgvRemarks = (TextBox)gvd.FindControl("txtgvRemarks");

                if (lblgvempname.Text != "" && lblgvRDate.Text != "")
                {
                    chkYes.Visible = false;
                    chkNo.Visible = false;
                    txtgvRemarks.Visible = false;
                }
                else
                {
                    chkYes.Visible = true;
                    chkNo.Visible = true;
                    txtgvRemarks.Visible = true;
                }
            }
        }

        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
            Label lblgvForeignAmt = (Label)gvr.FindControl("lblgvForeignAmount");

            lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
            lblgvForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvForeignAmt.Text);
        }
        //AllPageCode();
    }

    #region List Work
    protected void btnCancelView_Click(object sender, EventArgs e)
    {
        Lbl_Tab_New.Text = "New";

        btnList_Click(null, null);

        txtReconciledByView.Text = "";
        txtReconcilationNoView.Text = "";
        txtReconciledDateView.Text = "";
        txtAccountNameView.Text = "";

        tr1ViewSupplier.Visible = false;
        tr2ViewCustomer.Visible = false;
        txtSupplierNameView.Text = "";
        txtCustomerNameView.Text = "";

        txtRemarksView.Text = "";
        txtTotalRRecordView.Text = "";
        txtTotalRAmountView.Text = "";
        txtTotalNRRecordView.Text = "";
        txtTotalNRAmountView.Text = "";
        GvView.DataSource = null;
        GvView.DataBind();
        PnlNewEdit.Visible = true;
        PnlView.Visible = false;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        Lbl_Tab_New.Text = "View";
        PnlList.Visible = false;
        PnlNewEdit.Visible = false;
        PnlView.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        string strReconciledId = e.CommandArgument.ToString();
        DataTable dtReconcileHeader = ObjReconcileHeader.GetReconcileHeaderAllTrueByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strReconciledId);
        if (dtReconcileHeader.Rows.Count > 0)
        {
            txtReconciledByView.Text = GetEmployeeNameByEmpId(dtReconcileHeader.Rows[0]["Reconciled_By"].ToString());
            txtReconcilationNoView.Text = dtReconcileHeader.Rows[0]["ReconcilationNo"].ToString();
            txtReconciledDateView.Text = Convert.ToDateTime(dtReconcileHeader.Rows[0]["ReconcileDate"].ToString()).ToString(objsys.SetDateFormat());
            string strAccountId = dtReconcileHeader.Rows[0]["Account_No"].ToString();
            txtAccountNameView.Text = GetAccountNameByTransId(strAccountId);
            if (strAccountId != "" && strAccountId != "0")
            {
                //for Customer & Supplier Account
                string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                string strEmployeeAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

                if (strAccountId == strReceiveVoucherAcc)
                {
                    tr1ViewSupplier.Visible = false;
                    tr2ViewCustomer.Visible = true;
                    txtSupplierNameView.Text = "";
                    string strCustomerId = dtReconcileHeader.Rows[0]["Other_Account_No"].ToString();
                    if (strCustomerId != "" && strCustomerId != "0")
                    {
                        //txtCustomerNameView.Text = GetCustomerNameByContactId(strCustomerId);
                        DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strCustomerId);
                        txtCustomerNameView.Text = _dtTemp.Rows.Count > 0 ? _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString() : "";
                        _dtTemp.Dispose();
                    }
                    else
                    {
                        txtCustomerNameView.Text = "";
                    }
                }
                else if (strAccountId == strPaymentVoucherAcc)
                {
                    tr1ViewSupplier.Visible = true;
                    tr2ViewCustomer.Visible = false;
                    string strSupplierId = dtReconcileHeader.Rows[0]["Other_Account_No"].ToString();
                    if (strSupplierId != "" && strSupplierId != "0")
                    {
                        // txtSupplierNameView.Text = GetCustomerNameByContactId(strSupplierId);
                        DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strSupplierId);
                        txtSupplierNameView.Text = _dtTemp.Rows.Count > 0 ? _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString() : "";
                        _dtTemp.Dispose();
                    }
                    else
                    {
                        txtSupplierNameView.Text = "";
                    }
                    txtCustomerNameView.Text = "";
                }
                else if (strAccountId==strEmployeeAcc)
                {
                    tr1ViewSupplier.Visible = false;
                    tr2ViewCustomer.Visible = false;
                    tr3ViewEmployee.Visible = true;
                   
                    string strEmployeeId = dtReconcileHeader.Rows[0]["Other_Account_No"].ToString();
                    if (strEmployeeId != "" && strEmployeeId != "0")
                    {
                        string Emp_ID = dtReconcileHeader.Rows[0]["Other_Account_No"].ToString();
                        DataTable _dtEmp = new EmployeeMaster(Session["DBConnection"].ToString()).GetEmployeeMasterById(Session["CompId"].ToString(), Emp_ID);
                        if (_dtEmp.Rows.Count > 0)
                        {
                            txtEmployeeNameView.Text = _dtEmp.Rows[0]["emp_name"].ToString() + "/" + _dtEmp.Rows[0]["emp_code"].ToString();
                        }
                        _dtEmp.Dispose();
                    }
                    else
                    {
                        txtEmployeeNameView.Text = "";
                    }
                    //txtEmployeeNameView.Text = "";
                }
                else
                {
                    tr1ViewSupplier.Visible = false;
                    tr2ViewCustomer.Visible = false;
                    txtSupplierNameView.Text = "";
                    txtCustomerNameView.Text = "";
                }
            }

            txtRemarksView.Text = dtReconcileHeader.Rows[0]["Remarks"].ToString();
            txtTotalRRecordView.Text = dtReconcileHeader.Rows[0]["Total_Reconciled_Record"].ToString();
            txtTotalRAmountView.Text = objsys.GetCurencyConversionForInv(strCurrency, dtReconcileHeader.Rows[0]["Total_Reconciled_Amount"].ToString());
            txtTotalNRRecordView.Text = dtReconcileHeader.Rows[0]["Total_Not_Reconciled_Record"].ToString();
            txtTotalNRAmountView.Text = objsys.GetCurencyConversionForInv(strCurrency, dtReconcileHeader.Rows[0]["Total_Not_Reconciled_Amount"].ToString());

            DataTable dtReconcileDetail = ObjReconcileDetail.GetReconcileDetailDataByHeaderIdWithVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strReconciledId);
            if (dtReconcileDetail != null && dtReconcileDetail.Rows.Count > 0)
            {
                Session["dtVoucherV"] = dtReconcileDetail;
                Session["dtFilterV"] = dtReconcileDetail;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvView, dtReconcileDetail, "", "");

                foreach (GridViewRow gvView in GvView.Rows)
                {
                    Label lblgvDebitAmt = (Label)gvView.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmt = (Label)gvView.FindControl("lblgvCreditAmount");

                    lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
                    lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
                }
            }
            else
            {
                GvReconcile.DataSource = null;
                GvReconcile.DataBind();
            }


            //For Fill Uploaded Document
            try
            {
                DataTable dtDirectory = new DataView(objDir.getDirectoryMasterByCompanyid(StrCompId), "Company_id='" + StrCompId.ToString() + "'and Field3='" + strReconciledId + "'and Field4='ReconciledDocument'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtFile = new DataTable();
                string str = string.Empty;
                foreach (DataRow dr in dtDirectory.Rows)
                {
                    dtFile.Merge(ObjFile.Get_FileTransaction_By_DirectoryidandObjectId(StrCompId, "0", dr["Id"].ToString()));
                }
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)GvUploadedDocument, dtFile, "", "");
                //AllPageCode();
            }
            catch
            {

            }
        }
    }
    protected string GetStatus(string strIsReconciled)
    {
        string strStatus = string.Empty;
        if (strIsReconciled != "0" && strIsReconciled != "")
        {
            if (strIsReconciled == "True")
            {
                strStatus = "Reconciled";
            }
            else if (strIsReconciled == "False")
            {
                strStatus = "Conflicted";
            }
            else
            {
                strStatus = "";
            }
        }
        else
        {
            strStatus = "";
        }
        return strStatus;
    }
    protected void GvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        GvView.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtRFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvView, dt, "", "");
        foreach (GridViewRow gvr in GvView.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");

            lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvCreditAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
        }
        //AllPageCode();
    }
    private void FillGrid()
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        DataTable dtReconcile = ObjReconcileHeader.GetReconcileHeaderAllTrue(StrCompId, StrBrandId, strLocationId);
        lblTotalRecordsList.Text = Resources.Attendance.Total_Records + " : " + dtReconcile.Rows.Count + "";

        if (dtReconcile != null && dtReconcile.Rows.Count > 0)
        {
            Session["dtVoucherR"] = dtReconcile;
            Session["dtFilterR"] = dtReconcile;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvReconcile, dtReconcile, "", "");
        }
        else
        {
            GvReconcile.DataSource = null;
            GvReconcile.DataBind();
        }

        foreach (GridViewRow gv in GvReconcile.Rows)
        {
            Label lblgvTotalRAmount = (Label)gv.FindControl("lblgvTotalReconciledAmount");
            Label lblgvTotalNRAmount = (Label)gv.FindControl("lblgvTotalNotReconciledAmount");

            lblgvTotalRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalRAmount.Text);
            lblgvTotalNRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalNRAmount.Text);
        }

        lblTotalRecordsList.Text = Resources.Attendance.Total_Records + " : " + dtReconcile.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    protected void GvReconcile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        GvReconcile.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterR"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvReconcile, dt, "", "");
        foreach (GridViewRow gv in GvReconcile.Rows)
        {
            Label lblgvTotalRAmount = (Label)gv.FindControl("lblgvTotalReconciledAmount");
            Label lblgvTotalNRAmount = (Label)gv.FindControl("lblgvTotalNotReconciledAmount");

            lblgvTotalRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalRAmount.Text);
            lblgvTotalNRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalNRAmount.Text);
        }
        //AllPageCode();
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

            DataTable dtVoucher = (DataTable)Session["dtVoucherR"];
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvReconcile, view.ToTable(), "", "");

            Session["dtFilterR"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvReconcile_Sorting(object sender, GridViewSortEventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();
        DataTable dt = (DataTable)Session["dtFilterR"];
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
        Session["dtFilterR"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvReconcile, dt, "", "");
        foreach (GridViewRow gv in GvReconcile.Rows)
        {
            Label lblgvTotalRAmount = (Label)gv.FindControl("lblgvTotalReconciledAmount");
            Label lblgvTotalNRAmount = (Label)gv.FindControl("lblgvTotalNotReconciledAmount");

            lblgvTotalRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalRAmount.Text);
            lblgvTotalNRAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvTotalNRAmount.Text);
        }
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string strHeaderId = e.CommandArgument.ToString();
        if (strHeaderId != "")
        {
            DataTable dtDetail = ObjReconcileDetail.GetReconcileDetailDataByHeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strHeaderId);
            if (dtDetail.Rows.Count > 0)
            {
                dtDetail = new DataView(dtDetail, "Is_Reconciled='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        string strVDId = dtDetail.Rows[i]["VD_Id"].ToString();
                        if (strVDId != "" && strVDId != "0")
                        {
                            string strSQL = "Update Ac_Voucher_Detail set Field3='', Field4='' where Trans_Id='" + strVDId + "'";
                            int v = objDA.execute_Command(strSQL);
                        }
                    }
                }
            }

            ObjReconcileHeader.DeleteReconcileHeaderPermanent(strHeaderId);
            ObjReconcileDetail.DeleteReconcileDetailPermanent(strHeaderId);
            DisplayMessage("Record Deleted");
        }
        FillGrid();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }
    protected void lnkAddDocument_Command(object sender, CommandEventArgs e)
    {
        hdnReconcileId.Value = e.CommandArgument.ToString();
        PanelView1.Visible = true;
        PanelView2.Visible = true;
        BindDocumentList();
        FillDocGrid();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Vooucher_Details_Modal_Popup()", true);
    }
    #endregion

    #region Add Document
    public void FillDocGrid()
    {
        try
        {
            DataTable dtDirectory = new DataView(objDir.getDirectoryMasterByCompanyid(StrCompId), "Company_id='" + StrCompId.ToString() + "'and Field3='" + hdnReconcileId.Value + "'and Field4='ReconciledDocument'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtFile = new DataTable();
            string str = string.Empty;
            foreach (DataRow dr in dtDirectory.Rows)
            {
                dtFile.Merge(ObjFile.Get_FileTransaction_By_DirectoryidandObjectId(StrCompId, "0", dr["Id"].ToString()));
            }
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvFileMaster, dtFile, "", "");
            //AllPageCode();
        }
        catch
        {
        }
    }
    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;
        ViewReset();
        hdnReconcileId.Value = "0";
    }
    void ViewReset()
    {
        btnList_Click(null, null);
        hdnReconcileId.Value = "0";
    }
    void BindDocumentList()
    {
        DataTable dtdocument = new DataTable();
        string Documentid = "0";
        dtdocument = ObjDocument.getdocumentmaster(Session["CompId"].ToString(), Documentid);
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)ddlDocumentName, dtdocument, "Document_name", "Id");
    }
    protected void ImgButtonDocumentAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlDocumentName.SelectedIndex == 0)
        {
            DisplayMessage("Select Document Name");
            ddlDocumentName.Focus();
            return;
        }
        if (UploadFile.HasFile == false)
        {
            DisplayMessage("Upload The File");
            UploadFile.Focus();
            return;
        }
        string filepath = string.Empty;
        int b = 0;
        string DirectoryName;
        try
        {
            filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString() + "/" + UploadFile.FileName;
            CreateDirectoryIfNotExist(Server.MapPath("~/" + "ArcaWing/" + Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString()));

            DirectoryName = Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString();
            DataTable dtDir = objDir.GetDirectoryMaster_By_DirectoryName(StrCompId, DirectoryName);
            if (dtDir.Rows.Count == 0)
            {
                b = objDir.InsertDirectorymaster(StrCompId, DirectoryName, "1", "0", hdnReconcileId.Value.Trim(), "ReconciledDocument", "0", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),Session["EmpId"].ToString());
            }
            else
            {
                b = Convert.ToInt32(dtDir.Rows[0]["Id"].ToString());
            }
            UploadFile.SaveAs(Server.MapPath(filepath));
        }
        catch
        {

        }

        Byte[] bytes = new Byte[0];
        try
        {
            Stream fs = UploadFile.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            bytes = br.ReadBytes((Int32)fs.Length);
        }
        catch
        {

        }

        ObjFile.Insert_In_FileTransaction(StrCompId, b.ToString(), ddlDocumentName.SelectedValue.ToString(), "0", UploadFile.FileName.ToString(), DateTime.Now.ToString(), bytes, "", DateTime.Now.AddYears(20).ToString(), "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        string ext = string.Empty;
        string filename = UploadFile.FileName;
        try
        {
            ext = Path.GetExtension(filepath);
        }
        catch
        {

        }

        FillDocGrid();
        //AllPageCode();
        ddlDocumentName.SelectedIndex = 0;
    }
    public int CreateDirectoryIfNotExist(string NewDirectory)
    {
        int checkDirectory = 0;
        try
        {
            // Checking the existance of directory
            if (!Directory.Exists(NewDirectory))
            {
                //If No any such directory then creates the new one
                Directory.CreateDirectory(NewDirectory);
            }
            else
            {
                checkDirectory = 1;
            }
        }
        catch (IOException _err)
        {
            Response.Write(_err.Message);
        }
        return checkDirectory;
    }
    protected void IbtnDeleteDocument_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjFile.Get_FileTransaction_By_TransactionId(StrCompId.ToString(), e.CommandArgument.ToString());
        if (dt != null)
        {

            try
            {
                string FilePath = string.Empty;
                FilePath = Server.MapPath(StrCompId + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + e.CommandName.ToString() + dt.Rows[0]["File_Name"].ToString());
                FilePath = FilePath.Replace("MasterSetup", "ArcaWing");
                System.IO.File.Delete(FilePath);
            }
            catch
            {

            }

            ObjFile.Delete_in_FileTransactionParmanent(StrCompId, e.CommandArgument.ToString());
            FillDocGrid();
        }
    }
    private void download(DataTable dt)
    {
        Byte[] bytes = (Byte[])dt.Rows[0]["File_Data"];
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = dt.Rows[0]["ContentType"].ToString();
        Response.AddHeader("content-disposition", "attachment;filename="
        + dt.Rows[0]["File_Name"].ToString());
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
    protected void OnDownloadCommand(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = ObjFile.Get_FileTransaction_By_TransactionId(StrCompId, e.CommandArgument.ToString());
        download(dt);
    }
    #endregion

    #region ReportWork
    protected void txtAccountNameReport_TextChanged(object sender, EventArgs e)
    {
        RequiredFieldValidator6.Enabled = false;
        RequiredFieldValidator7.Enabled = false;

        if (txtAccountNameReport.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
            dtAccount = new DataView(dtAccount, "IsActive='True' and Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
            string retval = string.Empty;
            if (txtAccountNameReport.Text != "")
            {
                string strAccountName = txtAccountNameReport.Text.Trim().Split('/')[0].ToString();
                dtAccount = new DataView(dtAccount, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count > 0)
                {
                    retval = (txtAccountNameReport.Text.Split('/'))[txtAccountNameReport.Text.Split('/').Length - 1];
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

            if (retval != "0" && retval != "")
            {
                if (dtAccount != null && dtAccount.Rows.Count > 0)
                {
                    //for Customer & Supplier Account
                    string strReceiveVoucherAcc = string.Empty;
                    DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
                    if (dtParam.Rows.Count > 0)
                    {
                        strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
                    }
                    else
                    {
                        strReceiveVoucherAcc = "0";
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

                    if (txtAccountNameReport.Text.Split('/')[1].ToString() == strReceiveVoucherAcc)
                    {
                        trSupplierReport.Visible = false;
                        trCustomerReport.Visible = true;
                        txtSupplierNameReport.Text = "";
                        txtCustomerNameReport.Text = "";
                        txtCustomerNameReport.Focus();
                        RequiredFieldValidator7.Enabled = true;
                    }
                    else if (txtAccountNameReport.Text.Split('/')[1].ToString() == strPaymentVoucherAcc)
                    {
                        trSupplierReport.Visible = true;
                        trCustomerReport.Visible = false;
                        txtSupplierNameReport.Text = "";
                        txtCustomerNameReport.Text = "";
                        txtSupplierNameReport.Focus();
                        RequiredFieldValidator6.Enabled = true;
                    }
                    else
                    {
                        trSupplierReport.Visible = false;
                        trCustomerReport.Visible = false;
                        txtSupplierNameReport.Text = "";
                        txtCustomerNameReport.Text = "";
                    }
                }
                else
                {
                    txtAccountNameReport.Text = "";
                    DisplayMessage("No Account Found");
                    txtAccountNameReport.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountNameReport.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNameReport);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNameReport);
        }
    }
    protected void txtSupplierNameReport_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierNameReport.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSupplierNameReport.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        return;
                    }

                }
            }
        }
        catch
        { }
        DisplayMessage("Supplier is not valid");
        txtSupplierNameReport.Text = "";
        txtSupplierNameReport.Focus();

        //if (txtSupplierNameReport.Text != "")
        //{
        //    try
        //    {
        //        txtSupplierNameReport.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Supplier Name");
        //        txtSupplierNameReport.Text = "";
        //        txtSupplierNameReport.Focus();
        //        return;
        //    }

        //    DataTable dt = objContact.GetContactByContactName(txtSupplierNameReport.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Supplier Name");
        //        txtSupplierNameReport.Text = "";
        //        txtSupplierNameReport.Focus();
        //    }
        //    else
        //    {
        //        string strSupplierId = txtSupplierNameReport.Text.Trim().Split('/')[1].ToString();
        //        if (strSupplierId != "0" && strSupplierId != "")
        //        {
        //            DataTable dtSup = ObjSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
        //            if (dtSup.Rows.Count > 0)
        //            {
        //                Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
        //            }
        //            else
        //            {
        //                DisplayMessage("First Set Supplier Details in Supplier Setup");
        //                txtSupplierNameReport.Text = "";
        //                txtSupplierNameReport.Focus();
        //                return;
        //            }

        //            if (Session["SupplierAccountId"].ToString() == "0" && Session["SupplierAccountId"].ToString() == "")
        //            {
        //                DisplayMessage("First Set Supplier Account in Supplier Setup");
        //                txtSupplierNameReport.Text = "";
        //                txtSupplierNameReport.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Supplier Name");
        //    txtSupplierNameReport.Focus();
        //}
    }
    protected void txtCustomerNameReport_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtCustomerNameReport.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtCustomerNameReport.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["CustomerAccountId"] = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        return;
                    }

                }
            }
        }
        catch
        {

        }
        DisplayMessage("Customer is not valid");
        txtCustomerNameReport.Text = "";
        txtCustomerNameReport.Focus();
        //if (txtCustomerNameReport.Text != "")
        //{
        //    try
        //    {
        //        txtCustomerNameReport.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Customer Name");
        //        txtCustomerNameReport.Text = "";
        //        txtCustomerNameReport.Focus();
        //        return;
        //    }

        //    DataTable dt = objContact.GetContactByContactName(txtCustomerNameReport.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Customer Name");
        //        txtCustomerNameReport.Text = "";
        //        txtCustomerNameReport.Focus();
        //    }
        //    else
        //    {
        //        string strCustomerId = txtCustomerNameReport.Text.Trim().Split('/')[1].ToString();
        //        if (strCustomerId != "0" && strCustomerId != "")
        //        {
        //            DataTable dtCus = ObjCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
        //            if (dtCus.Rows.Count > 0)
        //            {
        //                Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
        //            }
        //            else
        //            {
        //                DisplayMessage("First Set Customer Details in Customer Setup");
        //                txtCustomerNameReport.Text = "";
        //                txtCustomerNameReport.Focus();
        //                return;
        //            }

        //            if (Session["CustomerAccountId"].ToString() == "0" && Session["CustomerAccountId"].ToString() == "")
        //            {
        //                DisplayMessage("First Set Customer Account in Customer Setup");
        //                txtCustomerNameReport.Text = "";
        //                txtCustomerNameReport.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Customer Name");
        //    txtCustomerNameReport.Focus();
        //}
    }
    protected void txtReconciledByReport_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtReconciledByReport.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtReconciledByReport.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                //hdnEmployeeId.Value = strEmployeeId;
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtReconciledByReport.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReconciledByReport);
            }
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(StrCompId, strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(StrCompId, retval);
                if (dtEmp.Rows.Count > 0)
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
    protected void btnReportAll_Click(object sender, EventArgs e)
    {
        DateTime dtToDate = new DateTime();
        if (txtFromDateReport.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDateReport.Text);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtFromDateReport.Focus();
                return;
            }
        }

        if (txtToDateReport.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDateReport.Text);
                dtToDate = Convert.ToDateTime(txtToDateReport.Text);
                dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtToDateReport.Focus();
                return;
            }
        }

        if (txtFromDateReport.Text != "" && txtToDateReport.Text != "")
        {
            if (Convert.ToDateTime(txtFromDateReport.Text) > Convert.ToDateTime(txtToDateReport.Text))
            {
                DisplayMessage("from date should be less then to date ");
                txtFromDateReport.Focus();
                return;
            }
        }

        if (txtFromDateReport.Text != "")
        {
            if (txtToDateReport.Text != "")
            {

            }
            else
            {
                DisplayMessage("To date should be Enter");
                txtToDateReport.Focus();
                return;
            }
        }

        string strReportType = string.Empty;
        string strReconciledBy = string.Empty;
        string strAccountId = string.Empty;
        string strOtherAccountId = "0";
        string strReconciledNo = string.Empty;

        if (rbHeaderReport.Checked == true)
        {
            strReportType = "Header";
        }
        else if (rbDetailReport.Checked == true)
        {
            strReportType = "Detail";
        }
        else if (rbByVoucherReport.Checked == true)
        {
            strReportType = "ByVoucher";
        }
        else
        {
            DisplayMessage("Need to Select One Report Type");
            rbHeaderReport.Focus();
            return;
        }

        if (txtAccountNameReport.Text != "" && txtAccountNameReport.Text != "0")
        {
            strAccountId = txtAccountNameReport.Text.Split('/')[1].ToString();
        }
        else
        {
            strAccountId = "0";
        }

        if (txtReconciledByReport.Text != "" && txtReconciledByReport.Text != "0")
        {
            strReconciledBy = txtReconciledByReport.Text.Split('/')[1].ToString();
        }
        else
        {
            strReconciledBy = "0";
        }

        if (txtSupplierNameReport.Visible == true)
        {
            if (txtSupplierNameReport.Text != "" && txtSupplierNameReport.Text != "0")
            {
                strOtherAccountId = txtSupplierNameReport.Text.Split('/')[1].ToString();
            }
            else
            {
                strOtherAccountId = "0";
            }
        }

        if (txtCustomerNameReport.Visible == true)
        {
            if (txtCustomerNameReport.Text != "" && txtCustomerNameReport.Text != "0")
            {
                strOtherAccountId = txtCustomerNameReport.Text.Split('/')[1].ToString();
            }
            else
            {
                strOtherAccountId = "0";
            }
        }

        if (txtReconciledNoReport.Text != "")
        {
            strReconciledNo = txtReconciledNoReport.Text;
        }
        else
        {
            strReconciledNo = "0";
        }

        DataTable dtReport = new DataTable();
        if (strReportType == "Header")
        {
            dtReport = ObjReconcileHeader.GetReconcileHeaderReport(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtFromDateReport.Text, txtToDateReport.Text);
            if (dtReport.Rows.Count > 0)
            {
                if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0" && strOtherAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0" && strReconciledBy != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledBy != "0")
                {
                    dtReport = new DataView(dtReport, "Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strAccountId != "0" && strOtherAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "Account_No='" + strAccountId + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }
        else
        {
            dtReport = ObjReconcileDetail.GetReconcileDetailReport(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtFromDateReport.Text, txtToDateReport.Text);
            if (dtReport.Rows.Count > 0)
            {
                if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0" && strOtherAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0" && strReconciledBy != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledNo != "0")
                {
                    dtReport = new DataView(dtReport, "ReconcilationNo='" + strReconciledNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strReconciledBy != "0")
                {
                    dtReport = new DataView(dtReport, "Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strAccountId != "0" && strOtherAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (strAccountId != "0")
                {
                    dtReport = new DataView(dtReport, "Account_No='" + strAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }


        if (dtReport.Rows.Count > 0)
        {
            ArrayList objArr = new ArrayList();
            objArr.Add(strReconciledNo);
            objArr.Add(strReconciledBy);
            objArr.Add(strAccountId);
            objArr.Add(strOtherAccountId);
            objArr.Add(txtFromDateReport.Text);
            objArr.Add(txtToDateReport.Text);
            Session["dtAcRParam"] = objArr;

            if (strReportType == "Header")
            {
                string strCmd = string.Format("window.open('../Accounts_Report/ReconciledReportHeader.aspx','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else if (strReportType == "Detail")
            {
                string strCmd = string.Format("window.open('../Accounts_Report/ReconciledReportDetail.aspx','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else if (strReportType == "ByVoucher")
            {
                string strCmd = string.Format("window.open('../Accounts_Report/ReconciledReportByVoucher.aspx','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
        }
        else
        {
            DisplayMessage("You have no Record According to Parameter");
            return;
        }
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strReconcileId = e.CommandArgument.ToString();
        if (strReconcileId != "0" && strReconcileId != "")
        {
            DataTable dtDetail = ObjReconcileDetail.GetReconcileDetailReportByVoucherOnly(strReconcileId);
            if (dtDetail.Rows.Count > 0)
            {
                ArrayList objArr = new ArrayList();
                objArr.Add(strReconcileId);
                Session["dtAcRVParam"] = objArr;

                string strCmd = string.Format("window.open('../Accounts_Report/ReconcileReportPrint.aspx','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else
            {
                DisplayMessage("You have no Record According to Parameter");
                return;
            }
        }
    }
    #endregion

    #region VoucherView
    protected void lnkViewVoucherDetail_Command(object sender, CommandEventArgs e)
    {
        bool isEdit = false;
        string strTransId = e.CommandArgument.ToString();
        if (((LinkButton)sender).ID == "btnEdit")
        {
            isEdit = true;
        }
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
                    txtVFinanceCode.Text = dtFy.Rows[0]["Finance_Code"].ToString() + "/" + dtFy.Rows[0]["Trans_Id"].ToString();
                }
            }

            string strToLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
            if (strToLocationId != "0" && strToLocationId != "")
            {
                txtVToLocation.Text = GetLocationName(strToLocationId) + "/" + strToLocationId;
            }
            else
            {
                txtVToLocation.Text = "";
            }

            string strDepartmentId = dtVoucherEdit.Rows[0]["Department_Id"].ToString();
            if (strDepartmentId != "0" && strDepartmentId != "")
            {
                txtVDepartment.Text = GetDepartmentName(strDepartmentId) + "/" + strDepartmentId;
            }
            else
            {
                txtVDepartment.Text = "";
            }

            txtVVoucherNo.Text = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
            txtVVoucherDate.Text = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(objsys.SetDateFormat());

            strCashCheque = dtVoucherEdit.Rows[0]["Field2"].ToString();
            if (strCashCheque == "Cash")
            {
                rbVCashPayment.Checked = true;
                rbVChequePayment.Checked = false;
                rbVCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "Cheque")
            {
                rbVChequePayment.Checked = true;
                rbVCashPayment.Checked = false;
                rbVCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "")
            {
                rbVCashPayment.Checked = true;
                rbVCashPayment_CheckedChanged(null, null);
            }

            strVoucherType = dtVoucherEdit.Rows[0]["Voucher_Type"].ToString();
            if (strVoucherType != "0")
            {
                ddlVVoucherType.SelectedValue = strVoucherType;
                ddlVVoucherType.Enabled = false;
            }
            else
            {
                ddlVVoucherType.SelectedValue = "--Select--";
            }

            string strChequeIssueDate = dtVoucherEdit.Rows[0]["Cheque_Issue_Date"].ToString();
            if (strChequeIssueDate != "" && strChequeIssueDate != "1/1/1800")
            {
                txtVChequeIssueDate.Text = Convert.ToDateTime(strChequeIssueDate).ToString(objsys.SetDateFormat());
            }
            string strChequeClearDate = dtVoucherEdit.Rows[0]["Cheque_Clear_Date"].ToString();
            if (strChequeClearDate != "" && strChequeClearDate != "1/1/1800")
            {
                txtVChequeClearDate.Text = Convert.ToDateTime(strChequeClearDate).ToString(objsys.SetDateFormat());
            }
            txtVChequeNo.Text = dtVoucherEdit.Rows[0]["Cheque_No"].ToString();

            txtVReference.Text = dtVoucherEdit.Rows[0]["RefrenceNo"].ToString();
            string strCurrencyId = dtVoucherEdit.Rows[0]["Currency_Id"].ToString();
            //ddlCurrency.SelectedValue = strCurrencyId;

            //txtExchangeRate.Text = dtVoucherEdit.Rows[0]["Exchange_Rate"].ToString();
            //txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            //Add Child Concept
            GvVDetail.DataSource = null;
            GvVDetail.DataBind();

            string strCurrency = Session["LocCurrencyId"].ToString();
            DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            if (dtDetail.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvVDetail, dtDetail, "", "");

                //For Total
                double sumDebit = 0;
                double sumCredit = 0;
                foreach (GridViewRow gvr in GvVDetail.Rows)
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

                Label lblDebitTotal = (Label)(GvVDetail.FooterRow.FindControl("lblgvDebitTotal"));
                Label lblCreditTotal = (Label)(GvVDetail.FooterRow.FindControl("lblgvCreditTotal"));

                lblDebitTotal.Text = sumDebit.ToString();
                lblCreditTotal.Text = sumCredit.ToString();

                lblDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDebitTotal.Text);
                lblCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblCreditTotal.Text);
            }
        }

        //pnl1.Visible = true;
        //pnl2.Visible = true;
        //ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Voucher_Detail_Popup()", true);
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
    protected void btnCancelPopLeave_Click(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        Reset();
    }
    protected void rbVCashPayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rbVCashPayment.Checked == true)
        {
            trVCheque1.Visible = false;
            trVCheque2.Visible = false;
            txtVChequeIssueDate.Text = "";
            txtVChequeClearDate.Text = "";
            txtVChequeNo.Text = "";
        }
        else if (rbVChequePayment.Checked == true)
        {
            trVCheque1.Visible = true;
            trVCheque2.Visible = true;
        }
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
    public void Reset()
    {
        FillGrid();
        txtVVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        ddlVVoucherType.Enabled = true;
        ddlVVoucherType.SelectedValue = "--Select--";
        txtVReference.Text = "0";
        PnlNewEdit.Enabled = true;

        //txtExchangeRate.Text = "";
        txtVChequeIssueDate.Text = "";
        txtVChequeClearDate.Text = "";
        txtVChequeNo.Text = "";

        GvVDetail.DataSource = null;
        GvVDetail.DataBind();

        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";

        rbVCashPayment.Checked = true;
        rbVChequePayment.Checked = false;
        rbVCashPayment_CheckedChanged(null, null);

    }
    #endregion

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (UploadFile.HasFile)
        {
            string filepath = string.Empty;
            int b = 0;
            string DirectoryName;
            filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString() + "/" + UploadFile.FileName;
            CreateDirectoryIfNotExist(Server.MapPath("~/" + "ArcaWing/" + Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString()));

            DirectoryName = Session["CompId"].ToString() + "/ReconciledDocument/" + hdnReconcileId.Value + "/" + ddlDocumentName.SelectedValue.ToString();
            DataTable dtDir = objDir.GetDirectoryMaster_By_DirectoryName(StrCompId, DirectoryName);
            if (dtDir.Rows.Count == 0)
            {
                b = objDir.InsertDirectorymaster(StrCompId, DirectoryName, "1", "0", hdnReconcileId.Value.Trim(), "ReconciledDocument", "0", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),Session["EmpId"].ToString());
            }
            else
            {
                b = Convert.ToInt32(dtDir.Rows[0]["Id"].ToString());
            }
            UploadFile.SaveAs(Server.MapPath(filepath));
        }
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvReconcile, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvReconcile, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
}