using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for hr_empgratuityDetail
/// </summary>
public class hr_empgratuityDetail
{
    DataAccessClass daClass = null;
    public hr_empgratuityDetail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }


    public int InsertRecord(string stremp_gratuity_id, string stryear_count, string stryear_name, string strgratuity_days, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate,string gratuity_days_physical, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[9];
        paramList[0] = new PassDataToSql("@emp_gratuity_id", stremp_gratuity_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@year_count", stryear_count, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@year_name", stryear_name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@gratuity_days", strgratuity_days, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Reference_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[8] = new PassDataToSql("@gratuity_days_physical", gratuity_days_physical, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_hr_empgratuitydetail_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[7].ParaValue);
    }


    public int Deleterecord(string stremp_gratuity_id,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@emp_gratuity_id", stremp_gratuity_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_hr_empgratuitydetail_Deleterow", paramList,ref trns);
        return Convert.ToInt32(paramList[1].ParaValue);
    }



    public DataTable GetRecordBy_GratuiryPlanId(string stremp_gratuity_id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@emp_gratuity_id", stremp_gratuity_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_hr_empgratuitydetail_SelectRow", paramList);
        return dtInfo;
    }







}