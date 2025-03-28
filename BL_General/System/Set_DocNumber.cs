using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Set_DocNumber
/// </summary>
public class Set_DocNumber
{
    DataAccessClass Daclass = null;
    private string _strConString = string.Empty;
    public Set_DocNumber(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        Daclass = new DataAccessClass(strConString);
        _strConString = strConString;
    }
    public int InsertDocumentNumber(string strCompany_Id, string strBrandId, string strLocationId, string Module_Id, string Object_Id, string Prefix, string Suffix, string CompId, string BrandId, string LocationId, string DeptId, string EmpId, string Year, string Month, string Day, string IsUse, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strFinancialYearValue, string strAutoGenerateNumber, string strAutoGenerateNumberMonth, string strColour, string strSize)
    {
        PassDataToSql[] paramList = new PassDataToSql[34];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Prefix", Prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Suffix", Suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CompId", CompId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@LocationId", LocationId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@DeptId", DeptId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Day", Day, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@IsUse", IsUse, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[15] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[29] = new PassDataToSql("@FinancialYearValue", strFinancialYearValue, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@AutoGenerateNumber", strAutoGenerateNumber, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@AutoGenerateNumberMonth", strAutoGenerateNumberMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[32] = new PassDataToSql("@Colour", strColour, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Size", strSize, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        Daclass.execute_Sp("sp_Set_DocNumber_Insert", paramList);
        return Convert.ToInt32(paramList[14].ParaValue);
    }
    public int UpdateDocumentNumber(string strCompany_Id, string strBrandId, string strLocationId, string Trans_Id, string Module_Id, string Object_Id, string Prefix, string Suffix, string CompId, string BrandId, string LocationId, string DeptId, string EmpId, string Year, string Month, string Day, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate, string strFinancialYearValue, string strAutoGenerateNumber, string strAutoGenerateNumberMonth, string strColour,  string strSize)
    {
        PassDataToSql[] paramList = new PassDataToSql[32];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Prefix", Prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Suffix", Suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CompId", CompId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@LocationId", LocationId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@DeptId", DeptId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Day", Day, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[14] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[27] = new PassDataToSql("@FinancialYearValue", strFinancialYearValue, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@AutoGenerateNumber", strAutoGenerateNumber, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@AutoGenerateNumberMonth", strAutoGenerateNumberMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[30] = new PassDataToSql("@Colour", strColour, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@Size", strSize, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        Daclass.execute_Sp("sp_Set_DocNumber_Update", paramList);
        return Convert.ToInt32(paramList[13].ParaValue);
    }
    public int DeleteDocumentNumber(string strCompany_Id, string strBrandId, string strLocationId, string strTrans_Id, string Module_Id, string strObject_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Object_Id", strObject_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        Daclass.execute_Sp("sp_Set_DocNumber_DeleteRow", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }
    public int DeleteDocumentNumberbyModuleIdandObjectId(string strCompany_Id, string strBrandId, string strLocationId, string strTrans_Id, string Module_Id, string strObject_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Object_Id", strObject_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        Daclass.execute_Sp("sp_Set_DocNumber_DeleteRow", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }
    public DataTable GetDocumentNumberAll(string strCompanyId, string strBrandId, string strLocationId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_Set_DocNumber_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetDocumentNumberAll(string strCompanyId, string strBrandId, string strLocationId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_Set_DocNumber_SelectRow", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetDocumentNumberAll(string StrCompanyId, string strBrandId, string strLocationId, string ModuleId, string ObjectId, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", StrCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", ObjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", ModuleId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_Set_DocNumber_SelectRow", paramList);
        return dtInfo;
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetDocumentNumberAll(string StrCompanyId, string strBrandId, string strLocationId, string ModuleId, string ObjectId, string OpType, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", StrCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", ObjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", ModuleId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_Set_DocNumber_SelectRow", paramList, ref trns);
        return dtInfo;
    }
    public string GetDocumentNo(bool IsFormateOnly, string CompanyId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string LastTransId, string strBrandId, string strLocId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string strCompId = string.Empty;
        try
        {
            string sql = "select company_id from set_brandmaster where brand_id='" + strBrandId + "'";
            strCompId = Daclass.get_SingleValue(sql);
        }
        catch
        {

        }

        string StrDocument = string.Empty;
        DataTable dt = new DataTable();
        if (IsUseCompIdInWhere)
        {
            dt = GetDocumentNumberAll(CompanyId, strBrandId, strLocId, ModuleId, ObjectId, "2");
        }
        else
        {
            dt = GetDocumentNumberAll(strCompId, strBrandId, strLocId, ModuleId, ObjectId, "2");
        }

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }

            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += strCompId;
            }

            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += strBrandId;
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += strLocId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += strDepartmentId;
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += "-" + Common.GetEmployeeCode(strEmpId, _strConString, strCompId) + "-";
            }


            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
            }
            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
            }

            if (Convert.ToBoolean(dr["FinancialYearValue"].ToString()))
            {
                string strFinanceMonth = "0";
                if (dr["AutoGenerateNumberMonth"].ToString() != "")
                {
                    strFinanceMonth = dr["AutoGenerateNumberMonth"].ToString();
                }

                DataTable dtFinanceDetail = Daclass.return_DataTable("select * from Ac_Finance_Year_Info where Company_Id='" + strCompId + "' and Status='Open'");
                if (dtFinanceDetail.Rows.Count > 0)
                {
                    string strFinanceFromDate = dtFinanceDetail.Rows[0]["From_Date"].ToString();
                    DateTime dtFromDate = DateTime.Parse(strFinanceFromDate);
                    int StartYear = dtFromDate.Year;

                    DateTime dtTodayDate = DateTime.Now;
                    int CurrentYear = dtTodayDate.Year;
                    int CurrentMonth = dtTodayDate.Month;

                    if (strFinanceMonth != "0")
                    {
                        if (CurrentMonth > int.Parse(strFinanceMonth))
                        {
                            int NewYear = StartYear + 1;

                            StrDocument += StartYear + "-" + NewYear.ToString().Substring(2, 2);
                        }
                        else if (CurrentMonth == int.Parse(strFinanceMonth))
                        {
                            int NewYear = StartYear + 1;
                            StrDocument += StartYear + "-" + NewYear.ToString().Substring(2, 2);
                        }
                        else if (CurrentMonth < int.Parse(strFinanceMonth))
                        {
                            StartYear = StartYear - 1;
                            StrDocument += StartYear + "-" + CurrentYear.ToString().Substring(2, 2);
                        }
                    }
                    else
                    {

                    }
                }
            }


            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }



            if (!IsFormateOnly)
            {
                if (StrDocument != "")
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
                else
                {
                    StrDocument += (LastTransId + 1).ToString();

                }
            }
            else
            {
                StrDocument += "";
            }
        }
        return StrDocument.Trim();
    }


    public string GetDocumentNo(bool IsFormateOnly, string CompanyId, string LocationId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string LastTransId, string strBrandId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string strCompId = string.Empty;
        try
        {
            string sql = "select company_id from set_brandmaster where brand_id='" + strBrandId + "'";
            strCompId = Daclass.get_SingleValue(sql);
        }
        catch
        {

        }

        string StrDocument = string.Empty;
        DataTable dt = new DataTable();
        if (IsUseCompIdInWhere)
        {
            dt = GetDocumentNumberAll(CompanyId, strBrandId, LocationId, ModuleId, ObjectId, "2");
        }
        else
        {
            dt = GetDocumentNumberAll(strCompId, strBrandId, LocationId, ModuleId, ObjectId, "2");
        }

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += strCompId;
            }

            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += strBrandId;
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += LocationId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += strDepartmentId;
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += "-" + Common.GetEmployeeCode(strEmpId, _strConString, strCompId) + "-";
            }


            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
            }
            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
            }

            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }

            if (!IsFormateOnly)
            {
                if (StrDocument != "")
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
                else
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
            }
            else
            {
                StrDocument += "";
            }
        }
        return StrDocument.Trim();
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public string GetDocumentNo(bool IsFormateOnly, string CompanyId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string LastTransId, ref SqlTransaction trns, string strBrandId, string strLocId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();
        if (IsUseCompIdInWhere)
        {
            dt = GetDocumentNumberAll(CompanyId, strBrandId, strLocId, ModuleId, ObjectId, "2", ref trns);
        }
        else
        {
            dt = GetDocumentNumberAll(CompanyId, strBrandId, strLocId, ModuleId, ObjectId, "2", ref trns);
        }

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += CompanyId;
            }

            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += strBrandId;
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += strLocId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += strDepartmentId;
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += strEmpId;
            }


            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
            }
            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
            }

            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }

            if (!IsFormateOnly)
            {
                if (StrDocument != "")
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
                else
                {
                    StrDocument += (LastTransId + 1).ToString();

                }
            }
            else
            {
                StrDocument += "";
            }
        }
        return StrDocument.Trim();
    }

    public string GetDocumentNo(string TransId, string CompanyId, string ModuleId, string ObjectId, string ModelId, string manufacturingBrandId, string CategoryId, string supplierId, string strBrandId, string strLocId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();

        dt = new DataView(GetDocumentNumberAll(CompanyId, ModuleId, ObjectId), "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += CompanyId;
            }
            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += strBrandId;
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += strLocId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += strDepartmentId;
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += strEmpId;
            }

            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
            }

            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
            }
            if (Convert.ToBoolean(dr["Field1"].ToString()))
            {
                StrDocument += ModelId;
            }
            if (Convert.ToBoolean(dr["Field2"].ToString()))
            {
                StrDocument += CategoryId;
            }
            if (Convert.ToBoolean(dr["Field3"].ToString()))
            {
                StrDocument += manufacturingBrandId;
            }
            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }
            else
            {
                StrDocument += "-";
            }
        }
        return StrDocument;
    }

    public string GetDocumentNo(string TransId, string CompanyId, string ModuleId, string ObjectId, string ModelId, string manufacturingBrandId, string CategoryId, string supplierId, ref SqlTransaction trns, string strBrandId, string strLocId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();

        dt = new DataView(GetDocumentNumberAll(CompanyId, ModuleId, ObjectId, ref trns), "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += CompanyId;
            }
            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += strBrandId;
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += strLocId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += strDepartmentId;
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += strEmpId;
            }

            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
            }

            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
            }
            if (Convert.ToBoolean(dr["Field1"].ToString()))
            {
                StrDocument += ModelId;
            }
            if (Convert.ToBoolean(dr["Field2"].ToString()))
            {
                StrDocument += CategoryId;
            }
            if (Convert.ToBoolean(dr["Field3"].ToString()))
            {
                StrDocument += manufacturingBrandId;
            }
            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }
            else
            {
                StrDocument += "-";
            }
        }
        return StrDocument;
    }


    public string GetDocumentNoProduct(string TransId, string CompanyId, string ModuleId, string ObjectId, string ModelId, string strColourId, string strSizeId, string manufacturingBrandId, string CategoryId, string supplierId, ref SqlTransaction trns, string strBrandId, string strLocId, string strDepartmentId, string strEmpId, string strTimeZoneId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();

        //dt = new DataView(GetDocumentNumberAll(CompanyId, ModuleId, ObjectId, ref trns), "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(GetDocumentNumberAll(CompanyId, strBrandId, strLocId, ModuleId, ObjectId, "2", ref trns), "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();

        

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (Convert.ToBoolean(dr["Field1"].ToString()) && Convert.ToBoolean(dr["Colour"].ToString()) && Convert.ToBoolean(dr["Size"].ToString()))
            {
                if (Convert.ToBoolean(dr["Field1"].ToString()))
                {
                    StrDocument += ModelId;
                }                                
                if (Convert.ToBoolean(dr["Colour"].ToString()))
                {
                    StrDocument += "-" + strColourId;
                }
                if (Convert.ToBoolean(dr["Size"].ToString()))
                {
                    StrDocument += "-" + strSizeId;
                }
            }
            else
            {
                if (dr["Prefix"].ToString() != "")
                {
                    StrDocument += dr["Prefix"].ToString();
                }
                if (Convert.ToBoolean(dr["CompId"].ToString()))
                {
                    StrDocument += "-" + CompanyId;
                }
                if (Convert.ToBoolean(dr["BrandId"].ToString()))
                {
                    StrDocument += "-" + strBrandId;
                }

                if (Convert.ToBoolean(dr["LocationId"].ToString()))
                {
                    StrDocument += "-" + strLocId;
                }

                if (Convert.ToBoolean(dr["DeptId"].ToString()))
                {
                    StrDocument += "-" + strDepartmentId;
                }

                if (Convert.ToBoolean(dr["EmpId"].ToString()))
                {
                    StrDocument += "-" + strEmpId;
                }

                if (Convert.ToBoolean(dr["Year"].ToString()))
                {
                    StrDocument += "-" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year.ToString();
                }

                if (Convert.ToBoolean(dr["Month"].ToString()))
                {
                    StrDocument += "-" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month.ToString();
                }

                if (Convert.ToBoolean(dr["Day"].ToString()))
                {
                    StrDocument += "-" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day.ToString();
                }
                if (Convert.ToBoolean(dr["Field1"].ToString()))
                {
                    StrDocument += "-" + ModelId;
                }
                if (Convert.ToBoolean(dr["Field2"].ToString()))
                {
                    StrDocument += "-" + CategoryId;
                }
                if (Convert.ToBoolean(dr["Field3"].ToString()))
                {
                    StrDocument += "-" + manufacturingBrandId;
                }
                if (Convert.ToBoolean(dr["Colour"].ToString()))
                {
                    StrDocument += "-" + strColourId;
                }
                if (Convert.ToBoolean(dr["Size"].ToString()))
                {
                    StrDocument += "-" + strSizeId;
                }
                if (dr["Suffix"].ToString() != "")
                {
                    StrDocument += dr["Suffix"].ToString();
                }
                else
                {
                    StrDocument += "-";
                }
            }          
        }
        return StrDocument;
    }
}
