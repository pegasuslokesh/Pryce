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

public partial class Attendance_Report_LeaveReportNew : BasePage
{

    //Att_LeaveReport RptShift = new Att_LeaveReport();
    XtraReport RptShift = new XtraReport();
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_LeaveReport.repx");

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "105", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("105", (DataTable)Session["ModuleName"]);
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
    public void GetReport()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string EmpList = string.Empty;
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
            FromDate = Convert.ToDateTime(Session["FromDate"].ToString());
            ToDate = Convert.ToDateTime(Session["ToDate"].ToString());
            EmpList = Session["EmpList"].ToString();
            //DataTable dtFilter = new DataTable();
            AttendanceDataSet rptdata = new AttendanceDataSet();
            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Set_Att_Employee_Leave_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Set_Att_Employee_Leave_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(rptdata.sp_Set_Att_Employee_Leave_Report, int.Parse(Session["CompId"].ToString()), EmpList);
            //if (EmpList != "")
            //{
            //    dtFilter = new DataView(rptdata.sp_Set_Att_Employee_Leave_Report, "Emp_Id in (" + EmpList.Substring(0, EmpList.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
            //}
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
            Report_Title.Text = "Employee Leave Report";
            //------------------



            // Detail Header Table
            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            xrTableCell7.Text = "Leave Name";
            
            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = "Total Leave";

            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = "Paid Leave";

            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = "Schedule Type";

            XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            xrTableCell4.Text = "Is Year Carry";
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
            //RptShift.setTitleName("Employee Leave Report");
            //RptShift.setcompanyname(CompanyName);
            //RptShift.setaddress(CompanyAddress);
            //RptShift.setUserName(Session["UserId"].ToString());
            //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Leave_Name, Resources.Attendance.Total_Leave, Resources.Attendance.Paid_Leave, Resources.Attendance.Schedule_Type, Resources.Attendance.Is_Year_Carry);
            RptShift.DataSource = rptdata.sp_Set_Att_Employee_Leave_Report;
            RptShift.DataMember = "sp_Set_Att_Employee_Leave_Report";
            rptViewer.Report = RptShift;
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
