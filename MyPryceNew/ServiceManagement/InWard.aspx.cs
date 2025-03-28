using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;
public partial class ServiceManagement_InWard : System.Web.UI.Page
{
    SM_Ticket_Master objTicketMaster = null;
    SystemParameter objSysParam = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnitMaster = null;
    DataAccessClass objDa = null;
    LocationMaster objLocation = null;
    SM_Inward_header objInWardheader = null;
    SM_Inward_Detail objInWardDetail = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Ac_ChartOfAccount objCOA = null;
    SM_GetPass_Header objGatepassheader = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
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
        objTicketMaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objInWardheader = new SM_Inward_header(Session["DBConnection"].ToString());
        objInWardDetail = new SM_Inward_Detail(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objGatepassheader = new SM_GetPass_Header(Session["DBConnection"].ToString());
        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/InWard.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();
            btnRefreshReport_Click(null, null);
            ListItem Li = new ListItem();
            Li.Text = "--Select--";
            Li.Value = "0";
            txtInWardNo.Text = "";
            txtInWardNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtInWardNo.Text;
            fillExpenses();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
        }
        try
        {
            GvInWard.DataSource = Session["dtInWardData"] as DataTable;
            GvInWard.DataBind();
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
        GvInWard.Columns[0].Visible = true;
        GvInWard.Columns[1].Visible = true;
        GvInWard.Columns[2].Visible = true;
        GvInWard.Columns[3].Visible = true;
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
        double ItemExpenses = 0;
        if (((Button)sender).ID == "btnClose")
        {
            Status = "Post";
        }
        else
        {
            Status = "UnPost";
        }
        //here we are checking that any item selected or not for send in rma
        if (txtInWarddate.Text == "")
        {
            DisplayMessage("Enter Inward date");
            txtInWarddate.Focus();
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
        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Received By");
            txtHandledEmp.Focus();
            return;
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
        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Received Employeee Name");
            txtHandledEmp.Focus();
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
            if (txtpaidamount.Text == "")
            {
                DisplayMessage("Enter Expenses Amount");
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
        DataTable dtTemp = new DataTable();
        if (hdnValue.Value == "")
        {
            dtTemp = objDa.return_DataTable("select GetPass_Id from SM_Inward_Detail");
        }
        else
        {
            dtTemp = objDa.return_DataTable("select GetPass_Id from SM_Inward_Detail where Header_Id<>" + hdnValue.Value + "");
        }
        if (GvItemDetail.Rows.Count > 0)
        {
            if (dtTemp.Rows.Count > 0)
            {
                DataTable dDetailRecord = getItemDetailinDatatable();
                foreach (DataRow dr in dDetailRecord.Rows)
                {
                    try
                    {
                        ItemExpenses += Convert.ToDouble(dr["ExpCharge"].ToString());
                    }
                    catch
                    {
                        ItemExpenses += 0;
                    }
                    if (new DataView(dtTemp, "GetPass_Id=" + dr["GetPass_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        DisplayMessage("Item  " + ProductCode(dr["Item_Id"].ToString()) + "   already received and gate pass number is " + dr["GetPassNo"].ToString());
                        return;
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Product Not Found");
            return;
        }
        string strVoucherId = string.Empty;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (hdnValue.Value == "")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    return;
                }
                int b = objInWardheader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtInWardNo.Text, txtInWarddate.Text, strSuppplierId, strContactId, txtContactNo.Text, txtEmailId.Text, Emp_ID, txtRemarks.Text, strShipping_Line, strExpenses, txtpaidamount.Text, strshippingAccount, strExpensesAccount, ddlShipBy.SelectedValue, txtAirwaybillno.Text, Status, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    strVoucherId = b.ToString();
                    string dtCount = objInWardheader.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                    objInWardheader.Updatecode(b.ToString(), txtInWardNo.Text + dtCount, ref trns);
                    txtInWardNo.Text = txtInWardNo.Text + dtCount;

                    strvoucherno = txtInWardNo.Text;
                    //here we will insert record in detail table
                    DataTable dtDetail = getItemDetailinDatatable();
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        objInWardDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, b.ToString(), dr["Job_No"].ToString(), dr["GetPass_Id"].ToString(), dr["Item_Id"].ToString(), dr["Problem"].ToString(), dr["Status"].ToString(), dr["Verified_By"].ToString(), dr["ExpDetail"].ToString(), dr["ExpCharge"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        string strsql = "update SM_GetPass_Detail set  Status='Received' where Trans_Id=" + dr["GetPass_Id"].ToString() + "";
                        objDa.execute_Command(strsql, ref trns);
                        if (Status == "Post")
                        {
                            //if item status is  complete then we will update in job card item detail 
                            if (dr["Status"].ToString() == "Complete")
                            {
                                strsql = "update SM_JobCards_ItemDetail set Status='Complete' where Trans_Id=" + dr["Job_No"].ToString() + "";
                                objDa.execute_Command(strsql, ref trns);
                            }
                        }
                    }
                    //DisplayMessage("Record Saved","green");
                }
            }
            else
            {
                strVoucherId = hdnValue.Value;
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    return;
                }
                objInWardheader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtInWardNo.Text, txtInWarddate.Text, strSuppplierId, strContactId, txtContactNo.Text, txtEmailId.Text, Emp_ID, txtRemarks.Text, strShipping_Line, strExpenses, txtpaidamount.Text, strshippingAccount, strExpensesAccount, ddlShipBy.SelectedValue, txtAirwaybillno.Text, Status, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strvoucherno = txtInWardNo.Text;
                //here we will delete and reinsert record in item detail 
                objInWardDetail.DeleteRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, ref trns);
                //here we will insert record in detail table
                DataTable dtDetail = getItemDetailinDatatable();
                foreach (DataRow dr in dtDetail.Rows)
                {
                    objInWardDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, dr["Job_No"].ToString(), dr["GetPass_Id"].ToString(), dr["Item_Id"].ToString(), dr["Problem"].ToString(), dr["Status"].ToString(), dr["Verified_By"].ToString(), dr["ExpDetail"].ToString(), dr["ExpCharge"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    string strsql = "update SM_GetPass_Detail set  Status='Received' where Trans_Id=" + dr["GetPass_Id"].ToString() + "";
                    objDa.execute_Command(strsql, ref trns);
                    if (Status == "Post")
                    {
                        //if item status is  complete then we will update in job card item detail 
                        if (dr["Status"].ToString() == "Complete")
                        {
                            strsql = "update SM_JobCards_ItemDetail set Status='Complete' where Trans_Id=" + dr["Job_No"].ToString() + "";
                            objDa.execute_Command(strsql, ref trns);
                        }
                    }
                }
            }
            //accounts entry
            //
            if (Status == "Post")
            {
                DataTable dtDetail = getItemDetailinDatatable();
                //here we are entering record in  stock table 
                foreach (DataRow dr in dtDetail.Rows)
                {
                    DataTable dtJobDetail = objDa.return_DataTable("select SM_JobCards_Header.Field3,SM_JobCards_ItemDetail.ProductSerialNo,SM_JobCards_ItemDetail.qty from SM_JobCards_Header left join SM_JobCards_ItemDetail on SM_JobCards_Header.Trans_Id = SM_JobCards_ItemDetail.Header_Id where SM_JobCards_Header.Trans_Id=" + dr["Job_No"].ToString() + "", ref trns);
                    if (dtJobDetail.Rows[0]["Field3"].ToString().Trim() == "External")
                    {
                        continue;
                    }
                    if (dtJobDetail.Rows[0]["ProductSerialNo"].ToString().Trim() != "")
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "IW", strVoucherId, dr["Item_Id"].ToString(), GetUnitId(dr["Item_Id"].ToString(), ref trns), "I", "0", "0", dtJobDetail.Rows[0]["qty"].ToString().Trim(), DateTime.Now.ToString(), dtJobDetail.Rows[0]["ProductSerialNo"].ToString().Trim(), DateTime.Now.ToString(), "0", "0", "0", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    ObjProductledger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "IW", strVoucherId, Session["LocId"].ToString(), dr["Item_Id"].ToString(), GetUnitId(dr["Item_Id"].ToString(), ref trns), "I", "0", "0", dtJobDetail.Rows[0]["qty"].ToString().Trim(), "0", "1/1/1800", GetAverageCost(dr["Item_Id"].ToString(), ref trns).ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), objSysParam.getDateForInput(txtInWarddate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                }
                double PaidTotalExpenses = Convert.ToDouble(txtpaidamount.Text);
                if (ItemExpenses != 0)
                {
                    if (txtShippingAcc.Text.Trim() == "")
                    {
                        DisplayMessage("Please Enter Account (Debit) Details");
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    if (txtExpensesAccount.Text.Trim() == "")
                    {
                        DisplayMessage("Please Enter Account (Credit) Details");
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    string strNarration = "Expenses Detail for Inward no . " + strvoucherno + "";
                    string strRepairingNarration = "Repairing Charges for Inward no . " + strvoucherno + "";
                    string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), ddlLoc.SelectedValue, ref trns).Rows[0]["Field1"].ToString();
                    string strAccountId = string.Empty;
                    string strOtherAccountNo = "0";
                    strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
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
                    int VoucherId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", strvoucherno, "SM_InWard", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", strNarration, strCurrencyId, "0.00", strNarration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    if (ItemExpenses != 0 && PaidTotalExpenses != 0)
                    {
                        //For Debit
                        string strCompanyCrrValueDr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (PaidTotalExpenses + ItemExpenses).ToString(), ref trns,Session["CompId"].ToString(),Session["LocId"].ToString());
                        string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", (PaidTotalExpenses + ItemExpenses).ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        //for credit of itemExpenses
                        string strCompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), ItemExpenses.ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtShippingAcc.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", ItemExpenses.ToString(), "Repairing Charges for Inward no . " + strvoucherno + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", (strAccountId.Split(',').Contains(txtShippingAcc.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        //for credit of total expenses
                        strCompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), PaidTotalExpenses.ToString(), ref trns, Session["CompId"].ToString(), Session["LocId"].ToString());
                        CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtShippingAcc.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalExpenses.ToString(), "Shipping Charges for Inward no . " + strvoucherno + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", (strAccountId.Split(',').Contains(txtShippingAcc.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        if (ItemExpenses != 0)
                        {
                            //For Debit
                            string strCompanyCrrValueDr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), ItemExpenses.ToString(), ref trns, Session["CompId"].ToString(), Session["LocId"].ToString());
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", (PaidTotalExpenses + ItemExpenses).ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            //for credit of itemExpenses
                            string strCompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), ItemExpenses.ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtShippingAcc.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", ItemExpenses.ToString(), "Repairing Charges for Inward no . " + strvoucherno + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", (strAccountId.Split(',').Contains(txtShippingAcc.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        if (PaidTotalExpenses != 0)
                        {
                            //For Debit
                            string strCompanyCrrValueDr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), PaidTotalExpenses.ToString(), ref trns, Session["CompId"].ToString(), Session["LocId"].ToString());
                            string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtExpensesAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", (PaidTotalExpenses + ItemExpenses).ToString(), "0.00", strNarration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", (strAccountId.Split(',').Contains(txtExpensesAccount.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            // for credit of total expenses
                            string strCompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), PaidTotalExpenses.ToString(), ref trns, Session["CompId"].ToString(), Session["LocId"].ToString());
                            string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, VoucherId.ToString(), "1", txtShippingAcc.Text.Split('/')[1].ToString(), strOtherAccountNo, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalExpenses.ToString(), "Shipping Charges for Inward no . " + strvoucherno + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", (strAccountId.Split(',').Contains(txtShippingAcc.Text.Split('/')[1].ToString()) ? "False" : "True"), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }

                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            if (hdnValue.Value == "")
            {
                DisplayMessage("Record Saved", "green");
            }
            else
            {
                DisplayMessage("Record Updated", "green");
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
    public static double GetAverageCost(string strProductId, ref SqlTransaction trns)
    {
        Inv_StockDetail objStockDetail = new Inv_StockDetail(HttpContext.Current.Session["DBConnection"].ToString());
        double AverageCost = 0;
        try
        {
            AverageCost = Convert.ToDouble(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId, ref trns).Rows[0]["Field2"].ToString());
        }
        catch
        {
            AverageCost = 0;
        }
        return AverageCost;
    }
    protected string GetUnitId(string ProductId, ref SqlTransaction trns)
    {
        string strUnitId = "0";
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
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
        if (ddlFieldName.SelectedItem.Value == "Inward_Date")
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
            DataTable dtCustomInq = (DataTable)Session["dtInWardData"];
            DataView view = new DataView(dtCustomInq, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvInWard, view.ToTable(), "", "");
            Session["dtInWardData"] = view.ToTable();
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
        DataTable dt = (DataTable)Session["dtInWardData"];
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
        Session["dtInWardData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvInWard, dt, "", "");
        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvInWard.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtInWardData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvInWard, dt, "", "");
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ddlLoc.Enabled = false;
        ddlLoc.SelectedValue = e.CommandName.ToString();
        DataTable dt = objInWardheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;
            if (dt.Rows[0]["Field1"].ToString() == "Post" && objSenderID != "lnkViewDetail")
            {
                DisplayMessage("You can not edit posted record");
                return;
            }
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            hdnValue.Value = e.CommandArgument.ToString();
            txtInWardNo.Text = dt.Rows[0]["Inward_Voucher_No"].ToString();
            txtInWarddate.Text = Convert.ToDateTime(dt.Rows[0]["Inward_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSuppliername.Text = dt.Rows[0]["ManufacturerName"].ToString() + "///" + dt.Rows[0]["Manufacturur_Id"].ToString();
            FillInwardList(dt.Rows[0]["Manufacturur_Id"].ToString());
            if (dt.Rows[0]["Contact_Person"].ToString() != "0")
            {
                txtEContact.Text = dt.Rows[0]["ContactPersonName"].ToString() + "///" + dt.Rows[0]["Contact_Person"].ToString();
            }
            txtContactNo.Text = dt.Rows[0]["Contact_Person_Telephone"].ToString();
            txtEmailId.Text = dt.Rows[0]["Contact_Person_EmailId"].ToString();
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Received_By"].ToString());
            if (Emp_Code == "0" || Emp_Code == "")
            {
                DisplayMessage("Employee not exists");
                return;
            }
            txtHandledEmp.Text = dt.Rows[0]["receivedEmp"].ToString() + "/" + Emp_Code;
            if (dt.Rows[0]["Shipping_Company_Name"].ToString() != "" && dt.Rows[0]["Shipping_Company_Name"].ToString() != "0")
            {
                txtShippingLine.Text = dt.Rows[0]["ShippingLine"].ToString() + "/" + dt.Rows[0]["Shipping_Company_Name"].ToString();
            }
            if (dt.Rows[0]["Expenses_Id"].ToString() != "" && dt.Rows[0]["Expenses_Id"].ToString() != "0")
            {
                ddlExpense.SelectedValue = dt.Rows[0]["Expenses_Id"].ToString();
            }
            if (dt.Rows[0]["Shipping_Account"].ToString() != "" && dt.Rows[0]["Shipping_Account"].ToString() != "0")
            {
                txtShippingAcc.Text = dt.Rows[0]["shippingAccountname"].ToString() + "/" + dt.Rows[0]["Shipping_Account"].ToString();
            }
            if (dt.Rows[0]["Expenses_Account"].ToString() != "" && dt.Rows[0]["Expenses_Account"].ToString() != "0")
            {
                txtExpensesAccount.Text = dt.Rows[0]["expensesAccountname"].ToString() + "/" + dt.Rows[0]["Expenses_Account"].ToString();
            }
            txtpaidamount.Text = dt.Rows[0]["Shipping_Charge"].ToString();
            ddlShipBy.SelectedValue = dt.Rows[0]["Shipping_via"].ToString();
            txtAirwaybillno.Text = dt.Rows[0]["Docket_Number"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            DataTable dtDetail = objInWardDetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
            if (dtDetail.Rows.Count > 0)
            {
                dtDetail = dtDetail.DefaultView.ToTable(true, "GetPassNo", "GetPass_Id", "Item_Id", "Job_No", "Job_Ref_No", "Problem", "Status", "Verified_By", "ExpDetail", "ExpCharge");
                objPageCmn.FillData((object)GvItemDetail, dtDetail, "", "");
            }
            TabContainer2.ActiveTabIndex = 0;
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objInWardheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Field1"].ToString() == "Post")
            {
                DisplayMessage("You can not delete posted record");
                return;
            }
        }
        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        b = objInWardheader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), hdnValue.Value);
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
        ddLgetPass.Items.Clear();
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
                FillInwardList(strCustomerId);
                txtContactNo.Text = txtSuppliername.Text.Split('/')[1].ToString();
                txtEmailId.Text = txtSuppliername.Text.Split('/')[2].ToString();
                string strsql = "select name,field2,Field1,Trans_Id from Ems_ContactMaster where Status='Individual' and Company_Id=" + strCustomerId + " and  IsActive='True'";
                DataTable dt = objDa.return_DataTable(strsql);
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
    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(((TextBox)sender).Text.Split('/')[1].ToString());
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    return;
                }
                //strEmpId = ((TextBox)sender).Text.Split('/')[1].ToString();
                strEmpId = Emp_ID;
                if (((TextBox)sender).ID.Trim() == "txtHandledEmp")
                {
                    txtAssignedTo.Text = ((TextBox)sender).Text;
                }
            }
            catch
            {
                strEmpId = "0";
            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                ((TextBox)sender).Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
                return;
            }
        }
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefTo(string prefixText, int count, string contextKey)
    {
        //EmployeeMaster ObjEmp = new EmployeeMaster();
        DataTable dtCon = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and LOcation_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
       // DataTable dtMain = new DataTable();
        //dtMain = dt.Copy();
        //string filtertext = "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')";
        //DataTable dtCon = new DataView(dt, filtertext, "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
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
    public void Reset()
    {
        ddlLoc.Enabled = true;
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        txtInWardNo.Text = "";
        txtInWardNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtInWardNo.Text;
        //GetRmaItemdetail();
        txtInWarddate.Text = "";
        txtSuppliername.Text = "";
        txtEContact.Text = "";
        hdnValue.Value = "";
        txtContactNo.Text = "";
        txtEmailId.Text = "";
        txtHandledEmp.Text = "";
        txtRemarks.Text = "";
        txtShippingLine.Text = "";
        ddlExpense.SelectedIndex = 0;
        txtpaidamount.Text = "";
        txtExpensesAccount.Text = "";
        txtShippingAcc.Text = "";
        txtAirwaybillno.Text = "";
        ddLgetPass.Items.Clear();
        ResetDetailsection();
        GvItemDetail.DataSource = null;
        GvItemDetail.DataBind();
        //txtgetPassdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtInWarddate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        //AllPageCode();
    }
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        dt = objInWardheader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        if (ddlPosted.SelectedIndex == 0 || ddlPosted.SelectedIndex == 1)
        {
            dt = new DataView(dt, "Field1='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //cmn.FillData((object)GvCustomerInquiry, dt, "", "");
        GvInWard.DataSource = dt;
        GvInWard.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtInWardData"] = dt;
        //AllPageCode();
    }
    protected string GetDocumentNumber()
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "350", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
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
        if (ddlFieldName.SelectedItem.Value == "Inward_Date")
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
        hdnItemEditId.Value = e.CommandArgument.ToString();
        DataTable dt = getItemDetailinDatatable();
        dt = new DataView(dt, "GetPass_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)GvItemDetail, dt, "", "");
        ResetDetailsection();
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
            DataTable dtUName = objUnitMaster.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
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
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnitMaster.GetUnitMasterById(Session["CompId"].ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    protected void btnItemSave_Click(object sender, EventArgs e)
    {
        if (hdnItemEditId.Value.Trim() == "0")
        {
            if (ddLgetPass.SelectedIndex == 0)
            {
                DisplayMessage("Select get Pass no.");
                ddLgetPass.Focus();
                return;
            }
            if (ddlProduct.SelectedIndex == 0)
            {
                DisplayMessage("Select Product in list");
                ddlProduct.Focus();
                return;
            }
        }
        if (txtAssignedTo.Text == "")
        {
            DisplayMessage("Enter Verified by name");
            txtAssignedTo.Focus();
            return;
        }
        if (txtExpCharge.Text == "")
        {
            txtExpCharge.Text = "0";
        }
        //here we are checking that multiple record exist or not for same get pass id
        DataTable dtTemp = getItemDetailinDatatable();
        if (ddlProduct.SelectedValue == "")
        {
            return;
        }
        dtTemp = new DataView(dtTemp, "GetPass_Id=" + ddlProduct.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dtTemp.Rows.Count > 0)
        {
            DisplayMessage("Item already exists");
            ddlProduct.Focus();
            return;
        }
        DataTable dt = new DataTable();
        if (hdnItemEditId.Value.Trim() == "0")
        {
            dt.Columns.Add("GetPassNo");
            dt.Columns.Add("GetPass_Id");
            dt.Columns.Add("Item_Id");
            dt.Columns.Add("Job_No");
            dt.Columns.Add("Job_Ref_No");
            dt.Columns.Add("Problem");
            dt.Columns.Add("Status");
            dt.Columns.Add("Verified_By");
            dt.Columns.Add("ExpDetail");
            dt.Columns.Add("ExpCharge");
            if (GvItemDetail.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["GetPassNo"] = ddLgetPass.SelectedItem.Text;
                dr["GetPass_Id"] = ddlProduct.SelectedValue;
                dr["Item_Id"] = hdnItemId.Value;
                dr["Job_No"] = ddlJobNo.SelectedValue;
                dr["Job_Ref_No"] = ddlJobNo.SelectedItem.Text;
                dr["Problem"] = txtproblem.Text;
                dr["Status"] = ddlStatus.SelectedValue;
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    return;
                }
                dr["Verified_By"] = Emp_ID;
                dr["ExpDetail"] = txtExpDetail.Text;
                dr["ExpCharge"] = txtExpCharge.Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = getItemDetailinDatatable();
                DataRow dr = dt.NewRow();
                dr["GetPassNo"] = ddLgetPass.SelectedItem.Text;
                dr["GetPass_Id"] = ddlProduct.SelectedValue;
                dr["Item_Id"] = hdnItemId.Value;
                dr["Job_No"] = ddlJobNo.SelectedValue;
                dr["Job_Ref_No"] = ddlJobNo.SelectedItem.Text;
                dr["Problem"] = txtproblem.Text;
                dr["Status"] = ddlStatus.SelectedValue;
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    return;
                }
                dr["Verified_By"] = Emp_ID;
                //dr["Verified_By"] = txtAssignedTo.Text.Split('/')[1].ToString();
                dr["ExpDetail"] = txtExpDetail.Text;
                dr["ExpCharge"] = txtExpCharge.Text;
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getItemDetailinDatatable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["GetPass_Id"].ToString() == hdnItemEditId.Value.Trim())
                {
                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                    string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                    string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                    if (Emp_ID == "0" || Emp_ID == "")
                    {
                        DisplayMessage("Employee not exists");
                        return;
                    }
                    dt.Rows[i]["Verified_By"] = Emp_ID;
                    //dt.Rows[i]["Verified_By"] = txtAssignedTo.Text.Split('/')[1].ToString();
                    dt.Rows[i]["Status"] = ddlStatus.SelectedValue;
                    dt.Rows[i]["ExpDetail"] = txtExpDetail.Text;
                    dt.Rows[i]["ExpCharge"] = txtExpCharge.Text;
                    dt.Rows[i]["Problem"] = txtproblem.Text;
                }
            }
            string strCustomerId = string.Empty;
            try
            {
                strCustomerId = txtSuppliername.Text.Split('/')[3].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            FillInwardList(strCustomerId);
        }
        objPageCmn.FillData((object)GvItemDetail, dt, "", "");
        ResetDetailsection();
    }
    public void ResetDetailsection()
    {
        try
        {
            ddlProduct.SelectedIndex = 0;
        }
        catch
        {
        }
        txtproblem.Text = "";
        txtExpCharge.Text = "0";
        txtExpDetail.Text = "";
        ddlJobNo.Items.Clear();
        hdnItemEditId.Value = "0";
        hdnItemId.Value = "";
    }
    public DataTable getItemDetailinDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("GetPassNo");
        dt.Columns.Add("GetPass_Id");
        dt.Columns.Add("Item_Id");
        dt.Columns.Add("Job_No");
        dt.Columns.Add("Job_Ref_No");
        dt.Columns.Add("Problem");
        dt.Columns.Add("Status");
        dt.Columns.Add("Verified_By");
        dt.Columns.Add("ExpDetail");
        dt.Columns.Add("ExpCharge");
        foreach (GridViewRow gvrow in GvItemDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr["GetPassNo"] = ((Label)gvrow.FindControl("lblSerialNo")).Text;
            dr["GetPass_Id"] = ((Label)gvrow.FindControl("lblgetPassId")).Text;
            dr["Item_Id"] = ((Label)gvrow.FindControl("lblgvProductId")).Text;
            dr["Job_No"] = ((Label)gvrow.FindControl("lblJobId")).Text;
            dr["Job_Ref_No"] = ((Label)gvrow.FindControl("lblJobNo")).Text;
            dr["Problem"] = ((Label)gvrow.FindControl("lblgvProblem")).Text;
            dr["Status"] = ((Label)gvrow.FindControl("lblgvStatus")).Text;
            dr["Verified_By"] = ((Label)gvrow.FindControl("lblgvAssignedtoId")).Text;
            dr["ExpDetail"] = ((Label)gvrow.FindControl("lblgvExpdetail")).Text;
            dr["ExpCharge"] = ((Label)gvrow.FindControl("lblgvExpcharge")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void btnItemCancel_Click(object sender, EventArgs e)
    {
        ResetDetailsection();
    }
    protected string GetAssignedPersonName(string strEmpId)
    {
        string strEmpName = string.Empty;
        DataTable dtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), strEmpId);
        if (dtEmp.Rows.Count > 0)
        {
            strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
        }
        return strEmpName;
    }
    protected void imgBtnItemEdit_Command(object sender, CommandEventArgs e)
    {

        hdnItemEditId.Value = e.CommandArgument.ToString();
        DataTable dt = getItemDetailinDatatable();
        dt = new DataView(dt, "GetPass_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddLgetPass.Items.Clear();
            ListItem LiGetPass = new ListItem();
            LiGetPass.Text = dt.Rows[0]["GetPassNo"].ToString();
            LiGetPass.Value = dt.Rows[0]["GetPassNo"].ToString();
            ddLgetPass.Items.Insert(0, LiGetPass);
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Verified_By"].ToString());
            if (Emp_Code == "0" || Emp_Code == "")
            {
                DisplayMessage("Employee not exists");
                return;
            }
            txtAssignedTo.Text = GetAssignedPersonName(dt.Rows[0]["Verified_By"].ToString()) + "/" + Emp_Code;
            ddlProduct.Items.Clear();
            ListItem Li = new ListItem();
            Li.Text = SuggestedProductName(dt.Rows[0]["Item_Id"].ToString());
            Li.Value = dt.Rows[0]["Item_Id"].ToString();
            ddlProduct.Items.Insert(0, Li);
            ddlJobNo.Items.Clear();
            ListItem Lijob = new ListItem();
            Lijob.Text = dt.Rows[0]["Job_Ref_No"].ToString();
            Lijob.Value = dt.Rows[0]["Job_No"].ToString();
            ddlJobNo.Items.Insert(0, Lijob);
            txtproblem.Text = dt.Rows[0]["Problem"].ToString();
            ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            txtExpDetail.Text = dt.Rows[0]["ExpDetail"].ToString();
            txtExpCharge.Text = dt.Rows[0]["ExpCharge"].ToString();
            hdnItemId.Value = dt.Rows[0]["Item_Id"].ToString();
        }
    }
    public void FillInwardList(string strmanufactururId)
    {
        string strsql = "select distinct SM_GetPass_Header.Trans_Id,SM_GetPass_Header.Get_Pass_No from SM_GetPass_Header inner join  SM_GetPass_Detail on SM_GetPass_Header.Trans_Id	= SM_GetPass_Detail.Header_Id where SM_GetPass_Header.Company_Id=" + Session["CompId"].ToString() + " and SM_GetPass_Header.Brand_Id=" + Session["BrandId"].ToString() + " and SM_GetPass_Header.Location_Id=" + ddlLoc.SelectedValue + " and SM_GetPass_Header.Status='Post' and SM_GetPass_Header.Manufacturer_Id='" + strmanufactururId + "' and SM_GetPass_Detail.Status='Send'";
        DataTable dt = objDa.return_DataTable(strsql);
        objPageCmn.FillData((object)ddLgetPass, dt, "Get_Pass_No", "Trans_Id");
        ddlProduct.Items.Clear();
    }
    protected void ddLgetPass_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProduct.Items.Clear();
        if (ddLgetPass.SelectedIndex > 0)
        {
            DataTable dt = objGatepassheader.GetAllRecord_For_PendingInward(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            dt = new DataView(dt, "Header_Id=" + ddLgetPass.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            //string strsql = "select Inv_ProductMaster.EProductName,SM_GetPass_Detail.Trans_Id from SM_GetPass_Detail inner join Inv_ProductMaster on SM_GetPass_Detail.Item_Id=Inv_ProductMaster.ProductId where SM_GetPass_Detail.Header_Id=" + ddLgetPass.SelectedValue + " and Status='Send'";
            //DataTable dt = objDa.return_DataTable(strsql);
            objPageCmn.FillData((object)ddlProduct, dt, "EProductName", "Trans_Id");
        }
    }
    protected void ddlProduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtproblem.Text = "";
        ddlJobNo.Items.Clear();
        txtExpDetail.Text = "";
        txtExpCharge.Text = "";
        txtqty.Text = "0";
        ddlJobType.SelectedIndex = 0;
        if (ddlProduct.SelectedIndex > 0)
        {
            DataTable dt = objGatepassheader.GetAllRecord_For_PendingInward(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            dt = new DataView(dt, "Header_Id=" + ddLgetPass.SelectedValue + " and Trans_id=" + ddlProduct.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            // string strsql = "select SM_GetPass_Detail.Problem,SM_GetPass_Detail.Job_No as Job_Id,SM_JobCards_Header.Job_No,SM_GetPass_Detail.Item_Id	 from SM_GetPass_Detail inner join SM_JobCards_ItemDetail on  SM_JobCards_ItemDetail.trans_id=SM_GetPass_Detail.Job_No inner join SM_JobCards_Header on SM_JobCards_ItemDetail.Header_Id	= SM_JobCards_Header.Trans_Id  where SM_GetPass_Detail.Trans_Id=" + ddlProduct.SelectedValue + "";
            // DataTable dt = objDa.return_DataTable(strsql);
            txtproblem.Text = dt.Rows[0]["Problem"].ToString();
            hdnItemId.Value = dt.Rows[0]["Item_Id"].ToString();
            ddlJobType.SelectedValue = dt.Rows[0]["Type"].ToString();
            txtqty.Text = dt.Rows[0]["Qty"].ToString();
            objPageCmn.FillData((object)ddlJobNo, dt, "Job_No", "Job_Id");
            ddlJobNo.Items.RemoveAt(0);
        }
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
    #region Pending
    protected void btnPending_Click(object sender, EventArgs e)
    {
        FillGridPendingOrder();
    }
    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQSeleclField.SelectedItem.Value == "Get_Pass_Date" || ddlQSeleclField.SelectedItem.Value == "Shipping_date" || ddlQSeleclField.SelectedItem.Value == "Expected_Receive_Date")
        {
            txtQValueDate.Visible = true;
            txtQValue.Visible = false;
            txtQValue.Text = "";
            txtQValueDate.Text = "";
        }
        else
        {
            txtQValueDate.Visible = false;
            txtQValue.Visible = true;
            txtQValueDate.Text = "";
            txtQValue.Text = "";
        }
    }
    protected void ImgBtnQBind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "Get_Pass_Date" || ddlQSeleclField.SelectedItem.Value == "Shipping_date" || ddlQSeleclField.SelectedItem.Value == "Expected_Receive_Date")
        {
            if (txtQValueDate.Text != "")
            {
                try
                {
                    objSysParam.getDateForInput(txtQValueDate.Text);
                    txtQValue.Text = Convert.ToDateTime(txtQValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtQValueDate.Text = "";
                    txtQValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtQValueDate.Focus();
                txtQValue.Text = "";
                return;
            }
        }
        if (ddlQOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String)='" + txtQValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String) like '%" + txtQValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlQSeleclField.SelectedValue + ",System.String) Like '" + txtQValue.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["dtPendingOrder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSalesOrder, view.ToTable(), "", "");
            Session["dtFilterPendingOrder"] = view.ToTable();
            lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlQSeleclField.SelectedIndex = 1;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        txtQValue.Visible = true;
        txtQValueDate.Visible = false;
        FillGridPendingOrder();
    }
    protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSalesOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterPendingOrder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dt, "", "");
        //AllPageCode();
        gvSalesOrder.BottomPagerRow.Focus();
    }
    protected void gvPurchaseOrder_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilterPendingOrder"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterPendingOrder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dt, "", "");
        //AllPageCode();
        gvSalesOrder.HeaderRow.Focus();
    }
    private void FillGridPendingOrder()
    {
        DataTable dtQuotation = objGatepassheader.GetAllRecord_For_PendingInward(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtQuotation.Rows.Count + "";
        Session["dtPendingOrder"] = dtQuotation;
        Session["dtFilterPendingOrder"] = dtQuotation;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSalesOrder, dtQuotation, "", "");
    }
    #endregion
    protected void IbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/InWard", "ServiceManagement", "InWard", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected void txtExpCharge_TextChanged(object sender, EventArgs e)
    {
        int parsedValue;
        decimal parseddecimal;
        if (!int.TryParse(txtExpCharge.Text, out parsedValue))
        {
            if (!decimal.TryParse(txtExpCharge.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                txtExpCharge.Text = "";
                txtExpCharge.Focus();
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