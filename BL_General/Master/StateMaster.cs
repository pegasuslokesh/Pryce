using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;
/// <summary>
/// Summary description for StateMaster
/// </summary>
public class StateMaster
{
    public DataAccessClass daClass = null;

    public string TransId { get; set; }
    public string Country_Id { get; set; }
    public string State_Name { get; set; }
    public string State_Name_Local { get; set; }
    public string IsActive { get; set; }
    

    public StateMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }

    public int InsertState(string Country_Id, string State_Name, string State_Name_Local, string strCreatedBy, string strModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Country_Id", Country_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@State_Name", State_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@State_Name_Local", State_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_sys_statemaster_Insert", paramList);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public int InsertState( string Country_Id, string State_Name, string State_Name_Local, string strCreatedBy, string strModifiedBy,ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Country_Id", Country_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@State_Name", State_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@State_Name_Local", State_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_sys_statemaster_Insert", paramList,ref trans);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public int UpdateState(string Trans_Id,string country_id,  string State_Name, string State_Name_Local, string strModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Country_Id", country_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@State_Name", State_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@State_Name_Local", State_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_sys_statemaster_Insert", paramList);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public int UpdateState(string Trans_Id,string country_id, string State_Name, string State_Name_Local, string strModifiedBy, ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Country_Id", country_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@State_Name", State_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@State_Name_Local", State_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_sys_statemaster_Insert", paramList, ref trans);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public DataTable GetAllStateByCountryId(string Country_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@country_id", Country_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetAllIsActiveTrueState()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@country_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetAllIsActiveFalseState()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@country_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "False", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetAllStateByTrans_Id(string trans_id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@country_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", trans_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetAllStateByPrefixText(string PrefixText,string countryId="0")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@country_id", countryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@PrefixText", PrefixText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_selectPreText", paramList))
            return dtInfo;
    }

    public int SetStateIsActiveTrue(string Trans_Id,string UserId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@trans_id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@modifiedBy", UserId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_sys_statemaster_Update", paramList);
        return 1;
    }

    public int SetStateIsActiveFalse(string Trans_Id, string UserId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@trans_id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@isActive", "False", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@modifiedBy", UserId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_sys_statemaster_Update", paramList);
        return 1;
    }

    public DataTable GetAllStateByCountryIdNTrans_Id(string Country_Id,string Trans_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@country_id", Country_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_statemaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetStateDataByCountryNStateName(string Country_Name, string State_Name)
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select sys_statemaster.*, sys_countrymaster.Country_Name from sys_statemaster left join sys_countrymaster on sys_countrymaster.Country_Id= sys_statemaster.Country_Id where sys_countrymaster.Country_Name= '" + Country_Name + "' and sys_statemaster.State_Name= '" + State_Name + "' and sys_statemaster.isactive='true'"))
            return dtInfo;
    }

    public DataTable GetStateDataByCountryNStateName(string Country_Name, string State_Name,ref SqlTransaction trans)
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select sys_statemaster.*, sys_countrymaster.Country_Name from sys_statemaster left join sys_countrymaster on sys_countrymaster.Country_Id= sys_statemaster.Country_Id where sys_countrymaster.Country_Name= '" + Country_Name + "' and sys_statemaster.State_Name= '" + State_Name + "' and sys_statemaster.isactive='true'", ref trans))
            return dtInfo;
    }

    public string GetStateIdByStateNameNCountryName(string State_Name,string Country_Name,ref SqlTransaction trans)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Sys_StateMaster.Trans_Id from Sys_StateMaster left join Sys_CountryMaster on Sys_CountryMaster.Country_Id=Sys_StateMaster.Country_Id where  Sys_StateMaster.State_Name= '" + State_Name + "' and Sys_CountryMaster.Country_Name='"+ Country_Name + "' and sys_statemaster.isactive='true'", ref trans);
        if(dtInfo.Rows.Count>0)
        {
            return dtInfo.Rows[0][0].ToString();
        }
        return "";
    }

    public string GetStateIdByStateNameNCountryName(string State_Name, string Country_Name)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Sys_StateMaster.Trans_Id from Sys_StateMaster left join Sys_CountryMaster on Sys_CountryMaster.Country_Id=Sys_StateMaster.Country_Id where  Sys_StateMaster.State_Name= '" + State_Name + "' and Sys_CountryMaster.Country_Name='" + Country_Name + "' and sys_statemaster.isactive='true'");
        if (dtInfo.Rows.Count > 0)
        {
            return dtInfo.Rows[0][0].ToString();
        }
        return "";
    }
    public string CheckStateExistOrNot(string CountryId, string State_Name)
    {
        string data = daClass.get_SingleValue("select count(trans_id) from Sys_StateMaster  where State_Name='" + State_Name + "' and Country_Id='" + CountryId + "' and isactive='true'");
        data = data == "@NOTFOUND@" ? "" : data;
        return data;
    }
    public string GetStateIdFromCountryIdNStateName(string CountryId, string State_Name)
    {
        string data = daClass.get_SingleValue("select top 1 trans_id from Sys_StateMaster  where State_Name='" + State_Name + "' and Country_Id='" + CountryId + "' and isactive='true'");
        data = data == "@NOTFOUND@" ? "" : data;
        return data;
    }
}


