using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class HR_Report_FinalSalaryStatement : System.Web.UI.Page
{
    SummarySalaryStatementReport objReport = null;
    BrandMaster ObjBrandMaster = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster =null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objReport = new SummarySalaryStatementReport(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());

        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "260", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        if (Session["Querystring"] == null)
        {
            //Response.Redirect("~/HR_Report/GeneratePayrollReport.aspx");
            string TARGET_URL = "../HR_Report/GeneratePayrollReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            GetReport();

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
    public void GetReport()
    {
        EmployeePaySlipDataSet objDataset = new EmployeePaySlipDataSet();

        EmployeePaySlipDataSetTableAdapters.sp_Pay_salarySummaryStatement_ReportTableAdapter adp=new EmployeePaySlipDataSetTableAdapters.sp_Pay_salarySummaryStatement_ReportTableAdapter();

        adp.Fill(objDataset.sp_Pay_salarySummaryStatement_Report, Convert.ToInt32(Session["Compid"].ToString()), Convert.ToInt32(Session["Month"].ToString()), Convert.ToInt32(Session["Year"].ToString()));

        DataTable dt = objDataset.sp_Pay_salarySummaryStatement_Report;
        string Empid = Session["Querystring"].ToString();
        try
        {
            dt = new DataView(dt, "Emp_Id in (" + Empid.Substring(0, Empid.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        string[] strParam = Common.ReportHeaderSetup("Company",Session["CompId"].ToString(), Session["DBConnection"].ToString());

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;


        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
       
        if (DtCompany.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();
            }
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();

        }

        CompanyAddress = strParam[2].ToString();
        
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            else
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            else
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
        }
        objReport.setBrandName(BrandName);
        objReport.setLocationName(LocationName);
        objReport.setDepartmentName(Session["DepartmentName"].ToString());
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setUserName(Session["UserId"].ToString());
        objReport.setcompanyname(CompanyName);
        objReport.setimage(Imageurl);
        objReport.setUserName(Session["UserId"].ToString());
        objReport.settitle("Summary Salary Statement");
        objReport.DataSource = dt;
        objReport.setMonthandYear(Session["Monthname"].ToString(), Session["year"].ToString());
        objReport.DataMember = "sp_Pay_salarySummaryStatement_Report";
        //objAlowaReport.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Month, Resources.Attendance.Year, Resources.Attendance.Employee_Name, Resources.Attendance.Employee_Code, Resources.Attendance.Allowance_Name, Resources.Attendance.Allowance_Value, Resources.Attendance.Paid_Amount, Resources.Attendance.Total);
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
    }
}