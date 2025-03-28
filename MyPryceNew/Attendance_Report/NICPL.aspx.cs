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
using DevExpress.XtraReports.UI;
using PegasusDataAccess;

public partial class Attendance_Report_NICPL : System.Web.UI.Page
{
    XtraReport RptNICPL = new XtraReport();
    //Att_AbsentReport RptShift = new Att_AbsentReport();
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_Employee_Notification objEmpNotice = null;
    PegasusDataAccess.DataAccessClass objDA = null;



    protected void Page_Load(object sender, EventArgs e)
    {
        RptNICPL.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "NICPL.repx");


        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        GetReport();
    }
    public void GetReport()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        string Trans_Id = Request.QueryString["TransId"].ToString();
        DataTable dtFilter = new DataTable();
        FromDate = DateTime.Now;
        ToDate  = DateTime.Now;

        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@FromDate", FromDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ToDate", ToDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emplist, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtFilter = objDA.Reuturn_Datatable_Search("sp_Att_NIC_PL_Report", paramList);

        XRLabel lblUserName = (XRLabel)RptNICPL.FindControl("lblUserName", true);
        lblUserName.Text = Session["UserId"].ToString();

        XRPictureBox pBox = (XRPictureBox)RptNICPL.FindControl("pBox", true);
        try
        {
            pBox.ImageUrl = dtFilter.Rows[0]["f51"].ToString();

        }
        catch
        {

        }

        XRLabel lblAOut = (XRLabel)RptNICPL.FindControl("lblAOut", true);
    

        XRLabel lblAIn = (XRLabel)RptNICPL.FindControl("lblAIn", true);
       
        char[] spearator = { ':' };
        DateTime tTemp = DateTime.Now;

        DateTime t1 = new DateTime(tTemp.Year, tTemp.Month, tTemp.Day, Convert.ToInt16(dtFilter.Rows[0]["From_Time"].ToString().Split(spearator, StringSplitOptions.None)[0]), Convert.ToInt16(dtFilter.Rows[0]["From_Time"].ToString().Split(spearator, StringSplitOptions.None)[1]), 0);
        DateTime t2 = new DateTime(tTemp.Year, tTemp.Month, tTemp.Day, Convert.ToInt16(dtFilter.Rows[0]["To_Time"].ToString().Split(spearator, StringSplitOptions.None)[0]), Convert.ToInt16(dtFilter.Rows[0]["To_Time"].ToString().Split(spearator, StringSplitOptions.None)[1]), 0);

        DataTable dtAttendanceRecord = objDA .return_DataTable("Select Att_AttendanceRegister .Emp_Id , Att_AttendanceRegister .Att_Date ,  Att_AttendanceRegister .OnDuty_Time ,Att_AttendanceRegister .OffDuty_Time , Att_AttendanceRegister .In_Time ,Att_AttendanceRegister .Out_Time , LateMin , EarlyMin , (SELECT TOP 1 Cast(Cast(event_time AS TIME) AS VARCHAR(5))                     FROM   att_attendancelog                     WHERE  emp_id = att_attendanceregister.Emp_Id                             AND event_date = Cast(att_attendanceregister.att_date                                                  AS                                                  DATE                                             )                            AND func_code = '2'                            AND att_attendanceregister.emp_id =                                att_attendancelog.emp_id                     ORDER  BY event_time ASC)                    AS SIn,                    (SELECT TOP 1 Cast(Cast(event_time AS TIME) AS VARCHAR(5))                     FROM   att_attendancelog                      WHERE  emp_id =  att_attendanceregister.Emp_Id                             AND event_date = Cast(att_attendanceregister.att_date                                                  AS                                                  DATE                                             )                            AND func_code = '2'                            AND att_attendanceregister.emp_id =                                att_attendancelog.emp_id                     ORDER  BY event_time DESC)                    AS SOut    From   Att_AttendanceRegister where Att_AttendanceRegister.Emp_Id = " + dtFilter.Rows[0]["f41"].ToString() + " and Att_AttendanceRegister.Att_Date ='" + dtFilter.Rows[0]["Partial_Leave_Date"].ToString() + "'             ");

        if (dtAttendanceRecord.Rows.Count > 0)
        {
            if (Convert.ToInt32(dtAttendanceRecord.Rows[0]["LateMin"].ToString()) > 0)
            {
                lblAIn.Text = "";
                lblAOut.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["In_Time"].ToString()).ToString("HH:mm");

            }
            else if (Convert.ToInt32(dtAttendanceRecord.Rows[0]["EarlyMin"].ToString()) > 0)
            {
                
                lblAOut.Text = "";
                lblAIn.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["Out_Time"].ToString()).ToString("HH:mm");

            }
            else
            {
                lblAIn.Text = "";
                lblAOut.Text = "";
            }

        }

        if (t1>t2)
        {
            t2 = t2.AddDays(1);
        }

        TimeSpan duration = t2 - t1;

        string strDur = duration.Hours.ToString("00") + ":" + duration.Minutes.ToString("00");

        XRLabel lblD = (XRLabel)RptNICPL.FindControl("lblD", true);
        lblD.Text = strDur;
        XRLabel lblD1 = (XRLabel)RptNICPL.FindControl("lblD1", true);
        lblD1.Text = strDur;
        XRLabel lblD2 = (XRLabel)RptNICPL.FindControl("lblD2", true);
        lblD2.Text = strDur;





        RptNICPL.DataSource = dtFilter;
        RptNICPL.DataMember = "CustomSqlQuery";
        rptViewer.Report = RptNICPL;
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