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

public partial class HR_Report_DirectoryReport : BasePage
{
    DirectoryWiseReport objReport = null;
    DirectoryWiseReportByDocument objReportByDocument = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    SystemParameter objSys = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new DirectoryWiseReport(Session["DBConnection"].ToString());
        objReportByDocument = new DirectoryWiseReportByDocument(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());


        GetReport();
        Page.Title = objSys.GetSysTitle();
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
    void GetReport()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EmpId");
        dt.Columns.Add("EmpName");
        dt.Columns.Add("Directory_Name");
        dt.Columns.Add("Directory_Created_Date");
        dt.Columns.Add("Document_Name");

        dt.Columns.Add("File_Name");
        dt.Columns.Add("File_Upload_Date");
        dt.Columns.Add("File_Expiry_Date");


        string Id = (string)Session["DirectoryLIst"];
        //int ClaimType = (int)Session["ClaimType"];
        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                DataTable dtClaimRecod = (DataTable)Session["ClaimRecord"];
                dtClaimRecod = new DataView(dtClaimRecod, "Directory_Id='" + str.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtClaimRecod.Rows.Count > 0)
                {

                    for (int i = 0; i < dtClaimRecod.Rows.Count; i++)
                    {
                        //string EmployeeName = GetEmployeeName(str);

                        //int salary = 10000;
                        //double Claimamount = 0;
                        //if (dtClaimRecod.Rows[i]["Value_Type"].ToString() == "2")
                        //{
                        //    int value = Convert.ToInt32(dtClaimRecod.Rows[i]["Value"]);
                        //    Claimamount = salary * value / 100;
                        //}
                        //else
                        //{
                        //    Claimamount = Convert.ToDouble(dtClaimRecod.Rows[i]["Value"].ToString());
                        //}

                        //int Month = Convert.ToInt32(dtClaimRecod.Rows[i]["Penalty_Month"]);

                        DataRow dr = dt.NewRow();
                        dr[0] = dtClaimRecod.Rows[i]["Document_Master_Id"].ToString();
                        dr[1] = "";
                        dr[2] = dtClaimRecod.Rows[i]["Directory_Name"].ToString();
                        dr[3] = dtClaimRecod.Rows[i]["Directory_For"].ToString();
                        dr[4] = dtClaimRecod.Rows[i]["Document_Name"].ToString();
                        dr[5] = dtClaimRecod.Rows[i]["File_Name"].ToString();
                        dr[6] = dtClaimRecod.Rows[i]["File_Upload_Date"].ToString();
                        dr[7] = dtClaimRecod.Rows[i]["File_Expiry_Date"].ToString();

                        dt.Rows.Add(dr);
                    }
                }



            }
        }
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";

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
        if (Session["Document_Group"].ToString().Trim() == "YES")
        {
            objReportByDocument.SetImage(Imageurl);
            objReportByDocument.setTitleName(Resources.Attendance.Directory_Report);
            objReportByDocument.setcompanyname(CompanyName);
            objReportByDocument.setaddress(CompanyAddress);
            objReportByDocument.setUserName(Session["UserId"].ToString());
            objReportByDocument.setArabic();
            objReportByDocument.DataSource = dt;
            objReportByDocument.DataMember = "EmpDirectory";
            ReportViewer1.Report = objReportByDocument;

        }
        else
        {
            objReport.SetImage(Imageurl);
            objReport.setTitleName(Resources.Attendance.Directory_Report);
            objReport.setcompanyname(CompanyName);
            objReport.setaddress(CompanyAddress);
            objReport.setUserName(Session["UserId"].ToString());
            objReport.setArabic();
            objReport.DataSource = dt;
            objReport.DataMember = "EmpDirectory";
            ReportViewer1.Report = objReport;

        }


        ReportToolbar1.ReportViewer = ReportViewer1;





    }
}
