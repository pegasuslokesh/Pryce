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

public partial class HR_Report_Employee_TotalSalary_Report : BasePage
{
    SystemParameter objSys = null;
    Pay_Employee_Month objPayEmpMonth = null;
    DesignationMaster objDesg = null;
    DepartmentMaster objDep = null;
    Employee_Total_Sal_Report objEmpmasterToalSal = new Employee_Total_Sal_Report();
    Document_Master ObjDoc = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    EmployeeMaster objEmpmaster = null;
    EmployeeParameter objEmpParam = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "145", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dt = objEmpmaster.GetEmployeeMasterAllData(Session["CompId"].ToString());
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

        DataTable dt = objEmpmaster.GetEmployeeMasterAllData(Session["CompId"].ToString());
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
        double netsalary = 0;

        double basicsal = 0;
        double totalattndsal = 0;
        double totalOTsal = 0;
        double totalpenasal = 0;
        double totalallowattendclaim = 0;
        double totaldeducloanpenalty = 0;
        double pvmonthbal_claimPenalty = 0;

        string doj = "";
        string EmployeeName = "";
        string EmployeeCode = "";

        DataTable dtEmptotalsal = new DataTable();
        DataTable dtEmpRecords = new DataTable();
        dtEmptotalsal.Columns.Add("Emp_Id");
        dtEmptotalsal.Columns.Add("EmpCode");
        dtEmptotalsal.Columns.Add("EmpName");

        dtEmptotalsal.Columns.Add("Designation");
        dtEmptotalsal.Columns.Add("Department");
        dtEmptotalsal.Columns.Add("DOJ");
        dtEmptotalsal.Columns.Add("Month");
        dtEmptotalsal.Columns.Add("Year");
        dtEmptotalsal.Columns.Add("BankAccountNo");



        //Attendance Salary
        dtEmptotalsal.Columns.Add("WorkedSal");
        dtEmptotalsal.Columns.Add("WeekOffSal");
        dtEmptotalsal.Columns.Add("HolidaysSal");
        dtEmptotalsal.Columns.Add("LeavedaysSal");

        //Overtime Salary
        dtEmptotalsal.Columns.Add("NormalOTSal");
        dtEmptotalsal.Columns.Add("WeekOffOTSal");
        dtEmptotalsal.Columns.Add("HolidaysOTSal");

        //Penalty Sal
        dtEmptotalsal.Columns.Add("LatePenaSal");
        dtEmptotalsal.Columns.Add("EarlyPenaSal");
        dtEmptotalsal.Columns.Add("PartialPenaSal");
        dtEmptotalsal.Columns.Add("AbsentPenaSal");

        dtEmptotalsal.Columns.Add("AllowanceActAmt");
        dtEmptotalsal.Columns.Add("DeductionActAmt");
        dtEmptotalsal.Columns.Add("TotalEmpPenalty");
        dtEmptotalsal.Columns.Add("TotalEmpClaim");
        dtEmptotalsal.Columns.Add("TotalEmpLoan");

        dtEmptotalsal.Columns.Add("NetSalary");

        dtEmptotalsal.Columns.Add("totalAttendSal");
        dtEmptotalsal.Columns.Add("TotalAdjustmentAmt");
        dtEmptotalsal.Columns.Add("TotalDeducloanpenaty");
        dtEmptotalsal.Columns.Add("Currency_Symbol");
        dtEmptotalsal.Columns.Add("ImageUrl");
        dtEmptotalsal.Columns.Add("TotalGrossSalary");
        dtEmptotalsal.Columns.Add("TotalGrossSalary_EmployeeCurrency");





        string EmpReport = string.Empty;
        string EmpIds = string.Empty;

        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start
        EmpReport = Session["Querystring"].ToString();


        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "22");


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
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report3"].ToString()))
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
                EmployeeName = GetEmployeeName(str);
                EmployeeCode = GetEmployeeCode(str);

                totalattndsal = 0;
                totalOTsal = 0;
                totalpenasal = 0;
                totalallowattendclaim = 0;
                totaldeducloanpenalty = 0;
                 pvmonthbal_claimPenalty=0;


                DataTable dtemppara = new DataTable();
                dtemppara = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                if (dtemppara.Rows.Count > 0)
                {

                    basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                }


                EmployeeName = GetEmployeeName(str);
                EmployeeCode = GetEmployeeCode(str);


                netsalary = 0;



                DataTable dtEmpInfo = new DataTable();
                DataTable dtDepartment = new DataTable();
                DataTable dtDesignation = new DataTable();

                DataTable dtPayEmpMonth = new DataTable();
                DataTable dtEmpAttendance = new DataTable();


                dtEmpInfo = objEmpmaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), EmployeeCode.ToString());
                dtDepartment = objDep.GetDepartmentMasterById(dtEmpInfo.Rows[0]["Department_Id"].ToString());
                dtDesignation = objDesg.GetDesignationMasterById(dtEmpInfo.Rows[0]["Designation_Id"].ToString());

                dtPayEmpMonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                dtPayEmpMonth = new DataView(dtPayEmpMonth, "Emp_Id=" + str + " and Month=" + Session["Month"].ToString() + " and Year='" + Session["Year"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



                //  dtEmpAttendance = objEmpmasterAttendance.GetRecord_Emp_Attendance(str, Session["Month"].ToString(), Session["Year"].ToString());



                for (int i = 0; i < dtEmpInfo.Rows.Count; i++)
                {
                    if (dtPayEmpMonth.Rows.Count > 0)
                    {

                        for (int a = 0; a < dtPayEmpMonth.Rows.Count; a++)
                        {
                            DataRow dr = dtEmptotalsal.NewRow();




                            double worksal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Worked_Min_Salary"].ToString());
                            double leavesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Leave_Days_Salary"].ToString());
                            double holidayssal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holidays_Salary"].ToString());
                            double weekofsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_Salary"].ToString());

                            totalattndsal = worksal + leavesal + holidayssal + weekofsal;

                           

                            double noramlotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Normal_OT_Min_Salary"].ToString());
                            double weekotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Week_Off_OT_Min_Salary"].ToString());
                            double holidaotsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Holiday_OT_Min_Salary"].ToString());
                            totalOTsal = noramlotsal + weekotsal + holidaotsal;

                            double latesal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Late_Min_Penalty"].ToString());
                            double earlysal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Early_Min_Penalty"].ToString());
                            double partialsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Patial_Violation_Penalty"].ToString());
                            double absentsal = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Absent_Salary"].ToString());

                            totalpenasal = latesal + earlysal + partialsal + absentsal;
                            netsalary = (totalattndsal + totalOTsal) - totalpenasal;
                            dr["totalAttendSal"] = netsalary.ToString();





                            dr["AllowanceActAmt"] = dtPayEmpMonth.Rows[0]["Total_Allowance"].ToString();
                            double totallow = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Total_Allowance"].ToString());


                            dr["TotalEmpClaim"] = dtPayEmpMonth.Rows[0]["Employee_Claim"].ToString();
                            double totclaim = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Employee_Claim"].ToString());
                            totalallowattendclaim = netsalary + totallow + totclaim;

                            dr["NetSalary"] = totalallowattendclaim.ToString();


                            dr["DeductionActAmt"] = dtPayEmpMonth.Rows[0]["Total_Deduction"].ToString();
                            double totdeduc = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Total_Deduction"].ToString());
                            dr["TotalEmpLoan"] = dtPayEmpMonth.Rows[0]["Emlployee_Loan"].ToString();
                            double totloan = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Emlployee_Loan"].ToString());
                            dr["TotalEmpPenalty"] = dtPayEmpMonth.Rows[0]["Employee_Penalty"].ToString();
                            double totpenalty = Convert.ToDouble(dtPayEmpMonth.Rows[0]["Employee_Penalty"].ToString());
                            totaldeducloanpenalty = totdeduc + totloan + totpenalty;

                            dr["TotalDeducloanpenaty"] = totaldeducloanpenalty.ToString();
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

                            dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(dtPayEmpMonth.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
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

                            dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(dtPayEmpMonth.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
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




                            dr["TotalAdjustmentAmt"] = pvmonthbal_claimPenalty;


                            dr["TotalGrossSalary"] =  SystemParameter.GetCurrencySmbol(Common.GetEmployeeCurreny(str, Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()),objSys.GetCurencyConversionForInv(Common.GetEmployeeCurreny(str, Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()),(totalallowattendclaim - totaldeducloanpenalty+pvmonthbal_claimPenalty).ToString()), Session["DBConnection"].ToString());
                            dr["TotalGrossSalary_EmployeeCurrency"] = (totalallowattendclaim - totaldeducloanpenalty + pvmonthbal_claimPenalty).ToString();
                            
                            dr["EmpCode"] = EmployeeCode;
                            //this code is modified by jitendra upadhyay on 02-04-2014
                            //for also add the emp_id field and also set the employee id

                            //code start
                            dr["Emp_Id"] = str;
                            //code end
                            
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
                            dr["Month"] = Session["MonthName"].ToString();
                            dr["Year"] = Session["Year"].ToString();
                            dr["BankAccountNo"] = dtEmpInfo.Rows[0]["Account_No"].ToString();
                            dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                            dtEmptotalsal.Rows.Add(dr);


                        }

                    }
                }




            }






        }




        DataTable dtEmpSal = dtEmptotalsal;
        //this code is modified on 02-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dtEmpSal.Rows.Count; i++)
        {
            DataTable dtEmployeemaster = new DataTable();

            try
            {
                dtEmployeemaster = objEmpmaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), dtEmpSal.Rows[i]["EmpCode"].ToString());
            }
            catch
            {

            }

            if (dtEmployeemaster.Rows.Count > 0)
            {
                dtEmpSal.Rows[i]["ImageUrl"] = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmployeemaster.Rows[0]["Emp_Image"].ToString();

            }

            dtEmpSal.Rows[i]["WorkedSal"] =  objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["WorkedSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["WeekOffSal"] =objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["WeekOffSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["HolidaysSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["HolidaysSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["LeavedaysSal"] =objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["LeavedaysSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["NormalOTSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["NormalOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["WeekOffOTSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["WeekOffOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["HolidaysOTSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["HolidaysOTSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["LatePenaSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["LatePenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["EarlyPenaSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["EarlyPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["PartialPenaSal"] =  objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["PartialPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["AbsentPenaSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["AbsentPenaSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["AllowanceActAmt"] =  objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["AllowanceActAmt"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["DeductionActAmt"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["DeductionActAmt"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["TotalEmpPenalty"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["TotalEmpPenalty"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["TotalEmpClaim"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["TotalEmpClaim"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["TotalEmpLoan"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["TotalEmpLoan"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["NetSalary"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["NetSalary"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["totalAttendSal"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["totalAttendSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["TotalAdjustmentAmt"] = objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["TotalAdjustmentAmt"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpSal.Rows[i]["TotalDeducloanpenaty"] =objSys.GetCurencyConversion(dtEmpSal.Rows[i]["Emp_Id"].ToString(), dtEmpSal.Rows[i]["TotalDeducloanpenaty"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

            dtEmpSal.Rows[i]["TotalGrossSalary"] = dtEmpSal.Rows[i]["TotalGrossSalary"].ToString(); ;
            dtEmpSal.Rows[i]["TotalGrossSalary_EmployeeCurrency"] = dtEmpSal.Rows[i]["TotalGrossSalary_EmployeeCurrency"].ToString();
        
        }
        //code end


        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string CompanyContact = "";
        string EmpImageurl = "";
        string Mailid = "";
        string Websitename = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());

        DataTable dtEmpmaster = new DataTable();

        try
        {
            dtEmpmaster = objEmpmaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), dtEmpSal.Rows[0]["EmpCode"].ToString());
        }
        catch
        {

        }

        if (dtEmpmaster.Rows.Count > 0)
        {
            EmpImageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmpmaster.Rows[0]["Emp_Image"].ToString();

        }

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
        objEmpmasterToalSal.setBrand(BrandName);
        objEmpmasterToalSal.setLocation(LocationName);
        objEmpmasterToalSal.setDepartment(Session["DepartmentName"].ToString());
        objEmpmasterToalSal.setcompanyAddress(CompanyAddress);
        objEmpmasterToalSal.setcompanyname(CompanyName);
        objEmpmasterToalSal.setempimage(EmpImageurl);
        objEmpmasterToalSal.setwebsite(Websitename);
        objEmpmasterToalSal.setmailid(Mailid);
        objEmpmasterToalSal.setheader(Resources.Attendance.Created_By, Resources.Attendance.PAYSLIP_FOR_THE_MONTH_OF, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Date_of_Joining, Resources.Attendance.BANK_A_C_NO_, Resources.Attendance.Salary, Resources.Attendance.Allowance, Resources.Attendance.Deduction, Resources.Attendance.Penalty, Resources.Attendance.Claim, Resources.Attendance.Loan, Resources.Attendance.Attendance_Salary, Resources.Attendance.Gross_Pay, Resources.Attendance.Previous_Month_Claim_Penalty);
        objEmpmasterToalSal.setUserName(Session["UserId"].ToString());

        objEmpmasterToalSal.setimage(Imageurl);
        objEmpmasterToalSal.setcontact(CompanyContact);
        objEmpmasterToalSal.DataSource = dtEmpSal;
        objEmpmasterToalSal.DataMember = "dtEmpPayslip";
        ReportViewer1.Report = objEmpmasterToalSal;
        ReportToolbar1.ReportViewer = ReportViewer1;

    }

}
