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

public partial class Attendance_Report_UserTransferReportNew : BasePage
{
    XtraReport RptShift = new XtraReport();
    //UserTransferReport RptShift = new UserTransferReport();
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    SystemParameter objSys = null;
    BrandMaster objBrand = null;
    Attendance objAttendance = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "UserTransferReport.repx");
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "130", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("130", (DataTable)Session["ModuleName"]);
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

            Emplist = Session["EmpList"].ToString();

            DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Ser_UserTransfer_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Ser_UserTransfer_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(rptdata.sp_Ser_UserTransfer_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));



            if (Emplist != "")
            {
                dtFilter = new DataView(rptdata.sp_Ser_UserTransfer_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
            }


            if (ddlReport.SelectedValue == "True")
            {
                dtFilter = new DataView(dtFilter, "Status='True'", "", DataViewRowState.CurrentRows).ToTable();

            }
            else if (ddlReport.SelectedValue == "False")
            {
                dtFilter = new DataView(dtFilter, "Status='False'", "", DataViewRowState.CurrentRows).ToTable();

            }

            else if (ddlReport.SelectedValue == "All")
            {

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
            Report_Title.Text = "User Transaction Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
            //------------------



            // Detail Header Table
            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = "Id";

            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = "Name";

            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = "Device Name";

            XRLabel xrTableCell6 = (XRLabel)RptShift.FindControl("xrTableCell6", true);
            xrTableCell6.Text = "IP Address";

            XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            xrTableCell4.Text = "Location";

            XRLabel xrTableCell9 = (XRLabel)RptShift.FindControl("xrTableCell9", true);
            xrTableCell9.Text = "Status";

            XRLabel xrTableCell10 = (XRLabel)RptShift.FindControl("xrTableCell10", true);
            xrTableCell10.Text = "Transfer Date";

            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            xrTableCell7.Text = "Upload Data";
            //-------------------------------------------

            // Detail Id And Name
            //XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            //xrTableCell13.Text = Resources.Attendance.Id;

            //XRLabel xrTableCell17 = (XRLabel)RptShift.FindControl("xrTableCell17", true);
            //xrTableCell17.Text = Resources.Attendance.Name;
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
            //RptShift.setTitleName("User Transaction Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            //RptShift.setcompanyname(CompanyName);
            //RptShift.setaddress(CompanyAddress);
            //RptShift.setUserName(Session["UserId"].ToString());
            //RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Device_Name, Resources.Attendance.IP_Address, Resources.Attendance.Location, Resources.Attendance.Status, Resources.Attendance.Transfer_Date, Resources.Attendance.Upload_Data);
            RptShift.DataSource = dtFilter;
            RptShift.DataMember = "sp_Ser_UserTransfer_Report";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;



        }


    }

    protected void ddlReportTrOrFal_SelectedIndexChanged(object sender, EventArgs e)
    {

        GetReport();


    }
}
