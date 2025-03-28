using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.XtraReports.UI;

public partial class Accounts_Report_SupplierStatement : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    SupplierStatement objTrialBalanceprint = null;
    PartyStatement ObjPartyStatement = null;
    CompanyMaster Objcompany = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objsys = null;

    string strCompId = string.Empty;
    string strBrandId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new SupplierStatement(Session["DBConnection"].ToString());
        ObjPartyStatement = new PartyStatement(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());

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
        string strLastBalance = string.Empty;
        string strSta = string.Empty;

        XRTable XrAgeing = new XRTable();

        XrAgeing = (XRTable)objTrialBalanceprint.FindControl("xrtable6", true);
        XrAgeing.Visible = false;

        ArrayList objArr = (ArrayList)Session["dtAcParam"];
        AccountsDataset ObjAccountDataset = new AccountsDataset();
        ObjAccountDataset.EnforceConstraints = false;


        //AccountsDatasetTableAdapters.sp_Ac_AllStatements_SelectRowTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_AllStatements_SelectRowTableAdapter();

        //adp.Fill(ObjAccountDataset.sp_Ac_AllStatements_SelectRow, Convert.ToInt32(strCompId), Convert.ToInt32(strBrandId), objArr[3].ToString().Trim(), Convert.ToInt32(objArr[5].ToString().Trim()), Convert.ToInt32(objArr[0].ToString().Trim()), DateTime.Parse(objArr[1].ToString().Trim()), DateTime.Parse(objArr[2].ToString().Trim()), Convert.ToInt32(objArr[14].ToString().Trim()));

        //(DataTable)Session["dtSupplierStatement"]
        //DataTable dt = ObjAccountDataset.sp_Ac_AllStatements_SelectRow;
        DataTable dt = (DataTable)Session["dtSupplierStatement"];
        //for filter by date
        //if (objArr[1].ToString().Trim() != "")
        //{
        //    DateTime dtToDate = Convert.ToDateTime(objArr[2].ToString());
        //    dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);
        //    dt = new DataView(dt, "Voucher_Date >= '" + objArr[1].ToString() + "' and  Voucher_Date <= '" + dtToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat());
        //    Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
        //    strFilter = "From " + Convert.ToDateTime(objArr[1].ToString()).ToString(objsys.SetDateFormat()) + " To " + Convert.ToDateTime(objArr[2].ToString()).ToString(objsys.SetDateFormat());
        //}

        //for filter by location
        //if (objArr[13].ToString().Trim() != "--Select--")
        //{
        //    dt = new DataView(dt, "Voucher_Type='" + objArr[13].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //}

        string strStatus = "False";
        string strBalanceA = string.Empty;

        int fyear_id = 0;
        fyear_id = Common.getFinancialYearId(Convert.ToDateTime(objArr[1].ToString()), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());


        foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;

        double OpeningBalance = 0;
        //PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass();
        //DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + strCompId + "','" + strBrandId + "','" + objArr[3].ToString().Trim() + "',  '" + objArr[4].ToString().Trim() + "', '0','" + objArr[5].ToString().Trim() + "','" + objArr[0].ToString() + "' ,'" + objArr[14].ToString().Trim() + "', '" + fyear_id + "')) OpeningBalance");
        //if (dtOpeningBalance.Rows.Count > 0)
        //{
        //    OpeningBalance = Convert.ToDouble(dtOpeningBalance.Rows[0][0].ToString());
        //    if (OpeningBalance < 0)
        //    {
        //        strSta = "Credit";
        //    }
        //    else if (OpeningBalance > 0)
        //    {
        //        strSta = "Debit";
        //    }
        //}
        //else
        //{
        //    OpeningBalance = 0;
        //}
        double.TryParse(objArr[16].ToString().Trim(), out OpeningBalance);
        //OpeningBalance = Convert.ToDouble(objArr[16].ToString().Trim());
        double openingForeign = 0;
        double.TryParse(objArr[17].ToString().Trim(), out openingForeign);
        if (OpeningBalance > 0)
        {
            strSta = "Credit";
        }
        else if (OpeningBalance < 0)
        {
            strSta = "Debit";
        }
        double localBalance = OpeningBalance;
        double foreignBalance = openingForeign;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (strStatus == "False")
            {
                if (OpeningBalance != 0)
                {
                    if (strSta == "Credit")
                    {
                        dt.Rows[i]["BalanceAmount"] = OpeningBalance.ToString();
                        //dt.Rows[i]["Foreign_Amount"]= openingForeign.ToString().Split('-')[1].ToString();
                        strStatus = "True";
                    }
                    else if (strSta == "Debit")
                    {
                        dt.Rows[i]["BalanceAmount"] = OpeningBalance.ToString();
                        //dt.Rows[i]["Foreign_Amount"] = "-" + openingForeign.ToString();
                        strStatus = "True";
                    }

                    //if (OpeningBalance < 0)
                    //{
                    //    dt.Rows[i]["BalanceAmount"] = OpeningBalance.ToString().Split('-')[1].ToString();
                    //    strStatus = "True";
                    //}
                    //else if (OpeningBalance > 0)
                    //{
                    //    dt.Rows[i]["BalanceAmount"] = "-" + OpeningBalance.ToString();
                    //    strStatus = "True";
                    //}

                    //dt.Rows[i]["BalanceAmount"] = objArr[4].ToString();
                    //strStatus = "True";
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

            dt.Rows[i]["Foreign_Amount"] = objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), dt.Rows[i]["Foreign_Amount"].ToString());
            if (Convert.ToDouble(dt.Rows[i]["Debit_Amount"].ToString()) > 0)
            {
                foreignBalance = foreignBalance - double.Parse(dt.Rows[i]["Foreign_Amount"].ToString());
                try
                {
                    dt.Rows[i]["BalanceAmount"] = (Convert.ToDouble(dt.Rows[i]["BalanceAmount"].ToString()) - Convert.ToDouble(dt.Rows[i]["Debit_Amount"].ToString())).ToString();
                }
                catch
                {
                    dt.Rows[i]["BalanceAmount"] = objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(dt.Rows[i]["BalanceAmount"].ToString()) - Convert.ToDouble(dt.Rows[i]["Debit_Amount"].ToString())).ToString());
                }
                strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                strLastBalance = strBalanceA;
            }
            else if (Convert.ToDouble(dt.Rows[i]["Credit_Amount"].ToString()) > 0)
            {
                foreignBalance = foreignBalance + double.Parse(dt.Rows[i]["Foreign_Amount"].ToString());
                try
                {
                    dt.Rows[i]["BalanceAmount"] = (Convert.ToDouble(dt.Rows[i]["BalanceAmount"].ToString()) + Convert.ToDouble(dt.Rows[i]["Credit_Amount"].ToString())).ToString();
                }
                catch
                {

                    dt.Rows[i]["BalanceAmount"] = objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), (Convert.ToDouble(dt.Rows[i]["BalanceAmount"].ToString()) + Convert.ToDouble(dt.Rows[i]["Credit_Amount"].ToString())).ToString());

                }


                strBalanceA = dt.Rows[i]["BalanceAmount"].ToString();
                strLastBalance = strBalanceA;
            }

            dt.Rows[i]["fBalanceAmount"] = objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), foreignBalance.ToString());
        }

        string strFinalOpening = string.Empty;
        //For Opening Balance
        if (strSta == "Credit")
        {
            strFinalOpening = OpeningBalance.ToString();
        }
        else if (strSta == "Debit")
        {
            strFinalOpening = OpeningBalance.ToString();
        }

        DataTable DtCompany = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Field2"].ToString();
        }

        if (objArr[5].ToString().Trim() != "0")
        {

            objTrialBalanceprint.DataSource = dt;
            Session["DtReportStatement"] = dt;
            objTrialBalanceprint.DataMember = "sp_Ac_AllStatements_SelectRow";
            ReportViewer1.Report = objTrialBalanceprint;
            ReportToolbar1.ReportViewer = ReportViewer1;
            objTrialBalanceprint.setcompanyname(CompanyName);
            objTrialBalanceprint.SetImage(Imageurl);
            objTrialBalanceprint.setFOpeningBalance(objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), openingForeign.ToString()));
            objTrialBalanceprint.setForeignBalance(objsys.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), foreignBalance.ToString()));
            objTrialBalanceprint.setReportTitle("SUPPLIER STATEMENT");
            lblHeader.Text = "SUPPLIER STATEMENT";
            objTrialBalanceprint.setSupplierText("Supplier Name");
            objTrialBalanceprint.set0_30(objArr[7].ToString());
            objTrialBalanceprint.set31_60(objArr[8].ToString());
            objTrialBalanceprint.set61_90(objArr[9].ToString());
            objTrialBalanceprint.set91_180(objArr[10].ToString());
            objTrialBalanceprint.set181_365(objArr[11].ToString());
            objTrialBalanceprint.setabove365(objArr[12].ToString());

            if (objArr[6].ToString().Trim() == "True")
            {
                XrAgeing.Visible = true;
            }
            else
            {
                XrAgeing.Visible = false;
            }

            objTrialBalanceprint.setDateFilter(strFilter);
            objTrialBalanceprint.setOPeningBalance(strFinalOpening);
            objTrialBalanceprint.setCurrencySymbol(objArr[15].ToString().Trim());
            objTrialBalanceprint.setLastBalance(strLastBalance);
        }
        else
        {
            ObjPartyStatement.DataSource = dt;
            Session["DtReportStatement"] = dt;
            ObjPartyStatement.DataMember = "sp_Ac_AllStatements_SelectRow";
            ReportViewer1.Report = ObjPartyStatement;
            ReportToolbar1.ReportViewer = ReportViewer1;
            ObjPartyStatement.setcompanyname(CompanyName);
            ObjPartyStatement.SetImage(Imageurl);

            ObjPartyStatement.setReportTitle("PARTY STATEMENT");
            lblHeader.Text = "PARTY STATEMENT";
            ObjPartyStatement.setSupplierText("Supplier Name");
            ObjPartyStatement.set0_30(objArr[7].ToString());
            ObjPartyStatement.set31_60(objArr[8].ToString());
            ObjPartyStatement.set61_90(objArr[9].ToString());
            ObjPartyStatement.set91_180(objArr[10].ToString());
            ObjPartyStatement.set181_365(objArr[11].ToString());
            ObjPartyStatement.setabove365(objArr[12].ToString());
            ObjPartyStatement.setDateFilter(strFilter);
            ObjPartyStatement.setOPeningBalance(strFinalOpening);
            ObjPartyStatement.setCurrencySymbol(objArr[15].ToString().Trim());
            ObjPartyStatement.setLastBalance(strLastBalance);
        }
    }
}