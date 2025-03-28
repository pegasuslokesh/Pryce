using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class HR_Report_Employee_Termination_Report : System.Web.UI.Page
{
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
    EmployeeTerminationReport objEmpTermination = null;
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
        objEmpTermination = new EmployeeTerminationReport(Session["DBConnection"].ToString());
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

        GetReport();
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

        EmployeeTerminationDataset rptdata = new EmployeeTerminationDataset();

        rptdata.EnforceConstraints = false;
        EmployeeTerminationDatasetTableAdapters.sp_Pay_Termination_selectRowTableAdapter adp = new EmployeeTerminationDatasetTableAdapters.sp_Pay_Termination_selectRowTableAdapter();
        try
        {
            adp.Fill(rptdata.sp_Pay_Termination_selectRow,0,Convert.ToInt32(Request.QueryString["Id"].ToString()), 1);

        }
        catch
        {
        }
        dtFilter = rptdata.sp_Pay_Termination_selectRow;




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

        for (int i = 0; i < dtFilter.Rows.Count; i++)
        {
            dtFilter.Rows[i]["EmpImageUrl"] = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtFilter.Rows[i]["EmpImage"].ToString();

        }
        objEmpTermination.setBrandName(BrandName);
        objEmpTermination.setLocationName(LocationName);
        objEmpTermination.setcompanyAddress(CompanyAddress);
        objEmpTermination.setcompanyname(CompanyName);
        //objEmpPayslip.setempimage(EmpImageurl);
        objEmpTermination.setmailid(Mailid);
        objEmpTermination.setwebsite(Websitename);


        objEmpTermination.setimage(Imageurl);
        objEmpTermination.setcontact(CompanyContact);



        objEmpTermination.setUserName(Session["UserId"].ToString());
        objEmpTermination.setUserName1(Session["UserId"].ToString());


        //objEmpTermination.Setheader(Resources.Attendance.Created_By, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Designation, Resources.Attendance.Department, Resources.Attendance.Date_of_Joining, Resources.Attendance.BANK_A_C_NO_, Resources.Attendance.Attendance1, Resources.Attendance.Attendance1, Resources.Attendance.Days_Present, Resources.Attendance.Week_Off, Resources.Attendance.Days_absent, Resources.Attendance.Holiday, Resources.Attendance.Leave, Resources.Attendance.Basic_Salary, Resources.Attendance.Attendance_Salary, Resources.Attendance.Over_Time_Salary, Resources.Attendance.Penalty, Resources.Attendance.Work_Days, Resources.Attendance.Normal_Days_OT, Resources.Attendance.Week_Off_OT, Resources.Attendance.Holiday_OT, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Partial, Resources.Attendance.Absent, Resources.Attendance.Total, Resources.Attendance.Gross_Pay, Resources.Attendance.PF, Resources.Attendance.ESIC, Resources.Attendance.Net_Salary, Resources.Attendance.HR_SIGNATURE, Resources.Attendance.EMPLOYEE_SIGNATURE, Resources.Attendance.PAYSLIP_FOR_THE_MONTH_OF + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString(), Resources.Attendance.Employee_copy, Resources.Attendance.Employer_copy, Resources.Attendance.Total_Days);

        objEmpTermination.DataSource = dtFilter;
        objEmpTermination.DataMember = "sp_Pay_Termination_selectRow";



        ReportViewer1.Report = objEmpTermination;

        ReportToolbar1.ReportViewer = ReportViewer1;



    }
}
