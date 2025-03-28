using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_AllSupplierDetail : System.Web.UI.Page
{
    Ac_ParameterMaster objAcParameter = null;
    EmployeeMaster objEmployee = null;
    AllSupplierRecord objTrialBalanceprint = null;
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

        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new AllSupplierRecord(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        //strLocationId = Session["LocId"].ToString();
        DateTime dtFromDate = new DateTime();
        DateTime dtToDate = new DateTime();


        string CompanyName = "";
        string Imageurl = "";
        string strFilter = string.Empty;

        //For Account Information
        string strAccountNumber = string.Empty;
        string strCashAccount = string.Empty;
        string strCreditAccount = string.Empty;
        string strPaymentVoucherAcc = string.Empty;
        string strCurrencyType = string.Empty;

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(strCompId);
        DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCash.Rows.Count > 0)
        {
            strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        strAccountNumber = strCashAccount + "," + strCreditAccount + "," + strPaymentVoucherAcc;
        //End

        ArrayList objArr = (ArrayList)Session["dtAllSupParam"];
        if (objArr[0].ToString().Trim() != "" && objArr[0].ToString().Trim() != "0")
        {
            strLocationId = objArr[0].ToString().Trim();
        }

        if (objArr[1].ToString().Trim() != "")
        {
            dtFromDate = DateTime.Parse(objArr[1].ToString().Trim());
        }

        if (objArr[2].ToString().Trim() != "")
        {
            dtToDate = Convert.ToDateTime(objArr[2].ToString());
            dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
        }

        if (objArr[4].ToString().Trim() != "")
        {
            if (objArr[4].ToString().Trim() == "True")
            {
                strCurrencyType = "1";
            }
            else if (objArr[4].ToString().Trim() == "False")
            {
                strCurrencyType = "2";
            }
        }

        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        AccountsDatasetTableAdapters.sp_Ac_AllSupplierDetail_SelectRowTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_AllSupplierDetail_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_AllSupplierDetail_SelectRow, Convert.ToInt32(strCompId), Convert.ToInt32(strBrandId), strAccountNumber, Convert.ToInt32(strPaymentVoucherAcc), strLocationId, dtFromDate, dtToDate, Convert.ToInt32(strCurrencyType), Convert.ToInt32(Session["FinanceYearId"].ToString()), 1);

        //for filter by date
        if (dtFromDate.ToString() != "")
        {
            //dt = new DataView(dt, "Voucher_Date >= '" + objArr[1].ToString() + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat());
            Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
            strFilter = "From " + Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
        }

        DataTable dt = ObjAccountDataset.sp_Ac_AllSupplierDetail_SelectRow;
        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "Name not is null", "", DataViewRowState.CurrentRows).ToTable();
            if (objArr[3].ToString().Trim() != "")
            {
                if (objArr[3].ToString().Trim() == "False")
                {
                    dt = new DataView(dt, "Opening_Final<>'0' or Debit_Final<>'0' or Credit_Final<>'0' or Closing_Final<>'0'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }


        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_AllSupplierDetail_SelectRow";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.setCurrencySymbol(objArr[5].ToString().Trim());
        objTrialBalanceprint.SetImage(Imageurl);
        objTrialBalanceprint.setDateFilter(strFilter);
    }
}