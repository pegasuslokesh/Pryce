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

public partial class HR_Report_PenaltyReport : BasePage
{
    SystemParameter objSys = null;
    EmployeePenaltyReport_Currency objReport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    EmployeeParameter ObjSalary = null;
    Pay_Employee_Penalty ObjPenalty = null;
    EmployeeMaster objEmp = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objReport = new EmployeePenaltyReport_Currency(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjSalary = new EmployeeParameter(Session["DBConnection"].ToString());
        ObjPenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "136", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            GetReport();
        } Page.Title = objSys.GetSysTitle();

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

    void GetReport()
    {



        DataTable DtClaimRecord = new DataTable();
        DtClaimRecord = ObjPenalty.GetRecord_From_PayEmployeePenalty_By_MonthAndYear(Session["CompId"].ToString(), Session["Month"].ToString(), Session["Year"].ToString());



       
        DataTable dt = new DataTable();
        dt.Columns.Add("EmpId");
        dt.Columns.Add("EmpName");
        dt.Columns.Add("Penalty_Name");
        dt.Columns.Add("Penalty_Value");
        dt.Columns.Add("Penalty_Date");
       
        dt.Columns.Add("Month");
        dt.Columns.Add("Year");
        dt.Columns.Add("Currency_Symbol");
        dt.Columns.Add("Employee_Id");


        string Id = string.Empty;
        string EmpReport = string.Empty;


        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start
        EmpReport = Session["Querystring"].ToString();

        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "24");


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
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report5"].ToString()))
        //        {
        //            Id += EmpReport.Split(',')[i].ToString() + ",";
        //        }
        //    }
        //}

        //code end
       
        double Penaltysum = 0;
        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                DataTable dtClaimRecod = DtClaimRecord;
                dtClaimRecod = new DataView(dtClaimRecod, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtClaimRecod.Rows.Count > 0)
                {
                    DataTable dtsalary = ObjSalary.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                    string[] Montharr = new string[12];


                    Montharr[0] = "January";
                    Montharr[1] = "February";
                    Montharr[2] = "March";
                    Montharr[3] = "April";
                    Montharr[4] = "May";
                    Montharr[5] = "June";
                    Montharr[6] = "July";
                    Montharr[7] = "August";
                    Montharr[8] = "September";
                    Montharr[9] = "October";
                    Montharr[10] = "November";
                    Montharr[11] = "December";
                    double salary = 0;
                    for (int i = 0; i < dtClaimRecod.Rows.Count; i++)
                    {
                        if (dtsalary.Rows.Count > 0)
                        {

                             salary = Convert.ToDouble(dtsalary.Rows[0]["Basic_Salary"]);
                        }
                        double Claimamount = 0;
                        if (dtClaimRecod.Rows[i]["Value_Type"].ToString() == "2")
                        {
                            double value = Convert.ToDouble(dtClaimRecod.Rows[i]["Value"]);
                            Claimamount = salary * value / 100;
                        }
                        else
                        {
                            Claimamount = Convert.ToDouble(dtClaimRecod.Rows[i]["Value"].ToString());
                        }

                        int Month = Convert.ToInt32(dtClaimRecod.Rows[i]["Penalty_Month"]);

                        DataRow dr = dt.NewRow();
                        dr[0] =GetEmployeeCode(dtClaimRecod.Rows[i]["Emp_Id"].ToString());
                        dr[1] = dtClaimRecod.Rows[i]["Emp_Name"].ToString();
                        dr[2] = dtClaimRecod.Rows[i]["Penalty_Name"].ToString();
                        //this code is modified on 02-04-2014 by jitendra upadhyay
                        //for convert the amount in selected currency by employee and  company
                        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
                        //this function return the amount after currency conversion

                        //code start
                        dr[3] = objSys.GetCurencyConversion(str,Claimamount.ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                        //code end
                        dr[4] = dtClaimRecod.Rows[i]["Penalty_Date"].ToString();
                        dr[5] = Montharr[Month - 1].ToString();
                        dr[6] = dtClaimRecod.Rows[i]["Penalty_Year"].ToString();
                        dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                        Penaltysum +=Convert.ToDouble(objSys.GetCurencyConversion(str, Claimamount.ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString())) ;
                        dr["Employee_Id"] = str;
                        dt.Rows.Add(dr);
                    }
                }



            }
        }
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
        objReport.setDepartment(Session["DepartmentName"].ToString());
        objReport.setBrandName(BrandName);
        objReport.setLocation(LocationName);
     
        objReport.SetImage(Imageurl);
        objReport.setTitleName(Resources.Attendance.Penalty_Report);
        objReport.SetCompanyName(CompanyName);
        objReport.SetAddress(CompanyAddress);
        objReport.setSum(Penaltysum);
        objReport.setUserName(Session["UserId"].ToString());
        objReport.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Employee_Name, Resources.Attendance.Employee_Code, Resources.Attendance.Penalty_Name, Resources.Attendance.Penalty_Date, Resources.Attendance.Penalty_Value, Resources.Attendance.Total);
        objReport.DataSource = dt;
        objReport.DataMember = "EmpPenalty";
        ReportViewer1.Report = objReport;
        ReportToolbar1.ReportViewer = ReportViewer1;





    }

}
