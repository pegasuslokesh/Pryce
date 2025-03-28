using System;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
public partial class WebUserControl_StockAnalysis : System.Web.UI.UserControl
{
    Inv_StockDetail objStockDetail = null;
    Common cmn = null;
    Inv_ProductMaster objProductMaster = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductLedger ObjProductLadger = null;
    Inventory_Common objinvCommon = null;
    Inv_ModelMaster ObjModelMaster = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objinvCommon = new Inventory_Common(Session["DBConnection"].ToString());
        ObjModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillRefTye();
            FillddlLocation();
            //getProductDetail("103", "","");
        }
    }
    public void FillRefTye()
    {
        DataTable dt = ObjProductLadger.GetProductLedgerAll();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable(true, "Ref_Type", "TransType");
        Ddlreftype.DataSource = dt;
        Ddlreftype.DataTextField = "Ref_Type";
        Ddlreftype.DataValueField = "TransType";
        Ddlreftype.DataBind();
        Ddlreftype.Items.Insert(0, "ALL");
    }
    private void FillddlLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
    public void getProductDetail(string strProductId, string strtype, string ContactId)
    {
        hdnProductId.Value = strProductId;
        if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows.Count == 0)
        {
            return;
        }
        lblproductid.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ProductCode"].ToString();
        lblProductName.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["EProductName"].ToString();
        lblModel.Text = getModelNo(objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ModelNo"].ToString());

        DataTable dtData = objinvCommon.GetProductDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strProductId,Session["FinanceYearId"].ToString());
        dtData.Columns["Stock_Qty"].ColumnName = "Stock Qty";
        dtData.Columns["Unposted_Sales_Qty"].ColumnName = "Unposted Sales Qty";
        dtData.Columns["Unposted_Delivery_Qty"].ColumnName = "Unposted Delivery Qty";
        dtData.Columns["Sales_Order_Qty"].ColumnName = "Sales Order Qty";
        dtData.Columns["Damage_Qty"].ColumnName = "Damage Qty";
        dtData.Columns["Purchase_Order_Qty"].ColumnName = "Purchase Order Qty";

        objPageCmn.FillData((object)gvStockInfo, dtData, "", "");
        DataTable dt = new DataView(ObjProductLadger.GetProductLedgerAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + strProductId + "", "", DataViewRowState.CurrentRows).ToTable();
        if (strtype.Trim() == "SALES")
        {
            Ddlreftype.SelectedValue = "SI";
        }
        else if (strtype == "PURCHASE")
        {
            Ddlreftype.SelectedValue = "PG";
            try
            {
                gvProductLadger.Columns[3].Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24", "20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                gvProductLadger.Columns[5].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {
            }
        }
        FillGrid();
    }
    public string getModelNo(string ModelId)
    {
        string modelNo = string.Empty;
        if (ModelId == "")
        {
            ModelId = "0";
        }
        DataTable dtModel = ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, "True");
        if (dtModel.Rows.Count != 0)
        {
            modelNo = dtModel.Rows[0]["Model_No"].ToString();
        }
        return modelNo;
    }
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        if (ddlLocation.SelectedIndex == 0)
        {
            dt = new DataView(ObjProductLadger.GetProductLedgerByCompanyIdandBrandIdandProductID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value), "", "", DataViewRowState.CurrentRows).ToTable();
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
            dt = new DataView(ObjProductLadger.GetProductLedgerAllByProductID(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnProductId.Value), "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Ddlreftype.SelectedIndex > 0)
        {
            dt = new DataView(dt, "TransType='" + Ddlreftype.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (txtValue.Text != "")
        {
            if (ddlOption.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption.SelectedIndex == 1)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String)='" + txtValue.Text.Trim() + "'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    }
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                DataView view = new DataView(dt, condition, "", DataViewRowState.CurrentRows);
                dt = view.ToTable();
                btnRefresh.Focus();
            }
        }
        try
        {
            dt = new DataView(dt, "", "ModifiedDate desc", DataViewRowState.CurrentRows).ToTable().Rows.Cast<System.Data.DataRow>().Take(30).CopyToDataTable();
        }
        catch
        {
        }
        dt = new DataView(dt,"TransType='SI'", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)gvProductLadger, dt, "", "");
        Session["dtStockAnalysis"] = dt;
    }
    public string SetDecimal(string amount, string CurrencyID)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(CurrencyID, amount);
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void btnbind_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        FillGrid();
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        getProductDetail(hdnProductId.Value, "","");
    }
    protected void lnkRef_Click(object sender, EventArgs e)
    {

    }

    //need to add on page to run
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static bool IsObjectPermission(string ModuelId, string ObjectId)
    //{
    //    Common cmn = new Common();
    //    bool Result = false;

    //    if (HttpContext.Current.Session["EmpId"].ToString() == "0")
    //    {
    //        Result = true;
    //    }
    //    else
    //    {
    //        if (cmn.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), ModuelId, ObjectId).Rows.Count > 0)
    //        {
    //            Result = true;
    //        }
    //    }
    //    return Result;
    //}
}