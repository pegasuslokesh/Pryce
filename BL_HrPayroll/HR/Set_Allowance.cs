﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Set_Allowance
/// </summary>
public class Set_Allowance
{
    DataAccessClass daClass = null;
    public Set_Allowance(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int InsertAllowance(string CompanyId, string Allowance, string Allowance_L, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate,string strCalculationType, string strCalculationValue)
    {
        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance", Allowance, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance_L", Allowance_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[10] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[16] = new PassDataToSql("@Calculation_Type", strCalculationType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Calculation_Value", strCalculationValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_AllowanceMaster_Insert", paramList);
        return Convert.ToInt32(paramList[15].ParaValue);
    }
    public int UpdateAllowance(string CompanyId, string strAllowance_Id, string Allowance, string Allowance_L, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strModifiedBy, string strModifiedDate, string strCalculationType, string strCalculationValue)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", strAllowance_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", Allowance, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Allowance_L", Allowance_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[15] = new PassDataToSql("@Calculation_Type", strCalculationType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Calculation_Value", strCalculationValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_AllowanceMaster_Update", paramList);
        return Convert.ToInt32(paramList[14].ParaValue);
    }
    public int DeleteAllowance(string CompanyId, string strAllowance_Id, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", strAllowance_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_AllowanceMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public DataTable GetAllowanceTrueAll(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllowanceTrueAll(string CompanyId,string location_id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", location_id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }


    public DataTable GetAllowanceTrueAll(string CompanyId,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList,ref trns);
        return dtInfo;
    }


    public DataTable GetAllowanceAll(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllowanceTruebyId(string CompanyId, string strAllowance_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", strAllowance_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetAllowanceTruebyId(string CompanyId, string strAllowance_Id,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", strAllowance_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList,ref trns);
        return dtInfo;
    }
    public DataTable GetAllowanceByAllowance(string CompanyId, string Allowance)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", Allowance, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllowanceFalseAll(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetDistinctAllowance(string CompanyId, string prefixlength)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Allowance_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Allowance", prefixlength, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@location_id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_AllowanceMaster_SelectRow", paramList);
        return dtInfo;

    }
    
}
