using System;
using System.Data;
using System.Configuration;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Inv_PaymentTrans
/// </summary>
public class Inv_PaymentTrans
{
    DataAccessClass DaClass = null;
    public Inv_PaymentTrans(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        DaClass = new DataAccessClass(strConString);
    }
    public int insert(string CompanyId, string PaymentModeId, string TypeTrans, string TransNo, string Amount, string AccountNo, string CardNo, string CardName, string BankAccountNo, string BankId, string BankAccountName, string ChequeNo, string ChequeDate, string ExpensesCharges, string ExpCurrencyID, string ExpExchangeRate, string FCExpAmount, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] ParamList = new PassDataToSql[30];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@PaymentModeId", PaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", TypeTrans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Amount", Amount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[5] = new PassDataToSql("@AccountNo", AccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[6] = new PassDataToSql("@CardNo", CardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[7] = new PassDataToSql("@CardName", CardName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[8] = new PassDataToSql("@BankAccountNo", BankAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[9] = new PassDataToSql("@BankId", BankId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[10] = new PassDataToSql("@BankAccountName", BankAccountName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[11] = new PassDataToSql("@ChequeNo", ChequeNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[12] = new PassDataToSql("@ChequeDate", ChequeDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[13] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[14] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[15] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[16] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[17] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[18] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        ParamList[19] = new PassDataToSql("@Pay_Charges", ExpensesCharges, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[20] = new PassDataToSql("@PayCurrencyID", ExpCurrencyID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[21] = new PassDataToSql("@PayExchangeRate", ExpExchangeRate, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[22] = new PassDataToSql("@FCPayAmount", FCExpAmount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[23] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[24] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[25] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[26] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[27] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[28] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        ParamList[29] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        DaClass.execute_Sp("Inv_PaymentTrn_Insert", ParamList);
        return Convert.ToInt32(ParamList[18].ParaValue);

    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public int insert(string CompanyId, string PaymentModeId, string TypeTrans, string TransNo, string Amount, string AccountNo, string CardNo, string CardName, string BankAccountNo, string BankId, string BankAccountName, string ChequeNo, string ChequeDate, string ExpensesCharges, string ExpCurrencyID, string ExpExchangeRate, string FCExpAmount, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] ParamList = new PassDataToSql[30];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@PaymentModeId", PaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", TypeTrans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Amount", Amount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[5] = new PassDataToSql("@AccountNo", AccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[6] = new PassDataToSql("@CardNo", CardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[7] = new PassDataToSql("@CardName", CardName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[8] = new PassDataToSql("@BankAccountNo", BankAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[9] = new PassDataToSql("@BankId", BankId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[10] = new PassDataToSql("@BankAccountName", BankAccountName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[11] = new PassDataToSql("@ChequeNo", ChequeNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[12] = new PassDataToSql("@ChequeDate", ChequeDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[13] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[14] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[15] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[16] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[17] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[18] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        ParamList[19] = new PassDataToSql("@Pay_Charges", ExpensesCharges, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[20] = new PassDataToSql("@PayCurrencyID", ExpCurrencyID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[21] = new PassDataToSql("@PayExchangeRate", ExpExchangeRate, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[22] = new PassDataToSql("@FCPayAmount", FCExpAmount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[23] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[24] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[25] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[26] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[27] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[28] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        ParamList[29] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        DaClass.execute_Sp("Inv_PaymentTrn_Insert", ParamList,ref trns);
        return Convert.ToInt32(ParamList[18].ParaValue);

    }
   
    
    
    public int Update(string CompanyId, string TransId, string PaymentModeId, string TypeTrans, string TransNo, string Amount, string AccountNo, string CardNo, string CardName, string BankAccountNo, string BankId, string BankAccountName, string ChequeNo, string ChequeDate, string ExpensesCharges, string ExpCurrencyID, string ExpExchangeRate, string FCExpAmount, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] ParamList = new PassDataToSql[29];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@PaymentModeId", PaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", TypeTrans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Amount", Amount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[5] = new PassDataToSql("@AccountNo", AccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[6] = new PassDataToSql("@CardNo", CardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[7] = new PassDataToSql("@CardName", CardName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[8] = new PassDataToSql("@BankAccountNo", BankAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[9] = new PassDataToSql("@BankId", BankId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[10] = new PassDataToSql("@BankAccountName", BankAccountName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[11] = new PassDataToSql("@ChequeNo", ChequeNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[12] = new PassDataToSql("@ChequeDate", ChequeDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[13] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[14] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[15] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[16] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        ParamList[17] = new PassDataToSql("@TransID", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[18] = new PassDataToSql("@Pay_Charges", ExpensesCharges, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[19] = new PassDataToSql("@PayCurrencyID", ExpCurrencyID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[20] = new PassDataToSql("@PayExchangeRate", ExpExchangeRate, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        ParamList[21] = new PassDataToSql("@FCPayAmount", FCExpAmount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[22] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[23] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[24] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[25] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        ParamList[26] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[27] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        ParamList[28] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        DaClass.execute_Sp("Inv_PaymentTrn_Update", ParamList);
        return Convert.ToInt32(ParamList[16].ParaValue);

    }
    public int Delete(string CompanyId, string TransId, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] ParamList = new PassDataToSql[6];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[5] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_PaymentTrn_RowStatus", ParamList);
        return Convert.ToInt32(ParamList[5].ParaValue);

    }
    public DataTable GetPaymentTrans()
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransTrue()
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransTrue(string CompanyId)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransTrue(string CompanyId, string TransId)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransTrue(string CompanyId, string TransType, string TransNo)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", TransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "8", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public DataTable GetPaymentTransTrue(string CompanyId, string TransType, string TransNo, ref SqlTransaction trns)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", TransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "8", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList,ref trns);
        return dt;

    }
    public DataTable GetPaymentTransFalse()
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransFalse(string CompanyId)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetPaymentTransFalse(string CompanyId,string TransId)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TransID", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TypeTrans", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@TransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_PaymentTrn_SelectRow", ParamList);
        return dt;

    }
    public int DeleteByRefandRefNo(string CompanyId, string TypeTrans, string TransNo)
    {
        PassDataToSql[] ParamList = new PassDataToSql[4];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TypeTrans", TypeTrans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_PaymentTrn_Delete", ParamList);
        return Convert.ToInt32(ParamList[3].ParaValue);

    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay


    public int DeleteByRefandRefNo(string CompanyId, string TypeTrans, string TransNo, ref SqlTransaction trns)
    {
        PassDataToSql[] ParamList = new PassDataToSql[4];
        ParamList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@TypeTrans", TypeTrans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@TransNo", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_PaymentTrn_Delete", ParamList,ref trns);
        return Convert.ToInt32(ParamList[3].ParaValue);

    }
 
  }
