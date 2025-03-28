using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
public partial class ServiceManagement_Gate_Pass : System.Web.UI.Page
{
    SystemParameter objSysParam = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    Inv_ProductMaster ObjProductMaster = null;
    DataAccessClass objDa = null;
    Inv_UnitMaster UM = null;
    LocationMaster objLocation = null;
    SM_GetPass_Header objGatepassheader = null;
    SM_GetPass_Detail objGatepassDetail = null;
    SM_JobCard_ItemDetail objJobCardItemdetail = null;
    Set_AddressMaster ObjAdd = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Inv_StockDetail objStockDetail = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductledger = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objGatepassheader = new SM_GetPass_Header(Session["DBConnection"].ToString());
        objGatepassDetail = new SM_GetPass_Detail(Session["DBConnection"].ToString());
        objJobCardItemdetail = new SM_JobCard_ItemDetail(Session["DBConnection"].ToString());
        ObjAdd = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/Gate_Pass.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);
            Reset();
            FillGrid();
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtender_txtExpectedEnddate.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            btnRefreshReport_Click(null, null);
            ListItem Li = new ListItem();
            Li.Text = "--Select--";
            Li.Value = "0";
            txtGetPassNo.Text = "";
            txtGetPassNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtGetPassNo.Text;
            fillExpenses();
            GetRmaItemdetail();
        }
        try
        {
            GvGatePass.DataSource = Session["dtGatePassData"] as DataTable;
            GvGatePass.DataBind();
        }
        catch
        {
        }
        //AllPageCode();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        btnInquirySave.Visible = clsPagePermission.bEdit;
        btnClose.Visible = clsPagePermission.bView;
        GvGatePass.Columns[0].Visible = clsPagePermission.bPrint;
        GvGatePass.Columns[1].Visible = clsPagePermission.bView;
        GvGatePass.Columns[2].Visible = clsPagePermission.bEdit;
        GvGatePass.Columns[3].Visible = clsPagePermission.bDelete;
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Focus();
    }
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string strSuppplierId = string.Empty;
        string strContactId = string.Empty;
        string strShipTo = string.Empty;
        string strShipping_Addresss = string.Empty;
        string strShipping_Line = string.Empty;
        string strExpenses = string.Empty;
        string strshippingAccount = string.Empty;
        string strExpensesAccount = string.Empty;
        string Status = string.Empty;
        string strvoucherno = string.Empty;
        string strsql = string.Empty;
        if (((Button)sender).ID == "btnClose")
        {
            Status = "Post";
        }
        else
        {
            Status = "UnPost";
        }
        //here we are checking that any item selected or not for send in rma
        bool Result = false;
        foreach (GridViewRow gvrow in gvRMAItemDetail.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chk")).Checked)
            {
                if (hdnValue.Value == "")
                {
                    strsql = "select Job_No from SM_GetPass_Detail where Job_No=" + ((Label)gvrow.FindControl("lblTransId")).Text + " and Status='Send'";
                }
                else
                {
                    strsql = "select Job_No from SM_GetPass_Detail where Job_No=" + ((Label)gvrow.FindControl("lblTransId")).Text + " and Status='Send' and Header_Id<>" + hdnValue.Value + "";
                }
                try
                {
                    if (objDa.return_DataTable(strsql).Rows.Count > 0)
                    {
                        DisplayMessage("Gate pass already exist for product code " + ((Label)gvrow.FindControl("lblgvProduct")).Text + " and job no. is " + ((Label)gvrow.FindControl("lblJobNo")).Text);
                        return;
                    }
                }
                catch
                {
                }
                Result = true;
                break;
            }
        }
        if (!Result)
        {
            DisplayMessage("select an item to create a gate pass");
            return;
        }
        ///here we are checking stock qty for internal rma item
        ///
        if (Status.Trim() == "Post")
        {
            double StockQty = 0;
            foreach (GridViewRow gvr in gvRMAItemDetail.Rows)
            {
                if (((CheckBox)gvr.FindControl("chk")).Checked)
                {
                    if (((Label)gvr.FindControl("lblType")).Text.Trim() == "External")
                    {
                        continue;
                    }
                    if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ((Label)gvr.FindControl("lblgvProductId")).Text, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString().Trim().ToUpper() == "S")
                    {
                        if (((Label)gvr.FindControl("lblgvRequiredQty")).Text == "")
                        {
                            ((Label)gvr.FindControl("lblgvRequiredQty")).Text = "0";
                        }
                        try
                        {
                            StockQty = Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, Session["FinanceYearId"].ToString(), ((Label)gvr.FindControl("lblgvProductId")).Text).Rows[0]["Quantity"].ToString());
                        }
                        catch
                        {
                            StockQty = 0;
                        }
                        if (Convert.ToDouble(((Label)gvr.FindControl("lblgvRequiredQty")).Text) > StockQty)
                        {
                            DisplayMessage("Stock not available for " + ((Label)gvr.FindControl("lblgvProduct")).Text + " product");
                            return;
                        }
                    }
                }
            }
        }
        if (txtgetPassdate.Text == "")
        {
            DisplayMessage("Enter gate pass date");
            txtgetPassdate.Focus();
            return;
        }
        if (txtSuppliername.Text == "")
        {
            DisplayMessage("Enter Manufacturer name");
            txtSuppliername.Focus();
            return;
        }
        if (txtSuppliername.Text != "")
        {
            strSuppplierId = txtSuppliername.Text.Split('/')[3].ToString();
        }
        if (txtEContact.Text != "")
        {
            strContactId = txtEContact.Text.Split('/')[3].ToString();
        }
        else
        {
            strContactId = "0";
        }
        if (txtShipCustomerName.Text != "")
        {
            strShipTo = txtShipCustomerName.Text.Split('/')[1].ToString();
        }
        else
        {
            strShipTo = "0";
        }
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAddId = ObjAdd.GetAddressDataByAddressName(txtShipingAddress.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strShipping_Addresss = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
        }
        else
        {
            strShipping_Addresss = "0";
        }
        if (txtShippingLine.Text != "")
        {
            strShipping_Line = txtShippingLine.Text.Split('/')[1].ToString();
        }
        else
        {
            strShipping_Line = "0";
        }
        if (ddlExpense.SelectedIndex > 0)
        {
            strExpenses = ddlExpense.SelectedValue;
        }
        else
        {
            strExpenses = "0";
        }
        if (txtShippingAcc.Text != "")
        {
            strshippingAccount = txtShippingAcc.Text.Split('/')[1].ToString();
        }
        else
        {
            strshippingAccount = "0";
        }
        if (txtExpensesAccount.Text != "")
        {
            strExpensesAccount = txtExpensesAccount.Text.Split('/')[1].ToString();
        }
        else
        {
            strExpensesAccount = "0";
        }
        if (txtExpectedRecdate.Text == "")
        {
            DisplayMessage("Enter Expected Receive date");
            txtExpectedRecdate.Focus();
            return;
        }
        if (txtShippingDate.Text == "")
        {
            DisplayMessage("Enter Shipping date");
            txtShippingDate.Focus();
            return;
        }
        if (txtpaidamount.Text == "")
        {
            txtpaidamount.Text = "0";
        }
        else
        {
            try
            {
                Convert.ToDouble(txtpaidamount.Text);
            }
            catch
            {
                DisplayMessage("Enter Numeric value in shipping charge");
                txtpaidamount.Focus();
                return;
            }
        }
        if (ddlExpense.SelectedIndex > 0 || Convert.ToDouble(txtpaidamount.Text) > 0)
        {
            if (Convert.ToDouble(txtpaidamount.Text) <= 0)
            {
                DisplayMessage("Enter Shipping Charge");
                txtpaidamount.Focus();
                return;
            }
            if (txtShippingAcc.Text == "")
            {
                DisplayMessage("Enter shipping Account");
                txtShippingAcc.Focus();
                return;
            }
            if (txtExpensesAccount.Text == "")
            {
                DisplayMessage("Enter Expenses Account");
                txtExpensesAccount.Focus();
                return;
            }
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strVoucherId = string.Empty;
        try
        {
            if (hdnValue.Value == "")
            {
                int b = objGatepassheader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtGetPassNo.Text, txtgetPassdate.Text, txtShippingDate.Text, txtExpectedRecdate.Text, strSuppplierId, ddlShipBy.SelectedValue, strShipping_Line, txtAirwaybillno.Text, strContactId, txtContactNo.Text, txtEmailId.Text, txtRemarks.Text, Status, strShipTo, strShipping_Addresss, strExpenses, txtpaidamount.Text, strshippingAccount, strExpensesAccount, DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    strVoucherId = b.ToString();

                    string dtCount = objGatepassheader.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                    objGatepassheader.Updatecode(b.ToString(), txtGetPassNo.Text + dtCount, ref trns);
                    txtGetPassNo.Text = txtGetPassNo.Text + dtCount;

                    strvoucherno = txtGetPassNo.Text;
                    //here we will insert record in detail table
                    int counter = 0;
                    //dt.Columns.Add("DeliveryDate");
                    //dt.Columns.Add("Problem");
                    foreach (GridViewRow gvr in gvRMAItemDetail.Rows)
                    {
                        if (((CheckBox)gvr.FindControl("chk")).Checked)
                        {
                            objGatepassDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, b.ToString(), ((Label)gvr.FindControl("lblTransId")).Text, ((Label)gvr.FindControl("lblgvProductId")).Text, ((Label)gvr.FindControl("lblgvProblem")).Text, "Send", txtgetPassdate.Text, DateTime.Now.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //insert record in labour detail table
                    DisplayMessage("Record Saved", "green");
                }
            }
            else
            {
                strVoucherId = hdnValue.Value;
                objGatepassheader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtGetPassNo.Text, txtgetPassdate.Text, txtShippingDate.Text, txtExpectedRecdate.Text, strSuppplierId, ddlShipBy.SelectedValue, strShipping_Line, txtAirwaybillno.Text, strContactId, txtContactNo.Text, txtEmailId.Text, txtRemarks.Text, Status, strShipTo, strShipping_Addresss, strExpenses, txtpaidamount.Text, strshippingAccount, strExpensesAccount, DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strvoucherno = txtGetPassNo.Text;
                //here we will delete and reinsert record in item detail 
                objGatepassDetail.DeleteRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, ref trns);
                foreach (GridViewRow gvr in gvRMAItemDetail.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chk")).Checked)
                    {
                        objGatepassDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, ((Label)gvr.FindControl("lblTransId")).Text, ((Label)gvr.FindControl("lblgvProductId")).Text, ((Label)gvr.FindControl("lblgvProblem")).Text, "Send", txtgetPassdate.Text, DateTime.Now.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                DisplayMessage("Record Updated", "green");
            }
            //accounts entry
            if (Status == "Post")
            {
                //here we are entering record in  stock table 
                foreach (GridViewRow gvr in gvRMAItemDetail.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chk")).Checked)
                    {
                        if (((Label)gvr.FindControl("lblType")).Text.Trim() == "External")
                        {
                            continue;
                        }
                        if (((Label)gvr.FindControl("lblproductSerial")).Text.Trim() != "")
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "RMA", strVoucherId, ((Label)gvr.FindControl("lblgvProductId")).Text, ((Label)gvr.FindControl("lblUnitId")).Text, "O", "0", "0", ((Label)gvr.FindControl("lblgvRequiredQty")).Text, DateTime.Now.ToString(), ((Label)gvr.FindControl("lblproductSerial")).Text, DateTime.Now.ToString(), "0", "0", "0", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        ObjProductledger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "RMA", strVoucherId, ddlLoc.SelectedValue, ((Label)gvr.FindControl("lblgvProductId")).Text, ((Label)gvr.FindControl("lblUnitId")).Text, "O", "0", "0", "0", ((Label)gvr.FindControl("lblgvRequiredQty")).Text, "1/1/1800", GetAverageCost(((Label)gvr.FindControl("lblgvProductId")).Text.Trim()).ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), objSysParam.getDateForInput(txtgetPassdate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }
                }
                double PaidTotalExpenses = Convert.ToDouble(txtpaidamount.Text);
                if (PaidTotalExpenses != 0)
                {
                    string strNarration = "Shipping Expenses for get pass no . " + strvoucherno + "";
                    //For Bank Account
                    string strAccountId = string.Empty;
                    string strOtherAccountNo = "0";
                    strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                    Ac_ParameterMaster objacmaster = new Ac_ParameterMaster(Session["CompId"].ToString(), ref trns);
                    string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), ddlLoc.SelectedValue, ref trns).Rows[0]["Field1"].ToString();
                    if (txtShippingAcc.Text.Split('/')[1].ToString() == objacmaster.supplierAcNo)
                    {
                        strOtherAccountNo = new Ac_AccountMaster(Session["DBConnection"].ToString()).GetSupplierAccountByCurrency(strSuppplierId, strCurrencyId).ToString();
                        //strOtherAccountNo = strSuppplierId;
                        if (strOtherAccountNo == "0")
                        {
                            throw new Exception("Supplier doesn't have account, Plase contact to Accounts Department");
                        }
                    }
                    //for Voucher Number
                    string strVoucherNumber = ObjDoc.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                    if (strVoucherNumber != "")
                    {
                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, Session["FinanceYearId"].ToString(), ref trns);
                        if (dtCount.Rows.Count > 0)
                        {
                            dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        if (dtCount.Rows.Count == 0)
                        {
                            strVoucherNumber = strVoucherNumber + "1";
                        }
                        else
                        {
                            strVoucherNumber = strVoucherNumber + (dtCount.Rows.Count + 1);
                        }
                    }
                    int VoucherId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, Session["FinanceYearId"].ToString(), ddlLoc.SelectedValue, "0", strvoucherno, "SM_RMA", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", strNarration, strCurrencyId, "0.00", strNarration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //str for Employee Id
                    //For Debit
                    string strCompanyCrrValueDr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), PaidTotalExpenses.ToString(), ref trns,Session["CompId"].ToString(),Session["LocId"].ToString());
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalExpenses.ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //For Credit
                    string strCompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), PaidTotalExpenses.ToString(), ref trns, Session["CompId"].ToString(), Session["LocId"].ToString());
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtShippingAcc.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalExpenses.ToString(), strNarration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", (strAccountId.Split(',').Contains(txtShippingAcc.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            //code end
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();
            Reset();
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
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
    public static double GetAverageCost(string strProductId)
    {
        Inv_StockDetail objStockDetail = new Inv_StockDetail(HttpContext.Current.Session["DBConnection"].ToString());
        double AverageCost = 0;
        try
        {
            AverageCost = Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Field2"].ToString());
        }
        catch
        {
            AverageCost = 0;
        }
        return AverageCost;
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "Get_Pass_Date" || ddlFieldName.SelectedItem.Value == "Shipping_date" || ddlFieldName.SelectedItem.Value == "Expected_Receive_Date")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    txtValue.Text = objSysParam.getDateForInput(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtValue.Text = "";
                return;
            }
        }
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
            DataTable dtCustomInq = (DataTable)Session["dtGatePassData"];
            DataView view = new DataView(dtCustomInq, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //cmn.FillData((object)GvCustomerInquiry, view.ToTable(), "", "");
            GvGatePass.DataSource = view.ToTable();
            GvGatePass.DataBind();
            Session["dtGatePassData"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        //AllPageCode();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvCustomerInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtGatePassData"];
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
        Session["dtGatePassData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvGatePass, dt, "", "");
        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvGatePass.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtGatePassData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvGatePass, dt, "", "");
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ddlLoc.Enabled = false;
        ddlLoc.SelectedValue = e.CommandName.ToString();

        DataTable dt = objGatepassheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {

            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;

            try
            {
                GetRmaItemdetailByVendorForView(dt.Rows[0]["Manufacturer_Id"].ToString(), e.CommandArgument.ToString());
                if (gvRMAItemDetail.Rows.Count == 0)
                {
                    GetRmaItemdetailForPostedRecord(e.CommandArgument.ToString());
                }
            }
            catch
            {

            }


            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                if (ddlPosted.SelectedIndex != 0)
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }

            }
            if (dt.Rows[0]["Status"].ToString() == "Post")
            {
                if (objSenderID != "lnkViewDetail")
                {
                    DisplayMessage("You can not edit posted record");
                    return;
                }
                GetRmaItemdetailForPostedRecord(e.CommandArgument.ToString());
            }
            else
            {
                GetRmaItemdetail();
            }
            hdnValue.Value = e.CommandArgument.ToString();
            txtGetPassNo.Text = dt.Rows[0]["Get_Pass_No"].ToString();
            txtgetPassdate.Text = Convert.ToDateTime(dt.Rows[0]["Get_Pass_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtShippingDate.Text = Convert.ToDateTime(dt.Rows[0]["Shipping_date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSuppliername.Text = dt.Rows[0]["ManufacturerName"].ToString() + "///" + dt.Rows[0]["Manufacturer_Id"].ToString();
            Session["ContactID"] = dt.Rows[0]["Manufacturer_Id"].ToString();
            if (dt.Rows[0]["Contact_Person"].ToString() != "0")
            {
                txtEContact.Text = dt.Rows[0]["ContactPersonName"].ToString() + "///" + dt.Rows[0]["Contact_Person"].ToString();
            }
            txtContactNo.Text = dt.Rows[0]["Contact_Person_Telephone"].ToString();
            txtEmailId.Text = dt.Rows[0]["Contact_Person_EmailId"].ToString();
            if (dt.Rows[0]["Field1"].ToString() != "" && dt.Rows[0]["Field1"].ToString() != "0")
            {
                txtShipCustomerName.Text = dt.Rows[0]["ShipCustomerName"].ToString() + "/" + dt.Rows[0]["Field1"].ToString();
            }
            if (dt.Rows[0]["Field2"].ToString() != "" && dt.Rows[0]["Field2"].ToString() != "0")
            {
                txtShipingAddress.Text = dt.Rows[0]["Addressname"].ToString();
            }
            txtExpectedRecdate.Text = Convert.ToDateTime(dt.Rows[0]["Expected_Receive_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            if (dt.Rows[0]["Shipping_Company_Name"].ToString() != "" && dt.Rows[0]["Shipping_Company_Name"].ToString() != "0")
            {
                txtShippingLine.Text = dt.Rows[0]["ShippingLine"].ToString() + "/" + dt.Rows[0]["Shipping_Company_Name"].ToString();
            }
            if (dt.Rows[0]["Field3"].ToString() != "" && dt.Rows[0]["Field3"].ToString() != "0")
            {
                ddlExpense.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            if (dt.Rows[0]["Field5"].ToString() != "" && dt.Rows[0]["Field5"].ToString() != "0")
            {
                txtShippingAcc.Text = dt.Rows[0]["shippingAccountname"].ToString() + "/" + dt.Rows[0]["Field5"].ToString();
            }
            if (dt.Rows[0]["Field6"].ToString() != "" && dt.Rows[0]["Field6"].ToString() != "0")
            {
                txtExpensesAccount.Text = dt.Rows[0]["expensesAccountname"].ToString() + "/" + dt.Rows[0]["Field6"].ToString();
            }
            txtpaidamount.Text = dt.Rows[0]["Field4"].ToString();
            ddlShipBy.SelectedValue = dt.Rows[0]["Shipping_via"].ToString();
            txtAirwaybillno.Text = dt.Rows[0]["Docket_Number"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            DataTable dtDetail = objGatepassDetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
            if (dtDetail.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in gvRMAItemDetail.Rows)
                {
                    if (new DataView(dtDetail, "Job_No='" + ((Label)gvrow.FindControl("lblTransId")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ((CheckBox)gvrow.FindControl("chk")).Checked = true;
                    }
                }
            }
            TabContainer2.ActiveTabIndex = 0;
        }
    }
    protected void OnUpdateCommand(object sender, CommandEventArgs e)
    {
        if (txtUpdateDate.Text.Trim() == "")
        {
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtUpdateDate.Text.Trim());
            }
            catch
            {
                DisplayMessage("Invalid Date Format ,Try Again");
                txtUpdateDate.Text = "";
                txtUpdateDate.Focus();
                return;
            }
        }
        if (hdnTrans_Id.Value != "")
        {
            objDa.execute_Command("update SM_GetPass_Header set Expected_Receive_Date='" + txtUpdateDate.Text + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnTrans_Id.Value + "");
            DisplayMessage("Record Updated Successfully", "green");
            hdnTrans_Id.Value = "";
            txtUpdateDate.Text = "";
            FillGrid();
        }
        else
        {
            DisplayMessage("cant Update");
            return;
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objGatepassheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Status"].ToString() == "Post")
            {
                DisplayMessage("You can not delete posted record");
                return;
            }
        }
        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        b = objGatepassheader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), hdnValue.Value);
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGrid();
        Reset();
        //AllPageCode();
    }
    protected void txtECustomer_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtSuppliername.Text != "")
        {
            try
            {
                strCustomerId = txtSuppliername.Text.Split('/')[3].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtSuppliername.Text = "";
                txtEContact.Text = "";
                txtContactNo.Text = "";
                txtEmailId.Text = "";
                txtSuppliername.Focus();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliername);
                //txtContactNo.Text = "";
                //txtEContact.Text = "";
                //txtEmailId.Text = "";
                //Session["ContactID"] = "0";
            }
            else
            {
                txtContactNo.Text = txtSuppliername.Text.Split('/')[1].ToString();
                txtEmailId.Text = txtSuppliername.Text.Split('/')[2].ToString();
                DataTable dt = objContact.GetContactTrueAllData(strCustomerId, "Individual");
                txtEContact.Text = "";
                if (dt.Rows.Count > 0)
                {
                    txtEContact.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[0]["Trans_Id"].ToString();
                }
                else
                {
                    txtEContact.Text = txtSuppliername.Text;
                }
                txtEContact_TextChanged(null, null);
                txtEContact.Focus();
                Session["ContactID"] = strCustomerId;
                GetRmaItemdetailByVendor(strCustomerId);

            }
            txtEContact.Focus();
        }
        // AllPageCode();
    }
    protected void txtEContact_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtEContact.Text != "")
        {
            try
            {
                strCustomerId = txtEContact.Text.Split('/')[3].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtEContact.Text = "";
                txtContactNo.Text = "";
                txtEmailId.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEContact);
            }
            else
            {
                DataTable dtcust = objContact.GetContactTrueById(strCustomerId.ToString());
                if (dtcust.Rows.Count > 0)
                {
                    if (dtcust.Rows[0]["Field2"].ToString() != "")
                    {
                        txtContactNo.Text = dtcust.Rows[0]["Field2"].ToString();
                    }
                    if (dtcust.Rows[0]["Field1"].ToString() != "")
                    {
                        txtEmailId.Text = dtcust.Rows[0]["Field1"].ToString();
                    }
                }
            }
            txtContactNo.Focus();
        }
        // AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListShipTo(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtContact = objcontact.GetContactTrueAllData();
        DataTable dtMain = new DataTable();
        dtMain = dtContact.Copy();
        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = HttpContext.Current.Session["ContactID"].ToString();
        if (id == "0")
        {
            dt = ObjContactMaster.GetContactTrueAllData();
            string filtertext = "Name like '%" + prefixText + "%'";
            dt = new DataView(dt, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = ObjContactMaster.GetContactTrueAllData(id, "Individual");
        }
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
            }
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Field2"].ToString() + "/" + dtcon.Rows[i]["Field1"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_Suppliers objSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = objSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtCustomer, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Field2"].ToString() + "/" + dtCon.Rows[i]["Field1"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.GetDistinctAddressName(prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        return str;
    }
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtShipingAddress.Text != "")
        {
            DataTable dtAM = ObjAdd.GetAddressDataByAddressName(txtShipingAddress.Text.Trim().Split('/')[0].ToString());
            //txtShipCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dtAM.Rows.Count > 0)
            {
            }
            else
            {
                txtShipingAddress.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
                return;
            }
        }
        //AllPageCode();
    }
    protected void txtShipTo_TextChanged(object sender, EventArgs e)
    {
        if (txtShipCustomerName.Text != "")
        {
            string[] ShipName = txtShipCustomerName.Text.Split('/');
            DataTable DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());
            if (DtCustomer.Rows.Count > 0)
            {
                DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[1].ToString().Trim());
                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    if (txtShipingAddress.Text == "")
                    {
                        txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                    }
                }
                else
                {
                    txtShipingAddress.Text = "";
                }
            }
            else
            {
                DisplayMessage("Select Ship to in Suggestion Only");
                txtShipCustomerName.Text = "";
                txtShipCustomerName.Focus();
                return;
            }
        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
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
        DataTable dtres = (DataTable)Session["MessageDt"];
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
        ddlLoc.Enabled = true;
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        txtGetPassNo.Text = "";
        txtGetPassNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtGetPassNo.Text;
        GetRmaItemdetail();
        txtgetPassdate.Text = "";
        txtSuppliername.Text = "";
        txtEContact.Text = "";
        hdnValue.Value = "";
        txtContactNo.Text = "";
        txtEmailId.Text = "";
        txtShipCustomerName.Text = "";
        gvRMAItemDetail.DataSource = null;
        gvRMAItemDetail.DataBind();
        txtShipingAddress.Text = "";
        txtExpectedRecdate.Text = "";
        txtRemarks.Text = "";
        txtShippingLine.Text = "";
        ddlExpense.SelectedIndex = 0;
        txtpaidamount.Text = "";
        txtExpensesAccount.Text = "";
        txtShippingAcc.Text = "";
        txtAirwaybillno.Text = "";
        txtShippingDate.Text = "";
        txtgetPassdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.New;
        //AllPageCode();
    }
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        dt = objGatepassheader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        if (ddlPosted.SelectedIndex == 0 || ddlPosted.SelectedIndex == 1)
        {
            dt = new DataView(dt, "Status='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //cmn.FillData((object)GvCustomerInquiry, dt, "", "");
        GvGatePass.DataSource = dt;
        GvGatePass.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtGatePassData"] = dt;
        // AllPageCode();
    }
    protected string GetDocumentNumber()
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "349", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public string GetEmpName()
    {
        string EmpName = string.Empty;
        DataTable dtEmployee = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        try
        {
            EmpName = dtEmployee.Rows[0]["Emp_Name"].ToString() + "/" + dtEmployee.Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
        }
        return EmpName;
    }
    #region Date Search
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "Get_Pass_Date" || ddlFieldName.SelectedItem.Value == "Shipping_date" || ddlFieldName.SelectedItem.Value == "Expected_Receive_Date")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
        ddlFieldName.Focus();
    }
    #endregion
    #region ItemDetail
    protected void imgBtnItemDelete_Command(object sender, CommandEventArgs e)
    {
        //hdnItemEditId.Value = e.CommandArgument.ToString();
        //DataTable dt = getItemDetailinDatatable();
        //dt = new DataView(dt, "Serial_No<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //cmn.FillData((object)GvItemDetail, dt, "", "");
        //ResetDetailsection();
    }
    public string SuggestedProductName(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {
        }
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        return ProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "";
        }
        return ProductName;
    }
    protected string GetUnitName(string ProductId)
    {
        string strUnitName = string.Empty;
        string strUnitId = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {
        }
        if (dt.Rows.Count != 0)
        {
            strUnitId = dt.Rows[0]["UnitId"].ToString();
        }
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }
    protected string GetUnitId(string ProductId)
    {
        string strUnitId = "0";
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {
        }
        if (dt.Rows.Count != 0)
        {
            strUnitId = dt.Rows[0]["UnitId"].ToString();
        }
        return strUnitId;
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = UM.GetUnitMasterById(Session["CompId"].ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    #endregion
    #region Posting
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        //AllPageCode();
    }
    #endregion
    #region ShippingInformation
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtContact = ObjContactMaster.GetAutoCompleteContactTrueAllData(prefixText);
        string[] filterlist = new string[dtContact.Rows.Count];
        if (dtContact.Rows.Count > 0)
        {
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                filterlist[i] = dtContact.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    protected void txtShippingLine_TextChanged(object sender, EventArgs e)
    {
        if (txtShippingLine.Text != "")
        {
            try
            {
                string ContactId = string.Empty;
                try
                {
                    ContactId = txtShippingLine.Text.Split('/')[1].ToString();
                }
                catch
                {
                    ContactId = "0";
                }
                if ((ContactId == "") || (ContactId == "0"))
                {
                    DisplayMessage("Please Select from Suggestions Only");
                    txtShippingLine.Text = "";
                    txtShippingLine.Focus();
                    return;
                }
                else
                {
                }
            }
            catch
            {
            }
        }
    }
    public void fillExpenses()
    {
        DataTable dt = ObjShipExp.GetShipExpMaster(Session["CompId"].ToString().ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "AccountName Like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
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
                dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    protected void txtExpensesAccount_TextChanged(object sender, EventArgs e)
    {
        if (txtExpensesAccount.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(Session["CompId"].ToString());
            try
            {
                dtAccount = new DataView(dtAccount, "AccountName='" + txtExpensesAccount.Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                    txtExpensesAccount.Text = "";
                    DisplayMessage("No Account Found");
                    txtExpensesAccount.Focus();
                    return;
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                txtExpensesAccount.Text = "";
                DisplayMessage("No Account Found");
                txtExpensesAccount.Focus();
                return;
            }
        }
    }
    protected void txtShippingAcc_TextChanged(object sender, EventArgs e)
    {
        if (txtShippingAcc.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(Session["CompId"].ToString());
            try
            {
                dtAccount = new DataView(dtAccount, "AccountName='" + txtShippingAcc.Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShippingAcc);
                    txtShippingAcc.Text = "";
                    DisplayMessage("No Account Found");
                    txtShippingAcc.Focus();
                    return;
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShippingAcc);
                txtShippingAcc.Text = "";
                DisplayMessage("No Account Found");
                txtShippingAcc.Focus();
                return;
            }
        }
    }
    protected void ddlExpense_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpense.SelectedValue == "--Select--")
        {
            txtExpensesAccount.Text = "";
        }
        else if (ddlExpense.SelectedValue != "--Select--")
        {
            DataTable dtExp = ObjShipExp.GetShipExpMasterById(Session["CompId"].ToString(), ddlExpense.SelectedValue);
            if (dtExp.Rows.Count > 0)
            {
                string strAccountId = dtExp.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtExpensesAccount.Text = strAccountName + "/" + strAccountId;
                }
            }
        }
        // GetData();
    }
    #endregion
    #region RmaItemList

    public void GetRmaItemdetail()
    {
        DataTable dtDetail = objJobCardItemdetail.GetAllRMARecord_By_Status(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "Post");
        objPageCmn.FillData((object)gvRMAItemDetailPending, dtDetail, "", "");
        //get labour detail 
    }
    public void GetRmaItemdetailByVendor(string vendorId)
    {
        DataTable dtDetail = objJobCardItemdetail.GetAllRMARecord_ByVendorIdID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), vendorId, "Post");
        objPageCmn.FillData((object)gvRMAItemDetail, dtDetail, "", "");
        //get labour detail 
    }



    public void GetRmaItemdetailByVendorForView(string vendorId, string headerId)
    {
        try
        {


            DataTable dtDetail = objJobCardItemdetail.GetAllRMARecord_ByVendorIdID(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, vendorId, "Post");
            objPageCmn.FillData((object)gvRMAItemDetail, dtDetail, "", "");

            //DataTable dtItemDetails = objGatepassDetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), headerId);

            //for (int i = 0; i < dtItemDetails.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dtDetail.Rows.Count; j++)
            //    {
            //        if (dtDetail.Rows[j]["ProductId"].ToString() == dtItemDetails.Rows[i]["Item_Id"].ToString() && dtDetail.Rows[j]["Trans_id"].ToString() == dtItemDetails.Rows[i]["job_no"].ToString())
            //        {
            //            (gvRMAItemDetail.Rows[i].FindControl("chk") as CheckBox).Checked = true;
            //            break;
            //        }
            //    }
            //}
        }
        catch
        {

        }
        //get item detail for view and edit
    }

    public void GetRmaItemdetailForPostedRecord(string strTransId)
    {
        DataTable dtDetail = objJobCardItemdetail.GetAllRMARecord_ForViewOnly(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, strTransId);
        objPageCmn.FillData((object)gvRMAItemDetail, dtDetail, "", "");
        //get labour detail 
    }
    #endregion
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../ServiceManagementReport/RMAReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    protected void imgupdate1_Command(object sender, CommandEventArgs e)
    {
        hdnTrans_Id.Value = e.CommandArgument.ToString();
        txtUpdateDate.Text = e.CommandName.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_UpdateDate_Popup()", true);
    }
    protected void txtpaidamount_TextChanged(object sender, EventArgs e)
    {
        int parsedValue;
        decimal parseddecimal;
        if (!int.TryParse(txtpaidamount.Text, out parsedValue))
        {
            if (!decimal.TryParse(txtpaidamount.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                txtpaidamount.Text = "";
                txtpaidamount.Focus();
                return;
            }
        }
    }
    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A3;
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}