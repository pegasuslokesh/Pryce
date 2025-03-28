using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;

public partial class Inventory_ModelProductList : System.Web.UI.Page
{
    Inv_ProductMaster ObjProductMaster = null;
    Common cmn = null;
    SystemParameter ObjSysPeram = null;
    LocationMaster objLocation = null;
    DataAccessClass objDa = null;
    Inv_StockDetail objStockDetail = null;
    PageControlCommon objPageCmn = null;
    string SortExpression = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            FillDataListGrid();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {


        if (ddlOption.SelectedIndex != 0)
        {

            string condition = string.Empty;
            if (ddlFieldName.SelectedIndex == 3)
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + ddlitemtypeserach.SelectedValue.Trim() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + ddlitemtypeserach.SelectedValue.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '" + ddlitemtypeserach.SelectedValue.Trim() + "%'";
                }

            }
            else
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                }
            }

            DataView view = new DataView();


            DataTable dtProduct = (DataTable)Session["dtModelProduct"];
            //for filter discontinue product
            //filter option added by jitendra upadhyay according the discussion with pooja mam
            //created on 03-08-2016

            view = new DataView(dtProduct, condition, "", DataViewRowState.CurrentRows);


            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            Session["dtModelProductFilter"] = view.ToTable();
            objPageCmn.FillData((object)dtlistProduct, view.ToTable(), "", "");
            objPageCmn.FillData((object)gvProduct, view.ToTable(), "", "");
            txtValue.Focus();
        }

    }
    protected void ddlFieldName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 3)
        {
            ddlitemtypeserach.Visible = true;
            txtValue.Visible = false;
            ddlitemtypeserach.SelectedIndex = 0;
        }
        else
        {
            ddlitemtypeserach.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
        }
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {

        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlitemtypeserach.Visible = false;
        txtValue.Visible = true;
        ddlitemtypeserach.SelectedIndex = 0;
        txtValue.Focus();
        FillDataListGrid();


    }
    private void FillDataListGrid()
    {

        int counter = 0;

        DataTable dtproduct = objDa.return_DataTable("select (select Category_Name from inv_product_categorymaster where category_id in (select top 1 categoryid from inv_product_category where productid=Pm.ProductId)) as CategoryName,(select inv_product_Image.Field1 from inv_product_Image where inv_product_Image.ProductId=Pm.ProductId) as PImage, pm.ProductId,Pm.ProductCode,Pm.EProductName,SUBSTRING(PM.EProductName,0,100) as ShortProductName,       case when PM.ItemType='S' then 'Stockable'               when PM.ItemType='NS' then 'Non Stockable'               when PM.ItemType='A' then 'Assemble'               when PM.ItemType='K' then 'Kit' end as ItemTypeValue,          Inv_UnitMaster.Unit_Name as UnitName,          Pm.SalesPrice1 as ProductSalesPrice,             case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Pm.CreatedBy)=0 then 'Superadmin'           else  substring((select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.emp_id=  (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Pm.CreatedBy)),0,17) end as CreatedEmpName,         case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Pm.modifiedBy)=0 then 'Superadmin'           else  substring((select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.emp_id=  (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=pm.modifiedBy)),0,17) end as ModifiedEmpName  ,     case when     (select inv_stockdetail.quantity from inv_stockdetail where inv_stockdetail.Company_Id=" + Session["CompId"].ToString() + " And inv_stockdetail.Brand_Id=" + Session["BrandId"].ToString() + " And inv_stockdetail.Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + " and inv_stockdetail.ProductId=PM.ProductId and  Inv_StockDetail.Finance_Year_Id = " + Session["FinanceYearId"].ToString() + ") IS null then   0     else  cast((select inv_stockdetail.quantity from inv_stockdetail where inv_stockdetail.Company_Id=" + Session["CompId"].ToString() + " And inv_stockdetail.Brand_Id=" + Session["BrandId"].ToString() + " And inv_stockdetail.Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + " and inv_stockdetail.ProductId=PM.ProductId and   Inv_StockDetail.Finance_Year_Id = " + Session["FinanceYearId"].ToString() + ") as numeric(18,3))          end  as StockQty                                   from Inv_ProductMaster as PM inner join Inv_ModelMaster on PM.ModelNo=Inv_ModelMaster.Trans_Id  Inner join Inv_UnitMaster on pm.UnitId=Inv_UnitMaster.Unit_Id where PM.Company_Id=" + Session["CompId"].ToString() + " and PM.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_ModelMaster.Model_No='" + Request.QueryString["ModelNo"].ToString() + "' and PM.IsActive='True' and PM.Field1=' ' order by pm.ProductId");

        DataTable dtproductBrandSearch = new DataTable();
        DataTable dtproductCateSearch = new DataTable();
        string strsearchProductId = string.Empty;

        //for filter discontinue product
        //filter option added by jitendra upadhyay according the discussion with pooja mam
        //created on 03-08-2016
        Session["dtModelProduct"] = dtproduct;
        Session["dtModelProductFilter"] = dtproduct;
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
        objPageCmn.FillData((object)dtlistProduct, dtproduct, "", "");
        objPageCmn.FillData((object)gvProduct, dtproduct, "", "");
    }
    protected void imbBtnGrid_Click(object sender, ImageClickEventArgs e)
    {
        dtlistProduct.Visible = false;
        gvProduct.Visible = true;
        FillDataListGrid();
        imgBtnDatalist.Visible = true;
        imbBtnGrid.Visible = false;
        txtValue.Focus();

    }

    protected void imgBtnDatalist_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        dtlistProduct.Visible = true;
        gvProduct.Visible = false;
        FillDataListGrid();
        imgBtnDatalist.Visible = false;
        imbBtnGrid.Visible = true;
        txtValue.Focus();
    }

    protected void imgExport_Click(object sender, ImageClickEventArgs e)
    {
        if (gvProduct.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition",
            "attachment;filename=ModelProductList.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //GVTrailBalance.AllowPaging = false;
            //GVTrailBalance.DataBind();

            //Change the Header Row back to white color
            //GVComplete.HeaderRow.Style.Add("background-color", "#FFFFFF");

            //Apply style to Individual Cells
            //GVComplete.HeaderRow.Cells[0].Style.Add("background-color", "green");
            //GVComplete.HeaderRow.Cells[1].Style.Add("background-color", "green");
            //GVComplete.HeaderRow.Cells[2].Style.Add("background-color", "green");
            //GVTrailBalance.HeaderRow.Cells[3].Style.Add("background-color", "green");

            for (int i = 0; i < gvProduct.Rows.Count; i++)
            {
                GridViewRow row = gvProduct.Rows[i];

                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;

                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");

                //Apply style to Individual Cells of Alternating Row
                if (i % 2 != 0)
                {
                    // row.Cells[0].Style.Add("background-color", "#C2D69B");
                    // row.Cells[1].Style.Add("background-color", "#C2D69B");
                    // row.Cells[2].Style.Add("background-color", "#C2D69B");
                    //row.Cells[3].Style.Add("background-color", "#C2D69B");
                }
            }
            gvProduct.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            DisplayMessage("Record Not Available");
        }
    }



    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProduct.PageIndex = e.NewPageIndex;
        objPageCmn.FillData((object)gvProduct, (DataTable)Session["dtModelProductFilter"], "", "");

        gvProduct.BottomPagerRow.Focus();

    }
    public string GetSalesPriceUsingID(string ProductID)
    {
        double dUnitCost = 0;
        try
        {
            double.TryParse(ObjProductMaster.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", "0", ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString(), out dUnitCost);
        }
        catch
        {

        }
        return dUnitCost.ToString();
    }
    public string GetSalesPriceinLocal(string Amount)
    {

        string Exchangerate = string.Empty;
        try
        {

            Exchangerate = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        }
        catch
        {
            Exchangerate = "1";
        }
        string SalesPrice = string.Empty;
        try
        {

            SalesPrice = ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(Amount) * Convert.ToDouble(Exchangerate)).ToString());
        }
        catch
        {
            SalesPrice = "0";
        }

        return SalesPrice;
    }



    protected void GvProductDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((HiddenField)e.Row.FindControl("hdngvProductId")).Value;
            DataList dtlistStock = (DataList)e.Row.FindControl("dtlistStock");
            objPageCmn.FillData((object)dtlistStock, FillStockLocation(ProductID), "", "");
        }
    }



    public DataTable FillStockLocation(string strProductId)
    {
        DataTable dt = objStockDetail.GetStockDetail(Session["CompId"].ToString(), strProductId, Session["FinanceYearId"].ToString());
        return dt;
    }

    public string SetDecimal(string amount)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysPeram.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);

    }


    protected void gvProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        SortExpression = e.SortExpression;
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtModelProductFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();

        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        //AllPageCode();
        gvProduct.HeaderRow.Focus();
    }
}