using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using DevExpress.XtraReports.UI;
using System.IO;

public partial class Attendance_Report_Device_Information_Report : BasePage
{
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    Set_ApplicationParameter objApp = null;
    XtraReport RptDeviceInfo = new XtraReport();
    protected void Page_Load(object sender, EventArgs e)
    {
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objApp = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        if (Request.QueryString["Type"] == null)
        {
            RptDeviceInfo.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_DeviceInfo_ByEmployee.repx");
        }
        else
        {
            RptDeviceInfo.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_DeviceInfo.repx");
        }
        GetReport();
    }

    public void GetReport()
    {
        DataTable dtFilter = new DataTable();
        string Emplist = string.Empty;
        if (Session["EmpList"] == null && Request.QueryString["Type"] == null)
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

            //DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;

            AttendanceDataSetTableAdapters.sp_Att_DeviceInformation_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_DeviceInformation_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            if (Request.QueryString["Type"] == null)
            {
                adp.Fill(rptdata.sp_Att_DeviceInformation_Report, Session["EmpList"].ToString(), "", 1);
            }
            else
            {
                adp.Fill(rptdata.sp_Att_DeviceInformation_Report, "", Session["DeviceList"].ToString(), 0);
            }
            dtFilter = rptdata.sp_Att_DeviceInformation_Report;

            string CompanyName = "";
            string CompanyAddress = "";
            string Imageurl = "";


            // Get Company Name
            CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
            // Image Url
            Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");

            DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
            if (DtAddress.Rows.Count > 0)
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }

            XRPictureBox Company_Logo = (XRPictureBox)RptDeviceInfo.FindControl("Company_Logo", true);
            try
            {
                Company_Logo.ImageUrl = Imageurl;
            }
            catch
            {
            }
            //------------------

            //for device info group by employee




            // Report_Title- for set report title
            //xrTableCell13- group header table cell for set employeee code
            //tableCell1- department
            //xrTableCell17- employee name
            //tableCell4- dsignation
            //xrTableCell7- sr. no.
            //xrTableCell1 - device name
            //xrTableCell20 - ip address
            // tableCell14- last downlod time
            //tableCell16 - last restart time
            //xrTableCell2 - Is Face
            //xrTableCell4- is Finger
            // xrTableCell3 - finger count
            //tableCell7 - total user
            //tableCell10 - total face
            //tableCell12- total finger
            if (Request.QueryString["Type"] == null)
            {
                XRTableCell xrEmployeeCode = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell13", true);
                xrEmployeeCode.Text = Resources.Attendance.Employee_Code;
                XRTableCell xrDepartment = (XRTableCell)RptDeviceInfo.FindControl("tableCell1", true);
                xrDepartment.Text = Resources.Attendance.Department;
                XRTableCell xrEmployeeName = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell17", true);
                xrEmployeeName.Text = Resources.Attendance.Employee_Name;
                XRTableCell xrDesignation = (XRTableCell)RptDeviceInfo.FindControl("tableCell4", true);
                xrDesignation.Text = Resources.Attendance.Designation;
               
                XRTableCell xrDeviceName = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell1", true);
                xrDeviceName.Text = Resources.Attendance.Device_Name;
                XRTableCell xrIpAddress = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell20", true);
                xrIpAddress.Text = Resources.Attendance.IP_Address;
                XRTableCell xrLastDownloadTime = (XRTableCell)RptDeviceInfo.FindControl("tableCell14", true);
                xrLastDownloadTime.Text = Resources.Attendance.Last_Download_Time;
                XRTableCell xrLastRestartTime = (XRTableCell)RptDeviceInfo.FindControl("tableCell16", true);
                xrLastRestartTime.Text = Resources.Attendance.Last_Restart_Time;
            }
            else
            {
                XRTableCell xrEmployeeCode = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell1", true);
                xrEmployeeCode.Text = Resources.Attendance.Code;
                XRTableCell xrEmployeeName = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell20", true);
                xrEmployeeName.Text = Resources.Attendance.Name;
                XRTableCell xrDeviceName = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell13", true);
                xrDeviceName.Text = Resources.Attendance.Device_Name;
                XRTableCell xrIpAddress = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell17", true);
                xrIpAddress.Text = Resources.Attendance.IP_Address;
                XRTableCell xrLastDownloadTime = (XRTableCell)RptDeviceInfo.FindControl("tableCell1", true);
                xrLastDownloadTime.Text = Resources.Attendance.Last_Download_Time;
                XRTableCell xrLastRestartTime = (XRTableCell)RptDeviceInfo.FindControl("tableCell4", true);
                xrLastRestartTime.Text = Resources.Attendance.Last_Restart_Time;

            }

            //Comany Name
            XRTableCell xrserialNo = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell7", true);
            xrserialNo.Text = Resources.Attendance.SR;
            XRTableCell xrIsFace = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell2", true);
            xrIsFace.Text = Resources.Attendance.Is_Face;
            XRTableCell xrIsFinger = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell3", true);
            xrIsFinger.Text = Resources.Attendance.Is_Finger;
            XRTableCell xrFingerCount = (XRTableCell)RptDeviceInfo.FindControl("xrTableCell3", true);
            xrFingerCount.Text = Resources.Attendance.Finger_Count;
            XRTableCell xrTotaluser = (XRTableCell)RptDeviceInfo.FindControl("tableCell7", true);
            xrTotaluser.Text = Resources.Attendance.Total_User;
            XRTableCell xrTotalface = (XRTableCell)RptDeviceInfo.FindControl("tableCell10", true);
            xrTotalface.Text = Resources.Attendance.Total_Face;
            XRTableCell xrTotalFinger = (XRTableCell)RptDeviceInfo.FindControl("tableCell12", true);
            xrTotalFinger.Text = Resources.Attendance.Total_Finger;
            XRLabel Lbl_Report_Title = (XRLabel)RptDeviceInfo.FindControl("Report_Title", true);
            Lbl_Report_Title.Text = Resources.Attendance.Device_Information_Report;

            XRLabel Lbl_Company_Name = (XRLabel)RptDeviceInfo.FindControl("Lbl_Company_Name", true);
            Lbl_Company_Name.Text = CompanyName;
            XRLabel Lbl_Company_Address = (XRLabel)RptDeviceInfo.FindControl("Lbl_Company_Address", true);
            Lbl_Company_Address.Text = CompanyAddress;
            XRLabel Lbl_Created_By = (XRLabel)RptDeviceInfo.FindControl("Lbl_Created_By", true);
            Lbl_Created_By.Text = Resources.Attendance.Created_By;
            XRLabel Lbl_Created_By_Name = (XRLabel)RptDeviceInfo.FindControl("Lbl_Created_By_Name", true);
            Lbl_Created_By_Name.Text = Session["UserId"].ToString();
            //RptShift.setheaderName(Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Department, Resources.Attendance.Designation, Resources.Attendance.Phone_No_, Resources.Attendance.Email_Id, Resources.Attendance.Late_Count);
            RptDeviceInfo.DataSource = dtFilter;
            RptDeviceInfo.DataMember = "sp_Att_DeviceInformation_Report";
            rptViewer.Report = RptDeviceInfo;
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