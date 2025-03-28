using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class HR_RollBackPayroll : BasePage
{
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Pay_Employee_Month objPayEmpMonth = null;
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    EmployeeMaster objEmp = null;
    HR_Leave_Salary objLeaveSalary = null;
    DataAccessClass ObjDa = null;
    Common cmn = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_Employe_Allowance objpayrollall = null;
    Pay_Employee_Deduction objpayrolldeduc = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {

            Response.Redirect("~/ERPLogin.aspx");
        }

        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objLeaveSalary = new HR_Leave_Salary(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objpayrolldeduc = new Pay_Employee_Deduction(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/RollBackPayroll.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlDeaprtment();
            FillddlLocation();
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";
            lblSelectRecd.Text = "";
            GetEmplyeeData();

            //this code is created by jitendra upadhyay on 17-07-2014
            //this code for set the gridview page size according the system parameter
            try
            {
                gvEmpRollBack.PageSize = int.Parse(Session["GridSize"].ToString());

            }
            catch
            {

            }

        }
        AllPageCode();
    }
    public void AllPageCode()
    {


        Session["AccordianId"] = "111";
        Session["HeaderText"] = "HR";
    }


    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
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
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }

    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D",Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }

        dt = dt.DefaultView.ToTable(true, "DeptName", "Dep_Id");



        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)dpDepartment, dt, "DeptName", "Dep_Id");
        }
        else
        {
            try
            {
                dpDepartment.Items.Clear();
                dpDepartment.DataSource = null;
                dpDepartment.DataBind();
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
            catch
            {
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
        }
    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGrid();
                    lblSelectRecd.Text = "";
                    ddlField1.SelectedIndex = 1;
                    ddlOption1.SelectedIndex = 2;
                    txtValue1.Text = "";
                }
                else
                {

                    dt = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

                    if (ddlLocation.SelectedIndex == 0)
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    try
                    {
                        if (Session["SessionDepId"] != null)
                        {
                            dt = new DataView(dt, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                        }
                    }
                    catch
                    {
                    }
                    if (dpDepartment.SelectedIndex != 0)
                    {
                        try
                        {
                            dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 23-05-2015
                        objPageCmn.FillData((object)gvEmpRollBack, dt, "", "");
                        Session["dtEmpRollBack"] = dt;
                        lblTotalRecordsRollBack.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                    else
                    {
                        gvEmpRollBack.DataSource = null;
                        gvEmpRollBack.DataBind();
                        Session["dtEmpRollBack"] = dt;
                        lblTotalRecordsRollBack.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                }





            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";



    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();



    }
    public string getRollBackMonthYear(string EmployeeId)
    {
        string EmpStatus = string.Empty;
        DataTable Dt = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["CompId"].ToString());
        Dt = new DataView(Dt, "Emp_Id=" + EmployeeId + "", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt.Rows.Count > 0)
        {
            EmpStatus = "0";
            try
            {
                Dt = new DataView(Dt, "", "Year desc", DataViewRowState.CurrentRows).ToTable();
                string sortYear = Dt.Rows[0]["Year"].ToString();
                Dt = new DataView(Dt, "Year=" + sortYear + "", "Month desc", DataViewRowState.CurrentRows).ToTable();
                ddlMonth.SelectedValue = Dt.Rows[0]["Month"].ToString();
                TxtYear.Text = Dt.Rows[0]["Year"].ToString();
            }
            catch
            {

            }
        }
        return EmpStatus;
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
    public void FillGrid()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());
        
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpRollBack"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpRollBack, dtEmp, "", "");
            lblTotalRecordsRollBack.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        Session["dtRollBack"] = null;
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";


    }
    public void GetEmplyeeData()
    {
        DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(Session["LocId"].ToString(), "0", true, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["RoleId"].ToString(), Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());

       
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpRollBack"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpRollBack, dtEmp, "", "");
            lblTotalRecordsRollBack.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }
        else
        {
            Session["dtEmpRollBack"] = null;
            gvEmpRollBack.DataSource = null;
            gvEmpRollBack.DataBind();
        }
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";
    }
    protected void gvEmpRollBack_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmpRollBack.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpRollBack"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpRollBack, dtEmp, "", "");
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvEmpRollBack.Rows.Count; i++)
        {
            Label lblconid = (Label)gvEmpRollBack.Rows[i].FindControl("lblEmpId");
            string[] split = lblSelectRecd.Text.Split(',');

            for (int j = 0; j < lblSelectRecd.Text.Split(',').Length; j++)
            {
                if (lblSelectRecd.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectRecd.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvEmpRollBack.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        Session["dtRollBack"] = null;


    }

    protected void btnRollBackbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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
            DataTable dtEmp = (DataTable)Session["dtEmpRollBack"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            DataTable dtTotalRecord = view.ToTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpRollBack, view.ToTable(), "", "");
            lblTotalRecordsRollBack.Text = Resources.Attendance.Total_Records + " : " + dtTotalRecord.Rows.Count.ToString() + "";
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";
        }
        Session["dtRollBack"] = null;
        txtValue1.Focus();

    }
    protected void BtnRollBackPayroll_Click(object sender, EventArgs e)
    {
        if (lblSelectRecd.Text == "")
        {
            DisplayMessage("Select Employee First");
            return;
        }
        if (ddlMonth.SelectedIndex == 0 || TxtYear.Text == "")
        {
            DisplayMessage("Employee Has no Posted Payroll");
            return;
        }


        string strTransId = string.Empty;
        string strempCode = string.Empty;
        string strloanVoucherId = string.Empty;
        DataTable dTtemp = new DataTable();
        string strLeaveVoucherNo = string.Empty;

        //Add Taken Leave Salary Concept On 10-09-2015
        DataTable dtLeaveSalary = objLeaveSalary.GetAllLeaveSalary(lblSelectRecd.Text);
        if (dtLeaveSalary.Rows.Count > 0)
        {
            dtLeaveSalary = new DataView(dtLeaveSalary, "L_Month='" + ddlMonth.SelectedIndex + "' and L_Year='" + TxtYear.Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dtLeaveSalary.Rows.Count > 0)
            {
                //bool IsReport = Convert.ToBoolean(dtLeaveSalary.Rows[0]["Is_Report"].ToString());
                //string strDate = dtLeaveSalary.Rows[0]["F1"].ToString();

                //if (IsReport && strDate != "")
                //{
                ddlMonth.SelectedIndex = 0;
                TxtYear.Text = "";
                FillGrid();
                lblSelectRecd.Text = "";
                DisplayMessage("Employee Already take Leave Salary for This Month so you cant RollBack for that Employee");
                return;
                //}
            }
        }

        dTtemp = ObjDa.return_DataTable("select pay_employe_month.Field10,pay_employe_month.Leave_Voucher_No,set_employeemaster.Emp_Code,isnull(pay_employe_month.Loan_Voucher_No,0) as Loan_Voucher_No from pay_employe_month inner join set_employeemaster on  pay_employe_month.Emp_Id=Set_EmployeeMaster.Emp_Id  where pay_employe_month.emp_id='" + lblSelectRecd.Text + "' and pay_employe_month.month='" + ddlMonth.SelectedValue.Trim() + "' and pay_employe_month.year='" + TxtYear.Text.Trim() + "'");

        if (dTtemp.Rows.Count > 0)
        {
            strTransId = dTtemp.Rows[0]["Field10"].ToString();
            strempCode = dTtemp.Rows[0]["Emp_Code"].ToString();
            strloanVoucherId = dTtemp.Rows[0]["Loan_Voucher_No"].ToString();
            strLeaveVoucherNo = dTtemp.Rows[0]["Leave_Voucher_No"].ToString();
        }


        if (strTransId == "")
        {
            strTransId = "0";
        }

        if (strLeaveVoucherNo == "")
        {
            strLeaveVoucherNo = "0";
        }
        //if (strTransId.ToString() != "0")
        //{
        //    string RelativeSalaryDeleted = IsRelativeVoucherDeleted(strTransId);
        //    string RelativeSalaryPosted = IsRelativeVoucherPost(strTransId);
        //    if (RelativeSalaryDeleted != "")
        //    {
        //        DisplayMessage(RelativeSalaryDeleted);
        //        return;
        //    }
        //    if (RelativeSalaryPosted != "")
        //    {
        //        DisplayMessage(RelativeSalaryPosted);
        //        return;
        //    }
        //}

        //if (strloanVoucherId.ToString() != "0")
        //{
        //    string RelativeLoanDeleted = IsRelativeVoucherDeleted(strloanVoucherId);
        //    string RelativeLoanPosted = IsRelativeVoucherPost(strloanVoucherId);
        //    if (RelativeLoanDeleted != "")
        //    {
        //        DisplayMessage(RelativeLoanDeleted);
        //        return;
        //    }
        //    if (RelativeLoanPosted != "")
        //    {
        //        DisplayMessage(RelativeLoanPosted);
        //        return;
        //    }
        //}

        if (!IsSalaryVoucherPost(strTransId, lblSelectRecd.Text, strempCode, true))
        {
            DisplayMessage("Finance voucher posted , you can not process for rollback");
            return;
        }


        if (strloanVoucherId != "0")
        {
            if (!IsLOanVoucherPost(strloanVoucherId, lblSelectRecd.Text, strempCode))
            {
                DisplayMessage("Loan voucher posted , you can not process for rollback");
                return;
            }
        }

        //for rollback leave voucher
        //added on 21/03/2018 by jitendra upadhyay

        if (strLeaveVoucherNo != "0")
        {
            if (!IsVoucherPost(strLeaveVoucherNo, lblSelectRecd.Text, strempCode, true))
            {
                DisplayMessage("Loan voucher posted , you can not process for rollback");
                return;
            }
        }


        //for rollback allowance voucher
        //added on 21/03/2018 by jitendra upadhyay

        DataTable dtpostedVoucher = objpayrollall.GetPostedAllowanceAll(lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text);

        dtpostedVoucher = new DataView(dtpostedVoucher, "Field10<>'0'", "", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow dr in dtpostedVoucher.Rows)
        {
            if (!IsVoucherPost(dr["Field10"].ToString(), lblSelectRecd.Text, strempCode, true))
            {
                DisplayMessage("Allowances voucher posted , you can not process for rollback");
                return;
            }
        }



        //for rollback deduction voucher
        //added on 21/03/2018 by jitendra upadhyay

        dtpostedVoucher = objpayrolldeduc.GetRecordPostedDeductionAll(lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text);

        dtpostedVoucher = new DataView(dtpostedVoucher, "Field10<>'0'", "", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow dr in dtpostedVoucher.Rows)
        {
            if (!IsVoucherPost(dr["Field10"].ToString(), lblSelectRecd.Text, strempCode, false))
            {
                DisplayMessage("Deduction voucher posted , you can not process for rollback");
                return;
            }
        }




        //DataTable dtpostedAllowances = objpayrolldeduc.GetRecordPostedDeductionAll(lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text);




        string strLoandetailId = "0";
        DataTable Dtloan = new DataTable();
        Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");

        Dtloan = new DataView(Dtloan, " Emp_Id=" + lblSelectRecd.Text + "", "", DataViewRowState.CurrentRows).ToTable();
        for (int i = 0; i < Dtloan.Rows.Count; i++)
        {
            DataTable dtloandetial = new DataTable();
            dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString());
            dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtloandetial.Rows.Count > 0)
            {
                double loan = 0;
                if (dtloandetial.Rows[0]["Total_Amount"].ToString() != "")
                {
                    loan = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"]);
                }
                strLoandetailId = dtloandetial.Rows[0]["Trans_Id"].ToString();
                //if (strloanVoucherId != "0")
                //{
                objEmpLoan.UpdateRecord_loandetials_WithPaidStatusandAmount(Dtloan.Rows[i]["Loan_Id"].ToString(), dtloandetial.Rows[0]["Trans_Id"].ToString(), "0.000000", "Pending");
                //}
            }
            //if (strloanVoucherId != "0")
            //{
            ObjDa.execute_Command("update Pay_Employee_Loan_Detail set Previous_Balance='0.000000',Total_Amount=Montly_Installment, Is_Status='Pending',Employee_Paid='0.000000' where Loan_Id=" + Dtloan.Rows[i]["Loan_Id"].ToString() + " and Trans_Id>" + strLoandetailId + "");
            //}
        }

        if (chkRollbackEmp.Checked == false)
        {
            int b = objPayEmpMonth.RollBackTransaction(Session["CompId"].ToString(), lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text);
        }
        else
        {
            int b = objPayEmpMonth.RollBackEmpTransaction(lblSelectRecd.Text);
        }

        DisplayMessage("Payroll is RollBack successfully");
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";
        FillGrid();
        lblSelectRecd.Text = "";
    }

    public string IsRelativeVoucherPost(string Trans_Id)
    {
        string Result = string.Empty;
        DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(Trans_Id, "1");
        if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
        {
            if (Dt_Sal_Loan.Rows.Count > 1)
            {
                foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                {
                    if (Convert.ToBoolean(Dr_Loan["ReconciledFromFinance"].ToString()) == true)
                    {
                        if (Dr_Loan["Field5"].ToString() == "")
                        {
                            Result = "Salary Voucher is Posted, So it cannot be Rollback !";
                            break;
                        }
                        else
                        {
                            Result = "Loan Voucher is Posted, So it cannot be Rollback !";
                            break;
                        }
                    }
                }
            }
        }
        return Result;
    }

    public string IsRelativeVoucherDeleted(string Trans_Id)
    {
        string Result = string.Empty;
        DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(Trans_Id, "1");
        if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
        {
            if (Dt_Sal_Loan.Rows.Count > 1)
            {
                foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                {
                    if (Convert.ToBoolean(Dr_Loan["IsActive"].ToString()) == false)
                    {
                        if (Dr_Loan["Field5"].ToString() == "")
                        {
                            Result = "Salary Voucher is Deleted, So it cannot be Rollback !";
                            break;
                        }
                        else
                        {
                            Result = "Loan Voucher is Deleted, So it cannot be Rollback !";
                            break;
                        }
                    }
                }
            }
        }
        return Result;
    }

    public bool IsVoucherPost(string strVoucherheaderId, string strEmpId, string strEmpCode, bool IsEmployeeCredit)
    {

        DataTable dtVoucherdetail = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dt = new DataTable();
        string strNarration = string.Empty;
        double DebitAmount = 0;
        double CreditAmount = 0;
        bool Result = true;

        dt = ObjDa.return_DataTable("select Trans_Id from Ac_Voucher_Header where Ac_Voucher_Header.TRans_Id=" + strVoucherheaderId + " and ReconciledFromFinance='True'");

        if (dt.Rows.Count > 0)
        {
            Result = false;
        }
        else
        {



            dtVoucherdetail = ObjDa.return_DataTable("select  * from ac_voucher_detail where Voucher_No=" + strVoucherheaderId + "");

            dtTemp = new DataView(dtVoucherdetail, "Other_Account_No<>'0'", "", DataViewRowState.CurrentRows).ToTable();



            if (dtTemp.Rows.Count == 1)
            {
                ObjDa.execute_Command("update  Ac_Voucher_Header   set IsActive='False'    where Trans_id=" + strVoucherheaderId + "");
            }
            else
            {




                if (IsEmployeeCredit)
                {
                    CreditAmount = Convert.ToDouble(new DataView(dtVoucherdetail, "Other_Account_No=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Credit_Amount"].ToString());
                    DebitAmount = Convert.ToDouble(new DataView(dtVoucherdetail, "Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Debit_Amount"].ToString());
                    strNarration = new DataView(dtVoucherdetail, "Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Narration"].ToString();

                    ObjDa.execute_Command("update  ac_voucher_detail   set Debit_Amount=" + (DebitAmount - CreditAmount).ToString() + ",Companycurrdebit=" + (DebitAmount - CreditAmount).ToString() + ",Narration='" + strNarration.Replace(strEmpCode + ",", "").Replace("'", "") + "'  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No='0'");
                }
                else
                {
                    DebitAmount = Convert.ToDouble(new DataView(dtVoucherdetail, "Other_Account_No=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Debit_Amount"].ToString());
                    CreditAmount = Convert.ToDouble(new DataView(dtVoucherdetail, "Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Credit_Amount"].ToString());
                    strNarration = new DataView(dtVoucherdetail, "Other_Account_No='0'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Narration"].ToString();

                    ObjDa.execute_Command("update  ac_voucher_detail   set Credit_Amount=" + (CreditAmount - DebitAmount).ToString() + ",CompanycurrCredit=" + (CreditAmount - DebitAmount).ToString() + ",Narration='" + strNarration.Replace(strEmpCode + ",", "").Replace("'", "") + "'  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No='0'");
                }

                ObjDa.execute_Command("Delete from  ac_voucher_detail  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No=" + strEmpId + "");

            }


        }


        dt.Dispose();
        dtVoucherdetail.Dispose();
        dtTemp.Dispose();
        return Result;
    }

    public bool IsSalaryVoucherPost(string strVoucherheaderId, string strEmpId, string strEmpCode, bool IsEmployeeCredit)
    {
        DataTable dtVoucherdetail = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dt = new DataTable();
        string strNarration = string.Empty;
       
        double CreditAmount = 0;
        bool Result = true;

        dt = ObjDa.return_DataTable("select Trans_Id from Ac_Voucher_Header where Ac_Voucher_Header.TRans_Id=" + strVoucherheaderId + " and ReconciledFromFinance='True'");

        if (dt.Rows.Count > 0)
        {
            Result = false;
        }
        else
        {



            dtVoucherdetail = ObjDa.return_DataTable("select  * from ac_voucher_detail where Voucher_No=" + strVoucherheaderId + "");

            dtTemp = new DataView(dtVoucherdetail, "Other_Account_No<>'0'", "", DataViewRowState.CurrentRows).ToTable();


            if (dtTemp.Rows.Count == 1)
            {
                ObjDa.execute_Command("update  Ac_Voucher_Header   set IsActive='False'    where Trans_id=" + strVoucherheaderId + "");
            }
            else
            {
                ObjDa.execute_Command("Delete from  ac_voucher_detail  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No=" + strEmpId + "");

                dtTemp = new DataView(dtVoucherdetail, "Other_Account_No<>'0' and Other_Account_No<>"+strEmpId+"", "", DataViewRowState.CurrentRows).ToTable();

                foreach (DataRow dr in dtTemp.Rows)
                {
                    CreditAmount += Convert.ToDouble(dr["Credit_Amount"]) - Convert.ToDouble(dr["Debit_Amount"]);
                }

                if (CreditAmount >=0)
                {
                    ObjDa.execute_Command("update  ac_voucher_detail   set Debit_Amount=" + CreditAmount.ToString() + ",Credit_Amount='0',Companycurrdebit=" + CreditAmount.ToString() + ",CompanycurrCredit='0',Narration='" + strNarration.Replace(strEmpCode + ",", "").Replace("'", "") + "'  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No='0'");
                }
                else
                {
                    ObjDa.execute_Command("update  ac_voucher_detail   set Debit_Amount='0',Credit_Amount="+Math.Abs(CreditAmount).ToString() + ",Companycurrdebit='0',CompanycurrCredit=" + Math.Abs(CreditAmount).ToString() + ",Narration='" + strNarration.Replace(strEmpCode + ",", "").Replace("'", "") + "'  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No='0'");
                }
            }


        }


        dt.Dispose();
        dtVoucherdetail.Dispose();
        dtTemp.Dispose();
        return Result;
    }


    public bool IsLOanVoucherPost(string strVoucherheaderId, string strEmpId, string strEmpCode)
    {
        DataTable dtVoucherdetail = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dt = new DataTable();
        string strNarration = string.Empty;
        double DebitAmount = 0;
        double CreditAmount = 0;
        bool Result = true;

        dt = ObjDa.return_DataTable("select Trans_Id from Ac_Voucher_Header where Ac_Voucher_Header.TRans_Id=" + strVoucherheaderId + " and ReconciledFromFinance='True'");

        if (dt.Rows.Count > 0)
        {
            Result = false;
        }
        else
        {

            dtVoucherdetail = ObjDa.return_DataTable("select  * from ac_voucher_detail where Voucher_No=" + strVoucherheaderId + "");

            dtTemp = new DataView(dtVoucherdetail, "Other_Account_No<>" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTemp.Rows.Count == 0)
            {
                ObjDa.execute_Command("update  Ac_Voucher_Header   set IsActive='False'    where Trans_id=" + strVoucherheaderId + "");
            }
            else
            {
                ObjDa.execute_Command("Delete from  ac_voucher_detail  where Voucher_No=" + strVoucherheaderId + " and Other_Account_No=" + strEmpId + "");

            }

        }


        dt.Dispose();

        return Result;
    }
    protected void btnRollBackRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        FillGrid();
        Session["dtRollBack"] = null;
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";



    }
    protected void chkgvSelect_CheckedChangedRollBack(object sender, EventArgs e)
    {
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmpRollBack.Rows[index].FindControl("lblEmpId");
        foreach (GridViewRow gvRow in gvEmpRollBack.Rows)
        {
            Label lblEmpId = (Label)gvRow.FindControl("lblEmpId");
            CheckBox chk = (CheckBox)gvRow.FindControl("chkgvSelect");

            if (lblEmpId.Text != lb.Text)
            {
                chk.Checked = false;
            }

        }




        if (getRollBackMonthYear(lb.Text) == "")
        {
            DisplayMessage("Employee Has no Posted Payroll");
            ((CheckBox)gvEmpRollBack.Rows[index].FindControl("chkgvSelect")).Checked = false;
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";

            return;
        }

        if (((CheckBox)gvEmpRollBack.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            //add employee id
            lblSelectRecd.Text = lb.Text;

        }

        else
        {
            lblSelectRecd.Text = "";
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";

        }
        Session["dtRollBack"] = null;

    }
}
