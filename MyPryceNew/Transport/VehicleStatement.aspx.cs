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

public partial class Transport_VehicleStatement : System.Web.UI.Page
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
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();

    Ac_Voucher_Header objVoucherHeader = null;
    Ems_ContactMaster objContact = null;
    Ac_Finance_Year_Info objFYI = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Reconcile_Detail objReconcileDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    int fyear_id = 0;
    Inv_ParameterMaster objInvParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

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
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "389", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

            DateTime dt = DateTime.Now;
            DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
            txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());


            if (Request.QueryString["Id"] != null)
            {
                //DataTable dtSupName = objContact.GetContactTrueById(Request.QueryString["Id"].ToString());
                //if (dtSupName.Rows.Count > 0)
                //{
                //    txtVehicleName.Text = dtSupName.Rows[0]["Name"].ToString() + "/" + dtSupName.Rows[0]["Trans_Id"].ToString();
                //}

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
                btnGetReport_Click(sender, e);

                if (txtReconciled.Text == "")
                {
                    txtReconciled.Text = "33FFCC";
                    txtReconciled.ForeColor = System.Drawing.Color.FromName("#33FFCC");
                }
                if (txtConflicted.Text == "")
                {
                    txtConflicted.Text = "66FF66";
                    txtConflicted.ForeColor = System.Drawing.Color.FromName("#66FF66");
                }
                if (txtNotReconciled.Text == "")
                {
                    txtNotReconciled.Text = "FF33FF";
                    txtNotReconciled.ForeColor = System.Drawing.Color.FromName("#FF33FF");
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
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("389", (DataTable)Session["ModuleName"]);
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
            //btnGetReport.Visible = true;
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "389",Session["CompId"].ToString());
        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                //btnGetReport.Visible = true;
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
        ddlVehicleList.SelectedIndex = 0;
        DateTime dt = DateTime.Now;
        DateTime Fistdaydate = dt.AddDays(-(dt.Day - 1));
        txtFromDate.Text = Fistdaydate.ToString(objsys.SetDateFormat());

        txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        txtForeignCreditOpening.Text = "";
        txtClosingCredit.Text = "";
        txtForeignCreditClosing.Text = "";
        txtCreditBalanceAmount.Text = "";

        //txtSupCurrency.Text = "";
        //td1.Visible = false;
        //td2.Visible = false;
        //td3.Visible = false;

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

        FillLocation();
        SetColorCode();
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
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
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

        ////for Check Financial Year
        //if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtToDate.Text)))
        //{
        //    GVSStatement.DataSource = null;
        //    GVSStatement.DataBind();
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}

        //txtVehicleName_TextChanged(sender, e);
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
        string strPaymentVoucherAcc = string.Empty;
        DateTime dtToDate = new DateTime();

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Vehicle Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

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

        //if (txtVehicleName.Text == "")
        //{
        //    DisplayMessage("Fill Supplier Name");
        //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVehicleName);
        //    return;
        //}

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string strExchangeRate = "1";  //SystemParameter.GetExchageRate(strCurrency, Session["SupplierCurrency"].ToString());


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
            DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strPaymentVoucherAcc + "','" + ddlVehicleList.SelectedValue + "','" + strCurrencyType + "', '" + fyear_id + "')) OpeningBalance");
            string strForeignOpening = objDA.get_SingleValue("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strPaymentVoucherAcc + "','" + ddlVehicleList.SelectedValue + "',3, '" + fyear_id + "')) OpeningBalance");
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

        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strPaymentVoucherAcc, ddlVehicleList.SelectedValue, txtFromDate.Text, txtToDate.Text, strCurrencyType);
        if (dtStatement.Rows.Count > 0)
        {
            if (dtStatement.Rows.Count > 0)
            {
                if (ddlSVoucherType.SelectedValue != "--Select--")
                {
                    dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
                    if (dtStatement.Rows.Count > 0)
                    {
                        GVSStatement.DataSource = dtStatement;
                        GVSStatement.DataBind();
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
                        GVSStatement.DataSource = null;
                        GVSStatement.DataBind();
                    }
                }
                else
                {

                    GVSStatement.DataSource = dtStatement;
                    GVSStatement.DataBind();
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
            }
            else
            {
                chkAgeingAnalysis.Visible = false;
                GVSStatement.DataSource = null;
                GVSStatement.DataBind();
            }

            //Regrading Balance & Total         
            string strStatus = "False";
            string strBalanceA = string.Empty;
            string strBalanceF = string.Empty;
            double debitamount = 0;
            double creditamount = 0;
            double debitTotalamount = 0;
            double creditTotalamount = 0;
            string strSupCurrId = Session["LocCurrencyId"].ToString();
            SetColorCode();
            foreach (GridViewRow gvr in GVSStatement.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
                debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");
                Label lblgvFregnAmount = (Label)gvr.FindControl("lblgvForeignAmount");
                Label lblgvFregnBalance = (Label)gvr.FindControl("lblgvFVBalance");

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

                double OpeningD = Convert.ToDouble(txtOpeningDebit.Text);
                double FoeignOpen = Convert.ToDouble(txtForeignCreditOpening.Text);

                if (strStatus == "False")
                {
                    if (txtOpeningDebit.Text != "")
                    {
                        if (strSta == "Credit")
                        {
                            if (OpeningD != 0)
                            {
                                if (OpeningD > 0)
                                {
                                    lblgvBalance.Text = OpeningD.ToString();
                                }
                                else
                                {
                                    lblgvBalance.Text = txtOpeningDebit.Text.Split('-')[1].ToString();
                                }
                            }
                            else
                            {
                                lblgvBalance.Text = "0";
                            }
                            //lblgvBalance.Text = txtOpeningDebit.Text.Split('-')[1].ToString();
                            if (FoeignOpen != 0)
                            {
                                if (FoeignOpen > 0)
                                {
                                    lblgvFregnBalance.Text = FoeignOpen.ToString();
                                }
                                else
                                {
                                    lblgvFregnBalance.Text = txtForeignCreditOpening.Text.Split('-')[1].ToString();
                                }
                            }
                            else
                            {
                                lblgvFregnBalance.Text = "0";
                            }
                            strStatus = "True";
                        }
                        else if (strSta == "Debit")
                        {
                            lblgvBalance.Text = "-" + txtOpeningDebit.Text;
                            lblgvFregnBalance.Text = "-" + txtForeignCreditOpening.Text;
                            strStatus = "True";
                        }
                        else
                        {
                            lblgvBalance.Text = "0";
                            lblgvFregnBalance.Text = "0";
                            strStatus = "True";
                        }
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
                    lblgvFregnBalance.Text = strBalanceF;
                }

                if (debitamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(debitamount.ToString())).ToString();
                    lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                    strBalanceA = lblgvBalance.Text;

                    //lblgvFregnAmount.Text = objsys.GetCurencyConversionForInv(Session["SupplierCurrency"].ToString(), (float.Parse(strExchangeRate) * float.Parse(debitamount.ToString())).ToString());
                    lblgvFregnBalance.Text = (Convert.ToDouble(lblgvFregnBalance.Text) - Convert.ToDouble(lblgvFregnAmount.Text)).ToString();
                    lblgvFregnBalance.Text = objsys.GetCurencyConversionForInv(strSupCurrId, lblgvFregnBalance.Text);
                    strBalanceF = lblgvFregnBalance.Text;
                }
                else if (creditamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(creditamount.ToString())).ToString();
                    lblgvBalance.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalance.Text);
                    strBalanceA = lblgvBalance.Text;
                    //for Foreign
                    //lblgvFregnAmount.Text = objsys.GetCurencyConversionForInv(Session["SupplierCurrency"].ToString(), (float.Parse(strExchangeRate) * float.Parse(creditamount.ToString())).ToString());
                    lblgvFregnBalance.Text = (Convert.ToDouble(lblgvFregnBalance.Text) + Convert.ToDouble(lblgvFregnAmount.Text)).ToString();
                    lblgvFregnBalance.Text = objsys.GetCurencyConversionForInv(strSupCurrId, lblgvFregnBalance.Text);
                    strBalanceF = lblgvFregnBalance.Text;
                }

                debitTotalamount += debitamount;
                creditTotalamount += creditamount;
                txtClosingCredit.Text = strBalanceA;

                txtForeignCreditClosing.Text = strBalanceF;
                txtForeignCreditClosing.Text = objsys.GetCurencyConversionForInv(strSupCurrId, txtForeignCreditClosing.Text);

                txtCreditBalanceAmount.Text = strBalanceA;
                txtCreditBalanceAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtCreditBalanceAmount.Text);
                lblgvFregnAmount.Text = objsys.GetCurencyConversionForInv(strSupCurrId, lblgvFregnAmount.Text);
            }

            if (strSta == "Credit")
            {
                if (txtOpeningDebit.Text != "0" && txtOpeningDebit.Text != "")
                {
                    if (Convert.ToDouble(txtOpeningDebit.Text) != 0)
                    {
                        if (Convert.ToDouble(txtOpeningDebit.Text) > 0)
                        {
                            txtOpeningDebit.Text = txtOpeningDebit.Text;
                        }
                        else
                        {
                            txtOpeningDebit.Text = txtOpeningDebit.Text.Split('-')[1].ToString();
                        }
                    }
                }
                else
                {
                    txtOpeningDebit.Text = "0";
                }
                if (txtForeignCreditOpening.Text != "0" && txtForeignCreditOpening.Text != "")
                {
                    if (Convert.ToDouble(txtForeignCreditOpening.Text) != 0)
                    {
                        if (Convert.ToDouble(txtForeignCreditOpening.Text) > 0)
                        {
                            txtForeignCreditOpening.Text = txtForeignCreditOpening.Text;
                        }
                        else
                        {
                            txtForeignCreditOpening.Text = txtForeignCreditOpening.Text.Split('-')[1].ToString();
                        }
                    }
                }
                else
                {
                    txtForeignCreditOpening.Text = "0";
                }
            }
            else if (strSta == "Debit")
            {
                txtOpeningDebit.Text = "-" + txtOpeningDebit.Text;
                txtForeignCreditOpening.Text = "-" + txtForeignCreditOpening.Text;
            }

            if (GVSStatement.Rows.Count > 0)
            {
                Label lblgvDebitTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvDebitTotal");
                Label lblgvCreditTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvCreditTotal");
                Label lblgvBalanceTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvBalanceTotal");

                Label lblgvFreignBalanceTotal = (Label)GVSStatement.FooterRow.FindControl("lblgvFreignBalanceTotal");

                lblgvDebitTotal.Text = debitTotalamount.ToString();
                lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitTotal.Text);
                lblgvCreditTotal.Text = creditTotalamount.ToString();
                lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditTotal.Text);
                lblgvBalanceTotal.Text = strBalanceA;
                lblgvBalanceTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvBalanceTotal.Text);
                lblgvFreignBalanceTotal.Text = strBalanceF;
                lblgvFreignBalanceTotal.Text = objsys.GetCurencyConversionForInv(strSupCurrId, lblgvFreignBalanceTotal.Text);

                //string strSupCurrId = Session["SupplierCurrency"].ToString();
                GVSStatement.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(strSupCurrId, GVSStatement.HeaderRow.Cells[9].Text, Session["DBConnection"].ToString());
                GVSStatement.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVSStatement.HeaderRow.Cells[8].Text, Session["DBConnection"].ToString());
            }
            else
            {
                chkAgeingAnalysis.Visible = false;
                //DisplayMessage("You have no record available");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVehicleList);
            }
        }
        else
        {
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();

            if (strSta == "Credit")
            {
                if (txtOpeningDebit.Text != "0" && txtOpeningDebit.Text != "")
                {
                    if (Convert.ToDouble(txtOpeningDebit.Text) != 0)
                    {
                        if (Convert.ToDouble(txtOpeningDebit.Text) > 0)
                        {
                            txtOpeningDebit.Text = txtOpeningDebit.Text;
                        }
                        else
                        {
                            txtOpeningDebit.Text = txtOpeningDebit.Text.Split('-')[1].ToString();
                        }
                    }
                }
                else
                {
                    txtOpeningDebit.Text = "0";
                }
                if (txtForeignCreditOpening.Text != "0" && txtForeignCreditOpening.Text != "")
                {
                    if (Convert.ToDouble(txtForeignCreditOpening.Text) != 0)
                    {
                        if (Convert.ToDouble(txtForeignCreditOpening.Text) > 0)
                        {
                            txtForeignCreditOpening.Text = txtForeignCreditOpening.Text;
                        }
                        else
                        {
                            txtForeignCreditOpening.Text = txtForeignCreditOpening.Text.Split('-')[1].ToString();
                        }
                    }
                }
                else
                {
                    txtForeignCreditOpening.Text = "0";
                }
            }
            else if (strSta == "Debit")
            {
                txtOpeningDebit.Text = "-" + txtOpeningDebit.Text;
                txtForeignCreditOpening.Text = "-" + txtForeignCreditOpening.Text;
            }
            txtClosingCredit.Text = txtOpeningDebit.Text;
            txtForeignCreditClosing.Text = txtForeignCreditOpening.Text;

            //DisplayMessage("You have no record available");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVehicleList);
        }
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

            if (ddlVehicleList.SelectedIndex > 0)
            {
                //string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id from ac_ageing_detail group by Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and  other_account_no='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and AgeingType='PV'";
                //DataTable dtAgeing = da.return_DataTable(sql);

                DataTable dtAgeingDetail = ObjAgeingDetail.GetAgeingDetailAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (dtAgeingDetail.Rows.Count > 0)
                {
                    dtAgeingDetail = new DataView(dtAgeingDetail, "other_account_no='" + ddlVehicleList.SelectedValue + "' and AgeingType='PV'", "", DataViewRowState.CurrentRows).ToTable();
                }

                //For 0-30 Days
                try
                {
                    DataTable dt30Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "4", strCurrencyType);
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
                    DataTable dt60Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "5", strCurrencyType);
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
                    DataTable dt90Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "6", strCurrencyType);
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
                    DataTable dt180Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "7", strCurrencyType);
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
                    DataTable dt365Days = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "8", strCurrencyType);
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
                    DataTable dt365DaysAbove = ObjAgeingDetail.GetAgeingDetailforStatements(StrCompId, StrBrandId, strLocationId, ddlVehicleList.SelectedValue, "PV", "9", strCurrencyType);
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

            SetColorCode();
            double debitamount = 0;
            double creditamount = 0;
            foreach (GridViewRow gvr in GVSStatement.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvCreditAmount.Text);
                debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");
                Label lblgvFregnAmount = (Label)gvr.FindControl("lblgvForeignAmount");
                Label lblgvFregnBalance = (Label)gvr.FindControl("lblgvFVBalance");

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
    //protected void GVSStatement_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    string strCurrencyId = string.Empty;
    //    //AllPageCode();
    //    GridView gvStatement = (GridView)sender;
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        strCurrencyId = Session["SupplierCurrency"].ToString();
    //        gvStatement.Columns[9].HeaderText = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Foreign);
    //    }
    //}
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
        AllPageCode();
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
    public static string[] GetCompletionVehicleList(string prefixText, int count, string contextKey)
    {
        string strContactId = "0";
        string[] filterlist = null;

        Ems_ContactMaster objContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        Page page = HttpContext.Current.Handler as Page;
        
        //if (txtContractor.Text != "")
        //{
        //    string[] CustomerName = txtContractor.Text.Split('/');

        //    DataTable dtVehicle = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

        //    if (dtVehicle.Rows.Count > 0)
        //    {
        //        strContactId = CustomerName[1].ToString().Trim();

        //        dtVehicle = ObjDa.return_DataTable("select distinct Prj_VehicleMaster.Name,tp_Vehicle_Ledger.Vehicle_Id from tp_Vehicle_Ledger inner join Prj_VehicleMaster on tp_Vehicle_Ledger.Vehicle_Id = Prj_VehicleMaster.Vehicle_Id and tp_Vehicle_Ledger.Supplier_Id='" + strContactId + "' order by Prj_VehicleMaster.Name");

        //        filterlist = new string[dtVehicle.Rows.Count];
        //        if (dtVehicle.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtVehicle.Rows.Count; i++)
        //            {
        //                filterlist[i] = dtVehicle.Rows[i]["Name"].ToString() + "/" + dtVehicle.Rows[i]["Vehicle_Id"].ToString();
        //            }
        //        }
        //    }
        //}
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
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True'  and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            dtCustomer = ObjContactMaster.GetContactTrueAllData();
        }
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
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
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(),Session["CompId"].ToString());

        if (fyear_id == 0)
        {
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        //for Check Financial Year
        //if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text)))
        //{
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}

        //if (txtVehicleName.Text == "")
        //{
        //    DisplayMessage("Fill Supplier Name");
        //    txtFromDate.Focus();
        //    return;
        //}

        //txtVehicleName_TextChanged(sender, e);
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
        string strPaymentVoucherAcc = string.Empty;
        DateTime dtToDate = new DateTime();

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Vehicle Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

        DateTime dtFromdate = Convert.ToDateTime(txtFromDate.Text);
        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);

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
        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strPaymentVoucherAcc, ddlVehicleList.SelectedValue, txtFromDate.Text, txtToDate.Text, strCurrencyType);
        if (dtStatement.Rows.Count > 0)
        {
            if (ddlSVoucherType.SelectedValue != "--Select--")
            {
                dtStatement = new DataView(dtStatement, "Voucher_Type='" + ddlSVoucherType.SelectedValue + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();
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
        objArr.Add(ddlVehicleList.SelectedValue);
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strLocationId);
        objArr.Add(dtFromdate);
        objArr.Add(strPaymentVoucherAcc);
        objArr.Add(strAgeingReport);
        objArr.Add(txt0to30.Text);
        objArr.Add(txt31to60.Text);
        objArr.Add(txt61to90.Text);
        objArr.Add(txt91to180.Text);
        objArr.Add(txt181to365.Text);
        objArr.Add(txtAbove365.Text);
        objArr.Add(ddlSVoucherType.SelectedValue);
        objArr.Add(strCurrencyType);
        objArr.Add(strCurrencyId);

        Session["dtAcParam"] = objArr;
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
            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
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
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        //Reset();
    }
    #endregion

    protected void txtContractor_TextChanged(object sender, EventArgs e)
    {
        ddlVehicleList.Items.Clear();

        string strContactId = "0";

        if (txtContractor.Text != "")
        {
            string[] CustomerName = txtContractor.Text.Split('/');

            DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {

                strContactId = CustomerName[1].ToString().Trim();


                DtCustomer = da.return_DataTable("select distinct Prj_VehicleMaster.Name,tp_Vehicle_Ledger.Vehicle_Id from tp_Vehicle_Ledger inner join Prj_VehicleMaster on tp_Vehicle_Ledger.Vehicle_Id = Prj_VehicleMaster.Vehicle_Id and tp_Vehicle_Ledger.Supplier_Id='" + strContactId + "' order by Prj_VehicleMaster.Name");

                ddlVehicleList.DataSource = DtCustomer;
                ddlVehicleList.DataTextField = "Name";
                ddlVehicleList.DataValueField = "Vehicle_Id";
                ddlVehicleList.DataBind();

            }
            else
            {

                DisplayMessage("Enter Customer Name in suggestion Only");
                txtContractor.Text = "";
                txtContractor.Focus();

            }
        }

    }

}