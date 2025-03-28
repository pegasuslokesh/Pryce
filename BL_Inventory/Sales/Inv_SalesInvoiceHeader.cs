using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for Inv_SalesInvoiceHeader
/// </summary>
public class Inv_SalesInvoiceHeader
{
    DataAccessClass daClass = null;
    Common cm = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    private string _strConString = string.Empty;

    public Inv_SalesInvoiceHeader(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        cm = new Common(strConString);
        objSInvDetail = new Inv_SalesInvoiceDetail(strConString);
        objInvParam = new Inv_ParameterMaster(strConString);
        ObjStockBatchMaster = new Inv_StockBatchMaster(strConString);
        _strConString = strConString;
    }
    public int InsertSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strInvoice_No, string strInvoice_Date, string strPaymentModeId, string strCurrency_Id, string strSIFromTransType, string strSIFromTransNo, string strSalesPerson_Id, string strPosNo, string strRemark, string strAccount_No, string strInvoice_Costing, string strShift, string strPost, string strTender, string strAmount, string strTotalQuantity, string strTotalAmount, string strNetTaxP, string strNetTaxV, string strNetAmount, string strNetDiscountP, string strNetDiscountV, string strGrandTotal, string strSupplier_Id, string Invoice_Ref_No, string Invoice_Merchant_Id, string Ref_Order_Number, string strCondition1, string strCondition2, string strCondition3, string strCondition4, string strCondition5, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string Trans_Type, string contactId)
    {
        PassDataToSql[] paramList = new PassDataToSql[49];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_Date", strInvoice_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@PaymentModeId", strPaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@SIFromTransType", strSIFromTransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@SIFromTransNo", strSIFromTransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@SalesPerson_Id", strSalesPerson_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@PosNo", strPosNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Remark", strRemark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Invoice_Costing", strInvoice_Costing, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Shift", strShift, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Post", strPost, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Tender", strTender, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@TotalQuantity", strTotalQuantity, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@TotalAmount", strTotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@NetTaxP", strNetTaxP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@NetTaxV", strNetTaxV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@NetAmount", strNetAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@NetDiscountP", strNetDiscountP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@NetDiscountV", strNetDiscountV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@GrandTotal", strGrandTotal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Invoice_Ref_No", Invoice_Ref_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Invoice_Merchant_Id", Invoice_Merchant_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Ref_Order_Number", Ref_Order_Number, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Condition1", strCondition1, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@Condition2", strCondition2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Condition3", strCondition3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Condition4", strCondition4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Condition5", strCondition5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[46] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Trans_Type", Trans_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@contactId", contactId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_Insert", paramList);
        return Convert.ToInt32(paramList[45].ParaValue);
    }

    public void InsertSInvHeader_Extra(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strInvoice_No, string strShipment_Id, string strShipment_Contact, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Shipment_Id", strShipment_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Shipment_Contact", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        //paramList[6] = new PassDataToSql("@Payment_Reference_Oano", strPayment_Reference_Oano, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //paramList[7] = new PassDataToSql("@Payment_Reference_1", strPayment_Reference_1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //paramList[8] = new PassDataToSql("@Payment_Reference_2", strPayment_Reference_2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //paramList[9] = new PassDataToSql("@Payment_Reference_3", strPayment_Reference_3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_Extra_Insert", paramList, ref trns);
    }


    //Commented Code on 18-12-2023 By Lokesh as we have created a New SP for Return Report
    //public DataTable GetReturnReportData(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTransId, string IFromDate, string IToDate, string RFromDate, string RToDate)
    //{
    //    DataTable dtInfo = new DataTable();
    //    PassDataToSql[] paramList = new PassDataToSql[8];
    //    paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
    //    paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
    //    paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
    //    paramList[3] = new PassDataToSql("@TransId", strTransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
    //    paramList[4] = new PassDataToSql("@Invoice_From_Date", IFromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
    //    paramList[5] = new PassDataToSql("@Invoice_To_Date", IToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
    //    paramList[6] = new PassDataToSql("@Return_From_Date", RFromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
    //    paramList[7] = new PassDataToSql("@Return_To_Date", RToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

    //    dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceDetail_SelectRow_Report_Update", paramList);
    //    return dtInfo;
    //}

    public DataTable GetReturnReportData(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTransId, string IFromDate, string IToDate, string RFromDate, string RToDate, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[9];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TransId", strTransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_From_Date", IFromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_To_Date", IToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Return_From_Date", RFromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Return_To_Date", RToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesReturnDetail_SelectRow_Report", paramList);
        return dtInfo;
    }


    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public int InsertSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strInvoice_No, string strInvoice_Date, string strPaymentModeId, string strCurrency_Id, string strSIFromTransType, string strSIFromTransNo, string strSalesPerson_Id, string strPosNo, string strRemark, string strAccount_No, string strInvoice_Costing, string strShift, string strPost, string strTender, string strAmount, string strTotalQuantity, string strTotalAmount, string strNetTaxP, string strNetTaxV, string strNetAmount, string strNetDiscountP, string strNetDiscountV, string strGrandTotal, string strSupplier_Id, string Invoice_Ref_No, string Invoice_Merchant_Id, string Ref_Order_Number, string strCondition1, string strCondition2, string strCondition3, string strCondition4, string strCondition5, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string Trans_Type, string contactId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[49];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_Date", strInvoice_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@PaymentModeId", strPaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@SIFromTransType", strSIFromTransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@SIFromTransNo", strSIFromTransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@SalesPerson_Id", strSalesPerson_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@PosNo", strPosNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Remark", strRemark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Invoice_Costing", strInvoice_Costing, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Shift", strShift, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Post", strPost, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Tender", strTender, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@TotalQuantity", strTotalQuantity, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@TotalAmount", strTotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@NetTaxP", strNetTaxP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@NetTaxV", strNetTaxV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@NetAmount", strNetAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@NetDiscountP", strNetDiscountP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@NetDiscountV", strNetDiscountV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@GrandTotal", strGrandTotal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Invoice_Ref_No", Invoice_Ref_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Invoice_Merchant_Id", Invoice_Merchant_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Ref_Order_Number", Ref_Order_Number, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Condition1", strCondition1, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@Condition2", strCondition2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Condition3", strCondition3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Condition4", strCondition4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Condition5", strCondition5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[46] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Trans_Type", Trans_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@contactId", contactId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[45].ParaValue);
    }

    public void Updatecode(string Id, string Code)
    {
        cm.UpdateCodeForDocumentNo("Inv_SalesInvoiceHeader", "Invoice_No", "Trans_Id", Id, Code);
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public void Updatecode(string Id, string Code, ref SqlTransaction trns)
    {
        cm.UpdateCodeForDocumentNo("Inv_SalesInvoiceHeader", "Invoice_No", "Trans_Id", Id, Code, ref trns);
    }


    public void SalesReturnUpdatecode(string Id, string Code, ref SqlTransaction trns)
    {
        cm.UpdateCodeForDocumentNo("Inv_SalesInvoiceHeader", "ReturnNo", "Trans_Id", Id, Code, ref trns);
    }
    public int UpdateSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string strInvoice_No, string strInvoice_Date, string strPaymentModeId, string strCurrency_Id, string strSIFromTransType, string strSIFromTransNo, string strSalesPerson_Id, string strPosNo, string strRemark, string strAccount_No, string strInvoice_Costing, string strShift, string strPost, string strTender, string strAmount, string strTotalQuantity, string strTotalAmount, string strNetTaxP, string strNetTaxV, string strNetAmount, string strNetDiscountP, string strNetDiscountV, string strGrandTotal, string strSupplier_Id, string Invoice_Ref_No, string Invoice_Merchant_Id, string Ref_Order_Number, string strCondition1, string strCondition2, string strCondition3, string strCondition4, string strCondition5, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate, string Trans_Type, string contactId)
    {
        PassDataToSql[] paramList = new PassDataToSql[48];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_Date", strInvoice_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@PaymentModeId", strPaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@SIFromTransType", strSIFromTransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@SIFromTransNo", strSIFromTransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@SalesPerson_Id", strSalesPerson_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@PosNo", strPosNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Remark", strRemark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Invoice_Costing", strInvoice_Costing, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Shift", strShift, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Post", strPost, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Tender", strTender, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@TotalQuantity", strTotalQuantity, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@TotalAmount", strTotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@NetTaxP", strNetTaxP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@NetTaxV", strNetTaxV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@NetAmount", strNetAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@NetDiscountP", strNetDiscountP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@NetDiscountV", strNetDiscountV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@GrandTotal", strGrandTotal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Invoice_Ref_No", Invoice_Ref_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Invoice_Merchant_Id", Invoice_Merchant_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Ref_Order_Number", Ref_Order_Number, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[31] = new PassDataToSql("@Condition1", strCondition1, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Condition2", strCondition2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Condition3", strCondition3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Condition4", strCondition4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Condition5", strCondition5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[45] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Trans_Type", Trans_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@contactId", contactId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_Update", paramList);
        return Convert.ToInt32(paramList[44].ParaValue);
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public int UpdateSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string strInvoice_No, string strInvoice_Date, string strPaymentModeId, string strCurrency_Id, string strSIFromTransType, string strSIFromTransNo, string strSalesPerson_Id, string strPosNo, string strRemark, string strAccount_No, string strInvoice_Costing, string strShift, string strPost, string strTender, string strAmount, string strTotalQuantity, string strTotalAmount, string strNetTaxP, string strNetTaxV, string strNetAmount, string strNetDiscountP, string strNetDiscountV, string strGrandTotal, string strSupplier_Id, string Invoice_Ref_No, string Invoice_Merchant_Id, string Ref_Order_Number, string strCondition1, string strCondition2, string strCondition3, string strCondition4, string strCondition5, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate, string Trans_Type, string contactId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[48];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Invoice_Date", strInvoice_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@PaymentModeId", strPaymentModeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Currency_Id", strCurrency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@SIFromTransType", strSIFromTransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@SIFromTransNo", strSIFromTransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@SalesPerson_Id", strSalesPerson_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@PosNo", strPosNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Remark", strRemark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Account_No", strAccount_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Invoice_Costing", strInvoice_Costing, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Shift", strShift, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Post", strPost, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Tender", strTender, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@TotalQuantity", strTotalQuantity, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@TotalAmount", strTotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@NetTaxP", strNetTaxP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@NetTaxV", strNetTaxV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@NetAmount", strNetAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@NetDiscountP", strNetDiscountP, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@NetDiscountV", strNetDiscountV, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@GrandTotal", strGrandTotal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Invoice_Ref_No", Invoice_Ref_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Invoice_Merchant_Id", Invoice_Merchant_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@Ref_Order_Number", Ref_Order_Number, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[31] = new PassDataToSql("@Condition1", strCondition1, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Condition2", strCondition2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Condition3", strCondition3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Condition4", strCondition4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Condition5", strCondition5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[45] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Trans_Type", Trans_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@contactId", contactId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[44].ParaValue);
    }

    public int DeleteSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_RowStatus", paramList);
        return Convert.ToInt32(paramList[7].ParaValue);
    }

    public DataTable GetSInvHeaderAll(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay


    public DataTable GetSInvHeaderAll(string strCompany_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllTrue(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllTrueByIndex(string strCompany_Id, string strBrand_Id, string strLocation_Id, string WhereClauses, string TotalCount, string BatchNo, string PageSize, string SearchField, string SearchType, string SearchValue)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@WhereClauses", WhereClauses, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@TotalCount", TotalCount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@BatchNo", BatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@PageSize", PageSize, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@SearchField", SearchField, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@SearchType", SearchType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@SearchValue", SearchValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRowByIndex", paramList);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllFalse(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllByTransId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetCommissionRecordByDate(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strType, string strFromDate = "", string strToDate = "")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", strType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@FromDate", strFromDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ToDate", strToDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow_date", paramList);
        return dtInfo;
    }


    public DataTable GetCommissionRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strType, string strFromDate = "", string strToDate = "")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", strType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@FromDate", strFromDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ToDate", strToDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetCommissionRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strType, string strTransId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", strType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "10", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }


    public DataTable GetCommissionRecordForAddManual(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", strType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "9", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }


    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetSInvHeaderAllByTransId(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllByInvoiceNo(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strInvoice_No)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }

    //created for rollback Transaction

    public DataTable GetSInvHeaderAllByInvoiceNo(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strInvoice_No, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", strInvoice_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetSInvHeaderAllByFromTransType(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strSIFromTransType, string strSIFromTransNo)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", strSIFromTransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", strSIFromTransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetMaxSalesInvoiceId(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);
        return dtInfo;
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetMaxSalesInvoiceId(string strCompany_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList, ref trns);
        return dtInfo;
    }



    public string GetAutoID(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);

        string strPattern = "SInv" + DateTime.Now.Day.ToString() + "" + DateTime.Now.Month.ToString() + "" + DateTime.Now.Year + "";

        DataTable dtTemp = new DataTable();
        dtTemp = new DataView(dtInfo, "Invoice_No Like '%" + strPattern + "%'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtTemp.Rows.Count > 0)
        {
            strPattern = strPattern + (dtTemp.Rows.Count + 1);
        }
        else
        {
            strPattern = strPattern + "1";
        }
        return strPattern;
    }
    public DataTable GetDistinctInvoiceNo(string strCompanyId, string strBrandId, string strLocationId, string strPrefixText)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Invoice_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@SIFromTransType", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@SIFromTransNo", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectRow", paramList);

        dtInfo = new DataView(dtInfo, "Post='True'", "", DataViewRowState.CurrentRows).ToTable();

        dtInfo = new DataView(dtInfo, "Invoice_No Like '%" + strPrefixText + "%'", "Invoice_No Asc", DataViewRowState.CurrentRows).ToTable();

        return dtInfo;
    }

    public DataTable FilterByInvoiceNo(string strLocationId, string strPrefixText)
    {
        return daClass.return_DataTable("select invoice_no from inv_salesinvoiceheader where location_id=" + strLocationId.Trim() + " and invoice_no like '%" + strPrefixText + "%' and IsActive='True' and Post='True'");
    }


    //updated for rollback Trsnaction
    //16-02-2016
    //by jitendra upadhyay

    public int UpdateReturnSInvHeader(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string strReturnNo, string strReturnDate, string strRemarks, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Return_No", strReturnNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Return_Date", strReturnDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[7] = new PassDataToSql("@Remarks", strRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_SalesInvoiceHeader_ReturnUpdate", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int UpdateSalesInvoiceApproval(string CompanyId, string brandId, string Locationid, string TransId, string Field4)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeaderApproval_Update", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public int UpdateSalesInvoiceApproval(string CompanyId, string brandId, string Locationid, string TransId, string Field4, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Inv_SalesInvoiceHeaderApproval_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public DataTable GetSInvHeaderAllTrueByCustomerID(string CustomerID)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Customer_Id", CustomerID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_SelectByCustomerID", paramList);
        return dtInfo;
    }
    public DataTable GetInvoiceList(string strLocationId, string recordType, string displayLength, string displayStart, string sortCol, string sortDir, string search)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@recordType", recordType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@displayLength", displayLength, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@displayStart", displayStart, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@sortCol", sortCol, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@sortDir", sortDir, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@search", search, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@OptType", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_inv_getSalesInvoiceList", paramList))
        {
            return dtInfo;
        }
    }
    public int GetInvoiceListCount(string strLocationId, string recordType)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@recordType", recordType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@displayLength", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@displayStart", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@sortCol", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@sortDir", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@search", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@OptType", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_inv_getSalesInvoiceList", paramList))
        {
            if (dtInfo.Rows.Count > 0)
            {
                return Convert.ToInt32(dtInfo.Rows[0][0].ToString());
            }
            else
            { return 0; }
        }
    }

    public DataTable getInvoiceListForSalesReturn(string strPageIndex, string strPageSize, string strSortExpr, string strSortDire, string strWhereClause)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@PageIndex", strPageIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@PageSize", strPageSize, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@SortExpr", strSortExpr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@SortDire", strSortDire, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@WhereClause", strWhereClause, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_SalesInvoiceHeader_By_PageIndex", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable FilterByInvoiceNo(string strCompanyId, string strBrandId, string strLocationId, string customerId, string strPrefixText)
    {
        return daClass.return_DataTable("select top 10 Invoice_No from inv_salesinvoiceheader where Company_Id='" + strCompanyId + "' and Brand_Id='" + strBrandId + "' and Location_Id ='" + strLocationId + "' and Supplier_Id='" + customerId + "' and invoice_no like '%" + strPrefixText + "%' and IsActive='True'");
    }
    public bool validateByInvoiceNo(string strCompanyId, string strBrandId, string strLocationId, string customerId, string strPrefixText)
    {
        string data = daClass.get_SingleValue("select count(Invoice_No) from inv_salesinvoiceheader where Company_Id='" + strCompanyId + "' and Brand_Id='" + strBrandId + "' and Location_Id ='" + strLocationId + "' and Supplier_Id='" + customerId + "' and invoice_no = '" + strPrefixText + "' and IsActive='True'");
        data = data == "@NOTFOUND@" ? "0" : data;
        if (data == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public class clsInvoiceList
    {
        public string salesInvoiceId { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string refType { get; set; }
        public string refNo { get; set; }
        public string salesPerson { get; set; }
        public string createdBy { get; set; }
        public string approvalStatus { get; set; }
        public string currencyCode { get; set; }
        public string customerName { get; set; }
        public string invoiceAmount { get; set; }
    }

    public class clsImportEComInvoiceList
    {
        public bool is_valid { get; set; }
        public string validation_remark { get; set; }
        public string merchant { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string billing_address { get; set; }
        public string billing_country { get; set; }
        public string billing_state { get; set; }
        public string billing_city { get; set; }
        public string billing_pin { get; set; }
        public string shipping_address { get; set; }
        public string shipping_country { get; set; }
        public string shipping_state { get; set; }
        public string shipping_city { get; set; }
        public string shipping_pin { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string gst_no { get; set; }
        public string order_id { get; set; }
        public string invoice_no { get; set; }
        public string invoice_date { get; set; }
        public string invoice_amount { get; set; }
        public string sales_person { get; set; }
        public string payment_mode { get; set; }
        public string product_id { get; set; }
        public string qty { get; set; }
        public string product_sno { get; set; }
        public string unit_price { get; set; }
        public string discount_rate { get; set; }
        public string cgst_rate { get; set; }
        public string sgst_rate { get; set; }
        public string igst_rate { get; set; }
        public string tcs_product_id { get; set; }
        public string tcs_qty { get; set; }
        public string tcs_rate { get; set; }
        public string tcs_cgst { get; set; }
        public string tcs_sgst { get; set; }
        public string tcs_igst { get; set; }

        public string shipmentid { get; set; }
    }

    public class clsSInvHeader
    {
        public int salesInvoiceId { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public int currencyDecimalCount { get; set; }
        public string denomination { get; set; }
        public string siFromTransType { get; set; }
        public string salesPersonId { get; set; }
        public string salesPersonCode { get; set; }
        public string salesPersonName { get; set; }
        public string posNo { get; set; }
        public string remark { get; set; }
        public string accountNo { get; set; }
        public string invoiceCosting { get; set; }
        public string grossAmount { get; set; }
        public string totalQuantity { get; set; }
        public string taxPer { get; set; }
        public string taxAmount { get; set; }
        public string discountPer { get; set; }
        public string discountAmount { get; set; }
        public string expensesAmount { get; set; }
        public string netAmount { get; set; }
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string invoiceRefNo { get; set; }
        public string invoiceMerchantId { get; set; }
        public string refOrderNumber { get; set; }
        public string condition1 { get; set; }
        public string netAmountWithExp { get; set; }
        public string jobCardId { get; set; }
        public string condition4 { get; set; }
        public string condition5 { get; set; }
        public string billingAddressId { get; set; } //field1
        public string billingAddressName { get; set; }
        public string shippingCustomerId { get; set; } //field2
        public string shippingCustomerName { get; set; }
        public string shippingAddressId { get; set; } //field3
        public string shippingAddressName { get; set; }
        public string exchangeRate { get; set; }
        public string transType { get; set; }
        public string docNo { get; set; }
        public string rowLocId { get; set; }
        public bool isApproved { get; set; }
        public string contactId { get; set; }
        public string contactName { get; set; }
        public string paymentMode { get; set; }
        public List<clsSInvDetail> lstProductDetail { get; set; }
        public List<clsInvTaxDetail> lstTaxDetail { get; set; }
        public List<clsProductSno> lstProductSerialDetail { get; set; }
        public List<clsSInvExpenses> lstExpensesDetail { get; set; }
        public List<clsPayTrns> lstPaymentDetail { get; set; }
        public Set_CustomerMaster.clsCustomerMaster clsCustomerDetail { get; set; }

        public string shipmentid { get; set; }
    }
    public class clsSInvDetail
    {
        public int siDetailId { get; set; }
        public string invoiceId { get; set; }
        public string sNo { get; set; }
        public string salesOrderId { get; set; }
        public string salesOrderNo { get; set; }
        public bool isDeliveryVoucherAllow { get; set; }
        public string productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string inventoryType { get; set; }
        public string unitId { get; set; }
        public string unitName { get; set; }
        public string unitPrice { get; set; }
        public string unitCost { get; set; }
        public string orderQty { get; set; }
        public string soldQty { get; set; }
        public string sysQty { get; set; }
        public string remainQty { get; set; }
        public string returnQty { get; set; }
        public string salesQty { get; set; }
        public string taxPer { get; set; }
        public string taxValue { get; set; }
        public string discountPer { get; set; }
        public string discountValue { get; set; }
        public string avgCost { get; set; } //field2
        public string freeQty { get; set; }
        public string lineTotal { get; set; }
    }
    public class clsInvTaxDetail
    {
        public string transId { get; set; }
        public string refType { get; set; }
        public string refId { get; set; }
        public string productId { get; set; }
        public string taxId { get; set; }
        public string taxName { get; set; }
        public string taxPer { get; set; }
        public string taxValue { get; set; }
        public string taxableAmount { get; set; } //field1
        public string refDetailId { get; set; }//field2
        public string transType { get; set; } //field3
        public string expensesId { get; set; }
        public string refRowSno { get; set; }
    }
    public class clsPayTrns
    {
        public int transId { get; set; }
        public string paymentModeId { get; set; }
        public string paymentModeName { get; set; }
        public string accountId { get; set; }
        public string accountName { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string localAmount { get; set; }
        public string foreignAmount { get; set; }
        public string exchangeRate { get; set; }
        public string cardNo { get; set; }
        public string cardName { get; set; }
        public string chequeNo { get; set; }
        public string chequeDate { get; set; }
        public string bankId { get; set; }
        public string bankName { get; set; }
        public string creditNoteVoucherId { get; set; } //in case of credit note(credit note id which one used in current invoice)
        public string paymentType { get; set; } //cash/credit
    }
    public class clsSInvExpenses
    {
        public string transId { get; set; }
        public string expenesId { get; set; }
        public string expensesName { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string foreignAmount { get; set; }
        public string localAmount { get; set; }
        public string exchangeRate { get; set; }
        public string accountId { get; set; }
        public string accountName { get; set; }
        public string refRowSno { get; set; }
    }
    public class clsProductSno
    {
        public string productId { get; set; }
        public string serialNo { get; set; }
        public string refRowSno { get; set; }
    }

    //for import invoice excel data for e-commerce order
    public int GetInvoiceCountByMerchantIdAndRefInvoiceNo(string strLocationId, string strMerchantId, string strRefInvoiceNo)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@MerchantId", strMerchantId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@RefInvoiceNo", strRefInvoiceNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "select count(*) from Inv_SalesInvoiceHeader where Location_Id=@LocationId and Inv_SalesInvoiceHeader.Invoice_Merchant_Id=@MerchantId and Inv_SalesInvoiceHeader.Invoice_Ref_No=@RefInvoiceNo and isactive='true'";
        using (DataTable dtInfo = daClass.GetDtFromQryWithParam(sql, paramList))
        {
            if (dtInfo.Rows.Count > 0)
            {
                return Convert.ToInt32(dtInfo.Rows[0][0].ToString());
            }
            else
            { return 0; }
        }
    }

    //for import invoice excel data for e-commerce order
    public int GetInvoiceCountByMerchantIdAndRefOrderId(string strLocationId, string strMerchantId, string strRefOrderId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@MerchantId", strMerchantId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@RefOrderId", strRefOrderId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "select count(*) from Inv_SalesInvoiceHeader where Location_Id=@LocationId and Inv_SalesInvoiceHeader.Invoice_Merchant_Id=@MerchantId and Inv_SalesInvoiceHeader.Ref_Order_Number=@RefOrderId and isactive='true'";
        using (DataTable dtInfo = daClass.GetDtFromQryWithParam(sql, paramList))
        {
            if (dtInfo.Rows.Count > 0)
            {
                return Convert.ToInt32(dtInfo.Rows[0][0].ToString());
            }
            else
            { return 0; }
        }
    }


    //for import invoice excel data for e-commerce order
    public int GetInvoiceIdByMerchantIdAndRefInvoiceNo(string strLocationId, string strMerchantId, string strRefInvoiceNo)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@MerchantId", strMerchantId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@RefInvoiceNo", strRefInvoiceNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "select Trans_Id from Inv_SalesInvoiceHeader where Location_Id=@LocationId and Inv_SalesInvoiceHeader.Invoice_Merchant_Id=@MerchantId and Inv_SalesInvoiceHeader.Invoice_Ref_No=@RefInvoiceNo and isactive='true'";
        using (DataTable dtInfo = daClass.GetDtFromQryWithParam(sql, paramList))
        {
            if (dtInfo.Rows.Count > 0)
            {
                return Convert.ToInt32(dtInfo.Rows[0][0].ToString());
            }
            else
            { return 0; }
        }
    }

    public clsSInvHeader getInvoiceClassForSalesReturn(string strInvoiceId, string StrCompId, string StrBrandId, string StrLocationId, string strUserId, string strFinanceYearId)
    {
        try
        {
            clsSInvHeader clsSiHeader = new clsSInvHeader();
            List<clsSInvDetail> lstClsSInvDetail = new List<clsSInvDetail> { };
            List<clsInvTaxDetail> lstClsInvTaxDetail = new List<clsInvTaxDetail> { };
            List<clsProductSno> lstClsProductSno = new List<clsProductSno> { };
            using (DataTable dtInvEdit = GetSInvHeaderAllByTransId(StrCompId, StrBrandId, StrLocationId, strInvoiceId))
            {
                //fill sales invoice header class
                DataRow dr = dtInvEdit.Rows[0];
                if (string.IsNullOrEmpty(dr["currencyDecimalCount"].ToString()) || dr["currencyDecimalCount"].ToString() == "0")
                {
                    clsSiHeader.currencyDecimalCount = 2;
                }
                else
                {
                    clsSiHeader.currencyDecimalCount = int.Parse(dr["currencyDecimalCount"].ToString());
                }

                clsSiHeader.denomination = dr["denomination"].ToString();
                clsSiHeader.salesInvoiceId = Convert.ToInt32(dr["trans_id"].ToString());
                clsSiHeader.invoiceNo = dr["Invoice_No"].ToString();
                //clsSiHeader.billingAddressName = dr["billingAddressName"].ToString(); //update code
                clsSiHeader.billingAddressId = dr["Field1"].ToString();
                //clsSiHeader.shippingAddressName = dr["shippingAddressName"].ToString(); ; //update code
                //clsSiHeader.shippingAddressId = dr["Field3"].ToString();
                //clsSiHeader.shippingCustomerId = dr["Field2"].ToString();
                // clsSiHeader.shippingCustomerName = dr["ShipCustomerName"].ToString();
                clsSiHeader.invoiceDate = Convert.ToDateTime(dr["Invoice_Date"].ToString()).ToString("dd-MMM-yyyy");
                clsSiHeader.siFromTransType = dr["SIFromTransType"].ToString();
                clsSiHeader.invoiceRefNo = dr["Invoice_Ref_No"].ToString();
                clsSiHeader.refOrderNumber = dr["Ref_Order_Number"].ToString();
                clsSiHeader.customerId = dr["Supplier_Id"].ToString();
                //clsSiHeader.customerName = dr["CustomerName"].ToString();
                //clsSiHeader.exchangeRate = dr["Field5"].ToString();
                clsSiHeader.salesPersonId = dr["SalesPerson_Id"].ToString();
                //clsSiHeader.salesPersonCode = dr["salesPersonCode"].ToString();
                //clsSiHeader.salesPersonName = dr["salesPersonName"].ToString();
                clsSiHeader.accountNo = dr["Account_No"].ToString();
                //clsSiHeader.contactId = dr["Contactid"].ToString();
                //clsSiHeader.contactName = dr["contactName"].ToString();
                clsSiHeader.transType = dr["Trans_Type"].ToString();
                //HttpContext.Current.Session["ContactId"] = dr["Supplier_Id"].ToString();

                //clsSiHeader.invoiceCosting = SystemParameter.GetAmountWithDecimal(dr["Invoice_Costing"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.totalQuantity = SystemParameter.GetAmountWithDecimal(dr["TotalQuantity"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.grossAmount = SystemParameter.GetAmountWithDecimal(dr["TotalAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.taxPer = SystemParameter.GetAmountWithDecimal(dr["NetTaxP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.taxAmount = SystemParameter.GetAmountWithDecimal(dr["NetTaxV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.netAmount = SystemParameter.GetAmountWithDecimal(dr["NetAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.discountPer = SystemParameter.GetAmountWithDecimal(dr["NetDiscountP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.discountAmount = SystemParameter.GetAmountWithDecimal(dr["NetDiscountV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.grossAmount = SystemParameter.GetAmountWithDecimal(dr["GrandTotal"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                clsSiHeader.currencyId = dr["Currency_Id"].ToString();
                //if (dr["Invoice_Merchant_Id"].ToString() != "0" && dr["Invoice_Merchant_Id"].ToString() != "")
                //{
                //    clsSiHeader.invoiceMerchantId = dr["Invoice_Merchant_Id"].ToString();
                //}
                //else
                //{
                //    clsSiHeader.invoiceMerchantId = "0";
                //}
                //if (dr["Condition3"].ToString() != "")
                //{
                //    clsSiHeader.jobCardId = dr["Condition3"].ToString();
                //}
                //try
                //{
                //    clsSiHeader.condition1 = dr["Condition1"].ToString();
                //}
                //catch { }
                //clsSiHeader.remark = dr["Remark"].ToString();
                //if (clsSiHeader.condition1.Trim() == "")
                //{
                //    clsSiHeader.condition1 = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Terms & Condition(Sales Invoice)").Rows[0]["Field1"].ToString();
                //}
                //clsSiHeader.isApproved = dr["Field4"].ToString() == "Approved" ? true : false;
                //Get Detail Record
                using (DataTable dtDetail = objSInvDetail.GetSInvDetailByInvoiceNo(StrCompId, StrBrandId, StrLocationId, strInvoiceId, strFinanceYearId))
                {
                    DataTable dtSerial = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(StrCompId, StrBrandId, StrLocationId, "SI", strInvoiceId);
                    DataTable dtTax = new DataTable();
                    if (double.Parse(clsSiHeader.taxAmount) > 0)
                    {
                        string sql = "SELECT Inv_TaxRefDetail.Tax_Id,Inv_TaxRefDetail.Tax_Per, Sys_TaxMaster.Tax_Name as taxName,Inv_TaxRefDetail.Field2 FROM Inv_TaxRefDetail left join Sys_TaxMaster on Sys_TaxMaster.Trans_Id = Inv_TaxRefDetail.Tax_Id WHERE Inv_TaxRefDetail.Ref_Type = 'SINV' AND Inv_TaxRefDetail.Ref_Id = '" + clsSiHeader.salesInvoiceId + "'";
                        dtTax = daClass.return_DataTable(sql);
                    }
                    int counter = 0;
                    foreach (DataRow drDetail in dtDetail.Rows)
                    {
                        counter++;
                        clsSInvDetail clsDetail = new clsSInvDetail();
                        clsDetail.sNo = counter.ToString();
                        clsDetail.siDetailId = Convert.ToInt32(drDetail["Trans_id"].ToString());
                        clsDetail.salesOrderId = drDetail["SoID"].ToString();
                        clsDetail.salesOrderNo = drDetail["SalesOrderNo"].ToString();
                        clsDetail.isDeliveryVoucherAllow = false; //need to check
                        clsDetail.productCode = drDetail["productCode"].ToString();
                        clsDetail.productId = drDetail["Product_Id"].ToString();
                        clsDetail.productName = drDetail["ProductName"].ToString();
                        clsDetail.productDescription = drDetail["ProductDescription"].ToString();
                        clsDetail.unitId = drDetail["Unit_Id"].ToString();
                        clsDetail.unitName = drDetail["Unit_Name"].ToString();
                        clsDetail.unitPrice = SystemParameter.GetAmountWithDecimal(drDetail["UnitPrice"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.freeQty = SystemParameter.GetAmountWithDecimal(drDetail["FreeQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.orderQty = SystemParameter.GetAmountWithDecimal(drDetail["OrderQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.soldQty = SystemParameter.GetAmountWithDecimal(drDetail["SoldQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.sysQty = SystemParameter.GetAmountWithDecimal(drDetail["SysQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.remainQty = SystemParameter.GetAmountWithDecimal(drDetail["RemainQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.returnQty = SystemParameter.GetAmountWithDecimal(drDetail["ReturnQty"].ToString() == null ? "0" : drDetail["ReturnQty"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.salesQty = SystemParameter.GetAmountWithDecimal(drDetail["Quantity"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.discountPer = SystemParameter.GetAmountWithDecimal(drDetail["DiscountP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.discountValue = SystemParameter.GetAmountWithDecimal(drDetail["DiscountV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.taxPer = SystemParameter.GetAmountWithDecimal(drDetail["TaxP"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.taxValue = SystemParameter.GetAmountWithDecimal(drDetail["TaxV"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsDetail.inventoryType = drDetail["MaintainStock"].ToString();
                        lstClsSInvDetail.Add(clsDetail);
                        //Add Serial No detail.
                        if (dtSerial.Rows.Count > 0)
                        {
                            using (DataTable dtSerialTemp = new DataView(dtSerial, "ProductId='" + clsDetail.productId + "'", "", DataViewRowState.CurrentRows).ToTable())
                            {
                                foreach (DataRow drSerial in dtSerialTemp.Rows)
                                {
                                    clsProductSno clsSerial = new clsProductSno();
                                    clsSerial.serialNo = drSerial["SerialNo"].ToString();
                                    clsSerial.refRowSno = clsDetail.sNo;
                                    clsSerial.productId = clsDetail.productId;
                                    lstClsProductSno.Add(clsSerial);
                                }
                            }
                        }
                        //Add TaxDetail
                        if (dtTax.Rows.Count > 0)
                        {
                            using (DataTable dtTaxTemp = new DataView(dtTax, "Field2='" + clsDetail.siDetailId + "'", "", DataViewRowState.CurrentRows).ToTable())
                            {
                                foreach (DataRow drTax in dtTaxTemp.Rows)
                                {
                                    clsInvTaxDetail clsTax = new clsInvTaxDetail();
                                    clsTax.taxId = drTax["Tax_Id"].ToString();
                                    clsTax.taxName = drTax["taxName"].ToString();
                                    clsTax.taxPer = drTax["Tax_Per"].ToString();
                                    clsTax.refRowSno = clsDetail.sNo;
                                    clsTax.productId = clsDetail.productId;
                                    lstClsInvTaxDetail.Add(clsTax);
                                }
                            }
                        }
                    }
                    clsSiHeader.lstProductDetail = lstClsSInvDetail;
                    clsSiHeader.lstTaxDetail = lstClsInvTaxDetail;
                    clsSiHeader.lstProductSerialDetail = lstClsProductSno;
                    dtSerial = null;
                    dtTax = null;
                }
                ////add expenses detail
                //List<clsSInvExpenses> lstClsExpenses = new List<clsSInvExpenses> { };
                //decimal expensesAmount = 0;
                //using (DataTable dtExpenses = new Inv_ShipExpDetail().Get_ShipExpDetailByInvoiceId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strInvoiceId, "SI"))
                //{
                //    foreach (DataRow drExpenses in dtExpenses.Rows)
                //    {
                //        clsSInvExpenses clsExpenses = new clsSInvExpenses();
                //        clsExpenses.expensesName = drExpenses["Exp_Name"].ToString();
                //        clsExpenses.expenesId = drExpenses["Expense_Id"].ToString();
                //        clsExpenses.currencyName = drExpenses["CurrencyName"].ToString();
                //        clsExpenses.currencyId = drExpenses["ExpCurrencyId"].ToString();
                //        clsExpenses.foreignAmount = SystemParameter.GetAmountWithDecimal(drExpenses["FCExpAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //        clsExpenses.exchangeRate = SystemParameter.GetAmountWithDecimal(drExpenses["ExpExchangeRate"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //        clsExpenses.localAmount = SystemParameter.GetAmountWithDecimal(drExpenses["Exp_Charges"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //        expensesAmount += Convert.ToDecimal(clsExpenses.localAmount);
                //        lstClsExpenses.Add(clsExpenses);
                //    }
                //}
                //clsSiHeader.lstExpensesDetail = lstClsExpenses;
                // getting expenses amount total from expenses and setting it to header table
                //clsSiHeader.expensesAmount = SystemParameter.GetAmountWithDecimal(expensesAmount.ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //clsSiHeader.netAmountWithExp = SystemParameter.GetAmountWithDecimal((expensesAmount + Convert.ToDecimal(clsSiHeader.grossAmount)).ToString(), clsSiHeader.currencyDecimalCount.ToString());
                //addd payment detail
                List<clsPayTrns> lstClsPaymentDetail = new List<clsPayTrns> { };
                using (DataTable dtPayment = new Inv_PaymentTrans(_strConString).GetPaymentTransTrue(StrCompId.ToString(), "SI", strInvoiceId))
                {
                    foreach (DataRow drPayment in dtPayment.Rows)
                    {
                        clsPayTrns clsPayment = new clsPayTrns();
                        clsPayment.paymentModeId = drPayment["PaymentModeId"].ToString();
                        clsPayment.paymentModeName = drPayment["PaymentName"].ToString();
                        clsPayment.accountId = drPayment["AccountNo"].ToString();
                        clsPayment.accountName = drPayment["AccountName"].ToString();
                        clsPayment.cardName = drPayment["CardName"].ToString();
                        clsPayment.cardNo = drPayment["CardNo"].ToString();
                        clsPayment.bankId = drPayment["BankId"].ToString();
                        clsPayment.chequeNo = drPayment["ChequeNo"].ToString();
                        clsPayment.chequeDate = drPayment["ChequeDate"].ToString();
                        clsPayment.localAmount = SystemParameter.GetAmountWithDecimal(drPayment["Pay_Charges"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsPayment.currencyId = drPayment["PayCurrencyID"].ToString();
                        clsPayment.exchangeRate = drPayment["PayExchangeRate"].ToString();
                        clsPayment.foreignAmount = SystemParameter.GetAmountWithDecimal(drPayment["FCPayAmount"].ToString(), clsSiHeader.currencyDecimalCount.ToString());
                        clsPayment.creditNoteVoucherId = drPayment["Field3"].ToString();
                        clsPayment.paymentType = drPayment["PaymentType"].ToString();
                        lstClsPaymentDetail.Add(clsPayment);
                    }
                }
                clsSiHeader.lstPaymentDetail = lstClsPaymentDetail;
            }
            return clsSiHeader;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
