using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Ac_Ageing_Detail_Old
/// </summary>
public class Ac_Ageing_Detail_Old
{
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_ParameterMaster objAccParameter = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    SystemParameter objSys = null;
    LocationMaster ObjLocation = null;
    public Ac_Ageing_Detail_Old(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        objVoucherDetail = new Ac_Voucher_Detail(strConString);
        objVoucherHeader = new Ac_Voucher_Header(strConString);
        objAccParameter = new Ac_ParameterMaster(strConString);
        objAgeingDetail = new Ac_Ageing_Detail(strConString);
        objSys = new SystemParameter(strConString);
        ObjLocation = new LocationMaster(strConString);
    }
    public bool updateAgeingPendingInvoice(string StrCompId, string StrBrandId, string strLocationId, double voucherId, GridView gvAgeing, SqlTransaction trns, Boolean isSettlementGrid = false)
    {
        //------------------------code to update ageing by neelkanth purohit 18-feb-2017--------------------------
        //Ac_Voucher_Detail objVoucherDetail = new Ac_Voucher_Detail();
        //Ac_Voucher_Header objVoucherHeader = new Ac_Voucher_Header();
        //Ac_ParameterMaster objAccParameter = new Ac_ParameterMaster(StrCompId);

        DataTable dtVoucherDetail;
        DataTable dtVoucherHeader;

        string strNarration;

        dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, voucherId.ToString(), ref trns);


        //dtVoucherHeader = new DataView(dtVoucherHeader, "ReconciledFromFinance='true'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtVoucherHeader.Rows.Count == 0)
        {
            return false;
        }


        dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, voucherId.ToString(), ref trns);
        dtVoucherDetail = new DataView(dtVoucherDetail, "Other_account_no >0", "", DataViewRowState.CurrentRows).ToTable();



        DataRow drVoucherHeader = dtVoucherHeader.Rows[0];

        strNarration = drVoucherHeader["Narration"].ToString();

        string transaction_currency = dtVoucherDetail.Rows[0]["Currency_Id"].ToString();
        string strExchangeRate = dtVoucherDetail.Rows[0]["Exchange_Rate"].ToString();

        //String strDetailNarration = string.Empty;
        double ageLocalAmount = 0;
        double ageForeignAmount = 0;
        double ageCompanyAmount = 0;
        double exchangeRate = 0;
        Boolean localTransaction;
        //localTransaction = (SystemParameter.GetLocationCurrencyId(trns.Connection.ConnectionString,HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["LocId"].ToString()) == transaction_currency ? true : false);

        string strTemp = ObjLocation.GetLocationMasterById(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
        localTransaction = (strTemp == transaction_currency ? true : false);
        if (localTransaction == true)
        {
            exchangeRate = 1;
        }
        else
        {
            double.TryParse(strExchangeRate, out exchangeRate);
        }
        
        foreach (GridViewRow gvr in gvAgeing.Rows)
        {
            if ((isSettlementGrid == false ? ((CheckBox)gvr.FindControl("chkTrandId")).Checked : true) == true)
            {

                Label lblgvInvoiceNo = (Label)gvr.FindControl("lblPONo");
                strNarration = "Paid Amount for That Invoices : " + lblgvInvoiceNo.Text;
                TextBox txtPayAmount = (isSettlementGrid == false ? (TextBox)gvr.FindControl("txtpayLocal") : (TextBox)gvr.FindControl("txtSettleAmount"));
                if (localTransaction == true)
                {
                    double.TryParse(txtPayAmount.Text, out ageLocalAmount);
                    ageForeignAmount = ageLocalAmount;

                    string strToCurrency = HttpContext.Current.Session["CurrencyId"].ToString();
                    string strLocalAmount = ageLocalAmount.ToString();
                    string strForienAmount = "";
                    //string strToCurrency, string strLocalAmount, ref SqlTransaction trns, string strCompId, string strLocId,double dExchangeRate
                    try
                    {
                        strForienAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);
                        strForienAmount = strForienAmount + "/" + strExchangeRate;
                    }
                    catch
                    {
                        strForienAmount = "0";
                    }
                  //  double.TryParse(objSys.GetCurrency1(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), exchangeRate), out ageCompanyAmount );
                }
                else
                {
                    double.TryParse(txtPayAmount.Text, out ageForeignAmount);


                    //string amount,string strConString,string strCompId, string strLocId

                    //ageLocalAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);

                    //GetLocationCurrencyId(strConString, strCompId, strLocId)
                    //objsys.GetCurencyConversionForInv(GetLocationCurrencyId(strConString,strCompId,strLocId), amount)

                    string strLocationCurrencyId = "";
                    //strLocationCurrencyId = objSys.GetLocationCurrencyId1(ref trns, HttpContext.Current.Session["CurrencyId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    strLocationCurrencyId = strTemp;
                   // double dTemp = ageLocalAmount * exchangeRate;

                    double dTemp = ageForeignAmount * exchangeRate;
                    
                    ageLocalAmount = double.Parse(objSys.GetCurencyConversionForInv(strLocationCurrencyId.ToString(), dTemp.ToString(), ref trns));


                    //ageLocalAmount = double.Parse(SystemParameter.SetDecimal((ageForeignAmount * exchangeRate).ToString(), trns.Connection.ConnectionString, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                    //string strToCurrency, string strLocalAmount, ref SqlTransaction trns, string strCompId, string strLocId,double dExchangeRate)
                    //double.TryParse(SystemParameter.GetCurrency(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), trns.Connection.ConnectionString, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out ageCompanyAmount);
                    double.TryParse(objSys.GetCurrency1(HttpContext.Current.Session["CurrencyId"].ToString(), ageLocalAmount.ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),exchangeRate), out ageCompanyAmount);
                }
                if (ageLocalAmount > 0)
                {
                    objAgeingDetail.InsertAgeingDetail(StrCompId, StrBrandId, ((HiddenField)gvr.FindControl("hdnLocationId")).Value, ((HiddenField)gvr.FindControl("hdnRefType")).Value, ((HiddenField)gvr.FindControl("hdnRefId")).Value, lblgvInvoiceNo.Text, ((Label)gvr.FindControl("lblInvoiceDate")).Text, ((HiddenField)gvr.FindControl("hdnAccountNo")).Value, ((HiddenField)gvr.FindControl("hdnOtherAccountNo")).Value, "0", ageLocalAmount.ToString(), "0", drVoucherHeader["Cheque_Issue_Date"].ToString(), drVoucherHeader["Cheque_Clear_Date"].ToString(), drVoucherHeader["Cheque_No"].ToString(), strNarration.ToString(), HttpContext.Current.Session["EmpId"].ToString(), transaction_currency.ToString(), exchangeRate.ToString(), ageForeignAmount.ToString(), "0.00", ageCompanyAmount.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ((HiddenField)gvr.FindControl("hdnAgeingType")).Value, voucherId.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), (drVoucherHeader["Field3"].ToString() == "Pending" ? "False" : "True"), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

            }
        }
        return true;

    }
}