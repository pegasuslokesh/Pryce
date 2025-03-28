using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using PegasusDataAccess;
public partial class Inventory_ProductBuilder : BasePage
{
    #region Defined Class Object
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    BillOfMaterial ObjInvBOM = null;
    Inv_ProductMaster ObjInvProductMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_OptionCategoryMaster ObjOpCate = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    DataAccessClass objda = null;
    ProductOptionCategoryDetail objProOpCatedetail = null;
    Inv_Model_Category objModelCategory = null;
    Set_Suppliers ObjSupplierMaster = null;
    Inv_Model_Suppliers objModelSupplier = null;
    LocationMaster objLocation = null;
    Inv_StockDetail objStockDetail = null;
    Inv_UnitMaster ObjInvUnitMaster = null;
    Inv_Model_Category_Product objInvModelCategoryProduct = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    string StrBrandId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrCompId = Session["CompId"].ToString();
        StrUserId = Session["UserId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";

        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        ObjInvProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjOpCate = new Inv_OptionCategoryMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objProOpCatedetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        objModelCategory = new Inv_Model_Category(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        objModelSupplier = new Inv_Model_Suppliers(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjInvUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objInvModelCategoryProduct = new Inv_Model_Category_Product(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductBuilder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["dtFilter_Pro_Build"] = null;
            Session["DtModel"] = null;
            Session["DtQuotationItemList"] = null;
            FillModelGrid();
            FillModelCategory();

        }
        //AllPageCode();
        Page.Title = ObjSysParam.GetSysTitle();
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();

    }
    #endregion
    private void FillModelCategory()
    {
        DataTable dsCategory = null;
        dsCategory = objModelCategory.GetAllRecord_for_ProductBuilder(Session["CompId"].ToString(), Session["BrandId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "CategoryId");
    }
    #region System Defined Function:-Event
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        resetCategory();
        DataTable dt = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString(), "True");
        if (dt.Rows.Count > 0)
        {
            hdnTrans_id.Value = e.CommandArgument.ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtProductPartNo.Text = dt.Rows[0]["Model_No"].ToString();
            lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString().ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
            txtPrice.Text = "0";
            txtModelNo_TextChanged(null, null);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        setCategory(e.CommandArgument.ToString());
    }


    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True");
        try
        {
            dtModel = new DataView(dtModel, "IsLabel='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        DataTable dtSearch = new DataTable();
        DataTable dt = new DataTable();
        string supplierName = string.Empty;
        string stCount = string.Empty;
        string strSupplierId = string.Empty;
        string strCount = string.Empty;
        int i = 0;
        int j = 0;
        supplierName = txtSupplierSearch.Text.Trim();
        if (ddlcategorysearch.SelectedIndex != 0)
        {
            dt = objModelCategory.GetModelByCategoryId(Session["Compid"].ToString(), Session["BrandId"].ToString(), ddlcategorysearch.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                dtSearch = dt;
            }
            else
            {
                dtSearch = null;
            }
        }
        if (txtSupplierSearch.Text != "")
        {
            if (dtSearch != null)
            {
                strSupplierId = (supplierName.ToString().Split('/'))[supplierName.ToString().Split('/').Length - 1];
                DataTable dtSupp = objModelSupplier.GetModelSuppliersBySupplierId(Session["Compid"].ToString(), Session["BrandId"].ToString(), strSupplierId);
                for (j = 0; j < dtSupp.Rows.Count; j++)
                {
                    if (stCount == "")
                    {
                        stCount = dtSupp.Rows[j]["ModelId"].ToString();
                    }
                    else
                    {
                        stCount = stCount + "," + dtSupp.Rows[j]["ModelId"].ToString();
                    }
                }
                if (stCount != "")
                {
                    if (dtSearch.Rows.Count > 0)
                    {
                        dtSupp = new DataView(dtSearch, "ModelId in (" + stCount + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                if (dtSupp.Rows.Count > 0)
                {
                    dtSearch = dtSupp;
                }
                else
                {
                    dtSearch = null;
                }
                dtSupp = null;
            }
        }
        if (dtSearch != null)
        {
            for (i = 0; i < dtSearch.Rows.Count; i++)
            {
                if (strCount == "")
                {
                    strCount = dtSearch.Rows[i]["ModelId"].ToString();
                }
                else
                {
                    strCount = strCount + "," + dtSearch.Rows[i]["ModelId"].ToString();
                }
            }
        }
        if (strCount != "")
        {
            dtModel = new DataView(dtModel, "Trans_Id in (" + strCount + ")", "Model_Name asc", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            dtModel = new DataView(dtModel, condition, "", DataViewRowState.CurrentRows).ToTable();
        }


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dtModel, "", "");
        objPageCmn.FillData((object)dtlistProduct, dtModel, "", "");
        lblTotalRecord.Text = Resources.Attendance.Total_Records + ": " + dtModel.Rows.Count.ToString() + "";
        Session["DtModel"] = dtModel;
        dtModel = null;
        dtSearch = null;
        dt = null;
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlcategorysearch.SelectedIndex = 0;
        txtSupplierSearch.Text = "";
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
        FillModelGrid();
        txtValue.Focus();
    }
    protected void GridProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMaster.PageIndex = e.NewPageIndex;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, (DataTable)Session["dtFilter_Pro_Build"], "", "");
        gvModelMaster.Focus();
        //AllPageCode();
    }
    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")

        {
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    setCategory(dt.Rows[0]["Trans_Id"].ToString());
                    FillRelatedProduct(dt.Rows[0]["Trans_Id"].ToString());
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
                    fillDataByModel(dt);
                    DataTable dtoptDetail = new DataTable();
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {
                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        Label lblCategoryId = (Label)OPRow.FindControl("lblOptionCategoryId");
                        dtoptDetail.Merge(objProOpCatedetail.GetDetailByOpCateId(StrCompId.ToString(), lblCategoryId.Text.Trim()));
                        if (ViewState["labelCateId"] == null)
                        {
                            if (dtoptDetail.Rows.Count != 0)
                            {
                                ViewState["labelCateId"] = lblCategoryId.Text.Trim();
                            }
                        }
                        foreach (ListItem li in RdoList.Items)
                        {
                            if (li.Selected)
                            {
                                rdoOption_SelectedIndexChanged(((object)RdoList), e);
                            }
                        }
                    }
                    if (!txtOptionPartNo.Text.Contains("#"))
                    {
                        if (dtoptDetail.Rows.Count != 0)
                        {
                            txtOptionPartNo.Text = txtOptionPartNo.Text + "#" + "0";
                        }
                    }
                }
                else
                {
                    txtModelNo.Text = "";
                    DisplayMessage("Select Model Name");
                    txtModelNo.Focus();
                }
            }
            catch
            {
            }
        }
    }
    protected void txtModelName_TextChanged(object sender, EventArgs e)
    {
        if (txtModelName.Text != "")
        {
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_Name='" + txtModelName.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
                    FillRelatedProduct(dt.Rows[0]["Trans_Id"].ToString());
                    fillDataByModel(dt);
                    DataTable dtoptDetail = new DataTable();
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {
                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        Label lblCategoryId = (Label)OPRow.FindControl("lblOptionCategoryId");
                        dtoptDetail.Merge(objProOpCatedetail.GetDetailByOpCateId(StrCompId.ToString(), lblCategoryId.Text.Trim()));
                        if (ViewState["labelCateId"] == null)
                        {
                            if (dtoptDetail.Rows.Count != 0)
                            {
                                ViewState["labelCateId"] = lblCategoryId.Text.Trim();
                            }
                        }
                        foreach (ListItem li in RdoList.Items)
                        {
                            if (li.Selected)
                            {
                                rdoOption_SelectedIndexChanged(((object)RdoList), e);
                            }
                        }
                    }
                    if (!txtOptionPartNo.Text.Contains("#"))
                    {
                        if (dtoptDetail.Rows.Count != 0)
                        {
                            txtOptionPartNo.Text = txtOptionPartNo.Text + "#" + "0";
                        }
                    }
                }
                else
                {
                    txtModelNo.Text = "";
                    DisplayMessage("Select Model Name");
                    txtModelNo.Focus();
                }
            }
            catch
            {
            }
        }
    }
    protected void gvOptionCategory_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow GvOptrow in gvOptionCategory.Rows)
        {
            try
            {
                string ModelId = ViewState["ModelId"].ToString();
                RadioButtonList RdoList = (RadioButtonList)GvOptrow.FindControl("rdoOption");
                Label lblOpCatiD = (Label)GvOptrow.FindControl("lblOptionCategoryId");
                DataTable dtcatedetail = objProOpCatedetail.GetDetailByOpCateId(StrCompId.ToString(), lblOpCatiD.Text.Trim());
                if (dtcatedetail.Rows.Count == 0)
                {
                    DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "OptionCategoryId='" + lblOpCatiD.Text.Trim() + "'", "TransId Asc", DataViewRowState.CurrentRows).ToTable();
                    int SerialNo = 0;
                    string StrValue = "";
                    try
                    {
                        SerialNo = Convert.ToInt32(dtOption.Rows[0]["Field1"].ToString()) - 1;
                        StrValue = txtOptionPartNo.Text[SerialNo].ToString();
                    }
                    catch
                    {
                        SerialNo = -1;
                    }
                    foreach (DataRow row in dtOption.Rows)
                    {
                        var txt = string.Empty;
                        string ProductName = string.Empty;
                        bool b = false;
                        if (row["SubProductId"].ToString() == "0")
                        {
                            txt = row["ShortDescription"].ToString();
                        }
                        else
                        {
                            ProductName = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(StrCompId.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId = '" + row["SubProductId"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["EProductName"].ToString();
                            txt = ProductName.ToString() + "," + row["ShortDescription"].ToString();
                        }
                        try
                        {
                            if (Convert.ToBoolean(row["PDefault"].ToString()))
                            {
                                b = true;
                            }
                            if (StrValue != "")
                            {
                                if (StrValue.ToString() == "0")
                                {
                                    b = false;
                                }
                                else
                                {
                                    if (StrValue.ToString() == row["OptionId"].ToString())
                                    {
                                        b = true;
                                    }
                                    else
                                    {
                                        b = false;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        var val = row["OptionId"].ToString();
                        var item = new ListItem(txt, val);
                        item.Selected = b;
                        RdoList.Items.Add(item);
                    }
                }
                else
                {
                    var txt = string.Empty;
                    var val = string.Empty;
                    string newStr = "";
                    if (txtOptionPartNo.Text != "")
                    {
                        newStr = txtOptionPartNo.Text.Substring(txtOptionPartNo.Text.LastIndexOf("#")).Replace("#", "");
                    }
                    foreach (DataRow row in dtcatedetail.Rows)
                    {
                        txt = row["Width"].ToString() + "MM" + "X" + row["Height"].ToString() + "MM";
                        val = row["Trans_Id"].ToString();
                        var item = new ListItem(txt, val);
                        if (txtOptionPartNo.Text.Contains("#"))
                        {
                            if (val.Trim() == newStr.Trim())
                            {
                                item.Selected = true;
                            }
                        }
                        RdoList.Items.Add(item);
                    }
                }
            }
            catch
            {
            }
        }
    }
    protected void txtProductPartNo_TextChanged(object sender, EventArgs e)
    {
        if (txtProductPartNo.Text != "")
        {
            bool b = true;
            string str = string.Empty;
            string ProductId = string.Empty;
            bool ISZero = true;
            bool IsAllow = true;
            for (int i = txtProductPartNo.Text.Length; i > 0; i--)
            {
                if (b)
                {
                    if (!txtProductPartNo.Text[i - 1].ToString().Contains("-"))
                    {
                        str = txtProductPartNo.Text[i - 1].ToString() + str;
                        if (txtProductPartNo.Text[i - 1].ToString() != "0")
                        {
                            if (ISZero)
                            {
                                ISZero = false;
                            }
                        }
                    }
                    else
                    {
                        b = false;
                    }
                }
                else
                {
                    ProductId = txtProductPartNo.Text[i - 1].ToString() + ProductId;
                }
            }
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_No='" + ProductId.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtOption = ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), dt.Rows[0]["Trans_Id"].ToString());
                if (!str.Contains("#"))
                {
                    dtOption.Columns["Field1"].DataType = typeof(int);
                    if (Convert.ToInt32(new DataView(dtOption, "", "Field1 Desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString()) != str.Trim().Length)
                    {
                        DisplayMessage("Invaild Part No ");
                        gvOptionCategory.DataSource = null;
                        gvOptionCategory.DataBind();
                        txtModelName.Text = "";
                        txtModelNo.Text = "";
                        txtDesc.Text = "";
                        txtPrice.Text = "0";
                        txtProductPartNo.Text = "";
                        txtOptionPartNo.Text = "";
                        txtNewPartNo.Text = "";
                        tblgrid.Visible = false;
                        return;
                    }
                }
                DataTable dtTemp = new DataView(dtOption, "PDefault='True'", "Field1 Asc", DataViewRowState.CurrentRows).ToTable();
                if (ISZero)
                {
                    if (dtTemp.Rows.Count > 0)
                    {
                        IsAllow = false;
                    }
                }
                else
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        string Str = str.Trim().ToString()[Convert.ToInt32(dtTemp.Rows[i]["Field1"].ToString()) - 1].ToString();
                        if (Str.Trim() == "0")
                        {
                            IsAllow = false;
                            break;
                        }
                        else
                        {
                            if (checkOptionId(dt.Rows[0]["Trans_Id"].ToString(), dtTemp.Rows[i]["Field1"].ToString(), Str.Trim()).Rows.Count == 0)
                            {
                                checkOptionId(dt.Rows[0]["Trans_Id"].ToString(), dtTemp.Rows[i]["Field1"].ToString(), Str.Trim());
                                //if (new DataView(dtOption, "optionId='" + Str.Trim() + "' and Field1='" + Convert.ToInt32(dtTemp.Rows[i]["Field1"].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                                //{
                                IsAllow = false;
                                break;
                            }
                        }
                    }
                }
                if (!IsAllow)
                {
                    DisplayMessage("Invaild Part No ");
                    gvOptionCategory.DataSource = null;
                    gvOptionCategory.DataBind();
                    txtModelName.Text = "";
                    txtModelNo.Text = "";
                    txtDesc.Text = "";
                    txtPrice.Text = "0";
                    txtProductPartNo.Text = "";
                    txtOptionPartNo.Text = "";
                    txtNewPartNo.Text = "";
                    tblgrid.Visible = false;
                    return;
                }
                txtProductPartNo.Text = "";
                txtOptionPartNo.Text = "";
                txtNewPartNo.Text = "";
                txtProductPartNo.Text = ProductId.Trim();
                txtOptionPartNo.Text = str.Trim();
                if (dt.Rows.Count != 0)
                {
                    txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
                    txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
                    txtModelNo_TextChanged(null, null);
                    //foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    //{
                    //    Label lblCategoryId = (Label)OPRow.FindControl("lblOptionCategoryId");
                    //    RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                    //    foreach (ListItem li in RdoList.Items)
                    //    {
                    //        if (li.Selected)
                    //        {
                    //            DataTable dtOptionTemp = new DataView(dtOption, "OptionCategoryId='" + lblCategoryId.Text.Trim() + "'and OptionId='" + li.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //            string UnitPrice = dtOptionTemp.Rows[0]["UnitPrice"].ToString();
                    //            txtPrice.Text = (float.Parse(txtPrice.Text) + (float.Parse(UnitPrice) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString()))).ToString();
                    //            txtDesc.Text += "," + dtOptionTemp.Rows[0]["ShortDescription"].ToString();
                    //        }
                    //    }
                    //}
                    //txtPrice.Text = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), txtPrice.Text.Trim());
                    tblgrid.Visible = true;
                }
            }
            catch
            {
                DisplayMessage("Invaild Part No ");
                gvOptionCategory.DataSource = null;
                gvOptionCategory.DataBind();
                txtModelName.Text = "";
                txtModelNo.Text = "";
                txtDesc.Text = "";
                txtPrice.Text = "0";
                txtProductPartNo.Text = "";
                txtOptionPartNo.Text = "";
                txtNewPartNo.Text = "";
                tblgrid.Visible = false;
                return;
            }
        }
    }
    public DataTable checkOptionId(string ModelId, string serialNo, string OptionId)
    {
        DataTable dt = new DataTable();
        string sql = string.Empty;
        sql = " select * from Inv_BOM where Company_id=" + Session["CompId"].ToString() + " and ModelId=" + ModelId + " and Field1='" + serialNo + "'  and OptionID like '%" + OptionId + "%' collate Latin1_General_CS_AS";
        return objda.return_DataTable(sql);
    }
    protected void gvModelMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Pro_Build"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");
        //AllPageCode();
        gvModelMaster.BottomPagerRow.Focus();
    }
    protected void gvModelMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pro_Build"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pro_Build"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");
        //AllPageCode();
        gvModelMaster.HeaderRow.Focus();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        tblgrid.Visible = false;
        txtModelName.Text = "";
        txtModelNo.Text = "";
        txtProductPartNo.Text = "";
        txtOptionPartNo.Text = "";
        txtNewPartNo.Text = "";
        gvOptionCategory.DataSource = null;
        gvOptionCategory.DataBind();
        txtModelNo.Focus();
        txtDesc.Text = "";
        txtPrice.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["DtQuotationItemList"] = null;
        objPageCmn.FillData((object)gvProduct, null, "", "");
        btnTranferInquotaion.Visible = false;
        btnExporttoExcel.Visible = false;
        gvRelatedProduct.DataSource = null;
        gvRelatedProduct.DataBind();
        Session["dtModelRelatedProduct"] = null;
        //gvPartNo.DataSource = null;
        //gvPartNo.DataBind();
        Div_Add_Product.Visible = false;
    }
    public void somethingToRunInThread()
    {
        System.Windows.Forms.Clipboard.SetText(txtProductPartNo.Text.Trim() + "-" + txtOptionPartNo.Text.Trim());
    }
    protected void copy_to_clipboard()
    {
        Thread clipboardThread = new Thread(somethingToRunInThread);
        clipboardThread.SetApartmentState(ApartmentState.STA);
        clipboardThread.IsBackground = false;
        clipboardThread.Start();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        tblgrid.Visible = false;
        FillModelGrid();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        btnReset_Click(null, null);
        txtValue.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        //gvPartNo.DataSource = null;
        //gvPartNo.DataBind();
        //AllPageCode();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnCopyPartNo_Click(object sender, EventArgs e)
    {
        copy_to_clipboard();
    }
    //void fillpart()
    //{
    //    string ModelId = ViewState["ModelId"].ToString();
    //    DataTable dt = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable();
    //    DataTable dt1 = new DataTable();
    //    DataTable dtCate = dt.DefaultView.ToTable(true, "OptionCategoryId");
    //    string DefaultVal = string.Empty;
    //    string PartNo = string.Empty;
    //    if (dtCate.Rows.Count != 0)
    //    {
    //        for (int i = 0; i < dtCate.Rows.Count; i++)
    //        {
    //            DataTable dttemp = new DataView(dt, "OptionCategoryId='" + dtCate.Rows[i]["OptionCategoryId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            if (dttemp.Rows.Count == 1)
    //            {
    //                DefaultVal += dttemp.Rows[0]["OptionId"].ToString();
    //            }
    //            else
    //            {
    //                for (int j = 0; j < dttemp.Rows.Count; j++)
    //                {
    //                    dt1.Merge(new DataView(dt, "OptionId='" + dttemp.Rows[j]["OptionId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
    //                }
    //            }
    //        }
    //        dt = null;
    //        dt = dt1.Copy();
    //        string Value = string.Empty;
    //        string Option = string.Empty;
    //        string s = string.Empty;
    //        DataTable dtPartNo = new DataTable();
    //        dtPartNo.Columns.Add("PartNo");
    //        dtPartNo.Rows.Add(DefaultVal);
    //        if (dt.Rows.Count != 0)
    //        {
    //            dtPartNo.Rows.Clear();
    //            DataTable dtTemp1 = dt.Clone();
    //            DataTable dtTemp2 = dt.Clone();
    //            bool b = true;
    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                if (b)
    //                {
    //                    try
    //                    {
    //                        dtTemp1.ImportRow(dr);
    //                        b = false;
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }
    //                else
    //                {
    //                    try
    //                    {
    //                        dtTemp2.ImportRow(dr);
    //                        b = true;
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }
    //            }
    //            string Cat = string.Empty;
    //            for (int i = 0; i < dtTemp1.Rows.Count; i++)
    //            {
    //                Cat = string.Empty;
    //                string PNo = string.Empty;
    //                for (int j = 0; j < dtTemp2.Rows.Count; j++)
    //                {
    //                    if (dtTemp1.Rows[i]["OptionCategoryId"].ToString() != dtTemp2.Rows[j]["OptionCategoryId"].ToString())
    //                    {
    //                        if (!Cat.Contains(dtTemp2.Rows[j]["OptionCategoryId"].ToString()))
    //                        {
    //                            PNo += dtTemp2.Rows[j]["OptionId"].ToString();
    //                            Cat += dtTemp2.Rows[j]["OptionCategoryId"].ToString();
    //                        }
    //                    }
    //                }
    //                dtPartNo.Rows.Add();
    //                dtPartNo.Rows[dtPartNo.Rows.Count - 1][0] = DefaultVal + dtTemp1.Rows[i]["OptionId"].ToString() + PNo.ToString();
    //            }
    //            for (int i = 0; i < dtTemp2.Rows.Count; i++)
    //            {
    //                string PNo = string.Empty;
    //                Cat = string.Empty;
    //                for (int j = 0; j < dtTemp1.Rows.Count; j++)
    //                {
    //                    if (dtTemp2.Rows[i]["OptionCategoryId"].ToString() != dtTemp1.Rows[j]["OptionCategoryId"].ToString())
    //                    {
    //                        if (!Cat.Contains(dtTemp1.Rows[j]["OptionCategoryId"].ToString()))
    //                        {
    //                            PNo += dtTemp1.Rows[j]["OptionId"].ToString();
    //                            Cat += dtTemp1.Rows[j]["OptionCategoryId"].ToString();
    //                        }
    //                    }
    //                }
    //                dtPartNo.Rows.Add();
    //                dtPartNo.Rows[dtPartNo.Rows.Count - 1][0] = DefaultVal + dtTemp2.Rows[i]["OptionId"].ToString() + PNo.ToString();
    //            }
    //        }
    //        gvPartNo.DataSource = dtPartNo;
    //        gvPartNo.DataBind();
    //    }
    //    else
    //    {
    //        gvPartNo.DataSource = null;
    //        gvPartNo.DataBind();
    //    }
    //}
    //protected void lblPartNo_Click(object sender, EventArgs e)
    //{
    //    LinkButton li = (LinkButton)sender;
    //    txtOptionPartNo.Text = li.Text.Trim();
    //    txtModelNo_TextChanged(null, null);
    //    txtOptionPartNo_TextChanged(null, null);
    //}
    #endregion
    #region AutoComplete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();
        try
        {
            dt = new DataView(dt, "IsLabel='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_no Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            //dt = dtTemp.Copy();
        }
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelName(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();
        try
        {
            dt = new DataView(dt, "IsLabel='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        dt = new DataView(dt, "Model_Name like '%" + prefixText.ToString() + "%'", "Model_Name asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            // dt = dtTemp.Copy();
        }
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_Name"].ToString();
        }
        return txt;
    }
    //Supplier :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        dtCon = null;
        return filterlist;
    }
    //Supplier :- End
    #endregion
    #region User Defined Function
    //Comment By Akshay on  25-02-2014 after Model Concept
    //public string GetItemType(string IT)
    //{
    //    string retval = string.Empty;
    //    if (IT == "A")
    //    {
    //        retval = "Assemble  (Search as A)";
    //    }
    //    if (IT == "K")
    //    {
    //        retval = "KIT  (Search as K)";
    //    }
    //    if (IT == "S")
    //    {
    //        retval = "Stockable (Search as S)";
    //    }
    //    if (IT == "NS")
    //    {
    //        retval = "Non-Stockable (Search as NS)";
    //    }
    //    return retval;
    //}
    private void FillModelGrid()
    {
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True");
        try
        {
            dtModel = new DataView(dtModel, "IsLabel='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        DataTable dtSearch = new DataTable();
        DataTable dt = new DataTable();
        string supplierName = string.Empty;
        string stCount = string.Empty;
        string strSupplierId = string.Empty;
        string strCount = string.Empty;
        int i = 0;
        int j = 0;
        supplierName = txtSupplierSearch.Text.Trim();
        if (ddlcategorysearch.SelectedIndex > 0)
        {
            dt = objModelCategory.GetModelByCategoryId(Session["Compid"].ToString(), Session["BrandId"].ToString(), ddlcategorysearch.SelectedValue.ToString());
            if (dt == null)
            {
                dtSearch = null;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    dtSearch = dt;
                }
                else
                {
                    dtSearch = null;
                }
            }
        }
        if (txtSupplierSearch.Text != "")
        {
            if (dtSearch != null)
            {
                strSupplierId = (supplierName.ToString().Split('/'))[supplierName.ToString().Split('/').Length - 1];
                DataTable dtSupp = objModelSupplier.GetModelSuppliersBySupplierId(Session["Compid"].ToString(), Session["BrandId"].ToString(), strSupplierId);
                for (j = 0; j < dtSupp.Rows.Count; j++)
                {
                    if (stCount == "")
                    {
                        stCount = dtSupp.Rows[j]["ModelId"].ToString();
                    }
                    else
                    {
                        stCount = stCount + "," + dtSupp.Rows[j]["ModelId"].ToString();
                    }
                }
                if (stCount != "")
                {
                    if (dtSearch.Rows.Count > 0)
                    {
                        dtSupp = new DataView(dtSearch, "ModelId in (" + stCount + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                if (dtSupp.Rows.Count > 0)
                {
                    dtSearch = dtSupp;
                }
                else
                {
                    dtSearch = null;
                }
                dtSupp = null;
            }
        }
        if (dtSearch != null)
        {
            for (i = 0; i < dtSearch.Rows.Count; i++)
            {
                if (strCount == "")
                {
                    strCount = dtSearch.Rows[i]["ModelId"].ToString();
                }
                else
                {
                    strCount = strCount + "," + dtSearch.Rows[i]["ModelId"].ToString();
                }
            }
        }
        if (strCount != "")
        {
            dtModel = new DataView(dtModel, "Trans_Id in (" + strCount + ")", "Model_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dtModel, "", "");
        objPageCmn.FillData((object)dtlistProduct, dtModel, "", "");
        Session["dtFilter_Pro_Build"] = dtModel;
        Session["DtModel"] = dtModel;
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtModel.Rows.Count;
        //AllPageCode();
        dtModel = null;
        dt = null;
        dtSearch = null;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        dtres = null;
        return ArebicMessage;
    }
    public void fillOptionCategorygrid()
    {
        try
        {
            string ModelId = ViewState["ModelId"].ToString();
            DataTable dt = ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString());
            dt = new DataView(dt, "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable(true, "OptionCategoryId");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvOptionCategory, dt, "", "");
            if (dt.Rows.Count == 0)
            {
                txtOptionPartNo.Text = "";
                txtNewPartNo.Text = "";
                txtDesc.Text = "";
            }
            else
            {
                if (txtOptionPartNo.Text == "")
                {
                    txtOptionPartNo.Text = "0".PadRight(dt.Rows.Count, '0');
                    txtNewPartNo.Text = txtProductPartNo.Text + "-" + txtOptionPartNo.Text;
                }
            }
            dt = null;
        }
        catch
        {
        }
    }
    public string GetOpCateName(string OpCatId)
    {
        string OpCateName = string.Empty;
        try
        {
            OpCateName = ObjOpCate.GetOptionCategoryTruebyId(StrCompId.ToString(), OpCatId.ToString()).Rows[0]["EName"].ToString();
        }
        catch
        {
        }
        return OpCateName;
    }
    //Updated
    protected void rdoOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_optionCategory.Attributes.Add("Class", "box box-primary");
        GridViewRow Row = (GridViewRow)((RadioButtonList)sender).Parent.Parent;
        Label lblCategoryId = (Label)Row.FindControl("lblOptionCategoryId");
        RadioButtonList RdoList = (RadioButtonList)Row.FindControl("rdoOption");
        string PartNo = string.Empty;
        string Desc = string.Empty;
        float Price = 0;
        string OldPartOption = string.Empty;
        string value = string.Empty;
        string ModelId = ViewState["ModelId"].ToString();
        string strsno = string.Empty;
        DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "OptionCategoryId='" + lblCategoryId.Text.Trim() + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();
        DataTable dtopCategoryDetail = objProOpCatedetail.GetDetailByOpCateId(StrCompId.ToString(), lblCategoryId.Text.Trim());
        for (int i = 0; i < RdoList.Items.Count; i++)
        {
            if (RdoList.Items[i].Selected)
            {
                if (dtopCategoryDetail.Rows.Count == 0)
                {
                    DataTable dtOptionTemp = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    string UnitPrice = dtOptionTemp.Rows[0]["UnitPrice"].ToString();
                    Price += float.Parse(UnitPrice) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                    Desc += "," + dtOptionTemp.Rows[0]["ShortDescription"].ToString();
                    strsno = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString();
                    try
                    {
                        OldPartOption = txtOptionPartNo.Text[Convert.ToInt32(strsno) - 1].ToString();
                    }
                    catch
                    {
                    }
                    try
                    {
                        txtOptionPartNo.Text = txtOptionPartNo.Text.Remove(Convert.ToInt32(strsno) - 1, 1);
                    }
                    catch
                    {
                    }
                    try
                    {
                        txtOptionPartNo.Text = txtOptionPartNo.Text.Insert(Convert.ToInt32(strsno) - 1, RdoList.Items[i].Value);
                    }
                    catch
                    {
                    }
                    try
                    {
                        dtOptionTemp = new DataView(dtOption, "OptionId='" + OldPartOption.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        txtDesc.Text = txtDesc.Text.Replace("," + dtOptionTemp.Rows[0]["ShortDescription"].ToString(), "");
                        if (!txtOptionPartNo.Text.Contains("#"))
                        {
                            txtPrice.Text = (float.Parse(txtPrice.Text.ToString()) - (float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString()))).ToString();
                        }
                    }
                    catch
                    {
                    }
                    dtOptionTemp = null;
                }
                else
                {
                    DataTable DTtEMP = new DataView(dtopCategoryDetail, "Trans_ID='" + RdoList.Items[i].Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Desc += "," + DTtEMP.Rows[0]["Height"].ToString() + "MM" + "X" + DTtEMP.Rows[0]["Width"].ToString() + "MM";
                    string newStr = string.Empty;
                    newStr = txtOptionPartNo.Text.Substring(txtOptionPartNo.Text.LastIndexOf("#")).Replace("#", "");
                    if (!txtOptionPartNo.Text.Contains("#"))
                    {
                        txtOptionPartNo.Text.Insert(txtOptionPartNo.Text.Length - 1, "#0");
                    }
                    try
                    {
                        OldPartOption = newStr.ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        txtOptionPartNo.Text = txtOptionPartNo.Text.Replace("#" + newStr, "#" + RdoList.Items[i].Value);
                    }
                    catch
                    {

                    }
                    try
                    {
                        DTtEMP = new DataView(dtopCategoryDetail, "Trans_Id='" + OldPartOption.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        txtDesc.Text = txtDesc.Text.Replace("," + DTtEMP.Rows[0]["Height"].ToString() + "MM" + "X" + DTtEMP.Rows[0]["Width"].ToString() + "MM", "");
                    }
                    catch
                    {
                    }
                    DTtEMP = null;
                }
            }
            if (txtPrice.Text == "")
            {
                txtPrice.Text = "0";
            }
        }
        if (!txtOptionPartNo.Text.Contains("#"))
        {
            txtPrice.Text = (float.Parse(txtPrice.Text.ToString()) + Price).ToString();
            txtPrice.Text = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), txtPrice.Text.Trim());
        }
        else
        {
            CalculatePriceforLable();
        }
        dtOption = null;
        dtopCategoryDetail = null;
        txtDesc.Text += Desc;
        getdescription(txtOptionPartNo.Text, ModelId);
    }
    public void getdescription(string PartNumber, string ModelId)
    {
        txtDesc.Text = "";
        DataTable dtModel = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), "True"), "Trans_Id='" + ModelId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtModel.Rows.Count > 0)
        {
            txtDesc.Text = dtModel.Rows[0]["Field3"].ToString();
            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString(), ModelId.ToString());
            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();
                if (Charvalue != "0")
                {
                    if (Charvalue == "#")
                    {
                        break;
                    }
                    if (txtDesc.Text == "")
                    {
                        txtDesc.Text = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();
                    }
                    else
                    {
                        txtDesc.Text = txtDesc.Text + "," + new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();
                    }
                }
            }
            dtBom = null;
        }
        dtModel = null;
        txtNewPartNo.Text = txtProductPartNo.Text + "-" + txtOptionPartNo.Text;
    }
    public void fillDataByModel(DataTable dt)
    {
        try
        {
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
            txtProductPartNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtPrice.Text = dt.Rows[0]["Field1"].ToString();
            txtDesc.Text = dt.Rows[0]["Field3"].ToString();
            txtOptionPartNo.Focus();
            ViewState["ModelId"] = dt.Rows[0]["Trans_Id"].ToString();
            hdnModelPrice.Value = dt.Rows[0]["Field1"].ToString();
            hdnModelImage.Value = dt.Rows[0]["Field2"].ToString();
            hdnModelDesc.Value = dt.Rows[0]["Field3"].ToString();
            hdnCurrencyId.Value = dt.Rows[0]["Field4"].ToString();
            fillOptionCategorygrid();
            fillpart();
            tblgrid.Visible = true;
        }
        catch
        {
        }
    }
    #endregion
    #region Report
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        InventoryDataSet rptdata = new InventoryDataSet();
        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_BOM_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), 0, Convert.ToInt32(e.CommandArgument.ToString()), 4);
        if (rptdata.sp_Inv_BOM_SelectRow.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/PriceListReport.aspx?ModelId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (txtModelNo.Text.Trim() == "" || txtModelName.Text == "")
        {
            DisplayMessage("Enter Model Number or Name");
            txtModelNo.Focus();
            return;
        }
        if (!txtOptionPartNo.Text.Contains("#"))
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Company_Id");
            dt.Columns.Add("Model_No");
            dt.Columns.Add("Model_Name");
            dt.Columns.Add("PartNo.");
            dt.Columns.Add("Description");
            dt.Columns.Add("ModelImage");
            dt.Columns.Add("CurrencyId");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("LineTotal");
            dt.Rows.Add(Session["CompId"].ToString(), txtModelNo.Text, txtModelName.Text, txtProductPartNo.Text + "-" + txtOptionPartNo.Text, hdnModelDesc.Value, hdnModelImage.Value, hdnCurrencyId.Value, ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, hdnModelPrice.Value), ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, "1"), ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, hdnModelPrice.Value));
            foreach (GridViewRow gvRow in gvOptionCategory.Rows)
            {
                Label lblOptionCategoryId = (Label)gvRow.FindControl("lblOptionCategoryId");
                RadioButtonList rdoOption = (RadioButtonList)gvRow.FindControl("rdoOption");
                RadioButtonList RdoList = (RadioButtonList)gvRow.FindControl("rdoOption");
                DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ViewState["ModelId"].ToString()), "OptionCategoryId='" + lblOptionCategoryId.Text.Trim() + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < RdoList.Items.Count; i++)
                {
                    if (RdoList.Items[i].Selected)
                    {
                        DataTable dtOptionTemp = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        double LineTotal = 0;
                        try
                        {
                            LineTotal = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                        }
                        catch
                        {
                        }
                        dt.Rows.Add(Session["CompId"].ToString(), txtModelNo.Text, txtModelName.Text, "", dtOptionTemp.Rows[0]["ShortDescription"].ToString(), hdnModelImage.Value, hdnCurrencyId.Value, ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, dtOptionTemp.Rows[0]["UnitPrice"].ToString()), ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, dtOptionTemp.Rows[0]["Quantity"].ToString()), ObjSysParam.GetCurencyConversionForInv(hdnCurrencyId.Value, LineTotal.ToString()));
                    }
                }
            }
            Session["DTPartNoReport"] = dt;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/UserPartNo_Report.aspx','window','width=1024');", true);
        }
    }
    protected void btnPriceList_Click(object sender, EventArgs e)
    {
        if (txtModelNo.Text.Trim() == "" || txtModelName.Text == "")
        {
            DisplayMessage("Enter Model Number or Name");
            txtModelNo.Focus();
            return;
        }
        InventoryDataSet rptdata = new InventoryDataSet();
        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_BOM_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), 0, Convert.ToInt32(ViewState["ModelId"].ToString()), 4);
        if (rptdata.sp_Inv_BOM_SelectRow.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/PriceListReport.aspx?ModelId=" + ViewState["ModelId"].ToString() + "','window','width=1024');", true);
    }
    #endregion
    public void CalculatePriceforLable()
    {
        try
        {
            DataTable dt = new DataTable();
            string ModelId = ViewState["ModelId"].ToString();
            if (ViewState["labelCateId"] != null)
            {
                dt = objProOpCatedetail.GetDetailByOpCateId(StrCompId.ToString(), ViewState["labelCateId"].ToString());
            }
            DataTable dtOption = ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString());
            float labelQty = 0;
            float ColorPrice = 0;
            float MaterialPrice = 0;
            float PaperSize = 0;
            string labelQtyCateId = dt.Rows[0]["QtyCategoryId"].ToString();
            string labelMeterialCateId = dt.Rows[0]["MaterialCategoryId"].ToString();
            string labelColorCateId = dt.Rows[0]["ColorCategoryId"].ToString();
            foreach (GridViewRow row in gvOptionCategory.Rows)
            {
                RadioButtonList RdoList = (RadioButtonList)row.FindControl("rdoOption");
                Label lblCategoryId = (Label)row.FindControl("lblOptionCategoryId");
                for (int i = 0; i < RdoList.Items.Count; i++)
                {
                    if (RdoList.Items[i].Selected)
                    {
                        if (ViewState["labelCateId"].ToString() == lblCategoryId.Text.Trim())
                        {
                            dt = new DataView(dt, "Trans_Id='" + RdoList.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                            PaperSize = float.Parse(dt.Rows[0]["MMSize"].ToString()) / float.Parse(dt.Rows[0]["DefaultValue"].ToString());
                        }
                        else
                        {
                            DataTable dtOptionTemp = new DataView(dtOption, "OptionCategoryId='" + lblCategoryId.Text.Trim() + "' and OptionId='" + RdoList.Items[i].Value + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();
                            if (dt.Rows.Count != 0)
                            {
                                if (lblCategoryId.Text.Trim() == labelMeterialCateId.Trim())
                                {
                                    MaterialPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString());
                                }
                                if (lblCategoryId.Text.Trim() == labelColorCateId.Trim())
                                {
                                    ColorPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                                }
                                if (lblCategoryId.Text.Trim() == labelQtyCateId.Trim())
                                {
                                    labelQty = float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                                }
                            }
                            dtOptionTemp = null;
                        }
                    }
                }
            }
            dt = null;
            dtOption = null;
            float Price = ((PaperSize * MaterialPrice) + ColorPrice) * labelQty;
            txtPrice.Text = Price.ToString();
        }
        catch
        {
            txtPrice.Text = "0";
        }
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, ObjSysParam.GetCurencyConversionForInv(CurrencyId, Amount), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }
        return Amountwithsymbol;
    }
    #region generatepartno
    void fillpart()
    {
        Session["PartList"] = null;
        string ModelId = ViewState["ModelId"].ToString();
        DataTable dt = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable();
        DataTable dtCate = dt.DefaultView.ToTable(true, "OptionCategoryId");
        ArrayList arr = new ArrayList();
        ArrayList ar1 = new ArrayList();
        DataTable dtPartNo = new DataTable();
        dtPartNo.Columns.Add("PartNo");
        dtPartNo.Columns.Add("Stock");
        foreach (DataRow dr in dtCate.Rows)
        {
            DataTable dttemp = new DataView(dt, "OptionCategoryId='" + dr["OptionCategoryId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["PartList"] == null)
            {
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    DataRow dr1 = dtPartNo.NewRow();
                    dr1[0] = dttemp.Rows[i]["OptionID"].ToString();
                    
                    
                    dtPartNo.Rows.Add(dr1);
                }
                Session["PartList"] = dtPartNo;
            }
            else
            {
                //save all arraylist dat in datatable
                dtPartNo = new DataTable();
                dtPartNo.Columns.Add("PartNo");
                dtPartNo.Columns.Add("Stock");
                DataTable dt1 = (DataTable)Session["PartList"];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    for (int j = 0; j < dttemp.Rows.Count; j++)
                    {
                        DataRow dr1 = dtPartNo.NewRow();
                        dr1[0] = dt1.Rows[i][0].ToString() + dttemp.Rows[j]["OptionID"].ToString();

                        dtPartNo.Rows.Add(dr1);
                    }
                }
                Session["PartList"] = dtPartNo;
                dt1 = null;
            }
            dttemp = null;
        }


        for (int i = 0; i < dtPartNo.Rows.Count; i++)
        {
            try
            {
                dtPartNo.Rows[i][1] = Common.GetAmountDecimal(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ObjInvProductMaster.GetProductIdbyProductCode(txtModelNo.Text.Trim() + "-" + dtPartNo.Rows[i][0].ToString(), HttpContext.Current.Session["BrandId"].ToString())).Rows[0]["Quantity"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            }
            catch
            {
                dtPartNo.Rows[i][1] = "0";
            }
        }



        gvPartNo.DataSource = (DataTable)Session["PartList"];
        gvPartNo.DataBind();
        dt = null;
        dtCate = null;
        dtPartNo = null;
    }
    protected void lblPartNo_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div_SuggestedPartNo.Attributes.Add("Class", "box box-primary");
        LinkButton li = (LinkButton)sender;
        txtOptionPartNo.Text = li.Text.Trim();
        txtModelNo_TextChanged(null, null);
        //txtOptionPartNo_TextChanged(null, null);
    }
    #endregion
    #region FilterCategory
    protected void imbBtnGrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dtlistProduct.Visible = false;
        gvModelMaster.Visible = true;
        imgBtnDatalist.Visible = true;
        imbBtnGrid.Visible = false;
        txtValue.Focus();
        //AllPageCode();
    }
    protected void imgBtnDatalist_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dtlistProduct.Visible = true;
        gvModelMaster.Visible = false;
        imgBtnDatalist.Visible = false;
        imbBtnGrid.Visible = true;
        txtValue.Focus();
        //AllPageCode();
    }

    protected void txtSupplierSearch_OnTextChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string SupplierName = string.Empty;
        SupplierName = txtSupplierSearch.Text.Trim();
        if (SupplierName.ToString() != "")
        {
            try
            {
                string strSupplierId = "";
                strSupplierId = (SupplierName.ToString().Split('/'))[SupplierName.ToString().Split('/').Length - 1];
                string query = "Supplier_Id = '" + strSupplierId + "'";
                string name = ObjSupplierMaster.GetSupplierNameById(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), strSupplierId);
                if (name == "")
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSupplierSearch.Text = "";
                    txtSupplierSearch.Focus();
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
                }
            }
            catch
            {
                DisplayMessage("Invalid Supplier Name");
                txtSupplierSearch.Text = "";
                txtSupplierSearch.Focus();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
            }
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierSearch.Focus();
            txtSupplierSearch.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
        }
    }
    #endregion
    //add product list for sales quotation 
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtProduct = (DataTable)Session["DtQuotationItemList"];
        try
        {
            dtProduct = new DataView(dtProduct, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        Session["DtQuotationItemList"] = dtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dtProduct, "", "");
        dtProduct = null;
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        if (txtOptionPartNo.Text == "")
        {
            DisplayMessage("Part number not found");
            return;
        }
        if (Request.QueryString["Type"] != null)
        {
            btnTranferInquotaion.Visible = true;
        }
        btnExporttoExcel.Visible = true;
        Div_Add_Product.Visible = true;
        DataTable dt = getProductTable();

        dt.Columns.Add("Field1");

        if (Session["DtQuotationItemList"] == null)
        {
            DataRow Dr = dt.NewRow();
            Dr["Trans_Id"] = 1;
            DataTable dtProduct = ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            try
            {
                dtProduct = new DataView(dtProduct, "ProductCode='" + txtProductPartNo.Text + "-" + txtOptionPartNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtProduct.Rows.Count > 0)
            {
                Dr["ProductId"] = dtProduct.Rows[0]["ProductId"].ToString();
                Dr["UnitId"] = dtProduct.Rows[0]["UnitId"].ToString();
                Dr["UnitName"] = GetUnitName(dtProduct.Rows[0]["UnitId"].ToString());
            }
            else
            {
                Dr["ProductId"] = "0";
                Dr["UnitId"] = 1;
                Dr["UnitName"] = "Pcs";
            }
            Dr["ProductCode"] = txtProductPartNo.Text + "-" + txtOptionPartNo.Text;
            Dr["ProductDescription"] = txtDesc.Text;
            Dr["SuggestedProductName"] = txtDesc.Text;
            Dr["Quantity"] = 1;
            if (txtPrice.Text == "")
            {
                txtPrice.Text = "0";
            }
            Dr["EstimatedUnitPrice"] = txtPrice.Text;
            Dr["Type"] = "";
            dt.Rows.Add(Dr);
            dtProduct = null;
        }
        else
        {
            dt = (DataTable)Session["DtQuotationItemList"];
            if (new DataView(dt, "ProductCode='" + txtProductPartNo.Text + "-" + txtOptionPartNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("Product Already Exists");
                return;
            }
            DataRow Dr = dt.NewRow();
            DataTable dtTemp = dt.Copy();
            Dr["Trans_Id"] = (float.Parse(new DataView(dtTemp, "", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable().Rows.Count.ToString()) + 1).ToString();
            DataTable dtProduct = ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            try
            {
                dtProduct = new DataView(dtProduct, "ProductCode='" + txtProductPartNo.Text + "-" + txtOptionPartNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtProduct.Rows.Count > 0)
            {
                Dr["ProductId"] = dtProduct.Rows[0]["ProductId"].ToString();
                Dr["UnitId"] = dtProduct.Rows[0]["UnitId"].ToString();
                Dr["UnitName"] = GetUnitName(dtProduct.Rows[0]["UnitId"].ToString());
            }
            else
            {
                Dr["ProductId"] = "0";
                Dr["UnitId"] = 1;
                Dr["UnitName"] = "Pcs";
            }
            Dr["ProductCode"] = txtProductPartNo.Text + "-" + txtOptionPartNo.Text;
            Dr["ProductDescription"] = txtDesc.Text;
            Dr["SuggestedProductName"] = txtDesc.Text;
            Dr["Quantity"] = 1;
            if (txtPrice.Text == "")
            {
                txtPrice.Text = "0";
            }
            Dr["EstimatedUnitPrice"] = txtPrice.Text;
            Dr["Type"] = "";
            dt.Rows.Add(Dr);
            dtTemp = null;
        }
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        Session["DtQuotationItemList"] = dt;
        dt = null;
    }
    public string getProductImage(string productId)
    {
        string image = "";
        image = objda.get_SingleValue("select top 1 Field1 from inv_product_image where ProductId=" + productId + "");
        image = image == "@NOTFOUND@" ? "" : "~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + image;
        return image;
    }
    public string GetUnitName(string UnitId)
    {
        DataTable dt = new DataView(ObjInvUnitMaster.GetUnitMasterAll(StrCompId.ToString()), "Unit_Id='" + UnitId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string strUnitName = "NA";
        if (dt.Rows.Count != 0)
        {
            strUnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        dt = null;
        return strUnitName;
    }
    public DataTable CreateProductDataTableForSalesQuotation()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TaxPercent");
        dt.Columns.Add("TaxValue");
        dt.Columns.Add("PriceAfterTax");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("DiscountValue");
        dt.Columns.Add("PriceAfterDiscount");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Quantity");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("PurchaseProductDescription");
        dt.Columns.Add("PurchaseProductPrice");
        dt.Columns.Add("SalesPrice");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("AgentCommission");
        return dt;
    }
    public DataTable AddRecordForSalesQuotation()
    {
        DataTable DtProduct = CreateProductDataTableForSalesQuotation();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["TaxPercent"] = "0";
            dr["TaxValue"] = "0";
            dr["PriceAfterTax"] = "0";
            dr["DiscountPercent"] = "0";
            dr["DiscountValue"] = "0";
            dr["PriceAfterDiscount"] = "0";
            dr["Quantity"] = gvQuantity.Text;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            try
            {
                dr["Currency_Id"] = Request.QueryString["CurId"].ToString();
            }
            catch
            {
                dr["Currency_Id"] = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            }
            dr["EstimatedUnitPrice"] = "0";
            dr["PurchaseProductDescription"] = "";
            dr["PurchaseProductPrice"] = "";
            dr["AgentCommission"] = "0";
            double unitPrice = 0;
            try
            {
                unitPrice = Convert.ToDouble(ObjInvProductMaster.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "C", Request.QueryString["CustomerId"].ToString(), gvProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString());
            }
            catch
            {
            }
            try
            {
                dr["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), unitPrice.ToString());
            }
            catch
            {
                dr["SalesPrice"] = "0";
            }
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            DtProduct.Rows.Add(dr);
            DtMaxSerial = null;
        }
        return DtProduct;
    }
    protected void btnTranferInquotaion_Click(object sender, EventArgs e)
    {
        if (gvProduct.Rows.Count == 0)
        {
            DisplayMessage("Product not found");
            return;
        }
        else
        {
            if (Request.QueryString["Type"].ToString().Trim() == "SINQ")
            {
                Session["DtSearchProduct"] = AddRecordForSalesInquiry();
            }
            if (Request.QueryString["Type"].ToString().Trim() == "SQ")
            {
                Session["DtSearchProduct"] = AddRecordForSalesQuotation();
            }
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
    }
    protected void btnExporttoExcel_Click(object sender, EventArgs e)
    {
        if (gvProduct.Rows.Count == 0)
        {
            DisplayMessage("Product not found");
            return;
        }
        else
        {
            DataTable dt = (DataTable)Session["DtQuotationItemList"];
            dt = dt.DefaultView.ToTable(true, "ProductCode", "ProductDescription", "UnitName", "Quantity", "EstimatedUnitPrice");
            ExportTableData(dt);
            dt = null;
        }
    }
    public DataTable CreateProductDataTableForSalesInquiry()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("SuggestedProductName");
        return dt;
    }
    public DataTable AddRecordForSalesInquiry()
    {
        DataTable DtProduct = CreateProductDataTableForSalesInquiry();
        if (Session["DtSearchProduct"] != null)
        {
            DtProduct = (DataTable)Session["DtSearchProduct"];
        }
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            HiddenField gvProductId = (HiddenField)gvRow.FindControl("hdnProductid");
            Label gvProductCode = (Label)gvRow.FindControl("lblProductCode");
            Label gvQuantity = (Label)gvRow.FindControl("lblQuantity");
            HiddenField hdngvunitId = (HiddenField)gvRow.FindControl("hdnunitId");
            Label lblgvDesc = (Label)gvRow.FindControl("lblDesc");
            Label lblgvproductname = (Label)gvRow.FindControl("lblProductName");
            DataTable DtMaxSerial = new DataTable();
            try
            {
                DtMaxSerial.Merge(DtProduct);
                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            DataRow dr = DtProduct.NewRow();
            try
            {
                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
            }
            catch
            {
                dr["Serial_No"] = "1";
            }
            dr["Quantity"] = gvQuantity.Text;
            dr["ProductDescription"] = lblgvDesc.Text;
            dr["SuggestedProductName"] = lblgvproductname.Text;
            dr["Product_Id"] = gvProductId.Value;
            dr["UnitId"] = hdngvunitId.Value;
            try
            {
                dr["Currency_Id"] = Request.QueryString["CurId"].ToString();
            }
            catch
            {
                dr["Currency_Id"] = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            }
            dr["EstimatedUnitPrice"] = "0";
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            DtProduct.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = DtProduct;
        return DtProduct;
    }
    public void ExportTableData(DataTable dtdata)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ProductList.xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtdata.Copy();
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
        dt = null;
    }
    protected void chkRelatedProduct_OnCheckedChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] != null)
        {
            btnTranferInquotaion.Visible = true;
        }
        btnExporttoExcel.Visible = true;
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        string ProductId = ((Label)gvrow.FindControl("lblgvProductid")).Text;
        DataTable dt = getProductTable();

        if (Session["DtQuotationItemList"] == null)
        {
            DataRow Dr = dt.NewRow();
            Dr["Trans_Id"] = 1;
            DataTable dtProduct = ObjInvProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtProduct.Rows.Count > 0)
            {
                Dr["ProductId"] = dtProduct.Rows[0]["ProductId"].ToString();
                Dr["UnitId"] = dtProduct.Rows[0]["UnitId"].ToString();
                Dr["UnitName"] = GetUnitName(dtProduct.Rows[0]["UnitId"].ToString());
            }
            else
            {
                Dr["ProductId"] = "0";
                Dr["UnitId"] = 1;
                Dr["UnitName"] = "Pcs";
            }
            Dr["ProductCode"] = dtProduct.Rows[0]["ProductCode"].ToString();
            Dr["ProductDescription"] = dtProduct.Rows[0]["EProductName"].ToString();
            Dr["SuggestedProductName"] = dtProduct.Rows[0]["EProductName"].ToString();
            Dr["Quantity"] = 1;
            Dr["EstimatedUnitPrice"] = GetSalesPriceinLocal(dtProduct.Rows[0]["SalesPrice1"].ToString());
            Dr["Type"] = "RL";
            dt.Rows.Add(Dr);
            dtProduct = null;
        }
        else
        {
            dt = (DataTable)Session["DtQuotationItemList"];
            if (new DataView(dt, "ProductCode='" + ((Label)gvrow.FindControl("lblgvProductCode")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("Product Already Exists");
                ((CheckBox)gvrow.FindControl("chkRelatedProduct")).Checked = false;
                return;
            }
            DataRow Dr = dt.NewRow();
            DataTable dtTemp = dt.Copy();
            Dr["Trans_Id"] = (float.Parse(new DataView(dtTemp, "", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable().Rows.Count.ToString()) + 1).ToString();
            DataTable dtProduct = ObjInvProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtProduct.Rows.Count > 0)
            {
                Dr["ProductId"] = dtProduct.Rows[0]["ProductId"].ToString();
                Dr["UnitId"] = dtProduct.Rows[0]["UnitId"].ToString();
                Dr["UnitName"] = GetUnitName(dtProduct.Rows[0]["UnitId"].ToString());
            }
            else
            {
                Dr["ProductId"] = "0";
                Dr["UnitId"] = 1;
                Dr["UnitName"] = "Pcs";
            }
            Dr["ProductCode"] = dtProduct.Rows[0]["ProductCode"].ToString();
            Dr["ProductDescription"] = dtProduct.Rows[0]["EProductName"].ToString();
            Dr["SuggestedProductName"] = dtProduct.Rows[0]["EProductName"].ToString();
            Dr["Quantity"] = 1;
            Dr["EstimatedUnitPrice"] = GetSalesPriceinLocal(dtProduct.Rows[0]["SalesPrice1"].ToString());
            Dr["Type"] = "RL";
            dt.Rows.Add(Dr);
            dtTemp = null;
            dtProduct = null;
        }
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        Session["DtQuotationItemList"] = dt;
        DataTable dtRelatedProduct = new DataView((DataTable)Session["dtModelRelatedProduct"], "ProductId<>" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable();
        gvRelatedProduct.DataSource = dtRelatedProduct;
        gvRelatedProduct.DataBind();
        Session["dtModelRelatedProduct"] = dtRelatedProduct;
        dtRelatedProduct = null;
        dt = null;
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
            SalesPrice = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (Convert.ToDouble(Amount) * Convert.ToDouble(Exchangerate)).ToString());
        }
        catch
        {
            SalesPrice = "0";
        }
        return SalesPrice;
    }
    public void FillRelatedProduct(string strModelId)
    {
        DataTable dtProductCat = objInvModelCategoryProduct.SelectModelCategoryProductRow(strModelId);
        dtProductCat = dtProductCat.DefaultView.ToTable(true, "CategoryId", "categoryName", "ProductId", "ProductCode", "ProductName");
        gvRelatedProduct.DataSource = dtProductCat;
        gvRelatedProduct.DataBind();
        Session["dtModelRelatedProduct"] = dtProductCat;
        dtProductCat = null;
    }
    protected void btnSaveItemToProduct_Click(object sender, EventArgs e)
    {
        string strCurrencyId = Session["LocCurrencyId"].ToString();
        string countryId = new Country_Currency(Session["DBConnection"].ToString()).GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
        string stockable = "S";
        string count = "";
        stockable = objda.get_SingleValue("select top 1 ItemType from inv_productMaster where modelNo='" + hdnTrans_id.Value + "'");
        stockable = stockable == "@NOTFOUND@" ? "S" : stockable;
        int index = 0;

        for (int i = 0; i < gvProduct.Rows.Count; i++)
        {
            Label producName = gvProduct.Rows[i].FindControl("lblProductName") as Label;
            Label productCode = gvProduct.Rows[i].FindControl("lblProductCode") as Label;
            count = objda.get_SingleValue("select count(*) from inv_productMaster where productCode='" + productCode.Text + "'");
            count = count == "@NOTFOUNT@" ? "" : count;
            if (count != "" && count != "0")
            {
                continue;
            }
            index++;
            Label productDescription = gvProduct.Rows[i].FindControl("ProductDescription") as Label;
            HiddenField hdnUnit = gvProduct.Rows[i].FindControl("hdnunitId") as HiddenField;
            Label unitPrice = gvProduct.Rows[i].FindControl("lblUnitPrice") as Label;
            int productid = ObjInvProductMaster.InsertProductMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), productCode.Text, "false", hdnTrans_id.Value, "0", producName.Text, "", countryId, hdnUnit.Value, stockable, "", "false", "Internally", "false", "1", "", txtDesc.Text, unitPrice.Text, "0", "0", "", "", "ReseverQty", "", "", "", "", "0", "", "0", "0", "", "", "0", "0", "0", "", "", "", "", "", "true", "0", "0", "true", System.DateTime.Now.ToString(), "true", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0", strCurrencyId,"","","");
            ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString(), productid.ToString(), Session["BrandId"].ToString(), "", "", "", "", "", "true", System.DateTime.Now.ToString(), "true", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
        }
        if (index > 0)
        {
            DisplayMessage("Items Added Successfully");
        }
        else
        {
            DisplayMessage("Items Saved Already");
        }
    }
    protected void ddlModelCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModelCategory.SelectedValue != "0")
        {
            using (DataTable dtProductData = ObjInvProductMaster.GetProductDataByCategoryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ddlModelCategory.SelectedValue.ToString(), hdnTrans_id.Value))
            {
                gvCategoryProduct.DataSource = dtProductData;
                gvCategoryProduct.DataBind();
            }
        }
    }
    protected void btnAddToItemList_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Session["DtQuotationItemList"] != null)
        {
            dt = Session["DtQuotationItemList"] as DataTable;
        }
        else
        {
            dt = getProductTable();
        }
        int count = 0;
        for (int i = 0; i < gvCategoryProduct.Rows.Count; i++)
        {
            CheckBox chk = gvCategoryProduct.Rows[i].FindControl("chkProductSelect") as CheckBox;
            if (chk.Checked)
            {
                count++;
                HiddenField ProductId = gvCategoryProduct.Rows[i].FindControl("gvhdnProductId") as HiddenField;
                HiddenField UnitId = gvCategoryProduct.Rows[i].FindControl("gvhdnUnitId") as HiddenField;
                Label ProductCode = gvCategoryProduct.Rows[i].FindControl("gvlblProductCode") as Label;
                Label ProductDescription = gvCategoryProduct.Rows[i].FindControl("gvlblProductDescription") as Label;
                Label SuggestedProductName = gvCategoryProduct.Rows[i].FindControl("gvlblProductName") as Label;
                Label Quantity = gvCategoryProduct.Rows[i].FindControl("gvlblStock") as Label;
                Label UnitName = gvCategoryProduct.Rows[i].FindControl("gvlblUnitName") as Label;
                Label EstimatedUnitPrice = gvCategoryProduct.Rows[i].FindControl("gvlblProductPrice") as Label;
                HiddenField Type = gvCategoryProduct.Rows[i].FindControl("gvhdnItemType") as HiddenField;
                dt.Rows.Add((dt.Rows.Count + 1).ToString(), ProductId.Value, UnitId.Value, ProductCode.Text, ProductDescription.Text, SuggestedProductName.Text, Quantity.Text, UnitName.Text, EstimatedUnitPrice.Text, Type.Value);
            }
        }
        gvProduct.DataSource = dt;
        gvProduct.DataBind();
        Session["DtQuotationItemList"] = dt;
        btnExporttoExcel.Visible = true;
        Div_Add_Product.Visible = true;
        if (count > 0)
        {
            DisplayMessage("Items added successfully");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        dt = null;
    }
    public void resetCategory()
    {
        gvCategoryProduct.DataSource = null;
        gvCategoryProduct.DataBind();
        ddlModelCategory.Items.Clear();
        ddlModelCategory.DataSource = null;
        ddlModelCategory.DataBind();
    }

    public void setCategory(string modelId)
    {
        using (DataTable dtCategoryData = ObjProductCateMaster.GetProductCategoryByModelId(modelId))
        {
            ddlModelCategory.Items.Clear();
            if (dtCategoryData.Rows.Count > 0)
            {
                ddlModelCategory.DataSource = dtCategoryData;
                ddlModelCategory.DataTextField = "Category_Name";
                ddlModelCategory.DataValueField = "Category_Id";
                ddlModelCategory.DataBind();
                ddlModelCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlModelCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    public DataTable getProductTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("ProductId");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductCode");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("UnitName");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("Type");
        return dt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }


    protected void txtNewPartNo_TextChanged(object sender, EventArgs e)
    {
        if (txtNewPartNo.Text.Trim() != "")
        {
            DataTable dt = objda.return_DataTable("select distinct inv_modelmaster.model_no ,inv_modelmaster.model_name from inv_productmaster inner join inv_modelmaster on inv_modelmaster.trans_id = inv_productmaster.modelno where inv_productmaster.ProductCode = '" + txtNewPartNo.Text + "'");

            if (dt.Rows.Count > 0)
            {
                txtModelNo.Text = dt.Rows[0]["model_no"].ToString();
                txtModelName.Text = dt.Rows[0]["model_name"].ToString();
                txtModelNo_TextChanged(null, null);
            }
            else
            {
                DisplayMessage("No module on this product found");
            }

        }

    }
}