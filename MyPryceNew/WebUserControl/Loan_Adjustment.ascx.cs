using PegasusDataAccess;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class WebUserControl_Loan_Adjustment : System.Web.UI.UserControl
{
    DataAccessClass da = null;
    SystemParameter ObjSysParam = null;
    Pay_Employee_Loan ObjLoan = null;
    Common ObjComman = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objApproalEmp = null;
    LocationMaster objLocation = null;
    EmployeeMaster ObjEmp = null;
    Common cmn = null;
    NotificationMaster Obj_Notifiacation = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_Approval_Employee objEmpApproval = null;
    Set_DocNumber objDocNo = null;
    Ac_ParameterMaster objAcParameter = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        Txt_Adjustment_Amount_LControl.Text = Hdn_Adjustment_Amount_LControl.Value;
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
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public string GetType(string Type)
    {
        if (Type == "1")
        {
            Type = "January";
        }
        if (Type == "2")
        {
            Type = "February";
        }
        if (Type == "3")
        {
            Type = "March";
        }
        if (Type == "4")
        {
            Type = "April";
        }
        if (Type == "5")
        {
            Type = "May";
        }
        if (Type == "6")
        {
            Type = "June";
        }
        if (Type == "7")
        {
            Type = "July";
        }
        if (Type == "8")
        {
            Type = "August";
        }
        if (Type == "9")
        {
            Type = "September";
        }
        if (Type == "10")
        {
            Type = "October";
        }
        if (Type == "11")
        {
            Type = "November";
        }
        if (Type == "12")
        {
            Type = "December";
        }
        return Type;
    }
    public static string GetIntrest(object Interest)
    {
        SystemParameter Objsys = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        if (Interest.ToString() == "")
        {
            Interest = "0%";
        }
        else
        {
            Interest = Interest.ToString() + '%';
        }
        return Interest.ToString();
    }
    protected void Btn_Edit_Control_command(object sender, CommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent;
        Hiddenid_Loan.Value = e.CommandArgument.ToString();
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), Hiddenid_Loan.Value, "Pending");
        GridBind_Loan_Detail_LControl(Hiddenid_Loan.Value);
        Div_Details.Style.Add("display", "");
        Grid_View_Loan_Control.Rows[row.RowIndex].Cells[0].Focus();
    }
    //protected void Btn_Cancel_LControl_Click(object sender, EventArgs e)
    //{
    //    Reset_LControl();
    //}
    protected void Btn_Reset_LControl_Click(object sender, EventArgs e)
    {
        Reset_LControl();
    }
    void Reset_LControl()
    {
        Grid_View_Loan_Control.DataSource = null;
        Grid_View_Loan_Control.DataBind();
        Grid_View_Loan_Detail_Control.DataSource = null;
        Grid_View_Loan_Detail_Control.DataBind();
        Txt_Loan_Emp_Name.Text = "";
        Txt_Pending_Amount_LControl.Text = "0.00";
        Txt_Adjustment_Amount_LControl.Text = "0.00";
        Txt_Account_Name_LControl.Text = "";
        HidEmpId_Loan.Value = "";
        HDFSort_Loan.Value = "";
        HdfSortDetail.Value = "";
        Hiddenid_Loan.Value = "";
        Hdn_Adjustment_Amount_LControl.Value = "0.00";
        Div_Details.Style.Add("display", "none");
    }
    protected void Btn_Save_LControl_Click(object sender, EventArgs e)
    {
        if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null && Session["FinanceYearId"] != null && Session["UserId"] != null && Session["EmpId"] != null)
        {
            try
            {
                if (Txt_Loan_Emp_Name.Text == "")
                {
                    DisplayMessage("Please Enter Employee Name");
                    Txt_Loan_Emp_Name.Focus();
                    return;
                }
                if (Txt_Account_Name_LControl.Text == "")
                {
                    DisplayMessage("Please Select Credit Account");
                    Txt_Account_Name_LControl.Focus();
                    return;
                }
                int IsSelectedRow = Check_Selected_Row();
                {
                    if (IsSelectedRow == 0)
                    {
                        DisplayMessage("Please select at least one row");
                        return;
                    }
                }
                int Is_Sequence = Check_Selected_In_Sequence();
                {
                    if (Is_Sequence > 0)
                    {
                        DisplayMessage("Please select in sequence after paid installment");
                        return;
                    }
                }
                // string Emp_Finance_Status = Check_Transfer_In_Finance();
                //if (Emp_Finance_Status == "Break")
                //{
                //    DisplayMessage("Some vouchers are pending, please check transfer in finance");
                //    return;
                //}
                if (Chk_Emp_A_Salary_Ac_LControl.Checked || Txt_Account_Name_LControl.Text.Split('/')[1] == "181")
                {
                    string Emp_Acc_Status = Check_Employee_Account();
                    if (Emp_Acc_Status == "Break")
                    {
                        DisplayMessage("Unable to deduct loan installment amount " + Txt_Adjustment_Amount_LControl.Text + " because of insufficient closing balance");
                        return;
                    }
                }
            }
            catch
            {
            }
            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                string Str_Selected_Month_Year = Selected_Month_Year();
                string strVoucherNumber = string.Empty;
                string strVoucher_Type = string.Empty;
                if (Chk_Emp_A_Salary_Ac_LControl.Checked || Txt_Account_Name_LControl.Text.Split('/')[1] == "181")
                {
                    strVoucherNumber = Get_Voucher_Number("JV", "160", "302", trns);
                    strVoucher_Type = "JV";
                }
                else
                {
                    strVoucherNumber = Get_Voucher_Number("RV", "160", "303", trns);
                    strVoucher_Type = "RV";
                }
                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
                int VMaxId_Loan = 0;
                // For Header Insert
                VMaxId_Loan = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "Loan Adjustment", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), strVoucher_Type, "1/1/1800", "1/1/1800", "", "", strCurrencyId, "1", "Loan Adjustment for " + Txt_Loan_Emp_Name.Text.Split('/')[0].ToString() + " For the Month " + Str_Selected_Month_Year + "", false.ToString(), false.ToString(), false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strAccountId_Loan = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                //For Details Debit
                string CompanyCurrDebit = GetCurrency(strCurrencyId, Txt_Adjustment_Amount_LControl.Text.ToString()).Trim().Split('/')[0].ToString();
                if (strAccountId_Loan.Split(',').Contains(Txt_Account_Name_LControl.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VMaxId_Loan.ToString(), "1", "143", "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Txt_Adjustment_Amount_LControl.Text, "Loan installment deposited for " + Txt_Loan_Emp_Name.Text.Split('/')[0].ToString() + " For the Month " + Str_Selected_Month_Year + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VMaxId_Loan.ToString(), "1", "143", HidEmpId_Loan.Value, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Txt_Adjustment_Amount_LControl.Text, "Loan installment deposited for " + Txt_Loan_Emp_Name.Text.Split('/')[0].ToString() + " For the Month " + Str_Selected_Month_Year + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                //For Details Credit
                string CompanyCurrCredit = CompanyCurrDebit;
                if (strAccountId_Loan.Split(',').Contains(Txt_Account_Name_LControl.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VMaxId_Loan.ToString(), "1", Txt_Account_Name_LControl.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", Txt_Adjustment_Amount_LControl.Text.Trim(), "0.00", "Loan installment deposited for the Month " + Str_Selected_Month_Year + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VMaxId_Loan.ToString(), "1", Txt_Account_Name_LControl.Text.Split('/')[1].ToString(), HidEmpId_Loan.Value, "0", "PP", "1/1/1800", "1/1/1800", "", Txt_Adjustment_Amount_LControl.Text.Trim(), "0.00", "Loan installment deposited for the Month " + Str_Selected_Month_Year + "", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                //For Update Loan Detail
                int Loan_Update = Update_Loan_Detail(trns);
                if (Loan_Update != 0)
                {
                    DisplayMessage("Record Saved Successfully", "green");
                    Reset_LControl();
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                else
                {
                    DisplayMessage("Record not saved");
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
        else
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
    }
    protected void Btn_Get_Loan_Click(object sender, EventArgs e)
    {
        GridBind_Loan_LControl();
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(ObjSysParam.SetDateFormat());
    }

    void GridBind_Loan_LControl()
    {
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
        Dt = new DataView(Dt, "Emp_Id='" + HidEmpId_Loan.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt.Rows.Count > 0)
        {
            Dt.Columns.Add("Intrest_Amt");
            foreach (DataRow DTR in Dt.Rows)
            {
                DTR["Intrest_Amt"] = Convert.ToDouble(DTR["Gross_Amount"]) - Convert.ToDouble(DTR["Loan_Amount"]);
            }
            objPageCmn.FillData((object)Grid_View_Loan_Control, Dt, "", "");
            Session["dtFilter_Pay_Emp_Loan"] = Dt;
        }
        else
        {
            DataTable Dtclear = new DataTable();
            objPageCmn.FillData((object)Grid_View_Loan_Control, Dtclear, "", "");
        }
    }
    void GridBind_Loan_Detail_LControl(string LoanId)
    {
        DataTable dt = new DataTable();
        dt = ObjLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(LoanId);
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)Grid_View_Loan_Detail_Control, dt, "", "");
            Session["dtLoanDetail"] = dt;
            double Montly_Installment = Convert.ToDouble(dt.Compute("SUM(Montly_Installment)", string.Empty));
            double Employee_Paid = 0;
            try
            {
                Employee_Paid = Convert.ToDouble(dt.Compute("SUM(Employee_Paid)", "Is_Status = 'Paid'"));
            }
            catch
            {
                Employee_Paid = 0;
            }
            if (Grid_View_Loan_Detail_Control.Rows.Count > 0)
            {
                Label M_Installment = (Label)Grid_View_Loan_Detail_Control.FooterRow.FindControl("lblgvMonth_Installment");
                Label Paid = (Label)Grid_View_Loan_Detail_Control.FooterRow.FindControl("lblgvPaid");
                Label Pending = (Label)Grid_View_Loan_Detail_Control.FooterRow.FindControl("lblgvPending");
                Txt_Pending_Amount_LControl.Text = Pending.Text;
                M_Installment.Text = Common.GetAmountDecimal(Montly_Installment.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                Paid.Text = Common.GetAmountDecimal(Employee_Paid.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + " (Paid)";
                Pending.Text = Common.GetAmountDecimal((Montly_Installment - Employee_Paid).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + " (Pending)";
                Txt_Pending_Amount_LControl.Text = Common.GetAmountDecimal((Montly_Installment - Employee_Paid).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            }
            foreach (GridViewRow GVR in Grid_View_Loan_Detail_Control.Rows)
            {
                Label lblvaluetype4 = (Label)GVR.FindControl("lblvaluetype4");
                CheckBox Chk_Gv_Select_LControl = (CheckBox)GVR.FindControl("Chk_Gv_Select_LControl");
                if (lblvaluetype4.Text == "Paid")
                    Chk_Gv_Select_LControl.Visible = false;
            }
        }
        else
        {
            DataTable Dtclear = new DataTable();
            objPageCmn.FillData((object)Grid_View_Loan_Detail_Control, Dtclear, "", "");
        }
    }
    protected void Chk_Emp_A_Salary_Ac_LControl_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Emp_A_Salary_Ac_LControl.Checked)
        {
            DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(HttpContext.Current.Session["CompId"].ToString());
            DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Employee Account'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPaymentVoucher.Rows.Count > 0)
            {
                Txt_Account_Name_LControl.Text = dtPaymentVoucher.Rows[0]["Param_Name"].ToString() + "/" + dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
            }
        }
        else
        {
            Txt_Account_Name_LControl.Text = "";
        }
    }
    protected void Chk_Gv_Select_LControl_All_LControl_CheckedChanged(object sender, EventArgs e)
    {
        double tot = 0.00;
        CheckBox chkSelAll = ((CheckBox)Grid_View_Loan_Detail_Control.HeaderRow.FindControl("Chk_Gv_Select_LControl_All_LControl"));
        foreach (GridViewRow gr in Grid_View_Loan_Detail_Control.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                Label Lbl_Total_Amount = (Label)gr.FindControl("lblvaluetype2");
                CheckBox Chk = (CheckBox)gr.FindControl("Chk_Gv_Select_LControl");
                if (Chk.Visible == true)
                {
                    Chk.Checked = true;
                    tot += Convert.ToDouble(Lbl_Total_Amount.Text);
                }
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_LControl")).Checked = false;
            }
        }
        Hdn_Adjustment_Amount_LControl.Value = tot.ToString();
        Txt_Adjustment_Amount_LControl.Text = tot.ToString();
    }
    protected void Grid_View_Loan_Detail_Control_Sorting(object sender, GridViewSortEventArgs e)
    {
        HdfSortDetail.Value = HdfSortDetail.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtLoanDetail"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HdfSortDetail.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtLoanDetail"] = dt;
        objPageCmn.FillData((object)Grid_View_Loan_Detail_Control, dt, "", "");
    }
    protected void Grid_View_Loan_Detail_Control_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_View_Loan_Detail_Control.PageIndex = e.NewPageIndex;
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtLoanDetail"];
        objPageCmn.FillData((object)Grid_View_Loan_Detail_Control, dt, "", "");

    }
    protected void Grid_View_Loan_Control_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_View_Loan_Control.PageIndex = e.NewPageIndex;
        GridBind_Loan_LControl();

        Grid_View_Loan_Control.HeaderRow.Focus();
    }
    protected void Grid_View_Loan_Control_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort_Loan.Value = HDFSort_Loan.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pay_Emp_Loan"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort_Loan.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pay_Emp_Loan"] = dt;
        objPageCmn.FillData((object)Grid_View_Loan_Control, dt, "", "");

        Grid_View_Loan_Control.HeaderRow.Focus();
    }
    protected void Txt_Loan_Emp_Name_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string empid = string.Empty;
            if (((TextBox)sender).Text != "")
            {
                try
                {
                    empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];
                }
                catch
                {
                    Reset_LControl();
                    empid = "0";
                }
                DataTable dtEmp = ObjEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtEmp.Rows.Count > 0)
                {
                    empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                    HidEmpId_Loan.Value = empid;
                }
                else
                {
                    Reset_LControl();
                    DisplayMessage("Employee Not Exists");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    HidEmpId_Loan.Value = "";
                    return;
                }
            }
            else
            {
                Reset_LControl();
            }
            Txt_Loan_Emp_Name.Focus();
        }
        catch
        { }
    }
    protected void Txt_Account_Name_LControl_TextChanged(object sender, EventArgs e)
    {
        Chk_Emp_A_Salary_Ac_LControl.Checked = false;
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
    public string Check_Employee_Account()
    {
        string Emp_Status = string.Empty;
        if (Chk_Emp_A_Salary_Ac_LControl.Checked || Txt_Account_Name_LControl.Text.Split('/')[1] == "181")
        {
            string strPaymentVoucherAcc = string.Empty;
            string strAccount = "Employee Account";
            DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
            DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='" + strAccount.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPaymentVoucher.Rows.Count > 0)
            {
                strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtClosingBalance = da.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"] + "', '" + DateTime.Now.ToString() + "', '0','" + strPaymentVoucherAcc + "','" + HidEmpId_Loan.Value + "','1', '" + Session["FinanceYearId"].ToString() + "')) ClosingBalance");
            if (dtClosingBalance != null && dtClosingBalance.Rows.Count > 0)
            {
                if (Convert.ToDouble(dtClosingBalance.Rows[0][0].ToString()) >= 0)
                {
                    Emp_Status = "Break";
                }
                else if (Convert.ToDouble(dtClosingBalance.Rows[0][0].ToString()) < 0)
                {
                    double Net_Balance = Convert.ToDouble(Txt_Adjustment_Amount_LControl.Text) + Convert.ToDouble(dtClosingBalance.Rows[0][0].ToString());
                    if (Net_Balance > 0)
                    {
                        Emp_Status = "Break";
                    }
                }
            }
        }
        return Emp_Status;
    }
    public string Check_Transfer_In_Finance()
    {
        string Emp_Finance_Status = string.Empty;
        DataTable Dt_Finance_Salary = objVoucherDetail.Get_Voucher_IsPosted("181", HidEmpId_Loan.Value, "True", "False", "1");
        if (Dt_Finance_Salary != null && Dt_Finance_Salary.Rows.Count > 0)
        {
            if (Convert.ToInt64(Dt_Finance_Salary.Rows[0][0].ToString()) > 0)
                Emp_Finance_Status = "Break";
        }
        DataTable Dt_Finance_Loan = objVoucherDetail.Get_Voucher_IsPosted("143", HidEmpId_Loan.Value, "True", "False", "1");
        if (Dt_Finance_Loan != null && Dt_Finance_Loan.Rows.Count > 0)
        {
            if (Convert.ToInt64(Dt_Finance_Loan.Rows[0][0].ToString()) > 0)
                Emp_Finance_Status = "Break";
        }
        return Emp_Finance_Status;
    }
    public int Check_Selected_Row()
    {
        int IsSelectedRow = 0;
        foreach (GridViewRow GVR in Grid_View_Loan_Detail_Control.Rows)
        {
            CheckBox Chk_Gv_Select_LControl = (CheckBox)GVR.FindControl("Chk_Gv_Select_LControl");
            if (Chk_Gv_Select_LControl.Checked)
                IsSelectedRow++;
        }
        return IsSelectedRow;
    }
    public int Check_Selected_In_Sequence()
    {
        int Is_Sequence = 0;
        int i = 1;
        foreach (GridViewRow GVR in Grid_View_Loan_Detail_Control.Rows)
        {
            if (GVR.RowIndex > 1)
            {
                i++;
            }
            CheckBox Chk_Sequence = (CheckBox)Grid_View_Loan_Detail_Control.Rows[i - 1].FindControl("Chk_Gv_Select_LControl");
            CheckBox Chk_Gv_Select_LControl = (CheckBox)GVR.FindControl("Chk_Gv_Select_LControl");
            if (Chk_Sequence.Checked == false && Chk_Gv_Select_LControl.Checked == true && Chk_Sequence.Visible == true)
                Is_Sequence++;
        }
        return Is_Sequence;
    }
    public string Get_Voucher_Number(string Voucher_Type, string Module_Id, string Object_Id, SqlTransaction trns)
    {
        string strVoucherNumber = string.Empty;
        //strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns);
        strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, Module_Id, Object_Id, "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        if (strVoucherNumber != "")
        {
            DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
            if (dtCount.Rows.Count > 0)
            {
                dtCount = new DataView(dtCount, "Voucher_Type='" + Voucher_Type + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtCount.Rows.Count == 0)
            {
                strVoucherNumber = strVoucherNumber + "1";
            }
            else
            {
                double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                strVoucherNumber = strVoucherNumber + TotalCount;
            }
        }
        return strVoucherNumber;
    }
    public string Selected_Month_Year()
    {
        string Str_Selected_Month_Year = string.Empty;
        foreach (GridViewRow GVR in Grid_View_Loan_Detail_Control.Rows)
        {
            Label Lbl_Month = (Label)GVR.FindControl("lbl_Month");
            Label Lbl_Year = (Label)GVR.FindControl("lbl_year");
            Label Lbl_Total_Amount = (Label)GVR.FindControl("lblvaluetype2");
            CheckBox Chk_Gv_Select_LControl = (CheckBox)GVR.FindControl("Chk_Gv_Select_LControl");
            if (Chk_Gv_Select_LControl.Checked)
            {
                if (Str_Selected_Month_Year == "")
                    Str_Selected_Month_Year = Lbl_Month.Text + "-" + Lbl_Year.Text + ",";
                else
                    Str_Selected_Month_Year = Str_Selected_Month_Year + Lbl_Month.Text + "-" + Lbl_Year.Text + ",";
            }
        }

        return Str_Selected_Month_Year.TrimEnd(',');
    }
    public int Update_Loan_Detail(SqlTransaction trns)
    {
        int Loan_Update = 0;
        foreach (GridViewRow GVR in Grid_View_Loan_Detail_Control.Rows)
        {
            Label Lbl_Year = (Label)GVR.FindControl("lbl_year");
            Label Lbl_Previous_Balance = (Label)GVR.FindControl("lblPenaltynameList");
            Label Lbl_Monthly_Installment = (Label)GVR.FindControl("lblvaluetype1");
            Label Lbl_Total_Amount = (Label)GVR.FindControl("lblvaluetype2");
            CheckBox Chk_Gv_Select_LControl = (CheckBox)GVR.FindControl("Chk_Gv_Select_LControl");
            HiddenField Hdn_Loan_Detail_ID = (HiddenField)GVR.FindControl("Hdn_Loan_Detail_ID");
            HiddenField Hdn_Loan_ID = (HiddenField)GVR.FindControl("Hdn_Loan_ID");
            HiddenField Hdn_Month = (HiddenField)GVR.FindControl("Hdn_Month");
            if (Chk_Gv_Select_LControl.Checked)
            {
                Loan_Update = ObjLoan.Update_Pay_Employee_Loan_Detail(Session["CompId"].ToString(), Hdn_Loan_ID.Value, Hdn_Loan_Detail_ID.Value, Hdn_Month.Value, Lbl_Year.Text, "0.000000", "0.000000", "0.000000", "0.000000", "Paid", "1", ref trns);
            }
        }
        return Loan_Update;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["CurrencyId"].ToString();
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
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployee = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployee.GetEmployee_InPayroll(HttpContext.Current.Session["CompId"].ToString());
        DataTable dtAll = new DataTable();
        try
        {
            dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            dtAll = dt.Copy();
            dt = new DataView(dt, "Emp_Name like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
        }
        return str;
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
}