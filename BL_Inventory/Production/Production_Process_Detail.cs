using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Production_Process_Detail
/// </summary>
public class Production_Process_Detail
{
    DataAccessClass daClass = null;
    public Production_Process_Detail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }

    
    public int InsertRecord(string str_Ref_Job_No, string str_ProductId, string str_UnitId, string str_UnitPrice, string str_Request_Qty, string str_Remain_Qty,string str_Production_Qty,string str_Line_Total,string strExtra_Qty, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Ref_Job_No", str_Ref_Job_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ProductId", str_ProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@UnitId", str_UnitId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@UnitPrice", str_UnitPrice, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Request_Qty", str_Request_Qty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Remain_Qty", str_Remain_Qty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Production_Qty", str_Production_Qty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Line_Total", str_Line_Total, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Extra_Qty", strExtra_Qty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ReferanceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Inv_Production_Process_Detail_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[9].ParaValue);
    }

    public void DeleteRecord_By_RefJobNo(string Ref_Job_No, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Ref_Job_No", Ref_Job_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferanceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Inv_Production_Process_Detail_DeleteRow", paramList, ref trns);
    }

    public DataTable GetRecord_By_RefJobNo(string Ref_Job_No)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Ref_Job_No", Ref_Job_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_Production_Process_Detail_SelectRow", paramList);

        return dtInfo;
    }
}