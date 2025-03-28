using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Ems_ContactMaster
/// </summary>
public class Ems_ContactMaster
{
    DataAccessClass daClass = null;
    public Ems_ContactMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int InsertContactMaster(string Code, string Name, string Name_L, string strCivil_Id, string strDep_Id, string strDesignation_Id, string strReligion_Id, string strContact_Company_Id, string strIsEmail, string strIsSMS, string Status, string Is_Reseller, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strRemarks, string strParentContactId)
    {
        PassDataToSql[] paramList = new PassDataToSql[27];
        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name_L", Name_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Civil_Id", strCivil_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", strDep_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Designation_Id", strDesignation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Religion_Id", strReligion_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Company_Id", strContact_Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsEmail", strIsEmail, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@IsSMS", strIsSMS, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[11] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[24] = new PassDataToSql("@Is_Reseller", Is_Reseller, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Remarks", strRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@ParentContactId",strParentContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_Insert", paramList);
        return Convert.ToInt32(paramList[23].ParaValue);
    }
    public int InsertContactMaster(string Code, string Name, string Name_L, string strCivil_Id, string strDep_Id, string strDesignation_Id, string strReligion_Id, string strContact_Company_Id, string strIsEmail, string strIsSMS, string Status, string Is_Reseller, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strRemarks,string strParentContactId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[27];
        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name_L", Name_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Civil_Id", strCivil_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", strDep_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Designation_Id", strDesignation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Religion_Id", strReligion_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Company_Id", strContact_Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsEmail", strIsEmail, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@IsSMS", strIsSMS, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[24] = new PassDataToSql("@Is_Reseller", Is_Reseller, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Remarks", strRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@ParentContactId", strParentContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[23].ParaValue);
    }
    public int UpdateContactMaster(string Trans_Id, string Code, string Name, string Name_L, string strCivil_Id, string strDep_Id, string strDesignation_Id, string strReligion_Id, string strContact_Company_Id, string strIsEmail, string strIsSMS, string status, string Is_Reseller, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate, string strRemarks, string strParentContactId)
    {
        PassDataToSql[] paramList = new PassDataToSql[26];

        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name_L", Name_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Civil_Id", strCivil_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", strDep_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Designation_Id", strDesignation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Religion_Id", strReligion_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Company_Id", strContact_Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsEmail", strIsEmail, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@IsSMS", strIsSMS, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[22] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Is_Reseller", Is_Reseller, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Remarks", strRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ParentContactId", strParentContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_Update", paramList);
        return Convert.ToInt32(paramList[21].ParaValue);
    }


    public int UpdateContactMaster(string Trans_Id, string Code, string Name, string Name_L, string strCivil_Id, string strDep_Id, string strDesignation_Id, string strReligion_Id, string strContact_Company_Id, string strIsEmail, string strIsSMS, string status, string Is_Reseller, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate, string strRemarks, string strParentContactId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[26];

        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name_L", Name_L, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Civil_Id", strCivil_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", strDep_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Designation_Id", strDesignation_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Religion_Id", strReligion_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Company_Id", strContact_Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsEmail", strIsEmail, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@IsSMS", strIsSMS, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[22] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Is_Reseller", Is_Reseller, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Remarks", strRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ParentContactId", strParentContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_Update", paramList,ref trns);
        return Convert.ToInt32(paramList[21].ParaValue);
    }

    public int DeleteContactMaster(string Trans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }

    public int DeleteContactMaster_By_ParentId(string Trans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Ems_ContactMaster_RowStatus", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }
    public int UpdateParentContact(string OldProjectId, string NewProjectId, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@OldParentContactId", OldProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@NewParentContactId", NewProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ems_ContactMaster_UpdateParentProjectId", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }


    public void UpdateContactMaster(string Trans_Id, string Code)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_EMS_ContactMaster_UpdateCode", paramList);
    }
    public void UpdateContactMaster(string Trans_Id, string Code,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_EMS_ContactMaster_UpdateCode", paramList,ref trns);
    }

    public DataTable GetContactAllData()
    {
        
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return dtInfo;
        }
            
    }
    public DataTable GetContactAllData(ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }
    public DataTable GetContactTrueAllData(string RefCompanyId, string Status)
    {
        using (DataTable dtInfo = new DataView(GetContactTrueAllData(), "Company_Id=" + RefCompanyId + " and Status='" + Status + "'", "Name Asc", DataViewRowState.CurrentRows).ToTable())
        { 
            return dtInfo;
        }
    }
    public DataTable GetContactTrueAllData()
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetAutoCompleteContactTrueAllData(string prefixText)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return new DataView(dtInfo, "Filtertext like'%" + prefixText + "%'", "Name Asc", DataViewRowState.CurrentRows).ToTable();
        }
    }
    public DataTable GetContactFalseAllData()
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetContactTrueById(string TransId)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow_By_ContactId", paramList))
        {
            return dtInfo;
        }
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public DataTable GetContactTrueById(string TransId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow_By_ContactId", paramList, ref trns))
        {
            return dtInfo;
        }
    }

    public DataTable GetContactTrueAutoCompleteById(string Prefix)
    {
        DataTable dtInfo = new DataTable();
        try
        {
            dtInfo = new DataView(GetContactTrueAllData(), "Code like'%" + Prefix + "%'", "Name Asc", DataViewRowState.CurrentRows).ToTable();
        }

        catch
        {

        }
        return dtInfo;
    }
    public DataTable GetContactByContactName(string Name)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList);
        return dtInfo;
    }


    //created for rollback Transaction
    public DataTable GetContactByContactName(string Name, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList, ref trns))
        {
            return dtInfo;
        }
    }



    public bool IsUsedInInventory(string CompanyId, string BrandId, string Id, string GroupId, string Optype)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@CompanyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Id", Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@GroupId", GroupId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Retvel ", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_checkCustomerOrSupplierIsUser", paramList);
        return Convert.ToBoolean(paramList[4].ParaValue);

    }
    public bool IsUsedInInventory(string CompanyId, string BrandId, string Id, string GroupId, string Optype,ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@CompanyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Id", Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@GroupId", GroupId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Retvel ", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Optype", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_checkCustomerOrSupplierIsUser", paramList,ref trns);
        return Convert.ToBoolean(paramList[4].ParaValue);

    }

    public DataTable GetContactNameandEmailIdAllData()
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowNameEmail", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetContactNameandEmailIdAllDataByGroup(string Add_Type)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Add_Type", @Add_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowNameEmailByGroup", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetAddressByRefType_Id(string Ref_Type, string Ref_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Add_Type", Ref_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Add_Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_Address_Customer_By_CustomerId", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetAddressByRefType_Id(string Ref_Type, string Ref_Id,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Add_Type", Ref_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Add_Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_Address_Customer_By_CustomerId", paramList))
        {
            return dtInfo;
        }
        
    }

    public DataTable GetContactByContactGroupId(string Group_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", Group_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return dtInfo;
        }
    }

    //Code For Excel Update Insert
    public int InsertContactMasterByExcel(string Code, string Name, string Status, string strField1, string strField2, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[13] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Ems_ContactMaster_UpdateByExcel", paramList);
        return Convert.ToInt32(paramList[12].ParaValue);
    }
    public int UpdateContactMasterByExcel(string Trans_Id, string Code, string Name, string status, string strField1, string strField2, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[15];

        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field6", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field7", DateTime.Now.ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@IsActive", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@CreatedBy", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@CreatedDate", DateTime.Now.ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[13] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        daClass.execute_Sp("sp_Ems_ContactMaster_UpdateByExcel", paramList);
        return Convert.ToInt32(paramList[12].ParaValue);
    }

    public DataTable GetContactByCode(string Code)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow_ByContactCode", paramList))
        {
            return dtInfo;
        }
     
    }
    public DataTable GetContactByName(string Name)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow_ByContactCode", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetContactForMainGrid(string condition)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", condition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }

    }


    public DataTable GetContactForMainGridByComp(string condition)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", condition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }

    }
    public DataTable GetContactForMainGridByGroupId(string condition)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", condition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }
    }


    public DataTable GetAllContact_By_ParentId(string strParentId)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition",strParentId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }

    }
    public DataTable GetContactTrueAllDataForGRid()
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }

    }

    public DataTable GetContactTrueAllDataForGRidByIndex(string condition,string BatchNo, string PageSize, string TotalCount, string IsFavorite, string SearchField, string SearchType, string SearchValue, string SearchGroup,string UserID)
    {
        PassDataToSql[] paramList = new PassDataToSql[16];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", condition, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@BatchNo", BatchNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@PageSize", PageSize, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@TotalCount", TotalCount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsFavorite", IsFavorite, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@SearchField", SearchField, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@SearchType", SearchType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@SearchValue", SearchValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@SearchGroup", SearchGroup, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@User_ID", UserID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
       
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGridByIndex", paramList))
        {
            return dtInfo;
        }

    }

    public int updateVerifyStatus(string ContactId, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@ContactId", ContactId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModfiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Ems_ContactMaster_VerifyStatus", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public DataTable GetAllCompanyList()
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Code", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@GroupId", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@condition", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRowForGrid", paramList))
        {
            return dtInfo;
        }

    }

    public int SaveFavoriteContact(string UserId, string ContactId, string Type)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@UserId", UserId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ContactId", ContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Type", Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ems_FavoriteContact", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    //end Code
    public DataTable GetTop1FinanceContactPersonName(string company,string brand,string location, string Companytrans_id)
    {
        try
        {
            DataTable dtdept = new DataTable();
            dtdept = daClass.return_DataTable("SELECT top 1 (case when param_value is null then 0 else param_value end) FROM dbo.Ac_Parameter_Location where Company_Id='"+ company + "' and Brand_Id='" + brand + "' and Location_Id in (SELECT CAST(Value AS int) FROM F_Split('" + location + "', ',')) and Param_Name='Finance_Department_id'");

            DataTable dtInfo = new DataTable();
            dtInfo = daClass.return_DataTable("select top 1 Trans_Id,name from Ems_ContactMaster where Company_Id='" + Companytrans_id + "' and Dep_Id='" + dtdept.Rows[0][0].ToString() + "'");
            return dtInfo;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetAllContactNameByCompanyID(string companyid)
    {
        try
        {            
            DataTable dtInfo = new DataTable();
            dtInfo = daClass.return_DataTable("select Trans_Id as ContactPersonID,Name,Company_Id from Ems_ContactMaster where Company_Id IN (SELECT CAST(Value AS int) AS Company FROM F_Split('" + companyid+"', ','))");
            if(dtInfo.Rows.Count==0)
            {
                DataTable dtInfo1 = daClass.return_DataTable("select Trans_Id as ContactPersonID,Name,Company_Id from Ems_ContactMaster where trans_id IN (SELECT CAST(Value AS int) AS Company FROM F_Split('" + companyid + "', ','))");
                return dtInfo1;
            }
            return dtInfo;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetAllContactNameAndNoByCompanyID(string companyid)
    {
        try
        {
            using (DataTable dtInfo = daClass.return_DataTable("select Ems_ContactMaster.Trans_Id as ContactPersonID,Ems_ContactMaster.Name,Ems_ContactMaster.Company_Id,(case when Set_ContactNo_Master.Phone_no ='' then Ems_ContactMaster.Field2 else  Set_ContactNo_Master.Phone_no end ) as Phone_no from Ems_ContactMaster  left join Set_ContactNo_Master on Set_ContactNo_Master.Ref_table_pk=Ems_ContactMaster.Trans_Id where Ems_ContactMaster.Company_Id IN (SELECT CAST(Value AS int) AS Company FROM F_Split('" + companyid + "', ','))"))
            {
                return dtInfo;
            }
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetAllContactNameAndNoByCompanyIDNCustName(string companyid,string CustName)
    {
        try
        {
            using (DataTable dtInfo = daClass.return_DataTable("select Ems_ContactMaster.Trans_Id as ContactPersonID,Ems_ContactMaster.Name,Ems_ContactMaster.Company_Id,(case when Set_ContactNo_Master.Phone_no ='' then Ems_ContactMaster.Field2 else  Set_ContactNo_Master.Phone_no end ) as Phone_no from Ems_ContactMaster  left join Set_ContactNo_Master on Set_ContactNo_Master.Ref_table_pk=Ems_ContactMaster.Trans_Id where Ems_ContactMaster.Company_Id IN (SELECT CAST(Value AS int) AS Company FROM F_Split('" + companyid + "', ',')) and Ems_ContactMaster.Name='" + CustName + "'"))
            {
                return dtInfo;
            }
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetCustomerAsPerFilterText(string prefixText)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@ContactCompany_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@StrText", prefixText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectPreText", paramList))
        {
            return dtInfo;
        }
    }
    //This will only individual contacts
    public DataTable GetContactAsPerFilterText(string prefixText,string ContactCompanyId="0")
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@ContactCompany_id", ContactCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@StrText", prefixText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectPreText", paramList))
        {
            return dtInfo;
        }
    }
    //This will return company as well as individual contacts
    public DataTable GetAllContactAsPerFilterText(string prefixText, string ContactCompanyId = "0")
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@ContactCompany_id", ContactCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@StrText", prefixText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectPreText", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetAllContactNameByCompanyID(string companyid,string status)
    {
        try
        {
            using (DataTable dtInfo = daClass.return_DataTable("select Trans_Id,Name from Ems_ContactMaster where Company_Id ='" + companyid + "' and status='" + status + "' and IsActive='true'"))
            {
                return dtInfo;
            }
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetAllContactNameByAddressId(string AddressId)
    {
        try
        {
            using (DataTable dtInfo = daClass.return_DataTable("select distinct Ems_ContactMaster.Name,Ems_ContactMaster.Trans_Id from Set_AddressChild left join Set_AddressMaster on Set_AddressMaster.Trans_Id = Set_AddressChild.Add_Ref_Id left join Ems_ContactMaster on Ems_ContactMaster.Company_Id = Set_AddressChild.Ref_Id where Add_Type = 'Contact' and Set_AddressMaster.Trans_Id = '" + AddressId + "'"))
            {
                return dtInfo;
            }
        }
        catch
        {
            return null;
        }
    }
    public string GetContactIdByContactName(string contactName)
    {
        try
        {
            string id = "0";
            id= daClass.get_SingleValue("select top 1 trans_id from Ems_ContactMaster where Name='"+contactName+"' and isActive='true'");
            id = id == "@NOTFOUND@" ? "0" : id;
            return id;
        }
        catch
        {
            return null;
        }
    }

    public string GetContactIdByContactNameNID(string contactName, string contactId)
    {
        try
        {
            string id = "0";
            id = daClass.get_SingleValue("select top 1 trans_id from Ems_ContactMaster where Name='" + contactName + "' and trans_id ='"+contactId+"' and isActive='true'");
            id = id == "@NOTFOUND@" ? "0" : id;
            return id;
        }
        catch
        {
            return null;
        }
    }

    public string GetContactNameByContactiD(string contactId)
    {
        try
        {
            string name = "0";
            name = daClass.get_SingleValue("select top 1 Name from Ems_ContactMaster where trans_Id='" + contactId + "' and isActive='true'");
            name = name == "@NOTFOUND@" ? "" : name;
            return name;
        }
        catch
        {
            return null;
        }
    }
    public DataTable getSupplierNamePreText(string preText)
    {
        try
        {
            using (DataTable dtInfo = daClass.return_DataTable("SELECT top 10  (Ems_ContactMaster.Name + '/' + CAST(Ems_ContactMaster.Trans_Id AS nvarchar) + '/' + Ems_ContactMaster.Field1 + '/' + Ems_ContactMaster.Field2) AS Filtertext FROM Ems_ContactMaster LEFT JOIN Ems_ContactMaster e2 ON Ems_ContactMaster.Company_Id = e2.Trans_Id WHERE Ems_ContactMaster.IsActive = 'True' and (Ems_ContactMaster.Name + '/' + CAST(Ems_ContactMaster.Trans_Id AS nvarchar(50)) + '/' + Ems_ContactMaster.Field1 + '/' + Ems_ContactMaster.Field2) like '%" + preText + "%' ORDER BY Ems_ContactMaster.Name ASC"))
            {
                return dtInfo;
            }
        }
        catch
        {
            return null;
        }

    }
    public string getContactCodeFromId(string transId)
    {
        try
        {
            string code = "";
            code = daClass.get_SingleValue("select Code from Ems_ContactMaster where Trans_Id='" + transId + "'");
            code = code == "@NOTFOUND@" ? "" : code;
            return code;
        }
        catch
        {
            return null;
        }

    }
    public DataTable getContactEmailFromCustId(string custId)
    {
        try
        {
            DataTable dt = new DataTable();

            using (dt = daClass.return_DataTable("SELECT Ems_ContactMaster.Name ,Ems_ContactMaster.Trans_Id ,Ems_ContactMaster.Field1 as email,Ems_ContactMaster.Field2 as contact FROM Ems_ContactMaster WHERE Ems_ContactMaster.IsActive = 'True' AND Ems_ContactMaster.status = 'Individual' and Ems_ContactMaster.Field1<>''  AND Ems_ContactMaster.Company_Id ='"+ custId + "' ORDER BY Ems_ContactMaster.Name ASC"))
                return dt;
        }
        catch
        {
            return null;
        }

    }
}
