using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Country_Currency
/// </summary>
public class Country_Currency
{
    DataAccessClass daClass = null;
	public Country_Currency(string strConString)
	{
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
	}

    public int InsertCountry_Currency(string CountryId, string CurrencyId, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("SP_InsertCountryCurrency", paramList);
        return Convert.ToInt32(paramList[14].ParaValue);
    }

    public int InsertCountry_Currency(string CountryId, string CurrencyId, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("SP_InsertCountryCurrency", paramList, ref trns);
        return Convert.ToInt32(paramList[14].ParaValue);
    }

    public int DeleteCountryCurrency(string CountryId)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input); ;
        paramList[1] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Sp_DeleteCountry_Currency", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }

    public DataTable GetCurrencyByCountryId(string CountryId,string OpType)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("Get_CurrencyByCountryId", paramList))
            return dtInfo;
    }

    //created for rollback Trsnaction
    //16-02-2016
    //by jitendra upadhyay


    public DataTable GetCurrencyByCountryId(string CountryId, string OpType,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("Get_CurrencyByCountryId", paramList, ref trns))
            return dtInfo;
    }
    public DataTable getTimeZoneIdByCurrencyId(string currencyId)
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select Sys_CountryMaster.Field1,Sys_CountryMaster.Country_Name,Sys_CountryMaster.Country_Id from Sys_CountryMaster inner join Sys_Country_Currency on Sys_Country_Currency.Country_Id = Sys_CountryMaster.Country_Id where Sys_Country_Currency.Currency_Id = '"+currencyId+"' and Sys_CountryMaster.IsActive = 'true'"))
            return dtInfo;
    }
    public string getTimeZoneIdNameByCurrencyId(string currencyId)
    {
        string timeZoneData = "";
        timeZoneData = daClass.get_SingleValue("select top 1 Sys_CountryMaster.Field1 from Sys_CountryMaster inner join Sys_Country_Currency on Sys_Country_Currency.Country_Id = Sys_CountryMaster.Country_Id where Sys_Country_Currency.Currency_Id = '" + currencyId + "' and Sys_CountryMaster.IsActive = 'true'");
        timeZoneData = timeZoneData == "@NOTFOUND@" ? "Arab Standard Time" : timeZoneData;
        return timeZoneData;
    }

    public string getCurrencyByLocation(string locationId)
    {
        string currencyId = "";
        currencyId = daClass.get_SingleValue("select Field1 from set_locationmaster where Location_Id='"+locationId+"' and IsActive='true'");
        currencyId = currencyId == "@NOTFOUND@" ? "0" : currencyId;
        return currencyId;
    }
    public string getCurrencyByLocation(string locationId,ref SqlTransaction trans)
    {
        string currencyId = "";
        currencyId = daClass.get_SingleValue("select Field1 from set_locationmaster where Location_Id='" + locationId + "' and IsActive='true'",ref trans);
        currencyId = currencyId == "@NOTFOUND@" ? "0" : currencyId;
        return currencyId;
    }
}
