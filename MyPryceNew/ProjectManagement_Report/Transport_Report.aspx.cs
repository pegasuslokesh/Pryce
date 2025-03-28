using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class ProjectManagement_Report_Transport_Report : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();
    VisitDetailReport objReport = null;
    Prj_Visit_Task objVisitTask = null;

    Attendance objAttendance = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objReport = new VisitDetailReport(Session["DBConnection"].ToString());
        objVisitTask = new Prj_Visit_Task(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "275", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        GetReport();
       

    }
   
    void GetReport()
    {
        DataTable dt=new DataTable();

        dt = (DataTable)Session["DtTransportReport"];

        string CompanyName = "";
        string Imageurl = "";
        
        // Get Company Name
        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");



        //get detail report band
       
      

        objReport.setcompanyname(CompanyName);
        objReport.setReportTitle("Transport Report");
        objReport.SetImage(Imageurl);
        objReport.DataSource = dt;
        objReport.DataMember = "sp_Prj_VisitMaster_SelectRow_Report";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
     

    }
    
}