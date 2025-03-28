using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_AllCustomerDetail : System.Web.UI.Page
{
    Ac_ParameterMaster objAcParameter = null;
    EmployeeMaster objEmployee = null;
    AllCustomerRecord objTrialBalanceprint = null;
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
        objTrialBalanceprint = new AllCustomerRecord(Session["DBConnection"].ToString());
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

        //For Account Parameter
        string strReceiveVoucherAcc = string.Empty;
        string strCurrencyType = string.Empty;

        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(strCompId);
        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

        string CompanyName = "";
        string Imageurl = "";
        string strFilter = string.Empty;

        ArrayList objArr = (ArrayList)Session["dtAllCusParam"];
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


        AccountsDatasetTableAdapters.sp_Ac_AllCustomer_BalanceTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_AllCustomer_BalanceTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountDataset.sp_Ac_AllCustomer_Balance, Convert.ToInt32(strCompId), Convert.ToInt32(strBrandId), strReceiveVoucherAcc, Convert.ToInt32(strReceiveVoucherAcc), strLocationId, dtFromDate, dtToDate, Convert.ToInt32(strCurrencyType), Convert.ToInt32(Session["FinanceYearId"].ToString()), 1);

        DataTable dt = ObjAccountDataset.sp_Ac_AllCustomer_Balance;

        //for filter by date
        if (dtFromDate.ToString() != "")
        {
            //dt = new DataView(dt, "Voucher_Date >= '" + objArr[1].ToString() + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat());
            Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
            strFilter = "From " + Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
        }


        if (dt.Rows.Count > 0)
        {
            dt = new DataView(dt, "Name not is null", "", DataViewRowState.CurrentRows).ToTable();
            if (objArr[3].ToString().Trim() != "")
            {
                if (objArr[3].ToString().Trim() == "False")
                {
                    dt = new DataView(dt, "Closing_Final<>'0'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }

        dt = dt.DefaultView.ToTable( /*distinct*/ true);


        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }

        objTrialBalanceprint.DataSource = dt;
        Session["DtReportStatement"] = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_AllCustomer_Balance";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.setCurrencySymbol(objArr[5].ToString().Trim());
        objTrialBalanceprint.SetImage(Imageurl);
        objTrialBalanceprint.setDateFilter(strFilter);

    }
}