using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Dashboard_Ecommerce : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {        
        }        
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetECommerceOrder(string from_date, string to_date)
    {
        List<ECommerceDashboard> lstObj = null;
        try
        {
            lstObj = Dashboard_Ecommerce.GetECommerceOrderData( from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.TotalOrder).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);

            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetECommerceReturn(string from_date, string to_date)
    {
        List<ECommerceDashboard> lstObj = null;
        try
        {
            lstObj = Dashboard_Ecommerce.GetECommerceReturnData(from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.TotalOrder).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);

            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetECommerceOrderReturnProduct(string from_date, string to_date)
    {
        List<ECommerceDashboard> lstObj = null;
        try
        {
            lstObj = Dashboard_Ecommerce.GetECommerceOrdeReturnrProduct(from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.Qty).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.EProductName).ToList();
            lstData.Add(x);

            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }
    public static ICollection<ECommerceDashboard> GetECommerceOrdeReturnrProduct(string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        List<ECommerceDashboard> lst = new List<ECommerceDashboard>() { };
        using (SqlConnection con = new SqlConnection(strConString))
        {

            try
            {

                if (from_date.Length == 0)
                {
                    from_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }

                if (to_date.Length == 0)
                {
                    to_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }
                //string strSQL = "Select Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name , Count(distinct(alternate_scan)) as 'Total Order'  From  tns_merchant_product_serial inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id     inner join Inv_ProductMaster on Inv_ProductMaster.ProductCode = tns_merchant_product_serial.product_code   Where  Cast(t_datetime as Date) >='" + from_date +"' and Cast(t_datetime as Date) <='"+ to_date + "' and frmType ='SI'  group by Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name   order by  Sys_MerchantMaster.Merchant_Name";
                string strSQL = "Select  Sys_MerchantMaster.Merchant_Name, Inv_ProductMaster.EProductName , SUM(Qty) as Qty  From  tns_merchant_product_serial inner join Inv_ProductMaster  on  Inv_ProductMaster.ProductCode = tns_merchant_product_serial.product_code  inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id       Where   Cast(t_datetime as Date) >='" + from_date + "' and Cast(t_datetime as Date) <='" + to_date + "'  and frmType ='SR'  Group by EProductName,Sys_MerchantMaster.Merchant_Name order by  Sys_MerchantMaster.Merchant_Name, EProductName ";
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(strSQL, con);
                adp.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ECommerceDashboard ed = new ECommerceDashboard();
                    ed.EProductName = dt.Rows[i][1].ToString();
                    ed.MerchantName = dt.Rows[i][0].ToString();
                    ed.Qty = Convert.ToInt16(dt.Rows[i][2].ToString());
                    lst.Add(ed);
                }
                dt.Dispose();


            }
            catch (Exception ex)
            {

            }

        }
        return lst;
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetECommerceOrderProduct(string from_date, string to_date)
    {
        List<ECommerceDashboard> lstObj = null;
        try
        {
            lstObj = Dashboard_Ecommerce.GetECommerceOrderProduct(from_date, to_date, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["lang"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["RoleId"].ToString(), HttpContext.Current.Session["UserId"].ToString()).ToList();
            List<object> lstData = new List<object>();
            List<object> x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.MerchantName).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.Qty).ToList();
            lstData.Add(x);
            x = new List<object>();
            x = (from ECommerceDashboard aa in lstObj select (object)aa.EProductName).ToList();
            lstData.Add(x);

            return new JavaScriptSerializer().Serialize(lstData);
        }
        catch (Exception ex)
        {
            return new JavaScriptSerializer().Serialize(lstObj);
        }
    }
    public static ICollection<ECommerceDashboard> GetECommerceOrderProduct(string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        List<ECommerceDashboard> lst = new List<ECommerceDashboard>() { };
        using (SqlConnection con = new SqlConnection(strConString))
        {

            try
            {

                if (from_date.Length == 0)
                {
                    from_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }

                if (to_date.Length == 0)
                {
                    to_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }
                //string strSQL = "Select Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name , Count(distinct(alternate_scan)) as 'Total Order'  From  tns_merchant_product_serial inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id     inner join Inv_ProductMaster on Inv_ProductMaster.ProductCode = tns_merchant_product_serial.product_code   Where  Cast(t_datetime as Date) >='" + from_date +"' and Cast(t_datetime as Date) <='"+ to_date + "' and frmType ='SI'  group by Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name   order by  Sys_MerchantMaster.Merchant_Name";
                string strSQL = "Select  Sys_MerchantMaster.Merchant_Name, Inv_ProductMaster.EProductName , SUM(Qty) as Qty  From  tns_merchant_product_serial inner join Inv_ProductMaster  on  Inv_ProductMaster.ProductCode = tns_merchant_product_serial.product_code  inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id       Where   Cast(t_datetime as Date) >='" + from_date + "' and Cast(t_datetime as Date) <='" + to_date + "'  and frmType ='SI'  Group by EProductName,Sys_MerchantMaster.Merchant_Name order by  Sys_MerchantMaster.Merchant_Name, EProductName ";
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(strSQL, con);
                adp.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ECommerceDashboard ed = new ECommerceDashboard();
                    ed.EProductName = dt.Rows[i][1].ToString();
                    ed.MerchantName = dt.Rows[i][0].ToString();
                    ed.Qty = Convert.ToInt16(dt.Rows[i][2].ToString());
                    lst.Add(ed);
                }
                dt.Dispose();


            }
            catch (Exception ex)
            {

            }

        }
        return lst;
    }
    public static  ICollection<ECommerceDashboard> GetECommerceOrderData( string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        List<ECommerceDashboard> lst = new List<ECommerceDashboard>() { };
        using (SqlConnection con = new SqlConnection(strConString))
        {
           
            try
            {

                if(from_date.Length == 0)
                {
                    from_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }

                if (to_date.Length == 0)
                {
                    to_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }
                //string strSQL = "Select Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name , Count(distinct(alternate_scan)) as 'Total Order'  From  tns_merchant_product_serial inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id     inner join Inv_ProductMaster on Inv_ProductMaster.ProductCode = tns_merchant_product_serial.product_code   Where  Cast(t_datetime as Date) >='" + from_date +"' and Cast(t_datetime as Date) <='"+ to_date + "' and frmType ='SI'  group by Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name   order by  Sys_MerchantMaster.Merchant_Name";
                string strSQL = "Select trans_Id,Merchant_Name , Count(distinct(orderid)) as 'Total Order' From   (Select   Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name  , case  when tns_merchant_product_serial.order_id = ''  then alternate_scan else tns_merchant_product_serial.order_id  end orderid From  tns_merchant_product_serial inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id     Where  Cast(t_datetime as Date) >='" + from_date + "' and Cast(t_datetime as Date) <='" + to_date + "'  and frmType ='SI'  ) tbl group by trans_Id,Merchant_Name   order by  Merchant_Name";
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(strSQL, con);
                adp.Fill(dt);
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    ECommerceDashboard ed = new ECommerceDashboard();
                    ed.MerchantId = dt.Rows[i][2].ToString();
                    ed.MerchantName = dt.Rows[i][1].ToString();
                    ed.TotalOrder = Convert.ToInt16(dt.Rows[i][2].ToString());
                    lst.Add(ed);
                }
                dt.Dispose();


            }
            catch (Exception ex)
            {
                
            }
            
        }
        return lst;
    }

    public static ICollection<ECommerceDashboard> GetECommerceReturnData(string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        List<ECommerceDashboard> lst = new List<ECommerceDashboard>() { };
        using (SqlConnection con = new SqlConnection(strConString))
        {

            try
            {

                if (from_date.Length == 0)
                {
                    from_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }

                if (to_date.Length == 0)
                {
                    to_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                }
                string strSQL = "Select trans_Id,Merchant_Name , Count(distinct(orderid)) as 'Total Order' From   (Select   Sys_MerchantMaster.trans_Id,Sys_MerchantMaster.Merchant_Name  , case  when tns_merchant_product_serial.order_id = ''  then alternate_scan else tns_merchant_product_serial.order_id  end orderid From  tns_merchant_product_serial inner join Sys_MerchantMaster on  Sys_MerchantMaster.Trans_Id = tns_merchant_product_serial.merchant_id       Where  Cast(t_datetime as Date) >='" + from_date + "' and Cast(t_datetime as Date) <='" + to_date + "'  and frmType ='SR'  ) tbl group by trans_Id,Merchant_Name   order by  Merchant_Name";
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(strSQL, con);
                adp.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ECommerceDashboard ed = new ECommerceDashboard();
                    ed.MerchantId = dt.Rows[i][2].ToString();
                    ed.MerchantName = dt.Rows[i][1].ToString();
                    ed.TotalOrder = Convert.ToInt16(dt.Rows[i][2].ToString());
                    lst.Add(ed);
                }
                dt.Dispose();


            }
            catch (Exception ex)
            {

            }

        }
        return lst;
    }


}
public class ECommerceDashboard
{
    public string MerchantName { get; set; }
    public int TotalOrder { get; set; }
    public string MerchantId { get; set; }
    public int Qty { get; set; }
    public string EProductName { get; set; }
}