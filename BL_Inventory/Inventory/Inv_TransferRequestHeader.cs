﻿using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Inv_TransferRequestHeader
/// </summary>
public class Inv_TransferRequestHeader
{
    DataAccessClass daClass = null;
    Common cm = null;
	public Inv_TransferRequestHeader(string strConString)
	{
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        cm = new Common(strConString);
	}
    public string getAutoId()
    {
        DataTable dt = GetTransferRequestHeader();
        string I = "0";
        if (dt.Rows.Count == 0)
        {
            I = daClass.get_SingleValue("SELECT IDENT_CURRENT('Inv_TransferRequestHeader')");
        }
        else
        {
            I = (Convert.ToInt32(daClass.get_SingleValue("SELECT IDENT_CURRENT('Inv_TransferRequestHeader')")) + 1).ToString();
        }

        return I;
    }
    public int InsertTransferRequestHeader(string CompanyId, string brandId, string Locationid, string RequestNo, string RequestDate, string Remark, string Post, string status,string RequestLocationID,  string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[22];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
      
        paramList[3] = new PassDataToSql("@RequestNo", RequestNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@TDate", RequestDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Post", Post, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
       
        paramList[7] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
       
        paramList[14] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[20] = new PassDataToSql("@RequestLocationID", RequestLocationID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        
        daClass.execute_Sp("sp_Inv_TransferRequestHeader_Insert", paramList);
        return Convert.ToInt32(paramList[19].ParaValue);
    }
    //created for rollback Trsnaction
    //24-02-2016
    //by jitendra upadhyay
    public int InsertTransferRequestHeader(string CompanyId, string brandId, string Locationid, string RequestNo, string RequestDate, string Remark, string Post, string status, string RequestLocationID, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[22];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@RequestNo", RequestNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@TDate", RequestDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Post", Post, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[14] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[20] = new PassDataToSql("@RequestLocationID", RequestLocationID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_TransferRequestHeader_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[19].ParaValue);
    }


    public void Updatecode(string Id, string Code)
    {
        cm.UpdateCodeForDocumentNo("Inv_TransferRequestHeader", "RequestNo", "Trans_Id", Id, Code);

    }
    //created for rollback Trsnaction
    //24-02-2016
    //by jitendra upadhyay

    public void Updatecode(string Id, string Code, ref SqlTransaction trns)
    {
        cm.UpdateCodeForDocumentNo("Inv_TransferRequestHeader", "RequestNo", "Trans_Id", Id, Code,ref trns);

    }

    public int UpdateTransferRequestHeader(string Trans_Id, string CompanyId, string brandId, string Locationid, string RequestNo, string RequestDate, string Remark, string Post, string Status, string RequestLocationID, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
       
        paramList[3] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@RequestNo", RequestNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@TDate", RequestDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Post", Post, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[19] = new PassDataToSql("@RequestLocationID", RequestLocationID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Status",Status, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
       

        daClass.execute_Sp("sp_Inv_TransferRequestHeader_Update", paramList,ref trns);
        return Convert.ToInt32(paramList[18].ParaValue);
    }


    public int DeleteTransferRequestHeader(string CompanyId, string brand_ID, string Location_Id, string Trans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@brand_ID", brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Inv_TransferRequestHeader_RowStatus", paramList);
        return Convert.ToInt32(paramList[7].ParaValue);
    }
    public DataTable GetAllRecord(string CompanyId, string BrandId, string LocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id",BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id",LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }

    //created for rollback Trsnaction
    //24-02-2016
    //by jitendra upadhyay

    public DataTable GetAllRecord(string CompanyId, string BrandId, string LocationId, string TransId, ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList,ref trns))
            return dt;
    }
    public DataTable GetAllRecord_True(string CompanyId, string BrandId, string LocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id",BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id",LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id",TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }
    public DataTable GetAllRecord_False(string CompanyId, string BrandId, string LocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id",BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id",LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id",TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }
    public DataTable GetRecordUsingTransId(string CompanyId, string BrandId, string LocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id",BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id",LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id",TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }
    //created for rollback Trsnaction
    //16-02-2016
    //by jitendra upadhyay

    public DataTable GetRecordUsingTransId(string CompanyId, string BrandId, string LocationId, string TransId, ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList,ref trns))
            return dt;
    }
    public DataTable GetTransferRequestHeader()
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id","0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id","0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "5", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }

    public DataTable GetTransferRequestHeader(ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "5", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList, ref trns))
            return dt;
    }
    public DataTable GetRecordUsingTransIdByRequestLocation(string CompanyId, string BrandId, string ReqLocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", ReqLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "7", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }
    public DataTable GetAllRecord_TrueByRequestLocation(string CompanyId, string BrandId, string ReqLocationId, string TransId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", ReqLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "6", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (dt = daClass.Reuturn_Datatable_Search("sp_Inv_TransferRequestHeader_SelectRow", paramList))
            return dt;
    }
    public int UpdateStatusInTransferRequestHeader(string Trans_Id, string CompanyId, string brandId, string Locationid,  string ModifiedBy, string ModifiedDate,string Status)
    {

        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[7] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        daClass.execute_Sp("sp_Inv_TransferRequestHeader_UpdateStatus", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }
    //created for rollback Transaction by jitendra upadhyay 
    //on 29-03-2016


    public int UpdateStatusInTransferRequestHeader(string Trans_Id, string CompanyId, string brandId, string Locationid, string ModifiedBy, string ModifiedDate, string Status,ref SqlTransaction trns)    
    {

        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Locationid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[7] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        daClass.execute_Sp("sp_Inv_TransferRequestHeader_UpdateStatus", paramList,ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }


}
