using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_ProductStock : BasePage
{
    Inv_ParameterMaster objParam = null;
    Inv_StockDetail ObjStockDetail = null;
    SystemParameter ObjSysParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    Common cmn = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_Product_Category ObjProductCate = null;
    Inv_ProductBrandMaster ObjProductBrand = null;
    PageControlCommon objPageCmn = null;
    Inv_Product_Brand objProductByBrand = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjProductCate = new Inv_Product_Category(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjProductBrand=new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        objProductByBrand=new Inv_Product_Brand(Session["DBConnection"].ToString());
        Page.Title = ObjSysParam.GetSysTitle();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductStock.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            btnExportExcel.Visible = clsPagePermission.bDownload;
            //gvProductStock.Columns[8].Visible =clsPagePermission.bShowCostPrice;
            FillddlLocation();
            fillGrid();

            //here we modify code for show cost price according the login user permission
            //code created by jitendra on 13-08-2016
            //try
            //{
            //    gvProductStock.Columns[5].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));

            //    gvProductStock.Columns[6].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            //}
            //catch
            //{

            //}
            FillProductBrand(ddlProductBrand);
            FillProductCategorySerch(ddlcategorysearch);
        }
        //AllPageCode();
    }

    private void FillProductCategorySerch(DropDownList ddl)
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        if (dsCategory.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddl, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }


    //this Code Is Add By Rahul Sharma on date 11-01-2024
    private void FillProductBrand(DropDownList ddl)
    {
        DataTable dtBrand = null;
        dtBrand = ObjProductBrand.GetProductBrandTrueAllData(Session["CompId"].ToString());
        if (dtBrand.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddl, dtBrand, "Brand_Name", "PBrandId");
        }
        else
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }





    public void fillGrid()
    {
        DataTable dt = new DataTable();

        if (ddlLocation.SelectedIndex == 0)
        {
            dt = ObjStockDetail.GetStockDetailByCompanyIdandBrandId(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
            {
                if (LocIds != "")
                {
                    dt = new DataView(dt, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }


            }
        }
        else
        {
            dt = ObjStockDetail.GetStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString());

        }



        string strCategoryFilter = string.Empty;
        if (ddlcategorysearch.SelectedIndex > 0)
        {

            DataTable dtProductCate = ObjProductCate.GetProductByCategoryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlcategorysearch.SelectedValue);
            for (int i = 0; i < dtProductCate.Rows.Count; i++)
            {
                strCategoryFilter += dtProductCate.Rows[i]["ProductId"].ToString() + ",";
            }

            if (strCategoryFilter != "")
            {
                dt = new DataView(dt, "ProductId in (" + strCategoryFilter.Substring(0, strCategoryFilter.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        //This Brand Filter Add By Rahul Sharma on date 11-01-2024
        string strBrandFilter = string.Empty;

        if(ddlProductBrand.SelectedIndex > 0)
        {
            DataTable dtProductByBrand = objProductByBrand.GetDataBrandId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlProductBrand.SelectedValue);
            if (dtProductByBrand.Rows.Count > 0)
            {
                for (int i = 0; i < dtProductByBrand.Rows.Count; i++)
                {
                    strBrandFilter += dtProductByBrand.Rows[i]["ProductId"].ToString() + ",";
                }
                if (strBrandFilter != "")
                {
                    dt = new DataView(dt, "ProductId in (" + strBrandFilter.Substring(0, strBrandFilter.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }

        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductStock, dt, "", "");       

        GridFooterTotal();
        ViewState["dtFilter"] = dt;
        Session["DtPl"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        //AllPageCode();
    }

    public void GridFooterTotal()
    {
        double SalesPrice1 = 0;
        double SalesPrice2 = 0;
        double SalesPrice3 = 0;
        double AverageCost = 0;

        foreach (GridViewRow gvrow in gvProductStock.Rows)
        {
            SalesPrice1 += Convert.ToDouble(((Label)gvrow.FindControl("lblSalesPrice1")).Text);
            SalesPrice2 += Convert.ToDouble(((Label)gvrow.FindControl("lblSalesPrice2")).Text);
            SalesPrice3 += Convert.ToDouble(((Label)gvrow.FindControl("lblSalesPrice3")).Text);
            AverageCost += Convert.ToDouble(((Label)gvrow.FindControl("lblAvgCost")).Text);
        }
        try
        {
            ((Label)gvProductStock.FooterRow.FindControl("lblFooterSalesPrice1")).Text = SetDecimal(SalesPrice1.ToString(), Session["CurrencyId"].ToString());
            ((Label)gvProductStock.FooterRow.FindControl("lblFooterSalesPrice2")).Text = SetDecimal(SalesPrice2.ToString(), Session["CurrencyId"].ToString());
            ((Label)gvProductStock.FooterRow.FindControl("lblFooterSalesPrice3")).Text = SetDecimal(SalesPrice3.ToString(), Session["CurrencyId"].ToString());
            ((Label)gvProductStock.FooterRow.FindControl("lblFooterAvgCost")).Text = SetDecimal(AverageCost.ToString(), Session["CurrencyId"].ToString());
        }
        catch
        {

        }
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string strvalue = string.Empty;

        strvalue = txtValue.Text.Trim();
        if (ddlOption.SelectedIndex != 0)
        {
            if (ddlFieldName.SelectedIndex == 2)
            {
                if (txtValue.Text == "")
                {
                    txtValue.Text = "0";
                }

                txtValue.Text = Math.Round(Convert.ToDouble(txtValue.Text)).ToString("0.000000");
            }

            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["DtPl"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);

            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvProductStock, view.ToTable(), "", "");


            txtValue.Text = strvalue;
            GridFooterTotal();

            txtValue.Focus();
        }
        //AllPageCode();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillGrid();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        //AllPageCode();
    }
    #region Location

    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, "All");
            ddlLocation.SelectedValue = Session["LocId"].ToString();

        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        ddlcategorysearch.SelectedIndex = 0;
        btnRefresh_Click(null, null);
    }
    #endregion
    #region PrintReport

    protected void IbtnPrint_Command(object sender, EventArgs e)
    {

        Session["DtProductStock"] = (DataTable)ViewState["dtFilter"];

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/ProductStockPrint.aspx','window','width=1024');", true);

    }
    #endregion
    public string SetDecimal(string amount, string CurrencyID)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(CurrencyID, amount);

    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["dtFilter"] != null)
        {
            DataTable dt = ViewState["dtFilter"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                //dt = dv.ToTable(false, "productCode","eProductName","Model_no","UnitName","field1", "field2","Quantity","LocationName");
                dt = dv.ToTable(false, "productCode", "eProductName", "Model_no", "ColourName", "SizeName", "UnitName", "SalesPrice1", "SalesPrice2", "SalesPrice3", "field2", "Quantity", "LocationName");
                //dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(0);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(1);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(3);
                //dt.Columns.RemoveAt(8);
                //dt.Columns.RemoveAt(8);

                dt.Columns["productCode"].SetOrdinal(0);
                dt.Columns["EproductName"].SetOrdinal(1);
                dt.Columns["Model_No"].SetOrdinal(2);
                dt.Columns["Unitname"].SetOrdinal(3);

                dt.Columns["ColourName"].SetOrdinal(4);
                dt.Columns["SizeName"].SetOrdinal(5);

                dt.Columns["Field2"].SetOrdinal(6);
                dt.Columns["Quantity"].SetOrdinal(7);
                dt.Columns["LocationName"].SetOrdinal(8);

                // dt.Columns["field1"].ColumnName = "UnitCost";
                dt.Columns["SalesPrice1"].ColumnName = "SalesPrice1";
                dt.Columns["SalesPrice2"].ColumnName = "SalesPrice2";
                dt.Columns["SalesPrice3"].ColumnName = "SalesPrice3";
                dt.Columns["field2"].ColumnName = "AverageCost";
                dt.Columns["Eproductname"].ColumnName = "ProductName";


                ExportTableData(dt);
            }
        }
    }

    public class productData
    {

        public string ProductCode { get; set; }
        public string EProductName { get; set; }
        public string Model_No { get; set; }
        public string Unitname { get; set; }
        public string UnitCost { get; set; }
        public string Averagecost { get; set; }
        public string Quantity { get; set; }
        public string Location { get; set; }
    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "ProductStock";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}
