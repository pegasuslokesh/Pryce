using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PegasusDataAccess;

/// <summary>
/// Summary description for Inventory_Common
/// </summary>
public class Inventory_Common
{

    Common cmn = null;
    DataAccessClass daClass = null;

    public Inventory_Common(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        cmn = new Common(strConString);
    }
    
    public static bool CostPermission(string strCompanyId, string strBrandId, string strLocationId, string strEmpId,string strConString,string strUserId,string strApplicationId)
    {
        Inv_UserPermission objUserPermission = new Inv_UserPermission(strConString);
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(strConString);
        Common ObjComman = new Common(strConString);
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModuleId = objObjectEntry.GetModuleObjectByApplicationId(strApplicationId);

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("24", dtModuleId);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }

        bool Result = false;


        if (strEmpId == "0")
        {
            Result = true;

        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(strUserId, strModuleId, "24",strCompanyId);

            dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=19", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAllPageCode.Rows.Count > 0)
            {

                Result = true;
            }
        }
        return Result;
    }
    public static bool CheckUserPermission(string strObjectId, string strOperationId,string strConString,string strEmpId, string strUserId,string strCompId,string strApplicationId)
    {
        Inv_UserPermission objUserPermission = new Inv_UserPermission(strConString);
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(strConString);
        Common ObjComman = new Common(strConString);
        string strModuleId = string.Empty;

        DataTable dtModuleId = objObjectEntry.GetModuleObjectByApplicationId(strApplicationId);
        

        DataTable dtModule = objObjectEntry.GetModuleIdAndName(strObjectId, dtModuleId);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();

        }

        bool Result = false;


        if (strEmpId == "0")
        {
            Result = true;

        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(strUserId, strModuleId, strObjectId,strCompId);

            dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=" + strOperationId + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAllPageCode.Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
    public DataTable GetProductDetail(string strCompanyId, string strBrandId, string strLocationId, string strProductId, string strFinanceYearId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ProductId", strProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Finance_Year_Id", strFinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_GetProductDetail_For_StockAnalysis", paramList);

        return dtInfo;
    }
    //public DataTable GetProductDetail(string strCompanyId, string strBrandId, string strLocationId, string strProductId,string strFinanceYearId)
    //{
    //    DataTable dtInfo = new DataTable();
    //    PassDataToSql[] paramList = new PassDataToSql[6];
    //    paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[2] = new PassDataToSql("@Location_ID", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[3] = new PassDataToSql("@ProductId", strProductId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[4] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[5] = new PassDataToSql("@Finance_Year_Id", strFinanceYearId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    dtInfo = daClass.Reuturn_Datatable_Search("sp_GetProductDetail_For_StockAnalysis", paramList);
    //    return dtInfo;
    //}

    public static bool GetPhyscialInventoryStatus(string strConString,string strCompId, string strBrandId, string strLocId)
    {
        bool Result = false;
        Inv_PhysicalHeader ObjPhysical = new Inv_PhysicalHeader(strConString);

        DataTable dt = ObjPhysical.GetPhysicalHeaderAllTrue(strCompId, strBrandId, strLocId);

        try
        {
            dt = new DataView(dt, "Post='False'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                Result = true;
            }
        }
        catch
        {

        }

        return Result;
    }
    public static bool CheckValidSerialForSalesinvoice(string strProductId, string StrSerial,string strConString)
    {
        bool Result = false;


        Inv_StockBatchMaster objStockbatch = new Inv_StockBatchMaster(strConString);


        DataTable dt = objStockbatch.GetStockBatchMasterAll_forcheckvalidSerial(strProductId, StrSerial);


        if (dt.Rows.Count > 0)
        {
            Result = true;
        }

        return Result;

    }
    public static bool CheckValidSerialForSalesinvoice(string StrSerial,string strConString)
    {
        bool Result = false;


        Inv_StockBatchMaster objStockbatch = new Inv_StockBatchMaster(strConString);


        DataTable dt = objStockbatch.GetStockBatchMasterAll_forcheckvalidSerial(StrSerial);


        if (dt.Rows.Count > 0)
        {
            Result = true;
        }

        return Result;

    }
    public static bool IsSalesTaxEnabled(string strConString,string strCompId,string strBrandId, string strLocId)
    {
        bool result = false;
        try
        {
            result = Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(strCompId, strBrandId, strLocId, "IsTaxSales").Rows[0]["ParameterValue"].ToString());
        }
        catch(Exception ex)
        {
            
        }
        return result;
    }

    //public static bool IsSalesTaxEnabled(string locationId,string strConString)
    //{
    //    return  Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), locationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString());
    //}
    public static bool IsSalesDiscountEnabled(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        bool result = false;
        try
        {
            result = Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(strCompId, strBrandId, strLocId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString());
        }
        catch(Exception ex)
        {

        }
        return result;
    }
    public static bool IsPurchaseDiscountEnabled(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        bool result = false;
        try
        {
            result= Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(strCompId, strBrandId, strLocId, "IsDiscount").Rows[0]["ParameterValue"].ToString());
        }
        catch(Exception ex)
        {

        }
        return result;
    }

    public static bool IsPurchaseTaxEnabled(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        bool result = false;
        try
        {
            result = Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(strCompId, strBrandId, strLocId, "IsTax").Rows[0]["ParameterValue"].ToString());
        }
        catch (Exception ex)
        {

        }
        return result;
    }


    //public static bool IsPurchaseTaxEnabled(string strConString)
    //{
    //    if (HttpContext.Current.Session["IsPurchaseTaxEnabled"] == null)
    //    {
    //        HttpContext.Current.Session["IsPurchaseTaxEnabled"] = Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "IsTax").Rows[0]["ParameterValue"].ToString());
    //    }

    //    return Convert.ToBoolean(HttpContext.Current.Session["IsPurchaseTaxEnabled"]);
    //}
    //public static bool IsPurchaseTaxEnabled(string locationId,string strConString)
    //{
    //    return Convert.ToBoolean(new Inv_ParameterMaster(strConString).GetParameterValue_By_ParameterName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), locationId, "IsTax").Rows[0]["ParameterValue"].ToString());     
    //}
    public static bool IsScanningsolutioninSales(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        bool IsAllow = false;
        Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(strConString);
        using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(strCompId, strBrandId, strLocId, "IsDeliveryScanning"))
        {
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    IsAllow = true;
                }
            }
        }
        return IsAllow;
    }
}