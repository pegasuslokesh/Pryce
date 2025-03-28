using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

public partial class Inventory_Inv_Dashboard : System.Web.UI.Page
{
    Inv_Dashbord objInvDashboard = new Inv_Dashbord();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetInventoryCategoryChartData(string store_no, string from_date, string to_date)
    {
        List<Inv_Dashbord.StockByCategory> lstObj = null;
        try
        {
            lstObj = new Inv_Dashbord().getCategoryWisesStock(store_no, from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_Dashbord.StockByCategory aa in lstObj select (object)aa.cat_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.StockByCategory aa in lstObj select (object)aa.stock_qty).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.StockByCategory aa in lstObj select (object)aa.cat_id).ToList();
            lstData.Add(x);

            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductStockByCategory(string store_no, string from_date, string to_date, string category_id)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Inv_Dashbord.ProductStockView> lstObj = new Inv_Dashbord().getProductWiseStockByCategory(store_no, from_date, to_date, category_id, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return serializer.Serialize(lstObj);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetLocationWiseStock(string from_date, string to_date)
    {
        List<Inv_Dashbord.StockByLocation> lst = new Inv_Dashbord().getStockByLocation(from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lst);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetFastMovingItemChartData(string store_no, string from_date, string to_date, int noOfTopItems = 10)
    {
        List<Inv_Dashbord.FastMovingItemView> lstObj = new Inv_Dashbord().getFastMovingItemByQty(store_no, from_date, to_date, HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), noOfTopItems).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_Dashbord.FastMovingItemView aa in lstObj select (object)aa.item_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.FastMovingItemView aa in lstObj select (object)aa.qty).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.FastMovingItemView aa in lstObj select (object)aa.item_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDashboardFilterParamValue(string from_date, string to_date, string store_no, string filter_type)
    {
        Inv_Dashbord.DashboardFilter objFilter = Inv_Dashbord.getDashboardFilterParamValues((Inv_Dashbord.MyDashboardFilter)int.Parse(filter_type), from_date, to_date, store_no);
        return new JavaScriptSerializer().Serialize(objFilter);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetInventoryBrandChartData(string store_no, string from_date, string to_date)
    {
        List<Inv_Dashbord.StockByBrand> lstObj = new Inv_Dashbord().getBrandWisesStock(store_no, from_date, to_date, HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_Dashbord.StockByBrand aa in lstObj select (object)aa.brand_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.StockByBrand aa in lstObj select (object)aa.stock_qty).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_Dashbord.StockByBrand aa in lstObj select (object)aa.brand_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductStockByBrand(string store_no, string from_date, string to_date, string brand_id)
    {
        List<Inv_Dashbord.ProductStockView> lstObj = new List<Inv_Dashbord.ProductStockView> { };
        try
        {
            lstObj = new Inv_Dashbord().getProductWiseStockByBrand(store_no, from_date, to_date, brand_id, HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            return new JavaScriptSerializer().Serialize(lstObj);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductSalesDetailByProductId(string store_no, string from_date, string to_date, string product_id)
    {
        List<Inv_Dashbord.ProductSalesDetailView> lstObj = new Inv_Dashbord().getProductSalesDetailByProductId(store_no, from_date, to_date, product_id, HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lstObj);
    }

}