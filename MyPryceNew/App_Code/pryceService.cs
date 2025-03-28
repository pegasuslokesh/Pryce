using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.Configuration;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

/// <summary>
/// Summary description for pryceService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class pryceService : System.Web.Services.WebService
{
    //DataAccessClass Objda = null;
    //Att_inOutReport objInOutReport = null;
    //LogProcess ObjLogProcess = null;
    //AccessGroupSummary objgroupsummary = null;
    //LocationMaster ObjLocationMaster = null;
    //Country_Currency objCC = null;
    //private string _strConString = string.Empty;
    //public pryceService(string strConString)
    //{

    //    //Uncomment the following line if using designed components 
    //    //InitializeComponent(); 
    //    Objda = new DataAccessClass(strConString);
    //    ObjLogProcess = new LogProcess(strConString);
    //    objgroupsummary = new AccessGroupSummary(strConString);
    //    ObjLocationMaster = new LocationMaster(strConString);
    //    objCC = new Country_Currency(strConString);
    //    objInOutReport = new Att_inOutReport(strConString);
    //    _strConString = strConString;
    //}





    [WebMethod(EnableSession = true)]
    public string GetInoutRptPdfUrl(string strCompanyId, string strBrandId, string strLocationId, string strEmpList, string strFromDate, string strToDate, string strfileName, string strDBConnection)
    {
        string rptType = "1";
        string strLang = "0";
        string strUserId = "SuperAdmin";
        string result = string.Empty;
        //Attendance_Report_InOutReportNew newInoutReport = new Attendance_Report_InOutReportNew();

        if (strEmpList == string.Empty)
        {
            return result;
        }
        System.Web.HttpContext.Current.Session["DBConnection"] = strDBConnection;
        LocationMaster ObjLocationMaster = new LocationMaster(strDBConnection);
        Country_Currency objCC = new Country_Currency(strDBConnection);
        Att_inOutReport objInOutReport = new Att_inOutReport(strDBConnection);
        DataTable dtlocation = ObjLocationMaster.GetLocationMasterById(strCompanyId, strLocationId);
        System.Web.HttpContext.Current.Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(dtlocation.Rows[0]["Field1"].ToString());
        //DateTime FromDate = new DateTime();
        // DateTime ToDate = new DateTime();
        string strFilePath = string.Empty;
        strFilePath = Server.MapPath("~/Temp/" + strfileName.Trim() + ".pdf");
        DevExpress.XtraReports.UI.XtraReport xrRpt = objInOutReport.GetReport(strCompanyId, strBrandId, strLocationId, strEmpList, strFromDate, strToDate, rptType, strLang, strUserId);
        xrRpt.ExportToPdf(strFilePath);
        return strFilePath;
    }

    [WebMethod(EnableSession = true)]
    
    public string GetAccessGroupSummary(string strCompanyId, string strBrandId, string strLocationId, string strEmpIdList, string strFromDate, string strTodate, string strDBConnection)
    {
        System.Web.HttpContext.Current.Session["DBConnection"] = strDBConnection;
        LocationMaster ObjLocationMaster = new LocationMaster(strDBConnection);
        Country_Currency objCC = new Country_Currency(strDBConnection);
        AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);

        DataTable dtlocation = ObjLocationMaster.GetLocationMasterById(strCompanyId, strLocationId);
        System.Web.HttpContext.Current.Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(dtlocation.Rows[0]["Field1"].ToString());
        Table Table_AcessGroupReport = objgroupsummary.GetReport1(strCompanyId, strBrandId, strLocationId, strEmpIdList, strFromDate, strTodate, false);
        string strFilePath = string.Empty;
        strFilePath = Server.MapPath("~/Temp/TSC Time Attendance Details Report_" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyyHHmmss") + ".xls");
        int rowcount = Table_AcessGroupReport.Rows.Count;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        Table_AcessGroupReport.RenderControl(hw);
        string renderedGridView = tw.ToString();
        System.IO.File.WriteAllText(strFilePath, renderedGridView);
        Excel.Application xlApp = new Excel.Application();
        xlApp.DisplayAlerts = false;
        object misValue = System.Reflection.Missing.Value;
        //Excel.Workbook xrworkbook1 = xlApplication.Workbooks.Open(strFilePath);
        Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(strFilePath);
        xlWorkBook.SaveAs(strFilePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
            Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        xlWorkBook.Close();
        xlApp.Quit();
        Marshal.ReleaseComObject(xlWorkBook);
        Marshal.ReleaseComObject(xlApp);
        return strFilePath;
    }


    [WebMethod(EnableSession = true)]

    public string GetNotRegistered(string strEmpIdList,string strDBConnection)
    {
        System.Web.HttpContext.Current.Session["DBConnection"] = strDBConnection;
        AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);
        Table Table_AcessGroupReport = objgroupsummary.GetNotRegisteredList(strEmpIdList);
        string strFilePath = string.Empty;
        strFilePath = Server.MapPath("~/Temp/TSC Not Registered Report_" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyyHHmmss") + ".xls");
        int rowcount = Table_AcessGroupReport.Rows.Count;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        Table_AcessGroupReport.RenderControl(hw);
        string renderedGridView = tw.ToString();
        System.IO.File.WriteAllText(strFilePath, renderedGridView);
        Excel.Application xlApp = new Excel.Application();
        xlApp.DisplayAlerts = false;
        object misValue = System.Reflection.Missing.Value;
        //Excel.Workbook xrworkbook1 = xlApplication.Workbooks.Open(strFilePath);
        Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(strFilePath);
        xlWorkBook.SaveAs(strFilePath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
            Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        xlWorkBook.Close();
        xlApp.Quit();
        Marshal.ReleaseComObject(xlWorkBook);
        Marshal.ReleaseComObject(xlApp);
        return strFilePath;
    }


    [WebMethod(EnableSession = true)]

    public string GetDBDetail(string strQuery)
    {
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Objda.return_DataTable(strQuery);

        return dt.Rows[0][0].ToString();

    }

    [WebMethod(EnableSession = true)]

    public string AutoLogProcess(string strCompanyId, string strBrandId, string strLocationId, string strEmpList, string strFromDate, string strToDate, string strDBConnection)
    {
        string strUserId = "SuperAdmin";
        bool result = false;
        string strlogprocessLocationName = string.Empty;
        //Attendance_Report_InOutReportNew newInoutReport = new Attendance_Report_InOutReportNew();

        if (strEmpList == string.Empty)
        {
            return result.ToString();
        }
        System.Web.HttpContext.Current.Session["DBConnection"] = strDBConnection;
        LocationMaster ObjLocationMaster = new LocationMaster(strDBConnection);
        Country_Currency objCC = new Country_Currency(strDBConnection);
        AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);
        DataAccessClass Objda = new DataAccessClass(strDBConnection);
        LogProcess ObjLogProcess = new LogProcess(strDBConnection);

        DataTable dtlocation = ObjLocationMaster.GetLocationMasterById(strCompanyId, strLocationId);
        System.Web.HttpContext.Current.Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(dtlocation.Rows[0]["Field1"].ToString());
        strlogprocessLocationName = dtlocation.Rows[0]["Location_Name"].ToString();
        DataTable dtLeaveIntegration = new DataTable();
        System.Web.HttpContext.Current.Session["LeaveIntegration"] = null;
        if (ConfigurationManager.AppSettings["LeaveIntegration"].ToString().Trim() == "1")
        {
            try
            {
                dtLeaveIntegration = Objda.GeTOracleRecord("SELECT employeecode,shiftnameleavetype,fromdate,todate FROM APPS.XXTSC_TMS_LEAVE_MF WHERE 1 = 1", ReadHRMSConStringFromFile());
                //DataTable dtLeaveIntegration = Objda.GeTOracleRecord("SELECT employeecode,shiftnameleavetype,fromdate,todate FROM APPS.XXTSC_TMS_LEAVE_MF WHERE 1 = 1 and location='" + strlogprocessLocationName + "'", ReadHRMSConStringFromFile());
                if (dtLeaveIntegration.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["LeaveIntegration"] = dtLeaveIntegration;
                }
            }
            catch
            {

            }
        }

        result = Convert.ToBoolean(ObjLogProcess.autoLogProcess(strCompanyId, strBrandId, strLocationId, strEmpList, strUserId, "", Convert.ToDateTime(strFromDate), Convert.ToDateTime(strToDate), "0", dtLeaveIntegration, HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(),ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString())[0]);


        return result.ToString();
    }


    public string ReadHRMSConStringFromFile()
    {

        try
        {
            FileStream fs = new FileStream("C:\\PegasusSQL\\HRMSCONNECTIONSTRING.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string sqlStrSetting = sw.ReadLine();
            fs.Close();
            sw.Close();
            return sqlStrSetting;
        }
        catch
        {
            return "";
        }
    }

    [WebMethod(EnableSession = true)]
    public string[] Get_Notification()
    {
        string[] strResult = new string[3];
        //LocationMaster ObjLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //Country_Currency objCC = new Country_Currency(strDBConnection);
        //AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);
        //DataAccessClass Objda = new DataAccessClass(strDBConnection);
        //LogProcess ObjLogProcess = new LogProcess(strDBConnection);
        DataAccessClass daClass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null && Session["UserId"] != null && Session["NotificationCount"] != null)
            {
                //get count for unread Reminder
                string sql = "SELECT COUNT(*) FROM set_notification_trans WHERE Is_read = 'False' AND set_notification_trans.Notification_type_id=39 AND Receipent_id ='" + Session["EmpId"].ToString() + "'";
                int rec_count = 0;
                Int32.TryParse(daClass.get_SingleValue(sql), out rec_count);
                strResult[0] = rec_count.ToString();

                //get count for unread Reminder
                sql = "SELECT COUNT(*) FROM set_notification_trans WHERE Is_read = 'False' AND set_notification_trans.Notification_type_id <> 39 AND Receipent_id ='" + Session["EmpId"].ToString() + "'";
                rec_count = 0;
                Int32.TryParse(daClass.get_SingleValue(sql), out rec_count);
                strResult[1] = rec_count.ToString();
            }

            if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null)
            {
                //get count for unread Reminder
                int rec_count = 0;
                using (DataTable dtAppSetupStatus = new DataTable())
                {
                    dtAppSetupStatus.Columns.Add("setupStatus");
                    string strStatus = string.Empty;
                    //check timetable setup
                    string sql = "select count(*) from dbo.Att_TimeTable where isActive='true' and company_id='" + Session["CompId"].ToString() + "'";
                    int resultCount = Int32.Parse(daClass.get_SingleValue(sql));
                    DataRow dr = dtAppSetupStatus.Rows.Add();
                    //dr["setupName"] = "Time Table";
                    //dr["setupStatus"] = resultCount==0? "<span class='label label-success'>Incomplete</span>" : "<span class='label label-warning'>Complete</span>";
                    rec_count = resultCount == 0 ? rec_count + 1 : rec_count;
                    dr["setupStatus"] = "Time Table " + (resultCount == 0 ? "<span class='label label-warning'>Incomplete</span>" : "<span class='label label-success'>Complete</span>");

                    //check shift setup
                    sql = "select count(*) from dbo.Att_ShiftManagement where isActive='true' and company_id='" + Session["CompId"].ToString() + "'";
                    resultCount = Int32.Parse(daClass.get_SingleValue(sql));
                    dr = dtAppSetupStatus.Rows.Add();
                    rec_count = resultCount == 0 ? rec_count + 1 : rec_count;
                    dr["setupStatus"] = "Shift setup " + (resultCount == 0 ? "<span class='label label-warning'>Incomplete</span>" : "<span class='label label-success'>Complete</span>");

                    //check shift setup
                    sql = "select count(*) from dbo.Att_DeviceMaster where isActive='true' and company_id='" + Session["CompId"].ToString() + "'";
                    resultCount = Int32.Parse(daClass.get_SingleValue(sql));
                    rec_count = resultCount == 0 ? rec_count + 1 : rec_count;
                    dr = dtAppSetupStatus.Rows.Add();
                    dr["setupStatus"] = "Device Setup " + (resultCount == 0 ? "<span class='label label-warning'>Incomplete</span>" : "<span class='label label-success'>Complete</span>");


                    if (rec_count > 0)
                    {
                        Session["tblAppSetupPendings"] = dtAppSetupStatus;
                    }
                    else
                    {
                        Session["tblAppSetupPendings"] = null;
                    }
                }
                strResult[2] = rec_count.ToString();
            }
            return strResult;
        }
        catch (Exception err)
        {
            strResult[0] = "0";
            strResult[1] = "0";
            strResult[2] = "0";
            return strResult;
        }
    }
    
    [WebMethod(EnableSession = true)]
    public string Read_Setup_Pendings()
    {
        string strResult = string.Empty;
        if (Session["tblAppSetupPendings"] == null) return strResult;
        try
        {
            using (DataTable _dt = (DataTable)Session["tblAppSetupPendings"])
            {

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    strResult = strResult + "<li class=*header*>" + _dt.Rows[i]["setupStatus"] + "</li>";
                }
                
                strResult = strResult.Replace('*', '"');
                
            }
            return strResult;
        }
        catch (Exception err)
        {
            return null;
        }
    }
    [WebMethod(EnableSession = true)]
    public string Read_Reminder(string recCount)
    {
        //LocationMaster ObjLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //Country_Currency objCC = new Country_Currency(strDBConnection);
        //AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        //LogProcess ObjLogProcess = new LogProcess(strDBConnection);

        if (recCount == "0")
        {
            Session["ReminderCount"] = 10;
        }
        else
        {
            Session["ReminderCount"] = int.Parse(Session["ReminderCount"].ToString()) + int.Parse(recCount);
        }
        string strResult = string.Empty;
        
        //DataAccessClass daClass = new DataAccessClass();
        try
        {
            if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null && Session["UserId"] != null && Session["ReminderCount"] != null)
            {
                //Mark unread reocord as read for reminder
                PassDataToSql[] paramList = new PassDataToSql[6];
                paramList[0] = new PassDataToSql("@Company_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[1] = new PassDataToSql("@Brand_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[2] = new PassDataToSql("@Location_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[3] = new PassDataToSql("@Receipent_id", Session["UserId"].ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
                paramList[4] = new PassDataToSql("@Op_Type", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
                Objda.execute_Sp("sp_Update_Notification", paramList);

                paramList = new PassDataToSql[2];
                paramList[0] = new PassDataToSql("@Receipent_id", Session["EmpId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[1] = new PassDataToSql("@num", Session["ReminderCount"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                DataTable Dt_reminder = Objda.Reuturn_Datatable_Search("sp_get_Notification_Reminder", paramList);


                string demo = "";

                for (int i = 0; i < Dt_reminder.Rows.Count; i++)
                {
                    demo = demo + "<li> <a href=" + Dt_reminder.Rows[i]["Link_url"].ToString() + " title=*" + Dt_reminder.Rows[i]["N_Message"].ToString() + "*> <i class=*fa fa-users text-aqua*></i> " + Dt_reminder.Rows[i]["N_Message"].ToString() + " </a> </li>";
                }
                if (Dt_reminder.Rows.Count < 10)
                {
                    strResult = "<li class=*header*>All Reminder Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + "  </ul> </li>";
                }
                else
                {
                    strResult = "<li class=*header*>All Reminder Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + "  </ul> </li> <li class=*footer*><a href=*#*  onclick=*Read_Reminder(10)*>View More</a></li>";
                }
                strResult = strResult.Replace('*', '"');
                Dt_reminder.Dispose();
            }
            return strResult;
        }
        catch (Exception err)
        {
            return null;
        }
    }

    [WebMethod(EnableSession = true)]
    public string Read_Notification(string recCount)
    {
        //LocationMaster ObjLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //Country_Currency objCC = new Country_Currency(strDBConnection);
        //AccessGroupSummary objgroupsummary = new AccessGroupSummary(strDBConnection);
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        //LogProcess ObjLogProcess = new LogProcess(strDBConnection);

        if (recCount == "0")
        {
            Session["NotificationCount"] = 10;
        }
        else
        {
            Session["NotificationCount"] = int.Parse(Session["NotificationCount"].ToString()) + int.Parse(recCount);
        }

        string strResult = string.Empty;
        
        try
        {
            if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null && Session["UserId"] != null && Session["NotificationCount"] != null)
            {
                PassDataToSql[] paramList = new PassDataToSql[6];
                paramList[0] = new PassDataToSql("@Company_id", Session["CompId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[1] = new PassDataToSql("@Brand_id", Session["BrandId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[2] = new PassDataToSql("@Location_id", Session["LocId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[3] = new PassDataToSql("@Receipent_id", Session["UserId"].ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
                paramList[4] = new PassDataToSql("@Op_Type", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
                Objda.execute_Sp("sp_Update_Notification", paramList);

                paramList = new PassDataToSql[5];
                paramList[0] = new PassDataToSql("@Company_id", Session["CompId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[1] = new PassDataToSql("@Brand_id", Session["BrandId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[2] = new PassDataToSql("@Location_id", Session["LocId"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                paramList[3] = new PassDataToSql("@Receipent_id", Session["UserId"].ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
                paramList[4] = new PassDataToSql("@Get_Record", Session["NotificationCount"].ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                DataTable Dt_Notification = Objda.Reuturn_Datatable_Search("sp_Get_Notification", paramList);


                string Alert_Message = string.Empty;
                string Start_Notitification = string.Empty;
                string End = string.Empty;
                Start_Notitification = "<li><ul class=*menu*>";
                if (Dt_Notification.Rows.Count < 10)
                {
                    End = "</ul></li>";
                    //End = "</ul></li><li class=*footer*><a href=*#* onclick=*Read_Notification(10)*>More</a></li>";
                }
                else
                {
                    End = "</ul></li><li class=*footer*><a href=*#* onclick=*Read_Notification(10)*>More</a></li>";
                }
                int Tot_Row = 0;
                if (Convert.ToInt16(Session["NotificationCount"].ToString()) <= Dt_Notification.Rows.Count)
                {
                    Tot_Row = Convert.ToInt16(Session["NotificationCount"].ToString());
                }
                if (Convert.ToInt16(Session["NotificationCount"].ToString()) > Dt_Notification.Rows.Count)
                {
                    Tot_Row = Convert.ToInt16(Dt_Notification.Rows.Count.ToString());
                }
                for (int i = 0; i < Tot_Row; i++)
                {
                    string Time = string.Empty;

                    if (Convert.ToInt64(Dt_Notification.Rows[i]["Years"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Months"].ToString()) >= 12 && Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) >= 366)
                    {
                        Time = Dt_Notification.Rows[i]["Years"].ToString() + " Year";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Months"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) >= 30)
                    {
                        Time = Dt_Notification.Rows[i]["Months"].ToString() + " Months";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) == 1)
                            Time = " Yesterday";
                        else
                            Time = Dt_Notification.Rows[i]["Days"].ToString() + " Days";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Hours"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Hours"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) <= 59)
                            Time = Dt_Notification.Rows[i]["Minutes"].ToString() + " Minutes";
                        else
                            Time = Dt_Notification.Rows[i]["Hours"].ToString() + " Hours";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Seconds"].ToString()) <= 59)
                            Time = Dt_Notification.Rows[i]["Seconds"].ToString() + " Seconds";
                        else
                            Time = Dt_Notification.Rows[i]["Minutes"].ToString() + " Minutes";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Seconds"].ToString()) >= 0)
                    {
                        Time = Dt_Notification.Rows[i]["Seconds"].ToString() + " Seconds";
                    }
                    int Msg_Length = 0;
                    if (Dt_Notification.Rows[i]["Message_Length"].ToString() != "")
                        Msg_Length = Convert.ToInt32(Dt_Notification.Rows[i]["Message_Length"].ToString());
                    int message_s = Dt_Notification.Rows[i]["N_Message"].ToString().Length;
                    string msg = "";
                    if (message_s > Msg_Length)
                    {
                        msg = Dt_Notification.Rows[i]["N_Message"].ToString().Substring(0, Convert.ToInt32(Msg_Length));
                    }
                    else
                    {
                        msg = Dt_Notification.Rows[i]["N_Message"].ToString();
                    }
                    string Emp_Image_Notification = Dt_Notification.Rows[i]["Field1"].ToString();

                    if (Dt_Notification.Rows[i]["Field1"].ToString() != null)
                    {
                        string strImgPath = Dt_Notification.Rows[i]["Field1"].ToString();
                        if (strImgPath != string.Empty && strImgPath.Substring(0, 2) == "..")
                        {
                            strImgPath = strImgPath.Substring(2, strImgPath.Length - 3);
                        }
                        if (File.Exists(strImgPath) == true)
                        {
                            Emp_Image_Notification = Dt_Notification.Rows[i]["Field1"].ToString();
                        }
                        else
                        {
                            Emp_Image_Notification = "../Bootstrap_Files/dist/img/Bavatar.png";
                        }
                    }
                    else
                    {
                        Emp_Image_Notification = "../Bootstrap_Files/dist/img/Bavatar.png";
                    }

                    Alert_Message = Alert_Message + "<li><a id=*mybtn* title=*" + Dt_Notification.Rows[i]["N_Message"].ToString() + "* href = *" + Dt_Notification.Rows[i]["Link_url"].ToString() + "* ><div class=*pull-left*><img src = *" + Emp_Image_Notification + "* class=*img-circle* alt=**></div><h4>" + Dt_Notification.Rows[i]["Title"].ToString() + "<small><i class=*fa fa-clock-o*></i>" + " " + "" + Time + "</small></h4><p>" + msg + "</p></a></li>";
                }

                strResult = Start_Notitification.Replace('*', '"') + Alert_Message.Replace('*', '"') + End.Replace('*', '"');
                Dt_Notification.Dispose();
            }

            return strResult;
        }
        catch (Exception err)
        {
            return null;
        }
    }
    [WebMethod(enableSession: true)]
    public void fillTblInvoice(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, string strRecordType)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        int totalRecord = 0;
        int totalDisplayRecord = 0;
        
        List<Inv_SalesInvoiceHeader.clsInvoiceList> lstClsInvLst = new List<Inv_SalesInvoiceHeader.clsInvoiceList> { };
        try
        {
            Inv_SalesInvoiceHeader objSInvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            using (DataTable dtInvoice = objSInvHeader.GetInvoiceList(HttpContext.Current.Session["LocId"].ToString(), strRecordType, displayLength.ToString(), displayStart.ToString(), sortCol.ToString(), sortDir, search))
            {
                if (dtInvoice.Rows.Count > 0)
                {

                    totalRecord = string.IsNullOrEmpty(sSearch) ? Convert.ToInt32(dtInvoice.Rows[0]["TotalCount"].ToString()) : objSInvHeader.GetInvoiceListCount(HttpContext.Current.Session["LocId"].ToString(), strRecordType);
                    totalDisplayRecord = Convert.ToInt32(dtInvoice.Rows[0]["TotalCount"].ToString());
                    foreach (DataRow dr in dtInvoice.Rows)
                    {
                        Inv_SalesInvoiceHeader.clsInvoiceList clsInvLst = new Inv_SalesInvoiceHeader.clsInvoiceList();
                        clsInvLst.salesInvoiceId = dr["salesInvoiceId"].ToString();
                        clsInvLst.invoiceNo = dr["Invoice_No"].ToString();
                        clsInvLst.invoiceDate = DateTime.Parse(dr["Invoice_Date"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss");
                        clsInvLst.refType = dr["Ref_Type"].ToString();
                        clsInvLst.refNo = dr["OrderList"].ToString();
                        clsInvLst.salesPerson = dr["salesPersonName"].ToString();
                        clsInvLst.createdBy = dr["InvoiceCreatedBy"].ToString();
                        clsInvLst.approvalStatus = dr["Field4"].ToString();
                        clsInvLst.currencyCode = dr["Currency_Code"].ToString();
                        clsInvLst.customerName = dr["CustomerName"].ToString();
                        clsInvLst.invoiceAmount = SystemParameter.GetAmountWithDecimal(dr["GrandTotalWithExpenses"].ToString(), dr["currencyDecimalCount"].ToString());
                        lstClsInvLst.Add(clsInvLst);
                    }

                    //return (js.Serialize(result));
                }
                else
                {
                    totalRecord = objSInvHeader.GetInvoiceListCount(HttpContext.Current.Session["LocId"].ToString(), strRecordType);
                    totalDisplayRecord = 0;
                }
                var result = new
                {
                    iTotalRecords = totalRecord,
                    iTotalDisplayRecords = totalDisplayRecord,
                    aaData = lstClsInvLst
                };
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpContext.Current.Response.Write(js.Serialize(result));

            }
        }
        catch (Exception ex)
        {
            var result = new
            {
                iTotalRecords = 0,
                iTotalDisplayRecords = 0,
                aaData = lstClsInvLst
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            HttpContext.Current.Response.Write(js.Serialize(result));
        }
    }


}
