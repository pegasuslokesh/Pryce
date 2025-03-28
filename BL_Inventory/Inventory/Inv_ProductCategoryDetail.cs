using System;
using System.Data;
using System.Configuration;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Inv_ProductCategoryDetail
/// </summary>
public class Inv_ProductCategoryDetail
{
    DataAccessClass DaClass = null;
    public Inv_ProductCategoryDetail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        DaClass = new DataAccessClass(strConString);
    }
    public int ProductCategoryDetail_Insert(string Company_Id, string Brand_Id, string Ref_Id, string Parent_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] ParamList = new PassDataToSql[17];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Parent_Id", Parent_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[4] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[5] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[6] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[7] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[8] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[9] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        ParamList[10] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[11] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        ParamList[12] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[13] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[14] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ParamList[15] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        ParamList[16] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_Product_CategoryDetail_Insert", ParamList,ref trns);
        return Convert.ToInt32(ParamList[16].ParaValue);

    }
    public int ProductCategoryDetail_Delete(string Company_Id, string Brand_Id, string Ref_Id,ref SqlTransaction trns)
    {
        PassDataToSql[] ParamList = new PassDataToSql[4];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_Product_CategoryDetail_Delete", ParamList,ref trns);
        return Convert.ToInt32(ParamList[3].ParaValue);

    }
    public int ProductCategoryDetail_Delete(string Company_Id, string Brand_Id, string Ref_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[4];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_Product_CategoryDetail_Delete", ParamList);
        return Convert.ToInt32(ParamList[3].ParaValue);

    }
    public int ProductCategoryDetail_DeleteByParentId(string Company_Id, string Brand_Id, string Parent_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[4];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Parent_Id", Parent_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DaClass.execute_Sp("Inv_Product_CategoryDetail_DeleteByParentId", ParamList);
        return Convert.ToInt32(ParamList[3].ParaValue);

    }
    public DataTable GetProductCategoryDetail(string Company_Id, string Brand_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        ParamList[4] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_Product_CategoryDetail_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetProductCategoryDetail(string Company_Id, string Brand_Id, string Trans_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        ParamList[4] = new PassDataToSql("@Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_Product_Category_SelectRow", ParamList);
        return dt;

    }
    public DataTable GetProductCategoryDetailByRefId(string Company_Id, string Brand_Id, string Ref_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        ParamList[4] = new PassDataToSql("@Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_Product_CategoryDetail_SelectRow", ParamList);
        return dt;

    }

    public DataTable GetProductCategoryDetailByParentId(string Company_Id, string Parent_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Parent_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        ParamList[4] = new PassDataToSql("@Optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_Product_CategoryDetail_SelectRow", ParamList);
        return dt;

    }
    public DataTable ProductCategoryDetail_Update(string Company_Id,string Brand_Id,string Ref_Id ,string Parent_Id)
    {
        PassDataToSql[] ParamList = new PassDataToSql[5];
        ParamList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        ParamList[3] = new PassDataToSql("@Parent_Id", Parent_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        ParamList[4] = new PassDataToSql("@ReferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        DataTable dt = DaClass.Reuturn_Datatable_Search("Inv_Product_CategoryDetail_Update", ParamList);
        return dt;

    }
    ///Inv_Product_CategoryDetail_Update


}
