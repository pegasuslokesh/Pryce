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

public partial class Accounts_Report_SupplierAgeingPrint : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    SupplierAgeingPrint objTrialBalanceprint = null;
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
        objTrialBalanceprint = new SupplierAgeingPrint(Session["DBConnection"].ToString());
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

        AccountsDatasetTableAdapters.sp_Ac_SupplierAgeing_SelectRowTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_SupplierAgeing_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_SupplierAgeing_SelectRow, Convert.ToInt32(strBrandId), DateTime.Parse(objArr[2].ToString().Trim()), DateTime.Parse(objArr[3].ToString().Trim()), objArr[1].ToString().Trim());

        DataTable dt = ObjAccountDataset.sp_Ac_SupplierAgeing_SelectRow;

        if (dt.Rows.Count > 0 && dt != null)
        {
            if (objArr[0].ToString().Trim() != "" && objArr[0].ToString().Trim() != "0")
            {
                dt = new DataView(dt, "Other_Account_No='" + objArr[0].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (objArr[5].ToString().Trim() != "" && objArr[5].ToString().Trim() != "0")
            {
                dt = new DataView(dt, "Days_Overdue>='" + objArr[5].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (objArr[4].ToString().Trim() == "False")
            {
                dt = new DataView(dt, "Due_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
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


        string po_number = "";
        if (dt.Rows.Count > 0)
        {
            dt.Columns["Po_No"].ReadOnly = false;
            foreach (DataRow dr in dt.Rows)
            {
                po_number = "";
                string sql = "select distinct (Case when Inv_PurchaseInvoiceHeader.SupInvoiceNo='' or Inv_PurchaseInvoiceHeader.SupInvoiceNo is null then '0' else Inv_PurchaseInvoiceHeader.SupInvoiceNo end) as po_number from dbo.Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.InvoiceNo='" + dr["Invoice_No"].ToString() + "'";
                DataTable dtPoNumber = da.return_DataTable(sql);
                if (dtPoNumber.Rows.Count > 0)
                {
                    foreach (DataRow dr_po in dtPoNumber.Rows)
                    {
                        if (po_number != "")
                        {
                            po_number = po_number + "," + dr_po["po_number"].ToString();
                        }
                        else
                        {
                            po_number = dr_po["po_number"].ToString();
                        }
                    }
                }
                dtPoNumber.Dispose();
                dr["Po_No"] = po_number;
            }
        }

        DataTable DtCompany = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Field2"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_SupplierAgeing_SelectRow";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.SetImage(Imageurl);
        objTrialBalanceprint.setDateFilter(strFilter);
    }
}