using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Arc_Directory_Privileges
/// </summary>
public class Arc_Directory_Privileges
{
    DataAccessClass da = null;

    public Arc_Directory_Privileges(string strConString)
    {
        da = new DataAccessClass(strConString);
    }

    public int InsertRecord(string strDirectoryId,string strEmployeeId)
    {
        PassDataToSql[] param = new PassDataToSql[3];
        param[0] = new PassDataToSql("@Directory_Id", strDirectoryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("Sp_Arc_Directory_Privileges_Insert", param);
        return Convert.ToInt32(param[2].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }


    public int InsertRecord(string strDirectoryId, string strEmployeeId,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[3];
        param[0] = new PassDataToSql("@Directory_Id", strDirectoryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("Sp_Arc_Directory_Privileges_Insert", param,ref trns);
        return Convert.ToInt32(param[2].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }



    public int DeleteRecord(string strDirectoryId)
    {
        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Directory_Id", strDirectoryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("Sp_Arc_Directory_Privileges_DeleteRow", param);
        return Convert.ToInt32(param[1].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }



    public DataTable getRecordbyLoginEmployeeId(string strEmpId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramlist = new PassDataToSql[3];

        paramlist[0] = new PassDataToSql("@Directory_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramlist[1] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramlist[2] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dt = da.Reuturn_Datatable_Search("Sp_Arc_Directory_Privileges_SelectRow", paramlist);
        return dt;

    }


    public DataTable getRecordbyDirectoryId(string strDirectoryId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramlist = new PassDataToSql[3];

        paramlist[0] = new PassDataToSql("@Directory_Id", strDirectoryId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramlist[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramlist[2] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dt = da.Reuturn_Datatable_Search("Sp_Arc_Directory_Privileges_SelectRow", paramlist);
        return dt;

    }




    public Arc_Directory_Privileges()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}