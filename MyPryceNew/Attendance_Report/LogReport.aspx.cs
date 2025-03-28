using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using System.Web;

public partial class Attendance_Report_LogReportNew : BasePage
{
    //Att_LogReport RptShift = new Att_LogReport();
    XtraReport RptShift = new XtraReport();
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    BrandMaster objBrand = null;
    Att_Employee_Notification objEmpNotice = null;
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



        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_LogReport.repx");

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Session["UserId"] != null)
            {
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "114", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();
            FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
            ToDate = objSys.getDateForInput(Session["ToDate"].ToString());
            string Emplist = string.Empty;
            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "15");
            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            //DataTable dtFilter = new DataTable();
            //AttendanceDataSet rptdata = new AttendanceDataSet();
            //rptdata.EnforceConstraints = false;
            //AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter();
            //adp.Fill(rptdata.sp_Att_AttendanceLog_Report, int.Parse(Session["CompId"].ToString()), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Session["EmpList"].ToString(), DDl_Filter.SelectedItem.Text.ToString());
            //if (Emplist != "")
            //{
            //    dtFilter = new DataView(rptdata.sp_Att_AttendanceLog_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") and IsActive='True' ", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //DataTable Dt_Filter_by_Type = dtFilter.DefaultView.ToTable(true, "Verified_Type");
            //DDl_Filter.DataSource = Dt_Filter_by_Type;
            //DDl_Filter.DataTextField = "Verified_Type";
            ////DDl_Filter.DataBind();
            DDl_Filter.Items.Insert(0, "--By All--");
            DDl_Filter.SelectedIndex = 0;
        }
        GetReport();
        //New Code 

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("114", (DataTable)Session["ModuleName"]);
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

    //protected void DocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    //{
    //    e.Key = Guid.NewGuid().ToString();
    //    Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    //}

    //protected void DocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    //{
    //    Stream stream = Page.Session[e.Key] as Stream;
    //    if (stream != null)
    //        e.RestoreDocumentFromStream(stream);
    //}


    public void GetReport()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        if (Session["EmpList"] == null)
        {
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



            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "15");
            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }
            DataTable dtFilter = new DataTable();
            AttendanceDataSet rptdata = new AttendanceDataSet();
            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(rptdata.sp_Att_AttendanceLog_Report, int.Parse(Session["CompId"].ToString()), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Session["EmpList"].ToString(), DDl_Filter.SelectedItem.Text.ToString());
            //if (Emplist != "")
            //{
            //    if (DDl_Filter.SelectedItem.Text.ToString() == "--By All--")
            //        dtFilter = new DataView(rptdata.sp_Att_AttendanceLog_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") and IsActive='True' ", "", DataViewRowState.CurrentRows).ToTable();
            //    else
            //        dtFilter = new DataView(rptdata.sp_Att_AttendanceLog_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") and Verified_Type='" + DDl_Filter.SelectedItem.Text.ToString() + "' and IsActive='True' ", "", DataViewRowState.CurrentRows).ToTable();
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
            Lbl_Brand.Text = "Company Name" + " : ";
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
            Report_Title.Text = "Log Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()); ;
            //------------------



            // Detail Header Table
            XRLabel xrTableCell11 = (XRLabel)RptShift.FindControl("xrTableCell11", true);
            xrTableCell11.Text = "Event Date";

            XRLabel xrTableCell7 = (XRLabel)RptShift.FindControl("xrTableCell7", true);
            xrTableCell7.Text = "Event Time";

            XRLabel xrTableCell1 = (XRLabel)RptShift.FindControl("xrTableCell1", true);
            xrTableCell1.Text = "Function Code";

            XRLabel xrTableCell2 = (XRLabel)RptShift.FindControl("xrTableCell2", true);
            xrTableCell2.Text = "Type";

            XRLabel xrTableCell3 = (XRLabel)RptShift.FindControl("xrTableCell3", true);
            xrTableCell3.Text = "Device";

            //XRLabel xrTableCell4 = (XRLabel)RptShift.FindControl("xrTableCell4", true);
            //xrTableCell4.Text = "";
            //-------------------------------------------

            // Detail Id And Name
            XRLabel xrTableCell13 = (XRLabel)RptShift.FindControl("xrTableCell13", true);
            //xrTableCell13.Text = Resources.Attendance.Id;
            xrTableCell13.Text ="ID";

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


            // RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, , Resources.Attendance.Event_Time, , Resources.Attendance.Type, Resources.Attendance.Created_By);
            RptShift.DataSource = rptdata.sp_Att_AttendanceLog_Report;
            RptShift.DataMember = "sp_Att_AttendanceLog_Report";
            RptShift.CreateDocument(true);
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;
        }
    }

    protected void DDl_Filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetReport();
    }
}
