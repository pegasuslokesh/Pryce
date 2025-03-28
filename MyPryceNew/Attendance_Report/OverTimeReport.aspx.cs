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

public partial class Attendance_Report_OverTimeReportNew : BasePage
{
    XtraReport RptShift = new XtraReport();
    //Att_OverTimeReport RptShift = new Att_OverTimeReport();
    Attendance objAttendance = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    SystemParameter objSys = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    DepartmentMaster objDept = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_OverTimeReport.repx");
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "100", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("100", (DataTable)Session["ModuleName"]);
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


            //code update on 27-09-2014

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "17");


            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }

            //code end

            //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
            //{
            //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

            //    if (dtEmpNF.Rows.Count > 0)
            //    {
            //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Rpt_OverTime"].ToString()))
            //        {
            //            Emplist += EmpReport.Split(',')[i].ToString() + ",";
            //        }
            //    }
            //}

            //code end


            DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist, 5);

            }
            catch
            {
            }
            dtFilter = rptdata.sp_Att_AttendanceRegister_Report;

            //if (Emplist != "")
            //{
            //    dtFilter = new DataView(rptdata.sp_Att_AttendanceRegister_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dtFilter.Rows.Count > 0)
            //    {
            //        dtFilter = new DataView(dtFilter, "OverTime_Min<>'0' or Week_Off_Min<>'0' or Holiday_Min<>'0'", "", DataViewRowState.CurrentRows).ToTable();

            //    }
            //}
            
            for (int k = 0; k < dtFilter.Rows.Count; k++)
            {
               
                if (dtFilter.Rows[k]["Week_Off_Min"].ToString() != "0")
                {
                    dtFilter.Rows[k]["Field2"] = dtFilter.Rows[k]["Week_Off_Min"];
                    dtFilter.Rows[k]["Field3"] = "Week Off";
                }
                else if (dtFilter.Rows[k]["Holiday_Min"].ToString() != "0")
                {
                    dtFilter.Rows[k]["Field2"] = dtFilter.Rows[k]["Holiday_Min"];
                    dtFilter.Rows[k]["Field3"] = "Holiday";
                }
                else if (dtFilter.Rows[k]["OverTime_Min"].ToString() != "0")
                {
                    dtFilter.Rows[k]["Field2"] = dtFilter.Rows[k]["OverTime_Min"];
                    dtFilter.Rows[k]["Field3"] = "Normal";
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
            Report_Title.Text = "Over Time Report" + " From " + FromDate.ToString("dd-MMM-yyyy") + " To " + ToDate.ToString("dd-MMM-yyyy");
            //------------------



            // Detail Header Table
            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            xrTableCell7.Text = "Date";

            XRLabel xrTableCell24 = (XRLabel)RptShift.FindControl("xrTableCell24", true);
            xrTableCell24.Text = "Shift Name";

            XRLabel xrTableCell22 = (XRLabel)RptShift.FindControl("xrTableCell22", true);
            xrTableCell22.Text = "Time Table Name";

            XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            xrTableCell4.Text = "In Time";

            XRLabel xrTableCell6 = (XRLabel)RptShift.FindControl("xrTableCell6", true);
            xrTableCell6.Text = "Out Time";

            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = "Over Time Hour";

            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = "Over Time Type";



            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = "Normal OT Hour";

            XRLabel xrTableCell27 = (XRLabel)RptShift.FindControl("xrTableCell27", true);
            xrTableCell27.Text = "Week Off OT Hour";

            XRLabel xrTableCell20 = (XRLabel)RptShift.FindControl("xrTableCell20", true);
            xrTableCell20.Text = "Holiday OT Hour";

            XRLabel xrTableCell12 = (XRLabel)RptShift.FindControl("xrTableCell12", true);
            xrTableCell12.Text = "Total Over Time Hour";

      

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
            //RptShift.setTitleName("Over Time Report" + " From " + FromDate.ToString("dd-MMM-yyyy") + " To " + ToDate.ToString("dd-MMM-yyyy"));
            //RptShift.setcompanyname(CompanyName);
            //RptShift.setaddress(CompanyAddress);
            //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Over_Time_Hour, Resources.Attendance.Over_Time_Type, Resources.Attendance.Normal_OT_Hour, Resources.Attendance.Week_Off_OT_Hour, Resources.Attendance.Holiday_OT_Hour);
            //RptShift.setUserName(Session["UserId"].ToString());
            RptShift.DataSource = dtFilter;
            RptShift.DataMember = "sp_Att_AttendanceRegister_Report";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;
        }


    }
}
