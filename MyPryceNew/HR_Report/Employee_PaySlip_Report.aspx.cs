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
using System.Globalization;
using System.Threading;
using System.IO;

public partial class TempReport_Employee_PaySlip_Report : BasePage
{
    Set_ApplicationParameter objAppParam = null;
    Pay_Employee_Attendance objEmpAttendance = null;
    Set_Allowance ObjAllow = null;
    Set_Deduction ObjDeduc = null;
    Set_Deduction objDeduction = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_Employee_Month objPayEmpMonth = null;
    Pay_Employee_Penalty objPEpenalty = null;
    Pay_Employee_claim objPEClaim = null;
    Pay_Employee_Deduction objpayrolldeduc = null;
    Pay_Employe_Allowance objpayrollall = null;
    Employee_Pay_Slip_Report objEmpPayslip = null;
    Document_Master ObjDoc = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    EmployeeMaster objEmp = null;
    DepartmentMaster objDep = null;
    EmployeeMaster objEmpmaster = null;
    NationalityMaster objNat = null;
    DesignationMaster objDesg = null;
    QualificationMaster objQualif = null;
    EmployeeParameter objempparam = null;
    CurrencyMaster ObjCurrency = null;
    SystemParameter objSys = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpAttendance = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        objDeduction = new Set_Deduction(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objPEpenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objPEClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objpayrolldeduc = new Pay_Employee_Deduction(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objEmpPayslip = new Employee_Pay_Slip_Report(Session["DBConnection"].ToString());
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objNat = new NationalityMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objQualif = new QualificationMaster(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        ObjCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "144", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "109";
        Session["HeaderText"] = "HR Report";
        if (Session["Querystring"] == null)
        {
            //Response.Redirect("~/HR_Report/GeneratePayrollReport.aspx");
            string TARGET_URL = "../HR_Report/GeneratePayrollReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            getrecoerd();

        }
        Page.Title = objSys.GetSysTitle();
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
    public string GetEmployeeName(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            empname = dt.Rows[0]["Emp_Name"].ToString();
            if (empname == "")
            {
                empname = "No Name";
            }
        }
        else
        {
            empname = "No Name";
        }

        return empname;



    }
    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Code"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }

        }
        else
        {
            empname = "No Code";

        }

        return empname;



    }
    public void getrecoerd()
    {
        DateTime Date = DateTime.Now;
        double sumallowance = 0;
        double sumdeduction = 0;
        double netsalary = 0;
        double sumloan = 0;
        double sumclaim = 0;
        double sumpenalty = 0;
        double totalAllowClaim = 0;
        double totalDeducPenaLoan = 0;
        double basicsal = 0;
        double prvmonthbal = 0;
        double totaldays = 0;
        double pvmonthbal_claimPenalty = 0;

        double totalattndsal = 0;
        double totalOTsal = 0;
        double totalpenasal = 0;
        double grossattendance = 0;
        string doj = "";
        string EmployeeName = "";
        string EmployeeCode = "";
        DataTable dtEmployeePaySlip = new DataTable();
        DataTable dtEmployeePayRecord = new DataTable();

        dtEmployeePaySlip.Columns.Add("EmpCode");
        dtEmployeePaySlip.Columns.Add("EmpName");
        dtEmployeePaySlip.Columns.Add("Designation");
        dtEmployeePaySlip.Columns.Add("Department");
        dtEmployeePaySlip.Columns.Add("DOJ");
        dtEmployeePaySlip.Columns.Add("Month");
        dtEmployeePaySlip.Columns.Add("Year");
        dtEmployeePaySlip.Columns.Add("BankAccountNo");
        dtEmployeePaySlip.Columns.Add("AllowanceName");
        dtEmployeePaySlip.Columns.Add("AllowanceActAmt");
        dtEmployeePaySlip.Columns.Add("NetSalary");
        dtEmployeePaySlip.Columns.Add("TypeAllow");
        dtEmployeePaySlip.Columns.Add("DaysPresent");
        dtEmployeePaySlip.Columns.Add("WeekOff");
        dtEmployeePaySlip.Columns.Add("daysAbsent");
        dtEmployeePaySlip.Columns.Add("Holiday");
        dtEmployeePaySlip.Columns.Add("Leaves");
        dtEmployeePaySlip.Columns.Add("Totaldays");
        dtEmployeePaySlip.Columns.Add("Emp_Id");
        dtEmployeePaySlip.Columns.Add("WorkedSal");
        dtEmployeePaySlip.Columns.Add("WeekOffSal");
        dtEmployeePaySlip.Columns.Add("HolidaysSal");
        dtEmployeePaySlip.Columns.Add("LeavedaysSal");
        dtEmployeePaySlip.Columns.Add("NormalOTSal");
        dtEmployeePaySlip.Columns.Add("WeekOffOTSal");
        dtEmployeePaySlip.Columns.Add("HolidaysOTSal");
        dtEmployeePaySlip.Columns.Add("LatePenaSal");
        dtEmployeePaySlip.Columns.Add("EarlyPenaSal");
        dtEmployeePaySlip.Columns.Add("PartialPenaSal");
        dtEmployeePaySlip.Columns.Add("AbsentPenaSal");
        dtEmployeePaySlip.Columns.Add("GrossAttendanceSal");
        dtEmployeePaySlip.Columns.Add("totalAttendSal");
        dtEmployeePaySlip.Columns.Add("TotalOTSal");
        dtEmployeePaySlip.Columns.Add("TotalPenaltySal");
        dtEmployeePaySlip.Columns.Add("TotalAdjustmentAmt");
        dtEmployeePaySlip.Columns.Add("PF");
        dtEmployeePaySlip.Columns.Add("ESIC");
        dtEmployeePaySlip.Columns.Add("Currency_Symbol");
        dtEmployeePaySlip.Columns.Add("EmpImageUrl");
        dtEmployeePaySlip.Columns.Add("Basic");
        dtEmployeePaySlip.Columns.Add("Dt_Type");
        dtEmployeePaySlip.Columns.Add("EarningName");
        dtEmployeePaySlip.Columns.Add("EarningActAmt");
        dtEmployeePaySlip.Columns.Add("DeductionsName");
        dtEmployeePaySlip.Columns.Add("DeductionsActAmt");


        string EmpReport = string.Empty;
        string EmpIds = string.Empty;

        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start
        //try
        //{
        //    if (Session["TerminateEmpId"].ToString() != "")
        //    {
        //        EmpReport = Session["TerminateEmpId"].ToString();
        //        Date = Convert.ToDateTime(Session["TerminationDate"].ToString());
        //        Session["Querystring"] = Session["TerminateEmpId"].ToString();
        //        Session["Month"] = Date.Month;
        //        Session["Year"] = Date.Year;
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    EmpReport = Session["Querystring"].ToString();
        //}


        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "20");


        foreach (DataRow dr in dtEmpNF.Rows)
        {
            EmpIds += dr["Emp_Id"] + ",";
        }

        //code end



        //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
        //{
        //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

        //    if (dtEmpNF.Rows.Count > 0)
        //    {
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report1"].ToString()))
        //        {
        //            EmpIds += EmpReport.Split(',')[i].ToString() + ",";
        //        }
        //    }
        //}

        //code end





        foreach (string str in EmpIds.Split(','))
        {


            if ((str != ""))
            {
                DataTable dtemppara = new DataTable();
                dtemppara = objempparam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                if (dtemppara.Rows.Count > 0)
                {
                    if (dtemppara.Rows[0]["Basic_Salary"].ToString() != "")
                    {
                        basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                    }
                    else
                    {
                        basicsal = 0;
                    }
                }


                EmployeeName = GetEmployeeName(str);
                EmployeeCode = GetEmployeeCode(str);

                sumallowance = 0;
                sumdeduction = 0;
                netsalary = 0;
                sumclaim = 0;
                sumpenalty = 0;
                sumloan = 0;
                totalAllowClaim = 0;
                totalDeducPenaLoan = 0;
                prvmonthbal = 0;
                pvmonthbal_claimPenalty = 0;

                totaldays = 0;
                grossattendance = 0;


                DataTable dtAllowance = new DataTable();
                DataTable dtDeuction = new DataTable();
                DataTable dtEmpInfo = new DataTable();
                DataTable dtDepartment = new DataTable();
                DataTable dtDesignation = new DataTable();
                DataTable dtLoan = new DataTable();
                DataTable dtPenalty = new DataTable();
                DataTable dtClaim = new DataTable();
                DataTable dtPayEmpMonth = new DataTable();
                DataTable dtEmpAttendance = new DataTable();
                DataTable dtPrvMonthBal = new DataTable();
                //this code is creeated on 01-04-2014 by jitendra upadhyay and divya mam
                //this code for showing the actual amount with previous month adjustment
                //code start
                int Lastmonth = 0;
                int LastYear = 0;
                double PvmonthClaim = 0;
                double pvmonthPenalty = 0;
                if (Session["Month"].ToString() == "1")
                {
                    LastYear = Convert.ToInt32(Session["Year"].ToString()) - 1;
                    Lastmonth = 12;
                }
                else
                {
                    Lastmonth = Convert.ToInt32(Session["Month"].ToString()) - 1;
                    LastYear = Convert.ToInt32(Session["Year"].ToString());
                }


                DataTable dtDueamount = new DataTable();

                dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(str, HttpContext.Current.Session["CompId"].ToString());
                try
                {
                    dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Claim'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (dtDueamount.Rows.Count > 0)
                {
                    PvmonthClaim = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
                }

                dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(str, HttpContext.Current.Session["CompId"].ToString());
                try
                {
                    dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Penalty'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (dtDueamount.Rows.Count > 0)
                {
                    pvmonthPenalty = Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
                }
                pvmonthbal_claimPenalty = PvmonthClaim - pvmonthPenalty;


                dtEmpInfo = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
                //EmployeeCode.ToString());
                dtEmpInfo = new DataView(dtEmpInfo, "Emp_Code='" + EmployeeCode.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                dtDepartment = objDep.GetDepartmentMasterById(dtEmpInfo.Rows[0]["Department_Id"].ToString());
                dtDesignation = objDesg.GetDesignationMasterById(dtEmpInfo.Rows[0]["Designation_Id"].ToString());
                dtAllowance = objpayrollall.GetPostedAllowanceAll(str, Session["Month"].ToString(), Session["Year"].ToString());

                dtDeuction = objpayrolldeduc.GetRecordPostedDeductionAll(str, Session["Month"].ToString(), Session["Year"].ToString());

                //      dtPayEmpMonth = objPayEmpMonth.Getallrecords(str);

                dtPayEmpMonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                //dtPayEmpMonth = new DataView(dtPayEmpMonth,                                "Emp_Id=" + str + " and Month=" + Session["Month"].ToString() +                               " and Year='" + Session["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


                dtPenalty = objPEpenalty.GetRecord_From_PayEmployeePenalty_By_MonthAndYear(Session["CompId"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());
                dtPenalty = new DataView(dtPenalty, "Emp_Id=" + str + " and Penalty_Month=" + Session["Month"].ToString() + " and Penalty_Year='" + Session["Year"].ToString() + "' and Field3<>'0' ", "", DataViewRowState.CurrentRows).ToTable();

                dtClaim = objPEClaim.GetRecord_From_PayEmployeeClaim(Session["CompId"].ToString(), str, "0", Session["Month"].ToString(), Session["Year"].ToString(), "Approved", "", "");
                dtClaim = new DataView(dtClaim, "Emp_Id=" + str + " and Claim_Month=" + Session["Month"].ToString() + " and Claim_Year='" + Session["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                dtLoan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                //dtLoan = new DataView(dtLoan, "Emp_Id=" + str + " and Month=" + Session["Month"].ToString() + " and Year='" + Session["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                dtEmpAttendance = objEmpAttendance.GetRecord_Emp_Attendance(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());



                for (int i = 0; i < dtEmpInfo.Rows.Count; i++)
                {
                    string EmpImageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmpInfo.Rows[0]["Emp_Image"].ToString();




                    if (dtEmpAttendance.Rows.Count > 0)
                    {

                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = "";
                        dr["Emp_Id"] = str;
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["Basic"] = basicsal;
                        dr["Dt_Type"] = 1;

                        //Attendance Days
                        dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                        dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                        dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                        dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                        dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();

                        double dayspres = Convert.ToDouble(dtEmpAttendance.Rows[0]["Worked_Days"].ToString());
                        double weekofdays = Convert.ToDouble(dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString());
                        double daysabsent = Convert.ToDouble(dtEmpAttendance.Rows[0]["Absent_Days"].ToString());
                        double holidays = Convert.ToDouble(dtEmpAttendance.Rows[0]["Holiday_Days"].ToString());
                        double daysleave = Convert.ToDouble(dtEmpAttendance.Rows[0]["Leave_Days"].ToString());
                        totaldays = dayspres + weekofdays + daysabsent + daysleave + holidays;
                        dr["Totaldays"] = totaldays.ToString();

                        //Attendance Salary
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            // worksal = worksal - leavesal - holidayssal - weekofsal;
                            //dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {

                            dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                            dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                            dr["Emp_Id"] = str;

                        }


                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");


                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();






                        dtEmployeePaySlip.Rows.Add(dr);


                    }
                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        //dr["TypeAllow"] = "PREVIOUS MONTH ADJUSTMENTS";
                        //dr["AllowanceActAmt"] = "0";
                        //dr["AllowanceName"] = "NO Deduction";
                        try
                        {
                            dr["Dt_Type"] = 1;
                            dr["Emp_Id"] = str;
                            dr["DaysPresent"] = "0";
                            dr["WeekOff"] = "0";
                            dr["daysAbsent"] = "0";
                            dr["Holiday"] = "0";
                            dr["Leaves"] = "0";
                            dr["Totaldays"] = "0";
                            dr["Basic"] = basicsal;
                            dr["EmpImageUrl"] = EmpImageurl;
                            dr["WorkedSal"] = "0";
                            dr["LeavedaysSal"] = "0";
                            dr["HolidaysSal"] = "0";
                            dr["WeekOffSal"] = "0";

                            dr["totalAttendSal"] = "0";

                            dr["NormalOTSal"] = "0";
                            dr["WeekOffOTSal"] = "0";
                            dr["HolidaysOTSal"] = "0";
                            dr["TotalOTSal"] = "0";


                            dr["LatePenaSal"] = "0";
                            dr["EarlyPenaSal"] = "0";
                            dr["PartialPenaSal"] = "0";
                            dr["AbsentPenaSal"] = "0";
                        }
                        catch
                        {

                        }


                        dr["TotalPenaltySal"] = totalpenasal.ToString();
                        dr["GrossAttendanceSal"] = grossattendance.ToString();


                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }



                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {

                            dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                            dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                            dr["Emp_Id"] = str;

                        }


                        dtEmployeePaySlip.Rows.Add(dr);


                    }


                    if (dtAllowance.Rows.Count > 0)
                    {


                        for (int j = 0; j < dtAllowance.Rows.Count; j++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            dr["TypeAllow"] = Resources.Attendance.Allowance;
                            dr["Basic"] = basicsal;
                            dr["Emp_Id"] = str;
                            dr["Dt_Type"] = 2;
                            dr["AllowanceActAmt"] = dtAllowance.Rows[j]["Act_Allowance_Value"].ToString();
                            double allow = Convert.ToDouble(dtAllowance.Rows[j]["Act_Allowance_Value"].ToString());
                            sumallowance += allow;
                            if (dtAllowance.Rows[j]["Allowance_Id"].ToString() != "0" && dtAllowance.Rows[j]["Allowance_Id"].ToString() != "")
                            {

                                DataTable dtAllowancename = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), dtAllowance.Rows[j]["Allowance_Id"].ToString());

                                dtAllowancename = new DataView(dtAllowancename, "Allowance_Id=" + dtAllowance.Rows[j]["Allowance_Id"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtAllowancename.Rows.Count > 0)
                                {
                                    dr["AllowanceName"] = dtAllowancename.Rows[0]["Allowance"].ToString();
                                }
                                else
                                {
                                    dr["AllowanceName"] = "";
                                }

                            }
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();


                            }

                            try
                            {

                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();
                                dr["EmpImageUrl"] = EmpImageurl;

                            }
                            catch
                            {


                            }

                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();
                                double worksal = 0;
                                double leavesal = 0;
                                double holidayssal = 0;
                                double weekofsal = 0;
                                try
                                {
                                    worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                }
                                catch
                                {
                                    worksal = 0;
                                }
                                try
                                {
                                    leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                }
                                catch
                                {
                                    leavesal = 0;
                                }
                                try
                                {
                                    holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                }
                                catch
                                {
                                    holidayssal = 0;
                                }
                                try
                                {
                                    weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                }
                                catch
                                {
                                    weekofsal = 0;
                                }
                                // worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {
                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }

                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");

                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                            dtEmployeePaySlip.Rows.Add(dr);

                        }



                    }
                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = Resources.Attendance.Allowance;
                        dr["Basic"] = basicsal;
                        dr["AllowanceActAmt"] = "0";
                        dr["Emp_Id"] = str;
                        dr["Dt_Type"] = 2;
                        dr["AllowanceName"] = "No Allowances";
                        dr["EmpImageUrl"] = EmpImageurl;
                        try
                        {

                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();
                        }
                        catch
                        {

                        }
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {

                            dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                            dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                            dr["Emp_Id"] = str;

                        }


                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            //  worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }


                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                        dtEmployeePaySlip.Rows.Add(dr);


                    }



                    if (dtClaim.Rows.Count > 0)
                    {
                        for (int c = 0; c < dtClaim.Rows.Count; c++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            dr["TypeAllow"] = Resources.Attendance.Claim;
                            dr["Basic"] = basicsal;
                            dr["Dt_Type"] = 3;
                            dr["AllowanceName"] = dtClaim.Rows[c]["Claim_Name"].ToString();

                            dr["EmpImageUrl"] = EmpImageurl;
                            dr["Emp_Id"] = str;

                            if (dtClaim.Rows[c]["Value_Type"].ToString() == "1")
                            {
                                if (dtClaim.Rows[c]["Field2"].ToString() != "")
                                {
                                    Double h = Convert.ToDouble(dtClaim.Rows[c]["Field2"]);
                                    dr["AllowanceActAmt"] = dtClaim.Rows[c]["Field2"].ToString();

                                    sumclaim += h;
                                }
                                else
                                {
                                    dr["AllowanceActAmt"] = "0";

                                    sumclaim += 0;
                                }
                            }
                            else
                            {
                                if (dtClaim.Rows[c]["Field2"].ToString() != "")
                                {

                                    double val = Convert.ToDouble((Convert.ToDouble(dtClaim.Rows[c]["Field2"].ToString())));
                                    dr["AllowanceActAmt"] = val.ToString();
                                    sumclaim += val;
                                }
                                else
                                {
                                    dr["AllowanceActAmt"] = "0";
                                    sumclaim += 0;
                                }
                            }

                            try
                            {

                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();

                            }
                            catch
                            {

                            }
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                                double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                //  worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {

                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }




                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;

                            }


                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");

                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();
                            //dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            //dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            //dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            //dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            //dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            //totalDeducPenaLoan = sumdeduction + sumpenalty + sumloan;

                            //netsalary = totalAllowClaim - totalDeducPenaLoan + (prvmonthbal);

                            //dr["NetSalary"] = netsalary.ToString();

                            dtEmployeePaySlip.Rows.Add(dr);
                        }
                    }

                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = Resources.Attendance.Claim;
                        dr["AllowanceActAmt"] = "0";
                        dr["AllowanceName"] = "No Claims";
                        dr["Dt_Type"] = 3;
                        dr["Basic"] = basicsal;
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["Emp_Id"] = str;
                        try
                        {

                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;

                            }



                        }
                        catch
                        {

                        }


                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            // worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {

                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }

                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                        dtEmployeePaySlip.Rows.Add(dr);


                    }



                    if (dtPenalty.Rows.Count > 0)
                    {
                        for (int p = 0; p < dtPenalty.Rows.Count; p++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            dr["TypeAllow"] = Resources.Attendance.Penalty;
                            dr["Basic"] = basicsal;
                            dr["Dt_Type"] = 4;
                            dr["AllowanceName"] = dtPenalty.Rows[p]["Penalty_Name"].ToString();
                            dr["EmpImageUrl"] = EmpImageurl;
                            dr["Emp_Id"] = str;
                            if (dtPenalty.Rows[p]["Value_Type"].ToString() == "1")
                            {
                                if (dtPenalty.Rows[p]["Field2"].ToString() != "")
                                {
                                    dr["AllowanceActAmt"] = dtPenalty.Rows[p]["Field2"].ToString();
                                    Double tp = Convert.ToDouble(dtPenalty.Rows[p]["Field2"].ToString());

                                    sumpenalty += tp;
                                }
                                else
                                {
                                    dr["AllowanceActAmt"] = "0";
                                    sumpenalty += 0;
                                }
                            }
                            else
                            {
                                if (dtPenalty.Rows[p]["Field2"].ToString() != "")
                                {
                                    double val = Convert.ToDouble((Convert.ToDouble(dtPenalty.Rows[p]["Field2"].ToString())));
                                    sumpenalty += val;
                                    dr["AllowanceActAmt"] = val.ToString();
                                }
                                else
                                {
                                    dr["AllowanceActAmt"] = "0";
                                }
                            }

                            try
                            {

                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();


                            }
                            catch
                            {


                            }

                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                                double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                // worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {

                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;
                            }


                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");

                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                            dtEmployeePaySlip.Rows.Add(dr);


                        }

                    }

                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = Resources.Attendance.Penalty;
                        dr["Basic"] = basicsal;
                        dr["Dt_Type"] = 4;
                        dr["AllowanceActAmt"] = "0";
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["AllowanceName"] = "No Penalties";
                        dr["Emp_Id"] = str;
                        try
                        {

                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;
                            }


                        }
                        catch
                        {

                        }


                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            //worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }

                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();



                        dtEmployeePaySlip.Rows.Add(dr);


                    }


                    dtLoan = new DataView(dtLoan, " Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtLoan.Rows.Count > 0)
                    {
                        for (int l = 0; l < dtLoan.Rows.Count; l++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            DataTable dtloandetial = new DataTable();

                            dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(dtLoan.Rows[l]["Loan_Id"].ToString());
                            dtloandetial = new DataView(dtloandetial, "Loan_Id=" + dtLoan.Rows[l]["Loan_Id"].ToString() + " and Month=" + Session["Month"].ToString() + " and Year=" + Session["Year"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();

                            if (dtloandetial.Rows.Count > 0)
                            {
                                dr["Dt_Type"] = 5;
                                dr["TypeAllow"] = Resources.Attendance.Loan;
                                dr["Basic"] = basicsal;
                                dr["EmpImageUrl"] = EmpImageurl;
                                dr["Emp_Id"] = str;
                                dr["AllowanceName"] = dtLoan.Rows[l]["Loan_Name"].ToString();
                                dr["AllowanceActAmt"] = dtloandetial.Rows[0]["Total_Amount"].ToString();

                                if (dtloandetial.Rows[0]["Employee_Paid"].ToString() != "" && dtloandetial.Rows[0]["Employee_Paid"].ToString() != "0.000")
                                {
                                    double loan = Convert.ToDouble(dtloandetial.Rows[0]["Employee_Paid"].ToString());


                                    sumloan += loan;
                                    dr["AllowanceActAmt"] = loan.ToString();

                                }
                                else
                                {
                                    dr["AllowanceActAmt"] = 0;
                                    sumloan += 0;
                                }


                            }
                            else
                            {
                                dr["TypeAllow"] = Resources.Attendance.Loan;
                                dr["Basic"] = basicsal;
                                dr["AllowanceName"] = dtLoan.Rows[l]["Loan_Name"].ToString();
                                dr["AllowanceActAmt"] = "0";
                                dr["Emp_Id"] = str;
                            }

                            try
                            {
                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();

                            }

                            catch
                            {

                            }

                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                                double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                //  worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {
                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }

                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;

                            }


                            // double latesal = Convert.ToDouble(dtEmpAttendance.Rows[0]["Late_Min_Penalty"].ToString());
                            // double earlysal = Convert.ToDouble(dtEmpAttendance.Rows[0]["Early_Min_Penalty"].ToString());
                            // double partialsal = Convert.ToDouble(dtEmpAttendance.Rows[0]["Parital_Violation_Penalty"].ToString());
                            // double absentsal = Convert.ToDouble(dtEmpAttendance.Rows[0]["Absent_Day_Penalty"].ToString());

                            //totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();
                            dr["GrossAttendanceSal"] = grossattendance.ToString();


                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                            dtEmployeePaySlip.Rows.Add(dr);


                        }

                    }

                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = Resources.Attendance.Loan;
                        dr["Basic"] = basicsal;
                        dr["Dt_Type"] = 5;
                        dr["AllowanceActAmt"] = "0";
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["AllowanceName"] = "No Loans";
                        dr["Emp_Id"] = str;
                        try
                        {

                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();

                        }
                        catch
                        {

                        }
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            // worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }

                        if (dtPayEmpMonth.Rows.Count > 0)
                        {

                            dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                            dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                            dr["Emp_Id"] = str;
                        }



                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                        dtEmployeePaySlip.Rows.Add(dr);


                    }


                    DataTable dtEmpPrvablmonth = new DataTable();

                    dtEmpPrvablmonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    if (dtEmpPrvablmonth.Rows.Count > 0)
                    {
                        for (int prv = 0; prv < dtEmpPrvablmonth.Rows.Count; prv++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            dr["TypeAllow"] = Resources.Attendance.Previous_Month_Adjustment;
                            dr["Basic"] = basicsal;
                            dr["EmpImageUrl"] = EmpImageurl;
                            dr["Emp_Id"] = str;
                            dr["Dt_Type"] = 6;
                            dr["AllowanceName"] = Resources.Attendance.Previous_Month_Claim_Penalty;
                            dr["AllowanceActAmt"] = pvmonthbal_claimPenalty.ToString();

                            double pr = Convert.ToDouble(dtEmpPrvablmonth.Rows[0]["Previous_Month_Balance"].ToString());
                            prvmonthbal += pr;
                            try
                            {
                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();

                            }
                            catch
                            {
                            }


                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;
                            }

                            // double latesal = Convert.ToDouble(dtEmpAttendance.Rows[0]["Late_Min_Penalty"].ToString());
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                                double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                //     worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {
                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }

                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();


                            dtEmployeePaySlip.Rows.Add(dr);


                        }

                    }
                    else
                    {
                        DataRow dr = dtEmployeePaySlip.NewRow();
                        dr["TypeAllow"] = Resources.Attendance.Previous_Month_Adjustment;
                        dr["Basic"] = basicsal;
                        dr["Dt_Type"] = 6;
                        dr["AllowanceActAmt"] = "0";
                        dr["Emp_Id"] = str;
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["AllowanceName"] = Resources.Attendance.Previous_Month_Claim_Penalty;
                        try
                        {

                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();
                            if (dtPayEmpMonth.Rows.Count > 0)
                            {

                                dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                dr["Emp_Id"] = str;
                            }


                        }
                        catch
                        {

                        }


                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            //worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }
                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();



                        dtEmployeePaySlip.Rows.Add(dr);


                    }


                    if (dtDeuction.Rows.Count > 0)
                    {

                        for (int k = 0; k < dtDeuction.Rows.Count; k++)
                        {
                            DataRow dr = dtEmployeePaySlip.NewRow();
                            dr["TypeAllow"] = Resources.Attendance.Deduction;
                            dr["Basic"] = basicsal;
                            dr["Dt_Type"] = 7;
                            dr["Emp_Id"] = str;
                            dr["EmpImageUrl"] = EmpImageurl;
                            dr["AllowanceActAmt"] = dtDeuction.Rows[k]["Act_Deduction_Value"].ToString();
                            if (dtDeuction.Rows[k]["Act_Deduction_Value"].ToString() != "")
                            {
                                double deduc = Convert.ToDouble(dtDeuction.Rows[k]["Act_Deduction_Value"].ToString());
                                sumdeduction += deduc;
                            }

                            if (dtDeuction.Rows[k]["Deduction_Id"].ToString() != "0" && dtDeuction.Rows[k]["Deduction_Id"].ToString() != "")
                            {
                                DataTable dtDeductionname = objDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), dtDeuction.Rows[k]["Deduction_Id"].ToString());

                                dtDeductionname = new DataView(dtDeductionname, "Deduction_Id=" + dtDeuction.Rows[k]["Deduction_Id"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtDeductionname.Rows.Count > 0)
                                {
                                    dr["AllowanceName"] = dtDeductionname.Rows[0]["Deduction"].ToString();
                                }
                                else
                                {
                                    dr["AllowanceName"] = "";
                                }

                            }
                            try
                            {

                                dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                                dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                                dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                                dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                                dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                                dr["Totaldays"] = totaldays.ToString();
                                if (dtPayEmpMonth.Rows.Count > 0)
                                {

                                    dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                                    dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                                    dr["Emp_Id"] = str;
                                }


                            }
                            catch
                            {

                            }


                            if (dtPayEmpMonth.Rows.Count > 0)
                            {
                                dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                                dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                                dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                                dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                                double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                                double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                                double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                                double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                                //worksal = worksal - leavesal - holidayssal - weekofsal;
                                dr["WorkedSal"] = worksal.ToString();
                                try
                                {
                                    dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                                }
                                catch
                                {
                                }

                                totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                                dr["totalAttendSal"] = totalattndsal.ToString();

                                dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                                double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                                double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                                double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                                totalOTsal = noramlotsal + weekotsal + holidaotsal;
                                dr["TotalOTSal"] = totalOTsal.ToString();


                                dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                                dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                                dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                                dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                                double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                                double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                                double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                                double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                                totalpenasal = latesal + earlysal + partialsal + absentsal;
                                dr["TotalPenaltySal"] = totalpenasal.ToString();

                                grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                                dr["GrossAttendanceSal"] = grossattendance.ToString();

                            }

                            dr["EmpCode"] = EmployeeCode;
                            dr["EmpName"] = EmployeeName;
                            DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                            dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                            if (dtDesignation.Rows.Count > 0)
                            {
                                dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                            }
                            if (dtDepartment.Rows.Count > 0)
                            {
                                dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                            }
                            dr["Month"] = Session["Month"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();

                            totalAllowClaim = sumallowance + sumclaim + grossattendance;

                            totalDeducPenaLoan = sumdeduction + sumpenalty + sumloan;

                            netsalary = totalAllowClaim - totalDeducPenaLoan + (pvmonthbal_claimPenalty);
                            dr["NetSalary"] = netsalary.ToString();

                            dtEmployeePaySlip.Rows.Add(dr);

                        }

                    }
                    else
                    {

                        DataRow dr = dtEmployeePaySlip.NewRow();

                        dr["Dt_Type"] = 7;
                        dr["TypeAllow"] = Resources.Attendance.Deduction;
                        dr["Basic"] = basicsal;
                        dr["AllowanceActAmt"] = "0";
                        dr["EmpImageUrl"] = EmpImageurl;
                        dr["Emp_Id"] = str;
                        dr["AllowanceName"] = "No Deductions";
                        try
                        {
                            dr["DaysPresent"] = dtEmpAttendance.Rows[0]["Worked_Days"].ToString();
                            dr["WeekOff"] = dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString();
                            dr["daysAbsent"] = dtEmpAttendance.Rows[0]["Absent_Days"].ToString();
                            dr["Holiday"] = dtEmpAttendance.Rows[0]["Holiday_Days"].ToString();
                            dr["Leaves"] = dtEmpAttendance.Rows[0]["Leave_Days"].ToString();
                            dr["Totaldays"] = totaldays.ToString();


                        }
                        catch
                        {

                        }
                        if (dtPayEmpMonth.Rows.Count > 0)
                        {

                            dr["PF"] = dtPayEmpMonth.Rows[0]["Employee_PF"].ToString();
                            dr["ESIC"] = dtPayEmpMonth.Rows[0]["Employee_ESIC"].ToString();
                            dr["Emp_Id"] = str;
                        }

                        if (dtPayEmpMonth.Rows.Count > 0)
                        {
                            dr["WorkedSal"] = dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString();
                            dr["LeavedaysSal"] = dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString();
                            dr["HolidaysSal"] = dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString();
                            dr["WeekOffSal"] = dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString();

                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());
                            //worksal = worksal - leavesal - holidayssal - weekofsal;
                            dr["WorkedSal"] = worksal.ToString();
                            try
                            {
                                dtEmpAttendance.Rows[0]["Basic_Work_Salary"] = worksal.ToString();
                            }
                            catch
                            {
                            }

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;
                            dr["totalAttendSal"] = totalattndsal.ToString();

                            dr["NormalOTSal"] = dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString();
                            dr["WeekOffOTSal"] = dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                            dr["HolidaysOTSal"] = dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString();

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;
                            dr["TotalOTSal"] = totalOTsal.ToString();


                            dr["LatePenaSal"] = dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString();
                            dr["EarlyPenaSal"] = dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString();
                            dr["PartialPenaSal"] = dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString();
                            dr["AbsentPenaSal"] = dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString();

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            dr["TotalPenaltySal"] = totalpenasal.ToString();

                            grossattendance = (totalattndsal + totalOTsal) - totalpenasal;

                            dr["GrossAttendanceSal"] = grossattendance.ToString();

                        }

                        dr["EmpCode"] = EmployeeCode;
                        dr["EmpName"] = EmployeeName;
                        DateTime dt = Convert.ToDateTime(dtEmpInfo.Rows[0]["DOJ"].ToString());
                        dr["DOJ"] = dt.ToString("dd-MMM-yyyy");
                        if (dtDesignation.Rows.Count > 0)
                        {
                            dr["Designation"] = dtDesignation.Rows[0]["Designation"].ToString();
                        }
                        if (dtDepartment.Rows.Count > 0)
                        {
                            dr["Department"] = dtDepartment.Rows[0]["Dep_Name"].ToString();
                        }
                        dr["Month"] = Session["Month"].ToString();
                        dr["Year"] = Session["Year"].ToString();
                        dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();

                        totalAllowClaim = sumallowance + sumclaim + grossattendance;
                        totalDeducPenaLoan = sumdeduction + sumpenalty + sumloan;

                        netsalary = totalAllowClaim - totalDeducPenaLoan + (pvmonthbal_claimPenalty);
                        dr["NetSalary"] = netsalary.ToString();

                        dtEmployeePaySlip.Rows.Add(dr);
                    }


                }



            }


        }








        DataTable dtEmpsSlip = dtEmployeePaySlip;
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string CompanyContact = "";
        //string EmpImageurl = "";
        string Mailid = "";
        string Websitename = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        DataTable dtEmpmaster = new DataTable();
        if (dtEmpsSlip != null && dtEmpsSlip.Rows.Count > 0)
            dtEmpmaster = objEmpmaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), dtEmpsSlip.Rows[0]["EmpCode"].ToString());

        if (DtCompany.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();
            }

            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();

        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            CompanyContact = DtAddress.Rows[0]["PhoneNo1"].ToString();
            Mailid = DtAddress.Rows[0]["EmailId1"].ToString();
            Websitename = DtAddress.Rows[0]["WebSite"].ToString();

        }

        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            else
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            else
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
        }        
            objEmpPayslip.setBrandName(BrandName);
            objEmpPayslip.setLocationName(LocationName);
            objEmpPayslip.setcompanyAddress(CompanyAddress);
            objEmpPayslip.setcompanyname(CompanyName);
            //objEmpPayslip.setempimage(EmpImageurl);
            objEmpPayslip.setmailid(Mailid);
            objEmpPayslip.setwebsite(Websitename);
            objEmpPayslip.setimage(Imageurl);
            objEmpPayslip.setcontact(CompanyContact);
            objEmpPayslip.setUserName(Session["UserId"].ToString());
            objEmpPayslip.setUserName1(Session["UserId"].ToString());
            objEmpPayslip.Setheader(Resources.Attendance.Created_By, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Date_of_Joining, Resources.Attendance.BANK_A_C_NO_, Resources.Attendance.Attendance1, Resources.Attendance.Attendance1, Resources.Attendance.Days_Present, Resources.Attendance.Week_Off, Resources.Attendance.Days_absent, Resources.Attendance.Holiday, Resources.Attendance.Leave, Resources.Attendance.Basic_Salary, Resources.Attendance.Attendance_Salary, Resources.Attendance.Over_Time_Salary, Resources.Attendance.Penalty, Resources.Attendance.Work_Days, Resources.Attendance.Normal_Days_OT, Resources.Attendance.Week_Off_OT, Resources.Attendance.Holiday_OT, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Partial, Resources.Attendance.Absent, Resources.Attendance.Total, Resources.Attendance.Gross_Pay, Resources.Attendance.PF, Resources.Attendance.ESIC, Resources.Attendance.Net_Salary, Resources.Attendance.HR_SIGNATURE, Resources.Attendance.EMPLOYEE_SIGNATURE, Resources.Attendance.PAYSLIP_FOR_THE_MONTH_OF + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString(), Resources.Attendance.Employee_copy, Resources.Attendance.Employer_copy, Resources.Attendance.Total_Days);
            string Type = "";
            string attendrecord = "";
            DataTable dt1 = new DataTable();
            dt1 = dtEmpsSlip;
            DataTable dtrecord = new DataTable();
            dtrecord = dtEmpsSlip;
            string amount = "";
            dt1 = new DataView(dt1, "TypeAllow<>'" + amount.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //this code is modified on 02-04-2014 by jitendra upadhyay
            //for convert the amount in selected currency by employee and  company
            //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
            //this function return the amount after currency conversion
            //code start
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string CurrencySymbol = string.Empty;
                CurrencySymbol = objSys.GetCurencySymbol(dt1.Rows[i]["Emp_Id"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                dt1.Rows[i]["Currency_Symbol"] = CurrencySymbol;
                dt1.Rows[i]["Basic"] = CurrencySymbol + objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["Basic"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["AllowanceActAmt"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["AllowanceActAmt"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["NetSalary"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["NetSalary"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["WorkedSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["WorkedSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["WeekOffSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["WeekOffSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["HolidaysSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["HolidaysSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["LeavedaysSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["LeavedaysSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["NormalOTSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["NormalOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["WeekOffOTSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["WeekOffOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["HolidaysOTSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["HolidaysOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["LatePenaSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["LatePenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["EarlyPenaSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["EarlyPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["PartialPenaSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["PartialPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["AbsentPenaSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["AbsentPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["GrossAttendanceSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["GrossAttendanceSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["totalAttendSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["totalAttendSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["TotalOTSal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["TotalOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["TotalPenaltySal"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["TotalPenaltySal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["TotalAdjustmentAmt"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["TotalAdjustmentAmt"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["PF"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["PF"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                dt1.Rows[i]["ESIC"] = objSys.GetCurencyConversion(dt1.Rows[i]["Emp_Id"].ToString(), dt1.Rows[i]["ESIC"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            }
            //code end       
            objEmpPayslip.DataSource = dt1;
            objEmpPayslip.DataMember = "dtEmpPayslip";
            ReportViewer1.Report = objEmpPayslip;
            ReportToolbar1.ReportViewer = ReportViewer1;
            //CultureInfo ci = CultureInfo.CreateSpecificCulture("SA");
            //Thread.CurrentThread.CurrentCulture = ci;
            //Thread.CurrentThread.CurrentUICulture = ci;
        
    }
}