using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

public partial class Sales_SalesDashboard : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetLocationWiseSales(string from_date, string to_date)
    {
        List<Inv_SalesDashboard.SalesByLoc> lst = new Inv_SalesDashboard().getLocationWiseSales(from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lst);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDashboardFilterParamValue(string from_date, string to_date, string store_no, string filter_type)
    {
        Inv_Dashbord.DashboardFilter objFilter = Inv_Dashbord.getDashboardFilterParamValues((Inv_Dashbord.MyDashboardFilter)int.Parse(filter_type), from_date, to_date, store_no);
        return new JavaScriptSerializer().Serialize(objFilter);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetSalesCategoryChartData(string from_date, string to_date, string store_no, string filter_type)
    {
        List<Inv_SalesDashboard.SalesByCategory> lstObj = new Inv_SalesDashboard().getCategoryWiseSales(store_no, from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByCategory aa in lstObj select (object)aa.cat_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByCategory aa in lstObj select (object)aa.total_sale).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByCategory aa in lstObj select (object)aa.total_cost).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByCategory aa in lstObj select (object)aa.cat_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductSummaryForCategory(string store_no, string from_date, string to_date, string category_id)
    {
        List<Inv_SalesDashboard.ProductSalesSummary> lstObj = new Inv_SalesDashboard().getProductWiseSales(store_no, from_date, to_date, category_id, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lstObj);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetSalesPersonChartData(string store_no, string from_date, string to_date)
    {
        List<Inv_SalesDashboard.SalesBySalesPerson> lstObj = new Inv_SalesDashboard().getSalesPersonWiseSales(store_no, from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.sales_person_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.sales_quota).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.current_forecast_amt).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.current_order_amt).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.current_invoice_amt).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesBySalesPerson aa in lstObj select (object)aa.sman_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetBrandSalesChartData(string store_no, string from_date, string to_date)
    {
        List<Inv_SalesDashboard.SalesByBrand> lstObj = new Inv_SalesDashboard().getBrandWiseSales(store_no, from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByBrand aa in lstObj select (object)aa.brand_name).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByBrand aa in lstObj select (object)Double.Parse(aa.total_sale)).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByBrand aa in lstObj select (object)aa.brand_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductSummaryForBrand(string store_no, string from_date, string to_date, string brand_id)
    {
        List<Inv_SalesDashboard.ProductSalesSummary> lstObj = new Inv_SalesDashboard().GetProductSummaryForBrand(store_no, from_date, to_date, brand_id, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lstObj);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetPayMethodChartData(string store_no, string from_date, string to_date)
    {
        List<Inv_SalesDashboard.SalesByPayMethod> lstObj = new Inv_SalesDashboard().getPayMethodWiseSales(store_no, from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        try
        {
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByPayMethod aa in lstObj select (object)aa.payment_method).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByPayMethod aa in lstObj select (object)aa.amount).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from Inv_SalesDashboard.SalesByPayMethod aa in lstObj select (object)aa.payment_id).ToList();
            lstData.Add(x);
            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetSinvHeaderByPayMethod(string store_no, string from_date, string to_date, string payment_id)
    {
        List<Inv_SalesDashboard.SalesInvHeader> lstObj = new Inv_SalesDashboard().getPayMethodWiseSinvHeader(store_no, from_date, to_date, payment_id, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
        return new JavaScriptSerializer().Serialize(lstObj);
    }
    
}