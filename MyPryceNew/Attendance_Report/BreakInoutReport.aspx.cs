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

public partial class Attendance_Report_BreakInoutReportNew : BasePage
{
    XtraReport RptShift = new XtraReport();
    //Att_BreakInOut RptShift = new Att_BreakInOut();
    Attendance objAttendance = null;
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Att_ScheduleMaster objEmpSch = null;
    Set_AddressChild ObjAddress = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objEmpParam = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_BreakInOut.repx");

        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
        if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "206", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        GetReport();
        //New Code 

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("206", (DataTable)Session["ModuleName"]);
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
        //End Code

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
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

        DataTable DtBreak = new DataTable();
        DtBreak.Columns.Add("EmpCode");
        DtBreak.Columns.Add("EmpName");
        DtBreak.Columns.Add("BreakInTime");
        DtBreak.Columns.Add("BreakOutTime");
        DtBreak.Columns.Add("Date");
        DtBreak.Columns.Add("Duration");
        DtBreak.Columns.Add("AssignBreakMin");
        DtBreak.Columns.Add("ViolationMin");
        DtBreak.Columns.Add("WorkMethod");
        DtBreak.Columns.Add("DayBreakMin");


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
            EmpReport = Session["EmpList"].ToString();

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "18");


            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
            //{
            //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

            //    if (dtEmpNF.Rows.Count > 0)
            //    {
            //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Rpt_Violation"].ToString()))
            //        {
            //            Emplist += EmpReport.Split(',')[i].ToString() + ",";
            //        }
            //    }
            //}

            //code end

            DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter();

            //adp.Fill(rptdata.sp_Att_AttendanceLog_Report, int.Parse(Session["CompId"].ToString()), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            if (Emplist != "")
            {
                dtFilter = new DataView(rptdata.sp_Att_AttendanceLog_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
            }

            string InKey = string.Empty;
            string OutKey = string.Empty;
            string BreakInKey = string.Empty;
            string BreakOutKey = string.Empty;

            string CompanyWorkCalMethod = string.Empty;

            InKey = objAppParam.GetApplicationParameterValueByParamName("In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            OutKey = objAppParam.GetApplicationParameterValueByParamName("Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            BreakInKey = objAppParam.GetApplicationParameterValueByParamName("Break In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            BreakOutKey = objAppParam.GetApplicationParameterValueByParamName("Break Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            CompanyWorkCalMethod = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            DataTable dtFilterEmp = new DataTable();
            if (dtFilter.Rows.Count > 0)
            {
                dtFilterEmp = dtFilter.DefaultView.ToTable(true, "Emp_Id");
            }

            for (int i = 0; i < dtFilterEmp.Rows.Count; i++)
            {
                string EmpCalMethod = string.Empty;
                EmpCalMethod = objEmpParam.GetEmployeeParameterByParameterName(dtFilterEmp.Rows[i]["Emp_Id"].ToString(), "Effective_Work_Cal_Method");

                if (EmpCalMethod == "")
                {
                    EmpCalMethod = CompanyWorkCalMethod;
                }

                DataTable dtLog = new DataTable();
                dtLog = new DataView(dtFilter, "Emp_Id='" + dtFilterEmp.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                int BreakMin = 0;
                int EmpBreakMin = 0;
                int DayBreakMin = 0;
                FromDate = objSys.getDateForInput(Session["FromDate"].ToString());

                DateTime OnDutyTime = new DateTime();
                DateTime OffDutyTime = new DateTime();
                string BeginingIn = string.Empty;
                string BeginingOut = string.Empty;
                string EndingIn = string.Empty;
                string EndingOut = string.Empty;

                DateTime BIn1 = new DateTime();
                DateTime BOut1 = new DateTime();
                DateTime EIn = new DateTime();
                DateTime EOut = new DateTime();

                while (FromDate <= ToDate)
                {
                    string BIn = string.Empty;
                    string Bout = string.Empty;
                    string Duration = string.Empty;


                    DataTable dt = new DataTable();

                    BreakMin = 0;
                    EmpBreakMin = 0;
                    DayBreakMin = 0;
                    DataTable dtSchedule = objEmpSch.GetSheduleDescriptionByEmpId(dtFilterEmp.Rows[i]["Emp_Id"].ToString(), FromDate.ToString());
                    try
                    {
                        if (dtSchedule.Rows.Count > 0)
                        {
                            // Shift Assign
                            for (int j = 0; j < dtSchedule.Rows.Count; j++)
                            {
                                if (dtSchedule.Rows[j]["Break_Min"].ToString() != "")
                                {
                                    BreakMin += int.Parse(dtSchedule.Rows[j]["Break_Min"].ToString());

                                    if (j == 0)
                                    {
                                        BeginingIn = dtSchedule.Rows[j]["Beginning_In"].ToString();
                                        EndingIn = dtSchedule.Rows[j]["Ending_In"].ToString();
                                        BeginingOut = dtSchedule.Rows[j]["Beginning_Out"].ToString();
                                        EndingOut = dtSchedule.Rows[j]["Ending_Out"].ToString();
                                        BIn1 = Convert.ToDateTime(dtSchedule.Rows[j]["Beginning_In"].ToString());
                                        EIn = Convert.ToDateTime(dtSchedule.Rows[j]["Ending_In"].ToString());
                                        BOut1 = Convert.ToDateTime(dtSchedule.Rows[j]["Beginning_Out"].ToString());
                                        EOut = Convert.ToDateTime(dtSchedule.Rows[j]["Ending_Out"].ToString());
                                    }
                                }
                            }

                            try
                            {
                                if (Convert.ToDateTime(BeginingIn) <= Convert.ToDateTime(EndingOut))
                                {
                                    dt = new DataView(dtLog, "Event_Date='" + FromDate.ToString() + "'", "Event_Time", DataViewRowState.CurrentRows).ToTable();


                                    OnDutyTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, BIn1.Hour, BIn1.Minute, BIn1.Second);
                                    OffDutyTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, EOut.Hour, EOut.Minute, EOut.Second);


                                    dt = new DataView(dt, "Event_Time>='" + OnDutyTime + "'  and Event_Time<='" + OffDutyTime + "'", "Event_Time", DataViewRowState.CurrentRows).ToTable();

                                }
                                else
                                {
                                    dt = new DataView(dtLog, "Event_Date>='" + FromDate.ToString() + "'  and Event_Date<='" + FromDate.AddDays(1).ToString() + "'", "Event_Time", DataViewRowState.CurrentRows).ToTable();

                                    OnDutyTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, BIn1.Hour, BIn1.Minute, BIn1.Second);
                                    OffDutyTime = new DateTime(FromDate.AddDays(1).Year, FromDate.AddDays(1).Month, FromDate.AddDays(1).Day, EOut.Hour, EOut.Minute, EOut.Second);

                                    dt = new DataView(dt, "Event_Time>='" + OnDutyTime + "'  and Event_Time<='" + OffDutyTime + "'", "Event_Time", DataViewRowState.CurrentRows).ToTable();
                                }


                                if (EmpCalMethod == "PairWise")
                                {
                                    //Pairwise case

                                    if (dt.Rows.Count == 4)
                                    {
                                        DayBreakMin = BreakMin;

                                        for (int m = 0; m < dt.Rows.Count; m++)
                                        {
                                            if (m % 2 != 0)
                                            {
                                                try
                                                {
                                                    BIn = dt.Rows[m]["Event_Time"].ToString();
                                                    Bout = dt.Rows[m + 1]["Event_Time"].ToString();
                                                }
                                                catch
                                                {

                                                }
                                            }

                                            if (BIn != "" && Bout != "")
                                            {
                                                EmpBreakMin = 0;
                                                DateTime Bin1 = Convert.ToDateTime(BIn);
                                                DateTime Bout1 = Convert.ToDateTime(Bout);

                                                EmpBreakMin = GetTimeDifference(Bin1, Bout1);

                                                DataRow dr = DtBreak.NewRow();
                                                dr["EmpCode"] = dt.Rows[m]["Emp_Code"].ToString();
                                                dr["EmpName"] = dt.Rows[m]["Emp_Name"].ToString();
                                                dr["BreakInTime"] = Bin1.ToString("HH:mm");
                                                dr["BreakOutTime"] = Bout1.ToString("HH:mm");
                                                dr["Date"] = FromDate.ToString();
                                                dr["AssignBreakMin"] = BreakMin.ToString();
                                                dr["Duration"] = EmpBreakMin.ToString();
                                                dr["ViolationMin"] = "0";
                                                dr["WorkMethod"] = EmpCalMethod;
                                                dr["DayBreakMin"] = DayBreakMin.ToString();
                                                DayBreakMin = 0;
                                                int violationmin = 0;
                                                violationmin = EmpBreakMin - BreakMin;
                                                BreakMin = BreakMin - EmpBreakMin;
                                                if (BreakMin < 0)
                                                {
                                                    BreakMin = 0;
                                                }
                                                if (violationmin < 0)
                                                {
                                                    violationmin = 0;
                                                }
                                                dr["ViolationMin"] = violationmin.ToString();


                                                DtBreak.Rows.Add(dr);



                                                BIn = "";
                                                Bout = "";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Inout Case
                                    dt = new DataView(dt, "Func_Code in('" + BreakInKey + "','" + BreakOutKey + "','Break In  ','Break Out  ') ", "", DataViewRowState.CurrentRows).ToTable();

                                    if (dt.Rows.Count > 0)
                                    {
                                        DayBreakMin = BreakMin;
                                    }

                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {


                                        if (dt.Rows[k]["Func_Code"].ToString() == BreakInKey)
                                        {
                                            BIn = dt.Rows[k]["Event_Time"].ToString();

                                        }
                                        else if (dt.Rows[k]["Func_Code"].ToString() == BreakOutKey)
                                        {
                                            Bout = dt.Rows[k]["Event_Time"].ToString();

                                            if (BIn == "")
                                            {
                                                Bout = "";

                                            }


                                        }

                                        if (BIn != "" && Bout != "")
                                        {
                                            EmpBreakMin = 0;
                                            DateTime Bin1 = Convert.ToDateTime(BIn);
                                            DateTime Bout1 = Convert.ToDateTime(Bout);

                                            EmpBreakMin = GetTimeDifference(Bin1, Bout1);
                                            DataRow dr = DtBreak.NewRow();
                                            dr["EmpCode"] = dt.Rows[k]["Emp_Code"].ToString();
                                            dr["EmpName"] = dt.Rows[k]["Emp_Name"].ToString();
                                            dr["BreakInTime"] = Bin1.ToString("HH:mm");
                                            dr["BreakOutTime"] = Bout1.ToString("HH:mm");
                                            dr["Date"] = FromDate.ToString();
                                            dr["AssignBreakMin"] = BreakMin.ToString();
                                            dr["Duration"] = EmpBreakMin.ToString();
                                            dr["ViolationMin"] = "0";
                                            dr["WorkMethod"] = EmpCalMethod;
                                            dr["DayBreakMin"] = DayBreakMin.ToString();
                                            DayBreakMin = 0;
                                            int violationmin = 0;
                                            violationmin = EmpBreakMin - BreakMin;
                                            BreakMin = BreakMin - EmpBreakMin;
                                            if (BreakMin < 0)
                                            {
                                                BreakMin = 0;

                                            }
                                            if (violationmin < 0)
                                            {
                                                violationmin = 0;

                                            }
                                            dr["ViolationMin"] = violationmin.ToString();


                                            DtBreak.Rows.Add(dr);



                                            BIn = "";
                                            Bout = "";
                                        }
                                    }


                                }
                            }
                            catch (Exception Ex)
                            {
                                FromDate = FromDate.AddDays(1);
                                continue;
                            }
                            FromDate = FromDate.AddDays(1);
                        }
                        else
                        {
                            // No Shift Assign here we use function key for BreakIn/Out

                            BreakMin = 0;
                            try
                            {
                                dt = new DataView(dt, "Func_Code in('" + BreakInKey + "','" + BreakOutKey + "') ", "", DataViewRowState.CurrentRows).ToTable();


                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (dt.Rows[k]["Func_Code"].ToString() == BreakInKey)
                                    {
                                        BIn = dt.Rows[k]["Event_Time"].ToString();

                                    }
                                    else if (dt.Rows[k]["Func_Code"].ToString() == BreakOutKey)
                                    {
                                        Bout = dt.Rows[k]["Event_Time"].ToString();

                                        if (BIn == "")
                                        {
                                            Bout = "";

                                        }
                                    }

                                    if (BIn != "" && Bout != "")
                                    {
                                        DateTime Bin1 = Convert.ToDateTime(BIn);
                                        DateTime Bout1 = Convert.ToDateTime(Bout);

                                        EmpBreakMin = GetTimeDifference(Bin1, Bout1);
                                        DataRow dr = DtBreak.NewRow();
                                        dr["EmpCode"] = dt.Rows[k]["Emp_Code"].ToString();
                                        dr["EmpName"] = dt.Rows[k]["Emp_Name"].ToString();
                                        dr["BreakInTime"] = Bin1.ToString("HH:mm");
                                        dr["BreakOutTime"] = Bout1.ToString("HH:mm");
                                        dr["Date"] = FromDate.ToString();
                                        dr["AssignBreakMin"] = BreakMin.ToString();
                                        dr["Duration"] = EmpBreakMin.ToString();
                                        dr["ViolationMin"] = "0";
                                        dr["WorkMethod"] = EmpCalMethod;

                                        int violationmin = 0;
                                        violationmin = EmpBreakMin - BreakMin;
                                        if (BreakMin < 0)
                                        {
                                            BreakMin = 0;

                                        }
                                        if (violationmin < 0)
                                        {
                                            violationmin = 0;

                                        }
                                        dr["ViolationMin"] = violationmin.ToString();


                                        DtBreak.Rows.Add(dr);



                                        BIn = "";
                                        Bout = "";

                                    }
                                }
                            }

                            catch
                            {
                            }

                            FromDate = FromDate.AddDays(1);
                        }


                    }

                    catch (Exception Ex)
                    {
                        FromDate = FromDate.AddDays(1);
                        return;
                    }

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
            FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
            ToDate = objSys.getDateForInput(Session["ToDate"].ToString());

            //---Company Logo
            XRPictureBox Company_Logo = (XRPictureBox)RptShift.FindControl("Company_Logo", true);
            try
            {
                Company_Logo.ImageUrl = Imageurl;
            }
            catch
            {
            }
            //------------------

            //Comany Name
            XRLabel Lbl_Company_Name = (XRLabel)RptShift.FindControl("Lbl_Company_Name", true);
            Lbl_Company_Name.Text = CompanyName;
            //------------------

            //Comapny Address
            XRLabel Lbl_Company_Address = (XRLabel)RptShift.FindControl("Lbl_Company_Address", true);
            Lbl_Company_Address.Text = CompanyAddress;
            //------------------


            //Brand Name
            XRLabel Lbl_Brand = (XRLabel)RptShift.FindControl("Lbl_Brand", true);
            Lbl_Brand.Text = Resources.Attendance.Brand + " : ";
            XRLabel Lbl_Brand_Name = (XRLabel)RptShift.FindControl("Lbl_Brand_Name", true);
            Lbl_Brand_Name.Text = BrandName;
            //------------------

            // Location Name
            XRLabel Lbl_Location = (XRLabel)RptShift.FindControl("Lbl_Location", true);
            Lbl_Location.Text = Resources.Attendance.Location + " : ";
            XRLabel Lbl_Location_Name = (XRLabel)RptShift.FindControl("Lbl_Location_Name", true);
            Lbl_Location_Name.Text = LocationName;
            //------------------

            // Department Name
            XRLabel Lbl_Department = (XRLabel)RptShift.FindControl("Lbl_Department", true);
            Lbl_Department.Text = Resources.Attendance.Department + " : ";
            XRLabel Lbl_Department_Name = (XRLabel)RptShift.FindControl("Lbl_Department_Name", true);
            Lbl_Department_Name.Text = DepartmentName;
            //------------------

            // Report Title
            XRLabel Report_Title = (XRLabel)RptShift.FindControl("Report_Title", true);
            Report_Title.Text = "Break In/Out Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
            //------------------



            // Detail Header Table
            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            xrTableCell7.Text = "Date";

            XRLabel xrTableCell11 = (XRLabel)RptShift.FindControl("xrTableCell11", true);
            xrTableCell11.Text = "Calculation Method";

            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = "Break In Time";

            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = "Break Out Time";

            XRLabel xrTableCell19 = (XRLabel)RptShift.FindControl("xrTableCell19", true);
            xrTableCell19.Text = "Assign Break Minute";

            XRLabel xrTableCell26 = (XRLabel)RptShift.FindControl("xrTableCell26", true);
            xrTableCell26.Text = "Remain Minute";

            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = "Break Hour";

            XRLabel xrTableCell22 = (XRLabel)RptShift.FindControl("xrTableCell22", true);
            xrTableCell22.Text = "Violation Hour";

            XRLabel xrTableCell27 = (XRLabel)RptShift.FindControl("xrTableCell27", true);
            xrTableCell27.Text = "Total Assign Break Hour";

            XRLabel xrTableCell12 = (XRLabel)RptShift.FindControl("xrTableCell12", true);
            xrTableCell12.Text = "00:00";

            XRLabel xrTableCell20 = (XRLabel)RptShift.FindControl("xrTableCell20", true);
            xrTableCell20.Text = "Total Break Hour";

            XRLabel xrTableCell21 = (XRLabel)RptShift.FindControl("xrTableCell21", true);
            xrTableCell21.Text = "00:00";

            XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            xrTableCell4.Text = "Total Break Hour Violaton";

            XRLabel xrTableCell25 = (XRLabel)RptShift.FindControl("xrTableCell25", true);
            xrTableCell25.Text = "00:00";
            //-------------------------------------------

            // Detail Id And Name
            XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            xrTableCell13.Text = Resources.Attendance.Id;

            XRLabel xrTableCell17 = (XRLabel)RptShift.FindControl("xrTableCell17", true);
            xrTableCell17.Text = Resources.Attendance.Name;
            //--------------------------------------------

            //Footer
            // Create by
            XRLabel Lbl_Created_By = (XRLabel)RptShift.FindControl("Lbl_Created_By", true);
            Lbl_Created_By.Text = Resources.Attendance.Created_By;
            XRLabel Lbl_Created_By_Name = (XRLabel)RptShift.FindControl("Lbl_Created_By_Name", true);
            Lbl_Created_By_Name.Text = Session["UserId"].ToString();
            //--------------------

            //RptShift.SetImage(Imageurl);
            //RptShift.SetBrandName(BrandName);
            //RptShift.SetLocationName(LocationName);
            //RptShift.SetDepartmentName(DepartmentName);
            //RptShift.setTitleName("Break In/Out Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            //RptShift.setcompanyname(CompanyName);
            //RptShift.setaddress(CompanyAddress);
            //RptShift.setUserName(Session["UserId"].ToString());
            //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Calculation_Method, Resources.Attendance.Break_In_Time, Resources.Attendance.Break_Out_Time, Resources.Attendance.Assign_Break_Minute, Resources.Attendance.Remain_Minute, "Break Hour", "Violation Hour", "Total Assign Break Hour", "Total Break Hour", "Total Break Hour Violaton");
            DtBreak = new DataView(DtBreak, "ViolationMin<>0", "", DataViewRowState.CurrentRows).ToTable();
            RptShift.DataSource = DtBreak;
            RptShift.DataMember = "BreakInBreakOut";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;





        }
    }


    private int GetTimeDifference(DateTime inTime, DateTime outTime)
    {
        // On Duty time  = in Time
        // OutTime ==  Actual In Time
        int timeDifference = 0;

        if (outTime >= inTime)
        {
            timeDifference = outTime.Subtract(inTime).Hours * 60 + outTime.Subtract(inTime).Minutes;



        }
        else
        {
            DateTime TempDateIn = new DateTime(inTime.Year, inTime.Month, inTime.Day, 23, 59, 0);
            DateTime TempDateOut = new DateTime(outTime.Year, outTime.Month, outTime.Day, 0, 0, 0);
            timeDifference = TempDateIn.Subtract(inTime).Hours * 60 + TempDateIn.Subtract(inTime).Minutes;
            timeDifference += outTime.Subtract(TempDateOut).Hours * 60 + outTime.Subtract(TempDateOut).Minutes + 1;
        }
        return timeDifference;
    }

}
