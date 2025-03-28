using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inv_SalesDashboard
{
    public class SalesByLoc
    {
        public string loc_id { get; set; }
        public string loc_name { get; set; }
        public string total_sale { get; set; }
        public string total_cost { get; set; }
    }

    public class SalesByCategory
    {
        public string cat_id { get; set; }
        public string cat_name { get; set; }
        public string total_sale { get; set; }
        public string total_cost { get; set; }
    }

    public class SalesBySalesPerson
    {
        public string sman_id { get; set; }
        public string sales_person_name { get; set; }
        public string sales_quota { get; set; }
        public string current_invoice_amt { get; set; }
        public string current_order_amt { get; set; }
        public string current_forecast_amt { get; set; }
    }

    public class ProductSalesSummary
    {
        [Display(Name = "Product ID")]
        public string product_id { get; set; }
        [Display(Name = "Product Code")]
        public string product_code { get; set; }
        [Display(Name = "Product Name")]
        public string product_name { get; set; }
        [Display(Name = "Total Sales")]
        public string total_sale { get; set; }
        [Display(Name = "Total Cost")]
        public string total_cost { get; set; }
        [Display(Name = "Profit(%)")]
        public string profit_per { get; set; }
        [Display(Name = "Profit(Amt)")]
        public string profit_amt { get; set; }
    }

    public class SalesByBrand
    {
        public string brand_id { get; set; }
        public string brand_name { get; set; }
        public string total_sale { get; set; }
        public string total_cost { get; set; }
    }

    public class SalesByPayMethod
    {
        public string payment_id { get; set; }
        public string payment_method { get; set; }
        public string amount { get; set; }
    }

    public class SalesInvHeader
    {
        public string trans_id { get; set; }
        public string trns_no { get; set; }
        public string trns_date { get; set; }
        public string customer_name { get; set; }
        public string invoice_amount { get; set; }
        public string pay_amount { get; set; }
        public string created_by { get; set; }
        public string trns_type { get; set; }
    }

    public ICollection<SalesByLoc> getLocationWiseSales(string fDate, string tDate, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesByLoc> lst = new List<SalesByLoc>() { };
            try
            {
                con.Open();
                string sql = string.Empty;
                string strLocIds = string.Empty;
                string where_clause = " and sih.company_id=@compId";
                if (strEmpId != "0")
                {
                    string LocIds = new Common(strConString).GetRoleDataPermission(strRoleId, "L", strUserId, strCmpId, strEmpId);
                    strLocIds = LocIds.Substring(0, LocIds.Length - 1);
                    where_clause += " and sih.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locIds, ','))";
                }
                //sql = "select Set_LocationMaster.Location_Id,Set_LocationMaster.Location_Name,Set_LocationMaster.Location_Name_L,sum(Inv_StockDetail.Quantity) as stock_qty ,sum(Inv_StockDetail.Quantity * cast(Inv_StockDetail.Field2 as decimal)) as stock_value from inv_stockdetail inner join Set_LocationMaster on Set_LocationMaster.location_id=Inv_StockDetail.location_id where " + where_clause + " group by Set_LocationMaster.Location_Id,Set_LocationMaster.Location_Name,Set_LocationMaster.Location_Name_L";
                sql = @"select sih.Location_Id,lm.Location_Name,lm.Location_Name_L, sum(sih.invoice_costing) as total_cost,sum(sih.GrandTotal) as total_sale from inv_salesinvoiceheader sih 
inner join set_locationMaster lm on lm.Location_Id = sih.Location_Id
where sih.IsActive = 'true' and sih.Post = 'true' ";
                sql = sql + " " + where_clause + " and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate group by sih.Location_Id,lm.Location_Name,lm.Location_Name_L";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = fDate });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = tDate });
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                if (strEmpId != "0")
                {
                    cmd.Parameters.Add(new SqlParameter("@locIds", strLocIds));
                }

                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesByLoc cls = new SalesByLoc();
                        if (strLang == "2")
                        {
                            cls.loc_name = OraReader["Location_Name_L"].ToString();
                        }
                        else
                        {
                            cls.loc_name = OraReader["Location_Name"].ToString();
                        }
                        cls.total_sale = double.Parse(OraReader["total_sale"].ToString()).ToString("0.000");
                        cls.total_cost = double.Parse(OraReader["total_cost"].ToString()).ToString("0.000");
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

    public ICollection<SalesByCategory> getCategoryWiseSales(string store_no, string from_date, string to_date, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesByCategory> lst = new List<SalesByCategory>() { };
            try
            {
                con.Open();
                //sql = "SELECT * FROM (SELECT PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME,sum(SINV_DETAIL.QUANTITY) TOTAL_QTY FROM SINV_DETAIL INNER JOIN SINV_HEADER ON (SINV_DETAIL.COMPANY_CODE=SINV_HEADER.COMPANY_CODE AND SINV_DETAIL.BRAND_CODE=SINV_HEADER.BRAND_CODE AND SINV_DETAIL.TRANS_NO=SINV_HEADER.TRANS_NO AND SINV_DETAIL.TYPE_TRANS=SINV_HEADER.TYPE_TRANS) INNER JOIN PRODUCTS ON (PRODUCTS.COMPANY_CODE=SINV_DETAIL.COMPANY_CODE AND PRODUCTS.BRAND_CODE=SINV_DETAIL.BRAND_CODE AND PRODUCTS.PRODUCT_ID=SINV_DETAIL.PRODUCT_ID) WHERE SINV_HEADER.SDATE>='" + str30DaysBefore + "' AND SINV_HEADER.SDATE<='" + strCurrentDate + "' AND SINV_HEADER.COMPANY_CODE='P0001' GROUP BY PRODUCTS.PRODUCT_ID,PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME order by sum(SINV_DETAIL.QUANTITY) desc) where rownum<=5";
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

                sql = @"select sum(sid.Quantity * (sid.UnitPrice - sid.DiscountV + sid.TaxV)) total_sale,sum(sid.Quantity * sid.field2) as total_cost, pcm.Category_Name,pcm.Category_Name_L,pcm.Category_Id
from inv_salesinvoiceheader sih 
inner join inv_salesinvoicedetail sid
on sih.Trans_Id=sid.Invoice_No
inner join Inv_Product_Category pc
on pc.ProductId=sid.Product_Id
inner join Inv_Product_CategoryMaster pcm
on pcm.Category_Id=pc.CategoryId
where  sih.IsActive = 'true'  and sih.company_id=@compId and sih.location_id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and sih.Post = 'true' 
group by pcm.Category_Id,pcm.Category_Name,pcm.Category_Name_L";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesByCategory cls = new SalesByCategory();
                        if (strLang == "2")
                        {
                            cls.cat_name = OraReader["Category_Name_L"].ToString();
                        }
                        else
                        {
                            cls.cat_name = OraReader["Category_Name"].ToString();
                        }
                        cls.total_sale = double.Parse(OraReader["total_sale"].ToString()).ToString("0.000");
                        cls.total_cost = double.Parse(OraReader["total_cost"].ToString()).ToString("0.000");
                        cls.cat_id = OraReader["Category_Id"].ToString();
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

    public ICollection<Inv_SalesDashboard.ProductSalesSummary> getProductWiseSales(string store_no, string from_date, string to_date, string category_id, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<Inv_SalesDashboard.ProductSalesSummary> lst = new List<Inv_SalesDashboard.ProductSalesSummary>() { };
            try
            {
                con.Open();
                //sql = "SELECT * FROM (SELECT PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME,sum(SINV_DETAIL.QUANTITY) TOTAL_QTY FROM SINV_DETAIL INNER JOIN SINV_HEADER ON (SINV_DETAIL.COMPANY_CODE=SINV_HEADER.COMPANY_CODE AND SINV_DETAIL.BRAND_CODE=SINV_HEADER.BRAND_CODE AND SINV_DETAIL.TRANS_NO=SINV_HEADER.TRANS_NO AND SINV_DETAIL.TYPE_TRANS=SINV_HEADER.TYPE_TRANS) INNER JOIN PRODUCTS ON (PRODUCTS.COMPANY_CODE=SINV_DETAIL.COMPANY_CODE AND PRODUCTS.BRAND_CODE=SINV_DETAIL.BRAND_CODE AND PRODUCTS.PRODUCT_ID=SINV_DETAIL.PRODUCT_ID) WHERE SINV_HEADER.SDATE>='" + str30DaysBefore + "' AND SINV_HEADER.SDATE<='" + strCurrentDate + "' AND SINV_HEADER.COMPANY_CODE='P0001' GROUP BY PRODUCTS.PRODUCT_ID,PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME order by sum(SINV_DETAIL.QUANTITY) desc) where rownum<=5";
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

                //sql = "select Inv_ItemMaster.ItemId,Inv_ItemMaster.ItemName,sum(sinv_d.qty*sinv_d.unitPrice) as total_sale,sum(sinv_d.qty*sinv_d.UnitCost) as total_cost from dbo.POS_SalesHeader sinv_h inner join pos_salesDetail sinv_d on (sinv_h.InvNo=sinv_d.InvNo and sinv_h.PosNo=sinv_d.PosNo) inner join Set_BranchMaster on sinv_h.BranchID=set_branchMaster.branchId inner join Inv_ItemMaster on Inv_ItemMaster.itemId=sinv_d.itemId where sinv_h.companyId='1' and sinv_h.cancel='false' and convert(date,sinv_h.date) >='" + from_date + "' and convert(date,sinv_h.date) <='" + to_date + "' and Inv_ItemMaster.itemCategoryId='" + category_id + "' " + branchClause + " group by Inv_ItemMaster.ItemId,Inv_ItemMaster.ItemName";
                sql = @"select sum(sid.Quantity * (sid.UnitPrice - sid.DiscountV + sid.TaxV)) total_sale, sum(sid.Quantity * sid.field2) as total_cost, pm.ProductId,pm.ProductCode,pm.EProductName ,pm.LProductName
from inv_salesinvoiceheader sih 
inner join inv_salesinvoicedetail sid
on sih.Trans_Id=sid.Invoice_No
inner join inv_productmaster pm
on pm.ProductId=sid.Product_Id
inner join Inv_Product_Category pc
on pc.ProductId=pm.ProductId
where sih.IsActive = 'true' and sih.Post = 'true' and sih.company_id=@compId and sih.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and pc.CategoryId=@categoryId 
group by pm.ProductId,pm.EProductName ,pm.LProductName,pm.ProductCode";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@categoryId", category_id));
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
               
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        Inv_SalesDashboard.ProductSalesSummary cls = new Inv_SalesDashboard.ProductSalesSummary();
                        if (strLang == "2")
                        {
                            cls.product_name = OraReader["LProductName"].ToString();
                        }
                        else
                        {
                            cls.product_name = OraReader["EProductName"].ToString();
                        }
                        cls.total_sale = double.Parse(OraReader["total_sale"].ToString()).ToString("0.000");
                        cls.total_cost = double.Parse(OraReader["total_cost"].ToString()).ToString("0.000");
                        cls.product_id = OraReader["ProductId"].ToString();
                        cls.product_code = OraReader["ProductCode"].ToString();
                        //get profit % & amount
                        cls.profit_amt = (double.Parse(cls.total_sale) - double.Parse(cls.total_cost)).ToString("0.000");
                        if (double.Parse(cls.total_cost) > 0)
                        {
                            cls.profit_per = ((double.Parse(cls.profit_amt) / double.Parse(cls.total_cost)) * 100).ToString("0.00");
                        }
                        else
                        {
                            cls.profit_per = double.Parse(cls.total_sale).ToString("0.00");
                        }

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

    public ICollection<SalesBySalesPerson> getSalesPersonWiseSales(string store_no, string fDate, string tDate, string strCmpId, string strLang, string strConString,string strEmpId,string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesBySalesPerson> lst = new List<SalesBySalesPerson>() { };
            string strLocIds = string.Empty;
            try
            {
                con.Open();
                DateTime from_date = DateTime.Parse(fDate);
                DateTime to_date = DateTime.Parse(tDate);

                //sql = "SELECT * FROM (SELECT PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME,sum(SINV_DETAIL.QUANTITY) TOTAL_QTY FROM SINV_DETAIL INNER JOIN SINV_HEADER ON (SINV_DETAIL.COMPANY_CODE=SINV_HEADER.COMPANY_CODE AND SINV_DETAIL.BRAND_CODE=SINV_HEADER.BRAND_CODE AND SINV_DETAIL.TRANS_NO=SINV_HEADER.TRANS_NO AND SINV_DETAIL.TYPE_TRANS=SINV_HEADER.TYPE_TRANS) INNER JOIN PRODUCTS ON (PRODUCTS.COMPANY_CODE=SINV_DETAIL.COMPANY_CODE AND PRODUCTS.BRAND_CODE=SINV_DETAIL.BRAND_CODE AND PRODUCTS.PRODUCT_ID=SINV_DETAIL.PRODUCT_ID) WHERE SINV_HEADER.SDATE>='" + str30DaysBefore + "' AND SINV_HEADER.SDATE<='" + strCurrentDate + "' AND SINV_HEADER.COMPANY_CODE='P0001' GROUP BY PRODUCTS.PRODUCT_ID,PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME order by sum(SINV_DETAIL.QUANTITY) desc) where rownum<=5";
                string sql = string.Empty;
                string branchClause = string.Empty;
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

                //sql = "select Set_EmployeeMaster.EmployeeId,Set_EmployeeMaster.EmployeeName,sum(sinv_h.NetAmount) as amount from dbo.POS_SalesHeader sinv_h left join Set_EmployeeMaster on sinv_h.WaiterID=Set_EmployeeMaster.EmployeeId where sinv_h.companyId='" + HttpContext.Current.Session["companyCode"].ToString() + "' and sinv_h.cancel='false' and convert(date,sinv_h.date) >='" + from_date.ToString("dd-MMM-yyyy") + "' and convert(date,sinv_h.date) <='" + to_date.ToString("dd-MMM-yyyy") + "' " + branchClause + " group by Set_EmployeeMaster.EmployeeId,Set_EmployeeMaster.EmployeeName";
                sql = @"SELECT em.Emp_Name,
    em.emp_id,
    lm.Location_Name,
    isnull(sccd.total_sales_quota,0) as sales_quota,
    isnull(current_month_forcast.forcast_amt,0) as current_forcast_amt,
    isnull(current_month_sale.invoiced_amount,0) as current_invoice_amt,
    isnull(current_month_order.ordered_amount,0) as current_order_amt
  FROM set_employeemaster em
  inner join Inv_SalesCommissionConfiguration_Header scch
    on scch.Employee_Id=em.Emp_Id
    inner join (select SUM(Sales_Quota) as total_sales_quota,Ref_Id from dbo.Inv_SalesCommissionConfiguration_Detail group by Ref_Id) sccd
    on scch.Trans_Id=sccd.Ref_Id
  inner join Set_LocationMaster lm 
  on lm.Location_Id=em.Location_Id  
  left join (select sqh.Company_Id,sqh.Emp_Id,sum(sqh.Amount) as forcast_amt from Inv_SalesQuotationHeader sqh
    inner join Inv_SalesInquiryHeader sih
on sih.SInquiryID = sqh.SInquiry_No where sqh.IsActive='true' AND sqh.Field7>=@fDate AND sqh.Field7 <= @tDate
group by sqh.Company_Id,sqh.Emp_Id
) current_month_forcast
	on current_month_forcast.Emp_Id=em.Emp_Id
  left join
    (select sih.Company_Id,sih.SalesPerson_Id,sum(sih.GrandTotal) as invoiced_amount from Inv_SalesInvoiceHeader sih
where sih.IsActive='true' AND sih.Invoice_Date >= @fDate AND sih.Invoice_Date <= @tDate
group by sih.Company_Id,sih.SalesPerson_Id) current_month_sale
on current_month_sale.SalesPerson_Id = em.Emp_id
left join 
(select soh.Company_Id,soh.SalesPerson_Id,SUM(soh.netAmount) as ordered_amount 
from Inv_SalesOrderHeader soh 
where soh.IsActive='true' AND soh.SalesOrderDate >= @fDate AND soh.SalesOrderDate <=@tDate
and soh.trans_id not in  (select sid.SIFromTransNo from  (select * from Inv_SalesInvoiceDetail where SIFromTransType='S') sid
inner join  Inv_SalesInvoiceHeader sih
on sid.Invoice_No=sih.Trans_Id where sih.IsActive='true')
group by soh.Company_Id,soh.SalesPerson_Id)current_month_order
on current_month_order.SalesPerson_Id=em.Emp_Id	
where scch.isactive='true' and em.Field2='False'  and em.Location_Id in (SELECT CAST(Value AS int) FROM F_Split(@locationId, ','))";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@fDate", fDate));
                cmd.Parameters.Add(new SqlParameter("@tDate", tDate));
                cmd.Parameters.Add(new SqlParameter("@locationId", strLocIds));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesBySalesPerson cls = new SalesBySalesPerson();
                        if (strLang == "2")
                        {
                            cls.sales_person_name = OraReader["Emp_Name"].ToString();
                        }
                        else
                        {
                            cls.sales_person_name = OraReader["Emp_Name"].ToString();
                        }
                        if (string.IsNullOrEmpty(cls.sales_person_name))
                        {
                            cls.sales_person_name = "without Sales Person";
                        }
                        cls.sales_quota = double.Parse(OraReader["sales_quota"].ToString()).ToString("0.000");
                        cls.current_forecast_amt = double.Parse(OraReader["current_forcast_amt"].ToString()).ToString("0.000");
                        cls.current_invoice_amt = double.Parse(OraReader["current_invoice_amt"].ToString()).ToString("0.000");
                        cls.current_order_amt = double.Parse(OraReader["current_order_amt"].ToString()).ToString("0.000");
                        cls.sman_id= OraReader["emp_id"].ToString();
                        if (string.IsNullOrEmpty(cls.sman_id))
                        {
                            cls.sman_id = "0";
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

    public ICollection<SalesByBrand> getBrandWiseSales(string store_no, string fDate, string tDate, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesByBrand> lst = new List<SalesByBrand>() { };
            try
            {
                con.Open();
                DateTime from_date = DateTime.Parse(fDate);
                DateTime to_date = DateTime.Parse(tDate);

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

                sql = @"select sum(sid.Quantity * (sid.UnitPrice - sid.DiscountV + sid.TaxV)) total_sale,sum(sid.Quantity * sid.UnitCost) as total_cost, pbm.PBrandId,pbm.Brand_Name,pbm.Brand_Name_L
from inv_salesinvoiceheader sih 
inner join inv_salesinvoicedetail sid
on sih.Trans_Id=sid.Invoice_No
inner join Inv_Product_Brand pb
on pb.ProductId=sid.Product_Id
inner join Inv_ProductBrandMaster pbm
on pbm.PBrandId=pb.PBrandId
where  sih.IsActive = 'true' and sih.company_id=@compId and sih.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and sih.Post = 'true' 
group by pbm.PBrandId,pbm.Brand_Name,pbm.Brand_Name_L";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesByBrand cls = new SalesByBrand();
                        if (strLang == "2")
                        {
                            cls.brand_name = OraReader["Brand_Name_L"].ToString();
                        }
                        else
                        {
                            cls.brand_name = OraReader["Brand_Name"].ToString();
                        }
                        cls.total_sale = double.Parse(OraReader["total_sale"].ToString()).ToString("0.000");
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
    
    public ICollection<Inv_SalesDashboard.ProductSalesSummary> GetProductSummaryForBrand(string store_no, string from_date, string to_date, string brand_id, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<Inv_SalesDashboard.ProductSalesSummary> lst = new List<Inv_SalesDashboard.ProductSalesSummary>() { };
            try
            {
                con.Open();
                //sql = "SELECT * FROM (SELECT PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME,sum(SINV_DETAIL.QUANTITY) TOTAL_QTY FROM SINV_DETAIL INNER JOIN SINV_HEADER ON (SINV_DETAIL.COMPANY_CODE=SINV_HEADER.COMPANY_CODE AND SINV_DETAIL.BRAND_CODE=SINV_HEADER.BRAND_CODE AND SINV_DETAIL.TRANS_NO=SINV_HEADER.TRANS_NO AND SINV_DETAIL.TYPE_TRANS=SINV_HEADER.TYPE_TRANS) INNER JOIN PRODUCTS ON (PRODUCTS.COMPANY_CODE=SINV_DETAIL.COMPANY_CODE AND PRODUCTS.BRAND_CODE=SINV_DETAIL.BRAND_CODE AND PRODUCTS.PRODUCT_ID=SINV_DETAIL.PRODUCT_ID) WHERE SINV_HEADER.SDATE>='" + str30DaysBefore + "' AND SINV_HEADER.SDATE<='" + strCurrentDate + "' AND SINV_HEADER.COMPANY_CODE='P0001' GROUP BY PRODUCTS.PRODUCT_ID,PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME order by sum(SINV_DETAIL.QUANTITY) desc) where rownum<=5";
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

                //sql = "select Inv_ItemMaster.ItemId,Inv_ItemMaster.ItemName,sum(sinv_d.qty*sinv_d.unitPrice) as total_sale,sum(sinv_d.qty*sinv_d.UnitCost) as total_cost from dbo.POS_SalesHeader sinv_h inner join pos_salesDetail sinv_d on (sinv_h.InvNo=sinv_d.InvNo and sinv_h.PosNo=sinv_d.PosNo) inner join Set_BranchMaster on sinv_h.BranchID=set_branchMaster.branchId inner join Inv_ItemMaster on Inv_ItemMaster.itemId=sinv_d.itemId where sinv_h.companyId='1' and sinv_h.cancel='false' and convert(date,sinv_h.date) >='" + from_date + "' and convert(date,sinv_h.date) <='" + to_date + "' and Inv_ItemMaster.itemCategoryId='" + category_id + "' " + branchClause + " group by Inv_ItemMaster.ItemId,Inv_ItemMaster.ItemName";
                sql = @"select sum(sid.Quantity * (sid.UnitPrice - sid.DiscountV + sid.TaxV)) total_sale, sum(sid.Quantity * sid.UnitCost) as total_cost, pm.ProductId,pm.ProductCode,pm.EProductName ,pm.LProductName
from inv_salesinvoiceheader sih 
inner join inv_salesinvoicedetail sid
on sih.Trans_Id=sid.Invoice_No
inner join inv_productmaster pm
on pm.ProductId=sid.Product_Id
inner join Inv_Product_Brand pb
on pb.ProductId=sid.Product_Id
where sih.IsActive = 'true' and sih.Post = 'true' and sih.company_id=@compId and sih.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and pb.PBrandId=@brandId 
group by pm.ProductId,pm.EProductName ,pm.LProductName,pm.ProductCode";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@brandId", brand_id));
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        Inv_SalesDashboard.ProductSalesSummary cls = new Inv_SalesDashboard.ProductSalesSummary();
                        if (strLang == "2")
                        {
                            cls.product_name = OraReader["LProductName"].ToString();
                        }
                        else
                        {
                            cls.product_name = OraReader["EProductName"].ToString();
                        }
                        cls.total_sale = double.Parse(OraReader["total_sale"].ToString()).ToString("0.000");
                        cls.total_cost = double.Parse(OraReader["total_cost"].ToString()).ToString("0.000");
                        cls.product_id = OraReader["ProductId"].ToString();
                        cls.product_code = OraReader["ProductCode"].ToString();
                        //get profit % & amount
                        cls.profit_amt = (double.Parse(cls.total_sale) - double.Parse(cls.total_cost)).ToString("0.000");
                        if (double.Parse(cls.total_cost) > 0)
                        {
                            cls.profit_per = ((double.Parse(cls.profit_amt) / double.Parse(cls.total_cost)) * 100).ToString("0.00");
                        }
                        else
                        {
                            cls.profit_per = double.Parse(cls.total_sale).ToString("0.00");
                        }

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

    public ICollection<SalesByPayMethod> getPayMethodWiseSales(string store_no, string fDate, string tDate, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesByPayMethod> lst = new List<SalesByPayMethod>() { };
            try
            {
                con.Open();
                DateTime from_date = DateTime.Parse(fDate);
                DateTime to_date = DateTime.Parse(tDate);

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

                sql = @"select sum(sih.GrandTotal) as invoice_amount,sum(pt.Pay_Charges) as pay_amount, pmm.Pay_Mod_Name,pmm.Pay_Mod_Name_L
from inv_salesinvoiceheader sih 
inner join (select transNo,Pay_Charges,PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans='SI')pt
on pt.TransNo=sih.Trans_Id
inner join Set_Payment_Mode_Master pmm
on pmm.Pay_Mode_Id=pt.PaymentModeId
where  sih.IsActive = 'true' and sih.company_id=@compId and sih.location_id in (SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and sih.Post = 'true' 
group by pmm.Pay_Mod_Name,pmm.Pay_Mod_Name_L";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesByPayMethod cls = new SalesByPayMethod();
                        if (strLang == "2")
                        {
                            cls.payment_method = OraReader["Pay_Mod_Name_L"].ToString();
                        }
                        else
                        {
                            cls.payment_method = OraReader["Pay_Mod_Name"].ToString();
                        }
                        cls.amount = double.Parse(OraReader["pay_amount"].ToString()).ToString("0.000");
                        cls.payment_id = "0";
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

    public ICollection<SalesInvHeader> getPayMethodWiseSinvHeader(string store_no, string fDate, string tDate, string payment_id, string strCmpId, string strLang, string strConString, string strEmpId, string strRoleId, string strUserId)
    {
        using (SqlConnection con = new SqlConnection(strConString))
        {
            List<SalesInvHeader> lst = new List<SalesInvHeader>() { };
            try
            {
                con.Open();
                DateTime from_date = DateTime.Parse(fDate);
                DateTime to_date = DateTime.Parse(tDate);

                //sql = "SELECT * FROM (SELECT PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME,sum(SINV_DETAIL.QUANTITY) TOTAL_QTY FROM SINV_DETAIL INNER JOIN SINV_HEADER ON (SINV_DETAIL.COMPANY_CODE=SINV_HEADER.COMPANY_CODE AND SINV_DETAIL.BRAND_CODE=SINV_HEADER.BRAND_CODE AND SINV_DETAIL.TRANS_NO=SINV_HEADER.TRANS_NO AND SINV_DETAIL.TYPE_TRANS=SINV_HEADER.TYPE_TRANS) INNER JOIN PRODUCTS ON (PRODUCTS.COMPANY_CODE=SINV_DETAIL.COMPANY_CODE AND PRODUCTS.BRAND_CODE=SINV_DETAIL.BRAND_CODE AND PRODUCTS.PRODUCT_ID=SINV_DETAIL.PRODUCT_ID) WHERE SINV_HEADER.SDATE>='" + str30DaysBefore + "' AND SINV_HEADER.SDATE<='" + strCurrentDate + "' AND SINV_HEADER.COMPANY_CODE='P0001' GROUP BY PRODUCTS.PRODUCT_ID,PRODUCTS.ENG_NAME,PRODUCTS.ARB_NAME order by sum(SINV_DETAIL.QUANTITY) desc) where rownum<=5";
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

                sql = @"select sih.trans_id,sih.invoice_no,sih.GrandTotal as invoice_amount, sih.Invoice_Date, pt.Pay_Charges as pay_amount,em.Emp_Name as created_by,cm.Name as customer_name
from inv_salesinvoiceheader sih 
inner join (select transNo,Pay_Charges,PaymentModeId from dbo.Inv_PaymentTrn where TypeTrans='SI')pt
on pt.TransNo=sih.Trans_Id
inner join Set_Payment_Mode_Master pmm
on pmm.Pay_Mode_Id=pt.PaymentModeId
left join Set_UserMaster um
on um.User_Id=sih.createdby
left join Set_EmployeeMaster em
on em.Emp_Id=um.Emp_Id
left join Ems_ContactMaster cm
on cm.Trans_Id=sih.Supplier_Id
where  sih.IsActive = 'true' and pmm.Pay_Mod_Name=@payMode and sih.company_id=@compId and sih.location_id in(SELECT CAST(Value AS int) AS trans_id FROM F_Split(@locId, ',')) and sih.Invoice_Date >= @fDate and sih.Invoice_Date <= @tDate and sih.Post = 'true'";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@compId", strCmpId));
                cmd.Parameters.Add(new SqlParameter("@fDate", System.Data.SqlDbType.DateTime) { Value = from_date });
                cmd.Parameters.Add(new SqlParameter("@tDate", System.Data.SqlDbType.DateTime) { Value = to_date });
                cmd.Parameters.Add(new SqlParameter("@locId", strLocIds));
                cmd.Parameters.Add(new SqlParameter("@payMode", payment_id));

                SqlDataReader OraReader = cmd.ExecuteReader();
                if (OraReader.HasRows)
                {
                    while (OraReader.Read())
                    {
                        SalesInvHeader cls = new SalesInvHeader();
                        cls.trans_id= OraReader["trans_id"].ToString();
                        cls.trns_no = OraReader["invoice_no"].ToString();
                        cls.invoice_amount = double.Parse(OraReader["invoice_amount"].ToString()).ToString("0.000");
                        cls.pay_amount = double.Parse(OraReader["pay_amount"].ToString()).ToString("0.000");
                        cls.created_by = OraReader["created_by"].ToString();
                        cls.customer_name= OraReader["customer_name"].ToString(); 
                        cls.trns_date = DateTime.Parse(OraReader["invoice_date"].ToString()).ToString("dd-MMM-yyyy");
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

