using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public partial class VoucherEntries_ReceiveVouchers : System.Web.UI.Page
{
    Ems_ContactMaster ObjContactMaster = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    LocationMaster ObjLocation = null;
    SystemParameter objsys = null;
    Set_BankMaster objBank = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Finance_Year_Info objFYI = null;
    Set_Suppliers objSupplier = null;
    Ac_ParameterMaster objAccParameter = null;
    EmployeeMaster objEmployee = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    public const int grdDefaultColCount = 6;
    private const string strPageName = "ReceiveVouchers";

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../VoucherEntries/ReceiveVouchers.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillLocationList();
            FillCurrency();

            try
            {
                ddlLocalCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
                ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
                txtExchangeRate.Text = "1";
                hdnFExchangeRate.Value = txtExchangeRate.Text;
            }
            catch
            {

            }

            CalendarExtender_chequeCleardate.Format = objsys.SetDateFormat();
            CalendarExtender_txtchequeissuedate.Format = objsys.SetDateFormat();
            CalendarExtender_txtVoucherDate.Format = objsys.SetDateFormat();
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            //AllPageCode();
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "303", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            hdnLocId.Value = Session["LocId"].ToString();
            hdnCurrencyId.Value = Session["LocCurrencyId"].ToString();
            strLocationId = Session["LocId"].ToString();
            btnList_Click(sender, e);
            FillGrid();
            rbCashPayment.Checked = true;
            getPageControlsVisibility();
        }
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = ddlLocationList.SelectedValue;
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnVoucherSave.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }

    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/RV", "VoucherEntries", "ReceiveVoucher", e.CommandName.ToString(), e.CommandName.ToString());

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion

    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLocalCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            objPageCmn.FillData((object)ddlForeginCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlLocalCurrency.Items.Insert(0, "--Select--");
            ddlLocalCurrency.SelectedIndex = 0;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCostCenterName(string prefixText, int count, string contextKey)
    {
        DataAccessClass da = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = da.return_DataTable("Select * from Ac_CostCenter where IsActive='1'");
        DataTable dt = new DataView(dt1, "CenterName like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["CenterName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["CenterName"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
    protected void txtCostCenter_TextChanged(object sender, EventArgs e)
    {
        DataTable dtCOC = da.return_DataTable("Select Trans_Id,CenterName from Ac_CostCenter where  COA_Id='" + txtPaymentTo.Text.Split('/')[1].ToString() + "' And Trans_Id='" + txtCostCenter.Text.Split('/')[1].ToString() + "' ");
        if (dtCOC.Rows.Count > 0)
        {

        }
        else
        {
            txtCostCenter.Text = "";
            DisplayMessage("Please Select valid Cost Center");
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public string GetDateFormat(string Value)
    {
        string newdate = string.Empty;
        try
        {
            newdate = Convert.ToDateTime(Value).ToString(objsys.SetDateFormat());
        }
        catch
        {

        }
        return newdate;
    }
    protected void txtPaymentTo_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = da.return_DataTable(sql);

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

        if (txtPaymentTo.Text != "")
        {

            //for Customer & Supplier Account
            string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

            // for Employee and Vehicle Account
            string strEmployeeAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            string strVehicleAcc = Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            string strEmployeeLoanAcc = Ac_ParameterMaster.GetEmployeeLoanAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

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

            //string strEmployeeAcc = string.Empty;
            //DataTable dtEmployee = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account");
            //if (dtEmployee.Rows.Count > 0)
            //{
            //    strEmployeeAcc = dtEmployee.Rows[0]["Param_Value"].ToString();
            //}
            //else
            //{
            //    strEmployeeAcc = "0";
            //}

            //string strVehicleAcc = string.Empty;
            //DataTable dtVehicle = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Vehicle Account");
            //if (dtVehicle.Rows.Count > 0)
            //{
            //    strVehicleAcc = dtVehicle.Rows[0]["Param_Value"].ToString();
            //}
            //else
            //{
            //    strVehicleAcc = "0";
            //}
            DataTable dtCOC = da.return_DataTable("Select Trans_Id,CenterName from Ac_CostCenter where  COA_Id='" + txtPaymentTo.Text.Split('/')[1].ToString() + "'");
            if (dtCOC.Rows.Count > 0)
            {
                pnlCostCenter.Visible = true;
            }
            if (txtPaymentTo.Text.Split('/')[1].ToString() == strReceiveVoucherAcc)
            {
                trSupplier.Visible = false;
                trCustomer.Visible = true;
                trEmployee.Visible = false;
                trVehicle.Visible = false;
                txtCustomerName.Text = "";
                txtSupplierName.Text = "";
                txtEmployeeName.Text = "";
                txtVehicleName.Text = "";
                txtCustomerName.Focus();
            }
            else if (txtPaymentTo.Text.Split('/')[1].ToString() == strPaymentVoucherAcc)
            {
                trSupplier.Visible = true;
                trCustomer.Visible = false;
                trEmployee.Visible = false;
                trVehicle.Visible = false;
                txtCustomerName.Text = "";
                txtSupplierName.Text = "";
                txtEmployeeName.Text = "";
                txtVehicleName.Text = "";
                txtSupplierName.Focus();
            }
            else if (txtPaymentTo.Text.Split('/')[1].ToString() == strEmployeeAcc || txtPaymentTo.Text.Split('/')[1].ToString() == strEmployeeLoanAcc)
            {
                trSupplier.Visible = false;
                trCustomer.Visible = false;
                trEmployee.Visible = true;
                trVehicle.Visible = false;
                txtCustomerName.Text = "";
                txtSupplierName.Text = "";
                txtEmployeeName.Text = "";
                txtVehicleName.Text = "";
                txtEmployeeName.Focus();
            }
            else if (txtPaymentTo.Text.Split('/')[1].ToString() == strVehicleAcc)
            {
                trSupplier.Visible = false;
                trCustomer.Visible = false;
                trEmployee.Visible = false;
                trVehicle.Visible = true;
                txtCustomerName.Text = "";
                txtSupplierName.Text = "";
                txtEmployeeName.Text = "";
                txtVehicleName.Text = "";
                txtVehicleName.Focus();
            }
            else
            {
                trSupplier.Visible = false;
                trCustomer.Visible = false;
                trEmployee.Visible = false;
                trVehicle.Visible = false;
                txtCustomerName.Text = "";
                txtSupplierName.Text = "";
                txtEmployeeName.Text = "";
                txtVehicleName.Text = "";
                txtReceiveAmount.Focus();
            }
        }
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
        catch { }
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

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
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
        //            DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
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

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
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
        //            DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
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

            DataTable dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), txtEmployeeName.Text.Trim().Split('/')[1].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Employee Name");
                txtEmployeeName.Text = "";
                txtEmployeeName.Focus();
                return;
            }
            //else
            //{
            //    string strEmployeeId = txtEmployeeName.Text.Trim().Split('/')[1].ToString();
            //    if (strEmployeeId != "0" && strEmployeeId != "")
            //    {
            //        DataTable dtEmp = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strEmployeeId);
            //        if (dtEmp.Rows.Count > 0)
            //        {
            //            Session["EmployeeAccountId"] = dtEmp.Rows[0]["Account_No"].ToString();
            //        }
            //        else
            //        {
            //            DisplayMessage("First Set Employee Details in Master Setup");
            //            txtEmployeeName.Text = "";
            //            txtEmployeeName.Focus();
            //            return;
            //        }

            //        if (Session["EmployeeAccountId"].ToString() == "0" && Session["EmployeeAccountId"].ToString() == "")
            //        {
            //            DisplayMessage("First Set Employee Account in Master Setup");
            //            txtEmployeeName.Text = "";
            //            txtEmployeeName.Focus();
            //            return;
            //        }
            //    }
            //}
        }
        else
        {
            DisplayMessage("Select Employee Name");
            txtEmployeeName.Focus();
        }
    }
    protected void txtVehicleName_TextChanged(object sender, EventArgs e)
    {
        if (txtVehicleName.Text != "")
        {
            try
            {
                txtVehicleName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Vehicle Name");
                txtVehicleName.Text = "";
                txtVehicleName.Focus();
                return;
            }

            DataTable dt = objVehicleMaster.GetRecord_By_VehicleId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, txtVehicleName.Text.Trim().Split('/')[1].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Vehicle Name");
                txtVehicleName.Text = "";
                txtVehicleName.Focus();
                return;
            }
            //else
            //{
            //    string strCustomerId = txtVehicleName.Text.Trim().Split('/')[1].ToString();
            //    if (strCustomerId != "0" && strCustomerId != "")
            //    {
            //        DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
            //        if (dtCus.Rows.Count > 0)
            //        {
            //            Session["VehicleAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
            //        }
            //        else
            //        {
            //            DisplayMessage("First Set Vehicle Details in Transport Setup");
            //            txtVehicleName.Text = "";
            //            txtVehicleName.Focus();
            //            return;
            //        }

            //        if (Session["VehicleAccountId"].ToString() == "0" && Session["VehicleAccountId"].ToString() == "")
            //        {
            //            DisplayMessage("First Set Vehicle Account in System Setup");
            //            txtVehicleName.Text = "";
            //            txtVehicleName.Focus();
            //            return;
            //        }
            //    }
            //}
        }
        else
        {
            DisplayMessage("Select Vehicle Name");
            txtVehicleName.Focus();
        }
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
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
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
    public static string[] GetCompletionListReceiveAccountName(string prefixText, int count, string contextKey)
    {
        string sql = string.Empty;
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = new DataTable();
        sql = "select AccountName,Ac_ChartOfAccount.Trans_Id from Ac_ChartOfAccount left join ac_groups on Ac_ChartOfAccount.acc_group_id=ac_groups.trans_id where Ac_ChartOfAccount.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Ac_ChartOfAccount.IsActive='True' and Ac_ChartOfAccount.Field1='False' and (ac_groups.ac_groupName='Bank Account' or ac_groups.ac_groupName='Cash in hand')";
        dtCOA = daclass.return_DataTable(sql);
        if (dtCOA.Rows.Count == 0)
        {
            sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
            dtCOA = daclass.return_DataTable(sql);
        }

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
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Name"].ToString() + "/" + dt.Rows[i]["Bank_Id"].ToString();
        }
        return txt;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployee(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '%" + prefixText.ToString() + "%'", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
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
    public static string[] GetCompletionListVehicle(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = (dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString());
        }
        return txt;
    }
    #endregion

    protected void btnVoucherSave_Click(object sender, EventArgs e)
    {
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        strLocationId = hdnLocId.Value;
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        //Get Location by Cash Flow.
        string strLocationIdCash = "0";

        DataTable dtCashLocation = objAccParameterLocation.GetParameterValue_By_NameAndValue("CashFlowLocation", strLocationId);
        if (dtCashLocation.Rows.Count > 0)
        {
            for (int l = 0; l < dtCashLocation.Rows.Count; l++)
            {
                if (strLocationIdCash == "" || strLocationIdCash == "0")
                {
                    strLocationIdCash = dtCashLocation.Rows[l]["Location_Id"].ToString();
                }
                else
                {
                    strLocationIdCash = strLocationIdCash + "," + dtCashLocation.Rows[l]["Location_Id"].ToString();
                }
            }
        }

        //for Cash flow Posted
        //For Cash flow Account
        string strCashflowPostedV = string.Empty;
        string strAccountIdCash = string.Empty;
        DataTable dtAccountCash = objAccParameterLocation.GetParameterValue_By_ParameterNameforBrand(Session["CompId"].ToString(), Session["BrandId"].ToString(), "CashFlowAccount");
        dtAccountCash = new DataView(dtAccountCash, "Location_Id in (" + strLocationIdCash + ")", "", DataViewRowState.CurrentRows).ToTable();
        if (dtAccountCash.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccountCash.Rows.Count; i++)
            {
                if (strAccountIdCash == "")
                {
                    strAccountIdCash = dtAccountCash.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountIdCash = strAccountIdCash + "," + dtAccountCash.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountIdCash = "0";
        }

        string strCashFinalDate = string.Empty;
        string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True' and  IsActive='True'";
        DataTable dtCashflowDetail = objDA.return_DataTable(strsql);
        if (dtCashflowDetail.Rows.Count > 0)
        {
            strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
        }


        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            HiddenField hdngvAccountNo = (HiddenField)gvr.FindControl("hdngvAccountNo");
            if (strAccountIdCash.Split(',').Contains(hdngvAccountNo.Value))
            {
                if (strCashFinalDate != "")
                {
                    DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);
                    if (dtFinalDate >= DateTime.Parse(txtVoucherDate.Text))
                    {
                        if (strCashflowPostedV == "")
                        {
                            strCashflowPostedV = txtVoucherNo.Text;
                        }
                    }
                }
            }
        }

        if (strCashflowPostedV == "")
        {
            if (strAccountIdCash.Split(',').Contains(txtCashBank.Text.Split('/')[1].ToString()))
            {
                if (strCashFinalDate != "")
                {
                    DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);
                    if (dtFinalDate >= DateTime.Parse(txtVoucherDate.Text))
                    {
                        if (strCashflowPostedV == "")
                        {
                            strCashflowPostedV = txtVoucherNo.Text;
                        }
                    }
                }
            }
        }

        if (strCashflowPostedV != "")
        {
            DisplayMessage("Your Cashflow is Posted for That Voucher Number :- " + strCashflowPostedV + "");
            return;
        }

        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strChequeIssueDate = string.Empty;
        string strChequeClearDate = string.Empty;
        string strCashCheque = string.Empty;

        if (rbCashPayment.Checked == true)
        {
            strCashCheque = "Cash";
        }
        else if (rbChequePayment.Checked == true)
        {
            strCashCheque = "Cheque";
        }
        else
        {
            DisplayMessage("Choose Any Payment Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(rbCashPayment);
            return;
        }

        if (txtChequeIssueDate.Text == "")
        {
            strChequeIssueDate = "1/1/1800";
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtChequeIssueDate.Text);
                strChequeIssueDate = txtChequeIssueDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Issue Date in format " + objsys.SetDateFormat() + "");
                txtChequeIssueDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);
                return;
            }
        }

        if (txtChequeClearDate.Text == "")
        {
            strChequeClearDate = "1/1/1800";
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtChequeClearDate.Text);
                strChequeClearDate = txtChequeClearDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Clear Date in format " + objsys.SetDateFormat() + "");
                txtChequeClearDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);
                return;
            }
        }

        if (GvDetail.Rows.Count > 0)
        {

        }
        else
        {
            DisplayMessage("Atleast Add One Row In Detail Section");
            return;
        }

        if (txtCashBank.Text == "")
        {
            DisplayMessage("Select Cask/Bank Account");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCashBank);
            return;
        }

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

        // for Employee and Vehicle Account
        string strEmployeeAcc = string.Empty;
        DataTable dtEmployee = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account");
        if (dtEmployee.Rows.Count > 0)
        {
            strEmployeeAcc = dtEmployee.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strEmployeeAcc = "0";
        }

        string strVehicleAcc = string.Empty;
        DataTable dtVehicle = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Vehicle Account");
        if (dtVehicle.Rows.Count > 0)
        {
            strVehicleAcc = dtVehicle.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strVehicleAcc = "0";
        }

        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "BankAccount");
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
        else
        {
            strAccountId = "0";
        }

        string strReconcile = string.Empty;
        if (chkReconcile.Visible == true)
        {
            if (chkReconcile.Checked == true)
            {
                strReconcile = "True";
            }
            else if (chkReconcile.Checked == false)
            {
                strReconcile = "False";
            }
            else
            {
                strReconcile = "False";
            }
        }

        string strMaxVoucherId = string.Empty;


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {
                strMaxVoucherId = hdnVoucherId.Value;
                b = objVoucherHeader.UpdateVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), hdnRef_Id.Value, hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ddlLocalCurrency.SelectedValue.ToString(), "0", txtReference.Text, false.ToString(), false.ToString(), true.ToString(), "RV", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);

                //-------------------commented by neelkanth PUrohit 16-feb-2017 -----------------------------------------
                //objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);

                double SumDebitTotal = 0;
                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    //for Credit Entry
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((Label)gvr.FindControl("lblgvDebitAmount")).Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();


                    string CostID = "";
                    if (((Label)gvr.FindControl("lblgvCostCenter")).Text != "" && ((Label)gvr.FindControl("lblgvCostCenter")).Text != null)
                    {
                        CostID = ((Label)gvr.FindControl("lblgvCostCenter")).Text.Split('/')[1].ToString();
                    }

                    if (strAccountId.Split(',').Contains(((HiddenField)gvr.FindControl("hdngvAccountNo")).Value))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, ((Label)gvr.FindControl("lblSNo")).Text, ((HiddenField)gvr.FindControl("hdngvAccountNo")).Value, ((HiddenField)gvr.FindControl("hdngvOtherAccountNo")).Value, hdnRef_Id.Value, hdnRef_Type.Value, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0.00", ((Label)gvr.FindControl("lblgvDebitAmount")).Text, ((Label)gvr.FindControl("lblgvNarration")).Text, CostID, Session["EmpId"].ToString(), ((HiddenField)gvr.FindControl("hdngvCurrencyId")).Value, ((Label)gvr.FindControl("lblgvExchangeRate")).Text, ((Label)gvr.FindControl("lblgvForeignAmount")).Text, "0.00", CompanyCurrDebit, "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, ((Label)gvr.FindControl("lblSNo")).Text, ((HiddenField)gvr.FindControl("hdngvAccountNo")).Value, ((HiddenField)gvr.FindControl("hdngvOtherAccountNo")).Value, hdnRef_Id.Value, hdnRef_Type.Value, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0.00", ((Label)gvr.FindControl("lblgvDebitAmount")).Text, ((Label)gvr.FindControl("lblgvNarration")).Text, CostID, Session["EmpId"].ToString(), ((HiddenField)gvr.FindControl("hdngvCurrencyId")).Value, ((Label)gvr.FindControl("lblgvExchangeRate")).Text, ((Label)gvr.FindControl("lblgvForeignAmount")).Text, "0.00", CompanyCurrDebit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }


                    if (((Label)gvr.FindControl("lblgvDebitAmount")).Text == "")
                    {
                        ((Label)gvr.FindControl("lblgvDebitAmount")).Text = "0";
                    }
                    SumDebitTotal += Convert.ToDouble(((Label)gvr.FindControl("lblgvDebitAmount")).Text);



                }

                //For Debit Entry

                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), SumDebitTotal.ToString());
                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                if (strAccountId.Split(',').Contains(txtCashBank.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtCashBank.Text.Split('/')[1].ToString(), "0", "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, SumDebitTotal.ToString(), "0.00", txtReference.Text, "", Session["EmpId"].ToString(), ddlLocalCurrency.SelectedValue, "0.00", "0.00", CompanyCurrCredit, "0.00", "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnVoucherId.Value, "1", txtCashBank.Text.Split('/')[1].ToString(), "0", "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, SumDebitTotal.ToString(), "0.00", txtReference.Text, "", Session["EmpId"].ToString(), ddlLocalCurrency.SelectedValue, "0.00", "0.00", CompanyCurrCredit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                btnList_Click(null, null);
                DisplayMessage("Voucher Updated Successfully!");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString(), strLocationId, Session["DepartmentId"].ToString(), "0", "0", "0", DateTime.Now.ToString(), txtVoucherNo.Text, objsys.getDateForInput(txtVoucherDate.Text).ToString(), ddlVoucherType.SelectedValue, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0", ddlLocalCurrency.SelectedValue.ToString(), "0", txtReference.Text, false.ToString(), false.ToString(), true.ToString(), "RV", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                string strMaxId = string.Empty;
                DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                if (dtMaxId.Rows.Count > 0)
                {
                    strMaxId = dtMaxId.Rows[0][0].ToString();



                    strMaxVoucherId = strMaxId;
                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {
                        string sql = string.Empty;
                        if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11") //this is OPC & Jaipur Location
                        {
                            sql = "SELECT count(*) FROM Ac_Voucher_Header where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Voucher_Type='" + ddlVoucherType.SelectedValue + "' and Voucher_No Like '%" + txtVoucherNo.Text + "%'";
                            int strVoucherCount = Int32.Parse(objDA.get_SingleValue(sql, ref trns));

                            if (strVoucherCount == 0)
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + "1", ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + "1";
                            }
                            else
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + strVoucherCount, ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + strVoucherCount;
                            }
                        }
                        else
                        {
                            DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString(), ref trns);
                            if (dtCount.Rows.Count > 0)
                            {
                                dtCount = new DataView(dtCount, "Voucher_Type='" + ddlVoucherType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            if (dtCount.Rows.Count == 0)
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + "1", ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + "1";
                            }
                            else
                            {
                                objVoucherHeader.Updatecode(strMaxId, txtVoucherNo.Text + dtCount.Rows.Count, ref trns);
                                txtVoucherNo.Text = txtVoucherNo.Text + dtCount.Rows.Count;
                            }
                        }
                    }

                    //Add Detail/Ageing Detail Section.
                    objVoucherDetail.DeleteVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, ref trns);

                    double SumDebitTotal = 0;
                    foreach (GridViewRow gvr in GvDetail.Rows)
                    {
                        //for Credit Entry
                        string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), ((Label)gvr.FindControl("lblgvDebitAmount")).Text);
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        string CostID = "";
                        if (((Label)gvr.FindControl("lblgvCostCenter")).Text != "" && ((Label)gvr.FindControl("lblgvCostCenter")).Text != null)
                        {
                            CostID = ((Label)gvr.FindControl("lblgvCostCenter")).Text.Split('/')[1].ToString();
                        }
                        if (strAccountId.Split(',').Contains(((HiddenField)gvr.FindControl("hdngvAccountNo")).Value))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, ((Label)gvr.FindControl("lblSNo")).Text, ((HiddenField)gvr.FindControl("hdngvAccountNo")).Value, ((HiddenField)gvr.FindControl("hdngvOtherAccountNo")).Value, "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0.00", ((Label)gvr.FindControl("lblgvDebitAmount")).Text, ((Label)gvr.FindControl("lblgvNarration")).Text, CostID, Session["EmpId"].ToString(), ((HiddenField)gvr.FindControl("hdngvCurrencyId")).Value, ((Label)gvr.FindControl("lblgvExchangeRate")).Text, ((Label)gvr.FindControl("lblgvForeignAmount")).Text, "0.00", CompanyCurrDebit, "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, ((Label)gvr.FindControl("lblSNo")).Text, ((HiddenField)gvr.FindControl("hdngvAccountNo")).Value, ((HiddenField)gvr.FindControl("hdngvOtherAccountNo")).Value, "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, "0.00", ((Label)gvr.FindControl("lblgvDebitAmount")).Text, ((Label)gvr.FindControl("lblgvNarration")).Text, CostID, Session["EmpId"].ToString(), ((HiddenField)gvr.FindControl("hdngvCurrencyId")).Value, ((Label)gvr.FindControl("lblgvExchangeRate")).Text, ((Label)gvr.FindControl("lblgvForeignAmount")).Text, "0.00", CompanyCurrDebit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        if (((Label)gvr.FindControl("lblgvDebitAmount")).Text == "")
                        {
                            ((Label)gvr.FindControl("lblgvDebitAmount")).Text = "0";
                        }
                        SumDebitTotal += Convert.ToDouble(((Label)gvr.FindControl("lblgvDebitAmount")).Text);


                    }

                    //For Debit Entry
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), SumDebitTotal.ToString());
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtCashBank.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtCashBank.Text.Split('/')[1].ToString(), "0", "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, SumDebitTotal.ToString(), "0.00", txtReference.Text, "", Session["EmpId"].ToString(), ddlLocalCurrency.SelectedValue, "0.00", "0.00", CompanyCurrCredit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, "1", txtCashBank.Text.Split('/')[1].ToString(), "0", "0", "RV", strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, SumDebitTotal.ToString(), "0.00", txtReference.Text, "", Session["EmpId"].ToString(), ddlLocalCurrency.SelectedValue, "0.00", "0.00", CompanyCurrCredit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //End             
                }
                DisplayMessage("Voucher Saved Successfully!");
            }


            if (objAgeingDetail.checkAgeingConsistency_and_Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxVoucherId, ref trns, Session["UserId"].ToString(), Session["EmpId"].ToString()) == true)
            {

            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            FillGrid();
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));
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
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    public void Reset()
    {
        rbCashPayment.Checked = true;
        rbChequePayment.Checked = false;
        rbCashPayment_CheckedChanged(null, null);

        Lbl_Tab_New.Text = Resources.Attendance.New;

        txtChequeIssueDate.Text = "";
        txtChequeClearDate.Text = "";
        txtChequeNo.Text = "";
        txtReference.Text = "";
        txtDetailNarration.Text = "";
        //txtNarration.Text = "";
        txtCashBank.Text = "";

        FillCurrency();
        try
        {
            ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
            ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
            txtExchangeRate.Text = "1";
            hdnFExchangeRate.Value = txtExchangeRate.Text;
        }
        catch
        {

        }

        //txtExchangeRate.Text = "";
        txtForeignamount.Text = "";
        txtTotalReceivedAmount.Text = "";

        hdnRef_Id.Value = "0";
        hdnRef_Type.Value = "0";
        hdnInvoiceNumber.Value = "0";
        hdnInvoiceDate.Value = "0";

        btnVoucherSave.Visible = true;

        GvDetail.DataSource = null;
        GvDetail.DataBind();
        hdnVoucherId.Value = "0";

        txtCustomerName.Text = "";
        txtSupplierName.Text = "";
        txtEmployeeName.Text = "";
        txtVehicleName.Text = "";
        trSupplier.Visible = false;
        trCustomer.Visible = false;
        trEmployee.Visible = false;
        trVehicle.Visible = false;

        txtVoucherNo.Text = ViewState["DocNo"].ToString();
        //ViewState["DocNo"] = objDocNo.GetDocumentNo(true, "0", false, "160", "303", "0");

        txtReceiveAmount.Enabled = true;
        txtForeignamount.Enabled = true;

    }
    protected void btnVoucherCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    #region Detail Section
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = hdnCurrencyId.Value;

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        //try
        //{
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
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void txtReceiveAmount_TextChanged(object sender, EventArgs e)
    {
        updateControlsValue();
        //if (txtReceiveAmount.Text != "")
        //{
        //    double ReceiveAmount = Convert.ToDouble(txtReceiveAmount.Text);
        //    if (ReceiveAmount != 0)
        //    {
        //        ddlForeginCurrency.SelectedValue = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        //        ddlForeginCurrency_SelectedIndexChanged(sender, e);
        //        txtDetailNarration.Focus();
        //    }
        //}
    }
    protected void txtForeignamount_TextChanged(object sender, EventArgs e)
    {
        updateControlsValue(true);
    }
    protected void ddlForeginCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForeginCurrency.SelectedIndex > 0)
        {

            hdnFExchangeRate.Value = SystemParameter.GetExchageRate(ddlForeginCurrency.SelectedValue, ddlLocalCurrency.SelectedValue, Session["DBConnection"].ToString());
            //  string  strFireignExchange = hdnFExchangeRate.Value;

            txtExchangeRate.Text = hdnFExchangeRate.Value;

            //string strFireignExchange = GetCurrency(ddlForeginCurrency.SelectedValue,"1");
            //txtPaidForeignamount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
            // hdnFExchangeRate.Value = strFireignExchange.Trim().Split('/')[1].ToString();


            //line commented by jitendra on 06-03-2017

            if (ddlForeginCurrency.SelectedValue != ddlLocalCurrency.SelectedValue)
            {
                updateControlsValue(true);
                txtReceiveAmount.Enabled = false;
                txtForeignamount.Enabled = true;

            }
            else
            {
                //hdnFExchangeRate.Value = (1 / double.Parse(hdnFExchangeRate.Value)).ToString("0.000000");
                updateControlsValue();
                txtReceiveAmount.Enabled = true;
                txtForeignamount.Enabled = false;
            }
        }
    }
    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {

        if (ddlForeginCurrency.SelectedValue == ddlLocalCurrency.SelectedValue)
        {
            updateControlsValue();
        }
        else
        {
            updateControlsValue(true);
        }
        //double ReceiveAmount = Convert.ToDouble(txtReceiveAmount.Text);

        //if (ReceiveAmount != 0)
        //{
        //    if (txtExchangeRate.Text != "")
        //    {
        //        txtForeignamount.Text = objsys.GetCurencyConversionForInv(ddlForeginCurrency.SelectedValue, (float.Parse(txtExchangeRate.Text) * float.Parse(txtReceiveAmount.Text)).ToString());
        //        hdnFExchangeRate.Value = txtExchangeRate.Text;
        //    }
        //}
    }
    protected void btnDetailSave_Click(object sender, EventArgs e)
    {
        string Description = string.Empty;
        if (txtPaymentTo.Text != "")
        {
            string strAccountName = txtPaymentTo.Text.Trim().Split('/')[0].ToString();

            string strA = "0";
            foreach (GridViewRow gve in GvDetail.Rows)
            {
                Label lblgvAccountName = (Label)gve.FindControl("lblgvAccountName");
                if (strAccountName == lblgvAccountName.Text)
                {
                    strA = "1";
                }
            }

            if (trSupplier.Visible == true)
            {
                if (txtSupplierName.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Enter Supplier Name");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
                    return;
                }
            }
            else if (trCustomer.Visible == true)
            {
                if (txtCustomerName.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Enter Customer Name");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                    return;
                }
            }
            else if (trEmployee.Visible == true)
            {
                if (txtEmployeeName.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Enter Employee Name");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                    return;
                }
            }
            else if (trVehicle.Visible == true)
            {
                if (txtVehicleName.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Enter Vehicle Name");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVehicleName);
                    return;
                }
            }

            if (txtReceiveAmount.Text != "")
            {

            }
            else
            {
                txtReceiveAmount.Text = "0";
            }

            //check currency constrain neelkanth purohit - 29/08/2018
            try
            {
                if (txtPaymentTo.Text.Trim().Split('/')[1].ToString() == Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) || txtPaymentTo.Text.Trim().Split('/')[1].ToString() == Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()))
                {
                    string strOtherAccountId = txtSupplierName.Text != "" ? txtSupplierName.Text.Split('/')[1].ToString() : txtCustomerName.Text.Split('/')[1].ToString();
                    DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(strOtherAccountId);
                    if (_dtTemp.Rows[0]["Currency_Id"].ToString() != ddlForeginCurrency.SelectedValue)
                    {
                        DisplayMessage("Account Currency and voucher currency should same");
                        return;
                    }
                    _dtTemp.Dispose();
                }
            }
            catch { }
            //---------------------------end---------------------------------

            if (hdnAccountNo.Value == "")
            {
                FillProductChidGird("Save");
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentTo);
        }
        txtPaymentTo.Focus();

        double TotalDebit = 0;
        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            Label lblDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            double DebitAmt = Convert.ToDouble(lblDebitAmount.Text);

            if (DebitAmt != 0)
            {
                TotalDebit += DebitAmt;
            }
        }
        txtTotalReceivedAmount.Text = TotalDebit.ToString();
        string strCurrency = hdnCurrencyId.Value;
        txtTotalReceivedAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtTotalReceivedAmount.Text);

        txtReceiveAmount.Enabled = true;
        txtForeignamount.Enabled = true;
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountNo);
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
            DataTable dtAccName = ObjContactMaster.GetContactTrueById(strContactId);
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
    protected string GetOtherAccountNameForDetail(string strContactId, string Type)
    {
        string strCustomerName = string.Empty;
        if (strContactId != "0" && strContactId != "")
        {
            DataTable dtAccName = new DataTable();
            if (Type == "Supplier")
                dtAccName = ObjContactMaster.GetContactTrueById(strContactId);
            else if (Type == "Employee")
                dtAccName = da.return_DataTable("Select Emp_Name as Name from Set_EmployeeMaster where Emp_Id = " + strContactId + " ");
            else if (Type == "Vehicle")
                dtAccName = da.return_DataTable("Select Name from Prj_VehicleMaster where Vehicle_Id = " + strContactId + "");

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
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Account_No");
        dt.Columns.Add("Other_Account_No");
        dt.Columns.Add("Credit_Amount");
        dt.Columns.Add("Narration");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("Exchange_Rate");
        dt.Columns.Add("Foreign_Amount");
        dt.Columns.Add("Serial_No");
        dt.Columns.Add("Type");
        dt.Columns.Add("CostCenter");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;

        DataTable dt = CreateProductDataTable();
        if (GvDetail.Rows.Count > 0)
        {
            for (int i = 0; i < GvDetail.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvDetail.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
                    HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
                    HiddenField hdngvOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvOtherAccountNo");
                    Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
                    Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
                    HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
                    Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");
                    HiddenField hdngvType = (HiddenField)GvDetail.Rows[i].FindControl("hdngvType");
                    Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
                    dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
                    dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
                    if (hdngvOtherAccountNo.Value != "0")
                    {
                        dt.Rows[i]["Other_Account_No"] = hdngvOtherAccountNo.Value;
                        dt.Rows[i]["Type"] = hdngvType.Value;
                    }
                    else
                    {
                        dt.Rows[i]["Other_Account_No"] = "0";
                    }
                    dt.Rows[i]["Credit_Amount"] = lblgvDebitAmount.Text;
                    dt.Rows[i]["Narration"] = lblgvNarration.Text;
                    dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
                    dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
                    dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
                    dt.Rows[i]["CostCenter"] = lblgvCostCenter.Text;
                }
                else
                {
                    DataTable DtMaxSerial = new DataTable();
                    try
                    {
                        DtMaxSerial.Merge(dt);
                        DtMaxSerial = new DataView(DtMaxSerial, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }

                    dt.Rows.Add(i);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[i]["Trans_Id"] = (float.Parse(DtMaxSerial.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["Trans_Id"] = "1";
                    }

                    dt.Rows[i]["Serial_No"] = "1";
                    dt.Rows[i]["Account_No"] = txtPaymentTo.Text.Split('/')[1].ToString();
                    if (trCustomer.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtCustomerName.Text.Split('/')[1].ToString();
                        dt.Rows[i]["Type"] = "Customer";
                    }
                    else if (trSupplier.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtSupplierName.Text.Split('/')[1].ToString();
                        dt.Rows[i]["Type"] = "Supplier";
                    }
                    else if (trEmployee.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtEmployeeName.Text.Split('/')[1].ToString();
                        dt.Rows[i]["Type"] = "Employee";
                    }
                    else if (trVehicle.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtVehicleName.Text.Split('/')[1].ToString();
                        dt.Rows[i]["Type"] = "Vehicle";
                    }
                    else
                    {
                        dt.Rows[i]["Other_Account_No"] = "0";
                    }
                    dt.Rows[i]["Credit_Amount"] = txtReceiveAmount.Text;
                    if (txtReference.Text.Length == 0)
                    {
                        txtReference.Text = txtDetailNarration.Text;
                    }
                    dt.Rows[i]["Narration"] = txtDetailNarration.Text;
                    dt.Rows[i]["Currency_Id"] = ddlForeginCurrency.SelectedValue;
                    dt.Rows[i]["Foreign_Amount"] = txtForeignamount.Text;
                    dt.Rows[i]["Exchange_Rate"] = txtExchangeRate.Text;
                    dt.Rows[i]["CostCenter"] = txtCostCenter.Text;
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            dt.Rows[0]["Serial_No"] = "1";
            dt.Rows[0]["Account_No"] = txtPaymentTo.Text.Split('/')[1].ToString();
            if (trCustomer.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtCustomerName.Text.Split('/')[1].ToString();
                dt.Rows[0]["Type"] = "Customer";
            }
            else if (trSupplier.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtSupplierName.Text.Split('/')[1].ToString();
                dt.Rows[0]["Type"] = "Supplier";
            }
            else if (trEmployee.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtEmployeeName.Text.Split('/')[1].ToString();
                dt.Rows[0]["Type"] = "Employee";
            }
            else if (trVehicle.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtVehicleName.Text.Split('/')[1].ToString();
                dt.Rows[0]["Type"] = "Vehicle";
            }
            else
            {
                dt.Rows[0]["Other_Account_No"] = "0";
            }
            dt.Rows[0]["Credit_Amount"] = txtReceiveAmount.Text;
            if (txtReference.Text.Length == 0)
            {
                txtReference.Text = txtDetailNarration.Text;
            }
            dt.Rows[0]["Narration"] = txtDetailNarration.Text;
            dt.Rows[0]["Currency_Id"] = ddlForeginCurrency.SelectedValue;
            dt.Rows[0]["Foreign_Amount"] = txtForeignamount.Text;
            dt.Rows[0]["Exchange_Rate"] = txtExchangeRate.Text;
            dt.Rows[0]["CostCenter"] = txtCostCenter.Text;
        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDetail, dt, "", "");

            string strCurrency = hdnCurrencyId.Value;
            foreach (GridViewRow gvr in GvDetail.Rows)
            {
                Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");


                lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
                lblgvFrgnAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
                lblgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
            }
        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
            HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
            HiddenField hdngvOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvOtherAccountNo");
            Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
            Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
            HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
            Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");
            HiddenField hdngvType = (HiddenField)GvDetail.Rows[i].FindControl("hdngvType");
            Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
            dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;
            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
            if (hdngvOtherAccountNo.Value != "0")
            {
                dt.Rows[i]["Other_Account_No"] = hdngvOtherAccountNo.Value;
                dt.Rows[i]["Type"] = hdngvType.Value;
            }
            else
            {
                dt.Rows[i]["Other_Account_No"] = "0";
            }
            dt.Rows[i]["Credit_Amount"] = lblgvDebitAmount.Text;
            dt.Rows[i]["Narration"] = lblgvNarration.Text;
            dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
            dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
            dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
            dt.Rows[i]["CostCenter"] = lblgvCostCenter.Text;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAccountNo.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    protected void imgBtnDetailDelete_Command(object sender, CommandEventArgs e)
    {
        hdnAccountNo.Value = e.CommandArgument.ToString();
        FillProductChidGird("Del");

        double TotalDebit = 0;
        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            Label lblDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
            double DebitAmt = Convert.ToDouble(lblDebitAmount.Text);

            if (DebitAmt != 0)
            {
                TotalDebit += DebitAmt;
            }
        }
        txtTotalReceivedAmount.Text = TotalDebit.ToString();
        string strCurrency = hdnCurrencyId.Value;
        txtTotalReceivedAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtTotalReceivedAmount.Text);
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDetail, dt, "", "");
        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");


            lblgvDebitAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvFrgnAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
            lblgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
        }
        ResetDetail();
    }
    public void ResetDetail()
    {
        txtCostCenter.Text = "";
        pnlCostCenter.Visible = false;
        txtPaymentTo.Text = "";
        txtCustomerName.Text = "";
        txtSupplierName.Text = "";
        txtEmployeeName.Text = "";
        txtVehicleName.Text = "";
        trSupplier.Visible = false;
        trCustomer.Visible = false;
        trEmployee.Visible = false;
        trVehicle.Visible = false;
        txtReceiveAmount.Text = "";
        txtForeignamount.Text = "";
        FillCurrency();
        try
        {
            ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
            ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
            txtExchangeRate.Text = "1";
            hdnFExchangeRate.Value = txtExchangeRate.Text;
        }
        catch
        {

        }
        //txtExchangeRate.Text = "";
        hdnAccountNo.Value = "";
    }
    #endregion

    #region List View
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
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

            DataTable dtVoucher = (DataTable)Session["dtVoucher"];
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, view.ToTable(), "", "");

            string strCurrency = hdnCurrencyId.Value;
            foreach (GridViewRow gvr in GvVoucher.Rows)
            {
                Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
                lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
            }

            Session["dtFilter_Rec_Voucher"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
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
    protected void GvVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucher.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Rec_Voucher"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Rec_Voucher"];
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
        Session["dtFilter_Rec_Voucher"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string strbtnStatus = string.Empty;
        if (sender is ImageButton)
        {
            ImageButton btnId = (ImageButton)sender;

            if (btnId.ID == "btnEdit")
            {
                strbtnStatus = "Edit";
            }

            if (btnId.ID == "lnkViewDetail")
            {
                strbtnStatus = "View";
            }
        }


        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;

        hdnVoucherId.Value = e.CommandArgument.ToString();
        hdnLocId.Value = ddlLocationList.SelectedValue;
        strLocationId = hdnLocId.Value;
        hdnCurrencyId.Value = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
        DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited");
                return;
            }


            if (strbtnStatus == "Edit")
            {
                //Get Location by Cash Flow.
                string strLocationIdCash = string.Empty;
                DataTable dtCashLocation = objAccParameterLocation.GetParameterValue_By_NameAndValue("CashFlowLocation", strLocationId);
                if (dtCashLocation.Rows.Count > 0)
                {
                    for (int l = 0; l < dtCashLocation.Rows.Count; l++)
                    {
                        if (strLocationIdCash == "")
                        {
                            strLocationIdCash = dtCashLocation.Rows[l]["Location_Id"].ToString();
                        }
                        else
                        {
                            strLocationIdCash = strLocationIdCash + "," + dtCashLocation.Rows[l]["Location_Id"].ToString();
                        }
                    }
                }

                //for Cash flow Posted
                //For Cash flow Account
                string strVoucherDate = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(objsys.SetDateFormat());
                string strVoucherNo = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
                string strCashflowPostedV = string.Empty;
                string strAccountIdCash = string.Empty;
                DataTable dtAccountCash = objAccParameterLocation.GetParameterValue_By_ParameterNameforBrand(Session["CompId"].ToString(), Session["BrandId"].ToString(), "CashFlowAccount");
                dtAccountCash = new DataView(dtAccountCash, "Location_Id in (" + strLocationIdCash + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccountCash.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAccountCash.Rows.Count; i++)
                    {
                        if (strAccountIdCash == "")
                        {
                            strAccountIdCash = dtAccountCash.Rows[i]["Param_Value"].ToString();
                        }
                        else
                        {
                            strAccountIdCash = strAccountIdCash + "," + dtAccountCash.Rows[i]["Param_Value"].ToString();
                        }
                    }
                }
                else
                {
                    strAccountIdCash = "0";
                }

                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    string strAccountNo = dtDetail.Rows[i]["Account_No"].ToString();

                    if (strAccountIdCash.Split(',').Contains(strAccountNo))
                    {
                        string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True' and  IsActive='True'";
                        DataTable dtCashflowDetail = objDA.return_DataTable(strsql);
                        if (dtCashflowDetail.Rows.Count > 0)
                        {
                            string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                            if (strCashFinalDate != "")
                            {
                                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                                if (dtFinalDate >= DateTime.Parse(strVoucherDate))
                                {
                                    if (strCashflowPostedV == "")
                                    {
                                        strCashflowPostedV = strVoucherNo;
                                    }
                                }
                            }
                        }
                    }
                }

                if (strCashflowPostedV != "")
                {
                    DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPostedV + "");
                    return;
                }
            }

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
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

            txtReference.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();
            string strCurrencyId = dtVoucherEdit.Rows[0]["Currency_Id"].ToString();
            ddlLocalCurrency.SelectedValue = strCurrencyId;
            //txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            hdnRef_Id.Value = dtVoucherEdit.Rows[0]["Ref_Id"].ToString();
            hdnRef_Type.Value = dtVoucherEdit.Rows[0]["Ref_Type"].ToString();
            hdnInvoiceNumber.Value = dtVoucherEdit.Rows[0]["Inv_Number"].ToString();
            hdnInvoiceDate.Value = dtVoucherEdit.Rows[0]["Inv_Date"].ToString();

            //For Cash Bank Account
            DataTable dtCashBank = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            dtCashBank = new DataView(dtCashBank, "Debit_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCashBank.Rows.Count > 0)
            {
                txtCashBank.Text = GetAccountNameByTransId(dtCashBank.Rows[0]["Account_No"].ToString()) + "/" + dtCashBank.Rows[0]["Account_No"].ToString();
            }

            //for Reconciel Cheque
            //For Bank Account
            string strAccountId = string.Empty;
            DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), strLocationId, hdnLocId.Value, "BankAccount");
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
            else
            {
                strAccountId = "0";
            }

            DataTable dtReconcile = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
            if (dtReconcile.Rows.Count > 0)
            {
                for (int i = 0; i < dtReconcile.Rows.Count; i++)
                {
                    if (strAccountId.Split(',').Contains(dtReconcile.Rows[i]["Account_No"].ToString()))
                    {
                        string strStatus = dtReconcile.Rows[i]["Field2"].ToString();
                        if (strStatus == "False")
                        {
                            chkReconcile.Checked = false;
                            chkReconcile.Visible = true;
                        }
                        else if (strStatus == "True")
                        {
                            chkReconcile.Checked = true;
                            chkReconcile.Visible = true;
                        }
                        else
                        {
                            chkReconcile.Checked = false;
                            chkReconcile.Visible = true;
                        }
                    }
                    else
                    {
                        chkReconcile.Checked = false;
                        chkReconcile.Visible = true;
                    }
                }
            }
            else
            {
                chkReconcile.Checked = false;
                chkReconcile.Visible = true;
            }

            //Add Child Concept
            //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            string strCurrency = hdnCurrencyId.Value;
            dtDetail = new DataView(dtDetail, "Credit_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDetail.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvDetail, dtDetail, "", "");


                double TotalDebit = 0;
                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    Label lblDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");
                    double DebitAmt = Convert.ToDouble(lblDebitAmount.Text);

                    if (DebitAmt != 0)
                    {
                        TotalDebit += DebitAmt;
                    }

                    lblDebitAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDebitAmount.Text);
                    lblgvFrgnAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
                    lblgvExchangeRate.Text = objsys.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
                }
                txtTotalReceivedAmount.Text = TotalDebit.ToString();
                txtTotalReceivedAmount.Text = objsys.GetCurencyConversionForInv(strCurrency, txtTotalReceivedAmount.Text);
            }
        }
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);
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
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //create this validation by jitendra upadhyay on 04-02-2014 
        // here we set validation that the after post the record user can't delete the record
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is posted, So it cannot be Deleted !");
                return;
            }
            strLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
        }
        int b = 0;
        hdnVoucherId.Value = e.CommandArgument.ToString();
        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DataTable dtAgeDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtAgeDetail.Rows.Count > 0)
            {
                objAgeingDetail.DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
            }
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        ////FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
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
    private void FillGrid()
    {
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='False'";
        }
        strLocationId = ddlLocationList.SelectedValue;
        DataTable dtBrand = new DataView(objVoucherHeader.GetVoucherHeaderAllTrue(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString()), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";

        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            dtBrand = new DataView(dtBrand, "Voucher_Type='RV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
            Session["dtVoucher"] = dtBrand;
            Session["dtFilter_Rec_Voucher"] = dtBrand;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtBrand, "", "");
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
        }

        //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    public string GetFinanceCode(string strFinanceId)
    {
        string strFinanceCode = string.Empty;
        DataTable dtFI = objFYI.GetInfoByTransId(StrCompId, strFinanceId);
        if (dtFI.Rows.Count > 0)
        {
            strFinanceCode = dtFI.Rows[0]["Finance_Code"].ToString();
        }
        else
        {
            strFinanceCode = "0";
        }
        return strFinanceCode;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        Lbl_Tab_New.Text = Resources.Attendance.View;
        btnVoucherSave.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
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
    #endregion

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }
    protected void GvVoucherBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucherBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }

        string temp = string.Empty;

        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }

        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        dt = new DataView(dt, "Voucher_Type='RV'", "Voucher_Date DESC", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucherBin.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }

        Session["dtVoucherBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            // ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["dtVoucherBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, view.ToTable(), "", "");

            string strCurrency = hdnCurrencyId.Value;
            foreach (GridViewRow gvr in GvVoucherBin.Rows)
            {
                Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
                lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
            }

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        if (GvVoucherBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvVoucherBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        fleg = 1;
                    }
                    else
                    {
                        fleg = 0;
                    }
                }
                if (fleg == 0)
                {
                    DisplayMessage("Please Select Record");
                }
                else
                {
                    DisplayMessage("Record Not Activated");
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvVoucherBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvVoucherBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucherBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtVoucher = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtVoucher.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtVocher = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, dtVocher, "", "");
            string strCurrency = hdnCurrencyId.Value;
            foreach (GridViewRow gvr in GvVoucherBin.Rows)
            {
                Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
                lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
            }
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblSelectedRecord.Text.Split(',')[j].Trim() != "" && lblSelectedRecord.Text.Split(',')[j].Trim() != "0")
                    {
                        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderAllFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, Session["FinanceYearId"].ToString());
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + lblSelectedRecord.Text.Split(',')[j].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            string strVoucherDate = dtVoucherHeader.Rows[0]["Voucher_Date"].ToString();
                            if (strVoucherDate != "")
                            {
                                if (!Common.IsFinancialyearAllow(Convert.ToDateTime(strVoucherDate), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                                {
                                    DisplayMessage("Log In Financial year not allowing to perform this action");
                                    return;
                                }
                                else
                                {
                                    b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int flag = 0;
            foreach (GridViewRow Gvr in GvVoucherBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion
    public string GetVoucherAmount(string strVoucherId)
    {
        string strVoucherAmount = string.Empty;
        DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocId.Value, strVoucherId);
        if (dtVoucherD.Rows.Count > 0)
        {
            double debitamount = 0;
            for (int i = 0; i < dtVoucherD.Rows.Count; i++)
            {
                debitamount += Convert.ToDouble(dtVoucherD.Rows[i]["Debit_Amount"].ToString());
            }
            if (debitamount != 0)
            {
                strVoucherAmount = debitamount.ToString();
            }
        }
        else
        {
            strVoucherAmount = "0";
        }
        return strVoucherAmount;
    }
    protected void ImgDateSearch_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date.");
            txtFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + objsys.SetDateFormat() + "");
                txtFromDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                return;
            }
        }

        DateTime dtToDate = new DateTime();
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date.");
            txtToDate.Focus();
            return;
        }
        else
        {
            try
            {
                objsys.getDateForInput(txtToDate.Text);

                Convert.ToDateTime(txtToDate.Text);
                dtToDate = Convert.ToDateTime(txtToDate.Text);
                dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + objsys.SetDateFormat() + "");
                txtToDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                return;
            }
        }

        if (DateTime.Parse(txtFromDate.Text) >= dtToDate)
        {
            DisplayMessage("From Date should small..");
            txtFromDate.Focus();
            return;
        }

        DataTable dtDate = (DataTable)Session["dtFilter_Rec_Voucher"];
        if (dtDate.Rows.Count > 0)
        {
            dtDate = new DataView(dtDate, "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDate.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, dtDate, "", "");
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtDate.Rows.Count + "";
                Session["dtFilter_Rec_Voucher"] = dtDate;
                //AllPageCode();
            }
            else
            {
                GvVoucher.DataSource = null;
                GvVoucher.DataBind();
                DisplayMessage("You have no record according to searching criteria");
            }
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
            DisplayMessage("You have no record according to searching criteria");
        }

        string strCurrency = hdnCurrencyId.Value;
        foreach (GridViewRow gvr in GvVoucher.Rows)
        {
            Label lblVoucherAmt = (Label)gvr.FindControl("lblgvExchangerate");
            lblVoucherAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblVoucherAmt.Text);
        }
    }
    protected void updateControlsValue(bool isForeignToLocal = false)
    {
        if (ddlForeginCurrency.SelectedIndex == 0)
        {
            ddlForeginCurrency.SelectedValue = ddlLocalCurrency.SelectedValue;
            txtExchangeRate.Text = "1";
            hdnFExchangeRate.Value = txtExchangeRate.Text;
        }
        string strFireignExchange = string.Empty;

        //double debit = 0;
        //double credit = 0;
        double trns_amount = 0;
        double exchange_rate = 0;
        double foreign = 0;


        //double.TryParse(txtCreditAmount.Text, out credit);




        if (ddlForeginCurrency.SelectedValue == ddlLocalCurrency.SelectedValue)
        {
            txtExchangeRate.Text = "1";
            txtForeignamount.Enabled = false;
        }
        else
        {
            txtForeignamount.Enabled = true;
        }

        hdnFExchangeRate.Value = txtExchangeRate.Text;
        double.TryParse(txtExchangeRate.Text, out exchange_rate);
        double.TryParse(exchange_rate.ToString("0.000000"), out exchange_rate);

        if (isForeignToLocal == false)
        {
            //line commented by  jitendra on 06-03-2017
            // double.TryParse((1 / exchange_rate).ToString("0.000000"), out exchange_rate);
            hdnFExchangeRate.Value = exchange_rate.ToString();
            double.TryParse(txtReceiveAmount.Text, out trns_amount);
            //trns_amount = debit;
            txtForeignamount.Text = GetCurrency(ddlLocalCurrency.SelectedValue, (exchange_rate * trns_amount).ToString());
            txtForeignamount.Text = txtForeignamount.Text.Trim().Split('/')[0].ToString();
        }
        else
        {
            //line added by jitendra on 06-03-2017


            //code start

            double.TryParse((exchange_rate).ToString("0.000000"), out exchange_rate);

            //code end
            double.TryParse(txtForeignamount.Text, out foreign);
            txtReceiveAmount.Text = GetCurrency(ddlLocalCurrency.SelectedValue, (exchange_rate * foreign).ToString());
            txtReceiveAmount.Text = txtReceiveAmount.Text.Trim().Split('/')[0].ToString();
        }

        txtExchangeRate.Text = exchange_rate.ToString();
        hdnFExchangeRate.Value = exchange_rate.ToString();
    }

    #region Print

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strVoucherId = e.CommandArgument.ToString();

        ArrayList objArr = new ArrayList();
        objArr.Add(strVoucherId);
        objArr.Add("ReceiveVoucher");

        Session["dtAcVoucher"] = objArr;

        string strCmd = string.Format("window.open('../Accounts_Report/JournalVoucherReport.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
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
    #endregion
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvVoucher, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvVoucher, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        hdnLocId.Value = ddlLocationList.SelectedValue;
        hdnCurrencyId.Value = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), hdnLocId.Value).Rows[0]["Currency_id"].ToString();
        ddlLocalCurrency.SelectedValue = hdnCurrencyId.Value;
        ddlForeginCurrency.SelectedValue = hdnCurrencyId.Value;
        //txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0");
        ViewState["DocNo"] = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), hdnLocId.Value, false, "160", "303", "0", Session["BrandId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (hdnVoucherId.Value == "0")
        {
            txtVoucherNo.Text = ViewState["DocNo"].ToString();
        }
    }
    public void FillLocationList()
    {
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
            objPageCmn.FillData((object)ddlLocationList, dtLoc, "Location_Name", "Location_Id");
            ddlLocationList.Items.RemoveAt(0);
            ddlLocationList.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            ddlLocationList.Items.Insert(0, "--Select--");
            ddlLocationList.SelectedIndex = 0;
        }
    }
}