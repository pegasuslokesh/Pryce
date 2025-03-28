using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;

/// <summary>
/// Summary description for IT_App_Op_Permission
/// </summary>
public class IT_App_Op_Permission
{
    DataAccessClass da = null;
    public IT_App_Op_Permission(string strConString)
    {
        da = new DataAccessClass(strConString);
    }
    

    public int insertRecord(string Object_Id, string Op_Id)
    {
        
        PassDataToSql[] ParamList=new PassDataToSql[3];

         ParamList[0] = new PassDataToSql("@Object_Id",Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Op_Id", Op_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Refference_Id","0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("IT_App_Op_Permission_Insert", ParamList);
        return Convert.ToInt32(ParamList[2].ParaValue);

       
    }
    public int DeleteRecord(string Object_Id)
    {

        PassDataToSql[] ParamList = new PassDataToSql[2];

         ParamList[0] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
         ParamList[1] = new PassDataToSql("@Refference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
         da.execute_Sp("IT_App_Op_Permission_DeleteRow", ParamList);
        return Convert.ToInt32(ParamList[1].ParaValue);


    }
    public DataTable GetRecord( string Object_Id)
    {

        PassDataToSql[] ParamList = new PassDataToSql[1];

           ParamList[0] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
         return  da.Reuturn_Datatable_Search("IT_App_Op_Permission_SelectRow", ParamList);
       


    }












    
}
