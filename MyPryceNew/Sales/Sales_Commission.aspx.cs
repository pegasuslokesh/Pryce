using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Sales_Sales_Commission : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    Inv_SalesCommission_Header objsalesCommissionHeader = null;
    Inv_SalesCommission_Detail objsalesCommissionDetail = null;
    Inv_SalesCommission_Employee_Detail objcommEmployeeDetail = null;
    EmployeeMaster objEmployee = null;
    Inv_SalesInvoiceHeader objSinvoiceHeader = null;
    Inv_SalesInvoiceDetail objSinvoiceDetail = null;
    Ems_ContactMaster ObjContact = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_ProductMaster objProductmaster = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    DataAccessClass objDa = null;
    Set_DocNumber objDocNo = null;
    LocationMaster objLocation = null;
    UserMaster objUserMaster = null;
    CurrencyMaster objCurrency = null;
    DataAccessClass da = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_SalesCommissionConfiguration_Header objConfigHeader = null;
    Inv_SalesCommissionConfiguration_Detail objConfigDetail = null;
    Inv_ReturnCommission objReturnCommission = null;
    Inv_ParameterMaster objInvParam = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //AllPageCode();

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objsalesCommissionHeader = new Inv_SalesCommission_Header(Session["DBConnection"].ToString());
        objsalesCommissionDetail = new Inv_SalesCommission_Detail(Session["DBConnection"].ToString());
        objcommEmployeeDetail = new Inv_SalesCommission_Employee_Detail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objSinvoiceHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSinvoiceDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        ObjContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objProductmaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objUserMaster = new UserMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objConfigHeader = new Inv_SalesCommissionConfiguration_Header(Session["DBConnection"].ToString());
        objConfigDetail = new Inv_SalesCommissionConfiguration_Detail(Session["DBConnection"].ToString());
        objReturnCommission = new Inv_ReturnCommission(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/Sales_Commission.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            Calender.Format = objSys.SetDateFormat();
            CalendarFrom.Format = objSys.SetDateFormat();
            CalendarTo.Format = objSys.SetDateFormat();
            Session["dtSalesCommissionDetail"] = null;
            FillGrid();
            txtVoucherNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            txtVoucherDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
            CalendartxtValueDate.Format = objSys.SetDateFormat();
            Session["Type"] = ddlType.SelectedValue;
            FillCurrency();
            txtPaymentFromDate_CalendarExtender.Format = objSys.SetDateFormat();
            txtPaymentToDate_CalendarExtender.Format = objSys.SetDateFormat();
            CalendarExtender_txtPaymentReportFromDate.Format = objSys.SetDateFormat();
            CalendarExtender_txtPaymentReportToDate.Format = objSys.SetDateFormat();

            ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            lblSalesQuotaCurrencyName.Text = ddlCurrency.SelectedItem.Text;
            lblLocalCurrencyCode.Text = ddlCurrency.SelectedItem.Text;

            //this code is created by jitendra upadhyay to validate payment date according the inventory parameter 

            int Month = 0;
            try
            {
                Month = Convert.ToInt32(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Commission Payment Allow(In Month)").Rows[0]["ParameterValue"].ToString());

                Month = Month * (-1);
            }
            catch
            {

            }

            DateTime dt = DateTime.Now.AddMonths(Month);

            dt = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));

            //txtPaymentToDate.Text = dt.ToString(objSys.SetDateFormat());
            GetSetAccountDetail();
        }
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "325", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;

        btnPost.Visible = clsPagePermission.bAdd;
        trFinance.Visible = clsPagePermission.bAdd;

        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        gvDetail.Columns[0].Visible = clsPagePermission.bDelete;
        gvEmployee.Columns[1].Visible = clsPagePermission.bPrint;
        gvPaymentHistory.Columns[0].Visible = clsPagePermission.bPrint;

        if (ddlType.SelectedValue.Trim() != "Agent" && ddlType.SelectedValue.Trim() != "Developer")
        {
            pnldetail.Visible = true;
        }
        else
        {
            pnldetail.Visible = false;
        }

        Session["IsPayCommission"] = true;
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Code", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Voucher_Date")
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
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Voucher_Date")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    objSys.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + objSys.SetDateFormat() + "");
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
            DataTable dtCust = (DataTable)Session["Country"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Sale_comm"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvSalesCommission, view.ToTable(), "", "");

            //AllPageCode();
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void bnPayCommission_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            bool isEmployeeSelected = false;
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked)
                {
                    isEmployeeSelected = true;
                    break;
                }
            }

            if (!isEmployeeSelected)
            {
                DisplayMessage("Select Employee For commission");
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }

            if (txtDebitAccount.Text == "")
            {
                DisplayMessage("Fill Debit Account Value");
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }

            if (txtCreditAccount.Text == "")
            {
                DisplayMessage("Fill Credit Account Value");
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }


            if (editid.Value != "")
            {
                if (txttotalRemainAmount.Text == "")
                {
                    txttotalRemainAmount.Text = "0";
                }

                if (Convert.ToDouble(txttotalRemainAmount.Text) == 0)
                {
                    DisplayMessage("Remain amount should be greater than 0 !");
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    return;
                }

                DataTable dtDetail = objsalesCommissionDetail.GetRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    objcommEmployeeDetail.DeleteRecord_By_VoucherNo(dr["Trans_Id"].ToString(), ref trns);
                }

                objsalesCommissionDetail.DeleteRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);



                //For Finance Entry
                string strEmployeeName = string.Empty;
                double PaidTotalCommmissionamount = 0;
                foreach (GridViewRow gvr in gvEmployee.Rows)
                {
                    CheckBox chkSelectEmp = (CheckBox)gvr.FindControl("chkgvSelect");
                    Label lblEmployeeName = (Label)gvr.FindControl("lblProuctCode");
                    Label lblRemainPaidAmount = (Label)gvr.FindControl("lblRemain");
                    double RemainPaidAmount = Convert.ToDouble(lblRemainPaidAmount.Text);

                    if (chkSelectEmp.Checked == true)
                    {
                        if (RemainPaidAmount != 0)
                        {
                            PaidTotalCommmissionamount += RemainPaidAmount;
                            if (strEmployeeName == "")
                            {
                                strEmployeeName = lblEmployeeName.Text;
                            }
                            else
                            {
                                strEmployeeName = strEmployeeName + "," + lblEmployeeName.Text;
                            }
                        }
                    }
                }

                if (PaidTotalCommmissionamount != 0)
                {
                    //For Bank Account
                    string strAccountId = string.Empty;
                    DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount");
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

                    string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();


                    //for Voucher Number
                    //string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());

                    string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "304", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                    if (strVoucherNumber != "")
                    {
                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
                        if (dtCount.Rows.Count > 0)
                        {
                            dtCount = new DataView(dtCount, "Voucher_Type='PV'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        if (dtCount.Rows.Count == 0)
                        {
                            strVoucherNumber = strVoucherNumber + "1";
                        }
                        else
                        {
                            double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                            strVoucherNumber = strVoucherNumber + dtCount.Rows.Count;
                        }
                    }

                    objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "PV", "1/1/1800", "1/1/1800", "", "Commission for '" + txtVoucherNo.Text + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payment Voucher For Sales Commission for '" + ddlType.SelectedValue + "' which is '" + strEmployeeName + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    string strVMaxId = string.Empty;
                    DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId();
                    if (dtVMaxId.Rows.Count > 0)
                    {
                        strVMaxId = dtVMaxId.Rows[0][0].ToString();
                    }

                    //str for Employee Id
                    //For Debit
                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(txtDebitAccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtDebitAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "Commission for '" + ddlType.SelectedValue + "' which is '" + strEmployeeName + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtDebitAccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "Commission for '" + ddlType.SelectedValue + "' which is '" + strEmployeeName + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }

                    //For Credit
                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(txtCreditAccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtCreditAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "Commission for '" + ddlType.SelectedValue + "' which is '" + strEmployeeName + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtCreditAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "Commission for '" + ddlType.SelectedValue + "' which is '" + strEmployeeName + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }

                //get record from employee table
                DataTable dtEmp = new DataTable();

                dtEmp.Columns.Add("EmpId");
                dtEmp.Columns.Add("Is_Paid", typeof(bool));

                foreach (GridViewRow gvrow in gvEmployee.Rows)
                {
                    if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked && Convert.ToDouble(((Label)gvrow.FindControl("lblRemain")).Text) > 0)
                    {
                        DataRow dr = dtEmp.NewRow();

                        dr[0] = ((Label)gvrow.FindControl("lblEmpId")).Text;
                        dr[1] = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
                        dtEmp.Rows.Add(dr);
                    }
                }

                foreach (GridViewRow gvrow in gvDetail.Rows)
                {
                    string strPaiddate = string.Empty;
                    int voucherId = 0;
                    int counter = 0;

                    //if is received is true and is paid is false then 



                    voucherId = objsalesCommissionDetail.InsertRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblInvoiceNo")).Text, objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString(), ((Label)gvrow.FindControl("lblCustomerId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblforeignAmt")).Text, ((TextBox)gvrow.FindControl("txtper")).Text, ((TextBox)gvrow.FindControl("lblcommamt")).Text, ((Label)gvrow.FindControl("lblisreceived")).Text, true.ToString(), DateTime.Now.ToString(), ddlType.SelectedValue, ((Label)gvrow.FindControl("lblAmt")).Text, ((Label)gvrow.FindControl("lblisreturn")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //}

                    //for insert record in child table
                    GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));

                    foreach (GridViewRow gvr in gvchild.Rows)
                    {

                        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text) && !Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text))
                        {
                            SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString());
                            if (new DataView(dtEmp, "EmpId=" + ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), true.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                if (((Label)gvr.FindControl("lblpaiddate")).Text == "")
                                {
                                    strPaiddate = "1/1/1900";
                                }
                                else
                                {
                                    strPaiddate = ((Label)gvr.FindControl("lblpaiddate")).Text;
                                }
                                objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), ((Label)gvr.FindControl("lblispaid")).Text, strPaiddate, ref trns);
                            }
                        }
                        else
                        {
                            if (((Label)gvr.FindControl("lblpaiddate")).Text == "")
                            {
                                strPaiddate = "1/1/1900";
                            }
                            else
                            {
                                strPaiddate = ((Label)gvr.FindControl("lblpaiddate")).Text;
                            }
                            objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), ((Label)gvr.FindControl("lblispaid")).Text, strPaiddate, ref trns);
                        }
                    }
                }

                dtDetail = objsalesCommissionDetail.GetRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                dtDetail.Columns["Field2"].ColumnName = "LocalAmount";
                dtDetail.Columns["Field3"].ColumnName = "Is_Return";
                dtDetail.Columns["Field4"].ColumnName = "CurrencyId";
                dtDetail = dtDetail.DefaultView.ToTable(true, "Trans_Id", "Invoice_Id", "Invoice_No", "Invoice_Date", "Customer_Id", "Name", "Product_Id", "ProductCode", "EProductName", "Amount", "Comission_Percentage", "Comission_Amount", "Is_Receive", "Is_Paid", "Paid_Date", "LocalAmount", "Is_Return", "CurrencyId");

                Session["dtSalesCommissionDetail"] = dtDetail;
                objPageCmn.FillData((object)gvDetail, dtDetail, "", "");
                foreach (GridViewRow gvr in gvDetail.Rows)
                {
                    GridView gvchild = (GridView)gvr.FindControl("gvEmp");

                    DataTable dtchild = objcommEmployeeDetail.GetRecord_By_VoucherNo(((Label)gvr.FindControl("lblTransId")).Text, ref trns);

                    dtchild = dtchild.DefaultView.ToTable(true, "Trans_Id", "Commission_Person", "Commission_Percentage", "Is_Paid", "Paid_Date");

                    gvchild.DataSource = dtchild;
                    gvchild.DataBind();
                }
                GetGridTotal();
                //updatre header amount
                string sql = "update dbo.Inv_SalesCommission_Header set Total_Sales_Amount=" + txttotalsalesAmount.Text + ",Total_Received_Amount=" + txttotalReceivedAmount.Text + ",Total_Paid_Amount=" + txttotalPaidAmount.Text + ",Total_Remain_Amount=" + txttotalRemainAmount.Text + ",Total_Comission_Amount=" + txtNetCommission.Text + ",Field2=" + txttotalreturmCommission.Text + ",Field3=" + txtTotalActualCommission.Text + " where Trans_Id=" + editid.Value + "";
                objDa.execute_Command(sql, ref trns);
            }
            DisplayMessage("Commission paid successfully");

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            getpaymentHistory(editid.Value);
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
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
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
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
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
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        chkPost.Checked = true;
        btnSave_Click(sender, e);
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        //here we chekc validation before insert
        Button btn = (Button)sender;

        if (btn.ID.Trim() == "btnPost")
        {
            chkPost.Checked = true;
        }
        else
        {
            chkPost.Checked = false;
        }



        if (txtVoucherNo.Text == "")
        {
            DisplayMessage("Enter Voucher No !");
            txtVoucherNo.Focus();
            return;
        }

        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Enter Voucher date !");
            txtVoucherDate.Focus();

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtVoucherDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid voucher date !");
                txtVoucherDate.Focus();

                return;
            }
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        if (txtfromdate.Text == "")
        {
            DisplayMessage("Enter From date !");
            txtfromdate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtfromdate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid from date !");
                txtfromdate.Focus();

                return;
            }
        }

        if (txttodate.Text == "")
        {
            DisplayMessage("Enter to date !");
            txttodate.Focus();

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txttodate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid to date !");
                txttodate.Focus();

                return;
            }
        }

        //here we chekc that form date shoule begreater then to date 


        if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
        {
            DisplayMessage("from date should be less than to date !");
            txtfromdate.Focus();

            return;
        }



        string Strsalesperson = string.Empty;


        if (ddlType.SelectedValue.Trim() != "Agent" && ddlType.SelectedValue.Trim() != "Developer")
        {
            if (txtSalesPerson.Text.Trim() == "")
            {
                DisplayMessage("Enter Sales person !");
                txtSalesPerson.Focus();

                return;
            }
            else
            {
                try
                {
                    Strsalesperson = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
                }
                catch
                {
                    DisplayMessage("Employee not found !");
                    txtSalesPerson.Focus();

                    return;
                }

            }
        }
        else
        {
            Strsalesperson = "0";
        }

        if (txttotalsalesAmount.Text == "")
        {
            txttotalsalesAmount.Text = "0";
        }
        if (txttotalReceivedAmount.Text == "")
        {
            txttotalReceivedAmount.Text = "0";
        }
        if (txttotalPaidAmount.Text == "")
        {
            txttotalPaidAmount.Text = "0";
        }
        if (txttotalRemainAmount.Text == "")
        {
            txttotalRemainAmount.Text = "0";
        }
        if (txtNetCommission.Text == "")
        {
            txtNetCommission.Text = "0";
        }
        if (txtUnpaidbalance.Text == "")
        {
            txtUnpaidbalance.Text = "0";
        }
        if (txttotalreturmCommission.Text == "")
        {
            txttotalreturmCommission.Text = "0";
        }
        if (txtTotalActualCommission.Text == "")
        {
            txtTotalActualCommission.Text = "0";
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        try
        {
            int b = 0;

            if (editid.Value == "")
            {
                b = objsalesCommissionHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtVoucherNo.Text, objSys.getDateForInput(txtVoucherDate.Text).ToString(), objSys.getDateForInput(txtfromdate.Text).ToString(), objSys.getDateForInput(txttodate.Text).ToString(), Strsalesperson, "0", txttotalsalesAmount.Text, txttotalReceivedAmount.Text, txttotalPaidAmount.Text, txttotalRemainAmount.Text, txtNetCommission.Text, chkPost.Checked.ToString(), txtremarks.Text, ddlType.SelectedValue, txtUnpaidbalance.Text, txttotalreturmCommission.Text, txtTotalActualCommission.Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {

                        DataTable dtCount = objsalesCommissionHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);


                        if (dtCount.Rows.Count == 0)
                        {
                            objsalesCommissionHeader.Updatecode(b.ToString(), txtVoucherNo.Text + "1", ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + "1";

                        }
                        else
                        {
                            DataTable dtCount1 = new DataView(dtCount, "Voucher_No='" + txtVoucherNo.Text + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
                            int NoRow = dtCount.Rows.Count;
                            if (dtCount1.Rows.Count > 0)
                            {

                                bool bCodeFlag = true;
                                while (bCodeFlag)
                                {
                                    NoRow += 1;
                                    DataTable dtTemp = new DataView(dtCount, "Voucher_No='" + txtVoucherNo.Text + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtTemp.Rows.Count == 0)
                                    {
                                        bCodeFlag = false;
                                    }
                                }
                            }

                            objsalesCommissionHeader.Updatecode(b.ToString(), txtVoucherNo.Text + NoRow.ToString(), ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + NoRow.ToString();
                        }
                    }
                    //here we insert record in detail table

                    foreach (GridViewRow gvrow in gvDetail.Rows)
                    {
                        string strPaiddate = string.Empty;

                        int voucherId = objsalesCommissionDetail.InsertRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), b.ToString(), ((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblInvoiceNo")).Text, objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString(), ((Label)gvrow.FindControl("lblCustomerId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblforeignAmt")).Text, ((TextBox)gvrow.FindControl("txtper")).Text, ((TextBox)gvrow.FindControl("lblcommamt")).Text, ((Label)gvrow.FindControl("lblisreceived")).Text, true.ToString(), DateTime.Now.ToString(), ddlType.SelectedValue, ((Label)gvrow.FindControl("lblAmt")).Text, ((Label)gvrow.FindControl("lblisreturn")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, ((Label)gvrow.FindControl("lblProjectId")).Text, false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        //for insert record in child table
                        GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));

                        foreach (GridViewRow gvr in gvchild.Rows)
                        {
                            string strcommissionPaiddate = string.Empty;

                            if (Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text))
                            {
                                strcommissionPaiddate = ((Label)gvr.FindControl("lblpaiddate")).Text;
                            }
                            else
                            {
                                strcommissionPaiddate = "1/1/1900 12:00:00 AM";
                            }
                            objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text).ToString(), strcommissionPaiddate, ref trns);
                        }
                    }
                    DisplayMessage("Record Saved", "green");
                    //FillGrid();
                    //Reset();
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
            else
            {
                objsalesCommissionHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtVoucherNo.Text, objSys.getDateForInput(txtVoucherDate.Text).ToString(), objSys.getDateForInput(txtfromdate.Text).ToString(), objSys.getDateForInput(txttodate.Text).ToString(), Strsalesperson, "0", txttotalsalesAmount.Text, txttotalReceivedAmount.Text, txttotalPaidAmount.Text, txttotalRemainAmount.Text, txtNetCommission.Text, chkPost.Checked.ToString(), txtremarks.Text, ddlType.SelectedValue, txtUnpaidbalance.Text, txttotalreturmCommission.Text, txtTotalActualCommission.Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //here we insert record in detail table
                //first we dete and reinsert
                //here we insert record in detail table

                //delete record  from child table 
                DataTable dtDetail = objsalesCommissionDetail.GetRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    objcommEmployeeDetail.DeleteRecord_By_VoucherNo(dr["Trans_Id"].ToString(), ref trns);
                }
                objsalesCommissionDetail.DeleteRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                foreach (GridViewRow gvrow in gvDetail.Rows)
                {
                    string strPaiddate = string.Empty;


                    int voucherId = objsalesCommissionDetail.InsertRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblInvoiceNo")).Text, objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString(), ((Label)gvrow.FindControl("lblCustomerId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblforeignAmt")).Text, ((TextBox)gvrow.FindControl("txtper")).Text, ((TextBox)gvrow.FindControl("lblcommamt")).Text, ((Label)gvrow.FindControl("lblisreceived")).Text, true.ToString(), DateTime.Now.ToString(), ddlType.SelectedValue, ((Label)gvrow.FindControl("lblAmt")).Text, ((Label)gvrow.FindControl("lblisreturn")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, ((Label)gvrow.FindControl("lblProjectId")).Text, false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                    //for insert record in child table
                    GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));

                    foreach (GridViewRow gvr in gvchild.Rows)
                    {

                        string strcommissionPaiddate = string.Empty;

                        if (Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text))
                        {
                            strcommissionPaiddate = ((Label)gvr.FindControl("lblpaiddate")).Text;
                        }
                        else
                        {
                            strcommissionPaiddate = "1/1/1900 12:00:00 AM";
                        }
                        objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text).ToString(), strcommissionPaiddate, ref trns);

                        //here we check that any return exist or not again this invoice if found than insert in return commision table 
                        if (chkPost.Checked)
                        {

                            double ReturnAmount = 0;
                            //code modified by jitendra upadhyay for get return amount without tax
                            //code modified on 12-july-2018

                            // string strsql = "select ((Inv_SalesReturnDetail.UnitPrice*Inv_SalesReturnDetail.ReturnQty)-Inv_SalesReturnDetail.DiscountV+Inv_SalesReturnDetail.TaxV) as TotalReturmAmount,Inv_SalesReturnHeader.Trans_Id from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No where Inv_SalesReturnHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesReturnHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesReturnHeader.Location_Id=" + Session["LocId"].ToString() + " and Inv_SalesReturnHeader.Post='True'  and Inv_SalesReturnDetail.Invoice_No=" + ((Label)gvrow.FindControl("lblInvoiceId")).Text + " and Inv_SalesReturnDetail.Product_Id=" + ((Label)gvrow.FindControl("lblproductId")).Text + " order by Inv_SalesReturnHeader.Trans_Id desc";
                            string strsql = "select ((Inv_SalesReturnDetail.UnitPrice*Inv_SalesReturnDetail.ReturnQty)-Inv_SalesReturnDetail.DiscountV) as TotalReturmAmount,Inv_SalesReturnHeader.Trans_Id from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No where Inv_SalesReturnHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesReturnHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesReturnHeader.Location_Id=" + Session["LocId"].ToString() + " and Inv_SalesReturnHeader.Post='True'  and Inv_SalesReturnDetail.Invoice_No=" + ((Label)gvrow.FindControl("lblInvoiceId")).Text + " and Inv_SalesReturnDetail.Product_Id=" + ((Label)gvrow.FindControl("lblproductId")).Text + " order by Inv_SalesReturnHeader.Trans_Id desc";
                            DataTable dt = objDa.return_DataTable(strsql, ref trns);


                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    ReturnAmount = Convert.ToDouble(dr[0].ToString());
                                }
                                catch
                                {

                                }
                                if (ReturnAmount > 0)
                                {
                                    //double Exchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(((Label)gvrow.FindControl("lblgvCurrencyId")).Text, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString(), ref trns));

                                    string strExchnagerate = "";
                                    string strFromCurrency = ((Label)gvrow.FindControl("lblgvCurrencyId")).Text;
                                    string strToCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
                                    string strBaseCurrency = objSys.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString();


                                    if (strBaseCurrency == strFromCurrency)
                                    {
                                        strExchnagerate = objCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString();

                                    }
                                    else
                                    {
                                        strExchnagerate = ((1 / float.Parse(objCurrency.GetCurrencyMasterById(strFromCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(objCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();
                                    }
                                    double Exchnagerate = Convert.ToDouble(strExchnagerate);
                                    //double Exchnagerate = Convert.ToDouble(objSys.GetExchageRate1(((Label)gvrow.FindControl("lblgvCurrencyId")).Text, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString(), ref trns));



                                    double ProductReturmAmount = Convert.ToDouble(ReturnAmount) * Exchnagerate;
                                    double CommisionPercentage = Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text);
                                    double ReturnCommisionAmt = (ProductReturmAmount * CommisionPercentage) / 100;


                                    objReturnCommission.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, editid.Value, voucherId.ToString(), dr[1].ToString(), ReturnCommisionAmt.ToString(), false.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                    }
                }
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);
        //AllPageCode();
    }
    public void GetSetAccountDetail()
    {

        txtpaymentdebitaccount.Text = "";
        txtpaymentCreditaccount.Text = "";
        txtDebitAccount.Text = "";
        txtCreditAccount.Text = "";
        string strAccountId = string.Empty;
        string strAccountName = string.Empty;


        strAccountId = objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Commission Debit");

        if (strAccountId.Trim() != "False")
        {
            txtpaymentdebitaccount.Text = GetAccountNamebyTransId(strAccountId) + "/" + strAccountId;

        }

        strAccountId = objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Commission Credit");

        if (strAccountId.Trim() != "False")
        {
            txtpaymentCreditaccount.Text = GetAccountNamebyTransId(strAccountId) + "/" + strAccountId;

        }


        if (ddlType.SelectedValue.Trim() == "Agent")
        {
            strAccountId = objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Agent Commission Debit");

            if (strAccountId.Trim() != "False")
            {
                txtDebitAccount.Text = GetAccountNamebyTransId(strAccountId) + "/" + strAccountId;
            }

            strAccountId = objAccParameterLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Agent Commission Credit");

            if (strAccountId.Trim() != "False")
            {
                txtCreditAccount.Text = GetAccountNamebyTransId(strAccountId) + "/" + strAccountId;
            }

        }
        else
        {
            txtDebitAccount.Text = txtpaymentdebitaccount.Text;
            txtCreditAccount.Text = txtpaymentCreditaccount.Text;
        }
    }

    public string GetAccountNamebyTransId(string strAccountno)
    {
        string strAccountName = string.Empty;
        DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountno);
        if (dtAcc.Rows.Count > 0)
        {
            strAccountName = dtAcc.Rows[0]["AccountName"].ToString();

        }
        return strAccountName;

    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        btnPayCommission.Visible = false;
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            if (Convert.ToBoolean(((Label)gvrow.FindControl("lblPostStatus")).Text) && ((Label)gvrow.FindControl("lblType")).Text.Trim() == "Agent" && Convert.ToBoolean(Session["IsPayCommission"].ToString()))
            {
                btnPayCommission.Visible = true;
            }

        }
        else
        {
            if (Convert.ToBoolean(((Label)gvrow.FindControl("lblPostStatus")).Text))
            {

                DisplayMessage("Record is posted , you can not edit !");
                return;
            }
            else
            {

            }

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

        //this code is updated by jitendra upadhyay 
        //for set visible true to pay commisison button when record posted otherwise it will be false
        //code updated on 06-09-2016

        if (Lbl_Tab_New.Text == Resources.Attendance.New || Lbl_Tab_New.Text == Resources.Attendance.Edit)
        {
            btnSave.Visible = true;

        }
        else
        {
            btnSave.Visible = false;

        }
        if (Lbl_Tab_New.Text == Resources.Attendance.Edit)
        {
            btnPost.Visible = true;
        }
        else
        {
            btnPost.Visible = false;
        }

        if (Lbl_Tab_New.Text == Resources.Attendance.Edit || Lbl_Tab_New.Text == Resources.Attendance.View)
        {

            trFinance.Visible = true;
        }
        else
        {
            trFinance.Visible = false;
        }

        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objsalesCommissionHeader.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            //For Finance Entry


            txtVoucherNo.Text = dt.Rows[0]["Voucher_No"].ToString();
            ddlType.SelectedValue = dt.Rows[0]["Field1"].ToString();
            GetSetAccountDetail();
            ddlType.Enabled = false;
            Session["Type"] = ddlType.SelectedValue;
            txtVoucherDate.Text = Convert.ToDateTime(dt.Rows[0]["Voucher_Date"].ToString()).ToString(objSys.SetDateFormat());
            Session["DtFromDate"] = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).ToString(objSys.SetDateFormat());
            Session["DToDate"] = Convert.ToDateTime(dt.Rows[0]["To_Date"].ToString()).ToString(objSys.SetDateFormat());
            txtfromdate.Text = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).ToString(objSys.SetDateFormat());
            txttodate.Text = Convert.ToDateTime(dt.Rows[0]["To_Date"].ToString()).ToString(objSys.SetDateFormat());
            if (ddlType.SelectedValue.Trim() != "Agent" && ddlType.SelectedValue.Trim() != "Developer")
            {
                txtSalesPerson.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Emp_Id"].ToString());
                Div_Sales_Person.Visible = true;
            }
            else
            {
                Div_Sales_Person.Visible = false;
            }
            txtremarks.Text = dt.Rows[0]["Remark"].ToString();
            txttotalsalesAmount.Text = SetDecimal(dt.Rows[0]["Total_Sales_Amount"].ToString());
            txttotalReceivedAmount.Text = SetDecimal(dt.Rows[0]["Total_Received_Amount"].ToString());
            txttotalPaidAmount.Text = SetDecimal(dt.Rows[0]["Total_Paid_Amount"].ToString());
            txttotalRemainAmount.Text = SetDecimal(dt.Rows[0]["Total_Remain_Amount"].ToString());
            txtNetCommission.Text = SetDecimal(dt.Rows[0]["Total_Comission_Amount"].ToString());
            txtUnpaidbalance.Text = SetDecimal(dt.Rows[0]["Field2"].ToString());

            //get detail record

            DataTable dtDetail = objsalesCommissionDetail.GetRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value);


            dtDetail.Columns["Field2"].ColumnName = "LocalAmount";
            dtDetail.Columns["Field3"].ColumnName = "Is_Return";
            dtDetail.Columns["Field4"].ColumnName = "CurrencyId";
            dtDetail.Columns["Field5"].ColumnName = "projectid";
            dtDetail = dtDetail.DefaultView.ToTable(true, "Trans_Id", "Invoice_Id", "Invoice_No", "Invoice_Date", "Customer_Id", "Name", "Product_Id", "ProductCode", "EProductName", "Amount", "Comission_Percentage", "Comission_Amount", "Is_Receive", "Is_Paid", "Paid_Date", "AgentId", "Is_Return", "LocalAmount", "CurrencyId", "projectid");

            if (dtDetail.Rows.Count > 0)
            {


                dtDetail = new DataView(dtDetail, "", "Invoice_Date asc", DataViewRowState.CurrentRows).ToTable();
                Session["dtSalesCommissionDetail"] = dtDetail;

                objPageCmn.FillData((object)gvDetail, dtDetail, "", "");


                //also we create dattable for check payment paid history 

                getpaymentHistory(editid.Value);

                //for bind record in child table

                foreach (GridViewRow gvr in gvDetail.Rows)
                {

                    if (ddlType.SelectedValue.Trim() == "Agent")
                    {
                        ((LinkButton)gvr.FindControl("lnkAddEmp")).Visible = false;
                    }
                    else
                    {
                        ((LinkButton)gvr.FindControl("lnkAddEmp")).Visible = true;
                    }
                    GridView gvchild = (GridView)gvr.FindControl("gvEmp");

                    DataTable dtchild = objcommEmployeeDetail.GetRecord_By_VoucherNo(((Label)gvr.FindControl("lblTransId")).Text);

                    dtchild = dtchild.DefaultView.ToTable(true, "Trans_Id", "Commission_Person", "Commission_Percentage", "Is_Paid", "Paid_Date");

                    gvchild.DataSource = dtchild;
                    gvchild.DataBind();

                }

                GetGridTotal();
            }

            //here we update record in headertable forget updated calculation

            objsalesCommissionHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtVoucherNo.Text, objSys.getDateForInput(txtVoucherDate.Text).ToString(), objSys.getDateForInput(txtfromdate.Text).ToString(), objSys.getDateForInput(txttodate.Text).ToString(), dt.Rows[0]["Emp_Id"].ToString(), "0", txttotalsalesAmount.Text, txttotalReceivedAmount.Text, txttotalPaidAmount.Text, txttotalRemainAmount.Text, txtNetCommission.Text, dt.Rows[0]["Post"].ToString(), txtremarks.Text, ddlType.SelectedValue, txtUnpaidbalance.Text, txttotalreturmCommission.Text, txtTotalActualCommission.Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objsalesCommissionHeader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            DisplayMessage("Record Deleted");

            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        try
        {
            gvSalesCommission.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
    }
    protected void gvSalesCommission_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSalesCommission.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Sale_comm"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvSalesCommission, dt, "", "");
        //AllPageCode();
        gvSalesCommission.HeaderRow.Focus();

    }
    protected void gvSalesCommission_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Sale_comm"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Sale_comm"] = dt;
        gvSalesCommission.DataSource = dt;
        gvSalesCommission.DataBind();
        //AllPageCode();
        gvSalesCommission.HeaderRow.Focus();
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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid()
    {
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "True")
        {
            PostStatus = " Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "False")
        {
            PostStatus = " Post='False'";
        }
        DataTable dt = new DataView(objsalesCommissionHeader.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        //here we showing record according is stand alone status
        //for showing record for only login employee

        //code added by jitendra on 08-03-2016
        //code start
        bool IsSingleUser = false;

        DataTable dtUser = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        try
        {
            IsSingleUser = Convert.ToBoolean(dtUser.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }

        if (IsSingleUser)
        {

            dt = new DataView(dt, "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        //code end 


        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvSalesCommission, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Sale_comm"] = dt;
        Session["Country"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void Reset()
    {
        txtVoucherNo.Text = "";
        txtVoucherDate.Text = "";
        txtVoucherDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        txtfromdate.Text = "";
        txttodate.Text = "";
        txtSalesPerson.Text = "";
        txtremarks.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        Session["dtSalesCommissionDetail"] = null;
        objPageCmn.FillData((object)gvDetail, null, "", "");
        txtVoucherNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        ResetDetailsection();
        txttotalsalesAmount.Text = "0";
        txttotalReceivedAmount.Text = "0";
        txttotalPaidAmount.Text = "0";
        txttotalRemainAmount.Text = "0";
        txtNetCommission.Text = "0";
        txtUnpaidbalance.Text = "0";
        chkPost.Checked = false;
        Session["SalesPersonId"] = null;
        Session["DtFromDate"] = null;
        Session["DToDate"] = null;
        ddlType.Enabled = true;
        ddlType.SelectedIndex = 0;
        //AllPageCode();
        gvEmployee.DataSource = null;
        gvEmployee.DataBind();
        gvPaymentHistory.DataSource = null;
        gvPaymentHistory.DataBind();
        btnPayCommission.Visible = false;
        Div_Sales_Person.Visible = true;
        GetSetAccountDetail();
    }
    protected void txtfromdate_OnTextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtfromdate.Text);
                Session["DtFromDate"] = txtfromdate.Text;
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                return;
            }
        }
    }
    protected void txttodate_OnTextChanged(object sender, EventArgs e)
    {
        if (txttodate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txttodate.Text);
                Session["DToDate"] = txttodate.Text;
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                return;
            }
        }
    }
    protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Type"] = ddlType.SelectedValue;
        txtSalesPerson.Text = "";
        //if (ddlType.SelectedValue == "Agent" || ddlType.SelectedValue == "Developer")
        //{
        //    Div_Sales_Person.Visible = false;
        //}
        //else
        //{
        //    Div_Sales_Person.Visible = true;
        //}

        GetSetAccountDetail();

        //AllPageCode();
    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {

        TextBox txtCommPerson = (TextBox)sender;
        if (txtfromdate.Text != "" && txttodate.Text != "")
        {
            string strEmployeeId = string.Empty;
            if (txtSalesPerson.Text != "")
            {
                strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtCommPerson.Text.Split('/')[1].ToString());
                if (strEmployeeId != "" && strEmployeeId != "0")
                {
                    Session["DtFromDate"] = txtfromdate.Text;
                    Session["DToDate"] = txttodate.Text;
                    Session["SalesPersonId"] = txtCommPerson.Text.Split('/')[0].ToString();
                }
                else
                {
                    DisplayMessage("Select Employee In Suggestions Only");
                    txtCommPerson.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCommPerson);
                    Session["SalesPersonId"] = null;
                }
            }
        }
        else
        {
            DisplayMessage("Enter From Date and to date");
            txtfromdate.Focus();
            Session["DtFromDate"] = null;
            Session["DToDate"] = null;
            return;
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
                if (dtEmp.Rows.Count == 0)
                { retval = ""; }
            }
            else
            { retval = ""; }
        }
        else
        { retval = ""; }
        return retval;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        UserMaster objUserMaster = new UserMaster(HttpContext.Current.Session["DBConnection"].ToString());
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        //if (Convert.ToBoolean(objUserMaster.GetUserMasterByUserId(HttpContext.Current.Session["UserId"].ToString(), "0").Rows[0]["Field6"].ToString()))
        //{
        //    dt1 = new DataView(dt1, "Emp_Id=" + HttpContext.Current.Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //}
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

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
    #region DetailSection

    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;


        DataTable dt = objProductmaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    protected string GeDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    protected void ddlProduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlProduct.SelectedIndex != 0)
        {
            DataTable dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), hdnInvoiceId.Value, Session["FinanceYearId"].ToString());

            dtDetail = new DataView(dtDetail, "Product_Id=" + ddlProduct.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();


            //here we set return flag in according return qty

            hdnReturnFlag.Value = "False";

            if (dtDetail.Rows[0]["ReturnQty"] != null)
            {
                try
                {
                    if (Convert.ToDouble(dtDetail.Rows[0]["ReturnQty"].ToString()) > 0)
                    {

                        hdnReturnFlag.Value = "True";
                    }
                }
                catch
                {

                }
            }

            //here we get local amount

            //code modified by jitendra upadhyay because commission amount was coming including tax amount

            //code modified on 12-07-2018
            txtLocalAmount.Text = getLocalAmount(((Convert.ToDouble(dtDetail.Rows[0]["UnitPrice"].ToString()) - Convert.ToDouble(dtDetail.Rows[0]["DiscountV"].ToString())) * Convert.ToDouble(dtDetail.Rows[0]["Quantity"].ToString())).ToString(), ddlCurrency.SelectedValue);
            txtAmount.Text = ((Convert.ToDouble(dtDetail.Rows[0]["UnitPrice"].ToString()) - Convert.ToDouble(dtDetail.Rows[0]["DiscountV"].ToString())) * Convert.ToDouble(dtDetail.Rows[0]["Quantity"].ToString())).ToString();


            // txtLocalAmount.Text = getLocalAmount(((Convert.ToDouble(dtDetail.Rows[0]["UnitPrice"].ToString()) + Convert.ToDouble(dtDetail.Rows[0]["TaxV"].ToString()) - Convert.ToDouble(dtDetail.Rows[0]["DiscountV"].ToString())) * Convert.ToDouble(dtDetail.Rows[0]["Quantity"].ToString())).ToString(), ddlCurrency.SelectedValue);


            // txtAmount.Text = ((Convert.ToDouble(dtDetail.Rows[0]["UnitPrice"].ToString()) + Convert.ToDouble(dtDetail.Rows[0]["TaxV"].ToString()) - Convert.ToDouble(dtDetail.Rows[0]["DiscountV"].ToString())) * Convert.ToDouble(dtDetail.Rows[0]["Quantity"].ToString())).ToString();


            DataTable dtProduct = objProductmaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlProduct.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString());



            if (ddlType.SelectedValue == "Sales")
            {
                txtcommpercentage.Text = dtProduct.Rows[0]["Field4"].ToString();
            }
            else if (ddlType.SelectedValue == "Technical")
            {
                txtcommpercentage.Text = dtProduct.Rows[0]["Field5"].ToString();
            }
            if (txtcommpercentage.Text == "")
            {
                txtcommpercentage.Text = "0";
                txttotalCommission.Text = "0";
            }
            else
            {
                if (txtLocalAmount.Text == "")
                {
                    txtLocalAmount.Text = "0";
                }

                txttotalCommission.Text = SetDecimal((Convert.ToDouble(txtLocalAmount.Text) * (Convert.ToDouble(txtcommpercentage.Text) / 100)).ToString());
                txtcommpercentage.Text = SetDecimal(txtcommpercentage.Text);
            }
        }
    }

    protected void txtcommpercentage_OnTextChanged(object sender, EventArgs e)
    {

        if (txtLocalAmount.Text == "")
        {
            txtLocalAmount.Text = "0";
        }


        //if (Convert.ToDouble(txtLocalAmount.Text) <= 0)
        //{
        //    DisplayMessage("Product Amount should be greater than 0 ");
        //    txtcommpercentage.Text = "0.000";
        //    txttotalCommission.Text = "0.000";
        //    return;
        //}

        if (txtcommpercentage.Text == "")
        {
            txtcommpercentage.Text = "0";
        }

        txttotalCommission.Text = SetDecimal((Convert.ToDouble(txtLocalAmount.Text) * (Convert.ToDouble(txtcommpercentage.Text) / 100)).ToString());
        txtcommpercentage.Text = SetDecimal(txtcommpercentage.Text);

    }

    protected void txttotalCommission_OnTextChanged(object sender, EventArgs e)
    {
        if (txtLocalAmount.Text == "")
        {
            txtLocalAmount.Text = "0";
        }
        if (Convert.ToDouble(txtLocalAmount.Text) <= 0)
        {
            DisplayMessage("Product Amount should be greater than 0 ");
            txtcommpercentage.Text = "0.000";
            txttotalCommission.Text = "0.000";
            return;
        }
        if (txttotalCommission.Text == "")
        {
            txttotalCommission.Text = "0";
        }
        txtcommpercentage.Text = SetDecimal(((Convert.ToDouble(txttotalCommission.Text) * 100) / Convert.ToDouble(txtLocalAmount.Text)).ToString());
        txttotalCommission.Text = SetDecimal(txttotalCommission.Text);
    }

    protected void txtsalesinvoice_OnTextChanged(object sender, EventArgs e)
    {

        if (txtsalesinvoice.Text != "")
        {

            DataTable dtdetail = objSinvoiceHeader.GetCommissionRecordForAddManual(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["Type"].ToString());

            dtdetail = new DataView(dtdetail, "Invoice_No='" + txtsalesinvoice.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtdetail.Rows.Count == 0)
            {
                DisplayMessage("Select invoice in suggestion only");
                ResetDetailsection();
                return;
            }

            DataTable dtInvoice = objSinvoiceHeader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), txtsalesinvoice.Text.Trim());

            if (dtInvoice.Rows.Count > 0)
            {
                txtInvoicedate.Text = Convert.ToDateTime(dtInvoice.Rows[0]["Invoice_Date"].ToString()).ToString(objSys.SetDateFormat());

                txtcutomerName.Text = dtInvoice.Rows[0]["CustomerName"].ToString() + "/" + dtInvoice.Rows[0]["Supplier_Id"].ToString();

                hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();

                ddlCurrency.SelectedValue = dtInvoice.Rows[0]["Currency_Id"].ToString();

                DataTable dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dtInvoice.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString());


                objPageCmn.FillData((object)ddlProduct, dtDetail, "ProductName", "Product_Id");

                txtAmount.Text = "0";
                txtcommpercentage.Text = "0";
                txttotalCommission.Text = "0";
            }
            else
            {
                ResetDetailsection();
            }
        }
        else
        {
            ResetDetailsection();
        }
    }

    public void ResetDetailsection()
    {
        txtsalesinvoice.Text = "";
        txtInvoicedate.Text = "";
        txtcutomerName.Text = "";
        ddlProduct.Items.Clear();
        txtAmount.Text = "";
        txtcommpercentage.Text = "";
        txttotalCommission.Text = "";

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        string[] str = new string[0];
        if (HttpContext.Current.Session["DtFromDate"] != null && HttpContext.Current.Session["DToDate"] != null)
        {

            dt = objSinvoiceHeader.GetCommissionRecordForAddManual(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["Type"].ToString());

            DateTime ToDate = Convert.ToDateTime(HttpContext.Current.Session["DToDate"].ToString());
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                dt = new DataView(dt, "Invoice_Date>='" + HttpContext.Current.Session["DtFromDate"].ToString() + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            dt = new DataView(dt, "Invoice_No like '%" + prefixText + "%' ", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Invoice_No"].ToString();
                }
            }
        }
        return str;
    }
    protected void IbtnAddCommission_Click(object sender, object e)
    {

        //if (txtSalesPerson.Text == "")
        //{
        //    DisplayMessage("Enter Sales Person");
        //    txtSalesPerson.Focus();
        //    return;
        //}


        bool IsReceived = false;
        DataTable dtTemp = new DataTable();
        double strno = 0;
        if (ddlProduct.SelectedIndex > 0)
        {
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            if (txtcommpercentage.Text == "")
            {
                txtcommpercentage.Text = "0";
            }
            if (txttotalCommission.Text == "")
            {
                txttotalCommission.Text = "0";
            }

            DataTable dt = (DataTable)Session["dtSalesCommissionDetail"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Trans_Id", typeof(double));
                dt.Columns.Add("Invoice_Id");
                dt.Columns.Add("Invoice_No");
                dt.Columns.Add("Invoice_Date", typeof(DateTime));
                dt.Columns.Add("Customer_Id");
                dt.Columns.Add("Name");
                dt.Columns.Add("Product_Id");
                dt.Columns.Add("ProductCode");
                dt.Columns.Add("EProductName");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Comission_Percentage");
                dt.Columns.Add("Comission_Amount");
                dt.Columns.Add("Is_Receive", typeof(bool));
                dt.Columns.Add("Is_Paid", typeof(bool));
                dt.Columns.Add("Paid_Date", typeof(DateTime));
                dt.Columns.Add("AgentId", typeof(int));
                dt.Columns.Add("Is_Return", typeof(bool));
                dt.Columns.Add("LocalAmount");
                dt.Columns.Add("CurrencyId");
                dt.Columns.Add("ProjectId");
                strno = 1;
            }
            else
            {

                if (new DataView((DataTable)Session["dtSalesCommissionDetail"], "Invoice_Id=" + hdnInvoiceId.Value.ToString() + " and Product_Id=" + ddlProduct.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    DisplayMessage("Product already exists !");
                    return;
                }
                try
                {

                    strno = Convert.ToDouble(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1;
                }
                catch
                {
                    strno = 1;
                }
            }

            //here we check that payment received or not for this invoice
            //if payment received than we update sttaus of is received
            try
            {
                DataTable dtAgeing = objDa.return_DataTable("select (Max(Invoice_Amount)-sum(Paid_Receive_Amount)) as NetSum from ac_ageing_detail where Ref_Type='SINV' and Ref_Id=" + hdnInvoiceId.Value + "");

                if (dtAgeing.Rows.Count > 0)
                {

                    if (Convert.ToDouble(dtAgeing.Rows[0]["NetSum"].ToString()) <= 0)
                    {
                        IsReceived = true;
                    }
                }
            }
            catch
            {
            }

            dt.Rows.Add(strno.ToString(), hdnInvoiceId.Value, txtsalesinvoice.Text, txtInvoicedate.Text, txtcutomerName.Text.Split('/')[1].ToString(), txtcutomerName.Text.Split('/')[0].ToString(), ddlProduct.SelectedValue, ProductCode(ddlProduct.SelectedValue), ddlProduct.SelectedItem.Text, txtAmount.Text, txtcommpercentage.Text, txttotalCommission.Text, IsReceived, false.ToString(), "1/1/1900", "0", hdnReturnFlag.Value, txtLocalAmount.Text, ddlCurrency.SelectedValue);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            DataTable dtchild = getChildrecordinDatatatable();
            dt = new DataView(dt, "", "Invoice_Date asc", DataViewRowState.CurrentRows).ToTable();

            objPageCmn.FillData((object)gvDetail, dt, "", "");
            Session["dtSalesCommissionDetail"] = dt;
            txtAmount.Text = "0";
            txtcommpercentage.Text = "0";
            txttotalCommission.Text = "0";
            ddlProduct.SelectedIndex = 0;
            setChildrecordinDatatatable(dtchild);
            GetGridTotal();
        }
        else
        {
            txtAmount.Text = "0";
            txtcommpercentage.Text = "0";
            txttotalCommission.Text = "0";
            ddlProduct.Items.Clear();
            DisplayMessage("Select Product Name");
            return;

        }
    }
    public void GetChildGrid()
    {


        foreach (GridViewRow gvr in gvDetail.Rows)
        {

            GridView gvChild = (GridView)gvr.FindControl("gvEmp");

            if (gvChild.Rows.Count == 0)
            {
                DataTable dtEmp = new DataTable();

                dtEmp.Columns.Add("Trans_Id", typeof(double));
                dtEmp.Columns.Add("Commission_Person");
                dtEmp.Columns.Add("Commission_Percentage", typeof(double));
                dtEmp.Columns.Add("Is_Paid", typeof(bool));
                dtEmp.Columns.Add("Paid_Date", typeof(DateTime));

                DataRow dr;

                if (ddlType.SelectedValue.Trim() == "Developer")
                {
                    DataTable dtDevelopr = objDa.return_DataTable("select emp_id from Prj_Project_Team where Project_Id=" + ((Label)gvr.FindControl("lblProjectId")).Text + " and Field5='True'");
                    int counter = 0;
                    foreach (DataRow drnew in dtDevelopr.Rows)
                    {
                        counter++;
                        dr = dtEmp.NewRow();
                        dr[0] = counter.ToString();
                        ((LinkButton)gvr.FindControl("lnkAddEmp")).Visible = true;
                        dr[1] = drnew["emp_id"].ToString();
                        dr[2] = SetDecimal((Convert.ToDouble(((TextBox)gvr.FindControl("txtper")).Text) / dtDevelopr.Rows.Count).ToString()).ToString();
                        dr[3] = false;
                        dr[4] = "1/1/1900 12:00:00 AM";
                        dtEmp.Rows.Add(dr);

                    }
                }
                else
                {

                    dr = dtEmp.NewRow();
                    dr[0] = 1;
                    if (ddlType.SelectedValue.Trim() == "Agent")
                    {
                        ((LinkButton)gvr.FindControl("lnkAddEmp")).Visible = false;

                        try
                        {
                            dr[1] = new DataView((DataTable)Session["dtSalesCommissionDetail"], "Invoice_Id=" + ((Label)gvr.FindControl("lblInvoiceId")).Text + " and Product_Id=" + ((Label)gvr.FindControl("lblproductId")).Text + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["AgentId"].ToString();
                        }
                        catch
                        {
                            dr[1] = "0";
                        }
                    }
                    else
                    {
                        ((LinkButton)gvr.FindControl("lnkAddEmp")).Visible = true;
                        dr[1] = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
                    }
                    dr[2] = ((TextBox)gvr.FindControl("txtper")).Text;
                    dr[3] = false;
                    dr[4] = "1/1/1900 12:00:00 AM";
                    dtEmp.Rows.Add(dr);
                }



                gvChild.DataSource = dtEmp;
                gvChild.DataBind();
            }
        }
    }
    protected void IbtnDeleteComm_Command(object sender, CommandEventArgs e)
    {
        Session["dtSalesCommissionDetail"] = new DataView((DataTable)Session["dtSalesCommissionDetail"], "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dtchild = getChildrecordinDatatatable();
        gvDetail.DataSource = (DataTable)Session["dtSalesCommissionDetail"];
        gvDetail.DataBind();
        setChildrecordinDatatatable(dtchild);
        GetGridTotal();
    }
    public void GetGridTotal()
    {

        DataTable dtEmpCommission = new DataTable();
        double totalsalesAmount = 0;
        double totalReceived = 0;
        double totalpaid = 0;
        double totalRemain = 0;
        double totalCommission = 0;
        double totalretrunCommission = 0;



        foreach (GridViewRow gvrow in gvDetail.Rows)
        {

            if (ddlType.SelectedValue.Trim() == "Agent")
            {
                ((LinkButton)gvrow.FindControl("lnkAddEmp")).Visible = false;
            }
            else
            {
                ((LinkButton)gvrow.FindControl("lnkAddEmp")).Visible = true;
            }

            totalsalesAmount += Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text);
            totalCommission += Convert.ToDouble(((TextBox)gvrow.FindControl("lblcommamt")).Text);


            if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
            {
                totalReceived += Convert.ToDouble(((TextBox)gvrow.FindControl("lblcommamt")).Text);
            }

            if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text))
            {
                totalretrunCommission += Convert.ToDouble(((TextBox)gvrow.FindControl("lblcommamt")).Text);
            }
        }

        gvEmployee.DataSource = GetEmployeeCommissionDetail();
        gvEmployee.DataBind();
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            totalpaid += Convert.ToDouble(((Label)gvrow.FindControl("lblPaid")).Text);
        }
        txttotalsalesAmount.Text = SetDecimal(totalsalesAmount.ToString());
        txttotalReceivedAmount.Text = SetDecimal(totalReceived.ToString());
        txttotalPaidAmount.Text = SetDecimal(totalpaid.ToString());
        txttotalRemainAmount.Text = SetDecimal((totalReceived - totalpaid).ToString());
        txtNetCommission.Text = SetDecimal(totalCommission.ToString());
        txttotalreturmCommission.Text = SetDecimal(totalretrunCommission.ToString());
        txtTotalActualCommission.Text = SetDecimal((totalCommission - totalretrunCommission).ToString());
        txtUnpaidbalance.Text = SetDecimal(((totalCommission - totalretrunCommission) - totalpaid).ToString());
    }


    public string GetReturnadjusted(string strHeaderTransId, string strcommissionperson)
    {
        string strsql = "select isnull(SUM(Return_Amount),0) from Inv_ReturnCommisison where Commision_Person=" + strcommissionperson + " and Voucher_Header_Id=" + strHeaderTransId + " and Is_Adjustable='True'";
        return SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());
    }

    public DataTable GetEmployeeCommissionDetail()
    {
        DataTable dtEmpcom = new DataTable();
        DataTable dtEmpCommission = new DataTable();
        dtEmpCommission.Columns.Add("EmpId");
        dtEmpCommission.Columns.Add("ReceivedAmt");
        dtEmpCommission.Columns.Add("PaidAmt");
        dtEmpCommission.Columns.Add("RemainAmt");
        dtEmpCommission.Columns.Add("TotalCommission");
        dtEmpCommission.Columns.Add("Is_Paid", typeof(bool));
        dtEmpCommission.Columns.Add("Returncommission");
        dtEmpCommission.Columns.Add("Remaincommission");
        dtEmpCommission.Columns.Add("AdjustedReturn");

        foreach (GridViewRow gvrow in gvDetail.Rows)
        {

            GridView gvchild = (GridView)gvrow.FindControl("gvEmp");

            foreach (GridViewRow gvchildrow in gvchild.Rows)
            {
                DataRow dr = dtEmpCommission.NewRow();


                dr[0] = ((HiddenField)gvchildrow.FindControl("hdnGvEmpId")).Value;

                //for received

                //here we checking return condition 

                if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                {
                    if (ddlType.SelectedValue.Trim() == "Agent")
                    {
                        dr[1] = SetDecimal(((TextBox)gvrow.FindControl("lblcommamt")).Text);
                    }
                    else
                    {
                        dr[1] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvchildrow.FindControl("lblcommissionper")).Text)) / 100).ToString());

                    }
                }
                else
                {
                    dr[1] = "0";
                }


                //for paid


                if (((Label)gvchildrow.FindControl("lblispaid")).Text == "")
                {
                    ((Label)gvchildrow.FindControl("lblispaid")).Text = false.ToString();
                }

                if (Convert.ToBoolean(((Label)gvchildrow.FindControl("lblispaid")).Text))
                {

                    if (ddlType.SelectedValue.Trim() == "Agent")
                    {
                        dr[2] = SetDecimal(((TextBox)gvrow.FindControl("lblcommamt")).Text);

                    }
                    else
                    {

                        dr[2] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvchildrow.FindControl("lblcommissionper")).Text)) / 100).ToString());

                    }
                }
                else
                {
                    dr[2] = "0";
                }




                dr[3] = Convert.ToDouble(dr[1].ToString()) - Convert.ToDouble(dr[2].ToString());



                if (ddlType.SelectedValue.Trim() == "Agent")
                {

                    dr[4] = SetDecimal(((TextBox)gvrow.FindControl("lblcommamt")).Text);
                }
                else
                {
                    dr[4] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvchildrow.FindControl("lblcommissionper")).Text)) / 100).ToString());

                }


                if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text) && Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                {
                    if (ddlType.SelectedValue.Trim() == "Agent")
                    {
                        dr[6] = SetDecimal(((TextBox)gvrow.FindControl("lblcommamt")).Text);
                    }
                    else
                    {

                        double ReturnAmount = 0;
                        double ReturnCommisionAmt = 0;
                        string strsql = "select sum(((Inv_SalesReturnDetail.UnitPrice*Inv_SalesReturnDetail.ReturnQty)-Inv_SalesReturnDetail.DiscountV+Inv_SalesReturnDetail.TaxV)) as TotalReturmAmount from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No where Inv_SalesReturnHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesReturnHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesReturnHeader.Location_Id=" + Session["LocId"].ToString() + " and Inv_SalesReturnHeader.Post='True'  and Inv_SalesReturnDetail.Invoice_No=" + ((Label)gvrow.FindControl("lblInvoiceId")).Text + " and Inv_SalesReturnDetail.Product_Id=" + ((Label)gvrow.FindControl("lblproductId")).Text + "";

                        DataTable dtreturn = objDa.return_DataTable(strsql);


                        if (dtreturn.Rows.Count > 0)
                        {

                            ReturnAmount = Convert.ToDouble(dtreturn.Rows[0][0].ToString());
                            double Exchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(((Label)gvrow.FindControl("lblgvCurrencyId")).Text, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString()));
                            double ProductReturmAmount = Convert.ToDouble(ReturnAmount) * Exchnagerate;
                            double CommisionPercentage = Convert.ToDouble(((Label)gvchildrow.FindControl("lblcommissionper")).Text);
                            ReturnCommisionAmt = (ProductReturmAmount * CommisionPercentage) / 100;

                        }


                        // dr[6] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvchildrow.FindControl("lblcommissionper")).Text)) / 100).ToString());
                        dr[6] = SetDecimal(ReturnCommisionAmt.ToString());
                    }
                }
                else
                {
                    dr[6] = "0";
                }




                //for remain commission 

                dr[7] = Convert.ToDouble(dr[4].ToString()) - Convert.ToDouble(dr[6].ToString());



                dr[5] = Convert.ToBoolean(((Label)gvchildrow.FindControl("lblispaid")).Text);


                if (editid.Value != "")
                {
                    dr[8] = SetDecimal(objDa.return_DataTable("select SUM(Return_Amount) from Inv_ReturnCommisison where Commision_Person=" + ((HiddenField)gvchildrow.FindControl("hdnGvEmpId")).Value + " and Voucher_Header_Id=" + editid.Value + " and Is_Adjustable='True'").Rows[0][0].ToString());
                }
                else
                {
                    dr[8] = "0.000";
                }


                dtEmpCommission.Rows.Add(dr);

            }
        }


        dtEmpcom.Columns.Add("EmpId");
        dtEmpcom.Columns.Add("ReceivedAmt");
        dtEmpcom.Columns.Add("PaidAmt");
        dtEmpcom.Columns.Add("RemainAmt");
        dtEmpcom.Columns.Add("TotalCommission");
        dtEmpcom.Columns.Add("Is_Paid", typeof(bool));
        dtEmpcom.Columns.Add("Returncommission");
        dtEmpcom.Columns.Add("Remaincommission");
        dtEmpcom.Columns.Add("AdjustedReturn");
        DataTable dtTemp = dtEmpCommission.Copy();
        dtEmpCommission = dtEmpCommission.DefaultView.ToTable(true, "EmpId");

        for (int i = 0; i < dtEmpCommission.Rows.Count; i++)
        {
            DataRow dr = dtEmpcom.NewRow();
            double ReceivedAmt = 0;
            double PaidAmt = 0;
            double RemainAmt = 0;
            double TotalCommission = 0;
            double returnCommission = 0;
            double remainCommission = 0;
            double adjustedCommission = 0;

            DataTable dt = new DataView(dtTemp, "EmpId=" + dtEmpCommission.Rows[i][0].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


            for (int j = 0; j < dt.Rows.Count; j++)
            {
                ReceivedAmt += Convert.ToDouble(dt.Rows[j]["ReceivedAmt"].ToString());
                PaidAmt += Convert.ToDouble(dt.Rows[j]["PaidAmt"].ToString());
                RemainAmt += Convert.ToDouble(dt.Rows[j]["RemainAmt"].ToString());
                TotalCommission += Convert.ToDouble(dt.Rows[j]["TotalCommission"].ToString());
                returnCommission += Convert.ToDouble(dt.Rows[j]["Returncommission"].ToString());
                remainCommission += Convert.ToDouble(dt.Rows[j]["Remaincommission"].ToString());
                adjustedCommission += Convert.ToDouble(dt.Rows[j]["AdjustedReturn"].ToString());
            }



            dr[0] = dtEmpCommission.Rows[i][0].ToString();
            dr[1] = SetDecimal(ReceivedAmt.ToString());
            dr[2] = SetDecimal(PaidAmt.ToString());
            if (ddlType.SelectedValue.Trim() == "Agent")
            {
                dr[3] = SetDecimal((RemainAmt - returnCommission).ToString());

                if ((RemainAmt - returnCommission) < 0)
                {
                    dr[3] = "0.000";
                }
            }
            else
            {
                dr[3] = SetDecimal(RemainAmt.ToString());
            }
            dr[4] = SetDecimal(TotalCommission.ToString());
            dr[6] = SetDecimal(returnCommission.ToString());
            dr[7] = SetDecimal(remainCommission.ToString());
            dr[8] = SetDecimal(adjustedCommission.ToString());
            if (TotalCommission == PaidAmt)
            {
                dr[5] = true;
            }
            else
            {
                dr[5] = false;
            }
            dtEmpcom.Rows.Add(dr);


        }


        return dtEmpcom;
    }
    protected void txtper_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblTransId = ((Label)Row.FindControl("lblTransId"));

        GridView gvchild = (GridView)Row.FindControl("gvEmp");

        if (((TextBox)Row.FindControl("txtper")).Text == "")
        {
            ((TextBox)Row.FindControl("txtper")).Text = "0";
        }

        ((TextBox)Row.FindControl("lblcommamt")).Text = SetDecimal(((Convert.ToDouble(((Label)Row.FindControl("lblAmt")).Text) * Convert.ToDouble(((TextBox)Row.FindControl("txtper")).Text)) / 100).ToString());

        ((TextBox)Row.FindControl("txtper")).Text = SetDecimal(((TextBox)Row.FindControl("txtper")).Text);
        double comm_percentage = Convert.ToDouble(((TextBox)Row.FindControl("txtper")).Text);

        double Emppercentage = comm_percentage / gvchild.Rows.Count;
        foreach (GridViewRow gvr in gvchild.Rows)
        {
            ((Label)gvr.FindControl("lblcommissionper")).Text = SetDecimal(Emppercentage.ToString());

        }
        GetGridTotal();



        DataTable dtsalescommission = (DataTable)Session["dtSalesCommissionDetail"];

        for (int i = 0; i < dtsalescommission.Rows.Count; i++)
        {
            if (dtsalescommission.Rows[i]["Trans_Id"].ToString() == lblTransId.Text)
            {
                dtsalescommission.Rows[i]["Comission_Percentage"] = ((TextBox)Row.FindControl("txtper")).Text;
                dtsalescommission.Rows[i]["Comission_Amount"] = ((TextBox)Row.FindControl("lblcommamt")).Text;
                break;
            }
        }
        Session["dtSalesCommissionDetail"] = dtsalescommission;

    }
    protected void lblcommamt_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblTransId = ((Label)Row.FindControl("lblTransId"));

        GridView gvchild = (GridView)Row.FindControl("gvEmp");

        if (((TextBox)Row.FindControl("lblcommamt")).Text == "")
        {
            ((TextBox)Row.FindControl("lblcommamt")).Text = "0";
        }



        ((TextBox)Row.FindControl("txtper")).Text = SetDecimal(((100 * Convert.ToDouble(((TextBox)Row.FindControl("lblcommamt")).Text)) / Convert.ToDouble(((Label)Row.FindControl("lblAmt")).Text)).ToString());

        ((TextBox)Row.FindControl("lblcommamt")).Text = SetDecimal(((TextBox)Row.FindControl("lblcommamt")).Text);
        double comm_percentage = Convert.ToDouble(((TextBox)Row.FindControl("txtper")).Text);

        double Emppercentage = comm_percentage / gvchild.Rows.Count;
        foreach (GridViewRow gvr in gvchild.Rows)
        {
            ((Label)gvr.FindControl("lblcommissionper")).Text = SetDecimal(Emppercentage.ToString());

        }
        GetGridTotal();

        DataTable dtsalescommission = (DataTable)Session["dtSalesCommissionDetail"];

        for (int i = 0; i < dtsalescommission.Rows.Count; i++)
        {
            if (dtsalescommission.Rows[i]["Trans_Id"].ToString() == lblTransId.Text)
            {
                dtsalescommission.Rows[i]["Comission_Percentage"] = ((TextBox)Row.FindControl("txtper")).Text;
                dtsalescommission.Rows[i]["Comission_Amount"] = ((TextBox)Row.FindControl("lblcommamt")).Text;
                break;
            }
        }
        Session["dtSalesCommissionDetail"] = dtsalescommission;

    }
    #endregion
    #region updatereceivedsttaus
    protected void btnRefreshReceivestatus_Click(object sender, EventArgs e)
    {
                int counter = 0;

        DataTable dt = new DataTable();
        if (Session["dtSalesCommissionDetail"] != null)
        {
            dt = (DataTable)Session["dtSalesCommissionDetail"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (!Convert.ToBoolean(dt.Rows[i]["Is_Receive"].ToString()))
                {

                    //Invoice_Id

                    //here we checking that if invoice payment is cash than no need to check that payment received or not 

                    //code start

                    string strPaymenttype = objSinvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["Invoice_Id"].ToString()).Rows[0]["PaymentType"].ToString();
                    //code end


                    //if payment received than we update sttaus of is received

                    if (strPaymenttype.Trim() == "Credit")
                    {

                        DataTable dtAgeing = objDa.return_DataTable("select (sum(case when paid_receive_amount=0 then foreign_amount else 0 end)-sum(case when paid_receive_amount>0 then foreign_amount else 0 end)) as NetSum from ac_ageing_detail where Ref_Type='SINV' and Ref_Id=" + dt.Rows[i]["Invoice_Id"].ToString() + " and isactive='True'");


                        if (dtAgeing.Rows.Count > 0)
                        {
                            if (dtAgeing.Rows[0]["NetSum"].ToString().Trim() != "")
                            {

                                if (Convert.ToDouble(dtAgeing.Rows[0]["NetSum"].ToString()) <= 0)
                                {
                                    dt.Rows[i]["Is_Receive"] = true;
                                    counter++;
                                }
                            }
                        }
                    }
                    else if (strPaymenttype.Trim() == "Cash")
                    {
                        dt.Rows[i]["Is_Receive"] = true;
                        counter++;
                    }
                }

                //update return flag
                //here we set return flag in according return qty


                if (!Convert.ToBoolean(dt.Rows[i]["Is_Return"].ToString()))
                {

                    DataTable dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dt.Rows[i]["Invoice_Id"].ToString(), Session["FinanceYearId"].ToString());

                    dtDetail = new DataView(dtDetail, "Product_Id=" + dt.Rows[i]["Product_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


                    bool IsReturn = false;
                    if (dtDetail.Rows[0]["ReturnQty"] != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(dtDetail.Rows[0]["ReturnQty"].ToString()) > 0)
                            {

                                IsReturn = true;
                                counter++;
                            }
                        }
                        catch
                        {

                        }
                    }
                    dt.Rows[i]["Is_Return"] = IsReturn;
                }
            }
            Session["dtSalesCommissionDetail"] = dt;
            DataTable dtchild = getChildrecordinDatatatable();
            objPageCmn.FillData((object)gvDetail, dt, "", "");
            setChildrecordinDatatatable(dtchild);
            GetGridTotal();
        }

        DisplayMessage(counter + " Record Affected ");

        if (counter == 0)
        {
            return;
        }

        if (editid.Value != "")
        {

            string Strsalesperson = string.Empty;


            if (ddlType.SelectedValue.Trim() != "Agent")
            {
                if (txtSalesPerson.Text.Trim() == "")
                {
                    DisplayMessage("Enter Sales person !");
                    txtSalesPerson.Focus();

                    return;
                }
                else
                {
                    try
                    {
                        Strsalesperson = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
                    }
                    catch
                    {
                        DisplayMessage("Employee not found !");
                        txtSalesPerson.Focus();

                        return;
                    }

                }
            }
            else
            {
                Strsalesperson = "0";
            }

            //here we checking post status of current record
            bool PostStatus = false;

            DataTable dtPostStatus = objsalesCommissionHeader.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtPostStatus.Rows.Count > 0)
            {
                PostStatus = Convert.ToBoolean(dtPostStatus.Rows[0]["Post"].ToString());

            }

            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                objsalesCommissionHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtVoucherNo.Text, objSys.getDateForInput(txtVoucherDate.Text).ToString(), objSys.getDateForInput(txtfromdate.Text).ToString(), objSys.getDateForInput(txttodate.Text).ToString(), Strsalesperson, "0", txttotalsalesAmount.Text, txttotalReceivedAmount.Text, txttotalPaidAmount.Text, txttotalRemainAmount.Text, txtNetCommission.Text, PostStatus.ToString(), txtremarks.Text, ddlType.SelectedValue, txtUnpaidbalance.Text, txttotalreturmCommission.Text, txtTotalActualCommission.Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //here we insert record in detail table
                //first we dete and reinsert
                //here we insert record in detail table

                //delete record  from child table 
                DataTable dtDetail = objsalesCommissionDetail.GetRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    objcommEmployeeDetail.DeleteRecord_By_VoucherNo(dr["Trans_Id"].ToString(), ref trns);
                }
                objsalesCommissionDetail.DeleteRecord_By_VoucherNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ref trns);

                foreach (GridViewRow gvrow in gvDetail.Rows)
                {
                    string strPaiddate = string.Empty;


                    int voucherId = objsalesCommissionDetail.InsertRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), editid.Value, ((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblInvoiceNo")).Text, objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString(), ((Label)gvrow.FindControl("lblCustomerId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblforeignAmt")).Text, ((TextBox)gvrow.FindControl("txtper")).Text, ((TextBox)gvrow.FindControl("lblcommamt")).Text, ((Label)gvrow.FindControl("lblisreceived")).Text, true.ToString(), DateTime.Now.ToString(), ddlType.SelectedValue, ((Label)gvrow.FindControl("lblAmt")).Text, ((Label)gvrow.FindControl("lblisreturn")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //here we update new voucher detail id in return commission table

                    string strsql = "update Inv_ReturnCommisison set Voucher_Detail_Id=" + voucherId.ToString() + " where Voucher_Detail_Id=" + ((Label)gvrow.FindControl("lblTransId")).Text + " and Voucher_Header_Id=" + editid.Value + "";
                    objDa.execute_Command(strsql, ref trns);


                    //for insert record in child table
                    GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));

                    foreach (GridViewRow gvr in gvchild.Rows)
                    {

                        string strcommissionPaiddate = string.Empty;

                        if (Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text))
                        {
                            strcommissionPaiddate = ((Label)gvr.FindControl("lblpaiddate")).Text;
                        }
                        else
                        {
                            strcommissionPaiddate = "1/1/1900 12:00:00 AM";
                        }
                        objcommEmployeeDetail.InsertRecord(voucherId.ToString(), ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value, ((Label)gvr.FindControl("lblcommissionper")).Text, SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvr.FindControl("lblcommissionper")).Text)) / 100).ToString()), Convert.ToBoolean(((Label)gvr.FindControl("lblispaid")).Text).ToString(), strcommissionPaiddate, ref trns);
                    }
                }
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



    }
    #endregion
    #region Post
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();

    }
    #endregion
    #region GetInvoiceDetail
    protected void btngetCommissionRecord_Click(object sender, EventArgs e)
    {
        if (txtfromdate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtfromdate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtfromdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid date");
                txtfromdate.Focus();
                return;
            }
        }

        if (txttodate.Text == "")
        {
            DisplayMessage("Enter to Date");
            txttodate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txttodate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid date");
                txttodate.Focus();
                return;
            }
        }

        if (Convert.ToDateTime(txtfromdate.Text) > Convert.ToDateTime(txttodate.Text))
        {
            DisplayMessage("from date should be less then to date");
            txtfromdate.Focus();
            return;
        }


        if (ddlType.SelectedValue.Trim() != "Agent" && ddlType.SelectedValue.Trim() != "Developer")
        {
            if (txtSalesPerson.Text == "")
            {
                DisplayMessage("Enter Sales person");
                txtSalesPerson.Focus();
                return;
            }
        }


        objPageCmn.FillData((object)gvDetail, null, "", "");
        Session["dtSalesCommissionDetail"] = null;
        GetGridTotal();

        DataTable dt = new DataTable();

        if (editid.Value == "")
        {
            // dt = objSinvoiceHeader.GetCommissionRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), ddlType.SelectedValue,Convert.ToDateTime(txtfromdate.Text).ToString("yyyy-MM-dd"),Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd"));
            dt = objSinvoiceHeader.GetCommissionRecordByDate(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), ddlType.SelectedValue, Convert.ToDateTime(txtfromdate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txttodate.Text).ToString("yyyy-MM-dd"));
        }
        else
        {
            dt = objSinvoiceHeader.GetCommissionRecord(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), ddlType.SelectedValue, editid.Value);
        }

        DateTime ToDate = Convert.ToDateTime(txttodate.Text);
        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
        if (ddlType.SelectedValue.Trim() == "Developer")
        {
            try
            {
                dt = new DataView(dt, "Invoice_Date>='" + txtfromdate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "' and projectId<>0 and DeveloperCommission<>0", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        else if (ddlType.SelectedValue.Trim() == "Agent")
        {
            try
            {
                dt = new DataView(dt, "Invoice_Date>='" + txtfromdate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        else if (ddlType.SelectedValue.Trim() == "Sales")
        {
            try
            {
                dt = new DataView(dt, "Invoice_Date>='" + txtfromdate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "' and SalesPerson_Id='" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        else if (ddlType.SelectedValue.Trim() == "Technical")
        {
            try
            {
                dt = new DataView(dt, "Invoice_Date>='" + txtfromdate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        //if type selected agent then we get only those record in which agent name and agenet commissiom is entered


        if (ddlType.SelectedValue.Trim() == "Agent")
        {
            try
            {
                dt = new DataView(dt, "AgentCommission<>0", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

        }

        //for techical team we need to show only service related product
        if (ddlType.SelectedValue == "Technical")
        {
            dt = new DataView(dt, "ProductCode like '%srv%'", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");

            return;
        }

        ddlType.Enabled = false;
        DataTable dtCommssion = new DataTable();
        dtCommssion.Columns.Add("Trans_Id", typeof(double));
        dtCommssion.Columns.Add("Invoice_Id");
        dtCommssion.Columns.Add("Invoice_No");
        dtCommssion.Columns.Add("Invoice_Date", typeof(DateTime));
        dtCommssion.Columns.Add("Customer_Id");
        dtCommssion.Columns.Add("Name");
        dtCommssion.Columns.Add("Product_Id");
        dtCommssion.Columns.Add("ProductCode");
        dtCommssion.Columns.Add("EProductName");
        dtCommssion.Columns.Add("Amount");
        dtCommssion.Columns.Add("Comission_Percentage");
        dtCommssion.Columns.Add("Comission_Amount");
        dtCommssion.Columns.Add("Is_Receive", typeof(bool));
        dtCommssion.Columns.Add("Is_Paid", typeof(bool));
        dtCommssion.Columns.Add("Paid_Date", typeof(DateTime));
        dtCommssion.Columns.Add("AgentId", typeof(int));
        dtCommssion.Columns.Add("Is_Return", typeof(bool));
        dtCommssion.Columns.Add("LocalAmount");
        dtCommssion.Columns.Add("CurrencyId");
        dtCommssion.Columns.Add("projectid");
        //dtCommssion.Columns.Add("Invoice_Month");
        //dtCommssion.Columns.Add("Invoice_Year");
        //dtCommssion.Columns.Add("CategoryId");

        double srNo = 0;
        foreach (DataRow dr in dt.Rows)
        {
            srNo++;
            DataRow DrNew = dtCommssion.NewRow();
            DrNew["Trans_Id"] = srNo;
            DrNew["Invoice_Id"] = dr["Trans_Id"].ToString();
            DrNew["projectid"] = dr["projectid"].ToString();
            DrNew["Invoice_No"] = dr["Invoice_No"].ToString();
            DrNew["Invoice_Date"] = dr["Invoice_Date"].ToString();
            DrNew["Customer_Id"] = dr["Supplier_Id"].ToString();
            DrNew["Name"] = dr["Name"].ToString();
            DrNew["Product_Id"] = dr["ProductId"].ToString();
            DrNew["ProductCode"] = dr["ProductCode"].ToString();
            DrNew["EProductName"] = dr["EProductName"].ToString();
            DrNew["Amount"] = dr["Amount"].ToString();
            DrNew["Comission_Percentage"] = dr["Comission_Percentage"].ToString();
            if (ddlType.SelectedValue.Trim() == "Agent")
            {
                DrNew["Comission_Amount"] = dr["AgentCommission"].ToString();

                DrNew["Comission_Percentage"] = SetDecimal(((100 * Convert.ToDouble(DrNew["Comission_Amount"].ToString())) / Convert.ToDouble(DrNew["Amount"].ToString())).ToString());

            }
            else
            {
                DrNew["Comission_Amount"] = SetDecimal(dr["Comission_Amount"].ToString());
            }

            try
            {
                DrNew["AgentId"] = dr["AgentId"].ToString();
            }
            catch
            {
                DrNew["AgentId"] = "0";
            }



            bool IsReceived = false;


            string strPaymenttype = objSinvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["Trans_Id"].ToString()).Rows[0]["PaymentType"].ToString();
            //code end


            //if payment received than we update sttaus of is received

            if (strPaymenttype.Trim() == "Credit")
            {

                DataTable dtAgeing = objDa.return_DataTable("select (max(Invoice_Amount)-sum(Paid_Receive_Amount)) as NetSum from ac_ageing_detail where Ref_Type='SINV' and Ref_Id=" + dr["Trans_Id"].ToString() + "");


                if (dtAgeing.Rows.Count > 0)
                {
                    if (dtAgeing.Rows[0]["NetSum"].ToString().Trim() != "")
                    {

                        if (Convert.ToDouble(dtAgeing.Rows[0]["NetSum"].ToString()) <= 0)
                        {
                            IsReceived = true;
                        }
                    }
                }
            }
            else if (strPaymenttype.Trim() == "Cash")
            {
                IsReceived = true;
            }



            DrNew["Is_Receive"] = IsReceived;
            DrNew["Is_Paid"] = false;
            DrNew["Paid_Date"] = "1/1/1900";
            DrNew["Is_Return"] = dr["Is_Return"].ToString();
            DrNew["LocalAmount"] = dr["LocalAmount"].ToString();
            DrNew["CurrencyId"] = dr["Currency_Id"].ToString();
            //DrNew["Invoice_Month"] = Convert.ToDateTime(dr["Invoice_Date"].ToString()).Month.ToString();
            //DrNew["Invoice_Year"] = Convert.ToDateTime(dr["Invoice_Date"].ToString()).Year.ToString();
            //DrNew["CategoryId"] = dr["CategoryId"].ToString();
            dtCommssion.Rows.Add(DrNew);
        }
        //if (txtSalesPerson.Text == "")
        //    DisplayMessage("Please Enter Employee Name");
        //DataTable Dt_New_Commission = null;
        //try
        //{
        //    Dt_New_Commission = Commission_Function(dtCommssion, txtSalesPerson.Text.Split('/')[1].ToString());
        //}
        //catch { }
        Session["dtSalesCommissionDetail"] = dtCommssion;
        objPageCmn.FillData((object)gvDetail, dtCommssion, "", "");
        //Session["dtSalesCommissionDetail"] = Dt_New_Commission;
        //cmn.FillData((object)gvDetail, Dt_New_Commission, "", "");
        if (ddlType.SelectedValue.ToString() == "Sales")
        {
            foreach (GridViewRow gvr in gvDetail.Rows)
            {
                ((LinkButton)gvr.FindControl("lnkAddEmp")).Attributes.Add("style", "display:none");
                ((TextBox)gvr.FindControl("lblcommamt")).Enabled = false;
                ((TextBox)gvr.FindControl("txtper")).Enabled = false;
            }
        }
        GetChildGrid();
        GetGridTotal();
        //AllPageCode();
    }

    protected void lblgvSInvNo_Command(object sender, CommandEventArgs e)
    {

        if (!isSalesInvoicePermission())
        {
            DisplayMessage("User have no permission to view sales invoice ");
            return;
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales/SalesInvoice.aspx?Id=" + e.CommandArgument.ToString() + "')", true);
    }
    public bool isSalesInvoicePermission()
    {
        bool isAllow = false;

        //here we checking user permission for view sales order info 

        if (Session["EmpId"].ToString() == "0")
        {
            isAllow = true;
        }
        else
        {

            DataTable dtAllpagecode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "144", "92", Session["CompId"].ToString());

            if (dtAllpagecode.Rows.Count > 0)
            {
                isAllow = true;

            }
        }

        return isAllow;
    }
    #endregion
    #region setDecimal
    public string SetDecimal(string amount)
    {
        return objSys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }
    public string SetDecimal(string amount, ref SqlTransaction trns)
    {
        return objSys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount, ref trns);
    }
    #endregion
    #region Addemployee

    protected void IbtnDeleteEmp_Command(object sender, CommandEventArgs e)
    {

        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        GridView gvchild = (GridView)(gvrow.Parent.Parent);


        if (gvchild.Rows.Count == 1)
        {
            DisplayMessage("You can not delete ,at least one person required !");
            return;
        }
        GridViewRow Gv1Row = (GridViewRow)(gvchild.NamingContainer);

        double comm_percentage = Convert.ToDouble(((TextBox)gvDetail.Rows[Gv1Row.RowIndex].FindControl("txtper")).Text);

        double Emppercentage = comm_percentage / (gvchild.Rows.Count - 1);

        DataTable dtEmp = new DataTable();

        dtEmp.Columns.Add("Trans_Id", typeof(double));
        dtEmp.Columns.Add("Commission_Person");
        dtEmp.Columns.Add("Commission_Percentage", typeof(double));
        dtEmp.Columns.Add("Is_Paid", typeof(bool));
        dtEmp.Columns.Add("Paid_Date", typeof(DateTime));

        foreach (GridViewRow gvr in gvchild.Rows)
        {
            DataRow dr = dtEmp.NewRow();
            dr[0] = ((HiddenField)gvr.FindControl("hdnGvTransId")).Value;
            dr[1] = ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value;
            dr[2] = Emppercentage;
            dr[3] = ((Label)gvr.FindControl("lblispaid")).Text;
            if (((Label)gvr.FindControl("lblpaiddate")).Text == "")
            {

                dr[4] = "1/1/1900 12:00:00 AM";
            }
            else
            {
                dr[4] = ((Label)gvr.FindControl("lblpaiddate")).Text;
            }

            dtEmp.Rows.Add(dr);
        }
        dtEmp = new DataView(dtEmp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        gvchild.DataSource = dtEmp;
        gvchild.DataBind();
        gvEmployee.DataSource = GetEmployeeCommissionDetail();
        gvEmployee.DataBind();
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        if (ddlType.SelectedValue.Trim() == "Agent")
        {
            if (EmployeeId != "" && EmployeeId != "0")
            {
                EmployeeName = ObjContact.GetContactTrueById(EmployeeId).Rows[0]["Name"].ToString();
            }
            else
            {
                EmployeeName = "";
            }
        }
        else
        {
            if (EmployeeId != "" && EmployeeId != "0")
            {
                DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
                if (Dt.Rows.Count > 0)
                {
                    EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
                }
            }
            else
            {
                EmployeeName = "";
            }
        }

        return EmployeeName;
    }

    public string GetSalesPerson(string strInvoiceId)
    {
        string Salesperson = string.Empty;

        try
        {
            Salesperson = objSinvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strInvoiceId).Rows[0]["EmployeeName"].ToString();
        }
        catch
        {


        }

        return Salesperson;
    }

    protected void lnkAddEmp_Click(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["RowIndex"] = Row.RowIndex;

        txtHandledEmp.Text = "";
        txtHandledEmp.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Serial_No_Modal_Show()", true);
    }

    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtHandledEmp.Text != "")
        {
            strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtHandledEmp.Text.Split('/')[1].ToString());
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtHandledEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmp);
            }
        }
        //AllPageCode();
    }

    protected void btnSavecommissionemp_Click(object sender, EventArgs e)
    {
        string EmployeeId = string.Empty;

        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtHandledEmp.Focus();
            return;
        }
        else
        {
            try
            {
                EmployeeId = HR_EmployeeDetail.GetEmployeeId(txtHandledEmp.Text.Split('/')[1].ToString());
            }
            catch
            {
                DisplayMessage("Select Employeee in suggestion only");
                txtHandledEmp.Text = "";
                txtHandledEmp.Focus();
                return;
            }
        }

        GridView gvchild = ((GridView)gvDetail.Rows[(int)ViewState["RowIndex"]].FindControl("gvEmp"));

        double comm_percentage = Convert.ToDouble(((TextBox)gvDetail.Rows[(int)ViewState["RowIndex"]].FindControl("txtper")).Text);

        double Emppercentage = comm_percentage / (gvchild.Rows.Count + 1);
        DataTable dtEmp = new DataTable();

        dtEmp.Columns.Add("Trans_Id", typeof(double));
        dtEmp.Columns.Add("Commission_Person");
        dtEmp.Columns.Add("Commission_Percentage", typeof(double));
        dtEmp.Columns.Add("Is_Paid", typeof(bool));
        dtEmp.Columns.Add("Paid_Date", typeof(DateTime));

        foreach (GridViewRow gvr in gvchild.Rows)
        {
            DataRow dr = dtEmp.NewRow();
            dr[0] = ((HiddenField)gvr.FindControl("hdnGvTransId")).Value;
            dr[1] = ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value;
            dr[2] = Emppercentage;
            dr[3] = ((Label)gvr.FindControl("lblispaid")).Text;
            if (((Label)gvr.FindControl("lblpaiddate")).Text == "")
            {
                dr[4] = "1/1/1900 12:00:00 AM";
            }
            else
            {
                dr[4] = ((Label)gvr.FindControl("lblpaiddate")).Text;
            }
            dtEmp.Rows.Add(dr);
        }


        if (new DataView(dtEmp, "Commission_Person=" + EmployeeId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
        {
            DisplayMessage("Employee Already exists");
            txtHandledEmp.Focus();
            return;
        }

        DataRow drNew = dtEmp.NewRow();
        try
        {
            drNew[0] = Convert.ToDouble(new DataView(dtEmp, "", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1;
        }
        catch
        {
            drNew[0] = "1";
        }
        drNew[1] = HR_EmployeeDetail.GetEmployeeId(txtHandledEmp.Text.Split('/')[1].ToString());
        drNew[2] = Emppercentage;
        drNew[3] = false.ToString();
        drNew[4] = "1/1/1900 12:00:00 AM";
        dtEmp.Rows.Add(drNew);
        gvchild.DataSource = dtEmp;
        gvchild.DataBind();

        gvEmployee.DataSource = GetEmployeeCommissionDetail();
        gvEmployee.DataBind();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Serial_No_Modal_Show()", true);
    }

    public DataTable getChildrecordinDatatatable()
    {
        DataTable dtEmp = new DataTable();

        dtEmp.Columns.Add("Voucher_No", typeof(double));
        dtEmp.Columns.Add("Trans_Id", typeof(double));
        dtEmp.Columns.Add("Commission_Person");
        dtEmp.Columns.Add("Commission_Percentage", typeof(double));
        dtEmp.Columns.Add("Is_Paid", typeof(bool));
        dtEmp.Columns.Add("Paid_Date", typeof(DateTime));

        foreach (GridViewRow gvrHeader in gvDetail.Rows)
        {
            GridView gvchild = ((GridView)gvrHeader.FindControl("gvEmp"));


            foreach (GridViewRow gvr in gvchild.Rows)
            {
                DataRow dr = dtEmp.NewRow();
                dr[0] = ((Label)gvrHeader.FindControl("lblTransId")).Text;
                dr[1] = ((HiddenField)gvr.FindControl("hdnGvTransId")).Value;
                dr[2] = ((HiddenField)gvr.FindControl("hdnGvEmpId")).Value;
                dr[3] = ((Label)gvr.FindControl("lblcommissionper")).Text;
                dr[4] = ((Label)gvr.FindControl("lblispaid")).Text;
                if (((Label)gvr.FindControl("lblpaiddate")).Text == "")
                {

                    dr[5] = "1/1/1900 12:00:00 AM";
                }
                else
                {
                    dr[5] = ((Label)gvr.FindControl("lblpaiddate")).Text;
                }
                dtEmp.Rows.Add(dr);
            }
        }
        return dtEmp;
    }

    public void setChildrecordinDatatatable(DataTable dtChild)
    {
        foreach (GridViewRow gvrHeader in gvDetail.Rows)
        {
            GridView gvchild = ((GridView)gvrHeader.FindControl("gvEmp"));
            Label lblTransId = ((Label)gvrHeader.FindControl("lblTransId"));
            if (new DataView(dtChild, "Voucher_No=" + lblTransId.Text + "", "Trans_Id", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {

                gvchild.DataSource = new DataView(dtChild, "Voucher_No=" + lblTransId.Text + "", "Trans_Id", DataViewRowState.CurrentRows).ToTable();
                gvchild.DataBind();
            }
            else
            {

                DataTable dtEmp = new DataTable();

                dtEmp.Columns.Add("Trans_Id", typeof(double));
                dtEmp.Columns.Add("Commission_Person");
                dtEmp.Columns.Add("Commission_Percentage", typeof(double));
                dtEmp.Columns.Add("Is_Paid", typeof(bool));
                dtEmp.Columns.Add("Paid_Date", typeof(DateTime));
                DataRow dr = dtEmp.NewRow();
                dr[0] = 1;

                if (txtSalesPerson.Text != "")
                {
                    dr[1] = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
                }
                else
                {
                    dr[1] = "0";
                }

                dr[2] = ((TextBox)gvrHeader.FindControl("txtper")).Text;
                dr[3] = false;
                dr[4] = "1/1/1900 12:00:00 AM";
                dtEmp.Rows.Add(dr);
                gvchild.DataSource = dtEmp;
                gvchild.DataBind();
            }
        }
    }
    #endregion
    #region Print


    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string objSenderID = string.Empty;

        if (sender is LinkButton)
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }
        else
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }


        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("VoucherNo");
        dtReport.Columns.Add("VoucherDate");
        dtReport.Columns.Add("Remarks");
        dtReport.Columns.Add("InvoiceNo");
        dtReport.Columns.Add("Invoicedate");
        dtReport.Columns.Add("ProductId");
        dtReport.Columns.Add("ProductName");
        dtReport.Columns.Add("CustomerName");
        dtReport.Columns.Add("Amount");
        dtReport.Columns.Add("Comm_percentage");
        dtReport.Columns.Add("Comm_Amount");
        dtReport.Columns.Add("EmployeeName");
        dtReport.Columns.Add("ReceivedAmt");
        dtReport.Columns.Add("PaidAmt");
        dtReport.Columns.Add("RemainAmt");
        dtReport.Columns.Add("FromDate");
        dtReport.Columns.Add("ToDate");
        dtReport.Columns.Add("SalesPerson");
        dtReport.Columns.Add("ReturnAmt");

        foreach (GridViewRow gvrow in gvDetail.Rows)
        {
            GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));



            foreach (GridViewRow gvrchild in gvchild.Rows)
            {
                if (((HiddenField)gvrchild.FindControl("hdnGvEmpId")).Value == e.CommandArgument.ToString())
                {
                    //for return 

                    if (objSenderID == "lblreturncom")
                    {
                        if (!Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text))
                        {
                            continue;
                        }
                    }
                    //for without return 
                    //if (objSenderID == "IbtnPrint")
                    //{
                    //    if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text))
                    //    {
                    //        continue;
                    //    }
                    //}


                    DataRow dr = dtReport.NewRow();
                    dr[0] = txtVoucherNo.Text;
                    dr[1] = txtVoucherDate.Text;
                    dr[2] = txtremarks.Text;
                    dr[3] = ((Label)gvrow.FindControl("lblInvoiceNo")).Text;
                    dr[4] = objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString();
                    dr[5] = ((Label)gvrow.FindControl("lblProuctCode")).Text;
                    dr[6] = ((Label)gvrow.FindControl("lblProuctName")).Text;
                    dr[7] = ((Label)gvrow.FindControl("lblcustomerName")).Text;
                    dr[8] = ((Label)gvrow.FindControl("lblAmt")).Text;
                    dr[9] = ((Label)gvrchild.FindControl("lblcommissionper")).Text;
                    dr[10] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                    dr[11] = GetEmployeeName(e.CommandArgument.ToString());

                    if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                    {
                        dr[12] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                    }
                    else
                    {
                        dr[12] = "0.000";
                    }
                    if (Convert.ToBoolean(((Label)gvrchild.FindControl("lblispaid")).Text))
                    {
                        dr[13] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                    }
                    else
                    {
                        dr[13] = "0.000";
                    }

                    if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                    {
                        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text))
                        {
                            dr[18] = getReturmAmount(((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, ((Label)gvrchild.FindControl("lblcommissionper")).Text);
                        }
                        else
                        {
                            dr[18] = "0.000";
                        }
                    }
                    else
                    {
                        dr[18] = "0.000";
                    }

                    if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                    {

                        if ((Convert.ToDouble(dr[12].ToString()) - Convert.ToDouble(dr[13].ToString()) - Convert.ToDouble(dr[18].ToString())) == 0)
                        {
                            dr[14] = "0.000";
                        }
                        else
                        {

                            dr[14] = SetDecimal((Convert.ToDouble(dr[12].ToString()) - Convert.ToDouble(dr[13].ToString()) - Convert.ToDouble(dr[18].ToString())).ToString());
                        }
                    }
                    else
                    {
                        dr[14] = "0.000";
                    }
                    dr[15] = txtfromdate.Text;
                    dr[16] = txttodate.Text;
                    dr[17] = ((Label)gvrow.FindControl("lblSalespersonName")).Text;



                    dtReport.Rows.Add(dr);
                }
            }
        }

        if (dtReport.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        Session["DtCommissionReport"] = dtReport;


        Session["DtCommissionReport_Range"] = "From " + Convert.ToDateTime(txtfromdate.Text).ToString(objSys.SetDateFormat()) + " To " + Convert.ToDateTime(txttodate.Text).ToString(objSys.SetDateFormat());


        string strCmd = string.Format("window.open('../Sales_Report/SalesCommission.aspx?Type=" + ddlType.SelectedValue.Trim() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    public string getReturmAmount(string strinvoiceId, string strProductId, string strCurrencyId, string strCommpercentage)
    {
        double ReturnCommisionAmt = 0;
        double ReturnAmount = 0;
        string strsql = "select sum(((Inv_SalesReturnDetail.UnitPrice*Inv_SalesReturnDetail.ReturnQty)-Inv_SalesReturnDetail.DiscountV+Inv_SalesReturnDetail.TaxV)) as TotalReturmAmount from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No where Inv_SalesReturnHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesReturnHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesReturnHeader.Location_Id=" + Session["LocId"].ToString() + " and Inv_SalesReturnHeader.Post='True'  and Inv_SalesReturnDetail.Invoice_No=" + strinvoiceId + " and Inv_SalesReturnDetail.Product_Id=" + strProductId + "";
        DataTable dt = objDa.return_DataTable(strsql);

        if (dt.Rows.Count > 0)
        {
            ReturnAmount = Convert.ToDouble(dt.Rows[0][0].ToString());
            double Exchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString()));
            double ProductReturmAmount = Convert.ToDouble(ReturnAmount) * Exchnagerate;
            double CommisionPercentage = Convert.ToDouble(strCommpercentage);
            ReturnCommisionAmt = (ProductReturmAmount * CommisionPercentage) / 100;
        }

        return SetDecimal(ReturnCommisionAmt.ToString());
    }


    #endregion
    #region Paymenthistory


    public void getpaymentHistory(string VoucherNo)
    {
        //string strsql = "WITH tb AS (SELECT Inv_SalesCommission_Employee_Detail.Commission_Person, Inv_SalesCommission_Employee_Detail.Commission_Amount, Inv_SalesCommission_Employee_Detail.Paid_Date, Inv_ReturnCommisison.Return_Amount FROM Inv_SalesCommission_Employee_Detail LEFT JOIN (SELECT SUM(Return_Amount) AS Return_Amount, Commision_Person, Voucher_Detail_Id, Is_Adjustable FROM Inv_ReturnCommisison GROUP BY Commision_Person, Voucher_Detail_Id, Is_Adjustable) Inv_ReturnCommisison ON Inv_SalesCommission_Employee_Detail.Voucher_No = Inv_ReturnCommisison.Voucher_Detail_Id AND Inv_ReturnCommisison.Commision_Person = Inv_SalesCommission_Employee_Detail.commission_person AND Inv_ReturnCommisison.Is_Adjustable = 'True' WHERE Voucher_No IN (SELECT Inv_SalesCommission_Detail.Trans_Id FROM Inv_SalesCommission_Detail WHERE Inv_SalesCommission_Detail.Voucher_No = " + VoucherNo + ") AND Is_Paid = 'True') SELECT Commission_Person, (ISNULL(SUM(Commission_Amount), 0) - ISNULL(SUM(Return_Amount), 0)) AS PaidAmount, Paid_Date FROM tb GROUP BY Commission_Person, Paid_Date";
        string strsql = string.Empty;
        strsql = "select Inv_SalesCommission_Employee_Detail.Commission_Person, Inv_SalesCommission_Employee_Detail.Paid_Date, (sum(Inv_SalesCommission_Employee_Detail.Commission_Amount)- (select ISNULL( SUM(Return_Amount),0) from Inv_ReturnCommisison where Voucher_Header_Id = " + VoucherNo + " and Inv_ReturnCommisison.Field1=Inv_SalesCommission_Employee_Detail.Finance_Voucher_No and Inv_ReturnCommisison.Commision_Person = Inv_SalesCommission_Employee_Detail.Commission_Person and Inv_ReturnCommisison.Is_Adjustable='True' and Inv_ReturnCommisison.Field1<>'')) as PaidAmount from Inv_SalesCommission_Employee_Detail WHERE Inv_SalesCommission_Employee_Detail.Voucher_No IN (SELECT Inv_SalesCommission_Detail.Trans_Id FROM Inv_SalesCommission_Detail WHERE Inv_SalesCommission_Detail.Voucher_No = " + VoucherNo + ") AND Inv_SalesCommission_Employee_Detail.Is_Paid = 'True' group by Paid_Date,Inv_SalesCommission_Employee_Detail.Commission_Person,Inv_SalesCommission_Employee_Detail.Finance_Voucher_No";
        //string strsql = "with tb as (  select Inv_SalesCommission_Employee_Detail.Commission_Person,Inv_SalesCommission_Employee_Detail.Commission_Amount,Inv_SalesCommission_Employee_Detail.Paid_Date,Inv_ReturnCommisison.Return_Amount  from Inv_SalesCommission_Employee_Detail      left join Inv_ReturnCommisison on Inv_SalesCommission_Employee_Detail.Voucher_No = Inv_ReturnCommisison.Voucher_Detail_Id and  Inv_ReturnCommisison.Commision_Person=Inv_SalesCommission_Employee_Detail.commission_person and Inv_ReturnCommisison.Is_Adjustable='True'    where  Voucher_No in (select Inv_SalesCommission_Detail.Trans_Id  from Inv_SalesCommission_Detail   where Inv_SalesCommission_Detail.Voucher_No=" + VoucherNo + ") and Is_Paid='True') select Commission_Person,(isnull(SUM(Commission_Amount),0) -isnull(SUM(Return_Amount),0)) as PaidAmount,Paid_Date from tb group by Commission_Person,Paid_Date";
        // string strsql = "with tb as (  select Inv_SalesCommission_Employee_Detail.Commission_Person,Inv_SalesCommission_Employee_Detail.Commission_Amount,Inv_SalesCommission_Employee_Detail.Paid_Date  from Inv_SalesCommission_Employee_Detail   where  Voucher_No in (select Inv_SalesCommission_Detail.Trans_Id  from Inv_SalesCommission_Detail where Voucher_No=" + VoucherNo + ") and Is_Paid='True') select Commission_Person,SUM(Commission_Amount) as PaidAmount,Paid_Date from tb group by Commission_Person,Paid_Date";

        DataTable dt = objDa.return_DataTable(strsql);

        gvPaymentHistory.DataSource = dt;
        gvPaymentHistory.DataBind();
    }
    //here we write code for print payment history 

    protected void IbtnPrintPaymentHistory_Command(object sender, CommandEventArgs e)
    {
        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("VoucherNo");
        dtReport.Columns.Add("VoucherDate");
        dtReport.Columns.Add("Remarks");
        dtReport.Columns.Add("InvoiceNo");
        dtReport.Columns.Add("Invoicedate");
        dtReport.Columns.Add("ProductId");
        dtReport.Columns.Add("ProductName");
        dtReport.Columns.Add("CustomerName");
        dtReport.Columns.Add("Amount");
        dtReport.Columns.Add("Comm_percentage");
        dtReport.Columns.Add("Comm_Amount");
        dtReport.Columns.Add("EmployeeName");
        dtReport.Columns.Add("ReceivedAmt");
        dtReport.Columns.Add("PaidAmt");
        dtReport.Columns.Add("RemainAmt");
        dtReport.Columns.Add("FromDate");
        dtReport.Columns.Add("ToDate");
        dtReport.Columns.Add("SalesPerson");
        dtReport.Columns.Add("ReturnAmt");
        foreach (GridViewRow gvrow in gvDetail.Rows)
        {
            GridView gvchild = ((GridView)gvrow.FindControl("gvEmp"));



            foreach (GridViewRow gvrchild in gvchild.Rows)
            {
                if (((HiddenField)gvrchild.FindControl("hdnGvEmpId")).Value == e.CommandArgument.ToString() && Convert.ToBoolean(((Label)gvrchild.FindControl("lblispaid")).Text))
                {

                    if (((Label)gvrchild.FindControl("lblpaiddate")).Text.Trim() == e.CommandName.ToString().Trim())
                    {


                        //if (Convert.ToBoolean(((Label)gvrchild.FindControl("lblispaid")).Text))
                        //{
                        DataRow dr = dtReport.NewRow();
                        dr[0] = txtVoucherNo.Text;
                        dr[1] = txtVoucherDate.Text;
                        dr[2] = "Commission Amount Paid on  '" + e.CommandName.ToString() + "'";
                        dr[3] = ((Label)gvrow.FindControl("lblInvoiceNo")).Text;
                        dr[4] = objSys.getDateForInput(((Label)gvrow.FindControl("lblInvoiceDate")).Text).ToString();
                        dr[5] = ((Label)gvrow.FindControl("lblProuctCode")).Text;
                        dr[6] = ((Label)gvrow.FindControl("lblProuctName")).Text;
                        dr[7] = ((Label)gvrow.FindControl("lblcustomerName")).Text;
                        dr[8] = ((Label)gvrow.FindControl("lblAmt")).Text;
                        dr[9] = ((Label)gvrchild.FindControl("lblcommissionper")).Text;
                        dr[10] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                        dr[11] = GetEmployeeName(e.CommandArgument.ToString());

                        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                        {
                            dr[12] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                        }
                        else
                        {
                            dr[12] = "0.000";
                        }
                        if (Convert.ToBoolean(((Label)gvrchild.FindControl("lblispaid")).Text))
                        {
                            dr[13] = SetDecimal(((Convert.ToDouble(((Label)gvrow.FindControl("lblAmt")).Text) * Convert.ToDouble(((Label)gvrchild.FindControl("lblcommissionper")).Text)) / 100).ToString());
                        }
                        else
                        {
                            dr[13] = "0.000";
                        }

                        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                        {
                            if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreturn")).Text))
                            {
                                dr[18] = getReturmAmount(((Label)gvrow.FindControl("lblInvoiceId")).Text, ((Label)gvrow.FindControl("lblproductId")).Text, ((Label)gvrow.FindControl("lblgvCurrencyId")).Text, ((Label)gvrchild.FindControl("lblcommissionper")).Text);
                            }
                            else
                            {
                                dr[18] = "0.000";
                            }
                        }
                        else
                        {
                            dr[18] = "0.000";
                        }



                        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblisreceived")).Text))
                        {
                            if ((Convert.ToDouble(dr[12].ToString()) - Convert.ToDouble(dr[13].ToString()) - Convert.ToDouble(dr[18].ToString())) == 0)
                            {
                                dr[14] = "0.000";
                            }
                            else
                            {

                                dr[14] = SetDecimal((Convert.ToDouble(dr[12].ToString()) - Convert.ToDouble(dr[13].ToString()) - Convert.ToDouble(dr[18].ToString())).ToString());
                            }
                        }
                        else
                        {
                            dr[14] = "0.000";
                        }

                        dr[15] = txtfromdate.Text;
                        dr[16] = txttodate.Text;
                        dr[17] = ((Label)gvrow.FindControl("lblSalespersonName")).Text;
                        dtReport.Rows.Add(dr);
                    }




                    //}
                }
            }
        }

        if (dtReport.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        Session["DtCommissionReport"] = dtReport;


        Session["DtCommissionReport_Range"] = "From " + Convert.ToDateTime(txtfromdate.Text).ToString(objSys.SetDateFormat()) + " To " + Convert.ToDateTime(txttodate.Text).ToString(objSys.SetDateFormat());


        string strCmd = string.Format("window.open('../Sales_Report/SalesCommission.aspx?Type=" + ddlType.SelectedValue.Trim() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }



    #endregion
    //code created for get local amount
    public string getLocalAmount(string strForeignAmount, string strforeignCurrency)
    {
        string strlocalAmount = string.Empty;

        string strExchnagerate = string.Empty;

        try
        {
            strExchnagerate = SystemParameter.GetExchageRate(strforeignCurrency, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        }
        catch
        {
            strExchnagerate = "1";
        }

        if (strExchnagerate == "")
        {
            strExchnagerate = "1";
        }

        strlocalAmount = objSys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(strForeignAmount) * Convert.ToDouble(strExchnagerate)).ToString());


        return strlocalAmount;
    }
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    { return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString()); }
    #region PaycommissionTab


    protected void btnGetPaymentdetail_Click(object sender, EventArgs e)
    {
        if (txtPaymentFromDate.Text == "")
        {
            DisplayMessage("Enter From date");
            txtPaymentFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtPaymentFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                txtPaymentFromDate.Focus();
                return;
            }
        }
        if (txtPaymentToDate.Text == "")
        {
            DisplayMessage("Enter To date");
            txtPaymentToDate.Focus();
            return;
        }
        else
        {

            try
            {
                Convert.ToDateTime(txtPaymentToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                txtPaymentToDate.Focus();
                return;
            }

        }

        if (Convert.ToDateTime(txtPaymentFromDate.Text) > Convert.ToDateTime(txtPaymentToDate.Text))
        {
            DisplayMessage("From Date should be less than to date ");
            txtfromdate.Focus();
            return;
        }

        if (txtCommissionPerson.Text == "")
        {
            DisplayMessage("Enter Commission Person");
            txtCommissionPerson.Focus();
            return;
        }


        //DataTable dtPaycommission = objsalesCommissionDetail.GetPaymentListforCommissionPerson(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPaymentFromDate.Text, txtPaymentToDate.Text, txtCommissionPerson.Text.Split('/')[1].ToString());


        //bool IsReturn = false;
        //bool IsReceived = false;
        //foreach (DataRow dr in dtPaycommission.Rows)
        //{
        //    IsReceived = Convert.ToBoolean(dr["Is_Receive"].ToString());
        //    IsReturn = Convert.ToBoolean(dr["IsReturn"].ToString());


        //    if (!Convert.ToBoolean(dr["Is_Receive"].ToString()))
        //    {

        //        //Invoice_Id

        //        //here we checking that if invoice payment is cash than no need to check that payment received or not 

        //        //code start

        //        string strPaymenttype = objSinvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["Invoice_Id"].ToString()).Rows[0]["PaymentType"].ToString();
        //        //code end


        //        //if payment received than we update sttaus of is received

        //        if (strPaymenttype.Trim() == "Credit")
        //        {

        //            DataTable dtAgeing = objDa.return_DataTable("select (max(Invoice_Amount)-sum(Paid_Receive_Amount)) as NetSum from ac_ageing_detail where Ref_Type='SINV' and Ref_Id=" + dr["Invoice_Id"].ToString() + "");


        //            if (dtAgeing.Rows.Count > 0)
        //            {
        //                if (dtAgeing.Rows[0]["NetSum"].ToString().Trim() != "")
        //                {

        //                    if (Convert.ToDouble(dtAgeing.Rows[0]["NetSum"].ToString()) <= 0)
        //                    {
        //                        //here we update isn received flag in database on basis of trans id 
        //                        IsReceived = true;
        //                    }
        //                }
        //            }
        //        }
        //        else if (strPaymenttype.Trim() == "Cash")
        //        {
        //            //here we update isn received flag in database on basis of trans id 
        //            IsReceived = true;
        //        }
        //    }

        //    //update return flag
        //    //here we set return flag in according return qty


        //    if (!Convert.ToBoolean(dr["IsReturn"].ToString()))
        //    {

        //        DataTable dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dr["Invoice_Id"].ToString());

        //        dtDetail = new DataView(dtDetail, "Product_Id=" + dr["Product_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();



        //        if (dtDetail.Rows[0]["ReturnQty"] != null)
        //        {
        //            try
        //            {
        //                if (Convert.ToDouble(dtDetail.Rows[0]["ReturnQty"].ToString()) > 0)
        //                {

        //                    IsReturn = true;

        //                }
        //            }
        //            catch
        //            {

        //            }
        //        }

        //    }

        //    //update return and receive status in database in commisison detail table on basis of trans idf 

        //    objsalesCommissionDetail.UpdateReceiveandReturnStatus(dr["DetailTransId"].ToString(), IsReceived.ToString(), IsReturn.ToString());


        //}


        //Inv_SalesCommission_Detail.Is_Receive='True' and Inv_SalesCommission_Detail.Field3='False'


        DataTable dtPaycommission = objsalesCommissionDetail.GetPendingPaymentListforCommissionPerson_(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPaymentFromDate.Text, txtPaymentToDate.Text, HR_EmployeeDetail.GetEmployeeId(txtCommissionPerson.Text.Split('/')[1].ToString()));


        object sumObject = 0;
        try
        {
            sumObject = dtPaycommission.Compute("Sum(Commission_Amount)", "");
        }
        catch
        {
            sumObject = 0;
        }

        txtTotalPaymentCommission.Text = SetDecimal(sumObject.ToString());
        DataTable dtReturnDetail = objReturnCommission.GetAllRecord_By_CommisisonPersonandIsadjustFlag(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HR_EmployeeDetail.GetEmployeeId(txtCommissionPerson.Text.Split('/')[1].ToString()), false.ToString());

        try
        {
            sumObject = dtReturnDetail.Compute("Sum(Return_Amount)", "");
        }
        catch
        {
            sumObject = 0;
        }
        txtTotalReturmAmount.Text = SetDecimal(sumObject.ToString());
        txtSelectedCommission.Text = "0";
        txtSelectedReturnAmount.Text = "0";
        txtNetpayCommision.Text = "0";
        txtNetpayCommisionLocal.Text = "0";
        gvPaymentdetail.DataSource = dtPaycommission;
        gvPaymentdetail.DataBind();
        gvReturndetail.DataSource = dtReturnDetail;
        gvReturndetail.DataBind();
        GetSetAccountDetail();

    }

    protected void txtCommissionPerson_TextChanged(object sender, EventArgs e)
    {

        TextBox txtCommPerson = (TextBox)sender;

        string strEmployeeId = string.Empty;
        if (txtCommPerson.Text != "")
        {
            strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtCommPerson.Text.Split('/')[1].ToString());
            if (strEmployeeId != "" && strEmployeeId != "0")
            {


            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtCommPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCommPerson);

            }
        }

    }
    protected void chkHeader_OnCheckedChanged(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(((CheckBox)gvPaymentdetail.HeaderRow.FindControl("chkHeader")).Checked))
        {
            txtSelectedCommission.Text = txtTotalPaymentCommission.Text;
        }
        else
        {
            txtSelectedCommission.Text = "0";

        }
        foreach (GridViewRow gvrow in gvPaymentdetail.Rows)
        {

            ((CheckBox)gvrow.FindControl("chkSelect")).Checked = ((CheckBox)gvPaymentdetail.HeaderRow.FindControl("chkHeader")).Checked;
        }

        if (txtSelectedReturnAmount.Text == "")
        {
            txtSelectedReturnAmount.Text = "0";
        }
        if (txtSelectedCommission.Text == "")
        {
            txtSelectedCommission.Text = "0";
        }

        txtNetpayCommision.Text = SetDecimal((Convert.ToDouble(txtSelectedCommission.Text) - Convert.ToDouble(txtSelectedReturnAmount.Text)).ToString());



        txtNetpayCommisionLocal.Text = GetAmountInEmployeeCurrency(txtNetpayCommision.Text);
    }

    public string GetAmountInEmployeeCurrency(string strNetCommission)
    {
        string strLocalAmount = "0";

        string strEmployeeCurrency = Common.GetEmployeeCurreny(HR_EmployeeDetail.GetEmployeeId(txtCommissionPerson.Text.Split('/')[1].ToString()), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());

        if (strEmployeeCurrency != Session["LocCurrencyId"].ToString())
        {

            strLocalAmount = (Convert.ToDouble(strNetCommission) * Convert.ToDouble(SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), strEmployeeCurrency, Session["DBConnection"].ToString()))).ToString();

            strLocalAmount = SystemParameter.GetCurrencySmbol(strEmployeeCurrency, objSys.GetCurencyConversionForInv(strEmployeeCurrency, strLocalAmount), Session["DBConnection"].ToString());
        }
        else
        {
            strLocalAmount = strNetCommission;
        }

        return strLocalAmount;
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        double NetAmount = 0;

        bool IsAllSelect = true;

        foreach (GridViewRow gvrow in gvPaymentdetail.Rows)
        {

            if (Convert.ToBoolean(((CheckBox)gvrow.FindControl("chkSelect")).Checked))
            {
                NetAmount += Convert.ToDouble(((Label)gvrow.FindControl("lblcommissionAmt")).Text);
            }
            else
            {

                IsAllSelect = false;
            }
        }

        ((CheckBox)gvPaymentdetail.HeaderRow.FindControl("chkHeader")).Checked = IsAllSelect;

        txtSelectedCommission.Text = SetDecimal(NetAmount.ToString());

        if (txtSelectedReturnAmount.Text == "")
        {
            txtSelectedReturnAmount.Text = "0";
        }

        txtNetpayCommision.Text = SetDecimal((Convert.ToDouble(txtSelectedCommission.Text) - Convert.ToDouble(txtSelectedReturnAmount.Text)).ToString());

        txtNetpayCommisionLocal.Text = GetAmountInEmployeeCurrency(txtNetpayCommision.Text);

    }

    protected void btnPayPaymentCommission_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());



        if (gvPaymentdetail.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }



        if (txtCommissionPerson.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtCommissionPerson.Focus();
            return;
        }


        bool isCommissionSelected = false;
        bool isReturnSelected = false;
        foreach (GridViewRow gvrow in gvPaymentdetail.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                isCommissionSelected = true;
                break;
            }
        }
        foreach (GridViewRow gvrow in gvReturndetail.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                isReturnSelected = true;
                break;
            }
        }

        if (!isCommissionSelected && !isReturnSelected)
        {
            DisplayMessage("Select record For commission");

            return;
        }

        if (txtpaymentdebitaccount.Text == "")
        {
            DisplayMessage("Fill Debit Account Value");
            return;
        }

        if (txtpaymentCreditaccount.Text == "")
        {
            DisplayMessage("Fill Credit Account Value");
            return;
        }


        string strReturnCommisson_Id = "";
        string strVMaxId = "0";

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            if (txtNetpayCommision.Text == "")
            {
                txtNetpayCommision.Text = "0";
            }

            double PaidTotalCommmissionamount = Convert.ToDouble(txtNetpayCommision.Text);


            //For Finance Entry
            string strvoucherno = string.Empty;

            foreach (GridViewRow gvr in gvPaymentdetail.Rows)
            {
                CheckBox chkSelectRecord = (CheckBox)gvr.FindControl("chkSelect");
                Label lblVoucherNo = (Label)gvr.FindControl("lblVoucherNo");

                if (chkSelectRecord.Checked == true)
                {
                    if (!strvoucherno.Split(',').Contains(lblVoucherNo.Text))
                    {
                        if (strvoucherno == "")
                        {
                            strvoucherno = lblVoucherNo.Text;
                        }
                        else
                        {
                            strvoucherno = strvoucherno + "," + lblVoucherNo.Text;
                        }
                    }
                }
            }

            //here we update is adjust flag true in return voucher table if record exists



            foreach (GridViewRow gvr in gvReturndetail.Rows)
            {
                CheckBox chkSelectRecord = (CheckBox)gvr.FindControl("chkSelect");
                Label lblTransId = (Label)gvr.FindControl("lblTransId");
                if (chkSelectRecord.Checked == true)
                {
                    strReturnCommisson_Id += lblTransId.Text + ",";
                    objReturnCommission.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblTransId.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

            }


            if (PaidTotalCommmissionamount != 0)
            {
                //For Bank Account
                string strAccountId = string.Empty;
                DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
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

                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


                //for Voucher Number
                string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "304", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {
                    DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
                    if (dtCount.Rows.Count > 0)
                    {
                        dtCount = new DataView(dtCount, "Voucher_Type='PV'", "", DataViewRowState.CurrentRows).ToTable();
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

                objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "PV", "1/1/1800", "1/1/1800", "", "Commission for '" + strvoucherno + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payment Voucher(" + txtNetpayCommisionLocal.Text + ") For Sales Commission for voucher number : '" + strvoucherno + "' and Commission person     is '" + txtCommissionPerson.Text.Split('/')[0].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                if (dtVMaxId.Rows.Count > 0)
                {
                    strVMaxId = dtVMaxId.Rows[0][0].ToString();
                }

                //str for Employee Id
                //For Debit
                string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
                string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "Commission for '" + strvoucherno + "' which is '" + txtCommissionPerson.Text.Split('/')[0].ToString() + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "Commission for '" + strvoucherno + "' which is '" + txtCommissionPerson.Text.Split('/')[0].ToString() + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "Commission for '" + strvoucherno + "' which is '" + txtCommissionPerson.Text.Split('/')[0].ToString() + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "Commission for '" + strvoucherno + "' which is '" + txtCommissionPerson.Text.Split('/')[0].ToString() + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }

            //get record from employee table


            //here we are updating finance voucher id in commission table

            //for Inv_SalesCommission_Employee_Detail table

            foreach (GridViewRow gvr in gvPaymentdetail.Rows)
            {
                CheckBox chkSelectRecord = (CheckBox)gvr.FindControl("chkSelect");

                if (chkSelectRecord.Checked == true)
                {
                    //here we update is paid sttaus in employee detail table 

                    objcommEmployeeDetail.UpdateRecord(((Label)gvr.FindControl("lblTransId")).Text, true.ToString(), DateTime.Now.ToString(), strVMaxId, ref trns);

                }
            }


            //for update finanve voucher id in return table

            if (strReturnCommisson_Id != "")
            {
                objDa.execute_Command("update Inv_ReturnCommisison set Field1='" + strVMaxId + "' where trans_id in (" + strReturnCommisson_Id.Substring(0, strReturnCommisson_Id.Length - 1) + ") ", ref trns);
            }



            //updatre header amount


            DisplayMessage("Commission paid successfully");

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            ResetPaymentPanel();

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


    protected void btnPayCommissionreset_Click(object sender, EventArgs e)
    {

        ResetPaymentPanel();
    }

    public void ResetPaymentPanel()
    {
        txtPaymentFromDate.Text = "";
        txtCommissionPerson.Text = "";
        objPageCmn.FillData((object)gvPaymentdetail, null, "", "");
        objPageCmn.FillData((object)gvReturndetail, null, "", "");
        txtTotalPaymentCommission.Text = "0";
        txtSelectedCommission.Text = "0";
        txtTotalReturmAmount.Text = "0";
        txtSelectedReturnAmount.Text = "0";
        txtNetpayCommision.Text = "0";
        txtNetpayCommisionLocal.Text = "0";
        GetSetAccountDetail();
    }



    protected void chkReturnHeader_OnCheckedChanged(object sender, EventArgs e)
    {

        if (Convert.ToBoolean(((CheckBox)gvReturndetail.HeaderRow.FindControl("chkHeader")).Checked))
        {
            txtSelectedReturnAmount.Text = txtTotalReturmAmount.Text;
        }
        else
        {
            txtSelectedReturnAmount.Text = "0";

        }
        foreach (GridViewRow gvrow in gvReturndetail.Rows)
        {

            ((CheckBox)gvrow.FindControl("chkSelect")).Checked = ((CheckBox)gvReturndetail.HeaderRow.FindControl("chkHeader")).Checked;
        }

        if (txtSelectedReturnAmount.Text == "")
        {
            txtSelectedReturnAmount.Text = "0";
        }
        if (txtSelectedCommission.Text == "")
        {
            txtSelectedCommission.Text = "0";
        }

        txtNetpayCommision.Text = SetDecimal((Convert.ToDouble(txtSelectedCommission.Text) - Convert.ToDouble(txtSelectedReturnAmount.Text)).ToString());


        txtNetpayCommisionLocal.Text = GetAmountInEmployeeCurrency(txtNetpayCommision.Text);
    }

    protected void chkReturnSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        double NetAmount = 0;

        bool IsAllSelect = true;

        foreach (GridViewRow gvrow in gvReturndetail.Rows)
        {

            if (Convert.ToBoolean(((CheckBox)gvrow.FindControl("chkSelect")).Checked))
            {
                NetAmount += Convert.ToDouble(((Label)gvrow.FindControl("lblreturmAmount")).Text);
            }
            else
            {

                IsAllSelect = false;
            }
        }

        ((CheckBox)gvReturndetail.HeaderRow.FindControl("chkHeader")).Checked = IsAllSelect;

        txtSelectedReturnAmount.Text = SetDecimal(NetAmount.ToString());

        if (txtSelectedReturnAmount.Text == "")
        {
            txtSelectedReturnAmount.Text = "0";
        }
        if (txtSelectedCommission.Text == "")
        {
            txtSelectedCommission.Text = "0";
        }

        txtNetpayCommision.Text = SetDecimal((Convert.ToDouble(txtSelectedCommission.Text) - Convert.ToDouble(txtSelectedReturnAmount.Text)).ToString());

        txtNetpayCommisionLocal.Text = GetAmountInEmployeeCurrency(txtNetpayCommision.Text);

    }
    #endregion
    #region commissionreporttab


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True'  and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {

        }

        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString();
            }
        }
        return filterlist;

    }

    protected void btnPayPaymentCommissionReport_Click(object sender, EventArgs e)
    {
        if (txtPaymentReportFromDate.Text != "")
        {

            try
            {
                Convert.ToDateTime(txtPaymentReportFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                txtPaymentReportFromDate.Focus();
                return;
            }

        }
        if (txtPaymentReportToDate.Text != "")
        {

            try
            {
                Convert.ToDateTime(txtPaymentReportToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                txtPaymentReportToDate.Focus();
                return;
            }

        }

        if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
        {
            if (Convert.ToDateTime(txtPaymentReportFromDate.Text) > Convert.ToDateTime(txtPaymentReportToDate.Text))
            {
                DisplayMessage("From Date should be less than to date ");
                txtPaymentReportFromDate.Focus();
                return;
            }
        }

        DataTable dtCommisison = new DataTable();
        String setDateCriteria = string.Empty;
        string strVoucherType = string.Empty;
        if (HeaderReportVoucher_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_VoucherReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_VoucherReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Header_VoucherReport, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Header_VoucherReport;



            //if enter from date and to date 

            if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
            {

                dtCommisison = new DataView(dtCommisison, "From_Date>='" + txtPaymentReportFromDate.Text + "' and To_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
            }



            //if enter commission person 

            if (txtCommissionReportPerson.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "CommissionPerson='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by paid and pending payment 
            if (PendingReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "TotalUnpaidBalace<>'0'", "", DataViewRowState.CurrentRows).ToTable();


            }
            else if (PaidVoucherReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Total_Paid_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by Voucher no.

            if (TextReportVoucherNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_No='" + TextReportVoucherNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            // Filter by commission Type

            if (ddlCommissionReport.SelectedIndex > 0)
            {
                dtCommisison = new DataView(dtCommisison, "CommissionType='" + ddlCommissionReport.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            strVoucherType = "HV";
        }

        if (HeaderReportInvoice_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_InvoiceReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_InvoiceReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Header_InvoiceReport, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Header_InvoiceReport;



            //if enter from date and to date 

            if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_Date>='" + txtPaymentReportFromDate.Text + "' and Invoice_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
            }

            //here we filter by Customer Name

            if (txtReportcustomerName.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "CustomerName='" + txtReportcustomerName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            //if enter Invoice Number 

            if (txtInvoiceNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            strVoucherType = "HI";

        }


        if (DetailReportCustomer_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Detail_Report;



            //if enter from date and to date 

            if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_Date>='" + txtPaymentReportFromDate.Text + "' and Voucher_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
            }

            //if enter commission person 

            if (txtCommissionReportPerson.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            //if enter Invoice Number 

            if (txtInvoiceNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by paid and pending payment 
            if (PendingReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='False'", "", DataViewRowState.CurrentRows).ToTable();


            }
            else if (PaidVoucherReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='True'", "", DataViewRowState.CurrentRows).ToTable();

            }

            //here we filter by Voucher no.

            if (TextReportVoucherNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_No='" + TextReportVoucherNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by Customer Name

            if (txtReportcustomerName.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "CustomerName='" + txtReportcustomerName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            strVoucherType = "DC";

        }


        if (DetailReportVoucher_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Detail_Report;



            //if enter from date and to date 

            if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_Date>='" + txtPaymentReportFromDate.Text + "' and Voucher_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
            }

            //if enter Invoice Number 

            if (txtInvoiceNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }



            //if enter commission person 

            if (txtCommissionReportPerson.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by paid and pending payment 
            if (PendingReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='False'", "", DataViewRowState.CurrentRows).ToTable();


            }
            else if (PaidVoucherReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='True'", "", DataViewRowState.CurrentRows).ToTable();

            }

            //here we filter by Voucher no.

            if (TextReportVoucherNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_No='" + TextReportVoucherNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by Customer Name

            if (txtReportcustomerName.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "CustomerName='" + txtReportcustomerName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            strVoucherType = "DV";

        }

        if (DetailReportInvoice_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Detail_Report;



            //if enter from date and to date 

            if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_Date>='" + txtPaymentReportFromDate.Text + "' and Invoice_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
            }

            //if enter Invoice Number 

            if (txtInvoiceNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //if enter commission person 

            if (txtCommissionReportPerson.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by paid and pending payment 
            if (PendingReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='False'", "", DataViewRowState.CurrentRows).ToTable();


            }
            else if (PaidVoucherReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "Is_Paid='True'", "", DataViewRowState.CurrentRows).ToTable();

            }

            //here we filter by Voucher no.

            if (TextReportVoucherNo.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Voucher_No='" + TextReportVoucherNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by Customer Name

            if (txtReportcustomerName.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "CustomerName='" + txtReportcustomerName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            strVoucherType = "DI";

        }

        if (SummaryReport_Rb.Checked)
        {
            SalesDataSet objSalesDataSet = new SalesDataSet();
            objSalesDataSet.EnforceConstraints = false;

            SalesDataSetTableAdapters.sp_Inv_SalesCommission_Summary_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Summary_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Summary_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

            dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Summary_Report;



            //if enter commission person 

            if (txtCommissionReportPerson.Text != "")
            {
                dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //here we filter by paid and pending payment 
            if (PendingReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "RemainAmount<>'0'", "", DataViewRowState.CurrentRows).ToTable();


            }
            else if (PaidVoucherReport_Rb.Checked)
            {
                dtCommisison = new DataView(dtCommisison, "PaidAmount<>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            strVoucherType = "SR";


        }


        ArrayList objarr = new ArrayList();

        objarr.Add(dtCommisison);
        objarr.Add(setDateCriteria);
        objarr.Add(txtCommissionReportPerson.Text.Split('/')[0].ToString());

        Session["dtCommissionreport"] = objarr;


        string strCmd = string.Format("window.open('../Sales_Report/SalesCommHeaderVoucherReport.aspx?ReportType=" + strVoucherType + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void btnPayCommissionReportreset_Click(object sender, EventArgs e)
    {
        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";

        PendingReport_Rb.Checked = false;
        PaidVoucherReport_Rb.Checked = false;
        AllReport_Rb.Checked = true;

        HeaderReportVoucher_Rb.Checked = true;
        HeaderReportInvoice_Rb.Checked = false;
        DetailReportVoucher_Rb.Checked = false;
        DetailReportInvoice_Rb.Checked = false;
        DetailReportCustomer_Rb.Checked = false;
        SummaryReport_Rb.Checked = false;


    }
    public void HeaderReport()
    {
        txtInvoiceNo.Enabled = false;
        txtCommissionReportPerson.Enabled = true;
        ddlCommissionReport.Enabled = true;
        TextReportVoucherNo.Enabled = true;
        txtPaymentReportToDate.Enabled = true;
        txtPaymentReportFromDate.Enabled = true;
        txtReportcustomerName.Enabled = true;
        AllReport_Rb.Enabled = true;
        PendingReport_Rb.Enabled = true;
        PaidVoucherReport_Rb.Enabled = true;
        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";


    }
    protected void HeaderReportVoucher_CheckedChanged(object sender, EventArgs e)
    {
        HeaderReport();
    }

    protected void HeaderReportInvoice_CheckedChanged(object sender, EventArgs e)
    {
        txtInvoiceNo.Enabled = true;
        txtCommissionReportPerson.Enabled = false;
        ddlCommissionReport.Enabled = false;
        TextReportVoucherNo.Enabled = false;
        txtPaymentReportToDate.Enabled = true;
        txtPaymentReportFromDate.Enabled = true;
        txtReportcustomerName.Enabled = true;
        AllReport_Rb.Enabled = false;
        PendingReport_Rb.Enabled = false;
        PaidVoucherReport_Rb.Enabled = false;

        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";

    }

    protected void DetailReportCustomer_CheckedChanged(object sender, EventArgs e)
    {
        txtInvoiceNo.Enabled = true;
        txtCommissionReportPerson.Enabled = true;
        ddlCommissionReport.Enabled = false;
        TextReportVoucherNo.Enabled = true;
        txtPaymentReportToDate.Enabled = true;
        txtPaymentReportFromDate.Enabled = true;
        txtReportcustomerName.Enabled = true;
        AllReport_Rb.Enabled = true;
        PendingReport_Rb.Enabled = true;
        PaidVoucherReport_Rb.Enabled = true;

        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";
    }

    protected void DetailReportVoucher_CheckedChanged(object sender, EventArgs e)
    {
        txtInvoiceNo.Enabled = true;
        txtCommissionReportPerson.Enabled = true;
        ddlCommissionReport.Enabled = false;
        TextReportVoucherNo.Enabled = true;
        txtPaymentReportToDate.Enabled = true;
        txtPaymentReportFromDate.Enabled = true;
        txtReportcustomerName.Enabled = true;
        AllReport_Rb.Enabled = true;
        PendingReport_Rb.Enabled = true;
        PaidVoucherReport_Rb.Enabled = true;

        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";
    }

    protected void DetailReportInvoice_CheckedChanged(object sender, EventArgs e)
    {
        txtInvoiceNo.Enabled = true;
        txtCommissionReportPerson.Enabled = true;
        ddlCommissionReport.Enabled = false;
        TextReportVoucherNo.Enabled = true;
        txtPaymentReportToDate.Enabled = true;
        txtPaymentReportFromDate.Enabled = true;
        txtReportcustomerName.Enabled = true;
        AllReport_Rb.Enabled = true;
        PendingReport_Rb.Enabled = true;
        PaidVoucherReport_Rb.Enabled = true;

        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";
    }

    protected void SummaryReport_CheckedChanged(object sender, EventArgs e)
    {
        txtInvoiceNo.Enabled = false;
        txtCommissionReportPerson.Enabled = true;
        ddlCommissionReport.Enabled = false;
        TextReportVoucherNo.Enabled = false;
        txtPaymentReportToDate.Enabled = false;
        txtPaymentReportFromDate.Enabled = false;
        txtReportcustomerName.Enabled = false;
        AllReport_Rb.Enabled = true;
        PendingReport_Rb.Enabled = true;
        PaidVoucherReport_Rb.Enabled = true;

        txtPaymentReportFromDate.Text = "";
        txtPaymentReportToDate.Text = "";
        txtCommissionReportPerson.Text = "";
        txtInvoiceNo.Text = "";
        TextReportVoucherNo.Text = "";
        txtReportcustomerName.Text = "";
    }




    #endregion
    //#region commissionreporttab

    //protected void btnPaymentReport_Click(object sender, EventArgs e)
    //{
    //    pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
    //    pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
    //    pnlMenyPayment.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
    //    pnlMenyPaymentReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
    //    pnlMenyConfiguration.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
    //    pnlConfiguration.Visible = false;
    //    pnlPaymentCommissionReport.Visible = true;
    //    pnlPaymentCommission.Visible = false;
    //    PnlList.Visible = false;
    //    PnlNewEdit.Visible = false;



    //}

    //protected void btnPayPaymentCommissionReport_Click(object sender, EventArgs e)
    //{
    //    if (txtPaymentReportFromDate.Text != "")
    //    {

    //        try
    //        {
    //            Convert.ToDateTime(txtPaymentReportFromDate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Valid Date");
    //            txtPaymentReportFromDate.Focus();
    //            return;
    //        }

    //    }
    //    if (txtPaymentReportToDate.Text != "")
    //    {

    //        try
    //        {
    //            Convert.ToDateTime(txtPaymentReportToDate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Valid Date");
    //            txtPaymentReportToDate.Focus();
    //            return;
    //        }

    //    }

    //    if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
    //    {
    //        if (Convert.ToDateTime(txtPaymentReportFromDate.Text) > Convert.ToDateTime(txtPaymentReportToDate.Text))
    //        {
    //            DisplayMessage("From Date should be less than to date ");
    //            txtPaymentReportFromDate.Focus();
    //            return;
    //        }
    //    }

    //    DataTable dtCommisison = new DataTable();
    //    String setDateCriteria = string.Empty;
    //    string strVoucherType = string.Empty;
    //    if (HeaderReportVoucher_Rb.Checked)
    //    {
    //        SalesDataSet objSalesDataSet = new SalesDataSet();
    //        objSalesDataSet.EnforceConstraints = false;

    //        SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_VoucherReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_VoucherReportTableAdapter();

    //        adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Header_VoucherReport, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

    //        dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Header_VoucherReport;



    //        //if enter from date and to date 

    //        if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
    //        {

    //            dtCommisison = new DataView(dtCommisison, "From_Date>='" + txtPaymentReportFromDate.Text + "' and To_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //            setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
    //        }



    //        //if enter commission person 

    //        if (txtCommissionReportPerson.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "CommissionPerson='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        //here we filter by paid and pending payment 
    //        if (PendingReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "TotalUnpaidBalace<>'0'", "", DataViewRowState.CurrentRows).ToTable();


    //        }
    //        else if (PaidVoucherReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Total_Paid_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        strVoucherType = "HV";
    //    }

    //    if (HeaderReportInvoice_Rb.Checked)
    //    {
    //        SalesDataSet objSalesDataSet = new SalesDataSet();
    //        objSalesDataSet.EnforceConstraints = false;

    //        SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_InvoiceReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Header_InvoiceReportTableAdapter();

    //        adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Header_InvoiceReport, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

    //        dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Header_InvoiceReport;



    //        //if enter from date and to date 

    //        if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Invoice_Date>='" + txtPaymentReportFromDate.Text + "' and Invoice_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //            setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
    //        }




    //        //if enter Invoice Number 

    //        if (txtInvoiceNo.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        strVoucherType = "HI";

    //    }

    //    if (DetailReportVoucher_Rb.Checked)
    //    {
    //        SalesDataSet objSalesDataSet = new SalesDataSet();
    //        objSalesDataSet.EnforceConstraints = false;

    //        SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter();

    //        adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

    //        dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Detail_Report;



    //        //if enter from date and to date 

    //        if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Voucher_Date>='" + txtPaymentReportFromDate.Text + "' and Voucher_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //            setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
    //        }

    //        //if enter Invoice Number 

    //        if (txtInvoiceNo.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }



    //        //if enter commission person 

    //        if (txtCommissionReportPerson.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        //here we filter by paid and pending payment 
    //        if (PendingReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Is_Paid='False'", "", DataViewRowState.CurrentRows).ToTable();


    //        }
    //        else if (PaidVoucherReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Is_Paid='True'", "", DataViewRowState.CurrentRows).ToTable();

    //        }

    //        strVoucherType = "DV";

    //    }

    //    if (DetailReportInvoice_Rb.Checked)
    //    {
    //        SalesDataSet objSalesDataSet = new SalesDataSet();
    //        objSalesDataSet.EnforceConstraints = false;

    //        SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Detail_ReportTableAdapter();

    //        adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

    //        dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Detail_Report;



    //        //if enter from date and to date 

    //        if (txtPaymentReportFromDate.Text != "" && txtPaymentReportToDate.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Invoice_Date>='" + txtPaymentReportFromDate.Text + "' and Invoice_Date<='" + txtPaymentReportToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //            setDateCriteria = "From : " + txtPaymentReportFromDate.Text + " To : " + txtPaymentReportToDate.Text;
    //        }

    //        //if enter Invoice Number 

    //        if (txtInvoiceNo.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Invoice_No='" + txtInvoiceNo.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        //if enter commission person 

    //        if (txtCommissionReportPerson.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        //here we filter by paid and pending payment 
    //        if (PendingReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Is_Paid='False'", "", DataViewRowState.CurrentRows).ToTable();


    //        }
    //        else if (PaidVoucherReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Is_Paid='True'", "", DataViewRowState.CurrentRows).ToTable();

    //        }

    //        strVoucherType = "DI";

    //    }

    //    if (SummaryReport_Rb.Checked)
    //    {
    //        SalesDataSet objSalesDataSet = new SalesDataSet();
    //        objSalesDataSet.EnforceConstraints = false;

    //        SalesDataSetTableAdapters.sp_Inv_SalesCommission_Summary_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesCommission_Summary_ReportTableAdapter();

    //        adp.Fill(objSalesDataSet.sp_Inv_SalesCommission_Summary_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));

    //        dtCommisison = objSalesDataSet.sp_Inv_SalesCommission_Summary_Report;



    //        //if enter commission person 

    //        if (txtCommissionReportPerson.Text != "")
    //        {
    //            dtCommisison = new DataView(dtCommisison, "Commission_Person='" + txtCommissionReportPerson.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        //here we filter by paid and pending payment 
    //        if (PendingReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "RemainAmount<>'0'", "", DataViewRowState.CurrentRows).ToTable();


    //        }
    //        else if (PaidVoucherReport_Rb.Checked)
    //        {
    //            dtCommisison = new DataView(dtCommisison, "PaidAmount<>0", "", DataViewRowState.CurrentRows).ToTable();
    //        }

    //        strVoucherType = "SR";

    //    }


    //    ArrayList objarr = new ArrayList();

    //    objarr.Add(dtCommisison);
    //    objarr.Add(setDateCriteria);
    //    objarr.Add(txtCommissionReportPerson.Text.Split('/')[0].ToString());

    //    Session["dtCommissionreport"] = objarr;


    //    string strCmd = string.Format("window.open('../Sales_Report/SalesCommHeaderVoucherReport.aspx?ReportType=" + strVoucherType + "','window','width=1024, ');");
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    //}
    //protected void btnPayCommissionReportreset_Click(object sender, EventArgs e)
    //{
    //    txtPaymentReportFromDate.Text = "";
    //    txtPaymentReportToDate.Text = "";
    //    txtCommissionReportPerson.Text = "";
    //    txtInvoiceNo.Text = "";

    //    PendingReport_Rb.Checked = false;
    //    PaidVoucherReport_Rb.Checked = false;
    //    AllReport_Rb.Checked = true;

    //    HeaderReportVoucher_Rb.Checked = true;
    //    HeaderReportInvoice_Rb.Checked = false;
    //    DetailReportVoucher_Rb.Checked = false;
    //    DetailReportInvoice_Rb.Checked = false;
    //    SummaryReport_Rb.Checked = false;
    //}
    //#endregion
    #region Configuration

    protected void btnConfiguration_Click(object sender, EventArgs e)
    {
        FillProductCategorySerch();
    }
    public void FillProductCategorySerch()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        if (dsCategory.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddlcategorysearch.Items.Insert(0, "--Select One--");
            ddlcategorysearch.SelectedIndex = 0;
        }
    }
    protected void btnsaveConfiguration_Click(object sender, EventArgs e)
    {
        if (txtCommissionPersonConfig.Text == "")
        {
            DisplayMessage("Enter Sales Person");
            txtCommissionPersonConfig.Focus();
            return;
        }

        if (txtTotalSalesConfig.Text == "")
        {
            txtTotalSalesConfig.Text = "0";
        }
        if (txtCommPercentageConfig.Text == "")
        {
            txtCommPercentageConfig.Text = "0";
        }

        if (ddlParameterLevel.SelectedIndex == 1)
        {
            if (gvCategorySalesConfiguration.Rows.Count == 0)
            {
                DisplayMessage("Add at least one category");
                return;
            }
        }
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (hdnConfigurationId.Value == "")
            {
                int b = objConfigHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", HR_EmployeeDetail.GetEmployeeId(txtCommissionPersonConfig.Text.Split('/')[1].ToString()), ddlParameterLevel.SelectedValue, txtNotesConfig.Text, "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (ddlParameterLevel.SelectedIndex == 0)
                {
                    objConfigDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), "0", txtTotalSalesConfig.Text, txtCommPercentageConfig.Text, chkIsAllowCommission.Checked.ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }
                else
                {
                    foreach (GridViewRow gvrow in gvCategorySalesConfiguration.Rows)
                    {
                        objConfigDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), ((HiddenField)gvrow.FindControl("hdncategoryid")).Value, ((Label)gvrow.FindControl("lblSalesquota")).Text, ((Label)gvrow.FindControl("lblconfigPer")).Text, ((Label)gvrow.FindControl("lblAllow")).Text, "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    }
                }

                DisplayMessage("Record Saved", "green");

            }
            else
            {
                objConfigHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnConfigurationId.Value, "0", HR_EmployeeDetail.GetEmployeeId(txtCommissionPersonConfig.Text.Split('/')[1].ToString()), ddlParameterLevel.SelectedValue, txtNotesConfig.Text, "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //delete record in detail table and reinsert
                objConfigDetail.DeleteRecord_By_RefferencId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnConfigurationId.Value, ref trns);
                if (ddlParameterLevel.SelectedIndex == 0)
                {
                    objConfigDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnConfigurationId.Value, "0", txtTotalSalesConfig.Text, txtCommPercentageConfig.Text, chkIsAllowCommission.Checked.ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }
                else
                {
                    foreach (GridViewRow gvrow in gvCategorySalesConfiguration.Rows)
                    {

                        objConfigDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnConfigurationId.Value, ((HiddenField)gvrow.FindControl("hdncategoryid")).Value, ((Label)gvrow.FindControl("lblSalesquota")).Text, ((Label)gvrow.FindControl("lblconfigPer")).Text, ((Label)gvrow.FindControl("lblAllow")).Text, "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    }
                }


                DisplayMessage("Record Updated", "green");

            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnResetConfiguration_Click(null, null);
            txtCommissionPersonConfig.Focus();
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

    protected void btnResetConfiguration_Click(object sender, EventArgs e)
    {
        hdnConfigurationId.Value = "";
        txtCommissionPersonConfig.Text = "";
        ddlParameterLevel.SelectedIndex = 0;
        txtNotesConfig.Text = "";
        ResetConbfigurationPanel();
    }
    public void ResetConbfigurationPanel()
    {

        objPageCmn.FillData((object)gvCategorySalesConfiguration, null, "", "");
        Session["DtCategorySalesQuota"] = null;
        ddlcategorysearch.SelectedIndex = 0;
        txtTotalSalesConfig.Text = "";
        txtCommPercentageConfig.Text = "";
        chkIsAllowCommission.Checked = false;
        if (ddlParameterLevel.SelectedIndex == 0)
        {
            trCategory.Visible = false;
            pnlImgConfigDetailSave.Visible = false;
        }
        else
        {
            trCategory.Visible = true;
            pnlImgConfigDetailSave.Visible = true;

        }
    }



    protected void ImgConfigDetailSave_Click(object sender, EventArgs e)
    {
        if (ddlcategorysearch.SelectedIndex <= 0)
        {
            DisplayMessage("Select Category Name");
            ddlcategorysearch.Focus();
            return;
        }
        if (txtTotalSalesConfig.Text == "")
        {
            txtTotalSalesConfig.Text = "0";
        }
        if (txtCommPercentageConfig.Text == "")
        {
            txtCommPercentageConfig.Text = "0";
        }


        DataTable dtSalesQuota = new DataTable();

        if (Session["DtCategorySalesQuota"] == null)
        {
            dtSalesQuota = CreateCategoryCommissionDatatable();

            dtSalesQuota.Rows.Add(1, ddlcategorysearch.SelectedValue, ddlcategorysearch.SelectedItem.Text, txtTotalSalesConfig.Text, txtCommPercentageConfig.Text, chkIsAllowCommission.Checked);
        }
        else
        {
            dtSalesQuota = (DataTable)Session["DtCategorySalesQuota"];
            DataTable dtTemp = new DataTable();

            try
            {
                dtTemp = new DataView(dtSalesQuota, "Category_Id=" + ddlcategorysearch.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
            if (dtTemp.Rows.Count > 0)
            {
                DisplayMessage("Category already exists");
                ddlcategorysearch.Focus();
                return;
            }

            float srNo = 0;
            try
            {
                srNo = float.Parse(new DataView(dtSalesQuota, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1;
            }
            catch
            {
                srNo = 1;
            }

            dtSalesQuota.Rows.Add(srNo, ddlcategorysearch.SelectedValue, ddlcategorysearch.SelectedItem.Text, txtTotalSalesConfig.Text, txtCommPercentageConfig.Text, chkIsAllowCommission.Checked);

        }
        objPageCmn.FillData((object)gvCategorySalesConfiguration, dtSalesQuota, "", "");
        Session["DtCategorySalesQuota"] = dtSalesQuota;
        ddlcategorysearch.SelectedIndex = 0;
        txtTotalSalesConfig.Text = "";
        txtCommPercentageConfig.Text = "";
        chkIsAllowCommission.Checked = false;
    }

    protected void IbtnDeleteConfig_Command(object sender, CommandEventArgs e)
    {
        DataTable dtSalesQuota = new DataView((DataTable)Session["DtCategorySalesQuota"], "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)gvCategorySalesConfiguration, dtSalesQuota, "", "");

        Session["DtCategorySalesQuota"] = dtSalesQuota;

    }
    protected void ddlParameterLevel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ResetConbfigurationPanel();
    }

    protected void txtCommissionPersonConfig_TextChanged(object sender, EventArgs e)
    {
        string strSender = ((TextBox)sender).Text;
        btnResetConfiguration_Click(null, null);

        string strEmployeeId = string.Empty;
        if (strSender != "")
        {
            txtCommissionPersonConfig.Text = strSender;
            strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtCommissionPersonConfig.Text.Split('/')[1].ToString());
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

                //if record exist in configuration table than select and display record

                DataTable dt = objConfigHeader.GetRecord_By_BySalesPerson(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmployeeId);
                if (dt.Rows.Count > 0)
                {
                    hdnConfigurationId.Value = dt.Rows[0]["Trans_Id"].ToString();
                    ddlParameterLevel.SelectedValue = dt.Rows[0]["Parameter_Level"].ToString().Trim();
                    txtNotesConfig.Text = dt.Rows[0]["Field1"].ToString();
                    ddlParameterLevel_OnSelectedIndexChanged(null, null);
                    //now weget record from detail table

                    DataTable dtDetail = objConfigDetail.GetRecord_By_RefferencId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnConfigurationId.Value);

                    if (dtDetail.Rows.Count > 0)
                    {
                        if (ddlParameterLevel.SelectedIndex == 0)
                        {
                            txtTotalSalesConfig.Text = SetDecimal(dtDetail.Rows[0]["Sales_Quota"].ToString());
                            txtCommPercentageConfig.Text = SetDecimal(dtDetail.Rows[0]["Commission_Percentage"].ToString());
                            chkIsAllowCommission.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Allow"].ToString());
                        }
                        else
                        {
                            dtDetail = dtDetail.DefaultView.ToTable(true, "Trans_Id", "Category_Id", "Category_Name", "Sales_Quota", "Commission_Percentage", "Is_Allow");

                            objPageCmn.FillData((object)gvCategorySalesConfiguration, dtDetail, "", "");

                            Session["DtCategorySalesQuota"] = dtDetail;
                        }
                    }

                }
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtCommissionPersonConfig.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCommissionPersonConfig);

            }
        }

    }

    public DataTable CreateCategoryCommissionDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Category_Id", typeof(int));
        dt.Columns.Add("Category_Name", typeof(string));
        dt.Columns.Add("Sales_Quota", typeof(double));
        dt.Columns.Add("Commission_Percentage", typeof(double));
        dt.Columns.Add("Is_Allow", typeof(bool));
        return dt;
    }



    #endregion
    public DataTable Commission_Function(DataTable Dt_Commission, string Emp_Id)
    {
        if (ddlType.SelectedValue.ToString() == "Sales")
        {
            decimal Amount = 0;
            string strsql = "Select ISCCH.Parameter_Level,ISCCD.Category_Id,ISCCD.Sales_Quota,ISCCD.Commission_Percentage,ISCCD.Is_Allow From Inv_SalesCommissionConfiguration_Header ISCCH  Inner Join Inv_SalesCommissionConfiguration_Detail ISCCD On ISCCH.Trans_Id=ISCCD.Ref_Id Where Employee_Id ='" + Emp_Id + "' and ISCCH.IsActive='True' and ISCCD.IsActive='True'";
            DataTable Dt_Configuration = objDa.return_DataTable(strsql);
            if (Dt_Configuration != null && Dt_Configuration.Rows.Count > 0)
            {
                foreach (DataRow Dt_Config_Row in Dt_Configuration.Rows)
                {
                    if (Dt_Config_Row["Is_Allow"].ToString() == "False" && Dt_Config_Row["Parameter_Level"].ToString().Trim() == "On Sales")
                    {
                        DataView view = new DataView(Dt_Commission);
                        DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
                        foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
                        {
                            Amount = 0;
                            int No_Of_Time = 0;
                            foreach (DataRow Dt_Row in Dt_Commission.Rows)
                            {
                                if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                {
                                    Amount += Convert.ToDecimal(Dt_Row["Amount"].ToString());
                                    if (Amount >= Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                                    {
                                        if (No_Of_Time == 0)
                                        {
                                            if (Amount > Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                                            {
                                                decimal Commission_On_Amount = Amount - Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString());
                                                decimal Net_Commission = (Commission_On_Amount * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100;
                                                decimal C_Percent = ((Net_Commission * 100) / Convert.ToDecimal(Dt_Row["Amount"].ToString()));
                                                Dt_Row["Comission_Percentage"] = C_Percent.ToString();
                                                Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * C_Percent) / 100).ToString();
                                            }
                                        }
                                        else
                                        {
                                            Dt_Row["Comission_Percentage"] = Dt_Config_Row["Commission_Percentage"].ToString();
                                            Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100).ToString();
                                        }
                                        No_Of_Time++;
                                    }
                                    else
                                    {
                                        Dt_Row["Comission_Percentage"] = "0.00";
                                        Dt_Row["Comission_Amount"] = "0.00";
                                    }
                                }
                            }
                        }
                    }
                    else if (Dt_Config_Row["Is_Allow"].ToString() == "True" && Dt_Config_Row["Parameter_Level"].ToString().Trim() == "On Sales")
                    {
                        DataView view = new DataView(Dt_Commission);
                        DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
                        foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
                        {
                            Amount = 0;
                            foreach (DataRow Dt_Row in Dt_Commission.Rows)
                            {
                                if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                {
                                    Amount += Convert.ToDecimal(Dt_Row["Amount"].ToString());
                                }
                            }
                            if (Amount > Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                            {
                                foreach (DataRow Dt_Row in Dt_Commission.Rows)
                                {
                                    if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                    {
                                        Dt_Row["Comission_Percentage"] = Dt_Config_Row["Commission_Percentage"].ToString();
                                        Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100).ToString();
                                    }
                                }
                            }
                        }
                    }
                    else if (Dt_Config_Row["Is_Allow"].ToString() == "False" && Dt_Config_Row["Parameter_Level"].ToString().Trim() == "On Category")
                    {
                        decimal Amount_Category = 0;
                        int No_Of_Time = 0;
                        DataView view = new DataView(Dt_Commission);
                        DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
                        foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
                        {
                            No_Of_Time = 0;
                            Amount_Category = 0;
                            foreach (DataRow Dt_Row in Dt_Commission.Rows)
                            {
                                if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                {
                                    if (Dt_Row["CategoryId"].ToString() == Dt_Config_Row["Category_Id"].ToString())
                                    {
                                        Amount_Category += Convert.ToDecimal(Dt_Row["Amount"].ToString());
                                        if (Amount_Category >= Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                                        {
                                            if (No_Of_Time == 0)
                                            {
                                                if (Amount_Category > Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                                                {
                                                    decimal Commission_On_Amount = Amount_Category - Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString());
                                                    decimal Net_Commission = (Commission_On_Amount * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100;
                                                    decimal C_Percent = ((Net_Commission * 100) / Convert.ToDecimal(Dt_Row["Amount"].ToString()));
                                                    Dt_Row["Comission_Percentage"] = C_Percent.ToString();
                                                    Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * C_Percent) / 100).ToString();
                                                }
                                            }
                                            else
                                            {
                                                Dt_Row["Comission_Percentage"] = Dt_Config_Row["Commission_Percentage"].ToString();
                                                Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100).ToString();
                                            }
                                            No_Of_Time++;
                                        }
                                        else
                                        {
                                            Dt_Row["Comission_Percentage"] = "0.00";
                                            Dt_Row["Comission_Amount"] = "0.00";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Dt_Config_Row["Is_Allow"].ToString() == "True" && Dt_Config_Row["Parameter_Level"].ToString().Trim() == "On Category")
                    {
                        decimal Amount_Category = 0;
                        DataView view = new DataView(Dt_Commission);
                        DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
                        foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
                        {
                            Amount_Category = 0;
                            foreach (DataRow Dt_Row in Dt_Commission.Rows)
                            {
                                if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                {
                                    if (Dt_Row["CategoryId"].ToString() == Dt_Config_Row["Category_Id"].ToString())
                                    {
                                        Amount_Category += Convert.ToDecimal(Dt_Row["Amount"].ToString());
                                    }
                                }
                            }
                            if (Amount_Category > Convert.ToDecimal(Dt_Config_Row["Sales_Quota"].ToString()))
                            {
                                foreach (DataRow Dt_Row in Dt_Commission.Rows)
                                {
                                    if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
                                    {
                                        if (Dt_Row["CategoryId"].ToString() == Dt_Config_Row["Category_Id"].ToString())
                                        {
                                            Dt_Row["Comission_Percentage"] = Dt_Config_Row["Commission_Percentage"].ToString();
                                            Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Config_Row["Commission_Percentage"].ToString())) / 100).ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //if (Dt_Configuration.Rows[0]["Parameter_Level"].ToString().Trim() == "On Sales")
            //{
            //    DataView view = new DataView(Dt_Commission);                
            //    view.Sort = "Amount";
            //    DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
            //    foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
            //    {
            //        Amount = 0;
            //        int No_Of_Time = 0;
            //        foreach (DataRow Dt_Row in Dt_Commission.Rows)
            //        {
            //            if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
            //            {
            //                Amount += Convert.ToDecimal(Dt_Row["Amount"].ToString());
            //                if (Amount >= Convert.ToDecimal(Dt_Configuration.Rows[0]["Sales_Quota"].ToString()))
            //                {
            //                    if (No_Of_Time == 0)
            //                    {
            //                        if (Amount > Convert.ToDecimal(Dt_Configuration.Rows[0]["Sales_Quota"].ToString()))
            //                        {
            //                            decimal Commission_On_Amount = Amount - Convert.ToDecimal(Dt_Configuration.Rows[0]["Sales_Quota"].ToString());
            //                            decimal Net_Commission = (Commission_On_Amount * Convert.ToDecimal(Dt_Configuration.Rows[0]["Commission_Percentage"].ToString())) / 100;
            //                            decimal C_Percent = ((Net_Commission * 100) / Convert.ToDecimal(Dt_Row["Amount"].ToString()));
            //                            Dt_Row["Comission_Percentage"] = C_Percent.ToString();
            //                            Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * C_Percent) / 100).ToString();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Dt_Row["Comission_Percentage"] = Dt_Configuration.Rows[0]["Commission_Percentage"].ToString();
            //                        Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Configuration.Rows[0]["Commission_Percentage"].ToString())) / 100).ToString();
            //                    }
            //                    No_Of_Time++;
            //                }
            //                else
            //                {
            //                    Dt_Row["Comission_Percentage"] = "0.00";
            //                    Dt_Row["Comission_Amount"] = "0.00";
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (Dt_Configuration.Rows[0]["Parameter_Level"].ToString().Trim() == "On Category")
            //{
            //    foreach (DataRow Dt_Rows_Config in Dt_Configuration.Rows)
            //    {
            //        decimal Amount_Category = 0;
            //        int No_Of_Time = 0;
            //        DataView view = new DataView(Dt_Commission);
            //        view.Sort = "Amount,CategoryId";
            //        DataTable Distinct_Month_Year = view.ToTable(true, "Invoice_Month", "Invoice_Year");
            //        foreach (DataRow Dt_Month_Row in Distinct_Month_Year.Rows)
            //        {
            //            No_Of_Time = 0;
            //            Amount_Category = 0;
            //            foreach (DataRow Dt_Row in Dt_Commission.Rows)
            //            {
            //                if (Dt_Row["Invoice_Month"].ToString() == Dt_Month_Row["Invoice_Month"].ToString() && Dt_Row["Invoice_Year"].ToString() == Dt_Month_Row["Invoice_Year"].ToString())
            //                {
            //                    if (Dt_Row["CategoryId"].ToString() == Dt_Rows_Config["Category_Id"].ToString())
            //                    {
            //                        Amount_Category += Convert.ToDecimal(Dt_Row["Amount"].ToString());
            //                        if (Amount_Category >= Convert.ToDecimal(Dt_Rows_Config["Sales_Quota"].ToString()))
            //                        {
            //                            if (No_Of_Time == 0)
            //                            {
            //                                if (Amount_Category > Convert.ToDecimal(Dt_Rows_Config["Sales_Quota"].ToString()))
            //                                {
            //                                    decimal Commission_On_Amount = Amount_Category - Convert.ToDecimal(Dt_Rows_Config["Sales_Quota"].ToString());
            //                                    decimal Net_Commission = (Commission_On_Amount * Convert.ToDecimal(Dt_Rows_Config["Commission_Percentage"].ToString())) / 100;
            //                                    decimal C_Percent = ((Net_Commission * 100) / Convert.ToDecimal(Dt_Row["Amount"].ToString()));
            //                                    Dt_Row["Comission_Percentage"] = C_Percent.ToString();
            //                                    Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * C_Percent) / 100).ToString();
            //                                }
            //                            }
            //                            else
            //                            {
            //                                Dt_Row["Comission_Percentage"] = Dt_Rows_Config["Commission_Percentage"].ToString();
            //                                Dt_Row["Comission_Amount"] = ((Convert.ToDecimal(Dt_Row["Amount"].ToString()) * Convert.ToDecimal(Dt_Rows_Config["Commission_Percentage"].ToString())) / 100).ToString();
            //                            }
            //                            No_Of_Time++;
            //                        }
            //                        else
            //                        {
            //                            Dt_Row["Comission_Percentage"] = "0.00";
            //                            Dt_Row["Comission_Amount"] = "0.00";
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        DataView Dt_Sort = new DataView(Dt_Commission);
        Dt_Sort.Sort = "Invoice_Date";
        Dt_Commission = Dt_Sort.ToTable();
        return Dt_Commission;
    }
}