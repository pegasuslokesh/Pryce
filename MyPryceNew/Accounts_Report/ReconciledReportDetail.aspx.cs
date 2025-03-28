using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_ReconciledReportDetail : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    ReconciledReportDetail objTrialBalanceprint = null;
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
        objTrialBalanceprint = new ReconciledReportDetail(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());

        if (Session["dtAcRParam"] != null)
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


        ArrayList objArr = (ArrayList)Session["dtAcRParam"];
        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;

        AccountsDatasetTableAdapters.sp_Ac_Reconcile_Detail_ReportTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_Reconcile_Detail_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_Reconcile_Detail_Report, Convert.ToInt32(strCompId), Convert.ToInt32(strBrandId), Convert.ToInt32(strLocationId), DateTime.Parse(objArr[4].ToString().Trim()), DateTime.Parse(objArr[5].ToString().Trim()), 1);



        DataTable dt = ObjAccountDataset.sp_Ac_Reconcile_Detail_Report;

        //for filter by date
        if (objArr[1].ToString().Trim() != "")
        {
            DateTime dtToDate = Convert.ToDateTime(objArr[5].ToString());
            dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            Convert.ToDateTime(objArr[4].ToString()).ToString(objsys.SetDateFormat());
            Convert.ToDateTime(objArr[5].ToString()).ToString(objsys.SetDateFormat());
            strFilter = "From " + Convert.ToDateTime(objArr[4].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[5].ToString()).ToString(objsys.SetDateFormat());
        }

        string strReconciledNo = objArr[0].ToString().Trim();
        string strReconciledBy = objArr[1].ToString().Trim();
        string strAccountId = objArr[2].ToString().Trim();
        string strOtherAccountId = objArr[3].ToString().Trim();

        //For Filteration for all
        if (dt.Rows.Count > 0)
        {
            if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0" && strOtherAccountId != "0")
            {
                dt = new DataView(dt, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strReconciledNo != "0" && strReconciledBy != "0" && strAccountId != "0")
            {
                dt = new DataView(dt, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "' and Account_No='" + strAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strReconciledNo != "0" && strReconciledBy != "0")
            {
                dt = new DataView(dt, "ReconcilationNo='" + strReconciledNo + "' and Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strReconciledNo != "0")
            {
                dt = new DataView(dt, "ReconcilationNo='" + strReconciledNo + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strReconciledBy != "0")
            {
                dt = new DataView(dt, "Reconciled_By='" + strReconciledBy + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strAccountId != "0" && strOtherAccountId != "0")
            {
                dt = new DataView(dt, "Account_No='" + strAccountId + "' and Other_Account_No='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (strAccountId != "0")
            {
                dt = new DataView(dt, "Account_No='" + strAccountId + "' ", "", DataViewRowState.CurrentRows).ToTable();
            }
        }


        foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;

        DataTable DtCompany = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Location_Name"].ToString();
            //Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Field2"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_Reconcile_Detail_Report";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.setReportTitle("RECONCILED DETAIL REPORT");
        lblHeader.Text = "RECONCILED DETAIL REPORT";
        objTrialBalanceprint.setDateFilter(strFilter);
        objTrialBalanceprint.setReconciledBy(strReconciledBy);
        objTrialBalanceprint.setAccount(strAccountId);
    }
}