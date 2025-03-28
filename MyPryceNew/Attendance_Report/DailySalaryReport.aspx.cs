using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using PegasusDataAccess;
using System.IO;
using DevExpress.XtraReports.UI;

public partial class Attendance_Report_DailySalaryReportNew : BasePage
{
    //XtraReport RptShift = new XtraReport();
    Attendance objAttendance = null;
    Att_DailySalaryReport1 RptShift = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    SystemParameter objSys = null;
    Att_Leave_Request objLeaveReq = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_PartialLeave_Request objPartial = null;
    Att_Employee_Notification objEmpNotice = null;
    DataAccessClass ObjDa = null;
    LeaveMaster_deduction objLeavededuction = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_DailySalaryReport1.repx");
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        RptShift = new Att_DailySalaryReport1(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objLeaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "112", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
        GetReport();
        //New Code 

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("112", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
    }

    protected void DocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }

    protected void DocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }


    public void GetReport()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        DateTime FromDate1 = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        bool LateMinCountAsHalfDay = true;
        if (Session["EmpList"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
            ToDate = objSys.getDateForInput(Session["ToDate"].ToString());
            FromDate1 = FromDate;
            EmpReport = Session["EmpList"].ToString();
            //code update on 27-09-2014
            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "16");
            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            //code end
            DataTable dtFilter = new DataTable();
            AttendanceDataSet rptdata = new AttendanceDataSet();
            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist, 6);
            }
            catch
            {

            }

            dtFilter = rptdata.sp_Att_AttendanceRegister_Report;
            DataTable dtEmp = new DataTable();
            if (dtFilter.Rows.Count > 0)
            {
                dtEmp = dtFilter.DefaultView.ToTable(true, "Emp_Id");
            }

            DataTable dtTemp = new DataTable();
            DataTable dtSalary = new DataTable();
            dtSalary = dtFilter.Clone();
            int Total_Days = 0;
            int Days_In_WorkMin = 0;
            int effectiveworkminute = 0;
            int Worked_Days = 0;
            int Week_Off_Days = 0;
            int Holiday_Days = 0;
            int Leave_Days = 0;
            int Absent_Days = 0;
            int Assigned_Worked_Min = 0;
            int WeekOffCount = 0;
            string Is_Absent = string.Empty;
            bool Is_AbsentPenalty;

            double Basic_Salary = 0;
            double Basic_Min_Salary = 0;
            double Normal_OT_Salary = 0;
            double Week_Off_OT_Salary = 0;
            double Holiday_OT_Salary = 0;
            double Absent_Penalty = 0;
            double Late_Penalty_Min = 0;
            double Early_Penalty_Min = 0;
            double Partial_Penalty_Min = 0;
            int Total_Worked_Min = 0;
            int Holiday_OT_Min = 0;
            int Week_Off_OT_Min = 0;
            int Normal_OT_Min = 0;
            int Late_Min = 0;
            int Early_Min = 0;
            int Partial_Min = 0;
            double Basic_Work_Salary = 0;
            double Normal_OT_Work_Salary = 0;
            double WeekOff_OT_Work_Salary = 0;
            double Holiday_OT_Work_Salary = 0;
            double Week_Off_Days_Salary = 0;
            double Holiday_Days_Salary = 0;
            double Leave_Days_Salary = 0;
            double Absent_Day_Penalty = 0;
            double Late_Min_Penalty = 0;
            double Early_Min_Penalty = 0;
            double Parital_Violation_Penalty = 0;

            bool IsLeaveSalary = false;
            bool IsEmpLate = false;
            bool IsEmpEarly = false;
            bool IsEmpPartial = false;
            bool Is_EmpOverTime = false;
            int PartialMin = 0;
            int TotalOTMin = 0;
            string WeekOffOTEnabled = string.Empty;
            string HolidayOTEnabled = string.Empty;
            string IsWeekOff = string.Empty;
            string IsHoliday = string.Empty;
            double TotalGrossSalary = 0;
            string Partial_PenaltyMethode = string.Empty;
            double Basic_Min_Salary_Penalty_Ot = 0;
            double Total_Worked_Min_Penalty_Ot = 0;
            string WorkCalMethod = string.Empty;
            string AbsentType = string.Empty;

            string WeekOff = string.Empty;
            string ExcludeWeekOffInSalary = string.Empty;

            bool IsrelaxationMinute_In_Penalty = false;
            bool IsbreakMinute_In_Penalty = false;
            bool IsworkMinute_In_Penalty = false;
            string strPaysalaryMethod = objAppParam.GetApplicationParameterValueByParamName("Pay Salary Acc To Work Hour or Ref Hour", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            IsrelaxationMinute_In_Penalty = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsrelaxationMinute_In_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            IsbreakMinute_In_Penalty = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsbreakMinute_In_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            IsworkMinute_In_Penalty = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsworkMinute_In_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));


            //

            string SalaryCalculationMethode = string.Empty;
            int DaysInMonth = 0;
            int j = 0;





            SalaryCalculationMethode = (objAppParam.GetApplicationParameterValueByParamName("Salary Calculate According To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            // Nitin Jain On 05/01/2015 
            LateMinCountAsHalfDay = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("LateInCountAsHalfDay", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            DaysInMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Days In Month", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            WeekOffOTEnabled = objAppParam.GetApplicationParameterValueByParamName("WeekOffOTEnable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            // For OverTIme 
            HolidayOTEnabled = objAppParam.GetApplicationParameterValueByParamName("HolidayOTEnable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            AbsentType = objAppParam.GetApplicationParameterValueByParamName("Absent_Type", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            WeekOff = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            ExcludeWeekOffInSalary = objAppParam.GetApplicationParameterValueByParamName("ExcludeWeekOffInSalary", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //ExcludeWeekOffInSalary = "True";
            
            IsLeaveSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            
            //New Weekoff Count Concept Added on 27-05-2023
            WeekOffCount = 0;
            if (ExcludeWeekOffInSalary == "True")
            {
                WeekOff = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                foreach (string str in WeekOff.Split(','))
                {

                    while (FromDate <= ToDate)
                    {
                        if (str == FromDate.DayOfWeek.ToString())
                        {
                            IsWeekOff = true.ToString();

                            if (WeekOffCount == 0)
                            {
                                WeekOffCount = 1;
                            }
                            else
                            {
                                WeekOffCount = WeekOffCount + 1;
                            }
                        }
                        FromDate = FromDate.AddDays(1);
                        continue;
                    }
                }
            }





            for (int i = 0; i < dtEmp.Rows.Count; i++)
            {
                FromDate = FromDate1;
                IsEmpLate = false;
                IsEmpEarly = false;
                IsEmpPartial = false;
                Is_AbsentPenalty = false;
                TotalOTMin = 0;
                Assigned_Worked_Min = 0;
                Basic_Salary = 0;
                Basic_Min_Salary = 0;
                Basic_Min_Salary_Penalty_Ot = 0;
                Normal_OT_Salary = 0;
                Week_Off_OT_Salary = 0;
                Holiday_OT_Salary = 0;
                Absent_Penalty = 0;
                Late_Penalty_Min = 0;
                Early_Penalty_Min = 0;
                Partial_Penalty_Min = 0;
                TotalGrossSalary = 0;
                try
                {
                    IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field2"));
                    IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field1"));
                    IsEmpPartial = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field13"));
                    Is_AbsentPenalty = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field3"));
                    Is_EmpOverTime = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Is_OverTime"));
                    // Modified By Nitin Jain

                    // Modified by kunal update Totaldays according to Month days date 7 Jan 2014
                    DateTime FDate = new DateTime(FromDate.Year, FromDate.Month, 1);

                    DateTime NDate = FDate.AddMonths(1);
                    NDate = new DateTime(NDate.Year, NDate.Month, 1);

                    DateTime TDate = NDate.AddDays(-1);

                    if (SalaryCalculationMethode == "Fixed Days")
                    {
                        Total_Days = DaysInMonth;
                    }
                    else
                    {
                        Total_Days = TDate.Subtract(FDate).Days + 1;
                    }

                    Total_Days = Total_Days - WeekOffCount;


                    Days_In_WorkMin = int.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Assign_Min"));
                }
                catch
                {

                }

                try
                {
                    Basic_Salary = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Basic_Salary"));
                }
                catch
                {

                }

                int assingmin = 0;
                try
                {
                    assingmin = int.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Assign_Min"));
                }
                catch
                {

                }

                WorkCalMethod = objAttendance.GetWorkCalculationMethod(dtEmp.Rows[i]["Emp_Id"].ToString(), Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


                //if (Basic_Min_Salary.ToString() == "NaN")
                //{
                //    Basic_Min_Salary = 0;
                //}
                while (FromDate <= ToDate)
                {
                    PartialMin = 0;
                    Total_Worked_Min = 0;
                    Holiday_OT_Min = 0;
                    Week_Off_OT_Min = 0;
                    Normal_OT_Min = 0;
                    Late_Min = 0;
                    Early_Min = 0;
                    Partial_Min = 0;
                    Basic_Work_Salary = 0;
                    Normal_OT_Work_Salary = 0;
                    WeekOff_OT_Work_Salary = 0;
                    Holiday_OT_Work_Salary = 0;
                    Week_Off_Days_Salary = 0;
                    Holiday_Days_Salary = 0;
                    Leave_Days_Salary = 0;
                    // Absent_Day_Penalty = 0;
                    Late_Min_Penalty = 0;
                    Early_Min_Penalty = 0;
                    Parital_Violation_Penalty = 0;
                    PartialMin = 0;
                    Basic_Min_Salary_Penalty_Ot = 0;


                    dtTemp = dtFilter.Clone();
                    dtTemp = new DataView(dtFilter, "Att_Date='" + FromDate.ToString("dd-MMM-yyyy") + "' and Emp_Id='" + dtEmp.Rows[i]["Emp_Id"].ToString() + "'   ", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtTemp.Rows.Count <= 0)
                    {
                        FromDate = FromDate.AddDays(1);
                        continue;

                    }

                    int ApprovedButPenalty = 0;
                    if (dtTemp.Rows[0]["Shift_Id"].ToString() != "0" || dtTemp.Rows[0]["TimeTable_Id"].ToString() != "0")
                    {

                        if (dtTemp.Rows.Count > 0)
                        {

                            if (dtTemp.Rows.Count > 0)
                            {
                                for (int s = 0; s < dtTemp.Rows.Count; s++)
                                {



                                    ApprovedButPenalty += Convert.ToInt16(dtTemp.Rows[s]["Partial_Min"].ToString());
                                    Total_Worked_Min += Convert.ToInt32(dtTemp.Rows[s]["TotalAssign_Min"].ToString());
                                    //updated by jitendra on 28-08-2019
                                    //updated part = Basic_Salary/ dtTemp.Rows.Count


                                    Basic_Min_Salary_Penalty_Ot += Attendance.GetPerMinuteSalary_For_OT_Penalty(Total_Worked_Min, Basic_Salary / dtTemp.Rows.Count, Total_Days, dtTemp.Rows[s]["TimeTable_Id"].ToString(), IsrelaxationMinute_In_Penalty, IsbreakMinute_In_Penalty, IsworkMinute_In_Penalty, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                }
                            }
                            else
                            {
                                Total_Worked_Min = Convert.ToInt32(dtTemp.Rows[0]["TotalAssign_Min"].ToString());
                            }


                            if (Total_Worked_Min <= 0)
                            {
                                Total_Worked_Min = assingmin;
                            }

                            //if ((Total_Days * Total_Worked_Min) > 0)
                            //{
                            Basic_Min_Salary = Basic_Salary / (Total_Days * Total_Worked_Min);


                            //}
                            //else
                            //{
                            //    Basic_Min_Salary = Basic_Salary / 1;

                            //}


                            //if (Basic_Min_Salary.ToString() == "NaN")
                            //{
                            //    Basic_Min_Salary = 0;
                            //}

                            Is_Absent = dtTemp.Rows[0]["Is_Absent"].ToString();



                            for (int k = 0; k < dtTemp.Rows.Count; k++)
                            {

                                // updated by jitendra on 28 - 08 - 2019
                                //updated part = Basic_Salary/ dtTemp.Rows.Count
                                Basic_Min_Salary_Penalty_Ot = Attendance.GetPerMinuteSalary_For_OT_Penalty(Convert.ToInt32(dtTemp.Rows[k]["TotalAssign_Min"].ToString()), Basic_Salary / dtTemp.Rows.Count, Total_Days, dtTemp.Rows[k]["TimeTable_Id"].ToString(), IsrelaxationMinute_In_Penalty, IsbreakMinute_In_Penalty, IsworkMinute_In_Penalty, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                                dtSalary.ImportRow(dtTemp.Rows[k]);

                                dtSalary.Rows[j]["Field2"] = "";
                                dtSalary.Rows[j]["Field3"] = "";
                                dtSalary.Rows[j]["Field4"] = "";

                                //IsEmpLate = Convert.ToBoolean(dtTemp.Rows[k]["IsLate"].ToString());
                                //IsEmpEarly = Convert.ToBoolean(dtTemp.Rows[k]["IsEarlyout"].ToString());



                                PartialMin = int.Parse(dtTemp.Rows[k]["Partial_Violation_Min"].ToString());

                                //Modified By Nitin Jain
                                DataTable DtPartial = objPartial.GetPartialLeaveRequestById(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString());


                                if (DtPartial == null)
                                {
                                    Partial_Penalty_Min = 0;
                                    Parital_Violation_Penalty = 0;
                                }
                                else
                                {
                                    DtPartial = new DataView(DtPartial, "Partial_Leave_Date='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (DtPartial.Rows.Count > 0)
                                    {
                                        if (PartialMin > 0)
                                        {
                                            // Nitin Jain Late Min Salary Calculation here 
                                            PartialMin = int.Parse(dtTemp.Rows[k]["Partial_Violation_Min"].ToString());
                                            if (IsEmpPartial == true)
                                            {
                                                Parital_Violation_Penalty = objAttendance.ParialPenalty(Session["CompId"].ToString(), Basic_Min_Salary_Penalty_Ot, dtEmp.Rows[i]["Emp_Id"].ToString(), 1, PartialMin, "DailySalary", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                            }
                                            else
                                            {

                                                //updated by jitendra upadhyay because partial violation penalty was coming when partial penalty is disable
                                                //updated on 13/12/2017

                                                //Parital_Violation_Penalty = PartialMin * Basic_Min_Salary;
                                                Parital_Violation_Penalty = 0;
                                            }
                                        }
                                        else
                                        {
                                            Partial_Penalty_Min = 0;
                                        }

                                    }
                                    else
                                    {
                                        Parital_Violation_Penalty = 0;
                                    }
                                }

                                Total_Worked_Min = int.Parse(dtTemp.Rows[k]["TotalAssign_Min"].ToString());
                                if (dtTemp.Rows[k]["Field1"].ToString() == "")
                                {
                                    dtTemp.Rows[k]["Field1"] = "0";
                                }
                                if (dtTemp.Rows[k]["Early_Relaxation_Min"].ToString() == "")
                                {
                                    dtTemp.Rows[k]["Early_Relaxation_Min"] = "0";
                                }
                                if (IsEmpLate == false && IsEmpEarly == false)
                                {
                                    if (IsWeekOff == false.ToString() && IsHoliday == false.ToString())
                                    {
                                        Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());
                                    }
                                    else
                                    {
                                        Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Late_Relaxation_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Early_Relaxation_Min"].ToString());
                                    }
                                }
                                else if (IsEmpLate == false && IsEmpEarly == true)
                                {
                                    Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Early_Relaxation_Min"].ToString());
                                }
                                else if (IsEmpLate == true && IsEmpEarly == false)
                                {
                                    Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Late_Relaxation_Min"].ToString());
                                }
                                else
                                {
                                    if (int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) > 0)
                                    {
                                        Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Late_Relaxation_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Early_Relaxation_Min"].ToString());
                                    }
                                    else
                                    {
                                        Days_In_WorkMin = 0;
                                    }
                                }
                                IsWeekOff = dtTemp.Rows[k]["Is_Week_Off"].ToString();
                                // Nitin Jain Late Min Salary Calculation here 
                                Late_Min = int.Parse(dtTemp.Rows[k]["LateMin"].ToString());
                                // Nitin Jain If Count Late Min as Half Day or Not...
                                if (IsEmpLate == true)
                                {
                                    if (LateMinCountAsHalfDay == true)
                                    {
                                        if (Late_Min > 0)
                                        {
                                            Late_Min_Penalty = (double.Parse(dtTemp.Rows[k]["TotalAssign_Min"].ToString()) / 2) * Basic_Min_Salary_Penalty_Ot;
                                        }
                                    }
                                    else
                                    {
                                        Late_Min_Penalty = objAttendance.LatePenalty(Session["CompId"].ToString(), Basic_Min_Salary_Penalty_Ot, dtEmp.Rows[i]["Emp_Id"].ToString(), 1, Late_Min, "LogProcess", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                    }
                                    //Late_Min_Penalty = Late_Min * Late_Penalty_Min;
                                }
                                else
                                {
                                    Late_Min_Penalty = Late_Min * Basic_Min_Salary_Penalty_Ot;
                                }
                                // Nitin Jain Earlyt Min Salary Calculation here 
                                Early_Min = int.Parse(dtTemp.Rows[k]["EarlyMin"].ToString());
                                if (IsEmpEarly)
                                {
                                    Early_Min_Penalty = objAttendance.EarlyPenalty(Session["CompId"].ToString(), Basic_Min_Salary_Penalty_Ot, dtEmp.Rows[i]["Emp_Id"].ToString(), 1, Early_Min, "LogProcess", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                }
                                else
                                {
                                    Early_Min_Penalty = Basic_Min_Salary_Penalty_Ot * Early_Min;
                                }
                                // Absent Day Penalty 
                                if (Is_AbsentPenalty == true)
                                {
                                    Absent_Day_Penalty = objAttendance.AbsentDaysSalary(Session["CompId"].ToString(), Basic_Min_Salary, dtEmp.Rows[i]["Emp_Id"].ToString(), Total_Worked_Min, 1, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                }
                                else
                                {
                                    Absent_Day_Penalty = 0;
                                }
                                //Priya jain(16.04.2014)
                                //On 10-07-2015
                                if (WorkCalMethod == "PairWise")
                                {
                                    if (Is_Absent == "True")
                                    {
                                        Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                    }
                                    else
                                    {
                                        Absent_Day_Penalty = 0;

                                        if (Days_In_WorkMin.ToString() == "0")
                                        {
                                            Basic_Work_Salary = Basic_Min_Salary * Days_In_WorkMin;
                                        }
                                        else
                                        {
                                            Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                        }
                                        //updated on 24/11/2017

                                        //effectiveworkminute = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());


                                        //if (effectiveworkminute > Total_Worked_Min)
                                        //{
                                        //    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                        //}
                                        //else
                                        //{
                                        //    Basic_Work_Salary = Basic_Min_Salary * effectiveworkminute;
                                        //}

                                    }
                                }
                                else
                                {
                                    if (Is_Absent == "True")
                                    {
                                        Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                    }
                                    else
                                    {
                                        Absent_Day_Penalty = 0;
                                        //effectiveworkminute = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());

                                        //if (effectiveworkminute > Total_Worked_Min)
                                        //{
                                        //    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                        //}
                                        //else
                                        //{
                                        //    Basic_Work_Salary = Basic_Min_Salary * effectiveworkminute;
                                        //}


                                        if (Days_In_WorkMin.ToString() == "0")
                                        {
                                            Basic_Work_Salary = Basic_Min_Salary * Days_In_WorkMin;
                                        }
                                        else
                                        {
                                            Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                        }
                                    }
                                }
                                // Nitin Jain Normal OT Salary Calculation Here
                                Normal_OT_Min = int.Parse(dtTemp.Rows[k]["OverTime_Min"].ToString());
                                if (Normal_OT_Min > 0)
                                {
                                    Normal_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary_Penalty_Ot, Normal_OT_Min, "Normal");
                                }
                                else
                                {
                                    Normal_OT_Work_Salary = 0;
                                }
                                // Nitin Jain Week Off OT Salary Calculation Here 
                                Week_Off_OT_Min = int.Parse(dtTemp.Rows[k]["Week_Off_Min"].ToString());
                                if (Week_Off_OT_Min > 0)
                                {
                                    WeekOff_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary_Penalty_Ot, Week_Off_OT_Min, "WeekOff");
                                }
                                else
                                {
                                    WeekOff_OT_Work_Salary = 0;
                                }
                                // Nitin Jain Holiday OT Salary Calculation Here 
                                Holiday_OT_Min = int.Parse(dtTemp.Rows[k]["Holiday_Min"].ToString());
                                if (Holiday_OT_Min > 0)
                                {
                                    Holiday_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary_Penalty_Ot, Holiday_OT_Min, "Holiday");
                                }
                                else
                                {
                                    Holiday_OT_Work_Salary = 0;
                                }
                                double TotalSalary = 0;
                                TotalOTMin += int.Parse(dtTemp.Rows[k]["OverTime_Min"].ToString());
                                //code modified by jitendra upadhyay on 18-02-2017 according the mr. nawaz sir testing
                                //is leave condition i update in first if befire now it was after week off and holiday condition


                                double ApprovedButPenaltyD = 0.0d;
                                if (HttpContext.Current.Session["LocId"].ToString() == "8")
                                {
                                    ApprovedButPenaltyD = ApprovedButPenalty * Basic_Min_Salary;
                                }

                                if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Week_Off"].ToString()))
                                {
                                    dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), WeekOff_OT_Work_Salary.ToString()).ToString();
                                    TotalSalary = Total_Worked_Min * Basic_Min_Salary;
                                    dtSalary.Rows[j]["OverTime_Min"] = dtTemp.Rows[k]["Week_Off_Min"];
                                    TotalOTMin += int.Parse(dtTemp.Rows[k]["Week_Off_Min"].ToString());
                                }
                                else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Holiday"].ToString()))
                                {
                                    dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), Holiday_OT_Work_Salary.ToString()).ToString();
                                    TotalSalary = Total_Worked_Min * Basic_Min_Salary;
                                    dtSalary.Rows[j]["OverTime_Min"] = dtTemp.Rows[k]["Holiday_Min"];
                                    TotalOTMin += int.Parse(dtTemp.Rows[k]["Holiday_Min"].ToString());
                                }
                                else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Leave"].ToString()))
                                {

                                    if (Total_Worked_Min <= 0)
                                    {
                                        Total_Worked_Min = assingmin;
                                    }
                                    //Code Modified for paid leave 23 Dec 2013 Kunal

                                    //bool IsPaid = false;

                                    //IsPaid = objLeaveReq.IsPaidLeave(dtTemp.Rows[k]["Att_Date"].ToString(), dtTemp.Rows[k]["Emp_Id"].ToString());
                                    //if (IsPaid)
                                    //{
                                    //    TotalSalary = Total_Worked_Min * Basic_Min_Salary;
                                    //}
                                    //else
                                    //{
                                    //    TotalSalary = 0;
                                    //}

                                    string strLeaveTypeId = "0";
                                    DataTable dtLeaveRequest = new DataTable();


                                    bool IsLSalary = false;
                                    DataTable dtLeaveSalary = objLeaveReq.IsLeaveSalary(dtTemp.Rows[k]["Att_Date"].ToString(), dtTemp.Rows[k]["Emp_Id"].ToString());
                                    if (dtLeaveSalary.Rows.Count > 0)
                                    {
                                        IsLSalary = Convert.ToBoolean(dtLeaveSalary.Rows[0]["LeaveSalary"].ToString());
                                    }
                                    bool IsPaid = false;
                                    if (IsLeaveSalary)
                                    {
                                        try
                                        {
                                            strLeaveTypeId = ObjDa.return_DataTable("SELECT Att_Leave_Request_Child.LeaveType_Id FROM Att_Leave_Request_Child LEFT OUTER JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.IsActive = 'True') AND (Att_Leave_Request_Child.Leave_Date = '" + dtTemp.Rows[k]["Att_Date"].ToString() + "') and Att_Leave_Request.Emp_Id='" + dtTemp.Rows[k]["Emp_Id"].ToString() + "'").Rows[0]["LeaveType_Id"].ToString();
                                        }
                                        catch (Exception ex)
                                        {

                                        }


                                        // dtSalary.Rows[j]["Field5"] = System.Math.Round(TotalGrossSalary, 2).ToString();
                                        IsPaid = objLeaveReq.IsPaidLeave(dtTemp.Rows[k]["Att_Date"].ToString(), dtTemp.Rows[k]["Emp_Id"].ToString());
                                        if (IsPaid)
                                        {
                                            //Add On 06-06-2016 By Lokesh

                                            //if (!IsLSalary)
                                            //{
                                            TotalSalary = Basic_Salary / Total_Days;
                                            //}
                                            //else
                                            //{
                                            //TotalSalary = 0;
                                            //}
                                            //TotalSalary = Total_Worked_Min * Basic_Min_Salary;

                                        }
                                        else
                                        {
                                            TotalSalary = 0;
                                            //here we are deducting salary according sick leave  deduction slab

                                            if (GetEmployeeSickLeavededuction(dtTemp.Rows[k]["Emp_Id"].ToString(), dtTemp.Rows[k]["Att_Date"].ToString(), strLeaveTypeId) > 0)
                                            {
                                                TotalSalary = (Total_Worked_Min * Basic_Min_Salary) - ((Total_Worked_Min * Basic_Min_Salary) * (GetEmployeeSickLeavededuction(dtTemp.Rows[k]["Emp_Id"].ToString(), dtTemp.Rows[k]["Att_Date"].ToString(), strLeaveTypeId) / 100));
                                            }
                                            else
                                            {
                                                //if (!IsLSalary)
                                                //{
                                                //    TotalSalary = Basic_Salary / Total_Days;
                                                //}
                                                //else
                                                //{
                                                //    TotalSalary = 0;
                                                //}
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //TotalSalary = Total_Worked_Min * Basic_Min_Salary;

                                        //above line commented by jitendra because this is wrong code according company parameter

                                        TotalSalary = 0;
                                    }

                                    dtSalary.Rows[j]["Field4"] = "0";
                                }

                                //Updated By Nitin jain(05.05.2014) 
                                else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Absent"].ToString()))
                                {
                                    if (Is_AbsentPenalty == false)
                                    {
                                        TotalSalary = Basic_Work_Salary - Basic_Work_Salary;
                                    }
                                    else
                                    {
                                        TotalSalary = 0 - Absent_Day_Penalty;
                                    }
                                    dtSalary.Rows[j]["Field4"] = "0";

                                }
                                else
                                {
                                    dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), Normal_OT_Work_Salary.ToString()).ToString();


                                    if (strPaysalaryMethod.Trim() == "Work Calculation")
                                    {
                                        Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["LateMin"].ToString()) + int.Parse(dtTemp.Rows[k]["Late_Relaxation_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["EarlyMin"].ToString()) + int.Parse(dtTemp.Rows[k]["Early_Relaxation_Min"].ToString());

                                        if (Days_In_WorkMin > int.Parse(dtTemp.Rows[k]["TotalAssign_Min"].ToString()))
                                        {

                                            Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["TotalAssign_Min"].ToString());

                                        }

                                        Basic_Work_Salary = Days_In_WorkMin * Basic_Min_Salary;
                                    }
                                    TotalSalary = Basic_Work_Salary - Late_Min_Penalty - Early_Min_Penalty - Parital_Violation_Penalty - ApprovedButPenaltyD;
                                    //if(Basic_Work_Salary >Late_Min_Penalty &&  Basic_Work_Salary > Early_Min_Penalty &&  Basic_Work_Salary > Parital_Violation_Penalty &&  Basic_Work_Salary > ApprovedButPenaltyD)
                                    //{
                                    //    TotalSalary = Basic_Work_Salary - Late_Min_Penalty - Early_Min_Penalty - Parital_Violation_Penalty - ApprovedButPenaltyD;
                                    //}
                                    //else
                                    //{
                                    //    TotalSalary = 0;

                                    //}
                                }

                                dtSalary.Rows[j]["Field2"] = TotalOTMin.ToString();


                                dtSalary.Rows[j]["Field3"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), TotalSalary.ToString()).ToString();
                                try
                                {
                                    TotalGrossSalary = TotalGrossSalary + TotalSalary + double.Parse(dtSalary.Rows[j]["Field4"].ToString());
                                }
                                catch
                                {
                                    TotalGrossSalary = TotalGrossSalary + TotalSalary;
                                }

                                //Commented on 21-08-2023
                                //try
                                //{
                                //    TotalGrossSalary = TotalGrossSalary + TotalSalary + double.Parse(dtSalary.Rows[j]["Field4"].ToString());
                                //}
                                //catch
                                //{
                                //    TotalGrossSalary = TotalGrossSalary + TotalSalary;
                                //}
                                
                                dtSalary.Rows[j]["Field5"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), TotalGrossSalary.ToString()).ToString();
                                j++;
                            }

                        }
                    }
                    // Done By Nitin Jain Calculate Salary For Without Shoft
                    else
                    {
                        if (dtTemp.Rows.Count > 0)
                        {
                            for (int s = 0; s < dtTemp.Rows.Count; s++)
                            {
                                Total_Worked_Min += Convert.ToInt32(dtTemp.Rows[s]["TotalAssign_Min"].ToString());
                            }
                        }
                        else
                        {
                            Total_Worked_Min = int.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Assign_Min"));
                        }


                        if (Total_Worked_Min <= 0)
                        {
                            Total_Worked_Min = assingmin;
                        }


                        DateTime FDate = new DateTime(FromDate.Year, FromDate.Month, 1);

                        DateTime NDate = FDate.AddMonths(1);
                        NDate = new DateTime(NDate.Year, NDate.Month, 1);

                        DateTime TDate = NDate.AddDays(-1);

                        if (SalaryCalculationMethode == "Fixed Days")
                        {
                            Total_Days = DaysInMonth;
                        }
                        else
                        {
                            Total_Days = TDate.Subtract(FDate).Days + 1;
                        }

                        Total_Days = Total_Days - WeekOffCount;


                        Basic_Min_Salary = Basic_Salary / (Total_Days * Total_Worked_Min);


                        Is_Absent = dtTemp.Rows[0]["Is_Absent"].ToString();


                        // Salary Calculation 
                        for (int k = 0; k < dtTemp.Rows.Count; k++)
                        {
                            dtSalary.ImportRow(dtTemp.Rows[k]);

                            dtSalary.Rows[j]["Field2"] = "";
                            dtSalary.Rows[j]["Field3"] = "";
                            dtSalary.Rows[j]["Field4"] = "";


                            PartialMin = int.Parse(dtTemp.Rows[k]["Partial_Violation_Min"].ToString());

                            //Modified By Nitin Jain
                            DataTable DtPartialLeave = objPartial.GetPartialLeaveRequestById(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString());

                            if (DtPartialLeave == null)
                            {
                                Partial_Penalty_Min = 0;
                                Parital_Violation_Penalty = 0;
                            }
                            else
                            {

                                DtPartialLeave = new DataView(DtPartialLeave, "Partial_Leave_Date='" + FromDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (DtPartialLeave.Rows.Count > 0)
                                {
                                    if (PartialMin > 0)
                                    {
                                        // Nitin Jain Partial Min Salary Calculation here 
                                        PartialMin = int.Parse(dtTemp.Rows[k]["Partial_Violation_Min"].ToString());
                                        if (IsEmpPartial == true)
                                        {
                                            Parital_Violation_Penalty = objAttendance.ParialPenalty(Session["CompId"].ToString(), Basic_Min_Salary, dtEmp.Rows[i]["Emp_Id"].ToString(), 1, PartialMin, "DailySalary", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                        }
                                        else
                                        {
                                            //updated by jitendra upadhyay because partial violation penalty was coming when partial penalty is disable
                                            //updated on 13/12/2017

                                            //Parital_Violation_Penalty = PartialMin * Basic_Min_Salary;
                                            Parital_Violation_Penalty = 0;
                                        }
                                        //....................................
                                    }
                                    else
                                    {
                                        Partial_Penalty_Min = 0;
                                    }
                                }
                                else
                                {
                                    Parital_Violation_Penalty = 0;
                                }
                            }


                            // Updated By Nitin Jain .. Updated Date - 03/05/2014  -- To take asign work minute on bases of shift on perticular day

                            // Total_Worked_Min = assingmin;
                            Total_Worked_Min = int.Parse(dtTemp.Rows[k]["TotalAssign_Min"].ToString());
                            //  Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Field1"].ToString());
                            if (dtTemp.Rows[k]["Field1"].ToString() == "")
                            {
                                dtTemp.Rows[k]["Field1"] = "0";
                            }
                            if (dtTemp.Rows[k]["Early_Relaxation_Min"].ToString() == "")
                            {
                                dtTemp.Rows[k]["Early_Relaxation_Min"] = "0";
                            }
                            Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());
                            IsHoliday = dtTemp.Rows[k]["Is_Holiday"].ToString();

                            // Absent Day Penalty 
                            if (Is_AbsentPenalty == true)
                            {
                                Absent_Day_Penalty = objAttendance.AbsentDaysSalary(Session["CompId"].ToString(), Basic_Min_Salary, dtEmp.Rows[i]["Emp_Id"].ToString(), Total_Worked_Min, 1, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                            }
                            else
                            {
                                Absent_Day_Penalty = 0;
                            }
                            //Priya jain(16.04.2014)
                            if (WorkCalMethod == "PairWise")
                            {
                                if (Is_Absent == "True")
                                {
                                    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                }
                                else
                                {


                                    //updated on 24/11/21017
                                    //effectiveworkminute = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());

                                    //if (effectiveworkminute > Total_Worked_Min)
                                    //{
                                    //    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                    //}
                                    //else
                                    //{
                                    //    Basic_Work_Salary = Basic_Min_Salary * effectiveworkminute;
                                    //}


                                    Basic_Work_Salary = Basic_Min_Salary * Days_In_WorkMin;
                                }
                            }
                            else
                            {
                                if (Is_Absent == "True")
                                {
                                    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                }
                                else
                                {
                                    Basic_Work_Salary = Basic_Min_Salary * Days_In_WorkMin;

                                    //effectiveworkminute = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());
                                    //if (effectiveworkminute > Total_Worked_Min)
                                    //{
                                    //    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                    //}
                                    //else
                                    //{
                                    //    Basic_Work_Salary = Basic_Min_Salary * effectiveworkminute;
                                    //}

                                }
                            }

                            if (Is_EmpOverTime != false)
                            {
                                // Nitin Jain Normal OT Salary Calculation Here
                                Normal_OT_Min = int.Parse(dtTemp.Rows[k]["OverTime_Min"].ToString());
                                if (Normal_OT_Min > 0)
                                {
                                    Normal_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary, Normal_OT_Min, "Normal");
                                }
                                else
                                {
                                    Normal_OT_Work_Salary = 0;
                                }
                                // Nitin Jain Week Off OT Salary Calculation Here 
                                Week_Off_OT_Min = int.Parse(dtTemp.Rows[k]["Week_Off_Min"].ToString());
                                if (Week_Off_OT_Min > 0)
                                {
                                    WeekOff_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary, Week_Off_OT_Min, "WeekOff");
                                }
                                else
                                {
                                    WeekOff_OT_Work_Salary = 0;
                                }
                                // Nitin Jain Holiday OT Salary Calculation Here 
                                Holiday_OT_Min = int.Parse(dtTemp.Rows[k]["Holiday_Min"].ToString());
                                if (Holiday_OT_Min > 0)
                                {
                                    Holiday_OT_Work_Salary = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtEmp.Rows[i]["Emp_Id"].ToString(), Basic_Min_Salary, Holiday_OT_Min, "Holiday");
                                }
                                else
                                {
                                    Holiday_OT_Work_Salary = 0;
                                }

                                //LINE ADDED BY JITENDRA UPADHYAY BECAUSE IT WAS SHOWING WORK SALARY INCLUDED OT WHEN OVERTIME IS ENABLE

                                //CODE START

                                //15-09-2017
                                effectiveworkminute = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString());
                                if (effectiveworkminute > Total_Worked_Min)
                                {
                                    Basic_Work_Salary = Basic_Min_Salary * Total_Worked_Min;
                                }

                                //CODE END

                            }
                            double TotalSalary = 0;
                            TotalOTMin += int.Parse(dtTemp.Rows[k]["OverTime_Min"].ToString());
                            if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Week_Off"].ToString()))
                            {
                                dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), WeekOff_OT_Work_Salary.ToString()).ToString();
                                if (ExcludeWeekOffInSalary == "True")
                                {
                                    TotalSalary = 0;
                                }
                                else
                                {
                                    TotalSalary = int.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Assign_Min")) * Basic_Min_Salary;
                                }
                                dtSalary.Rows[j]["OverTime_Min"] = dtTemp.Rows[k]["Week_Off_Min"];
                                TotalOTMin += int.Parse(dtTemp.Rows[k]["Week_Off_Min"].ToString());
                            }
                            else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Holiday"].ToString()))
                            {
                                dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), Holiday_OT_Work_Salary.ToString()).ToString();
                                TotalSalary = int.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Assign_Min")) * Basic_Min_Salary;
                                dtSalary.Rows[j]["OverTime_Min"] = dtTemp.Rows[k]["Holiday_Min"];
                                TotalOTMin += int.Parse(dtTemp.Rows[k]["Holiday_Min"].ToString());
                            }
                            else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Leave"].ToString()))
                            {
                                //Code Modified for paid leave 23 Dec 2013 Kunal

                                string strLeaveType = ObjDa.return_DataTable("SELECT Att_Leave_Request_Child.LeaveType_Id FROM Att_Leave_Request_Child LEFT OUTER JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE  (Att_Leave_Request_Child.IsActive = 'True') AND (Att_Leave_Request_Child.Leave_Date = '" + dtTemp.Rows[k]["Att_Date"].ToString() + "') and Att_Leave_Request.Emp_Id='" + dtTemp.Rows[k]["Emp_Id"].ToString() + "'").Rows[0]["LeaveType_Id"].ToString();


                                bool IsPaid = false;
                                if (IsLeaveSalary)
                                {

                                    // dtSalary.Rows[j]["Field5"] = System.Math.Round(TotalGrossSalary, 2).ToString();
                                    IsPaid = objLeaveReq.IsPaidLeave(dtTemp.Rows[k]["Att_Date"].ToString(), dtTemp.Rows[k]["Emp_Id"].ToString());
                                    if (IsPaid)
                                    {
                                        //Add On 06-06-2016 By Lokesh
                                        bool IsLSalary = false;
                                        DataTable dtLeaveSalary = objLeaveReq.IsLeaveSalary(dtTemp.Rows[k]["Att_Date"].ToString(), dtTemp.Rows[k]["Emp_Id"].ToString());
                                        if (dtLeaveSalary.Rows.Count > 0)
                                        {
                                            IsLSalary = Convert.ToBoolean(dtLeaveSalary.Rows[0]["LeaveSalary"].ToString());
                                        }
                                        //if (!IsLSalary)
                                        //{
                                        TotalSalary = Total_Worked_Min * Basic_Min_Salary;
                                        //}
                                        //else
                                        //{
                                        //    TotalSalary = 0;
                                        //}

                                        //OLD Only
                                        //TotalSalary = Total_Worked_Min * Basic_Min_Salary;
                                    }
                                    else
                                    {

                                        TotalSalary = 0;

                                        //here we are deducting salary according sick leave  deduction slab
                                        if (GetEmployeeSickLeavededuction(dtTemp.Rows[k]["Emp_Id"].ToString(), dtTemp.Rows[k]["Att_Date"].ToString(), strLeaveType) > 0)
                                        {
                                            TotalSalary = Total_Worked_Min * Basic_Min_Salary - ((Total_Worked_Min * Basic_Min_Salary) * (GetEmployeeSickLeavededuction(dtTemp.Rows[k]["Emp_Id"].ToString(), dtTemp.Rows[k]["Att_Date"].ToString(), strLeaveType) / 100));
                                        }


                                    }
                                }
                                else
                                {
                                    TotalSalary = 0;
                                }


                                dtSalary.Rows[j]["Field4"] = "0";
                            }
                            //Updated By Nitin jain(05.05.2014) 
                            else if (Convert.ToBoolean(dtTemp.Rows[k]["Is_Absent"].ToString()))
                            {
                                //Update 27-Feb-2015
                                //TotalSalary = 0 - Absent_Day_Penalty;

                                //10-07-2015
                                TotalSalary = 0 - Absent_Day_Penalty;

                                dtSalary.Rows[j]["Field4"] = "0";
                            }
                            else
                            {
                                dtSalary.Rows[j]["Field4"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), Normal_OT_Work_Salary.ToString()).ToString();
                                TotalSalary = Basic_Work_Salary - Late_Min_Penalty - Early_Min_Penalty - Parital_Violation_Penalty;
                            }
                            dtSalary.Rows[j]["Field3"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), TotalSalary.ToString()).ToString();

                            dtSalary.Rows[j]["Field2"] = TotalOTMin.ToString();
                            try
                            {
                                TotalGrossSalary = TotalGrossSalary + TotalSalary + double.Parse(dtSalary.Rows[j]["Field4"].ToString());
                            }
                            catch
                            {
                                TotalGrossSalary = TotalGrossSalary + TotalSalary;
                            }


                            dtSalary.Rows[j]["Field5"] = objSys.GetCurencyConversionForInv(Session["Currencyid"].ToString(), TotalGrossSalary.ToString()).ToString();

                            j++;
                        }
                    }
                    //........................................................
                    FromDate = FromDate.AddDays(1);
                }
            }
            //for (int k = 0; k < dtSalary.Rows.Count; k++)
            //{
            //    int counter = 0;
            //    dtSalary.Rows[k]["Field5"] = SystemParameter.GetCurrencySmbol(Common.GetEmployeeCurreny(dtSalary.Rows[k]["Emp_Id"].ToString()), objSys.GetCurencyConversionForInv(Common.GetEmployeeCurreny(dtSalary.Rows[k]["Emp_Id"].ToString()), dtSalary.Rows[k]["Field5"].ToString())); ;
            //    if (Convert.ToBoolean(dtSalary.Rows[k]["Is_Week_Off"].ToString()))
            //    {
            //        if (objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString()) == "")
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = "ffffff";
            //        }
            //        else
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString());
            //        }
            //        counter = 1;
            //    }
            //    else if (Convert.ToBoolean(dtSalary.Rows[k]["Is_Holiday"].ToString()))
            //    {
            //        if (objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString()) == "")
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = "ffffff";
            //        }
            //        else
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString());
            //        }

            //        counter = 1;
            //    }
            //    else if (Convert.ToBoolean(dtSalary.Rows[k]["Is_Leave"].ToString()))
            //    {
            //        if (objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString()) == "")
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = "ffffff";
            //        }
            //        else
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString());
            //        }

            //        counter = 1;
            //    }
            //    else if (Convert.ToBoolean(dtSalary.Rows[k]["Is_Absent"].ToString()))
            //    {
            //        if (objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString()) == "")
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = "ffffff";
            //        }
            //        else
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString());
            //        }

            //        counter = 1;
            //    }

            //    if (counter == 0)
            //    {
            //        if (objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString()) == "")
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = "ffffff";
            //        }
            //        else
            //        {
            //            dtSalary.Rows[k]["Colour_Code"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString());
            //        }
            //    }
            //}
            string CompanyName = "";
            string CompanyAddress = "";
            string Imageurl = "";
            string BrandName = "";
            string LocationName = "";
            string DepartmentName = "";
            // Get Company Name
            CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
            // Image Url
            Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
            // Get Brand Name
            BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());
            // Get Location Name
            if (Session["LocationName"].ToString() == "")
            {
                LocationName = objAttendance.GetLocationName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["Lang"].ToString());
            }
            else
            {
                LocationName = Session["LocationName"].ToString();
            }
            // Get Department Name
            if (Session["DepName"].ToString() == "")
            {
                DepartmentName = "All";
            }
            else
            {
                DepartmentName = Session["DepName"].ToString();
            }
            // Get Company Address
            DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
            if (DtAddress.Rows.Count > 0)
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }

            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("WeekOff_Report", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                dtSalary = new DataView(dtSalary, "Is_Week_Off='" + false.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Holiday_Report", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                dtSalary = new DataView(dtSalary, "Is_Holiday='" + false.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Report", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                dtSalary = new DataView(dtSalary, "Is_Leave='" + false.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //---Company Logo
            //XRPictureBox Company_Logo = (XRPictureBox)RptShift.FindControl("Company_Logo", true);
            //try
            //{
            //    Company_Logo.ImageUrl = Imageurl;
            //}
            //catch
            //{
            //}
            //------------------

            //Comany Name
            //XRLabel Lbl_Company_Name = (XRLabel)RptShift.FindControl("Lbl_Company_Name", true);
            //Lbl_Company_Name.Text = CompanyName;
            //------------------

            //Comapny Address
            //XRLabel Lbl_Company_Address = (XRLabel)RptShift.FindControl("Lbl_Company_Address", true);
            //Lbl_Company_Address.Text = CompanyAddress;
            //------------------


            //Brand Name
            //XRLabel Lbl_Brand = (XRLabel)RptShift.FindControl("Lbl_Brand", true);
            //Lbl_Brand.Text = Resources.Attendance.Brand + " : ";
            //XRLabel Lbl_Brand_Name = (XRLabel)RptShift.FindControl("Lbl_Brand_Name", true);
            //Lbl_Brand_Name.Text = BrandName;
            ////------------------

            //// Location Name
            //XRLabel Lbl_Location = (XRLabel)RptShift.FindControl("Lbl_Location", true);
            //Lbl_Location.Text = Resources.Attendance.Location + " : ";
            //XRLabel Lbl_Location_Name = (XRLabel)RptShift.FindControl("Lbl_Location_Name", true);
            //Lbl_Location_Name.Text = LocationName;
            ////------------------

            //// Department Name
            //XRLabel Lbl_Department = (XRLabel)RptShift.FindControl("Lbl_Department", true);
            //Lbl_Department.Text = Resources.Attendance.Department + " : ";
            //XRLabel Lbl_Department_Name = (XRLabel)RptShift.FindControl("Lbl_Department_Name", true);
            //Lbl_Department_Name.Text = DepartmentName;
            //------------------

            //// Report Title
            //XRLabel Report_Title = (XRLabel)RptShift.FindControl("Report_Title", true);
            //Report_Title.Text = "Salary Report" + " From " + FromDate1.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
            //------------------



            // Detail Header Table
            //XRLabel xrTableCell21 = (XRLabel)RptShift.FindControl("xrTableCell21", true);
            //xrTableCell21.Text = "Name";

            //XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            //xrTableCell7.Text = "Date";

            //XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            //xrTableCell1.Text = "In Time";

            //XRLabel xrTableCell22 = (XRLabel)RptShift.FindControl("xrTableCell22", true);
            //xrTableCell22.Text = "Out Time";

            //XRLabel xrTableCell24 = (XRLabel)RptShift.FindControl("xrTableCell24", true);
            //xrTableCell24.Text = "Work Hour";

            //XRLabel xrTableCell25 = (XRLabel)RptShift.FindControl("xrTableCell25", true);
            //xrTableCell25.Text = "Over Time Hour";

            //XRLabel xrTableCell23 = (XRLabel)RptShift.FindControl("xrTableCell23", true);
            //xrTableCell23.Text = "Salary";

            //XRLabel xrTableCell26 = (XRLabel)RptShift.FindControl("xrTableCell26", true);
            //xrTableCell26.Text = "OT Salary";

            //XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            //xrTableCell3.Text = "Is Off";

            //XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            //xrTableCell2.Text = "Is Holiday";

            //XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            //xrTableCell4.Text = "Is Leave";

            //XRLabel xrTableCell52 = (XRLabel)RptShift.FindControl("xrTableCell52", true);
            //xrTableCell52.Text = "Absent";
            ////-------------------------------------------

            //// Detail Id And Name
            //XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            //xrTableCell13.Text = Resources.Attendance.Id;

            //XRLabel xrTableCell17 = (XRLabel)RptShift.FindControl("xrTableCell17", true);
            //xrTableCell17.Text = Resources.Attendance.Name;
            //--------------------------------------------

            //Footer
            // Create by
            //XRLabel Lbl_Created_By = (XRLabel)RptShift.FindControl("Lbl_Created_By", true);
            //Lbl_Created_By.Text = Resources.Attendance.Created_By;
            //XRLabel Lbl_Created_By_Name = (XRLabel)RptShift.FindControl("Lbl_Created_By_Name", true);
            //Lbl_Created_By_Name.Text = Session["UserId"].ToString();
            //--------------------


            //group footer
            //XRLabel lblFooterEmpCode = (XRLabel)RptShift.FindControl("xrTableCell19", true);
            //lblFooterEmpCode.Text = Resources.Attendance.Id;

            //XRLabel lblFooterEmpName = (XRLabel)RptShift.FindControl("xrTableCell31", true);
            //lblFooterEmpName.Text = Resources.Attendance.Name;

            //XRLabel lblFooterweekoffcount = (XRLabel)RptShift.FindControl("xrTableCell41", true);
            //lblFooterweekoffcount.Text = Resources.Attendance.Week_Off_Count;

            //XRLabel lblFooterholidayCount = (XRLabel)RptShift.FindControl("xrTableCell46", true);
            //lblFooterholidayCount.Text = Resources.Attendance.Holiday_Count;

            //XRLabel lblFooterLeaveCount = (XRLabel)RptShift.FindControl("xrTableCell43", true);
            //lblFooterLeaveCount.Text = Resources.Attendance.Leave_Count;

            //XRLabel lblFooterworkHour = (XRLabel)RptShift.FindControl("xrTableCell32", true);
            //lblFooterworkHour.Text = Resources.Attendance.Work_Hour;

            //XRLabel lblFooterAssignHour = (XRLabel)RptShift.FindControl("xrTableCell34", true);
            //lblFooterAssignHour.Text = Resources.Attendance.Assigned_Hour;


            //XRLabel lblFooterOTHour = (XRLabel)RptShift.FindControl("xrTableCell45", true);
            //lblFooterOTHour.Text = Resources.Attendance.Over_Time_Hour;


            //XRLabel lblFooterworksalary = (XRLabel)RptShift.FindControl("xrTableCell37", true);
            //lblFooterworksalary.Text = Resources.Attendance.Work_Salary;

            //XRLabel lblFooterOTsalary = (XRLabel)RptShift.FindControl("xrTableCell39", true);
            //lblFooterOTsalary.Text = Resources.Attendance.OT_Salary;

            //XRLabel lblFooteGrosssalary = (XRLabel)RptShift.FindControl("xrTableCell49", true);
            //lblFooteGrosssalary.Text = Resources.Attendance.Gross_Salary;

            RptShift.SetImage(Imageurl);
            RptShift.SetBrandName(BrandName);
            RptShift.SetLocationName(LocationName);
            RptShift.SetDepartmentName(DepartmentName);
            RptShift.setTitleName("Salary Report" + " From " + FromDate1.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            RptShift.setcompanyname(CompanyName);
            RptShift.setaddress(CompanyAddress);
            RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Work_Hour, Resources.Attendance.Over_Time_Hour, Resources.Attendance.Salary, Resources.Attendance.OT_Salary, Resources.Attendance.Is_Off, Resources.Attendance.Is_Holiday, Resources.Attendance.Is_Leave, Resources.Attendance.Week_Off_Count, Resources.Attendance.Holiday_Count, Resources.Attendance.Leave_Count, Resources.Attendance.Assigned_Hour, Resources.Attendance.Over_Time_Hour, Resources.Attendance.Work_Salary, Resources.Attendance.OT_Salary, Resources.Attendance.Gross_Total);
            RptShift.setUserName(Session["UserId"].ToString());
            RptShift.DataSource = dtSalary;
            RptShift.DataMember = "sp_Att_AttendanceRegister_Report";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;
        }
    }




    public Double GetEmployeeSickLeavededuction(string strEmpId, string attdate, string strLeaveTypeId)
    {




        double deduction_percentage = 0;

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }

        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }

        int exceedDays = 0;

        string strsql = "select count(*), isnull( max(Att_Leave_Request.Leave_Type_Id),0) from Att_Leave_Request inner join Att_Leave_Request_Child on Att_Leave_Request.Trans_Id = Att_Leave_Request_Child.Ref_Id where Att_Leave_Request_Child.Is_Paid='False' and Att_Leave_Request.Emp_Id=" + strEmpId + " and Att_Leave_Request.Is_Approved='True' and Att_Leave_Request_Child.Leave_Date<='" + attdate.ToString() + "' and Att_Leave_Request.From_Date>='" + FinancialYearStartDate.ToString() + "' and Att_Leave_Request.To_Date<='" + FinancialYearEndDate.ToString() + "' and Att_Leave_Request_Child.LeaveType_Id='" + strLeaveTypeId + "'";

        if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
        {
            exceedDays = Convert.ToInt32(ObjDa.return_DataTable(strsql).Rows[0][0].ToString());
            //  strLeaaveTypeId = ObjDa.return_DataTable(strsql).Rows[0][1].ToString();
        }

        DataTable dtdeduction = objLeavededuction.GetRecordbyLeaveTypeandEmployeeId(strLeaveTypeId, strEmpId);

        if (dtdeduction.Rows.Count > 0)
        {

            strsql = "select deduction_percentage from Att_LeaveMaster_deduction where daysfrom <= '" + exceedDays.ToString() + "' and daysto >= '" + exceedDays.ToString() + "' and emp_id=" + strEmpId + " and LeaveType_id=" + strLeaveTypeId + "";

            if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
            {
                deduction_percentage = Convert.ToDouble(ObjDa.return_DataTable(strsql).Rows[0][0].ToString());
            }
        }
        else
        {
            deduction_percentage = 100;
        }

        return deduction_percentage;

    }

}
