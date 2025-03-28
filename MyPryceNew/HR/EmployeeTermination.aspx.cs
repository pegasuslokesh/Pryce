using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;
using System.Collections;
using System.Configuration;

public partial class HR_EmployeeTermination : System.Web.UI.Page
{
    Att_Employee_Leave objEmpleave = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_DocNumber objDocNo = null;
    IT_ObjectEntry objObjectEntry = null;
    HR_Leave_Salary objLeaveSalary = null;
    Attendance objAtt = null;
    CurrencyMaster objcurrency = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass ObjDa = null;
    LocationMaster objLocation = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Attendance objAttendance = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_Employee_Month objPayRoll = null;
    Att_AttendanceLog objAttLog = null;
    Pay_Employee_claim ObjClaim = null;
    Common cmn = null;
    Set_Approval_Employee objEmpApproval = null;
    LogProcess ObjLogprocess = null;
    Att_AttendanceRegister objattregister = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objLeaveSalary = new HR_Leave_Salary(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        objcurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objPayRoll = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjLogprocess = new LogProcess(Session["DBConnection"].ToString());
        objattregister = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/EmployeeTermination.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            Session["dtFilter_Emp_Terminate"] = null;
            txtTermDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
            txtTermDate_CalendarExtender.Format = objSys.SetDateFormat();
            FillGrid();
            // Hide Panels According to Application Id..........................
            //DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
            //if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "1")
            //{
            //    btnLogProcess.Visible = false;
            //}
        }
        //AllPageCode();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnTerminated.Visible = clsPagePermission.bEdit;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
    }

    protected void btnLogProcess_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text != "")
        {
            //string url = "../Attendance/LogProcess.aspx";
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1150,scrollbars=Yes')", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance/LogProcess.aspx?Request_ID=" + txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] + "');", true);
            Session["TerminationDate"] = txtTermDate.Text;
            Session["TerminateEmpId"] = hdnEmpId.Value;
        }
        else
        {
            DisplayMessage("Please Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
    }

    protected void btnGeneratePayroll_Click(object sender, EventArgs e)
    {
        //string url = "../HR/GenerateEmpPayroll.aspx";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1150,scrollbars=Yes')", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR/GenerateEmpPayroll.aspx');", true);
        Session["TerminationDate"] = txtTermDate.Text;
        Session["TerminateEmpId"] = hdnEmpId.Value;
    }

    protected void btnLeaveSal_Click(object sender, EventArgs e)
    {
        string url = "../HR/LeaveSalary.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1150,scrollbars=Yes')", true);

    }
    protected void btnApply1_Click(object sender, EventArgs e)
    {
        bool LogPostOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryLogPostOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        bool PayrollPostOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryPayrollPostOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        bool FinanceOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryCheckFinanceOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));

        if (hdnEmpId.Value != "")
        {
            bool IsAllow = false;

            if (txtEmpName.Text.Trim() == "")
            {
                DisplayMessage("Select Employee Name in Sugestion");
                txtEmpName.Focus();
                pnlTermination.Visible = false;
                return;
            }

            if (txtTermDate.Text == "")
            {
                DisplayMessage("Enter Termination Date");
                txtTermDate.Focus();
                pnlTermination.Visible = false;
                return;
            }
            else
            {
                try
                {
                    Convert.ToDateTime(txtTermDate.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date Format");
                    txtTermDate.Focus();
                    pnlTermination.Visible = false;
                    return;
                }
            }

            DateTime dtDoj = Convert.ToDateTime(objAtt.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value).Rows[0][0].ToString());

            //here checkig that termination date should not be less then date of joining

            //validation added by jitendra upadhyay on 25-05-2018



            if (Convert.ToDateTime(txtTermDate.Text).Date < dtDoj.Date)
            {
                DisplayMessage("Unable To Terminate because termination date is less then date of joining");
                return;
            }

            //DataTable dtLogs = objEmpleave.Get_Log_By_Date_EmpID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString(), Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString(), "1", "1");
            //if (dtLogs != null && dtLogs.Rows.Count > 0)
            //{
            //    //DisplayMessage("Unable To Employee Terminate because log is found for " + Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString("dd-MMM-yyyy") + "");
            //    //dtLogs.Dispose();
            //    //return;
            //}


            //DataTable Dt_Unpost = objVoucherHeader.Get_Unposted_Voucher(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, "181,143", "1");
            //if (Dt_Unpost != null && Dt_Unpost.Rows.Count > 0)
            //{
            //    DisplayMessage("Unable To Employee Terminate, Please Post Pending Voucher");
            //    Dt_Unpost.Dispose();
            //    return;
            //}

            DataTable dtResources = ObjDa.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id=" + hdnEmpId.Value + " and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' and Set_EmployeeResources.Is_Returnable='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type<>'In' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");
            if (dtResources.Rows.Count > 0)
            {
                DisplayMessage("First, Submit all pending resources for terminating employee");
                dtResources.Dispose();
                return;
            }

            //New Code Started on 09-April-2024 By Lokesh
            DataTable dtApproval = objEmpApproval.GetApprovalChildTermination1(Session["CompId"].ToString(), "0", hdnEmpId.Value);
            if (dtApproval != null && dtApproval.Rows.Count > 0)
            {
                DisplayMessage("Some Pending request in Approval section which will be approve or reject by terminating employee");
                return;
            }
            DataTable dtApproval1 = objEmpApproval.GetApprovalChildTermination2(Session["CompId"].ToString(), "0", hdnEmpId.Value);
            if (dtApproval1 != null && dtApproval1.Rows.Count > 0)
            {
                DisplayMessage("Some request is pending in approval section");
                return;
            }
            //End Code

            //Commented Code on 09-April-2024 By Lokesh
            //DataTable dtApproval = objEmpApproval.GetApprovalChild(Session["CompId"].ToString(), "0");
            //if (dtApproval != null && dtApproval.Rows.Count > 0)
            //{
            //    if (new DataView(dtApproval, "Emp_Id=" + hdnEmpId.Value + " and Status='Pending' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            //    {
            //        DisplayMessage("Some Pending request in Approval section which will be approve or reject by terminating employee");
            //        return;                   
            //    }
            //    //added new validation

            //    if (new DataView(dtApproval, "Request_Emp_Id=" + hdnEmpId.Value + " and Status='Pending' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            //    {
            //        DisplayMessage("Some request is pending in approval section");
            //        return;
            //    }
            //}


            //here we are checking that account configured or not 
            string strCreditAccount = string.Empty;
            string strDebitAccount = string.Empty;
            string strLoanDebitAccount = string.Empty;
            DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
            DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCredit.Rows.Count > 0)
            {
                strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();

            }

            DataTable dtDebit = new DataView(dtAcParameter, "Param_Name='HR Section'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {
                strDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();

            }

            dtDebit = new DataView(dtAcParameter, "Param_Name='Employee Loan Account'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDebit.Rows.Count > 0)
            {

                strLoanDebitAccount = dtDebit.Rows[0]["Param_Value"].ToString();
            }


            if (FinanceOnTermination == true)
            {
                if (strCreditAccount == "" || strDebitAccount == "" || strLoanDebitAccount == "")
                {
                    DisplayMessage("Finance account not configured");
                    return;
                }
            }


            DateTime dtLogprocessfrom = new DateTime();
            DateTime tTermination = Convert.ToDateTime(txtTermDate.Text);
            DataTable dtTermination = new DataTable();
            bool isCheckPayrollattpost = (dtDoj.Month == tTermination.Month && dtDoj.Year == tTermination.Year) ? false : true;
            dtLogprocessfrom = (dtDoj.Month == tTermination.Month && dtDoj.Year == tTermination.Year) ? dtDoj : new DateTime(tTermination.Year, tTermination.Month, 1);
            DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
            if (DtApp_Id.Rows[0]["Param_Value"].ToString() != "1")
            {

                if (isCheckPayrollattpost)
                {
                    dtTermination = objAttLog.Get_Pay_Employee_Attendance(hdnEmpId.Value, tTermination.AddMonths(-1).Month.ToString(), tTermination.AddMonths(-1).Year.ToString());
                    if (dtTermination.Rows.Count == 0)
                    {
                        if (LogPostOnTermination == true)
                        {
                            DisplayMessage("Please do log post for previous month before employee termination");
                            return;
                        }
                    }
                }
            }

            if (isCheckPayrollattpost)
            {
                dtTermination = objPayRoll.GetAllRecordPostedEmpMonth(hdnEmpId.Value, tTermination.AddMonths(-1).Month.ToString(), tTermination.AddMonths(-1).Year.ToString(), Session["CompId"].ToString());
                if (dtTermination.Rows.Count == 0)
                {
                    if (PayrollPostOnTermination == true)
                    {
                        DisplayMessage("Please post payroll for previous month before employee termination");
                        return;
                    }
                }
            }


            //validation for leave salary


            //for leave salary
            bool IsLeaveSalary = false;
            string strpaymentDebitaccount = string.Empty;
            string strpaymentCreditaccount = string.Empty;
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())) == true)
            {
                dtDebit = new DataView(dtAcParameter, "Param_Name='Leave Salary Account'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtDebit.Rows.Count > 0)
                {
                    strpaymentDebitaccount = dtDebit.Rows[0]["Param_Value"].ToString();
                }
                dtDebit = new DataView(dtAcParameter, "Param_Name='Employee Account'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtDebit.Rows.Count > 0)
                {
                    strpaymentCreditaccount = dtDebit.Rows[0]["Param_Value"].ToString();
                }

                if (strpaymentDebitaccount == "" || strpaymentCreditaccount == "")
                {
                    if (FinanceOnTermination == true)
                    {
                        DisplayMessage("Account not configured for leave salary or employee account");
                        return;
                    }
                }
                IsLeaveSalary = true;
            }



            string[] strLogprocessresult = new string[2];

            //log process code

            //start

            //before star this process we need to delete previous processed record behalf of employeee , month and year
            //start
            // objattregister.DeleteAttendanceRegister()

            ObjDa.execute_Command("Delete from att_attendanceregister where emp_id=" + hdnEmpId.Value + " and   month(att_date)=" + dtLogprocessfrom.Month.ToString() + "  and year(att_date)=" + dtLogprocessfrom.Year.ToString() + "");
            //end

            if (LogPostOnTermination == true)
            {
                strLogprocessresult = ObjLogprocess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, Session["UserId"].ToString(), "0", dtLogprocessfrom, tTermination, Session["EmpId"].ToString(), null, HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(), ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString());
            }

            if (!Convert.ToBoolean(strLogprocessresult[0]))
            {
                DisplayMessage("Log Process Error : " + strLogprocessresult[1]);
                return;
            }

            //end

            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();

            try
            {

                //auto log post
                ObjLogprocess.autoLogPost(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, tTermination.Month.ToString(), tTermination.Year.ToString(), Session["UserId"].ToString(), ref trns, Session["TimeZoneId"].ToString());


                dtTermination = objPayRoll.GetAllRecordPostedEmpMonth(hdnEmpId.Value, tTermination.Month.ToString(), tTermination.Year.ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString());
                if (dtTermination.Rows.Count == 0)
                {
                    if (PayrollPostOnTermination == true)
                    {
                        //generate payroll
                        ObjLogprocess.GeneratePayroll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), tTermination.Month.ToString(), tTermination.Year.ToString(), "", Session["userId"].ToString(), hdnEmpId.Value, true, ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                        //post payroll
                        ObjLogprocess.PostPayroll(hdnEmpId.Value + ",", Session["LocId"].ToString(), true, ref trns, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["TimeZoneId"].ToString(), Session["UserId"].ToString(), Session["FinanceYearId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString());
                    }
                }

                if (IsLeaveSalary)
                {
                    try
                    {
                        EmployeeLeaveSalary(dtDoj, tTermination, strpaymentCreditaccount, strpaymentDebitaccount, ref trns);
                    }
                    catch (Exception ex)
                    {

                    }

                }



                string EmpId = string.Empty;
                int savestatus = 0;

                savestatus = objEmp.UpdateTerminationStatus(hdnEmpId.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (savestatus != 0)
                {
                    if (txtHandledby2.Text != "")
                    {
                        ObjDa.execute_Command("Update Set_Approval_Employee Set Emp_Id='" + hdnHandledby.Value + "' Where Emp_Id='" + hdnEmpId.Value + "'", ref trns);
                        ObjDa.execute_Command("Update Set_EmployeeMaster Set Field5='" + hdnHandledby.Value + "' where Emp_Type='On Role' and Field2='False' and IsActive='True' and Field5='" + hdnEmpId.Value + "'", ref trns);
                    }
                    else
                    {
                        ObjDa.execute_Command("Delete From Set_Approval_Employee Where Emp_Id='" + hdnEmpId.Value + "'", ref trns);
                        ObjDa.execute_Command("Update Set_EmployeeMaster Set Field5='' where Emp_Type='On Role' and Field2='False' and IsActive='True' and Field5='" + hdnEmpId.Value + "'", ref trns);
                    }

                    EmpId = hdnEmpId.Value;
                    string empid = txtHandledby2.Text;
                    int pos = empid.LastIndexOf("/") + 1;
                    string id = empid.Substring(pos, empid.Length - pos);
                    objEmp.Insert_Pay_Termination(Session["CompId"].ToString(), hdnEmpId.Value, objSys.getDateForInput(txtTermDate.Text).ToString(), ddlType.SelectedValue, txtReason.Text, "0", "0", "0", "0", "0", "0", "0", "0", "0", "True", hdnHandledby.Value, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    if (FinanceOnTermination == true)
                    {
                        DisplayMessage("Employee Terminated , you can check finance voucher in transfer in finance");
                    }
                    else
                    {
                        DisplayMessage("Employee Terminated Succesfully");
                    }


                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {

                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();

                    FillGrid();
                    Session["ET_Hdnempid"] = null;
                    if (txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] == Session["UserId"].ToString())
                    {
                        Session.Abandon();
                        Response.Redirect("~/ERPLogin.aspx");
                    }
                    btnReset_Click(null, null);
                    //this code is created by jitendra upadhyay on 24-03-2015
                    //this code for delete all referrence of this employee after termination
                    //code start
                    try
                    {
                        Common.DisableEmployee(Session["CompId"].ToString(), EmpId, Session["DBConnection"].ToString());
                    }
                    catch
                    {

                    }
                    //code end
                }
                else
                {
                    DisplayMessage("Employee Not Terminated Successfully");
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
            //AllPageCode();
        }
    }

    protected void btnApply_Click(object sender, EventArgs e)
    {
        bool LogPostOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryLogPostOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        bool PayrollPostOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryPayrollPostOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        bool FinanceOnTermination = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryCheckFinanceOnTermination", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));






        if (hdnEmpId.Value != "")
        {
            bool IsAllow = false;
            if (txtEmpName.Text.Trim() == "")
            {
                DisplayMessage("Select Employee Name in Sugestion");
                txtEmpName.Focus();
                pnlTermination.Visible = false;
                return;
            }
            if (txtTermDate.Text == "")
            {
                DisplayMessage("Enter Termination Date");
                txtTermDate.Focus();
                pnlTermination.Visible = false;
                return;
            }
            else
            {
                try
                {
                    Convert.ToDateTime(txtTermDate.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date Format");
                    txtTermDate.Focus();
                    pnlTermination.Visible = false;
                    return;
                }
            }

            DateTime dtDoj = Convert.ToDateTime(objAtt.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value).Rows[0][0].ToString());

            //here checkig


            DataTable dtLogs = objEmpleave.Get_Log_By_Date_EmpID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString(), Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString(), "1", "1");
            if (dtLogs != null && dtLogs.Rows.Count > 0)
            {
                DisplayMessage("Unable To Employee Terminate because log is found for " + Convert.ToDateTime(txtTermDate.Text).AddDays(1).ToString("dd-MMM-yyyy") + "");
                dtLogs.Dispose();
                return;
            }

            if (FinanceOnTermination == true)
            {
                DataTable Dt_Unpost = objVoucherHeader.Get_Unposted_Voucher(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, "181,143", "1");
                if (Dt_Unpost != null && Dt_Unpost.Rows.Count > 0)
                {
                    DisplayMessage("Unable To Employee Terminate, Please Post Pending Voucher");
                    Dt_Unpost.Dispose();
                    return;
                }
            }


            DataTable dtResources = ObjDa.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id=" + hdnEmpId.Value + " and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' and Set_EmployeeResources.Is_Returnable='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type<>'In' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");
            if (dtResources.Rows.Count > 0)
            {
                DisplayMessage("First, Submit all pending resources for terminating employee");
                dtResources.Dispose();
                return;
            }
            DataTable dtApproval = objEmpApproval.GetApprovalChild(Session["CompId"].ToString(), "0");
            if (dtApproval != null && dtApproval.Rows.Count > 0)
            {
                dtApproval = new DataView(dtApproval, "Emp_Id=" + hdnEmpId.Value + " and Status='Pending' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtApproval.Rows.Count > 0)
                {
                    DisplayMessage("Some Pending request in Approval section which will be approve or reject by terminating employee");
                    return;
                }
            }




            DateTime tTermination = Convert.ToDateTime(txtTermDate.Text);
            DataTable dtTermination = new DataTable();
            DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
            if (DtApp_Id.Rows[0]["Param_Value"].ToString() != "1")
            {
                dtTermination = objAttLog.Get_Pay_Employee_Attendance(hdnEmpId.Value, tTermination.Month.ToString(), tTermination.Year.ToString());
                if (dtTermination.Rows.Count == 0)
                {
                    if (LogPostOnTermination == true)
                    {
                        DisplayMessage("Please do log post before employee termination");
                        return;
                    }
                }
            }
            dtTermination = objPayRoll.GetAllRecordPostedEmpMonth(hdnEmpId.Value, tTermination.Month.ToString(), tTermination.Year.ToString(), Session["CompId"].ToString());
            if (dtTermination.Rows.Count == 0)
            {
                if (PayrollPostOnTermination == true)
                {
                    DisplayMessage("Please post payroll before employee termination");
                    return;
                }
            }
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())) == true)
            {
                if (Convert.ToBoolean(Check_Leave_Salary()) == false)
                {
                    DisplayMessage("Please Generate Leave salary before employee termination");
                    return;
                }
            }

            lblComp1.Text = Common.Get_Location_Currency_LControl(Session["CompId"].ToString(), Session["DBConnection"].ToString(), Session["LocId"].ToString());
            string FinalIndemnitySal = string.Empty;
            //IndemnitySalary
            if (ddlType.SelectedValue == "1")
            {
                string strSQL = "Select SUM(Indemnity_Salary) AS IndemnitySalary ,SUM(Indemnity_Days) AS IndemnityDays from Att_IndemnitySalary where Emp_Id = '" + hdnEmpId.Value + "'";
                DataTable Dt = ObjDa.return_DataTable(strSQL);
                if (Dt.Rows.Count > 0)
                {
                    txtIndemnitySalary.Text = Dt.Rows[0]["IndemnitySalary"].ToString();
                }
                else
                {
                    txtIndemnitySalary.Text = "0";
                }
            }
            else
            {
                txtIndemnitySalary.Text = "0";
            }

            // Leave Salary 
            string strSQL1 = "select SUM(Total) AS LeaveSalaryT from Att_LeaveSalary where Emp_Id = '" + hdnEmpId.Value + "' and Is_Report =0";
            DataTable DtLT = ObjDa.return_DataTable(strSQL1);
            double LST;
            try
            {
                LST = Convert.ToDouble(DtLT.Rows[0][0].ToString());
            }
            catch
            {
                LST = 0;
            }
            txtLeaveSalary.Text = LST.ToString();
            try
            {
                txtCurrentmonthsalary.Text = (float.Parse(dtTermination.Rows[0]["Basic_Work_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Normal_OT_Work_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Week_Off_Days_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Holiday_Days_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Leave_Days_Salary"].ToString()) - float.Parse(dtTermination.Rows[0]["Absent_Day_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Late_Min_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Early_Min_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Parital_Violation_Penalty"].ToString())).ToString();
            }
            catch
            {

            }
            string Amount = txtCurrentmonthsalary.Text;
            txtCurrentmonthsalary.Text = objSys.GetCurencyConversion(hdnEmpId.Value, txtCurrentmonthsalary.Text, Session["LocCurrencyId"].ToString());
            txtMonthadjustment.Text = "0";
            txtLoanAmount.Text = "0";
            txtTotalAmount.Text = txtCurrentmonthsalary.Text;
            txtPaidAmount.Text = txtCurrentmonthsalary.Text;
            txtTotalAmountEmployeeCurrency.Text = objSys.GetCurencyConversion_For_EmployeeCurrency(hdnEmpId.Value, Amount, Session["CompId"].ToString(), Session["LocCurrencyId"].ToString());
            txtPaidAmountEmployeeCurrency.Text = objSys.GetCurencyConversion_For_EmployeeCurrency(hdnEmpId.Value, Amount, Session["CompId"].ToString(), Session["LocCurrencyId"].ToString());
            //here we check that employee in payroll or not
            DataTable Dt_Emp_In_Payroll = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());
            if (Dt_Emp_In_Payroll != null && Dt_Emp_In_Payroll.Rows.Count > 0)
            {
                Dt_Emp_In_Payroll = new DataView(Dt_Emp_In_Payroll, "Emp_Id=" + hdnEmpId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Emp_In_Payroll.Rows.Count > 0)
                {
                    int NextMonth = 0;
                    int NextYear = 0;
                    if (tTermination.Month == 12)
                    {
                        NextMonth = 1;
                        NextYear = tTermination.Year + 1;
                    }
                    else
                    {
                        NextMonth = tTermination.Month + 1;
                        NextYear = tTermination.Year;
                    }
                    IsAllow = true;
                    pnlTermination.Visible = true;
                    ddlType.Focus();
                    try
                    {
                        txtCurrentmonthsalary.Text = (float.Parse(dtTermination.Rows[0]["Worked_Min_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Leave_Days_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Week_Off_Salary"].ToString()) + float.Parse(dtTermination.Rows[0]["Holidays_Salary"].ToString()) - float.Parse(dtTermination.Rows[0]["Absent_Salary"].ToString()) - float.Parse(dtTermination.Rows[0]["Late_Min_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Early_Min_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtTermination.Rows[0]["Employee_Penalty"].ToString()) + float.Parse(dtTermination.Rows[0]["Employee_Claim"].ToString()) - float.Parse(dtTermination.Rows[0]["Emlployee_Loan"].ToString()) + float.Parse(dtTermination.Rows[0]["Total_Allowance"].ToString()) - float.Parse(dtTermination.Rows[0]["Total_Deduction"].ToString()) - double.Parse(dtTermination.Rows[0]["Employee_PF"].ToString()) - double.Parse(dtTermination.Rows[0]["Employee_ESIC"].ToString())).ToString();
                        txtCurrentmonthsalary.Text = objSys.GetCurencyConversion(hdnEmpId.Value, txtCurrentmonthsalary.Text, Session["LocCurrencyId"].ToString());
                    }
                    catch
                    {
                        txtCurrentmonthsalary.Text = "0";
                    }
                    pnlTermination.Visible = true;
                    ddlType.Focus();
                    double TotalNextMonthadjustment = 0;
                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(hdnEmpId.Value, Session["CompId"].ToString());
                    if (dtDueamount.Rows.Count > 0)
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + tTermination.Month.ToString() + " and Year=" + tTermination.Year.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtDueamount != null && dtDueamount.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtDueamount.Rows.Count; i++)
                            {
                                try
                                {
                                    if (dtDueamount.Rows[i]["Field1"].ToString().Trim() == "Allowance")
                                    {
                                        TotalNextMonthadjustment += Convert.ToDouble(dtDueamount.Rows[i]["Amount"].ToString());
                                    }
                                    if (dtDueamount.Rows[i]["Field1"].ToString().Trim() == "Deduction")
                                    {
                                        TotalNextMonthadjustment -= Convert.ToDouble(dtDueamount.Rows[i]["Amount"].ToString());
                                    }
                                    if (dtDueamount.Rows[i]["Field1"].ToString().Trim() == "Claim")
                                    {
                                        TotalNextMonthadjustment += Convert.ToDouble(dtDueamount.Rows[i]["Amount"].ToString());
                                    }
                                    if (dtDueamount.Rows[i]["Field1"].ToString().Trim() == "Penalty")
                                    {
                                        TotalNextMonthadjustment -= Convert.ToDouble(dtDueamount.Rows[i]["Amount"].ToString());
                                    }
                                }
                                catch
                                {
                                    TotalNextMonthadjustment += 0;
                                }
                            }
                        }
                    }
                    txtMonthadjustment.Text = objSys.GetCurencyConversion(hdnEmpId.Value, TotalNextMonthadjustment.ToString(), Session["LocCurrencyId"].ToString());
                    double TotalLOanAmount = 0;
                    DataTable Dtloan = new DataTable();
                    Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                    if (Dtloan.Rows.Count > 0)
                    {
                        Dtloan = new DataView(Dtloan, " Emp_Id=" + hdnEmpId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                        for (int i = 0; i < Dtloan.Rows.Count; i++)
                        {
                            DataTable dtloandetial = new DataTable();
                            dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString());
                            for (int j = 0; j < dtloandetial.Rows.Count; j++)
                            {
                                if (dtloandetial.Rows[j]["Is_Status"].ToString().Trim() == "Pending")
                                {
                                    try
                                    {
                                        TotalLOanAmount += Convert.ToDouble(dtloandetial.Rows[j]["Total_Amount"].ToString());
                                    }
                                    catch
                                    {
                                        TotalLOanAmount += 0;
                                    }
                                }
                            }
                        }
                    }
                    txtLoanAmount.Text = objSys.GetCurencyConversion(hdnEmpId.Value, TotalLOanAmount.ToString(), Session["LocCurrencyId"].ToString());
                    if (txtIndemnitySalary.Text == "")
                    {
                        txtIndemnitySalary.Text = "0";
                    }
                    if (txtLeaveSalary.Text == "")
                    {
                        txtLeaveSalary.Text = "0";
                    }
                    txtTotalAmount.Text = objSys.GetCurencyConversion(hdnEmpId.Value, (Convert.ToDouble(txtIndemnitySalary.Text) + Convert.ToDouble(txtLeaveSalary.Text) + Convert.ToDouble(txtCurrentmonthsalary.Text) + Convert.ToDouble(txtMonthadjustment.Text) - Convert.ToDouble(txtLoanAmount.Text)).ToString(), Session["LocCurrencyId"].ToString());
                    txtTotalAmountEmployeeCurrency.Text = objSys.GetCurencyConversion_For_EmployeeCurrency(hdnEmpId.Value, (Convert.ToDouble(txtIndemnitySalary.Text) + Convert.ToDouble(txtLeaveSalary.Text) + Convert.ToDouble(txtCurrentmonthsalary.Text) + Convert.ToDouble(txtMonthadjustment.Text) - Convert.ToDouble(txtLoanAmount.Text)).ToString(), Session["CompId"].ToString(), Session["LocCurrencyId"].ToString());
                    txtPaidAmount.Text = objSys.GetCurencyConversion(hdnEmpId.Value, (Convert.ToDouble(txtIndemnitySalary.Text) + Convert.ToDouble(txtLeaveSalary.Text) + Convert.ToDouble(txtCurrentmonthsalary.Text) + Convert.ToDouble(txtMonthadjustment.Text) - Convert.ToDouble(txtLoanAmount.Text)).ToString(), Session["LocCurrencyId"].ToString());
                    txtPaidAmountEmployeeCurrency.Text = objSys.GetCurencyConversion_For_EmployeeCurrency(hdnEmpId.Value, (Convert.ToDouble(txtIndemnitySalary.Text) + Convert.ToDouble(txtLeaveSalary.Text) + Convert.ToDouble(txtCurrentmonthsalary.Text) + Convert.ToDouble(txtMonthadjustment.Text) - Convert.ToDouble(txtLoanAmount.Text)).ToString(), Session["CompId"].ToString(), Session["LocCurrencyId"].ToString());
                }
                else
                {
                    IsAllow = true;
                    pnlTermination.Visible = true;
                    ddlType.Focus();
                }
            }
            //AllPageCode();
        }
    }

    protected void btnTerminated_Click(object sender, EventArgs e)
    {
        string EmpId = string.Empty;
        if (txtReason.Text.Trim() == "")
        {
            DisplayMessage("Enter Reason");
            txtReason.Focus();
            return;
        }

        if (txtPaidAmount.Text.Trim() == "")
        {
            DisplayMessage("Enter Paid Amount");
            txtPaidAmount.Focus();
            return;
        }
        decimal Month_Salary = 0;
        decimal Loan_Amount = 0;
        decimal Leave_Salary = 0;
        decimal Indemnity_Salary = 0;
        decimal Total_Amount = 0;
        decimal Paid_Amount = 0;

        if (txtCurrentmonthsalary.Text.Trim() == "")
            Month_Salary = 0;
        else
            Month_Salary = Convert.ToDecimal(txtCurrentmonthsalary.Text.Trim());

        if (txtLoanAmount.Text.Trim() == "")
            Loan_Amount = 0;
        else
            Loan_Amount = Convert.ToDecimal(txtLoanAmount.Text.Trim());

        if (txtLeaveSalary.Text.Trim() == "")
            Leave_Salary = 0;
        else
            Leave_Salary = Convert.ToDecimal(txtLeaveSalary.Text.Trim());

        if (txtIndemnitySalary.Text.Trim() == "")
            Indemnity_Salary = 0;
        else
            Indemnity_Salary = Convert.ToDecimal(txtIndemnitySalary.Text.Trim());

        if (txtTotalAmount.Text.Trim() == "")
            Total_Amount = 0;
        else
            Total_Amount = Convert.ToDecimal(txtTotalAmount.Text.Trim());

        if (txtPaidAmount.Text.Trim() == "")
            Paid_Amount = 0;
        else
            Paid_Amount = Convert.ToDecimal(txtPaidAmount.Text.Trim());

        if (txtLoanAmount.Text != "")
        {
            if (Convert.ToDouble(txtLoanAmount.Text) != 0)
            {
                DisplayMessage("Unable To proceed, Please Adjust Loan Amount");
                return;
            }
        }

        //if (Paid_Amount > Total_Amount)
        //{
        //    DisplayMessage("Paid amount is more than total amount");
        //    return;
        //}
        if (Total_Amount < 0)
        {
            DisplayMessage("Unable To proceed, Because Total Amount Less then Zero");
            return;
        }
        else
        {
            if (hdnTerminationId.Value == "0")
            {
                //here we update the termination date in employee master table
                int savestatus = 0;

                savestatus = objEmp.UpdateTerminationStatus(hdnEmpId.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                if (savestatus != 0)
                {
                    EmpId = hdnEmpId.Value;
                    string empid = txtHandledby2.Text;
                    int pos = empid.LastIndexOf("/") + 1;
                    string id = empid.Substring(pos, empid.Length - pos);

                    objEmp.Insert_Pay_Termination(Session["CompId"].ToString(), hdnEmpId.Value, objSys.getDateForInput(txtTermDate.Text).ToString(), ddlType.SelectedValue, txtReason.Text, txtCurrentmonthsalary.Text, txtMonthadjustment.Text, txtLoanAmount.Text, txtTotalAmount.Text, txtPaidAmount.Text, txtTotalAmountEmployeeCurrency.Text, txtPaidAmountEmployeeCurrency.Text, txtIndemnitySalary.Text, txtLeaveSalary.Text, "False", id, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    if (ddlType.SelectedIndex == 0)
                    {
                        DisplayMessage("Employee Terminated Successfully");
                    }
                    else
                    {
                        DisplayMessage("Employee Resigned Successfully");
                    }

                    FillGrid();
                    Session["ET_Hdnempid"] = null;
                    if (txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] == Session["UserId"].ToString())
                    {
                        Session.Abandon();
                        Response.Redirect("~/ERPLogin.aspx");
                    }
                    btnReset_Click(null, null);
                    //this code is created by jitendra upadhyay on 24-03-2015
                    //this code for delete all referrence of this employee after termination
                    //code start
                    try
                    {
                        Common.DisableEmployee(Session["CompId"].ToString(), EmpId, Session["DBConnection"].ToString());
                    }
                    catch
                    {

                    }
                    //code end
                }
                else
                {
                    if (ddlType.SelectedIndex == 0)
                    {
                        DisplayMessage("Employee Not Terminated Successfully");
                    }
                    else
                    {
                        DisplayMessage("Employee Not Resigned Successfully");
                    }

                }
            }
            else
            {
                string Hand_By_ID = string.Empty;
                if (txtHandledby2.Text != "")
                {
                    Hand_By_ID = txtHandledby2.Text.Split('/')[1].ToString();
                }
                else
                {
                    Hand_By_ID = "0";
                }
                ObjDa.execute_Command("Update Set_Approval_Employee Set Emp_Id='" + Hand_By_ID + "' Where Emp_Id='" + hdnEmpId.Value + "'");
                ObjDa.execute_Command("Update Set_EmployeeMaster Set Field5='" + Hand_By_ID + "' where Emp_Type='On Role' and Field2='False' and IsActive='True' and Field5='" + hdnEmpId.Value + "'");
                objEmp.Update_Pay_Termination(Session["CompId"].ToString(), hdnEmpId.Value, hdnTerminationId.Value, txtPaidAmount.Text, txtPaidAmountEmployeeCurrency.Text, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
                if (ddlType.SelectedIndex == 0)
                {
                    DisplayMessage("Employee Terminated Successfully");
                }
                else
                {
                    DisplayMessage("Employee Resigned Successfully");
                }

                FillGrid();
                Session["ET_Hdnempid"] = null;
                if (txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] == Session["UserId"].ToString())
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                btnReset_Click(null, null);
            }
            //AllPageCode();
        }
    }
    protected void btnTerminatePost_Click(object sender, EventArgs e)
    {
        if (txtPaidAmount.Text.Trim() == "")
        {
            DisplayMessage("Enter Paid Amount");
            txtPaidAmount.Focus();
            return;
        }

        //DataTable Dt_Pending_Approval = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
        //if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
        //{
        //    Dt_Pending_Approval = new DataView(Dt_Pending_Approval, "Emp_Id='" + hdnEmpId.Value + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
        //    if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
        //    {
        //        DisplayMessage("Approval Request is in under processing, Please Approve or reject all pending request.");
        //        return;
        //    }
        //}

        decimal Month_Salary = 0;
        decimal Loan_Amount = 0;
        decimal Leave_Salary = 0;
        decimal Indemnity_Salary = 0;
        decimal Total_Amount = 0;
        decimal Paid_Amount = 0;

        if (txtCurrentmonthsalary.Text.Trim() == "")
            Month_Salary = 0;
        else
            Month_Salary = Convert.ToDecimal(txtCurrentmonthsalary.Text.Trim());

        if (txtLoanAmount.Text.Trim() == "")
            Loan_Amount = 0;
        else
            Loan_Amount = Convert.ToDecimal(txtLoanAmount.Text.Trim());

        if (txtLeaveSalary.Text.Trim() == "")
            Leave_Salary = 0;
        else
            Leave_Salary = Convert.ToDecimal(txtLeaveSalary.Text.Trim());

        if (txtIndemnitySalary.Text.Trim() == "")
            Indemnity_Salary = 0;
        else
            Indemnity_Salary = Convert.ToDecimal(txtIndemnitySalary.Text.Trim());

        if (txtTotalAmount.Text.Trim() == "")
            Total_Amount = 0;
        else
            Total_Amount = Convert.ToDecimal(txtTotalAmount.Text.Trim());

        if (txtPaidAmount.Text.Trim() == "")
            Paid_Amount = 0;
        else
            Paid_Amount = Convert.ToDecimal(txtPaidAmount.Text.Trim());

        if (txtLoanAmount.Text != "")
        {
            if (Convert.ToDouble(txtLoanAmount.Text) != 0)
            {
                DisplayMessage("Unable To Employee Terminate, Please Adjust Loan Amount");
                return;
            }
        }

        //if (Paid_Amount > Total_Amount)
        //{
        //    DisplayMessage("Paid amount is more than total amount");
        //    return;
        //}
        if (Total_Amount < 0)
        {
            DisplayMessage("Unable To Employee Terminate, Because Total Amount Less then Zero");
            return;
        }
        else
        {

            if (hdnTerminationId.Value != "0")
            {
                objEmp.Update_Pay_Termination(Session["CompId"].ToString(), hdnEmpId.Value, hdnTerminationId.Value, txtPaidAmount.Text, txtPaidAmountEmployeeCurrency.Text, "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                DisplayMessage("Employee Terminated & Posted Successfully");

                FillGrid();
                Session["ET_Hdnempid"] = null;
                if (txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] == Session["UserId"].ToString())
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                btnReset_Click(null, null);
            }
            else
            {
                string EmpId = string.Empty;
                int savestatus = 0;

                savestatus = objEmp.UpdateTerminationStatus(hdnEmpId.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                if (savestatus != 0)
                {
                    if (txtHandledby2.Text != "")
                    {
                        ObjDa.execute_Command("Update Set_Approval_Employee Set Emp_Id='" + txtHandledby2.Text.Split('/')[1].ToString() + "' Where Emp_Id='" + hdnEmpId.Value + "'");
                        ObjDa.execute_Command("Update Set_EmployeeMaster Set Field5='" + txtHandledby2.Text.Split('/')[1].ToString() + "' where Emp_Type='On Role' and Field2='False' and IsActive='True' and Field5='" + hdnEmpId.Value + "'");
                    }
                    else
                    {
                        ObjDa.execute_Command("Delete From Set_Approval_Employee Where Emp_Id='" + hdnEmpId.Value + "'");
                        ObjDa.execute_Command("Update Set_EmployeeMaster Set Field5='' where Emp_Type='On Role' and Field2='False' and IsActive='True' and Field5='" + hdnEmpId.Value + "'");
                    }

                    EmpId = hdnEmpId.Value;
                    string empid = txtHandledby2.Text;
                    int pos = empid.LastIndexOf("/") + 1;
                    string id = empid.Substring(pos, empid.Length - pos);
                    objEmp.Insert_Pay_Termination(Session["CompId"].ToString(), hdnEmpId.Value, objSys.getDateForInput(txtTermDate.Text).ToString(), ddlType.SelectedValue, txtReason.Text, txtCurrentmonthsalary.Text, txtMonthadjustment.Text, txtLoanAmount.Text, txtTotalAmount.Text, txtPaidAmount.Text, txtTotalAmountEmployeeCurrency.Text, txtPaidAmountEmployeeCurrency.Text, txtIndemnitySalary.Text, txtLeaveSalary.Text, "True", id, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    DisplayMessage("Employee Terminated Successfully");

                    FillGrid();
                    Session["ET_Hdnempid"] = null;
                    if (txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1] == Session["UserId"].ToString())
                    {
                        Session.Abandon();
                        Response.Redirect("~/ERPLogin.aspx");
                    }
                    btnReset_Click(null, null);
                    //this code is created by jitendra upadhyay on 24-03-2015
                    //this code for delete all referrence of this employee after termination
                    //code start
                    try
                    {
                        Common.DisableEmployee(Session["CompId"].ToString(), EmpId, Session["DBConnection"].ToString());
                    }
                    catch
                    {

                    }
                    //code end
                }
                else
                {
                    DisplayMessage("Employee Not Terminated Successfully");
                }
            }
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtEmpName.Text = "";
        txtReason.Text = "";
        txtTermDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        hdnEmpId.Value = "";
        hdnHandledby.Value = "0";
        txtEmpName.ReadOnly = false;
        txtTermDate.ReadOnly = false;
        txtHandledby2.Text = "";
        //btnApply.Visible = true;
        ////btnLogProcess.Visible = true;
        ////btnGeneratePayroll.Visible = true;
        ddlType.Enabled = true;
        txtReason.ReadOnly = false;
        pnlTermination.Visible = false;
        hdnTerminationId.Value = "0";
        txtEmpName.Focus();
        //AllPageCode();
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        txtEmpName.Text = "";
        txtTermDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        hdnEmpId.Value = "";
        pnlTermination.Visible = false;
        txtEmpName.Focus();
        FillGrid();
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;


        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            try
            {
                dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmp.Rows.Count > 0)
                {
                    empid = dtEmp.Rows[0]["Emp_Id"].ToString();

                    if (empid != hdnEmpId.Value)
                    {
                        pnlTermination.Visible = false;

                    }
                    hdnEmpId.Value = empid;
                    Session["ET_Hdnempid"] = empid;
                    txtTermDate.Focus();
                }
                else
                {
                    DisplayMessage("Employee Not Exists");
                    txtEmpName.Text = "";
                    Session["ET_Hdnempid"] = 0;
                    txtEmpName.Focus();
                    hdnEmpId.Value = "";
                    return;
                }

            }
            catch (Exception er)
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                Session["ET_Hdnempid"] = 0;
                txtEmpName.Focus();
                hdnEmpId.Value = "";
                return;
            }
        }
        //AllPageCode();
    }


    protected void txtHandledby2_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        hdnHandledby.Value = "0";

        if (txtHandledby2.Text != "")
        {
            empid = txtHandledby2.Text.Split('/')[txtHandledby2.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            try
            {
                dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmp.Rows.Count > 0)
                {

                    txtHandledby2.Text = "" + dtEmp.Rows[0]["Emp_Name"].ToString() + "/(" + dtEmp.Rows[0]["Designation"].ToString() + ")/" + dtEmp.Rows[0]["Emp_Code"].ToString() + "";

                    empid = dtEmp.Rows[0]["Emp_Id"].ToString();

                    hdnHandledby.Value = empid;

                    ddlType.Focus();
                }
                else
                {
                    DisplayMessage("Employee Not Exists");
                    txtHandledby2.Text = "";

                    txtHandledby2.Focus();
                    hdnHandledby.Value = "0";
                    return;
                }
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                txtHandledby2.Text = "";

                txtHandledby2.Focus();
                hdnHandledby.Value = "0";
                return;
            }
        }
        //AllPageCode();
    }
    public string setDateFormat(string Date)
    {
        string NewDate = string.Empty;
        if (Date != "")
        {
            try
            {
                NewDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
            }
            catch
            {

            }
        }
        return NewDate;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
            DataTable dtCust = (DataTable)Session["Company"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Emp_Terminate"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvTermination, view.ToTable(), "", "");
        }
        txtValue.Focus();
        //AllPageCode();
    }
    public void FillGrid()
    {
        EmployeeMaster objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        DataTable dtEmp = objEmp.GetEmployeeMasterAllData(HttpContext.Current.Session["CompId"].ToString());

        try
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id in (" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (HttpContext.Current.Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        string ValidEmployee = string.Empty;
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            ValidEmployee += dtEmp.Rows[i]["Emp_Id"].ToString() + ",";
        }

        DataTable DtTermination = objEmp.GetEmployeeTermination_By_CompanyId(Session["CompId"].ToString());
        try
        {
            DtTermination = new DataView(DtTermination, "Employee_Id in(" + ValidEmployee.Substring(0, ValidEmployee.Length - 1) + ")", "id", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        //Common Function add By Lokesh on 23-05-2015

        objPageCmn.FillData((object)gvTermination, DtTermination, "", "");
        Session["dtFilter_Emp_Terminate"] = DtTermination;
        Session["Company"] = DtTermination;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + DtTermination.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    protected void gvTermination_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTermination.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Emp_Terminate"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvTermination, dt, "", "");
        //AllPageCode();
    }
    protected void gvTermination_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Emp_Terminate"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Emp_Terminate"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvTermination, dt, "", "");
        //AllPageCode();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string strPost = string.Empty;


        DataTable dtTerminationData = objEmp.GetEmployeeTermination_By_EmployeeId(e.CommandArgument.ToString());
        if (dtTerminationData.Rows.Count > 0)
        {
            strPost = dtTerminationData.Rows[0]["Field3"].ToString();
            if (strPost == "True")
            {
                DisplayMessage("That is Posted Record you cant Edit");
                hdnTerminationId.Value = "0";
                hdnEmpId.Value = "0";
                return;
            }
        }
        hdnTerminationId.Value = e.CommandArgument.ToString();

        txtEmpName.Text = dtTerminationData.Rows[0]["EmpName"].ToString() + "/(" + dtTerminationData.Rows[0]["DesignationName"].ToString() + ")/" + dtTerminationData.Rows[0]["EmpCode"].ToString() + "";
        hdnEmpId.Value = dtTerminationData.Rows[0]["Employee_Id"].ToString();
        txtEmpName.ReadOnly = true;
        txtTermDate.Text = Convert.ToDateTime(dtTerminationData.Rows[0]["Termination_Date"].ToString()).ToString(objSys.SetDateFormat());
        txtTermDate.ReadOnly = true;

        //btnApply_Click(sender, e);

        //btnApply.Visible = false;
        //btnLogProcess.Visible = false;
        //btnGeneratePayroll.Visible = false;

        ddlType.SelectedValue = dtTerminationData.Rows[0]["Type"].ToString();
        ddlType.Enabled = false;
        txtReason.Text = dtTerminationData.Rows[0]["Reason"].ToString();
        txtReason.ReadOnly = true;
        txtPaidAmount.Text = dtTerminationData.Rows[0]["Company_PaidAmount"].ToString();
        txtPaidAmountEmployeeCurrency.Text = dtTerminationData.Rows[0]["Employee_PaidAmount"].ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListName1(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        //if (HttpContext.Current.Session["ET_Hdnempid"] != null && HttpContext.Current.Session["ET_Hdnempid"].ToString() != "")
        //    dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%') and Emp_Id<>" + HttpContext.Current.Session["ET_Hdnempid"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //else
        //    dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = "" + dt.Rows[i]["Emp_Name"].ToString() + "/(" + dt.Rows[i]["Designation"].ToString() + ")/" + dt.Rows[i]["Emp_Code"].ToString() + "";
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster objEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);// objEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        try
        {
            //            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
        }
        if (HttpContext.Current.Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }

        //if (dtEmp.Rows.Count > 0)
        //{
        //    dtEmp = new DataView(dtEmp, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        //}



        string[] str = new string[dtEmp.Rows.Count];
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            str[i] = "" + dtEmp.Rows[i]["Emp_Name"].ToString() + "/(" + dtEmp.Rows[i]["Designation"].ToString() + ")/" + dtEmp.Rows[i]["Emp_Code"].ToString() + "";
        }
        return str;
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnApply_Click(null, null);

    }

    #region PrintReport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_Termination_Report.aspx?Id=" + e.CommandArgument.ToString() + "')", true);
        string strlocCurrencyId = string.Empty;
        DataTable dtLocation = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (dtLocation.Rows.Count > 0)
        {
            strlocCurrencyId = dtLocation.Rows[0]["Field1"].ToString();
        }

        string strPaymentVoucherAcc = string.Empty;

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Employee Account'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        DateTime Dtfromdate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
        DateTime dtTodate = DateTime.Now;
        int fyear_id = Common.getFinancialYearId(Dtfromdate, Session["DBConnection"].ToString(), Session["CompId"].ToString());

        DataTable dtOpeningBalance = ObjDa.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + Dtfromdate + "', '0','" + strPaymentVoucherAcc + "','" + e.CommandArgument.ToString() + "','1', '" + fyear_id + "')) OpeningBalance");

        string strOpeningDebit = dtOpeningBalance.Rows[0][0].ToString();

        ArrayList objArr = new ArrayList();

        objArr.Add(strPaymentVoucherAcc);
        objArr.Add(Dtfromdate.ToString(objSys.SetDateFormat()));
        objArr.Add(dtTodate.ToString(objSys.SetDateFormat()));
        objArr.Add(Session["LocId"].ToString());
        objArr.Add(strOpeningDebit);
        objArr.Add("--Select--");
        objArr.Add("1");
        objArr.Add(strlocCurrencyId);
        if (Convert.ToDouble(strOpeningDebit) <= 0)
        {
            objArr.Add("Debit");
        }
        else
        {
            objArr.Add("Credit");
        }
        objArr.Add(e.CommandArgument.ToString());
        objArr.Add(dtEmp.Rows[0]["Emp_Name"].ToString() + "-" + dtEmp.Rows[0]["Emp_Code"].ToString());


        Session["dtAcParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/AccountStatement.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void IbtnPayslipt_Command(object sender, CommandEventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        DateTime Terminationdate = Convert.ToDateTime(((Label)gvrow.FindControl("lblterminationdate")).Text);
        Session["Querystring"] = e.CommandArgument.ToString();
        Session["Month"] = Terminationdate.Month;
        Session["Year"] = Terminationdate.Year;

        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
        string strMonthName = mfi.GetMonthName(Terminationdate.Month).ToString();

        Session["MonthName"] = strMonthName;


        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/Employee_PaySlip_Report.aspx')", true);

    }
    #endregion

    protected void btnSaveGST_Click(object sender, EventArgs e)
    {

        if (txtname.Text == "")
        {

        }
        else
        {
            string empid = txtname.Text;
            int pos = empid.LastIndexOf("/") + 1;
            string id = empid.Substring(pos, empid.Length - pos);

            string ss = Hdn_Edit_ID.Value;
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
            ObjEmployeeMaster.SetHandledBy(HttpContext.Current.Session["CompId"].ToString(), ss, id);
            FillGrid();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);

        }
    }

    protected void txtname_TextChanged(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return txt;
    }


    protected void btnEdit1_Command(object sender, CommandEventArgs e)
    {
        txtname.Text = e.CommandName.ToString();
        Hdn_Edit_ID.Value = e.CommandArgument.ToString();
        //Hdn_Name.Value =
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
    }

    protected void btnRemoveID_Click(object sender, EventArgs e)
    {
        string ss = Hdn_Edit_ID.Value;
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjEmployeeMaster.SetHandledBy(HttpContext.Current.Session["CompId"].ToString(), ss, "");
        FillGrid();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
    }



    public bool EmployeeLeaveSalary(DateTime DOJ, DateTime DtTermination, string strPaymentCreditAccount, string strPaymentDebitAccount, ref SqlTransaction trns)
    {

        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        }
        catch
        {
            FinancialYearMonth = 1;
        }

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }



        DateTime dtLSFromdate = new DateTime();
        DateTime dtLSTodate = DtTermination;
        DataTable dtPaidLeave = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtLeaveEmpInfo = ObjDa.return_DataTable("Select Att_LeaveMaster.Field1 AS ISLeaveSalary From Set_Att_Employee_Leave INNER JOIN  Att_LeaveMaster ON  Set_Att_Employee_Leave.LeaveType_Id =  Att_LeaveMaster.Leave_Id  AND  Att_LeaveMaster.Field1 = 'True' AND  Set_Att_Employee_Leave.Emp_Id = '" + hdnEmpId.Value + "' and Set_Att_Employee_Leave.Paid_Leave>0", ref trns);
        DataTable dtPaidLeave_Temp = new DataTable();
        DataTable dtMaxDate = ObjDa.return_DataTable("select MAX( dateadd(day,1, to_date)) from att_leavesalary where emp_id=" + hdnEmpId.Value + " and F5='Approved'", ref trns);


        if (dtLeaveEmpInfo.Rows.Count == 0)
        {
            try
            {
                dtLSFromdate = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString());
            }
            catch
            {
                dtLSFromdate = DOJ;
            }
        }
        else
        {
            try
            {
                if (Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).Year == FinancialYearStartDate.Year)
                {
                    dtLSFromdate = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString());
                }
                else
                {
                    dtLSFromdate = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1);
                }

            }
            catch
            {
                if (DOJ.Year == FinancialYearStartDate.Year)
                {
                    dtLSFromdate = DOJ;
                }
                else
                {
                    dtLSFromdate = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1);
                }

            }

        }


        if (dtLSFromdate.Month == dtLSTodate.Month && dtLSFromdate.Year == dtLSTodate.Year)
        {
            //leave salary already generated till termination date 
            return true;
        }

        //here we are writing code for leave salary 


        DataTable dtleavesalary = new DataTable();
        dtleavesalary.Columns.Add("Is_Report", typeof(bool));
        dtleavesalary.Columns.Add("From_Date");
        dtleavesalary.Columns.Add("To_Date");
        dtleavesalary.Columns.Add("Trans_Id", typeof(float));
        dtleavesalary.Columns.Add("Leave_Type_Id");
        dtleavesalary.Columns.Add("Total_Leave_Count");
        dtleavesalary.Columns.Add("Leave_Count");
        dtleavesalary.Columns.Add("Used_Leave_Count");
        dtleavesalary.Columns.Add("Balance_Leave_Count");
        dtleavesalary.Columns.Add("Per_Day_Salary");
        dtleavesalary.Columns.Add("Total");




        dtLeaveEmpInfo = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Assign_Days,Att_Employee_Leave_Trans.Leave_Type_Id from Att_Employee_Leave_Trans inner join  Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id=Att_LeaveMaster.Leave_Id where  Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive='True' and Att_Employee_Leave_Trans.Year=" + dtLSFromdate.Year + " AND Att_Employee_Leave_Trans.EMP_ID=" + hdnEmpId.Value + " and Att_Employee_Leave_Trans.isactive='True'", ref trns);

        if (dtLeaveEmpInfo.Rows.Count == 0)
        {
            return true;
        }

        double TotalLeaveSal = 0;
        double TotalLeaveSum = 0;
        double ActualLeaveSum = 0;
        double UsedLeaveSum = 0;
        double BalanceLeaveSum = 0;
        double PLeave = 0;
        double TotalLeaveCount = 0;
        double UsedLeaveCount = 0;
        double balanceLeaveCount = 0;
        string LsApplicableAllowances = string.Empty;
        string LS_Month_Days_Count = string.Empty;
        string LsPayScale = string.Empty;
        double Basicsalary = 0;
        bool IsWeekOffSalary = false;
        bool IsHolidaySalary = false;
        bool IsAbsentSalary = false;
        bool IsPaidLeaveForLeavesalary = false;
        bool IsUnPaidLeaveForLeavesalary = false;
        DateTime dtFromdate = new DateTime();
        DateTime dtTodate = new DateTime();
        double LeaveSalWeekOffCount = 0;
        double LeaveSalHolidayCount = 0;
        double LeaveSalAbsentCount = 0;
        double LeaveSalWorkedDays = 0;
        double LeaveSalPaidleave = 0;
        double LeaveSalunPaidleave = 0;
        double PerDaySalary = 0;
        string strLEaveTypeId = string.Empty;
        dtFromdate = dtLSFromdate;
        dtTodate = dtLSTodate;


        //here we are checking that leave salary generted or not for selected date criteria
        //27-09-2017
        if (ObjDa.return_DataTable("select Emp_Id from Att_LeaveSalary where F5='Approved' and Emp_Id =" + hdnEmpId.Value + " and (((From_Date  between '" + dtFromdate + "'  And '" + dtTodate + "') or (To_Date  between '" + dtFromdate + "'  And '" + dtTodate + "')))", ref trns).Rows.Count > 0)
        {

            return true;
        }


        LsApplicableAllowances = objAppParam.GetApplicationParameterValueByParamName("LS_ApplicableAllowance", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        LS_Month_Days_Count = objAppParam.GetApplicationParameterValueByParamName("LS_Month_Days_Count", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        LsPayScale = objAppParam.GetApplicationParameterValueByParamName("Ls_Pay_Scale", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        IsWeekOffSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsWeekOffLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        IsHolidaySalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsHolidayLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        IsAbsentSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAbsentLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        IsPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        IsUnPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsUnPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns));
        string strrmonthList = string.Empty;

        double Allowancevalue = 0;
        DataTable dtAttendanceRegister = new DataTable();
        DateTime dtStartdate = new DateTime();
        DateTime dtEndDate = new DateTime();
        DataTable dtTemp = new DataTable();
        int Trans_Id = 0;

        double TotalLeavesalary = 0;
        int loopcounter = 0;


        while (dtFromdate <= dtTodate)
        {

            if (strrmonthList == "")
            {
                strrmonthList = dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
            }
            else if (!strrmonthList.Split(',').Contains(dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString()))
            {
                strrmonthList = strrmonthList + "," + dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
            }

            if (dtFromdate.Month == DOJ.Month && dtFromdate.Year == DOJ.Year)
            {
                dtStartdate = DOJ;
                dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
            }
            else
            {
                if (loopcounter == 0)
                {
                    dtStartdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day);
                    dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
                }
                else
                {
                    dtStartdate = new DateTime(dtFromdate.Year, dtFromdate.Month, 1);
                    dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));

                }

            }

            dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + dtStartdate.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'", ref trns);

            dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtStartdate + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "", ref trns);



            if (LsPayScale == "Actual")
            {
                try
                {
                    Basicsalary = Convert.ToDouble(ObjDa.return_DataTable("select Field6 from pay_employe_month where Emp_Id = " + hdnEmpId.Value + " and Month =" + dtEndDate.Month + " and YEAR = " + dtEndDate.Year + "", ref trns).Rows[0][0].ToString());
                }
                catch
                {
                    Basicsalary = 0;
                }
            }
            else
            {
                try
                {
                    Basicsalary = Convert.ToDouble(ObjDa.return_DataTable("select basic_salary from Set_Employee_Parameter where Emp_Id=" + hdnEmpId.Value + "", ref trns).Rows[0][0].ToString());
                }
                catch
                {
                    Basicsalary = 0;
                }

            }


            if (LsApplicableAllowances != "0")
            {
                try
                {
                    Allowancevalue = Convert.ToDouble(ObjDa.return_DataTable("select isnull( sum(Allowance_Value),0) from Pay_Employe_Allowance where Emp_Id = " + hdnEmpId.Value + " and Month =" + dtEndDate.Month + " and YEAR = " + dtEndDate.Year + " and Allowance_Id in (" + LsApplicableAllowances.Substring(0, LsApplicableAllowances.Length - 1) + ")", ref trns).Rows[0][0].ToString());
                }
                catch
                {
                    Allowancevalue = 0;
                }
            }

            if (LS_Month_Days_Count == "Month Days")
            {
                PerDaySalary = (Basicsalary + Allowancevalue) / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month);
            }
            else
            {
                PerDaySalary = (Basicsalary + Allowancevalue) / Convert.ToDouble(LS_Month_Days_Count);
            }

            if (IsWeekOffSalary)
            {
                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Week_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                    LeaveSalWeekOffCount = dtTemp.Rows.Count;
                }
            }


            if (IsHolidaySalary)
            {
                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataTable();
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Holiday='True'", "", DataViewRowState.CurrentRows).ToTable();

                    LeaveSalHolidayCount = dtTemp.Rows.Count;
                }
            }


            if (IsAbsentSalary)
            {

                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataTable();
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Absent='True'", "", DataViewRowState.CurrentRows).ToTable();

                    LeaveSalAbsentCount = dtTemp.Rows.Count;
                }
            }


            if (IsPaidLeaveForLeavesalary)
            {
                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataTable();
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Leave='True' and EffectiveWork_Min>0", "", DataViewRowState.CurrentRows).ToTable();

                    LeaveSalPaidleave = dtTemp.Rows.Count;
                }

            }



            if (IsUnPaidLeaveForLeavesalary)
            {
                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataTable();
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Leave='True' and EffectiveWork_Min<=0", "", DataViewRowState.CurrentRows).ToTable();

                    LeaveSalunPaidleave = dtTemp.Rows.Count;
                }

            }



            if (dtAttendanceRegister.Rows.Count > 0)
            {

                dtTemp = new DataTable();
                dtTemp = new DataView(dtAttendanceRegister, "Is_Week_Off='False' and Is_Holiday='False' and Is_Absent='False' and Is_Leave='False' ", "", DataViewRowState.CurrentRows).ToTable();
            }

            LeaveSalWorkedDays = dtTemp.Rows.Count;


            LeaveSalWorkedDays = LeaveSalWorkedDays + LeaveSalunPaidleave + LeaveSalPaidleave + LeaveSalWeekOffCount + LeaveSalAbsentCount + LeaveSalHolidayCount;



            for (int iLeave = 0; iLeave < dtLeaveEmpInfo.Rows.Count; iLeave++)
            {
                Trans_Id++;
                UsedLeaveCount = 0;
                //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()
                strLEaveTypeId = dtLeaveEmpInfo.Rows[iLeave]["Leave_Type_Id"].ToString();
                PLeave = Convert.ToDouble(dtLeaveEmpInfo.Rows[iLeave]["Assign_Days"].ToString()) / 12;
                //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()

                if (dtPaidLeave.Rows.Count > 0)
                {
                    dtPaidLeave_Temp = new DataView(dtPaidLeave, "Leave_type_Id=" + strLEaveTypeId + "", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtPaidLeave_Temp.Rows.Count > 0)
                    {
                        UsedLeaveCount = dtPaidLeave_Temp.Rows.Count;
                    }
                }

                TotalLeaveCount = PLeave;

                if (dtEndDate.Month == DOJ.Month && dtEndDate.Year == DOJ.Year && DOJ.Day > 1)
                {
                    DateTime dtEm = new DateTime(dtEndDate.Year, dtEndDate.Month, DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month));
                    int rDays = Convert.ToInt32((dtEm - DOJ).TotalDays);
                    PLeave = (PLeave / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month)) * rDays;

                    PLeave = (PLeave / rDays) * LeaveSalWorkedDays;
                }
                else
                {
                    PLeave = (PLeave / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month)) * LeaveSalWorkedDays;
                }



                balanceLeaveCount = PLeave - UsedLeaveCount;

                TotalLeaveSal = balanceLeaveCount * PerDaySalary;

                DataRow dr = dtleavesalary.NewRow();
                dr[0] = false;
                dr[1] = dtStartdate.ToString();
                dr[2] = dtEndDate.ToString();
                dr[3] = Trans_Id;
                dr[4] = strLEaveTypeId;
                dr[5] = Common.GetAmountDecimal(TotalLeaveCount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dr[6] = Common.GetAmountDecimal(PLeave.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dr[7] = Common.GetAmountDecimal(UsedLeaveCount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dr[8] = Common.GetAmountDecimal(balanceLeaveCount.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dr[9] = Common.GetAmountDecimal(PerDaySalary.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dr[10] = Common.GetAmountDecimal((Convert.ToDouble(dr[8].ToString()) * Convert.ToDouble(dr[9].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                TotalLeavesalary += Convert.ToDouble(dr[10].ToString());
                TotalLeaveSum += TotalLeaveCount;
                ActualLeaveSum += PLeave;
                UsedLeaveSum += UsedLeaveCount;
                BalanceLeaveSum += balanceLeaveCount;
                dtleavesalary.Rows.Add(dr);


            }



            loopcounter++;

            dtFromdate = dtFromdate.AddMonths(1);


        }
        //here we are saving leave salary in table and finance section



        DataTable dtLeaveTrans = new DataTable();
        DataTable dtleave = new DataTable();

        dtleave.Columns.Add("LeaveId");
        dtleave.Columns.Add("TotalLeave");
        dtleave.Columns.Add("usedLeave");



        double PaidTotaamount = 0;


        string strnarration = string.Empty;
        double TotalLeave = 0;
        double usedLeave = 0;
        string strLeaveTypeId = string.Empty;
        double UsedDays_Trans = 0;
        double PaidDays_Trans = 0;
        double Remainingdays_Trans = 0;
        int MaxCounter = 0;


        foreach (DataRow gvrow in dtleavesalary.Rows)
        {


            strLeaveTypeId = gvrow["Leave_Type_Id"].ToString();

            //here we are checking employee ligible or not

            if (!objAttendance.IsLeaveMaturity(hdnEmpId.Value, strLeaveTypeId, ref trns, Session["CompId"].ToString(), Session["TimeZoneId"].ToString()))
            {
                continue;
            }

            dtTemp = new DataView(dtleave, "LeaveId=" + strLeaveTypeId + "", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTemp.Rows.Count > 0)
            {
                for (int i = 0; i < dtleave.Rows.Count; i++)
                {
                    if (dtleave.Rows[i]["LeaveId"].ToString() != strLeaveTypeId)
                    {
                        break;
                    }

                    TotalLeave = Convert.ToDouble(dtleave.Rows[i]["TotalLeave"].ToString());
                    usedLeave = Convert.ToDouble(dtleave.Rows[i]["usedLeave"].ToString());

                    dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = Common.GetAmountDecimal((TotalLeave + Convert.ToDouble(gvrow["Total_Leave_Count"].ToString())).ToString(), ref trns, Session["LocCurrencyId"].ToString());
                    dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = Common.GetAmountDecimal((usedLeave + Convert.ToDouble(gvrow["Used_Leave_Count"].ToString())).ToString(), ref trns, Session["LocCurrencyId"].ToString());
                }
            }
            else
            {
                dtleave.Rows.Add();
                dtleave.Rows[dtleave.Rows.Count - 1]["LeaveId"] = strLeaveTypeId;
                dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = gvrow["Total_Leave_Count"].ToString();
                dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = gvrow["Used_Leave_Count"].ToString();
            }

            PaidTotaamount += Convert.ToDouble(Common.GetAmountDecimal(gvrow["Total"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
        }




        string strTransId = string.Empty;



        string strSQL = string.Empty;
        int b = 0;


        MaxCounter = Convert.ToInt32(ObjDa.return_DataTable("select isnull( max(cast(F6 as int)),0)+1 from att_leavesalary where F6<>'' and F6 is not null", ref trns).Rows[0][0].ToString());
        //here  are checking that employee finished leave maturity or not , if maturity assigned on leave level 

        //code start


        foreach (DataRow gvrow in dtleavesalary.Rows)
        {
            if (!objAttendance.IsLeaveMaturity(hdnEmpId.Value, gvrow["Leave_Type_Id"].ToString(), ref trns, Session["CompId"].ToString(), Session["TimeZoneId"].ToString()))
            {
                continue;
            }

            strSQL = "INSERT INTO Att_LeaveSalary (Emp_Id,L_Month,L_Year ,Leave_Type_Id,Leave_Count,Per_Day_Salary,Total,Is_Report,F1,F2,F3,F4,F5,F6,F7,From_Date,To_Date)  VALUES ('" + hdnEmpId.Value + "','" + Convert.ToDateTime(gvrow["From_Date"].ToString()).Month + "','" + Convert.ToDateTime(gvrow["From_Date"].ToString()).Year + "','" + gvrow["Leave_Type_Id"].ToString() + "','" + Common.GetAmountDecimal(gvrow["Leave_Count"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','" + Common.GetAmountDecimal(gvrow["Per_Day_Salary"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','" + Common.GetAmountDecimal(gvrow["Total"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','" + true.ToString() + "','" + string.Empty + "','" + Common.GetAmountDecimal(gvrow["Total_Leave_Count"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','" + Common.GetAmountDecimal(gvrow["Used_Leave_Count"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','" + Common.GetAmountDecimal(gvrow["Balance_Leave_Count"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()) + "','Approved','" + MaxCounter.ToString() + "','True','" + Convert.ToDateTime(gvrow["From_date"].ToString()).ToString() + "','" + Convert.ToDateTime(gvrow["To_date"].ToString()).ToString() + "')";
            b = ObjDa.execute_Command(strSQL, ref trns);

        }

        //if generated leave salary greatter then 0 then we will update leave balance 
        if (PaidTotaamount > 0)
        {
            foreach (DataRow dr in dtleave.Rows)
            {
                dtLeaveTrans = ObjDa.return_DataTable("select Trans_Id,Att_Employee_Leave_Trans.Leave_Type_Id,Att_Employee_Leave_Trans.used_days,Att_Employee_Leave_Trans.Remaining_Days,Att_Employee_Leave_Trans.Field2 from Att_Employee_Leave_Trans inner join Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id = Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id = " + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive = 'True' and Att_Employee_Leave_Trans.Year = '" + DtTermination.Year + "' and Att_Employee_Leave_Trans.Field1 <> '0' and Att_Employee_Leave_Trans.IsActive='True' and Att_Employee_Leave_Trans.Leave_Type_Id='" + dr["LeaveId"].ToString() + "'", ref trns);

                //used leave updated for india location
                if (dtLeaveTrans.Rows.Count > 0)
                {
                    UsedDays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["used_days"].ToString());
                    PaidDays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["Field2"].ToString());
                    Remainingdays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["Remaining_Days"].ToString());

                    UsedDays_Trans = UsedDays_Trans + (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    PaidDays_Trans = PaidDays_Trans - (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    Remainingdays_Trans = Remainingdays_Trans - (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));

                    ObjDa.execute_Command("update Att_Employee_Leave_Trans set used_days=" + UsedDays_Trans + ",Remaining_Days=" + Remainingdays_Trans + ",Field2='" + PaidDays_Trans + "' where trans_id=" + dtLeaveTrans.Rows[0]["Trans_Id"].ToString() + "", ref trns);
                }
            }
        }

        int MaxId = 0;

        if (PaidTotaamount != 0)
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
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

            //for Voucher Number
            string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            if (strVoucherNumber != "")
            {
                int counter = objAcParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);


                if (counter == 0)
                {
                    strVoucherNumber = strVoucherNumber + "1";
                }
                else
                {
                    strVoucherNumber = strVoucherNumber + (counter + 1);
                }
            }
            //

            if (PaidTotaamount > 0)
            {
                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", MaxCounter.ToString(), "Leave Salary", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Leave salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", strCurrencyId, "1", "Leave salary From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", false.ToString(), false.ToString(), false.ToString(), "JV", "", "Approved", hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", MaxCounter.ToString(), "Leave Salary", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Excess leave usage deduction For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", strCurrencyId, "1", "Excess leave usage deduction From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", false.ToString(), false.ToString(), false.ToString(), "JV", "", "Approved", hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            string strVMaxId = MaxId.ToString();



            double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), ref trns));

            string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
            string strLocAmount = PaidTotaamount.ToString();
            string strForeignAmount = PaidTotaamount.ToString();
            string strForeignExchangerate = "1";



            //str for Employee Id


            if (PaidTotaamount > 0)
            {
                //For Debit

                if (strAccountId.Split(',').Contains(strPaymentDebitAccount))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentDebitAccount, "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentDebitAccount, "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                if (strAccountId.Split(',').Contains(strPaymentCreditAccount))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentCreditAccount, hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentCreditAccount, hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            else
            { //For credit


                PaidTotaamount = Math.Abs(PaidTotaamount);

                strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                strLocAmount = PaidTotaamount.ToString();
                strForeignAmount = PaidTotaamount.ToString();
                strForeignExchangerate = "1";




                if (strAccountId.Split(',').Contains(strPaymentDebitAccount))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentDebitAccount, "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Excess leave usage deduction For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentDebitAccount, "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Excess leave usage deduction For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Debit
                if (strAccountId.Split(',').Contains(strPaymentCreditAccount))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentCreditAccount, hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Excess leave usage deduction For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", strPaymentCreditAccount, hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Excess leave usage deduction For '" + txtEmpName.Text.Split('/')[0].ToString() + "' From '" + dtLSFromdate.ToString(objSys.SetDateFormat()) + "' to '" + dtLSTodate.ToString(objSys.SetDateFormat()) + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

            }
        }

        return true;

    }

    public bool Check_Leave_Salary()
    {
        bool Is_Leave_Salary_Generated = true;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        string Emp_DOJ = string.Empty;
        string Leave_Salary_From_Date = string.Empty;
        string Leave_Salary_To_Date = txtTermDate.Text;
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value);
        DataTable dtPaidLeave = new DataTable();
        DataTable dtPaidLeave_Temp = new DataTable();
        DataTable dtleavesalary = new DataTable();
        dtleavesalary.Columns.Add("Is_Report", typeof(bool));
        dtleavesalary.Columns.Add("From_Date");
        dtleavesalary.Columns.Add("To_Date");
        dtleavesalary.Columns.Add("Trans_Id", typeof(float));
        dtleavesalary.Columns.Add("Leave_Type_Id");
        dtleavesalary.Columns.Add("Total_Leave_Count");
        dtleavesalary.Columns.Add("Leave_Count");
        dtleavesalary.Columns.Add("Used_Leave_Count");
        dtleavesalary.Columns.Add("Balance_Leave_Count");
        dtleavesalary.Columns.Add("Per_Day_Salary");
        dtleavesalary.Columns.Add("Total");
        string strsql = string.Empty;
        DataTable dt = new DataTable();

        if (dtEmp != null && dtEmp.Rows.Count > 0)
        {
            Emp_DOJ = dtEmp.Rows[0]["DOJ"].ToString();
        }
        try
        {
            FinancialYearMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        DataTable Dt_Leave_Salary = ObjDa.return_DataTable("Select Set_Att_Employee_Leave.* , Att_LeaveMaster.Field1 AS ISLeaveSalary From Set_Att_Employee_Leave INNER JOIN  Att_LeaveMaster ON  Set_Att_Employee_Leave.LeaveType_Id =  Att_LeaveMaster.Leave_Id  AND  Att_LeaveMaster.Field1 = 'True' AND  Set_Att_Employee_Leave.Emp_Id = '" + hdnEmpId.Value + "' and Set_Att_Employee_Leave.Paid_Leave>0");
        //here we are getting max date for leave salary
        DataTable dtMaxDate = ObjDa.return_DataTable("select MAX( dateadd(day,1, to_date)) from att_leavesalary where emp_id=" + hdnEmpId.Value + "");
        if (Dt_Leave_Salary.Rows.Count == 0)
        {
            try
            {
                Leave_Salary_From_Date = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).ToString(objSys.SetDateFormat());
            }
            catch
            {
                Leave_Salary_From_Date = Convert.ToDateTime(Emp_DOJ).ToString(objSys.SetDateFormat());
            }
        }
        else
        {
            try
            {
                if (Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).Year == FinancialYearStartDate.Year)
                {
                    Leave_Salary_From_Date = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).ToString(objSys.SetDateFormat());
                }
                else
                {
                    Leave_Salary_From_Date = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1).ToString(objSys.SetDateFormat());
                }

            }
            catch
            {
                if (Convert.ToDateTime(Emp_DOJ).Year == FinancialYearStartDate.Year)
                {
                    Leave_Salary_From_Date = Convert.ToDateTime(Emp_DOJ).ToString(objSys.SetDateFormat());
                }
                else
                {
                    Leave_Salary_From_Date = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1).ToString(objSys.SetDateFormat());
                }
            }
        }

        DataTable Dt_Leave_Salary_Assigned = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Assign_Days,Att_Employee_Leave_Trans.Leave_Type_Id from Att_Employee_Leave_Trans inner join  Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id=Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id=" + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive='True' and Att_Employee_Leave_Trans.Year=" + Convert.ToDateTime(Leave_Salary_From_Date).Year + "");

        if (Dt_Leave_Salary_Assigned.Rows.Count > 0)
        {
            double TotalLeaveSal = 0;
            double TotalLeaveSum = 0;
            double ActualLeaveSum = 0;
            double UsedLeaveSum = 0;
            double BalanceLeaveSum = 0;
            double PLeave = 0;
            double TotalLeaveCount = 0;
            double UsedLeaveCount = 0;
            double balanceLeaveCount = 0;
            string LsApplicableAllowances = string.Empty;
            string LS_Month_Days_Count = string.Empty;
            string LsPayScale = string.Empty;
            double Basicsalary = 0;
            bool IsWeekOffSalary = false;
            bool IsHolidaySalary = false;
            bool IsAbsentSalary = false;
            bool IsPaidLeaveForLeavesalary = false;
            bool IsUnPaidLeaveForLeavesalary = false;
            DateTime dtFromdate = new DateTime();
            DateTime dtTodate = new DateTime();
            double LeaveSalWeekOffCount = 0;
            double LeaveSalHolidayCount = 0;
            double LeaveSalAbsentCount = 0;
            double LeaveSalWorkedDays = 0;
            double LeaveSalPaidleave = 0;
            double LeaveSalunPaidleave = 0;
            double PerDaySalary = 0;
            string strLEaveTypeId = string.Empty;
            dtFromdate = Convert.ToDateTime(Leave_Salary_From_Date);
            dtTodate = Convert.ToDateTime(Leave_Salary_To_Date);
            //here we are checking that leave salary generted or not for selected date criteria
            //27-09-2017
            //if (ObjDa.return_DataTable("select Emp_Id from Att_LeaveSalary where Emp_Id =" + hdnEmpId.Value + " and (((From_Date  between '" + dtFromdate + "'  And '" + dtTodate + "') or (To_Date  between '" + dtFromdate + "'  And '" + dtTodate + "')))").Rows.Count > 0)
            //{
            //    DisplayMessage("Leave Salary Generated for selected date criteria");
            //    return;
            //}
            LsApplicableAllowances = objAppParam.GetApplicationParameterValueByParamName("LS_ApplicableAllowance", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            LS_Month_Days_Count = objAppParam.GetApplicationParameterValueByParamName("LS_Month_Days_Count", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            LsPayScale = objAppParam.GetApplicationParameterValueByParamName("Ls_Pay_Scale", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            IsWeekOffSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsWeekOffLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            IsHolidaySalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsHolidayLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            IsAbsentSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAbsentLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            IsPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            IsUnPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsUnPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            string strrmonthList = string.Empty;
            double Allowancevalue = 0;
            DataTable dtAttendanceRegister = new DataTable();
            DateTime dtStartdate = new DateTime();
            DateTime dtEndDate = new DateTime();
            DataTable dtTemp = new DataTable();
            int Trans_Id = 0;
            DataTable DTDOJ = objAtt.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
            DateTime DOJ = Convert.ToDateTime(DTDOJ.Rows[0]["DOJ"].ToString());
            double TotalLeavesalary = 0;
            int loopcounter = 0;
            while (dtFromdate.Month <= dtTodate.Month)
            {
                if (strrmonthList == "")
                {
                    strrmonthList = dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
                }
                else if (!strrmonthList.Split(',').Contains(dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString()))
                {
                    strrmonthList = strrmonthList + "," + dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
                }
                if (dtFromdate.Month == DOJ.Month && dtFromdate.Year == DOJ.Year)
                {
                    dtStartdate = DOJ;
                    dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
                }
                else
                {
                    if (loopcounter == 0)
                    {
                        dtStartdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day);
                        dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
                    }
                    else
                    {
                        dtStartdate = new DateTime(dtFromdate.Year, dtFromdate.Month, 1);
                        dtEndDate = new DateTime(dtFromdate.Year, dtFromdate.Month, DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
                    }
                }
                dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + dtStartdate.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'");
                dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtStartdate + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "");
                if (LsPayScale == "Actual")
                {
                    try
                    {
                        Basicsalary = Convert.ToDouble(ObjDa.return_DataTable("select Field6 from pay_employe_month where Emp_Id = " + hdnEmpId.Value + " and Month =" + dtEndDate.Month + " and YEAR = " + dtEndDate.Year + "").Rows[0][0].ToString());
                    }
                    catch
                    {
                        Basicsalary = 0;
                    }
                }
                else
                {
                    try
                    {
                        Basicsalary = Convert.ToDouble(ObjDa.return_DataTable("select basic_salary from Set_Employee_Parameter where Emp_Id=" + hdnEmpId.Value + "").Rows[0][0].ToString());
                    }
                    catch
                    {
                        Basicsalary = 0;
                    }
                }
                if (LsApplicableAllowances != "0")
                {
                    try
                    {
                        Allowancevalue = Convert.ToDouble(ObjDa.return_DataTable("select isnull( sum(Allowance_Value),0) from Pay_Employe_Allowance where Emp_Id = " + hdnEmpId.Value + " and Month =" + dtEndDate.Month + " and YEAR = " + dtEndDate.Year + " and Allowance_Id in (" + LsApplicableAllowances.Substring(0, LsApplicableAllowances.Length - 1) + ")").Rows[0][0].ToString());
                    }
                    catch
                    {
                        Allowancevalue = 0;
                    }
                }
                if (LS_Month_Days_Count == "Month Days")
                {
                    PerDaySalary = (Basicsalary + Allowancevalue) / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month);
                }
                else
                {
                    PerDaySalary = (Basicsalary + Allowancevalue) / Convert.ToDouble(LS_Month_Days_Count);
                }
                if (IsWeekOffSalary)
                {
                    if (dtAttendanceRegister.Rows.Count > 0)
                    {
                        dtTemp = new DataView(dtAttendanceRegister, "Is_Week_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                        LeaveSalWeekOffCount = dtTemp.Rows.Count;
                    }
                }
                if (IsHolidaySalary)
                {
                    if (dtAttendanceRegister.Rows.Count > 0)
                    {
                        dtTemp = new DataTable();
                        dtTemp = new DataView(dtAttendanceRegister, "Is_Holiday='True'", "", DataViewRowState.CurrentRows).ToTable();
                        LeaveSalHolidayCount = dtTemp.Rows.Count;
                    }
                }
                if (IsAbsentSalary)
                {
                    if (dtAttendanceRegister.Rows.Count > 0)
                    {
                        dtTemp = new DataTable();
                        dtTemp = new DataView(dtAttendanceRegister, "Is_Absent='True'", "", DataViewRowState.CurrentRows).ToTable();
                        LeaveSalAbsentCount = dtTemp.Rows.Count;
                    }
                }
                if (IsPaidLeaveForLeavesalary)
                {
                    if (dtAttendanceRegister.Rows.Count > 0)
                    {
                        dtTemp = new DataTable();
                        dtTemp = new DataView(dtAttendanceRegister, "Is_Leave='True' and EffectiveWork_Min>0", "", DataViewRowState.CurrentRows).ToTable();
                        LeaveSalPaidleave = dtTemp.Rows.Count;
                    }
                }
                if (IsUnPaidLeaveForLeavesalary)
                {
                    if (dtAttendanceRegister.Rows.Count > 0)
                    {
                        dtTemp = new DataTable();
                        dtTemp = new DataView(dtAttendanceRegister, "Is_Leave='True' and EffectiveWork_Min<=0", "", DataViewRowState.CurrentRows).ToTable();
                        LeaveSalunPaidleave = dtTemp.Rows.Count;
                    }
                }
                if (dtAttendanceRegister.Rows.Count > 0)
                {
                    dtTemp = new DataTable();
                    dtTemp = new DataView(dtAttendanceRegister, "Is_Week_Off='False' and Is_Holiday='False' and Is_Absent='False' and Is_Leave='False' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                LeaveSalWorkedDays = dtTemp.Rows.Count;
                LeaveSalWorkedDays = LeaveSalWorkedDays + LeaveSalunPaidleave + LeaveSalPaidleave + LeaveSalWeekOffCount + LeaveSalAbsentCount + LeaveSalHolidayCount;

                DataTable dtLeaveEmpInfo = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Assign_Days,Att_Employee_Leave_Trans.Leave_Type_Id from Att_Employee_Leave_Trans inner join  Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id=Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id=" + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive='True' and Att_Employee_Leave_Trans.Year=" + Convert.ToDateTime(Leave_Salary_From_Date).Year + "");
                if (dtLeaveEmpInfo != null && dtLeaveEmpInfo.Rows.Count > 0)
                {
                    for (int iLeave = 0; iLeave < dtLeaveEmpInfo.Rows.Count; iLeave++)
                    {
                        Trans_Id++;
                        UsedLeaveCount = 0;
                        //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()
                        strLEaveTypeId = dtLeaveEmpInfo.Rows[iLeave]["Leave_Type_Id"].ToString();
                        PLeave = Convert.ToDouble(dtLeaveEmpInfo.Rows[iLeave]["Assign_Days"].ToString()) / 12;
                        //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()
                        if (dtPaidLeave.Rows.Count > 0)
                        {
                            dtPaidLeave_Temp = new DataView(dtPaidLeave, "Leave_type_Id=" + strLEaveTypeId + "", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtPaidLeave_Temp.Rows.Count > 0)
                            {
                                UsedLeaveCount = dtPaidLeave_Temp.Rows.Count;
                            }
                        }
                        TotalLeaveCount = PLeave;
                        if (dtEndDate.Month == DOJ.Month && dtEndDate.Year == DOJ.Year && DOJ.Day > 1)
                        {
                            DateTime dtEm = new DateTime(dtEndDate.Year, dtEndDate.Month, DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month));
                            int rDays = Convert.ToInt32((dtEm - DOJ).TotalDays);
                            PLeave = (PLeave / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month)) * rDays;
                            PLeave = (PLeave / rDays) * LeaveSalWorkedDays;
                        }
                        else
                        {
                            PLeave = (PLeave / DateTime.DaysInMonth(dtEndDate.Year, dtEndDate.Month)) * LeaveSalWorkedDays;
                        }
                        balanceLeaveCount = PLeave - UsedLeaveCount;
                        TotalLeaveSal = balanceLeaveCount * PerDaySalary;
                        DataRow dr = dtleavesalary.NewRow();
                        dr[0] = false;
                        dr[1] = dtStartdate.ToString();
                        dr[2] = dtEndDate.ToString();
                        dr[3] = Trans_Id;
                        dr[4] = strLEaveTypeId;
                        dr[5] = TotalLeaveCount;
                        dr[6] = PLeave.ToString();
                        dr[7] = UsedLeaveCount.ToString();
                        dr[8] = balanceLeaveCount.ToString();
                        dr[9] = PerDaySalary.ToString();
                        dr[10] = TotalLeaveSal.ToString();
                        TotalLeavesalary += TotalLeaveSal;
                        TotalLeaveSum += TotalLeaveCount;
                        ActualLeaveSum += PLeave;
                        UsedLeaveSum += UsedLeaveCount;
                        BalanceLeaveSum += balanceLeaveCount;
                        dtleavesalary.Rows.Add(dr);
                    }
                    loopcounter++;
                    dtFromdate = dtFromdate.AddMonths(1);
                }
            }
            if (dtleavesalary.Rows.Count > 0)
            {
                Is_Leave_Salary_Generated = false;
                //cmn.FillData((object)gvLeaveSalaryDetail, dtleavesalary, "", "");
            }
        }
        return Is_Leave_Salary_Generated;
    }

    protected void btnFoleUpload_Command(object sender, CommandEventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/Employee", "HR", "Employee", e.CommandName.ToString(), ((Label)gvrow.FindControl("lblEmpName")).Text + "(" + e.CommandName.ToString() + ")");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}