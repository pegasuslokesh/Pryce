using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Attendance_Report_SalarySummaryReport : BasePage
{
    Att_SalaryReport objReport = null;
    EmployeeParameter objEmpParam = null;
    Attendance objAttendance = null;
    Set_ApplicationParameter objAppParam = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    SystemParameter objSys = null;
    Att_AttendanceRegister objsalary = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    DepartmentMaster objDept = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objReport = new Att_SalaryReport(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objsalary = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        GetReport();
        Session["AccordianId"] = "6";
        Session["HeaderText"] = "Attendance Salary Reports";
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
            //this code is created by jitendra upadhyay on 19-09-2014
            //this code for employee should not be showing in repot if not exists in employee notification for in out report
            //code start
            EmpReport = Session["EmpList"].ToString();

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "16");


            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
            //{
            //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

            //    if (dtEmpNF.Rows.Count > 0)
            //    {
            //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Rpt_Salary"].ToString()))
            //        {
            //            Emplist += EmpReport.Split(',')[i].ToString() + ",";
            //        }
            //    }
            //}

            //code end


            DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist, 2);
            }
            catch
            {

            }
            dtFilter = rptdata.sp_Att_AttendanceRegister_Report;
            DataTable dtEmp = dtFilter.DefaultView.ToTable(true, "Emp_Id", "Emp_Name", "Emp_Code");
            //string EmpId = dtEmp.Rows[0]["Emp_Id"].ToString();
            //string Emp_Name = dtEmp.Rows[0]["Emp_Name"].ToString();
            DataTable dtTemp = new DataTable();
            DataTable dtSalary = new DataTable();
            DataTable dtMinutes = new DataTable();

            //dtMinutes = objsalary.GetSalarySummeryReportByEmpId(Emplist.ToString(), FromDate.ToString(), ToDate.ToString());

            //dtSalary = dtFilter.Clone();
            int Total_Days = 0;
            int Days_In_WorkMin = 0;
            int Worked_Days = 0;
            int Week_Off_Days = 0;
            int Holiday_Days = 0;
            int Leave_Days = 0;
            int Absent_Days = 0;
            int Assigned_Worked_Min = 0;


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


            bool IsEmpLate = false;
            bool IsEmpEarly = false;
            bool IsEmpPartial = false;
            string LateMethod = string.Empty;
            string EarlyMethod = string.Empty;
            string PartialMethod = string.Empty;
            int LatePenaltyDedMin = 0;
            int EarlyPenaltyDedMin = 0;
            int PartialPenaltyDedMin = 0;
            int PartialMin = 0;
            int TotalOTMin = 0;
            double TotalGrossSalary = 0;
            double EarlyMinutSalary = 0;
            double Gross_Salary = 0;
            double LateMinuteSalary = 0;
            double WorkHourSalary = 0;
            string SalaryCalculationMethode = string.Empty;
            int DaysInMonth = 0;

            DataColumn column;

            column = new DataColumn();

            column = dtSalary.Columns.Add("EmpId", typeof(string));
            column = new DataColumn();

            column = dtSalary.Columns.Add("Emp_Code", typeof(string));
            column = new DataColumn();


            column = dtSalary.Columns.Add("Emp_Name", typeof(string));
            column = new DataColumn();

            column = dtSalary.Columns.Add("Att_Date", typeof(string));




            //create 1st Column
            column = new DataColumn();

            column = dtSalary.Columns.Add("LateMinuteSalary", typeof(string));

            //Create 2nd Column
            column = new DataColumn();
            column = dtSalary.Columns.Add("EarlyMinuteSalary", typeof(string));

            //Create 3rd Column
            column = new DataColumn();
            column = dtSalary.Columns.Add("WorkHourSalary", typeof(string));

            //Create 4th Column

            column = new DataColumn();

            column = dtSalary.Columns.Add("Gross_Salary", typeof(string));


            for (int i = 0; i < dtEmp.Rows.Count; i++)
            {
                string EmpId = dtEmp.Rows[i]["Emp_Id"].ToString();
                string Emp_Name = dtEmp.Rows[i]["Emp_Name"].ToString();
                string Emp_Code = dtEmp.Rows[i]["Emp_Code"].ToString();

                FromDate = FromDate1;
                IsEmpLate = false;
                IsEmpEarly = false;
                IsEmpPartial = false;
                LateMethod = string.Empty;
                EarlyMethod = string.Empty;
                PartialMethod = string.Empty;
                LatePenaltyDedMin = 0;
                EarlyPenaltyDedMin = 0;
                PartialPenaltyDedMin = 0;
                TotalOTMin = 0;

                Assigned_Worked_Min = 0;


                Basic_Salary = 0;
                Basic_Min_Salary = 0;
                Normal_OT_Salary = 0;
                Week_Off_OT_Salary = 0;
                Holiday_OT_Salary = 0;
                Absent_Penalty = 0;
                Late_Penalty_Min = 0;
                Early_Penalty_Min = 0;
                Partial_Penalty_Min = 0;
                TotalGrossSalary = 0;
                EarlyMinutSalary = 0;
                Gross_Salary = 0;
                LateMinuteSalary = 0;
                WorkHourSalary = 0;
                try
                {

                    IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field2"));
                    IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(dtEmp.Rows[i]["Emp_Id"].ToString(), "Field1"));

                    LateMethod = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    EarlyMethod = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    LatePenaltyDedMin = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Late_Penalty_Min_Deduct", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                    EarlyPenaltyDedMin = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Early_Penalty_Min_Deduct", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

                    // Modified By Nitin Jain Date 19-06-2014  to get Days in Month from Company Parameter if It is Monthly Bases or Fixed Days Bases
                    SalaryCalculationMethode = (objAppParam.GetApplicationParameterValueByParamName("Salary Calculate According To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                    DaysInMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Days In Month", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

                    DateTime FDate = new DateTime(FromDate.Year, FromDate.Month, 1);

                    DateTime NDate = FDate.AddMonths(1);
                    NDate = new DateTime(NDate.Year, NDate.Month, 1);

                    DateTime TDate = NDate.AddDays(-1);

                    // Modified By Nitin Jain Date 19-06-2014  to get Days in Month from Company Parameter if It is Monthly Bases or Fixed Days Bases
                    if (SalaryCalculationMethode == "Fixed Days")
                    {
                        Total_Days = DaysInMonth;
                    }
                    else
                    {
                        Total_Days = TDate.Subtract(FDate).Days + 1;
                    }
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

                Basic_Min_Salary = Basic_Salary / (Total_Days * assingmin);
                if (Basic_Min_Salary.ToString() == "NaN")
                {
                    Basic_Min_Salary = 0;

                }
                Late_Penalty_Min = OnMinuteLatePenalty(Basic_Min_Salary, dtEmp.Rows[i]["Emp_Id"].ToString());

                Early_Penalty_Min = OnMinuteEarlyPenalty(Basic_Min_Salary, dtEmp.Rows[i]["Emp_Id"].ToString());
                while (FromDate <= ToDate)
                {

                    dtTemp = dtFilter.Clone();
                    dtTemp = new DataView(dtFilter, "Att_Date='" + FromDate.ToString("dd-MMM-yyyy") + "' and Emp_Id='" + dtEmp.Rows[i]["Emp_Id"].ToString() + "'   ", "", DataViewRowState.CurrentRows).ToTable();
                    string Att_date = dtTemp.Rows[0]["Att_Date"].ToString();
                    if (dtTemp.Rows.Count > 0)
                    {

                        for (int k = 0; k < dtTemp.Rows.Count; k++)
                        {
                            Total_Worked_Min = assingmin;
                            if (dtTemp.Rows[k]["Field1"].ToString() == "")
                            {
                                dtTemp.Rows[k]["Field1"] = "0";
                            }

                            Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Field1"].ToString());


                            Late_Min = int.Parse(dtTemp.Rows[k]["Late_Relaxation_Min"].ToString());

                            if (IsEmpLate && LateMethod == "Min")
                            {
                                Late_Min = Late_Min * LatePenaltyDedMin;

                            }

                            Early_Min = int.Parse(dtTemp.Rows[k]["Early_Relaxation_Min"].ToString());

                            if (IsEmpEarly && EarlyMethod == "Min")
                            {
                                Early_Min = Early_Min * EarlyPenaltyDedMin;

                            }

                            Total_Worked_Min = assingmin;
                            Days_In_WorkMin = int.Parse(dtTemp.Rows[k]["EffectiveWork_Min"].ToString()) + int.Parse(dtTemp.Rows[k]["Field1"].ToString());
                            LateMinuteSalary = Late_Min * Late_Penalty_Min;
                            EarlyMinutSalary = Early_Penalty_Min * Early_Min;
                            WorkHourSalary = Basic_Min_Salary * Total_Worked_Min;
                            Gross_Salary = WorkHourSalary - LateMinuteSalary - EarlyMinutSalary;

                            DataRow dr = dtSalary.NewRow();
                            for (int m = 0; m < dtSalary.Columns.Count; m++)
                            {

                                dr["EmpId"] = EmpId.ToString();
                                dr["Emp_Code"] = Emp_Code.ToString();
                                dr["Emp_Name"] = Emp_Name.ToString();
                                dr["Att_Date"] = Att_date.ToString();
                                dr["LateMinuteSalary"] = System.Math.Round(LateMinuteSalary, 2).ToString();

                                dr["EarlyMinuteSalary"] = System.Math.Round(EarlyMinutSalary, 2).ToString();
                                //Work hour salary

                                dr["WorkHourSalary"] = System.Math.Round(WorkHourSalary, 2).ToString();


                                dr["Gross_Salary"] = System.Math.Round(Gross_Salary, 2).ToString();


                            }
                            dtSalary.Rows.Add(dr);










                        }//for loop
                    }//if loop
                    FromDate = FromDate.AddDays(1);

                }//while loop

            }//for loop
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
            if (Session["LocName"].ToString() == "")
            {
                LocationName = objAttendance.GetLocationName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["Lang"].ToString());
            }
            else
            {
                LocationName = Session["LocName"].ToString();
            }
            // Get Department Name
            //if (Session["DepName"].ToString() == "")
            //{
            //    DepartmentName = "All";
            //}
            //else
            //{
            //    DepartmentName = Session["DepName"].ToString();
            //}
            // Get Company Address
            DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
            if (DtAddress.Rows.Count > 0)
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            objReport.SetImage(Imageurl);
            objReport.SetBrandName(BrandName);
            objReport.SetLocationName(LocationName);
            objReport.SetDepartmentName(DepartmentName);
            objReport.setTitleName("Salary Summary Report" + " From " + FromDate1.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            objReport.setcompanyname(CompanyName);
            objReport.setaddress(CompanyAddress);
            objReport.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Attendance_Date, Resources.Attendance.LateMinuteSalary, Resources.Attendance.EarlyOutMinuteSalary, Resources.Attendance.WorkHourSalary, Resources.Attendance.Gross_Total);
            objReport.setUserName(Session["UserId"].ToString());
            objReport.DataSource = dtSalary;
            objReport.DataMember = "sp_Att_AttendanceRegister_Report";

            ReportViewer1.Report = objReport;
            ReportToolbar1.ReportViewer = ReportViewer1;

        }
    }


    public double OnMinuteLatePenalty(double PerMinSal, string EmpId)
    {
        double sal = 0;
        string Method = string.Empty;
        string Type = string.Empty;
        double Value = 0;
        bool IsEmpLate = false;
        // Modify by Priya Jain(01.04.2014)
        try
        {
            IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field6"));
        }
        catch
        {
        }


        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (IsEmpLate)
        {
            if (Method == "Salary")
            {
                try
                {
                    Type = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Type", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch
                {
                }
                try
                {

                    Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Value", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                }
                catch
                {
                }
                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;

                }
                else
                {
                    sal = Value / 60;
                }

            }
            else
            {
                sal = PerMinSal;
            }
        }
        return sal;

    }
    public double OnMinuteEarlyPenalty(double PerMinSal, string EmpId)
    {
        double sal = 0;

        string Type = string.Empty;
        string Method = string.Empty;
        double Value = 0;
        bool IsEmpEarly = false;
        //Modify by Priya Jain(01.04.2014)
        try
        {
            IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field6"));
        }
        catch
        {

        }


        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsEmpEarly)
        {
            if (Method == "Salary")
            {
                try
                {
                    Type = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Type", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch
                {
                }
                try
                {
                    Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Value", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                }
                catch
                {
                }

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;

                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                sal = PerMinSal;
            }
        }
        return sal;

    }

}
