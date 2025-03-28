using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class HR_EmployeeLeaveSalary : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
 
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Set_DocNumber objDocNo = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    HR_Leave_Salary objLeaveSalary = null;
    Pay_Employee_claim objPayEmpClaim = null;
    Attendance objAtt = null;
    Pay_Employee_claim ObjClaim = null;
    Attendance objAttendance = null;
    CurrencyMaster objcurrency = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass ObjDa = null;
    LocationMaster objLocation = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_Approval_Employee objApproalEmp = null;
    Att_Leave_Request objleaveReq = null;
    PageControlCommon objPageCmn = null;

    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objLeaveSalary = new HR_Leave_Salary(Session["DBConnection"].ToString());
        objPayEmpClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objcurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/EmployeeLeaveSalary.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtLSEmpName.Focus();

            string symbol = objcurrency.GetCurrencyMasterById(Session["LocCurrencyId"].ToString()).Rows[0]["Currency_symbol"].ToString();
            try
            {
                ViewState["Symbol"] = symbol.ToString();
            }
            catch
            {
                ViewState["Symbol"] = "";
            }
            txtApplyLeaveCount.Text = "0";
            txtApplyLeaveSalarytotal.Text = "0";
            txtEmpName.Focus();
            lblClaimTotal.Text = "0";
            lblFinalTotal.Text = "0";
            //txtUse.Text = "0";
            //FinalTotal.Text = "0";
            txtLeaveCount.Text = "0";


            txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
            CalendarExtender1.Format = objSys.SetDateFormat();
            try
            {
                txtpaymentCreditaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
                txtpaymentCreditaccountApproved.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
                txtLSDebitAccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());

                txtpaymentdebitaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Leave Salary Account").Rows[0]["Param_Value"].ToString());
                txtpaymentdebitaccountApproved.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Leave Salary Account").Rows[0]["Param_Value"].ToString());


            }
            catch
            {

            }

        }
        //// AllPageCode();
    }

    protected void Btn_List_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {

    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnLSSave.Visible = clsPagePermission.bEdit;
        btnSave.Visible = clsPagePermission.bEdit;
        btnLsSaveApproved.Visible = clsPagePermission.bEdit;
    }
    protected void btnGetLeaveSalary_Click(object sender, EventArgs e)
    {

        divGenerateLeaveSalary.Visible = false;
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        objPageCmn.FillData((object)gvLeaveSalaryDetail, null, "", "");
        txtLSEmpName.Enabled = true;
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

        if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())))
        {
            DisplayMessage("Leave Salary Functionality is disable in company parameter");
            return;
        }


        //for check employee terminated or not

        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value);

        if (dtEmp.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtEmp.Rows[0]["Field2"].ToString()))
            {
                DisplayMessage("Employee is Terminated You can see his report only");
                return;
            }
        }


        try
        {
            Convert.ToDateTime(txtFromDate.Text);

        }
        catch
        {
            DisplayMessage("From date is invalid");
            txtFromDate.Text = "";
            txtFromDate.Focus();
            return;
        }



        try
        {
            Convert.ToDateTime(txtToDate.Text);

        }
        catch
        {
            DisplayMessage("To date is invalid");
            txtToDate.Text = "";
            txtToDate.Focus();
            return;
        }



        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            DisplayMessage("From date should be less then to date");
            txtFromDate.Focus();
            return;
        }


        if (Convert.ToDateTime(txtFromDate.Text).Year != Convert.ToDateTime(txtToDate.Text).Year)
        {
            DisplayMessage("Year should be same for selected date criteria");
            txtToDate.Focus();
            return;
        }

        DateTime dtFinaldate = new DateTime();
        int daysinMonth = DateTime.DaysInMonth(Convert.ToDateTime(txtToDate.Text).Year, Convert.ToDateTime(txtToDate.Text).Month);
        dtFinaldate = new DateTime(Convert.ToDateTime(txtToDate.Text).Year, Convert.ToDateTime(txtToDate.Text).Month, daysinMonth);

        if (Convert.ToDateTime(txtToDate.Text) != dtFinaldate)
        {
            DisplayMessage("To date should be month end date");
            txtToDate.Focus();
            return;
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
        dtFromdate = Convert.ToDateTime(txtFromDate.Text);
        dtTodate = Convert.ToDateTime(txtToDate.Text);


        //here we are checking that leave salary generted or not for selected date criteria
        //27-09-2017
        if (ObjDa.return_DataTable("select Emp_Id from Att_LeaveSalary where Emp_Id =" + hdnEmpId.Value + " and (((From_Date  between '" + dtFromdate + "'  And '" + dtTodate + "') or (To_Date  between '" + dtFromdate + "'  And '" + dtTodate + "')))").Rows.Count > 0)
        {
            DisplayMessage("Leave Salary Generated for selected date criteria");
            return;
        }


        LsApplicableAllowances = objAppParam.GetApplicationParameterValueByParamName("LS_ApplicableAllowance", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        LS_Month_Days_Count = objAppParam.GetApplicationParameterValueByParamName("LS_Month_Days_Count", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        LsPayScale = objAppParam.GetApplicationParameterValueByParamName("Ls_Pay_Scale", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        IsWeekOffSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsWeekOffLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsHolidaySalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsHolidayLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsAbsentSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAbsentLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsUnPaidLeaveForLeavesalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsUnPaidLeaveForLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        string strrmonthList = string.Empty;

        while (dtFromdate <= dtTodate)
        {


            dt = ObjDa.return_DataTable("select Emp_Id from Pay_Employee_Attendance where Emp_Id = " + hdnEmpId.Value + " and MONTH =" + dtFromdate.Month + "  and YEAR = '" + dtFromdate.Year + "'");

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Log Should be post for selected date criteria");
                txtFromDate.Focus();
                return;

            }

            dt = ObjDa.return_DataTable("select Emp_Id from Pay_Employe_Month where Emp_Id = " + hdnEmpId.Value + "  and MONTH =" + dtFromdate.Month + "  and YEAR = '" + dtFromdate.Year + "'");

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Payroll Should be post for selected date criteria");
                txtFromDate.Focus();
                return;

            }

            if (strrmonthList == "")
            {
                strrmonthList = dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
            }
            else if (!strrmonthList.Split(',').Contains(dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString()))
            {
                strrmonthList = strrmonthList + "," + dtFromdate.Month.ToString() + "-" + dtFromdate.Year.ToString();
            }


            dtFromdate = dtFromdate.AddDays(1);


        }


        dtFromdate = Convert.ToDateTime(txtFromDate.Text);



        int CurrentMonth = 0;
        int CurrentYear = 0;
        double Allowancevalue = 0;
        DataTable dtAttendanceRegister = new DataTable();
        DateTime dtStartdate = new DateTime();
        DateTime dtEndDate = new DateTime();
        DataTable dtTemp = new DataTable();
        int Trans_Id = 0;
        DataTable DTDOJ = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
        DateTime DOJ = Convert.ToDateTime(DTDOJ.Rows[0]["DOJ"].ToString());
        double TotalLeavesalary = 0;
        foreach (string str in strrmonthList.Split(','))
        {
            UsedLeaveCount = 0;
            LeaveSalWeekOffCount = 0;
            LeaveSalHolidayCount = 0;
            LeaveSalAbsentCount = 0;
            LeaveSalPaidleave = 0;
            LeaveSalunPaidleave = 0;
            LeaveSalWorkedDays = 0;
            CurrentMonth = int.Parse(str.Split('-')[0].ToString());
            CurrentYear = int.Parse(str.Split('-')[1].ToString());
            LeaveSalWorkedDays = 0;
            dtStartdate = dtFromdate;
            while (dtFromdate <= dtTodate)
            {

                dtFromdate = dtFromdate.AddDays(1);


                if (dtFromdate.Month != CurrentMonth || dtFromdate.Year != CurrentYear)
                {
                    dtEndDate = dtFromdate.AddDays(-1);


                    if (dtEndDate.Month == DOJ.Month && dtEndDate.Year == DOJ.Year && DOJ.Day > 1)
                    {
                        dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + DOJ.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'");

                        dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + DOJ + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "");
                    }
                    else
                    {
                        dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + dtStartdate.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'");

                        dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtStartdate + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "");
                    }




                    break;
                }
                else if (dtFromdate == dtTodate)
                {
                    dtEndDate = dtFromdate;
                    if (dtEndDate.Month == DOJ.Month && dtEndDate.Year == DOJ.Year && DOJ.Day > 1)
                    {
                        dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + DOJ.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'");

                        dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + DOJ + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "");
                    }
                    else
                    {
                        dtAttendanceRegister = ObjDa.return_DataTable("select Is_Absent,Is_Week_Off,is_holiday,Is_Leave,Effectivework_Min from Att_AttendanceRegister where Emp_Id=" + hdnEmpId.Value + " and Att_Date>='" + dtStartdate.ToString() + "' and Att_Date<='" + dtEndDate.ToString() + "'");

                        dtPaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Leave_Type_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtStartdate + "' and '" + dtEndDate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "");
                    }
                    break;
                }

            }


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

            DataTable dtLeaveEmpInfo = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Assign_Days,Att_Employee_Leave_Trans.Leave_Type_Id from Att_Employee_Leave_Trans inner join  Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id=Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id=" + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive='True' and Att_Employee_Leave_Trans.Year=" + Convert.ToDateTime(txtFromDate.Text).Year + "");

            for (int iLeave = 0; iLeave < dtLeaveEmpInfo.Rows.Count; iLeave++)
            {
                Trans_Id++;
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

                //string strSQL = "INSERT INTO Att_LeaveSalary (Emp_Id,L_Month,L_Year ,Leave_Type_Id,Leave_Count,Per_Day_Salary,Total,Is_Report,F1,F2,F3,F4,F5,F6,F7)  VALUES ('" + empidlist.Split(',')[i].ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + txtYear.Text + "','" + strLEaveTypeId + "','" + PLeave + "','" + PerDaySalary + "','" + TotalLeaveSal + "','" + false.ToString() + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "')";
                //int q = ObjDa.execute_Command(strSQL);
            }
        }

        if (dtleavesalary.Rows.Count > 0)
        {
            divGenerateLeaveSalary.Visible = true;
            objPageCmn.FillData((object)gvLeaveSalaryDetail, dtleavesalary, "", "");
            txtLSEmpName.Enabled = false;
        }


        try
        {
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum")).Text = Common.GetAmountDecimal(TotalLeavesalary.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_LeaveCount")).Text = Common.GetAmountDecimal(TotalLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text = Common.GetAmountDecimal(ActualLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_UsedLeaveCount")).Text = Common.GetAmountDecimal(UsedLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text = Common.GetAmountDecimal(BalanceLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        catch
        {

        }

    }



    protected void btnGetLeaveSalary1_Click(object sender, EventArgs e)
    {

        DataTable dtleaveSalary = new DataTable();


        divPendingLeaveSalary.Visible = false;

        dtleaveSalary = ObjDa.return_DataTable("select att_leavemaster.Leave_Name as LeaveName, att_leavesalary.from_date,att_leavesalary.to_date, att_leavesalary.f2 as TotalLeave,att_leavesalary.Leave_Count as ActualLeave,att_leavesalary.F3 as UsedLeave,att_leavesalary.F4 as BalanceLeave,att_leavesalary.Per_Day_Salary,att_leavesalary.Total from att_leavesalary inner join att_leavemaster on att_leavesalary.Leave_Type_Id = att_leavemaster.Leave_Id where att_leavesalary.Emp_Id=" + hdnEmpId.Value + " and att_leavesalary.F5='Pending'");

        if (dtleaveSalary.Rows.Count > 0)
        {
            divPendingLeaveSalary.Visible = true;
            gvPendingLeaveSalary.DataSource = dtleaveSalary;
            gvPendingLeaveSalary.DataBind();

            double Total = 0;


            for (int i = 0; i < dtleaveSalary.Rows.Count; i++)
            {
                Total += Convert.ToDouble(dtleaveSalary.Rows[i]["Total"].ToString());
            }

            try
            {
                ((Label)gvPendingLeaveSalary.FooterRow.FindControl("lblTotalSum")).Text = Common.GetAmountDecimal(Total.ToString(), Session["DBConnection"].ToString(),Session["CompId"].ToString());
            }
            catch
            {

            }

        }



        dtleaveSalary = ObjDa.return_DataTable("select emp_id  from att_leavesalary where emp_id=" + hdnEmpId.Value + " and F5='Pending'");

        if (dtleaveSalary.Rows.Count > 0)
        {
            DisplayMessage("Already previous request is under process");
            txtLSEmpName.Focus();
            return;
        }



        divGenerateLeaveSalary.Visible = false;
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        objPageCmn.FillData((object)gvLeaveSalaryDetail, null, "", "");
        txtLSEmpName.Enabled = true;
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

        if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(),Session["BrandId"].ToString(),Session["LocId"].ToString())))
        {
            DisplayMessage("Leave Salary Functionality is disable in company parameter");
            return;
        }


        //for check employee terminated or not

        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value);

        if (dtEmp.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtEmp.Rows[0]["Field2"].ToString()))
            {
                DisplayMessage("Employee is Terminated You can see his report only");
                return;
            }
        }


        try
        {
            Convert.ToDateTime(txtFromDate.Text);

        }
        catch
        {
            DisplayMessage("From date is invalid");
            txtFromDate.Text = "";
            txtFromDate.Focus();
            return;
        }



        try
        {
            Convert.ToDateTime(txtToDate.Text);

        }
        catch
        {
            DisplayMessage("To date is invalid");
            txtToDate.Text = "";
            txtToDate.Focus();
            return;
        }



        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            DisplayMessage("From date should be less then to date");
            txtFromDate.Focus();
            return;
        }


        if (Convert.ToDateTime(txtFromDate.Text).Year != Convert.ToDateTime(txtToDate.Text).Year)
        {
            DisplayMessage("Year should be same for selected date criteria");
            txtToDate.Focus();
            return;
        }

        DateTime dtFinaldate = new DateTime();
        int daysinMonth = DateTime.DaysInMonth(Convert.ToDateTime(txtToDate.Text).Year, Convert.ToDateTime(txtToDate.Text).Month);
        dtFinaldate = new DateTime(Convert.ToDateTime(txtToDate.Text).Year, Convert.ToDateTime(txtToDate.Text).Month, daysinMonth);

        if (Convert.ToDateTime(txtToDate.Text) != dtFinaldate)
        {
            DisplayMessage("To date should be month end date");
            txtToDate.Focus();
            return;
        }

        //here we are checking that leave assigned or not in which leave salary flag should be selected

        DataTable dtLeaveEmpInfo = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Assign_Days,Att_Employee_Leave_Trans.Leave_Type_Id from Att_Employee_Leave_Trans inner join  Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id=Att_LeaveMaster.Leave_Id where  Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive='True' and Att_Employee_Leave_Trans.Year=" + Convert.ToDateTime(txtFromDate.Text).Year + " AND Att_Employee_Leave_Trans.EMP_ID=" + hdnEmpId.Value + " and Att_Employee_Leave_Trans.isactive='True'");

        if (dtLeaveEmpInfo.Rows.Count == 0)
        {
            DisplayMessage("Leave not assigned");
            return;
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
        dtFromdate = Convert.ToDateTime(txtFromDate.Text);
        dtTodate = Convert.ToDateTime(txtToDate.Text);


        //here we are checking that leave salary generted or not for selected date criteria
        //27-09-2017
        if (ObjDa.return_DataTable("select Emp_Id from Att_LeaveSalary where F5='Approved' and Emp_Id =" + hdnEmpId.Value + " and (((From_Date  between '" + dtFromdate + "'  And '" + dtTodate + "') or (To_Date  between '" + dtFromdate + "'  And '" + dtTodate + "')))").Rows.Count > 0)
        {
            DisplayMessage("Leave Salary Generated for selected date criteria");
            return;
        }


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
        DataTable DTDOJ = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
        DateTime DOJ = Convert.ToDateTime(DTDOJ.Rows[0]["DOJ"].ToString());
        double TotalLeavesalary = 0;
        int loopcounter = 0;


        while (dtFromdate <= dtTodate)
        {


            dt = ObjDa.return_DataTable("select Emp_Id from Pay_Employee_Attendance where Emp_Id = " + hdnEmpId.Value + " and MONTH =" + dtFromdate.Month + "  and YEAR = '" + dtFromdate.Year + "'");

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Log Should be post for selected date criteria");
                txtFromDate.Focus();
                return;

            }

            dt = ObjDa.return_DataTable("select Emp_Id from Pay_Employe_Month where Emp_Id = " + hdnEmpId.Value + "  and MONTH =" + dtFromdate.Month + "  and YEAR = '" + dtFromdate.Year + "'");

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Payroll Should be post for selected date criteria");
                txtFromDate.Focus();
                return;

            }

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



        if (dtleavesalary.Rows.Count > 0)
        {
            divGenerateLeaveSalary.Visible = true;
            objPageCmn.FillData((object)gvLeaveSalaryDetail, dtleavesalary, "", "");
            txtLSEmpName.Enabled = false;
        }


        try
        {
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum")).Text = Common.GetAmountDecimal(TotalLeavesalary.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_LeaveCount")).Text = Common.GetAmountDecimal(TotalLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text = Common.GetAmountDecimal(ActualLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_UsedLeaveCount")).Text = Common.GetAmountDecimal(UsedLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text = Common.GetAmountDecimal(BalanceLeaveSum.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        catch
        {

        }

    }



    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {

        txtNarration.Text = "";

        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
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

        DataTable dtleaveSalary = new DataTable();
        DataTable dtLeaveEmpInfo = new DataTable();
        DataTable dtMaxDate = new DataTable();
        string empid = string.Empty;


        if (((TextBox)sender).Text != "")
        {
            empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                if (!((TextBox)sender).Text.Trim().Contains('/'))
                {
                    DisplayMessage("Select employee name in suggestion only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }


                txtNarration.Text = "Leave Salary For " + txtEmpName.Text.Split('/')[0].ToString() + "";
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnEmpId.Value = empid;

                //here we are getting max date for leave salary


                if (((TextBox)sender).ID == "txtLSEmpName")
                {

                    txtFromDate.Text = "";
                    txtToDate.Text = "";

                    dtLeaveEmpInfo = ObjDa.return_DataTable("Select Set_Att_Employee_Leave.* , Att_LeaveMaster.Field1 AS ISLeaveSalary From Set_Att_Employee_Leave INNER JOIN  Att_LeaveMaster ON  Set_Att_Employee_Leave.LeaveType_Id =  Att_LeaveMaster.Leave_Id  AND  Att_LeaveMaster.Field1 = 'True' AND  Set_Att_Employee_Leave.Emp_Id = '" + empid + "' and Set_Att_Employee_Leave.Paid_Leave>0");

                    dtMaxDate = ObjDa.return_DataTable("select MAX( dateadd(day,1, to_date)) from att_leavesalary where emp_id=" + hdnEmpId.Value + " and F5='Approved'");


                    if (dtLeaveEmpInfo.Rows.Count == 0)
                    {
                        try
                        {
                            txtFromDate.Text = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).ToString(objSys.SetDateFormat());
                        }
                        catch
                        {
                            txtFromDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).ToString(objSys.SetDateFormat());
                        }
                    }
                    else
                    {
                        try
                        {

                            txtFromDate.Text = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).ToString(objSys.SetDateFormat());
                            //if (Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).Year == FinancialYearStartDate.Year)
                            //{
                            //    txtFromDate.Text = Convert.ToDateTime(dtMaxDate.Rows[0][0].ToString()).ToString(objSys.SetDateFormat());
                            //}
                            //else
                            //{
                            //    txtFromDate.Text = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1).ToString(objSys.SetDateFormat());
                            //}

                        }
                        catch
                        {
                            if (Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).Year == FinancialYearStartDate.Year)
                            {
                                txtFromDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).ToString(objSys.SetDateFormat());
                            }
                            else
                            {
                                txtFromDate.Text = new DateTime(FinancialYearStartDate.Year, FinancialYearStartDate.Month, 1).ToString(objSys.SetDateFormat());
                            }
                        }
                    }
                }

                if (((TextBox)sender).ID == "txtEmpName")
                {
                    RefreshData();
                }

            }
            else
            {
                if (((TextBox)sender).ID == "txtEmpName")
                {
                    RefreshData();
                }

                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
        txtToDate.Focus();

    }


    public void RefreshData()
    {
        gvEmployeeTakenLeave.DataSource = null;
        gvEmployeeTakenLeave.DataBind();
        //gvEmployeeClaim.DataSource = null;
        //gvEmployeeClaim.DataBind();
        gvLeaveDetail.DataSource = null;
        gvLeaveDetail.DataBind();
        gvLeaveSalary.DataSource = null;
        gvLeaveSalary.DataBind();
        divLeave.Visible = false;
        lblClaimTotal.Text = "0";
        lblFinalTotal.Text = "0";
        //txtUse.Text = "0";
        //FinalTotal.Text = "0";
        txtApplyLeaveCount.Text = "0";
        txtApplyLeaveSalarytotal.Text = "0";
        txtLeaveCount.Text = "0";
        divPending.Visible = false;
        divLeaveSalary.Visible = false;
        divLeave.Visible = false;
        divClaim.Visible = false;
        //txtEmpName.Text = "";
        //txtEmpName.Focus();
        Session["dtFilterClaim"] = null;
        Session["dtFilter_Leave_Sal"] = null;
        Session["dtFilterLeave"] = null;
        ViewState["SortDirClaim"] = null;
        ViewState["SortDir"] = null;
        ViewState["SortDirLeave"] = null;
        txtEmpName.Enabled = true;
        //ViewState["Symbol"] = "";
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "Field2='False'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                str = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
                }
            }
        }
        return str;
    }
    protected void btnLSSave_Click(object sender, EventArgs e)
    {
        bool ApprovalFunctionality = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        string strpost = string.Empty;
        string strStatus = string.Empty;

        if (gvLeaveSalaryDetail.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            gvLeaveSalaryDetail.Focus();
            return;
        }


        if (txtpaymentdebitaccount.Text == "")
        {
            DisplayMessage("Configure Leave salary account in finance parameter");
            return;
        }

        if (txtpaymentCreditaccount.Text == "")
        {
            DisplayMessage("Configure Employee salary account in finance parameter");
            return;
        }

        DataTable dtTemp = new DataTable();
        DataTable dtLeaveTrans = new DataTable();
        DataTable dtleave = new DataTable();

        dtleave.Columns.Add("LeaveId");
        dtleave.Columns.Add("TotalLeave");
        dtleave.Columns.Add("usedLeave");

        DataTable dt = new DataTable();
        string EmpPermission = string.Empty;
        strStatus = "Approved";
        if (ApprovalFunctionality)
        {
            string strObjectId = Common.GetObjectIdbyPageURL("../HR/EmployeeLeaveSalary.aspx", Session["DBConnection"].ToString());


            EmpPermission = objSys.Get_Approval_Parameter_By_Name("Leave Salary").Rows[0]["Approval_Level"].ToString();
            if (EmpPermission == "")
            {
                EmpPermission = "Company";
            }
            dt = objApproalEmp.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),strObjectId, hdnEmpId.Value);
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }

            strStatus = "Pending";
        }
        else
        {
            strpost = "True";
        }

        double PaidTotaamount = 0;

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strnarration = string.Empty;
        double TotalLeave = 0;
        double usedLeave = 0;
        string strLeaveTypeId = string.Empty;
        double UsedDays_Trans = 0;
        double PaidDays_Trans = 0;
        double Remainingdays_Trans = 0;
        int MaxCounter = 0;


        foreach (GridViewRow gvrow in gvLeaveSalaryDetail.Rows)
        {

            if (!objAttendance.IsLeaveMaturity(hdnEmpId.Value, ((Label)gvrow.FindControl("lblLeaveTypeId")).Text,Session["CompId"].ToString(),Session["TimeZoneId"].ToString()))
            {
                DisplayMessage("Employee not eligible");
                return;
            }
            strLeaveTypeId = ((Label)gvrow.FindControl("lblLeaveTypeId")).Text;

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

                    dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = Common.GetAmountDecimal((TotalLeave + Convert.ToDouble(((Label)gvrow.FindControl("lblTotalLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = Common.GetAmountDecimal((usedLeave + Convert.ToDouble(((Label)gvrow.FindControl("lblUsedLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                }
            }
            else
            {
                dtleave.Rows.Add();
                dtleave.Rows[dtleave.Rows.Count - 1]["LeaveId"] = strLeaveTypeId;
                dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = ((Label)gvrow.FindControl("lblTotalLeaveCount")).Text;
                dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = ((Label)gvrow.FindControl("lblUsedLeaveCount")).Text;
            }


            //TotalLeave = Convert.ToDouble(((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_LeaveCount")).Text);
            //usedLeave = Convert.ToDouble(((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_UsedLeaveCount")).Text);
            PaidTotaamount += Convert.ToDouble(((Label)gvrow.FindControl("lblTotal")).Text);
        }



        if (PaidTotaamount == 0)
        {
            DisplayMessage("Generated leave salary should be greater then 0");
            return;
        }



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {


            string strSQL = string.Empty;
            int b = 0;


            MaxCounter = Convert.ToInt32(ObjDa.return_DataTable("select isnull( max(cast(F6 as int)),0)+1 from att_leavesalary where F6<>'' and F6 is not null").Rows[0][0].ToString());
            //here  are checking that employee finished leave maturity or not , if maturity assigned on leave level 

            //code start


            foreach (GridViewRow gvrow in gvLeaveSalaryDetail.Rows)
            {
                strSQL = "INSERT INTO Att_LeaveSalary (Emp_Id,L_Month,L_Year ,Leave_Type_Id,Leave_Count,Per_Day_Salary,Total,Is_Report,F1,F2,F3,F4,F5,F6,F7,From_Date,To_Date)  VALUES ('" + hdnEmpId.Value + "','" + Convert.ToDateTime(((Label)gvrow.FindControl("lblFromdate")).Text).Month + "','" + Convert.ToDateTime(((Label)gvrow.FindControl("lblFromdate")).Text).Year + "','" + ((Label)gvrow.FindControl("lblLeaveTypeId")).Text + "','" + ((TextBox)gvrow.FindControl("lblLeaveCount")).Text + "','" + ((Label)gvrow.FindControl("lblPerDaySalary")).Text + "','" + ((Label)gvrow.FindControl("lblTotal")).Text + "','" + false.ToString() + "','" + string.Empty + "','" + ((Label)gvrow.FindControl("lblTotalLeaveCount")).Text + "','" + ((Label)gvrow.FindControl("lblUsedLeaveCount")).Text + "','" + ((Label)gvrow.FindControl("lblBalanceLeaveCount")).Text + "','" + strStatus + "','" + MaxCounter.ToString() + "','" + strpost + "','" + Convert.ToDateTime(((Label)gvrow.FindControl("lblFromdate")).Text).ToString() + "','" + Convert.ToDateTime(((Label)gvrow.FindControl("lblTodate")).Text).ToString() + "')";
                b = ObjDa.execute_Command(strSQL, ref trns);

            }


            if (ApprovalFunctionality)
            {
                GetEmployeeName(hdnEmpId.Value);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int cur_trans_id = 0;
                    string PriorityEmpId = dt.Rows[j]["Emp_Id"].ToString();
                    string IsPriority = dt.Rows[j]["Priority"].ToString();
                    if (EmpPermission == "1")
                    {
                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dt.Rows[j]["Approval_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString(), "0", "0", "0", hdnEmpId.Value, MaxCounter.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);

                    }
                    else if (EmpPermission == "2")
                    {
                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dt.Rows[j]["Approval_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", "0", hdnEmpId.Value, MaxCounter.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);

                    }
                    else if (EmpPermission == "3")
                    {
                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dt.Rows[j]["Approval_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", hdnEmpId.Value, MaxCounter.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);

                    }
                    else
                    {
                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dt.Rows[j]["Approval_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", hdnEmpId.Value, MaxCounter.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                    }



                    Set_Notification(hdnEmpId.Value, PriorityEmpId, txtLSEmpName.Text, "", cur_trans_id.ToString(), ViewState["Emp_Img"].ToString(), txtFromDate.Text, txtToDate.Text, ref trns);

                }
            }
            else
            {

                //if generated leave salary greatter then 0 then we will update leave balance 
                if (PaidTotaamount > 0)
                {

                    foreach (DataRow dr in dtleave.Rows)
                    {
                        dtLeaveTrans = ObjDa.return_DataTable("select Trans_Id,Att_Employee_Leave_Trans.Leave_Type_Id,Att_Employee_Leave_Trans.used_days,Att_Employee_Leave_Trans.Remaining_Days,Att_Employee_Leave_Trans.Field2 from Att_Employee_Leave_Trans inner join Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id = Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id = " + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive = 'True' and Att_Employee_Leave_Trans.Year = '" + Convert.ToDateTime(txtFromDate.Text).Year + "' and Att_Employee_Leave_Trans.Field1 <> '0' and Att_Employee_Leave_Trans.IsActive='True' and Att_Employee_Leave_Trans.Leave_Type_Id='" + dr["LeaveId"].ToString() + "'", ref trns);

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
                string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {

                    //int counter = Ac_ParameterMaster.GetCounterforVoucherNumber(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);


                    //if (counter == 0)
                    //{
                    //    strVoucherNumber = strVoucherNumber + "1";
                    //}
                    //else
                    //{
                    //    strVoucherNumber = strVoucherNumber + (counter + 1);
                    //}

                    if (strVoucherNumber != "")
                    {
                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
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
                            double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                            strVoucherNumber = strVoucherNumber + TotalCount;
                        }
                    }
                }
                //

                if (PaidTotaamount > 0)
                {
                    MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", MaxCounter.ToString(), "Leave Salary", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Leave salary For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", strCurrencyId, "1", "Leave salary From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", false.ToString(), false.ToString(), false.ToString(), "JV", "", strStatus, hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", MaxCounter.ToString(), "Leave Salary", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Excess leave usage deduction For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", strCurrencyId, "1", "Excess leave usage deduction From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", false.ToString(), false.ToString(), false.ToString(), "JV", "", strStatus, hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                string strVMaxId = MaxId.ToString();



                double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));

                string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";



                //str for Employee Id


                if (PaidTotaamount > 0)
                {
                    //For Debit

                    if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Credit
                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                else
                { //For credit


                    PaidTotaamount = Math.Abs(PaidTotaamount);

                    strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                    strLocAmount = PaidTotaamount.ToString();
                    strForeignAmount = PaidTotaamount.ToString();
                    strForeignExchangerate = "1";




                    if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Excess leave usage deduction For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Excess leave usage deduction For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Debit
                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Excess leave usage deduction For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Excess leave usage deduction For '" + txtLSEmpName.Text.Split('/')[0].ToString() + "' From '" + txtFromDate.Text + "' to '" + txtToDate.Text + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                }



            }



            gvLeaveSalaryDetail.DataSource = null;
            gvLeaveSalaryDetail.DataBind();

            divGenerateLeaveSalary.Visible = false;

            DisplayMessage("Record Saved", "green");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();


            RefreshData();


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


    private void Set_Notification(string strEmpId, string PriorityEmpId, string strEmpName, string hdnTransid, string cur_trans_id, string strempImage, string strFromDate, string strTodate, ref SqlTransaction trns)
    {
        NotificationMaster Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/hr"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, strEmpId, PriorityEmpId, ref trns);
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";

        string Message = string.Empty;


        Message = strEmpName + " request for Leave salary from '" + strFromDate + "' to '" + strTodate + "'";


        // For Insert        
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), strEmpId, PriorityEmpId, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", cur_trans_id, "False", strempImage, "", "", "", "", HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0", ref trns);

    }



    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
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


    protected void btnLSCancel_Click(object sender, EventArgs e)
    {
        gvLeaveSalaryDetail.DataSource = null;
        gvLeaveSalaryDetail.DataBind();

        divGenerateLeaveSalary.Visible = false;
        txtLSEmpName.Enabled = true;
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

    public string GetMonthName(int monthNumber)
    {
        string strMonthName = "";
        return strMonthName;
        //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);

    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }

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
    //protected void ddlrefType_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlrefType.SelectedIndex == 0)
    //    {
    //        txtpaymentdebitaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers").Rows[0]["Param_Value"].ToString());
    //    }
    //    else
    //    {
    //        txtpaymentdebitaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers").Rows[0]["Param_Value"].ToString());

    //    }



    //}

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
    #endregion

    #region Payment

    protected void btnLeaveSalary_Click(object sender, EventArgs e)
    {




        DataTable dtEmp = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dtEmp = new DataView(dtEmp, "Emp_Id='" + hdnEmpId.Value + "' and Field2='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtEmp.Rows.Count > 0)
        {
            DisplayMessage("Employee is Terminated You can see his report only");
            return;
        }

        RefreshData();
        lblClaimTotal.Text = "0";
        lblFinalTotal.Text = "0";
        //txtUse.Text = "0";

        //FinalTotal.Text = "0";

        if (txtEmpName.Text != "")
        {
            ViewState["Count"] = 0;
            FillGridLeaveDetail();
            FillGrid();
            FillGrid_LeaveSalaryTaken();

            if (gvLeaveSalary.Rows.Count == 0)
            {
                DisplayMessage("Pending leave salary not found");
                txtEmpName.Focus();
                return;
            }

        }
        else
        {
            DisplayMessage("Select Employee");
            txtEmpName.Text = "";
            txtEmpName.Focus();
            return;
        }
    }
    private void FillGridLeaveDetail()
    {
        DataTable dt = objAtt.GetLeaveRequestByEmpId(Session["CompId"].ToString(), hdnEmpId.Value);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
        Session["dtFilterLeave"] = dt;
        if (dt.Rows.Count > 0)
        {
            divLeave.Visible = true;
            ViewState["Count"] = 1;
        }
        else
        {
            divLeave.Visible = false;
            ViewState["Count"] = 0;
        }
    }
    private void FillGrid()
    {
        DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(hdnEmpId.Value);



        if (dt.Rows.Count > 0)
        {
            lblFinalTotal.Text = dt.Rows[0]["FinalTotal"].ToString();
            txtLeaveCount.Text = dt.Rows[0]["TotalLeaveCount"].ToString();
        }

        lblFinalTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblFinalTotal.Text);
        lblFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";
        lblApplyFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
        TotalLeaveSum();

        Session["dtFilter_Leave_Sal"] = dt;
        if (dt.Rows.Count > 0)
        {
            divLeaveSalary.Visible = true;
            ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
            divPending.Visible = true;
        }
        else
        {
            divLeaveSalary.Visible = false;
            divPending.Visible = false;
        }


    }
    private void FillGrid_LeaveSalaryTaken()
    {
        DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID_LeaveTakenNew(hdnEmpId.Value);



        //lblFinalTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblFinalTotal.Text);
        //lblFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";
        lblClaimTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployeeTakenLeave, dt, "", "");
        TotalLeaveSum_SalaryTaken();

        //Session["dtFilter_Leave_Sal"] = dt;
        if (dt.Rows.Count > 0)
        {
            divClaim.Visible = true;
            //ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
        }
        else
        {
            divClaim.Visible = false;
        }


    }
    protected void gvLeaveDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveDetail.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterLeave"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
    }
    protected void gvLeaveDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterLeave"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDirLeave"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirLeave"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirLeave"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirLeave"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirLeave"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtFilterLeave"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
        //// AllPageCode();
    }
    protected void gvLeaveSalary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveSalary.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Leave_Sal"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
    }
    protected void gvLeaveSalary_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Leave_Sal"];
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
        Session["dtFilter_Leave_Sal"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
        //AllPageCode();
        try
        {
            gvLeaveSalary.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    private void FillGridClaim()
    {
        //DataTable dt = objPayEmpClaim.Get_PayEmployeeClaim_By_EmpId_LeaveSalary(Session["CompId"].ToString(), hdnEmpId.Value);

        //if (dt != null && dt.Rows.Count > 0)
        //{
        //    lblClaimTotal.Text = dt.Rows[0]["FinalTotal"].ToString();
        //}
        //lblClaimTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblClaimTotal.Text);
        //lblClaimTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //gvEmployeeClaim.DataSource = dt;
        //gvEmployeeClaim.DataBind();
        //if (dt.Rows.Count > 0)
        //{

        //    lblClaimTotal.Visible =true ;
        //    ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
        //}
        //else
        //{
        //    lblClaimTotal.Visible = false;
        //}
        //Session["dtFilterClaim"] = dt;
        //if (dt.Rows.Count > 0)
        //{
        //    divClaim.Visible = true;
        //}
        //else
        //{
        //    divClaim.Visible = false;
        //}

    }
    protected void gvEmployeeClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvEmployeeClaim.PageIndex = e.NewPageIndex;
        //DataTable dt = (DataTable)Session["dtFilterClaim"];
        //gvEmployeeClaim.DataSource = dt;
        //gvEmployeeClaim.DataBind();

    }
    protected void gvEmployeeClaim_Sorting(object sender, GridViewSortEventArgs e)
    {
        //    DataTable dt = (DataTable)Session["dtFilterClaim"];
        //    string sortdirclaim = "DESC";
        //    if (ViewState["SortDirClaim"] != null)
        //    {
        //        sortdirclaim = ViewState["SortDirClaim"].ToString();
        //        if (sortdirclaim == "ASC")
        //        {
        //            e.SortDirection = SortDirection.Descending;
        //            ViewState["SortDirClaim"] = "DESC";
        //        }
        //        else
        //        {
        //            e.SortDirection = SortDirection.Ascending;
        //            ViewState["SortDirClaim"] = "ASC";
        //        }
        //    }
        //    else
        //    {
        //        ViewState["SortDirClaim"] = "DESC";
        //    }

        //    dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirClaim"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        //    Session["dtFilterClaim"] = dt;
        //    gvEmployeeClaim.DataSource = dt;
        //    gvEmployeeClaim.DataBind();
        //    //AllPageCode();

    }
    protected void txtUse_OnTextChanged(object sender, EventArgs e)
    {
        //if (txtUse.Text != "")
        //{
        //    if (Convert.ToDouble(txtUse.Text) > Convert.ToDouble(FinalTotal.Text))
        //    {
        //        DisplayMessage("Used Cannot be greater than pending");
        //        txtUse.Text = "";
        //        txtUse.Focus();
        //        return;

        //    }
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strnarration = string.Empty;

        strnarration = "Leave Salary For '" + txtEmpName.Text.Split('/')[0].ToString() + "'";


        if (txtNarration.Text == "")
        {
            DisplayMessage("Enter Narration");
            txtNarration.Focus();
            return;
        }


        if (gvLeaveSalary.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            gvLeaveSalary.Focus();
            return;
        }






        double PaidTotaamount = 0;

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
        {

            if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
            {

                if (!objAttendance.IsLeaveMaturity(hdnEmpId.Value, ((Label)gvrow.FindControl("lblLeaveTypeId")).Text,Session["CompId"].ToString(),Session["TimeZoneId"].ToString()))
                {
                    DisplayMessage("Employee not eligible");
                    return;
                }


                PaidTotaamount += Convert.ToDouble(((Label)gvrow.FindControl("lblTotal")).Text);

            }
        }

        if (PaidTotaamount <= 0)
        {
            DisplayMessage("Leave salary should be gerater then 0");
            return;
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {

            string strSQL = string.Empty;
            int b = 0;

            //here  are checking that employee finished leave maturity or not , if maturity assigned on leave level 

            //code start


            foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
            {
                if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
                {

                    b = objLeaveSalary.UpdateLeaveSalaryStatus(((Label)gvrow.FindControl("lblTransId")).Text, DateTime.Now.ToString(), ref trns);
                }
            }

            int MaxId = 0;


            if (b != 0)
            {
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
                    //string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());

                    string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
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

                    MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", txtNarration.Text, strCurrencyId, "1", txtNarration.Text, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    string strVMaxId = MaxId.ToString();



                    double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));

                    string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                    string strLocAmount = PaidTotaamount.ToString();
                    string strForeignAmount = PaidTotaamount.ToString();
                    string strForeignExchangerate = "1";



                    //str for Employee Id
                    //For Debit


                    if (strAccountId.Split(',').Contains(txtLSDebitAccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSDebitAccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", txtNarration.Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSDebitAccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", txtNarration.Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }



                    //For Credit


                    if (strAccountId.Split(',').Contains(txtLSCreditaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, txtNarration.Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtLSCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, txtNarration.Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                }

                DisplayMessage("Record Saved", "green");
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                RefreshData();
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshData();
    }

    #region Report

    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text != "")
        {
            //if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString())) == false)
            //{
            //DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(hdnEmpId.Value);
            //if (dt.Rows.Count > 0)
            //{
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmployeeLeaveSalary.aspx?EmpId=" + hdnEmpId.Value + "','window','width=1024');", true);
            //}
            //else
            //{
            //    DisplayMessage("Record Not available");
            //    return;
            //}

            //}
            //else
            //{
            //    DisplayMessage("You are Not Applicable");
            //    txtEmpName.Text = "";
            //    txtEmpName.Focus();
            //    return;
            //}
        }
        else
        {
            DisplayMessage("Select Employee");
            txtEmpName.Text = "";
            txtEmpName.Focus();
            return;
        }
    }
    #endregion

    public void TotalLeaveSum_SalaryTaken()
    {

        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);
        foreach (GridViewRow gvrow in gvEmployeeTakenLeave.Rows)
        {

            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }

        }
        lblClaimTotal.Text = TotalLeaveSalary.ToString();

    }
    public void TotalLeaveSum()
    {
        float TotalLeaveCount = float.Parse(txtApplyLeaveCount.Text);
        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);
        foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
        {
            try
            {
                TotalLeaveCount += float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount += 0;
            }
            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }

        }
        txtLeaveCount.Text = TotalLeaveCount.ToString();
        lblFinalTotal.Text = TotalLeaveSalary.ToString();
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvLeaveSalary.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow GVR_Row in gvLeaveSalary.Rows)
        {
            CheckBox chk = (CheckBox)GVR_Row.FindControl("chkselect");
            if (chkSelAll.Checked == true)
                chk.Checked = true;
            else
                chk.Checked = false;
        }
        float TotalLeaveCount = 0;
        float TotalLeaveSalary = 0;
        foreach (GridViewRow GVR_Row in gvLeaveSalary.Rows)
        {
            if (((CheckBox)GVR_Row.FindControl("chkselect")).Checked)
            {
                try
                {
                    TotalLeaveCount += float.Parse(((Label)GVR_Row.FindControl("lblLeaveCount")).Text);
                }
                catch
                {
                    TotalLeaveCount += 0;
                }
                try
                {
                    TotalLeaveSalary += float.Parse(((Label)GVR_Row.FindControl("lblTotal")).Text);
                }
                catch
                {
                    TotalLeaveSalary += 0;
                }
            }
            else
            {
                try
                {
                    TotalLeaveCount = 0;
                }
                catch
                {
                    TotalLeaveCount = 0;
                }
                try
                {
                    TotalLeaveSalary = 0;
                }
                catch
                {
                    TotalLeaveSalary = 0;
                }
            }
        }
        txtApplyLeaveCount.Text = TotalLeaveCount.ToString();
        txtApplyLeaveSalarytotal.Text = TotalLeaveSalary.ToString();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;

        float TotalLeaveCount = float.Parse(txtApplyLeaveCount.Text);
        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);



        if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
        {
            try
            {
                TotalLeaveCount += float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount += 0;
            }
            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }
        }
        else
        {
            try
            {
                TotalLeaveCount -= float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount -= 0;
            }
            try
            {
                TotalLeaveSalary -= float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary -= 0;
            }

        }
        txtApplyLeaveCount.Text = TotalLeaveCount.ToString();
        txtApplyLeaveSalarytotal.Text = TotalLeaveSalary.ToString();

    }

    #endregion


    protected void btnGetLeaveSalaryApproved_Click(object sender, EventArgs e)
    {
        DataTable dtLeavesalary = ObjDa.return_DataTable("select att_leavesalary.Trans_Id,att_leavesalary.Leave_Type_Id,att_leavemaster.Leave_Name as LeaveName, att_leavesalary.from_date,att_leavesalary.to_date, att_leavesalary.f2 as Total_Leave_Count,att_leavesalary.Leave_Count ,att_leavesalary.F3 as Used_Leave_Count,att_leavesalary.F4 as Balance_Leave_Count,att_leavesalary.Per_Day_Salary,att_leavesalary.Total from att_leavesalary inner join att_leavemaster on att_leavesalary.Leave_Type_Id = att_leavemaster.Leave_Id where att_leavesalary.f5='Approved' and att_leavesalary.emp_id=" + hdnEmpId.Value + " and att_leavesalary.f7=''");

        if (dtLeavesalary.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            txtempName_Approved.Focus();
            return;
        }

        double TotalLeave = 0;
        double ActualLeave = 0;
        double UsedLeave = 0;
        double BalanceLeave = 0;
        double TotalSalary = 0;

        divGenerateLeaveSalaryApproved.Visible = false;

        objPageCmn.FillData((object)gvLeavesalaryApproved, null, "", "");
        txtempName_Approved.Enabled = true;

        if (dtLeavesalary.Rows.Count > 0)
        {
            divGenerateLeaveSalaryApproved.Visible = true;
            objPageCmn.FillData((object)gvLeavesalaryApproved, dtLeavesalary, "", "");
            txtempName_Approved.Enabled = false;

            foreach (GridViewRow gvr in gvLeavesalaryApproved.Rows)
            {
                TotalLeave += Convert.ToDouble(((Label)gvr.FindControl("lblTotalLeaveCount")).Text);
                ActualLeave += Convert.ToDouble(((Label)gvr.FindControl("lblLeaveCount")).Text);
                UsedLeave += Convert.ToDouble(((Label)gvr.FindControl("lblUsedLeaveCount")).Text);
                BalanceLeave += Convert.ToDouble(((Label)gvr.FindControl("lblBalanceLeaveCount")).Text);
                TotalSalary += Convert.ToDouble(((Label)gvr.FindControl("lblTotal")).Text);
            }


            try
            {
                ((Label)gvLeavesalaryApproved.FooterRow.FindControl("lblTotalSum_LeaveCount")).Text = Common.GetAmountDecimal(TotalLeave.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                ((Label)gvLeavesalaryApproved.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text = Common.GetAmountDecimal(ActualLeave.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                ((Label)gvLeavesalaryApproved.FooterRow.FindControl("lblTotalSum_UsedLeaveCount")).Text = Common.GetAmountDecimal(UsedLeave.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                ((Label)gvLeavesalaryApproved.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text = Common.GetAmountDecimal(BalanceLeave.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                ((Label)gvLeavesalaryApproved.FooterRow.FindControl("lblTotalSum")).Text = Common.GetAmountDecimal(TotalSalary.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            }
            catch
            {

            }



        }
    }

    protected void btnLsSaveApproved_Click(object sender, EventArgs e)
    {

        string strStatus = string.Empty;

        if (gvLeavesalaryApproved.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            gvLeavesalaryApproved.Focus();
            return;
        }


        if (txtpaymentdebitaccountApproved.Text == "")
        {
            DisplayMessage("Configure Leave salary account in finance parameter");
            return;
        }

        if (txtpaymentCreditaccountApproved.Text == "")
        {
            DisplayMessage("Configure Employee salary account in finance parameter");
            return;
        }



        DataTable dtTemp = new DataTable();
        DataTable dtLeaveTrans = new DataTable();
        DataTable dtleave = new DataTable();

        dtleave.Columns.Add("LeaveId");
        dtleave.Columns.Add("TotalLeave");
        dtleave.Columns.Add("usedLeave");


        double PaidTotaamount = 0;

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strnarration = string.Empty;
        double TotalLeave = 0;
        double usedLeave = 0;
        string strLeaveTypeId = string.Empty;
        double UsedDays_Trans = 0;
        double PaidDays_Trans = 0;
        double Remainingdays_Trans = 0;


        foreach (GridViewRow gvrow in gvLeavesalaryApproved.Rows)
        {
            strLeaveTypeId = ((Label)gvrow.FindControl("lblLeaveTypeId")).Text;

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

                    dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = Common.GetAmountDecimal((TotalLeave + Convert.ToDouble(((Label)gvrow.FindControl("lblTotalLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = Common.GetAmountDecimal((usedLeave + Convert.ToDouble(((Label)gvrow.FindControl("lblUsedLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                }
            }
            else
            {
                dtleave.Rows.Add();
                dtleave.Rows[dtleave.Rows.Count - 1]["LeaveId"] = strLeaveTypeId;
                dtleave.Rows[dtleave.Rows.Count - 1]["TotalLeave"] = ((Label)gvrow.FindControl("lblTotalLeaveCount")).Text;
                dtleave.Rows[dtleave.Rows.Count - 1]["usedLeave"] = ((Label)gvrow.FindControl("lblUsedLeaveCount")).Text;
            }

            PaidTotaamount += Convert.ToDouble(((Label)gvrow.FindControl("lblTotal")).Text);
        }



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {


            string strSQL = string.Empty;
            int b = 0;

            foreach (GridViewRow gvrow in gvLeavesalaryApproved.Rows)
            {
                strSQL = "update Att_LeaveSalary set F7='True' where Trans_id=" + ((Label)gvrow.FindControl("lblTransId")).Text + "";
                b = ObjDa.execute_Command(strSQL, ref trns);

            }


            foreach (DataRow dr in dtleave.Rows)
            {
                dtLeaveTrans = ObjDa.return_DataTable("select Trans_Id,Att_Employee_Leave_Trans.Leave_Type_Id,Att_Employee_Leave_Trans.used_days,Att_Employee_Leave_Trans.Remaining_Days,Att_Employee_Leave_Trans.Field2 from Att_Employee_Leave_Trans inner join Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id = Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id = " + hdnEmpId.Value + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive = 'True' and Att_Employee_Leave_Trans.Year = '" + Convert.ToDateTime(txtFromDate.Text).Year + "' and Att_Employee_Leave_Trans.Field1 <> '0' and Att_Employee_Leave_Trans.IsActive='True' and Att_Employee_Leave_Trans.Leave_Type_Id='" + dr["LeaveId"].ToString() + "'");

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

                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Leave Salary For '" + txtempName_Approved.Text.Split('/')[0].ToString() + "'", strCurrencyId, "1", "Leave Salary", false.ToString(), false.ToString(), false.ToString(), "JV", "", "", hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                string strVMaxId = MaxId.ToString();


                double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));

                string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";



                //str for Employee Id
                //For Debit

                if (strAccountId.Split(',').Contains(txtpaymentdebitaccountApproved.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccountApproved.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtempName_Approved.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccountApproved.Text.Split('/')[1].ToString(), "0", "0", "LS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Leave Salary For '" + txtempName_Approved.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                if (strAccountId.Split(',').Contains(txtpaymentCreditaccountApproved.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccountApproved.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtempName_Approved.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccountApproved.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "LS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Leave Salary For '" + txtempName_Approved.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }


            gvLeavesalaryApproved.DataSource = null;
            gvLeavesalaryApproved.DataBind();
            divGenerateLeaveSalaryApproved.Visible = false;

            DisplayMessage("Record Saved", "green");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            txtempName_Approved.Focus();
            txtempName_Approved.Text = "";
            txtempName_Approved.Enabled = true;
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

    protected void btnLSCancelApproved_Click(object sender, EventArgs e)
    {
        txtempName_Approved.Enabled = true;
        divGenerateLeaveSalaryApproved.Visible = false;
        txtempName_Approved.Text = "";

    }

    protected void btnresetLeaveSalary_Click(object sender, EventArgs e)
    {
        btnLSCancel_Click(null, null);
        txtLSEmpName.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtFromDate.ReadOnly = false;
        divPendingLeaveSalary.Visible = false;
        txtLSEmpName.Focus();
    }

    protected void lblLeaveCount_TextChanged(object sender, EventArgs e)
    {


        GridViewRow Gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        double Totalleave = 0;
        double ActualLeave = 0;
        double Usedleave = 0;
        double BalanceLeave = 0;
        double Perdaysalary = 0;

        try
        {
            ActualLeave = Convert.ToDouble(((TextBox)Gvrow.FindControl("lblLeaveCount")).Text);
        }
        catch
        {
            ((TextBox)Gvrow.FindControl("lblLeaveCount")).Text = "0";
        }


        Totalleave = Convert.ToDouble(((Label)Gvrow.FindControl("lblTotalLeaveCount")).Text);


        if (ActualLeave > Totalleave)
        {
            DisplayMessage("Actual leave should not be greater then total leave");
            ((TextBox)Gvrow.FindControl("lblLeaveCount")).Text = "0";
            ActualLeave = 0;
        }


        try
        {
            Usedleave = Convert.ToDouble(((Label)Gvrow.FindControl("lblUsedLeaveCount")).Text);
        }
        catch
        {
            ((Label)Gvrow.FindControl("lblUsedLeaveCount")).Text = "0";
        }

        try
        {
            BalanceLeave = Convert.ToDouble(((Label)Gvrow.FindControl("lblBalanceLeaveCount")).Text);
        }
        catch
        {
            ((Label)Gvrow.FindControl("lblBalanceLeaveCount")).Text = "0";
        }

        try
        {
            Perdaysalary = Convert.ToDouble(((Label)Gvrow.FindControl("lblPerDaySalary")).Text);
        }
        catch
        {
            ((Label)Gvrow.FindControl("lblPerDaySalary")).Text = "0";
        }

        ((Label)Gvrow.FindControl("lblBalanceLeaveCount")).Text = Common.GetAmountDecimal((ActualLeave - Usedleave).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        ((Label)Gvrow.FindControl("lblTotal")).Text = Common.GetAmountDecimal(((ActualLeave - Usedleave) * Perdaysalary).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        GetGridFooterSum();

    }

    public void GetGridFooterSum()
    {
        ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text = "0";

        ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text = "0";
        ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum")).Text = "0";

        foreach (GridViewRow gvrow in gvLeaveSalaryDetail.Rows)
        {
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text = Common.GetAmountDecimal((Convert.ToDouble(((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_ActualLeaveCount")).Text) + Convert.ToDouble(((TextBox)gvrow.FindControl("lblLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text = Common.GetAmountDecimal((Convert.ToDouble(((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum_BalanceLeaveCount")).Text) + Convert.ToDouble(((Label)gvrow.FindControl("lblBalanceLeaveCount")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum")).Text = Common.GetAmountDecimal((Convert.ToDouble(((Label)gvLeaveSalaryDetail.FooterRow.FindControl("lblTotalSum")).Text) + Convert.ToDouble(((Label)gvrow.FindControl("lblTotal")).Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
    }
}