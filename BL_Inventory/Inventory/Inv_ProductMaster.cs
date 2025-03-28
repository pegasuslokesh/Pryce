using System;
using System.Data;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Inv_ProductMaster
/// </summary>
public class Inv_ProductMaster
{
    public DataAccessClass daClass = null;
    public Inv_ProductMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int InsertProductMaster(string CompanyId, string BrandId, string ProductCode, string PartNo, string ModelNo, string ModelName, string ProductName, string ProductNameAr, string CountryId, string UnitId, string ItemType, string HScode, string HasBatchNo, string TypeOfBatchNo, string HasSerialNo, string ReorderQty, string CostPrice, string Description, string SalesPrice1, string SalesPrice2, string SalesPrice3, string ProductColor, string WSalesPrice, string ReservedQty, string DamageQty, string ExpiredQty, string MaximumQty, string MinimumQty, string Profit, string Discount, string MaintainStock, string URL, string Weight, string WeightUnitID, string DimLenth, string DimHieght, string DimDepth, string AlternateId1, string AlternateId2, string AlternateId3, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, string developerCommission, string projectId, string currency_Id, string sno_prefix, string sno_suffix, string sno_startFrom)
    {
        developerCommission = developerCommission == "" ? "0.00" : developerCommission;
        projectId = projectId == "" ? "0" : projectId;
        //Country id as Made In Country,
        PassDataToSql[] paramList = new PassDataToSql[59];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@PartNo", PartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModelNo", ModelNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@EProductName", ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@LProductName", ProductNameAr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@CountryId", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@UnitId", UnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ItemType", ItemType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@HScode", HScode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@HasBatchNo", HasBatchNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@TypeOfBatchNo", TypeOfBatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@HasSerialNo", HasSerialNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReorderQty", ReorderQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CostPrice", CostPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@SalesPrice1", SalesPrice1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@SalesPrice2", SalesPrice2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@SalesPrice3", SalesPrice3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ProductColor", ProductColor, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@WSalePrice", WSalesPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ReservedQty", ReservedQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@DamageQty", DamageQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ExpiredQty", ExpiredQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@MaximumQty", MaximumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@MinimumQty", MinimumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Profit", Profit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Discount", Discount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@MaintainStock", MaintainStock, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@URL", URL, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@VMWeight", Weight, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@WeighUnitID", WeightUnitID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@DimLenth", DimLenth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@DimHieght", DimHieght, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@DimDepth", DimDepth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@AlternateId1", AlternateId1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@AlternateId2", AlternateId2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@AlternateId3", AlternateId3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[39] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[46] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[52] = new PassDataToSql("@ModelName", ModelName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@developerCommission", developerCommission, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[54] = new PassDataToSql("@projectId", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[55] = new PassDataToSql("@currency_id", currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[56] = new PassDataToSql("@SnoPrefix", sno_prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[57] = new PassDataToSql("@SnoSuffix", sno_suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[58] = new PassDataToSql("@SnoStartFrom", sno_startFrom, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Inv_ProductMaster_Insert", paramList);
        return Convert.ToInt32(paramList[51].ParaValue);
    }
    public int InsertProductMaster(string CompanyId, string BrandId, string ProductCode, string PartNo, string ModelNo, string ModelName, string ProductName, string ProductNameAr, string CountryId, string UnitId, string ItemType, string HScode, string HasBatchNo, string TypeOfBatchNo, string HasSerialNo, string ReorderQty, string CostPrice, string Description, string SalesPrice1, string SalesPrice2, string SalesPrice3, string ProductColor, string WSalesPrice, string ReservedQty, string DamageQty, string ExpiredQty, string MaximumQty, string MinimumQty, string Profit, string Discount, string MaintainStock, string URL, string Weight, string WeightUnitID, string DimLenth, string DimHieght, string DimDepth, string AlternateId1, string AlternateId2, string AlternateId3, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, string developerCommission, string projectId, string currency_id, string sno_prefix, string sno_suffix, string sno_startFrom, string strSizeId, string strColourId, string strAPIStatus, ref SqlTransaction trns)
    {
        developerCommission = developerCommission == "" ? "0.00" : developerCommission;
        projectId = projectId == "" ? "0" : projectId;

        //Country id as Made In Country,
        PassDataToSql[] paramList = new PassDataToSql[62];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@PartNo", PartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModelNo", ModelNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@EProductName", ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@LProductName", ProductNameAr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@CountryId", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@UnitId", UnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ItemType", ItemType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@HScode", HScode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@HasBatchNo", HasBatchNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@TypeOfBatchNo", TypeOfBatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@HasSerialNo", HasSerialNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReorderQty", ReorderQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CostPrice", CostPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@SalesPrice1", SalesPrice1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@SalesPrice2", SalesPrice2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@SalesPrice3", SalesPrice3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ProductColor", ProductColor, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@WSalePrice", WSalesPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ReservedQty", ReservedQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@DamageQty", DamageQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ExpiredQty", ExpiredQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@MaximumQty", MaximumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@MinimumQty", MinimumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Profit", Profit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Discount", Discount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@MaintainStock", MaintainStock, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@URL", URL, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@VMWeight", Weight, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@WeighUnitID", WeightUnitID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@DimLenth", DimLenth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@DimHieght", DimHieght, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@DimDepth", DimDepth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@AlternateId1", AlternateId1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@AlternateId2", AlternateId2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@AlternateId3", AlternateId3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[39] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[46] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[52] = new PassDataToSql("@ModelName", ModelName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@developerCommission", developerCommission, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[54] = new PassDataToSql("@projectId", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[55] = new PassDataToSql("@currency_id", currency_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[56] = new PassDataToSql("@SnoPrefix", sno_prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[57] = new PassDataToSql("@SnoSuffix", sno_suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[58] = new PassDataToSql("@SnoStartFrom", sno_startFrom, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[59] = new PassDataToSql("@SizeId", strSizeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[60] = new PassDataToSql("@ColourId", strColourId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[61] = new PassDataToSql("@APIStatus", strAPIStatus, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);


        daClass.execute_Sp("sp_Inv_ProductMaster_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[51].ParaValue);
    }

    public void  InsertProductLabelInfo(string CompanyId, string BrandId, string ProductId, string L_ProductName, string L_ProductDescription, ref SqlTransaction trns)
    {
        

        //Country id as Made In Country,
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@L_ProductName", L_ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@L_ProductDescription", L_ProductDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
       

        daClass.execute_Sp("sp_Inv_ProductLabel_Insert", paramList, ref trns);
       
    }

  


    public int UpdateProductMaster(string CompanyId, string BrandId, string ProductId, string ProductCode, string PartNo, string ModelNo, string ModelName, string ProductName, string ProductNameAr, string CountryId, string UnitId, string ItemType, string HScode, string HasBatchNo, string TypeOfBatchNo, string HasSerialNo, string ReorderQty, string CostPrice, string Description, string SalesPrice1, string SalesPrice2, string SalesPrice3, string ProductColor, string WSalesPrice, string ReservedQty, string DamageQty, string ExpiredQty, string MaximumQty, string MinimumQty, string Profit, string Discount, string MaintainStock, string URL, string Weight, string WeightUnitID, string DimLenth, string DimHieght, string DimDepth, string AlternateId1, string AlternateId2, string AlternateId3, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate, string developerCommission, string projectId, string currency_id, string sno_prefix, string sno_suffix, string sno_startFrom)
    {
        developerCommission = developerCommission == "" ? "0.00" : developerCommission;
        projectId = projectId == "" ? "0" : projectId;//Country id as Made In Country
        PassDataToSql[] paramList = new PassDataToSql[58];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@PartNo", PartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModelNo", ModelNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@EProductName", ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@LProductName", ProductNameAr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CountryId", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@UnitId", UnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ItemType", ItemType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@HScode", HScode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@HasBatchNo", HasBatchNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@TypeOfBatchNo", TypeOfBatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@HasSerialNo", HasSerialNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReorderQty", ReorderQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CostPrice", CostPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@SalesPrice1", SalesPrice1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@SalesPrice2", SalesPrice2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@SalesPrice3", SalesPrice3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ProductColor", ProductColor, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@WSalePrice", WSalesPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReservedQty", ReservedQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@DamageQty", DamageQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ExpiredQty", ExpiredQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@MaximumQty", MaximumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@MinimumQty", MinimumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Profit", Profit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Discount", Discount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@MaintainStock", MaintainStock, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@URL", URL, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@VMWeight", Weight, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@WeighUnitID", WeightUnitID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@DimLenth", DimLenth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@DimHieght", DimHieght, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@DimDepth", DimDepth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@AlternateId1", AlternateId1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@AlternateId2", AlternateId2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@AlternateId3", AlternateId3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[40] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);


        paramList[47] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[51] = new PassDataToSql("@ModelName", ModelName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@developerCommission", developerCommission, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@projectId", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[54] = new PassDataToSql("@currency_id", currency_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[55] = new PassDataToSql("@SnoPrefix", sno_prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[56] = new PassDataToSql("@SnoSuffix", sno_suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[57] = new PassDataToSql("@SnoStartFrom", sno_startFrom, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_Update", paramList);
        return Convert.ToInt32(paramList[50].ParaValue);
    }

    public int DeleteProductLabelInfo(string CompanyId, string BrandId, string ProductId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[50].ParaValue);
    }

    public int UpdateProductMaster(string CompanyId, string BrandId, string ProductId, string ProductCode, string PartNo, string ModelNo, string ModelName, string ProductName, string ProductNameAr, string CountryId, string UnitId, string ItemType, string HScode, string HasBatchNo, string TypeOfBatchNo, string HasSerialNo, string ReorderQty, string CostPrice, string Description, string SalesPrice1, string SalesPrice2, string SalesPrice3, string ProductColor, string WSalesPrice, string ReservedQty, string DamageQty, string ExpiredQty, string MaximumQty, string MinimumQty, string Profit, string Discount, string MaintainStock, string URL, string Weight, string WeightUnitID, string DimLenth, string DimHieght, string DimDepth, string AlternateId1, string AlternateId2, string AlternateId3, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate, string developerCommission, string projectId, string currency_id, string sno_prefix, string sno_suffix, string sno_startFrom, string strSizeId, string strColourId, ref SqlTransaction trns)
    {
        developerCommission = developerCommission == "" ? "0.00" : developerCommission;
        projectId = projectId == "" ? "0" : projectId;
        //Country id as Made In Country
        PassDataToSql[] paramList = new PassDataToSql[60];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@PartNo", PartNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModelNo", ModelNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@EProductName", ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@LProductName", ProductNameAr, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CountryId", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@UnitId", UnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ItemType", ItemType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@HScode", HScode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@HasBatchNo", HasBatchNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@TypeOfBatchNo", TypeOfBatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@HasSerialNo", HasSerialNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReorderQty", ReorderQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CostPrice", CostPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@SalesPrice1", SalesPrice1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@SalesPrice2", SalesPrice2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@SalesPrice3", SalesPrice3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ProductColor", ProductColor, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@WSalePrice", WSalesPrice, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReservedQty", ReservedQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@DamageQty", DamageQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ExpiredQty", ExpiredQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@MaximumQty", MaximumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@MinimumQty", MinimumQty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Profit", Profit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Discount", Discount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@MaintainStock", MaintainStock, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@URL", URL, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@VMWeight", Weight, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@WeighUnitID", WeightUnitID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@DimLenth", DimLenth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@DimHieght", DimHieght, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@DimDepth", DimDepth, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@AlternateId1", AlternateId1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@AlternateId2", AlternateId2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@AlternateId3", AlternateId3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[40] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);


        paramList[47] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[51] = new PassDataToSql("@ModelName", ModelName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@developerCommission", developerCommission, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@projectId", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[54] = new PassDataToSql("@currency_id", currency_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[55] = new PassDataToSql("@SnoPrefix", sno_prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[56] = new PassDataToSql("@SnoSuffix", sno_suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[57] = new PassDataToSql("@SnoStartFrom", sno_startFrom, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[58] = new PassDataToSql("@SizeId", strSizeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[59] = new PassDataToSql("@ColourId", strColourId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[50].ParaValue);
    }
    public int DeleteProductMaster(string CompanyId, string ProductId, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Inv_ProductMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public int updateVerifyStatus(string ProductId, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Inv_ProductMaster_VerifyStatus", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public int RestoreProductMaster(string CompanyId, string ProductId, string ModifiedBy, string ModifiedDate, string IsActive)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[6] = new PassDataToSql("@ProductCode", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }


    public int UpdateProductId(string CompanyId, string ProductId, string ModifiedBy, string ModifiedDate, string ProductCode)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[6] = new PassDataToSql("@IsActive", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public int UpdateProductId(string CompanyId, string ProductId, string ModifiedBy, string ModifiedDate, string ProductCode, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[6] = new PassDataToSql("@IsActive", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_RowStatus", paramList, ref trns);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public int UpdateProductIdforModelColourSize(string CompanyId, string ProductId, string ModifiedBy, string ModifiedDate, string ProductCode, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[6] = new PassDataToSql("@IsActive", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_ProductMaster_RowStatus_OnlyProduct", paramList, ref trns);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public DataTable GetProductMasterById(string CompanyId, string BrandId, string ProductId, string FinanceYearId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetProductLabelMasterInfo(string CompanyId, string BrandId, string ProductId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        
        
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductLabelMaster_SelectRow", paramList);

        return dtInfo;
    }


    //created for rollback Transaction

    public DataTable GetProductMasterById(string CompanyId, string BrandId, string ProductId, string FinanceYearId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetProductMasterTrueAll(string CompanyId, string BrandId, string LocationId, string FinanceYearId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable Get_Inv_Product_Master_Search(string Company_Id, string Brand_Id, string Location_Id, string ProductId, string ProductCode, string ProductName, string Alternate1, string Alternate2, string Alternate3, string CategoryId, string CategoryCode, string CategoryName, string Optype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[13];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ProductName", ProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Alternate1", Alternate1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Alternate2", Alternate2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Alternate3", Alternate3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@CategoryId", CategoryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@CategoryCode", CategoryCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CategoryName", CategoryName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Optype", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Get_Inv_Product_Master_Search", paramList);

        return dtInfo;
    }

    public DataTable GetProductMaster_By_PageNumber(string CompanyId, string BrandId, string LocationId, string strProductCode, string strProductId, string FinanceYearId, string PageSize, string PageNo, string mBrand_id, string category_id, string disContinueOnly, string searchProductIdSeries, string searchField, string searchOperator, string searchValue, string isRecCountOnly, string strOpType, string strIsActive)
    {
        string strsql = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", strProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ProductId", strProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", strOpType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@PageSize", PageSize, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@PageNo", PageNo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@mBrand_id", mBrand_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@category_id", category_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@disContinueOnly", disContinueOnly, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@searchProductIdSeries", searchProductIdSeries, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@searchField", searchField, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@searchOperator", searchOperator, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@searchValue", searchValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@isRecCountOnly", isRecCountOnly, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow_by_pageNo", paramList);

        return dtInfo;
    }

    public DataTable GetProductMasterTrueAll(string CompanyId, string BrandId, string LocationId, string FinanceYearId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList, ref trns);

        return dtInfo;
    }
    public DataTable GetProductMasterTrueAllwithStock(string CompanyId, string BrandId, string LocationId, string FinanceYearId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", FinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetProductMasterTrueAllCategorywithStock(string CompanyId, string BrandId, string LocationId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetProductMasterTrueAllRackwithStock(string CompanyId, string BrandId, string LocationId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "10", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetProductMasterTrueAllManifacturingBrandwithStock(string CompanyId, string BrandId, string LocationId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "9", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetProductMasterTrueAllwithoutcompanyidandBrandId(string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetProductMasterFalseAll(string CompanyId, string BrandId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public string GetAutoID(string CompanyId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);


        string strPattern = "P" + DateTime.Now.Day.ToString() + "" + DateTime.Now.Month.ToString() + "" + DateTime.Now.Year + "";

        DataTable dtTemp = new DataTable();
        dtTemp = new DataView(dtInfo, "ProductCode Like '" + strPattern + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();

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
    public DataTable GetProductMaserTrueAllByProductCode(string CompanyId, string BrandId, string ProductCode, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", ProductCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetDistinctProductName(string strCompanyId, string BrandId, string strPrefixText, string strFinanceYear,string strLocId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "12", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", strLocId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        dtInfo = new DataView(dtInfo, "EProductName Like '%" + strPrefixText + "%' and Field1=' '", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
        return dtInfo;
    }
    public DataTable GetDistinctProductCode(string strCompanyId, string BrandId, string strPrefixText, string strFinanceYear,string strLocId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "12", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", strLocId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        dtInfo = new DataView(dtInfo, "ProductCode Like '%" + strPrefixText + "%' and Field1=' '", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();
        return dtInfo;
    }

    public DataTable GetProductMasterAll(string CompanyId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable SearchProductMasterByParameter(string CompanyId, string BrandId, string LocationId, string Param_Value)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Product_Param", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Inv_ProductMaster_SearchProduct", paramList);

        return dtInfo;
    }
    public int UpdateProductIDInReference(string SuggestedProductName, string ProductId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@SuggestedProductName", SuggestedProductName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Product_Id", ProductId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Inv_UpdateProductIDInReference", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);


    }
    public DataTable GetSalesPrice_According_InventoryParameter(string strCompany_Id, string strBrandId, string strLocationId, string strContactType, string strContact_Id, string strProduct_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Contact_Type", strContactType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Contact_Id", strContact_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Product_Id", strProduct_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@BrandId", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@FinacialYearId","0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_GetSalesPrice_AccordingParameter", paramList);
        return dtInfo;
    }
    public DataTable GetSalesPrice_According_InventoryParameter(string strCompany_Id, string strBrandId, string strLocationId, string strContactType, string strContact_Id, string strProduct_Id,string FinancialYearId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Contact_Type", strContactType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Contact_Id", strContact_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Product_Id", strProduct_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@BrandId", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@LocationId", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@FinacialYearId", FinancialYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_GetSalesPrice_AccordingParameter", paramList);
        return dtInfo;
    }
    public DataTable GetProductCodebyProductId(string ProductId, string strFinanceYear)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductCode", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Finance_Year_Id", strFinanceYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ProductMaster_SelectRow", paramList);

        return dtInfo;
    }

    public string GetProductIdbyProductCode(string ProductCode,string strBrandId)
    {
        string id = "0";
        id = daClass.get_SingleValue("select top 1 ProductId from Inv_ProductMaster where Brand_Id=" + strBrandId + " and ProductCode='" + ProductCode + "'");
        id = id == "@NOTFOUND@" ? "0" : id;
        return id;
    }
    public string GetProductNamebyProductCode(string ProductCode)
    {
        string EProductName = "";
        EProductName = daClass.get_SingleValue("select top 1 EProductName from Inv_ProductMaster where  ProductCode='" + ProductCode + "'");
        EProductName = EProductName == "@NOTFOUND@" ? "" : EProductName;
        return EProductName;
    }
    public string GetProductCodebyProductId(string ProductId)
    {
        string id = "0";
        id = daClass.get_SingleValue("select top 1 ProductCode from Inv_ProductMaster where  ProductId='" + ProductId + "'");
        id = id == "@NOTFOUND@" ? "0" : id;
        return id;
    }

    public string GetProductNamebyProductId(string ProductId)
    {
        string id = "";
        id = daClass.get_SingleValue("select top 1 EProductName from Inv_ProductMaster where ProductId='" + ProductId + "'");
        id = id == "@NOTFOUND@" ? "" : id;
        return id;
    }

    public string GetProductDescriptionbyProductId(string ProductId)
    {
        return daClass.get_SingleValue("select top 1 Description from Inv_ProductMaster where  ProductId='" + ProductId + "'");
    }

    public string GetProductIdbyProductName(string ProductName,string strBrandId)
    {
        string id = "";
        id = daClass.get_SingleValue("select top 1 ProductId from Inv_ProductMaster where Brand_Id=" + strBrandId + " and EProductName='" + ProductName + "'");
        id = id == "@NOTFOUND@" ? "" : id;
        return id;
    }

    public DataTable GetProductName_PreText(string companyid, string brandid, string ProductName)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("SELECT distinct top 15 Pm.ProductId, pm.EProductName FROM Inv_ProductMaster AS Pm LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId WHERE Inv_Product_CompanyBrand.Company_Id = '" + companyid + "' AND Inv_Product_CompanyBrand.BrandId = '" + brandid + "' AND pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True' and pm.EProductName like '%" + ProductName + "%'"))
        {
            return dt_product;
        }
    }

    public DataTable GetProductCode_PreText(string companyid, string brandid, string productcode)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("SELECT distinct top 15 Pm.ProductId, pm.productcode FROM Inv_ProductMaster AS Pm LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId WHERE Inv_Product_CompanyBrand.Company_Id = '" + companyid + "' AND Inv_Product_CompanyBrand.BrandId = '" + brandid + "' AND pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True' and pm.productcode like '%" + productcode + "%'"))
        {
            return dt_product;
        }
    }


    public DataTable GetProductDataByCategoryId(string companyid, string brandid, string location, string financialYrId, string categoryId)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("select distinct inv_productmaster.productid, inv_productmaster.MaintainStock, inv_productmaster.ProductCode, inv_productmaster.EProductName, inv_productmaster.modelno, '' as Description, inv_productmaster.UnitId, inv_productmaster.ItemType, inv_productmaster.salesprice1, Inv_Product_CategoryMaster.Category_Name, Inv_UnitMaster.unit_name, case when Inv_StockDetail.Quantity is null then 0 else Inv_StockDetail.Quantity end as Quantity from Inv_ProductMaster inner join Inv_Product_RelProduct on Inv_Product_RelProduct.SubProduct_Id =Inv_ProductMaster.ProductId inner join Inv_Product_Category on Inv_Product_Category.ProductId = Inv_Product_RelProduct.SubProduct_Id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.Category_Id = Inv_Product_Category.CategoryId LEFT JOIN Inv_StockDetail ON Inv_StockDetail.productid = Inv_Product_RelProduct.SubProduct_Id LEFT JOIN Inv_UnitMaster ON Inv_UnitMaster.Unit_Id = inv_productmaster.UnitId where Inv_Product_Category.CategoryId = '" + categoryId + "' and inv_productmaster.Company_Id = '" + companyid + "' and inv_productmaster.Brand_Id = '" + brandid + "' and Inv_StockDetail.Finance_Year_Id = '" + financialYrId + "' and Inv_StockDetail.Location_Id = '" + location + "'"))
            return dt_product;
    }
    public DataTable GetProductDataByCategoryId(string companyid, string brandid, string location, string financialYrId, string categoryId, string modelId)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("select distinct inv_productmaster.productid, inv_productmaster.MaintainStock, inv_productmaster.ProductCode, inv_productmaster.EProductName, inv_productmaster.modelno, '' as Description, inv_productmaster.UnitId, inv_productmaster.ItemType, inv_productmaster.salesprice1, Inv_Product_CategoryMaster.Category_Name, Inv_UnitMaster.unit_name, case when Inv_StockDetail.Quantity is null then 0 else Inv_StockDetail.Quantity end as Quantity from Inv_ProductMaster inner join Inv_Product_RelProduct on Inv_Product_RelProduct.SubProduct_Id =Inv_ProductMaster.ProductId inner join Inv_Product_Category on Inv_Product_Category.ProductId = Inv_Product_RelProduct.SubProduct_Id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.Category_Id = Inv_Product_Category.CategoryId LEFT JOIN Inv_StockDetail ON Inv_StockDetail.productid = Inv_Product_RelProduct.SubProduct_Id LEFT JOIN Inv_UnitMaster ON Inv_UnitMaster.Unit_Id = inv_productmaster.UnitId where Inv_Product_Category.CategoryId = '" + categoryId + "' and inv_productmaster.Company_Id = '" + companyid + "' and inv_productmaster.Brand_Id = '" + brandid + "' and Inv_StockDetail.Finance_Year_Id = '" + financialYrId + "' and Inv_StockDetail.Location_Id = '" + location + "' and inv_productmaster.modelno ='" + modelId + "'"))
            return dt_product;
    }
    public DataTable GetRelatedProductDataByProductId(string companyid, string brandid, string location, string financialYrId, string productID)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("select inv_productmaster.productid,inv_productmaster.MaintainStock,inv_productmaster.ProductCode,inv_productmaster.EProductName,inv_productmaster.Description,inv_productmaster.UnitId,Inv_UnitMaster.unit_name,inv_productmaster.ItemType,inv_productmaster.salesprice1,Inv_StockDetail.Quantity,Inv_Product_CategoryMaster.Category_Name from Inv_Product_RelProduct inner join inv_productmaster on inv_productmaster.ProductId = Inv_Product_RelProduct.SubProduct_Id inner join Inv_Product_Category on Inv_Product_Category.ProductId =Inv_Product_RelProduct.Product_Id  inner join Inv_StockDetail on Inv_StockDetail.productid=Inv_Product_RelProduct.Product_Id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.Category_Id = Inv_Product_Category.CategoryId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id=inv_productmaster.UnitId where inv_productmaster.IsActive='true' and inv_productmaster.ProductId='" + productID + "' and Inv_StockDetail.Finance_Year_Id='" + financialYrId + "' and Inv_StockDetail.Location_Id='" + location + "' and inv_productmaster.Company_Id='" + companyid + "' and inv_productmaster.Brand_Id='" + brandid + "'"))
            return dt_product;
    }

    public DataTable GetRelatedProductDataByModelNo(string companyid, string brandid, string location, string financialYrId, string modelno)
    {
        DataTable dt_product = new DataTable();
        using (dt_product = daClass.return_DataTable("SELECT pm.ProductId, Pm.ProductCode, Pm.EProductName, pm.UnitId, pm.maintainstock, '' as description, Inv_ModelMaster.Model_Name, Inv_ModelMaster.Model_No, Inv_UnitMaster.Unit_Name AS UnitName, Pm.SalesPrice1 AS ProductSalesPrice, CASE WHEN inv_stockdetail.quantity IS NULL THEN 0 ELSE CAST(inv_stockdetail.quantity AS numeric(18, 3)) END AS quantity FROM Inv_ProductMaster AS PM INNER JOIN Inv_ModelMaster ON PM.ModelNo = Inv_ModelMaster.Trans_Id INNER JOIN Inv_UnitMaster ON pm.UnitId = Inv_UnitMaster.Unit_Id left join inv_stockdetail on inv_stockdetail.Company_Id = '" + companyid + "' AND inv_stockdetail.Brand_Id = '" + brandid + "' AND inv_stockdetail.Location_Id = '" + location + "' AND inv_stockdetail.ProductId = PM.ProductId AND Inv_StockDetail.Finance_Year_Id = '" + financialYrId + "' WHERE PM.Company_Id = '" + companyid + "' AND PM.Brand_Id = '" + brandid + "' AND Inv_ModelMaster.Model_No = '" + modelno + "' AND PM.IsActive = 'True' AND PM.Field1 = ' ' ORDER BY pm.ProductId"))
            return dt_product;
    }

    public string getProductAutoSerialInitials(string productCode,string strCompId)
    {
        string defaultNo = string.Empty;
        try
        {
            string sql = "select ProductId,SnoPrefix,SnoSuffix,SnoStartFrom,ModelNo from Inv_ProductMaster where company_id=@company_id and ProductCode=@product_code";
            PassDataToSql[] paramList = new PassDataToSql[2];
            paramList[0] = new PassDataToSql("@company_id", strCompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@product_code", productCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            using (DataTable _dtProduct = daClass.GetDtFromQryWithParam(sql, paramList))
            {
                string model_id = string.Empty;
                if (!string.IsNullOrEmpty(_dtProduct.Rows[0]["ModelNo"].ToString()) && int.Parse(_dtProduct.Rows[0]["ModelNo"].ToString()) > 0)
                {
                    model_id = _dtProduct.Rows[0]["ModelNo"].ToString();
                    //Get serial settings from model master
                    sql = "select SnoPrefix, SnoSuffix, SnoStartFrom, trans_id from Inv_ModelMaster where company_id = @company_id and trans_id = @model_id";
                    PassDataToSql[] paramList1 = new PassDataToSql[2];
                    paramList1[0] = new PassDataToSql("@company_id", strCompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
                    paramList1[1] = new PassDataToSql("@model_id", model_id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
                    using (DataTable _dtModel = daClass.GetDtFromQryWithParam(sql, paramList1))
                    {
                        foreach (DataRow dr in _dtModel.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["SnoPrefix"].ToString().Trim()) || !string.IsNullOrEmpty(dr["SnoSuffix"].ToString().Trim()))
                            {
                                defaultNo += dr["SnoPrefix"].ToString();
                               // defaultNo += dr["trans_id"].ToString();
                                defaultNo += dr["SnoSuffix"].ToString();
                                defaultNo += "|" + dr["SnoStartFrom"].ToString();
                            }
                        }
                    }
                }
                if (defaultNo == string.Empty)
                {
                    foreach (DataRow dr in _dtProduct.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["SnoPrefix"].ToString().Trim()) || !string.IsNullOrEmpty(dr["SnoSuffix"].ToString().Trim()))
                        {
                            defaultNo += dr["SnoPrefix"].ToString();
                           // defaultNo += dr["ProductId"].ToString();
                            defaultNo += dr["SnoSuffix"].ToString();
                            defaultNo += "|" + dr["SnoStartFrom"].ToString();
                        }
                    }
                }
            }
        }
        catch
        {
            defaultNo = string.Empty;
        }
        return defaultNo;
    }

    public DataTable getAutoProductSerial(string product_id, int qty, string default_no,string strCompId)
    {
        DataTable _dt = new DataTable();
        _dt.Columns.Add("product_id");
        _dt.Columns.Add("sno");
        try
        {
            string default_sno = default_no.Split('|')[0].ToString();
            int noStartFrom = 0;
            int.TryParse(default_no.Split('|')[1].ToString(), out noStartFrom);
            PassDataToSql[] paramList = new PassDataToSql[2];
            paramList[0] = new PassDataToSql("@company_id", strCompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            //paramList[1] = new PassDataToSql("@product_id", product_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@serial_no", default_sno + '%', PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            int snoLen = default_sno.Length+1;
            
           
            int lastSno = 0;
            string sql = "select max(cast(substring(SerialNo," + snoLen + ",len(SerialNo)) as int)) as lastSno from Inv_StockBatchMaster where Company_Id=@company_id and SerialNo like @serial_no   and ISNUMERIC(substring(SerialNo,"+ snoLen + ",len(SerialNo))) =1";
            using (DataTable dtExistingRec = daClass.GetDtFromQryWithParam(sql, paramList))
            {
                if (dtExistingRec.Rows.Count > 0 && !string.IsNullOrEmpty(dtExistingRec.Rows[0]["lastSno"].ToString()))
                {
                    lastSno = int.Parse(dtExistingRec.Rows[0]["lastSno"].ToString());
                }
            }
            if (lastSno == 0 && noStartFrom > 0)
            {
                lastSno = noStartFrom-1;
            }
            qty += lastSno;
            lastSno++;
            for (int i = lastSno; i <= qty; i++)
            {
                DataRow dr = _dt.Rows.Add();
                dr["product_id"] = product_id;
                dr["sno"] = default_sno + i.ToString();
            }

        }
        catch (Exception ex)
        {
            _dt = null;
        }
        return _dt;
    }

    //for import invoice excel data for e-commerce order
    public int GetProductIdByCodeAndAlternetId(string strCompanyId, string strBrandId, string strProductId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@CompanyId", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@BrandId", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ProductId", strProductId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "SELECT Inv_ProductMaster.ProductId FROM Inv_ProductMaster WHERE Inv_ProductMaster.Company_Id = @CompanyId AND Inv_ProductMaster.Brand_Id = @BrandId AND Inv_ProductMaster.IsActive = 'True' AND (Inv_ProductMaster.ProductCode = @ProductId OR Inv_ProductMaster.AlternateId1 = @ProductId OR Inv_ProductMaster.AlternateId2 = @ProductId OR Inv_ProductMaster.AlternateId3 =@ProductId)";
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

    public DataTable GetProductIdByScanning(string strCompanyId, string strVoucherId, string strScanText)
    {
        //int productId = 0;
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@CompanyId", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@VoucherId", strVoucherId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@StrScanText", strScanText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        string sql = "SELECT pm.ProductId, pm.ProductCode, pm.EProductName, pd.SystemQuantity,'0' as rackId,'' as serial from Inv_PhysicalDetail pd inner join Inv_PhysicalHeader ph on ph.trans_id= pd.Header_Id inner join Inv_ProductMaster pm on pd.ProductId=pm.ProductId WHERE ph.Trans_Id=@VoucherId and pm.Company_Id = @CompanyId AND (pm.ProductCode=@StrScanText or pm.AlternateId1 = @StrScanText or  pm.AlternateId2 = @StrScanText or pm.AlternateId3 = @StrScanText) AND ph.IsActive='true' and pm.IsActive = 'True'";
        using (DataTable _dt = daClass.GetDtFromQryWithParam(sql, paramList))
        {
            if (_dt.Rows.Count>0)
            {
                return _dt;
            }
            else
            {
                sql = "SELECT pm.ProductId, pm.ProductCode, pm.EProductName, pd.SystemQuantity,'0' as rackId,@StrScanText as serial FROM (select sbm.ProductId from Inv_StockBatchMaster sbm where sbm.SerialNo=@StrScanText and sbm.isactive='true' and sbm.company_id=@CompanyId group by sbm.ProductId having sum(case when sbm.InOut='I' then sbm.quantity else -sbm.Quantity end)>=1)sbm inner join Inv_ProductMaster pm on sbm.ProductId=pm.ProductId inner join Inv_PhysicalDetail pd on pd.ProductId=sbm.ProductId inner join Inv_PhysicalHeader ph on ph.Trans_Id=pd.Header_Id where ph.IsActive='true' and ph.Trans_Id=@VoucherId";
                using (DataTable _dt1 = daClass.GetDtFromQryWithParam(sql, paramList))
                {
                    return _dt1;
                }

            }
            
        }
        //return productId;
    }
   
    public DataTable getProductTableForAdvancFilter(string strCompanyId, string strBrandId, string strLocationId,string strFinanceYearId,string whereClause)
    {
        try
        {
            PassDataToSql[] paramList = new PassDataToSql[4];
            paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@Finance_Year", strFinanceYearId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            string sql = @"SELECT
      PM.ProductCode,
      PM.Description,
      PM.ProductId,
      PM.EProductName,
      Inv_ModelMaster.Model_No,
      PM.UnitId,
      PM.MaintainStock,
      UM.Unit_Name,
      PCM.Category_Name,
      PBM.Brand_Name,
      PC.CategoryId,
      PB.PBrandId,
      PM.ModelName,
      CASE
        WHEN PM.ItemType = 'S' THEN 'Stockable'
        WHEN PM.ItemType = 'NS' THEN 'Non Stockable'
        WHEN PM.ItemType = 'A' THEN 'Assemble'
        WHEN PM.ItemType = 'K' THEN 'Kit'
      END AS ItemType,
      CASE
        WHEN PM.MaintainStock = 'FE' THEN 'FIFO EXPIRY DATE'
        WHEN PM.MaintainStock = 'FM' THEN 'FIFO MANUFACTURING DATE'
        WHEN PM.MaintainStock = 'LE' THEN 'LIFO EXPIRY DATE'
        WHEN PM.MaintainStock = 'LM' THEN 'LIFO MANUFACTURING DATE'
        WHEN PM.MaintainStock = 'SNO' THEN 'Serial No.'
        WHEN PM.MaintainStock = 'BW' THEN 'Batch Wise'
        WHEN PM.MaintainStock = 'NM' THEN 'No Method'
        ELSE '-'
      END AS InventoryType,
      PM.CostPrice,
      Set_companymaster.Currency_Id AS CurrencyId,
      PM.AlternateId1,
      PM.AlternateId2,
      PM.AlternateId3,
      PM.Field1 AS AlternateId,
      '0' AS Exchanage_Rate,
      Inv_StockDetail.Quantity AS StockQuantity,
      CASE
        WHEN Inv_ParameterMaster.ParameterValue = 1 THEN Pm.SalesPrice1
        WHEN Inv_ParameterMaster.ParameterValue = 2 THEN Pm.SalesPrice2
        WHEN Inv_ParameterMaster.ParameterValue = 3 THEN Pm.SalesPrice3
        ELSE '0'
      END AS SalesPrice,
      SUBSTRING(PM.EProductName, 0, 120) AS ShortProductName
    FROM Inv_ProductMaster AS PM
    LEFT JOIN Inv_Product_Brand AS PB
      ON PM.ProductId = PB.ProductId
    LEFT JOIN Inv_Product_Category AS PC
      ON PM.ProductId = PC.ProductId
    LEFT JOIN Inv_UnitMaster AS UM
      ON pm.UnitId = UM.Unit_Id
    LEFT JOIN Inv_Product_CategoryMaster AS PCM
      ON PC.CategoryId = PCM.Category_Id
    LEFT JOIN Inv_ProductBrandMaster AS PBM
      ON PB.PBrandId = PBM.PBrandId
    LEFT JOIN Inv_ParameterMaster
      ON Inv_ParameterMaster.ParameterName = 'Sales Price'
      AND Inv_ParameterMaster.Company_Id = @Company_Id
      AND Inv_ParameterMaster.Brand_Id = @Brand_Id
      AND Inv_ParameterMaster.Location_Id = @Location_Id
    LEFT JOIN Inv_StockDetail
      ON Inv_StockDetail.Company_Id = @Company_Id
      AND Inv_StockDetail.Brand_Id = @Brand_Id
      AND Inv_StockDetail.Location_Id = @Location_Id
      AND Inv_StockDetail.ProductId = PM.ProductId
      AND Inv_StockDetail.Finance_Year_Id = @Finance_Year
    LEFT JOIN Set_companymaster
      ON Set_companymaster.Company_Id = @Company_Id
    LEFT JOIN Inv_ModelMaster
      ON Inv_ModelMaster.Trans_Id = pm.ModelNo
    WHERE PM.Company_Id = @Company_Id
    AND PM.Brand_Id = @Brand_Id
    AND PM.IsActive = 'True'
    AND PM.Field1 = ' '
    AND Pm.Field3 = 'True'";
            sql += " and " + whereClause;

            using (DataTable _dt = daClass.GetDtFromQryWithParam(sql, paramList))
            {
                return _dt;
            }
        }
        catch(Exception ex)
        {
            return null;
        }
    }
    
}
