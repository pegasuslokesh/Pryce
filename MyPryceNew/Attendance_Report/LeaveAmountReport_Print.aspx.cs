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

public partial class Attendance_Report_LeaveAmountReport_Print : System.Web.UI.Page
{
    //XtraReport RptShift = new XtraReport();
    Attendance objAttendance = null;
    Att_LeaveAmountReport RptShift = null;
    Set_AddressChild ObjAddress = null;
    SystemParameter objSys = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    DataAccessClass ObjDa = null;
    LeaveMaster_deduction objLeavededuction = null;
    hr_empgratuity Emp_Gratuity = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_DailySalaryReport1.repx");

        RptShift = new Att_LeaveAmountReport(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "253", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("253", (DataTable)Session["ModuleName"]);
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
        if (Request.QueryString["Trans_Id"] != null)
        {
            Session["EmpList"] = Request.QueryString["Trans_Id"].ToString();
        }

        string Emplist = Session["EmpList"].ToString();

        if (Session["EmpList"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../HR_Report/GeneratePayrollReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            DataTable dtFilter = new DataTable();
            AttendanceDataSet rptdata = new AttendanceDataSet();
            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_LeaveAmountReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_LeaveAmountReportTableAdapter();
            //adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_LeaveAmountReport, int.Parse(Session["CompId"].ToString()), int.Parse(Session["BrandId"].ToString()), DateTime.Now.Year.ToString(), 3);
            }
            catch (Exception ex)
            {

            }

            dtFilter = rptdata.sp_Att_LeaveAmountReport;
            DataTable dtEmp = new DataTable();

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
            LocationName = objAttendance.GetLocationName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["Lang"].ToString());
            //Get Department Name
            DepartmentName = "All";

            // Get Company Address
            DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
            if (DtAddress.Rows.Count > 0)
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }

            RptShift.SetImage(Imageurl);
            RptShift.SetBrandName(BrandName);
            RptShift.SetLocationName(LocationName);
            RptShift.SetDepartmentName(DepartmentName);
            //RptShift.setTitleName("Salary Report" + " From " + FromDate1.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            RptShift.setTitleName("Leave Application Schedule");
            RptShift.setcompanyname(CompanyName);
            RptShift.setaddress(CompanyAddress);
           // RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Work_Hour, Resources.Attendance.Over_Time_Hour, Resources.Attendance.Salary, Resources.Attendance.OT_Salary, Resources.Attendance.Is_Off, Resources.Attendance.Is_Holiday, Resources.Attendance.Is_Leave, Resources.Attendance.Week_Off_Count, Resources.Attendance.Holiday_Count, Resources.Attendance.Leave_Count, Resources.Attendance.Assigned_Hour, Resources.Attendance.Over_Time_Hour, Resources.Attendance.Work_Salary, Resources.Attendance.OT_Salary, Resources.Attendance.Gross_Total);
            RptShift.setUserName(Session["UserId"].ToString());
            RptShift.DataSource = dtFilter;
            RptShift.DataMember = "sp_Att_LeaveAmountReport";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;
        }
    }
}