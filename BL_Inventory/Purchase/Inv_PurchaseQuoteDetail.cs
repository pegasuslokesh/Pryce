﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Inv_PurchaseQuoteDetail
/// </summary>
public class Inv_PurchaseQuoteDetail
{
    DataAccessClass daClass = null;
    public Inv_PurchaseQuoteDetail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int InsertQuoteDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No, string strSupplier_Id, string strProduct_Id,string SuggestedProductName, string strProductDescription, string strRefrencedProductName, string strRefrencedPartNo, string strUnitId, string strReqQty, string strUnitPrice, string strTaxPercentage, string strTaxValue, string strPriceAfterTax, string strDisPercentage, string strDiscountValue, string strAmount, string strTermsCondition, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[33];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Product_Id", strProduct_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ProductDescription", strProductDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@RefrencedProductName", strRefrencedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@RefrencedPartNo", strRefrencedPartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@UnitId", strUnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReqQty", strReqQty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@UnitPrice", strUnitPrice, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@TaxPercentage", strTaxPercentage, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@TaxValue", strTaxValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@PriceAfterTax", strPriceAfterTax, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@DisPercentage", strDisPercentage, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@DiscountValue", strDiscountValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@TermsCondition", strTermsCondition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[32] = new PassDataToSql("@SuggestedProductName",SuggestedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_Insert", paramList);
        return Convert.ToInt32(paramList[31].ParaValue);
    }
    //created for rollback Trsnaction
    //10-03-2016
    //by jitendra upadhyay
    public int InsertQuoteDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No, string strSupplier_Id, string strProduct_Id, string SuggestedProductName, string strProductDescription, string strRefrencedProductName, string strRefrencedPartNo, string strUnitId, string strReqQty, string strUnitPrice, string strTaxPercentage, string strTaxValue, string strPriceAfterTax, string strDisPercentage, string strDiscountValue, string strAmount, string strTermsCondition, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[33];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Product_Id", strProduct_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ProductDescription", strProductDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@RefrencedProductName", strRefrencedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@RefrencedPartNo", strRefrencedPartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@UnitId", strUnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReqQty", strReqQty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@UnitPrice", strUnitPrice, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@TaxPercentage", strTaxPercentage, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@TaxValue", strTaxValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@PriceAfterTax", strPriceAfterTax, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@DisPercentage", strDisPercentage, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@DiscountValue", strDiscountValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@TermsCondition", strTermsCondition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[32] = new PassDataToSql("@SuggestedProductName", SuggestedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        
        
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[31].ParaValue);
    }
    public int UpdateQuoteDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id, string strRPQ_No, string strSupplier_Id, string strProduct_Id, string SuggestedProductName, string strProductDescription, string strRefrencedProductName, string strRefrencedPartNo, string strUnitId, string strReqQty, string strUnitPrice, string strTaxPercentage, string strTaxValue, string strPriceAfterTax, string strDisPercentage, string strDiscountValue, string strAmount, string strTermsCondition, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[32];

        paramList[0] = new PassDataToSql("@Company_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Supplier_Id", strSupplier_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Product_Id", strProduct_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ProductDescription", strProductDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@RefrencedProductName", strRefrencedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@RefrencedPartNo", strRefrencedPartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@UnitId", strUnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ReqQty", strReqQty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@UnitPrice", strUnitPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@TaxPercentage", strTaxPercentage, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@TaxValue", strTaxValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@PriceAfterTax", strPriceAfterTax, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@DisPercentage", strDisPercentage, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@DiscountValue", strDiscountValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Amount", strAmount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@TermsCondition", strTermsCondition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[31] = new PassDataToSql("@SuggestedProductName", SuggestedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_Update", paramList);
        return Convert.ToInt32(paramList[30].ParaValue);
    }
    public void DeleteQuoteDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No, string strSupplierId)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Supplier_Id", strSupplierId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_RowStatus", paramList);
    }
    //created for rollback Trsnaction
    //10-03-2016
    //by jitendra upadhyay
    public void DeleteQuoteDetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No, string strSupplierId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Supplier_Id", strSupplierId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_RowStatus", paramList,ref trns);
    }
    public DataTable GetQuoteDetailAllTrue(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetQuoteDetailAllFalse(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetQuoteDetailByTrans_Id(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strTrans_Id)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetQuoteDetailByRPQ_No(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    //created for rollback Trsnaction
    //09-03-2016
    //by jitendra upadhyay
    public DataTable GetQuoteDetailByRPQ_No(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strRPQ_No, ref SqlTransaction trns)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", strRPQ_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetQuoteDetailAllData(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@RPQ_No", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_PurchaseQuoteDetail_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetPurchaseQuationDatilsNotInPOdetail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string RPQId)
    {
        
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Rpq_Id", RPQId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Inv_PurchaseQuationFilterbyPo", paramList))
        {
            return dtInfo;
        }
    }
    public int UpdateQuoteDetailSupplierByTransID(string TransId,string St)
    {
       
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Field2", St, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
       
        daClass.execute_Sp("sp_Inv_PurchaseQuoteDetail_Update_Product_Supplier", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
      

    }
}
