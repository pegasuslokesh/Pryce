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

public partial class Attendance_Report_TimeSheetReport1 : System.Web.UI.Page
{
    TimeSheet objReport = new TimeSheet();
    AttendanceDataSet objDataset = new AttendanceDataSet();
    Att_AttendanceRegister Att_Reg = null;
    Att_AttendanceLog objAttLog = null;
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    DataTable dtCount = new DataTable();
    Attendance objAttendance = null;
    DataTable dtLog = new DataTable();
    DataTable dtReport = new DataTable();

    DataTable OnOffTime = new DataTable();
    string Fromdate = string.Empty;
    string Todate = string.Empty;
    DataTable dtinfo = new DataTable();
    int workedWithoutRest = 0;
    string EmpId = string.Empty;
    DataTable dtInfo = new DataTable();
    string count = "2";

    protected void Page_Load(object sender, EventArgs e)
    {
        Att_Reg = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            DataColumn column = new DataColumn();
            column = dtInfo.Columns.Add("Is_Absent");
            column = new DataColumn();
            column = dtInfo.Columns.Add("WeekOffOT");
            column = new DataColumn();
            column = dtInfo.Columns.Add("HolidayOT");
            column = new DataColumn();
            column = dtInfo.Columns.Add("NormalDaysOT");
            column = new DataColumn();
            column = dtInfo.Columns.Add("LateInMin");
            column = new DataColumn();
            column = dtInfo.Columns.Add("CardNo");
            column = new DataColumn();
            column = dtInfo.Columns.Add("Emp_Name");
            column = new DataColumn();
            column = dtInfo.Columns.Add("Days_Withoutrest");
            getReport();
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

    public void getReport()
    {
        
        Fromdate = Session["FromDate"].ToString();
        Todate = Session["ToDate"].ToString();

        string EmpList = Session["EmpList"].ToString();
        //string EmpId = EmpList.Substring(0, EmpList.Length - 1);
           string[] split = EmpList.Split(',');
           foreach (string item in split)
           {
               if (item != "")
               {
                   EmpId = item;
               }
               else
               {
                   break;
               }
               ViewState["EmpId"]=EmpId;
              
               objDataset.EnforceConstraints = false;
               // DateTime FromDate =Convert.ToDateTime(04/01/2014);
               //DateTime ToDate =Convert.ToDateTime(04/10/2014);
               // Fromdate=Fromdate.ToString();
               //Todate=ToDate.ToString();
           


               //for (int j = 0; j < Convert.ToInt32(EmpList.Length - 1); j = j + 2)
               //{

               // OnDuty Off Duty Time Dt
               // FOr OnOff TIme--------------------------------
               OnOffTime = Att_Reg.GetTimeSheetReportByEmpId(EmpId.ToString(), Fromdate, Todate, "2");

               // Get Assing TimeTable
               dtCount = Att_Reg.GetTimeSheetReportByEmpId(EmpId.ToString(), Fromdate, Todate, "1");

               DataView datavw = new DataView();
               datavw = dtCount.DefaultView;
              
               datavw.RowFilter = "Event_Count='" + count + "'";
               if (datavw.Count > 0)
               {
                   for (int i = 0; i < datavw.Count; i++)
                   {
                       dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(EmpId, datavw[i]["Event_Date"].ToString(), datavw[i]["Event_Date"].ToString());
                       DateTime EventDate1 = Convert.ToDateTime(dtLog.Rows[0]["Event_Time"].ToString());
                       string E1 = EventDate1.ToString("HH:mm");
                       DateTime EventDate2 = Convert.ToDateTime(dtLog.Rows[1]["Event_Time"].ToString());
                       string E2 = EventDate2.ToString("HH:mm");

                       //-----------------
                       DateTime OnTimeDate = Convert.ToDateTime(OnOffTime.Rows[0]["OnDuty_Time"].ToString());
                       string OnTime = OnTimeDate.ToString("HH:mm");
                       DateTime OffTimeDate = Convert.ToDateTime(OnOffTime.Rows[0]["OffDuty_Time"].ToString());
                       string OffTime = OffTimeDate.ToString("HH:mm");
                       //-----------------

                       if (E1 == OnTime && E2 == OffTime)
                       {
                           workedWithoutRest += 1;
                       }


                   }
                   FillGridSheet();
               }
           }

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

        objReport.Setcompanyname(CompanyName);
        objReport.SetCompanyAddress(CompanyAddress);
        objReport.SetImageUrl(Imageurl);
        objReport.SetUserName(Session["UserId"].ToString());
        objReport.SetReportHeader("Time Sheet Report");
        DataTable FInalDt = (DataTable)ViewState["dtInfo"];
        dtReport = FInalDt;

      

        objReport.DataSource = dtReport;
        objReport.DataMember = "GetTimesheetSummery_Report";
        ReportViewer1.Report = objReport;
        ReportToolbar1.ReportViewer = ReportViewer1;     
        
    }

    private void FillGridSheet()
    {

        // string EmpList = Session["EmpList"].ToString();
        //string EmpId = EmpList.Substring(0, EmpList.Length - 1);
        string EmpId = ViewState["EmpId"].ToString();
        Fromdate = Session["FromDate"].ToString();
        Todate = Session["ToDate"].ToString(); ;
        DataTable DtInfo = new DataTable();

        DtInfo = Att_Reg.GetTimeSheetReportByEmpId(EmpId.ToString(), Fromdate, Todate, "0");
        DataColumn workWithotRest = DtInfo.Columns.Add("Days_Withoutrest", typeof(string));
      foreach (DataRow dr in DtInfo.Rows)
      {
          dr["Days_Withoutrest"] = workedWithoutRest;  
      }


      DataRow row = dtInfo.NewRow();
      row["Is_Absent"] = DtInfo.Rows[0]["Is_Absent"];
      row["WeekOffOT"] = DtInfo.Rows[0]["WeekOffOT"];
      row["HolidayOT"] = DtInfo.Rows[0]["HolidayOT"];
      row["LateInMin"] = DtInfo.Rows[0]["LateInMin"];
      row["CardNo"] = DtInfo.Rows[0]["CardNo"];
      row["Emp_Name"] = DtInfo.Rows[0]["Emp_Name"];
      row["Days_Withoutrest"] = DtInfo.Rows[0]["Days_Withoutrest"];
      dtInfo.Rows.Add(row);



      ViewState["dtInfo"] = dtInfo;
     
    }

    }

