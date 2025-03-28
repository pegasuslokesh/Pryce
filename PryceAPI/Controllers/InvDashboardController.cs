using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PegasusDataAccess;
using System.Data.SqlClient;

namespace PryceAPI.Controllers
{
    public class InvDashboardController : ApiController
    {
        public ICollection<Inv_Dashbord.StockByCategory> getCategoryWisesStock(string store_no, string from_date, string to_date, string strCmpId, string strLang, string strConString)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.StockByCategory> lst = new List<Inv_Dashbord.StockByCategory>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " inv_stockDetail.company_id='" + strCmpId + "'";
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        where_clause = where_clause + " and inv_stockDetail.location_id='" + store_no + "'";
                    }

                    sql = "select Inv_Product_CategoryMaster.Category_Id,Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Name_L,sum(Inv_StockDetail.Quantity) as stock_qty ,sum(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join inv_productmaster on inv_productmaster.ProductId=Inv_StockDetail.ProductId left join Inv_Product_Category on Inv_Product_Category.ProductId=Inv_ProductMaster.ProductId inner join Inv_Product_CategoryMaster on Inv_Product_Category.CategoryId=Inv_Product_CategoryMaster.Category_Id where " + where_clause + " group by Inv_Product_CategoryMaster.Category_Id,Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Name_L";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.StockByCategory cls = new Inv_Dashbord.StockByCategory();
                            if (strLang == "2")
                            {
                                cls.cat_name = OraReader["Category_Name_L"].ToString();
                            }
                            else
                            {
                                cls.cat_name = OraReader["Category_Name"].ToString();
                            }
                            cls.stock_qty = double.Parse(OraReader["stock_qty"].ToString()).ToString("0.00");
                            cls.stock_value = double.Parse(OraReader["stock_value"].ToString()).ToString("0.000");
                            cls.cat_id = OraReader["Category_Id"].ToString();
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch (Exception ex)
                {
                    return lst;
                }
            }
        }

        public ICollection<Inv_Dashbord.ProductStockView> getProductWiseStockByCategory(string location_id, string from_date, string to_date, string category_id, string strCmpId, string strLang, string strConString)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.ProductStockView> lst = new List<Inv_Dashbord.ProductStockView>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " inv_stockdetail.company_id='" + strCmpId + "' and Inv_Product_Category.CategoryId=@category_id and inv_stockdetail.quantity>0 ";
                    if (!string.IsNullOrEmpty(location_id) && location_id != "0")
                    {
                        where_clause = where_clause + " and inv_stockdetail.location_id=@location_id";
                    }
                    //sql = "select inv_itemmaster.ItemID,inv_itemmaster.ItemName,inv_itemmaster.Item_Name_L,Sys_UnitSetup.UnitName, sum(inv_stockmaster.StockQty) as stock_qty ,sum(inv_stockmaster.StockQty *inv_stockmaster.AvgCost) as stock_value from inv_stockmaster inner join inv_itemmaster on inv_itemmaster.ItemId=inv_stockmaster.itemId inner join Sys_UnitSetup on Sys_UnitSetup.unitId=inv_itemmaster.UnitID where " + where_clause + " group by inv_itemmaster.ItemID,inv_itemmaster.ItemName,inv_itemmaster.Item_Name_L,Sys_UnitSetup.UnitName";
                    sql = "select inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L, Inv_StockDetail.Quantity as stock_qty ,(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join inv_productmaster on inv_productmaster.ProductId=Inv_StockDetail.ProductId left join Inv_Product_Category on Inv_Product_Category.ProductId=Inv_ProductMaster.ProductId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId where " + where_clause;
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@category_id", int.Parse(category_id));
                    if (!string.IsNullOrEmpty(location_id) && location_id != "0")
                    {
                        cmd.Parameters.AddWithValue("@location_id", int.Parse(location_id));
                    }
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.ProductStockView cls = new Inv_Dashbord.ProductStockView();
                            if (strLang == "2")
                            {
                                cls.product_name = OraReader["LProductName"].ToString();
                                cls.unit_name = OraReader["Unit_Name_L"].ToString();
                            }
                            else
                            {
                                cls.product_name = OraReader["EProductName"].ToString();
                                cls.unit_name = OraReader["Unit_Name"].ToString();
                            }
                            cls.stock_qty = double.Parse(OraReader["stock_qty"].ToString()).ToString("0.00");
                            cls.stock_value = double.Parse(OraReader["stock_value"].ToString()).ToString("0.000");

                            cls.product_id = OraReader["ProductId"].ToString();

                            if (double.Parse(cls.stock_value) != 0)
                            {
                                cls.cost_price = Math.Abs(double.Parse(cls.stock_value) / double.Parse(cls.stock_qty)).ToString("0.000");
                            }
                            else
                            {
                                cls.cost_price = "0.000";
                            }

                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch
                {
                    return lst;
                }
            }
        }
        public ICollection<Inv_Dashbord.StockByLocation> getStockByLocation(string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.StockByLocation> lst = new List<Inv_Dashbord.StockByLocation>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " inv_stockDetail.company_id='" + strCmpId + "'";
                    if (strEmpId != "0")
                    {
                        string LocIds = new Common(con.ConnectionString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        where_clause += " and inv_stockDetail.location_id in (" + LocIds.Substring(0, LocIds.Length - 1) + ")";
                    }
                    sql = "select Set_LocationMaster.Location_Id,Set_LocationMaster.Location_Name,Set_LocationMaster.Location_Name_L,sum(Inv_StockDetail.Quantity) as stock_qty ,sum(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join Set_LocationMaster on Set_LocationMaster.location_id=Inv_StockDetail.location_id where " + where_clause + " group by Set_LocationMaster.Location_Id,Set_LocationMaster.Location_Name,Set_LocationMaster.Location_Name_L";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.StockByLocation cls = new Inv_Dashbord.StockByLocation();
                            if (strLang == "2")
                            {
                                cls.loc_name = OraReader["Location_Name_L"].ToString();
                            }
                            else
                            {
                                cls.loc_name = OraReader["Location_Name"].ToString();
                            }
                            cls.stock_qty = double.Parse(OraReader["stock_qty"].ToString()).ToString("0.00");
                            cls.stock_value = double.Parse(OraReader["stock_value"].ToString()).ToString("0.000");
                            cls.loc_id = OraReader["Location_Id"].ToString();
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch (Exception ex)
                {
                    return lst;
                }
            }
        }

        public ICollection<Inv_Dashbord.FastMovingItemView> getFastMovingItemByQty(string store_no, string from_date, string to_date, string strConString, string strCompId, string strLang, int noOfTopItems = 10)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.FastMovingItemView> lst = new List<Inv_Dashbord.FastMovingItemView>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " Inv_ProductLedger.company_id='" + strCompId + "' and inv_productledger.QuantityOut>0 and inv_productledger.ModifiedDate >='" + from_date + "' and inv_productledger.ModifiedDate<= '" + to_date + "'";
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        where_clause += " and Inv_ProductLedger.location_id=@location_id";
                    }


                    sql = "select top " + noOfTopItems + " inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L, sum(Inv_ProductLedger.QuantityOut) as QuantityOut from Inv_ProductLedger inner join inv_productmaster on inv_productmaster.ProductId=Inv_ProductLedger.ProductId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId where " + where_clause + " group by inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L order by sum(Inv_ProductLedger.QuantityOut) desc";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    //cmd.Parameters.AddWithValue("@top_rec", noOfTopItems);
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        cmd.Parameters.AddWithValue("@location_id", store_no);
                    }
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.FastMovingItemView cls = new Inv_Dashbord.FastMovingItemView();
                            if (strLang == "2")
                            {
                                cls.item_name = OraReader["LProductName"].ToString();
                            }
                            else
                            {
                                cls.item_name = OraReader["EProductName"].ToString();
                            }
                            cls.qty = double.Parse(OraReader["QuantityOut"].ToString()).ToString("0.00");
                            cls.item_id = OraReader["ProductId"].ToString();
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch (Exception ex)
                {
                    return lst;
                }
            }
        }

        public ICollection<Inv_Dashbord.StockByBrand> getBrandWisesStock(string store_no, string from_date, string to_date, string strConString, string strCompId, string strLang)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.StockByBrand> lst = new List<Inv_Dashbord.StockByBrand>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " Inv_Product_Brand.company_id='" + strCompId + "' ";
                    if (!string.IsNullOrEmpty(store_no))
                    {
                        where_clause = where_clause + " and inv_stockdetail.location_id=@location_id";
                    }
                    sql = "select Inv_ProductBrandMaster.brand_name,Inv_ProductBrandMaster.Brand_Name_L,Inv_ProductBrandMaster.PBrandId, sum(Inv_StockDetail.Quantity) as stock_qty ,sum(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join inv_productmaster on inv_productmaster.ProductId=Inv_StockDetail.ProductId inner join Inv_Product_Brand on Inv_Product_Brand.ProductId=Inv_ProductMaster.ProductId inner join Inv_ProductBrandMaster on Inv_ProductBrandMaster.PBrandId=Inv_Product_Brand.PBrandId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId where " + where_clause + " group by Inv_ProductBrandMaster.brand_name,Inv_ProductBrandMaster.Brand_Name_L,Inv_ProductBrandMaster.PBrandId";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        cmd.Parameters.AddWithValue("@location_id", int.Parse(store_no));
                    }
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.StockByBrand cls = new Inv_Dashbord.StockByBrand();
                            if (strLang == "2")
                            {
                                cls.brand_name = OraReader["Brand_Name_L"].ToString();
                            }
                            else
                            {
                                cls.brand_name = OraReader["brand_name"].ToString();
                            }
                            cls.stock_qty = double.Parse(OraReader["stock_qty"].ToString()).ToString("0.00");
                            cls.stock_value = double.Parse(OraReader["stock_value"].ToString()).ToString("0.000");
                            cls.brand_id = OraReader["PBrandId"].ToString();
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch
                {
                    return lst;
                }
            }
        }

        public ICollection<Inv_Dashbord.ProductStockView> getProductWiseStockByBrand(string store_no, string from_date, string to_date, string brand_id, string strConString, string strCompId, string strLang)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.ProductStockView> lst = new List<Inv_Dashbord.ProductStockView>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " Inv_Product_Brand.company_id='" + strCompId + "' and Inv_Product_Brand.PBrandId=@brand_id and Inv_StockDetail.Quantity<>0 ";
                    if (!string.IsNullOrEmpty(store_no))
                    {
                        where_clause = where_clause + " and inv_stockdetail.location_id=@location_id";
                    }
                    sql = "select inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L, Inv_StockDetail.Quantity as stock_qty ,(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join inv_productmaster on inv_productmaster.ProductId=Inv_StockDetail.ProductId inner join Inv_Product_Brand on Inv_Product_Brand.ProductId=Inv_ProductMaster.ProductId inner join Inv_ProductBrandMaster on Inv_ProductBrandMaster.PBrandId=Inv_Product_Brand.PBrandId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId where " + where_clause;

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@brand_id", int.Parse(brand_id));
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        cmd.Parameters.AddWithValue("@location_id", int.Parse(store_no));
                    }

                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.ProductStockView cls = new Inv_Dashbord.ProductStockView();
                            if (strLang == "2")
                            {
                                cls.product_name = OraReader["LProductName"].ToString();
                                cls.unit_name = OraReader["Unit_Name_L"].ToString();
                            }
                            else
                            {
                                cls.product_name = OraReader["EProductName"].ToString();
                                cls.unit_name = OraReader["Unit_Name"].ToString();
                            }
                            cls.stock_qty = double.Parse(OraReader["stock_qty"].ToString()).ToString("0.00");
                            cls.stock_value = double.Parse(OraReader["stock_value"].ToString()).ToString("0.000");

                            cls.product_id = OraReader["ProductId"].ToString();

                            if (double.Parse(cls.stock_value) != 0)
                            {
                                cls.cost_price = Math.Abs(double.Parse(cls.stock_value) / double.Parse(cls.stock_qty)).ToString("0.000");
                            }
                            else
                            {
                                cls.cost_price = "0.000";
                            }
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch
                {
                    return lst;
                }
            }
        }

        public ICollection<Inv_Dashbord.ProductSalesDetailView> getProductSalesDetailByProductId(string store_no, string from_date, string to_date, string product_id, string strConString, string strCompId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                List<Inv_Dashbord.ProductSalesDetailView> lst = new List<Inv_Dashbord.ProductSalesDetailView>() { };
                try
                {
                    con.Open();
                    string sql = string.Empty;
                    string where_clause = " Inv_ProductLedger.company_id='" + strCompId + "' and Inv_ProductLedger.ProductId=@product_id and inv_productledger.QuantityOut>0 and inv_productledger.ModifiedDate >='" + from_date + "' and inv_productledger.ModifiedDate<= '" + to_date + "' ";
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        where_clause += " and Inv_ProductLedger.location_id=@location_id";
                    }

                    sql = "select inv_productmaster.ProductId,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L, Inv_ProductLedger.QuantityOut,Inv_ProductLedger.field1,Inv_ProductLedger.TransType,Inv_ProductLedger.TransTypeId,Inv_ProductLedger.ModifiedDate from Inv_ProductLedger inner join inv_productmaster on inv_productmaster.ProductId=Inv_ProductLedger.ProductId left join Inv_UnitMaster on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId where " + where_clause;
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@product_id", product_id);
                    if (!string.IsNullOrEmpty(store_no) && store_no != "0")
                    {
                        cmd.Parameters.AddWithValue("@location_id", store_no);
                    }
                    SqlDataReader OraReader = cmd.ExecuteReader();
                    if (OraReader.HasRows)
                    {
                        while (OraReader.Read())
                        {
                            Inv_Dashbord.ProductSalesDetailView cls = new Inv_Dashbord.ProductSalesDetailView();
                            cls.trans_type = OraReader["TransType"].ToString();
                            cls.trans_no = OraReader["TransTypeId"].ToString();
                            cls.trans_date = DateTime.Parse(OraReader["ModifiedDate"].ToString()).ToString("dd-MMM-yyyy");
                            cls.qty = double.Parse(OraReader["QuantityOut"].ToString()).ToString("0.00");
                            cls.unit_price = double.Parse(OraReader["field1"].ToString()).ToString("0.000");
                            cls.line_total = (double.Parse(OraReader["QuantityOut"].ToString()) * double.Parse(OraReader["field1"].ToString())).ToString("0.000");
                            lst.Add(cls);
                        }
                    }
                    OraReader.Close();
                    return lst;
                }
                catch
                {
                    return lst;
                }
            }
        }
    }
}
