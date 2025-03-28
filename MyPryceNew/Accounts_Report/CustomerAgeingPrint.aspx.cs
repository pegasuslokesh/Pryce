using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using PegasusDataAccess;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_CustomerAgeingPrint : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    CustomerAgeingPrint objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objsys = null;
    DataAccessClass da = null;

    string strCompId = string.Empty;
    string strBrandId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new CustomerAgeingPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());


        if (Session["dtAcParam"] != null)
        {
            GetReport();
        }
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        string CompanyName = "";
        string Imageurl = "";
        string strFilter = string.Empty;

        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;
        ArrayList objArr = (ArrayList)Session["dtAcParam"];

        AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingReportTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingReport, strCompId, strBrandId, objArr[1].ToString(), objArr[6].ToString(), objArr[0].ToString(), "0", "", true);

        DataTable dt = ObjAccountDataset.Sp_Ac_InvoiceAgeingReport;

        if (dt.Rows.Count > 0 && dt != null)
        {


            if (objArr[5].ToString().Trim() != "" && objArr[5].ToString().Trim() != "0")
            {
                dt = new DataView(dt, "Due_Days>='" + objArr[5].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


            if (!Convert.ToBoolean(objArr[7].ToString()))
            {
                dt = new DataView(dt, "actual_balance_amt<>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            try
            {
                dt = new DataView(dt, "Invoice_date=>'" + objArr[2].ToString().Trim() + "' and Invoice_date<='" + objArr[3].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

        }

        //for filter by date
        if (objArr[2].ToString().Trim() != "")
        {
            DateTime dtToDate = Convert.ToDateTime(objArr[3].ToString());
            dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            //dt = new DataView(dt, "Voucher_Date >= '" + objArr[2].ToString() + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
            Convert.ToDateTime(objArr[3].ToString()).ToString(objsys.SetDateFormat());
            strFilter = "From " + Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[3].ToString()).ToString(objsys.SetDateFormat());
        }


        DataTable DtCompany = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Field2"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_CustomerAgeing_SelectRow";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.SetImage(Imageurl);
        objTrialBalanceprint.setDateFilter(strFilter);
    }
}