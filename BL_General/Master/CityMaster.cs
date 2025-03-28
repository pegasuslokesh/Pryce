using System;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;

/// <summary>
/// Summary description for CityMaster
/// </summary>
public class CityMaster
{
    public DataAccessClass daClass = null;
    public string TransId { get; set; }
    public string Country_Id { get; set; }
    public string State_Id { get; set; }
    public string State_Name { get; set; }
    public string City_Name { get; set; }
    public string City_Name_Local { get; set; }

    public CityMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }

    public int InsertCity(string State_Id, string City_Name, string City_Name_Local, string strCreatedBy, string strModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@State_Id", State_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@City_Name", City_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@City_Name_Local", City_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CityMaster_insert", paramList);
        return Convert.ToInt32(paramList[10].ParaValue);
    }


    public int InsertCity(string State_Id, string City_Name, string City_Name_Local, string strCreatedBy, string strModifiedBy, ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@State_Id", State_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@City_Name", City_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@City_Name_Local", City_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CityMaster_insert", paramList, ref trans);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public int UpdateCity(string Trans_Id, string state_Id, string City_Name, string City_Name_Local, string strModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@State_Id", state_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@City_Name", City_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@City_Name_Local", City_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CityMaster_insert", paramList);
        return Convert.ToInt32(paramList[10].ParaValue);
    }


    public int UpdateCity(string Trans_Id, string City_Name, string City_Name_Local, string strModifiedBy, ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[11];

        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@State_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@City_Name", City_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@City_Name_Local", City_Name_Local, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CityMaster_insert", paramList, ref trans);
        return Convert.ToInt32(paramList[10].ParaValue);
    }

    public DataTable GetAllCityByStateId(string State_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@state_id", State_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CityMaster_Select", paramList))
            return dtInfo;
    }

    public DataTable GetAllIsActiveTrueCities()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@state_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CityMaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetAllIsActiveFalseCities()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@state_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "False", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CityMaster_Select", paramList))
            return dtInfo;
    }
    public DataTable GetCityByTrans_Id(string trans_id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@state_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@trans_id", trans_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@op_type", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CityMaster_Select", paramList))
            return dtInfo;
    }


    public DataTable GetAllCityByPrefixText(string PrefixText, string StateId = "0")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@state_id", StateId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@PrefixText", PrefixText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_sys_citymaster_selectPreText", paramList))
            return dtInfo;
    }

    public int SetCityIsActiveTrue(string Trans_Id, string UserId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@trans_id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@isActive", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@modifiedBy", UserId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_sys_CityMaster_Update", paramList);
        return 1;
    }

    public int SetCityIsActiveFalse(string Trans_Id, string UserId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@trans_id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@isActive", "False", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@modifiedBy", UserId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_sys_CityMaster_Update", paramList);
        return 1;
    }
    public DataTable GetCityDataByCountry_State_CityName(string Country_Name, string State_Name, string City_Name)
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select Sys_CityMaster.*,sys_statemaster.State_Name,Sys_CountryMaster.Country_Name,Sys_CountryMaster.country_id from Sys_CityMaster left join sys_statemaster on sys_statemaster.Trans_Id=Sys_CityMaster.State_Id left join Sys_CountryMaster on Sys_CountryMaster.Country_Id=sys_statemaster.Country_Id where Sys_CityMaster.City_Name='" + City_Name + "' and sys_statemaster.State_Name='" + State_Name + "' and Sys_CountryMaster.Country_Name='" + Country_Name + "' and Sys_CityMaster.isactive='true'"))
            return dtInfo;
    }
    public DataTable GetCityDataByCountry_State_CityName(string Country_Name, string State_Name, string City_Name, ref SqlTransaction trans)
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select Sys_CityMaster.*,sys_statemaster.State_Name,Sys_CountryMaster.Country_Name,Sys_CountryMaster.country_id from Sys_CityMaster left join sys_statemaster on sys_statemaster.Trans_Id=Sys_CityMaster.State_Id left join Sys_CountryMaster on Sys_CountryMaster.Country_Id=sys_statemaster.Country_Id where Sys_CityMaster.City_Name='" + City_Name + "' and sys_statemaster.State_Name='" + State_Name + "' and Sys_CountryMaster.Country_Name='" + Country_Name + "' and Sys_CityMaster.isactive='true'", ref trans))
            return dtInfo;
    }
    public string CheckCityExistOrNot(string StateId, string City_Name)
    {
        string data = daClass.get_SingleValue("select count(trans_id) from Sys_CityMaster  where City_Name='" + City_Name + "' and State_Id='" + StateId + "' and isactive='true'");
        data = data == "@NOTFOUND@" ? "" : data;
        return data;
    }
    public string GetCityIdFromStateIdNCityName(string StateId, string City_Name)
    {
        string data = daClass.get_SingleValue("select top 1 trans_id from Sys_CityMaster  where City_Name='" + City_Name + "' and State_Id='" + StateId + "' and isactive='true'");
        data = data == "@NOTFOUND@" ? "" : data;
        return data;
    }

}