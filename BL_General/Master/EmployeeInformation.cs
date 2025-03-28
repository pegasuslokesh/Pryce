using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;

/// <summary>
/// Summary description for EmployeeInformation
/// </summary>
public class EmployeeInformation
{

    DataAccessClass daClass = null;
    public EmployeeInformation(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }

    public int UpdateAccessControlInfo(string enrollno, string EmpCardNo, string pwd, string priv, string fingertemp, string FaceData, string FaceIndex, string FaceLength, string idwFingerIndex, string iflag, string sEnabled, string Temp6, string Temp7,string strTimeZoneId)
    {

        int retval = 0;
        if (FaceLength == "")
        {
            FaceLength = "0";
        }
        if (idwFingerIndex == "")
        {
            idwFingerIndex = "0";
        }
        if (iflag == "")
        {
            iflag = "0";
        }



        PassDataToSql[] paramList = new PassDataToSql[17];

        paramList[0] = new PassDataToSql("@Emp_Id", enrollno, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@CardNo", EmpCardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Template1", priv, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Template2", pwd, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Template3", fingertemp, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //face data
        paramList[5] = new PassDataToSql("@Template4", FaceData, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        //face index
        paramList[6] = new PassDataToSql("@Template5", FaceIndex, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Template6", Temp6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Template7", Temp7, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Template8", FaceLength, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Template9", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Template10", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[13] = new PassDataToSql("@idwFingerIndex", idwFingerIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[14] = new PassDataToSql("@iflag", iflag, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@sEnabled", sEnabled, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("Set_EmployeeInformation_Update", paramList);

        retval = Convert.ToInt32(paramList[12].ParaValue);


        return retval;
    }



    public int UpdateAccessControlFingerInfo(string enrollno, string EmpCardNo, string pwd, string priv, string fingertemp, string FaceData, string FaceIndex, string FaceLength, string idwFingerIndex, string iflag, string sEnabled, string Temp6, string Temp7,string strTimeZoneId)
    {

        int retval = 0;
        if (FaceLength == "")
        {
            FaceLength = "0";
        }
        if (idwFingerIndex == "")
        {
            idwFingerIndex = "0";
        }
        if (iflag == "")
        {
            iflag = "0";
        }



        PassDataToSql[] paramList = new PassDataToSql[17];

        paramList[0] = new PassDataToSql("@Emp_Id", enrollno, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@CardNo", EmpCardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Template1", priv, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Template2", pwd, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Template3", fingertemp, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //face data
        paramList[5] = new PassDataToSql("@Template4", FaceData, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        //face index
        paramList[6] = new PassDataToSql("@Template5", FaceIndex, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Template6", Temp6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Template7", Temp7, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Template8", FaceLength, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Template9", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Template10", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[13] = new PassDataToSql("@idwFingerIndex", idwFingerIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[14] = new PassDataToSql("@iflag", iflag, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@sEnabled", sEnabled, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("Set_EmployeeInformation_UpdateFingerData", paramList);

        retval = Convert.ToInt32(paramList[12].ParaValue);


        return retval;
    }



    public int UpdateAccessControlFaceInfo(string enrollno, string EmpCardNo, string pwd, string priv, string fingertemp, string FaceData, string FaceIndex, string FaceLength, string idwFingerIndex, string iflag, string sEnabled, string Temp6, string Temp7,string strTimeZoneId)
    {

        int retval = 0;
        if (FaceLength == "")
        {
            FaceLength = "0";
        }
        if (idwFingerIndex == "")
        {
            idwFingerIndex = "0";
        }
        if (iflag == "")
        {
            iflag = "0";
        }



        PassDataToSql[] paramList = new PassDataToSql[17];

        paramList[0] = new PassDataToSql("@Emp_Id", enrollno, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@CardNo", EmpCardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Template1", priv, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Template2", pwd, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Template3", fingertemp, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //face data
        paramList[5] = new PassDataToSql("@Template4", FaceData, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        //face index
        paramList[6] = new PassDataToSql("@Template5", FaceIndex, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Template6", Temp6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Template7", Temp7, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Template8", FaceLength, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Template9", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Template10", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[13] = new PassDataToSql("@idwFingerIndex", idwFingerIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[14] = new PassDataToSql("@iflag", iflag, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@sEnabled", sEnabled, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("Set_EmployeeInformation_UpdateFaceData", paramList);

        retval = Convert.ToInt32(paramList[12].ParaValue);


        return retval;
    }

    public int InsertEmployeeInformation(string strEmpId, string strEmpCardNo, string strTemplate1, string strTemplate2, string strTemplate3, string strTemplate4, string strTemplate5, string strTemplate6, string strTemplate7, string strTemplate8, string strTemplate9, string strTemplate10, string strStatus, string stridwFingerIndex, string striflag, string strsEnabled)
    {
        int retval = 0;
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@CardNo", strEmpCardNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Template1", strTemplate1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Template2", strTemplate2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Template3", strTemplate3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Template4", strTemplate4, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Template5", strTemplate5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Template6", strTemplate6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Template7", strTemplate7, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Template8", strTemplate8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Template9", strTemplate9, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Template10", strTemplate10, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@idwFingerIndex", stridwFingerIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@iflag", striflag, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@sEnabled", strsEnabled, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_EmployeeInformation_Insert", paramList);
        retval = Convert.ToInt32(paramList[16].ParaValue);
        return retval;
    }

    public int DeleteEmployeeInformationByEmpId(string EmpId)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Set_EmployeeInformation_Delete", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }
    public DataTable GetEmployeeAccessInfoByEmpId(string empid)
    {
        DataTable dt = new DataTable();
        //string str = string.Empty;
        // str = "";
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dt = daClass.Reuturn_Datatable_Search("sp_Set_EmployeeInformation_SelectRow", paramList);
        return dt;
    }
    public DataTable GetEmployeeAccessInfo()
    {
        DataTable dt = new DataTable();
        //string str = string.Empty;
        // str = "";
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dt = daClass.Reuturn_Datatable_Search("sp_Set_EmployeeInformation_SelectRow", paramList);
        return dt;
    }
}
