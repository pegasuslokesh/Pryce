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
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.IO;

public partial class Reports_PayrollSummayReport : BasePage
{
    SystemParameter objSys = null;
    Pay_SummaryReport Objreport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    Pay_Employee_Loan ObjLoan = null;
    EmployeeMaster objEmp = null;
    Pay_Employee_Month objPayEmpMonth = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        Objreport = new Pay_SummaryReport(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();

        if (!IsPostBack)
        {

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "152", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            if (Request.QueryString["Value"] != null)
            {
                if (Request.QueryString["Value"].ToString() != null)
                {

                    string Value = Request.QueryString["Value"].ToString();
                    ddlPayrollType.SelectedValue = Value;
                }
            }
            ddlPayroll_OnSelectedIndexChanged(null, null);

        }

        

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


    protected void ddlPayroll_OnSelectedIndexChanged(object sender, EventArgs e)
    {


        GetReport();

    }

    void GetReport()
    {
        string[] month = new string[13];
        month[0] = "--Select--";
        month[1] = "JAN'";
        month[2] = "FEB'";
        month[3] = "MAR'";
        month[4] = "APR'";
        month[5] = "MAY'";
        month[6] = "JUN'";
        month[7] = "JUL'";
        month[8] = "AUG'";
        month[9] = "SEP'";
        month[10] = "OCT'";
        month[11] = "NOV'";
        month[12] = "DEC'";
        DataTable DtClaimRecord = new DataTable();




        if (ddlPayrollType.SelectedValue == "1")
        {

            DtClaimRecord = objPayEmpMonth.GetPayTempData(Session["CompId"].ToString());
        }
        else
        {

            DtClaimRecord = objPayEmpMonth.GetPayData(Session["CompId"].ToString());
        }


        if (Session["Month"] != null)
        {
            DtClaimRecord = new DataView(DtClaimRecord, "Month=" + Session["Month"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Session["Year"] != null)
        {
            DtClaimRecord = new DataView(DtClaimRecord, "Year=" + Session["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }


        DataTable dt = new DataTable();
        dt.Columns.Add("Emp_Id");
        dt.Columns.Add("EmpCode");
        dt.Columns.Add("EmpName");
        dt.Columns.Add("Present");
        dt.Columns.Add("Absent");
        dt.Columns.Add("Basic");

        dt.Columns.Add("WorkSalary");
        dt.Columns.Add("AttPenalty");
        dt.Columns.Add("Overtime");
        dt.Columns.Add("AttTotal");
        dt.Columns.Add("Allowance");
        dt.Columns.Add("Claim");
        dt.Columns.Add("AllowanceTotal");
        dt.Columns.Add("Deduction");
        dt.Columns.Add("DeductionPenalty");
        dt.Columns.Add("DeductionTotal");
        dt.Columns.Add("GrossTotal");
        dt.Columns.Add("PF");
        dt.Columns.Add("ESIC");
        dt.Columns.Add("TotalGrossAmount");
        dt.Columns.Add("TotalGrossAmount_EmployeeCurrency");
        dt.Columns.Add("Currency_Symbol");
        dt.Columns.Add("Employee_Loan");
        dt.Columns.Add("Previous_Month_Balance");


        double basicsal = 0;


        double Employee_PF = 0;

        double Employee_ESIC = 0;


        double PF = 0;
        double ESIC = 0;



        bool IsPF = false;
        bool IsESIC = false;

        bool IsEmpINPayroll = false;

        try
        {

            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }

        try
        {
            Employee_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }




        string Id = string.Empty;
        string EmpReport = string.Empty;


        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start
        EmpReport = Session["Querystring"].ToString();


        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "21");


        foreach (DataRow dr in dtEmpNF.Rows)
        {
            Id += dr["Emp_Id"] + ",";
        }

        //code end



        //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
        //{
        //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

        //    if (dtEmpNF.Rows.Count > 0)
        //    {
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report2"].ToString()))
        //        {
        //            Id += EmpReport.Split(',')[i].ToString() + ",";
        //        }
        //    }
        //}

        //code end


        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                basicsal = 0;
                DataTable dtClaimRecord = DtClaimRecord;
                dtClaimRecord = new DataView(dtClaimRecord, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtClaimRecord.Rows.Count > 0)
                {
                    string[] Montharr = new string[12];


                    for (int i = 0; i < dtClaimRecord.Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["EmpCode"] = GetEmployeeCode(dtClaimRecord.Rows[i]["Emp_Id"].ToString());
                        dr["EmpName"] = GetEmployeeName(dtClaimRecord.Rows[i]["Emp_Id"].ToString());
                        dr["Emp_Id"] = str;


                        dr["Previous_Month_Balance"] = objSys.GetCurencyConversion(str, dtClaimRecord.Rows[i]["Previous_Month_Balance"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());


                        int PresentDays = 0;

                        try
                        {
                            PresentDays = int.Parse(dtClaimRecord.Rows[i]["Worked_Days"].ToString()) + int.Parse(dtClaimRecord.Rows[i]["Week_Off_Days"].ToString()) + int.Parse(dtClaimRecord.Rows[i]["Holiday_Days"].ToString()) + int.Parse(dtClaimRecord.Rows[i]["Leave_Days"].ToString());
                        }
                        catch
                        {

                        }



                        dr["Present"] = PresentDays;



                        dr["Absent"] = dtClaimRecord.Rows[i]["Absent_Days"].ToString();


                        DataTable dtemppara = new DataTable();
                        dtemppara = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                        if (dtemppara.Rows.Count > 0)
                        {

                            basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                        }
                        if (ddlPayrollType.SelectedValue.Trim() == "1")
                        {
                            dr["Basic"] = basicsal.ToString();
                        }
                        else
                        {
                            try
                            {
                                dr["Basic"] = double.Parse(dtClaimRecord.Rows[i]["Field6"].ToString());
                            }
                            catch
                            {
                                dr["Basic"] = basicsal.ToString();
                            }


                        }

                        double WorkSal = 0;

                        WorkSal = double.Parse(dtClaimRecord.Rows[i]["Worked_Min_Salary"].ToString());

                        WorkSal = WorkSal + double.Parse(dtClaimRecord.Rows[i]["Leave_Days_Salary"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Week_Off_Salary"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Holidays_Salary"].ToString());

                        dr["WorkSalary"] = WorkSal.ToString();




                        double AttPenalty = 0;
                        double AbsentPenalty = 0;
                        if (dtClaimRecord.Rows[i]["Absent_Salary"].ToString() != "")
                        {
                            AbsentPenalty = double.Parse(dtClaimRecord.Rows[i]["Absent_Salary"].ToString());
                        }

                        try
                        {
                            AttPenalty = double.Parse(dtClaimRecord.Rows[i]["Late_Min_Penalty"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Early_Min_Penalty"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Patial_Violation_Penalty"].ToString()) + AbsentPenalty;
                        }
                        catch
                        {
                        }




                        dr["AttPenalty"] = AttPenalty.ToString();
                        double OtSal = 0;
                        try
                        {
                            OtSal = double.Parse(dtClaimRecord.Rows[i]["Normal_OT_Min_Salary"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Holiday_OT_Min_Salary"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Week_Off_OT_Min_Salary"].ToString());
                        }
                        catch
                        {
                            try
                            {
                                OtSal = double.Parse(dtClaimRecord.Rows[i]["Normal_OT_Min_Salary"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Week_Off_OT_Min_Salary"].ToString());

                            }
                            catch
                            {

                            }

                        }
                        dr["Overtime"] = OtSal.ToString();


                        dr["AttTotal"] = (OtSal + WorkSal - AttPenalty).ToString();
                        dr["Allowance"] = dtClaimRecord.Rows[i]["Total_Allowance"].ToString();
                        dr["Claim"] = dtClaimRecord.Rows[i]["Employee_Claim"].ToString();

                        double TotalAllow = 0;
                        try
                        {
                            TotalAllow = double.Parse(dtClaimRecord.Rows[i]["Total_Allowance"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Employee_Claim"].ToString());

                        }
                        catch
                        {

                        }
                        dr["AllowanceTotal"] = TotalAllow.ToString();
                        dr["Deduction"] = dtClaimRecord.Rows[i]["Total_Deduction"].ToString();
                        dr["DeductionPenalty"] = dtClaimRecord.Rows[i]["Employee_Penalty"].ToString();


                        //




                        PF = 0;
                        ESIC = 0;



                        IsPF = false;
                        IsESIC = false;

                        IsEmpINPayroll = false;

                        DataTable dt1 = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());


                        if (dt1.Rows.Count > 0)
                        {
                            try
                            {
                                IsEmpINPayroll = Convert.ToBoolean(dt1.Rows[0]["Field6"].ToString());
                            }
                            catch
                            {

                            }
                            try
                            {
                                basicsal = double.Parse(dt1.Rows[0]["Basic_Salary"].ToString());
                            }
                            catch
                            {

                            }

                            try
                            {
                                IsPF = Convert.ToBoolean(dt1.Rows[0]["Field4"].ToString());
                            }
                            catch
                            {

                            }
                            try
                            {
                                IsESIC = Convert.ToBoolean(dt1.Rows[0]["Field5"].ToString());
                            }
                            catch
                            {

                            }
                        }


                        if (IsEmpINPayroll)
                        {
                            if (basicsal.ToString() != "")
                            {
                                if (IsPF == true)
                                {
                                    PF = (basicsal * Employee_PF) / 100;
                                }
                                if (IsESIC == true)
                                {
                                    ESIC = (basicsal * Employee_ESIC) / 100;
                                }


                            }

                        }











                        //


                        PF = Convert.ToDouble(dtClaimRecord.Rows[i]["Employee_PF"].ToString());
                        ESIC = Convert.ToDouble(dtClaimRecord.Rows[i]["Employee_ESIC"].ToString());


                        double TotalDed = 0;
                        try
                        {
                            TotalDed = double.Parse(dtClaimRecord.Rows[i]["Total_Deduction"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Emlployee_Loan"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Employee_Penalty"].ToString()) + PF + ESIC;

                        }
                        catch
                        {

                        }

                        dr["DeductionTotal"] = TotalDed.ToString();
                        dr["PF"] = PF.ToString();
                        dr["ESIC"] = ESIC.ToString();
                        dr["GrossTotal"] = double.Parse(dr["AttTotal"].ToString()) + double.Parse(dr["AllowanceTotal"].ToString()) - double.Parse(dr["DeductionTotal"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Previous_Month_Balance"].ToString());
                        dr["GrossTotal"] = Common.Get_Roundoff_Amount_By_Location(dr["GrossTotal"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        dr["TotalGrossAmount"] = double.Parse(dr["AttTotal"].ToString()) + double.Parse(dr["AllowanceTotal"].ToString()) - double.Parse(dr["DeductionTotal"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Previous_Month_Balance"].ToString());
                        dr["TotalGrossAmount_EmployeeCurrency"] = double.Parse(dr["AttTotal"].ToString()) + double.Parse(dr["AllowanceTotal"].ToString()) - double.Parse(dr["DeductionTotal"].ToString()) + double.Parse(dtClaimRecord.Rows[i]["Previous_Month_Balance"].ToString());


                        dr["TotalGrossAmount"] = Common.Get_Roundoff_Amount_By_Location(dr["TotalGrossAmount"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        dr["TotalGrossAmount_EmployeeCurrency"] = Common.Get_Roundoff_Amount_By_Location(dr["TotalGrossAmount_EmployeeCurrency"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                        dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                        dr["Employee_Loan"] = dtClaimRecord.Rows[i]["Emlployee_Loan"].ToString();
                        dt.Rows.Add(dr);



                    }
                }
                else
                {

                }


            }
        }


        //this code is modified on 02-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dt.Rows.Count; i++)
        {


            dt.Rows[i]["Basic"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Basic"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["WorkSalary"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["WorkSalary"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["AttPenalty"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["AttPenalty"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Overtime"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Overtime"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["AttTotal"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["AttTotal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Allowance"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Allowance"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Claim"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Claim"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["AllowanceTotal"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["AllowanceTotal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            dt.Rows[i]["Deduction"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Deduction"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            dt.Rows[i]["DeductionPenalty"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["DeductionPenalty"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["DeductionTotal"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["DeductionTotal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["GrossTotal"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["GrossTotal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            dt.Rows[i]["GrossTotal"] = Common.Get_Roundoff_Amount_By_Location(dt.Rows[i]["GrossTotal"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            dt.Rows[i]["PF"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["PF"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["ESIC"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["ESIC"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Employee_Loan"] = objSys.GetCurencyConversion(dt.Rows[i]["Emp_Id"].ToString(), dt.Rows[i]["Employee_Loan"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            double Exchnagerate = 0;

            Exchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(Common.GetEmployeeCurreny(dt.Rows[i]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));

            dt.Rows[i]["TotalGrossAmount_EmployeeCurrency"] = objSys.GetCurencyConversion_For_EmployeeCurrency(dt.Rows[i]["Emp_Id"].ToString(), (Exchnagerate * Convert.ToDouble(dt.Rows[i]["TotalGrossAmount_EmployeeCurrency"].ToString())).ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            dt.Rows[i]["TotalGrossAmount_EmployeeCurrency"] = Common.Get_Roundoff_Amount_By_Location(dt.Rows[i]["TotalGrossAmount_EmployeeCurrency"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }


        if (dt.Rows.Count <= 0)
        {
            return;
        }

        //code end
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
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
        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            else
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
        }

        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            else
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
        }
        Objreport.setBrandName(BrandName);
        Objreport.setLocationName(LocationName);
        Objreport.setDepartmentName(Session["DepartmentName"].ToString());
        Objreport.SetImage(Imageurl);

        Objreport.setTitleName(Resources.Attendance.Payroll_Summary_Report + ' ' + Resources.Attendance.For + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString() + "");
        Objreport.setheader(Resources.Attendance.Created_By, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Days, Resources.Attendance.Present, Resources.Attendance.Absent, Resources.Attendance.Basic_Salary, Resources.Attendance.Work_Salary, Resources.Attendance.OT_Salary, Resources.Attendance.Penalty, Resources.Attendance.Total, Resources.Attendance.Attendance_Salary, Resources.Attendance.Earning, Resources.Attendance.Allowance, Resources.Attendance.Claim, Resources.Attendance.Deduction, Resources.Attendance.PF, Resources.Attendance.ESIC, Resources.Attendance.Net_Salary);
        Objreport.setcompanyname(CompanyName);
        Objreport.setaddress(CompanyAddress);
        Objreport.setUserName(Session["UserId"].ToString());
        Objreport.DataSource = dt;
        Objreport.DataMember = "Pay_Summary";
        ReportViewer1.Report = Objreport;
        ReportToolbar1.ReportViewer = ReportViewer1;






    }
    //private void ChangeReportSettings(object sender)
    //{

    //    PrintingSystem ps = sender as PrintingSystem;
    //    bool isLocationChanged = false;
    //    int newPageWidth = ps.PageBounds.Width - ps.PageMargins.Left - ps.PageMargins.Right;
    //    int currentPageWidth = this.PageWidth - this.Margins.Left - this.Margins.Right;

    //    foreach (Band _band in base.Bands)
    //    {
    //        foreach (XRControl _control in _band.Controls)
    //        {
    //            isLocationChanged = true;
    //            _control.Location = new Point(0, _control.Location.Y);

    //            // Adjust the width of the control to the new paper width so it will wrap properly.
    //            _control.SizeF = new SizeF(newPageWidth, _control.HeightF);
    //        }
    //    }

    //    if (isLocationChanged == true)
    //    {
    //        Margins.Top = ps.PageMargins.Top;
    //        Margins.Bottom = ps.PageMargins.Bottom;
    //        Margins.Left = ps.PageMargins.Left;
    //        Margins.Right = ps.PageMargins.Right;
    //        PaperKind = ps.PageSettings.PaperKind;
    //        PaperName = ps.PageSettings.PaperName;
    //        Landscape = ps.PageSettings.Landscape;
    //        //CreateDocument();
    //    }
    //}





}
