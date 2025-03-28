using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class HR_Report_EmployeePayrollPosted : BasePage
{
    EmployeePostedPayroll objReport = new EmployeePostedPayroll();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    EmployeeMaster ObjEmployee = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        GetReport();
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
        if (Session["PostedEmployee"] != null)
        {
            DataTable DtPostedReport = (DataTable)Session["PostedEmployee"];
            string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string LocationName = string.Empty;
        string BrandName = string.Empty;


        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
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
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["PostedLocationId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            }
            else
            {
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
            }
        }

        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            }
            else
            {
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
            }
        }
      

        objReport.SetImage(Imageurl);
        objReport.SetCompanyName(CompanyName);
        objReport.SetAddress(CompanyAddress);
        objReport.SetBrandName(BrandName);
        objReport.SetLocationName(LocationName);
        objReport.SetHeaderName();
        //Objreport.setSumClaimAmount(SumClaim);
        objReport.setUserName(Session["UserId"].ToString());
        objReport.setTitleName(Resources.Attendance.Payroll_Posted_Report);
        objReport.DataSource = DtPostedReport;
        objReport.DataMember = "EmployeePostedPayroll";
        ReportViewer1.Report = objReport;
        ReportToolbar1.ReportViewer = ReportViewer1;
            

        }
 
    }
}
