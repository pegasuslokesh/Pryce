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

public partial class Attendance_Report_LateInReportNew : BasePage
{

    Att_LateInReport RptShift = new Att_LateInReport();
    SystemParameter objSys = null;
    Attendance objAttendance = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    Set_ApplicationParameter objApp = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    BrandMaster objBrand = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objApp = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "154", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("154",(DataTable)Session["ModuleName"]);
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

            //DataTable dtFilter = new DataTable();

            AttendanceDataSet rptdata = new AttendanceDataSet();

            rptdata.EnforceConstraints = false;
            AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_LateReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_LateReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(rptdata.sp_Att_AttendanceRegister_LateReport, int.Parse(Session["CompId"].ToString()), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist);

            //if (Emplist != "")
            //{
            //    dtFilter = new DataView(rptdata.sp_Att_AttendanceRegister_LateReport, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") and LateCount<>'0'", "", DataViewRowState.CurrentRows).ToTable();
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
            int LateCount = 0;

            try
            {
                LateCount = int.Parse(objApp.GetApplicationParameterValueByParamName("Late_Occurence", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            catch
            {

            }

            //for (int k = 0; k < dtFilter.Rows.Count; k++)
            //{
            //    dtFilter.Rows[k]["LateParam"] = LateCount.ToString();
            //}
            if (Settings.IsDemo)
            {
                RptShift.Watermark.Text = "Demo Version";
                RptShift.Watermark.ForeColor = System.Drawing.Color.Red;
            }
            RptShift.SetImage(Imageurl);
            RptShift.SetBrandName(BrandName);
            RptShift.SetLocationName(LocationName);
            RptShift.SetDepartmentName(DepartmentName);
            RptShift.setTitleName("Late In Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat()));
            RptShift.setcompanyname(CompanyName);
            RptShift.setaddress(CompanyAddress);
            RptShift.setUserName(Session["UserId"].ToString());
            RptShift.setheaderName(Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Department, Resources.Attendance.Designation, Resources.Attendance.Phone_No_, Resources.Attendance.Email_Id, Resources.Attendance.Late_Count);
            RptShift.DataSource = rptdata.sp_Att_AttendanceRegister_LateReport;
            RptShift.DataMember = "sp_Att_AttendanceRegister_LateReport";
            rptViewer.Report = RptShift;
            rptToolBar.ReportViewer = rptViewer;

        }


    }
}
