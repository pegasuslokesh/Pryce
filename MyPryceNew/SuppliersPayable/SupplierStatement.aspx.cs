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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;

public partial class SuppliersPayable_SupplierStatement : System.Web.UI.Page
{
    CompanyMaster ObjCompany = null;
    Set_Location_Department objLocDept = null;
    Set_CustomerMaster ObjCoustmer = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;

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

    Ac_Voucher_Header objVoucherHeader = null;
    Ems_ContactMaster objContact = null;
    Ac_Finance_Year_Info objFYI = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Reconcile_Detail objReconcileDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    int fyear_id = 0;
    Inv_ParameterMaster objInvParam = null;
    Dictionary<string, string> Reconcile_Colors = new Dictionary<string, string>();
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
        objReconcileDetail = new Ac_Reconcile_Detail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayable/SupplierStatement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "308").ToString() == "False")
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


            if (Request.QueryString["Id"] != null)
            {
                DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(Request.QueryString["Id"].ToString());
                txtSupplierName.Text = _dtTemp.Rows[0]["Name"].ToString() + "/" + _dtTemp.Rows[0]["Trans_Id"].ToString();
                txtFromDate.Text = Session["From"].ToString();
                txtToDate.Text = Session["To"].ToString();
                string strLocation = Session["SupLocId"].ToString();
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
                SetColorCode();
                btnGetReport_Click(sender, e);
            }
            hdnLocationIds.Value = Session["LocId"].ToString();
            Session["dtAcParam"] = null;
            Session["dtSupplierStatement"] = null;
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        SetColorCode();
        //if (tblRepeater.DataSource!=null)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "gridViewToDataTable()", true);
        //}

    }
    protected void SetColorCode()
    {
        //ForColoCodes
        if (ViewState["Reconcile_Colors"] != null)
        {
            return;
        }
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
        Reconcile_Colors.Clear();
        Reconcile_Colors.Add("Reconciled", "#" + txtReconciled.Text);
        Reconcile_Colors.Add("Conflicted", "#" + txtConflicted.Text);
        Reconcile_Colors.Add("NotReconciled", "#" + txtNotReconciled.Text);
        ViewState["Reconcile_Colors"] = Reconcile_Colors;

    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        //div_inAactive_account.Visible = clsPagePermission.bViewAllUserRecord;
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        ////New Code created by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("308");
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
                        Session["SupplierCurrency"] = dt.Rows[0]["Currency_Id"].ToString();
                        chkAgeingAnalysis_CheckedChanged(null, null);
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
        txtSupplierName.Text = "";
        DateTime dt = DateTime.Now;
        DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
        txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

        txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        txtForeignCreditOpening.Text = "";
        txtClosingCredit.Text = "";
        txtForeignCreditClosing.Text = "";
        txtCreditBalanceAmount.Text = "";

        GVSStatement.DataSource = null;
        GVSStatement.DataBind();

        chkAgeingAnalysis.Checked = false;
        tdAgeing.Visible = false;
        hdnAgeing.Value = "0";
        txt0to30.Text = "";
        txt31to60.Text = "";
        txt61to90.Text = "";
        txt91to180.Text = "";
        txt181to365.Text = "";
        txtAbove365.Text = "";
        //btnShowReport.Enabled = false;
        FillLocation();
        //SetColorCode();
    }
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        GVSStatement.DataSource = null;
        GVSStatement.DataBind();
        hdnCurrencyType.Value = "1";
        hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();
        Session["dtSupplierStatement"] = null;
        Session["dtAcParam"] = null;
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
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(),Session["CompId"].ToString());

        if (fyear_id == 0)
        {
            DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
            return;
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
        else
        {
            DisplayMessage("Need to Fill To Date");
            txtFromDate.Focus();
            return;
        }

        txtSupplierName_TextChanged(sender, e);
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

        if (strLocationId == "")
        {
            strLocationId = Session["LocId"].ToString();
        }

        //Check Location Currency
        string strCurrencyIdNew = string.Empty;
        string strFlag = "True";
        if (strLocationId != "")
        {
            string sql = "select distinct field1 from set_locationMaster where location_id in(" + strLocationId + ")";
            DataTable dtLocationData = da.return_DataTable_Modify(sql);
            strFlag = dtLocationData.Rows.Count > 1 ? "False" : "True";
        }

        if (strFlag == "True")
        {
            hdnCurrencyType.Value = "1";
            hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();

        }
        else if (strFlag == "False")
        {
            hdnCurrencyType.Value = "2";
            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                hdnCurrencyId.Value = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }

        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
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

        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
            return;
        }

        string strCurrency = Session["LocCurrencyId"].ToString(); ;

        string strSta = string.Empty;
        //For Opening Balance Start
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
            double OpeningBalance = 0;
            PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
            DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strPaymentVoucherAcc + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "','" + hdnCurrencyType.Value + "', '" + fyear_id + "')) OpeningBalance");
            string strForeignOpening = objDA.get_SingleValue("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strPaymentVoucherAcc + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "',3, '" + fyear_id + "')) OpeningBalance");
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
            }

            if (OpeningBalance != 0)
            {
                string DOB = OpeningBalance.ToString();
                txtOpeningDebit.Text = objsys.GetCurencyConversionForInv(strCurrency, DOB);
                txtForeignCreditOpening.Text = objsys.GetCurencyConversionForInv(strCurrency, strForeignOpening);
            }
            else
            {
                string DOB = OpeningBalance.ToString();
                txtOpeningDebit.Text = objsys.GetCurencyConversionForInv(strCurrency, DOB);
                txtForeignCreditOpening.Text = objsys.GetCurencyConversionForInv(strCurrency, strForeignOpening);
            }
        }
        txtClosingCredit.Text = txtOpeningDebit.Text;
        txtForeignCreditClosing.Text = txtForeignCreditOpening.Text;

        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, hdnCurrencyType.Value);
        if (dtStatement.Rows.Count == 0)
        {
            return;
        }

        if (ddlSVoucherType.SelectedValue != "--Select--")
        {
            dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
            if (dtStatement.Rows.Count > 0)
            {
                chkAgeingAnalysis.Visible = true;
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
            else
            {
                chkAgeingAnalysis.Visible = false;
            }
        }
        else
        {
            chkAgeingAnalysis.Visible = true;
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

        Reconcile_Colors = (Dictionary<string, string>)ViewState["Reconcile_Colors"];
        //List<ClsSupplierStatement> objStatementList = getObjSupplierStatementList(dtStatement, txtOpeningDebit.Text, txtForeignCreditOpening.Text, Reconcile_Colors);
        //if (objStatementList.Count > 0)
        //{
        //    GVSStatement.DataSource = objStatementList;
        //    GVSStatement.DataBind();
        //    ClsSupplierStatement obj = objStatementList.Last();
        //    txtClosingCredit.Text = obj.L_Balance;
        //    txtForeignCreditClosing.Text = obj.F_Balance;
        //    Label lblgvDebitTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvDebitTotal");
        //    Label lblgvCreditTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvCreditTotal");
        //    lblgvDebitTotal.Text = double.Parse(obj.Debit_Total) > 0 ? obj.Debit_Total : "";
        //    lblgvCreditTotal.Text = double.Parse(obj.Credit_Total) > 0 ? obj.Credit_Total : "";

        //    ArrayList objArr = new ArrayList();
        //    objArr.Add(txtSupplierName.Text.Split('/')[1].ToString());
        //    objArr.Add(txtFromDate.Text);
        //    objArr.Add(txtToDate.Text);
        //    objArr.Add(hdnLocationIds.Value);
        //    objArr.Add(txtFromDate.Text);
        //    objArr.Add(strPaymentVoucherAcc);
        //    objArr.Add(chkAgeingAnalysis.Checked.ToString());
        //    objArr.Add(txt0to30.Text);
        //    objArr.Add(txt31to60.Text);
        //    objArr.Add(txt61to90.Text);
        //    objArr.Add(txt91to180.Text);
        //    objArr.Add(txt181to365.Text);
        //    objArr.Add(txtAbove365.Text);
        //    objArr.Add(ddlSVoucherType.SelectedValue);
        //    objArr.Add(hdnCurrencyType.Value);
        //    objArr.Add(hdnCurrencyId.Value);
        //    objArr.Add(txtOpeningDebit.Text);
        //    objArr.Add(txtForeignCreditOpening.Text);
        //    Session["dtAcParam"] = objArr;
        //    Session["dtSupplierStatement"] = dtStatement;

        //    //tblRepeater.DataSource = objStatementList;
        //    //tblRepeater.DataBind();
        //    //(tblRepeater.Controls[tblRepeater.Controls.Count - 1].Controls[0].FindControl("lblDrTotal") as Label).Text = obj.Debit_Total;
        //    //(tblRepeater.Controls[tblRepeater.Controls.Count - 1].Controls[0].FindControl("lblCrTotal") as Label).Text = obj.Credit_Total;
        //    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "gridViewToDataTable()", true);
        //}

    }
    protected void chkAgeingAnalysis_CheckedChanged(object sender, EventArgs e)
    {
        if (GVSStatement.Rows.Count == 0)
        {
            return;
        }
        if (chkAgeingAnalysis.Checked == true)
        {
            hdnAgeing.Value = "True";
            tdAgeing.Visible = true;
            lblAgeingAnalysis.Text = "";
            lblAgeingAnalysis.Text = SystemParameter.GetCurrencySmbol(hdnCurrencyId.Value, Resources.Attendance.Ageing_Analysis, Session["DBConnection"].ToString());

            if (txtSupplierName.Text != "")
            {
                DataTable dtAgeingDetail = ObjAgeingDetail.GetAgeingDetailAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (dtAgeingDetail.Rows.Count > 0)
                {
                    dtAgeingDetail = new DataView(dtAgeingDetail, "other_account_no='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and AgeingType='PV'", "", DataViewRowState.CurrentRows).ToTable();
                }

                //For 0-30 Days
                try
                {
                    DataTable dt30Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "4", hdnCurrencyType.Value);
                    if (dt30Days.Rows.Count > 0)
                    {
                        txt0to30.Text = dt30Days.Rows[0][0].ToString();
                        if (txt0to30.Text == "")
                        {
                            txt0to30.Text = "0.00";
                        }
                        txt0to30.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt0to30.Text);

                    }
                    else
                    {
                        txt0to30.Text = "0.00";
                        txt0to30.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt0to30.Text);
                    }
                }
                catch
                {
                    txt0to30.Text = "0.00";
                    txt0to30.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt0to30.Text);
                }

                //For 31-60 Days
                try
                {
                    DataTable dt60Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "5", hdnCurrencyType.Value);
                    if (dt60Days.Rows.Count > 0)
                    {
                        txt31to60.Text = dt60Days.Rows[0][0].ToString();
                        if (txt31to60.Text == "")
                        {
                            txt31to60.Text = "0.00";
                        }
                        txt31to60.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt31to60.Text);
                    }
                    else
                    {
                        txt31to60.Text = "0.00";
                        txt31to60.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt31to60.Text);
                    }
                }
                catch
                {
                    txt31to60.Text = "0.00";
                    txt31to60.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt31to60.Text);
                }

                //For 61-90 Days
                try
                {
                    DataTable dt90Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "6", hdnCurrencyType.Value);
                    if (dt90Days.Rows.Count > 0)
                    {
                        txt61to90.Text = dt90Days.Rows[0][0].ToString();
                        if (txt61to90.Text == "")
                        {
                            txt61to90.Text = "0.00";
                        }
                        txt61to90.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt61to90.Text);
                    }
                    else
                    {
                        txt61to90.Text = "0.00";
                        txt61to90.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt61to90.Text);
                    }
                }
                catch
                {
                    txt61to90.Text = "0.00";
                    txt61to90.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt61to90.Text);
                }

                //For 91-180 Days
                try
                {
                    DataTable dt180Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "7", hdnCurrencyType.Value);
                    if (dt180Days.Rows.Count > 0)
                    {
                        txt91to180.Text = dt180Days.Rows[0][0].ToString();
                        if (txt91to180.Text == "")
                        {
                            txt91to180.Text = "0.00";
                        }
                        txt91to180.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt91to180.Text);
                    }
                    else
                    {
                        txt91to180.Text = "0.00";
                        txt91to180.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt91to180.Text);
                    }
                }
                catch
                {
                    txt91to180.Text = "0.00";
                    txt91to180.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt91to180.Text);
                }

                //For 181-365 Days
                try
                {
                    DataTable dt365Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "8", hdnCurrencyType.Value);
                    if (dt365Days.Rows.Count > 0)
                    {
                        txt181to365.Text = dt365Days.Rows[0][0].ToString();
                        if (txt181to365.Text == "")
                        {
                            txt181to365.Text = "0.00";
                        }
                        txt181to365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt181to365.Text);
                    }
                    else
                    {
                        txt181to365.Text = "0.00";
                        txt181to365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt181to365.Text);
                    }
                }
                catch
                {
                    txt181to365.Text = "0.00";
                    txt181to365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txt181to365.Text);
                }

                //For 181-365 Days
                try
                {
                    DataTable dt365DaysAbove = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, txtSupplierName.Text.Split('/')[1].ToString(), "PV", "9", hdnCurrencyType.Value);
                    if (dt365DaysAbove.Rows.Count > 0)
                    {
                        txtAbove365.Text = dt365DaysAbove.Rows[0][0].ToString();
                        if (txtAbove365.Text == "")
                        {
                            txtAbove365.Text = "0.00";
                        }
                        txtAbove365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txtAbove365.Text);
                    }
                    else
                    {
                        txtAbove365.Text = "0.00";
                        txtAbove365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txtAbove365.Text);
                    }
                }
                catch
                {
                    txtAbove365.Text = "0.00";
                    txtAbove365.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, txtAbove365.Text);
                }
            }

            //SetColorCode();
            //double debitamount = 0;
            //double creditamount = 0;
            //foreach (GridViewRow gvr in GVSStatement.Rows)
            //{
            //    HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
            //    Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            //    Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
            //    lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, lblgvDebitAmount.Text);
            //    lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(hdnCurrencyId.Value, lblgvCreditAmount.Text);
            //    debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
            //    creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
            //    Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");
            //    Label lblgvFregnAmount = (Label)gvr.FindControl("lblgvForeignAmount");
            //    Label lblgvFregnBalance = (Label)gvr.FindControl("lblgvFVBalance");

            //    if (hdngvDetailId.Value != "" && hdngvDetailId.Value != "0")
            //    {
            //        DataTable dtRDetail = objReconcileDetail.GetReconcileDetailAllDataOnly();
            //        if (dtRDetail != null)
            //        {
            //            if (dtRDetail.Rows.Count > 0)
            //            {
            //                dtRDetail = new DataView(dtRDetail, "VD_Id='" + hdngvDetailId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //                if (dtRDetail.Rows.Count > 0)
            //                {
            //                    string status = dtRDetail.Rows[0]["Is_Reconciled"].ToString();
            //                    if (status == "True")
            //                    {
            //                        gvr.BackColor = System.Drawing.Color.FromName("#" + txtReconciled.Text);
            //                    }
            //                    else if (status == "False")
            //                    {
            //                        gvr.BackColor = System.Drawing.Color.FromName("#" + txtConflicted.Text);
            //                    }
            //                }
            //                else
            //                {
            //                    gvr.BackColor = System.Drawing.Color.FromName("#" + txtNotReconciled.Text);
            //                }
            //            }
            //        }
            //    }
            //}
        }
        else
        {
            hdnAgeing.Value = "0";
            tdAgeing.Visible = false;
            txt0to30.Text = "";
            txt31to60.Text = "";
            txt61to90.Text = "";
            txt91to180.Text = "";
            txt181to365.Text = "";
            txtAbove365.Text = "";
        }
        //btnGetReport_Click(null, null);
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

    protected void lblgvVoucherNo_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
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
            }
            else
            {
                DisplayMessage("You have no permission for view detail");
                return;
            }
        }
        else if (RefId != "0" && RefId != "")
        {
            if (RefType == "PINV")
            {
                if (IsObjectPermission("143", "48"))
                {
                    string strCmd = string.Format("window.open('../Purchase/PurchaseInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
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
    public static string GetJsonSupplierStatement(string strSupplier, string strFromDate, string strToDate, string strLocationIds, string strVoucherType)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            //validate supplier name
            Ac_AccountMaster objAcAccountMaster = new Ac_AccountMaster(HttpContext.Current.Session["DBConnection"].ToString());
            int otherAccountId = 0;
            int.TryParse(strSupplier.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                using (DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString()))
                {
                    if (dt.Rows.Count > 0 && dt.Rows[0]["Name"].ToString().ToUpper() == strSupplier.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        HttpContext.Current.Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());
                        HttpContext.Current.Session["SupplierCurrency"] = dt.Rows[0]["Currency_Id"].ToString();
                    }
                    else
                    {
                        otherAccountId = 0;
                    }
                }
            }
            if (otherAccountId == 0)
            {
                throw new Exception("Supplier name is not valid");
            }

            //validate fromdate and todate
            DateTime dtFromDate;
            DateTime dtToDate;
            if(!DateTime.TryParse(strFromDate.ToString(),out dtFromDate))
            {
                throw new Exception("From date is not valid");
            }
            if(!DateTime.TryParse(strToDate.ToString(), out dtToDate))
            {
                throw new Exception("To date is not valid");
            }

            PegasusDataAccess.DataAccessClass objDa = new PegasusDataAccess.DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());
            
            //get financial year id
            int fyear_id = 0;
            fyear_id = Common.getFinancialYearId(Convert.ToDateTime(strFromDate),HttpContext.Current.Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString());
            if (fyear_id==0)
            {
                throw new Exception("From Date is not belong to any financial year");
            }


            //Get currency type if multiple location has been selected and having same currency then currency type should 1 other wise 2
            string strCurrencyType = "1";
            if (strLocationIds != "")
            {
                string sql = "select distinct field1 from set_locationMaster where location_id in(" + strLocationIds + ")";
                DataTable dtLocationData = objDa.return_DataTable_Modify(sql);
                strCurrencyType = dtLocationData.Rows.Count > 1 ? "2" : "1";
                dtLocationData = null;
            }


            Ac_Voucher_Detail objVoucherDetail = new Ac_Voucher_Detail(HttpContext.Current.Session["DBConnection"].ToString());
            clsSupplierStatement clsSupplierStatement = new clsSupplierStatement();

            //get local opening balance
            double lOb = 0;
            double.TryParse(objDa.get_SingleValue("select (dbo.Ac_GetBalance('" + HttpContext.Current.Session["CompId"].ToString() + "', '" + HttpContext.Current.Session["BrandId"].ToString() + "','" + strLocationIds + "', '" + strFromDate + "', '0','" + strPaymentVoucherAcc + "','" + strSupplier.Split('/')[1].ToString() + "'," + strCurrencyType + ", '" + fyear_id + "')) OpeningBalance"), out lOb);
            string strLOb = lOb <= 0 ? Math.Abs(lOb).ToString() : "-" +  lOb.ToString();
            //get foreign opening balance
            double fOb = 0;
            double.TryParse(objDa.get_SingleValue("select (dbo.Ac_GetBalance('" + HttpContext.Current.Session["CompId"].ToString() + "', '" + HttpContext.Current.Session["BrandId"].ToString() + "','" + strLocationIds + "', '" + strFromDate + "', '0','" + strPaymentVoucherAcc + "','" + strSupplier.Split('/')[1].ToString() + "',3, '" + fyear_id + "')) OpeningBalance"), out fOb);
            string strFOb = fOb <= 0 ? Math.Abs(fOb).ToString() : "-" + fOb.ToString();

            //get statement data from voucher detail
            DataTable dtStatement = objVoucherDetail.GetAllStatementData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), strLocationIds, Ac_ParameterMaster.GetSupplierAccountNo(HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString()), strSupplier.Split('/')[1].ToString(), strFromDate, strToDate, "1");
            if(strVoucherType!="" && strVoucherType != "--Select--")
            {
                dtStatement = new DataView(dtStatement, "voucher_type='" + strVoucherType + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            List<clsSupplierStatementDetail> listObj = new List<clsSupplierStatementDetail> { };
            if (dtStatement.Rows.Count > 0)
            {
                listObj = getObjSupplierStatementList(dtStatement, strLOb, strFOb);
            }
            //return listObj;
            SystemParameter objSys = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
            strLOb = objSys.GetCurencyConversionForInv(HttpContext.Current.Session["LocCurrencyId"].ToString(), strLOb);
            strFOb = objSys.GetCurencyConversionForInv(HttpContext.Current.Session["SupplierCurrency"].ToString(), strFOb);
            clsSupplierStatement.lOb = clsSupplierStatement.lCb = strLOb;
            clsSupplierStatement.fOb = clsSupplierStatement.fCb = strFOb;
            clsSupplierStatement.clsSupplierStatementDetail = listObj;
            
            if (listObj.Count>0)
            {
                clsSupplierStatementDetail obj = listObj.Last();
                clsSupplierStatement.lCb = obj.lBalance;
                clsSupplierStatement.fCb = obj.fBalance;
                clsSupplierStatement.debitTotal = double.Parse(obj.debitTotal) > 0 ? obj.debitTotal : "";
                clsSupplierStatement.creditTotal = double.Parse(obj.creditTotal) > 0 ? obj.creditTotal : "";
            }

            //store data in session variable to print report
            ArrayList objArr = new ArrayList();
            objArr.Add(strSupplier.Split('/')[1].ToString());
            objArr.Add(strFromDate);
            objArr.Add(strToDate);
            objArr.Add(strLocationIds);
            objArr.Add(strFromDate);
            objArr.Add(strPaymentVoucherAcc);
            objArr.Add("false");
            objArr.Add("0");
            objArr.Add("0");
            objArr.Add("0");
            objArr.Add("0");
            objArr.Add("0");
            objArr.Add("0");
            objArr.Add(strVoucherType);
            objArr.Add(strCurrencyType);
            objArr.Add("");
            objArr.Add(strLOb);
            objArr.Add(strFOb);
            HttpContext.Current.Session["dtAcParam"] = objArr;
            HttpContext.Current.Session["dtSupplierStatement"] = dtStatement;

            return js.Serialize(clsSupplierStatement);

        }
        catch (Exception ex)
        {
            return ex.Message;
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
        //btnPushDept.Focus();
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
        //btnPullDept.Focus();
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
        //btnPushAllDept.Focus();
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
        //btnPullAllDept.Focus();
    }
    #endregion

    #region Print
    protected void btnShowProductReport_Click(object sender, EventArgs e)
    {
        if (GVSStatement.Rows.Count == 0 || Session["dtAcParam"] == null)
        {
            return;
        }

        string strCmd = string.Format("window.open('../Accounts_Report/SupplierStatementWithProductDetail.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        if (GVSStatement.Rows.Count == 0 || Session["dtAcParam"] == null)
        {
            return;
        }

        string strCmd = string.Format("window.open('../Accounts_Report/SupplierStatement.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnAllSupplier_Click(object sender, EventArgs e)
    {
        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text),Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
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

        //For Account Parameter
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

        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

        string strAllSupplierId = string.Empty;

        string Asql = "(SELECT STUFF((SELECT Distinct ',' + RTRIM(Other_Account_No) FROM Ac_Voucher_Detail where Account_No='" + strPaymentVoucherAcc + "' and Other_Account_No <>0  FOR XML PATH('')),1,1,'') )";
        DataTable dt = da.return_DataTable(Asql);
        if (dt.Rows.Count > 0)
        {
            strAllSupplierId = dt.Rows[0][0].ToString();
        }

        DataTable dtStatement = objVoucherDetail.GetStatementDetail();
        //dtStatement = new DataView(dtStatement, "Post='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (strLocationId != "" && strLocationId != "0")
        {
            dtStatement = new DataView(dtStatement, "Location_Id in (" + strLocationId + ") and  Other_Account_No in (" + strAllSupplierId + ") and Account_No in ('" + strCashAccount + "','" + strCreditAccount + "','" + strPaymentVoucherAcc + "')", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtStatement.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(strLocationId);
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);


        Session["dtAllSupParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AllSupplierDetail.aspx','window','width=1024, ');");
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
        Ac_Voucher_Header.clsVoucherHeader clsVh = Ac_Voucher_Header.getVoucherWithDetail(e.CommandArgument.ToString(),HttpContext.Current.Session["DBConnection"].ToString(),Session["CompId"].ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            //string strFinanceCode = dtVoucherEdit.Rows[0]["Finance_Code"].ToString();
            //if (strFinanceCode != "0" && strFinanceCode != "")
            //{
            //    DataTable dtFy = objFYI.GetInfoByTransId(StrCompId, strFinanceCode);
            //    if (dtFy.Rows.Count > 0)
            //    {
            //        txtFinanceCode.Text = dtFy.Rows[0]["Finance_Code"].ToString() + "/" + dtFy.Rows[0]["Trans_Id"].ToString();
            //    }
            //}

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
                //txtDepartment.Text = GetDepartmentName(strDepartmentId) + "/" + strDepartmentId;
            }
            else
            {
                //txtDepartment.Text = "";
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
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");
                    Label lblgvFregnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                    HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");


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
                    lblgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
                    lblgvFregnAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvFregnAmt.Text);

                    if (hdngvCurrencyId.Value != "0" && hdngvCurrencyId.Value != "")
                    {
                        //lblgvFregnAmt.Text = objsys.GetCurencyConversionForInv(((Label)Row.FindControl("hidExpCur")).Text, ((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                        //lblgvFregnAmt.Text = objsys.GetCurencySymbol(lblgvFregnAmt.Text, ViewState["CurrencyId"].ToString());
                        lblgvFregnAmt.Text = SystemParameter.GetCurrencySmbol(hdngvCurrencyId.Value, lblgvFregnAmt.Text, Session["DBConnection"].ToString());
                    }
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
        // ModalPopupExtender1.Show();

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

    }
    #endregion
    public class clsSupplierStatement
    {
        public string lOb { get; set; }
        public string fOb { get; set; }
        public string debitTotal { get; set; }
        public string creditTotal { get; set; }
        public string lCb { get; set; }
        public string fCb { get; set; }
        public List<clsSupplierStatementDetail> clsSupplierStatementDetail { get; set; }
    }
    public class clsSupplierStatementDetail
    {
        public int headerTransId { get; set; }
        public int detailTransId { get; set; }
        public string voucherNo { get; set; }
        public string voucherDate { get; set; }
        public string voucherType { get; set; }
        public string narration { get; set; }
        public string referenceNumber { get; set; }
        public string debitAmount { get; set; }
        public string creditAmount { get; set; }
        public string lBalance { get; set; }
        public string foreignAmount { get; set; }
        public string fBalance { get; set; }
        public string refType { get; set; }
        public string refId { get; set; }
        public int locationId { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public string reconciledStatus { get; set; }
        public string creditTotal { get; set; }
        public string debitTotal { get; set; }
        public string Inv_Number { get; set; }
        public string Inv_Date { get; set; }
    }


    public static List<clsSupplierStatementDetail> getObjSupplierStatementList(DataTable dt, string strLocalOb, string strForeignOb)
    {
        double localCb = 0;
        double foreignCb = 0;
        double.TryParse(strLocalOb, out localCb);
        double.TryParse(strForeignOb, out foreignCb);
        double debit_total = 0;
        double credit_total = 0;
        double debit_amount = 0;
        double credit_amount = 0;
        double foreign_amount = 0;
        List<clsSupplierStatementDetail> objList = new List<clsSupplierStatementDetail>();
        foreach (DataRow dr in dt.Rows)
        {
            clsSupplierStatementDetail obj = new clsSupplierStatementDetail();
            obj.headerTransId = int.Parse(dr["Header_Trans_Id"].ToString());
            obj.voucherNo = dr["Voucher_no"].ToString();
            obj.voucherDate = DateTime.Parse(dr["voucher_date"].ToString()).ToString("dd-MMM-yyyy");
            obj.voucherType = dr["Voucher_Type"].ToString();
            obj.narration = dr["Narration"].ToString();
            obj.referenceNumber = dr["RefrenceNumber"].ToString();
            double.TryParse(dr["Debit_Amount"].ToString(), out debit_amount);
            obj.debitAmount = debit_amount > 0 ? SystemParameter.GetAmountWithDecimal(dr["Debit_Amount"].ToString(), dr["LDecimalCount"].ToString()) : "";
            debit_total += debit_amount;
            obj.debitTotal = SystemParameter.GetAmountWithDecimal(debit_total.ToString(), dr["LDecimalCount"].ToString());
            double.TryParse(dr["Credit_Amount"].ToString(), out credit_amount);
            obj.creditAmount = credit_amount > 0 ? SystemParameter.GetAmountWithDecimal(dr["Credit_Amount"].ToString(), dr["LDecimalCount"].ToString()) : "";
            credit_total += credit_amount;
            obj.creditTotal = SystemParameter.GetAmountWithDecimal(credit_total.ToString(), dr["LDecimalCount"].ToString());
            localCb = localCb + credit_amount - debit_amount;
            obj.lBalance = SystemParameter.GetAmountWithDecimal(localCb.ToString(), dr["LDecimalCount"].ToString());
            obj.foreignAmount = SystemParameter.GetAmountWithDecimal(dr["Foreign_Amount"].ToString(), dr["FDecimalCount"].ToString());
            double.TryParse(credit_amount > 0 ? dr["Foreign_Amount"].ToString() : ("-" + obj.foreignAmount).ToString(), out foreign_amount);
            foreignCb = foreignCb + foreign_amount;
            obj.fBalance = SystemParameter.GetAmountWithDecimal(foreignCb.ToString(), dr["FDecimalCount"].ToString());
            obj.detailTransId = int.Parse(dr["Detail_Trans_Id"].ToString());
            obj.refId = dr["Ref_Id"].ToString();
            obj.refType = dr["Ref_Type"].ToString();
            obj.createdBy = dr["CreatedBy_User"].ToString();
            obj.modifiedBy = dr["ModifiedBy_User"].ToString();
            obj.locationId = int.Parse(dr["location_id"].ToString());
            if (dr["Is_Reconciled"].ToString() == "True")
            {
                obj.reconciledStatus = "Reconciled";
            }
            else if (dr["Is_Reconciled"].ToString() == "False")
            {
                obj.reconciledStatus = "Conflicted";
            }
            else
            {
                obj.reconciledStatus = "NotReconciled";
            }
            obj.Inv_Date= dr["Inv_Date"].ToString();
            obj.Inv_Number= dr["Inv_Number"].ToString();

            objList.Add(obj);
        }
        return objList;

    }
    //public static List<ClsSupplierStatement> getObjSupplierStatementList(Dictionary<string, string> param, string strLocalOb, string strForeignOb)
    //{
    //    using (DataTable dt = new DataTable())
    //    {
    //        List<ClsSupplierStatement> objList = new List<ClsSupplierStatement>();
    //        try
    //        {
    //            string _strOtherAccountId = "0";
    //            string _strContactName = string.Empty;
    //            DateTime _fromDate = new DateTime();
    //            DateTime _toDate = new DateTime();
    //            string _strLocations = string.Empty;
    //            string _voucherType = string.Empty;
    //            foreach (var item in param)
    //            {
    //                if (item.Key == "ContactName")
    //                {
    //                    _strOtherAccountId = item.Value.ToString().Split('/')[1].ToString();
    //                    _strContactName = item.Value.ToString().Split('/')[0].ToString();
    //                }
    //                if (item.Key == "FromDate")
    //                {
    //                    _fromDate = DateTime.Parse(item.Value);
    //                }
    //                if (item.Key == "ToDate")
    //                {
    //                    _toDate = DateTime.Parse(item.Value);
    //                }
    //                if (item.Key == "Locations")
    //                {
    //                    _strLocations = item.Value;
    //                }
    //                if (item.Key == "VoucherType")
    //                {
    //                    _voucherType = item.Value;
    //                }
    //            }
    //            Ac_Voucher_Detail objVoucherDetail = new Ac_Voucher_Detail();
    //            DataTable dtStatement = objVoucherDetail.GetAllStatementData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), _strLocations, Ac_ParameterMaster.GetSupplierAccountNo(HttpContext.Current.Session["CompId"].ToString()), _strOtherAccountId, _fromDate.ToString(), _toDate.ToString(), "1");
    //            if (_voucherType != "--Select--")
    //            {

    //            }
    //            double localCb = 0;
    //            double foreignCb = 0;
    //            double.TryParse(strLocalOb, out localCb);
    //            double.TryParse(strForeignOb, out foreignCb);
    //            double debit_total = 0;
    //            double credit_total = 0;
    //            double debit_amount = 0;
    //            double credit_amount = 0;

    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                ClsSupplierStatement obj = new ClsSupplierStatement();
    //                obj.Header_Trans_Id = int.Parse(dr["Header_Trans_Id"].ToString());
    //                obj.Voucher_No = dr["Voucher_no"].ToString();
    //                obj.Voucher_Date = DateTime.Parse(dr["voucher_date"].ToString()).ToString("dd-MMM-yyyy");
    //                obj.Voucher_Type = dr["Voucher_Type"].ToString();
    //                obj.Narration = dr["Narration"].ToString();
    //                obj.RefrenceNumber = dr["RefrenceNumber"].ToString();
    //                double.TryParse(dr["Debit_Amount"].ToString(), out debit_amount);
    //                obj.Debit_Amount = debit_amount > 0 ? SystemParameter.GetAmountWithDecimal(dr["Debit_Amount"].ToString(), dr["LDecimalCount"].ToString()) : "";
    //                debit_total += debit_amount;
    //                obj.Debit_Total = SystemParameter.GetAmountWithDecimal(debit_total.ToString(), dr["LDecimalCount"].ToString());
    //                double.TryParse(dr["Credit_Amount"].ToString(), out credit_amount);
    //                obj.Credit_Amount = credit_amount > 0 ? SystemParameter.GetAmountWithDecimal(dr["Credit_Amount"].ToString(), dr["LDecimalCount"].ToString()) : "";
    //                credit_total += credit_amount;
    //                obj.Credit_Total = SystemParameter.GetAmountWithDecimal(credit_total.ToString(), dr["LDecimalCount"].ToString());
    //                localCb = localCb + credit_amount - debit_amount;
    //                obj.L_Balance = SystemParameter.GetAmountWithDecimal(localCb.ToString(), dr["LDecimalCount"].ToString());
    //                obj.Foreign_Amount = SystemParameter.GetAmountWithDecimal(dr["Foreign_Amount"].ToString(), dr["FDecimalCount"].ToString());
    //                foreignCb = foreignCb + (credit_amount > 0 ? credit_amount : -debit_amount);
    //                obj.F_Balance = SystemParameter.GetAmountWithDecimal(foreignCb.ToString(), dr["FDecimalCount"].ToString());
    //                obj.Detail_Trans_Id = int.Parse(dr["Detail_Trans_Id"].ToString());
    //                obj.Ref_Id = dr["Ref_Id"].ToString();
    //                obj.Ref_Type = dr["Ref_Type"].ToString();
    //                obj.CreatedBy = dr["CreatedBy_User"].ToString();
    //                obj.ModifiedBy = dr["ModifiedBy_User"].ToString();
    //                objList.Add(obj);
    //            }
    //            return objList;

    //        }
    //        catch
    //        {
    //            return objList;
    //        }
    //    }
    //}
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetVoucherData(string strVoucherNo)
    {
        Ac_Voucher_Header.clsVoucherHeader clsVh = Ac_Voucher_Header.getVoucherWithDetail(strVoucherNo,HttpContext.Current.Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString());
        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(clsVh);
    }

}