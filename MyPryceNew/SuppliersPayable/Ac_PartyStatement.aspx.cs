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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;

public partial class SuppliersPayable_Ac_PartyStatement : System.Web.UI.Page
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
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    int fyear_id = 0;
    Inv_ParameterMaster objInvParam = null;
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
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayable/Ac_PartyStatement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "367").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}
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
                DataTable dtSupName = objContact.GetContactTrueById(Request.QueryString["Id"].ToString());
                if (dtSupName.Rows.Count > 0)
                {
                    txtSupplierName.Text = dtSupName.Rows[0]["Name"].ToString() + "/" + dtSupName.Rows[0]["Trans_Id"].ToString();
                }

                txtFromDate.Text = Session["From"].ToString();
                txtToDate.Text = Session["To"].ToString();
                string strLocation = Session["SupLocId"].ToString();
                if (strLocation != "")
                {
                    for (int j = 0; j < strLocation.Split(',').Length; j++)
                    {
                        if (strLocation.Split(',')[j] != "")
                        {

                            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
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

            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Purchase Running Bill").Rows.Count > 0)
            {
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Purchase Running Bill").Rows[0]["ParameterValue"].ToString()))
                {
                    panelRunningBill.Visible = true;

                }
            }
        }
        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        SetColorCode();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
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
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        
    }
    protected void txtSupplierName_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                FillCurrency(otherAccountId.ToString());
                return;
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

        GVSStatement.DataSource = null;
        GVSStatement.DataBind();
        FillLocation();
        SetColorCode();
        FillCurrency("");
    }
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlCurrency.SelectedValue) || ddlCurrency.SelectedValue =="0")
        {
            DisplayMessage("Please select currency");
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

        //txtSupplierName_TextChanged(sender, e);
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
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
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

        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
            return;
        }

        string strCurrency = Session["LocCurrencyId"].ToString();
        string strExchangeRate = SystemParameter.GetExchageRate(strCurrency, ddlCurrency.SelectedValue, Session["DBConnection"].ToString());


        string strSta = string.Empty;
        //For Opening Balance Start
        string strCustomerAcNo = "0";
        string strSupplierAcNo = "0";
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

            double ForeignOpening = 0;

           


            //int otherAccountId = 0;
            //int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            //if (otherAccountId > 0)
            //{
                strCustomerAcNo = objAcAccountMaster.GetCustomerAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue).ToString();
                strSupplierAcNo = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue).ToString();
                //DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSupplierName.Text.Trim().Split('/')[0].ToString().ToUpper())
                //    {
                //        Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString());
                //        Session["SupplierCurrency"] = dt.Rows[0]["Currency_Id"].ToString();
                //        return;
                //    }

                //}
            //}


            PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());

            //OpeningBalance = Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString()) + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "','" + strCurrencyType + "', '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
            //OpeningBalance = OpeningBalance + Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString()) + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "','" + strCurrencyType + "', '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());

            //ForeignOpening = Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString()) + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "',3, '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
            //ForeignOpening = ForeignOpening + Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString()) + "','" + txtSupplierName.Text.Split('/')[1].ToString() + "',3, '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
            if (strSupplierAcNo != "0")
            {
                OpeningBalance = Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "','" + strSupplierAcNo + "','" + strCurrencyType + "', '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
                ForeignOpening = Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "','" + strSupplierAcNo + "',3, '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
            }
            if (strCustomerAcNo != "0")
            {
                OpeningBalance = OpeningBalance + Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "','" + strCustomerAcNo + "','" + strCurrencyType + "', '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
                ForeignOpening = ForeignOpening + Convert.ToDouble(objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "','" + strCustomerAcNo + "',3, '" + fyear_id + "')) OpeningBalance").Rows[0][0].ToString());
            }

            
            


            if (OpeningBalance < 0)
            {
                strSta = "Credit";
            }
            else if (OpeningBalance > 0)
            {
                strSta = "Debit";
            }


            if (OpeningBalance != 0)
            {
                string DOB = OpeningBalance.ToString();
                txtOpeningDebit.Text = objsys.GetCurencyConversionForInv(strCurrency, DOB);
                txtForeignCreditOpening.Text = objsys.GetCurencyConversionForInv(strCurrency, ForeignOpening.ToString());
            }
            else
            {
                string DOB = OpeningBalance.ToString();
                txtOpeningDebit.Text = objsys.GetCurencyConversionForInv(strCurrency, DOB);
                txtForeignCreditOpening.Text = objsys.GetCurencyConversionForInv(strCurrency, ForeignOpening.ToString());
            }
        }

        //DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", txtSupplierName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, strCurrencyType);
        DataTable dtStatement = new DataTable();
        if (strSupplierAcNo!="0")
        {
            dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", strSupplierAcNo, txtFromDate.Text, txtToDate.Text, strCurrencyType);
        }
        if (strCustomerAcNo != "0")
        {
            DataTable dtStatementCust = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", strCustomerAcNo, txtFromDate.Text, txtToDate.Text, strCurrencyType);
            if (dtStatementCust.Rows.Count > 0)
            {
                dtStatement.Merge(dtStatementCust);
                dtStatement = new DataView(dtStatement, "", "voucher_date asc", DataViewRowState.CurrentRows).ToTable();
            }
        }
        

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

                    }
                    else
                    {

                        GVSStatement.DataSource = null;
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
            string strStatus = "False";
            string strBalanceA = string.Empty;
            string strBalanceF = string.Empty;
            double debitamount = 0;
            double creditamount = 0;
            double debitTotalamount = 0;
            double creditTotalamount = 0;
            string strSupCurrId = ddlCurrency.SelectedValue;
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

                //DisplayMessage("You have no record available");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
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
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
        }

        //GetProjectedBalance(strLocationId);
    }

    public void GetProjectedBalance(string strLocationId)
    {

        if (strLocationId == "")
        {
            strLocationId = "0";
        }

        double OpeningBalance = 0;
        txtProjectedOpeningBalance.Text = "0";

        gvProjectedBalance.DataSource = null;
        gvProjectedBalance.DataBind();

        string strsql = string.Empty;

        double InvoiceBalance = 0;
        double TDsBalance = 0;
        double ItemLOssdeduction = 0;
        if (txtFromDate.Text != "")
        {
            InvoiceBalance = Convert.ToDouble(da.return_DataTable("select ISNULL( sum(grandtotal),0) from Inv_PurchaseInvoiceHeader where   Location_ID in (" + strLocationId + ") and SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Field5= 0 and Field4='Running' and IsActive='True'  and InvoiceDate<'" + Convert.ToDateTime(txtFromDate.Text) + "'").Rows[0][0].ToString());
            TDsBalance = Convert.ToDouble(da.return_DataTable("select ISNULL( sum( Deduction_Amount),0) from Inv_InvoiceDeductionInfo inner join Inv_PurchaseInvoiceHeader on Inv_InvoiceDeductionInfo.Ref_Id= Inv_PurchaseInvoiceHeader.transid and Inv_InvoiceDeductionInfo.Ref_Type='Supplier' and  Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Inv_PurchaseInvoiceHeader.Field5= 0 and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_InvoiceDeductionInfo.IsActive='True'  and InvoiceDate<'" + Convert.ToDateTime(txtFromDate.Text) + "'").Rows[0][0].ToString());
            try
            {
                ItemLOssdeduction = Convert.ToDouble(da.return_DataTable("select  sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)) as RemainQty from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where Trn_Date<'" + Convert.ToDateTime(txtFromDate.Text) + "' and  Set_EmployeeResources.Emp_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.Field5=1 and Set_EmployeeResources.Is_Returnable='True' and Set_EmployeeResources.IsActive='True'  having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0").Rows[0][0].ToString());
            }
            catch
            {

            }
          

        }
        else
        {
            InvoiceBalance = Convert.ToDouble(da.return_DataTable("select ISNULL( sum(grandtotal),0) from Inv_PurchaseInvoiceHeader where   Location_ID in (" + strLocationId + ") and SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Field5= 0 and Field4='Running' and IsActive='True'").Rows[0][0].ToString());
            TDsBalance = Convert.ToDouble(da.return_DataTable("select ISNULL( sum( Deduction_Amount),0) from Inv_InvoiceDeductionInfo inner join Inv_PurchaseInvoiceHeader on Inv_InvoiceDeductionInfo.Ref_Id= Inv_PurchaseInvoiceHeader.transid and Inv_InvoiceDeductionInfo.Ref_Type='Supplier' and  Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Inv_PurchaseInvoiceHeader.Field5= 0 and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_InvoiceDeductionInfo.IsActive='True'").Rows[0][0].ToString());
            try
            {
                ItemLOssdeduction = Convert.ToDouble(da.return_DataTable("select  sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)) as RemainQty from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where   Set_EmployeeResources.Emp_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.Field5=1 and Set_EmployeeResources.Is_Returnable='True' and Set_EmployeeResources.IsActive='True'  having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0").Rows[0][0].ToString());
            }
            catch
            {

            }
            
        }

        OpeningBalance = (InvoiceBalance - TDsBalance-ItemLOssdeduction);

        txtProjectedOpeningBalance.Text = Common.GetAmountDecimal((InvoiceBalance - TDsBalance).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        if (txtFromDate.Text != "")
        {
            strsql = "(select  Inv_PurchaseInvoiceHeader.TransID, Inv_PurchaseInvoiceHeader.InvoiceNo as Voucher_No, Inv_PurchaseInvoiceHeader.InvoiceDate as Voucher_Date,'PI' as Voucher_Type,'purchase Invoice Payment' as Narration,Inv_PurchaseInvoiceHeader.SupInvoiceNo as RefrenceNumber,0 as Debit_Amount,Inv_PurchaseInvoiceHeader.GrandTotal as Credit_Amount, Inv_PurchaseInvoiceHeader.CreatedBy,Inv_PurchaseInvoiceHeader.ModifiedBy from Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_PurchaseInvoiceHeader.IsActive='True' and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_PurchaseInvoiceHeader.Field5='0' and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Inv_PurchaseInvoiceHeader.InvoiceDate>='" + Convert.ToDateTime(txtFromDate.Text) + "' and Inv_PurchaseInvoiceHeader.InvoiceDate<='" + Convert.ToDateTime(txtToDate.Text) + "') union all (select Inv_PurchaseInvoiceHeader.TransID, Inv_PurchaseInvoiceHeader.InvoiceNo as Voucher_No, Inv_PurchaseInvoiceHeader.InvoiceDate as Voucher_Date,Inv_InvoiceDeductionInfo.Deduction_Type as Voucher_Type,Inv_InvoiceDeductionInfo.Deduction_Type+' deduction' as Narration,Inv_PurchaseInvoiceHeader.SupInvoiceNo as RefrenceNumber,Inv_InvoiceDeductionInfo.Deduction_Amount as Debit_Amount,0 as Credit_Amount, Inv_PurchaseInvoiceHeader.CreatedBy,Inv_PurchaseInvoiceHeader.ModifiedBy from Inv_InvoiceDeductionInfo inner join Inv_PurchaseInvoiceHeader on Inv_InvoiceDeductionInfo.Ref_Id = Inv_PurchaseInvoiceHeader.TransID where Inv_InvoiceDeductionInfo.ref_type='Supplier' and Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_InvoiceDeductionInfo.IsActive='True' and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_PurchaseInvoiceHeader.Field5='0' and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Inv_PurchaseInvoiceHeader.InvoiceDate>='" + Convert.ToDateTime(txtFromDate.Text) + "' and Inv_PurchaseInvoiceHeader.InvoiceDate<='" + Convert.ToDateTime(txtToDate.Text) + "') union all (select  0 as TransID,'1' as Voucher_No,MAX(Set_EmployeeResources.Trn_Date) as Voucher_Date,'LP' as Voucher_Type,'Debit Note for Issue tools' as Narration,0 as RefrenceNumber, sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)) as Debit_Amount,0 as Credit_Amount,'superadmin' as CreatedBy,'superadmin' as ModifiedBy from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where   Set_EmployeeResources.Emp_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.Field5=1 and Set_EmployeeResources.Is_Returnable='True' and Set_EmployeeResources.IsActive='True' and  Set_EmployeeResources.Trn_Date>='" + Convert.ToDateTime(txtFromDate.Text) + "' and Set_EmployeeResources.Trn_Date<='" + Convert.ToDateTime(txtToDate.Text) + "'  having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0)";
        }
        else
        {
            strsql = "(select Inv_PurchaseInvoiceHeader.TransID,Inv_PurchaseInvoiceHeader.InvoiceNo as Voucher_No, Inv_PurchaseInvoiceHeader.InvoiceDate as Voucher_Date,'PI' as Voucher_Type,'purchase Invoice Payment' as Narration,Inv_PurchaseInvoiceHeader.SupInvoiceNo as RefrenceNumber,0 as Debit_Amount,Inv_PurchaseInvoiceHeader.GrandTotal as Credit_Amount, Inv_PurchaseInvoiceHeader.CreatedBy,Inv_PurchaseInvoiceHeader.ModifiedBy from Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_PurchaseInvoiceHeader.IsActive='True' and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_PurchaseInvoiceHeader.Field5='0' and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "') union all (select Inv_PurchaseInvoiceHeader.TransID, Inv_PurchaseInvoiceHeader.InvoiceNo as Voucher_No, Inv_PurchaseInvoiceHeader.InvoiceDate as Voucher_Date,Inv_InvoiceDeductionInfo.Deduction_Type as Voucher_Type,Inv_InvoiceDeductionInfo.Deduction_Type+' deduction' as Narration,Inv_PurchaseInvoiceHeader.SupInvoiceNo as RefrenceNumber,Inv_InvoiceDeductionInfo.Deduction_Amount as Debit_Amount,0 as Credit_Amount, Inv_PurchaseInvoiceHeader.CreatedBy,Inv_PurchaseInvoiceHeader.ModifiedBy from Inv_InvoiceDeductionInfo inner join Inv_PurchaseInvoiceHeader on Inv_InvoiceDeductionInfo.Ref_Id = Inv_PurchaseInvoiceHeader.TransID where Inv_InvoiceDeductionInfo.ref_type='Supplier' and Inv_PurchaseInvoiceHeader.Location_ID in (" + strLocationId + ") and Inv_InvoiceDeductionInfo.IsActive='True' and Inv_PurchaseInvoiceHeader.Field4='Running' and Inv_PurchaseInvoiceHeader.Field5='0' and Inv_PurchaseInvoiceHeader.SupplierId='" + txtSupplierName.Text.Split('/')[1].ToString() + "') union all (select  0 as TransID,'1' as Voucher_No,MAX(Set_EmployeeResources.Trn_Date) as Voucher_Date,'LP' as Voucher_Type,'Debit Note for Issue tools' as Narration,0 as RefrenceNumber, sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then CAST( Set_EmployeeResources.Field3 AS numeric(18,3)) else 0 end)) as Debit_Amount,0 as Credit_Amount,'superadmin' as CreatedBy,'superadmin' as ModifiedBy from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where   Set_EmployeeResources.Emp_Id='" + txtSupplierName.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.Field5=1 and Set_EmployeeResources.Is_Returnable='True' and Set_EmployeeResources.IsActive='True'    having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0)";
        }

        DataTable dtprojectedStatement = new DataTable();

        dtprojectedStatement = da.return_DataTable(strsql);


        if (dtprojectedStatement.Rows.Count == 0)
        {
            return;
        }

        dtprojectedStatement = new DataView(dtprojectedStatement, "", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();

        gvProjectedBalance.DataSource = dtprojectedStatement;
        gvProjectedBalance.DataBind();




        foreach (GridViewRow gvrow in gvProjectedBalance.Rows)
        {

            OpeningBalance = OpeningBalance + Convert.ToDouble(((Label)gvrow.FindControl("lblgvCreditAmount")).Text) - Convert.ToDouble(((Label)gvrow.FindControl("lblgvDebitAmount")).Text);

            ((Label)gvrow.FindControl("lblgvBalance")).Text = Common.GetAmountDecimal(OpeningBalance.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }

        try
        {

            ((Label)gvProjectedBalance.FooterRow.FindControl("lblgvactualBalanceValue")).Text = Common.GetAmountDecimal((Convert.ToDouble(txtClosingCredit.Text) + OpeningBalance).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        }
        catch
        {

        }

        Session["dtProjected"] = dtprojectedStatement;
        Session["dtProjectedOpening"] = txtProjectedOpeningBalance.Text;


        string ClosingBalance = "Closing Balance (" + txtClosingCredit.Text + "+" + Common.GetAmountDecimal(OpeningBalance.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + ") = " + Common.GetAmountDecimal((Convert.ToDouble(txtClosingCredit.Text) + OpeningBalance).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        Session["dtProjectedClosing"] = ClosingBalance;

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
        string sql = "select distinct top 20 ems_contactmaster.trans_id,ems_contactmaster.name from ems_contactmaster inner join set_suppliers on set_suppliers.supplier_id=ems_contactmaster.trans_id inner join set_customermaster on set_customermaster.customer_id=ems_contactmaster.trans_id where ems_contactmaster.isactive='true' and name like '%" + prefixText + "%'";
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objDa.return_DataTable(sql);
        //Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster();
        //DataTable dtCon = objAcParamMaster.GetSupplierAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
    public void FillCurrency(string strContactId)
    {
        try
        {
            if (string.IsNullOrEmpty(strContactId) || strContactId=="0")
            {
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Insert(0, "--Select--");
                return;
            }

            string sql = "select distinct sys_currencymaster.currency_name,sys_currencymaster.currency_id from sys_currencymaster inner join ac_accountmaster on ac_accountmaster.currency_id=sys_currencymaster.currency_id where ac_accountmaster.ref_id='" + strContactId + "'";
            using (DataTable dt = da.return_DataTable(sql))
            {
                objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            }
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
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
            foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
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
            foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
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
        foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (System.Web.UI.WebControls.ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (System.Web.UI.WebControls.ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion

    #region Print

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(), Session["CompId"].ToString());

        if (fyear_id == 0)
        {
            GVSStatement.DataSource = null;
            GVSStatement.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


      

        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            txtFromDate.Focus();
            return;
        }

        //txtSupplierName_TextChanged(sender, e);
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
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
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
        DataTable dtStatement = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strPaymentVoucherAcc, txtSupplierName.Text.Split('/')[1].ToString(), txtFromDate.Text, txtToDate.Text, strCurrencyType);
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
                    //DisplayMessage("You Have No Record According to Criteria");
                    //txtFromDate.Focus();
                    //return;
                }
            }
            else
            {


            }
        }
        else
        {
            //DisplayMessage("You Have No Record According to Criteria");
            //txtFromDate.Focus();
            //return;
        }

        btnGetReport_Click(null, null);


        ArrayList objArr = new ArrayList();
        objArr.Add(txtSupplierName.Text.Split('/')[1].ToString());
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strLocationId);
        objArr.Add(dtFromdate);
        objArr.Add("0");
        objArr.Add("False");
        objArr.Add("0");
        objArr.Add("0");
        objArr.Add("0");
        objArr.Add("0");
        objArr.Add("0");
        objArr.Add("0");
        objArr.Add(ddlSVoucherType.SelectedValue);
        objArr.Add(strCurrencyType);
        objArr.Add(strCurrencyId);

        Session["dtAcParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/SupplierStatement.aspx','window','width=1024, ');");
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
    #endregion



    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                FillCurrency(otherAccountId.ToString());
                return;
            }
        }
        catch
        { }
        DisplayMessage("Supplier is not valid");
        txtSupplierName.Text = "";
        txtSupplierName.Focus();
    }
}