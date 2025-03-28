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
using PegasusDataAccess;

public partial class HR_Report_Employee_Pay_Slip : BasePage
{
    DataAccessClass daClass = null;
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
    Employee_Pay_Slip_Report_New objEmpPayslip = null;
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

        daClass = new DataAccessClass(Session["DBConnection"].ToString());
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
        objEmpPayslip = new Employee_Pay_Slip_Report_New(Session["DBConnection"].ToString());
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
            if (Session["UserId"] != null)
            {
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "144", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
            }
            else
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
    public void getrecoerd()
    {
        string Imageurl = "";
        string LocationName = string.Empty;
        DateTime Date = DateTime.Now;
        string EmpIds = string.Empty;
        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "20");
        string Currency_ID = Common.Get_Location_Currency_ID(Session["CompId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        foreach (DataRow dr in dtEmpNF.Rows)
        {
            EmpIds += dr["Emp_Id"] + ",";
        }

        DataTable Dt_Pay_Slip = new DataTable();
        Dt_Pay_Slip.Columns.Add("Emp_Id");
        Dt_Pay_Slip.Columns.Add("Emp_Code");
        Dt_Pay_Slip.Columns.Add("Emp_Name");
        Dt_Pay_Slip.Columns.Add("Basic_Salary");
        Dt_Pay_Slip.Columns.Add("Designation");
        Dt_Pay_Slip.Columns.Add("Dep_Name");
        Dt_Pay_Slip.Columns.Add("Account_No");
        Dt_Pay_Slip.Columns.Add("Bank_Name");
        Dt_Pay_Slip.Columns.Add("Allowance");
        Dt_Pay_Slip.Columns.Add("Act_Allowance_Value");
        Dt_Pay_Slip.Columns.Add("Late_Min_Penalty");
        Dt_Pay_Slip.Columns.Add("Early_Min_Penalty");
        Dt_Pay_Slip.Columns.Add("Patial_Violation_Penalty");
        Dt_Pay_Slip.Columns.Add("Employee_Penalty");
        Dt_Pay_Slip.Columns.Add("Employee_Claim");
        Dt_Pay_Slip.Columns.Add("Emlployee_Loan");
        Dt_Pay_Slip.Columns.Add("Total_Allowance");
        Dt_Pay_Slip.Columns.Add("Total_Deduction");
        Dt_Pay_Slip.Columns.Add("Previous_Month_Balance");
        Dt_Pay_Slip.Columns.Add("Employee_PF");
        Dt_Pay_Slip.Columns.Add("Employer_PF");
        Dt_Pay_Slip.Columns.Add("Employee_ESIC");
        Dt_Pay_Slip.Columns.Add("Employer_ESIC");
        Dt_Pay_Slip.Columns.Add("Deduction");
        Dt_Pay_Slip.Columns.Add("Deduction_Value");
        Dt_Pay_Slip.Columns.Add("S_No");
        Dt_Pay_Slip.Columns.Add("totaldays");
        Dt_Pay_Slip.Columns.Add("dayspres");
        Dt_Pay_Slip.Columns.Add("Tot_Allowance");
        Dt_Pay_Slip.Columns.Add("Tot_Deduction");
        Dt_Pay_Slip.Columns.Add("Net_Salary");
        

        foreach (string str in EmpIds.Split(','))
        {
            if ((str != ""))
            {

               DataTable Dt_Earning = objPEpenalty.Get_Pay_Slip_Record(str,Session["Month"].ToString(), Session["Year"].ToString(), Session["CompId"].ToString(), "1");
                DataTable Dt_Deduction = objPEpenalty.Get_Pay_Slip_Record(str,Session["Month"].ToString(), Session["Year"].ToString(), Session["CompId"].ToString(), "2");

                DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
                if (DtLocation.Rows.Count > 0)
                {
                    if (Session["Lang"].ToString() == "1")
                        LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
                    else
                        LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
                }

                string Comp_ID = string.Empty;
                DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
                if (DtCompany.Rows.Count > 0)
                {
                    Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
                }

                DataTable Dt_New = Dt_Earning;
                Dt_New.Columns.Add("Deduction");
                Dt_New.Columns.Add("Deduction_Value");
                int Earning_Row = Dt_Earning.Rows.Count;
                int Deduction_Row = Dt_Deduction.Rows.Count;
                int i = 0;
                foreach (DataRow DR in Dt_Deduction.Rows)
                {
                    if (i >= Earning_Row)
                    {
                        Dt_New.Rows.Add();
                    }
                    Dt_Earning.Rows[i]["Emp_Id"] = DR["Emp_Id"].ToString();
                    Dt_Earning.Rows[i]["Deduction"] = DR["Deduction"].ToString();
                    Dt_Earning.Rows[i]["Deduction_Value"] = DR["Deduction_Value"].ToString();
                    i++;
                }

                double Tot_Allowance = 0;
                double Tot_Deduction = 0;
                double NetSalary = 0;
                string Net_Salary = "0";
                foreach (DataRow DR in Dt_Earning.Rows)
                {
                    if (DR["Act_Allowance_Value"].ToString() == "")
                        Tot_Allowance += 0;
                    else
                        Tot_Allowance += Convert.ToDouble(DR["Act_Allowance_Value"].ToString());

                    if (DR["Deduction_Value"].ToString() == "")
                        Tot_Deduction += 0;
                    else
                        Tot_Deduction += Convert.ToDouble(DR["Deduction_Value"].ToString());


                    NetSalary = Tot_Allowance - Tot_Deduction;
                    Net_Salary = objSys.GetCurencyConversionForInv(Currency_ID.ToString(), NetSalary.ToString());
                    DR["Act_Allowance_Value"] = Common.GetAmountDecimal_By_Location(DR["Act_Allowance_Value"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()).ToString();
                    DR["Deduction_Value"] = Common.GetAmountDecimal_By_Location(DR["Deduction_Value"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()).ToString();
                }
                double totaldays = 0;
                double dayspres = 0;
                DataTable dtEmpAttendance = objEmpAttendance.GetRecord_Emp_Attendance(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                if (dtEmpAttendance != null && dtEmpAttendance.Rows.Count > 0)
                {
                    dayspres = Convert.ToDouble(dtEmpAttendance.Rows[0]["Worked_Days"].ToString());
                    double weekofdays = Convert.ToDouble(dtEmpAttendance.Rows[0]["Week_Off_Days"].ToString());
                    double daysabsent = Convert.ToDouble(dtEmpAttendance.Rows[0]["Absent_Days"].ToString());
                    double holidays = Convert.ToDouble(dtEmpAttendance.Rows[0]["Holiday_Days"].ToString());
                    double daysleave = Convert.ToDouble(dtEmpAttendance.Rows[0]["Leave_Days"].ToString());
                    totaldays = dayspres + weekofdays + daysabsent + daysleave + holidays;
                }
                
                Dt_Earning.Columns.Add("S_No");
                int j = 1;
                foreach (DataRow DR in Dt_Earning.Rows)
                {
                    DR["S_No"] = j;
                    j++;
                }

                foreach(DataRow DVR in Dt_Earning.Rows)
                {
                    DataRow DPS = Dt_Pay_Slip.NewRow();
                    DPS["Emp_Id"] = DVR["Emp_Id"].ToString();
                    DPS["Emp_Code"]= DVR["Emp_Code"].ToString();
                    DPS["Emp_Name"]= DVR["Emp_Name"].ToString();
                    DPS["Basic_Salary"]= DVR["Basic_Salary"].ToString();
                    DPS["Designation"]= DVR["Designation"].ToString();
                    DPS["Dep_Name"]= DVR["Dep_Name"].ToString();
                    DPS["Account_No"]= DVR["Account_No"].ToString();
                    DPS["Bank_Name"]= DVR["Bank_Name"].ToString();
                    if (DVR["Allowance"].ToString() != "")
                    {
                        DPS["Allowance"] = DVR["Allowance"].ToString();
                        DPS["Act_Allowance_Value"] = DVR["Act_Allowance_Value"].ToString();
                    }
                    else
                    {
                        DPS["Allowance"] = "";
                        DPS["Act_Allowance_Value"] = "";
                    }
                    DPS["Late_Min_Penalty"]= DVR["Late_Min_Penalty"].ToString();
                    DPS["Early_Min_Penalty"]= DVR["Early_Min_Penalty"].ToString();
                    DPS["Patial_Violation_Penalty"]= DVR["Patial_Violation_Penalty"].ToString();
                    DPS["Employee_Penalty"]= DVR["Employee_Penalty"].ToString();
                    DPS["Employee_Claim"]= DVR["Employee_Claim"].ToString();
                    DPS["Emlployee_Loan"]= DVR["Emlployee_Loan"].ToString();
                    DPS["Total_Allowance"]= DVR["Total_Allowance"].ToString();
                    DPS["Total_Deduction"]= DVR["Total_Deduction"].ToString();
                    DPS["Previous_Month_Balance"]= DVR["Previous_Month_Balance"].ToString();
                    DPS["Employee_PF"]= DVR["Employee_PF"].ToString();
                    DPS["Employer_PF"]= DVR["Employer_PF"].ToString();
                    DPS["Employee_ESIC"]= DVR["Employee_ESIC"].ToString();
                    DPS["Employer_ESIC"]= DVR["Employer_ESIC"].ToString();
                    if (DVR["Deduction"].ToString() != "")
                    {
                        DPS["Deduction"] = DVR["Deduction"].ToString();
                        DPS["Deduction_Value"] = DVR["Deduction_Value"].ToString();
                    }
                    else
                    {
                        DPS["Deduction"] = "";
                        DPS["Deduction_Value"] = "";
                    }
                    DPS["S_No"]= DVR["S_No"].ToString();
                    DPS["totaldays"] = totaldays.ToString();
                    DPS["dayspres"] = dayspres.ToString();
                    DPS["Tot_Allowance"] = Common.GetAmountDecimal_By_Location(Tot_Allowance.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    DPS["Tot_Deduction"] = Common.GetAmountDecimal_By_Location(Tot_Deduction.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    DPS["Net_Salary"] = Common.Get_Roundoff_Amount_By_Location(Net_Salary.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    Dt_Pay_Slip.Rows.Add(DPS);
                }
            }
        }


        //objEmpPayslip.Net_Amount(totaldays.ToString(), dayspres.ToString(), Common.GetAmountDecimal_By_Location(Tot_Allowance.ToString()), Common.GetAmountDecimal_By_Location(Tot_Deduction.ToString()), Common.GetAmountDecimal_By_Location(Net_Salary.ToString()));
        objEmpPayslip.Company_ID(Session["CompId"].ToString());
        objEmpPayslip.setLocationName(LocationName);        
        objEmpPayslip.setimage(Imageurl);
        objEmpPayslip.Setheader(Resources.Attendance.Created_By, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Date_of_Joining, Resources.Attendance.BANK_A_C_NO_, Resources.Attendance.Attendance1, Resources.Attendance.Attendance1, Resources.Attendance.Days_Present, Resources.Attendance.Week_Off, Resources.Attendance.Days_absent, Resources.Attendance.Holiday, Resources.Attendance.Leave, Resources.Attendance.Basic_Salary, Resources.Attendance.Attendance_Salary, Resources.Attendance.Over_Time_Salary, Resources.Attendance.Penalty, Resources.Attendance.Work_Days, Resources.Attendance.Normal_Days_OT, Resources.Attendance.Week_Off_OT, Resources.Attendance.Holiday_OT, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Partial, Resources.Attendance.Absent, Resources.Attendance.Total, Resources.Attendance.Gross_Pay, Resources.Attendance.PF, Resources.Attendance.ESIC, Resources.Attendance.Net_Salary, Resources.Attendance.HR_SIGNATURE, Resources.Attendance.EMPLOYEE_SIGNATURE, "Salary Slip for the month of " + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString(), Resources.Attendance.Employee_copy, Resources.Attendance.Employer_copy, Resources.Attendance.Total_Days);        
        objEmpPayslip.DataSource = Dt_Pay_Slip;
        objEmpPayslip.DataMember = "Dt_Earning";
        ReportViewer1.Report = objEmpPayslip;
        ReportToolbar1.ReportViewer = ReportViewer1;
    }
}