using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Set_CustomerMaster_CreditParameter
/// </summary>
public class Set_CustomerMaster_CreditParameter
{
    DataAccessClass objDa = null;
    DataAccessClass daClass = null;
    private string _strConString = string.Empty;
    public Set_CustomerMaster_CreditParameter(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        objDa = new DataAccessClass(strConString);
        daClass = new DataAccessClass(strConString);
        _strConString = strConString;

    }


    public int InsertRecord(string Customer_Id, string Credit_Limit, string Credit_Limit_Currency, string Credit_Days, string Is_Adavance_Cheque_Basis, string Is_Invoice_To_Invoice, string Is_Half_Advance, string Financial_Statement_Name, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[21];

        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Credit_Limit", Credit_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Credit_Limit_Currency", Credit_Limit_Currency, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Credit_Days", Credit_Days, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Is_Adavance_Cheque_Basis", Is_Adavance_Cheque_Basis, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Is_Invoice_To_Invoice", Is_Invoice_To_Invoice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Half_Advance", Is_Half_Advance, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Financial_Statement_Name", Financial_Statement_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Set_CustomerMaster_CreditParameter_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[20].ParaValue);
    }

    public int InsertRecord(string Customer_Id, string Credit_Limit, string Credit_Limit_Currency, string Credit_Days, string Is_Adavance_Cheque_Basis, string Is_Invoice_To_Invoice, string Is_Half_Advance, string Financial_Statement_Name, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[21];

        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Credit_Limit", Credit_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Credit_Limit_Currency", Credit_Limit_Currency, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Credit_Days", Credit_Days, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Is_Adavance_Cheque_Basis", Is_Adavance_Cheque_Basis, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Is_Invoice_To_Invoice", Is_Invoice_To_Invoice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Half_Advance", Is_Half_Advance, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Financial_Statement_Name", Financial_Statement_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Set_CustomerMaster_CreditParameter_Insert", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }

    public int DeleteRecord_By_CustomerId(string Customer_Id, string strRecordType, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[2] = new PassDataToSql("@RecordType", strRecordType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_CustomerMaster_CreditParameter_DeleteRow", paramList, ref trns);
        return Convert.ToInt32(paramList[1].ParaValue);
    }

    public int DeleteRecord_By_CustomerId(string Customer_Id, string strRecordType)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[2] = new PassDataToSql("@RecordType", strRecordType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_CustomerMaster_CreditParameter_DeleteRow", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }




    public DataTable GetRecord_By_CustomerId(string Customer_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_CustomerMaster_CreditParameter_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetCustomerRecord_By_OtherAccountId(string strOtherAccountId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Customer_Id", strOtherAccountId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Op_Type", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_CustomerMaster_CreditParameter_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetSupplierRecord_By_OtherAccountId(string strOtherAccountId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Customer_Id", strOtherAccountId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Op_Type", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_CustomerMaster_CreditParameter_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetRecord_By_CustomerId(string Customer_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Customer_Id", Customer_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_CustomerMaster_CreditParameter_SelectRow", paramList, ref trns);
        return dtInfo;
    }

    public string[] checkCreditLimit(int inovice_id, double InvoiceCreditAmount, string strCustomerId, string strOtherAccountId, string strInvoiceDate, string strCurrencyId,string strCompId, string strBrandId, string strLocId, string strFinanceYearId)
    {
        string[] _result = new string[2];
        try
        {
            DataTable dtCreditParameter = GetCustomerRecord_By_OtherAccountId(strOtherAccountId);
            if (dtCreditParameter.Rows.Count==0 || double.Parse(dtCreditParameter.Rows[0]["Credit_Limit"].ToString()) == 0)
            {
                throw new Exception("This is Cash Customer ,you can not generate credit invoice");
            }

            string strStatus = dtCreditParameter.Rows[0]["field4"].ToString();
            if (strStatus == "Pending" || strStatus == "Rejected")
            {
                throw new Exception("Customer credit request is " + strStatus + ". ,you can not generate invoice");
            }

            double LocalInvoiceAmt=0;
            double TotalUnpostInvoicesum = 0;
            double CreditLimit = 0;
            int CreditDays = 0;
            double AdvacnechequeAmount = 0;
            double closingBalance = 0;
            double dueamount = 0;
            double ActualCreditLimit = 0;
            double advanceamt = 0;
            double unPostedVoucherAmount = 0;
           
            CreditLimit = Convert.ToDouble(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
            CreditDays = Convert.ToInt32(dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim());

            //calculation for get unposted invoice sum
            string sqlUnPostedInvoice = string.Empty;
            if (inovice_id == 0)
            {
                sqlUnPostedInvoice = "select SUM(pay.Pay_Charges) as LAmount,SUM(pay.FCPayAmount) as FAmount from inv_salesinvoiceheader SIH left join (select TransNo, FCPayAmount, Pay_Charges, PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans = 'SI')pay on pay.TransNo = SIH.Trans_Id inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id = pay.PaymentModeId where SIH.Post = '0' and Set_Payment_Mode_Master.Field1 = 'Credit' and sih.Location_Id = " + strLocId + " and SIH.Supplier_Id = '" + strCustomerId + "' and SIH.Currency_id='" + strCurrencyId + "' and SIH.isActive='true' ";
            }
            else
            {
                sqlUnPostedInvoice = "select SUM(pay.Pay_Charges) as LAmount,SUM(pay.FCPayAmount) as FAmount from inv_salesinvoiceheader SIH left join (select TransNo, FCPayAmount, Pay_Charges, PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans = 'SI')pay on pay.TransNo = SIH.Trans_Id inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id = pay.PaymentModeId where SIH.Post = '0' and Set_Payment_Mode_Master.Field1 = 'Credit' and sih.Location_Id = " + strLocId + " and SIH.Supplier_Id = '" + strCustomerId + "' and SIH.Currency_id='" + strCurrencyId + "' and SIH.trans_id<>'" + inovice_id + "' and SIH.isActive='true'";
            }
            DataTable dtUnPostInvoice = objDa.return_DataTable(sqlUnPostedInvoice);
            if (dtUnPostInvoice.Rows.Count > 0)
            {
                Double.TryParse(dtUnPostInvoice.Rows[0]["FAmount"].ToString(), out TotalUnpostInvoicesum);
            }
            dtUnPostInvoice.Dispose();

            //Get current foreign balance of selected account
            string strsql = "select  dbo.Ac_GetBalance(" + strCompId + "," + strBrandId + "," + strLocId + ",'" + DateTime.Now.ToString() + "',0," + Ac_ParameterMaster.GetCustomerAccountNo(strCompId, _strConString) +  "," + strOtherAccountId + ",3,'" + strFinanceYearId + "')";
            double.TryParse(objDa.get_SingleValue(strsql), out closingBalance);

            //Get Unposted Voucher Active voucher
            strsql = "select sum((case when Debit_Amount > 0 then Foreign_Amount else 0 end)) as amt  from ac_voucher_detail inner join ac_voucher_header on ac_voucher_header.trans_id = ac_voucher_detail.voucher_no where ac_voucher_header.Location_id='" + strLocId + "' and ac_voucher_header.IsActive = 'true' and ac_voucher_header.ReconciledFromFinance = 'false' and Account_No = '" + Ac_ParameterMaster.GetCustomerAccountNo(strCompId, _strConString) + "' and Other_Account_No = '" + strOtherAccountId + "'";
            double.TryParse(objDa.get_SingleValue(strsql), out unPostedVoucherAmount);
            closingBalance = closingBalance + unPostedVoucherAmount;

            closingBalance = closingBalance + TotalUnpostInvoicesum;
            if (closingBalance < 0)
            {   
                advanceamt = -closingBalance;
            }
            else
            {
                dueamount = closingBalance;
            }

            LocalInvoiceAmt = InvoiceCreditAmount;
            ActualCreditLimit = CreditLimit - dueamount + advanceamt;
            //when only credit tlimit is mentioned then we add validation 
            if (!Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()) && !Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()) && !Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
            {
                if (LocalInvoiceAmt > ActualCreditLimit)
                {
                    throw new Exception("Customer credit limit is over (Excess Limit - " + SystemParameter.GetAmountInLoginLocationCurrency((LocalInvoiceAmt-ActualCreditLimit).ToString(),_strConString,2, strCurrencyId) + ")");
                }
            }

            else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
            {
                DateTime ToDate = Convert.ToDateTime(strInvoiceDate);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                ToDate = ToDate.AddDays(CreditDays);
                string sql = "select sum((case when Credit_Amount > 0 then Foreign_Amount else 0 end)) as amt  from ac_voucher_detail inner join ac_voucher_header on ac_voucher_header.trans_id = ac_voucher_detail.voucher_no where ac_voucher_header.Location_id='" + strLocId + "' and ac_voucher_header.IsActive = 'true' and Account_No = '" + Ac_ParameterMaster.GetCustomerAccountNo(strCompId, _strConString) + "' and Other_Account_No = '" + strOtherAccountId + "' and ac_voucher_detail.Cheque_Clear_Date>= '" + strInvoiceDate + "' and ac_voucher_detail.Cheque_Clear_Date<= '" + ToDate.ToString() + "'";
                double.TryParse(objDa.get_SingleValue(sql), out AdvacnechequeAmount);
                if ((LocalInvoiceAmt + TotalUnpostInvoicesum) > ActualCreditLimit)
                {
                    throw new Exception("Customer credit limit is over (Excess Limit - " + SystemParameter.GetAmountInLoginLocationCurrency(((LocalInvoiceAmt + TotalUnpostInvoicesum) - ActualCreditLimit).ToString(),_strConString,2,strCurrencyId) + ")");
                }

                if (AdvacnechequeAmount == 0 || (LocalInvoiceAmt + TotalUnpostInvoicesum) > AdvacnechequeAmount)
                {
                    throw new Exception("pdc cheque required of  " + CreditDays + " days");
                }
            }
            else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
            {
                if ((dueamount + TotalUnpostInvoicesum) > 0)
                {
                    throw new Exception("Previous Invoice amount (" + SystemParameter.GetAmountInLoginLocationCurrency((dueamount + TotalUnpostInvoicesum).ToString(),_strConString,2,strCurrencyId)  +") is Pending so you can not generate new invoice ");
                }
                else if (LocalInvoiceAmt > ActualCreditLimit)
                {
                    throw new Exception("Credit limit is " + CreditLimit + " so Invoice amount should be less than or equal to Credit limit");
                }
            }
            else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
            {
                if ((LocalInvoiceAmt + TotalUnpostInvoicesum) > ActualCreditLimit)
                {
                    throw new Exception("Credit balance is " + (ActualCreditLimit) + " so invoice amount should be less than or equal to credit balance");
                }

                if ((LocalInvoiceAmt / 2) > closingBalance)
                {
                    throw new Exception("half Invoice amount should be less than or equal to Closing balance");
                }
            }
            _result[0] = "True";
            _result[1] = "Customer Eligible for credit inoice";
            return _result;
        }
        catch (Exception ex)
        {
            _result[0] = "false";
            _result[1] = ex.Message;
            return _result;
        }
    }

}