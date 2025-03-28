using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Inv_Dashbord
/// </summary>
public class Inv_Dashbord
{
    public Inv_Dashbord()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class StockByCategory
    {
        public string cat_id { get; set; }
        public string cat_name { get; set; }
        public string stock_qty { get; set; }
        public string stock_value { get; set; }
    }

    public class StockByLocation
    {
        public string loc_id { get; set; }
        public string loc_name { get; set; }
        public string stock_qty { get; set; }
        public string stock_value { get; set; }
    }

    public class ProductStockView
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string unit_name { get; set; }
        public string stock_qty { get; set; }
        public string cost_price { get; set; }
        public string stock_value { get; set; }
    }

    public class FastMovingItemView
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string qty { get; set; }
        public string amount { get; set; }
    }

    public class DashboardFilter
    {
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string store_no { get; set; }
        public string filter_type { get; set; }
    }

    public enum enmPurchaseType
    {
        All,
        Local,
        Foreign
    }

    public class StockByBrand
    {
        public string brand_id { get; set; }
        public string brand_name { get; set; }
        public string stock_qty { get; set; }
        public string stock_value { get; set; }
    }

    public enum MyDashboardFilter
    {
        [Display(Name = "Today")]
        Today,
        [Display(Name = "Yesterday")]
        Yesterday,
        [Display(Name = "This Week")]
        ThisWeek,
        [Display(Name = "This Month")]
        ThisMonth,
        [Display(Name = "This Quarter")]
        ThisQuarter,
        [Display(Name = "This Year")]
        ThisYear,
        [Display(Name = "Custom")]
        Custom
    }

    public class ProductSalesDetailView
    {
        public string trans_type { get; set; }
        public string trans_no { get; set; }
        public string trans_date { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string qty { get; set; }
        public string unit_price { get; set; }
        public string unit_cost { get; set; }
        public string line_total { get; set; }
        public string reference { get; set; }
        public string unit_name { get; set; }
    }
    public static DashboardFilter getDashboardFilterParamValues(MyDashboardFilter filterType, string fromDate, string toDate, string storeNo)
    {
        DashboardFilter cls = new DashboardFilter();
        try
        {
            //DateTime todayDate = Common.GetTimeZoneDate(DateTime.UtcNow, HttpContext.Current.Session["timeZone"].ToString());
            DateTime todayDate = DateTime.Now;
            switch (filterType)
            {
                case MyDashboardFilter.Today:
                    cls.from_date = cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.Yesterday:
                    cls.from_date = cls.to_date = todayDate.AddDays(-1).ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisWeek:
                    cls.from_date = todayDate.AddDays(-1 * (int)todayDate.DayOfWeek).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisMonth:
                    cls.from_date = (new DateTime(todayDate.Year, todayDate.Month, 1)).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisQuarter:
                    var current_month = todayDate.Month;
                    if (current_month >= 1 && current_month <= 3)
                    {
                        cls.from_date = (new DateTime(todayDate.Year, 1, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 4 && current_month <= 6)
                    {
                        cls.from_date = (new DateTime(todayDate.Year, 4, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 7 && current_month <= 9)
                    {
                        cls.from_date = (new DateTime(todayDate.Year, 7, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 9 && current_month <= 12)
                    {
                        cls.from_date = (new DateTime(todayDate.Year, 9, 1)).ToString("dd-MMM-yyyy");
                    }
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisYear:
                    cls.from_date = (new DateTime(todayDate.Year, 1, 1)).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.Custom:
                    cls.from_date = DateTime.Parse(fromDate).ToString("dd-MMM-yyyy");
                    cls.to_date = DateTime.Parse(toDate).ToString("dd-MMM-yyyy");
                    break;
            }
            //DateTime NewDate = Common.GetTimeZoneDate(DateTime.Parse(cls.from_date), HttpContext.Current.Session["timeZone"].ToString());
            cls.filter_type = ((int)filterType).ToString();
            cls.store_no = storeNo;

            return cls;
        }
        catch (Exception ex)
        {
            return cls;
        }

    }

    public ICollection<StockByCategory> getCategoryWisesStock(string store_no, string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<StockByCategory> lst = new List<StockByCategory>() { };
            try
            {
                con.Open();
                string sql = string.Empty;

                string strLocIds = string.Empty;
                if (!string.IsNullOrEmpty(store_no))
                {
                    strLocIds = store_no;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }

                }

                string strFYearId = Common.getFinancialYearId(DateTime.Parse(to_date), strConString, strCmpId).ToString();

                sql = @"select Inv_Product_CategoryMaster.Category_Id,Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Name_L,
sum(case when pl.Finance_Year_Id<>@fYearId and pl.TransType='OP' then 0 else pl.QuantityIn - pl.QuantityOut end) as stock_qty,
sum((pl.QuantityIn - pl.QuantityOut) * pl.UnitPrice) as stock_value 
from Inv_ProductLedger pl
inner join inv_productmaster pm
on pl.ProductId=pm.ProductId
left join Inv_Product_Category 
on Inv_Product_Category.ProductId=pl.ProductId 
inner join Inv_Product_CategoryMaster 
on Inv_Product_Category.CategoryId=Inv_Product_CategoryMaster.Category_Id 
where pl.Company_Id=@compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and pm.ItemType='S' 
and ((pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate) or pl.Finance_Year_Id=@fYearId)
group by Inv_Product_CategoryMaster.Category_Id,Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Name_L";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@fYearId", strFYearId));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        StockByCategory cls = new StockByCategory();
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

    public ICollection<ProductStockView> getProductWiseStockByCategory(string location_id, string from_date, string to_date, string category_id, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<ProductStockView> lst = new List<ProductStockView>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;
                if (!string.IsNullOrEmpty(location_id))
                {
                    strLocIds = location_id;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }
                }

                string strFYearId = Common.getFinancialYearId(DateTime.Parse(to_date), strConString, strCmpId).ToString();

                //sql = "select inv_itemmaster.ItemID,inv_itemmaster.ItemName,inv_itemmaster.Item_Name_L,Sys_UnitSetup.UnitName, sum(inv_stockmaster.StockQty) as stock_qty ,sum(inv_stockmaster.StockQty *inv_stockmaster.AvgCost) as stock_value from inv_stockmaster inner join inv_itemmaster on inv_itemmaster.ItemId=inv_stockmaster.itemId inner join Sys_UnitSetup on Sys_UnitSetup.unitId=inv_itemmaster.UnitID where " + where_clause + " group by inv_itemmaster.ItemID,inv_itemmaster.ItemName,inv_itemmaster.Item_Name_L,Sys_UnitSetup.UnitName";
                sql = @"select pm.ProductId,pm.EProductName,pm.LProductName,um.Unit_Name,um.Unit_Name_L, 
sum(case when pl.Finance_Year_Id<>@fYearId and pl.TransType='OP' then 0 else pl.QuantityIn - pl.QuantityOut end) as stock_qty,
sum((pl.QuantityIn - pl.QuantityOut) * pl.UnitPrice) as stock_value 
from Inv_ProductLedger pl 
inner join inv_productmaster pm 
on pm.ProductId=pl.ProductId 
inner join Inv_Product_Category pc 
on pc.ProductId=pm.ProductId 
inner join Inv_UnitMaster um 
on um.Unit_Id = pm.UnitId
where pl.Company_Id=@compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
and pm.ItemType='S' 
and ((pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate) or pl.Finance_Year_Id=@fYearId)
and pc.CategoryId=@catId 
group by pm.ProductId,pm.EProductName,pm.LProductName,um.Unit_Name,um.Unit_Name_L";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@catId", int.Parse(category_id));
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@fYearId", strFYearId));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        ProductStockView cls = new ProductStockView();
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
    public ICollection<StockByLocation> getStockByLocation(string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<StockByLocation> lst = new List<StockByLocation>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;

                if (strEmpId == "0")
                {
                    sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                    strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                }
                else
                {
                    string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                    strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                }

                string strFYearId = Common.getFinancialYearId(DateTime.Parse(to_date), strConString, strCmpId).ToString();

                sql = @"select lm.Location_Id,lm.Location_Name,lm.Location_Name_L,
sum(case when pl.Finance_Year_Id<>@fYearId and pl.TransType='OP' then 0 else pl.QuantityIn - pl.QuantityOut end) as stock_qty,
sum((pl.QuantityIn - pl.QuantityOut) * pl.UnitPrice) as stock_value 
from Inv_ProductLedger pl
inner
join inv_productmaster pm
on pl.ProductId = pm.ProductId
inner
join Set_LocationMaster lm
on lm.Location_Id = pl.Location_Id
where pl.Company_Id = @compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
and pm.ItemType = 'S' 
and ((pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate) or pl.Finance_Year_Id=@fYearId)
and pl.IsActive = 'true'
group by lm.Location_Id,lm.Location_Name,lm.Location_Name_L";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@fYearId", strFYearId));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        StockByLocation cls = new StockByLocation();
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

    public ICollection<FastMovingItemView> getFastMovingItemByQty(string store_no, string from_date, string to_date, string strConString, string strCmpId, string strLang, string strEmpId, string strRoleId, string strUserId,int noOfTopItems = 10)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<FastMovingItemView> lst = new List<FastMovingItemView>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                
                string strLocIds = string.Empty;
                if (!string.IsNullOrEmpty(store_no))
                {
                    strLocIds = store_no;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }
                }


                sql = "select top " + noOfTopItems + " inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L,";
                sql+=@"sum(Inv_ProductLedger.QuantityOut) as QuantityOut 
from Inv_ProductLedger 
inner join inv_productmaster 
on inv_productmaster.ProductId=Inv_ProductLedger.ProductId 
left join Inv_UnitMaster 
on Inv_UnitMaster.Unit_Id = Inv_ProductMaster.UnitId 
where Inv_ProductLedger.company_id=@compId and inv_productledger.QuantityOut>0 and inv_productledger.ModifiedDate >= @fDate and inv_productledger.ModifiedDate <= @tDate and Inv_ProductLedger.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
group by inv_productmaster.ProductId,Inv_ProductMaster.EProductName,Inv_ProductMaster.LProductName,Inv_UnitMaster.Unit_Name,Inv_UnitMaster.Unit_Name_L order by sum(Inv_ProductLedger.QuantityOut) desc";
                SqlCommand cmd = new SqlCommand(sql, con);
                //cmd.Parameters.AddWithValue("@top_rec", noOfTopItems);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        FastMovingItemView cls = new FastMovingItemView();
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

    public ICollection<StockByBrand> getBrandWisesStock(string store_no, string from_date, string to_date, string strConString, string strCmpId, string strLang, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<StockByBrand> lst = new List<StockByBrand>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;
                if (!string.IsNullOrEmpty(store_no))
                {
                    strLocIds = store_no;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }
                }

                string strFYearId = Common.getFinancialYearId(DateTime.Parse(to_date), strConString, strCmpId).ToString();

                sql = @"select pbm.brand_name,pbm.Brand_Name_L,pbm.PBrandId,
sum(case when pl.Finance_Year_Id<>@fYearId and pl.TransType='OP' then 0 else pl.QuantityIn - pl.QuantityOut end) as stock_qty,
sum((pl.QuantityIn - pl.QuantityOut) * pl.UnitPrice) as stock_value 
from Inv_ProductLedger pl
inner join inv_productmaster pm
on pl.ProductId=pm.ProductId
inner join Inv_Product_Brand pb
on pb.ProductId=pm.ProductId 
inner join Inv_ProductBrandMaster  pbm
on pbm.PBrandId=pb.PBrandId 
where pl.Company_Id=@compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
and pm.ItemType='S' 
and ((pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate) or pl.Finance_Year_Id=@fYearId)
and pl.IsActive='true'
group by pbm.PBrandId,pbm.brand_name,pbm.Brand_Name_L";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@fYearId", strFYearId));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        StockByBrand cls = new StockByBrand();
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
            catch(Exception ex)
            {
                return lst;
            }
        }
    }

    public ICollection<ProductStockView> getProductWiseStockByBrand(string store_no, string from_date, string to_date, string brand_id, string strConString, string strCmpId, string strLang, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<ProductStockView> lst = new List<ProductStockView>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;
                if (!string.IsNullOrEmpty(store_no))
                {
                    strLocIds = store_no;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }
                }

                string strFYearId = Common.getFinancialYearId(DateTime.Parse(to_date), strConString, strCmpId).ToString();

                sql = @"select pm.ProductId,pm.EProductName,pm.LProductName,um.Unit_Name,um.Unit_Name_L, 
sum(case when pl.Finance_Year_Id<>@fYearId and pl.TransType='OP' then 0 else pl.QuantityIn - pl.QuantityOut end) as stock_qty,
sum((pl.QuantityIn - pl.QuantityOut) * pl.UnitPrice) as stock_value 
from Inv_ProductLedger pl 
inner join inv_productmaster pm 
on pm.ProductId=pl.ProductId 
inner join Inv_Product_Brand pb
on pb.ProductId=pm.ProductId 
inner join Inv_UnitMaster um 
on um.Unit_Id = pm.UnitId
where pl.Company_Id=@compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
and pm.ItemType='S' 
and ((pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate) or pl.Finance_Year_Id=@fYearId)
and pb.PBrandId=@pBrandId 
group by pm.ProductId,pm.EProductName,pm.LProductName,um.Unit_Name,um.Unit_Name_L
having sum(pl.QuantityIn-pl.QuantityOut)<>0";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pBrandId", int.Parse(brand_id));
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@fYearId", strFYearId));

                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        ProductStockView cls = new ProductStockView();
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

    public ICollection<ProductSalesDetailView> getProductSalesDetailByProductId(string store_no, string from_date, string to_date, string product_id, string strConString, string strCmpId, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<ProductSalesDetailView> lst = new List<ProductSalesDetailView>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;

                if (!string.IsNullOrEmpty(store_no))
                {
                    strLocIds = store_no;
                }
                else
                {
                    if (strEmpId == "0")
                    {
                        sql = @"SELECT abc = STUFF((
            SELECT ',' + cast(location_id as varchar)
            FROM Set_LocationMaster where Company_Id='" + strCmpId + "' and IsActive='true' FOR XML PATH('') ), 1, 1, '')";
                        strLocIds = new PegasusDataAccess.DataAccessClass(strConString).get_SingleValue(sql);
                    }
                    else
                    {
                        string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                        strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    }
                }

                sql = @"select pm.ProductId,um.Unit_Name,um.Unit_Name_L, pl.QuantityOut,pl.field1,pl.TransType,pl.TransTypeId,pl.ModifiedDate 
from Inv_ProductLedger pl
inner join inv_productmaster pm
on pm.ProductId=pl.ProductId 
left join Inv_UnitMaster um
on um.Unit_Id = pm.UnitId 
where pl.Company_Id=@compId and pl.Location_Id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) 
and pl.ProductId=@productId
and pl.CreatedDate>=@fDate and pl.CreatedDate<=@tDate";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@productId", product_id);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        ProductSalesDetailView cls = new ProductSalesDetailView();
                        cls.trans_type = OraReader["TransType"].ToString();
                        cls.trans_no = OraReader["TransTypeId"].ToString();
                        cls.trans_date = DateTime.Parse(OraReader["ModifiedDate"].ToString()).ToString("dd-MMM-yyyy");
                        cls.qty = double.Parse(OraReader["QuantityOut"].ToString()).ToString("0.00");
                        cls.unit_name = OraReader["unit_name"].ToString();
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