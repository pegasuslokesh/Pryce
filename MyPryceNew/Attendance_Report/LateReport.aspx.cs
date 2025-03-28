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
using System.IO;
using DevExpress.XtraReports.UI;

public partial class Attendance_Report_LateReportNew : BasePage
{
    XtraReport RptShift = new XtraReport();
   // Att_LateReport RptShift = new Att_LateReport();
    SystemParameter objSys = null;
    Attendance objAttendance = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_LateReport.repx");
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "97", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("97", (DataTable)Session["ModuleName"]);
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
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        //End Code
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



            //this code is created by jitendra upadhyay on 19-09-2014
            //this code for employee should not be showing in repot if not exists in employee notification for in out report
            //code start


            //code update on 27-09-2014

            //DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "12");


            //foreach (DataRow dr in dtEmpNF.Rows)
            //{
            //    Emplist += dr["Emp_Id"] + ",";
            //}

            //code end


            DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Session["EmpList"].ToString(), 9);
            }
            catch
            {

            }
            dtFilter = rptdata.sp_Att_AttendanceRegister_Report;


            //dtFilter = new DataView(dtFilter, "Emp_Id=57", "", DataViewRowState.CurrentRows).ToTable();

            TimeSpan span = TimeSpan.FromMinutes(155);
            string label = span.ToString(@"hh\:mm");



            dtFilter.AcceptChanges();

            

            
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
            if (Settings.IsDemo)
            {
                RptShift.Watermark.Text = "Demo Version";
                RptShift.Watermark.ForeColor = System.Drawing.Color.Red;
            }

            XRPictureBox xrPictureBox1 = (XRPictureBox)RptShift.FindControl("xrPictureBox1", true);
            try
            {
                xrPictureBox1.ImageUrl = Imageurl;
            }
            catch
            {

            }

            XRLabel xrTableCell56 = (XRLabel)RptShift.FindControl("xrTableCell56", true);
            xrTableCell56.Text = BrandName;
            XRLabel xrTableCell58 = (XRLabel)RptShift.FindControl("xrTableCell58", true);
            xrTableCell58.Text = LocationName;
            XRLabel xrTableCell54 = (XRLabel)RptShift.FindControl("xrTableCell54", true);
            xrTableCell54.Text = DepartmentName;


            XRLabel xrTitle = (XRLabel)RptShift.FindControl("xrTitle", true);
            xrTitle.Text = "Late In Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
            XRLabel xrCompName = (XRLabel)RptShift.FindControl("xrCompName", true);
            xrCompName.Text = CompanyName;
            XRLabel xrCompAddress = (XRLabel)RptShift.FindControl("xrCompAddress", true);
            xrCompAddress.Text = CompanyAddress;
            XRLabel xrLabel15 = (XRLabel)RptShift.FindControl("xrLabel15", true);
            xrLabel15.Text = Session["UserId"].ToString();


           
           
            XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            //xrTableCell13.Text = Resources.Attendance.Id;
            xrTableCell13.Text = "ID";
            XRLabel xrTableCell17 = (XRLabel)RptShift.FindControl("xrTableCell17", true);
            xrTableCell17.Text = Resources.Attendance.Name;
            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);

            xrTableCell7.Text = Resources.Attendance.Date;
            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = Resources.Attendance.Shift_Name;
            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = Resources.Attendance.On_Duty_Time;
            XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            xrTableCell4.Text = Resources.Attendance.In_Time;
            XRLabel xrTableCell6 = (XRLabel)RptShift.FindControl("xrTableCell6", true);
            xrTableCell6.Text = Resources.Attendance.Late_Hour;
            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = Resources.Attendance.Late_Count;
            XRLabel xrTableCell9 = (XRLabel)RptShift.FindControl("xrTableCell9", true);
            xrTableCell9.Text = Resources.Attendance.Late_Hours;
            XRLabel xrTableCell21 = (XRLabel)RptShift.FindControl("xrTableCell21", true);
            xrTableCell21.Text = "Relaxation Hour";
            XRLabel xrTableCell55 = (XRLabel)RptShift.FindControl("xrTableCell55", true);
            //  xrTableCell55.Text = Resources.Attendance.Brand;
            xrTableCell55.Text = "Company Name : ";
            XRLabel xrTableCell57 = (XRLabel)RptShift.FindControl("xrTableCell57", true);
            xrTableCell57.Text = Resources.Attendance.Location;
            XRLabel xrTableCell53 = (XRLabel)RptShift.FindControl("xrTableCell53", true);
            xrTableCell53.Text = Resources.Attendance.Department;




            RptShift.DataSource = dtFilter;
            RptShift.DataMember = "sp_Att_AttendanceRegister_Report";
            RptShift.CreateDocument(true);
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;
        }
    }
}
