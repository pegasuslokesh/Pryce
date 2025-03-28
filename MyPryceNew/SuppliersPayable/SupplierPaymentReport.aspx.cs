using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using PegasusDataAccess;

public partial class SuppliersPayable_SupplierPaymentReport : System.Web.UI.Page
{
    Set_SupplierReport objReport = null;
    LocationMaster objLoc = null;
    DataAccessClass objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new Set_SupplierReport(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());

        objLoc = new LocationMaster(Session["DBConnection"].ToString());
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
        string Imageurl = "";

        DataTable dt = new DataTable();
        AccountsDataset rptdata = new AccountsDataset();
        rptdata.EnforceConstraints = false;


        DataTable DtLocation = objLoc.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            objReport.SetLocationName(DtLocation.Rows[0]["Location_Name"].ToString());
            if (DtLocation.Rows[0]["Field2"].ToString() != "" && DtLocation.Rows[0]["Field2"].ToString() != null)
            {
                Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();
            }
            else
            {
                Imageurl = "";
            }
        }
        else
        {
            Imageurl = "";
        }
        if ( Request.QueryString["VoucherId"] != null)
        {




            AccountsDatasetTableAdapters.Supplier_Payment_ReportTableAdapter adp = new  AccountsDatasetTableAdapters.Supplier_Payment_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            try
            {
                string voucherId = Request.QueryString["VoucherId"].ToString();
                adp.Fill(rptdata.Supplier_Payment_Report, Convert.ToInt32(Session["UserId"].ToString()), Convert.ToInt32(voucherId));

                //adp.Fill(rptdata.Supplier_Payment_Report, Convert.ToInt32(Request.QueryString["VoucherId"].ToString()), Convert.ToInt32(Request.QueryString["EmpId"].ToString()));
            }
            catch (Exception ex)
            {

            }
            dt = rptdata.Supplier_Payment_Report;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    objReport.SetDate(DateTime.Now.ToString("dd-MMM-yyyy"));
                    objReport.setCompanyLogo(Imageurl);
                }
                catch
                {

                }
                objReport.DataSource = dt;
                objReport.DataMember = "Supplier_Payment_Report";
                rptViewer.Report = objReport;               

            }
            rptToolBar.ReportViewer = rptViewer;

        }

        }
    }