using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class HR_Report_ToDoListReport : System.Web.UI.Page
{
    TaskDetailReport objReport = null;
    Prj_Visit_Task objVisitTask = null;

    Attendance objAttendance = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new TaskDetailReport(Session["DBConnection"].ToString());
        objVisitTask = new Prj_Visit_Task(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
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
    void GetReport()
    {
        DataTable dt = new DataTable();

        dt = (DataTable)Session["DtTaskdetail"];

        string CompanyName = "";
        string Imageurl = "";

        // Get Company Name
        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");



        //get detail report band



        objReport.setcompanyname(CompanyName);
        objReport.setReportTitle("Employee Task Report");
        objReport.SetImage(Imageurl);
        objReport.DataSource = dt;
        objReport.DataMember = "sp_HR_FollowUp_Report";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
    }
    
}