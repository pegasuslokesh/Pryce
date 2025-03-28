using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;

/// <summary>
/// Summary description for CurrencyMaster
/// </summary>
public class CurrencyMaster
{
    DataAccessClass daClass = null;
    public CurrencyMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    //This Function Created By Rahul Sharma on date 02-12-2023
    public string SetCountryExchangeReate(string CurrencyId)
    {
        string BaseCurrency=daClass.get_SingleValue("Select Currency_Code from Sys_CurrencyMaster where Currency_ID='"+CurrencyId+"'");
        string apiKey = "a92449f8a18c3396b992f29e";
        string baseCurrency = BaseCurrency;
        string apiUrl = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{baseCurrency}";
        string Response;
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Response = responseBody;
                    return Response;
                   
                    // Handle the response data as needed
                }
                else
                {
                    // Response="$"Error: {response.StatusCode} - {response.ReasonPhrase}"";
                    return Response = "False";
                }
            }
        }
        catch (Exception ex)
        {
            return Response = "False";
        }
       

    }

    public int InsertCurrencyMaster(string ECurrencyName, string LCurrencyName, string CurrencyCode, string CurrencySymbol, string CurrencyValue, string IsBaseCurrency, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[19];


        paramList[0] = new PassDataToSql("@Currency_Name", ECurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name_L", LCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Currency_Code", CurrencyCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Currency_Symbol", CurrencySymbol, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Is_BaseCurrency", IsBaseCurrency, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Currency_Value", CurrencyValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CurrencyMaster_Insert", paramList);
        return Convert.ToInt32(paramList[18].ParaValue);
    }

    public int InsertCurrencyMaster(string ECurrencyName, string LCurrencyName, string CurrencyCode, string CurrencySymbol, string CurrencyValue, string IsBaseCurrency, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[19];


        paramList[0] = new PassDataToSql("@Currency_Name", ECurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name_L", LCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Currency_Code", CurrencyCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Currency_Symbol", CurrencySymbol, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Is_BaseCurrency", IsBaseCurrency, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Currency_Value", CurrencyValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CurrencyMaster_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[18].ParaValue);
    }

    public int UpdateCurrencyMaster(string Currency_Id, string ECurrencyName, string LCurrencyName, string CurrencyCode, string CurrencySymbol, string CurrencyValue, string IsBaseCurrency, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[18];


        paramList[0] = new PassDataToSql("@Currency_Name", ECurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name_L", LCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Currency_Code", CurrencyCode, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[15] = new PassDataToSql("@Currency_Symbol", CurrencySymbol, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Is_BaseCurrency", IsBaseCurrency, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Currency_Value", CurrencyValue, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Sys_CurrencyMaster_Update", paramList);
        return Convert.ToInt32(paramList[14].ParaValue);
    }
    public int DeleteCurrencyMaster(string CurrencyId, string CountryId, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Country_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Sys_CurrencyMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public DataTable GetCurrencyMasterById(string CurrencyId,string conString="")
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList,conString))

            return dtInfo;
    }
    //created for rollback Trsnaction
    //11-03-2016
    //by jitendra upadhyay
    public DataTable GetCurrencyMasterById(string CurrencyId, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList, ref trns))

            return dtInfo;
    }
    public DataTable GetCurrencyMaster()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))

            return dtInfo;
    }

    public DataTable GetCurrencyMaster(ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList, ref trns))

            return dtInfo;
    }
    public DataTable Get_ActiveCurrencyMaster()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))

            return dtInfo;
    }
    public DataTable Get_ActiveCurrencyMaster(ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList, ref trns))

            return dtInfo;
    }
    public DataTable Get_CurrencyMaster()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))
            return dtInfo;
    }


    public DataTable GetCurrencyMasterInactive()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))
            return dtInfo;
    }
    public DataTable GetCurrencyMasterByCurrencyName(string strCurrencyName, string CountryId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Currency_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", strCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))
            return dtInfo;
    }

    public DataTable GetCurrencyMasterByCurrencyName(string strCurrencyName, string CountryId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Currency_Id", CountryId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", strCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using(dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList, ref trns))
        return dtInfo;
    }

    public DataTable GetCurrencyMasterByCurrencyNames(string strCurrencyName)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Currency_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", strCurrencyName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))
            return dtInfo;
    }

    public DataTable GetDistinctCurrencyName(string strPrefixText)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList);

        using (dtInfo = new DataView(dtInfo, "Currency_Name Like '" + strPrefixText + "%'", "", DataViewRowState.CurrentRows).ToTable())
            return dtInfo;
    }

    // Add by ghanshyam suthar on 23/11/2017
    public DataTable GetDecimalCount(string Currency_ID, string Currency_Code, string Currency_Name, string Country_Id, string Country_Name, string Country_Code, string Optype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Currency_ID", Currency_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Code", Currency_Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Currency_Name", Currency_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Country_Id", Country_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Country_Name", Country_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Country_Code", Country_Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Optype", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("GetDecimalCount", paramList))
            return dtInfo;
    }

    public DataTable GetCurrencyListForDDL()
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select Sys_CurrencyMaster.Currency_ID,Sys_CurrencyMaster.Currency_Name   From Sys_CurrencyMaster  Left Join Sys_Country_Currency ON Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.Currency_Id Left Join Sys_CountryMaster ON   Sys_Country_Currency.Country_Id = Sys_CountryMaster.Country_Id Where ISNULL(Sys_CurrencyMaster.IsActive, 0) = 1"))
            return dtInfo;
    }


    public string GetDecimalCountByCurrencyId(string strCurrencyId)
    {
        string strdecimalcount = string.Empty;

        strdecimalcount = daClass.return_DataTable("select ISNULL( Sys_Country_Currency.field1,0) from Sys_Country_Currency left join Sys_CurrencyMaster on Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.currency_id where Sys_CurrencyMaster.Currency_ID='" + strCurrencyId.Trim() + "'  ").Rows[0][0].ToString();

        if (strdecimalcount == "")
        {
            strdecimalcount = "2";
        }

        return strdecimalcount;
    }

    public static string GetCurrencyNameByCurrencyId(string strCurrencyId,string strConString)
    {
        CurrencyMaster objCurrency = new CurrencyMaster(strConString);
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            using (DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId))
            {
                if (dtCName.Rows.Count > 0)
                {
                    strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
                }
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    // returns only currency name added by divya on 12/8/2018
    public string GetCurrencyNameById(string CurrencyId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Currency_Id", CurrencyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Currency_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_CurrencyMaster_SelectRow", paramList))
            return dtInfo.Rows[0]["Currency_Name"].ToString();
    }
}
