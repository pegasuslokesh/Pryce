using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;

public partial class Attendance_Report_Vacation_Leave_Application : System.Web.UI.Page
{
    LeaveApplicationForm objReport = null;
    //Leave_Application objReport = new Leave_Application();
    ShortLeave_Application ObjShortLeave = null;
    SystemParameter objsys = null;
    SickLeave_Application ObjSickLeavereport = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    Att_Leave_Request objleaveReq = null;
    Attendance objAttendance = null;
    DutyResumptionReport objDutyReport = null;
    SalaryCertificateRequestForm objSalaryReport = null;
    DataAccessClass objda = null;

    LocationMaster objLoc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new LeaveApplicationForm(Session["DBConnection"].ToString());
        //Leave_Application objReport = new Leave_Application();
        ObjShortLeave = new ShortLeave_Application(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjSickLeavereport = new SickLeave_Application(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objDutyReport = new DutyResumptionReport(Session["DBConnection"].ToString());
        objSalaryReport = new SalaryCertificateRequestForm(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());

        objLoc = new LocationMaster(Session["DBConnection"].ToString());

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
        //string Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");

        string Imageurl = "";

        DataTable DtLocation = objLoc.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (DtLocation.Rows[0]["Field2"].ToString() != "" && DtLocation.Rows[0]["Field2"].ToString() != null)
            {
                Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();
            }
            else
            {
                Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
            }
        }
        else
        {
            Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
        }
      
        DataTable dtFilter = new DataTable();

        AttendanceDataSet rptdata = new AttendanceDataSet();

        rptdata.EnforceConstraints = false;



        if (Request.QueryString["EmpId"] != null && Request.QueryString["Type"] == null)
        {

            AttendanceDataSetTableAdapters.sp_att_LeaveRequest_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_att_LeaveRequest_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_att_LeaveRequest_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Request.QueryString["EmpId"].ToString()), Convert.ToInt32(Request.QueryString["TransId"].ToString()));
            }
            catch
            {

            }
            dtFilter = rptdata.sp_att_LeaveRequest_Report;



            if (dtFilter.Rows.Count > 1)
            {
                objReport.setCompanyLogo(Imageurl);
                objReport.DataSource = dtFilter;
                objReport.DataMember = "sp_att_LeaveRequest_Report";
                rptViewer.Report = objReport;
                lblHeader.Text = "Leave Application Form";

            }
            else if (dtFilter.Rows.Count == 1)
            {
                string strLeaveTypeid = string.Empty;




                  strLeaveTypeid = objda.return_DataTable("select Att_Leave_Request.Leave_Type_Id from Att_LeaveRequest_header inner join  Att_Leave_Request on Att_LeaveRequest_header.Trans_Id= Att_Leave_Request.Field2 where Att_LeaveRequest_header.Trans_Id=" + Request.QueryString["TransId"].ToString() + "").Rows[0][0].ToString();



                  DataTable dtleavededuction = ObjLeavededuction.GetRecordbyLeaveTypeId(strLeaveTypeid);

                  if (dtleavededuction.Rows.Count == 0)
                  {
                      objReport.setCompanyLogo(Imageurl);
                      objReport.DataSource = dtFilter;
                      objReport.DataMember = "sp_att_LeaveRequest_Report";
                      rptViewer.Report = objReport;
                      lblHeader.Text = "Leave Application Form";

                  }
                  else
                  {
                      ObjSickLeavereport.setCompanyLogo(Imageurl);
                      ObjSickLeavereport.DataSource = dtFilter;
                      ObjSickLeavereport.DataMember = "sp_att_LeaveRequest_Report";
                      rptViewer.Report = ObjSickLeavereport;
                      lblHeader.Text = "Sick Leave Form";

                  }

                  dtleavededuction.Dispose();
                  
            }


            //DataTable dtLeave = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
            //try
            //{
            //    dtLeave = new DataView(dtLeave, "Trans_Id=" + Request.QueryString["TransId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //catch
            //{
            //}



            //DataTable dtleavededuction = ObjLeavededuction.GetRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtLeave.Rows[0]["Leave_Type_Id"].ToString(), "0");

            //if (dtleavededuction.Rows.Count == 0)
            //{

            //    DateTime dtfrom = Convert.ToDateTime(dtFilter.Rows[0]["Fromdate"].ToString());
            //    DateTime Todate = Convert.ToDateTime(dtFilter.Rows[0]["ToDate"].ToString());
            //    string HeaderName = string.Empty;
            //    string EmpSignature = string.Empty;
            //    if (dtfrom == Todate)
            //    {
            //        HeaderName = "I would kindly request you to grant me leave on " + dtfrom.ToString(objsys.SetDateFormat()) + ".";
            //    }
            //    else
            //    {
            //        HeaderName = "I would kindly request you to grant me leave from " + dtfrom.ToString(objsys.SetDateFormat()) + " to " + Todate.ToString(objsys.SetDateFormat()) + ".";
            //    }
            //    if (dtFilter.Rows[0]["Signatureurl"].ToString() != "")
            //    {
            //        EmpSignature = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtFilter.Rows[0]["Signatureurl"].ToString();
            //    }
            //    // objReport.setReportHeader(HeaderName);
            //    // objReport.setsignature(EmpSignature);
            //    try
            //    {
            //        if (dtFilter.Rows[0]["LeaveName"].ToString() != "")
            //        {
            //            //      objReport.setSubject(dtFilter.Rows[0]["LeaveName"].ToString() + " Application");
            //        }
            //        else
            //        {

            //        }
            //    }

            //    catch
            //    {

            //    }



            //}
            //else
            //{
            //    ObjSickLeavereport.setCompanyLogo(Imageurl);
            //    ObjSickLeavereport.DataSource = dtFilter;
            //    ObjSickLeavereport.DataMember = "sp_att_LeaveRequest_Report";
            //    rptViewer.Report = ObjSickLeavereport;
            //    lblHeader.Text = "Sick Leave Form";
            //}


         
        }
        else if (Request.QueryString["Type"] == null && Request.QueryString["TransId"] != null)
        {
            AttendanceDataSetTableAdapters.sp_Att_PartialLeave_Request_SelectRowByTransIdTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_PartialLeave_Request_SelectRowByTransIdTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_PartialLeave_Request_SelectRowByTransId, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Request.QueryString["TransId"].ToString()));
            }
            catch
            {
            }
            dtFilter = rptdata.sp_Att_PartialLeave_Request_SelectRowByTransId;


            if (dtFilter.Rows.Count > 0)
            {
                ObjShortLeave.setTotalHours(GetHours(GetMinuteDiff(dtFilter.Rows[0]["To_Time"].ToString(), dtFilter.Rows[0]["From_Time"].ToString())));
            }

            ObjShortLeave.setCompanyLogo(Imageurl);
            ObjShortLeave.DataSource = dtFilter;
            ObjShortLeave.DataMember = "sp_Att_PartialLeave_Request_SelectRowByTransId";
            rptViewer.Report = ObjShortLeave;
            lblHeader.Text = "Short Leave Form";

        }
        else if (Request.QueryString["EmpId"] != null && Request.QueryString["Type"] != null)
        {


            ///for duty resumption report


            AttendanceDataSetTableAdapters.sp_Set_EmployeeMaster_SelectRow_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Set_EmployeeMaster_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Set_EmployeeMaster_SelectRow_Report, Request.QueryString["EmpId"].ToString());
            }
            catch
            {
            }
            dtFilter = rptdata.sp_Set_EmployeeMaster_SelectRow_Report;


            if (Request.QueryString["Type"] == "1")
            {
                objDutyReport.setCompanyLogo(Imageurl);
                objDutyReport.DataSource = dtFilter;
                objDutyReport.DataMember = "sp_Set_EmployeeMaster_SelectRow_Report";
                rptViewer.Report = objDutyReport;
                lblHeader.Text = "Duty Resumption Report";
            }
            else
            {
                objSalaryReport.setCompanyLogo(Imageurl);
                objSalaryReport.DataSource = dtFilter;
                objSalaryReport.DataMember = "sp_Set_EmployeeMaster_SelectRow_Report";
                rptViewer.Report = objSalaryReport;
                lblHeader.Text = "Salary Certificate Request Form";
            }
        }


        rptToolBar.ReportViewer = rptViewer;

    }




    private int GetMinuteDiff(string greatertime, string lesstime)
    {

        if (greatertime == "__:__:__" || greatertime == "")
        {
            return 0;
        }
        if (lesstime == "__:__:__" || lesstime == "")
        {
            return 0;
        }
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }

    public string GetHours(object obj)
    {
        if (obj.ToString() == "")
        {
            return "";
        }
        string retval = string.Empty;
        retval = ((Convert.ToInt32(obj) / 60) < 10) ? "0" + (Convert.ToInt32(obj) / 60).ToString() : ((Convert.ToInt32(obj) / 60)).ToString();
        retval += ":" + (((Convert.ToInt32(obj) % 60) < 10) ? "0" + (Convert.ToInt32(obj) % 60) : (Convert.ToInt32(obj) % 60).ToString());

        return retval;
    }
}