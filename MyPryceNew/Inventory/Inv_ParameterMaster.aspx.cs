using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class Inventory_Inv_ParameterMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Inv_ParameterMaster objParam = null;
    Ac_ChartOfAccount objCOA = null;
    EmployeeMaster objEmp = null;
    Inv_UserPermission objInvPermission = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass objDa = null;    
    string StrUserId = string.Empty;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objInvPermission = new Inv_UserPermission(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        AllPageCode();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "158", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            ddlOption.SelectedIndex = 2;
            FillGrid();
            ddlFieldName.SelectedIndex = 0;

            btnInventory_Click(null, null);
            setParameterValue();
        }
    }
    public void setParameterValue()
    {
        try
        {
            chkIsunitCostshow.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsUnitCost").Rows[0]["ParameterValue"].ToString());
        }
        catch
        {
            chkIsunitCostshow.Checked = false;
        }

        ddlPurchaseTax.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTax").Rows[0]["ParameterValue"].ToString();
        ddlPurchaseDiscount.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscount").Rows[0]["ParameterValue"].ToString();
        ddlSalesPrice.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Price").Rows[0]["ParameterValue"].ToString();
        ddlSalesTax.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString();
        ddlSalesDiscount.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString();
        ddlIsProductNameshow.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsProductNameShow").Rows[0]["ParameterValue"].ToString();
        txtSalesQuotationFooter.Content = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Quotation Footer").Rows[0]["Field1"].ToString();
        ddlPurchaseRequestApoproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseRequestApproval").Rows[0]["ParameterValue"].ToString();
        ddlPurchaseOrderApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseOrderApproval").Rows[0]["ParameterValue"].ToString();
        
        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow TAX Edit On Individual Transactions Purchase").Rows.Count > 0)
            DDL_Edit_Individual_Purchase.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow TAX Edit On Individual Transactions Purchase").Rows[0]["ParameterValue"].ToString();
        else
            DDL_Edit_Individual_Purchase.SelectedValue = "False";

        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow TAX Edit On Individual Transactions Sales").Rows.Count > 0)
            DDL_Edit_Individual_Sales.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow TAX Edit On Individual Transactions Sales").Rows[0]["ParameterValue"].ToString();
        else
            DDL_Edit_Individual_Sales.SelectedValue = "False";

        
        try
        {
            chkIsPurchaseRunningBill.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Purchase Running Bill").Rows[0]["ParameterValue"].ToString());
        }
        catch
        {

        }
        try
        {
            ddlItemReorder.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Item_Reorder_Functionality").Rows[0]["ParameterValue"].ToString();
        }
        catch
        {

        }


        try
        {
            ChkSalesRunningBill.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Sales Running Bill").Rows[0]["ParameterValue"].ToString());
        }
        catch
        {

        }




        chkSalesbelowcostprice.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Sales Below Cost Price").Rows[0]["ParameterValue"].ToString());
        ddlSalesQuotationApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationApproval").Rows[0]["ParameterValue"].ToString();
        ddlSalesOrderApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval").Rows[0]["ParameterValue"].ToString();
        ddlCashOrderApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashSalesOrderApproval").Rows[0]["ParameterValue"].ToString();

        ddlSalesInvoiceApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInvoiceApproval").Rows[0]["ParameterValue"].ToString();
        ddlCreditInvoiceApproval.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"].ToString();

        txtSalesQuotationheader.Content = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationHeader").Rows[0]["Field1"].ToString();
        txtSalesQuotationTermsandconditon.Content = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Quotation)").Rows[0]["Field1"].ToString();
        txtSalesInvoiceTermsandconditon.Content = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();

        ddlPriceshouldbe.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Price Should be").Rows[0]["ParameterValue"].ToString();

        ddlReportFormat.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString();
        ddlPurchaseReportheader.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Report Header").Rows[0]["ParameterValue"].ToString();

        ddlSalesReportHeader.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString();

        ddlPurchaeOrderduty.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Order(IsDuty)").Rows[0]["ParameterValue"].ToString();

        //for inventory parameter

        //serial related validation

        //code start
        chkisRequiredonPurchase.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Purchase Goods").Rows[0]["ParameterValue"].ToString());
        chkIsRequiredOnTransfer.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Transfer Voucher").Rows[0]["ParameterValue"].ToString());
        chkisRequiredonSalesreturn.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Sales Return").Rows[0]["ParameterValue"].ToString());

        //code end
        try
        {
            SalesQtaPrm.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Quotation Allow for New").Rows[0]["ParameterValue"].ToString());

        }
        catch(Exception ex)
        {

        }
        chkIsDelivery.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Delivery Voucher allow").Rows[0]["ParameterValue"].ToString());
        chkCashInvoiceAllow.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Allow Retail Cash Invoice").Rows[0]["ParameterValue"].ToString());


        ddlRptDV.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DeliveryVoucherReportHeaderLevel").Rows[0]["ParameterValue"].ToString();

        ddlIsBarcodeallow.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is barcode allow(report)").Rows[0]["ParameterValue"].ToString();


        string strAccountId = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Account Parameter").Rows[0]["ParameterValue"].ToString();
        DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, strAccountId);
        if (dtAccount.Rows.Count > 0)
        {
            txtPurchaseAccountNo.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + strAccountId;
        }
        else
        {
            txtPurchaseAccountNo.Text = "";
        }
        // Added By KSR on 11-09-2017
        // Set Expenses Account No
        try
        {
            string strExpensesAccountId = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
            DataTable dtExpensesAccount = objCOA.GetCOAByTransId(StrCompId, strExpensesAccountId);
            if (dtExpensesAccount.Rows.Count > 0)
            {
                txtExpensesAccountNo.Text = dtExpensesAccount.Rows[0]["AccountName"].ToString() + "/" + strExpensesAccountId;
            }
            else
            {
                txtExpensesAccountNo.Text = "";
            }
        }
        catch
        {
            txtExpensesAccountNo.Text = "";
        }

        strAccountId = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Account Parameter").Rows[0]["ParameterValue"].ToString();
        dtAccount = objCOA.GetCOAByTransId(StrCompId, strAccountId);
        if (dtAccount.Rows.Count > 0)
        {
            txtSalesAccountNumber.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + strAccountId;
        }
        else
        {
            txtSalesAccountNumber.Text = "";
        }

        ddlRptPR.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseRequestReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptPI.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInquiryReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptPO.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseOrderReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptPIn.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptGR.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "GoodsReceiveReportHeaderLevel").Rows[0]["ParameterValue"].ToString();

        ddlRptSI.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInquiryReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptSQ.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptSO.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        ddlRptSIn.SelectedValue = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString();
        txtCommisisonpaymentallow.Text = objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Commission Payment Allow(In Month)").Rows[0]["ParameterValue"].ToString();


        //IsInvoiceScanning
        try
        {
            chkscanningsolution.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning").Rows[0]["ParameterValue"].ToString());

        }
        catch
        {
            chkscanningsolution.Checked = false;
        }


        try
        {
            chkTransferOutScanning.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferOutScanning").Rows[0]["ParameterValue"].ToString());

        }
        catch
        {
            chkTransferOutScanning.Checked = false;
        }

        try
        {
            chkTransferInScanning.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferInScanning").Rows[0]["ParameterValue"].ToString());

        }
        catch
        {
            chkTransferInScanning.Checked = false;
        }


        try
        {
            chkDeliveryScanningSolution.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning").Rows[0]["ParameterValue"].ToString());

        }
        catch
        {
            chkDeliveryScanningSolution.Checked = false;
        }
        // For Costing Entry on Sales Invoice
        try
        {
            chkCostingEntry.Checked = Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsCostingEntry").Rows[0]["ParameterValue"].ToString());

        }
        catch
        {
            chkCostingEntry.Checked = false;
        }

    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("158", (DataTable)Session["ModuleName"]);
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

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        if (Session["EmpId"].ToString() == "0")
        {
            btnCSave.Visible = true;
            foreach (GridViewRow Row in GvParameter.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
            }
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "158", HttpContext.Current.Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "2")
            {
                btnCSave.Visible = true;
            }
            foreach (GridViewRow Row in GvParameter.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "2")
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                }

            }
        }

    }
    #region User Defined Funcation

    private void FillGrid()
    {
        DataTable dtBrand = objInvPermission.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtParameter"] = dtBrand;
        ViewState["dtFilter"] = dtBrand;

        objPageCmn.FillData((object)GvParameter, dtBrand, "", "");
    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
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
    public void Reset()
    {
        txtEmp.Text = "";
        chkIsShowSupplier.Checked = false;
        chkinvCostPrice.Checked = false;
        editid.Value = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtEmp.Enabled = true;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    #endregion


    #region System Defined Funcation



    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {

        if (txtEmp.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmp.Focus();
            return;
        }

        string empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];


        empid = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid).Rows[0]["Emp_Id"].ToString();

        int b = 0;

        if (editid.Value == "")
        {
            if (objInvPermission.GetRecord_By_EmpId(StrCompId, StrBrandId, strLocationId, empid).Rows.Count == 0)
            {

                b = objInvPermission.InsertRecord(StrCompId, StrBrandId, strLocationId, empid, chkIsShowSupplier.Checked.ToString(), chkinvCostPrice.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                DisplayMessage("Employee Record Exists");
                txtEmp.Focus();
                return;
            }


            if (b != 0)
            {
                DisplayMessage("Record Saved","green");

            }
            else
            {
                DisplayMessage("Record  Saved");
            }
        }
        else
        {
            objInvPermission.DeleteRecord(StrCompId, StrBrandId, strLocationId, editid.Value);

            b = objInvPermission.InsertRecord(StrCompId, StrBrandId, strLocationId, empid, chkIsShowSupplier.Checked.ToString(), chkinvCostPrice.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (b != 0)
            {

                DisplayMessage("Record Saved","green");

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }

        Reset();
        FillGrid();
        AllPageCode();

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtParameter = objInvPermission.GetRecord_By_EmpId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), editid.Value);

        if (dtParameter.Rows.Count > 0)
        {
            txtEmp.Text = "" + dtParameter.Rows[0]["Emp_Name"].ToString() + "/(" + dtParameter.Rows[0]["Designation"].ToString() + ")/" + dtParameter.Rows[0]["Emp_Code"].ToString() + "";
            chkIsShowSupplier.Checked = Convert.ToBoolean(dtParameter.Rows[0]["IsShowSupplier"].ToString());
            chkinvCostPrice.Checked = Convert.ToBoolean(dtParameter.Rows[0]["IsShowCostPrice"].ToString());
            txtEmp.Enabled = false;
            txtEmp.Focus();
        }
    }
    protected void GvParameter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvParameter.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");

        AllPageCode();


    }


    protected void btnbindrpt_Click(object sender, ImageClickEventArgs e)
    {

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }
            DataTable dtCurrency = (DataTable)Session["dtParameter"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvParameter, view.ToTable(), "", "");

            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";


            AllPageCode();


        }
        txtValue.Focus();
    }
    protected void GvParameter_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtFilter"];
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
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");

        AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }
    #endregion
    #region Inventory
    protected void btnInventory_Click(object sender, EventArgs e)
    {
        //PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //PnlPurchase.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlSales.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlUserPermission.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //User_Level.Attributes.Add("class", "tab-pane");
        //pnlInventoryDetail.Attributes.Add("class", "tab-pane active");
        //pnlPurchaseDetail.Attributes.Add("class", "tab-pane");
        //PnlSalesDetail.Attributes.Add("class", "tab-pane");

        //Li_Purchase_Details.Attributes.Add("class", "tab-pane");
        //Li_Sales_Details.Attributes.Add("class", "tab-pane");
        //Li_User_Details.Attributes.Add("class", "tab-pane");
        //Li_Inventory_Details.Attributes.Add("class", "tab-pane active");
    }

    #endregion
    #region Purchase
    protected void BtnPurchase_Click(object sender, EventArgs e)
    {
        //PnlPurchase.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlSales.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlUserPermission.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //User_Level.Attributes.Add("class", "tab-pane");
        //pnlInventoryDetail.Attributes.Add("class", "tab-pane");
        //pnlPurchaseDetail.Attributes.Add("class", "tab-pane active");
        //PnlSalesDetail.Attributes.Add("class", "tab-pane");

        //Li_Purchase_Details.Attributes.Add("class", "tab-pane active");
        //Li_Sales_Details.Attributes.Add("class", "tab-pane");
        //Li_User_Details.Attributes.Add("class", "tab-pane");
        //Li_Inventory_Details.Attributes.Add("class", "tab-pane");

    }
    protected void btnSavePurchase_Click(object sender, EventArgs e)
    {
        DataTable Dt_Parameter = objSys.GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "" && ddlPurchaseTax.SelectedValue == "True")
            {
                DisplayMessage("Please Update Tax System in System Parameter befor IsTax Update");
                return;
            }
            else
            {
                objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsTax", ddlPurchaseTax.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
            

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsUnitCost", chkIsunitCostshow.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsDiscount", ddlPurchaseDiscount.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Purchase Account Parameter", GetAccountIdForPurchase(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Expenses Account Parameter", GetAccountIdForExpenses(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseRequestApproval", ddlPurchaseRequestApoproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseOrderApproval", ddlPurchaseOrderApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Purchase Report Header", ddlPurchaseReportheader.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Purchase Order(IsDuty)", ddlPurchaeOrderduty.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseRequestReportHeaderLevel", ddlRptPR.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseInquiryReportHeaderLevel", ddlRptPI.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseOrderReportHeaderLevel", ddlRptPO.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "PurchaseInvoiceReportHeaderLevel", ddlRptPIn.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "GoodsReceiveReportHeaderLevel", ddlRptGR.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());


        if (objParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Is Purchase Running Bill").Rows.Count == 0)
        {
            objParam.InsertParameterMaster(StrCompId, StrBrandId, strLocationId, "Is Purchase Running Bill", chkIsPurchaseRunningBill.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Purchase Running Bill", chkIsPurchaseRunningBill.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        // Added By KSR on 11-09-2017
        if (objParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Expenses Account Parameter").Rows.Count == 0)
        {
            objParam.InsertParameterMaster(StrCompId, StrBrandId, strLocationId, "Expenses Account Parameter", GetAccountIdForExpenses(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Expenses Account Parameter", GetAccountIdForExpenses(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        // Added By ghanshyam Suthar on 21-03-2018
        if (objParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Allow TAX Edit On Individual Transactions Purchase").Rows.Count == 0)
        {
            objParam.InsertParameterMaster(StrCompId, StrBrandId, strLocationId, "Allow TAX Edit On Individual Transactions Purchase", "False", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Allow TAX Edit On Individual Transactions Purchase", DDL_Edit_Individual_Purchase.SelectedValue.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }


        DisplayMessage("Record Updated", "green");
    }
    protected void btnCancelPurchase_Click(object sender, EventArgs e)
    {
        setParameterValue();
    }
    protected void txtPurchaseAccountNo_TextChanged(object sender, EventArgs e)
    {
        if (txtPurchaseAccountNo.Text != "")
        {
            string strTransId = GetAccountIdForPurchase();
            if (strTransId != "")
            {
                DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
                }
                else
                {
                    txtPurchaseAccountNo.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPurchaseAccountNo);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Account Name");
                txtPurchaseAccountNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPurchaseAccountNo);
            }
        }
    }
    protected void txtExpensesAccountNo_TextChanged(object sender, EventArgs e)
    {
        if (txtExpensesAccountNo.Text != "")
        {
            string strTransId = GetAccountIdForExpenses();
            if (strTransId != "")
            {
                DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
                }
                else
                {
                    txtExpensesAccountNo.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccountNo);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Account Name");
                txtExpensesAccountNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccountNo);
            }
        }
    }
    private string GetAccountIdForPurchase()
    {
        string retval = string.Empty;

        if (txtPurchaseAccountNo.Text != "")
        {

            retval = (txtPurchaseAccountNo.Text.Split('/'))[txtPurchaseAccountNo.Text.Split('/').Length - 1];
            try
            {
                DataTable dtCOA = objCOA.GetCOAByTransId(StrCompId, retval);
                if (dtCOA.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            catch
            {
                retval = "";
            }
            return retval;

        }
        else
        {
            retval = "0";

        }
        return retval;


    }
    private string GetAccountIdForExpenses()
    {
        string retval = string.Empty;

        if (txtExpensesAccountNo.Text != "")
        {

            retval = (txtExpensesAccountNo.Text.Split('/'))[txtExpensesAccountNo.Text.Split('/').Length - 1];
            try
            {
                DataTable dtCOA = objCOA.GetCOAByTransId(StrCompId, retval);
                if (dtCOA.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            catch
            {
                retval = "";
            }
            return retval;

        }
        else
        {
            retval = "0";

        }
        return retval;


    }
    #endregion
    #region Sales
    protected void btnSales_Click(object sender, EventArgs e)
    {
        //PnlSales.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //PnlPurchase.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlUserPermission.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //User_Level.Attributes.Add("class", "tab-pane");
        //pnlInventoryDetail.Attributes.Add("class", "tab-pane");
        //pnlPurchaseDetail.Attributes.Add("class", "tab-pane");
        //PnlSalesDetail.Attributes.Add("class", "tab-pane active");

        //Li_Purchase_Details.Attributes.Add("class", "tab-pane");
        //Li_Sales_Details.Attributes.Add("class", "tab-pane active");
        //Li_User_Details.Attributes.Add("class", "tab-pane");
        //Li_Inventory_Details.Attributes.Add("class", "tab-pane");

    }
    protected void btnUserPermission_Click(object sender, EventArgs e)
    {
        //PnlSales.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlPurchase.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlUserPermission.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //User_Level.Attributes.Add("class", "tab-pane active");
        //pnlInventoryDetail.Attributes.Add("class", "tab-pane");
        //pnlPurchaseDetail.Attributes.Add("class", "tab-pane");
        //PnlSalesDetail.Attributes.Add("class", "tab-pane");

        //Li_Purchase_Details.Attributes.Add("class", "tab-pane");
        //Li_Sales_Details.Attributes.Add("class", "tab-pane");
        //Li_User_Details.Attributes.Add("class", "tab-pane active");
        //Li_Inventory_Details.Attributes.Add("class", "tab-pane");

    }
    protected void btnSaveSales_Click(object sender, EventArgs e)
    {
        DataTable Dt_Parameter = objSys.GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "" && ddlSalesTax.SelectedValue == "True")
            {
                DisplayMessage("Please Update Tax System in System Parameter befor IsTax Update");
                return;
            }
            else
            {
                objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsTaxSales", ddlSalesTax.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Sales Price", ddlSalesPrice.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Sales Account Parameter", GetAccountIdForSales(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsDiscountSales", ddlSalesDiscount.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsProductNameShow", ddlIsProductNameshow.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Sales Quotation Footer", "", txtSalesQuotationFooter.Content, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesQuotationHeader", "", txtSalesQuotationheader.Content, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Terms & Condition(Sales Quotation)", "", txtSalesQuotationTermsandconditon.Content, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Terms & Condition(Sales Invoice)", "", txtSalesInvoiceTermsandconditon.Content, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesQuotationApproval", ddlSalesQuotationApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesOrderApproval", ddlSalesOrderApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "CashSalesOrderApproval", ddlCashOrderApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesInvoiceApproval", ddlSalesInvoiceApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "CreditInvoiceApproval", ddlCreditInvoiceApproval.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Price Should be", ddlPriceshouldbe.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Invoice Report Format", ddlReportFormat.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Sales Report Header", ddlSalesReportHeader.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesInquiryReportHeaderLevel", ddlRptSI.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesQuotationReportHeaderLevel", ddlRptSQ.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesOrderReportHeaderLevel", ddlRptSO.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "SalesInvoiceReportHeaderLevel", ddlRptSIn.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Delivery Voucher allow", chkIsDelivery.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Allow Retail Cash Invoice", chkCashInvoiceAllow.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        //

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "DeliveryVoucherReportHeaderLevel", ddlRptDV.SelectedValue.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is barcode allow(report)", ddlIsBarcodeallow.SelectedValue.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Sales Below Cost Price", chkSalesbelowcostprice.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());


        //heer we checking that parameter exist or not if exist then update  otherwise insert

        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsInvoiceScanning").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'IsInvoiceScanning'           ,'" + chkscanningsolution.Checked.ToString() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString() + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString() + "'           ,'admin'           ,'" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsInvoiceScanning", chkscanningsolution.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'IsDeliveryScanning'           ,'" + chkDeliveryScanningSolution.Checked.ToString() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString() + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString() + "'           ,'admin'           ,'" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsDeliveryScanning", chkDeliveryScanningSolution.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }
        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Quotation Allow for New").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'Is Quotation Allow for New'           ,'" + SalesQtaPrm.Checked.ToString() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString("yyyy-MM-dd") + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString("yyyy-MM-dd") + "'           ,'admin'           ,'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Quotation Allow for New", SalesQtaPrm.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        if (objParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Is Sales Running Bill").Rows.Count==0)
        {
            objParam.InsertParameterMaster(StrCompId, StrBrandId, strLocationId, "Is Sales Running Bill", ChkSalesRunningBill.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Sales Running Bill", ChkSalesRunningBill.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsCostingEntry").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster] ([Company_Id],[Brand_Id],[Location_Id],[ParameterName],[ParameterValue],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]) VALUES('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'IsCostingEntry','" + chkCostingEntry.Checked.ToString() + "',' ',' ',' ',' ',' ','False','" + DateTime.Now.ToString() + "','True','admin','" + DateTime.Now.ToString() + "','admin','" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsCostingEntry", chkCostingEntry.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        if (objParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Allow TAX Edit On Individual Transactions Sales").Rows.Count == 0)
        {
            objParam.InsertParameterMaster(StrCompId, StrBrandId, strLocationId, "Allow TAX Edit On Individual Transactions Sales", "False", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Allow TAX Edit On Individual Transactions Sales", DDL_Edit_Individual_Sales.SelectedValue.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        }


        DisplayMessage("Record Updated", "green");
        setParameterValue();
    }

    protected void btnCancelSales_Click(object sender, EventArgs e)
    {
        setParameterValue();
    }
    protected void txtSalesAccountNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtSalesAccountNumber.Text != "")
        {
            string strTransId = GetAccountIdForSales();
            if (strTransId != "")
            {
                DataTable dtAccount = objCOA.GetCOAByTransId(StrCompId, strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
                }
                else
                {
                    txtSalesAccountNumber.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesAccountNumber);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Account Name");
                txtSalesAccountNumber.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesAccountNumber);

            }
        }
    }
    private string GetAccountIdForSales()
    {
        string retval = string.Empty;

        if (txtSalesAccountNumber.Text != "")
        {

            retval = (txtSalesAccountNumber.Text.Split('/'))[txtSalesAccountNumber.Text.Split('/').Length - 1];
            try
            {
                DataTable dtCOA = objCOA.GetCOAByTransId(StrCompId, retval);
                if (dtCOA.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            catch
            {
                retval = "";
            }
            return retval;

        }
        else
        {
            retval = "0";

        }
        return retval;


    }
    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

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
        else
        {
            if (prefixText.Length > 2)
            {
                txt = null;
            }
            else
            {
                dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dtCOA.Rows.Count > 0)
                {
                    txt = new string[dtCOA.Rows.Count];
                    for (int i = 0; i < dtCOA.Rows.Count; i++)
                    {
                        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return txt;
    }

    protected void Btn_Tab_Click(object sender, EventArgs e)
    {
        Label13.Text = Resources.Attendance.Purchase.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_tab_position()", true);
    }

    #region InventoryParameter

    protected void btnSaveInventory_Click(object sender, EventArgs e)
    {
        if (txtCommisisonpaymentallow.Text == "")
        {
            txtCommisisonpaymentallow.Text = "0";
        }

        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Serial on Transfer Voucher", chkIsRequiredOnTransfer.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Serial on Purchase Goods", chkisRequiredonPurchase.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Is Serial on Sales Return", chkisRequiredonSalesreturn.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Commission Payment Allow(In Month)", txtCommisisonpaymentallow.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());



        //heer we checking that parameter exist or not if exist then update  otherwise insert

        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferOutScanning").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'IsTransferOutScanning'           ,'" + chkTransferOutScanning.Checked.ToString() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString() + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString() + "'           ,'admin'           ,'" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsTransferOutScanning", chkTransferOutScanning.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }



        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferInScanning").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'IsTransferInScanning'           ,'" + chkTransferInScanning.Checked.ToString() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString() + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString() + "'           ,'admin'           ,'" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "IsTransferInScanning", chkTransferInScanning.Checked.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }


        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Item_Reorder_Functionality").Rows.Count == 0)
        {
            string strsql = "INSERT INTO [dbo].[Inv_ParameterMaster]           ([Company_Id]           ,[Brand_Id]           ,[Location_Id]           ,[ParameterName]           ,[ParameterValue]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES           ('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + Session["LocId"].ToString() + "', 'Item_Reorder_Functionality'           ,'" + ddlItemReorder.SelectedValue.Trim() + "'      ,' '          ,' '           ,' '        ,' '          ,' '         ,'False'         ,'" + DateTime.Now.ToString() + "'          ,'True'           ,'admin'          ,'" + DateTime.Now.ToString() + "'           ,'admin'           ,'" + DateTime.Now.ToString() + "')";
            objDa.execute_Command(strsql);
        }
        else
        {
            objParam.UpdateParameterMaster(StrCompId, StrBrandId, strLocationId, "0", "Item_Reorder_Functionality", ddlItemReorder.SelectedValue.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        }





        DisplayMessage("Record Updated", "green");
        setParameterValue();

    }

    protected void btnResetInventory_Click(object sender, EventArgs e)
    {

        setParameterValue();
    }
    #endregion


    #region userLevelParameter

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }

    protected void ddlEmp_TextChanged(object sender, EventArgs e)
    {
        string empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];

        DataTable dt = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
        if (dt.Rows.Count > 0)
        {
            // txtEmailId.Text = dt.Rows[0]["Email_Id"].ToString();
        }

    }

    #endregion
}
