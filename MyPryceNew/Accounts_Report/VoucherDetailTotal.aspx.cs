using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class Accounts_Report_VoucherDetailTotal : System.Web.UI.Page
{
    VoucherDetailsReport objReport = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        objReport = new VoucherDetailsReport(Session["DBConnection"].ToString());

        GetReport();
    }

    public void GetReport()
    {
        AccountsDataset objAccountDataset = new AccountsDataset();
        objAccountDataset.EnforceConstraints = false;

        ArrayList objarr = (ArrayList)Session["ArrVoucherDetail"];
        AccountsDatasetTableAdapters.sp_Ac_VoucherDetailTotalTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_VoucherDetailTotalTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(objAccountDataset.sp_Ac_VoucherDetailTotal, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(objarr[3].ToString()));

        DataTable dt = objAccountDataset.sp_Ac_VoucherDetailTotal;





        dt = new DataView(dt, "Location_Id in (" + objarr[2].ToString() + ") ", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();



        string strdateCritera = string.Empty;
        if (objarr[0].ToString() != "" && objarr[1].ToString() != "")
        {
            dt = new DataView(dt, "Voucher_Date>='" + objarr[0].ToString() + "' and  Voucher_Date<='" + objarr[1].ToString() + "'", "Voucher_Date", DataViewRowState.CurrentRows).ToTable();

            strdateCritera = "From : " + objarr[0].ToString() + " To : " + objarr[1].ToString();
        }

        string[] strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());

        string CompanyName = strParam[0].ToString();

        string Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();



        objReport.DataSource = dt;
        objReport.DataMember = "sp_Ac_VoucherDetailTotal";
        ReportViewer.Report = objReport;
        ReportToolbar.ReportViewer = ReportViewer;
        objReport.setdateCriteria(strdateCritera);
        objReport.setcompanyname(CompanyName);
        objReport.SetImage(Imageurl);

    }
}