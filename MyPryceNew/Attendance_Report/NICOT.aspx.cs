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

public partial class Attendance_Report_NICOT : System.Web.UI.Page
{
    XtraReport RptTimeCard = new XtraReport();
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
        RptTimeCard.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "NICOT.repx");


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

            EmpReport = Session["EmpList"].ToString();

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "14");

            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }



            DataTable dtFilter = new DataTable();



            Emplist = Emplist.Substring(0, Emplist.Length - 1);

            // dtFilter = objDA.return_DataTable("Select Set_CompanyMaster.Company_Name, Set_LocationMaster.Location_Name_L ,  emp_code ,Set_Employeemaster.Emp_Name ,Set_Employeemaster.Emp_Name_L ,  Set_DepartmentMaster.Dep_Name,Set_DepartmentMaster.Dep_Name_L  ,Set_DesignationMaster.Designation,Set_DesignationMaster.Designation_L ,Set_EmployeeMaster.Grade     , Cast(Att_AttendanceRegister.Att_date as Date) as Date,Day(Att_AttendanceRegister.Att_date) as Day, cast(Cast(OnDuty_Time as Time) as varchar(5)) as ShiftStart ,Cast(Cast(OffDuty_Time as Time) as varchar(5)) as ShiftEnd, Cast(Cast(In_Time as Time) as varchar(5))as RIn,Cast(Cast(Out_Time as Time) as varchar(5)) as ROut,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'  and   Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id     order by Event_Time  ASC)  as SIn,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)   as SOut , dbo.[MinutesToDuration]((Select Work_Minute   From  Att_TimeTable  Where Att_TimeTable.TimeTable_Id = Att_AttendanceRegister.TimeTable_Id   ) ) as 'ShiftHR' ,  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end) as 'NetHr'  ,  dbo.[MinutesToDuration](LateMin)  as 'DIn', dbo.[MinutesToDuration]( DATEDIFF( minute,  (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  ASC), (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'     and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)      ))as Normal,   dbo.[MinutesToDuration](EarlyMin)  as 'Dout', dbo.[MinutesToDuration]( (Select Field2  From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = Att_AttendanceRegister.Att_Date and    Att_ScheduleDescription.Emp_Id =  Att_AttendanceRegister.Emp_Id))  as PDOT,      Case  When  DATENAME(DW, Att_AttendanceRegister.Att_Date ) = 'Friday' then  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end)      else  case When OffDuty_Time   <> '1900-01-01 00:00:00.000'  then  case  when datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ) >=  60 then  dbo.[MinutesToDuration](datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ))  else '' end  else  '' end end    as AOT,   Case When   Att_AttendanceRegister.HR_Status = '1' then   dbo.[MinutesToDuration](Att_AttendanceRegister.OverTime_Min) end as ApprovedOT,  (Select TimeTable_Name   From  Att_TimeTable  Where  Att_TimeTable.TimeTable_Id =   Att_AttendanceRegister.TimeTable_Id) as  Calc,Case When  Is_Absent = 1 then  'AB' else 'PR' end    as Status, Att_AttendanceRegister.Is_Absent ,Att_AttendanceRegister.Is_Holiday ,Att_AttendanceRegister.Is_Leave,Att_AttendanceRegister.Is_Week_Off  From Att_AttendanceRegister  INNER JOIN  Set_Employeemaster ON  Att_AttendanceRegister.Emp_Id =  Set_Employeemaster.Emp_Id INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id  = Set_EmployeeMaster .Location_Id   INNER JOIN Set_CompanyMaster   On Set_CompanyMaster.Company_Id   = Set_EmployeeMaster .Company_Id     INNER JOIN Set_DepartmentMaster On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster .Department_Id  INNER JOIN Set_DesignationMaster On Set_DesignationMaster.Designation_Id = Set_EmployeeMaster .Designation_Id   where   Att_AttendanceRegister.Emp_id  In  (" + Emplist + ")    and (Att_Date >= '" + FromDate + "' and Att_Date <= '" + ToDate + "') Order by Att_AttendanceRegister.Emp_id,Att_Date             ");

            PassDataToSql[] paramList = new PassDataToSql[4];
            paramList[0] = new PassDataToSql("@FromDate", FromDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@ToDate", ToDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Emp_Id", Emplist, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


            dtFilter = objDA.Reuturn_Datatable_Search("sp_Att_NIC_OT_Report", paramList);





            //  XRLabel lblFromDate = (XRLabel)RptTimeCard.FindControl("lblFromDate", true);
            //  lblFromDate.Text = FromDate.ToString("dd-MM-yyyy");


            //    XRLabel lblToDate = (XRLabel)RptTimeCard.FindControl("lblToDate", true);
            //  lblToDate.Text = ToDate.ToString("dd-MM-yyyy");


            //XRLabel lblUserName = (XRLabel)RptTimeCard.FindControl("lblUserName", true);
            //lblUserName.Text = Session["UserId"].ToString();





            RptTimeCard.DataSource = dtFilter;
            RptTimeCard.DataMember = "CustomSqlQuery";
            rptViewer.Report = RptTimeCard;
            rptToolBar.ReportViewer = rptViewer;

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

}