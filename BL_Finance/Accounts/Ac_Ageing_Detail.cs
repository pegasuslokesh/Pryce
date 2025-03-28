using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Ac_Ageing_Detail
/// </summary>
public class Ac_Ageing_Detail
{
    DataAccessClass daClass = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    LocationMaster ObjLocation = null;
    SystemParameter objsys = null;
    Ac_Parameter_Location objAccParameter = null;
    CompanyMaster objCompanyMaster = null;
    Ac_ParameterMaster objAcParaMaster = null;
    Common cm = null;
    CurrencyMaster ObjCurrency = null;
    public Ac_Ageing_Detail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objVoucherHeader = new Ac_Voucher_Header(strConString);
        objVoucherDetail = new Ac_Voucher_Detail(strConString);
        ObjLocation = new LocationMaster(strConString);
        objAccParameter = new Ac_Parameter_Location(strConString);
        objsys = new SystemParameter(strConString);
        cm = new Common(strConString);
        objCompanyMaster = new CompanyMaster(strConString);
        objAcParaMaster = new Ac_ParameterMaster(strConString);
        ObjCurrency = new CurrencyMaster(strConString);
    }
    public int InsertAgeingDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strReftype, string strRefId, string strInvoice_No, string strInvoiceDate, string strAccount_No, string strOther_Account_No, string strInvoice_Amount, string strPaid_Receive_Amount, string strDue_Amount, string strCheque_Issue_Date, string strCheque_Clear_Date, string strCheque_No, string strNarration, string strEmp_Id, string strCurrency_Id, string strExchange_Rate, string strForeign_Amount, string strCompanyCurrDebit, string strCompanyCurrCredit, string strFinancialYearId, string strAgeingType, string strVoucherId, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[38];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Ref_Type", strReftype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", strRefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Invoice_Date", strInvoiceDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Other_Account_No", strOther_Account_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Invoice_Amount", strInvoice_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Paid_Receive_Amount", strPaid_Receive_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Due_Amount", strDue_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Cheque_Issue_Date", strCheque_Issue_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Cheque_Clear_Date", strCheque_Clear_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Cheque_No", strCheque_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Narration", strNarration, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Exchange_Rate", strExchange_Rate, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Foreign_Amount", strForeign_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CompanyCurrDebit", strCompanyCurrDebit, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CompanyCurrCredit", strCompanyCurrCredit, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@FinancialYearId", strFinancialYearId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@AgeingType", strAgeingType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_Insert", paramList);
        return Convert.ToInt32(paramList[37].ParaValue);
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public int InsertAgeingDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strReftype, string strRefId, string strInvoice_No, string strInvoiceDate, string strAccount_No, string strOther_Account_No, string strInvoice_Amount, string strPaid_Receive_Amount, string strDue_Amount, string strCheque_Issue_Date, string strCheque_Clear_Date, string strCheque_No, string strNarration, string strEmp_Id, string strCurrency_Id, string strExchange_Rate, string strForeign_Amount, string strCompanyCurrDebit, string strCompanyCurrCredit, string strFinancialYearId, string strAgeingType, string strVoucherId, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[38];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Ref_Type", strReftype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", strRefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Invoice_Date", strInvoiceDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Other_Account_No", strOther_Account_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Invoice_Amount", strInvoice_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Paid_Receive_Amount", strPaid_Receive_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Due_Amount", strDue_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Cheque_Issue_Date", strCheque_Issue_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Cheque_Clear_Date", strCheque_Clear_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Cheque_No", strCheque_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Narration", strNarration, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Exchange_Rate", strExchange_Rate, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Foreign_Amount", strForeign_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CompanyCurrDebit", strCompanyCurrDebit, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CompanyCurrCredit", strCompanyCurrCredit, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@FinancialYearId", strFinancialYearId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@AgeingType", strAgeingType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[37].ParaValue);
    }
    public DataTable GetAgeingDetailByVoucherId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strVoucherId)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }

    }
    public DataTable GetAgeingDetailAll(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetAgeingDetailTillBrand(string strCompany_Id, string strBrand_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetAgeingDetailTillBrand(string strCompany_Id, string strBrand_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetAgeingDetailAllWithVoucherDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "10", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetAgeingDetailAll(string strCompany_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetAgeingDetailAllTrueFalse(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetAgeingDetailAllTrueFalse(string strCompany_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetAgeingDetailforStatements(string strCompany_Id, string strBrand_Id, string strLocationId, string strOtherAccountNo, string strAgeingType, string strOptype, string strCurrencyType)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Id", strCurrencyType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_Account_No", strOtherAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@AgeingType", strAgeingType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@VoucherId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@optype", strOptype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_Detail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public void DeleteAgeingDetailByVoucherId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strVoucher_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Voucher_Id", strVoucher_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus", paramList);
    }
    public void DeleteAgeingDetailByInvoice(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRef_Id, string strRef_Type, string strInvoice_No)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Ref_Id", strRef_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Type", strRef_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus_ByInvoice", paramList);
    }

    public void DeleteAgeingDetailByInvoice(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRef_Id, string strRef_Type, string strInvoice_No, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Ref_Id", strRef_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Type", strRef_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus_ByInvoice", paramList, ref trns);
    }

    public void DeleteAgeingDetailByInvoice(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRef_Id, string strRef_Type, string strInvoice_No, string strOtherAcNO, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Ref_Id", strRef_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Ref_Type", strRef_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Other_account_no", strOtherAcNO, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "DELETE FROM Ac_Ageing_Detail WHERE [Company_Id] = @Company_Id AND [Brand_Id] = @Brand_Id AND [Location_Id] = @Location_Id AND [Ref_Id] = @Ref_Id  AND[Ref_Type] = @Ref_Type AND [Invoice_No] = @Invoice_No and [other_account_no]=@Other_account_no";
        daClass.execute_CommandWithParams(sql, paramList, ref trns);
    }


    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public void DeleteAgeingDetailByVoucherId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strVoucher_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Voucher_Id", strVoucher_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus", paramList, ref trns);
    }
    public void DeleteAgeingDetailByTransId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus_ByTransID", paramList);
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public void DeleteAgeingDetailByTransId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ac_Ageing_Detail_RowStatus_ByTransID", paramList, ref trns);
    }
    public int DeleteAgeingIsActive(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@VoucherId", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_DeleteByIsActive", paramList);
        return Convert.ToInt32(paramList[7].ParaValue);
    }

    public int DeleteAgeingIsActive(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string IsActive, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@VoucherId", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_DeleteByIsActive", paramList, ref trns);
        return Convert.ToInt32(paramList[7].ParaValue);
    }

    public int UpdateAgeingDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string strDue_Amount, string strField2, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Due_Amount", strDue_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Ageing_Detail_Update", paramList);
        return Convert.ToInt32(paramList[9].ParaValue);
    }

    //For Customer Ageing Report
    public DataTable GetAgeingDetailforCustomerAgeing(string strBrand_Id, string strLocationId, string strFromDate, string strToDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@From_Date", strFromDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@To_Date", strToDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_CustomerAgeing_SelectRow", paramList))
        {
            return dtInfo;
        }
    }

    //For Supplier Ageing Report
    public DataTable GetAgeingDetailforSupplierAgeing(string strBrand_Id, string strLocationId, string strFromDate, string strToDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@From_Date", strFromDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@To_Date", strToDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_SupplierAgeing_SelectRow", paramList))
        {
            return dtInfo;
        }
    }

    public void insert_Ageing(string StrCompId, string StrBrandId, string strLocationId, string empID, string userID, string str_voucher_id, SqlTransaction trns)
    {

        //Ac_Ageing_Detail objAgeingDetail = new Ac_Ageing_Detail();


        DataTable dtVoucherDetail;
        DataTable dtVoucherHeader;
        double paidAmount = 0;
        double dueAmount = 0;
        double invoiceAmount = 0;
        int creditDays = 0;

        string strType = "";


        dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, str_voucher_id, ref trns);
        dtVoucherHeader = new DataView(dtVoucherHeader, "ReconciledFromFinance='true'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtVoucherHeader.Rows.Count == 0)
        {
            return;
        }

        strLocationId = dtVoucherHeader.Rows[0]["Location_id"].ToString();

        //DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, str_voucher_id, ref trns);


        DataRow drVoucherHeader = dtVoucherHeader.Rows[0];

        if (drVoucherHeader["Ref_Type"].ToString() == "PINV" || drVoucherHeader["Ref_Type"].ToString() == "SINV")
        {
            string sql = "select account_no,other_account_no,SUM(debit_amount) as debit_amount,SUM(credit_amount)as credit_amount,MAX(Exchange_Rate)as Exchange_Rate,SUM(Foreign_amount)as Foreign_Amount,SUM(companyCurrDebit) as companyCurrDebit, SUM(CompanyCurrCredit) as CompanyCurrCredit,MAX(Currency_id)as Currency_id  from ac_voucher_detail where Company_Id='" + StrCompId + "' and Brand_Id='" + StrBrandId + "' and Location_Id='" + strLocationId + "' and voucher_no='" + drVoucherHeader["Trans_id"].ToString() + "' and other_account_no>0 group by account_no,other_account_no";
            dtVoucherDetail = daClass.return_DataTable(sql, ref trns);
        }
        else
        {
            return;
            //dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, str_voucher_id, ref trns);
            //dtVoucherDetail = new DataView(dtVoucherDetail, "(account_no='" + objAccParameter.customerAcNo + "' or account_no='" + objAccParameter.supplierAcNo + "')  and other_account_no>0", "", DataViewRowState.CurrentRows).ToTable();
        }

        DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, str_voucher_id, "false", userID, DateTime.Now.ToString(), ref trns);

        if (dtVoucherDetail.Rows.Count > 0)
        {
            string sqlCreditDays = "";
            foreach (DataRow drVoucherDeatil in dtVoucherDetail.Rows)
            {
                dueAmount = 0;
                paidAmount = 0;

                if (drVoucherHeader["Ref_Type"].ToString() == "PINV")
                {
                    invoiceAmount = double.Parse(drVoucherDeatil["Credit_Amount"].ToString());
                    //sqlCreditDays = "select Set_CustomerMaster_CreditParameter.Credit_Days FROM Set_CustomerMaster_CreditParameter where Set_CustomerMaster_CreditParameter.Field2='S' and Set_CustomerMaster_CreditParameter.Customer_Id = '" + drVoucherDeatil["other_account_no"].ToString() + "'";
                    sqlCreditDays = "select Set_CustomerMaster_CreditParameter.Credit_Days FROM Set_CustomerMaster_CreditParameter inner join ac_accountMaster on ac_accountMaster.ref_id=Set_CustomerMaster_CreditParameter.Customer_Id where Set_CustomerMaster_CreditParameter.Field2='S' and ac_accountMaster.trans_id = '" + drVoucherDeatil["other_account_no"].ToString() + "'";
                    int.TryParse(daClass.get_SingleValue(sqlCreditDays, ref trns).ToString(), out creditDays);
                }
                else if (drVoucherHeader["Ref_Type"].ToString() == "SINV")
                {
                    invoiceAmount = double.Parse(drVoucherDeatil["Debit_Amount"].ToString());
                    //sqlCreditDays = "select Set_CustomerMaster_CreditParameter.Credit_Days FROM Set_CustomerMaster_CreditParameter where Set_CustomerMaster_CreditParameter.Field2='C' and Set_CustomerMaster_CreditParameter.Customer_Id = '" + drVoucherDeatil["other_account_no"].ToString() + "'";
                    sqlCreditDays = "select Set_CustomerMaster_CreditParameter.Credit_Days FROM Set_CustomerMaster_CreditParameter inner join ac_accountMaster on ac_accountMaster.ref_id=Set_CustomerMaster_CreditParameter.Customer_Id where Set_CustomerMaster_CreditParameter.Field2='C' and ac_accountMaster.trans_id = '" + drVoucherDeatil["other_account_no"].ToString() + "'";
                    int.TryParse(daClass.get_SingleValue(sqlCreditDays, ref trns).ToString(), out creditDays);
                }
                else
                {
                    invoiceAmount = 0;
                    creditDays = 0;
                }


                //objAcParaMaster   Ac_ParameterMaster.GetCustomerAccountNo
                if (drVoucherDeatil["account_no"].ToString() == objAcParaMaster.GetCustomerAccountNo1(StrCompId, ref trns))
                {
                    dueAmount = double.Parse(drVoucherDeatil["Debit_Amount"].ToString());

                    paidAmount = double.Parse(drVoucherDeatil["Credit_Amount"].ToString());
                    strType = "RV";

                }
                else if (drVoucherDeatil["account_no"].ToString() == objAcParaMaster.GetSupplierAccountNo1(StrCompId, ref trns))
                //For Ageing Detail Insert
                {

                    dueAmount = double.Parse(drVoucherDeatil["Credit_Amount"].ToString());

                    paidAmount = double.Parse(drVoucherDeatil["Debit_Amount"].ToString());
                    strType = "PV";

                }

                //string strAgeCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), lblgvDebitAmount.Text);
                //string AgeCompanyCurrDebit = strAgeCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                InsertAgeingDetail(drVoucherHeader["Company_id"].ToString(),
                    drVoucherHeader["Brand_id"].ToString(),
                    drVoucherHeader["Location_id"].ToString(),
                    drVoucherHeader["Ref_Type"].ToString(),
                    drVoucherHeader["Ref_id"].ToString(),
                    drVoucherHeader["Inv_Number"].ToString(),
                    drVoucherHeader["Voucher_Date"].ToString(),
                    drVoucherDeatil["account_no"].ToString(),
                    drVoucherDeatil["other_account_no"].ToString(),
                    invoiceAmount.ToString(),
                    paidAmount.ToString(),
                    dueAmount.ToString(),
                    drVoucherHeader["Cheque_Issue_Date"].ToString(),
                    drVoucherHeader["Cheque_Clear_Date"].ToString(), drVoucherHeader["Cheque_No"].ToString(),
                    drVoucherHeader["Narration"].ToString(),
                    empID.ToString(),
                    drVoucherDeatil["Currency_Id"].ToString(),
                    drVoucherDeatil["Exchange_Rate"].ToString(),
                    drVoucherDeatil["Foreign_Amount"].ToString(),
                    drVoucherDeatil["CompanyCurrDebit"].ToString(),
                    drVoucherDeatil["CompanyCurrCredit"].ToString(),
                    drVoucherHeader["Finance_Code"].ToString(),
                    strType,
                    drVoucherHeader["Trans_Id"].ToString(), "", "", creditDays.ToString(), "", "", "True", DateTime.Now.ToString(), true.ToString(), userID.ToString(), DateTime.Now.ToString(), userID.ToString(), DateTime.Now.ToString(), ref trns);

                //Check for credit Note Adjustment
                if (strType == "RV")
                {
                    string sql = "select field3 as voucher_id,Pay_Charges, FCPayAmount from Inv_PaymentTrn where TypeTrans='SI' and TransNo='" + drVoucherHeader["Ref_id"].ToString() + "' and field3<>'' and field3<>'0'";
                    DataTable _dt = daClass.return_DataTable(sql, ref trns);
                    if (_dt.Rows.Count > 0)
                    {
                        DataTable _dtCreditNote = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, _dt.Rows[0]["voucher_id"].ToString(), ref trns);
                        DataRow _drCreditNote = _dtCreditNote.Rows[0];
                        string strNarration = string.Empty;
                        strNarration = _drCreditNote["Voucher_No"].ToString() + " Adjusted against Invoice-" + drVoucherHeader["voucher_no"].ToString();
                        InsertAgeingDetail(drVoucherHeader["Company_id"].ToString(),
                    drVoucherHeader["Brand_id"].ToString(),
                    drVoucherHeader["Location_id"].ToString(),
                    drVoucherHeader["Ref_Type"].ToString(),
                    drVoucherHeader["Ref_id"].ToString(),
                    drVoucherHeader["Inv_Number"].ToString(),
                    drVoucherHeader["Voucher_Date"].ToString(),
                    drVoucherDeatil["account_no"].ToString(),
                    drVoucherDeatil["other_account_no"].ToString(),
                    invoiceAmount.ToString(),
                    _dt.Rows[0]["Pay_Charges"].ToString(),
                    "0",
                    drVoucherHeader["Cheque_Issue_Date"].ToString(),
                    drVoucherHeader["Cheque_Clear_Date"].ToString(), drVoucherHeader["Cheque_No"].ToString(),
                    strNarration,
                    empID.ToString(),
                    drVoucherDeatil["Currency_Id"].ToString(),
                    drVoucherDeatil["Exchange_Rate"].ToString(),
                    _dt.Rows[0]["FCPayAmount"].ToString(),
                    drVoucherDeatil["CompanyCurrDebit"].ToString(),
                    drVoucherDeatil["CompanyCurrCredit"].ToString(),
                    drVoucherHeader["Finance_Code"].ToString(),
                    strType,
                    _drCreditNote["Trans_id"].ToString(), "", "", creditDays.ToString(), "", "", "True", DateTime.Now.ToString(), true.ToString(), userID.ToString(), DateTime.Now.ToString(), userID.ToString(), DateTime.Now.ToString(), ref trns);
                        _dtCreditNote.Dispose();
                    }
                    _dt.Dispose();
                }
                //--------code end-------

                //Advance(against order) adjustment - 10/09/2018 - Neelkanth Purohit
                DataTable _dtPendingCreditVoucer = new DataTable();
                try
                {

                    if (strType == "RV")
                    {
                        _dtPendingCreditVoucer = GetUnAdjustedCustomerCreditVoucher(strLocationId, drVoucherDeatil["Currency_Id"].ToString(), drVoucherDeatil["account_no"].ToString(), drVoucherDeatil["other_account_no"].ToString(), "ALL", ref trns);
                    }
                    else
                    {
                        _dtPendingCreditVoucer = GetUnAdjustedSupplierDebitVoucher(strLocationId, drVoucherDeatil["Currency_Id"].ToString(), drVoucherDeatil["account_no"].ToString(), drVoucherDeatil["other_account_no"].ToString(), "ALL", ref trns);
                    }
                }
                catch (Exception ex)
                {
                    _dtPendingCreditVoucer = null;
                }

                if (_dtPendingCreditVoucer != null && _dtPendingCreditVoucer.Rows.Count > 0)
                {
                    string sql = string.Empty;
                    string strOrderIds = string.Empty;
                    if (strType == "RV")
                    {
                        sql = "SELECT STUFF((SELECT ',' + cast(SIFromTransNo as varchar) from inv_salesinvoicedetail where SIFromTransType='S' and SIFromTransNo>0 and invoice_no='" + drVoucherHeader["Ref_id"].ToString() + "' FOR XML PATH('') ), 1, 1, '')";
                    }
                    else
                    {
                        sql = "SELECT STUFF((SELECT ',' + cast(POId as varchar) from Inv_PurchaseInvoiceDetail where POId>0 and InvoiceNo='" + drVoucherHeader["Ref_id"].ToString() + "' FOR XML PATH('') ), 1, 1, '')";
                    }
                    strOrderIds = daClass.get_SingleValue(sql);
                    if (!string.IsNullOrEmpty(strOrderIds))
                    {
                        _dtPendingCreditVoucer = new DataView(_dtPendingCreditVoucer, "ref_id in (" + strOrderIds + ")", "", DataViewRowState.CurrentRows).ToTable();
                        if (_dtPendingCreditVoucer.Rows.Count > 0)
                        {
                            string strNarration = string.Empty;
                            strNarration = _dtPendingCreditVoucer.Rows[0]["Voucher_No"].ToString() + " Adjusted against Invoice-" + drVoucherHeader["voucher_no"].ToString();
                            double localBalanceAmt = 0;
                            double.TryParse(_dtPendingCreditVoucer.Rows[0]["l_balance_amount"].ToString(), out localBalanceAmt);

                            double fBalanceAmt = 0;
                            double.TryParse(_dtPendingCreditVoucer.Rows[0]["f_balance_amount"].ToString(), out fBalanceAmt);

                            double fInvoiceAmt = 0;
                            double.TryParse(drVoucherDeatil["Foreign_Amount"].ToString(), out fInvoiceAmt);
                            if (fBalanceAmt > fInvoiceAmt)
                            {
                                fBalanceAmt = fInvoiceAmt;
                                localBalanceAmt = fBalanceAmt * double.Parse(_dtPendingCreditVoucer.Rows[0]["exchange_rate"].ToString());
                                //set decimal as per currency
                                localBalanceAmt = double.Parse(objsys.GetCurencyConversionForInv(_dtPendingCreditVoucer.Rows[0]["currency_id"].ToString(), localBalanceAmt.ToString()));
                            }

                            InsertAgeingDetail(drVoucherHeader["Company_id"].ToString(),
                            drVoucherHeader["Brand_id"].ToString(),
                            drVoucherHeader["Location_id"].ToString(),
                            drVoucherHeader["Ref_Type"].ToString(),
                            drVoucherHeader["Ref_id"].ToString(),
                            drVoucherHeader["Inv_Number"].ToString(),
                            drVoucherHeader["Voucher_Date"].ToString(),
                            drVoucherDeatil["account_no"].ToString(),
                            drVoucherDeatil["other_account_no"].ToString(),
                            invoiceAmount.ToString(),
                            localBalanceAmt.ToString(),
                            "0",
                            drVoucherHeader["Cheque_Issue_Date"].ToString(),
                            drVoucherHeader["Cheque_Clear_Date"].ToString(), drVoucherHeader["Cheque_No"].ToString(),
                            strNarration,
                            empID.ToString(),
                            drVoucherDeatil["Currency_Id"].ToString(),
                            _dtPendingCreditVoucer.Rows[0]["Exchange_Rate"].ToString(),
                            fBalanceAmt.ToString(),
                            "0",
                            "0",
                            drVoucherHeader["Finance_Code"].ToString(),
                            strType,
                            _dtPendingCreditVoucer.Rows[0]["voucher_id"].ToString(), "", "", creditDays.ToString(), "", "", "True", DateTime.Now.ToString(), true.ToString(), userID.ToString(), DateTime.Now.ToString(), userID.ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                //-------------------end--------------------------

            }
        }
    }

    public bool checkAgeingConsistency_and_Insert(string StrCompId, string StrBrandId, string strLocationId, string strVoucherId, ref SqlTransaction trns, string strUserId, string strEmpId)
    {
        DataTable dtAgeing = new DataTable();
        bool _result = true;
        string strCompCurrencyId = "0";
        try
        {
            strCompCurrencyId = objCompanyMaster.GetCompanyMasterById(StrCompId, ref trns).Rows[0]["Currency_Id"].ToString();
        }
        catch (Exception ex)
        {

        }
        if (objAccParameter.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Auto_Ageing_Settlement", ref trns).Rows.Count > 0)
        {
            if (objAccParameter.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, strLocationId, "Auto_Ageing_Settlement", ref trns).Rows[0]["Param_Value"].ToString() == "True")
            {
                DataTable drVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, strVoucherId, ref trns);
                //if voucher is not reconciled then no need to any ageing adjustment
                if (bool.Parse(drVoucherHeader.Rows[0]["ReconciledFromFinance"].ToString()) == false)
                {
                    return false;
                }
                string strType = string.Empty;
                double tenderReceivedAmount = 0;
                string sql = string.Empty;




                //sql = "select other_account_no,currency_id,exchange_rate, account_no, case when  account_no='" + Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, ref trns) + "'  then  sum(credit_amount-debit_amount) else sum(debit_amount-credit_amount) end as LAmount, case when  account_no='" + Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, ref trns) + "'  then  sum((case when Credit_amount>0 then foreign_amount else 0 end) - (case when Debit_amount>0 then foreign_amount else 0 end))    else sum((case when debit_amount>0 then foreign_amount else 0 end) - (case when credit_amount>0 then foreign_amount else 0 end)) end as fAmount from ac_voucher_detail where Voucher_No='" + strVoucherId + "'  and other_account_no >0 group by other_account_no,account_no,currency_id,exchange_rate";
                sql = "select other_account_no,currency_id,exchange_rate, account_no, case when  account_no='" + objAcParaMaster.GetCustomerAccountNo1(StrCompId, ref trns).ToString() + "'  then  sum(credit_amount-debit_amount) else sum(debit_amount-credit_amount) end as LAmount, case when  account_no='" + objAcParaMaster.GetCustomerAccountNo1(StrCompId, ref trns).ToString() + "'  then  sum((case when Credit_amount>0 then foreign_amount else 0 end) - (case when Debit_amount>0 then foreign_amount else 0 end))    else sum((case when debit_amount>0 then foreign_amount else 0 end) - (case when credit_amount>0 then foreign_amount else 0 end)) end as fAmount from ac_voucher_detail where Voucher_No='" + strVoucherId + "'  and other_account_no >0 group by other_account_no,account_no,currency_id,exchange_rate";
                DataTable dtVoucherDetail = daClass.return_DataTable(sql, ref trns);
                if (dtVoucherDetail.Rows.Count == 0)
                {
                    return false;
                }

                if (double.Parse(dtVoucherDetail.Rows[0]["LAmount"].ToString()) < 0 || double.Parse(dtVoucherDetail.Rows[0]["fAmount"].ToString()) < 0)
                {
                    return false;
                }

                string strLocationCurrency = "";
                strLocationCurrency = ObjLocation.Get_Currency_By_Location_ID(StrCompId, strLocationId).Rows[0]["Currency_id"].ToString();
                //if (dtVoucherDetail.Rows[0]["account_no"].ToString() == Ac_ParameterMaster.GetCustomerAccountNo(StrCompId, ref trns))
                if (dtVoucherDetail.Rows[0]["account_no"].ToString() == objAcParaMaster.GetCustomerAccountNo1(StrCompId, ref trns))
                {
                    strType = "RV";
                }
                else if (dtVoucherDetail.Rows[0]["account_no"].ToString() == objAcParaMaster.GetSupplierAccountNo1(StrCompId, ref trns))
                //For Ageing Detail Insert
                {

                    strType = "PV";

                }
                else
                {
                    return false;
                }


                //DataTable dtAgeing = GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, strVoucherId);
                dtAgeing = daClass.return_DataTable("select * from ac_ageing_detail where company_id='" + StrCompId + "' and brand_id='" + StrBrandId + "' and location_id='" + strLocationId + "' and voucherid='" + strVoucherId + "' and IsActive='False'", ref trns);
                if (dtAgeing.Rows.Count > 0)
                {
                    //string sql = " select other_account_no,currency_id,exchange_rate, sum(debit_amount-credit_amount) as LAmount, sum((case when debit_amount>0 then foreing_amount else 0 end) - (case when credit_amount>0 then foreing_amount else 0 end)) as fAmount form ac_voucher_detail where voucher_id='" + strVoucherId + "' group by other_account_no,currency_id,exchange_rate";

                    if (dtVoucherDetail.Rows.Count > 0)
                    {

                        DataTable dtPendingAgeing = getPendingAgeingTable(StrCompId, StrBrandId, strLocationId, "PV", dtVoucherDetail.Rows[0]["other_account_no"].ToString(), "", dtAgeing.Rows[0]["Invoice_no"].ToString(), ref trns);
                        if (dtPendingAgeing.Rows.Count > 0)
                        {

                            if (dtPendingAgeing.Rows[0]["currency_id"].ToString() == strLocationCurrency)
                            {
                                tenderReceivedAmount = double.Parse(dtVoucherDetail.Rows[0]["LAmount"].ToString());
                            }
                            else
                            {
                                tenderReceivedAmount = double.Parse(dtVoucherDetail.Rows[0]["FAmount"].ToString());
                            }
                            if (tenderReceivedAmount > double.Parse(dtPendingAgeing.Rows[0]["actual_balance_amt"].ToString()))
                            {
                                _result = false;
                            }
                            else
                            {
                                daClass.execute_Command("update ac_ageing_detail set isActive='true' where company_id='" + StrCompId + "' and brand_id='" + StrBrandId + "' and location_id='" + strLocationId + "' and voucherId='" + strVoucherId + "'", ref trns);
                            }
                        }

                        else
                        {
                            _result = false;
                        }
                        dtPendingAgeing.Dispose();
                    }
                    dtVoucherDetail.Dispose();

                }
                else
                {

                    DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, strVoucherId, ref trns);


                    dtAgeing = getPendingAgeingTable(StrCompId, StrBrandId, strLocationId, strType, dtVoucherDetail.Rows[0]["other_account_no"].ToString(), "", "", ref trns);


                    string sqlCreditDays = string.Empty;


                    //here we are getting credit days for customer and supplier

                    bool IsActiveStatus = true;

                    string strNarration = string.Empty;

                    double ExchnageRate = 0;

                    double LocalAmount = 0;
                    double ForeignAmount = 0;
                    double CompanyAmount = 0;
                    double AdjustmentAmount = 0;
                    double Payamount = 0;
                    double.TryParse(dtVoucherDetail.Rows[0]["Exchange_rate"].ToString(), out ExchnageRate);


                    if (drVoucherHeader.Rows[0]["Field3"].ToString().Trim() == "Pending")
                    {
                        IsActiveStatus = false;
                    }




                    if (dtVoucherDetail.Rows[0]["Currency_Id"].ToString() == ObjLocation.Get_Currency_By_Location_ID(StrCompId, strLocationId).Rows[0]["Currency_id"].ToString())
                    {

                        AdjustmentAmount = double.Parse(dtVoucherDetail.Rows[0]["LAmount"].ToString());
                    }
                    else
                    {
                        AdjustmentAmount = double.Parse(dtVoucherDetail.Rows[0]["FAmount"].ToString());
                    }

                    foreach (DataRow dr in dtAgeing.Rows)
                    {


                        if (AdjustmentAmount == 0)
                        {
                            break;
                        }



                        if (dr["currency_id"].ToString() == dtVoucherDetail.Rows[0]["Currency_Id"].ToString())
                        {




                            if (AdjustmentAmount <= double.Parse(dr["actual_balance_amt"].ToString()))
                            {
                                Payamount = AdjustmentAmount;
                            }
                            else
                            {
                                Payamount = double.Parse(dr["actual_balance_amt"].ToString());
                            }



                            strNarration = "Paid Amount for That Invoices : " + dr["Invoice_No"].ToString();


                            //for local
                            if (ExchnageRate == 1 || ExchnageRate == 0)
                            {
                                LocalAmount = ForeignAmount = Payamount;


                            }
                            else
                            {
                                ForeignAmount = Payamount;

                                LocalAmount = ForeignAmount * ExchnageRate;
                            }


                            string strCurrency = ObjLocation.GetLocationMasterById(StrCompId, strLocationId).Rows[0]["Field1"].ToString();

                            //CurrencyMaster ObjCurrency = new CurrencyMaster(trns.Connection.ConnectionString);
                            string strExchangeRate = string.Empty;
                            //string strFromCurrency, string strToCurrency, ref SqlTransaction trns

                            if (objsys.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString() == strCurrency)
                            {
                                strExchangeRate = ObjCurrency.GetCurrencyMasterById(strCompCurrencyId, ref trns).Rows[0]["Currency_Value"].ToString();
                                //strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, trns.Connection.ConnectionString).Rows[0]["Currency_Value"].ToString();
                            }
                            else
                            {
                                strExchangeRate = ((1 / float.Parse(ObjCurrency.GetCurrencyMasterById(strCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(ObjCurrency.GetCurrencyMasterById(strCompCurrencyId, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();

                            }



                            // string  strExchangeRate = objsys.GetExchageRate1(strCurrency, strCompCurrencyId, ref trns );


                            Double d = 0;
                            try
                            {
                                d = Convert.ToDouble(strExchangeRate);
                            }
                            catch
                            {

                            }
                            string strCompanyAmount = objsys.GetCurrency1(strCompCurrencyId, LocalAmount.ToString(), ref trns, StrCompId, strLocationId, d);
                            //  double.TryParse(SystemParameter.GetCurrency(strCompCurrencyId, LocalAmount.ToString(), trns.Connection.ConnectionString,StrCompId,strLocationId), out CompanyAmount);

                            AdjustmentAmount = AdjustmentAmount - Payamount;


                            InsertAgeingDetail(drVoucherHeader.Rows[0]["Company_id"].ToString(),
                            drVoucherHeader.Rows[0]["Brand_id"].ToString(),
                            drVoucherHeader.Rows[0]["Location_id"].ToString(),
                            dr["Ref_Type"].ToString(),
                            dr["Ref_id"].ToString(),
                            dr["Invoice_No"].ToString(),
                            dr["Invoice_Date"].ToString(),
                            dtVoucherDetail.Rows[0]["account_no"].ToString(),
                            dtVoucherDetail.Rows[0]["other_account_no"].ToString(),
                            "0",
                            LocalAmount.ToString(),
                            "0",
                            drVoucherHeader.Rows[0]["Cheque_Issue_Date"].ToString(),
                           drVoucherHeader.Rows[0]["Cheque_Clear_Date"].ToString(), drVoucherHeader.Rows[0]["Cheque_No"].ToString(),
                            drVoucherHeader.Rows[0]["Narration"].ToString(),
                           strEmpId,
                            dtVoucherDetail.Rows[0]["Currency_Id"].ToString(),
                            dtVoucherDetail.Rows[0]["Exchange_Rate"].ToString(),
                            ForeignAmount.ToString(),
                            CompanyAmount.ToString(),
                            "0",
                            drVoucherHeader.Rows[0]["Finance_Code"].ToString(),
                            strType,
                            drVoucherHeader.Rows[0]["Trans_Id"].ToString(), "", "", "0", "", "", "True", DateTime.Now.ToString(), IsActiveStatus.ToString(), strUserId, DateTime.Now.ToString(), strUserId, DateTime.Now.ToString(), ref trns);
                        }
                    }

                }
            }
        }
        dtAgeing.Dispose();
        return _result;
    }

    public DataTable getPendingAgeingTable(string strCompanyId, string strBrandId, string strLocationId, string strAgeingType, string strOtherAccountNo, string strVoucherId, string strInvoiceNo, bool IsAllInvoices = false)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@AgeingType", strAgeingType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OtherAccountNo", strOtherAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@InvoiceNo", strInvoiceNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsAgeingReport", IsAllInvoices.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_Ac_InvoiceAgeingReport", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable getPendingAgeingTable(string strCompanyId, string strBrandId, string strLocationId, string strAgeingType, string strOtherAccountNo, string strVoucherId, string strInvoiceNo, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@AgeingType", strAgeingType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OtherAccountNo", strOtherAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@InvoiceNo", strInvoiceNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsAgeingReport", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_Ac_InvoiceAgeingReport", paramList, ref trns))
        {
            return dtInfo;
        }
    }

    public DataTable getPaidAgeingTable(string strCompanyId, string strBrandId, string strLocationId, string strAgeingType, string strOtherAccountNo, string strVoucherId, string strInvoiceNo)
    {

        string strAgeing = string.Empty;

        if (strAgeingType == "PV")
        {
            strAgeing = "S";
        }
        else
        {
            strAgeing = "C";
        }

        double _voucherId = 0;
        double.TryParse(strVoucherId, out _voucherId);
        string editSql = "";
        if (_voucherId > 0)
        {
            editSql = "and voucherId<>" + _voucherId + "";
        }

        if (strInvoiceNo != "")
        {
            editSql = editSql + " and Invoice_No='" + strInvoiceNo + "'";
        }

        string sql = "select Pending_Invoice.*,Sys_CurrencyMaster.Currency_Name," +
          " f_Invoice_Amount as actual_Invoice_amt ,   F_Receive_Amount as actual_Receive_amt , " +
" F_balance_Amount as actual_balance_amt" +
          " from (" +
" select AgeingType,Account_No,Other_Account_No,Max(  case when Invoice_Amount>0 then Exchange_rate else 0 end    ) as Exchange_Rate,MAX( case when Invoice_Amount>0 then Currency_Id else 0 end ) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount," +
"sum(due_amount-paid_Receive_Amount) as L_Balance_Amount, " +
" MAX(case when Invoice_Amount>0 then Foreign_Amount else 0 end) as f_Invoice_Amount," +
" sum(case when paid_Receive_Amount>0 then foreign_amount else 0 end) as F_Receive_Amount, " +
" sum(case when due_amount>0 then foreign_amount else 0 end) as F_due_Amount," +
" sum((case when due_amount>0 then foreign_amount else 0 end)- (case when paid_Receive_Amount>0 then foreign_amount else 0 end)) as F_balance_Amount," +
" sum(due_amount) as Due_Amount,Ref_Type,Ref_Id,Company_Id,Brand_Id, Location_Id, IsActive ," +
"  DATEADD(DAY,CAST( max(ac_ageing_detail.field3) as int), ac_ageing_detail.Invoice_Date) AS paymentDate,  ( DATEDIFF(DAY, DATEADD(DAY, CAST(MAX(ac_ageing_detail.field3) AS int), ac_ageing_detail.Invoice_Date), GETDATE())) AS Due_Days " +
"from ac_ageing_detail where other_account_no='" + strOtherAccountNo.ToString() + "' and AgeingType='" + strAgeingType + "' and IsActive='True' and " +
" Company_Id='" + strCompanyId.ToString() + "' and Brand_Id='" + strBrandId.ToString() + "' and Location_Id='" + strLocationId.ToString() + "' and Invoice_No='" + strInvoiceNo + "'" +
editSql +
" group by Company_Id,Brand_Id, Location_Id,AgeingType, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Account_No,Other_Account_No,AgeingType,IsActive )Pending_Invoice" +
" left join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Pending_Invoice.Currency_Id " +
" where F_Receive_Amount>0";

        using (DataTable dtInfo = daClass.return_DataTable(sql))
        {
            return dtInfo;
        }
    }




    public DataTable GetUnAdjustedCustomerCreditVoucher(string strLocationId, string strCurrencyId, string strAccountNo, string strOtherAccontNo, string strVoucherType)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_id", strCurrencyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Account_no", strAccountNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Other_account_no", strOtherAccontNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@voucher_type", strVoucherType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_GetUnAdjustedCreditVoucher", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetUnAdjustedCustomerCreditVoucher(string strLocationId, string strCurrencyId, string strAccountNo, string strOtherAccontNo, string strVoucherType, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_id", strCurrencyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Account_no", strAccountNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Other_account_no", strOtherAccontNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@voucher_type", strVoucherType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_GetUnAdjustedCreditVoucher", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetUnAdjustedSupplierDebitVoucher(string strLocationId, string strCurrencyId, string strAccountNo, string strOtherAccontNo, string strVoucherType)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_id", strCurrencyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Account_no", strAccountNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Other_account_no", strOtherAccontNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@voucher_type", strVoucherType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_GetUnAdjustedDebitVoucher", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetUnAdjustedSupplierDebitVoucher(string strLocationId, string strCurrencyId, string strAccountNo, string strOtherAccontNo, string strVoucherType, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_id", strCurrencyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Account_no", strAccountNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Other_account_no", strOtherAccontNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@voucher_type", strVoucherType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Ageing_GetUnAdjustedDebitVoucher", paramList, ref trns);
        return dtInfo;
    }
    public class clsAgeingDaysDetail
    {
        public string amtCurrent { get; set; }
        public string perCurrent { get; set; }
        public string amt1to5Days { get; set; }
        public string per1to5Days { get; set; }
        public string amt6to30Days { get; set; }
        public string per6to30Days { get; set; }
        public string amtOver30Days { get; set; }
        public string perOver30Days { get; set; }
        public string amtTotalPastDue { get; set; }
        public string perTotalPastDue { get; set; }
        public string amtTotalBalance { get; set; }
        public string perTotalBalance { get; set; }
    }

    public clsAgeingDaysDetail getAgingDayDetail(DataTable dt, int decimalCount)
    {
        clsAgeingDaysDetail clsAging = new clsAgeingDaysDetail();
        if (dt.Rows.Count == 0)
        {
            clsAging.amtCurrent = "";
            clsAging.perCurrent = "";
            clsAging.amt1to5Days = "";
            clsAging.per1to5Days = "";
            clsAging.amt6to30Days = "";
            clsAging.per6to30Days = "";
            clsAging.amtOver30Days = "";
            clsAging.perOver30Days = "";
            clsAging.amtTotalPastDue = "";
            clsAging.perTotalPastDue = "";
            clsAging.amtTotalBalance = "";
            clsAging.perTotalBalance = "";
            return clsAging;
        }

        var myLinqQuery = dt.AsEnumerable()
                          .GroupBy(g => 1)
                         .Select(g => new
                         {
                             amtCurrent = g.Where(i => i.Field<int>("Due_Days") <= 0).Sum(i => i.Field<decimal>("actual_balance_amt")),
                             amt1to5 = g.Where(i => i.Field<int>("Due_Days") >= 1 && i.Field<int>("Due_Days") <= 5).Sum(i => i.Field<decimal>("actual_balance_amt")),
                             amt6to30 = g.Where(i => i.Field<int>("Due_Days") >= 6 && i.Field<int>("Due_Days") <= 30).Sum(i => i.Field<decimal>("actual_balance_amt")),
                             amtOver30 = g.Where(i => i.Field<int>("Due_Days") >= 31).Sum(i => i.Field<decimal>("actual_balance_amt"))
                         }).ToList();
        double amtCurrent = 0;
        double perCurrent = 0;
        double amt1to5Days = 0;
        double per1to5Days = 0;
        double amt6to30Days = 0;
        double per6to30Days = 0;
        double amtOver30Days = 0;
        double perOver30Days = 0;
        double amtTotalPastDue = 0;
        double perTotalPastDue = 0;
        double amtTotalBalance = 0;

        foreach (var item in myLinqQuery)
        {
            double.TryParse(item.amtCurrent.ToString(), out amtCurrent);
            double.TryParse(item.amt1to5.ToString(), out amt1to5Days);
            double.TryParse(item.amt6to30.ToString(), out amt6to30Days);
            double.TryParse(item.amtOver30.ToString(), out amtOver30Days);
        }

        amtTotalPastDue = amt1to5Days + amt6to30Days + amtOver30Days;
        amtTotalBalance = amtCurrent + amtTotalPastDue;
        double.TryParse(((amtCurrent * 100) / amtTotalBalance).ToString(), out perCurrent);
        double.TryParse(((amt1to5Days * 100) / amtTotalBalance).ToString(), out per1to5Days);
        double.TryParse(((amt6to30Days * 100) / amtTotalBalance).ToString(), out per6to30Days);
        double.TryParse(((amtOver30Days * 100) / amtTotalBalance).ToString(), out perOver30Days);
        double.TryParse(((amtTotalPastDue * 100) / amtTotalBalance).ToString(), out perTotalPastDue);

        clsAging.amtCurrent = SystemParameter.GetAmountWithDecimal(amtCurrent.ToString(), decimalCount.ToString());
        clsAging.perCurrent = perCurrent.ToString("0.00") + "%";
        clsAging.amt1to5Days = SystemParameter.GetAmountWithDecimal(amt1to5Days.ToString(), decimalCount.ToString());
        clsAging.per1to5Days = per1to5Days.ToString("0.00") + "%";
        clsAging.amt6to30Days = SystemParameter.GetAmountWithDecimal(amt6to30Days.ToString(), decimalCount.ToString());
        clsAging.per6to30Days = per6to30Days.ToString("0.00") + "%";
        clsAging.amtOver30Days = SystemParameter.GetAmountWithDecimal(amtOver30Days.ToString(), decimalCount.ToString());
        clsAging.perOver30Days = perOver30Days.ToString("0.00") + "%";
        clsAging.amtTotalPastDue = SystemParameter.GetAmountWithDecimal(amtTotalPastDue.ToString(), decimalCount.ToString());
        clsAging.perTotalPastDue = perTotalPastDue.ToString("0.00") + "%";
        clsAging.amtTotalBalance = SystemParameter.GetAmountWithDecimal(amtTotalBalance.ToString(), decimalCount.ToString());
        clsAging.perTotalBalance = "100%";
        return clsAging;
    }

}