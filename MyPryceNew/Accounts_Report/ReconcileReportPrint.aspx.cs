using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_ReconcileReportPrint : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    ReconcileReportPrint objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objsys = null;

    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new ReconcileReportPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());


        if (Session["dtAcRVParam"] != null)
        {
            GetReport();
        }
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        string CompanyName = "";
        string strFilter = string.Empty;

        ArrayList objArr = (ArrayList)Session["dtAcRVParam"];
        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;

        AccountsDatasetTableAdapters.sp_Ac_Reconcile_Detail_VoucherOnlyTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_Reconcile_Detail_VoucherOnlyTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_Reconcile_Detail_VoucherOnly, Convert.ToInt32(objArr[0].ToString()), 1);

        DataTable dt = ObjAccountDataset.sp_Ac_Reconcile_Detail_VoucherOnly;
        foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;

        DataTable DtCompany = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Location_Name"].ToString();
            //Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Field2"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_Reconcile_Detail_VoucherOnly";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.setReportTitle("RECONCILED REPORT BY VOUCHER");
        lblHeader.Text = "RECONCILED REPORT BY VOUCHER";
    }
}