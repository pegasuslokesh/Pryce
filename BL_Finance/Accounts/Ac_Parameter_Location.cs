using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Ac_Parameter_Location
/// </summary>
public class Ac_Parameter_Location
{
    DataAccessClass daClass = null;
    public Ac_Parameter_Location(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int InsertRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strParamName, string strParamvalue, string strDescription, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[19];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", strParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Value", strParamvalue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Description", strDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Location_Insert", paramList);
        return Convert.ToInt32(paramList[18].ParaValue);
    }

    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public int InsertRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strParamName, string strParamvalue, string strDescription, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[19];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", strParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Value", strParamvalue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Description", strDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Location_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[18].ParaValue);
    }
    public int DeleteRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Location_DeleteRow", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }
    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public int DeleteRecord(string strCompany_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Location_DeleteRow", paramList, ref trns);
        return Convert.ToInt32(paramList[3].ParaValue);
    }
    public DataTable GetParameterMasterById(string Company_Id, string strBrand_Id, string strLocation_Id, string Parameter_Loc_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", Parameter_Loc_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetParameterMasterAllTrue(string Company_Id, string strBrand_Id, string strLocation_Id,string conString="")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList,conString);

        return dtInfo;
    }

    
    
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public DataTable GetParameterMasterAllTrue(string Company_Id, string strBrand_Id, string strLocation_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetParameterValue_By_ParameterName(string Company_Id, string strBrand_Id, string strLocation_Id, string ParameterName, string conString="")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList, conString);

        return dtInfo;
    }
    public string GetParameterValue_By_ParameterNameValue(string Company_Id, string strBrand_Id, string strLocation_Id, string ParameterName)
    {
        string str = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            str = dtInfo.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            str = "False";
        }
        return str;
    }
    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public DataTable GetParameterValue_By_ParameterName(string Company_Id, string strBrand_Id, string strLocation_id, string ParameterName, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetParameterValue_By_NameAndValue(string ParameterName, string strParameterValue)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", strParameterValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList);

        return dtInfo;
    }

    //Created for Rollback Transaction
    //16-06-2016
    //By Lokesh
    public DataTable GetParameterValue_By_NameAndValue(string ParameterName, string strParameterValue, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", strParameterValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetParameterValue_By_ParameterNameforBrand(string Company_Id, string strBrand_Id, string ParameterName)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Parameter_Loc_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Param_Value", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("Optype", "5", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_Location_SelectRow", paramList);

        return dtInfo;
    }
    public string getBankAccounts(string strComapnyId, string strBrandId, string strLocationId)
    {
        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = GetParameterValue_By_ParameterName(strComapnyId, strBrandId, strLocationId, "BankAccount");
        if (dtAccount.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccount.Rows.Count; i++)
            {
                if (strAccountId == "")
                {
                    strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountId = "0";
        }
        return strAccountId;
    }

    public string getBankAccounts(string strComapnyId, string strBrandId, string strLocationId,ref SqlTransaction trns)
    {
        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = GetParameterValue_By_ParameterName(strComapnyId, strBrandId, strLocationId, "BankAccount",ref trns);
        if (dtAccount.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccount.Rows.Count; i++)
            {
                if (strAccountId == "")
                {
                    strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountId = "0";
        }
        return strAccountId;
    }
    public string getCashFlowLocationGroup(string strComapnyId, string strBrandId, string strLocationId)
    {
        //Get Location by Cash Flow.
        string strCashFlowLocationGroup = "0";
        DataTable dtCashLocation = GetParameterValue_By_NameAndValue("CashFlowLocation", strLocationId);
        if (dtCashLocation.Rows.Count > 0)
        {
            for (int l = 0; l < dtCashLocation.Rows.Count; l++)
            {
                if (strCashFlowLocationGroup == "0")
                {
                    strCashFlowLocationGroup = dtCashLocation.Rows[l]["Location_Id"].ToString();
                }
                else
                {
                    strCashFlowLocationGroup = strCashFlowLocationGroup + "," + dtCashLocation.Rows[l]["Location_Id"].ToString();
                }
            }
        }
        return strCashFlowLocationGroup;
    }
    public string getCashFlowAccounts(string strComapnyId, string strBrandId, string strLocationId, string strLocationGroup)
    {
        string strAccountIdCash = "0";
        if (strLocationId != "0")
        {
            DataTable dtAccountCash = GetParameterValue_By_ParameterNameforBrand(strComapnyId, strBrandId, "CashFlowAccount");
            dtAccountCash = new DataView(dtAccountCash, "Location_Id in (" + strLocationGroup + ")", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAccountCash.Rows.Count > 0)
            {
                for (int i = 0; i < dtAccountCash.Rows.Count; i++)
                {
                    if (strAccountIdCash == "0")
                    {
                        strAccountIdCash = dtAccountCash.Rows[i]["Param_Value"].ToString();
                    }
                    else
                    {
                        strAccountIdCash = strAccountIdCash + "," + dtAccountCash.Rows[i]["Param_Value"].ToString();
                    }
                }
            }
            else
            {
                strAccountIdCash = "0";
            }
        }
        return strAccountIdCash;

    }

    public Boolean ValidateVoucherForCashFlow(string strComapnyId, string strBrandId, string strLocationId, string strVoucherId)
    {
        Boolean isCashFlowAccount = false;
        Boolean isValidate = true;
        string strVoucherAccounts = string.Empty;
        string strVoucherDate = string.Empty;

        DataTable dtVoucherheader = daClass.return_DataTable("select Ac_Voucher_Header.Voucher_Date,Ac_Voucher_Detail.account_no from Ac_Voucher_Header inner join Ac_Voucher_Detail on Ac_Voucher_Header.Trans_Id = Ac_Voucher_Detail.Voucher_No where  Ac_Voucher_Header.Trans_Id=" + strVoucherId + " and Ac_Voucher_Header.company_id='" + strComapnyId + "' and Ac_Voucher_Header.brand_id='" + strBrandId + "' and Ac_Voucher_Header.location_id='" + strLocationId + "'");


        foreach (DataRow dr in dtVoucherheader.Rows)
        {
            strVoucherDate = dr["Voucher_Date"].ToString();
            if (dr["account_no"].ToString() != "0")
            {
                if (strVoucherAccounts == "")
                {

                    strVoucherAccounts = dr["account_no"].ToString();
                }
                else
                {
                    strVoucherAccounts = strVoucherAccounts + "," + dr["account_no"].ToString();
                }
            }

        }
        dtVoucherheader.Dispose();


        string strLocationIdCash = getCashFlowLocationGroup(strComapnyId, strBrandId, strLocationId);
        if (strLocationId == "0") return true;

        string strAccountIdCash = getCashFlowAccounts(strComapnyId, strBrandId, strLocationId, strLocationIdCash);
        if (strLocationId == "0") return true;
        foreach (string strAccountNo in strVoucherAccounts.Split(','))
        {
            if (strAccountIdCash.Split(',').Contains(strAccountNo))
            {
                isCashFlowAccount = true;
                continue;
            }
        }
        if (isCashFlowAccount == true)
        {
            string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + strComapnyId + "' and Brand_Id='" + strBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True'";
            DataTable dtCashflowDetail = daClass.return_DataTable(strsql);
            if (dtCashflowDetail.Rows.Count > 0)
            {
                string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();

                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                if (dtFinalDate >= DateTime.Parse(strVoucherDate))
                {
                    isValidate = false;
                }

            }

        }
        return isValidate;
    }
    public Boolean ValidateVoucherForCashFlow(string strComapnyId, string strBrandId, string strLocationId, string strVoucherAccounts, string strVoucherDate)
    {
        Boolean isCashFlowAccount = false;
        Boolean isValidate = true;
        string strLocationIdCash = getCashFlowLocationGroup(strComapnyId, strBrandId, strLocationId);
        if (strLocationId == "0") return true;

        string strAccountIdCash = getCashFlowAccounts(strComapnyId, strBrandId, strLocationId, strLocationIdCash);
        if (strLocationId == "0") return true;
        foreach (string strAccountNo in strVoucherAccounts.Split(','))
        {
            if (strAccountIdCash.Split(',').Contains(strAccountNo))
            {
                isCashFlowAccount = true;
                continue;
            }
        }
        if (isCashFlowAccount == true)
        {
            string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + strComapnyId + "' and Brand_Id='" + strBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True' and  IsActive='True'";
            DataTable dtCashflowDetail = daClass.return_DataTable(strsql);
            if (dtCashflowDetail.Rows.Count > 0)
            {
                string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();

                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                if (dtFinalDate >= DateTime.Parse(strVoucherDate))
                {
                    isValidate = false;
                }

            }

        }
        return isValidate;
    }

    public Boolean ValidateVoucherForCashFlow(string strComapnyId, string strVoucherId)
    {
        Boolean isCashFlowAccount = false;
        Boolean isValidate = true;
        string strVoucherAccounts = string.Empty;
        string strVoucherDate = string.Empty;
        string strBrandId = string.Empty;
        string strLocationId = string.Empty;
        string strSql = "Select Ac_Voucher_Header.Brand_id,Ac_Voucher_Header.Location_id,Ac_Voucher_Header.Voucher_Date,STUFF(( SELECT ',' + cast(account_no as varchar) FROM Ac_Voucher_Detail where Ac_Voucher_Detail.Voucher_No='19355' FOR XML PATH('') ), 1, 1, '') as account_nos from Ac_Voucher_Header where Ac_Voucher_Header.Trans_Id=" + strVoucherId + " and Ac_Voucher_Header.company_id='" + strComapnyId + "'";
        //DataTable dtVoucherheader = daClass.return_DataTable("select Ac_Voucher_Header.Brand_id,Ac_Voucher_Header.Location_id,Ac_Voucher_Header.Voucher_Date,Ac_Voucher_Detail.account_no from Ac_Voucher_Header inner join Ac_Voucher_Detail on Ac_Voucher_Header.Trans_Id = Ac_Voucher_Detail.Voucher_No where  Ac_Voucher_Header.Trans_Id=" + strVoucherId + " and Ac_Voucher_Header.company_id='" + strComapnyId + "'");
        DataTable dtVoucherheader = daClass.return_DataTable(strSql);
        strBrandId = dtVoucherheader.Rows[0]["Brand_id"].ToString();
        strLocationId = dtVoucherheader.Rows[0]["Location_id"].ToString();
        strVoucherDate = dtVoucherheader.Rows[0]["Voucher_Date"].ToString();
        strVoucherAccounts = dtVoucherheader.Rows[0]["account_nos"].ToString();
        //foreach (DataRow dr in dtVoucherheader.Rows)
        //{
        //    strVoucherDate = dr["Voucher_Date"].ToString();
            
        //    if (dr["account_no"].ToString() != "0")
        //    {
        //        if (strVoucherAccounts == "")
        //        {

        //            strVoucherAccounts = dr["account_no"].ToString();
        //        }
        //        else
        //        {
        //            strVoucherAccounts = strVoucherAccounts + "," + dr["account_no"].ToString();
        //        }
        //    }

        //}
        dtVoucherheader.Dispose();


        string strLocationIdCash = getCashFlowLocationGroup(strComapnyId, strBrandId, strLocationId);
        if (strLocationId == "0") return true;

        string strAccountIdCash = getCashFlowAccounts(strComapnyId, strBrandId, strLocationId, strLocationIdCash);
        if (strAccountIdCash == "0") return true;
        foreach (string strAccountNo in strVoucherAccounts.Split(','))
        {
            if (strAccountIdCash.Split(',').Contains(strAccountNo))
            {
                isCashFlowAccount = true;
                continue;
            }
        }
        if (isCashFlowAccount == true)
        {
            string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + strComapnyId + "' and Brand_Id='" + strBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True'";
            DataTable dtCashflowDetail = daClass.return_DataTable(strsql);
            if (dtCashflowDetail.Rows.Count > 0)
            {
                string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();

                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                if (dtFinalDate >= DateTime.Parse(strVoucherDate))
                {
                    isValidate = false;
                }

            }

        }
        return isValidate;
    }

    public Boolean ValidateVoucherReconcileForCashFlow(string strComapnyId, string strBrandId, string strLocationId, string strVoucherId,string strReconcileDate)
    {
        Boolean isCashFlowAccount = false;
        Boolean isValidate = true;
        string strVoucherAccounts = string.Empty;
        
        DataTable dtVoucherheader = daClass.return_DataTable("select Ac_Voucher_Header.Voucher_Date,Ac_Voucher_Detail.account_no from Ac_Voucher_Header inner join Ac_Voucher_Detail on Ac_Voucher_Header.Trans_Id = Ac_Voucher_Detail.Voucher_No where  Ac_Voucher_Header.Trans_Id=" + strVoucherId + " and Ac_Voucher_Header.company_id='" + strComapnyId + "'");


        foreach (DataRow dr in dtVoucherheader.Rows)
        {
            
            if (dr["account_no"].ToString() != "0")
            {
                if (strVoucherAccounts == "")
                {

                    strVoucherAccounts = dr["account_no"].ToString();
                }
                else
                {
                    strVoucherAccounts = strVoucherAccounts + "," + dr["account_no"].ToString();
                }
            }

        }
        dtVoucherheader.Dispose();


        string strLocationIdCash = getCashFlowLocationGroup(strComapnyId, strBrandId, strLocationId);
        if (strLocationId == "0") return true;

        string strAccountIdCash = getCashFlowAccounts(strComapnyId, strBrandId, strLocationId, strLocationIdCash);
        if (strLocationId == "0") return true;
        foreach (string strAccountNo in strVoucherAccounts.Split(','))
        {
            if (strAccountIdCash.Split(',').Contains(strAccountNo))
            {
                isCashFlowAccount = true;
                continue;
            }
        }
        if (isCashFlowAccount == true)
        {
            string strsql = "SELECT MAX (CF_Date)  FROM Ac_CashFlow_Header Where Company_Id ='" + strComapnyId + "' and Brand_Id='" + strBrandId + "' and Location_Id in (" + strLocationIdCash + ") and ReconcileStatus='True'";
            DataTable dtCashflowDetail = daClass.return_DataTable(strsql);
            if (dtCashflowDetail.Rows.Count > 0)
            {
                string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();

                DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                if (dtFinalDate >= DateTime.Parse(strReconcileDate))
                {
                    isValidate = false;
                }

            }

        }
        return isValidate;
    }
}