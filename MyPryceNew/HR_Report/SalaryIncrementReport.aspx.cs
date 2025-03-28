using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class HR_Report_SalaryIncrementReport : System.Web.UI.Page
{
    SystemParameter objSys = null;
    Pay_Employee_Month objPayEmpMonth = null;
    DesignationMaster objDesg = null;
    DepartmentMaster objDep = null;
    // Employee_Pay_Slip_Report objEmpmasterPayslip = new Employee_Pay_Slip_Report();
    Salary_Increment_Report objEmpmasterToalSal = null;
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
        // Employee_Pay_Slip_Report objEmpmasterPayslip = new Employee_Pay_Slip_Report();
        objEmpmasterToalSal = new Salary_Increment_Report(Session["DBConnection"].ToString());
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (Request.QueryString["Emp_Id"] != null)
        {
            GetReport();
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
    public void GetReport()
    {

        DataTable dtFilter = new DataTable();
        EmployeePaySlipDataSet rptdata = new EmployeePaySlipDataSet();
        rptdata.EnforceConstraints = false;
        EmployeePaySlipDataSetTableAdapters.sp_HR_Salary_Increment_SelectRow_ReportTableAdapter adp = new EmployeePaySlipDataSetTableAdapters.sp_HR_Salary_Increment_SelectRow_ReportTableAdapter();
        try
        {

            adp.Fill(rptdata.sp_HR_Salary_Increment_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Request.QueryString["Emp_Id"].ToString()));
        }
        catch
        {
        }
        dtFilter = rptdata.sp_HR_Salary_Increment_SelectRow_Report;
       



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
            //dtEmpmaster = objEmpmaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), dtEmpSal.Rows[0]["EmpCode"].ToString());
        }
        catch
        {

        }

        if (dtEmpmaster.Rows.Count > 0)
        {
            // EmpImageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmpmaster.Rows[0]["Emp_Image"].ToString();

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
        //objEmpmasterToalSal.setDepartment(Session["DepartmentName"].ToString());
        objEmpmasterToalSal.setcompanyAddress(CompanyAddress);
        objEmpmasterToalSal.setcompanyname(CompanyName);
        objEmpmasterToalSal.setempimage(EmpImageurl);
        objEmpmasterToalSal.setwebsite(Websitename);
        objEmpmasterToalSal.setmailid(Mailid);
        objEmpmasterToalSal.setheader(Resources.Attendance.Created_By, Resources.Attendance.PAYSLIP_FOR_THE_MONTH_OF, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Date_of_Joining, Resources.Attendance.BANK_A_C_NO_, Resources.Attendance.Salary, Resources.Attendance.Allowance, Resources.Attendance.Deduction, Resources.Attendance.Penalty, Resources.Attendance.Claim, Resources.Attendance.Loan, Resources.Attendance.Attendance_Salary, Resources.Attendance.Gross_Pay, Resources.Attendance.Previous_Month_Claim_Penalty);
        objEmpmasterToalSal.setUserName(Session["UserId"].ToString());

        //objEmpmasterToalSal.setimage(Imageurl);
        objEmpmasterToalSal.setcontact(CompanyContact);
        objEmpmasterToalSal.DataSource = dtFilter;
        objEmpmasterToalSal.DataMember = "sp_HR_Salary_Increment_SelectRow_Report";
        ReportViewer1.Report = objEmpmasterToalSal;
        ReportToolbar1.ReportViewer = ReportViewer1;

    }
}