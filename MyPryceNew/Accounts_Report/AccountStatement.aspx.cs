using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_AccountStatement : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    AccountStatementPrint objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objsys = null;

    string strCompId = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new AccountStatementPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        if (Session["dtAcParam"] != null)
        {
            GetReport();
        }
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        string CompanyName = "";
        string Imageurl = "";
        string strFilter = string.Empty;
        string strLastBalance = string.Empty;

        ArrayList objArr = (ArrayList)Session["dtAcParam"];


        int count = objArr.Count;

        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        AccountsDatasetTableAdapters.sp_Ac_AllStatements_SelectRowTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_AllStatements_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        if (count > 9)
        {
            adp.Fill(ObjAccountDataset.sp_Ac_AllStatements_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), objArr[3].ToString(), Convert.ToInt32(objArr[0].ToString()), Convert.ToInt32(objArr[9].ToString()), DateTime.Parse(objArr[1].ToString()), DateTime.Parse(objArr[2].ToString()), Convert.ToInt32(objArr[6].ToString()));
        }
        else
        {
            adp.Fill(ObjAccountDataset.sp_Ac_AllStatements_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), objArr[3].ToString(), Convert.ToInt32(objArr[0].ToString()), 0, DateTime.Parse(objArr[1].ToString()), DateTime.Parse(objArr[2].ToString()), Convert.ToInt32(objArr[6].ToString()));
        }

        DataTable dt = ObjAccountDataset.sp_Ac_AllStatements_SelectRow;

        if (Request.QueryString["Type"] != null)
        {
            dt = new DataView(dt, "BankReconcilation='True'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //for filter by date
        if (objArr[1].ToString().Trim() != "")
        {
            DateTime dtToDate = Convert.ToDateTime(objArr[2].ToString());
            dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
            dt = new DataView(dt, "Voucher_Date >= '" + objArr[1].ToString() + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat());
            Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
            strFilter = "From " + Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
        }

        //for filter by VoucherType
        if (objArr[5].ToString().Trim() != "--Select--")
        {
            dt = new DataView(dt, "Voucher_Type='" + objArr[5].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        // Filter opening Balance

        string strStatus = "False";
        string strBalanceA = string.Empty;
        foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (strStatus == "False")
            {
                if (objArr[4].ToString() != "")
                {
                    if (objArr[8].ToString().Trim() == "Debit")
                    {
                        dt.Rows[i]["BalanceAmount"] = objArr[4].ToString();
                        strStatus = "True";
                    }
                    else if (objArr[8].ToString().Trim() == "Credit")
                    {
                        if (objArr[1].ToString().Trim() != "")
                        {
                            if (float.Parse(dt.Rows[i]["Debit_Amount"].ToString()) > 0)
                            {
                                if (count > 9)
                                {
                                    dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["Debit_Amount"].ToString()) - float.Parse(objArr[4].ToString())).ToString();
                                }
                                else
                                {
                                    dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["Debit_Amount"].ToString()) + float.Parse(objArr[4].ToString())).ToString();
                                }
                                strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                                strLastBalance = strBalanceA;
                            }
                            else if (float.Parse(dt.Rows[i]["Credit_Amount"].ToString()) > 0)
                            {
                                if (count > 9)
                                {
                                    dt.Rows[i]["BalanceAmount"] = (float.Parse(objArr[4].ToString()) + float.Parse(dt.Rows[i]["Credit_Amount"].ToString())).ToString();
                                }
                                else
                                {
                                    dt.Rows[i]["BalanceAmount"] = (float.Parse(objArr[4].ToString()) - float.Parse(dt.Rows[i]["Credit_Amount"].ToString())).ToString();
                                }
                                strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                                strLastBalance = strBalanceA;
                            }
                        }

                    }
                }
                else
                {
                    dt.Rows[i]["BalanceAmount"] = "0";
                    strStatus = "True";
                }
            }
            else
            {
                dt.Rows[i]["BalanceAmount"] = strBalanceA;
                strLastBalance = strBalanceA;
            }


            if (strStatus == "False")
            {
                strStatus = "True";
            }
            else
            {
                if (float.Parse(dt.Rows[i]["Debit_Amount"].ToString()) > 0)
                {
                    if (count > 9)
                    {
                        dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["BalanceAmount"].ToString()) - float.Parse(dt.Rows[i]["Debit_Amount"].ToString())).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["BalanceAmount"].ToString()) + float.Parse(dt.Rows[i]["Debit_Amount"].ToString())).ToString();
                    }
                    strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                    strLastBalance = strBalanceA;
                }
                else if (float.Parse(dt.Rows[i]["Credit_Amount"].ToString()) > 0)
                {
                    if (count > 9)
                    {
                        dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["BalanceAmount"].ToString()) + float.Parse(dt.Rows[i]["Credit_Amount"].ToString())).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["BalanceAmount"] = (float.Parse(dt.Rows[i]["BalanceAmount"].ToString()) - float.Parse(dt.Rows[i]["Credit_Amount"].ToString())).ToString();
                    }
                    strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                    strLastBalance = strBalanceA;
                }
            }

            if (count > 9)
            {
                dt.Rows[i]["AccountName"] = dt.Rows[i]["AccountName"].ToString() + "(" + objArr[10].ToString() + ")";
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
        objTrialBalanceprint.DataMember = "sp_Ac_AllStatements_SelectRow";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.setReportTitle("STATEMENT OF ACCOUNTS");
        lblHeader.Text = "STATEMENT OF ACCOUNTS";
        objTrialBalanceprint.setDateFilter(strFilter);
        objTrialBalanceprint.setOPeningBalance(objArr[4].ToString());
        objTrialBalanceprint.setLastBalance(strLastBalance);
    }
}