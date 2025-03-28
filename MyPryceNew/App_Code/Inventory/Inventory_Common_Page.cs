using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using PegasusDataAccess;
/// <summary>
/// Summary description for Inventory_Common_Page
/// </summary>
public class Inventory_Common_Page
{
    public Inventory_Common_Page()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string[] isSerialNumberValid(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber, string strConString)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(strConString);
        SystemParameter ObjSysParam = new SystemParameter(strConString);
        string[] Result = new string[5];


        int counter = 0;

        foreach (GridViewRow gvrow in gvSerialNumber.Rows)
        {
            if (((Label)gvrow.FindControl("lblsrno")).Text.Trim() == serialNumber)
            {
                counter = 1;
                break;
            }
        }

        if (counter == 0)
        {



            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ProductId);



            try
            {
                dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (dtserial.Rows.Count > 0)
            {

                if (dtserial.Rows[0]["TransType"].ToString() == "PR")
                {
                    Result[0] = "ALREADY OUT";
                }
                else if (dtserial.Rows[0]["TransType"].ToString() == "SR")
                {
                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
                else
                {
                    DataTable dtCopy = dtserial.Copy();

                    dtserial = new DataView(dtserial, "InOut='O' and (TransType='SI' or TransType='DV')  and TransTypeId<>" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtserial.Rows.Count > 0)
                    {

                        Result[0] = "ALREADY OUT";

                    }

                    else
                    {
                        dtserial = dtCopy.Copy();
                        dtserial = new DataView(dtserial, "InOut='I'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtserial.Rows.Count > 0)
                        {

                            Result[0] = "VALID";
                            Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                            Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                            Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                            Result[4] = dtserial.Rows[0]["BatchNo"].ToString();





                        }
                    }
                }

            }
            else
            {
                Result[0] = "NOT EXISTS";
            }




        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    //this functionis created by jitendra upadhyay on 11/05/2015 for fill unit dropdown acording the product id
    public static bool FillUnitDropDown_ByProductId(DropDownList ddlUnit, string ProductId, string strConString)
    {
        DataAccessClass daclass = new DataAccessClass(strConString);
        bool Result = false;
        DataTable dtUnit = new DataTable();
        string Sql = string.Empty;
        string UnitId = "0";

        if (ProductId != "0")
        {
            //here we find out the base unit of product id if exists
            Sql = "select UnitId from Inv_ProductMaster where ProductId=" + ProductId + "";
            try
            {
                UnitId = daclass.return_DataTable(Sql).Rows[0][0].ToString();
            }
            catch
            {
            }
            //here we selct only those unit which have conversion exist with base unit

            Sql = "select Unit_Id,Unit_Name  from inv_unitmaster where company_id=" + HttpContext.Current.Session["CompId"].ToString() + " and (Unit_Id=" + UnitId + "  or Unit_Id =(select UM.Conversion_Unit from inv_unitmaster as UM where UM.Unit_Id=" + UnitId + ") or Conversion_Unit=" + UnitId + ") and IsActive='True' order by Unit_Name ";
        }
        else
        {
            //here we select all unit for those  product which is not exists in database
            Sql = "select Unit_Id,Unit_Name from Inv_UnitMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'  order by Unit_Name ";

        }
        dtUnit = daclass.return_DataTable(Sql);
        if (dtUnit != null)
        {
            ddlUnit.DataSource = dtUnit;
            ddlUnit.DataTextField = "Unit_Name";
            ddlUnit.DataValueField = "Unit_Id";
            ddlUnit.DataBind();
            Result = true;
            if (UnitId != "0")
            {
                try
                {
                    ddlUnit.SelectedValue = UnitId;
                }
                catch
                {
                }
            }
        }
        else
        {
            ddlUnit.DataSource = null;
            ddlUnit.DataBind();
        }
        return Result;
    }
}