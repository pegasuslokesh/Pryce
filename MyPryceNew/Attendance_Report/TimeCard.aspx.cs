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

public partial class Attendance_Report_TimeCard : BasePage
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
        RptTimeCard.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "TimeCard.repx");


        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        GetReport();
        if (!IsPostBack)
        {

            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "99", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}

            //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

            ////New Code 
            //string strModuleId = string.Empty;
            //string strModuleName = string.Empty;

            //DataTable dtModule = objObjectEntry.GetModuleIdAndName("99", (DataTable)Session["ModuleName"]);
            //if (dtModule.Rows.Count > 0)
            //{
            //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
            //}
            //else
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}
            ////End Code





            //Session["AccordianId"] = strModuleId;
            //Session["HeaderText"] = strModuleName;



        }
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

            //this code is created by jitendra upadhyay on 19-09-2014
            //this code for employee should not be showing in repot if not exists in employee notification for absent report
            //code start

            //code update on 27-09-2014

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "14");

            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }

            //code end

            //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
            //{
            //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

            //     if (dtEmpNF.Rows.Count > 0)
            //     {
            //         if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Rpt_Absent"].ToString()))
            //         {
            //             Emplist += EmpReport.Split(',')[i].ToString() + ",";
            //         }
            //     }
            //}

            //code end

            DataTable dtFilter = new DataTable();

            //AttendanceDataSet rptdata = new AttendanceDataSet();



            //rptdata.EnforceConstraints = false;
            //AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();

            //adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            //try
            //{
            //    adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist, 1);
            //}
            //catch
            //{
            //}
            //dtFilter = rptdata.sp_Att_AttendanceRegister_Report;




            //string CompanyName = "";
            //string CompanyAddress = "";
            //string Imageurl = "";
            //string BrandName = "";
            //string LocationName = "";
            //string DepartmentName = "";
            //// Get Company Name
            //CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
            //// Image Url
            //Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
            //// Get Brand Name
            //BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());
            //// Get Location Name
            //if (Session["LocationName"].ToString() == "")
            //{
            //    LocationName = objAttendance.GetLocationName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["Lang"].ToString());
            //}
            //else
            //{
            //    LocationName = Session["LocationName"].ToString();
            //}
            //// Get Department Name
            //if (Session["DepName"].ToString() == "")
            //{
            //    DepartmentName = "All";
            //}
            //else
            //{
            //    DepartmentName = Session["DepName"].ToString();
            //}
            //// Get Company Address
            //DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
            //if (DtAddress.Rows.Count > 0)
            //{
            //    CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            //}


            ////---Company Logo
            //XRPictureBox Company_Logo = (XRPictureBox)RptShift.FindControl("Company_Logo", true);
            //try
            //{
            //    Company_Logo.ImageUrl = Imageurl;
            //}
            //catch
            //{
            //}
            ////------------------

            ////Comany Name
            //XRLabel Lbl_Company_Name = (XRLabel)RptShift.FindControl("Lbl_Company_Name", true);
            //Lbl_Company_Name.Text = CompanyName;
            ////------------------

            ////Comapny Address
            //XRLabel Lbl_Company_Address = (XRLabel)RptShift.FindControl("Lbl_Company_Address", true);
            //Lbl_Company_Address.Text = CompanyAddress;
            ////------------------


            ////Brand Name
            //XRLabel Lbl_Brand = (XRLabel)RptShift.FindControl("Lbl_Brand", true);
            //Lbl_Brand.Text = Resources.Attendance.Brand + " : ";
            //XRLabel Lbl_Brand_Name = (XRLabel)RptShift.FindControl("Lbl_Brand_Name", true);
            //Lbl_Brand_Name.Text = BrandName;
            ////------------------

            //// Location Name
            //XRLabel Lbl_Location = (XRLabel)RptShift.FindControl("Lbl_Location", true);
            //Lbl_Location.Text = Resources.Attendance.Location + " : ";
            //XRLabel Lbl_Location_Name = (XRLabel)RptShift.FindControl("Lbl_Location_Name", true);
            //Lbl_Location_Name.Text = LocationName;
            ////------------------

            //// Department Name
            //XRLabel Lbl_Department = (XRLabel)RptShift.FindControl("Lbl_Department", true);
            //Lbl_Department.Text = Resources.Attendance.Department + " : ";
            //XRLabel Lbl_Department_Name = (XRLabel)RptShift.FindControl("Lbl_Department_Name", true);
            //Lbl_Department_Name.Text = DepartmentName;
            ////------------------

            //// Report Title
            //XRLabel Report_Title = (XRLabel)RptShift.FindControl("Report_Title", true);
            //Report_Title.Text = "Absent Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
            ////------------------



            //// Detail Header Table
            //XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            //xrTableCell7.Text = "Date";

            //XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            //xrTableCell1.Text = "Shift Name";

            //XRLabel xrTableCell21 = (XRLabel)RptShift.FindControl("xrTableCell21", true);
            //xrTableCell21.Text = "Time Table Name";

            //XRLabel xrTableCell5 = (XRLabel)RptShift.FindControl("xrTableCell5", true);
            //xrTableCell5.Text = "On Duty Time";

            //XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            //xrTableCell2.Text = "Off Duty Time";

            ////XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            ////xrTableCell4.Text = "In Time";

            ////XRLabel xrTableCell6 = (XRLabel)RptShift.FindControl("xrTableCell6", true);
            ////xrTableCell6.Text = "Out Time";
            ////-------------------------------------------

            //// Detail Id And Name
            //XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            //xrTableCell13.Text = Resources.Attendance.Id;

            //XRLabel xrTableCell17 = (XRLabel)RptShift.FindControl("xrTableCell17", true);
            //xrTableCell17.Text = Resources.Attendance.Name;
            ////--------------------------------------------

            ////Footer
            //// Create by
            //XRLabel Lbl_Created_By = (XRLabel)RptShift.FindControl("Lbl_Created_By", true);
            //Lbl_Created_By.Text = Resources.Attendance.Created_By;
            //XRLabel Lbl_Created_By_Name = (XRLabel)RptShift.FindControl("Lbl_Created_By_Name", true);
            //Lbl_Created_By_Name.Text = Session["UserId"].ToString();
            //--------------------




            //RptShift.SetImage(Imageurl);
            //RptShift.SetBrandName(BrandName);
            //RptShift.SetLocationName(LocationName);
            //RptShift.SetDepartmentName(DepartmentName);
            //RptShift.setTitleName("Absent Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            //RptShift.setcompanyname(CompanyName);
            //RptShift.setaddress(CompanyAddress);
           
            //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Absent_Count);

            //dtFilter = objDA.return_DataTable("Select emp_code ,Set_Employeemaster.Emp_Name ,  Set_DepartmentMaster.Dep_Name ,Set_DesignationMaster.Designation  , Cast(Att_AttendanceRegister.Att_date as Date) as Date,Day(Att_AttendanceRegister.Att_date) as Day, cast(Cast(OnDuty_Time as Time) as varchar(5)) as ShiftStart ,Cast(Cast(OffDuty_Time as Time) as varchar(5)) as ShiftEnd, Cast(Cast(In_Time as Time) as varchar(5))as RIn,Cast(Cast(Out_Time as Time) as varchar(5)) as ROut,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'  and   Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id     order by Event_Time  ASC)  as SIn,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)   as SOut , dbo.[MinutesToDuration]((Select Work_Minute   From  Att_TimeTable  Where Att_TimeTable.TimeTable_Id = Att_AttendanceRegister.TimeTable_Id   ) ) as 'ShiftHR' ,  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end) as 'NetHr'  ,  dbo.[MinutesToDuration](LateMin)  as 'DIn', dbo.[MinutesToDuration]( DATEDIFF( minute,  (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  ASC), (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'     and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)      ))as Normal,   dbo.[MinutesToDuration](EarlyMin)  as 'Dout', dbo.[MinutesToDuration]( (Select Field2  From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = Att_AttendanceRegister.Att_Date and    Att_ScheduleDescription.Emp_Id =  Att_AttendanceRegister.Emp_Id))  as PDOT,     case When OffDuty_Time   <> '1900-01-01 00:00:00.000'  then  dbo.[MinutesToDuration](datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  )) else  '00:00' end  as AOT,  dbo.[MinutesToDuration](Att_AttendanceRegister.OverTime_Min) as ApprovedOT,'' Calc,'' as Status, Att_AttendanceRegister.Is_Absent ,Att_AttendanceRegister.Is_Holiday ,Att_AttendanceRegister.Is_Leave,Att_AttendanceRegister.Is_Week_Off  From Att_AttendanceRegister  INNER JOIN  Set_Employeemaster ON  Att_AttendanceRegister.Emp_Id =  Set_Employeemaster.Emp_Id INNER JOIN Set_DepartmentMaster On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster .Department_Id  INNER JOIN Set_DesignationMaster On Set_DesignationMaster.Designation_Id = Set_EmployeeMaster .Designation_Id  where   Att_AttendanceRegister.Emp_id  In  (1492,2527,1911)  and Month(Att_Date) =  6 and Year(Att_Date) =  2021 Order by Att_AttendanceRegister.Emp_id,Att_Date           ");
            Emplist = Emplist.Substring(0, Emplist.Length - 1);
            //dtFilter = objDA.return_DataTable("Select emp_code ,Set_Employeemaster.Emp_Name ,  Set_DepartmentMaster.Dep_Name ,Set_DesignationMaster.Designation  , Cast(Att_AttendanceRegister.Att_date as Date) as Date,Day(Att_AttendanceRegister.Att_date) as Day, cast(Cast(OnDuty_Time as Time) as varchar(5)) as ShiftStart ,Cast(Cast(OffDuty_Time as Time) as varchar(5)) as ShiftEnd, Cast(Cast(In_Time as Time) as varchar(5))as RIn,Cast(Cast(Out_Time as Time) as varchar(5)) as ROut,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'  and   Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id     order by Event_Time  ASC)  as SIn,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)   as SOut , dbo.[MinutesToDuration]((Select Work_Minute   From  Att_TimeTable  Where Att_TimeTable.TimeTable_Id = Att_AttendanceRegister.TimeTable_Id   ) ) as 'ShiftHR' ,  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end) as 'NetHr'  ,  dbo.[MinutesToDuration](LateMin)  as 'DIn', dbo.[MinutesToDuration]( DATEDIFF( minute,  (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  ASC), (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'     and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)      ))as Normal,   dbo.[MinutesToDuration](EarlyMin)  as 'Dout', dbo.[MinutesToDuration]( (Select Field2  From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = Att_AttendanceRegister.Att_Date and    Att_ScheduleDescription.Emp_Id =  Att_AttendanceRegister.Emp_Id))  as PDOT,     Case  When  DATENAME(DW, Att_AttendanceRegister.Att_Date ) = 'Friday' then  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end)     else   case When OffDuty_Time   <> '1900-01-01 00:00:00.000'  then  case  when datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ) >  0 then  dbo.[MinutesToDuration](datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ))  else '00:00' end  else  '00:00' end end  as AOT,  dbo.[MinutesToDuration](Att_AttendanceRegister.OverTime_Min) as ApprovedOT,'' Calc,'' as Status, Att_AttendanceRegister.Is_Absent ,Att_AttendanceRegister.Is_Holiday ,Att_AttendanceRegister.Is_Leave,Att_AttendanceRegister.Is_Week_Off  From Att_AttendanceRegister  INNER JOIN  Set_Employeemaster ON  Att_AttendanceRegister.Emp_Id =  Set_Employeemaster.Emp_Id INNER JOIN Set_DepartmentMaster On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster .Department_Id  INNER JOIN Set_DesignationMaster On Set_DesignationMaster.Designation_Id = Set_EmployeeMaster .Designation_Id  where   Att_AttendanceRegister.Emp_id  In  (" +  Emplist+")  and (Att_Date >= '"+ FromDate + "' and Att_Date <= '"+ ToDate + "') Order by Att_AttendanceRegister.Emp_id,Att_Date           ");

            //dtFilter = objDA.return_DataTable("Select Set_CompanyMaster.Company_Name, Set_LocationMaster.Location_Name_L ,  emp_code ,Set_Employeemaster.Emp_Name ,Set_Employeemaster.Emp_Name_L ,  Set_DepartmentMaster.Dep_Name,Set_DepartmentMaster.Dep_Name_L  ,Set_DesignationMaster.Designation,Set_DesignationMaster.Designation_L ,Set_EmployeeMaster.Grade     , Cast(Att_AttendanceRegister.Att_date as Date) as Date,Day(Att_AttendanceRegister.Att_date) as Day, cast(Cast(OnDuty_Time as Time) as varchar(5)) as ShiftStart ,Cast(Cast(OffDuty_Time as Time) as varchar(5)) as ShiftEnd, Cast(Cast(In_Time as Time) as varchar(5))as RIn,Cast(Cast(Out_Time as Time) as varchar(5)) as ROut,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'  and   Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id     order by Event_Time  ASC)  as SIn,  (Select Top 1 Cast(Cast(Event_Time as Time)  as varchar(5))  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)   as SOut , dbo.[MinutesToDuration]((Select Work_Minute   From  Att_TimeTable  Where Att_TimeTable.TimeTable_Id = Att_AttendanceRegister.TimeTable_Id   ) ) as 'ShiftHR' ,  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end) as 'NetHr'  ,  dbo.[MinutesToDuration](LateMin)  as 'DIn', dbo.[MinutesToDuration]( DATEDIFF( minute,  (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'    and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  ASC), (Select Top 1 Cast(Event_Time as Time)  From Att_AttendanceLog Where Emp_Id = 2527 and Event_Date = Cast(Att_AttendanceRegister.Att_Date as Date) and Func_Code ='2'     and    Att_AttendanceRegister.Emp_Id = Att_AttendanceLog.Emp_Id    order by Event_Time  DESC)      ))as Normal,   dbo.[MinutesToDuration](EarlyMin)  as 'Dout', dbo.[MinutesToDuration]( (Select Field2  From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = Att_AttendanceRegister.Att_Date and    Att_ScheduleDescription.Emp_Id =  Att_AttendanceRegister.Emp_Id))  as PDOT,      Case  When  DATENAME(DW, Att_AttendanceRegister.Att_Date ) = 'Friday' then  dbo.[MinutesToDuration](case   When EffectiveWork_Min > 0  then   (Att_AttendanceRegister.TotalAssign_Min -LateMin - EarlyMin )  Else  0 end)      else  case When OffDuty_Time   <> '1900-01-01 00:00:00.000'  then  case  when datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ) >=  60 then  dbo.[MinutesToDuration](datediff(minute,Cast(OffDuty_Time as Time), Cast(Out_Time as Time)  ))  else '' end  else  '' end end    as AOT,   Case When   Att_AttendanceRegister.HR_Status = '1' then   dbo.[MinutesToDuration](Att_AttendanceRegister.OverTime_Min) end as ApprovedOT,  (Select TimeTable_Name   From  Att_TimeTable  Where  Att_TimeTable.TimeTable_Id =   Att_AttendanceRegister.TimeTable_Id) as  Calc,Case When  Is_Absent = 1 then  'AB' else 'PR' end    as Status, Att_AttendanceRegister.Is_Absent ,Att_AttendanceRegister.Is_Holiday ,Att_AttendanceRegister.Is_Leave,Att_AttendanceRegister.Is_Week_Off  From Att_AttendanceRegister  INNER JOIN  Set_Employeemaster ON  Att_AttendanceRegister.Emp_Id =  Set_Employeemaster.Emp_Id INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id  = Set_EmployeeMaster .Location_Id   INNER JOIN Set_CompanyMaster   On Set_CompanyMaster.Company_Id   = Set_EmployeeMaster .Company_Id     INNER JOIN Set_DepartmentMaster On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster .Department_Id  INNER JOIN Set_DesignationMaster On Set_DesignationMaster.Designation_Id = Set_EmployeeMaster .Designation_Id   where   Att_AttendanceRegister.Emp_id  In  (" + Emplist + ")    and (Att_Date >= '" + FromDate + "' and Att_Date <= '" + ToDate + "') Order by Att_AttendanceRegister.Emp_id,Att_Date             ");

            //502,503,505,509,511,512,513,514,517,519


            PassDataToSql[] paramList = new PassDataToSql[4];
            paramList[0] = new PassDataToSql("@FromDate", FromDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@ToDate", ToDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Emp_Id", Emplist, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


            dtFilter = objDA.Reuturn_Datatable_Search("sp_Att_NIC_Report", paramList);

           

            // Distinct Employee Get 
            // Remvoe Ahead date values 
            // NetHR Total Set Here





            // Logic For Over Time Report.

            //int dDayDifference = (ToDate - FromDate).Days;

            //DataTable dtOTReport = new DataTable("OTReport");
            //dtOTReport.Columns.Add(new DataColumn("location_name_l"));
            //dtOTReport.Columns.Add(new DataColumn("emp_code"));
            //dtOTReport.Columns.Add(new DataColumn("emp_name"));
            //dtOTReport.Columns.Add(new DataColumn("designation"));
            //dtOTReport.Columns.Add(new DataColumn("dep_name"));
            //dtOTReport.Columns.Add(new DataColumn("Dep_Code"));

            //int i = 1;
            //while(i <= dDayDifference)
            //{
            //    dtOTReport.Columns.Add(i.ToString());
            //    i++;
            //}

            //dtOTReport.Columns.Add(new DataColumn("Total"));
            //dtOTReport.Columns.Add(new DataColumn("Remarks"));


            // Distinct Employee
            // Loop Employee
            // Create Row and Fill Data then add in table





            //



            XRLabel lblFromDate = (XRLabel)RptTimeCard.FindControl("lblFromDate", true);
            lblFromDate.Text = FromDate.ToString("dd-MM-yyyy");


            XRLabel lblToDate = (XRLabel)RptTimeCard.FindControl("lblToDate", true);
            lblToDate.Text = ToDate.ToString("dd-MM-yyyy");


            //XRLabel lblTotalMin = (XRLabel)RptTimeCard.FindControl("lblTotalMin", true);
            //lblTotalMin.BeforePrint += LblTotalMin_BeforePrint;

              XRLabel lblUserName = (XRLabel)RptTimeCard.FindControl("lblUserName", true);
            lblUserName.Text = Session["UserId"].ToString();


           XRTableRow xrTRow =(XRTableRow)RptTimeCard.FindControl("tableRow1", true);
            xrTRow.BeforePrint += XrTRow_BeforePrint;


            RptTimeCard.DataSource = dtFilter;
            RptTimeCard.DataMember = "CustomSqlQuery";
            rptViewer.Report = RptTimeCard;
            rptToolBar.ReportViewer = rptViewer;

        }
    }

    //private void LblTotalMin_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    string str = ((XRLabel)sender).Text;
    //    string strResult = "";
    //    if(str.Length >0)
    //    {
    //        int tMin = 0;
    //        try
    //        {
    //            tMin= Convert.ToInt32(str);
    //            strResult = (Convert.ToInt16(tMin / 60).ToString() + ":" + Convert.ToUInt16(tMin % 60));
    //        }
    //        catch
    //        {

    //        }
             
    //    }
        
    //}

    private void XrTRow_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

        try
        {
            XRTableRow row = sender as XRTableRow;

            string strDate = ((XRTableCell)row.Cells[0]).Text.ToString();


            string[] dParam = strDate.Split(new Char[] { '-' });
            int y, m, d;
            y = Convert.ToInt32("20" + dParam[2]);
            m = Convert.ToInt32(dParam[1]);
            d = Convert.ToInt32(dParam[0]);

            if (new DateTime(y, m, d).DayOfWeek == DayOfWeek.Friday)
            {
                row.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);

            }
            else
            {
                row.BackColor = System.Drawing.Color.White;
            }
        }
        catch
        {

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