using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;
using DevExpress.XtraReports.UI;
using System.IO;

public partial class HR_Pay_Employee_Loan : BasePage
{
    SystemParameter objSys = null;
    DataAccessClass ObjDa = null;
    Set_Approval_Employee objApproalEmp = null;
    LocationMaster objLocation = null;
    Pay_Employee_Loan ObjLoan = null;
    EmployeeMaster ObjEmp = null;
    Common cmn = null;
    NotificationMaster Obj_Notifiacation = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_DocNumber objDocNo = null;
    Ac_ParameterMaster objAcParameter = null;
    CustomizedReport CR = null;
    PageControlCommon objPageCmn = null;

    private DataTable reportsTable;
    XtraReport rep = new XtraReport();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "19";
        Session["HeaderText"] = "HR";

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        CR = new CustomizedReport(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/Pay_Employee_Loan.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);

            ddldeductionstart.Items.Clear();
            ListItem li = new ListItem();
            li.Text = Common.GetMonthName(DateTime.Now.Month) + "-" + DateTime.Now.Year.ToString();
            li.Value = "0";
            ddldeductionstart.Items.Insert(0, "--Select--");
            ddldeductionstart.Items.Insert(1, li);
            li = new ListItem();
            li.Text = Common.GetMonthName(DateTime.Now.AddMonths(1).Month) + "-" + DateTime.Now.AddMonths(1).Year.ToString();
            li.Value = "1";
            ddldeductionstart.Items.Insert(2, li);
            ValidEmpList();
            GridBind();
            txtEmpName.Focus();
           
            try
            {
                GridViewLoan.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {
            }
            try
            {
                txtLSDebitAccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Loan Account").Rows[0]["Param_Value"].ToString());
            }
            catch
            {
            }
            Calender.Format = objSys.SetDateFormat();
        }
        Page.Title = objSys.GetSysTitle();

        if (hdnRPTClick.Value == "1")
        {
            getreport();
        }

    
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        BtnSave.Visible = clsPagePermission.bAdd;
        btnPayment.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    public string GetDate(string strDate)
    {
        return Convert.ToDateTime(strDate).ToString(objSys.SetDateFormat());
    }

  
    void ValidEmpList()
    {
        DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(ddlLocation.SelectedValue, "0", true, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["RoleId"].ToString(), Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());

        string EmpLIst = string.Empty;
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            if (EmpLIst.Trim() == "")
            {
                EmpLIst = dtEmp.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                EmpLIst = EmpLIst + "," + dtEmp.Rows[i]["Emp_Id"].ToString();
            }
        }
        Session["dtEmpList"] = EmpLIst;
    }

    void GridBind()
    {
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Pending");
        try
        {
            if (Session["dtEmpList"] != null)
            {
                Dt = new DataView(Dt, "Emp_Id in (" + Session["dtEmpList"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        if (Dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GridViewLoan, Dt, "", "");
            Session["dtFilter_Pay__Emp__Loan"] = Dt;
            lblTotalRecordsLoan.Text = Resources.Attendance.Total_Records + " : " + Dt.Rows.Count.ToString() + "";
        }
        else
        {
            DataTable Dtclear = new DataTable();
            objPageCmn.FillData((object)GridViewLoan, Dtclear, "", "");
            lblTotalRecordsLoan.Text = Resources.Attendance.Total_Records + " :0";
        }
       
    }

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtEmpName.Focus();
        txtLoanAmount.Text = "";
        txtEmpName.Text = "";
        txtLoanName.Text = "";
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {

        try
        {
            Convert.ToDateTime(txtRequestDate.Text);
        }
        catch
        {
            DisplayMessage("Enter valid date");
            txtRequestDate.Focus();
            return;
        }

        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtEmpName.Focus();
            return;
        }
        if (txtLoanName.Text == "")
        {
            DisplayMessage("Enter Loan Name");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtLoanName.Focus();
            return;
        }
        if (txtLoanAmount.Text == "")
        {
            DisplayMessage("Enter Loan amount");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtLoanAmount.Focus();
            return;
        }
        if (txtInterest.Text == "")
        {
            DisplayMessage("Enter Interest");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtInterest.Focus();
            return;
        }
        if (txtDuration.Text == "" || txtDuration.Text == "0")
        {
            DisplayMessage("Enter Duration");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtDuration.Focus();
            return;
        }
        if (txtGrossAmount.Text == "")
        {
            DisplayMessage("Press Tab Key for Gross Amount");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            txtDuration.Focus();
            return;
        }
        if (ddldeductionstart.SelectedIndex <= 0)
        {
            DisplayMessage("Select Deduction Start Month");
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            return;
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            DataTable Dt_Approval_Chain = new DataTable();
            Dt_Approval_Chain = objApproalEmp.getApprovalChainByObjectid("69", HidEmpId.Value.ToString(), ref trns, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (Dt_Approval_Chain.Rows.Count == 0)
            {
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                DisplayMessage("You Cannot Request For Loan First Set Approval For loan on Approval Master");
                BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
                return;
            }

            if (HiddeniD.Value == "")
            {
                int CheckInsertion = 0;
                CheckInsertion = ObjLoan.Insert_In_Pay_Employee_Loan(Session["CompId"].ToString(), HidEmpId.Value, txtLoanName.Text, objSys.getDateForInput(txtRequestDate.Text).ToString(), DateTime.Now.ToString(), txtLoanAmount.Text, txtDuration.Text, txtInterest.Text, txtGrossAmount.Text, txtMonthlyInstallment.Text, "Pending", "69", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (CheckInsertion > 0)
                {
                    if (Dt_Approval_Chain.Rows.Count > 0)
                    {
                        string PriorityEmpId = string.Empty;
                        string IsPriority = string.Empty;
                        string EmpPermission = string.Empty;
                        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Loan", ref trns).Rows[0]["Approval_Level"].ToString();
                        for (int j = 0; j < Dt_Approval_Chain.Rows.Count; j++)
                        {
                            PriorityEmpId = Dt_Approval_Chain.Rows[j]["Emp_Id"].ToString();
                            IsPriority = Dt_Approval_Chain.Rows[j]["Priority"].ToString();
                            int cur_trans_id = 0;
                            if (EmpPermission == "1")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), "0", "0", "0", HidEmpId.Value.ToString(), CheckInsertion.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "2")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", HidEmpId.Value.ToString(), CheckInsertion.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "3")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", HidEmpId.Value.ToString(), CheckInsertion.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", HidEmpId.Value.ToString(), CheckInsertion.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            Session["PriorityEmpId"] = PriorityEmpId;
                            Session["cur_trans_id"] = cur_trans_id;
                            Set_Notification(CheckInsertion.ToString(), ref trns);
                        }
                    }
                    Double Intereset = Convert.ToDouble(txtInterest.Text);
                    int Duration = Convert.ToInt32(txtDuration.Text);
                    Double LoanInstallmentAmount = Convert.ToDouble(txtMonthlyInstallment.Text);
                    DateTime dtFromdate = new DateTime(DateTime.Now.AddMonths(Convert.ToInt32(ddldeductionstart.SelectedValue)).Year, DateTime.Now.AddMonths(Convert.ToInt32(ddldeductionstart.SelectedValue)).Month, 1);
                    DateTime dtToDate = new DateTime();
                    if (Duration <= 0)
                    {
                        dtToDate = new DateTime(dtFromdate.Year, dtFromdate.Month, 1);
                    }
                    else
                    {
                        dtToDate = new DateTime(dtFromdate.AddMonths(Duration - 1).Year, dtFromdate.AddMonths(Duration - 1).Month, 1);
                    }
                    while (dtFromdate <= dtToDate)
                    {
                        ObjLoan.Insert_In_Pay_Employee_LoanDetail(CheckInsertion.ToString(), dtFromdate.Month.ToString(), dtFromdate.Year.ToString(), "0", txtMonthlyInstallment.Text, txtMonthlyInstallment.Text, "0", "Pending", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        dtFromdate = dtFromdate.AddMonths(1);
                    }
                    DisplayMessage("Record Saved Successfully","green");
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    GridBind();
                    txtEmpName.Focus();
                    Reset();
                    BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
                    return;
                }
            }
            else
            {
                // For Edit Update
                int b = ObjLoan.UpdateRecord_In_Pay_Employee_Loan(Session["CompId"].ToString(), HiddeniD.Value, txtLoanAmount.Text, objSys.getDateForInput(txtRequestDate.Text).ToString(), DateTime.Now.ToString(), txtDuration.Text, txtInterest.Text, txtGrossAmount.Text, txtMonthlyInstallment.Text, "Pending", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b == 1)
                {
                    DataTable DtLoan = ObjLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(HiddeniD.Value, ref trns);
                    if (DtLoan.Rows.Count > 0)
                    {
                        ObjLoan.DeleteRecord_From_PayEmployeeLoanDetail(HiddeniD.Value, ref trns);
                    }
                    int s = objApproalEmp.DeleteEmpLoanApproval("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), "0", HiddeniD.Value.ToString(), ref trns);

                    if (Dt_Approval_Chain.Rows.Count > 0)
                    {
                        string EmpPermission = string.Empty;
                        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Loan", ref trns).Rows[0]["Approval_Level"].ToString();

                        string PriorityEmpId = string.Empty;
                        string IsPriority = string.Empty;
                        for (int j = 0; j < Dt_Approval_Chain.Rows.Count; j++)
                        {

                            PriorityEmpId = Dt_Approval_Chain.Rows[j]["Emp_Id"].ToString();
                            IsPriority = Dt_Approval_Chain.Rows[j]["Priority"].ToString();

                            int cur_trans_id = 0;
                            if (EmpPermission == "1")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), "0", "0", "0", HidEmpId.Value.ToString(), HiddeniD.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "2")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", HidEmpId.Value.ToString(), HiddeniD.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "3")
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", HidEmpId.Value.ToString(), HiddeniD.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("5", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", HidEmpId.Value.ToString(), HiddeniD.Value, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            Session["PriorityEmpId"] = PriorityEmpId;
                            Session["cur_trans_id"] = cur_trans_id;
                            Set_Notification(b.ToString(), ref trns);
                        }
                    }
                    Double Intereset = Convert.ToDouble(txtInterest.Text);
                    int Duration = Convert.ToInt32(txtDuration.Text);
                    Double LoanInstallmentAmount = Convert.ToDouble(txtMonthlyInstallment.Text);
                    DateTime dtFromdate = new DateTime(DateTime.Now.AddMonths(Convert.ToInt32(ddldeductionstart.SelectedValue)).Year, DateTime.Now.AddMonths(Convert.ToInt32(ddldeductionstart.SelectedValue)).Month, 1);
                    DateTime dtToDate = new DateTime();
                    if (Duration <= 0)
                    {
                        dtToDate = new DateTime(dtFromdate.Year, dtFromdate.Month, 1);
                    }
                    else
                    {
                        dtToDate = new DateTime(dtFromdate.AddMonths(Duration - 1).Year, dtFromdate.AddMonths(Duration - 1).Month, 1);
                    }

                    while (dtFromdate <= dtToDate)
                    {
                        ObjLoan.Insert_In_Pay_Employee_LoanDetail(HiddeniD.Value, dtFromdate.Month.ToString(), dtFromdate.Year.ToString(), "0", txtMonthlyInstallment.Text, txtMonthlyInstallment.Text, "0", "Pending", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        dtFromdate = dtFromdate.AddMonths(1);
                    }
                    DisplayMessage("Record Update Successfully");
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    GridBind();
                    txtEmpName.Focus();
                    Reset();
                    BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
                }
                else
                {
                    DisplayMessage("Loan Is already Approved,You Can Not Update");
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
                    return;
                }
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
            BtnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(BtnSave, "").ToString());
            return;
        }
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

    protected void btnEdit_command(object sender, CommandEventArgs e)
    {
        HiddeniD.Value = e.CommandArgument.ToString();
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), HiddeniD.Value, "Pending");
        if (Dt.Rows.Count > 0)
        {
            try
            {
                DataTable dtApproved = objApproalEmp.GetApprovalChild("0", "5");
                try
                {
                    dtApproved = new DataView(dtApproved, "Approval_Type='Loan'   and Ref_id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                bool IsAllow = true;
                foreach (DataRow dr in dtApproved.Rows)
                {
                    if (dr["Status"].ToString() != "Pending")
                    {
                        IsAllow = false;
                    }
                }
                if (!IsAllow)
                {
                    DisplayMessage("You Can not Edit ,Used in Approval");
                    HiddeniD.Value = "";
                    return;
                }
            }
            catch
            {
            }
            HidEmpId.Value = Dt.Rows[0]["Emp_Id"].ToString();
            txtEmpName.Text = Dt.Rows[0]["Emp_Name"].ToString();
            txtLoanName.Text = Dt.Rows[0]["Loan_Name"].ToString();
            txtLoanAmount.Text = Common.GetAmountDecimal(Dt.Rows[0]["Loan_Amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtRequestDate.Text = Convert.ToDateTime(Dt.Rows[0]["Loan_Request_Date"].ToString()).ToString(objSys.SetDateFormat());
            txtDuration.Text = Dt.Rows[0]["Loan_Duration"].ToString();
            txtInterest.Text = Common.GetAmountDecimal(Dt.Rows[0]["Loan_Interest"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtGrossAmount.Text = Common.GetAmountDecimal(Dt.Rows[0]["Gross_Amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtMonthlyInstallment.Text = Common.GetAmountDecimal(Dt.Rows[0]["Monthly_Installment"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            DataTable dtLoanDetail = ObjLoan.GetRecord_From_PayEmployeeLoanDetailAll();
            dtLoanDetail = new DataView(dtLoanDetail, "Loan_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtLoanDetail.Rows.Count > 0)
            {
                ddldeductionstart.SelectedValue = ddldeductionstart.Items.FindByText(Common.GetMonthName(Convert.ToInt32(dtLoanDetail.Rows[0]["Month"].ToString())) + "-" + dtLoanDetail.Rows[0]["Year"].ToString()).Value;
            }
            else
            {
                ddldeductionstart.SelectedValue = "--Select--";
            }
            hidLoanType.Value = Dt.Rows[0]["Field1"].ToString();
            Lbl_Loan_Tab.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Edit_Tab_Position()", true);
        }
        txtLoanName.Focus();
    }

    protected void IbtnDelete_command(object sender, CommandEventArgs e)
    {
        DataTable dtEmpTrans = new DataTable();
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), e.CommandArgument.ToString(), "Pending");
        if (Dt.Rows.Count > 0)
        {
            try
            {
                DataTable dtApproved = objApproalEmp.GetApprovalChild("0", "5");
                try
                {
                    dtApproved = new DataView(dtApproved, "Approval_Type='Loan'   and Ref_id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                bool IsAllow = true;
                foreach (DataRow dr in dtApproved.Rows)
                {
                    if (dr["Status"].ToString() != "Pending")
                    {
                        IsAllow = false;
                    }
                }
                if (!IsAllow)
                {
                    DisplayMessage("You Can not Delete,Used in Approval");
                    return;
                }
            }
            catch
            {
            }
        }
        HiddeniD.Value = e.CommandArgument.ToString();
        int CheckDeletion = 0;
        CheckDeletion = ObjLoan.DeleteRecord_in_Pay_Employee_Loan(Session["CompId"].ToString(), HiddeniD.Value, "Cancelled", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (dtEmpTrans.Rows.Count > 0)
            objApproalEmp.DeleteApprovalTransaciton("Loan", HiddeniD.Value, Dt.Rows[0]["Field1"].ToString(), dtEmpTrans.Rows[0]["Approval_Id"].ToString());
        DisplayMessage("Record Deleted");
        DataTable dtGrid = new DataTable();
        GridBind();
    }

    void Reset()
    {
        HiddeniD.Value = "";
        HidEmpId.Value = "";
        hidLoanType.Value = "";
        txtMonthlyInstallment.Text = "";
        txtInterest.Text = "";
        txtDuration.Text = "";
        txtGrossAmount.Text = "";
        txtEmpName.Text = "";
        txtLoanName.Text = "";
        txtLoanAmount.Text = "";
        txtEmpName.Focus();
        ddldeductionstart.SelectedValue = "--Select--";
        txtRequestDate.Text = "";
    }

    protected void TxtEmpName_TextChanged(object sender, EventArgs e)
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
                empid = "0";
            }
            DataTable dtEmp = ObjEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                HidEmpId.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                HidEmpId.Value = "";
                return;
            }
        }
        txtLoanName.Focus();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployee = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Common.GetEmployeeListbyLocationIdandDepartmentValue(HttpContext.Current.Session["LocId"].ToString(), "0", true, HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());
        DataTable dtAll = new DataTable();
        try
        {

            dtAll = dt.Copy();
            dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i]["Emp_Name"].ToString() + "/(" + dt.Rows[i]["Designation"].ToString() + ")/" + dt.Rows[i]["Emp_Code"].ToString() + "";
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetLoanName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Pay_Employee_Loan.GetLoanName("", HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());
        dt = new DataView(dt, "Loan_Name lIKE '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i][0].ToString();
        }
        return str;
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }
        return EmployeeName;
    }

    public string GetEmployeeName(string EmployeeId, ref SqlTransaction trns)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }
        return EmployeeName;
    }

    private void Set_Notification(string Ref_ID, ref SqlTransaction trns)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/hr"));
        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, HidEmpId.Value, Session["PriorityEmpId"].ToString(), ref trns);
        GetEmployeeName(HidEmpId.Value, ref trns);
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = txtEmpName.Text + " applied Loan Request for " + txtLoanName.Text;
        Save_Notification = Obj_Notifiacation.DeleteNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), HidEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Ref_ID.ToString(), "16", ref trns);
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), HidEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0", ref trns);
    }

    protected void TxtDuration_TextChanged(object sender, EventArgs e)
    {
        if (txtLoanAmount.Text == "")
        {
            txtLoanAmount.Text = "0";
        }
        else
        {
            try
            {
                Convert.ToDouble(txtLoanAmount.Text);
            }
            catch
            {
                txtLoanAmount.Text = "0";
            }
        }

        if (txtInterest.Text == "")
        {
            txtInterest.Text = "0";
        }


        try
        {
            Convert.ToDouble(txtInterest.Text);
        }
        catch
        {
            txtInterest.Text = "0";
        }

        if (txtDuration.Text == "")
        {
            txtDuration.Text = "0";
        }
        Double Intereset = Convert.ToDouble(txtInterest.Text);
        int Duration = Convert.ToInt32(txtDuration.Text);
        Double LoanAmount = Convert.ToDouble(txtLoanAmount.Text);
        Double GrossAmount = (LoanAmount * Duration * Intereset) / (12 * 100);
        Double totalGrossAmount = GrossAmount + LoanAmount;
        txtGrossAmount.Text = Common.GetAmountDecimal(totalGrossAmount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        if (Duration <= 0)
        {
            txtMonthlyInstallment.Text = totalGrossAmount.ToString();
        }
        else
        {
            txtMonthlyInstallment.Text = Common.GetAmountDecimal((totalGrossAmount / Duration).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }


        if (((TextBox)sender).ID == "txtLoanAmount")
        {
            txtInterest.Focus();
        }
        else if (((TextBox)sender).ID == "txtInterest")
        {
            txtDuration.Focus();
        }
        else if (((TextBox)sender).ID == "txtDuration")
        {
            ddldeductionstart.Focus();
        }

    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        txtEmpName.Enabled = true;
        txtLoanName.Enabled = true;
        txtLoanAmount.Enabled = true;
        txtDuration.Enabled = true;
        txtInterest.Enabled = true;
        ddldeductionstart.Enabled = true;
        BtnSave.Visible = true;
        BtnReset.Visible = true;
        Reset();
        Lbl_Loan_Tab.Text = Resources.Attendance.Loan;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Cancel_Tab_Position()", true);
    }

    protected void GridViewLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewLoan.PageIndex = e.NewPageIndex;
        GridBind();
      
        GridViewLoan.HeaderRow.Focus();
    }

    protected void btnLoanbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["dtFilter_Pay__Emp__Loan"] != null)
        {
            if (ddlOption1.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption1.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
                }
                else if (ddlOption1.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";
                }
                DataTable dtEmp = (DataTable)Session["dtFilter_Pay__Emp__Loan"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                objPageCmn.FillData((object)GridViewLoan, view.ToTable(), "", "");
                lblTotalRecordsLoan.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            }
        }
       
        txtValue1.Focus();
    }

    protected void btnLoanRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        GridBind();
       
        txtValue1.Focus();
    }

    protected void GridViewLoan_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pay__Emp__Loan"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pay__Emp__Loan"] = dt;
        objPageCmn.FillData((object)GridViewLoan, dt, "", "");
        
        GridViewLoan.HeaderRow.Focus();
    }

    protected void btnGetRecord_Click(object sender, EventArgs e)
    {
        divPending.Visible = false;
        txtEmpName_Payment.Enabled = true;
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
        Dt = new DataView(Dt, "Emp_Id=" + HidEmpId.Value + " and (Field2='False' or Field2='') ", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)gvPayment, Dt, "", "");
        if (Dt.Rows.Count > 0)
        {
            divPending.Visible = true;
            txtEmpName_Payment.Enabled = false;
        }
        else
        {
            DisplayMessage("Record Not Found");
        }
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow1 = (GridViewRow)((CheckBox)sender).Parent.Parent;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            if (gvrow.RowIndex != gvrow1.RowIndex)
            {
                ((CheckBox)gvrow.FindControl("chkselect")).Checked = false;
            }
        }
    }

    protected void btnPayment_Click(object sender, EventArgs e)
    {
        string strnarration = string.Empty;
        string Loan_Installment = string.Empty;
        string Loan_Interest = string.Empty;
        string LoanName = string.Empty;

        double PaidTotaamount = 0;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string strLoanId = string.Empty;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
            {
                PaidTotaamount += Convert.ToDouble(((Label)gvrow.FindControl("lblvaluetype")).Text);
                strLoanId = ((Label)gvrow.FindControl("lblTransId")).Text;
            }
        }
        DataTable Dt_Loan_Details = ObjLoan.Get_Employee_Loan(strLoanId, "0", "0", "Pending", "Pending", "3");
        if (Dt_Loan_Details != null && Dt_Loan_Details.Rows.Count > 0)
        {
            LoanName = Dt_Loan_Details.Rows[0]["Loan_Name"].ToString();
            Loan_Installment = Dt_Loan_Details.Rows[0]["Loan_Duration"].ToString();
            Loan_Interest = Dt_Loan_Details.Rows[0]["Loan_Interest"].ToString();
        }
        strnarration = "Loan Payment For " + txtEmpName_Payment.Text.Split('/')[0].ToString() + ", " + LoanName + ", " + Loan_Interest + "%, " + Loan_Installment + " Installment";
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            string strSQL = string.Empty;
            int b = 0;
            int MaxId = 0;
            if (PaidTotaamount != 0)
            {
                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
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
                //string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());

                string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "304", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
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



                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", strLoanId, "LS", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "PV", "1/1/1800", "1/1/1800", "", strnarration, strCurrencyId, "1", strnarration, false.ToString(), false.ToString(), false.ToString(), "PV", "", "", HidEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = MaxId.ToString();
                ObjDa.execute_Command("update pay_employee_loan set Field2='True',Field3='" + strVMaxId + "' where Loan_Id=" + strLoanId + "", ref trns);
                double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));
                string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";
                if (strAccountId.Split(',').Contains(txtLSDebitAccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSDebitAccount.Text.Split('/')[1].ToString(), HidEmpId.Value, strLoanId, "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", strnarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSDebitAccount.Text.Split('/')[1].ToString(), HidEmpId.Value, strLoanId, "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", strnarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                if (strAccountId.Split(',').Contains(txtLSCreditaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSCreditaccount.Text.Split('/')[1].ToString(), "0", strLoanId, "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, strnarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSCreditaccount.Text.Split('/')[1].ToString(), "0", strLoanId, "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, strnarration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            DisplayMessage("Record Saved","green");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnPaymentCancel_Click(null, null);
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

    protected void btnPaymentCancel_Click(object sender, EventArgs e)
    {
        divPending.Visible = false;
        txtEmpName_Payment.Text = "";
        HidEmpId.Value = "";
        txtEmpName_Payment.Enabled = true;
        objPageCmn.FillData((object)gvPayment, null, "", "");
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
                DataTable dtCOA = ObjDa.return_DataTable(sql);
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

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";
            DataTable dtAccName = ObjDa.return_DataTable(sql);
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

    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), e.CommandArgument.ToString(), "Pending");
        if (Dt.Rows.Count > 0)
        {
            txtEmpName.Text = Dt.Rows[0]["Emp_Name"].ToString();
            txtLoanName.Text = Dt.Rows[0]["Loan_Name"].ToString();
            txtLoanAmount.Text = Common.GetAmountDecimal(Dt.Rows[0]["Loan_Amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtDuration.Text = Dt.Rows[0]["Loan_Duration"].ToString();
            txtInterest.Text = Common.GetAmountDecimal(Dt.Rows[0]["Loan_Interest"].ToString(),Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtGrossAmount.Text = Common.GetAmountDecimal(Dt.Rows[0]["Gross_Amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtMonthlyInstallment.Text = Common.GetAmountDecimal(Dt.Rows[0]["Monthly_Installment"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            DataTable dtLoanDetail = ObjLoan.GetRecord_From_PayEmployeeLoanDetailAll();
            dtLoanDetail = new DataView(dtLoanDetail, "Loan_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtLoanDetail.Rows.Count > 0)
            {
                ddldeductionstart.SelectedValue = ddldeductionstart.Items.FindByText(Common.GetMonthName(Convert.ToInt32(dtLoanDetail.Rows[0]["Month"].ToString())) + "-" + dtLoanDetail.Rows[0]["Year"].ToString()).Value;
            }
            else
            {
                ddldeductionstart.SelectedValue = "--Select--";
            }
            Lbl_Loan_Tab.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Edit_Tab_Position()", true);

            txtEmpName.Enabled = false;
            txtLoanName.Enabled = false;
            txtLoanAmount.Enabled = false;
            txtDuration.Enabled = false;
            txtInterest.Enabled = false;
            ddldeductionstart.Enabled = false;
            BtnSave.Visible = false;
            BtnReset.Visible = false;
        }
    }

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        hdnLoanId.Value = e.CommandArgument.ToString();
        hdnRPTClick.Value = "1";
        getreport();
        ScriptManager.RegisterStartupScript(this, GetType(), "", "openLoanRequestReport();", true);

    }

    public void getreport()
    {
        if (hdnLoanId.Value == "0" || hdnLoanId.Value == "")
        {
            return;
        }

        reportsTable = CR.getReportsById("51");
        if (reportsTable.Rows.Count > 0)
        {
            byte[] reportData = (Byte[])reportsTable.Rows[0]["LayoutData"];
            MemoryStream ms = new MemoryStream(reportData);
            rep.LoadLayoutFromXml(ms);
        }

        try
        {
            rep.Parameters["LoanId"].Value = hdnLoanId.Value;
        }
        catch
        {

        }
        rep.DataSource = ObjDa.return_DataTable("select Pay_Employee_Loan.*,Set_EmployeeMaster.Emp_Name,(select dbo.fn_AmountToWords(Pay_Employee_Loan.Loan_Amount,Set_LocationMaster.Field1)) as Loan_amt_inwords ,(select dbo.fn_AmountToWords(Pay_Employee_Loan.Monthly_Installment,Set_LocationMaster.Field1)) as Monthly_installment_inwords,Set_DesignationMaster.Designation,Set_EmployeeMaster.Civil_Id,Set_EmployeeMaster.FatherName, Set_EmployeeMaster.Pan,emp_address.Address_Name,company.* from Pay_Employee_Loan left join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id=Pay_Employee_Loan.Emp_Id left join Set_LocationMaster on Set_LocationMaster.Location_Id=Set_EmployeeMaster.Location_Id left join Set_DesignationMaster on Set_DesignationMaster.Designation_Id=Set_EmployeeMaster.Designation_Id left join (select Set_AddressMaster.address_name,Set_AddressChild.ref_id from Set_AddressChild inner join Set_AddressMaster on Set_AddressMaster.Trans_Id=Set_AddressChild.Add_Ref_Id where Set_AddressChild.add_type='Employee' )emp_address on emp_address.Ref_Id=Set_EmployeeMaster.Emp_Id inner join (SELECT Set_CompanyMaster.Company_Id,Set_CompanyMaster.Company_Name AS HeaderName,Set_CompanyMaster.Company_Name_L AS HeaderName_L, Set_AddressMaster.Address FROM Set_CompanyMaster FULL OUTER JOIN Set_AddressChild ON Set_CompanyMaster.Company_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Company' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id) company on company.Company_Id=Pay_Employee_Loan.Company_Id where Pay_Employee_Loan.loan_id='" + hdnLoanId.Value + "'");
        rep.CreateDocument();
        ReportViewer1.OpenReport(rep);
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidEmpList();
        GridBind();
    }
}