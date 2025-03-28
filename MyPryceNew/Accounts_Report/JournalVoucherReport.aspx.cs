using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_JournalVoucherReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    JournalVoucherPrint objTrialBalanceprint = null;
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
        objTrialBalanceprint = new JournalVoucherPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (Session["dtAcVoucher"] != null)
        {
            GetReport();
        }
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        string LocationName = "";
        string Imageurl = "";

        ArrayList objArr = (ArrayList)Session["dtAcVoucher"];


        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        AccountsDatasetTableAdapters.sp_Ac_JornalVoucher_ReportTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_JornalVoucher_ReportTableAdapter();

        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjAccountDataset.sp_Ac_JornalVoucher_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(objArr[0].ToString().Trim()));


        DataTable dt = ObjAccountDataset.sp_Ac_JornalVoucher_Report;
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();
        }

        if (objArr[1].ToString().Trim() == "JournalVoucher")
        {
            objTrialBalanceprint.SetOtherAccountNumberHeader("Other Account Name");
            objTrialBalanceprint.setReportTitle("JOURNAL VOUCHER");
            lblHeader.Text = "Journal Voucher";
        }
        else if (objArr[1].ToString().Trim() == "PaymentVoucher")
        {
            objTrialBalanceprint.SetOtherAccountNumberHeader("Other Account Name");
            objTrialBalanceprint.setReportTitle("PAYMENT VOUCHER");
            lblHeader.Text = "Payment Voucher";
        }
        else if (objArr[1].ToString().Trim() == "ReceiveVoucher")
        {
            objTrialBalanceprint.SetOtherAccountNumberHeader("Other Account Name");
            objTrialBalanceprint.setReportTitle("RECEIVE VOUCHER");
            lblHeader.Text = "Receive Voucher";
        }

        objTrialBalanceprint.DataSource = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_JornalVoucher_Report";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(LocationName);
        objTrialBalanceprint.SetImage(Imageurl);
    }
}