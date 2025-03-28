using DevExpress.Web;
using PegasusDataAccess;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Attendance_HalfDayRequest : BasePage
{
    Att_AttendanceLog objAttLog = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    SystemParameter objSys = null;
    Att_Employee_Leave objEmpleave = null;
    EmployeeMaster objEmp = null;
    Set_Approval_Employee objApproalEmp = null;
    Att_HalfDay_Request objHalfDay = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    Common cmn = null;
    Att_ScheduleMaster objEmpSch = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Att_PartialLeave_Request objPartial = null;
    Set_ApprovalMaster ObjApproval = null;
    SendMailSms ObjSendMailSms = null;
    Attendance objAttendance = null;
    NotificationMaster Obj_Notifiacation = null;
    CompanyMaster objComp = null;
    Att_HalfDayRequestReport objHalfDayRequestReport = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass Objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        btnApply.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnApply, "").ToString());
        txtEmpName.Focus();

        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objHalfDayRequestReport = new Att_HalfDayRequestReport(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Objda=new DataAccessClass(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        
        if (!IsPostBack)
        {
            Session["empimgpath"] = null;
            Session["empimgpathFull"] = null;

            Common.clsPagePermission clsPagePermission = new Common.clsPagePermission();
            if (Request.QueryString["Emp_Id"] == null)
            {
                clsPagePermission = cmn.getPagePermission("../Attendance/HalfDayRequest.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()); //objObjectEntry.GetModuleIdAndName("211");
            }
            else
            {
                clsPagePermission = cmn.getPagePermission("../Attendance/HalfDayRequest.aspx?emp_id=0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()); //objObjectEntry.GetModuleIdAndName("215");
            }

            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            setYearList();
            txtEmpName.Focus();
            objPageCmn.fillLocationWithAllOption(ddlLoc);
            FillLeaveStatus();
            if (Request.QueryString["Emp_Id"] != null)
            {
                txtEmpName.Text = Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                hdnEmpId.Value = Session["EmpId"].ToString();
                FillEmpLeave(hdnEmpId.Value);
                FillHalfDaySummary(hdnEmpId.Value);
                txtEmpName.Enabled = false;
            }
        }
        CalendarExtender2.Format = objSys.SetDateFormat();
        if (Settings.IsValid == false)
        {
            btnApply.Visible = false;
        }
        if (hdnReportId.Value != "")
        {
            halfDayReport();
        }

    }
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg") && (ext != ".pdf"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge , .pdf extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll"))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll");
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                string TransId = hidtransId.Value;
                //string a = objPartial.UpdateImagePath(TransId, FULogoPath.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName);
                string a = objHalfDay.UpdateImageFile(TransId, FULogoPath.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName);

                Session["empimgpath"] = FULogoPath.FileName;
                Session["empimgpathFull"] = "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName;
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "$('#myModal').modal('toggle');", "", true);
       FillLeaveStatus();
    }
    public void OnDownloadDocumentCommandList(object sender, CommandEventArgs e)
    {
        try
        {
            DataTable dtLeave = new DataTable();
            if (Request.QueryString["Emp_Id"] == null)
            {
                dtLeave = objHalfDay.GetHalfDayRequest(Session["CompId"].ToString(), ddlYearList.SelectedValue);

            }
            else
            {

                dtLeave = objHalfDay.GetHalfDayRequest(Session["CompId"].ToString());

            }

            if (dtLeave.Rows.Count > 0)
            {

                if (ddlLeaveStatus.SelectedIndex == 1)
                {
                    dtLeave = new DataView(dtLeave, "Is_Confirmed='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlLeaveStatus.SelectedIndex == 2)
                {
                    dtLeave = new DataView(dtLeave, "Is_Confirmed='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlLeaveStatus.SelectedIndex == 3)
                {
                    dtLeave = new DataView(dtLeave, "Is_Confirmed='Canceled'", "", DataViewRowState.CurrentRows).ToTable();
                }

                dtLeave = new DataView(dtLeave, "Trans_id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    downloadfile(dtLeave);
                    //resetfile();
                    Page page = new Page();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void downloadfile(DataTable dt)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["Field4"].ToString() + ";");
        response.TransmitFile(Server.MapPath(dt.Rows[0]["Field5"].ToString().Replace("~/", "~//")));
        response.Flush();
        response.End();
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnApply.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    public string getRollBackMonthYear(string EmployeeId)
    {
        string MonthNo = string.Empty;
        // DataTable Dt = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["CompId"].ToString());
        DataTable Dt = objAttLog.Get_Pay_Employee_AttendanceByCompId(Session["CompId"].ToString());
        Dt = new DataView(Dt, "Emp_Id=" + EmployeeId + "", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt.Rows.Count > 0)
        {
            MonthNo = "0";
            try
            {
                Dt = new DataView(Dt, "", "Year desc", DataViewRowState.CurrentRows).ToTable();
                string sortYear = Dt.Rows[0]["Year"].ToString();
                Dt = new DataView(Dt, "Year=" + sortYear + "", "Month desc", DataViewRowState.CurrentRows).ToTable();
                MonthNo = Dt.Rows[0]["Month"].ToString();
               
            }
            catch
            {

            }
        }
        return MonthNo;
    }
    public void FillHalfDaySummary(string EmpId)
    {
        if (EmpId != "")
        {
            string Year = string.Empty;
            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int FinancialYearMonth = 0;
            if (dt.Rows.Count > 0)
            {
                FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
            }
            if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month >= FinancialYearMonth)
            {
                Year = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
            }
            else
            {
                Year = (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1).ToString();
            }
            DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(hdnEmpId.Value, Year);
            if (dtEmpHalf.Rows.Count > 0)
            {
                Session["HR_Half_DtLeaveStatus"] = dtEmpHalf;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSummary, dtEmpHalf, "", "");
            }
            else
            {
                DisplayMessage("No Leave Assign For this Financial Year");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                return;
            }
            dt = null;
        }
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        int b = 0;
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
                    hdnEmpId.Value = empid;
                    FillEmpLeave(hdnEmpId.Value);
                    FillHalfDaySummary(empid);
                    txtApplyDate.Focus();
                    txtInTime.Text = "";
                    txtOuttime.Text = "";
                    panel_inout_time.Visible = false;
                    rbtnHalfDayEvening.Checked = false;
                    rbtnHalfDayMorning.Checked = false;
                }
                else
                {
                    DisplayMessage("Employee Not Exists");
                    txtEmpName.Text = "";
                    txtEmpName.Focus();
                    hdnEmpId.Value = "";
                    return;
                }
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                hdnEmpId.Value = "";
            }
            dtEmp = null;
        }
        else
        {
            objPageCmn.FillData((object)gvEmpPendingLeave, null, "", "");
            objPageCmn.FillData((object)gvLeaveSummary, null, "", "");
        }
    }
    protected void btnApply_Click(object sender, EventArgs e)
    {
        // int ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString()));
        int ProbationMonth = 0;
        bool IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (IsProbationPeriod == true)
        {
            ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        else
        {
            ProbationMonth = 0;
        }
        int b = 0;
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (txtApplyDate.Text == "")
        {
            DisplayMessage("Enter Apply Date");
            txtApplyDate.Focus();
            return;
        }
        else
        {
            try
            {
                objSys.getDateForInput(txtApplyDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct Apply Date Format dd-MMM-yyyy");
                txtApplyDate.Focus();
                return;
            }
        }
        //if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtApplyDate.Text)))
        //{
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}
        //this code is created by jitendra upadhyay on 10-09-2014
        //this code for check posted month before leave request
        //code start
        // Nitin Jain 27/11/2014 , Code TO CHeck Apply Leave before Completion of Probation Period....
        DataTable Dt = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
        DateTime NewDate = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).AddMonths(ProbationMonth);
        if (Convert.ToDateTime(txtApplyDate.Text) >= Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).AddMonths(ProbationMonth))
        {
            // Nitin jain On 28/11/2014 , Code To Give Leave Yearly On Bases of Days Worked
            double UsedDays = 0;
            double PendingDays = 0;
            double Total_Days = 0;
            double PerDayLeave = 0;
            string Schedule_Type = string.Empty;
            int Leave_Id = 0;
            double TotalWorkedDays = 0;
            double Total_WorkedDays_Leave = 0;
            DataTable DtLeaveStatus = (DataTable)Session["HR_Half_DtLeaveStatus"];
            if (DtLeaveStatus.Rows.Count > 0)
            {
                // Leave_Id = Convert.ToInt32(ddlLeaveType.SelectedValue);
                // DtLeaveStatus = new DataView(DtLeaveStatus, "Leave_Type_Id='" + Leave_Id + "' and Shedule_Type='Yearly' ", "", DataViewRowState.CurrentRows).ToTable();
                UsedDays = Convert.ToInt32(DtLeaveStatus.Rows[0]["Used_Days"].ToString());
                PendingDays = Convert.ToInt32(DtLeaveStatus.Rows[0]["Pending_Days"].ToString());
                Total_Days = Convert.ToInt32(DtLeaveStatus.Rows[0]["Total_Days"].ToString());
                // Total Used Lave Till today
                UsedDays = UsedDays + PendingDays;
                // Get Per Day Leave For Employee
                PerDayLeave = Total_Days / 365;
                // Get Joining Date of Employee 
                DateTime DOJ = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString());
                DateTime FinalDate = Convert.ToDateTime(txtApplyDate.Text);
                TotalWorkedDays = (FinalDate - DOJ).TotalDays;
                //if (TotalWorkedDays > 365)
                //{
                //    int temp = Convert.ToInt32(Convert.ToInt64(TotalWorkedDays) % 365);
                //    TotalWorkedDays = temp;

                //}
                Total_WorkedDays_Leave = PerDayLeave * TotalWorkedDays;
                // UsedDays = UsedDays + Convert.ToInt32(lblDays.Text);
                //if (Total_WorkedDays_Leave < UsedDays)
                if (TotalWorkedDays < UsedDays)
                {
                    DisplayMessage("You may not Apply more then " + Total_WorkedDays_Leave + " Leave");
                    return;
                }
            }
        }
        else
        {
            DisplayMessage("You Can Not Leave Request during Probation Period or apply date should be greater or equal to date of joining");
            return;
        }
        //--------------------------------------------------------------------------------------------
        int Monthposted = objSys.getDateForInput(txtApplyDate.Text.ToString()).Month; ;
        //    int Month = Convert.ToDateTime(txtFrom.Text).ToString();
        int YearPosted = Convert.ToDateTime(txtApplyDate.Text).Year;
        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(hdnEmpId.Value, Monthposted.ToString(), YearPosted.ToString());
        if (dtPostedList.Rows.Count > 0)
        {
            DisplayMessage("Log Posted For This Date Criteria");
            return;
        }
        //code end
        if (rbtnHalfDayEvening.Checked == false && rbtnHalfDayMorning.Checked == false)
        {
            DisplayMessage("Please select Morning or Evening");
            rbtnHalfDayEvening.Focus();
            rbtnHalfDayMorning.Focus();
            return;
        }
        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }
        // Check Holiday or Not For Leave Apply For the Day...............
        DateTime fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());
        string Holiday = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), hdnEmpId.Value, fromdate2, fromdate2, "Holiday", 1, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        if (Holiday != string.Empty)
        {
            DisplayMessage(Holiday.ToString());
            //  ClearFormFields();
            return;
        }
        // Holiday Code over ........................................
        //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
        //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
        DateTime fromdateW = Convert.ToDateTime(txtApplyDate.Text);
        string WeekOff = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), hdnEmpId.Value, fromdateW, fromdateW, "WeekOff", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        if (WeekOff != string.Empty)
        {
            DisplayMessage(WeekOff.ToString());
            //  ClearFormFields();
            return;
        }
        // Week Off Code Over .........................................................................
        // Full Day Leave Check Start...............................................
        DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(hdnEmpId.Value);
        DateTime fromdate1 = objSys.getDateForInput(txtApplyDate.Text);
        // Nitin Jain Is Already Approved,Pending  Full Day Leave on Date FOr Employee 
        string FullDay = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), hdnEmpId.Value, fromdate1, fromdate1, "FullDay", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        if (FullDay != string.Empty)
        {
            DisplayMessage(FullDay.ToString());
            //    ClearFormFields();
            return;
        }
        // Full Day Partial Leave CHeck Between Days Code Over ....................................................
        //code start
        DataTable dtPartialLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
        dtPartialLeave = new DataView(dtPartialLeave, "Partial_Leave_Type='0' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id='" + hdnEmpId.Value + "' and Partial_Leave_Date>='" + fromdate1 + "' and Partial_Leave_Date<='" + fromdate1 + "' and Is_Confirmed<>'Canceled'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPartialLeave.Rows.Count > 0)
        {
            DateTime DtFrom = Convert.ToDateTime(dtPartialLeave.Rows[0]["From_Time"].ToString());
            DateTime DtTo = Convert.ToDateTime(dtPartialLeave.Rows[0]["To_Time"].ToString());
            //code start for get the time for moring and evening
            DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(hdnEmpId.Value);
            dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date='" + objSys.getDateForInput(txtApplyDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            string strInTime = string.Empty;
            string strOutTime = string.Empty;
            if (dtShiftAllDate.Rows.Count > 0)
            {
                if (dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString() == "" || dtShiftAllDate.Rows[0]["OFFDuty_Time"].ToString() == "")
                {
                    if (dtPartialLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                    {
                        DisplayMessage("You have Already Apply Partial Leave On Date : " + DateFormat(fromdate1.ToString()) + " So You May Not Apply Half Day Leave");
                        return;
                    }
                }
                else
                {
                    DateTime dtOutTime = new DateTime();
                    try
                    {
                        strInTime = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString()).ToString("HH:mm");
                        strOutTime = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OFFDuty_Time"].ToString()).ToString("HH:mm");
                        dtOutTime = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OFFDuty_Time"].ToString());
                    }
                    catch
                    {
                        strInTime = "00:00";
                        strOutTime = "00:00";
                    }
                    int minute = GetMinuteDiff(strOutTime, strInTime);
                    minute = minute / 2;
                    DateTime dtDate = new DateTime();
                    dtDate = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString());
                    DateTime da1 = dtDate.AddMinutes(minute);
                    if (rbtnHalfDayEvening.Checked == true)
                    {
                        da1 = da1.AddMinutes(1);
                    }
                    if (rbtnHalfDayMorning.Checked == true)
                    {
                        DateTime dtHalfDayDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Day, da1.Hour, da1.Minute, da1.Second);
                        strOutTime = da1.ToString("HH:mm");
                        //set the date for da1 datetime
                        if (DtTo <= dtHalfDayDate)
                        {
                            DisplayMessage("You Have Already Apply For Partial Leave on Morning Duration");
                            return;
                        }
                    }
                    if (rbtnHalfDayEvening.Checked == true)
                    {
                        DateTime dtHalfDayDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Day, da1.Hour, da1.Minute, da1.Second);
                        if (DtTo >= dtHalfDayDate)
                        {
                            if (dtPartialLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                            {
                                DisplayMessage("You Have Already Apply For Partial Leave on Evening Duration");
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                if (dtPartialLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                {
                    DisplayMessage("You have Already Apply Partial Leave On Date : " + DateFormat(fromdate1.ToString()) + " So You May Not Apply Half Day Leave");
                    return;
                }
            }
        }
        //code end
        string HalfDayType = string.Empty;
        if (rbtnHalfDayMorning.Checked)
        {
            HalfDayType = rbtnHalfDayMorning.Text;
        }
        else
        {
            HalfDayType = rbtnHalfDayEvening.Text;
        }
        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }
        int RemainHalfDay = 0;
        int PendingHalfDay = 0;
        int UsedDay = 0;
        if (hdnEdit.Value == "")
        {
            DataTable dt1 = new DataTable();
            string EmpPermission = string.Empty;
            EmpPermission = objSys.Get_Approval_Parameter_By_Name("HalfDay").Rows[0]["Approval_Level"].ToString();
            dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "211", hdnEmpId.Value);
            if (dt1.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }
            string Year = string.Empty;
            if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month >= FinancialYearMonth)
            {
                Year = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
            }
            else
            {
                Year = (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1).ToString();
            }
            DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpId.Value, Session["CompId"].ToString());
            if (dtEmpParam.Rows.Count == 0)
            {
                DisplayMessage("No Half Day Assign to Employee");
                return;
            }
            DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
            //Old Code
            dtHalfDay = new DataView(dtHalfDay, "HalfDay_Date='" + Convert.ToDateTime(txtApplyDate.Text) + "' and HalfDay_Type='" + HalfDayType + "' and Is_Confirmed<>'Canceled'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtHalfDay.Rows.Count > 0)
            {
                DisplayMessage("Half day already apply on Date " + txtApplyDate.Text + " in " + HalfDayType + " ");
                return;
            }
            
            DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(hdnEmpId.Value, Year);
            if (dtEmpHalf.Rows.Count > 0)
            {
                //Add By Rahul Sharma on date 13-05-2023
                string ISRule = Objda.get_SingleValue("Select Field4 from Set_Att_Employee_Leave where Emp_Id='" + hdnEmpId.Value + "' And IsActive='1'");
                if (ISRule == "True")
                {
                    float TotalDays = float.Parse(dtEmpHalf.Rows[0]["Total_Days"].ToString());
                    string UsedDays = Objda.get_SingleValue("Select COUNT(Trans_Id) as UsedDays from Att_HalfDay_Request where Emp_Id='"+ hdnEmpId.Value + "' And Is_Confirmed !='Canceled' And YEAR(HalfDay_Date) in (Select Year(From_Date) from Ac_Finance_Year_Info where Status='Open' And Company_Id='"+ HttpContext.Current.Session["CompId"].ToString() + "')");
                    DateTime currentDateTime = DateTime.Now;
                    // Get the current month
                    string currentMonth = getRollBackMonthYear(hdnEmpId.Value);
                    float RemainingDays = (TotalDays / 12)*float.Parse(currentMonth.ToString());
                    if (RemainingDays > float.Parse(UsedDays))
                    {


                    }
                    else
                    {
                        DisplayMessage("No half day balance");
                        return;
                    }

                }
            }
            else
            {
                int AssignHalfDayLeave = 0;
                if (dtEmpParam.Rows.Count > 0)
                {
                    try
                    {
                        AssignHalfDayLeave = int.Parse(dtEmpParam.Rows[0]["Field12"].ToString());
                    }
                    catch
                    {
                    }
                }
                SaveHalfDayLeave(hdnEmpId.Value, AssignHalfDayLeave.ToString());
                dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(hdnEmpId.Value, Year);
            }
            RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
            PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
            UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
            if (RemainHalfDay != 0)
            {
                RemainHalfDay = RemainHalfDay - 1;
                PendingHalfDay = PendingHalfDay + 1;
                objEmpHalfDay.UpdateEmployeeHalfDayTransaction(Session["CompId"].ToString(), hdnEmpId.Value, Year, "0", UsedDay.ToString(), RemainHalfDay.ToString(), PendingHalfDay.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            }
            else
            {
                DisplayMessage("No half day balance");
                return;
            }

            string FileName = "";
            string FileUrl = "";
            try
            {
                FileName = Session["empimgpath"].ToString();
                FileUrl = Session["empimgpathFull"].ToString();
            }
            catch
            {
              FileName = "";
               FileUrl = "";
            }
            


            b = objHalfDay.InsertHalfDayRequest(Session["CompId"].ToString(), hdnEmpId.Value, HalfDayType, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), "Pending", txtDescription.Text, "", "", "", "",FileName,FileUrl, true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            if (b != 0)
            {
                // objApproalEmp.InsertApprovalChildMaster("HalfDay", b.ToString(), "211", hdnEmpId.Value, objSys.getDateForInput(txtApplyDate.Text.ToString()).ToString());
                if (dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        int cur_trans_id = 0;
                        string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                        string IsPriority = dt1.Rows[j]["Priority"].ToString();
                        if (EmpPermission == "1")
                        {  
                            cur_trans_id = objApproalEmp.InsertApprovalTransaciton("2", Session["CompId"].ToString(), "0", "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        }
                        else if (EmpPermission == "2")
                        {
                            cur_trans_id = objApproalEmp.InsertApprovalTransaciton("2", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        }
                        else if (EmpPermission == "3")
                        {
                            cur_trans_id = objApproalEmp.InsertApprovalTransaciton("2", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        }
                        else
                        {
                            cur_trans_id = objApproalEmp.InsertApprovalTransaciton("2", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        }
                        // Insert Notification For Leave by  ghanshyam suthar
                        Session["PriorityEmpId"] = PriorityEmpId;
                        Session["cur_trans_id"] = cur_trans_id;
                        Set_Notification();
                        //--------------------------------------
                        //for Email Code
                        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                        {
                            if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("2").Rows[0]["Approval_Type"].ToString() == "Priority") || (dt1.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("2").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                            {
                                string strPendingApproval = string.Empty;
                                if (PriorityEmpId != "" && PriorityEmpId != "0")
                                {
                                    DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), PriorityEmpId);
                                    if (dtEmpDetail.Rows.Count > 0)
                                    {
                                        if (ObjApproval.GetApprovalMasterById("2").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dt1.Rows.Count > 1)
                                        {
                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dt1.Rows[1]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                        }
                                        string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Half Day leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Half Day (" + HalfDayType + ")<br />Leave Date : " + txtApplyDate.Text + "<br /> Reason For Leave : " + txtDescription.Text + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
                                        //string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>Request for Half Day Leave '" + HalfDayType + "' By '" + Common.GetEmployeeName(hdnEmpId.Value) + "'<br />On Date '" + txtApplyDate.Text + "'<br /> and Given Reason is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                        ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Half Day Leave Apply By'" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "","", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                    }
                                }
                            }
                        }
                        //objApproalEmp.InsertApprovalTransaciton("HalfDay", b.ToString(), "211", dt1.Rows[j]["Emp_Id"].ToString(), "Pending", dt1.Rows[j]["Approval_Id"].ToString());
                    }
                }
                DisplayMessage("Half Day leave submitted");
                btnReset_Click(null, null);
            }
        }
        else
        {
            DataTable dt1 = new DataTable();
            string EmpPermission = string.Empty;
            EmpPermission = objSys.Get_Approval_Parameter_By_Name("HalfDay").Rows[0]["Approval_Level"].ToString();
            dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "211", hdnEmpId.Value);
            if (dt1.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }
            b = objHalfDay.UpdateHalfDayRequest(hdnEdit.Value, Session["CompId"].ToString(), hdnEmpId.Value, HalfDayType,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            if (b != 0)
            {
                Session["PriorityEmpId"] = hdnEmpId.Value;
                Session["cur_trans_id"] = hdnEdit.Value;
                Set_Notification();
                DisplayMessage("Record Updated", "green");
                btnCancel_Click(null, null);
                FillLeaveStatus();
                hdnEdit.Value = "";
                Lbl_Tab_New.Text = Resources.Attendance.New;
                txtEmpName.Enabled = true;
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
        }
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
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/attendance"));
        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, hdnEmpId.Value, Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        GetEmployeeName(hdnEmpId.Value);
        string Message = string.Empty;
        Message = txtEmpName.Text.Trim() + " applied Half Day Request for " + txtApplyDate.Text + "";
        if (hdnEdit.Value == "")
        {
            // For Insert        
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", "0");
        }
        else
        {
            // For Update
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["cur_trans_id"].ToString(), "15");
        }
    }
    private void ClearFormFields()
    {
        txtApplyDate.Text = "";
        txtDescription.Text = "";
        rbtnHalfDayMorning.Checked = true;
        rbtnHalfDayEvening.Checked = false;
        txtInTime.Text = "";
        txtOuttime.Text = "";
        return;
    }
    protected void rbtnHalfDayMorning_OnCheckedChanged(object sender, EventArgs e)
    {
        if (txtApplyDate.Text.Trim() == "")
        {
            DisplayMessage("Enter Apply Date");
            txtApplyDate.Focus();
            rbtnHalfDayEvening.Checked = false;
            rbtnHalfDayMorning.Checked = false;
            panel_inout_time.Visible = false;
            txtApplyDate.Text = "";
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtApplyDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Apply Date");
                txtApplyDate.Focus();
                rbtnHalfDayEvening.Checked = false;
                rbtnHalfDayMorning.Checked = false;
                panel_inout_time.Visible = false;
                txtApplyDate.Text = "";
                return;
            }
        }
        DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + objSys.getDateForInput(txtApplyDate.Text) + "' and Emp_Id='" + hdnEmpId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtHoliday1.Rows.Count > 0)
        {
            DisplayMessage("Employee has Holiday on Date " + txtApplyDate.Text + " so cannot apply");
            rbtnHalfDayEvening.Checked = false;
            rbtnHalfDayMorning.Checked = false;
            panel_inout_time.Visible = false;
            return;
        }
        // Holiday Code over ........................................
        //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
        DataTable dtSch = objEmpSch.GetSheduleDescription(hdnEmpId.Value);
        // Modified By Nitin jain, Date 09-07-2014 Check If Week Off Or Not For Leave Apply Date....
        DataTable dtSch1 = new DataView(dtSch, "Att_Date='" + objSys.getDateForInput(txtApplyDate.Text) + "' and Emp_Id='" + hdnEmpId.Value + "' and Is_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtSch1.Rows.Count > 0)
        {
            DisplayMessage("Employee has Week off on Date " + txtApplyDate.Text + " so cannot apply");
            rbtnHalfDayEvening.Checked = false;
            rbtnHalfDayMorning.Checked = false;
            panel_inout_time.Visible = false;
            return;
        }
        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(hdnEmpId.Value);
        dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date='" + objSys.getDateForInput(txtApplyDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
        txtInTime.Text = "";
        txtOuttime.Text = "";
        if (dtShiftAllDate.Rows.Count > 0)
        {
            try
            {
                txtInTime.Text = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString()).ToString("HH:mm");
                txtOuttime.Text = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OFFDuty_Time"].ToString()).ToString("HH:mm");
            }
            catch
            {
                txtInTime.Text = "00:00";
                txtOuttime.Text = "00:00";
            }
            panel_inout_time.Visible = true;
            int minute = GetMinuteDiff(txtOuttime.Text, txtInTime.Text);
            minute = minute / 2;
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString());
            DateTime da1 = dt.AddMinutes(minute);
            if (rbtnHalfDayEvening.Checked == true)
            {
                da1 = da1.AddMinutes(1);
            }
            panel_inout_time.Visible = true;
            if (rbtnHalfDayMorning.Checked == true)
            {
                txtOuttime.Text = da1.ToString("HH:mm");
            }
            if (rbtnHalfDayEvening.Checked == true)
            {
                txtInTime.Text = da1.ToString("HH:mm");
            }
        }
        dtShiftAllDate = null;
        dtHoliday2 = null;
        dtHoliday1 = null;
        dtSch1 = null;
        dtSch = null;
    }
    private string DateFormat(string Date)
    {
        string SystemDate = string.Empty;
        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
        }
        catch
        {
            SystemDate = Date;
        }
        return SystemDate;
    }
    public void SaveHalfDayLeave(string EmpId, string AssignLeave)
    {
        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }
        double TotalDays = 0;
        int TotalHalfDay = 0;
        double HalfDayPerMonth = 0;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

       
        if ( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month >= FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            TotalDays = (FinancialYearEndDate.Subtract( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()))).Days;
            double Months1 = TotalDays / 30;
            Months1 = System.Math.Round(Months1);
            HalfDayPerMonth = double.Parse(AssignLeave) / 12;
            Months1 = HalfDayPerMonth * Months1;
            Months1 = System.Math.Round(Months1);
            TotalHalfDay = int.Parse(Months1.ToString());
            objEmpHalfDay.DeleteEmployeeHalfDayTransByEmpIdYear(EmpId,  Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString());
            objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId,  Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString(), TotalHalfDay.ToString(), "0", TotalHalfDay.ToString(), "0", "", "", "", "", "", true.ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        }
        else
        {
            FinancialYearStartDate = new DateTime( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            TotalDays = FinancialYearEndDate.Subtract( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString())).Days;
            double Months1 = TotalDays / 30;
            Months1 = System.Math.Round(Months1);
            HalfDayPerMonth = double.Parse(AssignLeave) / 12;
            Months1 = HalfDayPerMonth * Months1;
            Months1 = System.Math.Round(Months1);
            TotalHalfDay = int.Parse(Months1.ToString());
            objEmpHalfDay.DeleteEmployeeHalfDayTransByEmpIdYear(EmpId, ( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1).ToString());
            objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId, ( Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1).ToString(), TotalHalfDay.ToString(), "0", TotalHalfDay.ToString(), "0", "", "", "", "", "", true.ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        }
        dt = null;
    }
    public int getCurrentMonthLeaveCount(DateTime applydate)
    {
        int Count = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), hdnEmpId.Value, applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and HalfDay_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Count++;
            }
        }
        else
        {
            Count = 0;
        }
        dt = null;
        return Count;
    }
    public int getCurrentMonth(DateTime applydate)
    {
        int useminutes = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), hdnEmpId.Value, applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and HalfDay_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());
            }
        }
        else
        {
            useminutes = 0;
        }
        dt = null;
        return useminutes;
    }
    public int getMinuteInADay(DateTime applydate)
    {
        int useminutes = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
        dt = new DataView(dt, "Is_Confirmed='Approved' and HalfDay_Type='0' and HalfDay_Date='" + applydate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());
            }
        }
        else
        {
            useminutes = 0;
        }
        dt = null;
        return useminutes;
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }
    public string leavetype(string type)
    {
        string t = string.Empty;
        if (type == "0")
        {
            t = "Personal";
        }
        else
        {
            t = "Official";
        }
        return t;
    }

    #region locationFilter

    protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }
    protected void ddlLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValueDate.Text = "";

        txtValue.Text = "";
        if (ddlFieldName.SelectedItem.Text == "HalfDay_Date")
        {
            txtValueDate.Visible = true;

            txtValue.Visible = false;
            ddlOption.Enabled = false;
            ddlOption.SelectedIndex = 1;
        }
        else
        {
            ddlOption.Enabled = true;
            txtValueDate.Visible = false;

            txtValue.Visible = true;
        }
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
            DataTable dtCust = (DataTable)Session["HR1_Half_DtLeaveStatus"];
            DataView view = new DataView();

            if (ddlFieldName.SelectedValue.Trim() == "HalfDay_Date")
            {
                view = new DataView(dtCust, "HalfDay_Date='" + Convert.ToDateTime(txtValueDate.Text).ToString() + "'", "", DataViewRowState.CurrentRows);
            }
            else
            {

                view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            }


            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, view.ToTable(), "", "");
            txtValue.Focus();

        }

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        ddlOption.Enabled = true;
    }

    #endregion



    public void FillLeaveStatus()
    {
        DataTable dtLeave = new DataTable();
        try
        {
            dtLeave = objHalfDay.GetHalfDayRequest(Session["CompId"].ToString(), ddlYearList.SelectedValue);
        }
        catch
        {
            dtLeave = objHalfDay.GetHalfDayRequest(Session["CompId"].ToString());
        }
        if (ddlLeaveStatus.SelectedIndex == 0)
        {
            dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id in (" + ddlLoc.SelectedValue.Trim() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtLeave = new DataView(dtLeave, "Is_Confirmed='" + ddlLeaveStatus.SelectedValue.Trim() + "' and Location_Id in (" + ddlLoc.SelectedValue.Trim() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (Request.QueryString["Emp_Id"] != null)
        {
            dtLeave = new DataView(dtLeave, "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dtLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, dtLeave, "", "");
            foreach (GridViewRow gvr in gvLeaveStatus.Rows)
            {
                Label lblFileName = (Label)gvr.FindControl("lblFileDownloadList");
                Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathList");
                LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadList");
                Button UploadFile = (Button)gvr.FindControl("ChoosefileUpload");

                if (lblFileName.Text == "" && lblFilePath.Text == "")
                {
                    lnkDownload.Visible = false;
                    UploadFile.Visible = true;
                }
                else
                {
                    UploadFile.Visible = true;
                    lnkDownload.Visible = true;
                }
            }
            Session["HR1_Half_DtLeaveStatus"] = dtLeave;
        }
        else
        {
            gvLeaveStatus.DataSource = null;
            gvLeaveStatus.DataBind();
            Session["HR1_Half_DtLeaveStatus"] = null;
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtLeave.Rows.Count.ToString() + "";
        dtLeave = null;
    }
    protected void gvLeaveStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveStatus.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["HR1_Half_DtLeaveStatus"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");
        foreach (GridViewRow gvr in gvLeaveStatus.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownloadList");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathList");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadList");
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                lnkDownload.Visible = false;
            }
            else
            {
                lnkDownload.Visible = true;
            }
        }
        dt = null;
    }
    protected void btnApprove_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objHalfDay.HalfDayApproveReject(e.CommandArgument.ToString(), "Approved", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        int RemainHalfDay = 0;
        int PendingHalfDay = 0;
        int UsedDay = 0;
        string year = string.Empty;
        int month = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            month = Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Month;
        }
        DataTable dtApp = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dtApp.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dtApp.Rows[0]["Param_Value"].ToString());
        }
        if (month >= FinancialYearMonth)
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year).ToString();
        }
        else
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year - 1).ToString();
        }
        DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(e.CommandName.ToString(), year);
        if (dtEmpHalf.Rows.Count > 0)
        {
            UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
            PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
            RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
            PendingHalfDay = PendingHalfDay - 1;
            UsedDay = UsedDay + 1;
        }
        objEmpHalfDay.UpdateEmployeeHalfDayTransaction(Session["CompId"].ToString(), e.CommandName.ToString(), dtEmpHalf.Rows[0]["Year"].ToString(), "0", UsedDay.ToString(), RemainHalfDay.ToString(), PendingHalfDay.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        if (b != 0)
        {
            DisplayMessage("Half Day Leave Approved");
            FillLeaveStatus();
        }
        dt = null;
        dtEmpHalf = null;
        dtApp = null;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Common cmn = new Common(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            txtEmpName.Enabled = false;
            hdnEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
            hdnEdit.Value = e.CommandArgument.ToString();
            txtEmpName.Text = cmn.GetEmpName(hdnEmpId.Value, HttpContext.Current.Session["CompId"].ToString());
            DataTable dtEmployee = Common.GetEmployee(txtEmpName.Text, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Session["DBConnection"].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                txtEmpName.Text = "" + dtEmployee.Rows[0][1].ToString() + "/(" + dtEmployee.Rows[0][2].ToString() + ")/" + dtEmployee.Rows[0][0].ToString() + "";
            }
            FillEmpLeave(hdnEmpId.Value);
            FillHalfDaySummary(hdnEmpId.Value);
            if (dt.Rows[0]["HalfDay_Type"].ToString() == "Morning")
            {
                rbtnHalfDayMorning.Checked = true;
                rbtnHalfDayEvening.Checked = false;
            }
            else if (dt.Rows[0]["HalfDay_Type"].ToString() == "Evening")
            {
                rbtnHalfDayEvening.Checked = true;
                rbtnHalfDayMorning.Checked = false;
            }
            txtApplyDate.Text = Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).ToString(objSys.SetDateFormat());
            txtDescription.Text = dt.Rows[0]["Description"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
            dtEmployee = null;
        }
        dt = null;
    }
    protected void IbtnReject_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objHalfDay.HalfDayApproveReject(e.CommandArgument.ToString(), "Canceled", true.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        int RemainHalfDay = 0;
        int PendingHalfDay = 0;
        int UsedDay = 0;
        string year = string.Empty;
        int month = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            month = Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Month;
        }
        DataTable dtApp = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dtApp.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dtApp.Rows[0]["Param_Value"].ToString());
        }
        if (month >= FinancialYearMonth)
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year).ToString();
        }
        else
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year - 1).ToString();
        }
        DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(e.CommandName.ToString(), year);
        if (dtEmpHalf.Rows.Count > 0)
        {
            RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
            PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
            PendingHalfDay = PendingHalfDay - 1;
            RemainHalfDay = RemainHalfDay + 1;
            UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
        }
        objEmpHalfDay.UpdateEmployeeHalfDayTransaction(Session["CompId"].ToString(), e.CommandName.ToString(), dtEmpHalf.Rows[0]["Year"].ToString(), "0", UsedDay.ToString(), RemainHalfDay.ToString(), PendingHalfDay.ToString(), Session["UserId"].ToString(),Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        if (b != 0)
        {
            DisplayMessage("Half Day Leave Rejected");
            FillLeaveStatus();
        }
        dt = null;
        dtEmpHalf = null;
        dtApp = null;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    private void Reset()
    {
        Session["empimgpath"] = "";
        Session["empimgpathFull"] = "";
        txtApplyDate.Text = "";
        txtDescription.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        if (Request.QueryString["Emp_Id"] == null)
        {
            txtEmpName.Text = "";
            hdnEdit.Value = "";
            gvLeaveSummary.DataSource = null;
            gvLeaveSummary.DataBind();
            gvEmpPendingLeave.DataSource = null;
            gvEmpPendingLeave.DataBind();
            ViewState["dtEmpLeave"] = null;
            txtEmpName.Enabled = true;
        }
        else
        {
            FillEmpLeave(Session["EmpId"].ToString());
            FillHalfDaySummary(Session["EmpId"].ToString());
        }
        rbtnHalfDayEvening.Checked = false;
        rbtnHalfDayMorning.Checked = false;
        panel_inout_time.Visible = false;
        txtInTime.Text = "";
        txtOuttime.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        rbtnHalfDayMorning.Checked = false;
        rbtnHalfDayEvening.Checked = false;
        txtApplyDate.Text = "";
        txtDescription.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        hdnEdit.Value = "";
        if (Request.QueryString["Emp_Id"] == null)
        {
            txtEmpName.Text = "";
            txtEmpName.Enabled = true;
            gvLeaveSummary.DataSource = null;
            gvLeaveSummary.DataBind();
            gvEmpPendingLeave.DataSource = null;
            gvEmpPendingLeave.DataBind();
            ViewState["dtEmpLeave"] = null;
        }
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        dt = null;
        return str;
    }
    public void FillEmpLeave(string EmpId)
    {
        DataTable dtLeave = objHalfDay.GetHalfDayRequest(Session["CompId"].ToString());
        dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Emp_Id=" + EmpId + "", "", DataViewRowState.CurrentRows).ToTable();
        dtLeave = new DataView(dtLeave, "Is_Confirmed='Pending' or Is_Confirmed='Approved'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpPendingLeave, dtLeave, "", "");
        ViewState["dtEmpLeave"] = dtLeave;
        dtLeave = null;
    }
    protected void gvEmpPendingLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmpPendingLeave.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtEmpLeave"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpPendingLeave, dt, "", "");
        dt = null;
    }

    public void halfDayReport()
    {
        if (hdnReportId.Value == "")
        {
            return;
        }

        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_HalfDay_Request_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_HalfDay_Request_ReportTableAdapter();
        adp.Fill(rptdata.sp_Att_HalfDay_Request_Report, Convert.ToInt32(hdnPrintTransId.Value), Convert.ToInt32(hdnLocationId.Value), Convert.ToInt32(hdnEmpidVal.Value));
        dtFilter = rptdata.sp_Att_HalfDay_Request_Report;

        objHalfDayRequestReport.DataSource = dtFilter;
        objHalfDayRequestReport.DataMember = "sp_Att_HalfDay_Request_Report";
        objHalfDayRequestReport.CreateDocument();
        ReportViewer1.OpenReport(objHalfDayRequestReport);
        dtFilter.Dispose();
    }

    protected void btnPrint_Command(object sender, CommandEventArgs e)
    {
        hdnReportId.Value = e.CommandName.ToString();
        hdnPrintTransId.Value = e.CommandArgument.ToString();
        GridViewRow Gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        hdnLocationId.Value = ((Label)Gvrow.FindControl("lblLocationId")).Text;
        hdnEmpidVal.Value = ((Label)Gvrow.FindControl("lblEmpId1")).Text;
        halfDayReport();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openReport()", true);
    }
    public void setYearList()
    {
        DataTable dt = objEmpHalfDay.getYearList();
        ddlYearList.DataSource = dt;
        ddlYearList.DataTextField = "year";
        ddlYearList.DataValueField = "year";
        ddlYearList.DataBind();
    }

    protected void ddlYearList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }
}