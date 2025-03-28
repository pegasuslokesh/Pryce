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

public partial class Attendance_Report_InOutReportNew : BasePage
{
    Set_ApplicationParameter objAppParam = null;
    Att_InOutReport RptShift = null;
    Att_InOutReport_1 RptShift_Seperate = null;
    Attendance objAttendance = null;
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    Att_Employee_Notification objEmpNotice = null;
    Att_Leave_Request objleaveReq = null;
    DataAccessClass ObjDa = null;
    LeaveMaster objLeave = null;
    Att_inOutReport objInOutReport = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EmpList"] == null || Session["FromDate"] == null || Session["ToDate"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }

        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        RptShift = new Att_InOutReport(Session["DBConnection"].ToString());
        RptShift_Seperate = new Att_InOutReport_1(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLeave = new LeaveMaster(Session["DBConnection"].ToString());
        objInOutReport = new Att_inOutReport(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "96", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("96", (DataTable)Session["ModuleName"]);
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
    public void GetReport1()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        if (Session["EmpList"] == null || Session["FromDate"] == null || Session["ToDate"] == null)
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

            EmpReport = Session["EmpList"].ToString();

            //this code is created by jitendra upadhyay on 19-09-2014
            //this code for employee should not be showing in repot if not exists in employee notification for in out report
            //code start
            //code update on 27-09-2014

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(EmpReport, "11");

            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            //code end

            DataTable dtFilter = new DataTable();
            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist, 2);
            }
            catch
            {

            }
            dtFilter = rptdata.sp_Att_AttendanceRegister_Report;

            //if (Emplist != "")
            //{
            //    dtFilter = new DataView(rptdata.sp_Att_AttendanceRegister_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
            //}

            for (int k = 0; k < dtFilter.Rows.Count; k++)
            {


                if (Convert.ToBoolean(dtFilter.Rows[k]["Is_TempShift"].ToString()))
                {

                    dtFilter.Rows[k]["Shift_Name"] = "Temp Shift";

                    if (objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code",Session["CompId"].ToString());
                }

                if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Week_Off"].ToString()))
                {
                    dtFilter.Rows[k]["Field1"] = "Off";
                    dtFilter.Rows[k]["OverTime_Min"] = dtFilter.Rows[k]["Week_Off_Min"];
                    if (objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString());
                }
                else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Holiday"].ToString()))
                {
                    dtFilter.Rows[k]["Field1"] = "Holiday";
                    dtFilter.Rows[k]["OverTime_Min"] = dtFilter.Rows[k]["Holiday_Min"];
                    if (objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString());

                }
                else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Leave"].ToString()))
                {
                    DateTime dtDate = DateTime.Parse(dtFilter.Rows[k]["Att_Date"].ToString());
                    string strEmployeeId = dtFilter.Rows[k]["Emp_Id"].ToString();

                    DataTable DtLeave = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), strEmployeeId);
                    DtLeave = new DataView(DtLeave, "From_Date <='" + dtDate.ToString() + "' and To_Date>='" + dtDate.ToString() + "' and Is_Approved='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (DtLeave.Rows.Count > 0)
                    {
                        //if(DtLeave.Rows[0]["Leave_Name"].ToString().Length>11)
                        //{
                        //    dtFilter.Rows[k]["Field1"] = DtLeave.Rows[0]["Leave_Name"].ToString().Substring(0, 9) + "..";
                        //}
                        //else
                        //{
                        dtFilter.Rows[k]["Field1"] = DtLeave.Rows[0]["Leave_Name"].ToString();
                        //}

                    }
                    else
                    {
                        dtFilter.Rows[k]["Field1"] = "Leave";
                    }

                    if (objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString());


                }
                else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Absent"].ToString()))
                {
                    dtFilter.Rows[k]["Field1"] = "Absent";
                    if (objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString());


                }
                else
                {
                    dtFilter.Rows[k]["Field1"] = "Present";

                    if (objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
                    {
                        dtFilter.Rows[k]["Field2"] = "ffffff";
                    }
                    else
                    {
                        dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString());
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







            if (Request.QueryString["_Id"] == null)
            {

                RptShift.SetCompanyId(Session["CompId"].ToString());
                RptShift.SetImage(Imageurl);
                RptShift.SetBrandName(BrandName);
                RptShift.SetLocationName(LocationName);
                RptShift.SetDepartmentName(DepartmentName);
                RptShift.setTitleName("In Out Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
                RptShift.setcompanyname(CompanyName);
                RptShift.setaddress(CompanyAddress);
                RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Work, Resources.Attendance.Over_Time, Resources.Attendance.Status, Resources.Attendance.Total, Resources.Attendance.Brand, Resources.Attendance.Location, Resources.Attendance.Department);
                RptShift.setUserName(Session["UserId"].ToString());
                RptShift.DataSource = dtFilter;
                RptShift.DataMember = "sp_Att_AttendanceRegister_Report";
                rptViewer.Report = RptShift;
            }
            else
            {
                DataTable dtTemp = new DataTable();

                //here we are making temporary table for show all leave type and count for selected date anbd emnployee ]

                DataTable dtLeave = objLeave.GetLeaveMaster(Session["CompId"].ToString());

                for (int k = 0; k < dtLeave.Rows.Count; k++)
                {
                    dtTemp.Columns.Add(dtLeave.Rows[k]["Leave_Name"].ToString());
                }

                dtTemp.Columns.Add("Holidays");


                Session["dtLeaveHeader"] = dtTemp;
                Session["dtLeaveDetail"] = dtFilter;
                RptShift_Seperate.SetCompanyId(Session["CompId"].ToString());
                RptShift_Seperate.SetImage(Imageurl);
                RptShift_Seperate.SetBrandName(BrandName);
                RptShift_Seperate.SetLocationName(LocationName);
                RptShift_Seperate.SetDepartmentName(DepartmentName);
                RptShift_Seperate.setTitleName("In Out Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
                RptShift_Seperate.setcompanyname(CompanyName);
                RptShift_Seperate.setaddress(CompanyAddress);
                RptShift_Seperate.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Work, Resources.Attendance.Over_Time, Resources.Attendance.Status, Resources.Attendance.Total, Resources.Attendance.Brand, Resources.Attendance.Location, Resources.Attendance.Department);
                RptShift_Seperate.setUserName(Session["UserId"].ToString());
                RptShift_Seperate.DataSource = dtFilter;
                RptShift_Seperate.DataMember = "sp_Att_AttendanceRegister_Report";
                rptViewer.Report = RptShift_Seperate;
            }
            rptToolBar.ReportViewer = rptViewer;

        }


    }
    public void GetReport()
    {
        //DateTime FromDate = new DateTime();
        // DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        string rptType = "1";
        if (Session["EmpList"] == null || Session["FromDate"] == null || Session["ToDate"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        if (Request.QueryString["_Id"] == null)
        {
            rptType = "0";
        }


        DevExpress.XtraReports.UI.XtraReport xrRpt = objInOutReport.GetReport(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpList"].ToString(), Session["FromDate"].ToString(), Session["ToDate"].ToString(), rptType, Session["Lang"].ToString(), Session["UserId"].ToString());
       // DevExpress.XtraReports.UI.XtraReport xrRpt = objInOutReport.GetReport(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "101,113,114,122,124,125,126,127,128,129,130,132,133,134,135,136,142,143,148,151,153,154,156,158,159,163,164,165,166,167,169,170,172,175,178,180,181,183,185,186,187,188,189,190,191,193,196,198,199,20,201,202,203,204,205,206,207,208,210,211,212,213,215,224,225,227,228,229,233,234,235,236,237,241,243,244,245,246,249,257,258,259,260,261,264,265,266,267,268,271,272,273,276,277,278,281,283,284,285,286,288,292,295,296,299,302,306,308,309,311,314,317,318,319,32,321,323,324,331,332,334,340,341,342,343,346,347,348,352,353,355,356,361,363,365,368,369,370,373,375,376,38,384,388,389,39,390,394,396,397,399,401,402,403,407,409,41,411,413,414,415,417,418,419,42,422,426,429,43,430,432,434,435,44,440,441,442,444,446,448,45,453,454,455,456,459,460,464,468,469,470,471,476,479,482,484,485,486,487,493,495,497,503,506,507,508,510,511,518,522,527,528,531,532,533,539,54,540,543,544,545,549,55,550,552,553,554,559,56,560,561,562,563,564,566,567,571,577,578,58,581,585,59,591,594,595,597,599,60,600,605,606,61,610,612,614,62,620,623,624,625,626,627,629,633,634,638,639,64,640,642,644,646,647,649,65,653,654,658,66,660,661,663,664,665,666,670,673,674,68,680,681,683,687,69,694,695,696,698,70,700,701,702,703,704,705,706,707,710,715,717,718,719,721,726,728,73,730,731,732,735,736,738,739,74,745,746,749,750,752,755,756,757,760,762,767,77,771,773,774,775,778,779,78,780,781,783,784,801,804,805,809,810,819,821,823,826,827,83,840,845,85,851,854,855,856,857,860,864,866,867,868,869,870,871,872,874,903,907,909,91,911,912,913,914,915,93,935,936,937,94,941,943,947,95,954,955,962,968,975", Session["FromDate"].ToString(), Session["ToDate"].ToString(), rptType, Session["Lang"].ToString(), Session["UserId"].ToString());
        rptViewer.Report = xrRpt;
        rptToolBar.ReportViewer = rptViewer;
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


}
