using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using PegasusDataAccess;

public partial class HR_HR_AdvancePayment : System.Web.UI.Page
{
    Pay_AdvancePayment objAdvancePayment = null;
    CurrencyMaster objCurrency = null;
    EmployeeMaster objEmployee = null;
    DataAccessClass objda = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    LocationMaster objLocation = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objAdvancePayment = new Pay_AdvancePayment(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/HR_AdvancePayment.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillGrid();
            FillCurrency();
            Calender.Format = ObjSys.SetDateFormat();
            txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
            SetAccountDetail();
        }
    }

    public void SetAccountDetail()
    {
        string strCashAccount = string.Empty;
        string strHRAccount = string.Empty;
        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());

        DataTable dtDebit = new DataView(dtAcParameter, "Param_Name='Employee Advance Payment'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {
            string strAccountId = dtDebit.Rows[0]["Param_Value"].ToString();
            DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAcc.Rows.Count > 0)
            {
                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();

                txtpaymentdebitaccount.Text = strAccountName + "/" + strAccountId;
            }
        }


    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }


        try
        {
            ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        }
        catch
        {
        }
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btn_Post.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    protected void Btn_List_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {
        txtEmployee.Focus();
    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }

    protected void Btn_Post_Click(object sender, EventArgs e)
    {
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
        Btn_Save_Click(sender, e);
       
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        bool IsPost = false;


        string strCreditAc = "0";
        string strDebitAc = "0";

        if (txtpaymentCreditaccount.Text != "")
        {
            strCreditAc = txtpaymentCreditaccount.Text.Split('/')[1].ToString();

        }

        if (txtpaymentdebitaccount.Text != "")
        {
            strDebitAc = txtpaymentdebitaccount.Text.Split('/')[1].ToString();
        }



        if (((Button)sender).ID.Trim() == "btn_Post")
        {
            IsPost = true;
        }


        double PaidTotaamount = 0;

        try
        {
            PaidTotaamount = Convert.ToDouble(txtAdvanceAmount.Text);
        }
        catch
        {

        }
        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        string Emp_Code = txtEmployee.Text.Split('/')[1].ToString();
        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        if (Emp_ID == "0" || Emp_ID == "")
        {
            DisplayMessage("Employee not exists");
            txtEmployee.Focus();
            txtEmployee.Text = "";
            return;
        }
        string strEmployeeId = GetEmployeeId(txtEmployee.Text.Split('/')[0].ToString() + "/" + Emp_ID);

        string Employee_Name = txtEmployee.Text.Split('/')[0].ToString() + "/" + Emp_ID;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            if (hdnEditId.Value == "")
            {

                //insert record

                int b = objAdvancePayment.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmployeeId, ObjSys.getDateForInput(txttrnDate.Text).ToString(), txtAdvanceAmount.Text, ddlCurrency.SelectedValue, chkIsAmountAdjusted.Checked.ToString(), "0", txtRemarks.Text, true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), IsPost.ToString(), strCreditAc, strDebitAc, ref trns);

                strTransId = b.ToString();
            }
            else
            {
                //update record

                strTransId = hdnEditId.Value;
                objAdvancePayment.UpdateRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, strEmployeeId, ObjSys.getDateForInput(txttrnDate.Text).ToString(), txtAdvanceAmount.Text, ddlCurrency.SelectedValue, chkIsAmountAdjusted.Checked.ToString(), "0", txtRemarks.Text, true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), IsPost.ToString(), strCreditAc, strDebitAc, ref trns);

            }

            if (((Button)sender).ID.Trim() == "btn_Post")
            {
                objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), strEmployeeId, ObjSys.getDateForInput(txttrnDate.Text).ToString(), txtAdvanceAmount.Text, "0", ddlCurrency.SelectedValue, getConvertedAmount(ddlCurrency.SelectedValue, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), txtAdvanceAmount.Text), "0", getConvertedAmount(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString(), txtAdvanceAmount.Text), "0", "Pay_EmployeeAdvancePayment", strTransId, "0", "Advance Payment Entry", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),false.ToString(), ref trns);

                if (PaidTotaamount != 0)
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
                    string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                    if (strVoucherNumber != "")
                    {
                        int counter = objAcParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PV", Session["FinanceYearId"].ToString(), ref trns);


                        if (counter == 0)
                        {
                            strVoucherNumber = strVoucherNumber + "1";
                        }
                        else
                        {
                            strVoucherNumber = strVoucherNumber + (counter + 1);
                        }
                    }

                    int MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "PV", "1/1/1800", "1/1/1800", "", "Employee Adavance Payment entry of " + Employee_Name.Split('/')[0].ToString() + "", strCurrencyId, "0.00", " Employee Advance Payment Voucher and Employee is '" + Employee_Name.Split('/')[0].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    string strVMaxId = MaxId.ToString();


                    string strCompAmount = getConvertedAmount(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString(), PaidTotaamount.ToString());
                    string strLocAmount = getConvertedAmount(ddlCurrency.SelectedValue, strCurrencyId, PaidTotaamount.ToString());
                    string strForeignAmount = PaidTotaamount.ToString();
                    string strForeignExchangerate = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, strCurrencyId, Session["DBConnection"].ToString());



                    //str for Employee Id
                    //For Debit
                    string EmpAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
                    string EmpAccountId = EmpAccountName.Split('/')[1].ToString();

                    string strCompanyCrrValueDr = getConvertedAmount(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString(), PaidTotaamount.ToString());
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    //if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                    //{
                    //    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Employee Adavance Payment entry of " + txtEmployee.Text.Split('/')[0].ToString() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //}
                    //else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1",EmpAccountId, Employee_Name.Split('/')[1].ToString(), "0", "EAP", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Employee Adavance Payment entry of " + Employee_Name.Split('/')[0].ToString() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Credit
                    string strCompanyCrrValueCr = strCompanyCrrValueDr;
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Employee Adavance Payment entry of " + Employee_Name.Split('/')[0].ToString() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Employee Adavance Payment entry of " + Employee_Name.Split('/')[0].ToString() + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();





            if (hdnEditId.Value == "")
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Saved Successfully", "green");
                }

            }
            else
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Updated Successfully", "green");
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);


                Btn_List_Click(null, null);
            }
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

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = objda.return_DataTable(sql);
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



    public string getConvertedAmount(string strFromCurrency, string strToCurrency, string strAmount)
    {
        string strconvertedAmount = string.Empty;
        double ExchangeRate = 0;
        try
        {
            ExchangeRate = Convert.ToDouble(SystemParameter.GetExchageRate(strFromCurrency, strToCurrency, Session["DBConnection"].ToString()));
        }
        catch
        {

        }

        if (strAmount == "")
        {
            strAmount = "0";
        }

        strconvertedAmount = ObjSys.GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strAmount) * ExchangeRate).ToString());




        return strconvertedAmount;
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    public void Reset()
    {
        txtEmployee.Text = "";
        txtAdvanceAmount.Text = "";
        txtRemarks.Text = "";
        hdnEditId.Value = "";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        Btn_New.Text = Resources.Attendance.New;
        txtEmployee.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
    }



    #region List

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = objAdvancePayment.GetTrueAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (ddlPosted.SelectedIndex == 0)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlPosted.SelectedIndex == 1)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='False'", "", DataViewRowState.CurrentRows).ToTable();

        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_HR_Adv_Payment"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
      

    }


    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_HR_Adv_Payment"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
    
        GvsalaryPlan.HeaderRow.Focus();
    }

    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_HR_Adv_Payment"];
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
        Session["dtFilter_HR_Adv_Payment"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
      
        GvsalaryPlan.HeaderRow.Focus();
    }





    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
            DataTable dtDeduction = (DataTable)Session["dtDeduction"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtFilter_HR_Adv_Payment"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objAdvancePayment.GetRecordByTrans_Id(Session["CompId"].ToString(), e.CommandArgument.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }



            try
            {
                hdnEditId.Value = e.CommandArgument.ToString();
                txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["Trn_Date"].ToString()).ToString(ObjSys.SetDateFormat());
                //txtEmployee.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Id"].ToString();
                txtEmployee.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString();
                //txtEmployee.Text = "";
                txtAdvanceAmount.Text = dt.Rows[0]["Advance_Amount"].ToString();
                ddlCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                chkIsAmountAdjusted.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Adjusted"].ToString());

                if (dt.Rows[0]["Credit_Account"].ToString() != "0")
                {
                    txtpaymentCreditaccount.Text = GetAccountName(dt.Rows[0]["Credit_Account"].ToString()) + "/" + dt.Rows[0]["Credit_Account"].ToString();
                }
                else
                {
                    txtpaymentCreditaccount.Text = "";
                }


                if (dt.Rows[0]["Debit_Account"].ToString() != "0")
                {
                    txtpaymentdebitaccount.Text = GetAccountName(dt.Rows[0]["Debit_Account"].ToString()) + "/" + dt.Rows[0]["Debit_Account"].ToString();

                }
                else
                {
                    txtpaymentdebitaccount.Text = "";

                }


            }
            catch
            {

            }
            txtEmployee.Focus();

            //TabContainer1.ActiveTabIndex = 1;
            // Btn_New_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            //Txt_Plan_Name.Focus();
        }
    }



    public string GetAccountName(string strTransId)
    {
        string strAcName = string.Empty;

        if (strTransId == "")
        {
            strTransId = "0";
        }
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_id='" + strTransId + "' and IsActive='True'";
        DataTable dtCOA = objda.return_DataTable(sql);

        if (dtCOA.Rows.Count > 0)
        {
            strAcName = dtCOA.Rows[0]["AccountName"].ToString();


        }

        return strAcName;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();
        dt = objAdvancePayment.GetRecordByTrans_Id(Session["CompId"].ToString(), e.CommandArgument.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = objAdvancePayment.Deletrecord(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");

            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    #endregion


    #region Bin


    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = objAdvancePayment.GetFalseAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
           
        }
    }


    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtBinDeduction"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
                
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs  e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (GvsalaryPlanBin.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = objAdvancePayment.Deletrecord(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                return;
            }
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                int index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {

        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        CheckBox chkSelAll = ((CheckBox)GvsalaryPlanBin.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void GvsalaryPlanBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvsalaryPlanBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

        PopulateCheckedValues();
      

    }
    protected void GvsalaryPlanBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

       
        PopulateCheckedValues();
    }
    #endregion



    #region Account
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
                DataTable dtCOA = objda.return_DataTable(sql);

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
    #endregion

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
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
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtEmployee.Text != "")
        {

            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
           string Emp_Code = txtEmployee.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            if (Emp_ID == "0" || Emp_ID == "")
            {
                DisplayMessage("Employee not exists");
                txtEmployee.Focus();
                txtEmployee.Text = "";
                return;
            }


            strEmployeeId = GetEmployeeId(txtEmployee.Text.Split('/')[0].ToString() + "/" + Emp_ID);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtEmployee.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployee);
            }
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
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText); 
        //ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        //dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
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
}