using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Accounts_Report_SupplierPaymentVoucherReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    SupplierPaymentVoucher objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    string strCompId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new SupplierPaymentVoucher(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (Request.QueryString["Id"] != null)
        {
            GetReport();
        }
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        string LocationName = "";
        string Imageurl = "";


        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        AccountsDatasetTableAdapters.sp_Ac_SupplierPaymentVoucher_ReportTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_SupplierPaymentVoucher_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_SupplierPaymentVoucher_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));


        DataTable dt = ObjAccountDataset.sp_Ac_SupplierPaymentVoucher_Report;
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_SupplierPaymentVoucher_Report";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(LocationName);
        objTrialBalanceprint.SetImage(Imageurl);
    }
}