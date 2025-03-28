using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Inventory_LabelBuilder : BasePage
{
    #region Defined Class Object

    BillOfMaterial ObjInvBOM = null;
    Inv_ProductMaster ObjInvProductMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_OptionCategoryMaster ObjOpCate = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    DataAccessClass objda = null;
    ProductOptionCategoryDetail objProOpCatedetail = null;
    Inv_Model_Category objModelCategory = null;
    LocationMaster objLocation = null;
    Inv_ParameterMaster objInvParm = null;
    Inv_StockDetail objStock = null;
    Set_Suppliers ObjSupplierMaster = null;
    Inv_Model_Suppliers objModelSupplier = null;
    Inv_UnitMaster ObjInvUnitMaster = null;
    Inv_Model_Category_Product objInvModelCategoryProduct = null;
    CurrencyMaster ObjCurrencyMaster = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";

        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        ObjInvProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjOpCate = new Inv_OptionCategoryMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objProOpCatedetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        objModelCategory = new Inv_Model_Category(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objInvParm = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objStock = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        objModelSupplier = new Inv_Model_Suppliers(Session["DBConnection"].ToString());
        ObjInvUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objInvModelCategoryProduct = new Inv_Model_Category_Product(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["IsShowCostPrice"] = false;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/Labelbuilder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["dtFilter_L_B"] = null;
            Session["DtModel"] = null;
            FillModelGrid();
            Session["LabelCoreCategoryId"] = null;
            Session["LabelQtyCategoryId"] = null;
            Session["LabelPackingCategoryId"] = null;
            Session["PartList"] = null;

            FillModelCategory();
            FillCategory();
            FillCurrency();
            Session["DtQuotationItemList"] = null;
            if (Request.QueryString["SOP"] != null)
            {
                btnClose.Visible = true;
            }
            FillMaterial();
        }
        //AllPageCode();
        Page.Title = ObjSysParam.GetSysTitle();

    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        Session["IsShowCostPrice"] = clsPagePermission.bShowCostPrice;
    }
    #endregion
    private void FillModelCategory()
    {
        DataTable dsCategory = null;
        dsCategory = objModelCategory.GetAllRecord_for_LabelBuilder(Session["CompId"].ToString(), Session["BrandId"].ToString());

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "CategoryId");
    }
    #region System Defined Function:-Event
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        DataTable dt = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString(), "True");


        if (dt.Rows.Count > 0)
        {

            txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtProductPartNo.Text = dt.Rows[0]["Model_No"].ToString();
            lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
            txtPrice.Text = "0";
            Session["LabelCoreCategoryId"] = null;
            Session["LabelQtyCategoryId"] = null;
            Session["LabelPackingCategoryId"] = null;
            txtModelNo_TextChanged(null, null);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True");
        try
        {
            dtModel = new DataView(dtModel, "IsLabel='True'", "", DataViewRowState.CurrentRows).ToTable();
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
            DataView view = new DataView(dtModel, condition, "", DataViewRowState.CurrentRows);
        }

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dtModel, "", "");
        objPageCmn.FillData((object)dtlistProduct, dtModel, "", "");
        lblTotalRecord.Text = Resources.Attendance.Total_Records + ": " + dtModel.Rows.Count.ToString() + "";
        Session["Model"] = dtModel;
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
        ddlcategorysearch.SelectedIndex = 0;
        txtSupplierSearch.Text = "";
        FillModelGrid();
        txtValue.Focus(); ;
    }

    protected void GridProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMaster.PageIndex = e.NewPageIndex;

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, (DataTable)Session["dtFilter_L_B"], "", "");

        gvModelMaster.Focus();
        //AllPageCode();

    }

    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {
            Session["LabelCoreCategoryId"] = null;
            Session["LabelQtyCategoryId"] = null;
            Session["LabelPackingCategoryId"] = null;
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
                    fillDataByModel(dt);
                    DataTable dtoptDetail = new DataTable();
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {

                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        Label lblCategoryId = (Label)OPRow.FindControl("lblOptionCategoryId");
                        dtoptDetail.Merge(objProOpCatedetail.GetDetailByOpCateId(Session["CompId"].ToString().ToString(), lblCategoryId.Text.Trim()));
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
                            txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                            lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);

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
            Session["LabelCoreCategoryId"] = null;
            Session["LabelQtyCategoryId"] = null;
            Session["LabelPackingCategoryId"] = null;
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True"), "Model_Name='" + txtModelName.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());



                    fillDataByModel(dt);
                    DataTable dtoptDetail = new DataTable();
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {

                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        Label lblCategoryId = (Label)OPRow.FindControl("lblOptionCategoryId");
                        dtoptDetail.Merge(objProOpCatedetail.GetDetailByOpCateId(Session["CompId"].ToString().ToString(), lblCategoryId.Text.Trim()));
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
                            txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                            lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
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

                DataTable dtcatedetail = objProOpCatedetail.GetDetailByOpCateId(Session["CompId"].ToString().ToString(), lblOpCatiD.Text.Trim());
                if (dtcatedetail.Rows.Count == 0)
                {
                    DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString()), "OptionCategoryId='" + lblOpCatiD.Text.Trim() + "'", "TransId Asc", DataViewRowState.CurrentRows).ToTable();


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
                            ProductName = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId = '" + row["SubProductId"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["EProductName"].ToString();
                            txt = row["ShortDescription"].ToString();
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

                    string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
                    DataTable ModelLabel = objda.return_DataTable(sql);

                    foreach (DataRow row in ModelLabel.Rows)
                    {
                        if (row["Field1"].ToString().Trim() != "")
                        {
                            txt = row["Field3"].ToString().Trim() + row["Width"].ToString() + "mm" + "X" + row["Height"].ToString() + "mm - " + row["Field1"].ToString().Trim();
                        }
                        else
                        {
                            txt = row["Field3"].ToString().Trim() + row["Width"].ToString() + "mm" + "X" + row["Height"].ToString() + "mm";
                        }




                        val = row["Field2"].ToString();
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
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + ProductId.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtOption = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), dt.Rows[0]["Trans_Id"].ToString());
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
                        txtFinalPartNo.Text = "";
                        lblstockinfo.Text = "0.000";
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
                    txtFinalPartNo.Text = "";
                    lblstockinfo.Text = "0.000";
                    tblgrid.Visible = false;
                    return;
                }
                txtProductPartNo.Text = "";
                txtOptionPartNo.Text = "";
                txtFinalPartNo.Text = "";
                txtProductPartNo.Text = ProductId.Trim();
                txtOptionPartNo.Text = str.Trim();
                txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                lblstockinfo.Text = "0.000";
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
                txtFinalPartNo.Text = "";
                lblstockinfo.Text = "0.000";
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

        DataTable dt = (DataTable)Session["dtFilter_L_B"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");


        //AllPageCode();
        gvModelMaster.BottomPagerRow.Focus();

    }
    protected void gvModelMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_L_B"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_L_B"] = dt;
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
        txtFinalPartNo.Text = "";
        lblstockinfo.Text = "0.000";
        gvOptionCategory.DataSource = null;
        gvOptionCategory.DataBind();
        txtModelNo.Focus();
        txtDesc.Text = "";
        txtPrice.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["DtQuotationItemList"] = null;
        Session["LabelCoreCategoryId"] = null;
        Session["LabelQtyCategoryId"] = null;
        Session["LabelPackingCategoryId"] = null;
        gvRelatedProduct.DataSource = null;
        gvRelatedProduct.DataBind();
        Session["dtModelRelatedProduct"] = null;

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
        txtValue.Focus();
        //AllPageCode();
        btnReset_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnCopyPartNo_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "Copy1();", true);  
        copy_to_clipboard();
    }
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
            dt = new DataView(dt, "IsLabel='True'", "", DataViewRowState.CurrentRows).ToTable();
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
            dt = new DataView(dt, "IsLabel='True'", "", DataViewRowState.CurrentRows).ToTable();
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
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True");
        try
        {
            dtModel = new DataView(dtModel, "IsLabel='True'", "", DataViewRowState.CurrentRows).ToTable();
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

        Session["dtFilter_L_B"] = dtModel;
        Session["DtModel"] = dtModel;
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtModel.Rows.Count;
        //AllPageCode();
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
        return ArebicMessage;
    }
    public void fillOptionCategorygrid()
    {
        try
        {
            string ModelId = ViewState["ModelId"].ToString();

            DataTable dt = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString());
            dt = new DataView(dt, "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable(true, "OptionCategoryId");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvOptionCategory, dt, "", "");


            if (dt.Rows.Count == 0)
            {

                txtOptionPartNo.Text = "";
                txtFinalPartNo.Text = "";
                lblstockinfo.Text = "0.000";
                txtDesc.Text = "";
            }
            else
            {
                if (txtOptionPartNo.Text == "")
                {
                    txtOptionPartNo.Text = "0".PadRight(dt.Rows.Count, '0');

                    txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                    lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
                }
            }
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
            OpCateName = ObjOpCate.GetOptionCategoryTruebyId(Session["CompId"].ToString().ToString(), OpCatId.ToString()).Rows[0]["EName"].ToString();
        }
        catch
        {

        }
        return OpCateName;
    }

    //Updated


    protected void rdoOption_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString()), "OptionCategoryId='" + lblCategoryId.Text.Trim() + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();
        DataTable dtopCategoryDetail = objProOpCatedetail.GetDetailByOpCateId(Session["CompId"].ToString().ToString(), lblCategoryId.Text.Trim());
        string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
        DataTable dtmodelLabel = objda.return_DataTable(sql);
        for (int i = 0; i < RdoList.Items.Count; i++)
        {

            if (RdoList.Items[i].Selected)
            {
                if (dtopCategoryDetail.Rows.Count == 0)
                {
                    //for handle if session is null
                    string strLabelCoreCategoryId = string.Empty;
                    string strLabelQtyCategoryId = string.Empty;
                    string LabelPackingCategoryId = string.Empty;
                    if (Session["LabelCoreCategoryId"] != null)
                    {
                        strLabelCoreCategoryId = Session["LabelCoreCategoryId"].ToString();
                    }
                    else
                    {
                        strLabelCoreCategoryId = "0";
                    }
                    if (Session["LabelQtyCategoryId"] != null)
                    {
                        strLabelQtyCategoryId = Session["LabelQtyCategoryId"].ToString();
                    }
                    else
                    {
                        strLabelQtyCategoryId = "0";
                    }
                    if (Session["LabelPackingCategoryId"] != null)
                    {
                        LabelPackingCategoryId = Session["LabelPackingCategoryId"].ToString();
                    }
                    else
                    {
                        LabelPackingCategoryId = "0";
                    }



                    DataTable dtOptionTemp = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (strLabelCoreCategoryId == lblCategoryId.Text.Trim())
                    {
                        FilterLabel(dtOptionTemp.Rows[0]["TransID"].ToString(), strLabelQtyCategoryId);
                    }
                    if (strLabelQtyCategoryId == lblCategoryId.Text.Trim())
                    {
                        FilterLabel(dtOptionTemp.Rows[0]["TransID"].ToString(), LabelPackingCategoryId);
                    }
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
                        txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                        lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
                    }
                    catch
                    {

                    }
                    try
                    {
                        txtOptionPartNo.Text = txtOptionPartNo.Text.Insert(Convert.ToInt32(strsno) - 1, RdoList.Items[i].Value);
                        txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                        lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
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

                }
                else
                {


                    DataTable dtLabel = new DataView(dtmodelLabel, "Field2=" + RdoList.Items[i].Value.ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable();

                    FilterLabel(dtLabel.Rows[0]["Trans_Id"].ToString(), dtopCategoryDetail.Rows[0]["Field2"].ToString());
                    FilterLabel(dtLabel.Rows[0]["Trans_Id"].ToString(), dtopCategoryDetail.Rows[0]["Field3"].ToString());

                    Session["LabelCoreCategoryId"] = dtopCategoryDetail.Rows[0]["Field2"].ToString();
                    Session["LabelQtyCategoryId"] = dtopCategoryDetail.Rows[0]["QtyCategoryId"].ToString();
                    Session["LabelPackingCategoryId"] = dtopCategoryDetail.Rows[0]["Field1"].ToString();
                    DataTable DTtEMP = new DataView(dtmodelLabel, "Field2='" + RdoList.Items[i].Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (DTtEMP.Rows[0]["Field1"].ToString().Trim() != "")
                        Desc += "," + DTtEMP.Rows[0]["Field3"].ToString().Trim() + DTtEMP.Rows[0]["Height"].ToString() + "mm" + "X" + DTtEMP.Rows[0]["Width"].ToString() + "mm - " + DTtEMP.Rows[0]["Field1"].ToString().Trim();
                    else
                        Desc += "," + DTtEMP.Rows[0]["Field3"].ToString().Trim() + DTtEMP.Rows[0]["Height"].ToString() + "mm" + "X" + DTtEMP.Rows[0]["Width"].ToString() + "mm";




                    string newStr = string.Empty;

                    newStr = txtOptionPartNo.Text.Substring(txtOptionPartNo.Text.LastIndexOf("#")).Replace("#", "");


                    if (!txtOptionPartNo.Text.Contains("#"))
                    {
                        txtOptionPartNo.Text.Insert(txtOptionPartNo.Text.Length - 1, "#0");

                        txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                        lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
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
                        txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
                        lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
                    }
                    catch
                    {

                    }

                    try
                    {

                        DTtEMP = new DataView(dtmodelLabel, "Field2='" + OldPartOption.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                        txtDesc.Text = txtDesc.Text.Replace("," + DTtEMP.Rows[0]["Field3"].ToString() + DTtEMP.Rows[0]["Height"].ToString() + "mm" + "X" + DTtEMP.Rows[0]["Width"].ToString() + "mm", "");
                    }
                    catch
                    {

                    }
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
        txtDesc.Text += Desc;

        getdescription(txtOptionPartNo.Text, ModelId);
    }


    #region FilterCategory
    public void FilterLabel(string strParentId, string strChildId)
    {
        string ModelId = ViewState["ModelId"].ToString();
        DataTable dtBom = new DataTable();
        dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString(), ModelId.ToString());
        try
        {
            dtBom = new DataView(dtBom, "OptionCategoryID=" + strChildId + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        foreach (GridViewRow gvRow in gvOptionCategory.Rows)
        {
            RadioButtonList RdoList = (RadioButtonList)gvRow.FindControl("rdoOption");
            Label lblCategoryId = (Label)gvRow.FindControl("lblOptionCategoryId");

            if (lblCategoryId.Text == strChildId)
            {

                string sql = "select Child_Id from dbo.Inv_Label_Filter where Parent_Id=" + strParentId + "";
                DataTable dtLabelFilter = objda.return_DataTable(sql);

                if (dtLabelFilter.Rows.Count > 0)
                {

                    string strQuantityTransId = string.Empty;
                    foreach (DataRow dr in dtLabelFilter.Rows)
                    {
                        if (strQuantityTransId == "")
                        {
                            strQuantityTransId = dr["Child_Id"].ToString();
                        }
                        else
                        {
                            strQuantityTransId = strQuantityTransId + "," + dr["Child_Id"].ToString();
                        }
                    }
                    try
                    {
                        dtBom = new DataView(dtBom, "TransID in (" + strQuantityTransId + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                    if (dtBom.Rows.Count > 0)
                    {

                        RdoList.Items.Clear();

                        foreach (DataRow row in dtBom.Rows)
                        {
                            var txt = string.Empty;

                            txt = row["ShortDescription"].ToString();

                            var val = row["OptionID"].ToString();
                            var item = new ListItem(txt, val);

                            if (row["OptionID"].ToString() == txtOptionPartNo.Text[Convert.ToInt32(row["Field1"].ToString()) - 1].ToString())
                            {
                                item.Selected = true;
                            }
                            RdoList.Items.Add(item);
                        }
                    }

                }
                break;
            }
        }


    }
    #endregion

    public void getdescription(string PartNumber, string ModelId)
    {
        txtDesc.Text = "";
        string sql = "";
        DataTable dt = new DataTable();
        DataTable dtModel = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), "True"), "Trans_Id='" + ModelId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtModel.Rows.Count > 0)
        {


            txtDesc.Text = dtModel.Rows[0]["Field3"].ToString();



            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString(), ModelId.ToString());

            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();

                if (PartNumber.Contains('#'))
                {
                    if (i == 1)
                    {
                        try
                        {
                            sql = "select width,height,Field1,Field3 from Inv_Model_LabelSize where Model_Id=" + ModelId.ToString() + " and  Field2=" + PartNumber.Split('#')[1].ToString() + "";
                            dt = objda.return_DataTable(sql);



                            if (dt.Rows[0]["Field1"].ToString().Trim() != "")
                                txtDesc.Text = txtDesc.Text + "," + dt.Rows[0]["Field3"].ToString() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm - " + dt.Rows[0]["Field1"].ToString().Trim();
                            else
                                txtDesc.Text = txtDesc.Text + "," + dt.Rows[0]["Field3"].ToString() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm";


                        }
                        catch
                        {

                        }
                    }
                }


                if (Charvalue != "0")
                {
                    if (PartNumber.Contains('#'))
                    {
                        if (Convert.ToInt32(srno) == 2)
                        {
                            continue;
                        }
                    }

                    if (Charvalue == "#")
                    {
                        break;
                    }


                    if (new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString().Trim() != "")
                    {

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


            }
        }

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
            FillRelatedProduct(dt.Rows[0]["Trans_Id"].ToString());
            ViewState["ModelId"] = dt.Rows[0]["Trans_Id"].ToString();
            hdnModelPrice.Value = dt.Rows[0]["Field1"].ToString();
            hdnModelImage.Value = dt.Rows[0]["Field2"].ToString();
            hdnModelDesc.Value = dt.Rows[0]["Field3"].ToString();
            hdnCurrencyId.Value = dt.Rows[0]["Field4"].ToString();

            fillOptionCategorygrid();
            //fillpart();
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

        fillpart();

        if (Session["PartList"] != null)
        {
            Session["DTPartNoReport"] = Session["PartList"];
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/LabelBuilderReport.aspx?ModelId=" + ViewState["ModelId"].ToString() + "','window','width=1024');", true);
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
            DataTable dtmodellabel = new DataTable();
            string ModelId = ViewState["ModelId"].ToString();

            if (ViewState["labelCateId"] != null)
            {
                dt = objProOpCatedetail.GetDetailByOpCateId(Session["CompId"].ToString().ToString(), ViewState["labelCateId"].ToString());
                string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
                dtmodellabel = objda.return_DataTable(sql);

            }
            DataTable dtOption = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString());

            float labelQty = 0;
            float ColorPrice = 0;
            float MaterialPrice = 0;
            float PaperSize = 0;
            float Packingqty = 0;
            string labelQtyCateId = dt.Rows[0]["QtyCategoryId"].ToString();
            string labelMeterialCateId = dt.Rows[0]["MaterialCategoryId"].ToString();
            string labelColorCateId = dt.Rows[0]["ColorCategoryId"].ToString();
            string PackingCatId = dt.Rows[0]["Field1"].ToString();
            string strExchangerate = "0";
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
                            dtmodellabel = new DataView(dtmodellabel, "Field2='" + RdoList.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                            PaperSize = float.Parse(dtmodellabel.Rows[0]["MMSize"].ToString()) / float.Parse(dt.Rows[0]["DefaultValue"].ToString());
                        }
                        else
                        {

                            DataTable dtOptionTemp = new DataView(dtOption, "OptionCategoryId='" + lblCategoryId.Text.Trim() + "' and OptionId='" + RdoList.Items[i].Value + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();
                            if (dt.Rows.Count != 0)
                            {
                                if (lblCategoryId.Text.Trim() == labelMeterialCateId.Trim())
                                {

                                    if (dtOptionTemp.Rows[0]["SubProductID"].ToString() != "0")
                                    {
                                        if (objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dtOptionTemp.Rows[0]["SubProductID"].ToString().Trim()).Rows.Count > 0)
                                        {
                                            try
                                            {

                                                MaterialPrice = float.Parse(new DataView(objStock.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + dtOptionTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitPrice"].ToString());

                                                strExchangerate = SystemParameter.GetExchageRate(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString(), Session["DBConnection"].ToString());
                                                if (strExchangerate == "")
                                                {
                                                    strExchangerate = "1";
                                                }

                                                MaterialPrice = MaterialPrice * float.Parse(strExchangerate);
                                            }
                                            catch
                                            {
                                                MaterialPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            MaterialPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        MaterialPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString());
                                    }
                                }
                                if (lblCategoryId.Text.Trim() == labelColorCateId.Trim())
                                {

                                    ColorPrice = float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                                }
                                if (lblCategoryId.Text.Trim() == labelQtyCateId.Trim())
                                {
                                    labelQty = float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());

                                }
                                if (lblCategoryId.Text.Trim() == PackingCatId.Trim())
                                {
                                    Packingqty = float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());

                                }
                            }
                        }
                    }
                }
            }

            float M2PaperUsage = PaperSize * labelQty * Packingqty;
            float Price = (M2PaperUsage * MaterialPrice) + (M2PaperUsage * ColorPrice);
            //float Price = ((PaperSize * MaterialPrice) + ColorPrice) * labelQty;

            string costtimes = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Sales_Price_1"].ToString();
            txtPrice.Text = (float.Parse(costtimes) * Price).ToString();
        }
        catch
        {
            txtPrice.Text = "0";
        }
    }
    #region generatepartno
    void fillpart()
    {
        try
        {

            string strlabelqtyCategoryid = string.Empty;
            string strlPackingCategoryid = string.Empty;
            string strCoreCategoryId = string.Empty;
            string strPerforationCategoryId = string.Empty;
            string strLabelSizeCategoryId = string.Empty;
            //here we get quantity  and packing category id
            string strsql = "select distinct OptionCategoryId as LabelSizeCategoryId, Field2 as CoreCategoryId,QtyCategoryId,Field1 as PackingCategoryId,QtyCategoryId,Field3 as PerforationCategoryId from dbo.Inv_ProductOptionCategoryDetail";
            DataTable dtDetail = objda.return_DataTable(strsql);

            strlabelqtyCategoryid = dtDetail.Rows[0]["QtyCategoryId"].ToString();
            strlPackingCategoryid = dtDetail.Rows[0]["PackingCategoryId"].ToString();
            strCoreCategoryId = dtDetail.Rows[0]["CoreCategoryId"].ToString();
            strCoreCategoryId = dtDetail.Rows[0]["CoreCategoryId"].ToString();
            strLabelSizeCategoryId = dtDetail.Rows[0]["LabelSizeCategoryId"].ToString();
            //strPerforationCategoryId = dtDetail.Rows[0]["PerforationCategoryId"].ToString();


            Session["PartList"] = null;
            string ModelId = ViewState["ModelId"].ToString();
            DataTable dt = new DataView(ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString()), "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                DataTable dtCate = dt.DefaultView.ToTable(true, "OptionCategoryId");
                ArrayList arr = new ArrayList();
                ArrayList ar1 = new ArrayList();
                DataTable dtPartNo = new DataTable();
                dtPartNo.Columns.Add("PartNo");
                dtPartNo.Columns.Add("TransID");

                foreach (DataRow dr in dtCate.Rows)
                {
                    DataTable dttemp = new DataView(dt, "OptionCategoryId='" + dr["OptionCategoryId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (Session["PartList"] == null)
                    {
                        for (int i = 0; i < dttemp.Rows.Count; i++)
                        {
                            DataRow dr1 = dtPartNo.NewRow();
                            if (dr["OptionCategoryId"].ToString().Trim() == strLabelSizeCategoryId.Trim())
                            {
                                dr1[0] = "0";
                            }
                            else
                            {
                                dr1[0] = dttemp.Rows[i]["OptionID"].ToString();
                            }
                            dtPartNo.Rows.Add(dr1);
                        }
                        Session["PartList"] = dtPartNo;

                    }

                    else
                    {


                        //save all arraylist dat in datatable

                        dtPartNo = new DataTable();
                        dtPartNo.Columns.Add("PartNo");
                        DataTable dt1 = (DataTable)Session["PartList"];

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dttemp.Rows.Count; j++)
                            {
                                //here we check that this combination is allow or not
                                //if current category is packing oy label quantity then 
                                if (dr["OptionCategoryId"].ToString() == strlabelqtyCategoryid || dr["OptionCategoryId"].ToString() == strlPackingCategoryid)
                                {

                                    string OptionId = dt1.Rows[i][0].ToString()[dt1.Rows[i][0].ToString().Length - 1].ToString();

                                    string ParentId = new DataView(dt, "OptionID='" + OptionId + "' and Field1='" + dt1.Rows[i][0].ToString().Length + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["TransID"].ToString();
                                    string ChildId = dttemp.Rows[j]["TransID"].ToString();
                                    string sql = "select Child_Id from dbo.Inv_Label_Filter where Parent_Id=" + ParentId + "";
                                    DataTable dtLabelFilter = objda.return_DataTable(sql);
                                    if (dtLabelFilter.Rows.Count == 0)
                                    {
                                        DataRow dr1 = dtPartNo.NewRow();

                                        dr1[0] = dt1.Rows[i][0].ToString() + dttemp.Rows[j]["OptionID"].ToString();

                                        dtPartNo.Rows.Add(dr1);
                                    }
                                    else
                                    {
                                        sql = "select Child_Id from dbo.Inv_Label_Filter where Parent_Id=" + ParentId + " and child_Id=" + ChildId + " ";
                                        if (objda.return_DataTable(sql).Rows.Count > 0)
                                        {
                                            DataRow dr1 = dtPartNo.NewRow();
                                            dr1[0] = dt1.Rows[i][0].ToString() + dttemp.Rows[j]["OptionID"].ToString();
                                            dtPartNo.Rows.Add(dr1);
                                        }
                                    }

                                }
                                else
                                {

                                    DataRow dr1 = dtPartNo.NewRow();
                                    if (dr["OptionCategoryId"].ToString().Trim() == strLabelSizeCategoryId.Trim())
                                    {
                                        dr1[0] = dt1.Rows[i][0].ToString() + "0";
                                    }
                                    else
                                    {
                                        dr1[0] = dt1.Rows[i][0].ToString() + dttemp.Rows[j]["OptionID"].ToString();
                                    }
                                    dtPartNo.Rows.Add(dr1);
                                }
                            }
                        }

                        Session["PartList"] = dtPartNo;
                    }
                }

                //for set label in prefix
                if (Session["PartList"] != null)
                {
                    //here we get serial number of core and perforation
                    int coreIndex = 0;
                    int PerforationIndex = 0;
                    coreIndex = Convert.ToInt32(new DataView(dt, "OptionCategoryId='" + strCoreCategoryId + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString());
                    //PerforationIndex = Convert.ToInt32(new DataView(dt, "OptionCategoryId='" + strPerforationCategoryId + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString());
                    string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
                    DataTable dtlabel = objda.return_DataTable(sql);
                    if (dtlabel.Rows.Count > 0)
                    {
                        DataTable dtPart = (DataTable)Session["PartList"];
                        dtPartNo = new DataTable();
                        dtPartNo.Columns.Add("PartNo");
                        dtPartNo.Columns.Add("LabelId");
                        dtPartNo.Columns.Add("LabelSize");
                        dtPartNo.Columns.Add("Description");
                        dtPartNo.Columns.Add("SalesPrice");
                        dtPartNo.Columns.Add("ModelDescription");

                        for (int i = 0; i < dtPart.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtlabel.Rows.Count; j++)
                            {

                                //here check for core 

                                string childId = new DataView(dt, "OptionID='" + dtPart.Rows[i][0].ToString()[coreIndex - 1].ToString() + "' and Field1='" + coreIndex.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["TransID"].ToString();
                                //string childId1 = new DataView(dt, "OptionID='" + dtPart.Rows[i][0].ToString()[PerforationIndex - 1].ToString() + "' and Field1='" + PerforationIndex.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["TransID"].ToString();
                                sql = "select Child_Id from dbo.Inv_Label_Filter where Parent_Id=" + dtlabel.Rows[j]["Trans_Id"].ToString() + "";
                                DataTable dtLabelFilter = objda.return_DataTable(sql);
                                if (dtLabelFilter.Rows.Count > 0)
                                {
                                    sql = "select Child_Id from dbo.Inv_Label_Filter where Parent_Id=" + dtlabel.Rows[j]["Trans_Id"].ToString() + " and child_Id In ('" + childId + "') ";
                                    if (objda.return_DataTable(sql).Rows.Count > 0)
                                    {

                                        DataRow dr1 = dtPartNo.NewRow();
                                        dr1[4] = CalculateLabelCost(ModelId, dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString());
                                        dr1[0] = txtModelNo.Text.Trim() + "-" + dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString();
                                        dr1[1] = dtlabel.Rows[j]["Trans_Id"].ToString();

                                        if (dtlabel.Rows[j]["Field1"].ToString().Trim() != "")
                                            dr1[2] = dtlabel.Rows[j]["Field3"].ToString().Trim() + dtlabel.Rows[j]["Width"].ToString() + "X" + dtlabel.Rows[j]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[j]["Field1"].ToString().Trim();
                                        else
                                            dr1[2] = dtlabel.Rows[j]["Field3"].ToString().Trim() + dtlabel.Rows[j]["Width"].ToString() + "X" + dtlabel.Rows[j]["Height"].ToString() + " " + "(mm)";


                                        dr1[3] = getLabeldescription(dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString(), ModelId);
                                        dr1[5] = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Description"].ToString();
                                        dtPartNo.Rows.Add(dr1);
                                    }
                                }
                                else
                                {

                                    DataRow dr1 = dtPartNo.NewRow();
                                    dr1[4] = CalculateLabelCost(ModelId, dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString());
                                    dr1[0] = txtModelNo.Text.Trim() + "-" + dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString();
                                    dr1[1] = dtlabel.Rows[j]["Trans_Id"].ToString();
                                    if (dtlabel.Rows[j]["Field1"].ToString().Trim() != "")
                                        dr1[2] = dtlabel.Rows[j]["Field3"].ToString().Trim() + dtlabel.Rows[j]["Width"].ToString() + "X" + dtlabel.Rows[j]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[j]["Field1"].ToString().Trim();
                                    else
                                        dr1[2] = dtlabel.Rows[j]["Field3"].ToString().Trim() + dtlabel.Rows[j]["Width"].ToString() + "X" + dtlabel.Rows[j]["Height"].ToString() + " " + "(mm)";


                                    dr1[3] = getLabeldescription(dtPart.Rows[i][0].ToString() + "#" + dtlabel.Rows[j]["Field2"].ToString(), ModelId);
                                    dr1[5] = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Description"].ToString();
                                    dtPartNo.Rows.Add(dr1);
                                }
                            }

                        }

                        Session["PartList"] = dtPartNo;
                    }
                    else
                    {
                        //here we set label descrition

                        DataTable dtPart = (DataTable)Session["PartList"];
                        dtPartNo = new DataTable();
                        dtPartNo.Columns.Add("PartNo");
                        dtPartNo.Columns.Add("LabelId");
                        dtPartNo.Columns.Add("LabelSize");
                        dtPartNo.Columns.Add("Description");
                        dtPartNo.Columns.Add("SalesPrice");
                        dtPartNo.Columns.Add("ModelDescription");
                        for (int i = 0; i < dtPart.Rows.Count; i++)
                        {


                            DataRow dr1 = dtPartNo.NewRow();
                            dr1[0] = txtModelNo.Text.Trim() + "-" + dtPart.Rows[i][0].ToString().Trim();
                            dr1[1] = "0";
                            dr1[2] = "";
                            dr1[3] = getLabeldescription(dtPart.Rows[i][0].ToString().Trim(), ModelId);
                            dr1[4] = "0";
                            dr1[5] = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Description"].ToString();
                            dtPartNo.Rows.Add(dr1);
                        }
                        Session["PartList"] = dtPartNo;
                    }
                }
            }
        }
        catch
        {

        }
        //gvPartNo.DataSource = (DataTable)Session["PartList"];
        //gvPartNo.DataBind();
    }
    protected void lblPartNo_Click(object sender, EventArgs e)
    {
        LinkButton li = (LinkButton)sender;
        txtOptionPartNo.Text = li.Text.Trim();
        txtFinalPartNo.Text = txtModelNo.Text + "-" + txtOptionPartNo.Text;
        lblstockinfo.Text = GetProductStock(txtFinalPartNo.Text);
        txtModelNo_TextChanged(null, null);
        //txtOptionPartNo_TextChanged(null, null);

    }

    public string getLabeldescription(string PartNumber, string ModelId)
    {
        string Description = string.Empty;

        string sql = "";
        DataTable dt = new DataTable();
        DataTable dtModel = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), "True"), "Trans_Id='" + ModelId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtModel.Rows.Count > 0)
        {


            Description = dtModel.Rows[0]["Field3"].ToString();
            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString(), ModelId.ToString());

            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();

                if (PartNumber.Contains('#'))
                {
                    if (i == 1)
                    {
                        try
                        {
                            sql = "select width,height,Field1,Field3 from Inv_Model_LabelSize where Model_Id=" + ModelId + " and Field2=" + PartNumber.Split('#')[1].ToString() + "";
                            dt = objda.return_DataTable(sql);
                            if (dt.Rows[0]["Field1"].ToString().Trim() != "")
                                Description = Description + "," + dt.Rows[0]["Field3"].ToString().Trim() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm - " + dt.Rows[0]["Field1"].ToString().Trim();
                            else
                                Description = Description + "," + dt.Rows[0]["Field3"].ToString().Trim() + dt.Rows[0]["width"].ToString() + "mm" + "X" + dt.Rows[0]["height"].ToString() + "mm";


                        }
                        catch
                        {

                        }
                    }
                }
                if (Charvalue != "0")
                {
                    if (PartNumber.Contains('#'))
                    {
                        if (Convert.ToInt32(srno) == 2)
                        {
                            continue;
                        }
                    }



                    if (Charvalue == "#")
                    {
                        break;
                    }
                    if (new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString().Trim() != "")
                    {

                        if (Description == "")
                        {

                            Description = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();

                        }
                        else
                        {

                            Description = Description + "," + new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ShortDescription"].ToString();

                        }
                    }
                }


            }
        }

        return Description;

    }
    public string CalculateLabelCost(string ModelId, string PartNumber)
    {

        string Usedquantity = "0";
        try
        {
            DataTable dt = new DataTable();
            DataTable dtmodellabel = new DataTable();


            dt = objProOpCatedetail.GetAllRecord(Session["CompId"].ToString().ToString());
            string sql = "select * from Inv_Model_LabelSize where model_id=" + ModelId + "";
            dtmodellabel = objda.return_DataTable(sql);

            DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId.ToString());

            float labelQty = 0;
            float ColorPrice = 0;
            float MaterialPrice = 0;
            float PaperSize = 0;
            float Packingqty = 0;
            string labelQtyCateId = dt.Rows[0]["QtyCategoryId"].ToString();
            string labelMeterialCateId = dt.Rows[0]["MaterialCategoryId"].ToString();
            string labelColorCateId = dt.Rows[0]["ColorCategoryId"].ToString();
            string LabelSizeCatId = dt.Rows[0]["OptionCategoryId"].ToString();
            string PackingCatId = dt.Rows[0]["Field1"].ToString();
            string strExchangerate = "0";

            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();

                if (Charvalue == "#")
                {
                    dtmodellabel = new DataView(dtmodellabel, "Field2='" + PartNumber.Split('#')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    PaperSize = float.Parse(dtmodellabel.Rows[0]["MMSize"].ToString()) / float.Parse(dt.Rows[0]["DefaultValue"].ToString());
                    break;
                }

                DataTable dtTemp = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelQtyCateId.Trim())
                    {
                        labelQty = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());

                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelMeterialCateId.Trim())
                    {
                        if (dtTemp.Rows[0]["SubProductID"].ToString() != "0")
                        {
                            if (objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim()).Rows.Count > 0)
                            {
                                try
                                {

                                    MaterialPrice = float.Parse(new DataView(objStock.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitPrice"].ToString());

                                    strExchangerate = SystemParameter.GetExchageRate(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString(), Session["DBConnection"].ToString());
                                    if (strExchangerate == "")
                                    {
                                        strExchangerate = "1";
                                    }

                                    MaterialPrice = MaterialPrice * float.Parse(strExchangerate);
                                }
                                catch
                                {
                                    MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                                }
                            }
                            else
                            {
                                MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                            }
                        }
                        else
                        {
                            MaterialPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString());
                        }
                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelColorCateId.Trim())
                    {

                        ColorPrice = float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtTemp.Rows[0]["Quantity"].ToString());
                    }
                    if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == PackingCatId.Trim())
                    {
                        Packingqty = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());
                    }
                }
            }
            strExchangerate = "1";
            try
            {

                if (ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString() != "" && ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString() != "0")
                {
                    strExchangerate = SystemParameter.GetExchageRate(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, true.ToString()).Rows[0]["Field4"].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());

                }
                else
                {
                    strExchangerate = "1";
                }
            }
            catch
            {
                strExchangerate = "1";
            }

            float M2PaperUsage = PaperSize * labelQty * Packingqty;
            string costtimes = string.Empty;

            float Price = (M2PaperUsage * MaterialPrice) + (M2PaperUsage * ColorPrice);

            if (objInvParm.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Price").Rows[0]["ParameterValue"].ToString().Trim() == "1")
            {
                costtimes = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Sales_Price_1"].ToString();

                Usedquantity = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(strExchangerate) * (Price * float.Parse(costtimes))).ToString());
            }
            if (objInvParm.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Price").Rows[0]["ParameterValue"].ToString().Trim() == "2")
            {
                costtimes = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Sales_Price_2"].ToString();

                Usedquantity = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(strExchangerate) * (Price * float.Parse(costtimes))).ToString());

            }
            if (objInvParm.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Price").Rows[0]["ParameterValue"].ToString().Trim() == "3")
            {
                costtimes = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Sales_Price_3"].ToString();

                Usedquantity = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), (float.Parse(strExchangerate) * (Price * float.Parse(costtimes))).ToString());

            }
        }
        catch
        {
            Usedquantity = "0";
        }

        return Usedquantity;
    }
    #endregion
    #region FilterCategory

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

                DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString());
                dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btngo);
                }
                else
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
    //Supplier :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    //Supplier :- End
    #endregion
    #region StockInfo

    public string GetProductStock(string strPartNo)
    {
        string SysQty = string.Empty;
        try
        {

            SysQty = objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), GetProductId_By_ProductCode(strPartNo)).Rows[0]["Quantity"].ToString();
        }
        catch
        {
            SysQty = "0";
        }


        if (SysQty == "")
        {
            SysQty = "0.000";
        }

        return GetAmountDecimal(SysQty);
    }

    public string GetProductId_By_ProductCode(string strPartNo)
    {
        string ProductId = string.Empty;
        DataTable dt = ObjInvProductMaster.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), strPartNo, HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductId = dt.Rows[0]["ProductId"].ToString();
        }
        else
        {
            ProductId = "0";
        }
        return ProductId;
    }

    public string GetAmountDecimal(string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);

    }


    #endregion
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


    }


    public string GetUnitName(string UnitId)
    {
        DataTable dt = new DataView(ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString()), "Unit_Id='" + UnitId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string strUnitName = "NA";
        if (dt.Rows.Count != 0)
        {
            strUnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return strUnitName;
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

            dt.Rows.Add(Dr);
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

            dt.Rows.Add(Dr);
        }

        objPageCmn.FillData((object)gvProduct, dt, "", "");

        Session["DtQuotationItemList"] = dt;
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
                dr["Sysqty"] = objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";

            }
            DtProduct.Rows.Add(dr);
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
                dr["Sysqty"] = objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), gvProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";

            }
            DtProduct.Rows.Add(dr);
        }
        return DtProduct;
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
        }
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

            dt.Rows.Add(Dr);
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

            dt.Rows.Add(Dr);
        }

        objPageCmn.FillData((object)gvProduct, dt, "", "");

        Session["DtQuotationItemList"] = dt;


        DataTable dtRelatedProduct = new DataView((DataTable)Session["dtModelRelatedProduct"], "ProductId<>" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable();

        gvRelatedProduct.DataSource = dtRelatedProduct;
        gvRelatedProduct.DataBind();
        Session["dtModelRelatedProduct"] = dtRelatedProduct;
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

    }
    #region SearchEngine

    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlModelNo.Items.Clear();
        ddlIndustry.Items.Clear();
        ddlApplicationType.Items.Clear();
        ddlModelNo.Items.Clear();


        DataTable dt = objda.return_DataTable("select distinct inv_product_categorymaster.category_id,inv_product_categorymaster.Category_Name  from Inv_Model_CategoryConfig inner join inv_product_categorymaster on Inv_Model_CategoryConfig.Industry_id = inv_product_categorymaster.Category_Id where Inv_Model_CategoryConfig.material_id = " + ddlcategory.SelectedValue.Trim() + "  order by inv_product_categorymaster.Category_Name ");
        ddlIndustry.DataSource = dt;
        ddlIndustry.DataTextField = "category_name";
        ddlIndustry.DataValueField = "category_id";
        ddlIndustry.DataBind();
        ddlIndustry.Items.Insert(0, new ListItem("--Select--", "0"));
        txtmodelnamedesc.Text = "";
    }


    public void FillModelSearchEngine()
    {
        ddlModelNo.Items.Clear();
        ddlIndustry.Items.Clear();
        DataTable dt = objda.return_DataTable("select distinct inv_modelmaster.Trans_Id,Inv_ModelMaster.Model_No from Inv_Model_CategoryConfig inner join inv_modelmaster on Inv_Model_CategoryConfig.model_id = inv_modelmaster.Trans_Id where Inv_Model_CategoryConfig.Material_Id = " + ddlcategory.SelectedValue.Trim() + " order by Inv_ModelMaster.model_no");
        ddlModelNo.DataSource = dt;
        ddlModelNo.DataTextField = "model_no";
        ddlModelNo.DataValueField = "Trans_Id";
        ddlModelNo.DataBind();
        ddlModelNo.Items.Insert(0, new ListItem("--Select--", "0"));
        txtmodelnamedesc.Text = "";
    }

    public void FillCategory()
    {
        DataTable dtCategory = objda.return_DataTable("select distinct inv_product_categorymaster.category_id,inv_product_categorymaster.Category_Name  from Inv_Model_CategoryConfig inner join inv_product_categorymaster on Inv_Model_CategoryConfig.material_id = inv_product_categorymaster.Category_Id order by inv_product_categorymaster.Category_Name ");
        //DataTable dtCategory = objda.return_DataTable("select distinct Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Id from    inv_model_category inner join Inv_ModelMaster   on inv_model_category.ModelId = Inv_ModelMaster.Trans_Id inner join Inv_Product_CategoryMaster on inv_model_category.categoryid = Inv_Product_CategoryMaster.Category_Id    where Inv_ModelMaster.islabel = 'True' order by Inv_Product_CategoryMaster.Category_Name");

        
        ddlcategory.DataSource = dtCategory;
        ddlcategory.DataTextField = "category_name";
        ddlcategory.DataValueField = "category_id";
        ddlcategory.DataBind();
        ddlcategory.Items.Insert(0, new ListItem("--Select--", "0"));

    }

    protected void ddlIndustry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlApplicationType.Items.Clear();
        ddlModelNo.Items.Clear();
        DataTable dt = objda.return_DataTable("select distinct inv_product_categorymaster.category_id,inv_product_categorymaster.Category_Name  from Inv_Model_CategoryConfig inner join inv_product_categorymaster on Inv_Model_CategoryConfig.Application_Type_Id = inv_product_categorymaster.Category_Id where Inv_Model_CategoryConfig.material_id = " + ddlcategory.SelectedValue.Trim() + " and Inv_Model_CategoryConfig.Industry_Id=" + ddlIndustry.SelectedValue.Trim() + "  order by inv_product_categorymaster.Category_Name ");
        ddlApplicationType.DataSource = dt;
        ddlApplicationType.DataTextField = "category_name";
        ddlApplicationType.DataValueField = "category_id";
        ddlApplicationType.DataBind();
        ddlApplicationType.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void ddlApplicationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlModelNo.Items.Clear();
        DataTable dt = objda.return_DataTable("select distinct inv_modelmaster.Trans_Id,Inv_ModelMaster.Model_No from Inv_Model_CategoryConfig inner join inv_modelmaster on Inv_Model_CategoryConfig.model_id = inv_modelmaster.Trans_Id where Inv_Model_CategoryConfig.Material_Id = " + ddlcategory.SelectedValue.Trim() + " and Inv_Model_CategoryConfig.Industry_Id=" + ddlIndustry.SelectedValue.Trim() + " and Inv_Model_CategoryConfig.Application_Type_Id=" + ddlApplicationType.SelectedValue.Trim() + " order by Inv_ModelMaster.model_no");
        ddlModelNo.DataSource = dt;
        ddlModelNo.DataTextField = "model_no";
        ddlModelNo.DataValueField = "Trans_Id";
        ddlModelNo.DataBind();
        ddlModelNo.Items.Insert(0, new ListItem("--Select--", "0"));
        txtmodelnamedesc.Text = "";
    }


    #endregion

    protected void ddlModelNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmodelnamedesc.Text = "";
        //for printer
        ddlPrinter.Items.Clear();
        //DataTable dt = objda.return_DataTable("select distinct set_printermaster.Printer_Name,inv_bom.Field3 as Printer_Id from inv_bom inner join Inv_ModelMaster on inv_bom.modelid = Inv_ModelMaster.Trans_id inner join set_printermaster  on Inv_BOM.Field3 = set_printermaster.Printer_id where Inv_BOM.ModelId = " + ddlModelNo.SelectedValue.Trim() + " and set_printermaster.IsActive='True'");
        DataTable dt = objda.return_DataTable("select Printer_id,Printer_Name from set_printermaster where printer_id in ( SELECT CAST(Value AS int) AS emp_id FROM F_Split(STUFF(( SELECT ',' + fIELD3 FROM INV_BOM WHERE FIELD3<>'' AND Inv_BOM.ModelId = " + ddlModelNo.SelectedValue.Trim() + " FOR XML PATH('') ), 1, 1, ''),','))");

        ddlPrinter.DataSource = dt;
        ddlPrinter.DataTextField = "Printer_Name";
        ddlPrinter.DataValueField = "Printer_Id";
        ddlPrinter.DataBind();
        ddlPrinter.Items.Insert(0, new ListItem("--Select--", "0"));

        DataTable dtmodel = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ddlModelNo.SelectedValue.Trim(), "True");

        if (dtmodel.Rows.Count > 0)
        {
            txtmodelnamedesc.Text = dtmodel.Rows[0]["Model_Name"].ToString();
        }



        //string stroptioncategoryid = "0";
        //string strsql = string.Empty;
        //strsql = "select distinct Field2 as CoreCategoryId,QtyCategoryId,Field1 as PackingCategoryId,QtyCategoryId,Field3 as PerforationCategoryId from dbo.Inv_ProductOptionCategoryDetail";
        //DataTable dtDetail = objda.return_DataTable(strsql);
        //if (dtDetail.Rows.Count > 0)
        //{
        //    stroptioncategoryid = dtDetail.Rows[0]["CoreCategoryId"].ToString();
        //}




    }

    protected void ddlPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCore.Items.Clear();
        DataTable dtcorelist = objda.return_DataTable("select OptionDescription,TransID from inv_bom where modelid =" + ddlModelNo.SelectedValue.Trim() + " and  " + ddlPrinter.SelectedValue + " in  (select cast(Value as int)  from dbo.F_Split(Field3,',')) ");
        ddlCore.DataSource = dtcorelist;
        ddlCore.DataTextField = "OptionDescription";
        ddlCore.DataValueField = "TransID";
        ddlCore.DataBind();
        ddlCore.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void ddlCore_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddllabelSize.Items.Clear();
        ddllabelqty.Items.Clear();

        //for label size

        DataTable dt = objda.return_DataTable("select case when Inv_Model_LabelSize.Field1<>'' then   Inv_Model_LabelSize.Field3+cast(Inv_Model_LabelSize.width as varchar(30))+'X'+cast(Inv_Model_LabelSize.height as varchar(30))+' (mm)- '+cast(Inv_Model_LabelSize.gap as varchar(30))+' (gap)- '+Inv_Model_LabelSize.Field1 else Inv_Model_LabelSize.Field3+cast(Inv_Model_LabelSize.width as varchar(30))+'X'+cast(Inv_Model_LabelSize.height as varchar(30))+' (mm)- '+cast(Inv_Model_LabelSize.gap as varchar(30))+' (gap)' end as LabelSize,Inv_Model_LabelSize.Trans_Id from dbo.Inv_Label_Filter  inner join Inv_BOM on Inv_Label_Filter.Child_Id = inv_bom.TransID  inner join Inv_Model_LabelSize on  Inv_Model_LabelSize.Trans_Id = Inv_Label_Filter.Parent_Id where Inv_Label_Filter.Child_Id=" + ddlCore.SelectedValue.Trim() + "");
        ddllabelSize.DataSource = dt;
        ddllabelSize.DataTextField = "LabelSize";
        ddllabelSize.DataValueField = "Trans_Id";
        ddllabelSize.DataBind();
        ddllabelSize.Items.Insert(0, new ListItem("--Select--", "0"));


        dt = objda.return_DataTable("select Inv_BOM.transid,Inv_BOM.optiondescription from dbo.Inv_Label_Filter  inner join Inv_BOM on Inv_Label_Filter.Child_Id = inv_bom.TransID   where Inv_Label_Filter.parent_Id=" + ddlCore.SelectedValue.Trim() + "");
        ddllabelqty.DataSource = dt;
        ddllabelqty.DataTextField = "optiondescription";
        ddllabelqty.DataValueField = "transid";
        ddllabelqty.DataBind();
        ddllabelqty.Items.Insert(0, new ListItem("--Select--", "0"));


    }

    protected void ddllabelSize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    public void FillCurrency()
    {
        try
        {
            DataTable dt = ObjCurrencyMaster.GetCurrencyMaster();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");

        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;

        }
        if (ddlCurrency.Items.Count > 0)
        {
            try
            {
                ddlCurrency.SelectedValue = Session["CurrencyId"].ToString();

            }
            catch
            {
            }
        }
    }

    protected void ddllabelqty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strsql = "select distinct Field2 as CoreCategoryId,QtyCategoryId,Field1 as PackingCategoryId,QtyCategoryId,Field3 as PerforationCategoryId,MaterialCategoryId,ColorCategoryId from dbo.Inv_ProductOptionCategoryDetail";
        DataTable dtDetail = objda.return_DataTable(strsql);
        if (dtDetail.Rows.Count > 0)
        {
            ddlpacking.Items.Clear();
            DataTable dt = objda.return_DataTable("select Inv_BOM.transid,Inv_BOM.optiondescription from dbo.Inv_Label_Filter  inner join Inv_BOM on Inv_Label_Filter.Child_Id = inv_bom.TransID   where Inv_Label_Filter.parent_Id=" + ddllabelqty.SelectedValue.Trim() + " and inv_bom.optioncategoryId=" + dtDetail.Rows[0]["PackingCategoryId"].ToString() + "");
            ddlpacking.DataSource = dt;
            ddlpacking.DataTextField = "optiondescription";
            ddlpacking.DataValueField = "transid";
            ddlpacking.DataBind();
            ddlpacking.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }

    protected void ddlpacking_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnshowpart_Click(object sender, EventArgs e)
    {
        Session["dtLabelpartList"] = null;

        lblsearchenginerecord.Text = Resources.Attendance.Total_Records + " : 0";
        if (ddlcategory.SelectedIndex == 0)
        {
            DisplayMessage("select Model category");
            ddlcategory.Focus();
            return;
        }


        if (ddlModelNo.SelectedIndex == 0)
        {
            DisplayMessage("select Model No");
            ddlModelNo.Focus();
            return;
        }


        if (ddlPrinter.SelectedIndex == 0)
        {
            DisplayMessage("select printer");
            ddlPrinter.Focus();
            return;
        }



        if (ddlCore.SelectedIndex == 0)
        {
            DisplayMessage("select core");
            ddlCore.Focus();
            return;
        }


        if (ddllabelSize.SelectedIndex == 0)
        {
            DisplayMessage("select Label Size");
            ddlCore.Focus();
            return;
        }




        DataTable dtTemp = new DataTable();

        DataTable dtBOM = objda.return_DataTable("select * from inv_bom where inv_bom.ModelId =" + ddlModelNo.SelectedValue + "");

        double ModelSalesprice = 0;
        double ModelCostprice = 0;
        double ExchangeRate = 1;
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ddlModelNo.SelectedValue.Trim(), "True");
        if (dtModel.Rows.Count > 0)
        {
            double.TryParse(dtModel.Rows[0]["Field1"].ToString(), out ModelSalesprice);
            double.TryParse(dtModel.Rows[0]["Cost_Price"].ToString(), out ModelCostprice);
            if (ddlCurrency.SelectedValue != dtModel.Rows[0]["Field4"].ToString())
            {
                double.TryParse(SystemParameter.GetExchageRate(dtModel.Rows[0]["Field4"].ToString(), ddlCurrency.SelectedValue.Trim(), Session["DBConnection"].ToString()), out ExchangeRate);
            }

            if (rbtnmodelPrice.Checked)
            {
                ModelSalesprice = ModelSalesprice * ExchangeRate;
            }
            ModelCostprice = ModelCostprice * ExchangeRate;

        }
        string strMaterialCategoryId = string.Empty;
        string strcorecategoryid = string.Empty;
        string strqtycategoryid = string.Empty;
        string strPackingcategoryId = string.Empty;
        string strcolocorcategoryid = string.Empty;
        string strsql = "select distinct Field2 as CoreCategoryId,QtyCategoryId,Field1 as PackingCategoryId,QtyCategoryId,Field3 as PerforationCategoryId,MaterialCategoryId,ColorCategoryId from dbo.Inv_ProductOptionCategoryDetail";
        DataTable dtDetail = objda.return_DataTable(strsql);
        if (dtDetail.Rows.Count > 0)
        {
            strMaterialCategoryId = dtDetail.Rows[0]["MaterialCategoryId"].ToString();
            strcorecategoryid = dtDetail.Rows[0]["CoreCategoryId"].ToString();
            strqtycategoryid = dtDetail.Rows[0]["QtyCategoryId"].ToString();
            strPackingcategoryId = dtDetail.Rows[0]["PackingCategoryId"].ToString();
            strcolocorcategoryid = dtDetail.Rows[0]["ColorCategoryId"].ToString();
        }


        DataTable dt = new DataTable();
        dt.Columns.Add("Category");
        dt.Columns.Add("Model");
        dt.Columns.Add("Printer");
        dt.Columns.Add("Core");
        dt.Columns.Add("Size");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Packing");
        dt.Columns.Add("partNo");
        dt.Columns.Add("PartDescription");
        dt.Columns.Add("ItemCreated");
        dt.Columns.Add("stock", typeof(float));
        dt.Columns.Add("Currency");
        if ((bool)Session["IsShowCostPrice"] == true)
        {
            dt.Columns.Add("CostPrice");
        }
        dt.Columns.Add("Discount");
        dt.Columns.Add("SalesPrice");
        string strpart = ddlModelNo.SelectedItem.Text + "-";
        string strDescritption = GetModelDescriptionByModelNo(ddlModelNo.SelectedItem.Text.Trim());
        //for get material option id
        strpart = strpart + new DataView(dtBOM, "OptionCategoryID=" + strMaterialCategoryId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["OptionID"].ToString();
        strDescritption = strDescritption + "," + new DataView(dtBOM, "OptionCategoryID=" + strMaterialCategoryId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["OptionDescription"].ToString();

        //for label size
        strpart = strpart + "0";
        strDescritption = strDescritption + "," + ddllabelSize.SelectedItem.Text.Trim();

        //for get core option id
        strpart = strpart + new DataView(dtBOM, "TransID=" + ddlCore.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["OptionID"].ToString();
        strDescritption = strDescritption + "," + ddlCore.SelectedItem.Text;
        DataTable dtPartdetail = new DataTable();

        if (ddllabelqty.SelectedIndex > 0 && ddlpacking.SelectedIndex > 0)
        {
            dtPartdetail = objda.return_DataTable("select * from (select quantity as lblqty,optiondescription as lblqtydesc,OptionID as lblqtyoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strqtycategoryid + " and TransID=" + ddllabelqty.SelectedValue + ")label_qty cross join (select UnitPrice as PackingPrice ,quantity as Packingqty,optiondescription as packingdesc, OptionID as Packingoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = '" + strPackingcategoryId + "' and TransID=" + ddlpacking.SelectedValue + ")packing cross join (select optiondescription as lblformatdesc,OptionID as lblformatoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = 68)label_format cross join (select optiondescription as lblcolordesc,OptionID as lblcoloroption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strcolocorcategoryid + ")lblcolor");
        }
        else if (ddllabelqty.SelectedIndex > 0 && ddlpacking.SelectedIndex == 0)
        {
            dtPartdetail = objda.return_DataTable("select * from (select quantity as lblqty,optiondescription as lblqtydesc,OptionID as lblqtyoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strqtycategoryid + " and TransID=" + ddllabelqty.SelectedValue + ")label_qty cross join (select UnitPrice as PackingPrice ,quantity as Packingqty,optiondescription as packingdesc, OptionID as Packingoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = '" + strPackingcategoryId + "')packing cross join (select optiondescription as lblformatdesc,OptionID as lblformatoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = 68)label_format cross join (select optiondescription as lblcolordesc,OptionID as lblcoloroption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strcolocorcategoryid + ")lblcolor");
        }
        else
        {
            dtPartdetail = objda.return_DataTable("select * from (select quantity as lblqty,optiondescription as lblqtydesc,OptionID as lblqtyoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strqtycategoryid + ")label_qty cross join (select UnitPrice as PackingPrice ,quantity as Packingqty,optiondescription as packingdesc, OptionID as Packingoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = '" + strPackingcategoryId + "')packing cross join (select optiondescription as lblformatdesc,OptionID as lblformatoption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = 68)label_format cross join (select optiondescription as lblcolordesc,OptionID as lblcoloroption from inv_bom where ModelId ='" + ddlModelNo.SelectedValue + "' and optioncategoryid = " + strcolocorcategoryid + ")lblcolor");
        }

        DataTable dtModelSize = objda.return_DataTable("select Field2,Width,Height from inv_model_labelsize where Trans_id=" + ddllabelSize.SelectedValue.Trim() + "");
        string stroption = dtModelSize.Rows[0]["Field2"].ToString();

        foreach (DataRow dr in dtPartdetail.Rows)
        {
            DataRow dr1 = dt.NewRow();
            dr1["Category"] = ddlcategory.SelectedItem.Text.Trim();
            dr1["Model"] = ddlModelNo.SelectedItem.Text.Trim();
            dr1["Printer"] = ddlPrinter.SelectedItem.Text.Trim();
            dr1["Core"] = ddlCore.SelectedItem.Text.Trim();
            dr1["Size"] = ddllabelSize.SelectedItem.Text.Trim();
            dr1["Quantity"] = dr["lblqtydesc"].ToString().Trim();
            dr1["Packing"] = dr["packingdesc"].ToString().Trim();
            dr1["partNo"] = strpart + dr["lblqtyoption"].ToString() + dr["Packingoption"].ToString() + dr["lblformatoption"].ToString() + dr["lblcoloroption"].ToString() + "#" + stroption;
            dr1["PartDescription"] = strDescritption + "," + dr["lblqtydesc"].ToString() + "," + dr["packingdesc"].ToString() + "," + dr["lblformatdesc"].ToString() + "," + dr["lblcolordesc"].ToString();

            if (ObjInvProductMaster.GetProductIdbyProductCode(dr1["partNo"].ToString(), HttpContext.Current.Session["BrandId"].ToString()) == "0")
            {
                dr1["ItemCreated"] = "No";
            }
            else
            {
                dr1["ItemCreated"] = "Yes";
            }

            try
            {
                dr1["stock"] = Common.GetAmountDecimal(objStock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ObjInvProductMaster.GetProductIdbyProductCode(dr1["partNo"].ToString(), HttpContext.Current.Session["BrandId"].ToString())).Rows[0]["Quantity"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            }
            catch
            {
                dr1["stock"] = "0";
            }
            dr1["Currency"] = ddlCurrency.SelectedItem.Text.Trim();
            if (rbtnPackingPrice.Checked)
            {
                double.TryParse(dr["PackingPrice"].ToString(), out ModelSalesprice);
                ModelSalesprice = ModelSalesprice * ExchangeRate;
            }
            string[] strPrice = getLabelPrice(dtModelSize.Rows[0]["Height"].ToString(), dtModelSize.Rows[0]["Width"].ToString(), dr["lblqty"].ToString(), dr["Packingqty"].ToString(), ModelSalesprice, ModelCostprice);
            if ((bool)Session["IsShowCostPrice"] == true)
            {
                dr1["CostPrice"] = strPrice[1].ToString();
            }

            dr1["Discount"] = txtDiscount.Text == "" ? "0" : txtDiscount.Text;
            dr1["SalesPrice"] = strPrice[0].ToString();
            dt.Rows.Add(dr1);
        }
        gvPartNo.DataSource = dt;
        gvPartNo.DataBind();
        lblsearchenginerecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
        Session["dtLabelpartList"] = dt;

    }

    public string GetModelDescriptionByModelNo(string strModelNo)
    {
        DataTable dt = objda.return_DataTable("select Field3 from inv_modelmaster where model_no ='" + strModelNo + "'");
        return dt.Rows[0]["Field3"].ToString();
    }

    public string[] getLabelPrice(string strHeight, string strWidth, string strquantity, string strpackingquantity, double salesprice, double costprice)
    {
        string[] strprice = new string[2];
        strprice[0] = "0";
        strprice[1] = "0";
        double lblsizewidth = 0;
        double lblsizeheight = 0;
        double lblpacking = 0;
        double lblquantity = 0;
        double lblDiscount = 0;
        double lblSalesPrice = 0;
        double lblCostPrice = 0;

        double.TryParse(strWidth, out lblsizewidth);
        double.TryParse(strHeight, out lblsizeheight);
        double.TryParse(txtDiscount.Text, out lblDiscount);
        double.TryParse(strquantity, out lblquantity);
        double.TryParse(strpackingquantity, out lblpacking);
        //lblSalesPrice = (lblsizewidth + 3 * lblsizeheight + 3);
        //lblSalesPrice = lblSalesPrice / 921600;
        //lblSalesPrice = lblSalesPrice * lblquantity*lblpacking;
        //lblSalesPrice = lblSalesPrice * salesprice;
        lblSalesPrice = ((((lblsizewidth + 3) * (lblsizeheight + 3)) / 921600) * lblquantity * lblpacking) * salesprice;
        lblCostPrice = ((((lblsizewidth + 3) * (lblsizeheight + 3)) / 921600) * lblquantity * lblpacking) * costprice;
        if (lblDiscount > 0)
        {
            lblSalesPrice = lblSalesPrice - (((lblSalesPrice * lblDiscount)) / 100);
        }
        strprice[0] = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, lblSalesPrice.ToString());
        strprice[1] = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, lblCostPrice.ToString());



        return strprice;
    }


    protected void btnexportexcel_Click(object sender, EventArgs e)
    {
        if (gvPartNo.Rows.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtLabelpartList"];
            if (chkItemCreated.Checked)
            {
                dt = new DataView(dt, "ItemCreated='Yes'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (chkIsStock.Checked)
            {
                dt = new DataView(dt, "stock>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            ExportTableData(dt);
        }
        else
        {
            DisplayMessage("Record not found");
        }
    }



    protected void chkItemCreated_CheckedChanged(object sender, EventArgs e)
    {
        if (Session["dtLabelpartList"] != null)
        {
            DataTable dt = (DataTable)Session["dtLabelpartList"];
            if (chkItemCreated.Checked)
            {
                dt = new DataView(dt, "ItemCreated='Yes'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (chkIsStock.Checked)
            {
                dt = new DataView(dt, "stock>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            gvPartNo.DataSource = dt;
            gvPartNo.DataBind();
            lblsearchenginerecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvPartNo.DataSource = null;
            gvPartNo.DataBind();
            lblsearchenginerecord.Text = Resources.Attendance.Total_Records + " : 0";
        }
    }

    #region FilterConfiguration
    protected void btnsave_Config_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());





        string strmodelId = string.Empty;
        string strIndustryId = string.Empty;
        string strApplicationTypeId = string.Empty;

        if (ddlMaterial.SelectedIndex == 0)
        {
            DisplayMessage("Please select Material");
            ddlMaterial.Focus();
            return;

        }

        if (txtConfigModel.Text.Trim() == "")
        {
            DisplayMessage("Please Enter Model No");
            txtConfigModel.Focus();
            return;

        }
        else
        {
            DataTable dtModel = objda.return_DataTable("select model_no,Trans_Id from inv_modelmaster where model_no='" + txtConfigModel.Text.Trim() + "' and isactive='True'");
            if (dtModel.Rows.Count == 0)
            {
                DisplayMessage("Model no is invalid");
                txtConfigModel.Focus();
                return;
            }
            else
            {
                strmodelId = dtModel.Rows[0]["Trans_id"].ToString();
            }
        }



        if (txtIndustry.Text.Trim() == "")
        {
            DisplayMessage("Please Enter Industy Name");
            txtIndustry.Focus();
            return;
        }
        else
        {
            DataTable dtCategory = objda.return_DataTable("select Category_Id from inv_product_categorymaster where isactive = 'True' and Category_Name = '" + txtIndustry.Text.Trim() + "'");
            if (dtCategory.Rows.Count == 0)
            {
                DisplayMessage("Industy Name is invalid");
                txtIndustry.Focus();
                return;
            }
            else
            {
                strIndustryId = dtCategory.Rows[0]["Category_Id"].ToString();
            }
        }




        if (txtApplicationType.Text.Trim() == "")
        {
            DisplayMessage("Please Enter Application Type");
            txtApplicationType.Focus();
            return;
        }
        else
        {
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();

            try
            {
                objda.execute_Command("delete from Inv_Model_CategoryConfig where Material_Id='" + ddlMaterial.SelectedValue.Trim() + "' and Industry_Id=" + strIndustryId + " and Model_Id=" + strmodelId + "", ref trns);


                foreach (string str in txtApplicationType.Text.Trim().Split(';'))
                {
                    if (str == "")
                    {
                        continue;
                    }

                    DataTable dtCategory = objda.return_DataTable("select Category_Id from inv_product_categorymaster where isactive = 'True' and Category_Name = '" + str + "'", ref trns);
                    if (dtCategory.Rows.Count == 0)
                    {
                        DisplayMessage("Application Name('" + str + "') is invalid");
                        txtApplicationType.Focus();
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {

                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    else
                    {
                        strApplicationTypeId = dtCategory.Rows[0]["Category_Id"].ToString();
                        objda.execute_Command("INSERT INTO [Inv_Model_CategoryConfig]  ([Material_Id]           ,[Industry_Id]           ,[Application_Type_Id]           ,[Model_Id])     VALUES           (" + ddlMaterial.SelectedValue.Trim() + "," + strIndustryId + "," + strApplicationTypeId + "," + strmodelId + ")", ref trns);
                    }
                }

                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();

                DisplayMessage("Record Saved Successfully");
                FillGridView_Filter();
            }
            catch (Exception ex)
            {

                DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {

                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;

            }
        }


    }

    public void FillGridView_Filter()
    {
        DataTable dt = objda.return_DataTable("select Inv_Model_CategoryConfig.Trans_Id,Material.Category_Name as Material_Name,inv_modelmaster.Model_No,Industry.Category_Name as Industry_Name,ApplicationTB.Category_Name as Application_Name from  Inv_Model_CategoryConfig inner join inv_product_categorymaster as Material on Inv_Model_CategoryConfig.material_id = Material.category_id inner join inv_product_categorymaster as Industry on Inv_Model_CategoryConfig.Industry_id = Industry.category_id inner join inv_product_categorymaster as ApplicationTB on Inv_Model_CategoryConfig.Application_type_id = ApplicationTB.category_id inner join inv_modelmaster on Inv_Model_CategoryConfig.model_id = inv_modelmaster.Trans_Id where Inv_Model_CategoryConfig.Material_Id=" + ddlMaterial.SelectedValue + "");
        GvConfigFilter.DataSource = dt;
        GvConfigFilter.DataBind();
    }

    public void FillMaterial()
    {
        DataTable dtCategory = objda.return_DataTable("select distinct Inv_Product_CategoryMaster.Category_Name,Inv_Product_CategoryMaster.Category_Id   from    inv_model_category   inner join Inv_ModelMaster   on   inv_model_category.ModelId =Inv_ModelMaster.Trans_Id inner join Inv_Product_CategoryMaster on inv_model_category.categoryid= Inv_Product_CategoryMaster.Category_Id    where Inv_ModelMaster.islabel='True' order by Inv_Product_CategoryMaster.Category_Name");
        ddlMaterial.DataSource = dtCategory;
        ddlMaterial.DataTextField = "category_name";
        ddlMaterial.DataValueField = "category_id";
        ddlMaterial.DataBind();
        ddlMaterial.Items.Insert(0, new ListItem("--Select--", "0"));

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListIndustry(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster ObjInvProductCategory = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvProductCategory.GetProductCategoryTrueAllData(HttpContext.Current.Session["CompId"].ToString());
        DataTable dtTemp = dt.Copy();



        dt = new DataView(dt, "Category_Name like '%" + prefixText.ToString() + "%'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            //dt = dtTemp.Copy();

        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category_Name"].ToString();
        }


        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListApplicationType(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster ObjInvProductCategory = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvProductCategory.GetProductCategoryTrueAllData(HttpContext.Current.Session["CompId"].ToString());
        DataTable dtTemp = dt.Copy();



        dt = new DataView(dt, "Category_Name like '%" + prefixText.ToString() + "%'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            //dt = dtTemp.Copy();

        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category_Name"].ToString() + ";";
        }


        return txt;
    }

    protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGridView_Filter();
    }
    #endregion







}
