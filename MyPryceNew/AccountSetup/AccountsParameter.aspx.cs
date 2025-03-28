using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class AccountSetup_AccountsParameter : System.Web.UI.Page
{
    Ac_ParameterMaster objAcParam = null;
    Ac_Parameter_Location objAcParamLocation = null;
    DataAccessClass daclass = null;
    SystemParameter objSys = null;
    LocationMaster ObjLocation = null;
    Ac_ChartOfAccount ObjCOA = null;
    Ac_CashFlow_Header objCashFlowHeader = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;
    Common cmn = null;
    IT_ObjectEntry objObjectEntry = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    EmployeeMaster ObjEmp = null;
    DepartmentMaster objDep = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objAcParam = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAcParamLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        daclass = new DataAccessClass(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objCashFlowHeader = new Ac_CashFlow_Header(Session["DBConnection"].ToString());
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "280", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillRecord();
            AllPageCode();
            fillDepartment();
        }
    }
    protected void fillDepartment()
    {
        DataTable dtDepartment = new DataTable();
        dtDepartment = objDep.GetDepartmentMaster();
        objPageCmn.FillData((object)ddlDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }

    protected void btnLocationLevel_Click(object sender, EventArgs e)
    {
        FillLocation();
        FillCOA();
        FillBank();
        FillLocationRecord();
    }
    public void AllPageCode()
    {
        Page.Title = objSys.GetSysTitle();

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("280", (DataTable)Session["ModuleName"]);
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

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            btnLocationSave.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "280",HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {
                    ////btnCSave.Visible = true;                
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnSave.Visible = true;
                            btnLocationSave.Visible = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }
    public void FillRecord()
    {
        if (objAcParam.GetParameterMasterAllTrue(Session["CompId"].ToString()) != null)
        {
            if (objAcParam.GetParameterMasterAllTrue(Session["CompId"].ToString()).Rows.Count > 0)
            {
                txtAcSalesInvoice.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Sales Invoice").Rows[0]["Param_Value"].ToString());
                txtAcPurchaseInvoice.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Purchase Invoice").Rows[0]["Param_Value"].ToString());
                txtAcHrSection.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "HR Section").Rows[0]["Param_Value"].ToString());
                txtSalesReturn.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Sales Return").Rows[0]["Param_Value"].ToString());
                try
                {
                    txtSOAdvanceCreditAC.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "SO Advance Credit Account").Rows[0]["Param_Value"].ToString());
                }
                catch
                {

                }

                try
                {
                    txtEmpAdvancePayment.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Advance Payment").Rows[0]["Param_Value"].ToString());
                }
                catch
                {

                }

                txtPurchaseReturn.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Purchase Return").Rows[0]["Param_Value"].ToString());
                txtAcPaymentVouchers.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers").Rows[0]["Param_Value"].ToString());
                txtAcReceiveVouchers.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers").Rows[0]["Param_Value"].ToString());

                txtAcCashTransaction.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Cash Transaction").Rows[0]["Param_Value"].ToString());
                //txtPOExpensesDebit.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "PO Expenses Debit Account").Rows[0]["Param_Value"].ToString());

                try
                {
                    txtPOAdvanceDebit.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "PO Advance Debit Account").Rows[0]["Param_Value"].ToString());
                }
                catch
                {

                }
                txtRoundOffAc.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "RoundOffAccount").Rows[0]["Param_Value"].ToString());
                //txtProfitandLoss.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Profit&Loss").Rows[0]["Param_Value"].ToString());
                //txtCapitalAccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "CapitalAccount").Rows[0]["Param_Value"].ToString());
                //txtAcdebitTransaction.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "debit Transaction").Rows[0]["Param_Value"].ToString());

                txtCostOfSales.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Cost Of Sales").Rows[0]["Param_Value"].ToString());
                //txtInventoryParameter.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Inventory Parameter").Rows[0]["Param_Value"].ToString());
                //txtAcLoss.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Loss").Rows[0]["Param_Value"].ToString());
                //txtAcProfit.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Profit").Rows[0]["Param_Value"].ToString());
                //txtAcGeneralaccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "General Account").Rows[0]["Param_Value"].ToString());
                //txtAcTradingaccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Trading Account").Rows[0]["Param_Value"].ToString());
                //txtAcCreditaccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Credit Account").Rows[0]["Param_Value"].ToString());
                txtEmployeeAccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
                txtVehicleAccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Vehicle Account").Rows[0]["Param_Value"].ToString());
                Txt_LeaveSalary_Account.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Leave Salary Account").Rows[0]["Param_Value"].ToString());
                txtEmpLoanAccount.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Loan Account").Rows[0]["Param_Value"].ToString());
                Txt_Indirect_Income.Text = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Indirect Income").Rows[0]["Param_Value"].ToString());
            }
            else
            {
                Txt_Indirect_Income.Text = "";
                txtEmpLoanAccount.Text = "";
                txtAcSalesInvoice.Text = "";
                txtAcPurchaseInvoice.Text = "";
                txtAcHrSection.Text = "";
                txtSalesReturn.Text = "";
                txtSOAdvanceCreditAC.Text = "";
                txtPurchaseReturn.Text = "";
                txtAcPaymentVouchers.Text = "";
                txtAcReceiveVouchers.Text = "";
                txtAcCashTransaction.Text = "";

                txtPOAdvanceDebit.Text = "";
                txtRoundOffAc.Text = "";
                //txtProfitandLoss.Text = "";
                //txtCapitalAccount.Text = "";
                //txtAcdebitTransaction.Text = "";
                txtCostOfSales.Text = "";
                //txtAcLoss.Text = "";
                //txtAcProfit.Text = "";
                //txtAcGeneralaccount.Text = "";
                //txtAcTradingaccount.Text = "";
                //txtAcCreditaccount.Text = "";
            }
        }
        else
        {
            txtAcSalesInvoice.Text = "";
            txtAcPurchaseInvoice.Text = "";
            txtAcHrSection.Text = "";
            txtSalesReturn.Text = "";
            txtSOAdvanceCreditAC.Text = "";
            txtPurchaseReturn.Text = "";
            txtAcPaymentVouchers.Text = "";
            txtAcReceiveVouchers.Text = "";
            txtAcCashTransaction.Text = "";
            txtEmpAdvancePayment.Text = "";

            txtPOAdvanceDebit.Text = "";
            txtRoundOffAc.Text = "";
            //txtProfitandLoss.Text = "";
            //txtCapitalAccount.Text = "";
            //txtAcdebitTransaction.Text = "";
            txtCostOfSales.Text = "";
            //txtAcLoss.Text = "";
            //txtAcProfit.Text = "";
            //txtAcGeneralaccount.Text = "";
            //txtAcTradingaccount.Text = "";
            //txtAcCreditaccount.Text = "";
            txtEmpLoanAccount.Text = "";
            Txt_Indirect_Income.Text = "";
        }
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = daclass.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {


        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            objAcParam.DeleteRecord(Session["CompId"].ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Employee Advance Payment", txtEmpAdvancePayment.Text.Split('/')[1].ToString(), "Advance Payment", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            objAcParam.InsertRecord(Session["CompId"].ToString(), "Sales Invoice", txtAcSalesInvoice.Text.Split('/')[1].ToString(), "Sales Invoice", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            if (txtAcPurchaseInvoice.Text.Trim() != "")
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Purchase Invoice", txtAcPurchaseInvoice.Text.Split('/')[1].ToString(), "Purchase Invoice", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            else
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Purchase Invoice", "0", "Purchase Invoice", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "HR Section", txtAcHrSection.Text.Split('/')[1].ToString(), "HR Section", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Sales Return", txtSalesReturn.Text.Split('/')[1].ToString(), "Sales Return", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            objAcParam.InsertRecord(Session["CompId"].ToString(), "SO Advance Credit Account", txtSOAdvanceCreditAC.Text.Split('/')[1].ToString(), "SO Advance Credit A/C", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            if (txtPurchaseReturn.Text.Trim() != "")
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Purchase Return", txtPurchaseReturn.Text.Split('/')[1].ToString(), "Purchase Return", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            else
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Purchase Return", "0", "Purchase Return", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Payment Vouchers", txtAcPaymentVouchers.Text.Split('/')[1].ToString(), "Suppliers Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Receive Vouchers", txtAcReceiveVouchers.Text.Split('/')[1].ToString(), "Customers Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Cash Transaction", txtAcCashTransaction.Text.Split('/')[1].ToString(), "Cash Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


            objAcParam.InsertRecord(Session["CompId"].ToString(), "RoundOffAccount", txtRoundOffAc.Text.Split('/')[1].ToString(), "Round Off Account for All Locations", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //int profitAndLossAc = 0;
            //int.TryParse(txtProfitandLoss.Text.Split('/')[1].ToString(), out profitAndLossAc);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Profit&Loss", profitAndLossAc.ToString(), "For Pofit and loss account entry According to financial year", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //int capitalAc = 0;
            //int.TryParse(txtCapitalAccount.Text.Split('/')[1].ToString(), out capitalAc);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "CapitalAccount", capitalAc.ToString(), "For Financial Year Closing Capital Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //objAcParam.InsertRecord(Session["CompId"].ToString(), "PO Expenses Debit Account", txtPOExpensesDebit.Text.Split('/')[1].ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAcParam.InsertRecord(Session["CompId"].ToString(), "PO Advance Debit Account", txtPOAdvanceDebit.Text.Split('/')[1].ToString(), "PO Advance Debit A/C", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //objAcParam.InsertRecord(Session["CompId"].ToString(), "debit Transaction", txtAcdebitTransaction.Text.Split('/')[1].ToString(), "Debit Transaction", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Inventory Parameter", txtInventoryParameter.Text.Split('/')[1].ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAcParam.InsertRecord(Session["CompId"].ToString(), "Cost Of Sales", txtCostOfSales.Text.Split('/')[1].ToString(), "Cost Of Sales", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Loss", txtAcLoss.Text.Split('/')[1].ToString(), "Loss", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Profit", txtAcProfit.Text.Split('/')[1].ToString(), "Profit", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "General Account", txtAcGeneralaccount.Text.Split('/')[1].ToString(), "General Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Trading Account", txtAcTradingaccount.Text.Split('/')[1].ToString(), "Trading Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //objAcParam.InsertRecord(Session["CompId"].ToString(), "Credit Account", txtAcCreditaccount.Text.Split('/')[1].ToString(), "Credit Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            if (!String.IsNullOrEmpty(txtEmployeeAccount.Text))
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Employee Account", txtEmployeeAccount.Text.Split('/')[1].ToString(), "Employee Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            if (!String.IsNullOrEmpty(txtVehicleAccount.Text))
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Vehicle Account", txtVehicleAccount.Text.Split('/')[1].ToString(), "Vehicle Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            if (!String.IsNullOrEmpty(txtEmpLoanAccount.Text))
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Employee Loan Account", txtEmpLoanAccount.Text.Split('/')[1].ToString(), "Employee Loan Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            if (!String.IsNullOrEmpty(Txt_LeaveSalary_Account.Text))
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Leave Salary Account", Txt_LeaveSalary_Account.Text.Split('/')[1].ToString(), "Leave Salary Account", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


            if (!String.IsNullOrEmpty(Txt_Indirect_Income.Text))
                objAcParam.InsertRecord(Session["CompId"].ToString(), "Indirect Income", Txt_Indirect_Income.Text.Split('/')[1].ToString(), "Indirect Income", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);




            DisplayMessage("Record Saved", "green");
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillRecord();
    }
    public void Reset()
    {
        txtAcSalesInvoice.Text = "";
        txtAcPurchaseInvoice.Text = "";
        txtAcHrSection.Text = "";
        txtSalesReturn.Text = "";
        txtSOAdvanceCreditAC.Text = "";
        txtPurchaseReturn.Text = "";
        txtAcPaymentVouchers.Text = "";
        txtAcReceiveVouchers.Text = "";
        txtAcCashTransaction.Text = "";

        txtRoundOffAc.Text = "";
        //txtProfitandLoss.Text = "";
        //txtCapitalAccount.Text = "";
        //txtPOExpensesDebit.Text = "";
        txtPOAdvanceDebit.Text = "";
        //txtAcdebitTransaction.Text = "";
        //txtAcCreditaccount.Text = "";
        //txtInventoryParameter.Text = "";
        txtCostOfSales.Text = "";
        // txtAcLoss.Text = "";
        // txtAcProfit.Text = "";
        //txtAcGeneralaccount.Text = "";
        //txtAcTradingaccount.Text = "";
        txtEmpAdvancePayment.Text = "";
        txtEmployeeAccount.Text = "";
        txtVehicleAccount.Text = "";
        txtEmpLoanAccount.Text = "";
        Txt_LeaveSalary_Account.Text = "";
        Txt_Indirect_Income.Text = "";

    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = daclass.return_DataTable(sql);

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
    }
    protected void txtEmployeeAccount_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = daclass.return_DataTable(sql);

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
    }
    protected void txtVehicleAccount_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = daclass.return_DataTable(sql);

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
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        try
        {
            dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


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
        //lstLocationSelect.Items.Clear();
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
        //lstLocation.Items.Clear();
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

    #region COAWork
    public void FillCOA()
    {
        lstCOA.Items.Clear();
        lstCOA.DataSource = null;
        lstCOA.DataBind();
        DataTable dtCOA = new DataTable();
        dtCOA = ObjCOA.GetCOAAllTrue(Session["CompId"].ToString());
        if (dtCOA.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstCOA, dtCOA, "AccountName", "Trans_Id");
        }
    }
    protected void btnPushCOA_Click(object sender, EventArgs e)
    {
        if (lstCOA.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstCOA.Items)
            {
                if (li.Selected)
                {
                    lstCOASelected.Items.Add(li);
                }
            }
            foreach (ListItem li in lstCOASelected.Items)
            {
                lstCOA.Items.Remove(li);
            }
            lstCOASelected.SelectedIndex = -1;
        }
        btnPushCOA.Focus();
    }
    protected void btnPullCOA_Click(object sender, EventArgs e)
    {
        if (lstCOASelected.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstCOASelected.Items)
            {
                if (li.Selected)
                {
                    lstCOA.Items.Add(li);
                }
            }
            foreach (ListItem li in lstCOA.Items)
            {
                if (li.Selected)
                {
                    lstCOASelected.Items.Remove(li);
                }
            }
            lstCOA.SelectedIndex = -1;
        }
        btnPullCOA.Focus();
    }
    protected void btnPushAllCOA_Click(object sender, EventArgs e)
    {
        //lstCOASelected.Items.Clear();
        foreach (ListItem li in lstCOA.Items)
        {
            lstCOASelected.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstCOASelected.Items)
        {
            lstCOA.Items.Remove(DeptItem);
        }
        btnPushAllCOA.Focus();
    }
    protected void btnPullAllCOA_Click(object sender, EventArgs e)
    {
        //lstCOA.Items.Clear();
        foreach (ListItem li in lstCOASelected.Items)
        {
            lstCOA.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstCOA.Items)
        {
            lstCOASelected.Items.Remove(DeptItem);
        }
        btnPullAllCOA.Focus();
    }
    #endregion

    #region BankAccountWork
    public void FillBank()
    {
        lstBankAccount.Items.Clear();
        lstBankAccount.DataSource = null;
        lstBankAccount.DataBind();
        DataTable dtBank = new DataTable();
        dtBank = ObjCOA.GetCOAAllTrue(Session["CompId"].ToString());
        if (dtBank.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstBankAccount, dtBank, "AccountName", "Trans_Id");
        }
    }
    protected void btnPushBank_Click(object sender, EventArgs e)
    {
        if (lstBankAccount.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstBankAccount.Items)
            {
                if (li.Selected)
                {
                    lstBankAccountSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstBankAccountSelect.Items)
            {
                lstBankAccount.Items.Remove(li);
            }
            lstBankAccountSelect.SelectedIndex = -1;
        }
        btnPushBank.Focus();
    }
    protected void btnPullBank_Click(object sender, EventArgs e)
    {
        if (lstBankAccountSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstBankAccountSelect.Items)
            {
                if (li.Selected)
                {
                    lstBankAccount.Items.Add(li);
                }
            }
            foreach (ListItem li in lstBankAccount.Items)
            {
                if (li.Selected)
                {
                    lstBankAccountSelect.Items.Remove(li);
                }
            }
            lstBankAccount.SelectedIndex = -1;
        }
        btnPullBank.Focus();
    }
    protected void btnPushAllBank_Click(object sender, EventArgs e)
    {
        //lstCOASelected.Items.Clear();
        foreach (ListItem li in lstBankAccount.Items)
        {
            lstBankAccountSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstBankAccountSelect.Items)
        {
            lstBankAccount.Items.Remove(DeptItem);
        }
        btnPushAllBank.Focus();
    }
    protected void btnPullAllBank_Click(object sender, EventArgs e)
    {
        //lstCOA.Items.Clear();
        foreach (ListItem li in lstBankAccountSelect.Items)
        {
            lstBankAccount.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstCOA.Items)
        {
            lstBankAccountSelect.Items.Remove(DeptItem);
        }
        btnPullAllBank.Focus();
    }
    #endregion

    #region LocationSave
    protected void btnLocationSave_Click(object sender, EventArgs e)
    {

        DataTable dtParam = new DataTable();

        string strSPVremindTo = "0";
        string strCRVRemindTo = "0";

        try
        {
            string Emp_Code = txtSupplierPaymentReminderEmp.Text.Split('/')[1].ToString();
            strSPVremindTo = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

        }
        catch
        { }

        try
        {
            string Emp_Code = txtSupplierPaymentReminderEmp.Text.Split('/')[1].ToString();
            strCRVRemindTo = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        }
        catch
        { }

        if (txtEmail.Text == string.Empty || IsValidEmail(txtEmail.Text) == false)
        {
            DisplayMessage("Email is invalid");
            txtEmail.Text = "";
            txtEmail.Focus();
            return;
        }
        string strpassword = string.Empty;
        if (txtPasswordEmail.Text.Trim() == "")
        {
            try
            {
                strpassword = Common.Decrypt(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Password"));
            }
            catch
            {
                DisplayMessage("Enter Password");
                txtPasswordEmail.Focus();
                return;
            }

        }
        else
        {
            strpassword = txtPasswordEmail.Text;
        }



        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            objAcParamLocation.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
            //Allow Data for Transfer In Finance
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow Transfer In Finance", chkTransferInFinance.Checked.ToString(), "Allow All Data in Transfer In Finance for Post in Finance", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //sales commission record
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Commission Debit", txtSCDebitAccountSales.Text.Split('/')[1].ToString(), "Debit Account Use For Sales Commission Module(Sales,Technical and Developer)", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Commission Credit", txtSCCreditAccountSales.Text.Split('/')[1].ToString(), "Credit Account Use For Sales Commission Module(Sales,Technical and Developer)", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Agent Commission Debit", txtSCDebitAccountAgent.Text.Split('/')[1].ToString(), "Debit Account Use For Sales Commission Module(For agent only)", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Agent Commission Credit", txtSCCreditAccountAgent.Text.Split('/')[1].ToString(), "Credit Account Use For Sales Commission Module(For agent only)", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //For Save Account
            for (int i = 0; i < lstCOASelected.Items.Count; i++)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount", lstCOASelected.Items[i].Value, "Cash Flow Accounts", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Save Location
            for (int i = 0; i < lstLocationSelect.Items.Count; i++)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowLocation", lstLocationSelect.Items[i].Value, "Cash Flow Locations", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Save BankAccount
            for (int i = 0; i < lstBankAccountSelect.Items.Count; i++)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", lstBankAccountSelect.Items[i].Value, "All Bank Accounts for Cheque", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Cash flow Parameter
            //For Weekoff
            if (chkWeekOff.Checked == true)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowWeekOff", true.ToString(), "If That Parameter is True then on week off Date will add in final Date", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowWeekOff", false.ToString(), "If That Parameter is True then on week off date will add in final date", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Holiday
            if (chkHoliday.Checked == true)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowHoliday", true.ToString(), "If That Parameter is True then on Holiday date will add in final date", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowHoliday", false.ToString(), "If That Parameter is True then on Holiday date will add in final date", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //For Payment Approval
            if (chkPaymentApproval.Checked == true)
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PaymentApproval", true.ToString(), "If That Parameter is True Payment Will Work According to Approval", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PaymentApproval", false.ToString(), "If That Parameter is False Payment Will Work According to Approval", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            //for Cash Flow Start Date & Opening Balance
            string strCashStatus = string.Empty;
            string strCashDate = string.Empty;
            string strCashOpening = string.Empty;
            DataTable dtCashflow = objCashFlowHeader.GetCashFlowAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (dtCashflow.Rows.Count > 1)
            {
                dtCashflow = new DataView(dtCashflow, "CF_OpeningAmount='0'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCashflow.Rows.Count > 0)
                {
                    strCashStatus = "True";
                    strCashDate = dtCashflow.Rows[0]["CF_Date"].ToString();
                    strCashOpening = dtCashflow.Rows[0]["CF_ClosingAmount"].ToString();
                }
            }
            else
            {
                strCashStatus = "False";
            }

            if (strCashStatus == "False")
            {
                PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                string strsql = "Delete FROM Ac_CashFlow_Header Where Company_Id ='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id= '" + Session["LocId"].ToString() + "'";
                objDA.execute_Command(strsql);

                string strDetailsql = "Delete FROM Ac_CashFlow_Detail Where Company_Id ='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id= '" + Session["LocId"].ToString() + "'";
                objDA.execute_Command(strDetailsql);

                try
                {
                    if (txtCashFlowStartDate.Text == "")
                    {
                        objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate", "", "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        DateTime dtDate = DateTime.Parse(txtCashFlowStartDate.Text);
                        string strClosingDate = dtDate.AddDays(-1).ToString("dd-MMM-yyyy");
                        objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate", "", "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), txtCashFlowStartDate.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        //Cash Flow Entries
                        int b = 0;
                        b = objCashFlowHeader.InsertCashFlowHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strClosingDate, "0.00", txtCashFlowOpeningBalance.Text, true.ToString(), txtCashFlowOpeningBalance.Text, "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        if (b != 0)
                        {
                            string strMaxId = string.Empty;
                            DataTable dtMaxId = objCashFlowHeader.GetCashFlowMaxId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                            if (dtMaxId.Rows.Count > 0)
                            {
                                strMaxId = dtMaxId.Rows[0][0].ToString();
                                objCashFlowDetail.InsertCashFlowDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, strClosingDate, "0.00", txtCashFlowOpeningBalance.Text, "Account", "0", "0", txtCashFlowOpeningBalance.Text, txtCashFlowOpeningBalance.Text, "Closing Balance for'" + strClosingDate + "'", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                }
                catch
                {
                    objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate", "", "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                if (txtCashFlowOpeningBalance.Text == "")
                {
                    objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowOpeningBalance", "0.00", "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowOpeningBalance", txtCashFlowOpeningBalance.Text, "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            else if (strCashStatus == "True")
            {
                //for Date
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate", "", "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), strCashDate, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //for Opening Balance
                objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowOpeningBalance", strCashOpening, "Cash flow Start date is Last Day Date with Closing Only Showing Opening", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ReconciledColorCode", txtReconciled.Text, "Reconciled Color for Grid Statements", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            ///auto ageing settlement

            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Auto_Ageing_Settlement", chkautoageingsettlement.Checked.ToString(), "Automatic ageing settlement on payment approval and journal voucher", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);



            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ConflictedColorCode", txtConflicted.Text, "Conflicted Color for Grid Statements", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "NotReconciledColorCode", txtNotReconciled.Text, "NotReconciled Color for Grid Statements", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Seperate voucher for Leave", chkLeaveSalary.Checked.ToString(), "For make seperate voucher of leave salary", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Seperate voucher for Allowance", chkallowances.Checked.ToString(), "For make seperate voucher of Allowancecs", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Seperate voucher for Deduction", chkDeduction.Checked.ToString(), "For make seperate voucher of Deduction", "", "", "", "", "", false.ToString(), "1/1/1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SPV Remind To", strSPVremindTo, "Supplier Payment Remind To", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CRV Remind To", strCRVRemindTo, "Customer Payment Remind To", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

            //insert email detail


            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email", txtEmail.Text, "Finance Email Address", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Password", Common.Encrypt(strpassword), "Finance Email Password", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_SMTP", txtSMTP.Text, "SMTP server", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Port_Out", txtSmtpPort.Text, "SMTP Port", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_EnableSSL", chkEnableSSL.Checked.ToString(), "Enable SSL", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Pop", txtPop3.Text, "Pop3 server", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Port_In", txtpopport.Text, "Pop3 server Port", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Display_Text", txtDisplayText.Text, "Email display text", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer", txtEstatementFooter.Content, txtEstatementFooter.Content, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            //save finance department
            objAcParamLocation.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Department_id", ddlDepartment.SelectedValue, "Finance Department", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


            DisplayMessage("Record Saved", "green");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            FillLocationRecord();
        }
        catch
        {
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
    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    protected void btnLocationReset_Click(object sender, EventArgs e)
    {
        LocationReset();
    }
    protected void btnLocationCancel_Click(object sender, EventArgs e)
    {
        FillLocationRecord();
    }
    public void LocationReset()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();

        lstCOA.Items.Clear();
        lstCOA.DataSource = null;
        lstCOA.DataBind();

        lstBankAccount.Items.Clear();
        lstBankAccount.DataSource = null;
        lstBankAccount.DataBind();
        chkTransferInFinance.Checked = false;
        chkWeekOff.Checked = false;
        chkHoliday.Checked = false;
        chkPaymentApproval.Checked = false;
        txtSCDebitAccountSales.Text = "";
        txtSCCreditAccountSales.Text = "";
        txtSCDebitAccountAgent.Text = "";
        txtSCCreditAccountAgent.Text = "";
        txtCashFlowStartDate.Text = "";
        txtCashFlowOpeningBalance.Text = "";
        txtReconciled.Text = "";
        txtConflicted.Text = "";
        txtNotReconciled.Text = "";

        txtSupplierPaymentReminderEmp.Text = "";
        txtCustomerPaymentReminderEmp.Text = "";
        txtEmail.Text = "";
        txtPasswordEmail.Text = "";
        txtSMTP.Text = "";
        txtSmtpPort.Text = "";
        txtPop3.Text = "";
        txtpopport.Text = "";
        txtDisplayText.Text = "";
        chkEnableSSL.Checked = false;
        txtEstatementFooter.Content = "";

    }
    public void FillLocationRecord()
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        //get auto ageing settle

        try
        {

            txtSCCreditAccountSales.Text = GetAccountNameByTransId(objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Sales Commission Credit").Rows[0]["Param_Value"].ToString());
            txtSCDebitAccountSales.Text = GetAccountNameByTransId(objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Sales Commission Debit").Rows[0]["Param_Value"].ToString());
            txtSCCreditAccountAgent.Text = GetAccountNameByTransId(objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Agent Commission Credit").Rows[0]["Param_Value"].ToString());
            txtSCDebitAccountAgent.Text = GetAccountNameByTransId(objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Agent Commission Debit").Rows[0]["Param_Value"].ToString());
        }
        catch
        {

        }



        chkautoageingsettlement.Checked = false;

        if (objAcParamLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Auto_Ageing_Settlement").Rows.Count > 0)
        {

            if (objAcParamLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Auto_Ageing_Settlement").Rows[0]["Param_Value"].ToString() == "True")
            {
                chkautoageingsettlement.Checked = true;
            }
        }

        try
        {
            chkLeaveSalary.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Leave").Rows[0]["Param_Value"].ToString());
            chkallowances.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Allowance").Rows[0]["Param_Value"].ToString());
            chkDeduction.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "Seperate voucher for Deduction").Rows[0]["Param_Value"].ToString());
        }
        catch
        {

        }


        try
        {


        }
        catch
        {

        }



        if (objAcParamLocation.GetParameterMasterAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) != null)
        {
            if (objAcParamLocation.GetParameterMasterAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows.Count > 0)
            {
                //for Fill Accounts
                FillCOA();
                lstCOASelected.Items.Clear();
                lstCOASelected.DataSource = null;
                lstCOASelected.DataBind();

                DataTable dtSelectedAccount = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount");
                for (int i = 0; i < dtSelectedAccount.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = dtSelectedAccount.Rows[i]["Param_Value"].ToString();
                    try
                    {
                        li.Text = ObjCOA.GetCOAByTransId(Session["CompId"].ToString(), dtSelectedAccount.Rows[i]["Param_Value"].ToString()).Rows[0]["AccountName"].ToString();
                    }
                    catch
                    {

                    }
                    lstCOASelected.Items.Add(li);
                    lstCOA.Items.Remove(li);
                }

                //for Fill Locations
                FillLocation();
                lstLocationSelect.Items.Clear();
                lstLocationSelect.DataSource = null;
                lstLocationSelect.DataBind();

                DataTable dtSelectedLocation = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowLocation");
                for (int i = 0; i < dtSelectedLocation.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = dtSelectedLocation.Rows[i]["Param_Value"].ToString();
                    try
                    {
                        li.Text = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), dtSelectedLocation.Rows[i]["Param_Value"].ToString()).Rows[0]["Location_Name"].ToString();
                    }
                    catch
                    {

                    }
                    lstLocationSelect.Items.Add(li);
                    lstLocation.Items.Remove(li);
                }

                //for Fill Bank Account
                FillBank();
                lstBankAccountSelect.Items.Clear();
                lstBankAccountSelect.DataSource = null;
                lstBankAccountSelect.DataBind();

                DataTable dtBankAccountLocation = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount");
                for (int i = 0; i < dtBankAccountLocation.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = dtBankAccountLocation.Rows[i]["Param_Value"].ToString();
                    try
                    {
                        li.Text = ObjCOA.GetCOAByTransId(Session["CompId"].ToString(), dtBankAccountLocation.Rows[i]["Param_Value"].ToString()).Rows[0]["AccountName"].ToString();
                    }
                    catch
                    {

                    }
                    lstBankAccountSelect.Items.Add(li);
                    lstBankAccount.Items.Remove(li);
                }

                //Fill Cash Flow Parameter                
                chkWeekOff.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowWeekOff"));
                chkHoliday.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowHoliday"));
                chkPaymentApproval.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PaymentApproval"));

                //fill cash flow date & balance
                txtCashFlowOpeningBalance.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowOpeningBalance");
                if (txtCashFlowOpeningBalance.Text != "")
                {
                    txtCashFlowOpeningBalance.Text = objSys.GetCurencyConversionForInv(strCurrency, txtCashFlowOpeningBalance.Text);
                }

                DataTable dtCashflowDate = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowStartDate");
                if (dtCashflowDate.Rows.Count > 0)
                {
                    txtCashFlowStartDate.Text = Convert.ToDateTime(dtCashflowDate.Rows[0]["Field7"].ToString()).ToString(objSys.SetDateFormat());
                }

                DataTable dtCashflow = objCashFlowHeader.GetCashFlowAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (dtCashflow.Rows.Count > 1)
                {
                    txtCashFlowOpeningBalance.Enabled = false;
                    txtCashFlowStartDate.Enabled = false;
                }
                else
                {
                    txtCashFlowOpeningBalance.Enabled = true;
                    txtCashFlowStartDate.Enabled = true;
                }

                //Fill Traansfer In Finance
                chkTransferInFinance.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow Transfer In Finance"));

                //FillColorCodes
                txtReconciled.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ReconciledColorCode");
                txtConflicted.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "ConflictedColorCode");
                txtNotReconciled.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "NotReconciledColorCode");
                try
                {
                    string strSPVRemindTo = "0";
                    strSPVRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SPV Remind To");
                    if (strSPVRemindTo != "0")
                    {
                        DataTable dt = ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), strSPVRemindTo);
                        txtSupplierPaymentReminderEmp.Text = dt.Rows[0]["emp_name"].ToString() + "/" + dt.Rows[0]["emp_code"].ToString();
                    }
                }
                catch { }
                try
                {
                    string strCRVRemindTo = "0";
                    strCRVRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CRV Remind To");
                    if (strCRVRemindTo != "0")
                    {
                        DataTable dt = ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), strCRVRemindTo);
                        txtCustomerPaymentReminderEmp.Text = dt.Rows[0]["emp_name"].ToString() + "/" + dt.Rows[0]["emp_code"].ToString();
                    }
                }
                catch { }
                try
                {
                    txtEmail.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email");
                    txtPasswordEmail.Text = Common.Decrypt(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Password"));
                    txtSMTP.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_SMTP");
                    txtSmtpPort.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Port_Out");
                    txtPop3.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Pop");
                    txtpopport.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Port_In");
                    txtDisplayText.Text = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Display_Text");
                    chkEnableSSL.Checked = Convert.ToBoolean(objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_EnableSSL"));
                    txtEstatementFooter.Content = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer");
                    ddlDepartment.SelectedValue = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Department_Id");
                }
                catch
                {

                }
            }
            else
            {
                FillCOA();
                FillLocation();
                FillBank();
                chkWeekOff.Checked = false;
                chkHoliday.Checked = false;
                chkPaymentApproval.Checked = false;
                txtCashFlowOpeningBalance.Text = "";
                txtCashFlowStartDate.Text = "";
                txtReconciled.Text = "";
                txtConflicted.Text = "";
                txtNotReconciled.Text = "";
                chkTransferInFinance.Checked = false;
                txtSupplierPaymentReminderEmp.Text = "";
                txtCustomerPaymentReminderEmp.Text = "";
                //email setup
                txtEmail.Text = "";
                txtPasswordEmail.Text = "";
                txtSMTP.Text = "";
                txtSmtpPort.Text = "";
                txtPop3.Text = "";
                txtpopport.Text = "";
                txtDisplayText.Text = "";
                chkEnableSSL.Checked = false;
                txtEstatementFooter.Content = "";
                ddlDepartment.SelectedIndex = 0;
            }
        }
        else
        {
            FillCOA();
            FillLocation();
            FillBank();
            chkWeekOff.Checked = false;
            chkHoliday.Checked = false;
            chkPaymentApproval.Checked = false;
            txtCashFlowOpeningBalance.Text = "";
            txtCashFlowStartDate.Text = "";
            txtReconciled.Text = "";
            txtConflicted.Text = "";
            txtNotReconciled.Text = "";
            txtSupplierPaymentReminderEmp.Text = "";
            chkTransferInFinance.Checked = false;
            txtCustomerPaymentReminderEmp.Text = "";
            //email setup control
            txtEmail.Text = "";
            txtPasswordEmail.Text = "";
            txtSMTP.Text = "";
            txtSmtpPort.Text = "";
            txtPop3.Text = "";
            txtpopport.Text = "";
            txtDisplayText.Text = "";
            chkEnableSSL.Checked = false;
            txtEstatementFooter.Content = "";
            ddlDepartment.SelectedIndex = 0;
        }
    }
    #endregion
    protected void txtSupplierPaymentReminderEmp_textChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (txtSupplierPaymentReminderEmp.Text != "")
        {
            try
            {

                string Emp_Code = txtSupplierPaymentReminderEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strEmpId = Emp_ID;

            }
            catch
            {
                strEmpId = "0";

            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtSupplierPaymentReminderEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierPaymentReminderEmp);
                return;

            }


        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and LOcation_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



        DataTable dtMain = new DataTable();
        dtMain = dt.Copy();


        string filtertext = "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')";
        DataTable dtCon = new DataView(dt, filtertext, "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString();
            }

        }
        return filterlist;

    }

    protected void txtCustomerPaymentReminderEmp_textChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (txtCustomerPaymentReminderEmp.Text != "")
        {
            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtCustomerPaymentReminderEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strEmpId = Emp_ID;

            }
            catch
            {
                strEmpId = "0";

            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtCustomerPaymentReminderEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerPaymentReminderEmp);
                return;

            }


        }
    }
}