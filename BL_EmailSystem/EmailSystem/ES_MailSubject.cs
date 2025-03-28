using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using PegasusDataAccess;
using System.Data;

/// <summary>
/// Summary description for ES_MailSubject
/// </summary>
public class ES_MailSubject
{
    DataAccessClass Daclass = null;
    EmployeeMaster objEmployee = null;
    //private string strEmailconn = ConfigurationManager.ConnectionStrings["Pega_Email_System"].ConnectionString;
    //private string strEmailconn = HttpContext.Current.Session["DBConnection_ES"].ToString();
    public ES_MailSubject(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        Daclass = new DataAccessClass(strConString);
        objEmployee = new EmployeeMaster(strConString);
    }

    public int InsertEmailSubject(string strCompany_Id, string Module_Id, string Object_Id, string Prefix, string Suffix, string ContactId, string EmpId, string strDocNo, string Year, string Month, string Day, string IsUse, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[25];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Prefix", Prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Suffix", Suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Contact_Id", ContactId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Day", Day, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsUse", IsUse, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[11] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[12] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[17] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@DocNo", strDocNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);


        Daclass.execute_Sp("sp_ES_MailSubject_Insert", paramList);
        return Convert.ToInt32(paramList[11].ParaValue);
    }
    public int UpdateEmailSubject(string strCompany_Id, string Trans_Id, string Module_Id, string Object_Id, string Prefix, string Suffix, string ContactId, string EmpId, string strDocNo, string Year, string Month, string Day, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[23];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Prefix", Prefix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Suffix", Suffix, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Contact_Id", ContactId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Day", Day, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
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

        paramList[21] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@DocNo", strDocNo, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);


        Daclass.execute_Sp("sp_ES_MailSubject_Update", paramList);
        return Convert.ToInt32(paramList[10].ParaValue);
    }
    public int DeleteEmailSubject(string strCompany_Id, string strTrans_Id, string Module_Id, string strObject_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strTrans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Module_Id", Module_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Object_Id", strObject_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        Daclass.execute_Sp("sp_ES_MailSubject_Delete", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }
    public DataTable GetEmailSubjectAll(string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_ES_MailSubject_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetEmailSubjectAll(string strCompanyId, string ModuleId, string ObjectId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", ObjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", ModuleId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_ES_MailSubject_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetEmailSubjectAll(string ModuleId, string ObjectId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", ObjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", ModuleId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = Daclass.Reuturn_Datatable_Search("sp_ES_MailSubject_SelectRow", paramList);
        return dtInfo;
    }
    public string GetEmailSubjectAll(string CompanyId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string ContactCode, string DocNo,string strLoginEmpId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();
        if (IsUseCompIdInWhere)
        {
            dt = GetEmailSubjectAll(CompanyId, ModuleId, ObjectId);

        }
        else
        {
            dt = GetEmailSubjectAll(ModuleId, ObjectId);

        }
        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString() + "-";

            }
            if (Convert.ToBoolean(dr["Contact_Id"].ToString()))
            {
                if (ContactCode.ToString() != "")
                {
                    StrDocument += ContactCode.ToString() + "-";
                }


            }
            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                try
                {
                    StrDocument += objEmployee.GetEmployeeMasterById(CompanyId, strLoginEmpId).Rows[0]["Emp_Code"].ToString() + "";
                }
                catch
                {

                }

            }

            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += DateTime.Now.Year.ToString() + "-";
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += DateTime.Now.Month.ToString() + "-";
            }

            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += DateTime.Now.Day.ToString() + "-";
            }
            if (Convert.ToBoolean(dr["DocNO"].ToString()))
            {
                StrDocument += DocNo.ToString() + "";
            }

            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }





        }
        return StrDocument;


    }


    public static string GetMailSubject(string RefType, string RefNo, string strContactId, string strModuleId, string strObjectId,string strConString,string strLoginEmpId)
    {
        ES_MailSubject objEmailSubject = new ES_MailSubject(strConString);
        Ems_ContactMaster objContact = new Ems_ContactMaster(strConString);

        string StringSubject = string.Empty;
        string strContactCode = string.Empty;


        if (strContactId != "")
        {
            try
            {
                strContactCode = objContact.GetContactTrueById(strContactId).Rows[0]["Code"].ToString();
            }
            catch
            {
            }

        }

        StringSubject = objEmailSubject.GetEmailSubjectAll("", false, strModuleId, strObjectId, strContactCode, RefNo, strLoginEmpId);


        if (StringSubject == "")
        {

            StringSubject = RefType + " - " + RefNo;

            try
            {

                StringSubject = StringSubject.Replace("/", "").Trim();
                StringSubject = StringSubject.Replace("<", "").Trim();
                StringSubject = StringSubject.Replace(">", "").Trim();
                StringSubject = StringSubject.Replace(":", "").Trim();
                StringSubject = StringSubject.Replace("*", "").Trim();
                StringSubject = StringSubject.Replace("?", "").Trim();
                StringSubject = StringSubject.Replace("|", "").Trim();
                StringSubject = StringSubject.Replace(",", "").Trim();
                StringSubject = StringSubject.Replace("&", "").Trim();


            }
            catch
            {

            }
        }

        return StringSubject;
    }
}
