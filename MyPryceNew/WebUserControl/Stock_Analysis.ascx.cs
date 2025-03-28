using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUserControl_Stock_Analysis : System.Web.UI.UserControl
{
    Inv_StockDetail objStockDetail = null;
    Common cmn = null;
    Inv_ProductMaster objProductMaster = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            objPageCmn.FillData((object)gvStockInfo, objStockDetail.GetStockDetail(Session["CompId"].ToString(), ((HiddenField)Parent.FindControl("hdnStockAnalysisProductId")).Value,Session["FinanceYearId"].ToString()), "", "");
            getProductDetail(((HiddenField)Parent.FindControl("hdnStockAnalysisProductId")).Value);
        }
        catch
        {

        }
    }


    public void getProductDetail(string strProductId)
    {
        lblproductid.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ProductCode"].ToString();
        lblProductName.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["EProductName"].ToString();
        lblModel.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ModelName"].ToString();
    }
}